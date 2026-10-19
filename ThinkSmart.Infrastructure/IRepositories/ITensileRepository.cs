using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface ITensileRepository
    {
        //List<TensileDetails> GetTensileList(List<SearchField> searchFields, string sortCiteria);
        int GetUserLocationById(int userID);
        TensileDetails GetTensileDetailsByChuuban(string chuuban, short? gyouban);
        List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc);
       // List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        public List<NameValue> GetJISList();

        TensileDetails InsertTensileDetails(TensileDetails tensileDetails);
        public List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long tensileCD);
        public List<TensileDetails> GetTensileLoadList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD);
        public TensileDetails GetTensileDetailsByCD(long tensileCD, string chuuban, long gyouban);
        public long UpdateTensileDetails(TensileDetails tensileDetails);

        public ThabaDetails GetThabaDetailsByThabaNumber(long processPlanCD, short tabaNo);

        public TensileDetails GetFurnanceNo(string chuuban, short? gyouban,int detail_No);

        public string DeleteTensileDetails(long tensileCD);

        public List<TensileDetails> GetMECHTensileOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria, int rowLimit);
        public List<TensileDetails> GetMECHTensileOnDailyProcessStatus(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridresult);

        int GetRowCountForMechTensileFactory(List<SearchField> searchFields);
        int GetRowCountForMechTensileDailyAnalysis(List<SearchField> searchFields);
        decimal GetJISTestMagnification(int testSpecCd);
        List<TensileDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban);
        void UpdateFICombinedDecision(short tensileDecision, long processPlanCd);
        string ValidateTensileDetailsOnUpdateBtn(long MITensileCD);
    }
}
