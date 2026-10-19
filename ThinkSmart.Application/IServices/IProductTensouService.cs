using System;
using System.Collections.Generic;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IProductTensouService
    {
        public List<NameValue> GetVehicleName();
        public List<NameValue> GetForwordingLocation();
        public List<NameValue> GetTensouLocation(int USER_ID);
        List<GridSearchFields> GetProductMasterDataList(GridSearchFields gridSearchRequest); 
        List<GridSearchFields> GetProductDetailsByProductCD(string productCD);
    }
}
