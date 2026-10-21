using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.Repositories;
using System.Transactions;
using Org.BouncyCastle.Asn1.Ocsp;

namespace TSWebApplication.Application.Services
{
    public class ProcessDisplayConfigService : IProcessDisplayConfigService
    {
        private readonly IProcessDisplayConfigRepository _ProcessDisplayConfigRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;

        public ProcessDisplayConfigService(IProcessDisplayConfigRepository processDisplayConfigRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository)
        {
            _ProcessDisplayConfigRepository = processDisplayConfigRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
        }
        public List<ProcessDisplayConfigDetails> GetProcessDetails()
        {
            return _ProcessDisplayConfigRepository.GetProcessKubunDetails();
        }
        public List<ProcessDisplayConfigDetails> GetActiveNextProcessKubunDetails(long kubunCD)
        {
            return _ProcessDisplayConfigRepository.GetActiveNextProcessKubunDetails(kubunCD);
        }
        public void ProcessKubunDetailsSave(List<ProcessDisplayConfigDetails> requestData)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                _ProcessDisplayConfigRepository.DeleteExistedKubun(Convert.ToInt32(requestData[0].PROCESS_TYPE_CD));
                foreach (var request in requestData)
                {
                    _ProcessDisplayConfigRepository.SaveProcessKubun(request);
                }
                transaction.Complete();
            }
        }
       
    }
}
