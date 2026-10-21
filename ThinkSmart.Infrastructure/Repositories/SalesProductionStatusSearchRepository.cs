using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class SalesProductionStatusSearchRepository : BaseRepository, ISalesProductionStatusSearchRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly string specialChuuban;

        public SalesProductionStatusSearchRepository(LocalizationServices localizationServices,IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localizationServices;
            specialChuuban= Convert.ToString(_configuration.GetSection("SpecialChuuban").Value);
        }

        public List<SalesProductionStatusSearchDetails> GetSalesProductionStatusList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD)
        {
            List<SalesProductionStatusSearchDetails> result = new List<SalesProductionStatusSearchDetails>();
            int decisionValue = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            // string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, trim(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            selectQuery.Append(",CST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(",EMP.SHORT_NAME AS PP_EMPLOYEE_NAME");
            selectQuery.Append(",TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME");
            selectQuery.Append(",PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(",NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(",CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL AND PP.DIM_TOL_MIN_CHR IS NOT NULL THEN PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ELSE NVL(PP.DIM_TOL_MAX_CHR , PP.DIM_TOL_MIN_CHR) END AS PP_DIM_TOLERANCE");
            selectQuery.Append(",CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL AND PP.LENGTH_TOL_MIN_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR ||','||PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE NVL(PP.LENGTH_TOL_MAX_CHR , PP.LENGTH_TOL_MIN_CHR) END AS PP_LENGTH_TOL");
            selectQuery.Append(",PP.MAGARI_KUBUN AS PP_MAGARI_KUBUN");
            selectQuery.Append(",PP.MAGARI_SPAN AS PP_MAGARI_SPAN");
            selectQuery.Append(",PP.MAGARI_BEND_TOL AS PP_BEND_TOL");
            selectQuery.Append(",PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(",TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(",PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(",CONDITION1.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(",PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(",PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(",PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO");

            // selectQuery.Append(",CH_FINISHED_QTY(PP.PROCESS_PLAN_CD) AS PP_FINISHED_QTY"); // trn_Prod_Nyuka table not availale in windows
            // selectQuery.Append(", FI.FINISHED_QTY AS PP_FINISHED_QTY ");
           // selectQuery.Append(", (SELECT SUM(WIP_QTY) FROM TRN_WIP_STATUS WHERE PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ) AS FINISHED_QTY ");
            selectQuery.Append(", FI.FINISHED_QTY AS PP_FINISHED_QTY");
            selectQuery.Append(", CASE WHEN FI.FINISHED_QTY > 0 AND PP.ACT_ALLOC_QTY > 0 THEN TO_CHAR((FI.FINISHED_QTY/ PP.ACT_ALLOC_QTY) * 100, 'FM999999990.00') || '%' END AS SUCCESS_PERCENTAGE ");
            selectQuery.Append(",TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE, 'yyyy/mm/dd') AS PROCESS_PLAN_CMPL_DATE_STRING");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("  LEFT JOIN( ");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD, ");
            selectQuery.Append(" NVL(SUM(FI.FI_QTY), 0) AS FINISHED_QTY ");
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION   FI ");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN       PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" AND FI.COMBINED_DECISION = " + decisionValue);
            selectQuery.Append("  GROUP BY PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" ) FI ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD  ");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP MDG ON PP.DEPARTMENT_GROUP_CD = MDG.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.PROD_CONDITION_CD ");

            selectQuery.Append(" WHERE ");
            //selectQuery.Append(" (CASE");
            //selectQuery.Append(" WHEN (ROWNUM >" + _maxRowLimit + ")  THEN " + _maxRowLimit + "");
            //selectQuery.Append(" ELSE ROWNUM");
            //selectQuery.Append(" END)");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " AND ";
                string bindQuery = string.Empty;
                int i = 0;
                foreach (var searchField in searchCriteria)
                {

                    i = i + 1;
                    if (i == 1)
                    {
                        bindQuery = DynamicSearchProvider.ApplySearchCriteria(searchField, query);

                    }
                    else
                    {
                        bindQuery += whereCondition + " " + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    }
                }
                query = query + bindQuery;
                query = query + " AND CONCAT(PP.CHUUBAN,PP.GYOUBAN) NOT IN (" + specialChuuban + ")"; // EXCEPT SPECIAL TYPE PROCESS PLANCE CD SKIPPED.
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += " ORDER BY PP_CHUUBAN_GYOUBAN DESC, PP.PROD_SHAPE ASC, PP.PROD_NAME_CUST ASC, PP.PROD_DIM_D ASC, PP.PROD_LENGTH ASC";
            }
            

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<SalesProductionStatusSearchDetails>(query);
            return result;
        }

        public ProcessPlanForSalesProductionStatusDetails GetSalesStatusDetailsByProcessPlanCD(long processPlanCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(",TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            selectQuery.Append(",CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(",EMP.SHORT_NAME AS PP_EMPLOYEE_NAME");
            selectQuery.Append(",RE.SHORT_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(",PP.CHAMFER AS PP_CHAMFER");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");

            selectQuery.Append(",PP.COLOR_ID  AS PP_COLOR");
            selectQuery.Append(",PP.MATL_DIM_D ||' '|| PP.MATL_SHAPE  AS PP_MATL_DIM_D");
            selectQuery.Append(",PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(",PP.PROD_DIM_D ||' '|| PP.PROD_SHAPE AS  PP_PROD_DIM_D");
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(",PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_PROD_NAME_INT");
            selectQuery.Append(",PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(",PP.MAGARI_KUBUN AS PP_MAGARI_KUBUN");
            selectQuery.Append(",PP.MAGARI_SPAN AS PP_MAGARI_SPAN");
            selectQuery.Append(",PP.MAGARI_BEND_TOL AS PP_BEND_TOL");
            selectQuery.Append(",PP.EXT_TEXTURE AS PP_EXT_TEXTURE");

            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");

            selectQuery.Append(",PP.DISPLAY AS PP_DISPLAY");
            selectQuery.Append(",PP.STAMP AS PP_STAMP");
            selectQuery.Append(",PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(",NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(",PCK.PACKING_NAME  AS PP_PACKING_DECODE ");

            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(",PP.STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(",PP.NO_OF_UNITS AS PP_NO_OF_UNITS");
            selectQuery.Append(",PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(",PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL");
            selectQuery.Append(",PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL");
            selectQuery.Append(",PP.COIL_DIA_WINDING AS COIL_DIA_WINDING");
            selectQuery.Append(",PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO");
            selectQuery.Append(",PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(",PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK");
          //  selectQuery.Append(",PRODUCT_WIP_QTY(PP.PRODUCT_CD) AS WIP"); not used on screen hence commented on dated 2026-07-14
            //selectQuery.Append(",CH_FINISHED_QTY(PP.PROCESS_PLAN_CD) AS FI_QTY");

            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(",PP.NYUKA_CD AS NYUKA_CD ");
            selectQuery.Append(", COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            // selectQuery.Append(",MKR.PDF_FILE_PATH AS MAKER_MILLSHEET_PDF_FILE_PATH");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP  ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD  ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            // selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INT ON INT.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K  ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD "); // TRN_KAKOUIRAI table not availale in windows
            //selectQuery.Append(" LEFT JOIN HSKUSER.TRN_PROD_ORDER PROD  ");
            //selectQuery.Append(" ON PROD.CHUUBAN = TO_CHAR(pp.CHUUBAN) AND PROD.GYOUBAN = TO_CHAR(PP.GYOUBAN, 'FM00') AND  ");
            //selectQuery.Append(" ( CASE WHEN PROD.CHHUBAN_YEAR=0 AND SUBSTR(PROD.CREATED_DATETIME,1,4)= 9 THEN  ");
            //selectQuery.Append(" TO_CHAR(SUBSTR(ADD_MONTHS(TO_DATE(PROD.CREATED_DATETIME),12),3,2),'FM00') ELSE ");
            //selectQuery.Append(" TO_CHAR((SUBSTR(PROD.CREATED_DATETIME,3,1)||PROD.CHHUBAN_YEAR),'FM00') ");
            //selectQuery.Append(" END ) =  PP.CHUUBAN_YEAR ");
            //selectQuery.Append(" LEFT JOIN HSKUSER.MAKER_MILL_SHEET MKR ON MKR.CHARGE= PROD.CHARGE AND MKR.DIM_D=PROD.MATL_DIM_D  ");

            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD = " + processPlanCD);//PROCESS PLAN CD  

            return ExecuteQuerySingleResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
           

        }

        public List<ProcessPlanForSalesProductionStatusDetails> GetProcessDataDetailsByProcessPlanCD(long processPlanCD)
        {
            List<ProcessPlanForSalesProductionStatusDetails> result = new List<ProcessPlanForSalesProductionStatusDetails>();

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            //selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append("  P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(" ,P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(" ,P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(" ,P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(" ,P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(" ,P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(" ,P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(" ,P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(" ,P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(" ,P10.PROCESS_NAME AS PP_PROCESS_10 ");
            selectQuery.Append(" ,P11.PROCESS_NAME AS PP_PROCESS_11 ");
            selectQuery.Append(" ,P12.PROCESS_NAME AS PP_PROCESS_12 ");
            selectQuery.Append(" ,P13.PROCESS_NAME AS PP_PROCESS_13 ");

            selectQuery.Append(" ,PP.PROD_POINT_1 AS PP_PROD_POINT_1 ");
            selectQuery.Append(" ,PP.PROD_POINT_2 AS PP_PROD_POINT_2 ");
            selectQuery.Append(" ,PP.PROD_POINT_3 AS PP_PROD_POINT_3 ");
            selectQuery.Append(" ,PP.PROD_POINT_4 AS PP_PROD_POINT_4 ");
            selectQuery.Append(" ,PP.PROD_POINT_5 AS PP_PROD_POINT_5 ");
            selectQuery.Append(" ,PP.PROD_POINT_6 AS PP_PROD_POINT_6 ");
            selectQuery.Append(" ,PP.PROD_POINT_7 AS PP_PROD_POINT_7 ");
            selectQuery.Append(" ,PP.PROD_POINT_8 AS PP_PROD_POINT_8 ");
            selectQuery.Append(" ,PP.PROD_POINT_9 AS PP_PROD_POINT_9 ");
            selectQuery.Append(" ,PP.PROD_POINT_10 AS PP_PROD_POINT_10 ");
            selectQuery.Append(" ,PP.PROD_POINT_11 AS PP_PROD_POINT_11 ");
            selectQuery.Append(" ,PP.PROD_POINT_12 AS PP_PROD_POINT_12 ");
            selectQuery.Append(" ,PP.PROD_POINT_13 AS PP_PROD_POINT_13 ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD = " + processPlanCD);
            // selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL"); commented by Rahul 2024/06/21 confirmed by Nilesh Sir
            result = ExecuteQueryMultiResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;
        }

        public ProcessPlanForSalesProductionStatusDetails GetTimeAndBundlewiseDetailsByProcessPlanCD(long processPlanCD)
        {
           ProcessPlanForSalesProductionStatusDetails result = new ProcessPlanForSalesProductionStatusDetails();

            string timewiseQuery = "SELECT * FROM TABLE(GET_SALES_PRODUCTION_TIMEWISE_BY_PROCESS_PLAN_CD(:PROCESS_PLAN))";
            string bundlewiseQuery = "SELECT * FROM TABLE(GET_SALES_PRODUCTION_BUNDLEWISE_BY_PROCESS_PLAN_CD(:PROCESS_PLAN))";

            var timewiseData = ExecuteQuerySalesProduction<ProcessPlanForSalesProductionStatusDetails>(timewiseQuery, Convert.ToInt32(processPlanCD));
            var bundlewiseData = ExecuteQuerySalesProduction<ProcessPlanForSalesProductionStatusDetails>(bundlewiseQuery, Convert.ToInt32(processPlanCD));
                       
                result.GETDATATIMEWISE = timewiseData.AsEnumerable().OrderBy(r => r.END_DATE).ThenBy(r=>r.TABA_NO).ToList();
           
                result.GETDATABUNDLEWISE = bundlewiseData;
           
            return result;
        }

        public List<ProcessPlanForSalesProductionStatusDetails> GetHardnessAndPostHTDetailsByProcessPlanCD(long processPlanCD)
        {
            List<ProcessPlanForSalesProductionStatusDetails> result = new List<ProcessPlanForSalesProductionStatusDetails>();
            //var pross_plan = GetProcessPlanCD(Convert.ToInt32(internalQCD));
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT* FROM ( ");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" TO_CHAR(MI.TS_TENSILE,'FM999990.000') AS TENSILE_STRENGTH_FORMAT");
            selectQuery.Append(",TO_CHAR(MI.TS_ELONGATION,'FM9990.0') AS ELONGATION_STRING"); 
            selectQuery.Append(",ROUND(MI.TS_YIELD_STRENGTH) AS YIELD_POINT");
            selectQuery.Append(",TO_CHAR(MI.TS_REDUCTION, 'FM9990.0') AS REDUCTION_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_GAUGE_LENGTH, 'FM999990.00') AS GAUGE_LENGTH_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_MAX_LOAD_N,'FM999999990.0') AS MAX_LOAD_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_EXTENDED_LENGTH,'FM99990.00') AS CUTTING_ELONGATION_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_CUT_DIM,'FM9990.00') AS FRACTURE_DIM_STRING");
            selectQuery.Append(",PP.TEST_SPECIMEN"); 
            selectQuery.Append(",PP.TENSILE_STRN_PATTERN1");
            selectQuery.Append(",PP.TENSILE_STRN_PATTERN1_MIN_MAX");
            selectQuery.Append(",PP.HARDNESS_PATTERN1");
            selectQuery.Append(",PP.HARDNESS_PATTERN1_MIN_MAX");
            selectQuery.Append(",PP.HARDNESS_PATTERN1_CODE ");
            selectQuery.Append(",PP.HARDNESS_PATTERN2_MAX AS HARDNESS_MAX ");
            selectQuery.Append(",PP.HARDNESS_PATTERN2_MIN AS HARDNESS_MIN ");            
            
            selectQuery.Append(",PP.HARDNESS_PATTERN2_CODE  ");
            selectQuery.Append(",PP.HARDNESS_PATTERN3_ACTUAL ");

            selectQuery.Append(",PP.ELONGATION_PATTERN1 ");
            selectQuery.Append(",PP.ELONGATION_PATTERN1_MIN_MAX ");
            
            selectQuery.Append(",PP.PRESS_PATTERN1 ");
            selectQuery.Append(",PP.PRESS_PATTERN1_MIN_MAX ");
            selectQuery.Append(",TO_CHAR(HRD.HD_SUTEOSHI,'FM99990.0') AS CENTER_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_1, 'FM99990.0') AS D4_1_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_2, 'FM99990.0') AS D4_2_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_3, 'FM99990.0') AS D4_3_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_4, 'FM99990.0') AS D4_4_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_AVG, 'FM99990.00') AS D4_AVG_STRING");
            selectQuery.Append(", HRD.DETAIL_NO AS DETAIL_NO "); 
            //selectQuery.Append(",HRD.UPDATED_DATE AS UPDATED_DATE");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP  ");
            selectQuery.Append(" INNER JOIN TRN_MI_HARDNESS HRD ON HRD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MI_TENSILE_STRENGTH   MI ON MI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD AND HRD.DETAIL_NO = MI.DETAIL_NO ");
            //selectQuery.Append(" LEFT JOIN MST_JIS_TEST_SPEC JTS ON JTS.JIS_TEST_SPEC_CD = MI.TEST_SPECIMEN_CD"); // MST_JIS_TEST_SPEC table not availale in windows

            selectQuery.Append(" WHERE 1 = 1 AND PP.PROCESS_PLAN_CD = " + processPlanCD);
            //   selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL"); commented by Rahul 2024/06/21 confirmed by Nilesh Sir

            selectQuery.Append(" UNION ");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" TO_CHAR(MI.TS_TENSILE,'FM999990.000') AS TENSILE_STRENGTH_FORMAT");
            selectQuery.Append(",TO_CHAR(MI.TS_ELONGATION,'FM9990.0') AS ELONGATION_STRING");
            selectQuery.Append(",ROUND(MI.TS_YIELD_STRENGTH) AS YIELD_POINT");
            selectQuery.Append(",TO_CHAR(MI.TS_REDUCTION, 'FM9990.0') AS REDUCTION_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_GAUGE_LENGTH, 'FM999990.00') AS GAUGE_LENGTH_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_MAX_LOAD_N,'FM999999990.0') AS MAX_LOAD_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_EXTENDED_LENGTH,'FM99990.00') AS CUTTING_ELONGATION_STRING");
            selectQuery.Append(",TO_CHAR(MI.TS_CUT_DIM,'FM9990.00') AS FRACTURE_DIM_STRING");
            selectQuery.Append(",PP.TEST_SPECIMEN");
            selectQuery.Append(",PP.TENSILE_STRN_PATTERN1");
            selectQuery.Append(",PP.TENSILE_STRN_PATTERN1_MIN_MAX");
            selectQuery.Append(",PP.HARDNESS_PATTERN1");
            selectQuery.Append(",PP.HARDNESS_PATTERN1_MIN_MAX");
            selectQuery.Append(",PP.HARDNESS_PATTERN1_CODE AS STD_HARDNESS");
            selectQuery.Append(",PP.HARDNESS_PATTERN2_MAX AS HARDNESS_MAX");
            selectQuery.Append(",PP.HARDNESS_PATTERN2_MIN AS HARDNESS_MIN");
            selectQuery.Append(",PP.HARDNESS_PATTERN2_CODE  ");
            selectQuery.Append(",PP.HARDNESS_PATTERN3_ACTUAL ");
            selectQuery.Append(",PP.ELONGATION_PATTERN1 ");
            selectQuery.Append(",PP.ELONGATION_PATTERN1_MIN_MAX ");
            selectQuery.Append(",PP.PRESS_PATTERN1 ");
            selectQuery.Append(",PP.PRESS_PATTERN1_MIN_MAX ");
            selectQuery.Append(",TO_CHAR(HRD.HD_SUTEOSHI,'FM99990.0') AS CENTER_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_1, 'FM99990.0') AS D4_1_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_2, 'FM99990.0') AS D4_2_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_3, 'FM99990.0') AS D4_3_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_4, 'FM99990.0') AS D4_4_STRING");
            selectQuery.Append(",TO_CHAR(HRD.HD_D4_AVG, 'FM99990.00') AS D4_AVG_STRING");
            //selectQuery.Append(",MI.UPDATED_DATE AS UPDATED_DATE");
            selectQuery.Append(", MI.DETAIL_NO AS DETAIL_NO ");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP  ");
            selectQuery.Append(" INNER JOIN TRN_MI_TENSILE_STRENGTH   MI ON MI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MI_HARDNESS HRD ON HRD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD AND HRD.DETAIL_NO = MI.DETAIL_NO ");
            //selectQuery.Append(" LEFT JOIN MST_JIS_TEST_SPEC JTS ON JTS.JIS_TEST_SPEC_CD = MI.TEST_SPECIMEN_CD"); // MST_JIS_TEST_SPEC table not availale in windows

            selectQuery.Append(" WHERE 1 = 1 AND PP.PROCESS_PLAN_CD = " + processPlanCD);
            //  selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL"); commented by Rahul 2024/06/21 confirmed by Nilesh Sir
            //selectQuery.Append(" )ORDER BY UPDATED_DATE DESC  ");

            result = ExecuteQueryMultiResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;

        }
        public List<ProcessPlanForSalesProductionStatusDetails> GetMessageListDetailsByProcessPlanCD(long processPlanCD)
        {
            List<ProcessPlanForSalesProductionStatusDetails> result = new List<ProcessPlanForSalesProductionStatusDetails>();
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT");
            //selectQuery.Append(" MSG.MSG_CD ");
            //selectQuery.Append(",MSG.PROCESS_PLAN_CD ");
            //selectQuery.Append(",TO_CHAR(MSG.CREATED_DATE, 'yyyy/mm/dd HH24:MM') AS MESSAGE_DATE_TIME");
            //selectQuery.Append(",KUB.KUBUN_NAME AS PROCESS_NAME_JP");
            //selectQuery.Append(",KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN");
            //selectQuery.Append(",MAC.SHORT_NAME AS MACHINE_NAME");
            //selectQuery.Append(",MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            //selectQuery.Append(",MSG.OPERATOR_CD");
            //selectQuery.Append(",(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = OPERATOR_CD) AS END_OPERATOR_NAME  ");
            //selectQuery.Append(",MSG.MSG_CONTENTS AS MESSAGE");

            //selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            //selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = MSG.MACHINE_CD");
            //selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = MSG.PROCESS_KUBUN_ID");
            //selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);
            //selectQuery.Append(" ORDER BY MSG.CREATED_DATE DESC  ");

           
            // Bellow Code for TRN_MESSAGE  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    NULL AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    NULL AS PROCESS_NAME_JP, ");
            selectQuery.Append("    NULL AS PROCESS_NAME_EN, ");
            selectQuery.Append("    NULL AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NULL AS DEFECT_COUNT, ");
            selectQuery.Append("    NULL AS TABA_NO, ");
            selectQuery.Append("    NULL AS WIP_COUNT, ");
            selectQuery.Append("    NULL AS WIP_WEIGHT, ");
            selectQuery.Append("    NULL AS DECISION, ");
            selectQuery.Append("    NULL AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_KUBUN_ID IS NULL ");
            selectQuery.Append("AND MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_DRAWING  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    TDD.DRAWING_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NVL(TDD.DW_NG_BARS1,0) + NVL(TDD.DW_NG_BARS2,0) + NVL(TDD.DW_NG_BARS3,0) AS DEFECT_COUNT, ");
            selectQuery.Append("    RTRIM( ");
            selectQuery.Append("        CASE WHEN TDD.DW_TABA_NO_1 IS NOT NULL THEN TDD.DW_TABA_NO_1 || ',' END || ");
            selectQuery.Append("        CASE WHEN TDD.DW_TABA_NO_2 IS NOT NULL THEN TDD.DW_TABA_NO_2 || ',' END || ");
            selectQuery.Append("        CASE WHEN TDD.DW_TABA_NO_3 IS NOT NULL THEN TDD.DW_TABA_NO_3 || ',' END || ");
            selectQuery.Append("        CASE WHEN TDD.DW_TABA_NO_4 IS NOT NULL THEN TDD.DW_TABA_NO_4 || ',' END, ");
            selectQuery.Append("        ',' ");
            selectQuery.Append("    ) AS TABA_NO, ");
            selectQuery.Append("    NVL(TDD.DW_FINISHED_BARS,0) AS WIP_COUNT, ");
            selectQuery.Append("    NVL(TDD.DW_FINISHED_QTY,0) AS WIP_WEIGHT, ");
            selectQuery.Append("    TDD.DW_DECISION AS DECISION, ");
            selectQuery.Append("    TDD.DW_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_DRAWING TDD ");
            selectQuery.Append("    ON MSG.MSG_CD = TDD.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = TDD.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = TDD.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_STAMPING  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    TS.STAMPING_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NVL(TS.ST_NG_BARS1,0) + NVL(TS.ST_NG_BARS2,0) + NVL(TS.ST_NG_BARS3,0) AS DEFECT_COUNT, ");
            selectQuery.Append("    TO_CHAR(TS.ST_TABA_NO) AS TABA_NO, ");
            selectQuery.Append("    NVL(TS.ST_OK_BARS,0) AS WIP_COUNT, ");
            selectQuery.Append("    NVL(TS.ST_OK_QTY,0) AS WIP_WEIGHT, ");
            selectQuery.Append("    TS.ST_DECISION AS DECISION, ");
            selectQuery.Append("    TS.ST_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_STAMPING TS ");
            selectQuery.Append("    ON MSG.MSG_CD = TS.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = TS.PROCESS_KUBUN_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = TS.MACHINE_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_DRAWING_D  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    TDD.DRAWING_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NVL(TDD.DW_NG_BARS1,0) + NVL(TDD.DW_NG_BARS2,0) + NVL(TDD.DW_NG_BARS3,0) AS DEFECT_COUNT, ");
            selectQuery.Append("    TO_CHAR(TDD.DW_TABA_NO_) AS TABA_NO, ");
            selectQuery.Append("    NVL(TDD.DW_FINISHED_BARS,0) AS WIP_COUNT, ");
            selectQuery.Append("    NVL(TDD.DW_FINISHED_QTY,0) AS WIP_WEIGHT, ");
            selectQuery.Append("    TDD.DW_DECISION AS DECISION, ");
            selectQuery.Append("    TDD.DW_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_DRAWING_D TDD ");
            selectQuery.Append("    ON MSG.MSG_CD = TDD.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = TDD.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = TDD.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");
            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_STRAIGHTENING  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    TS.STRAIGHTENING_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NVL(TS.STR_NG_BARS1,0) + NVL(TS.STR_NG_BARS2,0) + NVL(TS.STR_NG_BARS3,0) AS DEFECT_COUNT, ");
            selectQuery.Append("    RTRIM( ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_1 IS NOT NULL THEN TS.STR_TABA_NO_1 || ',' END || ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_2 IS NOT NULL THEN TS.STR_TABA_NO_2 || ',' END || ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_3 IS NOT NULL THEN TS.STR_TABA_NO_3 || ',' END || ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_4 IS NOT NULL THEN TS.STR_TABA_NO_4 || ',' END, ");
            selectQuery.Append("        ',' ");
            selectQuery.Append("    ) AS TABA_NO, ");
            selectQuery.Append("    NVL(TS.STR_BARS,0) AS WIP_COUNT, ");
            selectQuery.Append("    NVL(TS.STR_QTY,0) AS WIP_WEIGHT, ");
            selectQuery.Append("    TS.STR_DECISION AS DECISION, ");
            selectQuery.Append("    TS.STR_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_STRAIGHTENING TS ");
            selectQuery.Append("    ON MSG.MSG_CD = TS.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = TS.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = TS.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_EC  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    TE.EC_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NVL(TE.EC_NG_BARS1,0) + NVL(TE.EC_NG_BARS2,0) + NVL(TE.EC_NG_BARS3,0) AS DEFECT_COUNT, ");
            selectQuery.Append("    TO_CHAR(TE.EC_TABA_NO) AS TABA_NO, ");
            selectQuery.Append("    NVL(TE.EC_OK_BARS,0) AS WIP_COUNT, ");
            selectQuery.Append("    NVL(TE.EC_OK_QTY,0) AS WIP_WEIGHT, ");
            selectQuery.Append("    TE.EC_DECISION AS DECISION, ");
            selectQuery.Append("    TE.EC_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_EC TE ");
            selectQuery.Append("    ON MSG.MSG_CD = TE.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = TE.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = TE.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");
            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_STRAIGHTENING2  TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    TS.STRAIGHTENING_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NVL(TS.STR_NG_BARS1,0) + NVL(TS.STR_NG_BARS1,0) + NVL(TS.STR_NG_BARS1,0) AS DEFECT_COUNT, ");
            selectQuery.Append("    RTRIM( ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_1 IS NOT NULL THEN TS.STR_TABA_NO_1 || ',' END || ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_2 IS NOT NULL THEN TS.STR_TABA_NO_2 || ',' END || ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_3 IS NOT NULL THEN TS.STR_TABA_NO_3 || ',' END || ");
            selectQuery.Append("        CASE WHEN TS.STR_TABA_NO_4 IS NOT NULL THEN TS.STR_TABA_NO_4 || ',' END, ");
            selectQuery.Append("        ',' ");
            selectQuery.Append("    ) AS TABA_NO, ");
            selectQuery.Append("    NVL(TS.STR_BARS,0) AS WIP_COUNT, ");
            selectQuery.Append("    NVL(TS.STR_QTY,0) AS WIP_WEIGHT, ");
            selectQuery.Append("    TS.STR_DECISION AS DECISION, ");
            selectQuery.Append("    TS.STR_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_STRAIGHTENING2 TS ");
            selectQuery.Append("    ON TS.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = TS.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = TS.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_HEAT_TREATMENT TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("    MSG.MSG_CD, ");
            selectQuery.Append("    THT.HEAT_TREATMENT_CD AS PROCESS_PK, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("    TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("    KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("    KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("    MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("    MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("    MSG.OPERATOR_CD, ");
            selectQuery.Append("    (SELECT EM.SHORT_NAME ");
            selectQuery.Append("       FROM MST_EMPLOYEE EM ");
            selectQuery.Append("      WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("    MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("    NULL AS DEFECT_COUNT, ");
            selectQuery.Append("    RTRIM( ");
            selectQuery.Append("        CASE WHEN THT.HT_TABA_NO_1 IS NOT NULL THEN THT.HT_TABA_NO_1 || ',' END || ");
            selectQuery.Append("        CASE WHEN THT.HT_TABA_NO_2 IS NOT NULL THEN THT.HT_TABA_NO_2 || ',' END || ");
            selectQuery.Append("        CASE WHEN THT.HT_TABA_NO_3 IS NOT NULL THEN THT.HT_TABA_NO_3 || ',' END || ");
            selectQuery.Append("        CASE WHEN THT.HT_TABA_NO_4 IS NOT NULL THEN THT.HT_TABA_NO_4 || ',' END, ");
            selectQuery.Append("        ',' ");
            selectQuery.Append("    ) AS TABA_NO, ");
            selectQuery.Append("    NULL AS WIP_COUNT, ");
            selectQuery.Append("    THT.HT_WEIGHT AS WIP_WEIGHT, ");
            selectQuery.Append("    NULL AS DECISION, ");
            selectQuery.Append("    THT.HT_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append("    ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_HEAT_TREATMENT THT ");
            selectQuery.Append("    ON THT.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ");
            selectQuery.Append("    ON MAC.MACHINE_CD = THT.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ");
            selectQuery.Append("    ON KUB.KUBUN_ID = THT.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD + " ");

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_FINAL_INSPECTION TABLE

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" TFI.FINAL_INSP_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" NULL AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME ");
            selectQuery.Append("    FROM MST_EMPLOYEE EM ");
            selectQuery.Append("   WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" TO_CHAR(TFI.FI_TABA_NO) AS TABA_NO, ");
            selectQuery.Append(" NVL(TFI.NO_OF_BARS, NULL) AS WIP_COUNT, ");
            selectQuery.Append(" NVL(TFI.FI_QTY, 0) AS WIP_WEIGHT, ");
            selectQuery.Append(" TFI.COMBINED_DECISION AS DECISION, ");
            selectQuery.Append(" TFI.FI_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION TFI ON TFI.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = TFI.INSP_PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_HARDENING_TEMPERING TABLE

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" THT.HARDENING_TEMPERING_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" NULL AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME ");
            selectQuery.Append("    FROM MST_EMPLOYEE EM ");
            selectQuery.Append("   WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" TO_CHAR(THT.TABA_NO) AS TABA_NO, ");
            selectQuery.Append(" NVL(THT.NO_OF_UNITS, 0) AS WIP_COUNT, ");
            selectQuery.Append(" NVL(THT.WEIGHT, 0) AS WIP_WEIGHT, ");
            selectQuery.Append(" NULL AS DECISION, ");
            selectQuery.Append(" THT.HT_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_HARDENING_TEMPERING THT ON THT.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = THT.PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_HT_QUALITY TABLE
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" THQ.HT_QUALITY_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" NULL AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME ");
            selectQuery.Append("    FROM MST_EMPLOYEE EM ");
            selectQuery.Append("   WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" NULL AS TABA_NO, ");
            selectQuery.Append(" NULL AS WIP_COUNT, ");
            selectQuery.Append(" NULL AS WIP_WEIGHT, ");
            selectQuery.Append(" THQ.DECISION AS DECISION, ");
            selectQuery.Append(" THQ.HTQ_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_HT_QUALITY THQ ON THQ.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = THQ.PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_CHAMFERING TABLE
            selectQuery.Append("SELECT ");
            selectQuery.Append("MSG.MSG_CD, ");
            selectQuery.Append("TC.CHAMFERING_CD AS PROCESS_PK, ");
            selectQuery.Append("MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append("TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append("KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append("KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append("MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append("MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("MSG.OPERATOR_CD, ");
            selectQuery.Append("(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append("MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append("NULL AS DEFECT_COUNT, ");
            selectQuery.Append("RTRIM(");
            selectQuery.Append("CASE WHEN TC.CH_TABA_NO_1 IS NOT NULL THEN TC.CH_TABA_NO_1 || ',' END || ");
            selectQuery.Append("CASE WHEN TC.CH_TABA_NO_2 IS NOT NULL THEN TC.CH_TABA_NO_2 || ',' END || ");
            selectQuery.Append("CASE WHEN TC.CH_TABA_NO_3 IS NOT NULL THEN TC.CH_TABA_NO_3 || ',' END || ");
            selectQuery.Append("CASE WHEN TC.CH_TABA_NO_4 IS NOT NULL THEN TC.CH_TABA_NO_4 || ',' END, ");
            selectQuery.Append("',') AS TABA_NO, ");
            selectQuery.Append("NVL(TC.CH_FINISHED_BARS, 0) AS WIP_COUNT, ");
            selectQuery.Append("NVL(TC.CH_FINISHED_QTY, 0) AS WIP_WEIGHT, ");
            selectQuery.Append("TC.CH_DECISION AS DECISION, ");
            selectQuery.Append("TC.CH_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append("FROM TRN_MESSAGE MSG ");
            selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN TRN_CHAMFERING TC ON TC.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append("INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = TC.MACHINE_CD ");
            selectQuery.Append("INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = TC.PROCESS_KUBUN_CD ");
            selectQuery.Append("WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_DRAWING_W TABLE
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" TDW.DRAWING_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" RTRIM(");
            selectQuery.Append(" CASE WHEN TDW.DW_TABA_NO_1 IS NOT NULL THEN TDW.DW_TABA_NO_1 || ',' END || ");
            selectQuery.Append(" CASE WHEN TDW.DW_TABA_NO_2 IS NOT NULL THEN TDW.DW_TABA_NO_2 || ',' END || ");
            selectQuery.Append(" CASE WHEN TDW.DW_TABA_NO_3 IS NOT NULL THEN TDW.DW_TABA_NO_3 || ',' END || ");
            selectQuery.Append(" CASE WHEN TDW.DW_TABA_NO_4 IS NOT NULL THEN TDW.DW_TABA_NO_4 || ',' END, ");
            selectQuery.Append(" ',' ) AS TABA_NO, ");
            selectQuery.Append(" NVL(TDW.DW_FINISHED_COILS, 0) AS WIP_COUNT, ");
            selectQuery.Append(" NVL(TDW.DW_FINISHED_QTY, 0) AS WIP_WEIGHT, ");
            selectQuery.Append(" TDW.DW_DECISION AS DECISION, ");
            selectQuery.Append(" TDW.DW_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_DRAWING_W TDW ON TDW.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = TDW.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = TDW.PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_INTERNAL_QUALITY TABLE
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" TIQ.INTERNAL_QUALITY_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" MAC.SHORT_NAME AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" NULL AS TABA_NO, ");
            selectQuery.Append(" NULL AS WIP_COUNT, ");
            selectQuery.Append(" NULL AS WIP_WEIGHT, ");
            selectQuery.Append(" TIQ.INTERNAL_QUALITY_DECISION AS DECISION, ");
            selectQuery.Append(" TIQ.IQ_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_INTERNAL_QUALITY TIQ ON TIQ.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = TIQ.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = TIQ.PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_MI_HARDNESS TABLE
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" TMH.MI_HARDNESS_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" NULL AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" NULL AS TABA_NO, ");
            selectQuery.Append(" NULL AS WIP_COUNT, ");
            selectQuery.Append(" NULL AS WIP_WEIGHT, ");
            selectQuery.Append(" TMH.HD_DECISION AS DECISION, ");
            selectQuery.Append(" TMH.HD_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_MI_HARDNESS TMH ON TMH.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = TMH.PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_MI_TENSILE_STRENGTH TABLE

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" TMT.MI_TS_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" KUB.KUBUN_NAME AS PROCESS_NAME_JP, ");
            selectQuery.Append(" KUB.KUBUN_NAME_EN AS PROCESS_NAME_EN, ");
            selectQuery.Append(" NULL AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" NULL AS TABA_NO, ");
            selectQuery.Append(" NULL AS WIP_COUNT, ");
            selectQuery.Append(" NULL AS WIP_WEIGHT, ");
            selectQuery.Append(" TMT.TS_DECISION AS DECISION, ");
            selectQuery.Append(" TMT.TS_NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_MI_TENSILE_STRENGTH TMT ON TMT.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = TMT.PROCESS_KUBUN_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" UNION ");
            // Bellow Code for TRN_WIP_STATUS TABLE

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MSG.MSG_CD, ");
            selectQuery.Append(" TWS.WIP_STATUS_CD AS PROCESS_PK, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD, ");
            selectQuery.Append(" TO_CHAR(MSG.CREATED_DATE, 'YYYY/MM/DD HH24:MI') AS MESSAGE_DATE_TIME, ");
            selectQuery.Append(" NULL AS PROCESS_NAME_JP, ");
            selectQuery.Append(" NULL AS PROCESS_NAME_EN, ");
            selectQuery.Append(" NULL AS MACHINE_NAME, ");
            selectQuery.Append(" MSG.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" MSG.OPERATOR_CD, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD = MSG.OPERATOR_CD) AS END_OPERATOR_NAME, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS MESSAGE, ");
            selectQuery.Append(" NULL AS DEFECT_COUNT, ");
            selectQuery.Append(" TO_CHAR(TWS.TABA_NO) AS TABA_NO, ");
            selectQuery.Append(" NVL(TWS.WIP_NO_OF_UNITS, 0) AS WIP_COUNT, ");
            selectQuery.Append(" NVL(TWS.WIP_QTY, 0) AS WIP_WEIGHT, ");
            selectQuery.Append(" TWS.PRE_PROCESS_DECISION AS DECISION, ");
            selectQuery.Append(" TWS.NEXT_PROCESS AS NEXT_PROCESS ");
            selectQuery.Append(" FROM TRN_MESSAGE MSG ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN TRN_WIP_STATUS TWS ON TWS.MSG_CD = MSG.MSG_CD ");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD = " + processPlanCD);

            selectQuery.Append(" ORDER BY MESSAGE_DATE_TIME DESC");
            result = ExecuteQueryMultiResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;

        }

        public ProcessPlanForSalesProductionStatusDetails GetFIFailureRateDetailsByProcessPlanCD(long processPlanCD)
        {
            ProcessPlanForSalesProductionStatusDetails result = new ProcessPlanForSalesProductionStatusDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT");
            selectQuery.Append(" DISTINCT (CH_FINISHED_QTY(PP.PROCESS_PLAN_CD))  AS FINISHED_QTYS");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");


            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION FI ON PP.PROCESS_PLAN_CD =FI.PROCESS_PLAN_CD");

            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD = " + processPlanCD);

            result = ExecuteQuerySingleResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;

        }

        public ProcessPlanForSalesProductionStatusDetails GetECDefectRateDetailsByProcessPlanCD(long processPlanCD)
        {
            ProcessPlanForSalesProductionStatusDetails result = new ProcessPlanForSalesProductionStatusDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT");
            selectQuery.Append(" PP.PROCESS_PLAN_CD");
            selectQuery.Append(" ,NVL(SUM(EC.EC_OK_QTY),0) AS EC_OK_QTY");
            selectQuery.Append(" ,NVL(SUM(EC.EC_NG_BAR_TOTAL),0) AS EC_NG_QTY");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_EC EC ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD");

            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD = " + processPlanCD + " AND EC.EC_END_DATE IS NOT NULL");
            selectQuery.Append(" AND EC.UNIT_TYPE = 1");
            selectQuery.Append(" AND EC.EC_TABA_NO NOT IN (");
            selectQuery.Append(" SELECT DW_TABA_NO FROM");
            selectQuery.Append(" ( SELECT ");
            selectQuery.Append("  DW_TABA_NO_1 AS DW_TABA_NO ");
            selectQuery.Append(" FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append("  UNION ");
            selectQuery.Append("  SELECT DW_TABA_NO_2 AS DW_TABA_NO");
            selectQuery.Append("  FROM TRN_DRAWING WHERE  EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append(" UNION ");
            selectQuery.Append(" SELECT DW_TABA_NO_3 AS DW_TABA_NO FROM ");
            selectQuery.Append(" TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append(" UNION ");
            selectQuery.Append(" SELECT DW_TABA_NO_4 AS DW_TABA_NO FROM");
            selectQuery.Append(" TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append(" ) WHERE DW_TABA_NO IS NOT NULL");
            selectQuery.Append(" ) GROUP BY  PP.PROCESS_PLAN_CD  ");

            result = ExecuteQuerySingleResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;

        }

        public ProcessPlanForSalesProductionStatusDetails GetWIPDetailsByProcessPlanCD(long processPlanCD)
        {
            ProcessPlanForSalesProductionStatusDetails result = new ProcessPlanForSalesProductionStatusDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT");
            selectQuery.Append(" PP.PROCESS_PLAN_CD");
            selectQuery.Append(",PP.PROCESS_PLAN_CMPL_DATE");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");

            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append(" ");

            result = ExecuteQuerySingleResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;

        }

        public void MessageDetailsInsert(ProcessPlanForSalesProductionStatusDetails messageDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_MESSAGE ( ");
            insertQuery.Append("  MSG_CD ");
            insertQuery.Append(" ,PROCESS_PLAN_CD ");
             insertQuery.Append(", PROCESS_KUBUN_ID ");
            insertQuery.Append(", OPERATOR_CD ");
            insertQuery.Append(", MSG_DATE ");
            insertQuery.Append(", MSG_CONTENTS ");
            insertQuery.Append(", INPUT_LOCATION_CD ");
            insertQuery.Append(", USER_ID ");
            insertQuery.Append(", CREATED_DATE ");
            insertQuery.Append(", UPDATED_DATE ");

            insertQuery.Append(") VALUES (");
            insertQuery.Append("MESSAGE_SEQ.NEXTVAL");
            insertQuery.Append(" ,'" + messageDetails.PP_PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + messageDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",'" + messageDetails.END_OPERATOR_NAME + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(messageDetails.MESSAGE_DATE_TIME).ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')");
            insertQuery.Append(",'" + messageDetails.PP_PROCESS_PLAN_REMARK + "'");
            insertQuery.Append(",'" + messageDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + messageDetails.USER_ID + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());
        }
        public void MessageDetailsUpdate(ProcessPlanForSalesProductionStatusDetails messageDetails)
        {
        
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_MESSAGE SET ");
            updateQuery.Append(" MSG_DATE = TO_DATE('" + messageDetails.MESSAGE_DATE_TIME + "','YYYY/MM/DD HH24:MI') ");
            updateQuery.Append(",OPERATOR_CD = '" + messageDetails.END_OPERATOR_NAME + "'");
            updateQuery.Append(",MSG_CONTENTS = '" + messageDetails.PP_PROCESS_PLAN_REMARK + "'");
            updateQuery.Append(",INPUT_LOCATION_CD ='" + messageDetails.INPUT_LOCATION_CD + "'");
            updateQuery.Append(",USER_ID ='" + messageDetails.USER_ID + "'");
            updateQuery.Append(",UPDATED_DATE = ");
            updateQuery.Append("SYSDATE");
            updateQuery.Append(" WHERE ");       
            updateQuery.Append(" MSG_CD = '" + messageDetails.MSG_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());

           
        
        }
        public void MessageDetailsDelete(long MSG_CD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_MESSAGE ");
            deleteQuery.Append(" WHERE MSG_CD =" + MSG_CD + "");
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public ProcessPlanForSalesProductionStatusDetails GetDrawingDefectRateDetailsByProcessPlanCD(long processPlanCD)
        {
            ProcessPlanForSalesProductionStatusDetails result = new ProcessPlanForSalesProductionStatusDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" SUM(NVL(DW_FINISHED_BARS, 0)) AS FINISHED_BARS,");
            selectQuery.Append(" SUM(NVL(EC_NG_BAR_TOTAL, 0)) AS EC_NG_QTY, ");
            selectQuery.Append(" SUM(NVL(EC_NG_BAR_TOTAL, 0)) AS TOTAL_NG_BARS, ");
            selectQuery.Append(" ( SUM(NVL(DW_FINISHED_BARS, 0)) - ( SUM(NVL(DW_NG_BARS1, 0)) + SUM(NVL(DW_NG_BARS2, 0)) + SUM(NVL(DW_NG_BARS3, 0)) ");
            selectQuery.Append(" + SUM(NVL( EC_NG_BAR_TOTAL, 0)) ) ) AS EC_OK_QTY ");
            selectQuery.Append(" FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append(" AND DW_END_DATE IS NOT NULL");
            selectQuery.Append(" GROUP BY PROCESS_PLAN_CD");


            result = ExecuteQuerySingleResult<ProcessPlanForSalesProductionStatusDetails>(selectQuery.ToString());
            return result;

        }
    }
}
