using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class HikinukiCoilToBarController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly IHikinukiCoilToBarService _hikinukiCoilToBarService;
        public HikinukiCoilToBarController(IViewRenderService viewRenderService, LocalizationServices localization, IHikinukiCoilToBarService hikinukiCoilToBarService)
        { 
            _viewRenderService = viewRenderService;
            _localization = localization;
            _hikinukiCoilToBarService = hikinukiCoilToBarService;
        }

        public IActionResult HikinukiCoilToBarEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult HikinukiEditDelete()
        {
           
            ViewBag.CultureName = UIHelper.GetCultureName();
            
            List<HikinukiDetails> result = new List<HikinukiDetails>();
            return View(result);
        }
        public IActionResult HikinukiOracleToHostDataTransfer()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        
        public ActionResult DisplayInactivePopup(string nameValue, string tableName)
        {
            string validationMsg = string.Empty;
         string cultureName =   UIHelper.GetCultureName();
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }

            if (tableName == "machine")
            {
                validationMsg += UIHelper.GetErrorMessageForInactiveMaster(nameValue, UIConstants.MasterName.MST_MACHINE, UIConstants.ActiveInactiveLabel.MST_MACHINE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_MACHINE_ACTIVE);
            }
            else
            {
                validationMsg += UIHelper.GetErrorMessageForInactiveMaster(nameValue, UIConstants.MasterName.MST_EMPLOYEE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_ACTIVE);
            }

            return Ok(new { message = validationMsg });
        }
        public IActionResult GridSearch([FromBody] List<HikinukiDetails> details)
        {
            foreach (HikinukiDetails record in details)
            {
                record.DW_FINISHED_BARS_DECODE = record.DW_FINISHED_BARS + UIUtils.GetUnitTypeName(Convert.ToString(record.UNIT_TYPE));
                record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
            }

            string view = _viewRenderService.RenderToString("HikinukiCoilToBar/HikinukiCoilToBarGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetTPTestList()
        {
            var result = _hikinukiCoilToBarService.GetTPTestList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDieDiaShape()
        {
            var result = _hikinukiCoilToBarService.GetDieDiaShapeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetUnitType()
        {
            var result = _hikinukiCoilToBarService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetBendStatus()
        {
            var result = _hikinukiCoilToBarService.GetBendStatusList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetExteriorCondition()
        {
            var result = _hikinukiCoilToBarService.GetExteriorConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDecision()
        {
            var result = _hikinukiCoilToBarService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        //public IActionResult GetNextProcess()
        //{
        //    var result = _hikinukiCoilToBarService.GetNextProcessList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}

        public ActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust") //1
                                        },
                                              new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName") //7
                                        },
                                               new TSSortField {
                                            FieldName = "PP.PROD_SHAPE,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape") //3
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D", //,PP.PROD_DIM_W,PP.PROD_DIM_OTHER
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD") //2
                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length") //4
                                        },
                                          new TSSortField {
                                            FieldName = "DR.DW_END_DATE",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay") //5
                                        },
                                          new TSSortField {
                                                 
                                            FieldName = "EMP.YOBINA,EMP.YOBIGANA,EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator") //6
                                        },
                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                           new TSSortField {
                                            FieldName = "DR.DW_TABA_NO_1, DR.DW_TABA_NO_2, DR.DW_TABA_NO_3, DR.DW_TABA_NO_4",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }
    }
}
