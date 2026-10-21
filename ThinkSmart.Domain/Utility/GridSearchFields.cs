using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public class GridSearchFields
    {
        public string CHUUBAN_GYOUBAN { get; set; }
        public string CHUUBAN { get; set; }
        public short? GYOUBAN { get; set; }
        public string MANUFACTURINGSPECIFICATIONNO1 { get; set; }
        public string PROD_SPEC_NO1 { get; set; }
        public string MANUFACTURINGSPECIFICATIONNO2 { get; set; }
        public string PROD_SPEC_NO2 { get; set; }
        public short? VER { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string RECEIVER_NAME { get; set; }
        public string SALES_INCHARGE { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public int? INTERNAL_NAME_CD { get; set; }
        public string INTERNAL_NAME_STRING { get; set; }
        public decimal? PROD_LENGTH { get; set; }
        public Single? PROD_LENGTH_MIN { get; set; }
        public Single? PROD_LENGTH_MAX { get; set; }
        public string CHARGE { get; set; }
        public string ACT_ALLOC_DATE { get; set; }
        public int? MACHINE_CD { get; set; }
        public string MACHINE_NAME { get; set; }
        public short? LOCATION_CD { get; set; }
        public string LOCATION_NAME { get; set; }
        public string START_DATETIME { get; set; }
        public string TRANSFER_DATE_START { get; set; }
        public string TRANSFER_DATE_END { get; set; }
        public int? START_OPERATOR_CD { get; set; }
        public int? METALLIC_LABEL_NO { get; set; }
        public int? METALLIC_PLATE_CODE { get; set; }
        public string END_DATETIME { get; set; }
        public int? END_OPERATOR_CD { get; set; }
        public string FURNACE_NO { get; set; }
        public string DIE_NO { get; set; }
        public string HT_CONDITION { get; set; }
        public string ROLLING_NUMBER { get; set; }
        public string SHAPE { get; set; }
        public Single? LENGTH_TOL_MIN { get; set; }
        public Single? LENGTH_TOL_MAX { get; set; }
        public short? INPUT_LOCATION_CD { get; set; }
        public string MATL_NAME_MAK { get; set; }
        public string MATL_SHAPE { get; set; }
        public decimal? MATL_DIM_D { get; set; }
        public decimal? MATL_DIM_W { get; set; }
        public string MATL_DIM_OTHER { get; set; }
        public int? MATL_LENGTH { get; set; }
        public decimal? KAKOUIARI_QTY { get; set; }
        public string DELIVERY_DATE { get; set; }
        public string DELIVERY_DATE_MIN { get; set; }
        public string DELIVERY_DATE_MAX { get; set; }
        public Single? NO_OF_UNITS { get; set; }
        public short? UNIT { get; set; }
        public double? ACT_ALLOC_QTY { get; set; }
        public string PROD_SPEC_NO { get; set; }
        public long? CHAMFERING_CD { get; set; }
        public long? STRAIGHTENING_CD { get; set; }
        public long? MI_HARDNESS_CD { get; set; }
        public long? MI_TS_CD { get; set; }
        public short? PROD_CONDITION_CD { get; set; }
        public short? MATL_CONDITION_CD { get; set; }
        public string TABA_NO_STRING { get; set; }
        public short? TABA_NO { get; set; }
        public short? TABA_NO_1 { get; set; }
        public short? TABA_NO_2 { get; set; }
        public short? TABA_NO_3 { get; set; }
        public short? TABA_NO_4 { get; set; }
        public string PROD_SHAPE { get; set; } 
        public string PP_DIM_TOL_MIN { get; set; }
        public string PP_DIM_TOL_MAX { get; set; }
        public Single? PROD_DIM_D { get; set; }
        public Single? PROD_DIM_D_MIN { get; set; }
        public Single? PROD_DIM_D_MAX { get; set; }
        public float? PROD_DIM_W { get; set; }
        //public decimal? PROD_DIM_OTHER { get; set; }
        public Single? DIM_TOL_MIN { get; set; }
        public Single? DIM_TOL_MAX { get; set; }
        public string DIM_TOL_MIN_CHR { get; set; }
        public string DIM_TOL_MAX_CHR { get; set; }
        public string LENGTH_OTHER { get; set; }
        public string INSPECTION_DATE { get; set; }
        public string INSPECTION_DATE1 { get; set; }
        public string INSPECTION_DATE2 { get; set; }
        public string LENGTH_TOL_MIN_CHR { get; set; }
        public string LENGTH_TOL_MAX_CHR { get; set; }
        public long? KAKOUIRAI_CD { get; set; }
        public int? ACT_ALLOC_CD { get; set; }
        public int? CUSTOMER_CD { get; set; }
        public int? PROCESS_PLAN_CD { get; set; }
        public short? DECISION { get; set; }
        public string DECISION_STRING { get; set; }
        public int? USER_ID { get; set; }
        public short? MAGNETIC_INSPECTIONS { get; set; }
        public decimal? FINISHED_COILS { get; set; }
        public decimal? FINISHED_QTY { get; set; }
        public int? EMPLOYEE_CD { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public long? ROOT_VER_PRODUCT_KEY { get; set; }
        public string START_OPERATOR { get; set; }
        public string END_OPERATOR { get; set; }

        public string NIFUDA_STRING { get; set; }
        public List<TSSortField> SORT_FIELDS { get; set; }

        public string PROD_VER { get; set; }
        public string PATTERN_NO { get; set; }
        public string PROD_CONDITION_STRING { get; set; }
        public string MATL_KUBUN_NAME { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string MAT_START_DIM_D { get; set; }
        public string MAT_END_DIM_D { get; set; }

        public string MATL_START_LENGTH { get; set; }
        public string MATL_END_LENGTH { get; set; }
        public int FUTOU_CD { get; set; }
        public int MOCHIKOMISAKI_CD { get; set; }

        public string RAWMATL { get; set; }
        public int? MATL_NAME_MAKER_CD { get; set; }

        public String KOUBAN { get; set; }

        public String ROW_MATL { get; set; }
        public String PROC_CONDITION_STRING { get; set; }
        public string ACT_ALLOC_DATE_STRING { get; set; }

        public int? PROCESS_CD { get; set; }
        public string DISPLAYPROCESSPLANDATA { get; set; }
        public string UNIT_TYPE_STRING { get; set; }

        public string PROD_DIM_OTHER_STRING { get; set; }
        public string PROCESS_NAME_STRING { get; set; }
        public Int16? NEXT_PROCESS_CD { get; set; }

        public string WIP_STATUS { get; set; }
        public string RESPONCE { get; set; }
        public string THABA_NO_STRING { get; set; }



        public string COMPLETE_START_DATE { get; set; }
        public string COMPLETE_END_DATE { get; set; }
        public string UPDATE_START_DATE { get; set; }
        public string UPDATE_END_DATE { get; set; }
        public Single? MAX_ROW_COUNT { get; set; }

        public int? MATL_LENGTH_MIN { get; set; }
        public int? MATL_LENGTH_MAX { get; set; }


        public string ACT_ALLOC_START_DATE { get; set; }
        public string ACT_ALLOC_END_DATE { get; set; }
        public string BEGINNING_START_DATE { get; set; }
        public string BEGINNING_END_DATE { get; set; }
        public string METALLIC_PLATE_CODE_STRING { get; set; }
        public string END_DATE1 { get; set; }
        public string END_DATE2 { get; set; }
        public int? DEPARTMENT_GROUP_CD { get; set; }
        public short? PROCESS_PLAN_KUBUN { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public short? MATL_TYPE_CBK { get; set; }
        public Single? MATL_DIM_D_MIN { get; set; }
        public Single? MATL_DIM_D_MAX { get; set; }
        public string GYOUBAN_HOST { get; set; }
        public string PROD_LENGTH_STRING { get; set; }
        public int? ACT_ALLOC_QTY_INT { get; set; }
        public float? HPO_INZU { get; set; }

        public DateTime? HOST_ACT_ALLOC_DATE { get; set; }
        public string HOST_UNIT_TYPE_STRING { get; set; }
        public short? HOST_DELIVERY_DATE { get; set; }

        public float? HOST_MATL_DIM_D { get; set; }
        public float? HOST_MATL_DIM_W { get; set; }
        public string UNIT_WT { get; set; }
        public short? COLOR_ID { get; set; }
        public string COLOR_NAME { get; set; }
        public short? MAGARI_KUBUN { get; set; }
        public short? MAGARI_SPAN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }

        public string EXT_TEXTURE { get; set; }
        public string DISPLAY { get; set; }
        public string TEST_SPECIMEN { get; set; }
        public string DECARB_1 { get; set; }
        public string DECARB_2 { get; set; }
        public string DECARB_3 { get; set; }
        public string DECARB_4 { get; set; }
        public Single? DECARB_VAL_3 { get; set; }
        public string PROD_POINT_1 { get; set; }
        public string PROD_POINT_2 { get; set; }
        public string PROD_POINT_3 { get; set; }
        public string PROD_POINT_4 { get; set; }
        public string PROD_POINT_5 { get; set; }
        public string PROD_POINT_6 { get; set; }
        public string PROD_POINT_7 { get; set; }
        public string PROD_POINT_8 { get; set; }
        public string PROD_POINT_9 { get; set; }
        public string PROD_POINT_10 { get; set; }
        public string PROD_POINT_11 { get; set; }
        public string PROD_POINT_12 { get; set; }
        public string PROD_POINT_13 { get; set; }
        public int? PROCESS_1_CD { get; set; }
        public int? PROCESS_2_CD { get; set; }
        public int? PROCESS_3_CD { get; set; }
        public int? PROCESS_4_CD { get; set; }
        public int? PROCESS_5_CD { get; set; }
        public int? PROCESS_6_CD { get; set; }
        public int? PROCESS_7_CD { get; set; }
        public int? PROCESS_8_CD { get; set; }
        public int? PROCESS_9_CD { get; set; }
        public int? PROCESS_10_CD { get; set; }
        public int? PROCESS_11_CD { get; set; }
        public int? PROCESS_12_CD { get; set; }
        public int? PROCESS_13_CD { get; set; }
        public int? RECEIVER_CD { get; set; }
        public string DECARB_VAL_1 { get; set; }
        public string DECARB_VAL_2 { get; set; }
        public Int64? PROCESS_PLAN_CD_HOST { get; set; }
        public long? PRODUCT_CD { get; set; }
        public short? PRODUCT_CONDITION_CD { get; set; }
        public short? STD_DEL_PERIOD { get; set; }
        public short? COIL_DIA_INTERNAL { get; set; }
        public short? COIL_DIA_EXTERNAL { get; set; }
        public short? COIL_DIA_WINDING { get; set; }
        public float? NEJIRE_ANGLE { get; set; }
        public short? NEJIRE_SPAN { get; set; }
        public short? UNIT_WT_KUBUN { get; set; }
        public short? UNIT_WT_MIN { get; set; }
        public short? UNIT_WT_MAX { get; set; }
        public short? PACKING_CD { get; set; }
        public short? TENSILE_STRN_PATTERN1 { get; set; }
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
        public string TEST_SPEC_REMARKS { get; set; }
        public short? RANK_STD { get; set; }
        public Single? CORNER_RADIUS { get; set; }
        public string MBEU { get; set; }
        public string PRINT_CONDITION { get; set; }
        public short? HT_CONDITION_CD { get; set; }
        public string MACHINE_ID { get; set; }
        public int? PRINT_DATE { get; set; }
        public string OPERATOR_NAME { get; set; }

        public string PROD_CHARGE { get; set; }
        public Single? MST_PROD_DIM_D { get; set; }
        public DateTime? PROD_CREATED_DATE { get; set; }
        public Single? HPO_DIM_TOL_MIN { get; set; }
        public Single? HPO_DIM_TOL_MAX { get; set; }
        public string PRODUCT_NAME_CUST { get; set; }
        public string MST_PROD_SPEC_NO1 { get; set; }
        public string MST_PROD_SPEC_NO2 { get; set; }

        public float? MATERIAL_LENGTH { get; set; }
        public string STAMP { get; set; }
        public Single? HOST_LENGTH_TOL_MIN { get; set; }
        public Single? HOST_LENGTH_TOL_MAX { get; set; }
        public string COIL_DIA_INT { get; set; }
        public string COIL_DIA_EXT { get; set; }
        public string COIL_WND_DIR { get; set; }
        public string CHAMFERING { get; set; }
        public string TENSILE_STRN_PATTERN1_STRING { get; set; }
        public string ELONGATION_1 { get; set; }
        public string ELONGATION_2 { get; set; }
        public string PRESS_1 { get; set; }
        public string PRESS_2 { get; set; }
        public string YEILD { get; set; }

        public string MAGARI_KUBUN_STRING { get; set; }
        public string MAGARI_SPAN_STRING { get; set; }
        public string MAGARI_BEND_TOL_STRING { get; set; }
        public string HT_SETUP_CONF_BY_OPERATOR { get; set; }
        public string HT_LOADING_CONF_BY_OPERATOR { get; set; }
        public string HT_OFFLOADED_BY_OPERATOR { get; set; }
        public string HT_OFFLOAD_CONF_BY_OPERATOR { get; set; }

        public string HARDNESS_1 { get; set; }
        public string HARDNESS_2 { get; set; }
        public string HARDNESS_3 { get; set; }
        public string METALLIC_LABEL_NO_ROLL_MONTH { get; set; }
        public string QUENCHING_MACHINE { get; set; }
        public string TEMPERING_MACHINE{get;set;}
        public string Q_FURNACE_IN_START_DATE { get; set; }
        public string Q_FURNACE_IN_END_DATE { get; set; }

        public string Q_FURNACE_OUT_START_DATE { get; set; }
        public string Q_FURNACE_OUT_END_DATE { get; set; }

        public string T_FURNACE_IN_START_DATE { get; set; }
        public string T_FURNACE_IN_END_DATE { get; set; }
        public string T_FURNACE_OUT_START_DATE { get; set; }
        public string T_FURNACE_OUT_END_DATE { get; set; }
        public string Q_OPERATOR { get; set; }
        public string T_OPERATOR { get; set; }
        public int ROW_LIMIT { get; set; }
        public short? PROCESS_KUBUN_CD { get; set; }
        public string NBNNO { get;set; }
        public string NBNGYO { get; set; }
        public string NBNNO_NBNGYO { get; set; }
        public string CULTURE_NAME { get; set; }
        public string CHHUBAN_YEAR { get; set; }
        public long DRAWING_CD { get; set; }
        public long STR_STRAIGHTENING_CD { get; set; }
        public short ISCHECKED { get; set; }
        public string KUBUN_NAME_JP { get; set; }
        public string DIM_TOL { get; set; }
        public string DIM_TOL_MAX_SPLIT { get; set; }
        public string DIM_TOL_MIN_SPLIT { get; set; }
        public string LEN_TOL { get; set; }
        public string LEN_TOL_MAX_SPLIT { get; set; }
        public string LEN_TOL_MIN_SPLIT { get; set; }
        public string CONDITION_DESCRIPTION { get; set; }
        public string PROCDUCT_KUBUN_STRING { get; set; }
        public short? PRODUCT_KUBUN { get; set; }
        public string MAGARI_BEND_TOLLERENCE { get; set; }
        public string MAKER_NAME { get; set;}
        
        public int? MAKER_CD { get; set; }
        public string MUKESAKI_NAME { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string MATL_SPECIAL_KUBUN { get; set; }
        public string MOCHIKOMISAKI_NAME { get; set; }
        public string HTSAKI_NAME { get; set; }
        public string MATERIAL_TYPE_NAME_CBK { get; set; }
        public int? MATERIAL_CD { get; set; }
        public int? ACTUAL_STOCK { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public int? ACTIVE_FLAG { get; set; }
        public int ROLL_YEAR { get; set; }
        public int ROLL_R { get; set; }
        public string MATL_SPECIAL_KUBUN_NAME { get; set; }
        public Single? DIM_D { get; set; }
        public short? UNCONFIRMED {  get; set; }
        public int INT_MATL_FC_SUMMARY_CD { get; set; }
        public float? RATE { get; set; }
        public string SQL_NAME { get; set; }
        public string FILE_NAME { get; set; }
        public short? ADD_DATE_FLAG { get; set; }
        public short? ADD_TIME_FLAG { get; set; }        
        public short? ROLL_FC_YEAR { get; set; }
        public short? ROLL_FC_R { get; set; }
        public short? CHAMFER { get; set; }
        public string CUSTOMER_CODE {  get; set; }
        public string MAKER_CODE {  get; set; }
        public string DEPT_CODE { get; set; }
        public string INCHARGE_CODE { get; set; }
        public string PACKING_NAME { get;set; }
        public string PROCESS_NAME1{ get; set; }
        public string PROCESS_NAME2{ get; set; }
        public string PROCESS_NAME3{ get; set; }
        public string PROCESS_NAME4{ get; set; }
        public string PROCESS_NAME5{ get; set; }
        public string PROCESS_NAME6{ get; set; }
        public string PROCESS_NAME7{ get; set; }
        public string PROCESS_NAME8{ get; set; }
        public string PROCESS_NAME9{ get; set; }
        public string PROCESS_NAME10 { get; set; }
        public string PROCESS_NAME11 { get; set; }
        public string PROCESS_NAME12 { get; set; }
        public string PROCESS_NAME13 { get; set; }
        public short? DEPARTMENT_CD { get; set; }
        public int? PROD_RECEIVER_CD { get; set; }
        public int HISTORY_MONTH1 { get; set; }
        public int HISTORY_MONTH2 { get; set; }
        public int HISTORY_MONTH3 { get; set; }
        public string CHUUBAN_GYOUBAN_STRING { get; set; }
        public string MATL_NAME_INT_NEW { get; set; }
        public string MATL_NAME_INT { get; set; }
        public MonthRollHeaderDto MonthRollHeaders { get; set; }
        public string PROD_DIM_OTHER { get; set; }
        public int? GRID_ROW_NO { get; set; }
        public string UNIQUE_TEMPLATE_NAME { get; set; }
        public string FUHEN_NO { get; set; }
        public int? IMPACT_TEST_PATTERN { get; set; }
        public int? IMPACT_TEST_MIN { get; set; }
        public int? IMPACT_TEST_MAX { get; set; }
        public string IMPACT_TEST_INFO { get; set; }
        public short? MUKESAKI_CD { get; set; }

        public long? VEHICLE_CD { get; set; }
        public string VEHICLE_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public string YOBIGANA { get; set; }
        public string YOBINA { get; set; }
        public string DRIVER_NAME { get; set; }
        public long? HOST_REFERENCE_NO { get; set; }
        public string KAKOUIRAI_REMARK { get; set; }
        public int? ACTIVE_CHUUBAN_ONLY { get; set; }
        public string REMARK { get; set; }
        public string MATL_NAME_MAKER { get; set; }
        public decimal? DIM_D1 { get; set; }
        public decimal? DIM_D2 { get; set; }
        public string CREATED_DATE1 { get; set; }
        public string CREATED_DATE2 { get; set; }

    }
    public class MonthRollHeaderDto
    {
        public string Month1 { get; set; }
        public string Month2 { get; set; }
        public string Month3 { get; set; }
        public string Roll1 { get; set; }
        public string Roll2 { get; set; }
        public string Roll3 { get; set; }
        public string Roll4 { get; set; }
    }
    public class CommonFields
    {
        public string CHUUBAN { get; set; }
        public short GYOUBAN { get; set; }
        public string PROCESS_PLAN_START_DATE { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public int? RESPONCE { get; set; }
        public string DIGIT_IN_CHUUBAN { get; set; }
        public string GYOUBAN_IN_CHUUBAN { get; set; }
        public string ORDER_NO_IN_CHUUBAN { get; set; }
        public string YEAR_IN_CHUUBAN { get; set; }
        public string DIGIT_IN_NIFUDA { get; set; }
        public string NIFUDA_NO { get; set; }
        public string StartDate { get; set; }
        public string PRODUCT_NAME_CUST { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_CD { get; set; }
        public string CHARGE { get; set; }
        public string MATDIMD { get; set; }
        public string MATNAMEMAKER { get; set; }
        public string FILENAME { get; set; }
        public string MAKER_MILL_SHEET_CD { get; set; }
        public DateTime? NIFUDA_COMPLETE_DATE { get; set; }

    }
}
