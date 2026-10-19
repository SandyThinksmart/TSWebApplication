using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IHeatTreatmentRepository
    {
        List<HeatTreatMentDetails> GetHeatTreatmentDetails(int? MACHINE_CD, bool isNull);
        List<HeatTreatMentDetails> GetHeatTreatmentEditDeleteStartEndDetails(int? HeatTreatMentCd, bool isNull);
        
        List<HeatTreatMentDetails> GetFurnaceNoDetails(string FURNACE_NO);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        List<NameValue> GetMachineNameList();
        List<NameValue> GetHeatTreatmentMachineNameList();
        
        HeatTreatMentDetails GetChuubanListForInvaidCheck(string chuubanList);
        List<string> GetCompletedChuubanList(string chuubanList);
        List<ProductOrderCompletion> GetSearchedProductOrderDetails(List<string> chubanList, int completeFlag);
        bool IsFurProcFurNoIsExist(int MACHINE_CD, string FURNACE_NO);
        List<HeatTreatMentDetails> InsertStartData(HeatTreatMentDetails heatTreatMentDetails);        
        long UpdateEndData(HeatTreatMentDetails heatTreatMentDetails);
        void Delete(int MACHINE_CD, string FURNACE_NO);
        List<HeatTreatMentDetails> GetHeatTreatmentDetails(string sortCriteria, List<SearchField> searchCriteria, short locationCD);
        List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria,int rowLimit);
        List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);
        int GetHTNumDetails();
        bool IsMachineStarted(int machineCD);

        int GetRowCountForHeatTreatmentFactory(List<SearchField> searchFields);
        int GetRowCountForHeatTreatmentDailyAnalysis(List<SearchField> searchFields);
        List<NameValue> GetLocationNameList();
        List<NameValue> GetFactoryLocationNameList();
        List<NameValue> GetHeatTreatmentMachineList();
    }
}
