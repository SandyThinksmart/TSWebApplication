using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
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
    public class RollForcastPopupRepository : BaseRepository, IRollForcastPopupRepository
    {
        private readonly LocalizationServices _localization;
        public RollForcastPopupRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }

        /// <summary>
        ///  Fetch Data From TRN_SALES_FORECAST Table.
        ///  Get One Month Product Forecast Details on Roll Forecast Popup.
        /// </summary>
        /// <returns> CREATED BY : shobhana on 2025/01/13 </returns>
        public List<RollForcastPopup> GetOneMonthProductForecastDetails(string FcSummaryNo, int Roll, int Year)
        {
            List<RollForcastPopup> result = new List<RollForcastPopup>();
            StringBuilder selectQuery = new StringBuilder();

            int dispatchR = 0;

            if (Roll == 12)
            {
                dispatchR = 1;
            }
            else
            {
                dispatchR = Roll + 1;
            }
            DateTime dispatchDate = new DateTime(Year, dispatchR, 1);

            DateTime resultMonth = dispatchDate.AddMonths(-2);

            int dispatchMonth1 = resultMonth.AddMonths(-1).Month;
            int dispatchMonth2 = resultMonth.AddMonths(-2).Month;
            int dispatchMonth3 = resultMonth.AddMonths(-3).Month;
            int dispatchMonth4 = resultMonth.AddMonths(-4).Month;

            int dispatchMonthYear1 = resultMonth.AddMonths(-1).Year;
            int dispatchMonthYear2 = resultMonth.AddMonths(-2).Year;
            int dispatchMonthYear3 = resultMonth.AddMonths(-3).Year;
            int dispatchMonthYear4 = resultMonth.AddMonths(-4).Year;


            selectQuery.Append("SELECT ");
            selectQuery.Append("PD.PROD_SPEC_NO,");
            selectQuery.Append("PD.PRODUCT_CD,");
            selectQuery.Append("PD.CUSTOMER_CD,");            
            selectQuery.Append("FC.ROLL_FC_KUBUN,");
            selectQuery.Append("PD.INTERNAL_NAME,");
            selectQuery.Append("PD.CUSTOMER_NAME,");
            selectQuery.Append("PD.PRODUCT_NAME_CUST,");
            selectQuery.Append("PD.DIM_D,");
            selectQuery.Append("PD.DIM_W,");
            selectQuery.Append("PD.DIM_OTHER,");
            selectQuery.Append("PD.SHAPE,");
            selectQuery.Append("PD.MATL_DIM_D,");
            selectQuery.Append("PD.MATL_DIM_W,");
            selectQuery.Append("PD.MATL_SHAPE,");
            selectQuery.Append("PD.MATL_DIM_OTHER,");
            selectQuery.Append("PD.MATL_LENGTH,");
            selectQuery.Append("PD.PRODUCT_LENGTH,");
            selectQuery.Append("PD.LENGTH_OTHER,");
            
            selectQuery.Append("(TRIM(TO_CHAR(PD.DIM_TOL_MAX, 'S0D099'))");
            selectQuery.Append("|| ','");
            selectQuery.Append("|| TRIM(TO_CHAR(PD.DIM_TOL_MIN, 'S0D099'))) DIM_TOL,");
            selectQuery.Append("TRIM((TO_CHAR(PD.LENGTH_TOL_MAX, 'S990D0'))");
            selectQuery.Append("|| ','");
            selectQuery.Append("|| TRIM(TO_CHAR(PD.LENGTH_TOL_MIN, 'S990D0'))) LEN_TOL,");
            selectQuery.Append("PD.MATL_NAME_MAKER,");
            selectQuery.Append("PD.CONDITION_DESCRIPTION,");
            selectQuery.Append("PD.HT_CONDITION,");
            selectQuery.Append("PD.MAKER_CD,");
            selectQuery.Append("PD.MAKER_NAME,");
            selectQuery.Append("FC.ROLL_FC_CD,");
            selectQuery.Append("FC.Roll_FC_NO AS FC_NO,");
            selectQuery.Append("FC.ROLL_FC_SRLNO AS FC_SRLNO,");
            selectQuery.Append("FC.ROLL_FC_KUBUN,");
            selectQuery.Append("FC.ROLL_FC_R,");
            selectQuery.Append("FC.ROLL_FC_YEAR,");
            selectQuery.Append("FC.ROLL_FC_DATE,");
            selectQuery.Append("FC.EMP_CD,");
            selectQuery.Append("FC.CUST_CD,");
            selectQuery.Append("FC.FC_QTY_R1,");
            selectQuery.Append("FC.FC_QTY_R2,");
            selectQuery.Append("FC.FC_QTY_R3,");
            selectQuery.Append("FC.FC_QTY_R4,");
            selectQuery.Append("PD.EMPLOYEE_SHORT_NAME AS INCHARGE,");
            selectQuery.Append("PD.CUSTOMER_NAME AS CUSTOMER,");
            selectQuery.Append("PD.REMARKS, ");
            selectQuery.Append("C.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append("FC.PROD_STOCK AS PROD_STOCK,");
            selectQuery.Append("FC.WIP_STOCK AS WIP_STOCK, ");
            selectQuery.Append("ROUND(NVL(BKI.KAKOIRAI_REM_QTY / 1000, 0),1) AS KAKOIRAI_REM_QTY, ");
            selectQuery.Append("ROUND(NVL(PRODSTOCK.PROD_STOCK_QTY, 0),1) AS PROD_STOCK_QTY, ");
            selectQuery.Append("LAST1_MNTH_DISPACH_QTY, ");
            selectQuery.Append("LAST2_MNTH_DISPACH_QTY, ");
            selectQuery.Append("LAST3_MNTH_DISPACH_QTY, ");
            selectQuery.Append("ROUND(((NVL(LAST1_MNTH_DISPACH_QTY, 0) + NVL(LAST2_MNTH_DISPACH_QTY, 0) + NVL(LAST3_MNTH_DISPACH_QTY, 0)) / 3), 1) AS LAST_MONTH_DISPATCH_AVG_QTY, ");
            selectQuery.Append("FC.MSG_TO_MATL_DEPT, ");
            selectQuery.Append("DISPATCH.DISPATCH_QTY1 AS DISPATCH_QTY1, ");
            selectQuery.Append("DISPATCH.DISPATCH_QTY2 AS DISPATCH_QTY2, ");
            selectQuery.Append("DISPATCH.DISPATCH_QTY3 AS DISPATCH_QTY3, ");
            selectQuery.Append("DISPATCH.DISPATCH_QTY4 AS DISPATCH_QTY4, ");
            selectQuery.Append("ROUND(((NVL(DISPATCH_QTY2, 0) + NVL(DISPATCH_QTY3, 0) + NVL(DISPATCH_QTY4, 0)) / 3), 1) AS DISPATCH_AVG_QTY,");
            selectQuery.Append("ROUND(NVL((NVL(FC_QTY_R1, 0) - (NVL(PRODSTOCK.PROD_STOCK_QTY, 0) + NVL(KAKOIRAI_REM_QTY, 0))), 0),1) MAT_QTY_1MONTH, ");
            selectQuery.Append("ROUND(NVL(((NVL(FC_QTY_R1, 0) + NVL(FC_QTY_R2, 0)) - (NVL(PRODSTOCK.PROD_STOCK_QTY, 0) + NVL(KAKOIRAI_REM_QTY, 0))), 0),1) MAT_QTY_2MONTH, ");
            selectQuery.Append("ROUND(NVL(((NVL(FC_QTY_R1, 0) + NVL(FC_QTY_R2, 0) + NVL(FC_QTY_R3, 0)) - (NVL(PRODSTOCK.PROD_STOCK_QTY, 0) + NVL(KAKOIRAI_REM_QTY, 0))), 0),1) MAT_QTY_3MONTH, ");
            selectQuery.Append("ROUND(NVL(((NVL(FC_QTY_R1, 0) + NVL(FC_QTY_R2, 0) + NVL(FC_QTY_R3, 0) + NVL(FC_QTY_R4, 0)) - (NVL(PRODSTOCK.PROD_STOCK_QTY, 0) + NVL(KAKOIRAI_REM_QTY, 0))), 0),1) MAT_QTY_4MONTH, ");
            selectQuery.Append("PD.MAGARI_KUBUN, ");
            selectQuery.Append("PD.MAGARI_BEND_TOL, ");
            selectQuery.Append("PD.MAGARI_SPAN, ");
            selectQuery.Append("FC.SALES_FC_DONE_DATE, ");
            selectQuery.Append("FC.REPLY_FROM_MATL_DEPT ");

            selectQuery.Append(" FROM V_PRODUCT_DETAILS PD ");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST FC ");
            selectQuery.Append(" ON FC.PRODUCT_CD = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION C ON PD.MATL_CONDITION_CD = C.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY AS ROOT_VER_PRODUCT_KEY1, ");
            selectQuery.Append(" SUM(BPS.CURR_STOCK) / 1000 PROD_STOCK_QTY, ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_PROD_STOCK_HOST BPS ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" BPS.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) PRODSTOCK ON PRODSTOCK.ROOT_VER_PRODUCT_KEY1 = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" LEFT JOIN BCH_KAKOIRAI_INFO BKI ON PD.ROOT_VER_PRODUCT_KEY = BKI.PROD_SPEC_CD ");
            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY, ");
            selectQuery.Append(" ROUND(SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth4 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear4 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ),1) AS DISPATCH_QTY1, ");
            selectQuery.Append(" ROUND(SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth3 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear3 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ), 1) AS DISPATCH_QTY2, ");
            selectQuery.Append(" ROUND(SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth2 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear2 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ), 1) AS DISPATCH_QTY3, ");
            selectQuery.Append(" ROUND(SUM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MONTH = '" + dispatchMonth1 + "' ");
            selectQuery.Append(" AND YEAR = '" + dispatchMonthYear1 + "' THEN ");
            selectQuery.Append(" NVL(DISPATCH_QTY, 0) / 1000 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ), 1) AS DISPATCH_QTY4 ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_MONTH_END_SHIPMENT ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" ) DISPATCH ON DISPATCH.ROOT_VER_PRODUCT_KEY = PD.ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append("WHERE PD.VERSION = (");
            selectQuery.Append("SELECT MAX(VERSION) ");
            selectQuery.Append("FROM V_PRODUCT_DETAILS ");
            selectQuery.Append("WHERE pd.ROOT_VER_PRODUCT_KEY = ROOT_VER_PRODUCT_KEY) ");
            selectQuery.Append("AND FC.INT_MATL_FC_SUMMARY_CD = " + FcSummaryNo + " ");
            selectQuery.Append("AND FC.ROLL_FC_KUBUN = 1 ");
            selectQuery.Append("ORDER BY ");
            selectQuery.Append("MATL_NAME_MAKER, ");
            selectQuery.Append("CUSTOMER_NAME, ");
            selectQuery.Append("INTERNAL_NAME, ");
            selectQuery.Append("DIM_D, ");
            selectQuery.Append("SHAPE, ");
            selectQuery.Append("PRODUCT_LENGTH, ");
            selectQuery.Append("MATL_SPECIAL_KUBUN");
            


            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<RollForcastPopup>(query);

            return result;
        }


        /// <summary>
        ///  Fetch Data From TRN_SALES_FORECAST Table.
        ///  Get One Month Material Forecast Details on Roll Forecast Popup.
        /// </summary>
        /// <returns> CREATED BY : shobhana on 2025/01/16 </returns>
        public List<RollForcastPopup> GetOneMonthMaterialForecastDetails(string FcSummaryNo)
        {
            var FC_KUBUN = 1;
            List<RollForcastPopup> result = new List<RollForcastPopup>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("TF.ROLL_FC_KUBUN, ");          
            selectQuery.Append(" DECODE(TF.ROLL_FC_KUBUN, 3, '無し', 2, '有り') AS MAKER_DETAILS, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TF.ROLL_FC_KUBUN = 2 THEN ");
            selectQuery.Append(" MD.MATL_MAKER_NAME ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END MATL_MAKER_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TF.ROLL_FC_KUBUN = 2 THEN ");
            selectQuery.Append(" MD.MAKER_SHORT_NAME ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" NULL ");
            selectQuery.Append(" END MAKER_NAME,  ");
            selectQuery.Append("MD.MATL_KUBUN, ");
            selectQuery.Append("MD.DIM_D, ");
            selectQuery.Append("MD.SHAPE, ");
            selectQuery.Append("MD.DIM_W, ");
            selectQuery.Append("MD.DIM_OTHER, ");
            selectQuery.Append("MD.MATERIAL_LENGTH, ");
            selectQuery.Append("TF.FC_QTY_R1, ");
            selectQuery.Append("TF.FC_QTY_R2, ");
            selectQuery.Append("TF.FC_QTY_R3, ");
            selectQuery.Append("TF.FC_QTY_R4, ");
            selectQuery.Append("0 AS MATL_STOCK_QTY, ");
            selectQuery.Append("MD.MATL_SPECIAL_KUBUN, ");
            selectQuery.Append("MD.CONDITION_DESCRIPTION, ");
            selectQuery.Append("MD.MOCHIKOMISAKI_CD, ");
            selectQuery.Append("MKSAKI.SHORT_NAME, ");
            selectQuery.Append("0 AS ORDER_REM_QTY, ");
            selectQuery.Append("TF.MSG_TO_MATL_DEPT, ");
            selectQuery.Append("TF.INT_MATL_FC_SUMMARY_CD, ");
            selectQuery.Append("TF.ROLL_FC_SRLNO, ");
            selectQuery.Append("TF.ROLL_FC_CD, ");
            selectQuery.Append("TF.ROLL_FC_DATE, ");
            selectQuery.Append("TF.ROLL_FC_R, ");
            selectQuery.Append("TF.ROLL_FC_YEAR, ");
            selectQuery.Append("TF.CUST_CD, ");
            selectQuery.Append("MD.INTERNAL_NAME_CD, ");
            selectQuery.Append("MD.INTERNAL_NAME, ");
            selectQuery.Append("ME.SHORT_NAME AS INCHARGE, ");
            selectQuery.Append("MC.SHORT_NAME AS CUSTOMER, ");
            selectQuery.Append("TO_NUMBER(0) AS IS_LAST_R_REC, ");
            selectQuery.Append("MD.REMARKS, ");
            selectQuery.Append("0 AS REQD_MATL_QTY_1MONTH, ");
            selectQuery.Append("0 AS REQD_MATL_QTY_2MONTH, ");
            selectQuery.Append("0 AS REQD_MATL_QTY_3MONTH, ");
            selectQuery.Append("0 AS REQD_MATL_QTY_4MONTH, ");
            selectQuery.Append("TF.SALES_FC_DONE_DATE, ");
            selectQuery.Append("TF.REPLY_FROM_MATL_DEPT ");
            selectQuery.Append("FROM V_HT_MATL_DETAILS MD ");
            selectQuery.Append("INNER JOIN TRN_SALES_FORECAST TF ON TF.MATERIAL_CD = MD.MATERIAL_CD ");
            selectQuery.Append("LEFT OUTER JOIN MST_CONDITION CND ON MD.CONDITION_CD = CND.CONDITION_CD ");
            selectQuery.Append("LEFT OUTER JOIN MST_MOCHIKOMISAKI MKSAKI ON MD.MOCHIKOMISAKI_CD = MKSAKI.MOCHIKOMISAKI_CD ");
            selectQuery.Append("JOIN MST_EMPLOYEE ME ON TF.EMP_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append("JOIN MST_CUSTOMER MC ON TF.CUST_CD = MC.CUSTOMER_CD ");
            selectQuery.Append("WHERE");
            selectQuery.Append(" TF.INT_MATL_FC_SUMMARY_CD = " + FcSummaryNo + " ");
            selectQuery.Append("AND TF.ROLL_FC_KUBUN <> 1 ");
            selectQuery.Append("ORDER BY ");
            selectQuery.Append("MATL_MAKER_NAME, ");
            selectQuery.Append("INTERNAL_NAME, ");
            selectQuery.Append("DIM_D, ");
            selectQuery.Append("SHAPE, ");
            selectQuery.Append("MATERIAL_LENGTH, ");
            selectQuery.Append("MATL_SPECIAL_KUBUN ");
            var query = selectQuery.ToString();
            selectQuery.Append(query);

            result = ExecuteQueryMultiResult<RollForcastPopup>(query);
            return result;
        }


    }

}
