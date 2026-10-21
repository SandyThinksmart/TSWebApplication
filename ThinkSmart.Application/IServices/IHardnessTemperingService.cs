using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IHardnessTemperingService
    {
        HardeningTemperingDetails GetHardnessTemperingDetailsByChuubanGyoban(string chuuban, short? gyouban);
        void InsertHardnessTemperingDetails(HardeningTemperingDetails details);
        long UpdateHardnessTemperingDetails(HardeningTemperingDetails details);
        void DeleteHardnessTemperingDetails(HardeningTemperingDetails request);
        HardeningTemperingDetails GetHardnessTemperingDetails(string furnaceNo, string chuuban, short? gyouban, int userid);
        void InsertHardnessTemperingTempDetails(List<HardeningTemperingDetails> details);
        List<HardeningTemperingDetails> GetHardnessTemperingList(GridSearchFields request);
        HardeningTemperingDetails GetHardnessTemperingDetailsByHardnessTemperingCD(long hardnessTemperingCD);
        public List<HardeningTemperingDetails> GetHardnessTempringOnFactoryDailyWorkStatus(GridSearchFields gridresult);
        List<HardeningTemperingDetails> GetHardnessTemperingDetailsOnDailyProcessAnalysis(GridSearchFields gridresult);
        List<NameValue> GetNextProcessList();
        List<NameValue> GetDecisionList();
        List<NameValue> GetTempTypeList();
        List<NameValue> GetTextureTypeList();
        int GetFurnaceDetails(string furnaceNo);
        HardeningTemperingDetails FurnaceIsAlreadyEntered(string furnaceNo);
        void InsertHTQualityDetails(HardeningTemperingDetails details);
        List<HardeningTemperingDetails> GetHTQualityList(GridSearchFields searchRequest);
        List<HardeningTemperingDetails> GetHardnessTemperingTempDetailsByUser(int userID);
        HardeningTemperingDetails GetHTQualityDetailsByCD(long HTQualityCD);
        long UpdateHTQualityDetails(HardeningTemperingDetails details);
        void DeleteHTQualityDetails(long htQuality, long? msgCD);        
        int GetRowCountForHardnessTemperingtFactory(GridSearchFields gridresult);
        int GetRowCountForHardnessTemperingDailyAnalysis(GridSearchFields gridresult);
        List<ThabaDetails> GetThabaDetailsByProcessPlan(int processPlanCD);
        bool IsProcessPlanAlreadyExist(GridSearchFields request);
    }
}
