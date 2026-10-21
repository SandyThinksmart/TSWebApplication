using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class HikinukiDetails
    {
        #region "process_plan"

        public string PROD_SPEC_NO { get; set; }
        public long? PP_PROCESS_PLAN_CD { get; set; }
        public long? PP_CUSTOMER_CD { get; set; }
        public long? PP_EMPLOYEE_CD { get; set; }
        public string PP_CHUUBAN { get; set; }
        public Int16? PP_GYOUBAN { get; set; }
        public string PP_GYOUBAN_STRING{get;set;}
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public long? PP_PRODUCT_HISTORY_CD { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public int? PP_KAKOUIRAI_CD { get; set; }
        public decimal? PP_CHUUBAN_YEAR { get; set; }
        public int? PP_ACT_ALLOC_CD { get; set; }
        public int? PP_ACT_ALLOC_TYPE { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }
        public string HOST_MACHINE_CODE { get; set; }   
        public string PP_DELIVERY_DATE_STRING { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }      //COIL_QTY
        public int? PRODUCT_NYUKO_CD { get; set; }
        public double? PP_QUANTITY { get; set; }
        public decimal? PP_ORIGINAL_QTY { get; set; }
        public Single? PP_NO_OF_UNITS { get; set; }
        public string PP_NO_OF_UNITS_DECODE { get; set; }     //PP.INZU || ' ' || PP.INZU_UNIT AS PP_NO_OF_UNITS_STRING

        public short? PP_UNIT { get; set; }
        public int? PP_MATL_NAME_MAK_CD { get; set; }
        public int? PP_MATL_NAME_INT_CD { get; set; }
        public int? PP_EMPLYOEE_CD { get; set; }
        public int? PP_PROD_NAME_CUST_CD { get; set; }
        public int? PP_RECEIVER_CD { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public Single? PP_MATL_DIM_D { get; set; }

        public string PP_MATL_DIM_D_SHAPE { get; set; }

        public string PP_MATL_DIM_OTHER { get; set; }
        public string PP_CHARGE { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public Single? PP_PROD_DIM_D { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        public Single? PP_DIM_TOL_MAX { get; set; }
        public Single? PP_DIM_TOL_MIN { get; set; }
        public string PP_DIM_TOLERANCE { get; set; }
        public Single? PP_PROD_LENGTH { get; set; }
        public string PP_LENGTH_TOL_MAX { get; set; }
        public string PP_LENGTH_TOL_MIN { get; set; }
        public string PP_LEN_TOL { get; set; }
        public short? PP_CHAMFER { get; set; }
        public int? PP_UNIT_WT { get; set; }
        public string PP_UNIT_WT_STRING { get; set; }
        public Int16? PP_COLOR_ID { get; set; }
        public decimal? PP_LENGTH { get; set; }
        public string PP_COLOR { get; set; }
        public string PP_MAGARI_KUBUN { get; set; }
        //  public decimal? PP_BEND_TOL { get; set; }
        public string PP_BEND_TOL { get; set; }
        public string MAGARI_BEND_TOLLERENCE { get; set; }
        public Int16? PP_MAGARI_SPAN { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_PACKING { get; set; }
        public string PP_PACKING_DECODE { get; set; }
        public string PP_TEST_SPECIMEN { get; set; }
        public string PP_TENSILE_STRENGTH { get; set; }
        public decimal? PP_TENSILE_STRN_MIN { get; set; }
        public decimal? PP_TENSILE_STRN_MAX { get; set; }
        public string PP_YEILD { get; set; }  //YIELD_POINT_PATTERN
        public string PP_ELONGATION1 { get; set; }
        public string PP_HARDNESS1 { get; set; }    //HD_PATTERN
        public string PP_DECARB { get; set; }
        public short? PP_PROD_MNGMNT_KUBUN { get; set; }
        public short? PP_SPCL_MNGMNT_KUBUN { get; set; }
        public short? PP_STORAGE_MNGMNT_KUBUN { get; set; }
        public Int16? PP_STD_DEL_PERIOD { get; set; }
        public string PP_STD_DEL_PERIOD_STRING { get; set; }
        public int? PP_STORAGE_LOCATION { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_MATL_NAME_INT { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_PROD_NAME_CUST { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public string PP_PROCESS_PLAN_REMARK { get; set; }
        public decimal? PP_FINISHED_QTY { get; set; }
        public string PP_REFERENCE_CHUBBAN { get; set; }
        public short? PP_SETSUZAN_KUBUN { get; set; }
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
        public DateTime? PP_PROCESS_PLAN_PLNG_DATE { get; set; }
        public short? PP_PROCESS_PLAN_CMPL_FLAG { get; set; }
        public DateTime? PROCESS_PLAN_START_DATE { get; set; }
        public short? PP_PROCESSS_PLAN_START_DATE { get; set; }
        public short? PP_PROCESS_PLAN_PRINT_FLAG { get; set; }
        public short? PP_PROCESS_PLAN_INACTIVE { get; set; }
        public int? PP_USER_ID { get; set; }
        public DateTime? PP_CREATED_DATE { get; set; }
        public DateTime? PP_UPDATED_DATE { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public DateTime? PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PP_ELONGATION2 { get; set; }   //ELONGATION_PATTERN
        public string PP_PRESS1 { get; set; }
        public string PP_PRESS2 { get; set; }
        public string PP_HARDNESS_2 { get; set; }   //HD_MIN
        public string PP_HARDNESS_3 { get; set; }  //HD_MAX

        public string PP_HARDNESS { get; set; }
        public string PP_DECARB_STD_1 { get; set; }
        public string PP_DECARB_STD_2 { get; set; }
        public string PP_DECARB_STD_3 { get; set; }
        public decimal? PP_DECARB_VAL_1 { get; set; }
        public decimal? PP_DECARB_VAL_2 { get; set; }
        public decimal? PP_DECARB_VAL_3 { get; set; }
        public string PP_COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public short? PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public string PP_COIL_DIA_WINDING { get; set; }     //WINDING_DIRECTION  plz change its datatype as short because in database it is number(1,0) and so error occured
                                                            //    public short? PP_COIL_DIA_WINDING { get; set; }    
       public short? COIL_DIA_INTERNAL { get; set; }
        public short? COIL_DIA_WINDING { get; set; }
        public string PP_PRESS { get; set; }    //REDUCTION_PATTERN

        public string PP_ELONGATION { get; set; }
        public decimal? PP_INZU { get; set; }
        public string PP_INZU_UNIT { get; set; }

        public string PP_NO_OF_COILS { get; set; }   //  PP.INZU || ' ' || PP.INZU_UNIT as PP_NO_OF_COILS 
        public string PP_STAMP { get; set; }
        public string PP_PRODUCT_CONDITION { get; set; }
        public DateTime? PP_ACT_ALLOC_DATE { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public string PP_PROD_NAME_INT { get; set; }
        public string PP_KAKOUIRAI_REMARKS { get; set; }

        public string PP_PRODUCTION_PROCESS { get; set; }
        public string PP_PRODUCTION_PROCESS_EN { get; set; }
        public string PP_PRODUCTION_PROCESS_JP { get; set; }
        public decimal? PP_DIM_D { get; set; }
        public string PP_DIM_OTHER { get; set; }
        public string PP_PROCESS_SPECIFICATION_REMARK { get; set; }
        public short? PP_FACTORY_PRODUCTION_FLAG { get; set; }
        public decimal? PP_DIM_W { get; set; }
        public string PP_HT_CONDITION { get; set; }
        public float? PP_CORNER_RADIUS { get; set; }
        public string PP_MBEU { get; set; }
        public string PP_DIM_D_X_DIM_W { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public int? PP_MATL_LENGTH { get; set; }
        public DateTime? PP_GENERATION_DATE { get; set; }
        public string PP_PROD_SPEC_NO1 { get; set; }
        public string PP_PROD_SPEC_NO2 { get; set; }
        public short? PP_UNIT_WT_KUBUN { get; set; }
        public short? PP_IMPACT_TEST_MIN { get; set; }
        public short? PP_IMPACT_TEST_MAX { get; set; }
        public string PP_IMPACT_TEST { get; set; }
        public short? PP_DELETE_FLAG { get; set; }
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
        public short? PP_MATL_TYPE_CBK { get; set; }
        public string PP_MAKER_NAME_SHORT { get; set; }
        public string PP_MATL_CONDITION { get; set; }
        public float? PP_MATL_DIM_W { get; set; }
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
        public short? PP_PROD_VER { get; set; }
        public string PP_CHAMFER_STRING { get; set; }
        public string PP_JIS_TEST_SPEC { get; set; }

        public string TEST_SPEC_REMARK { get; set; }
        public string PP_CORNER_RADIUS_STRING { get; set; }

        public string MATL_DIM { get; set; }    //Added by Sagar B on 15-5-2017
        public Single PP_MATL_DIM_D1 { get; set; }

        public string PP_PROD_LENGTH_OTHER { get; set; }

        public short? MAGARI_KUBUN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public Int16? MAGARI_SPAN { get; set; }

        public string TEST_SPECIMEN { get; set; }
        public float? TENSILE_STRN_PATTERN1 { get; set; }
        public int TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public short? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public short? TENSILE_STRN_PATTERN2_MAX { get; set; }
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
        public short? HARDNESS_PATTERN2_CODE { get; set; }
        public float? YEILD_PATTERN1 { get; set; }
        public int YEILD_PATTERN1_MIN_MAX { get; set; }
        public int? YEILD_PATTERN2_MIN { get; set; }
        public int? YEILD_PATTERN2_MAX { get; set; }
        public int YEILD_PATTERN3_ACTUAL { get; set; }
        public float? PRESS_PATTERN1 { get; set; }
        public int PRESS_PATTERN1_MIN_MAX { get; set; }
        public int? PRESS_PATTERN2_MIN { get; set; }
        public int? PRESS_PATTERN2_MAX { get; set; }
        public int PRESS_PATTERN3_ACTUAL { get; set; }
        public int IMPACT_TEST_PATTERN1 { get; set; }
        public short? IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public int IMPACT_TEST_PATTERN2_MIN { get; set; }
        public int IMPACT_TEST_PATTERN2_MAX { get; set; }
        public short? IMPACT_TEST_PATTERN3_ACTUAL { get; set; }

        public string DECARB { get; set; }
        public int DECARB_1 { get; set; }
        public int DECARB_2 { get; set; }
        public int DECARB_3 { get; set; }
        public float? DECARB_VAL_1 { get; set; }
        public float? DECARB_VAL_2 { get; set; }
        public float? DECARB_VAL_3 { get; set; }

        public short? DW_REDRAWING { get; set; }

        public short? PP_ROOT_VER_PRODUCT_KEY { get; set; }
        public string PP_INTERNAL_NAME { get; set; }
        public Single? PP_PROD_DIM_W { get; set; }
        public string PP_DIM_TOL_MIN_CHR { get; set; }
        public string PP_DIM_TOL_MAX_CHR { get; set; }
        public string PP_LENGTH_TOL_MIN_CHR { get; set; }
        public string PP_LENGTH_TOL_MAX_CHR { get; set; }
        public string PP_PROD_CONDITION_DESCRIPTION { get; set; }
        public Single? PP_NEJIRE_ANGLE { get; set; }
        public short? PP_NEJIRE_SPAN { get; set; }
        public short? PP_UNIT_WT_MIN { get; set; }
        public short? PP_UNIT_WT_MAX { get; set; }
        public short? PP_IMPACT_TEST_PATTERN1 { get; set; }
        public short? PP_IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public short? PP_IMPACT_TEST_PATTERN2_MIN { get; set; }
        public short? PP_IMPACT_TEST_PATTERN2_MAX { get; set; }
        public short? PP_IMPACT_TEST_PATTERN3_ACTUAL { get; set; }
        public short? RANK_STD { get; set; }
        public string PP_MATL_CONDITION_DESCRIPTION { get; set; }
        public Int16? PP_PROCESS_PLAN_KUBUN { get; set; }
        public double? PP_KAKOUIARI_QTY { get; set; }       
        public DateTime? PP_PROCESS_PLAN_PRINT_DATE { get; set; }
        public string PP_PROCESS_PLAN_PRINT_DATE_STR { get; set; }
        public string PP_UNIT_TYPE_STRING { get; set; }
        public string PP_PROD_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_SPCL_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_STORAGE_MNGMNT_KUBUN_STRING { get; set; }
        public Single? LENGTH_TOL_MIN { get; set; }
        public Single? LENGTH_TOL_MAX { get; set; }

        public string PP_MAGARI_KUBUN_STRING { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }

        public string TENSILE_STRN_PATTERN1_MIN_MAX_STRING { get; set; }
        public string ELONGATION_PATTERN1_MIN_MAX_STRING { get; set; }
        public string HARDNESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string YEILD_PATTERN1_MIN_MAX_STRING { get; set; }
        public string PRESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string IMPACT_TEST_PATTERN1_MIN_MAX_STRING { get; set; }
        public string PP_HT_CONDITION_STRING { get; set; }
        public string PP_MATL_TYPE_CBK_STRING { get; set; }
        public string PP_PROCESS_PLAN_KUBUN_STRING { get; set; }
        public string PROC_DEFECT_SHORT1 { get; set; }
        public string PROC_DEFECT_SHORT1_JP { get; set; }
        public string PROC_DEFECT_SHORT1_EN { get; set; }
        public string PROC_DEFECT_SHORT2 { get; set; }
        public string PROC_DEFECT_SHORT2_JP { get; set; }
        public string PROC_DEFECT_SHORT2_EN { get; set; }
        public string PROC_DEFECT_SHORT3 { get; set; }
        public string PROC_DEFECT_SHORT3_JP { get; set; }
        public string PROC_DEFECT_SHORT3_EN { get; set; }
        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }
        #endregion


        #region "DRAWING"
        public string CULTURE_NAME { get; set; }
        public Int64? DW_DRAWING_CD { get; set; }
        public short? DW_MACHINE_CD { get; set; }
        public DateTime? DW_START_DATE { get; set; }
        public int? DW_START_OPERATOR { get; set; }
        public int? DW_LOCATION_CD { get; set; }
        public string DW_START_OPERATOR_NAME { get; set; }
        public string DW_END_OPERATOR_NAME { get; set; }
        public string DW_NO_OF_COILS { get; set; }
        public double? DW_QTY { get; set; }
        public string DW_METALLIC_PLATE_CODE { get; set; }
        public Single? DW_DIE_DIM { get; set; }
        public string DW_DIE_NO { get; set; }
        public Single? DW_DIM1 { get; set; }   //draw_dim_1
        public Single? DW_UPPER_LOWER_ROLLER_ANGLE1 { get; set; }
        public Single? DW_UPPER_LOWER_ROLLER_ANGLE2 { get; set; }
        public Single? DW_UPPER_ROLLER_ELEVATION { get; set; }
        public Single? DW_TONNAGE { get; set; }   //compression_tonnage
        public Single? DW_HEX_R { get; set; }
        public DateTime? DW_END_DATE { get; set; }
        public int? DW_END_OPERATOR { get; set; }
        public short? DW_DECISION { get; set; }
        public short? DECISION_OK { get; set; }
        
        public string DW_DECISION_STRING { get; set; }
        public short? DW_ACT_DIM_CHK { get; set; }
        public Single? DW_ACT_DIM1 { get; set; }
        public Single? DW_ACT_DIM2 { get; set; }
        public Single? DW_ACT_DIM3 { get; set; }
        public Single? DW_ACT_DIM4 { get; set; }
        public Single? DW_ACT_DIM5 { get; set; }
        public Single? DW_ACT_DIM6 { get; set; }
        public short? DW_ACT_LENGTH_CHK { get; set; }
        public Single? DW_ACT_LENGTH1 { get; set; }
        public Single? DW_ACT_LENGTH2 { get; set; }
        public Single? DW_ACT_LENGTH3 { get; set; }


        public Single? DW_ACT_LENGTH4 { get; set; }
        public Single? DW_ACT_LENGTH5 { get; set; }
        public Single? DW_ACT_LENGTH6 { get; set; }

        public Single? DW_FINISHED_BARS { get; set; }

        public string DW_FINISHED_BARS_DECODE { get; set; }

        public short? DW_CORRECTED_BARS { get; set; }
        public short? DW_NG_BARS1 { get; set; }   //ng_qty_1
        public Int16? DW_NG_DETAILS1 { get; set; }
        public short? DW_NG_BARS2 { get; set; }   //ng_qty_2
        public Int16? DW_NG_DETAILS2 { get; set; }
        public short? DW_NG_BARS3 { get; set; }   //ng_qty_3
        public Int16? DW_NG_DETAILS3 { get; set; }
        public Single? DW_A_SIDE_FURE1 { get; set; }   //a_edge_distortion_1
        public Single? DW_A_SIDE_FURE2 { get; set; }  //a_edge_distortion_2
        public Single? DW_A_SIDE_FURE3 { get; set; }  //a_edge_distortion_3
        public short? DW_BEND_STATUS { get; set; }
        public string DW_BEND_STATUS_DECODE { get; set; }
        public Single? DW_B_SIDE_FURE1 { get; set; }   //b_edge_distortion_1
        public Single? DW_B_SIDE_FURE2 { get; set; }  //b_edge_distortion_2
        public Single? DW_B_SIDE_FURE3 { get; set; }  //b_edge_distortion_3
        public short? DW_EXT_TEXTURE_STATUS { get; set; }

        public string DW_EXT_TEXTURE_STATUS_DECODE { get; set; }
        public long? DW_MSG_CD { get; set; }
        public DateTime? DW_CREATED_DATE { get; set; }
        public DateTime? DW_UPDATED_DATE { get; set; }
        public double? DW_FINISHED_QTY { get; set; }
        public string DW_FURNACE_NO { get; set; }
        public Single? DW_DIM2 { get; set; }  //draw_dim_2
        public short? DW_TABA_NO_1 { get; set; }
        public short? DW_TABA_NO_2 { get; set; }
        public short? DW_TABA_NO_3 { get; set; }
        public short? DW_TABA_NO_4 { get; set; }
        public short? DW_TP_TEST { get; set; }
        public short? DW_UNIT_TYPE { get; set; }   //unit_type_desc
        public short? UNIT_TYPE { get; set; }
        public string DW_UNIT_TYPE_STRING { get; set; }

        public string DW_DIE_SHAPE { get; set; }

        public short? DW_FINISHED_QTY_CHK { get; set; }
        public string DW_REMARKS { get; set; }
        public short? DW_EC_NG_BAR_TOTAL { get; set; }
        public short? DW_SHIKII_VALUE { get; set; }
        public Single? DW_CARBON_PERCENTAGE { get; set; }
        public Single? DW_MAGNIZ_PERCENTAGE { get; set; }
        public Single? DW_MAGNETISM { get; set; }

        public string DW_MAGNETISM_DECODE { get; set; }

        public short? DW_DISPOSAL_NO { get; set; }
        public short? DW_EC_DONE_CHK { get; set; }
        public Single? DW_EC_COIL_SIZE { get; set; }
        public string DW_EC_COIL_SHAPE { get; set; }
        public short? DW_EC_IS_OK { get; set; }
        public decimal? EC_NG_BAR_TOTAL { get; set; }

        public string DW_MACHINE_NAME { get; set; }

        public string DW_TP_TEST_DESC { get; set; }

        public int? DW_USER_ID { get; set; }

        public string DW_START_DATE_STRING { get; set; }
        public string DW_END_DATE_STRING { get; set; }

        public string DW_TABA_NO_1_2 { get; set; }
        public string DW_TABA_NO_2_3 { get; set; }
        public short? DW_NEXT_PROCESS { get; set; }
        public int TABA_NO { get; set; }
        //Added by Sagar B on 15-5-2017 
        public string DW_ACT_ALLOC_QTY { get; set; }
        public decimal? GROUP_COUNT { get; set; }
        public decimal? TOTAL_ACT_ALLOC_QTY { get; set; }
        public decimal? TOTAL_FINISHED_QTY { get; set; }
        public string DW_TABA_NO_3_4 { get; set; }
        public string DW_DIE_SHAPE_DECODE { get; set; }
        public string MAX_PP_CHUUBAN_GYOUBAN { get; set; }
        public string MIN_PP_CHUUBAN_GYOUBAN { get; set; }
        public int? MAX_TABA { get; set; }
        public int? MIN_TABA { get; set; }

        public string DW_CREATED_DATE_STRING { get; set; } // Added by Girish Badwaik [2017/05/25], for Edit delete screen
        public string DW_UPDATED_DATE_STRING { get; set; } // Added by Girish Badwaik [2017/05/25], for Edit delete screen

        public decimal? NG_BAR_TOTAL { get; set; }  //Added by Sagar B [2017/07/19], For Process Status Search For Factory Screen 
        public string DW_EC_COIL_SIZE_SHAPE { get; set; } //Added by Sagar B [2017/07/19], For Process Status Search For Factory Screen
        public string YOBINA { get; set; } //Added by Sagar B [2017/07/24], For Process Status Search For Factory Screen
        public string DW_NG_DETAILS1_DECODE { get; set; } //Added by Sagar B [2017/07/26], For Process Status Search For Factory Screen
        public string DW_NG_DETAILS2_DECODE { get; set; }
        public string DW_NG_DETAILS3_DECODE { get; set; }
        public string PP_BEND_TOL_STRING { get; set; }
        public string DW_TABA_NO_1_2_3_4 { get; set; }
        public string PP_PROD_DIM_D_STRING { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public string DW_ACT_DIM1_STRING { get; set; }
        public string DW_ACT_DIM2_STRING { get; set; }
        public string DW_ACT_DIM3_STRING { get; set; }
        public string DW_ACT_DIM4_STRING { get; set; }
        public string DW_ACT_DIM5_STRING { get; set; }
        public string DW_ACT_DIM6_STRING { get; set; }
        public string DW_ACT_LENGTH1_STRING { get; set; }
        public string DW_ACT_LENGTH2_STRING { get; set; }
        public string DW_ACT_LENGTH3_STRING { get; set; }
        public string PP_MATL_DIM_D_STRING { get; set; }
        public string DW_DIE_DIM_STRING { get; set; }
        public string DW_DIM1_STRING { get; set; }
        public string DW_DIM2_STRING { get; set; }
        public string DW_UPPER_LOWER_ROLLER_ANG1_STR { get; set; }
        public string DW_UPPER_LOWER_ROLLER_ANG2_STR { get; set; }
        public string DW_UPPER_ROLLER_ELEVATION_STR { get; set; }
        public string DW_TONNAGE_STRING { get; set; }  
        public string DW_HEX_R_STRING { get; set; }
        public string DW_MAGNETISM_STRING { get; set; }
        public string MSG_CONTENTS { get; set; }
        public short DW_IS_REMARKS_UPDATE { get; set; }
        public List<MessageDetails> DW_MESSAGE_LIST { get; set; } 
        public short? INPUT_LOCATION_CD { get; set; }
        public string DW_NG_DETAILS1_NAME { get; set; }
        public string DW_NG_DETAILS2_NAME { get; set; }
        public string DW_NG_DETAILS3_NAME { get; set; }
        public int? MSG_CD { get; set; }
        public string LOCATION_NAME { get; set; }
        public string HOST_TRANSFER_DATE_STRING { get; set; }
        public short PROCESS_KUBUN_CD { get; set; }
        public string ng_details { get; set; }
        public HikinukiDetails chuban { get; set; }
        public List<HikinukiDetails> PROCESS_HISTORY { get; set; }
        public string PAST_DATA_EXIST { get; set; }
        public string PROCESS_KUBUN_EN { get; set; }
        public string PROCESS_KUBUN_JP { get; set; }
        public string NEXT_PROCESS_NAME { get; set; }
        #endregion
    }
}
