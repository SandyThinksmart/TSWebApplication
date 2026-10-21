using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HikinukiBarToBarController : ControllerBase
    {
        private readonly IHikinukiBarToBarService _hikinukibartobarService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        private readonly IAllCommonService _allCommonService;
        public HikinukiBarToBarController(LocalizationServices localization, IHikinukiBarToBarService hikinukibartobarService, IAllCommonService commonService, IAllCommonService allCommonService)
        {
            _localization = localization;
            _hikinukibartobarService = hikinukibartobarService;
            _commonService = commonService;
            _allCommonService = allCommonService;
        }
        //public IActionResult HikinukiBarToBarEntry(int userId = 0)
        //{
        //    //int locationCd = _stamppingService.GetUserLocationById(userId);
        //  //  ViewBag.UserId = userId;
        //   // return View();
        //}
        [HttpPost("MappChuubanAndGyoubanByBarcode")]
        public IActionResult MappChuubanAndGyoubanByBarcode(string orderNo)
        {
            bool returnval = false;
            var result = _commonService.MappChuubanAndGyoubanByBarcode(orderNo);

            if (result.ORDER_NO_IN_CHUUBAN != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }
        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }
        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;
            DateTime dateTime = _commonService.GetSystemDateTime();
            DateTime minDate = Convert.ToDateTime(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            DateTime maxDate = Convert.ToDateTime(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS)); ;
            minDate = minDate.AddDays(-request.DATERANGE);
            maxDate = maxDate.AddDays(+request.DATERANGE);

            if (Convert.ToDateTime(request.RANGE_DATETIME) < Convert.ToDateTime(minDate) || Convert.ToDateTime(request.RANGE_DATETIME) > Convert.ToDateTime(maxDate))
            {
                returnval = false;
            }
            else
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _hikinukibartobarService.GetHikinukiBarToBarList(request);
            return Ok(new { IsSuccess = true, message = result });
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string validationMsg = string.Empty;
            if ((request.PROD_DIM_D_MAX > 0) && (request.PROD_DIM_D_MIN > 0))
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");

            }
            if ((request.PROD_LENGTH_MAX > 0) && (request.PROD_LENGTH_MIN > 0))
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }
            return validationMsg;
        }

        [HttpPost("GetThabaDetailsByChuuban")]
        public ActionResult GetThabaDetailsByChuuban([FromBody] ThabaDetails request)
        {
            var result = _hikinukibartobarService.GetThabaDetailsByChuuban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GethikinukibartobarDetailsByChuuban")]
        public ActionResult GethikinukibartobarDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _hikinukibartobarService.GetHikinukibartobarDetailsByChuuban(request.CHUUBAN, request.GYOUBAN, request.MACHINE_CD, request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD, string cultureName)
        {
            var result = _hikinukibartobarService.GetHikinukiBarToBar24MachineList(machineCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpPost("HikinukiBarToBarDetailsSave")]
        public ActionResult HikinukiBarToBarDetailsSave([FromBody] HikinukiBarToBarDetails request)
        { 
            var result = _hikinukibartobarService.SaveHikinukiBarToBar(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("HikinukiBarToBarDetailsUpdate")]
        public ActionResult HikinukiBarToBarDetailsUpdate([FromBody] HikinukiBarToBarDetails request)
        {
         long msgCd=   _hikinukibartobarService.HikinukiBarToBarDetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = msgCd });
        }

        [HttpGet("GetHikinukiDetailsByDrawingCD")]
        public ActionResult GetHikinukiDetailsByDrawingCD(long DrawingCD,string cultureName)
        {
            var result = _hikinukibartobarService.GetHikinukiDetailsDrawingByCD(DrawingCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("CheckOperatorIncativeActive")]
        public ActionResult CheckOperatorIncativeActive(long OperatorCD)
        {
            string validationMsg = string.Empty;
            var result = _commonService.IsCheckEmployeeActiveInctiveByID(OperatorCD);
            if (result.Value == "inactive")
            {
                validationMsg += UIHelper.GetErrorMessageForInactiveMaster(result.Key, UIConstants.MasterName.MST_EMPLOYEE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_INACTIVE, UIConstants.ActiveInactiveLabel.MST_EMPLOYEE_ACTIVE);
                return Ok(new { IsSuccess = false, message = validationMsg });
            }
            else
            {
                return Ok(new { IsSuccess = true, message = result });

            }

        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingBtoB"))));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("InitializeSortPopup")]
        public ActionResult InitializeSortPopup()
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
                                            FieldName = "PP.PROD_SHAPE,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
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
                                            FieldName = "DW.DW_START_OPERATOR_CD",
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
                                            FieldName = "PP.CHUUBAN",
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
            return Ok(new { IsSuccess = true, message = sortFields });
        }

        [HttpPost("HikinukiDetailsDelete")]
        public ActionResult HikinukiDetailsDelete([FromBody] HikinukiBarToBarDetails request)
        {
            _hikinukibartobarService.Delete(request);
            return Ok(new { IsSuccess = true, message = "" });
        }
        [HttpPost("GetDrawingBarToBarDataOnFactoryDailyWorkStatus")]
        public IActionResult GetDrawingBarToBarDataOnFactoryDailyWorkStatus([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _hikinukibartobarService.GetDrawingBarToBarDataOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }


        [HttpPost("GetDailyProcessDrawingDGrid")]
        public IActionResult GetDailyProcessAnalysisDrawingDGrid(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _hikinukibartobarService.GetDailyProcessAnalysisHikinukiBarToBar(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetDieDiaShape")]
        public IActionResult GetDieDiaShape()
        {
            var result = _hikinukibartobarService.GetDieDiaShapeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
       
        [HttpPost("GetProcessDefect")]
        public IActionResult GetProcessDefect()
        {
            var result = _commonService.GetProcessDefectList(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessDrawingBtoB")));
            return Ok(new { IsSuccess = true, message = result });
        }    
       
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        {
            var result = _hikinukibartobarService.GetThabaDetailsByThabaNumber(request.CHUUBAN, request.GYOUBAN, request.TABA_NO);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList(string machineType)
        {
            var result = _allCommonService.GetMachineName(machineType);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForHikinukiBarToBarFactory")]
        public ActionResult GetRowCountForHikinukiBarToBarFactory(GridSearchFields gridSearchRequest)
        {
            int count = _hikinukibartobarService.GetRowCountForHikinukiBarToBarFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForHikinukiBarToBarDailyAnalysis")]
        public ActionResult GetRowCountForHikinukiBarToBarDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _hikinukibartobarService.GetRowCountForHikinukiBarToBarDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpGet("GetHikinukiBarToBarMachineList")]
        public IActionResult GetHikinukiBarToBarMachineList()
        {
            var result = _hikinukibartobarService.GetHikinukiBarToBarMachineList();
            return Ok(new { IsSuccess = true, message = result });
        }
    }
}
