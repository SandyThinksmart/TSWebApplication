using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IProcessDisplayConfigService
    {
        List<ProcessDisplayConfigDetails> GetProcessDetails();
        void ProcessKubunDetailsSave(List<ProcessDisplayConfigDetails> request);
        List<ProcessDisplayConfigDetails> GetActiveNextProcessKubunDetails(long kubunCD);
    }
}
