using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Application.Services
{
    public class SalesProductionStatusSearchService : ISalesProductionStatusSearchService
    {
        private readonly ISalesProductionStatusSearchRepository _SalesProductionStatusSearchRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;

        public SalesProductionStatusSearchService(ISalesProductionStatusSearchRepository salesProductionStatusSearchRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization)
        {
            _SalesProductionStatusSearchRepository = salesProductionStatusSearchRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
        }
        public List<SalesProductionStatusSearchDetails> GetSalesProductionStatusList(GridSearchFields gridSearchRequest)
        {
            short input_Location_CD = Convert.ToInt16(_UserLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = input_Location_CD;

            List<SalesProductionStatusSearchDetails> salesProductionStatusSearchDetails = new List<SalesProductionStatusSearchDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            if (gridSearchRequest.CULTURE_NAME == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            salesProductionStatusSearchDetails = _SalesProductionStatusSearchRepository.GetSalesProductionStatusList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            foreach (SalesProductionStatusSearchDetails record in salesProductionStatusSearchDetails)
            {
                record.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN)), Convert.ToDecimal(record.PP_BEND_TOL), Convert.ToInt32(record.PP_MAGARI_SPAN));
                if (string.IsNullOrEmpty(record.PROCESS_PLAN_CMPL_DATE_STRING))
                {
                    record.PROD_CONDITION_DESCRIPTION = _localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun1of2");
                }else
                {
                    record.PROD_CONDITION_DESCRIPTION = _localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun2of2");
                }
               
            }
            return salesProductionStatusSearchDetails;
        }
        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();



            if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN ,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END))",//"PP.CHUUBAN",
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
                    SearchCondition = SearchCondition.Equal.ToString(),
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

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SHAPE.ToLower().Trim()
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
            if (gridSearchRequest.MAX_ROW_COUNT > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ROWNUM <",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.MAX_ROW_COUNT.ToString(),
                };
                searchFields.Add(searchField);
            }
           
            return searchFields;
        }

        public ProcessPlanForSalesProductionStatusDetails getSalesProdStatusByProcessPlanCD(long processPlanCD, string cultureName)
        {
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            var _ProcessDetails = _SalesProductionStatusSearchRepository.GetSalesStatusDetailsByProcessPlanCD(processPlanCD);



            if (_ProcessDetails !=null )
            {
                //if (_ProcessDetails.PP_COLOR_ID > 0)
                //{
                //    _ProcessDetails.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(_ProcessDetails.PP_COLOR_ID));
                //}
                if (_ProcessDetails.PP_MAGARI_KUBUN != null)
                {
                    _ProcessDetails.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_ProcessDetails.PP_MAGARI_KUBUN)), Convert.ToDecimal(_ProcessDetails.PP_BEND_TOL), Convert.ToInt32(_ProcessDetails.PP_MAGARI_SPAN));
                }
                if (_ProcessDetails.PP_NO_OF_UNITS != null)
                {
                    _ProcessDetails.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(_ProcessDetails.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(_ProcessDetails.PP_UNIT));
                }
                if (_ProcessDetails.PP_STD_DEL_PERIOD >= 0)
                {
                    _ProcessDetails.PP_STD_DEL_PERIOD_STRING = _ProcessDetails.PP_STD_DEL_PERIOD + " " + _localization.GetLocalizedHtmlString("lbl_Day");
                }

                if (_ProcessDetails.PP_UNIT_WT_KUBUN > 0)
                {
                    _ProcessDetails.PP_UNIT_WT_STRING = Utils.GetUnitWeigth(Convert.ToInt16(_ProcessDetails.PP_UNIT_WT_KUBUN), Convert.ToInt32(_ProcessDetails.PP_UNIT_WT_STRING), Convert.ToInt16(_ProcessDetails.PP_UNIT_WT_MIN), _ProcessDetails.PP_UNIT_WT_MAX);

                }
                _ProcessDetails.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(_ProcessDetails.COIL_DIA_WINDING));

            }

            return _ProcessDetails;

        }

        public ProcessPlanForSalesProductionStatusDetails getTimeAndBundlewiseByProcessPlanCD(long processPlanCD, string cultureName)
        {
            string tabaChar = _localization.GetLocalizedHtmlString("lbl_Thaba_L");
            ProcessPlanForSalesProductionStatusDetails result= new ProcessPlanForSalesProductionStatusDetails();
            var _ProcessDetails = _SalesProductionStatusSearchRepository.GetTimeAndBundlewiseDetailsByProcessPlanCD(processPlanCD);

            var getDataTimewise = _ProcessDetails.GETDATATIMEWISE;

            foreach (ProcessPlanForSalesProductionStatusDetails record in getDataTimewise)
            {
                if (cultureName == "en")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                }

                if (Convert.ToInt32(record.COMBINE_TABA1) > 0 && record.MESSAGE != null)
                {
                    //var getData = record.MESSAGE.Split(" ");
                    //string keyName = string.Empty ; 
                    //string tahabadetils = string.Empty ;

                    //int i = 0;
                    //if (getData.Any())
                    //{
                    //    foreach (var item in getData)
                    //    {
                    //        if (i == 0)
                    //        {
                    //            keyName = item;
                    //        }
                    //        else
                    //        {
                    //            tahabadetils += item;
                    //        }
                    //        i++;
                    //    }
                    //    record.MESSAGE = _localization.GetLocalizedHtmlString(keyName).ToString().Replace("MSG_CONTENT", tahabadetils);
                    //}
                    record.MESSAGE = record.MESSAGE.Replace("lbl_ThabaMerge ", tabaChar);
                }
               
                if (Convert.ToInt32(record.DIVISION_TABA1) > 0 && record.MESSAGE != null || Convert.ToInt32(record.DIVISION_TABA2) > 0 && record.MESSAGE != null || Convert.ToInt32(record.DIVISION_TABA3) > 0 && record.MESSAGE != null || Convert.ToInt32(record.DIVISION_TABA4) > 0 && record.MESSAGE != null)
                {

                    //var getData = record.MESSAGE.Split(" ");
                    //string keyName = string.Empty;
                    //string tahabadetils = string.Empty;
                    //int i = 0;
                    //if (getData.Any())
                    //{
                    //    foreach (var item in getData)
                    //    {
                    //        if (i == 0)
                    //        {
                    //            keyName = item;
                    //        }
                    //        else
                    //        {
                    //            tahabadetils += item;
                    //        }
                    //        i++;
                    //    }
                    //    record.MESSAGE = _localization.GetLocalizedHtmlString(keyName).ToString().Replace("MSG_CONTENT", tahabadetils);
                    //}
                    record.MESSAGE = record.MESSAGE.Replace("lbl_ThabaDivision ", tabaChar);
                }
                // finished weight combined for TRN_TABA_DIVISION Table only due to multiple columns in table
                if (Convert.ToInt32(record.DIVISION_TABA2) > 0)
                {
                    record.FINISHED_QTYS = record.FINISHED_QTYS + record.FINISHED_QTYS2;
                    record.FINISHED_BARS = record.FINISHED_BARS + record.FINISHED_BARS2;
                }
                if (Convert.ToInt32(record.DIVISION_TABA3) > 0)
                {
                    record.FINISHED_QTYS = record.FINISHED_QTYS + record.FINISHED_QTYS3;
                    record.FINISHED_BARS = record.FINISHED_BARS + record.FINISHED_BARS3;
                }
                if (Convert.ToInt32(record.DIVISION_TABA4) > 0)
                {
                    record.FINISHED_QTYS = record.FINISHED_QTYS + record.FINISHED_QTYS4;
                    record.FINISHED_BARS = record.FINISHED_BARS + record.FINISHED_BARS4;
                }

                if (record.FI_DECISION != null)
                {
                    record.FI_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_DECISION)); //UIUtils.GetResultKubunValue(Convert.ToString(record.FI_DECISION));
                    int decisionFI = 0;
                    if (_localization.GetLocalizedHtmlString("lbl_OK") == record.FI_DECISION_STRING)
                    {
                        decisionFI = 1;
                    }else  if (_localization.GetLocalizedHtmlString("lbl_NG") == record.FI_DECISION_STRING)
                    {
                        decisionFI = 2;
                    } else  if (_localization.GetLocalizedHtmlString("lbl_Hold") == record.FI_DECISION_STRING)
                    {
                        decisionFI = 3;
                    }else if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == record.FI_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == record.FI_DECISION_STRING ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == record.FI_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == record.FI_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_ReEc") == record.FI_DECISION_STRING)
                    {
                        decisionFI = 4;
                    }                    
                    else if (!string.IsNullOrEmpty(record.FI_DECISION_STRING)) //if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == record.FI_DECISION_STRING)
                    {
                        decisionFI = 7;
                    }
                    if (decisionFI > 0)
                    {
                        // record.FI_COLOR = Utils.GetColorName(Convert.ToInt16(decisionFI));
                        record.FI_COLOR = Utils.GetColorNameForSalesProduction(Convert.ToInt16(decisionFI));
                    }
                   
                }
                if (record.FI_CHAMFER_DECISION != null)
                {
                    record.FI_CHAMFER_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_CHAMFER_DECISION));
                }
                if (record.FI_SPARK_DECISION != null)
                {
                    record.FI_SPARK_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_SPARK_DECISION));
                }


                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION_RESULT));
                int decisionVal=0;
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    decisionVal = 1;
                } else if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    decisionVal = 2;
                }else if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    decisionVal = 3;
                }else if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    decisionVal = 4;
                }
                else if (!string.IsNullOrEmpty(decisionName)) //if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    decisionVal = 7;
                }
                

                if (decisionVal > 0)
                {
                    // record.PP_COLOR = Utils.GetColorName(Convert.ToInt16(decisionVal));
                    record.PP_COLOR = Utils.GetColorNameForSalesProduction(Convert.ToInt16(decisionVal));
                }
                if (record.PP_MAGARI_KUBUN != null)
                {
                    record.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN)), Convert.ToDecimal(record.PP_BEND_TOL), Convert.ToInt32(record.PP_MAGARI_SPAN));
                }
                if (record.EC_DONE_CHECK != null)
                {
                    record.EC_DONE_CHECK_DECODE = UIUtils.Instance.GetECDoneCheckKubunValue(Convert.ToString(record.EC_DONE_CHECK));
                }

                record.FINISHED_BARS_DECODE = record.FINISHED_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                record.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(record.COIL_DIA_WINDING));
                record.RESULT_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION_RESULT));
                
                record.BEND_STATUS_DECODE = record.BEND_STATUS !=null ? UIUtils.GetBendStateKubunValue(Convert.ToString(record.BEND_STATUS)) : string.Empty;
                
                record.EXTERIOR_CONDITION_DECODE = record.EXTERIOR_CONDITION != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.EXTERIOR_CONDITION)) : string.Empty;

            }
            result.GETDATATIMEWISE = getDataTimewise;
            var getDataBundlewise = _ProcessDetails.GETDATABUNDLEWISE;

            foreach (ProcessPlanForSalesProductionStatusDetails record in getDataBundlewise)
            {
                if (cultureName == "en")
                {
                    record.PP_PROCESS_NAME = record.PROCESS_NAME_EN;
                }else
                {
                    record.PP_PROCESS_NAME = record.PROCESS_NAME_JP;
                }
                    
                string decisionName = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION_RESULT));
                int decisionVal = 0;
                if (_localization.GetLocalizedHtmlString("lbl_OK") == decisionName)
                {
                    decisionVal = 1;
                } else if (_localization.GetLocalizedHtmlString("lbl_NG") == decisionName)
                {
                    decisionVal = 2;
                } else if (_localization.GetLocalizedHtmlString("lbl_Hold") == decisionName)
                {
                    decisionVal = 3;
                }
                else if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == decisionName ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == decisionName || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == decisionName || _localization.GetLocalizedHtmlString("lbl_ReEc") == decisionName)
                {
                    decisionVal = 4;
                }
                else if (!string.IsNullOrEmpty(decisionName)) //if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == decisionName)
                {
                    decisionVal = 7;
                }
                if (decisionVal > 0)
                {
                    // record.PP_COLOR = Utils.GetColorName(Convert.ToInt16(decisionVal));
                    record.PP_COLOR = Utils.GetColorNameForSalesProduction(Convert.ToInt16(decisionVal));
                }

                if (record.FI_DECISION != null)
                {
                    record.FI_DECISION_STRING = UIUtils.GetFIResultKubun(Convert.ToString(record.FI_DECISION));
                    int decisionNum = 0;
                    if (_localization.GetLocalizedHtmlString("lbl_OK") == record.FI_DECISION_STRING)
                    {
                        decisionNum = 1;
                    } else if (_localization.GetLocalizedHtmlString("lbl_NG") == record.FI_DECISION_STRING)
                    {
                        decisionNum = 2;
                    } else if (_localization.GetLocalizedHtmlString("lbl_Hold") == record.FI_DECISION_STRING)
                    {
                        decisionNum = 3;
                    }
                    else if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == record.FI_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == record.FI_DECISION_STRING ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == record.FI_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == record.FI_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_ReEc") == record.FI_DECISION_STRING)
                    {
                        decisionNum = 4;
                    }
                    else if (!string.IsNullOrEmpty(record.FI_DECISION_STRING)) // if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == record.FI_DECISION_STRING)
                    {
                        decisionNum = 7;
                    }
                   
                    if (decisionNum > 0)
                    {
                        //record.FI_COLOR = Utils.GetColorName(Convert.ToInt16(decisionNum));
                        record.FI_COLOR = Utils.GetColorNameForSalesProduction(Convert.ToInt16(decisionNum));
                    }
                }


                if (record.EC_DECISION != null)
                {
                    record.EC_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.EC_DECISION));
                    int decisionEC = 0;
                    if (_localization.GetLocalizedHtmlString("lbl_OK") == record.EC_DECISION_STRING)
                    {
                        decisionEC = 1;
                    } else if (_localization.GetLocalizedHtmlString("lbl_NG") == record.EC_DECISION_STRING)
                    {
                        decisionEC = 2;
                    } else if (_localization.GetLocalizedHtmlString("lbl_Hold") == record.EC_DECISION_STRING)
                    {
                        decisionEC = 3;
                    }
                    else if (_localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") == record.EC_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_ReDrawing") == record.EC_DECISION_STRING ||
               _localization.GetLocalizedHtmlString("lbl_Restraightening") == record.EC_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") == record.EC_DECISION_STRING || _localization.GetLocalizedHtmlString("lbl_ReEc") == record.EC_DECISION_STRING)
                    {
                        decisionEC = 4;
                    }
                    else if (!string.IsNullOrEmpty(record.EC_DECISION_STRING)) //if (_localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") == record.EC_DECISION_STRING)
                    {
                        decisionEC = 7;
                    }
                    if (decisionEC > 0)
                    {
                        //record.EC_COLOR = Utils.GetColorName(Convert.ToInt16(decisionEC));
                        record.EC_COLOR = Utils.GetColorNameForSalesProduction(Convert.ToInt16(decisionEC));
                    }
                }


                if (record.PP_MAGARI_KUBUN != null)
                {
                    record.PP_MAGARI_BEND_TOL = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN)), Convert.ToDecimal(record.PP_BEND_TOL), Convert.ToInt32(record.PP_MAGARI_SPAN));
                }
                if (record.EC_DONE_CHECK != null)
                {
                    record.EC_DONE_CHECK_DECODE = UIUtils.Instance.GetECDoneCheckKubunValue(Convert.ToString(record.EC_DONE_CHECK));
                }

                record.FINISHED_BARS_DECODE = record.FINISHED_BARS + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE));
                if (record.UNIT_TYPE2 > 0)
                {
                    record.FINISHED_BARS2_DECODE = record.FINISHED_BARS2 + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE2));
                }

                if (record.UNIT_TYPE3 > 0)
                {
                    record.FINISHED_BARS3_DECODE = record.FINISHED_BARS3 + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE3));
                }
                if (record.UNIT_TYPE4 > 0)
                {
                    record.FINISHED_BARS4_DECODE = record.FINISHED_BARS4 + UIUtils.GetProductUnitKubunValue(Convert.ToString(record.UNIT_TYPE4));
                }
                        
                
                record.PP_COIL_DIA_WINDING = UIUtils.GetCoilDiaWindingKubunValue(Convert.ToString(record.COIL_DIA_WINDING));
                record.RESULT_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION_RESULT));
                record.EXTERIOR_CONDITION_DECODE = record.EXTERIOR_CONDITION != null ? UIUtils.GetExteriorConditionKubunValue(Convert.ToString(record.EXTERIOR_CONDITION)) : string.Empty;

            }
            result.GETDATABUNDLEWISE = getDataBundlewise;

            return result;
        }
        public List<ProcessPlanForSalesProductionStatusDetails> getProcessDataByProcessPlanCD(long processPlanCD)
        {
            var _ProcessDetails = _SalesProductionStatusSearchRepository.GetProcessDataDetailsByProcessPlanCD(processPlanCD);


            return _ProcessDetails;
        }

        public List<ProcessPlanForSalesProductionStatusDetails> getHardnessAndPostHTByProcessPlanCD(long processPlanCD)
        {
            var _ProcessDetails = _SalesProductionStatusSearchRepository.GetHardnessAndPostHTDetailsByProcessPlanCD(processPlanCD);

            foreach (ProcessPlanForSalesProductionStatusDetails record in _ProcessDetails)
            {
                //if (record.DECISION_RESULT > 0)
                //{
                //    record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.DECISION_RESULT));
                //}
                if (record.HARDNESS_PATTERN1_CODE > 0 )
                {
                    record.STD_HARDNESS_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN1_CODE));
                }
                if (record.HARDNESS_PATTERN2_CODE > 0)
                {
                    record.STD_HARDNESS_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN2_CODE));
                }
                if (record.HARDNESS_PATTERN3_ACTUAL > 0)
                {
                    record.STD_HARDNESS_STRING = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(record.HARDNESS_PATTERN3_ACTUAL));
                }

                
            }

            return _ProcessDetails;

        }
        public List<ProcessPlanForSalesProductionStatusDetails> getMessageDetailsByProcessPlanCD(long processPlanCD)
        {
            var _ProcessDetails = _SalesProductionStatusSearchRepository.GetMessageListDetailsByProcessPlanCD(processPlanCD);

            foreach (ProcessPlanForSalesProductionStatusDetails record in _ProcessDetails)
            {
                //if (record.DECISION_RESULT > 0)
                //{
                //    record.PP_COLOR = UIUtils.GetColorKubunValue(Convert.ToString(record.DECISION_RESULT));
                //}
                if (record.DECISION > 0)
                {
                    record.RESULT_DECISION_STRING = UIUtils.GetResultKubunValue(Convert.ToString(record.DECISION));
                }
                if (record.NEXT_PROCESS > 0)
                {
                    record.PP_PROCESS_NAME = UIUtils.GetNextProcessName(Convert.ToString(record.NEXT_PROCESS));
                }
              


            }



            return _ProcessDetails;
        }
        public ProcessPlanForSalesProductionStatusDetails getFIFailureRateByProcessPlanCD(long processPlanCD)
        {
            var _FIDetails = _SalesProductionStatusSearchRepository.GetFIFailureRateDetailsByProcessPlanCD(processPlanCD);
            return _FIDetails;
        }
        public ProcessPlanForSalesProductionStatusDetails getECDefectRateByProcessPlanCD(long processPlanCD)
        {
            var _ECDefectDetails = _SalesProductionStatusSearchRepository.GetECDefectRateDetailsByProcessPlanCD(processPlanCD);
            var drawingDefectDetails = _SalesProductionStatusSearchRepository.GetDrawingDefectRateDetailsByProcessPlanCD(processPlanCD);
            if (drawingDefectDetails != null && _ECDefectDetails != null)
            {

                _ECDefectDetails.EC_OK_QTY = _ECDefectDetails.EC_OK_QTY + drawingDefectDetails.EC_OK_QTY;
                _ECDefectDetails.EC_NG_QTY = _ECDefectDetails.EC_NG_QTY + drawingDefectDetails.EC_NG_QTY;
            }else if (drawingDefectDetails != null && _ECDefectDetails == null)
            {
                _ECDefectDetails = new ProcessPlanForSalesProductionStatusDetails();
                _ECDefectDetails.EC_OK_QTY = drawingDefectDetails.EC_OK_QTY;
                _ECDefectDetails.EC_NG_QTY = drawingDefectDetails.EC_NG_QTY;
            }
                return _ECDefectDetails;
        }
        public ProcessPlanForSalesProductionStatusDetails getWIPByProcessPlanCD(long processPlanCD)
        {
            var _FIDetails = _SalesProductionStatusSearchRepository.GetWIPDetailsByProcessPlanCD(processPlanCD);
            return _FIDetails;
        }
        public void MessageDetailsSave(ProcessPlanForSalesProductionStatusDetails request)
        {
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
            }

            //request.PROCESS_KUBUN_CD = "";

            _SalesProductionStatusSearchRepository.MessageDetailsInsert(request);
        }
        public void MessageDetailsUpdate(ProcessPlanForSalesProductionStatusDetails request)
        {
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _UserLoginRepository.GetUserLocationById(request.USER_ID.Value);
            }

            //   request.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessNoNextProcess")));

            _SalesProductionStatusSearchRepository.MessageDetailsUpdate(request);
        }

        public void MessageDetailsDelete(long MSG_CD)
        {
           

            _SalesProductionStatusSearchRepository.MessageDetailsDelete(MSG_CD);
        }
    }
}
