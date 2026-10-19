using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class CopyTagFromHostToOracleScreenController : Controller
    {
        public IActionResult CopyTagFromHostToOracleScreen()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
    }
}
