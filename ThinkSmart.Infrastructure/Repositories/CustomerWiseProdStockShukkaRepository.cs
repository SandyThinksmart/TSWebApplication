using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class CustomerWiseProdStockShukkaRepository : BaseRepository, ICustomerWiseProdStockShukkaRepository
    {
        private readonly LocalizationServices _localization;
        public CustomerWiseProdStockShukkaRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }

        /// <summary>
        /// Get Customer Details.
        /// Created by shobhana[11 March 2025]
        /// </summary>
        /// <returns>
        /// Customer Wise Product Stock Shukka List 
        /// </returns>
        public List<CustomerWiseProdStockShukka> GetCustomerDetails(string customerCD)
        {


            List<CustomerWiseProdStockShukka> result = new List<CustomerWiseProdStockShukka>();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append("SELECT");
            selectQuery.Append(" C.SHORT_NAME AS CUST_NAME,");
            selectQuery.Append(" C.CUSTOMER_CD,");
            selectQuery.Append("  ROUND(SUM(NVL(BKH.KAKOUIARI_QTY - BKH.FI_WEIGHT, 0)) / 1000,1) ORD_REM_STOCK,"); // Sum for customer 
            selectQuery.Append(" COALESCE(SUM(PSH.CURR_STOCK), 0) AS PROD_STOCK,"); // Sum for stock
            selectQuery.Append(" ROUND(SUM(NVL(BKH.ACT_ALLOC_PENDNIG_WT, 0)) / 1000,1) BEFORE_ACT_ALLOC,"); // Sum pending allocation
            selectQuery.Append(" ROUND(SUM(NVL(BKH.ACT_ALLOC_QTY - BKH.FI_WEIGHT, 0)) / 1000,1) WIP_ACT_ALLOC,"); // Sum WIP allocation
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 30 ");
            selectQuery.Append(" AND ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) <= 60 THEN ");
            selectQuery.Append(" CURR_STOCK ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(") / 1000 AS TOT_PEND_STOCK_ABOVE30, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 60 ");
            selectQuery.Append(" AND ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) <= 90 THEN ");
            selectQuery.Append(" CURR_STOCK ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(") / 1000 AS TOT_PEND_STOCK_ABOVE60, ");
            selectQuery.Append(" SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 90 THEN ");
            selectQuery.Append(" CURR_STOCK ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(") / 1000 AS TOT_PEND_STOCK_ABOVE90, ");
            selectQuery.Append("  ROUND(SUM(NVL(CURR_STOCK, 0)) / 1000,1) AS TOT_PROD_STOCK, ");
            selectQuery.Append(" BKH.KAKOUIRAI_HOST_CD, ");
            selectQuery.Append(" BKH.ROOT_VER_PRODUCT_KEY, ");
            selectQuery.Append(" BKH.PROD_SPEC_NO1, ");
            selectQuery.Append(" BKH.PROD_SPEC_NO2 ");
            selectQuery.Append(" FROM BCH_KAKOUIRAI_HOST BKH ");
            selectQuery.Append(" LEFT JOIN BCH_PROD_STOCK_HOST PSH ");
            selectQuery.Append(" ON BKH.ROOT_VER_PRODUCT_KEY = PSH.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER C ");
            selectQuery.Append(" ON PSH.CUSTOMER_CD = C.CUSTOMER_CD ");
            selectQuery.Append(" WHERE psh.ROOT_VER_PRODUCT_KEY IN( select DISTINCT P.ROOT_VER_PRODUCT_KEY FROM MST_PRODUCT P WHERE P.CUSTOMER_CD = '" + customerCD + "' AND VER = (SELECT MAX(VER) FROM MST_PRODUCT WHERE CUSTOMER_CD ='" + customerCD + "') ) ");
            selectQuery.Append(" GROUP BY  BKH.KAKOUIRAI_HOST_CD,BKH.ROOT_VER_PRODUCT_KEY,BKH.PROD_SPEC_NO1,BKH.PROD_SPEC_NO2,C.SHORT_NAME,C.CUSTOMER_CD ");
            selectQuery.Append(" ORDER BY BKH.KAKOUIRAI_HOST_CD DESC ");
            //selectQuery.Append(" FETCH FIRST 1 ROW ONLY ");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<CustomerWiseProdStockShukka>(query);

            return result;
        }

        /// <summary>
        /// Get Get Dispatch Qty Customer Wise Details.
        /// Created by shobhana[17 March 2025]
        /// </summary>
        /// <returns>
        /// Customer wise dispatch Qty List.
        /// </returns>
        //public List<ProductDetails> GetDispatchQtyCustomerWise(string customerCD)
        //{

        //    DateTime today = DateTime.Now;
        //    int currentMonth = today.Month;
        //    List<ProductDetails> detailsList = new List<ProductDetails>();
        //    List<decimal> ShukkaData = new List<decimal>();

        //    today = today.AddYears(-1);
        //    int lastYear = Convert.ToInt32(today.Year);
        //    today = today.AddYears(-1);
        //    int lasttolastYear = Convert.ToInt32(today.Year);
        //    int getMonth = 0;
        //    int getYear = 0;

        //    for (int j = 1; j <= 2; j++)
        //    {
        //        ProductDetails details = new ProductDetails(); // Instantiate new object inside the loop

        //        for (int i = 1; i <= 12; i++)
        //        {
        //            string getDay = Convert.ToString(DateTime.Now.Day.ToString("00"));

        //            if (i != 1)
        //            {
        //                today = today.AddMonths(+1);
        //                getMonth = Convert.ToInt16(today.Month);
        //                getYear = Convert.ToInt32(today.Year);
        //            }
        //            else
        //            {
        //                getMonth = currentMonth;
        //                getYear = Convert.ToInt32(today.Year);
        //            }
        //            getDay = today.Year.ToString().Substring(2, 2) + getMonth.ToString("00") + getDay;
        //            StringBuilder selectQuery = new StringBuilder();
        //            selectQuery.Append(" SELECT ROUND(NVL(sum(DISPATCH_QTY), '0') / 1000) ");
        //            selectQuery.Append(" FROM BCH_MONTH_END_SHIPMENT BMS");
        //            selectQuery.Append(" JOIN MST_PRODUCT P ON P.ROOT_VER_PRODUCT_KEY=BMS.ROOT_VER_PRODUCT_KEY");
        //            selectQuery.Append(" WHERE P.CUSTOMER_CD = " + "'" + customerCD + "'");
        //            selectQuery.Append(j == 1 ? " AND YEAR = " + lastYear : " AND YEAR = " + lasttolastYear);
        //            selectQuery.Append(" AND MONTH = " + getMonth);

        //            decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
        //            ShukkaData.Add(getQty);

        //            // Assign the quantity to the corresponding month property
        //            switch (i)
        //            {
        //                case 1: details.MONTH1_QTY = getQty; break;
        //                case 2: details.MONTH2_QTY = getQty; break;
        //                case 3: details.MONTH3_QTY = getQty; break;
        //                case 4: details.MONTH4_QTY = getQty; break;
        //                case 5: details.MONTH5_QTY = getQty; break;
        //                case 6: details.MONTH6_QTY = getQty; break;
        //                case 7: details.MONTH7_QTY = getQty; break;
        //                case 8: details.MONTH8_QTY = getQty; break;
        //                case 9: details.MONTH9_QTY = getQty; break;
        //                case 10: details.MONTH10_QTY = getQty; break;
        //                case 11: details.MONTH11_QTY = getQty; break;
        //                case 12: details.MONTH12_QTY = getQty; break;
        //            }
        //        }

        //        // Compute min, max, avg, total values
        //        details.MAX_QTY = ShukkaData.Max();
        //        details.MIN_QTY = ShukkaData.Min();
        //        details.AVG_QTY = Convert.ToDecimal(ShukkaData.Average().ToString("########.000"));
        //        details.TOTAL_QTY = Convert.ToDecimal(ShukkaData.Sum().ToString("########.000")); //ShukkaData.Sum();

        //        detailsList.Add(details); // Add the new object to the list
        //    }

        //    return detailsList;

        //}
        public List<ProductDetails> GetDispatchQtyCustomerWise(string customerCD)
        {
            int gridRowCnt = 2;
            int gridColCnt = 12;
            DateTime today = DateTime.Now;
            //int currentMonth = today.Month;
            List<ProductDetails> detailsList = new List<ProductDetails>();


            DateTime lastYearDate = today.AddYears(-1);
            int lastYear = Convert.ToInt32(lastYearDate.Year);
            DateTime lastToLastYearDate = lastYearDate.AddYears(-1);
            int lastTolastYear = Convert.ToInt32(lastToLastYearDate.Year);
            int getMonth = 0;
            int getYear = 0;

            for (int j = 1; j <= gridRowCnt; j++)
            {
                List<decimal> ShukkaData = new List<decimal>();
                ProductDetails details = new ProductDetails(); // Instantiate new object inside the loop


                for (int p = 0; p <= gridColCnt - 1; p++)
                {
                    if (j == 1)
                    {
                        DateTime thisMonthDate = lastYearDate.AddMonths(p);
                        getMonth = Convert.ToInt16(thisMonthDate.Month);
                        getYear = Convert.ToInt32(thisMonthDate.Year);
                    }
                    else
                    {
                        DateTime thisMonthDate = lastToLastYearDate.AddMonths(p);
                        getMonth = Convert.ToInt16(thisMonthDate.Month);
                        getYear = Convert.ToInt32(thisMonthDate.Year);
                    }

                    StringBuilder selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT NVL(SUM(DISPATCH_QTY), '0') / 1000  AS TOTALDISPATCHQTY") ;
                    selectQuery.Append(" FROM BCH_MONTH_END_SHIPMENT BMS");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" BMS.ROOT_VER_PRODUCT_KEY IN( ");
                    selectQuery.Append(" SELECT DISTINCT p.root_ver_product_key FROM mst_Product p ");
                    selectQuery.Append(" WHERE  P.CUSTOMER_CD = '" + customerCD + "') ");
                    selectQuery.Append(" AND YEAR = " + getYear + "");
                    selectQuery.Append(" AND MONTH = " + getMonth + "");
                    string query = selectQuery.ToString();
                    object result = ExecuteScalar(query);

                    decimal getQty = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                    //decimal getQty = Convert.ToDecimal(getQty1);
                    //int getQty1 = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

                    //selectQuery.Append("SELECT ROUND(NVL(SUM(DISPATCH_QTY), 0) / 1000) ");
                    //selectQuery.Append("FROM BCH_MONTH_END_SHIPMENT BMS ");
                    //selectQuery.Append("JOIN MST_PRODUCT P ON P.ROOT_VER_PRODUCT_KEY = BMS.ROOT_VER_PRODUCT_KEY ");
                    //selectQuery.Append("WHERE P.CUSTOMER_CD = '" + customerCD + "' ");
                    //selectQuery.Append("AND YEAR = " + getYear + " ");
                    //selectQuery.Append("AND MONTH = " + getMonth);

                    //decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
                    ShukkaData.Add(getQty);

                    // Assign the quantity to the corresponding month property
                    switch (p)
                    {
                        case 0: details.MONTH1_QTY = getQty; break;
                        case 1: details.MONTH2_QTY = getQty; break;
                        case 2: details.MONTH3_QTY = getQty; break;
                        case 3: details.MONTH4_QTY = getQty; break;
                        case 4: details.MONTH5_QTY = getQty; break;
                        case 5: details.MONTH6_QTY = getQty; break;
                        case 6: details.MONTH7_QTY = getQty; break;
                        case 7: details.MONTH8_QTY = getQty; break;
                        case 8: details.MONTH9_QTY = getQty; break;
                        case 9: details.MONTH10_QTY = getQty; break;
                        case 10: details.MONTH11_QTY = getQty; break;
                        case 11: details.MONTH12_QTY = getQty; break;
                    }
                }


                // Compute min, max, avg, total values
                details.MAX_QTY = ShukkaData.Max();
                details.MIN_QTY = ShukkaData.Min();
                details.AVG_QTY = Convert.ToDecimal(ShukkaData.Average().ToString("########.000"));
                details.TOTAL_QTY = Convert.ToDecimal(ShukkaData.Sum().ToString("########.000")); //ShukkaData.Sum();

                detailsList.Add(details); // Add the new object to the list
            }
            ProductDetails detailsThirdRow = new ProductDetails();
            int i = 0;
            var item = detailsList[i + 1];
            decimal month1Qty = Convert.ToDecimal(detailsList[i].MONTH1_QTY);
            decimal previousMonth1Qty = Convert.ToDecimal(detailsList[i + 1].MONTH1_QTY);
            decimal month2Qty = Convert.ToDecimal(detailsList[i].MONTH2_QTY);
            decimal previousMonth2Qty = Convert.ToDecimal(detailsList[i + 1].MONTH2_QTY);
            decimal month3Qty = Convert.ToDecimal(detailsList[i].MONTH3_QTY);
            decimal previousMonth3Qty = Convert.ToDecimal(detailsList[i + 1].MONTH3_QTY);
            decimal month4Qty = Convert.ToDecimal(detailsList[i].MONTH4_QTY);
            decimal previousMonth4Qty = Convert.ToDecimal(detailsList[i + 1].MONTH4_QTY);
            decimal month5Qty = Convert.ToDecimal(detailsList[i].MONTH5_QTY);
            decimal previousMonth5Qty = Convert.ToDecimal(detailsList[i + 1].MONTH5_QTY);
            decimal month6Qty = Convert.ToDecimal(detailsList[i].MONTH6_QTY);
            decimal previousMonth6Qty = Convert.ToDecimal(detailsList[i + 1].MONTH6_QTY);
            decimal month7Qty = Convert.ToDecimal(detailsList[i].MONTH7_QTY);
            decimal previousMonth7Qty = Convert.ToDecimal(detailsList[i + 1].MONTH7_QTY);
            decimal month8Qty = Convert.ToDecimal(detailsList[i].MONTH8_QTY);
            decimal previousMonth8Qty = Convert.ToDecimal(detailsList[i + 1].MONTH8_QTY);
            decimal month9Qty = Convert.ToDecimal(detailsList[i].MONTH9_QTY);
            decimal previousMonth9Qty = Convert.ToDecimal(detailsList[i + 1].MONTH9_QTY);
            decimal month10Qty = Convert.ToDecimal(detailsList[i].MONTH10_QTY);
            decimal previousMonth10Qty = Convert.ToDecimal(detailsList[i + 1].MONTH10_QTY);
            decimal month11Qty = Convert.ToDecimal(detailsList[i].MONTH11_QTY);
            decimal previousMonth11Qty = Convert.ToDecimal(detailsList[i + 1].MONTH11_QTY);
            decimal month12Qty = Convert.ToDecimal(detailsList[i].MONTH12_QTY);
            decimal previousMonth12Qty = Convert.ToDecimal(detailsList[i + 1].MONTH12_QTY);
            decimal monthMaxQty = Convert.ToDecimal(detailsList[i].MAX_QTY);
            decimal previousMonthMaxQty = Convert.ToDecimal(detailsList[i + 1].MAX_QTY);
            decimal monthMinQty = Convert.ToDecimal(detailsList[i].MIN_QTY);
            decimal previousMonthMinQty = Convert.ToDecimal(detailsList[i + 1].MIN_QTY);
            decimal monthAvgQty = Convert.ToDecimal(detailsList[i].AVG_QTY);
            decimal previousMonthAvgQty = Convert.ToDecimal(detailsList[i + 1].AVG_QTY);
            decimal monthTotQty = Convert.ToDecimal(detailsList[i].TOTAL_QTY);
            decimal previousMonthTotQty = Convert.ToDecimal(detailsList[i + 1].TOTAL_QTY);


            if (previousMonth1Qty != 0)
            {
                detailsThirdRow.MONTH1_QTY = 100 * month1Qty / previousMonth1Qty;
            }
            else
            {
                detailsThirdRow.MONTH1_QTY = 0;
            }
            if (previousMonth2Qty != 0)
            {
                detailsThirdRow.MONTH2_QTY = 100 * month2Qty / previousMonth2Qty;
            }
            else
            {
                detailsThirdRow.MONTH2_QTY = 0;
            }
            if (previousMonth3Qty != 0)
            {
                detailsThirdRow.MONTH3_QTY = 100 * month3Qty / previousMonth3Qty;
            }
            else
            {
                detailsThirdRow.MONTH3_QTY = 0;
            }
            if (previousMonth4Qty != 0)
            {
                detailsThirdRow.MONTH4_QTY = 100 * month4Qty / previousMonth4Qty;
            }
            else
            {
                detailsThirdRow.MONTH4_QTY = 0;
            }
            if (previousMonth5Qty != 0)
            {
                detailsThirdRow.MONTH5_QTY = 100 * month5Qty / previousMonth5Qty;
            }
            else
            {
                detailsThirdRow.MONTH5_QTY = 0;
            }
            if (previousMonth6Qty != 0)
            {
                detailsThirdRow.MONTH6_QTY = 100 * month6Qty / previousMonth6Qty;
            }
            else
            {
                detailsThirdRow.MONTH6_QTY = 0;
            }
            if (previousMonth7Qty != 0)
            {
                detailsThirdRow.MONTH7_QTY = 100 * month7Qty / previousMonth7Qty;
            }
            else
            {
                detailsThirdRow.MONTH7_QTY = 0;
            }
            if (previousMonth8Qty != 0)
            {
                detailsThirdRow.MONTH8_QTY = 100 * month8Qty / previousMonth8Qty;
            }
            else
            {
                detailsThirdRow.MONTH8_QTY = 0;
            }
            if (previousMonth9Qty != 0)
            {
                detailsThirdRow.MONTH9_QTY = 100 * month9Qty / previousMonth9Qty;
            }
            else
            {
                detailsThirdRow.MONTH9_QTY = 0;
            }
            if (previousMonth10Qty != 0)
            {
                detailsThirdRow.MONTH10_QTY = 100 * month10Qty / previousMonth10Qty;
            }
            else
            {
                detailsThirdRow.MONTH10_QTY = 0;
            }
            if (previousMonth11Qty != 0)
            {
                detailsThirdRow.MONTH11_QTY = 100 * month11Qty / previousMonth11Qty;
            }
            else
            {
                detailsThirdRow.MONTH11_QTY = 0;
            }
            if (previousMonth12Qty != 0)
            {
                detailsThirdRow.MONTH12_QTY = 100 * month12Qty / previousMonth12Qty;
            }
            else
            {
                detailsThirdRow.MONTH12_QTY = 0;
            }
            if (previousMonthMaxQty != 0)
            {
                detailsThirdRow.MAX_QTY = 100 * monthMaxQty / previousMonthMaxQty;
            }
            else
            {
                detailsThirdRow.MAX_QTY = 0;
            }
            if (previousMonthMinQty != 0)
            {
                detailsThirdRow.MIN_QTY = 100 * monthMinQty / previousMonthMinQty;
            }
            else
            {
                detailsThirdRow.MIN_QTY = 0;
            }
            if (previousMonthAvgQty != 0)
            {
                detailsThirdRow.AVG_QTY = 100 * monthAvgQty / previousMonthAvgQty;
            }
            else
            {
                detailsThirdRow.AVG_QTY = 0;
            }
            if (previousMonthTotQty != 0)
            {
                detailsThirdRow.TOTAL_QTY = 100 * monthTotQty / previousMonthTotQty;
            }
            else
            {
                detailsThirdRow.TOTAL_QTY = 0;
            }
            detailsList.Add(detailsThirdRow);


            return detailsList;

        }

        /// <summary>
        ///Get Prod Stock Qty Customer Wise.
        ///Created by shobhana[21 March 2025]
        /// </summary>
        /// <returns>
        /// Month Wise Product Stock list.
        /// </returns>
        //public List<ProductDetails> GetProdStockQtyCustomerWise(string customerCD)
        //{

        //    DateTime today = DateTime.Now;
        //    int currentMonth = today.Month;
        //    List<ProductDetails> detailsList = new List<ProductDetails>();
        //    List<decimal> ProdStockData = new List<decimal>();

        //    today = today.AddYears(-1);
        //    int lastYear = Convert.ToInt32(today.Year);
        //    today = today.AddYears(-1);
        //    int lasttolastYear = Convert.ToInt32(today.Year);
        //    int getMonth = 0;
        //    int getYear = 0;

        //    for (int j = 1; j <= 2; j++)
        //    {
        //        ProductDetails details = new ProductDetails();

        //        for (int i = 1; i <= 12; i++)
        //        {
        //            string getDay = Convert.ToString(DateTime.Now.Day.ToString("00"));

        //            if (i != 1)
        //            {
        //                today = today.AddMonths(+1);
        //                getMonth = Convert.ToInt16(today.Month);
        //                getYear = Convert.ToInt32(today.Year);
        //            }
        //            else
        //            {
        //                getMonth = currentMonth;
        //                getYear = Convert.ToInt32(today.Year);
        //            }
        //            getDay = today.Year.ToString().Substring(2, 2) + getMonth.ToString("00") + getDay;
        //            StringBuilder selectQuery = new StringBuilder();
        //            selectQuery.Append(" SELECT ROUND(NVL(sum(PROD_STOCK_QTY), '0') / 1000) ");
        //            selectQuery.Append(" FROM BCH_MONTH_END_PROD_STOCK BMS");
        //            selectQuery.Append(" JOIN MST_PRODUCT P ON P.ROOT_VER_PRODUCT_KEY=BMS.ROOT_VER_PRODUCT_KEY");
        //            selectQuery.Append(" WHERE P.CUSTOMER_CD = " + "'" + customerCD + "'");
        //            selectQuery.Append(j == 1 ? " AND YEAR = " + lastYear : " AND YEAR = " + lasttolastYear);
        //            selectQuery.Append(" AND MONTH = " + getMonth);

        //            decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
        //            ProdStockData.Add(getQty);
        //            // Assign the Qty to the corresponding month property
        //            switch (i)
        //            {
        //                case 1: details.MONTH1_QTY = getQty; break;
        //                case 2: details.MONTH2_QTY = getQty; break;
        //                case 3: details.MONTH3_QTY = getQty; break;
        //                case 4: details.MONTH4_QTY = getQty; break;
        //                case 5: details.MONTH5_QTY = getQty; break;
        //                case 6: details.MONTH6_QTY = getQty; break;
        //                case 7: details.MONTH7_QTY = getQty; break;
        //                case 8: details.MONTH8_QTY = getQty; break;
        //                case 9: details.MONTH9_QTY = getQty; break;
        //                case 10: details.MONTH10_QTY = getQty; break;
        //                case 11: details.MONTH11_QTY = getQty; break;
        //                case 12: details.MONTH12_QTY = getQty; break;
        //            }
        //        }
        //        // Compute min, max, avg, total values
        //        details.MAX_QTY = ProdStockData.Max();
        //        details.MIN_QTY = ProdStockData.Min();
        //        details.AVG_QTY = Convert.ToDecimal(ProdStockData.Average().ToString("########.000"));
        //        details.TOTAL_QTY = Convert.ToDecimal(ProdStockData.Sum().ToString("########.000"));

        //        detailsList.Add(details);
        //    }

        //    return detailsList;

        //}
        public List<ProductDetails> GetProdStockQtyCustomerWise(string customerCD)
        {

            int gridRowCnt = 2;
            int gridColCnt = 12;
            DateTime today = DateTime.Now;
            //int currentMonth = today.Month;
            List<ProductDetails> detailsList = new List<ProductDetails>();


            DateTime lastYearDate = today.AddYears(-1);
            int lastYear = Convert.ToInt32(lastYearDate.Year);
            DateTime lastToLastYearDate = lastYearDate.AddYears(-1);
            int lastTolastYear = Convert.ToInt32(lastToLastYearDate.Year);
            int getMonth = 0;
            int getYear = 0;

            for (int j = 1; j <= gridRowCnt; j++)
            {
                List<decimal> ProdStockData = new List<decimal>();
                ProductDetails details = new ProductDetails(); // Instantiate new object inside the loop


                for (int p = 0; p <= gridColCnt - 1; p++)
                {
                    if (j == 1)
                    {
                        DateTime thisMonthDate = lastYearDate.AddMonths(p);
                        getMonth = Convert.ToInt16(thisMonthDate.Month);
                        getYear = Convert.ToInt32(thisMonthDate.Year);
                    }
                    else
                    {
                        DateTime thisMonthDate = lastToLastYearDate.AddMonths(p);
                        getMonth = Convert.ToInt16(thisMonthDate.Month);
                        getYear = Convert.ToInt32(thisMonthDate.Year);
                    }

                    //StringBuilder selectQuery = new StringBuilder();
                    //selectQuery.Append(" SELECT ROUND(NVL(sum(PROD_STOCK_QTY), '0') / 1000) ");
                    //selectQuery.Append(" FROM BCH_MONTH_END_PROD_STOCK BMS");
                    //selectQuery.Append(" JOIN MST_PRODUCT P ON P.ROOT_VER_PRODUCT_KEY=BMS.ROOT_VER_PRODUCT_KEY");
                    //selectQuery.Append(" WHERE P.CUSTOMER_CD = " + "'" + customerCD + "'");
                    //selectQuery.Append(" AND YEAR = " + getYear + "");
                    //selectQuery.Append(" AND MONTH = " + getMonth + "");
                    StringBuilder selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT NVL(SUM(PROD_STOCK_QTY), '0') / 1000  AS TOTALPRODSTOCKQTY");
                    selectQuery.Append(" FROM ");
                    selectQuery.Append(" BCH_MONTH_END_PROD_STOCK MEPS WHERE MEPS.ROOT_VER_PRODUCT_KEY IN (   SELECT DISTINCT ");
                    selectQuery.Append(" P.ROOT_VER_PRODUCT_KEY ");
                    selectQuery.Append(" FROM MST_PRODUCT P ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" P.CUSTOMER_CD = '" + customerCD + "' AND VER = (SELECT MAX(VER) FROM MST_PRODUCT WHERE CUSTOMER_CD ='" + customerCD + "') )   ");
                    selectQuery.Append(" AND YEAR = " + getYear + " AND MONTH = " + getMonth + " ");
                    string query = selectQuery.ToString();
                    object result = ExecuteScalar(query);

                    decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
                    ProdStockData.Add(getQty);
                    // Assign the Qty to the corresponding month property
                    switch (p)
                    {
                        case 0: details.MONTH1_QTY = getQty; break;
                        case 1: details.MONTH2_QTY = getQty; break;
                        case 2: details.MONTH3_QTY = getQty; break;
                        case 3: details.MONTH4_QTY = getQty; break;
                        case 4: details.MONTH5_QTY = getQty; break;
                        case 5: details.MONTH6_QTY = getQty; break;
                        case 6: details.MONTH7_QTY = getQty; break;
                        case 7: details.MONTH8_QTY = getQty; break;
                        case 8: details.MONTH9_QTY = getQty; break;
                        case 9: details.MONTH10_QTY = getQty; break;
                        case 10: details.MONTH11_QTY = getQty; break;
                        case 11: details.MONTH12_QTY = getQty; break;
                    }
                }
                // Compute min, max, avg, total values
                details.MAX_QTY = ProdStockData.Max();
                details.MIN_QTY = ProdStockData.Min();
                details.AVG_QTY = Convert.ToDecimal(ProdStockData.Average().ToString("########.000"));
                details.TOTAL_QTY = Convert.ToDecimal(ProdStockData.Sum().ToString("########.000"));

                detailsList.Add(details);
            }
            ProductDetails detailsThirdRow = new ProductDetails();
            int i = 0;
            var item = detailsList[i + 1];
            decimal month1Qty = Convert.ToDecimal(detailsList[i].MONTH1_QTY);
            decimal previousMonth1Qty = Convert.ToDecimal(detailsList[i + 1].MONTH1_QTY);
            decimal month2Qty = Convert.ToDecimal(detailsList[i].MONTH2_QTY);
            decimal previousMonth2Qty = Convert.ToDecimal(detailsList[i + 1].MONTH2_QTY);
            decimal month3Qty = Convert.ToDecimal(detailsList[i].MONTH3_QTY);
            decimal previousMonth3Qty = Convert.ToDecimal(detailsList[i + 1].MONTH3_QTY);
            decimal month4Qty = Convert.ToDecimal(detailsList[i].MONTH4_QTY);
            decimal previousMonth4Qty = Convert.ToDecimal(detailsList[i + 1].MONTH4_QTY);
            decimal month5Qty = Convert.ToDecimal(detailsList[i].MONTH5_QTY);
            decimal previousMonth5Qty = Convert.ToDecimal(detailsList[i + 1].MONTH5_QTY);
            decimal month6Qty = Convert.ToDecimal(detailsList[i].MONTH6_QTY);
            decimal previousMonth6Qty = Convert.ToDecimal(detailsList[i + 1].MONTH6_QTY);
            decimal month7Qty = Convert.ToDecimal(detailsList[i].MONTH7_QTY);
            decimal previousMonth7Qty = Convert.ToDecimal(detailsList[i + 1].MONTH7_QTY);
            decimal month8Qty = Convert.ToDecimal(detailsList[i].MONTH8_QTY);
            decimal previousMonth8Qty = Convert.ToDecimal(detailsList[i + 1].MONTH8_QTY);
            decimal month9Qty = Convert.ToDecimal(detailsList[i].MONTH9_QTY);
            decimal previousMonth9Qty = Convert.ToDecimal(detailsList[i + 1].MONTH9_QTY);
            decimal month10Qty = Convert.ToDecimal(detailsList[i].MONTH10_QTY);
            decimal previousMonth10Qty = Convert.ToDecimal(detailsList[i + 1].MONTH10_QTY);
            decimal month11Qty = Convert.ToDecimal(detailsList[i].MONTH11_QTY);
            decimal previousMonth11Qty = Convert.ToDecimal(detailsList[i + 1].MONTH11_QTY);
            decimal month12Qty = Convert.ToDecimal(detailsList[i].MONTH12_QTY);
            decimal previousMonth12Qty = Convert.ToDecimal(detailsList[i + 1].MONTH12_QTY);
            decimal monthMaxQty = Convert.ToDecimal(detailsList[i].MAX_QTY);
            decimal previousMonthMaxQty = Convert.ToDecimal(detailsList[i + 1].MAX_QTY);
            decimal monthMinQty = Convert.ToDecimal(detailsList[i].MIN_QTY);
            decimal previousMonthMinQty = Convert.ToDecimal(detailsList[i + 1].MIN_QTY);
            decimal monthAvgQty = Convert.ToDecimal(detailsList[i].AVG_QTY);
            decimal previousMonthAvgQty = Convert.ToDecimal(detailsList[i + 1].AVG_QTY);
            decimal monthTotQty = Convert.ToDecimal(detailsList[i].TOTAL_QTY);
            decimal previousMonthTotQty = Convert.ToDecimal(detailsList[i + 1].TOTAL_QTY);


            if (previousMonth1Qty != 0)
            {
                detailsThirdRow.MONTH1_QTY = 100 * month1Qty / previousMonth1Qty;
            }
            else
            {
                detailsThirdRow.MONTH1_QTY = 0;
            }
            if (previousMonth2Qty != 0)
            {
                detailsThirdRow.MONTH2_QTY = 100 * month2Qty / previousMonth2Qty;
            }
            else
            {
                detailsThirdRow.MONTH2_QTY = 0;
            }
            if (previousMonth3Qty != 0)
            {
                detailsThirdRow.MONTH3_QTY = 100 * month3Qty / previousMonth3Qty;
            }
            else
            {
                detailsThirdRow.MONTH3_QTY = 0;
            }
            if (previousMonth4Qty != 0)
            {
                detailsThirdRow.MONTH4_QTY = 100 * month4Qty / previousMonth4Qty;
            }
            else
            {
                detailsThirdRow.MONTH4_QTY = 0;
            }
            if (previousMonth5Qty != 0)
            {
                detailsThirdRow.MONTH5_QTY = 100 * month5Qty / previousMonth5Qty;
            }
            else
            {
                detailsThirdRow.MONTH5_QTY = 0;
            }
            if (previousMonth6Qty != 0)
            {
                detailsThirdRow.MONTH6_QTY = 100 * month6Qty / previousMonth6Qty;
            }
            else
            {
                detailsThirdRow.MONTH6_QTY = 0;
            }
            if (previousMonth7Qty != 0)
            {
                detailsThirdRow.MONTH7_QTY = 100 * month7Qty / previousMonth7Qty;
            }
            else
            {
                detailsThirdRow.MONTH7_QTY = 0;
            }
            if (previousMonth8Qty != 0)
            {
                detailsThirdRow.MONTH8_QTY = 100 * month8Qty / previousMonth8Qty;
            }
            else
            {
                detailsThirdRow.MONTH8_QTY = 0;
            }
            if (previousMonth9Qty != 0)
            {
                detailsThirdRow.MONTH9_QTY = 100 * month9Qty / previousMonth9Qty;
            }
            else
            {
                detailsThirdRow.MONTH9_QTY = 0;
            }
            if (previousMonth10Qty != 0)
            {
                detailsThirdRow.MONTH10_QTY = 100 * month10Qty / previousMonth10Qty;
            }
            else
            {
                detailsThirdRow.MONTH10_QTY = 0;
            }
            if (previousMonth11Qty != 0)
            {
                detailsThirdRow.MONTH11_QTY = 100 * month11Qty / previousMonth11Qty;
            }
            else
            {
                detailsThirdRow.MONTH11_QTY = 0;
            }
            if (previousMonth12Qty != 0)
            {
                detailsThirdRow.MONTH12_QTY = 100 * month12Qty / previousMonth12Qty;
            }
            else
            {
                detailsThirdRow.MONTH12_QTY = 0;
            }
            if (previousMonthMaxQty != 0)
            {
                detailsThirdRow.MAX_QTY = 100 * monthMaxQty / previousMonthMaxQty;
            }
            else
            {
                detailsThirdRow.MAX_QTY = 0;
            }
            if (previousMonthMinQty != 0)
            {
                detailsThirdRow.MIN_QTY = 100 * monthMinQty / previousMonthMinQty;
            }
            else
            {
                detailsThirdRow.MIN_QTY = 0;
            }
            if (previousMonthAvgQty != 0)
            {
                detailsThirdRow.AVG_QTY = 100 * monthAvgQty / previousMonthAvgQty;
            }
            else
            {
                detailsThirdRow.AVG_QTY = 0;
            }
            if (previousMonthTotQty != 0)
            {
                detailsThirdRow.TOTAL_QTY = 100 * monthTotQty / previousMonthTotQty;
            }
            else
            {
                detailsThirdRow.TOTAL_QTY = 0;
            }
            detailsList.Add(detailsThirdRow);
            return detailsList;

        }

        /// <summary>
        /// Get Pending Product Stock Qty Customer CD Wise.
        /// Created by shobhana[21 March 2025]
        /// </summary>
        /// <returns>
        /// Month Wise Pending Stock Qty List return.
        /// </returns>
        //public List<ProductDetails> GetPendingStockQtyCustomerWise(string customerCD)
        //{

        //    DateTime today = DateTime.Now;
        //    int currentMonth = today.Month;
        //    List<ProductDetails> detailsList = new List<ProductDetails>();
        //    List<decimal> ProdStockData = new List<decimal>();

        //    today = today.AddYears(-1);
        //    int lastYear = Convert.ToInt32(today.Year);
        //    today = today.AddYears(-1);
        //    int lasttolastYear = Convert.ToInt32(today.Year);
        //    int getMonth = 0;
        //    int getYear = 0;

        //    for (int j = 1; j <= 2; j++)
        //    {
        //        ProductDetails details = new ProductDetails();
        //        for (int i = 1; i <= 12; i++)
        //        {
        //            string getDay = Convert.ToString(DateTime.Now.Day.ToString("00"));

        //            if (i != 1)
        //            {
        //                today = today.AddMonths(+1);
        //                getMonth = Convert.ToInt16(today.Month);
        //                getYear = Convert.ToInt32(today.Year);
        //            }
        //            else
        //            {
        //                getMonth = currentMonth;
        //                getYear = Convert.ToInt32(today.Year);
        //            }
        //            getDay = today.Year.ToString().Substring(2, 2) + getMonth.ToString("00") + getDay;

        //            StringBuilder selectQuery = new StringBuilder();
        //            selectQuery.Append(" SELECT ROUND(NVL(sum(PROD_STOCK_QTY), '0') / 1000) ");
        //            selectQuery.Append(" FROM BCH_MONTH_END_PROD_STOCK BMS");
        //            selectQuery.Append(" JOIN MST_PRODUCT P ON P.ROOT_VER_PRODUCT_KEY=BMS.ROOT_VER_PRODUCT_KEY");
        //            selectQuery.Append(" WHERE P.CUSTOMER_CD = " + "'" + customerCD + "'");
        //            selectQuery.Append(j == 1 ? " AND YEAR = " + lastYear : " AND YEAR = " + lasttolastYear);

        //            selectQuery.Append(" AND MONTH = " + getMonth);

        //            decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
        //            ProdStockData.Add(getQty);

        //            // Assign the quantity to the corresponding month property
        //            switch (i)
        //            {
        //                case 1: details.MONTH1_QTY = getQty; break;
        //                case 2: details.MONTH2_QTY = getQty; break;
        //                case 3: details.MONTH3_QTY = getQty; break;
        //                case 4: details.MONTH4_QTY = getQty; break;
        //                case 5: details.MONTH5_QTY = getQty; break;
        //                case 6: details.MONTH6_QTY = getQty; break;
        //                case 7: details.MONTH7_QTY = getQty; break;
        //                case 8: details.MONTH8_QTY = getQty; break;
        //                case 9: details.MONTH9_QTY = getQty; break;
        //                case 10: details.MONTH10_QTY = getQty; break;
        //                case 11: details.MONTH11_QTY = getQty; break;
        //                case 12: details.MONTH12_QTY = getQty; break;
        //            }
        //        }

        //        // Compute min, max, avg, total values
        //        details.MAX_QTY = ProdStockData.Max();
        //        details.MIN_QTY = ProdStockData.Min();
        //        details.AVG_QTY = Convert.ToDecimal(ProdStockData.Average().ToString("########.000"));
        //        details.TOTAL_QTY = Convert.ToDecimal(ProdStockData.Sum().ToString("########.000"));

        //        detailsList.Add(details);
        //    }

        //    return detailsList;

        //}
        public List<ProductDetails> GetPendingStockQtyCustomerWise(string customerCD)
        {

            int gridRowCnt = 2;
            int gridColCnt = 12;
            DateTime today = DateTime.Now;
            //int currentMonth = today.Month;
            List<ProductDetails> detailsList = new List<ProductDetails>();


            DateTime lastYearDate = today.AddYears(-1);
            int lastYear = Convert.ToInt32(lastYearDate.Year);
            DateTime lastToLastYearDate = lastYearDate.AddYears(-1);
            int lastTolastYear = Convert.ToInt32(lastToLastYearDate.Year);
            int getMonth = 0;
            int getYear = 0;

            for (int j = 1; j <= gridRowCnt; j++)
            {
                List<decimal> pendingStockData = new List<decimal>();
                ProductDetails details = new ProductDetails(); // Instantiate new object inside the loop


                for (int p = 0; p <= gridColCnt - 1; p++)
                {
                    string getDay = Convert.ToString(DateTime.Now.Day.ToString("00"));
                    if (j == 1)
                    {
                        DateTime thisMonthDate = lastYearDate.AddMonths(p);
                        getMonth = Convert.ToInt16(thisMonthDate.Month);
                        getYear = Convert.ToInt32(thisMonthDate.Year);
                    }
                    else
                    {
                        DateTime thisMonthDate = lastToLastYearDate.AddMonths(p);
                        getMonth = Convert.ToInt16(thisMonthDate.Month);
                        getYear = Convert.ToInt32(thisMonthDate.Year);
                    }

                    getDay = getYear.ToString().Substring(2, 2) + getMonth.ToString("00") + getDay;
                    StringBuilder selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT  ");
                    selectQuery.Append(" SUM(TO_CHAR(NVL(PENDSTOCK_ABOVE90, 0) / 1000, '99999999.9')) AS REMAIN_QTY ");
                    selectQuery.Append(" FROM (  ");
                    selectQuery.Append(" SELECT  ");
                    selectQuery.Append(" CASE WHEN TO_DATE( " + getDay + ", 'YYMMDD') - TO_DATE(STOCK_DATE, 'YYMMDD') > 90 THEN ");
                    selectQuery.Append("  SUM(CURR_STOCK) ELSE 0 END AS PENDSTOCK_ABOVE90 ");
                    //selectQuery.Append(" TO_NUMBER(TO_CHAR(NVL(DISPATCH_QTY,0)/1000,'99999999.999'))  ");
                    selectQuery.Append(" FROM ");
                    selectQuery.Append(" BCH_PROD_STOCK_HOST ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" CUSTOMER_CD = " + "'" + customerCD + "'");
                    selectQuery.Append(" GROUP BY ");
                    selectQuery.Append(" STOCK_DATE ");
                    selectQuery.Append(" )  ");
                    //selectQuery.Append(" AND YEAR = " + getYear);
                    //selectQuery.Append(" AND MONTH =  " + getMonth);
                    decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
                    pendingStockData.Add(getQty);

                    // Assign the quantity to the corresponding month property
                    switch (p)
                    {
                        case 0: details.MONTH1_QTY = getQty; break;
                        case 1: details.MONTH2_QTY = getQty; break;
                        case 2: details.MONTH3_QTY = getQty; break;
                        case 3: details.MONTH4_QTY = getQty; break;
                        case 4: details.MONTH5_QTY = getQty; break;
                        case 5: details.MONTH6_QTY = getQty; break;
                        case 6: details.MONTH7_QTY = getQty; break;
                        case 7: details.MONTH8_QTY = getQty; break;
                        case 8: details.MONTH9_QTY = getQty; break;
                        case 9: details.MONTH10_QTY = getQty; break;
                        case 10: details.MONTH11_QTY = getQty; break;
                        case 11: details.MONTH12_QTY = getQty; break;
                    }
                }

                // Compute min, max, avg, total values
                details.MAX_QTY = pendingStockData.Max();
                details.MIN_QTY = pendingStockData.Min();
                details.AVG_QTY = Convert.ToDecimal(pendingStockData.Average().ToString("########.000"));
                details.TOTAL_QTY = Convert.ToDecimal(pendingStockData.Sum().ToString("########.000"));

                detailsList.Add(details);
            }
            ProductDetails detailsThirdRow = new ProductDetails();
            int i = 0;
            var item = detailsList[i + 1];
            decimal month1Qty = Convert.ToDecimal(detailsList[i].MONTH1_QTY);
            decimal previousMonth1Qty = Convert.ToDecimal(detailsList[i + 1].MONTH1_QTY);
            decimal month2Qty = Convert.ToDecimal(detailsList[i].MONTH2_QTY);
            decimal previousMonth2Qty = Convert.ToDecimal(detailsList[i + 1].MONTH2_QTY);
            decimal month3Qty = Convert.ToDecimal(detailsList[i].MONTH3_QTY);
            decimal previousMonth3Qty = Convert.ToDecimal(detailsList[i + 1].MONTH3_QTY);
            decimal month4Qty = Convert.ToDecimal(detailsList[i].MONTH4_QTY);
            decimal previousMonth4Qty = Convert.ToDecimal(detailsList[i + 1].MONTH4_QTY);
            decimal month5Qty = Convert.ToDecimal(detailsList[i].MONTH5_QTY);
            decimal previousMonth5Qty = Convert.ToDecimal(detailsList[i + 1].MONTH5_QTY);
            decimal month6Qty = Convert.ToDecimal(detailsList[i].MONTH6_QTY);
            decimal previousMonth6Qty = Convert.ToDecimal(detailsList[i + 1].MONTH6_QTY);
            decimal month7Qty = Convert.ToDecimal(detailsList[i].MONTH7_QTY);
            decimal previousMonth7Qty = Convert.ToDecimal(detailsList[i + 1].MONTH7_QTY);
            decimal month8Qty = Convert.ToDecimal(detailsList[i].MONTH8_QTY);
            decimal previousMonth8Qty = Convert.ToDecimal(detailsList[i + 1].MONTH8_QTY);
            decimal month9Qty = Convert.ToDecimal(detailsList[i].MONTH9_QTY);
            decimal previousMonth9Qty = Convert.ToDecimal(detailsList[i + 1].MONTH9_QTY);
            decimal month10Qty = Convert.ToDecimal(detailsList[i].MONTH10_QTY);
            decimal previousMonth10Qty = Convert.ToDecimal(detailsList[i + 1].MONTH10_QTY);
            decimal month11Qty = Convert.ToDecimal(detailsList[i].MONTH11_QTY);
            decimal previousMonth11Qty = Convert.ToDecimal(detailsList[i + 1].MONTH11_QTY);
            decimal month12Qty = Convert.ToDecimal(detailsList[i].MONTH12_QTY);
            decimal previousMonth12Qty = Convert.ToDecimal(detailsList[i + 1].MONTH12_QTY);
            decimal monthMaxQty = Convert.ToDecimal(detailsList[i].MAX_QTY);
            decimal previousMonthMaxQty = Convert.ToDecimal(detailsList[i + 1].MAX_QTY);
            decimal monthMinQty = Convert.ToDecimal(detailsList[i].MIN_QTY);
            decimal previousMonthMinQty = Convert.ToDecimal(detailsList[i + 1].MIN_QTY);
            decimal monthAvgQty = Convert.ToDecimal(detailsList[i].AVG_QTY);
            decimal previousMonthAvgQty = Convert.ToDecimal(detailsList[i + 1].AVG_QTY);
            decimal monthTotQty = Convert.ToDecimal(detailsList[i].TOTAL_QTY);
            decimal previousMonthTotQty = Convert.ToDecimal(detailsList[i + 1].TOTAL_QTY);


            if (previousMonth1Qty != 0)
            {
                detailsThirdRow.MONTH1_QTY = 100 * month1Qty / previousMonth1Qty;
            }
            else
            {
                detailsThirdRow.MONTH1_QTY = 0;
            }
            if (previousMonth2Qty != 0)
            {
                detailsThirdRow.MONTH2_QTY = 100 * month2Qty / previousMonth2Qty;
            }
            else
            {
                detailsThirdRow.MONTH2_QTY = 0;
            }
            if (previousMonth3Qty != 0)
            {
                detailsThirdRow.MONTH3_QTY = 100 * month3Qty / previousMonth3Qty;
            }
            else
            {
                detailsThirdRow.MONTH3_QTY = 0;
            }
            if (previousMonth4Qty != 0)
            {
                detailsThirdRow.MONTH4_QTY = 100 * month4Qty / previousMonth4Qty;
            }
            else
            {
                detailsThirdRow.MONTH4_QTY = 0;
            }
            if (previousMonth5Qty != 0)
            {
                detailsThirdRow.MONTH5_QTY = 100 * month5Qty / previousMonth5Qty;
            }
            else
            {
                detailsThirdRow.MONTH5_QTY = 0;
            }
            if (previousMonth6Qty != 0)
            {
                detailsThirdRow.MONTH6_QTY = 100 * month6Qty / previousMonth6Qty;
            }
            else
            {
                detailsThirdRow.MONTH6_QTY = 0;
            }
            if (previousMonth7Qty != 0)
            {
                detailsThirdRow.MONTH7_QTY = 100 * month7Qty / previousMonth7Qty;
            }
            else
            {
                detailsThirdRow.MONTH7_QTY = 0;
            }
            if (previousMonth8Qty != 0)
            {
                detailsThirdRow.MONTH8_QTY = 100 * month8Qty / previousMonth8Qty;
            }
            else
            {
                detailsThirdRow.MONTH8_QTY = 0;
            }
            if (previousMonth9Qty != 0)
            {
                detailsThirdRow.MONTH9_QTY = 100 * month9Qty / previousMonth9Qty;
            }
            else
            {
                detailsThirdRow.MONTH9_QTY = 0;
            }
            if (previousMonth10Qty != 0)
            {
                detailsThirdRow.MONTH10_QTY = 100 * month10Qty / previousMonth10Qty;
            }
            else
            {
                detailsThirdRow.MONTH10_QTY = 0;
            }
            if (previousMonth11Qty != 0)
            {
                detailsThirdRow.MONTH11_QTY = 100 * month11Qty / previousMonth11Qty;
            }
            else
            {
                detailsThirdRow.MONTH11_QTY = 0;
            }
            if (previousMonth12Qty != 0)
            {
                detailsThirdRow.MONTH12_QTY = 100 * month12Qty / previousMonth12Qty;
            }
            else
            {
                detailsThirdRow.MONTH12_QTY = 0;
            }
            if (previousMonthMaxQty != 0)
            {
                detailsThirdRow.MAX_QTY = 100 * monthMaxQty / previousMonthMaxQty;
            }
            else
            {
                detailsThirdRow.MAX_QTY = 0;
            }
            if (previousMonthMinQty != 0)
            {
                detailsThirdRow.MIN_QTY = 100 * monthMinQty / previousMonthMinQty;
            }
            else
            {
                detailsThirdRow.MIN_QTY = 0;
            }
            if (previousMonthAvgQty != 0)
            {
                detailsThirdRow.AVG_QTY = 100 * monthAvgQty / previousMonthAvgQty;
            }
            else
            {
                detailsThirdRow.AVG_QTY = 0;
            }
            if (previousMonthTotQty != 0)
            {
                detailsThirdRow.TOTAL_QTY = 100 * monthTotQty / previousMonthTotQty;
            }
            else
            {
                detailsThirdRow.TOTAL_QTY = 0;
            }
            detailsList.Add(detailsThirdRow);
            return detailsList;

        }

    }
}
