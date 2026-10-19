using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class ProcessTypeRemarkUpdationDetails
    {
        public int KUBUN_ID { get; set; }
        public string KUBUN_NAME { get; set; }
        public string KUBUN_NAME_EN { get; set; }
        public string PP_PROCESS_NAME { get; set; }
        public string REMARK_TEMPLATE { get; set; }
        public int? USER_ID { get; set; }
    }
}
