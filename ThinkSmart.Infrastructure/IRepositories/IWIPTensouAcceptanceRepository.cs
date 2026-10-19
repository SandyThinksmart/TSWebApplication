using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IWIPTensouAcceptanceRepository
    {
        List<WIPTensouDetails> GetWIPTransferAcceptanceDetailsByTrasferCD(string tensouCD);
        Dictionary<string, List<string>> GetValidTensouNo(string WIPTensouCD);
        Dictionary<string, List<string>> GetCompleteChuuban(string completeTransferNo);
        Dictionary<string, List<string>> TransferNoAccepted(string transferNoAccepted);
        int GetMaxGroupNo();
        void WIPTensouAcceptanceDetailsDelete(string WIPTransferCD);
        void SaveWIPTensouAcceptanceDetails(WIPTensouDetails request);
       
        int GetMsgCdByMsgContent(string MSG_CONTENTS); 
        int GetMsgCdFromTensouRequest(int MSG_CD);
    }
}
