using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PoAndReceiptHistoryPopUpController : ControllerBase
    {
        private readonly IPoAndReceiptHistoryPopUpServices _poAndReceiptHistoryPopUpServices;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public PoAndReceiptHistoryPopUpController(IPoAndReceiptHistoryPopUpServices poAndReceiptHistoryPopUpServices, LocalizationServices localization, IAllCommonService commonService)
        {
            _poAndReceiptHistoryPopUpServices = poAndReceiptHistoryPopUpServices;
            _localization = localization;
            _allCommonService = commonService;

        }
        [HttpPost("GetBatchRollFromConfig")]
        public ActionResult GetBatchRollFromConfig()
        {
            int result = _allCommonService.GetBatchRollFromConfig();
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpGet("GetMakerWisePurchaseOrderQtyList")]
        public IActionResult GetMakerWisePurchaseOrderQtyList(int Batch_Roll)
        {
            bool isSuccess = false;
            var result = _poAndReceiptHistoryPopUpServices.GetMakerWisePurchaseOrderQtyList(Batch_Roll);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpGet("GetVendorWisePurchaseOrderQtyList")]
        public IActionResult GetVendorWisePurchaseOrderQtyList(int Batch_Roll)
        {
            bool isSuccess = false;
            var result = _poAndReceiptHistoryPopUpServices.GetVendorWisePurchaseOrderQtyList(Batch_Roll);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
    }
}
