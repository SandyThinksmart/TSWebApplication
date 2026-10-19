using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MakerMillSheetEDIController : ControllerBase
    {
        private readonly IMakerMillSheetEDIServices _makermillsheetediService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public MakerMillSheetEDIController(IMakerMillSheetEDIServices makermillsheetediService, LocalizationServices localization, IAllCommonService commonService)
        {
            _makermillsheetediService = makermillsheetediService;
            _localization = localization;
            _commonService = commonService;
        }

        //[HttpPost("ReadCSVFile")]
        //public ActionResult ReadCSVFile(string filename)
        //{
        //   var results = _makermillsheetediService.ReadCSVFile(filename);
        //    return Ok(new { IsSuccess = true, message = results });
        //}

        [HttpPost("MillSheetEDIDataInsert")]
        public ActionResult MillSheetEDIDataInsert([FromBody] List<MakerMillSheetEDIDetails> request, string cultureName, int userID)
        {
            DateTime printReportDate;
            int runID = 0;
            printReportDate = DateTime.Now;

            _makermillsheetediService.MillSheetEDILogDataInsert(request, cultureName, userID);

            //Get data for process report

            List<MakerMillSheetEDIDetails> allData = _makermillsheetediService.GetMakerMillSheetProcessRptData(runID, printReportDate);

            //Get data for error report

            var result = _makermillsheetediService.GetMakerMillSheetErrorRptData(runID, printReportDate); 

            List<MakerMillSheetEDIDetails> errorData=result.ErrorData;
             runID = result.RunID;

            return Ok(new { IsSuccess = true, AllData = allData, ErrorData = errorData, RunId = runID });
        }

        //[HttpGet("GetMakerMillSheetErrorRptData")]
        //public ActionResult GetMakerMillSheetErrorRptData(int runID, DateTime printReportDate)
        //{
          
        //    List<MakerMillSheetEDIDetails> errorData = _makermillsheetediService.GetMakerMillSheetErrorRptData(runID, printReportDate);

        //    return Ok(new { IsSuccess = true, ErrorData = errorData });
        //}

        //[HttpGet("GetMakerMillSheetProcessRptData")]
        //public ActionResult GetMakerMillSheetProcessRptData(int runID, DateTime printReportDate)
        //{

        //    List<MakerMillSheetEDIDetails> allData = _makermillsheetediService.GetMakerMillSheetProcessRptData(runID, printReportDate);

        //    return Ok(new { IsSuccess = true, AllData = allData });
        //}
    }
}
