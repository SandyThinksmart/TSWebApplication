using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IProductTensouRepository
    {
        List<GridSearchFields> GetProductMasterDataList(List<SearchField> searchFields, string sortCriteria);
        List<GridSearchFields> GetProductDetailsByProductCD(string productCD);
        List<WIPTensouDetails> GetProductTensouList(List<SearchField> searchFields, string sortCiteria, long locationCD);
        int GetMaxGroupNo();
        int SaveProductTensouDetails(WIPTensouDetails request);
        List<WIPTensouDetails> GetTransferCD(long transferGroupCD);
        Dictionary<string, List<string>> GetValidShukkaCD(string productCD);

        List<ProductTensouDetails> GetShukkaProductDetails(string shukkaCD);
    }
}
