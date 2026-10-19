using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class GeneralDailyProcessCancelController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        public GeneralDailyProcessCancelController(LocalizationServices localization, IViewRenderService viewRenderService)
        {
            _localization = localization;
            _viewRenderService = viewRenderService;
        }
        public IActionResult GeneralDailyProcessCancel()
        {
            return View();
        }

        //public IActionResult CheckValidationsOnGeneralDailyProcessCancel([FromBody] List<GeneralDailyProcessDetails> details)
        //{
        //    string view = _viewRenderService.RenderToString("GeneralDailyProcessCancel/GeneralDailyProcessGrid", details);
        //    return Json(new { IsSuccess = true, message = view });
        //}


        public IActionResult GeneralDailyProcessGrid([FromBody] List<GeneralDailyProcessDetails> details)
        {
            string view = _viewRenderService.RenderToString("GeneralDailyProcessCancel/GeneralDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

      
    }
}
