using Microsoft.AspNetCore.Mvc;

namespace TSWebApplication.Web.Controllers
{
    public class TensileEntryScreenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
