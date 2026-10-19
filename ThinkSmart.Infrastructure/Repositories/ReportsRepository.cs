using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ReportsRepository : BaseRepository, IReportsRepository
    {
        public ReportsRepository(IConfiguration _configuration) : base(_configuration)
        {
           
        }

        /// <summary>
        /// Get Final Inspect Details 
        /// Created by shobhana April 2025.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="endOperator"></param>
        /// <param name="orderBy"></param>
        /// <param name="printFlag"></param>
        /// <param name="isOnlyUnPrintFlag"></param>
        /// <param name="userID"></param>
        /// <returns> Final Inspection Details List</returns>
        public List<FinalInspectionDetails> GetFinalInspectDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isOnlyUnPrintFlag, int userID)
        {
            StringBuilder WhereCondition = new StringBuilder();
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
            //    WhereSubQueryCondition.Append("FI.PRINT_DATE IS NULL"); 
            //    WhereCondition.Append(" PRINT_DATE IS NULL ");
            //    WhereCondition.Append(" AND INPUT_LOCATION_CD = ");
            //    WhereCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            //    WhereSubQueryCondition.Append(" AND FI.INPUT_LOCATION_CD = ");
            //    WhereSubQueryCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            //}

            WhereCondition.Append(" TO_DATE(INSP_DATE_VALUE,'yyyy/mm/dd hh24:mi') BETWEEN TO_DATE('" + startDate + "','yyyy/mm/dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy/mm/dd hh24:mi') ");
            WhereSubQueryCondition.Append(" INSP_DATE BETWEEN TO_DATE('" + startDate + "','yyyy/mm/dd hh24:mi') AND TO_DATE('" + endDate + "','yyyy/mm/dd hh24:mi') ");
            WhereCondition.Append(" AND INPUT_LOCATION_CD = ");
            WhereCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");
            WhereSubQueryCondition.Append(" AND FI.INPUT_LOCATION_CD = ");
            WhereSubQueryCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + userID + ")");

            if (endOperator != "0") 
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
                selectQuery.Append(",NVL(TO_CHAR(PP.PROD_LENGTH),PP.LENGTH_OTHER) AS PROD_LENGTH_STRING");
                selectQuery.Append(",PP.NO_OF_UNITS AS PP_NO_OF_UNITS");
                selectQuery.Append(",PP.UNIT AS PP_UNIT");
                selectQuery.Append(", (PP.LENGTH_TOL_MIN_CHR ||','|| PP.LENGTH_TOL_MAX_CHR) AS LEN_TOL");
                selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PROD_LENGTH_TOL_MAX  ");
                selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PROD_LENGTH_TOL_MIN  ");
                // selectQuery.Append(", NULL AS MACHINABILITY_VALUE");
                selectQuery.Append(", GET_TENSILE_STATUS(PP.PROCESS_PLAN_CD) AS TS_DECISION");
                selectQuery.Append(", GET_HARDNESS_STATUS(PP.PROCESS_PLAN_CD) AS HD_DECISION");

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
                selectQuery.Append(", FI.FI_NEXT_PROCESS");
                selectQuery.Append(", TO_CHAR(FI.INSP_DATE,'YYYY/MM/DD hh24:mi') AS INSP_DATE_VALUE ");
                selectQuery.Append(", FI.PRINT_DATE AS PRINT_DATE ");
                selectQuery.Append(", EMP.SHORT_NAME AS OPERATOR_NAME ");
                selectQuery.Append(" ,PP.PROD_NAME_CUST ");

                selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR AS DIM_TOL_MAX_CHR ");
                selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR AS DIM_TOL_MIN_CHR ");
                selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR ");
                selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR ");
                selectQuery.Append(",FI.INPUT_LOCATION_CD");
                selectQuery.Append(",MSG.MSG_CONTENTS AS FI_REMARK"); 
                selectQuery.Append(",GRP_COUNT.GROUP_COUNT ");
                selectQuery.Append(",SUM_FI_QTY");

                selectQuery.Append("  FROM  TRN_FINAL_INSPECTION FI ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP  ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");

                selectQuery.Append(" INNER JOIN MST_CUSTOMER CN ON CN.CUSTOMER_CD = PP.CUSTOMER_CD");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = FI.MSG_CD");

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
                    selectQuery.Append("INSP_DATE,CHUBAN_GYOBAN,TABA_NO ");
                }
                else
                {
                    selectQuery.Append("CHUBAN_GYOBAN ");
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
        /// Get Final Inspection Details.
        /// Created by shobhana[April 2025]
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="endOperator"></param>
        /// <param name="orderBy"></param>
        /// <param name="printFlag"></param>
        /// <param name="isOnlyUnPrintFlag"></param>
        /// <param name="userID"></param>
        /// <returns> Final Inspection Details List</returns>
        public List<FinalInspectionDetails> GetStartChuuban(string startDate, string endDate, string printFlag, int isOnlyUnPrintFlag)
        {
            string WHERE_CONDITION = string.Empty;

            if (isOnlyUnPrintFlag == 0)
                WHERE_CONDITION = " PRINT_DATE IS NULL ";
            else
                WHERE_CONDITION = " TO_DATE(INSP_DATE_VALUE,'YYYY-MM-DD') BETWEEN TO_DATE('" + Convert.ToDateTime(startDate).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy-mm-dd') AND TO_DATE('" + Convert.ToDateTime(endDate).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy-mm-dd') ";

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
        public List<HikinukiDetails> GetHikinukiData(GridSearchFields request)
        {
                StringBuilder selectQuery = new StringBuilder();
           
                selectQuery.Append("SELECT DW_DRAWING_CD ");
                selectQuery.Append(" ,HOST_MACHINE_CODE ");
                selectQuery.Append(" ,PP_CHUUBAN_GYOUBAN ");
                selectQuery.Append(" ,PP_PROD_SHAPE ");
                selectQuery.Append(" ,PP_DIM_D ");
                selectQuery.Append(" ,PP_DIM_OTHER ");
                selectQuery.Append(" ,PP_LENGTH ");
                selectQuery.Append(" ,PP_PROD_NAME_CUST ");
                selectQuery.Append(" ,PP_CHARGE ");
                selectQuery.Append(" ,PP_CUSTOMER_NAME ");
                selectQuery.Append(" ,DW_FINISHED_BARS ");
                selectQuery.Append(" ,DW_FINISHED_QTY ");
                selectQuery.Append(" ,UNIT_TYPE ");
                selectQuery.Append(" ,DW_DECISION ");
                selectQuery.Append(" ,DW_TABA_NO_1_2_3_4 ");
                selectQuery.Append(" ,DW_END_DATE_STRING ");
                selectQuery.Append(" ,DW_END_DATE ");
                selectQuery.Append(" ,HOST_TRANSFER_DATE_STRING ");
                selectQuery.Append(" ,DW_END_OPERATOR "); 
                selectQuery.Append(" FROM ( ");
                
                selectQuery.Append("SELECT ");
                selectQuery.Append(" DR.DRAWING_CD AS DW_DRAWING_CD");
                selectQuery.Append(" ,MA.SHORT_NAME ||'(' || MA.HOST_MACHINE_CODE || ')' AS HOST_MACHINE_CODE ");
                selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
                selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
                selectQuery.Append(" ,PP.PROD_DIM_D AS PP_DIM_D ");
                selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_DIM_OTHER ");
                selectQuery.Append(" ,PP.PROD_LENGTH AS PP_LENGTH ");
                selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
                selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
                selectQuery.Append(" ,NVL(CUST.SHORT_NAME,CUST.YOBINA) AS PP_CUSTOMER_NAME ");
                selectQuery.Append(" ,DR.DW_FINISHED_BARS ");
                selectQuery.Append(" ,DR.DW_FINISHED_QTY ");
                selectQuery.Append(" ,DR.UNIT_TYPE ");
                selectQuery.Append(" ,DR.DW_DECISION ");
                selectQuery.Append(" ,CONCAT(DR.DW_TABA_NO_1, CONCAT(CONCAT( CASE ");
                selectQuery.Append(" WHEN DR.DW_TABA_NO_2 IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(', ', DR.DW_TABA_NO_2) ELSE NULL END, CASE ");
                selectQuery.Append(" WHEN DR.DW_TABA_NO_3 IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(', ', DR.DW_TABA_NO_3) ELSE NULL END ), CASE ");
                selectQuery.Append(" WHEN DR.DW_TABA_NO_4 IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(', ', DR.DW_TABA_NO_4) ELSE NULL END )) AS DW_TABA_NO_1_2_3_4");
                selectQuery.Append(" ,TO_CHAR(DR.HOST_TRANSFER_DATE,'yyyy/MM/dd') AS HOST_TRANSFER_DATE_STRING");
                selectQuery.Append(" ,DR.DW_END_OPERATOR_CD AS DW_END_OPERATOR");
                selectQuery.Append(" ,TO_CHAR(DR.DW_END_DATE,'YYYY/MM/DD') AS DW_END_DATE_STRING");
                selectQuery.Append(" ,DR.DW_END_DATE ");
                selectQuery.Append(" FROM TRN_DRAWING DR");
                selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD");
                selectQuery.Append(" LEFT JOIN MST_MACHINE MA ON MA.MACHINE_CD = DR.MACHINE_CD");
                selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" WHERE  DR.DW_END_DATE BETWEEN TO_DATE('" + request.START_DATETIME + "','yyyy/MM/dd HH24:MI') AND TO_DATE('" + request.END_DATETIME + "','yyyy/MM/dd HH24:MI') ");
                selectQuery.Append(" AND DR.INPUT_LOCATION_CD= " + request.INPUT_LOCATION_CD);
                if (request.PRINT_CONDITION == "print")
                {
                    selectQuery.Append(" AND TO_CHAR(DR.HOST_TRANSFER_DATE) = TO_DATE( '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd') ");
                }
                else
                {
                    selectQuery.Append(" AND DR.HOST_TRANSFER_DATE IS NOT NULL ");
                }

                selectQuery.Append(" UNION ");
                selectQuery.Append("SELECT ");
                selectQuery.Append(" DD.DRAWING_CD AS DW_DRAWING_CD");
                selectQuery.Append(" ,MA.SHORT_NAME ||'(' || MA.HOST_MACHINE_CODE || ')' AS HOST_MACHINE_CODE ");
                selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
                selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
                selectQuery.Append(" ,PP.PROD_DIM_D AS PP_DIM_D ");
                selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_DIM_OTHER ");
                selectQuery.Append(" ,PP.PROD_LENGTH AS PP_LENGTH ");
                selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
                selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
                selectQuery.Append(" ,NVL(CUST.SHORT_NAME,CUST.YOBINA) AS PP_CUSTOMER_NAME ");
                selectQuery.Append(" ,DD.DW_FINISHED_BARS ");
                selectQuery.Append(" ,DD.DW_FINISHED_QTY ");
                selectQuery.Append(" ,DD.UNIT_TYPE ");
                selectQuery.Append(" ,DD.DW_DECISION ");
                selectQuery.Append(" , TO_CHAR(DD.DW_TABA_NO_) AS DW_TABA_NO_1_2_3_4");
                selectQuery.Append(" ,TO_CHAR(DD.HOST_TRANSFER_DATE,'yyyy/MM/dd') AS HOST_TRANSFER_DATE_STRING");
                selectQuery.Append(" ,DD.DW_END_OPERATOR_CD AS DW_END_OPERATOR ");
                selectQuery.Append(" ,TO_CHAR(DD.DW_END_DATE,'YYYY/MM/DD') AS DW_END_DATE_STRING");
                selectQuery.Append(" ,DD.DW_END_DATE ");
                selectQuery.Append(" FROM TRN_DRAWING_D DD");
                selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DD.PROCESS_PLAN_CD");
                selectQuery.Append(" LEFT JOIN MST_MACHINE MA ON MA.MACHINE_CD = DD.MACHINE_CD");
                selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" WHERE  DD.DW_END_DATE BETWEEN TO_DATE('" + request.START_DATETIME + "','yyyy/MM/dd HH24:MI') AND TO_DATE('" + request.END_DATETIME + "','yyyy/MM/dd HH24:MI') ");
                selectQuery.Append(" AND DD.INPUT_LOCATION_CD= " + request.INPUT_LOCATION_CD);
                if (request.PRINT_CONDITION == "print")
                {
                    selectQuery.Append(" AND TO_CHAR(DD.HOST_TRANSFER_DATE) = TO_DATE( '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd') ");
                }
                else
                {
                    selectQuery.Append(" AND DD.HOST_TRANSFER_DATE IS NOT NULL ");
                }
            selectQuery.Append(" AND DD.IM_DRAWING <> 1  ");
            selectQuery.Append(" UNION ");

                selectQuery.Append("SELECT ");
                selectQuery.Append("  DW.DRAWING_CD AS DW_DRAWING_CD");
                selectQuery.Append(" ,MA.SHORT_NAME ||'(' || MA.HOST_MACHINE_CODE || ')' AS HOST_MACHINE_CODE ");
                selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
                selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
                selectQuery.Append(" ,PP.PROD_DIM_D AS PP_DIM_D ");
                selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_DIM_OTHER ");
                selectQuery.Append(" ,PP.PROD_LENGTH AS PP_LENGTH ");
                selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
                selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
                selectQuery.Append(" ,NVL(CUST.SHORT_NAME,CUST.YOBINA) AS PP_CUSTOMER_NAME ");
                selectQuery.Append(" ,DW.DW_FINISHED_COILS AS DW_FINISHED_BARS ");
                selectQuery.Append(" ,DW.DW_FINISHED_QTY ");
                selectQuery.Append(" ,DW.UNIT_TYPE ");
                selectQuery.Append(" ,DW.DW_DECISION ");
                selectQuery.Append(" ,CONCAT(DW.DW_TABA_NO_1, CONCAT(CONCAT( CASE ");
                selectQuery.Append(" WHEN DW.DW_TABA_NO_2 IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(', ', DW.DW_TABA_NO_2) ELSE NULL END, CASE ");
                selectQuery.Append(" WHEN DW.DW_TABA_NO_3 IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(', ', DW.DW_TABA_NO_3) ELSE NULL END ), CASE ");
                selectQuery.Append(" WHEN DW.DW_TABA_NO_4 IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(', ', DW.DW_TABA_NO_4) ELSE NULL END )) AS DW_TABA_NO_1_2_3_4");
                selectQuery.Append(" ,TO_CHAR(DW.HOST_TRANSFER_DATE,'yyyy/MM/dd') AS HOST_TRANSFER_DATE_STRING");
                selectQuery.Append(" ,DW.DW_END_OPERATOR_CD AS DW_END_OPERATOR");
                selectQuery.Append(" ,TO_CHAR(DW.DW_END_DATE,'YYYY/MM/DD') AS DW_END_DATE_STRING");
                selectQuery.Append(" ,DW.DW_END_DATE ");
                selectQuery.Append(" FROM TRN_DRAWING_W DW");
                selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD");
                selectQuery.Append(" LEFT JOIN MST_MACHINE MA ON MA.MACHINE_CD = DW.MACHINE_CD");
                selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" WHERE  DW.DW_END_DATE BETWEEN TO_DATE('" + request.START_DATETIME + "','yyyy/MM/dd HH24:MI') AND TO_DATE('" + request.END_DATETIME + "','yyyy/MM/dd HH24:MI') ");
                selectQuery.Append(" AND DW.INPUT_LOCATION_CD= " + request.INPUT_LOCATION_CD);
                if (request.PRINT_CONDITION == "print")
                {
                    selectQuery.Append(" AND TO_CHAR(DW.HOST_TRANSFER_DATE) = TO_DATE( '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd') ");
                }
                else
                {
                    selectQuery.Append(" AND DW.HOST_TRANSFER_DATE IS NOT NULL ");
                }
            selectQuery.Append(" AND DW.IM_DRAWING <> 1  ");
            selectQuery.Append(" )HIKINUKI ");
                selectQuery.Append(" ORDER BY HOST_MACHINE_CODE, DW_END_OPERATOR, DW_END_DATE ");
                var result =ExecuteQueryMultiResult<HikinukiDetails>(selectQuery.ToString());
                return result;
        }
        public List<FinalInspectionDetails> GetFinishProductDetails(FinalInspectionDetails fields)
        {
             StringBuilder selectQuery = new StringBuilder();
            fields.OPERATOR_ID = 0;
            StringBuilder WhereCondition = new StringBuilder();
           
                WhereCondition.Append(" INSP_DATE BETWEEN TO_DATE('" + fields.START_DATE + "','yyyy-mm-dd hh24:mi') AND TO_DATE('" + fields.END_DATE + "','yyyy-mm-dd hh24:mi') ");
                WhereCondition.Append(" AND FI.COMBINED_DECISION = '" + 10 + "'");
                WhereCondition.Append(" AND  FI.INPUT_LOCATION_CD = ( SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID =" + fields.USER_ID + ") ");

            if (fields.OPERATOR_ID != 0)    /// If Operator is select from FIPrintReportScreen.
                {
                    WhereCondition.Append(" AND INSP_BY = " + fields.OPERATOR_ID);
                }
                 if (fields.PRINT_FLAG == 1)    /// If Operator is select from FIPrintReportScreen.
                 {
                     WhereCondition.Append(" AND PRINT_DATE IS NULL ");
                 }

            selectQuery.Append(" SELECT ");
                selectQuery.Append(" FI.FINAL_INSP_CD AS FINAL_INSP_NO");
                selectQuery.Append(" ,TO_CHAR(FI.UPDATED_DATE, 'MM/DD') AS INSP_DATE_STRING");
                selectQuery.Append(" ,CN.SHORT_NAME AS CUSTOMER_NAME ");
                selectQuery.Append(" ,TO_CHAR(FI.INSP_DATE, 'YYYY/MM/DD') AS INSP_DATE_FOR_ORDER");
                //selectQuery.Append(" ,CONCAT(CONCAT(PRO.CHUUBAN, '-'), PRO.GYOUBAN) AS CHUUBAN_GYOUBAN");
                selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PRO.CHUUBAN,PRO.GYOUBAN) AS CHUUBAN_GYOUBAN");
                selectQuery.Append(" ,CONCAT(CONCAT(PRO.PROD_SPEC_NO1, '-'), PRO.PROD_SPEC_NO2) AS PROD_SPEC_NO");
                selectQuery.Append(" ,PRO.PROD_NAME_CUST");
                selectQuery.Append(" ,PRO.CHARGE");
                selectQuery.Append(" ,CONCAT(PRO.PROD_DIM_D, PRO.PROD_SHAPE) AS PROD_DIM_D_SHAPE");
                selectQuery.Append(" ,( PRO.DIM_TOL_MAX_CHR ");
                selectQuery.Append(" || ','");
                selectQuery.Append(" || PRO.DIM_TOL_MIN_CHR ) AS DIM_TOL");
                selectQuery.Append(" ,FI.FI_DIM1");
                selectQuery.Append(" ,FI.FI_DIM2");
                selectQuery.Append(" ,NVL(TO_CHAR(PRO.PROD_LENGTH),PRO.LENGTH_OTHER) AS PROD_LENGTH_STRING");
                selectQuery.Append(" ,CASE ");
                selectQuery.Append(" WHEN PRO.LENGTH_TOL_MAX_CHR IS NULL AND PRO.LENGTH_TOL_MIN_CHR IS NULL THEN ''");
                selectQuery.Append(" WHEN PRO.LENGTH_TOL_MAX_CHR IS NULL THEN TO_CHAR(PRO.LENGTH_TOL_MIN_CHR)");
                selectQuery.Append(" WHEN PRO.LENGTH_TOL_MIN_CHR IS NULL THEN TO_CHAR(PRO.LENGTH_TOL_MAX_CHR) ");
                selectQuery.Append(" ELSE TO_CHAR(PRO.LENGTH_TOL_MAX_CHR) || ',' || TO_CHAR(PRO.LENGTH_TOL_MIN_CHR)");
                selectQuery.Append(" END AS LEN_TOL");
                selectQuery.Append(" ,FI.FI_LENGTH AS FI_ACT_LENGTH");
                selectQuery.Append(" ,FI.MSG_CD");
                selectQuery.Append(" ,PRO.ACT_ALLOC_QTY");
                selectQuery.Append(" ,FI.NO_OF_BARS");
                selectQuery.Append(" ,FI.FI_TABA_NO");
                selectQuery.Append(" ,FI.FI_QTY");
                selectQuery.Append(" ,FI.UNIT_TYPE");
                selectQuery.Append(" ,FI.FI_LENGTH");
                selectQuery.Append(" ,EMP.SHORT_NAME AS EMPLOYEE_NAME");
                selectQuery.Append(" ,grp_count.group_count");
                selectQuery.Append(" ,MS.MSG_CONTENTS AS FI_REMARK");
                selectQuery.Append(" FROM  TRN_PROCESS_PLAN PRO ");
                selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION  FI  ON FI.PROCESS_PLAN_CD = PRO.PROCESS_PLAN_CD");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER CN ON CN.CUSTOMER_CD = PRO.CUSTOMER_CD");
                selectQuery.Append(" INNER JOIN  MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY");
                selectQuery.Append(" LEFT JOIN  TRN_MESSAGE MS ON MS.MSG_CD = FI.MSG_CD");
                selectQuery.Append(" INNER JOIN (SELECT COUNT(INSP_BY) AS GROUP_COUNT,MAX(FI.INSP_BY) AS INSP_BY ");
                selectQuery.Append(",SUM(FI.FI_QTY) AS SUM_FI_QTY");
                selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append("  INSP_DATE BETWEEN TO_DATE('" + fields.START_DATE + "','yyyy-mm-dd hh24:mi') AND TO_DATE('" + fields.END_DATE + "','yyyy-mm-dd hh24:mi')");
                selectQuery.Append(" AND FI.COMBINED_DECISION ='" + 10 + "'");
                selectQuery.Append(" AND FI.INPUT_LOCATION_CD = ");
            
                selectQuery.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + fields.USER_ID + ")");
                selectQuery.Append(" GROUP BY INSP_BY ) GRP_COUNT ON GRP_COUNT.INSP_BY = FI.INSP_BY  ");

                selectQuery.Append(" WHERE ");
                selectQuery.Append(WhereCondition);
                //selectQuery.Append(" ORDER BY EMPLOYEE_NAME");
                selectQuery.Append(" ORDER BY ");
               if (fields.SORT_ORDER == 0)       
                {
                 selectQuery.Append("INSP_DATE,CHUUBAN_GYOUBAN,FI_TABA_NO ");
               
                }
               else
               {
                selectQuery.Append("CHUUBAN_GYOUBAN,FI_TABA_NO ");
                
               }


            //selectQuery.Append(" ORDER BY  INSP_DATE_FOR_ORDER, PRO.PROD_NAME_CUST,PRO.PROD_DIM_D,PRO.PROD_SHAPE,PRO.PROD_LENGTH,PRO.CHUUBAN,PRO.GYOUBAN,FI.FI_TABA_NO ");
            var result = ExecuteQueryMultiResult<FinalInspectionDetails>(selectQuery.ToString());
            return result;
               
        }

        //Bind FI Simple Report
        public List<FinalInspectionDetails> GetSimpleFinalInspectionDetails(FinalInspectionDetails fields)
        {
            StringBuilder selectQuery = new StringBuilder();
           // fields.OPERATOR_ID = 0;
            StringBuilder WhereCondition = new StringBuilder();

            if (fields.PRINT_FLAG != 1)
            {
                WhereCondition.Append(" INSP_DATE BETWEEN TO_DATE('" + fields.START_DATE + "','yyyy-mm-dd hh24:mi') AND TO_DATE('" + fields.END_DATE + "','yyyy-mm-dd hh24:mi') ");

                if (fields.OPERATOR_ID != 0)    /// If Operator is select from FIPrintReportScreen.
                {
                    WhereCondition.Append(" AND FI.INSP_BY = " + fields.OPERATOR_ID);
                }
            }
            else
            { WhereCondition.Append(" PRINT_DATE IS NULL "); }
                
           
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FI.FINAL_INSP_CD AS FINAL_INSP_NO");
            selectQuery.Append(" ,TO_CHAR(FI.UPDATED_DATE, 'MM/DD') AS INSP_DATE_STRING");
            selectQuery.Append(" ,CN.SHORT_NAME AS CUSTOMER_NAME ");
            selectQuery.Append(" ,TO_CHAR(FI.INSP_DATE, 'YYYY/MM/DD') AS INSP_DATE_FOR_ORDER");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PRO.CHUUBAN,PRO.GYOUBAN) AS CHUUBAN_GYOUBAN");
            selectQuery.Append(" ,CONCAT(CONCAT(PRO.PROD_SPEC_NO1, '-'), PRO.PROD_SPEC_NO2) AS PROD_SPEC_NO");
            selectQuery.Append(" ,PRO.PROD_NAME_CUST");
            selectQuery.Append(" ,PRO.CHARGE");
            selectQuery.Append(" ,CONCAT(PRO.PROD_DIM_D, PRO.PROD_SHAPE) AS PROD_DIM_D_SHAPE");
            selectQuery.Append(" ,( PRO.DIM_TOL_MAX_CHR ");
            selectQuery.Append(" || ','");
            selectQuery.Append(" || PRO.DIM_TOL_MIN_CHR ) AS DIM_TOL");
            selectQuery.Append(" ,FI.FI_DIM1");
            selectQuery.Append(" ,FI.FI_DIM2");
            selectQuery.Append(" ,NVL(TO_CHAR(PRO.PROD_LENGTH),PRO.LENGTH_OTHER) AS PROD_LENGTH_STRING");
            selectQuery.Append(" ,CASE ");
            selectQuery.Append(" WHEN PRO.LENGTH_TOL_MAX_CHR IS NULL AND PRO.LENGTH_TOL_MIN_CHR IS NULL THEN ''");
            selectQuery.Append(" WHEN PRO.LENGTH_TOL_MAX_CHR IS NULL THEN TO_CHAR(PRO.LENGTH_TOL_MIN_CHR)");
            selectQuery.Append(" WHEN PRO.LENGTH_TOL_MIN_CHR IS NULL THEN TO_CHAR(PRO.LENGTH_TOL_MAX_CHR) ");
            selectQuery.Append(" ELSE TO_CHAR(PRO.LENGTH_TOL_MAX_CHR) || ',' || TO_CHAR(PRO.LENGTH_TOL_MIN_CHR)");
            selectQuery.Append(" END AS LEN_TOL");
            selectQuery.Append(" ,FI.FI_LENGTH AS FI_ACT_LENGTH");
            selectQuery.Append(" ,FI.MSG_CD");
            selectQuery.Append(" ,PRO.ACT_ALLOC_QTY");
            selectQuery.Append(" ,FI.NO_OF_BARS");
            selectQuery.Append(" ,FI.FI_TABA_NO");
            selectQuery.Append(" ,FI.FI_QTY");
            selectQuery.Append(" ,FI.UNIT_TYPE");
            selectQuery.Append(" ,FI.FI_LENGTH");
            selectQuery.Append(" ,FI.COMBINED_DECISION");
            selectQuery.Append(" ,EMP.SHORT_NAME AS EMPLOYEE_NAME");
            selectQuery.Append(" ,grp_count.group_count");
            selectQuery.Append(" ,MS.MSG_CONTENTS AS FI_REMARK");
            selectQuery.Append(" FROM  TRN_PROCESS_PLAN PRO ");
            selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION  FI  ON FI.PROCESS_PLAN_CD = PRO.PROCESS_PLAN_CD");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CN ON CN.CUSTOMER_CD = PRO.CUSTOMER_CD");
            selectQuery.Append(" INNER JOIN  MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY");
            selectQuery.Append(" LEFT JOIN  TRN_MESSAGE MS ON MS.MSG_CD = FI.MSG_CD");
            selectQuery.Append(" INNER JOIN (SELECT COUNT(INSP_BY) AS GROUP_COUNT,MAX(FI.INSP_BY) AS INSP_BY ");
            selectQuery.Append(",SUM(FI.FI_QTY) AS SUM_FI_QTY");
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE ");
            if (fields.PRINT_FLAG != 1)
            {
                selectQuery.Append(
                    " INSP_DATE BETWEEN TO_DATE('" + fields.START_DATE + "','yyyy-mm-dd hh24:mi') " +
                    "AND TO_DATE('" + fields.END_DATE + "','yyyy-mm-dd hh24:mi') "
                );
                if (fields.OPERATOR_ID != 0)
                {
                    selectQuery.Append(" AND INSP_BY = " + fields.OPERATOR_ID);
                }
            } 
            else
            {
                selectQuery.Append(" PRINT_DATE IS NULL ");
            }
            selectQuery.Append(" AND FI.EC_CD IS NULL ");
            selectQuery.Append(" AND FI.INPUT_LOCATION_CD = ");
            selectQuery.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + fields.USER_ID + ")");
            selectQuery.Append(" GROUP BY INSP_BY ) GRP_COUNT ON GRP_COUNT.INSP_BY = FI.INSP_BY  ");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FI.INPUT_LOCATION_CD = ( SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID =" + fields.USER_ID + ") ");
            selectQuery.Append(" AND FI.EC_CD IS NULL ");
            selectQuery.Append(" AND ");
            selectQuery.Append(WhereCondition);
            selectQuery.Append(" ORDER BY ");
            if (fields.SORT_ORDER == 0)
            {
                selectQuery.Append("INSP_DATE,CHUUBAN_GYOUBAN,FI_TABA_NO ");

            }
            else
            {
                selectQuery.Append("FI.INSP_BY,CHUUBAN_GYOUBAN,FI_TABA_NO ");

            }

            var result = ExecuteQueryMultiResult<FinalInspectionDetails>(selectQuery.ToString());
            return result;

        }
        public List<string> GetStartEndChuuban(FinalInspectionDetails fields)
        {
            fields.OPERATOR_ID = 0;
            StringBuilder WhereCondition = new StringBuilder();
            WhereCondition.Append("FI.INSP_DATE BETWEEN TO_DATE('" + Convert.ToDateTime(fields.START_DATE).ToString(Constants.DateFormats.YYYYMMDD) + "', 'yyyy-mm-dd') AND TO_DATE('" + Convert.ToDateTime(fields.END_DATE).ToString(Constants.DateFormats.YYYYMMDD) + "', 'yyyy-mm-dd')");
            WhereCondition.Append(" AND COMBINED_DECISION = '" + 10 + "'");
            WhereCondition.Append(" AND  FI.INPUT_LOCATION_CD = (SELECT LOCATION_CD  FROM MST_USER_LOGIN WHERE USER_ID = " + fields.USER_ID + ")");
            if (fields.OPERATOR_ID != 0)    /// If Operator is select from FIPrintReportScreen.
            {
                WhereCondition.Append(" AND INSP_BY = " + fields.OPERATOR_ID);
            }

                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append("SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN");
                selectQuery.Append(",TO_CHAR(PP.GYOUBAN) AS GYOUBAN");
                selectQuery.Append(",TO_CHAR(FI.INSP_DATE ,'YYYY/MM/dd') AS INSP_DATE_VALUE");
                selectQuery.Append(" ,FI.COMBINED_DECISION");
                selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
                selectQuery.Append(" INNER JOIN TRN_FINAL_INSPECTION FI ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD");
                selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY");
                selectQuery.Append(" INNER JOIN (SELECT COUNT(INSP_BY) AS GROUP_COUNT,MAX(FI.INSP_BY) AS INSP_BY ");
                selectQuery.Append(",SUM(FI.FI_QTY) AS SUM_FI_QTY");
                selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" FI.INPUT_LOCATION_CD = ");
                selectQuery.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + fields.USER_ID + ")");
                selectQuery.Append(" GROUP BY INSP_BY ) GRP_COUNT ON GRP_COUNT.INSP_BY = FI.INSP_BY  ");
               
                selectQuery.Append(" WHERE ");
                selectQuery.Append(WhereCondition);
                selectQuery.Append(" ORDER BY EMP.SHORT_NAME");
               if (fields.SORT_ORDER == 0)
               {

                selectQuery.Append(" ,FI.INSP_DATE,CHUUBAN,FI_TABA_NO ");
               }
              else
            {
                selectQuery.Append(" ,CHUUBAN,FI_TABA_NO ");
            }
            

            var result = ExecuteQueryMultiResult<string>(selectQuery.ToString());
                    return result;
        }
        public List<string> GetStartEndChuubanAndThaba(FinalInspectionDetails fields)
        {
                StringBuilder WhereCondition = new StringBuilder();

                WhereCondition.Append(" FI.INSP_DATE BETWEEN TO_DATE('" +
                    Convert.ToDateTime(fields.START_DATE).ToString(Constants.DateFormats.YYYYMMDD) +
                    "','yyyy-mm-dd') AND TO_DATE('" +
                    Convert.ToDateTime(fields.END_DATE).ToString(Constants.DateFormats.YYYYMMDD) +
                    "','yyyy-mm-dd') ");

                WhereCondition.Append(" AND FI.INPUT_LOCATION_CD = ");
                WhereCondition.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + fields.USER_ID + ") ");

                if (fields.OPERATOR_ID != 0)
                {
                    WhereCondition.Append(" AND FI.INSP_BY = " + fields.OPERATOR_ID);
                }

                StringBuilder selectQuery = new StringBuilder();

                selectQuery.Append(@"
                      SELECT 
                            GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN, PP.GYOUBAN)
                            || ','
                            || TO_CHAR(FI.FI_TABA_NO) AS CHUUBAN
                        FROM TRN_PROCESS_PLAN PP
                        INNER JOIN TRN_FINAL_INSPECTION FI
                            ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD
                        INNER JOIN MST_EMPLOYEE EMP
                            ON EMP.EMPLOYEE_CD = FI.INSP_BY
                        WHERE ");

                selectQuery.Append(WhereCondition);

                if (fields.SORT_ORDER == 0)
                {
                    selectQuery.Append(" ORDER BY FI.INSP_DATE, CHUUBAN, FI.FI_TABA_NO ");
                }
                else
                {
                    selectQuery.Append(" ORDER BY CHUUBAN, FI.FI_TABA_NO ");
                }

                return ExecuteQueryMultiResult<string>(selectQuery.ToString());
            
        }
        public List<WipStatusDetails> GetWIPStatusData(int userId, int locationCd)
        {
            StringBuilder selectQuery = new StringBuilder();
            
                selectQuery.Append(" SELECT " );
                selectQuery.Append(" PP_CHUUBAN_GYOUBAN ");
                selectQuery.Append(" ,PP_CUSTOMER_NAME ");
                selectQuery.Append(" ,PP_PROD_NAME_CUST ");
                selectQuery.Append(" ,INTERNAL_NAME ");
                selectQuery.Append(" ,PP_PROD_SPEC_NO ");
                selectQuery.Append(" ,PP_PROD_LENGTH_OTHER ");
                selectQuery.Append(" ,PP_PROD_DIM_D ");
                selectQuery.Append(" ,PP_PROD_SHAPE ");
                selectQuery.Append(" ,PP_CHARGE ");
                selectQuery.Append(" ,TABA_NO ");
                selectQuery.Append(", PP_PRODUCT_CONDITION");
                selectQuery.Append(" ,PP_DIM_TOLERANCE ");
                selectQuery.Append(" ,WIP_NO_OF_UNITS ");
                selectQuery.Append(" ,UNIT ");
                selectQuery.Append(" ,WIP_QTY ");
                selectQuery.Append(" ,PRE_PROSESS_DATE ");
                selectQuery.Append(" ,DELAY_DAYS_A ");
                selectQuery.Append(" ,DELAY_DAYS_B ");
                selectQuery.Append(" ,PP_ACT_ALLOC_DATE_STRING ");
                selectQuery.Append(" ,PP_ACT_ALLOC_QTY ");
                selectQuery.Append(" ,PP_EMPLOYEE_NAME ");
                selectQuery.Append(" ,PP_DELIVERY_DATE_STRING ");
                selectQuery.Append(" ,PROCESS_START_DATE_STRING ");
                selectQuery.Append(" ,REMARK ");
                selectQuery.Append(" ,PROD_SPEC_NO ");
                selectQuery.Append(" ,LOCATION_CD ");
                selectQuery.Append(" ,NEXT_PROSESS ");
                //selectQuery.Append(" ,PRE_PROCESS");
                selectQuery.Append(" ,PROCESS_WAITING_DAYS ");
                selectQuery.Append(" ,PROCESS_KUBUN_JP ");
                selectQuery.Append(" ,PROCESS_KUBUN_EN ");
                selectQuery.Append(" ,NEXT_PROCESS_JP ");
                selectQuery.Append(" ,NEXT_PROCESS_EN ");
                selectQuery.Append(" ,GROUP_COUNT ");
                selectQuery.Append(",PROCESS_PLAN_CD  ");
                selectQuery.Append(" FROM ( ");
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,");
                selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
                selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_OTHER,");
                selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
                selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
                selectQuery.Append(" WIP.TABA_NO AS TABA_NO, ");
                selectQuery.Append("CON.CONDITION_DESCRIPTION PP_PRODUCT_CONDITION,");
                selectQuery.Append(" CONCAT(DIM_TOL_MAX_CHR, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN DIM_TOL_MIN_CHR IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(',', DIM_TOL_MIN_CHR) ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END ");
                selectQuery.Append(") AS PP_DIM_TOLERANCE, ");
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS AS WIP_NO_OF_UNITS,");
                selectQuery.Append(" WIP.UNIT AS UNIT, ");
                selectQuery.Append(" WIP.WIP_QTY AS WIP_QTY,");
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD') AS PRE_PROSESS_DATE,");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A,");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
                selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING,");
                selectQuery.Append(" TO_DATE('') AS PROCESS_START_DATE_STRING,");
                selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.LOCATION_CD,");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROSESS,");
                //selectQuery.Append(" WIP.PRE_PROCESS AS PRE_PROCESS,");
                selectQuery.Append(" FLOOR(CURRENT_DATE - wip.PRE_PROCESS_DATE) AS PROCESS_WAITING_DAYS ,");
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
                selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");
                selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");
                selectQuery.Append(" WIP.PROCESS_PLAN_CD, ");
                //selectQuery.Append(" ( SELECT COUNT(wip2.pre_process) AS GROUP_COUNT ");
                //selectQuery.Append(" FROM TRN_WIP_STATUS WIP2 ");
                //selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN   PC ON WIP2.PRE_PROCESS = PC.KUBUN_ID");
                //selectQuery.Append(" WHERE WIP2.LOCATION_CD ='"+ locationCd + "' GROUP BY ");

                //selectQuery.Append(" WIP2.PRE_PROCESS,  WIP2.PROCESS_PLAN_CD ");
                //selectQuery.Append(" HAVING WIP2.PRE_PROCESS = WIP.PRE_PROCESS ");
                //selectQuery.Append(" AND WIP2.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                //selectQuery.Append(" ) AS GROUP_COUNT ");
                selectQuery.Append(" GC.GROUP_COUNT");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_WIP_STATUS WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DEP.EMPLOYEE_CD ");
                selectQuery.Append("LEFT JOIN MST_CONDITION con ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = WIP.MSG_CD ");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" LEFT JOIN ( SELECT COUNT(WIP2.PRE_PROCESS) AS GROUP_COUNT, WIP2.PRE_PROCESS FROM ");
                selectQuery.Append(" TRN_WIP_STATUS WIP2 INNER JOIN MST_PROCESS_KUBUN PC ON WIP2.PRE_PROCESS = PC.KUBUN_ID ");
                selectQuery.Append(" WHERE WIP2.LOCATION_CD = '" + locationCd + "'  ");
                selectQuery.Append(" GROUP BY WIP2.PRE_PROCESS ) GC ON WIP.PRE_PROCESS = GC.PRE_PROCESS");
                selectQuery.Append("   WHERE WIP.PRE_PROCESS IS NOT NULL");
                selectQuery.Append(" ) ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" LOCATION_CD = '" + locationCd + "'  ");
                selectQuery.Append(" ORDER BY  PROCESS_KUBUN_JP,NEXT_PROCESS_EN,PP_CHUUBAN_GYOUBAN ASC,TABA_NO ASC");
                return ExecuteQueryMultiResult<WipStatusDetails>(selectQuery.ToString());
        }
        //public List<WIPTensouDetails> GetWIPTensouAceeptanceDetailsbyDateLocation(string startdate, string enddate, int locationCD)
        //{
        //    StringBuilder selectQuery = new StringBuilder();

        //    //string startTransferDate = startdate + " 00:01";
        //    //string endTransferDate = enddate + " 23:59";

        //    selectQuery.Append(" SELECT ");
        //    selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
        //    selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
        //    selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
        //    selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
        //    selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER('',PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER)AS PROD_DIM_W_OTHER ");
        //    //selectQuery.Append(" ,PP.LENGTH_OTHER AS LENGTH_OTHER ");
        //    selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS LENGTH_OTHER");
        //    selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");

        //    selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
        //    //COMBINE TABA NO
        //    /* selectQuery.Append(", CASE WHEN WIP.TABA_NO_2 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1");
        //     selectQuery.Append(" || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 ");
        //     selectQuery.Append(" ELSE");
        //     selectQuery.Append(" CASE");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_4 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_4 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" TO_CHAR(WIP.TABA_NO_1)");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END  ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" AS TABA_NO");*/
        //    selectQuery.Append(", RTRIM(");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') || ");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') || ");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') || ");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', '')");
        //    selectQuery.Append(", ',') AS TABA_NO ");

        //    selectQuery.Append(",WIP.NO_OF_UNITS as NO_OF_UNITS ");
        //    selectQuery.Append(",WIP.UNIT_TYPE as UNIT_TYPE");
        //    selectQuery.Append(",WIP.QTY AS PP_QTY");
        //    selectQuery.Append(",PP.DELIVERY_DATE AS PP_DELIVERY_DATE_STRING");
        //    selectQuery.Append(",WIP.PLANNED_NEXT_PROSESS AS WIP_NEXT_PROSESS");
        //    selectQuery.Append(",concat(WIP.WIP_TRANSFER_GROUP_NO,WIP.SERIAL_NUMBER) as WIP_TRANSFER_GRPNO_SERNO");
        //    selectQuery.Append(",WIP.WIP_TRANSFER_GROUP_NO");
        //    selectQuery.Append(", LOC_FROM.SHORT_NAME AS LOCATION_FROM");
        //    selectQuery.Append(", LOC_TO.SHORT_NAME AS LOCATION_TO");
        //    selectQuery.Append(",WIP.WIP_TRANSFER_CD AS TRANSFER_CD");
        //    selectQuery.Append(",WIP.TRANSFER_DATE AS WIP_TRANSFER_DATE");
        //    selectQuery.Append(",WIP.INPUT_LOCATION_CD");
        //    selectQuery.Append(",MSG.MSG_CONTENTS AS REMARK");
        //    selectQuery.Append(",GC.GROUP_COUNT");
        //    selectQuery.Append("  FROM TRN_WIP_TRANSFER WIP ");
        //    selectQuery.Append("  INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD =PP.PROCESS_PLAN_CD ");
        //    selectQuery.Append("  LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = WIP.MSG_CD ");
        //    selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
        //    selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
        //    selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
        //    selectQuery.Append("LEFT JOIN (select LOCATION_CD_FROM, COUNT(LOCATION_CD_FROM) AS GROUP_COUNT FROM TRN_WIP_TRANSFER WHERE TRUNC(TRANSFER_DATE) BETWEEN TO_DATE('" + startdate + "','yyyy/mm/dd ') and to_date('" + enddate + "','yyyy/mm/dd ') AND LOCATION_CD_TO = '" + locationCD + "' AND  TRANSFER_DONE_DATE IS NULL GROUP BY LOCATION_CD_FROM) GC ON WIP.LOCATION_CD_FROM=GC.LOCATION_CD_FROM ");

        //    selectQuery.Append(" WHERE TRUNC(WIP.TRANSFER_DATE) BETWEEN TO_DATE('" + startdate + "','yyyy/mm/dd ') and to_date('" + enddate + "','yyyy/mm/dd ')");
        //    selectQuery.Append(" AND WIP.LOCATION_CD_TO= '" + locationCD + "'");
        //    selectQuery.Append("  AND TRANSFER_DONE_DATE IS NULL ");
        //    selectQuery.Append(" ORDER BY LOCATION_FROM,GC.GROUP_COUNT,WIP.WIP_TRANSFER_CD ");
        //    //selectQuery.Append(" ORDER BY LOCATION_FROM,GC.GROUP_COUNT,CUS.SHORT_NAME ,CUS.YOBIGANA,CUS.YOBINA,PP.PROD_NAME_CUST"); // LOC_TO.SHORT_NAME, 
        //    //selectQuery.Append(",PP.MATL_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER");
        //    //selectQuery.Append(",PP.LENGTH_OTHER,PP.MATL_LENGTH,PP.CHUUBAN,WIP.TABA_NO_1");

        //    return ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());
        //}
        //public List<WIPTensouDetails> GetWIPTensouRequestDetailsbyDateLocation(string date, int locationCD)
        //{
        //    DateTime fromDate = Convert.ToDateTime(date);
        //    fromDate = fromDate.AddMonths(-1);
        //    StringBuilder selectQuery = new StringBuilder();


        //    selectQuery.Append(" SELECT ");
        //    // selectQuery.Append("  PP.CHUUBAN AS PP_CHUUBAN ");
        //    selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
        //    selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
        //    selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
        //    selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
        //    selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER('',PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER)AS PROD_DIM_W_OTHER ");
        //    //selectQuery.Append(" ,PP.LENGTH_OTHER AS LENGTH_OTHER ");
        //    selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS LENGTH_OTHER");
        //    selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
        //    selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE");

        //    /* selectQuery.Append(", CASE WHEN WIP.TABA_NO_2 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1");
        //     selectQuery.Append(" || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 ");
        //     selectQuery.Append(" ELSE");
        //     selectQuery.Append(" CASE");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_4 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" CASE ");
        //     selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
        //     selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
        //     selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_4 ");
        //     selectQuery.Append(" ELSE ");
        //     selectQuery.Append(" TO_CHAR(WIP.TABA_NO_1)");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END  ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" END ");
        //     selectQuery.Append(" AS TABA_NO");*/
        //    //Bind Thaba no(ex.1,2,3,4)
        //    selectQuery.Append(", RTRIM(");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') || ");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') || ");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') || ");
        //    selectQuery.Append("NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', '')");
        //    selectQuery.Append(", ',') AS TABA_NO ");

        //    selectQuery.Append(",WIP.NO_OF_UNITS");
        //    selectQuery.Append(",WIP.UNIT_TYPE");
        //    selectQuery.Append(",WIP.QTY AS PP_QTY");
        //    selectQuery.Append(",PP.DELIVERY_DATE AS PP_DELIVERY_DATE_STRING");
        //    selectQuery.Append(",WIP.PLANNED_NEXT_PROSESS AS WIP_NEXT_PROSESS");
        //    selectQuery.Append(",concat(WIP.WIP_TRANSFER_GROUP_NO,WIP.SERIAL_NUMBER) as WIP_TRANSFER_GRPNO_SERNO");
        //    selectQuery.Append(",WIP.WIP_TRANSFER_GROUP_NO");
        //    selectQuery.Append(", LOC_FROM.SHORT_NAME AS LOCATION_FROM");
        //    selectQuery.Append(", LOC_TO.SHORT_NAME AS LOCATION_TO");
        //    selectQuery.Append(",WIP.WIP_TRANSFER_CD AS TRANSFER_CD");
        //    selectQuery.Append(",WIP.TRANSFER_DATE AS WIP_TRANSFER_DATE");
        //    selectQuery.Append(",WIP.LOCATION_CD_TO");
        //    selectQuery.Append(" ,MSG.MSG_CONTENTS as REMARK");
        //    selectQuery.Append(",GC.GROUP_COUNT");
        //    // selectQuery.Append(" (SELECT COUNT(LOCATION_CD_TO) FROM MST_LOCATION LC_TO WHERE LC_TO.LOCATION_CD=WIP.LOCATION_CD_TO and WIP.LOCATION_CD_FROM= '" + locationCD + "' )AS GROUP_COUNT ");
        //    selectQuery.Append("  FROM TRN_WIP_TRANSFER WIP  ");
        //    selectQuery.Append("  INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD =PP.PROCESS_PLAN_CD ");
        //    selectQuery.Append("  LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = WIP.MSG_CD ");
        //    selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
        //    selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
        //    selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
        //    selectQuery.Append("LEFT JOIN (SELECT  LOCATION_CD_TO AS LOCATION_CD, COUNT(LOCATION_CD_TO) AS GROUP_COUNT FROM TRN_WIP_TRANSFER WHERE TRUNC(TRANSFER_DATE) = TO_DATE ('" + date + "', 'yyyy/MM/dd') AND INPUT_LOCATION_CD = '" + locationCD + "' GROUP BY LOCATION_CD_TO) GC ON WIP.LOCATION_CD_TO=GC.LOCATION_CD");
        //    //selectQuery.Append("LEFT JOIN (SELECT  INPUT_LOCATION_CD AS LOCATION_CD, COUNT(INPUT_LOCATION_CD) AS GROUP_COUNT FROM TRN_WIP_TRANSFER WHERE WIP.TRANSFER_DATE BETWEEN TO_DATE ('" + fromDate.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd' ) AND  TO_DATE ('" + date + "', 'yyyy/MM/dd')");

        //    // selectQuery.Append(" WHERE WIP.TRANSFER_DATE <= '" + date + "'");
        //    selectQuery.Append(" where TRUNC(WIP.TRANSFER_DATE) = TO_DATE ('" + date + "', 'yyyy/MM/dd')");
        //    selectQuery.Append(" AND WIP.INPUT_LOCATION_CD= '" + locationCD + "'");
        //    selectQuery.Append(" ORDER BY LOCATION_CD_TO,WIP.WIP_TRANSFER_CD ");
        //    // selectQuery.Append(" ORDER BY LOC_TO.SHORT_NAME,CUS.SHORT_NAME ,CUS.YOBIGANA,CUS.YOBINA,PP.PROD_NAME_CUST");
        //    //  selectQuery.Append(",PP.MATL_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER");
        //    // selectQuery.Append(",PP.LENGTH_OTHER,PP.MATL_LENGTH,PP.CHUUBAN,WIP.TABA_NO_1");
        //    return ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());

        //}

        //



        /// <summary>
        /// Add by komal
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="yearR"></param>
        /// <returns></returns>
        /// 
        public List<WIPTensouDetails> GetWIPTensouAceeptanceDetailsbyDateLocation(string startdate, string enddate, int locationCD)
        {
            StringBuilder selectQuery = new StringBuilder();

            //string startTransferDate = startdate + " 00:01";
            //string endTransferDate = enddate + " 23:59";

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" 1 AS ITEM_TYPE ");// Type 1 is WIP
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER('',PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER)AS PROD_DIM_W_OTHER ");
            //selectQuery.Append(" ,PP.LENGTH_OTHER AS LENGTH_OTHER ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS LENGTH_OTHER");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");

            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            //COMBINE TABA NO
            /* selectQuery.Append(", CASE WHEN WIP.TABA_NO_2 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1");
             selectQuery.Append(" || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 ");
             selectQuery.Append(" ELSE");
             selectQuery.Append(" CASE");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_4 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_4 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" TO_CHAR(WIP.TABA_NO_1)");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END  ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" AS TABA_NO");*/
            selectQuery.Append(", RTRIM(");
            selectQuery.Append("NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') || ");
            selectQuery.Append("NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') || ");
            selectQuery.Append("NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') || ");
            selectQuery.Append("NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', '')");
            selectQuery.Append(", ',') AS TABA_NO ");

            selectQuery.Append(",WIP.NO_OF_UNITS as NO_OF_UNITS ");
            selectQuery.Append(",WIP.UNIT_TYPE as UNIT_TYPE");
            selectQuery.Append(",WIP.QTY AS PP_QTY");
            selectQuery.Append(",PP.DELIVERY_DATE AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(",WIP.PLANNED_NEXT_PROSESS AS WIP_NEXT_PROSESS");
            selectQuery.Append(",concat(WIP.TRANSFER_GROUP_NO,WIP.TRANSFER_CD) as WIP_TRANSFER_GRPNO_SERNO");
            selectQuery.Append(",WIP.TRANSFER_GROUP_NO");
            selectQuery.Append(", LOC_FROM.SHORT_NAME AS LOCATION_FROM");
            selectQuery.Append(", LOC_TO.SHORT_NAME AS LOCATION_TO");
            selectQuery.Append(",WIP.TRANSFER_CD AS TRANSFER_CD");
            selectQuery.Append(",WIP.TRANSFER_DATE AS WIP_TRANSFER_DATE");
            selectQuery.Append(",TRUNC(WIP.ACCEPT_PLANNED_DATE) AS WIP_ACCEPT_PLANNED_DATE");
            selectQuery.Append(",WIP.INPUT_LOCATION_CD");
            selectQuery.Append(",MSG.MSG_CONTENTS AS REMARK");
            selectQuery.Append(" ,VEH.SHORT_NAME AS VEHICLE_NAME");
            selectQuery.Append(" ,CON.CONDITION_DESCRIPTION  AS CONDITION_NAME");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(" ,GC.PP_MATL_TYPE_CBK");
            selectQuery.Append(",GC.GROUP_COUNT");
            selectQuery.Append("  FROM TRN_WIP_AND_PROD_TRANSFER WIP ");
            selectQuery.Append("  INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD =PP.PROCESS_PLAN_CD ");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = WIP.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            // selectQuery.Append("LEFT JOIN (select LOCATION_CD_FROM, COUNT(LOCATION_CD_FROM) AS GROUP_COUNT FROM TRN_WIP_AND_PROD_TRANSFER WHERE TRUNC(TRANSFER_DATE) BETWEEN TO_DATE('" + startdate + "','yyyy/mm/dd ') and to_date('" + enddate + "','yyyy/mm/dd ') AND LOCATION_CD_TO = '" + locationCD + "' AND  ACCEPTANCE_DATE IS NULL GROUP BY LOCATION_CD_FROM) GC ON WIP.LOCATION_CD_FROM=GC.LOCATION_CD_FROM ");

            selectQuery.Append(" LEFT JOIN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOCATION_CD_TO, ");
            selectQuery.Append(" START_VEHICLEL_CD, ");
            //selectQuery.Append(" TRUNC(WIP.TRANSFER_DATE) AS TRANSFER_DATE ,");
            selectQuery.Append(" TRUNC(ACCEPT_PLANNED_DATE) As ACCEPT_PLANNED_DATE,");
            selectQuery.Append(" LOCATION_CD_FROM, ");
            selectQuery.Append(" TRANSIT_LOCATION_CD, ");
            selectQuery.Append("  PP_MATL_TYPE_CBK, ");
            selectQuery.Append("   COUNT(*) AS group_count ");
            selectQuery.Append("     FROM ( ");
            selectQuery.Append("         SELECT ");
            selectQuery.Append("             LOCATION_CD_TO, ");
            selectQuery.Append("             START_VEHICLEL_CD, ");
            //selectQuery.Append(" TRUNC(WIP.TRANSFER_DATE) AS TRANSFER_DATE ,");
            selectQuery.Append(" TRUNC(ACCEPT_PLANNED_DATE) As ACCEPT_PLANNED_DATE,");
            selectQuery.Append("             TRANSIT_LOCATION_CD, ");
            selectQuery.Append("             LOCATION_CD_FROM, ");
            selectQuery.Append("             CASE ");
            selectQuery.Append("                 WHEN unit_type = 4 THEN 'COIL' ");
            selectQuery.Append("                 ELSE 'BAR' ");
            selectQuery.Append("             END AS PP_MATL_TYPE_CBK ");
            selectQuery.Append("         FROM TRN_WIP_AND_PROD_TRANSFER ");
            selectQuery.Append(" WHERE TRUNC(ACCEPT_PLANNED_DATE) BETWEEN TO_DATE('" + startdate + "','yyyy/mm/dd ') and to_date('" + enddate + "','yyyy/mm/dd ')");
            selectQuery.Append(" AND LOCATION_CD_TO= '" + locationCD + "'");
            selectQuery.Append("     ) ");
            selectQuery.Append("     GROUP BY ");
            selectQuery.Append("         LOCATION_CD_TO, ");
            selectQuery.Append("         START_VEHICLEL_CD, ");
            //selectQuery.Append(" TRUNC(WIP.TRANSFER_DATE) ,");
            selectQuery.Append(" TRUNC(ACCEPT_PLANNED_DATE) ,");
            selectQuery.Append("         TRANSIT_LOCATION_CD, ");
            selectQuery.Append("             LOCATION_CD_FROM, ");
            selectQuery.Append("         PP_MATL_TYPE_CBK ");
            selectQuery.Append(" ) gc  ON ");
            selectQuery.Append("      gc.location_cd_to = wip.location_cd_to ");
            selectQuery.Append("     AND gc.start_vehiclel_cd = wip.start_vehiclel_cd ");
            //selectQuery.Append("     AND gc.TRANSFER_DATE = TRUNC(WIP.TRANSFER_DATE) ");
            selectQuery.Append("     AND gc.ACCEPT_PLANNED_DATE = TRUNC(WIP.ACCEPT_PLANNED_DATE) ");
            selectQuery.Append("     AND gc.LOCATION_CD_FROM = wip.LOCATION_CD_FROM ");
            selectQuery.Append("     AND gc.pp_matl_type_cbk = ");
            selectQuery.Append("         CASE ");
            selectQuery.Append("             WHEN wip.unit_type = 4 THEN 'COIL' ");
            selectQuery.Append("             ELSE 'BAR' ");
            selectQuery.Append("         END ");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE VEH ON VEH.VEHICLE_CD = wip.START_VEHICLEL_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" WHERE TRUNC(WIP.ACCEPT_PLANNED_DATE) BETWEEN TO_DATE('" + startdate + "','yyyy/mm/dd ') and to_date('" + enddate + "','yyyy/mm/dd ')");
            selectQuery.Append(" AND WIP.location_cd_to= '" + locationCD + "'");
            selectQuery.Append("  AND ACCEPTANCE_DATE IS NULL ");

            selectQuery.Append(" ORDER BY ");
            selectQuery.Append(" WIP.START_VEHICLEL_CD,");
            selectQuery.Append(" WIP.LOCATION_CD_TO,");
            selectQuery.Append(" WIP.LOCATION_CD_FROM,");
            //selectQuery.Append(" TRUNC(WIP.TRANSFER_DATE),");
            selectQuery.Append(" TRUNC(WIP.ACCEPT_PLANNED_DATE),");
            selectQuery.Append(" WIP.TRANSIT_LOCATION_CD,");
            selectQuery.Append(" GC.PP_MATL_TYPE_CBK,");
            selectQuery.Append("  WIP.TRANSFER_CD");
            //selectQuery.Append(" ORDER BY LOCATION_FROM,GC.GROUP_COUNT,CUS.SHORT_NAME ,CUS.YOBIGANA,CUS.YOBINA,PP.PROD_NAME_CUST"); // LOC_TO.SHORT_NAME, 
            //selectQuery.Append(",PP.MATL_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER");
            //selectQuery.Append(",PP.LENGTH_OTHER,PP.MATL_LENGTH,PP.CHUUBAN,WIP.TABA_NO_1");

            return ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());
        }
        public List<WIPTensouDetails> GetWIPTensouRequestDetailsbyDateLocation(string date, int locationCD)
        {
            DateTime fromDate = Convert.ToDateTime(date);
            fromDate = fromDate.AddMonths(-1);
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            // selectQuery.Append("  PP.CHUUBAN AS PP_CHUUBAN ");
            selectQuery.Append(" 1 AS ITEM_TYPE ");// Type 1 is WIP
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER('',PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER)AS PROD_DIM_W_OTHER ");
            //selectQuery.Append(" ,PP.LENGTH_OTHER AS LENGTH_OTHER ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS LENGTH_OTHER");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE");

            /* selectQuery.Append(", CASE WHEN WIP.TABA_NO_2 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1");
             selectQuery.Append(" || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 ");
             selectQuery.Append(" ELSE");
             selectQuery.Append(" CASE");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_4 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" CASE ");
             selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
             selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
             selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_4 ");
             selectQuery.Append(" ELSE ");
             selectQuery.Append(" TO_CHAR(WIP.TABA_NO_1)");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END  ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" END ");
             selectQuery.Append(" AS TABA_NO");*/
            //Bind Thaba no(ex.1,2,3,4)
            selectQuery.Append(", RTRIM(");
            selectQuery.Append("NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') || ");
            selectQuery.Append("NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') || ");
            selectQuery.Append("NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') || ");
            selectQuery.Append("NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', '')");
            selectQuery.Append(", ',') AS TABA_NO ");
            selectQuery.Append(",WIP.NO_OF_UNITS");
            selectQuery.Append(",WIP.UNIT_TYPE");
            selectQuery.Append(",WIP.QTY AS PP_QTY");
            selectQuery.Append(",PP.DELIVERY_DATE AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(",WIP.PLANNED_NEXT_PROSESS AS WIP_NEXT_PROSESS");
            selectQuery.Append(",concat(WIP.TRANSFER_GROUP_NO,WIP.TRANSFER_CD) as WIP_TRANSFER_GRPNO_SERNO");
            selectQuery.Append(",WIP.TRANSFER_GROUP_NO");
            selectQuery.Append(", LOC_FROM.SHORT_NAME AS LOCATION_FROM");
            selectQuery.Append(", LOC_TO.SHORT_NAME AS LOCATION_TO");
            selectQuery.Append(",WIP.TRANSFER_CD AS TRANSFER_CD");
            selectQuery.Append(", TRUNC(WIP.TRANSFER_DATE) AS WIP_TRANSFER_DATE");
            selectQuery.Append(",TRUNC(WIP.ACCEPT_PLANNED_DATE) AS WIP_ACCEPT_PLANNED_DATE");
            selectQuery.Append(", WIP.LOCATION_CD_TO");
            selectQuery.Append(" ,MSG.MSG_CONTENTS as REMARK");
            selectQuery.Append(" ,VEH.SHORT_NAME AS VEHICLE_NAME");
            selectQuery.Append(" ,CON.CONDITION_DESCRIPTION  AS CONDITION_NAME");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(" ,GC.PP_MATL_TYPE_CBK");
            selectQuery.Append(",GC.GROUP_COUNT");
            // selectQuery.Append(" (SELECT COUNT(LOCATION_CD_TO) FROM MST_LOCATION LC_TO WHERE LC_TO.LOCATION_CD=WIP.LOCATION_CD_TO and WIP.LOCATION_CD_FROM= '" + locationCD + "' )AS GROUP_COUNT ");
            selectQuery.Append("  FROM TRN_WIP_AND_PROD_TRANSFER WIP  ");
            selectQuery.Append("  INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD =PP.PROCESS_PLAN_CD ");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = WIP.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            //selectQuery.Append("LEFT JOIN (SELECT START_VEHICLEL_CD,LOCATION_CD_TO,TRANSIT_LOCATION_CD, PP_MATL_TYPE_CBK,  MAX(TRANSFER_CD) AS TRANSFER_CD,COUNT(*) AS GROUP_COUNT FROM (select  start_vehiclel_cd,location_cd_to,transit_location_cd,transfer_cd, CASE WHEN unit_type  = 4      THEN 'COIL'      ELSE      'BAR'      END      AS pp_matl_type_cbk  from TRN_WIP_AND_PROD_TRANSFER WHERE TRUNC(TRANSFER_DATE) = TO_DATE ('" + date + "', 'yyyy/MM/dd') AND INPUT_LOCATION_CD = '" + locationCD + "') GROUP BY START_VEHICLEL_CD,TRANSIT_LOCATION_CD,LOCATION_CD_TO ,PP_MATL_TYPE_CBK) GC  ON GC.TRANSFER_CD= WIP.TRANSFER_CD ");
            selectQuery.Append(" LEFT JOIN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" start_vehiclel_cd, ");
            selectQuery.Append(" location_cd_to, ");
            selectQuery.Append(" LOCATION_CD_FROM,");
            //selectQuery.Append(" TRUNC(TRANSFER_DATE) AS TRANSFER_DATE,");
            selectQuery.Append(" WIP_ACCEPT_PLANNED_DATE,");
            selectQuery.Append(" transit_location_cd, ");
            selectQuery.Append(" pp_matl_type_cbk, ");
            selectQuery.Append(" COUNT(*) AS group_count ");
            selectQuery.Append(" FROM ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" start_vehiclel_cd, ");
            selectQuery.Append(" location_cd_to, ");
            selectQuery.Append(" LOCATION_CD_FROM,");
            //selectQuery.Append(" TRUNC(TRANSFER_DATE) AS TRANSFER_DATE,");,
            selectQuery.Append(" TRUNC(ACCEPT_PLANNED_DATE) AS WIP_ACCEPT_PLANNED_DATE,");
            selectQuery.Append(" transit_location_cd, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN unit_type = 4 THEN 'COIL' ");
            selectQuery.Append(" ELSE 'BAR' ");
            selectQuery.Append(" END AS pp_matl_type_cbk ");
            selectQuery.Append(" FROM trn_wip_and_prod_transfer ");
            selectQuery.Append(" WHERE TRUNC(transfer_date) = TO_DATE('" + date + "', 'YYYY/MM/DD') ");
            selectQuery.Append(" AND input_location_cd = '" + locationCD + "' ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" START_VEHICLEL_CD, ");
            selectQuery.Append(" LOCATION_CD_TO, ");
            selectQuery.Append(" LOCATION_CD_FROM,");
            //selectQuery.Append(" TRUNC(TRANSFER_DATE),");
            selectQuery.Append(" WIP_ACCEPT_PLANNED_DATE,");
            selectQuery.Append(" transit_location_cd, ");
            selectQuery.Append(" PP_MATL_TYPE_CBK ");
            selectQuery.Append(" ) gc ");
            selectQuery.Append(" ON gc.START_VEHICLEL_CD = wip.START_VEHICLEL_CD ");
            selectQuery.Append(" AND gc.LOCATION_CD_TO = wip.LOCATION_CD_TO ");
            //selectQuery.Append(" AND gc.TRANSFER_DATE = TRUNC(WIP.TRANSFER_DATE) ");
            selectQuery.Append("  AND WIP_ACCEPT_PLANNED_DATE = TRUNC(WIP.ACCEPT_PLANNED_DATE)");
            selectQuery.Append(" AND gc.LOCATION_CD_FROM = wip.LOCATION_CD_FROM ");
            selectQuery.Append(" AND gc.PP_MATL_TYPE_CBK = ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN wip.UNIT_TYPE = 4 THEN 'COIL' ");
            selectQuery.Append(" ELSE 'BAR' ");
            selectQuery.Append(" END ");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE VEH ON VEH.VEHICLE_CD = wip.START_VEHICLEL_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");

            //selectQuery.Append("LEFT JOIN (SELECT  INPUT_LOCATION_CD AS LOCATION_CD, COUNT(INPUT_LOCATION_CD) AS GROUP_COUNT FROM TRN_WIP_TRANSFER WHERE WIP.TRANSFER_DATE BETWEEN TO_DATE ('" + fromDate.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd' ) AND  TO_DATE ('" + date + "', 'yyyy/MM/dd')");

            // selectQuery.Append(" WHERE WIP.TRANSFER_DATE <= '" + date + "'");
            selectQuery.Append(" where TRUNC(WIP.TRANSFER_DATE) = TO_DATE ('" + date + "', 'yyyy/MM/dd')");
            selectQuery.Append(" AND WIP.INPUT_LOCATION_CD= '" + locationCD + "'");
            selectQuery.Append(" ORDER BY ");
            selectQuery.Append(" WIP.START_VEHICLEL_CD,");
            selectQuery.Append(" WIP.LOCATION_CD_TO,");
            selectQuery.Append(" WIP.LOCATION_CD_FROM,");
            //selectQuery.Append(" TRUNC(WIP.TRANSFER_DATE),");
            selectQuery.Append(" TRUNC(WIP.ACCEPT_PLANNED_DATE),");
            selectQuery.Append(" WIP.TRANSIT_LOCATION_CD,");
            selectQuery.Append(" GC.PP_MATL_TYPE_CBK ,");
            selectQuery.Append("  WIP.TRANSFER_CD");

            // selectQuery.Append(" ORDER BY LOC_TO.SHORT_NAME,CUS.SHORT_NAME ,CUS.YOBIGANA,CUS.YOBINA,PP.PROD_NAME_CUST");
            //  selectQuery.Append(",PP.MATL_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER");
            // selectQuery.Append(",PP.LENGTH_OTHER,PP.MATL_LENGTH,PP.CHUUBAN,WIP.TABA_NO_1");
            return ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());

        }

        public List<RollPoMainAnalysisDetails> GetPlaceOrderList(int userId, int yearR)
        {

            List<RollPoMainAnalysisDetails> list = new List<RollPoMainAnalysisDetails>();
            List<RollPoMainAnalysisDetails> result = new List<RollPoMainAnalysisDetails>();

            //StringBuilder selectPoNO = new StringBuilder();
            //selectPoNO.Append(" SELECT PO_NO ");
            //selectPoNO.Append(" FROM TRN_PO_HDR ");
            //selectPoNO.Append(" WHERE CREATED_DATE = ( ");
            //selectPoNO.Append(" SELECT MAX(CREATED_DATE) ");
            //selectPoNO.Append(" FROM TRN_PO_HDR ");
            //selectPoNO.Append(" ) ");
            //list = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(selectPoNO.ToString());

            //foreach (var row in list)
            //{
                StringBuilder selectQuery = new StringBuilder();

                selectQuery.Append(" SELECT ");
                selectQuery.Append(" PH.PO_NO, ");
                selectQuery.Append(" PO_DTL_CD, ");
                selectQuery.Append(" PO_DTL_NO, ");
                selectQuery.Append(" PO_DTL_SRLNO, ");
                selectQuery.Append(" PH.MATL_KUBUN AS MATERIAL_KUBUN, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN MAK.MAKER_NAME IS NOT NULL THEN ");
                selectQuery.Append(" TO_CHAR(MAK.MAKER_NAME || ' 殿') ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END MAKER_NAME, ");
                selectQuery.Append(" PD.PO_KUBUN, ");
                selectQuery.Append(" MATL_MAKER_NAME, ");
                selectQuery.Append(" MATERIAL_LENGTH, ");
                selectQuery.Append(" CONCAT(CONCAT(DIM_D, ' '), SHAPE) DIM_SHAPE, ");
                selectQuery.Append(" DIM_OTHER, ");
                selectQuery.Append(" CM.CONDITION_DESCRIPTION, ");
                selectQuery.Append(" MO.MKSAKI_NAME MOCHIKOMISAKI_NAME, ");
                selectQuery.Append(" MUK.MUKESAKI_NAME MUKESAKI_NAME, ");
                selectQuery.Append(" TO_CHAR(ORG_QTY) ORG_QTY, ");
                selectQuery.Append(" QTY, ");
                selectQuery.Append(" RATE, ");
                selectQuery.Append(" PD.REMARKS, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN PD.ORG_ROLL_R IS NOT NULL ");
                selectQuery.Append(" AND PD.ORG_CHANCE IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(CONCAT(TO_CHAR(PD.ORG_ROLL_R, '99'), '-'), PD.ORG_CHANCE) ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END ORG_DELIVERYDATE, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN PD.ROLL_R IS NOT NULL ");
                selectQuery.Append(" AND PD.CHANCE IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(CONCAT(TO_CHAR(PD.ROLL_R, '99'), '-'), PD.CHANCE) ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END DELIVERYDATE, ");
                selectQuery.Append(" S.SUPPLIER_NAME, ");
                selectQuery.Append(" S.TELEPHONE_NO, ");
                selectQuery.Append(" S.FAX_NO, ");
                selectQuery.Append(" S.ADDRESS, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN S.SUPPLIER_NAME IS NOT NULL THEN ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN S.CONTACT_PERSON IS NOT NULL THEN ");
                selectQuery.Append(" TO_CHAR(S.SUPPLIER_NAME) || '、 ' || TO_CHAR(S.CONTACT_PERSON) || ' 殿' ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" TO_CHAR(S.SUPPLIER_NAME) || ' 殿' ");
                selectQuery.Append(" END ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" ' ' ");
                selectQuery.Append(" END AS SUPP_CONT_PERSON, ");
                selectQuery.Append("  ");
                selectQuery.Append(" E.SHORT_NAME AS EMPLOYEE_NAME, ");
                selectQuery.Append(" KOUBAN, ");
                selectQuery.Append(" ( ");
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" B.PO_DTL_NO ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_PO_DTL B ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" B.PO_DTL_CD = PD.ORG_PO_DTL_CD ");
                selectQuery.Append(" ) AS ORG_PO_DTL_NO, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN PD.MOCHIKOMISAKI_CD = ( ");
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" D.MOCHIKOMISAKI_CD ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_PO_DTL D ");
                selectQuery.Append(" INNER JOIN MST_MOCHIKOMISAKI M ON D.MOCHIKOMISAKI_CD = M.MOCHIKOMISAKI_CD ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" D.PO_DTL_CD = PD.ORG_PO_DTL_CD ");
                selectQuery.Append(" ) THEN ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" ( ");
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" M.MKSAKI_NAME ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_PO_DTL          D ");
                selectQuery.Append(" INNER JOIN MST_MOCHIKOMISAKI   M ON D.MOCHIKOMISAKI_CD = M.MOCHIKOMISAKI_CD ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" D.PO_DTL_CD = PD.ORG_PO_DTL_CD ");
                selectQuery.Append(" ) ");
                selectQuery.Append(" END ORI_MKSAKI_NAME, ");
                selectQuery.Append(" PH.REPORT_PAGE_NO, ");
                selectQuery.Append(" DECODE(PD.PO_KUBUN, 1, 1, 2, 2, 3, 3, 4, 2, 5, 3) AS REPORT_KUBUN_SORT_ORDER ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_PO_HDR           PH ");
                selectQuery.Append(" INNER JOIN TRN_PO_DTL           PD ON PD.PO_NO = PH.PO_NO ");
                selectQuery.Append(" INNER JOIN MST_SUPPLIER S ON PH.SUPPLIER_CD = S.SUPPLIER_CD ");
                selectQuery.Append(" INNER JOIN MST_EMPLOYEE E ON PH.EMPLOYEE_CD = E.EMPLOYEE_CD ");
                selectQuery.Append(" INNER JOIN V_HT_MATL_DETAILS MD ON PD.MATERIAL_CD = MD.MATERIAL_CD ");
                selectQuery.Append(" INNER JOIN MST_MAKER MAK ON MD.MAKER_CD = MAK.MAKER_CD ");
                selectQuery.Append(" INNER JOIN MST_MUKESAKI MUK ON PD.MUKESAKI_CD = MUK.MUKESAKI_CD ");
                selectQuery.Append(" INNER JOIN MST_MOCHIKOMISAKI MO ON PD.MOCHIKOMISAKI_CD = MO.MOCHIKOMISAKI_CD ");
                selectQuery.Append(" LEFT OUTER JOIN MST_CONDITION        CM ON MD.CONDITION_CD = CM.CONDITION_CD ");
                selectQuery.Append(" LEFT OUTER JOIN TRN_KOUBAN_HISTORY   TKH ON PD.KOUBAN_HISTORY_NO = TKH.KOUBAN_HISTORY_NO ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" PH.MONTHLY_PO_YEAR_R ='" + yearR + "' AND ");
                selectQuery.Append(" PH.IS_MONTHLY_PO = 1  ");
                selectQuery.Append(" ORDER BY ");
            //selectQuery.Append(" REPORT_KUBUN_SORT_ORDER, ");
            //selectQuery.Append(" MOCHIKOMISAKI_NAME, ");
                selectQuery.Append(" PO_NO,");
                selectQuery.Append(" PO_DTL_SRLNO,");
                selectQuery.Append(" MUKESAKI_NAME, ");
                selectQuery.Append(" YYYY, ");
                selectQuery.Append(" ROLL_R, ");
                selectQuery.Append(" MATL_MAKER_NAME, ");
                selectQuery.Append(" DIM_D, ");
                selectQuery.Append(" SHAPE, ");
                selectQuery.Append(" MATERIAL_LENGTH, ");
                selectQuery.Append(" CHANCE ");
                
            result = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(selectQuery.ToString());

            return result;
        }
        public void UpdatePrintFlag(List<FinalInspectionDetails> request)
        {
           
            if (request == null || request.Count == 0)
                return;

            var finalInspNos = request
                .Select(x => x.FINAL_INSP_NO)
                .Distinct()
                .ToList();

            if (!finalInspNos.Any())
                return;

            ExecuteNonQuery("ALTER TRIGGER HSKUSER_TRN_FI_TRIGGER DISABLE");

            StringBuilder query = new StringBuilder();

            query.Append("UPDATE TRN_FINAL_INSPECTION ");
            query.Append("SET PRINT_DATE = SYSDATE ");
            query.Append("WHERE EC_CD IS NULL ");
            query.Append("AND PRINT_DATE IS NULL ");
            query.Append("AND FINAL_INSP_CD IN (");
            query.Append(string.Join(",", finalInspNos));
            query.Append(")");
           

           ExecuteNonQuery(query.ToString());

           ExecuteNonQuery("ALTER TRIGGER HSKUSER_TRN_FI_TRIGGER ENABLE");
        }
        public List<RollPoMainAnalysisDetails> GetMalNotFoundList(int userId, int year, int R)
        {

            //List<RollPoMainAnalysisDetails> list = new List<RollPoMainAnalysisDetails>();
            List<RollPoMainAnalysisDetails> result = new List<RollPoMainAnalysisDetails>();
           
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" TO_CHAR(MNF.CREATED_DATE, 'yyyy/mm/dd HH24:MI') AS IMPORT_DATE, ");           
            
            selectQuery.Append(" MNF.PRODUCT_CD, ");
            selectQuery.Append(" CONCAT(CONCAT(PD.PROD_SPEC_NO1, '-'), PD.PROD_SPEC_NO2) AS PROD_SPEC_NO, ");
            selectQuery.Append(" EM.SHORT_NAME AS EMPLOYEE_NAME, ");
            selectQuery.Append(" CM.SHORT_NAME AS CUSTOMER_NAME, ");
            // --decode(pv.matl_spec_kubun, 1, '汎用', 2, '特別') AS genspec,
            selectQuery.Append(" PD.MATL_SPECIAL_KUBUN, ");             
            selectQuery.Append(" INTN.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" PD.MATL_NAME_MAKER, ");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST, ");
            selectQuery.Append(" PD.MATL_DIM_D, ");
            selectQuery.Append(" PD.DIM_D, ");
            selectQuery.Append(" PD.MATL_SHAPE, ");
            selectQuery.Append(" PD.SHAPE, ");
            selectQuery.Append(" PD.MATL_LENGTH AS MATERIAL_LENGTH, ");            
            selectQuery.Append(" NVL(PD.LENGTH_OTHER, PD.PRODUCT_LENGTH) AS PROD_LENGTH,");
            // selectQuery.Append(" CONCAT(NVL(PD.PRODUCT_LENGTH, ''), NVL(PD.LENGTH_OTHER, '')) AS PROD_LENGTH, ");
            selectQuery.Append(" MM.SHORT_NAME AS MAKER_NAME");
            //selectQuery.Append(" FROM ");
            //selectQuery.Append(" TMP_MATL_NOT_FOUND MNF ");
            //selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST TF ON MNF.PRODUCT_CD = TF.PRODUCT_CD ");
            //selectQuery.Append(" INNER JOIN MST_PRODUCT PD ON MNF.PRODUCT_CD = PD.PRODUCT_CD ");            
            //selectQuery.Append(" INNER JOIN MST_CUSTOMER CM ON TF.CUST_CD = CM.CUSTOMER_CD ");
            //selectQuery.Append(" INNER JOIN MST_EMPLOYEE EM ON TF.EMP_CD = EM.EMPLOYEE_CD ");
            //selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INTN ON PD.INTERNAL_NAME_CD = INTN.INTERNAL_NAME_CD ");
            //selectQuery.Append(" LEFT OUTER JOIN MST_MAKER  MM ON PD.MAKER_CD = MM.MAKER_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TMP_MATL_NOT_FOUND MNF");
            selectQuery.Append(" INNER JOIN MST_PRODUCT PD ON MNF.PRODUCT_CD = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CM ON CM.CUSTOMER_CD = PD.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG ON DG.DEPARTMENT_GROUP_CD = PD.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EM ON DG.EMPLOYEE_CD = EM.EMPLOYEE_CD ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INTN ON PD.INTERNAL_NAME_CD = INTN.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT OUTER JOIN MST_MAKER MM ON PD.MAKER_CD = MM.MAKER_CD ");
            selectQuery.Append(" WHERE ");            
            selectQuery.Append(" PD.VER = ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAX(VER) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_PRODUCT PD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" PD.ROOT_VER_PRODUCT_KEY = MNF.PRODUCT_CD ");
            selectQuery.Append(" ) ");
           // selectQuery.Append(" AND TF.ROLL_FC_YEAR = " + year );
           // selectQuery.Append(" AND TF.ROLL_FC_R = " + R);
            selectQuery.Append(" ORDER BY ");            
            selectQuery.Append(" INTERNAL_NAME_STRING, ");
            selectQuery.Append(" MATL_NAME_MAKER, ");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST, ");
            selectQuery.Append(" MATL_SHAPE, ");
            selectQuery.Append(" MATL_DIM_D, ");
            selectQuery.Append(" SHAPE, ");
            selectQuery.Append(" PD.DIM_D, ");
            selectQuery.Append(" MATL_LENGTH, ");
            selectQuery.Append(" PRODUCT_LENGTH ");
            result = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(selectQuery.ToString());

            
            return result;
        }
        public List<RollPoMainAnalysisDetails> GetMalNameIntWiseData(int year, int R)
        {
            List<RollPoMainAnalysisDetails> matlData = new List<RollPoMainAnalysisDetails>();
            //List<RollSalesForecastDetails> matlData = new List<RollSalesForecastDetails>();

            StringBuilder selectQuery = new StringBuilder();

            int dispatchR = 0;            

            if (R == 12)
            {
                dispatchR = 1;
            }
            else
            {
                dispatchR = R + 1;
            }
            DateTime dispatchDate = new DateTime(year, dispatchR, 1);

            DateTime resultMonth = dispatchDate.AddMonths(-2);

            int dispatchMonth1 = resultMonth.AddMonths(-1).Month;
            int dispatchMonth2 = resultMonth.AddMonths(-2).Month;
            int dispatchMonth3 = resultMonth.AddMonths(-3).Month;
            int dispatchMonth4 = resultMonth.AddMonths(-4).Month;

            int dispatchMonthYear1 = resultMonth.AddMonths(-1).Year;
            int dispatchMonthYear2 = resultMonth.AddMonths(-2).Year;
            int dispatchMonthYear3 = resultMonth.AddMonths(-3).Year;
            int dispatchMonthYear4 = resultMonth.AddMonths(-4).Year;


            selectQuery.Append(" SELECT ");
            selectQuery.Append(" * ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FC_SUMMARY_DATA.*, ");
            selectQuery.Append(" ROWNUM RN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" (SELECT ");
            selectQuery.Append(" 1 AS SRNO, ");
            selectQuery.Append(" MI.INTERNAL_NAME             AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" FCS.DIM_D                    AS DIM_D, ");
            selectQuery.Append(" FCS.SHAPE                    AS SHAPE, ");
            selectQuery.Append(" LENGTH, ");
            selectQuery.Append(" FCS.MATL_KUBUN               AS MATL_KUBUN, ");
            selectQuery.Append(" FCS.MATL_SPECIAL_KUBUN       AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MC.SHORT_NAME                AS CUSTOMER_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            selectQuery.Append(" MD.MATL_MAKER_NAME ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END MATL_MAKER_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            selectQuery.Append(" MD.MAKER_SHORT_NAME ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END MAKER_NAME, ");
            selectQuery.Append(" MD.CONDITION_DESCRIPTION     AS CONDITION_DESCRIPTION, ");
            selectQuery.Append(" ME.SHORT_NAME                AS EMPLOYEE_NAME, ");
            selectQuery.Append(" NULL AS PRODUCT_NAME_CUST, ");
            selectQuery.Append(" NULL AS PROD_DIM_D, ");
            selectQuery.Append(" NULL AS PROD_SHAPE, ");
            selectQuery.Append(" NULL AS PROD_LENGTH, ");
            selectQuery.Append(" NULL AS PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" NULL AS PROD_STOCK_QTY, ");
            selectQuery.Append(" NULL AS PROD_STOCK, ");
            selectQuery.Append(" NULL AS WIP_STOCK, ");
            selectQuery.Append(" NULL AS KAKOIRAI_REM_QTY, ");
            selectQuery.Append(" TSF.FC_QTY_R1, ");
            selectQuery.Append(" TSF.FC_QTY_R2, ");
            selectQuery.Append(" TSF.FC_QTY_R3, ");
            selectQuery.Append(" TSF.FC_QTY_R4, ");
            selectQuery.Append(" TSF.MSG_TO_MATL_DEPT, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY1, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY2, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY3, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY4, ");
            selectQuery.Append(" NULL AS SPECIFIED_MAKER,             ");
            selectQuery.Append(" MD.DIM_D                     AS MATL_DIM_D, ");
            selectQuery.Append(" MD.SHAPE                     AS MATL_SHAPE ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME          MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION              CON ON FCS.CONDITION_CD = CON.CONDITION_CD ");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST         TSF ON FCS.INT_MATL_FC_SUMMARY_CD = TSF.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" INNER JOIN V_HT_MATL_DETAILS          MD ON TSF.MATERIAL_CD = MD.MATERIAL_CD ");
            selectQuery.Append(" JOIN MST_EMPLOYEE               ME ON TSF.EMP_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append(" JOIN MST_CUSTOMER               MC ON TSF.CUST_CD = MC.CUSTOMER_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FCS.MATL_KUBUN IN( 1 ) ");
            selectQuery.Append(" AND TSF.ROLL_FC_KUBUN != 1 ");
            selectQuery.Append(" AND ");
            selectQuery.Append(" NOT( ");
            selectQuery.Append(" (TSF.FC_QTY_R1 = 0 OR TSF.FC_QTY_R1 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R2 = 0 OR TSF.FC_QTY_R2 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R3 = 0 OR TSF.FC_QTY_R3 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R4 = 0 OR TSF.FC_QTY_R4 IS NULL) ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" UNION ALL ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" 2 AS SRNO, ");
            selectQuery.Append(" MI.INTERNAL_NAME             AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" FCS.DIM_D                    AS DIM_D, ");
            selectQuery.Append(" FCS.SHAPE                    AS SHAPE, ");
            selectQuery.Append(" LENGTH, ");
            selectQuery.Append(" FCS.MATL_KUBUN               AS MATL_KUBUN, ");
            selectQuery.Append(" FCS.MATL_SPECIAL_KUBUN       AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MC.SHORT_NAME                AS CUSTOMER_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            selectQuery.Append(" MD.MATL_MAKER_NAME ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END MATL_MAKER_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            selectQuery.Append(" MD.MAKER_SHORT_NAME ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END MAKER_NAME, ");
            selectQuery.Append(" MD.CONDITION_DESCRIPTION     AS CONDITION_DESCRIPTION, ");
            selectQuery.Append(" ME.SHORT_NAME                AS EMPLOYEE_NAME, ");
            selectQuery.Append(" NULL AS PRODUCT_NAME_CUST, ");
            selectQuery.Append(" NULL AS PROD_DIM_D, ");
            selectQuery.Append(" NULL AS PROD_SHAPE, ");
            selectQuery.Append(" NULL AS PROD_LENGTH, ");
            selectQuery.Append(" NULL AS PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" NULL AS PROD_STOCK_QTY, ");
            selectQuery.Append(" NULL AS PROD_STOCK, ");
            selectQuery.Append(" NULL AS WIP_STOCK, ");
            selectQuery.Append(" NULL AS KAKOIRAI_REM_QTY, ");
            selectQuery.Append(" TSF.FC_QTY_R1, ");
            selectQuery.Append(" TSF.FC_QTY_R2, ");
            selectQuery.Append(" TSF.FC_QTY_R3, ");
            selectQuery.Append(" TSF.FC_QTY_R4, ");
            selectQuery.Append(" TSF.MSG_TO_MATL_DEPT, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY1, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY2, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY3, ");
            selectQuery.Append(" 0.0 DISPATCH_QTY4, ");
            selectQuery.Append(" NULL AS SPECIFIED_MAKER, ");
            selectQuery.Append(" MD.DIM_D                     AS MATL_DIM_D, ");
            selectQuery.Append(" MD.SHAPE                     AS MATL_SHAPE ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME          MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION              CON ON FCS.CONDITION_CD = CON.CONDITION_CD ");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST         TSF ON FCS.INT_MATL_FC_SUMMARY_CD = TSF.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" INNER JOIN V_HT_MATL_DETAILS          MD ON TSF.MATERIAL_CD = MD.MATERIAL_CD ");
            selectQuery.Append(" JOIN MST_EMPLOYEE               ME ON TSF.EMP_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append(" JOIN MST_CUSTOMER               MC ON TSF.CUST_CD = MC.CUSTOMER_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FCS.MATL_KUBUN <> 1 ");
            selectQuery.Append(" AND TSF.ROLL_FC_KUBUN != 1 ");
            selectQuery.Append(" AND ");
            selectQuery.Append(" NOT( ");
            selectQuery.Append(" (TSF.FC_QTY_R1 = 0 OR TSF.FC_QTY_R1 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R2 = 0 OR TSF.FC_QTY_R2 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R3 = 0 OR TSF.FC_QTY_R3 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R4 = 0 OR TSF.FC_QTY_R4 IS NULL) ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" ) ");
            //selectQuery.Append(" ORDER BY ");
            //selectQuery.Append(" SRNO, ");
            //selectQuery.Append(" INTERNAL_NAME_STRING, ");
            //selectQuery.Append(" DIM_D, ");
            //selectQuery.Append(" SHAPE, ");
            //selectQuery.Append(" LENGTH, ");
            //selectQuery.Append(" MATL_SPECIAL_KUBUN ");
            selectQuery.Append(" ) FC_SUMMARY_DATA ");
            selectQuery.Append(" ) ");
            selectQuery.Append("  ");
            selectQuery.Append(" UNION ALL ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  * ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FC_SUMMARY_DATA.*, ");
            selectQuery.Append(" ROWNUM RN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" (SELECT ");
            selectQuery.Append(" 1 AS SRNO, ");
            selectQuery.Append(" MI.INTERNAL_NAME             AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" FCS.DIM_D                    AS DIM_D, ");
            selectQuery.Append(" FCS.SHAPE                    AS SHAPE, ");
            selectQuery.Append(" LENGTH, ");
            selectQuery.Append(" FCS.MATL_KUBUN               AS MATL_KUBUN, ");
            selectQuery.Append(" FCS.MATL_SPECIAL_KUBUN       AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MC.SHORT_NAME                AS CUSTOMER_NAME, ");
            selectQuery.Append(" PD.MATL_NAME_MAKER           AS MATL_MAKER_NAME, ");
            selectQuery.Append(" PD.MAKER_NAME, ");
            selectQuery.Append(" C.CONDITION_DESCRIPTION      AS CONDITION_DESCRIPTION, ");
            selectQuery.Append(" ME.SHORT_NAME                EMPLOYEE_NAME, ");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST, ");
            selectQuery.Append(" PD.DIM_D                     AS PROD_DIM_D, ");
            selectQuery.Append(" PD.SHAPE                   AS PROD_SHAPE, ");
            selectQuery.Append(" PD.PRODUCT_LENGTH            AS PROD_LENGTH, ");
            selectQuery.Append(" PD.CONDITION_DESCRIPTION     AS PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PRODSTOCK.PROD_STOCK_QTY, ");
            selectQuery.Append(" TSF.PROD_STOCK, ");
            selectQuery.Append(" TSF.WIP_STOCK, ");
            selectQuery.Append(" BKI.KAKOIRAI_REM_QTY / 1000 AS KAKOIRAI_REM_QTY, ");
            selectQuery.Append(" TSF.FC_QTY_R1, ");
            selectQuery.Append(" TSF.FC_QTY_R2, ");
            selectQuery.Append(" TSF.FC_QTY_R3, ");
            selectQuery.Append(" TSF.FC_QTY_R4, ");
            selectQuery.Append(" TSF.MSG_TO_MATL_DEPT, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY1 AS DISPATCH_QTY1, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY2 AS DISPATCH_QTY2, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY3 AS DISPATCH_QTY3, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY4 AS DISPATCH_QTY4, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PD.MAKER_SPEC_KUBUN = 3 THEN ");
            selectQuery.Append(" TO_NCHAR('') ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" PD.MAKER_NAME ");
            selectQuery.Append(" END AS SPECIFIED_MAKER, ");
            selectQuery.Append(" PD.MATL_DIM_D                AS MATL_DIM_D, ");
            selectQuery.Append(" PD.MATL_SHAPE                AS MATL_SHAPE ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME          MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST         TSF ON FCS.INT_MATL_FC_SUMMARY_CD = TSF.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" INNER JOIN V_PRODUCT_DETAILS          PD ON TSF.PRODUCT_CD = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER               MC ON TSF.CUST_CD = MC.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE               ME ON TSF.EMP_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION              C ON PD.MATL_CONDITION_CD = C.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN BCH_KAKOIRAI_INFO   BKI ON PD.ROOT_VER_PRODUCT_KEY = BKI.PROD_SPEC_CD ");
            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY AS ROOT_VER_PRODUCT_KEY1, ");
            selectQuery.Append(" SUM(BPS.CURR_STOCK) / 1000 PROD_STOCK_QTY, ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_PROD_STOCK_HOST BPS ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) PRODSTOCK ON PRODSTOCK.ROOT_VER_PRODUCT_KEY1 = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth4 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear4 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY1, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth3 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear3 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY2, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth2 +"' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear2 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY3, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth1 +"' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear1 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY4 ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_MONTH_END_SHIPMENT ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) DISPATCH ON DISPATCH.ROOT_VER_PRODUCT_KEY = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FCS.MATL_KUBUN IN( 1 ) ");
            selectQuery.Append(" AND PD.VERSION = ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAX(VERSION) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" V_PRODUCT_DETAILS ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" PD.ROOT_VER_PRODUCT_KEY = ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" AND TSF.ROLL_FC_KUBUN = 1 ");
            selectQuery.Append(" AND ");
            selectQuery.Append(" NOT( ");
            selectQuery.Append(" (TSF.FC_QTY_R1 = 0 OR TSF.FC_QTY_R1 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R2 = 0 OR TSF.FC_QTY_R2 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R3 = 0 OR TSF.FC_QTY_R3 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R4 = 0 OR TSF.FC_QTY_R4 IS NULL) ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" UNION ALL ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" 2 AS SRNO, ");
            selectQuery.Append(" MI.INTERNAL_NAME             AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" FCS.DIM_D                    AS DIM_D, ");
            selectQuery.Append(" FCS.SHAPE                    AS SHAPE, ");
            selectQuery.Append(" LENGTH, ");
            selectQuery.Append(" FCS.MATL_KUBUN               AS MATL_KUBUN, ");
            selectQuery.Append(" FCS.MATL_SPECIAL_KUBUN       AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MC.SHORT_NAME                AS CUSTOMER_NAME, ");
            selectQuery.Append(" PD.MATL_NAME_MAKER           AS MATL_MAKER_NAME, ");
            selectQuery.Append(" PD.MAKER_NAME, ");
            selectQuery.Append(" C.CONDITION_DESCRIPTION      AS CONDITION_DESCRIPTION, ");
            selectQuery.Append(" ME.SHORT_NAME                EMPLOYEE_NAME, ");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST, ");
            selectQuery.Append(" PD.DIM_D                     AS PROD_DIM_D, ");
            selectQuery.Append(" PD.SHAPE                   AS PROD_SHAPE, ");
            selectQuery.Append(" PD.PRODUCT_LENGTH            AS PROD_LENGTH, ");
            selectQuery.Append(" PD.CONDITION_DESCRIPTION     AS PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PRODSTOCK.PROD_STOCK_QTY, ");
            selectQuery.Append(" TSF.PROD_STOCK, ");
            selectQuery.Append(" TSF.WIP_STOCK, ");
            selectQuery.Append(" BKI.KAKOIRAI_REM_QTY / 1000 AS KAKOIRAI_REM_QTY, ");
            selectQuery.Append(" TSF.FC_QTY_R1, ");
            selectQuery.Append(" TSF.FC_QTY_R2, ");
            selectQuery.Append(" TSF.FC_QTY_R3, ");
            selectQuery.Append(" TSF.FC_QTY_R4, ");
            selectQuery.Append(" TSF.MSG_TO_MATL_DEPT, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY1 AS DISPATCH_QTY1, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY2 AS DISPATCH_QTY2, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY3 AS DISPATCH_QTY3, ");
            selectQuery.Append(" DISPATCH.DISPATCH_QTY4 AS DISPATCH_QTY4, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PD.MAKER_SPEC_KUBUN = 3 THEN ");
            selectQuery.Append(" TO_NCHAR('') ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" PD.MAKER_NAME ");
            selectQuery.Append(" END AS SPECIFIED_MAKER, ");
            selectQuery.Append(" PD.MATL_DIM_D                AS MATL_DIM_D, ");
            selectQuery.Append(" PD.MATL_SHAPE                AS MATL_SHAPE ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME          MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST         TSF ON FCS.INT_MATL_FC_SUMMARY_CD = TSF.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" INNER JOIN V_PRODUCT_DETAILS          PD ON TSF.PRODUCT_CD = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER               MC ON TSF.CUST_CD = MC.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE               ME ON TSF.EMP_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION              C ON PD.MATL_CONDITION_CD = C.CONDITION_CD ");
            selectQuery.Append(" LEFT  JOIN BCH_KAKOIRAI_INFO   BKI ON PD.ROOT_VER_PRODUCT_KEY = BKI.PROD_SPEC_CD ");
            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY AS ROOT_VER_PRODUCT_KEY1, ");
            selectQuery.Append(" SUM(BPS.CURR_STOCK) / 1000 PROD_STOCK_QTY, ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_PROD_STOCK_HOST BPS ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) PRODSTOCK ON PRODSTOCK.ROOT_VER_PRODUCT_KEY1 = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth4 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear4 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY1, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth3 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear3 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY2, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth2 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear2 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY3, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth1 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear1 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DISPATCH_QTY4 ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_MONTH_END_SHIPMENT ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) DISPATCH ON DISPATCH.ROOT_VER_PRODUCT_KEY = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FCS.MATL_KUBUN <> 1 ");
            selectQuery.Append(" AND PD.VERSION = ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAX(VERSION) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" V_PRODUCT_DETAILS ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" PD.ROOT_VER_PRODUCT_KEY = ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" AND TSF.ROLL_FC_KUBUN = 1 ");
            selectQuery.Append(" AND ");
            selectQuery.Append(" NOT( ");
            selectQuery.Append(" (TSF.FC_QTY_R1 = 0 OR TSF.FC_QTY_R1 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R2 = 0 OR TSF.FC_QTY_R2 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R3 = 0 OR TSF.FC_QTY_R3 IS NULL) AND ");
            selectQuery.Append(" (TSF.FC_QTY_R4 = 0 OR TSF.FC_QTY_R4 IS NULL) ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" ) ");
           
            selectQuery.Append(" ) FC_SUMMARY_DATA ");
            selectQuery.Append(" ) ) ");
            selectQuery.Append(" ORDER BY ");
            selectQuery.Append(" SRNO, ");
            selectQuery.Append(" INTERNAL_NAME_STRING, ");
            selectQuery.Append(" DIM_D, ");
            selectQuery.Append(" SHAPE, ");
            selectQuery.Append(" LENGTH, ");
            selectQuery.Append(" MATL_SPECIAL_KUBUN ");
            matlData = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(selectQuery.ToString());


            // selectMatlQuery.Append(" SELECT ");
            // selectMatlQuery.Append(" * ");
            // selectMatlQuery.Append(" FROM ");
            // selectMatlQuery.Append(" ( ");
            // selectMatlQuery.Append(" SELECT ");
            // selectMatlQuery.Append(" FC_SUMMARY_DATA.*, ");
            // selectMatlQuery.Append(" ROWNUM RN ");
            // selectMatlQuery.Append(" FROM ");
            // selectMatlQuery.Append(" ( ");
            // selectMatlQuery.Append(" (SELECT ");
            // selectMatlQuery.Append(" 1 AS SRNO, ");
            // selectMatlQuery.Append(" MI.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            // selectMatlQuery.Append(" FCS.DIM_D AS DIM_D, ");
            // selectMatlQuery.Append(" FCS.SHAPE AS SHAPE, ");
            // selectMatlQuery.Append(" LENGTH, ");
            // selectMatlQuery.Append(" FCS.MATL_KUBUN AS MATL_KUBUN, ");
            // selectMatlQuery.Append(" FCS.MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");

            // selectMatlQuery.Append(" FCS.EMPLOYEE_CD, ");

            // selectMatlQuery.Append(" FCS.INT_MATL_FC_SUMMARY_CD AS FCS_INT_MATL_FC_SUMMARY_CD, ");

            // selectMatlQuery.Append(" TSF.ROLL_FC_KUBUN, ");
            // selectMatlQuery.Append(" CASE ");
            // selectMatlQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            // selectMatlQuery.Append(" MD.MATL_MAKER_NAME ");
            // selectMatlQuery.Append(" ELSE ");
            // selectMatlQuery.Append(" NULL ");
            // selectMatlQuery.Append(" END MATL_MAKER_NAME, ");
            // selectMatlQuery.Append(" MD.MATL_KUBUN AS KUBUN, ");
            // selectMatlQuery.Append(" MD.DIM_D        AS MATL_DIM_D, ");
            // selectMatlQuery.Append(" MD.SHAPE AS MATL_SHAPE, ");
            // selectMatlQuery.Append(" MD.DIM_W, ");
            // selectMatlQuery.Append(" MD.DIM_OTHER, ");
            // selectMatlQuery.Append(" MD.MATERIAL_LENGTH, ");
            // selectMatlQuery.Append(" 0 AS MATL_STOCK_QTY, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R1, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R2, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R3, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R4, ");
            // selectMatlQuery.Append(" MD.MATERIAL_CD, ");
            // selectMatlQuery.Append(" MD.INTERNAL_NAME_CD, ");
            // selectMatlQuery.Append(" CASE ");
            // selectMatlQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            // selectMatlQuery.Append(" MD.MAKER_SHORT_NAME ");
            // selectMatlQuery.Append(" ELSE ");
            // selectMatlQuery.Append(" NULL ");
            // selectMatlQuery.Append(" END MAKER_NAME, ");
            // selectMatlQuery.Append(" MD.MATL_SPECIAL_KUBUN AS MATL_SPEC_KUBUN, ");
            // selectMatlQuery.Append(" MD.CONDITION_DESCRIPTION AS CONDITION_DESCRIPTION, ");
            // selectMatlQuery.Append(" MD.MOCHIKOMISAKI_CD, ");
            // selectMatlQuery.Append(" CASE ");
            // selectMatlQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            // selectMatlQuery.Append(" MD.MKSAKI_NAME ");
            // selectMatlQuery.Append(" ELSE ");
            // selectMatlQuery.Append(" NULL ");
            // selectMatlQuery.Append(" END MKSAKI_NAME, ");
            // selectMatlQuery.Append(" 0 AS ORDER_REM_QTY, ");
            // selectMatlQuery.Append(" TSF.MSG_TO_MATL_DEPT, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_NO, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_SRLNO, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_CD, ");
            // selectMatlQuery.Append(" 0 AS IS_LAST_R_REC, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_R, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_YEAR, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_DATE, ");
            // selectMatlQuery.Append(" TSF.EMP_CD, ");
            // selectMatlQuery.Append(" TSF.CUST_CD, ");
            // selectMatlQuery.Append(" MD.CONDITION_CD, ");
            // selectMatlQuery.Append(" ME.SHORT_NAME   AS EMPLOYEE_NAME, ");
            // selectMatlQuery.Append(" TSF.REPLY_FROM_MATL_DEPT, ");
            // selectMatlQuery.Append(" MC.SHORT_NAME   AS CUSTOMER_NAME, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY1, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY2, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY3, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY4, ");
            // selectMatlQuery.Append(" TSF.INT_MATL_FC_SUMMARY_CD AS TSF_INT_MATL_FC_SUMMARY_CD ");
            // selectMatlQuery.Append(" FROM ");
            // selectMatlQuery.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS ");
            // selectMatlQuery.Append(" INNER JOIN MST_INTERNAL_NAME          MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            // selectMatlQuery.Append(" LEFT JOIN MST_CONDITION               CON ON FCS.CONDITION_CD = CON.CONDITION_CD ");
            // selectMatlQuery.Append(" INNER JOIN  TRN_SALES_FORECAST        TSF ON FCS.INT_MATL_FC_SUMMARY_CD = TSF.INT_MATL_FC_SUMMARY_CD ");
            // selectMatlQuery.Append(" INNER JOIN V_HT_MATL_DETAILS          MD ON TSF.MATERIAL_CD = MD.MATERIAL_CD ");
            // selectMatlQuery.Append(" JOIN MST_EMPLOYEE                     ME ON TSF.EMP_CD = ME.EMPLOYEE_CD ");
            // selectMatlQuery.Append(" JOIN MST_CUSTOMER                     MC ON TSF.CUST_CD = MC.CUSTOMER_CD ");
            // selectMatlQuery.Append(" WHERE ");
            // selectMatlQuery.Append(" FCS.MATL_KUBUN IN( ");
            // selectMatlQuery.Append("     1 ");
            // selectMatlQuery.Append(" ) ");
            // selectMatlQuery.Append(" AND TSF.ROLL_FC_KUBUN != 1");
            // selectMatlQuery.Append(" UNION ALL ");
            // selectMatlQuery.Append(" SELECT ");
            // selectMatlQuery.Append(" 2 AS SRNO, ");
            // selectMatlQuery.Append(" MI.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            // selectMatlQuery.Append(" FCS.DIM_D AS DIM_D, ");
            // selectMatlQuery.Append(" FCS.SHAPE AS SHAPE, ");
            // selectMatlQuery.Append(" LENGTH, ");
            // selectMatlQuery.Append(" FCS.MATL_KUBUN AS MATL_KUBUN, ");
            // selectMatlQuery.Append(" FCS.MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");

            // selectMatlQuery.Append(" FCS.EMPLOYEE_CD, ");

            // selectMatlQuery.Append(" FCS.INT_MATL_FC_SUMMARY_CD AS FCS_INT_MATL_FC_SUMMARY_CD, ");

            // selectMatlQuery.Append(" TSF.ROLL_FC_KUBUN, ");
            // selectMatlQuery.Append(" CASE ");
            // selectMatlQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            // selectMatlQuery.Append(" MD.MATL_MAKER_NAME ");
            // selectMatlQuery.Append(" ELSE ");
            // selectMatlQuery.Append(" NULL ");
            // selectMatlQuery.Append(" END MATL_MAKER_NAME, ");
            // selectMatlQuery.Append(" MD.MATL_KUBUN AS KUBUN, ");
            // selectMatlQuery.Append(" MD.DIM_D        AS MATL_DIM_D, ");
            // selectMatlQuery.Append(" MD.SHAPE AS MATL_SHAPE, ");
            // selectMatlQuery.Append(" MD.DIM_W, ");
            // selectMatlQuery.Append(" MD.DIM_OTHER, ");
            // selectMatlQuery.Append(" MD.MATERIAL_LENGTH, ");
            // selectMatlQuery.Append(" 0 AS MATL_STOCK_QTY, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R1, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R2, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R3, ");
            // selectMatlQuery.Append(" TSF.FC_QTY_R4, ");
            // selectMatlQuery.Append(" MD.MATERIAL_CD, ");
            // selectMatlQuery.Append(" MD.INTERNAL_NAME_CD, ");
            // selectMatlQuery.Append(" CASE ");
            // selectMatlQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            // selectMatlQuery.Append(" MD.MAKER_SHORT_NAME ");
            // selectMatlQuery.Append(" ELSE ");
            // selectMatlQuery.Append(" NULL ");
            // selectMatlQuery.Append(" END MAKER_NAME, ");
            // selectMatlQuery.Append(" MD.MATL_SPECIAL_KUBUN AS MATL_SPEC_KUBUN, ");
            // selectMatlQuery.Append(" MD.CONDITION_DESCRIPTION AS CONDITION_DESCRIPTION, ");
            // selectMatlQuery.Append(" MD.MOCHIKOMISAKI_CD, ");
            // selectMatlQuery.Append(" CASE ");
            // selectMatlQuery.Append(" WHEN TSF.ROLL_FC_KUBUN = 2 THEN ");
            // selectMatlQuery.Append(" MD.MKSAKI_NAME ");
            // selectMatlQuery.Append(" ELSE ");
            // selectMatlQuery.Append(" NULL ");
            // selectMatlQuery.Append(" END MKSAKI_NAME, ");
            // selectMatlQuery.Append(" 0 AS ORDER_REM_QTY, ");
            // selectMatlQuery.Append(" TSF.MSG_TO_MATL_DEPT, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_NO, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_SRLNO, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_CD, ");
            // selectMatlQuery.Append(" 0 AS IS_LAST_R_REC, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_R, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_YEAR, ");
            // selectMatlQuery.Append(" TSF.ROLL_FC_DATE, ");
            // selectMatlQuery.Append(" TSF.EMP_CD, ");
            // selectMatlQuery.Append(" TSF.CUST_CD, ");
            // selectMatlQuery.Append(" MD.CONDITION_CD, ");
            // selectMatlQuery.Append(" ME.SHORT_NAME   AS EMPLOYEE_NAME, ");
            // selectMatlQuery.Append(" TSF.REPLY_FROM_MATL_DEPT, ");
            // selectMatlQuery.Append(" MC.SHORT_NAME   AS CUSTOMER_NAME, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY1, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY2, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY3, ");
            // selectMatlQuery.Append(" 0.0 DISPACH_QTY4, ");
            //// selectMatlQuery.Append(" 0.0 DISPACH_QTY_AVERAGE");
            // selectMatlQuery.Append(" TSF.INT_MATL_FC_SUMMARY_CD AS TSF_INT_MATL_FC_SUMMARY_CD ");
            // selectMatlQuery.Append(" FROM ");
            // selectMatlQuery.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS ");
            // selectMatlQuery.Append(" INNER JOIN MST_INTERNAL_NAME          MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            // selectMatlQuery.Append(" LEFT JOIN MST_CONDITION              CON ON FCS.CONDITION_CD = CON.CONDITION_CD ");
            // selectMatlQuery.Append(" INNER JOIN  TRN_SALES_FORECAST   TSF ON FCS.INT_MATL_FC_SUMMARY_CD = TSF.INT_MATL_FC_SUMMARY_CD ");
            // selectMatlQuery.Append(" INNER JOIN V_HT_MATL_DETAILS    MD ON TSF.MATERIAL_CD = MD.MATERIAL_CD ");
            // selectMatlQuery.Append(" JOIN MST_EMPLOYEE ME ON TSF.EMP_CD = ME.EMPLOYEE_CD ");
            // selectMatlQuery.Append(" JOIN MST_CUSTOMER MC ON TSF.CUST_CD = MC.CUSTOMER_CD ");
            // selectMatlQuery.Append(" WHERE ");
            // selectMatlQuery.Append(" FCS.MATL_KUBUN IN( ");
            // selectMatlQuery.Append("  2, ");
            // selectMatlQuery.Append("  3 ");
            // selectMatlQuery.Append(" ) ");
            // selectMatlQuery.Append(" AND TSF.ROLL_FC_KUBUN != 1");
            // selectMatlQuery.Append(" ) ");
            // selectMatlQuery.Append(" ORDER BY ");
            // selectMatlQuery.Append(" SRNO, ");
            // selectMatlQuery.Append(" INTERNAL_NAME_STRING, ");
            // selectMatlQuery.Append(" DIM_D, ");
            // selectMatlQuery.Append(" SHAPE, ");
            // selectMatlQuery.Append(" LENGTH, ");
            // selectMatlQuery.Append(" MATL_SPECIAL_KUBUN ");
            // selectMatlQuery.Append(" ) FC_SUMMARY_DATA ");
            // selectMatlQuery.Append(" ) ");
            //matlData = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(selectMatlQuery.ToString());

            //foreach (var matlnameIntWise in matlnameIntWiseData)
            //{
            //    if (matlnameIntWise.PO_REVIEWED_DATE == null)
            //    {
            //        continue;
            //    }
            //    long intMatlFcSummaryCd = matlnameIntWise.INT_MATL_FC_SUMMARY_CD;

            //    foreach (var matl in matlData)
            //    {
            //        if (matl.INT_MATL_FC_SUMMARY_CD != null && intMatlFcSummaryCd == matl.INT_MATL_FC_SUMMARY_CD)
            //        {


            //        }

            //    }

            //}

            //tempProdAndMatlData = detailDataDao.Dataset;

            //// For each row on screen 3 fetch product and material data
            //for (int matlNameIntRow = 0; matlNameIntRow < matlNameIntWiseData.Tables[0].Rows.Count;
            //     matlNameIntRow++)
            //{
            //    // Skip records where forecast does not exist
            //    if (Convert.ToInt32(
            //        matlNameIntWiseData.Tables[0].Rows[matlNameIntRow][PO_ANALYSIS_MAIN.FC_EXISTS_FLAG]) == 0)
            //    {
            //        continue;
            //    }

            //    totalQtyRow = null;
            //    tempDataRow = null;

            //    productData = new DataTable();
            //    materialData = new DataTable();

            //    headerDataRow = matlNameIntWiseData.Tables[0].Rows[matlNameIntRow];

            //    detailDataDao.FcSummaryNo =
            //        matlNameIntWiseData.Tables[0].Rows[matlNameIntRow][TMP_PO_FC.FC_SUMMARY_NO];

            //    fcSummaryNo =
            //        matlNameIntWiseData.Tables[0].Rows[matlNameIntRow][TMP_PO_FC.FC_SUMMARY_NO];



            //    // MATERIAL DATA
            //    for (int materialRows = 0;
            //         materialRows < tempProdAndMatlData.Tables[2].Rows.Count;
            //         materialRows++)
            //    {
            //        if (tempProdAndMatlData.Tables[2].Rows[materialRows][TMP_PO_FC.FC_SUMMARY_NO] != DBNull.Value &&
            //            fcSummaryNo.Equals(
            //                tempProdAndMatlData.Tables[2].Rows[materialRows][TMP_PO_FC.FC_SUMMARY_NO]))
            //        {
            //            tempDataRow = reportData.Tables[0].NewRow();
            //            matlDataRow = tempProdAndMatlData.Tables[2].Rows[materialRows];

            //            InsertHeaderRow(tempDataRow, headerDataRow);
            //            InsertMaterialRow(tempDataRow, matlDataRow);

            //            if (!(Convert.ToDecimal(tempDataRow[MATL_FORECAST_DETAIL_ANALYSIS.R_1]) == 0 &&
            //                  Convert.ToDecimal(tempDataRow[MATL_FORECAST_DETAIL_ANALYSIS.R_2]) == 0 &&
            //                  Convert.ToDecimal(tempDataRow[MATL_FORECAST_DETAIL_ANALYSIS.R_3]) == 0 &&
            //                  Convert.ToDecimal(tempDataRow[MATL_FORECAST_DETAIL_ANALYSIS.R_4]) == 0))
            //            {
            //                reportData.Tables[0].Rows.Add(tempDataRow);
            //            }
            //        }
            //    }
            //}

            return matlData;
        }

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" MAKER_MILL_SHEET_LOG_CD, FILENAME, MAKER_NAME, MATL_NAME_MAKER, CHARGE, ");
            sb.Append(" DIM_D AS DIM_D ,");
            sb.Append(" LENGTH, NO_OF_UNITS, QUANTITY,  HARDNESS_STD, HARDNESS_MIN , ");
            sb.Append("(CASE   WHEN GRAIN_SIZE_STD IS NULL   THEN NULL WHEN  HSKWEBUSER.IS_NUMBER(GRAIN_SIZE_STD) = 1 THEN   TO_CHAR (GRAIN_SIZE_STD, '90.0')  ELSE   GRAIN_SIZE_STD   END  ) AS GRAIN_SIZE_STD , ");
            sb.Append(" (CASE   WHEN GRAIN_SIZE_ACTUAL IS NULL   THEN NULL WHEN  HSKWEBUSER.IS_NUMBER(GRAIN_SIZE_ACTUAL) = 1 THEN   TO_CHAR (GRAIN_SIZE_ACTUAL, '90.0')  ELSE   GRAIN_SIZE_ACTUAL   END  ) AS GRAIN_SIZE_ACTUAL, ");
            sb.Append(" HARDNESS_MAX, HARDNESS_ACTUAL, CCNAME1, 	");
            sb.Append(" (case when CCRANGE1 is null then null else '×'||CCRANGE1 end ) as CCRANGE1 , CCMIN1, CCMAX1,	");
            sb.Append("	      CCACTUAL1 	");
            sb.Append(" , CCNAME2, 	");
            sb.Append("  (case when CCRANGE2 is null then null else '×'||CCRANGE2 end ) as CCRANGE2, 	");
            sb.Append(" CCMIN2, CCMAX2, CCACTUAL2, CCNAME3,	");
            sb.Append("  (case when CCRANGE3 is null then null else '×'||CCRANGE3 end ) as CCRANGE3,	");
            sb.Append("  CCMIN3, CCMAX3, CCACTUAL3, CCNAME4, 	");
            sb.Append("  (case when CCRANGE4 is null then null else '×'||CCRANGE4 end ) as CCRANGE4,	");
            sb.Append("   CCMIN4, CCMAX4,	");
            sb.Append(" CCACTUAL4, CCNAME5, 	");
            sb.Append(" (case when CCRANGE5 is null then null else '×'||CCRANGE5 end ) as CCRANGE5 ,	");
            sb.Append("  CCMIN5, CCMAX5, CCACTUAL5, CCNAME6,	");
            sb.Append(" (case when CCRANGE6 is null then null else '×'||CCRANGE6 end ) as CCRANGE6 , 	");
            sb.Append(" CCMIN6, CCMAX6, CCACTUAL6, CCNAME7, 	");
            sb.Append("  (case when CCRANGE7 is null then null else '×'||CCRANGE7 end ) as CCRANGE7,	");
            sb.Append("  CCMIN7, CCMAX7,       CCACTUAL7, CCNAME8,	");
            sb.Append(" (case when CCRANGE8 is null then null else '×'||CCRANGE8 end ) as CCRANGE8,       	");
            sb.Append("  CCMIN8, CCMAX8, CCACTUAL8, CCNAME9,	");
            sb.Append(" (case when CCRANGE9 is null then null else '×'||CCRANGE9 end ) as CCRANGE9,	");
            sb.Append(" CCMIN9, CCMAX9, CCACTUAL9, CCNAME10, CCMIN10,	");
            sb.Append(" CCMAX10, CCACTUAL10, CCNAME11, 	");
            sb.Append(" (case when CCRANGE10 is null then null else '×'||CCRANGE10 end ) as CCRANGE10,	");
            sb.Append("	      (case when CCRANGE11 is null then null else '×'||CCRANGE11 end ) as CCRANGE11,	");
            sb.Append(" CCMIN11, CCMAX11, CCACTUAL11,       CCNAME12, 	");
            sb.Append(" (case when CCRANGE12 is null then null else '×'||CCRANGE12 end ) as CCRANGE12, 	");
            sb.Append(" CCMIN12, CCMAX12, CCACTUAL12, CCNAME13, 	");
            sb.Append(" (case when CCRANGE13 is null then null else '×'||CCRANGE13 end ) as CCRANGE13,	");
            sb.Append(" CCMIN13, CCMAX13, CCACTUAL13, CCNAME14, 	");
            sb.Append(" (case when CCRANGE14 is null then null else '×'||CCRANGE14 end ) as CCRANGE14,	");
            sb.Append(" CCMIN14, CCMAX14,	");
            sb.Append(" CCACTUAL14, CCNAME15, 	");
            sb.Append(" (case when CCRANGE15 is null then null else '×'||CCRANGE15 end ) as CCRANGE15,	");
            sb.Append("  CCMIN15, CCMAX15, CCACTUAL15,	");
            sb.Append(" CCNAME16, 	");
            sb.Append(" (case when CCRANGE16 is null then null else '×'||CCRANGE16 end ) as CCRANGE16,	");
            sb.Append("  CCMIN16, CCMAX16, CCACTUAL16, CCNAME17, 	");
            sb.Append("	      (case when CCRANGE17 is null then null else '×'||CCRANGE17 end ) as CCRANGE17,	");
            sb.Append(" CCMIN17, CCMAX17, CCACTUAL17, CCNAME18, 	");
            sb.Append("	      (case when CCRANGE18 is null then null else '×'||CCRANGE18 end ) as CCRANGE18,	");
            sb.Append(" CCMIN18, CCMAX18,	");
            sb.Append(" CCACTUAL18, CCNAME19, 	");
            sb.Append(" (case when CCRANGE19 is null then null else '×'||CCRANGE19 end ) as CCRANGE19,	");
            sb.Append(" CCMIN19, CCMAX19, CCACTUAL19,	");
            sb.Append(" CCNAME20,	");
            sb.Append("  (case when CCRANGE20 is null then null else '×'||CCRANGE20 end ) as CCRANGE20,	");
            sb.Append("   CCMIN20, CCMAX20, CCACTUAL20, CCNAME21, 	");
            sb.Append("   (case when CCRANGE21 is null then null else '×'||CCRANGE21 end ) as CCRANGE21,	");
            sb.Append(" CCMIN21, CCMAX21, CCACTUAL21, CCNAME22, CCRANGE22, CCMIN22, CCMAX22,	");
            sb.Append(" CCACTUAL22, CCNAME23, 	");
            sb.Append("	      (case when CCRANGE23 is null then null else '×'||CCRANGE23 end ) as CCRANGE23,	");
            sb.Append(" CCMIN23, CCMAX23, CCACTUAL23,	");
            sb.Append(" CCNAME24, 	");
            sb.Append(" (case when CCRANGE24 is null then null else '×'||CCRANGE24 end ) as CCRANGE24,	");
            sb.Append(" CCMIN24, CCMAX24, CCACTUAL24, CCNAME25, 	");
            sb.Append(" (case when CCRANGE25 is null then null else '×'||CCRANGE25 end ) as CCRANGE25,	");
            sb.Append(" CCMIN25, CCMAX25, CCACTUAL25, HARD_STD1, HARD_MIN1, HARD_MAX1,	");
            sb.Append(" HARD_ACTUAL1, HARD_STD2, HARD_MIN2, HARD_MAX2, HARD_ACTUAL2, HARD_STD3,	");
            sb.Append(" HARD_MIN3, HARD_MAX3, HARD_ACTUAL3, HARD_STD4, HARD_MIN4, HARD_MAX4,	");
            sb.Append(" HARD_ACTUAL4, HARD_STD5, HARD_MIN5, HARD_MAX5, HARD_ACTUAL5, HARD_STD6,	");
            sb.Append(" HARD_MIN6, HARD_MAX6, HARD_ACTUAL6, HARD_STD7, HARD_MIN7, HARD_MAX7,	");
            sb.Append(" HARD_ACTUAL7, HARD_STD8, HARD_MIN8, HARD_MAX8, HARD_ACTUAL8, HARD_STD9,	");
            sb.Append(" HARD_MIN9, HARD_MAX9, HARD_ACTUAL9, HARD_STD10, HARD_MIN10, HARD_MAX10,	");
            sb.Append(" HARD_ACTUAL10, HARD_STD11, HARD_MIN11, HARD_MAX11, HARD_ACTUAL11,	");
            sb.Append(" HARD_STD12, HARD_MIN12, HARD_MAX12, HARD_ACTUAL12, HARD_STD13,	");
            sb.Append(" HARD_MIN13, HARD_MAX13, HARD_ACTUAL13, HARD_STD14, HARD_MIN14,	");
            sb.Append(" HARD_MAX14, HARD_ACTUAL14, HARD_STD15, HARD_MIN15, HARD_MAX15,	");
            sb.Append(" HARD_ACTUAL15, HARD_STD16, HARD_MIN16, HARD_MAX16, HARD_ACTUAL16,	");
            sb.Append(" HARD_STD17, HARD_MIN17, HARD_MAX17, HARD_ACTUAL17, HARD_STD18,	");
            sb.Append(" HARD_MIN18, HARD_MAX18, HARD_ACTUAL18, HARD_STD19, HARD_MIN19,	");
            sb.Append(" HARD_MAX19, HARD_ACTUAL19, HARD_STD20, HARD_MIN20, HARD_MAX20,	");
            sb.Append(" HARD_ACTUAL20, HARD_STD21, HARD_MIN21, HARD_MAX21, HARD_ACTUAL21,	");
            sb.Append(" HARD_STD22, HARD_MIN22, HARD_MAX22, HARD_ACTUAL22, HARD_STD23,	");
            sb.Append(" HARD_MIN23, HARD_MAX23, HARD_ACTUAL23, HARD_STD24, HARD_MIN24,	");
            sb.Append(" HARD_MAX24, HARD_ACTUAL24, HARD_STD25, HARD_MIN25, HARD_MAX25,	");
            sb.Append(" HARD_ACTUAL25, DUMMY, RUN_CD, STATUS, STATUS_MARK, MESSAGE,	");
            sb.Append(" TO_CHAR(CREATED_DATE,'yyyy/mm/dd hh24:mi:ss') AS CREATED_DATE ");
            sb.Append(" FROM  ");
            sb.Append(" MAKER_MILL_SHEET_LOG ");
            sb.Append(" WHERE ");
            sb.Append(" RUN_CD = " + runID);
            sb.Append(" AND STATUS_MARK <> 'S'");
            sb.Append(" ORDER BY MAKER_MILL_SHEET_LOG_CD ");

            return ExecuteQueryMultiResult<MakerMillSheetEDIDetails>(sb.ToString());

        }

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetErrorRptData(int runID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" MAKER_MILL_SHEET_LOG_CD, FILENAME, MAKER_NAME, MATL_NAME_MAKER, CHARGE, ");
            sb.Append("  DIM_D AS DIM_D ,");
            sb.Append(" LENGTH, NO_OF_UNITS, QUANTITY, HARDNESS_STD, HARDNESS_MIN,	");
            sb.Append("(CASE   WHEN GRAIN_SIZE_STD IS NULL   THEN NULL WHEN  HSKWEBUSER.IS_NUMBER(GRAIN_SIZE_STD) = 1 THEN   TO_CHAR (GRAIN_SIZE_STD, '90.0')  ELSE   GRAIN_SIZE_STD   END  ) AS GRAIN_SIZE_STD , ");
            sb.Append(" (CASE   WHEN GRAIN_SIZE_ACTUAL IS NULL   THEN NULL WHEN  HSKWEBUSER.IS_NUMBER(GRAIN_SIZE_ACTUAL) = 1 THEN   TO_CHAR (GRAIN_SIZE_ACTUAL, '90.0')  ELSE   GRAIN_SIZE_ACTUAL   END  ) AS GRAIN_SIZE_ACTUAL, ");
            sb.Append(" HARDNESS_MAX, HARDNESS_ACTUAL, CCNAME1, 	");
            sb.Append(" (case when CCRANGE1 is null then null else '×'||CCRANGE1 end ) as CCRANGE1 , CCMIN1, CCMAX1,	");
            sb.Append("	      CCACTUAL1 	");
            sb.Append(" , CCNAME2, 	");
            sb.Append("  (case when CCRANGE2 is null then null else '×'||CCRANGE2 end ) as CCRANGE2, 	");
            sb.Append(" CCMIN2, CCMAX2, CCACTUAL2, CCNAME3,	");
            sb.Append("  (case when CCRANGE3 is null then null else '×'||CCRANGE3 end ) as CCRANGE3,	");
            sb.Append("  CCMIN3, CCMAX3, CCACTUAL3, CCNAME4, 	");
            sb.Append("  (case when CCRANGE4 is null then null else '×'||CCRANGE4 end ) as CCRANGE4,	");
            sb.Append("   CCMIN4, CCMAX4,	");
            sb.Append(" CCACTUAL4, CCNAME5, 	");
            sb.Append(" (case when CCRANGE5 is null then null else '×'||CCRANGE5 end ) as CCRANGE5 ,	");
            sb.Append("  CCMIN5, CCMAX5, CCACTUAL5, CCNAME6,	");
            sb.Append(" (case when CCRANGE6 is null then null else '×'||CCRANGE6 end ) as CCRANGE6 , 	");
            sb.Append(" CCMIN6, CCMAX6, CCACTUAL6, CCNAME7, 	");
            sb.Append("  (case when CCRANGE7 is null then null else '×'||CCRANGE7 end ) as CCRANGE7,	");
            sb.Append("  CCMIN7, CCMAX7,       CCACTUAL7, CCNAME8 ,	");
            sb.Append(" (case when CCRANGE8 is null then null else '×'||CCRANGE8 end ) as CCRANGE8,       	");
            sb.Append("  CCMIN8, CCMAX8, CCACTUAL8, CCNAME9,	");
            sb.Append(" (case when CCRANGE9 is null then null else '×'||CCRANGE9 end ) as CCRANGE9,	");
            sb.Append(" CCMIN9, CCMAX9, CCACTUAL9, CCNAME10,  CCMIN10,	");
            sb.Append(" (case when CCRANGE10 is null then null else '×'||CCRANGE10 end ) as CCRANGE10,	");
            sb.Append(" CCMAX10, CCACTUAL10, CCNAME11, 	");
            sb.Append("	      (case when CCRANGE11 is null then null else '×'||CCRANGE11 end ) as CCRANGE11,	");
            sb.Append(" CCMIN11, CCMAX11, CCACTUAL11,       CCNAME12, 	");
            sb.Append(" (case when CCRANGE12 is null then null else '×'||CCRANGE12 end ) as CCRANGE12, 	");
            sb.Append(" CCMIN12, CCMAX12, CCACTUAL12, CCNAME13, 	");
            sb.Append(" (case when CCRANGE13 is null then null else '×'||CCRANGE13 end ) as CCRANGE13,	");
            sb.Append(" CCMIN13, CCMAX13, CCACTUAL13, CCNAME14, 	");
            sb.Append(" (case when CCRANGE14 is null then null else '×'||CCRANGE14 end ) as CCRANGE14,	");
            sb.Append(" CCMIN14, CCMAX14,	");
            sb.Append(" CCACTUAL14, CCNAME15, 	");
            sb.Append(" (case when CCRANGE15 is null then null else '×'||CCRANGE15 end ) as CCRANGE15,	");
            sb.Append("  CCMIN15, CCMAX15, CCACTUAL15,	");
            sb.Append(" CCNAME16, 	");
            sb.Append(" (case when CCRANGE16 is null then null else '×'||CCRANGE16 end ) as CCRANGE16,	");
            sb.Append("  CCMIN16, CCMAX16, CCACTUAL16, CCNAME17, 	");
            sb.Append("	      (case when CCRANGE17 is null then null else '×'||CCRANGE17 end ) as CCRANGE17,	");
            sb.Append(" CCMIN17, CCMAX17, CCACTUAL17, CCNAME18, 	");
            sb.Append("	      (case when CCRANGE18 is null then null else '×'||CCRANGE18 end ) as CCRANGE18,	");
            sb.Append(" CCMIN18, CCMAX18,	");
            sb.Append(" CCACTUAL18, CCNAME19, 	");
            sb.Append(" (case when CCRANGE19 is null then null else '×'||CCRANGE19 end ) as CCRANGE19,	");
            sb.Append(" CCMIN19, CCMAX19, CCACTUAL19,	");
            sb.Append(" CCNAME20,	");
            sb.Append("  (case when CCRANGE20 is null then null else '×'||CCRANGE20 end ) as CCRANGE20,	");
            sb.Append("   CCMIN20, CCMAX20, CCACTUAL20, CCNAME21, 	");
            sb.Append("   (case when CCRANGE21 is null then null else '×'||CCRANGE21 end ) as CCRANGE21,	");
            sb.Append("   (case when CCRANGE22 is null then null else '×'||CCRANGE22 end ) as CCRANGE22,	");
            sb.Append(" CCMIN21, CCMAX21, CCACTUAL21, CCNAME22, CCMIN22, CCMAX22,	");
            sb.Append(" CCACTUAL22, CCNAME23, 	");
            sb.Append("	      (case when CCRANGE23 is null then null else '×'||CCRANGE23 end ) as CCRANGE23,	");
            sb.Append(" CCMIN23, CCMAX23, CCACTUAL23,	");
            sb.Append(" CCNAME24, 	");
            sb.Append(" (case when CCRANGE24 is null then null else '×'||CCRANGE24 end ) as CCRANGE24,	");
            sb.Append(" CCMIN24, CCMAX24, CCACTUAL24, CCNAME25, 	");
            sb.Append(" (case when CCRANGE25 is null then null else '×'||CCRANGE25 end ) as CCRANGE25,	");
            sb.Append(" CCMIN25, CCMAX25, CCACTUAL25, HARD_STD1, HARD_MIN1, HARD_MAX1,	");
            sb.Append(" HARD_ACTUAL1, HARD_STD2, HARD_MIN2, HARD_MAX2, HARD_ACTUAL2, HARD_STD3,	");
            sb.Append(" HARD_MIN3, HARD_MAX3, HARD_ACTUAL3, HARD_STD4, HARD_MIN4, HARD_MAX4,	");
            sb.Append(" HARD_ACTUAL4, HARD_STD5, HARD_MIN5, HARD_MAX5, HARD_ACTUAL5, HARD_STD6,	");
            sb.Append(" HARD_MIN6, HARD_MAX6, HARD_ACTUAL6, HARD_STD7, HARD_MIN7, HARD_MAX7,	");
            sb.Append(" HARD_ACTUAL7, HARD_STD8, HARD_MIN8, HARD_MAX8, HARD_ACTUAL8, HARD_STD9,	");
            sb.Append(" HARD_MIN9, HARD_MAX9, HARD_ACTUAL9, HARD_STD10, HARD_MIN10, HARD_MAX10,	");
            sb.Append(" HARD_ACTUAL10, HARD_STD11, HARD_MIN11, HARD_MAX11, HARD_ACTUAL11,	");
            sb.Append(" HARD_STD12, HARD_MIN12, HARD_MAX12, HARD_ACTUAL12, HARD_STD13,	");
            sb.Append(" HARD_MIN13, HARD_MAX13, HARD_ACTUAL13, HARD_STD14, HARD_MIN14,	");
            sb.Append(" HARD_MAX14, HARD_ACTUAL14, HARD_STD15, HARD_MIN15, HARD_MAX15,	");
            sb.Append(" HARD_ACTUAL15, HARD_STD16, HARD_MIN16, HARD_MAX16, HARD_ACTUAL16,	");
            sb.Append(" HARD_STD17, HARD_MIN17, HARD_MAX17, HARD_ACTUAL17, HARD_STD18,	");
            sb.Append(" HARD_MIN18, HARD_MAX18, HARD_ACTUAL18, HARD_STD19, HARD_MIN19,	");
            sb.Append(" HARD_MAX19, HARD_ACTUAL19, HARD_STD20, HARD_MIN20, HARD_MAX20,	");
            sb.Append(" HARD_ACTUAL20, HARD_STD21, HARD_MIN21, HARD_MAX21, HARD_ACTUAL21,	");
            sb.Append(" HARD_STD22, HARD_MIN22, HARD_MAX22, HARD_ACTUAL22, HARD_STD23,	");
            sb.Append(" HARD_MIN23, HARD_MAX23, HARD_ACTUAL23, HARD_STD24, HARD_MIN24,	");
            sb.Append(" HARD_MAX24, HARD_ACTUAL24, HARD_STD25, HARD_MIN25, HARD_MAX25,	");
            sb.Append(" HARD_ACTUAL25, DUMMY, RUN_CD, STATUS, 'エラー' AS STATUS_MARK, MESSAGE,	");
            sb.Append(" TO_CHAR(CREATED_DATE,'yyyy/mm/dd hh24:mi:ss') AS CREATED_DATE ");
            sb.Append(" FROM  ");
            sb.Append(" MAKER_MILL_SHEET_LOG ");
            sb.Append(" WHERE ");
            sb.Append(" RUN_CD = " + runID);
            sb.Append(" AND STATUS = 1 ");
            sb.Append(" AND STATUS_MARK <> 'S'");
            sb.Append(" ORDER BY MAKER_MILL_SHEET_LOG_CD ");

            return ExecuteQueryMultiResult<MakerMillSheetEDIDetails>(sb.ToString());

        }
        public List<MakerMillSheetEDIDetails> GetEDIErrorReportData(int runCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAKER_NAME ");
            selectQuery.Append(" ,MATL_NAME_MAKER ");
            selectQuery.Append(" ,CHARGE ");
            selectQuery.Append(" ,DIM_D ");
            selectQuery.Append(" ,LENGTH ");
            selectQuery.Append(" ,FILENAME ");
            selectQuery.Append(" ,MESSAGE ");
            selectQuery.Append(" ,CREATED_DATE ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET_LOG ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" STATUS = 1 ");
            selectQuery.Append(" AND RUN_CD = " + runCD);
            return ExecuteQueryMultiResult<MakerMillSheetEDIDetails>(selectQuery.ToString());

        }

        public List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchReport(int userID, DateTime processDateTime, int printFlag)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG ");
            selectQuery.Append(" WHERE ");
            if (printFlag == 1)
            {
                selectQuery.Append(" PROCESS_DATE_AND_TIME = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') ");
            }
            else
            {
                selectQuery.Append(" TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");

            }
            selectQuery.Append(" ) AS TRANSFER_COUNT, ");
            selectQuery.Append(" TO_CHAR(PROCESS_DATE_AND_TIME, 'YYYY/MM/DD') AS PROCESS_DATE_AND_TIME, ");
            selectQuery.Append(" TO_CHAR(FUTO_NYUKO_DATE, 'YYYY/MM/DD') AS FUTO_NYUKO_DATE, ");
            selectQuery.Append(" ORDER_KOUBAN, ");
            selectQuery.Append(" STEEL_GRADE, ");
            selectQuery.Append(" DIMD_DECOMPOSITION_SIZE, ");
            selectQuery.Append(" SHAPE_INFO, ");
            selectQuery.Append(" DELIVERY_CONDITION, ");
            selectQuery.Append(" WINDING_DIRECTION, ");
            selectQuery.Append(" CHARGE, ");
            selectQuery.Append(" ERROR, ");
            selectQuery.Append(" CONTRACT_NUMBER ");
            selectQuery.Append(" || '-' ");
            selectQuery.Append(" || CONTRACT_GYOUBAN AS CONTRACT_NUMBER, ");
            selectQuery.Append(" THABA_NO, ");
            selectQuery.Append(" FLOOR(TO_NUMBER(TRANSLATE(NO_OF_UNITS, '-', ' '))) AS NO_OF_UNITS, ");
            selectQuery.Append(" FLOOR(TO_NUMBER(TRANSLATE(QUANTITY_IN_KG, '-', ' '))) AS QUANTITY_IN_KG, ");
            selectQuery.Append(" EXECUTE_RESULT, ");
           
            selectQuery.Append(" ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" SUM(FLOOR(TO_NUMBER(TRANSLATE(QUANTITY_IN_KG, '-', ' ')))) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG ");
            selectQuery.Append(" WHERE "); 
            if(printFlag == 1)
            {
                selectQuery.Append(" PROCESS_DATE_AND_TIME = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') ");

            }

            else
            {
                selectQuery.Append(" TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");

            }
            selectQuery.Append(" ) AS QTYSUM ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG ");
            selectQuery.Append(" WHERE ");
            if(printFlag == 1)
            {
                selectQuery.Append(" PROCESS_DATE_AND_TIME = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') ");

            }
            else
            {
                selectQuery.Append(" TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");

            }
            selectQuery.Append(" ORDER BY ");
            selectQuery.Append(" STEEL_GRADE, ");
            selectQuery.Append(" DIMD_DECOMPOSITION_SIZE, ");
            selectQuery.Append(" ORDER_KOUBAN, ");
            selectQuery.Append(" CHARGE, ");
            selectQuery.Append(" THABA_NO, ");
            selectQuery.Append(" FUTO_NYUKO_DATE ASC ");
            return ExecuteQueryMultiResult<DaidoFutouNyukoDetails>(selectQuery.ToString());
        }

        public List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchErrorReport(int userID, DateTime processDateTime, int printFlag)
        {
            StringBuilder selectErrorQuery = new StringBuilder();


            selectErrorQuery.Append(" SELECT ");
            selectErrorQuery.Append(" ( ");
            selectErrorQuery.Append(" SELECT ");
            selectErrorQuery.Append(" COUNT(*) ");
            selectErrorQuery.Append(" FROM ");
            selectErrorQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG ");
            selectErrorQuery.Append(" WHERE ");
            selectErrorQuery.Append(" EXECUTE_RESULT = 2 ");

            if(printFlag == 1)
            {
                selectErrorQuery.Append(" AND PROCESS_DATE_AND_TIME = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') ");

            }
            else
            {
                selectErrorQuery.Append(" AND TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");

            }
            selectErrorQuery.Append(" ) AS TRANSFER_COUNT, ");
            selectErrorQuery.Append(" TO_CHAR(PROCESS_DATE_AND_TIME, 'YYYY/MM/DD') AS PROCESS_DATE_AND_TIME, ");
            selectErrorQuery.Append(" TO_CHAR(FUTO_NYUKO_DATE, 'YYYY/MM/DD') AS FUTO_NYUKO_DATE, ");
            selectErrorQuery.Append(" ORDER_KOUBAN, ");
            selectErrorQuery.Append(" STEEL_GRADE, ");
            selectErrorQuery.Append(" DIMD_DECOMPOSITION_SIZE, ");
            selectErrorQuery.Append(" SHAPE_INFO, ");
            selectErrorQuery.Append(" DELIVERY_CONDITION, ");
            selectErrorQuery.Append(" WINDING_DIRECTION, ");
            selectErrorQuery.Append(" CHARGE, ");
            selectErrorQuery.Append(" ERROR, ");
            selectErrorQuery.Append(" CONTRACT_NUMBER ");
            selectErrorQuery.Append(" || '-' ");
            selectErrorQuery.Append(" || CONTRACT_GYOUBAN AS CONTRACT_NUMBER, ");
            selectErrorQuery.Append(" THABA_NO, ");
            selectErrorQuery.Append(" FLOOR(TO_NUMBER(TRANSLATE(NO_OF_UNITS, '-', ' '))) AS NO_OF_UNITS, ");
            selectErrorQuery.Append(" FLOOR(TO_NUMBER(TRANSLATE(QUANTITY_IN_KG, '-', ' '))) AS QUANTITY_IN_KG, ");
            selectErrorQuery.Append(" EXECUTE_RESULT, ");
           
            selectErrorQuery.Append(" ( ");
            selectErrorQuery.Append(" SELECT ");
            selectErrorQuery.Append(" SUM(FLOOR(TO_NUMBER(TRANSLATE(QUANTITY_IN_KG, '-', ' ')))) ");
            selectErrorQuery.Append(" FROM ");
            selectErrorQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG ");
            selectErrorQuery.Append(" WHERE ");
            selectErrorQuery.Append(" EXECUTE_RESULT = 2 ");
            if (printFlag == 1)
            {
                selectErrorQuery.Append(" AND PROCESS_DATE_AND_TIME = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') ");
            }
            else
            {
                selectErrorQuery.Append(" AND TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");

            }
            selectErrorQuery.Append(" ) AS QTYSUM ");
            selectErrorQuery.Append(" FROM ");
            selectErrorQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG ");
            selectErrorQuery.Append(" WHERE ");
            selectErrorQuery.Append(" EXECUTE_RESULT = 2 ");
            if (printFlag == 1)
            {
                selectErrorQuery.Append(" AND PROCESS_DATE_AND_TIME = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') ");
            }
            else
            {
                selectErrorQuery.Append(" AND TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + processDateTime.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");

            }
            selectErrorQuery.Append(" ORDER BY ");
            selectErrorQuery.Append(" STEEL_GRADE, ");
            selectErrorQuery.Append(" DIMD_DECOMPOSITION_SIZE, ");
            selectErrorQuery.Append(" ORDER_KOUBAN, ");
            selectErrorQuery.Append(" CHARGE, ");
            selectErrorQuery.Append(" THABA_NO, ");
            selectErrorQuery.Append(" FUTO_NYUKO_DATE ASC ");
            return ExecuteQueryMultiResult<DaidoFutouNyukoDetails>(selectErrorQuery.ToString());
        }
    }
}


 