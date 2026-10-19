using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class MakerMaterialService : IMakerMaterialService
    {
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IMakerMaterialRepository _makerMaterialRepository;

        public MakerMaterialService(IMakerMaterialRepository makerMaterialRepository, IAllCommonRepository commonRepository)
        {
            _makerMaterialRepository = makerMaterialRepository;
            _CommonRepository = commonRepository;
                 
        }
        public List<NameValue> GetMaterialTypeList()
        {
            var _conditionList = UIUtils.GetMatlList();
            return _conditionList;
        }
        public List<NameValue> GetMakersList()
        {
            return _makerMaterialRepository.GetMakersList();
        }
        public List<NameValue> GetMukesakiList(string yobina)
        {
            return _makerMaterialRepository.GetMukesakiList(yobina);
        }
        public List<MakerMaterialDetails> GetMaterialDetailsList(MakerMaterialDetails details)
        {
            return _makerMaterialRepository.GetMaterialList(details);
        }
        public List<MakerMaterialDetails> SaveMaterialDetails(List<MakerMaterialDetails> details)
        {
            return _makerMaterialRepository.SaveMaterialDetails(details);
        }
        public List<MakerMaterialDetails> UpdateMaterialDetails(List<MakerMaterialDetails> details)
        {
           return _makerMaterialRepository.UpdateMaterialDetails(details);
        }
        public void DeleteMaterialDetails(int groupNo)
        {
            _makerMaterialRepository.DeleteMaterialDetails(groupNo);
        }
        public List<MakerMaterialDetails> GetListPageData(MakerMaterialDetails details)
        {
            List<SearchField> searchFields = ComposeSearchCriteria(details);
            string selectedSortCriteria = Helper.ParseSortCriteria(details.SORT_FIELDS);
        
            return _makerMaterialRepository.GetListData(details, searchFields, selectedSortCriteria);
        }
        public List<MakerMaterialDetails> GetMaterialDetailsByGroupNo(int GroupNo)
        {
           
            var result = _makerMaterialRepository.GetMaterialDetailsByGroupNo(GroupNo);
          
            return result;
        }
        private List<SearchField> ComposeSearchCriteria(MakerMaterialDetails gridSearch)
        {
            var searchFields = new List<SearchField>();

           

            if (gridSearch.APPLY_ROLL_YEAR > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTMATL.APPLY_ROLL_YEAR",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearch.APPLY_ROLL_YEAR)
                };

                searchFields.Add(searchField);
            }

            if (gridSearch.APPLY_ROLL_MONTH > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTMATL.APPLY_ROLL_MONTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearch.APPLY_ROLL_MONTH)
                };

                searchFields.Add(searchField);
            }
            if (gridSearch.APPLY_ROLL_CHANCE > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTMATL.APPLY_ROLL_CHANCE",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearch.APPLY_ROLL_CHANCE)
                };

                searchFields.Add(searchField);
            }


            if (gridSearch.MAKER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MKR.MAKER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearch.MAKER_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearch.MUKESAKI_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTMATL.MUKESAKI_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearch.MUKESAKI_CD)
                };
                searchFields.Add(searchField);
            }
            if (gridSearch.MATL_KUBUN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "HTMATL.MATL_KUBUN",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearch.MATL_KUBUN)
                };
                searchFields.Add(searchField);
            }
            

            return searchFields;
        }
    }
}
