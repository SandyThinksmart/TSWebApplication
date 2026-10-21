using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IStraightening2Service
    {
        List<Straightening2Details> GetStraightening2List(GridSearchFields gridSearchRequest);
        Straightening2Details GetStraightening2DetailsByCD(long straighteningCD,string cultureName);
        void Delete(Straightening2Details request);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chuuban, short? gyouban);
        List<Straightening2Details> GetMachine24Details(string machineCd, string cultureName);
        Straightening2Details GetStraighteningDetailsByChuuban(string chuuban, short? gyouban, int? machineCD,string cultureName);
        Straightening2Details InsertStraightening2Details(Straightening2Details request);
        long Straightening2DetailsUpdate(Straightening2Details request);
        List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, short? gyuban, long? straighteningCD);
        //public List<Straightening2Details> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, string THABA_NO_STRING);
        string GetThabaDetailsByThabaNumber(GridSearchFields request);
        List<NameValue> GetUnitTypeList();
        List<NameValue> GetTPTestList();
        List<NameValue> GetNextProcessList();
        List<NameValue> GetDecisionList();
        List<NameValue> GetExteriorConditionList();
        List<NameValue> GetBendStatusList();
        List<Straightening2Details> GetStraightning2DailyReportDataOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest);
        List<Straightening2Details> GetStraightning2DailyProcessAnalysisStatus(GridSearchFields gridSearchRequest);
        List<NameValue> GetMachineNameList();
      
        int GetRowCountForStraightening2Factory(GridSearchFields gridresult);
        int GetRowCountForStraightening2DailyAnalysis(GridSearchFields gridresult);
    }
}
