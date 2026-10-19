using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChamfferController : ControllerBase
    {
        private readonly IChamfferService _chamfferService;        
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;       
        public ChamfferController(IChamfferService chamfferService, LocalizationServices localization, IAllCommonService commonService)
        {
            _chamfferService = chamfferService;           
            _localization = localization;
            _commonService = commonService;           
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
        public ActionResult GetSysDateTime()
        {
            //string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            //return Ok(new { IsSuccess = true, message = dateTime });

            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });
        }

        [HttpGet("GetMachineNamebyCD")]
        public ActionResult GetMachineNamebyCD(long machineCD)
        {
            var result = _commonService.GetMachineNameByID(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineDetails")]
        public IActionResult GetMachineDetails(string machineCD)
        {
            var result = _chamfferService.GetChamfering24MachineList(machineCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetChamferDetailsByChuuban")]
        public ActionResult GetChamferDetailsByChuuban([FromBody] ChamferDetails request)
        {
            var result = _chamfferService.GetChamferDetailsByChuuban(request.PP_CHUUBAN,Convert.ToString(request.PP_GYOUBAN), Convert.ToString(request.MACHINE_CD), request.NIFUDA_STRING, request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByChuuban")]
        public ActionResult GetThabaDetailsByChuuban([FromBody] ChamferDetails request)
        {
            var result = _chamfferService.GetThabaDetailsByChuuban(request.PP_CHUUBAN, Convert.ToString(request.PP_GYOUBAN));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetOperatorNamebyCD")]
        public ActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            //var result = _chamfferService.GetEmployeeNameByID(OperatorCD);
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("ChamferDetailsSave")]
        public ActionResult ChamferDetailsSave([FromBody] ChamferDetails request)
        {
            var result = _chamfferService.InsertChamferDetails(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("ChamferDetailsUpdate")]
        public IActionResult ChamferDetailsUpdate([FromBody] ChamferDetails request)
        {
            long msgCd =_chamfferService.ChamferDetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = msgCd });
        }

        [HttpPost("ChamferDetailsDelete")]
        public IActionResult ChamferDetailsDelete([FromBody] ChamferDetails request)
        {
            _chamfferService.Delete(request);
           
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            string message = ValidateSearchFields(request);
            if (string.IsNullOrEmpty(message))
            {
                var result = _chamfferService.GetChamfferList(request);                
                return Ok(new { IsSuccess = true, message = result });
            }
            else
            {
                return Ok(new { IsSuccess = false, message = message });
            }
        }
        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(request.PROD_DIM_D_MAX)?.Trim()) && string.IsNullOrEmpty(Convert.ToString(request.PROD_DIM_D_MIN)?.Trim()))
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(request?.PROD_LENGTH_MAX)?.Trim()) && string.IsNullOrEmpty(Convert.ToString(request?.PROD_LENGTH_MIN)?.Trim()))
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_LengthStartRequired");
            }

            if (!string.IsNullOrEmpty(request.START_DATETIME))
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

            //if (string.IsNullOrEmpty(request.txtOperationDateStart?.Trim()))
            //{
            //    validationMsg += Constants.StringValues.NEW_LINE;
            //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_ProcessDateStartRequired");
            //}
            return validationMsg;
        }

        [HttpPost("EmployeeList")]        
        public IActionResult EmployeeList(string? name = "", bool addEmpty = true)
        {
            var lst = _chamfferService.GetEmployeeNameList(name, addEmpty);
            //var lst = _allCommonService.GetEmployeeNameList(name, addEmpty);
            return Ok(new { IsSuccess = true, message = lst });
        }

        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD=0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetChamferingDetailsByChamferingCD")]
        public IActionResult GetChamferingDetailsByChamferingCD(long chamferingCD,string cultureName)
        {
            var result = _chamfferService.GetChamferingDetailsByCD(chamferingCD, cultureName);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _commonService.GetMachineName(Convert.ToString(UIConstants.K_MACHINEGROUP_CHAMFERING));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetConditionName")]
        public IActionResult GetConditionName()
        {
            var result = _commonService.GetConditionName();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessChamferS"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("InitializeSortPopup")]
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
                                            FieldName = "CH.CH_START_OPERATOR_CD",
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
                                            FieldName = "PP.CHUUBAN",
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
              
        
        [HttpGet("GetUnitType")]
        public IActionResult GetUnitType()
        {
            var result = _chamfferService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetThabaDetailsByThabaNumber")]
        public IActionResult GetThabaDetailsByThabaNumber([FromBody] ChamferDetails request)
        {
            bool returnval = false;
            string returnData = _chamfferService.GetThabaDetailsByThabaNumber(request);
            if (string.IsNullOrEmpty(returnData))
            {
                returnval = true;
            }
            return Ok(new { IsSuccess = returnval, message = returnData });
        }
        [HttpPost("GetChamferDailyReportOnFactoryDailyWorkStatus")]
        public IActionResult GetChamferDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)      
        {
            bool isSuccess = false;
            var result = _chamfferService.GetChamferDailyReportOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetChamferDetailsOnDailyProcessAnalysis")]
        public IActionResult GetChamferDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _chamfferService.GetChamferDetailsOnDailyProcessAnalysis(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
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

        [HttpPost("GetRowCountForChamfferFactory")]
        public ActionResult GetRowCountForChamfferFactory(GridSearchFields gridSearchRequest)
        {
            int count = _chamfferService.GetRowCountForChamfferFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForChamfferDailyAnalysis")]
        public ActionResult GetRowCountForChamfferDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _chamfferService.GetRowCountForChamfferDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
    }
}







