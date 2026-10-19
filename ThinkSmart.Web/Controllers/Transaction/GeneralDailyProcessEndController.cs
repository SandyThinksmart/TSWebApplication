using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class GeneralDailyProcessEndController : Controller
    {
        private readonly IGeneralDailyProcessService _generalDailyProcessService;
        public GeneralDailyProcessEndController(IGeneralDailyProcessService generalDailyProcessService)
        {
            _generalDailyProcessService = generalDailyProcessService;
        }
        public IActionResult GeneralDailyProcessEnd()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

       
        public IActionResult GetDecision()
        {
            var result = _generalDailyProcessService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetUnitType()
        {
            var result = _generalDailyProcessService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
