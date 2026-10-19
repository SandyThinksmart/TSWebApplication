using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class StoredProcedureDemoServices : IStoredProcedureDemoServices
    {
        private readonly IStoredProcedureDemoRepository _storedProcedureDemoRepository;
        
        private readonly LocalizationServices _localization;
        public StoredProcedureDemoServices(IStoredProcedureDemoRepository storedProcedureDemoRepository, LocalizationServices localization)
        {
            _storedProcedureDemoRepository = storedProcedureDemoRepository;
            
            _localization = localization;
        }
        public DataTable TrialDemo1()
        {
            return _storedProcedureDemoRepository.Trial1();
        }

        public DataTable TrialDemo2()
        {
            return _storedProcedureDemoRepository.Trial2();
        }
        public DataTable TrialDemo3()
        {
            return _storedProcedureDemoRepository.Trial3();
        }
    }
}
