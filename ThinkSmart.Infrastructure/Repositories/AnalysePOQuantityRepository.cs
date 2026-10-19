using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class AnalysePOQuantityRepository : BaseRepository, IAnalysePOQuantityRepository
    {
        private readonly string _connectionString;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly Boolean conditionPOMain = false;
        public AnalysePOQuantityRepository(IConfiguration _configuration, IHostEnvironment hostEnvironment) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _hostEnvironment = hostEnvironment;
            conditionPOMain = Convert.ToBoolean(_configuration.GetSection("ConditionPOMain").Value);
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/25      
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// </summary>
        /// <returns>
        /// Get data From Master Table MST_MUKESAKI
        /// </returns>
        /// 
        public List<NameValue> GetMukesakiList()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" MUKE.SHORT_NAME AS Name,");
            query.Append(" MUKE.MUKESAKI_CD AS CD ");
            query.Append(" FROM MST_MUKESAKI MUKE ");
            query.Append(" LEFT JOIN MST_MATERIAL MATL ON MUKE.MUKESAKI_CD = MATL.MUKESAKI_CD ");
            query.Append(" WHERE MUKE.IN_ACTIVE_FLAG = 0 ");
            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/25     
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// </summary>
        /// <returns>
        /// Get data From Master Table MST_SUPPLIER
        /// </returns>
        /// 
        public List<NameValue> GetSupplierList()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" DISTINCT(SUPP.SUPPLIER_CD) AS CD,  ");
            query.Append(" SUPP.SHORT_NAME AS Name ");
            query.Append(" FROM MST_SUPPLIER SUPP ");
            query.Append(" LEFT JOIN MST_MATERIAL MATL ON SUPP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            query.Append(" WHERE SUPP.IN_ACTIVE_FLAG = 0 ");
            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/25     
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// </summary>
        /// <returns>
        /// Get data From Master Table MST_MOCHIKOMISAKI
        /// </returns>
        /// 
        public List<NameValue> GetDestinationList()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" DISTINCT(MOCH.MOCHIKOMISAKI_CD) AS CD,  ");
            query.Append(" MOCH.SHORT_NAME AS Name ");
            query.Append(" FROM MST_MOCHIKOMISAKI MOCH ");
            query.Append(" LEFT JOIN MST_MATERIAL MATL ON MOCH.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");
            query.Append(" WHERE MOCH.IN_ACTIVE_FLAG = 0 ");
            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/25     
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// </summary>
        /// <returns>
        /// Get data From Master Table MST_HIKITORISAKI
        /// </returns>
        /// 
        public List<NameValue> GetHikitorisakiList()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" DISTINCT(HIKI.HTSAKI_CD) AS CD,  ");
            query.Append(" HIKI.SHORT_NAME AS Name ");
            query.Append(" FROM MST_HIKITORISAKI HIKI ");
            query.Append(" LEFT JOIN MST_MATERIAL MATL ON HIKI.HTSAKI_CD = MATL.HTSAKI_CD ");
            query.Append(" WHERE HIKI.IN_ACTIVE_FLAG = 0 ");
            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/25     
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// </summary>
        /// <returns>
        /// Get data From Master Table MST_FUTOU
        /// </returns>
        /// 
        public List<NameValue> GetFutouList()
        {
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" FUTOU_CD AS CD,");
            query.Append(" FUTOU_NAME AS Name ");
            query.Append(" FROM MST_FUTOU ");
            query.Append(" WHERE IN_ACTIVE_FLAG = 0 ");
            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/25     
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// <param name="MATERIAL_CD">
        /// </param>
        /// </summary>
        /// <returns>
        /// Delete Data from TEMP_ROLL_PO_DETAILS
        /// Insert Data into  TEMP_ROLL_PO_DETAILS
        /// </returns>
        /// 
        public string SaveMaterialPoData(List<RollPoMainAnalysisDetails> PODetails)
        {
            string result = "";
            float totalQty = 0;
            int temp_Roll_Month = 0;
            int temp_Roll_Year = 0;
            int INT_MATL_FC_SUMMARY_CD = 0;
            bool chechbox_Value = false;
            //int curr_Roll_Month = 0;
            //int curr_Roll_Year = 0;
            for (int i = 0; i <= 0; i++)
            {
                chechbox_Value = PODetails[i].CHECKEDBOX_VALUE;
                INT_MATL_FC_SUMMARY_CD = PODetails[i].INT_MATL_FC_SUMMARY_CD;
                //curr_Roll_Month = Convert.ToInt16(PODetails[i].ROLL_R);
                //curr_Roll_Year = Convert.ToInt16(PODetails[i].ROLL_YEAR);
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                StringBuilder deleteSql = new StringBuilder();
                deleteSql.Append(" DELETE FROM TEMP_ROLL_PO_DETAILS ");
                deleteSql.Append(" WHERE ");
                deleteSql.Append(" INT_MATL_FC_SUMMARY_CD  ='" + INT_MATL_FC_SUMMARY_CD + "'");
                ExecuteNonQuery(deleteSql.ToString());

                for (int i = 0; i < PODetails.Count; i++)
                {
                    if (PODetails[i].MATERIAL_CD != null)
                    {
                        StringBuilder insertSql = new StringBuilder();
                        insertSql.Append("INSERT INTO TEMP_ROLL_PO_DETAILS(");
                        insertSql.Append("ROLL_PO_DETAIL_CD,");
                        insertSql.Append("INT_MATL_FC_SUMMARY_CD,");
                        insertSql.Append("MATERIAL_CD,");
                        insertSql.Append("MUKESAKI_CD,");
                        insertSql.Append("QTY,");
                        insertSql.Append("ROLL_YEAR,");
                        insertSql.Append("ROLL_R,");
                        insertSql.Append("ROLL_CHANCE,");
                        insertSql.Append("SUPPLIER_CD,");
                        insertSql.Append("MOCHIKOMISAKI_CD,");
                        insertSql.Append("FUTOU_CD,");
                        insertSql.Append("HTSAKI_CD,");
                        insertSql.Append("REMARKS,");
                        insertSql.Append("USER_ID,");
                        insertSql.Append("CREATED_DATE,");
                        insertSql.Append("UPDATED_DATE,");
                        insertSql.Append("RATE ");
                        insertSql.Append(")");
                        insertSql.Append("VALUES( ");
                        insertSql.Append(" ROLL_PO_DETAIL_CD_SEQ.NEXTVAL,");
                        insertSql.Append("'" + PODetails[i].INT_MATL_FC_SUMMARY_CD + "',");
                        insertSql.Append("'" + PODetails[i].MATERIAL_CD + "',");
                        insertSql.Append("'" + PODetails[i].MUKESAKI_CD + "',");
                        insertSql.Append("'" + PODetails[i].QTY + "',");
                        insertSql.Append("'" + PODetails[i].ROLL_YEAR + "',");
                        insertSql.Append("'" + PODetails[i].ROLL_R + "',");
                        insertSql.Append("'" + PODetails[i].ROLL_CHANCE + "',");
                        insertSql.Append("'" + PODetails[i].SUPPLIER_CD + "',");
                        insertSql.Append("'" + PODetails[i].MOCHIKOMISAKI_CD + "',");
                        insertSql.Append("'" + PODetails[i].FUTOU_CD + "',");
                        insertSql.Append("'" + PODetails[i].HTSAKI_CD + "',");
                        insertSql.Append("'" + PODetails[i].REMARKS + "',");
                        insertSql.Append("'" + PODetails[i].USER_ID + "'");
                        insertSql.Append(",SYSDATE");
                        insertSql.Append(",SYSDATE ");
                        insertSql.Append(",'" + PODetails[i].RATE + "'");
                        insertSql.Append(")");
                        ExecuteNonQuery(insertSql.ToString());
                    }
                }

                StringBuilder selectQuery1 = new StringBuilder();
                RollPoMainAnalysisDetails configData = new RollPoMainAnalysisDetails();
                selectQuery1.Append("SELECT ");
                selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_YEAR' THEN VALUE END) AS ROLL_YEAR, ");
                selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_ROLL' THEN VALUE END) AS ROLL_R ");
                selectQuery1.Append("FROM MST_CONFIGURATION ");
                selectQuery1.Append("WHERE KEY IN ('FORECAST_BATCH_YEAR', 'FORECAST_BATCH_ROLL')");
                var query1 = selectQuery1.ToString();
                selectQuery1.Append(query1);
                configData = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query1);

                //curr_Roll_Month = Convert.ToInt16(configData.ROLL_R);
                //curr_Roll_Year = Convert.ToInt16(configData.ROLL_YEAR);
                //int roll_1Month_Ahead = curr_Roll_Month + 1;
                //int roll_1Year_Ahead = curr_Roll_Year;
                //int roll_1Month_Before = curr_Roll_Month - 1;
                //int roll_1Year_Before = curr_Roll_Year;
                //int roll_2Month_Before = curr_Roll_Month - 2;
                //int roll_2Year_Before = curr_Roll_Year;
                //int roll_3Month_Before = curr_Roll_Month - 3;
                //int roll_3Year_Before = curr_Roll_Year;


                int curr_Roll_Month = Convert.ToInt16(configData.ROLL_R);
                int curr_Roll_Year = Convert.ToInt16(configData.ROLL_YEAR);

                // Create a date (use day = 1)
                DateTime currentDate = new DateTime(curr_Roll_Year, curr_Roll_Month, 1);

                // 1 month ahead
                DateTime date_1Month_Ahead = currentDate.AddMonths(1);
                int roll_1Month_Ahead = date_1Month_Ahead.Month;
                int roll_1Year_Ahead = date_1Month_Ahead.Year;

                // 1 month before
                DateTime date_1Month_Before = currentDate.AddMonths(-1);
                int roll_1Month_Before = date_1Month_Before.Month;
                int roll_1Year_Before = date_1Month_Before.Year;

                // 2 months before
                DateTime date_2Month_Before = currentDate.AddMonths(-2);
                int roll_2Month_Before = date_2Month_Before.Month;
                int roll_2Year_Before = date_2Month_Before.Year;

                // 3 months before
                DateTime date_3Month_Before = currentDate.AddMonths(-3);
                int roll_3Month_Before = date_3Month_Before.Month;
                int roll_3Year_Before = date_3Month_Before.Year;


                List<RollPoMainAnalysisDetails> curr_Po_Details = new List<RollPoMainAnalysisDetails>();
                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append(" SELECT  ");
                selectQuery.Append(" ROLL_YEAR,");
                selectQuery.Append(" ROLL_R,");
                selectQuery.Append(" SUM(QTY) TOTALQTY ");
                selectQuery.Append(" FROM TEMP_ROLL_PO_DETAILS ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" INT_MATL_FC_SUMMARY_CD  ='" + INT_MATL_FC_SUMMARY_CD + "'");
                selectQuery.Append(" AND ( ");
                selectQuery.Append(" (ROLL_R = '" + curr_Roll_Month + "' AND ROLL_YEAR = '" + curr_Roll_Year + "' )  ");
                selectQuery.Append(" OR (ROLL_R = '" + roll_1Month_Ahead + "' AND ROLL_YEAR = '" + roll_1Year_Ahead + "' )  ");
                selectQuery.Append(" OR (ROLL_R = '" + roll_1Month_Before + "' AND ROLL_YEAR = '" + roll_1Year_Before + "' )  ");
                selectQuery.Append(" OR (ROLL_R = '" + roll_2Month_Before + "' AND ROLL_YEAR = '" + roll_2Year_Before + "' )  ");
                selectQuery.Append(" OR (ROLL_R = '" + roll_3Month_Before + "' AND ROLL_YEAR = '" + roll_3Year_Before + "' )  ");
                selectQuery.Append(" ) ");
                selectQuery.Append(" GROUP BY ROLL_YEAR, ");
                selectQuery.Append(" ROLL_R ");
                selectQuery.Append(" ORDER BY ROLL_R ");
                var query2 = selectQuery.ToString();
                curr_Po_Details = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(query2);

                StringBuilder updatePlantQty = new StringBuilder();
                updatePlantQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY SET ");
                updatePlantQty.Append(" PLAN_QTY_3MONTH_AHEAD = '',");
                updatePlantQty.Append(" PLAN_QTY_4MONTH_AHEAD = '',");
                updatePlantQty.Append(" PLAN_QTY_2MONTH_AHEAD = '',");
                updatePlantQty.Append(" PLAN_QTY_1MONTH_AHEAD = '',");
                updatePlantQty.Append(" PLAN_QTY_THIS_MONTH = ''");
                updatePlantQty.Append(" WHERE ");
                updatePlantQty.Append(" INT_MATL_FC_SUMMARY_CD  ='" + INT_MATL_FC_SUMMARY_CD + "'");
                ExecuteNonQuery(updatePlantQty.ToString());

                if (curr_Po_Details.Count > 0)
                {
                    for (int i = 0; i <= curr_Po_Details.Count - 1; i++)
                    {
                        temp_Roll_Month = Convert.ToInt16(curr_Po_Details[i].ROLL_R);
                        temp_Roll_Year = Convert.ToInt16(curr_Po_Details[i].ROLL_YEAR);
                        totalQty = Convert.ToSingle(curr_Po_Details[i].TOTALQTY);

                        StringBuilder updateSql = new StringBuilder();
                        updateSql.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY ");
                        if (chechbox_Value == true)
                        {
                            updateSql.Append(" SET PO_REVIEWED_DATE = SYSDATE,");
                        }
                        else
                        {
                            updateSql.Append(" SET PO_REVIEWED_DATE = NULL,");
                        }
                        if (temp_Roll_Month == curr_Roll_Month && temp_Roll_Year == curr_Roll_Year)
                        {
                            updateSql.Append(" PLAN_QTY_3MONTH_AHEAD = '" + totalQty + "' ");
                        }
                        if (temp_Roll_Month == roll_1Month_Ahead && temp_Roll_Year == roll_1Year_Ahead)
                        {
                            updateSql.Append(" PLAN_QTY_4MONTH_AHEAD = '" + totalQty + "' ");
                        }
                        if (temp_Roll_Month == roll_1Month_Before && temp_Roll_Year == roll_1Year_Before)
                        {
                            updateSql.Append(" PLAN_QTY_2MONTH_AHEAD = '" + totalQty + "' ");
                        }
                        if (temp_Roll_Month == roll_2Month_Before && temp_Roll_Year == roll_2Year_Before)
                        {
                            updateSql.Append(" PLAN_QTY_1MONTH_AHEAD = '" + totalQty + "' ");
                        }
                        if (temp_Roll_Month == roll_3Month_Before && temp_Roll_Year == roll_3Year_Before)
                        {
                            updateSql.Append(" PLAN_QTY_THIS_MONTH = '" + totalQty + "' ");
                        }
                        updateSql.Append(" WHERE ");
                        updateSql.Append(" INT_MATL_FC_SUMMARY_CD  ='" + INT_MATL_FC_SUMMARY_CD + "'");
                        ExecuteNonQuery(updateSql.ToString());
                       
                    }
                }
                result = "Data Inserted Successfully";
                transaction.Complete();
            }

            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/03/27       
        /// used screens: AnalysePOQuantity on MaterialMakerListGrid
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_ROLL_PO_DETAILS And TEMP_ROLL_MATL_SUMMARY Table
        /// </returns>
        public List<RollPoMainAnalysisDetails> GetInternalNameWiseMakerList(long INT_MATL_FC_SUMMARY_CD)
        {
            List<RollPoMainAnalysisDetails> result = new List<RollPoMainAnalysisDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAKER.SHORT_NAME AS MAKER_NAME, ");
            selectQuery.Append(" MATLMAKER.MATL_MAKER_NAME,");
            selectQuery.Append(" SUPP.SHORT_NAME AS SUPPLIER_NAME,");
            selectQuery.Append(" FUTOU.FUTOU_NAME AS FUTOU_NAME,");
            selectQuery.Append(" MOCHIKO.SHORT_NAME AS MOCHIKOMISAKI_NAME, ");
            selectQuery.Append(" MATL.MATERIAL_CD, ");
            selectQuery.Append(" MATLSUM.MUKESAKI_RATE AS RATE, ");
            selectQuery.Append(" MU.SHORT_NAME AS MUKESAKI_NAME, ");
            selectQuery.Append(" MATLSUM.MUKESAKI_CD, ");
            selectQuery.Append(" MATLSUM.INT_STOCK_QTY, ");
            selectQuery.Append(" MATLSUM.EXT_STOCK_QTY, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_PREV, ");

            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_1_1, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_1_2, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_1_3, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_1_4, ");

            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_2_1, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_2_2, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_2_3, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_2_4, ");

            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_3_1, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_3_2, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_3_3, ");
            selectQuery.Append(" MATLSUM.PO_REM_QTY_R_3_4, ");

            selectQuery.Append(" ((MATLSUM.PO_REM_QTY_R_1_1) + (MATLSUM.PO_REM_QTY_R_1_2) + (MATLSUM.PO_REM_QTY_R_1_3) + (MATLSUM.PO_REM_QTY_R_1_4) + " +
                                "(MATLSUM.PO_REM_QTY_R_2_1) + (MATLSUM.PO_REM_QTY_R_2_2) + (MATLSUM.PO_REM_QTY_R_2_3) + (MATLSUM.PO_REM_QTY_R_2_4) + " +
                                "(MATLSUM.PO_REM_QTY_R_3_1) + (MATLSUM.PO_REM_QTY_R_3_2) + (MATLSUM.PO_REM_QTY_R_3_3) + (MATLSUM.PO_REM_QTY_R_3_4)) AS PO_REM_QTY_TOTAL, ");

            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_1_1, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_1_2, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_1_3, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_1_4, ");

            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_2_1, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_2_2, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_2_3, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_2_4, ");

            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_3_1, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_3_2, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_3_3, ");
            selectQuery.Append(" MATLSUM.PO_PRV_QTY_R_3_4, ");


            selectQuery.Append(" ((MATLSUM.PO_PRV_QTY_R_1_1) + (MATLSUM.PO_PRV_QTY_R_1_2) + (MATLSUM.PO_PRV_QTY_R_1_3) + (MATLSUM.PO_PRV_QTY_R_1_4) + ");
            selectQuery.Append(" (MATLSUM.PO_PRV_QTY_R_2_1) + (MATLSUM.PO_PRV_QTY_R_2_2) + (MATLSUM.PO_PRV_QTY_R_2_3) + (MATLSUM.PO_PRV_QTY_R_2_4) + ");
            selectQuery.Append(" (MATLSUM.PO_PRV_QTY_R_3_1) + (MATLSUM.PO_PRV_QTY_R_3_2) + (MATLSUM.PO_PRV_QTY_R_3_3) + (MATLSUM.PO_PRV_QTY_R_3_4)");
            selectQuery.Append(" )/12 AS PO_PRE_QTY_TOTAL, ");

            selectQuery.Append(" MATLSUM.NYUKA_QTY_BOFORE_3, ");
            selectQuery.Append(" MATLSUM.NYUKA_QTY_BOFORE_2, ");
            selectQuery.Append(" MATLSUM.NYUKA_QTY_BOFORE_1, ");

            //selectQuery.Append("(NVL(MATLSUM.NYUKA_QTY_BOFORE_3, 0)+NVL(MATLSUM.NYUKA_QTY_BOFORE_2, 0)+NVL(MATLSUM.NYUKA_QTY_BOFORE_1, 0))/3 AS NYUKA_QTY_TOTAL, ");
            selectQuery.Append(" ROUND( ((MATLSUM.NYUKA_QTY_BOFORE_3) + (MATLSUM.NYUKA_QTY_BOFORE_2) + (MATLSUM.NYUKA_QTY_BOFORE_1)) / 3, 1) AS NYUKA_QTY_TOTAL, ");


            selectQuery.Append(" MATLSUM.PO_QTY_R_CM_1, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_CM_2, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_CM_3, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_CM_4, ");

            selectQuery.Append(" MATLSUM.PO_QTY_R_1_1, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_1_2, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_1_3, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_1_4, ");

            selectQuery.Append(" MATLSUM.PO_QTY_R_2_1, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_2_2, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_2_3, ");
            selectQuery.Append(" MATLSUM.PO_QTY_R_2_4, ");

            selectQuery.Append(" MATLSUM.PO_QTY_R_3_1,");
            selectQuery.Append(" MATLSUM.PO_QTY_R_3_2,");
            selectQuery.Append(" MATLSUM.PO_QTY_R_3_3,");
            selectQuery.Append(" MATLSUM.PO_QTY_R_3_4,");

            selectQuery.Append(" MATLSUM.PO_QTY_R_4_1,");
            selectQuery.Append(" MATLSUM.PO_QTY_R_4_2,");
            selectQuery.Append(" MATLSUM.PO_QTY_R_4_3,");
            selectQuery.Append(" MATLSUM.PO_QTY_R_4_4 ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY INNER JOIN TEMP_ROLL_MATL_SUMMARY MATLSUM ON TEMP_INT_MATL_FC_SUMMARY.INT_MATL_FC_SUMMARY_CD = MATLSUM.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" INNER JOIN MST_MUKESAKI MU  ON MU.MUKESAKI_CD = MATLSUM.MUKESAKI_CD");
            selectQuery.Append(" INNER JOIN MST_MATERIAL MATL ON MATLSUM.MATERIAL_CD = MATL.MATERIAL_CD ");
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER MATLMAKER ON MATL.MATL_NAME_MAKER_CD = MATLMAKER.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MAKER MAKER ON MAKER.MAKER_CD = MATLMAKER.MAKER_CD ");
            selectQuery.Append(" LEFT JOIN MST_SUPPLIER SUPP ON SUPP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            selectQuery.Append(" INNER JOIN MST_FUTOU FUTOU ON FUTOU.FUTOU_CD = MATL.FUTOU_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCHIKO ON MOCHIKO.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MATLSUM.INT_MATL_FC_SUMMARY_CD = '" + INT_MATL_FC_SUMMARY_CD + "' ORDER BY MAKER_NAME");

            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/04/04      
        /// used screens: AnalysePOQuantity on MaterialDetails
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_INT_MATL_FC_SUMMARY Table
        /// </returns>
        public RollPoMainAnalysisDetails GetInternalNameWIseMaterialDetails(long INT_MATL_FC_SUMMARY_CD)
        {
            RollPoMainAnalysisDetails result = new RollPoMainAnalysisDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" INTNAME.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" TFS.DIM_D, ");
            selectQuery.Append(" TFS.SHAPE, ");
            selectQuery.Append(" TFS.LENGTH, ");
            selectQuery.Append(" DECODE(TFS.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATL_KUBUN_NAME, ");
            selectQuery.Append(" TFS.MATL_KUBUN,");
            selectQuery.Append(" DECODE(TFS.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN_NAME, ");
            selectQuery.Append(" TFS.MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MSC.CONDITION_DESCRIPTION, ");
            selectQuery.Append(" MS.DIM_W, ");
            selectQuery.Append(" MS.DIM_OTHER, ");
            selectQuery.Append(" TFS.PO_REVIEWED_DATE ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY TFS ");
            selectQuery.Append(" LEFT OUTER JOIN MST_CONDITION MSC ON TFS.CONDITION_CD = MSC.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTNAME ON INTNAME.INTERNAL_NAME_CD = TFS.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN TEMP_ROLL_MATL_SUMMARY ON TEMP_ROLL_MATL_SUMMARY.INT_MATL_FC_SUMMARY_CD=TFS.INT_MATL_FC_SUMMARY_CD ");//change left to inner join
            selectQuery.Append(" LEFT JOIN MST_MATERIAL MS ON MS.MATERIAL_CD=TEMP_ROLL_MATL_SUMMARY.MATERIAL_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" TFS.INT_MATL_FC_SUMMARY_CD = '" + INT_MATL_FC_SUMMARY_CD + "'");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/05/05      
        /// used screens: RollSalesForecast
        /// Fetch Data From MST_MATERIAL Master Table 
        /// </summary>
        /// <param name="INT_MATL_FC_SUMMARY_CD">
        /// </param>
        /// <returns>
        ///Material Master Data
        /// </returns>
        public List<GridSearchFields> GetMaterialMasterDataList(List<SearchField> searchFields, string sortCriteria, long INT_MATL_FC_SUMMARY_CD)
        {

            StringBuilder selectQueryForIntSummary = new StringBuilder();
            selectQueryForIntSummary.Append(" SELECT ");
            selectQueryForIntSummary.Append(" INTNAME.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectQueryForIntSummary.Append(" TFS.DIM_D, ");
            selectQueryForIntSummary.Append(" TFS.SHAPE, ");
            selectQueryForIntSummary.Append(" TFS.LENGTH, ");
            selectQueryForIntSummary.Append(" DECODE(TFS.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATL_KUBUN_NAME, ");
            selectQueryForIntSummary.Append(" DECODE(TFS.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN_NAME, ");
            selectQueryForIntSummary.Append(" MS.CONDITION_DESCRIPTION, ");
            selectQueryForIntSummary.Append(" TFS.INTERNAL_NAME_CD, ");
            selectQueryForIntSummary.Append(" TFS.CONDITION_CD ");
            selectQueryForIntSummary.Append(" FROM ");
            selectQueryForIntSummary.Append(" TEMP_INT_MATL_FC_SUMMARY TFS ");
            selectQueryForIntSummary.Append(" LEFT OUTER JOIN MST_CONDITION MS ON TFS.CONDITION_CD = MS.CONDITION_CD ");
            selectQueryForIntSummary.Append(" LEFT JOIN MST_INTERNAL_NAME INTNAME ON INTNAME.INTERNAL_NAME_CD = TFS.INTERNAL_NAME_CD ");
            selectQueryForIntSummary.Append(" WHERE ");
            selectQueryForIntSummary.Append(" TFS.INT_MATL_FC_SUMMARY_CD = '" + INT_MATL_FC_SUMMARY_CD + "'");
            var query1 = selectQueryForIntSummary.ToString();
            selectQueryForIntSummary.Append(query1);
            var IntMatlFcSummaryData = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query1);

            StringBuilder selectQuery = new StringBuilder();
            List<GridSearchFields> result = new List<GridSearchFields>();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MATL.*,");
            selectQuery.Append(" MTMAK.MATL_MAKER_NAME,");
            selectQuery.Append(" MTINT.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" MAK.SHORT_NAME AS MAKER_NAME,");
            selectQuery.Append(" CON.CONDITION_DESCRIPTION,");
            selectQuery.Append(" SUP.SHORT_NAME AS SUPPLIER_NAME,");
            selectQuery.Append(" FU.FUTOU_NAME AS FUTOU_NAME,");
            selectQuery.Append(" MOCH.MKSAKI_NAME,");
            selectQuery.Append(" MOCH.SHORT_NAME AS MOCHIKOMISAKI_NAME,");
            selectQuery.Append(" ST.STEEL_TYPE_NAME,");
            selectQuery.Append(" HIKI.SHORT_NAME AS HTSAKI_NAME,");
            selectQuery.Append(" MUKE.SHORT_NAME AS MUKESAKI_NAME, ");
            selectQuery.Append(" MATL.MUKESAKI_CD, ");
            selectQuery.Append(" MATL.DIM_D AS MATL_DIM_D, ");
            selectQuery.Append(" MATL.SHAPE AS MATL_SHAPE, ");
            selectQuery.Append(" MATL.DIM_W AS MATL_DIM_W, ");
            selectQuery.Append(" MATL.DIM_OTHER AS MATL_DIM_OTHER, ");
            //selectQuery.Append(" MATLRATE.RATE, ");
            selectQuery.Append(" DECODE(MATL.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATERIAL_TYPE_NAME_CBK, ");
            selectQuery.Append(" DECODE(MATL.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN ");
            //selectQuery.Append(" (GET_FUTOU_NYUKO_QTY_BY_MATL(MATERIAL_CD) - GET_MATL_NYUKA_QTY(MATERIAL_CD) - GET_FUTOU_SHUKKO_QTY_BY_MATL(MATERIAL_CD) ) AS ACTUAL_STOCK ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_MATERIAL MATL ");
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER   MTMAK ON MATL.MATL_NAME_MAKER_CD = MTMAK.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MAKER MAK ON MAK.MAKER_CD = MTMAK.MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME MTINT ON MTMAK.INTERNAL_NAME_CD = MTINT.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_STEEL_TYPE ST ON ST.STEEL_TYPE_CD = MTINT.STEEL_TYPE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = MATL.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_SUPPLIER SUP ON SUP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            selectQuery.Append(" INNER JOIN MST_FUTOU FU ON FU.FUTOU_CD = MATL.FUTOU_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");
            selectQuery.Append(" LEFT JOIN MST_HIKITORISAKI HIKI ON HIKI.HTSAKI_CD = MATL.HTSAKI_CD  ");
            selectQuery.Append(" LEFT JOIN MST_MUKESAKI MUKE ON MUKE.MUKESAKI_CD = MATL.MUKESAKI_CD ");
            //selectQuery.Append(" LEFT JOIN MST_MATERIAL_RATE MATLRATE ON MATLRATE.MATL_NAME_MAKER_CD = MTMAK.MATL_NAME_MAKER_CD ");
            var query2 = selectQuery.ToString();
            string whereCondition = "";
            if (searchFields != null && searchFields.Count > 0)
            {
                whereCondition = " WHERE MATL.SHAPE = '" + IntMatlFcSummaryData.SHAPE + "' AND " +
                     "MATL.DIM_D = '" + IntMatlFcSummaryData.DIM_D + "' AND ";

                if (IntMatlFcSummaryData.LENGTH != null)
                {
                    whereCondition += "MATL.MATERIAL_LENGTH = '" + IntMatlFcSummaryData.LENGTH + "' AND ";
                }
                else
                {
                    whereCondition += "MATL.MATERIAL_LENGTH IS NULL AND ";
                }
                if (conditionPOMain == true)
                {
                    whereCondition += "MATL.CONDITION_CD = '" + IntMatlFcSummaryData.CONDITION_CD + "' AND ";
                }
                whereCondition += "MTINT.INTERNAL_NAME_CD = '" + IntMatlFcSummaryData.INTERNAL_NAME_CD + "' AND " +
                              "MATL.IN_ACTIVE_FLAG = 0 AND ";

                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query2);
                    andOperator = " AND ";
                }
                query2 = query2 + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query2 = query2 + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query2);
            result = ExecuteQueryMultiResult<GridSearchFields>(query2);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/05/06       
        /// used screens: AnalysePOQuantity on MaterialMakerGrid
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_ROLL_PO_DETAILS and TEMP_ROLL_MATL_SUMMARY Table
        /// </returns>
        public RollSalesForecastDetails GetMakerMaterialDetailsByCD(long INT_MATL_FC_SUMMARY_CD, long MATERIAL_CD, short MUKESAKI_CD)
        {
            RollSalesForecastDetails result = new RollSalesForecastDetails();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAKER.SHORT_NAME AS MAKER_NAME, ");
            selectQuery.Append(" MATLMAKER.MATL_MAKER_NAME,");
            selectQuery.Append(" MATLMAKER.MATL_NAME_MAKER_CD,");
            selectQuery.Append(" SUPP.SUPPLIER_CD,");
            selectQuery.Append(" FUTOU.FUTOU_CD,");
            selectQuery.Append(" MOCHIKO.MOCHIKOMISAKI_CD, ");
            //selectQuery.Append(" HIKI.HTSAKI_CD, ");
            selectQuery.Append(" MATL.MATERIAL_CD, ");
            selectQuery.Append(" MATL.DIM_D AS MATL_DIM_D, ");
            selectQuery.Append(" MATL.SHAPE AS MATL_SHAPE, ");
            selectQuery.Append(" MATL.DIM_W AS MATL_DIM_W , ");
            selectQuery.Append(" MATL.DIM_OTHER AS MATL_DIM_OTHER, ");
            selectQuery.Append(" MATL.MATERIAL_LENGTH, ");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION ,");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY.ROLL_YEAR, TEMP_INT_MATL_FC_SUMMARY.ROLL_R ,");
            selectQuery.Append(" DECODE(MATL.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATERIAL_TYPE_NAME_CBK, ");
            selectQuery.Append(" MATL.MATL_KUBUN,");
            selectQuery.Append(" DECODE(MATL.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MATLSUM.MUKESAKI_RATE AS RATE, ");
            selectQuery.Append(" MATLSUM.MUKESAKI_CD AS MUKESAKI_CD ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY INNER JOIN  TEMP_ROLL_MATL_SUMMARY   MATLSUM  ON TEMP_INT_MATL_FC_SUMMARY.INT_MATL_FC_SUMMARY_CD = MATLSUM.INT_MATL_FC_SUMMARY_CD  ");
            selectQuery.Append(" INNER JOIN MST_MATERIAL MATL ON MATLSUM.MATERIAL_CD = MATL.MATERIAL_CD ");
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER MATLMAKER ON MATL.MATL_NAME_MAKER_CD = MATLMAKER.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MAKER MAKER ON MAKER.MAKER_CD = MATLMAKER.MAKER_CD ");
            selectQuery.Append(" LEFT JOIN MST_SUPPLIER SUPP ON SUPP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            selectQuery.Append(" INNER JOIN MST_FUTOU FUTOU ON FUTOU.FUTOU_CD = MATL.FUTOU_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCHIKO ON MOCHIKO.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");
            //selectQuery.Append(" LEFT JOIN MST_HIKITORISAKI HIKI ON HIKI.HTSAKI_CD = MATL.HTSAKI_CD ");
            selectQuery.Append(" LEFT OUTER JOIN MST_CONDITION CND ON MATL.CONDITION_CD = CND.CONDITION_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MATLSUM.INT_MATL_FC_SUMMARY_CD = '" + INT_MATL_FC_SUMMARY_CD + "'");
            selectQuery.Append(" AND MATLSUM.MATERIAL_CD = '" + MATERIAL_CD + "'");
            selectQuery.Append(" AND MATLSUM.MUKESAKI_CD = '" + MUKESAKI_CD + "'");
            //selectQuery.Append(" GROUP BY  ");
            //selectQuery.Append(" MAKER.SHORT_NAME, ");
            //selectQuery.Append(" MATLMAKER.MATL_MAKER_NAME, ");
            //selectQuery.Append(" MOCHIKO.MOCHIKOMISAKI_CD, ");
            //selectQuery.Append(" SUPP.SUPPLIER_CD, ");
            //selectQuery.Append(" FUTOU.FUTOU_CD, ");
            ////selectQuery.Append(" HIKI.HTSAKI_CD, ");
            //selectQuery.Append(" MATL.MATERIAL_CD, ");
            //selectQuery.Append(" MATL.DIM_D, ");
            //selectQuery.Append(" MATL.SHAPE, ");
            //selectQuery.Append(" MATL.DIM_W, ");
            //selectQuery.Append(" MATL.DIM_OTHER, ");
            //selectQuery.Append(" MATL.MATERIAL_LENGTH, ");
            //selectQuery.Append(" CND.CONDITION_DESCRIPTION, ");
            //selectQuery.Append(" DECODE(MATL.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮'), ");
            //selectQuery.Append(" DECODE(MATL.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別'), ");
            //selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY.ROLL_YEAR, ");
            //selectQuery.Append(" TEMP_INT_MATL_FC_SUMMARY.ROLL_R ");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQuerySingleResult<RollSalesForecastDetails>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/05/21       
        /// used screens: AnalysePOQuantity on MaterialPoGrid
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_ROLL_PO_DETAILS  Table
        /// </returns>
        public List<RollPoMainAnalysisDetails> GetRollPoDetails(long INT_MATL_FC_SUMMARY_CD)
        {
            List<RollPoMainAnalysisDetails> result = new List<RollPoMainAnalysisDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MK.SHORT_NAME AS MAKER_NAME, ");
            selectQuery.Append(" MNM.MATL_MAKER_NAME, ");
            selectQuery.Append(" MNM.MATL_NAME_MAKER_CD,");
            selectQuery.Append(" MS.DIM_D AS MATL_DIM_D, ");
            selectQuery.Append(" MS.SHAPE AS MATL_SHAPE, ");
            selectQuery.Append(" MS.DIM_W AS MATL_DIM_W , ");
            selectQuery.Append(" MS.DIM_OTHER AS MATL_DIM_OTHER, ");
            selectQuery.Append(" MS.MATERIAL_LENGTH, ");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION ,");
            selectQuery.Append(" DECODE(MS.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATERIAL_TYPE_NAME_CBK, ");
            selectQuery.Append(" MS.MATL_KUBUN,");
            selectQuery.Append(" DECODE(MS.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN_NAME, ");
            //selectQuery.Append(" TRPD.RATE,");
            selectQuery.Append(" TRPD.* ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TEMP_ROLL_PO_DETAILS TRPD ");
            selectQuery.Append(" INNER JOIN MST_MATERIAL MS ON MS.MATERIAL_CD = TRPD.MATERIAL_CD ");
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER MNM ON MNM.MATL_NAME_MAKER_CD=MS.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MAKER MK ON MK.MAKER_CD=MNM.MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_CONDITION CND ON CND.CONDITION_CD=MS.CONDITION_CD ");
            selectQuery.Append(" INNER JOIN TEMP_INT_MATL_FC_SUMMARY ON TEMP_INT_MATL_FC_SUMMARY.INT_MATL_FC_SUMMARY_CD = TRPD.INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" TRPD.INT_MATL_FC_SUMMARY_CD = '" + INT_MATL_FC_SUMMARY_CD + "'");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/05/29   
        /// used screens: AnalysePoQty
        /// </summary>
        /// <param name="MATERIAL_CD"
        /// <returns>
        /// Fetch Data From MST_MATERIAL Master Table Using MATERIAL_CD
        /// </returns>
        /// 
        public RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD, short? MUKESAKI_CD)
        {
            StringBuilder selectQuery1 = new StringBuilder();
            RollPoMainAnalysisDetails configData = new RollPoMainAnalysisDetails();
            selectQuery1.Append("SELECT ");
            selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_YEAR' THEN VALUE END) AS ROLL_YEAR, ");
            selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_ROLL' THEN VALUE END) AS ROLL_R ");
            selectQuery1.Append("FROM MST_CONFIGURATION ");
            selectQuery1.Append("WHERE KEY IN ('FORECAST_BATCH_YEAR', 'FORECAST_BATCH_ROLL')");
            var query1 = selectQuery1.ToString();
            selectQuery1.Append(query1);
            configData = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query1);

            StringBuilder selectQuery2 = new StringBuilder();
            RollSalesForecastDetails result = new RollSalesForecastDetails();
            selectQuery2.Append(" SELECT ");
            selectQuery2.Append(" MATL.*,");
            selectQuery2.Append(" MTMAK.MATL_MAKER_NAME,");
            selectQuery2.Append(" MTMAK.MATL_NAME_MAKER_CD,");
            selectQuery2.Append(" MTINT.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery2.Append(" MAK.SHORT_NAME AS MAKER_NAME,");
            selectQuery2.Append(" CON.CONDITION_DESCRIPTION,");
            selectQuery2.Append(" MATL.DIM_D AS MATL_DIM_D, ");
            selectQuery2.Append(" MATL.SHAPE AS MATL_SHAPE, ");
            selectQuery2.Append(" MATL.DIM_W AS MATL_DIM_W, ");
            selectQuery2.Append(" MATL.DIM_OTHER AS MATL_DIM_OTHER, ");
            selectQuery2.Append("  " + configData.ROLL_YEAR + " AS ROLL_YEAR, ");
            selectQuery2.Append("  " + configData.ROLL_R + " AS ROLL_R, ");
            selectQuery2.Append(" DECODE(MATL.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATERIAL_TYPE_NAME_CBK, ");
            selectQuery2.Append(" MATL.MATL_KUBUN,");
            selectQuery2.Append(" DECODE(MATL.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN, ");
            if (MUKESAKI_CD != null)
            {
                selectQuery2.Append(" (SELECT GET_MATERIAL_RATE(MTMAK.MATL_NAME_MAKER_CD, MATL.DIM_D, " + MUKESAKI_CD + ", MATL.MATL_KUBUN) AS MUKESAKIRATE  FROM DUAL ) AS RATE ");
            }
            else
            {
                selectQuery2.Append(" NULL AS RATE ");
            }
                selectQuery2.Append(" FROM ");
            selectQuery2.Append(" MST_MATERIAL MATL ");
            selectQuery2.Append(" INNER JOIN MST_MATL_NAME_MAKER   MTMAK ON MATL.MATL_NAME_MAKER_CD = MTMAK.MATL_NAME_MAKER_CD ");
            selectQuery2.Append(" INNER JOIN MST_MAKER MAK ON MAK.MAKER_CD = MTMAK.MAKER_CD ");
            selectQuery2.Append(" INNER JOIN MST_INTERNAL_NAME MTINT ON MTMAK.INTERNAL_NAME_CD = MTINT.INTERNAL_NAME_CD ");
            selectQuery2.Append(" INNER JOIN MST_STEEL_TYPE ST ON ST.STEEL_TYPE_CD = MTINT.STEEL_TYPE_CD ");
            selectQuery2.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = MATL.CONDITION_CD ");
            selectQuery2.Append(" LEFT JOIN MST_SUPPLIER SUP ON SUP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            selectQuery2.Append(" INNER JOIN MST_FUTOU FU ON FU.FUTOU_CD = MATL.FUTOU_CD ");
            selectQuery2.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");
            selectQuery2.Append(" LEFT JOIN MST_HIKITORISAKI HIKI ON HIKI.HTSAKI_CD = MATL.HTSAKI_CD  ");
            selectQuery2.Append(" LEFT JOIN MST_MUKESAKI MUKE ON MUKE.MUKESAKI_CD = MATL.MUKESAKI_CD ");
            selectQuery2.Append("  WHERE MATL.MATERIAL_CD = " + MATERIAL_CD + " ");
            if (MUKESAKI_CD != null)
            {
                selectQuery2.Append("AND (MATL.MUKESAKI_CD = " + MUKESAKI_CD + " ) FETCH FIRST ROW ONLY ");
            }
            else
            {
                selectQuery2.Append("AND MATL.MUKESAKI_CD IS NULL FETCH FIRST ROW ONLY ");
            }
            result = ExecuteQuerySingleResult<RollSalesForecastDetails>(selectQuery2.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/06/09   
        /// used screens: AnalysePoQty
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_INT_MATL_FC_SUMMARY 
        /// checking PO_REVIEWED_DATE is updated or not into TEMP_INT_MATL_FC_SUMMARY table
        /// </returns>
        /// 
        public int checkAnalysisOverFlag(long INT_MATL_FC_SUMMARY_CD)
        {
            StringBuilder selectQuery1 = new StringBuilder();
            RollPoMainAnalysisDetails configData = new RollPoMainAnalysisDetails();
            selectQuery1.Append("SELECT ");
            selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_YEAR' THEN VALUE END) AS ROLL_YEAR, ");
            selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_ROLL' THEN VALUE END) AS ROLL_R ");
            selectQuery1.Append("FROM MST_CONFIGURATION ");
            selectQuery1.Append("WHERE KEY IN ('FORECAST_BATCH_YEAR', 'FORECAST_BATCH_ROLL')");
            var query1 = selectQuery1.ToString();
            selectQuery1.Append(query1);
            configData = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query1);

            StringBuilder selectQuery2 = new StringBuilder();
            selectQuery2.Append(" SELECT ");
            selectQuery2.Append(" COUNT(*) ");
            selectQuery2.Append(" FROM ");
            selectQuery2.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            selectQuery2.Append(" WHERE ");
            selectQuery2.Append(" ROLL_YEAR = " + configData.ROLL_YEAR + "  ");
            selectQuery2.Append(" AND ROLL_R = " + configData.ROLL_R + "  ");
            selectQuery2.Append(" AND PO_REVIEWED_DATE IS NOT NULL ");
            selectQuery2.Append(" AND INT_MATL_FC_SUMMARY_CD = " + INT_MATL_FC_SUMMARY_CD + "");
            var query2 = selectQuery2.ToString();
            selectQuery2.Append(query2);
            int result = Convert.ToInt16(ExecuteScalar(query2));
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/06/09   
        /// used screens: AnalysePoQty
        /// </summary>
        /// <returns>
        /// Fetch Data From TEMP_INT_MATL_FC_SUMMARY 
        /// checking PO_REVIEWED_DATE is updated or not into TEMP_INT_MATL_FC_SUMMARY table
        /// </returns>
        /// 
        public int checkPoCompleteFlag()
        {
            StringBuilder selectQuery1 = new StringBuilder();
            RollPoMainAnalysisDetails configData = new RollPoMainAnalysisDetails();
            selectQuery1.Append("SELECT ");
            selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_YEAR' THEN VALUE END) AS ROLL_YEAR, ");
            selectQuery1.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_ROLL' THEN VALUE END) AS ROLL_R ");
            selectQuery1.Append("FROM MST_CONFIGURATION ");
            selectQuery1.Append("WHERE KEY IN ('FORECAST_BATCH_YEAR', 'FORECAST_BATCH_ROLL')");
            var query1 = selectQuery1.ToString();
            selectQuery1.Append(query1);
            configData = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query1);
            string formattedRollR = FormatWithLeadingZero(Convert.ToInt16(configData.ROLL_R), "D2");
            StringBuilder selectQuery2 = new StringBuilder();
            selectQuery2.Append(" SELECT ");
            selectQuery2.Append(" COUNT(*) ");
            selectQuery2.Append(" FROM ");
            selectQuery2.Append(" TRN_PO_HDR ");
            selectQuery2.Append(" WHERE ");
            selectQuery2.Append(" MONTHLY_PO_YEAR_R = '" + configData.ROLL_YEAR.ToString() + formattedRollR + "' ");
            var query2 = selectQuery2.ToString();
            selectQuery2.Append(query2);
            int result = Convert.ToInt16(ExecuteScalar(query2));
            return result;
        }
        public string FormatWithLeadingZero(int number, string format)
        {
            return number.ToString(format);
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2026/04/13   
        /// used screens: AnalysePoQty
        /// </summary>
        /// <returns>
        /// Fetch Mukesaki Rate from GET_MATERIAL_RATE funtion
        /// </returns>
        /// 
        public float GetMukesakiMaterialRate(short mukesakiCd, decimal matlDimD, int matlKubun, int matlNameMakerCd)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT GET_MATERIAL_RATE(" + matlNameMakerCd + "," + matlDimD + ", " + mukesakiCd + ", " + matlKubun + ")  AS RATE FROM DUAL ");
            float mukesakiRate = Convert.ToSingle(ExecuteScalar(selectQuery.ToString()));
            return mukesakiRate;
        }

        public string UpdateForecastData(List<RollSalesForecastDetails> forecastDetails)
        {   
            string result = "";

            for (int i = 0; i < forecastDetails.Count; i++)
            {
                bool checkboxValue = forecastDetails[i].CHECKEDBOX_VALUE;
                long? rollFcCd = forecastDetails[i].ROLL_FC_CD;
                string materialReply = forecastDetails[i].REPLY_FROM_MATL_DEPT ?? "";

                StringBuilder updateQuery = new StringBuilder();

                updateQuery.Append("UPDATE TRN_SALES_FORECAST SET ");

                if (checkboxValue)
                {
                    updateQuery.Append("SALES_FC_DONE_DATE = TO_DATE('" +
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") +
                        "','yyyy/MM/dd HH24:MI:SS'), ");
                }
                else
                {
                    updateQuery.Append("SALES_FC_DONE_DATE = NULL, ");
                }

                updateQuery.Append("REPLY_FROM_MATL_DEPT = '" + materialReply.Replace("'", "''") + "', ");

                updateQuery.Append("UPDATED_DATE = TO_DATE('" +
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") +
                "','yyyy/MM/dd HH24:MI:SS') ");

                updateQuery.Append("WHERE ROLL_FC_CD = " + rollFcCd);

                ExecuteNonQuery(updateQuery.ToString());
            }

            return result;
        }
               
    }
}
