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
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class StamppingController : Controller
    {
       
        private readonly IStamppingService _stamppingService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
     
        public StamppingController(LocalizationServices localization, IStamppingService stamppingService,IViewRenderService viewRenderService)
        {
           
            _localization = localization;
            _stamppingService = stamppingService;
            _viewRenderService = viewRenderService;
        }
        public IActionResult StamppingEntry()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }


        public IActionResult StamppingEditDelete()
        {
  
            ViewBag.CultureName = UIHelper.GetCultureName();
            //ViewBag.UserId = 1; // use this when connected to oracle19C DB
            return View();
        }

        //public IActionResult EmployeeList(string name = "", bool addEmpty = true)
        //{
        //    var lst = _stamppingService.GetFilteredEmployeeNameList(name, addEmpty);
        //    return Json(new { IsSuccess = true, message = lst });
        //}

        //public IActionResult GetProcessDefect()
        //{
        //    var result = _stamppingService.GetProcessDefectList();
        //    return Json(new { IsSuccess = true, message = result });
        //}

        //public IActionResult GetNextProcess()
        //{
        //    var result = _stamppingService.GetNextProcessList();
        //    return Json(new { IsSuccess = true, message = result });
        //}



        //public IActionResult GetStamppingDetailsByStamppingCD(long stamppingCD)
        //{
        //    var result = _stamppingService.GetStamppingDetailsByStamppingCD(stamppingCD);            
        //    return Json(new { IsSuccess = true, message = result });
        //}

        //public IActionResult GetMachineList()
        //{
        //    var result = _stamppingService.GetMachineNameList();
        //    return Json(new { IsSuccess = true, message = result });
        //}

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
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                              new TSSortField {
                                            FieldName = "CST.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "ST_END_DATE_STRING",
                                            DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                          new TSSortField {
                                            //FieldName = "PP.EMPLOYEE_NAM --DW_START_OPERATOR_NAMEE",
                                            FieldName = "ST_END_OPERATOR_NAME",
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
                                            FieldName = "ST.ST_TABA_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }


                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        //public IActionResult GetStampingDetailsByChuuban([FromBody]DetailsByChuubanRequest request)
        //{
        //    StamppingDetails stamppingDetails = _stamppingService.GetStamppingDetailsByChuuban(request.chuuban,request.gyouban,request.mechineCD, request.nifudaNo);
        //    return Json(new { IsSuccess = true, message = stamppingDetails });
        //}


        //public IActionResult GetMachineDetails(string machineCD)
        //{
        //    var result = _stamppingService.GetStampping24MachineList(machineCD);
        //    return Json(new { IsSuccess = true, message = result });
        //}

        //public IActionResult StamppingDetailsSave([FromBody] StamppingUpdateRequest request)
        //{
        //    var result = _stamppingService.InsertStamppingDetails(request);
        //    return Json(new { IsSuccess = true, message = result });
        //}

        //private string ValidateSearchFields(StamppingGridSearchRequest request)
        //{
        //    string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
        //    string validationMsg = string.Empty;
        //    if (!string.IsNullOrEmpty(request.txtDimDMax?.Trim()) && string.IsNullOrEmpty(request.txtDimDMin?.Trim()))
        //    {
        //        validationMsg += Constants.StringValues.NEW_LINE;
        //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
        //    }
        //    if (!string.IsNullOrEmpty(request.txtLengthSearchMax?.Trim())&& string.IsNullOrEmpty(request?.txtLengthSearchMin?.Trim()))
        //    {
        //        validationMsg += Constants.StringValues.NEW_LINE;
        //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
        //    }

        //   return validationMsg;
        //}
        public IActionResult GridSearch([FromBody] List<StamppingDetails> details)
        {
            foreach (StamppingDetails record in details)
            {
                record.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(record.ST_OK_BARS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                //record.ST_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.ST_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.ST_DECISION));            
            }
            string view = _viewRenderService.RenderToString("Stampping/StamppingGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetNextProcess()
        {
            var result = _stamppingService.GetNextProcessList();
            return Json(new { IsSuccess = true, message = result });
        }
        public IActionResult GetDecision()
        {
            var result = _stamppingService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetUnitType()
        {
            var result = _stamppingService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public ActionResult MachineIsInactive(string machineName)
        {
            string validationMsg = string.Empty;

            validationMsg += UIHelper.GetErrorMessageForInactiveMaster(machineName, _localization.GetLocalizedHtmlString("lbl_MachineMaster"), _localization.GetLocalizedHtmlString("lbl_Remove"), _localization.GetLocalizedHtmlString("lbl_ReInstall"));
            return Ok(new { message = validationMsg });


        }
        public ActionResult OperatorIsInactive(string operatorName)
        {
            string validationMsg = string.Empty;

            validationMsg += UIHelper.GetErrorMessageForInactiveMaster(operatorName, _localization.GetLocalizedHtmlString("lbl_EmployeeMaster"), _localization.GetLocalizedHtmlString("lbl_InActiveEmployee") , _localization.GetLocalizedHtmlString("lbl_ActiveEmployee"));
            return Ok(new { message = validationMsg });
        }
    }
    
}
