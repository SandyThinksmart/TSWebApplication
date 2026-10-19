using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IRollPoForecastService
    {
        List<GridSearchFields> GetProductMasterDataList(GridSearchFields gridSearchRequest);
        DateTimeDetails GetPreviousForecastRoll(DateTime dateTime);
        //GridSearchFields GetProductDetailsByProductCD(long PRODUCT_CD);
        RollSalesForecastDetails GetProductDetailsByProductCD(long PRODUCT_CD);
        RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD);
        List<GridSearchFields> GetMaterialMasterDataList(GridSearchFields gridSearchRequest);
        List<NameValue> GetMaterialTypeList();
        List<NameValue> GetMatlSpecNameList();

        List<RollSalesForecastDetails> GetSpecInput1DetailsByCustInchrge_CD(int cust_Cd, int incharge_Cd,int current_R,int current_YYYY,decimal? HISTORY_MONTH1, decimal? HISTORY_MONTH2, decimal? HISTORY_MONTH3);
        //List<RollSalesForecastDetails> GetSpecInput2DetailsByCustInchrge_CD(int cust_Cd, int incharge_Cd, int? current_R, int? current_YYYY);
        List<RollSalesForecastDetails> GetSpecInput1MaterialDetailsByCustInchrge_CD(int cust_Cd, int incharge_Cd, int? current_R, int? current_YYYY);
        RollSalesForecastDetails SaveProductMaterialForecastDetails(RollSalesForecastDetails request); 
        //List<RollSalesForecastDetails> GetProductMaterialForecastDetails(int? customerId, int? forecastYear, int? forecastRollR);
        List<RollSalesForecastDetails> GetProductForecastDetails(GridSearchFields searchRequest);
        List<RollSalesForecastDetails> GetMaterialForecastDetails(GridSearchFields searchRequest);

    }
}
