using System;

namespace PointerApi.DTO
{
    public class PointerDto
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
        public DateTime Creation_Date { get; set; }
        public DateTime Commencement_Date { get; set; }
        public DateTime Archived_Date { get; set; }
        public string Action { get; set; }
        public string UDPRN { get; set; }
        public string Posttown { get; set; }
    }
}
