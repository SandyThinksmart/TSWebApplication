using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DaidoFutoBatchController : ControllerBase
    {

       
        private readonly IDaidoFutoBatchService _daidoFutoBatchService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;

        public DaidoFutoBatchController(IDaidoFutoBatchService daidoFutoBatchService, IAllCommonService commonService, LocalizationServices localization)
        {          
            
            _commonService = commonService;
            _daidoFutoBatchService = daidoFutoBatchService;

        }
        [HttpPost("GetFutouList")]
        public IActionResult GetFutouList()
        {
            var result = _daidoFutoBatchService.GetFutouList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            string nextDayDate = dateTime.AddDays(1).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            return Ok(new { IsSuccess = true, message = new { SysDate = sysDate, NextDayDate = nextDayDate } });
        }


        [HttpPost("DaidoNyukoEDIBatchDataInsert")]
        public IActionResult DaidoNyukoEDIBatchDataInsert([FromBody] List<DaidoFutouNyukoDetails> details,string cultureName, int userID)
        {           
          var result = _daidoFutoBatchService.DaidoNyukoEDIBatchDataInsert(details,cultureName,userID);
            return Ok(new { IsSuccess = true, message = "Data Insert Successfully",data = result });
        }

        [HttpPost("GetDataCount")]
        public ActionResult GetDataCount(DateTime Importdate)
        {
            var result = _daidoFutoBatchService.GetDataCount(Importdate);
            return Ok(new { IsSuccess = true, message = result });
        }

       
    }
}
