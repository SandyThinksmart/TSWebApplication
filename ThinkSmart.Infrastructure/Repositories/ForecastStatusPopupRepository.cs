using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    public class ForecastStatusPopupRepository : BaseRepository, IForecastStatusPopupRepository
    {
        private readonly LocalizationServices _localization;
        public ForecastStatusPopupRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/01/08       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Fetch Data From TRN_SALES_FORECAST Table
        /// </returns>
        public List<ForecastStatusPopupDetails> GetSalesForecastDataList()
        {
            int next_Roll;
            List<ForecastStatusPopupDetails> result = new List<ForecastStatusPopupDetails>();
            StringBuilder selectBatchRoll = new StringBuilder();
            selectBatchRoll.Append(" SELECT VALUE FROM MST_CONFIGURATION WHERE KEY IN('FORECAST_BATCH_ROLL','FORECAST_BATCH_YEAR' )");
            selectBatchRoll.Append(" ORDER BY CASE KEY WHEN 'FORECAST_BATCH_ROLL' THEN 1 WHEN 'FORECAST_BATCH_YEAR' THEN 2 END ");
            List<int> batch_Data = ExecuteQueryMultiResult<int>(selectBatchRoll.ToString());

            StringBuilder selectBatchRollDeptCd = new StringBuilder();
            selectBatchRollDeptCd.Append(" SELECT VALUE FROM MST_CONFIGURATION WHERE KEY IN('SALES_DEPARTMENT_CD' )");
            List<string> salesDeptList = ExecuteQueryMultiResult<string>(selectBatchRollDeptCd.ToString());
            string[] salesDeptCdArray = salesDeptList.ToArray();
            if (batch_Data != null)
            {
                StringBuilder selectQuery = new StringBuilder();
                int batch_Roll = batch_Data[0]; // at 0 th index bind batch roll and 1 st index batch year
                int batch_Year = batch_Data[1];

                selectQuery.Append(" SELECT ");
                selectQuery.Append(" EMP.SHORT_NAME AS EMPLOYEE_NAME,");
                selectQuery.Append(" DEPT.DEPARTMENT_NAME,");
                selectQuery.Append(" CASE WHEN SF.SALES_FC_DONE_DATE IS NULL THEN 0 ELSE 1 END AS ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_DEPARTMENT_GROUP DEPTGR ");
                selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DEPTGR.EMPLOYEE_CD AND EMP.IN_ACTIVE_FLAG = 0  ");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT DEPT ON DEPT.DEPARTMENT_CD = DEPTGR.DEPARTMENT_CD AND DEPT.DEPARTMENT_CD in("+ salesDeptCdArray[0] + ")");
                selectQuery.Append(" LEFT JOIN TRN_SALES_FORECAST SF ON SF.EMP_CD=DEPTGR.EMPLOYEE_CD AND SF.ROLL_FC_YEAR = " + batch_Year + " AND SF.ROLL_FC_R = " + batch_Roll + " ");
                selectQuery.Append(" GROUP BY  EMP.SHORT_NAME,DEPT.DEPARTMENT_NAME,SF.SALES_FC_DONE_DATE");
                selectQuery.Append(" ORDER BY  EMP.SHORT_NAME,DEPT.DEPARTMENT_NAME");
                var query = selectQuery.ToString();
                selectQuery.Append(query);
                result = ExecuteQueryMultiResult<ForecastStatusPopupDetails>(query);
            }


            return result;
        }
    }
}
