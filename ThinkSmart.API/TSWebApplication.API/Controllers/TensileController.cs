using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TensileController : Controller
    {
        private readonly ITensileService _tensileService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        
        public TensileController( ITensileService tensileService, LocalizationServices localization, IAllCommonService commonService)
        {
            
            _localization = localization;
            _tensileService = tensileService;
            _commonService = commonService;
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
        [HttpPost("GetTensileDetailsByChuuban")]
        public IActionResult GetTensileDetailsByChuuban([FromBody] TensileDetails request)
        {
            TensileDetails TensileDetails = _tensileService.GetTensileDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN,request.CULTURE_NAME);
            return Json(new { IsSuccess = true, message = TensileDetails });
        }

        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {

            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }

        [HttpPost("GetJISTestList")]
            public IActionResult GetJISTestList()
        {
            var result = _tensileService.GetJISTestList();
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessMechanical_Insp_Hardness_Tensile"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        
        [HttpPost("GetDimShape")]
        public IActionResult GetDimShape()
        {
            var result = _tensileService.GetDimShapeList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("EmployeeList")]
        public IActionResult EmployeeList(string? yobina = "", int operatorCD=0)
        {
            var lst = _commonService.GetOperatorName(yobina, operatorCD);
            return Json(new { IsSuccess = true, message = lst });
        }
        [HttpPost("TensileDetailsSave")]

        public IActionResult TensileDetailsSave([FromBody] TensileDetails request)
        {
            var result = _tensileService.InsertTensileDetails(request);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("GetThabaDetailsByChuuban")]
        public IActionResult GetThabaDetailsByChuuban([FromBody] TensileDetails request)
        {
            var result = _tensileService.GetThabaDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MI_TS_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber(long? processPlanCD,short tabaNo)
        {
            long ppCD=Convert.ToInt64(processPlanCD);
         //   short taba = Convert.ToInt16(request.DETAIL_NO);
            var result = _tensileService.GetThabaDetailsByThabaNumber(ppCD, tabaNo);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetFurnanceNo")]
        public IActionResult GetFurnanceNo([FromBody] TensileDetails request)
        {
            bool returnVal=false;

            TensileDetails result = new TensileDetails();
            result = _tensileService.GetFurnanceNo(request.PP_CHUUBAN, request.PP_GYOUBAN,request.DETAIL_NO);

            if (result != null )

            {
                returnVal = true;
            }
          
            return Ok(new { IsSuccess = returnVal, message = result });
        }

        [HttpPost("GetTensileDetailsByCD")]
        public IActionResult GetTensileDetailsByCD(long TensileCD, string chuuban, long gyouban,string culture_Name)
        {

            var result = _tensileService.GetTensileDetailsByCD(TensileCD, chuuban, gyouban,culture_Name);

            return Ok( new {IsSucess = true, message = result });
        }
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {


            //string message = ValidateSearchFields(request);
            //if (string.IsNullOrEmpty(message))
            //{
                var result = _tensileService.GetTensileList(request);


               
                return Json(new { IsSuccess = true, message = result });
            //}
            //else
            //{
            //    return Json(new { IsSuccess = false, message = message });
            //}
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (request.PROD_DIM_D_MAX > 0 && request.PROD_DIM_D_MIN <= 0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
            }
            if (request.PROD_LENGTH_MAX > 0 && request.PROD_LENGTH_MIN <= 0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }
            return validationMsg;
        }
        [HttpPost("TensileDetailsUpdate")]
        public IActionResult TensileDetailsUpdate([FromBody] TensileDetails request)
        {
            long msgCd = _tensileService.UpdateTensileDetails(request);
            return Ok(new { IsSuccess = true, message = msgCd });
        }
        
        [HttpPost("TensileDetailsDelete")]
        public IActionResult TensileDetailsDelete([FromBody] TensileDetails request)
        {
            var result = _tensileService.DeleteTensileDetails(request);

            return Json(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMECHTensileOnFactoryDailyWorkStatus")]
        public IActionResult GetMECHTensileOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _tensileService.GetMECHTensileOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetMECHTensileOnDailyProcessStatus")]
        public IActionResult GetMECHTensileOnDailyProcessStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _tensileService.GetMECHTensileOnDailyProcessStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        //Added Date Range
        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;

            DateTime minDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            DateTime maxDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
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

        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForMechTensileFactory")]
        public ActionResult GetRowCountForMechTensileFactory(GridSearchFields gridSearchRequest)
        {
            int count = _tensileService.GetRowCountForMechTensileFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForMechTensileDailyAnalysis")]
        public ActionResult GetRowCountForMechTensileDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _tensileService.GetRowCountForMechTensileDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("GetJISTestMagnification")]
        public IActionResult GetJISTestMagnification( int testSpecCd)
        {
            var result = _tensileService.GetJISTestMagnification(testSpecCd);
            return Ok(new { IsSuccess = true, message = result });

        }

        [HttpGet("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetFICombinedDecisionDetails")]
        public ActionResult GetFICombinedDecisionDetails([FromBody] CommonFields request)
        {
            var result = _tensileService.GetFICombinedDecisionDetails(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("UpdateFICombinedDecision")]
        public IActionResult UpdateFICombinedDecision(short tensileDecision, long processPlanCd)
        {
            _tensileService.UpdateFICombinedDecision(tensileDecision, processPlanCd);
            return Ok(new { IsSuccess = true });

        }
        [HttpPost("ValidateTensileDetailsOnUpdateBtn")]
        public IActionResult ValidateTensileDetailsOnUpdateBtn(long MITensileCD)
        {
            var result = _tensileService.ValidateTensileDetailsOnUpdateBtn(MITensileCD);
            return Ok(new { IsSuccess = true, message = result });

        }
    }
}
