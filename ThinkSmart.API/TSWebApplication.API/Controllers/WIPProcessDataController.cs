using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WIPProcessDataController : ControllerBase
    {
        private readonly IWIPProcessDataServices _wiprocessDataServices;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public WIPProcessDataController(IWIPProcessDataServices WIProcessDataServices, LocalizationServices localization, IAllCommonService commonService)
        {
            _wiprocessDataServices = WIProcessDataServices;
            _localization = localization;
            _commonService = commonService;

        }
        [HttpPost("IsWIPProcessDataExist")]
        public IActionResult IsWIPProcessDataExist(int userId)
        {
            List<string> ProcessPlanCount = new List<string>();
            int locationCD = _commonService.GetUserLocation(userId);
            bool isSuccess = false;
            int dataCount = _wiprocessDataServices.GetWIProcessDataList(locationCD);
            if (dataCount >0)
            {
                isSuccess = true;
               
            }
            return Ok(new { IsSuccess = isSuccess, Message = dataCount });

        }
    }
    }
