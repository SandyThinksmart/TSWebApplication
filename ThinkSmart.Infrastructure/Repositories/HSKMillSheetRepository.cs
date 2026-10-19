using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.Repositories;
using static TSWebApplication.Domain.Utility.Constants;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public class HSKMillSheetRepository : BaseRepository, IHSKMillSheetRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly string specialChuuban;
        public HSKMillSheetRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            specialChuuban = Convert.ToString(_configuration.GetSection("SpecialChuuban").Value);
        }
        /// <summary>
        /// Created By Sandeep Hajare 2024/15/06
        /// </summary>
        /// <param name="CHUUBAN"></param>
        /// <param name="GYOUBAN"></param>
        /// <returns>
        /// 1) Select TRN_PROCESS_Plan Data With check PROCESS_PLAN_CMPL_DATE
        /// 2) TRN_PROCESS_Plan Data Return HSKMillSheetDetails Model  
        /// </returns>
        public HSKMillSheetDetails GetHSKMillSheetDetailsByChuuban(string CHUUBAN, short? GYOUBAN)
        {
            HSKMillSheetDetails hskmillsheetDetails = new HSKMillSheetDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.CHUUBAN ");
            selectQuery.Append(" ,PP.GYOUBAN AS GYOUBAN_STRING");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,CUS.YOBINA AS PP_YOBINA ");
            selectQuery.Append(" ,CUS.CUSTOMER_CD AS PP_CUSTOMER_CD ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,MM.MAKER_CD AS PP_MAKER_CD ");
            selectQuery.Append(" ,MM.MAKER_NAME AS PP_MAKER_NAME ");
            // selectQuery.Append(" ,get_prod_condition(pp.ver, pp.PROD_SPEC_NO1, pp.PROD_SPEC_NO2) AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
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
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR_ID ");
            // selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");     // AND PR.VER = PP.PROD_VER   2019-10-02 Pradeep M.
            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO1  AS PP_PROD_SPEC_NO1 ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO2  AS PP_PROD_SPEC_NO2 ");
            selectQuery.Append(" ,PP.VER  AS PP_PROD_VER ");

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
            //   selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            //selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetProdUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");    //GetMatlUnitForDBQuery
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE  ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.ACT_ALLOC_DATE AS PP_ACT_ALLOC_DATE_STRING ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_WINDING) AS PP_COIL_DIA_WINDING ");
            //  selectQuery.Append(" ,K.REMARKS AS  PP_KAKOUIRAI_REMARKS  ");           //PP_KAKOUIRAIREMARKS
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK, ");

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
            selectQuery.Append("PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_MAKER MM ON MM.MAKER_CD = PP.MAKER_CD");
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

            selectQuery.Append(" WHERE PP.CHUUBAN= '" + CHUUBAN + "'");
            selectQuery.Append(" AND PP.GYOUBAN= '" + GYOUBAN + "'");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            // return ExecuteQuerySingleResult<HSKMillSheetDetails>(selectQuery.ToString());
            hskmillsheetDetails = ExecuteQuerySingleResult<HSKMillSheetDetails>(selectQuery.ToString());
            return hskmillsheetDetails;
        }


        /// <summary>
        /// Created By Sandeep Hajare 2024/15/06
        /// </summary>
        /// <param name="FILENAME"></param>

        /// <returns>
        /// 1) Select MAKER_MILL_SHEET Data With check FILENAME
        /// 2) TRN_PROCESS_Plan Data Return MakerMillSheetDetails Model  
        /// </returns>
        public MakerMillSheetDetails GetMakerMillSheetChemicalCompositionPopup(string FILENAME)
        {
            MakerMillSheetDetails makermillsheetDetails = new MakerMillSheetDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" maker_mill_sheet_cd ");
            selectQuery.Append(" ,hard_std1 ");
            selectQuery.Append(" ,hard_min1 ");
            selectQuery.Append(" ,hard_max1 ");
            selectQuery.Append(" ,hard_actual1 ");
            selectQuery.Append(" ,hard_std2 ");
            selectQuery.Append(" ,hard_min2 ");
            selectQuery.Append(" ,hard_max2 ");
            selectQuery.Append(" ,hard_actual2 ");
            selectQuery.Append(" ,hard_std3 ");
            selectQuery.Append(" ,hard_min3 ");
            selectQuery.Append(" ,hard_max3  ");
            selectQuery.Append(" ,hard_actual3 ");
            selectQuery.Append(" ,hard_std4");
            selectQuery.Append(" ,hard_min4");
            selectQuery.Append(" ,hard_max4 ");
            selectQuery.Append(" ,hard_actual4 ");
            selectQuery.Append(" ,hard_std5 ");
            selectQuery.Append(" ,hard_min5 ");
            selectQuery.Append(" ,hard_max5 ");
            selectQuery.Append(" ,hard_actual5 ");    //COIL_QTY

            selectQuery.Append(", hard_std6");
            selectQuery.Append(", hard_min6");
            selectQuery.Append(", hard_max6");
            selectQuery.Append(" ,hard_actual6 ");
            selectQuery.Append(" ,hard_std7 ");
            selectQuery.Append(" ,hard_min7 ");
            selectQuery.Append(" ,hard_max7 ");
            selectQuery.Append(" ,hard_actual7 ");
            selectQuery.Append(" ,hard_std8 ");
            selectQuery.Append(" ,hard_min8 ");
            selectQuery.Append(" ,hard_max8 ");
            selectQuery.Append(" ,hard_actual8 ");
            selectQuery.Append(" ,hard_std9 ");
            selectQuery.Append(" ,hard_min9 ");
            selectQuery.Append(" ,hard_max9 ");
            selectQuery.Append(" ,hard_actual9 ");
            selectQuery.Append(" ,hard_std10 ");
            selectQuery.Append(" ,hard_min10 ");
            selectQuery.Append(" ,hard_max10  ");
            selectQuery.Append(" ,hard_actual10 ");
            selectQuery.Append(" ,hard_std11 ");
            selectQuery.Append(" ,hard_min11 ");
            selectQuery.Append(", hard_max11 ");
            selectQuery.Append(", hard_actual11 ");
            selectQuery.Append(" ,hard_std12  ");
            selectQuery.Append(" ,hard_min12 ");
            selectQuery.Append(" ,hard_max12 ");
            selectQuery.Append(" ,hard_actual12 ");
            selectQuery.Append(" ,hard_std13 ");
            //  selectQuery.Append(" ,K.REMARKS AS  PP_KAKOUIRAI_REMARKS  ");           //PP_KAKOUIRAIREMARKS
            selectQuery.Append(" ,hard_min13 ");

            selectQuery.Append(" hard_max13, ");
            selectQuery.Append(" hard_actual13, ");
            selectQuery.Append(" hard_std14, ");
            selectQuery.Append(" hard_min14, ");
            selectQuery.Append(" hard_max14, ");
            selectQuery.Append(" hard_actual14, ");
            selectQuery.Append(" hard_std15, ");
            selectQuery.Append(" hard_min15, ");
            selectQuery.Append(" hard_max15, ");
            selectQuery.Append(" hard_actual15, ");
            selectQuery.Append(" hard_std16, ");
            selectQuery.Append(" hard_min16, ");
            selectQuery.Append(" hard_max16, ");
            selectQuery.Append(" hard_actual16,");
            selectQuery.Append(" hard_std17,");
            selectQuery.Append(" hard_min17,");
            selectQuery.Append(" hard_max17,");
            selectQuery.Append(" hard_actual17,");

            selectQuery.Append(" hard_std18,");
            selectQuery.Append("hard_min18,");
            selectQuery.Append(" hard_max18,");
            selectQuery.Append(" hard_actual18,");
            selectQuery.Append("hard_std19 ,");

            selectQuery.Append("hard_min19 ,");
            selectQuery.Append("hard_max19 ,");
            selectQuery.Append("hard_actual19 ,");
            selectQuery.Append("hard_std20 	,");
            selectQuery.Append("hard_min20	,");
            selectQuery.Append("hard_max20 ,");
            selectQuery.Append("hard_actual20,");

            selectQuery.Append("hard_std21 ,");
            selectQuery.Append("hard_min21 ,");
            selectQuery.Append("hard_max21 ,");
            selectQuery.Append("hard_actual21 ,");
            selectQuery.Append("hard_std22  ,");
            selectQuery.Append("hard_min22 ,");

            selectQuery.Append("hard_max22,");
            selectQuery.Append("hard_actual22,");
            selectQuery.Append("hard_std23,");
            selectQuery.Append("hard_min23,");
            selectQuery.Append("hard_max23,");

            selectQuery.Append("hard_actual23,");
            selectQuery.Append("hard_std24,");
            selectQuery.Append("hard_min24,");
            selectQuery.Append("hard_max24,");
            selectQuery.Append("hard_actual24,");

            selectQuery.Append("hard_std25,");
            selectQuery.Append("hard_min25,");
            selectQuery.Append("hard_max25,");
            selectQuery.Append("hard_actual25,");
            selectQuery.Append("grain_size_std,");
            selectQuery.Append("grain_size_actual");
            selectQuery.Append(",filename,");
            selectQuery.Append("to_char(created_date, 'YYYY/MM/DD') AS created_date");


            selectQuery.Append("  FROM MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE filename= '" + FILENAME + "'");
            // return ExecuteQuerySingleResult<HSKMillSheetDetails>(selectQuery.ToString());
            makermillsheetDetails = ExecuteQuerySingleResult<MakerMillSheetDetails>(selectQuery.ToString());
            return makermillsheetDetails;
        }


        /// <summary>
        /// Created By Sandeep Hajare 2026/01/19
        /// </summary>
        /// <param name="custName"></param>
        /// <param name="prodNameCust"></param>
        /// <returns>
        /// 1) Select ChemicalCompositionDetails Data With check prodNameCust and custName
        /// 2)  Data Return ChemicalCompositionDetails Model  
        /// </returns>
        public List<ChemicalCompositionDetails> GetHSKChemicalCompositionDetails(string custName, string prodNameCust)
        {
            List<ChemicalCompositionDetails> makermillsheetDetails = new List<ChemicalCompositionDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MCC.CHEMICAL_COMPOSITION_CD,");
            selectQuery.Append(" MC.CUSTOMER_CD,");
            selectQuery.Append(" MCC.PRODUCT_NAME_CUST,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_1,");
            selectQuery.Append(" MCC.MAGNIFICATION_1,");
            selectQuery.Append(" MCC.ES_MIN_1,");
            selectQuery.Append(" MCC.ES_MAX_1,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_2,");
            selectQuery.Append(" MCC.MAGNIFICATION_2,");
            selectQuery.Append(" MCC.ES_MIN_2,");
            selectQuery.Append(" MCC.ES_MAX_2,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_3,");
            selectQuery.Append(" MCC.MAGNIFICATION_3,");
            selectQuery.Append(" MCC.ES_MIN_3,");
            selectQuery.Append(" MCC.ES_MAX_3,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_4,");
            selectQuery.Append(" MCC.MAGNIFICATION_4,");
            selectQuery.Append(" MCC.ES_MIN_4,");
            selectQuery.Append(" MCC.ES_MAX_4,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_5,");
            selectQuery.Append(" MCC.MAGNIFICATION_5,");
            selectQuery.Append(" MCC.ES_MIN_5,");
            selectQuery.Append(" MCC.ES_MAX_5,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_6,");
            selectQuery.Append(" MCC.MAGNIFICATION_6,");
            selectQuery.Append(" MCC.ES_MIN_6,");
            selectQuery.Append(" MCC.ES_MAX_6,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_7,");
            selectQuery.Append(" MCC.MAGNIFICATION_7,");
            selectQuery.Append(" MCC.ES_MIN_7,");
            selectQuery.Append(" MCC.ES_MAX_7,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_8,");
            selectQuery.Append(" MCC.MAGNIFICATION_8,");
            selectQuery.Append(" MCC.ES_MIN_8,");
            selectQuery.Append(" MCC.ES_MAX_8,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_9,");
            selectQuery.Append(" MCC.MAGNIFICATION_9,");
            selectQuery.Append(" MCC.ES_MIN_9,");
            selectQuery.Append(" MCC.ES_MAX_9,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_10,");
            selectQuery.Append(" MCC.MAGNIFICATION_10,");
            selectQuery.Append(" MCC.ES_MIN_10,");
            selectQuery.Append(" MCC.ES_MAX_10,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_11,");
            selectQuery.Append(" MCC.MAGNIFICATION_11,");
            selectQuery.Append(" MCC.ES_MIN_11,");
            selectQuery.Append(" MCC.ES_MAX_11,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_12,");
            selectQuery.Append(" MCC.MAGNIFICATION_12,");
            selectQuery.Append(" MCC.ES_MIN_12,");
            selectQuery.Append(" MCC.ES_MAX_12,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_13,");
            selectQuery.Append(" MCC.MAGNIFICATION_13,");
            selectQuery.Append(" MCC.ES_MIN_13,");
            selectQuery.Append(" MCC.ES_MAX_13,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_14,");
            selectQuery.Append(" MCC.MAGNIFICATION_14,");
            selectQuery.Append(" MCC.ES_MIN_14,");
            selectQuery.Append(" MCC.ES_MAX_14,");
            selectQuery.Append(" MCC.ELEMENT_SYMBOL_15,");
            selectQuery.Append(" MCC.MAGNIFICATION_15,");
            selectQuery.Append(" MCC.ES_MIN_15,");
            selectQuery.Append(" MCC.ES_MAX_15,");
            selectQuery.Append(" MCC.IN_ACTIVE_FLAG");
            selectQuery.Append("  FROM MST_CHEMICAL_COMPOSITION MCC ");
            selectQuery.Append("LEFT JOIN MST_CUSTOMER MC ON MC.CUSTOMER_CD = MCC.CUSTOMER_CD ");
            selectQuery.Append(" WHERE MCC.PRODUCT_NAME_CUST= '" + prodNameCust + "'");
            selectQuery.Append(" AND ( MC.CUSTOMER_CD= '" + custName + "' OR MC.CUSTOMER_NAME IS NULL)");
            selectQuery.Append(" AND MCC.IN_ACTIVE_FLAG = 0");
            selectQuery.Append(" AND ROWNUM = 1");
            // return ExecuteQuerySingleResult<HSKMillSheetDetails>(selectQuery.ToString());
            makermillsheetDetails = ExecuteQueryMultiResult<ChemicalCompositionDetails>(selectQuery.ToString());
            return makermillsheetDetails;
        }

        /// <summary>
        /// Created By Sandeep Hajare 2026/01/19
        /// </summary>
        /// <param name="custName"></param>
        /// <param name="prodNameCust"></param>
        /// <returns>
        /// 1) Select ChemicalCompositionDetails Data With check prodNameCust and custName
        /// 2)  Data Return ChemicalCompositionDetails Model  
        /// </returns>
        public List<MakerMillSheetDetails> GetHSKChemicalCompositionAandMakerDetails(string custName, string prodNameCust, string makerMillsheetCD)
        {
            List<MakerMillSheetDetails> makermillsheetDetails = new List<MakerMillSheetDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAKER_MILL_SHEET_CD,");
            selectQuery.Append(" MAKER_CD,	");
            selectQuery.Append(" PDF_FILE_PATH,");
            selectQuery.Append(" FILENAME,");
            // selectQuery.Append(" PDF_PAGE_NUMBER,");
            selectQuery.Append(" KOUBAN,");
            selectQuery.Append(" MATL_NAME_MAKER,");
            selectQuery.Append(" DIM_D,");
            selectQuery.Append(" LENGTH	,	");
            selectQuery.Append(" CHARGE	,	");
            selectQuery.Append(" NO_OF_UNITS	,	");
            selectQuery.Append(" UNIT	,	");
            selectQuery.Append(" QUANTITY	,	");
            selectQuery.Append(" GRAIN_SIZE_STD	,	");
            selectQuery.Append(" GRAIN_SIZE_ACTUAL	,	");
            selectQuery.Append(" HARDNESS_STD	,	");
            selectQuery.Append(" HARDNESS_MIN	,	");
            selectQuery.Append(" HARDNESS_MAX	,	");
            selectQuery.Append(" HARDNESS_ACTUAL	,	");
            selectQuery.Append(" CCNAME1	,	");
            selectQuery.Append(" CCRANGE1,		");
            selectQuery.Append(" CCMIN1	,	");
            selectQuery.Append(" CCMAX1	,	");
            selectQuery.Append(" CCACTUAL1,		");
            selectQuery.Append(" CCNAME2	,	");
            selectQuery.Append(" CCRANGE2,		");
            selectQuery.Append(" CCMIN2	,	");
            selectQuery.Append(" CCMAX2	,	");
            selectQuery.Append(" CCACTUAL2,		");
            selectQuery.Append(" CCNAME3	,	");
            selectQuery.Append(" CCRANGE3,		");
            selectQuery.Append(" CCMIN3	,	");
            selectQuery.Append(" CCMAX3	,	");
            selectQuery.Append(" CCACTUAL3,		");
            selectQuery.Append(" CCNAME4	,	");
            selectQuery.Append(" CCRANGE4,		");
            selectQuery.Append(" CCMIN4	,	");
            selectQuery.Append(" CCMAX4	,	");
            selectQuery.Append(" CCACTUAL4	,	");
            selectQuery.Append(" CCNAME5	,	");
            selectQuery.Append(" CCRANGE5	,	");
            selectQuery.Append(" CCMIN5	,	");
            selectQuery.Append(" CCMAX5	,	");
            selectQuery.Append(" CCACTUAL5	,	");
            selectQuery.Append(" CCNAME6	,	");
            selectQuery.Append(" CCRANGE6	,	");
            selectQuery.Append(" CCMIN6	,	");
            selectQuery.Append(" CCMAX6	,	");
            selectQuery.Append(" CCACTUAL6	,	");
            selectQuery.Append(" CCNAME7	,	");
            selectQuery.Append(" CCRANGE7,		");
            selectQuery.Append(" CCMIN7,		");
            selectQuery.Append(" CCMAX7,		");
            selectQuery.Append(" CCACTUAL7,		");
            selectQuery.Append(" CCNAME8,		");
            selectQuery.Append(" CCRANGE8,		");
            selectQuery.Append(" CCMIN8,		");
            selectQuery.Append(" CCMAX8,		");
            selectQuery.Append(" CCACTUAL8	,	");
            selectQuery.Append(" CCNAME9	,	");
            selectQuery.Append(" CCRANGE9	,	");
            selectQuery.Append(" CCMIN9	,	");
            selectQuery.Append(" CCMAX9	,	");
            selectQuery.Append(" CCACTUAL9	,	");
            selectQuery.Append(" CCNAME10	,	");
            selectQuery.Append(" CCRANGE10	,	");
            selectQuery.Append(" CCMIN10	,	");
            selectQuery.Append(" CCMAX10	,	");
            selectQuery.Append(" CCACTUAL10	,	");
            selectQuery.Append(" CCNAME11	,	");
            selectQuery.Append(" CCRANGE11	,	");
            selectQuery.Append(" CCMIN11	,	");
            selectQuery.Append(" CCMAX11	,	");
            selectQuery.Append(" CCACTUAL11	,	");
            selectQuery.Append(" CCNAME12	,	");
            selectQuery.Append(" CCRANGE12	,	");
            selectQuery.Append(" CCMIN12	,	");
            selectQuery.Append(" CCMAX12	,	");
            selectQuery.Append(" CCACTUAL12	,	");
            selectQuery.Append(" CCNAME13	,	");
            selectQuery.Append(" CCRANGE13	,	");
            selectQuery.Append(" CCMIN13	,	");
            selectQuery.Append(" CCMAX13	,	");
            selectQuery.Append(" CCACTUAL13	,	");
            selectQuery.Append(" CCNAME14	,	");
            selectQuery.Append(" CCRANGE14	,	");
            selectQuery.Append(" CCMIN14	,	");
            selectQuery.Append(" CCMAX14	,	");
            selectQuery.Append(" CCACTUAL14	,	");
            selectQuery.Append(" CCNAME15	,	");
            selectQuery.Append(" CCRANGE15	,	");
            selectQuery.Append(" CCMIN15	,	");
            selectQuery.Append(" CCMAX15	,	");
            selectQuery.Append(" CCACTUAL15	,	");
            selectQuery.Append(" CCNAME16	,	");
            selectQuery.Append(" CCRANGE16	,	");
            selectQuery.Append(" CCMIN16	,	");
            selectQuery.Append(" CCMAX16	,	");
            selectQuery.Append(" CCACTUAL16	,	");
            selectQuery.Append(" CCNAME17	,	");
            selectQuery.Append(" CCRANGE17	,	");
            selectQuery.Append(" CCMIN17	,	");
            selectQuery.Append(" CCMAX17	,	");
            selectQuery.Append(" CCACTUAL17	,	");
            selectQuery.Append(" CCNAME18	,	");
            selectQuery.Append(" CCRANGE18	,	");
            selectQuery.Append(" CCMIN18	,	");
            selectQuery.Append(" CCMAX18	,	");
            selectQuery.Append(" CCACTUAL18	,	");
            selectQuery.Append(" CCNAME19	,	");
            selectQuery.Append(" CCRANGE19	,	");
            selectQuery.Append(" CCMIN19	,	");
            selectQuery.Append(" CCMAX19	,	");
            selectQuery.Append(" CCACTUAL19	,	");
            selectQuery.Append(" CCNAME20	,	");
            selectQuery.Append(" CCRANGE20	,	");
            selectQuery.Append(" CCMIN20	,	");
            selectQuery.Append(" CCMAX20	,	");
            selectQuery.Append(" CCACTUAL20	,	");
            selectQuery.Append(" CCNAME21	,	");
            selectQuery.Append(" CCRANGE21	,	");
            selectQuery.Append(" CCMIN21	,	");
            selectQuery.Append(" CCMAX21	,	");
            selectQuery.Append(" CCACTUAL21	,	");
            selectQuery.Append(" CCNAME22	,	");
            selectQuery.Append(" CCRANGE22	,	");
            selectQuery.Append(" CCMIN22	,	");
            selectQuery.Append(" CCMAX22	,	");
            selectQuery.Append(" CCACTUAL22	,	");
            selectQuery.Append(" CCNAME23	,	");
            selectQuery.Append(" CCRANGE23	,	");
            selectQuery.Append(" CCMIN23	,	");
            selectQuery.Append(" CCMAX23	,	");
            selectQuery.Append(" CCACTUAL23	,	");
            selectQuery.Append(" CCNAME24	,	");
            selectQuery.Append(" CCRANGE24	,	");
            selectQuery.Append(" CCMIN24	,	");
            selectQuery.Append(" CCMAX24	,	");
            selectQuery.Append(" CCACTUAL24	,	");
            selectQuery.Append(" CCNAME25	,	");
            selectQuery.Append(" CCRANGE25	,	");
            selectQuery.Append(" CCMIN25	,	");
            selectQuery.Append(" CCMAX25	,	");
            selectQuery.Append(" CCACTUAL25	,	");
            selectQuery.Append(" HARD_STD1	,	");
            selectQuery.Append(" HARD_MIN1	,	");
            selectQuery.Append(" HARD_MAX1	,	");
            selectQuery.Append(" HARD_ACTUAL1	,	");
            selectQuery.Append(" HARD_STD2	,	");
            selectQuery.Append(" HARD_MIN2	,	");
            selectQuery.Append(" HARD_MAX2	,	");
            selectQuery.Append(" HARD_ACTUAL2	,	");
            selectQuery.Append(" HARD_STD3	,	");
            selectQuery.Append(" HARD_MIN3	,	");
            selectQuery.Append(" HARD_MAX3	,	");
            selectQuery.Append(" HARD_ACTUAL3	,	");
            selectQuery.Append(" HARD_STD4	,	");
            selectQuery.Append(" HARD_MIN4	,	");
            selectQuery.Append(" HARD_MAX4	,	");
            selectQuery.Append(" HARD_ACTUAL4	,	");
            selectQuery.Append(" HARD_STD5	,	");
            selectQuery.Append(" HARD_MIN5	,	");
            selectQuery.Append(" HARD_MAX5	,	");
            selectQuery.Append(" HARD_ACTUAL5	,	");
            selectQuery.Append(" HARD_STD6	,	");
            selectQuery.Append(" HARD_MIN6	,	");
            selectQuery.Append(" HARD_MAX6	,	");
            selectQuery.Append(" HARD_ACTUAL6	,	");
            selectQuery.Append(" HARD_STD7	,	");
            selectQuery.Append(" HARD_MIN7	,	");
            selectQuery.Append(" HARD_MAX7	,	");
            selectQuery.Append(" HARD_ACTUAL7	,	");
            selectQuery.Append(" HARD_STD8	,	");
            selectQuery.Append(" HARD_MIN8	,	");
            selectQuery.Append(" HARD_MAX8	,	");
            selectQuery.Append(" HARD_ACTUAL8	,	");
            selectQuery.Append(" HARD_STD9	,	");
            selectQuery.Append(" HARD_MIN9	,	");
            selectQuery.Append(" HARD_MAX9	,	");
            selectQuery.Append(" HARD_ACTUAL9	,	");
            selectQuery.Append(" HARD_STD10	,	");
            selectQuery.Append(" HARD_MIN10	,	");
            selectQuery.Append(" HARD_MAX10	,	");
            selectQuery.Append(" HARD_ACTUAL10	,	");
            selectQuery.Append(" HARD_STD11	,	");
            selectQuery.Append(" HARD_MIN11	,	");
            selectQuery.Append(" HARD_MAX11	,	");
            selectQuery.Append(" HARD_ACTUAL11	,	");
            selectQuery.Append(" HARD_STD12	,	");
            selectQuery.Append(" HARD_MIN12	,	");
            selectQuery.Append(" HARD_MAX12	,	");
            selectQuery.Append(" HARD_ACTUAL12	,	");
            selectQuery.Append(" HARD_STD13	,	");
            selectQuery.Append(" HARD_MIN13	,	");
            selectQuery.Append(" HARD_MAX13	,	");
            selectQuery.Append(" HARD_ACTUAL13	,	");
            selectQuery.Append(" HARD_STD14	,	");
            selectQuery.Append(" HARD_MIN14	,	");
            selectQuery.Append(" HARD_MAX14	,	");
            selectQuery.Append(" HARD_ACTUAL14	,	");
            selectQuery.Append(" HARD_STD15	,	");
            selectQuery.Append(" HARD_MIN15	,	");
            selectQuery.Append(" HARD_MAX15	,	");
            selectQuery.Append(" HARD_ACTUAL15	,	");
            selectQuery.Append(" HARD_STD16	,	");
            selectQuery.Append(" HARD_MIN16	,	");
            selectQuery.Append(" HARD_MAX16	,	");
            selectQuery.Append(" HARD_ACTUAL16	,	");
            selectQuery.Append(" HARD_STD17	,	");
            selectQuery.Append(" HARD_MIN17	,	");
            selectQuery.Append(" HARD_MAX17	,	");
            selectQuery.Append(" HARD_ACTUAL17	,	");
            selectQuery.Append(" HARD_STD18	,	");
            selectQuery.Append(" HARD_MIN18	,	");
            selectQuery.Append(" HARD_MAX18	,	");
            selectQuery.Append(" HARD_ACTUAL18	,	");
            selectQuery.Append(" HARD_STD19	,	");
            selectQuery.Append(" HARD_MIN19	,	");
            selectQuery.Append(" HARD_MAX19	,	");
            selectQuery.Append(" HARD_ACTUAL19	,	");
            selectQuery.Append(" HARD_STD20	,	");
            selectQuery.Append(" HARD_MIN20	,	");
            selectQuery.Append(" HARD_MAX20	,	");
            selectQuery.Append(" HARD_ACTUAL20	,	");
            selectQuery.Append(" HARD_STD21	,	");
            selectQuery.Append(" HARD_MIN21	,	");
            selectQuery.Append(" HARD_MAX21	,	");
            selectQuery.Append(" HARD_ACTUAL21	,	");
            selectQuery.Append(" HARD_STD22	,	");
            selectQuery.Append(" HARD_MIN22	,	");
            selectQuery.Append(" HARD_MAX22	,	");
            selectQuery.Append(" HARD_ACTUAL22	,	");
            selectQuery.Append(" HARD_STD23	,	");
            selectQuery.Append(" HARD_MIN23	,	");
            selectQuery.Append(" HARD_MAX23	,	");
            selectQuery.Append(" HARD_ACTUAL23	,	");
            selectQuery.Append(" HARD_STD24	,	");
            selectQuery.Append(" HARD_MIN24	,	");
            selectQuery.Append(" HARD_MAX24	,	");
            selectQuery.Append(" HARD_ACTUAL24	,	");
            selectQuery.Append(" HARD_STD25	,	");
            selectQuery.Append(" HARD_MIN25	,	");
            selectQuery.Append(" HARD_MAX25	,	");
            selectQuery.Append(" HARD_ACTUAL25	,	");
            selectQuery.Append(" CREATED_DATE		");

            selectQuery.Append("  FROM MAKER_MILL_SHEET  ");

            selectQuery.Append(" WHERE MAKER_MILL_SHEET_CD = '" + makerMillsheetCD + "'");

            // return ExecuteQuerySingleResult<HSKMillSheetDetails>(selectQuery.ToString());
            makermillsheetDetails = ExecuteQueryMultiResult<MakerMillSheetDetails>(selectQuery.ToString());
            return makermillsheetDetails;
        }


        /// <summary>
        /// Created By Shohana 2025/27/05
        /// </summary>
        /// <param name="searchFields"></param>
        /// <param name="sortCiteria"></param>
        /// <param name="locationCD"></param>
        /// <returns>
        /// 1) Get List of HSK MIllSheet
        /// 2) On Search Criteria
        /// </returns>
        public List<HSKMillSheetDetails> GetHSKMillSheetList(List<SearchField> searchCriteria, string sortCiteria, short locationCD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" HMS.HSK_MILL_SHEET_CD, ");
            selectQuery.Append(" PP.CHUUBAN_YEAR || '-' || PP.CHUUBAN || '-' || PP.GYOUBAN AS PP_CHUUBAN, ");
            selectQuery.Append(" HMS.PROCESS_PLAN_CD, ");
            selectQuery.Append(" HMS.MAKER_MILL_SHEET_CD, ");
            selectQuery.Append(" HMS.CUSTOMER_CD, ");
            selectQuery.Append(" HMS.RECEIVER_NAME, ");
            selectQuery.Append(" M.MAKER_NAME, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" HMS.PROD_SHAPE, ");
            selectQuery.Append(" HMS.PROD_DIM_D, ");
            selectQuery.Append(" HMS.PROD_LENGTH, ");
            selectQuery.Append(" HMS.IS_MATL_DIM, ");
            selectQuery.Append(" TO_CHAR(HMS.DELIVERY_DATE, 'YYYY/MM/DD') AS DELIVERY_DATE, ");
            selectQuery.Append(" HMS.WEIGHT, ");
            selectQuery.Append(" HMS.NO_OF_UNITS, ");
            selectQuery.Append(" HMS.UNIT, ");
            selectQuery.Append(" HMS.CREATED_DATE, ");
            selectQuery.Append(" HMS.UPDATED_DATE, ");
            selectQuery.Append(" HMS.PROD_NAME_CUST, ");
            selectQuery.Append(" PP.UNIT_WT, ");
            selectQuery.Append(" PP.CHUUBAN, ");
            selectQuery.Append(" PP.GYOUBAN, ");
            selectQuery.Append(" CUST.CUSTOMER_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" HMS.RECEIVER_NAME, ");
            selectQuery.Append(" PP.CHUUBAN_YEAR ");
            selectQuery.Append("FROM TRN_HSK_MILL_SHEET HMS ");
            selectQuery.Append("LEFT JOIN MST_CONDITION CND ON CND.CONDITION_CD = HMS.PROD_CONDITION_CD ");
            selectQuery.Append("LEFT JOIN MAKER_MILL_SHEET MK ON MK.MAKER_MILL_SHEET_CD = HMS.MAKER_MILL_SHEET_CD ");
            selectQuery.Append("LEFT JOIN MST_MAKER M ON M.MAKER_CD = MK.MAKER_CD ");
            selectQuery.Append("LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HMS.PROCESS_PLAN_CD ");
            selectQuery.Append("LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = HMS.CUSTOMER_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" 1 = 1 ");
            selectQuery.Append(" AND PP.CHUUBAN IS NOT NULL ");

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

            selectQuery = new StringBuilder();
            selectQuery.Append(query);

            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;
        }


        /// <summary>
        /// Created By Sandeep Hajare 2025/06/05
        /// </summary>
        /// <param name="searchFields"></param>
        /// <param name="sortCiteria"></param>
        /// <param name="locationCD"></param>
        /// <returns>
        /// 1) Get List of HSK MIllSheet
        /// 2) On Search Criteria
        /// </returns>
        public List<HSKMillSheetDetails> GetHSKMillSheetHistoryPopUpList(List<SearchField> searchCriteria, string sortCiteria, short locationCD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            // selectQuery.Append(" TPP.HSK_MILL_SHEET_CD AS HSKMILL_SHEET_CD, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(CHUUBAN_YEAR || CHUUBAN, GYOUBAN)AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD, ");
            selectQuery.Append(" PP.CUSTOMER_CD, ");
            selectQuery.Append(" CUST.CUSTOMER_NAME          AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST, ");
            selectQuery.Append(" PP.PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_LENGTH, ");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION   AS PP_PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS DELIVERY_DATE, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" M.MAKER_NAME AS PP_MAKER_NAME_SHORT, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE, 'YYYY/MM/DD') AS PP_PROCESS_PLAN_CMPL_DATE_STRING, ");
            selectQuery.Append(" PP.CHUUBAN AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.GYOUBAN, ");
            selectQuery.Append(" PP.CREATED_DATE ");
            selectQuery.Append("FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("LEFT JOIN MST_CONDITION      CND ON CND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append("LEFT JOIN MST_MAKER          M ON M.MAKER_CD = PP.MAKER_CD ");
            selectQuery.Append("LEFT JOIN MST_CUSTOMER       CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");

            selectQuery.Append(" WHERE ");
            //  selectQuery.Append(" 1 = 1 ");
            selectQuery.Append(" PP.CHUUBAN IS NOT NULL ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NOT NULL ");
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

            selectQuery = new StringBuilder();
            selectQuery.Append(query);

            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;
        }

        /// <summary>
        /// Created By Sandeep Hajare 2025/06/05
        /// </summary>
        /// <param name="chuuban"></param>
        /// <param name="gouban"></param>

        /// <returns>
        /// 1) Get PROCESS_PLAN_CD

        /// </returns>
        public int GetProcessPlanCD(string chuuban, short gouban)
        {
            int processPlanCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT PROCESS_PLAN_CD");
            query.Append(" FROM TRN_PROCESS_PLAN ");
            query.Append(" WHERE CHUUBAN = '" + chuuban + "'");
            query.Append(" AND GYOUBAN = '" + gouban + "'");
            query.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
            //query.Append(" WHERE CHUUBAN=" + chuuban);
            // query.Append(" AND GYOUBAN=" + gouban);
            processPlanCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return processPlanCD;
        }
        /// <summary>
        /// Created By Sandeep Hajare 2025/06/05
        /// Retrieves the condition description for the specified product code.
        /// </summary>
        /// <param name="Prod_CD">The product code.</param>
        /// <returns>
        /// The CONDITION_DESCRIPTION associated with the specified product code.
        /// </returns>
        public string GetCondDescFromProdCD(long Prod_CD)
        {
            string ProdCond = "";
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MC.CONDITION_DESCRIPTION");
            query.Append(" FROM MST_PRODUCT MP  INNER JOIN MST_CONDITION MC ON MP.PRODUCT_CONDITION_CD = MC.CONDITION_CD ");
            query.Append("WHERE MP.PRODUCT_CD = '" + Prod_CD + "'");

            //query.Append(" WHERE CHUUBAN=" + chuuban);
            // query.Append(" AND GYOUBAN=" + gouban);
            ProdCond = Convert.ToString(ExecuteScalar(query.ToString()));
            return ProdCond;
        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves the latest PROCESS_PLAN_CD associated with the specified
        /// production order and material inspection hardness details.
        /// </summary>
        /// <param name="chuuban">Production order number.</param>
        /// <param name="gyouban">Production order line number.</param>
        /// <param name="charge">Charge number.</param>
        /// <param name="matDimD">Material dimension D (currently not used).</param>
        /// <param name="matNameMaker">Material maker name (currently not used).</param>
        /// <returns>
        /// Returns the highest PROCESS_PLAN_CD matching the specified CHUUBAN,
        /// GYOUBAN, and CHARGE with a corresponding record in TRN_MI_HARDNESS.
        /// Returns 0 if no matching value is found, subject to ExecuteScalar()
        /// and conversion behavior.
        /// </returns>
        public long GetProdOrderNoInspectionHardness(string chuuban, short gyouban, string charge, string matDimD, string matNameMaker)
        {
            long processPlanCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT PROCESS_PLAN_CD FROM (");
            query.Append(" SELECT TRN_PROCESS_PLAN.PROCESS_PLAN_CD FROM  TRN_PROCESS_PLAN ");
            query.Append(" INNER JOIN TRN_MI_HARDNESS ON TRN_MI_HARDNESS.PROCESS_PLAN_CD = TRN_PROCESS_PLAN.PROCESS_PLAN_CD");
            query.Append(" WHERE CHUUBAN = '" + chuuban + "'");
            query.Append(" AND GYOUBAN = '" + gyouban + "'");
            query.Append(" AND CHARGE='" + charge + "'");
            query.Append(" ORDER BY TRN_PROCESS_PLAN.PROCESS_PLAN_CD DESC");
            query.Append(" ) WHERE ROWNUM = 1");
            processPlanCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return processPlanCD;
        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves the PRODUCT_CD for the specified product specification numbers
        /// using the maximum available version.
        /// </summary>
        /// <param name="ProdSpecNo1">First product specification number.</param>
        /// <param name="ProdSpecNo2">Second product specification number.</param>
        /// <param name="ProdVer">Product version (currently not used).</param>
        /// <returns>
        /// Returns the PRODUCT_CD associated with the maximum VER for the specified
        /// PROD_SPEC_NO1 and PROD_SPEC_NO2.
        /// Returns 0 if no matching value is found, subject to ExecuteScalar()
        /// and conversion behavior.
        /// </returns>

        public long GetProductCD(string ProdSpecNo1, string ProdSpecNo2, string ProdVer)
        {
            long prodCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT PRODUCT_CD FROM ");
            query.Append(" MST_PRODUCT ");
            query.Append(" WHERE VER = ( SELECT MAX(VER) FROM MST_PRODUCT  WHERE PROD_SPEC_NO1 = '" + ProdSpecNo1 + "' AND PROD_SPEC_NO2 = '" + ProdSpecNo2 + "')");
            query.Append(" AND PROD_SPEC_NO1 = '" + ProdSpecNo1 + "' AND PROD_SPEC_NO2 = '" + ProdSpecNo2 + "' ");
            prodCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return prodCD;
        }
        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves the latest PROCESS_PLAN_CD associated with the specified
        /// production order and tensile strength inspection details.
        /// </summary>
        /// <param name="chuuban">Production order number.</param>
        /// <param name="gyouban">Production order line number.</param>
        /// <param name="charge">Charge number.</param>
        /// <param name="matDimD">Material dimension D (currently not used).</param>
        /// <param name="matNameMaker">Material maker name (currently not used).</param>
        /// <returns>
        /// Returns the highest PROCESS_PLAN_CD matching the specified CHUUBAN,
        /// GYOUBAN, and CHARGE with a corresponding record in TRN_MI_TENSILE_STRENGTH.
        /// Returns 0 if no matching value is found, subject to ExecuteScalar()
        /// and conversion behavior.
        /// </returns>
        public long GetProdOrderNoInspectionTensile(string chuuban, short gyouban, string charge, string matDimD, string matNameMaker)
        {
            long processPlanCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT PROCESS_PLAN_CD FROM (");
            query.Append(" SELECT TRN_PROCESS_PLAN.PROCESS_PLAN_CD FROM  TRN_PROCESS_PLAN ");
            query.Append(" INNER JOIN TRN_MI_TENSILE_STRENGTH ON TRN_MI_TENSILE_STRENGTH.PROCESS_PLAN_CD = TRN_PROCESS_PLAN.PROCESS_PLAN_CD");
            query.Append(" WHERE CHUUBAN = '" + chuuban + "'");
            query.Append(" AND GYOUBAN = '" + gyouban + "'");
            query.Append(" AND CHARGE='" + charge + "'");
            query.Append(" ORDER BY TRN_PROCESS_PLAN.PROCESS_PLAN_CD DESC");
            query.Append(" ) WHERE ROWNUM = 1");
            processPlanCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return processPlanCD;
        }
        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves post heat treatment inspection details from TRN_INTERNAL_QUALITY
        /// for the specified process plan.
        /// </summary>
        /// <param name="ProcessPlanCD">Process plan code used to retrieve internal quality inspection details.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing ACT_DM_F, ACT_DM_T_AVG,
        /// INT_QLTY_TEST_DATE, and INTERNAL_QUALITY_CD for the specified PROCESS_PLAN_CD.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>
        public List<HSKMillSheetDetails> GetHSKMillSheetPostHTInspectionDetails(string ProcessPlanCD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ACT_DM_F,ACT_DM_T_AVG,INT_QLTY_TEST_DATE,INTERNAL_QUALITY_CD");
            query.Append(" FROM TRN_INTERNAL_QUALITY ");
            query.Append(" WHERE PROCESS_PLAN_CD=" + ProcessPlanCD);
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;

        }
        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves process plan details required for the HSK mill sheet
        /// using the specified process plan code.
        /// </summary>
        /// <param name="ProcessPlanCD">Process plan code used to retrieve process plan and related master data.</param>
        /// <returns>
        /// Returns an HSKMillSheetDetails object containing product information,
        /// customer and receiver details, maker information, production specifications,
        /// dimensions, tolerances, delivery details, production process information,
        /// and inspection test patterns from TRN_PROCESS_PLAN and related master tables.
        /// Retrieves the record matching the specified PROCESS_PLAN_CD where
        /// PROCESS_PLAN_CMPL_DATE is NULL.
        /// </returns>
        public HSKMillSheetDetails GetHSKMillSheetProcessPlanCD(string ProcessPlanCD)
        {
            HSKMillSheetDetails hskmillsheetDetails = new HSKMillSheetDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.CHUUBAN ");
            selectQuery.Append(" ,TO_CHAR(PP.GYOUBAN, '00')AS GYOUBAN_STRING ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,CUS.YOBINA AS PP_YOBINA ");
            selectQuery.Append(" ,CUS.CUSTOMER_CD AS PP_CUSTOMER_CD ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,MM.MAKER_CD AS PP_MAKER_CD ");
            selectQuery.Append(" ,MM.MAKER_NAME AS PP_MAKER_NAME ");
            // selectQuery.Append(" ,get_prod_condition(pp.ver, pp.PROD_SPEC_NO1, pp.PROD_SPEC_NO2) AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS PP_EMPLOYEE_NAME ");
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
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR_ID ");
            // selectQuery.Append(" , ( SELECT  PR.TEST_SPEC_REMARKS  FROM MST_PRODUCT PR WHERE PR.PRODUCT_CD = PP.PRODUCT_CD ) AS PP_JIS_TEST_SPEC  ");     // AND PR.VER = PP.PROD_VER   2019-10-02 Pradeep M.
            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO1  AS PP_PROD_SPEC_NO1 ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO2  AS PP_PROD_SPEC_NO2 ");
            selectQuery.Append(" ,PP.VER  AS PP_PROD_VER ");

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
            //   selectQuery.Append(", NY.PRODUCT_NYUKO_CD");
            //selectQuery.Append(" , PP.NO_OF_UNITS || " + Utils.GetProdUnitForDBQuery("PP.UNIT") + " AS PP_NO_OF_UNITS_DECODE ");    //GetMatlUnitForDBQuery
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE  ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.ACT_ALLOC_DATE AS PP_ACT_ALLOC_DATE_STRING ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_INTERNAL)  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_WINDING) AS PP_COIL_DIA_WINDING ");
            //  selectQuery.Append(" ,K.REMARKS AS  PP_KAKOUIRAI_REMARKS  ");           //PP_KAKOUIRAIREMARKS
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK, ");

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
            selectQuery.Append("PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_MAKER MM ON MM.MAKER_CD = PP.MAKER_CD");
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


            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" WHERE PROCESS_PLAN_CD=" + ProcessPlanCD);
            hskmillsheetDetails = ExecuteQuerySingleResult<HSKMillSheetDetails>(selectQuery.ToString());
            return hskmillsheetDetails;

        }
        /// <summary>
        /// Created By Shobhana Unde 2025/06/24
        /// </summary>
        /// <param CD="postHTCD"></param>
        /// <returns>
        /// 1) Get Post HTInspection Details on HSK MIllSheet screen
        /// 2) Get DMF & DMT value
        /// </returns>
        public List<HSKMillSheetDetails> GetPostHTInspectionDetailsByPostHTCD(string postHTCD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();
            int dmf = (int)StandardDecarb.DMF;
            int dmt = (int)StandardDecarb.DMT;
            query.Append("SELECT ");
            query.Append("(");
            query.Append("CASE ");
            query.Append("WHEN PP.DECARB_1 = " + dmf); // DMF val:1
            query.Append("THEN PP.DECARB_VAL_1 ");
            query.Append("WHEN PP.DECARB_2 = " + dmf);
            query.Append("THEN PP.DECARB_VAL_2 ");
            query.Append("WHEN PP.DECARB_3 = " + dmf);
            query.Append("THEN PP.DECARB_VAL_3 ");
            query.Append("ELSE NULL ");
            query.Append("END ");
            query.Append(") AS STD_DMF, ");
            query.Append("(");
            query.Append("CASE ");
            query.Append("WHEN PP.DECARB_1 = " + dmt); // DMT val:3
            query.Append("THEN PP.DECARB_VAL_1 ");
            query.Append("WHEN PP.DECARB_2 = " + dmt);
            query.Append("THEN PP.DECARB_VAL_2 ");
            query.Append("WHEN PP.DECARB_3 = " + dmt);
            query.Append("THEN PP.DECARB_VAL_3 ");
            query.Append("ELSE NULL ");
            query.Append("END ");
            query.Append(") AS STD_DMT, ");
            query.Append("CASE ");
            query.Append("WHEN GET_PP_STD_DMF(PP.PRODUCT_CD) > 0 THEN ");
            query.Append("'≦' || GET_PP_STD_DMF(PP.PRODUCT_CD) * 100 ");
            query.Append("ELSE TO_CHAR(GET_PP_STD_DMF(PP.PRODUCT_CD) * 100) ");
            query.Append("END AS DMF_VAL, ");
            query.Append("CASE ");
            query.Append("WHEN GET_PP_STD_DMT(PP.PRODUCT_CD) > 0 THEN ");
            query.Append("'≦' || GET_PP_STD_DMT(PP.PRODUCT_CD) * 100 ");
            query.Append("ELSE TO_CHAR(GET_PP_STD_DMT(PP.PRODUCT_CD) * 100) ");
            query.Append("END AS DMT_VAL ");
            query.Append("FROM TRN_INTERNAL_QUALITY IQ ");
            query.Append("LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = IQ.PROCESS_PLAN_CD ");
            query.Append("WHERE IQ.INTERNAL_QUALITY_CD =" + postHTCD);
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;

        }
        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves final inspection details from TRN_FINAL_INSPECTION
        /// for the specified process plan.
        /// </summary>
        /// <param name="ProcessPlanCD">Process plan code used to retrieve final inspection details.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing FI_TABA_NO,
        /// FINAL_INSP_CD, FI_LENGTH, FI_DIM1, FI_DIM2, FI_DIM3, and
        /// FI_ACT_RADIUS_1 through FI_ACT_RADIUS_6 for the specified PROCESS_PLAN_CD.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>
        public List<HSKMillSheetDetails> GetHSKMillSheetFinalInspectionDetails(string ProcessPlanCD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT FI_TABA_NO,FINAL_INSP_CD,FI_LENGTH,FI_DIM1,FI_DIM2,FI_DIM3,FI_ACT_RADIUS_1,FI_ACT_RADIUS_2,FI_ACT_RADIUS_3,FI_ACT_RADIUS_4,FI_ACT_RADIUS_5,FI_ACT_RADIUS_6");
            query.Append(" FROM TRN_FINAL_INSPECTION ");
            query.Append(" WHERE PROCESS_PLAN_CD=" + ProcessPlanCD);
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;
        }
        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves final inspection details and standard dimensional tolerances
        /// for the specified final inspection code.
        /// </summary>
        /// <param name="finalInspCD">Final inspection code used to retrieve inspection details.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing final inspection dimensions,
        /// actual radius measurements, standard minimum and maximum dimensions,
        /// corner radius, length limits, and dimensional tolerances.
        /// Joins TRN_FINAL_INSPECTION with TRN_PROCESS_PLAN using PROCESS_PLAN_CD,
        /// filters records by FINAL_INSP_CD, and sorts the results by FI_TABA_NO.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>
        public List<HSKMillSheetDetails> GetFinalInspectionDetailsByPostFinalInspCD(string finalInspCD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT ");
            query.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            query.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            query.Append(" FI.FI_TABA_NO, ");

            query.Append(" FI.FI_DIM1, ");
            query.Append(" FI.FI_DIM2, ");
            query.Append(" FI.FI_DIM3, ");
            query.Append(" FI.FI_LENGTH, ");
            query.Append(" GET_STD_MAX_DIM_D(FI.PROCESS_PLAN_CD) AS MAX_DIMD, ");
            query.Append(" (CASE WHEN GET_CORNER_RADIUS(FI.PROCESS_PLAN_CD) IS NULL ");
            query.Append("       THEN NULL ELSE '≦ R' || GET_CORNER_RADIUS(FI.PROCESS_PLAN_CD) END) AS CORNER_RADIUS, ");
            query.Append(" GET_STD_MIN_DIM_D(FI.PROCESS_PLAN_CD) AS MIN_DIMD, ");
            query.Append(" FI.FI_ACT_RADIUS_1, ");
            query.Append(" FI.FI_ACT_RADIUS_2, ");
            query.Append(" FI.FI_ACT_RADIUS_3, ");
            query.Append(" FI.FI_ACT_RADIUS_4, ");
            query.Append(" FI.FI_ACT_RADIUS_5, ");
            query.Append(" FI.FI_ACT_RADIUS_6, ");
            query.Append(" FI.FINAL_INSP_CD ,");
            query.Append(" GET_STD_MAX_LENGTH(FI.PROCESS_PLAN_CD) AS FI_MAX_LENGTH, ");
            query.Append(" GET_STD_MIN_LENGTH(FI.PROCESS_PLAN_CD) AS FI_MIN_LENGTH ");
            query.Append(" FROM TRN_FINAL_INSPECTION FI ");
            query.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            query.Append(" WHERE FI.FINAL_INSP_CD = '" + finalInspCD + "' ");
            query.Append(" ORDER BY FI.FI_TABA_NO ");
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;

            //window query
            /*  SELECT
                 TPO.dim_tol_1, 
                 TPO.dim_tol_2, 
                 FI.FI_TABA_NO, 
                 FI.FI_ACT_DIM1, 
                 FI.FI_ACT_DIM2, 
                 FI.FI_ACT_DIM3, 
                 FI.FI_ACT_LENGTH,
                 GET_STD_MAX_DIM_D(FI.PROD_ORDER_NO) AS MAX_DIMD,
                 (CASE
                     WHEN GET_CORNER_RADIUS(FI.PROD_ORDER_NO) IS NULL
                     THEN NULL
                     ELSE '≦ R' || GET_CORNER_RADIUS(FI.PROD_ORDER_NO)
                  END) AS CORNER_RADIUS,
                 GET_STD_MIN_DIM_D(FI.PROD_ORDER_NO) AS MIN_DIMD,
                 FI.FI_ACT_RADIUS_1, 
                 FI.FI_ACT_RADIUS_2, 
                 FI.FI_ACT_RADIUS_3,
                 FI.FI_ACT_RADIUS_4, 
                 FI.FI_ACT_RADIUS_5, 
                 FI.FI_ACT_RADIUS_6,
                 FI.FINAL_INSP_NO,
                 GET_STD_MAX_LENGTH(FI.PROD_ORDER_NO) AS FI_MAX_LENGTH,
                 GET_STD_MIN_LENGTH(FI.PROD_ORDER_NO) AS FI_MIN_LENGTH
                 FROM
                 TRN_FINAL_INSPECTION FI
                 INNER JOIN
                 TRN_PROD_ORDER TPO ON FI.PROD_ORDER_NO = TPO.PROD_ORDER_NO
                 WHERE
                 FI.FINAL_INSP_NO = '7026'
                 ORDER BY
                 FI.FI_TABA_NO;
            */

        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves hardness and tensile strength inspection details for the specified
        /// process plan and inspection detail number, including standard test values
        /// and actual measurement results.
        /// </summary>
        /// <param name="prodOrderNo">Process plan code used to retrieve inspection details.</param>
        /// <param name="DetailNo">Inspection detail number used to filter hardness and tensile strength records.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing hardness standards and actual
        /// values, tensile strength, elongation, reduction, yield strength, impact test
        /// results, dimensional tolerances, test specimen details, and hardness measurements.
        /// Combines TRN_MI_HARDNESS and TRN_MI_TENSILE_STRENGTH data using FULL JOIN
        /// and retrieves matching records from TRN_PROCESS_PLAN and related master tables.
        /// Uses UNION to combine results from both query sections.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>

        public List<HSKMillSheetDetails> LoadHardnessPattern1DetailNoData(long prodOrderNo, int DetailNo)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT * FROM (");
            query.Append(" SELECT TPO.CHUUBAN AS CHUUBAN, TPO.GYOUBAN AS GYOUBAN, TH.MI_HARDNESS_CD AS HD_KEY, ");
            query.Append(" TTS.MI_TS_CD AS TS_KEY, ( CASE ");
            query.Append("  WHEN HARDNESS_PATTERN1_CODE IS NOT NULL  AND HARDNESS_PATTERN1_MIN_MAX = 1 THEN 'MAX'");
            query.Append(" WHEN HARDNESS_PATTERN1_CODE IS NOT NULL  AND HARDNESS_PATTERN1_MIN_MAX = 2 THEN 'MIN' ");
            query.Append(" WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MIN) END ");
            query.Append(" ) AS HD_STD_ACT_2, ");
            query.Append(" ( CASE WHEN HARDNESS_PATTERN1_CODE IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN1) ");
            query.Append(" WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MAX) END ");
            query.Append(" ) AS HD_STD_ACT_3, ");
            query.Append(" TTS.TS_CUT_DIM, TTS.TEST_SPECIMEN_CD, ");
            query.Append(" TPO.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            query.Append(" TPO.DIM_TOL_MIN AS  PP_DIM_TOL_MIN, ");
            query.Append(" TPO.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            query.Append(" TPO.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            query.Append(" GET_FORMATTED_VALUE(TPO.TENSILE_STRN_PATTERN1_MIN_MAX, TPO.TENSILE_STRN_PATTERN1, TPO.TENSILE_STRN_PATTERN3_ACTUAL) AS TENSILE_STRENGTH_PATTERN, ");
            query.Append(" GET_FORMATTED_VALUE2(TPO.TENSILE_STRN_PATTERN2_MIN, TPO.TENSILE_STRN_PATTERN2_MAX) AS TENSILE_STRENGTH_PATTERN2,");
            query.Append(" GET_FORMATTED_VALUE(TPO.YEILD_PATTERN1_MIN_MAX, TPO.YEILD_PATTERN1, TPO.YEILD_PATTERN3_ACTUAL) AS YEILD_PATTERN, ");
            query.Append(" GET_FORMATTED_VALUE(TPO.PRESS_PATTERN1_MIN_MAX, TPO.PRESS_PATTERN1, TPO.PRESS_PATTERN3_ACTUAL) AS REDUCTION_1, ");
            query.Append(" GET_FORMATTED_VALUE2(TPO.PRESS_PATTERN2_MIN, TPO.PRESS_PATTERN2_MAX) AS REDUCTION_2, ");
            query.Append(" GET_FORMATTED_VALUE(TPO.ELONGATION_PATTERN1_MIN_MAX, TPO.ELONGATION_PATTERN1, TPO.ELONGATION_PATTERN3_ACTUAL) AS ELONGATION_1, ");
            query.Append(" GET_FORMATTED_VALUE2(TPO.ELONGATION_PATTERN2_MIN, TPO.ELONGATION_PATTERN2_MAX) AS ELONGATION_2, ");
            query.Append(" CASE WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 10  THEN 'HRB'");
            query.Append("  WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 20  THEN  'HRC' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 30  THEN  'HV'");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 40  THEN  'HB' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 50  THEN  'HRA'");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 60  THEN  'HRD' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 70  THEN  '実績値' ELSE '実績値'");
            query.Append(" END AS HARDNESS_1, ");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_CODE IS NOT NULL THEN");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_MIN_MAX = 2 THEN '≧' || HARDNESS_PATTERN1 END ");
            query.Append(" ELSE CASE WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN");
            query.Append(" CASE WHEN HARDNESS_PATTERN2_MIN IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MIN) END END ");
            query.Append(" END AS HARDNESS_PATTERN12,");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_CODE IS NOT NULL THEN ");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_MIN_MAX = 1 THEN");
            query.Append(" '≦' || HARDNESS_PATTERN1 END ");
            query.Append(" ELSE CASE WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN");
            query.Append(" CASE WHEN HARDNESS_PATTERN2_MAX IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MAX) END END ");
            query.Append(" END AS HARDNESS_PATTERN13,");
            query.Append(" MTS.JIS_TEST_SPEC_SHORT   AS TEST_SPECIMEN, ");
            query.Append(" CASE WHEN TTS.TS_ACT_DIM IS NOT NULL THEN TO_CHAR(TTS.TS_ACT_DIM, '999.999') ");
            query.Append(" WHEN TH.HD_ACT_DIM IS NOT NULL THEN TO_CHAR(TH.HD_ACT_DIM, '999.999') ELSE NULL END AS ACT_DIM, ");
            query.Append(" CASE WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 10  THEN 'HRB' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 20  THEN  'HRC' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 30  THEN  'HV' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 40  THEN  'HB' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 50  THEN  'HRA'");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 60  THEN  'HRD' ");
            query.Append(" WHEN ( NVL(HARDNESS_PATTERN1_CODE, HARDNESS_PATTERN2_CODE) ) = 70  THEN  '実績値'");
            query.Append(" ELSE  '実績値' END AS HD_PATTERN_ACT, ");
            query.Append(" ( ROUND(HD_D4_1, 1) ) AS D4_1,");
            query.Append(" ( ROUND(HD_D4_2) ) AS D4_2, ");
            query.Append(" ( ROUND(HD_D4_3, 1) ) AS D4_3, ");
            query.Append(" ( ROUND(HD_D4_4, 1) ) AS D4_4, ");
            query.Append(" TH.HD_D4_AVG AS HD_D4_AVG, ");
            query.Append(" ( CASE WHEN TTS.TS_TENSILE = 0 THEN NULL ELSE TO_CHAR(ROUND(TTS.TS_TENSILE), '9990') END ) AS TS_RESULT, ");
            query.Append(" (  CASE WHEN TS_ELONGATION = 0 THEN NULL ELSE TO_CHAR(TS_ELONGATION, '90.0') END ) AS ELONG_RESULT, ");
            query.Append(" (  CASE WHEN TS_REDUCTION = 0 THEN  NULL ELSE TO_CHAR(TS_REDUCTION, '90.0')  END ) AS REDUCT_RESULT, ");
            query.Append(" (  CASE WHEN TS_YIELD_STRENGTH = 0 THEN  NULL ELSE TO_CHAR(TS_YIELD_STRENGTH, '9990') END ) AS YEILD_ACTUAL,");
            query.Append(" NULL AS TEST_IMPACT_STD, ");
            query.Append(" TTS.TS_TEST_IMPACT_1 AS TS_TEST_IMPACT_1,");
            query.Append(" TTS.TS_TEST_IMPACT_2 AS TS_TEST_IMPACT_2, ");
            query.Append(" TTS.TS_TEST_IMPACT_3 AS TS_TEST_IMPACT_3,");
            query.Append(" TTS.TS_MAX_LOAD_N ");
            query.Append(" FROM TRN_MI_HARDNESS TH");
            query.Append(" FULL JOIN TRN_MI_TENSILE_STRENGTH   TTS ON TH.PROCESS_PLAN_CD = TTS.PROCESS_PLAN_CD AND TH.DETAIL_NO = TTS.DETAIL_NO ");
            query.Append(" INNER JOIN MST_EMPLOYEE MES ON TH.HD_TEST_OPERATOR_CD = MES.EMPLOYEE_CD");
            query.Append(" LEFT JOIN MST_EMPLOYEE MEE ON TTS.TS_TEST_OPERATOR_CD = MEE.EMPLOYEE_CD ");
            query.Append(" LEFT OUTER JOIN MST_JIS_TEST_SPEC MTS ON TTS.TEST_SPECIMEN_CD = MTS.JIS_TEST_SPEC_CD ");
            query.Append(" INNER JOIN TRN_PROCESS_PLAN TPO ON TH.PROCESS_PLAN_CD = TPO.PROCESS_PLAN_CD ");
            query.Append(" WHERE 1 = 1 AND TPO.PROCESS_PLAN_CD = "+ prodOrderNo  +" AND TH.DETAIL_NO = " + DetailNo +")");
            query.Append(" UNION ");
            query.Append(" ( SELECT TPO.CHUUBAN AS CHUUBAN, TPO.GYOUBAN AS GYOUBAN, TH.MI_HARDNESS_CD AS HD_KEY, ");
            query.Append(" TTS.MI_TS_CD AS TS_KEY, ( CASE ");
            query.Append("  WHEN HARDNESS_PATTERN1_CODE IS NOT NULL  AND HARDNESS_PATTERN1_MIN_MAX = 1 THEN 'MAX'");
            query.Append(" WHEN HARDNESS_PATTERN1_CODE IS NOT NULL  AND HARDNESS_PATTERN1_MIN_MAX = 2 THEN 'MIN' ");
            query.Append(" WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MIN) END ");
            query.Append(" ) AS HD_STD_ACT_2, ");
            query.Append(" (CASE WHEN HARDNESS_PATTERN1_CODE IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN1) ");
            query.Append(" WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MAX) END ");
            query.Append(" ) AS HD_STD_ACT_3, ");
            query.Append(" TTS.TS_CUT_DIM, TTS.TEST_SPECIMEN_CD, ");
            query.Append(" TPO.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            query.Append(" TPO.DIM_TOL_MIN AS  PP_DIM_TOL_MIN, ");
            query.Append(" TPO.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            query.Append(" TPO.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            query.Append(" GET_FORMATTED_VALUE(TPO.TENSILE_STRN_PATTERN1_MIN_MAX, TPO.TENSILE_STRN_PATTERN1, TPO.TENSILE_STRN_PATTERN3_ACTUAL) AS TENSILE_STRENGTH_PATTERN, ");
            query.Append(" GET_FORMATTED_VALUE2(TPO.TENSILE_STRN_PATTERN2_MIN, TPO.TENSILE_STRN_PATTERN2_MAX) AS TENSILE_STRENGTH_PATTERN2,");
            query.Append(" GET_FORMATTED_VALUE(TPO.YEILD_PATTERN1_MIN_MAX, TPO.YEILD_PATTERN1, TPO.YEILD_PATTERN3_ACTUAL) AS YEILD_PATTERN, ");
            query.Append(" GET_FORMATTED_VALUE(TPO.PRESS_PATTERN1_MIN_MAX, TPO.PRESS_PATTERN1, TPO.PRESS_PATTERN3_ACTUAL) AS REDUCTION_1, ");
            query.Append(" GET_FORMATTED_VALUE2(TPO.PRESS_PATTERN2_MIN, TPO.PRESS_PATTERN2_MAX) AS REDUCTION_2, ");
            query.Append(" GET_FORMATTED_VALUE(TPO.ELONGATION_PATTERN1_MIN_MAX, TPO.ELONGATION_PATTERN1, TPO.ELONGATION_PATTERN3_ACTUAL) AS ELONGATION_1, ");
            query.Append(" GET_FORMATTED_VALUE2(TPO.ELONGATION_PATTERN2_MIN, TPO.ELONGATION_PATTERN2_MAX) AS ELONGATION_2, ");
            query.Append(" CASE WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 10  THEN 'HRB'");
            query.Append("  WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 20  THEN  'HRC' ");
            query.Append(" WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 30  THEN  'HV'");
            query.Append(" WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 40  THEN  'HB' ");
            query.Append(" WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 50  THEN  'HRA'");
            query.Append(" WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 60  THEN  'HRD' ");
            query.Append(" WHEN ( NVL(TPO.HARDNESS_PATTERN1_CODE, TPO.HARDNESS_PATTERN2_CODE) ) = 70  THEN  '実績値' ELSE '実績値'");
            query.Append(" END AS HARDNESS_1, ");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_CODE IS NOT NULL THEN");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_MIN_MAX = 2 THEN '≧' || HARDNESS_PATTERN1 END ");
            query.Append(" ELSE CASE WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN");
            query.Append(" CASE WHEN HARDNESS_PATTERN2_MIN IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MIN) END END ");
            query.Append(" END AS HARDNESS_PATTERN12,");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_CODE IS NOT NULL THEN ");
            query.Append(" CASE WHEN HARDNESS_PATTERN1_MIN_MAX = 1 THEN");
            query.Append(" '≦' || HARDNESS_PATTERN1 END ");
            query.Append(" ELSE CASE WHEN HARDNESS_PATTERN2_CODE IS NOT NULL THEN");
            query.Append(" CASE WHEN HARDNESS_PATTERN2_MAX IS NOT NULL THEN TO_CHAR(HARDNESS_PATTERN2_MAX) END END ");
            query.Append(" END AS HARDNESS_PATTERN13,");
            query.Append(" MTS.JIS_TEST_SPEC_SHORT   AS TEST_SPECIMEN, ");
            query.Append(" CASE WHEN TTS.TS_ACT_DIM IS NOT NULL THEN TO_CHAR(TTS.TS_ACT_DIM, '999.999') ");
            query.Append(" WHEN TH.HD_ACT_DIM IS NOT NULL THEN TO_CHAR(TH.HD_ACT_DIM, '999.999') ELSE NULL END AS ACT_DIM, ");
            query.Append(" CASE WHEN ( TH.HD_PATTERN_ACT ) = 10  THEN 'HRB' ");
            query.Append(" WHEN ( TH.HD_PATTERN_ACT ) = 20  THEN  'HRC' ");
            query.Append(" WHEN ( TH.HD_PATTERN_ACT ) = 30  THEN  'HV' ");
            query.Append(" WHEN ( TH.HD_PATTERN_ACT ) = 40  THEN  'HB' ");
            query.Append(" WHEN ( TH.HD_PATTERN_ACT ) = 50  THEN  'HRA'");
            query.Append(" WHEN ( TH.HD_PATTERN_ACT ) = 60  THEN  'HRD' ");
            query.Append(" WHEN ( TH.HD_PATTERN_ACT ) = 70  THEN  '実績値'");
            query.Append(" ELSE  '実績値' END AS HD_PATTERN_ACT, ");
            query.Append(" ( ROUND(HD_D4_1, 1) ) AS D4_1,");
            query.Append(" ( ROUND(HD_D4_2) ) AS D4_2, ");
            query.Append(" ( ROUND(HD_D4_3, 1) ) AS D4_3, ");
            query.Append(" ( ROUND(HD_D4_4, 1) ) AS D4_4, ");
            query.Append(" TH.HD_D4_AVG AS HD_D4_AVG, ");
            query.Append(" ( CASE WHEN TTS.TS_TENSILE = 0 THEN NULL ELSE TO_CHAR(ROUND(TTS.TS_TENSILE), '9990') END ) AS TS_RESULT, ");
            query.Append(" (  CASE WHEN TS_ELONGATION = 0 THEN NULL ELSE TO_CHAR(TS_ELONGATION, '90.0') END ) AS ELONG_RESULT, ");
            query.Append(" (  CASE WHEN TS_REDUCTION = 0 THEN  NULL ELSE TO_CHAR(TS_REDUCTION, '90.0')  END ) AS REDUCT_RESULT, ");
            query.Append(" (  CASE WHEN TS_YIELD_STRENGTH = 0 THEN  NULL ELSE TO_CHAR(TS_YIELD_STRENGTH, '9990') END ) AS YEILD_ACTUAL,");
            query.Append(" NULL AS TEST_IMPACT_STD, ");
            query.Append(" TTS.TS_TEST_IMPACT_1 AS TS_TEST_IMPACT_1,");
            query.Append(" TTS.TS_TEST_IMPACT_2 AS TS_TEST_IMPACT_2, ");
            query.Append(" TTS.TS_TEST_IMPACT_3 AS TS_TEST_IMPACT_3,");
            query.Append(" TTS.TS_MAX_LOAD_N ");
            query.Append(" FROM TRN_MI_TENSILE_STRENGTH   TTS");
            query.Append(" FULL JOIN TRN_MI_HARDNESS TH ON TTS.PROCESS_PLAN_CD = TH.PROCESS_PLAN_CD AND TTS.DETAIL_NO = TH.DETAIL_NO ");
            query.Append(" LEFT JOIN MST_EMPLOYEE MES ON TH.HD_TEST_OPERATOR_CD = MES.EMPLOYEE_CD");
            query.Append(" INNER JOIN MST_EMPLOYEE MEE ON TTS.TS_TEST_OPERATOR_CD = MEE.EMPLOYEE_CD ");
            query.Append(" LEFT OUTER JOIN MST_JIS_TEST_SPEC MTS ON TTS.TEST_SPECIMEN_CD = MTS.JIS_TEST_SPEC_CD ");
            query.Append(" INNER JOIN TRN_PROCESS_PLAN TPO ON TTS.PROCESS_PLAN_CD = TPO.PROCESS_PLAN_CD ");
            query.Append(" WHERE 1 = 1 AND TPO.PROCESS_PLAN_CD = " + prodOrderNo + " AND TH.DETAIL_NO = " + DetailNo + ")");
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;

        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves live process plan records for the specified production order number
        /// and line number to support HSK mill sheet grid search.
        /// </summary>
        /// <param name="chuuban">Production order number used to search process plan records.</param>
        /// <param name="gyouban">Production order line number used to filter process plan records.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing production order information,
        /// process plan code, customer details, product specifications, dimensions,
        /// delivery date, allocation date, charge, maker name, completion date,
        /// and creation date.
        /// Joins TRN_PROCESS_PLAN with MST_CONDITION, MST_MAKER, and MST_CUSTOMER
        /// to retrieve related master data.
        /// Filters records by CHUUBAN and GYOUBAN and includes only incomplete process
        /// plans where PROCESS_PLAN_CMPL_DATE is NULL.
        /// Sorts the results by CREATED_DATE in descending order.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>
        public List<HSKMillSheetDetails> HSKMillSheetGridSearchChuubanLiveCheck(string chuuban, string gyouban)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            // selectQuery.Append(" TPP.HSK_MILL_SHEET_CD AS HSKMILL_SHEET_CD, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(CHUUBAN_YEAR || CHUUBAN, GYOUBAN)AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD, ");
            selectQuery.Append(" PP.CUSTOMER_CD, ");
            selectQuery.Append(" CUST.CUSTOMER_NAME          AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST, ");
            selectQuery.Append(" PP.PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_LENGTH, ");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION   AS PP_PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS DELIVERY_DATE, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" M.MAKER_NAME AS PP_MAKER_NAME_SHORT, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE, 'YYYY/MM/DD') AS PP_PROCESS_PLAN_CMPL_DATE_STRING, ");
            selectQuery.Append(" PP.CHUUBAN AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.GYOUBAN, ");
            selectQuery.Append(" PP.CREATED_DATE ");
            selectQuery.Append("FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("LEFT JOIN MST_CONDITION      CND ON CND.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append("LEFT JOIN MST_MAKER          M ON M.MAKER_CD = PP.MAKER_CD ");
            selectQuery.Append("LEFT JOIN MST_CUSTOMER       CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");


            selectQuery.Append(" WHERE ");

            selectQuery.Append(" PP.CHUUBAN = '" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN = '" + gyouban + "' ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS  NULL ");
            selectQuery.Append(" ORDER BY CREATED_DATE DESC ");

            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(selectQuery.ToString());
            return hskmillsheetDetails;

        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves the default mill sheet remarks for the specified product code.
        /// </summary>
        /// <param name="Prod_CD">Product code used to retrieve the default mill sheet remarks.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing the seven default mill sheet
        /// remarks (MILL_SHEET_DEFAULT_REMARK1 through MILL_SHEET_DEFAULT_REMARK7)
        /// from MST_PRODUCT for the specified PRODUCT_CD.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>
        public List<HSKMillSheetDetails> GetProductDetails(long Prod_CD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT  ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK1 AS MILL_SHEET_REMARK_1, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK2 AS MILL_SHEET_REMARK_2, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK3 AS MILL_SHEET_REMARK_3, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK4 AS MILL_SHEET_REMARK_4, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK5 AS MILL_SHEET_REMARK_5, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK6 AS MILL_SHEET_REMARK_6, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK7 AS MILL_SHEET_REMARK_7 ");
            query.Append(" FROM MST_PRODUCT");

            query.Append(" WHERE PRODUCT_CD = " + Prod_CD + "");
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;

        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves the default mill sheet remark values from the product master
        /// for the specified product code.
        /// </summary>
        /// <param name="Prod_CD">Product code used to retrieve default mill sheet values.</param>
        /// <returns>
        /// Returns a list of HSKMillSheetDetails containing the seven default mill sheet
        /// remarks (MILL_SHEET_DEFAULT_REMARK1 through MILL_SHEET_DEFAULT_REMARK7),
        /// mapped to MILL_SHEET_REMARK_1 through MILL_SHEET_REMARK_7,
        /// from MST_PRODUCT for the specified PRODUCT_CD.
        /// Returns an empty list if no matching records are found, depending on
        /// ExecuteQueryMultiResult behavior.
        /// </returns>
        public List<HSKMillSheetDetails> GetProductMasterDefaultValue(long Prod_CD)
        {
            List<HSKMillSheetDetails> hskmillsheetDetails = new List<HSKMillSheetDetails>();
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT  ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK1 AS MILL_SHEET_REMARK_1, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK2 AS MILL_SHEET_REMARK_2, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK3 AS MILL_SHEET_REMARK_3, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK4 AS MILL_SHEET_REMARK_4, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK5 AS MILL_SHEET_REMARK_5, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK6 AS MILL_SHEET_REMARK_6, ");
            query.Append(" MILL_SHEET_DEFAULT_REMARK7 AS MILL_SHEET_REMARK_7 ");
            query.Append(" FROM MST_PRODUCT");

            query.Append(" WHERE PRODUCT_CD = " + Prod_CD + "");
            hskmillsheetDetails = ExecuteQueryMultiResult<HSKMillSheetDetails>(query.ToString());
            return hskmillsheetDetails;

        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Retrieves maker mill sheet records based on the specified search criteria
        /// and sorting conditions.
        /// </summary>
        /// <param name="searchCriteria">List of search fields used to dynamically filter maker mill sheet records. Can be null or empty to retrieve all records.</param>
        /// <param name="sortCiteria">Sorting criteria used to order the results. If null or empty, the default sorting order is applied.</param>
        /// <returns>
        /// Returns a list of MakerMillSheetDetails containing records from MAKER_MILL_SHEET.
        /// Applies the specified search criteria dynamically using DynamicSearchProvider.
        /// If custom sorting criteria are provided, applies them using Helper.ApplySortCriteria;
        /// otherwise, sorts by CREATED_DATE descending, MATL_NAME_MAKER ascending,
        /// DIM_D ascending, and MAKER_NAME ascending.
        /// </returns>
        public List<MakerMillSheetDetails> GetMakerMillsheetDetails(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MM.* ");
            selectQuery.Append("  FROM MAKER_MILL_SHEET MM ");
            selectQuery.Append("     WHERE 1 = 1 ");

            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                foreach (var field in searchCriteria)
                {
                    selectQuery.Append(" AND ");
                    selectQuery.Append(DynamicSearchProvider.ApplySearchCriteria(field, "MM"));
                }
            }

            //selectQuery.Append(" AND MM.PP_CHUUBAN_GYOUBAN NOT IN (" + specialChuuban + ") ");

            if (!string.IsNullOrEmpty(sortCiteria))
                selectQuery = new StringBuilder(Helper.ApplySortCriteria(sortCiteria, selectQuery.ToString()));
            else
                selectQuery.Append(" ORDER BY CREATED_DATE DESC , MATL_NAME_MAKER ASC,DIM_D ASC ,MAKER_NAME ASC");

            return ExecuteQueryMultiResult<MakerMillSheetDetails>(selectQuery.ToString());

        }

        /// <summary>
        /// Created By Sandeep Hajare
        /// Inserts hardness inspection details and related mill sheet information
        /// into TRN_HSK_MILL_SHEET.
        /// </summary>
        /// <param name="request">HSKMillSheetDetails object containing process plan information, customer and product details, inspection references, hardness settings, mill sheet remarks, chemical element symbols, magnification values, elemental specification limits, and user information.</param>
        /// <returns>
        /// This method does not return a value.
        /// Inserts a new record into TRN_HSK_MILL_SHEET and sets CREATED_DATE
        /// and UPDATED_DATE to SYSDATE.
        /// Executes the INSERT statement using ExecuteNonQuery.
        /// </returns>
        public void InsertHardnessDetails(HSKMillSheetDetails request)
        {
            // hardness_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Hardness").Value);
            StringBuilder insertQuery = new StringBuilder();
            // try
            // {
            insertQuery.Append(" INSERT INTO TRN_HSK_MILL_SHEET(");
            insertQuery.Append(" 	HSK_MILL_SHEET_CD	");
            insertQuery.Append(" ,	PROCESS_PLAN_CD	");
            insertQuery.Append(" ,	CUSTOMER_CD	");
            insertQuery.Append(" ,	INPUT_LOCATION_CD	");
            insertQuery.Append(" ,	IS_CUSTOMER_ANOTHER_NAME	");
            insertQuery.Append(" ,	MANUAL_CUSTOMER_NAME	");
            insertQuery.Append(" ,	IS_MAKER_ANOTHER_NAME	");
            insertQuery.Append(" ,	MATL_NAME_MAK_CD	");
            insertQuery.Append(" ,	RECEIVER_NAME	");
            insertQuery.Append(" ,	PROD_NAME_CUST	");
            insertQuery.Append(" ,	PROD_CONDITION_CD	");
            insertQuery.Append(" ,	PROD_SHAPE	");
            insertQuery.Append(" ,	PROD_DIM_D	");
            insertQuery.Append(" ,	PROD_DIM_W	");
            insertQuery.Append(" ,	PROD_DIM_OTHER	");
            insertQuery.Append(" ,	PROD_LENGTH	");
            insertQuery.Append(" ,	DIMDSHOWONREPORT	");
            insertQuery.Append(" ,	MATL_LENGTH	");
            insertQuery.Append(" ,	DELIVERY_DATE	");
            insertQuery.Append(" ,	NO_OF_UNITS	");
            insertQuery.Append(" , UNIT");
            insertQuery.Append(" ,	WEIGHT	");
            insertQuery.Append(" ,	IS_MATL_DIM	");
            insertQuery.Append(" ,	IS_PROD_DIM	");
            insertQuery.Append(" ,	CHEMICAL_COMPOSITION_CD	");
            insertQuery.Append(" ,	MAKER_MILL_SHEET_CD	");
            insertQuery.Append(" ,	JISDIVIDEBYTEN	");
            insertQuery.Append(" ,	IS_PROD_DIM_TOL	");
            insertQuery.Append(" ,	IS_SHOW_COIL_NO	");
            insertQuery.Append(" ,	IS_DIMTOLSHOWONREPORT	");
            insertQuery.Append(" ,	SHOW_HARD_TENSILE	");
            insertQuery.Append(" ,	COIL_NO_1	");
            insertQuery.Append(" ,	COIL_NO_2	");
            insertQuery.Append(" ,	COIL_NO_3	");
            insertQuery.Append(" ,	COIL_NO_4	");
            insertQuery.Append(" ,	COIL_NO_5	");
            insertQuery.Append(" ,	COIL_NO_6	");
            insertQuery.Append(" , COIL_NO_7");
            insertQuery.Append(" ,	MI_HARDNESS_CD1	");
            insertQuery.Append(" ,	MI_HARDNESS_CD2	");
            insertQuery.Append(" ,	MI_HARDNESS_CD3	");
            insertQuery.Append(" ,	MI_HARDNESS_CD4	");
            insertQuery.Append(" ,	MI_HARDNESS_CD5	");
            insertQuery.Append(" ,	MI_HARDNESS_CD6	");
            insertQuery.Append(" ,	MI_HARDNESS_CD7	");
            insertQuery.Append(" ,	MI_TS_CD1	");
            insertQuery.Append(" ,	MI_TS_CD2	");
            insertQuery.Append(" ,	MI_TS_CD3	");
            insertQuery.Append(" ,	MI_TS_CD4	");
            insertQuery.Append(" ,	MI_TS_CD5	");
            insertQuery.Append(" ,	MI_TS_CD6	");
            insertQuery.Append(" ,	MI_TS_CD7	");
            insertQuery.Append(" ,	INTERNAL_QUALITY_CD	");
            insertQuery.Append(" ,	HARDNESS_METHOD	");
            insertQuery.Append(" ,	FINAL_INSP_CD	");
            insertQuery.Append(" , RANK_ACTUAL_1");
            insertQuery.Append(" ,	RANK_ACTUAL_2	");
            insertQuery.Append(" ,	RANK_ACTUAL_3	");
            insertQuery.Append(" ,	RANK_ACTUAL_4	");
            insertQuery.Append(" ,	RANK_ACTUAL_5	");
            insertQuery.Append(" ,	RANK_ACTUAL_6	");
            insertQuery.Append(" ,	RANK_ACTUAL_7	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_1	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_2	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_3	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_4	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_5	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_6	");
            insertQuery.Append(" ,	MILL_SHEET_REMARK_7	");
            insertQuery.Append(" ,	USER_ID	");
            insertQuery.Append(" , ELEMENT_SYMBOL_1 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_2 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_3 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_4 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_5 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_6 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_7 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_8 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_9 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_10 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_11 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_12 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_13 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_14 ");
            insertQuery.Append(" , ELEMENT_SYMBOL_15 ");
            insertQuery.Append(" , MAGNIFICATION_1 ");
            insertQuery.Append(" , MAGNIFICATION_2 ");
            insertQuery.Append(" , MAGNIFICATION_3 ");
            insertQuery.Append(" , MAGNIFICATION_4 ");
            insertQuery.Append(" , MAGNIFICATION_5 ");
            insertQuery.Append(" , MAGNIFICATION_6 ");
            insertQuery.Append(" , MAGNIFICATION_7 ");
            insertQuery.Append(" , MAGNIFICATION_8 ");
            insertQuery.Append(" , MAGNIFICATION_9 ");
            insertQuery.Append(" , MAGNIFICATION_10 ");
            insertQuery.Append(" , MAGNIFICATION_11 ");
            insertQuery.Append(" , MAGNIFICATION_12 ");
            insertQuery.Append(" , MAGNIFICATION_13 ");
            insertQuery.Append(" , MAGNIFICATION_14 ");
            insertQuery.Append(" , MAGNIFICATION_15 ");
            insertQuery.Append(" , ES_MIN_1 ");
            insertQuery.Append(" , ES_MIN_2 ");
            insertQuery.Append(" , ES_MIN_3 ");
            insertQuery.Append(" , ES_MIN_4 ");
            insertQuery.Append(" , ES_MIN_5 ");
            insertQuery.Append(" , ES_MIN_6 ");
            insertQuery.Append(" , ES_MIN_7 ");
            insertQuery.Append(" , ES_MIN_8 ");
            insertQuery.Append(" , ES_MIN_9 ");
            insertQuery.Append(" , ES_MIN_10 ");
            insertQuery.Append(" , ES_MIN_11 ");
            insertQuery.Append(" , ES_MIN_12 ");
            insertQuery.Append(" , ES_MIN_13 ");
            insertQuery.Append(" , ES_MIN_14 ");
            insertQuery.Append(" , ES_MIN_15 ");
            insertQuery.Append(" , ES_MAX_1 ");
            insertQuery.Append(" , ES_MAX_2 ");
            insertQuery.Append(" , ES_MAX_3 ");
            insertQuery.Append(" , ES_MAX_4 ");
            insertQuery.Append(" , ES_MAX_5 ");
            insertQuery.Append(" , ES_MAX_6 ");
            insertQuery.Append(" , ES_MAX_7 ");
            insertQuery.Append(" , ES_MAX_8 ");
            insertQuery.Append(" , ES_MAX_9 ");
            insertQuery.Append(" , ES_MAX_10 ");
            insertQuery.Append(" , ES_MAX_11 ");
            insertQuery.Append(" , ES_MAX_12 ");
            insertQuery.Append(" , ES_MAX_13 ");
            insertQuery.Append(" , ES_MAX_14 ");
            insertQuery.Append(" , ES_MAX_15 ");
            insertQuery.Append(" ,	INPUT_LOCATION_CD	");
            insertQuery.Append(" ,	CREATED_DATE	");
            insertQuery.Append(" ,	UPDATED_DATE	");
            //insertQuery.Append(" ,	HD_NEXT_PROCESS	");


            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");


            insertQuery.Append(",'" + request.PROCESS_PLAN_CD + "'");

            insertQuery.Append(",'" + request.LOCATION_CD + "'");

            //insertQuery.Append(",'" + .TS_TEST_DATE + "'");

            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            //insertQuery.Append(",'" + hardnessDetails.HD_NEXT_PROCESS + "'");//next process remove for without taba requirement
            insertQuery.Append(") ");

            ExecuteNonQuery(insertQuery.ToString());

            //}
            //    catch (Exception ex)
            //    {
            //        ex.Data.Add("sql", insertQuery.ToString());
            //        throw;
            //    }
        }


        public void SaveHSKMillSheetDetails(HSKMillSheetDetails request)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_HSK_MILL_SHEET(");
            insertQuery.Append(" 	HSK_MILL_SHEET_CD	");
            insertQuery.Append(" ,	PROCESS_PLAN_CD	");
            insertQuery.Append(" ,	CUSTOMER_CD	");
            insertQuery.Append(" ,	INPUT_LOCATION_CD	");
            insertQuery.Append(" ,	IS_CUSTOMER_ANOTHER_NAME	");
            insertQuery.Append(" ,	MANUAL_CUSTOMER_NAME	");
            insertQuery.Append(" ,	IS_MAKER_ANOTHER_NAME	");
            insertQuery.Append(" ,	MATL_NAME_MAK_CD	");
            insertQuery.Append(" ,	RECEIVER_NAME	");
            insertQuery.Append(" ,	PROD_NAME_CUST	");
            insertQuery.Append(" ,	PROD_CONDITION_CD	");
            insertQuery.Append(" ,	PROD_SHAPE	");
            insertQuery.Append(" ,	PROD_DIM_D	");
            insertQuery.Append(" ,	PROD_DIM_W	");
            insertQuery.Append(" ,	PROD_DIM_OTHER	");
            insertQuery.Append(" ,	PROD_LENGTH	");
            insertQuery.Append(" ,	DIMDSHOWONREPORT	");
            insertQuery.Append(" ,	MATL_LENGTH	");
            insertQuery.Append(" ,	DELIVERY_DATE	");
            HSKMillSheetDetails hskMillSheetDetails = new HSKMillSheetDetails();    


        }
    }
}
