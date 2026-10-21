using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IYieldSearchService
    {

        List<ProcessPlanForYieldSearchDetails> GetYieldSearchList(GridSearchFields gridSearchRequest);
        List<ProcessPlanForYieldSearchDetails> UpdateECYieldValue(List<ProcessPlanForYieldSearchDetails> yieldDetails);
        decimal? CalculateECYield(long processPlanCd);
        //ProcessPlanForSalesProductionStatusDetails getSalesProdStatusByProcessPlanCD(long processPlanCD, string cultureName);
        //ProcessPlanForSalesProductionStatusDetails getTimeAndBundlewiseByProcessPlanCD(long processPlanCD, string cultureName);
        //List<ProcessPlanForSalesProductionStatusDetails> getProcessDataByProcessPlanCD(long processPlanCD);
        //List<ProcessPlanForSalesProductionStatusDetails> getHardnessAndPostHTByProcessPlanCD(long processPlanCD);
        //List<ProcessPlanForSalesProductionStatusDetails> getMessageDetailsByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails getFIFailureRateByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails getECDefectRateByProcessPlanCD(long processPlanCD);
        //ProcessPlanForSalesProductionStatusDetails getWIPByProcessPlanCD(long processPlanCD);
        //void MessageDetailsSave(ProcessPlanForSalesProductionStatusDetails request);
    }
}
