using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IReportsRepository
    {
        List<FinalInspectionDetails> GetFinalInspectDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isPrintByPrintFlag, int userID);
        List<FinalInspectionDetails> GetStartChuuban(string startDate, string endDate, string printFlag, int isPrintByPrintFlag);
        List<HikinukiDetails> GetHikinukiData(GridSearchFields gridSearchRequest);
        List<FinalInspectionDetails> GetFinishProductDetails(FinalInspectionDetails fields);
        List<string> GetStartEndChuuban(FinalInspectionDetails fields);
        List<WipStatusDetails> GetWIPStatusData(int userId, int locationCd);
        List<WIPTensouDetails> GetWIPTensouAceeptanceDetailsbyDateLocation(string startdate, string enddate, int locationCD);
        List<WIPTensouDetails> GetWIPTensouRequestDetailsbyDateLocation(string date, int locationCD);
        List<RollPoMainAnalysisDetails> GetPlaceOrderList(int userId, int yearR);
        void UpdatePrintFlag(List<FinalInspectionDetails> request);
        List<RollPoMainAnalysisDetails> GetMalNotFoundList(int userId, int year, int R);
        List<RollPoMainAnalysisDetails> GetMalNameIntWiseData(int year, int R); 
        List<FinalInspectionDetails> GetSimpleFinalInspectionDetails(FinalInspectionDetails fields);
        List<string> GetStartEndChuubanAndThaba(FinalInspectionDetails fields);
        List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID);
        List<MakerMillSheetEDIDetails> GetMakerMillSheetErrorRptData(int runID);
        List<MakerMillSheetEDIDetails> GetEDIErrorReportData(int runCD);
        List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchReport(int userID, DateTime processDateTime, int printFlag);

        List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchErrorReport(int userId, DateTime processDateTime,int printFlag);
    }
}
