using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProcessDisplayConfigRepository
    {
        List<ProcessDisplayConfigDetails> GetProcessKubunDetails();
        void SaveProcessKubun(ProcessDisplayConfigDetails request);
       void DeleteExistedKubun(int process);
        List<ProcessDisplayConfigDetails> GetActiveNextProcessKubunDetails(long KubunCD);
        
    }
}
