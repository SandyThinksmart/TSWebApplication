using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;



namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class StamppingController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IStamppingService _stamppingService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        //private readonly IViewRenderService _viewRenderService;
        public StamppingController(IStudentService studentService, IStamppingService stamppingServices, IAllCommonService commonService, LocalizationServices localization)
        {
            _studentService = studentService;
            _localization = localization;
            _commonService = commonService;
            _stamppingService = stamppingServices;

        }
        [HttpPost("MappNifudaByBarcode")]
        public IActionResult MappNifudaByBarcode(string nifudaNo)
        {
            bool returnval = false;
            var result = _commonService.MappNifudaByBarcode(nifudaNo);

            if (result.NIFUDA_NO != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
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
        [HttpPost("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Json(new { IsSuccess = true, message = result });

        }

        [HttpPost("StamppingDetailsSave")]
        public IActionResult StamppingDetailsSave([FromBody]StamppingDetails request  )
        {
            var result = _stamppingService.InsertStamppingDetails(request);
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpPost("StamppingDetailsUpdate")]
        public IActionResult StamppingDetailsUpdate([FromBody] StamppingDetails request)
        {
           long msgCd =  _stamppingService.UpdateStamppingDetails(request);
            return Json(new { issucess = true , message= msgCd });
        }
        [HttpPost("StamppingDetailsDelete")]
        public IActionResult StamppingDetailsDelete(StamppingDetails request)
        {
            _stamppingService.DeleteStamppingDetails(request);
            
            return Json(new { IsSuccess = true, message = "" });
        }
        [HttpPost("GetStampingDetailsByChuuban")]
        public IActionResult GetStampingDetailsByChuuban([FromBody]StamppingDetails request)
        {
            var result = _stamppingService.GetStamppingDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MACHINE_CD, request.NIFUDA_NO, request.CULTURE_NAME);
            return Json(new {issucess=true, message=result});
        }   
       
        //private string ValidateSearchFields(StamppingGridSearchRequest request)
        //{
        //    string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
        //    string validationMsg = string.Empty;
        //    if (!string.IsNullOrEmpty(request.txtDimDMax?.Trim()) && string.IsNullOrEmpty(request.txtDimDMin?.Trim()))
        //    {
        //        validationMsg += Constants.StringValues.NEW_LINE;
        //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
        //    }
        //    if (!string.IsNullOrEmpty(request.txtLengthSearchMax?.Trim()) && string.IsNullOrEmpty(request?.txtLengthSearchMin?.Trim()))
        //    {
        //        validationMsg += Constants.StringValues.NEW_LINE;
        //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
        //    }

        //    return validationMsg;
        //}

        [HttpGet("GetStamp")]
        public IActionResult GetStamp()
        {
            var result = _stamppingService.GetStamp();
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetStamppingDetailsByStamppingCD")]
        public IActionResult GetStamppingDetailsByStamppingCD(long stamppingCD, string cultureName)
        {
            var result = _stamppingService.GetStamppingDetailsByStamppingCD(stamppingCD,cultureName);
            return Json(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessStamping"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD, string cultureName)
        {
            var result = _stamppingService.GetStampping24MachineList(machineCD, cultureName);
            
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            //string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            //return Json(new { IsSuccess = true, message = dateTime });
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpPost("GetThabaDetailsByChuuban")]
        public IActionResult GetThabaDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _stamppingService.GetThabaDetails(request.CHUUBAN, request.GYOUBAN);
            return Json(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        {
            var result = _stamppingService.GetThabaDetailsByThabaNumber(request.CHUUBAN, request.GYOUBAN, request.TABA_NO);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            //string message = ValidateSearchFields(request);
            //if (string.IsNullOrEmpty(message))
            //{
                var result = _stamppingService.GetStamppingList(request);
               return Json(new { IsSuccess = true, message = result });
            //}
            //else
            //{
            //    return Json(new { IsSuccess = false, message = message });
            //}
        }      

        [HttpPost("EmployeeList")]
        public IActionResult EmployeeList(string? name="", bool addEmpty = true)
        {
           
            var lst = _stamppingService.GetFilteredEmployeeNameList(name, addEmpty);
            return Json(new { IsSuccess = true, message = lst });
        }

        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProcessDefect")]
        public IActionResult GetProcessDefect()
        {
            var result = _commonService.GetProcessDefectList(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessStamping")));
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _commonService.GetMachineName(Convert.ToString(UIConstants.K_MACHINEGROUP_STAMPPING));
            return Json(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetDecision")]
        public IActionResult GetDecision()
        {
            var result = _stamppingService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetTrialResult")]
        public IActionResult GetTrialResult()
        {
            var result = _stamppingService.GetTrialResultList();
            return Ok(new {IsSuccess=true, message=result});
        }
        [HttpGet("GetUnitType")]
        public IActionResult GetUnitType()
        {
            var result = _stamppingService.GetUnitTypeList();
            return Ok(new {IsSuccess=true,message=result});
        }
        [HttpPost("GetStamppingDailyReportOnFactoryDailyWorkStatus")]
        public IActionResult GetStamppingDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _stamppingService.GetStamppingDailyReportOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetDailyProcessStamppingReportGrid")]
        public IActionResult GetDailyProcessStamppingReportGrid(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _stamppingService.GetDailyProcessAnalysisStamppingReport(gridSearchRequest);
            if(result.Count > 0)
            {
                isSuccess = true;
            }
            return Json(new { IsSuccess = isSuccess, message = result });
        }

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

        [HttpPost("GetRowCountForStamppingFactory")]
        public ActionResult GetRowCountForStamppingFactory(GridSearchFields gridSearchRequest)
        {
            int count = _stamppingService.GetRowCountForStamppingFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForStamppingDailyAnalysis")]
        public ActionResult GetRowCountForStamppingDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _stamppingService.GetRowCountForStamppingDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        
    }
}
