using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class FinalInspectionDetails
    {
        public long FINAL_INSP_NO { get; set; }
        public long? NIFUDA_CD { get; set; }
        public long? PP_CUSTOMER_CD { get; set; }
        public long? PP_EMPLOYEE_CD { get; set; }
        public string CHUUBAN { get; set; }
        public short PP_GYOUBAN { get; set; }
        public string FI_NIFUDA_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public long CUSTOMER_CD { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public string CHARGE { get; set; }
        public string DIM_D { get; set; }
        public Single? PP_PROD_DIM_W { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        public Single? PP_DIM_TOL_MIN { get; set; }
        public string PP_GYOUBAN_STRING { get; set; }
        public Single? PP_DIM_TOL_MAX { get; set; }
        public Single? PP_PROD_LENGTH { get; set; }
        public string PP_PROD_LENGTH_OTHER { get; set; }
        public Single? PP_LENGTH_TOL_MIN { get; set; }
        public Single? PP_LENGTH_TOL_MAX { get; set; }
        public short? PP_COIL_DIA_INTERNAL { get; set; }
        public short? PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public short? PP_COIL_DIA_WINDING { get; set; }
        public string PP_PRODUCT_CONDITION { get; set; }
        public int? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public int? TENSILE_STRN_PATTERN2_MAX { get; set; }
        public int TENSILE_STRN_PATTERN3_ACTUAL { get; set; }
        public float? ELONGATION_PATTERN1 { get; set; }
        public Single? ELONGATION_PATTERN2_MIN { get; set; }
        public Single? ELONGATION_PATTERN2_MAX { get; set; }
        public int ELONGATION_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN1_CODE { get; set; }
        public int? HARDNESS_PATTERN1 { get; set; }
        public int HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public int? HARDNESS_PATTERN2_MIN { get; set; }
        public int? HARDNESS_PATTERN2_MAX { get; set; }
        public int HARDNESS_PATTERN3_ACTUAL { get; set; }
        public int? YEILD_PATTERN1 { get; set; }
        public int YEILD_PATTERN1_MIN_MAX { get; set; }
        public int? YEILD_PATTERN2_MIN { get; set; }
        public int? YEILD_PATTERN2_MAX { get; set; }
        public int YEILD_PATTERN3_ACTUAL { get; set; }
        public int? PRESS_PATTERN1 { get; set; }
        public int PRESS_PATTERN1_MIN_MAX { get; set; }
        public int? PRESS_PATTERN2_MIN { get; set; }
        public int? PRESS_PATTERN2_MAX { get; set; }
        public int PRESS_PATTERN3_ACTUAL { get; set; }
        public int DECARB_1 { get; set; }
        public int DECARB_2 { get; set; }
        public int DECARB_3 { get; set; }
        public float? DECARB_VAL_1 { get; set; }
        public float? DECARB_VAL_2 { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public Single? PP_MATL_DIM_D { get; set; }
        public string PP_MATL_DIM_D_SHAPE { get; set; }
        public string INSP_DATE_FOR_ORDER { get; set; }
        public string PP_MATL_DIM_OTHER { get; set; }
        public Single? PP_MATL_DIM_W { get; set; }
        public string PP_CHARGE { get; set; }
        public DateTime? PP_ACT_ALLOC_DATE { get; set; }
        public short? PP_UNIT { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }
        public Int16? PP_PROCESS_PLAN_KUBUN { get; set; }
        public short? PP_PROD_MNGMNT_KUBUN { get; set; }
        public short? PP_SPCL_MNGMNT_KUBUN { get; set; }
        public short? PP_STORAGE_MNGMNT_KUBUN { get; set; }
        public double? PP_KAKOUIARI_QTY { get; set; }
        public DateTime? DELIVERY_DATE { get; set; }
        public DateTime? PP_PROCESS_PLAN_PRINT_DATE { get; set; }
        public Single? PP_NO_OF_UNITS { get; set; }
        public Int32? PP_MATL_LENGTH { get; set; }
        public float? DECARB_VAL_3 { get; set; }
        public int? IMPACT_TEST_PATTERN1 { get; set; }
        public short? IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public int? IMPACT_TEST_PATTERN2_MIN { get; set; }
        public int? IMPACT_TEST_PATTERN2_MAX { get; set; }
        public short? IMPACT_TEST_PATTERN3_ACTUAL { get; set; }
        public Int16? RANK_STD { get; set; }
        public string PP_HT_CONDITION { get; set; }
        public short? PP_MATL_TYPE_CBK { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public short? HARDNESS_PATTERN2_CODE { get; set; }
        public short? PP_CHAMFER { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public string PP_PROCESS_1 { get; set; }
        public string PP_PROCESS_2 { get; set; }
        public string PP_PROCESS_3 { get; set; }
        public string PP_PROCESS_4 { get; set; }
        public string PP_PROCESS_5 { get; set; }
        public string PP_PROCESS_6 { get; set; }
        public string PP_PROCESS_7 { get; set; }
        public string PP_PROCESS_8 { get; set; }
        public string PP_PROCESS_9 { get; set; }
        public string PP_PROCESS_10 { get; set; }
        public string PP_PROCESS_11 { get; set; }
        public string PP_PROCESS_12 { get; set; }
        public string PP_PROCESS_13 { get; set; }
        public string PP_TEST_SPECIMEN { get; set; }
        public int? TENSILE_STRN_PATTERN1 { get; set; }
        public int TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public Single? PR_MAGARI_BEND_TOL { get; set; }
        public Single? NEJIRE_ANGLE { get; set; }
        public short? NEJIRE_SPAN { get; set; }
        public short? PP_UNIT_WT_KUBUN { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }
        public int? PP_UNIT_WT { get; set; }
        public int? PP_UNIT_WT_MIN { get; set; }
        public int? PP_UNIT_WT_MAX { get; set; }
        public Single? DIM_DD { get; set; }
        public string DIM_TOL { get; set; }
        public decimal? FI_DIM1 { get; set; }
        public decimal? FI_DIM2 { get; set; }
        public Single? FI_DIM3 { get; set; }
        public Single? FI_DIM4 { get; set; }
        public Single? FI_DIM_1 { get; set; }
        public Single? FI_DIM_2 { get; set; }
        public string PROD_LENGTH_TOL { get; set; }
        public decimal? FI_ACT_LENGTH { get; set; }
        public string ORG_COILS { get; set; }
        public decimal ACT_ALLOC_QTY { get; set; }
        public string TENSILE_STRN_PATTERN1_MIN_MAX_STRING { get; set; }
        public string TENSILE_STRN_PATTERN3_ACTUAL_STRING { get; set; }
        public string ELONGATION_PATTERN1_MIN_MAX_STRING { get; set; }
        public string ELONGATION_PATTERN3_ACTUAL_STRING { get; set; }
        public string HARDNESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string HARDNESS_PATTERN3_ACTUAL_STRING { get; set; }
        public string YEILD_PATTERN1_MIN_MAX_STRING { get; set; }
        public string YEILD_PATTERN3_ACTUAL_STRING { get; set; }
        public string PRESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string PRESS_PATTERN3_ACTUAL_STRING { get; set; }
        public string IMPACT_TEST_PATTERN1_MIN_MAX_STRING { get; set; }
        public string IMPACT_TEST_PATTERN3_ACTUAL_STRING { get; set; }
        public string PP_PROD_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_SPCL_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_STORAGE_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_UNIT_TYPE_STRING { get; set; }
        public string PP_DECARB_STD_1 { get; set; }
        public string PP_DECARB_STD_2 { get; set; }
        public string PP_DECARB_STD_3 { get; set; }
        public string PP_HT_CONDITION_STRING { get; set; }
        public string MATL_CONDITION_DESCRIPTION { get; set; }
        public string PP_DELIVERY_DATE_STRING { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public string PP_PROCESS_PLAN_PRINT_DATE_STR { get; set; }
        public string PP_CHAMFER_STRING { get; set; }
        public string PP_CORNER_RADIUS_STRING { get; set; }
        public string PP_MATL_TYPE_CBK_STRING { get; set; }
        public string PP_PROCESS_PLAN_KUBUN_STRING { get; set; }
        public short? UNIT_TYPE { get; set; }
        public decimal? NO_OF_UNITS { get; set; }
        public string FI_REMARK { get; set; }
        public short? FI_DIM_DECISION { get; set; }
        public string FI_DIM_DECISION_VALUE { get; set; }
        public short? FI_LENGTH_DECISION { get; set; }
        public string FI_LENGTH_DECISION_VALUE { get; set; }
        public short? FI_BEND_DECISION { get; set; }
        public short? COLOR_ID { get; set; }

        public string COLORNAME { get; set; }
        public string COLOR { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_STAMP { get; set; }
        public short? CHAMFER { get; set; }
        public short? FI_COLOR_DECISION { get; set; }
        public short? FI_CHAMFERING_DECISION { get; set; }
        public short? FI_EXT_TEXTURE1_DECISION { get; set; }
        public short? FI_EXT_TEXTURE2_DECISION { get; set; }
        public short? FI_CORNER_RADIUS { get; set; }
        public short? FI_SPARK_DECISION { get; set; }
        public string DIM_DECISION { get; set; }
        public string LENGTH_DECISION { get; set; }
        public string BEND_DECISION { get; set; }
        public string TEXTURE1_DECISION { get; set; }
        public string TEXTURE2_DECISION { get; set; }
        public short? COMBINED_DECISION { get; set; }
        public string COMBINED_DECISION_VALUE { get; set; }
        public short? MACHINABILITY { get; set; }
        public string CHUUBAN_GYOUBAN { get; set; }
        public string PROD_DIM_D_SHAPE { get; set; }
        public decimal? PROD_LENGTH { get; set; }
        public string PROD_LEN_OTHER { get; set; }
        public long? PROCESS_PLAN_CD { get; set; }
        public DateTime INSPECTED_DATE { get; set; }
        public long INSPECTED_BY { get; set; }

        // properties added for Templated EC History Grid
        public string cust_incharge { get; set; }
        public string receiver_name { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public string start_date_time { get; set; }
        public string start_operator_name { get; set; }
        public string metallic_label_code { get; set; }
        public decimal? actual_dim_1 { get; set; }
        public decimal? actual_dim_2 { get; set; }
        public string furnace_no { get; set; }
        public string is_ok { get; set; }
        public string inspection_coil_size { get; set; }
        public string shape_dim_d_len { get; set; }
        public string act_alloc_units { get; set; }
        public string end_date_time { get; set; }
        public string end_operator_name { get; set; }
        public string matl_name_maker { get; set; }
        public string bend_std { get; set; }
        public string production_process { get; set; }
        public string len_tol { get; set; }
        public string MATL_DIM_D { get; set; }
        public string visual_check { get; set; }
        public decimal? ok_bars { get; set; }
        public decimal? ok_qty { get; set; }
        public decimal? ng_qty { get; set; }
        public decimal? FI_QTY { get; set; }
        public decimal? NO_OF_BARS { get; set; }

        public double? FI_QTY_1 { get; set; }
        public Single? FI_NO_OF_BARS { get; set; }

        public Int16? FI_TABA_NO { get; set; }
        public int? taba_no { get; set; }
        public int? shikii { get; set; }
        public int? LINE_SPEED { get; set; }
        public int? PROD_DIM_D_LENGTH { get; set; }
        public string EC_NG_DETAILS1 { get; set; }
        public string EC_NG_DETAILS2 { get; set; }
        public string EC_NG_DETAILS3 { get; set; }
        public int? MSG_NO { get; set; }
        public decimal? EC_OK_QTY { get; set; }
        public decimal ACT_ALLOC_QTY1 { get; set; }
        public string MAGNETISM { get; set; }
        public decimal? PROD_LENGTHS1 { get; set; }
        public string INTERNAL_NAME { get; set; }
        public string PP_LOCATION_NAME { get; set; }
        

        public string DEPARTMENT_NAME { get; set; }
        public int? DECISION { get; set; }
        public int? SHIKII_VALUE { get; set; }

        public decimal? ng_bars_total { get; set; }

        public Int16? tp_test { get; set; }

        public string machine_name { get; set; }

        public int MACHINE_CD { get; set; }
        public string NG_DETAILS { get; set; }
        public string message_content { get; set; }
        public int ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public decimal? ng_percent { get; set; }
        public string decision_desc { get; set; }
        public string dim_dec { get; set; }
        public string length_dec { get; set; }
        public string bend_dec { get; set; }
        public string ext_surf_1_dec { get; set; }
        public string ext_surf_2_dec { get; set; }
        public string combined_dec { get; set; }

        // properties added for Templated EC Machine Grid
        public long EC_NO { get; set; }

        public string ACT_ALLOC_UNITS1 { get; set; }

        public short? EC_TABA_NO { get; set; }
        public string EC_OKBARS_UNITTYPE { get; set; }

        public short? EC_NG_BARS1 { get; set; }
        public short? EC_NG_BARS2 { get; set; }
        public short? EC_NG_BARS3 { get; set; }

     //   public short IS_OK { get; set; }
        public string kenshutu_kando { get; set; }
        public string LINE_SPEED1 { get; set; }


        public string PROD_SPEC_NO1 { get; set; }
        public string PROD_SPEC_NO { get; set; }

        public string PROD_DIM_D { get; set; }

        public string EC_METALLIC_PLATE_CODE { get; set; }
        public string METALLIC_LABEL_NO { get; set; }
        public string NO_OF_COILS { get; set; }
        public string QUANTITY { get; set; }
        public string DIE_DIM { get; set; }

        public DateTime ORDER_CLAUSE { get; set; }
        public decimal? FINISHED_QTY { get; set; }
        public string EXT_SURF_COND { get; set; }
        public string DW_BEND_STATUS { get; set; }

        public decimal? ec_ng_bar_total { get; set; }
        public string TABA_NO_1 { get; set; }
        public string TABA_NO_3 { get; set; }
        public decimal? actual_dim_3 { get; set; }
        public decimal? actual_dim_4 { get; set; }
        public decimal? actual_dim_5 { get; set; }
        public decimal? actual_dim_6 { get; set; }
        public decimal? actual_length_1 { get; set; }
        public decimal? actual_length_2 { get; set; }
        public decimal? actual_length_3 { get; set; }
        public string PROD_DIM_SHAPE { get; set; }
        public string FINISHED_BARS { get; set; }
        public DateTime? MCH_CHK_DATE { get; set; }
        public int? MCH_CHK_BY { get; set; }
        public Single? CORNER_RADIUS { get; set; }
        public Single? FI_ACT_RADIUS_1 { get; set; }
        public Single? FI_ACT_RADIUS_2 { get; set; }
        public Single? FI_ACT_RADIUS_3 { get; set; }
        public Single? FI_ACT_RADIUS_4 { get; set; }
        public Single? FI_ACT_RADIUS_5 { get; set; }
        public Single? FI_ACT_RADIUS_6 { get; set; }
        public string PPCHAMFER { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public DateTime INSP_DATE { get; set; }
        public DateTime INSP_DATE1 { get; set; }
        public short? FI_CORNER_RADIUS_STD_DECISION { get; set; }
        public string FI_CORNER_RADIUS_STD_DECISION_STRING { get; set; }
        public decimal STD_PRD_DIMD { get; set; }
        public decimal STD_PRD_DIM_TOL_MAX { get; set; }
        public decimal STD_PRD_DIM_TOL_MIN { get; set; }
        public string STD_PRD_LENGTH { get; set; }

        public decimal? STD_PRD_LENGTH_TOL_MAX { get; set; }
        public decimal? STD_PRD_LENGTH_TOL_MIN { get; set; }
        public decimal? RowIndex { get; set; }


        public string PROD_NAME_INT { get; set; }

        public decimal PROD_DIM_D_VALUE { get; set; }
        public string EXT_TEXTURE { get; set; }
        public string DIM_TOL_MIN { get; set; }
        public string DIM_TOL_MAX { get; set; }
        public string PROD_LENGTH_TOL_MAX { get; set; }
        public string PROD_LENGTH_TOL_MIN { get; set; }

        public string FI_COLOR_DECISION_VALUE { get; set; }
        public string FI_CHAMFERING_DECISION_VALUE { get; set; }
        public string FI_SPARK_DECISION_VALUE { get; set; }
        public string FI_EXT_TEXTURE_DECISION_VALUE { get; set; }
        public short? FI_EXT_TEXTURE_DECISION { get; set; }
        public string MACHINABILITY_VALUE { get; set; }
        public string TENSILE_VALUE { get; set; }
        public string HARDNESS_VALUE { get; set; }
        public int? PRINT_FLAG { get; set; }
        public string NIFUDA_NO { get; set; }

        public DateTime? PRINT_DATE { get; set; }
        public string CREATED_DATE { get; set; }
        public decimal? GROUP_COUNT { get; set; }
        public string INSP_DATE_VALUE { get; set; }
        public string INSP_DATE_STRING { get; set; }
        public string NO_OF_BARS_VALUE { get; set; }
        public int? INSP_BY { get; set; }
        public string OPERATOR_NAME { get; set; }

        public string GYOUBAN { get; set; }
        public string SHAPE { get; set; }
        public decimal? DIM_D_Value { get; set; }
        public decimal? SUM_FI_QTY { get; set; }

        //Added BY Sagar B on 31/7/2017 for Process Status search for SalesScreen
        public decimal? PP_CHUUBAN_YEAR { get; set; }
        public string INSP_BY_STRING { get; set; }
        public string CUST_YOBINA { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public string PP_CHUUBAN { get; set; }
        public Int16? PP_MAGARI_KUBUN { get; set; }
        public short? MAGARI_KUBUN { get; set; }
        public string PP_MAGARI_KUBUN_STRING { get; set; }
        public decimal? MAGARI_BEND_TOL { get; set; }
        public Int16? MAGARI_SPAN { get; set; }
        public string PROD_LENGTH_STRING { get; set; }
        public string FI_ACT_LENGTH_STRING { get; set; }
        public string FI_ACT_RADIUS_1_STRING { get; set; }
        public string FI_ACT_RADIUS_2_STRING { get; set; }
        public string FI_ACT_RADIUS_3_STRING { get; set; }
        public string FI_ACT_RADIUS_4_STRING { get; set; }
        public string FI_ACT_RADIUS_5_STRING { get; set; }
        public string FI_ACT_RADIUS_6_STRING { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public string MSG_CONTENTS { get; set; }
        public string PP_PROD_SPEC_NO1 { get; set; }
        public string PP_PROD_SPEC_NO2 { get; set; }

        public String FI_EXT_TEXTURE1_DECISION_VALUE { get; set; }
        public String FI_EXT_TEXTURE2_DECISION_VALUE { get; set; }
        public string FI_CHAMFERING_DECISION_STRING { get; set; }
        public string FI_COLOR_DECISION_STRING { get; set; }

        public string DIM_TOL_MAX_CHR { get; set; }
        public string DIM_TOL_MIN_CHR { get; set; }

        public string LENGTH_TOL_MAX_CHR { get; set; }
        public string LENGTH_TOL_MIN_CHR { get; set; }
        public short? FI_NEXT_PROCESS { get; set; }
        public int? INPUT_LOCATION_CD { get; set; }
        public int? LOCATION_CD { get; set; }
        public int USER_ID { get; set; }
        public long? EC_CD { get; set; }
        public int? VALIDATION_TYPE { get; set; }
        public string PROC_LOCATION_NAME { get; set; }
        public double? FI_LENGTH { get; set; }
        public Int64 PP_PROD_VER { get; set; }
        public string INSPECTED_DATE_STRING { get; set; }
        public double? PP_FINISHED_QTY { get; set; }
        public string ALL_LENGTH_TOL_MIN_CHR { get; set; }
        public string UNIT_TYPE_STRING { get; set; }
        public string FI_DIM_DECISION_STRING { get; set; }
        public string FI_LENGTH_DECISION_STRING { get; set; }
        public string FI_EXT_TEXTURE_DECISION_STRING { get; set; }
        public string FI_SPARK_DECISION_STRING { get; set; }
        public string ALL_DIM_TOL_MIN_MAX_CHR { get; set; }
        public string SPARK_INSP_DATE_STRING { get; set; }
        public string SPARK_INSP_OPERATOR_NAME { get; set; }
        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }
        public short? INSP_PROCESS_KUBUN_CD { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public int OPERATOR_ID { get; set; }
        public int SORT_ORDER { get; set; }
        public int IS_PRINT_BYPRINT_FLAG { get; set; }
        public List<string> CHUUBAN_LIST { get; set; }
        public string FI_DIM1_STRING { get; set; }
        public string FI_DIM2_STRING { get; set; }
        public string FI_DIM3_STRING { get; set; }
        public string FI_DIM4_STRING { get; set; }
        public short? TS_DECISION { get; set; }
        public short? HD_DECISION { get; set; }

    }
}
