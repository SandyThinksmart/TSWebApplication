using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Utility;
using System.Data;
using System.Globalization;
using TSWebApplication.Infrastructure.Repositories;
using TSWebApplication.Domain.Model;
using System.Transactions;
using System.Web;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Application.Services
{

    public class AllCommonService : IAllCommonService
    {

        private readonly IAllCommonRepository _CommonRepository;
        private readonly LocalizationServices _localization;
        public AllCommonService(IAllCommonRepository commonRepository, LocalizationServices localization)
        {
            _localization = localization;
            _CommonRepository = commonRepository;
        }
        public List<NameValue> GetProcessDefectList(int kubunCD)
        {
            var _processDefectList = _CommonRepository.GetProcessDefectList(kubunCD);
            return _processDefectList;
        }

        public List<NameValue> GetProductQuntityList()
        {
            var _prod_kubun = UIUtils.Instance.GetProductUnitKubun();
            return _prod_kubun;
        }


        public KeyValuePair<string, string> GetMachineNameByID(long machineCd)
        {
            var data = (machineCd > 0) ? _CommonRepository.GetMachineNameByID(machineCd) : null;
            if (data == null)
            {
                return new KeyValuePair<string, string>(null, null);
            }
            else
            {
                string inActive = data.IN_ACTIVE_FLAG == 1 ? "inactive" : "active";
                return new KeyValuePair<string, string>(data.MACHINE_NAME, inActive);  // Change Short Name to Full Name Client Visit[july 2025].
            }
        }
        public NameValue GetEmployeeNameByID(long employeeCd)
        {
            return (employeeCd > 0) ? _CommonRepository.GetEmployeeNameByID(employeeCd) : null;
        }

        public int GetEmployeeNameActiveID(long employeeCd)
        {
            return  Convert.ToInt32(_CommonRepository.GetEmployeeNameActive(employeeCd));
        }
        public KeyValuePair<string, string> IsCheckEmployeeActiveInctiveByID(long opratorCd)
        {
            var data = (opratorCd > 0) ? _CommonRepository.GetOperatorDetailsById(opratorCd) : null;
            if (data == null)
            {
                return new KeyValuePair<string, string>(null, null);
            }
            else
            {
                string inActive = data.IN_ACTIVE_FLAG == 1 ? "inactive" : "active";
                return new KeyValuePair<string, string>(data.SHORT_NAME, inActive);
            }
        }
        public void MessageDetailsDelete(long messageCD)
        {
            _CommonRepository.MessageDetailsDelete(messageCD);
        }
        public List<NameValue> GetOperatorName(string name, int operatorCD)
        {
            return _CommonRepository.GetOperatorNameList(name, operatorCD);
        }
        public List<NameValue> GetAllOperatorsList()
        {
            return _CommonRepository.GetOperatorList();
        }
        
        public List<NameValue> GetMachineName(string? machineType)
        {
            return _CommonRepository.GetMachineNameList(machineType);
             
        }
        public List<NameValue> GetConditionName()
        {
            return _CommonRepository.GetConditionNameList();

        }
        public List<string> GetChuubanListForInvaidCheck(string enteredCChubaans)
        {
            return _CommonRepository.GetChuubanListForInvaidCheck(enteredCChubaans);
        }
       public List<string> GetCompletedChuubanList(string enteredCChubaans)
        {
            return _CommonRepository.GetCompletedChuubanList(enteredCChubaans);
        }
        public List<NameValue> GetFutouName(string searchText)
        {
            var futouList = new List<NameValue>();

            if (searchText != null)
            {
                futouList = _CommonRepository.GetFutouNameList(searchText);
            }
            else
            {
                futouList = _CommonRepository.GetFutouNameList();
            }
             return futouList;
        }
        public List<NameValue> GetMochikomisakiName(string searchText)
        {
             var mochikomisakiList = new List<NameValue>();

            if (searchText != null)
            {
                mochikomisakiList = _CommonRepository.GetAllMochikomisakiActiveNameList(searchText);
            }
            else
            {
                mochikomisakiList = _CommonRepository.GetAllMochikomisakiActiveNameList();
            }
            return mochikomisakiList;
        }
        public List<NameValue> GetCustomerName(string name)
        {
            return _CommonRepository.GetCustomerNameList(name);
        }
        public List<NameValue> GetCustomerList(string name, int customerCD)
        {
            return _CommonRepository.GetCustomerList(name,customerCD);
        }
        public List<NameValue> GetMaterialMakerName()
        {
            return _CommonRepository.GetMaterialMakerNameList();
        }
        public List<NameValue> GetMakerName(string name)
        {
            return _CommonRepository.GetMakerNameAndCDList(name);
        }

        public List<NameValue> GetMakerName()
        {
            return _CommonRepository.GetMakerNameAndCDList();
        }
        public List<MstConfiguration> GetFTPDetail()
        {
            return _CommonRepository.GetFTPDetail();
        }
        public int  GetUserLocation(int userID)
        {
            return _CommonRepository.GetUserLocationById(userID);
        }
        public List<NameValue> GetNextProcessList(int processTypeCD)
        {
            var _nextProcessList = _CommonRepository.GetNextProcesKubun(processTypeCD);
            return _nextProcessList;
        }
        //public string ExportCSV(DataTable dt ,string fileName)
        //{
        //    return _CommonRepository.ExportCSV(dt ,fileName);
        //}

        public string ExportCSV(string sql, string PLSqlName, string outPutTable, string fileName,DataTable dataTable)
        {
            return _CommonRepository.ExportCSV(sql, PLSqlName, outPutTable, fileName,dataTable);

        }

        public string GetFileName(string fileName, bool addDate,bool addTime)
        {
            string fileNameFormat = fileName;

            DateTime date = DateTime.Now;
            var hour = date.Hour;
            var minute = date.Minute;
            var second = date.Second;

            var hrs = hour.ToString();
            var minutes = minute.ToString();
            var seconds = second.ToString();


            if (hrs.Length < 2)
            {
                hrs = "0" + hrs;
            }
            else
            {
                hrs = hrs;
            }
            if (minutes.Length < 2)
            {
                minutes = "0" + minutes;
            }
            else
            {
                minutes = minutes;
            }
            if (seconds.Length < 2)
            {
                seconds = "0" + seconds;
            }
            else
            {
                seconds = seconds;
            }
            var time = hrs + "" + minutes + "" + seconds;


            string formattedDate = date.ToString("yyyyMMdd");

            if(addDate)
            {
                fileNameFormat = fileNameFormat + "_" + formattedDate;
            }
            if(addTime)
            {
                fileNameFormat = fileNameFormat + "_" + time;
            }

             


            return fileNameFormat;

        }

        public string GetCultureName(string cultureName)
        {
            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            return cultureName;
        }
        public CommonFields ValidateChubban(string chuuban, short gyuban)
        {
            return _CommonRepository.ValidateChubban(chuuban, gyuban);
        }

        public CommonFields MappChuubanAndGyoubanByBarcode(string orderNo)
        {
            return _CommonRepository.GetMappedChuubanAndGyouban(orderNo);
        }
        public CommonFields MappNifudaByBarcode(string nifudaNo)
        {
           return _CommonRepository.GetMappedNifuda(nifudaNo);
        }
      
        public WipStatusDetails SaveWIPDataTabaWise(WipStatusDetails request)
        {
            WipStatusDetails result = new WipStatusDetails();
            
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();


                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.PROCESS_START_DATE = request.PROCESS_START_DATE;
                wipDetails.PROCESS_END_DATE = request.PROCESS_START_DATE;
                wipDetails.START_OPERATOR_CD = request.START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = request.START_OPERATOR_CD;
               if( request.PROCESS_KUBUN_CD == 0)
                {
                    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHTWipDivision")));
                }
                wipDetails.TABA_NO = request.TABA_NO1;
                wipDetails.NIFUDA_CD = 0;
                if (request.TABA_NO1 != null && request.TABA_NO2 != null || request.TABA_NO3 != null || request.TABA_NO4 != null)
                {
                    wipDetails.TABA_GROUP_NO = request.TABA_NO1;
                    wipDetails.FINISHED_BARS = request.NO_OF_UNITS_TABA_1;
                    wipDetails.UNIT = request.UNIT_TABA_1;
                    wipDetails.FINISHED_QTY = request.QUANITY_TABA_1;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS_TABA_1);
                    wipDetails.TABA_NO = request.TABA_NO1;
                  //  _CommonRepository.InsertWIPStatusData(wipDetails);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);

                }

                if (request.TABA_NO2 > 0)
                {
                    wipDetails.FINISHED_BARS = request.NO_OF_UNITS_TABA_2;
                    wipDetails.UNIT = request.UNIT_TABA_2;
                    wipDetails.FINISHED_QTY = request.QUANITY_TABA_2;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS_TABA_2);
                    wipDetails.TABA_NO = request.TABA_NO2;
                    //_CommonRepository.InsertWIPStatusData(wipDetails);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.TABA_NO3 > 0)
                {

                    wipDetails.FINISHED_BARS = request.NO_OF_UNITS_TABA_3;
                    wipDetails.UNIT = request.UNIT_TABA_3;
                    wipDetails.FINISHED_QTY = request.QUANITY_TABA_3;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS_TABA_3);
                    wipDetails.TABA_NO = request.TABA_NO3;
                    //_CommonRepository.InsertWIPStatusData(wipDetails);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                if (request.TABA_NO4 > 0)
                {

                    wipDetails.FINISHED_BARS = request.NO_OF_UNITS_TABA_4;
                    wipDetails.UNIT = request.UNIT_TABA_4;
                    wipDetails.FINISHED_QTY = request.QUANITY_TABA_4;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS_TABA_4);
                    wipDetails.TABA_NO = request.TABA_NO4;
                    // _CommonRepository.InsertWIPStatusData(wipDetails);
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                }
                transaction.Complete();
            
            
            }
            return null;  
        }
        public WipStatusDetails SaveWIPDataCombineTaba(WipStatusDetails request)
        {
            WipStatusDetails result = new WipStatusDetails();
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();
                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = null;
                wipDetails.PROCESS_START_DATE = request.PROCESS_START_DATE;
                wipDetails.PROCESS_END_DATE = request.PROCESS_START_DATE;
                wipDetails.START_OPERATOR_CD = request.START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = request.START_OPERATOR_CD;
                if (request.PROCESS_KUBUN_CD == 0)
                {
                    wipDetails.PROCESS_KUBUN_CD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessHTWipCombine")));
                }
                wipDetails.TABA_NO = request.TABA_NO1;
                wipDetails.NIFUDA_CD = 0;
                if (request.TABA_NO1 > 0)
                {
                    _CommonRepository.DeleteWIPStatusDataPreProcess(request.PROCESSPLANCD, Convert.ToInt16(request.TABA_NO1));
                }
                if (request.TABA_NO2 > 0)
                {
                    _CommonRepository.DeleteWIPStatusDataPreProcess(request.PROCESSPLANCD, Convert.ToInt16(request.TABA_NO2));
                }
                if (request.TABA_NO3 > 0)
                {
                    _CommonRepository.DeleteWIPStatusDataPreProcess(request.PROCESSPLANCD, Convert.ToInt16(request.TABA_NO3));
                }
                if (request.TABA_NO4 > 0)
                {
                    _CommonRepository.DeleteWIPStatusDataPreProcess(request.PROCESSPLANCD, Convert.ToInt16(request.TABA_NO4));
                }
                if (request.TABA_NO1 != null)
                {
                    wipDetails.TABA_GROUP_NO = request.TABA_NO1;
                    wipDetails.FINISHED_BARS = request.NO_OF_UNITS_TABA_1;
                    wipDetails.UNIT = request.UNIT_TABA_1;
                    wipDetails.FINISHED_QTY = request.QUANITY_TABA_1;
                    wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS_TABA_1);
                    wipDetails.TABA_NO = request.TABA_NO1;
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                    //_CommonRepository.InsertWIPStatusData(wipDetails);

                }
                transaction.Complete();

            }
            return null;
        }

        public WipStatusDetails InsertFIBulkWIPStatusData(WipStatusDetails request)
        {
            int locationCD = GetUserLocationById(Convert.ToInt32(request.PP_USER_ID));
            // Get msgCD
            int msgCD = GetMsgCDByNifudaCD(Convert.ToInt32(request.NIFUDA_CD));
            WipStatusDetails result = new WipStatusDetails();
            using (TransactionScope transaction = new TransactionScope())
            {
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(locationCD);
                wipDetails.MSG_CD = msgCD;
                wipDetails.FINISHED_BARS = request.WIP_NO_OF_UNITS;
                wipDetails.UNIT = request.UNIT;
                wipDetails.FINISHED_QTY = request.WIP_QTY;
                wipDetails.PROCESS_START_DATE = Convert.ToDateTime(formattedDate);
                wipDetails.PROCESS_END_DATE = Convert.ToDateTime(formattedDate);
                wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                wipDetails.PRE_PROSESS_DECISION = request.PRE_PROSESS_DECISION;
                wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS);
                wipDetails.START_OPERATOR_CD = request.START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = request.END_OPERATOR_CD;
                wipDetails.TABA_NO = request.TABA_NO1;
                wipDetails.NIFUDA_CD = request.NIFUDA_CD;
                wipDetails.TABA_GROUP_NO = request.TABA_GROUP_NO;

                if (request.TABA_NO1 != null)
                {
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                    //_CommonRepository.InsertWIPStatusData(wipDetails);

                }
                transaction.Complete();

            }
            return null;
        }



        public WipStatusDetails InsertWIPStatusData(WipStatusDetails request)
        {
            WipStatusDetails result = new WipStatusDetails();
            // Get msgCD
            int msgCD = GetMsgCDByNifudaCD(Convert.ToInt32(request.NIFUDA_CD));
          
            using (TransactionScope transaction = new TransactionScope())
            {
                WipStatusDetails wipDetails = new WipStatusDetails();

                wipDetails = new WipStatusDetails();
                wipDetails.PROCESS_PLAN_CD = request.PROCESS_PLAN_CD;
                wipDetails.LOCATION_CD = Convert.ToInt16(request.LOCATION_CD);
                wipDetails.MSG_CD = msgCD;
                wipDetails.FINISHED_BARS = request.WIP_NO_OF_UNITS;
                wipDetails.UNIT = request.UNIT;
                wipDetails.FINISHED_QTY = request.WIP_QTY;
                wipDetails.PROCESS_START_DATE = request.PROCESS_START_DATE;
                wipDetails.PROCESS_END_DATE = request.PROCESS_START_DATE;
                wipDetails.PROCESS_KUBUN_CD = request.PROCESS_KUBUN_CD;
                wipDetails.PRE_PROSESS_DECISION = request.PRE_PROSESS_DECISION;
                wipDetails.NEXT_PROCESS = Convert.ToInt16(request.NEXT_PROCESS);
                wipDetails.START_OPERATOR_CD = request.START_OPERATOR_CD;
                wipDetails.END_OPERATOR_CD = request.END_OPERATOR_CD;
                wipDetails.TABA_NO = request.TABA_NO1;
                wipDetails.NIFUDA_CD = request.NIFUDA_CD;
                //  wipDetails.TABA_GROUP_NO = request.TABA_NO1;

                if (request.TABA_NO1 != null)
                {
                    _CommonRepository.DeleteWIPStatusData(wipDetails);
                    //_CommonRepository.InsertWIPStatusData(wipDetails); 

                }
                transaction.Complete();

            }
            return null;
        }
    
        public WipStatusDetails DeleteWIPStatusDataPreProcess(WipStatusDetails request)
        {
                if (request.TABA_NO1 != null)
                {
                 _CommonRepository.DeleteWIPStatusDataPreProcess(Convert.ToInt32(request.PROCESS_PLAN_CD), Convert.ToInt16(request.TABA_NO1));
               
            }
         
            return null;

        }
        public WipStatusDetails DeleteWIPStatusData(WipStatusDetails request)
        {
            
                if (request.TABA_NO1 != null)
                {
                 request.TABA_NO = request.TABA_NO1;
                _CommonRepository.DeleteWIPStatusData(request);

                }
               
            return null;        

        }
        public string GetLocationName(int userID)
        {
            return _CommonRepository.GetLocationName(userID);
        }

        public int GetUserLocationById(int userID)
        {
            return _CommonRepository.GetUserLocationById(userID);
        }
        
        public int GetBatchRollFromConfig()
        {
            int count = _CommonRepository.GetBatchRollFromConfig();
            return count;
        }
        public int GetMsgCDByNifudaCD(int nifudaCD)
        {
            return _CommonRepository.GetMsgCDByNifudaCD(nifudaCD);
        }
        public DateTime GetSystemDateTime()
        {
            return _CommonRepository.GetSystemDateTime();
        }
        public List<NameValue> GetMukesakiName(string name, int mukesakiCD)
        {
            return _CommonRepository.GetMukesakiNameList(name, mukesakiCD);
        }
        public List<NameValue> GetSupplierName(string name, int supplierCD)
        {
            return _CommonRepository.GetSupplierNameList(name, supplierCD);
        }
        public List<NameValue> GetMochikomisakiList(string name, int mochikomisakiCD)
        {
            return _CommonRepository.GetMochikomisakiNameList(name, mochikomisakiCD);
        }
        public CommonFields GetChuubanGyuobanByNifuda(string nifudaNo)
        {
            var obj = _CommonRepository.GetChuubanGyuobanByNifuda(nifudaNo);
            return obj;
        }
        public List<NameValue> GetOperatorDeptName(string name, int operatorCD)
        {
            //return _CommonRepository.GetOperatorNameList(name, operatorCD);
            return _CommonRepository.GetOperatorDeptNameList(name, operatorCD);
        }
    }
 }

