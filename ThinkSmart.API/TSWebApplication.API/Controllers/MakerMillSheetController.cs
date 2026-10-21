using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
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
    public class MakerMillSheetController : ControllerBase
    {
        private readonly IMakerMillSheetServices _makermillsheetService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public MakerMillSheetController(IMakerMillSheetServices makermillsheetService, LocalizationServices localization, IAllCommonService commonService)
        {
            _makermillsheetService = makermillsheetService;
            _localization = localization;
            _commonService = commonService;
        }

        [HttpPost("MakerMillSheetDetailsSave")]
        public ActionResult MakerMillSheetDetailsSave([FromForm] List<MakerMillSheetDetails> request) //, [FromForm] IFormFile file
        {

            int responseCode = _makermillsheetService.InsertMakerMillSheetDetails(request);
            return Ok(new { IsSuccess = true, message = responseCode });
        }
        [HttpGet("GetUnitType")]
        public IActionResult GetUnitType()
        {
            var result = _makermillsheetService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetChemicalSpecifcation")]
        public IActionResult GetChemicalSpecifcation()
        {
            var result = _makermillsheetService.GetChemicalSpecifcationList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _makermillsheetService.GetMakerMillSheetList(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMakerMillSheetDetailsByCD")]
        public IActionResult GetMakerMillSheetDetailsByCD(long millsheetCD)
        {
            var result = _makermillsheetService.GetMakerMillSheetDetailsByCD(millsheetCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("MakerMillSheetDetailsUpdate")]
        public IActionResult MakerMillSheetDetailsUpdate([FromForm] MakerMillSheetDetails request)
        {
            int statusCode =_makermillsheetService.MakerMillSheetDetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = statusCode });
        }
        [HttpPost("DeleteMakerMillSheetDetails")]
        public IActionResult DeleteMakerMillSheetDetails(long MakerMillSheetCD)
        {
          int dataDeleted = _makermillsheetService.DeleteMakerMillSheetDetails(MakerMillSheetCD);
            return Ok(new { IsSuccess = true, message = dataDeleted });
        }
        [HttpGet("GetKubunName")]
        public ActionResult GetKubunName(string KubunName)
        {
            bool isKubunExist = _makermillsheetService.GetKubunName(KubunName);
            return Ok(new { IsSuccess = isKubunExist });
        }

        [HttpPost("GetNextAndPriviousPDFFile")]
        public IActionResult GetNextAndPriviousPDFFile(long MakerCD, string PDFPath, int PDFCount)
        {
            var result = _makermillsheetService.GetNextAndPriviousPDFFile(MakerCD, PDFPath, PDFCount);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("SavePDFSettings")]
        public IActionResult SavePDFSettings(List<MakerMillSheetConfiguration> details)
        {
           _makermillsheetService.SavePDFSettings(details);
            return Ok(new { IsSuccess = true });
        }
        [HttpPost("UpdatePDFSettings")]
        public IActionResult UpdatePDFSettings(List<MakerMillSheetConfiguration> details)
        {
            _makermillsheetService.UpdatePDFSettings(details);
            return Ok(new { IsSuccess = true });
        }
        
        [HttpPost("GetMillSheetPDFDetails")]
        public IActionResult GetMillSheetPDFDetails(int makerCD, string templateName)
        {
          var result =  _makermillsheetService.GetMillSheetPDFDetails(makerCD, templateName);
            return Ok(new { IsSuccess = true, message= result });
        }
        [HttpGet("GeMatlMakerList")]
        public IActionResult GeMatlMakerList()
        {
            var result = _makermillsheetService.GetMakerList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetDefaultLayoutData")]
        public IActionResult GetDefaultLayoutData(string templateName)
        {
            var result = _makermillsheetService.GetDefaultLayoutData(templateName);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMakerHardnessDetailJIS")]
        public IActionResult GetMakerHardnessDetailJIS(string FiileName)
        {
            var result = _makermillsheetService.GetMakerHardnessDetailJIS(FiileName);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMakerHardnessDetailJISCal")]
        public IActionResult GetMakerHardnessDetailJISCal(string FiileName)
        {
            var result = _makermillsheetService.GetMakerHardnessDetailJISCal(FiileName);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetMakerMillSheetDetail")]
        public IActionResult GetMakerMillSheetDetail(string Charge, string DIM_D)
        {
            var result = _makermillsheetService.GetMakerMillsheetDetails(Charge, DIM_D);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GridSearchPDFConfig")]
        public IActionResult GridSearchPDFConfig([FromBody] GridSearchFields request)
        {
            var result = _makermillsheetService.GetPDFConfigList(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetPDFDetailsByTemplateName")]
        public IActionResult GetPDFDetailsByTemplateName(string templateName)
        {
            var result = _makermillsheetService.GetPDFDetailsByTemplateName(templateName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("DeleteConfigDataByFieldCD")]
        public IActionResult DeleteConfigDataByFieldCD(MakerMillSheetConfiguration request)
        {
            _makermillsheetService.DeleteConfigDataByFieldCD(request);
            return Ok(new { IsSuccess = true, message = "" });
        }
        [HttpPost("DeleteConfigDataByTemplateName")]
        public IActionResult DeleteConfigDataByTemplateName(string templateName)
        {
            _makermillsheetService.DeleteConfigDataByTemplateName(templateName);
            return Ok(new { IsSuccess = true, message = "" });
        }
        [HttpPost("IsTemplateNameAlreadyExist")]
        public IActionResult IsTemplateNameAlreadyExist(string templateName)
        {
          bool returnValue =   _makermillsheetService.IsTemplateNameAlreadyExist(templateName);
            return Ok(new { message = returnValue });
        }
        [HttpGet("GetUniqueTemplateList")]
        public IActionResult GetUniqueTemplateList()
        {
            var result = _makermillsheetService.GetUniqueTemplateList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetMakerNameByConfigCD")]
        public IActionResult GetMakerNameByConfigCD(int configCD)
        {
            int makerCD = _makermillsheetService.GetMakerNameByConfigCD(configCD);
            return Ok(new { IsSuccess = true, message = makerCD });
        }
        [HttpGet("GetMaxLogRunCD")]
        public IActionResult GetMaxLogRunCD()
        {
            int runCD = _makermillsheetService.GetMaxLogRunCD();
            return Ok(new { IsSuccess = true, message = runCD });
        }
        [HttpPost("IsFieldElementSaved")]
        public IActionResult IsFieldElementSaved(int fieldCD, string templateName)
        {
            bool isSaved = _makermillsheetService.IsFieldElementSaved(fieldCD, templateName);
            return Ok(new { IsSuccess = true, message = isSaved });
        }
        [HttpPost("IsKoubanAlreadySaved")]
        public ActionResult IsKoubanAlreadySaved(string KubunName, int millsheetCD)
        {
            bool isKubunExist = _makermillsheetService.IsKoubanAlreadySaved(KubunName, millsheetCD);
            return Ok(new { IsSuccess = isKubunExist });
        }
                
        [HttpPost("UpdateEDIData")]
        public IActionResult UpdateEDIData([FromForm] List<MakerMillSheetDetails> request)
        {
             int result = _makermillsheetService.UpdateEDIData(request);
             bool isErrorFound = _makermillsheetService.IsEDIAllDataValid(request[0].RUN_CD);
            return Ok(new { IsSuccess = result, message = isErrorFound });
        }
        [HttpGet("IsFTPServerActive")]
        public IActionResult IsFTPServerActive()
        {
            var result = _makermillsheetService.IsFTPServerActive();
            return Ok(new { IsSuccess = result.Item1, message = result.Item2 });
        }
        
        [HttpPost("IsMillSheetAlreadySaved")]
        public IActionResult IsMillSheetAlreadySaved([FromBody] MakerMillSheetDetails request)
        {
            bool returnVal = false;
            returnVal = _makermillsheetService.IsDuplicateDataExist(request);
            return Ok(new { IsSuccess = returnVal });
        }

    }
}
