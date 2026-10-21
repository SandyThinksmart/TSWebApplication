using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class NextPrcessWaitingStatusSearchController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly INextProcessWaitingStatusSearchService _INextProcessWaitingStatusSearchService;
        private readonly CsvSqlString _csvSqlString;
        private readonly IAllCommonService _allCommonService;


        public NextPrcessWaitingStatusSearchController(IViewRenderService viewRenderService, LocalizationServices localizationServices,INextProcessWaitingStatusSearchService nextProcessWaitingStatusSearchService, CsvSqlString csvSqlString, IAllCommonService allCommonService)
        {


            _viewRenderService = viewRenderService;
            _localization = localizationServices;
            _INextProcessWaitingStatusSearchService = nextProcessWaitingStatusSearchService;
            _csvSqlString = csvSqlString;
            _allCommonService = allCommonService;
        }
        public IActionResult NextPrcessWaitingStatusSearch()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult NextProcessWaitingStatusSearchGrid([FromBody] List<WipStatusDetails> details)
        {
            foreach (WipStatusDetails record in details)
            {
                record.PRE_PROSESS_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.PRE_PROSESS_DECISION));
                record.UNIT_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT));
                record.NEXT_PROCESS_STRING = UIUtils.GetNextProcessName(Convert.ToString(record.NEXT_PROCESS_INVISIBLE));
                record.PRE_PROSESS_STRING = UIUtils.GetNextProcessName(Convert.ToString(record.PRE_PROSESS));
            }
            string view = _viewRenderService.RenderToString("NextPrcessWaitingStatusSearch/NextProcessWaitingStatusSearchGrid", details);
            return Json(new { IsSuccess = true, message = view });
            // return View();
        }

        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP_CUSTOMER_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),
                                          
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        },
                                         new TSSortField {
                                            FieldName = "NEXT_PROCESS_INVISIBLE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Process_L"),
                                             IsDefaultSort=true,

                                        },
                                          new TSSortField {
                                            FieldName = "WIP_STATUS_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_WIPStatus"),
                                             IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName = "PP_DELIVERY_DATE_STRING",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_DeliveryDate"),
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "PP_PROD_NAME_CUST",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                          
                                        },
                                        new TSSortField {
                                            FieldName = "PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=true,
                                          
                                        },
                                        new TSSortField {
                                            FieldName = "PROD_DIM_D,PROD_DIM_W,PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                           
                                        },

                                        new TSSortField {
                                            FieldName = "PP_PROD_LENGTH_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"), 
                                            IsDefaultSort=true,
                                           
                                        },
                                         new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=true,
                                          
                                        },

                                           new TSSortField {
                                            FieldName = "TABA_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                           
                                        },

                                             
                                       
                                      

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        public ActionResult GetProcess()
        {
            var result = _INextProcessWaitingStatusSearchService.GetProcessList();
            var removeIds = new List<int> { 810, 820, 830, 835, 999 , 900 , 901 };//Remove Process clientVisit[july 2025].Remove this process 810, 820, 830, 835 [October 2025]
            result = result.Where(x => !removeIds.Contains(x.CD)).ToList();
            return Ok(new { IsSuccess = true, message = result });

        }

        public IActionResult ExportCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaforNextProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            
            string SQL = _csvSqlString.GetNextProcessWaitingStatusSearchSQL(searchFields, selectedSortCriteria, gridSearchRequest);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName( _localization.GetLocalizedHtmlString("lbl_NextProcessWaitingCSV"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForCSV(gridSearchRequest.CULTURE_NAME);

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

        private List<SearchField> ComposeSearchCriteriaforNextProcess(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROCESS_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.NEXT_PROCESS",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_CD)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private DataTable SetHeaderForCSV(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WIPStatus"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Process_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_lastProcess"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_lastProcessDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Last_Pro_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DelayedDaysA"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DelayedDaysB"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDate(WIP)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LastProcessMessage"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductSpecNo"));

            return dt;
        }
    }
}
