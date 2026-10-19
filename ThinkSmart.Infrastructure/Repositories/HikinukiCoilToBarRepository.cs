using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class HikinukiCoilToBarRepository : BaseRepository,IHikinukiCoilToBarRepository
    {
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public HikinukiCoilToBarRepository(IConfiguration configuration, LocalizationServices localization) :base(configuration)
        {
            _localization = localization;
            days = Convert.ToInt32(configuration.GetSection("PastDaysForSearchChuuban").Value);
        }

        public List<ThabaDetails> GetThabaDetails(string chuuban, string gyouban)
        {
           
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT * FROM ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.CHUUBAN  ");
            selectQuery.Append(" ,PP.GYOUBAN ");
            selectQuery.Append(" ,DW.DW_DECISION AS DECISION");
            selectQuery.Append(" ,DW.DRAWING_CD");
            selectQuery.Append(" ,DW.CREATED_DATE ");
            selectQuery.Append(" ,CONCAT (DW.DW_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', DW.DW_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', DW.DW_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', DW.DW_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            selectQuery.Append(" ,DECODE(DW.DW_DECISION,0,1,2,3) AS PROCESS_STATUS, ");
            selectQuery.Append(" ROW_NUMBER() OVER ( ");
            selectQuery.Append(" PARTITION BY CONCAT(DW.DW_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_2 IS NOT NULL THEN CONCAT(', ', DW.DW_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_3 IS NOT NULL THEN CONCAT(', ', DW.DW_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE  WHEN DW.DW_TABA_NO_4 IS NOT NULL THEN CONCAT(', ', DW.DW_TABA_NO_4) ELSE NULL END )) ");
            selectQuery.Append(" ORDER BY dw.UPDATED_DATE DESC ) RN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" ) WHERE RN = 1  AND DECISION IS NOT NULL");
           

            //selectQuery.Append(" SELECT PP.CHUUBAN  ");
            //selectQuery.Append(" ,PP.GYOUBAN ");
            //selectQuery.Append(" ,DW.DW_DECISION AS DECISION");
            //selectQuery.Append(" ,DW.DRAWING_CD");
            //selectQuery.Append(" ,DW.CREATED_DATE ");
            //selectQuery.Append(" ,CONCAT (DW.DW_TABA_NO_1, CONCAT (CONCAT ( ");
            //selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', DW.DW_TABA_NO_2) ELSE NULL END, ");
            //selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', DW.DW_TABA_NO_3) ELSE NULL END ), ");
            //selectQuery.Append(" CASE WHEN DW.DW_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', DW.DW_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            //selectQuery.Append(" ,DECODE(DW.DW_DECISION,0,1,2,3) AS PROCESS_STATUS ");

            //selectQuery.Append(" FROM TRN_DRAWING DW INNER JOIN TRN_PROCESS_PLAN PP ");
            //selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            //selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            //selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            //selectQuery.Append(" AND DW.DW_DECISION IS NOT NULL");
            //selectQuery.Append(" AND DW.UPDATED_DATE IN ( ");
            //selectQuery.Append(" SELECT  MAX(DW.UPDATED_DATE) FROM ( ");
            //selectQuery.Append(" SELECT  DW.UPDATED_DATE , DW.DW_TABA_NO_1 AS TABA_NO_INT ");

            //selectQuery.Append(" FROM TRN_DRAWING DW INNER JOIN TRN_PROCESS_PLAN PP ");
            //selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            //selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            //selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");

            //selectQuery.Append(" ORDER BY DW_TABA_NO_1 ) GROUP BY DW_TABA_NO_1  ) ");
            //selectQuery.Append(" ORDER BY DW.DRAWING_CD ");
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;

        }

        public HikinukiDetails GetHikinukiDetailsByChuuban(string chuuban, string gyouban, string mechineCD)
        {

            int rowsCount = 0;

            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(" SELECT ");
            sQuery.Append(" COUNT(PP.PROCESS_PLAN_CD) ");
            sQuery.Append("  FROM TRN_DRAWING  DW ");
            sQuery.Append(" LEFT  JOIN  TRN_PROCESS_PLAN PP  ON ");
            sQuery.Append(" DW.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD ");
            sQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
            sQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
            sQuery.Append(" AND DW.MACHINE_CD = '" + mechineCD + "' ");
            sQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            sQuery.Append(" AND DW.CREATED_DATE >= TO_DATE(SYSDATE)-" + days);
            rowsCount = Convert.ToInt32(ExecuteScalar(sQuery.ToString()));


            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");

            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS DIM_TOL_MAX_CHR");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            //selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");

            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");

            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");    //COIL_QTY
                                                                              
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");
            
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            // selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");     // AND PR.VER = PP.PROD_VER   2019-10-02 Pradeep M.
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
         //   selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            //selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetProdUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");    //GetMatlUnitForDBQuery
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE  ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_WINDING) AS PP_COIL_DIA_WINDING ");
            selectQuery.Append("  ,PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS  ");           //PP_KAKOUIRAIREMARKS
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            selectQuery.Append(" ,DW.DRAWING_CD AS DW_DRAWING_CD ");
            selectQuery.Append(" ,DW.CREATED_DATE  AS DW_CREATED_DATE ");
            selectQuery.Append(" ,DW.UPDATED_DATE  AS DW_UPDATED_DATE ");
            selectQuery.Append(" ,(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD = DW.MACHINE_CD) AS DW_MACHINE_NAME ");
            selectQuery.Append(" ,DW.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,DW.DW_START_DATE AS DW_START_DATE  ");
            selectQuery.Append(",TO_CHAR( DW.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(" ,DW.DW_START_OPERATOR_CD AS DW_START_OPERATOR ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME AS SHORT_NAME FROM MST_EMPLOYEE em  WHERE EM.EMPLOYEE_CD = dw_start_operator_cd) AS DW_START_OPERATOR_NAME  ");
            selectQuery.Append("  ,DW.TP_TEST AS DW_TP_TEST ");
            //  selectQuery.Append(" ," + Utils.GetTPText("DW.TP_TEST ") + " AS DW_TP_TEST_DESC ");
            selectQuery.Append(" ,DW.DW_NO_OF_COILS as DW_NO_OF_COILS ");
            selectQuery.Append(" ,DW.DW_QTY  as DW_QTY ");
            selectQuery.Append(" ,DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,DW.FURNACE_NO AS DW_FURNACE_NO ");
            selectQuery.Append(" ,DW.DIE_DIM AS DW_DIE_DIM ");
            selectQuery.Append(" , DW.DIE_NO AS DW_DIE_NO ");
            selectQuery.Append(", DW.DIE_SHAPE  AS DW_DIE_SHAPE");
            selectQuery.Append(", DW.REDRAWING  AS DW_REDRAWING"); 

            selectQuery.Append(" ,DW.DW_DIM1 AS DW_DIM1 ");
            selectQuery.Append(" ,DW.DW_DIM2 AS DW_DIM2 ");
            selectQuery.Append(" ,DW.UPPER_LOWER_ROLLER_ANGLE1 AS DW_UPPER_LOWER_ROLLER_ANGLE1 ");
            selectQuery.Append(" ,DW.UPPER_LOWER_ROLLER_ANGLE2  AS DW_UPPER_LOWER_ROLLER_ANGLE2 ");
            selectQuery.Append(" ,DW.UPPER_ROLLER_ELEVATION  AS DW_UPPER_ROLLER_ELEVATION ");
            selectQuery.Append(" ,DW.DW_TONNAGE AS DW_TONNAGE ");
            selectQuery.Append(" ,DW.HEX_R  AS DW_HEX_R ");
            selectQuery.Append(" ,DW.DW_END_DATE AS DW_END_DATE ");
            selectQuery.Append(",TO_CHAR( DW.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(" ,DW.DW_END_OPERATOR_CD AS DW_END_OPERATOR ");
            selectQuery.Append(" , (SELECT EM.SHORT_NAME  FROM MST_EMPLOYEE em WHERE EM.EMPLOYEE_CD = dw_end_operator_CD) AS DW_END_OPERATOR_NAME  ");
            selectQuery.Append("  ,DW.DW_DECISION AS DW_DECISION ");

            // selectQuery.Append("," + Utils.GetProcessDecisionKubunStringForSQL("DW.DW_DECISION") + " AS  DW_DECISION_STRING ");

            selectQuery.Append(" ,DW.DW_ACT_DIM_CHK AS DW_ACT_DIM_CHK ");
            selectQuery.Append(" ,DW.DW_ACT_DIM1 AS DW_ACT_DIM1  ");
            selectQuery.Append(" ,DW.DW_ACT_DIM2  AS DW_ACT_DIM2  ");
            selectQuery.Append(" ,DW.DW_ACT_DIM3 AS DW_ACT_DIM3  ");
            selectQuery.Append(" ,DW.DW_ACT_DIM4 AS DW_ACT_DIM4 ");
            selectQuery.Append(" ,DW.DW_ACT_DIM5 AS DW_ACT_DIM5  ");
            selectQuery.Append(" ,DW.DW_ACT_DIM6 AS DW_ACT_DIM6  ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH_CHK AS DW_ACT_LENGTH_CHK   ");
            selectQuery.Append(" ,DW.DW_FINISHED_QTY_CHK AS DW_FINISHED_QTY_CHK  ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1 ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2  ");
            selectQuery.Append(" ,DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3  ");
            selectQuery.Append(" ,DW.DW_FINISHED_BARS AS DW_FINISHED_BARS  ");
            selectQuery.Append(" ,DW.DW_FINISHED_QTY AS DW_FINISHED_QTY  ");
            selectQuery.Append(", (SELECT DW.UNIT_TYPE FROM TRN_DRAWING DW INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE  PP.PROCESS_PLAN_CMPL_DATE IS NULL  AND PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND DW.MACHINE_CD = '" + mechineCD + "' ");
            selectQuery.Append(" AND DW.UNIT_TYPE IS NOT NULL AND DW.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " ORDER BY  DW.DRAWING_CD DESC FETCH FIRST ROW ONLY)  AS DW_UNIT_TYPE ");
            
            selectQuery.Append(" ,DW.DW_NG_BARS1 AS DW_NG_BARS1 ");
            selectQuery.Append(" ,DW.DW_NG_BARS2 AS DW_NG_BARS2 ");
            selectQuery.Append(" ,DW.DW_NG_BARS3 AS DW_NG_BARS3 ");
            selectQuery.Append(" ,DW.DW_NG_DETAILS1 AS DW_NG_DETAILS1 ");
            selectQuery.Append(" ,DW.DW_NG_DETAILS2 AS DW_NG_DETAILS2 ");
            selectQuery.Append(" ,DW.DW_NG_DETAILS3 AS DW_NG_DETAILS3 ");
            selectQuery.Append(" ,DW.A_SIDE_FURE1 AS DW_A_SIDE_FURE1 ");
            selectQuery.Append(" ,DW.A_SIDE_FURE2 AS DW_A_SIDE_FURE2 ");
            selectQuery.Append(" ,DW.A_SIDE_FURE3 AS DW_A_SIDE_FURE3 ");
            selectQuery.Append(" ,DW.DW_BEND_STATUS AS DW_BEND_STATUS  ");  //Decode
            selectQuery.Append(" ,DW.B_SIDE_FURE1 AS DW_B_SIDE_FURE1 ");
            selectQuery.Append(" ,DW.B_SIDE_FURE2 AS DW_B_SIDE_FURE2 ");
            selectQuery.Append(" ,DW.B_SIDE_FURE3 AS DW_B_SIDE_FURE3 ");
            selectQuery.Append(" ,DW.DW_EXT_TEXTURE_STATUS AS  DW_EXT_TEXTURE_STATUS ");  //--DECODE
            selectQuery.Append(" ,DW.DW_TABA_NO_1 as DW_TABA_NO_1 ");
            selectQuery.Append(" ,DW.DW_TABA_NO_2 as DW_TABA_NO_2 ");
            selectQuery.Append(" ,DW.DW_TABA_NO_3 as DW_TABA_NO_3 ");
            selectQuery.Append(" ,DW.DW_TABA_NO_4 as DW_TABA_NO_4 ");
            selectQuery.Append(" ,DW.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL ");
            selectQuery.Append(" ,DW.SHIKII_VALUE  AS DW_SHIKII_VALUE  ");
            selectQuery.Append(" ,DW.MSG_CD AS DW_MSG_CD ");
            selectQuery.Append(" ,M.MSG_CONTENTS AS DW_REMARKS ");
            selectQuery.Append(" ,DW.CARBON_PERCENTAGE AS DW_CARBON_PERCENTAGE ");
            selectQuery.Append(" ,DW.MAGNIZ_PERCENTAGE  AS DW_MAGNIZ_PERCENTAGE ");
            selectQuery.Append(" ,DW.DW_DISPOSAL_NO AS DW_DW_DISPOSAL_NO ");

            selectQuery.Append(" ,DW.EC_DONE_CHK AS DW_EC_DONE_CHK ");
            selectQuery.Append(" ,DW.EC_COIL_SHAPE AS DW_EC_COIL_SHAPE ");
            selectQuery.Append(" ,DW.EC_IS_OK AS DW_EC_IS_OK ");
            selectQuery.Append(" ,DW.MAGNETISM AS DW_MAGNETISM ");
            selectQuery.Append(" ,DW.EC_COIL_SIZE AS DW_EC_COIL_SIZE");
            selectQuery.Append(" ,DW.USER_ID AS DW_USER_ID,");
            selectQuery.Append("  DW.DW_TABA_NO_1,");
            selectQuery.Append("  DW.DW_TABA_NO_2,");
            selectQuery.Append("  DW.DW_TABA_NO_3,");
            selectQuery.Append("  DW.DW_TABA_NO_4, ");

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
            selectQuery.Append("PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE, ");
            selectQuery.Append("COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION, ");
            if (rowsCount == 0)
            {
                selectQuery.Append("'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append("'YES' AS PAST_DATA_EXIST ");
            }

            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING  DW ON");
            selectQuery.Append(" DW.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            ////selectQuery.Append(" LEFT JOIN TRN_NIFUDA N ON PP.PROCESS_PLAN_CD = N.PROCESS_PLAN_CD");
            ////selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = N.NIFUDA_CD");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append("  ON M.MSG_CD = DW.MSG_CD  ");
            //selectQuery.Append("  LEFT JOIN TRN_KAKOUIRAI K  ");
            //selectQuery.Append("  ON PP.KAKOUIRAI_CD   = K.KAKOUIRAI_CD  ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
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
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            if (rowsCount> 0)
            {
                selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
                selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
                selectQuery.Append(" AND DW.MACHINE_CD = '" + mechineCD + "' ");
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
                selectQuery.Append(" AND DW.CREATED_DATE >= TO_DATE(SYSDATE)-" + days);
            }else
            {
                selectQuery.Append(" WHERE PP.CHUUBAN= '" + chuuban + "'");
                selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            }


                selectQuery.Append(" ORDER BY DW.DRAWING_CD DESC");
            return ExecuteQuerySingleResult<HikinukiDetails>(selectQuery.ToString());
        }

        public HikinukiDetails GetHikinukiHistoryDetailsByChuuban(string chubaan, short? gyouban)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUST.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS DIM_TOL_MAX_CHR");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");

            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN ");

            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");    //COIL_QTY

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

            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE  ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_WINDING) AS PP_COIL_DIA_WINDING ");
            selectQuery.Append("  ,PP.KAKOUIRAI_REMARK AS  PP_KAKOUIRAI_REMARKS  ");           //PP_KAKOUIRAIREMARKS
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");

            selectQuery.Append(" , P1.PROCESS_NAME AS PP_PROCESS_1, ");
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
            selectQuery.Append(" DW.DRAWING_CD AS DW_DRAWING_CD ");
            selectQuery.Append(", PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(", COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING  DW ON");
            selectQuery.Append(" DW.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ON PCK.PACKING_CD = PP.PACKING_CD ");
            
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
            selectQuery.Append(" WHERE PP.CHUUBAN='" + chubaan + "' AND PP.GYOUBAN = '" + gyouban + "'");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" ORDER BY DW.DRAWING_CD DESC");
            return ExecuteQuerySingleResult<HikinukiDetails>(selectQuery.ToString());
        }

        public List<HikinukiDetails> GetProcessHistoryList(string chuubanNo, short? gyoubanNo, string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(DR.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DR.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" DR.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS UNIT_TYPE,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" DR.DIE_DIM AS DW_DIE_DIM, ");

            selectQuery.Append(" CONCAT(DW_TABA_NO_1, ");
            selectQuery.Append("CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_1 IS NULL AND DW_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('',DW_TABA_NO_2) ");
            selectQuery.Append(" WHEN DW_TABA_NO_2 IS NOT NULL THEN ");

            selectQuery.Append(" CONCAT(',', DW_TABA_NO_2) ");

            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");

            selectQuery.Append(" END ");
            selectQuery.Append(") AS DW_TABA_NO_1_2, ");

            selectQuery.Append(" TO_CHAR(DR.DW_ACT_DIM1, 'FM990.000') AS DW_ACT_DIM1_STRING, ");
            selectQuery.Append(" TO_CHAR(DR.DW_ACT_DIM2, 'FM990.000') AS DW_ACT_DIM2_STRING, ");
            selectQuery.Append(" TO_CHAR(DR.DW_ACT_DIM3, 'FM990.000') AS DW_ACT_DIM3_STRING, ");
            selectQuery.Append(" TO_CHAR(DR.DW_ACT_DIM4, 'FM990.000') AS DW_ACT_DIM4_STRING, ");
            selectQuery.Append(" TO_CHAR(DR.DW_ACT_DIM5, 'FM990.000') AS DW_ACT_DIM5_STRING, ");
            selectQuery.Append(" TO_CHAR(DR.DW_ACT_DIM6, 'FM990.000') AS DW_ACT_DIM6_STRING, ");

            selectQuery.Append(" DR.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1, ");
            selectQuery.Append(" DR.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2, ");
            selectQuery.Append(" DR.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3, ");

            selectQuery.Append(" DR.DW_FINISHED_BARS AS DW_FINISHED_BARS, ");
            selectQuery.Append(" DR.UNIT_TYPE AS DW_UNIT_TYPE, ");
            selectQuery.Append(" DR.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");
            selectQuery.Append(" DR.MAGNETISM AS DW_MAGNETISM, ");

            selectQuery.Append(" DR.DW_BEND_STATUS, ");
            selectQuery.Append(" DR.DW_EXT_TEXTURE_STATUS, ");
            selectQuery.Append(" M.MSG_CONTENTS AS DW_REMARKS, ");

            selectQuery.Append(" CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" DW_TABA_NO_3 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_3 IS NULL AND DW_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('' ,DW_TABA_NO_4) ");
            selectQuery.Append(" WHEN DW_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', DW_TABA_NO_4) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DW_TABA_NO_3_4, ");
            selectQuery.Append(" DR.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME AS PP_INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" DR.SHIKII_VALUE AS DW_SHIKII_VALUE, ");
            selectQuery.Append(" DR.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL, ");
            selectQuery.Append(" DR.DW_NG_BARS1 ,");
            selectQuery.Append(" DR.DW_NG_BARS2,");
            selectQuery.Append(" DR.DW_NG_BARS3");
           
            selectQuery.Append(", CASE WHEN PP.PROD_SPEC_NO1 IS NOT NULL AND PP.PROD_SPEC_NO2 IS NOT NULL THEN ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 || '-' || PP.PROD_SPEC_NO2 ");
            selectQuery.Append(" WHEN PP.PROD_SPEC_NO1 IS NOT NULL THEN PP.PROD_SPEC_NO1 ");
            selectQuery.Append(" ELSE  PP.PROD_SPEC_NO2 END AS PP_PROD_SPEC_NO ");





            if (cultureName =="en")
            {
                selectQuery.Append(", PROCK.KUBUN_NAME_EN AS NEXT_PROCESS_NAME "); 
                selectQuery.Append(", (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = DR.DW_NG_DETAILS1 ) AS DW_NG_DETAILS1_DECODE");
                selectQuery.Append(", (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = DR.DW_NG_DETAILS2 ) AS DW_NG_DETAILS2_DECODE");
                selectQuery.Append(", (SELECT OTHER_NAME FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = DR.DW_NG_DETAILS3 ) AS DW_NG_DETAILS3_DECODE");
            }
            else
            {
                selectQuery.Append(", PROCK.KUBUN_NAME AS NEXT_PROCESS_NAME ");

                selectQuery.Append(", (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = DR.DW_NG_DETAILS1) AS DW_NG_DETAILS1_DECODE");
                selectQuery.Append(", (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = DR.DW_NG_DETAILS2) AS DW_NG_DETAILS2_DECODE");
                selectQuery.Append(", (SELECT PROC_DEFECT_LONG FROM  MST_PROCESS_DEFECT PD WHERE PD.PROCESS_DEFECT_CD = DR.DW_NG_DETAILS3) AS DW_NG_DETAILS3_DECODE");
            }
           

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP JOIN TRN_DRAWING DR");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DR.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DR.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DR.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DR.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DR.DW_NEXT_PROCESS");

            selectQuery.Append(" WHERE  CHUUBAN = '" + chuubanNo + "' AND GYOUBAN ='" + gyoubanNo + "'");
            selectQuery.Append(" ORDER BY DR.DRAWING_CD DESC ");
            //selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            //selectQuery.Append(" ORDER BY END_DATE_TIME DESC) tempsql");
            //selectQuery.Append(" WHERE ROWNUM <= 10 ");
            return ExecuteQueryMultiResult<HikinukiDetails>(selectQuery.ToString());
        }

        public List<HikinukiDetails> GetHikinuki24MachineList(string machineCd)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append("DW.MACHINE_CD AS DW_MACHINE_CD,  ");
            selectQuery.Append(" DW.DW_DECISION AS DW_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");

            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append(" DW.DW_METALLIC_PLATE_CODE  AS DW_METALLIC_PLATE_CODE,	");
            // selectQuery.Append(" PP.MATL_DIM_D || MATL_SHAPE AS PP_MATL_DIM_D_SHAPE,	");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,	");
            selectQuery.Append(" PP.PROD_DIM_D || PP.PROD_SHAPE  AS  PP_PROD_DIM_D_STRING,	");
            selectQuery.Append("  DW.DIE_DIM AS DW_DIE_DIM,	");
            selectQuery.Append("  DW.DIE_SHAPE AS DW_DIE_SHAPE,	");
            selectQuery.Append("  DW.UNIT_TYPE,	");
            selectQuery.Append(" CONCAT (DW_TABA_NO_1, CASE WHEN DW_TABA_NO_2 IS NOT NULL THEN CONCAT ('-', DW_TABA_NO_2) ELSE NULL END) AS DW_TABA_NO_1_2,");
            selectQuery.Append("  DW.DW_ACT_DIM1 AS DW_ACT_DIM1,	");
            selectQuery.Append("  DW.DW_ACT_DIM2 AS DW_ACT_DIM2,	");
            selectQuery.Append("  DW.DW_ACT_DIM3 AS DW_ACT_DIM3,	");
            selectQuery.Append("  DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1,	");
            selectQuery.Append("  DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2,	");
            selectQuery.Append("  DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3,	");

            selectQuery.Append("  DW.DW_FINISHED_BARS AS DW_FINISHED_BARS,	");
            selectQuery.Append("  DW.DW_FINISHED_QTY AS DW_FINISHED_QTY,	");

            selectQuery.Append(" DW.DW_BEND_STATUS, ");
            selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS, ");

            selectQuery.Append("  M.MSG_CONTENTS AS DW_REMARKS, ");
            selectQuery.Append("  DW.MSG_CD AS DW_MSG_CD, ");
            selectQuery.Append("  PP.PROD_SPEC_NO as PROD_SPEC_NO,	");
            selectQuery.Append("  PP.CHARGE AS PP_CHARGE,	");
            selectQuery.Append("  PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append("  NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" CONCAT(CASE WHEN DW_TABA_NO_3 IS NOT NULL THEN DW_TABA_NO_3 ELSE NULL END,");
            selectQuery.Append(" CASE WHEN DW_TABA_NO_4 IS NOT NULL THEN CONCAT ('-', DW_TABA_NO_4) ELSE NULL END ) AS DW_TABA_NO_2_3,");
            selectQuery.Append("  DW.DW_ACT_DIM4 AS DW_ACT_DIM4,	");
            selectQuery.Append("  DW.DW_ACT_DIM5 AS DW_ACT_DIM5,	");
            selectQuery.Append("  DW.DW_ACT_DIM6 AS DW_ACT_DIM6,	");
            selectQuery.Append("  DW.MAGNETISM AS DW_MAGNETISM,	");
            selectQuery.Append("  DW.SHIKII_VALUE AS DW_SHIKII_VALUE,	");
            selectQuery.Append("  DW.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL,	");
            selectQuery.Append(" NVL (DW.DW_END_DATE, DW.DW_START_DATE) AS ORDER_CLAUSE ");

            selectQuery.Append(" FROM TRN_DRAWING DW");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ");
            selectQuery.Append(" ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD ");

            selectQuery.Append(" WHERE  DW.MACHINE_CD  = '" + machineCd + "' AND NVL (DW.DW_END_DATE, DW.DW_START_DATE) > SYSDATE - 1");
            selectQuery.Append(" ORDER BY ORDER_CLAUSE DESC");
            return ExecuteQueryMultiResult<HikinukiDetails>(selectQuery.ToString());
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
        public HikinukiDetails InsertHikinukiDetails(HikinukiDetails hikinukiDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_DRAWING(");
            insertQuery.Append(" DRAWING_CD,");
            insertQuery.Append(" PROCESS_PLAN_CD,");
            insertQuery.Append(" MACHINE_CD,");
            insertQuery.Append(" DW_START_DATE,");
            insertQuery.Append(" DW_START_OPERATOR_CD,");
            insertQuery.Append(" DW_NO_OF_COILS,");
            insertQuery.Append(" DW_QTY,");
            insertQuery.Append(" DW_METALLIC_PLATE_CODE,");
            insertQuery.Append(" DIE_DIM,");
            insertQuery.Append(" DIE_NO,");
            insertQuery.Append(" DW_DIM1,");
            insertQuery.Append(" UPPER_LOWER_ROLLER_ANGLE1,");
            insertQuery.Append(" UPPER_LOWER_ROLLER_ANGLE2,");
            insertQuery.Append(" UPPER_ROLLER_ELEVATION,");
            insertQuery.Append(" DW_TONNAGE,");
            insertQuery.Append(" HEX_R,");
            insertQuery.Append(" CREATED_DATE,");
            insertQuery.Append(" UPDATED_DATE,");
            insertQuery.Append(" FURNACE_NO,");
            insertQuery.Append(" DW_DIM2,");
            insertQuery.Append(" TP_TEST,");
            insertQuery.Append(" CARBON_PERCENTAGE,");
            insertQuery.Append(" MAGNIZ_PERCENTAGE,");
            insertQuery.Append(" EC_DONE_CHK,");
            insertQuery.Append(" SHIKII_VALUE,");
            insertQuery.Append(" EC_COIL_SIZE,");
            insertQuery.Append(" EC_COIL_SHAPE,");
            insertQuery.Append(" EC_IS_OK,");
            insertQuery.Append(" DIE_SHAPE, ");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" LOCATION_CD, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");
            insertQuery.Append(" REDRAWING, ");
            insertQuery.Append(" PROCESS_KUBUN_CD, ");
            insertQuery.Append(" DW_TABA_NO_1, ");
            insertQuery.Append(" DW_TABA_NO_2, ");
            insertQuery.Append(" DW_TABA_NO_3, ");
            insertQuery.Append(" DW_TABA_NO_4 ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("TRN_DRAWING_SEQ.NEXTVAL");
            insertQuery.Append(",'" + hikinukiDetails.PP_PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_MACHINE_CD + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(hikinukiDetails.DW_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + hikinukiDetails.DW_START_OPERATOR + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_NO_OF_COILS + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_QTY + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_METALLIC_PLATE_CODE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_DIE_DIM + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_DIE_NO + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_DIM1 + "'");     //
            insertQuery.Append(",'" + hikinukiDetails.DW_UPPER_LOWER_ROLLER_ANGLE1 + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_UPPER_LOWER_ROLLER_ANGLE2 + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_UPPER_ROLLER_ELEVATION + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_TONNAGE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_HEX_R + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + hikinukiDetails.DW_FURNACE_NO + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_DIM2 + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_TP_TEST + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_CARBON_PERCENTAGE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_MAGNIZ_PERCENTAGE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_EC_DONE_CHK + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_SHIKII_VALUE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_EC_COIL_SIZE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_EC_COIL_SHAPE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_EC_IS_OK + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_DIE_SHAPE + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_USER_ID + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_LOCATION_CD + "'");
            insertQuery.Append(",'" + hikinukiDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_REDRAWING + "'");
            insertQuery.Append(",'" + hikinukiDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_TABA_NO_1 + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_TABA_NO_2 + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_TABA_NO_3 + "'");
            insertQuery.Append(",'" + hikinukiDetails.DW_TABA_NO_4 + "'");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            return GetHikinukiDetailsByChuuban(hikinukiDetails.PP_CHUUBAN, Convert.ToString(hikinukiDetails.PP_GYOUBAN), hikinukiDetails.DW_MACHINE_CD.ToString());
        }

        public long UpdateHikinukiDetails(HikinukiDetails hikinukiDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_DRAWING SET ");


            // Start data is updated

            updateQuery.Append(" DW_START_DATE = ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(hikinukiDetails.DW_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateQuery.Append(" MACHINE_CD = '" + hikinukiDetails.DW_MACHINE_CD + "',"); 
            updateQuery.Append(" DW_START_OPERATOR_CD = '" + hikinukiDetails.DW_START_OPERATOR + "',");
            updateQuery.Append(" TP_TEST = '" + hikinukiDetails.DW_TP_TEST + "',");
            updateQuery.Append(" FURNACE_NO = '" + hikinukiDetails.DW_FURNACE_NO + "',");
            updateQuery.Append(" DW_NO_OF_COILS = '" + hikinukiDetails.DW_NO_OF_COILS + "',");
            updateQuery.Append(" DW_QTY = '" + hikinukiDetails.DW_QTY + "',");
            updateQuery.Append(" DW_METALLIC_PLATE_CODE = '" + hikinukiDetails.DW_METALLIC_PLATE_CODE + "',");
            updateQuery.Append(" CARBON_PERCENTAGE = '" + hikinukiDetails.DW_CARBON_PERCENTAGE + "',");
            updateQuery.Append(" MAGNIZ_PERCENTAGE = '" + hikinukiDetails.DW_MAGNIZ_PERCENTAGE + "',");
            updateQuery.Append(" DIE_DIM = '" + hikinukiDetails.DW_DIE_DIM + "',");
            updateQuery.Append(" DIE_SHAPE = '" + hikinukiDetails.DW_DIE_SHAPE + "',");
            updateQuery.Append(" DIE_NO = '" + hikinukiDetails.DW_DIE_NO + "',");
            updateQuery.Append(" DW_DIM1 = '" + hikinukiDetails.DW_DIM1 + "',");
            updateQuery.Append(" DW_DIM2 = '" + hikinukiDetails.DW_DIM2 + "',");
            updateQuery.Append(" UPPER_LOWER_ROLLER_ANGLE1 = '" + hikinukiDetails.DW_UPPER_LOWER_ROLLER_ANGLE1 + "',");
            updateQuery.Append(" UPPER_LOWER_ROLLER_ANGLE2 = '" + hikinukiDetails.DW_UPPER_LOWER_ROLLER_ANGLE2 + "',");
            updateQuery.Append(" UPPER_ROLLER_ELEVATION = '" + hikinukiDetails.DW_UPPER_ROLLER_ELEVATION + "',");
            updateQuery.Append(" DW_TONNAGE = '" + hikinukiDetails.DW_TONNAGE + "',");
            updateQuery.Append(" HEX_R = '" + hikinukiDetails.DW_HEX_R + "',");
            updateQuery.Append(" EC_DONE_CHK = '" + hikinukiDetails.DW_EC_DONE_CHK + "',");
            updateQuery.Append(" SHIKII_VALUE = '" + hikinukiDetails.DW_SHIKII_VALUE + "',");
            updateQuery.Append(" EC_COIL_SIZE = '" + hikinukiDetails.DW_EC_COIL_SIZE + "',");
            updateQuery.Append(" EC_COIL_SHAPE = '" + hikinukiDetails.DW_EC_COIL_SHAPE + "',");
            updateQuery.Append(" EC_IS_OK = '" + hikinukiDetails.DW_EC_IS_OK + "',");
            updateQuery.Append(" REDRAWING = '" + hikinukiDetails.DW_REDRAWING + "',"); 

            //END DATA

            updateQuery.Append(" DW_END_OPERATOR_CD ='" + hikinukiDetails.DW_END_OPERATOR + "',");
            updateQuery.Append(" DW_END_DATE= ");
            updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(hikinukiDetails.DW_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'), ");
            updateQuery.Append(" DW_DECISION  = '" + hikinukiDetails.DW_DECISION + "',");
            updateQuery.Append(" DW_ACT_DIM_CHK='" + hikinukiDetails.DW_ACT_DIM_CHK + "',");
            updateQuery.Append(" DW_ACT_DIM1 ='" + hikinukiDetails.DW_ACT_DIM1 + "',");
            updateQuery.Append(" DW_ACT_DIM2  = '" + hikinukiDetails.DW_ACT_DIM2 + "',");
            updateQuery.Append(" DW_ACT_DIM3='" + hikinukiDetails.DW_ACT_DIM3 + "',");
            updateQuery.Append(" DW_ACT_DIM4 ='" + hikinukiDetails.DW_ACT_DIM4 + "',");
            updateQuery.Append(" DW_ACT_DIM5  = '" + hikinukiDetails.DW_ACT_DIM5 + "',");
            updateQuery.Append(" DW_ACT_DIM6= '" + hikinukiDetails.DW_ACT_DIM6 + "',");
            updateQuery.Append(" DW_ACT_LENGTH_CHK= '" + hikinukiDetails.DW_ACT_LENGTH_CHK + "',");
            updateQuery.Append(" DW_ACT_LENGTH1 = '" + hikinukiDetails.DW_ACT_LENGTH1 + "',");
            updateQuery.Append(" DW_ACT_LENGTH2  = '" + hikinukiDetails.DW_ACT_LENGTH2 + "',");
            updateQuery.Append(" DW_ACT_LENGTH3= '" + hikinukiDetails.DW_ACT_LENGTH3 + "',");
            updateQuery.Append(" DW_FINISHED_BARS = '" + hikinukiDetails.DW_FINISHED_BARS + "',");
            updateQuery.Append(" UNIT_TYPE  = '" + hikinukiDetails.DW_UNIT_TYPE + "',");
            updateQuery.Append(" DW_FINISHED_QTY= '" + hikinukiDetails.DW_FINISHED_QTY + "',");
            updateQuery.Append(" DW_FINISHED_QTY_CHK  = '" + hikinukiDetails.DW_FINISHED_QTY_CHK + "',");
            updateQuery.Append(" DW_CORRECTED_BARS= '" + hikinukiDetails.DW_CORRECTED_BARS + "',");
            updateQuery.Append(" DW_NG_BARS1 = '" + hikinukiDetails.DW_NG_BARS1 + "',");
            updateQuery.Append(" DW_NG_BARS2  = '" + hikinukiDetails.DW_NG_BARS2 + "',");
            updateQuery.Append(" DW_NG_BARS3 = '" + hikinukiDetails.DW_NG_BARS3 + "',");
            updateQuery.Append(" DW_NG_DETAILS1 = '" + hikinukiDetails.DW_NG_DETAILS1 + "',");
            updateQuery.Append(" DW_NG_DETAILS2  = '" + hikinukiDetails.DW_NG_DETAILS2 + "',");
            updateQuery.Append(" DW_NG_DETAILS3 = '" + hikinukiDetails.DW_NG_DETAILS3 + "',");

            updateQuery.Append(" A_SIDE_FURE1 = '" + hikinukiDetails.DW_A_SIDE_FURE1 + "',");
            updateQuery.Append(" A_SIDE_FURE2  = '" + hikinukiDetails.DW_A_SIDE_FURE2 + "',");
            updateQuery.Append(" A_SIDE_FURE3 = '" + hikinukiDetails.DW_A_SIDE_FURE3 + "',");

            updateQuery.Append(" B_SIDE_FURE1 = '" + hikinukiDetails.DW_B_SIDE_FURE1 + "',");
            updateQuery.Append(" B_SIDE_FURE2  = '" + hikinukiDetails.DW_B_SIDE_FURE2 + "',");
            updateQuery.Append(" B_SIDE_FURE3 = '" + hikinukiDetails.DW_B_SIDE_FURE3 + "',");

            updateQuery.Append(" DW_TABA_NO_1 = '" + hikinukiDetails.DW_TABA_NO_1 + "',");
            updateQuery.Append(" DW_TABA_NO_2  = '" + hikinukiDetails.DW_TABA_NO_2 + "',");
            updateQuery.Append(" DW_TABA_NO_3 = '" + hikinukiDetails.DW_TABA_NO_3 + "',");
            updateQuery.Append(" DW_TABA_NO_4 = '" + hikinukiDetails.DW_TABA_NO_4 + "',");

            updateQuery.Append(" DW_BEND_STATUS = '" + hikinukiDetails.DW_BEND_STATUS + "',");
            updateQuery.Append(" DW_EXT_TEXTURE_STATUS  = '" + hikinukiDetails.DW_EXT_TEXTURE_STATUS + "',");
            updateQuery.Append(" DW_DISPOSAL_NO = '" + hikinukiDetails.DW_DISPOSAL_NO + "',");

            updateQuery.Append(" EC_NG_BAR_TOTAL ='" + hikinukiDetails.DW_EC_NG_BAR_TOTAL + "',");
            updateQuery.Append(" MAGNETISM  = '" + hikinukiDetails.DW_MAGNETISM + "',");
            updateQuery.Append(" MSG_CD ='" + hikinukiDetails.DW_MSG_CD + "',");
            updateQuery.Append(" DW_NEXT_PROCESS ='" + hikinukiDetails.DW_NEXT_PROCESS + "',"); 
            updateQuery.Append(" USER_ID ='" + hikinukiDetails.DW_USER_ID + "',");
            updateQuery.Append(" LOCATION_CD ='" + hikinukiDetails.DW_LOCATION_CD + "',");
            updateQuery.Append(" INPUT_LOCATION_CD ='" + (hikinukiDetails.INPUT_LOCATION_CD != null ? hikinukiDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "',");
            updateQuery.Append(" UPDATED_DATE = SYSDATE");

            updateQuery.Append(" WHERE ");
            updateQuery.Append(" DRAWING_CD= '" + hikinukiDetails.DW_DRAWING_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());
            long msgCD = Convert.ToInt64(ExecuteScalar("SELECT NVL(MSG_CD,0) FROM TRN_DRAWING WHERE DRAWING_CD= '" + hikinukiDetails.DW_DRAWING_CD + "'"));

            return msgCD;
        }

       
        public List<HikinukiDetails> GetHikinukiList(List<SearchField> searchCriteria, string sortCiteria, short locationCD)
        {
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" DR.DRAWING_CD AS DW_DRAWING_CD, "); 
            selectQuery.Append(" CONCAT(DR.DW_TABA_NO_1, CONCAT(CONCAT( ");
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
            selectQuery.Append(" )) AS DW_TABA_NO_1_2_3_4, ");
            selectQuery.Append("(SELECT MACHINE_NAME FROM MST_MACHINE  WHERE MACHINE_CD = DR.MACHINE_CD ) AS DW_MACHINE_NAME, ");
                        
            selectQuery.Append("TO_CHAR(DR.DW_END_DATE, 'yyyy/mm/dd HH24:mi') AS DW_END_DATE_STRING, ");
            selectQuery.Append("CST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append("PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            //selectQuery.Append("TO_CHAR(PP.PROD_DIM_D) AS PP_PROD_DIM_D_STRING, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, "); 
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            

            selectQuery.Append("PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D, PP.MATL_DIM_W, MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING, ");
            selectQuery.Append("PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append("DR.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE, ");
            selectQuery.Append("DR.DW_FINISHED_BARS, ");
            selectQuery.Append("DR.UNIT_TYPE, ");
            selectQuery.Append("DR.DW_FINISHED_QTY, ");



            //selectQuery.Append("ec.EC_OK_QTY as EC_OK_QTY,");
            selectQuery.Append("EMP.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            selectQuery.Append("DR.DW_DECISION ");
            selectQuery.Append("FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("INNER JOIN TRN_DRAWING DR ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD ");
            selectQuery.Append("INNER JOIN MST_EMPLOYEE EMP ON DR.DW_END_OPERATOR_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append("LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            selectQuery.Append("LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append("WHERE DR.INPUT_LOCATION_CD = " + locationCD);
            selectQuery.Append(" AND DR.DW_END_DATE IS NOT NULL");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
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
                query += " ORDER BY PP_CHUUBAN_GYOUBAN ASC, DR.DW_TABA_NO_1 ASC";
            }
            return ExecuteQueryMultiResult<HikinukiDetails>(query);
        }

        public HikinukiDetails GetHikinukiDetailsDrawingByCD(long drawingCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("   SELECT	DW.DRAWING_CD	AS	DW_DRAWING_CD  ");
            selectQuery.Append(",  DW.PROCESS_PLAN_CD	AS	PP_PROCESS_PLAN_CD  ");
            selectQuery.Append(",  DW.MACHINE_CD	AS	DW_MACHINE_CD  ");
            selectQuery.Append(",  (SELECT MACHINE_NAME FROM MST_MACHINE WHERE MACHINE_CD = DW.MACHINE_CD) AS DW_MACHINE_NAME ");
            selectQuery.Append(",  DW.DW_START_DATE	AS	DW_START_DATE  ");
            selectQuery.Append(",  TO_CHAR( DW.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(",  DW.DW_START_OPERATOR_CD	AS	DW_START_OPERATOR  ");
            selectQuery.Append(",  DW.DW_NO_OF_COILS	AS	DW_NO_OF_COILS  ");
            selectQuery.Append(",  DW.DW_QTY	AS	DW_QTY  ");
            selectQuery.Append(",  DW.DW_METALLIC_PLATE_CODE	AS	DW_METALLIC_PLATE_CODE  ");
            selectQuery.Append(",  DW.DIE_DIM	AS	DW_DIE_DIM  ");
            selectQuery.Append(",  DW.DIE_NO	AS	DW_DIE_NO  ");
            selectQuery.Append(",  DW.DW_DIM1	AS	DW_DIM1  ");
            selectQuery.Append(",  DW.UPPER_LOWER_ROLLER_ANGLE1	AS	DW_UPPER_LOWER_ROLLER_ANGLE1  ");
            selectQuery.Append(",  DW.UPPER_LOWER_ROLLER_ANGLE2	AS	DW_UPPER_LOWER_ROLLER_ANGLE2  ");
            selectQuery.Append(",  DW.UPPER_ROLLER_ELEVATION	AS	DW_UPPER_ROLLER_ELEVATION  ");
            selectQuery.Append(",  DW.DW_TONNAGE	AS	DW_TONNAGE  ");
            selectQuery.Append(",  DW.HEX_R	AS	DW_HEX_R  ");
            selectQuery.Append(",  DW.DW_END_DATE	AS	DW_END_DATE  ");
            selectQuery.Append(",  TO_CHAR( DW.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(",  DW.DW_END_OPERATOR_CD	AS	DW_END_OPERATOR  ");
            selectQuery.Append(",  DW.DW_DECISION	AS	DW_DECISION  ");
            selectQuery.Append(",  DW.DW_NEXT_PROCESS  "); 
            selectQuery.Append(",  DW.DW_ACT_DIM_CHK	AS	DW_ACT_DIM_CHK  ");
            selectQuery.Append(",  DW.DW_ACT_DIM1	AS	DW_ACT_DIM1  ");
            selectQuery.Append(",  DW.DW_ACT_DIM2	AS	DW_ACT_DIM2  ");
            selectQuery.Append(",  DW.DW_ACT_DIM3	AS	DW_ACT_DIM3  ");
            selectQuery.Append(",  DW.DW_ACT_DIM4	AS	DW_ACT_DIM4  ");
            selectQuery.Append(",  DW.DW_ACT_DIM5	AS	DW_ACT_DIM5  ");
            selectQuery.Append(",  DW.DW_ACT_DIM6	AS	DW_ACT_DIM6  ");
            selectQuery.Append(",  DW.DW_ACT_LENGTH_CHK	AS	DW_ACT_LENGTH_CHK  ");
            selectQuery.Append(",  DW.DW_ACT_LENGTH1	AS	DW_ACT_LENGTH1  ");
            selectQuery.Append(",  DW.DW_ACT_LENGTH2	AS	DW_ACT_LENGTH2  ");
            selectQuery.Append(",  DW.DW_ACT_LENGTH3	AS	DW_ACT_LENGTH3  ");
            selectQuery.Append(",  DW.DW_FINISHED_BARS	AS	DW_FINISHED_BARS  ");
            selectQuery.Append(",  DW.DW_CORRECTED_BARS	AS	DW_CORRECTED_BARS  ");
            selectQuery.Append(",  DW.DW_NG_BARS1	AS	DW_NG_BARS1  ");
            selectQuery.Append(",  DW.DW_NG_DETAILS1	AS	DW_NG_DETAILS1  ");
            selectQuery.Append(",  PROC1.PROC_DEFECT_LONG	AS	DW_NG_DETAILS1_NAME  ");
            selectQuery.Append(",  DW.DW_NG_BARS2	AS	DW_NG_BARS2  ");
            selectQuery.Append(",  DW.DW_NG_DETAILS2	AS	DW_NG_DETAILS2  ");
            selectQuery.Append(",  PROC2.PROC_DEFECT_LONG	AS	DW_NG_DETAILS2_NAME  ");
            selectQuery.Append(",  DW.DW_NG_BARS3	AS	DW_NG_BARS3  ");
            selectQuery.Append(",  DW.DW_NG_DETAILS3	AS	DW_NG_DETAILS3  ");
            selectQuery.Append(",  PROC3.PROC_DEFECT_LONG	AS	DW_NG_DETAILS3_NAME  ");
            selectQuery.Append(",  DW.A_SIDE_FURE1	AS	DW_A_SIDE_FURE1  ");
            selectQuery.Append(",  DW.A_SIDE_FURE2	AS	DW_A_SIDE_FURE2  ");
            selectQuery.Append(",  DW.A_SIDE_FURE3	AS	DW_A_SIDE_FURE3  ");
            selectQuery.Append(",  DW.DW_BEND_STATUS	AS	DW_BEND_STATUS  ");
            selectQuery.Append(",  DW.B_SIDE_FURE1	AS	DW_B_SIDE_FURE1  ");
            selectQuery.Append(",  DW.B_SIDE_FURE2	AS	DW_B_SIDE_FURE2  ");
            selectQuery.Append(",  DW.B_SIDE_FURE3	AS	DW_B_SIDE_FURE3  ");
            selectQuery.Append(",  DW.DW_EXT_TEXTURE_STATUS	AS	DW_EXT_TEXTURE_STATUS  ");
            selectQuery.Append(",  DW.MSG_CD	AS	DW_MSG_CD  ");
            selectQuery.Append(",  DW.CREATED_DATE	AS	DW_CREATED_DATE  ");
            selectQuery.Append(",  TO_CHAR( DW.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_CREATED_DATE_STRING");
            selectQuery.Append(",  DW.UPDATED_DATE	AS	DW_UPDATED_DATE  ");
            selectQuery.Append(",  TO_CHAR( DW.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_UPDATED_DATE_STRING");
            selectQuery.Append(",  DW.DW_FINISHED_QTY	AS	DW_FINISHED_QTY  ");
            selectQuery.Append(",  DW.FURNACE_NO	AS	DW_FURNACE_NO  ");
            selectQuery.Append(",  DW.DW_DIM2	AS	DW_DIM2  ");
            selectQuery.Append(",  DW.DW_TABA_NO_1	AS	DW_TABA_NO_1  ");
            selectQuery.Append(",  DW.DW_TABA_NO_2	AS	DW_TABA_NO_2  ");
            selectQuery.Append(",  DW.DW_TABA_NO_3	AS	DW_TABA_NO_3  ");
            selectQuery.Append(",  DW.DW_TABA_NO_4	AS	DW_TABA_NO_4  ");
            selectQuery.Append(",  DW.TP_TEST	AS	DW_TP_TEST  ");
            selectQuery.Append(",  DW.UNIT_TYPE	AS	DW_UNIT_TYPE  ");
            // selectQuery.Append(" ," + Utils.GetProductUnitKubunStringValueForSQL("DW.UNIT_TYPE") + "AS DW_UNIT_TYPE_STRING ");
            selectQuery.Append(",  NVL(DW.DIE_SHAPE,0)	AS	DW_DIE_SHAPE  ");
            selectQuery.Append(",  DW.DW_FINISHED_QTY_CHK	AS	DW_FINISHED_QTY_CHK  ");
            selectQuery.Append(",  M.MSG_CONTENTS AS DW_REMARKS  ");
            selectQuery.Append(",  M.MSG_CD AS DW_MSG_CD  ");
            selectQuery.Append(",  DW.EC_NG_BAR_TOTAL	AS	DW_EC_NG_BAR_TOTAL  ");
            selectQuery.Append(",  DW.SHIKII_VALUE	AS	DW_SHIKII_VALUE  ");
            selectQuery.Append(",  DW.CARBON_PERCENTAGE	AS	DW_CARBON_PERCENTAGE  ");
            selectQuery.Append(",  DW.MAGNIZ_PERCENTAGE	AS	DW_MAGNIZ_PERCENTAGE  ");
            selectQuery.Append(",  DW.DW_DISPOSAL_NO	AS	DW_DISPOSAL_NO  ");
            selectQuery.Append(",  DW.EC_DONE_CHK	AS	DW_EC_DONE_CHK  ");
            selectQuery.Append(",  DW.EC_COIL_SIZE	AS	DW_EC_COIL_SIZE  ");
            selectQuery.Append(",  DW.EC_COIL_SHAPE	AS	DW_EC_COIL_SHAPE  ");
            selectQuery.Append(",  DW.EC_IS_OK	AS	DW_EC_IS_OK  ");
            selectQuery.Append(",  DW.MAGNETISM	AS	DW_MAGNETISM  ");

            selectQuery.Append(",  DW.USER_ID	AS	DW_USER_ID  ");
            selectQuery.Append(" , GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,trim(LEADING ' ' FROM To_Char(PP.GYOUBAN,'00'))) AS PP_CHUUBAN_GYOUBAN ");   //selectQuery.Append(" , GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");  
            selectQuery.Append(",  PP.GYOUBAN	AS	PP_GYOUBAN  ");
            selectQuery.Append(",  PP.PROD_SPEC_NO	AS	PP_PROD_SPEC_NO  ");
            selectQuery.Append(",  PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(",  TO_CHAR(PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' )	AS	PP_DELIVERY_DATE_STRING  ");

            //selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME "); 
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(",  rec.SHORT_NAME	AS	PP_RECEIVER_NAME  ");
            selectQuery.Append(",  cst.short_name	AS	PP_CUSTOMER_NAME  ");
            selectQuery.Append(",  PP.CHAMFER	AS	PP_CHAMFER  ");
            selectQuery.Append(",  PP.COLOR_ID	AS	PP_COLOR  ");
            selectQuery.Append(",  PP.MATL_NAME_MAK	AS	PP_MATL_NAME_MAK  ");
            selectQuery.Append(",  PP.MATL_SHAPE	AS	PP_MATL_SHAPE  ");
            selectQuery.Append(",  PP.MATL_DIM_D	AS	PP_MATL_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(pp.matl_dim_d,PP.MATL_DIM_W ,null,null) AS PP_MATL_DIM_D_STRING ");
            selectQuery.Append(",  PP.CHARGE	AS	PP_CHARGE  ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN  AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT_STRING ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN  AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX  AS PP_UNIT_WT_MAX ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(", PP.NYUKA_CD AS PRODUCT_NYUKO_CD");
            selectQuery.Append(",  PP.MAGARI_SPAN	AS	PP_MAGARI_SPAN  ");
            selectQuery.Append(",  PP.PROD_SHAPE	AS	PP_PROD_SHAPE  ");
            selectQuery.Append(",  PP.PROD_DIM_D	AS	PP_PROD_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,NULL,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ");
            selectQuery.Append(",  PP.PROD_NAME_CUST	AS	PP_PROD_NAME_CUST  ");
            selectQuery.Append(",  PP.PROD_DIM_OTHER	AS	PP_PROD_DIM_OTHER  ");
            selectQuery.Append(",  PP.EXT_TEXTURE	AS	PP_EXT_TEXTURE  ");
            selectQuery.Append(",  PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(",  PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(",  PP.DISPLAY	AS	PP_DISPLAY  ");
            selectQuery.Append(",  PP.STAMP	AS	PP_STAMP  ");
            selectQuery.Append(",  PP.PROD_LENGTH	AS	PP_PROD_LENGTH  ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(",  PP.LENGTH_TOL_MIN_CHR	AS	PP_LENGTH_TOL_MIN  ");
            selectQuery.Append(",  PP.LENGTH_TOL_MAX_CHR	AS	PP_LENGTH_TOL_MAX  ");

            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(",  PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(",  PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");

            selectQuery.Append(",  PP.NO_OF_UNITS	AS	PP_NO_OF_UNITS  ");
            selectQuery.Append(",  PP.UNIT	AS	PP_UNIT  ");
            selectQuery.Append(",  PP.ACT_ALLOC_QTY	AS	PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(",  TO_CHAR(PP.COIL_DIA_INTERNAL)	AS	PP_COIL_DIA_INTERNAL  ");
            selectQuery.Append(",  PP.COIL_DIA_EXTERNAL	AS	PP_COIL_DIA_EXTERNAL  ");
            //  selectQuery.Append(",  PP.COIL_DIA_WINDING	AS	PP_COIL_DIA_WINDING  ");   // error to this object Its datatype is NUMBER(1,0) in prosuct master
            selectQuery.Append(", TO_CHAR(PP.COIL_DIA_WINDING)	AS	PP_COIL_DIA_WINDING  ");
            selectQuery.Append(",  PP.KAKOUIRAI_REMARK AS	PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_REMARK	AS	PP_PROCESS_PLAN_REMARK  ");
            selectQuery.Append(",  PP.STD_DEL_PERIOD	AS	PP_STD_DEL_PERIOD  ");
            selectQuery.Append(",  PP.CREATED_DATE	AS	PP_CREATED_DATE  ");
            selectQuery.Append(",  PP.UPDATED_DATE	AS	PP_UPDATED_DATE  ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_CD	AS	PP_PROCESS_PLAN_CD  ");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = DW_START_OPERATOR_CD) AS DW_START_OPERATOR_NAME");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = DW_END_OPERATOR_CD) AS DW_END_OPERATOR_NAME");
            selectQuery.Append(", DW_NG_DETAILS1  ");
            selectQuery.Append(", DW_NG_DETAILS2  ");
            selectQuery.Append(", DW_NG_DETAILS3  ");
            selectQuery.Append(", COND.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(", REDRAWING	AS	DW_REDRAWING  ");
            selectQuery.Append("  FROM TRN_DRAWING DW  ");
            selectQuery.Append("  LEFT JOIN TRN_PROCESS_PLAN PP  ");
            selectQuery.Append("  ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD  ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append("  ON M.MSG_CD = DW.MSG_CD  ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ");
            //selectQuery.Append(" ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append("   left join mst_customer cst on pp.CUSTOMER_CD = cst.CUSTOMER_CD"); //
            selectQuery.Append(" left join mst_department_group dg on pp.DEPARTMENT_GROUP_CD=dg.DEPARTMENT_GROUP_CD ");//
            selectQuery.Append(" left join mst_employee emp on dg.EMPLOYEE_CD= emp.EMPLOYEE_CD ");//
            selectQuery.Append(" left join mst_receiver rec on pp.RECEIVER_CD=rec.RECEIVER_CD ");//
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =dg.DEPARTMENT_CD"); //new join
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =dg.LOCATION_CD"); //new join
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT PROC1 ON PROC1.PROCESS_DEFECT_CD =DW.DW_NG_DETAILS1"); //new join
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT PROC2 ON PROC2.PROCESS_DEFECT_CD =DW.DW_NG_DETAILS2");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT PROC3 ON PROC3.PROCESS_DEFECT_CD =DW.DW_NG_DETAILS2");
            selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append("  WHERE DW.DRAWING_CD=" + drawingCD);
            return ExecuteQuerySingleResult<HikinukiDetails>(selectQuery.ToString());
        }

        public void Delete(long DRAWING_CD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_DRAWING ");
            deleteQuery.Append(" WHERE DRAWING_CD =" + DRAWING_CD);
            ExecuteNonQuery(deleteQuery.ToString());
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2023/01/06       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Hikinuki Coil to Bar (Combined Daily Report) process data according to search criteria        
        /// </returns>
        /// 
        public List<HikinukiDetails> GetCombinedDailyReportData(List<SearchField> searchCriteria, string sortCiteria,int rowLimit,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<HikinukiDetails> result = new List<HikinukiDetails>();

            selectQuery.Append(" SELECT ");

            selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(DW.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS UNIT_TYPE,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            //selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS LENGTH, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" DW.DIE_DIM AS DW_DIE_DIM, ");

            selectQuery.Append(" CONCAT(DW_TABA_NO_1, ");
            selectQuery.Append("CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_1 IS NULL AND DW_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('',DW_TABA_NO_2) ");
            selectQuery.Append(" WHEN DW_TABA_NO_2 IS NOT NULL THEN ");

            selectQuery.Append(" CONCAT(',', DW_TABA_NO_2) ");

            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");

            selectQuery.Append(" END ");
            selectQuery.Append(") AS DW_TABA_NO_1_2, ");

            //selectQuery.Append(" DW.DW_ACT_DIM1 AS DW_ACT_DIM1, ");
            //selectQuery.Append(" DW.DW_ACT_DIM2 AS DW_ACT_DIM2, ");
            //selectQuery.Append(" DW.DW_ACT_DIM3 AS DW_ACT_DIM3, ");
            //selectQuery.Append(" DW.DW_ACT_DIM4 AS DW_ACT_DIM4, ");
            //selectQuery.Append(" DW.DW_ACT_DIM5 AS DW_ACT_DIM5, ");
            //selectQuery.Append(" DW.DW_ACT_DIM6 AS DW_ACT_DIM6, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM1, 'FM990.000') AS DW_ACT_DIM1_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM2, 'FM990.000') AS DW_ACT_DIM2_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM3, 'FM990.000') AS DW_ACT_DIM3_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM4, 'FM990.000') AS DW_ACT_DIM4_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM5, 'FM990.000') AS DW_ACT_DIM5_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM6, 'FM990.000') AS DW_ACT_DIM6_STRING, ");

            selectQuery.Append(" DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3, ");

            selectQuery.Append(" DW.DW_FINISHED_BARS AS DW_FINISHED_BARS, ");
            selectQuery.Append(" DW.UNIT_TYPE AS DW_UNIT_TYPE, ");
            selectQuery.Append(" DW.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");
            selectQuery.Append(" DW.MAGNETISM AS DW_MAGNETISM, ");

            selectQuery.Append(" DW.DW_BEND_STATUS, ");
            selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS, ");
            selectQuery.Append(" M.MSG_CONTENTS AS DW_REMARKS, ");

            selectQuery.Append(" CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" DW_TABA_NO_3 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_3 IS NULL AND DW_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('' ,DW_TABA_NO_4) ");
            selectQuery.Append(" WHEN DW_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(',', DW_TABA_NO_4) ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS DW_TABA_NO_3_4, ");
            selectQuery.Append(" DW.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" DW.SHIKII_VALUE AS DW_SHIKII_VALUE, ");
            selectQuery.Append(" DW.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL, ");
            selectQuery.Append(" DW.DW_NG_BARS1 ,");
            if(cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS DW_NG_DETAILS1_DECODE,");
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS DW_NG_DETAILS2_DECODE,");
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS DW_NG_DETAILS3_DECODE,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS DW_NG_DETAILS1_DECODE,");
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS DW_NG_DETAILS2_DECODE,");
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS DW_NG_DETAILS3_DECODE,");
            }
            selectQuery.Append(" DW.DW_NG_BARS2,");            
            selectQuery.Append(" DW.DW_NG_BARS3,");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DW.DW_NG_DETAILS1 = DEFECT1.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DW.DW_NG_DETAILS2 = DEFECT2.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DW.DW_NG_DETAILS3 = DEFECT3.PROCESS_DEFECT_CD ");
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

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            //else
            //{
            //    query += " ORDER BY DW_END_DATE_STRING ASC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result= ExecuteQueryMultiResult<HikinukiDetails>(selectQuery.ToString());            
            return result;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2023/08/04       
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Hikinuki Coil to Bar process data according to search criteria        
        /// </returns>
        /// 
        public List<HikinukiDetails> GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
        {
            StringBuilder selectQuery = new StringBuilder();

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
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS PP_INTERNAL_NAME, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME,PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME,");
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
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_WINDING AS COIL_DIA_WINDING, ");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER, ");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN AS MAGARI_KUBUN, ");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
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
            selectQuery.Append(" PP.TEST_SPECIMEN AS TEST_SPECIMEN, ");
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
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS PP_IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS PP_IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS PP_IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");
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


            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(DW.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS DW_START_OPERATOR_NAME, ");
            selectQuery.Append(" DW.TP_TEST AS DW_TP_TEST, ");
            selectQuery.Append(" DW.FURNACE_NO AS DW_FURNACE_NO, ");
            selectQuery.Append(" DW.DW_NO_OF_COILS AS DW_NO_OF_COILS, ");
            selectQuery.Append(" DW.DW_QTY AS DW_QTY, ");
            selectQuery.Append(" DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" DW.CARBON_PERCENTAGE AS DW_CARBON_PERCENTAGE, ");
            selectQuery.Append(" DW.MAGNIZ_PERCENTAGE AS DW_MAGNIZ_PERCENTAGE, ");
            selectQuery.Append(" DW.DIE_DIM AS DW_DIE_DIM, ");
            selectQuery.Append(" DW.DIE_SHAPE AS DW_DIE_SHAPE, ");
            selectQuery.Append(" DW.DIE_NO AS DW_DIE_NO, ");
            selectQuery.Append(" DW.DW_DIM1 AS DW_DIM1, ");
            selectQuery.Append(" DW.DW_DIM2 AS DW_DIM2, ");
            selectQuery.Append(" DW.UPPER_LOWER_ROLLER_ANGLE1 AS DW_UPPER_LOWER_ROLLER_ANGLE1, ");
            selectQuery.Append(" DW.UPPER_LOWER_ROLLER_ANGLE2 AS DW_UPPER_LOWER_ROLLER_ANGLE2, ");
            selectQuery.Append(" DW.UPPER_ROLLER_ELEVATION AS DW_UPPER_ROLLER_ELEVATION, ");
            selectQuery.Append(" DW.DW_TONNAGE AS DW_TONNAGE, ");
            selectQuery.Append(" DW.HEX_R AS DW_HEX_R, ");
            selectQuery.Append(" DW.SHIKII_VALUE AS DW_SHIKII_VALUE, ");
            selectQuery.Append(" DW.EC_COIL_SIZE AS DW_EC_COIL_SIZE, ");
            selectQuery.Append(" DW.EC_COIL_SHAPE AS DW_EC_COIL_SHAPE, ");
            selectQuery.Append(" DW.EC_IS_OK AS DW_EC_IS_OK, ");
            selectQuery.Append(" DW.REDRAWING AS DW_REDRAWING,");
            selectQuery.Append(" TO_CHAR(DW.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            //selectQuery.Append(" DW.DW_ACT_DIM1 AS DW_ACT_DIM1, ");
            //selectQuery.Append(" DW.DW_ACT_DIM2 AS DW_ACT_DIM2, ");
            //selectQuery.Append(" DW.DW_ACT_DIM3 AS DW_ACT_DIM3, ");
            //selectQuery.Append(" DW.DW_ACT_DIM4 AS DW_ACT_DIM4, ");
            //selectQuery.Append(" DW.DW_ACT_DIM5 AS DW_ACT_DIM5, ");
            //selectQuery.Append(" DW.DW_ACT_DIM6 AS DW_ACT_DIM6, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM1, 'FM990.000') AS DW_ACT_DIM1_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM2, 'FM990.000') AS DW_ACT_DIM2_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM3, 'FM990.000') AS DW_ACT_DIM3_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM4, 'FM990.000') AS DW_ACT_DIM4_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM5, 'FM990.000') AS DW_ACT_DIM5_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM6, 'FM990.000') AS DW_ACT_DIM6_STRING, ");

            selectQuery.Append(" DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3, ");
            selectQuery.Append(" DW.DW_FINISHED_BARS AS DW_FINISHED_BARS, ");
            selectQuery.Append(" DW.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" DW.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");           
            selectQuery.Append(" DW.A_SIDE_FURE1 AS DW_A_SIDE_FURE1, ");
            selectQuery.Append(" DW.A_SIDE_FURE2 AS DW_A_SIDE_FURE2, ");
            selectQuery.Append(" DW.A_SIDE_FURE3 AS DW_A_SIDE_FURE3, ");
            selectQuery.Append(" DW.B_SIDE_FURE1 AS DW_B_SIDE_FURE1, ");
            selectQuery.Append(" DW.B_SIDE_FURE2 AS DW_B_SIDE_FURE2, ");
            selectQuery.Append(" DW.B_SIDE_FURE3 AS DW_B_SIDE_FURE3, ");
            selectQuery.Append(" DW.DW_TABA_NO_1 AS DW_TABA_NO_1, ");
            selectQuery.Append(" DW.DW_TABA_NO_2 AS DW_TABA_NO_2, ");
            selectQuery.Append(" DW.DW_TABA_NO_3 AS DW_TABA_NO_3, ");
            selectQuery.Append(" DW.DW_TABA_NO_4 AS DW_TABA_NO_4, ");
            selectQuery.Append(" DW.DW_BEND_STATUS AS DW_BEND_STATUS, ");
            selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS AS DW_EXT_TEXTURE_STATUS, ");
            selectQuery.Append(" DW.DW_NG_BARS1 AS DW_NG_BARS1, ");
            selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP,");
            selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN,");
            selectQuery.Append(" DW.DW_NG_BARS2 AS DW_NG_BARS2, ");
            selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP,");
            selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN,");
            selectQuery.Append(" DW.DW_NG_BARS3 AS DW_NG_BARS3, ");
            selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP,");
            selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN,");
            selectQuery.Append(" DW.DW_CORRECTED_BARS AS DW_CORRECTED_BARS, ");
            selectQuery.Append(" DW.DW_DISPOSAL_NO AS DW_DISPOSAL_NO, ");
            selectQuery.Append(" DW.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL, ");
            selectQuery.Append(" DW.MAGNETISM AS DW_MAGNETISM, ");
            selectQuery.Append(" DW.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS DW_REMARKS ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DW.DW_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
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
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = DW.DW_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD =  DW.DW_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD =  DW.DW_NG_DETAILS3 ");
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
            return ExecuteQueryMultiResult<HikinukiDetails>(query);
        }

        public List<HikinukiDetails> GetThabaDetailsByThabaNumber(long processPlanCD, string Thaba_No_String)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new List<HikinukiDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT DR.DRAWING_CD AS DW_DRAWING_CD  ");
            selectQuery.Append(" ,DR.DW_DECISION AS DW_DECISION");
            selectQuery.Append(" ,DR.REDRAWING AS DW_REDRAWING");
            selectQuery.Append(" ,DR.DW_TABA_NO_1");
            selectQuery.Append(" ,DR.DW_TABA_NO_2");
            selectQuery.Append(" ,DR.DW_TABA_NO_3");
            selectQuery.Append(" ,DR.DW_TABA_NO_4");
            selectQuery.Append(" ,CONCAT (DR.DW_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN DR.DW_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', DR.DW_TABA_NO_4) ELSE NULL END )) AS DW_TABA_NO_1_2_3_4 ");
            selectQuery.Append(" FROM TRN_DRAWING DR INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = DR.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.PROCESS_PLAN_CD =" + processPlanCD + " ");
            selectQuery.Append(" AND DR.DW_DECISION =" + decisionVal + " ");
            selectQuery.Append(" AND ( DR.DW_TABA_NO_1 IN ('" + Thaba_No_String + "') OR DR.DW_TABA_NO_2 IN ('" + Thaba_No_String + "') OR DR.DW_TABA_NO_3 IN ('" + Thaba_No_String + "') OR DR.DW_TABA_NO_4 IN ('" + Thaba_No_String + "') )");
            selectQuery.Append(" ORDER BY DR.DRAWING_CD ");

            result = ExecuteQueryMultiResult<HikinukiDetails>(selectQuery.ToString());
            return result;
        }
        public void InsertWIPStatusHikinukiBarToBar(HikinukiDetails hikinukibartobarDetails)
        {
            //StringBuilder insertQuery = new StringBuilder();
            //if (hikinukibartobarDetails.DW_END_DATE == null)
            //{
            //    insertQuery.Append(" INSERT INTO TRN_WIP_STATUS(");
            //    insertQuery.Append(" WIP_STATUS_CD,");
            //    insertQuery.Append(" PROCESS_PLAN_CD,");
            //    insertQuery.Append(" LOCATION_CD,");
            //    insertQuery.Append(" TABA_NO,");
            //    insertQuery.Append(" WIP_NO_OF_UNITS,");
            //    insertQuery.Append(" UNIT,");
            //    insertQuery.Append(" WIP_QTY,");
            //    insertQuery.Append(" NEXT_PROCESS,");
            //    insertQuery.Append(" PROCESS_START_DATE, ");
            //    insertQuery.Append(" START_EMPLOYEE_CD, ");
            //    insertQuery.Append(" PRE_PROCESS");
            //    insertQuery.Append(" ) ");
            //    insertQuery.Append(" VALUES(");
            //    insertQuery.Append("TRN_WIP_STATUS_SEQ.NEXTVAL");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.PROCESS_PLAN_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.LOCATION_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_TABA_NO_ + "'");
            //    insertQuery.Append(",'" + 0 + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.UNIT_TYPE + "'");
            //    insertQuery.Append(",'" + 0 + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_NEXT_PROCESS + "'");
            //    insertQuery.Append(", TO_DATE('" + (Convert.ToDateTime(hikinukibartobarDetails.DW_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_START_OPERATOR_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.PROCESS_KUBUN_CD + "'");
            //    insertQuery.Append(") ");
            //}
            //else
            //{

            //    insertQuery.Append(" INSERT INTO TRN_WIP_STATUS(");
            //    insertQuery.Append(" WIP_STATUS_CD,");
            //    insertQuery.Append(" PROCESS_PLAN_CD,");
            //    insertQuery.Append(" LOCATION_CD,");
            //    insertQuery.Append(" TABA_NO,");
            //    insertQuery.Append(" WIP_NO_OF_UNITS,");
            //    insertQuery.Append(" UNIT,");
            //    insertQuery.Append(" WIP_QTY,");
            //    insertQuery.Append(" PRE_PROCESS_DATE,");
            //    insertQuery.Append(" PRE_PROCESS,");
            //    insertQuery.Append(" PRE_PROCESS_DECISION,");
            //    insertQuery.Append(" NEXT_PROCESS,");
            //    insertQuery.Append(" MSG_CD, ");
            //    insertQuery.Append(" PROCESS_START_DATE, ");
            //    insertQuery.Append(" START_EMPLOYEE_CD, ");
            //    insertQuery.Append(" END_EMPLOYEE_CD");
            //    insertQuery.Append(" ) ");
            //    insertQuery.Append(" VALUES(");
            //    insertQuery.Append("TRN_WIP_STATUS_SEQ.NEXTVAL");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.PROCESS_PLAN_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.LOCATION_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_TABA_NO_ + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_FINISHED_BARS + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.UNIT_TYPE + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_FINISHED_QTY + "'");
            //    insertQuery.Append(", TO_DATE('" + (Convert.ToDateTime(hikinukibartobarDetails.DW_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.PROCESS_KUBUN_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_DECISION + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_NEXT_PROCESS + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.MSG_CD + "'");
            //    insertQuery.Append(", TO_DATE('" + (Convert.ToDateTime(hikinukibartobarDetails.DW_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_START_OPERATOR_CD + "'");
            //    insertQuery.Append(",'" + hikinukibartobarDetails.DW_END_OPERATOR_CD + "'");
            //    insertQuery.Append(") ");
            //}
            //ExecuteNonQuery(insertQuery.ToString());
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

        public int GetRowCountForCombinedDailyReportFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*)");            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DW.DW_NG_DETAILS1 = DEFECT1.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DW.DW_NG_DETAILS2 = DEFECT2.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DW.DW_NG_DETAILS3 = DEFECT3.PROCESS_DEFECT_CD ");
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
        public int GetRowCountForHikinukiCoilToBarDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*)  ");            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DW.DW_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
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
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = DW.DW_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD =  DW.DW_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD =  DW.DW_NG_DETAILS3 ");
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
        public int GetHikinukiList(string startDate, string endDate, int locationCD, string type)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT SUM(COUNTROWS) FROM (  ");
            selectQuery.Append(" SELECT COUNT(*) AS COUNTROWS  ");
            selectQuery.Append(" FROM TRN_DRAWING DR");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DR.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE DR.DW_END_DATE BETWEEN " + "TO_DATE ('" + startDate + "','YYYY/MM/DD HH24:MI')" + " AND " + "TO_DATE ('" + endDate + "','YYYY/MM/DD HH24:MI')");
            selectQuery.Append(" AND  DR.INPUT_LOCATION_CD = " + locationCD);
            if (type == "print")
            {
                selectQuery.Append(" AND  DR.HOST_TRANSFER_DATE IS NULL  ");
            }
            else
            {
                selectQuery.Append(" AND  DR.HOST_TRANSFER_DATE IS NOT NULL  ");
            }
            selectQuery.Append(" AND DR.DW_END_DATE IS NOT NULL  ");

            selectQuery.Append(" UNION  ");


            selectQuery.Append(" SELECT COUNT(*)  ");
            selectQuery.Append(" FROM TRN_DRAWING_W DW");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE DW.DW_END_DATE BETWEEN " + "TO_DATE ('" + startDate + "','YYYY/MM/DD HH24:MI')" + " AND " + "TO_DATE ('" + endDate + "','YYYY/MM/DD HH24:MI')");
            selectQuery.Append(" AND  DW.INPUT_LOCATION_CD = " + locationCD);
            if (type == "print")
            {
                selectQuery.Append(" AND  DW.HOST_TRANSFER_DATE IS NULL  ");
            }
            else
            {
                selectQuery.Append(" AND  DW.HOST_TRANSFER_DATE IS NOT NULL  ");
            }
            selectQuery.Append(" AND DW.DW_END_DATE IS NOT NULL  ");
            selectQuery.Append(" AND DW.IM_DRAWING <> 1  ");
            selectQuery.Append(" UNION  ");

            selectQuery.Append(" SELECT COUNT(*)  ");
            selectQuery.Append(" FROM TRN_DRAWING_D DD");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE DD.DW_END_DATE BETWEEN " + "TO_DATE ('" + startDate + "','YYYY/MM/DD HH24:MI')" + " AND " + "TO_DATE ('" + endDate + "','YYYY/MM/DD HH24:MI')");
            selectQuery.Append(" AND  DD.INPUT_LOCATION_CD = " + locationCD);
            if (type == "print")
            {
                selectQuery.Append(" AND  DD.HOST_TRANSFER_DATE IS NULL  ");
            }
            else
            {
                selectQuery.Append(" AND  DD.HOST_TRANSFER_DATE IS NOT NULL  ");
            }
            selectQuery.Append(" AND DD.DW_END_DATE IS NOT NULL  ");
            selectQuery.Append(" AND DD.IM_DRAWING <> 1  ");
            selectQuery.Append(" ) HIKINUKI ");
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }
        public int GetStatusCode()
        {
            int code;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT VALUE  ");
            selectQuery.Append(" FROM MST_CONFIGURATION ");
            selectQuery.Append(" WHERE KEY = 'HIKINUKIDATA_HOST_TRANFER_BTACH_STATUS'");

            code = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (code == 1)
            {
                StringBuilder updateQuery = new StringBuilder();

                updateQuery.Append(" UPDATE MST_CONFIGURATION ");
                updateQuery.Append(" SET ");
                updateQuery.Append(" VALUE = 0 ");
                updateQuery.Append(" WHERE KEY = 'HIKINUKIDATA_HOST_TRANFER_BTACH_STATUS'");
            }

            return code;


        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/07/17      
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>        
        /// To bind machine name of Hikinuki Coil to Bar Process
        /// </returns>
        /// 
        public List<NameValue> GetHikinukiCoilToBarMachineTypeList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,MACHINE_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" AND MACHINE_TYPE IN ( '" + UIConstants.K_MACHINEGROUP_DRAWING + "','" + UIConstants.K_MACHINEGROUP_STRAIGHT_CUT + "')");
                      
            query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }

    }


}
