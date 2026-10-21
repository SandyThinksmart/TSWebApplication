using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public class NameValue
    {
        public int CD { get; set; }
        public string VALUE { get; set; }
        public string Name { get; set; }
        public string YOBINA { get; set; }
        public short? IN_ACTIVE_FLAG { get; set; }
        public int PROCESS_KUBUN_CD { get; set; }
        public string KUBUN_NAME_JP { get; set; }
        public string KUBUN_NAME_EN { get; set; }
        public string PP_PROCESS_NAME { get; set; }
        public int? NEXT_PROCESS_FLAG { get; set; }
        public string DEFECT_NAME_JP { get; set; }
        public string DEFECT_NAME_EN { get; set; }
    }
}
