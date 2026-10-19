using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class RollSalesForecastDetails
    {
        public string PROD_SPEC_NO { get; set; }
        public int CUSTOMER_CD { get; set; }
        public int EMPLOYEE_CD { get; set; }
        public int PROD_SPEC_CD { get; set; }
        public long? PRODUCT_CD { get; set; }
        public int PRODUCT_KUBUN { get; set; }
        public int MATL_KUBUN { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public int INTERNAL_NAME_CD { get; set; }
        public string INTERNAL_NAME_STRING { get; set; }
        public string PRODUCT_NAME_CUST { get; set; }
        public Single? PP_PROD_DIM_D { get; set; }
        public decimal? PROD_DIM_W { get; set; }
        public decimal? PROD_DIM_D { get; set; }
        public string PROD_DIM_OTHER { get; set; }
        public string SHAPE { get; set; }
        public Single? PROD_LENGTH { get; set; }
        public string LENGTH_OTHER { get; set; }
        public string DIM_TOL { get; set; }
        public string LEN_TOL { get; set; }
        public short? MAGARI_KUBUN { get; set; }
        
        public short? MAGARI_SPAN { get; set; }
        public Single? MAGARI_BEND_TOL { get; set; }
        public string MAGARI_BEND_TOLLERENCE { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_PRODUCT_CONDITION { get; set; }
        public string CONDITION_DESCRIPTION { get; set; }
        public int? MAKER_CD { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string MAKER_NAME { get; set; }
        public long? ROLL_FC_CD { get; set; }
        public long? ROLL_FC_NO { get; set; }
        public long? FC_SRLNO { get; set; }
        public int ROLL_FC_KUBUN { get; set; }
        public int ROLL_FC_R { get; set; }
        public int ROLL_FC_YEAR { get; set; }
        public long? FC_CUST_INCHARGE_CD { get; set; }
        public long? FC_CUSTOMER_CD { get; set; }
        public DateTime ROLL_FC_DATE { get; set; }
        public decimal? FC_QTY_R1 { get; set; }
        public decimal? FC_QTY_R2 { get; set; }
        public decimal? FC_QTY_R3 { get; set; }
        public decimal? FC_QTY_R4 { get; set; }
        public DateTime ROLL_FC_LOCK_DATE { get; set; }
        public DateTime SALES_FC_DONE_DATE { get; set; }
        public string  MSG_TO_SHIZAI { get; set; }
        public string INCHAGE_SHORT_NAME { get; set; }
        public string PROCDUCT_KUBUN_STRING { get; set; }
        public string MATERIAL_KUBUN_STRING { get; set; }
        public int CURRENT_R { get; set; }
        public int CURRENT_YYYY { get; set; }
        public int LAST_R { get; set; }
        public int LAST_YYYY { get; set; }
        public decimal? DISPATCH_QTY1 { get; set; }
        public decimal? DISPATCH_QTY2 { get; set; }
        public decimal? DISPATCH_QTY3 { get; set; }
        public long? MATERIAL_CD { get; set; }
        public double? AVG_DISPATCH_QTY { get; set; }
        public decimal? HISTORY_MONTH1 { get; set; }
        public decimal? HISTORY_MONTH2 { get; set; }
        public decimal? HISTORY_MONTH3 { get; set; }

        public string MATERIAL_TYPE_NAME_CBK { get; set; }

        public decimal? MATL_DIM_W { get; set; }
        public decimal? MATL_DIM_D { get; set; }
        public string MATL_DIM_OTHER { get; set; }
        public string MATL_SHAPE { get; set; }
        public Single? MATERIAL_LENGTH { get; set; }
        public string MATL_SPECIAL_KUBUN { get; set; }
        public String MOCHIKOMISAKI_NAME { get; set; }
        public short? MATL_TYPE_CBK { get; set; }
        public int? USER_ID { get; set; }
        public decimal? WIP_STOCK { get; set; }
        public decimal? PROD_STOCK { get; set; }
        public RollSalesForecastDetails HEADER { get; set; }
        public List<RollSalesForecastDetails> PRODUCT_LIST { get;    set; }
        public List<RollSalesForecastDetails> MATERIAL_LIST { get; set; }
        public long? INT_MATL_FC_SUMMARY_CD { get; set; }
        
        // public List<RollSalesForecastDetails> ProductData { get; set; }
        // public List<RollSalesForecastDetails> MaterialData { get; set; }
        public short? MUKESAKI_CD { get; set; }
        public float? QTY { get; set; }
        public short? ROLL_CHANCE { get; set; }
        public int? SUPPLIER_CD { get; set; }
        public int? MOCHIKOMISAKI_CD { get; set; }
        public short? FUTOU_CD { get; set; }
        public short? HTSAKI_CD { get; set; }
        public string REMARKS { get; set; }

        public string REPLY_FROM_MATL_DEPT { get; set; }
        
        public int? MATL_SPECIAL_KUBUN_INT { get; set; }
        public int? ROLL_YEAR { get; set; }
        public int? ROLL_R { get; set; }
        public float? RATE { get; set; }
        
        public Boolean SALES_FC_DONE { get; set; }
        public string ROLL_FC_LOCK_DATE_STRING { get; set; }
        public string SALES_FC_DONE_DATE_STRING { get; set; }
        public string ROLL_FC_DATE_STRING { get; set; }

        public int? MATL_NAME_MAKER_CD { get; set; }

        public bool CHECKEDBOX_VALUE { get; set; }

    }
}
