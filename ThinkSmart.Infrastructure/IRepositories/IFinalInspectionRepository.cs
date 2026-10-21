using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IFinalInspectionRepository
    {
        List<FinalInspectionDetails> GetFinalInspectionDataOnFactoryDailyWorkStatus(List<SearchField> searchFields, string sortCriteria, int rowLimit);
        List<FinalInspectionDetails> GetFinalInspectionDataOnDailyProcessStatus(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridresult);
        int GetRowCountForFinalInspectionFactory(List<SearchField> searchFields);
        int GetRowCountForFinalInspectionDailyAnalysis(List<SearchField> searchFields);
        List<FinalInspectionDetails> GetFinalInspectDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isPrintByPrintFlag, int userID);

        List<FinalInspectionDetails> GetStartChuuban(string startDate, string endDate, string printFlag, int isPrintByPrintFlag);

        int GetFinishProductDetails(string startDate, string endDate, string endOperator, int printFlag, int userID); 
        int GetSimpleFinalInspectionDetails(string startDate, string endDate, string endOperator, int printFlag, int userID);
    }
}
