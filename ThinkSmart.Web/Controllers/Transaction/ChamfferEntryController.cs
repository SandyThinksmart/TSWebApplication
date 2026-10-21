using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ChamfferEntryController : Controller
    {
        public IActionResult ChamfferEntry()
        {
            return View();
        }
        public IActionResult ChamfferEditDelete ()
        {
            return View();
        }
    }
}
