using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class ProcessDisplayConfigDetails
    {
        public int KUBUN_ID { get; set; }
        public string KUBUN_NAME { get; set; }
        public string KUBUN_NAME_EN { get; set; }
        public string PP_PROCESS_NAME { get; set; }
        public int? USER_ID { get; set; }
        public int? PROCESS_TYPE_CD { get; set; }
        public int? NEXT_PROCESS_TYPE_CD { get; set; }
        public int? SHOW_PRIORITY { get; set; }
        public int? SHOW_ONOFF { get; set;}
    }
}
