using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using System.Linq;
using TSWebApplication.Domain.Resources;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel;
using System.Data;
using Microsoft.AspNetCore.Http;
using TSWebApplication.Application.IServices;
using TSWebApplication.Application.Services;
using Microsoft.Extensions.Hosting;
//using NPOI.OpenXmlFormats.Spreadsheet;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class FactoryDailyWorkStatusSearchProcessWiseController : Controller
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
        private readonly IHeatTreatmentRepository _heatTreatmentRepository;
        private readonly IAllCommonService _allCommonService;
        private readonly CsvSqlString _csvSqlString;
        public FactoryDailyWorkStatusSearchProcessWiseController(LocalizationServices localization, IViewRenderService viewRenderService, IHardnessTemperingService hardnessTemperingService, IHeatTreatmentService heatTreatmentService,
                                                     IHikinukiBarToBarService hikinukiBarToBarService, IHikinukiCoilToBarService hikinukiCoilToBarService, IGeneralDailyProcessService generalDailyProcessService, IColdDrawingWireDrawingService coldDrawingWireDrawingService,
                                                     IStraighteningService straighteningService, IStraightening2Service straightening2Service, IChamfferService chamfferService, IECService eCService, IStamppingService stamppingService, IPostHTInspectionService postHTInspectionService,
                                                     IMIHardnessService mIHardnessService, ITensileService tensileService, IFinalInspectionService finalInspectionService,IHeatTreatmentRepository heatRepository, IAllCommonService commonService, CsvSqlString csvSqlString)
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
            _heatTreatmentRepository = heatRepository;
            _allCommonService = commonService;
            _csvSqlString = csvSqlString;
        }
        string cultureName = CultureInfo.CurrentCulture.Name;
        public IActionResult FactoryDailyWorkStatusSearchProcessWise()
        {

            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
       
        public IActionResult HeatTreatmentGrid([FromBody] List<HeatTreatMentDetails> details)
        {
             foreach (HeatTreatMentDetails record in details)
            {
                record.HT_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

                record.PROC_CONDITION_STRING = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/HeatTreatmentGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult CombinedDailyReportGrid([FromBody] List<HikinukiDetails> details)
        {
            foreach (HikinukiDetails record in details)
            {
                record.DW_BEND_STATUS_DECODE = record.DW_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS)) : string.Empty;
                record.DW_EXT_TEXTURE_STATUS_DECODE = record.DW_EXT_TEXTURE_STATUS != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.DW_EXT_TEXTURE_STATUS)) : string.Empty;
                record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                record.DW_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.DW_UNIT_TYPE));
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

                string defectBars = string.Empty;
                if (record.DW_NG_BARS1 > 0)
                {
                    defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.DW_NG_DETAILS1_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + " " + record.DW_NG_BARS1 + _localization.GetLocalizedHtmlString("lbl_Hone");
                }
                if (record.DW_NG_BARS2 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.DW_NG_DETAILS2_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.DW_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.DW_NG_DETAILS2_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.DW_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (record.DW_NG_BARS3 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.DW_NG_DETAILS3_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.DW_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.DW_NG_DETAILS3_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.DW_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (!string.IsNullOrEmpty(record.DW_REMARKS))
                {
                    record.DW_REMARKS = record.DW_REMARKS + "<br>" + defectBars;
                }
                else
                {
                    record.DW_REMARKS = defectBars;
                }
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/CombinedDailyReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult StraighteningDailyReportGrid([FromBody] List<StraighteningDetails> details)
        {
            foreach (StraighteningDetails record in details)
            {
                if (record.STR_DECISION != null)
                {
                    record.STR_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                    record.STR_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                }
                string defectBars = string.Empty;
               
                if (record.STR_NG_BARS1 > 0)
                {
                    defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.STR_NG_DETAILS1_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + " " + record.STR_NG_BARS1 + _localization.GetLocalizedHtmlString("lbl_Hone");
                }
                if (record.STR_NG_BARS2 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.STR_NG_DETAILS2_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.STR_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.STR_NG_DETAILS2_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.STR_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (record.STR_NG_BARS3 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.STR_NG_DETAILS3_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.STR_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.STR_NG_DETAILS3_DECODE + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.STR_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (!string.IsNullOrEmpty(record.STR_REMARKS))
                {
                    record.STR_REMARKS = record.STR_REMARKS + "<br>" + defectBars;
                }
                else
                {
                    record.STR_REMARKS = defectBars;
                }
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/StraighteningDailyReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult ChamferDailyReportGrid([FromBody] List<ChamferDetails> details)
        {
            foreach (ChamferDetails record in details)
            {
                record.CH_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.CH_DECISION));
                record.CH_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/ChamferDailyReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult ECDailyReportGrid([FromBody] List<ECDetails> details)
        {
            foreach (ECDetails record in details)
            {                
                    record.EC_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.EC_DECISION));
                    record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                    record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));

            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/ECDailyReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult StamppingDailyReportGrid([FromBody] List<StamppingDetails> details)
        {
            foreach (StamppingDetails record in details)
            {
                record.ST_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.ST_DECISION));
                record.ST_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                //record.STAMP_DETAILS = UIUtils.GetStampbykubun(record.STAMP_DETAILS);
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));

                string defectBars = string.Empty;
                if (record.ST_NG_BARS1 > 0)
                {
                    defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.ST_NG_DETAILS1_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + " " + record.ST_NG_BARS1 + _localization.GetLocalizedHtmlString("lbl_Hone");
                }
                if (record.ST_NG_BARS2 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.ST_NG_DETAILS2_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.ST_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.ST_NG_DETAILS2_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.ST_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (record.ST_NG_BARS3 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.ST_NG_DETAILS3_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.ST_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.ST_NG_DETAILS3_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.ST_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (!string.IsNullOrEmpty(record.ST_REMARKS))
                {
                    record.ST_REMARKS = record.ST_REMARKS + "<br>" + defectBars;
                }
                else
                {
                    record.ST_REMARKS = defectBars;
                }
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/StamppingDailyReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult QATestHardnessGrid([FromBody] List<PostHTDetails> details)
        {
            foreach (PostHTDetails record in details)
            {
                //record.PP_HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));
                record.INT_QLTY_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.INTERNAL_QUALITY_DECISION));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/QATestHardnessGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult MechHardnessGrid([FromBody] List<MIHardnessDetails> details)
        {
            foreach (MIHardnessDetails record in details)
            {               
                    record.HD_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.HD_DECISION));
                    record.HD_PATTERN_ACT_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HD_PATTERN_ACT));                
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/MechHardnessGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult FinalIGrid([FromBody] List<FinalInspectionDetails> details)
        {
            foreach (FinalInspectionDetails record in details)
            {               
                record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.FI_DIM_DECISION_STRING = UIUtils.GetDimDecisionKubunValue(Convert.ToString(record.FI_DIM_DECISION));
                record.FI_LENGTH_DECISION_STRING = UIUtils.GetLengthDecisionKubunValue(Convert.ToString(record.FI_LENGTH_DECISION));
                record.FI_COLOR_DECISION_STRING = UIUtils.GetColorDecisionKubunValue(Convert.ToString(record.FI_COLOR_DECISION));
                record.FI_CHAMFERING_DECISION_STRING = UIUtils.GetChamferingDecisionKubunValue(Convert.ToString(record.FI_CHAMFERING_DECISION));
                record.FI_EXT_TEXTURE_DECISION_STRING = UIUtils.GetExtTextureDecisionKubunValue(Convert.ToString(record.FI_EXT_TEXTURE_DECISION));
                record.FI_SPARK_DECISION_STRING = UIUtils.GetSparkDecisionKubunValue(Convert.ToString(record.FI_SPARK_DECISION));
                record.COMBINED_DECISION_VALUE = UIUtils.GetResultKubunValue(Convert.ToString(record.COMBINED_DECISION));
                               
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/FinalIGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult MechTensileGrid([FromBody] List<TensileDetails> details)
        {
            foreach (TensileDetails record in details)
            {
                record.TS_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.TS_DECISION));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/MechTensileGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult WireDrawingReportGrid([FromBody] List<ColdDrawingWireDrawingDetails> details)
        {
            foreach (ColdDrawingWireDrawingDetails record in details)
            {
                record.DW_EXT_TEXTURE_STATUS_DECODE = record.DW_EXT_TEXTURE_STATUS != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.DW_EXT_TEXTURE_STATUS)) : string.Empty;
                record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                record.DW_WINDING_SHAPE1 = record.DW_WINDING_SHAPE != null ? UIUtils.GetColdDrawingWindingShape(Convert.ToString(record.DW_WINDING_SHAPE)) : string.Empty;
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/WireDrawingReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult DrawingBarToBarGrid([FromBody] List<HikinukiBarToBarDetails> details)
        {
            foreach (HikinukiBarToBarDetails record in details)
            {
                record.DW_BEND_STATUS_DECODE = record.DW_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.DW_BEND_STATUS)) : string.Empty;
                record.DW_EXT_TEXTURE_STATUS_DECODE = record.DW_EXT_TEXTURE_STATUS != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.DW_EXT_TEXTURE_STATUS)) : string.Empty;
                record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                record.DW_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DW_DECISION));
                string defectBars = string.Empty;
                
                    if (record.DW_NG_BARS1 > 0)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.DW_NG_DETAILS1_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + " " + record.DW_NG_BARS1 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    if (record.DW_NG_BARS2 > 0)
                    {
                        if (defectBars == string.Empty)
                        {
                            defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.DW_NG_DETAILS2_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.DW_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                        }else
                        {
                            defectBars = defectBars + " / " + record.DW_NG_DETAILS2_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.DW_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                        }
                        
                    }
                    if (record.DW_NG_BARS3 > 0)
                    {
                        if (defectBars == string.Empty)
                        {
                            defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.DW_NG_DETAILS3_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.DW_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                        }
                        else
                        {
                            defectBars = defectBars + " / " + record.DW_NG_DETAILS3_STRING + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.DW_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                        }

                    }
                if (!string.IsNullOrEmpty(record.DW_REMARKS))
                {
                    record.DW_REMARKS = record.DW_REMARKS + "<br>" + defectBars;
                }
                else
                {
                    record.DW_REMARKS = defectBars;
                }
                //record.HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.HT_CONDITION));

                //lbl_DefectDetails

            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/DrawingBarToBarGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult Straightening2Grid([FromBody] List<Straightening2Details> details)
        {
            foreach (Straightening2Details record in details)
            {               
                    record.STR_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.STR_DECISION));
                    record.STR_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.STR_UNIT_TYPE));
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                record.STR_BEND_STATUS_DECODE = record.STR_BEND_STATUS != null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.STR_BEND_STATUS)) : string.Empty;
                    record.STR_EXT_TEXTURE_STATUS_DECODE = record.STR_EXT_TEXTURE != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.STR_EXT_TEXTURE)) : string.Empty;
                //record.PP_HT_CONDITION = UIUtils.GetProcConditionKubunValue(Convert.ToString(record.PP_HT_CONDITION));
                string defectBars = string.Empty;

                if (record.STR_NG_BARS1 > 0)
                {
                    defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.PROC_DEFECT_SHORT1 + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + " " + record.STR_NG_BARS1 + _localization.GetLocalizedHtmlString("lbl_Hone");
                }
                if (record.STR_NG_BARS2 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.PROC_DEFECT_SHORT2 + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.STR_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.PROC_DEFECT_SHORT2 + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + " " + record.STR_NG_BARS2 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (record.STR_NG_BARS3 > 0)
                {
                    if (defectBars == string.Empty)
                    {
                        defectBars = _localization.GetLocalizedHtmlString("lbl_DefectDetails") + record.PROC_DEFECT_SHORT3 + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.STR_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }
                    else
                    {
                        defectBars = defectBars + " / " + record.PROC_DEFECT_SHORT3 + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + " " + record.STR_NG_BARS3 + _localization.GetLocalizedHtmlString("lbl_Hone");
                    }

                }
                if (!string.IsNullOrEmpty(record.STR_REMARKS))
                {
                    record.STR_REMARKS = record.STR_REMARKS + "<br>" + defectBars;
                }
                else
                {
                    record.STR_REMARKS = defectBars;
                }

            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/Straightening2Grid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult HardnessTemperingGrid([FromBody] List<HardeningTemperingDetails> details)
        {
            foreach (HardeningTemperingDetails record in details)
            {
                record.DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));
                record.PP_UNIT_TYPE_STRING = UIUtils.GetMaterialUnitKubunValue(Convert.ToString(record.PP_UNIT));
                record.UNIT_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT));
                record.WATER_OIL_STRING = UIUtils.GetHardnessOilTypeKubunValue(Convert.ToString(record.WATER_OIL_TYPE));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/HardnessTemperingGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GeneralProcessGrid([FromBody] List<GeneralDailyProcessDetails> details)
        {
            foreach (GeneralDailyProcessDetails record in details)
            {
                record.GP_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.GP_DECISION));
                record.GP_UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.GP_NG_UNIT_TYPE1_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE1));
                record.GP_NG_UNIT_TYPE2_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE2));
                record.GP_NG_UNIT_TYPE3_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE3));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/GeneralProcessGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult TransferReportGrid([FromBody] List<WIPTensouDetails> details)
        {
            foreach (WIPTensouDetails record in details)
            {
                //record.GP_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.GP_DECISION));
                record.UNIT_TYPE_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                //record.GP_NG_UNIT_TYPE1_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE1));
                //record.GP_NG_UNIT_TYPE2_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE2));
                //record.GP_NG_UNIT_TYPE3_STRING = UIUtils.GetProductUnitKubunValue(Convert.ToString(record.GP_NG_UNIT_TYPE3));
            }
            string view = _viewRenderService.RenderToString("FactoryDailyWorkStatusSearchProcessWise/TransferReportGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult InitializeSortPopup(string activeTabID)
        {
            List<TSSortField> sortFields = new List<TSSortField>();
            List<TSSortField> sortFields1 = new List<TSSortField>();

            string custSortOrder = "CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME";
            if (activeTabID == "titleHeatTreatment")
            {

                sortFields = new List<TSSortField> {
                                        
                                          new TSSortField {
                                            FieldName="HT_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceOutDate"),
                                              IsDefaultSort=true,
                                           
                                        },

                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField
                                       {
                                           FieldName = "FURNACE_NO",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                           IsDefaultSort = true,
                                       },


                                        new TSSortField
                                       {
                                           FieldName = "HT.HT_CONDITION",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcCondition"),
                                           IsDefaultSort = true,
                                       },

                                         new TSSortField
                                       {
                                           FieldName = "HT.PATTERN_NO",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_PatternNo"),
                                           IsDefaultSort = true,
                                       },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PROC_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessLocation"),

                                        },

                                        new TSSortField {
                                            FieldName="OFFLOAD_CONF_BY_EMPLOYEE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceVerifyBy"),

                                        },
                                        new TSSortField{
                                            FieldName = "MAC.MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }

                                        };

            }


            if (activeTabID == "titleDrawingCoilToBar")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="DW_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                           
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),                                            
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "DW_TABA_NO_1,DW_TABA_NO_2,DW_TABA_NO_3,DW_TABA_NO_4",                                           
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                            IsDefaultSort=true,
                                        },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessLocation"),

                                        },

                                        new TSSortField {
                                            FieldName="DW_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "DW_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }                                         
                                        };

            }
            if (activeTabID == "titleStraightening1")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="STR_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName = "STR_TABA_NO_1,STR_TABA_NO_2,STR_TABA_NO_3,STR_TABA_NO_4",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessLocation"),

                                        },

                                        new TSSortField {
                                            FieldName="STR_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "MAC.MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                       
                                        };

            }

            if (activeTabID == "titleChamfer")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="CH_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "CH_TABA_NO_1,CH_TABA_NO_2,CH_TABA_NO_3,CH_TABA_NO_4",                                           
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),
                                             IsDefaultSort=true,
                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PROCESS_LOCATION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessLocation"),

                                        },

                                        new TSSortField {
                                            FieldName="CH_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "CH_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        
                                        };

            }

            if (activeTabID == "titleEC")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="EC_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "EC_TABA_NO",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROCESS_LOCATION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessLocation"),

                                        },

                                        new TSSortField {
                                            FieldName="EC_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "EC_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        
                                        };

            }

            if (activeTabID == "titleStampping")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="ST_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "ST_TABA_NO",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessLocation"),

                                        },

                                        new TSSortField {
                                            FieldName="ST_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "ST_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                         
                                        };

            }

            if (activeTabID == "titleQATestHardness")
            {


                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="INT_QLTY_TEST_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PROD_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="INSPECTION_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionLoc"),

                                        },

                                        new TSSortField {
                                            FieldName="EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Inspector"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "IQ_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },

                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        };

            }

            if (activeTabID == "titleMechHardness")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="HD_TEST_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionLoc"),

                                        },

                                        new TSSortField {
                                            FieldName=" EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Inspector"),

                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        };

            }

            if (activeTabID == "titleMechTensile")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="TS_TEST_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                              IsDefaultSort=true,
                                           
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="INSPECTION_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionLoc"),

                                        },

                                        new TSSortField {
                                            FieldName="EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Inspector"),

                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        };

            }
            if (activeTabID == "titleFinalInspection")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="INSPECTED_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                              IsDefaultSort=true,
                                           
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName = "FI_TABA_NO",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PROC_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionLoc"),

                                        },

                                        new TSSortField {
                                            FieldName="EMPLOYEE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Inspector"),

                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                       
                                        };
            }

            if (activeTabID == "titleDrawingBarToBar")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="DW_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "DW_TABA_NO_",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="DW_LOCATION",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                        new TSSortField {
                                            FieldName="DW_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "MACH.MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        
                                        };

            }

            if (activeTabID == "titleWireDrawing")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="DW_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                           
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName = "DW_TABA_NO_1,DW_TABA_NO_2,DW_TABA_NO_3,DW_TABA_NO_4",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                        new TSSortField {
                                            FieldName="DW_END_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "DW_MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        
                                        };

            }
            if (activeTabID == "titleHardnessTempering")
            {

                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="INSPECTION_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionDate"),
                                            IsDefaultSort=true,
                                           
                                        },
                                        new TSSortField {
                                            FieldName="HT.END_DATE_TEMPERING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TemperingEndDate"),
                                            IsDefaultSort=true,
                                           
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField
                                       {
                                           FieldName = "FURNACE_NO",
                                           DisplayName = _localization.GetLocalizedHtmlString("lbl_FurnaceNum"),
                                           IsDefaultSort = true,
                                       },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InspectionLoc"),

                                        },

                                        new TSSortField {
                                            FieldName="INSPECTED_OPERATOR_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Inspector"),

                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        };

            }
            if (activeTabID == "titleStraightening2")
            {

                




                sortFields = new List<TSSortField> {


                                        new TSSortField {
                                            FieldName="STR_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "STR_TABA_NO_1,STR_TABA_NO_2,STR_TABA_NO_3,STR_TABA_NO_4",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="PP_LOCATION_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                        new TSSortField {
                                            FieldName="PP_EMPLOYEE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "MACH.MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        
                                        };
            }
            if (activeTabID == "titleGeneralDailyProcess")
            {

                sortFields = new List<TSSortField> {

                                new TSSortField {
                                            FieldName="GP.PROCESS_KUBUN_CD",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProcessKubun_L"),
                                              IsDefaultSort=true,
                                            
                                        },
                                        new TSSortField {
                                            FieldName="GP_END_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_OperationDay"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,
                                            
                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },                                        

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "GP_TABA_NO_1,GP_TABA_NO_2,GP_TABA_NO_3,GP_TABA_NO_4",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },


                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="LOC.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Location"),

                                        },

                                        new TSSortField {
                                            FieldName="EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Operator"),

                                        },
                                        new TSSortField
                                        {
                                            FieldName = "MAC.MACHINE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Machine_L"),
                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }
                                        
                                        };

            }
            if (activeTabID == "titleTransfer")
            {

                sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName="TRANSFER_DATE_STRING",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TransferDate"),
                                              IsDefaultSort=true,
                                            
                                        },
                                         new TSSortField {
                                            FieldName=custSortOrder,
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                              IsDefaultSort=true,

                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                            FieldName="PP_PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                        },
                                        new TSSortField {
                                             FieldName = "PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                        },
                                         new TSSortField {
                                            FieldName="PP.PROD_LENGTH,PP.LENGTH_OTHER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                        },

                                       new TSSortField {
                                            FieldName = "PP_CHUUBAN",
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                             IsDefaultSort=true,
                                        },
                                       new TSSortField {
                                            FieldName = "TABA_NO_1,TABA_NO_2,TABA_NO_3,TABA_NO_4",
                                            IsDefaultSort=true,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_TabaNo"),

                                        },

                                        new TSSortField {
                                            FieldName="PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"),

                                        },
                                        new TSSortField {
                                            FieldName="LOC_TO.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Location"),

                                        },
                                        new TSSortField {
                                            FieldName = "INTERNAL_NAME",
                                            IsDefaultSort=false,
                                            DisplayName =_localization.GetLocalizedHtmlString("lbl_InternalName"),

                                        }                                       

                                        };

            }
            sortFields = sortFields1.Concat(sortFields).ToList();
            return Ok(new { IsSuccess = true, message = sortFields });

        }
        public IActionResult ExportHeatTreatmentCSV([FromBody] GridSearchFields gridSearchRequest)//ExportHeatTreatmentCSV
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHeatTreatment(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            // CsvSqlString _csvSqlString = new CsvSqlString();
            string cultureNames = gridSearchRequest.CULTURE_NAME;
            string SQL = _csvSqlString.GetHeatTreatmentCSVSQLForFactory(searchFields, selectedSortCriteria, cultureNames);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") +_localization.GetLocalizedHtmlString("lbl_HeatTreatmentDailyReport"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForHeatTreatment(gridSearchRequest.CULTURE_NAME);
            //var result = _heatTreatmentService.GetHeatTreatmentFactoryDailyWorkStatusCSV(gridSearchRequest);
            // allCommonService.ExportCSV(SQL, PLSqlName, outPutTable, fileName, dataTableWithHeader);
          

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

        public IActionResult ExportDrawingBarToBarCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaDrawingBarToBar(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetHikinukiBarToBarSQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_BarToBarDailyReport"), addDateInFileName, addTimeInFileName);
            DataTable _dataTableWithHeader = SetHeaderForDrawingBarToBar(gridSearchRequest.CULTURE_NAME);

            

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

            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportCombinedDailyReportCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaCombinedDailyReport(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetCombinedDailyReportSQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_HikinukiCoilToBarDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForCombinedDailyReport(gridSearchRequest.CULTURE_NAME);


            

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

        public IActionResult ExportColdDrawingWireDrawingCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaColdDrawingWireDrawing(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetColdDrawingWireDrawingSQLForFactory(searchFields, selectedSortCriteria, cultureName);


            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_drawing24HoureMachineHeader"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForColdDrawingWireDrawing(gridSearchRequest.CULTURE_NAME);


            //string cultureName = gridSearchRequest.CULTURE_NAME;

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

            //var result = _coldDrawingWireDrawingService.GetColdDrawingWireDrawingDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportStraighteningCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaStraighteing(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetStraighteningSQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_ProcessROLLStrighening"), addDateInFileName, addTimeInFileName);


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
                      
            //var result = _straighteningService.GetStraighteningDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportStraightening2CSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaStraightening2(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetStraighteing2SQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_ProcessPRESSStrighening"), addDateInFileName, addTimeInFileName);


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
            //var result = _straightening2Service.GetStraightening2DetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportChamfferCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaChamffer(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            string SQL = _csvSqlString.GetChamfferSQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lblChamfer24HoureMachineHeader"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForChamffer(gridSearchRequest.CULTURE_NAME);


            

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
            //var result = _chamfferService.GetChamferDailyReportOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportECCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
			List<SearchField> searchFields = ComposeSearchCriteriaEC(gridSearchRequest);
			string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetECSQLForFactory(searchFields, selectedSortCriteria, cultureName);

			bool addDateInFileName = true;
			bool addTimeInFileName = false;

			string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_ECDailyReport"), addDateInFileName, addTimeInFileName);


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

			//var result = _ecService.GetECDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
			return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportStamppingCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaStampping(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetStamppingSQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_StampingDailyReport"), addDateInFileName, addTimeInFileName);

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
            //var result = _stamppingService.GetStamppingDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportQATestHardnessCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaQATestHardness(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetQATestHardnessSQLForFactory(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_QualityInspectionHardness"), addDateInFileName, addTimeInFileName);


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

            //var result = _postHTInspectionService.GetQATestHardnessOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportHardnessTemperingCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaHardnessTempering(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetHardnessTemperingSQLForFactory(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_HardnessTempering"), addDateInFileName, addTimeInFileName);

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
            //var result = _hardnessTemperingService.GetHardnessTemperingDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportMechHardnessCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaMechHardness(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetMechHardnessSQLForFactory(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_MechHardnessDailyReport"), addDateInFileName, addTimeInFileName);


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
            // var result = _mIHardnessService.GetMechHardnessDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportMechTensileCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaMechTensile(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetMechTensileSQLForFactory(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_MechTensileDailyReport"), addDateInFileName, addTimeInFileName);


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
            //var result = _tensileService.GetMECHTensileOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportFinalInspectionCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaFinalInspection(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);

            var SQL = _csvSqlString.GetFinalInspectionSQLForFactory(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_FinalInspectionDailyReport"), addDateInFileName, addTimeInFileName);


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
            //var result = _finalInspectionService.GetFinalInspectionDataOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        public IActionResult ExportGeneralProcessCSV([FromBody] GridSearchFields gridSearchRequest)
        {
			var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaGeneralProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetGeneralDailyProcessSQLForFactory(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_GeneralProcessDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForGeneralProcess(gridSearchRequest.CULTURE_NAME);




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

            //var result = _generalDailyProcessService.GetGeneralDailyProcessReportDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }

        public IActionResult ExportTransferCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteriaTransferReport(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string cultureName = gridSearchRequest.CULTURE_NAME;
            var SQL = _csvSqlString.GetTransferReportDailySQL(searchFields, selectedSortCriteria, cultureName);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;

            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_DailyCSV") + _localization.GetLocalizedHtmlString("lbl_TransferDailyReport"), addDateInFileName, addTimeInFileName);


            DataTable dataTableWithHeader = SetHeaderForTransferReport(gridSearchRequest.CULTURE_NAME);




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

            //var result = _generalDailyProcessService.GetGeneralDailyProcessReportDetailsOnFactoryDailyWorkStatusCSV(gridSearchRequest);
            return Ok(new { IsSuccess = true, message = message });
        }
        private List<SearchField> ComposeSearchCriteriaHeatTreatment(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CUSTOMER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.OFFLOAD_CONF_BY",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE 
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
                    SearchType = SearchType.Text,
                    SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROD_DIM_D_MAX == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
            if (gridSearchRequest.PROD_LENGTH_MAX == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MAX > 0 && gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                };
                searchFields.Add(searchField);
            }
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HT.HT_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "HT.HT_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE2
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                var searchField = new SearchField
                {
                    FieldName = "HT.HT_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaDrawingBarToBar(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DRAWINGD.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DRAWINGD.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CUSTOMER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DRAWINGD.DW_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
                    FieldName = "INTE.INTERNAL_NAME ",
                    SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = " PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE 
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
            if (gridSearchRequest.PROD_DIM_D_MAX == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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

            if (gridSearchRequest.PROD_LENGTH_MAX == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DRAWINGD.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DRAWINGD.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE2
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                var searchField = new SearchField
                {
                    FieldName = "DRAWINGD.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaCombinedDailyReport(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CUSTOMER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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


            if (gridSearchRequest.PROD_DIM_D_MAX == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
            if (gridSearchRequest.PROD_LENGTH_MAX == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE 
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
            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DW.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DW.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE2
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }

        private List<SearchField> ComposeSearchCriteriaColdDrawingWireDrawing(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CUSTOMER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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

            if (gridSearchRequest.PROD_DIM_D_MAX == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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

            if (gridSearchRequest.PROD_LENGTH_MAX == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE
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

            if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DW.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE1
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
            {
                if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
                {
                    var searchField = new SearchField
                    {
                        FieldName = "DW.DW_END_DATE",
                        SearchType = SearchType.DateTime,
                        SearchText1 = gridSearchRequest.INSPECTION_DATE2
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                var searchField = new SearchField
                {
                    FieldName = "DW.DW_END_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                };
                searchFields.Add(searchField);
            }



            return searchFields;
        }

		private List<SearchField> ComposeSearchCriteriaStraighteing(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			if (gridSearchRequest.MACHINE_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "STRAI.MACHINE_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "STRAI.LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STRAI.STR_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
					FieldName = "INTE.INTERNAL_NAME ",
					SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = " PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE
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
			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "STRAI.STR_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "STRAI.STR_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "STRAI.STR_END_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}

			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaStraightening2(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			if (gridSearchRequest.MACHINE_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "STR2.MACHINE_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "STR2.LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "STR2.STR_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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

			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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

			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

			if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE 
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "STR2.STR_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "STR2.STR_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "STR2.STR_END_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}


			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaChamffer(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			if (gridSearchRequest.MACHINE_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "CHA.MACHINE_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "CHA.LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CHA.CH_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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


			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

			if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE 
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "CHA.CH_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "CHA.CH_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "CHA.CH_END_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}


			return searchFields;
		}


		private List<SearchField> ComposeSearchCriteriaEC(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			if (gridSearchRequest.MACHINE_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "EC.MACHINE_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "EC.LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "EC.EC_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
					FieldName = "INTE.INTERNAL_NAME ",
					SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = " PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE
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
			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "EC.EC_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "EC.EC_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "EC.EC_END_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}

			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaStampping(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			if (gridSearchRequest.MACHINE_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "ST.MACHINE_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "ST.LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ST.ST_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
			if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE 
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
					SearchText1 = "'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO1) + "'"
				};
				searchFields.Add(searchField);
			}
			if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SPEC_NO2",
					SearchType = SearchType.Number,
					SearchText1 = "'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO2) + "'"
				};
				searchFields.Add(searchField);
			}
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "ST.ST_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "ST.ST_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "ST.ST_END_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}

			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaHardnessTempering(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
            var machineOrGroup = new List<SearchField>();
            var dateOrGroup = new List<SearchField>();
            var operatorOrGroup = new List<SearchField>();
            if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "NVL(HTQ.LOCATION_CD, HT.LOCATION_CD)",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                machineOrGroup.Add(new SearchField
                {
                    FieldName = "HT.HARDENING_MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                });

                machineOrGroup.Add(new SearchField
                {
                    FieldName = "HT.TEMPERING_MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                });
                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", machineOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
                    SearchType = SearchType.Custom
                });
            }
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                operatorOrGroup.Add(new SearchField
                {
                    FieldName = "HT.OPERATOR_CD_HARDENING",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                });

                operatorOrGroup.Add(new SearchField
                {
                    FieldName = "HT.OPERATOR_CD_TEMPERING",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                });
                operatorOrGroup.Add(new SearchField
                {
                    FieldName = "HTQ.INSPECTED_BY_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
                });
                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", operatorOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
                    SearchType = SearchType.Custom
                });
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
			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
			if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE 
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
					SearchText1 = "'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO1) + "'"
				};
				searchFields.Add(searchField);
			}
			if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SPEC_NO2",
					SearchType = SearchType.Number,
					SearchText1 = "'" + Convert.ToString(gridSearchRequest.PROD_SPEC_NO2) + "'"
				};
				searchFields.Add(searchField);
			}
            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.START_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.END_DATE_HARDENING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.START_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HT.END_DATE_TEMPERING",
                    SearchType = SearchType.DateTime,
                    SearchText1 = gridSearchRequest.INSPECTION_DATE1,
                    SearchText2 = gridSearchRequest.INSPECTION_DATE2
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "HTQ.INSPECTION_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE1).ToString("yyyy/MM/dd"),
                    SearchText2 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE2).ToString("yyyy/MM/dd")
                });

                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", dateOrGroup.Select(f =>
                    {
                        // ✅ FIX: DATE FIELD HANDLING
                        if (f.FieldName == "HTQ.INSPECTION_DATE")
                        {
                            return $"{f.FieldName} BETWEEN " +
                                   $"TO_DATE('{f.SearchText1} 00:00','yyyy/MM/dd hh24:mi') " +
                                   $"AND TO_DATE('{f.SearchText2} 23:59','yyyy/MM/dd hh24:mi')";
                        }

                        // DATETIME FIELDS
                        return $"{f.FieldName} BETWEEN " +
                               $"TO_DATE('{f.SearchText1}','yyyy/MM/dd hh24:mi') " +
                               $"AND TO_DATE('{f.SearchText2}','yyyy/MM/dd hh24:mi')";
                    })
                    ) + ")",
                    SearchType = SearchType.Custom
                });
            }
            

            return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaQATestHardness(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
            if (gridSearchRequest.MACHINE_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.MACHINE_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "IQ.LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "IQ.INT_QLTY_TEST_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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


			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

			if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "IQ.INT_QLTY_TEST_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "IQ.INT_QLTY_TEST_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "IQ.INT_QLTY_TEST_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}


			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaMechHardness(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "MI.INPUT_LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MI.HD_TEST_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
					FieldName = "INTE.INTERNAL_NAME ",
					SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE
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
			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_LENGTH ",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
					SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
				};
				searchFields.Add(searchField);
			}

			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "MI.HD_TEST_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "MI.HD_TEST_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "MI.HD_TEST_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}

			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaMechTensile(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "TENSILE.INPUT_LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "TENSILE.TS_TEST_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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

			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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

			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "TENSILE.TS_TEST_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "TENSILE.TS_TEST_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "TENSILE.TS_TEST_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}


			return searchFields;
		}

		private List<SearchField> ComposeSearchCriteriaFinalInspection(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "FI.INPUT_LOCATION_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "FI.INSP_BY",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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
					FieldName = "INTE.INTERNAL_NAME ",
					SearchType = SearchType.Text,
                    SearchCondition = SearchCondition.Equal.ToString(),
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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
            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = " PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE 
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
			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "FI.INSP_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "FI.INSP_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
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

			return searchFields;
		}
		private List<SearchField> ComposeSearchCriteriaGeneralProcess(GridSearchFields gridSearchRequest)
		{
			var searchFields = new List<SearchField>();
			if (gridSearchRequest.MACHINE_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "GP.MACHINE_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.MACHINE_CD)
				};
				searchFields.Add(searchField);
			}
			if (gridSearchRequest.LOCATION_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "NVL(GP.LOCATION_CD,GP.INPUT_LOCATION_CD)",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.PROCESS_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.PROCESS_KUBUN_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
			{
				var searchField = new SearchField
				{
					FieldName = "PP.CUSTOMER_CD",
					SearchType = SearchType.Number,
					SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
				};
				searchFields.Add(searchField);
			}
            if (gridSearchRequest.END_OPERATOR_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "GP.GP_END_OPERATOR_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.END_OPERATOR_CD)
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


			if (gridSearchRequest.PROD_DIM_D_MAX == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN == null)
			{
				if (gridSearchRequest.PROD_DIM_D_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_DIM_D",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
			if (gridSearchRequest.PROD_LENGTH_MAX == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MIN > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN == null)
			{
				if (gridSearchRequest.PROD_LENGTH_MAX > 0)
				{
					var searchField = new SearchField
					{
						FieldName = "PP.PROD_LENGTH",
						SearchType = SearchType.Number,
						SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
					};
					searchFields.Add(searchField);
				}
			}
			else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

			if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
			{
				var searchField = new SearchField
				{
					FieldName = "PP.PROD_SHAPE",
					SearchType = SearchType.Text,
					SearchText1 = gridSearchRequest.SHAPE
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
			if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
				{
					var searchField = new SearchField
					{
						FieldName = "GP.GP_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE1
					};
					searchFields.Add(searchField);
				}
			}
			else if (string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1))
			{
				if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
				{
					var searchField = new SearchField
					{
						FieldName = "GP.GP_END_DATE",
						SearchType = SearchType.DateTime,
						SearchText1 = gridSearchRequest.INSPECTION_DATE2
					};
					searchFields.Add(searchField);
				}
			}
			else if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
			{
				var searchField = new SearchField
				{
					FieldName = "GP.GP_END_DATE",
					SearchType = SearchType.DateTime,
					SearchText1 = gridSearchRequest.INSPECTION_DATE1,
					SearchText2 = gridSearchRequest.INSPECTION_DATE2
				};
				searchFields.Add(searchField);
			}

			return searchFields;
		}
        private List<SearchField> ComposeSearchCriteriaTransferReport(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            var locationOrGroup = new List<SearchField>();
            var dateOrGroup = new List<SearchField>();

            if (gridSearchRequest.LOCATION_CD > 0)
            {
                locationOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.LOCATION_CD_FROM",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                });

                locationOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.LOCATION_CD_TO",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                });

                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", locationOrGroup.Select(f => f.FieldName + " = " + f.SearchText1)) + ")",
                    SearchType = SearchType.Custom
                });

            }
            if (gridSearchRequest.PROCESS_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.PROCESS_KUBUN_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.CUSTOMER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CUSTOMER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_CD)
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


            if (gridSearchRequest.PROD_DIM_D_MAX == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN == null)
            {
                if (gridSearchRequest.PROD_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
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
            if (gridSearchRequest.PROD_LENGTH_MAX == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN == null)
            {
                if (gridSearchRequest.PROD_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "PP.PROD_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
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

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE
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

            if (!string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE1) && !string.IsNullOrEmpty(gridSearchRequest.INSPECTION_DATE2))
            {
                dateOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.ACCEPT_PLANNED_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE1).ToString("yyyy/MM/dd"),
                    SearchText2 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE2).ToString("yyyy/MM/dd")
                });

                dateOrGroup.Add(new SearchField
                {
                    FieldName = "WIP.TRANSFER_DATE",
                    SearchType = SearchType.DateTime,
                    SearchText1 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE1).ToString("yyyy/MM/dd"),
                    SearchText2 = DateTime.Parse(gridSearchRequest.INSPECTION_DATE2).ToString("yyyy/MM/dd")
                });

                searchFields.Add(new SearchField
                {
                    FieldName = "(" + string.Join(" OR ", dateOrGroup.Select(f =>
                        $"{f.FieldName} BETWEEN " +
                        $"TO_DATE('{f.SearchText1} 00:00','yyyy/MM/dd hh24:mi') " +
                        $"AND TO_DATE('{f.SearchText2} 23:59','yyyy/MM/dd hh24:mi')"
                    )) + ")",
                    SearchType = SearchType.Custom
                });
            }
            return searchFields;
        }

        private DataTable SetHeaderForHeatTreatment(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FuranceOutDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceVerifyBy"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LotNo"), typeof(string));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_RollMonth"), typeof(string));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CoilNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PositionInFurnace"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PatternNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryRemarks"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));



            return dt;
        }

        private DataTable SetHeaderForDrawingBarToBar(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Tolerance_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HTCondition"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActDimCSV"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BarCount"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExternalSurfaceCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStatus"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo")); 
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));

            return dt;
        }

        private DataTable SetHeaderForCombinedDailyReport(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReservedPieces"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReservedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia5"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia6"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Len1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Len2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Len3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedPieces"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MagnetismMT"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Bend"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExtTexture"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shikii"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNGBars"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));


            return dt;
        }

        private DataTable SetHeaderForColdDrawingWireDrawing(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Tolerance_L"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DieNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WindingShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActulaDimFirsttoLast"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExtCondition"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MagenticDust"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_bundleno"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));

            return dt;
        }

        private DataTable SetHeaderForStraightening(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimBeforeWithBracket"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimAfterWithBracket"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLengthAfterStraightening"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));


            return dt;
        }

		private DataTable SetHeaderForStraightening2(string cultureName)
		{
			_allCommonService.GetCultureName(cultureName);
			DataTable dt = new DataTable();
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductDim"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Tolerance_L"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimD"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimBeforeMAX"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimBeforeMIN"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimAfter1"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimAfter2"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimAfter3"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActDim1"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActDim2"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActDim3"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BarCount"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExtConditions"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BendStatus"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));


            return dt;
		}

		private DataTable SetHeaderForChamffer(string cultureName)
		{
			_allCommonService.GetCultureName(cultureName);
			DataTable dt = new DataTable();
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));


			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferedShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));

            return dt;
		}

		private DataTable SetHeaderForEC(string cultureName)
		{
			_allCommonService.GetCultureName(cultureName);
			DataTable dt = new DataTable();
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));


			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReservedPieces"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReservedWeight"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedPieces"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_BarCount"));

            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DefectName"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGQuantity3"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_IsOK"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LineSpeed"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Magnetism_L"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));

            return dt;
		}

        private DataTable SetHeaderForStampping(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessLocation"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));



            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocNoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stock_Unit"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Stamp_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));


            return dt;
        }

        private DataTable SetHeaderForHardnessTempering(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MetallicLabelNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocUnitsWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitsWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LiquidType(FurnaceSetting)"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceTemperature"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceConcentration"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceFrequncy"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_UnitType3OF3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Manganese"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Cr"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Mo"));

           
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessSpecified"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessTarget"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QMacNo/Operator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QFrncInOutDtTm"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QTemp X Time"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QNorthUp"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QNorthDown"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QSouthUp"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_QSouthDown"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TMacNo/Operator"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TFrncInOutDtTm"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TTemp X Time"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TNorthUp"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TNorthDown"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TSouthUp"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TSouthDown"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessTempLocation"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DM-F"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DM-T"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_JobOut"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_JobIn"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDecesion"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionRemark"));
            return dt;
        }

		private DataTable SetHeaderForQATestHardness(string cultureName)
		{
			_allCommonService.GetCultureName(cultureName);
			DataTable dt = new DataTable();
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionLoc"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDia"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));


			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Column"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcCondition"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Step"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PatternNo"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Center"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DM-F"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D1by4"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DM-T"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D2by4"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D3by4"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Average"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4by4"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Org"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4Avg"));

			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
			dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));


			return dt;
		}

        private DataTable SetHeaderForMechHardness(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionLoc"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Split"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Center"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D1by4"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D2by4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D3by4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_D4by4"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_HardnessTypeAct"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Average"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));


            return dt;
        }

        private DataTable SetHeaderForMechTensile(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionLoc"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Split"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dia"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FurnaceNum"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaxLoadN"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YieldLoadN"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim_S"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FractureDimension_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestDim"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_GuageDistance_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CuttingElongation_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TestImpact"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensileStrengthWithTs"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_YeildStrength"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ElongationWithEL"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReductionWithRA"));

            return dt;
        }

        private DataTable SetHeaderForFinalInspection(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionLoc"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_InspectionDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NifudaNumber"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyScreenShape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimTolerance"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim1Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim2Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim3Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualDim4Short"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActualLength_S"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocQty1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedUnit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWt"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CombinedDecision"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimSpark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthSpark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Color_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ChamferShort"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Spark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ExtTexture"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Inspector"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Remark"));

            return dt;
        }

        private DataTable SetHeaderForGeneralProcess(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessKubun_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Location"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayStart"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_OperationDayEnd"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Operator"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Machine_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Gouban"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimOtherFactory"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedPieces"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnits1"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString(""));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGWeight1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnits2"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString(""));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGWeight2"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGReason3"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGUnits3"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString(""));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NGWeight3"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DailyReportSupplement"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Decision"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));
            return dt;
        }

        private DataTable SetHeaderForTransferReport(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TransferDate"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceivedDate"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FromLoc"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PlannedLoc"));
            //dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ReceivedLoc"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_PlanDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TensoDestination"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TransferDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Source1"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_VehicleName"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WidthAndOther"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));


            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TabaNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_NoOfUnits"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Unit"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_WeightWithoutKG"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ManufacturingSpecificationNo"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_TransferNumber"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Message"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_CommunicationRemark"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lblNextProcess"));

            return dt;
        }


    }
}
