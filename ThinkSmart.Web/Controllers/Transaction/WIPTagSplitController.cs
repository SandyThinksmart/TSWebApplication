using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class WIPTagSplitController : Controller
    {
        public IActionResult WIPTagSplitDelete()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult GetProductUnitKubun()
        {
            var unitTypeList = UIUtils.Instance.GetProductUnitKubun();
            return Ok(new { IsSuccess = true, message = unitTypeList });
        }
       
    }
}
