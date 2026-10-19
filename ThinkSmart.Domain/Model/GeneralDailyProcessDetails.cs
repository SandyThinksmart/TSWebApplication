using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class GeneralDailyProcessDetails
    {
        #region "PROCESS_PLAN"
        public Int64 PP_PROCESS_PLAN_CD { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public string PP_CHUUBAN { get; set; }
        public int PP_GYOUBAN { get; set; }
        public string PP_GYOUBAN_STRING { get; set; }
        public string PP_DELIVERY_DATE_STRING { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_PROD_NAME_CUST { get; set; }
        public int PP_STD_DEL_PERIOD { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public int PP_CHAMFER { get; set; }
        public int? PP_UNIT_WT { get; set; }
        public int PP_COLOR_ID { get; set; }
        public string PP_COLOR { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public Single PP_MATL_DIM_D { get; set; }
        public string PP_CHARGE { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public Single PP_PROD_DIM_D { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public string PP_DIM_TOLERANCE { get; set; }
        public Single? PP_DIM_TOL_MAX { get; set; }
        public Single? PP_DIM_TOL_MIN { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_STAMP { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public Single? PP_PROD_LENGTH { get; set; }
        public string PP_LEN_TOL { get; set; }
        public float? PP_LENGTH_TOL_MAX { get; set; }
        public float? PP_LENGTH_TOL_MIN { get; set; }
        public string PP_PACKING { get; set; }
        public string PP_PACKING_DECODE { get; set; }
        public string PP_NO_OF_UNITS_DECODE { get; set; }
        public int PP_UNIT { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }
        public short? PP_COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public short? PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public string PP_COIL_DIA_WINDING { get; set; }     //WINDING_DIRECTION
        public short? COIL_DIA_WINDING { get; set; }
        public string PP_PRESS { get; set; }    //REDUCTION_PATTERN
        public string PP_KAKOUIRAI_REMARKS { get; set; }
        public string PP_PROCESS_PLAN_REMARK { get; set; }
        public string PP_PRODUCTION_PROCESS { get; set; }
        public List<MessageDetails> DW_MESSAGE_LIST { get; set; }
        public string PP_JIS_TEST_SPEC { get; set; }
        public string PP_TENSILE_STRENGTH { get; set; }
        public string PP_YEILD { get; set; }
        public string PP_ELONGATION { get; set; }
        public string PP_ELONGATION1 { get; set; }
        public string PP_ELONGATION2 { get; set; }
        public string PP_HARDNESS { get; set; }
        public string PP_DECARB { get; set; }
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
        public string PP_PROD_POINT_1 { get; set; }
        public string PP_PROD_POINT_2 { get; set; }
        public string PP_PROD_POINT_3 { get; set; }
        public string PP_PROD_POINT_4 { get; set; }
        public string PP_PROD_POINT_5 { get; set; }
        public string PP_PROD_POINT_6 { get; set; }
        public string PP_PROD_POINT_7 { get; set; }
        public string PP_PROD_POINT_8 { get; set; }
        public string PP_PROD_POINT_9 { get; set; }
        public string PP_PROD_POINT_10 { get; set; }
        public string PP_PROD_POINT_11 { get; set; }
        public string PP_PROD_POINT_12 { get; set; }
        public string PP_PROD_POINT_13 { get; set; }
        public DateTime PP_PROCESS_PLAN_PLNG_DATE { get; set; }
        public DateTime PROCESS_PLAN_CMPL_DATE { get; set; }
        public DateTime PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PP_CMPL_DATE_STRING { get; set; }
        public Single? PP_NO_OF_UNITS { get; set; }
        public int MAGARI_KUBUN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public int? MAGARI_SPAN { get; set; }
        public float? ELONGATION_PATTERN1 { get; set; }
        public int ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public Single? ELONGATION_PATTERN2_MIN { get; set; }
        public Single? ELONGATION_PATTERN2_MAX { get; set; }
        public int ELONGATION_PATTERN3_ACTUAL { get; set; }
        public int? TENSILE_STRN_PATTERN1 { get; set; }
        public int TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public int? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public int? TENSILE_STRN_PATTERN2_MAX { get; set; }
        public int TENSILE_STRN_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN1_CODE { get; set; }
        public int HARDNESS_PATTERN2_CODE { get; set; }
        public int? HARDNESS_PATTERN2_MIN { get; set; }
        public int? HARDNESS_PATTERN2_MAX { get; set; }
        public int? HARDNESS_PATTERN1 { get; set; }
        public int HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public int DECARB_1 { get; set; }
        public int DECARB_2 { get; set; }
        public int DECARB_3 { get; set; }
        public float? DECARB_VAL_1 { get; set; }
        public float? DECARB_VAL_2 { get; set; }
        public float? DECARB_VAL_3 { get; set; }
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
        public string PP_PROD_DIM_D_STRING { get; set; }
        public string PP_MATL_DIM_D_STRING { get; set; }
        public string PP_PROD_SPEC_NO1 { get; set; }
        public string PP_PROD_SPEC_NO2 { get; set; }
        public long? PP_ROOT_VER_PRODUCT_KEY { get; set; }
        public string PP_INTERNAL_NAME { get; set; }
        public Single? PP_PROD_DIM_W { get; set; }

        public string PP_DIM_TOL_MIN_CHR { get; set; }
        public string PP_DIM_TOL_MAX_CHR { get; set; }
        public string PP_PROD_LENGTH_OTHER { get; set; }
        public string PP_LENGTH_TOL_MIN_CHR { get; set; }
        public string PP_LENGTH_TOL_MAX_CHR { get; set; }
        public string PP_PROD_CONDITION_DESCRIPTION { get; set; }
        public Single? PP_CORNER_RADIUS { get; set; }
        public Single? PP_NEJIRE_ANGLE { get; set; }
        public short? PP_NEJIRE_SPAN { get; set; }
        public short? PP_UNIT_WT_KUBUN { get; set; }
        public short? PP_UNIT_WT_MIN { get; set; }
        public short? PP_UNIT_WT_MAX { get; set; }
        public string PP_TEST_SPECIMEN { get; set; }
        public short? PP_IMPACT_TEST_PATTERN1 { get; set; }
        public short? IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public short? PP_IMPACT_TEST_PATTERN2_MIN { get; set; }
        public short? PP_IMPACT_TEST_PATTERN2_MAX { get; set; }
        public short? PP_IMPACT_TEST_PATTERN3_ACTUAL { get; set; }
        public short? RANK_STD { get; set; }
        public string PP_HT_CONDITION { get; set; }
        public short? PP_MATL_TYPE_CBK { get; set; }
        public string PP_MATL_MAKER_NAME { get; set; }
        public Single? PP_MATL_DIM_W { get; set; }
        public string PP_MATL_DIM_OTHER { get; set; }
        public Int32? PP_MATL_LENGTH { get; set; }
        public string PP_MATL_CONDITION_DESCRIPTION { get; set; }
        public Int16? PP_PROCESS_PLAN_KUBUN { get; set; }
        public short? PP_PROD_MNGMNT_KUBUN { get; set; }
        public short? PP_SPCL_MNGMNT_KUBUN { get; set; }
        public short? PP_STORAGE_MNGMNT_KUBUN { get; set; }
        public double? PP_KAKOUIARI_QTY { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }
        public DateTime? PP_ACT_ALLOC_DATE { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public DateTime? PP_PROCESS_PLAN_PRINT_DATE { get; set; }
        public string PP_PROCESS_PLAN_PRINT_DATE_STR { get; set; }
        public short? PP_VER { get; set; }
        public string PP_UNIT_TYPE_STRING { get; set; }
        public string PP_MAGARI_KUBUN_STRING { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }
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
        public string PP_HT_CONDITION_STRING { get; set; }
        public string PP_PROCESS_PLAN_KUBUN_STRING { get; set; }
        public string PP_PROD_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_SPCL_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_STORAGE_MNGMNT_KUBUN_STRING { get; set; }
        public DateTime? PROCESS_PLAN_START_DATE { get; set; }
        public string PP_CHAMFER_STRING { get; set; }
        public string PP_CORNER_RADIUS_STRING { get; set; }
        public string PP_MATL_TYPE_CBK_STRING { get; set; }
        public int? USER_ID { get; set; }
        public short? INPUT_LOCATION_CD { get; set; }
        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }

        #endregion

        #region "GENERAL_DAILY_PROCESS"
        public short? PROCESS_KUBUN_CD { get; set; }
        public string PROCESS_KUBUN_STRING { get; set; }
        public short? MACHINE_CD { get; set; }
        public string GP_MACHINE_NAME { get; set; }
        public short? LOCATION_CD { get; set; }
        public string GP_LOCATION_NAME { get; set; }
        public DateTime? GP_START_DATE { get; set; }
        public string GP_START_DATE_STRING { get; set; }
        public string GP_DATE { get; set; }
        public int? GP_START_OPERATOR_CD { get; set; }
        public string GP_START_OPERATOR_NAME { get; set; }
        public DateTime? GP_END_DATE { get; set; }
        public string GP_END_DATE_STRING { get; set; }
        public int? GP_END_OPERATOR_CD { get; set; }
        public string GP_END_OPERATOR_NAME { get; set; }
        public Single? GP_OK_UNITS { get; set; }
        public Single? GP_OK_UNITS_SINGLE { get; set; }
        public short? UNIT_TYPE { get; set; }
        public string GP_UNIT_TYPE_STRING { get; set; }
        public double? GP_QTY { get; set; }
        public Double? GP_QTY_DOUBLE { get; set; }
        public short? GP_NG_DETAILS1 { get; set; }
        public Single? GP_NG_UNITS1 { get; set; }
        public short? GP_NG_UNIT_TYPE1 { get; set; }
        public double? GP_NG_QTY1 { get; set; }
        public short? GP_NG_DETAILS2 { get; set; }
        public Single? GP_NG_UNITS2 { get; set; }
        public short? GP_NG_UNIT_TYPE2 { get; set; }
        public double? GP_NG_QTY2 { get; set; }
        public short? GP_NG_DETAILS3 { get; set; }
        public Single? GP_NG_UNITS3 { get; set; }
        public short? GP_NG_UNIT_TYPE3 { get; set; }
        public double? GP_NG_QTY3 { get; set; }
        public short? GP_TABA_NO_1 {get;set;}
        public short? GP_TABA_NO_2 {get;set;}
        public short? GP_TABA_NO_3 {get;set;}
        public short? GP_TABA_NO_4 { get; set; }
        public string DAILY_PROCESS_NOTE { get; set; }
        public short? GP_DECISION { get; set; }
        public short? GP_NEXT_PROCESS { get; set; }
        public string PROC_DEFECT_STRING1 { get; set; }
        public string PROC_DEFECT_STRING2 { get; set; }
        public string PROC_DEFECT_STRING3 { get; set; }

        public string PROC_DEFECT1_JP { get; set; }
        public string PROC_DEFECT1_EN { get; set; }
        public string PROC_DEFECT2_JP { get; set; }
        public string PROC_DEFECT2_EN { get; set; }
        public string PROC_DEFECT3_JP { get; set; }
        public string PROC_DEFECT3_EN { get; set; }
        public string PROCESS_KUBUN_EN { get; set; }
        public string PROCESS_KUBUN_JP { get; set; }

        public string NEXT_PROCESS_EN { get; set; }
        public string NEXT_PROCESS_JP { get; set; }
        public string GP_NG_UNIT_TYPE1_STRING { get; set; }
        public string GP_NG_UNIT_TYPE2_STRING { get; set; }
        public string GP_NG_UNIT_TYPE3_STRING { get; set; }
        public string GP_DECISION_STRING { get; set; }
        public string GP_NEXT_PROCESS_NAME { get; set; }
        public int KUBUN_ID { get; set; }
        public string ALL_TABA_NO { get; set; }
        public string LENGTH_TOL_MIN_MAX_CHR { get; set; }
        public string DIM_TOL_MIN_MAX_CHR { get; set; }
        public string LENGTH { get; set; }
        public string RESPONCE { get; set; }
        public string GP_REMARK { get; set; }
        public string METALIC_PLATE_NO { get; set; }
        public string GP_START_PROCESS_NAME { get; set; }
        public short? PROCESS_DEFECT_CD { get; set; }
        public string PROC_DEFECT_SHORT { get; set; }
        
        public string GP_METALLIC_PLATE_CODE { get; set; }
        public int? TABA_COUNT { get; set; }
        public int? GP_OPERATOR_CD { get; set; }
        public int? GP_PROCESS_STATUS { get; set; }
        public string PP_PROCESS_NAME {  get; set; }
        public string PP_MACHINE_NAME { get; set; }
        public int? GEN_PROCESS_CD { get; set; }

        #endregion
    }

}
