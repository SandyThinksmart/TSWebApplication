using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IHikinukiCoilToBarService
    {
        List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban);
        HikinukiDetails GetHikinukiDetailsByChuuban(string chuuban, string gyouban, string mechineCD, string cultureName);
        HikinukiDetails GetHikinukiHistoryByChuuban(GridSearchFields request);
        List<HikinukiDetails> GetHikinuki24MachineDetails(string machineCd, string cultureName);
        HikinukiDetails InsertHikinukiDetails(HikinukiDetails request);
        long UpdateHikinukiDetails(HikinukiDetails request);
        List<HikinukiDetails> GetHikinukiCoilToBarList(GridSearchFields gridSearchRequest);
        HikinukiDetails GetHikinukiDetailsDrawingByCD(long drawingCD, string cultureName);
        void Delete(HikinukiDetails details);

        List<HikinukiDetails> GetCombinedDailyReportData(GridSearchFields gridresult);

        List<HikinukiDetails> GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(GridSearchFields gridresult);
        List<NameValue> GetTPTestList();
        List<NameValue> GetDieDiaShapeList();
        List<NameValue> GetUnitTypeList();
        List<NameValue> GetBendStatusList();
        List<NameValue> GetExteriorConditionList();
        List<NameValue> GetDecisionList();

        List<NameValue> GetNextProcessList();
        string GetThabaDetailsByThabaNumber(GridSearchFields request);

        int GetRowCountForCombinedDailyReportFactory(GridSearchFields gridresult);
        int GetRowCountForHikinukiCoilToBarDailyAnalysis(GridSearchFields gridresult);
        int GetHikinukiDataForOracleToHost(string startDate, string endDate, int userID,string type);
        int GetBatchStatusCode();
        List<NameValue> GetHikinukiCoilToBarMachineTypeList();
    }
}
