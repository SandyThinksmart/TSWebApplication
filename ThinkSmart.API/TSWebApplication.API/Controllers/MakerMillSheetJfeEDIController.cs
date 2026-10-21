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
    public class MakerMillSheetJfeEDIController : ControllerBase
    {
        private readonly IMakerMillSheetJfeEDIService _makermillsheetJfeediService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public MakerMillSheetJfeEDIController(IMakerMillSheetJfeEDIService makermillsheetJfeediService, LocalizationServices localization, IAllCommonService commonService)
        {
            _makermillsheetJfeediService = makermillsheetJfeediService;
            _localization = localization;
            _commonService = commonService;
        }

        [HttpPost("MillSheetJfeEDIDataInsert")]
        public ActionResult MillSheetJfeEDIDataInsert([FromBody] List<MakerMillSheetEDIDetails> request, string cultureName,int userID)
        {
            DateTime printReportDate;
            int runID = 0;
            printReportDate = DateTime.Now;

            _makermillsheetJfeediService.MillSheetJfeEDILogDataInsert(request, cultureName,userID);

            //Get data for process report

            List<MakerMillSheetEDIDetails> allData = _makermillsheetJfeediService.GetMakerMillSheetProcessRptData(runID, printReportDate);

            //Get data for error report

            var result = _makermillsheetJfeediService.GetMakerMillSheetErrorRptData(runID, printReportDate);

            List<MakerMillSheetEDIDetails> errorData = result.ErrorData;
            runID = result.RunID;

            return Ok(new { IsSuccess = true, AllData = allData, ErrorData = errorData, RunId = runID });
        }
    }
}
