using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ProcessPlanHandoverController : Controller
    {
        private readonly IProcessPlanHandoverService _processPlanHandoverService;
      
        public ProcessPlanHandoverController(IProcessPlanHandoverService processPlanHandoverService)
        {
            _processPlanHandoverService = processPlanHandoverService;
            
        }

        public IActionResult ProcessPlanHandoverEntryCancel()
        {
            return View();
        }
        
        public IActionResult GetNextProcessNameList()
        {
            var result = _processPlanHandoverService.GetNextProcessList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
