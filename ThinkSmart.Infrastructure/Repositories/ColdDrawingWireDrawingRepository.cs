using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
    public class ColdDrawingWireDrawingRepository : BaseRepository, IColdDrawingWireDrawingRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public ColdDrawingWireDrawingRepository(IConfiguration _configuration,LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }

        public ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByDrawingCD(long drawingCD)
        {
            ColdDrawingWireDrawingDetails ColdDrawingWireDrawingDetails = new ColdDrawingWireDrawingDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(",DR.PROCESS_PLAN_CD	AS	PROCESS_PLAN_CD  ");
            selectQuery.Append(",(SELECT SHORT_NAME FROM MST_MACHINE WHERE MACHINE_CD = DR.MACHINE_CD) AS DW_MACHINE_NAME ");
            selectQuery.Append(",DR.MACHINE_CD	AS	MACHINE_CD  ");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD  AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.UNIT AS PP_UNIT");
            //selectQuery.Append(" ,K.REMARKS AS  PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");           
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX ");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
           // selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            // selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.USER_ID AS USER_ID ");
            selectQuery.Append(", PP.COIL_DIA_WINDING AS COIL_DIA_WINDING");
            selectQuery.Append(" ,DR.ROLLING_NUMBER AS DW_ROLLING_NUMBER  ");
            selectQuery.Append(" ,DR.FURNACE_NO AS DW_FURNACE_NO  ");
            selectQuery.Append(" ,DR.DIE_DIM AS DW_DIE_DIM  ");
            selectQuery.Append(" ,DR.DIE_SHAPE AS DW_DIE_SHAPE  ");
            selectQuery.Append(" ,DR.DIE_NO AS DW_DIE_NO  ");
            selectQuery.Append(" ,DR.DIE_CHK_START AS DW_DIE_CHK_START  ");
            selectQuery.Append(" ,DR.WINDING_SHAPE AS DW_WINDING_SHAPE  ");
            selectQuery.Append(" ,DR.IM_DRAWING AS DW_IM_DRAWING  ");
            selectQuery.Append(" ,DR.REDRAWING AS DW_REDRAWING  ");
            selectQuery.Append(" ,DR.DW_START_DATE AS DW_START_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DR.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(" ,DR.DW_START_OPERATOR_CD AS DW_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME  ");

            //endDataPanel
            selectQuery.Append(" ,DR.DW_END_DATE AS DW_END_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DR.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(" ,DR.DW_END_OPERATOR_CD AS DW_END_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_END_OPERATOR_CD) AS DW_END_OPERATOR_NAME  ");
            selectQuery.Append(" ,DR.DW_DECISION AS DW_DECISION ");
            selectQuery.Append(" ,DR.DW_NEXT_PROCESS AS DW_DW_NEXT_PROCESS ");
            selectQuery.Append(" ,DR.DW_ACT_DIM_CHK AS DW_DW_ACT_DIM_CHK ");
            selectQuery.Append(" ,DR.DW_ACT_DIM_START AS DW_ACT_DIM_START ");
            selectQuery.Append(" ,DR.DW_ACT_DIM_END AS DW_ACT_DIM_END ");
            selectQuery.Append(" ,DR.DW_FINISHED_COILS AS DW_FINISHED_COILS ");
            selectQuery.Append(" ,DR.DW_FINISHED_QTY AS DW_FINISHED_QTY ");
            selectQuery.Append(" ,DR.DW_EXT_TEXTURE_STATUS AS DW_EXT_TEXTURE_STATUS ");
            selectQuery.Append(" ,DR.DIE_CHK_END AS DW_DIE_CHK_END ");
            selectQuery.Append(" ,DR.DW_MAGNETIC_INSPECTIONS AS DW_MAGNETIC_INSPECTIONS ");
            selectQuery.Append(" ,DR.DW_TABA_NO_1 AS DW_DW_TABA_NO_1 ");
            selectQuery.Append(" ,DR.DW_TABA_NO_2 AS DW_DW_TABA_NO_2 ");
            selectQuery.Append(" ,DR.DW_TABA_NO_3 AS DW_DW_TABA_NO_3 ");
            selectQuery.Append(" ,DR.DW_TABA_NO_4 AS DW_DW_TABA_NO_4 ");
            selectQuery.Append(" ,DR.MSG_CD AS DW_MSG_CD ");
            selectQuery.Append(" ,M.MSG_CONTENTS AS DW_REMARKS ");
            selectQuery.Append(" ,DR.DRAWING_CD AS DRAWING_CD ");
            selectQuery.Append(",PP.NYUKA_CD AS PRODUCT_NYUKO_CD ");


            selectQuery.Append(" ,P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(" ,P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(" ,P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(" ,P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(" ,P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(" ,P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(" ,P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(" ,P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(" ,P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(" ,P10.PROCESS_NAME AS PP_PROCESS_10 ");
            selectQuery.Append(" ,P11.PROCESS_NAME AS PP_PROCESS_11 ");
            selectQuery.Append(" ,P12.PROCESS_NAME AS PP_PROCESS_12 ");
            selectQuery.Append(" ,P13.PROCESS_NAME AS PP_PROCESS_13, ");

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
            selectQuery.Append("CON.CONDITION_DESCRIPTION,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN1,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append("PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS,");
            selectQuery.Append("PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC, ");
            selectQuery.Append("DR.DW_START_DATE AS DW_START_DATE, ");
            selectQuery.Append("DR.DW_END_DATE AS DW_END_DATE ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_W DR ON");
            selectQuery.Append(" DR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");

            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K  ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR ON DGPGR.DEPARTMENT_GROUP_CD = pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP ON DEP.DEPARTMENT_CD = DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DGPGR.LOCATION_CD");
            //selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
           // selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON DR.MSG_CD = M.MSG_CD");
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

            selectQuery.Append("  WHERE DR.DRAWING_CD=" + drawingCD);
            ColdDrawingWireDrawingDetails = ExecuteQuerySingleResult<ColdDrawingWireDrawingDetails>(selectQuery.ToString());
            return ColdDrawingWireDrawingDetails;
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

        public List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawing24MachineList(string machineCd)
        {
            List<ColdDrawingWireDrawingDetails> result = new List<ColdDrawingWireDrawingDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" DR.MACHINE_CD AS MACHINE_CD,  ");

            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");


            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");

            selectQuery.Append(" PP.PROD_DIM_D  AS PP_PROD_DIM_D,	");
            // selectQuery.Append(" PP.PROD_DIM_D || '' || PP.PROD_SHAPE  AS PP_PROD_DIM_D_STRING,	");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_SHAPE,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING,");
           // selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,	");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LEN_TOL, ");
            selectQuery.Append(" (CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE, ");

            selectQuery.Append(" DR.DW_DECISION AS  DW_DECISION, ");
            selectQuery.Append(" DR.DIE_DIM AS  PP_MATL_DIM_D_SHAPE, ");
            selectQuery.Append(" DR.DIE_SHAPE AS  DW_DIE_SHAPE, ");
            selectQuery.Append(" DR.WINDING_SHAPE AS DW_WINDING_SHAPE,	");
            selectQuery.Append(" DR.ROLLING_NUMBER AS DW_ROLLING_NUMBER,	");
            selectQuery.Append(" DR.DIE_NO AS DW_DIE_NO, ");
            selectQuery.Append(" DR.FURNACE_NO AS DW_FURNACE_NO, ");

            selectQuery.Append(" DR.DW_EXT_TEXTURE_STATUS AS DW_EXT_TEXTURE_STATUS,");
            selectQuery.Append(" DR.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");
            selectQuery.Append(" DR.DW_MAGNETIC_INSPECTIONS AS DW_MAGNETIC_INSPECTIONS, ");
            selectQuery.Append(" to_char(DR.DW_ACT_DIM_START,'FM990.000') AS DW_ACT_DIM_START_STRING ,");
            selectQuery.Append(" to_char(DR.DW_ACT_DIM_END,'FM990.000') AS DW_ACT_DIM_END_STRING ,");
           
            selectQuery.Append(" CONCAT (DR.DW_TABA_NO_1, CASE WHEN DR.DW_TABA_NO_2 IS NOT NULL THEN CONCAT ('-', DR.DW_TABA_NO_2) ELSE NULL END) AS DW_DW_TABA_NO_1_STRING ,");


            selectQuery.Append(" CONCAT ( CASE WHEN DR.DW_TABA_NO_3 IS NOT NULL THEN  CONCAT ('-',DR.DW_TABA_NO_3) ELSE NULL END,");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_4 IS NOT NULL THEN CONCAT('-', DR.DW_TABA_NO_4) ELSE NULL END) AS DW_DW_TABA_NO_2_STRING , ");


            selectQuery.Append("  M.MSG_CONTENTS AS DW_REMARKS, ");

            selectQuery.Append("  PP.CHARGE AS PP_CHARGE,	");

            selectQuery.Append("  NVL (DR.DW_END_DATE, DR.DW_START_DATE) AS ORDER_CLAUSE ");

            selectQuery.Append(" FROM TRN_DRAWING_W DR");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON DR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ");
            selectQuery.Append(" ON M.MSG_CD = DR.MSG_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" WHERE  DR.MACHINE_CD  = '" + machineCd + "' AND NVL (DR.DW_END_DATE, DR.DW_START_DATE) > SYSDATE - 1");           
            selectQuery.Append(" ORDER BY ORDER_CLAUSE DESC");
            result = ExecuteQueryMultiResult<ColdDrawingWireDrawingDetails>(selectQuery.ToString());
            return result;
        }

        public ColdDrawingWireDrawingDetails GetColdDrawingWireDrawingDetailsByChuuban(string chuuban, short? gyouban, int? machineCD)
        {
            ColdDrawingWireDrawingDetails ColdDrawingWireDrawingDetails = new ColdDrawingWireDrawingDetails();
            StringBuilder selectQuery = new StringBuilder();
            StringBuilder selectCountQuery = new StringBuilder();
            int count = 0;

            string whereCondition1 = " PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL";
            string whereCondition2 = "  AND DR.MACHINE_CD = '" + machineCD + "' AND DR.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + "";
            string finalWhere = "";
            finalWhere = whereCondition1 + whereCondition2;

            //SQL
            selectCountQuery.Append(" SELECT COUNT(*) ");
            selectCountQuery.Append(" FROM ");
            selectCountQuery.Append(" TRN_PROCESS_PLAN  PP ");
            selectCountQuery.Append(" INNER JOIN TRN_DRAWING_W DR ON DR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
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
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD  AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING AS COIL_DIA_WINDING");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.UNIT AS PP_UNIT");
            // selectQuery.Append(" ,K.REMARKS AS  PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(", PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX ");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            //selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            //selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.USER_ID AS USER_ID ");
            selectQuery.Append(" ,TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE, 'yyyy/MM/dd') AS PP_PROCESS_PLAN_CMPL_DATE ");

            selectQuery.Append(" ,DR.ROLLING_NUMBER AS DW_ROLLING_NUMBER  ");
            selectQuery.Append(" ,DR.FURNACE_NO AS DW_FURNACE_NO  ");
            selectQuery.Append(" ,DR.DIE_DIM AS DW_DIE_DIM  ");
            selectQuery.Append(" ,DR.DIE_SHAPE AS DW_DIE_SHAPE  ");
            selectQuery.Append(" ,DR.DIE_NO AS DW_DIE_NO  ");
            selectQuery.Append(" ,DR.DIE_CHK_START AS DW_DIE_CHK_START  ");
            selectQuery.Append(" ,DR.WINDING_SHAPE AS DW_WINDING_SHAPE  ");
            selectQuery.Append(" ,DR.IM_DRAWING AS DW_IM_DRAWING  ");
            selectQuery.Append(" ,DR.REDRAWING AS DW_REDRAWING  ");
            selectQuery.Append(" ,DR.DW_START_DATE AS DW_START_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DR.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(" ,DR.DW_START_OPERATOR_CD AS DW_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME  ");
            selectQuery.Append(" ,DR.DW_END_DATE AS DW_END_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DR.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(" ,DR.DRAWING_CD AS DRAWING_CD ");
            selectQuery.Append(" ,DR.DW_TABA_NO_1 ");
            selectQuery.Append(" ,DR.DW_TABA_NO_2 ");
            selectQuery.Append(" ,DR.DW_TABA_NO_3 ");
            selectQuery.Append(" ,DR.DW_TABA_NO_4 ");
            selectQuery.Append(",PP.NYUKA_CD AS PRODUCT_NYUKO_CD ");


            selectQuery.Append(" ,P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(" ,P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(" ,P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(" ,P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(" ,P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(" ,P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(" ,P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(" ,P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(" ,P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(" ,P10.PROCESS_NAME AS PP_PROCESS_10 ");
            selectQuery.Append(" ,P11.PROCESS_NAME AS PP_PROCESS_11 ");
            selectQuery.Append(" ,P12.PROCESS_NAME AS PP_PROCESS_12 ");
            selectQuery.Append(" ,P13.PROCESS_NAME AS PP_PROCESS_13, ");

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
            selectQuery.Append(" CON.CONDITION_DESCRIPTION, ");
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
            selectQuery.Append("PP.PROCESS_PLAN_START_DATE ");
            selectQuery.Append(",CASE ");
            selectQuery.Append(" WHEN DR.CREATED_DATE >= TO_DATE(SYSDATE) -" + days + " THEN ");
            selectQuery.Append(" 1");
            selectQuery.Append(" ELSE");
            selectQuery.Append(" 0");
            selectQuery.Append(" END AS SHOW_DEFAULT , ");
            if (count == 0)
            {
                selectQuery.Append("'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append("'YES' AS PAST_DATA_EXIST ");
            }

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_W DR ON");
            selectQuery.Append(" DR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" AND DR.MACHINE_CD= '" + machineCD + "'");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");
           // selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K  ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR ON DGPGR.DEPARTMENT_GROUP_CD = pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP ON DEP.DEPARTMENT_CD = DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON PP.LOCATION_CD = LOC.LOCATION_CD");
            //selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");
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

            /* selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
             selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
             selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
             selectQuery.Append(" AND DR.MACHINE_CD= '" + machineCD + "'");
             selectQuery.Append(" AND DR.CREATED_DATE >= TO_DATE(SYSDATE)-" + days + " ");*/

            selectQuery.Append(" WHERE " + finalWhere);
            selectQuery.Append(" ORDER BY DR.DRAWING_CD DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY ");
            ColdDrawingWireDrawingDetails = ExecuteQuerySingleResult<ColdDrawingWireDrawingDetails>(selectQuery.ToString());
            return ColdDrawingWireDrawingDetails;
        }

        public void InsertColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails ColdDrawingWireDrawingDetails)
        {            
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_DRAWING_W(");
            insertQuery.Append(" DRAWING_CD,");
            insertQuery.Append(" PROCESS_PLAN_CD,");
            insertQuery.Append(" MACHINE_CD,");
            insertQuery.Append(" LOCATION_CD,");
            insertQuery.Append(" DW_START_DATE,");
            insertQuery.Append(" DW_START_OPERATOR_CD,");

            insertQuery.Append(" ROLLING_NUMBER,");
            insertQuery.Append(" FURNACE_NO,");
            insertQuery.Append(" DIE_DIM,");
            insertQuery.Append(" DIE_SHAPE,");
            insertQuery.Append(" DIE_NO,");
            insertQuery.Append(" DIE_CHK_START,");
            insertQuery.Append(" WINDING_SHAPE,");
            insertQuery.Append(" IM_DRAWING,");
            insertQuery.Append(" REDRAWING,");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");
            insertQuery.Append(" PROCESS_KUBUN_CD, ");
            insertQuery.Append(" CREATED_DATE,");
            insertQuery.Append(" UPDATED_DATE,");
            insertQuery.Append(" DW_TABA_NO_1,");
            insertQuery.Append(" DW_TABA_NO_2,");
            insertQuery.Append(" DW_TABA_NO_3,");
            insertQuery.Append(" DW_TABA_NO_4");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("TRN_DRAWING_W_SEQ.nextval");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.LOCATION_CD + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(ColdDrawingWireDrawingDetails.DW_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DW_START_OPERATOR_CD + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.ROLLING_NUMBER + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.FURNACE_NO + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DIE_DIM + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DIE_SHAPE + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DIE_NO + "'");

            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DIE_CHK_START + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.WINDING_SHAPE + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.IM_DRAWING + "'");

            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.REDRAWING + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.USER_ID + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DW_TABA_NO_1 + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DW_TABA_NO_2 + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DW_TABA_NO_3 + "'");
            insertQuery.Append(",'" + ColdDrawingWireDrawingDetails.DW_TABA_NO_4 + "'");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            //result = GetColdDrawingWireDrawingDetailsByChuuban(ColdDrawingWireDrawingDetails.PP_CHUUBAN, ColdDrawingWireDrawingDetails.PP_GYOUBAN.ToString(), ColdDrawingWireDrawingDetails.MACHINE_CD.ToString());
            //return result;
        }

        public List<ColdDrawingWireDrawingDetails> GetColdDrawingWireDrawingList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD)
        {
            List<ColdDrawingWireDrawingDetails> result = new List<ColdDrawingWireDrawingDetails>();

            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, trim(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            //  selectQuery.Append(",DR.DW_TABA_NO_1 AS DW_DW_TABA_NO_1");
            selectQuery.Append(",CONCAT(DR.DW_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE  WHEN DR.DW_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', DR.DW_TABA_NO_2) ");
            selectQuery.Append(" ELSE   NULL ");
            selectQuery.Append(" END, CASE ");
            selectQuery.Append(" WHEN DR.DW_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', DR.DW_TABA_NO_3) ");
            selectQuery.Append(" ELSE NULL  END   ),");
            selectQuery.Append(" CASE  WHEN DR.DW_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', DR.DW_TABA_NO_4) ");
            selectQuery.Append(" ELSE   NULL  END ");
            selectQuery.Append(" )) AS ALL_TABA_NO ");

            selectQuery.Append(",MS.SHORT_NAME AS DW_MACHINE_NAME");
            selectQuery.Append(",(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_END_OPERATOR_CD) AS DW_END_OPERATOR_NAME");
            selectQuery.Append(",CST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            //  selectQuery.Append(",PP.PROD_DIM_D AS PP_PROD_DIM_D");
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
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D, PP.MATL_DIM_W, MATL_SHAPE, PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING");
            //selectQuery.Append(",PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(",PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(",DR.DW_FINISHED_COILS AS DW_FINISHED_COILS");
            selectQuery.Append(",DR.UNIT_TYPE");
            selectQuery.Append(",DR.DW_FINISHED_QTY AS DW_FINISHED_QTY");
            selectQuery.Append(",TO_CHAR(DR.DW_END_DATE,'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING");
            selectQuery.Append(",DR.DW_DECISION AS DW_DECISION");
            selectQuery.Append(",DR.DRAWING_CD AS DRAWING_CD");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_DRAWING_W DR  ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MS ON DR.MACHINE_CD=MS.MACHINE_CD   ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD  ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" WHERE DR.INPUT_LOCATION_CD =" + locationCD + " AND DR.DW_END_DATE IS NOT NULL ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL "); // added on dated 27/04/2023 as per doc
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
                query += "ORDER BY PP_CHUUBAN_GYOUBAN, DR.DW_TABA_NO_1";
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<ColdDrawingWireDrawingDetails>(query);
            return result;
        }
        public List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban)
        {
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN  ");
            selectQuery.Append(" ,PP.GYOUBAN ");
            selectQuery.Append(" ,DR.DW_DECISION AS DECISION");
            selectQuery.Append(" ,DR.DRAWING_CD");
            selectQuery.Append(" ,DR.CREATED_DATE ");
            selectQuery.Append(" ,CONCAT (DR.DW_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");

            selectQuery.Append(" ,DR.DW_DECISION AS PROCESS_STATUS ");
            selectQuery.Append(" FROM TRN_DRAWING_W DR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");
            selectQuery.Append(" AND DR.IM_DRAWING = 0 ");
            selectQuery.Append(" AND DR.UPDATED_DATE IN ( ");
            selectQuery.Append(" SELECT  MAX(DR.UPDATED_DATE) FROM ( ");
            selectQuery.Append(" SELECT  DR.UPDATED_DATE , DR.DW_TABA_NO_1 AS TABA_NO_INT ");

            selectQuery.Append(" FROM TRN_DRAWING_W DR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");

            selectQuery.Append(" ORDER BY DW_TABA_NO_1 ) GROUP BY DW_TABA_NO_1  ) ");
            selectQuery.Append(" ORDER BY DR.DRAWING_CD ");
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }

        public void DeleteColdDrawingWireDrawingDetails(long drawingCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_DRAWING_W ");
            deleteQuery.Append(" WHERE DRAWING_CD =" + drawingCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public long UpdateColdDrawingWireDrawingDetails(ColdDrawingWireDrawingDetails ColdDrawingWireDrawingDetails)
        {

            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_DRAWING_W SET ");

            //  updateQuery.Append(" LOCATION_CD ='" + ColdDrawingWireDrawingDetails.LOCATION_CD + "'");
            updateQuery.Append(" DW_START_DATE = TO_DATE('" + (Convert.ToDateTime(ColdDrawingWireDrawingDetails.DW_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(" ,DW_START_OPERATOR_CD ='" + ColdDrawingWireDrawingDetails.DW_START_OPERATOR_CD + "'");

            updateQuery.Append(" ,ROLLING_NUMBER ='" + ColdDrawingWireDrawingDetails.ROLLING_NUMBER + "'");
            updateQuery.Append(" ,FURNACE_NO ='" + ColdDrawingWireDrawingDetails.FURNACE_NO + "'");
            updateQuery.Append(" ,DIE_DIM ='" + ColdDrawingWireDrawingDetails.DIE_DIM + "'");
            updateQuery.Append(" ,DIE_SHAPE ='" + ColdDrawingWireDrawingDetails.DIE_SHAPE + "'");
            updateQuery.Append(" ,DIE_NO ='" + ColdDrawingWireDrawingDetails.DIE_NO + "'");
            updateQuery.Append(" ,DIE_CHK_START ='" + ColdDrawingWireDrawingDetails.DIE_CHK_START + "'");
            updateQuery.Append(" ,WINDING_SHAPE ='" + ColdDrawingWireDrawingDetails.WINDING_SHAPE + "'");
            updateQuery.Append(" ,IM_DRAWING ='" + ColdDrawingWireDrawingDetails.IM_DRAWING + "'");
            updateQuery.Append(" ,REDRAWING ='" + ColdDrawingWireDrawingDetails.REDRAWING + "'");


            updateQuery.Append(" ,MACHINE_CD ='" + ColdDrawingWireDrawingDetails.MACHINE_CD + "'");
            updateQuery.Append(", MSG_CD ='" + ColdDrawingWireDrawingDetails.MSG_CD + "'");
            updateQuery.Append(", DW_END_DATE = TO_DATE('" + (Convert.ToDateTime(ColdDrawingWireDrawingDetails.DW_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(", DW_END_OPERATOR_CD ='" + ColdDrawingWireDrawingDetails.DW_END_OPERATOR_CD + "'");
            updateQuery.Append(", DW_ACT_DIM_CHK ='" + ColdDrawingWireDrawingDetails.DW_ACT_DIM_CHK + "'");
            updateQuery.Append(", DW_ACT_DIM_START ='" + ColdDrawingWireDrawingDetails.DW_ACT_DIM_START + "'");
            updateQuery.Append(", DW_ACT_DIM_END ='" + ColdDrawingWireDrawingDetails.DW_ACT_DIM_END + "'");
            updateQuery.Append(", DW_FINISHED_COILS ='" + ColdDrawingWireDrawingDetails.DW_FINISHED_COILS + "'");
            //updateQuery.Append(", UNIT_TYPE ='" + ColdDrawingWireDrawingDetails.UNIT_TYPE + "'");
            updateQuery.Append(", DW_FINISHED_QTY ='" + ColdDrawingWireDrawingDetails.DW_FINISHED_QTY + "'");
            updateQuery.Append(", DW_TABA_NO_1 ='" + ColdDrawingWireDrawingDetails.DW_TABA_NO_1 + "'");
            updateQuery.Append(", DW_TABA_NO_2 ='" + ColdDrawingWireDrawingDetails.DW_TABA_NO_2 + "'");
            updateQuery.Append(", DW_TABA_NO_3 ='" + ColdDrawingWireDrawingDetails.DW_TABA_NO_3 + "'");
            updateQuery.Append(", DW_TABA_NO_4 ='" + ColdDrawingWireDrawingDetails.DW_TABA_NO_4 + "'");
            updateQuery.Append(", DW_EXT_TEXTURE_STATUS ='" + ColdDrawingWireDrawingDetails.DW_EXT_TEXTURE_STATUS + "'");
            updateQuery.Append(", DW_MAGNETIC_INSPECTIONS ='" + ColdDrawingWireDrawingDetails.DW_MAGNETIC_INSPECTIONS + "'");
            updateQuery.Append(", DIE_CHK_END ='" + ColdDrawingWireDrawingDetails.DIE_CHK_END + "'");
            updateQuery.Append(", DW_DECISION ='" + ColdDrawingWireDrawingDetails.DW_DECISION + "'");
            updateQuery.Append(", DW_NEXT_PROCESS ='" + ColdDrawingWireDrawingDetails.DW_NEXT_PROCESS + "'");
            updateQuery.Append(", PROCESS_KUBUN_CD ='" + ColdDrawingWireDrawingDetails.PROCESS_KUBUN_CD + "'");
            updateQuery.Append(", UNIT_TYPE ='4'");


            updateQuery.Append(", UPDATED_DATE = SYSDATE ");
            updateQuery.Append(", USER_ID ='" + ColdDrawingWireDrawingDetails.USER_ID + "'");
            updateQuery.Append(", INPUT_LOCATION_CD ='" + (ColdDrawingWireDrawingDetails.INPUT_LOCATION_CD != 0 ? ColdDrawingWireDrawingDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "'");
            updateQuery.Append(", LOCATION_CD ='" + ColdDrawingWireDrawingDetails.LOCATION_CD + "'");
            updateQuery.Append(" WHERE DRAWING_CD = ");
            updateQuery.Append(ColdDrawingWireDrawingDetails.DRAWING_CD);
            ExecuteNonQuery(updateQuery.ToString());
            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_DRAWING_W WHERE DRAWING_CD= '" + ColdDrawingWireDrawingDetails.DRAWING_CD + "'");
            long msgCd = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCd = Convert.ToInt64(cdVal);
            }
            return msgCd;
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
        public List<ColdDrawingWireDrawingDetails> GetColdDrawingList(int locationCd)
        {
            List<ColdDrawingWireDrawingDetails> result = new List<ColdDrawingWireDrawingDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, trim(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS PP_CHUUBAN_GYOUBAN,  ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append(" DR.DW_TABA_NO_1 AS DW_DW_TABA_NO_1, ");
            selectQuery.Append(" MS.SHORT_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" (SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME, ");
            selectQuery.Append(" CST.SHORT_NAME AS PP_CUSTOMER_NAME, ");

            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" DR.FURNACE_NO AS DW_FURNACE_NO, ");
            selectQuery.Append(" DR.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");
            selectQuery.Append(" DR.DW_START_DATE AS DW_START_DATE, ");
            selectQuery.Append(" DR.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" DR.DRAWING_CD AS DRAWING_CD ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP INNER JOIN TRN_DRAWING_W DR  ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MS ON DR.MACHINE_CD=MS.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD  ");
            selectQuery.Append(" WHERE DR.LOCATION_CD = '" + locationCd + "' AND DR.DW_END_OPERATOR_CD IS NOT NULL ");
            selectQuery.Append(" AND ROWNUM <=  (CASE WHEN (ROWNUM >100)  THEN 100 ELSE ROWNUM END) ");

            selectQuery.Append(" ORDER BY PP.CHUUBAN ASC, DR.DW_TABA_NO_1 ASC ");

            result = ExecuteQueryMultiResult<ColdDrawingWireDrawingDetails>(selectQuery.ToString());
            return result;


        }
        public List<ColdDrawingWireDrawingDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, string Thaba_No_String)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new List<ColdDrawingWireDrawingDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" WITH QUERY_STRING AS( ");
            selectQuery.Append(" SELECT DR.DRAWING_CD  ");
            selectQuery.Append(" ,DR.REDRAWING  "); 
            selectQuery.Append(" ,DR.DW_DECISION AS DECISION");
            
            //selectQuery.Append(" ,DR.DW_TABA_NO_1 ");
            //selectQuery.Append(" ,DR.DW_TABA_NO_2 ");
            //selectQuery.Append(" ,DR.DW_TABA_NO_3 ");
            //selectQuery.Append(" ,DR.DW_TABA_NO_4 ");
            selectQuery.Append(" ,CONCAT (DR.DW_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");


            selectQuery.Append(" FROM TRN_DRAWING_W DR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND DR.IM_DRAWING = 0");
            selectQuery.Append(" AND Dr.DW_DECISION ='" + decisionVal + "' ");
            selectQuery.Append(" AND DR.DW_TABA_NO_1 IN ('" + Thaba_No_String + "') OR DR.DW_TABA_NO_2 IN ('" + Thaba_No_String + "') OR DR.DW_TABA_NO_3 IN ('" + Thaba_No_String + "') OR DR.DW_TABA_NO_4 IN ('" + Thaba_No_String + "') ");
            selectQuery.Append(" ORDER BY DR.DRAWING_CD");
            selectQuery.Append(" ) ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  DRAWING_CD ");
            selectQuery.Append(" ,REDRAWING  ");
            selectQuery.Append(" ,DECISION ");
            selectQuery.Append(" ,REGEXP_SUBSTR(TABA_NO, '[^, ]+', 1, LEVEL) AS TABA_NO ");
            selectQuery.Append(" FROM QUERY_STRING ");
            selectQuery.Append(" CONNECT BY LEVEL <= REGEXP_COUNT(TABA_NO, ',') + 1 ");
            result = ExecuteQueryMultiResult<ColdDrawingWireDrawingDetails>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on  2023/04/14  
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Cold Drawing Wire Drawing(Coil To Coil) process data according to search criteria        
        /// </returns>
        /// 
        public List<ColdDrawingWireDrawingDetails> GetWireDrawingDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCriteria,int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<ColdDrawingWireDrawingDetails> result = new List<ColdDrawingWireDrawingDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(DW.DW_START_DATE, 'YYYY/MM/DD HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_END_DATE, 'YYYY/MM/DD HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PROD_SPEC_NO, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" DW.ROLLING_NUMBER AS ROLLING_NUMBER, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" DW.DIE_DIM AS DW_DIE_DIM, ");
            selectQuery.Append(" DW.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" DW.DIE_NO AS DW_DIE_NO,");
            //selectQuery.Append(" DW.DW_ACT_DIM_START,");
            // selectQuery.Append(" DW.DW_ACT_DIM_END,");
            selectQuery.Append(" CONCAT(TO_CHAR(DW.DW_ACT_DIM_START, 'FM990.000'), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_ACT_DIM_END IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(DW.DW_ACT_DIM_END, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append("  ");
            selectQuery.Append(" ) AS ALL_ACT_DIM, ");
            selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS,");
            selectQuery.Append(" DW.WINDING_SHAPE AS DW_WINDING_SHAPE,");
            selectQuery.Append(" DW.DW_FINISHED_QTY AS DW_FINISHED_QTY,");
            selectQuery.Append(" DW.DW_MAGNETIC_INSPECTIONS AS DW_MAGNETIC_INSPECTIONS, ");
            selectQuery.Append(" CONCAT(DW_TABA_NO_1, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', DW_TABA_NO_2) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DW_TABA_NO_1_2, ");
            selectQuery.Append(" CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" DW_TABA_NO_3 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', DW_TABA_NO_4) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DW_TABA_NO_3_4, ");
            selectQuery.Append(" M.MSG_CONTENTS AS DW_REMARKS, ");
            selectQuery.Append(" DW.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
            selectQuery.Append(" PP.PROD_LENGTH,");
            selectQuery.Append(" DW.IM_DRAWING AS DW_IM_DRAWING");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING_W DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DW.DW_NEXT_PROCESS");
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
            //    query += " ORDER BY DW_END_DATE_STRING ASC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<ColdDrawingWireDrawingDetails>(query);
            return result;
        }

        /// <summary>
        /// CREATED BY : 
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Cold Drawing Wire Drawing(Coil To Coil) process data according to search criteria        
        /// </returns>
        /// 
        public List<ColdDrawingWireDrawingDetails> GetDailyProcessColdDrawingW(List<SearchField> searchCriteria, string sortCriteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN ");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING  ");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.RECEIVER_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");            
            selectQuery.Append(", NVL(INTE.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(", PP.PROD_DIM_W AS PP_PROD_DIM_W");
            selectQuery.Append(", PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(", PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN");
            selectQuery.Append(", PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX");
            selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(", PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER");
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS COIL_DIA_EXTERNAL");
            selectQuery.Append(", PP.COIL_DIA_WINDING AS COIL_DIA_WINDING");

            selectQuery.Append(",  PP.NO_OF_UNITS AS PP_NO_OF_UNITS  ");
            selectQuery.Append(",  PP.UNIT_WT AS PP_UNIT_WT");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(",  PP.CHAMFER AS PP_CHAMFER");
            selectQuery.Append(",  PP.CORNER_RADIUS AS PP_CORNER_R");

            selectQuery.Append(", PP.MAGARI_KUBUN AS MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS NEJIRE_SPAN");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");
            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR");
            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");

            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_NAME_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_NAME_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_NAME_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_NAME_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_NAME_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_NAME_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_NAME_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_NAME_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_NAME_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_NAME_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_NAME_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_NAME_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_NAME_13");

            selectQuery.Append(", PP.TEST_SPECIMEN AS TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1");
            selectQuery.Append(", PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX");
            selectQuery.Append(", PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE");
            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1");

            selectQuery.Append(", PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX");
            selectQuery.Append(", PP.HARDNESS_PATTERN3_ACTUAL AS HARDNESS_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1");
            selectQuery.Append(", PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX");
            selectQuery.Append(", PP.YEILD_PATTERN3_ACTUAL AS YEILD_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1");
            selectQuery.Append(", PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX");
            selectQuery.Append(", PP.PRESS_PATTERN3_ACTUAL AS PRESS_PATTERN3_ACTUAL");

            selectQuery.Append(", PP.DECARB_1 AS DECARB_1");
            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1");
            selectQuery.Append(", PP.DECARB_2 AS DECARB_2");
            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2");
            selectQuery.Append(", PP.DECARB_3 AS DECARB_3");
            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1 ");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX ");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN ");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX ");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL ");
            selectQuery.Append(", PP.RANK_STD AS RANK_STD ");

            selectQuery.Append(", PP.HT_CONDITION AS PP_HT_CONDITION");
            selectQuery.Append(", PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK ");
            selectQuery.Append(", PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(", CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE");

            selectQuery.Append(", PP.PROCESS_PLAN_KUBUN AS PROCESS_PLAN_KUBUN ");
            selectQuery.Append(", PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN ");
            selectQuery.Append(", PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN");
            selectQuery.Append(", PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN");
            selectQuery.Append(", PP.KAKOUIARI_QTY AS KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            //
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");

            //Cold_Drawing_Wire          
            selectQuery.Append(", MAC.MACHINE_NAME AS DW_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS DW_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( DW.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(", EMP1.SHORT_NAME AS DW_START_OPERATOR_NAME");
            selectQuery.Append(", DW.ROLLING_NUMBER AS DW_ROLLING_NUMBER");
            selectQuery.Append(", DW.FURNACE_NO AS DW_FURNACE_NO");
            selectQuery.Append(", DW.DIE_DIM AS DW_DIE_DIM");
            selectQuery.Append(", DW.DIE_SHAPE AS DW_DIE_SHAPE");
            selectQuery.Append(", DW.DIE_NO AS DW_DIE_NO");
            selectQuery.Append(", DW.DIE_CHK_START AS DIE_CHK_START ");
            selectQuery.Append(", DW.WINDING_SHAPE AS DW_WINDING_SHAPE");
            selectQuery.Append(", DW.IM_DRAWING AS DW_IM_DRAWING");
            selectQuery.Append(", DW.REDRAWING AS DW_REDRAWING");
            selectQuery.Append(", TO_CHAR( DW.DW_END_DATE,'YYYY/MM/DD HH24:MI' ) AS DW_END_DATE_STRING");
            //selectQuery.Append(", DW.DW_ACT_DIM_CHK  AS DW_ACT_DIM_CHK ");
            //selectQuery.Append(", DW.DW_ACT_DIM_START AS DW_ACT_DIM_START ");
            //selectQuery.Append(", DW.DW_ACT_DIM_END AS DW_ACT_DIM_END ");
            selectQuery.Append(", TO_CHAR(DW.DW_ACT_DIM_START, 'FM990.000') AS DW_ACT_DIM_START_STRING ");
            selectQuery.Append(", TO_CHAR(DW.DW_ACT_DIM_END, 'FM990.000') AS DW_ACT_DIM_END_STRING ");
            selectQuery.Append(", DW.DW_MAGNETIC_INSPECTIONS AS DW_MAGNETIC_INSPECTIONS");
            selectQuery.Append(", EMP2.SHORT_NAME AS DW_END_OPERATOR_NAME ");


            selectQuery.Append(", DW.DW_FINISHED_COILS AS DW_FINISHED_COILS");
            selectQuery.Append(", DW.UNIT_TYPE AS DW_UNIT_TYPE");
            selectQuery.Append(", DW.DW_FINISHED_QTY AS DW_FINISHED_QTY");
            selectQuery.Append(", DW.DW_TABA_NO_1 AS DW_TABA_NO_1");
            selectQuery.Append(", DW.DW_TABA_NO_2 AS DW_TABA_NO_2");
            selectQuery.Append(", DW.DW_TABA_NO_3 AS DW_TABA_NO_3");
            selectQuery.Append(", DW.DW_TABA_NO_4 AS DW_TABA_NO_4");
            selectQuery.Append(", DW.UNIT_TYPE AS DW_UNIT_TYPE");
            selectQuery.Append(", DW.DW_EXT_TEXTURE_STATUS AS DW_EXT_TEXTURE_STATUS");
            selectQuery.Append(", DW.DIE_CHK_END AS DIE_CHK_END ");
            selectQuery.Append(", DW.DW_DECISION AS DW_DECISION  ");
            selectQuery.Append(", CON.CONDITION_CD ");
            selectQuery.Append(", CONDITION1.CONDITION_CD ");
            selectQuery.Append(",MSG.MSG_CONTENTS AS DW_REMARKS ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_W DW ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");            
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");            
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DW.DW_START_OPERATOR_CD");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD");
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
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON DW.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DW.MSG_CD ");
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
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return ExecuteQueryMultiResult<ColdDrawingWireDrawingDetails>(query);
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
        public int GetRowCountForWireDrawingFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT (*) ");            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING_W DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DW.DW_NEXT_PROCESS");
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
        public int GetRowCountForColdDrawingCoilToCoilDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_W DW ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DW.DW_START_OPERATOR_CD");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD");
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
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON DW.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DW.MSG_CD ");
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




