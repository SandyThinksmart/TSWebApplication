using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProcessPlanReportPrintReprintRepository
    {
        int GetProcessPlanReportCoilToCoilList(ProcessPlanReportSearchRequest request);
        int GetProcessPlanReportBarToBarList(ProcessPlanReportSearchRequest request);

        int GetProcessPlanReportCoilToBarList(ProcessPlanReportSearchRequest request);
    }
}
