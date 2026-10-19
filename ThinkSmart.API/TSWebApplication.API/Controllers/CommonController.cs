using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        public CommonController(IAllCommonService commonService, LocalizationServices localization)
        {
            _commonService = commonService;
            _localization = localization;
        }

        /// <summary>
        /// chuuban gyoban mapping barcode.
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns>Created by shobhana[2024]</returns>
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

       
        /// <summary>
        /// Mapping Nifuda By Barcode.
        /// </summary>
        /// <param name="nifudaNo"></param>
        /// <returns>Created by shobhana[2024]</returns>
        [HttpPost("MappNifudaByBarcode")]
        public IActionResult MappNifudaByBarcode(string nifudaNo)
        {
            bool returnval = false;
            var result = _commonService.MappNifudaByBarcode(nifudaNo);

            if (result.NIFUDA_NO != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }

        [HttpPost("SaveWIPDataTabaWise")]
        public ActionResult SaveWIPDataTabaWise([FromBody] WipStatusDetails request)
        {
            var result = _commonService.SaveWIPDataTabaWise(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("SaveWIPDataCombineTaba")]
        public ActionResult SaveWIPDataCombineTaba([FromBody] WipStatusDetails request)
        {
            var result = _commonService.SaveWIPDataCombineTaba(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        /// <summary>
        /// One Record.
        /// Method call - Insert WIP Status Data.
        /// Insert record in TRN_WIP_STATUS table.
        /// Conditions 1)DECISION ok then Delete record in TRN_WIP_STATUS table process plan & taba agains.
        /// 2)DECISION NG - then Insert Record TRN_WIP_STATUS table
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Creted by shobhana[2024]</returns>
        [HttpPost("InsertWIPStatusData")]
        public ActionResult InsertWIPStatusData([FromBody] WipStatusDetails request)
        {
            if (request.DECISION != 10)
            {
                var result = _commonService.InsertWIPStatusData(request);
                return Ok(new { IsSuccess = true, message = result });
            }
            else {
                
                    var result = _commonService.DeleteWIPStatusDataPreProcess(request);
                    return Ok(new { IsSuccess = true, message = result });
                
            }

        }

        /// <summary>
        /// Multiple Record data.
        /// Method call - save FI Bulk Without Nyuko time.
        /// Insert record in TRN_WIP_STATUS table.
        /// Conditions 1)DECISION ok then Delete record in TRN_WIP_STATUS table process plan & taba agains.
        /// 2)DECISION NG - then Insert Record TRN_WIP_STATUS table
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Created by shobhana[Nov 2024]</returns>
        [HttpPost("InsertFIBulkWIPStatusData")]
        public ActionResult InsertFIBulkWIPStatusData([FromBody] List<WipStatusDetails> request)
        {
            var results = new List<object>();
            foreach (var item in request) {

                if (item.PROCESS_KUBUN_CD == 0)
                {
                    item.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDistortion_Inspection")));
                }

                if (request.Count == 1)
                {
                    item.TABA_GROUP_NO = null;
                  
                }

            if (item.DECISION != 10)
            {
                var result= _commonService.InsertFIBulkWIPStatusData(item);
                    results.Add(new { IsSuccess = true, Message = result });
                }
            else
            {
                 var result = _commonService.DeleteWIPStatusDataPreProcess(item);
                    results.Add(new { IsSuccess = true, Message = result });
                }

        }
            return Ok(new { IsSuccess = true, Results = results });

        }

        /// <summary>
        /// latest process delete and previous condition as it is restore in TRN_WIP_STATUS table.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DeleteWIPStatusData")]
        public ActionResult DeleteWIPStatusData([FromBody] WipStatusDetails request)
        {
                var result = _commonService.DeleteWIPStatusData(request);
                return Ok(new { IsSuccess = true, message = result });
           
        }

        /// <summary>
        /// Created By Sandeep Hajare on 2025/01/03
        /// Maker List Bind in Dropdown with  MST_Master table.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GeMatlMakerList")]
        public IActionResult GeMatlMakerList()
        {
            var result = _commonService.GetMaterialMakerName();
            return Ok(new { IsSuccess = true, message = result });
        }

        /// <summary>
        /// Created By Sandeep Hajare on 2025/02/13
        /// FTP Detail Get with  MST_Configration table.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetFTPDetail")]
        public IActionResult GetFTPDetail()
        {
            var result = _commonService.GetFTPDetail();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetAllOperatorsList")]
        public IActionResult GetAllOperatorsList()
        {
            var result = _commonService.GetAllOperatorsList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetCustomerList")]
        public IActionResult GetCustomerList(string? name = null, int? customerCD = 0)
        {
            var result = _commonService.GetCustomerList(name,Convert.ToInt32(customerCD));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMukesakiList")]
        public IActionResult GetMukesakiList(string? name = null, int mukesakiCD = 0)
        {            
            var result = _commonService.GetMukesakiName(name, mukesakiCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("IsEndDateTimeGreaterThanStartDateTimeByTenMin")]
        public IActionResult IsEndDateTimeGreaterThanStartDateTimeByTenMin(string startDateTime, string endDateTime, int minutes)
        {
            bool returnVal=false;
            DateTime startDate = Convert.ToDateTime(startDateTime);
            DateTime endDate = Convert.ToDateTime(endDateTime);

            if (endDate >= startDate.AddMinutes(minutes))
            {
                returnVal = true;
            }
          
            return Ok(new { IsSuccess = returnVal });
        }

        [HttpGet("GetChuubanGyuobanByNifuda")]
        public IActionResult GetChuubanGyuobanByNifuda(string nifudaNo)
        {
            var result = _commonService.GetChuubanGyuobanByNifuda(nifudaNo);

            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetSupplierList")]
        public IActionResult GetSupplierList(string? name = null, int supplierCD = 0)
        {
            var result = _commonService.GetSupplierName(name, supplierCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMochikomisakiList")]
        public IActionResult GetMochikomisakiList(string? name = null, int mochikomisakiCD = 0)
        {
            var result = _commonService.GetMochikomisakiList(name, mochikomisakiCD);
            return Ok(new { IsSuccess = true, message = result });
        }
    }


}
