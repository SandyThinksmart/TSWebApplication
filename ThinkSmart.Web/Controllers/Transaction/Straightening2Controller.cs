using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using static Google.Cloud.Vision.V1.ProductSearchResults.Types;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class Straightening2Controller : Controller
    {
        private readonly IStraightening2Service _straightening2Service;
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        public Straightening2Controller(LocalizationServices localization, IViewRenderService viewRenderService, IStraightening2Service straightening2Service)
        {

            _localization = localization;
            _viewRenderService = viewRenderService;
            _straightening2Service = straightening2Service;
        }
        public IActionResult StraighteningEntry2()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            
            return View();
        }
        public IActionResult StraighteningEditDelete2()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult GetUnitType()
        {
            var result = _straightening2Service.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetExteriorCondition()
        {
            var result = _straightening2Service.GetExteriorConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetBendStatus()
        {
            var result = _straightening2Service.GetBendStatusList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDecision()
        {
            var result = _straightening2Service.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetTPTestList()
        {
            var result = _straightening2Service.GetTPTestList();
            return Ok(new { IsSuccess = true, message = result });
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

        public IActionResult GridSearch([FromBody] List<Straightening2Details> details)
        {
            foreach (Straightening2Details record in details)
            {
                record.STR_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.STR_BARS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.STR_UNIT_TYPE));
            }
            string view = _viewRenderService.RenderToString("Straightening2/Straightening2Grid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public ActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort =true,
                                             SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "STR_TABA_NUMBERS",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort =true
                                        },
                                        new TSSortField {
                                            FieldName = "STR_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                             IsDefaultSort=false,
                                             SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust")
                                        },
                                        new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName")
                                        },
                                        new TSSortField {
                                            FieldName = "PP_CUSTOMER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer")
                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdShape")
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductLength")
                                        },
                                        new TSSortField {
                                           // FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                           FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim")
                                        },
                                        new TSSortField {
                                            FieldName = "ST.STR_START_DATE",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay")
                                        },
                                          new TSSortField {
                                            FieldName = "STR_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator")
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }
    }
}
