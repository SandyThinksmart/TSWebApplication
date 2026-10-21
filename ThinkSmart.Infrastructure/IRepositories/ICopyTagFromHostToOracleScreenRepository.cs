using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface  ICopyTagFromHostToOracleScreenRepository
    {
        List<GridSearchFields> GetDetailsByNifuda(List<GridSearchFields> lstNifudaNo);
        string SaveNifudaData(List<GridSearchFields> tagDetails);
        List<GridSearchFields> DeleteCopyTagData(List<GridSearchFields> tagDetails);
    }
}
