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
    public class SalesProductionStatusSearchController : Controller
    {
        private readonly ISalesProductionStatusSearchService _SalesProductionStatusSearchService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;

        public SalesProductionStatusSearchController(ISalesProductionStatusSearchService salesProductionStatusSearchService, LocalizationServices localization, IAllCommonService commonService)
        {
            _localization = localization;
            _SalesProductionStatusSearchService = salesProductionStatusSearchService;
            _commonService = commonService;
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime(int isChecked)
        {
            DateTimeDetails result = new DateTimeDetails();
            DateTime dateTime = _commonService.GetSystemDateTime();
            result.END_DATETIME_STRING = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            //if (isChecked==1)
            //{
                dateTime = dateTime.AddMonths(-12);
                dateTime = dateTime.AddDays(1);
            //}
            //else
            //{
            //    dateTime = dateTime.AddMonths(-6);
            //    dateTime = dateTime.AddDays(1);
            //}
                
            result.START_DATETIME_STRING = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _SalesProductionStatusSearchService.GetSalesProductionStatusList(request);
            return Json(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("getSalesProdStatusByProcessPlanCD")]
        public ActionResult GetSalesProdStatusByProcessPlanCD([FromBody] ProcessPlanForSalesProductionStatusDetails request)
        {
            ProcessPlanForSalesProductionStatusDetails result = new ProcessPlanForSalesProductionStatusDetails();
            result = _SalesProductionStatusSearchService.getSalesProdStatusByProcessPlanCD(request.PP_PROCESS_PLAN_CD, request.CULTURE_NAME);

            var getResult = _SalesProductionStatusSearchService.getTimeAndBundlewiseByProcessPlanCD(request.PP_PROCESS_PLAN_CD, request.CULTURE_NAME);
            var getProcessDataResult = _SalesProductionStatusSearchService.getProcessDataByProcessPlanCD(request.PP_PROCESS_PLAN_CD);
            var getHardnessAndPostHTResult = _SalesProductionStatusSearchService.getHardnessAndPostHTByProcessPlanCD(request.PP_PROCESS_PLAN_CD);


            if (getResult !=null)
            {
                result.GETDATATIMEWISE = getResult.GETDATATIMEWISE;
                result.GETDATABUNDLEWISE = getResult.GETDATABUNDLEWISE;

            }
           
            if (getHardnessAndPostHTResult.Count > 0)
            {
                result.GETDATAHARDNESSANDPOSTHT = getHardnessAndPostHTResult;
            }
            if (getProcessDataResult.Count > 0)
            {
                result.GETPROCESSDATA = getProcessDataResult;
            }

          
                return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("getMessageListByProcessPlanCD")]
        public ActionResult GetMessageListByProcessPlanCD(long processPlanCD)
        {
            List<ProcessPlanForSalesProductionStatusDetails> result = new List<ProcessPlanForSalesProductionStatusDetails>();
            result = _SalesProductionStatusSearchService.getMessageDetailsByProcessPlanCD(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("getFIFailureRateByProcessPlanCD")]
        public ActionResult GetFIFailureRateByProcessPlanCD(long processPlanCD)
        {
            var result = _SalesProductionStatusSearchService.getFIFailureRateByProcessPlanCD(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("getECDefectRateByProcessPlanCD")]
        public ActionResult GetECDefectRateByProcessPlanCD(long processPlanCD)
        {
            var result = _SalesProductionStatusSearchService.getECDefectRateByProcessPlanCD(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("getWIPByProcessPlanCD")]
        public ActionResult GetWIPByProcessPlanCD(long processPlanCD)
        {
            var result = _SalesProductionStatusSearchService.getWIPByProcessPlanCD(processPlanCD);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("MessageSave")]
        public IActionResult MessageSave([FromBody] ProcessPlanForSalesProductionStatusDetails request)
        {
            _SalesProductionStatusSearchService.MessageDetailsSave(request);
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpPost("MessageUpdate")]
        public IActionResult MessageUpdate([FromBody] ProcessPlanForSalesProductionStatusDetails request)
        {
            _SalesProductionStatusSearchService.MessageDetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = "" });
        }
        [HttpPost("MessageDelete")]
        public IActionResult MessageDelete(long MSG_CD)
        {
            _SalesProductionStatusSearchService.MessageDetailsDelete(MSG_CD);
            return Ok(new { IsSuccess = true, message = "" });
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
    }
}
