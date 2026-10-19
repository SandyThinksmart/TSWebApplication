using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Constants;


namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StraighteningController : ControllerBase
    {
        private readonly IStraighteningService _straighteningService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        public StraighteningController(IStraighteningService straighteningService, LocalizationServices localization, IAllCommonService commonService)
        {
            _straighteningService = straighteningService;
            _localization = localization;

            _commonService = commonService;
        }
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
        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            // string message = ValidateSearchFields(request);
            // if (string.IsNullOrEmpty(message))
            // {
            var result = _straighteningService.GetStraighteningList(request);
            //var result = 0;
                return Ok(new { IsSuccess = true, message = result });
          //  }
          //  else
           // {
               // return Ok(new { IsSuccess = false, message = message });
           // }
        }

       
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _commonService.GetMachineName(Convert.ToString(UIConstants.K_MACHINEGROUP_STRAIGHTENING));
            // var result = _commonService.GetMachineName();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProcessDefect")]
        public ActionResult GetProcessDefect()
        {
            var result = _commonService.GetProcessDefectList(UIConstants.K_PROCESS_MULTI_ROLL_STRIGHENING);
            return Ok(new { IsSuccess = true, message = result });
        }

     
        [HttpPost("EmployeeList")]
        public IActionResult EmployeeList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        //[HttpGet("InitializeSortPopup")]
        //public ActionResult InitializeSortPopup()
        //{
        //    var sortFields = new List<TSSortField> {
        //                                new TSSortField {
        //                                    FieldName = "PP.CHUUBAN",
        //                                    DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
        //                                    IsDefaultSort =true,
        //                                     SortOrder=TSSortDirection.Ascending
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "ST.STR_TABA_NO_1,ST.STR_TABA_NO_2,ST.STR_TABA_NO_3,ST.STR_TABA_NO_4",
        //                                    DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),
        //                                    IsDefaultSort =true
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "PP.PROD_NAME_CUST",
        //                                    DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust")
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "INTE.INTERNAL_NAME",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName")
        //                                },

        //                                new TSSortField {
        //                                    FieldName = "PP.PROD_SHAPE",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape")
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "PP.PROD_LENGTH",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Length")
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD")
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "ST.STR_START_OPERATOR_CD",
        //                                    DisplayName =  _localization.GetLocalizedHtmlString("lbl_OperationDay")
        //                                },
        //                                  new TSSortField {
        //                                    FieldName = "EMP.YOBINA,EMP.YOBIGANA,EMP.SHORT_NAME",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator")
        //                                }
        //                            };
        //    return Ok(new { IsSuccess = true, message = sortFields });
        //}

        [HttpPost("StraighteningDetailsDelete")]
        public ActionResult StraighteningDetailsDelete([FromBody] StraighteningDetails request)
        {
            _straighteningService.Delete(request);
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpGet("GetStraighteningDetailsByStraighteningCD")]
        public ActionResult GetStraighteningDetailsByStraighteningCD(long STR_STRAIGHTENING_CD)
        {
            var result = _straighteningService.GetStraighteningDetailsByCD(STR_STRAIGHTENING_CD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByChuuban")]
        public ActionResult GetThabaDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _straighteningService.GetThabaDetailsByChuuban(request.CHUUBAN, Convert.ToString(request.GYOUBAN), Convert.ToInt64(request.STRAIGHTENING_CD));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByChuubanGyouban")]
        public ActionResult GetThabaDetailsByChuubanGyouban([FromBody] GridSearchFields request)
        {
            var result = _straighteningService.GetThabaDetailsByChuuban(request.CHUUBAN, Convert.ToString(request.GYOUBAN));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("StraighteningDetailsUpdate")] 
        public ActionResult StraighteningDetailsUpdate([FromBody] StraighteningDetails request) 
        {
          var msgCD = _straighteningService.StraighteningDetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = msgCD });
        }

        [HttpPost("GetStraighteningDetailsByChuuban")]
        public ActionResult GetStraighteningDetailsByChuuban([FromBody] GridSearchFields request)
        {
            var result = _straighteningService.GetStraighteningDetailsByChuuban(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetStraighteningHistoryByChuuban")]
        public ActionResult GetStraighteningHistoryByChuuban([FromBody] GridSearchFields request)
        {
            var result = _straighteningService.GetStraighteningHistoryByChuuban(request.CHUUBAN, Convert.ToString(request.GYOUBAN),request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("StraighteningDetailsSave")]
        public ActionResult StraighteningDetailsSave([FromBody] StraighteningDetails request)
        {
            var result = _straighteningService.InsertStraighteningDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD, string cultureName)
        {
            var result = _straighteningService.GetStraightening24MachineList(machineCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetStraighteningDailyReportOnFactoryDailyWorkStatus")]
        public IActionResult GetStraighteningDailyReportOnFactoryDailyWorkStatus([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _straighteningService.GetStraighteningDailyReportOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetStraighteningDailyReportOnDailyProcessAnalysis")]
        public IActionResult GetStraighteningDailyReportOnDailyProcessAnalysis([FromBody] GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _straighteningService.GetStraighteningDailyReportOnDailyProcessAnalysis(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
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

            //if (!string.IsNullOrEmpty(request.txtOperationDateStart))
            //{
            //    bool IsPODateOfStart = false;

            //    if (UIHelper.IsValidDate(request.txtOperationDateStart))
            //    {
            //        IsPODateOfStart = true;
            //    }
            //    if (IsPODateOfStart == false)
            //    {
            //        validationMsg += Constants.StringValues.NEW_LINE;
            //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_StartDateInvalid_M");
            //    }

            //    if (request.txtOperationDateEnd.Length > 0)
            //    {
            //        bool IsPODateOfEnd = false;

            //        if (UIHelper.IsValidDate(request.txtOperationDateEnd))
            //        {
            //            IsPODateOfEnd = true;
            //        }
            //        if (IsPODateOfEnd == false)
            //        {
            //            validationMsg += Constants.StringValues.NEW_LINE;
            //            validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_EndDateInvalid_M");
            //        }
            //    }
            //}

            //if (string.IsNullOrEmpty(request.txtOperationDateStart?.Trim()))
            //{
            //    validationMsg += Constants.StringValues.NEW_LINE;
            //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_ProcessDateStartRequired");
            //}
            return validationMsg;
        }

        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }

        [HttpGet("GetMachineNamebyCD")]
        public IActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpGet("GetOperatorNamebyCD")]
        public ActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            //var result = _chamfferService.GetEmployeeNameByID(OperatorCD);
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            //if (result != null)
            //{
            //    int flag;
            //    flag = Convert.ToInt32(_commonService.GetEmployeeNameActiveID(OperatorCD));

            //    if (flag == 1)
            //    {
            //        // result = "Inactive";
            //    }


            //}


            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIConstants.K_PROCESS_MULTI_ROLL_STRIGHENING));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetTPTest")]
        public ActionResult GetTPTest()
        {
            var result = _straighteningService.GetTPTestList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetUnitType")]
        public ActionResult GetUnitType()
        {
            var result = _straighteningService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetExternalSurfaceCondition")]
        public ActionResult GetExternalSurfaceCondition()
        {
            var result = _straighteningService.GetExternalSurfaceConditionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetBendStatus")]
        public ActionResult GetBendStatus()
        {
            var result = _straighteningService.GetGetBendStatusList();
            return Ok(new { IsSuccess = true, message = result });
        }
        
        //[HttpGet("GetMachineList")]
        //public IActionResult GetMachineList()
        //{
        //    var result = _straighteningService.GetMachineNameList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] GridSearchFields request)
        {
            bool returnval = false;
            string returnData = _straighteningService.GetThabaDetailsByThabaNumber(request);
            if (string.IsNullOrEmpty(returnData))
            {
                returnval = true;
            }
            return Ok(new { IsSuccess = returnval, message = returnData });
        }

        //Added Date Range
        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;

            DateTime minDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            DateTime maxDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
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
        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetRowCountForStraighteningFactory")]
        public ActionResult GetRowCountForStraighteningFactory(GridSearchFields gridSearchRequest)
        {
            int count = _straighteningService.GetRowCountForStraighteningFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForStraighteningDailyAnalysis")]
        public ActionResult GetRowCountForStraighteningDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _straighteningService.GetRowCountForStraighteningDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpGet("GetStraighteningMachineList")]
        public IActionResult GetStraighteningMachineList()
        {
            var result = _straighteningService.GetStraighteningMachineList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("MappNifudaByBarcode")]
        public IActionResult MappNifudaByBarcode(string nifudaNo)
        {
            bool returnval = false;
            var result = _commonService.MappNifudaByBarcode(nifudaNo);

            if (result.NIFUDA_NO != null)
            {
                returnval = true;
            }

            return Ok(new { IsSuccess = returnval, message = result });
        }

    }
}
