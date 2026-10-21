using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IPoAndReceiptHistoryPopUpServices
    {
        List<PoAndReceiptPopupDetails> GetMakerWisePurchaseOrderQtyList(int Batch_Roll);
        List<PoAndReceiptPopupDetails> GetVendorWisePurchaseOrderQtyList(int Batch_Roll);
    }
}
