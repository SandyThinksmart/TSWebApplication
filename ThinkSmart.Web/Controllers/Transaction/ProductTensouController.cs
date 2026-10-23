using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class ProductTensouController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        public ProductTensouController(LocalizationServices localization, IViewRenderService viewRenderService)
        {
            _localization = localization;
            _viewRenderService = viewRenderService;
        }
        public IActionResult ProductTensou()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();

        }
        public IActionResult ProductTensouListPage()
        {
            return View();
                
        }

        public IActionResult ProductTensouPopup()
        {
            return View();

        }

        public IActionResult ProductMasterListGrid([FromBody] List<GridSearchFields> details)
        {
            string view = _viewRenderService.RenderToString("ProductTensou/ProductTensouPopupGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult TensouDataSearchInGrid([FromBody] List<WIPTensouDetails> details)
        {
            string view = _viewRenderService.RenderToString("ProductTensou/ProductTensouListPageGrid", details);
            return Json(new { IsSuccess = true, message = view });
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


    }
}
