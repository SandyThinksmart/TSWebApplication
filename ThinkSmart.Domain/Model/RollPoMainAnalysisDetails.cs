using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class RollPoMainAnalysisDetails
    {
        public int INT_MATL_FC_SUMMARY_CD { get; set; }

        public int? ROLL_YEAR { get; set; }
        public int? ROLL_R { get; set; }
        public int? EMPLOYEE_CD { get; set; }
        public int? INTERNAL_NAME_CD { get; set; }
        public Single? DIM_D { get; set; }
        public string SHAPE { get; set; }
        public Single? LENGTH { get; set; }
        public int? MATL_SPECIAL_KUBUN { get; set; } 
        public int? MATL_KUBUN { get; set; }
        public Single? PO_QTY_3MONTH_AGO {  get; set; }
        public Single? PO_QTY_2MONTH_AGO { get; set; }
        public Single? PO_QTY_1MONTH_AGO { get; set; }
        public Single? REM_QTY_4MONTH_AGO { get; set; }
        public Single? REM_QTY_3MONTH_AGO { get; set; }
        public Single? REM_QTY_2MONTH_AGO { get; set; }
        public Single? REM_QTY_1MONTH_AGO { get; set; }
        public Single? RECVD_QTY_3MONTH_AGO { get; set; }
        public Single? RECVD_QTY_2MONTH_AGO { get; set; }
        public Single? RECVD_QTY_1MONTH_AGO { get; set; }
        public Single? PO_QTY_THIS_MONTH { get; set; }
        public Single? PO_QTY_1MONTH_AHEAD { get; set; }
        public Single? PO_QTY_2MONTH_AHEAD { get; set; }
        public Single? PO_QTY_3MONTH_AHEAD { get; set; }
        public Single? PO_QTY_4MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_THIS_MONTH { get; set; }
        public Single? PLAN_QTY_1MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_2MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_3MONTH_AHEAD { get; set; }
        public Single? PLAN_QTY_4MONTH_AHEAD { get; set; }
        public Single? SALES_FC_QTY_THIS_MONTH { get; set; }
        public Single? SALES_FC_QTY_1MONTH_AHEAD { get; set; }
        public Single? SALES_FC_QTY_2MONTH_AHEAD { get; set; }
        public Single? SALES_FC_QTY_3MONTH_AHEAD { get; set; }
        public Single? SALES_FC_QTY_4MONTH_AHEAD { get; set; }
        public Single? EXT_STOCK_QTY { get; set; }
        public Single? INT_STOCK_QTY { get; set; }
        public DateTime? PO_REVIEWED_DATE { get;set; }
        public int USER_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public int? CONDITION_CD { get; set; }
        public float? TOTALQTY { get; set; }
        public string INTERNAL_NAME_STRING { get; set; }
        public string MATL_KUBUN_NAME { get; set; }
        public string MATL_SPECIAL_KUBUN_NAME { get; set; }
        public string CONDITION_DESCRIPTION { get; set; }
        public Single? TOT_STOCK_QTY { get; set; }
        public Single? TOT_PO_QTY_MONTH_AGO { get; set; }
        public Single? TOT_REM_QTY_MONTH_AGO { get; set; }
        public Single? TOT_RECVD_QTY_MONTH_AGO { get; set; }
        public int? MATERIAL_CD { get; set; }
        public short? MUKESAKI_CD { get; set; }
        public float? QTY { get; set; }
        public short? ROLL_CHANCE { get; set; }
        public int? SUPPLIER_CD { get; set; }
        public int? MOCHIKOMISAKI_CD { get; set; }
        public short? FUTOU_CD { get; set; }
        public short? HTSAKI_CD { get; set; }
        public string REMARKS { get; set; }
        public short? MATL_TYPE_CBK { get; set; }

        //Added by Namrata 
        //Screen name AnalysePoQuntity
        public string MATL_MAKER_NAME { get; set; }
        public string MAKER_NAME { get; set; }
        public string MUKESAKI_NAME { get; set; }
        
        public float? RATE { get; set; }
        public string SUPPLIER_NAME { get; set;}
        public string FUTOU_NAME { get; set; }
        public string MOCHIKOMISAKI_NAME { get; set; }
        public Single? PO_REM_QTY_PREV { get; set; }
        public Single? PO_REM_QTY_R_3_1 { get; set; }
        public Single? PO_REM_QTY_R_3_2 { get; set; }
        public Single? PO_REM_QTY_R_3_3 { get; set; }
        public Single? PO_REM_QTY_R_3_4 { get; set; }
        public Single? PO_REM_QTY_R_2_1 { get; set; }
        public Single? PO_REM_QTY_R_2_2 { get; set; }
        public Single? PO_REM_QTY_R_2_3 { get; set; }
        public Single? PO_REM_QTY_R_2_4 { get; set; }
        public Single? PO_REM_QTY_R_1_1 { get; set; }
        public Single? PO_REM_QTY_R_1_2 { get; set; }
        public Single? PO_REM_QTY_R_1_3 { get; set; }
        public Single? PO_REM_QTY_R_1_4 { get; set; }
        public Single? PO_PRV_QTY_R_3_1 { get; set; }
        public Single? PO_PRV_QTY_R_3_2 { get; set; }
        public Single? PO_PRV_QTY_R_3_3 { get; set; }
        public Single? PO_PRV_QTY_R_3_4 { get; set; }

        public Single? PO_PRV_QTY_R_2_1 { get; set; }
        public Single? PO_PRV_QTY_R_2_2 { get; set; }
        public Single? PO_PRV_QTY_R_2_3 { get; set; }
        public Single? PO_PRV_QTY_R_2_4 { get; set; }
        public Single? PO_PRV_QTY_R_1_1 { get; set; }
        public Single? PO_PRV_QTY_R_1_2 { get; set; }
        public Single? PO_PRV_QTY_R_1_3 { get; set; }
        public Single? PO_PRV_QTY_R_1_4 { get; set; }
        public Single? PO_QTY_R_CM_1 { get; set; }
        public Single? PO_QTY_R_CM_2 { get; set; }
        public Single? PO_QTY_R_CM_3 { get; set; }
        public Single? PO_QTY_R_CM_4 { get; set; }
        public Single? PO_QTY_R_1_1 { get; set; }
        public Single? PO_QTY_R_1_2 { get; set; }
        public Single? PO_QTY_R_1_3 { get; set; }
        public Single? PO_QTY_R_1_4 { get; set; }
        public Single? PO_QTY_R_2_1 { get; set; }
        public Single? PO_QTY_R_2_2 { get; set; }
        public Single? PO_QTY_R_2_3 { get; set; }
        public Single? PO_QTY_R_2_4 { get; set; }
        public Single? PO_QTY_R_3_1 { get; set; }
        public Single? PO_QTY_R_3_2 { get; set; }
        public Single? PO_QTY_R_3_3 { get; set; }
        public Single? PO_QTY_R_3_4 { get; set; }
        public Single? PO_QTY_R_4_1 { get; set; }
        public Single? PO_QTY_R_4_2 { get; set; }
        public Single? PO_QTY_R_4_3 { get; set; }
        public Single? PO_QTY_R_4_4 { get; set; }
        public decimal? NYUKA_QTY_BOFORE_1 { get; set; }
        public decimal? NYUKA_QTY_BOFORE_2 { get; set; }
        public decimal? NYUKA_QTY_BOFORE_3 { get; set; }
        public Single? DIM_W { get; set; }
        public string DIM_OTHER { get; set; }
        public decimal? MATL_DIM_W { get; set; }
        public decimal? MATL_DIM_D { get; set; }
        public string MATL_DIM_OTHER { get; set; }
        public string MATL_SHAPE { get; set; }
        public string MATERIAL_TYPE_NAME_CBK { get; set; }

       // Added by Pallavi 
      // Place Order Report
      public int? PO_NO {  get; set; }
        public long? PO_DTL_CD { get; set; }
        public long ? PO_DTL_NO { get; set; }
        public int? PO_DTL_SRLNO { get; set; }
        public Single? MATERIAL_LENGTH { get; set; }
        public string DIM_SHAPE {  get; set; }
        public decimal? ORG_QTY { get; set; }
        public string ORG_DELIVERYDATE { get; set; }
        public string DELIVERYDATE {  get; set; }
        public string TELEPHONE_NO { get; set; }
        public string FAX_NO {  get; set; }
        public string ADDRESS { get; set; }
        public string SUPP_CONT_PERSON { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string ORG_KOUBAN { get; set; }
        public long? ORG_PO_DTL_NO { get; set; }
        public short? PO_KUBUN { get; set; }
        public int? MATERIAL_KUBUN { get; set; }
        public decimal? PO_REM_QTY_TOTAL { get; set; }
        public decimal? PO_PRE_QTY_TOTAL { get; set; }
        public decimal? NYUKA_QTY_TOTAL { get; set; }
        public bool CHECKEDBOX_VALUE { get; set; }

        //Added by Pallavi
        //Matl Not Found Report
        public DateTime IMPORT_DATE { get; set; }
        public string PROD_SPEC_NO { get; set; }
        public int PRODUCT_CD { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string MATL_NAME_MAKER { get; set; }
        public string PRODUCT_NAME_CUST {  get; set; }

        public string PROD_LENGTH { get; set; }

        //Added by Pallavi
        // Forecast Report
        public decimal? PROD_STOCK_QTY { get; set; }
        public decimal? KAKOIRAI_REM_QTY { get; set; }
        public decimal? PROD_STOCK { get; set; }
        public decimal? WIP_STOCK { get; set; }
        public decimal? FC_QTY_R1 { get; set; }
        public decimal? FC_QTY_R2 { get; set; }
        public decimal? FC_QTY_R3 { get; set; }
        public decimal? FC_QTY_R4 { get; set; }
        public string MSG_TO_MATL_DEPT { get; set; }
        public decimal? DISPATCH_QTY1 { get; set; }
        public decimal? DISPATCH_QTY2 { get; set; }
        public decimal? DISPATCH_QTY3 { get; set; }
        public decimal? DISPATCH_QTY4 { get; set; }
        public decimal? DISPACH_QTY_AVERAGE { get; set; }
        public int? MATL_NAME_MAKER_CD { get; set; }
        public decimal? PROD_DIM_D { get; set; }
        public string PROD_SHAPE { get; set; }
        public string PROD_CONDITION_DESCRIPTION { get; set; }


    }
}
