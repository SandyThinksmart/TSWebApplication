using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Straightening2Controller : ControllerBase
    {
        private readonly IStraightening2Service _straightening2Service;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public Straightening2Controller(IStraightening2Service straightening2Service, LocalizationServices localizationServices, IAllCommonService commonService)
        {
            _straightening2Service = straightening2Service;
            _localization = localizationServices;
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
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            //string message = ValidateSearchFields(request);
            //if (string.IsNullOrEmpty(message))
            //{
                var result = _straightening2Service.GetStraightening2List(request);
          
            return Ok(new { IsSuccess = true, message = result });
            //}
            //else
            //{
            //    return Ok(new { IsSuccess = false, message = message });
            //}
        }

        [HttpGet("GetStraighteningDetailsByStraighteningCD")]
        public ActionResult GetStraighteningDetailsByStraighteningCD(long STR_STRAIGHTENING_CD, string cultureName)
        {
            var result = _straightening2Service.GetStraightening2DetailsByCD(STR_STRAIGHTENING_CD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("StraighteningDetailsDelete")]
        public ActionResult StraighteningDetailsDelete([FromBody]Straightening2Details request)
        {
            _straightening2Service.Delete(request);

            //if (MSG_CD > 0)
            //{
            //    _commonService.MessageDetailsDelete(MSG_CD);
            //}

            return Ok(new { IsSuccess = true, message = "" });
        }
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _commonService.GetMachineName(Convert.ToString(UIConstants.K_MACHINEGROUP_STRAIGHTENING));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            //string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            //return Ok(new { IsSuccess = true, message = dateTime });
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpGet("GetUnitType")]
        public ActionResult GetUnitType()
        {
            var result = _straightening2Service.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetTPTestList")]
        public IActionResult GetTPTestList()
        {
            var result = _straightening2Service.GetTPTestList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIConstants.K_PROCESS_PRESS_STRAIGHTENING));
            return Ok(new { IsSuccess = true, message = result });
        }
        
        [HttpPost("GetExteriorCondition")]
        public IActionResult GetExteriorCondition()
        {
            var result = _straightening2Service.GetExteriorConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetBendStatus")]
        public IActionResult GetBendStatus()
        {
            var result = _straightening2Service.GetBendStatusList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetProcessDefect")]
        public ActionResult GetProcessDefect()
        {            
            var result = _commonService.GetProcessDefectList(UIConstants.K_PROCESS_PRESS_STRAIGHTENING);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetThabaDetailsByChuuban")]
        public ActionResult GetThabaDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _straightening2Service.GetThabaDetailsByChuuban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD, string cultureName)
        {
            var result = _straightening2Service.GetMachine24Details(machineCD, cultureName);            
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetStraighteningDetailsByChuuban")]
        public ActionResult GetStraighteningDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _straightening2Service.GetStraighteningDetailsByChuuban(request.CHUUBAN, request.GYOUBAN, request.MACHINE_CD,request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("StraighteningDetailsSave")]
        public ActionResult StraighteningDetailsSave([FromBody] Straightening2Details request)
        {
            var result = _straightening2Service.InsertStraightening2Details(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("StraighteningDetailsUpdate")]
        public ActionResult StraighteningDetailsUpdate([FromBody] Straightening2Details request)
        {
          long msgCd =  _straightening2Service.Straightening2DetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = msgCd });
        }

        [HttpPost("GetThabaDetailsByChuubanAndCD")]
        public ActionResult GetThabaDetailsByChuubanAndCD([FromBody] GridSearchFields request)
        {
            var result = _straightening2Service.GetThabaDetailsWithOKDecision(request.CHUUBAN, request.GYOUBAN, request.STRAIGHTENING_CD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetOperatorNamebyCD")]
        public ActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });      
        }
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (request.PROD_DIM_D_MAX > 0 && request.PROD_DIM_D_MIN <=0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
            }
            if (request.PROD_LENGTH_MAX > 0 && request.PROD_LENGTH_MIN <=0 )
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }

            if (!string.IsNullOrEmpty(request.START_DATETIME))
            {
                bool IsPODateOfStart = false;

                if (UIHelper.IsValidDate(request.START_DATETIME))
                {
                    IsPODateOfStart = true;
                }
                if (IsPODateOfStart == false)
                {
                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_StartDateInvalid_M");
                }

                if (request.END_DATETIME.Length > 0)
                {
                    bool IsPODateOfEnd = false;

                    if (UIHelper.IsValidDate(request.END_DATETIME))
                    {
                        IsPODateOfEnd = true;
                    }
                    if (IsPODateOfEnd == false)
                    {
                        validationMsg += Constants.StringValues.NEW_LINE;
                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_EndDateInvalid_M");
                    }
                }
            }

            //if (string.IsNullOrEmpty(request.txtOperationDateStart?.Trim()))
            //{
            //    validationMsg += Constants.StringValues.NEW_LINE;
            //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_ProcessDateStartRequired");
            //}
            return validationMsg;
        }
        //[HttpPost("GetThabaDetailsByThabaNumber")]
        //public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        //{
        //    var result = _straightening2Service.GetThabaDetailsByThabaNumber(request.CHUUBAN, request.GYOUBAN, request.THABA_NO_STRING); 
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        {
            bool returnval = false;

            string returnData = _straightening2Service.GetThabaDetailsByThabaNumber(request);

            if (string.IsNullOrEmpty(returnData))
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = returnData });
        }
        [HttpPost("GetStraightning2DailyReportDataOnFactoryDailyWorkStatus")]
        public IActionResult GetStraightning2DailyReportDataOnFactoryDailyWorkStatus([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _straightening2Service.GetStraightning2DailyReportDataOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetStraightning2DailyProcessAnalysisStatus")]
        public IActionResult GetStraightning2DailyProcessAnalysisStatus([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _straightening2Service.GetStraightning2DailyProcessAnalysisStatus(gridSearchRequest);
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

        [HttpPost("GetRowCountForStraightening2Factory")]
        public ActionResult GetRowCountForStraightening2Factory(GridSearchFields gridSearchRequest)
        {
            int count = _straightening2Service.GetRowCountForStraightening2Factory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForStraightening2DailyAnalysis")]
        public ActionResult GetRowCountForStraightening2DailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _straightening2Service.GetRowCountForStraightening2DailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

    }
}
