using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YieldSearchController : ControllerBase
    {
        private readonly IYieldSearchService _YieldSearchService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;

        public YieldSearchController(IYieldSearchService yieldSearchService, LocalizationServices localization, IAllCommonService commonService)
        {
            _localization = localization;
            _YieldSearchService = yieldSearchService;
            _commonService = commonService;
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            DateTimeDetails result = new DateTimeDetails();
            DateTime dateTime = _commonService.GetSystemDateTime();
            result.END_DATETIME_STRING = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            dateTime = dateTime.AddMonths(-1);

            result.START_DATETIME_STRING = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = result });
        }

        /// <summary>
        /// CREATED BY :Sunil Gokanwar on 2026/1/2  
        /// </summary>
        /// used screens: Yield Search
        /// <returns>yield data</returns>
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _YieldSearchService.GetYieldSearchList(request);
            //var details = _YieldSearchService.GetYieldSearchList(request);
            //List<ProcessPlanForYieldSearchDetails> result = _YieldSearchService.UpdateECYieldValue(details);
            return Ok(new { IsSuccess = true, message = result });
        }

        
        
    }
}
