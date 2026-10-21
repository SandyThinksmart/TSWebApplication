using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using TSWebApplication.Application.IServices;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class WIPTensouAcceptanceController : Controller
    {
        private readonly IWIPTensouAcceptanceService _IWIPTensouAcceptanceService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;

        public WIPTensouAcceptanceController(IWIPTensouAcceptanceService IWIPTensouAcceptanceService, LocalizationServices localization, IViewRenderService viewRenderService)
        {
            _IWIPTensouAcceptanceService = IWIPTensouAcceptanceService;
            _localization = localization;
            _viewRenderService = viewRenderService;
        }

        public IActionResult WIPTensouAcceptanceEntryEditDelete(int TransferCD = 0)
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

       
        //public IActionResult GetNextProcess()
        //{
        //    var result = _IWIPTensouAcceptanceService.GetNextProcessList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
    }
}
