using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IPoAndReceiptHistoryPopUpRepository
    {
        List<PoAndReceiptPopupDetails> GetMakerWisePurchaseOrderQtyList(int Batch_Roll);
        List<PoAndReceiptPopupDetails> GetVendorWisePurchaseOrderQtyList(int Batch_Roll);
    }
}
