# EDD-PointerApi

## Description
This is a simple unambitious postcode address lookup Api for Northern Ireland addresses.

e.g. from a consuming application the user adds a postcode

![lookup](https://user-images.githubusercontent.com/66303816/121494861-05f19b80-c9d1-11eb-8656-414decdb57ee.png)

And the api displays the results

![lookupList](https://user-images.githubusercontent.com/66303816/121495064-333e4980-c9d1-11eb-9a69-14120ea3de8e.png)


## Contents of this file

- [Contributing](#contributing)
- [Licensing](#licensing)
- [Project Documentation](#project-documentation)
    - [Why did we build this project](#why-did-we-build-this-project)
    - [What problem was it solving](#what-problem-was-it-solving)
    - [How did we do it](#how-did-we-do-it)
    - [Future Plans](#future-plans)
    - [Deployment Guide](#deployment-guide)

## Contributing

Contributions are welcomed! Read the [Contributing Guide](./docs/contributing/Index.md) for more information.

## Licensing

Unless stated otherwise, the codebase is released under the MIT License. This covers both the codebase and any sample code in the documentation. The documentation is © Crown copyright and available under the terms of the Open Government 3.0 licence.

## Project Documentation

### Why did we build this project?

We built this so applications can allow users to enter their postcode and recieve a list of addresses, in order to select their address. We built this api so the data and functionality can be shared by many applications.

### What problem was it solving?

This solves having to create a pointer table in every single application and adding the same code over and over again. There are 3 main endpoints:

- Search by postcode
- Search by postcode and premises number
- Search by x and y co-ordinates which is handy for plotting a point on a map for example

### How did we do it?

This is a dotnet core application which uses Mysql to store the pointer data, Entity Framework for data access and JWT to authenticate applications to allow them to use the api.

### Future plans

We may introduce a more advanced search if needed.

### Deployment guide

To run the databases you need mysql installed. Then run the below commands to set up the database:

- update-database

Restore the nuget package. Then to build run "dotnet build" in command line then dotnet run to run the site.

### Dataset

You can obtain the dataset which is around one million addresses from OSNI / LPS in csv format and manually input this into the database.

### Usage from consuming application

The consuming application will need a secret key and the api base address which they can obtain from DoF EDD.
To acutally use the api from your application you will need a view (I did this as a partial view), a pointer model, an address model / interface, a javascript file to interact with the view and a controller to execute the search.
Below are examples of how I did it:

Javascript for view

```
$('#SearchPostCode').on('keyup keypress', function (e)
{
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13)
    {
        getAddresses();
        e.preventDefault();
        return false;
    }
});

function getAddresses()
{
    let postCode = $("#SearchPostCode").val();
    $("#addressError").hide();

    if (postCode != "")
    {
        $("#loadSpinner").show();
       
        $.get('/Pointer/GetAddresses/', { postCode: postCode }, function (data) {
            $("#SearchAddress").empty();
            $("#SearchAddress").append($("<option value=''>Select Address</option>"));

            $.each(data, function ()
            {
                $(".govuk-error-summary").hide();
                $("#loadSpinner").hide();
                $("#SearchAddressList").show();
                $("#addressError").hide();
                $("#SearchAddress").append($("<option></option>").val(this["building_Number"]).html(this["building_Number"] + ' ' + this["primary_Thorfare"] + ',' + this["town"] + ',' + this["postcode"]));
            });
        }).fail(function ()
        {
            $(".govuk-error-summary").show();

            if ($(".error-items").length === 0)
            {
                $(".govuk-error-summary__list").append("<li><a class='error-items' href='#SearchPostCode'>Not a real postcode. Address could not be found.</a></li>");
            }

            $("#PostCodeSearchComponent").addClass("govuk-form-group--error");
            $("#SearchPostCode").addClass("govuk-input--error");
            $("#SearchPostCode").val("Not a postcode")
            $("#addressError").show();
            $("#loadSpinner").hide();
            $("#SearchAddressList").hide();
        });
    }
}

function fillAddressTextBoxes() {
    let myText = $("#SearchAddress :selected").text();

    if (myText != "Select Address")
    {
        let addressArray = myText.split(',');

        $("#Address1").val("");
        $("#Address2").val("");
        $("#Address3").val("");
        $("#TownCity").val("");
        $("#PostCode").val("");

        $("#Address1").val(addressArray[0]);
        $("#TownCity").val(addressArray[1]);
        $("#PostCode").val(addressArray[2]);
    }
}
```

Controller to manage search

```
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using probate.Config;
using probate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace probate.Controllers
{
    public class PointerController : Controller
    {
        private readonly IHttpClientFactory _pointerClient;
        private readonly IOptions<PointerConfig> _pointerConfig;

        public PointerController(IHttpClientFactory pointerClient, IOptions<PointerConfig> pointerConfig)
        {
            _pointerClient = pointerClient;
            _pointerConfig = pointerConfig;
        }

        [HttpGet]
        public async Task<JsonResult> GetAddressesAsync(string postCode)
        {
            var client = _pointerClient.CreateClient("PointerClient");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + CreateJwtToken());

            var result = await client.GetAsync("PostCodeSearch/" + postCode);

            List<Pointer> pointerAddresses = new List<Pointer>();

            if (result.IsSuccessStatusCode)
            {
                using (HttpContent content = result.Content)
                {
                    var resp = content.ReadAsStringAsync();
                    pointerAddresses = JsonConvert.DeserializeObject<IEnumerable<Pointer>>(resp.Result).ToList();
                }
            }

            return Json(pointerAddresses);
        }

        private string CreateJwtToken()
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var iat = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                { "iat", iat },
                { "kid", _pointerConfig.Value.kid }
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var jwtToken = encoder.Encode(payload, _pointerConfig.Value.secret);
            return jwtToken;
        }
    }
}
```

Pointer model

```
    public class Pointer
    {
        public string Organisation_Name { get; set; }
        public string Sub_Building_Name { get; set; }
        public string Building_Name { get; set; }
        public string Building_Number { get; set; }
        public string Primary_Thorfare { get; set; }
        public string Alt_Thorfare_Name1 { get; set; }
        public string Secondary_Thorfare { get; set; }
        public string Locality { get; set; }
        public string Townland { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string BLPU { get; set; }
        public int Unique_Building_ID { get; set; }
        public int UPRN { get; set; }
        public int USRN { get; set; }
        public string Local_Council { get; set; }
        public int X_COR { get; set; }
        public int Y_COR { get; set; }
        public string Temp_Coords { get; set; }
        public string Building_Status { get; set; }
        public string Address_Status { get; set; }
        public string Classification { get; set; }
        public string Creation_Date { get; set; }
        public string Commencement_Date { get; set; }
        public string Archived_Date { get; set; }
        public string Action { get; set; }
        public string UDPRN { get; set; }
        public string Posttown { get; set; }
    }
    
   ```
  
This is my partial view which is reused in our applications
    
```
@model probate.Models.IAddress

<div class="govuk-form-group">
    <div class="govuk-hint">
        To find your address, enter a valid Northern Ireland postcode and select find address.
    </div>

    <div class="govuk-form-group" id="PostCodeSearchComponent">
        <label class="govuk-label" for="SearchPostCode">
            Postcode
        </label>
        <span id="addressError" class="govuk-error-message" style="display:none;">
            <span class="govuk-visually-hidden">Error:</span> Enter a real postcode
        </span>

        <input class="govuk-input govuk-input--width-10" asp-for="SearchPostCode" type="text" autocomplete="chrome-off">

        <button class="govuk-button govuk-button--secondary" type="button" data-module="govuk-button" id="btnSearch" onclick="getAddresses();">
            Find address
        </button>
       <div id="loadSpinner" class="govuk-box-highlight" style="display:none;" role="status">
          <span class="spinner-border"></span>
          Loading, please wait
       </div>
    </div>
 </div>
    <div class="govuk-form-group" id="SearchAddressList" style="display:none;">
        <label class="govuk-label" asp-for="SearchAddress">
            Select an address
        </label>
        <select class="govuk-select" asp-for="SearchAddress" onchange="fillAddressTextBoxes();">
        </select>
    </div>
    <div class="govuk-hint">
        If you cannot find your address, enter your details below.
    </div>
    <div class="govuk-form-group">
        <label asp-for="Address1" class="govuk-label"></label>
        <span asp-validation-for="Address1" class="govuk-error-message"></span>
        <input class="govuk-input govuk-!-width-two-thirds" type="text" asp-for="Address1" autocomplete="address-line1" />
    </div>
    <div class="govuk-form-group">
        <label asp-for="Address2" class="govuk-label">Address Line 2 (optional)</label>
        <input class="govuk-input govuk-!-width-two-thirds" type="text" asp-for="Address2" value="@Model.Address2" autocomplete="address-line2" />
    </div>
    <div class="govuk-form-group">
        <label asp-for="Address3" class="govuk-label">Address Line 3 (optional)</label>
        <input class="govuk-input govuk-!-width-two-thirds" type="text" asp-for="Address3" autocomplete="address-line3" />
    </div>
    <div class="govuk-form-group">
        <label asp-for="TownCity" class="govuk-label">Town or city</label>
        <span asp-validation-for="TownCity" class="govuk-error-message"></span>
        <input class="govuk-input govuk-!-width-two-thirds" type="text" asp-for="TownCity" value="@Model.TownCity" autocomplete="address-level2" />
    </div>
    <div class="govuk-form-group">
        <label asp-for="PostCode" class="govuk-label">Postcode</label>
        <span asp-validation-for="PostCode" class="govuk-error-message"></span>
        <input class="govuk-input govuk-!-width-two-thirds" type="text" asp-for="PostCode" value="@Model.PostCode" autocomplete="postal-code" />
    </div>
    <div class="govuk-form-group">
        <label asp-for="Country" class="govuk-label">Country (optional)</label>
        <input class="govuk-input govuk-!-width-two-thirds" type="text" asp-for="Country" value="@Model.Country" autocomplete="country" />
    </div>
   
  ```
    
 IAddress interface used to capture the address
    
 ```
    public interface IAddress
    {
        public string SearchAddress { get; set; }
        public string SearchPostCode { get; set; }

        [DisplayName("Address line 1")]
        [Required(ErrorMessage = "Enter address line 1")]
        [StringLength(35, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string Address1 { get; set; }

        [DisplayName("Address line 2")]
        [StringLength(35, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string Address2 { get; set; }

        [DisplayName("Address line 3")]
        [StringLength(35, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string Address3 { get; set; }

        [DisplayName("Town or city")]
        [StringLength(35, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        [Required(ErrorMessage = "Enter town or city")]
        public string TownCity { get; set; }

        [DisplayName("Post code")]
        [Required(ErrorMessage = "Enter post code")]
        [StringLength(8, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string PostCode { get; set; }

        [DisplayName("Country")]
        [StringLength(35, ErrorMessage = "{0} must be a string with a maximum length of {1}")]
        public string Country { get; set; }
    }
    
```









