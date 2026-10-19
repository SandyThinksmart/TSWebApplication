using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IAnalysePOQuantityRepository
    {
        List<NameValue> GetMukesakiList();
        List<NameValue> GetSupplierList();
        List<NameValue> GetDestinationList();
        List<NameValue> GetHikitorisakiList();
        List<NameValue> GetFutouList();
        public string SaveMaterialPoData(List<RollPoMainAnalysisDetails> PODetails);
        List<RollPoMainAnalysisDetails> GetInternalNameWiseMakerList(long INT_MATL_FC_SUMMARY_CD);
        RollPoMainAnalysisDetails GetInternalNameWIseMaterialDetails(long INT_MATL_FC_SUMMARY_CD);
        List<GridSearchFields> GetMaterialMasterDataList(List<SearchField> searchFields, string sortCriteria, long INT_MATL_FC_SUMMARY_CD);
        RollSalesForecastDetails GetMakerMaterialDetailsByCD(long INT_MATL_FC_SUMMARY_CD, long MATERIAL_CD, short MUKESAKI_CD);
        List<RollPoMainAnalysisDetails> GetRollPoDetails(long INT_MATL_FC_SUMMARY_CD);
        RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD, short? MUKESAKI_CD);
        public int checkAnalysisOverFlag(long INT_MATL_FC_SUMMARY_CD);
        public int checkPoCompleteFlag();
        public float GetMukesakiMaterialRate(short mukesakiCd, decimal matlDimD, int matlKubun, int matlNameMakerCd);
        public string UpdateForecastData(List<RollSalesForecastDetails> forecastDetails);
        
    }
}
