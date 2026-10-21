using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProcessTypeRemarkUpdationController : ControllerBase
    {
        private readonly IProcessTypeRemarkUpdationService _processTypeRemarkUpdationService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;

        public ProcessTypeRemarkUpdationController(IProcessTypeRemarkUpdationService processTypeRemarkUpdationService, LocalizationServices localization, IAllCommonService commonService)
        {
            _processTypeRemarkUpdationService = processTypeRemarkUpdationService;
            _localization = localization;
            _commonService = commonService;
        }
        [HttpGet("GetProcessKubun")]
        public IActionResult GetProcessKubun()
        {
            var result = _processTypeRemarkUpdationService.GetProcessDetails();
            var removeIds = new List<int> { 30,40,120,130,140,145,150,155,160,170,190,810,820,830,835,840,880,890,900,901,999};//Remove this All Process On Remark master clientVisit[july 2025]
            result = result.Where(x => !removeIds.Contains(x.KUBUN_ID)).ToList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("UpdateProcessKubunRemark")]
        public IActionResult ProcessKubunRemarkUpdate(ProcessTypeRemarkUpdationDetails request)
        {
            _processTypeRemarkUpdationService.ProcessKubunRemarkUpdate(request);
            return Ok(new { IsSuccess = true, message = "" });
        }
    }
}
