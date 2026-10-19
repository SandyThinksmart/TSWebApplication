using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IECService
    {
        ECDetails GetECDetailsByChuuban(string nifuda, short? mechineCD, string cultureName);
        List<ECDetails> GetEC24MachineDetails(string machineCd, string cultureName);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, short? gyouban);
        ECDetails GetECHistoryByChuuban(string chuuban, short? gyouban, string cultureName);
        ECDetails InsertECDetails(ECDetails ecDetails);
        long UpdateECDetails(ECDetails ecDetails);
        void Delete(ECDetails request);
        ECDetails GetECDetailsByECCD(long ecCD, string cultureName);
        List<ECDetails> GetECList(GridSearchFields gridSearchRequest);
        string IsCheckEmployeeActiveInctiveByID(long machineCd);
        List<ECDetails> GetECDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest);
        public List<ECDetails> GetDailyProcessAnalysisECDeatils(GridSearchFields gridSearchRequest);
        List<NameValue> GetTPTestList();
        List<NameValue> GetNextProcessList();
        List<NameValue> GetDecisionList();
        List<NameValue> GetFIResultList();
        List<NameValue> GetProductUnitKubun();
        int IsFIAlreadyDone(long processPlanCD, short thabaNo);
        List<ECDetails> IsFinalInspectionCompleted(long processPlanCD, short thabaNo, int ecCD);
        ThabaDetails GetThabaDetailsByThabaNumber(long processPlanCD, short thabaNo);
        string GetFinalInspByECCD(long ecCD);
        int GetRowCountForECFactory(GridSearchFields gridresult);
        int GetRowCountForECDailyAnalysis(GridSearchFields gridresult);
        string GetHardnessDescion(long processPlanCD,string cultureName);
        string GetTensileDescion(long processPlanCD, string cultureName);
    }
}
