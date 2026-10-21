using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSWebApplication.Web.Controllers
{
    public class StraighteningEntryScreenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
