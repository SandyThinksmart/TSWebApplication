using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{

    public class GeneralDailyProcessEntryController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        public GeneralDailyProcessEntryController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }
        public IActionResult GeneralDailyProcessEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
    }
}
