
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeneralDailyProcessController : ControllerBase
    {
        private readonly IGeneralDailyProcessService _generalDailyProcessService;
        private readonly IAllCommonService _commonService;
        public GeneralDailyProcessController(IGeneralDailyProcessService generalDailyProcessService, IAllCommonService commonService)
        {
            _generalDailyProcessService = generalDailyProcessService;
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
        
        [HttpPost("GetGeneralDailyProcessData")]
        public IActionResult GetGeneralDailyProcessData([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.GetGeneralDailyProcessData(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpPost("CheckValidationsOnGeneralProcessStart")]
        public IActionResult CheckValidationsOnGeneralProcessStart(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.CheckValidationsOnGeneralProcessStart(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("checkTabaValidationForOkDecision")]
        public IActionResult checkTabaValidationForOkDecision(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.checkTabaValidationForOkDecision(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("GetProcessKubunList")]
        public IActionResult GetProcessKubunList()
        {
            var result = _generalDailyProcessService.GetProcessKubunList();
            var removeIds = new List<int> { 140, 999 , 900 , 901 };//Remove Kubun ClientVisit[july 2025]
            result = result.Where(x => !removeIds.Contains(x.KUBUN_ID)).ToList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetGeneralProcessOnFactoryDailyWorkStatus")]
        public IActionResult GetGeneralProcessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.GetGeneralProcessOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("CheckValidationsForProcessOperatorMachineCDOnEntry")]
        public IActionResult CheckValidationsForProcessOperatorMachineCD(GeneralDailyProcessDetails processDetails)
        {
           GeneralDailyProcessDetails data = new GeneralDailyProcessDetails();
            data = _generalDailyProcessService.CheckValidationsForProcessOperatorMachineCD(processDetails);
            return Ok(new { IsSuccess = true, data = data });
        }
        [HttpPost("getRemarksOfProcess")]
        public IActionResult getRemarksOfProcess(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.getRemarksOfProcess(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("saveGeneralProcessPlanEntryData")]
        public IActionResult saveGeneralProcessPlanEntryData([FromBody] GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.saveGeneralProcessPlanEntryData(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
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
        [HttpPost("CheckValidationsOnGeneralProcessEnd")]
        public IActionResult CheckValidationsOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.CheckValidationsOnGeneralProcessEnd(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("getGeneralDailyProcessStartData")]
        public IActionResult getGeneralDailyProcessStartData(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.getGeneralDailyProcessStartData(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess(GeneralDailyProcessDetails processDetails)
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt16(processDetails.PROCESS_KUBUN_CD));
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("CheckValidationsForOperatorAndEndDateTimeOnEnd")]
        public IActionResult CheckValidationsForOperatorAndEndDateTime(GeneralDailyProcessDetails processDetails)
        {
            GeneralDailyProcessDetails data = new GeneralDailyProcessDetails();
            data = _generalDailyProcessService.CheckValidationsForOperatorAndEndDateTime(processDetails);
            return Ok(new { IsSuccess = true, data = data });
        }
        [HttpPost("getRemarksForEndProcess")]
        public IActionResult getRemarksForEndProcess(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.getRemarksForEndProcess(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("updateGeneralProcessPlanEndData")]
        public IActionResult updateGeneralProcessPlanEndData([FromBody] GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.updateGeneralProcessPlanEndData(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        //[HttpPost("GetUnitType")]
        //public IActionResult GetUnitType()
        //{
        //    var result = _generalDailyProcessService.GetUnitTypeList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        [HttpPost("GetProcessDefectList")]
        public IActionResult GetProcessDefectList(int? KUBUN_ID)
        {
            bool isSuccess = false;
            var result = _commonService.GetProcessDefectList(Convert.ToInt32(KUBUN_ID));

            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("saveGeneralProcessPlanEntryEndData")]
        public IActionResult saveGeneralProcessPlanEntryEndData([FromBody] GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.saveGeneralProcessPlanEntryEndData(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetUnitsWeightOnGeneralProcessEnd")]
        public IActionResult GetUnitsWeightOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.GetUnitsWeightOnGeneralProcessEnd(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("GetProcessNameOnMergeScreen")]
        public IActionResult GetProcessNameOnMergeScreen(short PROCESS_KUBUN_CD)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.GetProcessNameOnMergeScreen(PROCESS_KUBUN_CD);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("CheckValidationsProcessMachineOprOnCancel")]
        public IActionResult CheckValidationsProcessMachineOprOnCancel(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.CheckValidationsProcessMachineOprOnCancel(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("DeleteGeneralProcessPlanData")]
        public IActionResult DeleteGeneralProcessPlanData([FromBody] GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.DeleteGeneralProcessPlanData(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetRowCountForGeneralDailyProcessFactory")]
        public ActionResult GetRowCountForGeneralDailyProcessFactory(GridSearchFields gridSearchRequest)
        {
            int count = _generalDailyProcessService.GetRowCountForGeneralDailyProcessFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForGeneralProcessDailyAnalysis")]
        public ActionResult GetRowCountForGeneralProcessDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _generalDailyProcessService.GetRowCountForGeneralProcessDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("CheckValidationsOnGeneralDailyProcessCancel")]
        public IActionResult CheckValidationsOnGeneralDailyProcessCancel(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.CheckValidationsOnGeneralDailyProcessCancel(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
        [HttpPost("ValidateGeneralProcess")]
        public IActionResult ValidateGeneralProcess(short PROCESS_KUBUN_CD)
        {
            bool returnval = false;
            var result = _generalDailyProcessService.ValidateGeneralProcess(PROCESS_KUBUN_CD);
            if (result != 0)
            {
                returnval = true;
            }
            else
            {
                returnval = false;
            }

            return Ok(new { generalProcess = returnval });
        }
        [HttpPost("GetChubbanDetailsForWeight")]
        public IActionResult GetChubbanDetailsForWeight(string PP_CHUUBAN)
        {
            var result = _generalDailyProcessService.GetChubbanDetailsForWeight(PP_CHUUBAN);
            

            return Ok(new { data = result });
        }
        [HttpPost("GetNextThabaForSpecialChuubanNo")]
        public IActionResult GetNextThabaForSpecialChuubanNo(string orderNo)
        {
            var result = _generalDailyProcessService.GetNextThabaForSpecialChuubanNo(orderNo);


            return Ok(new { data = result });
        }
        [HttpPost("GetGeneralProcessDataForCancel")]
        public IActionResult GetGeneralProcessDataForCancel(GeneralDailyProcessDetails processDetails)
        {
            var result = _generalDailyProcessService.GetGeneralProcessDataForCancel(processDetails);


            return Ok(new { data = result });
        }
        [HttpPost("GetProcessPlanData")]
        public IActionResult GetProcessPlanData(GeneralDailyProcessDetails processDetails)
        {
            var result = _generalDailyProcessService.GetProcessPlanData(processDetails);


            return Ok(new { data = result });
        }

        [HttpPost("GetMachineTypeList")]
        public IActionResult GetMachineTypeList(int machineType)
        {
            var result = _generalDailyProcessService.GetMachineTypeList(machineType);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("ValidateChubbanForProcessStart")]
        public IActionResult ValidateChubbanForProcessStart(string PP_CHUUBAN)
        {
            var result = _generalDailyProcessService.ValidateChubbanForProcessStart(PP_CHUUBAN);
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("ValidateChubbanForProcessEnd")]
        public IActionResult ValidateChubbanForProcessEnd(string PP_CHUUBAN)
        {
            var result = _generalDailyProcessService.ValidateChubbanForProcessEnd(PP_CHUUBAN);
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("CheckExistingDataValidation")]
        public IActionResult CheckExistingDataValidation(GeneralDailyProcessDetails processDetails)
        {
            bool isSuccess = false;
            var result = _generalDailyProcessService.CheckExistingDataValidation(processDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = result });
        }
    }
}
