using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IAllCommonService
    {
        List<NameValue> GetProcessDefectList(int kubunCD);

        List<NameValue> GetProductQuntityList();
        KeyValuePair<string, string> GetMachineNameByID(long machineCd);
        NameValue GetEmployeeNameByID(long employeeCd);
        CommonFields ValidateChubban(string chuuban, short gyuban);
        int GetEmployeeNameActiveID(long employeeCd);
        KeyValuePair<string, string> IsCheckEmployeeActiveInctiveByID(long employeeCd);
        void MessageDetailsDelete(long messageCD);
        List<NameValue> GetOperatorName(string name, int operatorCD);
        List<NameValue> GetMachineName(string? machineType="");
        List<NameValue> GetConditionName();
        List<string> GetChuubanListForInvaidCheck(string enteredCChubaans);
        List<string> GetCompletedChuubanList(string enteredCChubaans);
        List<NameValue> GetFutouName(string name);
        List<NameValue> GetMochikomisakiName(string name);
        List<NameValue> GetCustomerName(string name);
        List<NameValue> GetCustomerList(string name, int customerCD);
        List<NameValue> GetNextProcessList(int processTypeCD);
        List<NameValue> GetMaterialMakerName();
        List<NameValue> GetMakerName(string name);
        List<NameValue> GetMakerName();
        List<MstConfiguration> GetFTPDetail();
        int GetUserLocation(int userID);

        //string ExportCSV(DataTable dt,string fileName);
        string ExportCSV(string sql, string PLSqlName, string outPutTable, string fileName,DataTable dataTable);
        string GetFileName(string fileName, bool addDate, bool addTime);
        string GetCultureName(string cultureName);
        CommonFields MappChuubanAndGyoubanByBarcode(string orderNo);
        CommonFields MappNifudaByBarcode(string nifudaNo);
        WipStatusDetails SaveWIPDataTabaWise(WipStatusDetails WipStatusDetails);
        WipStatusDetails SaveWIPDataCombineTaba(WipStatusDetails WipStatusDetails); 
        WipStatusDetails InsertWIPStatusData(WipStatusDetails WipStatusDetails);
        WipStatusDetails InsertFIBulkWIPStatusData(WipStatusDetails WipStatusDetails);
        WipStatusDetails DeleteWIPStatusDataPreProcess(WipStatusDetails WipStatusDetails); 
        WipStatusDetails DeleteWIPStatusData(WipStatusDetails WipStatusDetails);
        List<NameValue> GetAllOperatorsList();
        string GetLocationName(int userID);
        int GetBatchRollFromConfig();
        DateTime GetSystemDateTime();
        List<NameValue> GetMukesakiName(string name, int mukesakiCD);
        List<NameValue> GetSupplierName(string name, int supplierCD);
        List<NameValue> GetMochikomisakiList(string name, int mochikomisakiCD);

        CommonFields GetChuubanGyuobanByNifuda(string nifudaNo);
        List<NameValue> GetOperatorDeptName(string name, int operatorCD);
    }
}
