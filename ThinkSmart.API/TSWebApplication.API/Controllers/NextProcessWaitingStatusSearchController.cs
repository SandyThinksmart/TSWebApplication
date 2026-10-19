using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NextProcessWaitingStatusSearchController : ControllerBase
    {
        private readonly INextProcessWaitingStatusSearchService _nextProcessWaitingStatusSearchService;
        public NextProcessWaitingStatusSearchController(INextProcessWaitingStatusSearchService nextProcessWaitingStatusSearchService)
        {
            _nextProcessWaitingStatusSearchService = nextProcessWaitingStatusSearchService;
            
        }
        
        [HttpPost("GetNextProcessWaitingStatusSearch")]
        public IActionResult GetNextProcessWaitingStatusSearch([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false; 
            var result = _nextProcessWaitingStatusSearchService.GetNextProcessWaitingStatusSearch(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpGet("GetLocationList")]
        public IActionResult GetLocationList()
        {
            var result = _nextProcessWaitingStatusSearchService.GetLocationNameList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
