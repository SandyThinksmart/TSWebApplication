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
    public class ProcessDisplayConfigController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        public ProcessDisplayConfigController(LocalizationServices localization, IViewRenderService viewRenderService)
        {
            _localization = localization;
            _viewRenderService = viewRenderService;
        }
        public IActionResult ProcessDisplayConfiguration()
        {
           
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult GetProcessKubunDetails([FromBody] List<ProcessDisplayConfigDetails> details)
        {
            string view = _viewRenderService.RenderToString("ProcessDisplayConfig/DisplayProcessGridView", details);
            return Json(new { IsSuccess = true, message = view });
        }

    }
}
