using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class RollPoForecastService : IRollPoForecastService
    {
        private readonly IRollPoForecastRepository _rollPoForecastRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
       private int  m_dispatchMonth1, m_dispatchMonth2, m_dispatchMonth3;
        private int m_dispatchYear1, m_dispatchYear2, m_dispatchYear3;

        public RollPoForecastService(IRollPoForecastRepository rollPoForecastRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _rollPoForecastRepository = rollPoForecastRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<GridSearchFields> GetProductMasterDataList(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaForProductList(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var obj = _rollPoForecastRepository.GetProductMasterDataList(searchFields, selectedSortCriteria);
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
       

        public DateTimeDetails GetPreviousForecastRoll(DateTime currentDate)
        {
            
            
            DateTime CurrentForecastDate,LastForecastDate;
            DateTimeDetails result = new DateTimeDetails();
            CurrentForecastDate = currentDate.AddMonths(2);
            result.CURRENT_R = CurrentForecastDate.Month;
            result.CURRENT_YYYY = CurrentForecastDate.Year;
            LastForecastDate= CurrentForecastDate.AddMonths(-1);
            result.LAST_R = LastForecastDate.Month;
            result.LAST_YYYY = LastForecastDate.Year;
            int count=_rollPoForecastRepository.IsBatchRunSuccess(result.CURRENT_R, result.CURRENT_YYYY);
            if (count > 0)
            {

                LastForecastDate = CurrentForecastDate.AddMonths(1);
                result.LAST_R = LastForecastDate.Month;
                result.LAST_YYYY = LastForecastDate.Year;
            }
            int countForecastData = _rollPoForecastRepository.IsForecastDataPresent(result.LAST_R, result.LAST_YYYY);
            if (countForecastData > 0)
            {
                    int countPreviousRData = _rollPoForecastRepository.IsPreviousRForecastLocked(result.LAST_R, result.LAST_YYYY);
                    if (countPreviousRData > 0)
                    {
                        result.FORECAST_R= currentDate.AddMonths(2);
                    result.CURRENT_R = result.FORECAST_R.Month;
                    result.CURRENT_YYYY = result.FORECAST_R.Year;

                }
                    else
                    {
                        result.FORECAST_R = CurrentForecastDate.AddMonths(-1);
                    result.CURRENT_R  = result.FORECAST_R.Month;
                    result.CURRENT_YYYY = result.FORECAST_R.Year;

                }
               
            }
            return result;
        }

        //public GridSearchFields GetProductDetailsByProductCD(long PRODUCT_CD)
        public RollSalesForecastDetails GetProductDetailsByProductCD(long PRODUCT_CD)
        {
            var _productDetails = _rollPoForecastRepository.GetProductDetailsByProductCD(PRODUCT_CD);

            _productDetails.PROCDUCT_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(_productDetails.PRODUCT_KUBUN));
            if (_productDetails.MAGARI_KUBUN != null)
            {
                _productDetails.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(_productDetails.MAGARI_KUBUN)), Convert.ToDecimal(_productDetails.MAGARI_BEND_TOL), Convert.ToInt32(_productDetails.MAGARI_SPAN));
            }

            return _productDetails;
        }
        public List<GridSearchFields> GetMaterialMasterDataList(GridSearchFields gridSearchRequest)
        {
            List<SearchField> searchFields = ComposeSearchCriteriaForMaterialList(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            var obj = _rollPoForecastRepository.GetMaterialMasterDataList(searchFields, selectedSortCriteria);
            foreach (var item in obj)
            {
                item.MATERIAL_TYPE_NAME_CBK = UIUtils.GetMaterialTypeName(Convert.ToString(item.MATL_TYPE_CBK));
                item.MATL_SPECIAL_KUBUN = UIUtils.GetGeneSpecNameForMaterialKubun(Convert.ToString(item.MATL_SPECIAL_KUBUN)); 
               
            }
            return obj;
        }
       
        public RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD)
        {
            var _productDetails = _rollPoForecastRepository.GetMaterialDetailsByMaterialCD(MATERIAL_CD);
           //_productDetails.MATERIAL_TYPE_NAME_CBK = UIUtils.GetMaterialTypeName(Convert.ToString(_productDetails.MATL_TYPE_CBK));
           // _productDetails.MATL_SPECIAL_KUBUN = UIUtils.GetGeneSpecNameForMaterialKubun(Convert.ToString(_productDetails.MATL_SPECIAL_KUBUN_INT));

          

            return _productDetails;
        }
        public List<NameValue> GetMaterialTypeList()
        {
            var _MaterialTypeList = UIUtils.GetMaterialTypeList();
            return _MaterialTypeList;
        }
        public List<NameValue> GetMatlSpecNameList()
        {
            var _MatlGenSplTypeList = UIUtils.GetMatlSpecNameList();
            return _MatlGenSplTypeList;
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


        public List<RollSalesForecastDetails> GetSpecInput1DetailsByCustInchrge_CD(RollSalesForecastDetails request)
        {
            List<RollSalesForecastDetails> _data = null;

            var recordCount = CalculateRParameter(request.CUSTOMER_CD,request.CURRENT_R,request.CURRENT_YYYY);
            if (recordCount.count != 0)
            {
                GetDispatchHistoryMonths(request.CURRENT_R, request.CURRENT_YYYY, out m_dispatchMonth1, out m_dispatchMonth2, out m_dispatchMonth3, out m_dispatchYear1, out m_dispatchYear2, out m_dispatchYear3);
                 _data = _rollPoForecastRepository.GetSpecInput1Details(request.CUSTOMER_CD, request.EMPLOYEE_CD, request.CURRENT_R, request.CURRENT_YYYY, m_dispatchMonth1, m_dispatchMonth2, m_dispatchMonth3, m_dispatchYear1, m_dispatchYear2, m_dispatchYear3);
                foreach (RollSalesForecastDetails record in _data)
                {
                    record.PROCDUCT_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(record.PRODUCT_KUBUN));
                    if (record.MAGARI_KUBUN != null)
                    {
                        record.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN)), Convert.ToDecimal(record.MAGARI_BEND_TOL), Convert.ToInt32(record.MAGARI_SPAN));
                    }
                    if (record.DISPATCH_QTY1 != null)
                    {
                        record.DISPATCH_QTY1 = Convert.ToDecimal(Convert.ToDecimal(record.DISPATCH_QTY1).ToString("#.##"));
                    }
                    if (record.DISPATCH_QTY2 != null)
                    {
                        record.DISPATCH_QTY2 = Convert.ToDecimal(Convert.ToDecimal(record.DISPATCH_QTY1).ToString("#.##"));
                    }
                    if (record.DISPATCH_QTY3 != null)
                    {
                        record.DISPATCH_QTY3 = Convert.ToDecimal(Convert.ToDecimal(record.DISPATCH_QTY1).ToString("#.##"));
                    }
                }
            }
                return _data;
            
            
        }

        //public List<RollSalesForecastDetails> GetSpecInput2DetailsByCustInchrge_CD(RollSalesForecastDetails rollSalesForecastRequest)
        //{

        //    //DateTimeDetails result = new DateTimeDetails();
        //    //result=CalculateRParameter(rollSalesForecastRequest.CUSTOMER_CD ,rollSalesForecastRequest.CURRENT_R, rollSalesForecastRequest.CURRENT_YYYY);
            
        //    ////var _data = _rollPoForecastRepository.GetSpecInput2Details(rollSalesForecastRequest);
        //    //foreach (RollSalesForecastDetails record in _data)
        //    //{
        //    //    record.PROCDUCT_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(record.PRODUCT_KUBUN));
        //    //    if (record.MAGARI_KUBUN != null)
        //    //    {
        //    //        record.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.MAGARI_KUBUN)), Convert.ToDecimal(record.MAGARI_BEND_TOL), Convert.ToInt32(record.MAGARI_SPAN));
        //    //    }


        //    //}
        //    //return _data;
        //    return null;
        //}
        public DateTimeDetails CalculateRParameter(int customer_CD,int current_R, int current_YYYY)
        {
            
            DateTimeDetails result = new DateTimeDetails();
            result = AddRs(current_R, current_YYYY, -1);
            while (DiffernceR(current_R,result.resultMonth) <= 6)
            {
                result.count = _rollPoForecastRepository.IsForecastDataPresentCustomerWise(customer_CD, result.LAST_R, result.LAST_YYYY);

                if (result.count > 0)
                {
                    break;
                }
                else
                {
                    // 🔥 FIX: use LAST_R and LAST_YYYY instead of CURRENT
                    result = AddRs(result.resultMonth, result.resultYear, -1);
                }
            }
            return result;
        }

        public DateTimeDetails Rcalculate(int sourceR, int sourceYYYY, int addR)
        {
            DateTime lastDate = new DateTime(sourceYYYY, sourceR, 1).AddMonths(addR);

            return new DateTimeDetails
            {
                LAST_R = lastDate.Month,
                LAST_YYYY = lastDate.Year,
                //CURRENT_R = sourceR,
                //CURRENT_YYYY = sourceYYYY
            };
        }

        public int DiffernceR(int current_R,int last_R)
        {
            int difference, month_count = 12;
                if (current_R > last_R)
            {
                difference = current_R - last_R;
            }
            else
            {
                difference = month_count - last_R + current_R;

            }
          return difference;
        }

        public void GetDispatchHistoryMonths(int forecastR,int forecastYYYY,out int month1, out int month2, out int month3, out int year1, out int year2, out int year3)
       {
            DateTimeDetails histortyMonthYear = new DateTimeDetails();
            histortyMonthYear = AddRs(forecastR, forecastYYYY, -5);
            month1= histortyMonthYear.resultMonth;
            year1= histortyMonthYear.resultYear;
            histortyMonthYear=AddRs(forecastR, forecastYYYY, -4);
            month2 = histortyMonthYear.resultMonth;
            year2 = histortyMonthYear.resultYear;
            histortyMonthYear=AddRs(forecastR, forecastYYYY, -3);
            month3 = histortyMonthYear.resultMonth;
            year3 = histortyMonthYear.resultYear;
        }
        public DateTimeDetails AddRs(int forecastR, int forecastYYYY, int addMonths)
        {
            DateTimeDetails result = new DateTimeDetails();
            DateTime date = new DateTime(forecastYYYY, forecastR, 1).AddMonths(addMonths);
            result.resultMonth = date.Month;
            result.resultYear = date.Year;
            return result;
        }

        public List<RollSalesForecastDetails> GetSpecInput1MaterialDetailsByCustInchrge_CD(int cust_Cd, int incharge_Cd, int? current_R, int? current_YYYY)
        {
            //var recordCount = CalculateRParameter(cust_Cd, current_R, current_YYYY);
            var _data = _rollPoForecastRepository.GetSpecInput1MaterailaDetails(cust_Cd, incharge_Cd, current_R,current_YYYY);
            foreach (RollSalesForecastDetails record in _data)
            {
                record.MATERIAL_TYPE_NAME_CBK = UIUtils.GetProductKubunNameByID(Convert.ToString(record.PRODUCT_KUBUN));
               
            }



            //return _productDetails;
            return _data;
        }

        public RollSalesForecastDetails SaveProductMaterialForecastDetails(RollSalesForecastDetails requestData)
        {

            requestData = _rollPoForecastRepository.SaveProductMaterialForecastDetails(requestData);
            return requestData;
        }


      
        public List<RollSalesForecastDetails> GetProductForecastDetails(GridSearchFields searchRequest)
        {
           
            List<SearchField> searchFields = ComposeSearchCriteria(searchRequest);
            List<RollSalesForecastDetails> list = new List<RollSalesForecastDetails>();
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            list = _rollPoForecastRepository.GetProductForecastDetails( searchFields, selectedSortCriteria);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    item.PROCDUCT_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(item.PRODUCT_KUBUN));
                    if (item.SALES_FC_DONE_DATE != null && item.SALES_FC_DONE_DATE != DateTime.MinValue)
                    {
                        item.SALES_FC_DONE = true;
                    }
                    else
                    {
                        item.SALES_FC_DONE = false;
                    }

                    if (item.MAGARI_KUBUN != null)
                    {
                        item.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(item.MAGARI_KUBUN)), Convert.ToDecimal(item.MAGARI_BEND_TOL), Convert.ToInt32(item.MAGARI_SPAN));
                    }
                }
            }
           
            return list;
        }

        public List<RollSalesForecastDetails> GetMaterialForecastDetails(GridSearchFields searchRequest)
        {

            List<SearchField> searchFields = ComposeSearchCriteria(searchRequest);
            List<RollSalesForecastDetails> list = new List<RollSalesForecastDetails>();
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            list = _rollPoForecastRepository.GetMaterialForecastDetails(searchFields, selectedSortCriteria);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    item.MATERIAL_KUBUN_STRING = UIUtils.GetProductKubunNameByID(Convert.ToString(item.MATL_KUBUN));
                    if (item.SALES_FC_DONE_DATE != null && item.SALES_FC_DONE_DATE != DateTime.MinValue)
                    {
                        item.SALES_FC_DONE = true;
                    }
                    else
                    {
                        item.SALES_FC_DONE = false;
                    }

                    if (item.MAGARI_KUBUN != null)
                    {
                        item.MAGARI_BEND_TOLLERENCE = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(item.MAGARI_KUBUN)), Convert.ToDecimal(item.MAGARI_BEND_TOL), Convert.ToInt32(item.MAGARI_SPAN));
                    }
                }
            }

            return list;
        }

        private List<SearchField> ComposeSearchCriteria(GridSearchFields request)
        {
            var searchFields = new List<SearchField>();

            if (request.CUSTOMER_CD > 0)
            {
                var cust_Cd = request.CUSTOMER_CD;
                var searchField = new SearchField
                {
                    FieldName = "CUST_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = cust_Cd.ToString()
                };

                searchFields.Add(searchField);
            }

            if (request.ROLL_YEAR > 0)
            {
                var rollFc_year = request.ROLL_YEAR;
                var searchField = new SearchField
                {
                    FieldName = "ROLL_FC_YEAR",
                    SearchType = SearchType.Number,
                    SearchText1 = rollFc_year.ToString()
                };

                searchFields.Add(searchField);
            }
            if (request.ROLL_R > 0)
            {
                var rollFc_R = request.ROLL_R;
                var searchField = new SearchField
                {
                    FieldName = "ROLL_FC_R",
                    SearchType = SearchType.Number,
                    SearchText1 = rollFc_R.ToString()
                };

                searchFields.Add(searchField);
            }



            if (request.HISTORY_MONTH1 > 0)
            {
                var historyMonth1 = request.HISTORY_MONTH1;
                var searchField = new SearchField
                {
                    FieldName = "MONTH",
                    SearchType = SearchType.Number,
                    SearchText1 = historyMonth1.ToString()
                };
                searchFields.Add(searchField);
            }
            if (request.HISTORY_MONTH2 > 0)
            {
                var historyMonth1 = request.HISTORY_MONTH2;
                var searchField = new SearchField
                {
                    FieldName = "MONTH",
                    SearchType = SearchType.Number,
                    SearchText1 = historyMonth1.ToString()
                };
                searchFields.Add(searchField);
            }
            if (request.HISTORY_MONTH3 > 0)
            {
                var historyMonth1 = request.HISTORY_MONTH3;
                var searchField = new SearchField
                {
                    FieldName = "MONTH",
                    SearchType = SearchType.Number,
                    SearchText1 = historyMonth1.ToString()
                };
                searchFields.Add(searchField);
            }



            return searchFields;

        }
    }
}
