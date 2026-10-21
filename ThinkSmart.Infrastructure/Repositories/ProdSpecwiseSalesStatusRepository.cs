using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProdSpecwiseSalesStatusRepository : BaseRepository ,IProdSpecwiseSalesStatusRepository
    {
        public ProdSpecwiseSalesStatusRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            
        }
        public ProdSpecificationDetails GetProdSpecDetailsByProductCD(string productCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            // selectQuery.Append(" SELECT  ");
            // selectQuery.Append(" PRODUCT_HOST_CD,  ");
            // selectQuery.Append(" PROD_SPEC_STRING,  ");
            // selectQuery.Append(" PRODUCT_NAME,  ");
            // selectQuery.Append(" INTERNAL_NAME,  ");
            // selectQuery.Append(" SHAPE,  ");
            // selectQuery.Append(" DIM_D,  ");
            // selectQuery.Append(" LENGTH,  ");
            // selectQuery.Append(" CONDITION,  ");
            // selectQuery.Append(" CUSTOMER_NAME_STRING,  ");
            // selectQuery.Append(" DIM_TOL_STRING,  ");
            // selectQuery.Append(" SUM(TO_CHAR(PENDSTOCK_ABOVE30/1000,'99999999.999')) AS PENDSTOCK_ABOVE30, ");
            // selectQuery.Append(" SUM(TO_CHAR(PENDSTOCK_ABOVE60/1000,'99999999.999')) AS PENDSTOCK_ABOVE60, ");
            // selectQuery.Append(" SUM(TO_CHAR(PENDSTOCK_ABOVE90/1000,'99999999.999')) AS PENDSTOCK_ABOVE90, ");
            // selectQuery.Append(" SUM(TO_CHAR(KAKOIRAI_REM_QTY/1000,'99999999.999')) AS KAKOIRAI_REM_QTY, ");
            // selectQuery.Append(" SUM(TO_CHAR(PROD_STOCK_QTY/1000,'99999999.999')) AS PROD_STOCK_QTY, ");
            // selectQuery.Append(" SUM(TO_CHAR((KAKOIRAI_REM_QTY/1000) + (PROD_STOCK_QTY/1000),'99999999.999')) AS TOTAL_SUM, ");
            // selectQuery.Append(" SUM(TO_CHAR(BEFORE_ALLOC_PLANNED_QTY/1000,'99999999.99')) AS BEFORE_ALLOC_PLANNED_QTY, ");
            // selectQuery.Append(" SUM(TO_CHAR(IN_PROGRESS_QTY/1000,'99999999.99')) AS IN_PROGRESS_QTY ");
            // selectQuery.Append(" FROM  ");
            // selectQuery.Append(" (  ");
            // selectQuery.Append(" SELECT  ");
            // selectQuery.Append(" PRODSTOCK.PRODUCT_HOST_CD,  ");
            // selectQuery.Append(" PRODSTOCK.PROD_SPEC_1 || '-' || PRODSTOCK.PROD_SPEC_2 AS PROD_SPEC_STRING, ");
            // selectQuery.Append(" PRODSTOCK.PRODUCT_NAME,  ");
            // selectQuery.Append(" PRODSTOCK.INTERNAL_NAME,  ");
            // selectQuery.Append(" PRODSTOCK.SHAPE, ");
            // selectQuery.Append(" PRODSTOCK.DIM_D,  ");
            // selectQuery.Append(" PRODSTOCK.LENGTH,  ");
            // selectQuery.Append(" PRODSTOCK.CONDITION,  ");
            // selectQuery.Append(" NVL(CUST.SHORT_NAME,CUST.CUSTOMER_NAME) AS CUSTOMER_NAME_STRING, ");
            //// selectQuery.Append(" PR.PRODUCT_KUBUN,  ");
            // selectQuery.Append(" PRODSTOCK.DIM_TOL AS DIM_TOL_STRING,  ");
            // //selectQuery.Append(" TRIM((TO_CHAR(PR.LENGTH_TOL_MAX, 'S990D0')) || ',' || TRIM(TO_CHAR(PR.LENGTH_TOL_MIN, 'S990D0'))) LEN_TOL_STRING  ");
            // //selectQuery.Append("  PR.MAGARI_KUBUN, ");
            // //selectQuery.Append("  PR.MAGARI_SPAN, ");
            // //selectQuery.Append("  TRIM(TO_CHAR(PR.MAGARI_BEND_TOL, 'S0D099')) AS MAGARI_BEND_TOL ");
            // selectQuery.Append(" CASE WHEN ROUND(SYSDATE - TO_DATE(TO_CHAR(PRODSTOCK.STOCK_DATE),'YYMMDD')) > 30  AND ROUND(SYSDATE - TO_DATE(TO_CHAR(PRODSTOCK.STOCK_DATE),'YYMMDD')) <= 60 THEN ");
            // selectQuery.Append(" SUM(PRODSTOCK.CURR_STOCK) ELSE 0 END AS PENDSTOCK_ABOVE30, ");
            // selectQuery.Append(" CASE WHEN ROUND(SYSDATE - TO_DATE(TO_CHAR(PRODSTOCK.STOCK_DATE),'YYMMDD')) > 60  AND ROUND(SYSDATE - TO_DATE(TO_CHAR(PRODSTOCK.STOCK_DATE),'YYMMDD')) <= 90 THEN ");
            // selectQuery.Append(" SUM(PRODSTOCK.CURR_STOCK) ELSE 0 END AS PENDSTOCK_ABOVE60, ");
            // selectQuery.Append(" CASE WHEN ROUND(SYSDATE - TO_DATE(TO_CHAR(PRODSTOCK.STOCK_DATE),'YYMMDD')) > 90 THEN ");
            // selectQuery.Append(" SUM(PRODSTOCK.CURR_STOCK) ELSE 0 END AS PENDSTOCK_ABOVE90, ");

            // selectQuery.Append(" NVL(SUM(KAKO.KAKOUIARI_QTY) - SUM(KAKO.FI_WEIGHT), 0) KAKOIRAI_REM_QTY, ");
            // selectQuery.Append(" NVL(SUM(PRODSTOCK.CURR_STOCK), 0) PROD_STOCK_QTY, ");
            // selectQuery.Append(" NVL(SUM(KAKO.ACT_ALLOC_PENDNIG_WT), 0) BEFORE_ALLOC_PLANNED_QTY, ");
            // selectQuery.Append(" NVL(SUM(ACT_ALLOC_DONE_WT) - SUM(FI_WEIGHT), 0) IN_PROGRESS_QTY ");

            // selectQuery.Append(" FROM  ");
            // selectQuery.Append(" BCH_PROD_STOCK_HOST PRODSTOCK ");
            // selectQuery.Append(" LEFT JOIN BCH_KAKOUIRAI_HOST KAKO ON PRODSTOCK.PRODUCT_HOST_CD = KAKO.PRODUCT_HOST_CD ");
            // selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON PRODSTOCK.CUSTOMER_HOST_CD = CUST.CUSTOMER_CD ");
            // selectQuery.Append(" LEFT JOIN MST_PRODUCT PRODUCT ON PRODUCT.PROD_SPEC_NO1=PRODSTOCK.PROD_SPEC_1 AND PRODUCT.PROD_SPEC_NO2=PRODSTOCK.PROD_SPEC_2 ");

            // selectQuery.Append(" WHERE  PRODUCT.PRODUCT_CD  = '" + productCD + "' ");
            // selectQuery.Append(" GROUP BY  ");
            // selectQuery.Append(" PRODSTOCK.PRODUCT_HOST_CD,  ");
            // selectQuery.Append(" PRODSTOCK.PROD_SPEC_1,  ");
            // selectQuery.Append(" PRODSTOCK.PROD_SPEC_2,  ");
            // selectQuery.Append(" PRODSTOCK.PRODUCT_NAME,  ");
            // selectQuery.Append(" PRODSTOCK.INTERNAL_NAME,  ");
            // selectQuery.Append(" PRODSTOCK.SHAPE,  ");
            // selectQuery.Append(" PRODSTOCK.DIM_D,  ");
            // selectQuery.Append(" PRODSTOCK.LENGTH,  ");
            // selectQuery.Append(" PRODSTOCK.CONDITION,  ");
            // selectQuery.Append(" CUST.SHORT_NAME,  ");
            // selectQuery.Append(" CUST.CUSTOMER_NAME,  ");
            // selectQuery.Append(" PRODSTOCK.DIM_TOL,  ");
            // selectQuery.Append(" PRODSTOCK.STOCK_DATE  ");

            // selectQuery.Append(" )  ");
            // selectQuery.Append(" GROUP BY  ");
            // selectQuery.Append(" PRODUCT_HOST_CD,  ");
            // selectQuery.Append(" PROD_SPEC_STRING,  ");
            // selectQuery.Append(" PRODUCT_NAME,  ");
            // selectQuery.Append(" INTERNAL_NAME,  ");
            // selectQuery.Append(" SHAPE,  ");
            // selectQuery.Append(" DIM_D,  ");
            // selectQuery.Append(" LENGTH,  ");
            // selectQuery.Append(" CONDITION,  ");
            // selectQuery.Append(" CUSTOMER_NAME_STRING,  ");
            // selectQuery.Append(" DIM_TOL_STRING  ");
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //selectQuery.Append("SELECT");
            //selectQuery.Append(" C.SHORT_NAME AS CUSTOMER_NAME_STRING,");
            //selectQuery.Append(" C.CUSTOMER_CD,");
            //selectQuery.Append("  ROUND(SUM(NVL(BKH.KAKOUIARI_QTY - BKH.FI_WEIGHT, 0)) / 1000,1) KAKOIRAI_REM_QTY,"); // Sum for customer 
            //selectQuery.Append(" ROUND(SUM(NVL(CURR_STOCK, 0)) / 1000,1) AS PROD_STOCK_QTY,"); // Sum for stock
            //selectQuery.Append(" ROUND(SUM(NVL(BKH.ACT_ALLOC_PENDNIG_WT, 0)) / 1000,1) BEFORE_ALLOC_PLANNED_QTY,"); // Sum pending allocation
            //selectQuery.Append(" ROUND(SUM(NVL(BKH.ACT_ALLOC_QTY - BKH.FI_WEIGHT, 0)) / 1000,1) IN_PROGRESS_QTY,"); // Sum WIP allocation
            //selectQuery.Append(" SUM( ");
            //selectQuery.Append(" CASE ");
            //selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 30 ");
            //selectQuery.Append(" AND ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) <= 60 THEN ");
            //selectQuery.Append(" CURR_STOCK ");
            //selectQuery.Append(" ELSE ");
            //selectQuery.Append(" 0 ");
            //selectQuery.Append(" END ");
            //selectQuery.Append(") / 1000 AS PENDSTOCK_ABOVE30, ");
            //selectQuery.Append(" SUM( ");
            //selectQuery.Append(" CASE ");
            //selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 60 ");
            //selectQuery.Append(" AND ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) <= 90 THEN ");
            //selectQuery.Append(" CURR_STOCK ");
            //selectQuery.Append(" ELSE ");
            //selectQuery.Append(" 0 ");
            //selectQuery.Append(" END ");
            //selectQuery.Append(") / 1000 AS PENDSTOCK_ABOVE60, ");
            //selectQuery.Append(" SUM( ");
            //selectQuery.Append(" CASE ");
            //selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 90 THEN ");
            //selectQuery.Append(" CURR_STOCK ");
            //selectQuery.Append(" ELSE ");
            //selectQuery.Append(" 0 ");
            //selectQuery.Append(" END ");
            //selectQuery.Append(") / 1000 AS PENDSTOCK_ABOVE90, ");
            //selectQuery.Append("  ROUND(SUM(NVL(CURR_STOCK, 0)) / 1000,1) AS TOT_PROD_STOCK, ");
            //selectQuery.Append(" BKH.KAKOUIRAI_HOST_CD, ");
            //selectQuery.Append(" BKH.ROOT_VER_PRODUCT_KEY, ");
            //selectQuery.Append(" BKH.PROD_SPEC_NO1, ");
            //selectQuery.Append(" BKH.PROD_SPEC_NO2, ");
            //selectQuery.Append(" BKH.PROD_SPEC_NO1 || '-'  || BKH.PROD_SPEC_NO2 AS PROD_SPEC_STRING, ");
            //selectQuery.Append(" PSH.PRODUCT_NAME,");
            //selectQuery.Append(" PSH.INTERNAL_NAME,");
            //selectQuery.Append(" PSH.SHAPE, ");
            //selectQuery.Append(" PSH.DIM_D, ");
            //selectQuery.Append(" PSH.LENGTH, ");
            //selectQuery.Append(" PSH.CONDITION, ");
            //selectQuery.Append(" PSH.DIM_TOL AS DIM_TOL_STRING  ");
            //selectQuery.Append(" FROM BCH_KAKOUIRAI_HOST BKH ");
            //selectQuery.Append(" LEFT JOIN BCH_PROD_STOCK_HOST PSH ");
            //selectQuery.Append(" ON BKH.ROOT_VER_PRODUCT_KEY = PSH.ROOT_VER_PRODUCT_KEY ");
            //selectQuery.Append(" LEFT JOIN MST_CUSTOMER C ");
            //selectQuery.Append(" ON PSH.CUSTOMER_CD = C.CUSTOMER_CD ");
            //selectQuery.Append(" LEFT JOIN MST_PRODUCT P ON P.ROOT_VER_PRODUCT_KEY = PSH.ROOT_VER_PRODUCT_KEY ");
            //selectQuery.Append(" WHERE P.PRODUCT_CD = '" + productCD + "'  ");
            //selectQuery.Append(" GROUP BY  BKH.KAKOUIRAI_HOST_CD,BKH.ROOT_VER_PRODUCT_KEY,BKH.PROD_SPEC_NO1,BKH.PROD_SPEC_NO2,C.SHORT_NAME,C.CUSTOMER_CD,PSH.PRODUCT_NAME,PSH.INTERNAL_NAME,PSH.SHAPE,PSH.DIM_D,PSH.LENGTH,PSH.CONDITION,PSH.DIM_TOL ");
            //selectQuery.Append(" ORDER BY BKH.KAKOUIRAI_HOST_CD DESC ");

              selectQuery.Append(" SELECT ");
              selectQuery.Append(" BKH.KAKOUIRAI_HOST_CD, ");
              selectQuery.Append(" BKH.ROOT_VER_PRODUCT_KEY, ");
              selectQuery.Append(" P.PROD_SPEC_NO1, ");
              selectQuery.Append(" P.PROD_SPEC_NO2, ");
              selectQuery.Append(" P.PROD_SPEC_NO1 ");
              selectQuery.Append(" || '-' ");
              selectQuery.Append(" || P.PROD_SPEC_NO2 AS PROD_SPEC_STRING, ");
              selectQuery.Append(" PSH.PRODUCT_NAME, ");
              selectQuery.Append(" PSH.INTERNAL_NAME, ");
              selectQuery.Append(" PSH.SHAPE, ");
              selectQuery.Append(" PSH.DIM_D, ");
              selectQuery.Append(" PSH.LENGTH, ");
              //selectQuery.Append(" PSH.CONDITION, ");
              selectQuery.Append(" P.MAGARI_KUBUN, ");
              selectQuery.Append(" P.MAGARI_BEND_TOL, ");
              selectQuery.Append(" P.MAGARI_SPAN, ");
              selectQuery.Append(" P.PRODUCT_KUBUN,");
              selectQuery.Append(" COND.CONDITION_DESCRIPTION AS CONDITION,");
              selectQuery.Append(" (TRIM(TO_CHAR(P.DIM_TOL_MAX, 'S0D099')) ");
              selectQuery.Append(" || ',' ");
              selectQuery.Append(" || TRIM(TO_CHAR(P.DIM_TOL_MIN, 'S0D099'))) DIM_TOL_STRING, ");
              selectQuery.Append("  ");
              selectQuery.Append(" TRIM((TO_CHAR(P.LENGTH_TOL_MAX, 'S990D0')) ");
              selectQuery.Append(" || ',' ");
              selectQuery.Append(" || TRIM(TO_CHAR(P.LENGTH_TOL_MIN, 'S990D0'))) LEN_TOL_STRING, ");
              
              selectQuery.Append(" C.SHORT_NAME AS CUSTOMER_NAME_STRING, ");
              selectQuery.Append(" C.CUSTOMER_CD, ");
              selectQuery.Append(" ROUND(SUM(NVL(BKH.KAKOUIARI_QTY - BKH.FI_WEIGHT, 0)) / 1000, 1) KAKOIRAI_REM_QTY, ");
              selectQuery.Append(" ROUND(SUM(NVL(CURR_STOCK, 0)) / 1000, 1) AS PROD_STOCK_QTY, ");
              selectQuery.Append(" ( ");
              selectQuery.Append(" ROUND(SUM(NVL(BKH.KAKOUIARI_QTY - BKH.FI_WEIGHT, 0)) / 1000, 1) + ");
              selectQuery.Append("  ROUND(SUM(NVL(CURR_STOCK, 0)) / 1000, 1) ");
              selectQuery.Append(" ) AS TOTAL_KAKO_PROD, ");
              selectQuery.Append(" ROUND(SUM(NVL(BKH.ACT_ALLOC_PENDNIG_WT, 0)) / 1000, 1) BEFORE_ALLOC_PLANNED_QTY, ");
              selectQuery.Append(" ROUND(SUM(NVL(BKH.ACT_ALLOC_QTY - BKH.FI_WEIGHT, 0)) / 1000, 1) IN_PROGRESS_QTY, ");
              selectQuery.Append(" ROUND(SUM( ");
              selectQuery.Append(" CASE ");
              selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 30 ");
              selectQuery.Append(" AND ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) <= 60 THEN ");
              selectQuery.Append(" CURR_STOCK ");
              selectQuery.Append(" ELSE ");
              selectQuery.Append("  0 ");
              selectQuery.Append(" END ");
              selectQuery.Append(" ) / 1000,1) AS PENDSTOCK_ABOVE30, ");
              selectQuery.Append(" ROUND(SUM( ");
              selectQuery.Append(" CASE ");
              selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 60 ");
              selectQuery.Append(" AND ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) <= 90 THEN ");
              selectQuery.Append(" CURR_STOCK ");
              selectQuery.Append(" ELSE ");
              selectQuery.Append(" 0 ");
              selectQuery.Append(" END ");
              selectQuery.Append(" ) / 1000,1) AS PENDSTOCK_ABOVE60, ");
              selectQuery.Append("  ROUND(SUM( ");
              selectQuery.Append(" CASE ");
              selectQuery.Append(" WHEN ROUND(SYSDATE - TO_DATE(STOCK_DATE, 'YY-MM-DD')) > 90 THEN ");
              selectQuery.Append(" CURR_STOCK ");
              selectQuery.Append(" ELSE ");
              selectQuery.Append(" 0 ");
              selectQuery.Append(" END ");
              selectQuery.Append(" ) / 1000,1) AS PENDSTOCK_ABOVE90, ");
              selectQuery.Append(" ROUND(SUM(NVL(CURR_STOCK, 0)) / 1000, 1) AS TOT_PROD_STOCK ");
              selectQuery.Append(" FROM ");
              selectQuery.Append(" MST_PRODUCT           P ");
              selectQuery.Append(" LEFT JOIN BCH_PROD_STOCK_HOST   PSH ON P.ROOT_VER_PRODUCT_KEY = PSH.ROOT_VER_PRODUCT_KEY ");
              selectQuery.Append(" LEFT JOIN MST_CUSTOMER C ON PSH.CUSTOMER_CD = C.CUSTOMER_CD ");
              selectQuery.Append(" LEFT JOIN BCH_KAKOUIRAI_HOST BKH  ON P.ROOT_VER_PRODUCT_KEY = BKH.ROOT_VER_PRODUCT_KEY ");
              selectQuery.Append(" LEFT OUTER JOIN MST_CONDITION COND ON P.PRODUCT_CONDITION_CD = COND.CONDITION_CD");
              selectQuery.Append(" WHERE ");
              selectQuery.Append(" P.PRODUCT_CD = '" + productCD + "'  ");
              selectQuery.Append(" AND P.VER = ( ");
              selectQuery.Append(" SELECT ");
              selectQuery.Append(" MAX(VER) ");
              selectQuery.Append(" FROM ");
              selectQuery.Append(" MST_PRODUCT ");
              selectQuery.Append(" WHERE ");
              selectQuery.Append(" P.ROOT_VER_PRODUCT_KEY = ROOT_VER_PRODUCT_KEY ");
              selectQuery.Append(" )     ");
              selectQuery.Append(" GROUP BY ");
              selectQuery.Append(" BKH.KAKOUIRAI_HOST_CD, ");
              selectQuery.Append(" BKH.ROOT_VER_PRODUCT_KEY, ");
              selectQuery.Append(" P.PROD_SPEC_NO1, ");
              selectQuery.Append(" P.PROD_SPEC_NO2, ");
              selectQuery.Append(" C.SHORT_NAME, ");
              selectQuery.Append(" C.CUSTOMER_CD, ");
              selectQuery.Append(" PSH.PRODUCT_NAME, ");
              selectQuery.Append(" PSH.INTERNAL_NAME, ");
              selectQuery.Append(" PSH.SHAPE, ");
              selectQuery.Append(" PSH.DIM_D, ");
              selectQuery.Append(" PSH.LENGTH, ");
              selectQuery.Append(" PSH.CONDITION, ");
              selectQuery.Append(" P.DIM_TOL_MAX, ");
              selectQuery.Append(" P.DIM_TOL_MIN, ");
              selectQuery.Append(" P.LENGTH_TOL_MAX, ");
              selectQuery.Append(" P.LENGTH_TOL_MIN, ");
              selectQuery.Append(" P.MAGARI_KUBUN, ");
              selectQuery.Append(" P.MAGARI_BEND_TOL, ");
              selectQuery.Append(" P.MAGARI_SPAN, ");
              selectQuery.Append(" P.PRODUCT_KUBUN, ");
              selectQuery.Append(" CONDITION_DESCRIPTION");




            var result = ExecuteQuerySingleResult<ProdSpecificationDetails>(selectQuery.ToString());
            return result;
        }

        public List<ProductDetails> GetShukkaDetailsByProductCD(string productCD)
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
                List<decimal> dispatchProdData = new List<decimal>();
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
                    selectQuery.Append(" SELECT NVL(SUM(DISPATCH_QTY), '0') / 1000  AS TOTALDISPATCHQTY");
                    selectQuery.Append(" FROM BCH_MONTH_END_SHIPMENT BMS");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" BMS.ROOT_VER_PRODUCT_KEY IN( ");
                    selectQuery.Append(" SELECT DISTINCT p.root_ver_product_key FROM mst_Product p ");
                    selectQuery.Append(" WHERE  P.PRODUCT_CD = '" + productCD + "') ");
                    selectQuery.Append(" AND YEAR = " + getYear + "");
                    selectQuery.Append(" AND MONTH = " + getMonth + "");
                    decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
                    dispatchProdData.Add(getQty);
                   
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

                details.MAX_QTY = dispatchProdData.Max();
                details.MIN_QTY = dispatchProdData.Min();
                details.AVG_QTY = Convert.ToDecimal(dispatchProdData.Average().ToString("########.0"));
                details.TOTAL_QTY = Convert.ToDecimal(dispatchProdData.Sum().ToString("########.0"));

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
        
        public List<ProductDetails> GetMonthWiseProdStockByProductCD(string productCD)
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
                List<decimal> monthStockData = new List<decimal>();
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
                    selectQuery.Append(" SELECT NVL(SUM(PROD_STOCK_QTY), '0') / 1000  AS TOTALPRODSTOCKQTY");
                    selectQuery.Append(" FROM ");
                    selectQuery.Append(" BCH_MONTH_END_PROD_STOCK MEPS WHERE MEPS.ROOT_VER_PRODUCT_KEY IN (   SELECT DISTINCT ");
                    selectQuery.Append(" P.ROOT_VER_PRODUCT_KEY ");
                    selectQuery.Append(" FROM MST_PRODUCT P ");
                    selectQuery.Append(" WHERE ");
                    selectQuery.Append(" P.PRODUCT_CD = '" + productCD + "' AND VER = (SELECT MAX(VER) FROM MST_PRODUCT WHERE PRODUCT_CD ='" + productCD + "') )   ");
                    selectQuery.Append(" AND YEAR = " + getYear + " AND MONTH = " + getMonth + " ");
                    decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
                    monthStockData.Add(getQty);

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

                details.MAX_QTY = monthStockData.Max();
                details.MIN_QTY = monthStockData.Min();
                details.AVG_QTY = Convert.ToDecimal(monthStockData.Average().ToString("########.0"));

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

        public List<ProductDetails> GetPendingStockByProductCD(string productCD)
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
                    selectQuery.Append(" PRODUCT_CD = " + "'" + productCD + "'");
                    selectQuery.Append(" GROUP BY ");
                    selectQuery.Append(" STOCK_DATE ");
                    selectQuery.Append(" )  ");
                    //selectQuery.Append(" AND YEAR = " + getYear);
                    //selectQuery.Append(" AND MONTH =  " + getMonth);
                    decimal getQty = Convert.ToDecimal(ExecuteScalar(selectQuery.ToString()));
                    pendingStockData.Add(getQty);

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

                details.MAX_QTY = pendingStockData.Max();
                details.MIN_QTY = pendingStockData.Min();
                details.AVG_QTY = Convert.ToDecimal(pendingStockData.Average().ToString("########.0"));
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
