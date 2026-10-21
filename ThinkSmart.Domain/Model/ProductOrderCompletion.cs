using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class ProductOrderCompletion
    {
        public int? PROCESS_PLAN_CD { get; set; }
        public string CHUUBAN { get; set; }
        public string CUSTOMER { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public decimal? BEND_TOLERANCE { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string SHAPE { get; set; }
        public string CHARGE { get; set; }
        public string PROD_SPEC_NO1 { get; set; }
        public string PROD_SPEC_NO2 { get; set; }
        public string PROD_SPEC_NO { get; set; }

        public DateTime? DELIVERY_DATE { get; set; }
        public DateTime? ACT_ALLOC_DATE { get; set; }
        public DateTime? PROCESS_PLAN_CMPL_DATE { get; set; }


        public Single? DIM_D { get; set; }
        public Single? PRODUCT_LENGTH { get; set; }
        public string PRODUCT_LENGTH_STRING { get; set; }
        public string DIM_TOL_MAX { get; set; }
        public string DIM_TOL_MIN { get; set; }
        public string LENGTH_TOLERANCE_MAX { get; set; }
        public string LENGTH_TOLERANCE_MIN { get; set; }
        public Single? MATL_DIM_D { get; set; }
        public Single? PP_PROD_DIM_D { get; set; }        
        public string MATL_DIM_STRING { get; set; }
        public string PROD_DIM_STRING { get; set; }
        public double? ACT_ALLOC_QTY { get; set; }
        public decimal FINISHED_QTY { get; set; }
        public decimal COMPLETEPER { get; set; }

        public short? GYOBAN { get; set; }
        public string MAGARI_KUBUN_SPAN_TOL { get; set; }
        public Int16? MAGARI_SPAN { get; set; }
        public int? PROCESS_PLAN_CMPL_FLAG { get; set; }

        public string DIM_TOLERANCE { get; set; }
        public string LEN_TOL { get; set; }
        public Single BEND_TOL { get; set; }
        public string MAGARI_BEND_TOL { get; set; }


        public string UNIT { get; set; }

        public string NO_OF_UNITS { get; set; }

        public string PRODUCT_CONDITION { get; set; }

        public Int16 ACT_DM_F { get; set; }
        public string MAGARI_KUBUN { get; set; }
        public string DMF_DMT { get; set; }
        public short? DECARB_1 { get; set; }
        public short? DECARB_2 { get; set; }
        public short? DECARB_3 { get; set; }
        public decimal DECARB_VAL_1 { get; set; }
        public Single? DECARB_VAL_2 { get; set; }
        public decimal DECARB_VAL_3 { get; set; }
        public Int64 INTERNAL_QUALITY_GROUP_NO { get; set; }
    }
}
