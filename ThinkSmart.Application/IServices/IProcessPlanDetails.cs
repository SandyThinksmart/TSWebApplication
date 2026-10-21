using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IProcessPlanDetails
    {
        int GetProcessPlanReportCoilToCoilList(ProcessPlanReportSearchRequest request);
        int GetProcessPlanReportBarToBarList(ProcessPlanReportSearchRequest request);

        int GetProcessPlanReportCoilToBarList(ProcessPlanReportSearchRequest request);

    }
}
