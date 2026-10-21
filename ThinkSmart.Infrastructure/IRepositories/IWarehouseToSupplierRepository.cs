using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IWarehouseToSupplierRepository
    {
        KoubanDetails GetKoubanDetailsByKouban(string koubaName);
        MaterialTensouDetails saveTensouDetails(MaterialTensouDetails request);
        MaterialTensouDetails updateTensouDetails(MaterialTensouDetails request);
        List<MaterialTensouDetails> GetMatlTensouList(string sortCriteria, List<SearchField> searchCriteria);
        MaterialTensouDetails GetMaterialTransferDetailsByTrasferCD(long TRANSFER_CD);

      //  List<MaterialTensouDetails> GetMaterialTensouList(List<SearchField> searchFields, string sortCiteria);
       

    }
}
