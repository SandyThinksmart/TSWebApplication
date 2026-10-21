using System;
using System.Collections.Generic;
using System.Text;
using static TSWebApplication.Domain.Utility.Constants;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Domain.Model
{
    public class MstConfiguration
    {
     public short CONFIGURATION_CD { get; set; }
     public string KEY { get; set; }
     public string VALUE { get; set; }
     public short KEY_TYPE { get; set; }
     public string DESCRIPTION { get; set; }
     public short IN_ACTIVE_FLAG { get; set; }
     public int USER_ID  { get; set; }
     public DateTime CREATED_DATE { get; set; }
    public DateTime UPDATED_DATE { get; set; }
    }
}
