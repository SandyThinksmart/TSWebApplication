using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IRollPoForecastRepository
    {
        List<GridSearchFields> GetProductMasterDataList(List<SearchField> searchFields, string sortCriteria);
        int IsBatchRunSuccess(int next_R, int next_YYYY);
        int IsForecastDataPresent(int last_R, int last_YYYY);
        int IsPreviousRForecastLocked(int last_R, int last_YYYY);
        //GridSearchFields GetProductDetailsByProductCD(long PRODUCT_CD);
        RollSalesForecastDetails GetProductDetailsByProductCD(long PRODUCT_CD);

        List<GridSearchFields> GetMaterialMasterDataList(List<SearchField> searchFields, string sortCriteria);
        RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD);
        List<RollSalesForecastDetails>  GetSpecInput1Details(int cust_CD,int incharge_Cd, int? current_R, int? current_YYYY,decimal? history_Month1,decimal? history_Month2,decimal? history_Month3);
        List<RollSalesForecastDetails> GetSpecInput2Details(int cust_CD, int incharge_Cd, int? current_R, int? current_YYYY);
        int IsForecastDataPresentCustomerWise(int CustCD,int last_R, int last_YYYY);
        List<RollSalesForecastDetails> GetSpecInput1MaterailaDetails(int cust_CD, int incharge_Cd, int? current_R, int? current_YYYY);

        RollSalesForecastDetails SaveProductMaterialForecastDetails(RollSalesForecastDetails data);
        //List<RollSalesForecastDetails> GetProductMaterialForecastDetails(int? customerId, int? forecastYear, int? forecastRollR);
        List<RollSalesForecastDetails> GetProductForecastDetails(List<SearchField> searchCriteria,string sortCriteria);

        List<RollSalesForecastDetails> GetMaterialForecastDetails(List<SearchField> searchCriteria, string sortCriteria);
    }
}
                                                                                                                                                                                                                                                                                                                                                  