using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IECRepository
    {
        ECDetails GetECDetailsByChuuban(string nifudaNo, short? mechineCD);
        List<ECDetails> GetEC24MachineList(string machineCd, string cultureName);
        List<ThabaDetails> GetThabaDetails(string chuuban, short? gyouban);
        ECDetails GetECProcessPlanDetailsByChuuban(string chubaan, short? gyouban);
        ECDetails InsertECDetails(ECDetails ecDetails);
        long UpdateECDetails(ECDetails request);
        FinalInspectionDetails Insert(FinalInspectionDetails finalInspection);
        void Delete(long ecCD);
        ECDetails GetECDetailsByECCD(long ecCD);
        List<ECDetails> GetECDetailsList(List<SearchField> searchCriteria, string sortCiteria, short locationCD);
        ECDetails GetOperatorDetailsById(long machineCd);
        List<ECDetails> GetECDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit);
        public List<ECDetails> GetDailyProcessECDetails(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);
        int FIAlreadyDone(long processPlanCD, short thabaNo);
        List<ECDetails> IsFICompleted(long nifudaCD, short thabaNo, int ecCD);
        ThabaDetails GetThabaDetailsByThabaNumber(long processPlanCD, short thabaNo);
        void DeleteFinalInspection(long ecCD);
        void UpdateFinalInspection(FinalInspectionDetails finalInspection);
        string GetFinalInspData(long ecCD);
        List<ECDetails> GetProcessHistoryList(string chuubanNo, short? gyoubanNo, string cultureName);
        int GetRowCountForECFactory(List<SearchField> searchFields);
        int GetRowCountForECDailyAnalysis(List<SearchField> searchFields);
        ECDetails GetPastECDetailsByProcessPlanCD(short machineCD, int processPlanCD);
        List<GridSearchFields> GetHardnessDescion(long processPlanCD);
        List<GridSearchFields> GetTensileDescion(long processPlanCD);
    }


}
