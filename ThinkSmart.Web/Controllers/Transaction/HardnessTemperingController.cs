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
    public class HardnessTemperingController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly IHardnessTemperingService _hardnessTemperingService;
        private readonly string unitName = string.Empty;
        public HardnessTemperingController( LocalizationServices localization, IHardnessTemperingService hardnessTemperingService, IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
            _localization = localization;
            _hardnessTemperingService = hardnessTemperingService;
        }
        public IActionResult HardnessTemperingEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            ViewBag.UnitType = Convert.ToInt16(UIUtils.GetUnitIDByName(Convert.ToString(UIConstants.K_UNIT_COIL)));
            return View();
        }
        public IActionResult HardnessTemperingEditDelete()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            ViewBag.UnitType = Convert.ToInt16(UIUtils.GetUnitIDByName(Convert.ToString(UIConstants.K_UNIT_COIL)));
            return View();
        }
        public IActionResult HardnessTemperingListPage()
        {
            //ViewBag.FromEntryEditDelete = fromEntryEditDelete;
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult HardnessTemperingEntryEditDelete()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
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
       
        // public IActionResult GetNextProcess()
        //{
        //    var result = _hardnessTemperingService.GetNextProcessList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        public IActionResult GetDecision()
        {
            var result = _hardnessTemperingService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetTemperingType()
        {
            var result = _hardnessTemperingService.GetTempTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetTextureType()
        {
            var result = _hardnessTemperingService.GetTextureTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
       

        public IActionResult InitializeSortPopup(bool isEntryEditDeleteScreen = false)
        {
            
                
            if (isEntryEditDeleteScreen != true)
            {
               var sortFields = new List<TSSortField> {

                                           

                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "HT.START_DATE_HARDENING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                            FieldName = "HT.FURNACE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                             IsDefaultSort=true,
                                             SortOrder=TSSortDirection.Ascending
                                        },


                                            new TSSortField {
                                            FieldName =   "EMP.YOBINA, EMP.YOBIGANA, EMP.SHORT_NAME" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimD"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                           new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },




                                    };
                return Json(new { IsSuccess = true, message = sortFields });
            }
            else
            {
                var sortFields = new List<TSSortField> {

                                           

                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                          new TSSortField {
                                            FieldName = "HTQ.INSPECTION_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                          new TSSortField {
                                            FieldName = "HT.FURNACE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                             IsDefaultSort=false,
                                             SortOrder=TSSortDirection.Ascending
                                        },


                                            new TSSortField {
                                            FieldName =   "EMP.YOBINA, EMP.YOBIGANA, EMP.SHORT_NAME" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimD"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                           new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },




                                    };
                return Json(new { IsSuccess = true, message = sortFields });
            }

            
        }

        public IActionResult GridSearch([FromBody] List<HardeningTemperingDetails> details)
        {
            foreach (var item in details)
            {

                item.UNIT_STRING = item.NO_OF_UNITS + " " + UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT_TYPE));
            }
            string view = _viewRenderService.RenderToString("HardnessTempering/HardnessTemperingListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult FillGrid([FromBody] List<HardeningTemperingDetails> details)
        {
            string view = _viewRenderService.RenderToString("HardnessTempering/HardnessTemperingListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
    }
}
