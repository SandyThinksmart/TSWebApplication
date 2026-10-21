using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private readonly IReportsService _reportsService;
  
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost("GetFIPrintDetails")]
        public IActionResult GetFIPrintDetails([FromBody] FinalInspectionDetails request)
        {
            bool isSuccess = false;
            DateTime strtDate;
            string _startDate;
            DateTime edDate;
            string _endDate;

            if (request.START_DATE != "")
            {
                strtDate = Convert.ToDateTime(request.START_DATE);
                _startDate = strtDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }
            else
            {
                _startDate = Convert.ToString(DateTime.Now);
            }
            if (request.END_DATE != "")
            {
                 edDate = Convert.ToDateTime(request.END_DATE);
                _endDate = edDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }
            else
            {
                _endDate = Convert.ToString(DateTime.Now);
            }

            var result = _reportsService.LoadFinalInspectionDetails(_startDate, _endDate, Convert.ToString(request.OPERATOR_ID), Convert.ToInt32(request.SORT_ORDER), "0", Convert.ToInt32(request.IS_PRINT_BYPRINT_FLAG), Convert.ToInt32(request.USER_ID));
            
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            //  return Ok(new { IsSuccess = isSuccess, message = result });
            return Ok(new { result });

        }

        [HttpPost("GetHikinukiDetails")]
        public IActionResult GetHikinukiDetails([FromBody] GridSearchFields request)
        {
            var result = _reportsService.GetHikinukiList(request);
            return Json(new { result });
        }
        [HttpPost("GetFinishProductDetails")]
        public IActionResult GetFinishProductDetails([FromBody] FinalInspectionDetails request)
        {
            var result = _reportsService.GetFinishProductData(request);
            return Json(new { result });
        }

        [HttpPost("GetSimpleFinalInspectionDetails")]
        public IActionResult GetSimpleFinalInspectionDetails([FromBody] FinalInspectionDetails request)
        {
            var result = _reportsService.GetSimpleFinalInspectionDetails(request);
            return Json(new { result });
        }


        [HttpPost("GetWIPStatusList")]
        public IActionResult GetWIPStatusList(int userID)
        {
            var result = _reportsService.GetWIPStatusList(userID);
            return Json(new { result });
        }
        [HttpPost("GetWIPTensouAcceptanceDetails")]
        public IActionResult GetWIPTensouAcceptanceDetails(string startDate, string endDate, int userID)
        {
            var result = _reportsService.GetWIPTensouAcceptanceDetails(startDate, endDate, userID);
            return Json(new { result });
        }

        [HttpPost("GetWIPTensouRequestDetails")]
        public IActionResult GetWIPTensouRequestDetails(string startDate, int userID)
        {
            var result = _reportsService.GetWIPTensouRequestDetails(startDate, userID);
            return Json(new { result });
        }
        [HttpPost("GetPlaceOrderList")]
        public IActionResult GetPlaceOrderList(int userID,int yearR)
        {
            var result = _reportsService.GetPlaceOrderList(userID, yearR);
            return Json(new { result });
        }

        [HttpPost("GetMalNotFoundList")]
        public IActionResult GetMalNotFoundList(int userID, int year,int R)
        {
            var result = _reportsService.GetMalNotFoundList(userID, year,R);
            return Json(new { result });
        }

        [HttpPost("GetMalNameIntWiseData")]
        public IActionResult GetMalNameIntWiseData(int year, int R)
        {
            var result = _reportsService.GetMalNameIntWiseData( year,  R);
            return Json(new { result });
        }

        [HttpPost("GetMakerMillSheetErrorRptData")]
        public IActionResult GetMakerMillSheetErrorRptData(int runID)
        {

            var result = _reportsService.GetMakerMillSheetErrorRptData(runID);

            return Json(new { result });

        }

        [HttpPost("GetMakerMillSheetProcessRptData")]
        public IActionResult GetMakerMillSheetProcessRptData(int runID)
        {

            var result = _reportsService.GetMakerMillSheetProcessRptData(runID);

            return Json(new { result });
        }
        [HttpPost("GetMillsheetEDIErrorDetails")]
        public IActionResult GetMillsheetEDIErrorDetails(int runCD)
        {

            var result = _reportsService.GetEDIErrorReportData(runCD);

            return Json(new { result });
        }

        [HttpPost("GetDaidoFutouNyukoBatchReport")]
        public IActionResult GetDaidoFutouNyukoBatchReport(int userID,DateTime processDateTime,int printFlag)
        {
            var result = _reportsService.GetDaidoFutouNyukoBatchReport(userID,processDateTime,printFlag);
            return Json(new { result });
        }

        [HttpPost("GetDaidoFutouNyukoBatchErrorReport")]
        public IActionResult GetDaidoFutouNyukoBatchErrorReport(int userID, DateTime processDateTime, int printFlag)
        {
            var result = _reportsService.GetDaidoFutouNyukoBatchErrorReport(userID, processDateTime, printFlag);
            return Json(new { result });
        }
    }
}
