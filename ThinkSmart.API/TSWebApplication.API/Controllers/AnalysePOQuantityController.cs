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
    public class AnalysePOQuantityController : ControllerBase
    {
        private readonly IAnalysePOQuantityServices _analysePOQuantityServices;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        public AnalysePOQuantityController(IAnalysePOQuantityServices analysePOQuantityServices, IAllCommonService commonService, LocalizationServices localization)
        {
            _analysePOQuantityServices = analysePOQuantityServices;
            _localization = localization;
            _commonService = commonService;
        }
        [HttpPost("GetMukesakiList")]
        public IActionResult GetMukesakiList()
        {
            var result = _analysePOQuantityServices.GetMukesakiList();
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("GetSupplierList")]
        public IActionResult GetSupplierList()
        {
            var result = _analysePOQuantityServices.GetSupplierList();
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("GetDestinationList")]
        public IActionResult GetDestinationList()
        {
            var result = _analysePOQuantityServices.GetDestinationList();
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("GetHikitorisakiList")]
        public IActionResult GetHikitorisakiList()
        {
            var result = _analysePOQuantityServices.GetHikitorisakiList();
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("GetFutouList")]
        public IActionResult GetFutouList()
        {
            var result = _analysePOQuantityServices.GetFutouList();
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpPost("SaveMaterialPoData")]
        public IActionResult SaveMaterialPoData([FromBody] List<RollPoMainAnalysisDetails>  PODetails)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.SaveMaterialPoData(PODetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetInternalNameWiseMakerList")]
        public IActionResult GetInternalNameWiseMakerList(long INT_MATL_FC_SUMMARY_CD)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.GetInternalNameWiseMakerList(INT_MATL_FC_SUMMARY_CD);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetInternalNameWIseMaterialDetails")]
        public IActionResult GetInternalNameWIseMaterialDetails(long INT_MATL_FC_SUMMARY_CD)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.GetInternalNameWIseMaterialDetails(INT_MATL_FC_SUMMARY_CD);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetMaterialMasterDataOnAnalysePoQty")]
        public IActionResult GetMaterialMasterDataList([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.GetMaterialMasterDataList(gridSearchRequest);
            if (result.Count!= 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetMakerMaterialDetailsByCD")]
        public IActionResult GetMakerMaterialDetailsByCD(long INT_MATL_FC_SUMMARY_CD,long MATERIAL_CD,short MUKESAKI_CD)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.GetMakerMaterialDetailsByCD(INT_MATL_FC_SUMMARY_CD, MATERIAL_CD, MUKESAKI_CD);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetRollPoDetails")]
        public IActionResult GetRollPoDetails(long INT_MATL_FC_SUMMARY_CD)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.GetRollPoDetails(INT_MATL_FC_SUMMARY_CD);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpGet("GetMaterialDetailsByMaterialCD")]
        public ActionResult GetMaterialDetailsByMaterialCD(long MATERIAL_CD, short? MUKESAKI_CD)
        {
            var result = _analysePOQuantityServices.GetMaterialDetailsByMaterialCD(MATERIAL_CD, MUKESAKI_CD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("checkAnalysisOverFlag")]
        public ActionResult checkAnalysisOverFlag(long INT_MATL_FC_SUMMARY_CD)
        {
            var result = _analysePOQuantityServices.checkAnalysisOverFlag(INT_MATL_FC_SUMMARY_CD);
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpGet("checkPoCompleteFlag")]
        public ActionResult checkPoCompleteFlag()
        {
            var result = _analysePOQuantityServices.checkPoCompleteFlag();
            return Ok(new { IsSuccess = true, data = result });
        }
        [HttpGet("GetMukesakiMaterialRate")]
        public ActionResult GetMukesakiMaterialRate(short mukesakiCd, decimal matlDimD, int matlKubun, int matlNameMakerCd)
        {
            var result = _analysePOQuantityServices.GetMukesakiMaterialRate(mukesakiCd, matlDimD, matlKubun, matlNameMakerCd);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("UpdateForecastData")]
        public IActionResult UpdateForecastData([FromBody] List<RollSalesForecastDetails> forecastDetails)
        {
            bool isSuccess = false;
            var result = _analysePOQuantityServices.UpdateForecastData(forecastDetails);
            isSuccess = true;
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
    }
}
