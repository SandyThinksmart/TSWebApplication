using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class RollForcastPopup
    {
        #region "Product ForeCast"
        public string PROD_SPEC_NO { get; set; } 
            public string PRODUCT_CD { get; set; } 
            public short? ROLL_FC_KUBUN { get; set; } 
            public string INTERNAL_NAME { get; set; } 
            public string CUSTOMER_NAME { get; set; } 
            public string PRODUCT_NAME_CUST { get; set; } 
            public decimal? DIM_D { get; set; } 
            public decimal? DIM_W { get; set; } 
            public string DIM_OTHER { get; set; } 
            public string SHAPE { get; set; } 
            public decimal? PRODUCT_LENGTH { get; set; } 
            public string LENGTH_OTHER { get; set; } 
            public string DIM_TOL { get; set; } 
            public string LEN_TOL { get; set; } 
            public string MATL_NAME_MAKER { get; set; } 
            public string MAKER_CD { get; set; } 
            public string MAKER_NAME { get; set; }
            public int? ROLL_FC_CD { get; set; } 
            public string Roll_FC_NO { get; set; } 
            public int ROLL_FC_SRLNO { get; set; }
            public decimal? ROLL_FC_R { get; set; } 
            public int ROLL_FC_YEAR { get; set; } 
            public DateTime ROLL_FC_DATE { get; set; } 
            public string EMPLOYEE_CD { get; set; } 
            public string CUSTOMER_CD { get; set; } 
            public decimal? FC_QTY_R1 { get; set; } 
            public decimal? FC_QTY_R2 { get; set; } 
            public decimal? FC_QTY_R3 { get; set; } 
            public decimal? FC_QTY_R4 { get; set; } 
            public string INCHARGE { get; set; } 
            public string CUSTOMER { get; set; } 
            public decimal? WIP_STOCK { get; set; } 
            public decimal? PROD_STOCK { get; set; }
            public decimal? DISPATCH_QTY1 { get; set; }
            public decimal? DISPATCH_QTY2 { get; set; }
            public decimal? DISPATCH_QTY3 { get; set; }
            public decimal? DISPATCH_QTY4 { get; set; }
            public decimal? AVG_DISPATCH_QTY { get; set; }
            public string PROD_KUBUN { get; set; }
            public decimal? PROD_STOCK_QTY { get; set; }
            public decimal? KAKOIRAI_REM_QTY { get; set; }
            public short? MAGARI_KUBUN { get; set; }
            public int? MAGARI_SPAN { get; set; }
            public string MAGARI_BEND_TOL { get; set; }
            public string BEND_TOLERANCE_STRING { get; set; }
            public decimal? MAT_QTY_1MONTH { get;set; }
            public decimal? MAT_QTY_2MONTH { get; set; }
            public decimal? MAT_QTY_3MONTH { get; set; }
            public decimal? MAT_QTY_4MONTH { get; set; } 
            public DateTime? SALES_FC_DONE_DATE { get; set; }

        #endregion

        #region  "Material ForeCast"
        public string MATL_MAKER_NAME { get; set; }
            public string MATL_KUBUN { get; set; }
            public decimal MATERIAL_LENGTH { get; set; }
            public int MATL_STOCK_QTY { get; set; }
            public string MATERIAL_CD { get; set; }
            public string MAKER_DETAILS { get; set; } 
            public string MATL_SPECIAL_KUBUN { get; set; }
            public string CONDITION_CD { get; set; }
            public string CONDITION_DESCRIPTION { get; set; }
           public string HT_CONDITION { get; set; }
          public string MOCHIKOMISAKI_CD { get; set; }
            public string MOCHIKOMISAKI_SHORT_NAME { get; set; }
            public int ORDER_REM_QTY { get; set; }
            public string MSG_TO_MATL_DEPT { get; set; }
            public string FC_SUMMARY_NO { get; set; }
            public string ROLL_FC_SRL_NO { get; set; }
            public string INTERNAL_NAME_CD { get; set; }
            public string INCHARGE_SHORT_NAME { get; set; }
            public string CUSTOMER_SHORT_NAME { get; set; }
            public string REMARKS { get; set; }
            public int IS_LAST_R_REC { get; set; }
            public decimal? MATL_DIM_D { get; set; }
            public decimal? MATL_DIM_W { get; set; }
            public string MATL_SHAPE { get; set; }
            public string MATL_DIM_OTHER { get; set; }
            public decimal? MATL_LENGTH { get; set; }
            public string MATL_CONDITION_DESCRIPTION { get; set; }
            public decimal? LAST1_MNTH_DISPACH_QTY {get; set;}
            public decimal? LAST2_MNTH_DISPACH_QTY {get; set;}
            public decimal? LAST3_MNTH_DISPACH_QTY { get; set; }
            public decimal? DISPATCH_AVG_QTY { get; set; }
            public decimal? LAST_MONTH_DISPATCH_AVG_QTY{get;set; }

            public string MATL_KUBUN_NAME { get; set; }
            public string MATL_SPECIAL_KUBUN_NAME { get; set; }

            public short? REQD_MATL_QTY_1MONTH { get; set; }
            public short? REQD_MATL_QTY_2MONTH { get; set; }
            public short? REQD_MATL_QTY_3MONTH { get; set; }
            public short? REQD_MATL_QTY_4MONTH { get; set; }
            public string REPLY_FROM_MATL_DEPT { get; set; }

        #endregion


    }


}
