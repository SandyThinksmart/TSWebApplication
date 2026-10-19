using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
    public class WIPTensouController : ControllerBase
    {
        private readonly IWIPTensouService _IWIPTensouService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;
        public WIPTensouController(IWIPTensouService WIPTensouService, LocalizationServices localization, IAllCommonService commonService)
        {

            _IWIPTensouService = WIPTensouService;
            _localization = localization;
            _commonService = commonService;
            // _hikinukibartobarService = hikinukibartobarService;
        }

        [HttpPost("GetTensouDetailsByChuubanGyouban")]
        public ActionResult GetTensouDetailsByChuubanGyouban([FromBody] GridSearchFields request)
        {
            var result = _IWIPTensouService.GetTensouDetailsByChuubanGyouban(request.NIFUDA_STRING, request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });
        }

        //Mapping Nifuda By Barcode
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

        //Mapping Chuuban By Barcode
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


        [HttpPost("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding"))));
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetProductUnitKubun")]
        public IActionResult GetProductUnitKubun()
        {

            var result = _IWIPTensouService.GetProductUnitKubun();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetLocation")]
        public ActionResult GetLocation(int USER_ID)
        {
            var result = _IWIPTensouService.GetTensouLocation(USER_ID);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetForwordingLocation")]
        public ActionResult GetForwordingLocation()
        {
            var result = _IWIPTensouService.GetForwordingLocation();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("GetVehicleName")]
        public ActionResult GetVehicleName()
        {
            var result = _IWIPTensouService.GetVehicleName();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetConditionName")]
        public IActionResult GetConditionName()
        {
            var result = _commonService.GetConditionName();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("SaveWIPTensouDetails")]
        public ActionResult SaveWIPTensouDetails(List<WIPTensouDetails> request)
        {
            var result = _IWIPTensouService.SaveWIPTensouDetails(request);
            return Ok(new { IsSuccess = true, message = result });
            //return null;
        }

        //Search data
        [HttpPost("TensouDataSearchInGrid")]
        public IActionResult TensouDataSearchInGrid([FromBody] GridSearchFields request)
        {
            var result = _IWIPTensouService.GetTensouDataSearchList(request);
            return Ok(new { IsSuccess = true, message = result });

        }

        private IActionResult Json(object value)
        {
            return new JsonResult(value);
        }

        private string ValidateSearchFields(GridSearchFields request)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string validationMsg = string.Empty;
            if (request.PP_DIM_TOL_MAX != "")
            {
                if (request.PP_DIM_TOL_MIN == "")
                {
                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_DimDStartRequired");
                }
            }
            if (request.TRANSFER_DATE_END != "")
            {
                if (request.TRANSFER_DATE_START == "")
                {

                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_TransferDateStartRequired");

                }
            }

            return validationMsg;
        }

        [HttpPost("IsFillGridViewMandatoryFields")]
        public IActionResult IsFillGridViewMandatoryFields([FromBody] List<WIPTensouDetails> lstGridData)//(List<WIPTensouDetails> lstGridData)
        {
            string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
            string message = string.Empty;
            string validationMsg = string.Empty;
            //
            List<string> termsList = new List<string>();

            var TabaNoOne = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_1 > 0).Select(x => x.WIP_TENSOU_TABA_NO_1).ToList();
            var t = lstGridData.Select(x => x.WIP_TENSOU_TABA_NO_1).ToList();
            var TABA_NO_1 = string.Join(",", TabaNoOne);


            var TabaNoTwo = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_2 > 0).Select(x => x.WIP_TENSOU_TABA_NO_2).ToList();
            var TABA_NO_2 = string.Join(",", TabaNoTwo);


            var TabaNoThree = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_3 > 0).Select(x => x.WIP_TENSOU_TABA_NO_3).ToList();
            var TABA_NO_3 = string.Join(",", TabaNoThree);


            var TabaNoFour = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_4 > 0).Select(x => x.WIP_TENSOU_TABA_NO_4).ToList();
            var TABA_NO_4 = string.Join(",", TabaNoFour);

            var noOfUnits = lstGridData.Where(x => x.WIP_TENSOU_NO_OF_UNITS > 0).Select(x => x.WIP_TENSOU_NO_OF_UNITS).ToList();
            var noOfUnit = string.Join(",", noOfUnits);

            //In Row NoOfUnits Required  
            if (noOfUnit == "")
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_InRowNoOfUnitsRequired").ToString();
            }
            else
            {
                if (lstGridData != null && lstGridData.Any())
                {

                    //if (lstGridData[0].WIP_TENSOU_TABA_NO_1.HasValue && lstGridData[0].WIP_TENSOU_TABA_NO_1 > 0)
                    // {
                    foreach (var item in lstGridData)
                    {
                        if (item.WIP_TENSOU_NO_OF_UNITS >= 0)
                        {
                            if (item.WIP_TENSOU_NO_OF_UNITS <= 0)
                            {
                                validationMsg += Constants.StringValues.NEW_LINE;
                                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_InRowNoOfUnitsRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
                            }
                            if (item.WIP_TENSOU_NEXT_PROCESS <= 0)
                            {
                                validationMsg += Constants.StringValues.NEW_LINE;
                                validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowNextProcessIsRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
                            }

                            if (item.WIP_TENSOU_QTY <= 0)
                            {
                                validationMsg += Constants.StringValues.NEW_LINE;
                                validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_InRowWeightRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
                            }

                            //if (item.WIP_TENSOU_TABA_NO_1 <= 0)
                            //{

                            //    validationMsg += Constants.StringValues.NEW_LINE;
                            //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowTabaNORequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
                            //}

                            //If the No Of Unit is a BN , then at least 1 bundle numbers is required
                            if (item.WIP_TENSOU_UNIT_TYPE == 2)
                            {
                                if (item.WIP_TENSOU_TABA_NO_1 == null
                                       && item.WIP_TENSOU_TABA_NO_2 == null
                                       && item.WIP_TENSOU_TABA_NO_3 == null
                                       && item.WIP_TENSOU_TABA_NO_4 == null)
                                {
                                    validationMsg += Constants.StringValues.NEW_LINE;
                                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_UnitBNBundleNoRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
                                }
                            }

                            //At Least One Bundle Required
                            if (item.WIP_TENSOU_UNIT_TYPE == 2)

                            {
                                if (item.WIP_TENSOU_NO_OF_UNITS != 1)
                                {
                                    validationMsg += Constants.StringValues.NEW_LINE;
                                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_AtLeastOneBundleRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
                                }
                            }
                        }
                        //else
                        //{

                        //}
                    }

                    // }
                    //else
                    //{
                    //    validationMsg += Constants.StringValues.NEW_LINE;
                    //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowTabaNORequired").ToString().Replace("NNN", (lstGridData[0].WIP_TENSOU_ROW + 1).ToString());
                    //}
                }

                //Same Taba Enter More Than One
                var str = TABA_NO_1 + "," + TABA_NO_2 + "," + TABA_NO_3 + "," + TABA_NO_4;
                termsList.Add(str);

                string[] AcceptanceTabaNo = str.Split(',');

                List<string> duplicateKeys = new List<string>();
                List<string> notDuplicateKeys = new List<string>();

                foreach (var text in AcceptanceTabaNo)
                {
                    if (text != "")
                    {
                        if (notDuplicateKeys.Contains(text))
                        {
                            duplicateKeys.Add(text);
                        }
                        else
                        {
                            notDuplicateKeys.Add(text);
                        }
                    }
                }

                //  if (string.IsNullOrEmpty(duplicateKeys)) { }
                if (duplicateKeys.Any())
                {
                    string result = string.Join(",", duplicateKeys); //Convert.ToString(duplicates);

                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_SameTabaEnterMoreThanOne").ToString().Replace("NNN", (result).ToString());

                }

                //--------------------------------------------------
                if (validationMsg.Trim() == string.Empty)
                {
                    return Ok(new { IsSuccess = true, message = message });
                }
                else
                {
                    return Ok(new { IsSuccess = false, message = validationMsg });
                }

            }
            //  return Ok(new { IsSuccess = false, message = _localization.GetLocalizedHtmlString("valMsg_COM_SelectAtLeastOneRecord") });
            return Ok(new { IsSuccess = false, message = validationMsg });
        }

        [HttpPost("getWIPTransferDetailsByTrasferCD")]
        public ActionResult getWIPTransferDetailsByTrasferCD([FromBody] WIPTensouDetails request)
        {
            var result = _IWIPTensouService.GetWIPTransferDetailsByTrasferCD(Convert.ToInt32(request.WIP_TRANSFER_CD), request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });

        }

        [HttpPost("IsAcceptanceCompleted")]
        public ActionResult IsAcceptanceCompleted([FromBody] WIPTensouDetails request)
        {
            var result = _IWIPTensouService.IsAcceptanceCompleted(Convert.ToInt32(request.WIP_TRANSFER_CD), request.CULTURE_NAME);
            return Ok(new { IsSuccess = true, message = result });

        }

        //Added Date Range
        [HttpPost("IsValidDateRange")]
        public ActionResult IsValidDateRange(DateTimeDetails request)
        {
            bool returnval = false;

            DateTime minDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD));
            DateTime maxDate = Convert.ToDateTime(DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD));
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

        [HttpPost("GetSysDateTime")]
        public IActionResult GetSysDateTime()
        {
            DateTime dateTime = _commonService.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));
            string nextDayDate = dateTime.AddDays(1).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            return Ok(new { IsSuccess = true,message = new{ SysDate = sysDate, NextDayDate = nextDayDate } });
        }

        [HttpGet("InitializeSortPopup")]
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                        //new TSSortField {
                                        //    FieldName = "DW_START_DATE",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                        //    IsDefaultSort=false,
                                        //    SortOrder=TSSortDirection.Descending
                                        //},
                                        // new TSSortField {
                                        //    FieldName = "dw_machine_name",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        //     IsDefaultSort=false,
                                        //     SortOrder=TSSortDirection.Ascending
                                        //},

                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                            new TSSortField {
                                            FieldName =   "TABA_NO_1" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "PP_CUSTOMER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                           new TSSortField {
                                            FieldName = "PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdShape"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "PP_PROD_DIM_D_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDim"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                            new TSSortField {
                                            FieldName = "PP_PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductLength"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },


                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        //Update Data
        [HttpPost("WIPTensouDetailsUpdate")]
        public IActionResult WIPTensouDetailsUpdate(List<WIPTensouDetails> request)
        {
            var result = _IWIPTensouService.WIPTensouDetailsUpdate(request);
            return Ok(new { IsSuccess = true, message = result });
        }

        //Delete Data
        [HttpPost("WIPTensouDetailsDelete")]
        public IActionResult WIPTensouDetailsDelete(List<WIPTensouDetails> request)
        {
           int result= _IWIPTensouService.WIPTensouDetailsDelete(request);
           return Json(new { IsSuccess = true, message = result });
        }


        // Report
        [HttpPost("GetWIPRequestReportData")]
        public IActionResult GetWIPRequestReportData(string transferDate, long userId)
        {
            bool isSuccess = false;
            int result = _IWIPTensouService.GetWIPRequestReportData(transferDate, userId);

            if (result > 0)
            {
                isSuccess = true;
            }

            return Ok(new { IsSuccess = isSuccess, Result = result });
        }

        [HttpPost("GetWIPAcceptancetReportData")]
        public IActionResult GetWIPAcceptancetReportData([FromBody] WIPTensouDetails request)
        {
            bool isSuccess = false;
            int result = _IWIPTensouService.GetWIPAcceptancetReportData(request);

            if (result > 0)
            {
                isSuccess = true;
            }

            return Ok(new { IsSuccess = isSuccess, Result = result });
        }

        [HttpPost("GetRowCountForTransferReport")]
        public ActionResult GetRowCountForTransferReport(GridSearchFields gridSearchRequest)
        {
            int count = _IWIPTensouService.GetRowCountForTransferReport(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }

        [HttpPost("GetTransferDetails")]
        public IActionResult GetTransferDetails(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _IWIPTensouService.GetTransferDetails(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }
    }
}
