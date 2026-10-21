using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    internal class YieldSearchService : IYieldSearchService
    {
        private readonly IYieldSearchRepository _YieldSearchRepository;
        private readonly IAllCommonRepository _CommonRepository;
        private readonly IUserLoginRepository _UserLoginRepository;
        private readonly LocalizationServices _localization;

        public YieldSearchService(IYieldSearchRepository YieldSearchRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization)
        {
            _YieldSearchRepository = YieldSearchRepository;
            _CommonRepository = allCommonRepository;
            _UserLoginRepository = userLoginRepository;
            _localization = localization;
        }
        public List<ProcessPlanForYieldSearchDetails> GetYieldSearchList(GridSearchFields gridSearchRequest)
        {
            short input_Location_CD = Convert.ToInt16(_UserLoginRepository.GetUserLocationById(Convert.ToInt32(gridSearchRequest.USER_ID)));
            gridSearchRequest.LOCATION_CD = input_Location_CD;

            List<ProcessPlanForYieldSearchDetails> yieldSearchDetails = new List<ProcessPlanForYieldSearchDetails>();
            List<SearchField> searchFields = ComposeSearchCriteria(gridSearchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(gridSearchRequest.SORT_FIELDS);
            if (gridSearchRequest.CULTURE_NAME == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            yieldSearchDetails = _YieldSearchRepository.GetYieldSearchList(searchFields, selectedSortCriteria, gridSearchRequest.LOCATION_CD);
            foreach (ProcessPlanForYieldSearchDetails record in yieldSearchDetails)
            {
                record.BEND_STD = Utils.GetMagariBendTolSpan(UIUtils.Instance.GetMagariKubunValue(Convert.ToString(record.PP_MAGARI_KUBUN)), Convert.ToDecimal(record.PP_BEND_TOL), Convert.ToInt32(record.PP_MAGARI_SPAN));
                //if (string.IsNullOrEmpty(record.PROCESS_PLAN_CMPL_DATE_STRING))
                //{
                //    record.PROD_CONDITION_DESCRIPTION = _localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun1of2");
                //}
                //else
                //{
                //    record.PROD_CONDITION_DESCRIPTION = _localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun2of2");
                //}

            }
            return yieldSearchDetails;
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
                    // FieldName = "PP.PROD_NAME_INT",
                    FieldName = "INTE.INTERNAL_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.INTERNAL_NAME_STRING.ToLower().Trim()
                };
                searchFields.Add(searchField);
            }

            if (gridSearchRequest.PROD_DIM_D_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_DIM_D_MAX)

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

            if (gridSearchRequest.PROD_LENGTH_MIN > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_LENGTH",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MIN),
                    SearchText2 = Convert.ToString(gridSearchRequest.PROD_LENGTH_MAX)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.CUSTOMER_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CST.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CUSTOMER_NAME.Trim()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.SALES_INCHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "EMP.SHORT_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.SALES_INCHARGE.Trim()
                };

                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.START_DATETIME))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE(TO_CHAR(PP.ACT_ALLOC_DATE))",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.START_DATETIME,
                    SearchText2 = gridSearchRequest.END_DATETIME
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO1))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO1",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SPEC_NO1,

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.PROD_SPEC_NO2))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.PROD_SPEC_NO2",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.PROD_SPEC_NO2
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "PP.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.COMPLETE_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "TO_DATE(TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE))",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.COMPLETE_START_DATE,
                    SearchText2 = gridSearchRequest.COMPLETE_END_DATE
                };
                searchFields.Add(searchField);
            }
            if (gridSearchRequest.MAX_ROW_COUNT > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "ROWNUM <",
                    SearchType = SearchType.Number,
                    SearchText1 = gridSearchRequest.MAX_ROW_COUNT.ToString(),
                };
                searchFields.Add(searchField);
            }
            return searchFields;
        }

        public List<ProcessPlanForYieldSearchDetails> UpdateECYieldValue(List<ProcessPlanForYieldSearchDetails> yieldDetails)
        {
            //try
            //{
                if (yieldDetails != null && yieldDetails.Count > 0)
                {
                    foreach (var item in yieldDetails)
                    {
                        long processPlanCd = item.PP_PROCESS_PLAN_CD;

                        decimal? ecYieldVal = CalculateECYield(processPlanCd);

                        if (ecYieldVal.HasValue)
                        {
                            item.EC_YIELD = ecYieldVal.Value;
                        }
                    }
                }

                return yieldDetails;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public decimal? CalculateECYield(long processPlanCd)
        {
            decimal ecYieldVal = 0;
            decimal ecOkQty = 0;
            decimal ecNgQty = 0;
            decimal tdEcOkQty = 0;
            decimal tdEcNgBarTotal = 0;

            //try
            //{
                DataTable ecYield = _YieldSearchRepository.LoadECValForECPercentData(processPlanCd);
                DataTable hikinukiYield = _YieldSearchRepository.GetDWValForECPercent(processPlanCd);

                if (ecYield != null && ecYield.Rows.Count > 0)
                {
                    for (int i = 0; i < ecYield.Rows.Count; i++)
                    {
                        if (ecYield.Rows[i]["EC_OK_QTY"] != DBNull.Value)
                        {
                            ecOkQty = Convert.ToDecimal(ecYield.Rows[i]["EC_OK_QTY"]);
                        }

                        if (ecYield.Rows[i]["EC_NG_QTY"] != DBNull.Value)
                        {
                            ecNgQty = Convert.ToDecimal(ecYield.Rows[i]["EC_NG_QTY"]);
                        }
                    }
                }

                if (hikinukiYield != null && hikinukiYield.Rows.Count > 0)
                {
                    for (int i = 0; i < hikinukiYield.Rows.Count; i++)
                    {
                        if (hikinukiYield.Rows[i]["TD_EC_OK_QTY"] != DBNull.Value)
                        {
                            tdEcOkQty = Convert.ToDecimal(hikinukiYield.Rows[i]["TD_EC_OK_QTY"]);
                        }

                        if (hikinukiYield.Rows[i]["EC_NG_BAR_TOTAL"] != DBNull.Value)
                        {
                            tdEcNgBarTotal = Convert.ToDecimal(hikinukiYield.Rows[i]["EC_NG_BAR_TOTAL"]);
                        }
                    }
                }

                decimal totalQty = ecOkQty + ecNgQty + tdEcOkQty + tdEcNgBarTotal;
                decimal totalNgQty = ecNgQty + tdEcNgBarTotal;

                if (totalQty <= 0)
                    return null;

                ecYieldVal = Math.Round((totalNgQty / totalQty) * 100, 2);

                return ecYieldVal;
            //}
            //catch()
        }

        
    }
}
