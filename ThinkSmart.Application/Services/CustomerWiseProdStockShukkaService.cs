using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;
using TSWebApplication.Infrastructure.IRepositories;
using System.Transactions;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class CustomerWiseProdStockShukkaService:ICustomerWiseProdStockShukkaService
    {
        private readonly ICustomerWiseProdStockShukkaRepository _CustomerWiseProdStockShukkaRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public CustomerWiseProdStockShukkaService(ICustomerWiseProdStockShukkaRepository CustomerWiseProdStockShukkaRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _CustomerWiseProdStockShukkaRepository = CustomerWiseProdStockShukkaRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }

        public List<CustomerWiseProdStockShukka> GetCustomerDetails(string customerCD)
        {
            var obj = _CustomerWiseProdStockShukkaRepository.GetCustomerDetails(customerCD);
            return obj;
        }
        public List<ProductDetails> GetDispatchQtyCustomerWise(string customerCD)
        {
            var obj = _CustomerWiseProdStockShukkaRepository.GetDispatchQtyCustomerWise(customerCD);
            return obj;
        }

        public List<ProductDetails> GetProdStockQtyCustomerWise(string customerCD)
        {
            var obj = _CustomerWiseProdStockShukkaRepository.GetProdStockQtyCustomerWise(customerCD);
            return obj;
        }
        
        public List<ProductDetails> GetPendingStockQtyCustomerWise(string customerCD)
        {
            var obj = _CustomerWiseProdStockShukkaRepository.GetPendingStockQtyCustomerWise(customerCD);
            return obj;
        }


    }
}
