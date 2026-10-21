using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class HSKMillSheetDetails
    {
        #region "process_plan"

        public string PROD_SPEC_NO { get; set; }
        public long? PP_PROCESS_PLAN_CD { get; set; }
        public long? PP_CUSTOMER_CD { get; set; }
        public long? PP_EMPLOYEE_CD { get; set; }
        public string PP_CHUUBAN { get; set; }
        public Int16? PP_GYOUBAN { get; set; }
        public string PP_GYOUBAN_STRING { get; set; }
        public string PP_YOBINA { get; set; }
        public string PP_MAKER_NAME { get; set; }
        
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public long? PP_PRODUCT_HISTORY_CD { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public int? PP_KAKOUIRAI_CD { get; set; }
        public decimal? PP_CHUUBAN_YEAR { get; set; }
        public int? PP_ACT_ALLOC_CD { get; set; }
        public int? PP_ACT_ALLOC_TYPE { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }
        public long? PP_MAKER_CD { get; set; }
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
        public string PP_COLOR_ID { get; set; }

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
        public string PP_PROD_LENGTH_STRING { get; set; }

        public short? MAGARI_KUBUN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public Int16? MAGARI_SPAN { get; set; }

        public string TEST_SPECIMEN { get; set; }
        public float TENSILE_STRN_PATTERN1 { get; set; }
        public int TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public short? TENSILE_STRN_PATTERN2_MIN { get; set; }
        public short? TENSILE_STRN_PATTERN2_MAX { get; set; }
        public int TENSILE_STRN_PATTERN3_ACTUAL { get; set; }
        public float ELONGATION_PATTERN1 { get; set; }
        public int ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public Single? ELONGATION_PATTERN2_MIN { get; set; }
        public Single? ELONGATION_PATTERN2_MAX { get; set; }
        public int ELONGATION_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN1 { get; set; }
        public int HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public int HARDNESS_PATTERN2_MIN { get; set; }
        public int HARDNESS_PATTERN2_MAX { get; set; }
        public int HARDNESS_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN1_CODE { get; set; }
        public short? HARDNESS_PATTERN2_CODE { get; set; }
        public float YEILD_PATTERN1 { get; set; }
        public int YEILD_PATTERN1_MIN_MAX { get; set; }
        public int YEILD_PATTERN2_MIN { get; set; }
        public int YEILD_PATTERN2_MAX { get; set; }
        public int YEILD_PATTERN3_ACTUAL { get; set; }
        public float PRESS_PATTERN1 { get; set; }
        public int PRESS_PATTERN1_MIN_MAX { get; set; }
        public int PRESS_PATTERN2_MIN { get; set; }
        public int PRESS_PATTERN2_MAX { get; set; }
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
        public float DECARB_VAL_1 { get; set; }
        public float DECARB_VAL_2 { get; set; }
        public float DECARB_VAL_3 { get; set; }


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
        public string PROC_DEFECT_SHORT2 { get; set; }
        public string PROC_DEFECT_SHORT3 { get; set; }
        public string HARDNESS_PATTERN1_CODE_STRING { get; set; }
        public string HARDNESS_PATTERN2_CODE_STRING { get; set; }
        #endregion

        #region "HSKMILL_SHEET"
        public long? HSK_MILL_SHEET_CD { get; set; }
        public long? PROCESS_PLAN_CD { get; set; }
        public long? CUSTOMER_CD { get; set; }
        public int? IS_CUSTOMER_ANOTHER_NAME { get; set; }
        public string MANUAL_CUSTOMER_NAME { get; set; }
        public int? IS_MAKER_ANOTHER_NAME { get; set; }
        public long? MATL_NAME_MAK_CD { get; set; }
        public string RECEIVER_NAME { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public int? PROD_CONDITION_CD { get; set; }
        public string PROD_SHAPE { get; set; }
        public decimal? PROD_DIM_D { get; set; }
        public decimal? PROD_DIM_W { get; set; }
        public string PROD_DIM_OTHER { get; set; }
        public string PROD_LENGTH { get; set; }
        public int? DIMDSHOWONREPORT { get; set; }
        public string MATL_LENGTH { get; set; }
        public string DELIVERY_DATE { get; set; }
        public decimal? NO_OF_UNITS { get; set; }
        public int? UNIT { get; set; }
        public decimal? WEIGHT { get; set; }
        public int? IS_MATL_DIM { get; set; }
        public int? IS_PROD_DIM { get; set; }
        public long? CHEMICAL_COMPOSITION_CD { get; set; }
        public long? MAKER_MILL_SHEET_CD { get; set; }
        public string ELEMENT_SYMBOL_1 { get; set; }
        public long? MAGNIFICATION_1 { get; set; }
        public decimal? ES_MIN_1 { get; set; }
        public decimal? ES_MAX_1 { get; set; }
        public string ELEMENT_SYMBOL_2 { get; set; }
        public long? MAGNIFICATION_2 { get; set; }
        public decimal? ES_MIN_2 { get; set; }
        public decimal? ES_MAX_2 { get; set; }
        public string ELEMENT_SYMBOL_3 { get; set; }
        public long? MAGNIFICATION_3 { get; set; }
        public decimal? ES_MIN_3 { get; set; }
        public decimal? ES_MAX_3 { get; set; }
        public string ELEMENT_SYMBOL_4 { get; set; }
        public long? MAGNIFICATION_4 { get; set; }
        public decimal? ES_MIN_4 { get; set; }
        public decimal? ES_MAX_4 { get; set; }
        public string ELEMENT_SYMBOL_5 { get; set; }
        public long? MAGNIFICATION_5 { get; set; }
        public decimal? ES_MIN_5 { get; set; }
        public decimal? ES_MAX_5 { get; set; }
        public string ELEMENT_SYMBOL_6 { get; set; }
        public long? MAGNIFICATION_6 { get; set; }
        public decimal? ES_MIN_6 { get; set; }
        public decimal? ES_MAX_6 { get; set; }
        public string ELEMENT_SYMBOL_7 { get; set; }
        public long? MAGNIFICATION_7 { get; set; }
        public decimal? ES_MIN_7 { get; set; }
        public decimal? ES_MAX_7 { get; set; }
        public string ELEMENT_SYMBOL_8 { get; set; }
        public long? MAGNIFICATION_8 { get; set; }
        public decimal? ES_MIN_8 { get; set; }
        public decimal? ES_MAX_8 { get; set; }
        public string ELEMENT_SYMBOL_9 { get; set; }
        public long? MAGNIFICATION_9 { get; set; }
        public decimal? ES_MIN_9 { get; set; }
        public decimal? ES_MAX_9 { get; set; }
        public string ELEMENT_SYMBOL_10 { get; set; }
        public long? MAGNIFICATION_10 { get; set; }
        public decimal? ES_MIN_10 { get; set; }
        public decimal? ES_MAX_10 { get; set; }
        public string ELEMENT_SYMBOL_11 { get; set; }
        public long? MAGNIFICATION_11 { get; set; }
        public decimal? ES_MIN_11 { get; set; }
        public decimal? ES_MAX_11 { get; set; }
        public string ELEMENT_SYMBOL_12 { get; set; }
        public long? MAGNIFICATION_12 { get; set; }
        public decimal? ES_MIN_12 { get; set; }
        public decimal? ES_MAX_12 { get; set; }
        public string ELEMENT_SYMBOL_13 { get; set; }
        public long? MAGNIFICATION_13 { get; set; }
        public decimal? ES_MIN_13 { get; set; }
        public decimal? ES_MAX_13 { get; set; }
        public string ELEMENT_SYMBOL_14 { get; set; }
        public long? MAGNIFICATION_14 { get; set; }
        public decimal? ES_MIN_14 { get; set; }
        public decimal? ES_MAX_14 { get; set; }
        public string ELEMENT_SYMBOL_15 { get; set; }
        public long? MAGNIFICATION_15 { get; set; }
        public decimal? ES_MIN_15 { get; set; }
        public decimal? ES_MAX_15 { get; set; }
        public int? SELECTED_HARDNESS_1 { get; set; }
        public int? SELECTED_HARDNESS_2 { get; set; }
        public int? SELECTED_HARDNESS_3 { get; set; }
        public int? JISDIVIDEBYTEN { get; set; }
        public int? IS_PROD_DIM_TOL { get; set; }
        public int? IS_SHOW_COIL_NO { get; set; }
        public int? IS_DIMTOLSHOWONREPORT { get; set; }
        public int? SHOW_HARD_TENSILE { get; set; }
        public int? COIL_NO_1 { get; set; }
        public int? COIL_NO_2 { get; set; }
        public int? COIL_NO_3 { get; set; }
        public int? COIL_NO_4 { get; set; }
        public int? COIL_NO_5 { get; set; }
        public int? COIL_NO_6 { get; set; }
        public int? COIL_NO_7 { get; set; }
        public int? HARDNESS_METHOD { get; set; }
        public long? MI_HARDNESS_CD1 { get; set; }
        public long? MI_HARDNESS_CD2 { get; set; }
        public long? MI_HARDNESS_CD3 { get; set; }
        public long? MI_HARDNESS_CD4 { get; set; }
        public long? MI_HARDNESS_CD5 { get; set; }
        public long? MI_HARDNESS_CD6 { get; set; }
        public long? MI_HARDNESS_CD7 { get; set; }
        public long? MI_TS_CD1 { get; set; }
        public long? MI_TS_CD2 { get; set; }
        public long? MI_TS_CD3 { get; set; }
        public long? MI_TS_CD4 { get; set; }
        public long? MI_TS_CD5 { get; set; }
        public long? MI_TS_CD6 { get; set; }
        public long? MI_TS_CD7 { get; set; }
        public long? INTERNAL_QUALITY_CD { get; set; }
        public long? FINAL_INSP_CD { get; set; }
        public int? RANK_ACTUAL_1 { get; set; }
        public int? RANK_ACTUAL_2 { get; set; }
        public int? RANK_ACTUAL_3 { get; set; }
        public int? RANK_ACTUAL_4 { get; set; }
        public int? RANK_ACTUAL_5 { get; set; }
        public int? RANK_ACTUAL_6 { get; set; }
        public int? RANK_ACTUAL_7 { get; set; }
        public string MILL_SHEET_REMARK_1 { get; set; }
        public string MILL_SHEET_REMARK_2 { get; set; }
        public string MILL_SHEET_REMARK_3 { get; set; }
        public string MILL_SHEET_REMARK_4 { get; set; }
        public string MILL_SHEET_REMARK_5 { get; set; }
        public string MILL_SHEET_REMARK_6 { get; set; }
        public string MILL_SHEET_REMARK_7 { get; set; }
        public long? USER_ID { get; set; }
        public int? INPUT_LOCATION_CD { get; set; }
        public string CREATED_DATE { get; set; }
        public string UPDATED_DATE { get; set; }

        public string LENGTH1_STRING { get; set; }
        public string LENGTH2_STRING { get; set; }
        public string CHUUBAN { get; set; }
        public short? GYOUBAN { get; set; }
        public string GYOUBAN_STRING { get; set; }
        public string DELIVERY_END_DATE_STRING { get; set; }
        public string DELIVERY_START_DATE_STRING { get; set; }
        public string CULTURE_NAME { get; set; }
       
        
        #endregion

        #region "HSKMILL_POSTHT"
        public string ACT_DM_F { get; set; }
        public string ACT_DM_T_AVG { get; set; }
        public string INT_QLTY_TEST_DATE { get; set; }
        public string STD_DMF { get; set; }
        public string STD_DMT { get; set; }
        public string DMF_VAL { get; set; }
        public string DMT_VAL { get; set; }
        #endregion

        #region "Final Inspection"
        public int FI_TABA_NO { get; set; }
        public decimal? FI_LENGTH { get; set; }
        public decimal? FI_DIM1 { get; set; }
        public decimal? FI_DIM2 { get; set; }
        public decimal? FI_DIM3 { get; set; }
        public int? FI_ACT_RADIUS_1 { get; set; }
        public int? FI_ACT_RADIUS_2 { get; set; }
        public int? FI_ACT_RADIUS_3 { get; set; }
        public int? FI_ACT_RADIUS_4 { get; set; }
        public int? FI_ACT_RADIUS_5 { get; set; }
        public int? FI_ACT_RADIUS_6 { get; set; }
        public decimal? CORNER_RADIUS { get; set; }
        public decimal? MAX_DIMD { get; set; }
        public decimal? MIN_DIMD { get; set; }
        public decimal? FI_MAX_LENGTH { get; set; }
        public decimal? FI_MIN_LENGTH { get; set; }

        #endregion

        #region "Hardness AND Tensile"
        public long? HD_KEY { get; set; }
        public long? TS_KEY { get; set; }
        public int? HD_STD_ACT_2 { get; set; }
        public int? HD_STD_ACT_3 { get; set; }
        public int? TS_CUT_DIM { get; set; }
        public long? TEST_SPECIMEN_CD { get; set; }
        public string TENSILE_STRENGTH_PATTERN { get; set; }
        public string TENSILE_STRENGTH_PATTERN2 { get; set; }
        public string YEILD_PATTERN { get; set; }

       
        public string HARDNESS_1 { get; set; }
        public string HARDNESS_PATTERN2 { get; set; }
        public string HARDNESS_PATTERN3 { get; set; }
        public decimal? ACT_DIM { get; set; }
        public string HD_PATTERN_ACT { get; set; }
        public int? D4_1 { get; set; }

        public int? D4_2 { get; set; }
        public int? D4_3 { get; set; }
        public int? D4_4 { get; set; }
        public decimal? HD_D4_AVG { get; set; }
        public decimal? TS_RESULT { get; set; }

        public decimal? YEILD_ACTUAL { get; set; }
        public decimal? ELONG_RESULT { get; set; }
        public decimal? REDUCT_RESULT { get; set; }

        public int? TEST_IMPACT_STD { get; set; }
        public int? TS_TEST_IMPACT_1 { get; set; }
        public int? TS_TEST_IMPACT_2 { get; set; }
        public int? TS_TEST_IMPACT_3 { get; set; }
        public int? TS_MAX_LOAD_N { get; set; }

        public string REDUCTION_1 { get; set; }

        public string REDUCTION_2 { get; set; }
        public string ELONGATION_1 { get; set; }
        public string ELONGATION_2 { get; set; }

        #endregion


    }
}
