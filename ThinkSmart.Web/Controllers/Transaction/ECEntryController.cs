using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ECEntryController : Controller
    {

        private readonly IECService _ecService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        public ECEntryController(LocalizationServices localization, IECService ecService,IViewRenderService viewRenderService)
        {

            _localization = localization;
            _ecService = ecService;
            _viewRenderService = viewRenderService;
        }
        public IActionResult EcEntry()
        {
           
            ViewBag.CultureName = UIHelper.GetCultureName();  
            return View();
        }
        public IActionResult ECEditDelete()
        {
            string a = Convert.ToString(UIHelper.GetCultureName());
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

        public IActionResult GridSearch([FromBody] List<ECDetails> details)
        {
            foreach (ECDetails record in details)
            {
                record.EC_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.EC_DECISION));
                if (record.EC_OK_BARS > 0)
                {
                    record.EC_OK_BARS_STRING = record.EC_OK_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                }
                
            }

            string view = _viewRenderService.RenderToString("ECEntry/ECGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public ActionResult DisplayInactivePopup(string nameValue, string tableName)
        {
            string validationMsg = string.Empty;
            if (tableName == "machine")
            {
                validationMsg += UIHelper.GetErrorMessageForInactiveMaster(nameValue, UIConstants.MasterName.MST_MACHINE, UIConstants.ActiveInactiveLabel.MST_MACHINE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_MACHINE_ACTIVE);
            }else
            {
                validationMsg += UIHelper.GetErrorMessageForInactiveMaster(nameValue, UIConstants.MasterName.MST_EMPLOYEE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_ACTIVE);
            }

            return Ok(new { message = validationMsg });
        }

        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName =   "ec.ec_taba_no" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "EC_START_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                            IsDefaultSort=false,
                                           
                                        },
                                        new TSSortField {
                                            FieldName =   "EC_MACHINE_NAME" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                            IsDefaultSort=false,
                                            
                                        },
                                        new TSSortField {
                                            FieldName = "cst.YOBINA, cst.YOBIGANA,cst.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                            IsDefaultSort=false,
                                        },
                                                                                 
                                            new TSSortField {
                                            FieldName = "pp_prod_name_cust",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=false,
                                        },
                                              new TSSortField {
                                            FieldName = "PP.PROD_SHAPE,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdShape"),
                                            IsDefaultSort=false,
                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",//,PP.PROD_DIM_W,PP.PROD_DIM_OTHER
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                            IsDefaultSort=false,
                                        },
                                                  new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductLength"),
                                            IsDefaultSort=false,
                                        },
                                                    new TSSortField {
                                            FieldName = "EC_START_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),
                                             IsDefaultSort=false,

                                        }
                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
        public IActionResult GetTPTestList()
        {
            var result = _ecService.GetTPTestList();
            return Ok(new { IsSuccess = true, message = result });
        }
        //public IActionResult GetNextProcess()
        //{
        //    var result = _ecService.GetNextProcessList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        public IActionResult GetDecision()
        {
            var result = _ecService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetFIDropdownList()
        {
            var result = _ecService.GetFIResultList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetProductUnitKubun()
        {
            var result = _ecService.GetProductUnitKubun();
            return Ok(new { IsSuccess = true, message = result });
        }
        //[HttpPost]
        //public IActionResult GetMachineNamebyCD(long machineCD)
        //{
        //    //var result = _ecService.GetMachineNameByID(machineCD);
        //    return Json(new { IsSuccess = true, message = result });
        //}

        //[HttpPost]
        //public IActionResult GetMachineDetails(string machineCD)
        //{
        //    //var result = _ecService.GetECLast24HrMachineData(machineCD);
        //    return Json(new { IsSuccess = true, message = result });
        //}


    }
}
