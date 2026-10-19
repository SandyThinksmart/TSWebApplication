using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Data;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CSVGenerationController : Controller
    {
        private readonly ICSVGenerationService _csvService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;


        public CSVGenerationController(ICSVGenerationService csvGenerationService, LocalizationServices localization, IAllCommonService commonService)
        {

            _localization = localization;
            _csvService = csvGenerationService;
            _commonService = commonService;

        }

        [HttpGet("GetGroupTitle")]
        public IActionResult GetGroupTitle()
        {
            var result = _csvService.GetCSVGroupTitle();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetCSVTitleList")]
        public IActionResult GetCSVTitleList(string groupTitleName)
        {
            var result =_csvService.GetCSVOutputTitleList(groupTitleName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetCSVConditionsList")]
        public IActionResult GetCSVConditionsList(int outputCD)
        {
            var result = _csvService.GetConditionsList(outputCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetLocationList")]
        public IActionResult GetLocationList()
        {
            var result = _csvService.GetLocationNameList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetNextProcessList")]
        public IActionResult GetNextProcessList()
        {
            var result = _csvService.GetNextProcessList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetLocationCDByUserID")]
        public IActionResult GetLocationCDByUserID(int userID)
        {
          short locationCD =  Convert.ToInt16(_commonService.GetUserLocation(userID));
            return Ok(new { IsSuccess = true, message = locationCD });
        }
    }
}
