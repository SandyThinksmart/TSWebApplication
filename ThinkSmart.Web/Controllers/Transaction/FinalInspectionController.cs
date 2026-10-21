using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class FinalInspectionController : Controller
    {
        public IActionResult FinalInspectionReport()
        {
            return View();
        }
    }
}
