using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Enums;


namespace TSWebApplication.Infrastructure.Repositories
{
    public class WIPTensouRepository : BaseRepository, IWIPTensouRepository
    {
        private readonly string _connectionString;
        private readonly object _sortField;
        public WIPTensouRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }

        public WIPTensouDetails GetTensouDetailsByChuubanGyouban(string nifuda)
        {
           string chuuban = nifuda.Substring(0, 7);
           string gyouban = nifuda.Substring(7).PadLeft(2, '0');
          StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            // selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS  PP_EMPLOYEE_NAME ");
            // selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX ");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR_ID ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,To_CHAR(PP.COIL_DIA_EXTERNAL) AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //  selectQuery.Append(" ,K.REMARKS AS PP_KAKOUIRAI_REMARKS ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE AS PROCESS_PLAN_START_DATE");

            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");

            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            //selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");

            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");

            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE "); 
            selectQuery.Append(" ,WIP.TRANSFER_CD");
            selectQuery.Append(" , PP.PROD_CONDITION_CD AS PP_CONDITION_CD");
           // selectQuery.Append(" ,(SELECT NIFUDA_CD FROM TRN_NIFUDA WHERE NIFUDA_NO = '" + nifudaVal + "') AS NIFUDA_CD ");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT JOIN TRN_WIP_AND_PROD_TRANSFER WIP ON");
            selectQuery.Append(" WIP.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
           
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");

            
                selectQuery.Append(" WHERE PP.CHUUBAN = '" + chuuban + "' ");
                selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "' ");
           
            //selectQuery.Append(" AND  ROWNUM <= 1");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" ORDER BY WIP.TRANSFER_CD DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY ");
            WIPTensouDetails result = ExecuteQuerySingleResult<WIPTensouDetails>(selectQuery.ToString());
            return result;
            // result = ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());
            //  return result;

        }

        //public int SaveWIPTensouDetails(WIPTensouDetails wipTensou)
        //{
        //    WIPTensouDetails wip = new WIPTensouDetails();
        //    StringBuilder insertQuery = new StringBuilder();
        //    var now = DateTime.Now;
        //    string time = now.ToString("HH:mm:ss");
        //    string dateTime = wipTensou.TRANSFER_DATE_STRING + " " + time;
        //    DateTime acceptanceDate =Convert.ToDateTime(wipTensou.TRANSFER_DATE_STRING).AddDays(1);
        //    string AcceptanceDate = wipTensou.ACCEPTANCE_DATE_STRING + " " + time;
        //    insertQuery.Append("INSERT INTO TRN_WIP_AND_PROD_TRANSFER (");
        //    insertQuery.Append("TRANSFER_CD, ");
        //    insertQuery.Append("TRANSFER_GROUP_NO, ");
        //    insertQuery.Append("PROCESS_PLAN_CD, ");
        //    insertQuery.Append("PROCESS_KUBUN_CD, ");
        //    insertQuery.Append("NIFUDA_CD, ");
        //    insertQuery.Append("LOCATION_CD_FROM, ");
        //    insertQuery.Append("TRANSIT_LOCATION_CD, ");
        //    insertQuery.Append("LOCATION_CD_TO, ");
        //    insertQuery.Append("START_VEHICLEL_CD, ");
        //    insertQuery.Append("RELAY_VEHICLEL_CD, ");
        //    insertQuery.Append("TRANSFER_DATE, ");
        //    insertQuery.Append("TRANSIT_PLANNED_DATE, ");
        //    insertQuery.Append("ACCEPTANCE_DATE, ");
        //    insertQuery.Append("TRANSIT_TYPE, ");
        //    insertQuery.Append("TABA_NO_1, ");
        //    insertQuery.Append("TABA_NO_2, ");
        //    insertQuery.Append("TABA_NO_3, ");
        //    insertQuery.Append("TABA_NO_4, ");
        //    insertQuery.Append("NO_OF_UNITS, ");
        //    insertQuery.Append("UNIT_TYPE, ");
        //    insertQuery.Append("QTY, ");
        //    insertQuery.Append("MSG_CD, ");
        //    insertQuery.Append("PLANNED_NEXT_PROSESS, ");
        //    insertQuery.Append("USER_ID, ");
        //    insertQuery.Append("INPUT_LOCATION_CD, ");
        //    insertQuery.Append("CREATED_DATE, ");
        //    insertQuery.Append("UPDATED_DATE ");
        //    insertQuery.Append(") VALUES (");
        //    insertQuery.Append("TRN_WIP_AND_PROD_TRANSFER_SEQ.NEXTVAL, ");
        //    insertQuery.Append("'" + wipTensou.WIP_TRANSFER_GROUP_NO + "', ");
        //    insertQuery.Append("'" + wipTensou.PROCESS_PLAN_CD + "', ");
        //    insertQuery.Append("'" + wipTensou.PROCESS_KUBUN_ID + "', ");
        //    insertQuery.Append("'" + wipTensou.NIFUDA_CD + "', ");
        //    insertQuery.Append("'" + wipTensou.LOCATION_CD + "', ");
        //    insertQuery.Append("NULL, "); // TRANSIT_LOCATION_CD (if not available)
        //    insertQuery.Append("'" + wipTensou.LOCATION_CD_TO + "', ");
        //    insertQuery.Append("'" + wipTensou.START_VEHICLEL_CD + "', ");
        //    insertQuery.Append("NULL, ");// RELAY_VEHICLEL_CD (if not available)
        //    insertQuery.Append("TO_DATE('" + dateTime + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "'), ");
        //    insertQuery.Append("NULL, "); // TRANSIT_PLANNED_DATE (if not available)
        //    insertQuery.Append("TO_DATE('" + AcceptanceDate + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "'), "); // ACCEPTANCE_DATE (if not available)
        //    insertQuery.Append("0, ");// TRANSIT_TYPE (if not available)
        //    insertQuery.Append("'" + wipTensou.TABA_NO_1 + "', ");
        //    insertQuery.Append("'" + wipTensou.TABA_NO_2 + "', ");
        //    insertQuery.Append("'" + wipTensou.TABA_NO_3 + "', ");
        //    insertQuery.Append("'" + wipTensou.TABA_NO_4 + "', ");
        //    insertQuery.Append("'" + wipTensou.NO_OF_UNITS + "', ");
        //    insertQuery.Append("'" + wipTensou.UNIT_TYPE + "', ");
        //    insertQuery.Append("'" + wipTensou.QTY + "', ");
        //    //insertQuery.Append("'" + wipTensou.MSG_CD + "', ");
        //    if (wipTensou.MSG_CD == 0)
        //    {
        //        insertQuery.Append(" null ,");
        //    }
        //    else
        //    {
        //        insertQuery.Append("'" + wipTensou.MSG_CD + "', ");
        //    }
        //    insertQuery.Append("'" + wipTensou.NEXT_PROSESS + "', ");
        //    insertQuery.Append("'" + wipTensou.USER_ID + "', ");
        //    insertQuery.Append("'" + wipTensou.INPUT_LOCATION_CD + "', ");
        //    insertQuery.Append("SYSDATE, ");
        //    insertQuery.Append("SYSDATE)");
        //    ExecuteNonQuery(insertQuery.ToString());


        //    StringBuilder selectQuery = new StringBuilder();
        //    selectQuery.Append(" SELECT MAX(TRANSFER_GROUP_NO) FROM TRN_WIP_AND_PROD_TRANSFER");
        //    int WIP_TRANSFER_GROUP_NO = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

        //    // int result = string.IsNullOrEmpty(MSG) ? 0 : Convert.ToInt32(MSG);
        //    return WIP_TRANSFER_GROUP_NO;

        //}

        public int SaveWIPTensouDetails(WIPTensouDetails wipTensou)
        {
            WIPTensouDetails wip = new WIPTensouDetails();
            StringBuilder insertQuery = new StringBuilder();
            var now = DateTime.Now;
            string time = "00:00:00";//now.ToString("HH:mm:ss");
            string dateTime = wipTensou.TRANSFER_DATE_STRING + " " + time;
            DateTime transferDate = Convert.ToDateTime(wipTensou.TRANSFER_DATE_STRING + " " + time);
            //DateTime acceptanceDate = Convert.ToDateTime(wipTensou.TRANSFER_DATE_STRING).AddDays(1);
            DateTime acceptancePlanDate = Convert.ToDateTime(wipTensou.ACCEPT_PLANNED_DATE + " " + time);
            if (transferDate == acceptancePlanDate)
            {
                acceptancePlanDate = acceptancePlanDate.AddMinutes(1);
            }

            string AcceptancePlanDate = acceptancePlanDate.ToString("yyyy/MM/dd HH:mm:ss");
            insertQuery.Append("INSERT INTO TRN_WIP_AND_PROD_TRANSFER (");
            insertQuery.Append("TRANSFER_CD, ");
            insertQuery.Append("TRANSFER_GROUP_NO, ");
            insertQuery.Append("PROCESS_PLAN_CD, ");
            insertQuery.Append("PROCESS_KUBUN_CD, ");
            insertQuery.Append("NIFUDA_CD, ");
            insertQuery.Append("LOCATION_CD_FROM, ");
            insertQuery.Append("TRANSIT_LOCATION_CD, ");
            insertQuery.Append("LOCATION_CD_TO, ");
            insertQuery.Append("START_VEHICLEL_CD, ");
            insertQuery.Append("RELAY_VEHICLEL_CD, ");
            insertQuery.Append("TRANSFER_DATE, ");
            insertQuery.Append("TRANSIT_PLANNED_DATE, ");
            insertQuery.Append("ACCEPTANCE_DATE, ");
            insertQuery.Append("ACCEPT_PLANNED_DATE,");
            insertQuery.Append("TRANSIT_TYPE, ");
            insertQuery.Append("TABA_NO_1, ");
            insertQuery.Append("TABA_NO_2, ");
            insertQuery.Append("TABA_NO_3, ");
            insertQuery.Append("TABA_NO_4, ");
            insertQuery.Append("NO_OF_UNITS, ");
            insertQuery.Append("UNIT_TYPE, ");
            insertQuery.Append("QTY, ");
            insertQuery.Append("MSG_CD, ");
            insertQuery.Append("PLANNED_NEXT_PROSESS, ");
            insertQuery.Append("USER_ID, ");
            insertQuery.Append("INPUT_LOCATION_CD, ");
            insertQuery.Append("CREATED_DATE, ");
            insertQuery.Append("UPDATED_DATE ");
            insertQuery.Append(") VALUES (");
            insertQuery.Append("TRN_WIP_AND_PROD_TRANSFER_SEQ.NEXTVAL, ");
            insertQuery.Append("'" + wipTensou.WIP_TRANSFER_GROUP_NO + "', ");
            insertQuery.Append("'" + wipTensou.PROCESS_PLAN_CD + "', ");
            insertQuery.Append("'" + wipTensou.PROCESS_KUBUN_ID + "', ");
            insertQuery.Append("'" + wipTensou.NIFUDA_CD + "', ");
            insertQuery.Append("'" + wipTensou.LOCATION_CD + "', ");
            insertQuery.Append("NULL, "); // TRANSIT_LOCATION_CD (if not available)
            insertQuery.Append("'" + wipTensou.LOCATION_CD_TO + "', ");
            insertQuery.Append("'" + wipTensou.START_VEHICLEL_CD + "', ");
            insertQuery.Append("NULL, ");// RELAY_VEHICLEL_CD (if not available)
            insertQuery.Append("TO_DATE('" + dateTime + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "'), ");
            insertQuery.Append("NULL, "); // TRANSIT_PLANNED_DATE (if not available)
            insertQuery.Append("NULL, ");
            insertQuery.Append("TO_DATE('" + AcceptancePlanDate + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "'), ");// ACCEPTANCE_DATE (if not available)
            insertQuery.Append("0, ");// TRANSIT_TYPE (if not available)
            insertQuery.Append("'" + wipTensou.TABA_NO_1 + "', ");
            insertQuery.Append("'" + wipTensou.TABA_NO_2 + "', ");
            insertQuery.Append("'" + wipTensou.TABA_NO_3 + "', ");
            insertQuery.Append("'" + wipTensou.TABA_NO_4 + "', ");
            insertQuery.Append("'" + wipTensou.NO_OF_UNITS + "', ");
            insertQuery.Append("'" + wipTensou.UNIT_TYPE + "', ");
            insertQuery.Append("'" + wipTensou.QTY + "', ");
            //insertQuery.Append("'" + wipTensou.MSG_CD + "', ");
            if (wipTensou.MSG_CD == 0)
            {
                insertQuery.Append(" null ,");
            }
            else
            {
                insertQuery.Append("'" + wipTensou.MSG_CD + "', ");
            }
            insertQuery.Append("'" + wipTensou.NEXT_PROSESS + "', ");
            insertQuery.Append("'" + wipTensou.USER_ID + "', ");
            insertQuery.Append("'" + wipTensou.INPUT_LOCATION_CD + "', ");
            insertQuery.Append("SYSDATE, ");
            insertQuery.Append("SYSDATE)");
            ExecuteNonQuery(insertQuery.ToString());
            //if(wipTensou.PP_CONDITION_CD!=null)
            UpdateProcessPlanData(wipTensou);

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MAX(TRANSFER_GROUP_NO) FROM TRN_WIP_AND_PROD_TRANSFER");
            int WIP_TRANSFER_GROUP_NO = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            // int result = string.IsNullOrEmpty(MSG) ? 0 : Convert.ToInt32(MSG);
            return WIP_TRANSFER_GROUP_NO;
            
        }
        public void UpdateProcessPlanData(WIPTensouDetails request)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_PROCESS_PLAN SET ");
           
            updateQuery.Append(" PROD_CONDITION_CD ='"+ request.PP_CONDITION_CD +"'");
            updateQuery.Append(", UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE PROCESS_PLAN_CD = '" + request.PROCESS_PLAN_CD +"'");
            ExecuteNonQuery(updateQuery.ToString());

        }
        public int GetMaxGroupNo()
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT MAX(TRANSFER_GROUP_NO) FROM TRN_WIP_AND_PROD_TRANSFER");
            int max = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            //int max = ExecuteQueryMultiResult<long?>(selectQuery);
            return max;
        }

        public string GetMessageFromMsgID(int MSG_CD)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT MSG_CONTENTS FROM TRN_MESSAGE WHERE MSG_CD='" + MSG_CD + "'");
            String MSG = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            //int max = ExecuteQueryMultiResult<long?>(selectQuery);
            return MSG;

        }

        public int GetMsgCdByMsgContent(string MSG_CONTENTS)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MSG_CD FROM TRN_MESSAGE WHERE MSG_CONTENTS='" + MSG_CONTENTS + "'");
            String MSG = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            int result = string.IsNullOrEmpty(MSG) ? 0 : Convert.ToInt32(MSG);
            return result;
        }
        public List<WIPTensouDetails> GetWIPTensouList(List<SearchField> searchCriteria, string sortCiteria, long locationCD)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();

            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append("SELECT * FROM (");
            selectQuery.Append("SELECT ");
            selectQuery.Append(" DISTINCT  WIP.TRANSFER_GROUP_NO");
            //selectQuery.Append(", WIP.WIP_TRANSFER_CD");
            selectQuery.Append(", (SELECT MAX( TO_CHAR(WIP.TRANSFER_DATE, 'YYYY/MM/DD')) FROM TRN_WIP_AND_PROD_TRANSFER WIP1 WHERE WIP1.TRANSFER_GROUP_NO =WIP.TRANSFER_GROUP_NO)  AS TRANSFER_DATE_STRING ");
            selectQuery.Append(" , ( ");
            selectQuery.Append(" SELECT MAX(TO_CHAR(WIP1.ACCEPT_PLANNED_DATE, 'YYYY/MM/DD'))");
            selectQuery.Append(" FROM TRN_WIP_AND_PROD_TRANSFER WIP1");
            selectQuery.Append(" WHERE WIP1.TRANSFER_GROUP_NO = WIP.TRANSFER_GROUP_NO");
            selectQuery.Append(" ) AS ACCEPT_PLANNED_DATE");
            selectQuery.Append(", LOCTO.SHORT_NAME AS LOCATION_TO");
            selectQuery.Append(", GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN)  AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(", CUS.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'',PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", NVL(PP.LENGTH_OTHER,PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS ACT_ALLOC_DATE_STRING ");
            selectQuery.Append(", PP.ACT_ALLOC_QTY");
            selectQuery.Append(", (PP.PROD_SPEC_NO1 || '-' || PP.PROD_SPEC_NO2) AS PP_PROD_SPEC_NO ");
            selectQuery.Append(", VHE.SHORT_NAME AS VEHICLE_NAME");
            selectQuery.Append(", LOCFROM.SHORT_NAME AS LOCATION_FROM  ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER WIP INNER JOIN  TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCTO ON LOCTO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCFROM ON LOCFROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD ");
            // selectQuery.Append(" LEFT JOIN TRN_ACTUAL_ALLOCATION  ACT ON ACT.ACT_ALLOC_CD =PP.ACT_ALLOC_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME MIN ON MIN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE VHE ON VHE.VEHICLE_CD = WIP.START_VEHICLEL_CD ");
            selectQuery.Append(" WHERE WIP.INPUT_LOCATION_CD = " + locationCD);
            // selectQuery.Append(" AND WIP.TRANSFER_DATE IS NOT NULL");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " AND ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    if (searchField.FieldName != null)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                        andOperator = " AND ";
                    }
                }

                query = query + whereCondition;
            }

            query = query + ")";

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += " ORDER BY PP_CHUUBAN_GYOUBAN ASC ";
            }


            result = ExecuteQueryMultiResult<WIPTensouDetails>(query);
            return result;
        }

        public List<WIPTensouDetails> GetWIPTransferDetailsByTrasferCD(long WIP_TRANSFER_CD)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN ");
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN ");
           
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            //selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS  PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR_ID ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,To_CHAR(PP.COIL_DIA_EXTERNAL) AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //  selectQuery.Append(" ,K.REMARKS AS PP_KAKOUIRAI_REMARKS ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");

            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");

            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");

            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");

            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" ,WIP.LOCATION_CD_TO ");
            selectQuery.Append(" ,WIP.START_VEHICLEL_CD ");
            selectQuery.Append(" ,WIP.TRANSFER_DATE ");
            selectQuery.Append(" ,WIP.TABA_NO_1 ");
            selectQuery.Append(" ,WIP.TABA_NO_2 ");
            selectQuery.Append(" ,WIP.TABA_NO_3 ");
            selectQuery.Append(" ,WIP.TABA_NO_4 ");
            selectQuery.Append(" ,WIP.NO_OF_UNITS ");
            selectQuery.Append(" ,WIP.UNIT_TYPE ");
            selectQuery.Append(" ,WIP.QTY ");
            selectQuery.Append(" ,WIP.MSG_CD ");
            selectQuery.Append(" ,WIP.ACCEPT_PLANNED_DATE AS PLANNED_DATE"); 
            selectQuery.Append(" ,WIP.PLANNED_NEXT_PROSESS AS NEXT_PROSESS");
            selectQuery.Append(" ,CASE WHEN WIP.NIFUDA_CD IS NULL THEN NULL ELSE (SELECT NIFUDA_NO FROM TRN_NIFUDA WHERE NIFUDA_CD = WIP.NIFUDA_CD)END AS NIFUDA_NO ");
            selectQuery.Append(" ,WIP.TRANSFER_CD");
            selectQuery.Append(" , PP.PROD_CONDITION_CD AS PP_CONDITION_CD");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("  INNER JOIN TRN_WIP_AND_PROD_TRANSFER WIP ON PP.PROCESS_PLAN_CD =WIP.PROCESS_PLAN_CD ");
            // selectQuery.Append("  LEFT JOIN TRN_KAKOUIRAI K  ");
            //  selectQuery.Append("  ON PP.KAKOUIRAI_CD   = K.KAKOUIRAI_CD  ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            // selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");

            //selectQuery.Append(" WHERE WIP.WIP_TRANSFER_CD= '" + wipTransfedCD + "'");
            selectQuery.Append(" WHERE WIP.TRANSFER_GROUP_NO = '" + WIP_TRANSFER_CD + "'");

            selectQuery.Append(" ORDER BY WIP.TRANSFER_CD ASC");

            result = ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());
            return result;


        }

        public List<WIPTensouDetails> IsAcceptanceCompleted(long WIP_TRANSFER_CD)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" ACCEPTANCE_DATE ");
            selectQuery.Append(" FROM TRN_WIP_AND_PROD_TRANSFER ");
            selectQuery.Append(" WHERE TRANSFER_GROUP_NO = '" + WIP_TRANSFER_CD + "'");

            result = ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString());
            return result;

        }

        public List<WIPTensouDetails> GetDataOnGridView(List<SearchField> searchCriteria, string sortCiteria, long locationCD)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();

            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append("SELECT * FROM (");
            selectQuery.Append("SELECT ");
            selectQuery.Append(" DISTINCT  WIP.TRANSFER_GROUP_NO");
            selectQuery.Append(" , WIP.WIP_TRANSFER_CD");
            selectQuery.Append(", (SELECT MAX( TO_CHAR(WIP.TRANSFER_DATE, 'YYYY/MM/DD')) FROM TRN_WIP_AND_PROD_TRANSFER WIP1 WHERE WIP1.TRANSFER_GROUP_NO =WIP.TRANSFER_GROUP_NO)  AS TRANSFER_DATE_STRING ");
            selectQuery.Append(", LOC.SHORT_NAME AS LOCATION_FROM");
            selectQuery.Append(", GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN)  AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(", CUS.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'',PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", NVL(PP.LENGTH_OTHER,PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE ");

            selectQuery.Append(", TO_CHAR(ACT.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS ACT_ALLOC_DATE_STRING ");
            selectQuery.Append(", ACT.ACT_ALLOC_QTY");
            selectQuery.Append(", (PP.PROD_SPEC_NO1 || '-' || PP.PROD_SPEC_NO2) AS PP_PROD_SPEC_NO ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER WIP INNER JOIN  TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_ACTUAL_ALLOCATION  ACT ON ACT.ACT_ALLOC_CD =PP.ACT_ALLOC_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME MIN ON MIN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    if (searchField.FieldName != null)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                        andOperator = " AND ";
                    }
                }

                query = query + whereCondition;
            }
            query = query + ")"; 

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
                //return query;
            }

            else
            {
                query = query + " ORDER BY TRANSFER_DATE_STRING DESC,PP_CHUUBAN_GYOUBAN ASC";
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<WIPTensouDetails>(query);
            return result;
        }

        public void WIPTensouDetailsUpdate(WIPTensouDetails request)
        {
            StringBuilder updateQuery = new StringBuilder();
            int result = GetCountWIPRequesttData(request);

            if (result != 0)
            {
                var now = DateTime.Now;
                string time = "00:00:00";//now.ToString("HH:mm:ss");
                string dateTime = request.TRANSFER_DATE_STRING + " " + time;
                //DateTime acceptanceDate = Convert.ToDateTime(request.TRANSFER_DATE_STRING).AddDays(1);
                string AcceptancePlanDate = request.ACCEPT_PLANNED_DATE + " " + time;
                updateQuery.Append("UPDATE TRN_WIP_AND_PROD_TRANSFER SET ");
                updateQuery.Append("  PROCESS_PLAN_CD ='" + request.PROCESS_PLAN_CD + "'");
                updateQuery.Append(" ,LOCATION_CD_FROM ='" + request.LOCATION_CD + "'");
                updateQuery.Append(" ,LOCATION_CD_TO ='" + request.LOCATION_CD_TO + "'");
                updateQuery.Append(" ,START_VEHICLEL_CD ='" + request.START_VEHICLEL_CD + "'");

                updateQuery.Append(" ,TRANSFER_DATE = TO_DATE('" + dateTime + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "')");
                updateQuery.Append(" ,TABA_NO_1 ='" + request.TABA_NO_1 + "'");
                updateQuery.Append(" ,TABA_NO_2 ='" + request.TABA_NO_2 + "'");
                updateQuery.Append(" ,TABA_NO_3 ='" + request.TABA_NO_3 + "'");
                updateQuery.Append(" ,TABA_NO_4 ='" + request.TABA_NO_4 + "'");
                updateQuery.Append(" ,NO_OF_UNITS ='" + request.NO_OF_UNITS + "'");
                updateQuery.Append(" ,UNIT_TYPE ='" + request.UNIT_TYPE + "'");
                updateQuery.Append(" ,QTY ='" + request.QTY + "'");
                if (request.MSG_CD == 0)
                {
                    updateQuery.Append(", MSG_CD = null ");
                }
                else
                {
                    updateQuery.Append(",MSG_CD ='" + request.MSG_CD + "'");
                }
                updateQuery.Append(" ,PLANNED_NEXT_PROSESS ='" + request.NEXT_PROSESS + "'");
               // updateQuery.Append(", TRANSFER_GROUP_NO ='" + request.WIP_TRANSFER_GROUP_NO + "'");
                //updateQuery.Append(", SERIAL_NUMBER ='" + request.SERIAL_NUMBER + "'");
                updateQuery.Append(", USER_ID ='" + request.USER_ID + "'");
                updateQuery.Append(", INPUT_LOCATION_CD ='" + (request.INPUT_LOCATION_CD != null ? request.INPUT_LOCATION_CD.ToString() : string.Empty) + "'");
                updateQuery.Append(" ,ACCEPT_PLANNED_DATE = TO_DATE('" + AcceptancePlanDate + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "')");

                //updateQuery.Append(" ,TABA_NO_1 ='" + request.TABA_NO_1 + "'");
              //  updateQuery.Append(", CREATED_DATE = SYSDATE ");
                updateQuery.Append(", UPDATED_DATE = SYSDATE ");
                updateQuery.Append(" WHERE TRANSFER_GROUP_NO = ");
                updateQuery.Append(request.WIP_TRANSFER_GROUP_NO);
                updateQuery.Append(" AND TRANSFER_CD = ");
                updateQuery.Append(request.TRANSFER_CD);
                ExecuteNonQuery(updateQuery.ToString());
                //if(request.PP_CONDITION_CD!=null)
               UpdateProcessPlanData(request);
            }
            else
            {
                SaveWIPTensouDetails(request);
            }
        }


        public int GetCountWIPRequesttData(WIPTensouDetails request)
        {

            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            selectQuery.Append("  FROM TRN_WIP_AND_PROD_TRANSFER WIP  ");
            selectQuery.Append(" WHERE WIP.TRANSFER_GROUP_NO = ");
            selectQuery.Append(" '" + request.WIP_TRANSFER_GROUP_NO + "' ");
            selectQuery.Append(" AND WIP.TRANSFER_CD = '" + request.TRANSFER_CD + "'");
            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;

        }

        //delete
        public int WIPTensouDetailsDelete(long WIPTransferCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            try
            {
                deleteQuery.Append(" DELETE FROM TRN_WIP_AND_PROD_TRANSFER ");
                deleteQuery.Append(" WHERE TRANSFER_GROUP_NO =" + WIPTransferCD);
                ExecuteNonQuery(deleteQuery.ToString());
                return 1; // success
            }
            catch (OracleException ex)
            {
                if (ex.Number == Constants.ExceptionNumber.FK_CONSTRAIN)
                {

                    return -1; // FK constraint violation
                }

                return 0; // other DB error


            }

        }



        public int GetLocCdByLocation(long userId)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID='" + userId + "'");
            int LocCD = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            //int max = ExecuteQueryMultiResult<long?>(selectQuery);
            return LocCD;

        }

        public int GetWIPRequestReportData(string transferDate, long userId)
        {

            DateTime fromDate = Convert.ToDateTime(transferDate);
            string getfromDate = Convert.ToString(fromDate.AddMonths(-1).ToString("yyyy/MM/dd"));

            StringBuilder selectQuery = new StringBuilder();
            int locationCD = GetLocCdByLocation(userId);
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            selectQuery.Append("  FROM TRN_WIP_AND_PROD_TRANSFER WIP  ");
            selectQuery.Append(" WHERE TRUNC(WIP.TRANSFER_DATE) = ");
            selectQuery.Append(" TO_DATE ('" + transferDate + "', 'yyyy/MM/dd') ");
            selectQuery.Append(" AND WIP.INPUT_LOCATION_CD = '" + locationCD + "'");
            int returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return returnVal;

        }
        public int GetWIPAcceptanceReportData(WIPTensouDetails details)
        {
            StringBuilder selectQuery = new StringBuilder();
            int locationCD = GetLocCdByLocation(Convert.ToInt64(details.USER_ID));

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" count(*)");
            selectQuery.Append("  FROM TRN_WIP_AND_PROD_TRANSFER WIP  ");
            selectQuery.Append(" WHERE TRUNC(WIP.ACCEPT_PLANNED_DATE) BETWEEN  TO_DATE('" + details.TRANSFER_DATE_START + "', 'yyyy/MM/dd') AND TO_DATE('" + details.TRANSFER_DATE_END + "', 'yyyy/MM/dd')");
            selectQuery.Append("  AND WIP.ACCEPTANCE_DATE IS NULL ");
            selectQuery.Append("  AND WIP.LOCATION_CD_TO = '" + locationCD + "'");

            int returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return returnVal;

        }

        public int GetRowCountForTransferReport(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            //selectQuery.Append(" FROM ");
            //selectQuery.Append(" TRN_WIP_TRANSFER WIP  ");
            //selectQuery.Append(" LEFT JOIN TRN_WIP_TRANSFER_ACCEPTANCE ACC ON ACC.WIP_TRANSFER_CD = WIP.WIP_TRANSFER_CD ");
            //selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            //selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");            
            //selectQuery.Append(" INNER JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            //selectQuery.Append(" INNER JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            //selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = NVL(ACC.NEXT_PROSESS,WIP.PLANNED_NEXT_PROSESS) ");
            //selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_ACC ON LOC_ACC.LOCATION_CD = ACC.INPUT_LOCATION_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_WIP ON WIP.MSG_CD = MSG_WIP.MSG_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_ACC ON ACC.MSG_CD = MSG_ACC.MSG_CD");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER WIP ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = NVL(WIP.ACCEPT_NEXT_PROSESS, WIP.PLANNED_NEXT_PROSESS) ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_WIP ON WIP.MSG_CD = MSG_WIP.MSG_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_ACC ON WIP.ACCEPT_MSG_CD = MSG_ACC.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE MV ON MV.VEHICLE_CD = WIP.START_VEHICLEL_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    //whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    if (searchField.SearchType == SearchType.Custom)
                    {
                        whereCondition += andOperator + "(" + searchField.FieldName + ")";
                    }
                    else
                    {
                        whereCondition += andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    }

                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }


        public List<WIPTensouDetails> GetTransferDetails(List<SearchField> searchCriteria, string sortCiteria, int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            //selectQuery.Append(" SELECT ");
            //selectQuery.Append(" WIP.TRANSFER_DATE , ");
            //selectQuery.Append(" ACC .ACCEPTANCE_DATE , ");
            //selectQuery.Append(" LOC_FROM.SHORT_NAME AS TRANSFER_START_LOCATION , ");
            //selectQuery.Append(" LOC_TO.SHORT_NAME AS TRANSFER_PLANNED_LOCATION , ");
            //selectQuery.Append(" LOC_ACC.SHORT_NAME AS TRANSFER_ACCEPTED_LOCATION , ");
            //selectQuery.Append("GET_HYPHENATED_CHUUBAN_GYOUBAN(pp.chuuban_year||PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN , ");
            //selectQuery.Append("CUST.SHORT_NAME AS PP_CUSTOMER_NAME , ");
            //selectQuery.Append("PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST , ");
            //selectQuery.Append("PP.PROD_SHAPE AS PP_PROD_SHAPE , ");
            //selectQuery.Append("PP.PROD_DIM_D AS PP_PROD_DIM_D , ");
            ////selectQuery.Append("PP.PROD_DIM_OTHER AS PROD_DIM_W_OTHER , ");
            //selectQuery.Append("GET_DIM_D_W_SHAPE_OTHER('',PP.PROD_DIM_W,'',NVL(TRIM(pp.prod_dim_other), '')) AS PP_PROD_DIM_W_OTHER, ");
            //selectQuery.Append("NVL(PP.LENGTH_OTHER, TO_CHAR(PP.PROD_LENGTH)) AS PP_PROD_LENGTH_STRING , ");
            //selectQuery.Append("PP.CHARGE AS PP_CHARGE , ");
            ////all taba no combine
            //selectQuery.Append("RTRIM(");
            //selectQuery.Append("NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') || ");
            //selectQuery.Append("NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') || ");
            //selectQuery.Append("NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') || ");
            //selectQuery.Append("NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', '')");
            //selectQuery.Append(", ',') AS TABA_STRING , ");

            //selectQuery.Append("NVL(ACC.NO_OF_UNITS,WIP.NO_OF_UNITS) AS NO_OF_UNITS , ");
            //selectQuery.Append("NVL(ACC.QTY,WIP.QTY) AS QTY , ");
            //selectQuery.Append("NVL(ACC.UNIT_TYPE,WIP.UNIT_TYPE) AS UNIT_TYPE , ");


            //selectQuery.Append("PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            //selectQuery.Append("WIP.WIP_TRANSFER_CD , ");
            //selectQuery.Append("MSG_WIP.MSG_CONTENTS AS TRANSFER_REMARK, ");
            //selectQuery.Append("MSG_ACC.MSG_CONTENTS AS ACCEPTANCE_REMARK, ");
            //selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP, ");
            //selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            //selectQuery.Append(" FROM ");
            //selectQuery.Append(" TRN_WIP_TRANSFER WIP");
            //selectQuery.Append(" INNER JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            //selectQuery.Append(" INNER JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");            
            //selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");            
            //selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            //selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_WIP_TRANSFER_ACCEPTANCE ACC ON ACC.WIP_TRANSFER_CD = WIP.WIP_TRANSFER_CD ");
            //selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = NVL(ACC.NEXT_PROSESS,WIP.PLANNED_NEXT_PROSESS) ");
            //selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_ACC ON LOC_ACC.LOCATION_CD = ACC.INPUT_LOCATION_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_WIP ON WIP.MSG_CD = MSG_WIP.MSG_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_ACC ON ACC.MSG_CD = MSG_ACC.MSG_CD");

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" TO_CHAR(WIP.ACCEPT_PLANNED_DATE,'yyyy/mm/dd' ) AS ACCEPT_PLANNED_DATE, ");
            selectQuery.Append(" LOC_TO.SHORT_NAME AS TRANSFER_PLANNED_LOCATION, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" TO_CHAR(WIP.TRANSFER_DATE,'yyyy/mm/dd' ) AS TRANSFER_DATE_STRING, ");
            selectQuery.Append(" LOC_FROM.SHORT_NAME AS TRANSFER_START_LOCATION,    ");
            selectQuery.Append(" MV.VEHICLE_NAME AS VEHICLE_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('', PP.PROD_DIM_W, '', NVL(TRIM(PP.PROD_DIM_OTHER), '')) AS PP_PROD_DIM_W_OTHER, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, TO_CHAR(PP.PROD_LENGTH)) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" RTRIM(NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') ");
            selectQuery.Append(" || NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') ");
            selectQuery.Append(" || NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') ");
            selectQuery.Append(" || NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', ''), ',') AS TABA_STRING, ");
            selectQuery.Append(" WIP.NO_OF_UNITS AS NO_OF_UNITS, ");
            selectQuery.Append(" WIP.QTY AS QTY, ");
            selectQuery.Append(" WIP.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
            selectQuery.Append(" WIP.TRANSFER_CD AS WIP_TRANSFER_CD, ");
            selectQuery.Append(" MSG_WIP.MSG_CONTENTS AS TRANSFER_REMARK, ");
            selectQuery.Append(" MSG_ACC.MSG_CONTENTS AS ACCEPTANCE_REMARK, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER WIP ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = NVL(WIP.ACCEPT_NEXT_PROSESS, WIP.PLANNED_NEXT_PROSESS) ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_WIP ON WIP.MSG_CD = MSG_WIP.MSG_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_ACC ON WIP.ACCEPT_MSG_CD = MSG_ACC.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE MV ON MV.VEHICLE_CD = WIP.START_VEHICLEL_CD ");

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
                    //whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    if (searchField.SearchType == SearchType.Custom)
                    {
                        whereCondition += andOperator + "(" + searchField.FieldName + ")";
                    }
                    else
                    {
                        whereCondition += andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    }
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
            //    query += " ORDER BY WIP.PROCESS_KUBUN_CD, WIP.TRANSFER_DATE DESC, CUST.YOBINA ASC, CUST.YOBIGANA ASC, CUST.SHORT_NAME ASC, PP_PROD_NAME_CUST ASC, PP_PROD_SHAPE ASC, PP.PROD_DIM_D ASC, PP.PROD_DIM_W ASC, pp.PROD_DIM_OTHER ASC, PP.PROD_LENGTH ASC, PP.LENGTH_OTHER ASC, PP_CHUUBAN ASC";

            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<WIPTensouDetails>(query);
            return result;
        }

        public List<WIPTensouDetails> GetTransferCD(long TransferGroupNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT TRANSFER_CD AS WIP_TRANSFER_CD,TRANSFER_GROUP_NO AS WIP_TRANSFER_GROUP_NO  from TRN_WIP_AND_PROD_TRANSFER WHERE TRANSFER_GROUP_NO= '" + TransferGroupNo + "'");
            var result = ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString()).ToList();
            return result;


        }

    }

}
