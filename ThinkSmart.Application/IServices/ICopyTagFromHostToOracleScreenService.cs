using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface ICopyTagFromHostToOracleScreenService
    {
        List<GridSearchFields> GetDetailsByNifuda(List<GridSearchFields> lstNifudaNo);
        string SaveNifudaData(List<GridSearchFields> processPlanDetails);

        List<GridSearchFields> DeleteCopyTagFromHostToOracleData(List<GridSearchFields> processPlanDetails); 
    }
}
