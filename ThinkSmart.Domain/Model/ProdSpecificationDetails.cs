using System;
using System.Collections.Generic;
using System.Text;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Domain.Model
{
    public class ProdSpecificationDetails
    {
       public string PRODUCT_HOST_CD {  get; set; }
        public string PROD_SPEC_NO1 { get; set; }
        public string PROD_SPEC_NO2 { get; set; }
        public string PROD_SPEC_STRING { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string INTERNAL_NAME { get; set; }
        public string SHAPE { get; set; }
        public decimal? DIM_D { get; set; }
        public string LENGTH { get; set; }
        public string CONDITION { get; set; }
        public string CUSTOMER_NAME_STRING { get; set; }
        public short? PRODUCT_KUBUN {  get; set; }
        public string PRODUCT_KUBUN_STRING { get; set; }
        public string DIM_TOL_STRING { get; set; }
        public string LEN_TOL_STRING { get; set; }
        public short? MAGARI_KUBUN { get; set; }
        public int? MAGARI_SPAN { get;set; }
        public string MAGARI_BEND_TOL { get;set; }
        public string BEND_TOLERANCE_STRING { get; set; }   
        public decimal? PENDSTOCK_ABOVE30 { get; set; }
        public decimal? PENDSTOCK_ABOVE60 { get; set; }
        public decimal? PENDSTOCK_ABOVE90 { get; set; }
        public decimal? KAKOIRAI_REM_QTY { get; set; }
        public decimal? PROD_STOCK_QTY { get; set; }
        public decimal? BEFORE_ALLOC_PLANNED_QTY { get; set; }
        public decimal? IN_PROGRESS_QTY { get; set; }
        public decimal? TOTAL_KAKO_PROD { get; set; }
        public List<ProductDetails> SHUKKA_DETAILS { get; set; }  





    }
}
