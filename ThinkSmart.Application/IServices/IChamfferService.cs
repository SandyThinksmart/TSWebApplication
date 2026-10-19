using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IChamfferService
    {
        List<NameValue> GetEmployeeNameList(string searchText, bool addEmpty);
        List<ChamferDetails> GetChamfferList(GridSearchFields gridSearchRequest);
        ChamferDetails GetChamferingDetailsByCD(long chamferingCD, string cultureName);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban, long chamferingCD);
        List<ThabaDetails> GetThabaDetailsByChuuban(string chubaan, string gyouban);
        List<NameValue> GetNextProcessList();
        void Delete(ChamferDetails request);
        long ChamferDetailsUpdate(ChamferDetails request);
        List<ChamferDetails> GetChamfering24MachineList(string machineCd);
        ChamferDetails GetChamferDetailsByChuuban(string chubaan, string gyouban, string mechineCD, string nifudaNo, string cultureName);
        ChamferDetails InsertChamferDetails(ChamferDetails chamferDetails);
        string GetEmployeeNameByID(long employeeCd);
        public KeyValuePair<string, string> GetMachineNameByID(long machineCd);
        List<NameValue> GetMachineNameList();
        List<NameValue> GetDecisionList();
        List<NameValue> GetUnitTypeList();
        public string GetThabaDetailsByThabaNumber(ChamferDetails request);
        public List<ChamferDetails> GetChamferDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        List<ChamferDetails> GetChamferDetailsOnDailyProcessAnalysis(GridSearchFields gridresult);
        int GetRowCountForChamfferFactory(GridSearchFields gridresult);
        int GetRowCountForChamfferDailyAnalysis(GridSearchFields gridresult);
    }
}
