using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class AnalysePOQuantityServices : IAnalysePOQuantityServices
    {

        private readonly IAnalysePOQuantityRepository _analysePOQuantityRepository;
        private readonly LocalizationServices _localization;
        public AnalysePOQuantityServices(IAnalysePOQuantityRepository analysePOQuantityRepository, LocalizationServices localization)
        {
            _localization = localization;
            _analysePOQuantityRepository = analysePOQuantityRepository;
        }
   
        public List<NameValue> GetMukesakiList()
        {
            var _MukesakiList = _analysePOQuantityRepository.GetMukesakiList();
            return _MukesakiList;
        }
        public List<NameValue> GetSupplierList()
        {
            var _SupplierList = _analysePOQuantityRepository.GetSupplierList();
            return _SupplierList;
        }
        public List<NameValue> GetDestinationList()
        {
            var _SupplierList = _analysePOQuantityRepository.GetDestinationList();
            return _SupplierList;
        }
        public List<NameValue> GetHikitorisakiList()
        {
            var _SupplierList = _analysePOQuantityRepository.GetHikitorisakiList();
            return _SupplierList;
        }
        public List<NameValue> GetFutouList()
        {
            var _SupplierList = _analysePOQuantityRepository.GetFutouList();
            return _SupplierList;
        }
        public string SaveMaterialPoData(List<RollPoMainAnalysisDetails> PODetails)
        {
            var obj = "";
            //using (TransactionScope transaction = new TransactionScope())
            //{
                    obj = _analysePOQuantityRepository.SaveMaterialPoData(PODetails);
               
                //transaction.Complete();
            //}
            return obj;
        }
        public List<RollPoMainAnalysisDetails> GetInternalNameWiseMakerList(long INT_MATL_FC_SUMMARY_CD)
        {
            var obj = _analysePOQuantityRepository.GetInternalNameWiseMakerList(INT_MATL_FC_SUMMARY_CD);
            return obj;
        }
        public RollPoMainAnalysisDetails GetInternalNameWIseMaterialDetails(long INT_MATL_FC_SUMMARY_CD)
        {
            var obj = _analysePOQuantityRepository.GetInternalNameWIseMaterialDetails(INT_MATL_FC_SUMMARY_CD);
            return obj;
        }
        public List<GridSearchFields> GetMaterialMasterDataList(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaForMaterialList(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var fc_Summary_No = gridSearchRequest.INT_MATL_FC_SUMMARY_CD;
            var obj = _analysePOQuantityRepository.GetMaterialMasterDataList(searchFields, selectedSortCriteria, fc_Summary_No);
            //foreach (var item in obj)
            //{
            //    item.MATERIAL_TYPE_NAME_CBK = UIUtils.GetMaterialTypeName(Convert.ToString(item.MATL_TYPE_CBK));
            //    item.MATL_SPECIAL_KUBUN = UIUtils.GetGeneSpecNameForMaterialKubun(Convert.ToString(item.MATL_SPECIAL_KUBUN));

            //}
            return obj;
        }
        public int checkAnalysisOverFlag(long INT_MATL_FC_SUMMARY_CD)
        {
            int AnalysisIsOver = 0;
            AnalysisIsOver = _analysePOQuantityRepository.checkAnalysisOverFlag(INT_MATL_FC_SUMMARY_CD);
            return AnalysisIsOver;
        }
        public int checkPoCompleteFlag()
        {
            int PoCompleteFlag = 0;
            PoCompleteFlag = _analysePOQuantityRepository.checkPoCompleteFlag();
            return PoCompleteFlag;
        }
        private List<SearchField> ComposeSearchCriteriaForMaterialList(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (!string.IsNullOrEmpty(gridSearchRequest.MAKER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MAK.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = Convert.ToString(gridSearchRequest.MAKER_NAME)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "MTINT.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_MAKER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MTMAK.MATL_MAKER_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_MAKER_NAME
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_DIM_D_MAX == null)
            {
                if (gridSearchRequest.MATL_DIM_D_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "MATL.DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.MATL_DIM_D_MIN == null)
            {
                if (gridSearchRequest.MATL_DIM_D_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "MATL.DIM_D",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.MATL_DIM_D_MIN > 0 && gridSearchRequest.MATL_DIM_D_MAX > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_D_MAX)

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.SHAPE",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.MATL_SHAPE + "'"
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MATL_LENGTH_MAX == null)
            {
                if (gridSearchRequest.MATL_LENGTH_MIN > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "MATL.MATERIAL_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.MATL_LENGTH_MIN == null)
            {
                if (gridSearchRequest.MATL_LENGTH_MAX > 0)
                {
                    var searchField = new SearchField
                    {
                        FieldName = "MATL.MATERIAL_LENGTH",
                        SearchType = SearchType.Number,
                        SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                    };
                    searchFields.Add(searchField);
                }
            }
            else if (gridSearchRequest.MATL_LENGTH_MIN > 0 && gridSearchRequest.MATL_LENGTH_MAX > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.MATERIAL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SPECIAL_KUBUN))
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.MATL_SPECIAL_KUBUN",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.MATL_SPECIAL_KUBUN + "'"
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MATL_TYPE_CBK > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.MATL_KUBUN",
                    SearchType = SearchType.Number,
                    SearchText1 = "'" + gridSearchRequest.MATL_TYPE_CBK + "'"
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }
        public RollSalesForecastDetails GetMakerMaterialDetailsByCD(long INT_MATL_FC_SUMMARY_CD, long MATERIAL_CD, short MUKESAKI_CD)
        {
            var obj = _analysePOQuantityRepository.GetMakerMaterialDetailsByCD(INT_MATL_FC_SUMMARY_CD, MATERIAL_CD, MUKESAKI_CD);
            return obj;
        }
        public List<RollPoMainAnalysisDetails> GetRollPoDetails(long INT_MATL_FC_SUMMARY_CD)
        {
            var obj = _analysePOQuantityRepository.GetRollPoDetails(INT_MATL_FC_SUMMARY_CD);
            return obj;
        }
        public RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD, short? MUKESAKI_CD)
        {
            var _productDetails = _analysePOQuantityRepository.GetMaterialDetailsByMaterialCD(MATERIAL_CD, MUKESAKI_CD);
            
            return _productDetails;
        }
        public float GetMukesakiMaterialRate(short mukesakiCd, decimal matlDimD, int matlKubun, int matlNameMakerCd)
        {
            float mukesakiRate  = _analysePOQuantityRepository.GetMukesakiMaterialRate(mukesakiCd, matlDimD, matlKubun, matlNameMakerCd);
            return mukesakiRate;
        }        

         public string UpdateForecastData(List<RollSalesForecastDetails> forecastDetails)
        {
            var obj = "";
          
            obj = _analysePOQuantityRepository.UpdateForecastData(forecastDetails);

            return obj;
        }
    }
}
