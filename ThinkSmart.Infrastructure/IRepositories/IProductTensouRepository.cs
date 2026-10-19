using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProductTensouRepository
    {
        List<GridSearchFields> GetProductMasterDataList(List<SearchField> searchFields, string sortCriteria);
        List<GridSearchFields> GetProductDetailsByProductCD(string productCD);
    }
}
