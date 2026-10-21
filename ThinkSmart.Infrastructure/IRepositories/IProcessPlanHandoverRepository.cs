using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProcessPlanHandoverRepository
    {
        List<GridSearchFields> GetDetailsByChuuban(List<GridSearchFields> processPlanDetails);
        string SaveProcessPlanHandoverData(List<GridSearchFields> processPlanDetails);
        List<GridSearchFields> DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails);
    }
}
