using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface ITensileService
    {
        int GetUserLocationById(int userID);
        TensileDetails GetTensileDetailsByChuuban(string chuuban, short? gyouban,string culture_Name);
        // List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        public List<NameValue> GetJISTestList();

        public List<NameValue> GetNextProcessList();

        public List<NameValue> GetDecisionList();
        public List<NameValue> GetDimShapeList();
        public List<NameValue> GetOperatorName(string name, int operatorCD);

        TensileDetails InsertTensileDetails(TensileDetails tensileDetails);
        public List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban, long tensileCD);
        public List<TensileDetails> GetTensileList(GridSearchFields gridSearchRequest);
        public TensileDetails GetTensileDetailsByCD(long TensileCD, string chuuban, long gyouban,string culture_Name);
        long UpdateTensileDetails(TensileDetails tensileDetails);

        public ThabaDetails GetThabaDetailsByThabaNumber(long PROCESS_PLAN_CD, short tabaNo);
        public TensileDetails GetFurnanceNo(string chuuban, short? gyouban,int detail_No);
        public string DeleteTensileDetails(TensileDetails request);

        public List<TensileDetails> GetMECHTensileOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        public List<TensileDetails> GetMECHTensileOnDailyProcessStatus(GridSearchFields gridresult);

        int GetRowCountForMechTensileFactory(GridSearchFields gridresult);
        int GetRowCountForMechTensileDailyAnalysis(GridSearchFields gridresult);
        decimal GetJISTestMagnification(int testSpecCd);
        public List<TensileDetails> GetFICombinedDecisionDetails(string chuuban, short? gyouban);
        void UpdateFICombinedDecision(short tensileDecision, long processPlanCd);
        public string ValidateTensileDetailsOnUpdateBtn(long MITensileCD);
    }
}
