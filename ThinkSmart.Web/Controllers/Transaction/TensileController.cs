using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Model;
using TSWebApplication.Application.IServices;
using Microsoft.AspNetCore.Http;
using TSWebApplication.Application.Services;

namespace TSWebApplication.Web.Controllers.Transaction
{

    public class TensileController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly ITensileService _TensileService;
        private readonly IViewRenderService _viewRenderService;
        public TensileController(LocalizationServices localization, ITensileService TensileService, IViewRenderService viewRenderService)
        {

            _localization = localization;
            _TensileService = TensileService;
            _viewRenderService = viewRenderService;

        }
        public IActionResult TensileEntry()
        {
 
            ViewBag.CultureName=UIHelper.GetCultureName();
            ViewBag.ProdSpecMax = UIConstants.K_PROD_SPEC_MAX;
            ViewBag.ProdSpecMin = UIConstants.K_PROD_SPEC_MIN;
            ViewBag.FI_Result_Ok = UIConstants.K_RESULT_OK;
            ViewBag.FI_Result_Ng = UIConstants.K_RESULT_NG;
            ViewBag.FI_Result_Untested = UIConstants.K_RESULT_UNTESTED;
            ViewBag.FI_Result_SparkUntested = UIConstants.K_RESULT_SPARKUNTESTED;
            ViewBag.FI_Spark_Untested = UIConstants.K_SPARK_UNTESTED;
            ViewBag.FI_Spark_Ok = UIConstants.K_SPARK_OK;
            ViewBag.FI_Spark_Ng = UIConstants.K_SPARK_NG;
            return View();
        }
        public IActionResult TensileEditDelete()
        {
            
            ViewBag.CultureName = UIHelper.GetCultureName();
            ViewBag.ProdSpecMax = UIConstants.K_PROD_SPEC_MAX;
            ViewBag.ProdSpecMin = UIConstants.K_PROD_SPEC_MIN;
            ViewBag.FI_Result_Ok = UIConstants.K_RESULT_OK;
            ViewBag.FI_Result_Ng = UIConstants.K_RESULT_NG;
            ViewBag.FI_Result_Untested = UIConstants.K_RESULT_UNTESTED;
            ViewBag.FI_Result_SparkUntested = UIConstants.K_RESULT_SPARKUNTESTED;
            ViewBag.FI_Spark_Untested = UIConstants.K_SPARK_UNTESTED;
            ViewBag.FI_Spark_Ok = UIConstants.K_SPARK_OK;
            ViewBag.FI_Spark_Ng = UIConstants.K_SPARK_NG;
            return View();
        }
        public IActionResult GetDecision()
        {
            var result = _TensileService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GridSearch([FromBody] List<TensileDetails> details)
        {
            foreach (TensileDetails record in details)
            {
                record.TS_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.TS_DECISION));
            }
            string view = _viewRenderService.RenderToString("Tensile/TensileGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "TS_TEST_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                       new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"),
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
                                            FieldName =   "DETAIL_NO" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
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



    }
}
