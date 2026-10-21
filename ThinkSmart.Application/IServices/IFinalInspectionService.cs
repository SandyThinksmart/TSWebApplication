using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Application.IServices
{
    public interface IFinalInspectionService
    {
        List<FinalInspectionDetails> GetFinalInspectionDataOnFactoryDailyWorkStatus(GridSearchFields gridSearchRequest);
        List<FinalInspectionDetails> GetFinalInspectionDataOnDailyProcessStatus(GridSearchFields gridSearchRequest);
        int GetRowCountForFinalInspectionFactory(GridSearchFields gridresult);
        int GetRowCountForFinalInspectionDailyAnalysis(GridSearchFields gridresult);
        List<NameValue> GetLocation(int USER_ID);
        List<FinalInspectionDetails> LoadFinalInspectionDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isPrintByPrintFlag, int userID);

        int GetFinishProductDetails (string startDate, string endDate, string endOperator,int printFlag, int userID); 

        int GetSimpleFinalInspectionDetails(string startDate, string endDate, string endOperator, int printFlag, int userID);

    }
}
