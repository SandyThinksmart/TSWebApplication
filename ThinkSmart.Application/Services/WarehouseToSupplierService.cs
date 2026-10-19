using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.Services
{
    public class WarehouseToSupplierService : IWarehouseToSupplierService
    {
        private readonly IWarehouseToSupplierRepository _warehouseToSupplierRepository;
        private readonly IUserLoginRepository _userLogin;
        private readonly IAllCommonRepository _AllCommonRepository;
        public WarehouseToSupplierService(IWarehouseToSupplierRepository WarehouseToSupplierRepository, IUserLoginRepository userLogin, IAllCommonRepository AllCommonRepository)
        {
            _userLogin = userLogin;
            _warehouseToSupplierRepository = WarehouseToSupplierRepository;
            _AllCommonRepository = AllCommonRepository;


        }

        public KoubanDetails GetKoubanDetails(string kouban)
        {
            return _warehouseToSupplierRepository.GetKoubanDetailsByKouban(kouban);
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }
        public MaterialTensouDetails saveWarehouseToSupplierDetails(MaterialTensouDetails request)
        {
            MaterialTensouDetails tensouDetails = new MaterialTensouDetails();
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(request.USER_ID);
                request.LOCATION_CD = request.INPUT_LOCATION_CD;
                _warehouseToSupplierRepository.saveTensouDetails(request);
            }
            return tensouDetails;

        }
        public MaterialTensouDetails updateWarehouseToSupplierDetails(MaterialTensouDetails request)
        {
            MaterialTensouDetails tensouDetails = new MaterialTensouDetails();
            if (request.USER_ID != null && request.USER_ID > 0)
            {
                request.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(request.USER_ID);
                request.LOCATION_CD = request.INPUT_LOCATION_CD;
                _warehouseToSupplierRepository.updateTensouDetails(request);
            }
            return tensouDetails;

        }
        public List<MaterialTensouDetails> GetDataOnGridView(GridSearchFields gridSearchRequest)
        {
            int Location_CD = _userLogin.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID));
            gridSearchRequest.LOCATION_CD = Convert.ToInt16(Location_CD);

            List<MaterialTensouDetails> result = new List<MaterialTensouDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            result = _warehouseToSupplierRepository.GetMatlTensouList(selectedSortCriteria,searchFields);
           return result;
        }
        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {
            var searchFields = new List<SearchField>();
            if (gridSearchRequest.MATL_NAME_MAKER_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MTL_MKR.MATL_NAME_MAKER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_NAME_MAKER_CD)
                };

                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_MAKER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "MTL_MKR.MATL_MAKER_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_MAKER_NAME.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
            {
                var searchField = new SearchField
                {
                    FieldName = "INT.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToString()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MAT_START_DIM_D))
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.MAT_START_DIM_D.ToLower().Trim(),
                    SearchText2 = gridSearchRequest.MAT_END_DIM_D.ToLower().Trim()

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_SHAPE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.SHAPE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_SHAPE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_START_LENGTH))
            {
                var searchField = new SearchField
                {
                    FieldName = "MATL.MATERIAL_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.MATL_START_LENGTH.ToLower().Trim(),
                    SearchText2 = gridSearchRequest.MATL_END_LENGTH.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.KOUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "KOB.KOUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.KOUBAN.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "TRANS.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE((TO_CHAR(TRANS.TRANSFER_DATE, 'mm/dd/yyyy')), 'mm/dd/yyyy')",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.FUTOU_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "FUTOU.FUTOU_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.FUTOU_CD.ToString()
                };

                searchFields.Add(searchField);
            }

            if (gridSearchRequest.MOCHIKOMISAKI_CD > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MOCH.MOCHIKOMISAKI_Cd",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.MOCHIKOMISAKI_CD.ToString()
                };

                searchFields.Add(searchField);
            }

            return searchFields;

        }

     
        public MaterialTensouDetails GetMaterialTransferDetailsByTrasferCD(long TRANSFER_CD)
        {
            var _MaterialTransferDetails = _warehouseToSupplierRepository.GetMaterialTransferDetailsByTrasferCD(TRANSFER_CD);
            return _MaterialTransferDetails;
        }
        //public List<MaterialTensouDetails> GetTensouDataSearchList(GridSearchFields gridSearchRequest)
        //{
        //    string Location_CD = Convert.ToString(_userLogin.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
        //    gridSearchRequest.LOCATION_CD = Convert.ToInt16(Location_CD);

        //    List<MaterialTensouDetails> TensouDetails = new List<MaterialTensouDetails>();
        //    List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
        //    string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
        //    TensouDetails = _warehouseToSupplierRepository.GetMaterialTensouList(searchFields, selectedSortCriteria);
        //    return TensouDetails;
        //}
    }
}
