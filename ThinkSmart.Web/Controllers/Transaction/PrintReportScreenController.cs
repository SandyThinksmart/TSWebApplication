using Microsoft.AspNetCore.Mvc;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class PrintReportScreenController : Controller
    {
        public IActionResult PrintReportScreen()
        {
            return View();
        }
    }
}
