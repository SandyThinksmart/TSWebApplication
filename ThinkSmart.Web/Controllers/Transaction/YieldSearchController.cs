using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Model;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Data;
using TSWebApplication.Application.IServices;
using static TSWebApplication.Domain.Utility.Enums;
using NPOI.POIFS.Crypt.Dsig;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class YieldSearchController : Controller
    {
        

        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        private readonly IAllCommonService _allCommonService;
        private readonly CsvSqlString _csvSqlString;
        public YieldSearchController(LocalizationServices localization, IViewRenderService viewRenderService, IAllCommonService commonService, CsvSqlString csvSqlString)
        {

            _localization = localization;
            _viewRenderService = viewRenderService;
            _allCommonService = commonService;
            _csvSqlString = csvSqlString;
        }

        public IActionResult YieldSearch()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

 
        public IActionResult MessageList()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

        /// <summary>
        /// CREATED BY :Sunil Gokanwar on 2025/12/28  
        /// </summary>
        /// used screens: Yield Search
        /// <returns>yield data</returns>
        public IActionResult GridSearch([FromBody] List<ProcessPlanForYieldSearchDetails> details)
        {
            string view = _viewRenderService.RenderToString("YieldSearch/YieldSearchGridView", details);
            return Json(new { IsSuccess = true, message = view });
        }

       
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        
                                        new TSSortField {
                                            FieldName = "PP_PROD_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                         new TSSortField {
                                            FieldName = "PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                             SortOrder=TSSortDirection.Ascending
                                        },
                                        
                                         new TSSortField {
                                            FieldName = "PP_PROD_DIM_D , PP_PROD_DIM_W ,PP_PROD_DIM_OTHER ",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimD_L"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                        // new TSSortField {
                                        //    FieldName = "PP_PROD_DIM_W",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                        //    IsDefaultSort=true,
                                        //    SortOrder=TSSortDirection.Ascending
                                        //},

                                        // new TSSortField {
                                        //    FieldName = "PP_PROD_DIM_OTHER",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_M"),
                                        //    IsDefaultSort=true,
                                        //    SortOrder=TSSortDirection.Ascending
                                        //},

                                            new TSSortField {
                                            FieldName =   "PP_PROD_LENGTH , PP_PROD_LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        //      new TSSortField {
                                        //    FieldName =   "PP_PROD_LENGTH_OTHER",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                        //    IsDefaultSort=true,
                                        //    SortOrder=TSSortDirection.Ascending
                                        //},

                                            new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                           IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                           new TSSortField {
                                            FieldName = "PP_CUSTOMER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                             new TSSortField {
                                            FieldName =   "MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Maker"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                        
                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        /// <summary>
        /// CREATED BY :Sunil Gokanwar on 2025/12/21  
        /// </summary>
        /// used screens: Yield Search
        /// <returns>CSV path</returns>
        public IActionResult ExportCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string SQL = _csvSqlString.GetYieldSearchStatusSQL(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_OrderYieldStatusCSV"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForCSV(gridSearchRequest.CULTURE_NAME);


            string cultureName = gridSearchRequest.CULTURE_NAME;

            try
            {
                string PLSqlName = String.Empty;
                string outPutTable = String.Empty;

                message = _allCommonService.ExportCSV(SQL, PLSqlName, outPutTable, fileName, dataTableWithHeader);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
            return Ok(new { IsSuccess = true, message = message });
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();



            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN, CASE WHEN LENGTH(TRIM(PP.GYOUBAN)) = 1 THEN '0' || PP.GYOUBAN END))",//"PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN.Trim()
                };
                searchFields.Add(searchField);

            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
                        
            if (gridSearchRequest.PROD_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
                };

                searchFields.Add(searchField);
            }
                        
            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE(TO_CHAR(PP.ACT_ALLOC_DATE))",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SPEC_NO1,

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SPEC_NO2
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MAX_ROW_COUNT > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ROWNUM <",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.MAX_ROW_COUNT.ToString(),
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private DataTable SetHeaderForCSV(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductSpecNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Condition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hikinuki") + " " + _localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hikinuki") + " " + _localization.GetLocalizedHtmlString("lbl_ProcessWiseYeild"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessStraightening") + " " + _localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessStraightening") + " " + _localization.GetLocalizedHtmlString("lbl_ProcessWiseYeild"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort") + " " + _localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort") + " " + _localization.GetLocalizedHtmlString("lbl_ProcessWiseYeild"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ECInspection") + " " + _localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ECInspection") + " " + _localization.GetLocalizedHtmlString("lbl_ProcessWiseYeild"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinalInspection") + " " + _localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinalInspection") + " " + _localization.GetLocalizedHtmlString("lbl_ProcessWiseYeild"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CompletionDate"));

            return dt;
        }
    }
}
