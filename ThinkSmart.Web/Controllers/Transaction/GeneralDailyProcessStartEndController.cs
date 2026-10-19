using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class GeneralDailyProcessStartEndController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly IGeneralDailyProcessService _generalDailyProcessService;
        public GeneralDailyProcessStartEndController(IViewRenderService viewRenderService, IGeneralDailyProcessService generalDailyProcessService)
        {
            _viewRenderService = viewRenderService;
            _generalDailyProcessService = generalDailyProcessService;
        }
        public IActionResult GeneralDailyProcessStartEnd()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult GetUnitType()
        {
            var result = _generalDailyProcessService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDecision()
        {
            var result = _generalDailyProcessService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
