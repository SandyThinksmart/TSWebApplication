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
    public class WIPTagSplitController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        private readonly IWIPTagSplitService _tagService;
        public WIPTagSplitController(LocalizationServices localization,IAllCommonService commonService, IWIPTagSplitService tagService)
        {
            _localization = localization;
            _commonService = commonService;
            _tagService = tagService;
        }
        [HttpGet("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHTWipDivision"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetTagDetailsByDivisionCD")]
        public IActionResult GetTagDetailsByDivisionCD(int divisionCD)
        {
            var result = _tagService.GetTagDetailsByDivisionCD(divisionCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("DeleteTagSplitData")]
        public IActionResult DeleteTagSplitData(WIPTagSplitDetails request)
        {
            _tagService.DeleteTagSplitData(request);
            return Ok(new { IsSuccess = true});
        }
        
    }
}
