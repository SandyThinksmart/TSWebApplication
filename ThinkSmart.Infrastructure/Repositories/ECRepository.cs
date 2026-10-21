using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ECRepository : BaseRepository, IECRepository
    {
        private readonly LocalizationServices _localization;
        private readonly IConfiguration _configuration;
        private readonly int days=0;
        public ECRepository(IConfiguration configuration,LocalizationServices localization) : base(configuration)
        {
            _localization = localization;
            _configuration = configuration;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }

        public int IsChuubanActive(string nifudaNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(PP.PROCESS_PLAN_CD) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_NIFUDA N ");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = N.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" UPPER(N.NIFUDA_NO) =  '" + nifudaNo.ToUpper() + "'");
            selectQuery.Append(" AND ");
            selectQuery.Append(" PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
           return Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        }
        public ECDetails GetECDetailsByChuuban(string nifudaNo, short? mechineCD)
        {

            int getActiveChuuban = IsChuubanActive(nifudaNo);
            int rowsCount = 0;

            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(" SELECT ");
            sQuery.Append(" COUNT(PP.PROCESS_PLAN_CD) ");
            sQuery.Append(" FROM TRN_NIFUDA N");
            sQuery.Append(" LEFT JOIN TRN_EC EC ON EC.PROCESS_PLAN_CD = N.PROCESS_PLAN_CD");
            sQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON N.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            sQuery.Append(" WHERE UPPER(N.NIFUDA_NO)= '" + nifudaNo.ToUpper() + "'");
            sQuery.Append(" AND EC.MACHINE_CD = '" + mechineCD + "' ");
            sQuery.Append(" AND EC.CREATED_DATE >= TO_DATE(SYSDATE)-" + days);
            if (getActiveChuuban > 0)
            {
                sQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            }

            rowsCount = Convert.ToInt32(ExecuteScalar(sQuery.ToString()));

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" EC.EC_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(", PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE"); 
            selectQuery.Append(", PP.PROCESS_PLAN_START_DATE AS PP_PROCESS_PLAN_PRINT_DATE_STR");
            selectQuery.Append(", N.NIFUDA_COMPLETE_DATE AS NIFUDA_COMPLETE_DATE");
            selectQuery.Append(" ,N.NIFUDA_CD ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOBAN");
            selectQuery.Append(" ,PP.CHUUBAN  AS PP_CHUUBAN ");
            selectQuery.Append(" ,PP.GYOUBAN  AS PP_GYOUBAN ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            // selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            //     selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" , PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_OTHER ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING ");
            selectQuery.Append(" ,PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,EC.EC_START_DATE AS EC_START_DATE  ");
            selectQuery.Append(",TO_CHAR( EC.EC_START_DATE,'yyyy/mm/dd HH24:MI' ) AS EC_START_DATE_STRING");
            selectQuery.Append(",EC.EC_START_OPERATOR_CD AS EC_START_OPERATOR_CD");
            selectQuery.Append(",(SELECT EM.SHORT_NAME AS SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = EC_START_OPERATOR_CD) AS EC_START_OPERATOR_NAME");
            selectQuery.Append(" ,EC.EC_TABA_NO AS EC_TABA_NO  ");
            selectQuery.Append(" ,EC.TP_TEST AS TP_TEST  ");
            selectQuery.Append(" ,EC.EC_METALLIC_PLATE_CODE AS EC_METALLIC_PLATE_CODE  ");
            selectQuery.Append(" ,EC.ACT_ALLOC_UNITS ");
            selectQuery.Append(" ,EC.ACT_ALLOC_QTY AS ACT_ALLOC_QTY ");
            selectQuery.Append(" ,EC.IS_OK AS IS_OK ");
            selectQuery.Append(" ,EC.EC_COIL_SIZE ");
            selectQuery.Append(" ,EC.EC_COIL_SHAPE ");
            selectQuery.Append(" ,EC.FURNACE_NO ");
            selectQuery.Append(" ,EC.SHIKII_VALUE ");
            selectQuery.Append(" ,EC.LINE_SPEED ");
            selectQuery.Append(" ,EC.EC_ACT_DIM1 ");
            selectQuery.Append(" ,EC.EC_ACT_DIM2 ");
            selectQuery.Append(" ,EC.EC_ACT_DIM3 ");
            selectQuery.Append(" ,EC.EC_ACT_DIM4 ");
            selectQuery.Append(" ,EC.DEAD_LENGTH_A ");
            selectQuery.Append(" ,EC.DEAD_LENGTH_B ");
            selectQuery.Append(" ,EC.EC_ACT_LENGTH ");
            selectQuery.Append(" ,EC.REEC ");
            selectQuery.Append(" ,EC.EC_END_DATE AS EC_END_DATE  ");
            selectQuery.Append(",TO_CHAR( EC.EC_END_DATE,'yyyy/mm/dd HH24:MI' ) AS EC_END_DATE_STRING");
            selectQuery.Append(" , (SELECT EC.UNIT_TYPE FROM TRN_EC EC INNER JOIN TRN_NIFUDA N ON EC.PROCESS_PLAN_CD = N.PROCESS_PLAN_CD INNER JOIN TRN_PROCESS_PLAN PP ON N.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE UPPER(N.NIFUDA_NO)= '" + nifudaNo.ToUpper() + "' AND EC.MACHINE_CD = '" + mechineCD + "' ");
            if (getActiveChuuban > 0)
            {
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            }
            selectQuery.Append(" AND EC.UNIT_TYPE IS NOT NULL AND EC.CREATED_DATE >= TO_DATE(SYSDATE)-" + days + " ORDER BY  EC.EC_CD DESC FETCH FIRST ROW ONLY)  AS UNIT_TYPE ");
            //Tab 4
            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC, ");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.YEILD_PATTERN1,");
            selectQuery.Append("PP.YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append("PP.YEILD_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.ELONGATION_PATTERN1,");
            selectQuery.Append("PP.ELONGATION_PATTERN1_MIN_MAX 	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MAX	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MIN ,");
            selectQuery.Append("PP.ELONGATION_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.PRESS_PATTERN1,");
            selectQuery.Append("PP.PRESS_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append("PP.PRESS_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.HARDNESS_PATTERN1 ,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_MIN_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MIN  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN3_ACTUAL  	,");

            selectQuery.Append("PP.DECARB_1 ,");
            selectQuery.Append("PP.DECARB_2 ,");
            selectQuery.Append("PP.DECARB_3 ,");
            selectQuery.Append("PP.DECARB_VAL_1 ,");
            selectQuery.Append("PP.DECARB_VAL_2  ,");
            selectQuery.Append("PP.DECARB_VAL_3,");

            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10, ");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11, ");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12, ");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.PROD_POINT_1 AS PP_PROD_POINT_1, ");
            selectQuery.Append(" PP.PROD_POINT_2 AS PP_PROD_POINT_2, ");
            selectQuery.Append(" PP.PROD_POINT_3 AS PP_PROD_POINT_3, ");
            selectQuery.Append(" PP.PROD_POINT_4 AS PP_PROD_POINT_4, ");
            selectQuery.Append(" PP.PROD_POINT_5 AS PP_PROD_POINT_5, ");
            selectQuery.Append(" PP.PROD_POINT_6 AS PP_PROD_POINT_6, ");
            selectQuery.Append(" PP.PROD_POINT_7 AS PP_PROD_POINT_7, ");
            selectQuery.Append(" PP.PROD_POINT_8 AS PP_PROD_POINT_8, ");
            selectQuery.Append(" PP.PROD_POINT_9 AS PP_PROD_POINT_9, ");
            selectQuery.Append(" PP.PROD_POINT_10 AS PP_PROD_POINT_10, ");
            selectQuery.Append(" PP.PROD_POINT_11 AS PP_PROD_POINT_11, ");
            selectQuery.Append(" PP.PROD_POINT_12 AS PP_PROD_POINT_12, ");
            selectQuery.Append(" PP.PROD_POINT_13 AS PP_PROD_POINT_13, ");
            selectQuery.Append(" COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION, ");
            if (rowsCount == 0)
            {
                selectQuery.Append(" 'NO' AS PAST_DATA_EXIST ");
            }else
            {
                selectQuery.Append(" 'YES' AS PAST_DATA_EXIST ");
            }

                // selectQuery.Append(" FI.COMBINED_DECISION "); 

                selectQuery.Append(" FROM TRN_NIFUDA N");
            selectQuery.Append(" LEFT JOIN TRN_EC EC ON EC.PROCESS_PLAN_CD = N.PROCESS_PLAN_CD");
        //    selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = N.NIFUDA_CD");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON N.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            
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
            //selectQuery.Append("  LEFT JOIN TRN_KAKOUIRAI K  ON PP.KAKOUIRAI_CD   = K.KAKOUIRAI_CD");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M   ON M.MSG_CD = EC.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            //   selectQuery.Append("  LEFT JOIN TRN_FINAL_INSPECTION FI   ON FI.PROCESS_PLAN_CD = pp.PROCESS_PLAN_CD");

            if (rowsCount > 0)
            {
                selectQuery.Append(" WHERE UPPER(N.NIFUDA_NO)= '" + nifudaNo.ToUpper() + "'");
                selectQuery.Append(" AND EC.MACHINE_CD = '" + mechineCD + "' ");
                selectQuery.Append(" AND EC.CREATED_DATE >= TO_DATE(SYSDATE)-" + days);
            }else
            {
                selectQuery.Append(" WHERE UPPER(N.NIFUDA_NO)= '" + nifudaNo.ToUpper() + "'");
            }

            if (getActiveChuuban > 0)
            {
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            }

            selectQuery.Append(" ORDER BY EC.EC_CD DESC");
            return ExecuteQuerySingleResult<ECDetails>(selectQuery.ToString());
        }

        public int FIAlreadyDone(long nifudaCD, short thabaNo)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(NIFUDA_CD)");
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION "); 
           // selectQuery.Append("  LEFT JOIN TRN_FINAL_INSPECTION FI   ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" WHERE NIFUDA_CD = " + nifudaCD);
            selectQuery.Append(" AND FI_TABA_NO = " + thabaNo);
           
            selectQuery.Append(" AND COMBINED_DECISION = " + decisionVal); 

            return Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        }
        public List<ECDetails> IsFICompleted(long nifudaCD, short thabaNo, int ecCD)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT NIFUDA_CD,EC_CD");
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION ");
            selectQuery.Append(" WHERE NIFUDA_CD = " + nifudaCD);
            selectQuery.Append(" AND FI_TABA_NO = " + thabaNo);
            selectQuery.Append(" AND COMBINED_DECISION = " + decisionVal);

            return ExecuteQueryMultiResult<ECDetails>(selectQuery.ToString());
        }
        public List<ECDetails> GetEC24MachineList(string machineCd,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append("EC.MACHINE_CD AS MACHINE_CD,  ");
            selectQuery.Append(" EC.EC_DECISION AS EC_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append(" EC.EC_METALLIC_PLATE_CODE ,	");
            selectQuery.Append(" PP.PROD_DIM_D || PP.PROD_SHAPE  AS  PP_PROD_DIMD_SHAPE,	");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,	");
            selectQuery.Append(" EC.EC_TABA_NO,  ");
            selectQuery.Append(" EC.EC_OK_BARS,  ");
            selectQuery.Append(" EC.UNIT_TYPE,  ");
            selectQuery.Append(" EC.EC_OK_QTY,  ");
            selectQuery.Append(" EC.EC_NG_QTY,  ");
            selectQuery.Append(" EC.EC_NG_PERCENTAGE,  ");
            selectQuery.Append(" EC.EC_NG_BAR_TOTAL,  ");
            selectQuery.Append(" EC.IS_OK,  ");
            selectQuery.Append(" EC.LINE_SPEED,  ");
            selectQuery.Append(" EC.MAGNETISM,  ");
            if (cultureName == "en")
            {
                selectQuery.Append(" (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE EC.EC_NG_DETAILS1 = PD.PROCESS_DEFECT_CD) AS EC_NG_DETAILS1_STRING, ");
                selectQuery.Append(" (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE EC.EC_NG_DETAILS2 = PD.PROCESS_DEFECT_CD) AS EC_NG_DETAILS2_STRING,");
                selectQuery.Append(" (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE EC.EC_NG_DETAILS3 = PD.PROCESS_DEFECT_CD) AS EC_NG_DETAILS3_STRING,");
            }else
            {
                selectQuery.Append(" (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE EC.EC_NG_DETAILS1 = PD.PROCESS_DEFECT_CD) AS EC_NG_DETAILS1_STRING, ");
                selectQuery.Append(" (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE EC.EC_NG_DETAILS2 = PD.PROCESS_DEFECT_CD) AS EC_NG_DETAILS2_STRING,");
                selectQuery.Append(" (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE EC.EC_NG_DETAILS3 = PD.PROCESS_DEFECT_CD) AS EC_NG_DETAILS3_STRING,");
            }
                selectQuery.Append(" EC.EC_NG_BARS1, ");
            selectQuery.Append(" EC.EC_NG_BARS2, ");
            selectQuery.Append(" EC.EC_NG_BARS3, ");
            
            //selectQuery.Append("  TO_CHAR (EC.MAGNETISM, '90.00') AS EC_MAGNETISM_STRING,	");
            selectQuery.Append("  EC.MAGNETISM,	");
            selectQuery.Append("  PP.PROD_SPEC_NO as PP_PROD_SPEC_NO,	");
            selectQuery.Append("  PP.CHARGE AS PP_CHARGE,	");
            selectQuery.Append("  NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append("  M.MSG_CONTENTS AS EC_REMARKS, ");
            selectQuery.Append("  EC.MSG_CD, ");
            selectQuery.Append("  EC.SHIKII_VALUE ,	");
            selectQuery.Append("  NVL (EC.EC_START_DATE, EC.EC_END_DATE) AS ORDER_CLAUSE ");
            selectQuery.Append(" FROM TRN_EC EC");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON EC.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = EC.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD ");

            selectQuery.Append(" WHERE  EC.MACHINE_CD  = '" + machineCd + "' AND NVL (EC.EC_END_DATE, EC.EC_START_DATE) > SYSDATE - 1");
           // selectQuery.Append("  AND ROWNUM <= 10");
            selectQuery.Append(" ORDER BY ORDER_CLAUSE DESC");
            return ExecuteQueryMultiResult<ECDetails>(selectQuery.ToString());
        }

        public List<ThabaDetails> GetThabaDetails(string chuuban, short? gyouban)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN  ");
            selectQuery.Append(" ,PP.GYOUBAN ");
            selectQuery.Append(" ,EC.EC_DECISION  AS DECISION");
            selectQuery.Append(" ,EC.CREATED_DATE ");
            selectQuery.Append(" ,TO_CHAR(EC.EC_TABA_NO) AS TABA_NO ");
            selectQuery.Append(" ,EC.EC_DECISION AS PROCESS_STATUS ");
            selectQuery.Append(" FROM TRN_EC EC  ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND EC.EC_DECISION  IS NOT NULL");

            selectQuery.Append(" ORDER BY EC.EC_CD ");
            return ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
        }
        public ThabaDetails GetThabaDetailsByThabaNumber(long processPlanCD, short thabaNo)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, EC.EC_CD, EC.REEC");
            selectQuery.Append(" FROM TRN_EC EC INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD =" + processPlanCD + " ");
            
            selectQuery.Append(" AND EC.EC_DECISION = "+ decisionVal);
            if (thabaNo > 0)
            {
                selectQuery.Append(" AND EC.EC_TABA_NO ='" + thabaNo + "' ");
            }
            
                selectQuery.Append(" ORDER BY EC.EC_CD ASC ");

            return ExecuteQuerySingleResult<ThabaDetails>(selectQuery.ToString());
             
        }
        public ECDetails GetECProcessPlanDetailsByChuuban(string chubaan, short? gyouban)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUST.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS DIM_TOL_MAX_CHR");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");

            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");

            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");    //COIL_QTY

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");

            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");

            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE  ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_WINDING) AS PP_COIL_DIA_WINDING ");
            selectQuery.Append("  ,PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS  ");           //PP_KAKOUIRAIREMARKS
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            selectQuery.Append(" , P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10, ");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11, ");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12, ");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13, ");

            selectQuery.Append(" PP.PROD_POINT_1 AS PP_PROD_POINT_1, ");
            selectQuery.Append(" PP.PROD_POINT_2 AS PP_PROD_POINT_2, ");
            selectQuery.Append(" PP.PROD_POINT_3 AS PP_PROD_POINT_3, ");
            selectQuery.Append(" PP.PROD_POINT_4 AS PP_PROD_POINT_4, ");
            selectQuery.Append(" PP.PROD_POINT_5 AS PP_PROD_POINT_5, ");
            selectQuery.Append(" PP.PROD_POINT_6 AS PP_PROD_POINT_6, ");
            selectQuery.Append(" PP.PROD_POINT_7 AS PP_PROD_POINT_7, ");
            selectQuery.Append(" PP.PROD_POINT_8 AS PP_PROD_POINT_8, ");
            selectQuery.Append(" PP.PROD_POINT_9 AS PP_PROD_POINT_9, ");
            selectQuery.Append(" PP.PROD_POINT_10 AS PP_PROD_POINT_10, ");
            selectQuery.Append(" PP.PROD_POINT_11 AS PP_PROD_POINT_11, ");
            selectQuery.Append(" PP.PROD_POINT_12 AS PP_PROD_POINT_12, ");
            selectQuery.Append(" PP.PROD_POINT_13 AS PP_PROD_POINT_13, ");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.ELONGATION_PATTERN1,");
            selectQuery.Append("PP.ELONGATION_PATTERN1_MIN_MAX 	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MAX	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MIN ,");
            selectQuery.Append("PP.ELONGATION_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.HARDNESS_PATTERN1 ,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_MIN_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MIN  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN3_ACTUAL  	,");

            selectQuery.Append("PP.DECARB_1 ,");
            selectQuery.Append("PP.DECARB_2 ,");
            selectQuery.Append("PP.DECARB_3 ,");
            selectQuery.Append("PP.DECARB_VAL_1 ,");
            selectQuery.Append("PP.DECARB_VAL_2  ,");
            selectQuery.Append("PP.DECARB_VAL_3,");

            selectQuery.Append("PP.YEILD_PATTERN1,");
            selectQuery.Append("PP.YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append("PP.YEILD_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.PRESS_PATTERN1,");
            selectQuery.Append("PP.PRESS_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append("PP.PRESS_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.IMPACT_TEST_PATTERN1,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN  TRN_EC EC ON EC.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MM ON EC.MACHINE_CD = MM.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON EC.MSG_CD = MSG.MSG_CD");
            selectQuery.Append(" LEFT JOIN TRN_FINAL_INSPECTION FI_INSP ON FI_INSP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD AND FI_INSP.EC_CD = EC.EC_CD");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ON PCK.PACKING_CD = PP.PACKING_CD ");

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
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");

            selectQuery.Append(" WHERE PP.CHUUBAN='" + chubaan + "' AND PP.GYOUBAN = '" + gyouban + "'");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" ORDER BY EC.EC_CD DESC");
           
            return ExecuteQuerySingleResult<ECDetails>(selectQuery.ToString());
        }

        public List<ECDetails> GetProcessHistoryList(string chuubanNo, short? gyoubanNo, string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS PP_PROCESS_LOCATION,");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MM.MACHINE_NAME AS EC_MACHINE_NAME ,");
            selectQuery.Append(" TO_CHAR(EC.EC_START_DATE, 'yyyy/mm/dd HH24:MI') AS EC_START_DATE_STRING ,");
            selectQuery.Append(" TO_CHAR(EC.EC_END_DATE, 'yyyy/mm/dd HH24:MI') AS EC_END_DATE_STRING ,");
            selectQuery.Append(" EMP.SHORT_NAME AS EC_END_OPERATOR_NAME,");
            selectQuery.Append(" CUS.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" EC.EC_METALLIC_PLATE_CODE ,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" EC.EC_TABA_NO AS EC_TABA_NO, ");
            selectQuery.Append(" EC.EC_OK_BARS,  ");
            selectQuery.Append(" EC.UNIT_TYPE AS UNIT_TYPE,");
            selectQuery.Append(" EC.EC_OK_QTY,  ");
            selectQuery.Append(" EC.EC_NG_BARS1,");
            selectQuery.Append(" EC.EC_NG_BARS2,");
            selectQuery.Append(" EC.EC_NG_BARS3,");
          
            selectQuery.Append(" EC.IS_OK,");
            selectQuery.Append(" EC.LINE_SPEED AS LINE_SPEED, ");
            selectQuery.Append(" EC.MAGNETISM  AS EC_MAGNETISM,  ");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" MSG.MSG_CONTENTS AS EC_REMARKS,");
            selectQuery.Append(" INTE.INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2,");
            selectQuery.Append(" EC.EC_DECISION ");
            
            if (cultureName == "en")
            {
                selectQuery.Append(", PROCK.KUBUN_NAME_EN AS NEXT_PROCESS ");
                selectQuery.Append(", (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1) AS EC_NG_DETAILS1_STRING");
                selectQuery.Append(", (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2) AS EC_NG_DETAILS2_STRING");
                selectQuery.Append(", (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3) AS EC_NG_DETAILS3_STRING");
            }
            else
            {
                selectQuery.Append(", PROCK.KUBUN_NAME AS NEXT_PROCESS ");
                selectQuery.Append(", (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1) AS EC_NG_DETAILS1_STRING");
                selectQuery.Append(", (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2) AS EC_NG_DETAILS2_STRING");
                selectQuery.Append(", (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3) AS EC_NG_DETAILS3_STRING");
            }

           
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN  TRN_EC EC ON EC.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MM ON EC.MACHINE_CD = MM.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON EC.MSG_CD = MSG.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = EC.EC_NEXT_PROCESS");
            selectQuery.Append(" LEFT JOIN TRN_FINAL_INSPECTION FI_INSP ON FI_INSP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD AND FI_INSP.EC_CD = EC.EC_CD");

            selectQuery.Append(" WHERE PP.CHUUBAN='" + chuubanNo + "' AND PP.GYOUBAN = '" + gyoubanNo + "'");

            selectQuery.Append(" ORDER BY EC.EC_CD DESC");

            return ExecuteQueryMultiResult<ECDetails>(selectQuery.ToString());
        }


        public ECDetails InsertECDetails(ECDetails ecDetails)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                if (ecDetails.REEC > 0)
                {
                    updateFIDecision(Convert.ToInt64(ecDetails.NIFUDA_CD), Convert.ToInt16(ecDetails.EC_TABA_NO));
                }
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append(" INSERT INTO TRN_EC(");
                insertQuery.Append(" EC_CD,");
                insertQuery.Append(" PROCESS_PLAN_CD,");
                insertQuery.Append(" MACHINE_CD,");
                insertQuery.Append(" LOCATION_CD,");
                insertQuery.Append(" EC_START_DATE,");
                insertQuery.Append(" EC_START_OPERATOR_CD,");
                insertQuery.Append(" EC_METALLIC_PLATE_CODE,");
                insertQuery.Append(" FURNACE_NO,");
                insertQuery.Append(" EC_TABA_NO,");
                insertQuery.Append(" TP_TEST,");
                insertQuery.Append(" ACT_ALLOC_UNITS,");
                insertQuery.Append(" ACT_ALLOC_QTY,");
                insertQuery.Append(" IS_OK,");
                insertQuery.Append(" EC_COIL_SIZE,");
                insertQuery.Append(" EC_COIL_SHAPE,");
                insertQuery.Append(" SHIKII_VALUE,");
                insertQuery.Append(" LINE_SPEED,");
                insertQuery.Append(" EC_ACT_DIM1, ");
                insertQuery.Append(" EC_ACT_DIM2, ");
                insertQuery.Append(" EC_ACT_DIM3, ");
                insertQuery.Append(" EC_ACT_DIM4, ");
                insertQuery.Append(" DEAD_LENGTH_A, ");
                insertQuery.Append(" DEAD_LENGTH_B, ");
                insertQuery.Append(" EC_ACT_LENGTH, ");
                insertQuery.Append(" REEC, ");
                insertQuery.Append(" PROCESS_KUBUN_CD, ");
                insertQuery.Append(" USER_ID, ");
                insertQuery.Append(" INPUT_LOCATION_CD, ");
                insertQuery.Append(" CREATED_DATE,");
                insertQuery.Append(" UPDATED_DATE");
                insertQuery.Append(" ) ");
                insertQuery.Append(" VALUES(");
                insertQuery.Append("EC_SEQ.NEXTVAL ,");
                insertQuery.Append("'" + ecDetails.PROCESS_PLAN_CD + "'");
                insertQuery.Append(",'" + ecDetails.MACHINE_CD + "'");
                insertQuery.Append(",'" + ecDetails.LOCATION_CD + "'");
                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(ecDetails.EC_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                insertQuery.Append(",'" + ecDetails.EC_START_OPERATOR_CD + "'");
                insertQuery.Append(",'" + ecDetails.EC_METALLIC_PLATE_CODE + "'");
                insertQuery.Append(",'" + ecDetails.FURNACE_NO + "'");
                insertQuery.Append(",'" + ecDetails.EC_TABA_NO + "'");
                insertQuery.Append(",'" + ecDetails.TP_TEST + "'");
                insertQuery.Append(",'" + ecDetails.ACT_ALLOC_UNITS + "'");
                insertQuery.Append(",'" + ecDetails.ACT_ALLOC_QTY + "'");     //
                insertQuery.Append(",'" + ecDetails.IS_OK + "'");
                insertQuery.Append(",'" + ecDetails.EC_COIL_SIZE + "'");
                insertQuery.Append(",'" + ecDetails.EC_COIL_SHAPE + "'");
                insertQuery.Append(",'" + ecDetails.SHIKII_VALUE + "'");
                insertQuery.Append(",'" + ecDetails.LINE_SPEED + "'");
                insertQuery.Append(",'" + ecDetails.EC_ACT_DIM1 + "'");
                insertQuery.Append(",'" + ecDetails.EC_ACT_DIM2 + "'");
                insertQuery.Append(",'" + ecDetails.EC_ACT_DIM3 + "'");
                insertQuery.Append(",'" + ecDetails.EC_ACT_DIM4 + "'");
                insertQuery.Append(",'" + ecDetails.DEAD_LENGTH_A + "'");
                insertQuery.Append(",'" + ecDetails.DEAD_LENGTH_B + "'");
                insertQuery.Append(",'" + ecDetails.EC_ACT_LENGTH + "'");
                insertQuery.Append(",'" + ecDetails.REEC + "'");
                insertQuery.Append(",'" + ecDetails.PROCESS_KUBUN_ID + "'");
                insertQuery.Append(",'" + ecDetails.USER_ID + "'");
                insertQuery.Append(",'" + ecDetails.INPUT_LOCATION_CD + "'");
                insertQuery.Append(",SYSDATE");
                insertQuery.Append(",SYSDATE ");
                insertQuery.Append(") ");
                ExecuteNonQuery(insertQuery.ToString());
                var result = GetECDetailsByChuuban(ecDetails.NIFUDA_NO, Convert.ToInt16(ecDetails.MACHINE_CD));
                transactionScope.Complete();

                return result;
            }
        }

        public void updateFIDecision(long nifudaCD, short thabNo)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_NG"));
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_FINAL_INSPECTION ");
            updateQuery.Append("SET COMBINED_DECISION = " + decisionVal);
            updateQuery.Append(" , UPDATED_DATE = SYSDATE");
            updateQuery.Append(" WHERE NIFUDA_CD = " + nifudaCD);
            updateQuery.Append(" AND FI_TABA_NO = " + thabNo);
            ExecuteNonQuery(updateQuery.ToString());
           
        }
        public long UpdateECDetails(ECDetails ecDetails)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            //if (ecDetails.EC_DECISION == decisionVal)
            //{
            //    ecDetails.FINAL_INSP = 1;
            //}else
            //{
            //    ecDetails.FINAL_INSP = null;
            //}

            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_EC SET ");
            updateQuery.Append(" MACHINE_CD = '" + ecDetails.MACHINE_CD + "',");
            // Start data is updated

            updateQuery.Append(" LOCATION_CD = " + ecDetails.LOCATION_CD + ",");
            updateQuery.Append(" EC_START_DATE = ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(ecDetails.EC_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateQuery.Append(" EC_START_OPERATOR_CD = '" + ecDetails.EC_START_OPERATOR_CD + "',");
            updateQuery.Append(" EC_METALLIC_PLATE_CODE = '" + ecDetails.EC_METALLIC_PLATE_CODE + "',");
            updateQuery.Append(" EC_TABA_NO = '" + ecDetails.EC_TABA_NO + "',");
            updateQuery.Append(" TP_TEST = '" + ecDetails.TP_TEST + "',");
            updateQuery.Append(" ACT_ALLOC_UNITS = '" + ecDetails.ACT_ALLOC_UNITS + "',");
            updateQuery.Append(" ACT_ALLOC_QTY = '" + ecDetails.ACT_ALLOC_QTY + "',");
            updateQuery.Append(" IS_OK = '" + ecDetails.IS_OK + "',");
            updateQuery.Append(" FURNACE_NO = '" + ecDetails.FURNACE_NO + "',");
            updateQuery.Append(" EC_COIL_SIZE = '" + ecDetails.EC_COIL_SIZE + "',");
            updateQuery.Append(" EC_COIL_SHAPE = '" + ecDetails.EC_COIL_SHAPE + "',");
            updateQuery.Append(" SHIKII_VALUE = '" + ecDetails.SHIKII_VALUE + "',");
            updateQuery.Append(" LINE_SPEED = '" + ecDetails.LINE_SPEED + "',");
            updateQuery.Append(" REEC = '" + ecDetails.REEC + "',");
            //end
            updateQuery.Append(" EC_END_OPERATOR_CD ='" + ecDetails.EC_END_OPERATOR_CD + "',");
            updateQuery.Append(" EC_END_DATE= ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(ecDetails.EC_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateQuery.Append(" EC_VISUAL_CHK  = '" + ecDetails.EC_VISUAL_CHK + "',");
            updateQuery.Append(" EC_ACT_DIM1='" + ecDetails.EC_ACT_DIM1 + "',");
            updateQuery.Append(" EC_ACT_DIM2='" + ecDetails.EC_ACT_DIM2 + "',");
            updateQuery.Append(" EC_ACT_DIM3='" + ecDetails.EC_ACT_DIM3 + "',");
            updateQuery.Append(" EC_ACT_DIM4='" + ecDetails.EC_ACT_DIM4 + "',");
            updateQuery.Append(" DEAD_LENGTH_A ='" + ecDetails.DEAD_LENGTH_A + "',");
            updateQuery.Append(" DEAD_LENGTH_B ='" + ecDetails.DEAD_LENGTH_B + "',");
            updateQuery.Append(" EC_ACT_LENGTH  = '" + ecDetails.EC_ACT_LENGTH + "',");
            updateQuery.Append(" MAGNETISM='" + ecDetails.MAGNETISM + "',");
            updateQuery.Append(" EC_OK_BARS='" + ecDetails.EC_OK_BARS + "',");
            updateQuery.Append(" UNIT_TYPE='" + ecDetails.UNIT_TYPE + "',");
            updateQuery.Append(" EC_OK_QTY='" + ecDetails.EC_OK_QTY + "',");
            updateQuery.Append(" EC_NG_QTY='" + ecDetails.EC_NG_QTY + "',");
            updateQuery.Append(" EC_NG_BARS1  = '" + ecDetails.EC_NG_BARS1 + "',");
            updateQuery.Append(" EC_NG_DETAILS1='" + ecDetails.EC_NG_DETAILS1 + "',");
            updateQuery.Append(" EC_NG_BARS2  = '" + ecDetails.EC_NG_BARS2 + "',");
            updateQuery.Append(" EC_NG_DETAILS2='" + ecDetails.EC_NG_DETAILS2 + "',");
            updateQuery.Append(" EC_NG_BARS3  = '" + ecDetails.EC_NG_BARS3 + "',");
            updateQuery.Append(" EC_NG_DETAILS3='" + ecDetails.EC_NG_DETAILS3 + "',");
            updateQuery.Append(" EC_NG_BAR_TOTAL  = '" + ecDetails.EC_NG_BAR_TOTAL + "',");
            updateQuery.Append(" EC_NG_PERCENTAGE='" + ecDetails.EC_NG_PERCENTAGE + "',");
            updateQuery.Append(" EC_DECISION  = '" + ecDetails.EC_DECISION + "',");
            updateQuery.Append(" EC_NEXT_PROCESS='" + ecDetails.EC_NEXT_PROCESS + "',");
            updateQuery.Append(" FINAL_INSP ='" + ecDetails.FINAL_INSP + "',");
            updateQuery.Append(" MSG_CD ='" + ecDetails.MSG_CD + "',");
            updateQuery.Append(" USER_ID ='" + ecDetails.USER_ID + "',");
            updateQuery.Append(" INPUT_LOCATION_CD ='" + (ecDetails.INPUT_LOCATION_CD > 0 ? ecDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "',");
            updateQuery.Append(" UPDATED_DATE = SYSDATE");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" EC_CD= '" + ecDetails.EC_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());
            long msgCD = Convert.ToInt64(ExecuteScalar("SELECT NVL(MSG_CD,0) FROM TRN_EC WHERE EC_CD= '" + ecDetails.EC_CD + "'"));
            
            
            return msgCD; 
        }

        public FinalInspectionDetails Insert(FinalInspectionDetails finalInspection)
        {
            StringBuilder InsertQuery = new StringBuilder();

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT  F.FINAL_INSP_CD AS FINAL_INSP_NO FROM TRN_FINAL_INSPECTION F ");
            selectQuery.Append("WHERE FINAL_INSP_CD = (SELECT MAX(FINAL_INSP_CD) FROM TRN_FINAL_INSPECTION)");

            InsertQuery.Append("INSERT INTO TRN_FINAL_INSPECTION(");
            InsertQuery.Append("FINAL_INSP_CD , ");
            InsertQuery.Append("PROCESS_PLAN_CD , ");
            InsertQuery.Append("NIFUDA_CD, ");
            InsertQuery.Append("INSP_DATE, ");
            InsertQuery.Append("INSP_BY, ");
            InsertQuery.Append("FI_DIM1, ");
            InsertQuery.Append("FI_DIM2, ");
            InsertQuery.Append("FI_DIM3, ");
            InsertQuery.Append("FI_DIM4, ");
            InsertQuery.Append("FI_LENGTH, ");
            InsertQuery.Append("FI_DIM_DECISION, ");
            InsertQuery.Append("FI_LENGTH_DECISION, ");
            InsertQuery.Append("FI_NEXT_PROCESS, ");
            InsertQuery.Append("COMBINED_DECISION, ");
            InsertQuery.Append("FI_COLOR_DECISION, ");
            InsertQuery.Append("FI_CHAMFERING_DECISION, ");
            InsertQuery.Append("FI_EXT_TEXTURE_DECISION, ");
            InsertQuery.Append("NO_OF_BARS, ");
            InsertQuery.Append("FI_QTY, ");
            InsertQuery.Append("FI_TABA_NO, ");
            InsertQuery.Append("UNIT_TYPE, ");
            InsertQuery.Append("INSP_PROCESS_KUBUN_CD, ");
            InsertQuery.Append("EC_CD, ");
            InsertQuery.Append("LOCATION_CD, ");
            InsertQuery.Append("INPUT_LOCATION_CD, ");
            InsertQuery.Append("USER_ID, ");
            InsertQuery.Append("CREATED_DATE, ");
            InsertQuery.Append("UPDATED_DATE ) ");

            InsertQuery.Append(" VALUES(");
            InsertQuery.Append("FINAL_INSPECTION_SEQ.NEXTVAL , ");           
            InsertQuery.Append(finalInspection.PROCESS_PLAN_CD + ",");
            InsertQuery.Append("'" + finalInspection.NIFUDA_CD + "',");
            InsertQuery.Append(" SYSDATE,");
            InsertQuery.Append(finalInspection.INSPECTED_BY + ",");

            InsertQuery.Append(finalInspection.FI_DIM1 + ",");
            InsertQuery.Append(finalInspection.FI_DIM2 + ",");
            InsertQuery.Append("'" + finalInspection.FI_DIM3  + "', ");
            InsertQuery.Append("'" + finalInspection.FI_DIM4 + "', ");
            InsertQuery.Append(finalInspection.FI_ACT_LENGTH + ",");
            InsertQuery.Append(finalInspection.FI_DIM_DECISION + ",");
            InsertQuery.Append(finalInspection.FI_LENGTH_DECISION + ",");
            InsertQuery.Append(finalInspection.FI_NEXT_PROCESS + ",");
            InsertQuery.Append(finalInspection.COMBINED_DECISION + ",");
            InsertQuery.Append(finalInspection.FI_COLOR_DECISION + ",");
            InsertQuery.Append(finalInspection.FI_CHAMFERING_DECISION + ",");
            InsertQuery.Append(finalInspection.FI_EXT_TEXTURE_DECISION + ",");

            InsertQuery.Append(finalInspection.NO_OF_BARS + ",");
            InsertQuery.Append(finalInspection.FI_QTY + ",");
            InsertQuery.Append(finalInspection.FI_TABA_NO + ",");
            InsertQuery.Append(finalInspection.UNIT_TYPE + ",");
            InsertQuery.Append(finalInspection.INSP_PROCESS_KUBUN_CD + ",");
            InsertQuery.Append("'" + finalInspection.EC_CD + "',");
            InsertQuery.Append("'" + finalInspection.LOCATION_CD + "',");
            InsertQuery.Append("'" + finalInspection.INPUT_LOCATION_CD + "',");
            InsertQuery.Append("'" + finalInspection.USER_ID + "',");
            InsertQuery.Append("SYSDATE,");
            InsertQuery.Append("SYSDATE ");
            InsertQuery.Append(") ");

            ExecuteNonQuery(InsertQuery.ToString());
            return ExecuteQuerySingleResult<FinalInspectionDetails>(selectQuery.ToString());
        }
        public void Delete(long ecCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_EC ");
            deleteQuery.Append(" WHERE EC_CD =" + ecCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }
        public void DeleteFinalInspection(long ecCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_FINAL_INSPECTION ");
            deleteQuery.Append(" WHERE EC_CD =" + ecCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }
        public void UpdateFinalInspection(FinalInspectionDetails finalInspection)
        {
            StringBuilder updateQuery = new StringBuilder();

           
            updateQuery.Append("UPDATE TRN_FINAL_INSPECTION SET ");
            updateQuery.Append("PROCESS_PLAN_CD = '" + finalInspection.PROCESS_PLAN_CD + "' , ");
            updateQuery.Append("NIFUDA_CD = '" + finalInspection.NIFUDA_CD + "', ");
            updateQuery.Append("INSP_BY = '" + finalInspection.INSPECTED_BY + "', ");
            updateQuery.Append("FI_DIM1 = '" + finalInspection.FI_DIM1 + "', ");
            updateQuery.Append("FI_DIM2 = '" + finalInspection.FI_DIM2 + "', ");
            updateQuery.Append("FI_LENGTH = '" + finalInspection.FI_ACT_LENGTH + "', ");
            updateQuery.Append("FI_DIM_DECISION = '" + finalInspection.FI_DIM_DECISION + "', ");
            updateQuery.Append("FI_LENGTH_DECISION = '" + finalInspection.FI_LENGTH_DECISION + "', ");
            updateQuery.Append("FI_NEXT_PROCESS = '" + finalInspection.FI_NEXT_PROCESS + "', ");
            updateQuery.Append("COMBINED_DECISION = '" + finalInspection.COMBINED_DECISION + "', ");
            updateQuery.Append("FI_COLOR_DECISION = '" + finalInspection.FI_COLOR_DECISION + "', ");
            updateQuery.Append("FI_CHAMFERING_DECISION = '" + finalInspection.FI_CHAMFERING_DECISION + "', ");
            updateQuery.Append("FI_EXT_TEXTURE_DECISION = '" + finalInspection.FI_EXT_TEXTURE_DECISION +  "', ");
            updateQuery.Append("NO_OF_BARS = '" + finalInspection.NO_OF_BARS + "', ");
            updateQuery.Append("FI_QTY = '" + finalInspection.FI_QTY + "', ");
            updateQuery.Append("FI_TABA_NO = '" + finalInspection.FI_TABA_NO + "', ");
            updateQuery.Append("UNIT_TYPE = '" + finalInspection.UNIT_TYPE + "' , ");
            updateQuery.Append("LOCATION_CD = '" + finalInspection.LOCATION_CD + "', ");
            updateQuery.Append("INPUT_LOCATION_CD = '" + finalInspection.INPUT_LOCATION_CD + "', ");
            updateQuery.Append("USER_ID = '" + finalInspection.USER_ID + "', ");

            updateQuery.Append("UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE EC_CD = '" + finalInspection.EC_CD + "' ");

            ExecuteNonQuery(updateQuery.ToString());
            
        }
        public string GetFinalInspData(long ecCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FINAL_INSP ");
            selectQuery.Append(" FROM TRN_EC EC");
           

            selectQuery.Append(" WHERE EC_CD = '" + ecCD + "' ");
            return Convert.ToString(ExecuteScalar(selectQuery.ToString()));
        }
        public ECDetails GetECDetailsByECCD(long ecCD)
        {
             StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" EC.EC_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(", PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");
           // selectQuery.Append(", N.NIFUDA_COMPLETE_DATE AS NIFUDA_COMPLETE_DATE");
            selectQuery.Append(" ,FI.NIFUDA_CD ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOBAN");
            selectQuery.Append(" ,PP.CHUUBAN  AS PP_CHUUBAN ");
            selectQuery.Append(" ,PP.GYOUBAN  AS PP_GYOUBAN ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            //  selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("  PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append("  ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" , PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS pP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("  PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append("  ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING ");
            selectQuery.Append(" ,PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(",TO_CHAR( EC.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CREATED_DATE_STRING");
            selectQuery.Append(",EC.MACHINE_CD AS MACHINE_CD");
            selectQuery.Append(" ,EC.EC_START_DATE AS EC_START_DATE  ");
            selectQuery.Append(",TO_CHAR( EC.EC_START_DATE,'yyyy/mm/dd HH24:MI' ) AS EC_START_DATE_STRING");
            selectQuery.Append(",EC.EC_START_OPERATOR_CD AS EC_START_OPERATOR_CD");
            selectQuery.Append(",(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = EC_START_OPERATOR_CD) AS EC_START_OPERATOR_NAME");
            selectQuery.Append(" ,EC.EC_TABA_NO AS EC_TABA_NO  ");
            selectQuery.Append(" ,EC.TP_TEST AS TP_TEST  ");
            selectQuery.Append(" ,EC.EC_METALLIC_PLATE_CODE AS EC_METALLIC_PLATE_CODE  ");
            selectQuery.Append(" ,EC.ACT_ALLOC_UNITS ");
            selectQuery.Append(" ,EC.ACT_ALLOC_QTY AS ACT_ALLOC_QTY ");
            selectQuery.Append(" ,EC.IS_OK AS IS_OK ");
            selectQuery.Append(" ,EC.EC_COIL_SIZE ");
            selectQuery.Append(" ,EC.EC_COIL_SHAPE ");
            selectQuery.Append(" ,EC.FURNACE_NO ");
            selectQuery.Append(" ,EC.SHIKII_VALUE ");
            selectQuery.Append(" ,EC.LINE_SPEED ");
            selectQuery.Append(" ,EC.REEC ");
            selectQuery.Append(" ,EC.EC_END_DATE AS EC_END_DATE  ");
            selectQuery.Append(",TO_CHAR( EC.EC_END_DATE,'yyyy/mm/dd HH24:MI' ) AS EC_END_DATE_STRING");

            selectQuery.Append(" ,EC.EC_ACT_DIM1 ");
            selectQuery.Append(" ,EC.EC_ACT_DIM2 ");
            selectQuery.Append(" ,EC.EC_ACT_DIM3 ");
            selectQuery.Append(" ,EC.EC_ACT_DIM4 ");
            selectQuery.Append(" ,EC.DEAD_LENGTH_A ");
            selectQuery.Append(" ,EC.DEAD_LENGTH_B ");
            selectQuery.Append(" ,EC.EC_ACT_LENGTH ");
            
            selectQuery.Append(" ,EC.EC_COIL_SIZE ");
            selectQuery.Append(" ,EC.EC_COIL_SHAPE ");
            selectQuery.Append(" ,EC.EC_END_OPERATOR_CD ");
                      
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = EC_END_OPERATOR_CD) AS EC_END_OPERATOR_NAME");
            selectQuery.Append(" ,EC.EC_VISUAL_CHK ");
            selectQuery.Append(" ,EC.EC_OK_BARS ");
            selectQuery.Append(" ,EC.UNIT_TYPE ");
            selectQuery.Append(" ,EC.EC_OK_QTY ");
            selectQuery.Append(" ,EC.EC_NG_QTY ");
            selectQuery.Append(" ,EC.EC_NG_BAR_TOTAL ");
            selectQuery.Append(" ,EC.EC_NEXT_PROCESS ");
            
            selectQuery.Append(" ,EC.MAGNETISM ");
            selectQuery.Append(" ,EC.FINAL_INSP ");

            selectQuery.Append(" ,MM.SHORT_NAME AS MACHINE_TYPE");
            selectQuery.Append(" ,EC.EC_NG_BARS1 ");
            selectQuery.Append(" ,EC.EC_NG_BARS2 ");
            selectQuery.Append(" ,EC.EC_NG_BARS3 ");
            selectQuery.Append(" ,EC.EC_NG_DETAILS1 ");
            selectQuery.Append(" ,EC.EC_NG_DETAILS2 ");
            selectQuery.Append(" ,EC.EC_NG_DETAILS3 ");
            selectQuery.Append(" ,EC.EC_NG_DETAILS3 ");
            selectQuery.Append(" ,M.MSG_CONTENTS AS EC_REMARKS");
            selectQuery.Append(" ,EC.EC_NG_PERCENTAGE ");
            selectQuery.Append(" ,EC.EC_DECISION ");
            selectQuery.Append(" ,EC.MSG_CD "); 
            selectQuery.Append(" ,FI.FI_DIM_DECISION ");
            selectQuery.Append(" ,FI.FI_LENGTH_DECISION ");
            selectQuery.Append(" ,FI.FI_CHAMFERING_DECISION ");
            selectQuery.Append(" ,FI.FI_COLOR_DECISION ");
            selectQuery.Append(" ,FI.FI_EXT_TEXTURE_DECISION ");
            selectQuery.Append(" ,FI.COMBINED_DECISION ");
            selectQuery.Append(" ,FI.FI_NEXT_PROCESS "); 

            //Tab 4
            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC, ");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.YEILD_PATTERN1,");
            selectQuery.Append("PP.YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append("PP.YEILD_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.ELONGATION_PATTERN1,");
            selectQuery.Append("PP.ELONGATION_PATTERN1_MIN_MAX 	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MAX	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MIN ,");
            selectQuery.Append("PP.ELONGATION_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.PRESS_PATTERN1,");
            selectQuery.Append("PP.PRESS_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append("PP.PRESS_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.HARDNESS_PATTERN1 ,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_MIN_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MIN  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN3_ACTUAL  	,");

            selectQuery.Append("PP.DECARB_1 ,");
            selectQuery.Append("PP.DECARB_2 ,");
            selectQuery.Append("PP.DECARB_3 ,");
            selectQuery.Append("PP.DECARB_VAL_1 ,");
            selectQuery.Append("PP.DECARB_VAL_2  ,");
            selectQuery.Append("PP.DECARB_VAL_3,");

            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10, ");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11, ");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12, ");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.PROD_POINT_1 AS PP_PROD_POINT_1, ");
            selectQuery.Append(" PP.PROD_POINT_2 AS PP_PROD_POINT_2, ");
            selectQuery.Append(" PP.PROD_POINT_3 AS PP_PROD_POINT_3, ");
            selectQuery.Append(" PP.PROD_POINT_4 AS PP_PROD_POINT_4, ");
            selectQuery.Append(" PP.PROD_POINT_5 AS PP_PROD_POINT_5, ");
            selectQuery.Append(" PP.PROD_POINT_6 AS PP_PROD_POINT_6, ");
            selectQuery.Append(" PP.PROD_POINT_7 AS PP_PROD_POINT_7, ");
            selectQuery.Append(" PP.PROD_POINT_8 AS PP_PROD_POINT_8, ");
            selectQuery.Append(" PP.PROD_POINT_9 AS PP_PROD_POINT_9, ");
            selectQuery.Append(" PP.PROD_POINT_10 AS PP_PROD_POINT_10, ");
            selectQuery.Append(" PP.PROD_POINT_11 AS PP_PROD_POINT_11, ");
            selectQuery.Append(" PP.PROD_POINT_12 AS PP_PROD_POINT_12, ");
            selectQuery.Append(" PP.PROD_POINT_13 AS PP_PROD_POINT_13, ");
            selectQuery.Append(" COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");

            selectQuery.Append(" FROM TRN_EC EC");   
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON EC.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" LEFT JOIN TRN_NIFUDA N ON PP.PROCESS_PLAN_CD = N.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MM ON EC.MACHINE_CD = MM.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN TRN_FINAL_INSPECTION FI ON FI.EC_CD = EC.EC_CD");
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
       //     selectQuery.Append("  LEFT JOIN TRN_KAKOUIRAI K  ON PP.KAKOUIRAI_CD   = K.KAKOUIRAI_CD");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M   ON M.MSG_CD = EC.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");

            selectQuery.Append(" WHERE EC.EC_CD = '" + ecCD + "' ");

            selectQuery.Append(" ORDER BY EC.EC_CD DESC");
            return ExecuteQuerySingleResult<ECDetails>(selectQuery.ToString());
        }

        public List<ECDetails> GetECDetailsList(List<SearchField> searchCriteria, string sortCiteria, short locationCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("get_hyphenated_chuuban_gyouban(pp.CHUUBAN_YEAR || pp.chuuban, pp.gyouban) AS PP_CHUUBAN_GYOBAN,");
            selectQuery.Append("pp.chuuban AS PP_CHUUBAN,");
            selectQuery.Append("pp.gyouban AS PP_GYOUBAN,");
            selectQuery.Append("pp.process_plan_cd AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append("ec.EC_CD,");
            selectQuery.Append("ec.EC_TABA_NO,");
            selectQuery.Append("ec.machine_cd as MACHINE_CD,");
            selectQuery.Append("(SELECT MACHINE_NAME FROM mst_machine  WHERE machine_cd = ec.machine_cd ) AS EC_MACHINE_NAME,");
            selectQuery.Append("pp.prod_spec_no AS PP_PROD_SPEC_NO,");
            selectQuery.Append("to_char(ec.EC_START_DATE, 'yyyy/mm/dd hh24:mi') AS EC_START_DATE_STRING,");
            selectQuery.Append("to_char(ec.EC_END_DATE, 'yyyy/mm/dd hh24:mi') AS EC_END_DATE_STRING,");
            selectQuery.Append("emp.short_name AS EC_START_OPERATOR_NAME,");
            selectQuery.Append("cst.short_name AS PP_CUSTOMER_NAME,");
            selectQuery.Append("pp.prod_name_cust AS PP_PROD_NAME_CUST,");
            selectQuery.Append("pp.prod_shape AS PP_PROD_SHAPE,");
            selectQuery.Append("NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append("pp.matl_name_mak AS PP_MATL_NAME_MAK,");
            selectQuery.Append("get_dim_d_w_shape_other(pp.matl_dim_d, pp.matl_dim_w, matl_shape,pp.matl_dim_other) AS PP_MATL_DIM_D_STRING,");
            selectQuery.Append("pp.charge AS PP_CHARGE,");
            selectQuery.Append("ec.EC_METALLIC_PLATE_CODE as EC_METALLIC_PLATE_CODE,");
            //  selectQuery.Append("pp.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIMD_SHAPE, "); 
            selectQuery.Append("ec.EC_OK_BARS as EC_OK_BARS,");
            selectQuery.Append("ec.UNIT_TYPE as UNIT_TYPE,");
            selectQuery.Append("ec.EC_OK_QTY as EC_OK_QTY,");
            selectQuery.Append("ec.EC_END_OPERATOR_CD as EC_END_OPERATOR_CD,");
            selectQuery.Append("ec.EC_DECISION ");
            selectQuery.Append("FROM TRN_PROCESS_PLAN pp ");
            selectQuery.Append("INNER JOIN TRN_EC EC ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN MST_EMPLOYEE EMP ON EC.EC_START_OPERATOR_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append("LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            selectQuery.Append("LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append("WHERE EC.INPUT_LOCATION_CD = " + locationCD);
            selectQuery.Append(" AND EC.EC_END_DATE IS NOT NULL");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " AND ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }
                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += " ORDER BY PP_CHUUBAN_GYOBAN ASC, ec.EC_TABA_NO ASC";
            }
            return ExecuteQueryMultiResult<ECDetails>(query);
        }

        public ECDetails GetOperatorDetailsById(long employeeCd)
        {
            ECDetails result = new ECDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM MST_EMPLOYEE EM");
            selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG ");
            selectQuery.Append(" ON DG.EMPLOYEE_CD =EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE EM.EMPLOYEE_CD = '" + employeeCd + "' ");
            result = ExecuteQuerySingleResult<ECDetails>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of EC process data according to search criteria        
        /// </returns>
        /// 
        public List<ECDetails> GetECDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<ECDetails> result = new List<ECDetails> ();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_PROCESS_LOCATION,");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MAC.MACHINE_NAME AS EC_MACHINE_NAME ,");
            selectQuery.Append(" TO_CHAR(EC.EC_START_DATE, 'yyyy/mm/dd HH24:MI') AS EC_START_DATE_STRING ,");
            selectQuery.Append(" TO_CHAR(EC.EC_END_DATE, 'yyyy/mm/dd HH24:MI') AS EC_END_DATE_STRING ,");
            selectQuery.Append(" EMP.SHORT_NAME AS EC_END_OPERATOR_NAME,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,"); 
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" EC.EC_METALLIC_PLATE_CODE ,"); 
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" EC.EC_TABA_NO AS EC_TABA_NO, ");
            selectQuery.Append(" EC.EC_OK_BARS,  ");
            selectQuery.Append(" EC.UNIT_TYPE AS UNIT_TYPE,");
            selectQuery.Append(" EC.EC_OK_QTY,  ");
            selectQuery.Append(" EC.EC_NG_BARS1,");
            selectQuery.Append(" EC.EC_NG_BARS2,");
            selectQuery.Append(" EC.EC_NG_BARS3,");
            selectQuery.Append(" DEF1.PROC_DEFECT_SHORT AS EC_NG_DETAILS1_JP,");
            selectQuery.Append(" DEF1.OTHER_NAME_SHORT AS EC_NG_DETAILS1_EN,");
            selectQuery.Append(" DEF2.PROC_DEFECT_SHORT AS EC_NG_DETAILS2_JP,");
            selectQuery.Append(" DEF2.OTHER_NAME_SHORT AS EC_NG_DETAILS2_EN,");
            selectQuery.Append(" DEF3.PROC_DEFECT_SHORT AS EC_NG_DETAILS3_JP,");
            selectQuery.Append(" DEF3.OTHER_NAME_SHORT AS EC_NG_DETAILS3_EN,");
            selectQuery.Append(" EC.IS_OK,");
            selectQuery.Append(" EC.LINE_SPEED AS LINE_SPEED, ");
            selectQuery.Append(" TO_NUMBER(EC.MAGNETISM)  AS EC_MAGNETISM,  ");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS EC_REMARKS,");
            selectQuery.Append(" INTE.INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2,");
            selectQuery.Append(" EC.EC_DECISION, ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");
            selectQuery.Append(" FROM TRN_EC EC");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = EC.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = EC.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = EC.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = EC.EC_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF1 ON DEF1.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF2 ON DEF2.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF3 ON DEF3.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = EC.EC_NEXT_PROCESS");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchFields != null && searchFields.Count > 0)
            {
                if (rowLimit > 0)
                {
                    whereCondition = " WHERE ROWNUM <=" + rowLimit + " AND ";
                }
                else
                {
                    whereCondition = " WHERE ";
                }

                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " and ";
                }

                query = query + whereCondition;
            }
            else if (rowLimit > 0 && searchFields.Count <= 0)
            {
                whereCondition = " WHERE ROWNUM <=" + rowLimit;
                query = query + whereCondition;
            }
            
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            //else
            //{
            //    query += " ORDER BY EC_END_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();    
            selectQuery.Append(query);
            result= ExecuteQueryMultiResult<ECDetails>(query);            
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of EC process data according to search criteria        
        /// </returns>
        /// 
        public List<ECDetails> GetDailyProcessECDetails(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");            
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN ");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");           
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.SHORT_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            //selectQuery.Append(", INTE.INTERNAL_NAME AS PP_INTERNAL_NAME");
            selectQuery.Append(",NVL(INTE.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME"); 
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(", PP.PROD_DIM_W AS PP_PROD_DIM_W");
            selectQuery.Append(", PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(", PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN");
            selectQuery.Append(", PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX");
            selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(", PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER");
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL");
            selectQuery.Append(", PP.COIL_DIA_WINDING AS COIL_DIA_WINDING");

            selectQuery.Append(",  PP.NO_OF_UNITS AS PP_NO_OF_UNITS  ");
            selectQuery.Append(",  PP.UNIT_WT AS PP_UNIT_WT");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(",  PP.CHAMFER AS PP_CHAMFER");
            selectQuery.Append(",  PP.CORNER_RADIUS AS PP_CORNER_R");

            selectQuery.Append(", PP.MAGARI_KUBUN AS MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS NEJIRE_SPAN");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");
            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR");
            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");

            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13");

            selectQuery.Append(", PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1");
            selectQuery.Append(",PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX");
            selectQuery.Append(", PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL");

            selectQuery.Append(", PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1");

            selectQuery.Append(", PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX");
            selectQuery.Append(", PP.HARDNESS_PATTERN3_ACTUAL AS HARDNESS_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1");
            selectQuery.Append(", PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX");
            selectQuery.Append(", PP.YEILD_PATTERN3_ACTUAL AS YEILD_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1");
            selectQuery.Append(", PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX");
            selectQuery.Append(", PP.PRESS_PATTERN3_ACTUAL AS PRESS_PATTERN3_ACTUAL");

            selectQuery.Append(", PP.DECARB_1 AS DECARB_1");
            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1");
            selectQuery.Append(", PP.DECARB_2 AS DECARB_2");
            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2");
            selectQuery.Append(", PP.DECARB_3 AS DECARB_3");
            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL, ");
            
            selectQuery.Append(" PP.RANK_STD AS RANK_STD ");
            selectQuery.Append(",PP.HT_CONDITION AS PP_HT_CONDITION  ");
            selectQuery.Append(",PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK ");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
          
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE");
           
            selectQuery.Append(", PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN");
            selectQuery.Append(", PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");

            //EC
            selectQuery.Append(", MAC.MACHINE_NAME AS EC_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS EC_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( EC.EC_START_DATE,'yyyy/mm/dd HH24:MI' ) AS EC_START_DATE_STRING");            
            selectQuery.Append(", EMP1.SHORT_NAME AS EC_START_OPERATOR_NAME");
            selectQuery.Append(", EC.EC_METALLIC_PLATE_CODE AS EC_METALLIC_PLATE_CODE");
            selectQuery.Append(", EC.FURNACE_NO AS FURNACE_NO");
            selectQuery.Append(", EC.TP_TEST AS TP_TEST");
            selectQuery.Append(", EC.SHIKII_VALUE AS SHIKII_VALUE");

            selectQuery.Append(", EC.DEAD_LENGTH_A AS DEAD_LENGTH_A");
            selectQuery.Append(", EC.DEAD_LENGTH_B AS DEAD_LENGTH_B");

            selectQuery.Append(", EC.ACT_ALLOC_QTY AS ACT_ALLOC_QTY");
            selectQuery.Append(", EC.ACT_ALLOC_UNITS AS ACT_ALLOC_UNITS");
            selectQuery.Append(", EC.IS_OK AS IS_OK"); 
            selectQuery.Append(", EC.EC_COIL_SIZE"); 
            selectQuery.Append(", EC.EC_COIL_SHAPE");
            selectQuery.Append(", EC.LINE_SPEED AS LINE_SPEED");
            selectQuery.Append(", EC.REEC AS REEC");

            selectQuery.Append(", TO_CHAR(EC.EC_END_DATE,'yyyy/mm/dd HH24:MI') AS EC_END_DATE_STRING");           
            selectQuery.Append(", EMP2.SHORT_NAME  AS EC_END_OPERATOR_NAME");
            
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM1, 'FM990.000') AS EC_ACT_DIM1_STRING");
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM2, 'FM990.000') AS EC_ACT_DIM2_STRING");
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM3, 'FM990.000') AS EC_ACT_DIM3_STRING");
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM4, 'FM990.000') AS EC_ACT_DIM4_STRING");

            selectQuery.Append(", EC.EC_ACT_LENGTH AS EC_ACT_LENGTH");
            selectQuery.Append(", EC.MAGNETISM  AS MAGNETISM ");
            selectQuery.Append(", EC.EC_TABA_NO AS EC_TABA_NO");
            selectQuery.Append(", EC.EC_OK_BARS AS EC_OK_BARS");
            selectQuery.Append(", EC.UNIT_TYPE  AS UNIT_TYPE ");
            selectQuery.Append(", EC.EC_OK_QTY  AS EC_OK_QTY ");
            selectQuery.Append(", EC.EC_NG_QTY  AS EC_NG_QTY ");
            selectQuery.Append(", EC.EC_NG_BARS1 AS EC_NG_BARS1");
            selectQuery.Append(", DEFECT1.PROC_DEFECT_SHORT AS EC_NG_DETAILS1_JP");
            selectQuery.Append(", DEFECT1.OTHER_NAME_SHORT AS EC_NG_DETAILS1_EN");
            selectQuery.Append(", EC.EC_NG_BARS2 AS EC_NG_BARS2");
            selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS EC_NG_DETAILS2_JP");
            selectQuery.Append(", DEFECT2.OTHER_NAME_SHORT AS EC_NG_DETAILS2_EN");
            selectQuery.Append(", EC.EC_NG_BARS3 AS EC_NG_BARS3");
            selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS EC_NG_DETAILS3_JP");
            selectQuery.Append(", DEFECT3.OTHER_NAME_SHORT AS EC_NG_DETAILS3_EN");
            //selectQuery.Append(", EC.EC_NG_PERCENTAGE AS EC_NG_PERCENTAGE, ");
            selectQuery.Append(", EC.EC_NG_BAR_TOTAL AS EC_NG_BAR_TOTAL, "); 
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PROCESS_PLAN_KUBUN, ");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN,M.MSG_CONTENTS AS MESSAGE_CONTENTS, ");
            selectQuery.Append(" EC.EC_DECISION AS EC_DECISION");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER JOIN TRN_EC EC ON EC.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");            
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = EC.MSG_CD");            
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = EC.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
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
           
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON EC.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = EC.EC_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = EC.EC_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3 ");
            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                if (gridsearchresult.ROW_LIMIT > 0)
                {
                    whereCondition = " WHERE ROWNUM <=" + gridsearchresult.ROW_LIMIT + " AND ";
                }
                else
                {
                    whereCondition = " WHERE ";
                }

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " and ";
                }

                query = query + whereCondition;
            }
            else if (gridsearchresult.ROW_LIMIT > 0 && searchCriteria.Count <= 0)
            {
                whereCondition = " WHERE ROWNUM <=" + gridsearchresult.ROW_LIMIT;
                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return ExecuteQueryMultiResult<ECDetails>(query);
        }

       
        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/04       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForECFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");            
            selectQuery.Append(" FROM TRN_EC EC");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = EC.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = EC.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = EC.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = EC.EC_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF1 ON DEF1.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF2 ON DEF2.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF3 ON DEF3.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = EC.EC_NEXT_PROCESS");
            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/05       
        /// used screens: Daily Process Analysis (CSV Export)
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForECDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");         
           
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER JOIN TRN_EC EC ON EC.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = EC.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = EC.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
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

            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON EC.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = EC.EC_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = EC.EC_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3 ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }
        public ECDetails GetPastECDetailsByProcessPlanCD(short machineCD, int processPlanCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");

            selectQuery.Append(" EC_METALLIC_PLATE_CODE  ");
            selectQuery.Append(" ,TP_TEST ");
            selectQuery.Append(" ,ACT_ALLOC_UNITS ");
            selectQuery.Append(" ,ACT_ALLOC_QTY ");
            selectQuery.Append(" ,IS_OK ");
            selectQuery.Append(" ,EC_COIL_SIZE ");
            selectQuery.Append(" ,EC_COIL_SHAPE ");
            selectQuery.Append(" ,FURNACE_NO ");
            selectQuery.Append(" ,SHIKII_VALUE ");
            selectQuery.Append(" ,LINE_SPEED ");
            selectQuery.Append(" FROM TRN_EC ");
            selectQuery.Append(" WHERE MACHINE_CD = " + machineCD + " AND PROCESS_PLAN_CD = " + processPlanCD);
            selectQuery.Append("AND  EC_START_DATE >= TO_DATE(SYSDATE)-" + days + " ORDER BY EC_START_DATE DESC");
            var result = ExecuteQuerySingleResult<ECDetails>(selectQuery.ToString());
            return result;
        }
       public List<GridSearchFields> GetHardnessDescion(long processPlanCD)
       {
            List<GridSearchFields> result= new List<GridSearchFields>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" DETAIL_NO AS TABA_NO ");
            selectQuery.Append(", HD_DECISION AS DECISION ");
            selectQuery.Append(" FROM  ");
            selectQuery.Append(" TRN_MI_HARDNESS  ");
            selectQuery.Append(" WHERE PROCESS_PLAN_CD = '" + processPlanCD + "'");
            selectQuery.Append(" GROUP BY  DETAIL_NO,HD_DECISION,UPDATED_DATE ");
            selectQuery.Append(" ORDER BY  UPDATED_DATE DESC ");
            result = ExecuteEdiQueryMultiResult<GridSearchFields>(selectQuery.ToString());
            return result;

        }
        public List<GridSearchFields> GetTensileDescion(long processPlanCD)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" DETAIL_NO AS TABA_NO ");
            selectQuery.Append(", TS_DECISION AS DECISION ");
            selectQuery.Append(" FROM  ");
            selectQuery.Append(" TRN_MI_TENSILE_STRENGTH  ");
            selectQuery.Append(" WHERE PROCESS_PLAN_CD = '" + processPlanCD + "'");
            selectQuery.Append(" GROUP BY  DETAIL_NO,TS_DECISION,UPDATED_DATE ");
            selectQuery.Append(" ORDER BY  UPDATED_DATE DESC ");
            result = ExecuteEdiQueryMultiResult<GridSearchFields>(selectQuery.ToString());
            return result;

        }
    }
}
