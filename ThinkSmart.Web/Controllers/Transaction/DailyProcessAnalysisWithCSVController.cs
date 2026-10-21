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
using Microsoft.Extensions.DependencyModel.Resolution;
using System.Collections;
using System.Data;
using TSWebApplication.Application.IServices;
using System.IO;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class DailyProcessAnalysisWithCSVController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        private readonly IHardnessTemperingService _hardnessTemperingService;
        private readonly IHeatTreatmentService _heatTreatmentService;
        private readonly IHikinukiBarToBarService _hikinukiBarToBarService;
        private readonly IHikinukiCoilToBarService _hikinukiCoilToBarService;
        private readonly IGeneralDailyProcessService _generalDailyProcessService;
        private readonly IColdDrawingWireDrawingService _coldDrawingWireDrawingService;
        private readonly IStraighteningService _straighteningService;
        private readonly IStraightening2Service _straightening2Service;
        private readonly IChamfferService _chamfferService;
        private readonly IECService _ecService;
        private readonly IStamppingService _stamppingService;
        private readonly IPostHTInspectionService _postHTInspectionService;
        private readonly IMIHardnessService _mIHardnessService;
        private readonly ITensileService _tensileService;
        private readonly IFinalInspectionService _finalInspectionService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IAllCommonService _allCommonService;
        private readonly CsvSqlString _csvSqlString;
        public DailyProcessAnalysisWithCSVController(IHostEnvironment hostEnvironment,LocalizationServices localization, IViewRenderService viewRenderService, IHardnessTemperingService hardnessTemperingService, IHeatTreatmentService heatTreatmentService,
                                                     IHikinukiBarToBarService hikinukiBarToBarService, IHikinukiCoilToBarService hikinukiCoilToBarService,IGeneralDailyProcessService generalDailyProcessService,IColdDrawingWireDrawingService coldDrawingWireDrawingService,
                                                     IStraighteningService straighteningService,IStraightening2Service straightening2Service,IChamfferService chamfferService,IECService eCService,IStamppingService stamppingService,IPostHTInspectionService postHTInspectionService,
                                                     IMIHardnessService mIHardnessService, ITensileService tensileService,IFinalInspectionService finalInspectionService, IAllCommonService commonService, CsvSqlString csvSqlString)
        {
            _localization = localization;           
            _viewRenderService = viewRenderService;
             _hardnessTemperingService = hardnessTemperingService;
            _heatTreatmentService = heatTreatmentService;
            _hikinukiBarToBarService = hikinukiBarToBarService;
            _hikinukiCoilToBarService = hikinukiCoilToBarService;
            _generalDailyProcessService = generalDailyProcessService;
            _coldDrawingWireDrawingService = coldDrawingWireDrawingService;
            _straighteningService = straighteningService;
            _straightening2Service = straightening2Service;
            _chamfferService = chamfferService;
            _ecService = eCService;
            _stamppingService = stamppingService;
            _postHTInspectionService = postHTInspectionService;
            _mIHardnessService = mIHardnessService;
            _tensileService = tensileService;
            _finalInspectionService = finalInspectionService;
            _hostEnvironment = hostEnvironment;
            _allCommonService = commonService;
            _csvSqlString = csvSqlString;
        }
        string cultureName = CultureInfo.CurrentCulture.Name;
        public IActionResult DailyProcessAnalysis()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        
        public IActionResult GridSearch([FromBody] List<StamppingDetails> details)
        {
           string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/DailyProcessAnalysisStamppingGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult InitializeSortPopup(string activeTabID)
        {
            var sortFields = new List<TSSortField>();
            var sortFields1 = new List<TSSortField>();


            if (activeTabID == "titleHardnessTempering")
            {
             
                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },
                                          new TSSortField {
                                            FieldName = "HT.FURNACE_NO",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceNum"),

                                        },

                                         new TSSortField {
                                            FieldName = "MACH.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_QuenchingMachine"),

                                        },
                                         new TSSortField {
                                            FieldName = "MACT.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TemperingMachine"),

                                        },
                                        
                                         new TSSortField {
                                            FieldName = "START_DATE_HARDENING_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime(Quenching)"),

                                        },
                                         new TSSortField {
                                            FieldName = "END_DATE_HARDENING_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceOutDateTime(Quenching)"),

                                        },

                                         new TSSortField {
                                            FieldName = "START_DATE_TEMPERING_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime(Tempering)"),

                                        },
                                         new TSSortField {
                                            FieldName = "END_DATE_TEMPERING_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceOutDatetime(Tempering)"),

                                        },
                                         new TSSortField {
                                            FieldName = "INSPECTION_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InspectionDate"),

                                        },
            };

            }


            if (activeTabID == "titleHeatTreatment")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                           new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },

                                          new TSSortField {
                                            FieldName = "HT.FURNACE_NO",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceNum"),

                                        },

                                        
                                         new TSSortField {
                                            FieldName = "HT.HT_CONDITION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProcCondition"),

                                        },

                                         new TSSortField {
                                            FieldName = "HT.PATTERN_NO",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_PatternNo_L"),

                                        },
                                         new TSSortField {
                                            FieldName = "HT_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime(lbl_StartTime)"),

                                        },

                                         new TSSortField {
                                            FieldName = "HT_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_FurnaceOutDateTimeEnd"),

                                        },
                                        
            };

            }
            if (activeTabID == "titleDrawingBarToBar")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CON.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },                                           
                                        
                                         new TSSortField {
                                            FieldName = "DW_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "DW_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };
            }

            if (activeTabID == "titleDrawingCoilToBar")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "DW_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "DW_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }

            if (activeTabID == "titleDrawingCoilToCoil")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CON.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "DW_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "DW_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }

            if (activeTabID == "titleStraightening1")
            {
                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MACH.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOCA.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "STR_START_DATE_TIME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "STR_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }

            if (activeTabID == "titleStraightening2")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MACH.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOCA.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "STR_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "STR_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }

            if (activeTabID == "titleChamfer")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "CH_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "CH_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }

            if (activeTabID == "titleEC")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CON.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "EC_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "EC_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }
            if (activeTabID == "titleStamping")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CON.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "ST_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },

                                         new TSSortField {
                                            FieldName = "ST_END_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_EndDateTime"),

                                        },

            };

            }

            if (activeTabID == "titlePostHTHardness")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                        
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                          new TSSortField {
                                            FieldName = "LOC.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "INT_QLTY_TEST_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InspectionDate"),

                                        },

                                        

            };

            }

            if (activeTabID == "titleMIHardness")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },
                                        
                                          new TSSortField {
                                            FieldName = "LOCA.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "HD_TEST_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InspectionDate"),

                                        },



            };

            }

            if (activeTabID == "titleMechTensile")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },

                                          new TSSortField {
                                            FieldName = "LOCA.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "TS_TEST_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InspectionDate"),

                                        },



            };

            }
            if (activeTabID == "titleFinalInspection")
            {

                sortFields = new List<TSSortField> {

                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },

                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),

                                        },

                                          new TSSortField {
                                            FieldName = "LOCA.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                         new TSSortField {
                                            FieldName = "INSPECTED_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InspectionDate"),

                                        },
                                         new TSSortField {
                                            FieldName = "SPARK_INSP_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_SparkFIDate"),

                                        },

                                         


            };

            }

            if (activeTabID == "titleGeneralDailyProcess")
            {
                string processKubun = "";
                if (cultureName == "en")
                {
                     processKubun = "PROCK.KUBUN_NAME_EN";
                }
                else
                {
                    processKubun = "PROCK.KUBUN_NAME";
                }

                sortFields = new List<TSSortField> {                     
                                        
                                          new TSSortField {
                                            FieldName = "CUST.SHORT_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Customer"),

                                        },
                                          new TSSortField {
                                            FieldName = "INTERN.INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"),

                                        },
                                           new TSSortField {
                                            FieldName = "PP.MATL_NAME_MAK",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"),

                                        },
                                             new TSSortField {
                                            FieldName = "CONDITION.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ConditionMaster"),

                                        },
                                               new TSSortField {
                                            FieldName = "CONDITION1.CONDITION_DESCRIPTION",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_MatlCondition"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP_GYOUBAN_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Gouban"),

                                        },

                                                    new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO1",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_SPEC_NO2",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.VER",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_VER"),

                                        },
                                                new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProdNameCust"),

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimW_L"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdLengthText"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_TYPE_CBK",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialShape_S"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimDD"),
                                            IsDefaultSort=false,

                                        },
                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_W",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimW"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialDimOther"),
                                            IsDefaultSort=false,

                                        },

                                        new TSSortField {
                                            FieldName = "PP.MATL_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialLength_L"),
                                            IsDefaultSort=false,

                                        },
                                         
                                         new TSSortField {
                                            FieldName = "PP.ACT_ALLOC_DATE",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ActAllocDate"),
                                            
                                        },                                        

                                         new TSSortField {
                                            FieldName = processKubun,
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_ProcessKubun_L"),

                                        },
                                         new TSSortField {
                                            FieldName = "MAC.MACHINE_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Machine_L"),

                                        },
                                         new TSSortField {
                                            FieldName = "GP_LOCATION_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Location"),

                                        },
                                         new TSSortField {
                                            FieldName = "GP_START_DATE_STRING",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_StartDateTime"),

                                        },


            };

            }
            sortFields = sortFields1.Concat(sortFields).ToList();
            return Ok(new { IsSuccess = true, message = sortFields });

        }
        public IActionResult StraighteningDailyProcessGrid([FromBody] List<StraighteningDetails> details)
        {
            foreach (StraighteningDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if (record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_R_STRING = record.PP_CORNER_RADIUS + "R";
                }

               
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    //record.PP_HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.STR_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.STR_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.STR_BEND_STATUS_DECODE = record.STR_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.STR_BEND_STATUS)) : string.Empty;
                    record.STR_EXT_TEXTURE_STATUS_DECODE = record.STR_EXT_TEXTURE != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.STR_EXT_TEXTURE)) : string.Empty;
                    record.STR_TP_TEST_STRING = UIUtils.GetTPTestKubunValue(Convert.ToString(record.STR_TP_TEST));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult HeatTreatmentGrid([FromBody] List<HeatTreatMentDetails> details)
        {
            foreach (HeatTreatMentDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if (record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }
               
                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    //record.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
                    record.PP_MAGARI_KUBUN = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.COLOR_ID_STRING = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));
                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));
                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));
                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));
                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));
                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));
                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));                    
                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PROCESS_PLAN_KUBUN));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.HT_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.COIL_BAR_TYPE_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.COIL_BAR_TYPE));
                    record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));




            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/HeatTreatmentDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessStamppingReportGrid([FromBody] List<StamppingDetails> details)
        {
            foreach(StamppingDetails record in details)
            {
                if(record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if(record.PP_CORNER_R > 0)
                {
                    record.PP_CORNER_R_STRING = record.PP_CORNER_R + "R";
                }

                
                    record.PP_MAGARI_KUBUN = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    record.ST_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.ST_UNIT_TYPE));
                    record.ST_TRIAL_RESULT_STRING = UIUtils.GetTrialResultKubun(Convert.ToString(record.ST_TRIAL_RESULT));
                    record.ST_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.ST_DECISION));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));
                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                //record.STAMP_DETAILS = UIUtils.GetStampbykubun(record.STAMP_DETAILS);
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));


            }

            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/DailyProcessAnalysisStamppingGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult HardnessTemperingDailyProcessGrid([FromBody] List<HardeningTemperingDetails> details)
        {
            
                foreach (HardeningTemperingDetails record in details)
                {
                    if (record.PP_CHAMFER > 0)
                    {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                    }
                    if (record.PP_CORNER_RADIUS > 0)
                    {
                    record.PP_CORNER_R_STRING = record.PP_CORNER_RADIUS + "R";
                    }

               
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));

                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PROCESS_PLAN_KUBUN));
                    record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.WATER_OIL_STRING = UIUtils.GetHardnessOilTypeKubunValue(Convert.ToString(record.WATER_OIL_TYPE));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));

            }


            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/HardnessTemperingDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult StraighteningDailyProcessGrid2([FromBody] List<Straightening2Details> details)
        {
            foreach (Straightening2Details record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if (record.CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_R_STRING = record.CORNER_RADIUS + "R";
                }
                

                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    record.STR_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                    //record.PP_HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.STR_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.STR_UNIT_TYPE));
                    record.STR_BEND_STATUS_DECODE = record.STR_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.STR_BEND_STATUS)) : string.Empty;
                    record.STR_EXT_TEXTURE_STATUS_DECODE = record.STR_EXT_TEXTURE != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.STR_EXT_TEXTURE)) : string.Empty;
                    record.STR_TP_TEST_STRING = UIUtils.GetTPTestKubunValue(Convert.ToString(record.TP_TEST));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));



            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid2", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessECDetailsGrid([FromBody] List<ECDetails> details)
        {
            foreach(ECDetails record in details)
            {
                if(record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if(record.PP_CORNER_R > 0)
                {
                    record.PP_CORNER_R_STRING = record.PP_CORNER_R + "R";
                }
                
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));


                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));
                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.EC_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.EC_DECISION));
                    //record.EC_MAGNETISM_STRING = UIUtils.GetMagneticDustList(Convert.ToString(record.MAGNETISM));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.TP_TEST_STRING = UIUtils.GetTPTestKubunValue(Convert.ToString(record.TP_TEST));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/DailyProcessAnalysisECDetailsGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessDrawingDGrid([FromBody] List<HikinukiBarToBarDetails> details)
        {
            foreach (HikinukiBarToBarDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;

                }
                if (record.PP_CORNER_R > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_R + "R";
                }
               
                    record.DW_BEND_STATUS_DECODE = UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS));
                    record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.DW_EXT_TEXTURE_STATUS_DECODE = UIUtils.GetExteriorCondition(Convert.ToString(record.DW_EXT_TEXTURE_STATUS));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));
                    //record.PP_COIL_DIA_WINDING = record.PP_COIL_DIA_WINDING;
                    record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));


                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));


                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));


                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));


                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));


                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                //record.HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));
                record.DW_DIE_SHAPE_DECODE = UIUtils.GetDieShapeList(Convert.ToString(record.DIE_SHAPE));



            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/DailyProcessAnalysisHikinukiBarToBarGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessColdDrawingWGrid([FromBody] List<ColdDrawingWireDrawingDetails> details)
        {
            foreach (ColdDrawingWireDrawingDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;

                }
                if (record.PP_CORNER_R > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_R + "R";
                }
                
                    record.DW_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.DW_UNIT_TYPE));
                    record.DW_EXT_TEXTURE_STATUS_DECODE = UIUtils.GetExteriorCondition(Convert.ToString(record.DW_EXT_TEXTURE_STATUS));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));

                    record.DW_WINDING_SHAPE1 = record.DW_WINDING_SHAPE != null ? UIUtils.GetColdDrawingWindingShape(Convert.ToString(record.DW_WINDING_SHAPE)) : string.Empty;

                    record.DW_DW_MAGNETIC_INSPECTIONS = UIUtils.GetMagneticDustList(Convert.ToString(record.DW_MAGNETIC_INSPECTIONS));
                    record.DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));
                record.DW_DIE_SHAPE_DECODE = UIUtils.GetDieShapeList(Convert.ToString(record.DW_DIE_SHAPE));


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/DailyProcessAnalysisColdDrawingWireDrawingWGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessFinalInspectionDetailsGrid([FromBody] List<FinalInspectionDetails> details)
        {
            foreach (FinalInspectionDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;

                }
                if (record.CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.CORNER_RADIUS + "R";
                }

               
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.COLORNAME = UIUtils.GetColorKubunValue(Convert.ToString(record.COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.FI_DIM_DECISION_VALUE = UIUtils.GetDimDecisionKubunValue(Convert.ToString(record.FI_DIM_DECISION));
                    record.FI_COLOR_DECISION_STRING = UIUtils.GetColorDecisionKubunValue(Convert.ToString(record.FI_COLOR_DECISION));
                    record.FI_LENGTH_DECISION_STRING = UIUtils.GetLengthDecisionKubunValue(Convert.ToString(record.FI_LENGTH_DECISION));
                    record.FI_CHAMFERING_DECISION_STRING = UIUtils.GetChamferingDecisionKubunValue(Convert.ToString(record.FI_CHAMFERING_DECISION));
                    record.FI_EXT_TEXTURE_DECISION_STRING = UIUtils.GetExtTextureDecisionKubunValue(Convert.ToString(record.FI_EXT_TEXTURE_DECISION));
                    record.FI_SPARK_DECISION_STRING = UIUtils.GetSparkDecisionKubunValue(Convert.ToString(record.FI_SPARK_DECISION));
                    record.COMBINED_DECISION_VALUE = UIUtils.GetResultKubunValue(Convert.ToString(record.COMBINED_DECISION));
                    record.FI_CORNER_RADIUS_STD_DECISION_STRING = UIUtils.GetCornerRadiuscisionKubunValue(Convert.ToString(record.FI_CORNER_RADIUS_STD_DECISION));

                    record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));

            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/FinalInspectionDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult ChamfferDailyProcessGrid([FromBody] List<ChamferDetails> details)
        {
            foreach (ChamferDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-"+ record.PP_CHAMFER;

                }
                if(record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }
                if(record.CH_SHAPE > 0)
                {
                    record.CH_SHAPE_STRING =  "C-" + record.CH_SHAPE;
                }

               
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                   // record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.CH_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.CH_DECISION));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));

                    record.CH_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));

                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/ChamfferDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult HikinukiCoilToBarDailyProcessGrid([FromBody] List<HikinukiDetails> details)
        {
            foreach (HikinukiDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;

                }
                if (record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }
                
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.DW_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

                    record.DW_BEND_STATUS_DECODE = record.DW_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS)) : string.Empty;
                    record.DW_EXT_TEXTURE_STATUS_DECODE = record.DW_EXT_TEXTURE_STATUS != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.DW_EXT_TEXTURE_STATUS)) : string.Empty;
                    record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                    record.DW_TP_TEST_DESC = UIUtils.GetTPTestKubunValue(Convert.ToString(record.DW_TP_TEST));
                    record.DW_DIE_SHAPE_DECODE = UIUtils.GetDieShapeList(Convert.ToString(record.DW_DIE_SHAPE));
                    record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                    record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));



            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/HikinukiCoilToBarDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessMechTensileDetailsGrid([FromBody] List<TensileDetails> details)
        {
            foreach(TensileDetails record in details)
            {
                if(record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if(record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }
               
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));

                    record.TS_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.TS_DECISION));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));

                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));
                record.TS_ACT_DIM_SHAPE = UIUtils.GetDieShapeList(record.TS_ACT_DIM_SHAPE);


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/TensileDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult QATestHardnessDailyProcessGrid([FromBody] List<PostHTDetails> details)
        {
            foreach(PostHTDetails record in details)
            {
                if(record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if(record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }

               
                    record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN));
                    record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                    //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                    record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                    record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                    record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                    record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                    record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                    record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                    record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                    record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));


                    record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                    record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                    record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                    record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                    record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                    record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));

                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));

                    //record.HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
                    record.INT_QLTY_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.INTERNAL_QUALITY_DECISION));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/QATestHardnessDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }
        public IActionResult GetDailyProcessMechHardnessDetailsGrid([FromBody] List<MIHardnessDetails> details)
        {
            foreach(MIHardnessDetails record in details)
            {
                if(record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if(record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }                
                   
                        record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                        record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN));
                        record.HD_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.HD_DECISION));
                        record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                        //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));


                        record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                        record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                        record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                        record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                        record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                        record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                        record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                        record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                        record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                        record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                        //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                        record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                        record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                        record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));
                        record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));

                        record.HD_PATTERN_ACT_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HD_PATTERN_ACT));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));


            }
            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/MiHardnessDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });

        }

        public IActionResult GeneralProcessDailyProcessGrid([FromBody] List<GeneralDailyProcessDetails> details)
        {
            foreach (GeneralDailyProcessDetails record in details)
            {
                if (record.PP_CHAMFER > 0)
                {
                    record.PP_CHAMFER_STRING = "C-" + record.PP_CHAMFER;
                }
                if (record.PP_CORNER_RADIUS > 0)
                {
                    record.PP_CORNER_RADIUS_STRING = record.PP_CORNER_RADIUS + "R";
                }
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                record.PP_MAGARI_KUBUN_STRING = UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN));
                record.PP_UNIT_WT_KUBUN_STRING = UIUtils.GetUnitWtKubun(Convert.ToString(record.PP_UNIT_WT_KUBUN));
                //record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.PP_COLOR_ID));

                record.TENSILE_STRN_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.TENSILE_STRN_PATTERN1_MIN_MAX));

                record.ELONGATION_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.ELONGATION_PATTERN1_MIN_MAX));

                record.HARDNESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_MIN_MAX));

                record.YEILD_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.YEILD_PATTERN1_MIN_MAX));

                record.PRESS_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.PRESS_PATTERN1_MIN_MAX));

                record.PP_DECARB_STD_1 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_1));
                record.PP_DECARB_STD_2 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_2));
                record.PP_DECARB_STD_3 = UIUtils.GetDecarbKubunStd(Convert.ToString(record.DECARB_3));

                record.IMPACT_TEST_PATTERN1_MIN_MAX_STRING = UIUtils.GetardnessMaxMinKubunValue(Convert.ToString(record.IMPACT_TEST_PATTERN1_MIN_MAX));

                //record.PP_HT_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));

                record.PP_PROCESS_PLAN_KUBUN_STRING = UIUtils.GetProcessPlanKubun(Convert.ToString(record.PP_PROCESS_PLAN_KUBUN));
                record.PP_PROD_MNGMNT_KUBUN_STRING = UIUtils.GetProductManagementKubun(Convert.ToString(record.PP_PROD_MNGMNT_KUBUN));
                record.PP_SPCL_MNGMNT_KUBUN_STRING = UIUtils.GetSpecialManagementKubun(Convert.ToString(record.PP_SPCL_MNGMNT_KUBUN));
                record.PP_STORAGE_MNGMNT_KUBUN_STRING = UIUtils.GetStorageManagementKubun(Convert.ToString(record.PP_STORAGE_MNGMNT_KUBUN));               

                record.PP_MATL_TYPE_CBK_STRING = UIUtils.GetMatlTypeCBK(Convert.ToString(record.PP_MATL_TYPE_CBK));
                record.GP_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

                record.GP_NG_UNIT_TYPE1_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE1));
                record.GP_NG_UNIT_TYPE2_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE2));
                record.GP_NG_UNIT_TYPE3_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE3));

                record.GP_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.GP_DECISION));
                record.HARDNESS_PATTERN1_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                record.HARDNESS_PATTERN2_CODE_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));

            }

            string view = _viewRenderService.RenderToString("DailyProcessAnalysisWithCSV/GeneralProcessDailyProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

       
        //CSV EXPORT

        public IActionResult ExportHardnessTemperingCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHardnessTempering(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetHardnessTemperingSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_HardnessTemperingDailyReport"), addDateInFileName, addTimeInFileName);

            DataTable dataTableWithHeader = SetHeaderForHardnessTempering(gridSearchRequest.CULTURE_NAME);


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

            //var result = _hardnessTemperingService.GetHardnessTemperingOnDailyProcessAnalysisCSV(gridSearchRequest);           
            return Ok(new { IsSuccess = true, message = message });
        }
        
        public IActionResult ExportHeatTreatmentCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHeatTreatment(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            string SQL = _csvSqlString.GetHeatTreatmentSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_HeatTreatmentDailyReport"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForHeatTreatment(gridSearchRequest.CULTURE_NAME);


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
            //var result = _heatTreatmentService.GetHeatTreatmentDetailsOnDailyProcessAnalysisCSV(gridSearchRequest);

            return Ok(new { IsSuccess = true, message = message });


        }
                
        public IActionResult ExportHikinukiBarToBarCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHikinukiBarToBar(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetHikinukiBarToBarSQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_HikinukiBarToBarDailyReport"), addDateInFileName, addTimeInFileName);
            DataTable _dataTableWithHeader = SetHeaderForHikinukiBarToBar(gridSearchRequest.CULTURE_NAME);           

            try
            {
                string PLSqlName = String.Empty;
                string outPutTable = String.Empty;

                message = _allCommonService.ExportCSV(SQL, PLSqlName, outPutTable, fileName, _dataTableWithHeader);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }

            //var result = _hikinukiBarToBarService.GetHikinukiBarToBarDetailsOnDailyProcessAnalysisCSV(gridSearchRequest);          
            return Ok(new { IsSuccess = true, message = message });
        }
                
        public IActionResult ExportHikinukiCoilToBarCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHikinukiCoilToBar(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetHikinukiCoilToBarSQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_HikinukiCoilToBarDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForHikinukiCoilToBar(gridSearchRequest.CULTURE_NAME);


            

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
            //var result = _hikinukiCoilToBarService.GetHikinukiCoilToBarDetailsOnDailyProcessAnalysisCSV(gridSearchRequest);           
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportHikinukiCoilToCoilCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHikinukiCoilToCoil(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetHikinukiCoilToCoilSQLForDailyProcess(searchFields, selectedSortCriteria);


            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_ColdDrawing(CoilToCoil)DailyReport"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForHikinukiCoilToCoil(gridSearchRequest.CULTURE_NAME);


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
            //var result = _coldDrawingWireDrawingService.GetHikinukiCoilToCoilDetailsOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportStraighteningCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaStraightning(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetStraighteningSQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_ProcessROLLStrighening"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForStraightening(gridSearchRequest.CULTURE_NAME);


            

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
            //var result = _straighteningService.GetStraighteningOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportStraightening2CSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaStraightning2(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetStraighteing2SQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_ProcessPRESSStrighening"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForStraightening2(gridSearchRequest.CULTURE_NAME);


            

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
            //var result = _straightening2Service.GetStraightening2OnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportChamfferCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaChamffer(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetChamfferSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lblChamfer24HoureMachineHeader"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForChamffer(gridSearchRequest.CULTURE_NAME);


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
            // var result = _chamfferService.GetChamfferOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportECCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaEC(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetECSQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_ECDailyScreenReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForEC(gridSearchRequest.CULTURE_NAME);


           

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
            //var result = _ecService.GetECOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportStamppingCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaStampping(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetStamppingSQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_StampingDailyReport"), addDateInFileName, addTimeInFileName);

            DataTable dataTableWithHeader = SetHeaderForStampping(gridSearchRequest.CULTURE_NAME);


            

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
            //var result = _stamppingService.GetStamppingOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportQATestHardnessCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaQATestHardness(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetQATestHardnessSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_QualityInspectionHardness"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForQATestHardness(gridSearchRequest.CULTURE_NAME);


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

            //var result = _postHTInspectionService.GetQATestHardnessOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportMechHardnessCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaMechHardness(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetMechHardnessSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_MechHardnessDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForMechHardness(gridSearchRequest.CULTURE_NAME);


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
            //var result = _mIHardnessService.GetMechHardnessOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportMechTensileCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaMechTensile(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetMechTensileSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_MechTensileDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForMechTensile(gridSearchRequest.CULTURE_NAME);


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
            //var result = _tensileService.GetMechTensileOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportFinalInspectionCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaFinalInspection(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetFinalInspectionSQLForDailyProcess(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_FinalInspectionDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForFinalInspection(gridSearchRequest.CULTURE_NAME);


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
            //var result = _finalInspectionService.GetFinalInspectionOnDailyProcessAnalysisCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportGeneralDailyProcessCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaGeneralDailyProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;

            var SQL = _csvSqlString.GetGeneralDailyProcessSQLForDailyProcess(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_AnalysisCSV") + _localization.GetLocalizedHtmlString("lbl_GeneralProcessDailyReport"), addDateInFileName, addTimeInFileName);

            DataTable dataTableWithHeader = SetHeaderForGeneralDailyProcess(gridSearchRequest.CULTURE_NAME);

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

            //var result = _generalDailyProcessService.GetGeneralDailyProcessOnDailyProcessAnalysisCSV(gridSearchRequest);            
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult GetDecision()
        {
            var result = _generalDailyProcessService.GetDecisionList();
            return Ok(new { IsSuccess = true, message = result });
        }

        public IActionResult GetProcConditionKubun()
        {
            var result = _heatTreatmentService.GetProcConditionKubun();
            return Ok(new { IsSuccess = true, message = result });
        }


        private List<SearchField> ComposeSearchCriteriaHeatTreatment(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PATTERN_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.PATTERN_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PATTERN_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_SETUP_CONF_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_SETUP_CONF_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_LOADING_CONF_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_LOADING_CONF_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_OFFLOADED_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP3.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_OFFLOADED_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_OFFLOAD_CONF_BY_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP4.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_OFFLOAD_CONF_BY_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_LABEL_NO_ROLL_MONTH))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.ROLL_MONTH",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_LABEL_NO_ROLL_MONTH.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.COIL_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaHikinukiBarToBar(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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

            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CON.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }

            //Drawing D search
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DW_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DW_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.DIE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DIE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.DIE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DD.DW_DECISION",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaHikinukiCoilToBar(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD ",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.DIE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DIE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.DIE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = " EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_DECISION",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaHikinukiCoilToCoil(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CON.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            //Cold_Drawing_Wire search
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ROLLING_NUMBER))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.ROLLING_NUMBER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.ROLLING_NUMBER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.DIE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DIE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.DIE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_DECISION",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaStraightning(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MACH.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPL.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPL1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_DECISION",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaStraightning2(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MACH.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPL.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPL1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STR.STR_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaChamffer(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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

            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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

            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CH.CH_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaEC(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(gridSearchRequest.PROD_VER)))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_VER)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CON.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }

            //EC search
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.EC_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.EC_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.EC_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(gridSearchRequest.END_OPERATOR)))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.EC_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaStampping(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CON.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            //stampping search
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.METALLIC_PLATE_CODE_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_METALLIC_PLATE_CODE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.METALLIC_PLATE_CODE_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaHardnessTempering(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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

            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.QUENCHING_MACHINE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MACH.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.QUENCHING_MACHINE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.TEMPERING_MACHINE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MACT.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.TEMPERING_MACHINE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.Q_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPH.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.Q_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.Q_FURNACE_IN_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.START_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.Q_FURNACE_IN_START_DATE,
                    SearchText2 = gridSearchRequest.Q_FURNACE_IN_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.Q_FURNACE_OUT_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.END_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.Q_FURNACE_OUT_START_DATE,
                    SearchText2 = gridSearchRequest.Q_FURNACE_OUT_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.T_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPT.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.T_OPERATOR.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.T_FURNACE_IN_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.START_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.T_FURNACE_IN_START_DATE,
                    SearchText2 = gridSearchRequest.T_FURNACE_IN_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.T_FURNACE_OUT_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.END_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.T_FURNACE_OUT_START_DATE,
                    SearchText2 = gridSearchRequest.T_FURNACE_OUT_END_DATE
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "HTQ.INSPECTION_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.OPERATOR_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMPL.EMPLOYEE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.OPERATOR_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTQ.DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaQATestHardness(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING).ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER).ToLower().Trim(),

                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.INT_QLTY_TEST_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.EMPLOYEE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.HT_CONDITION))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.HT_CONDITION",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.HT_CONDITION
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PATTERN_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.PATTERN_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PATTERN_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.INTERNAL_QUALITY_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaMechHardness(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING)
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "MIHARD.HD_TEST_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.EMPLOYEE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "MIHARD.HD_FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MIHARD.HD_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaMechTensile(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING)
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "MITENS.TS_TEST_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.EMPLOYEE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.FURNACE_NO))
            {
                var searchField = new SearchField
                {
                    FieldName = "MITENS.TS_FURNACE_NO",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.FURNACE_NO
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MITENS.TS_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaFinalInspection(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SHAPE 
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
            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING)
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
            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOCA.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.INSP_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.EMPLOYEE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.EMPLOYEE_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.SPARK_INSP_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.OPERATOR_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP3.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.OPERATOR_NAME
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.COMBINED_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaGeneralDailyProcess(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHUUBAN_YEAR||PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.GYOUBAN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.GYOUBAN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.GYOUBAN)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_VER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.VER",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.PROD_VER
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CUST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_NAME_CUST",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_NAME_CUST
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERN.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.PROD_SHAPE 
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

            if (gridSearchRequest.PROD_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_W)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_DIM_OTHER_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_OTHER_STRING)
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

            if (!string.IsNullOrEmpty(gridSearchRequest.LENGTH_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.LENGTH_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.LENGTH_OTHER),

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_TYPE_CBK",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_NAME_MAK",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 =  gridSearchRequest.MATL_SHAPE 
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_DIM_W > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_W",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_W)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_DIM_OTHER))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_DIM_OTHER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_DIM_OTHER
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.MATL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_CONDITION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONDITION1.CONDITION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_CONDITION_CD)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.ACT_ALLOC_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.ACT_ALLOC_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.ACT_ALLOC_START_DATE,
                    SearchText2 = gridSearchRequest.ACT_ALLOC_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROCESS_KUBUN_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PROCK.KUBUN_ID",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_KUBUN_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MACHINE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAC.MACHINE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MACHINE_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.LOCATION_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "LOC.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.LOCATION_NAME
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.BEGINNING_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_START_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.BEGINNING_START_DATE,
                    SearchText2 = gridSearchRequest.BEGINNING_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP1.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.START_OPERATOR
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.END_DATE1,
                    SearchText2 = gridSearchRequest.END_DATE2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.END_OPERATOR))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP2.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.END_OPERATOR
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.DECISION > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_DECISION",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.DECISION)
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private DataTable SetHeaderForHeatTreatment(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Heat Treatment
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PatternNo_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime(lbl_StartTime)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AssembleVerifyBy(Start)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceVerifyByStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PlanOutDateTimeStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceOutDateTimeEnd"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceOperatorEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceVerifyByEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CoilBarRpt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LotNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabel(lbl_RollMonth)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlLableNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PositionInFurnace1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PositionInFurnace2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryRemarks"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            return dt;
        }

        private DataTable SetHeaderForHikinukiBarToBar(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Hikinuki Bar To Bar
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Middle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReDrawing"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim1_XXL"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStatus"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalSurfaceCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CorrectedBars"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DisposalBars"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            return dt;
        }

        private DataTable SetHeaderForHikinukiCoilToBar(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Hikinuki Coil To Bar
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TPTest"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("C"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Manganese"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DrawDim1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DrawDim2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UpperLowerRollerAngle1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UpperLowerRollerAngle2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UpperRoller"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CompressionTonnage"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKanji_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ECThreshold"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionCoilSizeShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DetectionCoilType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_IsOK"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReDrawing"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim1_XXL"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim5"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AEdgeDistortion1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AEdgeDistortion2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AEdgeDistortion3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BEdgeDistortion1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BEdgeDistortion2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BEdgeDistortion3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStatus"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalSurfaceCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CorrectedBars"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DisposalBars"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGBarTotals"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Magnetism"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            return dt;
        }

        private DataTable SetHeaderForHikinukiCoilToCoil(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Hikinuki Coil To Coil
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DiceInspection(Start)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WindingShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Middle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReDrawing"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActDim(Start)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActDim(End)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalSurfaceCondition"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MagneticParticle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DiceInspection(End)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            return dt;
        }

        private DataTable SetHeaderForStraightening(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Straightening
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TPTest"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening4"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening5"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening6"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthBeforeStraightening1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthBeforeStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UpperLowerRollerAngle1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UpperLowerRollerAngle2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UpperRoller"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReservationNumber"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CompressionTonnage"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Restraightening"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening6"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthAfterStraightening1"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthAfterStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AEdgeDistortion1"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AEdgeDistortion2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_AEdgeDistortion3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BEdgeDistortion1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BEdgeDistortion2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BEdgeDistortion3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalSurfaceCondition"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStatus"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            return dt;
        }

        private DataTable SetHeaderForStraightening2(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Straighteing 2
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TPTest"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDBeforeStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthBeforeStraightening1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthBeforeStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReservationNumber"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Restraightening"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening_x-1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening_x-2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening_x-3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening_y-1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening_y-2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDimDAfterStraightening_y-3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthAfterStraightening1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthAfterStraightening2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalSurfaceCondition"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStatus"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForChamffer(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Chamffer
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShape（C-nn)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Re_Chamfering"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForEC(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //EC
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TPTest"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_IsOK"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CoilSize"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CoilShape"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shikii"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeadLengthA"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeadLengthB"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LineSpeed"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReEc"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim1_XXL"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Magnetism"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQty_1"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DefectRate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));

            return dt;
        }

        private DataTable SetHeaderForStampping(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Stampping
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessStamping"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TrialResult"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQty_1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForHardnessTempering(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Hardness Tempering
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QuenchingMachine"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TemperingMachine"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit(Hardness)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InputQunatity"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LiquidType(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Temperature(FurnaceSetting)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Concentration(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Frequency(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Carbon(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Manganese(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chromium(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Molybdenum(FurnaceSetting)"));

            
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TargetHardnessMin(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TargetHardnessMax(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator(Quenching)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime(Quenching)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceOutDateTime(Quenching)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Temperature(Quenching)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Time(Quenching)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUp①(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUp②(quenched)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUp③(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUpCenter(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDown①(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDown②(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDown③(quenched)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDownCenter(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUp①(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUp②(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUp③(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUpCenter(quenched)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDown①(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDown②(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDown③(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDownCenter(quenched)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator(Tempering)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceInDateTime(Tempering)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceOutDatetime(Tempering)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Temperature(Tempering)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Time(Tempering)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUp①(tempered)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUp②(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUp③(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthUpCenter(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDown①(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDown②(tempered)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDown③(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessNorthDownCenter(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUp①(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUp②(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUp③(tempered)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthUpCenter(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDown①(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDown②(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDown③(tempered)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSouthDownCenter(tempered)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMFMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMFMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMTMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMTMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Structure(lbl_OuterRpt)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Structure(lbl_InnerRpt)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDecesion"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            return dt;
        }

        private DataTable SetHeaderForQATestHardness(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Post HT
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcCondition"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PatternNo_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnancePosition1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnancePosition2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnancePositionStep"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DM-F"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMTMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMTMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMTAvg"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DMTMicroscopicMetalStructure"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessCenter"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessD4_1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessD4_2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessD4_3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessD4_4"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessD4_Avg"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForMechHardness(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Mech Hardness
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SplitNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Center(RefernceValue)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4_1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4_2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4_3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4_4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4Average"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4AverageWithCenter"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForMechTensile(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Mech Tensile
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SplitNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_JISTestSpecification"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_GuageDistance_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaxLoadN"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldLoadN"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileElongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FractureDimension_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthWithTs(lbl_ActualResult_L)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(Actual)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationWithEL(lbl_ActualResult_L)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReductionWithRA(lbl_ActualResult_L)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForFinalInspection(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //Final Inspection
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NifudaNumber"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim1Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim2Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim3Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim4Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimDecision"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualRadius1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualRadius2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualRadius3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualRadius4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualRadius5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualRadius6"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ColorDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExtTextureDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SparkDecision"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SparkFIDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SparkFIOperator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CombinedDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForGeneralDailyProcess(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VER"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceiverShortName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InternalNameMaster"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimD_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimW_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDimOther_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdDimTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthText"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMinText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdLengthTolMaxText"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrInnerDim"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrOuterDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HdrWindingDir"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ConditionMaster"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CornerRadiusKatakana_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureTol"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend/FureSpan"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistAngle"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TwistSpan"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitRangeMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalTexture"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Display_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo6"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo7"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo8"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo9"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo10"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo11"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo12"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessNo13"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductTestStandard"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrength(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMin"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationMax"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Elongation(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStandard_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Hardness(MIN-MAX)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessStdRange"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HrdnessActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildPointMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldPoint(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(MIN-MAX)"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PressMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Reduction(lbl_ActualPattern)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb1Max"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb2Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decarb3Max"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMinMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMin"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactMax"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpactActual"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StampGrade"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HeatTreatCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCoilBarKW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimDD"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimW"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialLength_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanType"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StorageMgmtKbn"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_KakouiraiWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DateOfIssue"));

            //General Daily Process
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessKubun_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_StartOperator1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndDateTime"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_EndOperator1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnits1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnit1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGWeight1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnits2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnit2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGWeight2"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnits3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnit3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGWeight3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo4"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyReportSupplement"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NextProcess"));

            return dt;
        }
    }



}
