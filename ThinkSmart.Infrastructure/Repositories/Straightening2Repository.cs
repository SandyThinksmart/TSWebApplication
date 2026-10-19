using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class Straightening2Repository : BaseRepository, IStraightening2Repository
    {
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public Straightening2Repository(IConfiguration _configuration,LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }

        public List<Straightening2Details> GetStraightening2List(List<SearchField> searchCriteria, string sortCiteria, long? inputLocationCD)
        {
            List<Straightening2Details> result = new List<Straightening2Details>();
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(", PP.CHUUBAN AS PP_CHUUBAN ");
            selectQuery.Append(", PP.GYOUBAN AS PP_GYOUBAN ");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            selectQuery.Append(",PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO ");
            selectQuery.Append(",TO_CHAR (ST.STR_START_DATE,'yyyy/mm/dd hh24:mi')  AS STR_START_DATE_TIME ");
            selectQuery.Append(",TO_CHAR(ST.STR_END_DATE,'yyyy/mm/dd hh24:mi') AS STR_END_DATE_STRING ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS STR_START_OPERATOR_NAME");
            selectQuery.Append(",(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_END_OPERATOR_CD) AS STR_END_OPERATOR_NAME");
            selectQuery.Append(",CST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            // selectQuery.Append(",PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(",GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W , null, CASE ");
            selectQuery.Append(" WHEN PP.PROD_DIM_OTHER IS NOT NULL  THEN PP.PROD_DIM_OTHER ");
            selectQuery.Append(" END  || CASE  WHEN PP.CHAMFER       IS NOT NULL   AND PP.PROD_DIM_OTHER IS NOT NULL ");
            selectQuery.Append(" THEN ',C-'      || PP.CHAMFER END  ||     CASE ");
            selectQuery.Append(" WHEN PP.CHAMFER       IS NOT NULL    AND PP.PROD_DIM_OTHER IS NULL ");
            selectQuery.Append(" THEN 'C-'   || PP.CHAMFER    END   || ");
            selectQuery.Append(" CASE   WHEN PP.CORNER_RADIUS IS NOT NULL  AND(PP.CHAMFER      IS NOT NULL ");
            selectQuery.Append(" OR PP.PROD_DIM_OTHER  IS NOT NULL)  THEN ',R-'     || PP.CORNER_RADIUS ");
            selectQuery.Append(" END ||  CASE    WHEN PP.CORNER_RADIUS IS NOT NULL ");
            selectQuery.Append(" AND PP.CHAMFER        IS NULL    AND PP.PROD_DIM_OTHER IS NULL ");
            selectQuery.Append(" THEN 'R-'   || PP.CORNER_RADIUS END) AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE");
            //selectQuery.Append(",PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            //  selectQuery.Append(",PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(",GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D, PP.MATL_DIM_W , MATL_SHAPE, PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_String");
            selectQuery.Append(",PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(",ST.STR_METALLIC_PLATE_CODE AS STR_METALLIC_PLATE_CODE ");
            selectQuery.Append(",ST.STR_TABA_NO_1 ");

            selectQuery.Append(" , CONCAT ");
            selectQuery.Append(" (ST.STR_TABA_NO_1, ");
            selectQuery.Append(" CONCAT ");
            selectQuery.Append(" (CONCAT  ");
            selectQuery.Append(" (CASE ");
            selectQuery.Append(" WHEN ST.STR_TABA_NO_2 IS NOT NULL ");
            selectQuery.Append(" THEN CONCAT(', ', ST.STR_TABA_NO_2) ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ST.STR_TABA_NO_3 IS NOT NULL ");
            selectQuery.Append(" THEN CONCAT(', ', ST.STR_TABA_NO_3) ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ST.STR_TABA_NO_4 IS NOT NULL ");
            selectQuery.Append(" THEN CONCAT(', ', ST.STR_TABA_NO_4) ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ");
            selectQuery.Append(" ) AS STR_TABA_NUMBERS ");
            selectQuery.Append(",ST.STR_BARS AS STR_BARS");
            selectQuery.Append(",ST.UNIT_TYPE AS STR_UNIT_TYPE");
            selectQuery.Append(", ST.STR_QTY  ");
            selectQuery.Append(" ,ST.UNIT_TYPE As STR_UNIT_TYPE");
            selectQuery.Append(" ,ST.MACHINE_CD AS STR_MACHINE_CD ");
            selectQuery.Append(",(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD =ST.MACHINE_CD ) AS STR_MACHINE_NAME");
             selectQuery.Append(",ST.STR_BEND_STATUS");
            selectQuery.Append(",ST.STR_EXT_TEXTURE");
            selectQuery.Append(",ST.STR_DECISION AS STR_DECISION");
            selectQuery.Append(", ST.RESERVATION_NUMBER AS STR_RESERVATION_NO ");
            selectQuery.Append(", ST.STRAIGHTENING_CD AS STRAIGHTENING_CD");
            selectQuery.Append(", INTE.INTERNAL_NAME");
            //sub query for count row no..
            selectQuery.Append(" ,(SELECT COUNT(PP.PROCESS_PLAN_CD)");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN P INNER JOIN TRN_STRAIGHTENING2 S ");
            selectQuery.Append(" ON P.PROCESS_PLAN_CD = S.PROCESS_PLAN_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EM ON S.STR_START_OPERATOR_CD = EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE S.STR_END_OPERATOR_CD IS NOT NULL ) ");
            selectQuery.Append(" AS ROW_COUNT ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_STRAIGHTENING2  ST  ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD=ST.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ");
            selectQuery.Append(" ON ST.STR_START_OPERATOR_CD=EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD= CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD =PP.INTERNAL_NAME_CD");


            selectQuery.Append(" WHERE ST.INPUT_LOCATION_CD= " + inputLocationCD + " AND ST.STR_END_DATE IS NOT NULL ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
           // selectQuery.Append(" AND ROWNUM <= ");
          //  selectQuery.Append(" (CASE");
          //  selectQuery.Append(" WHEN (ROWNUM >" + _maxRowLimit + ")  THEN " + _maxRowLimit + "");
            //selectQuery.Append(" ELSE ROWNUM");
           // selectQuery.Append(" END)");

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
            //else
            //{
            //    query += "ORDER BY PP.CHUUBAN ASC,PP.GYOUBAN ASC, ST.STR_TABA_NO_1 ASC";
            //}
            result = ExecuteQueryMultiResult<Straightening2Details>(query);
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
        public Straightening2Details GetStraightening2DetailsByCD(long StraighteningCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            //selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  ");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            //selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");    //AND PR.VER = PP.PROD_VER
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
           // selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");

            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(", PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //selectQuery.Append(", K.REMARKS 	AS	PP_KAKOUIRAI_REMARKS  ");  
            selectQuery.Append(",  PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(", PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");

            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE, ");

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

            selectQuery.Append(" P1.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_1, ");
            selectQuery.Append(" P2.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_2, ");
            selectQuery.Append(" P3.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_3, ");
            selectQuery.Append(" P4.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_4, ");
            selectQuery.Append(" P5.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_5, ");
            selectQuery.Append(" P6.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_6, ");
            selectQuery.Append(" P7.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_7, ");
            selectQuery.Append(" P8.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_8, ");
            selectQuery.Append(" P9.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_9, ");
            selectQuery.Append(" P10.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_10, ");
            selectQuery.Append(" P11.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_11, ");
            selectQuery.Append(" P12.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_12, ");
            selectQuery.Append(" P13.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_13, ");

            selectQuery.Append(" P1.PROCESS_PROD_POINT AS PP_PROD_POINT_1, ");
            selectQuery.Append(" P2.PROCESS_PROD_POINT AS PP_PROD_POINT_2, ");
            selectQuery.Append(" P3.PROCESS_PROD_POINT AS PP_PROD_POINT_3, ");
            selectQuery.Append(" P4.PROCESS_PROD_POINT AS PP_PROD_POINT_4, ");
            selectQuery.Append(" P5.PROCESS_PROD_POINT AS PP_PROD_POINT_5, ");
            selectQuery.Append(" P6.PROCESS_PROD_POINT AS PP_PROD_POINT_6, ");
            selectQuery.Append(" P7.PROCESS_PROD_POINT AS PP_PROD_POINT_7, ");
            selectQuery.Append(" P8.PROCESS_PROD_POINT AS PP_PROD_POINT_8, ");
            selectQuery.Append(" P9.PROCESS_PROD_POINT AS PP_PROD_POINT_9, ");
            selectQuery.Append(" P10.PROCESS_PROD_POINT AS PP_PROD_POINT_10, ");
            selectQuery.Append(" P11.PROCESS_PROD_POINT AS PP_PROD_POINT_11, ");
            selectQuery.Append(" P12.PROCESS_PROD_POINT AS PP_PROD_POINT_12, ");
            selectQuery.Append(" P13.PROCESS_PROD_POINT AS PP_PROD_POINT_13, ");

            selectQuery.Append(" P1.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_1, ");
            selectQuery.Append(" P2.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_2, ");
            selectQuery.Append(" P3.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_3, ");
            selectQuery.Append(" P4.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_4, ");
            selectQuery.Append(" P5.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_5, ");
            selectQuery.Append(" P6.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_6, ");
            selectQuery.Append(" P7.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_7, ");
            selectQuery.Append(" P8.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_8, ");
            selectQuery.Append(" P9.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_9, ");
            selectQuery.Append(" P10.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_10, ");
            selectQuery.Append(" P11.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_11, ");
            selectQuery.Append(" P12.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_12, ");
            selectQuery.Append(" P13.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_13, ");
            selectQuery.Append(" PP.NYUKA_CD ");

            selectQuery.Append(" ,STR.STRAIGHTENING_CD   ");
            selectQuery.Append(" ,STR.PROCESS_PLAN_CD  ");
            selectQuery.Append(" ,STR.MACHINE_CD  ");
            selectQuery.Append(",(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD =STR.MACHINE_CD ) AS STR_MACHINE_NAME");
            selectQuery.Append(" ,TO_CHAR( STR.STR_START_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_START_DATE_STRING ");
            selectQuery.Append(" , STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
            selectQuery.Append(", STR.TP_TEST	  ");
            selectQuery.Append(" , STR.STR_METALLIC_PLATE_CODE ");
            selectQuery.Append(", STR.FURNACE_NO	 ");
            selectQuery.Append(", STR_DIM_BEFORE_1 ");
            selectQuery.Append(", STR_DIM_BEFORE_2 ");
            selectQuery.Append(", STR_LENGTH_BEFORE_1 ");
            selectQuery.Append(", STR_LENGTH_BEFORE_2 ");
            selectQuery.Append(", RESERVATION_NUMBER ");
            selectQuery.Append(", RESTRAIGHTING ");

            selectQuery.Append(" ,TO_CHAR( STR.STR_END_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_END_DATE_STRING ");
            selectQuery.Append(" , STR.STR_END_OPERATOR_CD");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_END_OPERATOR_CD) AS STR_END_OPERATOR_NAME");


            selectQuery.Append(" , STR.STR_DIM_CHK ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_X_1 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_X_2 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_X_3 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_Y_1 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_Y_2 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_Y_3 ");
            selectQuery.Append(" , STR.STR_LENGTH_CHK ");
            selectQuery.Append(" , STR.STR_LENGTH_AFTER_1 ");
            selectQuery.Append(" , STR.STR_LENGTH_AFTER_2 ");
            selectQuery.Append(" , STR.STR_BARS ");
            selectQuery.Append(" , STR.UNIT_TYPE ");
            selectQuery.Append(" , STR.STR_QTY ");
            selectQuery.Append(" , STR.STR_FINISHED_QTY_CHK ");
            selectQuery.Append(" , STR.STR_TABA_NO_1 ");
            selectQuery.Append(" , STR.STR_TABA_NO_2 ");
            selectQuery.Append(" , STR.STR_TABA_NO_3 ");
            selectQuery.Append(" , STR.STR_TABA_NO_4 ");
            selectQuery.Append(" , STR.STR_EXT_TEXTURE ");
            selectQuery.Append(" , STR.STR_BEND_STATUS ");
            selectQuery.Append(" , STR.STR_NG_BARS1 ");
            selectQuery.Append(" , STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" , STR.STR_NG_BARS2 ");
            selectQuery.Append(" , STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" , STR.STR_NG_BARS3 ");
            selectQuery.Append(" , STR.STR_NG_DETAILS3 ");
            selectQuery.Append(" , STR.STR_DECISION ");
            selectQuery.Append(" , STR.STR_NEXT_PROCESS ");
            selectQuery.Append(" , STR.USER_ID ");
            selectQuery.Append(" , STR.INPUT_LOCATION_CD ");
            selectQuery.Append(" , STR.CREATED_DATE ");
            selectQuery.Append(" , STR.UPDATED_DATE ");

            selectQuery.Append(", STR.MSG_CD  ");
            selectQuery.Append(" ,M.MSG_CONTENTS AS STR_REMARKS ");
            selectQuery.Append(",TO_CHAR( STR.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_CREATED_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( STR.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_UPDATED_DATE_STRING ");
            selectQuery.Append(",MC.CONDITION_DESCRIPTION AS PP_CONDITION_DESCRIPTION ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_STRAIGHTENING2 STR ON STR.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = STR.MSG_CD  ");
            
            
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD "); // table not available in oracle 19c committed by shubham
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD "); 
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON PP.LOCATION_CD =LOC.LOCATION_CD");

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
            selectQuery.Append(" LEFT JOIN MST_CONDITION MC ON PP.PROD_CONDITION_CD = MC.CONDITION_CD");

            selectQuery.Append(" WHERE STR.STRAIGHTENING_CD= '" + StraighteningCD + "'");
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD DESC");
            return ExecuteQuerySingleResult<Straightening2Details>(selectQuery.ToString());
        }

        public void Delete(long id)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_STRAIGHTENING2 ");
            deleteQuery.Append(" WHERE STRAIGHTENING_CD =" + id);
            ExecuteNonQuery(deleteQuery.ToString());
        }
        public List<ThabaDetails> GetThabaDetails(string chubaan, short? gyouban)
        {
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, STR.STR_DECISION AS DECISION, STR.CREATED_DATE ,");
            selectQuery.Append(" CONCAT (STR.STR_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS TABA_NO, ");
            selectQuery.Append(" DECODE(STR.STR_DECISION,0,1,2,3) AS PROCESS_STATUS ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chubaan + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");

            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE  IS NULL ");
            selectQuery.Append(" AND STR.UPDATED_DATE IN ( ");
            selectQuery.Append(" SELECT  MAX(UPDATED_DATE) FROM ( ");
            selectQuery.Append(" SELECT  STR.UPDATED_DATE , CONCAT (STR.STR_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chubaan + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");

            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" ORDER BY TABA_NO ) group by TABA_NO  ) ");
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD ");
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }

        public List<Straightening2Details> GetStraightening24MachineDetails(string machineCd)
        {
            List<Straightening2Details> result = new List<Straightening2Details>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append("STR.MACHINE_CD ,  ");
            selectQuery.Append(" STR.STR_DECISION AS STR_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");
            //selectQuery.Append(" CONCAT (CONCAT (PP.CHUUBAN, '-'), PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,	");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append(" STR.STR_METALLIC_PLATE_CODE  AS STR_METALLIC_PLATE_CODE,	");
            selectQuery.Append(" PP.PROD_SHAPE  AS PP_PROD_SHAPE,	");
            //selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D ,	");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'',PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_SHAPE_STRING,");

            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,	");
            //selectQuery.Append(" PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE  AS  PP_MATL_DIM_D_SHAPE,	");
            selectQuery.Append("GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE, ");
            // selectQuery.Append("  STR.STR_DIM_AFTER_1 AS STR_DIE_DIM,	");
            selectQuery.Append(" CONCAT (STR_TABA_NO_1, CASE WHEN STR_TABA_NO_2 IS NOT NULL THEN CONCAT ('-', STR_TABA_NO_2) ELSE NULL END) AS STR_TABA_NO_1_2,");
             selectQuery.Append("  STR.STR_DIM_BEFORE_1,	");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_1, 'FM990.000') AS STR_DIM_BEFORE_1_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_2, 'FM990.000') AS STR_DIM_BEFORE_2_STRING,");
              selectQuery.Append("  STR.STR_DIM_BEFORE_2,	");
             //selectQuery.Append("  STR.STR_DIM_AFTER_Y_1,    ");
             //selectQuery.Append("  STR.STR_DIM_AFTER_Y_2,	");
             //selectQuery.Append("  STR.STR_DIM_AFTER_Y_3,	");
             selectQuery.Append(" CONCAT(TO_CHAR(STR_DIM_AFTER_X_1,'FM990.000'), CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_X_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_X_2,'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_X_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_X_3,'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_AFTER_X, ");


            selectQuery.Append("  STR.STR_LENGTH_AFTER_1,	");
            selectQuery.Append("  STR.STR_LENGTH_AFTER_2,	");
            // selectQuery.Append("  STR.STR_ACT_LENGTH3 AS STR_ACT_LENGTH3,	");

            selectQuery.Append("  STR.STR_BARS AS STR_BARS,	");
            selectQuery.Append("  STR.UNIT_TYPE AS STR_UNIT_TYPE, ");
            //selectQuery.Append("  STR.STR_BARS || " + Utils.GetProdUnitForDBQuery("STR.UNIT_TYPE") + " AS STR_BARS_DECODE ");

            selectQuery.Append("  STR.STR_QTY AS STR_QTY,	");
            //selectQuery.Append(Utils.GetBendStatusKubun("STR.STR_BEND_STATUS") + "  AS STR_BEND_STATUS_DECODE,");
            //selectQuery.Append(Utils.GetExtTextureStatus("STR.STR_EXT_TEXTURE") + " AS STR_EXT_TEXTURE_STATUS_DECODE, ");
            selectQuery.Append(" STR.STR_BEND_STATUS,");
            selectQuery.Append(" STR.STR_EXT_TEXTURE,");

            selectQuery.Append("  PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,	");
            selectQuery.Append("  PP.CHARGE AS PP_CHARGE,	");
            selectQuery.Append("  PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" CONCAT(CASE WHEN STR_TABA_NO_3 IS NOT NULL THEN STR_TABA_NO_3 ELSE NULL END,");
            selectQuery.Append(" CASE WHEN STR_TABA_NO_4 IS NOT NULL THEN CONCAT ('-', STR_TABA_NO_4) ELSE NULL END ) AS STR_TABA_NO_3_4,");
            //  selectQuery.Append("  STR.STR_ACT_DIM4 AS STR_ACT_DIM4,	");
            //  selectQuery.Append("  STR.STR_ACT_DIM5 AS STR_ACT_DIM5,	");
            //  selectQuery.Append("  STR.STR_ACT_DIM6 AS STR_ACT_DIM6,	");
            //  selectQuery.Append("  STR.STR_REMARKS AS STR_REMARKS,	");
            // selectQuery.Append("  TO_CHAR (STR.MAGNETISM, '90.00') AS STR_MAGNETISM_DECODE,	");
            //  selectQuery.Append("  STR.SHIKII_VALUE AS STR_SHIKII_VALUE,	");
            //  selectQuery.Append("  STR.EC_NG_BAR_TOTAL AS STR_EC_NG_BAR_TOTAL,	");
            selectQuery.Append(" NVL (STR.STR_END_DATE, STR.STR_START_DATE) AS ORDER_CLAUSE ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            //commented by vijay :: 08/02/2017 bcz removed from table. instead used in msg_message table. 
            // selectQuery.Append(", STR.STR_REMARKS ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS STR_REMARKS ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON STR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON STR.MSG_CD=MSG.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");

            selectQuery.Append(" WHERE  STR.MACHINE_CD  = '" + machineCd + "' AND NVL (STR.STR_END_DATE, STR.STR_START_DATE) > SYSDATE - 1");
            // selectQuery.Append("  AND ROWNUM <= 10");
            selectQuery.Append(" ORDER BY ORDER_CLAUSE ");
            result = ExecuteQueryMultiResult<Straightening2Details>(selectQuery.ToString());
            return result;
        }
        public List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc)
        {
            List<MessageDetails> result = new List<MessageDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("   MSG_DATE ");
            selectQuery.Append(" , TO_CHAR( MSG_DATE,'yyyy/mm/dd HH24:MI' ) AS MSG_DATE_STRING");
            selectQuery.Append(" , MSG_CONTENTS");
            selectQuery.Append(" , MAC.SHORT_NAME  AS MACHINE_NAME  ");
            selectQuery.Append(" , EMP.SHORT_NAME AS  OPERATOR_NAME ");
            selectQuery.Append(" , PKU.KUBUN_NAME AS PROCESS_NAME_JP ");
            selectQuery.Append(" , PKU.KUBUN_NAME_EN AS PROCESS_NAME_EN");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MESSAGE  MSG ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC  ON MAC.MACHINE_CD =MSG.MACHINE_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=MSG.OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PKU ON MSG.PROCESS_KUBUN_ID=PKU.KUBUN_ID");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD=" + processPlanCD);
            if (isSortDesc == true)
            {
                selectQuery.Append(" ORDER BY MSG_DATE DESC");
            }
            else
            {
                selectQuery.Append(" ORDER BY MSG_DATE ASC");
            }
            result = ExecuteQueryMultiResult<MessageDetails>(selectQuery.ToString());
            return result;
        }

        public Straightening2Details GetStraighteningDetailsByChuubanGyoubanMachine(string chuuban, short? gyouban, int? machineCD)
        {
            StringBuilder selectCountQuery = new StringBuilder();
            StringBuilder selectQuery = new StringBuilder();
            int count = 0;

            string whereCondition1 = " PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL";
            string whereCondition2 = "  AND STR.MACHINE_CD = '" + machineCD + "' AND STR.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + "";
            string finalWhere = "";
            finalWhere = whereCondition1 + whereCondition2;
            //SQL

            selectCountQuery.Append(" SELECT COUNT(*) ");
            selectCountQuery.Append(" FROM ");
            selectCountQuery.Append(" TRN_PROCESS_PLAN  PP ");
            selectCountQuery.Append(" INNER JOIN TRN_STRAIGHTENING2 STR ON STR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectCountQuery.Append(" WHERE " + finalWhere);
            count = Convert.ToInt32(ExecuteScalar(selectCountQuery.ToString()));

            if (count > 0)
            {
                finalWhere = whereCondition1 + whereCondition2;
            }
            else
            {
                finalWhere = whereCondition1;
            }

            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            //selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            

            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  ");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            //selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
        //    selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");    //AND PR.VER = PP.PROD_VER
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
            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(", PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //selectQuery.Append(", K.REMARKS AS	PP_KAKOUIRAI_REMARKS  "); 
            selectQuery.Append(", PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");

            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE AS PROCESS_PLAN_START_DATE, ");
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

            selectQuery.Append(" P1.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_1, ");
            selectQuery.Append(" P2.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_2, ");
            selectQuery.Append(" P3.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_3, ");
            selectQuery.Append(" P4.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_4, ");
            selectQuery.Append(" P5.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_5, ");
            selectQuery.Append(" P6.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_6, ");
            selectQuery.Append(" P7.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_7, ");
            selectQuery.Append(" P8.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_8, ");
            selectQuery.Append(" P9.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_9, ");
            selectQuery.Append(" P10.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_10, ");
            selectQuery.Append(" P11.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_11, ");
            selectQuery.Append(" P12.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_12, ");
            selectQuery.Append(" P13.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_13, ");

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

            selectQuery.Append(" P1.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_1, ");
            selectQuery.Append(" P2.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_2, ");
            selectQuery.Append(" P3.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_3, ");
            selectQuery.Append(" P4.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_4, ");
            selectQuery.Append(" P5.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_5, ");
            selectQuery.Append(" P6.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_6, ");
            selectQuery.Append(" P7.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_7, ");
            selectQuery.Append(" P8.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_8, ");
            selectQuery.Append(" P9.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_9, ");
            selectQuery.Append(" P10.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_10, ");
            selectQuery.Append(" P11.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_11, ");
            selectQuery.Append(" P12.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_12, ");
            selectQuery.Append(" P13.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_13, ");

            selectQuery.Append("PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.ELONGATION_PATTERN1,");
            selectQuery.Append("PP.ELONGATION_PATTERN1_MIN_MAX 	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX_SINGLE,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN_SINGLE,");
            selectQuery.Append("PP.ELONGATION_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.HARDNESS_PATTERN1 ,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_MIN_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MIN  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN3_ACTUAL  	,");

            selectQuery.Append("PP.DECARB_1,");
            selectQuery.Append("PP.DECARB_2,");
            selectQuery.Append("PP.DECARB_3,");
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
            selectQuery.Append("PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC, ");
            selectQuery.Append("PP.NYUKA_CD");
            selectQuery.Append(", PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(", MC.CONDITION_DESCRIPTION AS PP_CONDITION_DESCRIPTION ");
            selectQuery.Append(" ,STR.STRAIGHTENING_CD   ");
            selectQuery.Append(" ,STR.PROCESS_PLAN_CD  ");
            selectQuery.Append(" ,STR.MACHINE_CD  ");
            selectQuery.Append(",(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD =STR.MACHINE_CD ) AS STR_MACHINE_NAME");
            selectQuery.Append(" ,TO_CHAR( STR.STR_START_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_START_DATE_STRING ");
            selectQuery.Append(" , STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
            selectQuery.Append(", STR.TP_TEST	  ");
            selectQuery.Append(" , STR.STR_METALLIC_PLATE_CODE ");
            selectQuery.Append(", STR.FURNACE_NO	 ");
            selectQuery.Append(", STR_DIM_BEFORE_1 ");
            selectQuery.Append(", STR_DIM_BEFORE_2 ");
            selectQuery.Append(", STR_LENGTH_BEFORE_1 ");
            selectQuery.Append(", STR_LENGTH_BEFORE_2 ");
            selectQuery.Append(", RESERVATION_NUMBER ");
            selectQuery.Append(", RESTRAIGHTING ");

            selectQuery.Append(" ,TO_CHAR( STR.STR_END_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_END_DATE_STRING ");
            selectQuery.Append(" , STR.STR_END_OPERATOR_CD");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_END_OPERATOR_CD) AS STR_END_OPERATOR_NAME");


            selectQuery.Append(" , STR.STR_DIM_CHK ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_X_1 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_X_2 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_X_3 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_Y_1 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_Y_2 ");
            selectQuery.Append(" , STR.STR_DIM_AFTER_Y_3 ");
            selectQuery.Append(" , STR.STR_LENGTH_CHK ");
            selectQuery.Append(" , STR.STR_LENGTH_AFTER_1 ");
            selectQuery.Append(" , STR.STR_LENGTH_AFTER_2 ");
            selectQuery.Append(" , STR.STR_BARS ");
            //selectQuery.Append(" , STR.UNIT_TYPE ");
            selectQuery.Append(", (SELECT STR.UNIT_TYPE FROM TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE  PP.PROCESS_PLAN_CMPL_DATE IS NULL  AND PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND STR.MACHINE_CD = '" + machineCD + "' ");
            selectQuery.Append(" AND STR.UNIT_TYPE IS NOT NULL AND STR.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " ORDER BY  STR.STRAIGHTENING_CD DESC FETCH FIRST ROW ONLY)  AS UNIT_TYPE ");
            selectQuery.Append(" , STR.STR_QTY ");
            selectQuery.Append(" , STR.STR_FINISHED_QTY_CHK ");
            selectQuery.Append(" , STR.STR_TABA_NO_1 ");
            selectQuery.Append(" , STR.STR_TABA_NO_2 ");
            selectQuery.Append(" , STR.STR_TABA_NO_3 ");
            selectQuery.Append(" , STR.STR_TABA_NO_4 ");
            selectQuery.Append(" , STR.STR_EXT_TEXTURE ");
            selectQuery.Append(" , STR.STR_BEND_STATUS ");
            selectQuery.Append(" , STR.STR_NG_BARS1 ");
            selectQuery.Append(" , STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" , STR.STR_NG_BARS2 ");
            selectQuery.Append(" , STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" , STR.STR_NG_BARS3 ");
            selectQuery.Append(" , STR.STR_NG_DETAILS3 ");
            selectQuery.Append(" , STR.STR_DECISION ");
            selectQuery.Append(" , STR.STR_NEXT_PROCESS ");
            selectQuery.Append(" , STR.USER_ID ");
            selectQuery.Append(" , STR.INPUT_LOCATION_CD ");
            selectQuery.Append(" , STR.CREATED_DATE ");
            selectQuery.Append(" , STR.UPDATED_DATE ");

            selectQuery.Append(", STR.MSG_CD  ");
            selectQuery.Append(" ,M.MSG_CONTENTS AS STR_REMARKS ");
            selectQuery.Append(",TO_CHAR( STR.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_CREATED_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( STR.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_UPDATED_DATE_STRING ");
            selectQuery.Append(", STR.STR_TABA_NO_1  ");
            selectQuery.Append(", STR.STR_TABA_NO_2  ");
            selectQuery.Append(", STR.STR_TABA_NO_3  ");
            selectQuery.Append(", STR.STR_TABA_NO_4,  ");
            selectQuery.Append(" CASE WHEN STR.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " THEN 1 ");
            selectQuery.Append(" ELSE 0 END AS SHOW_DEFAULT,");
            if (count == 0)
            {
                selectQuery.Append("'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append("'YES' AS PAST_DATA_EXIST ");
            }
            //selectQuery.Append(",NY.PRODUCT_NYUKO_CD "); committed by shubham

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT JOIN TRN_STRAIGHTENING2 STR ON");
            selectQuery.Append(" STR.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");            
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = STR.MSG_CD ");

            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON PP.LOCATION_CD =LOC.LOCATION_CD");

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
            selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION MC ON PP.PROD_CONDITION_CD = MC.CONDITION_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            //selectQuery.Append(" INNER JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" INNER JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");

            selectQuery.Append(" WHERE " + finalWhere);
            /*  selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
              selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
              selectQuery.Append(" AND STR.MACHINE_CD = '" + machineCD + "'");
              selectQuery.Append(" AND STR.CREATED_DATE >= TO_DATE(SYSDATE)-" + days + " ");
              selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");*/
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY");
            return ExecuteQuerySingleResult<Straightening2Details>(selectQuery.ToString());
        }

        public void InsertStraightening2Details(Straightening2Details straighteningDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_STRAIGHTENING2(");
            insertQuery.Append(" STRAIGHTENING_CD ");
            insertQuery.Append(",  PROCESS_PLAN_CD ");
            insertQuery.Append(", MACHINE_CD ");
            insertQuery.Append(", LOCATION_CD ");
            insertQuery.Append(", MSG_CD ");
            insertQuery.Append(", STR_START_DATE ");
            insertQuery.Append(", STR_START_OPERATOR_CD ");
            insertQuery.Append(", TP_TEST ");
            insertQuery.Append(", STR_METALLIC_PLATE_CODE ");
            insertQuery.Append(", FURNACE_NO ");
            insertQuery.Append(", STR_DIM_BEFORE_1 ");
            insertQuery.Append(", STR_DIM_BEFORE_2 ");
            insertQuery.Append(", STR_LENGTH_BEFORE_1 ");
            insertQuery.Append(", STR_LENGTH_BEFORE_2 ");
            insertQuery.Append(", RESERVATION_NUMBER ");
            insertQuery.Append(", RESTRAIGHTING ");
            insertQuery.Append(", PROCESS_KUBUN_CD ");
            insertQuery.Append(", INPUT_LOCATION_CD ");
            insertQuery.Append(", USER_ID ");
            insertQuery.Append(", CREATED_DATE ");
            insertQuery.Append(", UPDATED_DATE ");
            insertQuery.Append(", STR_TABA_NO_1 ");
            insertQuery.Append(", STR_TABA_NO_2 ");
            insertQuery.Append(", STR_TABA_NO_3 ");
            insertQuery.Append(", STR_TABA_NO_4 ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");

            insertQuery.Append(" TRN_STRAIGHTENING2_SEQ.NEXTVAL ");
            insertQuery.Append(",'" + straighteningDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.LOCATION_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.MSG_CD + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(straighteningDetails.STR_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + straighteningDetails.STR_START_OPERATOR_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.TP_TEST + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_METALLIC_PLATE_CODE + "'");
            insertQuery.Append(",'" + straighteningDetails.FURNACE_NO + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_2 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_LENGTH_BEFORE_1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_LENGTH_BEFORE_2 + "'");
            insertQuery.Append(",'" + straighteningDetails.RESERVATION_NUMBER + "'");
            insertQuery.Append(",'" + straighteningDetails.RESTRAIGHTING + "'");
            insertQuery.Append(",'" + straighteningDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.USER_ID + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_2 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_3 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_4 + "'");


            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            //return GetStraighteningDetailsByChuubanGyoubanMachine(straighteningDetails.PP_CHUUBAN, Convert.ToInt16(straighteningDetails.PP_GYOUBAN), straighteningDetails.MACHINE_CD.ToString());
        }

        public long UpdateStraightening2Details(Straightening2Details straighteningDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_STRAIGHTENING2 SET ");

            // Start data is updated
            updateQuery.Append(" MACHINE_CD = '" + straighteningDetails.MACHINE_CD + "',");
            updateQuery.Append(" STR_START_DATE = ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(straighteningDetails.STR_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");
            updateQuery.Append(" STR_START_OPERATOR_CD = '" + straighteningDetails.STR_START_OPERATOR_CD + "',");
            updateQuery.Append(" TP_TEST ='" + straighteningDetails.TP_TEST + "',");
            updateQuery.Append(" STR_METALLIC_PLATE_CODE ='" + straighteningDetails.STR_METALLIC_PLATE_CODE + "',");
            updateQuery.Append(" FURNACE_NO ='" + straighteningDetails.FURNACE_NO + "',");
            updateQuery.Append(" STR_DIM_BEFORE_1='" + straighteningDetails.STR_DIM_BEFORE_1 + "',");
            updateQuery.Append(" STR_DIM_BEFORE_2='" + straighteningDetails.STR_DIM_BEFORE_2 + "',");
            updateQuery.Append(" STR_LENGTH_BEFORE_1 ='" + straighteningDetails.STR_LENGTH_BEFORE_1 + "',");
            updateQuery.Append(" STR_LENGTH_BEFORE_2 ='" + straighteningDetails.STR_LENGTH_BEFORE_2 + "',");
            updateQuery.Append(" RESERVATION_NUMBER ='" + straighteningDetails.RESERVATION_NUMBER + "',");
            updateQuery.Append(" RESTRAIGHTING ='" + straighteningDetails.RESTRAIGHTING + "',");

            // End data is updated
            updateQuery.Append(" STR_END_DATE= ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(straighteningDetails.STR_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");
            updateQuery.Append(" STR_END_OPERATOR_CD ='" + straighteningDetails.STR_END_OPERATOR_CD + "',");
            updateQuery.Append(" STR_DIM_CHK='" + straighteningDetails.STR_DIM_CHK + "',");
            updateQuery.Append(" STR_DIM_AFTER_X_1='" + straighteningDetails.STR_DIM_AFTER_X_1 + "',");
            updateQuery.Append(" STR_DIM_AFTER_X_2='" + straighteningDetails.STR_DIM_AFTER_X_2 + "',");
            updateQuery.Append(" STR_DIM_AFTER_X_3 ='" + straighteningDetails.STR_DIM_AFTER_X_3 + "',");
            updateQuery.Append(" STR_DIM_AFTER_Y_1 ='" + straighteningDetails.STR_DIM_AFTER_Y_1 + "',");
            updateQuery.Append(" STR_DIM_AFTER_Y_2 = '" + straighteningDetails.STR_DIM_AFTER_Y_2 + "',");
            updateQuery.Append(" STR_DIM_AFTER_Y_3 = '" + straighteningDetails.STR_DIM_AFTER_Y_3 + "',");
            updateQuery.Append(" STR_LENGTH_CHK= '" + straighteningDetails.STR_LENGTH_CHK + "',");
            updateQuery.Append(" STR_LENGTH_AFTER_1 = '" + straighteningDetails.STR_LENGTH_AFTER_1 + "',");
            updateQuery.Append(" STR_LENGTH_AFTER_2  = '" + straighteningDetails.STR_LENGTH_AFTER_2 + "',");
            updateQuery.Append(" STR_BARS = '" + straighteningDetails.STR_BARS + "',");

            updateQuery.Append(" UNIT_TYPE = '" + straighteningDetails.UNIT_TYPE + "',");

            updateQuery.Append(" STR_QTY  = '" + straighteningDetails.STR_QTY + "',");
            updateQuery.Append(" STR_FINISHED_QTY_CHK ='" + (straighteningDetails.STR_FINISHED_QTY_CHK != null ? straighteningDetails.STR_FINISHED_QTY_CHK.ToString() : string.Empty) + "',");
            updateQuery.Append(" STR_TABA_NO_1 = '" + straighteningDetails.STR_TABA_NO_1 + "',");
            updateQuery.Append(" STR_TABA_NO_2 ='" + straighteningDetails.STR_TABA_NO_2 + "',");
            updateQuery.Append(" STR_TABA_NO_3  = '" + straighteningDetails.STR_TABA_NO_3 + "',");
            updateQuery.Append(" STR_TABA_NO_4 = '" + straighteningDetails.STR_TABA_NO_4 + "',");
            updateQuery.Append(" STR_EXT_TEXTURE  = '" + straighteningDetails.STR_EXT_TEXTURE + "',");
            updateQuery.Append(" STR_BEND_STATUS = '" + straighteningDetails.STR_BEND_STATUS + "',");
            updateQuery.Append(" STR_NG_BARS1 = '" + straighteningDetails.STR_NG_BARS1 + "',");
            updateQuery.Append(" STR_NG_DETAILS1  = '" + straighteningDetails.STR_NG_DETAILS1 + "',");
            updateQuery.Append(" STR_NG_BARS2 = '" + straighteningDetails.STR_NG_BARS2 + "',");
            updateQuery.Append(" STR_NG_DETAILS2 = '" + straighteningDetails.STR_NG_DETAILS2 + "',");
            updateQuery.Append(" STR_NG_BARS3  = '" + straighteningDetails.STR_NG_BARS3 + "',");
            updateQuery.Append(" STR_NG_DETAILS3 = '" + straighteningDetails.STR_NG_DETAILS3 + "',");
            updateQuery.Append(" STR_DECISION = '" + straighteningDetails.STR_DECISION + "',");
            updateQuery.Append(" STR_NEXT_PROCESS  = '" + straighteningDetails.STR_NEXT_PROCESS + "',");
            updateQuery.Append(" MSG_CD ='" + straighteningDetails.MSG_CD + "',");
            updateQuery.Append(" USER_ID ='" + straighteningDetails.USER_ID + "',");
            updateQuery.Append(" LOCATION_CD ='" + straighteningDetails.LOCATION_CD + "',");
            updateQuery.Append(" INPUT_LOCATION_CD ='" + (straighteningDetails.INPUT_LOCATION_CD > 0 ? straighteningDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "',");
            updateQuery.Append(" UPDATED_DATE = SYSDATE");

            updateQuery.Append(" WHERE ");
            updateQuery.Append(" STRAIGHTENING_CD= '" + straighteningDetails.STRAIGHTENING_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());

            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_STRAIGHTENING2 WHERE STRAIGHTENING_CD= '" + straighteningDetails.STRAIGHTENING_CD + "'");

            long msgCD = 0;

            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCD = Convert.ToInt64(cdVal);
            }
            return msgCD;
        }

        public List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, short? gyuban, long? straighteningCD)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, STR.STR_DECISION AS DECISION, STR.CREATED_DATE ,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_1) AS TABA_NO_1,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_2) AS TABA_NO_2,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_3) AS TABA_NO_3,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_4) AS TABA_NO_4");
            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE STR.STR_DECISION = " + decisionVal);
            selectQuery.Append(" AND PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");
            if (straighteningCD != 0)
            {
                selectQuery.Append(" AND STR.STRAIGHTENING_CD !='" + straighteningCD + "' ");
            }
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
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
        //public List<Straightening2Details> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, string Thaba_No_String)
        //{
        //    var result = new List<Straightening2Details>();
        //    StringBuilder selectQuery = new StringBuilder();
        //    selectQuery.Append(" SELECT ");
        //    selectQuery.Append(" STR.straightening_cd AS STRAIGHTENING_CD  ");
        //    selectQuery.Append(" ,STR.STR_DECISION  ");
        //    selectQuery.Append(" ,CONCAT  (STR.STR_TABA_NO_1, CONCAT (CONCAT ( ");
        //    selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
        //    selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
        //    selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS STR_TABA_NO ");

        //    selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ");
        //    selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

        //    selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
        //    selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
        //    selectQuery.Append(" AND STR.STR_TABA_NO_1 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_2 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_3 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_4 IN ('" + Thaba_No_String + "') ");


        //    result = ExecuteQueryMultiResult<Straightening2Details>(selectQuery.ToString());
        //    return result;
        //}

        public List<Straightening2Details> GetThabaDetailsByThabaNumber(long processPlanCD, string Thaba_No_String)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new List<Straightening2Details>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" STR.STRAIGHTENING_CD AS STRAIGHTENING_CD, ");
            selectQuery.Append(" STR.STR_DECISION, ");
            selectQuery.Append(" STR.RESTRAIGHTING AS RESTRAIGHTING, ");
            selectQuery.Append(" STR.STR_TABA_NO_1, ");
            selectQuery.Append(" STR.STR_TABA_NO_2, ");
            selectQuery.Append(" STR.STR_TABA_NO_3, ");
            selectQuery.Append(" STR.STR_TABA_NO_4, ");
            selectQuery.Append(" CONCAT(STR.STR_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', STR.STR_TABA_NO_2) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', STR.STR_TABA_NO_3) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', STR.STR_TABA_NO_4) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" )) AS STR_TABA_NO ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STRAIGHTENING2   STR ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN     PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD =" + processPlanCD + " ");
            selectQuery.Append(" AND str.str_decision =" + decisionVal + " ");
            selectQuery.Append(" AND ( STR.STR_TABA_NO_1 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_2 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_3 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_4 IN ('" + Thaba_No_String + "') )");
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD ");

            result = ExecuteQueryMultiResult<Straightening2Details>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY :  Namrata Ubale  2023/04/18   
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Straighteing2 process data according to search criteria        
        /// </returns>
        /// 
        public List<Straightening2Details> GetStraightning2DailyReportDataOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria,int rowLimit,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<Straightening2Details> result = new List<Straightening2Details>();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MACH.MACHINE_NAME AS STR_MACHINE_NAME,");
            selectQuery.Append(" TO_CHAR(STR2.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" STR2.STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" STR2.STR_BEND_STATUS AS STR_BEND_STATUS,");
            selectQuery.Append(" STR2.STR_NG_BARS1 ,");
            if(cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1,");
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2,");
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1,");
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2,");
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3,");
            }
                
            
            selectQuery.Append(" STR2.STR_NG_BARS2 ,");  
            selectQuery.Append(" STR2.STR_NG_BARS3 ,");

            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_BEFORE_1, 'FM990.000') AS STR_DIM_BEFORE_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_BEFORE_2, 'FM990.000') AS STR_DIM_BEFORE_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_X_1, 'FM990.000') AS STR_DIM_AFTER_X_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_X_2, 'FM990.000') AS STR_DIM_AFTER_X_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_X_3, 'FM990.000') AS STR_DIM_AFTER_X_3_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_Y_1, 'FM990.000') AS STR_DIM_AFTER_Y_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_Y_2, 'FM990.000') AS STR_DIM_AFTER_Y_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_Y_3, 'FM990.000') AS STR_DIM_AFTER_Y_3_STRING, ");
            selectQuery.Append(" STR2.STR_BARS,");
            selectQuery.Append(" STR2.UNIT_TYPE AS STR_UNIT_TYPE,");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2,"); 
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS STR_REMARKS,");
            selectQuery.Append(" INTE.INTERNAL_NAME,");
            selectQuery.Append(" PP.CHAMFER, ");
            selectQuery.Append(" PP.PROD_LENGTH, ");
            selectQuery.Append(" STR2.STR_END_DATE, ");
            selectQuery.Append(" STR2.STR_DECISION, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,"); 
            selectQuery.Append(" STR2.STR_QTY,");
            selectQuery.Append(" STR2.STR_EXT_TEXTURE AS STR_EXT_TEXTURE, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS DIM_TOL_MAX_CHR,");
            selectQuery.Append(" CONCAT(PP.DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DIM_TOL_MIN_CHR IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', PP.DIM_TOL_MIN_CHR) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(") AS DIM_TOL_MIN_MAX_CHR, ");
            selectQuery.Append(" CONCAT(STR_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', STR_TABA_NO_2) ");
            selectQuery.Append(" ELSE   NULL ");
            selectQuery.Append(" END, CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', STR_TABA_NO_3) ");
            selectQuery.Append(" ELSE NULL  END), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', STR_TABA_NO_4) ");
            selectQuery.Append(" ELSE   NULL  END ");
            selectQuery.Append(" )) AS ALL_TABA_NO, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");
            //selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION");

            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR2");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR2.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR2.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = STR2.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
           
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD =STR2.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = STR2.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON STR2.STR_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON STR2.STR_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON STR2.STR_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = STR2.STR_NEXT_PROCESS");
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
                //    query = query + whereCondition;
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
            //    query += " ORDER BY STR_END_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<Straightening2Details>(query);            
            return result;
        }

        /// <summary>
        /// CREATED BY :     
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Straighteing2 process data according to search criteria        
        /// </returns>
        /// 
        public List<Straightening2Details> GetStraightning2DailyProcessAnalysisStatus(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VERR,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
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
            selectQuery.Append(" PP.COIL_DIA_WINDING AS COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER,");
            selectQuery.Append(" PP.MAGARI_KUBUN AS PP_MAGARI_KUBUN,"); 
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PR_MAGARI_BEND_TOL,");
            selectQuery.Append(" PP.MAGARI_SPAN,");
            selectQuery.Append(" PP.NEJIRE_ANGLE,");
            selectQuery.Append(" PP.NEJIRE_SPAN,");
            selectQuery.Append(" PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN,");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN,");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX,");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE,");
            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR,");
            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY,");
            selectQuery.Append(" PP.STAMP AS PP_STAMP");
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
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN_SINGLE,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX_SINGLE,");
            selectQuery.Append(" PP.ELONGATION_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.YEILD_PATTERN1,");
            selectQuery.Append(" PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append(" PP.YEILD_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.PRESS_PATTERN1,");
            selectQuery.Append(" PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX,");
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
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.TEST_SPEC_REMARKS,");
            selectQuery.Append(" PP.CORNER_RADIUS AS CORNER_RADIUS,");
            selectQuery.Append(" PP.MBEU,"); 
            selectQuery.Append(" PP.RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION,");
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK,");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");             
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_CONDITION_DESCRIPTION ,");
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
            selectQuery.Append(" MACH.MACHINE_NAME AS STR_MACHINE_NAME,");
            selectQuery.Append(" TO_CHAR(STR.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_STRING,");
            selectQuery.Append(" EMPL.SHORT_NAME AS STR_START_OPERATOR_NAME,");
            selectQuery.Append(" STR.TP_TEST,");
            selectQuery.Append(" STR.STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" STR.FURNACE_NO,");
           
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_1, 'FM990.000') AS STR_DIM_BEFORE_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_2, 'FM990.000') AS STR_DIM_BEFORE_2_STRING, ");
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_1,");
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_2,");
            selectQuery.Append(" STR.RESERVATION_NUMBER,");
            selectQuery.Append(" STR.RESTRAIGHTING AS RESTRAIGHTING,");
            selectQuery.Append(" TO_CHAR(STR.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING,");
            selectQuery.Append(" EMPL1.SHORT_NAME AS STR_END_OPERATOR_NAME,");
                       
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_X_1, 'FM990.000') AS STR_DIM_AFTER_X_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_X_2, 'FM990.000') AS STR_DIM_AFTER_X_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_X_3, 'FM990.000') AS STR_DIM_AFTER_X_3_STRING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_Y_1, 'FM990.000') AS STR_DIM_AFTER_Y_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_Y_2, 'FM990.000') AS STR_DIM_AFTER_Y_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_Y_3, 'FM990.000') AS STR_DIM_AFTER_Y_3_STRING, ");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_1,");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_2,");
            selectQuery.Append(" STR.STR_BARS,");
            selectQuery.Append(" STR.UNIT_TYPE AS STR_UNIT_TYPE,");
            selectQuery.Append(" STR.STR_QTY,");
            selectQuery.Append(" STR.STR_TABA_NO_1,");
            selectQuery.Append(" STR.STR_TABA_NO_2,");
            selectQuery.Append(" STR.STR_TABA_NO_3,");
            selectQuery.Append(" STR.STR_TABA_NO_4,");
            selectQuery.Append(" STR.STR_EXT_TEXTURE,"); 
            selectQuery.Append(" STR.STR_BEND_STATUS,");
            selectQuery.Append(" STR.STR_NG_BARS1,");
            selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP,");
            selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN,");
            selectQuery.Append(" STR.STR_NG_BARS2,");
            selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP,");
            selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN,");
            selectQuery.Append(" STR.STR_NG_BARS3,");
            selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP,");
            selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN,");
            selectQuery.Append(" STR.STR_DECISION AS STR_DECISION");
            selectQuery.Append(", MSG.MSG_CONTENTS AS STR_REMARKS ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");            
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=STR.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN  MST_MACHINE MACH ON MACH.MACHINE_CD=STR.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD=STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL1 ON EMPL1.EMPLOYEE_CD=STR.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS3 ");
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
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = STR.MSG_CD ");
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
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return ExecuteQueryMultiResult<Straightening2Details>(query);
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
        public int GetRowCountForStraightening2Factory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT COUNT(*) ");            
            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR2");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR2.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR2.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = STR2.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD =STR2.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = STR2.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON STR2.STR_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON STR2.STR_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON STR2.STR_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = STR2.STR_NEXT_PROCESS");
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
        public int GetRowCountForStraightening2DailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=STR.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN  MST_MACHINE MACH ON MACH.MACHINE_CD=STR.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD=STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL1 ON EMPL1.EMPLOYEE_CD=STR.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS3 ");
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
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = STR.MSG_CD ");

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
