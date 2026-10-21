using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class ThabaDetails
    {
        public string NIFUDA_NO { get; set; }
        public string CHUUBAN { get; set; }
        public int GYOUBAN { get; set; }
        public decimal? NO_OF_UNITS { get; set; }
        public string NO_OF_UNITS_STR { get; set; }
        public string UNIT { get; set; }
        public int? UNIT_KUBUN { get; set; }
        public decimal? QUANTITY { get; set; }
        //public string NIFUDA_TYPE { get; set; }             
        public short? NIFUDA_TYPE { get; set; } //created Date[20190916] By Amol Kengar as per DB changes
        public decimal? PRODUCT_HISTORY_CD { get; set; }
        public long? PP_PRODUCT_HISTORY_CD { get; set; }
        public int? SPEC_KUBUN { get; set; }
        public int? PROD_KUBUN { get; set; }
        public int? STD_DEL_PERIOD { get; set; }
        public int? CUSTOMER_DEPARTMENT_CD { get; set; }
        public int? SALES_INCHARGE_CD { get; set; }
        public string SALES_INCHARGE_NAME { get; set; }
        public int? DEPARTMENT_CD { get; set; }
        public int? LOCATION_CD { get; set; }
        public int? PRODUCT_NAME_CUSTOMER_CD { get; set; }
        public int? PRODUCT_NAME_INTERNAL_CD { get; set; }
        public string SHAPE { get; set; }
        public decimal? DIM_D { get; set; }
        public decimal? DIM_W { get; set; }
        public string DIM_OTHER { get; set; }
        public decimal? DIM_TOL_MIN { get; set; }
        public decimal? DIM_TOL_MAX { get; set; }
        public decimal? PRODUCT_LENGTH { get; set; }
        public string PROD_LEN_OTHER { get; set; }
        public decimal? LENGTH_TOLERANCE_MIN { get; set; }
        public decimal? LENGTH_TOLERANCE_MAX { get; set; }
        public int? CHAMFER { get; set; }
        public string MAGARI_KUBUN { get; set; }
        public int? MAGARI_KUBUN_INT { get; set; }
        public string BEND_TOLERANCE { get; set; }
        public decimal? BEND_TOL { get; set; }
        public int? MAGARI_SPAN { get; set; }
        public int? UNIT_WT { get; set; }
        public int? UNIT_WT_RANGE { get; set; }
        public int? BUNCH_QTY { get; set; }
        public int? CONDITION_CD { get; set; }
        public string REMARKS { get; set; }
        public int? IN_ACTIVE_FLAG { get; set; }
        public int? MATL_SPEC_KUBUN { get; set; }
        public int? MATL_TYPE { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string INTERNAL_NAME_NEW { get; set; }
        public string INTERNAL_NAME { get; set; }
        public int? MATL_CONDITION_CD { get; set; }
        public int? MATL_LENGTH { get; set; }
        public decimal? MATL_DIM_D { get; set; }
        public decimal? MATL_DIM_W { get; set; }
        public string MATL_DIM_OTHER { get; set; }
        public string MATL_SHAPE { get; set; }
        public int? MAKER_SPEC_KUBUN { get; set; }
        public string SPC_NOTE_1 { get; set; }
        public string SPC_NOTE_2 { get; set; }
        public int? MAKER_CD { get; set; }
        public int? MATL_KUBUN { get; set; }
        public string PROD_SPEC_NO { get; set; }
        public string PROD_SPEC_NO1 { get; set; }
        public string PROD_SPEC_NO2 { get; set; }
        public string RECEIVER { get; set; }
        public int? PROD_SPEC_CD { get; set; }
        public string PROC_SEQUENCE { get; set; }
        public decimal? REQ_PROCESSING_QTY { get; set; }
        public decimal? WIP_QTY { get; set; }
        public decimal? STOCK_QTY { get; set; }
        public decimal? STOCK_PER_MONTH_DISP { get; set; }
        public string CHARGE { get; set; }
        public string STAMP { get; set; }
        public int? INZU_UNIT { get; set; }
        public DateTime? DELIVERY_DATE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public DateTime? ACT_ALLOC_DATE { get; set; }
        //public string ACT_ALLOC_DATE_STRING { get; set; }   // Added by pradeep for ThabaCompletion screen  :: 20-04-2016
        public decimal? ACT_ALLOC_QTY { get; set; }
        public int DECISION { get; set; }
        public int? DECISION_VALUE { get; set; }
        public int? TABA_NO_INT { get; set; }
        public string TABA_NO { get; set; }
        public string TABA_NO_1 { get; set; }
        public string TABA_NO_2 { get; set; }
        public string TABA_NO_3 { get; set; }
        public string TABA_NO_4 { get; set; }
        public decimal? PROCESS_STATUS { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string DIM_TOLERANCE { get; set; }
        public string LEN_TOL { get; set; }
        public string dim_dOther { get; set; }
        public int? COMPLETE_FLAG { get; set; }
        public DateTime? NIFUDA_COMPLETE_DATE { get; set; }
        public string PRODUCT_NAME_CUST { get; set; }
        public int? HARDENING_TEMPERING_CD { get; set; }
        public long MI_HARDNESS_CD { get; set; }
        public long DRAWING_CD { get; set; }
        public long EC_CD { get; set; }
        public short REEC { get; set; }
        public int? mI_TS_CD { get; set; }
    }
}
