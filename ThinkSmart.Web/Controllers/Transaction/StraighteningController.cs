using Microsoft.AspNetCore.Http;
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


namespace TSWebApplication.Web.Controllers
{
    public class StraighteningController : Controller
    {
        private readonly IStraighteningService _straighteningService;
        private readonly IStamppingService _stamppingService;
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        public StraighteningController(IStraighteningService straighteningService, LocalizationServices localization, IStamppingService stamppingService, IViewRenderService viewRenderService)
        {
            _straighteningService = straighteningService;
            _localization = localization;
            _stamppingService = stamppingService;
            _viewRenderService = viewRenderService;
        }
        public IActionResult StraighteningEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult StraighteningEditDelete()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();

        }
        public IActionResult GetDecision()
        {
            var result = _straighteningService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GridSearch([FromBody] List<StraighteningDetails> details)
        {
            foreach (StraighteningDetails record in details)
            {
                record.STR_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                if (record.STR_UNIT_TYPE > 0)
                {
                    record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.STR_BARS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.STR_UNIT_TYPE));
                }
            }
            string view = _viewRenderService.RenderToString("Straightening/StraighteningGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetUnitType()
        {
            var result = _straighteningService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetExternalSurfaceCondition()
        {
            var result = _straighteningService.GetExternalSurfaceConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetTPTest()
        {
            var result = _straighteningService.GetTPTestList();
            return Ok(new { IsSuccess = true, message = result });
        }
        
        public IActionResult GetBendStatus()
        {
            var result = _straighteningService.GetGetBendStatusList();
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

        public ActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP.CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort =true,
                                             SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "ST.STR_TABA_NO_1,ST.STR_TABA_NO_2,ST.STR_TABA_NO_3,ST.STR_TABA_NO_4",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort =true
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
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape")
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length")
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD")
                                        },
                                        new TSSortField {
                                            FieldName = "ST.STR_START_OPERATOR_CD",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay")
                                        },
                                          new TSSortField {
                                            FieldName = "EMP.YOBINA,EMP.YOBIGANA,EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator")
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }
    }
}
