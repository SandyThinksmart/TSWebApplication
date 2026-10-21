using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Constants;
using Microsoft.Extensions.Primitives;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProcessPlanHandoverRepository : BaseRepository, IProcessPlanHandoverRepository
    {
        private readonly string _connectionString;
        string result;
        public ProcessPlanHandoverRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/10/26       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Fetch data by using OrNo using table TRN_PROCESS_PLAN
        /// </returns>
        /// 
        public List<GridSearchFields> GetDetailsByChuuban(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();
            
            int count = 0;
            int completeDateCnt = 0;
                for(int i = 0;i< processPlanDetails.Count;i++) { 
                    //string chuubanNo = processPlanDetails[i].CHUUBAN.Substring(0, 9);
                    //string gyoubanNo = processPlanDetails[i].CHUUBAN.Substring(9, 2);

                    StringBuilder selectQueryProcessCnt = new StringBuilder();
                    selectQueryProcessCnt.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
                    selectQueryProcessCnt.Append(" WHERE ");
                    selectQueryProcessCnt.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "' ");
                    count = Convert.ToInt32(ExecuteScalar(selectQueryProcessCnt .ToString()));
                if (count > 0)
                {
                    StringBuilder selectQueryComplProcessCnt = new StringBuilder();
                    selectQueryComplProcessCnt.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
                    selectQueryComplProcessCnt.Append(" WHERE ");
                    selectQueryComplProcessCnt.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "' ");
                    selectQueryComplProcessCnt.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
                    completeDateCnt = Convert.ToInt32(ExecuteScalar(selectQueryComplProcessCnt.ToString()));
                    if (completeDateCnt > 0)
                    {
                        StringBuilder selectQuery = new StringBuilder();
                        if (processPlanDetails[i].DISPLAYPROCESSPLANDATA == "beforeProcessStart")//
                        {
                            selectQuery.Append(" SELECT CUST.SHORT_NAME AS CUSTOMER_NAME, ");
                            selectQuery.Append(" PP.PROD_NAME_CUST,");
                            selectQuery.Append(" PP.PROD_SHAPE,");
                            selectQuery.Append(" PP.PROD_DIM_D,");
                            selectQuery.Append(" PP.PROD_DIM_W,");
                            selectQuery.Append(" TO_CHAR(PP.PROD_DIM_OTHER) AS PROD_DIM_OTHER_STRING,");
                            selectQuery.Append(" PP.PROD_LENGTH,");
                            selectQuery.Append(" PP.HT_CONDITION,");
                            selectQuery.Append(" PP.CHARGE,");
                            selectQuery.Append(" PP.ACT_ALLOC_CD,");
                            selectQuery.Append(" PP.ACT_ALLOC_QTY,");
                            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE,'YYYY/MM/DD HH24:MI') AS ACT_ALLOC_DATE_STRING,");
                            selectQuery.Append(" PP.NO_OF_UNITS,");
                            selectQuery.Append(" PP.UNIT,");
                            selectQuery.Append(" PP.PROD_SPEC_NO,");
                            selectQuery.Append(" PP.PROCESS_PLAN_CD,");
                            selectQuery.Append(" PP.LOCATION_CD, ");
                            selectQuery.Append(" CONCAT(CHUUBAN,CASE WHEN LENGTH(TRIM(GYOUBAN))=1 THEN '0'||GYOUBAN END) AS CHUUBAN_GYOUBAN ");
                            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
                            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                           // selectQuery.Append(" LEFT JOIN TRN_WIP_STATUS WIP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
                            selectQuery.Append(" WHERE CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "' AND PROCESS_PLAN_START_DATE IS NULL AND FIRST_PROCESS_CD IS NULL ");
                            var beforeProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                            if (beforeProcessPlanData != null)
                            {
                                result.Add(beforeProcessPlanData);
                            }
                            else
                            {

                                GridSearchFields message = new GridSearchFields()
                                {CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN , RESPONCE = "Process Is Already Started" };
                                result.Add(message);




                            }
                        }
                        else
                        {
                            selectQuery.Append(" SELECT CUST.SHORT_NAME AS CUSTOMER_NAME, ");
                            selectQuery.Append(" PP.PROD_NAME_CUST,");
                            selectQuery.Append(" PP.PROD_SHAPE,");
                            selectQuery.Append(" PP.PROD_DIM_D,");
                            selectQuery.Append(" PP.PROD_DIM_W,");
                            selectQuery.Append(" TO_CHAR(PP.PROD_DIM_OTHER) AS PROD_DIM_OTHER_STRING,");
                            selectQuery.Append(" PP.PROD_LENGTH,");
                            selectQuery.Append(" PP.HT_CONDITION,");
                            selectQuery.Append(" PP.CHARGE,");
                            selectQuery.Append(" PP.ACT_ALLOC_CD,");
                            selectQuery.Append(" PP.ACT_ALLOC_QTY,");
                            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE,'YYYY/MM/DD HH24:MI') AS ACT_ALLOC_DATE_STRING,");
                            selectQuery.Append(" PP.NO_OF_UNITS,");
                            selectQuery.Append(" PP.UNIT,");
                            selectQuery.Append(" PP.PROD_SPEC_NO,");
                            selectQuery.Append(" PP.PROCESS_PLAN_CD,");
                            selectQuery.Append(" PP.LOCATION_CD, ");
                            selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_CD, ");
                            selectQuery.Append(" CONCAT(CHUUBAN,CASE WHEN LENGTH(TRIM(GYOUBAN))=1 THEN '0'||GYOUBAN END) AS CHUUBAN_GYOUBAN ");
                            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
                            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                            selectQuery.Append(" INNER JOIN TRN_WIP_STATUS WIP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
                            selectQuery.Append(" WHERE CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "' AND PROCESS_PLAN_START_DATE IS NOT NULL AND FIRST_PROCESS_CD IS NOT NULL ");
                            var afterProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                            if (afterProcessPlanData != null)
                            {
                                result.Add(afterProcessPlanData);
                            }
                            else
                            {
                                GridSearchFields message = new GridSearchFields()
                                { CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN, RESPONCE = "Process Is Not Started" };
                                result.Add(message);
                            }
                        }

                       
                    }
                    else
                    {
                        GridSearchFields message = new GridSearchFields()
                        { CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN, RESPONCE = "Process Is Completed" };
                        result.Add(message);
                    }
                }
                else
                {
                    GridSearchFields message = new GridSearchFields()
                    { CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN, RESPONCE = "Or No Not Found" };
                    result.Add(message);
                }

            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/10/30       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Update data into table TRN_PROCESS_PLAN 
        /// Save data into table TRN_WIP_STATUS
        /// </returns>
        /// 
        public string SaveProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {
            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                //string chuubanNo = processPlanDetails[i].CHUUBAN_GYOUBAN.Substring(0, 9);
                //string gyoubanNo = processPlanDetails[i].CHUUBAN_GYOUBAN.Substring(9, 2);
                int processPlanCD = Convert.ToInt16(processPlanDetails[i].PROCESS_PLAN_CD);
                int locationCD =Convert.ToInt16(processPlanDetails[i].LOCATION_CD);
                int noOfUnits = Convert.ToInt16(processPlanDetails[i].NO_OF_UNITS);
                int unit = Convert.ToInt16(processPlanDetails[i].UNIT);
                int actAllocQty = Convert.ToInt16(processPlanDetails[i].ACT_ALLOC_QTY);
                int processCD = Convert.ToInt16(processPlanDetails[i].PROCESS_CD);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append("UPDATE TRN_PROCESS_PLAN SET ");
                updateQuery.Append(" FIRST_PROCESS_CD = '" + processPlanDetails[i].PROCESS_CD + "' ");
                updateQuery.Append(",PROCESS_PLAN_START_DATE = TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "','yyyy/MM/dd')");
                updateQuery.Append(" WHERE ");
                updateQuery.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN_GYOUBAN + "' ");

                ExecuteNonQuery(updateQuery.ToString());
                result = InsertProcessPlanDetails(processPlanCD, locationCD, noOfUnits, unit, actAllocQty, processCD);
            }
            return result;
        }

        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/11/01       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Update data into table TRN_PROCESS_PLAN 
        /// Delete data from table TRN_WIP_STATUS
        /// </returns>
        /// 
        public List<GridSearchFields> DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)

        {
            int count = processPlanDetails.Count;
            List<GridSearchFields> result = new List<GridSearchFields>();
            using (TransactionScope transaction = new TransactionScope())
                {
                   // try{
                    for (int i = 0; i < count; i++)
                    {
                        string chuban_Gyouban = processPlanDetails[i].CHUUBAN_GYOUBAN;
                        int processPlanCd = Convert.ToInt32(processPlanDetails[i].PROCESS_PLAN_CD);
                        bool started = CheckProcessStartStatus(processPlanCd);

                        if (started == true)
                        {
                                GridSearchFields message = new GridSearchFields()
                                { CHUUBAN_GYOUBAN = chuban_Gyouban, RESPONCE = "Process Started" };
                                result.Add(message);
                        }
                        else
                        {
                         var responce= DeleteProcessPlanDetails(chuban_Gyouban, processPlanCd);
                            //if (result.Count == 0)
                            //{
                                GridSearchFields message = new GridSearchFields()
                                { CHUUBAN_GYOUBAN = chuban_Gyouban, RESPONCE = responce };
                                result.Add(message);
                            //}
                           
                        }
                        
                    }
                    
                //}
                //catch (Exception ex)
                //    {
                //        throw;
                //    }
                transaction.Complete();
            }
            
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/10/30       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Check ProcessCd count into base table
        /// </returns>
        /// 
        private bool CheckProcessStartStatus(int processPlanCd)
        {
            int count = 0;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT COUNT(*) FROM TRN_WIP_STATUS ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND PROCESS_START_DATE IS NOT NULL ");
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
                return true;
            else
                return false;
        }
        //private bool CheckProcessStartStatus(int firstProcessCd, int processPlanCd)
        //{
        //    int count = 0;
        //    int firstProcessCD = firstProcessCd;
        //    StringBuilder selectQuery = new StringBuilder();
        //    switch (firstProcessCD)
        //    {
        //        case UIConstants.K_PROCESS_ACID_WASH:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_HEAT_TREATMENT:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_1:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_SHOT_BLAST:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_2:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_DRAWING_B_TO_B:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_DRAWING_D ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_DRAWING_C_TO_B:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_DRAWING ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_DRAWING_C_TO_C:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_DRAWING_W ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_SINGLE_CABLE_PULL:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_STRAIGHTENING:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_STRAIGHTENING ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_MULTI_ROLL_STRIGHENING:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_PRESS_STRAIGHTENING:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_CHAMFER_S:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_CHAMFER_L:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_EC:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_EC ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_STAMPING:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_STAMPING ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_CUTTING:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_VISUAL_INSPECTION:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_COMPLETED_INSPECTION:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_UNDER_INSPECTION:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_WIP_TENSO:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_QA:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //        case UIConstants.K_PROCESS_NO_NEXT_PROCESS:
        //            selectQuery.Append("SELECT COUNT(*) FROM TRN_HEAT_TREATMENT ");
        //            selectQuery.Append(" WHERE ");
        //            selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' ");
        //            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        //            break;
        //    }
        //    if (count > 0)
        //        return true;
        //    else 
        //        return false;
        //}
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/10/31       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Save data into table TRN_WIP_STATUS
        /// </returns>
        /// 
        public string InsertProcessPlanDetails(int processPlanCD,int locationCD,int noOfUnits,int unit,int actAllocQty,int processCD)
        {
            StringBuilder insertSql = new StringBuilder();
                insertSql.Append("INSERT INTO TRN_WIP_STATUS(");
                insertSql.Append("WIP_STATUS_CD,");
                insertSql.Append("PROCESS_PLAN_CD,");
                insertSql.Append("LOCATION_CD,");
                insertSql.Append("TABA_NO,"); 
                insertSql.Append("WIP_NO_OF_UNITS,"); 
                insertSql.Append("UNIT,");
                insertSql.Append("WIP_QTY,");
                insertSql.Append("NEXT_PROCESS");
                insertSql.Append(")");
                insertSql.Append("VALUES( ");
                insertSql.Append("TRN_WIP_STATUS_SEQ.NEXTVAL,");
                insertSql.Append("'" + processPlanCD + "',");
                insertSql.Append("'" + locationCD + "',");
                insertSql.Append("'" + 1 + "',");
                insertSql.Append("'" + noOfUnits + "',");
                insertSql.Append("'" + unit + "',");
                insertSql.Append("'" + actAllocQty + "',");
                insertSql.Append("'" + processCD + "'");
                insertSql.Append(")");
                 ExecuteNonQuery(insertSql.ToString());
                 result = "Data Inserted Successfully";
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/11/02       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Update data into table TRN_PROCESS_PLAN 
        /// Save data into table TRN_WIP_STATUS
        /// </returns>
        /// 
        public string DeleteProcessPlanDetails(string chuban_Gyouban, int processPlanCd)
        {
        //string chuubanNo = chuban_Gyouban.Substring(0, 9);
        //string gyoubanNo = chuban_Gyouban.Substring(9, 2);
        StringBuilder updateQuery = new StringBuilder();
        updateQuery.Append("UPDATE TRN_PROCESS_PLAN SET ");
        updateQuery.Append(" FIRST_PROCESS_CD = '" + "" + "' ");
        updateQuery.Append(",PROCESS_PLAN_START_DATE = '" + "" + "'");
        updateQuery.Append(" WHERE ");
        updateQuery.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + chuban_Gyouban + "' ")
                ;
        ExecuteNonQuery(updateQuery.ToString());
            StringBuilder deleteSql = new StringBuilder();
            deleteSql.Append("DELETE  FROM TRN_WIP_STATUS ");
            deleteSql.Append("WHERE ");
            deleteSql.Append("PROCESS_PLAN_CD = '" + processPlanCd + "'");
            deleteSql.Append("  AND PROCESS_START_DATE IS NULL");
            ExecuteNonQuery(deleteSql.ToString());
            result = "Data Deleted Successfully";
            return result;
        }
      

    }
}
