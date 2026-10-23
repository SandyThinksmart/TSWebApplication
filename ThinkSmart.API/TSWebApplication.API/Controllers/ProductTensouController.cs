using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductTensouController : ControllerBase
    {

        private readonly IProductTensouService _productTensouService;
        private readonly IAllCommonService _commonService;
        public ProductTensouController(IProductTensouService IProductTensouService, IAllCommonService commonService)
        {
            _productTensouService = IProductTensouService;
            _commonService = commonService;
        }

        [HttpPost("GetVehicleName")]
        public ActionResult GetVehicleName()
        {
            var result = _productTensouService.GetVehicleName();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetForwordingLocation")]
        public ActionResult GetForwordingLocation()
        {
            var result = _productTensouService.GetForwordingLocation();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetLocation")]
        public ActionResult GetLocation(string USER_ID)
        {
            var result = _productTensouService.GetTensouLocation(Convert.ToInt32(USER_ID));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            string nextDayDate = dateTime.AddDays(1).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            return Ok(new { IsSuccess = true, message = new { SysDate = sysDate, NextDayDate = nextDayDate } });
        }

        [HttpPost("GetProductMasterData")]
        public IActionResult GetProductMasterDataList([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _productTensouService.GetProductMasterDataList(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetProductDetailsByProductCD")]
        public ActionResult GetProductDetailsByProductCD(int PRODUCT_CD)
        {
            var result = _productTensouService.GetProductDetailsByProductCD(Convert.ToString(PRODUCT_CD));
            return Ok(new { IsSuccess = true, message = result });
        }

        //Search data
        [HttpPost("TensouDataSearchInGrid")]
        public IActionResult TensouDataSearchInGrid([FromBody] GridSearchFields request)
        {
            var result = _productTensouService.GetProductDataSearchList(request);
            return Ok(new { IsSuccess = true, message = result });

        }

        [HttpPost("SaveProductTensouDetails")]
        public ActionResult SaveWIPTensouDetails(List<WIPTensouDetails> request)
        {
            var result = _productTensouService.SaveProductTensouDetails(request);
            return Ok(new { IsSuccess = true, message = result });
            
        }

        //Delete Data
        [HttpPost("ProductTensouDetailsDelete")]
        public IActionResult ProductTensouDetailsDelete(List<WIPTensouDetails> request)
        {
            //int result = ;//_productTensouService.ProductTensouDetailsDelete(request);
            return null;// Json(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetValidShukkaCD")]
        public ActionResult GetValidShukkaCD(List<ProductTensouDetails> lstGridData)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;

            string shukkaCD = string.Empty;
            if (lstGridData != null && lstGridData.Any())
            {
                foreach (var item in lstGridData)
                {
                    var SHUKKA_INSTRUCTION_CD = lstGridData.Where(x => x.SHUKKA_INSTRUCTION_CD > 0).Select(x => x.SHUKKA_INSTRUCTION_CD).ToList();
                    shukkaCD = string.Join(",", SHUKKA_INSTRUCTION_CD);
                }
            }

            var result = _productTensouService.GetValidShukkaCD(shukkaCD);
            return Ok(new { IsSuccess = true, message = result });

        }

        
        [HttpPost("GetShukkaProductDetails")]
        public ActionResult GetShukkaProductDetails(string SHUKKA_INSTRUCTION_CD)
        {
            var result = _productTensouService.GetShukkaProductDetails(SHUKKA_INSTRUCTION_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

    }
}
