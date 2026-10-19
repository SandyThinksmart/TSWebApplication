using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.Repositories;
using System.Transactions;

namespace TSWebApplication.Application.Services
{
    public class ProcessTypeRemarkUpdationService : IProcessTypeRemarkUpdationService
    {
        private readonly IProcessTypeRemarkUpdationRepository _ProcessTypeRemarkUpdationRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;

        public ProcessTypeRemarkUpdationService(IProcessTypeRemarkUpdationRepository processTypeRemarkUpdationRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository)
        {
            _ProcessTypeRemarkUpdationRepository = processTypeRemarkUpdationRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
        }
        public List<ProcessTypeRemarkUpdationDetails> GetProcessDetails()
        {
            return _ProcessTypeRemarkUpdationRepository.GetProcessKubunDetails();
        }
        public void ProcessKubunRemarkUpdate(ProcessTypeRemarkUpdationDetails request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                _ProcessTypeRemarkUpdationRepository.ProcessKubunUpdateDetails(request);
                transaction.Complete();
            }
        }
    }
}
