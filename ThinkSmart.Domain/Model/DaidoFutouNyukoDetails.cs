using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class DaidoFutouNyukoDetails
    {
        public DateTime? PROCESS_DATE_AND_TIME { get; set; }
        public int? PROCESS_SERIAL_NUMBER { get; set; }
        public int? EXECUTE_RESULT {  get; set; }
        public string ERROR { get; set; }
        public int? FUTOU_CD { get; set; }
        public string TYPE_OF_DATA {  get; set; }
        public DateTime? FUTO_NYUKO_DATE { get; set; }
        public string CONTRACT_NUMBER {  get; set; }
        public string CONTRACT_GYOUBAN { get; set; }
        public string FUTO_NYUKO_LOCATION { get; set; }
        public string RECEIVER { get; set; }
        public string MUKESAKI { get; set; }
        public string MAKER_CODE {  get; set; }
        public string CONTRACTOR {  get; set; }
        public string PROCESSING_PERSON { get; set; }
        public string ORDER_KOUBAN { get; set; }
        public string STEEL_GRADE {  get; set; }
        public string DIM_IMAGE{ get; set; }
        public string DELIVERY_CONDITION { get; set; }
        public string WINDING_DIRECTION { get; set; }
        public string PROD_SPEC_KOUBAN { get; set; }
        public string CHARGE { get; set; }
        public string THABA_NO { get; set; }
        public string NO_OF_UNITS { get; set; }
        public string QUANTITY_IN_KG { get; set; }
        public string YARD_KBN { get; set; }
        public string NYUKA_KBN { get; set; }
        public string SHUKKA_KBN { get; set; }
        public string USER_MANAGEMENT_KEY { get; set; }
        public string DIMD_DECOMPOSITION_SIZE { get; set; }
        public string SHAPE_INFO { get; set; }
        public string ORDER_PERSON_CODE { get; set; }
        public string RESERVE { get; set; }
        public string DESTINATION_CODE { get; set; }
        public string FACTORY_SHUKKA_NUMBER { get; set; }
        public string DELIVERY_NOTE_NUMBER { get; set; }
        public int? NYUKO_NO { get; set; }
        public int? USER_ID { get; set; }
        public string CREATED_DATE {  get; set; }
        public string CREATED_TIME { get; set; }       
        public int TRANSFER_COUNT { get; set; }
        public int TOTAL_COUNT { get; set; }
        public int ERROR_COUNT { get; set; }
    }
}
