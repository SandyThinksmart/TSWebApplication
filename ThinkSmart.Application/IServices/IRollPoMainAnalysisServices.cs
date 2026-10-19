using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IRollPoMainAnalysisServices
    {
        List<NameValue> GetMaterialReply();
        int IsBatchRunSuccess(RollPoMainAnalysisDetails details);
        public string ExecuteMaterialForecastBatch(GridSearchFields details);

        RollPoMainAnalysisDetails GetForecastRoll(DateTime dateTime);
        void ImportBatchDataFromHost();

        public string GetLocationNameByEmployeeCD(int employeeCD);
        List<RollPoMainAnalysisDetails> GetRollPOMainAnalysisGridData(GridSearchFields gridresult);
        List<NameValue> GetMaterialTypeList();
        public int PlaceOrder(GridSearchFields details);

        public string SetAnalysisOverForFcSummaryWithNoFC();

        int IsPODone(DateTimeDetails details);
        int IsAllAnalysisIsOver();

        int CheckMatchingYearRRecords(RollPoMainAnalysisDetails details);
        int CheckForecastLocked(RollPoMainAnalysisDetails details);

        int GetEmployeeCd();

    }
}
