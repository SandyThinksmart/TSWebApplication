using System;
using System.Collections.Generic;
using System.Text;


namespace TSWebApplication.Domain.Model
{
    public class ColdDrawingWireDrawingDetails
    {
        #region "process_plan"
        public string CULTURE_NAME { get; set; }
        public string PROD_SPEC_NO { get; set; }
        public int PP_PROCESS_PLAN_CD { get; set; }
        public long PP_CUSTOMER_CD { get; set; }
        public long PP_EMPLOYEE_CD { get; set; }
        public string PP_CHUUBAN { get; set; }
        public short? PP_GYOUBAN { get; set; }
        public string PP_PROD_DIM_D_STRING { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public long PP_PRODUCT_HISTORY_CD { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public int PP_KAKOUIRAI_CD { get; set; }
        public decimal PP_CHUUBAN_YEAR { get; set; }
        public int PP_ACT_ALLOC_CD { get; set; }
        public int PP_ACT_ALLOC_TYPE { get; set; }
        public DateTime PP_DELIVERY_DATE { get; set; }

        public string PP_DELIVERY_DATE_STRING { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }      //COIL_QTY

        public string PP_MATL_DIM_D_STRING { get; set; }
        public double PP_QUANTITY { get; set; }
        public decimal PP_ORIGINAL_QTY { get; set; }
        public Single PP_NO_OF_UNITS { get; set; }
        public string PP_NO_OF_UNITS_DECODE { get; set; }     //PP.INZU || ' ' || PP.INZU_UNIT AS PP_NO_OF_UNITS_STRING

        public int PP_UNIT { get; set; }
        public int PP_MATL_NAME_MAK_CD { get; set; }
        public int PP_MATL_NAME_INT_CD { get; set; }
        public int PP_EMPLYOEE_CD { get; set; }
        public int PP_PROD_NAME_CUST_CD { get; set; }
        public int PP_RECEIVER_CD { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public Single PP_MATL_DIM_D { get; set; }

        public float PP_MATL_DIM_D_SHAPE { get; set; }

        public string PP_MATL_DIM_OTHER { get; set; }
        public string PP_CHARGE { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public Single? PP_PROD_DIM_D { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        //public Single PP_DIM_TOL_MAX { get; set; }
        public Single? PP_DIM_TOL_MAX { get; set; }
        //public Single? PP_DIM_TOL_MIN { get; set; }
        public Single? PP_DIM_TOL_MIN { get; set; }
        public string PP_DIM_TOLERANCE { get; set; }
        public Single? PP_PROD_LENGTH { get; set; }
        public Single? PP_LENGTH_TOL_MAX { get; set; }
        public Single? PP_LENGTH_TOL_MIN { get; set; }
        public string PP_LEN_TOL { get; set; }
        public int PP_CHAMFER { get; set; }
        public short? PP_UNIT_WT { get; set; }
        public int PP_COLOR_ID { get; set; }

        public string PP_COLOR { get; set; }
        public string PP_MAGARI_KUBUN { get; set; }
        //  public decimal PP_BEND_TOL { get; set; }
        public string PP_BEND_TOL { get; set; }
        public Single MAGARI_BEND_TOLLERENCE { get; set; }
        public int PP_MAGARI_SPAN { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_PACKING { get; set; }
        public string PP_PACKING_DECODE { get; set; }
        public string PP_TEST_SPECIMEN { get; set; }
        public string PP_TENSILE_STRENGTH { get; set; }
        public decimal PP_TENSILE_STRN_MIN { get; set; }
        public decimal PP_TENSILE_STRN_MAX { get; set; }
        public string PP_YEILD { get; set; }  //YIELD_POINT_PATTERN
        public string PP_ELONGATION1 { get; set; }
        public string PP_HARDNESS1 { get; set; }    //HD_PATTERN
        public string PP_DECARB { get; set; }
        public int PP_PROD_MNGMNT_KUBUN { get; set; }
        public int PP_SPCL_MNGMNT_KUBUN { get; set; }
        public int PP_STORAGE_MNGMNT_KUBUN { get; set; }
        public int PP_STD_DEL_PERIOD { get; set; }
        public string PP_STD_DEL_PERIOD_STRING { get; set; }
        public int PP_STORAGE_LOCATION { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_MATL_NAME_INT { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_PROD_NAME_CUST { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public string PP_PROCESS_PLAN_REMARK { get; set; }
        public decimal PP_FINISHED_QTY { get; set; }
        public string PP_REFERENCE_CHUBBAN { get; set; }
        public int PP_SETSUZAN_KUBUN { get; set; }
        public string PP_LOCATION_NAME { get; set; }
        public string PP_PROCESS_KUBUN { get; set; }
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
        public int PP_PROCESS_PLAN_CMPL_FLAG { get; set; }
        public int PP_PROCESS_PLAN_PRINT_FLAG { get; set; }
        public int PP_PROCESS_PLAN_INACTIVE { get; set; }
        public int PP_USER_ID { get; set; }
        public DateTime PP_CREATED_DATE { get; set; }
        public DateTime PP_UPDATED_DATE { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public string PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PP_ELONGATION2 { get; set; }   //ELONGATION_PATTERN
        public string PP_PRESS1 { get; set; }
        public string PP_PRESS2 { get; set; }
        public string PP_HARDNESS_2 { get; set; }   //HD_MIN
        public string PP_HARDNESS_3 { get; set; }  //HD_MAX

        public string PP_HARDNESS { get; set; }
        public string PP_DECARB_STD_1 { get; set; }
        public string PP_DECARB_STD_2 { get; set; }
        public string PP_DECARB_STD_3 { get; set; }
        public decimal PP_DECARB_VAL_1 { get; set; }
        public decimal PP_DECARB_VAL_2 { get; set; }
        public decimal PP_DECARB_VAL_3 { get; set; }
        public short? PP_COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public short? PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public string PP_COIL_DIA_WINDING { get; set; }
        public string PP_PRESS { get; set; }    //REDUCTION_PATTERN
        public string NIFUDA_NO { get; set; }
        public string PP_ELONGATION { get; set; }
        public decimal PP_INZU { get; set; }
        public string PP_INZU_UNIT { get; set; }

        public string PP_NO_OF_COILS { get; set; }   //  PP.INZU || ' ' || PP.INZU_UNIT as PP_NO_OF_COILS 
        public string PP_STAMP { get; set; }
        public string PP_PRODUCT_CONDITION { get; set; }
        public DateTime PP_ACT_ALLOC_DATE { get; set; }
        public string PP_PROD_NAME_INT { get; set; }
        public string PP_KAKOUIRAI_REMARKS { get; set; }
        public int? PRODUCT_NYUKO_CD {  get; set; }
        public string PP_PRODUCTION_PROCESS { get; set; }
        public string PP_PROCESS_SPECIFICATION_REMARK { get; set; }
        public int PP_FACTORY_PRODUCTION_FLAG { get; set; }
        public decimal PP_DIM_W { get; set; }
        public string PP_HT_CONDITION { get; set; }
        public decimal PP_CORNER_RADIUS { get; set; }
        public string PP_MBEU { get; set; }
        public string PP_DIM_D_X_DIM_W { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public int? PP_MATL_LENGTH { get; set; }
        public DateTime PP_GENERATION_DATE { get; set; }
        public string PP_PROD_SPEC_NO1 { get; set; }
        public string PP_PROD_SPEC_NO2 { get; set; }
        public int PP_UNIT_WT_KUBUN { get; set; }
        public int PP_IMPACT_TEST_MIN { get; set; }
        public int PP_IMPACT_TEST_MAX { get; set; }
        public string PP_IMPACT_TEST { get; set; }
        public int PP_DELETE_FLAG { get; set; }
        public string PP_PROD_POINT_SUB_1 { get; set; }
        public string PP_PROD_POINT_SUB_2 { get; set; }
        public string PP_PROD_POINT_SUB_3 { get; set; }
        public string PP_PROD_POINT_SUB_4 { get; set; }
        public string PP_PROD_POINT_SUB_5 { get; set; }
        public string PP_PROD_POINT_SUB_6 { get; set; }
        public string PP_PROD_POINT_SUB_7 { get; set; }
        public string PP_PROD_POINT_SUB_8 { get; set; }
        public string PP_PROD_POINT_SUB_9 { get; set; }
        public string PP_PROD_POINT_SUB_10 { get; set; }
        public string PP_PROD_POINT_SUB_11 { get; set; }
        public string PP_PROD_POINT_SUB_12 { get; set; }
        public string PP_PROD_POINT_SUB_13 { get; set; }
        public int PP_MATL_TYPE_CBK { get; set; }
        public string PP_MAKER_NAME_int { get; set; }
        public string PP_MATL_CONDITION { get; set; }
        public Single? PP_MATL_DIM_W { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_1 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_2 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_3 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_4 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_5 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_6 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_7 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_8 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_9 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_10 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_11 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_12 { get; set; }
        public string PP_PROCESS_ANOTHER_NAME_13 { get; set; }
        public string PP_PROCESS_NAME_1 { get; set; }
        public string PP_PROCESS_NAME_2 { get; set; }
        public string PP_PROCESS_NAME_3 { get; set; }
        public string PP_PROCESS_NAME_4 { get; set; }
        public string PP_PROCESS_NAME_5 { get; set; }
        public string PP_PROCESS_NAME_6 { get; set; }
        public string PP_PROCESS_NAME_7 { get; set; }
        public string PP_PROCESS_NAME_8 { get; set; }
        public string PP_PROCESS_NAME_9 { get; set; }
        public string PP_PROCESS_NAME_10 { get; set; }
        public string PP_PROCESS_NAME_11 { get; set; }
        public string PP_PROCESS_NAME_12 { get; set; }
        public string PP_PROCESS_NAME_13 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_1 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_2 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_3 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_4 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_5 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_6 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_7 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_8 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_9 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_10 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_11 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_12 { get; set; }
        public string PP_PROCESS_COLOR_KUBUN_13 { get; set; }
        public string PP_CUSTOMER_FULL_NAME { get; set; }
        public string PP_RECEIVER_FULL_NAME { get; set; }
        public int? PP_PROD_VER { get; set; }
        public string PP_INTERNAL_NAME { get; set; }
        public string PP_JIS_TEST_SPEC { get; set; }
        public Single? PP_PROD_DIM_W { get; set; }
        public string TEST_SPEC_REMARK { get; set; }
        public string PP_DIM_TOL_MIN_CHR { get; set; }
        public string PP_DIM_TOL_MAX_CHR { get; set; }
        public string PP_LENGTH_TOL_MIN_CHR { get; set; }
        public string PP_LENGTH_TOL_MAX_CHR { get; set; }
        public string MATL_DIM { get; set; }    //Added by Sagar B on 15-5-2017
        public Single PP_MATL_DIM_D1 { get; set; }
        public string PP_MATL_MAKER_NAME { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string PP_PROD_LENGTH_OTHER { get; set; }
        public Single? PP_CORNER_R { get; set; }
        public int MAGARI_KUBUN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public int? MAGARI_SPAN { get; set; }

        public string TEST_SPECIMEN { get; set; }
        public int? TENSILE_STRN_PATTERN1 { get; set; }
        public int TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public int? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public int? TENSILE_STRN_PATTERN2_MAX { get; set; }
        public int TENSILE_STRN_PATTERN3_ACTUAL { get; set; }
        public float? ELONGATION_PATTERN1 { get; set; }
        public int ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public Single? ELONGATION_PATTERN2_MIN { get; set; }
        public Single? ELONGATION_PATTERN2_MAX { get; set; }
        public int ELONGATION_PATTERN3_ACTUAL { get; set; }
        public int? HARDNESS_PATTERN1 { get; set; }
        public int HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public int? HARDNESS_PATTERN2_MIN { get; set; }
        public int? HARDNESS_PATTERN2_MAX { get; set; }
        public int HARDNESS_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN1_CODE { get; set; }
        public int HARDNESS_PATTERN2_CODE { get; set; }
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
        public int? IMPACT_TEST_PATTERN1 { get; set; }
        public int IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public int? IMPACT_TEST_PATTERN2_MIN { get; set; }
        public int? IMPACT_TEST_PATTERN2_MAX { get; set; }
        public int IMPACT_TEST_PATTERN3_ACTUAL { get; set; }
        public decimal PP_KAKOUIARI_QTY { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public string PP_PROCESS_PLAN_PRINT_DATE_STR { get; set; }
        public string DECARB { get; set; }
        public int DECARB_1 { get; set; }
        public int DECARB_2 { get; set; }
        public int DECARB_3 { get; set; }
        public Single? DECARB_VAL_1 { get; set; }
        public Single? DECARB_VAL_2 { get; set; }
        public Single? DECARB_VAL_3 { get; set; }
        public short? COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public short? COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public short? COIL_DIA_WINDING { get; set; }
        public string PP_UNIT_TYPE_STRING { get; set; }
        public string PP_MAGARI_KUBUN_STRING { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }
        public Single? NEJIRE_ANGLE { get; set; }
        public short? NEJIRE_SPAN { get; set; }
        public short? UNIT_WT_MIN { get; set; }
        public short? UNIT_WT_MAX { get; set; }
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
        public short? RANK_STD { get; set; }
        public string IMPACT_TEST_PATTERN3_ACTUAL_STRING { get; set; }
        public string IMPACT_TEST_PATTERN1_MIN_MAX_STRING { get; set; }
        public string PP_HT_CONDITION_STRING { get; set; }
        public string MATL_CONDITION_DESCRIPTION { get; set; }
        public string CONDITION_DESCRIPTION { get; set; }
        public Int16? PROCESS_PLAN_KUBUN { get; set; }
        public string PP_PROD_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_SPCL_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_STORAGE_MNGMNT_KUBUN_STRING { get; set; }
        public int PROCESS_KUBUN_CD { get; set; }
        public long DW_MSG_CD { get; set; }
        public DateTime? PROCESS_PLAN_START_DATE { get; set; }

        public List<MessageDetails> DW_MESSAGE_LIST { get; set; }
        public string PP_MATL_TYPE_CBK_STRING { get; set; }
        public string PP_GYOUBAN_STRING { get; set; }
        public string PP_CHAMFER_STRING { get; set; }
        public string PP_CORNER_RADIUS_STRING { get; set; }
        public string PP_PROCESS_PLAN_KUBUN_STRING { get; set; }
        public double? KAKOUIARI_QTY { get; set; }

        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }
        #endregion

        #region "ColdDrawingWireDrawing"
        public long DRAWING_CD { get; set; }
        public long PROCESS_PLAN_CD { get; set; }
        public int MACHINE_CD { get; set; }
        public int LOCATION_CD { get; set; }
        public long? MSG_CD { get; set; }
        public DateTime DW_START_DATE { get; set; }
        public int DW_START_OPERATOR_CD { get; set; }
       
        public string ROLLING_NUMBER { get; set; }
        public string FURNACE_NO { get; set; }
        public Single? DIE_DIM { get; set; }
        public string DIE_SHAPE { get; set; }
        public string DIE_NO { get; set; }
        public int DIE_CHK_START { get; set; }
        public int WINDING_SHAPE { get; set; }
        public int IM_DRAWING { get; set; }
        public int REDRAWING { get; set; }
        public DateTime DW_END_DATE { get; set; }
        public int DW_END_OPERATOR_CD { get; set; }
        public short? DW_ACT_DIM_CHK { get; set; }
        public Single? DW_ACT_DIM_START { get; set; }
        public Single? DW_ACT_DIM_END { get; set; }
        public Single? DW_FINISHED_COILS { get; set; }
        public int UNIT_TYPE { get; set; }
        public double? DW_FINISHED_QTY { get; set; }
        public Int16? DW_TABA_NO_1 { get; set; }
        public Int16? DW_TABA_NO_2 { get; set; }
        public Int16? DW_TABA_NO_3 { get; set; }
        public Int16? DW_TABA_NO_4 { get; set; }
        public int DW_EXT_TEXTURE_STATUS { get; set; }
        public string DW_EXT_TEXTURE_STATUS_DECODE { get; set; }        
        public string DW_DW_EXT_TEXTURE_STATUS { get; set; }
        public int DW_MAGNETIC_INSPECTIONS { get; set; }
        public string DW_DW_MAGNETIC_INSPECTIONS { get; set; }
        public int DIE_CHK_END { get; set; }
        public int DW_DECISION { get; set; }
        public int? DECISION_OK { get; set; }
        public int DW_NEXT_PROCESS { get; set; }
        public int? USER_ID { get; set; }
        public int INPUT_LOCATION_CD { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }

        public string DW_START_OPERATOR_NAME { get; set; }
        public string DW_END_OPERATOR_NAME { get; set; }
        public string DW_END_DATE_STRING { get; set; }
        public string DW_START_DATE_STRING { get; set; }
        public string DW_ROLLING_NUMBER { get; set; }
        public string DW_FURNACE_NO { get; set; }
        public string DW_DIE_NO { get; set; }
        public string DW_DIE_SHAPE { get; set; }
        public int DW_WINDING_SHAPE { get; set; }
        public string DW_WINDING_SHAPE1 { get; set; }
        public int DW_DIE_CHK_START { get; set; }
        public int DW_IM_DRAWING { get; set; }
        public int DW_REDRAWING { get; set; }
        public Single? DW_DIE_DIM { get; set; }
        public string DW_REMARKS { get; set; }
        public int DW_DW_TABA_NO_1 { get; set; }
        public int DW_DW_TABA_NO_2 { get; set; }
        public int DW_DW_TABA_NO_3 { get; set; }
        public int DW_DW_TABA_NO_4 { get; set; }
        public int DW_DW_ACT_DIM_CHK { get; set; }
        public string DW_MACHINE_NAME { get; set; }
        public string DW_LOCATION_NAME { get; set; }

        public int DW_UNIT_TYPE { get; set; }
        public string DW_UNIT_TYPE_STRING { get; set; }
        public int DW_DW_NEXT_PROCESS { get; set; }
        public int DW_DIE_CHK_END { get; set; }

        public string DW_DECISION_STRING { get; set; }
        public string DW_DW_TABA_NO_1_STRING { get; set; }
        public string DW_DW_TABA_NO_2_STRING { get; set; }
        public string ALL_TABA_NO { get; set; }
        public decimal? PROCESS_STATUS { get; set; }
        public int? TABA_NO_INT { get; set; }
        public string TABA_NO { get; set; }
        public string TABA_NO_1 { get; set; }
        public string TABA_NO_2 { get; set; }
        public string TABA_NO_3 { get; set; }
        public string TABA_NO_4 { get; set; }
        public int DECISION { get; set; }
        public string DW_TABA_NO_1_2 { get; set; }
        public string DW_TABA_NO_3_4 { get; set; }
        public short DW_IS_REMARKS_UPDATE { get; set; }
        public string DECISION_STRING { get; set; }
        public string PP_PRODUCTION_PROCESS_EN { get; set; }
        public string PP_PRODUCTION_PROCESS_JP { get; set; }
        public string DW_DIE_SHAPE_DECODE { get; set; }
        public string ALL_ACT_DIM { get; set; }
        public string DW_ACT_DIM_START_STRING { get; set; }
        public string DW_ACT_DIM_END_STRING { get; set; }
        public int SHOW_DEFAULT { get; set; }

        public string PAST_DATA_EXIST { get; set; }
        public string PROCESS_KUBUN_EN { get; set; }
        public string PROCESS_KUBUN_JP { get; set; }
        public string NEXT_PROCESS_NAME { get; set; }

        #endregion


    }
}
