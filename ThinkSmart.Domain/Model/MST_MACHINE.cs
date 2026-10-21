using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class MST_MACHINE
    {
        public int MACHINE_CD { get; set; }
        public int MACHINE_GROUP_CD { get; set; }
        public int LOCATION_CD { get; set; }
        public string MACHINE_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public int DISPLAY_PRIORITY { get; set; }
        public int IN_ACTIVE_FLAG { get; set; }
        public int USER_ID { get; set; }

    }
}
