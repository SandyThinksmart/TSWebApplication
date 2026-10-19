using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class SalesProductionStatusSearchDetails
    {
        #region "PROCESS_PLAN"
        //public int PP_PROCESS_PLAN_CD { get; set; }
        public long PP_PROCESS_PLAN_CD { get; set; }
        public long? PP_CUSTOMER_CD { get; set; }
        public string PP_CUSTOMER_NAME { get; set; }
        public string PP_EMPLOYEE_NAME { get; set; }
        public string PP_CHUUBAN { get; set; }
        public int PP_GYOUBAN { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public DateTime? PP_DELIVERY_DATE { get; set; }
        public string PP_DELIVERY_DATE_STRING { get; set; }
        public string PP_PROD_NAME { get; set; }
        public Single? PP_PROD_DIM_D { get; set; }
        public string PP_PROD_SHAPE { get; set; }
        public Single? PP_PROD_LENGTH { get; set; }
        public string PP_DIM_TOLERANCE { get; set; }
        public string PP_LENGTH_TOL { get; set; }
        public string PP_MAGARI_BEND_TOL { get; set; }
        public int PP_MAGARI_SPAN { get; set; }
        public Single? PP_BEND_TOL { get; set; }
        public int PP_MAGARI_KUBUN { get; set; }
        public string PP_CHARGE { get; set; }
        public DateTime? PP_ACT_ALLOC_DATE { get; set; }
        public string PP_ACT_ALLOC_DATE_STRING { get; set; }
        public decimal? PP_FINISHED_QTY { get; set; }
        public double? PP_ACT_ALLOC_QTY { get; set; }
        public string PROD_CONDITION_DESCRIPTION { get; set; }
        public Single? PP_MATL_DIM_D { get; set; }
        public string PP_MATL_NAME_MAK { get; set; }
        public string PP_PROD_SPEC_NO { get; set; }
        public DateTime? PP_PROCESS_PLAN_CMPL_DATE { get; set; }
        public string PROCESS_PLAN_CMPL_DATE_STRING { get; set; }
        public string PP_MATL_SHAPE { get; set; }
        public string PP_PROD_LENGTH_STRING { get; set; }
        public string SUCCESS_PERCENTAGE { get; set; }
        #endregion
    }
}
