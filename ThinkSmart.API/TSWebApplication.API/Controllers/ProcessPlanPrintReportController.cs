using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Domain.Model;


using TSWebApplication.Application.Services;

using TSWebApplication.Domain.Resources;
using TSWebApplication.Application.IServices;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProcessPlanPrintReportController : ControllerBase
    {
        private readonly IProcessPlanDetails _ProcessPlanDetails;
       // private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;

        public ProcessPlanPrintReportController(IProcessPlanDetails ProcessPlanServices, LocalizationServices localization)
        {
            _ProcessPlanDetails = ProcessPlanServices;
            _localization = localization;
            //_commonService = commonService;
        }

        [HttpPost("GetProcessPlanData")]
        public IActionResult GetProcessPlanData([FromBody] ProcessPlanReportSearchRequest request)
        {
            List<string> ProcessPlanCount = new List<string>();

            bool isSuccess = false;
            int CoiltoBar = _ProcessPlanDetails.GetProcessPlanReportCoilToBarList(request);


            ProcessPlanCount = new List<string>();

            if (CoiltoBar > 0)
            {
                isSuccess = true;
                ProcessPlanCount.Add(Convert.ToString(CoiltoBar));
            }
            else
            {
                CoiltoBar = 0;
                ProcessPlanCount.Add(Convert.ToString(CoiltoBar));
            }



            int CoiltoCoil = _ProcessPlanDetails.GetProcessPlanReportCoilToCoilList(request);


           

            if (CoiltoCoil > 0)
            {
                isSuccess = true;
                ProcessPlanCount.Add(Convert.ToString(CoiltoCoil));
            }
            else
            {
                CoiltoCoil = 0;
                ProcessPlanCount.Add(Convert.ToString(CoiltoCoil));
            }
            int Bartobar = _ProcessPlanDetails.GetProcessPlanReportBarToBarList(request);

            if (Bartobar > 0)
            {
                isSuccess = true;
                ProcessPlanCount.Add(Convert.ToString(Bartobar));
            }
            else
            {
                Bartobar = 0;
                ProcessPlanCount.Add(Convert.ToString(Bartobar));
            }

            return Ok(new { IsSuccess = isSuccess, Message= ProcessPlanCount });

        }
    }
}
