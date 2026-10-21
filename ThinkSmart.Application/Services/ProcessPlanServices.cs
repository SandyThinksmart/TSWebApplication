using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.Services
{
    public class ProcessPlanServices : IProcessPlanDetails
    {
        private readonly IProcessPlanReportPrintReprintRepository _processplanreportprintreprint;
        public ProcessPlanServices(IProcessPlanReportPrintReprintRepository ProcessPlanReportPrintReprint)
        {
            _processplanreportprintreprint = ProcessPlanReportPrintReprint;
        }
        public int GetProcessPlanReportCoilToCoilList(ProcessPlanReportSearchRequest request)
        {

            return _processplanreportprintreprint.GetProcessPlanReportCoilToCoilList(request);

        }

        public int GetProcessPlanReportBarToBarList(ProcessPlanReportSearchRequest request)
        {

            return _processplanreportprintreprint.GetProcessPlanReportBarToBarList(request);

        }
        public int GetProcessPlanReportCoilToBarList(ProcessPlanReportSearchRequest request)
        {

            return _processplanreportprintreprint.GetProcessPlanReportCoilToBarList(request);

        }

        
    }
}
