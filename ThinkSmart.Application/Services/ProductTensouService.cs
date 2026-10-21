using System;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

public class ProductTensouService : IProductTensouService
{
    private readonly IProductTensouRepository _ProductTensouRepository;
    private readonly IUserLoginRepository _userLogin;
    private readonly IAllCommonRepository _AllCommonRepository;
    private readonly LocalizationServices _localization;

    public ProductTensouService(
        IProductTensouRepository productTensouRepository,
        IUserLoginRepository userLogin,
        IAllCommonRepository AllCommonRepository,
        LocalizationServices localization)
    {
        _ProductTensouRepository = productTensouRepository;
        _userLogin = userLogin;
        _AllCommonRepository = AllCommonRepository;
        _localization = localization;
    }

    public List<NameValue> GetVehicleName()
    {
        return _userLogin.GetVehicleName();
    }

    public List<NameValue> GetForwordingLocation()
    {
        return _userLogin.GetForwordingLocation();
    }

    public List<NameValue> GetTensouLocation(int USER_ID)
    {
        return _userLogin.GetUserLocationNameById(USER_ID);
    }

    public List<GridSearchFields> GetProductMasterDataList(GridSearchFields gridSearchRequest)
    {
        List<SearchField> searchFields = ComposeSearchCriteriaForProductList(gridSearchRequest);
        string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
        var obj = _ProductTensouRepository.GetProductMasterDataList(searchFields, selectedSortCriteria);
        foreach (var item in obj)
        {
            item.PROCDUCT_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(item.PRODUCT_KUBUN));
            if (item.MAGARI_KUBUN != null)
            {
                item.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(item.MAGARI_KUBUN)), Convert.ToDecimal(item.MAGARI_BEND_TOL), Convert.ToInt32(item.MAGARI_SPAN));
            }

        }
        return obj;
    }

    private List<SearchField> ComposeSearchCriteriaForProductList(GridSearchFields gridSearchRequest)
    {
        var searchFields = new List<SearchField>();
        if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.CUSTOMER_NAME",
                SearchType = SearchType.Text,
                SearchText1 = Convert.ToString(gridSearchRequest.CUSTOMER_NAME)
            };
            searchFields.Add(searchField);
        }
        if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.INTERNAL_NAME",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING
            };
            searchFields.Add(searchField);
        }
        if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.PRODUCT_NAME_CUST",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.PROD_NAME_CUST
            };
            searchFields.Add(searchField);
        }
        if (gridSearchRequest.PROD_DIM_D_MAX == null)
        {
            if (gridSearchRequest.PROD_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PRD.DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN)
                };
                searchFields.Add(searchField);
            }
        }
        else if (gridSearchRequest.PROD_DIM_D_MIN == null)
        {
            if (gridSearchRequest.PROD_DIM_D_MAX > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PRD.DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)
                };
                searchFields.Add(searchField);
            }
        }
        else if (gridSearchRequest.PROD_DIM_D_MIN > 0 && gridSearchRequest.PROD_DIM_D_MAX > 0)
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.DIM_D",
                SearchType = SearchType.Number,
                SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

            };
            searchFields.Add(searchField);
        }
        if (!string.IsNullOrEmpty(gridSearchRequest.SHAPE))
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.SHAPE",
                SearchType = SearchType.Number,
                SearchText1 = "'" + gridSearchRequest.SHAPE + "'"
            };
            searchFields.Add(searchField);
        }

        if (gridSearchRequest.PROD_LENGTH_MAX == null)
        {
            if (gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PRD.PRODUCT_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN)
                };
                searchFields.Add(searchField);
            }
        }
        else if (gridSearchRequest.PROD_LENGTH_MIN == null)
        {
            if (gridSearchRequest.PROD_LENGTH_MAX > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PRD.PRODUCT_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }
        }
        else if (gridSearchRequest.PROD_LENGTH_MIN > 0 && gridSearchRequest.PROD_LENGTH_MAX > 0)
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.PRODUCT_LENGTH",
                SearchType = SearchType.Number,
                SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
            };
            searchFields.Add(searchField);
        }
        if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.PROD_SPEC_NO1",
                SearchType = SearchType.Text,
                SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO1 + "'"
            };
            searchFields.Add(searchField);
        }
        if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
        {
            var searchField = new SearchField
            {
                FieldName = "PRD.PROD_SPEC_NO2",
                SearchType = SearchType.Text,
                SearchText1 = "'" + gridSearchRequest.PROD_SPEC_NO2 + "'"
            };
            searchFields.Add(searchField);
        }
        return searchFields;
    }

    public List<GridSearchFields> GetProductDetailsByProductCD(string productCD)
    {
        
        var obj = _ProductTensouRepository.GetProductDetailsByProductCD(productCD);
        foreach (var item in obj)
        {
            item.PROCDUCT_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(item.PRODUCT_KUBUN));
            if (item.MAGARI_KUBUN != null)
            {
                item.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(item.MAGARI_KUBUN)), Convert.ToDecimal(item.MAGARI_BEND_TOL), Convert.ToInt32(item.MAGARI_SPAN));
                //item.INPUT_LOCATION_CD = _userLogin.GetUserLocationNameById(Convert.ToString(item.USER_ID));
            }
        }
        return obj;
    }
}