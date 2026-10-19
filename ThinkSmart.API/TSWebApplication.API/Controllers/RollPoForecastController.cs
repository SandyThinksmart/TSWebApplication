using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RollPoForecastController : ControllerBase
    {
        private readonly IRollPoForecastService _rollPoForecastService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
       
        public RollPoForecastController(IRollPoForecastService rollPoForecastService, LocalizationServices localization, IAllCommonService commonService)
        {
            _rollPoForecastService = rollPoForecastService;
            _localization = localization;
            _allCommonService = commonService;
            
        }
        [HttpPost("GetProductMasterData")]
        public IActionResult GetProductMasterDataList([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _rollPoForecastService.GetProductMasterDataList(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetInchargeList")]
        public IActionResult GetInchargeList(string? name, int operatorCD)
        {
           
            //var result = _allCommonService.GetOperatorName(name, operatorCD);
            var result = _allCommonService.GetOperatorDeptName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetCustomerList")]
        public IActionResult GetCustomerList(string? name)
        {
            var result = _allCommonService.GetCustomerName(name);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            DateTimeDetails result = new DateTimeDetails();
            DateTime currentDate = DateTime.Now;
            result.START_DATETIME_STRING = currentDate.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetForecastRoll")]
        public ActionResult GetForecastRoll()
        {
            DateTimeDetails resultData = new DateTimeDetails();
            DateTime currentDate = DateTime.Now;
            resultData.START_DATETIME_STRING= currentDate.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            resultData = _rollPoForecastService.GetPreviousForecastRoll(currentDate);
            return Ok(new { IsSuccess = true, message = resultData });
        }
        [HttpGet("GetProductDetailsByProductCD")]
        public ActionResult GetProductDetailsByProductCD(long PRODUCT_CD)
        {
            var result = _rollPoForecastService.GetProductDetailsByProductCD(PRODUCT_CD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMaterialMasterData")]
        public IActionResult GetMaterialMasterDataList([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _rollPoForecastService.GetMaterialMasterDataList(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpGet("GetMaterialDetailsByMaterialCD")]
        public ActionResult GetMaterialDetailsByMaterialCD(long MATERIAL_CD)
        {
            var result = _rollPoForecastService.GetMaterialDetailsByMaterialCD(MATERIAL_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetSpecInput1Data")]
        public IActionResult GetSpecInput1Data([FromBody] RollSalesForecastDetails request)
        {
            RollSalesForecastDetails rollSalesForecastDetails = new RollSalesForecastDetails();
            var productDetails = _rollPoForecastService.GetSpecInput1DetailsByCustInchrge_CD(request.CUSTOMER_CD, request.EMPLOYEE_CD,request.CURRENT_R,request.CURRENT_YYYY,request.HISTORY_MONTH1,request.HISTORY_MONTH2,request.HISTORY_MONTH3);
            var materialDetails = _rollPoForecastService.GetSpecInput1MaterialDetailsByCustInchrge_CD(request.CUSTOMER_CD, request.EMPLOYEE_CD, request.CURRENT_R, request.CURRENT_YYYY);
            rollSalesForecastDetails.PRODUCT_LIST = productDetails;
            rollSalesForecastDetails.MATERIAL_LIST = materialDetails;
            return Ok(new { IsSuccess = true, message = rollSalesForecastDetails });
        }
        //[HttpPost("GetSpecInput2Data")]
        //public IActionResult GetSpecInput2Data([FromBody] RollSalesForecastDetails request)
        //{
        //    var result = _rollPoForecastService.GetSpecInput2DetailsByCustInchrge_CD(request);
        //    return Ok(new { IsSuccess = true, message = result });
        //}
       
        [HttpPost("SaveProductMaterialForecastDetails")]
        public IActionResult SaveProductMaterialForecastDetails([FromBody] RollSalesForecastDetails request)
        {
            var result = _rollPoForecastService.SaveProductMaterialForecastDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
           

            RollSalesForecastDetails rollSalesForecastDetails = new RollSalesForecastDetails();
            var productListData = _rollPoForecastService.GetProductForecastDetails(request);
            var materialListData = _rollPoForecastService.GetMaterialForecastDetails(request);
            rollSalesForecastDetails.PRODUCT_LIST = productListData;
            rollSalesForecastDetails.MATERIAL_LIST = materialListData;
            return Ok(new { IsSuccess = true, message = rollSalesForecastDetails });
        }
        
    }
}







