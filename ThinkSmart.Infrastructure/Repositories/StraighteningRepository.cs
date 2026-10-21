using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Enums;
using System.Reflection;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class StraighteningRepository : BaseRepository, IStraighteningRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public StraighteningRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            //_connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }

        public List<StraighteningDetails> GetStraighteningList(List<SearchField> searchCriteria, string sortCiteria, long locationCD)
        {
            List<StraighteningDetails> result = new List<StraighteningDetails>();
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            //selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(", PP.CHUUBAN AS PP_CHUUBAN ");
            selectQuery.Append(", PP.GYOUBAN AS PP_GYOUBAN ");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            selectQuery.Append(",PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO ");
            selectQuery.Append(",TO_CHAR (ST.STR_START_DATE,'yyyy/mm/dd hh24:mi')  AS STR_START_DATE_TIME ");
            selectQuery.Append(",TO_CHAR(ST.STR_END_DATE,'yyyy/mm/dd hh24:mi') AS STR_END_DATE_STRING ");
            // selectQuery.Append(" ,EMP.SHORT_NAME AS STR_START_OPERATOR_NAME");
            selectQuery.Append(",(SELECT MACHINE_NAME FROM MST_MACHINE WHERE MACHINE_CD = ST.MACHINE_CD) AS STR_MACHINE_NAME ");
            //selectQuery.Append(",MS.SHORT_NAME AS ST_MACHINE_NAME");
            selectQuery.Append(",(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
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
            selectQuery.Append(" THEN 'R-'   || PP.CORNER_RADIUS END) AS PP_PROD_DIM_D_String");
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE");
            // selectQuery.Append(",PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
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
            //selectQuery.Append(", CONCAT(STR_TABA_NO_1,CONCAT(CONCAT(CASE WHEN STR_TABA_NO_2 IS NOT NULL THEN CONCAT(',',STR_TABA_NO_2) ELSE NULL END ");
            //selectQuery.Append(", CASE WHEN STR_TABA_NO_3 IS NOT NULL THEN CONCAT(',',STR_TABA_NO_3) ELSE NULL END ) ");
            //selectQuery.Append(", CASE WHEN STR_TABA_NO_4 IS NOT NULL THEN CONCAT(',' ,STR_TABA_NO_4) ELSE NULL END ) ) ");
            //selectQuery.Append(" ,CONCAT (ST.STR_BARS, DECODE (ST.UNIT_TYPE, 1, ' P', 2, ' B')) AS STR_FINISHED_BARS ");
            selectQuery.Append(",ST.STR_BARS AS STR_BARS");
            selectQuery.Append(",ST.UNIT_TYPE AS STR_UNIT_TYPE");
            //   selectQuery.Append(" , ST.STR_BARS || " + Utils.GetMatlUnitForDBQuery("ST.UNIT_TYPE ") + " AS STR_BARS_DECODE ");
            selectQuery.Append(", ST.STR_QTY  ");
            selectQuery.Append(" ,ST.UNIT_TYPE As STR_UNIT_TYPE");
            // selectQuery.Append(" CONCAT ( CONCAT( TS.STR_DIM_AFTER_MIN, '" + TILDE + "'),TS.STR_DIM_AFTER_MAX) AS ACTUAL_DIM , ") ;
            //selectQuery.Append(" CONCAT ( CONCAT( TS.STR_LENGTH_AFTER_1, '" + TILDE + "'),TS.STR_LENGTH_AFTER_2) AS ACTUAL_LENGTH , ");
            selectQuery.Append(" ,ST.MACHINE_CD AS STR_MACHINE_CD ");
            //selectQuery.Append(" ,DECODE (ST.STR_BEND_STATUS ,1, '良', 2, '良２', 3, '良3', 4, '保留',5, '矯正',6, '多ロール', 7, 'その他' ) AS STR_BEND_STATUS_VAL");
            //selectQuery.Append(" ,DECODE (ST.STR_EXT_TEXTURE, 1, '良',2, '保留',3, 'らせん矯正', 4, '肌矯正',5, '寸法矯正', 6, 'ダイス傷', 7, '錠傷', 8, 'カッター刃傷', 9, 'ローラー傷', 10, 'その他') AS STR_EXT_TEXTURE_STATUS ");
            selectQuery.Append(",ST.STR_BEND_STATUS");
            selectQuery.Append(",ST.STR_EXT_TEXTURE");

            selectQuery.Append(", ST.RESERVATION_NUMBER AS STR_RESERVATION_NO ");
            selectQuery.Append(", ST.STRAIGHTENING_CD AS STR_STRAIGHTENING_CD");
            selectQuery.Append(", INTE.INTERNAL_NAME");
            selectQuery.Append(", ST.STR_DECISION");
            //sub query for count row no..
            selectQuery.Append(" ,(SELECT COUNT(PP.PROCESS_PLAN_CD)");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN P INNER JOIN TRN_STRAIGHTENING S ");
            selectQuery.Append(" ON P.PROCESS_PLAN_CD = S.PROCESS_PLAN_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EM ON S.STR_START_OPERATOR_CD = EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE S.STR_END_OPERATOR_CD IS NOT NULL ) ");
            selectQuery.Append(" AS ROW_COUNT ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_STRAIGHTENING  ST  ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD=ST.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MS ON ST.MACHINE_CD=MS.MACHINE_CD   ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ");
            selectQuery.Append(" ON ST.STR_START_OPERATOR_CD=EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD= CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD =PP.INTERNAL_NAME_CD");


            selectQuery.Append(" WHERE ST.INPUT_LOCATION_CD =" + locationCD + " AND ST.str_end_operator_cd IS NOT NULL ");
            selectQuery.Append(" AND PP.process_plan_cmpl_date IS NULL");
            //selectQuery.Append(" AND ROWNUM <= ");
            //selectQuery.Append(" (CASE");
            //selectQuery.Append(" WHEN (ROWNUM >" + _maxRowLimit + ")  THEN " + _maxRowLimit + "");
            //selectQuery.Append(" ELSE ROWNUM");
            //selectQuery.Append(" END)");

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
                query += "ORDER BY PP.CHUUBAN ASC, ST.STR_TABA_NO_1 ASC";
            }
            result = ExecuteQueryMultiResult<StraighteningDetails>(query);
            return result;
        }

        public void Delete(long id)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_STRAIGHTENING ");
            deleteQuery.Append(" WHERE STRAIGHTENING_CD =" + id);
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public StraighteningDetails GetStraighteningDetailsByCD(long StraighteningCD)
        {
            StraighteningDetails result = new StraighteningDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("  SELECT ");
            // selectQuery.Append("  SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(",  PP.CHUUBAN AS PP_CHUUBAN  ");
            selectQuery.Append(",  PP.GYOUBAN	AS	PP_GYOUBAN  ");
            selectQuery.Append(",  PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(",  PP.PROD_SPEC_NO	AS	PP_PROD_SPEC_NO  ");
            selectQuery.Append(",  PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(",  TO_CHAR(PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' )	AS	PP_DELIVERY_DATE_STRING  ");
            selectQuery.Append(",  EMP.SHORT_NAME	AS	PP_EMPLOYEE_NAME  ");

            //selectQuery.Append(", CASE   WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL");
            //selectQuery.Append(" THEN EMP.SHORT_NAME   || ' '|| '(' || LOC.SHORT_NAME || ' ' || DEP.DEPARTMENT_NAME  || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(",  REC.SHORT_NAME	AS	PP_RECEIVER_NAME  ");
            selectQuery.Append(",  CST.SHORT_NAME	AS	PP_CUSTOMER_NAME  ");
            selectQuery.Append(",  PP.CHAMFER	AS	PP_CHAMFER  ");
            selectQuery.Append(",  PP.COLOR_ID	AS	PP_COLOR  ");
            selectQuery.Append(",  PP.MATL_NAME_MAK	AS	PP_MATL_NAME_MAK  ");
            selectQuery.Append(",  PP.MATL_SHAPE	AS	PP_MATL_SHAPE  ");
            selectQuery.Append(",  PP.MATL_DIM_D	AS	PP_MATL_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,null,null) AS PP_MATL_DIM_D_STRING ");
            selectQuery.Append(",  PP.CHARGE	AS	PP_CHARGE  ");
            selectQuery.Append(",  PP.MAGARI_KUBUN	AS	PP_MAGARI_KUBUN  ");
            selectQuery.Append(",  PP.MAGARI_BEND_TOL	AS	PP_BEND_TOL  ");
            selectQuery.Append(",  PP.MAGARI_SPAN	AS	PP_MAGARI_SPAN  ");
            selectQuery.Append(",  PP.PROD_SHAPE	AS	PP_PROD_SHAPE  ");
            selectQuery.Append(",  PP.PROD_DIM_D	AS	PP_PROD_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,NULL,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ");
            selectQuery.Append(",  PP.PROD_NAME_CUST	AS	PP_PROD_NAME_CUST  ");
            selectQuery.Append(",  PP.PROD_DIM_OTHER	AS	PP_PROD_DIM_OTHER  ");
            selectQuery.Append(",  PP.EXT_TEXTURE	AS	PP_EXT_TEXTURE  ");
            selectQuery.Append(",  PP.DIM_TOL_MIN_CHR	AS	PP_DIM_TOL_MIN  ");
            selectQuery.Append(",  PP.DIM_TOL_MAX_CHR	AS	PP_DIM_TOL_MAX  ");
            selectQuery.Append(", (CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(",  PP.DISPLAY	AS	PP_DISPLAY  ");
            selectQuery.Append(",  PP.STAMP	AS	PP_STAMP  ");
            selectQuery.Append(", CON.CONDITION_DESCRIPTION ");
            selectQuery.Append(",  PP.PROD_LENGTH	AS	PP_PROD_LENGTH  ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(",  PP.LENGTH_TOL_MIN_CHR	AS	PP_LENGTH_TOL_MIN  ");
            selectQuery.Append(",  PP.LENGTH_TOL_MAX_CHR	AS	PP_LENGTH_TOL_MAX  ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(",  PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(",  PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            //  selectQuery.Append(",  PP.INZU	AS	PP_INZU  ");
            //   selectQuery.Append(",  PP.INZU_UNIT	AS	PP_INZU_UNIT  ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN "); 
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX ");
            selectQuery.Append(",  PP.NO_OF_UNITS	AS	PP_NO_OF_UNITS  ");
            // selectQuery.Append("," + Utils.GetUnitString("PP.UNIT") + " AS PP_UNIT ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(",  PP.ACT_ALLOC_QTY	AS	PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(",  PP.COIL_DIA_INTERNAL AS	PP_COIL_DIA_INTERNAL  ");
            selectQuery.Append(",  PP.COIL_DIA_EXTERNAL	AS	PP_COIL_DIA_EXTERNAL ");
            // selectQuery.Append("," + Utils.GetCoilDiaWindingValueForDBQuery("PP.COIL_DIA_WINDING") + " AS PP_COIL_DIA_WINDING ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //selectQuery.Append(",  K.REMARKS	AS	PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_REMARK	AS	PP_PROCESS_PLAN_REMARK  ");
            selectQuery.Append(",  PP.STD_DEL_PERIOD	AS	PP_STD_DEL_PERIOD  ");
            selectQuery.Append(",  PP.CREATED_DATE	AS	PP_CREATED_DATE  ");
            selectQuery.Append(",  PP.UPDATED_DATE	AS	PP_UPDATED_DATE  ");
            selectQuery.Append(",  GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD)	AS	PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_CD	AS	PP_PROCESS_PLAN_CD  ");
            selectQuery.Append(",PP.NYUKA_CD AS PRODUCT_NYUKO_CD ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(", ST.STR_START_DATE AS STR_STR_START_DATE ");
            selectQuery.Append(", TO_CHAR( ST.STR_START_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_START_DATE_STRING");
            selectQuery.Append(", ST.STR_END_DATE  AS STR_END_DATE  ");
            selectQuery.Append(", TO_CHAR( ST.STR_END_DATE ,'yyyy/mm/dd HH24:MI' ) AS STR_END_DATE_STRING");
            selectQuery.Append(", ST.STR_START_OPERATOR_CD AS STR_START_OPERATOR ");
            selectQuery.Append(", ST.STR_END_OPERATOR_CD AS STR_END_OPERATOR");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = ST.STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = ST.STR_END_OPERATOR_CD) AS STR_END_OPERATOR_NAME");

            selectQuery.Append(", ST.TP_TEST AS STR_TP_TEST ");
            selectQuery.Append(", ST.FURNACE_NO AS STR_FURNACE_NO");
            selectQuery.Append(", ST.STR_METALLIC_PLATE_CODE AS STR_METALLIC_PLATE_CODE ");
            selectQuery.Append(", ST.STR_DECISION  ");
            selectQuery.Append(", ST.UPPER_LOWER_ROLLER_ANGLE1 AS STR_UPPER_LOWER_ROLLER_ANGLE1  ");
            selectQuery.Append(", ST.UPPER_LOWER_ROLLER_ANGLE2  AS STR_UPPER_LOWER_ROLLER_ANGLE2 ");
            selectQuery.Append(", ST.UPPER_ROLLER_ELEVATION  AS STR_UPPER_ROLLER_ELEVATION ");
            selectQuery.Append(", ST.RESTRAIGHTING AS STR_RESTRAIGHTING ");
            selectQuery.Append(", ST.STR_TONNAGE ");
            selectQuery.Append(", ST.STR_DIM_CHK ");
            selectQuery.Append(", ST.STR_LENGTH_CHK ");
            selectQuery.Append(", ST.STR_DIM_AFTER_1");
            selectQuery.Append(", ST.STR_DIM_AFTER_2");
            selectQuery.Append(", ST.STR_DIM_AFTER_3");
            selectQuery.Append(", ST.STR_DIM_AFTER_4");
            selectQuery.Append(", ST.STR_DIM_AFTER_5");
            selectQuery.Append(", ST.STR_DIM_AFTER_6");
            selectQuery.Append(", ST.STR_DIM_BEFORE_1");
            selectQuery.Append(", ST.STR_DIM_BEFORE_2");
            selectQuery.Append(", ST.STR_DIM_BEFORE_3");
            selectQuery.Append(", ST.STR_DIM_BEFORE_4");
            selectQuery.Append(", ST.STR_DIM_BEFORE_5");
            selectQuery.Append(", ST.STR_DIM_BEFORE_6");
            selectQuery.Append(", ST.STR_LENGTH_AFTER_2");
            selectQuery.Append(", ST.STR_LENGTH_AFTER_1");
            selectQuery.Append(", ST.STR_LENGTH_BEFORE_2");
            selectQuery.Append(", ST.STR_LENGTH_BEFORE_1");
            selectQuery.Append(", ST.UNIT_TYPE AS STR_UNIT_TYPE ");
            selectQuery.Append(", ST.STR_QTY ");
            selectQuery.Append(", ST.STR_FINISHED_QTY_CHK ");
            selectQuery.Append(", ST.STR_BARS");
            selectQuery.Append(", ST.STR_NG_BARS1");
            selectQuery.Append(", ST.STR_NG_BARS2 ");
            selectQuery.Append(", ST.STR_NG_BARS3 ");
            selectQuery.Append(", ST.STR_NG_DETAILS1");
            selectQuery.Append(", ST.STR_NG_DETAILS2 ");
            selectQuery.Append(", ST.STR_NG_DETAILS3 ");
            selectQuery.Append(", PD1.PROC_DEFECT_SHORT AS PROCESS_DEFECT1");
            selectQuery.Append(", PD2.PROC_DEFECT_SHORT AS PROCESS_DEFECT2");
            selectQuery.Append(", PD3.PROC_DEFECT_SHORT AS PROCESS_DEFECT3");
            selectQuery.Append(", ST.A_SIDE_FURE1 AS STR_A_SIDE_FURE1 ");
            selectQuery.Append(", ST.A_SIDE_FURE2 AS STR_A_SIDE_FURE2 ");
            selectQuery.Append(", ST.A_SIDE_FURE3 AS STR_A_SIDE_FURE3 ");
            selectQuery.Append(", ST.B_SIDE_FURE1 AS STR_B_SIDE_FURE1 ");
            selectQuery.Append(", ST.B_SIDE_FURE2 AS STR_B_SIDE_FURE2 ");
            selectQuery.Append(", ST.B_SIDE_FURE3 AS STR_B_SIDE_FURE3 ");
            selectQuery.Append(", ST.STR_BEND_STATUS ");
            selectQuery.Append(", ST.STR_EXT_TEXTURE ");
            selectQuery.Append(", ST.STR_TABA_NO_1");
            selectQuery.Append(", ST.STR_TABA_NO_2 ");
            selectQuery.Append(", ST.STR_TABA_NO_3 ");
            selectQuery.Append(", ST.STR_TABA_NO_4 ");
            selectQuery.Append(", ST.RESERVATION_NUMBER AS STR_RESERVATION_NUMBER");
            selectQuery.Append(", ST.STR_BARS ");
            selectQuery.Append(", ST.STR_NEXT_PROCESS ");
            selectQuery.Append(", M.MSG_CONTENTS AS STR_REMARKS  ");
            selectQuery.Append(", M.MSG_CD AS STR_MSG_CD  ");
            selectQuery.Append(", ST.MACHINE_CD AS STR_MACHINE_CD ");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_MACHINE WHERE MACHINE_CD = ST.MACHINE_CD) AS STR_MACHINE_NAME ");
            selectQuery.Append(", ST.STRAIGHTENING_CD AS STR_STRAIGHTENING_CD");
            selectQuery.Append(", ST.CREATED_DATE	AS	STR_CREATED_DATE  ");
            selectQuery.Append(", TO_CHAR( ST.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_CREATED_DATE_STRING");
            selectQuery.Append(", ST.UPDATED_DATE	AS	STR_UPDATED_DATE  ");
            selectQuery.Append(", TO_CHAR( ST.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_UPDATED_DATE_STRING");

            selectQuery.Append("  FROM TRN_STRAIGHTENING ST  ");
            selectQuery.Append("  LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD =ST.PROCESS_PLAN_CD ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON PP.RECEIVER_CD = REC.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP  ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD "); // tables not added in 19c commited by shubham
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP ON DEP.DEPARTMENT_CD = dg.DEPARTMENT_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DG ON PP.DEPARTMENT_GROUP_CD = DG.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON PP.LOCATION_CD = LOC.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT PD1 ON PD1.PROCESS_DEFECT_CD = ST.STR_NG_DETAILS1 ");   //STR_NG_Detalis col save process Defect PK 
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT PD2 ON PD2.PROCESS_DEFECT_CD = ST.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT PD3 ON PD3.PROCESS_DEFECT_CD = ST.STR_NG_DETAILS3");



            selectQuery.Append("  WHERE ");
            selectQuery.Append("  ST.STRAIGHTENING_CD =" + StraighteningCD);
            result = ExecuteQuerySingleResult<StraighteningDetails>(selectQuery.ToString());
            return result;
        }

        public List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, string gyuban, long straighteningCD)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, STR.STR_DECISION AS DECISION, STR.CREATED_DATE ,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_1) AS TABA_NO_1,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_2) AS TABA_NO_2,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_3) AS TABA_NO_3,");
            selectQuery.Append(" TO_CHAR(STR.STR_TABA_NO_4) AS TABA_NO_4");
            selectQuery.Append(" FROM TRN_STRAIGHTENING STR INNER JOIN TRN_PROCESS_PLAN PP ");
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

        public List<ThabaDetails> GetThabaDetails(string chuuban, string gyuban)
        {
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, STR.STR_DECISION AS DECISION, STR.CREATED_DATE ,");
            selectQuery.Append(" CONCAT (STR.STR_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS TABA_NO, ");
            selectQuery.Append(" STR.STR_DECISION AS PROCESS_STATUS ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING STR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");

            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE  IS NULL ");
            selectQuery.Append(" AND STR.UPDATED_DATE IN ( ");
            selectQuery.Append(" SELECT  MAX(UPDATED_DATE) FROM ( ");
            selectQuery.Append(" SELECT  STR.UPDATED_DATE , CONCAT (STR.STR_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING STR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");

            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" ORDER BY TABA_NO ) group by TABA_NO  ) ");
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD ");
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
        public MST_MACHINE GetMachineNameByID(long machineCd)
        {
            MST_MACHINE result = new MST_MACHINE();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * From  MST_MACHINE");
            selectQuery.Append(" WHERE MACHINE_CD = '" + machineCd + "' ");
            //selectQuery.Append("AND IN_ACTIVE_FLAG = 0");     //only Active Machine is selected Pradeep M.2019-09-11
            result = ExecuteQuerySingleResult<MST_MACHINE>(selectQuery.ToString());
            return result;
        }
        public MessageDetails MessageDetailsInsert(MessageDetails messageDetails)
        {
            MessageDetails result = new MessageDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT  M.MSG_CD, M.PROCESS_PLAN_CD, M.MACHINE_CD , M.PROCESS_KUBUN_ID, M.OPERATOR_CD  ,M.MSG_DATE , M.MSG_CONTENTS, M.USER_ID, M.CREATED_DATE, M.UPDATED_DATE FROM TRN_MESSAGE M ");
            selectQuery.Append("WHERE MSG_CD = (SELECT MAX(MSG_CD) FROM TRN_MESSAGE)");

            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_MESSAGE ( ");
            insertQuery.Append("  PROCESS_PLAN_CD ");
            insertQuery.Append(", MACHINE_CD ");
            insertQuery.Append(", PROCESS_KUBUN_ID ");
            insertQuery.Append(", OPERATOR_CD ");
            insertQuery.Append(", MSG_DATE ");
            insertQuery.Append(", MSG_CONTENTS ");
            insertQuery.Append(", INPUT_LOCATION_CD ");
            insertQuery.Append(", USER_ID ");
            insertQuery.Append(", CREATED_DATE ");
            insertQuery.Append(", UPDATED_DATE ");

            insertQuery.Append(") VALUES (");

            insertQuery.Append("'" + messageDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + messageDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + messageDetails.PROCESS_KUBUN_ID + "'");
            insertQuery.Append(",'" + messageDetails.OPERATOR_CD + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(",'" + messageDetails.MSG_CONTENTS + "'");
            insertQuery.Append(",'" + messageDetails.INPUT_LOCATION_CD + "'");
            //insertQuery.Append(",'" + 5 + "'");
            insertQuery.Append(",'" + messageDetails.USER_ID + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());
            result = ExecuteQuerySingleResult<MessageDetails>(selectQuery.ToString());
            return result;
        }

        public MessageDetails MessageDetailsUpdate(long messageCD, string msgContents, int userID)
        {
            MessageDetails result = new MessageDetails();

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT  M.MSG_CD, M.PROCESS_PLAN_CD, M.MACHINE_CD , M.PROCESS_KUBUN_ID, M.OPERATOR_CD  ,M.MSG_DATE , M.MSG_CONTENTS, M.USER_ID, M.CREATED_DATE, M.UPDATED_DATE FROM TRN_MESSAGE M ");
            selectQuery.Append("WHERE MSG_CD = " + messageCD);

            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE TRN_MESSAGE SET ");
            updateQuery.Append(" MSG_DATE = SYSDATE");
            updateQuery.Append(", MSG_CONTENTS = '");
            updateQuery.Append(msgContents + "'");
            updateQuery.Append(", USER_ID = '");
            updateQuery.Append(userID + "'");
            updateQuery.Append(", UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE MSG_CD = ");
            updateQuery.Append(messageCD);
            ExecuteNonQuery(updateQuery.ToString());
            result = ExecuteQuerySingleResult<MessageDetails>(selectQuery.ToString());
            return result;
        }

        public long StraighteningDetailsUpdate(StraighteningDetails Straightening)
        {
            StringBuilder updateQuery = new StringBuilder();
            StringBuilder updateDateQuery = new StringBuilder();
            updateQuery.Append("  UPDATE TRN_STRAIGHTENING SET ");
            updateQuery.Append("  STRAIGHTENING_CD =" + (Straightening.STR_STRAIGHTENING_CD != null ? Straightening.STR_STRAIGHTENING_CD.ToString().Trim() : "''"));
            updateQuery.Append(", PROCESS_PLAN_CD =" + (Straightening.PP_PROCESS_PLAN_CD != null ? Straightening.PP_PROCESS_PLAN_CD.ToString().Trim() : "''"));
            updateQuery.Append(", MACHINE_CD =" + (Straightening.STR_MACHINE_CD != null ? Straightening.STR_MACHINE_CD.ToString().Trim() : "''"));
            updateQuery.Append(", MSG_CD ='" + Straightening.STR_MSG_CD + "'");
            updateQuery.Append(", STR_START_DATE = TO_DATE('" + (Convert.ToDateTime(Straightening.STR_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            //updateQuery.Append(", STR_START_DATE = TO_DATE('" + Straightening.STR_START_DATE.Value.ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')");
            //updateQuery.Append(", STR_END_DATE = TO_DATE('" + Straightening.STR_END_DATE.Value.ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')");
            updateQuery.Append(", STR_START_OPERATOR_CD =" + (Straightening.STR_START_OPERATOR != null ? Straightening.STR_START_OPERATOR.ToString().Trim() : "''"));
            updateQuery.Append(", STR_METALLIC_PLATE_CODE ='" + (Straightening.STR_METALLIC_PLATE_CODE + "'" != null ? Straightening.STR_METALLIC_PLATE_CODE.ToString().Trim() : string.Empty) + "'");
            //updateQuery.Append(", STR_DIM_BEFORE_MIN =" + (Straightening.STR_DIM_BEFORE_MIN != null ? Straightening.STR_DIM_BEFORE_MIN.ToString().Trim() : "''"));
            // updateQuery.Append(", STR_DIM_BEFORE_MAX =" + (Straightening.STR_DIM_BEFORE_MAX != null ? Straightening.STR_DIM_BEFORE_MAX.ToString().Trim() : "''"));
            updateQuery.Append(",  STR_LENGTH_BEFORE_1 =" + (Straightening.STR_LENGTH_BEFORE_1 != null ? Straightening.STR_LENGTH_BEFORE_1.ToString().Trim() : "''"));
            updateQuery.Append(",  STR_LENGTH_BEFORE_2 =" + (Straightening.STR_LENGTH_BEFORE_2 != null ? Straightening.STR_LENGTH_BEFORE_2.ToString().Trim() : "''"));
            updateQuery.Append(",  UPPER_LOWER_ROLLER_ANGLE1 =" + (Straightening.STR_UPPER_LOWER_ROLLER_ANGLE1 != null ? Straightening.STR_UPPER_LOWER_ROLLER_ANGLE1.ToString().Trim() : "''"));
            updateQuery.Append(",  UPPER_LOWER_ROLLER_ANGLE2 =" + (Straightening.STR_UPPER_LOWER_ROLLER_ANGLE2 != null ? Straightening.STR_UPPER_LOWER_ROLLER_ANGLE2.ToString().Trim() : "''"));
            updateQuery.Append(",  UPPER_ROLLER_ELEVATION =" + (Straightening.STR_UPPER_ROLLER_ELEVATION != null ? Straightening.STR_UPPER_ROLLER_ELEVATION.ToString().Trim() : "''"));
            updateQuery.Append(",  STR_TONNAGE =" + (Straightening.STR_TONNAGE != null ? Straightening.STR_TONNAGE.ToString().Trim() : "''"));

            updateQuery.Append(", STR_END_DATE = TO_DATE('" + (Convert.ToDateTime(Straightening.STR_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(",  STR_END_OPERATOR_CD =" + (Straightening.STR_END_OPERATOR != null ? Straightening.STR_END_OPERATOR.ToString().Trim() : "''"));
            updateQuery.Append(",  STR_DECISION =" + (Straightening.STR_DECISION != null ? Straightening.STR_DECISION.ToString().Trim() : "''"));
            updateQuery.Append(",  STR_DIM_CHK =" + (Straightening.STR_DIM_CHK != null ? Straightening.STR_DIM_CHK.ToString() : "''"));
            updateQuery.Append(",  STR_LENGTH_CHK =" + (Straightening.STR_LENGTH_CHK != null ? Straightening.STR_LENGTH_CHK.ToString().Trim() : "''"));
            updateQuery.Append(" , STR_LENGTH_AFTER_1='" + (Straightening.STR_LENGTH_AFTER_1 != null ? Straightening.STR_LENGTH_AFTER_1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_LENGTH_AFTER_2='" + (Straightening.STR_LENGTH_AFTER_2 != null ? Straightening.STR_LENGTH_AFTER_2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_BARS='" + (Straightening.STR_BARS != null ? Straightening.STR_BARS.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_QTY='" + (Straightening.STR_QTY != null ? Straightening.STR_QTY.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NG_BARS1='" + (Straightening.STR_NG_BARS1 != null ? Straightening.STR_NG_BARS1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NG_DETAILS1='" + (Straightening.STR_NG_DETAILS1 != null ? Straightening.STR_NG_DETAILS1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NG_BARS2='" + (Straightening.STR_NG_BARS2 != null ? Straightening.STR_NG_BARS2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NG_DETAILS2='" + (Straightening.STR_NG_DETAILS2 != null ? Straightening.STR_NG_DETAILS2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NG_BARS3='" + (Straightening.STR_NG_BARS3 != null ? Straightening.STR_NG_BARS3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NG_DETAILS3='" + (Straightening.STR_NG_DETAILS3 != null ? Straightening.STR_NG_DETAILS3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_EXT_TEXTURE='" + (Straightening.STR_EXT_TEXTURE != null ? Straightening.STR_EXT_TEXTURE.ToString() : string.Empty) + "'");
            updateQuery.Append(" , RESERVATION_NUMBER='" + (Straightening.STR_RESERVATION_NUMBER != null ? Straightening.STR_RESERVATION_NUMBER.ToString() : string.Empty) + "'");
            updateQuery.Append(" , A_SIDE_FURE1='" + (Straightening.STR_A_SIDE_FURE1 != null ? Straightening.STR_A_SIDE_FURE1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , A_SIDE_FURE2='" + (Straightening.STR_A_SIDE_FURE2 != null ? Straightening.STR_A_SIDE_FURE2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , A_SIDE_FURE3='" + (Straightening.STR_A_SIDE_FURE3 != null ? Straightening.STR_A_SIDE_FURE3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , B_SIDE_FURE1='" + (Straightening.STR_B_SIDE_FURE1 != null ? Straightening.STR_B_SIDE_FURE1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , B_SIDE_FURE2='" + (Straightening.STR_B_SIDE_FURE2 != null ? Straightening.STR_B_SIDE_FURE2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , B_SIDE_FURE3='" + (Straightening.STR_B_SIDE_FURE3 != null ? Straightening.STR_B_SIDE_FURE3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_BEND_STATUS='" + (Straightening.STR_BEND_STATUS != null ? Straightening.STR_BEND_STATUS.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_TABA_NO_1='" + (Straightening.STR_TABA_NO_1 != null ? Straightening.STR_TABA_NO_1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_TABA_NO_2='" + (Straightening.STR_TABA_NO_2 != null ? Straightening.STR_TABA_NO_2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_TABA_NO_3='" + (Straightening.STR_TABA_NO_3 != null ? Straightening.STR_TABA_NO_3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_TABA_NO_4='" + (Straightening.STR_TABA_NO_4 != null ? Straightening.STR_TABA_NO_4.ToString() : string.Empty) + "'");
            updateQuery.Append(" , TP_TEST='" + (Straightening.STR_TP_TEST != null ? Straightening.STR_TP_TEST.ToString() : string.Empty) + "'");
            updateQuery.Append(" , UNIT_TYPE ='" + Straightening.STR_UNIT_TYPE + "'");
            updateQuery.Append(" , FURNACE_NO ='" + Straightening.STR_FURNACE_NO + "'");
            updateQuery.Append(" , STR_FINISHED_QTY_CHK ='" + (Straightening.STR_FINISHED_QTY_CHK != null ? Straightening.STR_FINISHED_QTY_CHK.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_BEFORE_1 ='" + (Straightening.STR_DIM_BEFORE_1 != null ? Straightening.STR_DIM_BEFORE_1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_BEFORE_2 ='" + (Straightening.STR_DIM_BEFORE_2 != null ? Straightening.STR_DIM_BEFORE_2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_BEFORE_3 ='" + (Straightening.STR_DIM_BEFORE_3 != null ? Straightening.STR_DIM_BEFORE_3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_BEFORE_4 ='" + (Straightening.STR_DIM_BEFORE_4 != null ? Straightening.STR_DIM_BEFORE_4.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_BEFORE_5 ='" + (Straightening.STR_DIM_BEFORE_5 != null ? Straightening.STR_DIM_BEFORE_5.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_BEFORE_6 ='" + (Straightening.STR_DIM_BEFORE_6 != null ? Straightening.STR_DIM_BEFORE_6.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_AFTER_1 ='" + (Straightening.STR_DIM_AFTER_1 != null ? Straightening.STR_DIM_AFTER_1.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_AFTER_2 ='" + (Straightening.STR_DIM_AFTER_2 != null ? Straightening.STR_DIM_AFTER_2.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_AFTER_3 ='" + (Straightening.STR_DIM_AFTER_3 != null ? Straightening.STR_DIM_AFTER_3.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_AFTER_4 ='" + (Straightening.STR_DIM_AFTER_4 != null ? Straightening.STR_DIM_AFTER_4.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_AFTER_5 ='" + (Straightening.STR_DIM_AFTER_5 != null ? Straightening.STR_DIM_AFTER_5.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_DIM_AFTER_6 ='" + (Straightening.STR_DIM_AFTER_6 != null ? Straightening.STR_DIM_AFTER_6.ToString() : string.Empty) + "'");
            updateQuery.Append(" , STR_NEXT_PROCESS ='" + (Straightening.STR_NEXT_PROCESS != null ? Straightening.STR_NEXT_PROCESS.ToString() : string.Empty) + "'");
            updateQuery.Append(" , RESTRAIGHTING ='" + (Straightening.STR_RESTRAIGHTING != null ? Straightening.STR_RESTRAIGHTING.ToString() : string.Empty) + "'");
            updateQuery.Append(" , USER_ID ='" + (Straightening.STR_USER_ID != null ? Straightening.STR_USER_ID.ToString() : string.Empty) + "'");
            updateQuery.Append(" , LOCATION_CD ='" + (Straightening.STR_LOCATION_CD != null ? Straightening.STR_LOCATION_CD.ToString() : string.Empty) + "'");
            updateQuery.Append(" , INPUT_LOCATION_CD ='" + (Straightening.INPUT_LOCATION_CD != null ? Straightening.INPUT_LOCATION_CD.ToString() : string.Empty) + "'");
            updateQuery.Append(" , UPDATED_DATE = SYSDATE ");
            updateQuery.Append("WHERE STRAIGHTENING_CD = " + Straightening.STR_STRAIGHTENING_CD);
            ExecuteNonQuery(updateQuery.ToString());

            if (Straightening.STR_MSG_CD > 0)
            {
                updateDateQuery.Append("  UPDATE TRN_MESSAGE SET ");
                updateDateQuery.Append(" MSG_DATE= ");
                updateDateQuery.Append("TO_DATE('" + Straightening.STR_START_DATE.Value.ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')");
                updateDateQuery.Append(" WHERE MSG_CD = " + Straightening.STR_MSG_CD);
                ExecuteNonQuery(updateDateQuery.ToString());
            }
            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_STRAIGHTENING WHERE STRAIGHTENING_CD= '" + Straightening.STR_STRAIGHTENING_CD + "'");

            long msgCD = 0;

            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCD = Convert.ToInt64(cdVal);
            }
            return msgCD;

        }

        public StraighteningDetails GetStraighteningDetailsByChuuban(string chuuban, string gyouban, string machineCD)
        {
             
            StraighteningDetails result = null;
            StringBuilder selectQuery = new StringBuilder();
            StringBuilder selectCountQuery = new StringBuilder();
            int count = 0;

            string whereCondition1 = " PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL";
            string whereCondition2 = "  AND STR.MACHINE_CD = '" + machineCD + "' AND STR.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + "";
            string finalWhere = "";
            finalWhere = whereCondition1 + whereCondition2;
            //SQL

            selectCountQuery.Append(" SELECT COUNT(*) ");
            selectCountQuery.Append(" FROM ");
            selectCountQuery.Append(" TRN_PROCESS_PLAN  PP ");
            selectCountQuery.Append(" INNER JOIN TRN_STRAIGHTENING STR ON STR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
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
            selectQuery.Append(", PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            //selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
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
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");    //COIL_QTY
                                                                               //selectQuery.Append(" ," + Utils.GetMagariKubunStringForSQL("PP.MAGARI_KUBUN"));
                                                                               //selectQuery.Append(" || (CASE WHEN PP.MAGARI_BEND_TOL IS NOT NULL ");
                                                                               //selectQuery.Append("  THEN('-' || TO_CHAR(PP.MAGARI_BEND_TOL, '0.000')) ");
                                                                               //selectQuery.Append("  END)");
                                                                               //selectQuery.Append(" || (CASE WHEN PP.MAGARI_SPAN IS NOT NULL ");
                                                                               //selectQuery.Append("  THEN('-' || PP.MAGARI_SPAN) END)	AS	PP_MAGARI_BEND_TOL  ");

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX ");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");    //AND PR.VER = PP.PROD_VER
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
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");   //EXT_SURF_COND


            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");


            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(",  PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            //selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetMatlUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            //selectQuery.Append("," + Utils.GetCoilDiaWindingValueForDBQuery("PP.COIL_DIA_WINDING") + " AS PP_COIL_DIA_WINDING ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //    selectQuery.Append(", K.REMARKS 	AS	PP_KAKOUIRAI_REMARKS  ");
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

            selectQuery.Append("PP.ELONGATION_PATTERN1,");
            selectQuery.Append("PP.ELONGATION_PATTERN1_MIN_MAX 	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MAX	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MIN ,");
            selectQuery.Append("PP.ELONGATION_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN3_ACTUAL,");

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
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL");



            selectQuery.Append(" ,STR.STRAIGHTENING_CD AS STR_STRAIGHTENING_CD  ");
            selectQuery.Append(" ,STR.PROCESS_PLAN_CD AS STR_PROCESS_PLAN_CD  ");
            selectQuery.Append(" ,STR.MACHINE_CD AS STR_MACHINE_CD  ");
            selectQuery.Append(",(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD =STR.MACHINE_CD ) AS STR_MACHINE_NAME");
            selectQuery.Append(" ,TO_CHAR( STR.STR_START_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_START_DATE_STRING ");
            selectQuery.Append(" , STR.STR_START_OPERATOR_CD AS STR_START_OPERATOR");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
            selectQuery.Append(" , STR.STR_METALLIC_PLATE_CODE AS STR_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,STR.STR_LENGTH_BEFORE_2 ");
            selectQuery.Append(" ,STR.STR_LENGTH_BEFORE_1 ");
            selectQuery.Append(" ,STR.UPPER_LOWER_ROLLER_ANGLE1 AS STR_UPPER_LOWER_ROLLER_ANGLE1");
            selectQuery.Append(", STR.UPPER_LOWER_ROLLER_ANGLE2 AS STR_UPPER_LOWER_ROLLER_ANGLE2");
            selectQuery.Append(" ,STR.UPPER_ROLLER_ELEVATION AS STR_UPPER_ROLLER_ELEVATION ");
            selectQuery.Append(" ,STR.STR_TONNAGE AS STR_TONNAGE ");
            selectQuery.Append(" ,TO_CHAR( STR.STR_END_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_END_DATE_STRING ");
            selectQuery.Append(" , STR.STR_END_OPERATOR_CD");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_END_OPERATOR_CD) AS STR_END_OPERATOR_NAME");
            selectQuery.Append(",STR.STR_DECISION ");
            //  selectQuery.Append("," + Utils.GetProcessDecisionKubunStringForSQL("STR.STR_DECISION") + " AS  STR_DECISION_STRING ");
            selectQuery.Append(", STR.STR_DECISION");
            selectQuery.Append(",STR.STR_DIM_CHK  ");

            selectQuery.Append(",STR.STR_LENGTH_CHK  ");
            selectQuery.Append(",STR.STR_LENGTH_AFTER_1  ");
            selectQuery.Append(",STR.STR_LENGTH_AFTER_2  ");
            selectQuery.Append(",STR.STR_BARS  ");
            selectQuery.Append(",STR.STR_QTY  ");
            selectQuery.Append(",STR.STR_NG_BARS1  ");
            selectQuery.Append(",STR.STR_NG_DETAILS1  ");
            selectQuery.Append(",STR.STR_NG_BARS2  ");
            selectQuery.Append(",STR.STR_NG_DETAILS2  ");
            selectQuery.Append(",STR.STR_NG_BARS3  ");
            selectQuery.Append(",STR.STR_NG_DETAILS3  ");
            selectQuery.Append(",STR.STR_EXT_TEXTURE  ");
            selectQuery.Append(",STR.MSG_CD	AS	STR_MSG_CD  ");
            selectQuery.Append(" ,M.MSG_CONTENTS AS STR_REMARKS ");
            selectQuery.Append(",STR.RESERVATION_NUMBER	AS	STR_RESERVATION_NO  ");
            selectQuery.Append(",STR.A_SIDE_FURE1	AS	STR_A_SIDE_FURE1  ");
            selectQuery.Append(",STR.A_SIDE_FURE2	AS	STR_A_SIDE_FURE2  ");
            selectQuery.Append(",STR.A_SIDE_FURE3	AS	STR_A_SIDE_FURE3  ");
            selectQuery.Append(",STR.STR_BEND_STATUS  ");
            selectQuery.Append(",STR.B_SIDE_FURE1	AS	STR_B_SIDE_FURE1  ");
            selectQuery.Append(",STR.B_SIDE_FURE2	AS	STR_B_SIDE_FURE2  ");
            selectQuery.Append(",STR.B_SIDE_FURE3	AS	STR_B_SIDE_FURE3  ");
            selectQuery.Append(",STR.STR_TABA_NO_1  ");
            selectQuery.Append(",STR.STR_TABA_NO_2  ");
            selectQuery.Append(",STR.STR_TABA_NO_3  ");
            selectQuery.Append(",STR.STR_TABA_NO_4  ");
            selectQuery.Append(",STR.TP_TEST	AS	STR_TP_TEST  ");
            //selectQuery.Append(",STR.UNIT_TYPE	AS	STR_UNIT_TYPE  ");
            selectQuery.Append(", (SELECT STR.UNIT_TYPE FROM TRN_STRAIGHTENING STR INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE  PP.PROCESS_PLAN_CMPL_DATE IS NULL  AND PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND STR.MACHINE_CD = '" + machineCD + "' ");
            selectQuery.Append(" AND STR.UNIT_TYPE IS NOT NULL AND STR.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " ORDER BY  STR.STRAIGHTENING_CD DESC FETCH FIRST ROW ONLY)  AS STR_UNIT_TYPE ");
            selectQuery.Append(",STR.FURNACE_NO	AS	STR_FURNACE_NO  ");
            selectQuery.Append(",STR.STR_FINISHED_QTY_CHK  ");
            selectQuery.Append(",STR.STR_DIM_BEFORE_1  ");
            selectQuery.Append(",STR.STR_DIM_BEFORE_2  ");
            selectQuery.Append(",STR.STR_DIM_BEFORE_3  ");
            selectQuery.Append(",STR.STR_DIM_BEFORE_4  ");
            selectQuery.Append(",STR.STR_DIM_BEFORE_5  ");
            selectQuery.Append(",STR.STR_DIM_BEFORE_6  ");
            selectQuery.Append(",STR.STR_DIM_AFTER_1  ");
            selectQuery.Append(",STR.STR_DIM_AFTER_2  ");
            selectQuery.Append(",STR.STR_DIM_AFTER_3  ");
            selectQuery.Append(",STR.STR_DIM_AFTER_4  ");
            selectQuery.Append(",STR.STR_DIM_AFTER_5  ");
            selectQuery.Append(",STR.STR_DIM_AFTER_6  ");
            selectQuery.Append(",CON.CONDITION_DESCRIPTION ");
            selectQuery.Append(",STR.RESTRAIGHTING	AS	STR_RESTRAIGHTING ");
            selectQuery.Append(",STR.USER_ID	AS	STR_USER_ID  ");
            selectQuery.Append(",STR.CREATED_DATE	AS	STR_CREATED_DATE  ");
            selectQuery.Append(",STR.UPDATED_DATE	AS	STR_UPDATED_DATE  ");
            selectQuery.Append(",STR.STR_DECISION AS STR_DECISION");   //Added by shobhana because decision wise test dia,len message.
            selectQuery.Append(",TO_CHAR( STR.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_CREATED_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( STR.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_UPDATED_DATE_STRING ");
            selectQuery.Append(",CASE ");
            selectQuery.Append(" WHEN STR.CREATED_DATE >= TO_DATE(SYSDATE) -" + days + " THEN ");
            selectQuery.Append(" 1");
            selectQuery.Append(" ELSE");
            selectQuery.Append(" 0");
            selectQuery.Append(" END AS SHOW_DEFAULT ,");
            if (count == 0)
            {
                selectQuery.Append("'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append("'YES' AS PAST_DATA_EXIST ");
            }

            selectQuery.Append(",PP.NYUKA_CD AS PRODUCT_NYUKO_CD ");

            // selectQuery.Append(" ,(SELECT  EM.SHORT_NAME FROM MST_EMPLOYEE EM WHERE EM.EMPLOYEE_CD=PP.EMPLYOEE_CD) AS PP_EMPLOYEE_NAME  ");
            //  selectQuery.Append(" ,PP.INZU || ' ' || PP.INZU_UNIT AS PP_NO_OF_UNITS_STRING ");
            // selectQuery.Append(" ,PP.TENSILE_STRENGTH AS PP_TENSILE_STRENGTH ");
            // selectQuery.Append(" ,PP.YEILD  AS PP_YEILD ");
            //selectQuery.Append(" ,PP.ELONGATION  AS PP_ELONGATION");
            // selectQuery.Append(" ,PP.ELONGATION1 AS PP_ELONGATION1 ");
            //selectQuery.Append(" ,PP.ELONGATION2 AS PP_ELONGATION2 ");
            //selectQuery.Append(" ,PP.PRESS AS PP_PRESS ");
            //selectQuery.Append(" ,PP.PRESS1 AS PP_PRESS1 ");
            //selectQuery.Append(" ,PP.PRESS2 AS PP_PRESS2 ");
            //selectQuery.Append(" ,PP.HARDNESS AS PP_HARDNESS ");
            //selectQuery.Append(" ,PP.HARDNESS1 ||' '|| PP.HARDNESS_2 ||' '|| PP.HARDNESS_3 AS PP_HARDNESS ");
            //selectQuery.Append(" ,PP.HARDNESS1 AS PP_HARDNESS1 ");
            //selectQuery.Append(" ,PP.HARDNESS_2 AS PP_HARDNESS_2  ");
            //selectQuery.Append(" ,PP.HARDNESS_3 AS PP_HARDNESS_3 ");
            //  selectQuery.Append(" ,PP.INZU || ' ' || PP.INZU_UNIT as PP_NO_OF_COILS ");    //No of units
            //  selectQuery.Append(" ,PP.INZU PP_INZU ");
            /*  selectQuery.Append(" ,PP.INZU_UNIT  PP_INZU_UNIT  ");  */    ////unit
                                                                             //   selectQuery.Append(",STR.STR_NEXT_PROCESS  ");
                                                                             //selectQuery.Append(",STR.STR_DIM_AFTER_MIN  ");
                                                                             //selectQuery.Append(",STR.STR_DIM_AFTER_MAX  ");


            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT JOIN TRN_STRAIGHTENING STR ON");
            selectQuery.Append(" STR.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" AND STR.MACHINE_CD = '" + machineCD + "' ");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append(" ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append(" ON M.MSG_CD = STR.MSG_CD  ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ");
            //selectQuery.Append(" ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");

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

            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");

            // selectQuery.Append(" INNER JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");
            /*  selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
              selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
              selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
              selectQuery.Append(" AND STR.MACHINE_CD = '" + machineCD + "' ");
              //selectQuery.Append(" AND str.CREATED_DATE >= SYSDATE - days ");
              selectQuery.Append(" AND STR.CREATED_DATE >= TO_DATE(SYSDATE)-" + days + " ");*/

            selectQuery.Append(" WHERE " + finalWhere);

            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY ");
            result = ExecuteQuerySingleResult<StraighteningDetails>(selectQuery.ToString());
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
        public List<StraighteningDetails> GetStraighteningHistoryByChuuban(string chuuban, string gyouban)
        {

            List<StraighteningDetails> result = null;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");

            selectQuery.Append(" ,STR.STRAIGHTENING_CD AS STR_STRAIGHTENING_CD  ");
            selectQuery.Append(" ,LOCA.SHORT_NAME AS PP_LOCATION_NAME ");
            selectQuery.Append(" ,TO_CHAR(STR.STR_START_DATE, 'yyyy/mm/dd HH24:MI') as STR_START_DATE_STRING ");
            selectQuery.Append(" ,TO_CHAR(STR.STR_END_DATE, 'yyyy/mm/dd HH24:MI') as STR_END_DATE_STRING ");
            selectQuery.Append(" ,CUST.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D "); 
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" , PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN_STRING");
            selectQuery.Append(" ,TRIM(TO_CHAR(PP.LENGTH_TOL_MIN, '990.00')) AS PP_LENGTH_TOL_MIN_STRING");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY ");
            
            selectQuery.Append(" ,CONCAT(DIM_TOL_MAX_CHR, ");
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

            selectQuery.Append(" CONCAT(TO_CHAR(STR_DIM_AFTER_1, 'FM990.000'), CONCAT(CONCAT(CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_5 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_5, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_6 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_6, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_AFTER ");

            selectQuery.Append(" ,CONCAT(TO_CHAR(STR_DIM_BEFORE_1, 'FM990.000'), CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_BEFORE ");
            selectQuery.Append(", STR.STR_NEXT_PROCESS ");
          
            selectQuery.Append(" ,TO_CHAR( STR.STR_START_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_START_DATE_STRING ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            //selectQuery.Append(" ,PP.PP_PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE, ");
            selectQuery.Append(" ,STR.STR_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_1 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_2 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_3 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_4 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_5 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_6 ");
            selectQuery.Append(" ,STR.STR_LENGTH_BEFORE_2 ");
            selectQuery.Append(" ,STR.STR_LENGTH_BEFORE_1 ");
            selectQuery.Append(" ,MESS.MSG_CONTENTS AS STR_REMARKS");
            /* selectQuery.Append(" , " + Utils.GetBendStatusKubun("STR.STR_BEND_STATUS") + " AS STR_BEND_STATUS_DECODE ");
             selectQuery.Append(" , " + Utils.GetExtTextureStatus("STR.STR_EXT_TEXTURE") + " AS STR_EXT_TEXTURE_STATUS_DECODE ");
             selectQuery.Append("," + Utils.GetProcessDecisionKubunStringForSQL("STR.STR_DECISION") + " AS  STR_DECISION_STRING ");
             selectQuery.Append(" CASE WHEN STR.STR_NG_DETAILS1 NOT IN (0) THEN CONCAT(" + Utils.GetExtTextureStatus("STR.STR_NG_DETAILS1") + ",'(' || NVL(STR.STR_NG_BARS1, 0) || ')') ELSE NULL END, ");
             selectQuery.Append(" CASE WHEN STR.STR_NG_DETAILS2 NOT IN (0) THEN CONCAT(CONCAT(', ', " + Utils.GetExtTextureStatus("STR.STR_NG_DETAILS2") + "),'(' || NVL(STR.STR_NG_BARS2, 0) || ')') ELSE NULL END), ");
             selectQuery.Append(" CASE WHEN STR.STR_NG_DETAILS3 NOT IN (0) THEN CONCAT(CONCAT(', ', " + Utils.GetExtTextureStatus("STR.STR_NG_DETAILS3") + "),'(' || NVL(STR.STR_NG_BARS3, 0) || ')') ELSE NULL END)AS STR_NG_DETAILS_ALL ");
             */
            selectQuery.Append(" , STR.STR_BEND_STATUS");
            selectQuery.Append(" , STR.STR_EXT_TEXTURE");
            selectQuery.Append(" , STR.STR_DECISION");
            selectQuery.Append(" , STR.STR_NG_DETAILS1");
            selectQuery.Append(" , STR.STR_NG_BARS1");
            selectQuery.Append(" , STR.STR_NG_DETAILS2");
            selectQuery.Append(" , STR.STR_NG_BARS2");
            selectQuery.Append(" , STR.STR_NG_DETAILS3");
            selectQuery.Append(" , STR.STR_NG_BARS3");

            selectQuery.Append(" ,STR.A_SIDE_FURE1 AS STR_A_SIDE_FURE1 ");
            selectQuery.Append(" ,STR.A_SIDE_FURE2 AS STR_A_SIDE_FURE2 ");
            selectQuery.Append(" ,STR.A_SIDE_FURE3 AS STR_A_SIDE_FURE3 ");
            selectQuery.Append(" ,TO_CHAR( STR.STR_END_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_END_DATE_STRING ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_END_OPERATOR_CD) AS STR_END_OPERATOR_NAME");
            selectQuery.Append(" ,STR.UPPER_LOWER_ROLLER_ANGLE1 AS STR_UPPER_LOWER_ROLLER_ANGLE1");
            selectQuery.Append(" ,STR.UPPER_LOWER_ROLLER_ANGLE2 AS STR_UPPER_LOWER_ROLLER_ANGLE2");
            selectQuery.Append(" ,STR.UPPER_ROLLER_ELEVATION AS STR_UPPER_ROLLER_ELEVATION ");
            selectQuery.Append(" ,STR.STR_TONNAGE ");
            selectQuery.Append(" ,STR.STR_DIM_AFTER_1 ");
            selectQuery.Append(" ,STR.STR_DIM_AFTER_2 ");
            selectQuery.Append(" ,STR.STR_DIM_AFTER_3 ");
            selectQuery.Append(" ,STR.STR_DIM_AFTER_4 ");
            selectQuery.Append(" ,STR.STR_DIM_AFTER_5 ");
            selectQuery.Append(" ,STR.STR_DIM_AFTER_6 ");
            selectQuery.Append(" ,STR.STR_LENGTH_AFTER_2 ");
            selectQuery.Append(" ,STR.STR_LENGTH_AFTER_1 ");

            selectQuery.Append(" ,CONCAT(STR.STR_TABA_NO_1, CONCAT(CONCAT(");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS STR_TABA_NO ");
            selectQuery.Append(" , STR.B_SIDE_FURE1 AS STR_B_SIDE_FURE1 ");
            selectQuery.Append(" , STR.B_SIDE_FURE2 AS STR_B_SIDE_FURE2 ");
            selectQuery.Append(" , STR.B_SIDE_FURE3 AS STR_B_SIDE_FURE3 ");
            selectQuery.Append(" ,(SELECT MAC.MACHINE_NAME AS MACHINE_NAME  FROM MST_MACHINE MAC  WHERE MACHINE_CD =STR.MACHINE_CD ) AS STR_MACHINE_NAME");
            selectQuery.Append(" ,STR.RESERVATION_NUMBER As STR_RESERVATION_NUMBER ");
            selectQuery.Append(" , STR.STR_BARS");
            selectQuery.Append(" , STR.UNIT_TYPE AS STR_UNIT_TYPE");
            // selectQuery.Append(" , STR.STR_BARS || " + Utils.GetMatlUnitForDBQuery("STR.UNIT_TYPE ") + " AS STR_BARS_DECODE ");
            selectQuery.Append(" , STR.STR_QTY AS TOTAL_STR_QTY ");
            selectQuery.Append(" , STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" , NVL(STR.STR_NG_BARS1, 0) + NVL(STR.STR_NG_BARS2, 0) + NVL(STR.STR_NG_BARS3, 0) AS NG_BAR_TOTAL ");
            selectQuery.Append(" ,PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP ");
            selectQuery.Append(" ,PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");

            selectQuery.Append(" ,STR.RESTRAIGHTING AS STR_RESTRAIGHTING ");

            /*selectQuery.Append(",STR.STR_NG_DETAILS1");
            selectQuery.Append(",STR.STR_NG_DETAILS2");
            selectQuery.Append(",STR.STR_NG_DETAILS3");
            selectQuery.Append(",STR.STR_NG_BARS1");
            selectQuery.Append(",STR.STR_NG_BARS2");
            selectQuery.Append(",STR.STR_NG_BARS3");*/

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_STRAIGHTENING STR ON");
            selectQuery.Append(" STR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = STR.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = STR.STR_NEXT_PROCESS ");
            selectQuery.Append(" WHERE ");
            //selectQuery.Append(" PP.ROOT_VER_PRODUCT_KEY = (select MAX(ROOT_VER_PRODUCT_KEY) as ROOT_VER_PRODUCT_KEY from TRN_PROCESS_PLAN where CHUUBAN='" + chuuban + "'"); // ROWNUM < =10 :diplay only latest 10 record in History grid(2nd Tab)
            //selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "')");
            //selectQuery.Append(" AND ");
            selectQuery.Append(" PP.CHUUBAN='" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "'");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD DESC");
            result = ExecuteQueryMultiResult<StraighteningDetails>(selectQuery.ToString());
            return result;
        }

        public StraighteningDetails GetStraighteningDetailsByChuubanGyouban(string chuuban, string gyouban)
        {
            StraighteningDetails result = null;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");            
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");    //COIL_QTY
                                                                               //selectQuery.Append(" ," + Utils.GetMagariKubunStringForSQL("PP.MAGARI_KUBUN"));
                                                                               //selectQuery.Append(" || (CASE WHEN PP.MAGARI_BEND_TOL IS NOT NULL ");
                                                                               //selectQuery.Append("  THEN('-' || TRIM(LEADING ' ' FROM TO_CHAR(PP.MAGARI_BEND_TOL,'0.000'))) ");
                                                                               //selectQuery.Append("  END)");
                                                                               //selectQuery.Append(" || (CASE WHEN PP.MAGARI_SPAN IS NOT NULL ");
                                                                               //selectQuery.Append("  THEN('-' || PP.MAGARI_SPAN) END)	AS	PP_MAGARI_BEND_TOL  ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX ");
            
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            //selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");    //AND PR.VER = PP.PROD_VER
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

            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(" ,PCK.PACKING_NAME	AS	PP_PACKING  ");

            // selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetMatlUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(", CON.CONDITION_DESCRIPTION ");


            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            // selectQuery.Append("," + Utils.GetCoilDiaWindingValueForDBQuery("PP.COIL_DIA_WINDING") + " AS PP_COIL_DIA_WINDING ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //selectQuery.Append(" ,K.REMARKS AS PP_KAKOUIRAI_REMARKS ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");



            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");

            selectQuery.Append(" ,STR.STRAIGHTENING_CD AS STR_STRAIGHTENING_CD  ");
            selectQuery.Append(" ,TO_CHAR( STR.STR_START_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_START_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( STR.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_CREATED_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( STR.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS STR_UPDATED_DATE_STRING ");
            selectQuery.Append(" ,STR.UPDATED_DATE  AS STR_UPDATED_DATE ");
            selectQuery.Append(", STR.CREATED_DATE AS     STR_CREATED_DATE");

            selectQuery.Append(" , STR.STR_START_OPERATOR_CD AS STR_START_OPERATOR");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = STR_START_OPERATOR_CD) AS STR_START_OPERATOR_NAME");
            selectQuery.Append(" ,STR.TP_TEST AS STR_TP_TEST   ");
            selectQuery.Append(",(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD =STR.MACHINE_CD ) AS STR_MACHINE_NAME");
            selectQuery.Append(" , STR.STR_METALLIC_PLATE_CODE AS STR_METALLIC_PLATE_CODE ");

            selectQuery.Append(" ,STR.STR_DIM_BEFORE_1 AS STR_DIM_BEFORE_1 ");
            selectQuery.Append("  ,STR.STR_DIM_BEFORE_2 AS STR_DIM_BEFORE_2 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_3 AS STR_DIM_BEFORE_3 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_4 AS STR_DIM_BEFORE_4 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_5 ");
            selectQuery.Append(" ,STR.STR_DIM_BEFORE_6 ");
            selectQuery.Append(" ,STR.FURNACE_NO AS STR_FURNACE_NO ");
            selectQuery.Append(" , STR.STR_LENGTH_BEFORE_2 AS STR_LENGTH_BEFORE_2 ");
            selectQuery.Append("  ,STR.STR_LENGTH_BEFORE_1 AS STR_LENGTH_BEFORE_1");
            selectQuery.Append(" ,STR.RESERVATION_NUMBER As STR_RESERVATION_NUMBER ");
            selectQuery.Append(" ,STR.UPPER_LOWER_ROLLER_ANGLE1 AS STR_UPPER_LOWER_ROLLER_ANGLE1");
            selectQuery.Append(", STR.UPPER_LOWER_ROLLER_ANGLE2 AS STR_UPPER_LOWER_ROLLER_ANGLE2");

            selectQuery.Append(" ,STR.UPPER_ROLLER_ELEVATION AS STR_UPPER_ROLLER_ELEVATION ");
            selectQuery.Append(" ,STR.STR_TONNAGE AS STR_TONNAGE ");
            selectQuery.Append(" ,STR.RESTRAIGHTING AS STR_RESTRAIGHTING ");

            selectQuery.Append(" ,TO_CHAR( STR.STR_END_DATE,'yyyy/mm/dd HH24:MI:SS' ) AS STR_END_DATE_STRING ");


            selectQuery.Append(" ,P1.PROCESS_NAME AS PP_PROCESS_1, ");
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
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL");
            selectQuery.Append(",PP.NYUKA_CD AS PRODUCT_NYUKO_CD ");
            selectQuery.Append(",STR.STR_DECISION ");

            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE ");

            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_STRAIGHTENING  STR ON");
            selectQuery.Append(" STR.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            //selectQuery.Append("  LEFT JOIN TRN_KAKOUIRAI K  ");
            //selectQuery.Append("  ON PP.KAKOUIRAI_CD   = K.KAKOUIRAI_CD  ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");

            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
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

            selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
            selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" ORDER BY STR.STRAIGHTENING_CD DESC");
            result = ExecuteQuerySingleResult<StraighteningDetails>(selectQuery.ToString());
            return result;
        }

        public void InsertStraighteningDetails(StraighteningDetails straighteningDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_STRAIGHTENING(");
            insertQuery.Append(" STRAIGHTENING_CD,");
            insertQuery.Append(" PROCESS_PLAN_CD,");
            insertQuery.Append(" MACHINE_CD,");
            insertQuery.Append(" STR_START_DATE,");
            insertQuery.Append(" STR_START_OPERATOR_CD,");
            insertQuery.Append(" STR_METALLIC_PLATE_CODE,");
            insertQuery.Append(" STR_DIM_BEFORE_1,");
            insertQuery.Append(" STR_DIM_BEFORE_2,");
            insertQuery.Append(" STR_DIM_BEFORE_3,");
            insertQuery.Append(" STR_DIM_BEFORE_4,");
            insertQuery.Append(" STR_DIM_BEFORE_5,");
            insertQuery.Append(" STR_DIM_BEFORE_6,");
            insertQuery.Append(" FURNACE_NO,");
            insertQuery.Append(" STR_LENGTH_BEFORE_1,");
            insertQuery.Append(" STR_LENGTH_BEFORE_2,");
            insertQuery.Append(" RESERVATION_NUMBER,");
            insertQuery.Append(" UPPER_LOWER_ROLLER_ANGLE1,");
            insertQuery.Append(" UPPER_LOWER_ROLLER_ANGLE2,");
            insertQuery.Append(" UPPER_ROLLER_ELEVATION,");
            insertQuery.Append(" STR_TONNAGE,");
            insertQuery.Append(" STR_DIM_CHK,");
            insertQuery.Append(" RESTRAIGHTING,");
            insertQuery.Append(" STR_TABA_NO_1,");
            insertQuery.Append(" STR_TABA_NO_2,");
            insertQuery.Append(" STR_TABA_NO_3,");
            insertQuery.Append(" STR_TABA_NO_4,");
            insertQuery.Append(" TP_TEST,");
            insertQuery.Append(" PROCESS_KUBUN_CD, ");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" LOCATION_CD, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");
            insertQuery.Append(" CREATED_DATE,");
            insertQuery.Append(" UPDATED_DATE ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("TRN_STRAIGHTENING_SEQ.NEXTVAL");
            insertQuery.Append(",'" + straighteningDetails.PP_PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_MACHINE_CD + "'");
            insertQuery.Append(", TO_DATE('" + (Convert.ToDateTime(straighteningDetails.STR_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            // insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(straighteningDetails.STR_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')");
            insertQuery.Append(",'" + straighteningDetails.STR_START_OPERATOR + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_METALLIC_PLATE_CODE + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_2 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_3 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_4 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_5 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_BEFORE_6 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_FURNACE_NO + "'");     //
            insertQuery.Append(",'" + straighteningDetails.STR_LENGTH_BEFORE_1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_LENGTH_BEFORE_2 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_RESERVATION_NUMBER + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_UPPER_LOWER_ROLLER_ANGLE1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_UPPER_LOWER_ROLLER_ANGLE2 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_UPPER_ROLLER_ELEVATION + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TONNAGE + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_DIM_CHK + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_RESTRAIGHTING + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_1 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_2 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_3 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TABA_NO_4 + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_TP_TEST + "'");
            insertQuery.Append(",'" + straighteningDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_USER_ID + "'");
            insertQuery.Append(",'" + straighteningDetails.STR_LOCATION_CD + "'");
            insertQuery.Append(",'" + straighteningDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");



            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
        }

        public List<StraighteningDetails> GetStraightening24MachineList(string machineCd)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append("STR.MACHINE_CD AS STR_MACHINE_CD,  ");
            selectQuery.Append(" STR.STR_DECISION AS STR_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,");
            //selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append(" STR.STR_METALLIC_PLATE_CODE  AS STR_METALLIC_PLATE_CODE,	");
            selectQuery.Append("  PP.PROD_DIM_D|| ''||PP.PROD_shape  AS PP_PROD_SHAPE,	");
            //selectQuery.Append(" PP.NO_OF_UNITS || ' C' AS PP_NO_OF_UNITS_DECODE ,	");

            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,	");
            selectQuery.Append(" PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE  AS  PP_MATL_DIM_D_SHAPE,	");
            selectQuery.Append("  STR.STR_DIM_AFTER_1 AS STR_DIE_DIM,	");
            selectQuery.Append(" CONCAT (STR_TABA_NO_1, CASE WHEN STR_TABA_NO_2 IS NOT NULL THEN CONCAT ('-', STR_TABA_NO_2) ELSE NULL END) AS STR_TABA_NO_1_2");

            selectQuery.Append(" ,CONCAT(TO_CHAR(STR_DIM_BEFORE_1, 'FM990.000'), CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS STR_DIM_BEFORE_STRING ");

            selectQuery.Append(" ,CONCAT(TO_CHAR(STR_DIM_AFTER_1, 'FM990.000'), CONCAT(CONCAT(CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_5 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_5, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_6 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_6, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS STR_DIM_AFTER_STRING, ");

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
            selectQuery.Append(" FROM TRN_STRAIGHTENING STR ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON STR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON STR.MSG_CD=MSG.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");

            selectQuery.Append(" WHERE  STR.MACHINE_CD  = '" + machineCd + "' AND NVL (STR.STR_END_DATE, STR.STR_START_DATE) > SYSDATE - 1");
            // selectQuery.Append("  AND ROWNUM <= 10");
            selectQuery.Append(" ORDER BY ORDER_CLAUSE DESC");
            List<StraighteningDetails> result = ExecuteQueryMultiResult<StraighteningDetails>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY :Namarta Uabale on 2023/01/16       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Straighteing1 process data according to search criteria        
        /// </returns>
        /// 
        public List<StraighteningDetails> GetStraighteningDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria, int rowLimit,string cultureName)
        {
            List<StraighteningDetails> result = new List<StraighteningDetails>();
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MAC.MACHINE_NAME AS STR_MACHINE_NAME ,");
            selectQuery.Append(" TO_CHAR(STRAI.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(STRAI.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING,");
            selectQuery.Append(" EMP.SHORT_NAME AS STR_END_OPERATOR_NAME,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" STRAI.STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN_STRING,");
            selectQuery.Append(" TRIM(TO_CHAR(PP.LENGTH_TOL_MIN,'990.00')) AS PP_LENGTH_TOL_MIN_STRING,");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS STR_REMARKS, ");
            selectQuery.Append(" INTE.INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 ,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 , ");
            selectQuery.Append(" PP.PROD_DIM_D, ");

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
            selectQuery.Append("CONCAT(STR_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', STR_TABA_NO_2) ");
            selectQuery.Append(" ELSE   NULL ");
            selectQuery.Append(" END, CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', STR_TABA_NO_3) ");
            selectQuery.Append(" ELSE NULL  END   ),");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', STR_TABA_NO_4) ");
            selectQuery.Append(" ELSE   NULL  END ");
            selectQuery.Append(" )) AS ALL_TABA_NO, ");

            selectQuery.Append(" CONCAT(TO_CHAR(STR_DIM_BEFORE_1, 'FM990.000'), CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_BEFORE, ");

            selectQuery.Append(" CONCAT(TO_CHAR(STR_DIM_AFTER_1, 'FM990.000'), CONCAT(CONCAT(CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_5 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_5, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_6 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_6, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_AFTER, ");
            selectQuery.Append(" STRAI.STR_LENGTH_AFTER_1,");
            selectQuery.Append(" STRAI.STR_LENGTH_AFTER_2,");

            selectQuery.Append(" STRAI.STR_DECISION AS STR_DECISION, ");
            selectQuery.Append(" STRAI.STR_BARS,");
            selectQuery.Append(" STRAI.UNIT_TYPE, ");
            selectQuery.Append(" STRAI.STR_QTY AS TOTAL_STR_QTY, ");

            selectQuery.Append(" STRAI.STR_NG_BARS1, ");
            if(cultureName == "en")
            {
                selectQuery.Append("DEFECT1.OTHER_NAME_SHORT AS STR_NG_DETAILS1_DECODE,");
                selectQuery.Append("DEFECT2.OTHER_NAME_SHORT AS STR_NG_DETAILS2_DECODE,");
                selectQuery.Append("DEFECT3.OTHER_NAME_SHORT AS STR_NG_DETAILS3_DECODE,");
            }
            else
            {
                selectQuery.Append("DEFECT1.PROC_DEFECT_SHORT AS STR_NG_DETAILS1_DECODE,");
                selectQuery.Append("DEFECT2.PROC_DEFECT_SHORT AS STR_NG_DETAILS2_DECODE,");
                selectQuery.Append("DEFECT3.PROC_DEFECT_SHORT AS STR_NG_DETAILS3_DECODE,");
            }

                selectQuery.Append(" STRAI.STR_NG_BARS2, ");
            
            selectQuery.Append(" STRAI.STR_NG_BARS3, ");
            

            selectQuery.Append(" PP.DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING STRAI ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STRAI.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STRAI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = STRAI.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = STRAI.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=STRAI.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = STRAI.STR_NEXT_PROCESS ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON STRAI.STR_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON STRAI.STR_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON STRAI.STR_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");
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

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            //else
            //{
            //    query += " ORDER BY STR_END_DATE_STRING ASC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<StraighteningDetails>(query);
            return result;
        }


        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/05/12       
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Straighteing1 process data according to search criteria        
        /// </returns>
        /// 
        public List<StraighteningDetails> GetStraighteningDailyReportOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridsearchresult)
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
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME,PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN_STRING,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX_STRING,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN_SINGLE,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX_SINGLE,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR,");
            //selectQuery.Append(" WHEN STR_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_WINDING AS COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER, ");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN AS MAGARI_KUBUN, ");
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
            selectQuery.Append(" PP.PROCESS_1_CD AS PP_PROCESS_1_INT,");
            selectQuery.Append(" PP.PROCESS_2_CD AS PP_PROCESS_2_INT,");
            selectQuery.Append(" PP.PROCESS_3_CD AS PP_PROCESS_3_INT,");
            selectQuery.Append(" PP.PROCESS_4_CD AS PP_PROCESS_4_INT,");
            selectQuery.Append(" PP.PROCESS_5_CD AS PP_PROCESS_5_INT,");
            selectQuery.Append(" PP.PROCESS_6_CD AS PP_PROCESS_6_INT,");
            selectQuery.Append(" PP.PROCESS_7_CD AS PP_PROCESS_7_INT,");
            selectQuery.Append(" PP.PROCESS_8_CD AS PP_PROCESS_8_INT,");
            selectQuery.Append(" PP.PROCESS_9_CD AS PP_PROCESS_9_INT,");
            selectQuery.Append(" PP.PROCESS_10_CD AS PP_PROCESS_10_INT,");
            selectQuery.Append(" PP.PROCESS_11_CD AS PP_PROCESS_11_INT,");
            selectQuery.Append(" PP.PROCESS_12_CD AS PP_PROCESS_12_INT,");
            selectQuery.Append(" PP.PROCESS_13_CD AS PP_PROCESS_13_INT,");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");
            selectQuery.Append(" PP.HARDNESS_PATTERN3_ACTUAL AS HARDNESS_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1,");
            selectQuery.Append(" PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX,");
            selectQuery.Append(" PP.YEILD_PATTERN3_ACTUAL AS YEILD_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1,");
            selectQuery.Append(" PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX,");
            selectQuery.Append(" PP.PRESS_PATTERN3_ACTUAL AS PRESS_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.DECARB_1 AS DECARB_1,");
            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1,");
            selectQuery.Append(" PP.DECARB_2 AS DECARB_2,");
            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2,");
            selectQuery.Append(" PP.DECARB_3 AS DECARB_3,");
            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.RANK_STD AS RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION,");
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK,");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PP_PROCESS_PLAN_KUBUN,");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN,");
            selectQuery.Append(" PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN,");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN,");
            selectQuery.Append(" PP.KAKOUIARI_QTY AS KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE , 'yyyy/mm/dd') AS PROCESS_PLAN_PRINT_DATE_STRING,");
            selectQuery.Append(" MACH.MACHINE_NAME AS STR_MACHINE_NAME,");
            selectQuery.Append(" LOCA.SHORT_NAME AS LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(STR.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_TIME,");
            selectQuery.Append(" EMPL.SHORT_NAME AS STR_START_OPERATOR_NAME,");
            selectQuery.Append(" STR.TP_TEST AS STR_TP_TEST,");
            selectQuery.Append(" STR.STR_METALLIC_PLATE_CODE AS STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" STR.FURNACE_NO AS STR_FURNACE_NO,");
            

            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_1, 'FM990.000') AS STR_DIM_BEFORE_1_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_2, 'FM990.000') AS STR_DIM_BEFORE_2_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_3, 'FM990.000') AS STR_DIM_BEFORE_3_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_4, 'FM990.000')AS STR_DIM_BEFORE_4_STRING,");

            
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_1 AS STR_LENGTH_BEFORE_1,");
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_2 AS STR_LENGTH_BEFORE_2,");
            selectQuery.Append(" STR.UPPER_LOWER_ROLLER_ANGLE1 AS STR_UPPER_LOWER_ROLLER_ANGLE1,");
            selectQuery.Append(" STR.UPPER_LOWER_ROLLER_ANGLE2 AS STR_UPPER_LOWER_ROLLER_ANGLE2,");
            selectQuery.Append(" STR.UPPER_ROLLER_ELEVATION AS STR_UPPER_ROLLER_ELEVATION,");
            selectQuery.Append(" STR.RESERVATION_NUMBER AS STR_RESERVATION_NUMBER,");
            selectQuery.Append(" STR.STR_TONNAGE AS STR_TONNAGE,");
            selectQuery.Append(" STR.RESTRAIGHTING AS STR_RESTRAIGHTING, ");

            selectQuery.Append(" TO_CHAR(STR.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING,");
            selectQuery.Append(" EMPL1.SHORT_NAME AS STR_END_OPERATOR_NAME,");
           
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_1, 'FM990.000') AS STR_DIM_AFTER_1_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_2, 'FM990.000') AS STR_DIM_AFTER_2_STRING ,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_3, 'FM990.000') AS STR_DIM_AFTER_3_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_4, 'FM990.000') AS STR_DIM_AFTER_4_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_5, 'FM990.000') AS STR_DIM_AFTER_5_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_6, 'FM990.000') AS STR_DIM_AFTER_6_STRING,");

            selectQuery.Append(" STR.STR_LENGTH_AFTER_1 AS STR_LENGTH_AFTER_1,");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_2 AS STR_LENGTH_AFTER_2,");
            selectQuery.Append(" STR.STR_BARS AS STR_BARS,");
            selectQuery.Append(" STR.UNIT_TYPE AS UNIT_TYPE,");
            selectQuery.Append(" STR.STR_QTY AS TOTAL_STR_QTY,");
            selectQuery.Append(" STR.A_SIDE_FURE1 AS STR_A_SIDE_FURE1,");
            selectQuery.Append(" STR.A_SIDE_FURE2 AS STR_A_SIDE_FURE2,");
            selectQuery.Append(" STR.A_SIDE_FURE3 AS STR_A_SIDE_FURE3,");
            selectQuery.Append(" STR.B_SIDE_FURE1 AS STR_B_SIDE_FURE1,");
            selectQuery.Append(" STR.B_SIDE_FURE2 AS STR_B_SIDE_FURE2,");
            selectQuery.Append(" STR.B_SIDE_FURE3 AS STR_B_SIDE_FURE3,");
            selectQuery.Append(" STR.STR_TABA_NO_1 AS STR_TABA_NO_1,");
            selectQuery.Append(" STR.STR_TABA_NO_2 AS STR_TABA_NO_2,");
            selectQuery.Append(" STR.STR_TABA_NO_3 AS STR_TABA_NO_3,");
            selectQuery.Append(" STR.STR_TABA_NO_4 AS STR_TABA_NO_4,");
            selectQuery.Append(" STR.STR_EXT_TEXTURE AS STR_EXT_TEXTURE,");
            selectQuery.Append(" STR.STR_BEND_STATUS AS STR_BEND_STATUS,");
            selectQuery.Append(" STR.STR_NG_BARS1 AS STR_NG_BARS1,");
            selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP,");
            selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN,");
            selectQuery.Append(" STR.STR_NG_BARS2 AS STR_NG_BARS2,");
            selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP,");
            selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN,");
            selectQuery.Append(" STR.STR_NG_BARS3 AS STR_NG_BARS3,");
            selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP,");
            selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN,");
            selectQuery.Append(" STR.STR_DECISION AS STR_DECISION");
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
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13 ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS STR_REMARKS ");
            selectQuery.Append("  FROM ");

            selectQuery.Append(" TRN_STRAIGHTENING STR ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = STR.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL1 ON EMPL1.EMPLOYEE_CD = STR.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
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
            }
            else if (gridsearchresult.ROW_LIMIT > 0 && searchFields.Count <= 0)
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
            return ExecuteQueryMultiResult<StraighteningDetails>(query);
        }
        public List<NameValue> GetMachineNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
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
        public List<StraighteningDetails> GetThabaDetailsByThabaNumber(int processPlanCD, string Thaba_No_String)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new List<StraighteningDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT STR.STRAIGHTENING_CD AS STR_STRAIGHTENING_CD  ");
            selectQuery.Append(" ,STR.STR_DECISION AS STR_DECISION");
            selectQuery.Append(" ,STR.RESTRAIGHTING AS STR_RESTRAIGHTING");
            selectQuery.Append(" ,STR.STR_TABA_NO_1");
            selectQuery.Append(" ,STR.STR_TABA_NO_2");
            selectQuery.Append(" ,STR.STR_TABA_NO_3");
            selectQuery.Append(" ,STR.STR_TABA_NO_4");
            selectQuery.Append(" ,CONCAT  (STR.STR_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN STR.STR_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', STR.STR_TABA_NO_4) ELSE NULL END )) AS STR_TABA_NO ");

            selectQuery.Append(" FROM TRN_STRAIGHTENING STR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE STR.PROCESS_PLAN_CD ='" + processPlanCD + "' ");
            //selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND STR.STR_DECISION =" + decisionVal + " ");
            selectQuery.Append(" AND (STR.STR_TABA_NO_1 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_2 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_3 IN ('" + Thaba_No_String + "') OR STR.STR_TABA_NO_4 IN ('" + Thaba_No_String + "') ) ORDER BY STR.STRAIGHTENING_CD ");


            result = ExecuteQueryMultiResult<StraighteningDetails>(selectQuery.ToString());
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
        public int GetRowCountForStraighteningFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM TRN_STRAIGHTENING STRAI ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STRAI.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STRAI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = STRAI.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = STRAI.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=STRAI.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = STRAI.STR_NEXT_PROCESS ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON STRAI.STR_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON STRAI.STR_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON STRAI.STR_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");

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
        public int GetRowCountForStraighteningDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STRAIGHTENING STR ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = STR.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL1 ON EMPL1.EMPLOYEE_CD = STR.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
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

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/07/17      
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>        
        /// To bind machine name of Roll Straightening and Press Straightening process
        /// </returns>
        /// 
        public List<NameValue> GetStraighteningMachineList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" AND MACHINE_TYPE = '" + UIConstants.K_MACHINEGROUP_STRAIGHTENING + "'");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
    }
}
