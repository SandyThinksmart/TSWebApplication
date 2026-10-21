using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class ProcessPlanForYieldSearchDetails
    {
        #region "PROCESS_PLAN"
        public Int64 PP_PROCESS_PLAN_CD { get; set; }
        public string PP_CHUUBAN { get; set; }
        public string PP_PROCESS_NAME { get; set; }
        public string PROCESS_PROD_POINT { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }
        public DateTime? PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PP_DELIVERY_DATE_STRING { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public short? PP_CHAMFER { get; set; }
        public Int32? PP_UNIT_WT { get; set; }
        public int UNIT_WT_MAX { get; set; }
        public int UNIT_WT_MIN { get; set; }
        public string PP_UNIT_WT_MIN_MAX { get; set; }
        public short? PP_COLOR_ID { get; set; }
        public string PP_COLOR { get; set; }
        public DateTime? PROCESS_END_DATE_STRING { get; set; }
        public string END_DATE { get; set; }
        public string PP_MATL_DIM_D { get; set; }
        public string PP_PROD_DIM_D { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        public string PP_CHARGE { get; set; }
        public short? PP_MAGARI_KUBUN { get; set; }
        public Single? PP_BEND_TOL { get; set; }
        public short? PP_MAGARI_SPAN { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public string PP_PROD_NAME_INT { get; set; }
        public string PP_PROD_NAME_CUST { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public string PP_DIM_TOLERANCE { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_STAMP { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public string PP_LEN_TOL { get; set; }
        public string PP_PACKING_DECODE { get; set; }
        public int PP_STD_DEL_PERIOD { get; set; }
        public Single? PP_NO_OF_UNITS { get; set; }
        public short? PP_UNIT { get; set; }
        public string PP_NO_OF_UNITS_DECODE { get; set; }
        public short? PP_COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public short? PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public string PP_COIL_DIA_WINDING { get; set; }
        public short? COIL_DIA_WINDING { get; set; }
        public double PP_ACT_ALLOC_QTY { get; set; }  //COIL_QTY
        public string PP_PROD_SPEC_NO { get; set; }
        public string PP_PROCESS_PLAN_REMARK { get; set; }
        public string PP_KAKOUIRAI_REMARKS { get; set; }
        public short? EC_DEFECT_RATE { get; set; }
        public Single? EC_OK_BARS { get; set; }
        public short? EC_DECISION { get; set; }
        public string EC_COLOR { get; set; }
        public string EC_DECISION_STRING { get; set; }

        public decimal? FI_QTY { get; set; }
        public int? FI_DECISION { get; set; }
        public string FI_DECISION_STRING { get; set; }
        public string FI_COLOR { get; set; }
        public string PROCESS_NAME_JP { get; set; }
        public string PROCESS_NAME_EN { get; set; }
        public string MACHINE_NAME { get; set; }
        public decimal UNIT_TYPE { get; set; }
        public decimal UNIT_TYPE2 { get; set; }
        public decimal UNIT_TYPE3 { get; set; }
        public decimal UNIT_TYPE4 { get; set; }
        public string FINISHED_BARS_DECODE { get; set; }
        public string FINISHED_BARS2_DECODE { get; set; }
        public string FINISHED_BARS3_DECODE { get; set; }
        public string FINISHED_BARS4_DECODE { get; set; }
        public decimal? FINISHED_BARS { get; set; }
        public decimal? FINISHED_BARS2 { get; set; }
        public decimal? FINISHED_BARS3 { get; set; }
        public decimal? FINISHED_BARS4 { get; set; }
        public decimal? FINISHED_QTYS { get; set; }
        public decimal? FINISHED_QTYS2 { get; set; }
        public decimal? FINISHED_QTYS3 { get; set; }
        public decimal? FINISHED_QTYS4 { get; set; }
        public int? EC_DONE_CHECK { get; set; }
        public string EC_DONE_CHECK_DECODE { get; set; }
        public decimal? DECISION_RESULT { get; set; }
        public string RESULT_DECISION_STRING { get; set; }
        public string MESSAGE { get; set; }
        public string END_OPERATOR_NAME { get; set; }
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
        public Single? TENSILE_STRENGTH { get; set; }
        public decimal? ELONGATION { get; set; }
        public Single? CUTTING_ELONGATION { get; set; }
        public Single? FRACTURE_DIM { get; set; }
        public decimal? YIELD_POINT { get; set; }
        public decimal? REDUCTION { get; set; }
        public decimal? GAUGE_LENGTH { get; set; }
        public double? MAX_LOAD { get; set; }
        public decimal? D4_1 { get; set; }
        public decimal? D4_2 { get; set; }
        public decimal? D4_3 { get; set; }
        public decimal? D4_4 { get; set; }
        public decimal? D4_AVG { get; set; }
        public Single? CENTER { get; set; }
        public int? TENSILE_STRN_PATTERN1 { get; set; }
        public short? TENSILE_STRN_PATTERN1_MIN_MAX { get; set; }
        public string TENSILE_STRN_STRING { get; set; }
        public int HARDNESS_PATTERN1 { get; set; }
        public int HARDNESS_PATTERN1_MIN_MAX { get; set; }
        public string STD_HARDNESS_STRING { get; set; }
        public string HARD_MIN_MAX_STRING { get; set; }
        public Int16? HARDNESS_MAX { get; set; }
        public Int16? HARDNESS_MIN { get; set; }
        public int HARDNESS_PATTERN3_ACTUAL { get; set; }
        public int HARDNESS_PATTERN1_CODE { get; set; }
        public int HARDNESS_PATTERN2_CODE { get; set; }
        public decimal? EXTERIOR_CONDITION { get; set; }
        public string EXTERIOR_CONDITION_DECODE { get; set; }
        public decimal? TOTAL_NG_BARS { get; set; }
        public string MESSAGE_DATE_TIME { get; set; }
        public decimal? EC_OK_QTY { get; set; }
        public decimal? EC_NG_QTY { get; set; }
        public int? USER_ID { get; set; }
        public int INPUT_LOCATION_CD { get; set; }
        public int PROCESS_KUBUN_CD { get; set; }
        public long? MSG_CD { get; set; }
        public string TABA_NO { get; set; }
        public string PP_STD_DEL_PERIOD_STRING { get; set; }
        public string CULTURE_NAME { get; set; }
        public long? SERIAL_NO { get; set; }
        public string COMBINE_TABA1 { get; set; }
        public string COMBINE_TABA2 { get; set; }
        public string COMBINE_TABA3 { get; set; }
        public string COMBINE_TABA4 { get; set; }
        public string DIVISION_TABA1 { get; set; }
        public string DIVISION_TABA2 { get; set; }
        public string DIVISION_TABA3 { get; set; }
        public string DIVISION_TABA4 { get; set; }
        public short? FI_COLOR_DECISION { get; set; }
        public short? FI_CHAMFER_DECISION { get; set; }
        public string FI_SPARK_DECISION { get; set; }
        public string FI_COLOR_DECISION_STRING { get; set; }
        public string FI_CHAMFER_DECISION_STRING { get; set; }
        public string FI_SPARK_DECISION_STRING { get; set; }
        public short? BEND_STATUS { get; set; }
        public string BEND_STATUS_DECODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public decimal? ELONGATION_PATTERN1 { get; set; }
        public short? ELONGATION_PATTERN1_MIN_MAX { get; set; }
        public string ELONGATION_PATTERN1_STRING { get; set; }
        public int? PRESS_PATTERN1 { get; set; }
        public short? PRESS_PATTERN1_MIN_MAX { get; set; }
        public string PRESS_PATTERN1_STRING { get; set; }
        public string TEST_SPECIMEN { get; set; }

        //Added
        public long? PP_CUSTOMER_CD { get; set; }
        public int PP_GYOUBAN { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public string PP_PROD_NAME { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public string PP_PROD_DIM_W { get; set; }
        public string PP_PROD_LENGTH { get; set; }
        public string PP_PROD_LENGTH_OTHER { get; set; }
        public string BEND_STD { get; set; }

        public string PP_LENGTH_TOL { get; set; }
        public DateTime? PP_ACT_ALLOC_DATE { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public decimal? PP_FINISHED_QTY { get; set; }
        public string PROD_CONDITION_DESCRIPTION { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public DateTime? PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PROCESS_PLAN_CMPL_DATE_STRING { get; set; }
        public string PP_MATL_SHAPE { get; set; }

        public decimal? HIKINUKI_QTY { get; set; }
        public decimal? STRAIGHTENING_QTY { get; set; }
        public decimal? CHAMFERING_QTY { get; set; }
        public decimal? EC_QTY { get; set; }
        public decimal? HIKINUKI_YIELD { get; set; }
        public decimal? STRAIGHTENING_YIELD { get; set; }
        public decimal? CHAMFERING_YIELD { get; set; }
        public decimal? EC_YIELD { get; set; }
        public decimal? FI_YIELD { get; set; }
        



        public List<ProcessPlanForSalesProductionStatusDetails> GETDATATIMEWISE { get; set; }
        public List<ProcessPlanForSalesProductionStatusDetails> GETDATABUNDLEWISE { get; set; }
        public List<ProcessPlanForSalesProductionStatusDetails> GETDATAHARDNESSANDPOSTHT { get; set; }
        public List<ProcessPlanForSalesProductionStatusDetails> GETPROCESSDATA { get; set; }

        #endregion
    }
}
