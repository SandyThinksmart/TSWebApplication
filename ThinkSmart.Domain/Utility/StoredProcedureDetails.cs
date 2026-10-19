using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public class StoredProcedureDetails
    {
        public string PROCEDURE_NAME { get; set; }
        public List<InputParameters> PARAMETERS { get; set; }
    }
    public class InputParameters
    {
        public string PARAMETER_NAME { get; set; }
        public string PARAMETER_VALUE { get; set; }
        public string DATA_TYPE { get; set; }
    }
}
