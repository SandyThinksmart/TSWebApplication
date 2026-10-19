using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IHardnessTemperingRepository
    {
        HardeningTemperingDetails GetHardnessTemperingDetailsByChuubanGyoban(string chuuban, short? gyouban);
        void InsertHardnessTemperingDetails(HardeningTemperingDetails details);
        long UpdateHardnessTemperingDetails(HardeningTemperingDetails details);
        void DeleteHardnessTemperingDetails(long hardening_tempering_cd);
        void InsertHardnessTemperingTempDetails(HardeningTemperingDetails details);
        List<HardeningTemperingDetails> GetHardnessTemperingList(List<SearchField> searchFields, string sortCiteria, int locationCD);
        HardeningTemperingDetails GetHardnessTemperingDetailsByHardnessTemperingCD(long hardnessTemperingCD);
        int GetUserLocationById(int userID);
        int GetFurnaceNumber(string furnaceNo);
        public List<HardeningTemperingDetails> GetHardnessTempringOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCiteria, int rowLimit);
        List<HardeningTemperingDetails> GetHardnessTemperingDetailsOnDailyProcessAnalysis(List<SearchField> searchFields, string sortCiteria, GridSearchFields gridresult);
        HardeningTemperingDetails GetHardnessTemperingDetails(string furnaceNo, string chuuban, short? gyouban, int inputLocationCD);
        HardeningTemperingDetails FurnaceIsAlreadyEntered(string furnaceNo);
        void InsertHTQualityDetails(HardeningTemperingDetails details);
        List<HardeningTemperingDetails> GetHTQualityList(List<SearchField> searchFields, string sortCiteria, int LocationCD);
        List<HardeningTemperingDetails> GetHardnessTemperingTempDetailsByUser(int userID);
        HardeningTemperingDetails GetHTQualityDetailsByCD(long HTQualityCD);
        long UpdateHTQualityDetails(HardeningTemperingDetails details);
        void DeleteHTQualityDetails(long htQualityCD);
        int GetRowCountForHardnessTemperingtFactory(List<SearchField> searchFields);
        int GetRowCountForHardnessTemperingDailyAnalysis(List<SearchField> searchFields);
        List<ThabaDetails> GetThabaDetails(int processPlanCD);
        bool IsProcessPlanSaved(GridSearchFields request);
    }
}
