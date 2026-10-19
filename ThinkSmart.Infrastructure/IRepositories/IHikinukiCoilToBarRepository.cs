using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IHikinukiCoilToBarRepository
    {
        List<ThabaDetails> GetThabaDetails(string chuuban, string gyouban);
        HikinukiDetails GetHikinukiDetailsByChuuban(string chuuban, string gyouban, string mechineCD);
        HikinukiDetails GetHikinukiHistoryDetailsByChuuban(string chubaan, short? gyouban);
        List<HikinukiDetails> GetProcessHistoryList(string chuubanNo, short? gyoubanNo, string cultureName);
        List<HikinukiDetails> GetHikinuki24MachineList(string machineCd);
        HikinukiDetails InsertHikinukiDetails(HikinukiDetails hikinukiDetails);
        long UpdateHikinukiDetails(HikinukiDetails hikinukiDetails);
        List<HikinukiDetails> GetHikinukiList(List<SearchField> searchCriteria, string sortCiteria, short locationCD);
        HikinukiDetails GetHikinukiDetailsDrawingByCD(long drawingCD);
        void Delete(long DRAWING_CD);
        List<HikinukiDetails> GetCombinedDailyReportData(List<SearchField> searchFields, string sortCiteria,int rowLimit,string cultureName);
        List<HikinukiDetails> GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);

        List<HikinukiDetails> GetThabaDetailsByThabaNumber(long processPlanCD, string Thaba_No_String);

        int GetRowCountForCombinedDailyReportFactory(List<SearchField> searchFields);
        int GetRowCountForHikinukiCoilToBarDailyAnalysis(List<SearchField> searchFields);
        int GetHikinukiList(string startDate, string endDate, int locationCD, string type);
        int GetStatusCode();
        List<NameValue> GetHikinukiCoilToBarMachineTypeList();
    }
}
