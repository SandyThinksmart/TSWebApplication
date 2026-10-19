using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IRollPoMainAnalysisRepository
    {
        int IsBatchRunSuccess(RollPoMainAnalysisDetails details);
        public string ExecuteMaterialForecastBatch(GridSearchFields details);

        RollPoMainAnalysisDetails GetForecastRoll();
        void ImportBatchData();

        public string GetLocationNameByEmployeeCD(int employeeCD);

        List<RollPoMainAnalysisDetails> GetRollPOMainAnalysisGridData(List<SearchField> searchFields,short? chUnconfirmed, string sortCiteria,int roll_Year,int roll_R);
        public int PlaceOrder(GridSearchFields details, short inputLocationCD);

        public string SetAnalysisOverForFcSummaryWithNoFC();

        int IsPODone(DateTimeDetails details);

        int IsAllAnalysisIsOver();

        int CheckMatchingYearRRecords(RollPoMainAnalysisDetails details);
        int CheckForecastLocked(RollPoMainAnalysisDetails details);
        int GetEmployeeCd();


    }
}
