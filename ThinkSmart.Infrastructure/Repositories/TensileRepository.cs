using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class TensileRepository : BaseRepository, ITensileRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly IConfiguration _configuration;
        private string tensile_Seq;
        public TensileRepository(IConfiguration configuration, LocalizationServices localization) : base(configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
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
        public TensileDetails GetTensileDetailsByChuuban(string chuuban, short? gyouban)
        {
            TensileDetails TensileDetails = new TensileDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            //selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            //selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS  PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");    //COIL_QTY
            selectQuery.Append(" ,PP.NYUKA_CD AS PP_NYUKA_CD ");
            selectQuery.Append(",  pp.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            //selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", PP.NYUKA_CD AS PP_NYUKA_CD  ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
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
            selectQuery.Append(" ,COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            selectQuery.Append(" ,K.REMARKS AS PP_KAKOUIRAI_REMARKS ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,TO_CHAR ( PP.PROCESS_PLAN_CMPL_DATE,'yyyy/MM/dd') AS PP_PROCESS_PLAN_CMPL_DATE");

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
            selectQuery.Append(",PP.PROCESS_PLAN_START_DATE");
            selectQuery.Append(",TS.MI_TS_CD ");
            //selectQuery.Append(" ,TS.TS_DECISION AS DECISION_OK ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(", (SELECT MAX(TS1.DETAIL_NO) FROM  TRN_PROCESS_PLAN PP1 INNER JOIN TRN_MI_TENSILE_STRENGTH   TS1 ON PP1.PROCESS_PLAN_CD = TS1.PROCESS_PLAN_CD WHERE  PP1.CHUUBAN = '" + chuuban + "' AND  PP1.GYOUBAN = '" + gyouban + "' AND PP1.PROCESS_PLAN_CMPL_DATE IS NULL ) AS TS_LAST_DETAIL_NO");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("  LEFT JOIN TRN_MI_TENSILE_STRENGTH TS ON TS.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON PP.LOCATION_CD =LOC.LOCATION_CD");
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
            selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
            selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" ORDER BY TS.MI_TS_CD DESC");
            TensileDetails = ExecuteQuerySingleResult<TensileDetails>(selectQuery.ToString());
            return TensileDetails;
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
        public List<NameValue> GetJISList()
        {
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT JIS.JIS_TEST_SPEC_CD AS CD ");
            selectQuery.Append(" ,JIS.JIS_TEST_SPEC_SHORT AS NAME ");

            selectQuery.Append(" FROM MST_JIS_TEST_SPEC JIS");
            selectQuery.Append(" WHERE JIS.IN_ACTIVE_FLAG =0 ");
            selectQuery.Append(" ORDER BY LOWER (JIS.JIS_TEST_SPEC_SHORT)");

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public TensileDetails InsertTensileDetails(TensileDetails tensileDetails)
        {
            tensile_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Tensile").Value);
            TensileDetails result = new TensileDetails();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_MI_TENSILE_STRENGTH(");
            insertQuery.Append(" 	MI_TS_CD ");
            insertQuery.Append(" ,	PROCESS_PLAN_CD	");
            insertQuery.Append(" ,	DETAIL_NO ");
            insertQuery.Append(" ,	LOCATION_CD	");
            insertQuery.Append(" ,	MSG_CD	");
            insertQuery.Append(" ,	TS_TEST_OPERATOR_CD	");
            insertQuery.Append(" ,	TS_TEST_DATE ");
            insertQuery.Append(" ,	TS_FURNACE_NO ");
            insertQuery.Append(" ,	TEST_SPECIMEN_CD ");
            insertQuery.Append(" ,	IS_ACT_DIM_RANGE_CHECK ");
            insertQuery.Append(" ,	TS_ACT_DIM ");
            insertQuery.Append(" ,	TS_ACT_DIM_SHAPE ");
            insertQuery.Append(" ,	TS_TEST_SPECIMEN_DIM ");
            insertQuery.Append(" ,	TS_GAUGE_LENGTH	");
            insertQuery.Append(" ,	TS_MAX_LOAD_N ");
            insertQuery.Append(" ,	TS_YIELD_LOAD_N	");
            insertQuery.Append(" ,	TS_EXTENDED_LENGTH	");
            insertQuery.Append(" ,	TS_CUT_DIM	");
            insertQuery.Append(" ,	TS_TEST_IMPACT_1 ");
            insertQuery.Append(" ,	TS_TEST_IMPACT_2 ");
            insertQuery.Append(" ,	TS_TEST_IMPACT_3");
            insertQuery.Append(" ,	TS_TENSILE	");
            insertQuery.Append(" ,	TS_YIELD_STRENGTH ");
            insertQuery.Append(" ,	TS_ELONGATION ");
            insertQuery.Append(" ,	TS_REDUCTION ");
            insertQuery.Append(" ,	TS_DECISION	");
            //insertQuery.Append(" ,	TS_NEXT_PROCESS	"); 
            insertQuery.Append(" ,	PROCESS_KUBUN_CD	");
            insertQuery.Append(" ,	USER_ID	");
            insertQuery.Append(" ,	INPUT_LOCATION_CD ");
            insertQuery.Append(" ,	CREATED_DATE ");
            insertQuery.Append(" ,	UPDATED_DATE ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            if (string.IsNullOrEmpty(tensile_Seq))
            {
                insertQuery.Append(" TRN_MI_TENSILE_STRENGTH_SEQ.NEXTVAL ");
            }
            else
            {
                insertQuery.Append(tensile_Seq);
            }

            insertQuery.Append(",'" + tensileDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + tensileDetails.DETAIL_NO + "'");
            insertQuery.Append(",'" + tensileDetails.LOCATION_CD + "'");
            insertQuery.Append(",'" + tensileDetails.MSG_CD + "'");
            insertQuery.Append(",'" + tensileDetails.TS_TEST_OPERATOR_CD + "'");
            //insertQuery.Append(",'" + .TS_TEST_DATE + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(tensileDetails.TS_TEST_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + tensileDetails.TS_FURNACE_NO + "'");
            insertQuery.Append(",'" + tensileDetails.TEST_SPECIMEN_CD + "'");
            insertQuery.Append(",'" + tensileDetails.IS_ACT_DIM_RANGE_CHECK + "'");
            insertQuery.Append(",'" + tensileDetails.TS_ACT_DIM + "'");
            insertQuery.Append(",'" + tensileDetails.TS_ACT_DIM_SHAPE + "'");
            insertQuery.Append(",'" + tensileDetails.TS_TEST_SPECIMEN_DIM + "'");
            insertQuery.Append(",'" + tensileDetails.TS_GAUGE_LENGTH + "'");
            insertQuery.Append(",'" + tensileDetails.TS_MAX_LOAD_N + "'");
            insertQuery.Append(",'" + tensileDetails.TS_YIELD_LOAD_N + "'");
            insertQuery.Append(",'" + tensileDetails.TS_EXTENDED_LENGTH + "'");
            insertQuery.Append(",'" + tensileDetails.TS_CUT_DIM + "'");
            insertQuery.Append(",'" + tensileDetails.TS_TEST_IMPACT_1 + "'");
            insertQuery.Append(",'" + tensileDetails.TS_TEST_IMPACT_2 + "'");
            insertQuery.Append(",'" + tensileDetails.TS_TEST_IMPACT_3 + "'");
            insertQuery.Append(",'" + tensileDetails.TS_TENSILE + "'");
            insertQuery.Append(",'" + tensileDetails.TS_YIELD_STRENGTH + "'");
            insertQuery.Append(",'" + tensileDetails.TS_ELONGATION + "'");
            insertQuery.Append(",'" + tensileDetails.TS_REDUCTION + "'");
            insertQuery.Append(",'" + tensileDetails.TS_DECISION + "'");
            //insertQuery.Append(",'" + tensileDetails.TS_NEXT_PROCESS + "'");
            insertQuery.Append(",'" + tensileDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",'" + tensileDetails.USER_ID + "'");
            insertQuery.Append(",'" + tensileDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            result = GetTensileDetailsByChuuban(tensileDetails.PP_CHUUBAN, tensileDetails.PP_GYOUBAN);
            return result;
        }

        public ThabaDetails GetThabaDetailsByThabaNumber(long processPlanCD, short tabaNo)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, TS.MI_TS_CD ");
            selectQuery.Append(" FROM TRN_MI_TENSILE_STRENGTH TS INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = TS.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD =" + processPlanCD + " ");

            selectQuery.Append(" AND TS.TS_DECISION = " + decisionVal);
            if (tabaNo > 0)
            {
                selectQuery.Append(" AND TS.DETAIL_NO ='" + tabaNo + "' ");
            }

            selectQuery.Append(" ORDER BY TS.MI_TS_CD ASC ");

            return ExecuteQuerySingleResult<ThabaDetails>(selectQuery.ToString());


        }
        public TensileDetails GetFurnanceNo(string chuuban, short? gyouban, int detail_No)
        {
            int process_plan_cd = GetProcessPlanCDByChuuban(chuuban, gyouban);

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT HD.HD_FURNACE_NO ");
            selectQuery.Append(" FROM TRN_MI_HARDNESS HD");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON HD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" WHERE HD.PROCESS_PLAN_CD ='" + process_plan_cd + "' AND DETAIL_NO='" + detail_No + "'");
            return ExecuteQuerySingleResult<TensileDetails>(selectQuery.ToString());

        }

        public int GetProcessPlanCDByChuuban(string chuuban, short? gyouban)
        {
            int processPlanCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT PROCESS_PLAN_CD");
            query.Append(" FROM TRN_PROCESS_PLAN ");
            query.Append(" WHERE CHUUBAN ='" + chuuban + "' ");
            query.Append(" AND GYOUBAN ='" + gyouban + "' ");
            query.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
            //query.Append(" WHERE USER_ID=" + userID);
            processPlanCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return processPlanCD;

        }

        public List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long tensileCD)
        {
            var result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, TS.TS_DECISION AS DECISION, TS.CREATED_DATE ,");
            selectQuery.Append(" TO_CHAR(TS.DETAIL_NO) AS TABA_NO_1");

            selectQuery.Append(" FROM TRN_MI_TENSILE_STRENGTH TS INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = TS.PROCESS_PLAN_CD ");


            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            if (tensileCD != 0)
            {
                selectQuery.Append(" AND TS.MI_TS_CD !='" + tensileCD + "' ");
            }


            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE  IS NULL ");
            selectQuery.Append(" AND TS.UPDATED_DATE IN ( ");
            selectQuery.Append(" SELECT  MAX(UPDATED_DATE) FROM ( ");
            selectQuery.Append(" SELECT  TS.UPDATED_DATE , ");
            selectQuery.Append(" DETAIL_NO AS TABA_NO ");
            selectQuery.Append(" FROM TRN_MI_TENSILE_STRENGTH TS INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = TS.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");

            if (tensileCD != 0)
            {
                selectQuery.Append(" AND TS.MI_TS_CD !='" + tensileCD + "' ");
            }
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" ORDER BY TABA_NO ) group by TABA_NO  ) ");
            selectQuery.Append(" ORDER BY TS.MI_TS_CD ");


            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }
        public List<TensileDetails> GetTensileLoadList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD)
        {
            List<TensileDetails> result = new List<TensileDetails>();

            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();

            //selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(", PP.CHUUBAN AS PP_CHUUBAN ");
            selectQuery.Append(", PP.GYOUBAN AS PP_GYOUBAN ");
            selectQuery.Append(", TS.MI_TS_CD");
            selectQuery.Append(", TS.DETAIL_NO");
            selectQuery.Append(", TO_CHAR(TS.TS_TEST_DATE, 'YYYY/MM/DD HH24:MI') AS TS_TEST_DATE_STRING");
            selectQuery.Append(", EMP.SHORT_NAME AS OPERATOR_NAME");
            selectQuery.Append(", CST.SHORT_NAME  AS PP_CUSTOMER_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST  AS PP_PROD_NAME_CUST");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W, NULL, ");
            selectQuery.Append(" CASE WHEN PP.PROD_DIM_OTHER IS NOT NULL THEN ");
            selectQuery.Append(" PP.PROD_DIM_OTHER  ");
            selectQuery.Append(" END  || ");
            selectQuery.Append(" CASE  ");
            selectQuery.Append(" WHEN PP.CHAMFER IS NOT NULL ");
            selectQuery.Append(" AND PP.PROD_DIM_OTHER IS NOT NULL THEN ");
            selectQuery.Append(" ',C-' || PP.CHAMFER ");
            selectQuery.Append(" END  || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.CHAMFER IS NOT NULL ");
            selectQuery.Append(" AND PP.PROD_DIM_OTHER IS NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.CORNER_RADIUS IS NOT NULL ");
            selectQuery.Append(" AND(PP.CHAMFER IS NOT NULL ");
            selectQuery.Append(" OR PP.PROD_DIM_OTHER IS NOT NULL) THEN ");
            selectQuery.Append(" ',R-' || PP.CORNER_RADIUS ");
            selectQuery.Append(" END || CASE ");
            selectQuery.Append(" WHEN PP.CORNER_RADIUS IS NOT NULL ");
            selectQuery.Append(" AND PP.CHAMFER IS NULL ");
            selectQuery.Append(" AND PP.PROD_DIM_OTHER IS NULL THEN ");
            selectQuery.Append(" 'R-' || PP.CORNER_RADIUS ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", NVL( PP.LENGTH_OTHER,  PP.PROD_LENGTH ) AS LENGTH_OTHER");
            selectQuery.Append(", PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D, PP.MATL_DIM_W, MATL_SHAPE, PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.CHARGE  AS PP_CHARGE");
            selectQuery.Append(",PP.PROD_LENGTH AS PP_PROD_LENGTH");
            //selectQuery.Append(", TS.TS_EXTENDED_LENGTH ");
            //selectQuery.Append(", TS.TS_GAUGE_LENGTH");
            selectQuery.Append(", TS.TS_FURNACE_NO AS TS_FURNACE_NO_STRING");
            //selectQuery.Append(", TS.TS_MAX_LOAD_N");
            //selectQuery.Append(", TS.TS_YIELD_LOAD_N");
            //selectQuery.Append(", TS.TS_ELONGATION");
            //selectQuery.Append(", TS.TS_TEST_IMPACT_1");
            //selectQuery.Append(", TS.TS_TEST_IMPACT_2");
            //selectQuery.Append(", TS.TS_TEST_IMPACT_3");
            selectQuery.Append(", TS.TS_DECISION");
            //selectQuery.Append(", TS.TS_CUT_DIM");


            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_MI_TENSILE_STRENGTH   TS  ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD=TS.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ");
            selectQuery.Append(" ON TS.TS_TEST_OPERATOR_CD=EMP.EMPLOYEE_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD= CST.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD =PP.INTERNAL_NAME_CD");


            selectQuery.Append(" WHERE TS.LOCATION_CD =" + locationCD + "  ");   //AND TS.CH_END_DATE IS NOT NULL
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" AND TS.TS_TEST_DATE IS NOT NULL ");
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
            //else
            //{
            //    query += "ORDER BY PP.CHUUBAN ASC,PP.GYOUBAN ASC,TS.DETAIL_NO ASC";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<TensileDetails>(query);
            return result;
        }
        public TensileDetails GetTensileDetailsByCD(long tensileCD, string chuuban, long gyouban)
        {
            TensileDetails tsdetails = new TensileDetails();
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append("  SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(",  PP.CHUUBAN AS PP_CHUUBAN  ");
            selectQuery.Append(",  PP.GYOUBAN	AS	PP_GYOUBAN  ");
            selectQuery.Append(",  PP.PROD_SPEC_NO	AS	PP_PROD_SPEC_NO  ");
            selectQuery.Append(",  PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(",  TO_CHAR(PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' )	AS	PP_DELIVERY_DATE_STRING  ");
            //selectQuery.Append(", CASE   WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL");
            //selectQuery.Append(" THEN EMP.SHORT_NAME   || ' '|| '(' || LOC.SHORT_NAME || ' ' || DEP.DEPARTMENT_NAME  || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(",  EMP.SHORT_NAME	AS	PP_EMPLOYEE_NAME  ");
            selectQuery.Append(",  REC.SHORT_NAME	AS	PP_RECEIVER_NAME  ");
            selectQuery.Append(",  CST.SHORT_NAME	AS	PP_CUSTOMER_NAME  ");
            selectQuery.Append(",  PP.CHAMFER	AS	PP_CHAMFER  ");
            selectQuery.Append(",  PP.NYUKA_CD AS PP_NYUKA_CD  ");
            selectQuery.Append(",  pp.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");
            selectQuery.Append(",  PP.COLOR_ID	AS	PP_COLOR  ");
            selectQuery.Append(",  PP.MATL_NAME_MAK	AS	PP_MATL_NAME_MAK  ");
            selectQuery.Append(",  PP.MATL_SHAPE	AS	PP_MATL_SHAPE  ");
            selectQuery.Append(",  PP.MATL_DIM_D	AS	PP_MATL_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,null,null) AS PP_MATL_DIM_D_STRING ");
            selectQuery.Append(",  PP.CHARGE	AS	PP_CHARGE  ");
            // selectQuery.Append(",  PP.MAGARI_KUBUN	AS	PP_MAGARI_KUBUN  ");
            selectQuery.Append(",  PP.MAGARI_BEND_TOL	AS	PP_BEND_TOL  ");
            selectQuery.Append(",  PP.MAGARI_SPAN	AS	PP_MAGARI_SPAN  ");
            selectQuery.Append(",  PP.PROD_SHAPE	AS	PP_PROD_SHAPE  ");
            selectQuery.Append(",  PP.PROD_DIM_D	AS	PP_PROD_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,NULL,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ");
            selectQuery.Append(",  PP.PROD_NAME_CUST	AS	PP_PROD_NAME_CUST  ");
            selectQuery.Append(",  PP.PROD_DIM_OTHER	AS	PP_PROD_DIM_OTHER  ");
            selectQuery.Append(",  PP.EXT_TEXTURE	AS	PP_EXT_TEXTURE  ");
            selectQuery.Append(",  PP.DIM_TOL_MIN_CHR  ");
            selectQuery.Append(",  PP.DIM_TOL_MAX_CHR  ");
            selectQuery.Append(", (CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(",  PP.DISPLAY	AS	PP_DISPLAY  ");
            selectQuery.Append(",  PP.STAMP	AS	PP_STAMP  ");
            selectQuery.Append(",  PP.PROD_LENGTH	AS	PP_PROD_LENGTH  ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(",  PP.LENGTH_TOL_MIN_CHR  ");
            selectQuery.Append(",  PP.LENGTH_TOL_MAX_CHR ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(",  PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(",  PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(",  PP.NO_OF_UNITS	AS	PP_NO_OF_UNITS  ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(",  PP.ACT_ALLOC_QTY	AS	PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(",  PP.COIL_DIA_INTERNAL AS	PP_COIL_DIA_INTERNAL  ");
            selectQuery.Append(",  PP.COIL_DIA_EXTERNAL	AS	PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING");
            selectQuery.Append(",  K.REMARKS	AS	PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_REMARK	AS	PP_PROCESS_PLAN_REMARK  ");
            selectQuery.Append(",  PP.STD_DEL_PERIOD || '日' 	AS	PP_STD_DEL_PERIOD_STRING  ");
            selectQuery.Append(",  PP.CREATED_DATE	AS	PP_CREATED_DATE  ");
            selectQuery.Append(",  PP.UPDATED_DATE	AS	PP_UPDATED_DATE  ");
            selectQuery.Append(",  GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD)	AS	PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_CD	AS	PP_PROCESS_PLAN_CD   ");
            selectQuery.Append(",  PP.NYUKA_CD AS PP_NYUKA_CD ");
            selectQuery.Append(", PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC,");
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
            //selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(", TE.MI_TS_CD ");
            selectQuery.Append(", TE.TS_TEST_OPERATOR_CD ");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = TE.TS_TEST_OPERATOR_CD) AS OPERATOR_NAME");
            selectQuery.Append(", TE.DETAIL_NO  ");
            selectQuery.Append(", TO_CHAR( TE.TS_TEST_DATE,'yyyy/mm/dd HH24:MI' ) AS TS_TEST_DATE_STRING ");
            selectQuery.Append(", IS_ACT_DIM_RANGE_CHECK");
            selectQuery.Append(", TE.TS_FURNACE_NO");
            selectQuery.Append(", TE.TS_ACT_DIM");
            selectQuery.Append(", TE.TS_ACT_DIM_SHAPE  ");
            selectQuery.Append(", TE.TS_TEST_SPECIMEN_DIM");
            selectQuery.Append(", TE.TS_MAX_LOAD_N ");
            selectQuery.Append(", TE.TS_DECISION  ");
            selectQuery.Append(", TE.TEST_SPECIMEN_CD");
            selectQuery.Append(", TE.TS_GAUGE_LENGTH");
            selectQuery.Append(", TE.TS_YIELD_LOAD_N");
            selectQuery.Append(", TE.TS_EXTENDED_LENGTH");
            selectQuery.Append(", TE.TS_CUT_DIM");
            selectQuery.Append(", TE.TS_TEST_IMPACT_1");
            selectQuery.Append(", TE.TS_TEST_IMPACT_2");
            selectQuery.Append(", TE.TS_TEST_IMPACT_3");
            selectQuery.Append(", TE.TS_NEXT_PROCESS");

            selectQuery.Append(", M.MSG_CONTENTS AS REMARKS  ");
            selectQuery.Append(", M.MSG_CD   ");
            selectQuery.Append(",(SELECT MAX(TS1.DETAIL_NO) FROM TRN_PROCESS_PLAN PP1 INNER JOIN TRN_MI_TENSILE_STRENGTH TS1 ON PP1.PROCESS_PLAN_CD = TS1.PROCESS_PLAN_CD And PP1.Chuuban= '" + chuuban + "' And PP1.gyouban = " + gyouban + "  AND PP1.PROCESS_PLAN_CMPL_DATE IS NULL ) AS TS_LAST_DETAIL_NO ");

            selectQuery.Append(", TO_CHAR( TE.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CREATED_DATE_STRING");
            selectQuery.Append(", TO_CHAR( TE.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS UPDATED_DATE_STRING");
            selectQuery.Append("  FROM TRN_MI_TENSILE_STRENGTH TE  ");
            selectQuery.Append("  INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD =TE.PROCESS_PLAN_CD ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append("  ON M.MSG_CD = TE.MSG_CD  ");
            //  selectQuery.Append("LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = dg.LOCATION_CD");

            selectQuery.Append(" INNER JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON PP.RECEIVER_CD = REC.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP  ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DG ON PP.DEPARTMENT_GROUP_CD = DG.DEPARTMENT_GROUP_CD ");// TABLE NOT AVAILABLE IN DB Commited by shubham
            // selectQuery.Append("LEFT JOIN MST_DEPARTMENT DEP ON DEP.DEPARTMENT_CD = DG.DEPARTMENT_CD"); 

            //selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");


            selectQuery.Append("  WHERE ");
            selectQuery.Append("  TE.MI_TS_CD =" + tensileCD);
            tsdetails = ExecuteQuerySingleResult<TensileDetails>(selectQuery.ToString());
            return tsdetails;
        }
        public long UpdateTensileDetails(TensileDetails tensileDetails)
        {
            long msgCD = 0;
            
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append("UPDATE TRN_MI_TENSILE_STRENGTH SET ");

                updateQuery.Append(" DETAIL_NO ='" + tensileDetails.DETAIL_NO + "',");
                updateQuery.Append(" MSG_CD ='" + tensileDetails.MSG_CD + "',");
                updateQuery.Append(" TS_TEST_DATE = ");
                updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(tensileDetails.TS_TEST_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");
                updateQuery.Append(" LOCATION_CD = '" + tensileDetails.LOCATION_CD + "',");
                updateQuery.Append(" TS_TEST_OPERATOR_CD ='" + tensileDetails.TS_TEST_OPERATOR_CD + "',");
                updateQuery.Append(" TS_FURNACE_NO ='" + tensileDetails.TS_FURNACE_NO + "',");
                updateQuery.Append(" TEST_SPECIMEN_CD='" + tensileDetails.TEST_SPECIMEN_CD + "',");
                updateQuery.Append(" IS_ACT_DIM_RANGE_CHECK ='" + tensileDetails.IS_ACT_DIM_RANGE_CHECK + "',");
                updateQuery.Append(" TS_ACT_DIM ='" + tensileDetails.TS_ACT_DIM + "',");
                updateQuery.Append(" TS_ACT_DIM_SHAPE='" + tensileDetails.TS_ACT_DIM_SHAPE + "',");
                updateQuery.Append(" TS_TEST_SPECIMEN_DIM ='" + tensileDetails.TS_TEST_SPECIMEN_DIM + "',");

                updateQuery.Append(" TS_GAUGE_LENGTH = '" + tensileDetails.TS_GAUGE_LENGTH + "',");
                updateQuery.Append(" TS_MAX_LOAD_N  = '" + tensileDetails.TS_MAX_LOAD_N + "',");
                updateQuery.Append(" TS_YIELD_LOAD_N= '" + tensileDetails.TS_YIELD_LOAD_N + "',");
                updateQuery.Append(" TS_EXTENDED_LENGTH = '" + tensileDetails.TS_EXTENDED_LENGTH + "',");
                updateQuery.Append(" TS_CUT_DIM  = '" + tensileDetails.TS_CUT_DIM + "',");
                updateQuery.Append(" TS_TEST_IMPACT_1  = '" + tensileDetails.TS_TEST_IMPACT_1 + "',");
                updateQuery.Append(" TS_TEST_IMPACT_2 = '" + tensileDetails.TS_TEST_IMPACT_2 + "',");
                updateQuery.Append(" TS_TEST_IMPACT_3='" + tensileDetails.TS_TEST_IMPACT_3 + "',");
                updateQuery.Append(" TS_TENSILE= '" + tensileDetails.TS_TENSILE + "',");

                updateQuery.Append(" TS_YIELD_STRENGTH  = '" + tensileDetails.TS_YIELD_STRENGTH + "',");
                updateQuery.Append(" TS_ELONGATION  = '" + tensileDetails.TS_ELONGATION + "',");
                updateQuery.Append(" TS_REDUCTION = '" + tensileDetails.TS_REDUCTION + "',");
                updateQuery.Append(" TS_DECISION='" + tensileDetails.TS_DECISION + "',");
                //updateQuery.Append(" TS_NEXT_PROCESS='" + tensileDetails.TS_NEXT_PROCESS + "',"); 
                updateQuery.Append(" USER_ID ='" + tensileDetails.USER_ID + "',");
                updateQuery.Append(" INPUT_LOCATION_CD ='" + (tensileDetails.INPUT_LOCATION_CD != null ? tensileDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "',");
                updateQuery.Append(" UPDATED_DATE = SYSDATE");

                updateQuery.Append(" WHERE ");
                updateQuery.Append(" MI_TS_CD= '" + tensileDetails.MI_TS_CD + "'");
                ExecuteNonQuery(updateQuery.ToString());
                object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_MI_TENSILE_STRENGTH WHERE MI_TS_CD= '" + tensileDetails.MI_TS_CD + "'");
                if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
                {
                    msgCD = Convert.ToInt64(cdVal);
                }
            
            return msgCD;
        }
        public string DeleteTensileDetails(long tensileCD)
        {
            string result = "";
            try
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(" DELETE FROM  TRN_MI_TENSILE_STRENGTH");
                deleteQuery.Append(" WHERE MI_TS_CD =" + tensileCD);
                ExecuteNonQuery(deleteQuery.ToString());
                result = "1"; //Data Deleted Successfully
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    result = "0";//Data not Deleted  
                }
                else
                {
                    throw;
                }

            }
            return result;
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Mech Tensile process data according to search criteria        
        /// </returns>
        /// 
        public List<TensileDetails> GetMECHTensileOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCriteria, int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<TensileDetails> result = new List<TensileDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS INSPECTION_LOCATION_NAME, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" TO_CHAR(TENSILE.TS_TEST_DATE, 'YYYY/MM/DD HH24:MI') AS TS_TEST_DATE_STRING, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
            selectQuery.Append(" TENSILE.DETAIL_NO AS DETAIL_NO,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,'' ) AS PP_MATL_DIM_D_STRING,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_SHAPE,'' ) AS PP_PROD_DIM_D_STRING,");
            selectQuery.Append(" PP.PROD_DIM_D AS PROD_DIM_D, ");
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" TENSILE.TS_FURNACE_NO AS TS_FURNACE_NO, ");
            selectQuery.Append(" TENSILE.TS_MAX_LOAD_N AS TS_MAX_LOAD_N, ");
            selectQuery.Append(" TENSILE.TS_YIELD_LOAD_N AS TS_YIELD_LOAD_N, ");
            selectQuery.Append(" TENSILE.TS_ACT_DIM AS TS_ACT_DIM, ");
            selectQuery.Append(" TENSILE.TS_GAUGE_LENGTH AS TS_GAUGE_LENGTH, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS REMARKS, ");
            selectQuery.Append(" TENSILE.TS_TENSILE AS TS_TENSILE, ");
            selectQuery.Append(" TENSILE.TS_ELONGATION AS TS_ELONGATION, ");
            selectQuery.Append(" TENSILE.TS_DECISION AS TS_DECISION, ");
            selectQuery.Append(" TENSILE.TS_REDUCTION AS TS_REDUCTION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" TENSILE.TS_TEST_SPECIMEN_DIM, ");
            selectQuery.Append(" TENSILE.TS_EXTENDED_LENGTH ,");
            selectQuery.Append(" TENSILE.TS_CUT_DIM, ");
            selectQuery.Append(" TENSILE.TS_TEST_SPECIMEN_DIM, ");
            selectQuery.Append(" TENSILE.TS_YIELD_STRENGTH AS TS_YIELD_STRENGTH, ");
            selectQuery.Append(" CONCAT(TS_TEST_IMPACT_1, CONCAT( CASE  WHEN TS_TEST_IMPACT_2 IS NOT NULL THEN  CONCAT(',', TS_TEST_IMPACT_2) ELSE NULL  END, CASE WHEN TS_TEST_IMPACT_3 IS NOT NULL THEN CONCAT (',', TS_TEST_IMPACT_3) ELSE NULL END) )AS TS_TEST_IMPACT_1_2_3,");
            selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MI_TENSILE_STRENGTH TENSILE ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON TENSILE.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = TENSILE.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = TENSILE.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = TENSILE.TS_TEST_OPERATOR_CD  ");

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

            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }
            //else
            //{
            //    query += " ORDER BY TS_TEST_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<TensileDetails>(query);
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Mech Tensile process data according to search criteria        
        /// </returns>
        /// 
        public List<TensileDetails> GetMECHTensileOnDailyProcessStatus(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridsearchresult)
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
            //selectQuery.Append(" EMP.EMPLOYEE_NAME AS DEPARTMENT_NAME,");
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME,");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
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
            selectQuery.Append(" PP.MAGARI_KUBUN AS MAGARI_KUBUN,");
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
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1,");
            selectQuery.Append(" PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX,");
            selectQuery.Append(" PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_CODE,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX,");
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
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS,");
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
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION,");
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
            selectQuery.Append(" MITENS.DETAIL_NO,");
            selectQuery.Append(" TO_CHAR(MITENS.TS_TEST_DATE, 'yyyy/mm/dd HH24:MI') AS TS_TEST_DATE_STRING,");
            selectQuery.Append(" EMP1.SHORT_NAME AS OPERATOR_NAME,");
            selectQuery.Append(" MITENS.TS_FURNACE_NO,");
            //selectQuery.Append(" MITENS.TEST_SPECIMEN_CD,");
            selectQuery.Append(" MJS.JIS_TEST_SPEC_SHORT,");
            selectQuery.Append(" MITENS.TS_ACT_DIM,");
            selectQuery.Append(" MITENS.TS_ACT_DIM_SHAPE AS TS_ACT_DIM_SHAPE,");
            selectQuery.Append(" MITENS.TS_TEST_SPECIMEN_DIM,");
            selectQuery.Append(" MITENS.TS_GAUGE_LENGTH,");
            selectQuery.Append(" MITENS.TS_MAX_LOAD_N,");
            selectQuery.Append(" MITENS.TS_YIELD_LOAD_N,");
            selectQuery.Append(" MITENS.TS_EXTENDED_LENGTH,");
            selectQuery.Append(" MITENS.TS_CUT_DIM,");
            selectQuery.Append(" MITENS.TS_TEST_IMPACT_1,");
            selectQuery.Append(" MITENS.TS_TEST_IMPACT_2,");
            selectQuery.Append(" MITENS.TS_TEST_IMPACT_3,");
            selectQuery.Append(" MITENS.TS_TENSILE,");
            selectQuery.Append(" MITENS.TS_YIELD_STRENGTH AS TS_YIELD_STRENGTH_SHORT,");
            selectQuery.Append(" MITENS.TS_ELONGATION,");
            selectQuery.Append(" MITENS.TS_REDUCTION,");
            selectQuery.Append(" MITENS.TS_DECISION, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS REMARKS ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MI_TENSILE_STRENGTH   MITENS INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = MITENS.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=MITENS.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = MITENS.TS_TEST_OPERATOR_CD ");
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
            selectQuery.Append(" LEFT JOIN MST_JIS_TEST_SPEC MJS ON MJS.JIS_TEST_SPEC_CD = MITENS.TEST_SPECIMEN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = MITENS.MSG_CD ");

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
            return ExecuteQueryMultiResult<TensileDetails>(query);
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
        public int GetRowCountForMechTensileFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MI_TENSILE_STRENGTH TENSILE ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON TENSILE.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = TENSILE.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = TENSILE.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = TENSILE.TS_TEST_OPERATOR_CD  ");

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
        public int GetRowCountForMechTensileDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MI_TENSILE_STRENGTH   MITENS INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = MITENS.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=MITENS.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = MITENS.TS_TEST_OPERATOR_CD ");
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
            selectQuery.Append(" LEFT JOIN MST_JIS_TEST_SPEC MJS ON MJS.JIS_TEST_SPEC_CD = MITENS.TEST_SPECIMEN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = MITENS.MSG_CD ");

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

        public decimal GetJISTestMagnification(int testSpecCd)
        {
            decimal testSpecCdMagnification;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MAGNIFICATION ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_JIS_TEST_SPEC ");
            selectQuery.Append(" WHERE JIS_TEST_SPEC_CD = '" + testSpecCd + "'   ");

            var query = selectQuery.ToString();
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            testSpecCdMagnification = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
            return testSpecCdMagnification;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2026/03/16     
        /// used screens: MIHardness Entry Screen
        /// </summary>
        /// <param name="chuuban"></param>
        /// <param name="gyouban"></param>
        /// <returns>
        /// List  of Combined Decisions of final inspection
        /// </returns>
        public List<TensileDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COMBINED_DECISION,FI_SPARK_DECISION ");
            selectQuery.Append(" FROM ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FI.COMBINED_DECISION,");
            selectQuery.Append(" FI.FI_SPARK_DECISION,");
            selectQuery.Append(" ROW_NUMBER() OVER ( PARTITION BY FI.NIFUDA_CD, FI.FI_TABA_NO ORDER BY FI.UPDATED_DATE DESC ) AS ROWNO ");
            selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI  ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" ) WHERE ROWNO = 1  ");
            return ExecuteQueryMultiResult<TensileDetails>(selectQuery.ToString());
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2026/03/16     
        /// used screens: MIHardness Entry Screen
        /// </summary>
        /// <param name="tensileDecision"></param>
        /// <param name="processPlanCd"></param>
        /// <returns>
        /// update the combined decision into final inspection table
        /// </returns>
        public void UpdateFICombinedDecision(short tensileDecision, long processPlanCd)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE TRN_FINAL_INSPECTION SET");
            updateQuery.Append(" COMBINED_DECISION ='" + tensileDecision + "'");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" PROCESS_PLAN_CD ='" + processPlanCd + "'");
            ExecuteNonQuery(updateQuery.ToString());
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2026/09/10     
        /// used screens: MITensile EditDelete Screen
        /// </summary>
        /// <param name="MIHardnessCD"></param>
        /// <returns>
        /// 0 - This Tensile Details is already used in Mill Sheet. So, you can not update this Tensile Details.
        /// 1 - This Tensile Details is already used in Mill Sheet. So, you can update this Tensile Details.
        /// </returns>
        public string ValidateTensileDetailsOnUpdateBtn(long MITensileCD)
        {
            int count = 0;
            string result = string.Empty;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM HSKUSER.HSK_MILL_SHEET ");
            selectQuery.Append(" WHERE TENSILE_CD1 ='" + MITensileCD + "' OR TENSILE_CD2='" + MITensileCD + "' OR TENSILE_CD3='" + MITensileCD + "' OR TENSILE_CD4='" + MITensileCD + "' OR TENSILE_CD5='" + MITensileCD + "' OR TENSILE_CD6='" + MITensileCD + "' OR TENSILE_CD7='" + MITensileCD + "'");
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                result = "0";//This Tensile Details is already used in Mill Sheet. So, you can not update this Tensile Details.
            }
            else
            {
                result = "1";//This Tensile Details is already used in Mill Sheet. So, you can update this Tensile Details.
            }
            return result;
        }
    }
}
