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
using TSWebApplication.Application.IServices;
using System.Globalization;
using Microsoft.AspNetCore.Http;


namespace TSWebApplication.Web.Controllers.Transaction
{
    public class HikinukiBarToBarController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IHikinukiBarToBarService _hikinukibartobarService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;

        public HikinukiBarToBarController(IAllCommonService commonService, LocalizationServices localization, IHikinukiBarToBarService hikinukibartobarService, IViewRenderService viewRenderService)
        {

            _commonService = commonService;
            _localization = localization;
            _hikinukibartobarService = hikinukibartobarService;
            _viewRenderService = viewRenderService;
        }
        public IActionResult HikinukiBarToBarEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult HikinukiBarToBarEditDelete()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult GetHTMaterial()
        {
            var result = _hikinukibartobarService.GetHTMaterialList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetUnitType()
        {
            var result = _hikinukibartobarService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetExteriorCondition()
        {
            var result = _hikinukibartobarService.GetExteriorConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetBendStatus()
        {
            var result = _hikinukibartobarService.GetBendStatusList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDecision()
        {
            var result = _hikinukibartobarService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GridSearch([FromBody] List<HikinukiBarToBarDetails> details)
        {
            foreach (HikinukiBarToBarDetails record in details)
            {
                record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.PP_UNIT));
               
            }
            string view = _viewRenderService.RenderToString("HikinukiBarToBar/HikinukiBarToBarGrid", details);
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
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"), //1                                             
                                            SortOrder=TSSortDirection.Descending
                                        },
                                              new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"), //7
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                               new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"), //3
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),//2
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"), //4
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                            FieldName = "DW.DW_START_DATE",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay"), //5
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                                 FieldName = "EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"), //6
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                           new TSSortField {
                                            FieldName = "DW.DW_TABA_NO_",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (request.PROD_DIM_D_MAX > 0 && request.PROD_DIM_D_MIN > 0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
            }
            if (request.PROD_LENGTH_MAX > 0 && request.PROD_LENGTH_MIN > 0)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }

            if (request.START_DATETIME.Length > 0)
            {
                bool IsPODateOfStart = false;

                if (UIHelper.IsValidDate(request.START_DATETIME))
                {
                    IsPODateOfStart = true;
                }
                if (IsPODateOfStart == false)
                {
                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_StartDateInvalid_M");
                }

                if (request.END_DATETIME.Length > 0)
                {
                    bool IsPODateOfEnd = false;

                    if (UIHelper.IsValidDate(request.END_DATETIME))
                    {
                        IsPODateOfEnd = true;
                    }
                    if (IsPODateOfEnd == false)
                    {
                        validationMsg += Constants.StringValues.NEW_LINE;
                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_EndDateInvalid_M");
                    }
                }
            }

            if (string.IsNullOrEmpty(request.START_DATETIME?.Trim()))
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_ProcessDateStartRequired");
            }
            return validationMsg;
        }
    }
    }
