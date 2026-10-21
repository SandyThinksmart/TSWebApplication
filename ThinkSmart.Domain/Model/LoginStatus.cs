using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class LoginStatus
    {
        public int? LOGIN_STATUS_CODE { get; set; }
        public string WORKSTATION_NAME { get; set; }
        public DateTime LAST_USED_TIME { get; set; }
        public string BROWSER_ID { get; set; }
        public string COOKIES_UNIQUE_ID { get; set; }

    }
}
