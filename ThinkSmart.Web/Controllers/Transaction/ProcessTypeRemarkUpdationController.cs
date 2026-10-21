using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ProcessTypeRemarkUpdationController : Controller
    {
        public IActionResult ProcessTypeRemarkUpdationScreen()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
    }
}
