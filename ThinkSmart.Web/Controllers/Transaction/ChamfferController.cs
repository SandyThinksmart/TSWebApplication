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

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ChamfferController : Controller
    {
        private readonly IChamfferService _chamfferService;
        //private readonly IStamppingService _stamppingService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        public ChamfferController(IChamfferService chamfferService, LocalizationServices localization,  IViewRenderService viewRenderService)
        {
            _chamfferService = chamfferService;
            _localization = localization;
            //_stamppingService = stamppingService;
            _viewRenderService = viewRenderService;
        }
        public IActionResult ChamfferEntry()
        {
 
            ViewBag.UserId = TempData["userId"];
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult ChamfferEditDelete()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

        public IActionResult ChamfferEditDelete1()
        {         
            return View();
        }
        public IActionResult GetDecision()
        {
            var result = _chamfferService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GridSearch([FromBody] List<ChamferDetails> details)
        {
            foreach (ChamferDetails record in details)
            {
                record.CH_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.CH_DECISION));
            }
            string view = _viewRenderService.RenderToString("Chamffer/ChamfferGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GetChamferingDetailsByChamferingCD(long chamferingCD, string cultureName)
        {
            var result = _chamfferService.GetChamferingDetailsByCD(chamferingCD, cultureName);
            return Json(new { IsSuccess = true, message = result });
        }

        public IActionResult GetThabaDetailsByChuuban([FromBody] ChamferDetails request)
        {
            var result = _chamfferService.GetThabaDetailsByChuuban(request.PP_CHUUBAN,Convert.ToString(request.PP_GYOUBAN), request.CHAMFERING_CD);
            return Json(new { IsSuccess = true, message = result });
        }

        public IActionResult ChamferDetailsDelete(long chamferingCD)
        {
         //   _chamfferService.Delete(chamferingCD);
            return Json(new { IsSuccess = true, message = "" });
        }

        [HttpPost]
        public IActionResult ChamferDetailsUpdate([FromBody] ChamferDetails request)
        {
            _chamfferService.ChamferDetailsUpdate(request);
            return Json(new { IsSuccess = true, message = "" });
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
        public IActionResult GetUnitType()
        {
            var result = _chamfferService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                           new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
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
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                            FieldName = "CH.CH_START_DATE",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                            //FieldName = "PP.EMPLOYEE_NAM --DW_START_OPERATOR_NAMEE",
                                            FieldName = "EMP.YOBINA,EMP.YOBIGANA,EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),
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
                                            FieldName = "CH.CH_TABA_NO_1",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }

        
    }
}
