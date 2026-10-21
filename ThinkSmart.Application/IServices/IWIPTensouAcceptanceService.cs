using System.Collections.Generic;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IWIPTensouAcceptanceService
    {
        List<NameValue> GetNextProcessList();
        List<NameValue> GetAcceptanceLocation(int USER_ID);

        List<WIPTensouDetails> GetWIPTransferAcceptanceDetailsByTrasferCD(string tensouCD,string CultureName);

        Dictionary<string, List<string>> GetValidTensouNo(string trasferCD);

        Dictionary<string, List<string>> GetCompleteChuuban(string completeTransferNo);

        Dictionary<string, List<string>> TransferNoAccepted(string transferNoAccepted);

        WIPTensouDetails SaveWIPTensouAcceptanceDetails(WIPTensouDetails request);

        void WIPTensouAcceptanceDetailsDelete(List<WIPTensouDetails> request);
    }
}
