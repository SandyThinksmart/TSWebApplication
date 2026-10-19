using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Model;
using TSWebApplication.Application.Services;
using TSWebApplication.Application.IServices;
using Microsoft.AspNetCore.Http;

namespace TSWebApplication.Web.Controllers.Transaction
{

    public class ColdDrawingWIREDRAWINGController : Controller
    {

        private readonly IColdDrawingWireDrawingService _ColdDrawingWireDrawingService;
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        private readonly IAllCommonService _commonService;
        public ColdDrawingWIREDRAWINGController(IColdDrawingWireDrawingService coldDrawingWireDrawingService, LocalizationServices localization, IViewRenderService viewRenderService, IAllCommonService commonService)
        {
            _localization = localization;
            _ColdDrawingWireDrawingService = coldDrawingWireDrawingService;
            _viewRenderService = viewRenderService;
            _commonService = commonService;
        }
        public IActionResult ColdDrawingWIREDRAWINGEntry()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult ColdDrawingWIREDRAWINGEditDelete()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();

        }
        public IActionResult GetExteriorCondition()
        {
            var result = _ColdDrawingWireDrawingService.GetExteriorConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDieDiaShape()
        {
            var result = _ColdDrawingWireDrawingService.GetDieDiaShapeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetwindingShape()
        {
            var result = _ColdDrawingWireDrawingService.GetwindingShapeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDiceInspStart()
        {
            var result = _ColdDrawingWireDrawingService.GetDiceInspStartList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDecision()
        {
            var result = _ColdDrawingWireDrawingService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDiceInspEnd()
        {
            var result = _ColdDrawingWireDrawingService.GetDiceInspEndList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetMDust()
        {
            var result = _ColdDrawingWireDrawingService.GetMDustList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GridSearch([FromBody] List<ColdDrawingWireDrawingDetails> details)
        {
            foreach (ColdDrawingWireDrawingDetails record in details)
            {
                record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                if (record.UNIT_TYPE > 0)
                {
                    record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.DW_FINISHED_COILS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                }
                
            }

            string view = _viewRenderService.RenderToString("ColdDrawingWireDrawing/ColdDrawingWireDrawingGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public ActionResult OperatorIsInactive(string operatorName)
        {
            string validationMsg = string.Empty;

            validationMsg += UIHelper.GetErrorMessageForInactiveMaster(operatorName, UIConstants.MasterName.MST_EMPLOYEE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_ACTIVE);
            return Ok(new { message = validationMsg });
        }

        public ActionResult MachineIsInactive(string machineName)
        {
            string validationMsg = string.Empty;

            validationMsg += UIHelper.GetErrorMessageForInactiveMaster(machineName, UIConstants.MasterName.MST_MACHINE, UIConstants.ActiveInactiveLabel.MST_MACHINE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_MACHINE_ACTIVE);
            return Ok(new { message = validationMsg });


        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "DW_START_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"), //7
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "DW_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                             IsDefaultSort=false,
                                             SortOrder=TSSortDirection.Ascending
                                        },

                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName =   "ALL_TABA_NO" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "PP_CUSTOMER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                           new TSSortField {
                                            FieldName = "PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdShape"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "PP_PROD_DIM_D_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                            new TSSortField {
                                            FieldName = "PP_PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductLength"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName = "DW_END_OPERATOR_NAME",
                                            IsDefaultSort=false,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator")
                                        }



                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
    }
}
