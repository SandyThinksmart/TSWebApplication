using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IHeatTreatmentService
    {
        List<HeatTreatMentDetails> GetHeatTreatmentStartDetails(int? machineCD, bool isNull);
        List<HeatTreatMentDetails> GetHeatTreatmentEditDeleteStartEndDetails(int? HeatTreatMentCd, bool isNull);
        
        List<HeatTreatMentDetails> GetFurnaceNo(string furnaceNo);
        List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty);
        List<NameValue> GetProcConditionKubun();
        List<NameValue> GetMachineNameList();
        List<NameValue> GetHeatTreatmentMachineNameList();
        
        List<NameValue> GetMaterialKubun();
        List<NameValue> GetUnitKubunList();
        HeatTreatMentDetails GetChuubanListForInvaidCheck(string enteredCChubaans);
        List<string> GetCompletedChuubanList(string enteredCChubaans);
        List<HeatTreatMentDetails> SetList(List<string> chubaanNos);
        bool IsFurProcFurNoIsExist(int machineCD, string furnaceNo);
        List<HeatTreatMentDetails> SaveStartData(List<HeatTreatMentDetails> reuest);       
        long UpdateEndData(List<HeatTreatMentDetails> heatTreatMentDetails);
        bool IsMachineAlreadyStarted(int machineCD);
        void Delete(List<HeatTreatMentDetails> request);
        List<HeatTreatMentDetails> GetHeatTreatmentList(GridSearchFields searchRequest);
       
        List<NameValue> GetProductUnitKubun();
        List<NameValue> GetMatlType();
        List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        List<HeatTreatMentDetails> GetHeatTreatmentDetailsOnDailyProcessAnalysis(GridSearchFields gridresult);
        int GetMaxHTNumber();
        int GetRowCountForHeatTreatmentFactory(GridSearchFields gridresult);

        int GetRowCountForHeatTreatmentDailyAnalysis(GridSearchFields gridresult);

       
        List<NameValue> GetLocationNameList();
        List<NameValue> GetHeatTreatmentMachineList();
    }
}
