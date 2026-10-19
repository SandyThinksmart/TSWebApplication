using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class BCH_PROD_STOCK
    {
        public string SLIP_NUMBER {get; set;}
        public decimal? NYUKA_DATE {get; set;}
        public string PRODUCT_NAME {get; set;}
        public string COMPANY_STANDARD {get; set;}
        public string STOCK_CD {get; set;}
        public decimal? DIM_D {get; set;}
        public string PROD_SHAPE {get; set;}
        public string ALLOC_CD {get; set;}
        public string CURRENT_CD {get; set;}
        public string DIM_TOL {get; set;}
        public string DIM_OTHER {get; set;}
        public string PRODUCT_LENGTH {get; set;}
        public string CONDITION {get; set;}
        public decimal? PROD_STOCK_QTY { get; set;}
        public string NYUKA_DATE_YEAR {get; set;}
        public string PRODUCT_CD { get; set; }
        public int? FLAG_CODE { get; set; }
        public int? DUPLICATE_RECORDS { get; set; }
        public string PROD_SPEC_1 { get; set; }
        public string PROD_SPEC_2 { get; set;}
        public int? SRNO { get; set;}


    }
}
