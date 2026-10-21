using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IWIPTensouRepository
    {
       WIPTensouDetails GetTensouDetailsByChuubanGyouban(string nifuda);
        int SaveWIPTensouDetails(WIPTensouDetails request);
        int GetMaxGroupNo();
        string GetMessageFromMsgID(int MSG_CD);
        int GetMsgCdByMsgContent(string MSG_CONTENTS);
        List<WIPTensouDetails> GetWIPTensouList(List<SearchField> searchFields, string sortCiteria, long locationCD);
        List<WIPTensouDetails> GetWIPTransferDetailsByTrasferCD(long WIP_TRANSFER_CD); 
        List<WIPTensouDetails> IsAcceptanceCompleted(long WIP_TRANSFER_CD); 
        List<WIPTensouDetails> GetDataOnGridView(List<SearchField> searchFields, string sortCiteria, long locationCD);
        void WIPTensouDetailsUpdate(WIPTensouDetails request);
        int WIPTensouDetailsDelete(long WIPTransferCD);
        
        int GetWIPRequestReportData(string transferDate, long userId);
        int GetWIPAcceptanceReportData(WIPTensouDetails details);
        
        
        int GetRowCountForTransferReport(List<SearchField> searchFields);
        List<WIPTensouDetails> GetTransferDetails(List<SearchField> searchFields, string sortCiteria, int rowLimit);

        List<WIPTensouDetails> GetTransferCD(long transferGroupCD);

    }
}
