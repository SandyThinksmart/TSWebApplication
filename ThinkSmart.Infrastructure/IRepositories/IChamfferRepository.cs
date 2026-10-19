using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IChamfferRepository
    {
        List<NameValue> GetEmployeeNameList(string searchText, bool addEmpty);
        List<ChamferDetails> GetChamferingList(List<SearchField> searchFields, string sortCiteria, short? locationCD);
        ChamferDetails GetChamferingDetailsByCD(long chamferingCD);
        List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, string gyouban, long chamferingCD);
        void Delete(long chamferingCD);
        long ChamferDetailsUpdate(ChamferDetails request);
        List<ChamferDetails> GetChamfering24MachineList(string machineCd);
        ChamferDetails GetChamferDetailsByChuubanGyoubanMachine(string chuuban, short gyouban, string machineCD, string nifudaNo);
        List<ThabaDetails> GetThabaDetails(string chuuban, string gyuban);
        void InsertChamferDetails(ChamferDetails request);
        string GetEmployeeNameByID(long employeeCd);
        int GetUserLocationById(int userID);
        MST_MACHINE GetMachineNameByID(long machineCd);
        List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc);
        MessageDetails MessageDetailsInsert(MessageDetails messageDetails);
        List<NameValue> GetMachineNameList();
        public List<ChamferDetails> GetThabaDetailsByThabaNumber(string chuuban, string gyouban, string Thaba_No_String);
        public List<ChamferDetails> GetChamferDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit);

        List<ChamferDetails> GetChamferDetailsOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);

        int GetRowCountForChamfferFactory(List<SearchField> searchFields);
        int GetRowCountForChamfferDailyAnalysis(List<SearchField> searchFields);
    }
}
