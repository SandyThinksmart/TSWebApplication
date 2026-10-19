using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatlNameWiseOrderQtyController : ControllerBase
    {
        private readonly IMatlNameWiseOrderQtyService _matlNameWiseOrderQtyService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public MatlNameWiseOrderQtyController(IMatlNameWiseOrderQtyService matlNameWiseOrderQtyService, LocalizationServices localization, IAllCommonService commonService)
        {
            _matlNameWiseOrderQtyService = matlNameWiseOrderQtyService;
            _localization = localization;
            _allCommonService = commonService;

        }
        [HttpPost("GetBatchRollFromConfig")]
        public ActionResult GetBatchRollFromConfig()
        {
            int result = _allCommonService.GetBatchRollFromConfig();
            return Ok(new { IsSuccess = true, message = result });
          
        }
        [HttpPost("GetMatlNameWiseOrderQty")]
        public IActionResult GetMaterialOrderQtyData(int Batch_Roll)
        {
            bool isSuccess = false;
            var result = _matlNameWiseOrderQtyService.GetMaterialOrderQtyData(Batch_Roll);
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
       
    }
}
