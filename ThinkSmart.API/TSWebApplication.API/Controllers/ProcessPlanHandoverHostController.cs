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
    public class ProcessPlanHandoverHostController : Controller
    { private readonly IProcessPlanHandoverHostService _processPlanHandoverHostService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        public ProcessPlanHandoverHostController(IProcessPlanHandoverHostService processPlanHandoverHostService, IAllCommonService commonService, LocalizationServices localization)
        {
            _processPlanHandoverHostService = processPlanHandoverHostService;
            _localization = localization;
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
        [HttpPost("ValidateChuuban")]
        public IActionResult ValidateChuuban(List<GridSearchFields> processPlanDetails)
        {
            string message = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverHostService.ValidateChuuban(processPlanDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }
        [HttpPost("GetProcessPlanDetails")]
        public IActionResult GetProcessPlanDetails(List<GridSearchFields> processPlanDetails)
        {
            string message = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverHostService.GetProcessPlanDetails(processPlanDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }
        [HttpPost("SaveProcessPlanHandoverData")]
        public IActionResult SaveProcessPlanHandoverData([FromBody] List<GridSearchFields> processPlanDetails)
        {
            string result = _processPlanHandoverHostService.SaveProcessPlanHandoverData(processPlanDetails);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("DeleteProcessPlanHandoverData")]
        public IActionResult DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)

        {
            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverHostService.DeleteProcessPlanHandoverData(processPlanDetails);
            return Ok(new { IsSuccess = true, data = data });
        }
        
        [HttpPost("ValidateDataExist")]
        public IActionResult ValidateDataExist(List<GridSearchFields> processPlanDetails)
        {
            string message = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverHostService.ValidateDataExist(processPlanDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess(GeneralDailyProcessDetails processDetails)
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt16(processDetails.PROCESS_KUBUN_CD));
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("ValidateDataForUpdate")]
        public IActionResult ValidateDataForUpdate(List<GridSearchFields> processPlanDetails)
        {
            string message = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverHostService.ValidateDataForUpdate(processPlanDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }
        [HttpPost("ValidateDataForAddNewBtn")]
        public IActionResult ValidateDataForAddNewBtn(List<GridSearchFields> processPlanDetails)
        {
            string message = string.Empty;
            bool isSuccess = false;

            List<GridSearchFields> data = new List<GridSearchFields>();
            data = _processPlanHandoverHostService.ValidateDataForAddNewBtn(processPlanDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, data = data });
        }
        [HttpPost("UpdateProcessPlanHandoverData")]
        public IActionResult UpdateProcessPlanHandoverData([FromBody] List<GridSearchFields> processPlanDetails)
        {
            string result = _processPlanHandoverHostService.UpdateProcessPlanHandoverData(processPlanDetails);
            return Ok(new { IsSuccess = true, message = result });
        }
        

    }
}
