using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;



namespace TSWebApplication.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class PostHtInspectionController : ControllerBase
    {

        private readonly IAllCommonService _allCommonService;
        private readonly IPostHTInspectionService _PostHTInspectionService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _commonService;

        private const int FixedRows = 20;
        int rowIndex = 0;
        List<string> ValidChuubanList = new List<string>();
        public PostHtInspectionController(IAllCommonService allCommonService, IPostHTInspectionService postHTInspectionService, LocalizationServices localization, IAllCommonService commonService)
        {
            _allCommonService = allCommonService;
            _PostHTInspectionService = postHTInspectionService;
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
        [HttpPost("GetSysDateTime")]
        public ActionResult GetSysDateTime()
        {
            string dateTime = DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
            return Ok(new { IsSuccess = true, message = dateTime });
        }
        [HttpPost("GetOperatorList")]
        public IActionResult GetOperatorList(string? name = "", int operatorCD=0)
        {
            var result = _allCommonService.GetOperatorName(name, operatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMachineList")]
        public IActionResult GetMachineList()
        {
            var result = _PostHTInspectionService.GetMachineName();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetResult")]
        public IActionResult GetResult()
        {

            var result = _PostHTInspectionService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }
        //[HttpGet("GetNextProcess")]
        //public IActionResult GetNextProcess()
        //{
        //    var result = _PostHTInspectionService.GetNextProcessList();
        //    return Ok(new { IsSuccess = true, message = result });
        //}
        [HttpGet("GetNextProcess")]
        public IActionResult GetNextProcess()
        {
            var result = _commonService.GetNextProcessList(Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessInternal_Quality_Inspection"))));
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetStackPosition")]
        public IActionResult GetStackPosition()
        {
            var result = _PostHTInspectionService.GetStackPositionList();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("IsFillGridViewMandatoryFields")]
        public IActionResult IsFillGridViewMandatoryFields(List<GridPostHTMandatoryFields> lstGridData)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;
            if (lstGridData != null && lstGridData.Any())
            {
                foreach (var item in lstGridData)
                {

                    if (item.POST_HT_ORDER_NO <= 0)
                    {
                        if (string.IsNullOrEmpty(item.POST_HT_D1) || string.IsNullOrEmpty(item.POST_HT_D2) || string.IsNullOrEmpty(item.POST_HT_D3))
                        {
                            validationMsg += Constants.StringValues.NEW_LINE;
                            validationMsg += _localization.GetLocalizedHtmlString("valMsg_RowDecrabRequired").ToString().Replace("NNN", (item.POST_HT_INSPECTION_ROW + 1).ToString());
                        }

                        if (item.POST_HT_Result <= 0)
                        {
                            validationMsg += Constants.StringValues.NEW_LINE;
                            validationMsg += _localization.GetLocalizedHtmlString("valMsg_RowResultRequired").ToString().Replace("NNN", (item.POST_HT_INSPECTION_ROW + 1).ToString());
                        }
                        //if (item.POST_HT_NEXT_PROCESS <= 0)
                        //{
                        //    validationMsg += Constants.StringValues.NEW_LINE;
                        //    validationMsg += _localization.GetLocalizedHtmlString("valMsg_InRowNextProcessIsRequired").ToString().Replace("NNN", (item.POST_HT_INSPECTION_ROW + 1).ToString());
                        //}
                    }
                    else
                    {
                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_MultipleChuubansSearch_OrNoRequired") + Constants.StringValues.NEW_LINE;
                        break;
                    }
                }

                if (validationMsg.Trim() == string.Empty)
                {
                    return Ok(new { IsSuccess = true, message = message });
                }
                else
                {
                    return Ok(new { IsSuccess = false, message = validationMsg });
                }
            }
            return Ok(new { IsSuccess = false, message = _localization.GetLocalizedHtmlString("valMsg_MultipleChuubansSearch_OrNoRequired") });
        }

        //[HttpPost("SearchBychuuban")]
        //public IActionResult SearchBychuuban(List<string> chuuban)
        //{           
        //    List<PostHTDetails> postHTDetails = new List<PostHTDetails>();   
            
        //    postHTDetails = CheckValidChubaan(chuuban);            
        //    return Ok(new { IsSuccess = true, message = postHTDetails });            
        //}
        [HttpPost("SearchBychuuban")]
        public IActionResult SearchBychuuban(List<string> chuuban)
        {
            /// return 1 for Success
            /// return 2 for OrNo not found
            /// return 3  for OrNo not started from Handover
            /// return 4 for complete date is null record not found for OrNo
            /// 
            CommonFields resultDetails = new CommonFields();
            List<PostHTDetails> postHTDetails = new List<PostHTDetails>();

            foreach (var item in chuuban)
            {

                // var result = _commonService.MappChuubanAndGyoubanByBarcode(item);
                bool isCharExist = false;
                string gyouban = string.Empty;
                string chuubanVal = string.Empty;
                // var result = _commonService.MappChuubanAndGyoubanByBarcode(item);

               // string gyouban = item.Substring(item.Length - 2);
               // string chuubanVal = item.Remove(item.Length - 2);



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
                    postHTDetails.Add(new PostHTDetails { CHUUBAN = item, CHUUBAN_CODE = 2 });
                }
                else
                {
                     resultDetails = _commonService.ValidateChubban(chuubanVal, Convert.ToInt16(gyouban));
                }



               

                if (resultDetails.RESPONCE == 1)
                {
                    var processPlanDetails = _PostHTInspectionService.GetChuubanListForInvaidCheck(item);
                    if (processPlanDetails != null)
                    {
                        postHTDetails.Add(new PostHTDetails{
                            CHUUBAN = item,
                            CUSTOMER_NAME = processPlanDetails.CUSTOMER_NAME,
                            PROD_NAME_CUST = processPlanDetails.PROD_NAME_CUST,
                            CHARGE = processPlanDetails.CHARGE,
                            PP_MATL_DIM_D_STRING = processPlanDetails.PP_MATL_DIM_D_STRING,
                            PP_PROD_DIM_D_STRING = processPlanDetails.PP_PROD_DIM_D_STRING,
                           // MATL_DIM_D = Convert.ToInt32(processPlanDetails.PP_MATL_DIM_D_STRING),
                           // PROD_DIM_D = Convert.ToInt32(processPlanDetails.PP_PROD_DIM_D_STRING),
                            PROCESS_PLAN_CD = processPlanDetails.PROCESS_PLAN_CD,
                           
                            CHUUBAN_CODE = 1
                        });
                    }

                }
                if (resultDetails.RESPONCE == 2)
                {
                    postHTDetails.Add(new PostHTDetails { CHUUBAN = item, CHUUBAN_CODE = 2 });
                }
                if (resultDetails.RESPONCE == 3)
                {
                    postHTDetails.Add(new PostHTDetails { CHUUBAN = item, CHUUBAN_CODE = 3 });
                }
                if (resultDetails.RESPONCE == 4)
                {
                    postHTDetails.Add(new PostHTDetails { CHUUBAN = item, CHUUBAN_CODE = 4 });
                }

            }

            return Ok(new { IsSuccess = true, message = postHTDetails });

        }

        [HttpPost("ValidateChubban")]
        public ActionResult ValidateChubban([FromBody] CommonFields request)
        {
            var result = _commonService.ValidateChubban(request.CHUUBAN, request.GYOUBAN);
            return Ok(new { IsSuccess = true, message = result });
        }
        //private List<PostHTDetails> CheckValidChubaan(List<string> lstChubaan)
        //{
        //    List<PostHTDetails> postHT = new List<PostHTDetails>();
        //    bool isValid = true;

        //    string enteredChuubans = string.Empty;
        //    string invalidChuubans = string.Empty;
        //    string notExistChuubans = string.Empty;

        //    List<string> ValidChuubans = new List<string>();
        //    string validChuubans = string.Empty;
        //    string completedChuubans = string.Empty;

        //    foreach (string _chuuban in lstChubaan)
        //    {

        //        if (enteredChuubans == string.Empty)
        //        {
        //            enteredChuubans = "'" + _chuuban + "'";
        //        }
        //        else
        //        {
        //            enteredChuubans = enteredChuubans + "," + "'" + _chuuban + "'";
        //        }
        //    }

        //    if (enteredChuubans != string.Empty)
        //    {
        //        List<string> invalidList = new List<string>();
        //        List<string> notExistChuubanList = new List<string>();

        //        // Get chubaan details.
        //        postHT = _PostHTInspectionService.GetChuubanListForInvaidCheck(enteredChuubans);

        //        if (postHT.Count != 0)
        //        {

        //            foreach (var chuuban in postHT)
        //            {
        //                chuuban.ISVALIDCHUUBAN = true;

        //            }

        //            var result2 = lstChubaan.Where(a => !postHT.Any(b => b.CHUUBAN.Contains(a)));


        //            foreach (var chuuban in result2)
        //            {
        //                PostHTDetails geo = new PostHTDetails
        //                {

        //                    CHUUBAN = chuuban,
        //                    ISVALIDCHUUBAN = false
        //                };

        //                postHT.Add(geo);

        //            }

        //            invalidList = invalidList.Distinct().ToList();
        //            notExistChuubanList = notExistChuubanList.Distinct().ToList();

        //            foreach (string invalidKouban in invalidList)
        //            {
        //                if (invalidChuubans == string.Empty)
        //                {
        //                    invalidChuubans = "" + invalidKouban + "";
        //                }
        //                else
        //                {
        //                    invalidChuubans = invalidChuubans + "," + "" + invalidKouban + "";
        //                }
        //            }

        //            foreach (string notExistChuuban in notExistChuubanList)
        //            {
        //                if (notExistChuubans == string.Empty)
        //                {
        //                    notExistChuubans = "" + notExistChuuban + "";
        //                }
        //                else
        //                {
        //                    notExistChuubans = notExistChuubans + "," + "" + notExistChuuban + ""; ;
        //                }
        //            }

        //            // Check chuuban is completed.

        //            foreach (string chubaan in ValidChuubans)
        //            {
        //                if (validChuubans == string.Empty)
        //                {
        //                    validChuubans = "" + chubaan + "";
        //                }
        //                else
        //                {
        //                    validChuubans = notExistChuubans + "," + "" + chubaan + ""; ;
        //                }
        //            }

        //            if (validChuubans != string.Empty)
        //            {

        //                var completedList = _PostHTInspectionService.GetCompletedChuubanList(validChuubans.ToLower());

        //                ValidChuubanList = new List<string>();

        //                foreach (string chubaan in ValidChuubans)
        //                {

        //                    if (completedList.Any(chubaan.ToLower().Contains))
        //                    {
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        ValidChuubanList.Add(chubaan);
        //                    }

        //                }

        //                foreach (string completeChuuban in completedList)
        //                {
        //                    if (completedChuubans == string.Empty)
        //                    {
        //                        completedChuubans = "" + completeChuuban + "";
        //                    }
        //                    else
        //                    {
        //                        completedChuubans = completedChuubans + "," + "" + completedChuubans + ""; ;
        //                    }
        //                }
        //            }
        //        }
        //        return postHT;
        //    }
        //    else
        //    {
        //        return postHT;
        //    }
        //}

        [HttpPost("SearchByFurnaceNo")]
        public IActionResult SearchByFurnaceNo(string furnanceNo)
        {
            List<PostHTDetails> furnaceDetails = new List<PostHTDetails>();
            string message = string.Empty;
            string validationMsg = string.Empty;
            bool isSucess = false;


            int isExits = _PostHTInspectionService.checkvalidFurnaceNo(furnanceNo);
            if (isExits > 0)
            {
                furnaceDetails = _PostHTInspectionService.getFurnaceDetails(furnanceNo);
                isSucess = true;
            }


            return Ok(new { IsSuccess = isSucess, message = furnaceDetails });
        }




        [HttpPost("FillGrid")]
        public IActionResult FillGrid([FromBody] List<PostHTDetails> details)
        {
            string message = string.Empty;

            // string[] No = details.Split(',');
            // var postHTDetails = new List<PostHTDetails>();
            //List<PostHTDetails> postHTDetails = new List<PostHTDetails>();
            List<PostHTDetails> BindPostData = new List<PostHTDetails>();
            PostHTDetails postHTDetails = new PostHTDetails();
            // for (int rowCount = 1; rowCount <= FixedRows; rowCount++)
            //  {
            foreach (var item in details)
            {
                postHTDetails.CHUUBAN = item.CHUUBAN;
                postHTDetails.CUSTOMER_NAME = item.CUSTOMER_NAME;
                postHTDetails.PROD_NAME = item.PROD_NAME;
                postHTDetails.CHARGE = item.CHARGE;
                postHTDetails.MATL_DIM_D = item.MATL_DIM_D;
                postHTDetails.PROD_DIM_D = item.PROD_DIM_D;
                postHTDetails.LOCATION_IN_FURNACE1 = item.LOCATION_IN_FURNACE1;
                postHTDetails.LOCATION_IN_FURNACE2 = item.LOCATION_IN_FURNACE2;
                postHTDetails.STACK_POSITION = item.STACK_POSITION;
                postHTDetails.ACT_DM_F = postHTDetails.ACT_DM_F;
                postHTDetails.ACT_DM_T_MIN = postHTDetails.ACT_DM_T_MIN;
                postHTDetails.ACT_DM_T_MAX = postHTDetails.ACT_DM_T_MAX;
                postHTDetails.ACT_DM_T_AVG = postHTDetails.ACT_DM_T_AVG;
                postHTDetails.ACT_STRUCTURE = postHTDetails.ACT_STRUCTURE;
                postHTDetails.HRD_CENTER = postHTDetails.HRD_CENTER;
                postHTDetails.HRD_D4_1 = Convert.ToSingle(postHTDetails.ACT_STRUCTURE);
                postHTDetails.HRD_D4_2 = Convert.ToSingle(postHTDetails.ACT_STRUCTURE);
                postHTDetails.HRD_D4_3 = Convert.ToSingle(postHTDetails.ACT_STRUCTURE);
                postHTDetails.HRD_D4_AVG = postHTDetails.HRD_D4_AVG;
                postHTDetails.INTERNAL_QUALITY_DECISION = postHTDetails.INTERNAL_QUALITY_DECISION;
                postHTDetails.IQ_NEXT_PROCESS = postHTDetails.IQ_NEXT_PROCESS;
                postHTDetails.PROCESS_PLAN_CD = item.PROCESS_PLAN_CD;
                // postHTDetails.USER_ID = Convert.ToInt32(Session["userID"]);
                //postHTDetails.INPUT_LOCATION_CD = locationCd;
                //postHTDetails.SERIAL_NUMBER = rowIndex + 1;

                // postHTDetails.DMF_DMT = Convert.ToDecimal(postHTDetails.DECARB_VAL_1).ToString("0.###") + Constants.StringValues.SPACE + _helper.GetGlobalResourceString("lbl_LessThan");
                //  postHTDetails.DMF_DMT += Constants.StringValues.SPACE + Convert.ToDecimal(postHTDetails.DECARB_VAL_3).ToString("0.###") + Constants.StringValues.SPACE + _helper.GetGlobalResourceString("lbl_LessThan");

                if (postHTDetails != null)
                {
                    BindPostData.Add(postHTDetails);
                }


            }
            // }
            return Ok(new { IsSuccess = true, message = BindPostData });
        }

        [HttpGet("FillEmptyGrid")]
        public IActionResult FillEmptyGrid()
        {
            var result = new List<PostHTDetails>();
            for (int rowCount = 1; rowCount <= FixedRows; rowCount++)
            {
                result.Add(new PostHTDetails());
            }
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpGet("GetMaxGruopNo")]
        public IActionResult GetMaxGruopNo()
        {
            var result = _PostHTInspectionService.GetMaxGruopNo();
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpGet("GetOperatorNamebyCD")]
        public ActionResult GetOperatorNamebyCD(long OperatorCD)
        {
            //var result = _PostHTInspectionService.GetEmployeeNameByID(OperatorCD);
            var result = _commonService.GetEmployeeNameByID(OperatorCD);
            return Ok(new { IsSuccess = true, message = result });
        }


        [HttpPost("savePostHTInspectionDetails")]
        public IActionResult savePostHTInspectionDetails([FromBody] List<PostHTDetails> requestData)
        {
            var result = _PostHTInspectionService.savePostHTInspectionDetails(requestData);
            return Ok(new { IsSuccess = true, message = result });
        }

        [HttpPost("updatePostHTInspectionDetails")]
        public IActionResult UpdatePostHTInspectionDetails([FromBody] List<PostHTDetails> requestData)
        {

            var result = _PostHTInspectionService.updatePostHTInspectionDetails(requestData);
            return Ok(new { IsSuccess = true, message = "" });
        }


        [HttpPost("PostHTInspectionDetailsDelete")]
        public IActionResult PostHTInspectionDetailsDelete(long groupNo)
        {

            _PostHTInspectionService.PostHTDetailsDelete(groupNo);
            return Ok(new { IsSuccess = true, message = "" });
        }

        [HttpPost("PostHTInspectionCheckMilkSheetData")]
        public IActionResult PostHTInspectionCheckMilkSheetData(long groupNo)
        {

            int count = _PostHTInspectionService.PostHTInspectionCheckMilkSheetData(groupNo);
            return Ok(new { IsSuccess = true, message = count });
        }


        [HttpPost("getPostHTDetailsByInternalQCD")]
        public ActionResult getPostHTDetailsByInternalQCD(long groupNo)
        {
            List<PostHTDetails> result = new List<PostHTDetails>();
            result = _PostHTInspectionService.getPostHTDetailsByInternalQCD(groupNo);

            return Ok(new { IsSuccess = true, message = result });

        }

        //sort popup
        [HttpGet("InitializeSortPopup")]
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                         new TSSortField {
                                            FieldName = "INT_QLTY_TEST_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending

                                        },
                                        //  new TSSortField {
                                        //    FieldName = "MACHINE_NAME",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        //},

                                            new TSSortField {
                                            FieldName = "FURNACE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),

                                        },
                                              new TSSortField {
                                            FieldName = "PROC_COND",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcCondition"),

                                        },
                                            new TSSortField {
                                            FieldName = "PATTERN_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_PatternNo"),

                                        }

                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }


        [HttpPost("GridSearch")]
        public IActionResult GetPostInspList([FromBody] GridSearchFields request)
        {

            List<PostHTDetails> post = new List<PostHTDetails>();
            List<SearchField> searchFields = new List<SearchField>();
            var SearchFields = ComposeSearchCriteria(request);

            InitializeSortPopup();
            post = _PostHTInspectionService.GetPostInspList(SearchFields, request);


            return Ok(new { IsSuccess = true, message = post });

        }


        [HttpGet("deleteRowOnGrid")]
        public IActionResult deleteRowOnGrid()
        {
            var result = new List<PostHTDetails>();
            for (int rowCount = 1; rowCount <= FixedRows; rowCount++)
            {
                //result.Add(new PostHTDetails());
            }
            return Ok(new { IsSuccess = true, message = result });
        }


        private List<SearchField> ComposeSearchCriteria(GridSearchFields request)
        {
            var searchFields = new List<SearchField>();

            if (request.MACHINE_CD > 0)
            {
                var machine = request.MACHINE_CD;
                var searchField = new SearchField
                {
                    FieldName = "INT.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = machine.ToString()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(request.FURNACE_NO))
            {
                var furnanceNum = request.FURNACE_NO;
                var searchField = new SearchField
                {
                    FieldName = "FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = furnanceNum.ToString()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(request.PROD_CONDITION_STRING))
            {
                var productCond = request.PROD_CONDITION_STRING;
                var searchField = new SearchField
                {
                    FieldName = "HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = productCond.ToString()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(request.PATTERN_NO))
            {
                var paternNo = request.PATTERN_NO;
                var searchField = new SearchField
                {
                    FieldName = "PATTERN_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(paternNo)
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(request.INSPECTION_DATE1))
            {
                var startD = request.INSPECTION_DATE1;
                var EndD = request.INSPECTION_DATE2;
                var searchField = new SearchField
                {

                    FieldName = "INT_QLTY_TEST_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = startD,
                    SearchText2 = EndD
                };

                searchFields.Add(searchField);
            }

            return searchFields;

        }
        private GridSearchFields ValidateSearchFields(GridSearchFields request)
        {
            string message = string.Empty;
            string validationMsg = string.Empty;

            if (request.START_DATETIME == string.Empty && request.START_DATETIME != string.Empty)
            {
                validationMsg += Constants.StringValues.NEW_LINE;
                validationMsg += _localization.GetLocalizedHtmlString("valMsg_InspStartDate_Required").ToString();
            }

            if (request.START_DATETIME == null)
            {
                if (!UIHelper.IsValidDate(request.START_DATETIME))
                {
                    validationMsg += Constants.StringValues.NEW_LINE;
                    validationMsg += _localization.GetLocalizedHtmlString("valMsg_InspStartDate_Invalid").ToString();
                }

                if (request.END_DATETIME == null)
                {
                    if (!UIHelper.IsValidDate(request.END_DATETIME))
                    {
                        validationMsg += Constants.StringValues.NEW_LINE;
                        validationMsg += _localization.GetLocalizedHtmlString("valMsg_InspEndDate_Invalid").ToString();
                    }
                }
            }
            if (validationMsg.Trim() == string.Empty)
            {
                return null;
                //return Ok(new { IsSuccess = true, message = message });
            }
            else
            {
                // return Ok(new { IsSuccess = false, message = validationMsg });
                return null;
            }



        }
        public class GridPostHTMandatoryFields
        {
            public int? POST_HT_INSPECTION_ROW { get; set; }
            public int? POST_HT_PROCESS_PLAN_CD { get; set; }
            public int? POST_HT_ORDER_NO { get; set; }
            public string? POST_HT_D1 { get; set; }
            public string? POST_HT_D2 { get; set; }
            public string? POST_HT_D3 { get; set; }
            public int? POST_HT_Result { get; set; }
            public int? POST_HT_NEXT_PROCESS { get; set; }

        }
        public class SearchFieldOnList
        {
            public string? txtMachine { get; set; }
            public int? txtFurnaceNum { get; set; }
            public string? txtProductCond { get; set; }
            public int? txtPatternNo { get; set; }
            public string? txtInspDateStart { get; set; }
            public string? txtInspDateEnd { get; set; }



        }
        [HttpPost("GetQATestHardnessOnFactoryDailyWorkStatus")]
        public IActionResult GetQATestHardnessOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _PostHTInspectionService.GetQATestHardnessOnFactoryDailyWorkStatus(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpPost("GetQATestHardnessDetailsOnDailyProcessAnalysis")]
        public IActionResult GetQATestHardnessDetailsOnDailyProcessAnalysis(GridSearchFields gridSearchRequest)
        {
            bool isSuccess = false;
            var result = _PostHTInspectionService.GetQATestHardnessDetailsOnDailyProcessAnalysis(gridSearchRequest);
            if (result.Count > 0)
            {
                isSuccess = true;
            }
            return Ok(new { IsSuccess = isSuccess, message = result });
        }

        [HttpGet("GetProcConditionKubun")]
        public IActionResult GetProcConditionKubun()
        {
            var result = _PostHTInspectionService.GetProcConditionKubun();
            return Ok(new { IsSuccess = true, message = result });
        }
        [HttpPost("GetFurnaceNo")]
        public IActionResult GetFurnaceNo(string furNo, bool isNull = true)
        {
            var result = _PostHTInspectionService.GetFurnaceNo(furNo, isNull);
            return Ok(new { IsSuccess = true, message = result });

        }

        [HttpPost("GetRowCountForQATestHardnessFactory")]
        public ActionResult GetRowCountForQATestHardnessFactory(GridSearchFields gridSearchRequest)
        {
            int count = _PostHTInspectionService.GetRowCountForQATestHardnessFactory(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
        [HttpPost("GetRowCountForPostHTHardnessDailyAnalysis")]
        public ActionResult GetRowCountForPostHTHardnessDailyAnalysis(GridSearchFields gridSearchRequest)
        {
            int count = _PostHTInspectionService.GetRowCountForPostHTHardnessDailyAnalysis(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = count });
        }
    }
}

