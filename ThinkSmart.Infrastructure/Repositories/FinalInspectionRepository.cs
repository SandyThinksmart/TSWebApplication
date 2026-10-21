using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Resources;
using System.Collections;
using System.Linq;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class FinalInspectionRepository : BaseRepository,IFinalInspectionRepository
    {
        private readonly LocalizationServices _localization;
        public FinalInspectionRepository(IConfiguration _configuration,LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Final Inspection process data according to search criteria        
        /// </returns>
        /// 
        public List<FinalInspectionDetails> GetFinalInspectionDataOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria ,int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<FinalInspectionDetails> result = new List<FinalInspectionDetails>();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOCA.SHORT_NAME AS PROC_LOCATION_NAME,");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD,");
            selectQuery.Append(" TO_CHAR(FI.INSP_DATE, 'yyyy/mm/dd HH24:MI') AS INSPECTED_DATE_STRING,");
            //selectQuery.Append("  FI.FI_DIM1,FI.FI_DIM2,FI.FI_DIM3,Fi.FI_DIM4,");
            selectQuery.Append(" EMP.SHORT_NAME AS EMPLOYEE_NAME,");
            selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST,");
            selectQuery.Append(" PP.CHARGE AS CHARGE,");
            selectQuery.Append(" NVL(NIFUDA.NIFUDA_NO,FI.FI_NIFUDA_NO) AS FI_NIFUDA_NO,");
            selectQuery.Append(" PP.PROD_DIM_D AS DIM_DD,");
            selectQuery.Append(" FI.FI_LENGTH AS FI_LENGTH,");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS FI_REMARK,INTE.INTERNAL_NAME,");
            selectQuery.Append(" pp.CHAMFER,");
            selectQuery.Append(" PP.PROD_SHAPE AS PROD_DIM_D_SHAPE,");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2,");
            selectQuery.Append(" FI.FI_TABA_NO AS FI_TABA_NO,");
            //selectQuery.Append(" PP.PROD_LENGTH,");
            selectQuery.Append(" FI.SPARK_INSP_BY, ");
            selectQuery.Append(" FI.FI_SPARK_DECISION AS FI_SPARK_DECISION,");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" FI.NO_OF_BARS AS FI_NO_OF_BARS, ");
            selectQuery.Append(" FI.UNIT_TYPE, "); 
            selectQuery.Append(" FI.FI_QTY AS PP_FINISHED_QTY, ");
            selectQuery.Append(" FI.COMBINED_DECISION AS COMBINED_DECISION, ");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM1, 'FM990.000') AS FI_DIM1_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM2, 'FM990.000') AS FI_DIM2_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM3, 'FM990.000') AS FI_DIM3_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM4, 'FM990.000') AS FI_DIM4_STRING,");
            //selectQuery.Append(" PP.COLOR_ID, ");
            selectQuery.Append(" FI.FI_DIM_DECISION,");
            selectQuery.Append(" FI.FI_LENGTH_DECISION,");
            selectQuery.Append(" FI.FI_COLOR_DECISION,");
            selectQuery.Append(" FI.FI_CHAMFERING_DECISION,");
            selectQuery.Append(" FI.FI_EXT_TEXTURE_DECISION,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
            //selectQuery.Append(" PP.PROD_LENGTH AS PROD_LENGTH_STRING,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PROD_LENGTH_STRING, ");
            selectQuery.Append(" CONCAT(PP.LENGTH_TOL_MAX_CHR, CASE WHEN PP.LENGTH_TOL_MIN_CHR IS NOT NULL THEN  CONCAT(',', PP.LENGTH_TOL_MIN_CHR) ELSE NULL  END) AS ALL_LENGTH_TOL_MIN_CHR,");
            selectQuery.Append(" CONCAT(PP.DIM_TOL_MAX_CHR, CASE WHEN PP.DIM_TOL_MIN_CHR IS NOT NULL THEN  CONCAT(',', PP.DIM_TOL_MIN_CHR) ELSE NULL  END) AS ALL_DIM_TOL_MIN_MAX_CHR");
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD =FI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = FI.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIFUDA ON NIFUDA.NIFUDA_CD=FI.NIFUDA_CD ");

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
                    andOperator = " AND ";
                }
                query = query + whereCondition;
                
            }
            else if (rowLimit > 0 && searchFields.Count <= 0)
            {
                whereCondition = " WHERE ROWNUM <=" + rowLimit;
                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }
            //else
            //{
            //    query += " ORDER BY INSPECTED_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result= ExecuteQueryMultiResult<FinalInspectionDetails>(query);            
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Final Inspection process data according to search criteria        
        /// </returns>
        /// 
        public List<FinalInspectionDetails> GetFinalInspectionDataOnDailyProcessStatus(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VER,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME,"); 
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST,");
            selectQuery.Append(" PP.PROD_SHAPE AS PROD_DIM_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS DIM_DD,");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
            //selectQuery.Append(" WHEN STR_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER,");
            selectQuery.Append(" PP.MAGARI_KUBUN,");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PR_MAGARI_BEND_TOL,");
            selectQuery.Append(" PP.MAGARI_SPAN,");
            selectQuery.Append(" PP.NEJIRE_ANGLE,");
            selectQuery.Append(" PP.NEJIRE_SPAN,");
            selectQuery.Append(" PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN,");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN,");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX,");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE,");
            selectQuery.Append(" PP.COLOR_ID AS COLOR,");
            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY,");

            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
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

            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.YEILD_PATTERN1,");
            selectQuery.Append(" PP.YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append(" PP.YEILD_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.PRESS_PATTERN1,");
            selectQuery.Append(" PP.PRESS_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX,");
            selectQuery.Append(" PP.PRESS_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.DECARB_1,");
            selectQuery.Append(" PP.DECARB_VAL_1,");
            selectQuery.Append(" PP.DECARB_2,");
            selectQuery.Append(" PP.DECARB_VAL_2,");
            selectQuery.Append(" PP.DECARB_3,");
            selectQuery.Append(" PP.DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.TEST_SPEC_REMARKS,");
            selectQuery.Append(" PP.CORNER_RADIUS AS CORNER_RADIUS,");
            selectQuery.Append(" PP.MBEU,");
            selectQuery.Append(" PP.RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION,");
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK,");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");            
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PP_PROCESS_PLAN_KUBUN,");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN,");
            selectQuery.Append(" PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN,");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN,");
            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR,");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(FI.INSP_DATE, 'yyyy/mm/dd HH24:MI') AS INSPECTED_DATE_STRING,");
            selectQuery.Append(" EMP2.SHORT_NAME AS OPERATOR_NAME, ");
            selectQuery.Append(" NVL(NIFUDA.NIFUDA_NO,FI.FI_NIFUDA_NO) AS FI_NIFUDA_NO,");
            selectQuery.Append(" FI.FI_TABA_NO,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM1, 'FM990.000') AS FI_DIM1_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM2, 'FM990.000') AS FI_DIM2_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM3, 'FM990.000') AS FI_DIM3_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM4, 'FM990.000') AS FI_DIM4_STRING,");
            selectQuery.Append(" FI.FI_DIM_DECISION AS FI_DIM_DECISION,");
            selectQuery.Append(" FI.FI_LENGTH,");
            selectQuery.Append(" FI.FI_LENGTH_DECISION AS FI_LENGTH_DECISION,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_1,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_2,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_3,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_4,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_5,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_5,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_6,");
            selectQuery.Append(" FI.FI_CORNER_RADIUS_STD_DECISION AS FI_CORNER_RADIUS_STD_DECISION,");
            selectQuery.Append(" FI.FI_COLOR_DECISION AS FI_COLOR_DECISION,");
            selectQuery.Append(" FI.FI_CHAMFERING_DECISION AS FI_CHAMFERING_DECISION,");
            selectQuery.Append(" FI.FI_EXT_TEXTURE_DECISION AS FI_EXT_TEXTURE_DECISION,");
            selectQuery.Append(" FI.FI_SPARK_DECISION AS FI_SPARK_DECISION,");
            selectQuery.Append(" FI.NO_OF_BARS AS FI_NO_OF_BARS,");
            selectQuery.Append(" FI.UNIT_TYPE,");
            selectQuery.Append(" FI.FI_QTY AS FI_QTY_1,");
            selectQuery.Append(" FI.COMBINED_DECISION AS COMBINED_DECISION,");
            selectQuery.Append(" EMP3.SHORT_NAME AS SPARK_INSP_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(FI.SPARK_INSP_DATE,'yyyy/mm/dd HH24:MI') AS SPARK_INSP_DATE_STRING,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS FI_REMARK ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");           
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=FI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=FI.INSP_BY ");
            //selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIF ON NIF.NIFUDA_CD = FI.NIFUDA_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP3 ON EMP3.EMPLOYEE_CD=FI.SPARK_INSP_BY ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = FI.MSG_CD ");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIFUDA ON NIFUDA.NIFUDA_CD=FI.NIFUDA_CD ");


            //selectQuery.Append("  WHERE ROWNUM <= 1000 ");
            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchFields != null && searchFields.Count > 0)
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

                foreach (var searchField in searchFields)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }
                query = query + whereCondition;
                //    query = query + whereCondition;
            }
            else if (gridsearchresult.ROW_LIMIT > 0 && searchFields.Count <= 0)
            {
                whereCondition = " WHERE ROWNUM <=" + gridsearchresult.ROW_LIMIT;
                query = query + whereCondition;
            }
            //if (!string.IsNullOrEmpty(sortCriteria))
            //{
            //    query = Helper.ApplySortCriteria(sortCriteria, query);
            //}
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return ExecuteQueryMultiResult<FinalInspectionDetails>(query);
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
        public int GetRowCountForFinalInspectionFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");           
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD =FI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = FI.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIFUDA ON NIFUDA.NIFUDA_CD=FI.NIFUDA_CD ");

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
        /// CREATED BY : Pallavi Gawade on 2024/07/08       
        /// used screens: Daily Process Analysis (CSV Export)
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForFinalInspectionDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT (*) "); 
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=FI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=FI.INSP_BY ");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIF ON NIF.NIFUDA_CD = FI.NIFUDA_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP3 ON EMP3.EMPLOYEE_CD=FI.SPARK_INSP_BY ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = FI.MSG_CD ");

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
        /// Get Final Inspect Details for  Report IsExit.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="endOperator"></param>
        /// <param name="orderBy"></param>
        /// <param name="printFlag"></param>
        /// <param name="isOnlyUnPrintFlag"></param>
        /// <param name="userID"></param>
        /// <returns></returns>

        public List<FinalInspectionDetails> GetFinalInspectDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isOnlyUnPrintFlag, int userID)
        {
            StringBuilder WhereCondition = new StringBuilder();
            //string WhereCondition  = string.Empty;
            StringBuilder WhereSubQueryCondition = new StringBuilder();

            //if (isOnlyUnPrintFlag == 0)
            //{
            //    WhereCondition.Append(" TO_DATE(INSP_DATE_VALUE,'yyyy/mm/dd hh24:mi') BETWEEN TO_DATE('" + startDate + "','yyyy/mm/dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy/mm/dd hh24:mi') ");
            //    WhereSubQueryCondition.Append(" INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy/mm/dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy/mm/dd hh24:mi') ");
            //    WhereCondition.Append(" AND INPUT_LOCATION_CD = ");
            //    WhereCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            //    WhereSubQueryCondition.Append(" AND FI.INPUT_LOCATION_CD = ");
            //    WhereSubQueryCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            //}
            //else
            //{
            //    //WhereSubQueryCondition.Append(" FI.PRINT_FLAG IN(" + printFlag + ")");
            //    //WhereCondition.Append(" PRINT_FLAG IN (" + printFlag + ") ");
            //    WhereSubQueryCondition.Append(" FI.PRINT_DATE IS NULL");              //[08/09/2017] Sagar B.this will always be 0.As it will show records whows pront flag is 0
            //    WhereCondition.Append(" PRINT_DATE IS NULL ");
            //    WhereCondition.Append(" AND INPUT_LOCATION_CD = ");
            //    WhereCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            //    WhereSubQueryCondition.Append(" AND FI.INPUT_LOCATION_CD = ");
            //    WhereSubQueryCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            //    // WHERE_CONDITION = " PRINT_DATE IS NULL ";
            //}
            WhereCondition.Append(" TO_DATE(INSP_DATE_VALUE,'yyyy/mm/dd hh24:mi') BETWEEN TO_DATE('" + startDate + "','yyyy/mm/dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy/mm/dd hh24:mi') ");
            WhereSubQueryCondition.Append(" INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy/mm/dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy/mm/dd hh24:mi') ");
            WhereCondition.Append(" AND INPUT_LOCATION_CD = ");
            WhereCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            WhereSubQueryCondition.Append(" AND FI.INPUT_LOCATION_CD = ");
            WhereSubQueryCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            if (endOperator != "0")    /// If Operator is select from FIPrintReportScreen.
            {
                WhereCondition.Append(" AND INSP_BY = " + endOperator);
            }

            StringBuilder selectQuery = new StringBuilder();
            try
            {

                selectQuery.Append("SELECT * FROM(");
                selectQuery.Append(" SELECT CN.SHORT_NAME AS CUSTOMER_NAME");
                selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN");
                selectQuery.Append(", TO_CHAR(PP.GYOUBAN)");
                selectQuery.Append(",CONCAT(CONCAT(PP.CHUUBAN, '-'), PP.GYOUBAN) AS CHUBAN_GYOBAN ");   
                selectQuery.Append(",PP.PROD_SPEC_NO");
                selectQuery.Append(",PP.MATL_NAME_MAK as PROD_NAME_INT ");
                selectQuery.Append(",PP.CHARGE");
                selectQuery.Append(",PP.PROD_DIM_D AS DIM_D_VALUE ");
                selectQuery.Append(",PP.PROD_SHAPE");
                selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W, PP.PROD_SHAPE,'') AS PROD_DIM_D_SHAPE ");
                selectQuery.Append(",( PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ) AS DIM_TOL");
                selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS DIM_TOL_MIN");
                selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS DIM_TOL_MAX");
                selectQuery.Append(", FI.FI_DIM1");
                selectQuery.Append(",FI.FI_DIM2");
                selectQuery.Append(",PP.PROD_LENGTH");
                selectQuery.Append(",To_Char(FI.No_Of_Bars) AS ORG_COILS");
                selectQuery.Append(", (PP.LENGTH_TOL_MIN_CHR ||','|| PP.LENGTH_TOL_MAX_CHR) AS LEN_TOL");
                selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PROD_LENGTH_TOL_MAX  ");
                selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PROD_LENGTH_TOL_MIN  ");
                selectQuery.Append(", NULL AS MACHINABILITY_VALUE");               
                selectQuery.Append(",FI.FINAL_INSP_CD AS FINAL_INSP_NO");
                selectQuery.Append("");
                selectQuery.Append(",FI.NO_OF_BARS ");
                selectQuery.Append(",FI.UNIT_TYPE ");
                selectQuery.Append(", FI.FI_TABA_NO AS TABA_NO");
                
                selectQuery.Append(",FI.INSP_BY");

                selectQuery.Append(",FI_QTY");

                selectQuery.Append(" ,FI.FI_DIM_DECISION");
                selectQuery.Append(" ,FI.FI_LENGTH_DECISION");
                selectQuery.Append(" ,FI.COMBINED_DECISION");
                selectQuery.Append(" ,FI.FI_COLOR_DECISION");
                selectQuery.Append(" ,FI.FI_CHAMFERING_DECISION");
                selectQuery.Append(" ,FI.FI_SPARK_DECISION");
                selectQuery.Append(" ,FI.FI_EXT_TEXTURE_DECISION");

                selectQuery.Append(", FI.FI_LENGTH AS FI_ACT_LENGTH");
                selectQuery.Append(", INSP_DATE");
                selectQuery.Append(" ,TO_CHAR(FI.INSP_DATE,'YYYY/MM/DD hh24:mi') AS INSP_DATE_VALUE ");
                selectQuery.Append(",FI.PRINT_DATE AS PRINT_DATE ");
                selectQuery.Append(" , EMP.SHORT_NAME AS OPERATOR_NAME ");
                selectQuery.Append(" ,PP.PROD_NAME_CUST ");

                selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR AS DIM_TOL_MAX_CHR ");
                selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR AS DIM_TOL_MIN_CHR ");
                selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR ");
                selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR ");
                selectQuery.Append(",FI.INPUT_LOCATION_CD");

                selectQuery.Append(",GRP_COUNT.GROUP_COUNT ");
                selectQuery.Append(",SUM_FI_QTY");

                selectQuery.Append("  FROM  TRN_FINAL_INSPECTION FI ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP  ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");

                selectQuery.Append(" INNER JOIN MST_CUSTOMER CN ON CN.CUSTOMER_CD = PP.CUSTOMER_CD");


                selectQuery.Append(" INNER JOIN  MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY ");

                selectQuery.Append(" INNER JOIN (SELECT COUNT(INSP_BY) AS GROUP_COUNT,MAX(FI.INSP_BY) AS INSP_BY ");
                selectQuery.Append(",SUM(FI.FI_QTY) AS SUM_FI_QTY");
                selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");



                selectQuery.Append(" WHERE ");
                selectQuery.Append(WhereSubQueryCondition);    
                selectQuery.Append(" GROUP BY INSP_BY ) GRP_COUNT ON GRP_COUNT.INSP_BY = FI.INSP_BY ) ");

                selectQuery.Append(" WHERE ");
                selectQuery.Append(WhereCondition);
                selectQuery.Append(" ORDER BY ");

                if (orderBy == 0)     
                {
                    if (isOnlyUnPrintFlag == 0)
                    {
                        selectQuery.Append(" INSP_DATE ");
                    }
                    else
                    {
                        selectQuery.Append(" INSP_BY ");
                    }
                }
                else
                {
                    selectQuery.Append(" CHUBAN_GYOBAN ");
                    selectQuery.Append(",TABA_NO");
                }

                var result = ExecuteQueryMultiResult<FinalInspectionDetails>(selectQuery.ToString()).ToList();
                return result;
               
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get start chubban 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="printFlag"></param>
        /// <param name="isOnlyUnPrintFlag"></param>
        /// <returns></returns>
        public List<FinalInspectionDetails> GetStartChuuban(string startDate, string endDate, string printFlag, int isOnlyUnPrintFlag)
        {
            string WHERE_CONDITION = string.Empty;
            if (isOnlyUnPrintFlag == 0)
                WHERE_CONDITION = " PRINT_DATE IS NULL ";
            else
                WHERE_CONDITION = " TO_DATE(INSP_DATE_VALUE,'YYYY/MM/DD') BETWEEN TO_DATE('" + Convert.ToDateTime(startDate).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy/mm/dd') AND TO_DATE('" + Convert.ToDateTime(endDate).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy/mm/dd') ";

            StringBuilder selectQuery = new StringBuilder();
            try
            {

                selectQuery.Append("SELECT   * FROM (");
                selectQuery.Append("SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN");
                selectQuery.Append(",TO_CHAR(PP.GYOUBAN) AS GYOUBAN");
                selectQuery.Append(",TO_CHAR(FI.INSP_DATE ,'YYYY/MM/dd') AS INSP_DATE_VALUE");
                selectQuery.Append(",CN.CUSTOMER_NAME AS CUSTOMER_NAME ");
                selectQuery.Append(" ,FI.FI_TABA_NO AS TABA_NO");
                selectQuery.Append(",FI.PRINT_DATE AS PRINT_DATE");
                selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
                selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION FI ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER  CN ON CN.CUSTOMER_CD=PP.CUSTOMER_CD");
               
                selectQuery.Append(")");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(WHERE_CONDITION);
                selectQuery.Append(" ORDER BY INSP_DATE_VALUE, CUSTOMER_NAME,CHUUBAN,GYOUBAN,TABA_NO");

               
                var result = ExecuteQueryMultiResult<FinalInspectionDetails>(selectQuery.ToString()).ToList();
                return result;
                
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }
        public int GetFinishProductDetails(string startDate, string endDate, string endOperator, int printFlag, int userID)
        {
            StringBuilder selectQuery = new StringBuilder();
            endOperator = "0";
            int count;
            StringBuilder WhereCondition = new StringBuilder();
            try
            {
                WhereCondition.Append(" INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy-mm-dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy-mm-dd hh24:mi') ");
                if (endOperator != "0")    /// If Operator is select from FIPrintReportScreen.
                {
                    WhereCondition.Append(" AND INSP_BY = " + endOperator);
                }
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" count(*) ");
                selectQuery.Append(" FROM  TRN_PROCESS_PLAN PRO ");
                selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION  FI  ON FI.PROCESS_PLAN_CD = PRO.PROCESS_PLAN_CD");
                selectQuery.Append(" INNER JOIN  MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY");
                selectQuery.Append(" INNER JOIN (SELECT COUNT(INSP_BY) AS GROUP_COUNT,MAX(FI.INSP_BY) AS INSP_BY ");
                selectQuery.Append(",SUM(FI.FI_QTY) AS SUM_FI_QTY");
                selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy-mm-dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy-mm-dd hh24:mi')");
                if (printFlag ==1)
                {
                    selectQuery.Append(" AND PRINT_DATE IS NULL");
                }
                
                selectQuery.Append(" AND FI.INPUT_LOCATION_CD = ");
                selectQuery.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
                selectQuery.Append(" GROUP BY INSP_BY ) GRP_COUNT ON GRP_COUNT.INSP_BY = FI.INSP_BY  ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(WhereCondition);
                count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                return count;

            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }

        public int GetSimpleFinalInspectionDetails(string startDate, string endDate, string endOperator, int printFlag, int userID)
        {
            StringBuilder selectQuery = new StringBuilder();
            StringBuilder whereCondition = new StringBuilder();

            //endOperator = "0";

            try
            {
                if (printFlag != 1)
                {
                    whereCondition.Append(
                        " FI.INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy-mm-dd hh24:mi') " +
                        "AND TO_DATE('" + endDate + "','yyyy-mm-dd hh24:mi') ");

                    if (endOperator != "0")
                    {
                        whereCondition.Append(" AND FI.INSP_BY = " + endOperator);
                    }
                }
                else
                {
                    whereCondition.Append(" FI.PRINT_DATE IS NULL ");
                }

                selectQuery.Append("SELECT COUNT(*) ");
                selectQuery.Append("FROM TRN_PROCESS_PLAN PRO ");
                selectQuery.Append("INNER JOIN TRN_FINAL_INSPECTION FI ON FI.PROCESS_PLAN_CD = PRO.PROCESS_PLAN_CD ");
                selectQuery.Append("INNER JOIN MST_CUSTOMER CN ON CN.CUSTOMER_CD = PRO.CUSTOMER_CD ");
                selectQuery.Append("INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY ");
                selectQuery.Append("LEFT JOIN TRN_MESSAGE MS ON MS.MSG_CD = FI.MSG_CD ");

                selectQuery.Append("INNER JOIN ( ");
                selectQuery.Append("SELECT COUNT(INSP_BY) AS GROUP_COUNT, ");
                selectQuery.Append("MAX(FI.INSP_BY) AS INSP_BY, ");
                selectQuery.Append("SUM(FI.FI_QTY) AS SUM_FI_QTY ");
                selectQuery.Append("FROM TRN_FINAL_INSPECTION FI ");
                selectQuery.Append("INNER JOIN TRN_PROCESS_PLAN PP ");
                selectQuery.Append("ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
                selectQuery.Append("WHERE ");

                if (printFlag != 1)
                {
                    selectQuery.Append(
                        "FI.INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy-mm-dd hh24:mi') " +
                        "AND TO_DATE('" + endDate + "','yyyy-mm-dd hh24:mi') ");

                    if (endOperator != "0")
                    {
                        selectQuery.Append("AND FI.INSP_BY = " + endOperator + " ");
                    }
                }
                else
                {
                    selectQuery.Append("FI.PRINT_DATE IS NULL ");
                }

                selectQuery.Append("AND FI.EC_CD IS NULL ");
                selectQuery.Append("AND FI.INPUT_LOCATION_CD = ");
                selectQuery.Append("(SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ") ");
                selectQuery.Append("GROUP BY INSP_BY ");
                selectQuery.Append(") GRP_COUNT ON GRP_COUNT.INSP_BY = FI.INSP_BY ");

                selectQuery.Append("WHERE ");
                selectQuery.Append("FI.INPUT_LOCATION_CD = ");
                selectQuery.Append("(SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ") ");
                selectQuery.Append("AND FI.EC_CD IS NULL ");
                selectQuery.Append("AND ");
                selectQuery.Append(whereCondition);

                int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                return count;
            }
            
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }

    }
    
}
