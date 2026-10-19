using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IYieldSearchRepository
    {
        List<ProcessPlanForYieldSearchDetails> GetYieldSearchList(List<SearchField> searchFields, string sortCiteria, short? locationCD);
        DataTable LoadECValForECPercentData(long processPlanCd);
        DataTable GetDWValForECPercent(long processPlanCd);
        //ProcessPlanForSalesProductionStatusDetails GetSalesStatusDetailsByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails GetTimeAndBundlewiseDetailsByProcessPlanCD(long processPlanCD);
        //List<ProcessPlanForSalesProductionStatusDetails> GetProcessDataDetailsByProcessPlanCD(long processPlanCD);
        //List<ProcessPlanForSalesProductionStatusDetails> GetHardnessAndPostHTDetailsByProcessPlanCD(long processPlanCD);
        //List<ProcessPlanForSalesProductionStatusDetails> GetMessageListDetailsByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails GetFIFailureRateDetailsByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails GetECDefectRateDetailsByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails GetWIPDetailsByProcessPlanCD(long processPlanCD);
        //void MessageDetailsInsert(ProcessPlanForSalesProductionStatusDetails request);
    }
}
