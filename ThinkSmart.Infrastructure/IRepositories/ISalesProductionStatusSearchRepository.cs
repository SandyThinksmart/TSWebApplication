using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface ISalesProductionStatusSearchRepository
    {
        List<SalesProductionStatusSearchDetails> GetSalesProductionStatusList(List<SearchField> searchFields, string sortCiteria, short? locationCD);
        ProcessPlanForSalesProductionStatusDetails GetSalesStatusDetailsByProcessPlanCD(long processPlanCD);
        ProcessPlanForSalesProductionStatusDetails GetTimeAndBundlewiseDetailsByProcessPlanCD(long processPlanCD);
        List<ProcessPlanForSalesProductionStatusDetails> GetProcessDataDetailsByProcessPlanCD(long processPlanCD); 
        List<ProcessPlanForSalesProductionStatusDetails> GetHardnessAndPostHTDetailsByProcessPlanCD(long processPlanCD);
        List<ProcessPlanForSalesProductionStatusDetails> GetMessageListDetailsByProcessPlanCD(long processPlanCD);
        ProcessPlanForSalesProductionStatusDetails GetFIFailureRateDetailsByProcessPlanCD(long processPlanCD);
        ProcessPlanForSalesProductionStatusDetails GetECDefectRateDetailsByProcessPlanCD(long processPlanCD);
        ProcessPlanForSalesProductionStatusDetails GetDrawingDefectRateDetailsByProcessPlanCD(long processPlanCD);
        ProcessPlanForSalesProductionStatusDetails GetWIPDetailsByProcessPlanCD(long processPlanCD);
        void MessageDetailsInsert(ProcessPlanForSalesProductionStatusDetails request);
        void MessageDetailsUpdate(ProcessPlanForSalesProductionStatusDetails request);
        void MessageDetailsDelete(long MSG_CD);

    }
}
