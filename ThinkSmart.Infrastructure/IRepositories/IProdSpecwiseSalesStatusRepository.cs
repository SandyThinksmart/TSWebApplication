using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProdSpecwiseSalesStatusRepository
    {
        ProdSpecificationDetails GetProdSpecDetailsByProductCD(string productCD);
        List<ProductDetails> GetShukkaDetailsByProductCD(string productCD); 
        List<ProductDetails> GetMonthWiseProdStockByProductCD(string productCD); 
        List<ProductDetails> GetPendingStockByProductCD(string productCD);
    }
}
