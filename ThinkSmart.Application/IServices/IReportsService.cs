using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IReportsService
    {
        List<FinalInspectionDetails> LoadFinalInspectionDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isPrintByPrintFlag, int userID);
        List<HikinukiDetails> GetHikinukiList(GridSearchFields gridSearchRequest);
        List<FinalInspectionDetails> GetFinishProductData(FinalInspectionDetails gridSearchRequest);
        List<WipStatusDetails> GetWIPStatusList(int userID);
        List<WIPTensouDetails> GetWIPTensouAcceptanceDetails(string startDate, string endDate, int userID);
        List<WIPTensouDetails> GetWIPTensouRequestDetails(string startDate, int userID);

        List<RollPoMainAnalysisDetails> GetPlaceOrderList(int userID, int yearR);
        List<RollPoMainAnalysisDetails> GetMalNotFoundList(int userID, int year,int R);
        List<RollPoMainAnalysisDetails> GetMalNameIntWiseData(int year , int R); 
        List<FinalInspectionDetails> GetSimpleFinalInspectionDetails(FinalInspectionDetails gridSearchRequest);
        List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID);
        List<MakerMillSheetEDIDetails> GetMakerMillSheetErrorRptData(int runID);
        List<MakerMillSheetEDIDetails> GetEDIErrorReportData(int runCD);
        List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchReport(int userID, DateTime processDateTime,int printFlag);

        List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchErrorReport(int userID, DateTime processDateTime, int printFlag);
    }
}
