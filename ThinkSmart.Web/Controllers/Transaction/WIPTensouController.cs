using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Application.IServices;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TSWebApplication.Application.Services;
using OpenCvSharp;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class WIPTensouController : Controller
    {
        private readonly IWIPTensouService _IWIPTensouService;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;

        public WIPTensouController(IWIPTensouService IWIPTensouService, LocalizationServices localization, IViewRenderService viewRenderService)
        {
            _IWIPTensouService = IWIPTensouService;
            _localization = localization;
            _viewRenderService = viewRenderService;
        }

        public IActionResult GetTensouDetailsByChuubanGyouban([FromBody] GridSearchFields request)
        {
            var result = _IWIPTensouService.GetTensouDetailsByChuubanGyouban(request.NIFUDA_STRING, request.CULTURE_NAME);
            return Json(new { IsSuccess = true, message = result });
        }

        public IActionResult WIPTensouEntryEditDelete(int TransferCD = 0)
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();

        }
        public IActionResult WIPTensouList()
        {
            //  ViewBag.CultureName = UIHelper.GetCultureName();
            // GridSearchFields request = new GridSearchFields();
            //  var result = "messageTest";

            // return Ok(new { IsSuccess = true, message = result });
            return View();

        }
        public IActionResult GetUnitType()
        {
            var result = _IWIPTensouService.GetProductUnitKubun();
            return Ok(new { IsSuccess = true, message = result });
        }

        public IActionResult WIPTensouReportPrint()
        {
            return View();
        }

        public IActionResult TensouDataSearchInGrid([FromBody] List<WIPTensouDetails> details)
        {
            string view = _viewRenderService.RenderToString("WIPTensou/WIPTensouGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }


        //public IActionResult IsFillGridViewMandatoryFields([FromBody] List<WIPTensouDetails> lstGridData)//(List<WIPTensouDetails> lstGridData)
        //{
        //    string isRequiredString = _localization.GetLocalizedHtmlString("valMsg_COM_IsRequired");
        //    string message = string.Empty;
        //    string validationMsg = string.Empty;
        //    //
        //    List<string> termsList = new List<string>();

        //    var TabaNoOne = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_1 > 0).Select(x => x.WIP_TENSOU_TABA_NO_1).ToList();
        //    var t = lstGridData.Select(x => x.WIP_TENSOU_TABA_NO_1).ToList();
        //    var TABA_NO_1 = string.Join(",", TabaNoOne);


        //    var TabaNoTwo = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_2 > 0).Select(x => x.WIP_TENSOU_TABA_NO_2).ToList();
        //    var TABA_NO_2 = string.Join(",", TabaNoTwo);


        //    var TabaNoThree = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_3 > 0).Select(x => x.WIP_TENSOU_TABA_NO_3).ToList();
        //    var TABA_NO_3 = string.Join(",", TabaNoThree);


        //    var TabaNoFour = lstGridData.Where(x => x.WIP_TENSOU_TABA_NO_4 > 0).Select(x => x.WIP_TENSOU_TABA_NO_4).ToList();
        //    var TABA_NO_4 = string.Join(",", TabaNoFour);

        //    var noOfUnits = lstGridData.Where(x => x.WIP_TENSOU_NO_OF_UNITS > 0).Select(x => x.WIP_TENSOU_NO_OF_UNITS).ToList();
        //    var noOfUnit = string.Join(",", noOfUnits);

        //    //In Row NoOfUnits Required  
        //    if (noOfUnit == "")
        //    {
        //        validationMsg += Constants.StringValues.NEW_LINE;
        //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_InRowNoOfUnitsRequired").ToString();
        //    }
        //    else
        //    {
        //        if (lstGridData != null && lstGridData.Any())
        //        {
        //            //if (lstGridData[0].WIP_TENSOU_TABA_NO_1.HasValue && lstGridData[0].WIP_TENSOU_TABA_NO_1 > 0)
        //            // {
        //            foreach (var item in lstGridData)
        //            {
        //                if (item.WIP_TENSOU_NO_OF_UNITS != 0 && item.WIP_TENSOU_QTY != 0)
        //                {
        //                    if (item.WIP_TENSOU_NO_OF_UNITS <= 0)
        //                    {
        //                        validationMsg += Constants.StringValues.NEW_LINE;
        //                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_InRowNoOfUnitsRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
        //                    }
        //                    if (item.WIP_TENSOU_NEXT_PROCESS <= 0)
        //                    {
        //                        validationMsg += Constants.StringValues.NEW_LINE;
        //                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowNextProcessIsRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
        //                    }

        //                    if (item.WIP_TENSOU_QTY <= 0)
        //                    {
        //                        validationMsg += Constants.StringValues.NEW_LINE;
        //                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_COM_InRowWeightRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
        //                    }

        //                    //if (item.WIP_TENSOU_TABA_NO_1 <= 0)
        //                    //{

        //                    //    validationMsg += Constants.StringValues.NEW_LINE;
        //                    //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowTabaNORequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
        //                    //}

        //                    //If the No Of Unit is a BN , then at least 1 bundle numbers is required
        //                    if (item.WIP_TENSOU_UNIT_TYPE == 2)
        //                    {
        //                        if (item.WIP_TENSOU_TABA_NO_1 == null
        //                               && item.WIP_TENSOU_TABA_NO_2 == null
        //                               && item.WIP_TENSOU_TABA_NO_3 == null
        //                               && item.WIP_TENSOU_TABA_NO_4 == null)
        //                        {
        //                            validationMsg += Constants.StringValues.NEW_LINE;
        //                            validationMsg += _localization.GetLocalizedHtmlString("valMsg_UnitBNBundleNoRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
        //                        }
        //                    }

        //                    //At Least One Bundle Required
        //                    if (item.WIP_TENSOU_UNIT_TYPE == 2)

        //                    {
        //                        if (item.WIP_TENSOU_NO_OF_UNITS != 1)
        //                        {
        //                            validationMsg += Constants.StringValues.NEW_LINE;
        //                            validationMsg += _localization.GetLocalizedHtmlString("valMsg_AtLeastOneBundleRequired").ToString().Replace("NNN", (item.WIP_TENSOU_ROW + 1).ToString());
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //        // }
        //        //else
        //        //{
        //        //    validationMsg += Constants.StringValues.NEW_LINE;
        //        //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowTabaNORequired").ToString().Replace("NNN", (lstGridData[0].WIP_TENSOU_ROW + 1).ToString());
        //        //}


        //    //Same Taba Enter More Than One
        //    var str = TABA_NO_1 + "," + TABA_NO_2 + "," + TABA_NO_3 + "," + TABA_NO_4;
        //    termsList.Add(str);

        //    string[] AcceptanceTabaNo = str.Split(',');

        //    List<string> duplicateKeys = new List<string>();
        //    List<string> notDuplicateKeys = new List<string>();

        //    foreach (var text in AcceptanceTabaNo)
        //    {
        //        if (text != "")
        //        {
        //            if (notDuplicateKeys.Contains(text))
        //            {
        //                duplicateKeys.Add(text);
        //            }
        //            else
        //            {
        //                notDuplicateKeys.Add(text);
        //            }
        //        }
        //    }

        //    //  if (string.IsNullOrEmpty(duplicateKeys)) { }
        //    if (duplicateKeys.Any())
        //    {
        //        string result = string.Join(",", duplicateKeys); //Convert.ToString(duplicates);

        //        validationMsg += Constants.StringValues.NEW_LINE;
        //        validationMsg += _localization.GetLocalizedHtmlString("valMsg_SameTabaEnterMoreThanOne").ToString().Replace("NNN", (result).ToString());

        //    }

        //    //--------------------------------------------------
        //    if (validationMsg.Trim() == string.Empty)
        //    {
        //        return Ok(new { IsSuccess = true, message = message });
        //    }
        //    else
        //    {
        //        return Ok(new { IsSuccess = false, message = validationMsg });
        //    }
        //  }
        //    return Ok(new { IsSuccess = false, message = validationMsg });
        //}


        public IActionResult IsFillGridViewMandatoryFields([FromBody] List<WIPTensouDetails> lstGridData)
        {
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
                            
                        }

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
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "CUSTOMER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                            IsDefaultSort=false,
                                           // SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=false,
                                           //  SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"),
                                             IsDefaultSort=false,
                                           //  SortOrder=TSSortDirection.Ascending
                                        },

                                         new TSSortField {
                                            FieldName = "PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,
                                          //  SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName =   "PP_PROD_DIM_D_STRING" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,
                                            //SortOrder=TSSortDirection.Ascending
                                        },
                                            new TSSortField {
                                            FieldName = "PP_ACT_ALLOC_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ActAllocDate"),
                                            IsDefaultSort=false,
                                           // SortOrder=TSSortDirection.Ascending
                                        },
                                        //new TSSortField {
                                        //    FieldName = "PP_PROD_NAME_CUST",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                        //    IsDefaultSort=false,
                                        //   // SortOrder=TSSortDirection.Ascending
                                        //},
                                       
                                            new TSSortField {
                                            FieldName = "PP.CHUUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                            // SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName = "PP_PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                             IsDefaultSort=false,
                                            //SortOrder=TSSortDirection.Ascending
                                        }

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
    }
}
