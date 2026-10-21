using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class MaterialTensouDetails
    {
        public int TRANSFER_CD { get; set; }
        public int? KOUBAN_CD { get; set; }
        public int FUTOU_CD { get; set; }
        public int MOCHIKOMISAKI_CD { get; set; }
        public DateTime? NYUKA_DATE { get; set; }
        public DateTime TRANSFER_DATE { get; set; }
        public string CHARGE { get; set; }
        public Single? NO_OF_UNITS { get; set; }
        public Int16? UNIT { get; set; }
        public double QUANTITY { get; set; }
        public string CHUUBAN { get; set; }
        public string SLIP_NUMBER { get; set; }
        public string REMARKS { get; set; }
        public int USER_ID { get; set; }
        public int INPUT_LOCATION_CD { get; set; }
        public int LOCATION_CD { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public string MAKER_NAME { get; set; }
        public string INTERNAL_NAME { get; set; }
        public Single? DIM_D { get; set; }
        public Single? DIM_W { get; set; }
        public string DIM_OTHER { get; set; }
        public string SHAPE { get; set; }
        public Single? MATERIAL_LENGTH { get; set; }
        public string CONDITION_DESCRIPTION { get; set; }
        public Int16? MATL_KUBUN { get; set; }
        public string MATL_KUBUN_NAME { get; set; }
        public string KOUBAN { get; set; }
        public string NO_OF_UNIT_STRING { get; set; }
        public string FUTOU_NAME { get; set; }
        public string MKSAKI_NAME { get; set; }
        public int? MATL_NAME_MAKER_CD { get; set; }
        public string MUKESAKI_NAME { get; set; }
        public int? MATL_SPECIAL_KUBUN { get; set; }
        
       
    }
}
