using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IWarehouseToSupplierService
    {
        KoubanDetails GetKoubanDetails(string kouban);
        List<NameValue> GetUnitTypeList();
        MaterialTensouDetails saveWarehouseToSupplierDetails(MaterialTensouDetails request);

        MaterialTensouDetails updateWarehouseToSupplierDetails(MaterialTensouDetails request);
        List<MaterialTensouDetails> GetDataOnGridView(GridSearchFields gridSearchRequest);
        MaterialTensouDetails GetMaterialTransferDetailsByTrasferCD(long WIP_TRANSFER_CD);

        // List<MaterialTensouDetails> GetTensouDataSearchList(GridSearchFields request);




    }
}
