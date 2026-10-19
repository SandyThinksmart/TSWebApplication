using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Domain.Model
{
    public class MakerMaterialDetails
    {
        public int? MATERIAL_RATE_CD { get; set; }
        public int? MATL_GROUP_NO { get; set; }
        public int? MAKER_CD { get; set; }
        public int? MUKESAKI_CD {  get; set; }
        public int? MATERIAL_CD { get; set; }
        public decimal? DIM_START {  get; set; }
        public decimal? DIM_END { get; set; }
        public int? APPLY_ROLL_YEAR { get; set; }
        public short? APPLY_ROLL_MONTH { get; set; }
        public short? APPLY_ROLL_CHANCE { get; set; }
        public int? MATL_NAME_MAKER_CD { get; set; }
        public string MATL_MAKER_NAME { get; set; }
        public short? MATL_KUBUN { get; set; }
        public decimal? RATE { get; set; }
        public string APPLICATION_DATE_STRING { get; set; }
        public string MAKER_NAME { get; set; }
        public string MUKESAKI_NAME { get; set; }
        public string MATERIAL_TYPE_STRING { get; set; }
        public string STEEL_TYPE_NAME { get; set; }
        public string MTL_NAME_INTERNAL { get; set; }
        public int? USER_ID { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public bool? SELECT_EVENT_FLAG { get; set; }
        public List<TSSortField> SORT_FIELDS { get; set; }
    }
}





