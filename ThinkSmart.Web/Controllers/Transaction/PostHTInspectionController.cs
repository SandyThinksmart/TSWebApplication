using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class PostHTInspectionController : Controller
    {
        private readonly IPostHTInspectionService _PostHTInspectionService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        public PostHTInspectionController(IPostHTInspectionService PostHTInspectionService, LocalizationServices localization, IViewRenderService viewRenderService)
        {
            _PostHTInspectionService = PostHTInspectionService;
            _localization = localization;           
            _viewRenderService = viewRenderService;
        }
        public IActionResult PostHTInspectionEntryEditDelete()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        
        public IActionResult PostHTInspectionList()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            return Ok(new { IsSuccess = true, message = dateTime });
        }
        public IActionResult GridSearch([FromBody] List<PostHTDetails> details)
        {
            foreach (PostHTDetails record in details)
            {
                record.PP_HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
            }
            string view = _viewRenderService.RenderToString("PostHTInspection/PostHTInspectionListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                            new TSSortField {
                                            FieldName = "INT_QLTY_TEST_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        //    new TSSortField {
                                        //    FieldName = "MACHINE_NAME",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        //    IsDefaultSort=false,
                                        //    SortOrder=TSSortDirection.Descending
                                        //},

                                            new TSSortField {
                                            FieldName = "FURNACE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                              new TSSortField {
                                            FieldName = "PROC_COND",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcCondition"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                            new TSSortField {
                                            FieldName = "PATTERN_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_PatternNo"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        }

                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }

        public IActionResult GetStackPosition()
        {
            var result = _PostHTInspectionService.GetStackPositionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetNextProcess()
        {
            var result = _PostHTInspectionService.GetNextProcessList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetResult()
        {

            var result = _PostHTInspectionService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetProcConditionKubun()
        {
            var result = _PostHTInspectionService.GetProcConditionKubun();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetInOutKubun()
        {
            var result = _PostHTInspectionService.GetInOutKubun();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetOrientationKubun()
        {
            var result = _PostHTInspectionService.GetOrientationKubun();
            return Ok(new { IsSuccess = true, message = result });
        }


    }
}
