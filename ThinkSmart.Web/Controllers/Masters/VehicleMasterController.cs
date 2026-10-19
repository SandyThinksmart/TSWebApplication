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
    public class VehicleMasterController : Controller
    {
        private readonly IMakerMaterialService _makerMaterialService;
        private readonly LocalizationServices _localization;
        private readonly IHttpContextAccessor _accessor;
        private readonly IViewRenderService _viewRenderService;

        public VehicleMasterController(IMakerMaterialService makerMaterialService, LocalizationServices localization, IHttpContextAccessor accessor, IViewRenderService viewRenderService)
        {
            _makerMaterialService = makerMaterialService;
            _localization = localization;
            _accessor = accessor;
            _viewRenderService = viewRenderService;
        }

        public IActionResult VehicleEntryEditDelete(int? vehicleId)
        {
            if (vehicleId.HasValue)
            {
                ViewBag.VehicleId = vehicleId.Value;
            }

            return View();
        }

        public IActionResult VehicleList()
        {
            return View();
        }

        public IActionResult GridSearch([FromBody] List<VehicleDetails> details)
        {
            string view = _viewRenderService.RenderToString("VehicleMaster/VehicleGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "YOBIGANA",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Yobigana_L"),
                                             IsDefaultSort=true,
                                             SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "VEHICLE_CD",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_VehicleOode"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                         new TSSortField {
                                            FieldName = "VEHICLE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_VehicleName"),
                                             IsDefaultSort=false,
                                             SortOrder=TSSortDirection.Descending
                                        },

                                         new TSSortField {
                                            FieldName = "SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ShortName"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
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
                                            FieldName =   "YOBINA",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Yobina"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        //      new TSSortField {
                                        //    FieldName =   "PP_PROD_LENGTH_OTHER",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                        //    IsDefaultSort=true,
                                        //    SortOrder=TSSortDirection.Ascending
                                        //},

                                            new TSSortField {
                                            FieldName = "DRIVER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DriverName"),
                                           IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                           new TSSortField {
                                            FieldName = "CREATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_CreatedDate_L"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                             new TSSortField {
                                            FieldName =   "UPDATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_UpdatedDate_L"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },


                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }
    }
}
