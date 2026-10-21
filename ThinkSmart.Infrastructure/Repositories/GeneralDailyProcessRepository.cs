using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class GeneralDailyProcessRepository : BaseRepository, IGeneralDailyProcessRepository
    {

        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly string _General_Daily_Process_Cd;

        private readonly string _specialChuuban;
        public GeneralDailyProcessRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _General_Daily_Process_Cd = _configuration.GetSection("General_Daily_Process_Cd").Value;
            _localization = localization;
            _specialChuuban = _configuration.GetSection("SpecialChuuban").Value;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/03/08       
        /// used screens: Daily Process Analysis (CSV Export)
        /// </summary>
        /// <returns>
        /// List of General Process data according to search criteria 
        /// </returns>
        ///    
        public List<GeneralDailyProcessDetails> GetGeneralDailyProcessData(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");

            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN, 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS PP_INTERNAL_NAME, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_WINDING AS COIL_DIA_WINDING, ");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION,");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER,");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN AS MAGARI_KUBUN, ");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN, ");
            selectQuery.Append(" PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN, ");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE, ");
            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");
            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
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
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10,");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11,");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12,");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13,");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN3_ACTUAL AS HARDNESS_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");
            selectQuery.Append(" PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX, ");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");
            selectQuery.Append(" PP.YEILD_PATTERN3_ACTUAL AS YEILD_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");
            selectQuery.Append(" PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");
            selectQuery.Append(" PP.PRESS_PATTERN3_ACTUAL AS PRESS_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.DECARB_1 AS DECARB_1, ");
            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");
            selectQuery.Append(" PP.DECARB_2 AS DECARB_2, ");
            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");
            selectQuery.Append(" PP.DECARB_3 AS DECARB_3, ");
            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS PP_IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS PP_IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS PP_IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK, ");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PP_PROCESS_PLAN_KUBUN, ");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'YYYY/MM/DD') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");

            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS GP_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS GP_LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(GP.GP_START_DATE, 'YYYY/MM/DD HH24:MI') AS GP_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS GP_START_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(GP.GP_END_DATE, 'YYYY/MM/DD HH24:MI') AS GP_END_DATE_STRING, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS GP_END_OPERATOR_NAME, ");
            selectQuery.Append(" GP.GP_OK_UNITS AS GP_OK_UNITS_SINGLE, ");
            selectQuery.Append(" GP.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" GP.GP_QTY AS GP_QTY_DOUBLE, ");
            selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT1_JP, ");
            selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT1_EN, ");
            selectQuery.Append(" GP.GP_NG_UNITS1, ");
            selectQuery.Append(" GP.GP_NG_UNIT_TYPE1 AS GP_NG_UNIT_TYPE1, ");
            selectQuery.Append(" GP.GP_NG_QTY1, ");
            selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT2_JP, ");
            selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT2_EN, ");
            selectQuery.Append(" GP.GP_NG_UNITS2, ");
            selectQuery.Append(" GP.GP_NG_UNIT_TYPE2 AS GP_NG_UNIT_TYPE2, ");
            selectQuery.Append(" GP.GP_NG_QTY2, ");
            selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT3_JP, ");
            selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT3_EN, ");
            selectQuery.Append(" GP.GP_NG_UNITS3, ");
            selectQuery.Append(" GP.GP_NG_UNIT_TYPE3 AS GP_NG_UNIT_TYPE3, ");
            selectQuery.Append(" GP.GP_NG_QTY3, ");
            selectQuery.Append(" GP.GP_TABA_NO_1,");
            selectQuery.Append(" GP.GP_TABA_NO_2,");
            selectQuery.Append(" GP.GP_TABA_NO_3,");
            selectQuery.Append(" GP.GP_TABA_NO_4,");
            selectQuery.Append(" GP.DAILY_PROCESS_NOTE, ");
            selectQuery.Append(" GP.GP_DECISION, ");
            selectQuery.Append(" GP.GP_NEXT_PROCESS, ");
            selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");
            selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_GENERAL_DAILY_PROCESS GP ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = GP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = GP.PROCESS_KUBUN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = GP.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = GP.GP_START_OPERATOR_CD ");

            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = GP.MACHINE_CD ");

            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = GP.GP_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS3 ");

            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = GP.GP_NEXT_PROCESS");


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
                    andOperator = " AND ";
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
            return ExecuteQueryMultiResult<GeneralDailyProcessDetails>(query);
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/03/15       
        /// used screens: GeneralDailyProcessStart and GeneralDailyProcessStartEnd
        /// </summary>
        /// <returns>
        /// Check data validation into table TRN_WIP_STATUS,TRN_PROCESS_PLAN ,TRN_GENERAL_DAILY_PROCESS
        /// </returns>
        /// 
        public GeneralDailyProcessDetails CheckValidationsOnGeneralProcessStart(GeneralDailyProcessDetails processDetails)
        {
            int count = 0;
            int completeDateCnt = 0;
            int thabaCnt = 0;
            int decisionCnt = 0;
            int preProcessCd = 0;
            long processPlanCd = 0;
            var result = string.Empty;
            int generalProCnt = 0;
            GeneralDailyProcessDetails processPlanTabaDetails = new GeneralDailyProcessDetails();
            GeneralDailyProcessDetails processPlanTabaDetailsWithOk = new GeneralDailyProcessDetails();
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' ");
            selectQueryProcessPlanCd.Append(" AND PROCESS_PLAN_START_DATE IS NOT NULL ");
            count = Convert.ToInt32(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (count > 0)
            {
                StringBuilder selectQueryComplProcessCnt = new StringBuilder();
                selectQueryComplProcessCnt.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
                selectQueryComplProcessCnt.Append(" WHERE ");
                selectQueryComplProcessCnt.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' ");
                selectQueryComplProcessCnt.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
                processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryComplProcessCnt.ToString()));
                if (processPlanCd > 0)
                {
                    StringBuilder selectQueryThabaProcessCnt = new StringBuilder();
                    selectQueryThabaProcessCnt.Append("SELECT GP_TABA_NO_1,GP_TABA_NO_2,GP_TABA_NO_3,GP_TABA_NO_4  FROM TRN_GENERAL_DAILY_PROCESS ");
                    selectQueryThabaProcessCnt.Append(" WHERE ");
                    selectQueryThabaProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
                    selectQueryThabaProcessCnt.Append(" AND GP_START_DATE IS NOT NULL AND GP_END_DATE IS NULL ");
                    selectQueryThabaProcessCnt.Append(" AND (GP_TABA_NO_1 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "') ");
                    selectQueryThabaProcessCnt.Append(" OR GP_TABA_NO_2 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "') ");
                    selectQueryThabaProcessCnt.Append(" OR GP_TABA_NO_3 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "') ");
                    selectQueryThabaProcessCnt.Append(" OR GP_TABA_NO_4 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "')) ");
                    processPlanTabaDetails = ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryThabaProcessCnt.ToString());
                    if (processPlanTabaDetails == null)
                    {
                        StringBuilder selectQueryForProcessCD = new StringBuilder();
                        selectQueryForProcessCD.Append("SELECT NVL(PRE_PROCESS,0) FROM TRN_WIP_STATUS ");
                        selectQueryForProcessCD.Append(" WHERE ");
                        selectQueryForProcessCD.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                        selectQueryForProcessCD.Append(" AND TABA_NO = '" + processDetails.GP_TABA_NO_1 + "' ");
                        preProcessCd = Convert.ToInt32(ExecuteScalar(selectQueryForProcessCD.ToString()));
                        processPlanDetails.METALIC_PLATE_NO = GetMetalicNoOfProcess(preProcessCd, processPlanCd, Convert.ToInt16(processDetails.GP_TABA_NO_1), Convert.ToInt16(processDetails.GP_TABA_NO_2), Convert.ToInt16(processDetails.GP_TABA_NO_3), Convert.ToInt16(processDetails.GP_TABA_NO_4));
                        processPlanDetails.PP_CHARGE = getChargeOfProcess(processPlanCd);
                        result = Convert.ToString(processPlanDetails);

                    }
                    else
                    {
                        processPlanTabaDetails.RESPONCE = "Process is already started for this bundle no";
                    }
                }
                else
                {
                    processPlanDetails.RESPONCE = "Chubban is already completed";
                }
            }
            else
            {
                processPlanDetails.RESPONCE = "Chubban is Not Found";
            }
            if (processPlanDetails.METALIC_PLATE_NO != null || processPlanDetails.RESPONCE != null)
            {
                return processPlanDetails;

            }
            else
            {
                return processPlanTabaDetails;
            }

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/03/16       
        /// used screens: GeneralDailyProcessStart
        /// get Metalic Plate no from Process Table using OrNo or TabaNo
        /// </summary>
        /// <returns>
        ///  Metalic Plate no from Process Table 
        /// </returns>
        /// 
        private string GetMetalicNoOfProcess(int preProcessCd, long processPlanCd, int TABA_NO_1, int TABA_NO_2, int TABA_NO_3, int TABA_NO_4)
        {
            string metalicPlateNo = "";
            StringBuilder selectQuery = new StringBuilder();
            switch (preProcessCd)
            {
                case UIConstants.K_PROCESS_DRAWING_B_TO_B:
                    selectQuery.Append("SELECT DW_METALLIC_PLATE_CODE FROM TRN_DRAWING_D ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND DW_TABA_NO_ = '" + TABA_NO_1 + "' ORDER BY DW_END_DATE DESC ");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_DRAWING_C_TO_B:
                    selectQuery.Append("SELECT DW_METALLIC_PLATE_CODE FROM TRN_DRAWING ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                    selectQuery.Append(" AND (DW_TABA_NO_1 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR DW_TABA_NO_2 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR DW_TABA_NO_3 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR DW_TABA_NO_4 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "')) ORDER BY DW_END_DATE DESC");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_EC:
                    selectQuery.Append("SELECT EC_METALLIC_PLATE_CODE FROM TRN_EC ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND EC_TABA_NO = '" + TABA_NO_1 + "' ORDER BY EC_END_DATE DESC ");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_HARD_TEMPERING_INSPECTION:
                    selectQuery.Append("SELECT METALLIC_PLATE_CODE FROM TRN_HARDENING_TEMPERING ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ORDER BY END_DATE_HARDENING DESC ");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_MULTI_ROLL_STRIGHENING://STRIGHENING 1
                    selectQuery.Append("SELECT STR_METALLIC_PLATE_CODE FROM TRN_STRAIGHTENING ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                    selectQuery.Append(" AND (STR_TABA_NO_1 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR STR_TABA_NO_2 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR STR_TABA_NO_3 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR STR_TABA_NO_4 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "')) ORDER BY STR_END_DATE DESC");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_PRESS_STRAIGHTENING://STRIGHENING 2
                    selectQuery.Append("SELECT STR_METALLIC_PLATE_CODE FROM TRN_STRAIGHTENING2 ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                    selectQuery.Append(" AND (STR_TABA_NO_1 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR STR_TABA_NO_2 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR STR_TABA_NO_3 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR STR_TABA_NO_4 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "')) ORDER BY STR_END_DATE DESC");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_STAMPING:
                    selectQuery.Append("SELECT ST_METALLIC_PLATE_CODE FROM TRN_STAMPING ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND ST_TABA_NO = '" + TABA_NO_1 + "' ORDER BY ST_END_DATE DESC ");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                case UIConstants.K_PROCESS_CHAMFER_S:
                    selectQuery.Append("SELECT CH_METALLIC_PLATE_CODE FROM TRN_CHAMFERING ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                    selectQuery.Append(" AND (CH_TABA_NO_1 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR CH_TABA_NO_2 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR CH_TABA_NO_3 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR CH_TABA_NO_4 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "')) ORDER BY CH_END_DATE DESC");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
                default:
                    selectQuery.Append("SELECT GP_METALLIC_PLATE_CODE  FROM TRN_GENERAL_DAILY_PROCESS ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
                    selectQuery.Append(" AND (GP_TABA_NO_1 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR GP_TABA_NO_2 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR GP_TABA_NO_3 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "') ");
                    selectQuery.Append(" OR GP_TABA_NO_4 IN ('" + TABA_NO_1 + "','" + TABA_NO_2 + "','" + TABA_NO_3 + "','" + TABA_NO_4 + "')) ORDER BY GP_END_DATE DESC");
                    metalicPlateNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                    break;
            }
            return metalicPlateNo;
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/03/15       
        /// used screens: Daily Process Analysis (CSV Export)
        /// </summary>
        /// <returns>
        /// Get KUBUN_NAME from MST_PROCESS_KUBUN table
        /// </returns>
        /// 
        public List<GeneralDailyProcessDetails> GetProcessKubunList()
        {
            List<GeneralDailyProcessDetails> result = new List<GeneralDailyProcessDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT KUBUN_ID AS KUBUN_ID,");
            selectQuery.Append(" KUBUN_NAME AS PROCESS_KUBUN_JP ,");
            selectQuery.Append(" KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");
            selectQuery.Append(" FROM MST_PROCESS_KUBUN");
            selectQuery.Append(" ORDER BY KUBUN_ID ASC ");
            result = ExecuteQueryMultiResult<GeneralDailyProcessDetails>(selectQuery.ToString());
            return result;
        }


        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of General Process data according to search criteria        
        /// </returns>
        /// 
        public List<GeneralDailyProcessDetails> GetGeneralProcessOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCiteria, int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<GeneralDailyProcessDetails> result = new List<GeneralDailyProcessDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" TO_CHAR(GP.GP_START_DATE, 'YYYY/MM/DD HH24:MI') AS GP_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(GP.GP_END_DATE, 'YYYY/MM/DD HH24:MI') AS GP_END_DATE_STRING, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS GP_MACHINE_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN, 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" CONCAT(GP_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', GP_TABA_NO_2) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', GP_TABA_NO_3) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', GP_TABA_NO_4) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" )) AS ALL_TABA_NO, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            //
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" CONCAT(DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DIM_TOL_MIN_CHR IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', DIM_TOL_MIN_CHR) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(") AS DIM_TOL_MIN_MAX_CHR, ");
            selectQuery.Append(" CONCAT(LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN LENGTH_TOL_MIN_CHR IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', LENGTH_TOL_MIN_CHR) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(") AS LENGTH_TOL_MIN_MAX_CHR, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" GP.GP_OK_UNITS AS GP_OK_UNITS_SINGLE, ");
            selectQuery.Append(" GP.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" GP.GP_QTY AS GP_QTY_DOUBLE, ");
            selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT1_JP, ");
            selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT1_EN, ");
            selectQuery.Append(" GP.GP_NG_UNITS1, ");
            selectQuery.Append(" GP.GP_NG_UNIT_TYPE1 AS GP_NG_UNIT_TYPE1, ");
            selectQuery.Append(" GP.GP_NG_QTY1, ");
            selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT2_JP, ");
            selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT2_EN, ");
            selectQuery.Append(" GP.GP_NG_UNITS2,  ");
            selectQuery.Append(" GP.GP_NG_UNIT_TYPE2 AS GP_NG_UNIT_TYPE2, ");
            selectQuery.Append(" GP.GP_NG_QTY2, ");
            selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT3_JP, ");
            selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT3_EN, ");
            selectQuery.Append(" GP.GP_NG_UNITS3, ");
            selectQuery.Append(" GP.GP_NG_UNIT_TYPE3 AS GP_NG_UNIT_TYPE3, ");
            selectQuery.Append(" GP.GP_NG_QTY3, ");
            selectQuery.Append(" GP.DAILY_PROCESS_NOTE, ");
            selectQuery.Append(" GP.GP_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" LOC.SHORT_NAME AS GP_LOCATION_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS GP_END_OPERATOR_NAME, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_GENERAL_DAILY_PROCESS GP ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON GP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = GP.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS3 ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = GP.PROCESS_KUBUN_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = GP.GP_NEXT_PROCESS ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD= GP.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD= GP.GP_END_OPERATOR_CD");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
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

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            else if (rowLimit > 0 && searchCriteria.Count <= 0)
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
            //    query += " ORDER BY GP.PROCESS_KUBUN_CD,GP_END_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP_PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<GeneralDailyProcessDetails>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/01/31       
        /// used screens: GeneratDailyProcessEntry
        /// Check Validations for Process,Operator And Machine
        /// </summary>
        /// <returns>
        /// </returns>
        /// 
        public GeneralDailyProcessDetails CheckValidationsForProcessOperatorMachineCD(GeneralDailyProcessDetails processDetails)
        {
            int operatorCdCnt = 0;
            int operatorCdFlagCnt = 0;
            int processCdCnt = 0;
            int machineCdCnt = 0;
            int machineCdFlagCnt = 0;
            GeneralDailyProcessDetails processPlanData = new GeneralDailyProcessDetails();
            StringBuilder selectQueryOperatorCd = new StringBuilder();
            selectQueryOperatorCd.Append("SELECT COUNT(*) FROM MST_EMPLOYEE ");
            selectQueryOperatorCd.Append("WHERE ");
            selectQueryOperatorCd.Append("EMPLOYEE_CD = '" + processDetails.GP_START_OPERATOR_CD + "' ");
            operatorCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryOperatorCd.ToString()));
            if (operatorCdCnt > 0)
            {
                StringBuilder selectQueryOperatorCdFlag = new StringBuilder();
                selectQueryOperatorCdFlag.Append("SELECT COUNT(*) FROM MST_EMPLOYEE ");
                selectQueryOperatorCdFlag.Append("WHERE ");
                selectQueryOperatorCdFlag.Append("EMPLOYEE_CD = '" + processDetails.GP_START_OPERATOR_CD + "' AND IN_ACTIVE_FLAG = 0 ");
                operatorCdFlagCnt = Convert.ToInt32(ExecuteScalar(selectQueryOperatorCdFlag.ToString()));
                if (operatorCdFlagCnt > 0)
                {
                    StringBuilder selectQueryProcessCd = new StringBuilder();
                    selectQueryProcessCd.Append("SELECT COUNT(*) FROM MST_PROCESS_KUBUN ");
                    selectQueryProcessCd.Append(" WHERE ");
                    selectQueryProcessCd.Append(" KUBUN_ID = '" + processDetails.PROCESS_KUBUN_CD + "' ");
                    processCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryProcessCd.ToString()));
                    if (processCdCnt > 0)
                    {
                        if (processDetails.MACHINE_CD != null)
                        {
                            StringBuilder selectQueryMachineCd = new StringBuilder();
                            selectQueryMachineCd.Append("SELECT COUNT(*)  FROM MST_MACHINE ");
                            selectQueryMachineCd.Append(" WHERE ");
                            selectQueryMachineCd.Append(" MACHINE_CD = '" + processDetails.MACHINE_CD + "'");
                            machineCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryMachineCd.ToString()));
                            if (machineCdCnt > 0)
                            {
                                StringBuilder selectQueryMachineCdFlag = new StringBuilder();
                                selectQueryMachineCdFlag.Append("SELECT COUNT(*)  FROM MST_MACHINE ");
                                selectQueryMachineCdFlag.Append(" WHERE ");
                                selectQueryMachineCdFlag.Append(" MACHINE_CD = '" + processDetails.MACHINE_CD + "'  AND IN_ACTIVE_FLAG = 0 ");
                                machineCdFlagCnt = Convert.ToInt32(ExecuteScalar(selectQueryMachineCdFlag.ToString()));
                                if (machineCdFlagCnt > 0)
                                {
                                    processPlanData = getProcessPlanDetails(processDetails);
                                    var result = getOperatorProcessMachineName(processDetails);
                                    if (result != null)
                                    {
                                        processPlanData.PP_EMPLOYEE_NAME = result.PP_EMPLOYEE_NAME;
                                        processPlanData.GP_START_PROCESS_NAME = result.GP_START_PROCESS_NAME;
                                        processPlanData.GP_MACHINE_NAME = result.GP_MACHINE_NAME;
                                    }
                                }
                                else
                                {
                                    processPlanData.RESPONCE = "Machine is Inactive";
                                }


                            }
                            else
                            {
                                processPlanData.RESPONCE = "Machine is not found";

                            }
                        }
                        else
                        {
                            processPlanData = getProcessPlanDetails(processDetails);
                            var result = getOperatorProcessMachineName(processDetails);
                            if (result != null)
                            {
                                processPlanData.PP_EMPLOYEE_NAME = result.PP_EMPLOYEE_NAME;
                                processPlanData.GP_START_PROCESS_NAME = result.GP_START_PROCESS_NAME;
                                processPlanData.GP_MACHINE_NAME = result.GP_MACHINE_NAME;
                            }
                        }
                    }
                    else
                    {
                        processPlanData.RESPONCE = "Process is not found";
                    }
                }
                else
                {
                    processPlanData.RESPONCE = "Operator is Inactive";
                }
            }
            else
            {
                processPlanData.RESPONCE = "Operator is not found";
            }
            return processPlanData;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/15       
        ///Check validation On GeneralProcessEnd And GeneralProcessEnd Screen for Chubban 
        ///Fetch Process Details From TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        /// Validation Message Or Process Details
        /// </returns>
        /// 
        public GeneralDailyProcessDetails getProcessPlanDetails(GeneralDailyProcessDetails processDetails)
        {
            StringBuilder selectQueryGetData = new StringBuilder();
            selectQueryGetData.Append("SELECT PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,PROD_DIM_D AS PP_PROD_DIM_D,PROD_DIM_W AS PP_PROD_DIM_W,PROD_SHAPE AS PP_PROD_SHAPE,PROD_DIM_OTHER AS PP_PROD_DIM_OTHER,DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQueryGetData.Append("DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR,NVL(LENGTH_OTHER,PROD_LENGTH) AS LENGTH,LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQueryGetData.Append("LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR,PROD_NAME_CUST AS PP_PROD_NAME_CUST,MST_CUSTOMER.SHORT_NAME AS PP_CUSTOMER_NAME FROM TRN_PROCESS_PLAN ");
            selectQueryGetData.Append("INNER JOIN MST_CUSTOMER ON TRN_PROCESS_PLAN.CUSTOMER_CD = MST_CUSTOMER.CUSTOMER_CD ");
            selectQueryGetData.Append("WHERE ");
            selectQueryGetData.Append("CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' AND PROCESS_PLAN_CMPL_DATE IS NULL");
            return ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryGetData.ToString());
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/20       
        /// used screens: GeneralDailyProcessStart
        /// get Remark from MST_PROCESS_KUBUN using PROCESS_KUBUN_CD 
        /// </summary>
        /// <returns>
        /// Return Remark
        /// </returns>
        /// 
        public string getRemarksOfProcess(GeneralDailyProcessDetails processDetails)
        {
            string result = "";
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT REMARK_TEMPLATE FROM MST_PROCESS_KUBUN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" KUBUN_ID = '" + processDetails.PROCESS_KUBUN_CD + "' ");
            result = Convert.ToString(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            return result;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/20       
        /// used screens: GeneralDailyProcessStart
        /// Insert Data into from TRN_GENERAL_DAILY_PROCESS
        /// </summary>
        /// <returns>
        /// </returns>
        /// 
        public string saveGeneralProcessPlanEntryData(GeneralDailyProcessDetails processDetails)
        {
            string result;
            StringBuilder insertSql = new StringBuilder();
            insertSql.Append("INSERT INTO TRN_GENERAL_DAILY_PROCESS(");
            insertSql.Append("GEN_PROCESS_CD,");
            insertSql.Append("PROCESS_KUBUN_CD,");
            insertSql.Append("PROCESS_PLAN_CD,");
            insertSql.Append("MACHINE_CD,");
            insertSql.Append("LOCATION_CD,");
            insertSql.Append("GP_START_DATE,");
            insertSql.Append("GP_START_OPERATOR_CD,");
            insertSql.Append("GP_TABA_NO_1,");
            insertSql.Append("GP_TABA_NO_2,");
            insertSql.Append("GP_TABA_NO_3,");
            insertSql.Append("GP_TABA_NO_4,");
            insertSql.Append("DAILY_PROCESS_NOTE,");
            insertSql.Append("USER_ID,");
            insertSql.Append("INPUT_LOCATION_CD,");
            insertSql.Append("CREATED_DATE,");
            insertSql.Append("UPDATED_DATE,");
            insertSql.Append("GP_METALLIC_PLATE_CODE");
            insertSql.Append(")");
            insertSql.Append("VALUES( ");
            insertSql.Append("GEN_DAILY_PROCESS_SEQ.NEXTVAL,");
            insertSql.Append("'" + processDetails.PROCESS_KUBUN_CD + "',");
            insertSql.Append("'" + processDetails.PP_PROCESS_PLAN_CD + "',");
            insertSql.Append("'" + processDetails.MACHINE_CD + "',");
            insertSql.Append("'" + processDetails.LOCATION_CD + "',");//if machine cd is null then save machine location else save user location
            insertSql.Append(" TO_DATE('" + (Convert.ToDateTime(processDetails.GP_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");
            insertSql.Append("'" + processDetails.GP_START_OPERATOR_CD + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_1 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_2 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_3 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_4 + "',");
            insertSql.Append("'" + processDetails.GP_REMARK + "',");
            insertSql.Append("'" + processDetails.USER_ID + "',");
            insertSql.Append("(SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + processDetails.USER_ID + ") ,");
            insertSql.Append("SYSDATE,");
            insertSql.Append("SYSDATE, ");
            insertSql.Append("'" + processDetails.GP_METALLIC_PLATE_CODE + "'");
            insertSql.Append(")");
            result = "Data Inserted Successfully";
            try
            {
                ExecuteNonQuery(insertSql.ToString());
            }
            catch (Exception e)
            {
                if (e.Message.Contains("TRN_GP_UK_USER_C_DATE_PROCESS_CD"))
                {
                    result = "Data Already Exist";
                }
                else
                {
                    throw;
                }
            }
            return result;

        }

        public int GetLocationByUserID(int? UserID)
        {
            int locationCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID='" + UserID + "'");
            locationCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return locationCD;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/03/18       
        /// used screens: GeneralDailyProcessStart
        /// get Charges from Process Table using OrNo From TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        /// </returns>
        /// 
        public string getChargeOfProcess(long processPlanCd)
        {
            string charge = "";
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT CHARGE FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND PROCESS_PLAN_CMPL_DATE IS NULL ");
            charge = Convert.ToString(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            return charge;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/15       
        ///Fetch EmployeeName,ProcessName and MachineName 
        /// </summary>
        /// <returns>
        /// EmployeeName,ProcessName and MachineName 
        /// </returns>
        ///
        public GeneralDailyProcessDetails getOperatorProcessMachineName(GeneralDailyProcessDetails processDetails)
        {
            string charge = "";
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            StringBuilder selectOperatorName = new StringBuilder();
            selectOperatorName.Append("SELECT SHORT_NAME AS PP_EMPLOYEE_NAME FROM MST_EMPLOYEE ");
            selectOperatorName.Append(" WHERE ");
            selectOperatorName.Append(" EMPLOYEE_CD = '" + processDetails.GP_START_OPERATOR_CD + "' ");
            processPlanDetails.PP_EMPLOYEE_NAME = Convert.ToString(ExecuteScalar(selectOperatorName.ToString()));

            StringBuilder selectProcessName = new StringBuilder();
            selectProcessName.Append("SELECT KUBUN_NAME AS GP_START_PROCESS_NAME FROM MST_PROCESS_KUBUN ");
            selectProcessName.Append(" WHERE ");
            selectProcessName.Append(" KUBUN_ID = '" + processDetails.PROCESS_KUBUN_CD + "' ");
            processPlanDetails.GP_START_PROCESS_NAME = Convert.ToString(ExecuteScalar(selectProcessName.ToString()));

            StringBuilder selectMachineName = new StringBuilder();
            selectMachineName.Append("SELECT MACHINE_NAME AS GP_MACHINE_NAME FROM MST_MACHINE ");
            selectMachineName.Append(" WHERE ");
            selectMachineName.Append(" MACHINE_CD = '" + processDetails.MACHINE_CD + "' ");
            processPlanDetails.GP_MACHINE_NAME = Convert.ToString(ExecuteScalar(selectMachineName.ToString()));

            return processPlanDetails;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/03       
        ///Check validation On GeneralProcessEnd Screen for Chubban 
        ///Get Start Process Details From TRN_GENERAL_DAILY_PROCESS And TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        /// Validation Message Or Start Process Details
        /// </returns>
        /// 
        public GeneralDailyProcessDetails CheckValidationsOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails)
        {
            long processPlanCd = 0;
            int dailyStartProcessCnt = 0;
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            GeneralDailyProcessDetails processPlanTabaDetails = new GeneralDailyProcessDetails();
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL");
            processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (processPlanCd > 0)
            {
                StringBuilder selectQueryThabaProcessCnt = new StringBuilder();
                selectQueryThabaProcessCnt.Append("SELECT COUNT(*) OVER(),GP_TABA_NO_1,GP_TABA_NO_2,GP_TABA_NO_3,GP_TABA_NO_4  FROM TRN_GENERAL_DAILY_PROCESS ");
                selectQueryThabaProcessCnt.Append(" WHERE ");
                selectQueryThabaProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
                selectQueryThabaProcessCnt.Append(" AND GP_START_DATE IS NOT NULL AND GP_END_DATE IS NULL ");
                selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                if (processDetails.GP_TABA_NO_2 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_2 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_3 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_3 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_4 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_4 IS NULL ");
                }
                processPlanTabaDetails = ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryThabaProcessCnt.ToString());
            }
            else
            {
                processPlanTabaDetails.RESPONCE = "Chubban is already completed";
            }

            return processPlanTabaDetails;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/07/05       
        ///get data On GeneralProcessEnd Screen for Chubban 
        ///Get Start Process Details From TRN_GENERAL_DAILY_PROCESS And TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        ///  Start Process Details
        /// </returns>
        /// 
        public GeneralDailyProcessDetails getGeneralDailyProcessStartData(GeneralDailyProcessDetails processDetails)
        {
            long processPlanCd = 0;
            int dailyStartProcessCnt = 0;
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            GeneralDailyProcessDetails processPlanTabaDetails = new GeneralDailyProcessDetails();
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL");
            processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (processPlanCd > 0)
            {

                StringBuilder selectQueryDailyStartProcessCnt = new StringBuilder();
                selectQueryDailyStartProcessCnt.Append("SELECT GP.GP_START_OPERATOR_CD,GP.GP_START_DATE,PK.KUBUN_NAME AS PROCESS_KUBUN_JP,PK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN,GP.PROCESS_KUBUN_CD,GP.GP_TABA_NO_1, ");
                selectQueryDailyStartProcessCnt.Append(" GP.GP_TABA_NO_2,GP.GP_TABA_NO_3,GP.GP_TABA_NO_4,GP.LOCATION_CD ");
                selectQueryDailyStartProcessCnt.Append(" FROM TRN_GENERAL_DAILY_PROCESS GP INNER JOIN MST_PROCESS_KUBUN PK ON PK.KUBUN_ID=GP.PROCESS_KUBUN_CD  ");
                selectQueryDailyStartProcessCnt.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = GP.PROCESS_PLAN_CD ");
                selectQueryDailyStartProcessCnt.Append(" WHERE ");
                selectQueryDailyStartProcessCnt.Append(" GP.PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                selectQueryDailyStartProcessCnt.Append(" AND GP.GP_END_DATE IS NULL ");
                //selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                if (processDetails.GP_TABA_NO_1 != null)
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                }
                if (processDetails.GP_TABA_NO_2 != null)
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
                }
                else
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_2 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_3 != null)
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
                }
                else
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_3 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_4 != null)
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
                }
                else
                {
                    selectQueryDailyStartProcessCnt.Append(" AND GP_TABA_NO_4 IS NULL ");
                }
                processPlanDetails = ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryDailyStartProcessCnt.ToString());

            }
            return processPlanDetails;
        }
        ///// <summary>
        ///// CREATED BY : Namrata Ubale on 2024/04/04       
        /////Bind Kubun Name On Next Process Dropdown On GeneralProcessEnd Screen
        /////Using Table MST_NEXT_PROCESS_CONFIGURATION and MST_PROCESS_KUBUN
        ///// </summary>
        ///// <returns>
        ///// </returns>
        ///// 
        //public List<GeneralDailyProcessDetails> GetNextProcessNameList(GeneralDailyProcessDetails processDetails)
        //{
        //    List<GeneralDailyProcessDetails> result = new List<GeneralDailyProcessDetails>();
        //    StringBuilder selectQuery = new StringBuilder();
        //    selectQuery.Append("SELECT PROCESSKUBUN.KUBUN_NAME AS GP_NEXT_PROCESS_NAME,PROCESSKUBUN.KUBUN_ID ");
        //    selectQuery.Append(" FROM MST_NEXT_PROCESS_CONFIGURATION NEXTPROCESS INNER JOIN MST_PROCESS_KUBUN PROCESSKUBUN ON NEXTPROCESS.NEXT_PROCESS_TYPE_CD=PROCESSKUBUN.KUBUN_ID ");
        //    selectQuery.Append(" WHERE ");
        //    selectQuery.Append(" NEXTPROCESS.PROCESS_TYPE_CD = '" + processDetails.PROCESS_KUBUN_CD + "' ORDER BY SHOW_PRIORITY  ");
        //    result = ExecuteQueryMultiResult<GeneralDailyProcessDetails>(selectQuery.ToString());
        //    if (result.Count == 0)
        //    {
        //        StringBuilder selectQueryForKubun = new StringBuilder();
        //        selectQueryForKubun.Append("SELECT KUBUN_NAME AS GP_NEXT_PROCESS_NAME,KUBUN_ID ");
        //        selectQueryForKubun.Append(" FROM MST_PROCESS_KUBUN ");
        //        result = ExecuteQueryMultiResult<GeneralDailyProcessDetails>(selectQueryForKubun.ToString());
        //    }
        //    return result;
        //    // }
        //}
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/10      
        /// used screens: GeneralProcessEnd
        /// Fetch data by using OrNo using table TRN_GENERAL_DAILY_PROCESS to check validation for Employee and Process start and Process End date validation.
        /// </summary>
        /// <returns>

        /// </returns>
        /// 
        public GeneralDailyProcessDetails CheckValidationsForOperatorAndEndDateTime(GeneralDailyProcessDetails processDetails)
        {
            int operatorCdCnt = 0;
            int operatorCdFlag = 0;
            DateTime StartDate;
            DateTime EndDate = Convert.ToDateTime(processDetails.GP_END_DATE);
            GeneralDailyProcessDetails processPlanData = new GeneralDailyProcessDetails();
            StringBuilder selectQueryOperatorCd = new StringBuilder();
            selectQueryOperatorCd.Append("SELECT COUNT(*) FROM MST_EMPLOYEE ");
            selectQueryOperatorCd.Append("WHERE ");
            selectQueryOperatorCd.Append("EMPLOYEE_CD = '" + processDetails.GP_START_OPERATOR_CD + "' ");
            operatorCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryOperatorCd.ToString()));
            if (operatorCdCnt > 0)
            {
                StringBuilder selectQueryOperatorCdFlag = new StringBuilder();
                selectQueryOperatorCdFlag.Append("SELECT COUNT(*) FROM MST_EMPLOYEE ");
                selectQueryOperatorCdFlag.Append("WHERE ");
                selectQueryOperatorCdFlag.Append("EMPLOYEE_CD = '" + processDetails.GP_START_OPERATOR_CD + "' AND IN_ACTIVE_FLAG = 0 ");
                operatorCdFlag = Convert.ToInt32(ExecuteScalar(selectQueryOperatorCdFlag.ToString()));
                if (operatorCdFlag > 0)
                {
                    StringBuilder selectQueryForDate = new StringBuilder();
                    selectQueryForDate.Append("SELECT GP.GP_START_DATE FROM TRN_GENERAL_DAILY_PROCESS GP ");
                    selectQueryForDate.Append("INNER JOIN TRN_PROCESS_PLAN  PP ON PP.PROCESS_PLAN_CD = GP.PROCESS_PLAN_CD ");
                    selectQueryForDate.Append("WHERE ");
                    selectQueryForDate.Append("PP.CHUUBAN || Lpad(PP.GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' ");
                    selectQueryForDate.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                    if (processDetails.GP_TABA_NO_2 != null)
                    {
                        selectQueryForDate.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
                    }
                    else
                    {
                        selectQueryForDate.Append(" AND GP_TABA_NO_2 IS NULL ");
                    }
                    if (processDetails.GP_TABA_NO_3 != null)
                    {
                        selectQueryForDate.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
                    }
                    else
                    {
                        selectQueryForDate.Append(" AND GP_TABA_NO_3 IS NULL ");
                    }
                    if (processDetails.GP_TABA_NO_4 != null)
                    {
                        selectQueryForDate.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
                    }
                    else
                    {
                        selectQueryForDate.Append(" AND GP_TABA_NO_4 IS NULL ");
                    }
                    selectQueryForDate.Append("AND GP.GP_START_DATE IS NOT NULL AND GP.GP_END_DATE IS NULL AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
                    StartDate = Convert.ToDateTime(ExecuteScalar(selectQueryForDate.ToString()));
                    //double diffMinutes = (EndDate - StartDate).TotalMinutes;
                    if (StartDate > EndDate)
                    {
                        processPlanData.RESPONCE = "Select Different Date bcoz start date is greater than end date";
                    }
                    //else if (diffMinutes < 10)
                    //{
                    //    processPlanData.RESPONCE = "Process End time is less than 10 min";
                    //}
                }
                else
                {
                    processPlanData.RESPONCE = "Operator is Inactive";
                }


            }
            else
            {
                processPlanData.RESPONCE = "Operator is not found";
            }
            return processPlanData;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/10      
        /// used screens: GeneralProcessEnd
        /// Fetch Start Process Remark by using OrNo from table TRN_GENERAL_DAILY_PROCESS
        /// </summary>
        /// <returns>

        /// </returns>
        /// 
        public string getRemarksForEndProcess(GeneralDailyProcessDetails processDetails)
        {
            string result = "";
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT DAILY_PROCESS_NOTE FROM TRN_GENERAL_DAILY_PROCESS ");
            selectQueryProcessPlanCd.Append("WHERE ");
            selectQueryProcessPlanCd.Append("PROCESS_PLAN_CD = '" + processDetails.PP_PROCESS_PLAN_CD + "' ");
            //selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
            if (processDetails.GP_TABA_NO_1 != null)
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
            }
            else
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_1 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_2 != null)
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
            }
            else
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_2 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_3 != null)
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
            }
            else
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_3 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_4 != null)
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
            }
            else
            {
                selectQueryProcessPlanCd.Append(" AND GP_TABA_NO_4 IS NULL ");
            }
            selectQueryProcessPlanCd.Append(" AND GP_END_DATE IS NULL ");
            result = Convert.ToString(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            return result;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/07/12       
        /// used screens: GeneralDailyProcessStart
        /// </summary>
        /// <returns>
        /// Check data validation into table TRN_GENERAL_DAILY_PROCESS for Ok Decision
        /// </returns>
        /// 
        public GeneralDailyProcessDetails checkTabaValidationForOkDecision(GeneralDailyProcessDetails processDetails)
        {
            int count = 0;
            int completeDateCnt = 0;
            int thabaCnt = 0;
            int decisionCnt = 0;
            int preProcessCd = 0;
            long processPlanCd = 0;
            var result = string.Empty;
            int generalProCnt = 0;
            GeneralDailyProcessDetails processPlanTabaDetails = new GeneralDailyProcessDetails();
            GeneralDailyProcessDetails processPlanTabaDetailsWithOk = new GeneralDailyProcessDetails();
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();

            StringBuilder selectQueryComplProcessCnt = new StringBuilder();
            selectQueryComplProcessCnt.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryComplProcessCnt.Append(" WHERE ");
            selectQueryComplProcessCnt.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' ");
            selectQueryComplProcessCnt.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
            processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryComplProcessCnt.ToString()));
            if (processPlanCd > 0)
            {

                StringBuilder selectQueryDecisionProcessCnt = new StringBuilder();
                selectQueryDecisionProcessCnt.Append("SELECT GP_TABA_NO_1,GP_TABA_NO_2,GP_TABA_NO_3,GP_TABA_NO_4  FROM TRN_GENERAL_DAILY_PROCESS ");
                selectQueryDecisionProcessCnt.Append(" WHERE ");
                selectQueryDecisionProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND PROCESS_KUBUN_CD = '" + processDetails.PROCESS_KUBUN_CD + "' ");
                selectQueryDecisionProcessCnt.Append(" AND GP_START_DATE IS NOT NULL AND GP_END_DATE IS NOT NULL AND GP_DECISION = 10  ");
                selectQueryDecisionProcessCnt.Append(" AND (GP_TABA_NO_1 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "') ");
                selectQueryDecisionProcessCnt.Append(" OR GP_TABA_NO_2 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "') ");
                selectQueryDecisionProcessCnt.Append(" OR GP_TABA_NO_3 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "') ");
                selectQueryDecisionProcessCnt.Append(" OR GP_TABA_NO_4 IN ('" + processDetails.GP_TABA_NO_1 + "','" + processDetails.GP_TABA_NO_2 + "','" + processDetails.GP_TABA_NO_3 + "','" + processDetails.GP_TABA_NO_4 + "')) ORDER BY GP_END_DATE DESC");
                processPlanTabaDetails = ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryDecisionProcessCnt.ToString());

            }
            if (processPlanTabaDetails != null)
            {
                processPlanTabaDetails.RESPONCE = "Process is  started with ok decision for this bundle no";
            }
            return processPlanTabaDetails;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/10      
        /// used screens: GeneralProcessEnd
        /// update GeneralDailyPocessEnd  Details into TRN_GENERAL_DAILY_PROCESS table on PP_PROCESS_PLAN_CD And GP_TABA_NO_1
        /// </summary>
        /// <returns>
        /// </returns>
        ///
        public string updateGeneralProcessPlanEndData(GeneralDailyProcessDetails processDetails)
        {
            string result;
            StringBuilder updateSql = new StringBuilder();
            updateSql.Append("UPDATE TRN_GENERAL_DAILY_PROCESS SET ");
            updateSql.Append("GP_END_DATE = TO_DATE('" + (Convert.ToDateTime(processDetails.GP_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");

            //updateSql.Append(" GP_END_DATE = TO_DATE('" + Convert.ToDateTime(processDetails.GP_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateSql.Append("GP_END_OPERATOR_CD ='" + processDetails.GP_END_OPERATOR_CD + "',");
            updateSql.Append("GP_OK_UNITS ='" + processDetails.GP_OK_UNITS + "',");
            updateSql.Append("UNIT_TYPE ='" + processDetails.UNIT_TYPE + "',");
            updateSql.Append("GP_QTY ='" + processDetails.GP_QTY + "',");
            updateSql.Append("GP_NG_DETAILS1 ='" + processDetails.GP_NG_DETAILS1 + "',");
            updateSql.Append("GP_NG_UNITS1 ='" + processDetails.GP_NG_UNITS1 + "',");
            updateSql.Append("GP_NG_UNIT_TYPE1 ='" + processDetails.GP_NG_UNIT_TYPE1 + "',");
            updateSql.Append("GP_NG_QTY1 ='" + processDetails.GP_NG_QTY1 + "',");
            updateSql.Append("GP_NG_DETAILS2 ='" + processDetails.GP_NG_DETAILS2 + "',");
            updateSql.Append("GP_NG_UNITS2 ='" + processDetails.GP_NG_UNITS2 + "',");
            updateSql.Append("GP_NG_UNIT_TYPE2 ='" + processDetails.GP_NG_UNIT_TYPE2 + "',");
            updateSql.Append("GP_NG_QTY2 ='" + processDetails.GP_NG_QTY2 + "',");
            updateSql.Append("GP_NG_DETAILS3 ='" + processDetails.GP_NG_DETAILS3 + "',");
            updateSql.Append("GP_NG_UNITS3 ='" + processDetails.GP_NG_UNITS3 + "',");
            updateSql.Append("GP_NG_UNIT_TYPE3 ='" + processDetails.GP_NG_UNIT_TYPE3 + "',");
            updateSql.Append("GP_NG_QTY3 ='" + processDetails.GP_NG_QTY3 + "',");
            updateSql.Append("DAILY_PROCESS_NOTE ='" + processDetails.GP_REMARK + "',");
            updateSql.Append("GP_DECISION ='" + processDetails.GP_DECISION + "',");
            updateSql.Append("GP_NEXT_PROCESS ='" + processDetails.GP_NEXT_PROCESS + "',");
            updateSql.Append("USER_ID ='" + processDetails.USER_ID + "',");
            updateSql.Append("INPUT_LOCATION_CD = (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + processDetails.USER_ID + "),");
            //updateSql.Append("UPDATED_DATE = TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "','yyyy/MM/dd') ");
            updateSql.Append("UPDATED_DATE = SYSDATE ");
            updateSql.Append("WHERE ");
            updateSql.Append("PROCESS_PLAN_CD = '" + processDetails.PP_PROCESS_PLAN_CD + "' ");
            //updateSql.Append("AND GP_TABA_NO_1 = '" + processDetails.GP_TABA_NO_1 + "' ");
            if (processDetails.GP_TABA_NO_1 != null)
            {
                updateSql.Append("AND GP_TABA_NO_1 = '" + processDetails.GP_TABA_NO_1 + "' ");
            }
            if (processDetails.GP_TABA_NO_2 != null)
            {
                updateSql.Append("AND GP_TABA_NO_2 = '" + processDetails.GP_TABA_NO_2 + "' ");
            }
            if (processDetails.GP_TABA_NO_3 != null)
            {
                updateSql.Append("AND GP_TABA_NO_3 = '" + processDetails.GP_TABA_NO_3 + "' ");
            }
            if (processDetails.GP_TABA_NO_4 != null)
            {
                updateSql.Append("AND GP_TABA_NO_4 = '" + processDetails.GP_TABA_NO_4 + "' ");
            }
            updateSql.Append("AND GP_START_DATE IS NOT NULL AND GP_END_DATE IS NULL");
            ExecuteNonQuery(updateSql.ToString());
            result = "Data Inserted Successfully";

            return result;

        }


        //public List<GeneralDailyProcessDetails> GetProcessDefectList(GeneralDailyProcessDetails processDetails)
        //{
        //    List<GeneralDailyProcessDetails> result = new List<GeneralDailyProcessDetails>();
        //    StringBuilder selectQuery = new StringBuilder();
        //    selectQuery.Append("SELECT PROCESS_DEFECT_CD,PROC_DEFECT_SHORT ");
        //    selectQuery.Append("FROM MST_PROCESS_DEFECT WHERE PROCESS_TYPE='" + processDetails.PROCESS_KUBUN_CD + "'");
        //    selectQuery.Append(" ORDER BY LOWER (PROC_DEFECT_SHORT)");
        //    result = ExecuteQueryMultiResult<GeneralDailyProcessDetails>(selectQuery.ToString());
        //    return result;
        //    // }
        //}
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/10      
        /// used screens: GeneralProcessEnd
        /// Get Privious ProcessCD from  TRN_WIP_STATUS table on PP_PROCESS_PLAN_CD
        /// </summary>
        /// <returns>
        /// </returns>
        ///
        public short getProcessFromPreviousProcess(int processPlanCd)
        {
            short nextProcess;
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT NVL(PRE_PROCESS,0) FROM TRN_WIP_STATUS ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
            nextProcess = Convert.ToInt16(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            return nextProcess;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/07      
        /// used screens: GeneralProcessEntryEnd
        /// Insert Data into TRN_GENERAL_DAILY_PROCESS And TRN_WIP_STATUS
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public string saveGeneralProcessPlanEntryEndData(GeneralDailyProcessDetails processDetails)
        {
            string result;
            StringBuilder insertSql = new StringBuilder();
            insertSql.Append("INSERT INTO TRN_GENERAL_DAILY_PROCESS(");
            insertSql.Append("GEN_PROCESS_CD,");
            insertSql.Append("PROCESS_KUBUN_CD,");
            insertSql.Append("PROCESS_PLAN_CD,");
            insertSql.Append("MACHINE_CD,");
            insertSql.Append("LOCATION_CD,");
            insertSql.Append("GP_START_DATE,");
            insertSql.Append("GP_START_OPERATOR_CD,");
            insertSql.Append("GP_END_DATE,");
            insertSql.Append("GP_END_OPERATOR_CD,");
            insertSql.Append("GP_OK_UNITS,");
            insertSql.Append("UNIT_TYPE,");
            insertSql.Append("GP_QTY,");
            insertSql.Append("GP_NG_DETAILS1,");
            insertSql.Append("GP_NG_UNITS1,");
            insertSql.Append("GP_NG_UNIT_TYPE1,");
            insertSql.Append("GP_NG_QTY1,");
            insertSql.Append("GP_NG_DETAILS2,");
            insertSql.Append("GP_NG_UNITS2,");
            insertSql.Append("GP_NG_UNIT_TYPE2,");
            insertSql.Append("GP_NG_QTY2,");
            insertSql.Append("GP_NG_DETAILS3,");
            insertSql.Append("GP_NG_UNITS3,");
            insertSql.Append("GP_NG_UNIT_TYPE3,");
            insertSql.Append("GP_NG_QTY3,");
            insertSql.Append("GP_TABA_NO_1,");
            insertSql.Append("GP_TABA_NO_2,");
            insertSql.Append("GP_TABA_NO_3,");
            insertSql.Append("GP_TABA_NO_4,");
            insertSql.Append("DAILY_PROCESS_NOTE,");
            insertSql.Append("GP_DECISION,");
            insertSql.Append("GP_NEXT_PROCESS,");
            insertSql.Append("USER_ID,");
            insertSql.Append("INPUT_LOCATION_CD,");
            insertSql.Append("CREATED_DATE,");
            insertSql.Append("UPDATED_DATE,");
            insertSql.Append("GP_METALLIC_PLATE_CODE");
            insertSql.Append(")");
            insertSql.Append("VALUES( ");
            insertSql.Append("GEN_DAILY_PROCESS_SEQ.NEXTVAL,");
            insertSql.Append("'" + processDetails.PROCESS_KUBUN_CD + "',");
            insertSql.Append("'" + processDetails.PP_PROCESS_PLAN_CD + "',");
            insertSql.Append("'" + processDetails.MACHINE_CD + "',");
            insertSql.Append("'" + processDetails.LOCATION_CD + "',");
            insertSql.Append(" TO_DATE('" + (Convert.ToDateTime(processDetails.GP_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");
            insertSql.Append("'" + processDetails.GP_START_OPERATOR_CD + "',");
            insertSql.Append(" TO_DATE('" + (Convert.ToDateTime(processDetails.GP_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");
            insertSql.Append("'" + processDetails.GP_END_OPERATOR_CD + "',");
            insertSql.Append("'" + processDetails.GP_OK_UNITS + "',");
            insertSql.Append("'" + processDetails.UNIT_TYPE + "',");
            insertSql.Append("'" + processDetails.GP_QTY + "',");
            insertSql.Append("'" + processDetails.GP_NG_DETAILS1 + "',");
            insertSql.Append("'" + processDetails.GP_NG_UNITS1 + "',");
            insertSql.Append("'" + processDetails.GP_NG_UNIT_TYPE1 + "',");
            insertSql.Append("'" + processDetails.GP_NG_QTY1 + "',");
            insertSql.Append("'" + processDetails.GP_NG_DETAILS2 + "',");
            insertSql.Append("'" + processDetails.GP_NG_UNITS2 + "',");
            insertSql.Append("'" + processDetails.GP_NG_UNIT_TYPE2 + "',");
            insertSql.Append("'" + processDetails.GP_NG_QTY2 + "',");
            insertSql.Append("'" + processDetails.GP_NG_DETAILS3 + "',");
            insertSql.Append("'" + processDetails.GP_NG_UNITS3 + "',");
            insertSql.Append("'" + processDetails.GP_NG_UNIT_TYPE3 + "',");
            insertSql.Append("'" + processDetails.GP_NG_QTY3 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_1 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_2 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_3 + "',");
            insertSql.Append("'" + processDetails.GP_TABA_NO_4 + "',");
            insertSql.Append("'" + processDetails.GP_REMARK + "',");
            insertSql.Append("'" + processDetails.GP_DECISION + "',");
            insertSql.Append("'" + processDetails.GP_NEXT_PROCESS + "',");
            insertSql.Append("'" + processDetails.USER_ID + "',");
            insertSql.Append("(SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + processDetails.USER_ID + ") ,");
            //insertSql.Append("TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "','yyyy/MM/dd'),");
            //insertSql.Append("TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "','yyyy/MM/dd'),");
            insertSql.Append("SYSDATE,");
            insertSql.Append("SYSDATE, ");
            insertSql.Append("'" + processDetails.GP_METALLIC_PLATE_CODE + "'");
            insertSql.Append(")");
            result = "Data Inserted Successfully";
            try
            {
                ExecuteNonQuery(insertSql.ToString());
            }
            catch (Exception e)
            {
                if (e.Message.Contains("TRN_GP_UK_USER_C_DATE_PROCESS_CD"))
                {
                    result = "Data Already Exist";
                }
                else
                {
                    throw;
                }
            }
            return result;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/07      
        /// used screens: GeneralProcessEnd
        /// Fetch Unit and Weight of previous process by using orNo and taba no
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public GeneralDailyProcessDetails GetUnitsWeightOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails)//currently not in used
        {
            int processPlanCd = 0;
            int dailyStartProcessCnt = 0;
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "'  AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
            processPlanCd = Convert.ToInt32(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (processPlanCd > 0)
            {
                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append("SELECT GP.GP_OK_UNITS  AS PP_NO_OF_UNITS,GP.GP_QTY AS PP_ACT_ALLOC_QTY,GP.UNIT_TYPE AS UNIT_TYPE ");
                selectQuery.Append(" FROM TRN_GENERAL_DAILY_PROCESS GP ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
                selectQuery.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                if (processDetails.GP_TABA_NO_2 != null)
                {
                    selectQuery.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
                }
                else
                {
                    selectQuery.Append(" AND GP_TABA_NO_2 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_3 != null)
                {
                    selectQuery.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
                }
                else
                {
                    selectQuery.Append(" AND GP_TABA_NO_3 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_4 != null)
                {
                    selectQuery.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
                }
                else
                {
                    selectQuery.Append(" AND GP_TABA_NO_4 IS NULL ");
                }
                selectQuery.Append(" AND GP_END_DATE IS NOT NULL ");

                selectQuery.Append(" ORDER BY GP_END_DATE DESC FETCH FIRST 1 ROW ONLY ");

                processPlanDetails = ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQuery.ToString());

            }
            return processPlanDetails;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processDetails"></param>
        /// <returns></returns>
        public GeneralDailyProcessDetails GetProcessNameOnMergeScreen(short PROCESS_KUBUN_CD)
        {
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            StringBuilder selectProcessName = new StringBuilder();
            selectProcessName.Append("SELECT KUBUN_NAME AS PROCESS_KUBUN_JP,KUBUN_NAME_EN AS PROCESS_KUBUN_EN FROM MST_PROCESS_KUBUN ");
            selectProcessName.Append(" WHERE ");
            selectProcessName.Append(" KUBUN_ID = '" + PROCESS_KUBUN_CD + "' ");
            processPlanDetails = ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectProcessName.ToString());
            return processPlanDetails;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/07      
        /// used screens: GeneralDailyProcessCancel
        /// Check validations on GeneralDailyProcessCancel Screen using PROCESS_KUBUN_CD,GP_START_OPERATOR_CD,MACHINE_CD
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public string CheckValidationsProcessMachineOprOnCancel(GeneralDailyProcessDetails processDetails)
        {
            var result = "";

            int operatorCdCnt = 0;
            int operatorCdFlag = 0;
            int processCdCnt = 0;
            int machineCdCnt = 0;
            int machineCdFlag = 0;
            StringBuilder selectQueryOperatorCd = new StringBuilder();
            selectQueryOperatorCd.Append("SELECT COUNT(*) FROM MST_EMPLOYEE ");
            selectQueryOperatorCd.Append("WHERE ");
            selectQueryOperatorCd.Append("EMPLOYEE_CD = '" + processDetails.GP_OPERATOR_CD + "' ");
            operatorCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryOperatorCd.ToString()));
            if (operatorCdCnt > 0)
            {
                StringBuilder selectQueryOperatorCdFlag = new StringBuilder();
                selectQueryOperatorCdFlag.Append("SELECT COUNT(*) FROM MST_EMPLOYEE ");
                selectQueryOperatorCdFlag.Append("WHERE ");
                selectQueryOperatorCdFlag.Append("EMPLOYEE_CD = '" + processDetails.GP_OPERATOR_CD + "' AND IN_ACTIVE_FLAG = 0 ");
                operatorCdFlag = Convert.ToInt32(ExecuteScalar(selectQueryOperatorCdFlag.ToString()));
                if (operatorCdFlag > 0)
                {
                    StringBuilder selectQueryProcessCd = new StringBuilder();
                    selectQueryProcessCd.Append("SELECT COUNT(*) FROM MST_PROCESS_KUBUN ");
                    selectQueryProcessCd.Append(" WHERE ");
                    selectQueryProcessCd.Append(" KUBUN_ID = '" + processDetails.PROCESS_KUBUN_CD + "' ");
                    processCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryProcessCd.ToString()));
                    if (processCdCnt > 0)
                    {
                        if (processDetails.MACHINE_CD != null)
                        {
                            StringBuilder selectQueryMachineCd = new StringBuilder();
                            selectQueryMachineCd.Append("SELECT COUNT(*)  FROM MST_MACHINE ");
                            selectQueryMachineCd.Append(" WHERE ");
                            selectQueryMachineCd.Append(" MACHINE_CD = '" + processDetails.MACHINE_CD + "' ");
                            machineCdCnt = Convert.ToInt32(ExecuteScalar(selectQueryMachineCd.ToString()));
                            if (machineCdCnt > 0)
                            {
                                StringBuilder selectQueryMachineCdFlag = new StringBuilder();
                                selectQueryMachineCdFlag.Append("SELECT COUNT(*)  FROM MST_MACHINE ");
                                selectQueryMachineCdFlag.Append(" WHERE ");
                                selectQueryMachineCdFlag.Append(" MACHINE_CD = '" + processDetails.MACHINE_CD + "'  AND IN_ACTIVE_FLAG = 0 ");
                                machineCdFlag = Convert.ToInt32(ExecuteScalar(selectQueryMachineCdFlag.ToString()));
                                if (machineCdFlag > 0)
                                {
                                    result = "Success";

                                }
                                else
                                {
                                    result = "Machine is Inactive";
                                }
                            }
                            else
                            {
                                result = "Machine is not found";
                            }

                        }
                        else
                        {
                            result = "Success";
                        }

                    }
                    else
                    {
                        result = "Process is not found";
                    }
                }
                else
                {
                    result = "Operator is Inactive";
                }
            }
            else
            {
                result = "Operator is not found";
            }
            return result;


        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/22      
        /// used screens: GeneralProcessCancel
        /// Fetch General Process Plan data of previous process by using chubban and taba no
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public List<GeneralDailyProcessDetails> CheckValidationsOnGeneralDailyProcessCancel(GeneralDailyProcessDetails processDetails)
        {
            int thabaCnt = 0;
            long processPlanCd = 0;
            List<GeneralDailyProcessDetails> processPlanDetails = new List<GeneralDailyProcessDetails>();
            StringBuilder selectQueryComplProcessCnt = new StringBuilder();
            selectQueryComplProcessCnt.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryComplProcessCnt.Append(" WHERE ");
            selectQueryComplProcessCnt.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' ");
            selectQueryComplProcessCnt.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL AND PROCESS_PLAN_START_DATE IS NOT NULL  ");
            processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryComplProcessCnt.ToString()));
            if (processPlanCd > 0)
            {
                StringBuilder selectQueryThabaProcessCnt = new StringBuilder();
                selectQueryThabaProcessCnt.Append("SELECT COUNT(*)  FROM TRN_GENERAL_DAILY_PROCESS ");
                selectQueryThabaProcessCnt.Append(" WHERE ");
                selectQueryThabaProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
                // selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                if (processDetails.GP_TABA_NO_1 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_1 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_2 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_2 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_3 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_3 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_4 != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
                }
                else
                {
                    selectQueryThabaProcessCnt.Append(" AND GP_TABA_NO_4 IS NULL ");
                }
                selectQueryThabaProcessCnt.Append(" AND PROCESS_KUBUN_CD = '" + processDetails.PROCESS_KUBUN_CD + "' ");
                selectQueryThabaProcessCnt.Append(" AND NVL(GP_END_OPERATOR_CD,GP_START_OPERATOR_CD) = '" + processDetails.GP_OPERATOR_CD + "' ");
                if (processDetails.MACHINE_CD != null)
                {
                    selectQueryThabaProcessCnt.Append(" AND MACHINE_CD = '" + processDetails.MACHINE_CD + "'");
                }

                thabaCnt = Convert.ToInt32(ExecuteScalar(selectQueryThabaProcessCnt.ToString()));
                if (thabaCnt > 0)
                {
                    processPlanDetails = GetGeneralDailyProcessDataForCancel(processPlanCd, Convert.ToInt16(processDetails.GP_OPERATOR_CD), Convert.ToInt16(processDetails.PROCESS_KUBUN_CD), Convert.ToInt16(processDetails.MACHINE_CD), Convert.ToInt16(processDetails.GP_TABA_NO_1), Convert.ToInt16(processDetails.GP_TABA_NO_2), Convert.ToInt16(processDetails.GP_TABA_NO_3), Convert.ToInt16(processDetails.GP_TABA_NO_4));
                }
                else
                {
                    GeneralDailyProcessDetails message = new GeneralDailyProcessDetails()
                    { RESPONCE = "Daily Process Does not Exist" };
                    processPlanDetails.Add(message);
                }
            }
            else
            {
                GeneralDailyProcessDetails message = new GeneralDailyProcessDetails()
                { RESPONCE = "Chubban is already completed" };
                processPlanDetails.Add(message);
            }
            return processPlanDetails;
        }
        public List<GeneralDailyProcessDetails> GetGeneralDailyProcessDataForCancel(long PROCESS_PLAN_CD, int GP_OPERATOR_CD, short PROCESS_KUBUN_CD, short MACHINE_CD, short GP_TABA_NO_1, short? GP_TABA_NO_2, short? GP_TABA_NO_3, short? GP_TABA_NO_4)
        {
            List<GeneralDailyProcessDetails> result = new List<GeneralDailyProcessDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append("GEN_PROCESS_CD,");
            selectQuery.Append("CASE WHEN GP_END_DATE IS NOT NULL THEN TO_CHAR(GP_END_DATE, 'yyyy/mm/dd HH24:MI') WHEN GP_END_DATE IS  NULL THEN TO_CHAR(GP_START_DATE, 'yyyy/mm/dd HH24:MI') END AS GP_DATE,");
            selectQuery.Append("CASE WHEN GP_END_OPERATOR_CD IS NOT NULL THEN 1 WHEN GP_END_OPERATOR_CD IS  NULL THEN 0 END AS GP_PROCESS_STATUS ");
            selectQuery.Append("FROM TRN_GENERAL_DAILY_PROCESS ");
            selectQuery.Append("WHERE PROCESS_PLAN_CD='" + PROCESS_PLAN_CD + "' AND NVL(GP_END_OPERATOR_CD,GP_START_OPERATOR_CD) = '" + GP_OPERATOR_CD + "' AND PROCESS_KUBUN_CD ='" + PROCESS_KUBUN_CD + "'  ");
            //selectQuery.Append(" AND GP_TABA_NO_1 ='" + GP_TABA_NO_1 + "'");
            if (GP_TABA_NO_1 != 0)
            {
                selectQuery.Append(" AND GP_TABA_NO_1 ='" + GP_TABA_NO_1 + "'");
            }
            else
            {
                selectQuery.Append(" AND GP_TABA_NO_1 IS NULL ");
            }
            if (GP_TABA_NO_2 != 0)
            {
                selectQuery.Append(" AND GP_TABA_NO_2 ='" + GP_TABA_NO_2 + "'");
            }
            else
            {
                selectQuery.Append(" AND GP_TABA_NO_2 IS NULL ");
            }
            if (GP_TABA_NO_3 != 0)
            {
                selectQuery.Append(" AND GP_TABA_NO_3 ='" + GP_TABA_NO_3 + "'");
            }
            else
            {
                selectQuery.Append(" AND GP_TABA_NO_3 IS NULL ");
            }
            if (GP_TABA_NO_4 != 0)
            {
                selectQuery.Append(" AND GP_TABA_NO_4 ='" + GP_TABA_NO_4 + "'");
            }
            else
            {
                selectQuery.Append(" AND GP_TABA_NO_4 IS NULL ");
            }
            if (MACHINE_CD != 0)
            {
                selectQuery.Append(" AND MACHINE_CD ='" + MACHINE_CD + "' ");
            }
            selectQuery.Append(" ORDER BY GP_DATE DESC ");
            selectQuery.Append(" FETCH FIRST 100 ROWS ONLY ");
            result = ExecuteQueryMultiResult<GeneralDailyProcessDetails>(selectQuery.ToString());
            return result;
        }
        public string DeleteGeneralProcessPlanData(GeneralDailyProcessDetails processDetails)
        {
            string result;
            string Query = string.Empty;
            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            StringBuilder deleteSql = new StringBuilder();
            deleteSql.Append("DELETE FROM TRN_GENERAL_DAILY_PROCESS ");
            deleteSql.Append("WHERE ");
            deleteSql.Append(" NVL(GP_END_OPERATOR_CD, GP_START_OPERATOR_CD)  ='" + processDetails.GP_START_OPERATOR_CD + "'");
            deleteSql.Append(" AND PROCESS_KUBUN_CD ='" + processDetails.PROCESS_KUBUN_CD + "'");
            deleteSql.Append(" AND PROCESS_PLAN_CD ='" + processDetails.PP_PROCESS_PLAN_CD + "'");
            deleteSql.Append(" AND GEN_PROCESS_CD ='" + processDetails.GEN_PROCESS_CD + "'");
            //deleteSql.Append(" AND NVL(GP_END_DATE, GP_START_DATE) = TO_DATE('" + (Convert.ToDateTime(processDetails.GP_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "', 'yyyy/mm/dd HH24:MI:SS')");
            //deleteSql.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
            if (processDetails.GP_TABA_NO_1 != null)
            {
                deleteSql.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
            }
            else
            {
                deleteSql.Append(" AND GP_TABA_NO_1 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_2 != null)
            {
                deleteSql.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
            }
            else
            {
                deleteSql.Append(" AND GP_TABA_NO_2 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_3 != null)
            {
                deleteSql.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
            }
            else
            {
                deleteSql.Append(" AND GP_TABA_NO_3 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_4 != null)
            {
                deleteSql.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
            }
            else
            {
                deleteSql.Append(" AND GP_TABA_NO_4 IS NULL ");
            }
            if (processDetails.MACHINE_CD != null)
            {
                deleteSql.Append(" AND MACHINE_CD ='" + processDetails.MACHINE_CD + "'");
            }
            ExecuteNonQuery(deleteSql.ToString());
            result = "Success";
            return result;

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
        public int GetRowCountForGeneralDailyProcessFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_GENERAL_DAILY_PROCESS GP ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON GP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = GP.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS3 ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = GP.PROCESS_KUBUN_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = GP.GP_NEXT_PROCESS ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD= GP.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD= GP.GP_END_OPERATOR_CD");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE CONCAT(PP.CHUUBAN, PP.GYOUBAN) NOT IN (" + _specialChuuban + ") AND ";

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
        /// used screens: Daily Process Analysis(CSV Export)
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForGeneralProcessDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_GENERAL_DAILY_PROCESS GP ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = GP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = GP.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = GP.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = GP.GP_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = GP.GP_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS3 ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = GP.PROCESS_KUBUN_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = GP.GP_NEXT_PROCESS");

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
        /// CREATED BY : Namrata Ubale on 2025/06/26     
        /// used screens: Daily Process Entry,End,Cancel,Merge
        /// </summary>
        /// <returns>
        /// Get Count of Daily General Process 
        /// This method is use to check valid Process used on Daily general Process Screen.
        /// </returns>
        /// 
        public int ValidateGeneralProcess(short PROCESS_KUBUN_CD)
        {
            List<int> processKubun = _General_Daily_Process_Cd.Split(",").Select(int.Parse).ToList();
            int isKubunExist = Convert.ToInt32(processKubun.AsEnumerable().Where(w => w == PROCESS_KUBUN_CD).Select(w => w.ToString()).FirstOrDefault());
            return isKubunExist;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/06/27 
        ///used screens: Daily Process End and Merge screen
        /// </summary>
        /// <returns>
        /// Fetch chubban Details From TRN_PROCESS_PLAN
        /// </returns>
        /// 
        public GeneralDailyProcessDetails GetChubbanDetailsForWeight(string PP_CHUUBAN)
        {
            StringBuilder selectQueryGetData = new StringBuilder();
            selectQueryGetData.Append("SELECT PROD_DIM_D AS PP_PROD_DIM_D,PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQueryGetData.Append("NVL(LENGTH_OTHER,PROD_LENGTH) AS LENGTH,ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQueryGetData.Append("FROM TRN_PROCESS_PLAN ");
            selectQueryGetData.Append("WHERE ");
            selectQueryGetData.Append("CHUUBAN || Lpad(GYOUBAN,2,0) = '" + PP_CHUUBAN + "' AND PROCESS_PLAN_CMPL_DATE IS NULL");
            return ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryGetData.ToString());
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/07/08 
        /// used screens: Daily Process Start and Merge screen
        /// </summary>
        /// <returns>
        /// get Max thaba No For Special chuuban details from TRN_PROCESS_PLAN
        /// </returns>
        /// 
        public int GetNextThabaForSpecialChuubanNo(string orderNo)
        {
            StringBuilder selectQueryGetData = new StringBuilder();
            selectQueryGetData.Append("SELECT GET_NEXT_TABA_FOR_SPECIAL_CHUBAN('" + orderNo + "') AS NEXT_TABA FROM DUAL");
            int taba_No = Convert.ToInt32(ExecuteScalar(selectQueryGetData.ToString()));
            return taba_No;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/07/08 
        /// used screens: Daily Process Cancel screen
        /// </summary>
        /// <returns>
        /// get Max thaba No For Special chuuban chubban details  
        /// </returns>
        /// 
        public GeneralDailyProcessDetails GetGeneralProcessDataForCancel(GeneralDailyProcessDetails processDetails)
        {
            StringBuilder selectQueryGetData = new StringBuilder();
            selectQueryGetData.Append(" SELECT ");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.PROD_DIM_W AS PP_PROD_DIM_W,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR,");
            selectQueryGetData.Append(" NVL(TRN_PROCESS_PLAN.LENGTH_OTHER,TRN_PROCESS_PLAN.PROD_LENGTH) AS LENGTH,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR,");
            selectQueryGetData.Append(" TRN_PROCESS_PLAN.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQueryGetData.Append(" MST_CUSTOMER.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQueryGetData.Append(" MST_PROCESS_KUBUN.KUBUN_NAME AS PP_PROCESS_NAME,");
            selectQueryGetData.Append(" MST_EMPLOYEE.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQueryGetData.Append(" MST_MACHINE.MACHINE_NAME AS PP_MACHINE_NAME ");
            selectQueryGetData.Append(" FROM  TRN_GENERAL_DAILY_PROCESS");
            selectQueryGetData.Append(" INNER JOIN TRN_PROCESS_PLAN ON TRN_PROCESS_PLAN.PROCESS_PLAN_CD = TRN_GENERAL_DAILY_PROCESS.PROCESS_PLAN_CD");
            selectQueryGetData.Append(" INNER JOIN MST_PROCESS_KUBUN ON MST_PROCESS_KUBUN.KUBUN_ID = TRN_GENERAL_DAILY_PROCESS.PROCESS_KUBUN_CD");
            selectQueryGetData.Append(" INNER JOIN MST_EMPLOYEE ON MST_EMPLOYEE.EMPLOYEE_CD = NVL(TRN_GENERAL_DAILY_PROCESS.GP_END_OPERATOR_CD, TRN_GENERAL_DAILY_PROCESS .GP_START_OPERATOR_CD)");
            selectQueryGetData.Append(" LEFT JOIN MST_MACHINE ON MST_MACHINE.MACHINE_CD = NVL(TRN_GENERAL_DAILY_PROCESS.MACHINE_CD, 0)");
            selectQueryGetData.Append(" INNER JOIN MST_CUSTOMER ON TRN_PROCESS_PLAN.CUSTOMER_CD = MST_CUSTOMER.CUSTOMER_CD ");
            selectQueryGetData.Append(" WHERE ");
            selectQueryGetData.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' AND TRN_PROCESS_PLAN.PROCESS_PLAN_CMPL_DATE IS NULL ");
            // selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
            if (processDetails.GP_TABA_NO_1 != null)
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
            }
            else
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_1 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_2 != null)
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
            }
            else
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_2 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_3 != null)
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
            }
            else
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_3 IS NULL ");
            }
            if (processDetails.GP_TABA_NO_4 != null)
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
            }
            else
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GP_TABA_NO_4 IS NULL ");
            }
            selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.PROCESS_KUBUN_CD = '" + processDetails.PROCESS_KUBUN_CD + "' ");
            if (processDetails.MACHINE_CD != null)
            {
                selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.MACHINE_CD ='" + processDetails.MACHINE_CD + "' ");
            }
            selectQueryGetData.Append(" AND NVL(TRN_GENERAL_DAILY_PROCESS.GP_END_OPERATOR_CD, TRN_GENERAL_DAILY_PROCESS.GP_START_OPERATOR_CD) = '" + processDetails.GP_START_OPERATOR_CD + "'");
            selectQueryGetData.Append(" AND TRN_GENERAL_DAILY_PROCESS.GEN_PROCESS_CD ='" + processDetails.GEN_PROCESS_CD + "' ");
            //selectQueryGetData.Append(" AND TO_CHAR(NVL(TRN_GENERAL_DAILY_PROCESS.GP_END_DATE, TRN_GENERAL_DAILY_PROCESS.GP_START_DATE) ,'yyyy/mm/dd HH24:MI:SS') = '" + (Convert.ToDateTime(processDetails.GP_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "'");
            return ExecuteQuerySingleResult<GeneralDailyProcessDetails>(selectQueryGetData.ToString());
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/07/15     
        /// used screens: GeneralProcessEnd And GeneralProcessStartEnd
        /// </summary>
        /// <returns>
        /// Fetch data by using OrNo using table TRN_PROCESS_PLAN
        /// </returns>
        /// 
        public GeneralDailyProcessDetails GetProcessPlanData(GeneralDailyProcessDetails processDetails)
        {
            GeneralDailyProcessDetails processPlanData = new GeneralDailyProcessDetails();
            processPlanData = getProcessPlanDetails(processDetails);
            var result = getOperatorProcessMachineName(processDetails);
            if (result != null)
            {
                processPlanData.PP_EMPLOYEE_NAME = result.PP_EMPLOYEE_NAME;
            }

            return processPlanData;

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/07/17      
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>        
        /// To bind machine name of General Daily Process
        /// </returns>
        /// 
        public List<NameValue> GetMachineTypeList(int machineTypeCd)
        {

            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            //query.Append(" AND MACHINE_TYPE IN ( '" + UIConstants.K_MACHINEGROUP_ACIDWASH + "','" + UIConstants.K_MACHINEGROUP_ATTACHING + "','" + UIConstants.K_MACHINEGROUP_SHOT_BLAST + "','" + UIConstants.K_MACHINEGROUP_STRAIGHT_CUT + "','" + UIConstants.K_MACHINEGROUP_UT + "','" + UIConstants.K_MACHINEGROUP_CUTTING + "','" + UIConstants.K_MACHINEGROUP_OTHER + "')");
            query.Append(" AND MACHINE_TYPE = '" + machineTypeCd + "'");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/10/06      
        /// used screens: General Process Start  And GeneralProcessStartEnd Screen 
        /// </summary>
        /// <returns>        
        /// check validation of chubban for without taba functionality into TRN_PROCESSS_PLAN Table
        /// </returns>
        /// 
        public GeneralDailyProcessDetails ValidateChubbanForProcessStart(string PP_CHUUBAN)
        {
            int count = 0;
            int completeDateCnt = 0;
            int thabaCnt = 0;
            int decisionCnt = 0;
            int preProcessCd = 0;
            long processPlanCd = 0;
            var result = string.Empty;
            int generalProCnt = 0;

            GeneralDailyProcessDetails processPlanDetails = new GeneralDailyProcessDetails();
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + PP_CHUUBAN + "' ");
            selectQueryProcessPlanCd.Append(" AND PROCESS_PLAN_START_DATE IS NOT NULL ");
            count = Convert.ToInt32(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (count > 0)
            {
                StringBuilder selectQueryComplProcessCnt = new StringBuilder();
                selectQueryComplProcessCnt.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
                selectQueryComplProcessCnt.Append(" WHERE ");
                selectQueryComplProcessCnt.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + PP_CHUUBAN + "' ");
                selectQueryComplProcessCnt.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
                processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryComplProcessCnt.ToString()));
                if (processPlanCd > 0)
                {
                    StringBuilder selectQueryProcessCnt = new StringBuilder();
                    selectQueryProcessCnt.Append("SELECT COUNT(*)  FROM TRN_GENERAL_DAILY_PROCESS ");
                    selectQueryProcessCnt.Append(" WHERE ");
                    selectQueryProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_1 IS NULL ");
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_2 IS NULL ");
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_3 IS NULL ");
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_4 IS NULL ");
                    selectQueryProcessCnt.Append(" AND GP_START_DATE IS NOT NULL AND GP_END_DATE IS NULL ");
                    int process_Count = Convert.ToInt32(ExecuteScalar(selectQueryProcessCnt.ToString()));
                    if (process_Count > 0)
                    {
                        processPlanDetails.RESPONCE = "Process is already Started for this chubban";
                    }
                    else
                    {
                        processPlanDetails.PP_CHARGE = getChargeOfProcess(processPlanCd);
                        result = Convert.ToString(processPlanDetails);
                    }
                }
                else
                {
                    processPlanDetails.RESPONCE = "Chubban is already completed";
                }
            }
            else
            {
                processPlanDetails.RESPONCE = "Chubban is Not Found";
            }
            return processPlanDetails;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/03 
        ///Get Start Process Details From TRN_GENERAL_DAILY_PROCESS And TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        /// check validation of chubban for without taba functionality into TRN_PROCESSS_PLAN Table
        /// </returns>
        /// 
        public string ValidateChubbanForProcessEnd(string PP_CHUUBAN)
        {
            long processPlanCd = 0;
            string result = "";
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + PP_CHUUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL");
            processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (processPlanCd == 0)
            {
                result = "Chubban is already completed";

            }
            else
            {
                StringBuilder selectQueryProcessCnt = new StringBuilder();
                selectQueryProcessCnt.Append("SELECT COUNT(*)  FROM TRN_GENERAL_DAILY_PROCESS ");
                selectQueryProcessCnt.Append(" WHERE ");
                selectQueryProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
                selectQueryProcessCnt.Append(" AND GP_TABA_NO_1 IS NULL ");
                selectQueryProcessCnt.Append(" AND GP_TABA_NO_2 IS NULL ");
                selectQueryProcessCnt.Append(" AND GP_TABA_NO_3 IS NULL ");
                selectQueryProcessCnt.Append(" AND GP_TABA_NO_4 IS NULL ");
                selectQueryProcessCnt.Append(" AND GP_START_DATE IS NOT NULL AND GP_END_DATE IS NULL ");
                int process_Count = Convert.ToInt32(ExecuteScalar(selectQueryProcessCnt.ToString()));
                if (process_Count == 0)
                {
                    result = "Process Start data not found for this chubban";
                }
            }
            return result;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/04/03 
        ///Get Start Process Details From TRN_GENERAL_DAILY_PROCESS And TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        /// check validation of chubban for without taba functionality into TRN_PROCESSS_PLAN Table
        /// </returns>
        /// 
        public string CheckExistingDataValidation(GeneralDailyProcessDetails processDetails)
        {
            long processPlanCd = 0;
            string result = "";
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            StringBuilder selectQueryProcessPlanCd = new StringBuilder();
            selectQueryProcessPlanCd.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQueryProcessPlanCd.Append(" WHERE ");
            selectQueryProcessPlanCd.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processDetails.PP_CHUUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL");
            processPlanCd = Convert.ToInt64(ExecuteScalar(selectQueryProcessPlanCd.ToString()));
            if (processPlanCd != 0)
            {

                StringBuilder selectQueryProcessCnt = new StringBuilder();
                selectQueryProcessCnt.Append("SELECT COUNT(*)  FROM TRN_GENERAL_DAILY_PROCESS ");
                selectQueryProcessCnt.Append(" WHERE ");
                selectQueryProcessCnt.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "'  ");
              
                selectQueryProcessCnt.Append(" AND PROCESS_KUBUN_CD = '" + processDetails.PROCESS_KUBUN_CD + "' ");
                if (processDetails.MACHINE_CD != null)
                {
                    selectQueryProcessCnt.Append(" AND MACHINE_CD ='" + processDetails.MACHINE_CD + "'");
                }
                else
                {
                    selectQueryProcessCnt.Append(" AND MACHINE_CD IS NULL ");
                }
                    selectQueryProcessCnt.Append(" AND GP_DECISION = '" + decisionVal + "' ");
                if (processDetails.GP_TABA_NO_1 != null)
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_1 ='" + processDetails.GP_TABA_NO_1 + "'");
                }
                else
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_1 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_2 != null)
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_2 ='" + processDetails.GP_TABA_NO_2 + "'");
                }
                else
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_2 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_3 != null)
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_3 ='" + processDetails.GP_TABA_NO_3 + "'");
                }
                else
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_3 IS NULL ");
                }
                if (processDetails.GP_TABA_NO_4 != null)
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_4 ='" + processDetails.GP_TABA_NO_4 + "'");
                }
                else
                {
                    selectQueryProcessCnt.Append(" AND GP_TABA_NO_4 IS NULL ");
                }
                int process_Count = Convert.ToInt32(ExecuteScalar(selectQueryProcessCnt.ToString()));
                if (process_Count > 0)
                {
                    result = "Process already exist for this chuuban with Ok decision";
                }
            }
            return result;

        }

    }


}
