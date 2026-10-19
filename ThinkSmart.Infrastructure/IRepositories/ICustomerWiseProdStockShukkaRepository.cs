using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface ICustomerWiseProdStockShukkaRepository
    {
        List<CustomerWiseProdStockShukka> GetCustomerDetails(string customerCD); 
        List<ProductDetails> GetDispatchQtyCustomerWise(string customerCD);
        List<ProductDetails> GetProdStockQtyCustomerWise(string customerCD); 
        List<ProductDetails> GetPendingStockQtyCustomerWise(string customerCD);
    }
}
