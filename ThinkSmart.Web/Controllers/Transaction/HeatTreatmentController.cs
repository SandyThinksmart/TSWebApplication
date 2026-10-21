using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class HeatTreatmentController : Controller
    {
        private readonly IHeatTreatmentService _heatTreatmentService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;

        public HeatTreatmentController(IHeatTreatmentService heatTreatmentService, IViewRenderService viewRenderService, LocalizationServices localization, IAllCommonService commonService)
        {
            _heatTreatmentService = heatTreatmentService;
            _viewRenderService = viewRenderService;
            _localization = localization;
            _commonService = commonService; 
        }
        public IActionResult HeatTreatmentEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult HeatTreatmentEditDelete(string furNo = "", int machineCD = 0)
        {
            ViewBag.furNo = furNo;
            ViewBag.machineCD = machineCD;
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult HeatTreatmentList()
        {
            return View();
        }

        public IActionResult GridSearch([FromBody] List<HeatTreatMentDetails> details)
        {
            string view = _viewRenderService.RenderToString("HeatTreatment/HeatTreatmentListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult FillGrid([FromBody] List<HeatTreatMentDetails> details)
        {
            string view = _viewRenderService.RenderToString("HeatTreatment/HeatTreatmentGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public ActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                       

                                            new TSSortField {
                                            FieldName = "MACHINE_CD",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceProc"),
                                            IsDefaultSort=true,

                                        },
                                            new TSSortField {
                                            FieldName = "FURNACE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                            IsDefaultSort=true,
                                        },
                                             new TSSortField {
                                           FieldName = "HT_START_DATE",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime"),
                                            IsDefaultSort=true,
                                           SortOrder=TSSortDirection.Descending

                                        },
                                              new TSSortField {
                                            FieldName = "HT_CONDITION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcCondition"),


                                        },
                                              new TSSortField {
                                            FieldName = "PATTERN_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_PatternNo"),

                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }

        public IActionResult GetProcConditionKubun()
        {
            var result = _heatTreatmentService.GetProcConditionKubun();
            return Ok(new { IsSuccess = true, message = result });
        }
        
        public IActionResult GetProductUnitKubun()
        {
            var result = _heatTreatmentService.GetProductUnitKubun();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetMatlType()
        {
            var result = _heatTreatmentService.GetMatlType();
            return Ok(new { IsSuccess = true, message = result });
        }

    }
}
