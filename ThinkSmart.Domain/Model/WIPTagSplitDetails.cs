using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class WIPTagSplitDetails
    {
        public int? TABA_DIVISION_CD { get; set; }
        public short? PROCESS_KUBUN_CD { get; set; }
        public int? PROCESS_PLAN_CD { get; set; }
        public string PP_CHUUBAN_GYOUBAN { get; set; }
        public short? LOCATION_CD { get; set; }
        public DateTime? DIVISION_DATE   { get; set; }
        public int? DIVISION_OPERATOR_CD { get; set; }
        public short? ORIGIN_TABA_NO { get; set; }
        public short? DIVISION_TABA_NO_1 { get; set; }
        public decimal? DIVISION_NO_OF_UNITS_1 { get; set; }
        public short? DIVISION_UNIT_1 { get; set; }
        public decimal? DIVISION_QTY_1 { get; set; }
        public short? DIVISION_NEXT_PROCESS_1 { get; set; }
        public short? DIVISION_TABA_NO_2 { get; set; }
        public decimal? DIVISION_NO_OF_UNITS_2 { get; set; }
        public short? DIVISION_UNIT_2 { get; set; }
        public decimal? DIVISION_QTY_2 { get; set; }
        public short? DIVISION_NEXT_PROCESS_2 { get; set; }
        public short? DIVISION_TABA_NO_3 { get; set; }
        public decimal? DIVISION_NO_OF_UNITS_3 { get; set; }
        public short? DIVISION_UNIT_3 { get; set; }
        public decimal? DIVISION_QTY_3 { get; set; }
        public short? DIVISION_NEXT_PROCESS_3 { get; set; }
        public short? DIVISION_TABA_NO_4 { get; set; }
        public decimal? DIVISION_NO_OF_UNITS_4 { get; set; }
        public short? DIVISION_UNIT_4 { get; set; }
        public decimal? DIVISION_QTY_4 { get; set; }
        public short? DIVISION_NEXT_PROCESS_4 { get; set; }
        public int? USER_ID { get; set; }
        public short? INPUT_LOCATION_CD { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string REMARK { get; set; }
        public decimal? PROD_DIM_D {  get; set; }
        public string PROD_SHAPE { get; set; }
        public decimal? PROD_LENGTH { get; set; }
        public string CHARGE { get; set; }
        public string PROD_NAME_CUST { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string DIM_TOLERANCE { get; set; }
        public short? CHAMFER { get; set; }
        public string EMPLOYEE_NAME { get; set; }

    }
}
