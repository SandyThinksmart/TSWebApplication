using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using UAParser;

namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class SessionHandlerController : Controller
    {
        private readonly ISessionHandlerServices _sessionHandlerServices;
        private readonly IAllCommonService _commonServices;
        protected int logouthrs;

        public SessionHandlerController(IAllCommonService allCommonService, ISessionHandlerServices sessionHandlerServices, IConfiguration _configuration)
        {
            _sessionHandlerServices = sessionHandlerServices;
            _commonServices = allCommonService;
            logouthrs = Convert.ToInt16(_configuration.GetSection("LogoutHours").GetSection("Hours").Value);
        }
        [HttpPost("ManageSession")]
        public IActionResult ManageSession(string cookieValue)
        {
            bool returnVal = false;
            string userId = string.Empty;
            string user = string.Empty;
            string location = string.Empty;
            if (!string.IsNullOrEmpty(cookieValue))
            {

                var browsername = string.Empty;

                var userAgent = HttpContext.Request.Headers["User-Agent"];
                var uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(userAgent);
                browsername = c.UA.Family;

                cookieValue = cookieValue.Replace('-', '+').Replace('_', '/').PadRight(4 * ((cookieValue.Length + 3) / 4), '=');
                byte[] decrypt = Convert.FromBase64String(cookieValue);
                string uniqueid = Convert.ToString(System.Text.ASCIIEncoding.Default.GetString(decrypt));

                string[] spitid = uniqueid.Split('-');
                user = Convert.ToString(spitid[1]);
                returnVal = Manage(Convert.ToString(spitid[1]), browsername, Convert.ToString(spitid[0]));
                userId=Convert.ToString(spitid[2]);
            }
            if (Convert.ToInt32(userId) > 0)
            {
                location =Convert.ToString( _commonServices.GetLocationName(Convert.ToInt32(userId)));
            }

            return Ok(new { IsSuccess = returnVal, message = (new { userID = userId, locationName = location,userName = user }) });


        }
        private bool Manage(string workstationName,string browserName, string randomValue)
        {
           
            var result = _sessionHandlerServices.checkUserLastActivity(workstationName, browserName, randomValue);

            if (result != null)
            {
                DateTime startTime = DateTime.Now;

                DateTime lastUsedTime = result.LAST_USED_TIME;
                TimeSpan span = startTime.Subtract(lastUsedTime);

                int usedHours = span.Hours;

                if (usedHours >= logouthrs)
                {
                    _sessionHandlerServices.deleteLoginData(result.COOKIES_UNIQUE_ID);
                    return false;
                }
                else
                {
                    _sessionHandlerServices.updateLastUsedTime(result.COOKIES_UNIQUE_ID);


                    return true;
                }

            }
            else
            {
                return false;
            }


        }
    }
}
