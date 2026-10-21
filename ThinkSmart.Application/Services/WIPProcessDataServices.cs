using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.Services
{
    public class WIPProcessDataServices : IWIPProcessDataServices
    {
        private readonly IWIPProcessDataRepository _wipprocessDataServicesRepository;
        public WIPProcessDataServices(IWIPProcessDataRepository WIPProcessData)
        {
            _wipprocessDataServicesRepository = WIPProcessData;
        }
        public int GetWIProcessDataList(int locationCd)
        {

            return _wipprocessDataServicesRepository.GetWIPProcessDataList(locationCd);

        }
    }
}
