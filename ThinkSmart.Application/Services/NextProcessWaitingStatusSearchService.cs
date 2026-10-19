using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class NextProcessWaitingStatusSearchService: INextProcessWaitingStatusSearchService
    {
        public readonly INextProcessWaitingStatusSearchRepository _nextProcessWaitingStatusSearchRepository;

        private readonly LocalizationServices _localization;
        public NextProcessWaitingStatusSearchService(INextProcessWaitingStatusSearchRepository nextProcessWaitingStatusSearchRepository, LocalizationServices localization)
        {
            _localization = localization;
            _nextProcessWaitingStatusSearchRepository = nextProcessWaitingStatusSearchRepository;
        }

        public List<NameValue> GetProcessList()
        {
            var _nextProcessList = UIUtils.GetNextProcesKubun();
            return _nextProcessList;
        }
        public List<WipStatusDetails> GetNextProcessWaitingStatusSearch(GridSearchFields gridSearchRequest)
        {

             List<SearchField> searchFields = ComposeSearchCriteriaforNextProcess(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var obj = _nextProcessWaitingStatusSearchRepository.GetNextProcessWaitingStatusSearch(searchFields, selectedSortCriteria, gridSearchRequest);

           
          
            return obj;
        }
        private List<SearchField> ComposeSearchCriteriaforNextProcess(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.LOCATION_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.LOCATION_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.LOCATION_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.PROCESS_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "WIP.NEXT_PROCESS",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROCESS_CD)
                };
                searchFields.Add(searchField);
            }         
           
            return searchFields;
        }

        public List<NameValue> GetLocationNameList()
        {
            var locationNameList = _nextProcessWaitingStatusSearchRepository.GetLocationNameList();
            return locationNameList;
        }

    }
}
