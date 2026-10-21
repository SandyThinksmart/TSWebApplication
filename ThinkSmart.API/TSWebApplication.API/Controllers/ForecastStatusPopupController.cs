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
    public class ForecastStatusPopupController : ControllerBase
    {
        private readonly IForecastStatusPopupService _forecastStatusPopupService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public ForecastStatusPopupController(IForecastStatusPopupService forecastStatusPopupService, LocalizationServices localization, IAllCommonService commonService)
        {
            _forecastStatusPopupService = forecastStatusPopupService;
            _localization = localization;
            _allCommonService = commonService;

        }
        [HttpPost("GetSalesForecastStatusData")]
        public IActionResult GetSalesForecastData()
        {
            bool isSuccess = false;
            var result = _forecastStatusPopupService.GetSalesForecastDataList();
            //if (result.Count > 0)
            //{
            //    isSuccess = true;
            //}
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
    }
}
