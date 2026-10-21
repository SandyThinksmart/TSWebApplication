using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class CustomerWiseProdStockShukka
    {
        public string CUST_NAME { get; set; }
        public int CUST_ID { get; set; }
        public decimal? ORD_REM_STOCK { get; set; }
        public int PROD_STOCK { get; set; }
        public short BEFORE_ACT_ALLOC { get; set; }
        public float WIP_ACT_ALLOC { get; set; }
        public decimal? TOTAL { get; set; }
        public decimal? TOT_PROD_STOCK { get; set; }
        public decimal? TOT_PEND_STOCK_ABOVE30 { get; set; }
        public decimal? TOT_PEND_STOCK_ABOVE60 { get; set; }
        public decimal? TOT_PEND_STOCK_ABOVE90 { get; set; }
        public decimal? DISPATCH_QTY { get; set; }
        

    }
}
