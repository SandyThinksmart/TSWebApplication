using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HeatTreatmentController : ControllerBase
    {
        private readonly IHeatTreatmentService _heatTreatmentService;
        private readonly IStamppingService _stamppingService;
        private readonly IAllCommonService _commonService;
        private readonly LocalizationServices _localization;
        private const int FixedRows = 30;
        List<string> ValidChuubanList = new List<string>();

        public HeatTreatmentController(IHeatTreatmentService heatTreatmentService, LocalizationServices localization, IStamppingService stamppingService, IAllCommonService commonService)
        {
            _heatTreatmentService = heatTreatmentService;
            _localization = localization;
            _stamppingService = stamppingService;
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
        [HttpGet("FillEmptyGrid")]
        public IActionResult FillEmptyGrid()
        {
            var result = new List<HeatTreatMentDetails>();
            for (int rowCount = 1; rowCount <= FixedRows; rowCount++)
            {
                result.Add(new HeatTreatMentDetails());
            }
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetHeatTreatmentEditDeleteStartEndDetails")]
        public IActionResult GetHeatTreatmentEditDeleteStartEndDetails(int? HeatTreatMentCd, bool isNull = true)
        {
            //string validationMsg = string.Empty;

            var result = _heatTreatmentService.GetHeatTreatmentEditDeleteStartEndDetails(HeatTreatMentCd, isNull);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetHeatTreatmentStartDetails")]
        public IActionResult GetHeatTreatmentStartDetails(int? machineCD, bool isNull = true)
        {
            //string validationMsg = string.Empty;

            var result = _heatTreatmentService.GetHeatTreatmentStartDetails(machineCD, isNull);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetHeatTreatmentMachineNameList")]
        public IActionResult GetHeatTreatmentMachineNameList()
        {
            var result = _heatTreatmentService.GetHeatTreatmentMachineNameList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _heatTreatmentService.GetMachineNameList();
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpGet("GetLocationList")]
        public IActionResult GetLocationList()
        {
            var result = _heatTreatmentService.GetLocationNameList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetFactoryLocationNameList")]
        public IActionResult GetFactoryLocationNameList()
        {
            var result = _heatTreatmentService.GetLocationNameList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetCustomerList")]
        public IActionResult GetCustomerList()
        {
            var result = _stamppingService.GetCustomerNameList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetMaterialKubun")]
        public IActionResult GetMaterialKubun()
        {
            var result = _heatTreatmentService.GetMaterialKubun();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetUnitKubunList")]
        public IActionResult GetUnitKubunList()
        {
            var result = _heatTreatmentService.GetUnitKubunList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProcessPlanData")]
        public IActionResult GetProcessPlanData([FromBody] ProcessPlanReportSearchRequest request)
        {
            bool isSuccess = false;
            // int result = _ProcessPlanDetails.GetInvoiceListCoilToCoilList(request);

            //if (result > 0)
            //{
            //    isSuccess = true;
            //}
            return Ok(new { IsSuccess = isSuccess });

        }
        [HttpGet("IsFurProcFurNoIsExist")]
        public IActionResult IsFurProcFurNoIsExist(int machineCD, string furnaceNo)
        {
            string validationMsg = string.Empty;

            bool isExist = _heatTreatmentService.IsFurProcFurNoIsExist(machineCD, furnaceNo);
            if (isExist)
            {
                validationMsg += Constants.StringValues.LINE_BREAK;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_FurnaceNoAlreadyExist");
                return Ok(new { IsSuccess = isExist, message = validationMsg });
            }
            return Ok(new { IsSuccess = isExist, message = "" });
        }


        [HttpPost("SaveStartData")]
        public IActionResult SaveStartData(List<HeatTreatMentDetails> heatTreats)
        {
            var result = _heatTreatmentService.SaveStartData(heatTreats);
            return Ok(new { IsSuccess = true, message = result });
        }

        //[HttpPost("SaveEndData")]
        //public IActionResult SaveEndData(List<HeatTreatMentDetails> heatTreats)
        //{
        //    _heatTreatmentService.UpdateEndData(heatTreats);
        //    return Ok(new { IsSuccess = true, message = "" });
        //}


        [HttpPost("UpdateEndData")]
        public IActionResult UpdateEndData([FromBody] List<HeatTreatMentDetails> request)
        {
            var result = _heatTreatmentService.UpdateEndData(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] List<HeatTreatMentDetails> request)
        {
            _heatTreatmentService.Delete(request);
            return Ok(new { IsSuccess = true, message = _heatTreatmentService });
        }

        [HttpPost("GridSearch")]
        public IActionResult GridSearch([FromBody] GridSearchFields request)
        {
            var result = _heatTreatmentService.GetHeatTreatmentList(request);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetHeatTreatmentDetailsOnFactoryDailyWOrkStatus")]
        public IActionResult GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _heatTreatmentService.GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetHeatTreatmentDetailsOnDailyProcessAnalysis")]
        public IActionResult GetHeatTreatmentDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _heatTreatmentService.GetHeatTreatmentDetailsOnDailyProcessAnalysis(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHeatTreatment"))));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("InitializeSortPopup")]
        public ActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                           FieldName = "HT_START_DATE",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime"),
                                           SortOrder=TSSortDirection.Descending

                                        },

                                            new TSSortField {
                                            FieldName = "MACHINE_CD",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceProc"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                            new TSSortField {
                                            FieldName = "FURNACE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                            IsDefaultSort=true,

                                        },
                                              new TSSortField {
                                            FieldName = "HT_CONDITION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcCondition"),


                                        },
                                              new TSSortField {
                                            FieldName = "PATTERN_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_PatternNo"),

                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }


        [HttpPost("SearchBychuuban")]
        public IActionResult SearchBychuuban(List<string> chuuban)
        {
            /// return 1 for Success
            /// return 2 for OrNo not found
            /// return 3  for OrNo not started from Handover
            /// return 4 for complete date is null record not found for OrNo
            /// 

            CommonFields resultDetails = new CommonFields();
            List<HeatTreatMentDetails> heatTreatment = new List<HeatTreatMentDetails>();

            foreach (var item in chuuban)
            {

                // var result = _commonService.MappChuubanAndGyoubanByBarcode(item);
                bool isCharExist = false;
                string gyouban = string.Empty;
                string chuubanVal = string.Empty;


                if (item.Length > 2)
                {
                    gyouban = item.Substring(item.Length - 2);
                    chuubanVal = item.Remove(item.Length - 2);
                }


                isCharExist = gyouban.Any(char.IsLower);
                if (isCharExist == false)
                {
                    isCharExist = gyouban.Any(char.IsUpper);
                }
                if (item.Length <= 2 || isCharExist == true)
                {
                    heatTreatment.Add(new HeatTreatMentDetails { CHUUBAN = item, CHUUBAN_CODE = 2 });
                }
                else
                {
                    resultDetails = _commonService.ValidateChubban(chuubanVal, Convert.ToInt16(gyouban));
                }


                if (resultDetails.RESPONCE == 1)
                {
                    var processPlanDetails = _heatTreatmentService.GetChuubanListForInvaidCheck(item);
                    if (processPlanDetails != null)
                    {
                        heatTreatment.Add(new HeatTreatMentDetails
                        {
                            CHUUBAN = item,
                            CUSTOMER_NAME = processPlanDetails.CUSTOMER_NAME,
                            PROD_NAME_CUST = processPlanDetails.PROD_NAME_CUST,
                            CHARGE = processPlanDetails.CHARGE,
                            MATL_DIM_STRING = processPlanDetails.MATL_DIM_STRING,

                            PROD_DIM_STRING = processPlanDetails.PROD_DIM_STRING,
                            PP_NO_OF_UNITS = processPlanDetails.PP_NO_OF_UNITS,
                            PP_UNIT = processPlanDetails.PP_UNIT,
                            PP_ACT_ALLOC_QTY = processPlanDetails.PP_ACT_ALLOC_QTY,
                            PROCESS_PLAN_CD = processPlanDetails.PROCESS_PLAN_CD,
                            CHUUBAN_CODE = 1
                        });
                    }

                }
                if (resultDetails.RESPONCE == 2)
                {
                    heatTreatment.Add(new HeatTreatMentDetails { CHUUBAN = item, CHUUBAN_CODE = 2 });
                }
                if (resultDetails.RESPONCE == 3)
                {
                    heatTreatment.Add(new HeatTreatMentDetails { CHUUBAN = item, CHUUBAN_CODE = 3 });
                }
                if (resultDetails.RESPONCE == 4)
                {
                    heatTreatment.Add(new HeatTreatMentDetails { CHUUBAN = item, CHUUBAN_CODE = 4 });
                }

            }

            return Ok(new { IsSuccess = true, message = heatTreatment });

        }

        private List<HeatTreatMentDetails> CheckValidChubaan(List<string> lstChubaan)
        {
            List<HeatTreatMentDetails> heatTreatMentDetails = new List<HeatTreatMentDetails>();
            bool isValid = true;

            string enteredChuubans = string.Empty;
            string invalidChuubans = string.Empty;
            string notExistChuubans = string.Empty;

            List<string> ValidChuubans = new List<string>();
            string validChuubans = string.Empty;
            string completedChuubans = string.Empty;

            foreach (string _chuuban in lstChubaan)
            {

                if (enteredChuubans == string.Empty)
                {
                    //enteredChuubans = "'" + lstChubaan + "'";
                    enteredChuubans = "'" + _chuuban + "'";
                }
                else
                {
                    //enteredChuubans = enteredChuubans + "," + "'" + lstChubaan + "'";
                    enteredChuubans = enteredChuubans + "," + "'" + _chuuban + "'";
                }
            }

            if (enteredChuubans != string.Empty)
            {
                List<string> invalidList = new List<string>();
                List<string> notExistChuubanList = new List<string>();

                // Get chubaan details.
                //  heatTreatMentDetails = _heatTreatmentService.GetChuubanListForInvaidCheck(enteredChuubans);

                if (heatTreatMentDetails.Count != 0)
                {

                    foreach (var chuuban in heatTreatMentDetails)
                    {
                        chuuban.ISVALIDCHUUBAN = true;

                    }

                    var result2 = lstChubaan.Where(a => !heatTreatMentDetails.Any(b => b.CHUUBAN.Contains(a)));


                    foreach (var chuuban in result2)
                    {
                        HeatTreatMentDetails geo = new HeatTreatMentDetails
                        {

                            CHUUBAN = chuuban,
                            ISVALIDCHUUBAN = false
                        };

                        heatTreatMentDetails.Add(geo);

                    }



                    //if (heatTreatMentDetails.Any())
                    //{
                    //    ValidChuubans.Add(enteredChuubans);
                    //}
                    //else
                    //{
                    //    //if (lstChubaan.Length == 11)
                    //    //{
                    //    //    notExistChuubanList.Add(lstChubaan);
                    //    //}
                    //    //else
                    //    //{
                    //    invalidList.Add(enteredChuubans);
                    //    //}
                    //}




                    //}

                    invalidList = invalidList.Distinct().ToList();
                    notExistChuubanList = notExistChuubanList.Distinct().ToList();

                    foreach (string invalidKouban in invalidList)
                    {
                        if (invalidChuubans == string.Empty)
                        {
                            invalidChuubans = "" + invalidKouban + "";
                        }
                        else
                        {
                            invalidChuubans = invalidChuubans + "," + "" + invalidKouban + "";
                        }
                    }

                    foreach (string notExistChuuban in notExistChuubanList)
                    {
                        if (notExistChuubans == string.Empty)
                        {
                            notExistChuubans = "" + notExistChuuban + "";
                        }
                        else
                        {
                            notExistChuubans = notExistChuubans + "," + "" + notExistChuuban + ""; ;
                        }
                    }

                    // Check chuuban is completed.

                    foreach (string chubaan in ValidChuubans)
                    {
                        if (validChuubans == string.Empty)
                        {
                            validChuubans = "" + chubaan + "";
                        }
                        else
                        {
                            validChuubans = notExistChuubans + "," + "" + chubaan + ""; ;
                        }
                    }

                    if (validChuubans != string.Empty)
                    {

                        var completedList = _heatTreatmentService.GetCompletedChuubanList(validChuubans.ToLower());

                        ValidChuubanList = new List<string>();

                        foreach (string chubaan in ValidChuubans)
                        {

                            if (completedList.Any(chubaan.ToLower().Contains))
                            {
                                continue;
                            }
                            else
                            {
                                ValidChuubanList.Add(chubaan);
                            }

                        }

                        foreach (string completeChuuban in completedList)
                        {
                            if (completedChuubans == string.Empty)
                            {
                                completedChuubans = "" + completeChuuban + "";
                            }
                            else
                            {
                                completedChuubans = completedChuubans + "," + "" + completedChuubans + ""; ;
                            }
                        }
                    }
                }

                //StringBuilder valMsg = new StringBuilder();

                //if (enteredChuubans != string.Empty)
                //{
                //    if (invalidChuubans != string.Empty)
                //    {
                //        valMsg.Append(_localization.GetLocalizedHtmlString("valMsg_COM_FollowingChuubanNotFound"));
                //        valMsg.Append("<br>");
                //        invalidChuubans = invalidChuubans.Replace(",", "</br>");
                //        valMsg.Append(invalidChuubans.ToUpper());
                //    }


                //    if (notExistChuubans != string.Empty)
                //    {
                //        valMsg.Append("<br>");
                //        valMsg.Append(_localization.GetLocalizedHtmlString("valMsg_MultipleChuubansSearch_OrNoNotFound"));
                //        valMsg.Append("<br>");
                //        notExistChuubans = notExistChuubans.Replace(",", "</br>");
                //        valMsg.Append(notExistChuubans.ToUpper());
                //    }


                //    if (completedChuubans != string.Empty)
                //    {
                //        valMsg.Append("<br>");
                //        valMsg.Append(_localization.GetLocalizedHtmlString("valMsg_COM_MultipleChuubansSearchAlreadyCompleted"));
                //        valMsg.Append("<br>");
                //        completedChuubans = completedChuubans.Replace(",", "</br>");
                //        valMsg.Append(completedChuubans.ToUpper());
                //    }

                //    if (Convert.ToString(valMsg) != string.Empty)
                //    {
                //        isValid = false;
                //        return valMsg.ToString();
                //        //popUpMessage.Visible = true;
                //        //popUpMessage.ShowMessage(Constants.AlertType.WARNNING, valMsg.ToString());
                //        //if (ViewState["controlID"] == null)
                //        //{
                //        //    ViewState["controlID"] = btnSearchByChubaanPopup.ClientID;
                //        //}
                //    }

                //}
                //else
                //{
                //    return _localization.GetLocalizedHtmlString("valMsg_COM_ChubanGyoubanRequired");
                //}
                return heatTreatMentDetails;
            }
            else
            {
                return heatTreatMentDetails;
            }
        }

        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            //string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            ////return Ok(new { IsSuccess = true, message = dateTime });
            //DateTimeDetails result = new DateTimeDetails();

            //result.END_DATETIME_STRING = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            //DateTime currentDate = DateTime.Now;
            //currentDate = currentDate.AddMonths(-1);
            //result.START_DATETIME_STRING = currentDate.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            //return Ok(new { IsSuccess = true, message = result });

            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            return Ok(new { IsSuccess = true, message = sysDate });


        }

        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = null, int operatorCD = 0)
        {
            var result = _commonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }



        [HttpGet("CheckOperatorIncativeActive")]
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
        [HttpPost("CustomerList")]
        public IActionResult CustomerList(string? yobina = "")
        {
            var lst = _commonService.GetCustomerName(yobina);
            return Ok(new { IsSuccess = true, message = lst });
        }

        [HttpGet("GetMaxHTNumber")]
        public IActionResult GetMaxHTNumber()
        {
            var result = _heatTreatmentService.GetMaxHTNumber();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetFurnaceNo")]
        public IActionResult GetMachineAndFurnaceNo(string furNo)
        {
            var result = _heatTreatmentService.GetFurnaceNo(furNo);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpPost("IsMachineAlreadyStarted")]
        public IActionResult IsMachineAlreadyStarted(int machineCD)
        {
            var result = _heatTreatmentService.IsMachineAlreadyStarted(machineCD);
            return Ok(new { IsSuccess = true, message = result });

        }
        [HttpGet("EmployeeList")]
        public ActionResult EmployeeList(bool addEmpty = true, string? name = null)
        {
            var lst = _heatTreatmentService.GetFilteredEmployeeNameList(name, addEmpty);
            return Ok(new { IsSuccess = true, message = lst });
        }

        [HttpPost("GetRowCountForHeatTreatmentFactory")]
        public ActionResult GetRowCountForHeatTreatmentFactory(GridSearchFields gridSearchRequest)
        {
            int count = _heatTreatmentService.GetRowCountForHeatTreatmentFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetRowCountForHeatTreatmentDailyAnalysis")]
        public ActionResult GetRowCountForHeatTreatmentDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _heatTreatmentService.GetRowCountForHeatTreatmentDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpGet("GetConditionName")]
        public IActionResult GetConditionName()
        {
            var result = _commonService.GetConditionName();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetHeatTreatmentMachineList")]
        public IActionResult GetHeatTreatmentMachineList()
        {
            var result = _heatTreatmentService.GetHeatTreatmentMachineList();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetOperatorNamebyCD")]
        public IActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
    }



    public class GridMandatoryFields
    {
        public int HEAT_TREATMENT_ROW { get; set; }
        public string? PROCESS_PLAN_CD { get; set; }
        public string? COIL_BAR_TYPE { get; set; }
        public string? LOT_NO { get; set; }
        public string? LOCATION_IN_FURNACE1 { get; set; }
        public string? LOCATION_IN_FURNACE2 { get; set; }
        public int? HT_NEXT_PROCESS { get; set; }

    }

}
