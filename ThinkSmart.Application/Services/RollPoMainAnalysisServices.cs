using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;
using TSWebApplication.Infrastructure.IRepositories;
using System.Transactions;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class RollPoMainAnalysisServices :IRollPoMainAnalysisServices
    {
        private readonly IRollPoMainAnalysisRepository _rollPoMainAnalysisRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public RollPoMainAnalysisServices(IRollPoMainAnalysisRepository rollPoMainAnalysisRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _rollPoMainAnalysisRepository = rollPoMainAnalysisRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
       
       
        public List<NameValue> GetMaterialReply()
        {
            var _UnitTypeList = UIUtils.GetMaterialReplyForcast();
            return _UnitTypeList;
        }

        public int IsBatchRunSuccess(RollPoMainAnalysisDetails details)
        {
            int count = _rollPoMainAnalysisRepository.IsBatchRunSuccess(details);
            return count;
        }

        public string ExecuteMaterialForecastBatch(GridSearchFields details)
        {
            var obj = "";
            //using (TransactionScope transactionScope = new TransactionScope())
            //{
                 obj = _rollPoMainAnalysisRepository.ExecuteMaterialForecastBatch(details);
               // transactionScope.Complete();
           // }

            return obj;
        }


        public RollPoMainAnalysisDetails GetForecastRoll(DateTime currentDate)
        {
            DateTime CurrentForecastDate;
            RollPoMainAnalysisDetails result = new RollPoMainAnalysisDetails();
            CurrentForecastDate = currentDate.AddMonths(1);
            result.ROLL_R = CurrentForecastDate.Month;
            result.ROLL_YEAR = CurrentForecastDate.Year;
            return result;
        }
        public void ImportBatchDataFromHost()
        {
            _rollPoMainAnalysisRepository.ImportBatchData();
        }

        public string GetLocationNameByEmployeeCD(int employeeCD)
        {
           var obj= _rollPoMainAnalysisRepository.GetLocationNameByEmployeeCD(employeeCD);
            return obj;
        }

        public List<RollPoMainAnalysisDetails> GetRollPOMainAnalysisGridData(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            short? chUnconfirmed = gridSearchRequest.UNCONFIRMED;
            int roll_Year=gridSearchRequest.ROLL_YEAR;
            int roll_R=gridSearchRequest.ROLL_R;
            var obj = _rollPoMainAnalysisRepository.GetRollPOMainAnalysisGridData(searchFields, chUnconfirmed, selectedSortCriteria,roll_Year,roll_R);
            return obj;
        }
        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
           

            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INTERNAL_NAME_STRING",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING
                };
                searchFields.Add(searchField);
            }
            
            if (gridSearchRequest.DIM_D > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.DIM_D)
                    

                };
                searchFields.Add(searchField);
            }
           

            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL_KUBUN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_TYPE_CBK)
                };
                searchFields.Add(searchField);
            }


            return searchFields;
        }
        public List<NameValue> GetMaterialTypeList()
        {
            var _MaterialTypeList = UIUtils.GetMaterialTypeList();
            return _MaterialTypeList;
        }

        public int PlaceOrder(GridSearchFields details)
        {
            short inputLocationCD = Convert.ToInt16(_userLoginRepository.GetUserLocationById(Convert.ToInt32(details.USER_ID)));

            int poSuccessFlag = _rollPoMainAnalysisRepository.PlaceOrder(details, inputLocationCD);
            return poSuccessFlag;
        }

        public string SetAnalysisOverForFcSummaryWithNoFC()
        {  
           var obj = _rollPoMainAnalysisRepository.SetAnalysisOverForFcSummaryWithNoFC();               
           return obj;
        }

        public int IsPODone(DateTimeDetails details)
        {
            int count = _rollPoMainAnalysisRepository.IsPODone(details);
            return count;
        }
        public int IsAllAnalysisIsOver()
        {
            int count = _rollPoMainAnalysisRepository.IsAllAnalysisIsOver();
            return count;
        }

        public int CheckMatchingYearRRecords(RollPoMainAnalysisDetails details)
        {
            int count = _rollPoMainAnalysisRepository.CheckMatchingYearRRecords(details);
            return count;
        }

        public int CheckForecastLocked(RollPoMainAnalysisDetails details)
        {
            int count = _rollPoMainAnalysisRepository.CheckForecastLocked(details);
            return count;
        }

        public int GetEmployeeCd()
        {
            int employeeCd = _rollPoMainAnalysisRepository.GetEmployeeCd();
            return employeeCd;
        }
    }
}
