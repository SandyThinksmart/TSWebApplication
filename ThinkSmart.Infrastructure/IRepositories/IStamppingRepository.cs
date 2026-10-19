using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IStamppingRepository
    {
        List<StamppingDetails> GetStamppingList(List<SearchField> searchFields, string sortCiteria, short? locationCD);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        StamppingDetails GetStamppingDetailsByStamppingCD(long stamppingCD);
        void DeleteStamppingDetails(long stamppingCD);
        MessageDetails MessageDetailsInsert(MessageDetails messageDetails);
        long UpdateStamppingDetails(StamppingDetails request);
        MessageDetails MessageDetailsUpdate(long messageCD, string msgContents, int userID);
        void MessageDetailsDelete(long messageCD);
        List<NameValue> GetMachineNameList(); 
        List<NameValue> GetLocationNameList();
        List<NameValue> GetCustomerNameList(); 
        StamppingDetails GetStamppingDetailsByChuuban(string PP_CHUUBAN, short? PP_GYOUBAN, int MACHINE_CD, string NIFUDA_NO);
        List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc);
        List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban);
        public List<StamppingDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? Thaba_No);
        MST_MACHINE GetMachineNameByID(long machineCd);
        List<StamppingDetails> GetStampping24MachineList(string machineCd);
        string GetEmployeeNameByID(long employeeCd);
        void InsertStamppingDetails(StamppingDetails request);
        int GetUserLocationById(int userID);
        public List<StamppingDetails> GetStamppingDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit,string cultureName);      
        public List<StamppingDetails> GetDailyProcessStamppingReport(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);
        int GetRowCountForStamppingFactory(List<SearchField> searchFields);
        int GetRowCountForStamppingDailyAnalysis(List<SearchField> searchFields);
        
    }
}
