using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProcessDisplayConfigController : ControllerBase
    {
        private readonly IProcessDisplayConfigService _ProcessDisplayConfigService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;


        public ProcessDisplayConfigController(IProcessDisplayConfigService processDisplayConfigService, LocalizationServices localization, IAllCommonService commonService)
        {

            _localization = localization;
            _ProcessDisplayConfigService = processDisplayConfigService;
            _commonService = commonService;
        }
        [HttpPost("GetProcessKubunDetails")]
        public IActionResult GetProcessDetails()
        {
            var result = _ProcessDisplayConfigService.GetProcessDetails();
            var removeIds = new List<int> { 140,810, 820, 830, 835 }; 
            result = result.Where(x => !removeIds.Contains(x.KUBUN_ID)).ToList();
           //  result = result.Where(x => x.KUBUN_ID != 140).ToList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetProcessKubun")]
        public IActionResult GetProcessKubun()
        {
            var result = _ProcessDisplayConfigService.GetProcessDetails();
            var removeIds = new List<int> { 140, 999 , 900 , 901 ,810,820,830,835}; //Remove Kubun clientVisit[july 2025]
            result = result.Where(x => !removeIds.Contains(x.KUBUN_ID)).ToList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("ProcessKubunDetailsSave")]
        public IActionResult ProcessKubunDetailsSave(List<ProcessDisplayConfigDetails> request)
        {
            _ProcessDisplayConfigService.ProcessKubunDetailsSave(request);
            return Ok(new { IsSuccess = true, message = "" });
        }
        
        [HttpPost("GetActiveNextProcessKubun")]
        public ActionResult GetActiveNextProcessKubun(long kubunCD)
        {
            List<ProcessDisplayConfigDetails> result = new List<ProcessDisplayConfigDetails>();
            result = _ProcessDisplayConfigService.GetActiveNextProcessKubunDetails(kubunCD);
            return Ok(new { IsSuccess = true, message = result });

        }
    }
}
