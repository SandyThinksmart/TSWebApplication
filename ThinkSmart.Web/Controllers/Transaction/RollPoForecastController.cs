using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
using static Google.Cloud.Vision.V1.ProductSearchResults.Types;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class RollPoForecastController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        private readonly IRollPoForecastService _rollPoForecastService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAllCommonService _allCommonService;
        private readonly CsvSqlString _csvSqlString;
        public RollPoForecastController(LocalizationServices localization, IViewRenderService viewRenderService, IRollPoForecastService rollPoForecastService, IHttpContextAccessor httpContext, CsvSqlString csvSqlString, IAllCommonService allCommonService)
        {
            _rollPoForecastService = rollPoForecastService;
            _localization = localization;
            _viewRenderService = viewRenderService;
            _httpContext = httpContext;
            _allCommonService = allCommonService;
            _csvSqlString = csvSqlString;
        }
        public IActionResult RollPoForecast()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult RollSalesForecast()
        {
            _httpContext.HttpContext.Session.Remove("ProductData");
            _httpContext.HttpContext.Session.Remove("MaterialData");
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult RollSalesForecastMaterialMasterList()
        {
            return View();
        }
        public IActionResult RollSalesForecastProductMasterList()
        {
            return View();
        }


        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                           new TSSortField {
                                            FieldName = "ROLL_FC_DATE",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_RequestedDate"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                           new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_CustomerName"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                              new TSSortField {
                                            FieldName = "ROLL_FC_YEAR",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_RollPoYear"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                               new TSSortField {
                                            FieldName = "ROLL_FC_R",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_RollPoNumber"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }

        public IActionResult InitializeSortPopupForMaterialList()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "MTINT.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        },
                                         new TSSortField {
                                            FieldName = "MUKE.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MukesakiShort"),
                                             IsDefaultSort=false,

                                        },
                                          new TSSortField {
                                            FieldName = "SUP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shousha"),
                                             IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "MOCH.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Mochikomisaki"),
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StockInWarehouse"),

                                        },
                                        new TSSortField {
                                            FieldName = "HIKI.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TransCompany"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "MATL.MATL_KUBUN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialKubun_L"),
                                            IsDefaultSort=true,

                                        },

                                        new TSSortField {
                                            FieldName = "MTMAK.MATL_MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),
                                            IsDefaultSort=true,

                                        },
                                         new TSSortField {
                                            FieldName = "MATL.SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=true,

                                        },
                                          new TSSortField {
                                            FieldName = "MATL.DIM_D,MATL.DIM_W,MATL.DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Dimension"),
                                            IsDefaultSort=true,

                                        },

                                           new TSSortField {
                                            FieldName = "MATL.MATERIAL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,

                                        },
                                            new TSSortField {
                                            FieldName = "CON.CONDITION_DESCRIPTION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Condition"),
                                            IsDefaultSort=true,

                                        },
                                             new TSSortField {
                                            FieldName = "MATL.MATL_SPECIAL_KUBUN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_GeneralSpecial_L"),
                                            IsDefaultSort=true,

                                        },
                                              new TSSortField {
                                            FieldName = "MAK.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerMaster"),
                                            IsDefaultSort=true,

                                        },





                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
        public IActionResult InitializeSortPopupForProductList()
        {

            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PRD.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        },
                                         new TSSortField {
                                            FieldName = "PRD.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlDimD"),
                                             IsDefaultSort=false,

                                        },
                                          new TSSortField {
                                            FieldName = "PRD.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                             IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PRD.PRODUCT_KUBUN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProductKubun_L"),
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "PRD.PRODUCT_NAME_CUST",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },
                                        new TSSortField {
                                            FieldName = "PRD.SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName = "PRD.DIM_D,PRD.DIM_W,PRD.DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Prod_DimD"),
                                            IsDefaultSort=true,

                                        },

                                        new TSSortField {
                                            FieldName = "PRD.PRODUCT_LENGTH,PRD.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLength"),
                                            IsDefaultSort=true,

                                        },
                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }



        public IActionResult ProductMasterListGrid([FromBody] List<GridSearchFields> details)
        {
            string view = _viewRenderService.RenderToString("RollPoForecast/ProductList", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult ProductListGrid([FromBody] List<RollSalesForecastDetails> details)
        {
           
            string view = _viewRenderService.RenderToString("RollPoForecast/RollSalesForecastProductGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetRollSalesForecastProductGrid([FromBody] List<RollSalesForecastDetails> details)
        {

            //List<RollSalesForecastDetails> productList = new List<RollSalesForecastDetails>();
            //List<RollSalesForecastDetails> productList1 = new List<RollSalesForecastDetails>();
            //if (_httpContext.HttpContext.Session.GetString("ProductData") != null)
            //{
            //    var list = JsonConvert.DeserializeObject<List<RollSalesForecastDetails>>(_httpContext.HttpContext.Session.GetString("ProductData"));
            //    productList.AddRange(list);
            //    productList.Add(details);
            //    _httpContext.HttpContext.Session.SetString("ProductData", JsonConvert.SerializeObject(productList));

            //}
            //else
            //{
            //    productList.Add(details);
            //    _httpContext.HttpContext.Session.SetString("ProductData", JsonConvert.SerializeObject(productList));
            //}
            string view = _viewRenderService.RenderToString("RollPoForecast/RollSalesForecastProductGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult MaterialMasterListGrid([FromBody] List<GridSearchFields> details)
        {
            string view = _viewRenderService.RenderToString("RollPoForecast/MaterialList", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult MaterailListGrid([FromBody] List<RollSalesForecastDetails> details)
        {
            
            string view = _viewRenderService.RenderToString("RollPoForecast/RollSalesForecastMaterialGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetMaterialTypeList()
        {
            var result = _rollPoForecastService.GetMaterialTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetMatlSpecNameList()
        {
            var result = _rollPoForecastService.GetMatlSpecNameList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetRollSalesForecastMaterialGrid([FromBody] List<RollSalesForecastDetails> details)
        {
            //TempData.Remove("MaterialData");
            //List<RollSalesForecastDetails> materialList = new List<RollSalesForecastDetails>();
            ////if (_httpContext.HttpContext.Session.GetString("MaterialData") != null)
            //if (TempData["MaterialData"] != null && !string.IsNullOrEmpty(TempData["MaterialData"].ToString()))
            //{
            //    //var list = JsonConvert.DeserializeObject<List<RollSalesForecastDetails>>(_httpContext.HttpContext.Session.GetString("MaterialData"));
            //    var list = JsonConvert.DeserializeObject<List<RollSalesForecastDetails>>(TempData["MaterialData"].ToString());
            //    materialList.AddRange(list);
            //    materialList.Add(details);
            //    TempData["MaterialData"] = JsonConvert.SerializeObject(materialList);
            //    // _httpContext.HttpContext.Session.SetString("MaterialData", JsonConvert.SerializeObject(materialList));

            //}
            //else
            //{
            //    materialList.Add(details);
            //    TempData["MaterialData"] = JsonConvert.SerializeObject(materialList);
            //    //_httpContext.HttpContext.Session.SetString("MaterialData", JsonConvert.SerializeObject(materialList));
            //}
            string view = _viewRenderService.RenderToString("RollPoForecast/RollSalesForecastMaterialGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }

        #region Bind Product And Material Sales Forecast List Page Data By Komal K
        public IActionResult GridSearchProductList([FromBody] List<RollSalesForecastDetails> details)
        {
            string view = _viewRenderService.RenderToString("RollPoForecast/RollPoProductGrid", details);

            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GridSearchMaterialList([FromBody] List<RollSalesForecastDetails> details)
        {
            string view = _viewRenderService.RenderToString("RollPoForecast/RollPoMaterialGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        #endregion

        public IActionResult ExportCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaforNextProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            string SQL = _csvSqlString.GetProductForecastDetailsSQL(searchFields, selectedSortCriteria, gridSearchRequest);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName("ProductDetail", addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForCSV(gridSearchRequest.CULTURE_NAME, gridSearchRequest);

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
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "SF.CUST_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.ROLL_FC_YEAR > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "SF.ROLL_FC_YEAR",//ROLL_FC_YEAR
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.ROLL_FC_YEAR)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.ROLL_FC_R > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "SF.ROLL_FC_R",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.ROLL_FC_R)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private DataTable SetHeaderForCSV(string cultureName, GridSearchFields gridSearchRequest)
        {
            _allCommonService.GetCultureName(cultureName);
            string m1 = gridSearchRequest.MonthRollHeaders?.Month1;
            string m2 = gridSearchRequest.MonthRollHeaders?.Month2;
            string m3 = gridSearchRequest.MonthRollHeaders?.Month3;
            string r1 = gridSearchRequest.MonthRollHeaders?.Roll1;
            string r2 = gridSearchRequest.MonthRollHeaders?.Roll2;
            string r3 = gridSearchRequest.MonthRollHeaders?.Roll3;
            string r4 = gridSearchRequest.MonthRollHeaders?.Roll4;

            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductKubun_L")); 
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("DispatchMasterKubun_10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Diameter"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Width"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AuxiliaryOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Frequency/Bend"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Condition"));
            dt.Columns.Add($"{m1}(ActualShukka)");
            dt.Columns.Add($"{m2}(ActualShukka)");
            dt.Columns.Add($"{m3}(ActualShukka)");
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Average"));
            dt.Columns.Add($"{r1}(Estimated Amount)");
            dt.Columns.Add($"{r2}(Estimated Amount)");
            dt.Columns.Add($"{r3}(Estimated Amount)");
            dt.Columns.Add($"{r4}(Estimated Amount)");
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductSpecNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblMaterialNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Internal"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_RemainingStock_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedProductStock"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Msg"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InquiryCompleted"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun2of2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Material_Reply"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Maker"));

            return dt;
        }

        public IActionResult ExportMaterialCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaforNextProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            string SQL = _csvSqlString.GetMaterialForecastDetailsSQL(searchFields, selectedSortCriteria, gridSearchRequest);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName("MaterialDetail", addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetMaterialHeaderForCSV(gridSearchRequest.CULTURE_NAME, gridSearchRequest);

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

        private DataTable SetMaterialHeaderForCSV(string cultureName, GridSearchFields gridSearchRequest)
        {
            _allCommonService.GetCultureName(cultureName);
            string m1 = gridSearchRequest.MonthRollHeaders?.Month1;
            string m2 = gridSearchRequest.MonthRollHeaders?.Month2;
            string m3 = gridSearchRequest.MonthRollHeaders?.Month3;
            string r1 = gridSearchRequest.MonthRollHeaders?.Roll1;
            string r2 = gridSearchRequest.MonthRollHeaders?.Roll2;
            string r3 = gridSearchRequest.MonthRollHeaders?.Roll3;
            string r4 = gridSearchRequest.MonthRollHeaders?.Roll4;

            DataTable dt = new DataTable();
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Maker_Details"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MakerName")); 
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialKubun_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Diameter"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Width"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Auxiliary"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add($"{r1}(Estimated Amount)");
            dt.Columns.Add($"{r2}(Estimated Amount)");
            dt.Columns.Add($"{r3}(Estimated Amount)");
            dt.Columns.Add($"{r4}(Estimated Amount)");
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Investigation_End"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MakerMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Internal"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_GeneralSpecial_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Condition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DirectLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Material_Reply"));

            return dt;
        }
    }
}
