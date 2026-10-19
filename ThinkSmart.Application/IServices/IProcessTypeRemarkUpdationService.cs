using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IProcessTypeRemarkUpdationService
    {
        List<ProcessTypeRemarkUpdationDetails> GetProcessDetails();
        void ProcessKubunRemarkUpdate(ProcessTypeRemarkUpdationDetails request);
    }
}
