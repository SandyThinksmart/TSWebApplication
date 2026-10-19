using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IStraightening2Repository
    {
        List<Straightening2Details> GetStraightening2List(List<SearchField> searchCriteria, string sortCiteria, long? locationCD);
        void Delete(long id);
        int GetUserLocationById(int userID);
        List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc);
        Straightening2Details GetStraightening2DetailsByCD(long StraighteningCD);
        List<ThabaDetails> GetThabaDetails(string chubaan, short? gyouban);
        List<Straightening2Details> GetStraightening24MachineDetails(string machineCd);
        Straightening2Details GetStraighteningDetailsByChuubanGyoubanMachine(string chuuban, short? gyouban, int? machineCD);
        void InsertStraightening2Details(Straightening2Details request);
        long UpdateStraightening2Details(Straightening2Details request);
        List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, short? gyuban, long? straighteningCD);
        //public List<Straightening2Details> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, string Thaba_No_String);
        List<Straightening2Details> GetThabaDetailsByThabaNumber(long processPlanCD, string Thaba_No_String);
        List<Straightening2Details> GetStraightning2DailyReportDataOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria,int rowLimit,string cultureName);
        List<Straightening2Details> GetStraightning2DailyProcessAnalysisStatus(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridresult);
        List<NameValue> GetMachineNameList();
        int GetRowCountForStraightening2Factory(List<SearchField> searchFields);
        int GetRowCountForStraightening2DailyAnalysis(List<SearchField> searchFields);
    }
}
