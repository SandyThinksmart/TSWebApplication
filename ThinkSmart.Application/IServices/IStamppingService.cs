using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IStamppingService
    {
        List<StamppingDetails> GetStamppingList(GridSearchFields gridSearchRequest);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        StamppingDetails GetStamppingDetailsByStamppingCD(long stamppingCD,string cultureName);
        List<NameValue> GetNextProcessList();
        List<NameValue> GetStamp();
        void DeleteStamppingDetails(StamppingDetails request);
        long UpdateStamppingDetails(StamppingDetails request);
        List<NameValue> GetMachineNameList();
        List<NameValue> GetLocationNameList();
        List<NameValue> GetCustomerNameList();
        StamppingDetails GetStamppingDetailsByChuuban(string PP_CHUUBAN, short? PP_GYOUBAN, int MACHINE_CD, string NIFUDA_NO, string cultureName);
        List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban);
        public List<StamppingDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? Thaba_No);
        List<StamppingDetails> GetStampping24MachineList(string machineCd, string cultureName);
        string GetEmployeeNameByID(long employeeCd);
        StamppingDetails InsertStamppingDetails(StamppingDetails request);
        int GetUserLocationById(int userID);
        List<NameValue> GetDecisionList();
        List<NameValue> GetTrialResultList();
        List<NameValue> GetUnitTypeList();
        public List<StamppingDetails> GetStamppingDailyReportOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        public List<StamppingDetails> GetDailyProcessAnalysisStamppingReport(GridSearchFields gridSearchRequest);
        int GetRowCountForStamppingFactory(GridSearchFields gridresult);
        int GetRowCountForStamppingDailyAnalysis(GridSearchFields gridresult);

    }
}
