using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;

using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

using static TSWebApplication.Domain.Utility.UIConstants;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ECController : ControllerBase
    {
        private readonly IECService _eCService;
        private readonly IAllCommonService _allCommonService;

        private readonly LocalizationServices _localization;

        public ECController(IECService eCService, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _eCService = eCService;
            _eCService = eCService;
            _localization = localization;
            _allCommonService = allCommonService;

        }
        //Mapping Nifuda By Barcode
        [HttpPost("MappNifudaByBarcode")]
        public IActionResult MappNifudaByBarcode(string nifudaNo)
        {
            bool returnval = false;
            var result = _allCommonService.MappNifudaByBarcode(nifudaNo);

            if (result.NIFUDA_NO != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }

        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _allCommonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            DateTime dateTime = _allCommonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;
            DateTime dateTime = _allCommonService.GetSystemDateTime();

            DateTime minDate =Convert.ToDateTime(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            DateTime maxDate = Convert.ToDateTime(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            minDate  = minDate.AddDays(-request.DATERANGE);
            maxDate = maxDate.AddDays(+request.DATERANGE);

            if (Convert.ToDateTime(request.RANGE_DATETIME) < Convert.ToDateTime(minDate) || Convert.ToDateTime(request.RANGE_DATETIME) > Convert.ToDateTime(maxDate))
            {
                returnval = false;
            }else
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval });
        }
        
        [HttpPost("IsFIAlreadyDone")]
        public ActionResult IsFIAlreadyDone(long nifudaCD, short thabaNo)
        {
            bool returnval = false; 
            int getCount = _eCService.IsFIAlreadyDone(nifudaCD, thabaNo);
            if (getCount > 0)
            {
                returnval = false;
            }else
            {
                returnval = true;   
            }

            return Ok(new { IsSuccess = returnval });
        }
        [HttpPost("IsFinalInspectionCompleted")]
        public ActionResult IsFinalInspectionCompleted(long nifudaCD, short thabaNo, int ecCD)
        {
            bool returnval = false;
            var result = _eCService.IsFinalInspectionCompleted(nifudaCD, thabaNo, ecCD);
            if (result.Count >=2)
            {
                returnval = false;
            }
            else if (result.Count==1 && result[0].EC_CD == ecCD)
            {
                returnval = true;
            }
            else if (result.Count ==0)
            {
                returnval = true;
            }

                return Ok(new { IsSuccess = returnval });
        }
        [HttpPost("GetECDetailsByChuuban")]
        public ActionResult GetECDetailsByChuuban([FromBody] ECDetails request)
        {
            var result = _eCService.GetECDetailsByChuuban(request.NIFUDA_NO, request.MACHINE_CD, request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProcessDefect")]
        public IActionResult GetProcessDefect()
        {
            var result = _allCommonService.GetProcessDefectList(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessEC")));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD, string cultureName)
        {
            var result = _eCService.GetEC24MachineDetails(machineCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("CheckOperatorIncativeActive")]
        public ActionResult CheckOperatorIncativeActive(long OperatorCD)
        {
            string validationMsg = string.Empty;
            var result = _allCommonService.IsCheckEmployeeActiveInctiveByID(OperatorCD);
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
        public ActionResult GetThabaDetailsByChuuban([FromBody] ECDetails request)
        {
            var result = _eCService.GetThabaDetailsByChuuban(request.PP_CHUUBAN,request.PP_GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber(long processPlanCD, short thabaNo)
        {
            var result = _eCService.GetThabaDetailsByThabaNumber(processPlanCD, thabaNo);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetECHistoryByChuuban")]
        public ActionResult GetECHistoryByChuuban([FromBody] GridSearchFields request)
        {
            var result = _eCService.GetECHistoryByChuuban(request.CHUUBAN, request.GYOUBAN, request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("ECDetailsSave")]
        public ActionResult ECDetailsSave([FromBody] ECDetails request)
        {
            var result = _eCService.InsertECDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("ECDetailsUpdate")]
        public ActionResult ECDetailsUpdate([FromBody] ECDetails request)
        {
          long msgCD =   _eCService.UpdateECDetails(request);
            return Ok(new { IsSuccess = true, message = msgCD });
        }


        [HttpPost("ECDetailsDelete")]
        public ActionResult ECDetailsDelete([FromBody] ECDetails request)
        {
            _eCService.Delete(request);
            return Ok(new { IsSuccess = true, message = "" });
        }
        
        [HttpGet("GetFinalInspByECCD")]
        public ActionResult GetFinalInspByECCD(long ecCD)
        {
            string finalInsp = _eCService.GetFinalInspByECCD(ecCD);
            return Ok(new { IsSuccess = true, message = finalInsp });
        }

        [HttpGet("GetECDetailsByECCD")]
        public ActionResult GetECDetailsByECCD(long ecCD, string cultureName)
        {
            var result = _eCService.GetECDetailsByECCD(ecCD , cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _allCommonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {           
               
         var result = _eCService.GetECList(request);
         return Ok(new { IsSuccess = true, message = result });
               
           
        }
        [HttpPost("GetECDailyReportOnFactoryDailyWorkStatus")]
        public IActionResult GetECDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _eCService.GetECDailyReportOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetDailyProcessECDetailsGrid")]
        public IActionResult GetDailyProcessECDetailsGrid(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _eCService.GetDailyProcessAnalysisECDeatils(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList(string machineType)
        {
            var result = _allCommonService.GetMachineName(machineType);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _allCommonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _allCommonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessEC"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetFINextProcess")]
        public IActionResult GetFINextProcess()
        {
            var result = _allCommonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessCompletedInspection"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _allCommonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForECFactory")]
        public ActionResult GetRowCountForECFactory(GridSearchFields gridSearchRequest)
        {
            int count = _eCService.GetRowCountForECFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForECDailyAnalysis")]
        public ActionResult GetRowCountForECDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _eCService.GetRowCountForECDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("GetHardnessTensileDescion")]
        public ActionResult GetHardnessTensileDescion(long processPlanCD, string cultureName)
        {
            string hardnessDecision = _eCService.GetHardnessDescion(processPlanCD, cultureName);
            string tensileDecision = _eCService.GetTensileDescion(processPlanCD, cultureName);
            

            return Ok(new { IsSuccess = true, message = new { HARDNESS_DESICION = hardnessDecision, TENSILE_DECISION = tensileDecision } });
        }
        [HttpPost("MappChuubanAndGyoubanByBarcode")]
        public IActionResult MappChuubanAndGyoubanByBarcode(string orderNo)
        {
            bool returnval = false;
            var result = _allCommonService.MappChuubanAndGyoubanByBarcode(orderNo);

            if (result.ORDER_NO_IN_CHUUBAN != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }
    }
}
