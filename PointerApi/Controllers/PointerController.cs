using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PointerApi.Data;
using PointerApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PointerController : ControllerBase
    {
        private readonly PointerContext _context;

        public PointerController(PointerContext context)
        {
            _context = context;
        }

        [HttpGet("PostCodeSearch/{postcode}", Name = "Get")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PointerModel>>> GetPointerByPostCode(string postcode)
        {
            var pointerModel = await _context.Pointer.Where(i => i.Postcode == FormatPostCode(postcode) && i.Building_Status == "BUILT" && i.Address_Status == "APPROVED").OrderBy(i => i.Building_Number.Length).ThenBy(i => i.Building_Number).ToListAsync();

            if (pointerModel.Count == 0)
            {
                return NoContent();
            }

            return Ok(pointerModel);
        }

        [HttpGet("PostCodeSearch/postcode={postcode}&buildingNumber={buildingNumber}")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PointerModel>>> GetPointerByPostCodeBuildingNumber(string postcode, string buildingNumber)
        {
            var pointerModel = await _context.Pointer.Where(i => i.Postcode == FormatPostCode(postcode) && i.Building_Number == buildingNumber.ToUpper() && i.Building_Status == "BUILT" && i.Address_Status == "APPROVED").OrderBy(i => i.Building_Number.Length).ThenBy(i => i.Building_Number).ToListAsync();

            if (pointerModel.Count == 0)
            {
                return NoContent();
            }

            return Ok(pointerModel);
        }

        [HttpGet("CoordinatesSearch/x_cor={xCor}&y_cor={yCor}")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PointerModel>>> GetPointerByCoordinates(int xCor, int yCor)
        {
            var pointerModel = await _context.Pointer.Where(i => i.X_COR == xCor && i.Y_COR == yCor && i.Building_Status == "BUILT" && i.Address_Status == "APPROVED").OrderBy(i => i.Building_Number.Length).ThenBy(i => i.Building_Number).ToListAsync();

            if (pointerModel.Count == 0)
            {
                return NoContent();
            }

            return Ok(pointerModel);
        }

        private string FormatPostCode(string postcode)
        {
            StringBuilder buildPostcode = new StringBuilder(postcode);

            buildPostcode.Replace("-", "");
            buildPostcode.Replace(" ", "");

            if (buildPostcode.Length == 6 || buildPostcode.Length == 7)
            {
                if (buildPostcode.Length == 6)
                {
                    buildPostcode.Insert(3, " ");
                }
                else
                {
                    buildPostcode.Insert(4, " ");
                }
            }

            return buildPostcode.ToString().ToUpper();
        }
    }
}