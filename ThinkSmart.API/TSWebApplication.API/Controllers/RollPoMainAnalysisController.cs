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
    public class RollPoMainAnalysisController : ControllerBase
    {
        private readonly IRollPoMainAnalysisServices _rollPoMainAnalysisServices;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public RollPoMainAnalysisController(IRollPoMainAnalysisServices rollPoMainAnalysisServices, LocalizationServices localization, IAllCommonService commonService)
        {
            _rollPoMainAnalysisServices = rollPoMainAnalysisServices;
            _localization = localization;
            _allCommonService = commonService;

        }
      
        /// <summary>
        /// Get Material reply Details.
        /// </summary>
        /// <returns>Created by shobhana[22 JAN 2025]
        /// </returns>
        [HttpPost("GetMaterialReply")]
        public IActionResult GetMaterialReply()
        {
            var result = _rollPoMainAnalysisServices.GetMaterialReply();
            return Ok(new { IsSuccess = true, message = result });
           
        }


        [HttpPost("IsBatchRunSuccess")]
        public ActionResult IsBatchRunSuccess([FromBody] RollPoMainAnalysisDetails details)    
        {            
            int result = _rollPoMainAnalysisServices.IsBatchRunSuccess(details);
            return Ok(new { IsSuccess = true, message = result });
        }



        [HttpPost("ExecuteMaterialForecastBatch")]
        public IActionResult ExecuteMaterialForecastBatch([FromBody] GridSearchFields details)
        {
            var  result = _rollPoMainAnalysisServices.ExecuteMaterialForecastBatch(details);
            return Ok(new { IsSuccess = true, message = result });
        }
       
        [HttpPost("GetForecastRoll")]
        public ActionResult GetForecastRoll()
        {     
             DateTime currentDate = DateTime.Now;
            var result = _rollPoMainAnalysisServices.GetForecastRoll(currentDate);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("ImportBatchDataFromHost")]
        public ActionResult ImportBatchDataFromHost()
        { 
           _rollPoMainAnalysisServices.ImportBatchDataFromHost();
            return Ok(new { IsSuccess = true });
        }

        [HttpPost("EmployeeList")]
        public IActionResult EmployeeList(string? yobina = "", int operatorCD=0)
        {
            var lst = _allCommonService.GetOperatorName(yobina, operatorCD);
            return Ok(new { IsSuccess = true, message = lst });
        }

        [HttpPost("GetLocationName")]
        public IActionResult GetLocationNameByEmployeeCD(int employeeCD )
        {
            var result = _rollPoMainAnalysisServices.GetLocationNameByEmployeeCD(employeeCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRollPOMainAnalysisGridData")]
        public IActionResult GetRollPOMainAnalysisGridData(GridSearchFields gridSearchRequest)
        {           
            var result = _rollPoMainAnalysisServices.GetRollPOMainAnalysisGridData(gridSearchRequest);            
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("PlaceOrder")]
        public IActionResult UpdateRollandYear([FromBody] GridSearchFields details)
        {
            var result = _rollPoMainAnalysisServices.PlaceOrder(details);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpPost("SetAnalysisOverForFcSummaryWithNoFC")]
        public IActionResult SetAnalysisOverForFcSummaryWithNoFC()
        {
            var result = _rollPoMainAnalysisServices.SetAnalysisOverForFcSummaryWithNoFC();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("IsPODone")]
        public ActionResult IsPODone([FromBody] DateTimeDetails details)
        {
            int result = _rollPoMainAnalysisServices.IsPODone(details);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("IsAllAnalysisIsOver")]
        public ActionResult IsAllAnalysisIsOver()
        {
            int result = _rollPoMainAnalysisServices.IsAllAnalysisIsOver();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("CheckMatchingYearRRecords")]
        public ActionResult AreThereMatchingYearRRecords([FromBody] RollPoMainAnalysisDetails details)
        {
            int result = _rollPoMainAnalysisServices.CheckMatchingYearRRecords(details);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("CheckForecastLocked")]
        public ActionResult CheckForecastLocked([FromBody] RollPoMainAnalysisDetails details)
        {
            int result = _rollPoMainAnalysisServices.CheckForecastLocked(details);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetEmployeeCd")]
        public IActionResult GetEmployeeCd()
        {
            var employeeCD = _rollPoMainAnalysisServices.GetEmployeeCd();
            return Ok(new { IsSuccess = true, message = employeeCD });
        }
    }

}