using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IAllCommonRepository
    {
        List<NameValue> GetProcessDefectList(int kubunCD);
        MST_MACHINE GetMachineNameByID(long machineCd);
        MessageDetails MessageDetailsInsert(MessageDetails messageDetails);
        NameValue GetEmployeeNameByID(long employeeCd);
        int GetEmployeeNameActive(long employeeCd);
        List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc);
        void MessageDetailsDelete(long messageCD);
        MessageDetails MessageDetailsUpdate(MessageDetails messageDetails);
        ECDetails GetOperatorDetailsById(long machineCd);
        List<NameValue> GetOperatorNameList(string name, int operatorCD);
        List<NameValue> GetMachineNameList(string? machineType = "");
        List<NameValue> GetConditionNameList();
        List<string> GetChuubanListForInvaidCheck(string chuubanList);
        List<string> GetCompletedChuubanList(string chuubanList);

        List<NameValue> GetFutouNameList(string searchText);
        List<NameValue> GetFutouNameList();
        List<NameValue> GetAllMochikomisakiActiveNameList(string searchText);
        List<NameValue> GetAllMochikomisakiActiveNameList();
        List<NameValue> GetCustomerNameList(string name);
        List<NameValue> GetCustomerList(string name, int customerCD);
        List<NameValue> GetMaterialMakerNameList();
        List<NameValue> GetMakerNameAndCDList(string name);
        List<NameValue> GetMakerNameAndCDList();
        List<MstConfiguration> GetFTPDetail();
        int GetUserLocationById(int userID);
        List<NameValue> GetNextProcesKubun(int processTypeCD);
        void InsertWIPStatusData(WipStatusDetails request);
        void DeleteWIPStatusData(WipStatusDetails wipDetails); 
        void DeleteWIPStatusDataPreProcess(long processPlanCD, short thabaNo);
        //string ExportCSV(DataTable dt, string fileName);
        string ExportCSV(string sql, string PLSqlName, string outPutTable, string fileName,DataTable dataTable);
        CommonFields ValidateChubban(string chuuban, short gyuban);
        CommonFields GetMappedChuubanAndGyouban(string orderNo);
        CommonFields GetMappedNifuda(string nifudaNo);
        string GetLocationName(int userId);
        List<NameValue> GetOperatorList();
        int GetBatchRollFromConfig();
        int GetMsgCDByNifudaCD(int userID);
        DateTime GetSystemDateTime();
        List<NameValue> GetMukesakiNameList(string name, int mukesakiCD);
        List<NameValue> GetSupplierNameList(string name, int supplierCD);
        List<NameValue> GetMochikomisakiNameList(string name, int mochikomisakiCD);

        CommonFields GetChuubanGyuobanByNifuda(string nifudaNo);

        List<NameValue> GetOperatorDeptNameList(string name, int operatorCD);
    }
}
