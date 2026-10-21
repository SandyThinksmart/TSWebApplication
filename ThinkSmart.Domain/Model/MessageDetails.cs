using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class MessageDetails
    {
        public long MSG_CD { get; set; }
        public long PROCESS_PLAN_CD { get; set; }
        public int? MACHINE_CD { get; set; }
        public int PROCESS_KUBUN_ID { get; set; }
        public long? OPERATOR_CD { get; set; }
        public DateTime MSG_DATE { get; set; }
        public string MSG_DATE_STRING { get; set; }
        public string MSG_CONTENTS { get; set; }
        public int USER_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public string MACHINE_NAME { get; set; }
        public string OPERATOR_NAME { get; set; }
        public string PROCESS_NAME_JP { get; set; }
        public string PROCESS_NAME_EN { get; set; }
        public int? INPUT_LOCATION_CD { get; set; }
        public int? LOCATION_CD { get; set; }

    }
}
