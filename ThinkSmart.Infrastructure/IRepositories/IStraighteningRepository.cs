using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IStraighteningRepository
    {
        List<StraighteningDetails> GetStraighteningList(List<SearchField> searchCriteria, string sortCiteri, long locationCD);
        void Delete(long id);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        StraighteningDetails GetStraighteningDetailsByCD(long StraighteningCD);
        List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, string gyuban, long straighteningCD);
        List<ThabaDetails> GetThabaDetails(string chuuban, string gyuban);
        int GetUserLocationById(int userID);
        MST_MACHINE GetMachineNameByID(long machineCd);
        MessageDetails MessageDetailsInsert(MessageDetails messageDetails);
        MessageDetails MessageDetailsUpdate(long messageCD, string msgContents, int userID);
        List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc);
        long StraighteningDetailsUpdate(StraighteningDetails request);
        StraighteningDetails GetStraighteningDetailsByChuuban(string chuuban, string gyouban, string machineCD);
        List<StraighteningDetails> GetStraighteningHistoryByChuuban(string chuuban, string gyouban);
        List<NameValue> GetMachineNameList();
        StraighteningDetails GetStraighteningDetailsByChuubanGyouban(string chuuban, string gyouban);
        void InsertStraighteningDetails(StraighteningDetails straighteningDetails);
        List<StraighteningDetails> GetStraightening24MachineList(string machineCd);
        List<StraighteningDetails> GetStraighteningDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit,string cultureName);//List<SearchField> searchFields,
        List<StraighteningDetails> GetStraighteningDailyReportOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);//List<SearchField> searchFields,

        public List<StraighteningDetails> GetThabaDetailsByThabaNumber(int processPlanCD, string Thaba_No_String);
        int GetRowCountForStraighteningFactory(List<SearchField> searchFields);
        int GetRowCountForStraighteningDailyAnalysis(List<SearchField> searchFields);
        List<NameValue> GetStraighteningMachineList();

    }
}
