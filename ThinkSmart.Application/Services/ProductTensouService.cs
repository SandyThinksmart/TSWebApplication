using System;
using System.Collections.Generic;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
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
            if (item.UNIT != null)
            {
                item.UNIT_TYPE_STRING = Convert.ToDecimal(item.NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(item.UNIT));
            }
        }
        return obj;
    }

    public List<WIPTensouDetails> GetProductDataSearchList(GridSearchFields gridSearchRequest)
    {
        string Location_CD = Convert.ToString(_userLogin.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
        gridSearchRequest.LOCATION_CD = Convert.ToInt16(Location_CD);

        List<WIPTensouDetails> WIPTensouDetails = new List<WIPTensouDetails>();
        List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
        string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
        WIPTensouDetails = _ProductTensouRepository.GetProductTensouList(searchFields, selectedSortCriteria, Convert.ToInt32(gridSearchRequest.LOCATION_CD));
        return WIPTensouDetails;
    }

    private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
    {
        var searchFields = new List<SearchField>();

        if (!string.IsNullOrEmpty(gridSearchRequest.CHUUBAN))
        {
            var searchField = new SearchField
            {
                FieldName = "CONCAT(PP.CHUUBAN_YEAR, CONCAT(PP.CHUUBAN ,CASE WHEN LENGTH(TRIM(PP.GYOUBAN))=1 THEN '0'||PP.GYOUBAN END))",//"PP.CHUUBAN",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.CHUUBAN.Trim()
            };
            searchFields.Add(searchField);

        }

        if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
        {
            var searchField = new SearchField
            {
                FieldName = "CUS.SHORT_NAME",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
            };

            searchFields.Add(searchField);
        }

        if (!string.IsNullOrEmpty(gridSearchRequest.PROD_NAME_CUST))
        {
            var searchField = new SearchField
            {
                FieldName = "PP.PROD_NAME_CUST",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.PROD_NAME_CUST.ToLower().Trim()
            };
            searchFields.Add(searchField);
        }

        if (!string.IsNullOrEmpty(gridSearchRequest.INTERNAL_NAME_STRING))
        {
            var searchField = new SearchField
            {
                FieldName = "MIN.INTERNAL_NAME",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
            };
            searchFields.Add(searchField);
        }

        if (gridSearchRequest.PP_DIM_TOL_MAX == "")
        {
            if (gridSearchRequest.PP_DIM_TOL_MIN != "")
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MIN)
                };
                searchFields.Add(searchField);
            }
        }
        else if (gridSearchRequest.PP_DIM_TOL_MIN == "")
        {
            if (gridSearchRequest.PP_DIM_TOL_MAX != "")
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MAX)
                };
                searchFields.Add(searchField);
            }
        }
        else if (!string.IsNullOrEmpty(gridSearchRequest.PP_DIM_TOL_MIN) && !string.IsNullOrEmpty(gridSearchRequest.PP_DIM_TOL_MAX))
        {
            var searchField = new SearchField
            {
                FieldName = "PP.PROD_DIM_D",
                SearchType = SearchType.Number,
                SearchText1 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MIN),
                SearchText2 = Convert.ToString(gridSearchRequest.PP_DIM_TOL_MAX)

            };
            searchFields.Add(searchField);
        }

        if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SHAPE))
        {
            var searchField = new SearchField
            {
                FieldName = "PP.PROD_SHAPE",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.PROD_SHAPE.ToLower().Trim()
            };
            searchFields.Add(searchField);
        }

        if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
        {
            var searchField = new SearchField
            {
                FieldName = "PP.CHARGE",
                SearchType = SearchType.Text,
                SearchText1 = gridSearchRequest.CHARGE.ToLower().Trim()
            };
            searchFields.Add(searchField);
        }

        if (string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_END))
        {
            if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START))
            {
                var searchField = new SearchField
                {
                    FieldName = "PROD.TRANSIT_PLANNED_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.TRANSFER_DATE_START
                };
                searchFields.Add(searchField);
            }
        }
        else if (string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START))
        {
            if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_END))
            {
                var searchField = new SearchField
                {
                    FieldName = "PROD.TRANSIT_PLANNED_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.TRANSFER_DATE_END
                };
                searchFields.Add(searchField);
            }
        }
        else if (!string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_START) && !string.IsNullOrEmpty(gridSearchRequest.TRANSFER_DATE_END))
        {
            var searchField = new SearchField
            {
                FieldName = "PROD.TRANSIT_PLANNED_DATE",
                SearchType = SearchType.Date,
                SearchText1 = gridSearchRequest.TRANSFER_DATE_START,
                SearchText2 = gridSearchRequest.TRANSFER_DATE_END
            };
            searchFields.Add(searchField);
        }

        return searchFields;
    }

    public List<WIPTensouDetails> SaveProductTensouDetails(List<WIPTensouDetails> lstGridData)
    {
        List<WIPTensouDetails> obj = new List<WIPTensouDetails>();
        var kubunCD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding")));
        var groupNo = _ProductTensouRepository.GetMaxGroupNo();
        int i = 0;

        foreach (var item in lstGridData)
        {
            if (i < lstGridData.Count)
            {
                if (item.USER_ID != null && item.USER_ID > 0)
                {
                    item.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(item.USER_ID.Value);
                    item.LOCATION_CD = item.INPUT_LOCATION_CD;
                }

                if (item.USER_ID != null && item.USER_ID > 0)
                {
                    item.INPUT_LOCATION_CD = _userLogin.GetUserLocationById(item.USER_ID.Value);
                    item.LOCATION_CD = item.INPUT_LOCATION_CD;
                }

                if (item.PP_UNIT != null)
                {
                    item.PP_NO_OF_UNITS_DECODE = Convert.ToDecimal(item.PP_NO_OF_UNITS).ToString("0.####") + UIUtils.GetProductUnitKubunValue(Convert.ToString(item.PP_UNIT));
                }

               
                item.WIP_TRANSFER_GROUP_NO = groupNo + 1;
                item.PROCESS_KUBUN_ID = kubunCD;
                //Product Tensou Insert
                int TransferGroupNo = _ProductTensouRepository.SaveProductTensouDetails(item);
                obj = _ProductTensouRepository.GetTransferCD(TransferGroupNo);
            }
        }
        return obj;
    }

    public Dictionary<string, List<string>> GetValidShukkaCD(string productCD)
    {
       
        var obj = _ProductTensouRepository.GetValidShukkaCD(productCD);
        return obj;
    }

    public List<ProductTensouDetails> GetShukkaProductDetails(string shukkaCD)
    {
        var obj= _ProductTensouRepository.GetShukkaProductDetails(shukkaCD);
        return obj;
    }
}