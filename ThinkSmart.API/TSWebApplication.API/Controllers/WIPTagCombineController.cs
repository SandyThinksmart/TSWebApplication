using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WIPTagCombineController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        private readonly IWIPTagCombineService _tagService;
        public WIPTagCombineController(LocalizationServices localization, IAllCommonService commonService, IWIPTagCombineService tagService)
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
        [HttpPost("GetTagDetailsByCombineCD")]
        public IActionResult GetTagDetailsByCombineCD(int combineCD)
        {
            var result = _tagService.GetTagDetailsByCombineCD(combineCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("DeleteTagCombineData")]
        public IActionResult DeleteTagCombineData(WIPTagCombineDetails request)
        {
            _tagService.DeleteTagCombineData(request);
            return Ok(new { IsSuccess = true });
        }
    }
}
