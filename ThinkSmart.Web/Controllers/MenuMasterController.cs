using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System;
using static TSWebApplication.Domain.Utility.Constants;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace TSWebApplication.Web.Controllers
{
    public class MenuMasterController : Controller
    {
        protected string loginURL = null;
        public MenuMasterController(IConfiguration _configuration)
        {
            loginURL = _configuration.GetSection("LoginUrl")["Url"];
            
        }
        public IActionResult MenuPage()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Culture"); // To set auto JP Culture removed 
            return View();
        }
            public IActionResult Index()
        {
            // string url = HttpContext.Request.GetDisplayUrl();           
            //string parameter = HttpContext.Request.Query["readData"].ToString();

            //if (string.IsNullOrEmpty(parameter))
            //{
            //    return Redirect(loginURL);
            //}
            //else
            //{
            //    string coockieValue = parameter.Replace('-', '+').Replace('_', '/').PadRight(4 * ((parameter.Length + 3) / 4), '=');
            //    byte[] decrypt = Convert.FromBase64String(parameter);
            //    string uniqueid = Convert.ToString(System.Text.ASCIIEncoding.Default.GetString(decrypt));

            //    string[] spitid = uniqueid.Split('-');
            //    int userID = Convert.ToInt32(spitid[2]);
            //    HttpContext.Session.SetInt32("userId", userID);

            //    return View();
            //}
            return View();
        }
        public IActionResult MobileIndex()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Culture"); // To set auto JP Culture removed 
            return View();
        }
            public IActionResult CreateSession(string loginInfo)
        {
            int userID = 0;
            if (!string.IsNullOrEmpty(loginInfo))
            {
                string coockieValue = loginInfo.Replace('-', '+').Replace('_', '/').PadRight(4 * ((loginInfo.Length + 3) / 4), '=');
                byte[] decrypt = Convert.FromBase64String(loginInfo);
                string uniqueid = Convert.ToString(System.Text.ASCIIEncoding.Default.GetString(decrypt));

                string[] spitid = uniqueid.Split('-');
                 userID = Convert.ToInt32(spitid[2]);
             
            }
            return Ok(new { userId = userID });
        }
    }
}
