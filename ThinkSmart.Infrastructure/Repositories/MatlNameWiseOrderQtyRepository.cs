using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class MatlNameWiseOrderQtyRepository : BaseRepository, IMatlNameWiseOrderQtyRepository
    {
        private readonly LocalizationServices _localization;
        public MatlNameWiseOrderQtyRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }
      
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/01/23       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_INT_MATL_FC_SUMMARY Table
        /// </returns>
        public List<MatlNameWiseOrderQtyDetails> GetMaterialOrderQtyData(int Batch_Roll)
        {
            var forecastRollsYear = GetForecastRollYear(Batch_Roll);
            List<MatlNameWiseOrderQtyDetails> result = new List<MatlNameWiseOrderQtyDetails>();
            StringBuilder selectQuery = new StringBuilder();
           
            selectQuery.Append(" SELECT PROD.*, ");
            selectQuery.Append(" (PLAN_QTY_1MONTH_AHEAD + PLAN_QTY_2MONTH_AHEAD + PLAN_QTY_3MONTH_AHEAD + PLAN_QTY_THIS_MONTH + PLAN_QTY_4MONTH_AHEAD) AS TOTALQTY ");
            selectQuery.Append(" FROM ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" INTNAME.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" CASE  WHEN ( GROUPING(matl_kubun) = 1 ) THEN '計' ELSE TO_CHAR(DECODE(FCSUM.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮')) END AS MATL_KUBUN_NAME , ");
            selectQuery.Append(" FCSUM.MATL_KUBUN, ");
            selectQuery.Append(" TO_CHAR(SUM(CASE WHEN PODTL.ROLL_R = '" + forecastRollsYear[0].Item2 + "' AND PODTL.ROLL_YEAR = '" + forecastRollsYear[0].Item1 + "' THEN PODTL.QTY ELSE 0 END)) AS PLAN_QTY_1MONTH_AHEAD, ");
            selectQuery.Append(" TO_CHAR(SUM(CASE WHEN PODTL.ROLL_R = '" + forecastRollsYear[1].Item2 + "' AND PODTL.ROLL_YEAR = '" + forecastRollsYear[1].Item1 + "' THEN PODTL.QTY ELSE 0 END)) AS PLAN_QTY_2MONTH_AHEAD, ");
            selectQuery.Append(" TO_CHAR(SUM(CASE WHEN PODTL.ROLL_R = '" + forecastRollsYear[2].Item2 + "' AND PODTL.ROLL_YEAR = '" + forecastRollsYear[2].Item1 + "' THEN PODTL.QTY ELSE 0 END)) AS PLAN_QTY_3MONTH_AHEAD, ");
            selectQuery.Append(" TO_CHAR(SUM(CASE WHEN PODTL.ROLL_R = '" + forecastRollsYear[3].Item2 + "' AND PODTL.ROLL_YEAR = '" + forecastRollsYear[3].Item1 + "' THEN PODTL.QTY ELSE 0 END)) AS PLAN_QTY_THIS_MONTH, ");
            selectQuery.Append(" TO_CHAR(SUM(CASE WHEN PODTL.ROLL_R = '" + forecastRollsYear[4].Item2 + "' AND PODTL.ROLL_YEAR = '" + forecastRollsYear[4].Item1 + "' THEN PODTL.QTY ELSE 0 END)) AS PLAN_QTY_4MONTH_AHEAD ");
            selectQuery.Append(" FROM TEMP_ROLL_PO_DETAILS PODTL ");
            selectQuery.Append(" INNER JOIN TEMP_INT_MATL_FC_SUMMARY FCSUM ON PODTL.INT_MATL_FC_SUMMARY_CD = FCSUM.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME INTNAME ON INTNAME.INTERNAL_NAME_CD = FCSUM.INTERNAL_NAME_CD ");
            selectQuery.Append(" GROUP BY  ROLLUP(INTNAME.INTERNAL_NAME, FCSUM.MATL_KUBUN) ");
            selectQuery.Append(" ) PROD ");
            //selectQuery.Append(" ORDER BY INTERNAL_NAME_STRING, MATL_KUBUN ");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<MatlNameWiseOrderQtyDetails>(query);
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

    }
}
