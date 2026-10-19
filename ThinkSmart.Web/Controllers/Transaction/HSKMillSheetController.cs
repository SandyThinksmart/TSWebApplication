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
using TSWebApplication.Application.Services;
using TSWebApplication.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class HSKMillSheetController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IHSKMillSheetServices _hskMillSheetService;
        //private readonly IStamppingService _stamppingService;
        private readonly IViewRenderService _viewRenderService;
      
        public HSKMillSheetController(IHSKMillSheetServices hskmillsheet, LocalizationServices localization, IViewRenderService viewRenderService)
        {

            _hskMillSheetService = hskmillsheet;
            _localization = localization;
            //_stamppingService = stamppingService;
            _viewRenderService = viewRenderService;

        }

        public IActionResult HSKMillSheetSaveEdit()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }


        public IActionResult HSKMillSheetHistorypopup()
        {
            return View();
        }

        public IActionResult HSKMillSheetEditDelete()
        {
            return View();
        }

        public IActionResult MakerMillSheetChemicalCompositionPopup()
        {
            return View();
        }
        public IActionResult HSKMillSheetFinalInspectionPopup()
        {
            return View();
        }
        public IActionResult HSKMillSheetChubanLiveCheck()
        {
            return View();
        }
        public IActionResult HSKMillSheetPostHTInspection()
        {
            return View();
        }

        public IActionResult InitializeSortPopupMakerMillSheet()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "MM.CHARGE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Charge"),
                                           // IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        // new TSSortField {
                                        //    FieldName = "MM.SHAPE",
                                        //    DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                        //   // IsDefaultSort=true,
                                        //    SortOrder=TSSortDirection.Descending
                                        //},

                                            new TSSortField {
                                            FieldName = "MM.CREATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_CreatedDate_L"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                            new TSSortField {
                                            FieldName = "MM.MATL_NAME_MAKER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialNameMaker"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MM.DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "MM.MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Maker"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }


        //public IActionResult InitializeSortPopup()
        //{
        //    var sortFields = new List<TSSortField> {

        //                                new TSSortField {
        //                                    FieldName = "CUST.CUSTOMER_NAME",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
        //                                   // IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Descending
        //                                },

        //                                new TSSortField {
        //                                    FieldName = "PP.ACT_ALLOC_DATE",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ActAllocDate"),
        //                                   // IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Descending
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "M.MAKER_NAME",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerMaster"),
        //                                   // IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Descending
        //                                },

        //                                   new TSSortField {
        //                                    FieldName = "PP.DELIVERY_DATE",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_DeliveryDate"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Descending
        //                                },
        //                                    new TSSortField {
        //                                    FieldName = "PP.PROD_NAME_CUST",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "PP.PROD_DIM_D",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                 new TSSortField {
        //                                    FieldName = "PP.PROD_SHAPE",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                   new TSSortField {
        //                                    FieldName = "PP.PROD_LENGTH",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                new TSSortField {
        //                                    FieldName = "CND.CONDITION_DESCRIPTION",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Condition"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },
        //                                 new TSSortField {
        //                                    FieldName = "PP.CHUUBAN",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },

        //                                new TSSortField {
        //                                    FieldName = "Irai_DATE",
        //                                    DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),
        //                                     IsDefaultSort=true,
        //                                    SortOrder=TSSortDirection.Ascending
        //                                },


        //                            };
        //    return Json(new { IsSuccess = true, message = sortFields });
        //}

        public IActionResult InitializeEditSortPopup()
        {



            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                           // IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("需要家"),
                                            // IsDefaultSort=true,
                                             SortOrder=TSSortDirection.Ascending
                                        },

                                         new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Condition"),
                                            //IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },

                                            new TSSortField {
                                            FieldName =   "HKS.YOBINA, HKS.YOBIGANA, HKS.SHORT_NAME" ,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Charge"),
                                           // IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerName"),
                                            // IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "LOC.YOBINA, LOC.YOBIGANA,LOC.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DelDate_L"),
                                           // IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Issue"),
                                           // IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_CreatedDate_L"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                            new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                          new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                           new TSSortField {
                                            FieldName = "Irai_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        public IActionResult InitializeSortPopupMikSheetList()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "HMS.CHUUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.CUST_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),                                           
                                            SortOrder=TSSortDirection.Descending
                                        },
                                       
                                          new TSSortField {
                                            FieldName = "HMS.CHARGE_NO",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Charge"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                            new TSSortField {
                                            FieldName = "HMS.MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerName"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                             new TSSortField {
                                            FieldName = "HMS.DELIVERY_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DelDate_L"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "HMS.INZU",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Issue"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                            new TSSortField {
                                            FieldName = "HMS.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"), 
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "HMS.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "HMS.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"), 
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        
                                          new TSSortField {
                                            FieldName = "CND.CONDITION_DESCRIPTION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Condition"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "HMS.RECEIVER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Receiver"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "HMS.CREATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_CreatedDate_L"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "HMS.PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        public IActionResult GridSearch([FromBody] List<HSKMillSheetDetails> details)
        {
            string view = _viewRenderService.RenderToString("HSKMillSheet/HSKMillSheetListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GridSearchHistoryPopup([FromBody] List<HSKMillSheetDetails> details)
        {
            string view = _viewRenderService.RenderToString("HSKMillSheet/HSKMillSheetHistorypopupGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult HSKMillSheetGridSearchChuubanLiveCheck([FromBody] List<HSKMillSheetDetails> details)
        {
            string view = _viewRenderService.RenderToString("HSKMillSheet/HSKMillSheetChubanLiveCheckPopUpGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GetHSKMillSheetPostHTInspectionDetails([FromBody] List<HSKMillSheetDetails> details)
        {
            string view = _viewRenderService.RenderToString("HSKMillSheet/HSKMillSheetPostHTInspectionGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GetHSKMillSheetFinalInspectionDetails([FromBody] List<HSKMillSheetDetails> details)
        {
            string view = _viewRenderService.RenderToString("HSKMillSheet/HSKMillSheetFinalInspectionpopupGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
    }
}
