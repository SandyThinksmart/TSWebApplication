using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Model;


namespace TSWebApplication.Web.Controllers.Transaction

{
    public class TagWiseStockStatusSearchController : Controller
    {
        private readonly LocalizationServices _localization;
        public TagWiseStockStatusSearchController(LocalizationServices localization)
        {
          
            _localization = localization;
           
        }
        public IActionResult TagWiseStockStatusSearch()
        {
            return View();
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                       
                                         new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        },

                                        new TSSortField {
                                            FieldName = "SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Condition"),
                                           
                                        },
                                        new TSSortField {
                                            FieldName = "DIM_D, DIM_W, DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InventoryLocation"),
                                         
                                        },
                                        new TSSortField {
                                            FieldName = "PRODUCT_LENGTH,LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                           
                                        },
                                           new TSSortField {
                                            FieldName = "CHUUBAN_GYOUBAN",
                                           //  DisplayName ="CHUUBAN",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),
                                        },
                                          new TSSortField {
                                            FieldName = "NIFUDA",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                        },


                                        new TSSortField {
                                            FieldName = "INVOICE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("製品荷札№"),
                                        },

                                        new TSSortField {
                                            FieldName = "NIFUDA_GEN_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_NyukoDate"),
                                         
                                        },
                                        new TSSortField {
                                            FieldName = "YOBINA,YOBIGANA,PLACE_OF_ISSUE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "NYUKA_CD",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                         },
                                          new TSSortField {
                                            FieldName = "NYUKA_CD",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                         },
                                            new TSSortField {
                                            FieldName = "NYUKA_CD",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                         },
                                            new TSSortField {
                                            FieldName = "NYUKA_CD",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Kubun"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                         },
                                    };

            
            return Json(new { IsSuccess = true, message = sortFields });
        }
    }
}
