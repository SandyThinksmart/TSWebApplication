using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class RollForcastPopupController :ControllerBase
    {
        private readonly IRollForcastPopupServices _rollForcastPopupServices;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public RollForcastPopupController(IRollForcastPopupServices rollForcastPopupServices, LocalizationServices localization, IAllCommonService commonService)
        {
            _rollForcastPopupServices = rollForcastPopupServices;
            _localization = localization;
            _allCommonService = commonService;

        }

        /// <summary>
        /// Get One Month Product Forecast Details.
        /// </summary>
        /// <returns>Created by shobhana[JAN 2025]
        /// </returns>
        [HttpPost("GetOneMonthProductForecastDetails")]
        public IActionResult GetOneMonthProductForecastDetails(string FcSummaryNo,int Roll, int Year)
        {
            bool isSuccess = false;
            var result = _rollForcastPopupServices.GetOneMonthProductForecastDetails(FcSummaryNo, Roll, Year);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        /// <summary>
        /// Get One Month Material Forecast Details.
        /// </summary>
        /// <returns>Created by shobhana[16 JAN 2025]
        /// </returns>
        [HttpPost("GetOneMonthMaterialForecastDetails")]
        public IActionResult GetOneMonthMaterialForecastDetails(string FcSummaryNo)
        {
            bool isSuccess = false;
            var result = _rollForcastPopupServices.GetOneMonthMaterialForecastDetails(FcSummaryNo);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

    }
}
