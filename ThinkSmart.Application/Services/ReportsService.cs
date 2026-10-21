using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;
using static System.Formats.Asn1.AsnWriter;

namespace TSWebApplication.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        public ReportsService(IReportsRepository reportsRepository, IUserLoginRepository userLoginRepository)
        {
            _userLoginRepository = userLoginRepository;
            _reportsRepository = reportsRepository;
        }

        public List<FinalInspectionDetails> LoadFinalInspectionDetails(string startDate, string endDate, string endOperator, int orderBy, string printFlag, int isPrintByPrintFlag, int userID)
        {
            using(TransactionScope _transaction = new TransactionScope())
            {
                var finalInspectionDetails = _reportsRepository.GetFinalInspectDetails(startDate, endDate, endOperator, orderBy, printFlag, isPrintByPrintFlag, userID);
                finalInspectionDetails[0].START_DATE = startDate;
                finalInspectionDetails[0].END_DATE = endDate;
                finalInspectionDetails[0].USER_ID = userID;
                finalInspectionDetails[0].OPERATOR_ID = Convert.ToInt32(endOperator);
                var getResult = _reportsRepository.GetStartEndChuubanAndThaba(finalInspectionDetails[0]);

                if (getResult.Count > 0)
                {
                    finalInspectionDetails[0].CHUUBAN_LIST = getResult;
                }

                List<int> decisionList = new List<int>();
                foreach (var item in finalInspectionDetails)
                {
                  
                    decisionList.Clear();
                   
                    decisionList.Add(Convert.ToInt16(item.TS_DECISION));
                    decisionList.Add(Convert.ToInt16(item.HD_DECISION));

                    // Determine overall result
                   
                    if (decisionList?.Count > 0)
                    {
                        int[] priorityOrder = { 20, 30, 35, 40, 60, 65, 70, 73, 76, 79, 80, 90, 10 };

                        foreach (var code in priorityOrder)
                        {
                            if (decisionList.Contains(code))
                            {
                                var result = UIUtils.GetResultKubun().FirstOrDefault(x => x.CD == code);
                                item.MACHINABILITY_VALUE = Convert.ToString(result.Name);
                              
                                break; // STOP at first match (this is the key)
                            }
                        }
                    }
                }
               
                _transaction.Complete();
                return finalInspectionDetails;
            }
            

           var chubbanThabaDetails = _reportsRepository.GetStartChuuban(startDate, endDate, printFlag, isPrintByPrintFlag);

            return chubbanThabaDetails;
        }

        public List<HikinukiDetails> GetHikinukiList(GridSearchFields gridSearchRequest)
        {
            int locationCD = _userLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID));
            gridSearchRequest.INPUT_LOCATION_CD =Convert.ToInt16(locationCD);
            var result = _reportsRepository.GetHikinukiData(gridSearchRequest);
          
            return result;
        }
        public List<FinalInspectionDetails> GetFinishProductData(FinalInspectionDetails gridSearchRequest)
        {
            List<FinalInspectionDetails> result = new List<FinalInspectionDetails>();
            using(TransactionScope _transaction = new TransactionScope())
            {
                result = _reportsRepository.GetFinishProductDetails(gridSearchRequest);
                //if (result.Count > 0)
                //{
                //    _reportsRepository.UpdatePrintFlag(result);
                //}
                var getResult = _reportsRepository.GetStartEndChuuban(gridSearchRequest);
                if (getResult.Count > 0)
                {
                    result[0].CHUUBAN_LIST = getResult;
                }
                _transaction.Complete();
            }
           
            return result;
        }

        public List<FinalInspectionDetails> GetSimpleFinalInspectionDetails(FinalInspectionDetails gridSearchRequest)
        {
            List<FinalInspectionDetails> result = new List<FinalInspectionDetails>();
            TransactionOptions options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(10) // Set timeout to 10 minutes
            };

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required,options))
            {
                if (gridSearchRequest.PRINT_FLAG == 1)  
                {
                    result = _reportsRepository.GetSimpleFinalInspectionDetails(gridSearchRequest);

                    if (result.Any())
                    {
                        _reportsRepository.UpdatePrintFlag(result);//Unprinted checkbox is checked, then update only records
                    }
                }
                else
                {
                    result = _reportsRepository.GetSimpleFinalInspectionDetails(gridSearchRequest);
                }
                
                _transaction.Complete();
            }
            return result;
        }
        public List<WipStatusDetails> GetWIPStatusList(int userID)
        {
            int locationCD = _userLoginRepository.GetUserLocationById(userID);
            return  _reportsRepository.GetWIPStatusData(userID, locationCD);
        }
        public List<WIPTensouDetails> GetWIPTensouAcceptanceDetails(string startDate, string endDate, int userID)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            int locationCD = _userLoginRepository.GetUserLocationById(userID);
            result = _reportsRepository.GetWIPTensouAceeptanceDetailsbyDateLocation(startDate, endDate, locationCD);
            return result;
        }
        public List<WIPTensouDetails> GetWIPTensouRequestDetails(string startDate, int userID)
        {
            int locationCD = _userLoginRepository.GetUserLocationById(userID);
            return _reportsRepository.GetWIPTensouRequestDetailsbyDateLocation(startDate, locationCD);
        }
        public List<RollPoMainAnalysisDetails> GetPlaceOrderList(int userID, int yearR)
        {           
            return _reportsRepository.GetPlaceOrderList(userID,yearR);
        }

        public List<RollPoMainAnalysisDetails> GetMalNotFoundList(int userID, int year, int R)
        {
            return _reportsRepository.GetMalNotFoundList(userID, year, R);
        }
        public List<RollPoMainAnalysisDetails> GetMalNameIntWiseData(int year, int R)
        {
            return _reportsRepository.GetMalNameIntWiseData( year,  R);
        }
        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID)
        {
            //runID = _runId;

            List<MakerMillSheetEDIDetails> allData = _reportsRepository.GetMakerMillSheetProcessRptData(runID);

            return allData;

        }

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetErrorRptData(int runID)
        {
            //runID = _runId;
            List<MakerMillSheetEDIDetails> errorData = _reportsRepository.GetMakerMillSheetErrorRptData(runID);

            return errorData;
        }
        public List<MakerMillSheetEDIDetails> GetEDIErrorReportData(int runCD)
        {
            return _reportsRepository.GetEDIErrorReportData(runCD);
        }

        public List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchReport(int userID, DateTime processDateTime, int printFlag)
        {
            return _reportsRepository.GetDaidoFutouNyukoBatchReport(userID, processDateTime, printFlag);
        }

        public List<DaidoFutouNyukoDetails> GetDaidoFutouNyukoBatchErrorReport(int userID, DateTime processDateTime, int printFlag)
        {
            return _reportsRepository.GetDaidoFutouNyukoBatchErrorReport(userID, processDateTime, printFlag);
        }
    }
}
