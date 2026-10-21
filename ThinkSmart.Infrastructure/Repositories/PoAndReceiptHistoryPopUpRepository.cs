using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class PoAndReceiptHistoryPopUpRepository : BaseRepository, IPoAndReceiptHistoryPopUpRepository
    {
        private readonly LocalizationServices _localization;
        public PoAndReceiptHistoryPopUpRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/04       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_INT_MATL_FC_SUMMARY Table
        /// </returns>
        public List<PoAndReceiptPopupDetails> GetMakerWisePurchaseOrderQtyList(int Batch_Roll)
        {
            DateTime currentDate = DateTime.Now;
            DateTime lastYear = currentDate.AddYears(-1);
            var forecastRollsYear = GetForecastRollYear(Batch_Roll);
            List<PoAndReceiptPopupDetails> result = new List<PoAndReceiptPopupDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MAKER_NAME,");
            selectQuery.Append(" VENDOR AS SUPPLIER_NAME, ");
            selectQuery.Append(" DECODE(PO_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') PO_KUBUN, ");
            selectQuery.Append(" TO_CHAR(SUM(R1)) AS R1, ");
            selectQuery.Append(" TO_CHAR(SUM(R2)) AS R2, ");
            selectQuery.Append(" TO_CHAR(SUM(R3)) AS R3, ");
            selectQuery.Append(" TO_CHAR(SUM(R4)) AS R4, ");
            selectQuery.Append(" TO_CHAR(SUM(R5)) AS R5, ");
            selectQuery.Append(" TO_CHAR((SUM(R1) + SUM(R2) + SUM(R3) + SUM(R4) + SUM(R5))) AS PO_DTL_TOTAL, ");
            selectQuery.Append(" TO_CHAR(SUM(R6)) AS R6, ");
            selectQuery.Append(" TO_CHAR(SUM(R7)) AS R7, ");
            selectQuery.Append(" TO_CHAR(SUM(R8)) AS R8, ");
            selectQuery.Append(" TO_CHAR(SUM(R9)) AS R9, ");
            selectQuery.Append(" TO_CHAR(SUM(R10)) AS R10, ");
            selectQuery.Append(" TO_CHAR((SUM(R6) + SUM(R7) + SUM(R8) + SUM(R9) + SUM(R10))) AS TOTAL ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" '' AS MAKER,");
            selectQuery.Append(" MM.SHORT_NAME    AS MAKER_NAME, ");
            selectQuery.Append(" MS.SHORT_NAME    AS VENDOR, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TK.CONF_YYYY IS NULL THEN ");
            selectQuery.Append(" TPD.YYYY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" TK.CONF_YYYY ");
            selectQuery.Append(" END AS YEAR, "); 
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[0].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[0].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[0].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[0].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R1, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[1].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[1].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[1].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[1].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R2, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[2].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[2].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[2].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[2].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R3, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[3].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[3].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[3].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[3].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R4, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[4].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[4].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[4].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[4].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R5, ");
            selectQuery.Append(" 0 AS PO_DTL_TOTAL, ");
            selectQuery.Append(" TPH.MATL_KUBUN   AS PO_KUBUN, ");
            selectQuery.Append(" 0 AS R6, ");
            selectQuery.Append(" 0 AS R7, ");
            selectQuery.Append(" 0 AS R8, ");
            selectQuery.Append(" 0 AS R9, ");
            selectQuery.Append(" 0 AS R10, ");
            selectQuery.Append(" 0 AS TOTAL, ");
            selectQuery.Append(" TPH.MATL_KUBUN   AS TMP_KUBUN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_PO_DTL TPD ");
            selectQuery.Append(" LEFT JOIN TRN_KOUBAN TK ON TPD.PO_DTL_CD = TK.PO_DTL_CD ");
            selectQuery.Append(" JOIN TRN_PO_HDR TPH ON TPD.PO_NO = TPH.PO_NO ");
            selectQuery.Append(" JOIN MST_MAKER MM ON TPH.MAKER_CD = MM.MAKER_CD ");
            selectQuery.Append(" JOIN MST_SUPPLIER MS ON TPH.SUPPLIER_CD = MS.SUPPLIER_CD ");
            selectQuery.Append(" UNION ALL ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" '' AS MAKER, ");
            selectQuery.Append(" MM.SHORT_NAME AS MAKER_NAME, ");
            selectQuery.Append(" MS.SHORT_NAME AS VENDOR, ");
            selectQuery.Append(" RPD.ROLL_YEAR AS YEAR, ");
            selectQuery.Append(" 0 AS R1, ");
            selectQuery.Append(" 0 AS R2, ");
            selectQuery.Append(" 0 AS R3, ");
            selectQuery.Append(" 0 AS R4, ");
            selectQuery.Append(" 0 AS R5, ");
            selectQuery.Append(" 0 AS PO_DTL_TOTAL, ");
            selectQuery.Append(" MHMS.MATL_KUBUN   AS PO_KUBUN, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[0].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[0].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R6, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[1].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[1].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R7, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[2].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[2].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R8, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[3].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[3].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R9, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[4].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[4].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R10, ");
            selectQuery.Append(" 0 AS TOTAL, ");
            selectQuery.Append(" MHMS.MATL_KUBUN   AS TMP_KUBUN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_ROLL_PO_DETAILS RPD ");
            selectQuery.Append(" JOIN MST_SUPPLIER MS ON RPD.SUPPLIER_CD = MS.SUPPLIER_CD ");
            selectQuery.Append(" JOIN MST_MATERIAL MHMS ON RPD.MATERIAL_CD  = MHMS.MATERIAL_CD ");
            selectQuery.Append(" JOIN MST_MATL_NAME_MAKER MMNM ON MHMS.MATL_NAME_MAKER_CD = MMNM.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" JOIN MST_MAKER MM ON MMNM.MAKER_CD = MM.MAKER_CD");
            selectQuery.Append(" ) ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" YEAR = '"+currentDate.Year+"' ");
           // selectQuery.Append(" OR YEAR = '"+lastYear.Year+"' ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" CUBE(VENDOR,PO_KUBUN),MAKER_NAME ");
            selectQuery.Append(" ORDER BY ");
            selectQuery.Append(" MAKER_NAME ");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<PoAndReceiptPopupDetails>(query);
            return result;
        }
        public List<Tuple<int, int>> GetForecastRollYear(int Batch_Roll)
        {
            List<Tuple<int, int>> Roll_Year_List = new List<Tuple<int, int>>();


            DateTime currentDate = DateTime.Now;

            // Calculate base month
            DateTime baseMonth1 = new DateTime(currentDate.Year, Batch_Roll, 1);

            // Calculate forecast months
            DateTime R5 = baseMonth1.AddMonths(1);
            DateTime R4 = baseMonth1;
            DateTime R3 = baseMonth1.AddMonths(-1);
            DateTime R2 = baseMonth1.AddMonths(-2);
            DateTime R1 = baseMonth1.AddMonths(-3);

            // Assign forecast rolls
                 Roll_Year_List.Add(Tuple.Create(R1.Year, R1.Month));
                 Roll_Year_List.Add(Tuple.Create(R2.Year, R2.Month));
                 Roll_Year_List.Add(Tuple.Create(R3.Year, R3.Month));
                 Roll_Year_List.Add(Tuple.Create(R4.Year, R4.Month));
                 Roll_Year_List.Add(Tuple.Create(R5.Year, R5.Month));
            // Output result

            return Roll_Year_List;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/04       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_INT_MATL_FC_SUMMARY Table
        /// </returns>
        public List<PoAndReceiptPopupDetails> GetVendorWisePurchaseOrderQtyList(int Batch_Roll)
        {
            DateTime currentDate = DateTime.Now;
            DateTime lastYear = currentDate.AddYears(-1);
            var forecastRollsYear = GetForecastRollYear(Batch_Roll);
            List<PoAndReceiptPopupDetails> result = new List<PoAndReceiptPopupDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" VENDOR AS SUPPLIER_NAME, ");
            selectQuery.Append(" TO_CHAR(SUM(R1)) AS R1, ");
            selectQuery.Append(" TO_CHAR(SUM(R2)) AS R2, ");
            selectQuery.Append(" TO_CHAR(SUM(R3)) AS R3, ");
            selectQuery.Append(" TO_CHAR(SUM(R4)) AS R4, ");
            selectQuery.Append(" TO_CHAR(SUM(R5)) AS R5, ");
            selectQuery.Append(" TO_CHAR((SUM(R1) + SUM(R2) + SUM(R3) + SUM(R4) + SUM(R5))) AS KOUBAN_TOTAL, ");
            selectQuery.Append(" TO_CHAR(SUM(R6)) AS R6, ");
            selectQuery.Append(" TO_CHAR(SUM(R7)) AS R7, ");
            selectQuery.Append(" TO_CHAR(SUM(R8)) AS R8, ");
            selectQuery.Append(" TO_CHAR(SUM(R9)) AS R9, ");
            selectQuery.Append(" TO_CHAR(SUM(R10)) AS R10, ");
            selectQuery.Append(" TO_CHAR((SUM(R6) + SUM(R7) + SUM(R8) + SUM(R9) + SUM(R10))) AS TOTAL ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" '' AS MAKER,");
            selectQuery.Append(" MM.SHORT_NAME    AS MAKER_NAME, ");
            selectQuery.Append(" MS.SHORT_NAME    AS VENDOR, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN TK.CONF_YYYY IS NULL THEN ");
            selectQuery.Append(" TPD.YYYY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" TK.CONF_YYYY ");
            selectQuery.Append(" END AS YEAR, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[0].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[0].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[0].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[0].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R1, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[1].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[1].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[1].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[1].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R2, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[2].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[2].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[2].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[2].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R3, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[3].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[3].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[3].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[3].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R4, ");
            selectQuery.Append(" ((( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TK.CONF_R = '" + forecastRollsYear[4].Item2 + "' ");//item2=roll
            selectQuery.Append(" AND TK.CONF_YYYY = '" + forecastRollsYear[4].Item1 + "' ");//item1=year
            selectQuery.Append(" AND tk.KOUBAN_COMPLETE_DATE is null ) THEN ");
            selectQuery.Append(" TK.CONF_QTY/1000 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) + ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( TPD.ROLL_R = '" + forecastRollsYear[4].Item2 + "'  ");
            selectQuery.Append(" AND TPD.YYYY = '" + forecastRollsYear[4].Item1 + "' ");
            selectQuery.Append(" AND TPD.INACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND TPD.PO_DTL_CD NOT IN ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PO_DTL_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_KOUBAN ");
            selectQuery.Append(" ) ) THEN ");
            selectQuery.Append(" TPD.QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) ) AS R5, ");
            selectQuery.Append(" 0 AS KOUBAN_TOTAL, ");
            selectQuery.Append(" 0 AS R6, ");
            selectQuery.Append(" 0 AS R7, ");
            selectQuery.Append(" 0 AS R8, ");
            selectQuery.Append(" 0 AS R9, ");
            selectQuery.Append(" 0 AS R10, ");
            selectQuery.Append(" 0 AS TOTAL ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_PO_DTL TPD ");
            selectQuery.Append(" LEFT JOIN TRN_KOUBAN TK ON TPD.PO_DTL_CD = TK.PO_DTL_CD ");
            selectQuery.Append(" JOIN TRN_PO_HDR TPH ON TPD.PO_NO = TPH.PO_NO ");
            selectQuery.Append(" JOIN MST_MAKER MM ON TPH.MAKER_CD = MM.MAKER_CD ");
            selectQuery.Append(" JOIN MST_SUPPLIER MS ON TPH.SUPPLIER_CD = MS.SUPPLIER_CD ");
            selectQuery.Append(" UNION ALL ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" '' AS MAKER, ");
            selectQuery.Append(" MM.SHORT_NAME AS MAKER_NAME, ");
            selectQuery.Append(" MS.SHORT_NAME AS VENDOR, ");
            selectQuery.Append(" RPD.ROLL_YEAR AS YEAR, ");
            selectQuery.Append(" 0 AS R1, ");
            selectQuery.Append(" 0 AS R2, ");
            selectQuery.Append(" 0 AS R3, ");
            selectQuery.Append(" 0 AS R4, ");
            selectQuery.Append(" 0 AS R5, ");
            selectQuery.Append(" 0 AS KOUBAN_TOTAL, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[0].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[0].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R6, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[1].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[1].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R7, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[2].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[2].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R8, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[3].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[3].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R9, ");
            selectQuery.Append(" ( ( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ( RPD.ROLL_R = '" + forecastRollsYear[4].Item2 + "' ");
            selectQuery.Append(" AND RPD.ROLL_YEAR = '" + forecastRollsYear[4].Item1 + "' ) THEN ");
            selectQuery.Append(" QTY ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) ) AS R10, ");
            selectQuery.Append(" 0 AS TOTAL ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_ROLL_PO_DETAILS RPD ");
            selectQuery.Append(" JOIN MST_SUPPLIER MS ON RPD.SUPPLIER_CD = MS.SUPPLIER_CD ");
            selectQuery.Append(" JOIN MST_MATERIAL MHMS ON RPD.MATERIAL_CD  = MHMS.MATERIAL_CD ");
            selectQuery.Append(" JOIN MST_MATL_NAME_MAKER MMNM ON MHMS.MATL_NAME_MAKER_CD = MMNM.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" JOIN MST_MAKER MM ON MMNM.MAKER_CD = MM.MAKER_CD");
            selectQuery.Append(" ) ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" YEAR = '"+currentDate.Year+"' ");
            selectQuery.Append(" OR YEAR = '"+lastYear.Year+"' ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" ROLLUP(VENDOR) ");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<PoAndReceiptPopupDetails>(query);
            return result;
        }
    }
}
