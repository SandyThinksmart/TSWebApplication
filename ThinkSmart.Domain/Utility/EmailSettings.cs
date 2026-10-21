using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public class EmailSettings
    {
        public bool EnableEmailNotifications { get; set; }
        public string PrimaryDomain { get; set; }

        public int PrimaryPort { get; set; }

        public bool enableSsl { get; set; }
        public bool defaultCredentials { get; set; }
        public string UsernameEmail { get; set; }
        public string UsernamePassword { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
    }
}
