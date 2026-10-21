using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class RollPoMainAnalysisController : Controller
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRollPoMainAnalysisServices _rollPoMainAnalysisService;

        public RollPoMainAnalysisController(IViewRenderService viewRenderService, LocalizationServices localization, IHttpContextAccessor httpContext, IRollPoMainAnalysisServices rollPoMainAnalysisService)
    {
        _viewRenderService = viewRenderService;
            _localization = localization;
            _httpContext = httpContext;
            _rollPoMainAnalysisService = rollPoMainAnalysisService;
        }
    
        public IActionResult RollPoMainAnalysis()
    {
            _httpContext.HttpContext.Session.Remove("MaterialDataForPO");
            return View();
    }
        public IActionResult ForecastStatusPopUp()
        {

            return View();
        }
    public IActionResult MatlNameWiseOrderQtyPopUp()
    {
        return View();
    }
    public IActionResult OrderAndReceiptHistoryPopUp()
    {
        return View();
    }
    public IActionResult AnalysePOQuantity()
    {
        return View();
    }
    public IActionResult RollForecastPopUp()
    {
        return View();
    }
    public IActionResult ProdSpecWiseSalesStatus()
    {
        return View();
    }
    public IActionResult CustomerWiseProdStockShukkaDetails()
    {
        return View();
    }
    public IActionResult ForecastStatusListGrid([FromBody] List<ForecastStatusPopupDetails> details)
    {
        string view = _viewRenderService.RenderToString("RollPoMainAnalysis/ForecastStatusDataList", details);
        return Json(new { IsSuccess = true, message = view });
    }

        public IActionResult GetOneMonthProductForecastDetails([FromBody] List<RollForcastPopup> details)
        {
            if (details != null)
            {
                foreach (RollForcastPopup record in details)
                {
                    if (record.ROLL_FC_KUBUN > 0)
                    {
                        record.PROD_KUBUN = Convert.ToString(UIUtils.GetProductKubunNameByID(Convert.ToString(record.ROLL_FC_KUBUN)));
                    }
                    if (record.MAGARI_KUBUN > 0)
                    {
                        record.BEND_TOLERANCE_STRING = Utils.GetBendTolerance(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN)), Convert.ToDecimal(record.MAGARI_BEND_TOL).ToString("0.000"), Convert.ToInt32(record.MAGARI_SPAN));
                    }
                }
            }
            
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/OneMonthProductForecastGrid", details);
            
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GetOneMonthMaterialForecastDetails([FromBody] List<RollForcastPopup> details)
        {
            if (details != null)
            {
                foreach (RollForcastPopup record in details)
                {
                    record.MATL_KUBUN_NAME = UIUtils.GetMatlTypeCBK(Convert.ToString(record.MATL_KUBUN));
                    record.MATL_SPECIAL_KUBUN_NAME = UIUtils.GetGeneSpecNameForMaterialKubun(Convert.ToString(record.MATL_SPECIAL_KUBUN));
                }
            }
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/OneMonthMaterialForecastGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult MatlNameWiseOrderQtyListGrid([FromBody] List<MatlNameWiseOrderQtyDetails> details)
        {
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/MatlNameWiseOrderQtyList", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult FillProductGrid([FromBody] ProdSpecificationDetails details)
        {
            if (details != null)
            {
                if (details.PRODUCT_KUBUN > 0)
                {
                    details.PRODUCT_KUBUN_STRING = Convert.ToString(UIUtils.GetProductKubunNameByID(Convert.ToString(details.PRODUCT_KUBUN)));
                }
                if (details.MAGARI_KUBUN > 0)
                {
                    details.BEND_TOLERANCE_STRING  = Utils.GetBendTolerance(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(details.MAGARI_KUBUN)), details.MAGARI_BEND_TOL, Convert.ToInt32(details.MAGARI_SPAN));
                }
            }
            string shukkaGrid = string.Empty;
            string ProductGridview = _viewRenderService.RenderToString("RollPoMainAnalysis/ProdStockShukkaDetailsGrid", details);
            //if (details.SHUKKA_DETAILS.Count> 0)
            //{
            //    shukkaGrid = _viewRenderService.RenderToString("RollPoMainAnalysis/ProdShukkaDetailsGrid", details.SHUKKA_DETAILS);
            //}
            return Json(new { IsSuccess = true, message = new { ProductGrid= ProductGridview } });
        }
        public IActionResult MakerWisePurchaseOrderQtyListGrid([FromBody] List<PoAndReceiptPopupDetails> details)
        {
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/MakerWisePurchaseOrderQtyList", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult VendorWisePurchaseOrderQtyListGrid([FromBody] List<PoAndReceiptPopupDetails> details)
        {
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/VendorWisePurchaseOrderQtyList", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GetCustomerDetails([FromBody] List<CustomerWiseProdStockShukka> details)
        {
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/CustomerWiseProdStockShukkaDetailsGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        //public IActionResult GetDispatchQtyCustomerWise([FromBody] List<ProductShukkaDetails> details)
        //{
        //    string view = _viewRenderService.RenderToString("RollPoMainAnalysis/ProdShukkaDetailsGrid", details);
        //    return Json(new { IsSuccess = true, message = details });
        //}

        public ActionResult InitializeSortPopup()
        {
                                      var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                           FieldName = "INTERNAL_NAME_STRING",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"),
                                           //SortOrder=TSSortDirection.Descending
                                           IsDefaultSort=true,
                                        },

                                            new TSSortField {
                                            FieldName = "DIM_D",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                           IsDefaultSort=true,
                                        },
                                            new TSSortField {
                                            FieldName = "SHAPE",   
                                                DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=true,
                                        },
                                              new TSSortField {
                                            FieldName = "LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,

                                        },
                                              new TSSortField {
                                            FieldName = "CONDITION_DESCRIPTION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Condition"),
                                            IsDefaultSort=true,
                                        },
                                                new TSSortField {
                                            FieldName = "MATL_KUBUN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_CoilBarKurokawa_L"),
                                            IsDefaultSort=true,
                                        },
                                                  new TSSortField {
                                            FieldName = "MATL_SPECIAL_KUBUN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_GeneralSpecial_S"),
                                            IsDefaultSort=true,
                                        }
                                    };
            return Ok(new { IsSuccess = true, message = sortFields });
        }
       
        public IActionResult RollPOMainAnalysisGrid([FromBody] List<RollPoMainAnalysisDetails> details)
        {
            foreach (RollPoMainAnalysisDetails record in details)
            {
                record.MATL_KUBUN_NAME = UIUtils.GetMatlTypeCBK(Convert.ToString(record.MATL_KUBUN));
                record.MATL_SPECIAL_KUBUN_NAME = UIUtils.GetGeneSpecNameForMaterialKubun(Convert.ToString(record.MATL_SPECIAL_KUBUN));
            }
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/RollPoMainAnalysisGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetMaterialTypeList()
        {
            var result = _rollPoMainAnalysisService.GetMaterialTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetInternalNameWiseMakerList([FromBody] List<RollPoMainAnalysisDetails> details)
        {
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/GetInternalNameWiseMakerList", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetMaterialMasterDataView([FromBody] List<RollSalesForecastDetails> details)
        {

           

                //List<RollSalesForecastDetails> materialList = new List<RollSalesForecastDetails>();
                //if (TempData["MaterialData"] != null && !string.IsNullOrEmpty(TempData["MaterialData"].ToString()))
                //{
                //    var list = JsonConvert.DeserializeObject<List<RollSalesForecastDetails>>(TempData["MaterialData"].ToString());
                //    materialList.AddRange(list);
                //    materialList.AddRange(details);
                //    TempData["MaterialData"] = JsonConvert.SerializeObject(materialList);
                //}
                //else
                //{
                //    materialList.AddRange(details);
                //    TempData["MaterialData"] = JsonConvert.SerializeObject(materialList);
                //}
                string view = _viewRenderService.RenderToString("RollPoMainAnalysis/MaterialPOGrid", details);
                return Json(new { IsSuccess = true, message = view });
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
        public IActionResult MaterialMasterListGrid([FromBody] List<GridSearchFields> details)
        {
            string view = _viewRenderService.RenderToString("RollPoMainAnalysis/AnalysePoQtyMaterialList", details);
            return Json(new { IsSuccess = true, message = view });
        }

    }
}
