using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using Microsoft.Extensions.Configuration;

using System.Data;
using System.Globalization;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using static TSWebApplication.Domain.Utility.Enums;
using System.Collections;
using System.Configuration;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class NextProcessWaitingStatusSearchRepository: BaseRepository, INextProcessWaitingStatusSearchRepository
    {
        private readonly string _connectionString;
        private readonly string _specialChuuban;
        public NextProcessWaitingStatusSearchRepository(IConfiguration configuration) : base(configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDBConnection");
            _specialChuuban = configuration.GetSection("SpecialChuuban").Value;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2023/10/26       
        /// used screens: Next Process Wating Status Search
        /// </summary>
        /// <returns>
        /// List of waiting process according to search criteria        
        /// </returns>
        /// 

        public List<WipStatusDetails> GetNextProcessWaitingStatusSearch(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridsearchresult)
        {
            var queryResult = "";
            List<WipStatusDetails> result = new List<WipStatusDetails>();
            StringBuilder selectQuery = new StringBuilder();
            if (gridsearchresult.WIP_STATUS == "1")
            {
                queryResult = "";
                selectQuery.Append(" SELECT * FROM (");
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
                selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
                selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
                //selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
                selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_W_OTHER, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS AS WIP_NO_OF_UNITS, ");
                selectQuery.Append(" WIP.UNIT AS UNIT, ");
                selectQuery.Append(" WIP.WIP_QTY AS WIP_QTY, ");
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" TO_DATE('') AS PROCESS_START_DATE_STRING,");
                selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");                
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.LOCATION_CD,");
                selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");
                selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE,");
                selectQuery.Append(" WIP.PRE_PROCESS AS PRE_PROSESS,");
                selectQuery.Append(" WIP.PRE_PROCESS_DECISION AS PRE_PROSESS_DECISION,");
                selectQuery.Append(" '1' as WIP_STATUS,");
                selectQuery.Append(" 'WAIT' AS WIP_STATUS_STRING");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_WIP_STATUS WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = WIP.MSG_CD ");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NOT NULL OR WIP.START_EMPLOYEE_CD IS NULL) AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuuban + ") ");
                
                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append(whereCondition);
                }                
                
                selectQuery.Append(" )");
                queryResult = selectQuery.ToString();
                
            }
            else if (gridsearchresult.WIP_STATUS == "2")
            {
                queryResult = "";
                selectQuery.Append(" SELECT * FROM (");
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
                selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
                selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
                //selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
                selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_W_OTHER, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS AS WIP_NO_OF_UNITS, ");
                selectQuery.Append(" WIP.UNIT AS UNIT, ");
                selectQuery.Append(" WIP.WIP_QTY AS WIP_QTY, ");                
                selectQuery.Append(" NULL AS PROCESS_KUBUN_JP, ");
                selectQuery.Append(" NULL AS PROCESS_KUBUN_EN, ");
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" TO_CHAR(WIP.PROCESS_START_DATE,'YYYY/MM/DD HH24:MI') AS PROCESS_START_DATE_STRING, ");
                selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");               
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.LOCATION_CD,");                
                selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");
                selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE,");
                selectQuery.Append(" WIP.PRE_PROCESS AS PRE_PROSESS,");
                selectQuery.Append(" WIP.PRE_PROCESS_DECISION AS PRE_PROSESS_DECISION,");
                selectQuery.Append(" '2' as WIP_STATUS,");
                selectQuery.Append(" 'WIP' AS WIP_STATUS_STRING");
               
                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_WIP_STATUS WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");              
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");               
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = WIP.MSG_CD ");              
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NULL AND WIP.START_EMPLOYEE_CD IS NOT NULL) AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuuban + ") ");
                
                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField,null);
                        andOperator = " AND ";
                    }
                     selectQuery.Append(whereCondition);
                }
                selectQuery.Append(" )");
                queryResult = selectQuery.ToString();
            }


            else
            {   
                selectQuery.Append(" SELECT* FROM (");
                selectQuery.Append(" (SELECT ");

                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");

                selectQuery.Append(" CUST.SHORT_NAME        AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST      AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME   AS INTERNAL_NAME, ");
                //selectQuery.Append(" PP.PROD_DIM_D          AS PP_PROD_DIM_D, ");
                selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_W_OTHER, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" PP.PROD_SHAPE          AS PP_PROD_SHAPE, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE              AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS    AS WIP_NO_OF_UNITS, ");
                selectQuery.Append(" WIP.UNIT               AS UNIT, ");
                selectQuery.Append(" WIP.WIP_QTY            AS WIP_QTY, ");                
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY       AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME         AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" NULL AS PROCESS_START_DATE_STRING, ");
                selectQuery.Append(" M.MSG_CONTENTS         AS REMARK, ");               
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.LOCATION_CD, ");               
                selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");
                selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE,");
                selectQuery.Append(" WIP.PRE_PROCESS AS PRE_PROSESS,");
                selectQuery.Append(" WIP.PRE_PROCESS_DECISION AS PRE_PROSESS_DECISION,");
                selectQuery.Append(" '1' AS WIP_STATUS, ");
                selectQuery.Append(" 'WAIT' AS WIP_STATUS_STRING");

                selectQuery.Append(" FROM ");

                selectQuery.Append(" TRN_WIP_STATUS  WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN       PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER           CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME      INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");                
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE           EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");               
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE            M ON M.MSG_CD = WIP.MSG_CD ");               
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NOT NULL OR WIP.START_EMPLOYEE_CD IS NULL) ");
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuuban + ")  ");
               
                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append( whereCondition);
                }
                selectQuery.Append("  ) ");
                

                selectQuery.Append("  UNION ALL ");

                selectQuery.Append(" (SELECT ");

                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
                selectQuery.Append(" CUST.SHORT_NAME        AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST      AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME   AS INTERNAL_NAME, ");
                //selectQuery.Append(" PP.PROD_DIM_D          AS PP_PROD_DIM_D, ");
                selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_W_OTHER, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" PP.PROD_SHAPE          AS PP_PROD_SHAPE, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");

                selectQuery.Append(" PP.CHARGE              AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS    AS WIP_NO_OF_UNITS, ");
                selectQuery.Append(" WIP.UNIT               AS UNIT, ");
                selectQuery.Append(" WIP.WIP_QTY            AS WIP_QTY, ");                
                selectQuery.Append(" NULL AS PROCESS_KUBUN_JP, ");
                selectQuery.Append(" NULL AS PROCESS_KUBUN_EN, ");
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY       AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME         AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" TO_CHAR(WIP.PROCESS_START_DATE, 'YYYY/MM/DD HH24:MI') AS PROCESS_START_DATE_STRING, ");
                selectQuery.Append(" M.MSG_CONTENTS         AS REMARK, ");                
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.LOCATION_CD, ");               
                selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");
                selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE,");
                selectQuery.Append(" WIP.PRE_PROCESS AS PRE_PROSESS,");
                selectQuery.Append(" WIP.PRE_PROCESS_DECISION AS PRE_PROSESS_DECISION,");
                selectQuery.Append(" '2' AS WIP_STATUS, ");
                selectQuery.Append(" 'WIP' AS WIP_STATUS_STRING");

                selectQuery.Append(" FROM ");

                selectQuery.Append(" TRN_WIP_STATUS         WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN       PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER           CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME      INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");               
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE           EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");              
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE            M ON M.MSG_CD = WIP.MSG_CD ");                
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NULL AND WIP.START_EMPLOYEE_CD IS NOT NULL) AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuuban + ")  ");
                
                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append (whereCondition);
                }
                selectQuery.Append(" ) ");
                selectQuery.Append(" ) ");

                queryResult = selectQuery.ToString();

            }
           
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                queryResult = Helper.ApplySortCriteria(sortCriteria, queryResult);
            }
            else
            {
                queryResult += " ORDER BY NEXT_PROCESS_INVISIBLE,WIP_STATUS_STRING,PP_DELIVERY_DATE_STRING DESC,PP_PROD_NAME_CUST,PP_PROD_SHAPE,PROD_DIM_D,PROD_DIM_W,PROD_DIM_OTHER,PP_PROD_LENGTH_STRING,PP_CHUUBAN_GYOUBAN,TABA_NO";
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(queryResult);
            result = ExecuteQueryMultiResult<WipStatusDetails>(queryResult);        


            return result;
        }
        public List<NameValue> GetLocationNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD AS CD  ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG  ");
            query.Append(" FROM MST_LOCATION WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY DISPLAY_PRIORITY, ");
            query.Append(" LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
    }
}
