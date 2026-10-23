using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class ProductTensouDetails
    {
        #region "PROCESS_PLAN"
        public int? PP_PROCESS_PLAN_CD { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public string PP_CHUUBAN { get; set; }
        public short PP_GYOUBAN { get; set; }
        public string PP_DELIVERY_DATE_STRING { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_PROD_NAME_CUST { get; set; }
        public long PP_STD_DEL_PERIOD { get; set; }
        public string PP_STD_DEL_PERIOD_STRING { get; set; }

        public string PP_CUSTOMER_NAME { get; set; }
        public string PP_RECEIVER_NAME { get; set; }
        public int PP_CHAMFER { get; set; }
        public Int16 PP_UNIT_WT { get; set; }
        public short? UNIT_WT_MIN { get; set; }
        public short? UNIT_WT_MAX { get; set; }
        public short? PP_UNIT_WT_KUBUN { get; set; }
        public string PP_UNIT_WT_KUBUN_STRING { get; set; }
        public string PP_COLOR_ID { get; set; }
        public string PP_COLOR { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public Single PP_MATL_DIM_D { get; set; }
        public string PP_CHARGE { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public Single PP_PROD_DIM_D { get; set; }
        public string PP_PROD_DIM_D_STRING { get; set; }
        public string PP_PROD_DIM_OTHER { get; set; }
        public string PP_EXT_TEXTURE { get; set; }
        public string PP_DIM_TOLERANCE { get; set; }
        public string PP_DIM_TOL_MAX { get; set; }
        public string PP_DIM_TOL_MIN { get; set; }
        public string PP_DISPLAY { get; set; }
        public string PP_STAMP { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public Single PP_PROD_LENGTH { get; set; }
        public string PP_LEN_TOL { get; set; }
        public string PP_LENGTH_TOL_MAX { get; set; }
        public string PP_LENGTH_TOL_MIN { get; set; }
        public string PP_PACKING { get; set; }
        public string PP_PACKING_DECODE { get; set; }
        public string PP_NO_OF_UNITS_DECODE { get; set; }
        public short? PP_UNIT { get; set; }
        public decimal? PP_QTY { get; set; }
        public double PP_ACT_ALLOC_QTY { get; set; }
        public string PP_COIL_DIA_INTERNAL { get; set; }    //INT_DIA_MIN
        public string PP_COIL_DIA_EXTERNAL { get; set; }    //EXT_DIA_MAX
        public string PP_COIL_DIA_WINDING { get; set; }     //WINDING_DIRECTION
        public short COIL_DIA_WINDING { get; set; }
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
        public DateTime? PP_PROCESS_PLAN_PLNG_DATE { get; set; }
        public DateTime PROCESS_PLAN_CMPL_DATE { get; set; }
        public DateTime? PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public DateTime? PROCESS_PLAN_START_DATE { get; set; }
        //public string PP_CMPL_DATE_string { get; set; }
        public Single PP_NO_OF_UNITS { get; set; }
        public int MAGARI_KUBUN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public int MAGARI_SPAN { get; set; }
        public string CHARGE { get; set; }
        public string TRANSFER_DATE_START { get; set; }
        public string TRANSFER_DATE_END { get; set; }
        public string PP_MATL_TYPE_CBK { get; set; }
        #endregion

        #region  "PRODUCT TRANSFER"
        public long? PRODUCT_TRANSFER_CD { get; set; }
        public int? TRANSFER_CD { get; set; }
        public int? START_VEHICLEL_CD { get; set; }
        public int? RELAY_VEHICLEL_CD { get; set; }
        public string TRANSIT_TYPE { get; set; }
        public long? PROCESS_PLAN_CD { get; set; }
        public int LOCATION_CD_FROM { get; set; }
        public Int16 LOCATION_CD_TO { get; set; }
        public Int16 TRANSIT_LOCATION_CD { get; set; }
        public Int16 VEHICLE { get; set; }
        public DateTime? TRANSFER_DATE { get; set; }
        public DateTime? PRODUCT_TRANSFER_DATE { get; set; }
        public DateTime? PRODUCT_ACCEPT_PLANNED_DATE { get; set; }
        public int? PRODUCT_NEXT_PROSESS { get; set; }
        public string DATE { get; set; }
        public string TRANSFER_DATE_STRING { get; set; }
        public string TRANSIT_PLANNED_DATE { get; set; }
        public string ACCEPTANCE_DATE_STRING { get; set; }
        public string ACCEPT_PLANNED_DATE { get; set; }
        public DateTime? PLANNED_DATE { get; set; }
        public Int16? TABA_NO_1 { get; set; }
        public Int16? TABA_NO_2 { get; set; }
        public Int16? TABA_NO_3 { get; set; }
        public Int16? TABA_NO_4 { get; set; }
        public string TABA_STRING { get; set; }
        public Single? NO_OF_UNITS { get; set; }
        public short? UNIT_TYPE { get; set; }
        public string UNIT_TYPE_STRING { get; set; }
        public string NO_OF_UNITS_STRING { get; set; }
        public double? QTY { get; set; }
        public Int32? MSG_CD { get; set; }
        public int? PREV_PROSESS { get; set; }
        public int NEXT_PROSESS { get; set; }
        public string NEXT_PROSESS_STRING { get; set; }
        public int? USER_ID { get; set; }
        public int? INPUT_LOCATION_CD { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string LOCATION_FROM { get; set; }
        public string LOCATION_TO { get; set; }
        public DateTime? ACT_ALLOC_DATE { get; set; }
        public double ACT_ALLOC_QTY { get; set; }
        public string ACT_ALLOC_DATE_STRING { get; set; }
        public long PRODUCT_TRANSFER_GROUP_NO { get; set; }
        public long TRANSFER_GROUP_NO { get; set; }
        public short? SERIAL_NUMBER { get; set; }
        public string ORDER_NO_STRING { get; set; }
        public string DIM_W_OTHER { get; set; }
        public string MSG_CONTENTS { get; set; }
        public string PP_MAIL_LENGTH { get; set; }
        public Int32 PP_MATL_LENGTH { get; set; }
        public long? LOCATION_CD { get; set; }
        public string LOCATION { get; set; }
        public string DECISION_STRING { get; set; }
        public int DECISION { get; set; }
        public int DECISION_OK { get; set; }
        public string Remark { get; set; }
        public string LOCATION_CD_FROM_STRING { get; set; }
        public string LOCATION_CD_TO_STRING { get; set; }
        public string INPUT_LOCATION_CD_STRING { get; set; }
        public int? GROUP_COUNT { get; set; }


        public string TRANSFER_REMARK { get; set; }
        public string ACCEPTANCE_REMARK { get; set; }
        public string TRANSFER_START_LOCATION { get; set; }

        public string TRANSFER_PLANNED_LOCATION { get; set; }

        public string TRANSFER_ACCEPTED_LOCATION { get; set; }
        public string CULTURE_NAME { get; set; }
        public int? NIFUDA_CD { get; set; }
        public string NIFUDA_NO { get; set; }
        public string VEHICLE_NAME { get; set; }
        public string CONDITION_NAME { get; set; }
        public int? PP_CONDITION_CD { get; set; }

        #endregion


        #region PRODUCTTensouDetailsOnGrid
        public int? PRODUCT_TENSOU_ROW { get; set; }
        public int? PRODUCT_TENSOU_PROCESS_PLAN_CD { get; set; }
        public string PRODUCT_TENSOU_ORDER_NO { get; set; }
        public int? PRODUCT_TENSOU_TABA_NO_1 { get; set; }
        public int? PRODUCT_TENSOU_TABA_NO_2 { get; set; }
        public int? PRODUCT_TENSOU_TABA_NO_3 { get; set; }
        public int? PRODUCT_TENSOU_TABA_NO_4 { get; set; }
        public Single? PRODUCT_TENSOU_NO_OF_UNITS { get; set; }
        public short? PRODUCT_TENSOU_UNIT_TYPE { get; set; }
        public double? PRODUCT_TENSOU_QTY { get; set; }

        public int? PRODUCT_TENSOU_Result { get; set; }
        public int? PRODUCT_TENSOU_NEXT_PROCESS { get; set; }

        public string TABA_NO { get; set; }
        public string LENGTH_OTHER { get; set; }
        public string PROD_DIM_W_OTHER { get; set; }
        public string NEXT_PROCESS_JP { get; set; }
        public string NEXT_PROCESS_EN { get; set; }
        public string NEXT_PROCESS_NAME { get; set; }

        public int ITEM_TYPE { get; set; }

        #endregion


        #region SHUKKA INSTRUCTION
        public int? SHUKKA_INSTRUCTION_CD { get; set; }
        public decimal? RATE { get; set; }

        #endregion

    }
}
