using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class PostHTDetails
    {

        #region "PROCESS_PLAN"
        public string PP_CHUUBAN { get; set; }
        public Int16? PP_GYOUBAN { get; set; }
        public string PP_PROD_SPEC_NO1 { get; set; }
        public string PP_PROD_SPEC_NO2 { get; set; }
        public int PROCESS_KUBUN_ID { get; set; }
        public long? PP_ROOT_VER_PRODUCT_KEY { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_PROD_NAME_CUST { get; set; }
        public string PP_INTERNAL_NAME { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public Single? PP_PROD_DIM_W { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        public Single? PP_DIM_TOL_MAX { get; set; }
        public Single? PP_DIM_TOL_MIN { get; set; }
        public string PP_DIM_TOL_MIN_CHR { get; set; }
        public string PP_DIM_TOL_MAX_CHR { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public Single? PP_PROD_LENGTH { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public float? PP_PROD_DIM_D { get; set; }
        public string PP_PROD_LENGTH_OTHER { get; set; }
        public string PP_LENGTH_TOL_MIN_CHR { get; set; }
        public string PP_LENGTH_TOL_MAX_CHR { get; set; }
        public string PP_PROD_DIM_D_STRING { get; set; }
        public string PP_MATL_DIM_D_STRING { get; set; }
        public string PP_PROD_CONDITION_DESCRIPTION { get; set; }
        public short? PP_CHAMFER { get; set; }
        public short PP_MAGARI_KUBUN { get; set; }
        public Single? PP_MAGARI_BEND_TOL { get; set; }
        public Int16? PP_MAGARI_SPAN { get; set; }
        public float? PP_NEJIRE_ANGLE { get; set; }
        public short? PP_NEJIRE_SPAN { get; set; }
        public short? PP_UNIT_WT_MIN { get; set; }
        public short? PP_UNIT_WT_MAX { get; set; }
        public short? PP_UNIT_WT_KUBUN { get; set; }
        public Int16? PP_UNIT_WT { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public Int16? PP_COLOR_ID { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_STAMP { get; set; }
        public float? PP_LENGTH_TOL_MAX { get; set; }
        public float? PP_LENGTH_TOL_MIN { get; set; }
        public short? PP_COIL_DIA_INTERNAL { get; set; }
        public short? PP_COIL_DIA_EXTERNAL { get; set; }
        public short? PP_COIL_DIA_WINDING { get; set; }
        public float? PP_CORNER_RADIUS { get; set; }
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
        public float? TENSILE_STRN_PATTERN1 { get; set; }
        public int TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public short? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public short? TENSILE_STRN_PATTERN2_MAX { get; set; }
        public short TENSILE_STRN_PATTERN3_ACTUAL { get; set; }

        public float? ELONGATION_PATTERN1 { get; set; }
        public int ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public float? ELONGATION_PATTERN2_MIN { get; set; }
        public float? ELONGATION_PATTERN2_MAX { get; set; }
        public short ELONGATION_PATTERN3_ACTUAL { get; set; }
        public short? HARDNESS_PATTERN1 { get; set; }
        public int HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public short? HARDNESS_PATTERN2_MIN { get; set; }
        public short? HARDNESS_PATTERN2_MAX { get; set; }
        public short? HARDNESS_PATTERN3_ACTUAL { get; set; }
        public short? HARDNESS_PATTERN1_CODE { get; set; }
        public short? HARDNESS_PATTERN2_CODE { get; set; }
        public float? YEILD_PATTERN1 { get; set; }
        public int YEILD_PATTERN1_MIN_MAX { get; set; }
        public short? YEILD_PATTERN2_MIN { get; set; }
        public short? YEILD_PATTERN2_MAX { get; set; }
        public short? YEILD_PATTERN3_ACTUAL { get; set; }
        public float? PRESS_PATTERN1 { get; set; }
        public int PRESS_PATTERN1_MIN_MAX { get; set; }
        public Int16? PRESS_PATTERN2_MIN { get; set; }
        public Int16? PRESS_PATTERN2_MAX { get; set; }
        public int PRESS_PATTERN3_ACTUAL { get; set; }
        public short? IMPACT_TEST_PATTERN1 { get; set; }
        public short? IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public short? IMPACT_TEST_PATTERN2_MIN { get; set; }
        public short? IMPACT_TEST_PATTERN2_MAX { get; set; }
        public short? IMPACT_TEST_PATTERN3_ACTUAL { get; set; }
        public int DECARB_1 { get; set; }
        public int DECARB_2 { get; set; }
        public int DECARB_3 { get; set; }
        public Single? DECARB_VAL_1 { get; set; }
        public Single? DECARB_VAL_2 { get; set; }
        public Single? DECARB_VAL_3 { get; set; }
        public short? RANK_STD { get; set; }
        public string PP_MATL_CONDITION_DESCRIPTION { get; set; }
        public Int16? PP_PROCESS_PLAN_KUBUN { get; set; }
        public double? PP_KAKOUIARI_QTY { get; set; }
        public DateTime? PP_PROCESS_PLAN_PRINT_DATE { get; set; }
        public string PP_PROCESS_PLAN_PRINT_DATE_STR { get; set; }
        public string PP_HT_CONDITION { get; set; }
        public short? PP_MATL_TYPE_CBK { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public Single? PP_MATL_DIM_D { get; set; }
        public float? PP_MATL_DIM_W { get; set; }
        public string PP_MATL_DIM_OTHER { get; set; }
        public int? PP_MATL_LENGTH { get; set; }
        public string PP_CHARGE { get; set; }
        public short? PP_PROD_MNGMNT_KUBUN { get; set; }
        public short? PP_SPCL_MNGMNT_KUBUN { get; set; }
        public short? PP_STORAGE_MNGMNT_KUBUN { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }

        public string PP_DELIVERY_DATE_STRING { get; set; }
        public Single? PP_NO_OF_UNITS { get; set; }
        public string PP_NO_OF_UNITS_DECODE { get; set; }
        public short? PP_UNIT { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }
        public DateTime? PP_ACT_ALLOC_DATE { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public short PRODUCT_CD { get; set; }
        public short GET_PP_STD_DMT { get; set; }
        public short GET_PP_STD_DMF { get; set; }
        public string PP_GYOUBAN_STRING { get; set; }
        public long? HEAT_TREATMENT_CD { get; set; }
        public string PP_PROD_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_SPCL_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_STORAGE_MNGMNT_KUBUN_STRING { get; set; }
        public short? PP_VER { get; set; }
        public string PP_CHAMFER_STRING { get; set; }
        public string PP_CORNER_RADIUS_STRING { get; set; }
        public string PP_MAGARI_KUBUN_STRING { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }
        public string PP_COLOR { get; set; }
        public string TENSILE_STRN_PATTERN1_MIN_MAX_STRING { get; set; }
        public string TENSILE_STRN_PATTERN3_ACTUAL_STRING { get; set; }
        public string ELONGATION_PATTERN1_MIN_MAX_STRING { get; set; }
        public string ELONGATION_PATTERN3_ACTUAL_STRING { get; set; }
        public string HARDNESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string HARDNESS_PATTERN3_ACTUAL_STRING { get; set; }
        public string YEILD_PATTERN1_MIN_MAX_STRING { get; set; }
        public string YEILD_PATTERN3_ACTUAL_STRING { get; set; }
        public string PRESS_PATTERN3_ACTUAL_STRING { get; set; }
        public string PRESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string PP_DECARB_STD_1 { get; set; }
        public string PP_DECARB_STD_2 { get; set; }
        public string PP_DECARB_STD_3 { get; set; }
        public string IMPACT_TEST_PATTERN1_MIN_MAX_STRING { get; set; }
        public string IMPACT_TEST_PATTERN3_ACTUAL_STRING { get; set; }
        public string PP_MATL_TYPE_CBK_STRING { get; set; }
        public string PP_PROCESS_PLAN_KUBUN_STRING { get; set; }
        public string PP_UNIT_TYPE_STRING { get; set; }
        public string PP_HT_CONDITION_STRING { get; set; }
        public long? MSG_CD { get; set; }
        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }
        #endregion


        #region "INTERNAL_QUALITY"
        public long? INTERNAL_QUALITY_CD { get; set; }
        public long PROCESS_PLAN_CD { get; set; }
        public Int16? MACHINE_CD { get; set; }
        public Int16 LOCATION_CD { get; set; }
        public Int32 INT_QLTY_TEST_OPERATOR_CD { get; set; }
        public DateTime? INT_QLTY_TEST_DATE { get; set; }
        public string PROC_COND { get; set; }

        public string HT_CONDITION { get; set; }
        // public Int16? PATTERN_NO { get; set; }
        public string PATTERN_NO { get; set; }
        public string FURNACE_NO { get; set; }
        public Int16? LOCATION_IN_FURNACE1 { get; set; }
        public Int16? LOCATION_IN_FURNACE2 { get; set; }
        public string STACK_POSITION { get; set; }
        //public short? ORIENTATION { get; set; }
        //public short? IN_OUT { get; set; }
        public short? ACT_DM_F { get; set; }
        public string ACT_DM_T_MIN { get; set; }
        public string ACT_DM_T_MAX { get; set; }
        public string ACT_DM_T_AVG { get; set; }
        public Single? ACT_STRUCTURE { get; set; }
        public Single? HRD_CENTER { get; set; }
        public Single? HRD_D4_1 { get; set; }
        public Single? HRD_D4_2 { get; set; }
        public Single? HRD_D4_3 { get; set; }
        public Single? HRD_D4_4 { get; set; }
        public Single HRD_D4_AVG { get; set; }
        public short INTERNAL_QUALITY_DECISION { get; set; }
        public int? USER_ID { get; set; }
        public Int16 INPUT_LOCATION_CD { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string IQ_MACHINE_NAME { get; set; }
        public string IQ_START_OPERATOR_NAME { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string PROD_NAME { get; set; }
        public string PROD_NAME_CUST { get; set; }        
        public string CHARGE { get; set; }
        public Single PROD_DIM_D { get; set; }
        public Single MATL_DIM_D { get; set; }
        public string DMF_DMT { get; set; }
        public Int64 INTERNAL_QUALITY_GROUP_NO { get; set; }
        public int? SERIAL_NUMBER { get; set; }
        public string CHUUBAN { get; set; }
        public int CHUUBAN_CODE { get; set; }

        //public decimal? DECARB_VAL_1 { get; set; }
        //public decimal? DECARB_VAL_3 { get; set; }
        public string INT_QLTY_TEST_DATE_STRING { get; set; }
        public int POSTHT_NEXT_PROCESS { get; set; }
        public long? IQ_NEXT_PROCESS { get; set; }
        public string INSPECTION_LOCATION_NAME { get; set; }

        public string INT_QLTY_DECISION_STRING { get; set; }
        public string INT_QLTY_REMARKS { get; set; }
        public string ALL_ACT_DM_T { get; set; }
        public string ACT_DM_T_AVG_STRING { get; set; }
        public string LOCATION_IN_FURNACE1_2 { get; set; }
        public DateTime INSP_DATE_START { get; set; }
        public DateTime INSP_DATE_END { get; set; }
        public bool ISVALIDCHUUBAN { get; set; }
        public DateTime? PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public DateTime? PROCESS_PLAN_START_DATE { get; set; }
        public short? ORIENTATION { get; set; }
        public short? IN_OUT { get; set; }

        #endregion
    }
}
