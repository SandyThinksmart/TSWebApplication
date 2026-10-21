using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProcessPlanHandoverHostRepository
    {
        List<GridSearchFields> ValidateChuuban(List<GridSearchFields> processPlanDetails);
        List<GridSearchFields> GetProcessPlanDetails(List<GridSearchFields> processPlanDetails);
        string SaveProcessPlanHandoverData(List<GridSearchFields> processPlanDetails);
        List<GridSearchFields> DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails);
        List<GridSearchFields> ValidateDataExist(List<GridSearchFields> processPlanDetails);
        List<GridSearchFields> ValidateDataForUpdate(List<GridSearchFields> processPlanDetails);
        string UpdateProcessPlanHandoverData(List<GridSearchFields> processPlanDetails);
        List<GridSearchFields> ValidateDataForAddNewBtn(List<GridSearchFields> processPlanDetails); 
    }
}
