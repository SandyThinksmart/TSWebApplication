using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
namespace TSWebApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CopyTagFromHostToOracleScreenController : ControllerBase
    {
        private readonly ICopyTagFromHostToOracleScreenService _copyTagFromHostToOracleScreenService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;

        public CopyTagFromHostToOracleScreenController(ICopyTagFromHostToOracleScreenService copyTagFromHostToOracleScreenService, IAllCommonService commonService, LocalizationServices localization)
        {
            _copyTagFromHostToOracleScreenService = copyTagFromHostToOracleScreenService;
            _localization = localization;
            _commonService = commonService;
        }
        [HttpPost("GetDetailsByNifuda")]
        public IActionResult GetDetailsByNifuda(List<GridSearchFields> nifudaNo)
        {
            //string message = string.Empty;
            //string enteredNifuda = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            //foreach (string _nifuda in nifudaNo)
            //{

            //    if (enteredNifuda == string.Empty)
            //    {
            //        enteredNifuda = "'" + _nifuda + "'";
            //    }
            //    else
            //    {
            //        enteredNifuda = enteredNifuda + "," + "'" + _nifuda + "'";
            //    }
            //}
             data = _copyTagFromHostToOracleScreenService.GetDetailsByNifuda(nifudaNo);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }
        [HttpPost("SaveNifudaData")]
        public IActionResult SaveNifudaData([FromBody] List<GridSearchFields> tagDetails)
        {
            //List<GridSearchFields> data = new List<GridSearchFields>();
           var data = _copyTagFromHostToOracleScreenService.SaveNifudaData(tagDetails);
            return Ok(new { IsSuccess = true, message = data });
        }

        [HttpPost("DeleteCopyTagFromHostToOracleData")]
        public IActionResult DeleteCopyTagFromHostToOracleData(List<GridSearchFields> tagDetails)

        {
            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _copyTagFromHostToOracleScreenService.DeleteCopyTagFromHostToOracleData(tagDetails);
            return Ok(new { IsSuccess = true, data = data });
        }
        [HttpPost("MappNifudaByBarcode")]
        public IActionResult MappNifudaByBarcode(string nifudaNo)
        {
            bool returnval = false;
            var result = _commonService.MappNifudaByBarcode(nifudaNo);

            if (result.NIFUDA_NO != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }
    }
}
