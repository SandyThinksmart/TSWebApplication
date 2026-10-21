using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class ProdSpecwiseSalesStatusService : IProdSpecwiseSalesStatusService
    {
        private readonly IProdSpecwiseSalesStatusRepository _prodSpecwiseSalesStatus;
        
        public ProdSpecwiseSalesStatusService(IProdSpecwiseSalesStatusRepository prodSpecwiseSalesStatus)
        {
            _prodSpecwiseSalesStatus = prodSpecwiseSalesStatus;
            
        }
        public ProdSpecificationDetails GetProdSpecDetailsByProductCD(string productCD)
        {
            ProdSpecificationDetails result = new ProdSpecificationDetails();
            result = _prodSpecwiseSalesStatus.GetProdSpecDetailsByProductCD(productCD);
            if (result != null)
            {

                var shukkaDetails = _prodSpecwiseSalesStatus.GetShukkaDetailsByProductCD(productCD);
                if (shukkaDetails.Count > 0)
                {
                    result.SHUKKA_DETAILS = shukkaDetails;
                }
            }
            return result;
        }
        

        public List<ProductDetails> GetMonthWiseProdStockByProductCD(string productCD)
        {
           var result = _prodSpecwiseSalesStatus.GetMonthWiseProdStockByProductCD(productCD);
           return result;
       
        }

        public List<ProductDetails> GetPendingStockByProductCD(string productCD)
        {
            var result = _prodSpecwiseSalesStatus.GetPendingStockByProductCD(productCD);
            return result;
            
        }
    }
}
