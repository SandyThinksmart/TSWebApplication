using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class CustomerWiseProdStockShukkaController : ControllerBase
    {
        private readonly ICustomerWiseProdStockShukkaService _customerWiseProdStockShukkaService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public CustomerWiseProdStockShukkaController(ICustomerWiseProdStockShukkaService CustomerWiseProdStockShukkaService, LocalizationServices localization, IAllCommonService commonService)
        {
            _customerWiseProdStockShukkaService = CustomerWiseProdStockShukkaService;
            _localization = localization;
            _allCommonService = commonService;
        }

        [HttpPost("GetCustomerDetails")]
        public IActionResult GetCustomerDetails(string customerCD)
        {
            bool isSuccess = false;
            var result = _customerWiseProdStockShukkaService.GetCustomerDetails(customerCD);
             if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.TOTAL = item.ORD_REM_STOCK + item.TOT_PROD_STOCK; 
                }
                    isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetDispatchQtyCustomerWise")]
        public IActionResult GetDispatchQtyCustomerWise(string customerCD)
        {

            bool returnval = false;
            var result = _customerWiseProdStockShukkaService.GetDispatchQtyCustomerWise(customerCD);
            if (result != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }

        [HttpPost("GetProdStockQtyCustomerWise")]
        public IActionResult GetProdStockQtyCustomerWise(string customerCD)
        {
            bool returnval = false;
            var result = _customerWiseProdStockShukkaService.GetProdStockQtyCustomerWise(customerCD);
            if (result != null)
            {
                returnval = true;
            }
            return Ok(new { IsSuccess = returnval, message = result });
        }
        
        [HttpPost("GetPendingStockQtyCustomerWise")]
        public IActionResult GetPendingStockQtyCustomerWise(string customerCD)
        {
            bool returnval = false;
            var result = _customerWiseProdStockShukkaService.GetPendingStockQtyCustomerWise(customerCD);
            if (result != null)
            {
                returnval = true;
            }
            return Ok(new { IsSuccess = returnval, message = result });
        }

        [HttpGet("SetGridHeaderWithMonth")]
        public IActionResult SetGridHeaderWithMonth()
        {
            DateTime firstRowDate = DateTime.Now;
            DateTime secondRowDate = DateTime.Now;
            firstRowDate = firstRowDate.AddMonths(-1);
            string lastYearEndDate = firstRowDate.Year + "/" + firstRowDate.Month.ToString("00");

            firstRowDate = firstRowDate.AddYears(-1).AddMonths(+1); ;
            string lastYearStartDate = firstRowDate.Year + "/" + firstRowDate.Month.ToString("00");

            secondRowDate = secondRowDate.AddYears(-1).AddMonths(-1); ;
            string lastToLastYearEndDate = secondRowDate.Year + "/" + secondRowDate.Month.ToString("00");

            secondRowDate = secondRowDate.AddYears(-1).AddMonths(+1);
            string lastToLastYearStartDate = secondRowDate.Year + "/" + secondRowDate.Month.ToString("00");


            string month1 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month2 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month3 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month4 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month5 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month6 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month7 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month8 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month9 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month10 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month11 = Convert.ToString(firstRowDate.Month.ToString("00"));
            firstRowDate = firstRowDate.AddMonths(+1);
            string month12 = Convert.ToString(firstRowDate.Month.ToString("00"));
            var result = (new
            {
                LAST_YEAR_START_DATE = lastYearStartDate,
                LAST_YEAR_END_DATE = lastYearEndDate,
                LASTTOLAST_YEAR_START_DATE = lastToLastYearStartDate,
                LASTTOLAST_YEAR_END_DATE = lastToLastYearEndDate,
                Month_1 = month1,
                Month_2 = month2,
                Month_3 = month3,
                Month_4 = month4,
                Month_5 = month5,
                Month_6 = month6,
                Month_7 = month7,
                Month_8 = month8,
                Month_9 = month9,
                Month_10 = month10,
                Month_11 = month11,
                Month_12 = month12

            });
            return Ok(new { message = result });
        }

    }
}
