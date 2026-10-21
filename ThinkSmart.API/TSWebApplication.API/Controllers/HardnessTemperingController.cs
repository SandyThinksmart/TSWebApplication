using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HardnessTemperingController : ControllerBase
    {
        private readonly LocalizationServices _localization;
        private readonly IHardnessTemperingService _hardnessTemperingService;
        private readonly IAllCommonService _commonService;
        
        public HardnessTemperingController(LocalizationServices localization, IHardnessTemperingService hardnessTemperingService, IAllCommonService commonService)
        {
            _localization = localization;
            _hardnessTemperingService = hardnessTemperingService;
            _commonService = commonService;
        }
        [HttpPost("IsValidDateRange")]
        public IActionResult IsValidDateRange(DateTimeDetails request)
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
        [HttpPost("GetHardnessTemperingDetailsByChuubanGyoban")]
        public IActionResult GetHardnessTemperingDetailsByChuubanGyoban([FromBody] GridSearchFields request)
        {
            var result = _hardnessTemperingService.GetHardnessTemperingDetailsByChuubanGyoban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetHardnessTemperingDetails")]
        public IActionResult GetHardnessTemperingDetails([FromBody] GridSearchFields request)
        {
           
            var result = _hardnessTemperingService.GetHardnessTemperingDetails(request.FURNACE_NO, request.CHUUBAN, request.GYOUBAN,Convert.ToInt16(request.USER_ID));

            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHardnessTemperingTempDetailsByUser")]
        public IActionResult GetHardnessTemperingTempDetailsByUser(int userID)
        {
            var result = _hardnessTemperingService.GetHardnessTemperingTempDetailsByUser(userID);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHTQualityDetailsByCD")]
        public IActionResult GetHTQualityDetailsByCD(long HTQualityCD)
        {
            var result = _hardnessTemperingService.GetHTQualityDetailsByCD(HTQualityCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHardeningTemperingData")]
        public IActionResult GetHardeningTemperingData([FromBody] GridSearchFields request)
        {
           
            int getCount = _hardnessTemperingService.GetFurnaceDetails(request.FURNACE_NO);

            if (getCount > 0)
            {
                var result = _hardnessTemperingService.GetHardnessTemperingDetails(request.FURNACE_NO, request.CHUUBAN, request.GYOUBAN, 0);
                return Ok(new { IsSuccess = true, message = result });
            }else
            {
                return Ok(new { IsSuccess =  false});
            }
           
        }

        [HttpPost("InsertHardnessTemperingDetails")]
        public IActionResult InsertHardnessTemperingDetails([FromBody] HardeningTemperingDetails request)
        {
            _hardnessTemperingService.InsertHardnessTemperingDetails(request);
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpPost("UpdateHardnessTemperingDetails")]
        public IActionResult UpdateHardnessTemperingDetails([FromBody] HardeningTemperingDetails request)
        {
           long msgCD = _hardnessTemperingService.UpdateHardnessTemperingDetails(request);
            return Ok(new { IsSuccess = true, message = msgCD });
        }
        

        [HttpPost("UpdateHTQualityDetails")]
        public IActionResult UpdateHTQualityDetails([FromBody] HardeningTemperingDetails request)
        {
            long msgCD = 0;
            msgCD = _hardnessTemperingService.UpdateHTQualityDetails(request);
            return Ok(new { IsSuccess = true, message = msgCD });
        }
        
        [HttpGet("DeleteHTQualityDetails")]
        public IActionResult DeleteHTQualityDetails(long htQualityCD, long? msgCD)
        {
            _hardnessTemperingService.DeleteHTQualityDetails(htQualityCD, msgCD);
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpPost("DeleteHardnessTemperingDetails")]
        public IActionResult DeleteHardnessTemperingDetails(HardeningTemperingDetails request)
        {
            _hardnessTemperingService.DeleteHardnessTemperingDetails(request);
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpPost("InsertHardnessTemperingTempDetails")]
        public IActionResult InsertHardnessTemperingTempDetails([FromBody] List<HardeningTemperingDetails> request)
        {
             _hardnessTemperingService.InsertHardnessTemperingTempDetails(request);
              return Ok (new { IsSuccess = true, message = "" });
        }

        [HttpPost("InsertHTQualityDetails")]
        public IActionResult InsertHTQualityDetails([FromBody] HardeningTemperingDetails request)
        {
            _hardnessTemperingService.InsertHTQualityDetails(request);
            return Ok(new { IsSuccess = true, message = "" });
        }

       [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _hardnessTemperingService.GetHardnessTemperingList(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("EntryEditDeleteGridSearch")]
        public IActionResult EntryEditDeleteGridSearch([FromBody] GridSearchFields request)
        {
            var result = _hardnessTemperingService.GetHTQualityList(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHardnessTemperingDetailsByHardnessTemperingCD")]
        public IActionResult GetHardnessTemperingDetailsByHardnessTemperingCD(long hardnessTemperingCD)
        {
            var result = _hardnessTemperingService.GetHardnessTemperingDetailsByHardnessTemperingCD(hardnessTemperingCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetHardnessTempringOnFactoryDailyWorkStatus")]
        public IActionResult GetHardnessTempringOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _hardnessTemperingService.GetHardnessTempringOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetHardnessTemperingDetailsOnDailyProcessAnalysis")]
        public IActionResult GetHardnessTemperingDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _hardnessTemperingService.GetHardnessTemperingDetailsOnDailyProcessAnalysis(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }

        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList(string machineType)
        {
            var result = _commonService.GetMachineName(machineType);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("CheckFurnaceIsExist")]
        public IActionResult CheckFurnaceIsExist(string furnaceNo)
        {
            bool isSuccess = false;

            int getCount = _hardnessTemperingService.GetFurnaceDetails(furnaceNo);
            if (getCount == 0)
            {
                isSuccess = true;
            }

            return Ok(new { IsSuccess = isSuccess, message = "" });
        }

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(string? yobina)
        {
            var result = _commonService.GetCustomerName(yobina);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("EmployeeList")]
        public IActionResult EmployeeList(string? yobina, int operatorCD)
        {
            var result = _commonService.GetOperatorName(yobina, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("CheckFurnaceIsAlreadyEntered")]
        public IActionResult CheckFurnaceIsAlreadyEntered(string furnaceNo)
        {

            HardeningTemperingDetails details = new HardeningTemperingDetails();
            details = _hardnessTemperingService.FurnaceIsAlreadyEntered(furnaceNo);
           

            return Ok(new { message = details });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_Refining"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForHardnessTemperingtFactory")]
        public ActionResult GetRowCountForHardnessTemperingtFactory(GridSearchFields gridSearchRequest)
        {
            int count = _hardnessTemperingService.GetRowCountForHardnessTemperingtFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForHardnessTemperingtDailyAnalysis")]
        public ActionResult GetRowCountForHardnessTemperingDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _hardnessTemperingService.GetRowCountForHardnessTemperingDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        
       [HttpPost("GetThabaDetailsByProcessPlan")]
        public ActionResult GetThabaDetailsByProcessPlan(int processPlanCD)
        {
            var result  = _hardnessTemperingService.GetThabaDetailsByProcessPlan(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("IsProcessPlanAlreadyExist")]
        public ActionResult IsProcessPlanAlreadyExist(GridSearchFields request)
        {
            bool isSaved = _hardnessTemperingService.IsProcessPlanAlreadyExist(request);
            return Ok(new { IsSuccess = isSaved });
        }
    }
}
