using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Masters
{
    public class MakerMaterialController : Controller
    {
        private readonly IMakerMaterialService _makerMaterialService;
        private readonly LocalizationServices _localization;
        private readonly IHttpContextAccessor _accessor;
        private readonly IViewRenderService _viewRenderService;
        public MakerMaterialController(IMakerMaterialService makerMaterialService, LocalizationServices localization, IHttpContextAccessor httpContextAccessor, IViewRenderService viewRenderService)
        {

            _makerMaterialService = makerMaterialService;
            _localization = localization;
            _accessor = httpContextAccessor;
            _viewRenderService = viewRenderService;
        }


        public IActionResult MakerMaterialRateEntry()
        {
            
            return View();
        }
      
        public IActionResult MakerMaterialRateList()
        {
            return View();
        }
        public IActionResult FillGrid([FromBody] List<MakerMaterialDetails> details)
        {
            
            string view = _viewRenderService.RenderToString("MakerMaterial/MaterialGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult FillListGrid([FromBody] List<MakerMaterialDetails> details)
        {
            foreach (var item in details)
            {
                if (item.MATL_KUBUN > 0)
                {
                    item.MATERIAL_TYPE_STRING = UIUtils.GetMatlTypeName(Convert.ToString(item.MATL_KUBUN));
                }
            }


            string view = _viewRenderService.RenderToString("MakerMaterial/MaterialListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetMaterialType()
        {
            var result = _makerMaterialService.GetMaterialTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                
                                      new TSSortField {
                                            FieldName = "HTMATL.RATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_RatePerKG"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },


                                         new TSSortField {
                                            FieldName = "MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerName"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName = "STL.STEEL_TYPE_NAME ",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_SteelMatlType"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MM.MATL_MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialNameMaker"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }
                                        ,
                                        new TSSortField {
                                            FieldName = "MTL.MATL_KUBUN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialKubun_L"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }
                                        ,
                                        new TSSortField {
                                            FieldName = "MUKESAKI_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MukesakiShort"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }
                                        ,
                                        new TSSortField {
                                            FieldName = "APPLICATION_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ApplicableStartDate"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        }

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
    }
}
