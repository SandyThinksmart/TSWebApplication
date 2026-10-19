using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;
using System.Net;
using System.Xml;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HikinukiCoilToBarController : ControllerBase
    {
        private readonly IHikinukiCoilToBarService _hikinukiCoilToBarService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        private readonly IAllCommonService _allCommonService;
        private readonly IConfiguration _configuration;
        private readonly string serviceUrl;
        private readonly string methodName;
        public HikinukiCoilToBarController(IHikinukiCoilToBarService hikinukiCoilToBarService, LocalizationServices localization, IAllCommonService commonService, IAllCommonService allCommonService, IConfiguration configuration)
        {
            _hikinukiCoilToBarService = hikinukiCoilToBarService;
            _localization = localization;
            _commonService = commonService;
            _allCommonService = allCommonService;
            _configuration = configuration;

            serviceUrl = _configuration.GetSection("Hikinuki_To_Host_Data_transfer").GetSection("ServiceUrl").Value;
            methodName = _configuration.GetSection("Hikinuki_To_Host_Data_transfer").GetSection("MethodName").Value;
        }
        [HttpPost("MappChuubanAndGyoubanByBarcode")]
        public IActionResult MappChuubanAndGyoubanByBarcode(string orderNo)
        {
            bool returnval = false;
            var result = _commonService.MappChuubanAndGyoubanByBarcode(orderNo);

            if (result.ORDER_NO_IN_CHUUBAN != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }

        [HttpPost("GetThabaDetailsByChuuban")]
        public ActionResult GetThabaDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _hikinukiCoilToBarService.GetThabaDetailsByChuuban(request.CHUUBAN, Convert.ToString(request.GYOUBAN));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHikinukiDetailsByChuuban")]
        public ActionResult GetHikinukiDetailsByChuuban([FromBody] HikinukiDetails request)
        {
            var result = _hikinukiCoilToBarService.GetHikinukiDetailsByChuuban(request.PP_CHUUBAN, Convert.ToString(request.PP_GYOUBAN), Convert.ToString(request.DW_MACHINE_CD), request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList(string machineType)
        {
            var result = _allCommonService.GetMachineName(machineType);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("getHikinukiHistoryByChuuban")]
        public ActionResult getHikinukiHistoryByChuuban([FromBody] GridSearchFields request)
        {
            var result = _hikinukiCoilToBarService.GetHikinukiHistoryByChuuban(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD, string cultureName)
        {
            var result = _hikinukiCoilToBarService.GetHikinuki24MachineDetails(machineCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }

        [HttpPost("HikinukiDetailsSave")]
        public ActionResult HikinukiDetailsSave([FromBody] HikinukiDetails request)
        {
            var result = _hikinukiCoilToBarService.InsertHikinukiDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;
            DateTime dateTime = _commonService.GetSystemDateTime();

            DateTime minDate = Convert.ToDateTime(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            DateTime maxDate = Convert.ToDateTime(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            minDate = minDate.AddDays(-request.DATERANGE);
            maxDate = maxDate.AddDays(+request.DATERANGE);

            if (Convert.ToDateTime(request.RANGE_DATETIME) < Convert.ToDateTime(minDate) || Convert.ToDateTime(request.RANGE_DATETIME) > Convert.ToDateTime(maxDate))
            {
                returnval = false;
            }
            else
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval });
        }

        [HttpPost("HikinukiDetailsUpdate")]
        public ActionResult HikinukiDetailsUpdate([FromBody] HikinukiDetails request)
        {
            long msgCD = _hikinukiCoilToBarService.UpdateHikinukiDetails(request);
            return Ok(new { IsSuccess = true, message = msgCD });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            
                var result = _hikinukiCoilToBarService.GetHikinukiCoilToBarList(request);
                return Ok(new { IsSuccess = true, message = result });
           
        }
        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        {
            bool returnval = false;

            string returnData = _hikinukiCoilToBarService.GetThabaDetailsByThabaNumber(request);

            if (string.IsNullOrEmpty(returnData))
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = returnData });
        }
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _allCommonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHikinukiDetailsByDrawingCD")]
        public ActionResult GetHikinukiDetailsByDrawingCD(long DrawingCD, string cultureName)
        {
            var result = _hikinukiCoilToBarService.GetHikinukiDetailsDrawingByCD(DrawingCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("HikinukiDetailsDelete")]
        public ActionResult HikinukiDetailsDelete(HikinukiDetails details)
        {
            _hikinukiCoilToBarService.Delete(details);
            return Ok(new { IsSuccess = true, message = "" });
        }


        [HttpPost("GetCombinedDailyReportData")]
        public ActionResult GetCombinedDailyReportData(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _hikinukiCoilToBarService.GetCombinedDailyReportData(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis")]
        public IActionResult GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _hikinukiCoilToBarService.GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpGet("GetProcessDefect")]
        public IActionResult GetProcessDefect()
        {
            var result = _commonService.GetProcessDefectList(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoB")));
            
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _allCommonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoB"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _allCommonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        //Added chuuban validation check this method
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForCombinedDailyReportFactory")]
        public ActionResult GetRowCountForCombinedDailyReportFactory(GridSearchFields gridSearchRequest)
        {
            int count = _hikinukiCoilToBarService.GetRowCountForCombinedDailyReportFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForHikinukiCoilToBarDailyAnalysis")]
        public ActionResult GetRowCountForHikinukiCoilToBarDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _hikinukiCoilToBarService.GetRowCountForHikinukiCoilToBarDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("GetDefaultSystemDateTime")]
        public IActionResult GetDefaultSystemDateTime()
        {
            DateTime dates = DateTime.Now;
            dates = dates.Date.Add(new TimeSpan(7, 59, 00));
            string endDate = dates.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            dates = dates.AddDays(-1).AddMinutes(+1);
            string StartDate = dates.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
           
            var result = new { START_DATE = StartDate, END_DATE = endDate };
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetEndDateByStartDate")]
        public IActionResult GetEndDateByStartDate(string date)
        {
            string StartDate=string.Empty;
            string endDate = string.Empty;
            try
            {
                DateTime dates = Convert.ToDateTime(date);
                StartDate = dates.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
                dates = dates.AddDays(+1).AddMinutes(-1);
                endDate = dates.ToString(Constants.DateFormats.YYYYMMDD_HHMM);

            }
            catch (Exception)
            {

            }
             

            var result = new { START_DATE = StartDate, END_DATE = endDate };
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("CheckDate")]
        public IActionResult CheckDate(string date)
        {
            bool returnval = false;
            try
            {
                DateTime dates = Convert.ToDateTime(date);
                returnval = true;
            }
            catch (Exception ex)
            {
                returnval = false;
            }


            return Ok(new { message = returnval });
        }
        [HttpPost("TransferDataToHost")]
        public async Task<int> TransferDataToHost(string startDate, string endDate, int userID)
        {
            int locationCD = _allCommonService.GetUserLocation(userID);

            PeriodicTimer ptimer = new PeriodicTimer(TimeSpan.FromSeconds(3));
            int statusCode = 0;  // statusCode =0 for no popup message, statusCode = 1 for data not found, statusCode = 2 for success, statusCode = 3 for exception
            int result = _hikinukiCoilToBarService.GetHikinukiDataForOracleToHost(startDate, endDate, userID, "print");

            if (result > 0)
            {
                ExecuteService(startDate, endDate,Convert.ToString(locationCD));
                while (await ptimer.WaitForNextTickAsync())
                {
                    statusCode = _hikinukiCoilToBarService.GetBatchStatusCode();
                    if (statusCode == 0 || statusCode == 2 || statusCode == 3)
                    {
                        ptimer.Dispose();
                    }

                }
            }
            else
            {
               
                statusCode = 1;
            }
           
            return statusCode;
        }
        private void ExecuteService(string startDate, string endDate, string locationCD)
        {
            string soapString =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <{0} xmlns=""http://tempuri.org/"">
                  {1}
                </{0}>
              </soap:Body>
            </soap:Envelope>";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@serviceUrl);
            webRequest.Headers.Add("SOAPAction", "\"http://tempuri.org/" + methodName + "\"");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            using (Stream stream = webRequest.GetRequestStream())
            {
                string postValues = "";

                postValues += string.Format("<{0}>{1}</{0}>", "StartDate", startDate);

                postValues += string.Format("<{0}>{1}</{0}>", "EndDate", endDate);

                postValues += string.Format("<{0}>{1}</{0}>", "LocationCD", locationCD);

                soapString = string.Format(soapString, "DataTransferWithParameter", postValues);
                using (StreamWriter stmw = new StreamWriter(stream))
                {
                    stmw.Write(soapString);
                }
            }

            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                string result = responseReader.ReadToEnd();

            }
           
        }
        [HttpPost("ReprintReport")]
        public IActionResult ReprintReport(string startDate, string endDate, int userID)
        {
            bool isSuccess = false;
            int result = _hikinukiCoilToBarService.GetHikinukiDataForOracleToHost(startDate, endDate, userID,"reprint");

            if (result > 0)
            {
                isSuccess = true;
            }

            return Ok(new { IsSuccess = isSuccess });
        }

        [HttpGet("GetHikinukiCoilToBarMachineTypeList")]
        public IActionResult GetHikinukiCoilToBarMachineTypeList()
        {
            var result = _hikinukiCoilToBarService.GetHikinukiCoilToBarMachineTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
