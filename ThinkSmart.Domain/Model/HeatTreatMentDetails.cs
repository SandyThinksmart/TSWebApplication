using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Domain.Model
{
    public class HeatTreatMentDetails
    {
        #region "process_plan"

        public string PROD_SPEC_NO { get; set; }
        public long? PP_PROCESS_PLAN_CD { get; set; }
        public long? PP_CUSTOMER_CD { get; set; }
        public long? PP_EMPLOYEE_CD { get; set; }
        public string PP_CHUUBAN { get; set; }
        public Int16? PP_GYOUBAN { get; set; }
        public string PP_GYOUBAN_STRING { get; set; }
        
        public Int16 LOCATION_CD { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public long? PP_PRODUCT_HISTORY_CD { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public int? PP_KAKOUIRAI_CD { get; set; }
        public decimal? PP_CHUUBAN_YEAR { get; set; }
        public int? PP_ACT_ALLOC_CD { get; set; }
        public int? PP_ACT_ALLOC_TYPE { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }

        public string PP_DELIVERY_DATE_STRING { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }      //COIL_QTY

        public decimal? PP_QUANTITY { get; set; }
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
        public Single? PP_LENGTH_TOL_MAX { get; set; }
        public Single? PP_LENGTH_TOL_MIN { get; set; }
        public string PP_LEN_TOL { get; set; }
        public short? PP_CHAMFER { get; set; }
        public Int16? PP_UNIT_WT { get; set; }
        public Int16? PP_COLOR_ID { get; set; }

        public string PP_COLOR { get; set; }
        public string PP_MAGARI_KUBUN { get; set; }
        //  public decimal? PP_BEND_TOL { get; set; }
        public string PP_BEND_TOL { get; set; }
        public string MAGARI_BEND_TOLLERENCE { get; set; }
        public int? PP_MAGARI_SPAN { get; set; }
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
        public decimal? PP_STD_DEL_PERIOD { get; set; }
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
        public short? PP_PROCESS_PLAN_PRINT_FLAG { get; set; }
        public short? PP_PROCESS_PLAN_INACTIVE { get; set; }
        public int? PP_USER_ID { get; set; }
        public DateTime? PP_CREATED_DATE { get; set; }
        public DateTime? PP_UPDATED_DATE { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public DateTime? PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PP_PROCESS_PLAN_CMPL_DATE_STRING { get; set; }
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
        public short? PP_COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public short? PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public short? PP_COIL_DIA_WINDING { get; set; }     //WINDING_DIRECTION  plz change its datatype as short because in database it is number(1,0) and so error occured
                                                            //    public short? PP_COIL_DIA_WINDING { get; set; }    
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
        public string PP_PROCESS_SPECIFICATION_REMARK { get; set; }
        public short? PP_FACTORY_PRODUCTION_FLAG { get; set; }
        public decimal? PP_DIM_W { get; set; }
        public string PP_HT_CONDITION { get; set; }
        public Single? PP_CORNER_RADIUS { get; set; }
        public string PP_MBEU { get; set; }
        public string PP_DIM_D_X_DIM_W { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public Int32? PP_MATL_LENGTH { get; set; }
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

        public string PP_JIS_TEST_SPEC { get; set; }

        public string TEST_SPEC_REMARK { get; set; }


        public string MATL_DIM { get; set; }
        public string PP_MATL_DIM_D1 { get; set; }

        public string PP_PROD_LENGTH_OTHER { get; set; }

        public short? MAGARI_KUBUN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public Int16? MAGARI_SPAN { get; set; }

        public string TEST_SPECIMEN { get; set; }
        public Int16? TENSILE_STRN_PATTERN1 { get; set; }
        public short? TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public short? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public short? TENSILE_STRN_PATTERN2_MAX { get; set; }
        public short? TENSILE_STRN_PATTERN3_ACTUAL { get; set; }
        public Single? ELONGATION_PATTERN1 { get; set; }
        public short? ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public Single? ELONGATION_PATTERN2_MIN { get; set; }
        public Single? ELONGATION_PATTERN2_MAX { get; set; }
        public short? ELONGATION_PATTERN3_ACTUAL { get; set; }
        public Int16? HARDNESS_PATTERN1 { get; set; }
        public short? HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public Int16? HARDNESS_PATTERN2_MIN { get; set; }
        public Int16? HARDNESS_PATTERN2_MAX { get; set; }
        public short? HARDNESS_PATTERN3_ACTUAL { get; set; }
        public short? HARDNESS_PATTERN1_CODE { get; set; }
        public short? HARDNESS_PATTERN2_CODE { get; set; }
        public short? YEILD_PATTERN1 { get; set; }
        public short? YEILD_PATTERN1_MIN_MAX { get; set; }
        public short? YEILD_PATTERN2_MIN { get; set; }
        public short? YEILD_PATTERN2_MAX { get; set; }
        public short? YEILD_PATTERN3_ACTUAL { get; set; }
        public short? PRESS_PATTERN1 { get; set; }
        public short? PRESS_PATTERN1_MIN_MAX { get; set; }
        public Int16? PRESS_PATTERN2_MIN { get; set; }
        public Int16? PRESS_PATTERN2_MAX { get; set; }
        public short? PRESS_PATTERN3_ACTUAL { get; set; }
        public short? IMPACT_TEST_PATTERN1 { get; set; }
        public short? IMPACT_TEST_PATTERN1_MIN_MAX { get; set; }
        public short? IMPACT_TEST_PATTERN2_MIN { get; set; }
        public short? IMPACT_TEST_PATTERN2_MAX { get; set; }
        public short? IMPACT_TEST_PATTERN3_ACTUAL { get; set; }

        public string DECARB { get; set; }
        public short? DECARB_1 { get; set; }
        public short? DECARB_2 { get; set; }
        public short? DECARB_3 { get; set; }
        public Single? DECARB_VAL_1 { get; set; }
        public Single? DECARB_VAL_2 { get; set; }
        public Single? DECARB_VAL_3 { get; set; }

        public short? DW_REDRAWING { get; set; }

        public long? PP_ROOT_VER_PRODUCT_KEY { get; set; }

        public string INTERNAL_NAME { get; set; }

        public Single? PP_PROD_DIM_W { get; set; }

        public string PP_DIM_TOL_MIN_CHR { get; set; }
        public string PP_DIM_TOL_MAX_CHR { get; set; }
        public string LENGTH_TOL_MIN_CHR { get; set; }
        public string LENGTH_TOL_MAX_CHR { get; set; }
        public Single? NEJIRE_ANGLE { get; set; }
        public short? NEJIRE_SPAN { get; set; }
        public short? UNIT_WT_MIN { get; set; }
        public short? UNIT_WT_MAX { get; set; }
        public short? RANK_STD { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public Int16? PROCESS_PLAN_KUBUN { get; set; }
        public double? KAKOUIARI_QTY { get; set; }
        public DateTime? PROCESS_PLAN_PRINT_DATE { get; set; }
        public string PROCESS_PLAN_PRINT_DATE_STRING { get; set; }
        public string PP_CHAMFER_STRING { get; set; }
        public string PP_CORNER_RADIUS_STRING { get; set; }
        public string PP_MATL_TYPE_CBK_STRING { get; set; }
        public string PP_PROCESS_PLAN_KUBUN_STRING { get; set; }
        public string PP_UNIT_TYPE_STRING { get; set; }
        public string HT_UNIT_TYPE_STRING { get; set; }
        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }
        #endregion

        public long? HEAT_TREATMENT_CD { get; set; }
        public short HEAT_TREATMENT_ROW { get; set; }
        public long HEAT_TREATMENT_NUMBER { get; set; }
        public int PROCESS_KUBUN_ID { get; set; }
        // public long? LOCATION_CD { get; set; }
        public Int16? MACHINE_CD { get; set; }
        public string FURNACE_NO { get; set; }
        public string HT_CONDITION { get; set; }
        
        public string PATTERN_NO { get; set; }
        public DateTime? HT_START_DATE { get; set; }
        public long SETUP_CONF_BY { get; set; }
        public long LOADING_CONF_BY { get; set; }
        public DateTime? PLANNED_UNLOAD_DATE { get; set; }
        public string HT_HEADER_REMARK { get; set; }
        public string PLANNED_UNLOAD_DATE_STRING { get; set; }
        public DateTime? HT_END_DATE { get; set; }
        public int? OFFLOADED_BY { get; set; }
        public int? OFFLOAD_CONF_BY { get; set; }
        public long PROCESS_PLAN_CD { get; set; }
        public Int16? COIL_BAR_TYPE { get; set; }
        public string COIL_BAR_TYPE_STRING { get; set; }
        public string LOT_NO { get; set; }
        public Single? HT_NO_OF_UNITS { get; set; }
        public Int16? UNIT_TYPE { get; set; }
        public double? HT_WEIGHT { get; set; }
        public string ROLL_MONTH { get; set; }
        public string COIL_NO { get; set; }
        public string LOCATION_IN_FURNACE1 { get; set; }
        public string LOCATION_IN_FURNACE2 { get; set; }
        public short? HT_TABA_NO_1 { get; set; }
        public short? HT_TABA_NO_2 { get; set; }
        public short? HT_TABA_NO_3 { get; set; }
        public short? HT_TABA_NO_4 { get; set; }
        public string TABA_NO { get; set; }
        public Int16? HT_NEXT_PROCESS { get; set; }        
        public string HT_REMARKS { get; set; }
        public int? USER_ID { get; set; }
        public Int16? INPUT_LOCATION_CD { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CHUUBAN { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public string CHARGE { get; set; }
        public string MATL_DIM_STRING { get; set; }
        public string PROD_DIM_STRING { get; set; }
        public string MACHINE_NAME { get; set; }
        public string PROC_CONDITION_STRING { get; set; }
        public string PP_HT_CONDITION_STRING { get; set; }
        
        public string HT_TABA_NO_1_2_3_4 { get; set; }
        public string HT_START_DATE_STRING { get; set; }
        public string HT_END_DATE_STRING { get; set; }
        public string LOCATION_NAME { get; set; }
        public string FURNACE_POSITION { get; set; }
        public string SETUP_CONF_BY_STRING { get; set; }
        public string PROC_LOCATION_NAME { get; set; }

        public string MATERIAL_DIA { get; set; }
        public string PRODUCT_DIA { get; set; }
        public long? MSG_CD { get; set; }
        public string PROD_CONDITION_DESCRIPTION { get; set; }
        public string MATL_CONDITION_DESCRIPTION { get; set; }
        public string SETUP_CONF_BY_EMPLOYEE_NAME { get; set; }
        public string LOADING_CONF_BY_EMPLOYEE_NAME { get; set; }
        public string OFFLOADED_BY_EMPLOYEE_NAME { get; set; }
        public string OFFLOAD_CONF_BY_EMPLOYEE_NAME { get; set; }
        public int HT_IS_REMARKS_UPDATE { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }
        public string COLOR_ID_STRING { get; set; }
        public string TENSILE_STRN_PATTERN1_MIN_MAX_STRING { get; set; }
        public string ELONGATION_PATTERN1_MIN_MAX_STRING { get; set; }
        public string HARDNESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string TENSILE_STRN_PATTERN3_ACTUAL_STRING { get; set; }
        public string ELONGATION_PATTERN3_ACTUAL_STRING { get; set; }
        public string HARDNESS_PATTERN3_ACTUAL_STRING { get; set; }
        public string YEILD_PATTERN1_MIN_MAX_STRING { get; set; }
        public string YEILD_PATTERN3_ACTUAL_STRING { get; set; }
        public string PRESS_PATTERN1_MIN_MAX_STRING { get; set; }
        public string PRESS_PATTERN3_ACTUAL_STRING { get; set; }
        public string IMPACT_TEST_PATTERN1_MIN_MAX_STRING { get; set; }
        public string IMPACT_TEST_PATTERN3_ACTUAL_STRING { get; set; }       
        public bool ISVALIDCHUUBAN { get; set; }
        public int CHUUBAN_CODE { get; set; }
        public DateTime? PROCESS_PLAN_START_DATE { get; set; }
        public string PP_PROD_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_SPCL_MNGMNT_KUBUN_STRING { get; set; }
        public string PP_STORAGE_MNGMNT_KUBUN_STRING { get; set; }
        public string PROCESS_KUBUN_EN { get; set; }
        public string PROCESS_KUBUN_JP { get; set; }
        public string NEXT_PROCESS_NAME { get; set; }










    }
}
