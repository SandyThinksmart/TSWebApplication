using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class SalesProductionStatusSearchController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        private readonly IAllCommonService _allCommonService;
        private readonly CsvSqlString _csvSqlString;
        private IHostingEnvironment Environment;
        public SalesProductionStatusSearchController(IHostingEnvironment _environment, LocalizationServices localization, IViewRenderService viewRenderService, IAllCommonService commonService, CsvSqlString csvSqlString)
        {
            _localization = localization;
            _viewRenderService = viewRenderService;
            _allCommonService = commonService;
            _csvSqlString = csvSqlString;
            Environment = _environment;
        }
        public IActionResult SalesProductionStatusSearch1()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult SalesProductionStatusSearch2()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult MessageList()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult getSalesProdStatusByTimewise([FromBody] List<ProcessPlanForSalesProductionStatusDetails> details)
        {
            string timeWiseView = _viewRenderService.RenderToString("SalesProductionStatusSearch/SalesProductionTimewiseGrid", details);
            //details = details.AsEnumerable().GroupBy(row => new { PP_PROCESS_NAME = row.PP_PROCESS_NAME }).Select(r => r.First()).ToList();
            //string processView = _viewRenderService.RenderToString("SalesProductionStatusSearch/ProcessGridView", details);
             return Json(new { IsSuccess = true, message = timeWiseView});
        }
        public IActionResult getSalesProdStatusByBundlewise([FromBody] ProcessPlanForSalesProductionStatusDetails details)
        {
            var timeWiseDetails = details.GETDATATIMEWISE;
            var bundleWiseDetails = details.GETDATABUNDLEWISE;

            for (int i = 0; i <= timeWiseDetails.Count - 1; i++)// for loop used for if comma separated value found in TABA_NO then new row created for each value and that comma added value row removed
            {
                if (!string.IsNullOrEmpty(timeWiseDetails[i].TABA_NO))
                {
                    if (timeWiseDetails[i].TABA_NO.Contains(','))
                    {
                        var tabaNo = timeWiseDetails[i].TABA_NO.Split(",");
                        List<ProcessPlanForSalesProductionStatusDetails> listData = new List<ProcessPlanForSalesProductionStatusDetails>();
                        ProcessPlanForSalesProductionStatusDetails list = new ProcessPlanForSalesProductionStatusDetails();
                        foreach (var item in tabaNo)
                        {
                            list = new ProcessPlanForSalesProductionStatusDetails();
                            list.SERIAL_NO = timeWiseDetails[i].SERIAL_NO;
                            list.PP_PROCESS_PLAN_CD = timeWiseDetails[i].PP_PROCESS_PLAN_CD;
                            list.PROCESS_NAME_JP = timeWiseDetails[i].PROCESS_NAME_JP;
                            list.PROCESS_NAME_EN = timeWiseDetails[i].PROCESS_NAME_EN;
                            list.PP_PROCESS_NAME = timeWiseDetails[i].PP_PROCESS_NAME;
                            list.END_DATE = timeWiseDetails[i].END_DATE;
                            list.MACHINE_NAME = timeWiseDetails[i].MACHINE_NAME;
                            list.FINISHED_BARS = timeWiseDetails[i].FINISHED_BARS;
                            list.FINISHED_BARS2 = timeWiseDetails[i].FINISHED_BARS2;
                            list.FINISHED_BARS3 = timeWiseDetails[i].FINISHED_BARS3;
                            list.FINISHED_BARS4 = timeWiseDetails[i].FINISHED_BARS4;
                            list.UNIT_TYPE = timeWiseDetails[i].UNIT_TYPE;
                            list.UNIT_TYPE2 = timeWiseDetails[i].UNIT_TYPE2;
                            list.UNIT_TYPE3 = timeWiseDetails[i].UNIT_TYPE3;
                            list.UNIT_TYPE4 = timeWiseDetails[i].UNIT_TYPE4;
                            list.FINISHED_QTYS = timeWiseDetails[i].FINISHED_QTYS;
                            list.FINISHED_QTYS2 = timeWiseDetails[i].FINISHED_QTYS2;
                            list.FINISHED_QTYS3 = timeWiseDetails[i].FINISHED_QTYS3;
                            list.FINISHED_QTYS4 = timeWiseDetails[i].FINISHED_QTYS4;
                            list.DECISION_RESULT = timeWiseDetails[i].DECISION_RESULT;
                            list.FI_DECISION = timeWiseDetails[i].FI_DECISION;
                            list.EC_DECISION = timeWiseDetails[i].EC_DECISION;
                            list.PP_COLOR = timeWiseDetails[i].PP_COLOR;
                            list.PP_CHAMFER = timeWiseDetails[i].PP_CHAMFER;
                            list.FI_CHAMFER_DECISION = timeWiseDetails[i].FI_CHAMFER_DECISION;
                            list.FI_SPARK_DECISION = timeWiseDetails[i].FI_SPARK_DECISION;
                            list.BEND_STATUS = timeWiseDetails[i].BEND_STATUS;
                            list.EXTERIOR_CONDITION = timeWiseDetails[i].EXTERIOR_CONDITION;
                            list.EC_DONE_CHECK = timeWiseDetails[i].EC_DONE_CHECK;
                            list.TOTAL_NG_BARS = timeWiseDetails[i].TOTAL_NG_BARS;
                            list.MESSAGE = timeWiseDetails[i].MESSAGE;
                            list.END_OPERATOR_NAME = timeWiseDetails[i].END_OPERATOR_NAME;
                            list.TABA_NO = item;
                            list.COMBINE_TABA1 = timeWiseDetails[i].COMBINE_TABA1;
                            list.COMBINE_TABA2 = timeWiseDetails[i].COMBINE_TABA2;
                            list.COMBINE_TABA3 = timeWiseDetails[i].COMBINE_TABA3;
                            list.COMBINE_TABA4 = timeWiseDetails[i].COMBINE_TABA4;
                            list.DIVISION_TABA1 = timeWiseDetails[i].DIVISION_TABA1;
                            list.DIVISION_TABA2 = timeWiseDetails[i].DIVISION_TABA2;
                            list.DIVISION_TABA3 = timeWiseDetails[i].DIVISION_TABA3;
                            list.DIVISION_TABA4 = timeWiseDetails[i].DIVISION_TABA4;
                            list.PP_PROCESS_1 = timeWiseDetails[i].PP_PROCESS_1;
                            list.PP_PROCESS_2 = timeWiseDetails[i].PP_PROCESS_2;
                            list.PP_PROCESS_3 = timeWiseDetails[i].PP_PROCESS_3;
                            list.PP_PROCESS_4 = timeWiseDetails[i].PP_PROCESS_4;
                            list.PP_PROCESS_5 = timeWiseDetails[i].PP_PROCESS_5;
                            list.PP_PROCESS_6 = timeWiseDetails[i].PP_PROCESS_6;
                            list.PP_PROCESS_7 = timeWiseDetails[i].PP_PROCESS_7;
                            list.PP_PROCESS_8 = timeWiseDetails[i].PP_PROCESS_8;
                            list.PP_PROCESS_9 = timeWiseDetails[i].PP_PROCESS_9;
                            list.PP_PROCESS_10 = timeWiseDetails[i].PP_PROCESS_10;
                            list.PP_PROCESS_11 = timeWiseDetails[i].PP_PROCESS_11;
                            list.PP_PROCESS_12 = timeWiseDetails[i].PP_PROCESS_12;
                            list.PP_PROCESS_13 = timeWiseDetails[i].PP_PROCESS_13;
                            listData.Add(list);
                        }
                        //}
                        timeWiseDetails.RemoveAt(i);
                        timeWiseDetails.AddRange(listData);
                    }

                }
            }


            //if (details.CULTURE_NAME == "en") {
            //    foreach (ProcessPlanForSalesProductionStatusDetails data in bundleWiseDetails) 
            //    {
            //        data.PP_PROCESS_NAME = data.PROCESS_NAME_EN;
            //    }

            //} else
            //{
            //    foreach (ProcessPlanForSalesProductionStatusDetails data in bundleWiseDetails)
            //    {
            //        data.PP_PROCESS_NAME = data.PROCESS_NAME_JP;
            //    }
            //}


            List<ProcessPlanForSalesProductionStatusDetails> salesProductionBundleWise = new List<ProcessPlanForSalesProductionStatusDetails>();
            var resultData = bundleWiseDetails.AsEnumerable().OrderByDescending(r => r.END_DATE).ThenByDescending(r => r.SERIAL_NO).DistinctBy(r => r.TABA_NO).Select(r => new { r.SERIAL_NO, r.TABA_NO, r.COMBINE_TABA1, r.COMBINE_TABA2, r.COMBINE_TABA3, r.COMBINE_TABA4, r.DIVISION_TABA1, r.DIVISION_TABA2, r.DIVISION_TABA3, r.DIVISION_TABA4, r.FINISHED_BARS_DECODE, r.FINISHED_BARS2_DECODE, r.FINISHED_BARS3_DECODE, r.FINISHED_BARS4_DECODE, r.FINISHED_QTYS, r.FINISHED_QTYS2, r.FINISHED_QTYS3, r.FINISHED_QTYS4, r.PP_COLOR, r.FI_COLOR, r.FI_DECISION_STRING, r.RESULT_DECISION_STRING, r.TOTAL_NG_BARS, r.PP_PROCESS_1, r.PP_PROCESS_2, r.PP_PROCESS_3, r.PP_PROCESS_4, r.PP_PROCESS_5, r.PP_PROCESS_6, r.PP_PROCESS_7, r.PP_PROCESS_8, r.PP_PROCESS_9, r.PP_PROCESS_10, r.PP_PROCESS_11, r.PP_PROCESS_12, r.PP_PROCESS_13,r.PP_PROCESS_NAME }).ToList();
            resultData = resultData.AsEnumerable().OrderBy(r => Convert.ToInt32(r.TABA_NO)).ToList();
           
            foreach (var item in resultData)
            {
                int i = 0;
                var listData = bundleWiseDetails.AsEnumerable().OrderBy(r => r.END_DATE).Where(r => r.TABA_NO == item.TABA_NO).ToList();
               //var getProcessList = listData.AsEnumerable().GroupBy(r => r.PP_PROCESS_NAME).Select(r => r.FirstOrDefault()).ToList();
                ProcessPlanForSalesProductionStatusDetails bundleWise = new ProcessPlanForSalesProductionStatusDetails();

                bundleWise.TABA_NO = item.TABA_NO;
               
                if (salesProductionBundleWise.Count> 0 )
                {
                    long serialNo = Convert.ToInt64(item.SERIAL_NO);
                    string processName = item.PP_PROCESS_NAME;
                    var getTimeWise = timeWiseDetails.AsEnumerable().Where(r => r.TABA_NO == bundleWise.TABA_NO).ToList(); //data avaibale as per single taba number in timewise, if exist then shown BARS, UNITTYPE and QTY
                    var getData = salesProductionBundleWise.AsEnumerable().Where(r => r.SERIAL_NO == serialNo && r.PP_PROCESS_NAME== processName).ToList();//as per serial number and process name different taba exist then only once unit and qty shown in below
                    // if (salesProductionBundleWise[0].SERIAL_NO != serialNo && getTimeWise.Count > 0) //salesProductionBundleWise[0].FINISHED_BARS_DECODE == null
                    if (!getData.Any() && getTimeWise.Count > 0)
                    {
                        bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS_DECODE;
                    }
                    if (!getData.Any() && getTimeWise.Count > 0) //salesProductionBundleWise[0].FINISHED_QTYS == null
                    {
                        bundleWise.FINISHED_QTYS = item.FINISHED_QTYS;
                    }
                    if (item.DIVISION_TABA1 == item.TABA_NO)
                    {
                        bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS_DECODE;
                        bundleWise.FINISHED_QTYS = item.FINISHED_QTYS;
                    }
                    if (item.DIVISION_TABA2 == item.TABA_NO)
                    {
                        bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS2_DECODE;
                        bundleWise.FINISHED_QTYS = item.FINISHED_QTYS2;
                    }
                    if (item.DIVISION_TABA3 == item.TABA_NO)
                    {
                        bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS3_DECODE;
                        bundleWise.FINISHED_QTYS = item.FINISHED_QTYS3;
                    }
                    if (item.DIVISION_TABA4 == item.TABA_NO)
                    {
                        bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS4_DECODE;
                        bundleWise.FINISHED_QTYS = item.FINISHED_QTYS4;
                    }

                    //if (item.COMBINE_TABA1 == item.TABA_NO)
                    //{
                    //    bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS_DECODE;
                    //    bundleWise.FINISHED_QTYS = item.FINISHED_QTYS;
                    //}
                    //if (item.COMBINE_TABA2 == item.TABA_NO)
                    //{
                    //    bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS2_DECODE;
                    //    bundleWise.FINISHED_QTYS = item.FINISHED_QTYS2;
                    //}
                    //if (item.COMBINE_TABA3 == item.TABA_NO)
                    //{
                    //    bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS3_DECODE;
                    //    bundleWise.FINISHED_QTYS = item.FINISHED_QTYS3;
                    //}
                    //if (item.COMBINE_TABA4 == item.TABA_NO)
                    //{
                    //    bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS4_DECODE;
                    //    bundleWise.FINISHED_QTYS = item.FINISHED_QTYS4;
                    //}

                }
                else
                {
                    bundleWise.FINISHED_BARS_DECODE = item.FINISHED_BARS_DECODE;
                    bundleWise.FINISHED_QTYS = item.FINISHED_QTYS;
                  
                }
                var fiData = bundleWiseDetails.AsEnumerable().Where(r => r.TABA_NO == item.TABA_NO && r.FI_DECISION != null).Select(r => new { r.FI_DECISION_STRING, r.FI_COLOR }).FirstOrDefault();
                if (fiData != null)
                {
                    bundleWise.FI_DECISION_STRING = fiData.FI_DECISION_STRING;
                    bundleWise.FI_COLOR = fiData.FI_COLOR;
                }

                var ecData = bundleWiseDetails.AsEnumerable().Where(r => r.TABA_NO == item.TABA_NO && r.EC_DECISION !=null).Select(r => new {r.EC_DECISION_STRING, r.EC_COLOR, r.TOTAL_NG_BARS }).FirstOrDefault();
                if (ecData !=null)
                {
                    bundleWise.EC_DECISION_STRING = ecData.EC_DECISION_STRING;
                    bundleWise.EC_COLOR = ecData.EC_COLOR;
                    bundleWise.TOTAL_NG_BARS = ecData.TOTAL_NG_BARS;
                }
                

                if (!string.IsNullOrEmpty(item.PP_COLOR))
                {
                    bundleWise.PP_COLOR = item.PP_COLOR;
                }
                
                bundleWise.RESULT_DECISION_STRING = item.RESULT_DECISION_STRING;
                bundleWise.SERIAL_NO = item.SERIAL_NO;
                bundleWise.PP_PROCESS_NAME = item.PP_PROCESS_NAME;

                foreach (var data in listData)
                {
                    i = i + 1;
                    if (i == 1)
                    {
                        bundleWise.PP_PROCESS_1 = data.PP_PROCESS_NAME;
                    }else if (i == 2)
                    {
                        bundleWise.PP_PROCESS_2 = data.PP_PROCESS_NAME;
                    }else if (i == 3)
                    {
                        bundleWise.PP_PROCESS_3 = data.PP_PROCESS_NAME;
                    }else if (i == 4)
                    {
                        bundleWise.PP_PROCESS_4 = data.PP_PROCESS_NAME;
                    }else if (i == 5)
                    {
                        bundleWise.PP_PROCESS_5 = data.PP_PROCESS_NAME;
                    }else if (i == 6)
                    {
                        bundleWise.PP_PROCESS_6 = data.PP_PROCESS_NAME;
                    }else if (i == 7)
                    {
                        bundleWise.PP_PROCESS_7 = data.PP_PROCESS_NAME;
                    }else if (i == 8)
                    {
                        bundleWise.PP_PROCESS_8 = data.PP_PROCESS_NAME;
                    }else if (i == 9)
                    {
                        bundleWise.PP_PROCESS_9 = data.PP_PROCESS_NAME;
                    }else if (i == 10)
                    {
                        bundleWise.PP_PROCESS_10 = data.PP_PROCESS_NAME;
                    }else if (i == 11)
                    {
                        bundleWise.PP_PROCESS_11 =data.PP_PROCESS_NAME;
                    }else if (i == 12)
                    {
                        bundleWise.PP_PROCESS_12 =data.PP_PROCESS_NAME;
                    }else if (i == 13)
                    {
                        bundleWise.PP_PROCESS_13 =data.PP_PROCESS_NAME;
                    }
                   
                }
               

                salesProductionBundleWise.Add(bundleWise);
            }
            if (salesProductionBundleWise.Count > 0)
            {
                salesProductionBundleWise = salesProductionBundleWise.AsEnumerable().Where(r => !string.IsNullOrEmpty(r.TABA_NO)).ToList(); // null taba row removed from bundle grid list cnf by Nilesh Shir 2026/06/11
            }
            string bundleWiseView = _viewRenderService.RenderToString("SalesProductionStatusSearch/SalesProductionBundlewiseGrid", salesProductionBundleWise);
            return Json(new { IsSuccess = true, message = bundleWiseView });
        }
        public IActionResult getProcessData([FromBody] List<ProcessPlanForSalesProductionStatusDetails> details)
        {
            List < ProcessPlanForSalesProductionStatusDetails > result =  new List<ProcessPlanForSalesProductionStatusDetails> ();

            foreach (var item in details)
            {
                ProcessPlanForSalesProductionStatusDetails processDetails = new ProcessPlanForSalesProductionStatusDetails ();
                if (item.PP_PROCESS_1 !=null)
                {
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_1;
                    
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_1;
                    
                    result.Add (processDetails);
                }
                if (item.PP_PROCESS_2 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_2;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_2;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_3 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_3;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_3;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_4 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_4;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_4;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_5 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_5;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_5;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_6 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_6;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_6;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_7 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_7;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_7;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_8 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_8;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_8;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_9 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_9;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_9;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_10 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_10;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_10;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_11 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_11;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_11;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_12 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_12;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_12;
                    result.Add(processDetails);
                }
                if (item.PP_PROCESS_13 != null)
                {
                    processDetails = new ProcessPlanForSalesProductionStatusDetails();
                    processDetails.PP_PROCESS_NAME = item.PP_PROCESS_13;
                    processDetails.PROCESS_PROD_POINT = item.PP_PROD_POINT_13;
                    result.Add(processDetails);
                }
            }
            //details = details.AsEnumerable().GroupBy(row => new { PP_PROCESS_NAME = row.PP_PROCESS_NAME }).Select(r => r.First()).ToList();
            string view = _viewRenderService.RenderToString("SalesProductionStatusSearch/ProcessGridView", result);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult getHardnessAndPostHTByProcessPlanCD([FromBody] List<ProcessPlanForSalesProductionStatusDetails> details)
        {
            if(details.Count > 0)
            {
                details = details.AsEnumerable().OrderBy(r => r.DETAIL_NO).ToList();
            }   
            foreach (var  item in details)
            {
                if (Convert.ToInt32(item.TENSILE_STRN_PATTERN1_MIN_MAX) > 0)
                {
                    string tnsileString = UIUtils.GetHardnessMaxMinKubun(Convert.ToString(item.TENSILE_STRN_PATTERN1_MIN_MAX));
                    item.TENSILE_STRN_STRING = item.TENSILE_STRN_PATTERN1 + tnsileString;
                }
                if (Convert.ToInt32(item.HARDNESS_PATTERN1_CODE) > 0)
                {
                    item.STD_HARDNESS_STRING = UIUtils.GetStandardHardnessKubunValue(item.HARDNESS_PATTERN1_CODE.ToString());
                }
                if (Convert.ToInt32(item.HARDNESS_PATTERN1_MIN_MAX) > 0)
                {
                    item.HARD_MIN_MAX_STRING = UIUtils.GetHardnessMaxMinKubun(item.HARDNESS_PATTERN1_MIN_MAX.ToString());
                }
                if (Convert.ToInt32(item.PRESS_PATTERN1_MIN_MAX) > 0)
                {
                    string pressString = UIUtils.GetHardnessMaxMinKubun(Convert.ToString(item.PRESS_PATTERN1_MIN_MAX));
                    item.PRESS_PATTERN1_STRING = item.PRESS_PATTERN1 + pressString;
                }
                if (Convert.ToInt32(item.ELONGATION_PATTERN1_MIN_MAX) > 0)
                {
                    string elognationString = UIUtils.GetHardnessMaxMinKubun(Convert.ToString(item.ELONGATION_PATTERN1_MIN_MAX));
                    item.ELONGATION_PATTERN1_STRING =  item.ELONGATION_PATTERN1 + elognationString;
                }
            }
            
            //details = details.AsEnumerable().GroupBy(row => new { PP_PROCESS_PLAN_CD = row.PP_PROCESS_PLAN_CD }).Select(r => r.First()).ToList();
            string view = _viewRenderService.RenderToString("SalesProductionStatusSearch/HardnessAndPostHTGridView", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GridSearch([FromBody] List<SalesProductionStatusSearchDetails> details)
        {
           
            string view = _viewRenderService.RenderToString("SalesProductionStatusSearch/SalesProductionStatusSearchGridView", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult getMessageListByProcessPlanCD([FromBody] List<ProcessPlanForSalesProductionStatusDetails> details)
        {
            string view = _viewRenderService.RenderToString("SalesProductionStatusSearch/MessageListGridView", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult InitializeSortPopup()
        {
            var sortFields = new List<TSSortField> {
                                        new TSSortField {
                                            FieldName = "PP_CHUUBAN_GYOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Chuuban"),
                                           IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "CST.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Customer"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "EMP.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                        new TSSortField {
                                            FieldName = "INTE.INTERNAL_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_InternalName"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "PP.PROD_SHAPE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Shape"),
                                             IsDefaultSort=true,
                                             SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "PP.PROD_NAME_CUST",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ProdNameCust"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "PP.PROD_DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Dimension"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName =   "PP.PROD_LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              new TSSortField {
                                            FieldName =  "TO_DATE(TO_CHAR(PP.ACT_ALLOC_DATE))",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_ActAllocDate"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                            new TSSortField {
                                            FieldName =  "TO_DATE(TO_CHAR(PP.DELIVERY_DATE))",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DeliveryDate"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                              

                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/06/17  
        /// </summary>
        /// used screens: Sales Production Status Search
        /// <returns>CSV path</returns>
        public IActionResult ExportCSV([FromBody] GridSearchFields gridSearchRequest)
        {
            var message = "";
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            string SQL = _csvSqlString.GetSalesProductionStatusSQL(searchFields, selectedSortCriteria);

            bool addDateInFileName = true;
            bool addTimeInFileName = false;
            string fileName = _allCommonService.GetFileName(_localization.GetLocalizedHtmlString("lbl_ProductionStatusCSV"), addDateInFileName, addTimeInFileName);
            DataTable dataTableWithHeader = SetHeaderForCSV(gridSearchRequest.CULTURE_NAME);           


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

        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();



            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN, CASE WHEN LENGTH(TRIM(PP.GYOUBAN)) = 1 THEN '0' || PP.GYOUBAN END))",//"PP.CHUUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHUUBAN.Trim()
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
                    // FieldName = "PP.PROD_NAME_INT",
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
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

            if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SHAPE.ToLower().Trim()
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

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.SALES_INCHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SALES_INCHARGE.Trim()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE(TO_CHAR(PP.ACT_ALLOC_DATE))",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SPEC_NO1,

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SPEC_NO2
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
            if (!string.IsNullOrEmpty(gridSearchRequest.COMPLETE_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE(TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE))",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.COMPLETE_START_DATE,
                    SearchText2 = gridSearchRequest.COMPLETE_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (Convert.ToInt32(gridSearchRequest.ACTIVE_CHUUBAN_ONLY) > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROCESS_PLAN_CMPL_DATE",
                    SearchType = SearchType.IsNull,
                    SearchText1 = gridSearchRequest.ACTIVE_CHUUBAN_ONLY.ToString(),
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        private DataTable SetHeaderForCSV(string cultureName)
        {
            _allCommonService.GetCultureName(cultureName);
            DataTable dt = new DataTable();
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Chuuban"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Customer"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SalesIncharge_M"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DeliveryDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProdNameCust"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Dimension"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Shape"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Length"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_DimTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_LengthTolerance"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FureMagariStd"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_Charge"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocDate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ActAllocWeight"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_FinishedWt"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_SuccessRate"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductionStatus_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MatlNameMaker"));

            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialDimD"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_MaterialShape_L"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProductSpecNo"));
            dt.Columns.Add(_localization.GetLocalizedHtmlString("lbl_ProcessPlanFinishedDate_L"));
            

            return dt;
        }
        public IActionResult DowloadAndSavePDF(string filePath)
        {
            List<MstConfiguration> configuration = new List<MstConfiguration>();
            string ftpPath = string.Empty;
            string ftpUsername = string.Empty;
            string ftpPassword = string.Empty;
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            string rootPath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet\\");


            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            configuration = _allCommonService.GetFTPDetail();

            foreach (var item in configuration)
            {
                if (item.KEY.ToUpper() == "MILLSHEET_FTPPATH")
                {
                    ftpPath = item.VALUE;
                   // ftpPath = ftpPath + "/";
                }
                else if (item.KEY.ToUpper() == "MILLSHEET_FTPPASSWORD")
                {
                    ftpPassword = item.VALUE;
                }
                else if (item.KEY.ToUpper() == "MILLSHEET_FTPUSERNAME")
                {
                    ftpUsername = item.VALUE;
                }
            }
            ftpPath = ftpPath+ "/" + filePath;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.UseBinary = true;
            request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = new FileStream(rootPath + fileName, FileMode.Create, FileAccess.Write))
            {
                responseStream.CopyTo(fileStream);
            }

            return Json(new { IsSuccess = true,message= "/MakerMillsheet/"+ fileName });
        }
    }

}
