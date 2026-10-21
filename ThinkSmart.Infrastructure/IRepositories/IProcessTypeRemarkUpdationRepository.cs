using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProcessTypeRemarkUpdationRepository
    {
        List<ProcessTypeRemarkUpdationDetails> GetProcessKubunDetails();
        void ProcessKubunUpdateDetails(ProcessTypeRemarkUpdationDetails request);
    }
}
