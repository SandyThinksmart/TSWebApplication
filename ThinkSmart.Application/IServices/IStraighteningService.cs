using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IStraighteningService
    {
        List<StraighteningDetails> GetStraighteningList(GridSearchFields gridSearchRequest);
        void Delete(StraighteningDetails request);
        StraighteningDetails GetStraighteningDetailsByCD(long straighteningCD);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban, long straighteningCD);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban);
        long StraighteningDetailsUpdate(StraighteningDetails request);
        StraighteningDetails GetStraighteningDetailsByChuuban(GridSearchFields request);
        StraighteningDetails GetStraighteningHistoryByChuuban(string chuuban, string gyouban,string culture);
        StraighteningDetails InsertStraighteningDetails(StraighteningDetails request);
        List<StraighteningDetails> GetStraightening24MachineList(string machineCd, string cultureName);
        List<StraighteningDetails> GetStraighteningDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        List<StraighteningDetails> GetStraighteningDailyReportOnDailyProcessAnalysis(GridSearchFields gridresult);
        List<NameValue> GetNextProcessList();
        List<NameValue> GetTPTestList();
        List<NameValue> GetUnitTypeList();
        List<NameValue> GetExternalSurfaceConditionList();
        List<NameValue> GetGetBendStatusList();
        List<NameValue> GetDecisionList();
        List<NameValue> GetMachineNameList();
        public string GetThabaDetailsByThabaNumber(GridSearchFields request);
        int GetRowCountForStraighteningFactory(GridSearchFields gridresult);
        int GetRowCountForStraighteningDailyAnalysis(GridSearchFields gridresult);
        List<NameValue> GetStraighteningMachineList();

    }
}
