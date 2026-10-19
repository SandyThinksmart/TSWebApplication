using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using TSWebApplication.Application.IServices;
using Microsoft.AspNetCore.Http;

namespace TSWebApplication.Web.Controllers.Transaction
{

    public class HardnessController : Controller
    {
        private readonly IMIHardnessService _mIHardnessService;
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
      
        public HardnessController (IMIHardnessService IMIHardnessService, LocalizationServices localization, IViewRenderService viewRenderService)
            {
            _mIHardnessService = IMIHardnessService;
            _localization = localization;
            _viewRenderService = viewRenderService;
        }
        public IActionResult HardnessEntry()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            ViewBag.FI_Result_Ok = UIConstants.K_RESULT_OK;
            ViewBag.FI_Result_Ng = UIConstants.K_RESULT_NG;
            ViewBag.FI_Result_Untested = UIConstants.K_RESULT_UNTESTED;
            ViewBag.FI_Result_SparkUntested = UIConstants.K_RESULT_SPARKUNTESTED; 
            ViewBag.FI_Spark_Untested = UIConstants.K_SPARK_UNTESTED;
            ViewBag.FI_Spark_Ok = UIConstants.K_SPARK_OK;
            ViewBag.FI_Spark_Ng = UIConstants.K_SPARK_NG;
            //int locationCd = _mIHardnessService.GetUserLocationById(userId);

            return View();
        }
        public IActionResult HardnessEditDelete()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            ViewBag.FI_Result_Ok = UIConstants.K_RESULT_OK;
            ViewBag.FI_Result_Ng = UIConstants.K_RESULT_NG;
            ViewBag.FI_Result_Untested = UIConstants.K_RESULT_UNTESTED;
            ViewBag.FI_Result_SparkUntested = UIConstants.K_RESULT_SPARKUNTESTED;
            ViewBag.FI_Spark_Untested = UIConstants.K_SPARK_UNTESTED;
            ViewBag.FI_Spark_Ok = UIConstants.K_SPARK_OK;
            ViewBag.FI_Spark_Ng = UIConstants.K_SPARK_NG;
            return View();
        }
       
        public IActionResult SetDecisionList()
        {
            var result = _mIHardnessService.SetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public ActionResult OperatorIsInactive(string operatorName)
        {
            string validationMsg = string.Empty;

            validationMsg += UIHelper.GetErrorMessageForInactiveMaster(operatorName, UIConstants.MasterName.MST_EMPLOYEE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_ACTIVE);
            return Ok(new { message = validationMsg });


        }
        public IActionResult GridSearch([FromBody] List<MIHardnessDetails> details)
        {
            foreach (MIHardnessDetails record in details)
            {
                record.HD_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.HD_DECISION));
                record.HD_PATTERN_ACT_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HD_PATTERN_ACT));
            }
            string view = _viewRenderService.RenderToString("Hardness/HardnessGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "HD_TEST_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                     
                                         new TSSortField {
                                            FieldName = "pp_chuuban_gyouban",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                            new TSSortField {
                                            FieldName =   "DETAIL_NO" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_SplitNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "pp_customer_name",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "pp_prod_name_cust",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                           new TSSortField {
                                            FieldName = "pp_prod_shape",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdShape"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                            new TSSortField {
                                            FieldName = "pp_prod_length",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductLength"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                             new TSSortField {
                                            FieldName = "OPERATOR_NAME",
                                            IsDefaultSort=false,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),
                                            SortOrder=TSSortDirection.Ascending

                                             }



                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
        public IActionResult SetHardnessKubuns()
        {
            var result = _mIHardnessService.SetHardnessKubunsList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
