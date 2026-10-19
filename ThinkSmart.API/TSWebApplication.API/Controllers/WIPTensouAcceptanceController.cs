using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WIPTensouAcceptanceController : ControllerBase
    {
        private readonly IWIPTensouAcceptanceService _IWIPTensouAcceptanceService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;

        public WIPTensouAcceptanceController(IWIPTensouAcceptanceService WIPTensouAcceptanceService, LocalizationServices localization, IAllCommonService commonService)
        {

            _IWIPTensouAcceptanceService = WIPTensouAcceptanceService;
            _localization = localization;
            _commonService = commonService;

        }

        [HttpGet("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Accepting_Transfer"))));
            return Ok(new { IsSuccess = true, message = result });
         
        }

        [HttpPost("GetAcceptanceLocation")]
        public ActionResult GetAcceptanceLocation(int USER_ID)
        {
            var result = _IWIPTensouAcceptanceService.GetAcceptanceLocation(USER_ID);
            return Ok(new { IsSuccess = true, message = result });
        }


        //Search Tensou Acceptance data
        [HttpPost("GetTensouAcceptanceDetailsByTransferCD")]
        public IActionResult GetTensouAcceptanceDetailsByTransferCD(string tensouCD, string CultureName)
        {
            var result = _IWIPTensouAcceptanceService.GetWIPTransferAcceptanceDetailsByTrasferCD(tensouCD, CultureName);
            return Ok(new { IsSuccess = true, message = result });
        }

        //GetValidTensouNo
        [HttpPost("GetValidTensouNo")]
        public IActionResult GetValidTensouNo(List<WIPTensouDetails> lstGridData)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;
            //string[] WIPTensouCD;
            string trasferCD = string.Empty;
            if (lstGridData != null && lstGridData.Any())
            {
                foreach (var item in lstGridData)
                {
                    var WIP_TRANSFER_CD = lstGridData.Where(x => x.WIP_TRANSFER_CD > 0).Select(x => x.WIP_TRANSFER_CD).ToList();
                    trasferCD = string.Join(",", WIP_TRANSFER_CD);
                }
            }

            var result = _IWIPTensouAcceptanceService.GetValidTensouNo(trasferCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        //set today date
        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }

        //Added Date Range
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

        //GetCompleteChuuban
        [HttpPost("GetCompleteChuuban")]
        public IActionResult GetCompleteChuuban(string completeTransferNo)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;
            string trasferCD = string.Empty;
           
            var result = _IWIPTensouAcceptanceService.GetCompleteChuuban(completeTransferNo);
            return Ok(new { IsSuccess = true, message = result });

        }
        //TransferNoAcceptedORNOT
        [HttpPost("TransferNoAccepted")]
        public IActionResult TransferNoAccepted(string transferNoAccepted)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;
            string trasferCD = string.Empty;

            var result = _IWIPTensouAcceptanceService.TransferNoAccepted(transferNoAccepted);
            return Ok(new { IsSuccess = true, message = result });


        }

        //save Data
        //[HttpPost("SaveWIPTensouAcceptanceDetails")]
        //public ActionResult SaveWIPTensouAcceptanceDetails([FromBody] WIPTensouDetails request)
        //{
        //    var result = _IWIPTensouAcceptanceService.SaveWIPTensouAcceptanceDetails(request);
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        [HttpPost("SaveWIPTensouAcceptanceDetails")]
        public ActionResult SaveWIPTensouAcceptanceDetails(List<WIPTensouDetails> lstGridData)
        {

            List<WIPTensouDetails> wip = new List<WIPTensouDetails>();
            if (lstGridData != null && lstGridData.Any())
            {
                foreach (var item in lstGridData)
                {
                      var result = _IWIPTensouAcceptanceService.SaveWIPTensouAcceptanceDetails(item);
                       wip.Add( result);
                     
                }
            }
            return Ok(new { IsSuccess = true, message = wip }); ;

        }

        private IActionResult Json(object value)
        {
            return new JsonResult(value);
        } 
        //Delete Data
        [HttpPost("WIPTensouAcceptanceDetailsDelete")]
        public IActionResult WIPTensouAcceptanceDetailsDelete(List<WIPTensouDetails> lstGridData)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;
            
            string WIPTransferCD = string.Empty;
            if (lstGridData != null && lstGridData.Any())
            {
                foreach (var item in lstGridData)
                {
                  //  var WIP_TRANSFER_CD = lstGridData.Where(x => x.WIP_TRANSFER_CD > 0).Select(x => x.WIP_TRANSFER_CD).ToList();
                 //   WIPTransferCD = string.Join(",", WIP_TRANSFER_CD);
                 //   item.WIP_TRANSFER_CD = WIP_TRANSFER_CD;
                    _IWIPTensouAcceptanceService.WIPTensouAcceptanceDetailsDelete(lstGridData);
                   
                }
            }

            return Json(new { IsSuccess = true, message = "" });
        }

    }
}


    

