using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FinalInspectionController : ControllerBase
    {
        private readonly IFinalInspectionService _finalInspectionService;
        private readonly IAllCommonService _allCommonService;

        private readonly LocalizationServices _localization;
        public FinalInspectionController(IFinalInspectionService finalInspectionService, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _finalInspectionService = finalInspectionService;
            _localization = localization;
            _allCommonService = allCommonService;

        }

        [HttpPost("GetFinalInspectionDataOnFactoryDailyWorkStatus")]
        public IActionResult GetFinalInspectionDataOnFactoryDailyWorkStatus([FromBody]GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _finalInspectionService.GetFinalInspectionDataOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetFinalInspectionDataOnDailyProcessStatus")]
        public IActionResult GetFinalInspectionDataOnDailyProcessStatus([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            //FactoryDailyWorkStatusSearchProcessWise gridSearchRequest = new FactoryDailyWorkStatusSearchProcessWise();
            var result = _finalInspectionService.GetFinalInspectionDataOnDailyProcessStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetRowCountForFinalInspectionFactory")]
        public ActionResult GetRowCountForFinalInspectionFactory(GridSearchFields gridSearchRequest)
        {
            int count = _finalInspectionService.GetRowCountForFinalInspectionFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("GetRowCountForFinalInspectionDailyAnalysis")]
        public ActionResult GetRowCountForFinalInspectionDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _finalInspectionService.GetRowCountForFinalInspectionDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        //Final Inspection Report
        /// <summary>
        /// To Get Operator List
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0 )
        {
            var result = _allCommonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetLocation")]
        public ActionResult GetLocation(int USER_ID)
        {
            var result = _finalInspectionService.GetLocation(USER_ID);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("isFIPrintDataExist")]
        public IActionResult isFIPrintDataExist(string StartDate,string EndDate,string OperatorID,string SortOrder,string isPrintByPrintFlag,string userId)
        {
            bool isSuccess = false;
            DateTime strtDate;
            string _startDate;
            DateTime edDate;
            string _endDate;

            if (StartDate != "")
            {
                strtDate = Convert.ToDateTime(StartDate);
                _startDate = strtDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }
            else
            {
                _startDate = Convert.ToString(DateTime.Now);
            }
            if (EndDate != "")
            {
                edDate = Convert.ToDateTime(EndDate);
                _endDate = edDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }
            else
            {
                _endDate = Convert.ToString(DateTime.Now);
            }

            var result = _finalInspectionService.LoadFinalInspectionDetails(_startDate, _endDate, OperatorID, Convert.ToInt32(SortOrder), "0", Convert.ToInt32(isPrintByPrintFlag), Convert.ToInt32(userId));
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });

        }


        [HttpPost("isFinishProductDataExist")]
        public IActionResult isFinishProductDataExist(string StartDate, string EndDate, string OperatorID, string SortOrder,int isPrintByPrintFlag, string userId)
        {
            bool isSuccess = false;
            DateTime strtDate;
            string _startDate;
            DateTime edDate;
            string _endDate;

            if (StartDate != "")
            {
                strtDate = Convert.ToDateTime(StartDate);
                _startDate = strtDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }

            else
            {
                _startDate = Convert.ToString(DateTime.Now);
            }
            if (EndDate != "")
            {
                edDate = Convert.ToDateTime(EndDate);
                _endDate = edDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }
            else
            {
                _endDate = Convert.ToString(DateTime.Now);
            }

            int result = _finalInspectionService.GetFinishProductDetails(_startDate, _endDate, OperatorID, isPrintByPrintFlag, Convert.ToInt32(userId));
            if (result > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });

        }


        [HttpPost("isFISimpleRecordDataExist")]
        public IActionResult isFISimpleRecordDataExist(string StartDate, string EndDate, string OperatorID, string SortOrder, string isPrintByPrintFlag, string userId)
        {
            bool isSuccess = false;
            DateTime strtDate;
            string _startDate;
            DateTime edDate;
            string _endDate;

            if (StartDate != "")
            {
                strtDate = Convert.ToDateTime(StartDate);
                _startDate = strtDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }

            else
            {
                _startDate = Convert.ToString(DateTime.Now);
            }
            if (EndDate != "")
            {
                edDate = Convert.ToDateTime(EndDate);
                _endDate = edDate.ToString(Constants.DateFormats.YYYYMMDD_HHMM);
            }
            else
            {
                _endDate = Convert.ToString(DateTime.Now);
            }

            int result = _finalInspectionService.GetSimpleFinalInspectionDetails(_startDate, _endDate, OperatorID, Convert.ToInt32(isPrintByPrintFlag), Convert.ToInt32(userId));
            if (result > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }


    }
}

