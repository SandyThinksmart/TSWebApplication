using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ColdDrawingWIREDRAWINGController : Controller
    {
          private readonly IStudentService _studentService;
        private readonly IColdDrawingWireDrawingService _ColdDrawingWireDrawingService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
      

        public ColdDrawingWIREDRAWINGController(IStudentService studentService, IColdDrawingWireDrawingService coldDrawingWireDrawingService, LocalizationServices localization, IAllCommonService commonService)
        {
            _studentService = studentService;
            _localization = localization;
            _ColdDrawingWireDrawingService = coldDrawingWireDrawingService;
            _commonService = commonService;
        }

        [HttpPost("MappChuubanAndGyoubanByBarcode")]
        public IActionResult MappChuubanAndGyoubanByBarcode(string orderNo)
        {
            bool returnval = false;
           var result= _commonService.MappChuubanAndGyoubanByBarcode(orderNo);

            if (result.ORDER_NO_IN_CHUUBAN != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }

       
        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetColdDrawingWireDrawingDetailsByChuuban")]
        public IActionResult GetColdDrawingWireDrawingDetailsByChuuban([FromBody] ColdDrawingWireDrawingDetails request)
        {
            var result = _ColdDrawingWireDrawingService.GetColdDrawingWireDrawingDetailsByChuuban(request.PP_CHUUBAN, request.PP_GYOUBAN, request.MACHINE_CD,request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
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

        [HttpPost("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD)
        {
            var result = _ColdDrawingWireDrawingService.GetColdDrawingWireDrawing24MachineList(machineCD);


            foreach (var record in result)
            {

                record.DW_WINDING_SHAPE1 = record.DW_WINDING_SHAPE != null ? UIUtils.GetColdDrawingWindingShape(Convert.ToString(record.DW_WINDING_SHAPE)) : string.Empty;
                record.DW_DW_EXT_TEXTURE_STATUS = record.DW_EXT_TEXTURE_STATUS != null ? UIUtils.GetExteriorCondition(Convert.ToString(record.DW_EXT_TEXTURE_STATUS)) : string.Empty;
                record.DW_DW_MAGNETIC_INSPECTIONS = record.DW_MAGNETIC_INSPECTIONS != null ? UIUtils.GetMagneticDustList(Convert.ToString(record.DW_MAGNETIC_INSPECTIONS)) : string.Empty;
                record.DW_DIE_SHAPE = record.DW_DIE_SHAPE != null ? UIUtils.GetDieShapeList(Convert.ToString(record.DW_DIE_SHAPE)) : string.Empty;
                

            }
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("CheckOperatorIncativeActive")]
        public ActionResult CheckOperatorIncativeActive(long OperatorCD)
        {
            string validationMsg = string.Empty;
            var result = _commonService.IsCheckEmployeeActiveInctiveByID(OperatorCD);
            if (result.Value == "inactive")
            {
                validationMsg += UIHelper.GetErrorMessageForInactiveMaster(result.Key, UIConstants.MasterName.MST_EMPLOYEE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_ACTIVE);
                return Ok(new { IsSuccess = false, message = validationMsg });
            }
            else
            {
                return Ok(new { IsSuccess = true, message = result });

            }

        }
                                  
        [HttpPost("GetThabaDetailsByChuuban")]
        public IActionResult GetThabaDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _ColdDrawingWireDrawingService.GetThabaDetails(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        {
            bool returnval = false;
            string returnData = _ColdDrawingWireDrawingService.GetThabaDetailsByThabaNumber(request);
            if (string.IsNullOrEmpty(returnData))
            {
                returnval = true;
            }
            return Ok(new { IsSuccess = returnval, message = returnData });
        }

        [HttpPost("ColdDrawingWireDrawingDetailsUpdate")]
        public IActionResult ColdDrawingWireDrawingDetailsUpdate([FromBody] ColdDrawingWireDrawingDetails request)
        {
           long msgCd= _ColdDrawingWireDrawingService.UpdateColdDrawingWireDrawingDetails(request);
            return Ok(new { IsSuccess = true, message = msgCd });
        }

        [HttpPost("ColdDrawingWireDrawingDetailsSave")]
        public IActionResult ColdDrawingWireDrawingDetailsSave([FromBody] ColdDrawingWireDrawingDetails request)
        {
            var result = _ColdDrawingWireDrawingService.InsertColdDrawingWireDrawingDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetColdDrawingWireDrawingDetailsByDrawingCD")]
        public IActionResult GetColdDrawingWireDrawingDetailsByDrawingCD(long drawingCD,string cultureName)
        {
            var result = _ColdDrawingWireDrawingService.GetColdDrawingWireDrawingDetailsByDrawingCD(drawingCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _commonService.GetMachineName(Convert.ToString(UIConstants.K_MACHINEGROUP_WIRE_DRAWING));
            return Json(new { IsSuccess = true, message = result });
        }

        [HttpPost("ColdDrawingWireDrawingDetailsDelete")]
        public IActionResult ColdDrawingWireDrawingDetailsDelete(ColdDrawingWireDrawingDetails request)
        {
           _ColdDrawingWireDrawingService.DeleteColdDrawingWireDrawingDetails(request);

            

            return Json(new { IsSuccess = true, message = "" });
        }

        [HttpGet("EmployeeList")]
        public IActionResult EmployeeList(string name = "", bool addEmpty = true)
        {
            var lst = _ColdDrawingWireDrawingService.GetFilteredEmployeeNameList(name, addEmpty);
            return Json(new { IsSuccess = true, message = lst });
        }


        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            //string message = ValidateSearchFields(request);
            //if (string.IsNullOrEmpty(message))
            //{
                var result = _ColdDrawingWireDrawingService.GetColdDrawingWireDrawingdetailList(request);

               
                //  string view = _viewRenderService.RenderToString("ColdDrawingWireDrawing/ColdDrawingWireDrawingGrid", result);
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
            if (request.PROD_DIM_D_MAX > 0 && request.PROD_DIM_D_MIN <=0 )
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
            }
            if (request.PROD_LENGTH_MAX > 0 && request?.PROD_LENGTH_MIN <=0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }
            return validationMsg;
        }

        [HttpPost("GetWireDrawingDailyReportOnFactoryDailyWorkStatus")]
        public IActionResult GetWireDrawingDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _ColdDrawingWireDrawingService.GetWireDrawingDailyReportOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetDailyProcessColdDrawingWGrid")]
        public IActionResult GetDailyProcessColdDrawingWGrid(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _ColdDrawingWireDrawingService.GetDailyProcessAnalysisColdDrawingW(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Json(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingCtoC"))));
            return Ok(new { IsSuccess = true, message = result });
        }

        //Added chuuban validation check this method
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForWireDrawingFactory")]
        public ActionResult GetRowCountForWireDrawingFactory(GridSearchFields gridSearchRequest)
        {
            int count = _ColdDrawingWireDrawingService.GetRowCountForWireDrawingFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForColdDrawingCoilToCoilDailyAnalysis")]
        public ActionResult GetRowCountForColdDrawingCoilToCoilDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _ColdDrawingWireDrawingService.GetRowCountForColdDrawingCoilToCoilDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
    }


}
