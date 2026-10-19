using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class HikinukiBarToBarRepository : BaseRepository, IHikinukiBarToBarRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public HikinukiBarToBarRepository(IConfiguration _configuration,LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }
        public List<HikinukiBarToBarDetails> GetHikinukiBarToBarList(List<SearchField> searchCriteria, string sortCiteria, short locationCD)
        {
            List<HikinukiBarToBarDetails> result = new List<HikinukiBarToBarDetails>();
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            //selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
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
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");    //AND PR.VER = PP.PROD_VER
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            //selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            // selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            //selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_SHAPE_STRING ");
            
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");


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
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
            //selectQuery.Append(", K.REMARKS AS PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(", PP.PROCESS_PLAN_REMARK AS PP_PROCESS_PLAN_REMARK ");

            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE, ");

            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1,");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2,");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3,");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4,");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5,");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6,");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7,");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8,");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9,");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10,");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11,");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12,");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13,");

            selectQuery.Append(" P1.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_1,");
            selectQuery.Append(" P2.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_2,");
            selectQuery.Append(" P3.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_3,");
            selectQuery.Append(" P4.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_4,");
            selectQuery.Append(" P5.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_5,");
            selectQuery.Append(" P6.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_6,");
            selectQuery.Append(" P7.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_7,");
            selectQuery.Append(" P8.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_8,");
            selectQuery.Append(" P9.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_9,");
            selectQuery.Append(" P10.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_10,");
            selectQuery.Append(" P11.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_11,");
            selectQuery.Append(" P12.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_12,");
            selectQuery.Append(" P13.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_13,");

            selectQuery.Append(" P1.PROCESS_PROD_POINT AS PP_PROD_POINT_1,");
            selectQuery.Append(" P2.PROCESS_PROD_POINT AS PP_PROD_POINT_2,");
            selectQuery.Append(" P3.PROCESS_PROD_POINT AS PP_PROD_POINT_3,");
            selectQuery.Append(" P4.PROCESS_PROD_POINT AS PP_PROD_POINT_4,");
            selectQuery.Append(" P5.PROCESS_PROD_POINT AS PP_PROD_POINT_5,");
            selectQuery.Append(" P6.PROCESS_PROD_POINT AS PP_PROD_POINT_6,");
            selectQuery.Append(" P7.PROCESS_PROD_POINT AS PP_PROD_POINT_7,");
            selectQuery.Append(" P8.PROCESS_PROD_POINT AS PP_PROD_POINT_8,");
            selectQuery.Append(" P9.PROCESS_PROD_POINT AS PP_PROD_POINT_9,");
            selectQuery.Append(" P10.PROCESS_PROD_POINT AS PP_PROD_POINT_10,");
            selectQuery.Append(" P11.PROCESS_PROD_POINT AS PP_PROD_POINT_11,");
            selectQuery.Append(" P12.PROCESS_PROD_POINT AS PP_PROD_POINT_12,");
            selectQuery.Append(" P13.PROCESS_PROD_POINT AS PP_PROD_POINT_13,");

            selectQuery.Append(" P1.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_1,");
            selectQuery.Append(" P2.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_2,");
            selectQuery.Append(" P3.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_3,");
            selectQuery.Append(" P4.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_4,");
            selectQuery.Append(" P5.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_5,");
            selectQuery.Append(" P6.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_6,");
            selectQuery.Append(" P7.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_7,");
            selectQuery.Append(" P8.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_8,");
            selectQuery.Append(" P9.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_9,");
            selectQuery.Append(" P10.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_10,");
            selectQuery.Append(" P11.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_11,");
            selectQuery.Append(" P12.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_12,");
            selectQuery.Append(" P13.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_13");

            selectQuery.Append(" ,DW.DRAWING_CD AS DRAWING_CD  ");
            selectQuery.Append(" ,DW.PROCESS_PLAN_CD AS DW_PROCESS_PLAN_CD  ");
            selectQuery.Append(" ,DW.MACHINE_CD AS MACHINE_CD  ");
            selectQuery.Append(",(SELECT MACHINE_NAME FROM MST_MACHINE WHERE MACHINE_CD =DW.MACHINE_CD ) AS DW_MACHINE_NAME");
            selectQuery.Append(" ,TO_CHAR( DW.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING ");
            selectQuery.Append(" , DW.DW_START_OPERATOR_CD AS DW_START_OPERATOR_CD");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME");
            selectQuery.Append(" , DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,DW.DIE_DIM ");
            selectQuery.Append(" ,DW.DIE_SHAPE");
            selectQuery.Append(" ,DW.DIE_NO");
            selectQuery.Append(" ,DW.HT_CONDITION");
            selectQuery.Append(", DW.REDRAWING");
            selectQuery.Append(",DW.DW_END_DATE");
            selectQuery.Append(",DW.DW_END_OPERATOR_CD");
            selectQuery.Append(",DW.DW_DECISION AS DW_DECISION");
            selectQuery.Append(",DW.DW_NEXT_PROCESS");
            selectQuery.Append(",DW.DW_ACT_DIM_CHK");
            selectQuery.Append(",DW.DW_ACT_DIM1");
            selectQuery.Append(",DW.DW_ACT_DIM2");
            selectQuery.Append(",DW.DW_ACT_DIM3");
            selectQuery.Append(",DW.DW_ACT_LENGTH_CHK");
            selectQuery.Append(",DW.DW_ACT_LENGTH1");
            selectQuery.Append(",DW.DW_ACT_LENGTH2");
            selectQuery.Append(",DW.DW_ACT_LENGTH3");
            selectQuery.Append(",DW.DW_FINISHED_BARS");
            selectQuery.Append(",DW.DW_FINISHED_QTY");
            selectQuery.Append(",DW.DW_FINISHED_QTY_CHK");
            selectQuery.Append(",DW.DW_TABA_NO_");
            selectQuery.Append(",DW.DW_EXT_TEXTURE_STATUS");
            selectQuery.Append(",DW.DW_BEND_STATUS");
            selectQuery.Append(",DW.UNIT_TYPE");
            selectQuery.Append(",DW.DW_NG_BARS1");
            selectQuery.Append(",DW.DW_NG_BARS2");
            selectQuery.Append(",DW.DW_NG_BARS3");
            selectQuery.Append(",DW.DW_CORRECTED_BARS");
            selectQuery.Append(",DW.DW_DISPOSAL_NO");
            selectQuery.Append(",DW.MSG_CD AS DW_MSG_CD");
            selectQuery.Append(",DW.USER_ID AS USER_ID");
            selectQuery.Append(",DW.CREATED_DATE AS DW_CREATED_DATE");
            selectQuery.Append(",DW.UPDATED_DATE AS DW_UPDATED_DATE");
            selectQuery.Append(",TO_CHAR(DW.CREATED_DATE,'yyyy/mm/dd HH24:MI') AS DW_CREATED_DATE_STRING");
            selectQuery.Append(",TO_CHAR(DW.UPDATED_DATE,'yyyy/mm/dd HH24:MI') AS DW_UPDATED_DATE_STRING");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT JOIN TRN_DRAWING_D DW ON DW.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K");
            //selectQuery.Append(" ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD");

            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD =PP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            //selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            //selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =PP.LOCATION_CD");

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

            selectQuery.Append(" WHERE DW.DW_END_DATE IS NOT NULL");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" AND DW.INPUT_LOCATION_CD = " + locationCD);
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
            //    query += "ORDER BY PP_CHUUBAN ,DW.DW_TABA_NO_ ASC ";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<HikinukiBarToBarDetails>(query);
            return result;
        }

        public List<ThabaDetails> GetThabaDetails(string chuuban, int gyouban)
        {
            List<ThabaDetails> result = null;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN  ");
            selectQuery.Append(" ,PP.GYOUBAN ");
            selectQuery.Append(" ,DW.DW_DECISION AS DECISION");
            selectQuery.Append(" ,DW.CREATED_DATE ");
            selectQuery.Append(" ,TO_CHAR(DW.DW_TABA_NO_) AS TABA_NO ");
            selectQuery.Append(" ,DECODE(DW.DW_DECISION,0,1,2,3) AS PROCESS_STATUS ");
            selectQuery.Append(" FROM TRN_DRAWING_D DW INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND DW.IM_DRAWING = 0 ");
            selectQuery.Append(" AND DW.DW_DECISION IS NOT NULL ");
            selectQuery.Append(" AND DW.UPDATED_DATE IN ( ");
            selectQuery.Append(" SELECT  MAX(DW.UPDATED_DATE) FROM ( ");
            selectQuery.Append(" SELECT  DW.UPDATED_DATE , DW.DW_TABA_NO_ AS TABA_NO_INT ");

            selectQuery.Append(" FROM TRN_DRAWING_D DW INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
           
            selectQuery.Append(" ORDER BY DW_TABA_NO_ ) GROUP BY DW_TABA_NO_  ) ");
            selectQuery.Append(" ORDER BY DW.DRAWING_CD ");
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }

        public HikinukiBarToBarDetails GetHikinukiBarToBarDetailsByChuuban(string chuuban, short? gyouban, int? mechineCD)
        {
            int rowsCount = 0;

            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(" SELECT ");
            sQuery.Append(" COUNT(PP.PROCESS_PLAN_CD) ");
            sQuery.Append(" FROM TRN_DRAWING_D DW ");
            sQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            sQuery.Append(" WHERE PP.PROCESS_PLAN_CMPL_DATE IS NULL AND PP.CHUUBAN= '" + chuuban + "'");
            sQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "' ");
            sQuery.Append(" AND DW.MACHINE_CD = '" + mechineCD + "' ");
            sQuery.Append(" AND DW.CREATED_DATE >= TO_DATE(SYSDATE)-" + days);
            rowsCount = Convert.ToInt32(ExecuteScalar(sQuery.ToString()));

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
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
            //selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            //selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ");
            //selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            //selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");
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
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");
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
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            //selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetProdUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");    //GetMatlUnitForDBQuery
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_WINDING ");
            //selectQuery.Append(" ,PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING ");
            selectQuery.Append(" ,PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            selectQuery.Append(" ,DW.DRAWING_CD AS DRAWING_CD ");
            selectQuery.Append(" ,DW.CREATED_DATE  AS CREATED_DATE ");
            selectQuery.Append(" ,DW.UPDATED_DATE  AS UPDATED_DATE ");
            selectQuery.Append(" ,(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD = DW.MACHINE_CD) AS DW_MACHINE_NAME ");
            selectQuery.Append(" ,TO_NUMBER(DW.PROCESS_PLAN_CD) ");
            selectQuery.Append(" ,DW.DW_START_DATE AS DW_START_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DW.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(" ,DW.DW_START_OPERATOR_CD AS DW_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME AS SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME  ");
            selectQuery.Append(" ,DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,DW.DIE_DIM AS DIE_DIM ");
            selectQuery.Append(" ,DW.DIE_SHAPE AS DIE_SHAPE ");
            selectQuery.Append(" ,DW.DIE_NO AS DIE_NO ");
            selectQuery.Append(" ,DW.HT_CONDITION AS HT_CONDITION ");
            selectQuery.Append(" ,DW.IM_DRAWING");
            selectQuery.Append(" ,DW.REDRAWING AS REDRAWING ");

            selectQuery.Append(" ,DW.MSG_CD AS MSG_CD ");
            // selectQuery.Append(" ,ST.STAMP_DETAILS AS STAMP_DETAILS ");
            selectQuery.Append(" ,DW.DW_END_DATE AS DW_END_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DW.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(" ,DW.DW_END_OPERATOR_CD AS DW_END_OPERATOR_CD ");
            selectQuery.Append(" ,DW.DW_TABA_NO_ AS DW_TABA_NO ");
            selectQuery.Append(" ,DW.DW_ACT_DIM_CHK AS DW_ACT_DIM_CHK ");
            selectQuery.Append(" ,DW.DW_ACT_DIM1 AS DW_ACT_DIM1 ");
            selectQuery.Append(" ,DW.DW_ACT_DIM2 AS DW_ACT_DIM2 ");
            selectQuery.Append(" ,DW.DW_ACT_DIM3 AS DW_ACT_DIM3 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH_CHK AS DW_ACT_LENGTH_CHK ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3 ");
            selectQuery.Append(" ,DW.DW_FINISHED_QTY AS DW_FINISHED_QTY ");
            selectQuery.Append(" ,DW.DW_FINISHED_QTY_CHK AS DW_FINISHED_QTY_CHK ");

            selectQuery.Append(" ,DW.DW_NG_BARS1 AS DW_NG_BARS1 ");
            selectQuery.Append(" ,DW.DW_NG_BARS2 AS DW_NG_BARS2 ");
            selectQuery.Append(" ,DW.DW_NG_BARS3 AS DW_NG_BARS3 ");
            selectQuery.Append(", DW.DW_NG_DETAILS1");
            selectQuery.Append(", DW.DW_NG_DETAILS2 ");
            selectQuery.Append(", DW.DW_NG_DETAILS3 ");


            selectQuery.Append(", DW.DW_NEXT_PROCESS ");
            selectQuery.Append(", M.MSG_CONTENTS AS DW_REMARKS  ");
            selectQuery.Append(" ,DW.DW_CORRECTED_BARS AS DW_CORRECTED_BARS ");
            selectQuery.Append(" ,DW.DW_DISPOSAL_NO AS DW_DISPOSAL_NO ");
            selectQuery.Append(", DW.DW_BEND_STATUS");
            selectQuery.Append(", DW.DW_EXT_TEXTURE_STATUS");
            selectQuery.Append(", (SELECT DW.UNIT_TYPE FROM TRN_DRAWING_D DW INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE  PP.PROCESS_PLAN_CMPL_DATE IS NULL  AND PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND DW.MACHINE_CD = '" + mechineCD + "' ");
            selectQuery.Append(" AND DW.UNIT_TYPE IS NOT NULL AND DW.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " ORDER BY  DW.DRAWING_CD DESC FETCH FIRST ROW ONLY)  AS UNIT_TYPE ");
            selectQuery.Append(" ,DW.DW_DECISION AS DW_DECISION ");
            selectQuery.Append(" ,DW.DW_NEXT_PROCESS AS DW_NEXT_PROCESS ");
            selectQuery.Append(" ,DW.USER_ID AS USER_ID ");
            selectQuery.Append(" ,DW.DW_TABA_NO_,");

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
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append("PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE,");
            selectQuery.Append("PP.PROCESS_PLAN_START_DATE, ");
            selectQuery.Append("COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION, ");
            if (rowsCount == 0)
            {
                selectQuery.Append("'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append("'YES' AS PAST_DATA_EXIST ");
            }

                selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_D DW ON");
            selectQuery.Append(" DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append(" ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append(" ON M.MSG_CD = DW.MSG_CD  ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K  ");
            //selectQuery.Append(" ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD  ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR ON DGPGR.DEPARTMENT_GROUP_CD = pp.DEPARTMENT_GROUP_CD ");
            //selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP ON DEP.DEPARTMENT_CD = DGPGR.DEPARTMENT_CD");
            //selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = PP.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
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
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            if (rowsCount > 0)
            {
                selectQuery.Append(" WHERE PP.PROCESS_PLAN_CMPL_DATE IS NULL AND PP.CHUUBAN= '" + chuuban + "'");
                selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "' ");
                selectQuery.Append(" AND DW.MACHINE_CD = '" + mechineCD + "' ");
                selectQuery.Append(" AND DW.CREATED_DATE >= TO_DATE(SYSDATE)-" + days);

            }else
            {
                selectQuery.Append(" WHERE PP.PROCESS_PLAN_CMPL_DATE IS NULL AND PP.CHUUBAN= '" + chuuban + "'");
                selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "' ");
            }

                selectQuery.Append(" ORDER BY DW.DRAWING_CD DESC");
            HikinukiBarToBarDetails result = ExecuteQuerySingleResult<HikinukiBarToBarDetails>(selectQuery.ToString());
            return result;
        }

        public List<HikinukiBarToBarDetails> GetHikinukiBarToBar24MachineList(string machineCd)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" DW.MACHINE_CD AS MACHINE_CD,  ");
            selectQuery.Append(" DW.DW_DECISION AS DW_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");
            //selectQuery.Append(" CONCAT (CONCAT (PP.CHUUBAN, '-'), PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,	");

            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append(" DW.DW_METALLIC_PLATE_CODE  AS DW_METALLIC_PLATE_CODE,	");

            selectQuery.Append(" (CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE, ");

            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,	");
            // selectQuery.Append(" PP.MATL_DIM_D || '' || PP.MATL_SHAPE  AS PP_MATL_DIM_D_SHAPE, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,	");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,	");
            selectQuery.Append(" PP.PROD_SHAPE  AS  PP_PROD_SHAPE, ");
            //selectQuery.Append(" PP.PROD_DIM_D || '' || PP.PROD_SHAPE  AS  PP_ACT_DIM_D_SHAPE, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_ACT_DIM_D_SHAPE, ");
            selectQuery.Append(" DW.DW_TABA_NO_ AS DW_TABA_NO_,");
            selectQuery.Append(" DW.DIE_DIM, ");
            selectQuery.Append(" DW.DIE_SHAPE, ");
            selectQuery.Append(" DW.DIE_NO,");
            selectQuery.Append(" DW.HT_CONDITION,");
            selectQuery.Append(" DW.DW_ACT_DIM1,  ");
            selectQuery.Append(" DW.DW_ACT_DIM2,  ");
            selectQuery.Append(" DW.DW_ACT_DIM3,  ");
            selectQuery.Append(" DW.DW_ACT_LENGTH1,  ");
            selectQuery.Append(" DW.DW_ACT_LENGTH2,  ");
            selectQuery.Append(" DW.DW_ACT_LENGTH3 , ");
            selectQuery.Append(" DW.DW_FINISHED_BARS ,  ");
            selectQuery.Append(" DW.UNIT_TYPE , ");
            selectQuery.Append(" DW.DW_FINISHED_QTY,  ");
            selectQuery.Append(" DW.DW_BEND_STATUS,  ");
            selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS,  ");
            selectQuery.Append(" M.MSG_CONTENTS, ");
            selectQuery.Append(" DW.MSG_CD AS MSG_CD, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PROD_SPEC_NO,	");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,	");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" NVL (DW.DW_END_DATE, DW.DW_START_DATE) AS ORDER_CLAUSE ");
            selectQuery.Append(" FROM TRN_DRAWING_D DW");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ");
            selectQuery.Append(" ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" WHERE  DW.MACHINE_CD  = '" + machineCd + "' ");
            selectQuery.Append("  AND NVL(DW.DW_END_DATE, DW.DW_START_DATE) > SYSDATE - 1 ");
            selectQuery.Append(" ORDER BY ORDER_CLAUSE DESC");
            List<HikinukiBarToBarDetails> result = ExecuteQueryMultiResult<HikinukiBarToBarDetails>(selectQuery.ToString());
            return result;
        }

        public void SaveHikinukiBarToBar(HikinukiBarToBarDetails hikinukibartobarDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_DRAWING_D(");
            insertQuery.Append(" DRAWING_CD,");
            insertQuery.Append(" PROCESS_PLAN_CD,");
            insertQuery.Append(" MACHINE_CD,");
            insertQuery.Append(" DW_START_DATE,");
            insertQuery.Append(" DW_START_OPERATOR_CD,");
            insertQuery.Append(" DW_METALLIC_PLATE_CODE,");
            insertQuery.Append(" DIE_DIM,");
            insertQuery.Append(" DIE_SHAPE,");
            insertQuery.Append(" DIE_NO,");
            insertQuery.Append(" HT_CONDITION,");
            insertQuery.Append(" IM_DRAWING,");
            insertQuery.Append(" REDRAWING,");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" LOCATION_CD, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");
            insertQuery.Append(" PROCESS_KUBUN_CD, ");
            insertQuery.Append(" CREATED_DATE,");
            insertQuery.Append(" UPDATED_DATE, ");
            insertQuery.Append(" DW_TABA_NO_ ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("TRN_DRAWING_D_SEQ.NEXTVAL");
            insertQuery.Append(",'" + hikinukibartobarDetails.PP_PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.MACHINE_CD + "'");
            insertQuery.Append(", TO_DATE('" + (Convert.ToDateTime(hikinukibartobarDetails.DW_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + hikinukibartobarDetails.DW_START_OPERATOR_CD + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.DW_METALLIC_PLATE_CODE + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.DIE_DIM + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.DIE_SHAPE + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.DIE_NO + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.HT_CONDITION + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.IM_DRAWING + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.REDRAWING + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.USER_ID + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.LOCATION_CD + "'");    //
            insertQuery.Append(",'" + hikinukibartobarDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + hikinukibartobarDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + hikinukibartobarDetails.DW_TABA_NO_ + "'");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
        }

        public HikinukiBarToBarDetails GetHikinukiBarToBarDetailsByDrawingCD(long drawingCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            // selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
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
            //selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            //selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ")/;
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
           // selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
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
            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            //selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetProdUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");    //GetMatlUnitForDBQuery
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_WINDING AS COIL_DIA_WINDING ");
            selectQuery.Append(" ,PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            selectQuery.Append(" ,DW.DRAWING_CD AS DRAWING_CD ");
            selectQuery.Append(" ,DW.MACHINE_CD ");
            selectQuery.Append(" ,DW.CREATED_DATE  AS CREATED_DATE ");
            selectQuery.Append(" ,DW.UPDATED_DATE  AS UPDATED_DATE ");
            selectQuery.Append(" ,(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD = DW.MACHINE_CD) AS DW_MACHINE_NAME ");
            selectQuery.Append(" ,DW.PROCESS_PLAN_CD AS PROCESS_PLAN_CD ");
            selectQuery.Append(" ,DW.DW_START_DATE AS DW_START_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DW.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(" ,DW.DW_START_OPERATOR_CD AS DW_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME  ");
            selectQuery.Append(" ,DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,DW.DIE_DIM AS DIE_DIM ");
            selectQuery.Append(" ,DW.DIE_SHAPE AS DIE_SHAPE ");
            selectQuery.Append(" ,DW.DIE_NO AS DIE_NO ");
            selectQuery.Append(" ,DW.HT_CONDITION AS HT_CONDITION ");
            selectQuery.Append(" ,DW.IM_DRAWING ");
            selectQuery.Append(" ,DW.REDRAWING AS REDRAWING ");


            selectQuery.Append(" ,DW.MSG_CD AS MSG_CD ");
            // selectQuery.Append(" ,ST.STAMP_DETAILS AS STAMP_DETAILS ");
            selectQuery.Append(" ,DW.DW_END_DATE AS DW_END_DATE  ");
            selectQuery.Append(" ,TO_CHAR( DW.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(" ,DW.DW_END_OPERATOR_CD AS DW_END_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = DW_END_OPERATOR_CD) AS DW_END_OPERATOR_NAME  ");
            selectQuery.Append(" ,DW.DW_TABA_NO_ AS DW_TABA_NO_ ");
            selectQuery.Append(" ,DW.DW_ACT_DIM_CHK AS DW_ACT_DIM_CHK ");
            selectQuery.Append(" ,DW.DW_ACT_DIM1 AS DW_ACT_DIM1 ");
            selectQuery.Append(" ,DW.DW_ACT_DIM2 AS DW_ACT_DIM2 ");
            selectQuery.Append(" ,DW.DW_ACT_DIM3 AS DW_ACT_DIM3 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH_CHK AS DW_ACT_LENGTH_CHK ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3 ");
            selectQuery.Append(" ,DW.DW_FINISHED_BARS AS DW_FINISHED_BARS ");
            selectQuery.Append(" ,DW.DW_FINISHED_QTY AS DW_FINISHED_QTY ");
            selectQuery.Append(" ,DW.DW_FINISHED_QTY_CHK AS DW_FINISHED_QTY_CHK ");

            selectQuery.Append(" ,DW.DW_NG_BARS1 AS DW_NG_BARS1 ");
            selectQuery.Append(" ,DW.DW_NG_BARS2 AS DW_NG_BARS2 ");
            selectQuery.Append(" ,DW.DW_NG_BARS3 AS DW_NG_BARS3 ");
            selectQuery.Append(", DW.DW_NG_DETAILS1");
            selectQuery.Append(", DW.DW_NG_DETAILS2 ");
            selectQuery.Append(", DW.DW_NG_DETAILS3 ");


            selectQuery.Append(", DW.DW_NEXT_PROCESS ");
            selectQuery.Append(", M.MSG_CONTENTS AS DW_REMARKS  ");
            selectQuery.Append(" ,DW.DW_CORRECTED_BARS AS DW_CORRECTED_BARS ");
            selectQuery.Append(" ,DW.DW_DISPOSAL_NO AS DW_DISPOSAL_NO ");
            selectQuery.Append(", DW.DW_BEND_STATUS");
            selectQuery.Append(", DW.DW_EXT_TEXTURE_STATUS");
            selectQuery.Append(", DW.UNIT_TYPE AS UNIT_TYPE ");
            selectQuery.Append(" ,DW.DW_DECISION AS DW_DECISION ");
            selectQuery.Append(" ,DW.DW_NEXT_PROCESS AS DW_NEXT_PROCESS ");
            selectQuery.Append(" ,DW.USER_ID AS USER_ID, ");
            //selectQuery.Append(" ,NY.PRODUCT_NYUKO_CD,");

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
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append("PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE,");
            selectQuery.Append("COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_D DW ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M  ON M.MSG_CD = DW.MSG_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K  ");
            //selectQuery.Append(" ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD  ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR ON DGPGR.DEPARTMENT_GROUP_CD = pp.DEPARTMENT_GROUP_CD ");
            //selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
            //selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP ON DEP.DEPARTMENT_CD = DGPGR.DEPARTMENT_CD");
            //selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DGPGR.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = PP.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
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
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");

            selectQuery.Append(" WHERE DW.DRAWING_CD= '" + drawingCD + "'");
            selectQuery.Append(" ORDER BY DW.DRAWING_CD DESC");
            HikinukiBarToBarDetails result = ExecuteQuerySingleResult<HikinukiBarToBarDetails>(selectQuery.ToString());
            return result;
        }

        public long HikinukiBarToBarDetailsUpdate(HikinukiBarToBarDetails hikinukibartobarDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_DRAWING_D SET ");

            // Start data is updated

            updateQuery.Append(" DW_START_DATE = ");
            //  updateQuery.Append(" TO_DATE('" + (Convert.ToDateTime(straighteningDetails.STR_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(hikinukibartobarDetails.DW_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");

            updateQuery.Append(" DW_START_OPERATOR_CD = '" + hikinukibartobarDetails.DW_START_OPERATOR_CD + "',");
            updateQuery.Append(" DW_METALLIC_PLATE_CODE ='" + hikinukibartobarDetails.DW_METALLIC_PLATE_CODE + "',");
            updateQuery.Append(" DIE_DIM='" + hikinukibartobarDetails.DIE_DIM + "',");
            updateQuery.Append(" DIE_SHAPE='" + hikinukibartobarDetails.DIE_SHAPE + "',");
            updateQuery.Append(" DIE_NO='" + hikinukibartobarDetails.DIE_NO + "',");
            updateQuery.Append(" HT_CONDITION='" + hikinukibartobarDetails.HT_CONDITION + "',");
            updateQuery.Append(" IM_DRAWING ='" + hikinukibartobarDetails.IM_DRAWING + "',");
            updateQuery.Append(" REDRAWING ='" + hikinukibartobarDetails.REDRAWING + "',");
            updateQuery.Append(" MACHINE_CD ='" + hikinukibartobarDetails.MACHINE_CD + "',");



            updateQuery.Append(" DW_END_OPERATOR_CD ='" + hikinukibartobarDetails.DW_END_OPERATOR_CD + "',");
            updateQuery.Append(" DW_END_DATE= ");
            //   updateQuery.Append(" TO_DATE('" + (Convert.ToDateTime(straighteningDetails.STR_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(hikinukibartobarDetails.DW_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");

            updateQuery.Append(" DW_DECISION ='" + hikinukibartobarDetails.DW_DECISION + "',");
            updateQuery.Append(" DW_NEXT_PROCESS ='" + hikinukibartobarDetails.DW_NEXT_PROCESS + "',");
            updateQuery.Append(" DW_ACT_DIM_CHK ='" + hikinukibartobarDetails.DW_ACT_DIM_CHK + "',");
            updateQuery.Append(" DW_ACT_DIM1 ='" + hikinukibartobarDetails.DW_ACT_DIM1 + "',");
            updateQuery.Append(" DW_ACT_DIM2 ='" + hikinukibartobarDetails.DW_ACT_DIM2 + "',");
            updateQuery.Append(" DW_ACT_DIM3 ='" + hikinukibartobarDetails.DW_ACT_DIM3 + "',");
            updateQuery.Append(" DW_ACT_LENGTH_CHK ='" + hikinukibartobarDetails.DW_ACT_LENGTH_CHK + "',");

            updateQuery.Append(" DW_ACT_LENGTH1 = '" + hikinukibartobarDetails.DW_ACT_LENGTH1 + "',");
            updateQuery.Append(" DW_ACT_LENGTH2  = '" + hikinukibartobarDetails.DW_ACT_LENGTH2 + "',");
            updateQuery.Append(" DW_ACT_LENGTH3 = '" + hikinukibartobarDetails.DW_ACT_LENGTH3 + "',");
            updateQuery.Append(" DW_FINISHED_BARS = '" + hikinukibartobarDetails.DW_FINISHED_BARS + "',");

            updateQuery.Append(" DW_FINISHED_QTY = '" + hikinukibartobarDetails.DW_FINISHED_QTY + "',");
            updateQuery.Append(" DW_FINISHED_QTY_CHK  = '" + hikinukibartobarDetails.DW_FINISHED_QTY_CHK + "',");
            updateQuery.Append(" DW_TABA_NO_ = '" + hikinukibartobarDetails.DW_TABA_NO_ + "',");
            updateQuery.Append(" DW_EXT_TEXTURE_STATUS  = '" + hikinukibartobarDetails.DW_EXT_TEXTURE_STATUS + "',");
            updateQuery.Append(" DW_BEND_STATUS = '" + hikinukibartobarDetails.DW_BEND_STATUS + "',");
            updateQuery.Append(" UNIT_TYPE = '" + hikinukibartobarDetails.UNIT_TYPE + "',");
            updateQuery.Append(" MSG_CD ='" + hikinukibartobarDetails.MSG_CD + "',");

            updateQuery.Append(" DW_NG_BARS1 = '" + hikinukibartobarDetails.DW_NG_BARS1 + "',");
            updateQuery.Append(" DW_NG_BARS2  = '" + hikinukibartobarDetails.DW_NG_BARS2 + "',");
            updateQuery.Append(" DW_NG_BARS3 = '" + hikinukibartobarDetails.DW_NG_BARS3 + "',");
            updateQuery.Append(" DW_NG_DETAILS1 = '" + hikinukibartobarDetails.DW_NG_DETAILS1 + "',");
            updateQuery.Append(" DW_NG_DETAILS2  = '" + hikinukibartobarDetails.DW_NG_DETAILS2 + "',");
            updateQuery.Append(" DW_NG_DETAILS3 = '" + hikinukibartobarDetails.DW_NG_DETAILS3 + "',");
            updateQuery.Append(" DW_CORRECTED_BARS  = '" + hikinukibartobarDetails.DW_CORRECTED_BARS + "',");
            updateQuery.Append(" DW_DISPOSAL_NO = '" + hikinukibartobarDetails.DW_DISPOSAL_NO + "',");

            updateQuery.Append(" PROCESS_KUBUN_CD ='" + hikinukibartobarDetails.PROCESS_KUBUN_CD + "',");


            updateQuery.Append(" USER_ID ='" + hikinukibartobarDetails.USER_ID + "',");
            updateQuery.Append(" LOCATION_CD ='" + hikinukibartobarDetails.LOCATION_CD + "',");
            updateQuery.Append(" INPUT_LOCATION_CD ='" + (hikinukibartobarDetails.INPUT_LOCATION_CD != null ? hikinukibartobarDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "',");
            updateQuery.Append(" UPDATED_DATE = SYSDATE");

            updateQuery.Append(" WHERE ");
            updateQuery.Append(" DRAWING_CD= '" + hikinukibartobarDetails.DRAWING_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());

            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_DRAWING_D WHERE DRAWING_CD= '" + hikinukibartobarDetails.DRAWING_CD + "'");

            long msgCD = 0;

            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCD = Convert.ToInt64(cdVal);
            }
            return msgCD;

        }

        public void Delete(long DRAWING_CD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_DRAWING_D ");
            deleteQuery.Append(" WHERE DRAWING_CD =" + DRAWING_CD);
            ExecuteNonQuery(deleteQuery.ToString());
        }

         /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Drawing Bar To Bar process data according to search criteria        
        /// </returns>
        /// 
        public List<HikinukiBarToBarDetails> GetDrawingBarToBarDataOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria,int rowLimit,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<HikinukiBarToBarDetails> result = new List<HikinukiBarToBarDetails>();
            selectQuery.Append("SELECT  ");
            selectQuery.Append("LOCA.SHORT_NAME AS DW_LOCATION,");
            selectQuery.Append("PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            selectQuery.Append("MACH.MACHINE_NAME AS DW_MACHINE_NAME,");
            selectQuery.Append(" TO_CHAR(DRAWINGD.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DRAWINGD.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append("EMP.SHORT_NAME AS DW_END_OPERATOR_NAME,");
            selectQuery.Append("CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append("GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append("PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,PP.PROD_SPEC_NO1,PP.PROD_SPEC_NO2,");
            selectQuery.Append("PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append("PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append("PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append("PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append("PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append("PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append("PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR,");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append("DRAWINGD.DIE_DIM,");
            selectQuery.Append("DRAWINGD.DIE_SHAPE,");
            selectQuery.Append("DRAWINGD.DIE_NO,");
            selectQuery.Append("DRAWINGD.DW_ACT_DIM1,");
            selectQuery.Append("DRAWINGD.DW_ACT_DIM2,");
            selectQuery.Append("DRAWINGD.DW_ACT_DIM3,");
            selectQuery.Append("DRAWINGD.DW_FINISHED_BARS,");
            selectQuery.Append("DRAWINGD.UNIT_TYPE,");
            selectQuery.Append("DRAWINGD.DW_BEND_STATUS,");
            selectQuery.Append("DRAWINGD.DW_EXT_TEXTURE_STATUS,");
            selectQuery.Append("DRAWINGD. DW_METALLIC_PLATE_CODE,");
            selectQuery.Append("DRAWINGD.IM_DRAWING AS IM_DRAWING,");
            selectQuery.Append("DRAWINGD.DW_FINISHED_QTY AS DW_FINISHED_QTY,");
            selectQuery.Append("DRAWINGD.DW_NG_BARS1,");
            if(cultureName == "en")
            {
                selectQuery.Append("DEFECT1.OTHER_NAME_SHORT AS DW_NG_DETAILS1_STRING,");
                selectQuery.Append("DEFECT2.OTHER_NAME_SHORT AS DW_NG_DETAILS2_STRING,");
                selectQuery.Append("DEFECT3.OTHER_NAME_SHORT AS DW_NG_DETAILS3_STRING,");
            }
            else
            {
                selectQuery.Append("DEFECT1.PROC_DEFECT_SHORT AS DW_NG_DETAILS1_STRING,");
                selectQuery.Append("DEFECT2.PROC_DEFECT_SHORT AS DW_NG_DETAILS2_STRING,");
                selectQuery.Append("DEFECT3.PROC_DEFECT_SHORT AS DW_NG_DETAILS3_STRING,");
            }
                
            selectQuery.Append("DRAWINGD.DW_NG_BARS2,");
            
            selectQuery.Append("DRAWINGD.DW_NG_BARS3,");
            
            selectQuery.Append("PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append("DRAWINGD.DW_TABA_NO_ AS DW_TABA_NO_,");
            selectQuery.Append("MESS.MSG_CONTENTS AS DW_REMARKS,");
            selectQuery.Append("INTE.INTERNAL_NAME,");
            selectQuery.Append("PP.CHAMFER, ");
            selectQuery.Append("DRAWINGD.DW_DECISION, ");
            selectQuery.Append("DRAWINGD.HT_CONDITION AS HT_CONDITION,  ");
            selectQuery.Append("CONCAT(TO_CHAR(DRAWINGD.DW_ACT_DIM1, 'FM990.000'), CONCAT( ");
            selectQuery.Append("CASE ");
            selectQuery.Append("WHEN DRAWINGD.DW_ACT_DIM2 IS NOT NULL THEN ");
            selectQuery.Append("CONCAT('|　', TO_CHAR(DRAWINGD.DW_ACT_DIM2, 'FM990.000')) ");
            selectQuery.Append("ELSE NULL END, ");
            selectQuery.Append("CASE ");
            selectQuery.Append("WHEN DRAWINGD.DW_ACT_DIM3 IS NOT NULL THEN ");
            selectQuery.Append("CONCAT('|　', TO_CHAR(DRAWINGD.DW_ACT_DIM3, 'FM990.000')) ");
            selectQuery.Append("ELSE NULL END ");
            selectQuery.Append(")) AS ALL_ACT_DIM, ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");

            selectQuery.Append(" FROM TRN_DRAWING_D DRAWINGD");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DRAWINGD.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = DRAWINGD.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = DRAWINGD.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD =DRAWINGD.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DRAWINGD.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DRAWINGD.DW_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DRAWINGD.DW_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DRAWINGD.DW_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DRAWINGD.DW_NEXT_PROCESS");
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
          
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }
            //else
            //{
            //    query += " ORDER BY DW_END_DATE_STRING ASC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP_PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<HikinukiBarToBarDetails>(query);
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Drawing Bar To Bar process data according to search criteria        
        /// </returns>
        /// 
        public List<HikinukiBarToBarDetails> GetDailyProcessHikinukiBarToBar(List<SearchField> searchCriteria, string sortCriteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.SHORT_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", EMPL.SHORT_NAME AS PP_EMPLOYEE_NAME ");
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
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL");
            selectQuery.Append(", PP.COIL_DIA_WINDING AS P_COIL_DIA_WINDING");

            selectQuery.Append(",  PP.NO_OF_UNITS AS PP_NO_OF_UNITS  ");
            selectQuery.Append(",  PP.UNIT_WT AS PP_UNIT_WT");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(",  PP.CHAMFER AS PP_CHAMFER");
            selectQuery.Append(",  PP.CORNER_RADIUS AS PP_CORNER_R");

            selectQuery.Append(", PP.MAGARI_KUBUN AS MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");
            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR_NAME");
            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");

            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
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
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13");

            selectQuery.Append(", PP.TEST_SPECIMEN AS TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1");
            selectQuery.Append(", PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX ");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX");
            selectQuery.Append(", PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1");           

            selectQuery.Append(", PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", pp.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE");
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

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.RANK_STD AS PP_RANK_STD");

            selectQuery.Append(", PP.HT_CONDITION AS PP_HT_CONDITION");
            selectQuery.Append(", PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK ");
            selectQuery.Append(", PP.MATL_NAME_MAK AS MATL_MAKER_NAME ");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH ");
            selectQuery.Append(" ,CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION ");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(", PP.PROCESS_PLAN_KUBUN AS PP_PROCESS_PLAN_KUBUN");
            selectQuery.Append(", PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN");
            selectQuery.Append(", PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN");
            selectQuery.Append(", PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(", PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");

            // Drawing D 
            selectQuery.Append(", MAC.MACHINE_NAME AS DW_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS DW_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( DD.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");           
            selectQuery.Append(", EMP1.SHORT_NAME AS DW_START_OPERATOR_NAME");
            selectQuery.Append(", DD.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE");
            selectQuery.Append(", DD.DIE_DIM AS DW_DIE_DIM");
            selectQuery.Append(", DD.DIE_SHAPE AS DIE_SHAPE");
            selectQuery.Append(", DD.DIE_NO AS DW_DIE_NO");
            selectQuery.Append(", DD.HT_CONDITION AS HT_CONDITION");
            selectQuery.Append(", DD.IM_DRAWING AS IM_DRAWING");
            selectQuery.Append(", DD.REDRAWING AS REDRAWING");
            selectQuery.Append(", TO_CHAR( DD.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(", DD.DW_END_OPERATOR_CD AS DW_END_OPERATOR_CD");
            selectQuery.Append(", EMP2.SHORT_NAME AS DW_END_OPERATOR_NAME");
            //selectQuery.Append(", DD.DW_ACT_DIM1 AS DW_ACT_DIM1");
            //selectQuery.Append(", DD.DW_ACT_DIM2 AS DW_ACT_DIM2");
            //selectQuery.Append(", DD.DW_ACT_DIM3 AS DW_ACT_DIM3");

            selectQuery.Append(", TO_CHAR(DD.DW_ACT_DIM1, 'FM990.000') AS DW_ACT_DIM1_STRING ");
            selectQuery.Append(", TO_CHAR(DD.DW_ACT_DIM2, 'FM990.000') AS DW_ACT_DIM2_STRING ");
            selectQuery.Append(", TO_CHAR(DD.DW_ACT_DIM3, 'FM990.000') AS DW_ACT_DIM3_STRING ");

           selectQuery.Append(", DD.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1");
            selectQuery.Append(", DD.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2");
            selectQuery.Append(", DD.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3");
            selectQuery.Append(", DD.DW_FINISHED_BARS AS DW_FINISHED_BARS");
            selectQuery.Append(", DD.UNIT_TYPE AS UNIT_TYPE");
            selectQuery.Append(", DD.DW_FINISHED_QTY AS DW_FINISHED_QTY");
            selectQuery.Append(", DD.DW_TABA_NO_ AS DW_TABA_NO_");
            selectQuery.Append(", DD.DW_BEND_STATUS AS DW_BEND_STATUS");
            selectQuery.Append(", DD.DW_EXT_TEXTURE_STATUS AS DW_EXT_TEXTURE_STATUS");
            selectQuery.Append(", DD.DW_NG_BARS1 AS DW_NG_BARS1");
            selectQuery.Append(", DD.DW_NG_BARS2 AS DW_NG_BARS2");
            selectQuery.Append(", DD.DW_NG_BARS3 AS DW_NG_BARS3");
           
            selectQuery.Append(" ,DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP");
            selectQuery.Append(" ,DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN");
            selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP");
            selectQuery.Append(", DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN");
            selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP");
            selectQuery.Append(", DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN");
            selectQuery.Append(", DD.DW_CORRECTED_BARS AS DW_CORRECTED_BARS");
            selectQuery.Append(", DD.DW_DISPOSAL_NO AS DW_DISPOSAL_NO");
            selectQuery.Append(", DD.DW_DECISION AS DW_DECISION,");
            selectQuery.Append(" MSG.MSG_CONTENTS AS DW_REMARKS ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER JOIN TRN_DRAWING_D DD ON DD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DD.DW_START_OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DD.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
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
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DD.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = DD.DW_END_OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DD.MSG_CD ");            

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
             return ExecuteQueryMultiResult<HikinukiBarToBarDetails>(query);
        }
       
        public HikinukiBarToBarDetails GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? ThabaNo)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new HikinukiBarToBarDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, DW.DRAWING_CD, DW.REDRAWING");
            selectQuery.Append(" FROM TRN_DRAWING_D DW INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND DW.DW_DECISION ='" + decisionVal + "' ");
            selectQuery.Append(" AND DW.IM_DRAWING = 0");
            if (ThabaNo != 0)
            {
                selectQuery.Append(" AND DW.DW_TABA_NO_ ='" + ThabaNo + "' ");
            }
            selectQuery.Append(" ORDER BY DW.DRAWING_CD"); 


            result = ExecuteQuerySingleResult<HikinukiBarToBarDetails>(selectQuery.ToString());
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
        public int GetRowCountForHikinukiBarToBarFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT COUNT(*) ");            
            selectQuery.Append(" FROM TRN_DRAWING_D DRAWINGD");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DRAWINGD.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = DRAWINGD.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = DRAWINGD.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD =DRAWINGD.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DRAWINGD.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DRAWINGD.DW_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DRAWINGD.DW_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DRAWINGD.DW_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DRAWINGD.DW_NEXT_PROCESS");
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
        public int GetRowCountForHikinukiBarToBarDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER JOIN TRN_DRAWING_D DD ON DD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DD.DW_START_OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DD.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
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
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DD.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = DD.DW_END_OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DD.MSG_CD ");

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
        /// To bind machine name of Hikinuki Bar to Bar Process
        /// </returns>
        /// 
        public List<NameValue> GetHikinukiBarToBarMachineList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" AND MACHINE_TYPE = '" + UIConstants.K_MACHINEGROUP_DRAW_BENCH + "'");
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
    }
}
