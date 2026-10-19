using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IProdSpecwiseSalesStatusService
    {
        ProdSpecificationDetails GetProdSpecDetailsByProductCD(string productCD); 
        List<ProductDetails> GetMonthWiseProdStockByProductCD(string productCD); 
        List<ProductDetails> GetPendingStockByProductCD(string productCD);
    }
}
