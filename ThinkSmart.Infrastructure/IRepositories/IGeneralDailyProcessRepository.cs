using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IGeneralDailyProcessRepository
    {
        List<GeneralDailyProcessDetails> GetGeneralDailyProcessData(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);
        public GeneralDailyProcessDetails CheckValidationsOnGeneralProcessStart(GeneralDailyProcessDetails processDetails);
        public GeneralDailyProcessDetails checkTabaValidationForOkDecision(GeneralDailyProcessDetails processDetails);
        List<GeneralDailyProcessDetails> GetProcessKubunList();
        public List<GeneralDailyProcessDetails> GetGeneralProcessOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria, int rowLimit);
        public GeneralDailyProcessDetails CheckValidationsForProcessOperatorMachineCD(GeneralDailyProcessDetails processDetails);
        public string getRemarksOfProcess(GeneralDailyProcessDetails processDetails);
        public string saveGeneralProcessPlanEntryData(GeneralDailyProcessDetails processDetails);
        int GetLocationByUserID(int? userID);
        public GeneralDailyProcessDetails CheckValidationsOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails);
        public GeneralDailyProcessDetails getGeneralDailyProcessStartData(GeneralDailyProcessDetails processDetails);
        //List<GeneralDailyProcessDetails> GetNextProcessNameList(GeneralDailyProcessDetails processDetails);
        public GeneralDailyProcessDetails CheckValidationsForOperatorAndEndDateTime(GeneralDailyProcessDetails processDetails);
        public string getRemarksForEndProcess(GeneralDailyProcessDetails processDetails);
        public string updateGeneralProcessPlanEndData(GeneralDailyProcessDetails processDetails);
        //List<GeneralDailyProcessDetails> GetProcessDefectList(GeneralDailyProcessDetails processDetails);
        public string saveGeneralProcessPlanEntryEndData(GeneralDailyProcessDetails processDetails);
        public GeneralDailyProcessDetails GetUnitsWeightOnGeneralProcessEnd(GeneralDailyProcessDetails processDetails);
        public GeneralDailyProcessDetails GetProcessNameOnMergeScreen(short PROCESS_KUBUN_CD);
        public string CheckValidationsProcessMachineOprOnCancel(GeneralDailyProcessDetails processDetails);
        public string DeleteGeneralProcessPlanData(GeneralDailyProcessDetails processDetails);
        int GetRowCountForGeneralDailyProcessFactory(List<SearchField> searchFields);
        int GetRowCountForGeneralProcessDailyAnalysis(List<SearchField> searchFields);
        public List<GeneralDailyProcessDetails> CheckValidationsOnGeneralDailyProcessCancel(GeneralDailyProcessDetails processDetails);
        int ValidateGeneralProcess(short PROCESS_KUBUN_CD);
        public GeneralDailyProcessDetails GetChubbanDetailsForWeight(string PP_CHUUBAN);
        public int GetNextThabaForSpecialChuubanNo(string orderNo);
        public GeneralDailyProcessDetails GetGeneralProcessDataForCancel(GeneralDailyProcessDetails processDetails);
        public GeneralDailyProcessDetails GetProcessPlanData(GeneralDailyProcessDetails processDetails);
        List<NameValue> GetMachineTypeList(int machineTypeCd);
       public GeneralDailyProcessDetails ValidateChubbanForProcessStart(string PP_CHUUBAN);
        public string ValidateChubbanForProcessEnd(string PP_CHUUBAN);
        public string CheckExistingDataValidation(GeneralDailyProcessDetails processDetails);

    }
}
