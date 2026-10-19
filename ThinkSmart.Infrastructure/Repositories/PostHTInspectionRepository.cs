using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class PostHTInspectionRepository : BaseRepository, IPostHTInspectionRepository
    {
        private readonly string _connectionString;
        private object _sortField;
        private readonly LocalizationServices _localization;
        private readonly IConfiguration _configuration;
        private string postHT_Seq;
        public PostHTInspectionRepository(IConfiguration configuration,LocalizationServices localization) : base(configuration)
        {
            _localization = localization;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }


        public List<ProductOrderCompletion> GetProcessPlanDetails(List<string> chubanList)
        {
            List<ProductOrderCompletion> lstAllProductOrderCompletion = new List<ProductOrderCompletion>();
            List<ProductOrderCompletion> ProductOrderCompletion = new List<ProductOrderCompletion>();
            foreach (var chubanGyuban in chubanList)
            {
                if (chubanGyuban.Length == 11)
                {
                    string chuubanNo = chubanGyuban.Substring(0, 9);
                    string gyoubanNo = chubanGyuban.Substring(9, 2);
                    StringBuilder selectQuery = new StringBuilder();
                    try
                    {
                        selectQuery.Append("  SELECT  GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN ");
                        selectQuery.Append(" , PP.PROCESS_PLAN_CD ");
                        selectQuery.Append(" , CUST.SHORT_NAME AS CUSTOMER ");
                        selectQuery.Append(" , PP.PROD_NAME_CUST AS PROD_NAME_CUST ");
                        selectQuery.Append(" , PP.MATL_DIM_D ");
                        selectQuery.Append(" , PP.PROD_DIM_D as PP_PROD_DIM_D");
                        selectQuery.Append(" , PP.CHARGE ");
                        selectQuery.Append(", INT.INTERNAL_QUALITY_GROUP_NO ");
                        selectQuery.Append(" , GET_PP_STD_DMT(PP.PRODUCT_CD) AS DECARB_VAL_1");
                        //selectQuery.Append(" , GET_PP_STD_DMT(PP.PRODUCT_CD) AS DECARB_VAL_2");
                        selectQuery.Append(" , GET_PP_STD_DMF(PP.PRODUCT_CD) AS DECARB_VAL_3");
                        selectQuery.Append(" FROM TRN_PROCESS_PLAN PP  ");
                        selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD");
                        selectQuery.Append(" LEFT JOIN TRN_INTERNAL_QUALITY INT ON PP.PROCESS_PLAN_CD = INT.PROCESS_PLAN_CD");
                        selectQuery.Append(" WHERE PP.CHUUBAN = '" + chuubanNo + "' AND PP.GYOUBAN = '" + gyoubanNo + "' ");
                        selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS  NULL");
                        var ProcessPlanDetails = ExecuteQuerySingleResult<ProductOrderCompletion>(selectQuery.ToString());
                        if (ProcessPlanDetails != null)
                        {
                            lstAllProductOrderCompletion.Add(ProcessPlanDetails);
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("sql", selectQuery.ToString());
                        throw;
                    }
                }
            }

            return lstAllProductOrderCompletion;
        }

        public int GetGroupNoDetails()
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" NVL(MAX(INTERNAL_QUALITY_GROUP_NO), 0)+ 1 AS GROUP_NO");
            selectQuery.Append(" FROM TRN_INTERNAL_QUALITY");
            var result = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return result;
        }

        public List<NameValue> GetMachineNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
          //  query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" AND MACHINE_TYPE = 20");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }


        public List<PostHTDetails> FurnaceHdfDetails(string furnace_no)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT INTERNAL_QUALITY_CD from TRN_INTERNAL_QUALITY WHERE FURNACE_NO= '" + furnace_no+"'");
            var result = ExecuteQueryMultiResult<PostHTDetails>(selectQuery.ToString()).ToList();
            return result;

          
        }

        public void savePostHTInspectionDetails(PostHTDetails postHTDetails)
        {
            postHT_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Post_HT").Value);
            //  List<PostHTDetails> result = new List<PostHTDetails>();
            StringBuilder selectQuery = new StringBuilder();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append("INSERT INTO TRN_INTERNAL_QUALITY ( ");
            insertQuery.Append("INTERNAL_QUALITY_CD, ");
            insertQuery.Append("PROCESS_PLAN_CD, ");
            insertQuery.Append("MACHINE_CD, ");
            insertQuery.Append("LOCATION_CD, ");
            insertQuery.Append("INT_QLTY_TEST_OPERATOR_CD, ");
            insertQuery.Append("INT_QLTY_TEST_DATE, ");
            insertQuery.Append("HT_CONDITION, ");
            insertQuery.Append("PATTERN_NO, ");
            insertQuery.Append("FURNACE_NO, ");
            insertQuery.Append("LOCATION_IN_FURNACE1, ");
            insertQuery.Append("LOCATION_IN_FURNACE2, ");
            insertQuery.Append("STACK_POSITION, ");
            insertQuery.Append("ORIENTATION, ");
            insertQuery.Append("IN_OUT, ");
            insertQuery.Append("ACT_DM_F, ");
            insertQuery.Append("ACT_DM_T_MIN, ");
            insertQuery.Append("ACT_DM_T_MAX, ");
            insertQuery.Append("ACT_DM_T_AVG, ");
            insertQuery.Append("ACT_STRUCTURE, ");
            insertQuery.Append("HRD_CENTER, ");
            insertQuery.Append("HRD_D4_1, ");
            insertQuery.Append("HRD_D4_2, ");
            insertQuery.Append("HRD_D4_3, ");
            insertQuery.Append("HRD_D4_AVG, ");
            insertQuery.Append("INTERNAL_QUALITY_DECISION, ");
            insertQuery.Append("IQ_NEXT_PROCESS, ");
            insertQuery.Append("USER_ID, ");
            insertQuery.Append("INPUT_LOCATION_CD, ");
            insertQuery.Append("PROCESS_KUBUN_CD,");
            insertQuery.Append("CREATED_DATE, ");
            insertQuery.Append("UPDATED_DATE, ");
            insertQuery.Append("INTERNAL_QUALITY_GROUP_NO, ");
            insertQuery.Append("SERIAL_NUMBER ");
            insertQuery.Append(") VALUES (");
            if (string.IsNullOrEmpty(postHT_Seq))
            {
                insertQuery.Append("TRN_INTERNAL_QUALITY_SEQ.NEXTVAL ,");
            }else
            {
                insertQuery.Append(postHT_Seq + " , ");
            }
                
            insertQuery.Append(postHTDetails.PROCESS_PLAN_CD);
            insertQuery.Append(",'" + postHTDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + postHTDetails.LOCATION_CD + "'");
            insertQuery.Append(",'" + postHTDetails.INT_QLTY_TEST_OPERATOR_CD + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(postHTDetails.INT_QLTY_TEST_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')");
            //insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(postHTDetails.INT_QLTY_TEST_DATE).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy/mm/dd')");
            insertQuery.Append(",'" + postHTDetails.HT_CONDITION + "'");
            insertQuery.Append(",'" + postHTDetails.PATTERN_NO + "'");
            insertQuery.Append(",'" + postHTDetails.FURNACE_NO + "'");
            insertQuery.Append(",'" + postHTDetails.LOCATION_IN_FURNACE1 + "'");
            insertQuery.Append(",'" + postHTDetails.LOCATION_IN_FURNACE2 + "'");
            insertQuery.Append(",'" + postHTDetails.STACK_POSITION + "'");
            insertQuery.Append(",'" + postHTDetails.ORIENTATION + "'");
            insertQuery.Append(",'" + postHTDetails.IN_OUT + "'");
            insertQuery.Append(",'" + postHTDetails.ACT_DM_F + "'");
            insertQuery.Append(",'" + postHTDetails.ACT_DM_T_MIN + "'");
            insertQuery.Append(",'" + postHTDetails.ACT_DM_T_MAX + "'");
            insertQuery.Append(",'" + postHTDetails.ACT_DM_T_AVG + "'");
            insertQuery.Append(",'" + postHTDetails.ACT_STRUCTURE + "'");
            insertQuery.Append(",'" + postHTDetails.HRD_CENTER + "'");
            insertQuery.Append(",'" + postHTDetails.HRD_D4_1 + "'");
            insertQuery.Append(",'" + postHTDetails.HRD_D4_2 + "'");
            insertQuery.Append(",'" + postHTDetails.HRD_D4_3 + "'");
            //  insertQuery.Append(",'" + postHTDetails.HRD_D4_4 + "'");
            insertQuery.Append(",'" + postHTDetails.HRD_D4_AVG + "'");
            insertQuery.Append(",'" + postHTDetails.INTERNAL_QUALITY_DECISION + "'");
            insertQuery.Append(",'" + postHTDetails.IQ_NEXT_PROCESS + "'");
            insertQuery.Append(",'" + postHTDetails.USER_ID + "'");
            insertQuery.Append(",'" + postHTDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + postHTDetails.PROCESS_KUBUN_ID + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",'" + postHTDetails.INTERNAL_QUALITY_GROUP_NO + "'");
            insertQuery.Append(",'" + postHTDetails.SERIAL_NUMBER + "'");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());

                      
        }

        public void updatePostHTInspectionDetails(PostHTDetails postHTDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_INTERNAL_QUALITY SET ");

            updateQuery.Append("INTERNAL_QUALITY_CD='" + postHTDetails.INTERNAL_QUALITY_CD + "',");
            updateQuery.Append("PROCESS_PLAN_CD='" + postHTDetails.PROCESS_PLAN_CD + "',");
            updateQuery.Append("MACHINE_CD='" + postHTDetails.MACHINE_CD + "',");
            updateQuery.Append("LOCATION_CD='" + postHTDetails.LOCATION_CD + "',");
            updateQuery.Append("INT_QLTY_TEST_OPERATOR_CD='" + postHTDetails.INT_QLTY_TEST_OPERATOR_CD + "',");
            //updateQuery.Append(" INT_QLTY_TEST_DATE = ");
            updateQuery.Append(" INT_QLTY_TEST_DATE = TO_DATE('" + postHTDetails.INT_QLTY_TEST_DATE_STRING + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "') ,");
            //updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(postHTDetails.INT_QLTY_TEST_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy/mm/dd')" + ",");
            updateQuery.Append("HT_CONDITION='" + postHTDetails.HT_CONDITION + "',");
            updateQuery.Append("PATTERN_NO='" + postHTDetails.PATTERN_NO + "',");
            updateQuery.Append("FURNACE_NO='" + postHTDetails.FURNACE_NO + "',");
            updateQuery.Append("LOCATION_IN_FURNACE1='" + postHTDetails.LOCATION_IN_FURNACE1 + "',");
            updateQuery.Append("LOCATION_IN_FURNACE2='" + postHTDetails.LOCATION_IN_FURNACE2 + "',");
            updateQuery.Append("STACK_POSITION='" + postHTDetails.STACK_POSITION + "',");
            updateQuery.Append("ORIENTATION='" + postHTDetails.ORIENTATION + "',");
            updateQuery.Append("IN_OUT='" + postHTDetails.IN_OUT + "',");

            updateQuery.Append("ACT_DM_F='" + postHTDetails.ACT_DM_F + "',");
            updateQuery.Append("ACT_DM_T_MIN='" + postHTDetails.ACT_DM_T_MIN + "',");
            updateQuery.Append("ACT_DM_T_MAX='" + postHTDetails.ACT_DM_T_MAX + "',");
            updateQuery.Append("ACT_DM_T_AVG='" + postHTDetails.ACT_DM_T_AVG + "',");
            updateQuery.Append("ACT_STRUCTURE='" + postHTDetails.ACT_STRUCTURE + "',");
            updateQuery.Append("HRD_CENTER='" + postHTDetails.HRD_CENTER + "',");
            updateQuery.Append("HRD_D4_1='" + postHTDetails.HRD_D4_1 + "',");
            updateQuery.Append("HRD_D4_2='" + postHTDetails.HRD_D4_2 + "',");
            updateQuery.Append("HRD_D4_3='" + postHTDetails.HRD_D4_3 + "',");

            updateQuery.Append("HRD_D4_AVG='" + postHTDetails.HRD_D4_AVG + "',");
            updateQuery.Append("INTERNAL_QUALITY_DECISION='" + postHTDetails.INTERNAL_QUALITY_DECISION + "',");
            updateQuery.Append("IQ_NEXT_PROCESS='" + postHTDetails.IQ_NEXT_PROCESS + "',");
            updateQuery.Append("USER_ID='" + postHTDetails.USER_ID + "',");
            updateQuery.Append("INPUT_LOCATION_CD='" + postHTDetails.INPUT_LOCATION_CD + "',");
            updateQuery.Append("UPDATED_DATE = SYSDATE");
            // updateQuery.Append("INTERNAL_QUALITY_GROUP_NO='" + postHTDetails.INTERNAL_QUALITY_GROUP_NO + "'");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" INTERNAL_QUALITY_CD= '" + postHTDetails.INTERNAL_QUALITY_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());
        }

        public int GetGroupNo()
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT INTERNAL_QUALITY_GROUP_SEQ.NEXTVAL  FROM DUAL");
            var query = selectQuery.ToString();
            var GroupNo = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return GroupNo;
        }

        public void Delete(int groupNo)
        {
            StringBuilder deleteQuery = new StringBuilder();

            deleteQuery.Append(" DELETE FROM  TRN_INTERNAL_QUALITY ");
            deleteQuery.Append(" WHERE INTERNAL_QUALITY_GROUP_NO =" + groupNo);

            var query = deleteQuery.ToString();
            var GroupNo = Convert.ToInt32(ExecuteScalar(query.ToString()));


        }
        public void PostHTDetailsDelete(long groupNo)
        {
            StringBuilder deleteQuery = new StringBuilder();

            deleteQuery.Append(" DELETE FROM  TRN_INTERNAL_QUALITY ");
            deleteQuery.Append(" WHERE INTERNAL_QUALITY_GROUP_NO =" + groupNo);

            var query = deleteQuery.ToString();
            var GroupNo = Convert.ToInt32(ExecuteScalar(query.ToString()));
        }
        public int PostHTInspectionCheckMilkSheetData(long groupNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT count(*) FROM HSKUSER.HSK_MILL_SHEET HMS ");
            selectQuery.Append(" WHERE HMS.POST_HT_INSP_DTL_CD IN ( ");
            selectQuery.Append(" SELECT CASE WHEN WIN_INTERNAL_QUALITY_CD IS NOT NULL ");
            selectQuery.Append(" THEN WIN_INTERNAL_QUALITY_CD ELSE INTERNAL_QUALITY_CD  END ");
            selectQuery.Append(" FROM TRN_INTERNAL_QUALITY ");
            selectQuery.Append(" WHERE INTERNAL_QUALITY_GROUP_NO ='" + groupNo + "')");
            var query = selectQuery.ToString();

            var result = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return result;
        }
        public List<PostHTDetails> GetPostInspList(List<SearchField> searchCriteria, string sortCiteria)
        {
            List<PostHTDetails> result = new List<PostHTDetails>();
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT  * FROM(");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" INT.INTERNAL_QUALITY_GROUP_NO AS INTERNAL_QUALITY_GROUP_NO, ");
          //  selectQuery.Append(" INT.INT_QLTY_TEST_DATE,");
            selectQuery.Append(" TO_CHAR(INT.INT_QLTY_TEST_DATE, 'YYYY/MM/DD') AS INT_QLTY_TEST_DATE_STRING, ");
            selectQuery.Append(" M.SHORT_NAME AS IQ_MACHINE_NAME,");
            selectQuery.Append(" INT.FURNACE_NO, ");
            selectQuery.Append(" INT.HT_CONDITION AS , ");
            selectQuery.Append(" EMP.SHORT_NAME AS IQ_START_OPERATOR_NAME,");
            //selectQuery.Append(" INT.INTERNAL_QUALITY_CD, ");
            //selectQuery.Append(" INT.PROCESS_PLAN_CD, ");
            selectQuery.Append(" INT.PATTERN_NO ");
            selectQuery.Append(" FROM TRN_INTERNAL_QUALITY INT  ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE M  on INT.MACHINE_CD= M.MACHINE_CD  ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP  on INT.INT_QLTY_TEST_OPERATOR_CD = EMP.EMPLOYEE_CD  ");
            //selectQuery.Append(" WHERE INTERNAL_QUALITY_CD IN ");
            //selectQuery.Append(" (SELECT  MAX(INTERNAL_QUALITY_CD) FROM TRN_INTERNAL_QUALITY GROUP BY INTERNAL_QUALITY_GROUP_NO)");
            //selectQuery.Append(")");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;



                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }
                query = query + whereCondition;
            }
            query += " GROUP BY INTERNAL_QUALITY_GROUP_NO,to_char(int.int_qlty_test_date, 'YYYY/MM/DD'),M.SHORT_NAME, INT.FURNACE_NO, INT.HT_CONDITION, EMP.SHORT_NAME, INT.PATTERN_NO";
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            
            result = ExecuteQueryMultiResult<PostHTDetails>(query);
            return result;


        }

       public PostHTDetails GetChuubanListForInvaidCheck(string chuubanList)
        {           
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT lower(CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00'))) AS CHUUBAN ");
            selectQuery.Append(" , CUST.SHORT_NAME AS CUSTOMER ");
            selectQuery.Append(" ,CUST.SHORT_NAME AS CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST");
            selectQuery.Append(" ,PP.CHARGE ");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS PP_MATL_DIM_D_STRING");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(" ,PP.PROD_DIM_D");
            selectQuery.Append(" ,PP.MATL_DIM_D");
            selectQuery.Append(" , PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" , PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE ");
            selectQuery.Append(" , PP.PROCESS_PLAN_START_DATE ");
            selectQuery.Append(" , HT.HEAT_TREATMENT_CD ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN trn_heat_treatment HT ON PP.PROCESS_PLAN_CD=HT.PROCESS_PLAN_CD");
            selectQuery.Append(" WHERE LOWER(CHUUBAN||TRIM(LEADING ' ' FROM TO_CHAR(GYOUBAN,'00')))  IN ('" + chuubanList + "') AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
           
            return ExecuteQuerySingleResult<PostHTDetails>(selectQuery.ToString());
        }
        public List<string> GetCompletedChuubanList(string chuubanList)
        {
            List<string> result = new List<string>();
            DataTable dt = new DataTable();
            StringBuilder selectCompleteChuubanQuery = new StringBuilder();
            selectCompleteChuubanQuery.Append("SELECT DISTINCT (CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00'))) AS CHUUBAN FROM TRN_PROCESS_PLAN");
            selectCompleteChuubanQuery.Append(" WHERE LOWER(CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00')))  IN (" + chuubanList + ")");
            selectCompleteChuubanQuery.Append(" AND PROCESS_PLAN_CMPL_DATE IS NOT Null");
            dt = ExecuteQueryDataTable(selectCompleteChuubanQuery.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(Convert.ToString(dt.Rows[i][0]));
            }
            return result;
        }

        public List<PostHTDetails> GetProcPlanCDDetailsByInternalQCD(long groupNo)
        {
            List<PostHTDetails> result = new List<PostHTDetails>();
            //var pross_plan = GetProcessPlanCD(Convert.ToInt32(internalQCD));
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN ");
            selectQuery.Append(",TO_CHAR(INT.INT_QLTY_TEST_DATE, 'YYYY/MM/DD') AS INT_QLTY_TEST_DATE_STRING ");
            selectQuery.Append(",INT.INT_QLTY_TEST_DATE ");
            selectQuery.Append(",EMP.SHORT_NAME AS IQ_START_OPERATOR_NAME");
            selectQuery.Append(",INT.INT_QLTY_TEST_OPERATOR_CD");
            selectQuery.Append(",INT.MACHINE_CD");
            selectQuery.Append(",INT.INTERNAL_QUALITY_GROUP_NO");
            selectQuery.Append(",INT.FURNACE_NO ");
            selectQuery.Append(",INT.HT_CONDITION ");
            selectQuery.Append(",INT.PATTERN_NO ");
            selectQuery.Append(",INT.PROCESS_PLAN_CD ");
            selectQuery.Append(",CUST.SHORT_NAME AS CUSTOMER_NAME ");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PROD_NAME ");
            selectQuery.Append(",PP.CHARGE ");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS PP_MATL_DIM_D_STRING");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PP_PROD_DIM_D_STRING"); 
           // selectQuery.Append(",PP.MATL_DIM_D ");
           //  selectQuery.Append(",PP.PROD_DIM_D ");
           //selectQuery.Append(",GET_PP_STD_DMT(PP.PRODUCT_CD) AS DECARB_VAL_1");
           //selectQuery.Append(",GET_PP_STD_DMF(PP.PRODUCT_CD) AS DECARB_VAL_3");
            selectQuery.Append(",INT.LOCATION_IN_FURNACE1 ");
            selectQuery.Append(",INT.LOCATION_IN_FURNACE2 ");
            selectQuery.Append(",INT.STACK_POSITION ");
            selectQuery.Append(",INT.ORIENTATION ");
            selectQuery.Append(",INT.IN_OUT ");
            selectQuery.Append(",INT.ACT_DM_F ");
            selectQuery.Append(",INT.ACT_DM_T_MIN ");
            selectQuery.Append(",INT.ACT_DM_T_MAX ");
            selectQuery.Append(",INT.ACT_DM_T_AVG ");
            selectQuery.Append(",INT.ACT_STRUCTURE ");
            selectQuery.Append(",INT.HRD_CENTER ");
            selectQuery.Append(",INT.HRD_D4_1 ");
            selectQuery.Append(",INT.HRD_D4_2 ");
            selectQuery.Append(",INT.HRD_D4_3 ");
            selectQuery.Append(",INT.HRD_D4_AVG ");
            selectQuery.Append(",INT.INTERNAL_QUALITY_DECISION ");
            selectQuery.Append(",INT.IQ_NEXT_PROCESS ");
            selectQuery.Append(",INT.INTERNAL_QUALITY_CD ");

            selectQuery.Append(" FROM TRN_INTERNAL_QUALITY INT  ");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = INT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = INT.INT_QLTY_TEST_OPERATOR_CD");
            selectQuery.Append(" WHERE INT.INTERNAL_QUALITY_GROUP_NO = " + groupNo);

            result = ExecuteQueryMultiResult<PostHTDetails>(selectQuery.ToString());
            return result;

        }

        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2026/06/15       
        /// used screens: Get Furnace No Data to TRN_HEAT_TREATMENT
        /// </summary>
        /// <returns>
        /// Count of Heat Treatment process data according to search FurnaceNo        
        /// </returns>
        /// 
        public int checkvalidFurnaceNo(string furnaceNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT count(FURNACE_NO)  FROM TRN_HEAT_TREATMENT");

            selectQuery.Append(" WHERE FURNACE_NO ='" + furnaceNo + "'  ");
            var query = selectQuery.ToString();

            var FurnaceNo = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return FurnaceNo;


        }
        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2026/06/15       
        /// used screens: Get Furnace No Data to TRN_HEAT_TREATMENT
        /// </summary>
        /// <returns>
        /// Details of Heat Treatment process data according to search FurnaceNo        
        /// </returns>
        /// 
        public List<PostHTDetails> getFurnaceDetails(string furnaceNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<PostHTDetails> fno = new List<PostHTDetails>();

            selectQuery.Append(" SELECT  ");
            //selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN ");
            //selectQuery.Append(",TO_CHAR(INT.INT_QLTY_TEST_DATE, 'YYYY/MM/DD') AS INT_QLTY_TEST_DATE_STRING ");
            //selectQuery.Append(",INT.INTERNAL_QUALITY_CD ");
            //selectQuery.Append(",EMP.SHORT_NAME AS IQ_START_OPERATOR_NAME");
            //selectQuery.Append(",INT.INT_QLTY_TEST_DATE ");
            //selectQuery.Append(",INT.INT_QLTY_TEST_OPERATOR_CD");
            //selectQuery.Append(",INT.MACHINE_CD ");
            //selectQuery.Append(",INT.FURNACE_NO ");
            //selectQuery.Append(",INT.HT_CONDITION ");
            //selectQuery.Append(",INT.PATTERN_NO ");
            //selectQuery.Append(",INT.PROCESS_PLAN_CD ");
            //selectQuery.Append(",INT.INTERNAL_QUALITY_GROUP_NO");
            //selectQuery.Append(",CUST.SHORT_NAME AS CUSTOMER_NAME ");
            //selectQuery.Append(",PP.PROD_NAME_CUST AS PROD_NAME ");
            //selectQuery.Append(",PP.CHARGE ");
            //selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS PP_MATL_DIM_D_STRING");
            //selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PP_PROD_DIM_D_STRING");
            //// selectQuery.Append(",PP.MATL_DIM_D ");
            //// selectQuery.Append(",PP.PROD_DIM_D ");
            
            ////selectQuery.Append(",GET_PP_STD_DMT(PP.PRODUCT_CD) AS DECARB_VAL_1");
            ////selectQuery.Append(",GET_PP_STD_DMF(PP.PRODUCT_CD) AS DECARB_VAL_3");
            //selectQuery.Append(",INT.LOCATION_IN_FURNACE1 ");
            //selectQuery.Append(",INT.LOCATION_IN_FURNACE2 ");
            //selectQuery.Append(",INT.STACK_POSITION ");
            //selectQuery.Append(",INT.ORIENTATION ");
            //selectQuery.Append(",INT.IN_OUT ");
            //selectQuery.Append(",INT.ACT_DM_F ");
            //selectQuery.Append(",INT.ACT_DM_T_MIN ");
            //selectQuery.Append(",INT.ACT_DM_T_MAX ");
            //selectQuery.Append(",INT.ACT_DM_T_AVG ");
            //selectQuery.Append(",INT.ACT_STRUCTURE ");
            //selectQuery.Append(",INT.HRD_CENTER ");
            //selectQuery.Append(",INT.HRD_D4_1 ");
            //selectQuery.Append(",INT.HRD_D4_2 ");
            //selectQuery.Append(",INT.HRD_D4_3 ");
            //selectQuery.Append(",INT.HRD_D4_AVG ");
            //selectQuery.Append(",INT.INTERNAL_QUALITY_DECISION ");
            //selectQuery.Append(",INT.IQ_NEXT_PROCESS ");

            selectQuery.Append(" lower(chuuban || TRIM(LEADING ' ' FROM to_char(gyouban, '00'))) AS chuuban ");
            //selectQuery.Append(",TO_CHAR(INT.INT_QLTY_TEST_DATE, 'YYYY/MM/DD') AS INT_QLTY_TEST_DATE_STRING ");
            //selectQuery.Append(",INT.INTERNAL_QUALITY_CD ");
            //selectQuery.Append(",EMP.SHORT_NAME AS IQ_START_OPERATOR_NAME");
            //selectQuery.Append(",INT.INT_QLTY_TEST_DATE ");
            //selectQuery.Append(",INT.INT_QLTY_TEST_OPERATOR_CD");
            selectQuery.Append(" ,HT.MACHINE_CD ");
            selectQuery.Append(",HT.FURNACE_NO ");
            selectQuery.Append(",HT.HT_CONDITION ");
            selectQuery.Append(",HT.PATTERN_NO ");
            selectQuery.Append(",PP.PROCESS_PLAN_CD ");
            //selectQuery.Append(",INT.INTERNAL_QUALITY_GROUP_NO");
            selectQuery.Append(",CUST.SHORT_NAME AS CUSTOMER_NAME ");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PROD_NAME ");
            selectQuery.Append(",PP.CHARGE ");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS PP_MATL_DIM_D_STRING");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(",PP.PROD_DIM_D ");
            selectQuery.Append(",PP.MATL_DIM_D ");
            
            selectQuery.Append(",PP.PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE");
            selectQuery.Append(",INT.INTERNAL_QUALITY_CD ");


            selectQuery.Append(" FROM TRN_HEAT_TREATMENT HT  ");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_INTERNAL_QUALITY     INT ON PP.PROCESS_PLAN_CD = INT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = HT.SETUP_CONF_BY");
            selectQuery.Append(" WHERE HT.FURNACE_NO = '" + furnaceNo + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND HT.HT_END_DATE IS NOT NULL");

            fno = ExecuteQueryMultiResult<PostHTDetails>(selectQuery.ToString());
            return fno;
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of QA Test Hardness process data according to search criteria        
        /// </returns>
        /// 
        public List<PostHTDetails> GetQATestHardnessOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCiteria,int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<PostHTDetails> result = new List<PostHTDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS INSPECTION_LOCATION_NAME, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD,");
            selectQuery.Append(" TO_CHAR(IQ.INT_QLTY_TEST_DATE, 'yyyy/mm/dd') AS INT_QLTY_TEST_DATE_STRING, ");
            selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME, ");
            selectQuery.Append(" PP.CHARGE AS CHARGE, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'' ) AS PP_MATL_DIM_D_STRING, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'' ) AS PP_PROD_DIM_D_STRING, ");
            //selectQuery.Append(" --
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" IQ.FURNACE_NO AS FURNACE_NO, ");
            //selectQuery.Append(" --
            selectQuery.Append(" IQ.HT_CONDITION, ");
            //selectQuery.Append(" --
            selectQuery.Append(" IQ.PATTERN_NO AS PATTERN_NO, ");
            selectQuery.Append(" IQ.HRD_CENTER AS HRD_CENTER, ");
            selectQuery.Append(" IQ.ACT_DM_F AS ACT_DM_F, ");
            //selectQuery.Append(" --
            //selectQuery.Append(" --
            //selectQuery.Append(" --
            selectQuery.Append(" IQ.HRD_D4_1 AS HRD_D4_1, ");
            selectQuery.Append(" IQ.HRD_D4_2 AS HRD_D4_2, ");
            selectQuery.Append(" IQ.HRD_D4_3 AS HRD_D4_3, ");
            selectQuery.Append(" IQ.HRD_D4_4 AS HRD_D4_4, ");
            selectQuery.Append(" IQ.HRD_D4_AVG AS HRD_D4_AVG, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PROD_DIM_D, ");
            selectQuery.Append(" M.MSG_CONTENTS AS INT_QLTY_REMARKS, ");
            selectQuery.Append(" IQ.INTERNAL_QUALITY_DECISION AS INTERNAL_QUALITY_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS IQ_MACHINE_NAME, ");
            selectQuery.Append(" CONCAT(IQ.ACT_DM_T_MIN, ");
            selectQuery.Append(" CASE  WHEN IQ.ACT_DM_T_MAX IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', IQ.ACT_DM_T_MAX) ");
            selectQuery.Append(" ELSE   NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS ALL_ACT_DM_T, ");
            selectQuery.Append(" IQ.ACT_DM_T_AVG AS ACT_DM_T_AVG_STRING,");
            selectQuery.Append(" IQ.ACT_STRUCTURE, ");
            selectQuery.Append(" CONCAT(DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', DIM_TOL_MAX_CHR) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(") AS DIM_TOL_MIN_MAX_CHR, ");
            selectQuery.Append(" CONCAT(IQ.LOCATION_IN_FURNACE1, ");
            selectQuery.Append(" CASE  WHEN IQ.LOCATION_IN_FURNACE2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', IQ.LOCATION_IN_FURNACE2) ");
            selectQuery.Append(" ELSE   NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS LOCATION_IN_FURNACE1_2, ");
            selectQuery.Append(" IQ.STACK_POSITION, ");
            selectQuery.Append(" EMP.SHORT_NAME AS IQ_START_OPERATOR_NAME ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_INTERNAL_QUALITY IQ ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON IQ.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = IQ.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = IQ.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = IQ.LOCATION_CD ");
            //selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = IQ.INT_QLTY_TEST_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD =IQ.INT_QLTY_TEST_OPERATOR_CD ");


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
            //    query += " ORDER BY INT_QLTY_TEST_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PROD_NAME,PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<PostHTDetails>(query);
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of QA Test Hardness process data according to search criteria        
        /// </returns>
        /// 
        public List<PostHTDetails> GetQATestHardnessDetailsOnDailyProcessAnalysis(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
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
            selectQuery.Append(" PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING, ");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER, ");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN AS PP_MAGARI_KUBUN, ");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PP_MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS PP_MAGARI_SPAN, ");
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
            selectQuery.Append(" PROCESS1.PROCESS_NAME  AS PP_PROCESS_1, ");
            selectQuery.Append(" PROCESS2.PROCESS_NAME  AS PP_PROCESS_2, ");
            selectQuery.Append(" PROCESS3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" PROCESS4.PROCESS_NAME  AS PP_PROCESS_4, ");
            selectQuery.Append(" PROCESS5.PROCESS_NAME  AS PP_PROCESS_5, ");
            selectQuery.Append(" PROCESS6.PROCESS_NAME  AS PP_PROCESS_6, ");
            selectQuery.Append(" PROCESS7.PROCESS_NAME  AS PP_PROCESS_7, ");
            selectQuery.Append(" PROCESS8.PROCESS_NAME  AS PP_PROCESS_8, ");
            selectQuery.Append(" PROCESS9.PROCESS_NAME  AS PP_PROCESS_9, ");
            selectQuery.Append(" PROCESS10.PROCESS_NAME  AS PP_PROCESS_10, ");
            selectQuery.Append(" PROCESS11.PROCESS_NAME  AS PP_PROCESS_11, ");
            selectQuery.Append(" PROCESS12.PROCESS_NAME  AS PP_PROCESS_12, ");
            selectQuery.Append(" PROCESS13.PROCESS_NAME  AS PP_PROCESS_13, ");
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
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");
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
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL, ");
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
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");

            selectQuery.Append(" MAC.MACHINE_NAME AS IQ_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS INSPECTION_LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(IQ.INT_QLTY_TEST_DATE, 'yyyy/mm/dd') AS INT_QLTY_TEST_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS IQ_START_OPERATOR_NAME, ");
            selectQuery.Append(" IQ.HT_CONDITION AS HT_CONDITION, ");
            selectQuery.Append(" IQ.PATTERN_NO AS PATTERN_NO, ");
            selectQuery.Append(" IQ.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" IQ.LOCATION_IN_FURNACE1 AS LOCATION_IN_FURNACE1, ");
            selectQuery.Append(" IQ.LOCATION_IN_FURNACE2 AS LOCATION_IN_FURNACE2, ");
            selectQuery.Append(" IQ.STACK_POSITION AS STACK_POSITION, ");
            selectQuery.Append(" IQ.ACT_DM_F AS ACT_DM_F, ");
            selectQuery.Append(" IQ.ACT_DM_T_MIN AS ACT_DM_T_MIN, ");
            selectQuery.Append(" IQ.ACT_DM_T_MAX AS ACT_DM_T_MAX, ");
            selectQuery.Append(" IQ.ACT_DM_T_AVG AS ACT_DM_T_AVG, ");
            selectQuery.Append(" IQ.ACT_STRUCTURE AS ACT_STRUCTURE, ");
            selectQuery.Append(" IQ.HRD_CENTER AS HRD_CENTER, ");
            selectQuery.Append(" IQ.HRD_D4_1 AS HRD_D4_1, ");
            selectQuery.Append(" IQ.HRD_D4_2 AS HRD_D4_2, ");
            selectQuery.Append(" IQ.HRD_D4_3 AS HRD_D4_3, ");
            selectQuery.Append(" IQ.HRD_D4_4 AS HRD_D4_4, ");
            selectQuery.Append(" IQ.HRD_D4_AVG AS HRD_D4_AVG, ");
            selectQuery.Append(" IQ.INTERNAL_QUALITY_DECISION AS INTERNAL_QUALITY_DECISION, ");
            selectQuery.Append(" M.MSG_CONTENTS AS INT_QLTY_REMARKS ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_INTERNAL_QUALITY IQ ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = IQ.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");           
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
           
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");            
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = IQ.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = IQ.INT_QLTY_TEST_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS1 ON PP.PROCESS_1_CD = PROCESS1.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS2 ON PP.PROCESS_2_CD = PROCESS2.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS3 ON PP.PROCESS_3_CD = PROCESS3.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS4 ON PP.PROCESS_4_CD = PROCESS4.PROCESS_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS5 ON PP.PROCESS_5_CD = PROCESS5.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS6 ON PP.PROCESS_6_CD = PROCESS6.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS7 ON PP.PROCESS_7_CD = PROCESS7.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS8 ON PP.PROCESS_8_CD = PROCESS8.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS9 ON PP.PROCESS_9_CD = PROCESS9.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS10 ON PP.PROCESS_10_CD = PROCESS10.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS11 ON PP.PROCESS_11_CD = PROCESS11.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS12 ON PP.PROCESS_12_CD = PROCESS12.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS13 ON PP.PROCESS_13_CD = PROCESS13.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = IQ.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = IQ.MACHINE_CD ");

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
            //if (!string.IsNullOrEmpty(sortCriteria))
            //{
            //    query = Helper.ApplySortCriteria(sortCriteria, query);
            //}
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return ExecuteQueryMultiResult<PostHTDetails>(query);
        }
        public int GetUserLocationById(int userID)
        {
            int locationCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD");
            query.Append(" FROM MST_USER_LOGIN ");
            query.Append(" WHERE USER_ID=" + userID);
            locationCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return locationCD;
        }
        public List<PostHTDetails> GetFurnaceNoDetails(string FURNACE_NO, bool isNull)
        {
            StringBuilder selectQuery = new StringBuilder();          

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" IQ.INTERNAL_QUALITY_CD");            
            selectQuery.Append(",IQ.FURNACE_NO ");
            
            selectQuery.Append(" FROM TRN_INTERNAL_QUALITY IQ");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP  ON PP.PROCESS_PLAN_CD = IQ.PROCESS_PLAN_CD");
            selectQuery.Append(" WHERE ");           
            selectQuery.Append(" LOWER(IQ.FURNACE_NO) = '" + FURNACE_NO + "'");
            
            return ExecuteQueryMultiResult<PostHTDetails>(selectQuery.ToString());


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
        public int GetRowCountForQATestHardnessFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_INTERNAL_QUALITY IQ ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON IQ.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = IQ.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = IQ.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = IQ.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD =IQ.INT_QLTY_TEST_OPERATOR_CD ");

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
        public int GetRowCountForPostHTHardnessDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_INTERNAL_QUALITY IQ ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = IQ.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = IQ.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = IQ.INT_QLTY_TEST_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS1 ON PP.PROCESS_1_CD = PROCESS1.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS2 ON PP.PROCESS_2_CD = PROCESS2.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS3 ON PP.PROCESS_3_CD = PROCESS3.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS4 ON PP.PROCESS_4_CD = PROCESS4.PROCESS_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS5 ON PP.PROCESS_5_CD = PROCESS5.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS6 ON PP.PROCESS_6_CD = PROCESS6.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS7 ON PP.PROCESS_7_CD = PROCESS7.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS8 ON PP.PROCESS_8_CD = PROCESS8.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS9 ON PP.PROCESS_9_CD = PROCESS9.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS10 ON PP.PROCESS_10_CD = PROCESS10.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS11 ON PP.PROCESS_11_CD = PROCESS11.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS12 ON PP.PROCESS_12_CD = PROCESS12.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS13 ON PP.PROCESS_13_CD = PROCESS13.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = IQ.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = IQ.MACHINE_CD ");

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
    }
}
