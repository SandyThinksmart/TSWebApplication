using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using System.Data;
using System.Globalization;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class HeatTreatmentRepository : BaseRepository, IHeatTreatmentRepository
    {
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly LocalizationServices _localization;


        public HeatTreatmentRepository(IConfiguration configuration, IAllCommonRepository allCommonRepository, LocalizationServices localization) : base(configuration)
        {
            _allCommonRepository = allCommonRepository;
            _localization = localization;
        }

        public List<HeatTreatMentDetails> GetHeatTreatmentEditDeleteStartEndDetails(int? HeatTreatMentCd, bool isNull)
        {


            StringBuilder selectQuery = new StringBuilder();
            string nullCondition = string.Empty;

            if (isNull)
            {
                nullCondition = "IS NULL";
            }
            else
            {
                nullCondition = "IS NOT NULL";
            }

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" HT.HEAT_TREATMENT_CD");
            selectQuery.Append(",HT.MACHINE_CD ");
            selectQuery.Append(",HT.FURNACE_NO ");
            selectQuery.Append(",HT.HEAT_TREATMENT_NUMBER ");
            selectQuery.Append(",HT.HT_CONDITION ");
            selectQuery.Append(",HT.PATTERN_NO ");
            selectQuery.Append(",HT.HT_START_DATE ");
            selectQuery.Append(",HT.SETUP_CONF_BY");
            selectQuery.Append(",HT.LOADING_CONF_BY");
            selectQuery.Append(",HT.PLANNED_UNLOAD_DATE");
            selectQuery.Append(",HT.HT_END_DATE");
            selectQuery.Append(",HT.OFFLOADED_BY");
            selectQuery.Append(",HT.OFFLOAD_CONF_BY");
            selectQuery.Append(",HT.PROCESS_PLAN_CD");
            selectQuery.Append(",HT.COIL_BAR_TYPE");
            selectQuery.Append(",HT.HT_END_DATE");
            selectQuery.Append(",HT.LOT_NO");
            selectQuery.Append(",HT.HT_NO_OF_UNITS");
            selectQuery.Append(",HT.UNIT_TYPE");
            selectQuery.Append(",HT.HT_WEIGHT");
            selectQuery.Append(",HT.ROLL_MONTH");
            selectQuery.Append(",HT.COIL_NO");
            selectQuery.Append(",HT.LOCATION_IN_FURNACE1");
            selectQuery.Append(",HT.LOCATION_IN_FURNACE2");
            selectQuery.Append(",HT.HT_TABA_NO_1");
            selectQuery.Append(",HT.HT_TABA_NO_2");
            selectQuery.Append(",HT.HT_TABA_NO_3");
            selectQuery.Append(",HT.HT_TABA_NO_4");
            selectQuery.Append(",HT.HT_NEXT_PROCESS");
            selectQuery.Append(",TM.MSG_CONTENTS AS HT_REMARKS");
            selectQuery.Append(",HT.MSG_CD");
            selectQuery.Append(",CUST.SHORT_NAME AS CUSTOMER_NAME");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PROD_NAME_CUST ");
            selectQuery.Append(",GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,TRIM(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS CHUUBAN ");
            selectQuery.Append(" , PP.CHARGE ");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS MATL_DIM_STRING");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PROD_DIM_STRING");
            selectQuery.Append(",HT.HT_END_DATE");
            selectQuery.Append(",HT.OFFLOADED_BY");
            selectQuery.Append(",HT.OFFLOAD_CONF_BY");
            selectQuery.Append(", HT.HT_REMARK  AS HT_HEADER_REMARK");
            selectQuery.Append(", PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE_STRING");
            selectQuery.Append("  FROM TRN_HEAT_TREATMENT HT");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP  ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD=CUST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE TM ON HT.MSG_CD=TM.MSG_CD ");

            selectQuery.Append(" WHERE ");
           // selectQuery.Append(" HT.HT_END_DATE IS NULL");
            if (HeatTreatMentCd > 0)
            {
                selectQuery.Append(" HT.HEAT_TREATMENT_NUMBER = '" + HeatTreatMentCd + "'");
            }

             selectQuery.Append(" ORDER BY HEAT_TREATMENT_ROW ");
            var get = ExecuteQueryMultiResult<HeatTreatMentDetails>(selectQuery.ToString());

            return get;


        }




        public List<HeatTreatMentDetails> GetHeatTreatmentDetails(int? MACHINE_CD, bool isNull)
        {


            StringBuilder selectQuery = new StringBuilder();
            string nullCondition = string.Empty;

            if (isNull)
            {
                nullCondition = "IS NULL";
            }
            else
            {
                nullCondition = "IS NOT NULL";
            }

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" HT.HEAT_TREATMENT_CD");
            selectQuery.Append(",HT.MACHINE_CD ");
            selectQuery.Append(",HT.FURNACE_NO ");
            selectQuery.Append(",HT.HEAT_TREATMENT_NUMBER ");
            selectQuery.Append(",HT.HT_CONDITION ");
            selectQuery.Append(",HT.PATTERN_NO ");
            selectQuery.Append(",HT.HT_START_DATE ");
            selectQuery.Append(",HT.SETUP_CONF_BY");
            selectQuery.Append(",EM1.SHORT_NAME AS SETUP_CONF_BY_EMPLOYEE_NAME"); 
            selectQuery.Append(",HT.LOADING_CONF_BY"); 
            selectQuery.Append(",EM2.SHORT_NAME AS LOADING_CONF_BY_EMPLOYEE_NAME");
            selectQuery.Append(",HT.PLANNED_UNLOAD_DATE");
            selectQuery.Append(",HT.HT_END_DATE");
            selectQuery.Append(",HT.OFFLOADED_BY");
            selectQuery.Append(",HT.OFFLOAD_CONF_BY");
            selectQuery.Append(",HT.PROCESS_PLAN_CD");
            selectQuery.Append(",HT.COIL_BAR_TYPE");
            selectQuery.Append(",HT.HT_END_DATE");
            selectQuery.Append(",HT.LOT_NO");
            selectQuery.Append(",HT.HT_NO_OF_UNITS");
            selectQuery.Append(",HT.UNIT_TYPE");
            selectQuery.Append(",HT.HT_WEIGHT");
            selectQuery.Append(",HT.ROLL_MONTH");
            selectQuery.Append(",HT.COIL_NO");
            selectQuery.Append(",HT.LOCATION_IN_FURNACE1");
            selectQuery.Append(",HT.LOCATION_IN_FURNACE2");
            selectQuery.Append(",HT.HT_TABA_NO_1");
            selectQuery.Append(",HT.HT_TABA_NO_2");
            selectQuery.Append(",HT.HT_TABA_NO_3");
            selectQuery.Append(",HT.HT_TABA_NO_4");
            selectQuery.Append(",HT.HT_NEXT_PROCESS");
            selectQuery.Append(",TM.MSG_CONTENTS AS HT_REMARKS");
            selectQuery.Append(",HT.MSG_CD");
            selectQuery.Append(",CUST.SHORT_NAME AS CUSTOMER_NAME");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PROD_NAME_CUST ");
            selectQuery.Append(",GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,TRIM(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS CHUUBAN ");
            selectQuery.Append(" , PP.CHARGE ");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS MATL_DIM_STRING");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PROD_DIM_STRING");
            selectQuery.Append(",HT.HT_END_DATE");
            selectQuery.Append(",HT.OFFLOADED_BY");
            selectQuery.Append(",HT.OFFLOAD_CONF_BY");
            selectQuery.Append(", HT.HT_REMARK  AS HT_HEADER_REMARK");
            selectQuery.Append(", PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE_STRING");
            selectQuery.Append("  FROM TRN_HEAT_TREATMENT HT");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP  ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD=CUST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE TM ON HT.MSG_CD=TM.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EM1 ON HT.SETUP_CONF_BY = EM1.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EM2 ON HT.LOADING_CONF_BY = EM2.EMPLOYEE_CD ");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" HT.HT_END_DATE IS NULL");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            if (MACHINE_CD > 0)
            {
                selectQuery.Append(" AND HT.MACHINE_CD = '" + MACHINE_CD + "'");
            }

            // selectQuery.Append(" AND HT.HT_END_DATE " + nullCondition);
            var get = ExecuteQueryMultiResult<HeatTreatMentDetails>(selectQuery.ToString());

            return get;


        }
        public List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty)
        {
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(searchText))
            {
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD");
                selectQuery.Append(" WHERE EMP.IN_ACTIVE_FLAG =0 ");
                // selectQuery.Append(" AND ROWNUM < 50 ");          //Show All records 2019-08-26
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME) ");
            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,EMP.SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP ");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD");
                selectQuery.Append(" WHERE LOWER (EMP.YOBINA) LIKE lower('" + searchText + "%') ");
                selectQuery.Append(" AND EMP.IN_ACTIVE_FLAG = 0");
                // query.Append(" AND ROWNUM < 50 ");      //Show All records 2019-08-26
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME)");
            }
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }

        public List<HeatTreatMentDetails> GetFurnaceNoDetails(string FURNACE_NO)
        {
            StringBuilder selectQuery = new StringBuilder();
            string nullCondition = string.Empty;

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" HT.HEAT_TREATMENT_CD");
            selectQuery.Append(",HT.MACHINE_CD ");
            selectQuery.Append(",HT.FURNACE_NO ");
            selectQuery.Append(",HT.HT_CONDITION ");
            selectQuery.Append(",HT.HEAT_TREATMENT_NUMBER ");

            selectQuery.Append("  FROM TRN_HEAT_TREATMENT HT");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP  ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD=CUST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE TM ON HT.MSG_CD=TM.MSG_CD ");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" LOWER(HT.FURNACE_NO) = '" + FURNACE_NO.ToLower() + "'" );
            return ExecuteQueryMultiResult<HeatTreatMentDetails>(selectQuery.ToString());
        }
        public bool IsMachineStarted(int machineCD)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            string nullCondition = string.Empty;

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" COUNT(*) ");
           
            selectQuery.Append("  FROM TRN_HEAT_TREATMENT ");
          
            selectQuery.Append(" WHERE ");
            selectQuery.Append("MACHINE_CD = " + machineCD + " AND HT_END_DATE IS NULL");
            int getCount= Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            if (getCount > 0)
            {
                returnVal = true;
            }

            return returnVal;
        }

        public HeatTreatMentDetails GetChuubanListForInvaidCheck(string chuubanList)
        {
            List<string> result = new List<string>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT lower(CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00'))) AS CHUUBAN ");
            selectQuery.Append(" , CUST.SHORT_NAME AS CUSTOMER ");
            selectQuery.Append(" ,CUST.SHORT_NAME AS CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST");
            selectQuery.Append(" ,PP.CHARGE ");
            selectQuery.Append(" ,PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,'',PP.MATL_SHAPE,'') AS MATL_DIM_STRING");
            selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,'',PP.PROD_SHAPE,'') AS PROD_DIM_STRING");
            selectQuery.Append(" ,PP.PROD_DIM_D");
            selectQuery.Append(" ,PP.MATL_DIM_D");
            selectQuery.Append(" , PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" , PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE ");
            selectQuery.Append(" , PP.PROCESS_PLAN_START_DATE ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD");
            //selectQuery.Append(" WHERE lower(CHUUBAN||GYOUBAN)  IN (" + chuubanList + ")");
            selectQuery.Append(" WHERE LOWER(CHUUBAN||TRIM(LEADING ' ' FROM TO_CHAR(GYOUBAN,'00')))  IN ('" + chuubanList + "') AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            //var result = ExecuteQueryMultiResult<HeatTreatMentDetails>(selectQuery.ToString());
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    result.Add(Convert.ToString(dt.Rows[i][0]));
            //}
            return ExecuteQuerySingleResult<HeatTreatMentDetails>(selectQuery.ToString());
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

        public List<ProductOrderCompletion> GetSearchedProductOrderDetails(List<string> chubanList, int completeFlag)
        {
            List<ProductOrderCompletion> lstAllProductOrderCompletion = new List<ProductOrderCompletion>();
            foreach (var chubanGyuban in chubanList)
            {
                if (chubanGyuban.Length == 11)
                {
                    string chuubanNo = chubanGyuban.Substring(0, 9);
                    string gyoubanNo = chubanGyuban.Substring(9, 2);

                    string value = string.Empty;
                    if (completeFlag == 0)
                    {
                        value = " IS NULL ";
                    }
                    else
                    {
                        value = " IS NOT NULL ";
                    }


                    StringBuilder selectQuery = new StringBuilder();


                    selectQuery.Append(" SELECT * ");
                    selectQuery.Append(" FROM (SELECT  GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS CHUUBAN ");
                    selectQuery.Append(" , PP.PROCESS_PLAN_CD ");
                    selectQuery.Append(" , CUST.SHORT_NAME AS CUSTOMER ");
                    selectQuery.Append(" , PP.DELIVERY_DATE AS DELIVERY_DATE ");
                    selectQuery.Append(" , PP.PROD_NAME_CUST AS PRODUCT_NAME_CUST ");
                    selectQuery.Append(" , CON.CONDITION_DESCRIPTION AS PRODUCT_CONDITION ");
                    //selectQuery.Append(" , PP.PROD_DIM_D AS DIM_D ");

                    selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(pp.prod_dim_d,PP.PROD_DIM_W,'' ");
                    selectQuery.Append(" , CASE WHEN PP.PROD_DIM_OTHER IS NOT NULL THEN PP.PROD_DIM_OTHER END || CASE WHEN PP.CHAMFER IS NOT NULL AND PP.PROD_DIM_OTHER IS NOT NULL ");
                    selectQuery.Append(" THEN ',C-' || PP.CHAMFER END || CASE WHEN PP.CHAMFER IS NOT NULL AND PP.PROD_DIM_OTHER IS NULL THEN 'C-' || PP.CHAMFER END || ");
                    selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL AND(PP.CHAMFER IS NOT NULL OR PP.PROD_DIM_OTHER  IS NOT NULL) THEN ',R-' || PP.CORNER_RADIUS END || ");
                    selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL AND PP.CHAMFER IS NULL AND PP.PROD_DIM_OTHER IS NULL THEN 'R-' || PP.CORNER_RADIUS END) AS PROD_DIM_D_STRING ");

                    selectQuery.Append(" , PP.PROD_SHAPE AS SHAPE ");
                    selectQuery.Append(" , PP.PROD_LENGTH AS PRODUCT_LENGTH ");
                    selectQuery.Append(" ,NVL( PP.LENGTH_OTHER , PP.PROD_LENGTH) AS PRODUCT_LENGTH_STRING ");

                    /*selectQuery.Append(" , PP.DIM_TOL_MAX AS DIM_TOL_MAX ");
                    selectQuery.Append(" , PP.DIM_TOL_MIN AS DIM_TOL_MIN ");
                    selectQuery.Append(" , PP.LENGTH_TOL_MAX AS LENGTH_TOLERANCE_MAX ");
                    selectQuery.Append(" , PP.LENGTH_TOL_MIN AS LENGTH_TOLERANCE_MIN ");*/
                    selectQuery.Append(",TO_CHAR(PP.MAGARI_KUBUN) AS MAGARI_KUBUN");
                    selectQuery.Append(", PP.MAGARI_BEND_TOL AS BEND_TOL");
                    selectQuery.Append(", PP.MAGARI_SPAN");



                    selectQuery.Append(" , PP.MATL_NAME_MAK AS MATL_MAKER_NAME ");
                    selectQuery.Append(" , PP.MATL_DIM_D AS MATL_DIM_D ");
                    selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,'','') AS MATL_DIM_STRING");
                    selectQuery.Append(" , GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'','') AS PROD_DIM_STRING");
                    selectQuery.Append(" , PP.CHARGE ");
                    selectQuery.Append(" , PP.PROD_SPEC_NO ");
                    selectQuery.Append(" , PP.PROD_NAME_CUST ");
                    selectQuery.Append(" , PP.ACT_ALLOC_DATE AS ACT_ALLOC_DATE ");
                    selectQuery.Append(" , TO_CHAR( PP.NO_OF_UNITS) AS NO_OF_UNITS");
                    selectQuery.Append(" ,  TO_CHAR(PP.UNIT) AS UNIT");
                    selectQuery.Append(" ,  PP.ACT_ALLOC_QTY  AS ACT_ALLOC_QTY ");
                    // selectQuery.Append(" , NVL (CH_FINISHED_QTY (PP.PROCESS_PLAN_CD),0) AS FINISHED_QTY ");
                    selectQuery.Append(" , FI1.FINISHED_QTY ");
                    selectQuery.Append(" , ROUND(NVL(( ( (NVL (CH_FINISHED_QTY (PP.CHUUBAN), 0)) / CH_ACT_QTY (PP.KAKOUIRAI_CD)) * 100),0)) AS COMPLETEPER ");
                    selectQuery.Append(" , PP.PROCESS_PLAN_CMPL_DATE");
                    selectQuery.Append(" , CASE WHEN  PP.DIM_TOL_MAX_CHR IS NOT NULL THEN");
                    selectQuery.Append("   (PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR) END AS DIM_TOLERANCE");
                    selectQuery.Append(" , CASE WHEN  PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN");
                    selectQuery.Append("   (PP.LENGTH_TOL_MAX_CHR ||','||PP.LENGTH_TOL_MIN_CHR) END AS LEN_TOL");
                    //selectQuery.Append(" ,    PP.BEND_TOL");
                    //selectQuery.Append(" , PP.DECARB_1 ");
                    //selectQuery.Append(" , PP.DECARB_VAL_1 ");
                    //selectQuery.Append(" , PP.DECARB_2 ");
                    //selectQuery.Append(" , PP.DECARB_VAL_2 ");
                    //selectQuery.Append(" , PP.DECARB_3 ");
                    //selectQuery.Append(" , PP.DECARB_VAL_3 ");
                    selectQuery.Append(" FROM TRN_PROCESS_PLAN PP  ");
                    selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PP.CUSTOMER_CD = CUST.CUSTOMER_CD");
                    selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD ");
                    selectQuery.Append(" LEFT JOIN (SELECT   PP1.PROCESS_PLAN_CD, SUM (FI.FI_QTY) AS FINISHED_QTY ");
                    selectQuery.Append(" FROM TRN_PROCESS_PLAN PP1 INNER JOIN TRN_FINAL_INSPECTION FI ");
                    selectQuery.Append(" ON FI.PROCESS_PLAN_CD = PP1.PROCESS_PLAN_CD ");
                    selectQuery.Append("  WHERE FI.COMBINED_DECISION = 1 ");
                    selectQuery.Append(" GROUP BY PP1.PROCESS_PLAN_CD) FI1 ");
                    selectQuery.Append(" ON PP.PROCESS_PLAN_CD = FI1.PROCESS_PLAN_CD ");



                    selectQuery.Append(" WHERE PP.CHUUBAN = '" + chuubanNo + "' AND PP.GYOUBAN = '" + gyoubanNo + "' ");
                    // selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE = " + completeFlag );
                    selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE " + value + ")");
                    //  selectQuery.Append(" AND PP.PROCESS_PLAN_INACTIVE=0 ");
                    //selectQuery.Append(" ORDER BY PP.CHUUBAN, CUST.YOBINA, CUST.YOBIGANA, CUST.SHORT_NAME, PP.PROD_NAME_CUST,PP.PROD_SHAPE, PP.PROD_DIM_D,PP.PROD_LENGTH,PP.PROD_SPEC_NO) ");     // Pradeep More  [22/11/2017] :: SORT ORDER: CHUUBAN,CUSTOMER,PROD NAME CUST,PROD DIM D,PROD SHAPE,PROD LENGTH,PROD SPEC NO.
                    //selectQuery.Append(" )");

                    var productOrderCompletion = ExecuteQuerySingleResult<ProductOrderCompletion>(selectQuery.ToString());
                    if (productOrderCompletion != null)
                    {
                        lstAllProductOrderCompletion.Add(productOrderCompletion);
                    }
                }
            }
            return lstAllProductOrderCompletion;
        }

        public bool IsFurProcFurNoIsExist(int MACHINE_CD, string FURNACE_NO)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  * FROM");
            selectQuery.Append(" TRN_HEAT_TREATMENT");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MACHINE_CD = '" + MACHINE_CD + "'");
            selectQuery.Append(" AND  FURNACE_NO = '" + FURNACE_NO + "'");
            var result = ExecuteQueryMultiResult<HeatTreatMentDetails>(selectQuery.ToString());
            if (result.Count > 0)
                return true;
            else
                return false;
        }

        public List<HeatTreatMentDetails> InsertStartData(HeatTreatMentDetails heatTreatMentDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append("INSERT INTO TRN_HEAT_TREATMENT ( ");
            insertQuery.Append(" HEAT_TREATMENT_CD ");
            insertQuery.Append(",HEAT_TREATMENT_ROW ");
            insertQuery.Append(",HEAT_TREATMENT_NUMBER ");
            insertQuery.Append(",LOCATION_CD ");
            insertQuery.Append(", MACHINE_CD ");
            insertQuery.Append(",FURNACE_NO ");
            insertQuery.Append(",HT_CONDITION ");
            insertQuery.Append(",PATTERN_NO ");
            insertQuery.Append(",HT_START_DATE ");
            insertQuery.Append(",SETUP_CONF_BY");
            insertQuery.Append(",LOADING_CONF_BY");
            insertQuery.Append(",PLANNED_UNLOAD_DATE");
            insertQuery.Append(",HT_REMARK");
            insertQuery.Append(",PROCESS_PLAN_CD");
            insertQuery.Append(",COIL_BAR_TYPE");
            insertQuery.Append(",LOT_NO");
            insertQuery.Append(",HT_NO_OF_UNITS");
            insertQuery.Append(",UNIT_TYPE");
            insertQuery.Append(",HT_WEIGHT");
            insertQuery.Append(",ROLL_MONTH");
            insertQuery.Append(",COIL_NO");
            insertQuery.Append(",LOCATION_IN_FURNACE1");
            insertQuery.Append(",LOCATION_IN_FURNACE2");
            insertQuery.Append(",HT_TABA_NO_1");
            insertQuery.Append(",HT_TABA_NO_2");
            insertQuery.Append(",HT_TABA_NO_3");
            insertQuery.Append(",HT_TABA_NO_4");
            insertQuery.Append(",HT_NEXT_PROCESS");
            insertQuery.Append(",MSG_CD");
            insertQuery.Append(",INPUT_LOCATION_CD");
            insertQuery.Append(",PROCESS_KUBUN_CD");
            insertQuery.Append(",USER_ID");
            insertQuery.Append(",CREATED_DATE");
            insertQuery.Append(",UPDATED_DATE");

            insertQuery.Append(") VALUES (");
            insertQuery.Append(" TRN_HEAT_TREATMENT_SEQ.NEXTVAL");
            insertQuery.Append(",'" + heatTreatMentDetails.HEAT_TREATMENT_ROW + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HEAT_TREATMENT_NUMBER + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.LOCATION_CD + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.FURNACE_NO + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_CONDITION + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.PATTERN_NO + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(heatTreatMentDetails.HT_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + heatTreatMentDetails.SETUP_CONF_BY + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.LOADING_CONF_BY + "'");
            if(heatTreatMentDetails.PLANNED_UNLOAD_DATE == null)
            {
                insertQuery.Append(", NULL");
            }
            else
            {
                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(heatTreatMentDetails.PLANNED_UNLOAD_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            }
           
            insertQuery.Append(",'" + heatTreatMentDetails.HT_HEADER_REMARK + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.COIL_BAR_TYPE + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.LOT_NO + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_NO_OF_UNITS + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.UNIT_TYPE + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_WEIGHT + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.ROLL_MONTH + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.COIL_NO + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.LOCATION_IN_FURNACE1 + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.LOCATION_IN_FURNACE2 + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_TABA_NO_1 + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_TABA_NO_2 + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_TABA_NO_3 + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_TABA_NO_4 + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.HT_NEXT_PROCESS + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.MSG_CD + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.PROCESS_KUBUN_ID + "'");
            insertQuery.Append(",'" + heatTreatMentDetails.USER_ID + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());


            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  HEAT_TREATMENT_ROW,HEAT_TREATMENT_CD, PROCESS_PLAN_CD FROM");
            selectQuery.Append(" TRN_HEAT_TREATMENT");

            selectQuery.Append(" WHERE ");
            //   selectQuery.Append(" HEAT_TREATMENT_ROW = '" + heatTreatMentDetails.HEAT_TREATMENT_ROW + "'");
            selectQuery.Append("  FURNACE_NO = '" + heatTreatMentDetails.FURNACE_NO + "'");
            return  ExecuteQueryMultiResult<HeatTreatMentDetails>(selectQuery.ToString());


        }
        public long UpdateEndData(HeatTreatMentDetails heatTreatMentDetails)
        {

            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_HEAT_TREATMENT SET ");

            updateQuery.Append(" MACHINE_CD = '" + heatTreatMentDetails.MACHINE_CD + "'");
            updateQuery.Append(",FURNACE_NO = '" + heatTreatMentDetails.FURNACE_NO + "'");
            updateQuery.Append(",HT_CONDITION ='" + heatTreatMentDetails.HT_CONDITION + "'");
            updateQuery.Append(",PATTERN_NO ='" + heatTreatMentDetails.PATTERN_NO + "'");
            updateQuery.Append(",HT_START_DATE = TO_DATE('" + Convert.ToDateTime(heatTreatMentDetails.HT_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(",SETUP_CONF_BY ='" + heatTreatMentDetails.SETUP_CONF_BY + "'");
            updateQuery.Append(",LOADING_CONF_BY ='" + heatTreatMentDetails.LOADING_CONF_BY + "'");
            updateQuery.Append(",PLANNED_UNLOAD_DATE = TO_DATE('" + Convert.ToDateTime(heatTreatMentDetails.PLANNED_UNLOAD_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(",HT_REMARK = '" + heatTreatMentDetails.HT_HEADER_REMARK + "'");


            updateQuery.Append(",HT_END_DATE = TO_DATE('" + Convert.ToDateTime(heatTreatMentDetails.HT_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(",OFFLOADED_BY = '" + heatTreatMentDetails.OFFLOADED_BY + "'");
            updateQuery.Append(",OFFLOAD_CONF_BY ='" + heatTreatMentDetails.OFFLOAD_CONF_BY + "'");

            updateQuery.Append(",PROCESS_PLAN_CD ='" + heatTreatMentDetails.PROCESS_PLAN_CD + "'");
            updateQuery.Append(",COIL_BAR_TYPE ='" + heatTreatMentDetails.COIL_BAR_TYPE + "'");
            updateQuery.Append(",LOT_NO='" + heatTreatMentDetails.LOT_NO + "'");
            updateQuery.Append(",HT_NO_OF_UNITS ='" + heatTreatMentDetails.HT_NO_OF_UNITS + "'");
            updateQuery.Append(",UNIT_TYPE ='" + heatTreatMentDetails.UNIT_TYPE + "'");
            updateQuery.Append(",HT_WEIGHT ='" + heatTreatMentDetails.HT_WEIGHT + "'");
            updateQuery.Append(",ROLL_MONTH ='" + heatTreatMentDetails.ROLL_MONTH + "'");
            updateQuery.Append(",COIL_NO ='" + heatTreatMentDetails.COIL_NO + "'");
            updateQuery.Append(",LOCATION_IN_FURNACE1 ='" + heatTreatMentDetails.LOCATION_IN_FURNACE1 + "'");
            updateQuery.Append(",LOCATION_IN_FURNACE2 ='" + heatTreatMentDetails.LOCATION_IN_FURNACE2 + "'");
            updateQuery.Append(",HT_TABA_NO_1 ='" + heatTreatMentDetails.HT_TABA_NO_1 + "'");
            updateQuery.Append(",HT_TABA_NO_2 ='" + heatTreatMentDetails.HT_TABA_NO_2 + "'");
            updateQuery.Append(",HT_TABA_NO_3 ='" + heatTreatMentDetails.HT_TABA_NO_3 + "'");
            updateQuery.Append(",HT_TABA_NO_4 ='" + heatTreatMentDetails.HT_TABA_NO_4 + "'");
            updateQuery.Append(",HT_NEXT_PROCESS ='" + heatTreatMentDetails.HT_NEXT_PROCESS + "'");
            updateQuery.Append(",MSG_CD ='" + heatTreatMentDetails.MSG_CD + "'");
            updateQuery.Append(",INPUT_LOCATION_CD = '" + heatTreatMentDetails.INPUT_LOCATION_CD + "'");
            updateQuery.Append(",USER_ID ='" + heatTreatMentDetails.USER_ID + "'");

            updateQuery.Append(",UPDATED_DATE = ");
            updateQuery.Append("SYSDATE");

            updateQuery.Append(" WHERE ");
          //  updateQuery.Append(" MACHINE_CD = '" + heatTreatMentDetails.MACHINE_CD + "'");
           // updateQuery.Append(" AND  FURNACE_NO = '" + heatTreatMentDetails.FURNACE_NO + "'");
            updateQuery.Append(" HEAT_TREATMENT_CD = '" + heatTreatMentDetails.HEAT_TREATMENT_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());

            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_HEAT_TREATMENT WHERE HEAT_TREATMENT_CD= '" + heatTreatMentDetails.HEAT_TREATMENT_CD + "'");
            long msgCd = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCd = Convert.ToInt64(cdVal);
            }
            return msgCd;
        }
        public void Delete(int HEAT_TREATMENT_NUMBER, string FURNACE_NO)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_HEAT_TREATMENT ");
            deleteQuery.Append(" WHERE HEAT_TREATMENT_NUMBER =" + HEAT_TREATMENT_NUMBER + " AND FURNACE_NO = '" + FURNACE_NO + "'");
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public List<HeatTreatMentDetails> GetHeatTreatmentDetails(string sortCriteria, List<SearchField> searchCriteria, short locationCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            // selectQuery.Append(" SELECT FURNACE_NO FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" HT.HEAT_TREATMENT_NUMBER AS HEAT_TREATMENT_NUMBER");
            selectQuery.Append(" ,MAC.MACHINE_NAME AS MACHINE_NAME");
            selectQuery.Append(",HT.FURNACE_NO ");
            selectQuery.Append(",HT.HT_CONDITION ");
            selectQuery.Append(", HT.PATTERN_NO ");
            selectQuery.Append(",HT.HT_START_DATE AS HT_START_DATE");
            selectQuery.Append(",TO_CHAR(HT.HT_START_DATE, 'yyyy/mm/dd') AS HT_START_DATE_STRING");
            selectQuery.Append(",HT.HT_END_DATE AS HT_END_DATE");
            selectQuery.Append(",TO_CHAR(HT.HT_END_DATE, 'yyyy/mm/dd') AS HT_END_DATE_STRING");
            selectQuery.Append(", HT.MACHINE_CD  AS MACHINE_CD");
           // selectQuery.Append(", HT.HT_REMARK  AS HT_HEADER_REMARK");
            selectQuery.Append("  FROM TRN_HEAT_TREATMENT HT");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD=HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD=HT.MACHINE_CD ");
            selectQuery.Append(" WHERE HT_END_DATE IS NOT NULL ");
           // selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
      //      selectQuery.Append(" AND HT.INPUT_LOCATION_CD = " + locationCD);

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

                query = query + whereCondition + " GROUP BY HT.HEAT_TREATMENT_NUMBER, HT.FURNACE_NO,MAC.MACHINE_NAME,HT.HT_CONDITION,HT.PATTERN_NO,HT.HT_START_DATE,HT.HT_END_DATE,HT.MACHINE_CD ";
            }
            else
            {
                query = query + " GROUP BY HT.HEAT_TREATMENT_NUMBER, HT.FURNACE_NO,MAC.SHORT_NAME,HT.HT_CONDITION,HT.PATTERN_NO,HT.HT_START_DATE,HT.HT_END_DATE,HT.MACHINE_CD,HT.HT_REMARK ";
            }
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }
            //else { 
            //query += " ORDER BY HT.FURNACE_NO DESC, HT.MACHINE_CD ASC,HT.HT_START_DATE DESC";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return ExecuteQueryMultiResult<HeatTreatMentDetails>(query);
        }


        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/01/06       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Heat Treatment process data according to search criteria        
        /// </returns>
        /// 
        public List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCiteria,int rowLimit)
        {
            List<HeatTreatMentDetails> result = new List<HeatTreatMentDetails>();
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOCA.SHORT_NAME AS PROC_LOCATION_NAME,");
            selectQuery.Append(" MAC.MACHINE_NAME AS MACHINE_NAME,");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS OFFLOAD_CONF_BY_EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST,");
            selectQuery.Append(" PP.CHARGE,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,"); 
            selectQuery.Append(" HT.LOT_NO,");
            selectQuery.Append(" HT.HT_NO_OF_UNITS,");
            selectQuery.Append(" HT.UNIT_TYPE AS UNIT_TYPE,");
            selectQuery.Append(" HT.HT_WEIGHT,");
            selectQuery.Append(" HT.ROLL_MONTH,");
            selectQuery.Append(" HT.COIL_NO,");
            selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO,");
            selectQuery.Append(" HT.PATTERN_NO,");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE1 ||'-'|| HT.LOCATION_IN_FURNACE2 AS LOCATION_NAME,");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE1,");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE2,");
            selectQuery.Append(" HT.HT_CONDITION,");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS PP_PROCESS_PLAN_REMARK,");
            selectQuery.Append(" HT.HT_REMARK AS HT_REMARKS,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,'' ) AS MATERIAL_DIA,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_SHAPE,'' ) AS PRODUCT_DIA,");
            selectQuery.Append(" INTE.INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_LENGTH,");
            selectQuery.Append(" HT.HT_END_DATE,");
            selectQuery.Append(" TO_CHAR(HT.HT_END_DATE, 'YYYY/MM/DD HH24:MI') AS HT_END_DATE_STRING,");
            selectQuery.Append(" HT.OFFLOAD_CONF_BY AS OFFLOAD_CONF_BY,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS CHUUBAN, ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");
            //selectQuery.Append(" M.MSG_CONTENTS AS HT_REMARKS ");
            selectQuery.Append(" FROM TRN_HEAT_TREATMENT HT INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.MACHINE_CD ");
            // selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = HT.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE CONFSTARTEMP ON CONFSTARTEMP.EMPLOYEE_CD = HT.LOADING_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE ENDEMP ON ENDEMP.EMPLOYEE_CD = HT.OFFLOADED_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE CONFENDEMP ON CONFENDEMP.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = HT.HT_NEXT_PROCESS");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = HT.MSG_CD ");


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
            //if (!string.IsNullOrEmpty(sortCiteria))
            //{
            //    query = query + " ORDER BY " + sortCiteria;
            //}
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            //else
            //{
            //    query += " ORDER BY HT_END_DATE_STRING,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,CHUUBAN,FURNACE_NO,HT.HT_CONDITION,HT.PATTERN_NO";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<HeatTreatMentDetails>(query);
            return result;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2023/05/22       
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Heat Treatment process data according to search criteria        
        /// </returns>
        /// 
        public List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnDailyProcessAnalysis(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_PROD_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
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
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING, ");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER, ");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN , ");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS NEJIRE_SPAN, ");
            selectQuery.Append(" PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN, ");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" PP.UNIT_WT_MIN AS UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE , ");
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
            selectQuery.Append(" PP.TEST_SPECIMEN AS TEST_SPECIMEN , ");
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
            selectQuery.Append(" PP.HARDNESS_PATTERN3_ACTUAL , ");
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
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL, ");
            //77 not in excel,
            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
            // --79,80 not in excel
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK, ");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");            
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE , ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PROCESS_PLAN_KUBUN, ");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.KAKOUIARI_QTY AS KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PROCESS_PLAN_PRINT_DATE_STRING, ");
            //HeatTreatment
            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS MACHINE_NAME, ");
            selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO,  ");
            //4
            selectQuery.Append(" HT.PATTERN_NO AS PATTERN_NO, ");
            selectQuery.Append(" TO_CHAR(HT.HT_START_DATE, 'yyyy/mm/dd HH24:MI') AS HT_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS SETUP_CONF_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS LOADING_CONF_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" TO_CHAR(HT.PLANNED_UNLOAD_DATE, 'yyyy/mm/dd HH24:MI') AS PLANNED_UNLOAD_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(HT.HT_END_DATE, 'yyyy/mm/dd HH24:MI') AS HT_END_DATE_STRING, ");
            selectQuery.Append(" EMP3.SHORT_NAME AS OFFLOADED_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" EMP4.SHORT_NAME AS OFFLOAD_CONF_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" HT.COIL_BAR_TYPE AS COIL_BAR_TYPE, ");
            selectQuery.Append(" HT.LOT_NO AS LOT_NO, ");
            selectQuery.Append(" HT.HT_NO_OF_UNITS AS HT_NO_OF_UNITS, ");
            selectQuery.Append(" HT.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" HT.HT_WEIGHT AS HT_WEIGHT, ");
            selectQuery.Append(" HT.ROLL_MONTH AS ROLL_MONTH, ");
            selectQuery.Append(" HT.COIL_NO AS COIL_NO, ");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE1 AS LOCATION_IN_FURNACE1, ");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE2 AS LOCATION_IN_FURNACE2, ");
            selectQuery.Append(" HT.HT_TABA_NO_1 AS HT_TABA_NO_1, ");
            selectQuery.Append(" HT.HT_TABA_NO_2 AS HT_TABA_NO_2, ");
            selectQuery.Append(" HT.HT_TABA_NO_3 AS HT_TABA_NO_3, ");
            selectQuery.Append(" HT.HT_TABA_NO_4 AS HT_TABA_NO_4, ");
            selectQuery.Append(" HT.HT_CONDITION AS HT_CONDITION, ");
            selectQuery.Append(" MESS.MSG_CONTENTS AS PP_PROCESS_PLAN_REMARK,");
            selectQuery.Append(" HT.HT_REMARK AS HT_REMARKS");


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HEAT_TREATMENT HT ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");            
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = HT.SETUP_CONF_BY ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = HT.LOADING_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP3 ON EMP3.EMPLOYEE_CD = HT.OFFLOADED_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP4 ON EMP4.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
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
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = HT.MSG_CD ");

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
            return ExecuteQueryMultiResult<HeatTreatMentDetails>(query);
        }

        public int GetHTNumDetails()
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" nvl(max(HEAT_TREATMENT_NUMBER), 0) + 1 AS HEAT_TREATMENT_NUMBER");
            selectQuery.Append(" FROM TRN_HEAT_TREATMENT");
            var result = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return result;
        }
        public List<NameValue> GetMachineNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }

        public List<NameValue> GetHeatTreatmentMachineNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" AND MACHINE_TYPE = 20");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/03       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForHeatTreatmentFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  COUNT(*)");            
            selectQuery.Append(" FROM TRN_HEAT_TREATMENT HT INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = HT.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE CONFSTARTEMP ON CONFSTARTEMP.EMPLOYEE_CD = HT.LOADING_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE ENDEMP ON ENDEMP.EMPLOYEE_CD = HT.OFFLOADED_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE CONFENDEMP ON CONFENDEMP.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = HT.HT_NEXT_PROCESS");

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
        public int GetRowCountForHeatTreatmentDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) "); 
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HEAT_TREATMENT HT ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = HT.SETUP_CONF_BY ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = HT.LOADING_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP3 ON EMP3.EMPLOYEE_CD = HT.OFFLOADED_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP4 ON EMP4.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
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
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = HT.MSG_CD ");

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

        public List<NameValue> GetLocationNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_LOCATION WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }

        public List<NameValue> GetFactoryLocationNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_LOCATION WHERE IN_ACTIVE_FLAG = 0 AND LOCATION_KUBUN =" +UIConstants.K_LOC_FACTORY + " "); //40 : Factory Kubun

            query.Append(" ORDER BY LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/05       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// To bind machine name of HeatTreatment process
        /// </returns>
        /// 
        public List<NameValue> GetHeatTreatmentMachineList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" AND MACHINE_TYPE ='"+UIConstants.K_MACHINEGROUP_HEAT_TREATMENT+"'");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }

    }
}
