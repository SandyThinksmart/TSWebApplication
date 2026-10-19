using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class MakerMillSheetConfiguration
    {
        public int MAKER_CONFIG_CD {  get; set; }
        public int MAKER_CD { get; set; }
        public string MAKER_NAME { get; set; }
        public string UNIQUE_TEMPLATE_NAME { get; set; }
        public int? FIELD_CD { get; set; }
        public decimal TOP_LEFT_X { get; set; }
        public decimal TOP_LEFT_Y { get; set; }
        public decimal TOP_RIGHT_X { get; set; }
        public decimal TOP_RIGHT_Y { get; set; }
        public decimal BOTTOM_LEFT_X { get; set; }
        public decimal BOTTOM_LEFT_Y { get; set; }
        public decimal BOTTOM_RIGHT_X { get; set; }
        public decimal BOTTOM_RIGHT_Y { get; set; }
        public decimal WIDTH { get; set; }
        public decimal HEIGHT { get; set; }
        public string ACTION { get; set; }
        public int USER_ID { get; set; }
    }
}
