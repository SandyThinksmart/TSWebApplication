using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class RollPoMainAnalysisRepository : BaseRepository, IRollPoMainAnalysisRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        protected List<PRODUCT_MASTER_HOST> prodMaster;
        private readonly Boolean conditionPOMain = false;
        public RollPoMainAnalysisRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            conditionPOMain =  Convert.ToBoolean(_configuration.GetSection("ConditionPOMain").Value); 

        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/01/17      
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Check Batch run successfully or not for that R and year from MST_CONFIGURATION
        /// </returns>
        /// 
        public int IsBatchRunSuccess(RollPoMainAnalysisDetails details)
        {

            int count;
            StringBuilder selectBatchRoll = new StringBuilder();
            selectBatchRoll.Append("SELECT VALUE FROM MST_CONFIGURATION WHERE KEY IN('FORECAST_BATCH_ROLL','FORECAST_BATCH_YEAR') ");
            selectBatchRoll.Append(" ORDER BY CASE KEY WHEN 'FORECAST_BATCH_ROLL' THEN 1 WHEN 'FORECAST_BATCH_YEAR' THEN 2 END ");
            List<int> batch_Data = ExecuteQueryMultiResult<int>(selectBatchRoll.ToString());
            int[] batchArray = batch_Data.ToArray();
            if (batchArray[0] == details.ROLL_R && batchArray[1] == details.ROLL_YEAR)
            {

                count = 1;
            }
            else
            {
                count = 0;
            }
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/01/17 
        /// used screens: RollPoMainAnalysis
        /// 1.Update ROLL_FC_LOCK_DATE in TRN_SALES_FORECAST table
        /// 2.Update INT_MATL_FC_SUMMARY_CD = null in TRN_SALES_FORECAST table
        /// 3.Delete all data from TEMP_INT_MATL_FC_SUMMARY
        /// 4.Drop TEMP_INT_MATL_FC_SUM_SEQ and then create TEMP_INT_MATL_FC_SUM_SEQ because it will always starts with 1 
        /// 5.Insert data into TEMP_INT_MATL_FC_SUMMARY table using group by of MST_MATERIAL table
        /// 6.Update INT_MATL_FC_SUMMARY_CD in TRN_SALES_FORECAST table 
        /// Purpose :  Batch is executed to get last R(month) PO qty,Remaining PO Qty ,Forecast Qty by Material Department,Forecast Qty by Sales Department,Nyuka Qty for each material.

        /// </summary>
        /// <returns>


        public string ExecuteMaterialForecastBatch(GridSearchFields details)
        {
            string result = "";
            bool allprodMatlFound = true;
           
            // using (TransactionScope transactionScope = new TransactionScope())
            //{

            // try { 

            StringBuilder updateRollFCLockDate = new StringBuilder();
            updateRollFCLockDate.Append("UPDATE TRN_SALES_FORECAST SET ");
            updateRollFCLockDate.Append("ROLL_FC_LOCK_DATE = TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','yyyy/MM/dd HH24:MI:SS') ");
            updateRollFCLockDate.Append("WHERE ");
            updateRollFCLockDate.Append("ROLL_FC_R = '" + details.ROLL_R + "' ");
            updateRollFCLockDate.Append("AND ROLL_FC_YEAR = '" + details.ROLL_YEAR + "' ");
            ExecuteNonQuery(updateRollFCLockDate.ToString());

            EmptyRequiredTables(true);
            StringBuilder updateSqlFCSummaryCD = new StringBuilder();
            updateSqlFCSummaryCD.Append("UPDATE TRN_SALES_FORECAST SET ");
            updateSqlFCSummaryCD.Append("INT_MATL_FC_SUMMARY_CD = ''");
            ExecuteNonQuery(updateSqlFCSummaryCD.ToString());

            StringBuilder deleteRollPODtlSql = new StringBuilder();
            deleteRollPODtlSql.Append("DELETE FROM TEMP_ROLL_PO_DETAILS ");
            ExecuteNonQuery(deleteRollPODtlSql.ToString());

            StringBuilder deleteFCSumSql = new StringBuilder();
            deleteFCSumSql.Append("DELETE FROM TEMP_INT_MATL_FC_SUMMARY ");
            ExecuteNonQuery(deleteFCSumSql.ToString());

            StringBuilder dropFCSumSEQSql = new StringBuilder();
            dropFCSumSEQSql.Append(" DROP SEQUENCE HSKWEBUSER.TEMP_INT_MATL_FC_SUM_SEQ ");
            ExecuteNonQuery(dropFCSumSEQSql.ToString());

            StringBuilder createFCSumSEQSql = new StringBuilder();
            createFCSumSEQSql.Append(" CREATE SEQUENCE HSKWEBUSER.TEMP_INT_MATL_FC_SUM_SEQ MINVALUE 0 MAXVALUE 999999999999 INCREMENT BY 1 START WITH 1 NOCACHE ");
            ExecuteNonQuery(createFCSumSEQSql.ToString());

            StringBuilder insertSql = new StringBuilder();
            insertSql.Append(" INSERT INTO TEMP_INT_MATL_FC_SUMMARY( ");
            insertSql.Append(" INT_MATL_FC_SUMMARY_CD,");
            insertSql.Append(" EMPLOYEE_CD,");
            insertSql.Append(" ROLL_YEAR,");
            insertSql.Append(" ROLL_R,");
            insertSql.Append(" SHAPE,");
            insertSql.Append(" MATL_KUBUN,");
            insertSql.Append(" DIM_D,");
            insertSql.Append(" INTERNAL_NAME_CD,");
            insertSql.Append(" LENGTH,");
            if(conditionPOMain ==  true)
            {
                insertSql.Append(" CONDITION_CD,");
            }
            
            insertSql.Append(" MATL_SPECIAL_KUBUN,");
            insertSql.Append(" USER_ID, ");
            insertSql.Append(" CREATED_DATE,");
            insertSql.Append(" UPDATED_DATE ");
            insertSql.Append(")");
            insertSql.Append(" SELECT ");
            insertSql.Append(" TEMP_INT_MATL_FC_SUM_SEQ.NEXTVAL AS INT_MATL_FC_SUMMARY_CD, ");
            insertSql.Append("'" + details.EMPLOYEE_CD + "',");
            insertSql.Append("'" + details.ROLL_YEAR + "',");
            insertSql.Append("'" + details.ROLL_R + "',");
            insertSql.Append(" SHAPE,");
            insertSql.Append(" MATL_KUBUN,");
            insertSql.Append(" DIM_D,");
            insertSql.Append(" INTERNAL_NAME_CD,");
            insertSql.Append(" MATERIAL_LENGTH,");
            if(conditionPOMain ==  true)
            {
                insertSql.Append(" CONDITION_CD,");
            }
            
            insertSql.Append(" MATL_SPECIAL_KUBUN, ");
            insertSql.Append("'" + details.USER_ID + "',");
            insertSql.Append("SYSDATE,");
            insertSql.Append("SYSDATE ");
            insertSql.Append(" FROM(");

            insertSql.Append(" SELECT ");
            insertSql.Append(" MM.SHAPE,");
            insertSql.Append(" MM.MATL_KUBUN,");
            insertSql.Append(" MM.DIM_D,");
            insertSql.Append(" MI.INTERNAL_NAME_CD,");
            insertSql.Append(" MM.MATERIAL_LENGTH,");
            if(conditionPOMain == true)
            {
                insertSql.Append(" MC.CONDITION_CD,");
            }
            
            insertSql.Append(" MM.MATL_SPECIAL_KUBUN ");
            insertSql.Append(" FROM MST_MATERIAL MM ");
            insertSql.Append(" INNER JOIN MST_MATL_NAME_MAKER MN ON MM.MATL_NAME_MAKER_CD = MN.MATL_NAME_MAKER_CD ");
            insertSql.Append(" INNER JOIN MST_INTERNAL_NAME MI ON MN.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");

            if (conditionPOMain == true)
            {
                insertSql.Append(" LEFT JOIN MST_CONDITION MC ON MC.CONDITION_CD = MM.CONDITION_CD ");
            }

            insertSql.Append(" WHERE ");
            insertSql.Append(" MM.IN_ACTIVE_FLAG = 0 ");
            insertSql.Append(" GROUP BY ");
            insertSql.Append(" MM.SHAPE, ");
            insertSql.Append(" MM.MATL_KUBUN, ");
            insertSql.Append(" MM.DIM_D, ");
            insertSql.Append(" MM.MATERIAL_LENGTH, ");
            insertSql.Append(" MM.MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                insertSql.Append(" MC.CONDITION_CD, ");
            }
            insertSql.Append(" MI.INTERNAL_NAME_CD ");
            
                
            insertSql.Append(")");

            ExecuteNonQuery(insertSql.ToString());

            //************************************** Code commented for testing batch ***********************************


            ////If material not found for product then matlNotFound report will generate
            //StringBuilder selectProductSql = new StringBuilder();

            //selectProductSql.Append(" SELECT MP.* ");
            //selectProductSql.Append(" FROM MST_PRODUCT MP INNER JOIN MST_INTERNAL_NAME MI ON MI.INTERNAL_NAME_CD=MP.INTERNAL_NAME_CD");
            //selectProductSql.Append(" WHERE MP.VER = ( ");
            //selectProductSql.Append(" SELECT MAX_VER.VER ");
            //selectProductSql.Append(" FROM( ");
            //selectProductSql.Append(" SELECT ");
            //selectProductSql.Append(" PROD_SPEC_NO1, ");
            //selectProductSql.Append(" PROD_SPEC_NO2, ");
            //selectProductSql.Append(" MAX(VER) AS VER ");
            //selectProductSql.Append(" FROM MST_PRODUCT ");
            //selectProductSql.Append(" GROUP BY ");
            //selectProductSql.Append(" PROD_SPEC_NO1, ");
            //selectProductSql.Append(" PROD_SPEC_NO2 ");
            //selectProductSql.Append(" ) MAX_VER ");
            //selectProductSql.Append(" WHERE MAX_VER.PROD_SPEC_NO1 = MP.PROD_SPEC_NO1 ");
            //selectProductSql.Append(" AND MAX_VER.PROD_SPEC_NO2 = MP.PROD_SPEC_NO2 ");
            //selectProductSql.Append(" ) ");
            //selectProductSql.Append(" AND MATL_KUBUN = 1 ");
            //selectProductSql.Append(" AND MP.PROD_SPEC_NO1 IS NOT NULL ");
            //selectProductSql.Append(" AND MP.PROD_SPEC_NO1 NOT IN('ｻｲｶｺ', 'ｾﾂﾀﾝ','ｺｸｲﾝ') ");
            //selectProductSql.Append(" AND MATL_NAME_MAKER NOT LIKE '%KKKKK%' ");
            //selectProductSql.Append(" AND INTERNAL_NAME NOT LIKE '%KKKKK%' ");

            //var prodQuery = selectProductSql.ToString();
            //selectProductSql.Append(prodQuery);
            //DataTable productData = ExecuteQueryDataTable(prodQuery);



            //foreach (DataRow prodRows in productData.Rows)
            //    {
            //        int productCD = Convert.ToInt32(prodRows["ROOT_VER_PRODUCT_KEY"]);

            //        RollPoMainAnalysisDetails materialDetails = GetMatlSpecsOfProductByProductCD(productCD);
            //        int? prodMatlType = 0;

               
            //        prodMatlType = materialDetails.MATL_TYPE_CBK;

            //        if (prodMatlType == 1)
            //        {
            //            string fcSummaryNo = GetFCSummaryMatchingWithMatlSpecs(materialDetails);
                    
            //        if (string.IsNullOrEmpty(fcSummaryNo))
            //            {
            //                InsertInTmpMatlNotFound(productCD);
            //                allprodMatlFound = false;
            //            }

            //        }


            //    }
            //if (allprodMatlFound == true)
            //{
                StringBuilder selectSalesFCSql = new StringBuilder();
            selectSalesFCSql.Append(" SELECT ");
            selectSalesFCSql.Append(" ROLL_FC_CD,");
            selectSalesFCSql.Append(" ROLL_FC_KUBUN,");
            selectSalesFCSql.Append(" PRODUCT_CD,");
            selectSalesFCSql.Append(" MATERIAL_CD ");
            selectSalesFCSql.Append(" FROM ");
            selectSalesFCSql.Append(" TRN_SALES_FORECAST ");
            selectSalesFCSql.Append(" WHERE ");
            selectSalesFCSql.Append(" ROLL_FC_YEAR ='" + details.ROLL_YEAR + "'");
            selectSalesFCSql.Append(" AND ROLL_FC_R ='" + details.ROLL_R + "'");

            var query = selectSalesFCSql.ToString();
            selectSalesFCSql.Append(query);
            DataTable forecastData = ExecuteQueryDataTable(query);





            foreach (DataRow forecastRows in forecastData.Rows)
            {
                int forecastKubun = Convert.ToInt16(forecastRows["ROLL_FC_KUBUN"]);
                int rollFCCD = Convert.ToInt32(forecastRows["ROLL_FC_CD"]);

                //************************************** Code commented for testing batch ***********************************
                if (forecastKubun == 1) // if forecast is product forecast (ROLL_FC_KUBUN = 1)
                {

                    int productCD = Convert.ToInt32(forecastRows["PRODUCT_CD"]);
                   
                    RollPoMainAnalysisDetails materialDetails = GetMatlSpecsOfProductByProductCD(productCD);
                    int? prodMatlType = 0;

                    if (materialDetails.MATL_TYPE_CBK != null)
                    {
                        prodMatlType = materialDetails.MATL_TYPE_CBK;
                    }

                    if (prodMatlType == 1)
                    {
                        string fcProdSummaryNo = GetFCSummaryMatchingWithMatlSpecs(materialDetails);

                        if (!string.IsNullOrEmpty(fcProdSummaryNo))
                        {
                            UpdateFCSummaryCD(fcProdSummaryNo, rollFCCD);

                        }
                        //else // this code is already check above so that commented here
                        //{
                        //    InsertInTmpMatlNotFound(productCD);
                        //    allprodMatlFound = false;
                        //}
                    }

                }
                if (forecastKubun == 2 || forecastKubun == 3) // if forecast is material forecast (ROLL_FC_KUBUN = 2 or 3)
                {
                    int materialCD = Convert.ToInt32(forecastRows["MATERIAL_CD"]);
                    RollPoMainAnalysisDetails materialDetails = GetMatlSpecsOfMaterialByMaterialCD(materialCD);
                    string fcMatlSummaryNo = GetFCSummaryMatchingWithMatlSpecs(materialDetails);
                    UpdateFCSummaryCD(fcMatlSummaryNo, rollFCCD);

                }
            }

            UpdateTempIntMatlFCSummaryWithForecastQty(details.ROLL_YEAR, details.ROLL_R);//data match
            UpdateTempIntMatlFCSummaryWithPOQty(details.ROLL_YEAR, details.ROLL_R);//data match
            UpdateTempIntMatlFCSummaryWithRecvdQty(details.ROLL_YEAR, details.ROLL_R);//data match 
            UpdateTempIntMatlFCSummaryWithRemQty4MonthAgo(details.ROLL_YEAR, details.ROLL_R);//data match but time taking query 
            UpdateTempIntMatlFCSummaryWithRemQty(details.ROLL_YEAR, details.ROLL_R);
            UpdateTempIntMatlFCSummaryWithIntStockQty();//data match 
            UpdateTempIntMatlFCSummaryWithExtStockQty();//data match

            StringBuilder deleteRollSumSql = new StringBuilder();
            deleteRollSumSql.Append("DELETE FROM TEMP_ROLL_MATL_SUMMARY ");
            ExecuteNonQuery(deleteRollSumSql.ToString());

            StringBuilder dropRollSumSEQSql = new StringBuilder();
            dropRollSumSEQSql.Append(" DROP SEQUENCE HSKWEBUSER.ROLL_MATL_SUMMARY_CD_SEQ ");
            ExecuteNonQuery(dropRollSumSEQSql.ToString());

            StringBuilder createRollSumSEQSql = new StringBuilder();
            createRollSumSEQSql.Append(" CREATE SEQUENCE  HSKWEBUSER.ROLL_MATL_SUMMARY_CD_SEQ  MINVALUE 0 MAXVALUE 99999999999999 INCREMENT BY 1 START WITH 1 NOCACHE ");
            ExecuteNonQuery(createRollSumSEQSql.ToString());

            StringBuilder selectFCSumSql = new StringBuilder();
            selectFCSumSql.Append(" SELECT ");
            selectFCSumSql.Append(" INT_MATL_FC_SUMMARY_CD,");
            selectFCSumSql.Append(" INTERNAL_NAME_CD,");
            selectFCSumSql.Append(" SHAPE,");
            selectFCSumSql.Append(" MATL_KUBUN, ");
            selectFCSumSql.Append(" DIM_D, ");
            selectFCSumSql.Append(" LENGTH, ");
            if(conditionPOMain == true)
            {
                selectFCSumSql.Append(" CONDITION_CD, ");
            }
            
            selectFCSumSql.Append(" MATL_SPECIAL_KUBUN ");
            selectFCSumSql.Append(" FROM TEMP_INT_MATL_FC_SUMMARY ");

            var queryFCSum = selectFCSumSql.ToString();
            selectFCSumSql.Append(queryFCSum);

            DataTable fCSumData = ExecuteQueryDataTable(queryFCSum);


            foreach (DataRow tmpFcSummaryRow in fCSumData.Rows)
            {

                long fcSummaryCd = Convert.ToInt64(tmpFcSummaryRow["INT_MATL_FC_SUMMARY_CD"]);

                DataTable matlData = GetMatlsMatchingWithFcSummary(tmpFcSummaryRow);


                foreach (DataRow matlDataRow in matlData.Rows)
                {

                    int matlKubun = Convert.ToInt32(matlDataRow["MATL_KUBUN"]);
                    int matlNameMakerCd = Convert.ToInt32(matlDataRow["MATL_NAME_MAKER_CD"]);

                    int materialCd = Convert.ToInt32(matlDataRow["MATERIAL_CD"]);
                    
                    decimal dimD =Convert.ToDecimal( matlDataRow["DIM_D"]);
                    int userId = Convert.ToInt32(details.USER_ID);
                    string matlKubunWhereCondition = string.Empty;
                    if (matlKubun == 1) // 1: Material type = COIL
                    {
                        matlKubunWhereCondition = " = " + matlKubun;
                    }
                    else
                    {
                        matlKubunWhereCondition = " <> 1"; //matlkubun = 2 is check not check matlKubun = 3
                    }
                    List<RollPoMainAnalysisDetails> mukesakiRateData = GetMukesakiAndRateData(matlNameMakerCd,dimD, matlKubunWhereCondition, matlKubun);


                    //if (mukesakiRateData.Count > 0)
                    //{
                    
                    foreach (var item in mukesakiRateData)
                    { 
                        if (item.MUKESAKI_CD.HasValue)
                        {                           
                            InsertTempRollMatlSummaryData(item.MUKESAKI_CD, item.RATE, fcSummaryCd, materialCd, userId);
                        }
                    }
                    // }
                    // else 
                    //  {
                    StringBuilder selectIppanCd = new StringBuilder();

                    selectIppanCd.Append(" SELECT VALUE FROM MST_CONFIGURATION WHERE KEY = 'MUKESAKI_IPPAN_CODE' ");
                    int mukesakiIppanCd = Convert.ToInt32(ExecuteScalar(selectIppanCd.ToString()));
                    InsertTempRollMatlSummaryDataofIppanRecord(mukesakiIppanCd, fcSummaryCd, materialCd, userId);

                    // }

                }


            }


            UpdateRollMatlSummaryWithPORemQtyPrev(details.ROLL_YEAR, details.ROLL_R);
            UpdateRollMatlSummaryWithPORemQty(details.ROLL_YEAR, details.ROLL_R);
            UpdateRollMatlSummaryWithPOPrvQtyAndPOQty(details.ROLL_YEAR, details.ROLL_R);//data match
            UpdateRollMatlSummaryWithIntStockQty();
            UpdateRollMatlSummaryWithExtStockQty();
            UpdateRollMatlSummaryWithNyukaQty(details.ROLL_YEAR, details.ROLL_R);//commented for batch testing

            StringBuilder updateYrQuery = new StringBuilder();
                updateYrQuery.Append(" UPDATE MST_CONFIGURATION ");
                updateYrQuery.Append(" SET ");
                updateYrQuery.Append(" VALUE = '" + details.ROLL_YEAR + "' ");
                updateYrQuery.Append(" WHERE KEY = 'FORECAST_BATCH_YEAR'  ");
                ExecuteNonQuery(updateYrQuery.ToString());

                StringBuilder updateRQuery = new StringBuilder();
                updateRQuery.Append(" UPDATE MST_CONFIGURATION ");
                updateRQuery.Append(" SET ");
                updateRQuery.Append(" VALUE = '" + details.ROLL_R + "' ");
                updateRQuery.Append(" WHERE KEY = 'FORECAST_BATCH_ROLL'  ");

                ExecuteNonQuery(updateRQuery.ToString());
                //transactionScope.Complete();
                //transactionScope.Dispose();
                result = "Batch Executed Successfully";

            //}
            //else
            //{

            //    EmptyRequiredTables(false);
            //    result = "Matl Not Found for one or more product";
            //}

            // transactionScope.Complete();

            //}


            //}


            //catch (Exception ex)
            //{
            //    result = "Error: " + ex.Message;
            //    EmptyRequiredTables(false);
            //}


            return result;
            // }


        }


        public void EmptyRequiredTables(bool emptyTmpMatlNotfound)
        {
            if (emptyTmpMatlNotfound)
            {
                StringBuilder deleteTmpMatlNotFound = new StringBuilder();
                deleteTmpMatlNotFound.Append("DELETE FROM TMP_MATL_NOT_FOUND ");
                ExecuteNonQuery(deleteTmpMatlNotFound.ToString());
            }
            StringBuilder updateSqlFCSummaryCD = new StringBuilder();
            updateSqlFCSummaryCD.Append("UPDATE TRN_SALES_FORECAST SET ");
            updateSqlFCSummaryCD.Append("INT_MATL_FC_SUMMARY_CD = ''");
            ExecuteNonQuery(updateSqlFCSummaryCD.ToString());

            StringBuilder deleteRollPODtlSql = new StringBuilder();
            deleteRollPODtlSql.Append("DELETE FROM TEMP_ROLL_PO_DETAILS ");
            ExecuteNonQuery(deleteRollPODtlSql.ToString());

            StringBuilder deleteFCSumSql = new StringBuilder();
            deleteFCSumSql.Append("DELETE FROM TEMP_INT_MATL_FC_SUMMARY ");
            ExecuteNonQuery(deleteFCSumSql.ToString());

            StringBuilder deleteRollSumSql = new StringBuilder();
            deleteRollSumSql.Append("DELETE FROM TEMP_ROLL_MATL_SUMMARY ");
            ExecuteNonQuery(deleteRollSumSql.ToString());
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/01/23       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Get Material details of product by ProductCD from MST_PRODUCT table
        /// </returns>
        /// 
        public RollPoMainAnalysisDetails GetMatlSpecsOfProductByProductCD(int productCD)
        {
            RollPoMainAnalysisDetails result = new RollPoMainAnalysisDetails();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" INTERNAL_NAME_CD, ");
            selectQuery.Append(" MATL_KUBUN, ");
            selectQuery.Append(" MATL_SHAPE AS SHAPE, ");
            selectQuery.Append(" MATL_DIM_D AS DIM_D,");
            selectQuery.Append(" MATL_TYPE_CBK, ");
            if(conditionPOMain == true)
            {
                selectQuery.Append(" MATL_CONDITION_CD AS CONDITION_CD, ");
            }
            
            selectQuery.Append(" MATL_LENGTH AS LENGTH, ");
            selectQuery.Append(" MATL_SPECIAL_KUBUN ");
            selectQuery.Append(" FROM MST_PRODUCT  ");
            selectQuery.Append(" WHERE ROOT_VER_PRODUCT_KEY = '" + productCD + "' ");
            selectQuery.Append(" ORDER BY VER DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY");
            result = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/01/24       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Get Material details of material by MaterialCD from MST_MATERIAL table
        /// </returns>
        /// 
        public RollPoMainAnalysisDetails GetMatlSpecsOfMaterialByMaterialCD(int materialCD)
        {
            RollPoMainAnalysisDetails result = new RollPoMainAnalysisDetails();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT MN.INTERNAL_NAME_CD, ");
            selectQuery.Append(" MATL_KUBUN, ");
            selectQuery.Append(" SHAPE AS SHAPE, ");
            selectQuery.Append(" DIM_D AS DIM_D, ");
            selectQuery.Append(" MATERIAL_LENGTH AS LENGTH, ");
            if(conditionPOMain == true)
            {
                selectQuery.Append(" CONDITION_CD, ");
            }
            
            selectQuery.Append(" MATL_SPECIAL_KUBUN ");
            selectQuery.Append(" FROM MST_MATERIAL MM INNER JOIN MST_MATL_NAME_MAKER MN ON MM.MATL_NAME_MAKER_CD = MN.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" WHERE MATERIAL_CD = '" + materialCD + "'");

            result = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/01/27       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Get INT_MATL_FC_SUMMARY_CD match with material details from TEMP_INT_MATL_FC_SUMMARY table
        /// </returns>
        /// 
        public string GetFCSummaryMatchingWithMatlSpecs(RollPoMainAnalysisDetails materialDetails)
        {
            string fcSummaryNo;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT INT_MATL_FC_SUMMARY_CD ");
            selectQuery.Append(" FROM TEMP_INT_MATL_FC_SUMMARY ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" INTERNAL_NAME_CD = '" + materialDetails.INTERNAL_NAME_CD + "'");
            if (materialDetails.MATL_KUBUN != null)
            {
                if (materialDetails.MATL_KUBUN > 0)
                {
                    selectQuery.Append(" AND MATL_KUBUN = '" + materialDetails.MATL_KUBUN + "'");

                }
            }
            selectQuery.Append(" AND DIM_D = '" + materialDetails.DIM_D + "'");
            selectQuery.Append(" AND SHAPE = '" + materialDetails.SHAPE + "'");

            if (materialDetails.MATL_SPECIAL_KUBUN != null)
            {
                if (materialDetails.MATL_SPECIAL_KUBUN > 0)
                {
                    selectQuery.Append(" AND MATL_SPECIAL_KUBUN = '" + materialDetails.MATL_SPECIAL_KUBUN + "'");

                }
            }

            if(conditionPOMain == true)
            {
                if (materialDetails.CONDITION_CD != null)
                {
                    if (materialDetails.CONDITION_CD > 0)
                    {

                        selectQuery.Append(" AND CONDITION_CD = '" + materialDetails.CONDITION_CD + "'");
                    }
                }
                else
                {
                    selectQuery.Append(" AND CONDITION_CD IS NULL");
                }
            }
           
            if (materialDetails.LENGTH != null)
            {
                if (materialDetails.LENGTH > 0)
                {
                    selectQuery.Append(" AND LENGTH =  '" + materialDetails.LENGTH + "'");

                }
            }
            else
            {
                selectQuery.Append(" AND LENGTH IS NULL");
            }

            fcSummaryNo = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            return fcSummaryNo;
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/01/28       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update INT_MATL_FC_SUMMARY_CD in TRN_SALES_FORECAST table
        /// </returns>
        /// 
        public void UpdateFCSummaryCD(string fcSummaryNo, int rollFCCD)
        {
            StringBuilder updateSqlFCSumCD = new StringBuilder();
            updateSqlFCSumCD.Append(" UPDATE TRN_SALES_FORECAST SET ");
            updateSqlFCSumCD.Append(" INT_MATL_FC_SUMMARY_CD = '" + fcSummaryNo + "'");
            updateSqlFCSumCD.Append(" WHERE ");
            updateSqlFCSumCD.Append(" ROLL_FC_CD = '" + rollFCCD + "'");
            ExecuteNonQuery(updateSqlFCSumCD.ToString());
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/05/06       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Get material details from V_HTMATL_DATA_WITH_MATLINT 
        /// </returns>
        /// 
        public DataTable GetMatlsMatchingWithFcSummary(DataRow materialDetails)
        {

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" HT_MATL_SPEC_CD AS MATERIAL_CD, ");
            selectQuery.Append(" MAKER_CD, ");
            selectQuery.Append(" MAKERSHORTNAME, ");
            selectQuery.Append(" MATL_CD_MAK AS MATL_NAME_MAKER_CD, ");
            selectQuery.Append(" MATL_KUBUN, ");
            selectQuery.Append(" DIM_D ");
            selectQuery.Append(" FROM V_HTMATL_DATA_WITH_MATLINT ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MATL_CD_INT = '" + materialDetails["INTERNAL_NAME_CD"] + "'");
            selectQuery.Append(" AND DIM_D = '" + materialDetails["DIM_D"] + "'");
            if (materialDetails["SHAPE"] != DBNull.Value)
            {
                selectQuery.Append(" AND SHAPE = '" + materialDetails["SHAPE"] + "'");
            }
            if (materialDetails["MATL_KUBUN"] != DBNull.Value)
            {
                if (Convert.ToDecimal(materialDetails["MATL_KUBUN"]) > 0)
                {

                    selectQuery.Append(" AND MATL_KUBUN = '" + materialDetails["MATL_KUBUN"] + "'");
                }
            }

            if (materialDetails["LENGTH"] != DBNull.Value)
            {
                //if (Convert.ToDecimal(materialDetails["LENGTH"]) > 0)
                //{
                selectQuery.Append(" AND LENGTH =  '" + materialDetails["LENGTH"] + "'");

                //}
            }
            else
            {
                selectQuery.Append(" AND LENGTH IS NULL");
            }
            if (materialDetails["MATL_SPECIAL_KUBUN"] != DBNull.Value)
            {
                if (Convert.ToDecimal(materialDetails["MATL_SPECIAL_KUBUN"]) > 0)
                {
                    selectQuery.Append(" AND GEN_SPC = '" + materialDetails["MATL_SPECIAL_KUBUN"] + "'");

                }
            }

            DataTable Result = ExecuteQueryDataTable(selectQuery.ToString());
            return Result;
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/11/14       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1. Get appropriate data of mukesaki and rate from this query to insert record in TEMP_ROLL_MATL_SUMMARY.
        /// </returns>
        /// 
        public List<RollPoMainAnalysisDetails> GetMukesakiAndRateData(int matlNameMakerCd, decimal dimD, string matlKubunWhereCondition,int matlKubun)
        {
            List<RollPoMainAnalysisDetails> result = new List<RollPoMainAnalysisDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT DISTINCT MR.MUKESAKI_CD,");
            selectQuery.Append(" RATE ");
            selectQuery.Append(" FROM MST_MUKESAKI MM ");
            selectQuery.Append(" INNER JOIN MST_MATERIAL_RATE MR ON MM.MUKESAKI_CD = MR.MUKESAKI_CD ");
            selectQuery.Append(" WHERE MM.IN_ACTIVE_FLAG = 0 ");
            selectQuery.Append(" AND MATL_NAME_MAKER_CD ='" + matlNameMakerCd + "'");
            selectQuery.Append(" AND MATL_KUBUN " + matlKubunWhereCondition);
            if(matlKubun != 1)
            {
                selectQuery.Append(" AND  " + dimD + " BETWEEN DIM_START AND DIM_END ");
            }
            selectQuery.Append(" AND TO_DATE(TO_CHAR(APPLY_ROLL_YEAR) || '/' || LPAD(APPLY_ROLL_MONTH,2,'0') || '/01','YYYY/MM/DD') = ");
            selectQuery.Append(" ( ");
            selectQuery.Append(" SELECT MAX(TO_DATE(TO_CHAR(APPLY_ROLL_YEAR) || '/' || LPAD(APPLY_ROLL_MONTH,2,'0') || '/01','YYYY/MM/DD')) ");
            selectQuery.Append(" FROM MST_MATERIAL_RATE ");
            selectQuery.Append(" WHERE MATL_NAME_MAKER_CD ='" + matlNameMakerCd + "'");
            selectQuery.Append(" AND MATL_KUBUN " + matlKubunWhereCondition);
            if (matlKubun != 1)
            {
                selectQuery.Append(" AND  " + dimD + " BETWEEN DIM_START AND DIM_END ");
            }
            selectQuery.Append(" AND TO_DATE(TO_CHAR(APPLY_ROLL_YEAR) || '/' || LPAD(APPLY_ROLL_MONTH,2,'0') || '/01','YYYY/MM/DD') <= SYSDATE ");
            selectQuery.Append(" AND MUKESAKI_CD = MR.MUKESAKI_CD  ");
            selectQuery.Append(" ) ");

            result = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(selectQuery.ToString());
            return result;
        }



        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/05/07       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Insert data into TEMP_ROLL_MATL_SUMMARY table
        /// </returns>
        /// 
        public void InsertTempRollMatlSummaryData(int? mukesakiCd, float? rate, long fcSummaryCd, int materialCd, int userId)
        {
            StringBuilder insertSql = new StringBuilder();
            insertSql.Append(" INSERT INTO TEMP_ROLL_MATL_SUMMARY( ");
            insertSql.Append(" ROLL_MATL_SUMMARY_CD,");
            insertSql.Append(" INT_MATL_FC_SUMMARY_CD,");
            insertSql.Append(" MATERIAL_CD,");
            insertSql.Append(" MUKESAKI_CD,");
            insertSql.Append(" MUKESAKI_RATE,");
            insertSql.Append(" USER_ID, ");
            insertSql.Append(" CREATED_DATE,");
            insertSql.Append(" UPDATED_DATE ");
            insertSql.Append(" )");
            insertSql.Append(" VALUES (");
            insertSql.Append(" ROLL_MATL_SUMMARY_CD_SEQ.NEXTVAL, ");
            insertSql.Append("'" + fcSummaryCd + "',");
            insertSql.Append("'" + materialCd + "',");
            insertSql.Append("'" + mukesakiCd + "',");
            insertSql.Append("'" + rate + "',");
            insertSql.Append("'" + userId + "',");
            insertSql.Append(" SYSDATE,");
            insertSql.Append(" SYSDATE) ");

            ExecuteNonQuery(insertSql.ToString());
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/05/07       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Now insert Ippan Mukesaki record (Mukesaki_Cd = 9999) in TEMP_ROLL_MATL_SUMMARY 
        /// </returns>
        /// 
        public void InsertTempRollMatlSummaryDataofIppanRecord(int ippanMukesakiCd, long fcSummaryCd, int materialCd, int userId)
        {
            StringBuilder insertSql = new StringBuilder();


            insertSql.Append(" INSERT INTO TEMP_ROLL_MATL_SUMMARY( ");
            insertSql.Append(" ROLL_MATL_SUMMARY_CD,");
            insertSql.Append(" INT_MATL_FC_SUMMARY_CD,");
            insertSql.Append(" MATERIAL_CD,");
            insertSql.Append(" MUKESAKI_CD,");
            insertSql.Append(" USER_ID, ");
            insertSql.Append(" CREATED_DATE,");
            insertSql.Append(" UPDATED_DATE )");
            insertSql.Append(" SELECT ");
            insertSql.Append(" ROLL_MATL_SUMMARY_CD_SEQ.NEXTVAL,");
            insertSql.Append("'" + fcSummaryCd + "',");
            insertSql.Append("'" + materialCd + "',");
            insertSql.Append("'" + ippanMukesakiCd + "',");
            insertSql.Append("'" + userId + "',");
            insertSql.Append(" SYSDATE,");
            insertSql.Append(" SYSDATE ");
            insertSql.Append(" FROM ");
            insertSql.Append(" DUAL ");
            insertSql.Append(" WHERE ");
            insertSql.Append(" NOT EXISTS( ");
            insertSql.Append(" SELECT ");
            insertSql.Append(" INT_MATL_FC_SUMMARY_CD ");
            insertSql.Append(" FROM ");
            insertSql.Append(" TEMP_ROLL_MATL_SUMMARY ");
            insertSql.Append(" WHERE ");
            insertSql.Append(" INT_MATL_FC_SUMMARY_CD = '" + fcSummaryCd + "' ");
            insertSql.Append(" AND MATERIAL_CD = '" + materialCd + "' ");
            insertSql.Append(" AND MUKESAKI_CD = '" + ippanMukesakiCd + "' ");
            insertSql.Append(" ) ");

            ExecuteNonQuery(insertSql.ToString());
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/02/10       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update SALES_FC_QTY_THIS_MONTH,SALES_FC_QTY_1MONTH_AHEAD,SALES_FC_QTY_2MONTH_AHEAD,SALES_FC_QTY_3MONTH_AHEAD in TEMP_INT_MATL_FC_SUMMARY table 
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithForecastQty(int roll_Year, int roll_R)
        {
            Single? R1 = null;
            Single? R2 = null;
            Single? R3 = null;
            Single? R4 = null;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" INT_MATL_FC_SUMMARY_CD,");
            selectQuery.Append(" SUM(FC_QTY_R1) AS R1,");
            selectQuery.Append(" SUM(FC_QTY_R2) AS R2,");
            selectQuery.Append(" SUM(FC_QTY_R3) AS R3,");
            selectQuery.Append(" SUM(FC_QTY_R4) AS R4");
            selectQuery.Append(" FROM");
            selectQuery.Append(" TRN_SALES_FORECAST");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" ROLL_FC_YEAR = '" + roll_Year + "'");
            selectQuery.Append(" AND ROLL_FC_R = '" + roll_R + "'");
            selectQuery.Append(" AND INT_MATL_FC_SUMMARY_CD IS NOT NULL");
            selectQuery.Append(" GROUP BY");
            selectQuery.Append(" INT_MATL_FC_SUMMARY_CD");

            var query = selectQuery.ToString();
            selectQuery.Append(query);
            DataTable fcQtyData = ExecuteQueryDataTable(query);

            foreach (DataRow fcQtyRows in fcQtyData.Rows)
            {
                if (fcQtyRows["R1"] == DBNull.Value)
                {
                    R1 = null;
                }
                else
                {
                    R1 = Convert.ToSingle(fcQtyRows["R1"]);
                }
                if (fcQtyRows["R2"] == DBNull.Value)
                {
                    R2 = null;
                }
                else
                {
                    R2 = Convert.ToSingle(fcQtyRows["R2"]);
                }
                if (fcQtyRows["R3"] == DBNull.Value)
                {
                    R3 = null;
                }
                else
                {
                    R3 = Convert.ToSingle(fcQtyRows["R3"]);
                }
                if (fcQtyRows["R4"] == DBNull.Value)
                {
                    R4 = null;
                }
                else
                {
                    R4 = Convert.ToSingle(fcQtyRows["R4"]);
                }



                long fcSummaryCd = Convert.ToInt64(fcQtyRows["INT_MATL_FC_SUMMARY_CD"]);

                StringBuilder updateSqlFCQty = new StringBuilder();
                updateSqlFCQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY SET ");
                updateSqlFCQty.Append(" SALES_FC_QTY_THIS_MONTH = '" + R1 + "',");
                updateSqlFCQty.Append(" SALES_FC_QTY_1MONTH_AHEAD = '" + R2 + "',");
                updateSqlFCQty.Append(" SALES_FC_QTY_2MONTH_AHEAD = '" + R3 + "',");
                updateSqlFCQty.Append(" SALES_FC_QTY_3MONTH_AHEAD = '" + R4 + "'");

                updateSqlFCQty.Append(" WHERE ");
                updateSqlFCQty.Append(" INT_MATL_FC_SUMMARY_CD = '" + fcSummaryCd + "'");
                ExecuteNonQuery(updateSqlFCQty.ToString());

            }

        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/02/10       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update PO_QTY_3MONTH_AGO,PO_QTY_2MONTH_AGO,PO_QTY_1MONTH_AGO in TEMP_INT_MATL_FC_SUMMARY table
        /// 2.Update PO_QTY_THIS_MONTH,PO_QTY_1MONTH_AHEAD,PO_QTY_2MONTH_AHEAD,PO_QTY_3MONTH_AHEAD,PO_QTY_4MONTH_AHEAD in TEMP_INT_MATL_FC_SUMMARY table
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithPOQty(int roll_Year, int roll_R)
        {
            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            StringBuilder updateSqlPOQty = new StringBuilder();
            updateSqlPOQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T ");
            updateSqlPOQty.Append(" SET(T.PO_QTY_3MONTH_AGO, T.PO_QTY_2MONTH_AGO, T.PO_QTY_1MONTH_AGO, T.PO_QTY_THIS_MONTH, ");
            updateSqlPOQty.Append(" T.PO_QTY_1MONTH_AHEAD, T.PO_QTY_2MONTH_AHEAD, T.PO_QTY_3MONTH_AHEAD, ");
            updateSqlPOQty.Append(" T.PO_QTY_4MONTH_AHEAD) = ");
            updateSqlPOQty.Append(" (SELECT ");
            updateSqlPOQty.Append(" U.PO_QTY_3MONTH_AGO, ");
            updateSqlPOQty.Append(" U.PO_QTY_2MONTH_AGO, ");
            updateSqlPOQty.Append(" U.PO_QTY_1MONTH_AGO, ");
            updateSqlPOQty.Append(" U.PO_QTY_THIS_MONTH, U.PO_QTY_1MONTH_AHEAD, U.PO_QTY_2MONTH_AHEAD, ");
            updateSqlPOQty.Append(" U.PO_QTY_3MONTH_AHEAD, U.PO_QTY_4MONTH_AHEAD ");
            updateSqlPOQty.Append(" FROM ");
            updateSqlPOQty.Append(" (SELECT ");
            updateSqlPOQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            updateSqlPOQty.Append(" SUM(CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_3MONTH_AGO, ");
            updateSqlPOQty.Append(" SUM(CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_2MONTH_AGO, ");
            updateSqlPOQty.Append(" SUM(CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_1MONTH_AGO, ");
            updateSqlPOQty.Append(" SUM ");
            updateSqlPOQty.Append(" (CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_THIS_MONTH, ");
            updateSqlPOQty.Append(" SUM ");
            updateSqlPOQty.Append(" (CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeTwoMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_1MONTH_AHEAD, ");
            updateSqlPOQty.Append(" SUM  ");
            updateSqlPOQty.Append(" (CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeOneMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append("  END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_2MONTH_AHEAD, ");
            updateSqlPOQty.Append(" SUM ");
            updateSqlPOQty.Append(" (CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY = '" + rollList.Roll_YearCurrentMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_3MONTH_AHEAD, ");
            updateSqlPOQty.Append(" SUM ");
            updateSqlPOQty.Append(" (CASE ");
            updateSqlPOQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY = '" + rollList.Roll_YearAfterOneMonth + "') ");
            updateSqlPOQty.Append(" THEN QTY ");
            updateSqlPOQty.Append(" END ");
            updateSqlPOQty.Append(" ) AS PO_QTY_4MONTH_AHEAD ");
            updateSqlPOQty.Append(" FROM ");
            updateSqlPOQty.Append(" (SELECT   ");
            updateSqlPOQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlPOQty.Append(" ROLL_R, ");
            updateSqlPOQty.Append(" YYYY, ");
            updateSqlPOQty.Append(" SUM(QTY) AS QTY ");
            updateSqlPOQty.Append(" FROM ");
            updateSqlPOQty.Append(" (SELECT ");
            updateSqlPOQty.Append(" ROLL_R, ");
            updateSqlPOQty.Append(" YYYY, ");
            updateSqlPOQty.Append(" QTY, ");
            updateSqlPOQty.Append(" MM.DIM_D, ");
            updateSqlPOQty.Append(" MM.SHAPE AS SHAPE, ");
            updateSqlPOQty.Append(" MM.MATERIAL_LENGTH AS LENGTH, ");
            updateSqlPOQty.Append(" MM.MATL_KUBUN AS MATL_KUBUN, ");
            updateSqlPOQty.Append(" MM.MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlPOQty.Append(" MC.CONDITION_CD, ");
            }
            updateSqlPOQty.Append(" MMNM.INTERNAL_NAME_CD AS INTERNAL_NAME_CD ");
            
           
            updateSqlPOQty.Append(" FROM ");
            updateSqlPOQty.Append(" TRN_PO_DTL TPD ");
            updateSqlPOQty.Append(" INNER JOIN MST_MATERIAL MM ON TPD.MATERIAL_CD = MM.MATERIAL_CD ");
            updateSqlPOQty.Append(" INNER JOIN MST_MATL_NAME_MAKER MMNM ON MM.MATL_NAME_MAKER_CD = MMNM.MATL_NAME_MAKER_CD ");
            if(conditionPOMain == true)
            {
                updateSqlPOQty.Append(" LEFT JOIN MST_CONDITION MC ON MC.CONDITION_CD = MM.CONDITION_CD ");
            }
            
            updateSqlPOQty.Append(" AND TPD.INACTIVE_FLAG = 0 "); //chnage to PO_STATUS 
            updateSqlPOQty.Append(" AND((ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' ");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeTwoMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeOneMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY = '" + rollList.Roll_YearCurrentMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" OR(ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY = '" + rollList.Roll_YearAfterOneMonth + "'");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" ) ");
            updateSqlPOQty.Append(" ) V, ");
            updateSqlPOQty.Append(" (SELECT ");
            updateSqlPOQty.Append(" DIM_D, ");
            updateSqlPOQty.Append(" SHAPE, ");
            updateSqlPOQty.Append(" LENGTH, ");
            updateSqlPOQty.Append(" MATL_KUBUN, ");
            updateSqlPOQty.Append(" MATL_SPECIAL_KUBUN, ");
            updateSqlPOQty.Append(" INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlPOQty.Append(" CONDITION_CD, ");
            }
            updateSqlPOQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            
            
            updateSqlPOQty.Append(" FROM ");
            updateSqlPOQty.Append(" TEMP_INT_MATL_FC_SUMMARY) Z ");
            updateSqlPOQty.Append(" WHERE V.DIM_D = Z.DIM_D ");
            updateSqlPOQty.Append(" AND V.SHAPE = Z.SHAPE ");
            updateSqlPOQty.Append(" AND(Z.LENGTH IS NULL OR V.LENGTH = Z.LENGTH ");
            updateSqlPOQty.Append("  ) ");
            updateSqlPOQty.Append(" AND V.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlPOQty.Append(" AND V.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlPOQty.Append(" AND V.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if(conditionPOMain == true)
            {
                updateSqlPOQty.Append(" AND V.CONDITION_CD = Z.CONDITION_CD ");
            }
            
            updateSqlPOQty.Append(" GROUP BY ");
            updateSqlPOQty.Append(" V.SHAPE, ");
            updateSqlPOQty.Append(" V.MATL_KUBUN, ");
            updateSqlPOQty.Append(" V.DIM_D, ");
            updateSqlPOQty.Append(" V.LENGTH, ");
            updateSqlPOQty.Append(" V.MATL_SPECIAL_KUBUN, ");
            updateSqlPOQty.Append(" V.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlPOQty.Append(" V.CONDITION_CD, ");
            }
            
            updateSqlPOQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlPOQty.Append(" YYYY, ");
            updateSqlPOQty.Append(" ROLL_R )");
            updateSqlPOQty.Append(" GROUP BY INT_MATL_FC_SUMMARY_CD) U ");
            updateSqlPOQty.Append(" WHERE T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD) ");
            ExecuteNonQuery(updateSqlPOQty.ToString());

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/02/10       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update RECVD_QTY_3MONTH_AGO,RECVD_QTY_2MONTH_AGO,RECVD_QTY_1MONTH_AGO in TEMP_INT_MATL_FC_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithRecvdQty(int roll_Year, int roll_R)
        {
            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            StringBuilder updateSqlRecvdQty = new StringBuilder();
            updateSqlRecvdQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T SET ");
            updateSqlRecvdQty.Append(" ( RECVD_QTY_3MONTH_AGO,");
            updateSqlRecvdQty.Append(" RECVD_QTY_2MONTH_AGO,");
            updateSqlRecvdQty.Append(" RECVD_QTY_1MONTH_AGO ) = ");
            updateSqlRecvdQty.Append(" ( SELECT ");
            updateSqlRecvdQty.Append(" U.RECVD_QTY_3MONTH_AGO,");
            updateSqlRecvdQty.Append(" U.RECVD_QTY_2MONTH_AGO, ");
            updateSqlRecvdQty.Append(" U.RECVD_QTY_1MONTH_AGO ");
            updateSqlRecvdQty.Append(" FROM ( ");
            updateSqlRecvdQty.Append(" SELECT ");
            updateSqlRecvdQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            updateSqlRecvdQty.Append(" SUM(CASE WHEN (NYUKA_MONTH = '" + rollList.Roll_MBeforeSixMonth + "' AND NYUKA_YEAR = '" + rollList.Roll_MYearBeforeSixMonth + "') THEN NYUKA_QTY END) AS RECVD_QTY_3MONTH_AGO, ");
            updateSqlRecvdQty.Append(" SUM(CASE WHEN (NYUKA_MONTH = '" + rollList.Roll_MBeforeFiveMonth + "' AND NYUKA_YEAR = '" + rollList.Roll_MYearBeforeFiveMonth + "') THEN NYUKA_QTY END) AS RECVD_QTY_2MONTH_AGO, ");
            updateSqlRecvdQty.Append(" SUM(CASE WHEN (NYUKA_MONTH = '" + rollList.Roll_MBeforeFourMonth + "' AND NYUKA_YEAR = '" + rollList.Roll_MYearBeforeFourMonth + "') THEN NYUKA_QTY END) AS RECVD_QTY_1MONTH_AGO ");
            updateSqlRecvdQty.Append(" FROM ( ");
            updateSqlRecvdQty.Append(" SELECT ");
            updateSqlRecvdQty.Append(" Z.INT_MATL_FC_SUMMARY_CD,");
            updateSqlRecvdQty.Append(" SUM(N.NYUKA_QTY) AS NYUKA_QTY,");
            updateSqlRecvdQty.Append(" N.NYUKA_MONTH,");
            updateSqlRecvdQty.Append(" N.NYUKA_YEAR ");
            updateSqlRecvdQty.Append(" FROM ( ");
            updateSqlRecvdQty.Append(" SELECT ");
            updateSqlRecvdQty.Append(" EXTRACT(MONTH FROM NYUKA_DATE) NYUKA_MONTH, ");
            updateSqlRecvdQty.Append(" EXTRACT(YEAR FROM NYUKA_DATE) NYUKA_YEAR, ");
            updateSqlRecvdQty.Append(" SUM(QUANTITY) / 1000 NYUKA_QTY,");
            updateSqlRecvdQty.Append(" VK.DIM_D,");
            updateSqlRecvdQty.Append(" VK.SHAPE,");
            updateSqlRecvdQty.Append(" VK.MATERIAL_LENGTH,");
            updateSqlRecvdQty.Append(" VK.MATL_KUBUN,");
            updateSqlRecvdQty.Append(" VK.INTERNAL_NAME_CD,");
            if (conditionPOMain == true)
            {
                updateSqlRecvdQty.Append(" VK.CONDITION_CD, ");
            }
            updateSqlRecvdQty.Append(" VK.MATL_SPECIAL_KUBUN ");
            
            
            updateSqlRecvdQty.Append(" FROM ");
            updateSqlRecvdQty.Append(" TRN_MATL_NYUKO_NYUKA TN ");
            updateSqlRecvdQty.Append(" JOIN V_KOUBAN_DETAILS VK ON VK.KOUBAN_CD = TN.KOUBAN_CD AND NYUKO_NYUKA_KUBUN = 1 ");
            updateSqlRecvdQty.Append(" WHERE  VK.KOUBAN_COMPLETE_DATE IS NULL AND ( ");
            updateSqlRecvdQty.Append(" (EXTRACT(MONTH FROM NYUKA_DATE) = '" + rollList.Roll_MBeforeSixMonth + "' AND EXTRACT(YEAR FROM NYUKA_DATE) = '" + rollList.Roll_MYearBeforeSixMonth + "') ");
            updateSqlRecvdQty.Append(" OR (EXTRACT(MONTH FROM NYUKA_DATE) = '" + rollList.Roll_MBeforeFiveMonth + "' AND EXTRACT(YEAR FROM NYUKA_DATE) = '" + rollList.Roll_MYearBeforeFiveMonth + "') ");
            updateSqlRecvdQty.Append(" OR (EXTRACT(MONTH FROM NYUKA_DATE) = '" + rollList.Roll_MBeforeFourMonth + "' AND EXTRACT(YEAR FROM NYUKA_DATE) = '" + rollList.Roll_MYearBeforeFourMonth + "') ");
            updateSqlRecvdQty.Append(" ) ");
            updateSqlRecvdQty.Append(" GROUP BY ");
            updateSqlRecvdQty.Append(" VK.SHAPE, ");
            updateSqlRecvdQty.Append(" VK.MATL_KUBUN, ");
            updateSqlRecvdQty.Append(" VK.DIM_D, ");
            updateSqlRecvdQty.Append(" VK.MATERIAL_LENGTH, ");
            updateSqlRecvdQty.Append(" VK.MATL_SPECIAL_KUBUN, ");
            updateSqlRecvdQty.Append(" VK.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlRecvdQty.Append(" VK.CONDITION_CD, ");
            }
            
            updateSqlRecvdQty.Append(" EXTRACT(YEAR FROM NYUKA_DATE), ");
            updateSqlRecvdQty.Append(" EXTRACT(MONTH FROM NYUKA_DATE) ");
            updateSqlRecvdQty.Append(" ) N, ");
            updateSqlRecvdQty.Append(" ( ");
            updateSqlRecvdQty.Append(" SELECT ");
            updateSqlRecvdQty.Append(" DIM_D, ");
            updateSqlRecvdQty.Append(" SHAPE, ");
            updateSqlRecvdQty.Append(" LENGTH, ");
            updateSqlRecvdQty.Append(" MATL_KUBUN, ");
            updateSqlRecvdQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlRecvdQty.Append(" MATL_SPECIAL_KUBUN, ");
            if(conditionPOMain == true)
            {
                updateSqlRecvdQty.Append(" CONDITION_CD, ");
            }
            updateSqlRecvdQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            
            updateSqlRecvdQty.Append(" FROM ");
            updateSqlRecvdQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlRecvdQty.Append(" ) Z ");
            updateSqlRecvdQty.Append(" WHERE N.DIM_D = Z.DIM_D ");
            updateSqlRecvdQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlRecvdQty.Append(" AND (Z.LENGTH IS NULL OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlRecvdQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlRecvdQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlRecvdQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if(conditionPOMain == true)
            {
                updateSqlRecvdQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            
            updateSqlRecvdQty.Append(" GROUP BY ");
            updateSqlRecvdQty.Append(" N.SHAPE, ");
            updateSqlRecvdQty.Append(" N.MATL_KUBUN, ");
            updateSqlRecvdQty.Append(" N.DIM_D, ");
            updateSqlRecvdQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlRecvdQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlRecvdQty.Append(" N.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlRecvdQty.Append(" N.CONDITION_CD, ");
            }
            
            updateSqlRecvdQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlRecvdQty.Append(" NYUKA_YEAR, ");
            updateSqlRecvdQty.Append(" NYUKA_MONTH ");
            updateSqlRecvdQty.Append(" ) GROUP BY INT_MATL_FC_SUMMARY_CD ");
            updateSqlRecvdQty.Append(" ) U ");
            updateSqlRecvdQty.Append(" WHERE T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD )");
            // updateSqlRecvdQty.Append(" ) ");
            ExecuteNonQuery(updateSqlRecvdQty.ToString());

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/02/10       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update REM_QTY_4MONTH_AGO in TEMP_INT_MATL_FC_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithRemQty4MonthAgo(int roll_Year, int roll_R)
        {

            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            StringBuilder updateSqlRemQty = new StringBuilder();
            //updateSqlRemQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T SET ");
            //updateSqlRemQty.Append(" (T.REM_QTY_4MONTH_AGO) = (");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" U.REM_QTY_4MONTH_AGO ");
            //updateSqlRemQty.Append(" FROM  ");
            //updateSqlRemQty.Append(" (SELECT ");
            //updateSqlRemQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" SUM(KOUBAN_MINUS_NYUKO_QTY) AS REM_QTY_4MONTH_AGO ");
            //updateSqlRemQty.Append(" FROM ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" SUM(DECODE(CONF_QTY, NULL, 0, CONF_QTY) - DECODE(TOT_NYUKO, NULL, 0, TOT_NYUKO)) / 1000 KOUBAN_MINUS_NYUKO_QTY, ");
            //updateSqlRemQty.Append(" DIM_D,");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN ");

            //updateSqlRemQty.Append(" FROM ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" ROLL_R, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" CONF_QTY, ");
            //updateSqlRemQty.Append(" (SELECT ");
            //updateSqlRemQty.Append(" SUM(TKO.QUANTITY) ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" TRN_FUTOU_NYUKO TKO ");
            //updateSqlRemQty.Append(" WHERE TKO.KOUBAN_CD = VKD.KOUBAN_CD) TOT_NYUKO, ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN ");

            //updateSqlRemQty.Append(" FROM V_KOUBAN_DETAILS VKD ");
            //updateSqlRemQty.Append(" JOIN TRN_PO_DTL TPD ON VKD.PO_DTL_CD = TPD.PO_DTL_CD ");
            //updateSqlRemQty.Append(" WHERE (KOUBAN_COMPLETE_DATE IS NULL) ");
            //updateSqlRemQty.Append(" AND ((YYYY < '" + rollList.Roll_RYearBeforeSixMonth + "') OR (ROLL_R < '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "')) ");
            //updateSqlRemQty.Append(" ) ");
            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" DIM_D,");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH,");
            //updateSqlRemQty.Append(" MATL_KUBUN,");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD,");
            //if (conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN ");

            //updateSqlRemQty.Append(" ) N, ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" LENGTH, ");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD ");

            //updateSqlRemQty.Append(" FROM TEMP_INT_MATL_FC_SUMMARY ");
            //updateSqlRemQty.Append(" ) Z ");
            //updateSqlRemQty.Append(" WHERE N.DIM_D = Z.DIM_D ");
            //updateSqlRemQty.Append(" AND N.SHAPE = Z.SHAPE ");
            //updateSqlRemQty.Append(" AND (Z.LENGTH IS NULL OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            //updateSqlRemQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            //updateSqlRemQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            //updateSqlRemQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            //}

            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" N.INTERNAL_NAME_CD, ");
            //updateSqlRemQty.Append(" N.DIM_D, ");
            //updateSqlRemQty.Append(" N.SHAPE, ");
            //updateSqlRemQty.Append(" N.MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" N.MATL_KUBUN, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" N.CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" N.MATL_SPECIAL_KUBUN ");

            //updateSqlRemQty.Append(" ) U ");
            //updateSqlRemQty.Append(" WHERE T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD)");

            
            ExecuteStoredProcedureRemQty4MonthAgo(Convert.ToInt32(rollList.Roll_RYearBeforeSixMonth), Convert.ToInt32(rollList.Roll_RBeforeSixMonth));
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/02/10       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update REM_QTY_3MONTH_AGO, REM_QTY_2MONTH_AGO, REM_QTY_1MONTH_AGO in TEMP_INT_MATL_FC_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithRemQty(int roll_Year, int roll_R)
        {

            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            StringBuilder updateSqlRemQty = new StringBuilder();


            //updateSqlRemQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T ");
            //updateSqlRemQty.Append(" SET(T.REM_QTY_3MONTH_AGO, T.REM_QTY_2MONTH_AGO, T.REM_QTY_1MONTH_AGO) = ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT REM_QTY_1MONTH_AGO, REM_QTY_2MONTH_AGO, REM_QTY_3MONTH_AGO  FROM(SELECT ");
            //updateSqlRemQty.Append(" SUM( ");
            //updateSqlRemQty.Append(" CASE ");
            //updateSqlRemQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') THEN ");
            //updateSqlRemQty.Append(" REM_QTY ");
            //updateSqlRemQty.Append(" END ");
            //updateSqlRemQty.Append(" ) AS REM_QTY_3MONTH_AGO, ");
            //updateSqlRemQty.Append(" SUM( ");
            //updateSqlRemQty.Append(" CASE ");
            //updateSqlRemQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') THEN ");
            //updateSqlRemQty.Append(" REM_QTY ");
            //updateSqlRemQty.Append(" END ");
            //updateSqlRemQty.Append(" ) AS REM_QTY_2MONTH_AGO, ");
            //updateSqlRemQty.Append(" SUM( ");
            //updateSqlRemQty.Append(" CASE ");
            //updateSqlRemQty.Append(" WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "') THEN ");
            //updateSqlRemQty.Append(" ");
            //updateSqlRemQty.Append(" REM_QTY ");
            //updateSqlRemQty.Append(" END ");
            //updateSqlRemQty.Append(" ) AS REM_QTY_1MONTH_AGO, INT_MATL_FC_SUMMARY_CD, COUNT(INT_MATL_FC_SUMMARY_CD) AS COUNT ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" ROLL_R, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" SUM(PO_PENDING_QTY) AS REM_QTY ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" ROLL_R, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" PO_PENDING_QTY ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" N.ROLL_R, ");
            //updateSqlRemQty.Append(" N.YYYY, ");
            //updateSqlRemQty.Append(" NVL(SUM(N.POQTY), 0) AS PO_PENDING_QTY ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" ROLL_R,");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" SUM(QTY) POQTY, ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH,");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN ");


            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" TRN_PO_DTL  TPD ");
            //updateSqlRemQty.Append(" JOIN V_HT_MATL_DETAILS VMATL ON TPD.MATERIAL_CD = VMATL.MATERIAL_CD ");
            //updateSqlRemQty.Append(" WHERE ");
            //updateSqlRemQty.Append(" TPD.PO_DTL_CD NOT IN( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" PO_DTL_CD ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" TRN_KOUBAN ");
            //updateSqlRemQty.Append(" ) ");
            //updateSqlRemQty.Append(" AND((ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') ");
            //updateSqlRemQty.Append(" OR (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') ");
            //updateSqlRemQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "')) ");
            //updateSqlRemQty.Append(" AND TPD.INACTIVE_FLAG = 0 ");// change to PO_STATUS
            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATL_KUBUN,  ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN,  ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD,  ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}

            //updateSqlRemQty.Append(" YYYY,  ");
            //updateSqlRemQty.Append(" ROLL_R ");
            //updateSqlRemQty.Append(" ) N, ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" LENGTH, ");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD ");

            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            //updateSqlRemQty.Append(" ) Z ");
            //updateSqlRemQty.Append(" WHERE ");
            //updateSqlRemQty.Append(" N.DIM_D = Z.DIM_D ");
            //updateSqlRemQty.Append(" AND N.SHAPE = Z.SHAPE ");
            //updateSqlRemQty.Append(" AND(Z.LENGTH IS NULL ");
            //updateSqlRemQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            //updateSqlRemQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            //updateSqlRemQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            //updateSqlRemQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            //}

            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" N.SHAPE, ");
            //updateSqlRemQty.Append(" N.MATL_KUBUN,");
            //updateSqlRemQty.Append(" N.DIM_D, ");
            //updateSqlRemQty.Append(" N.MATERIAL_LENGTH,");
            //updateSqlRemQty.Append(" N.MATL_SPECIAL_KUBUN,");
            //updateSqlRemQty.Append(" N.INTERNAL_NAME_CD,  ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" N.CONDITION_CD,");
            //}

            //updateSqlRemQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" ROLL_R");
            //updateSqlRemQty.Append(" ) ");
            //updateSqlRemQty.Append(" UNION ALL ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" ROLL_R, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" PO_PENDING_QTY ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" N.ROLL_R, ");
            //updateSqlRemQty.Append(" N.YYYY, ");
            //updateSqlRemQty.Append(" (SUM(DECODE(N.CONF_QTY, NULL, 0, N.CONF_QTY) - DECODE(N.TOT_NYUKO, NULL, 0, N.TOT_NYUKO)) / 1000) AS ");
            //updateSqlRemQty.Append(" PO_PENDING_QTY ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" ROLL_R, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}

            //updateSqlRemQty.Append(" NVL(SUM(CONF_QTY), 0) AS CONF_QTY, ");
            //updateSqlRemQty.Append(" NVL(SUM(( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" SUM(TKO.QUANTITY) ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" TRN_FUTOU_NYUKO TKO ");
            //updateSqlRemQty.Append(" WHERE ");
            //updateSqlRemQty.Append(" TKO.KOUBAN_CD = VKD.KOUBAN_CD ");
            //updateSqlRemQty.Append(" )), 0) TOT_NYUKO ");
            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" V_KOUBAN_DETAILS   VKD ");
            //updateSqlRemQty.Append(" JOIN TRN_PO_DTL TPD ON VKD.PO_DTL_CD = TPD.PO_DTL_CD ");
            //updateSqlRemQty.Append(" WHERE ");
            //updateSqlRemQty.Append(" ((ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') ");
            //updateSqlRemQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') ");
            //updateSqlRemQty.Append(" OR(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "')) ");
            //updateSqlRemQty.Append(" AND KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" MATL_KUBUN,  ");
            //updateSqlRemQty.Append(" DIM_D,   ");
            //updateSqlRemQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD,  ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}

            //updateSqlRemQty.Append(" YYYY,  ");
            //updateSqlRemQty.Append(" ROLL_R ");
            //updateSqlRemQty.Append(" ) N, ");
            //updateSqlRemQty.Append(" ( ");
            //updateSqlRemQty.Append(" SELECT ");
            //updateSqlRemQty.Append(" DIM_D, ");
            //updateSqlRemQty.Append(" SHAPE, ");
            //updateSqlRemQty.Append(" LENGTH, ");
            //updateSqlRemQty.Append(" MATL_KUBUN, ");
            //updateSqlRemQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlRemQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD ");

            //updateSqlRemQty.Append(" FROM ");
            //updateSqlRemQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            //updateSqlRemQty.Append(" ) Z ");
            //updateSqlRemQty.Append(" WHERE ");
            //updateSqlRemQty.Append(" N.DIM_D = Z.DIM_D ");
            //updateSqlRemQty.Append(" AND N.SHAPE = Z.SHAPE ");
            //updateSqlRemQty.Append(" AND(Z.LENGTH IS NULL ");
            //updateSqlRemQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            //updateSqlRemQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            //updateSqlRemQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            //updateSqlRemQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            //}

            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" N.SHAPE,  ");
            //updateSqlRemQty.Append(" N.MATL_KUBUN, ");
            //updateSqlRemQty.Append(" N.DIM_D,  ");
            //updateSqlRemQty.Append(" N.MATERIAL_LENGTH, ");
            //updateSqlRemQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            //updateSqlRemQty.Append(" N.INTERNAL_NAME_CD,   ");
            //if (conditionPOMain == true)
            //{
            //    updateSqlRemQty.Append(" N.CONDITION_CD, ");
            //}


            //updateSqlRemQty.Append(" Z.INT_MATL_FC_SUMMARY_CD,  ");
            //updateSqlRemQty.Append(" YYYY,  ");
            //updateSqlRemQty.Append(" ROLL_R ");
            //updateSqlRemQty.Append(" ) ");
            //updateSqlRemQty.Append(" ) ");
            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlRemQty.Append(" YYYY, ");
            //updateSqlRemQty.Append(" ROLL_R ");
            //updateSqlRemQty.Append(" ) ");
            //updateSqlRemQty.Append(" GROUP BY ");
            //updateSqlRemQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            //updateSqlRemQty.Append(" ) DTL ");
            //updateSqlRemQty.Append(" WHERE DTL.INT_MATL_FC_SUMMARY_CD = T.INT_MATL_FC_SUMMARY_CD) ");
            ////updateSqlRemQty.Append(" ) ");


            //ExecuteNonQuery(updateSqlRemQty.ToString());

            ExecuteStoredProcedureRemQtyLast3Months(Convert.ToInt32(rollList.Roll_RBeforeSixMonth), Convert.ToInt32(rollList.Roll_RYearBeforeSixMonth), Convert.ToInt32(rollList.Roll_RBeforeFiveMonth), Convert.ToInt32(rollList.Roll_RYearBeforeFiveMonth), Convert.ToInt32(rollList.Roll_RBeforeFourMonth), Convert.ToInt32(rollList.Roll_RYearBeforeFourMonth));

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/01       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update INT_STOCK_QTY in TEMP_INT_MATL_FC_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithIntStockQty()
        {
            StringBuilder updateSqlIntStockQty = new StringBuilder();
          

            updateSqlIntStockQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T ");
            updateSqlIntStockQty.Append(" SET ");
            updateSqlIntStockQty.Append(" (T.INT_STOCK_QTY) = ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" ROUND(U.INT_STOCK_QTY, 1) AS INT_STOCK_QTY ");
            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" STOCK.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlIntStockQty.Append(" STOCK_QTY, ");
            updateSqlIntStockQty.Append(" SHUKKO_QTY, ");
            updateSqlIntStockQty.Append(" SHUKKA_QTY, ");
            updateSqlIntStockQty.Append(" (NVL(STOCK_QTY, 0) - (NVL(SHUKKO_QTY, 0) + NVL(SHUKKA_QTY, 0))) AS INT_STOCK_QTY ");
            updateSqlIntStockQty.Append(" FROM ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlIntStockQty.Append(" (NVL(NYUKA_QTY, 0) - NVL(ACT_ALLOC_QTY, 0)) AS STOCK_QTY ");
            updateSqlIntStockQty.Append(" FROM ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlIntStockQty.Append(" SUM(N.QUANTITY) AS NYUKA_QTY ");
            updateSqlIntStockQty.Append(" FROM ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" (QUANTITY / 1000) AS QUANTITY, ");
            updateSqlIntStockQty.Append(" VKD.DIM_D, ");
            updateSqlIntStockQty.Append(" VKD.SHAPE, ");
            updateSqlIntStockQty.Append(" VKD.MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" VKD.MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" VKD.INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append("  VKD.CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" VKD.MATL_SPECIAL_KUBUN ");
            
            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TRN_MATL_NYUKO_NYUKA   TMN ");
            updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS       VKD ON TMN.KOUBAN_CD = VKD.KOUBAN_CD ");
            updateSqlIntStockQty.Append(" AND NYUKO_NYUKA_KUBUN = 1 ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlIntStockQty.Append(" ) N, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            
            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlIntStockQty.Append(" ) Z ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL ");
            updateSqlIntStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            updateSqlIntStockQty.Append(" GROUP BY ");
            updateSqlIntStockQty.Append(" N.SHAPE, ");
            updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.DIM_D, ");
            updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" N.CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlIntStockQty.Append(" ) A, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlIntStockQty.Append(" SUM(N.ACT_ALLOC_QTY) AS ACT_ALLOC_QTY ");
            updateSqlIntStockQty.Append(" FROM ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" (ACT_ALLOC_QTY / 1000) AS ACT_ALLOC_QTY, ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN ");

            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TRN_ACTUAL_ALLOCATION   AA ");
            updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS        VK ON AA.KOUBAN_CD = VK.KOUBAN_CD ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlIntStockQty.Append(" ) N, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }

            updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");

            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlIntStockQty.Append(" ) Z ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            updateSqlIntStockQty.Append(" GROUP BY ");
            updateSqlIntStockQty.Append(" N.SHAPE, ");
            updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.DIM_D, ");
            updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" N.CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlIntStockQty.Append(" ) B ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD = B.INT_MATL_FC_SUMMARY_CD(+) ");
            updateSqlIntStockQty.Append(" ) STOCK, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlIntStockQty.Append(" SUM(N.QUANTITY) AS SHUKKO_QTY ");
            updateSqlIntStockQty.Append(" FROM ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" QUANTITY / 1000 AS QUANTITY, ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN ");
            

            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TRN_MATL_SHUKKO    TS ");
            updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VK ON TS.KOUBAN_CD = VK.KOUBAN_CD ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlIntStockQty.Append(" ) N, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");
           
            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlIntStockQty.Append(" ) Z ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            updateSqlIntStockQty.Append(" GROUP BY ");
            updateSqlIntStockQty.Append(" N.SHAPE, ");
            updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.DIM_D, ");
            updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" N.CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlIntStockQty.Append(" ) SHUKKO, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlIntStockQty.Append(" SUM(N.QUANTITY) AS SHUKKA_QTY ");
            updateSqlIntStockQty.Append(" FROM ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" QTY / 1000 AS QUANTITY, ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN ");
            

            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TRN_MATL_SHUKKA   TS ");
            updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VK ON TS.KOUBAN_CD = VK.KOUBAN_CD ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlIntStockQty.Append(" ) N, ");
            updateSqlIntStockQty.Append(" ( ");
            updateSqlIntStockQty.Append(" SELECT ");
            updateSqlIntStockQty.Append(" DIM_D, ");
            updateSqlIntStockQty.Append(" SHAPE, ");
            updateSqlIntStockQty.Append(" LENGTH, ");
            updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");
           
            updateSqlIntStockQty.Append(" FROM ");
            updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlIntStockQty.Append(" ) Z ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            updateSqlIntStockQty.Append(" GROUP BY ");
            updateSqlIntStockQty.Append(" N.SHAPE, ");
            updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.DIM_D, ");
            updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if (conditionPOMain == true)
            {
                updateSqlIntStockQty.Append(" --N.CONDITION_CD, ");
            }
            updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlIntStockQty.Append(" ) SHUKKA ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" STOCK.INT_MATL_FC_SUMMARY_CD = SHUKKO.INT_MATL_FC_SUMMARY_CD(+) AND ");
            updateSqlIntStockQty.Append(" STOCK.INT_MATL_FC_SUMMARY_CD = SHUKKA.INT_MATL_FC_SUMMARY_CD(+) ");
            updateSqlIntStockQty.Append(" ) U ");
            updateSqlIntStockQty.Append(" WHERE ");
            updateSqlIntStockQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ) ");

            //updateSqlIntStockQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T SET ");
            //updateSqlIntStockQty.Append(" (T.INT_STOCK_QTY) = ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" ROUND(U.INT_STOCK_QTY, 1) AS INT_STOCK_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" STOCK.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" (NVL(STOCK_QTY, 0) - NVL(SHUKKO_QTY, 0)) AS INT_STOCK_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" (NVL(NYUKA_QTY, 0) - NVL(ACT_ALLOC_QTY, 0)) AS STOCK_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" SUM(N.QUANTITY) AS NYUKA_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" (QUANTITY / 1000) AS QUANTITY, ");
            //updateSqlIntStockQty.Append(" VKD.DIM_D, ");
            //updateSqlIntStockQty.Append(" VKD.SHAPE, ");
            //updateSqlIntStockQty.Append(" VKD.MATERIAL_LENGTH, ");
            //updateSqlIntStockQty.Append(" VKD.MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" VKD.INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" VKD.CONDITION_CD, ");
            //}
            //updateSqlIntStockQty.Append(" VKD.MATL_SPECIAL_KUBUN ");

            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_MATL_NYUKO_NYUKA          TMN ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VKD ON TMN.KOUBAN_CD = VKD.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" AND NYUKO_NYUKA_KUBUN = 1 ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" ) N, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" DIM_D, ");
            //updateSqlIntStockQty.Append(" SHAPE, ");
            //updateSqlIntStockQty.Append(" LENGTH, ");
            //updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");

            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ) Z ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            //updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            //updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL ");
            //updateSqlIntStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            //updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            //updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            //updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            //}

            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" N.SHAPE,");
            //updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" N.DIM_D, ");
            //updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            //updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            //updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" N.CONDITION_CD, ");
            //}

            //updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            //updateSqlIntStockQty.Append(" ) A, ");
            //updateSqlIntStockQty.Append(" ( SELECT ");
            //updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" SUM(N.ACT_ALLOC_QTY) AS ACT_ALLOC_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" (ACT_ALLOC_QTY / 1000) AS ACT_ALLOC_QTY, ");
            //updateSqlIntStockQty.Append(" DIM_D, ");
            //updateSqlIntStockQty.Append(" SHAPE, ");
            //updateSqlIntStockQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            //if (conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN ");


            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_ACTUAL_ALLOCATION   AA ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS     VK ON AA.KOUBAN_CD = VK.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" ) N, ");
            //updateSqlIntStockQty.Append(" ( SELECT ");
            //updateSqlIntStockQty.Append(" DIM_D, ");
            //updateSqlIntStockQty.Append(" SHAPE, ");
            //updateSqlIntStockQty.Append(" LENGTH, ");
            //updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            //if (conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");


            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ) Z ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            //updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            //updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL ");
            //updateSqlIntStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            //updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            //updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            //updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            //}

            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" N.SHAPE, ");
            //updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" N.DIM_D, ");
            //updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            //updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            //updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" N.CONDITION_CD, ");
            //}

            //updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            //updateSqlIntStockQty.Append(" ) B ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD = B.INT_MATL_FC_SUMMARY_CD(+) ");
            //updateSqlIntStockQty.Append(" ) STOCK, ");
            //updateSqlIntStockQty.Append(" ( SELECT ");
            //updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" SUM(N.QUANTITY) AS SHUKKO_QTY ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" ( SELECT ");
            //updateSqlIntStockQty.Append(" (QUANTITY / 1000) AS QUANTITY, ");
            //updateSqlIntStockQty.Append(" DIM_D, ");
            //updateSqlIntStockQty.Append(" SHAPE, ");
            //updateSqlIntStockQty.Append(" MATERIAL_LENGTH, ");
            //updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN ");

            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_MATL_SHUKKO         TS ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VK ON TS.KOUBAN_CD = VK.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" ) N, ");
            //updateSqlIntStockQty.Append(" ( SELECT ");
            //updateSqlIntStockQty.Append(" DIM_D, ");
            //updateSqlIntStockQty.Append(" SHAPE, ");
            //updateSqlIntStockQty.Append(" LENGTH, ");
            //updateSqlIntStockQty.Append(" MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" INTERNAL_NAME_CD, ");
            //updateSqlIntStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            //if (conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" CONDITION_CD, ");
            //}
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");


            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ) Z ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" N.DIM_D = Z.DIM_D ");
            //updateSqlIntStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            //updateSqlIntStockQty.Append(" AND(Z.LENGTH IS NULL ");
            //updateSqlIntStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            //updateSqlIntStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            //updateSqlIntStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            //updateSqlIntStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" AND N.CONDITION_CD = Z.CONDITION_CD ");
            //}

            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" N.SHAPE, ");
            //updateSqlIntStockQty.Append(" N.MATL_KUBUN, ");
            //updateSqlIntStockQty.Append(" N.DIM_D, ");
            //updateSqlIntStockQty.Append(" N.MATERIAL_LENGTH, ");
            //updateSqlIntStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            //updateSqlIntStockQty.Append(" N.INTERNAL_NAME_CD, ");
            //if(conditionPOMain == true)
            //{
            //    updateSqlIntStockQty.Append(" N.CONDITION_CD, ");
            //}

            //updateSqlIntStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            //updateSqlIntStockQty.Append(" ) C ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" STOCK.INT_MATL_FC_SUMMARY_CD = C.INT_MATL_FC_SUMMARY_CD(+) ) U ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ) ");

            ExecuteNonQuery(updateSqlIntStockQty.ToString());

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/14       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update EXT_STOCK_QTY in TEMP_INT_MATL_FC_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateTempIntMatlFCSummaryWithExtStockQty()
        {
            StringBuilder updateSqlExtStockQty = new StringBuilder();
            updateSqlExtStockQty.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY T ");
            updateSqlExtStockQty.Append(" SET ");
            updateSqlExtStockQty.Append(" (T.EXT_STOCK_QTY) = ( ");
            updateSqlExtStockQty.Append(" SELECT ");
            updateSqlExtStockQty.Append(" ROUND(U.EXT_STOCK_QTY, 1) AS EXT_STOCK_QTY ");
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" (SELECT ");
            updateSqlExtStockQty.Append(" FUTOU_STOCK.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlExtStockQty.Append(" FUTOU_STOCK, ");
            updateSqlExtStockQty.Append(" SHUKKO_QTY, ");
            updateSqlExtStockQty.Append(" (NVL(FUTOU_STOCK, 0) - NVL(SHUKKO_QTY, 0)) AS EXT_STOCK_QTY ");
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" (SELECT ");
            updateSqlExtStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlExtStockQty.Append(" NYUKO_QTY, ");
            updateSqlExtStockQty.Append(" NYUKA_QTY, ");
            updateSqlExtStockQty.Append(" (NVL(NYUKO_QTY, 0) - NVL(NYUKA_QTY, 0)) AS FUTOU_STOCK ");
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" ( ");
            updateSqlExtStockQty.Append(" SELECT ");
            updateSqlExtStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlExtStockQty.Append(" NVL(SUM(N.QUANTITY), 0) AS NYUKO_QTY ");
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" (SELECT ");
            updateSqlExtStockQty.Append(" (QUANTITY / 1000) AS QUANTITY, ");
            updateSqlExtStockQty.Append(" DIM_D, ");
            updateSqlExtStockQty.Append(" SHAPE, ");
            updateSqlExtStockQty.Append(" MATERIAL_LENGTH, ");
            updateSqlExtStockQty.Append(" MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlExtStockQty.Append(" MATL_SPECIAL_KUBUN ");
            
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" TRN_FUTOU_NYUKO          TNKO ");
            updateSqlExtStockQty.Append(" INNER JOIN V_KOUBAN_DETAILS   VKD ON TNKO.KOUBAN_CD = VKD.KOUBAN_CD ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlExtStockQty.Append(" ) N, ");
            updateSqlExtStockQty.Append(" (SELECT ");
            updateSqlExtStockQty.Append(" DIM_D, ");
            updateSqlExtStockQty.Append(" SHAPE, ");
            updateSqlExtStockQty.Append(" LENGTH, ");
            updateSqlExtStockQty.Append(" MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlExtStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlExtStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            
            
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlExtStockQty.Append(" ) Z ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlExtStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlExtStockQty.Append(" AND(Z.LENGTH IS NULL ");
            updateSqlExtStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlExtStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlExtStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlExtStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" AND N.CONDITION_CD= Z.CONDITION_CD ");
            }
            
            updateSqlExtStockQty.Append(" GROUP BY ");
            updateSqlExtStockQty.Append(" N.SHAPE, ");
            updateSqlExtStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" N.DIM_D, ");
            updateSqlExtStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlExtStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlExtStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" N.CONDITION_CD, ");
            }
            
            updateSqlExtStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlExtStockQty.Append(" ) A, ");
            updateSqlExtStockQty.Append(" (SELECT ");
            updateSqlExtStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlExtStockQty.Append(" NVL(SUM(N.QUANTITY), 0) AS NYUKA_QTY ");
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" ( ");
            updateSqlExtStockQty.Append(" SELECT ");
            updateSqlExtStockQty.Append(" (QUANTITY / 1000) AS QUANTITY, ");
            updateSqlExtStockQty.Append(" VKD.DIM_D, ");
            updateSqlExtStockQty.Append(" VKD.SHAPE, ");
            updateSqlExtStockQty.Append(" VKD.MATERIAL_LENGTH, ");
            updateSqlExtStockQty.Append(" VKD.MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" VKD.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" VKD.CONDITION_CD, ");
            }
            updateSqlExtStockQty.Append(" MATL_SPECIAL_KUBUN ");
            
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" TRN_MATL_NYUKO_NYUKA          TNKA ");
            updateSqlExtStockQty.Append(" JOIN V_KOUBAN_DETAILS   VKD ON TNKA.KOUBAN_CD = VKD.KOUBAN_CD ");
            updateSqlExtStockQty.Append("  WHERE ");
            updateSqlExtStockQty.Append(" VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlExtStockQty.Append(" ) N, ");
            updateSqlExtStockQty.Append(" (SELECT ");
            updateSqlExtStockQty.Append(" DIM_D, ");
            updateSqlExtStockQty.Append(" SHAPE, ");
            updateSqlExtStockQty.Append(" LENGTH, ");
            updateSqlExtStockQty.Append(" MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlExtStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlExtStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            
            
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlExtStockQty.Append(" ) Z ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlExtStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlExtStockQty.Append(" AND(Z.LENGTH IS NULL ");
            updateSqlExtStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlExtStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlExtStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlExtStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append("  AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            
            updateSqlExtStockQty.Append(" GROUP BY ");
            updateSqlExtStockQty.Append(" N.SHAPE, ");
            updateSqlExtStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" N.DIM_D, ");
            updateSqlExtStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlExtStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlExtStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" N.CONDITION_CD, ");
            }
            
            updateSqlExtStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlExtStockQty.Append(" ) B ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD = B.INT_MATL_FC_SUMMARY_CD(+) ");
            updateSqlExtStockQty.Append(" ) FUTOU_STOCK, ");
            updateSqlExtStockQty.Append(" ( SELECT ");
            updateSqlExtStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD, ");
            updateSqlExtStockQty.Append(" NVL(SUM(N.QUANTITY), 0) AS SHUKKO_QTY ");
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append("  ( SELECT ");
            updateSqlExtStockQty.Append(" (QUANTITY / 1000) AS QUANTITY, ");
            updateSqlExtStockQty.Append(" DIM_D, ");
            updateSqlExtStockQty.Append(" SHAPE, ");
            updateSqlExtStockQty.Append(" MATERIAL_LENGTH, ");
            updateSqlExtStockQty.Append(" MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlExtStockQty.Append(" MATL_SPECIAL_KUBUN ");
            
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" TRN_FUTOU_SHUKKO TS ");
            updateSqlExtStockQty.Append(" JOIN V_KOUBAN_DETAILS  VKD ON TS.KOUBAN_CD = VKD.KOUBAN_CD ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            updateSqlExtStockQty.Append(" ) N, ");
            updateSqlExtStockQty.Append(" ( SELECT ");
            updateSqlExtStockQty.Append(" DIM_D, ");
            updateSqlExtStockQty.Append(" SHAPE, ");
            updateSqlExtStockQty.Append(" LENGTH, ");
            updateSqlExtStockQty.Append(" MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" INTERNAL_NAME_CD, ");
            updateSqlExtStockQty.Append(" MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN, ");
            if (conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" CONDITION_CD, ");
            }
            updateSqlExtStockQty.Append(" INT_MATL_FC_SUMMARY_CD ");
            
            
            updateSqlExtStockQty.Append(" FROM ");
            updateSqlExtStockQty.Append(" TEMP_INT_MATL_FC_SUMMARY ");
            updateSqlExtStockQty.Append(" ) Z ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" N.DIM_D = Z.DIM_D ");
            updateSqlExtStockQty.Append(" AND N.SHAPE = Z.SHAPE ");
            updateSqlExtStockQty.Append(" AND(Z.LENGTH IS NULL ");
            updateSqlExtStockQty.Append(" OR N.MATERIAL_LENGTH = Z.LENGTH) ");
            updateSqlExtStockQty.Append(" AND N.MATL_KUBUN = Z.MATL_KUBUN ");
            updateSqlExtStockQty.Append(" AND N.MATL_SPECIAL_KUBUN = Z.MATL_SPECIAL_KUBUN ");
            updateSqlExtStockQty.Append(" AND N.INTERNAL_NAME_CD = Z.INTERNAL_NAME_CD ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append("  AND N.CONDITION_CD = Z.CONDITION_CD ");
            }
            
            updateSqlExtStockQty.Append(" GROUP BY ");
            updateSqlExtStockQty.Append(" N.SHAPE, ");
            updateSqlExtStockQty.Append(" N.MATL_KUBUN, ");
            updateSqlExtStockQty.Append(" N.DIM_D, ");
            updateSqlExtStockQty.Append(" N.MATERIAL_LENGTH, ");
            updateSqlExtStockQty.Append(" N.MATL_SPECIAL_KUBUN, ");
            updateSqlExtStockQty.Append(" N.INTERNAL_NAME_CD, ");
            if(conditionPOMain == true)
            {
                updateSqlExtStockQty.Append(" N.CONDITION_CD, ");
            }
            
            updateSqlExtStockQty.Append(" Z.INT_MATL_FC_SUMMARY_CD ");
            updateSqlExtStockQty.Append(" ) C ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" FUTOU_STOCK.INT_MATL_FC_SUMMARY_CD = C.INT_MATL_FC_SUMMARY_CD(+) ");
            updateSqlExtStockQty.Append(" ) U ");
            updateSqlExtStockQty.Append(" WHERE ");
            updateSqlExtStockQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD)");
            //updateSqlExtStockQty.Append(" ) ");



            
            ExecuteNonQuery(updateSqlExtStockQty.ToString());

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/28       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update PO_REM_QTY_PREV in TEMP_INT_MATL_FC_SUMMARY table    
        /// 
        public void UpdateRollMatlSummaryWithPORemQtyPrev(int roll_Year, int roll_R)
        {

            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            StringBuilder updateSqlPORemQtyPrevQty = new StringBuilder();

            //updateSqlPORemQtyPrevQty.Append(" UPDATE TEMP_ROLL_MATL_SUMMARY T ");
            //updateSqlPORemQtyPrevQty.Append(" SET (PO_REM_QTY_PREV) = ( ");
            //updateSqlPORemQtyPrevQty.Append(" SELECT PO_REM_QTY_PREV ");
            //updateSqlPORemQtyPrevQty.Append(" FROM ( ");
            //updateSqlPORemQtyPrevQty.Append(" SELECT ");
            //updateSqlPORemQtyPrevQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" A.MATERIAL_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" SUM(B.KOUBAN_MINUS_NYUKO_QTY) AS PO_REM_QTY_PREV ");
            //updateSqlPORemQtyPrevQty.Append(" FROM ( ");
            //updateSqlPORemQtyPrevQty.Append(" SELECT ");
            //updateSqlPORemQtyPrevQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" MATERIAL_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" MUKESAKI_CD ");
            //updateSqlPORemQtyPrevQty.Append(" FROM ");
            //updateSqlPORemQtyPrevQty.Append(" TEMP_ROLL_MATL_SUMMARY RMS  ");
            ////updateSqlPORemQtyPrevQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");
            //updateSqlPORemQtyPrevQty.Append(" ) A, ");
            //updateSqlPORemQtyPrevQty.Append(" ( ");
            //updateSqlPORemQtyPrevQty.Append(" SELECT ");
            //updateSqlPORemQtyPrevQty.Append(" MATERIAL_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" MUKESAKI_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" SUM(DECODE(CONF_QTY, NULL, 0, CONF_QTY) - DECODE(TOT_NYUKO, NULL, 0, TOT_NYUKO)) / 1000 AS KOUBAN_MINUS_NYUKO_QTY ");
            //updateSqlPORemQtyPrevQty.Append(" FROM ( ");
            //updateSqlPORemQtyPrevQty.Append(" SELECT ");
            //updateSqlPORemQtyPrevQty.Append(" TPD.MATERIAL_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" TPD.MUKESAKI_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" ROLL_R, ");
            //updateSqlPORemQtyPrevQty.Append(" YYYY, ");
            //updateSqlPORemQtyPrevQty.Append(" CONF_QTY, ");
            //updateSqlPORemQtyPrevQty.Append(" (SELECT ");
            //updateSqlPORemQtyPrevQty.Append(" SUM(TKO.QUANTITY) ");
            //updateSqlPORemQtyPrevQty.Append(" FROM ");
            //updateSqlPORemQtyPrevQty.Append(" TRN_FUTOU_NYUKO TKO ");
            //updateSqlPORemQtyPrevQty.Append(" WHERE TKO.KOUBAN_CD = VKD.KOUBAN_CD) AS TOT_NYUKO ");
            //updateSqlPORemQtyPrevQty.Append(" FROM ");
            //updateSqlPORemQtyPrevQty.Append(" V_KOUBAN_DETAILS VKD ");
            //updateSqlPORemQtyPrevQty.Append(" JOIN TRN_PO_DTL TPD ON VKD.PO_DTL_CD = TPD.PO_DTL_CD ");
            //updateSqlPORemQtyPrevQty.Append(" WHERE ");
            //updateSqlPORemQtyPrevQty.Append(" KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlPORemQtyPrevQty.Append(" AND ( (YYYY < '" + rollList.Roll_RYearBeforeSixMonth + "') ");
            //updateSqlPORemQtyPrevQty.Append(" OR (YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND ROLL_R < '" + rollList.Roll_RBeforeSixMonth + "')) ");
            //updateSqlPORemQtyPrevQty.Append(" ) ");
            //updateSqlPORemQtyPrevQty.Append(" GROUP BY ");
            //updateSqlPORemQtyPrevQty.Append(" MATERIAL_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" MUKESAKI_CD ");
            //updateSqlPORemQtyPrevQty.Append(" ) B ");
            //updateSqlPORemQtyPrevQty.Append(" WHERE ");
            //updateSqlPORemQtyPrevQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlPORemQtyPrevQty.Append(" AND A.MUKESAKI_CD = B.MUKESAKI_CD ");
            //updateSqlPORemQtyPrevQty.Append(" GROUP BY ");
            //updateSqlPORemQtyPrevQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" A.MATERIAL_CD, ");
            //updateSqlPORemQtyPrevQty.Append(" A.MUKESAKI_CD ");
            //updateSqlPORemQtyPrevQty.Append(" ) U ");
            //updateSqlPORemQtyPrevQty.Append(" WHERE ");
            //updateSqlPORemQtyPrevQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ");
            //updateSqlPORemQtyPrevQty.Append(" AND T.MATERIAL_CD = U.MATERIAL_CD ");
            //updateSqlPORemQtyPrevQty.Append(" AND T.MUKESAKI_CD = U.MUKESAKI_CD) ");
            ////updateSqlPORemQtyPrevQty.Append(" ) ");          

            //ExecuteNonQuery(updateSqlPORemQtyPrevQty.ToString());
            ExecuteStoredProcedureUpdatePoRemQtyPrev(Convert.ToInt32(rollList.Roll_RYearBeforeSixMonth), Convert.ToInt32(rollList.Roll_RBeforeSixMonth));
        }

        public void UpdateRollMatlSummaryWithNyukaQty(int roll_Year, int roll_R)
        {
            RollYearList rollList = new RollYearList(roll_Year, roll_R);
            //StringBuilder updateSqlNyukaQty = new StringBuilder();
            //updateSqlNyukaQty.Append(" UPDATE TEMP_ROLL_MATL_SUMMARY T ");
            //updateSqlNyukaQty.Append(" SET ");
            //updateSqlNyukaQty.Append(" NYUKA_QTY_BOFORE_3 = ( ");
            //updateSqlNyukaQty.Append(" SELECT ");
            //updateSqlNyukaQty.Append(" NVL(GET_MTL_NYKA_QTY_BY_MONTH(T.MATERIAL_CD,'" + rollList.Roll_MYearBeforeSixMonth + "/" + rollList.Roll_MBeforeSixMonth + "'), 0)/1000 ");
            //updateSqlNyukaQty.Append(" FROM DUAL");
            //updateSqlNyukaQty.Append(" ), ");
            //updateSqlNyukaQty.Append(" NYUKA_QTY_BOFORE_2 = ( ");
            //updateSqlNyukaQty.Append(" SELECT ");
            //updateSqlNyukaQty.Append(" NVL(GET_MTL_NYKA_QTY_BY_MONTH(T.MATERIAL_CD,'" + rollList.Roll_MYearBeforeFiveMonth + "/" + rollList.Roll_MBeforeFiveMonth + "'), 0)/1000 ");
            //updateSqlNyukaQty.Append(" FROM DUAL ");
            //updateSqlNyukaQty.Append(" ), ");
            //updateSqlNyukaQty.Append(" NYUKA_QTY_BOFORE_1 = (SELECT NVL(GET_MTL_NYKA_QTY_BY_MONTH(T.MATERIAL_CD,'" + rollList.Roll_MYearBeforeFourMonth + "/" + rollList.Roll_MBeforeFourMonth + "'), 0)/1000 ");
            //updateSqlNyukaQty.Append(" FROM DUAL) ");
            //updateSqlNyukaQty.Append(" WHERE ");

            //updateSqlNyukaQty.Append(" T.MATERIAL_CD = T.MATERIAL_CD ");

            string RollYr_BeforeSixMonth = rollList.Roll_MYearBeforeSixMonth + "/" + rollList.Roll_MBeforeSixMonth;
            string RollYr_BeforeFiveMonth = rollList.Roll_MYearBeforeFiveMonth + "/" + rollList.Roll_MBeforeFiveMonth;
            string RollYr_BeforeFourMonth = rollList.Roll_MYearBeforeFourMonth + "/" + rollList.Roll_MBeforeFourMonth;
            ExecuteStoredProcedureUpdateNyukaQtyLast3Months(RollYr_BeforeSixMonth, RollYr_BeforeFiveMonth, RollYr_BeforeFourMonth);
            

        }
        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/24       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update PO_REM_QTY_R_1_1,PO_REM_QTY_R_1_2,PO_REM_QTY_R_1_3,PO_REM_QTY_R_1_4,PO_REM_QTY_R_2_1,PO_REM_QTY_R_2_2,PO_REM_QTY_R_2_3,PO_REM_QTY_R_2_4,PO_REM_QTY_R_3_1,
        /// PO_REM_QTY_R_3_2,PO_REM_QTY_R_3_3,PO_REM_QTY_R_3_4 in TEMP_ROLL_MATL_SUMMARY table    
        /// 
        public void UpdateRollMatlSummaryWithPORemQty(int roll_Year, int roll_R)
        {

            RollYearList rollList = new RollYearList(roll_Year, roll_R);
            StringBuilder updateSqlPORemQty = new StringBuilder();
           // updateSqlPORemQty.Append(" UPDATE TEMP_ROLL_MATL_SUMMARY T  ");
           // updateSqlPORemQty.Append(" SET ");
           // updateSqlPORemQty.Append(" (T.PO_REM_QTY_R_1_1, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_1_2, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_1_3, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_1_4, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_2_1, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_2_2, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_2_3, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_2_4, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_3_1, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_3_2, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_3_3, ");
           // updateSqlPORemQty.Append(" T.PO_REM_QTY_R_3_4) = ( ");
           // updateSqlPORemQty.Append(" SELECT ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_1_1, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_1_2, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_1_3, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_1_4, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_2_1, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_2_2, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_2_3, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_2_4, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_3_1, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_3_2, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_3_3, ");
           // updateSqlPORemQty.Append(" U.PO_REM_QTY_R_3_4 ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_1_1) AS PO_REM_QTY_R_1_1, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_1_2) AS PO_REM_QTY_R_1_2, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_1_3) AS PO_REM_QTY_R_1_3, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_1_4) AS PO_REM_QTY_R_1_4, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_2_1) AS PO_REM_QTY_R_2_1, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_2_2) AS PO_REM_QTY_R_2_2, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_2_3) AS PO_REM_QTY_R_2_3, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_2_4) AS PO_REM_QTY_R_2_4, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_3_1) AS PO_REM_QTY_R_3_1, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_3_2) AS PO_REM_QTY_R_3_2, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_3_3) AS PO_REM_QTY_R_3_3, ");
           // updateSqlPORemQty.Append(" SUM(PO_REM_QTY_R_3_4) AS PO_REM_QTY_R_3_4 ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" ((SELECT ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 1) THEN  POQTY END  ) AS PO_REM_QTY_R_1_1, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_REM_QTY_R_1_2, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_REM_QTY_R_1_3, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_REM_QTY_R_1_4, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_REM_QTY_R_2_1, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_REM_QTY_R_2_2, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_REM_QTY_R_2_3, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_REM_QTY_R_2_4, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_REM_QTY_R_3_1, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_REM_QTY_R_3_2, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_REM_QTY_R_3_3, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_REM_QTY_R_3_4 ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" A.MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" A.MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" B.YYYY, ");
           // updateSqlPORemQty.Append(" B.ROLL_R, ");
           // updateSqlPORemQty.Append(" B.CHANCE, ");
           // updateSqlPORemQty.Append(" SUM(B.POQTY) POQTY ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" ( ");
           // updateSqlPORemQty.Append(" SELECT ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" TEMP_ROLL_MATL_SUMMARY  ");
           //// updateSqlPORemQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");
           // updateSqlPORemQty.Append(" ) A, ");
           // updateSqlPORemQty.Append(" ( ");
           // updateSqlPORemQty.Append(" SELECT ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" YYYY, ");
           // updateSqlPORemQty.Append(" ROLL_R, ");
           // updateSqlPORemQty.Append(" CHANCE, ");
           // updateSqlPORemQty.Append(" SUM(QTY) POQTY ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" TRN_PO_DTL TPD ");
           // updateSqlPORemQty.Append(" WHERE ");
           // updateSqlPORemQty.Append(" TPD.PO_DTL_CD NOT IN( ");
           // updateSqlPORemQty.Append(" SELECT ");
           // updateSqlPORemQty.Append(" PO_DTL_CD ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" TRN_KOUBAN ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" AND TPD.INACTIVE_FLAG = 0 "); //change to PO_STATUS
           // updateSqlPORemQty.Append(" AND((ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') OR");
           // updateSqlPORemQty.Append(" (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') OR");
           // updateSqlPORemQty.Append(" (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "')) ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" YYYY, ");
           // updateSqlPORemQty.Append(" ROLL_R, ");
           // updateSqlPORemQty.Append(" CHANCE ");
           // updateSqlPORemQty.Append(" ) B ");
           // updateSqlPORemQty.Append(" WHERE ");
           // updateSqlPORemQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
           // updateSqlPORemQty.Append(" AND A.MUKESAKI_CD = B.MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" A.MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" A.MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" B.YYYY, ");
           // updateSqlPORemQty.Append(" B.ROLL_R, ");
           // updateSqlPORemQty.Append(" B.CHANCE ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" UNION ALL ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 1) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_1_1, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 2) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_1_2, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 3) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_1_3, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 4) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_1_4, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 1) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_2_1, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 2) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_2_2, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 3) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_2_3, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 4) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_2_4, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 1) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_3_1, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 2) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_3_2, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 3) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_3_3, ");
           // updateSqlPORemQty.Append(" SUM(CASE WHEN(ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 4) THEN KOUBAN_MINUS_NYUKO_QTY END) AS PO_REM_QTY_R_3_4 ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" A.MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" A.MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" B.YYYY, ");
           // updateSqlPORemQty.Append(" B.ROLL_R, ");
           // updateSqlPORemQty.Append(" B.CHANCE, ");
           // updateSqlPORemQty.Append(" SUM(B.KOUBAN_MINUS_NYUKO_QTY) KOUBAN_MINUS_NYUKO_QTY ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
           // //updateSqlPORemQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");

           // updateSqlPORemQty.Append(" ) A, ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" YYYY, ");
           // updateSqlPORemQty.Append(" ROLL_R, ");
           // updateSqlPORemQty.Append(" CHANCE, ");
           // updateSqlPORemQty.Append(" SUM(DECODE(CONF_QTY, NULL, 0, CONF_QTY) - DECODE(TOT_NYUKO, NULL, 0, TOT_NYUKO)) / 1000 ");
           // updateSqlPORemQty.Append(" KOUBAN_MINUS_NYUKO_QTY ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" ROLL_R, ");
           // updateSqlPORemQty.Append(" CHANCE, ");
           // updateSqlPORemQty.Append(" YYYY, ");
           // updateSqlPORemQty.Append(" CONF_QTY, ");
           // updateSqlPORemQty.Append(" VKD.MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" VKD.MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" (SELECT ");
           // updateSqlPORemQty.Append(" SUM(TKO.QUANTITY) ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" TRN_FUTOU_NYUKO TKO ");
           // updateSqlPORemQty.Append(" WHERE ");
           // updateSqlPORemQty.Append(" TKO.KOUBAN_CD = VKD.KOUBAN_CD ");
           // updateSqlPORemQty.Append(" ) TOT_NYUKO ");
           // updateSqlPORemQty.Append(" FROM ");
           // updateSqlPORemQty.Append(" V_KOUBAN_DETAILS   VKD ");
           // updateSqlPORemQty.Append(" JOIN TRN_PO_DTL         TPD ON VKD.PO_DTL_CD = TPD.PO_DTL_CD ");
           // updateSqlPORemQty.Append(" WHERE ");
           // updateSqlPORemQty.Append(" KOUBAN_COMPLETE_DATE IS NULL ");
           // updateSqlPORemQty.Append(" AND((ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') OR");
           // updateSqlPORemQty.Append(" (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') OR");
           // updateSqlPORemQty.Append(" (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "')) ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" YYYY, ");
           // updateSqlPORemQty.Append(" ROLL_R, ");
           // updateSqlPORemQty.Append(" CHANCE ");
           // updateSqlPORemQty.Append(" ) B ");
           // updateSqlPORemQty.Append(" WHERE ");
           // updateSqlPORemQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
           // updateSqlPORemQty.Append(" AND A.MUKESAKI_CD = B.MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" A.MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" A.MUKESAKI_CD, ");
           // updateSqlPORemQty.Append(" B.YYYY, ");
           // updateSqlPORemQty.Append(" B.ROLL_R, ");
           // updateSqlPORemQty.Append(" B.CHANCE ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" ) ");
           // updateSqlPORemQty.Append(" GROUP BY ");
           // updateSqlPORemQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
           // updateSqlPORemQty.Append(" MATERIAL_CD, ");
           // updateSqlPORemQty.Append(" MUKESAKI_CD ");
           // updateSqlPORemQty.Append(" ) U ");
           // updateSqlPORemQty.Append(" WHERE ");
           // updateSqlPORemQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ");
           // updateSqlPORemQty.Append(" AND T.MATERIAL_CD = U.MATERIAL_CD ");
           // updateSqlPORemQty.Append(" AND T.MUKESAKI_CD = U.MUKESAKI_CD)");               
            //updateSqlPORemQty.Append(" ) ");
            
            ExecuteStoredProcedurePoRemQtyLast3Months(Convert.ToInt32(rollList.Roll_RBeforeSixMonth), Convert.ToInt32(rollList.Roll_RYearBeforeSixMonth), Convert.ToInt32(rollList.Roll_RBeforeFiveMonth), Convert.ToInt32(rollList.Roll_RYearBeforeFiveMonth), Convert.ToInt32(rollList.Roll_RBeforeFourMonth), Convert.ToInt32(rollList.Roll_RYearBeforeFourMonth));


        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/14       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update PO_REM_QTY_R_1_1,PO_REM_QTY_R_1_2,PO_REM_QTY_R_1_3,PO_REM_QTY_R_1_4,PO_REM_QTY_R_2_1,PO_REM_QTY_R_2_2,PO_REM_QTY_R_2_3,PO_REM_QTY_R_2_4,PO_REM_QTY_R_3_1,
        /// PO_REM_QTY_R_3_2,PO_REM_QTY_R_3_3,PO_REM_QTY_R_3_4 in TEMP_INT_MATL_FC_SUMMARY table    
        /// </returns>
        /// 

        public void UpdateRollMatlSummaryWithPOPrvQtyAndPOQty(int roll_Year, int roll_R)
        {

            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            StringBuilder updateSqlPOPrvQtyAndPOQty = new StringBuilder();
            // updateSqlPOPrvQtyAndPOQty.Append(" UPDATE TEMP_ROLL_MATL_SUMMARY T ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SET (T.PO_PRV_QTY_R_1_1, T.PO_PRV_QTY_R_1_2, T.PO_PRV_QTY_R_1_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_PRV_QTY_R_1_4, T.PO_PRV_QTY_R_2_1, T.PO_PRV_QTY_R_2_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_PRV_QTY_R_2_3, T.PO_PRV_QTY_R_2_4, T.PO_PRV_QTY_R_3_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_PRV_QTY_R_3_2, T.PO_PRV_QTY_R_3_3, T.PO_PRV_QTY_R_3_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_QTY_R_1_1, T.PO_QTY_R_1_2, T.PO_QTY_R_1_3, T.PO_QTY_R_1_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_QTY_R_2_1, T.PO_QTY_R_2_2, T.PO_QTY_R_2_3, T.PO_QTY_R_2_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_QTY_R_3_1, T.PO_QTY_R_3_2, T.PO_QTY_R_3_3, T.PO_QTY_R_3_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_QTY_R_4_1, T.PO_QTY_R_4_2, T.PO_QTY_R_4_3, T.PO_QTY_R_4_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.PO_QTY_R_CM_1, T.PO_QTY_R_CM_2, T.PO_QTY_R_CM_3, T.PO_QTY_R_CM_4) ");
            // updateSqlPOPrvQtyAndPOQty.Append(" = (SELECT U.PO_PRV_QTY_R_1_1, U.PO_PRV_QTY_R_1_2, U.PO_PRV_QTY_R_1_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_PRV_QTY_R_1_4, U.PO_PRV_QTY_R_2_1, U.PO_PRV_QTY_R_2_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_PRV_QTY_R_2_3, U.PO_PRV_QTY_R_2_4, U.PO_PRV_QTY_R_3_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_PRV_QTY_R_3_2, U.PO_PRV_QTY_R_3_3, U.PO_PRV_QTY_R_3_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_1_1, U.PO_QTY_R_1_2, U.PO_QTY_R_1_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_1_4, U.PO_QTY_R_2_1, U.PO_QTY_R_2_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_2_3, U.PO_QTY_R_2_4, U.PO_QTY_R_3_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_3_2, U.PO_QTY_R_3_3, U.PO_QTY_R_3_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_4_1, U.PO_QTY_R_4_2, U.PO_QTY_R_4_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_4_4, U.PO_QTY_R_CM_1, U.PO_QTY_R_CM_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" U.PO_QTY_R_CM_3, U.PO_QTY_R_CM_4 ");
            // updateSqlPOPrvQtyAndPOQty.Append(" FROM ");
            // updateSqlPOPrvQtyAndPOQty.Append(" (SELECT ");
            // updateSqlPOPrvQtyAndPOQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MATERIAL_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MUKESAKI_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_PRV_QTY_R_1_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_PRV_QTY_R_1_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_PRV_QTY_R_1_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_PRV_QTY_R_1_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_PRV_QTY_R_2_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_PRV_QTY_R_2_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_PRV_QTY_R_2_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_PRV_QTY_R_2_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_PRV_QTY_R_3_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_PRV_QTY_R_3_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_PRV_QTY_R_3_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_PRV_QTY_R_3_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_QTY_R_1_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_QTY_R_1_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_QTY_R_1_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_QTY_R_1_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeTwoMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_QTY_R_2_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeTwoMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_QTY_R_2_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeTwoMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_QTY_R_2_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeTwoMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_QTY_R_2_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeOneMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_QTY_R_3_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeOneMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_QTY_R_3_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeOneMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_QTY_R_3_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeOneMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_QTY_R_3_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY = '" + rollList.Roll_YearCurrentMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_QTY_R_4_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY = '" + rollList.Roll_YearCurrentMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_QTY_R_4_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY = '" + rollList.Roll_YearCurrentMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_QTY_R_4_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY = '" + rollList.Roll_YearCurrentMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_QTY_R_4_4, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY = '" + rollList.Roll_YearAfterOneMonth + "' AND CHANCE = 1) THEN POQTY END) AS PO_QTY_R_CM_1, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY = '" + rollList.Roll_YearAfterOneMonth + "' AND CHANCE = 2) THEN POQTY END) AS PO_QTY_R_CM_2, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY = '" + rollList.Roll_YearAfterOneMonth + "' AND CHANCE = 3) THEN POQTY END) AS PO_QTY_R_CM_3, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" SUM(CASE WHEN (ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY = '" + rollList.Roll_YearAfterOneMonth + "' AND CHANCE = 4) THEN POQTY END) AS PO_QTY_R_CM_4 ");
            // updateSqlPOPrvQtyAndPOQty.Append(" FROM ");
            // updateSqlPOPrvQtyAndPOQty.Append(" (SELECT ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.MATERIAL_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.MUKESAKI_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" B.YYYY, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" B.ROLL_R, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" B.CHANCE, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" NVL(SUM(B.POQTY), 0) POQTY ");
            // updateSqlPOPrvQtyAndPOQty.Append(" FROM ");
            // updateSqlPOPrvQtyAndPOQty.Append(" (SELECT INT_MATL_FC_SUMMARY_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MATERIAL_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MUKESAKI_CD ");
            // updateSqlPOPrvQtyAndPOQty.Append(" FROM ");
            // updateSqlPOPrvQtyAndPOQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
            //// updateSqlPOPrvQtyAndPOQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");
            // updateSqlPOPrvQtyAndPOQty.Append(" ) A, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" (SELECT ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MATERIAL_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MUKESAKI_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" YYYY, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" ROLL_R, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" CHANCE, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" NVL(QTY, 0) AS POQTY ");
            // updateSqlPOPrvQtyAndPOQty.Append(" FROM TRN_PO_DTL ");
            // updateSqlPOPrvQtyAndPOQty.Append(" WHERE ");
            // updateSqlPOPrvQtyAndPOQty.Append(" INACTIVE_FLAG = 0 "); // change to PO_STATUS
            // updateSqlPOPrvQtyAndPOQty.Append(" AND ((ROLL_R = '" + rollList.Roll_RBeforeSixMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeSixMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RBeforeFiveMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFiveMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RBeforeFourMonth + "' AND YYYY = '" + rollList.Roll_RYearBeforeFourMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RBeforeThreeMonth + "' AND YYYY = '" + rollList.Roll_YearBeforeThreeMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RBeforeTwoMonth + "' AND YYYY =  '" + rollList.Roll_YearBeforeTwoMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RBeforeOneMonth + "' AND YYYY =  '" + rollList.Roll_YearBeforeOneMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RCurrentMonth + "' AND YYYY =  '" + rollList.Roll_YearCurrentMonth + "') ");
            // updateSqlPOPrvQtyAndPOQty.Append(" OR (ROLL_R = '" + rollList.Roll_RAfterOneMonth + "' AND YYYY =  '" + rollList.Roll_YearAfterOneMonth + "'))) B ");
            // updateSqlPOPrvQtyAndPOQty.Append(" WHERE ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
            // updateSqlPOPrvQtyAndPOQty.Append(" AND A.MUKESAKI_CD = B.MUKESAKI_CD ");
            // updateSqlPOPrvQtyAndPOQty.Append(" GROUP BY ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.MATERIAL_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" A.MUKESAKI_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" B.YYYY, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" B.ROLL_R, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" B.CHANCE) ");
            // updateSqlPOPrvQtyAndPOQty.Append(" GROUP BY ");
            // updateSqlPOPrvQtyAndPOQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MATERIAL_CD, ");
            // updateSqlPOPrvQtyAndPOQty.Append(" MUKESAKI_CD) U ");
            // updateSqlPOPrvQtyAndPOQty.Append(" WHERE ");
            // updateSqlPOPrvQtyAndPOQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ");
            // updateSqlPOPrvQtyAndPOQty.Append(" AND T.MATERIAL_CD = U.MATERIAL_CD ");
            // updateSqlPOPrvQtyAndPOQty.Append(" AND T.MUKESAKI_CD = U.MUKESAKI_CD )");

            //updateSqlPOPrvQtyAndPOQty.Append(" ) ");


           
            ExecuteStoredProcedurePOQty(Convert.ToInt32(rollList.Roll_RBeforeSixMonth), Convert.ToInt32(rollList.Roll_RYearBeforeSixMonth), Convert.ToInt32(rollList.Roll_RBeforeFiveMonth), Convert.ToInt32(rollList.Roll_RYearBeforeFiveMonth), Convert.ToInt32(rollList.Roll_RBeforeFourMonth), Convert.ToInt32(rollList.Roll_RYearBeforeFourMonth),
               Convert.ToInt32(rollList.Roll_RBeforeThreeMonth), Convert.ToInt32(rollList.Roll_YearBeforeThreeMonth), Convert.ToInt32(rollList.Roll_RBeforeTwoMonth), Convert.ToInt32(rollList.Roll_YearBeforeTwoMonth), Convert.ToInt32(rollList.Roll_RBeforeOneMonth), Convert.ToInt32(rollList.Roll_YearBeforeOneMonth),
              Convert.ToInt32(rollList.Roll_RCurrentMonth), Convert.ToInt32(rollList.Roll_YearCurrentMonth), Convert.ToInt32(rollList.Roll_RAfterOneMonth), Convert.ToInt32(rollList.Roll_YearAfterOneMonth));


        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/14       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update INT_STOCK_QTY in TEMP_ROLL_MATL_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateRollMatlSummaryWithIntStockQty()
        {
            StringBuilder updateSqlIntStockQty = new StringBuilder();


            //updateSqlIntStockQty.Append(" UPDATE TEMP_ROLL_MATL_SUMMARY T ");
            //updateSqlIntStockQty.Append(" SET ");
            //updateSqlIntStockQty.Append(" (T.INT_STOCK_QTY) = ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" U.INT_STOCK_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" FS.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" FS.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" FS.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" FACTORY_STOCK_QTY, ");
            //updateSqlIntStockQty.Append(" SHUKKO_QTY, ");
            //updateSqlIntStockQty.Append(" SHUKKA_QTY, ");
            //updateSqlIntStockQty.Append(" (NVL(SUM(FACTORY_STOCK_QTY), 0) - (NVL(SUM(SHUKKO_QTY), 0) + NVL(SUM(SHUKKA_QTY), 0)))AS INT_STOCK_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" N.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" N.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" N.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" NVL(NYUKA_QTY, 0) - NVL(ACT_ALLOC_QTY, 0) AS FACTORY_STOCK_QTY ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" SUM(B.NYUKA_QTY) AS NYUKA_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
            //updateSqlIntStockQty.Append(" ) A, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" SUM(QUANTITY) / 1000 NYUKA_QTY, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_MATL_NYUKO_NYUKA  TNKA ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS  VKD ON TNKA.KOUBAN_CD = VKD.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" AND NYUKO_NYUKA_KUBUN = 1 ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) B ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) N, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" SUM(B.ACT_ALLOC_QTY) AS ACT_ALLOC_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
            //updateSqlIntStockQty.Append(" ) A, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" SUM(ACT_ALLOC_QTY) / 1000 AS ACT_ALLOC_QTY, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_ACTUAL_ALLOCATION   AA ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VK ON AA.KOUBAN_CD = VK.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) B ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) ACT_ALLOC ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" N.INT_MATL_FC_SUMMARY_CD = ACT_ALLOC.INT_MATL_FC_SUMMARY_CD(+) ");
            //updateSqlIntStockQty.Append(" AND N.MATERIAL_CD = ACT_ALLOC.MATERIAL_CD(+) ");
            //updateSqlIntStockQty.Append(" ) FS, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" SUM(B.SHUKKO_QTY) AS SHUKKO_QTY ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
            //updateSqlIntStockQty.Append(" ) A, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" SUM(QUANTITY / 1000) SHUKKO_QTY, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_MATL_SHUKKO TS ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VK ON TS.KOUBAN_CD = VK.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) B ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) SHUKKO, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" SUM(B.SHUKKA_QTY) AS SHUKKA_QTY ");
            //updateSqlIntStockQty.Append(" FROM ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
            //updateSqlIntStockQty.Append(" ) A, ");
            //updateSqlIntStockQty.Append(" ( ");
            //updateSqlIntStockQty.Append(" SELECT ");
            //updateSqlIntStockQty.Append(" SUM(QTY / 1000) SHUKKA_QTY, ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" FROM ");
            //updateSqlIntStockQty.Append(" TRN_MATL_SHUKKA TS ");
            //updateSqlIntStockQty.Append(" JOIN V_KOUBAN_DETAILS   VK ON TS.KOUBAN_CD = VK.KOUBAN_CD ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" VK.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" ) B ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" A.MUKESAKI_CD ");
            //updateSqlIntStockQty.Append(" )SHUKKA ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" FS.INT_MATL_FC_SUMMARY_CD = SHUKKO.INT_MATL_FC_SUMMARY_CD(+) ");
            //updateSqlIntStockQty.Append(" AND FS.MATERIAL_CD = SHUKKO.MATERIAL_CD(+) ");
            //updateSqlIntStockQty.Append(" AND FS.INT_MATL_FC_SUMMARY_CD = SHUKKA.INT_MATL_FC_SUMMARY_CD(+) ");
            //updateSqlIntStockQty.Append(" AND FS.MATERIAL_CD = SHUKKA.MATERIAL_CD(+) ");
            //updateSqlIntStockQty.Append(" GROUP BY ");
            //updateSqlIntStockQty.Append(" FS.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlIntStockQty.Append(" FS.MATERIAL_CD, ");
            //updateSqlIntStockQty.Append(" FS.MUKESAKI_CD, ");
            //updateSqlIntStockQty.Append(" FACTORY_STOCK_QTY, ");
            //updateSqlIntStockQty.Append(" SHUKKO_QTY, ");
            //updateSqlIntStockQty.Append(" SHUKKA_QTY ");
            //updateSqlIntStockQty.Append(" ) U ");
            //updateSqlIntStockQty.Append(" WHERE ");
            //updateSqlIntStockQty.Append(" T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ");
            //updateSqlIntStockQty.Append(" AND T.MATERIAL_CD = U.MATERIAL_CD ");
            //updateSqlIntStockQty.Append(" AND T.MUKESAKI_CD = U.MUKESAKI_CD ) ");


            //ExecuteNonQuery(updateSqlIntStockQty.ToString());

            ExecuteStoredProcedureIntStockQty();


        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/04/14       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Update EXT_STOCK_QTY in TEMP_ROLL_MATL_SUMMARY table       
        /// </returns>
        /// 
        public void UpdateRollMatlSummaryWithExtStockQty()
        {
            StringBuilder updateSqlExtStockQty = new StringBuilder();

            //updateSqlExtStockQty.Append(" UPDATE TEMP_ROLL_MATL_SUMMARY T ");
            //updateSqlExtStockQty.Append(" SET (T.EXT_STOCK_QTY) = ( ");
            //updateSqlExtStockQty.Append(" SELECT U.EXT_STOCK_QTY ");
            //updateSqlExtStockQty.Append(" FROM ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" FUTOU_STOCK.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" FUTOU_STOCK.MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" FUTOU_STOCK.MUKESAKI_CD, ");
            //updateSqlExtStockQty.Append(" NVL(SUM(FUTOU_STOCK_QTY), 0) - NVL(SUM(FUTOU_SHUKKO_QTY), 0) AS EXT_STOCK_QTY ");
            //updateSqlExtStockQty.Append(" FROM ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" NYUKO.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" NYUKO.MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" NYUKO.MUKESAKI_CD, ");
            //updateSqlExtStockQty.Append(" NVL(NYUKO_QTY, 0) - NVL(NYUKA_QTY, 0) AS FUTOU_STOCK_QTY ");
            //updateSqlExtStockQty.Append(" FROM ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlExtStockQty.Append(" NVL(SUM(B.NYUKO_QTY), 0) AS NYUKO_QTY ");
            //updateSqlExtStockQty.Append(" FROM ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" FROM ");
            //updateSqlExtStockQty.Append(" TEMP_ROLL_MATL_SUMMARY ");
            ////updateSqlExtStockQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" ) A, ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" SUM(QUANTITY) / 1000 NYUKO_QTY, ");
            //updateSqlExtStockQty.Append(" MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" FROM TRN_FUTOU_NYUKO TNKO ");
            //updateSqlExtStockQty.Append(" JOIN V_KOUBAN_DETAILS VKD ON TNKO.KOUBAN_CD = VKD.KOUBAN_CD ");
            //updateSqlExtStockQty.Append(" WHERE VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlExtStockQty.Append(" GROUP BY MATERIAL_CD, MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) B ");
            //updateSqlExtStockQty.Append(" WHERE A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" GROUP BY A.INT_MATL_FC_SUMMARY_CD, A.MATERIAL_CD, A.MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) NYUKO, ");
            //updateSqlExtStockQty.Append(" ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlExtStockQty.Append(" NVL(SUM(B.NYUKA_QTY), 0) AS NYUKA_QTY ");
            //updateSqlExtStockQty.Append(" FROM ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" FROM ");
            //updateSqlExtStockQty.Append(" TEMP_ROLL_MATL_SUMMARY  ");
            ////updateSqlExtStockQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" ) A, ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" SUM(QUANTITY) / 1000 NYUKA_QTY, ");
            //updateSqlExtStockQty.Append(" MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" FROM TRN_MATL_NYUKO_NYUKA TNKA ");
            //updateSqlExtStockQty.Append(" JOIN V_KOUBAN_DETAILS VKD ON TNKA.KOUBAN_CD = VKD.KOUBAN_CD AND NYUKO_NYUKA_KUBUN = 1 ");
            //updateSqlExtStockQty.Append(" WHERE VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlExtStockQty.Append(" GROUP BY MATERIAL_CD, MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) B ");
            //updateSqlExtStockQty.Append(" WHERE A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" GROUP BY A.INT_MATL_FC_SUMMARY_CD, A.MATERIAL_CD, A.MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) NYUKA ");
            //updateSqlExtStockQty.Append(" WHERE NYUKO.INT_MATL_FC_SUMMARY_CD = NYUKA.INT_MATL_FC_SUMMARY_CD(+) ");
            //updateSqlExtStockQty.Append(" AND NYUKO.MATERIAL_CD = NYUKA.MATERIAL_CD(+) ");
            //updateSqlExtStockQty.Append(" ) FUTOU_STOCK, ");
            //updateSqlExtStockQty.Append(" ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" A.INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" A.MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" A.MUKESAKI_CD, ");
            //updateSqlExtStockQty.Append(" NVL(SUM(B.FUTOU_SHUKKO_QTY), 0) AS FUTOU_SHUKKO_QTY ");
            //updateSqlExtStockQty.Append(" FROM ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" INT_MATL_FC_SUMMARY_CD, ");
            //updateSqlExtStockQty.Append(" MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" FROM ");
            //updateSqlExtStockQty.Append(" TEMP_ROLL_MATL_SUMMARY  ");
            ////updateSqlExtStockQty.Append(" JOIN MST_MATERIAL MM ON RMS.MATERIAL_CD = MM.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" ) A, ( ");
            //updateSqlExtStockQty.Append(" SELECT ");
            //updateSqlExtStockQty.Append(" SUM(QUANTITY) / 1000 FUTOU_SHUKKO_QTY, ");
            //updateSqlExtStockQty.Append(" MATERIAL_CD, ");
            //updateSqlExtStockQty.Append(" MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" FROM TRN_FUTOU_SHUKKO TS ");
            //updateSqlExtStockQty.Append(" JOIN V_KOUBAN_DETAILS VKD ON TS.KOUBAN_CD = VKD.KOUBAN_CD ");
            //updateSqlExtStockQty.Append(" WHERE VKD.KOUBAN_COMPLETE_DATE IS NULL ");
            //updateSqlExtStockQty.Append(" GROUP BY MATERIAL_CD, MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) B ");
            //updateSqlExtStockQty.Append(" WHERE A.MATERIAL_CD = B.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" GROUP BY A.INT_MATL_FC_SUMMARY_CD, A.MATERIAL_CD, A.MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) FUTOU_SHUKKO ");
            //updateSqlExtStockQty.Append(" WHERE FUTOU_STOCK.INT_MATL_FC_SUMMARY_CD = FUTOU_SHUKKO.INT_MATL_FC_SUMMARY_CD(+) ");
            //updateSqlExtStockQty.Append(" AND FUTOU_STOCK.MATERIAL_CD = FUTOU_SHUKKO.MATERIAL_CD(+) ");
            //updateSqlExtStockQty.Append(" AND FUTOU_STOCK.MUKESAKI_CD = FUTOU_SHUKKO.MUKESAKI_CD(+) ");
            //updateSqlExtStockQty.Append(" GROUP BY FUTOU_STOCK.INT_MATL_FC_SUMMARY_CD, FUTOU_STOCK.MATERIAL_CD, FUTOU_STOCK.MUKESAKI_CD ");
            //updateSqlExtStockQty.Append(" ) U ");
            //updateSqlExtStockQty.Append(" WHERE T.INT_MATL_FC_SUMMARY_CD = U.INT_MATL_FC_SUMMARY_CD ");
            //updateSqlExtStockQty.Append(" AND T.MATERIAL_CD = U.MATERIAL_CD ");
            //updateSqlExtStockQty.Append(" AND T.MUKESAKI_CD = U.MUKESAKI_CD)");
            ////updateSqlExtStockQty.Append(" ) ");

            //ExecuteNonQuery(updateSqlExtStockQty.ToString());
            ExecuteStoredProcedureExtStockQty();

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/03/19
        /// Used Screen : RollPomainAnalysis
        /// </summary>
        /// <returns>
        /// 1.Fetch data from TEMP_INT_MATL_FC_SUMMARY to display data on roll po main grid
        /// </returns>
        ///        
        public List<RollPoMainAnalysisDetails> GetRollPOMainAnalysisGridData(List<SearchField> searchCriteria, short? chUnconfirmed, string sortCriteria, int roll_Year, int roll_R)
        {
            RollYearList rollList = new RollYearList(roll_Year, roll_R);

            List<RollPoMainAnalysisDetails> result = new List<RollPoMainAnalysisDetails>();
            StringBuilder selectGridData = new StringBuilder();
            selectGridData.Append(" SELECT ");
            selectGridData.Append(" * ");
            selectGridData.Append(" FROM ");
            selectGridData.Append(" ( ");
            selectGridData.Append(" SELECT ");
            selectGridData.Append(" FC_SUMMARY_DATA.*, ");
            selectGridData.Append(" ROWNUM RN ");
            selectGridData.Append(" FROM ");
            selectGridData.Append(" ( ");
            selectGridData.Append(" (SELECT ");
            selectGridData.Append(" 1 AS srno, ");

            selectGridData.Append(" INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectGridData.Append(" DIM_D,");
            selectGridData.Append(" SHAPE,");
            selectGridData.Append(" LENGTH,");
            selectGridData.Append(" MATL_KUBUN,");

            selectGridData.Append(" MATL_SPECIAL_KUBUN,");
            if (conditionPOMain == true)
            {
                selectGridData.Append(" CONDITION_DESCRIPTION,");
            }

            selectGridData.Append(" PO_REVIEWED_DATE,");
            selectGridData.Append(" employee_cd,");

            selectGridData.Append(" DECODE(SIGN(REM_QTY_4MONTH_AGO), 0, '', -1, '', REM_QTY_4MONTH_AGO) REM_QTY_4MONTH_AGO,");

            selectGridData.Append(" DECODE(PO_QTY_1MONTH_AGO, 0, '', PO_QTY_1MONTH_AGO) PO_QTY_1MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(REM_QTY_1MONTH_AGO), 0, '', -1, '', REM_QTY_1MONTH_AGO) REM_QTY_1MONTH_AGO,");
            selectGridData.Append(" DECODE(RECVD_QTY_1MONTH_AGO, 0, '', RECVD_QTY_1MONTH_AGO) RECVD_QTY_1MONTH_AGO,");
            selectGridData.Append(" DECODE(PO_QTY_2MONTH_AGO, 0, '', PO_QTY_2MONTH_AGO) PO_QTY_2MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(REM_QTY_2MONTH_AGO), 0, '', -1, '', REM_QTY_2MONTH_AGO) REM_QTY_2MONTH_AGO,");
            selectGridData.Append(" DECODE(RECVD_QTY_2MONTH_AGO, 0, '', RECVD_QTY_2MONTH_AGO) RECVD_QTY_2MONTH_AGO,");
            selectGridData.Append(" DECODE(PO_QTY_3MONTH_AGO, 0, '', PO_QTY_3MONTH_AGO) PO_QTY_3MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(REM_QTY_3MONTH_AGO), 0, '', -1, '', REM_QTY_3MONTH_AGO) REM_QTY_3MONTH_AGO,");
            selectGridData.Append(" DECODE(RECVD_QTY_3MONTH_AGO, 0, '', RECVD_QTY_3MONTH_AGO) RECVD_QTY_3MONTH_AGO, ");
            selectGridData.Append(" DECODE((NVL(PO_QTY_1MONTH_AGO, 0) + NVL(PO_QTY_2MONTH_AGO, 0) + NVL(PO_QTY_3MONTH_AGO, 0)), 0, '', (NVL(PO_QTY_1MONTH_AGO, 0) + ");
            selectGridData.Append(" NVL(PO_QTY_2MONTH_AGO, 0) + NVL(PO_QTY_3MONTH_AGO, 0))) TOT_PO_QTY_MONTH_AGO, ");

            selectGridData.Append(" DECODE(SIGN(NVL(REM_QTY_1MONTH_AGO, 0) + NVL(REM_QTY_2MONTH_AGO, 0) + NVL(REM_QTY_3MONTH_AGO, 0)), 0, '', -1, '',");
            selectGridData.Append(" (NVL(REM_QTY_1MONTH_AGO, 0) + NVL(REM_QTY_2MONTH_AGO, 0) + NVL(REM_QTY_3MONTH_AGO, 0))) TOT_REM_QTY_MONTH_AGO ,");

            selectGridData.Append(" DECODE((NVL(RECVD_QTY_1MONTH_AGO, 0) + NVL(RECVD_QTY_2MONTH_AGO, 0) + NVL(RECVD_QTY_3MONTH_AGO, 0)), 0, '', (NVL(RECVD_QTY_1MONTH_AGO, 0) + NVL(RECVD_QTY_2MONTH_AGO, 0) + ");
            selectGridData.Append(" NVL(RECVD_QTY_3MONTH_AGO, 0))) TOT_RECVD_QTY_MONTH_AGO,");

            selectGridData.Append(" DECODE(PO_QTY_THIS_MONTH, 0, '', PO_QTY_THIS_MONTH) PO_QTY_THIS_MONTH,");

            selectGridData.Append(" SALES_FC_QTY_THIS_MONTH,");
            selectGridData.Append(" DECODE(PO_QTY_1MONTH_AHEAD, 0, '', PO_QTY_1MONTH_AHEAD) PO_QTY_1MONTH_AHEAD,");

            selectGridData.Append(" SALES_FC_QTY_1MONTH_AHEAD,");
            selectGridData.Append(" DECODE(PO_QTY_2MONTH_AHEAD, 0, '', PO_QTY_2MONTH_AHEAD) PO_QTY_2MONTH_AHEAD,");

            selectGridData.Append(" SALES_FC_QTY_2MONTH_AHEAD,");
            selectGridData.Append(" DECODE(PO_QTY_3MONTH_AHEAD, 0, '', PO_QTY_3MONTH_AHEAD) PO_QTY_3MONTH_AHEAD,");

            selectGridData.Append(" SALES_FC_QTY_3MONTH_AHEAD,");
            selectGridData.Append(" DECODE(PO_QTY_4MONTH_AHEAD, 0, '', PO_QTY_4MONTH_AHEAD) PO_QTY_4MONTH_AHEAD,");

            selectGridData.Append(" DECODE(EXT_STOCK_QTY, 0, '', EXT_STOCK_QTY) EXT_STOCK_QTY,");
            selectGridData.Append(" INT_MATL_FC_SUMMARY_CD,");

            selectGridData.Append(" DECODE(INT_STOCK_QTY, 0, '', INT_STOCK_QTY) INT_STOCK_QTY,");
            selectGridData.Append(" DECODE(NVL(EXT_STOCK_QTY, 0) + NVL(INT_STOCK_QTY, 0), 0, '', NVL(EXT_STOCK_QTY, 0) + NVL(INT_STOCK_QTY, 0)) TOT_STOCK_QTY,");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearBeforeThreeMonth + "', '" + rollList.Roll_RBeforeThreeMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_THIS_MONTH, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearBeforeTwoMonth + "', '" + rollList.Roll_RBeforeTwoMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_1MONTH_AHEAD, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearBeforeOneMonth + "', '" + rollList.Roll_RBeforeOneMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_2MONTH_AHEAD, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearCurrentMonth + "', '" + rollList.Roll_RCurrentMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_3MONTH_AHEAD, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearAfterOneMonth + "', '" + rollList.Roll_RAfterOneMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_4MONTH_AHEAD ");

            selectGridData.Append(" FROM");
            selectGridData.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS");
            selectGridData.Append(" INNER JOIN MST_INTERNAL_NAME MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            if (conditionPOMain == true)
            {
                selectGridData.Append(" LEFT JOIN MST_CONDITION  COND ON FCS.CONDITION_CD = COND.CONDITION_CD");
            }

            selectGridData.Append(" WHERE");
            selectGridData.Append(" MATL_KUBUN IN (1)");
            selectGridData.Append(" UNION ALL");
            selectGridData.Append(" SELECT");
            selectGridData.Append(" 2 AS SRNO,");

            selectGridData.Append(" INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectGridData.Append(" DIM_D,");
            selectGridData.Append(" SHAPE,");
            selectGridData.Append(" LENGTH,");
            selectGridData.Append(" MATL_KUBUN,");

            selectGridData.Append(" MATL_SPECIAL_KUBUN,");
            if (conditionPOMain == true)
            {
                selectGridData.Append(" CONDITION_DESCRIPTION,");
            }

            selectGridData.Append(" PO_REVIEWED_DATE,");
            selectGridData.Append(" employee_cd,");

            selectGridData.Append(" DECODE(SIGN(REM_QTY_4MONTH_AGO), 0, '', -1, '', REM_QTY_4MONTH_AGO) REM_QTY_4MONTH_AGO,");

            selectGridData.Append(" DECODE(PO_QTY_1MONTH_AGO, 0, '', PO_QTY_1MONTH_AGO) PO_QTY_1MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(REM_QTY_1MONTH_AGO), 0, '', -1, '', REM_QTY_1MONTH_AGO) REM_QTY_1MONTH_AGO,");
            selectGridData.Append(" DECODE(RECVD_QTY_1MONTH_AGO, 0, '', RECVD_QTY_1MONTH_AGO) RECVD_QTY_1MONTH_AGO,");
            selectGridData.Append(" DECODE(PO_QTY_2MONTH_AGO, 0, '', PO_QTY_2MONTH_AGO) PO_QTY_2MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(REM_QTY_2MONTH_AGO), 0, '', -1, '', REM_QTY_2MONTH_AGO) REM_QTY_2MONTH_AGO,");
            selectGridData.Append(" DECODE(RECVD_QTY_2MONTH_AGO, 0, '', RECVD_QTY_2MONTH_AGO) RECVD_QTY_2MONTH_AGO,");
            selectGridData.Append(" DECODE(PO_QTY_3MONTH_AGO, 0, '', PO_QTY_3MONTH_AGO) PO_QTY_3MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(REM_QTY_3MONTH_AGO), 0, '', -1, '', REM_QTY_3MONTH_AGO) REM_QTY_3MONTH_AGO,");
            selectGridData.Append(" DECODE(RECVD_QTY_3MONTH_AGO, 0, '', RECVD_QTY_3MONTH_AGO) RECVD_QTY_3MONTH_AGO,");

            selectGridData.Append(" DECODE((NVL(PO_QTY_1MONTH_AGO, 0) + NVL(PO_QTY_2MONTH_AGO, 0) + NVL(PO_QTY_3MONTH_AGO, 0)), 0, '', (NVL(PO_QTY_1MONTH_AGO, 0) +");
            selectGridData.Append(" NVL(PO_QTY_2MONTH_AGO, 0) + NVL(PO_QTY_3MONTH_AGO, 0))) TOT_PO_QTY_MONTH_AGO,");
            selectGridData.Append(" DECODE(SIGN(NVL(REM_QTY_1MONTH_AGO, 0) + NVL(REM_QTY_2MONTH_AGO, 0) + NVL(REM_QTY_3MONTH_AGO, 0)), 0, '', -1, '',");
            selectGridData.Append(" (NVL(REM_QTY_1MONTH_AGO, 0) + NVL(REM_QTY_2MONTH_AGO, 0) + NVL(REM_QTY_3MONTH_AGO, 0))) TOT_REM_QTY_MONTH_AGO ,");
            selectGridData.Append(" DECODE((NVL(RECVD_QTY_1MONTH_AGO, 0) + NVL(RECVD_QTY_2MONTH_AGO, 0) + NVL(RECVD_QTY_3MONTH_AGO, 0)), 0, '', (NVL(RECVD_QTY_1MONTH_AGO, 0) + NVL(RECVD_QTY_2MONTH_AGO, 0) +");
            selectGridData.Append(" NVL(RECVD_QTY_3MONTH_AGO, 0))) TOT_RECVD_QTY_MONTH_AGO,");
            selectGridData.Append(" DECODE(PO_QTY_THIS_MONTH, 0, '', PO_QTY_THIS_MONTH) PO_QTY_THIS_MONTH,");

            selectGridData.Append(" SALES_FC_QTY_THIS_MONTH,");
            selectGridData.Append(" DECODE(PO_QTY_1MONTH_AHEAD, 0, '', PO_QTY_1MONTH_AHEAD) PO_QTY_1MONTH_AHEAD,");

            selectGridData.Append(" SALES_FC_QTY_1MONTH_AHEAD,");
            selectGridData.Append(" DECODE(PO_QTY_2MONTH_AHEAD, 0, '', PO_QTY_2MONTH_AHEAD) PO_QTY_2MONTH_AHEAD,");

            selectGridData.Append(" SALES_FC_QTY_2MONTH_AHEAD,");
            selectGridData.Append(" DECODE(PO_QTY_3MONTH_AHEAD, 0, '', PO_QTY_3MONTH_AHEAD) PO_QTY_3MONTH_AHEAD,");

            selectGridData.Append(" SALES_FC_QTY_3MONTH_AHEAD,");
            selectGridData.Append(" DECODE(PO_QTY_4MONTH_AHEAD, 0, '', PO_QTY_4MONTH_AHEAD) PO_QTY_4MONTH_AHEAD,");


            selectGridData.Append(" DECODE(EXT_STOCK_QTY, 0, '', EXT_STOCK_QTY) EXT_STOCK_QTY,");
            selectGridData.Append(" INT_MATL_FC_SUMMARY_CD,");

            selectGridData.Append(" DECODE(INT_STOCK_QTY, 0, '', INT_STOCK_QTY) INT_STOCK_QTY,");
            selectGridData.Append(" DECODE(NVL(EXT_STOCK_QTY, 0) + NVL(INT_STOCK_QTY, 0), 0, '', NVL(EXT_STOCK_QTY, 0) + NVL(INT_STOCK_QTY, 0)) TOT_STOCK_QTY,");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearBeforeThreeMonth + "', '" + rollList.Roll_RBeforeThreeMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_THIS_MONTH, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearBeforeTwoMonth + "', '" + rollList.Roll_RBeforeTwoMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_1MONTH_AHEAD, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearBeforeOneMonth + "', '" + rollList.Roll_RBeforeOneMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_2MONTH_AHEAD, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearCurrentMonth + "', '" + rollList.Roll_RCurrentMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_3MONTH_AHEAD, ");
            selectGridData.Append(" GET_QTY_BY_ROLL('" + rollList.Roll_YearAfterOneMonth + "', '" + rollList.Roll_RAfterOneMonth + "', FCS.INT_MATL_FC_SUMMARY_CD) AS PLAN_QTY_4MONTH_AHEAD ");
            selectGridData.Append(" FROM");
            selectGridData.Append(" TEMP_INT_MATL_FC_SUMMARY   FCS");
            selectGridData.Append(" INNER JOIN MST_INTERNAL_NAME MI ON FCS.INTERNAL_NAME_CD = MI.INTERNAL_NAME_CD ");
            if (conditionPOMain == true)
            {
                selectGridData.Append(" LEFT JOIN MST_CONDITION COND ON FCS.CONDITION_CD = COND.CONDITION_CD");
            }

            selectGridData.Append(" WHERE");
            selectGridData.Append(" MATL_KUBUN IN(");
            selectGridData.Append(" 2,");
            selectGridData.Append(" 3");
            selectGridData.Append(" )");
            selectGridData.Append(" )");
            //selectGridData.Append(" ORDER BY");
            //selectGridData.Append(" SRNO,");
            //selectGridData.Append(" INTERNAL_NAME_STRING,");
            //selectGridData.Append(" DIM_D,");
            //selectGridData.Append(" SHAPE,");
            //selectGridData.Append(" LENGTH,");
            //selectGridData.Append(" MATL_SPECIAL_KUBUN");
            selectGridData.Append(" ) FC_SUMMARY_DATA");

            selectGridData.Append(")");
            var query = selectGridData.ToString();

            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition;
                if (chUnconfirmed == 1)
                {
                    whereCondition = "WHERE PO_REVIEWED_DATE IS NULL AND ";
                }
                else
                {
                    whereCondition = " WHERE ";
                }
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }
          
            if (!string.IsNullOrWhiteSpace(sortCriteria))
            {
                // Remove CONDITION_DESCRIPTION when flag is false
                if (!conditionPOMain && sortCriteria.Contains("CONDITION_DESCRIPTION"))
                {
                    sortCriteria = string.Join(", ",
                        sortCriteria
                            .Split(',')
                            .Select(x => x.Trim())
                            .Where(x => !x.StartsWith("CONDITION_DESCRIPTION",
                                        StringComparison.OrdinalIgnoreCase))
                    );
                }

                // Apply remaining sort criteria
                if (!string.IsNullOrWhiteSpace(sortCriteria))
                {
                    query = Helper.ApplySortCriteria(sortCriteria, query);
                }
               
            }
            else
            {
                query += " ORDER BY INTERNAL_NAME_STRING, DIM_D, SHAPE, LENGTH, MATL_KUBUN, MATL_SPECIAL_KUBUN";

                if (conditionPOMain == true)
                {
                    query += ", CONDITION_DESCRIPTION";
                }
            }

            selectGridData = new StringBuilder();
            selectGridData.Append(query);

            result = ExecuteQueryMultiResult<RollPoMainAnalysisDetails>(query);
            return result;
        }

        /// <summary>
        /// CREATED BY :Pallavi Gawade on 2025/03/04
        /// Used Screen : RollPomainAnalysis
        /// </summary>
        /// <param name="employeeCD"></param>
        /// <returns>
        /// 1.Get location name by employee cd
        /// </returns>
        public string GetLocationNameByEmployeeCD(int employeeCD)
        {
            string locationname;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("ML.SHORT_NAME ");
            selectQuery.Append("FROM ");
            selectQuery.Append("MST_LOCATION ML ");
            selectQuery.Append("INNER JOIN MST_DEPARTMENT_GROUP MD ON MD.LOCATION_CD = ML.LOCATION_CD ");
            selectQuery.Append("INNER JOIN MST_EMPLOYEE ME ON ME.EMPLOYEE_CD = MD.EMPLOYEE_CD ");
            selectQuery.Append("WHERE ");
            selectQuery.Append("ME.EMPLOYEE_CD = '" + employeeCD + "' ");
            selectQuery.Append("AND ROWNUM = 1 ");

            locationname = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            return locationname;
        }

        public void ImportBatchData()
        {

            prodMaster = new List<PRODUCT_MASTER_HOST>();

            StringBuilder selectProdMaster = new StringBuilder();
            selectProdMaster.Append(" SELECT ");
            selectProdMaster.Append(" 製造不変番号 AS MFHNNO, ");
            selectProdMaster.Append(" 製仕様番号１ AS PROD_SPEC_1, ");
            selectProdMaster.Append(" 製仕様番号２ AS PROD_SPEC_2, ");
            selectProdMaster.Append(" 製鋼種 AS MHINME, ");
            selectProdMaster.Append(" 製社内規格   AS MSYAKI, ");
            selectProdMaster.Append(" 製コード AS MHINCD, ");
            selectProdMaster.Append(" 製寸法 AS MSNPO1, ");
            selectProdMaster.Append(" 製形 AS MKATA1, ");
            selectProdMaster.Append(" 得意先コード AS MKYCD1, ");
            selectProdMaster.Append(" 製寸法公差 AS MSNPKS, ");
            selectProdMaster.Append(" 旧製長さ AS MNAGAS, ");
            selectProdMaster.Append(" 製納入状態 AS MJYTAI ");
            selectProdMaster.Append(" FROM SEIZOCSV ");
            prodMaster = ExecuteQueryMultiResult<PRODUCT_MASTER_HOST>(selectProdMaster.ToString());// PRODUCT MASTER DATA SELECTED

            //var prodStockDetails = GetProdStockBatchData();

            //var khakoraiDetails = GetKhakoraiBatchData();

            var shukkaDetails = GetProdShukkaData();

            using (TransactionScope transaction = new TransactionScope())
            {
                //if (prodStockDetails.Count > 0)
                //{
                //    ImportDataProdStock(prodStockDetails);
                //}

                //if (khakoraiDetails.Count > 0)
                //{
                //    ImportDataKhakorai(khakoraiDetails);
                //}

                transaction.Complete();
                transaction.Dispose();
            }



        }


        /// <summary>
        /// CREATED BY : RAHUL SUTAR on 2025/02/13       
        /// USED SCREENS: ROLLPOMAINANALYSIS
        /// FLAG CODE = 0 FOR SHAPE S, P AND F
        /// FLAG CODE = 1 FOR PRODUCT_CD, MHINME, MSYAKI, MHINCD, MKATA1, MKYCD1, MNAGAS AND MJYTAI
        /// FLAG CODE = 2 FOR MHINME, MSYAKI, MSNPO1, MKATA1, MSNPKS, MJYTAI, MKYCD1 AND MNAGAS  
        /// FLAG CODE = 3 FOR MHINME, MSYAKI, MSNPO1, MKATA1,  MJYTAI, MKYCD1 AND MNAGAS 
        /// FLAG CODE = 4 FOR MHINME, MSYAKI, MSNPO1, MKATA1, MKYCD1 AND MNAGAS 
        /// FLAG CODE = 5 FOR MHINME, MSYAKI, MSNPO1, MKATA1 AND MNAGAS 
        /// FLAG CODE = 6 FOR MSYAKI, MSNPO1, MKATA1 AND MNAGAS 
        /// FLAG CODE = 7 FOR MSYAKI, MKATA1 AND MNAGAS
        /// FLAG CODE = 8 FOR MSYAKI  AND MKATA1
        /// FLAG CODE = 98 FOR B, C, ｺｸｲﾝ, ｻｲｶｺ, ｾﾂﾀﾝ, ｿｻﾞｲ
        /// FLAG CODE = 99 FOR NOT MATCHING DATA
        /// </summary>
        /// <returns>
        /// IMPORT DATA IN BCH_PROD_STOCK
        /// </returns>
        public List<BCH_PROD_STOCK> GetProdStockBatchData()
        {
            List<BCH_PROD_STOCK> prodDetails = new List<BCH_PROD_STOCK>();

            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  * FROM ( ");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" NVL(MNY.MDENBN,'NO_NUMBER') AS SLIP_NUMBER, ");
            selectQuery.Append(" MNY.MDENBI AS NYUKA_DATE, ");
            selectQuery.Append(" MNY.MHINME AS PRODUCT_NAME, ");
            selectQuery.Append(" MNY.MSYAKI AS COMPANY_STANDARD, ");
            selectQuery.Append(" MNY.MHINCD AS STOCK_CD, ");
            selectQuery.Append(" MNY.MSNPO1 AS DIM_D, ");
            selectQuery.Append(" MNY.MKATA1 AS PROD_SHAPE, ");
            selectQuery.Append(" MNY.MKYCD1 AS ALLOC_CD, ");
            selectQuery.Append(" MNY.MZAICD AS CURRENT_CD, ");
            selectQuery.Append(" MNY.MSNPKS AS DIM_TOL, ");
            selectQuery.Append(" MNY.MIKEI1 AS DIM_OTHER, ");
            selectQuery.Append(" MNY.MNAGAS AS PRODUCT_LENGTH, ");
            selectQuery.Append(" MNY.MJYTAI AS CONDITION, ");
            selectQuery.Append(" MNY.MZAIKO AS PROD_STOCK_QTY, ");
            selectQuery.Append(" MNY.MDENSY AS NYUKA_DATE_YEAR, ");
            selectQuery.Append(" MNY.MFHNNO AS PRODUCT_CD, ");
            selectQuery.Append(" SEZ.製仕様番号１ AS PROD_SPEC_1, ");
            selectQuery.Append(" SEZ.製仕様番号２ AS PROD_SPEC_2, ");

            selectQuery.Append(" CASE WHEN MNY.MKATA1 = 'S' OR MNY.MKATA1 = 'P' OR MNY.MKATA1 = 'F' THEN 0 ");
            selectQuery.Append(" WHEN SEZ.製仕様番号１ IS NOT NULL THEN 1 ELSE NULL END AS FLAG_CODE, ");
            selectQuery.Append(" MNY.SRNO, ");
            selectQuery.Append(" NULL AS DUPLICATE_RECORDS ");
            selectQuery.Append(" FROM MNYUKTRF MNY ");
            selectQuery.Append(" LEFT JOIN SEIZOCSV SEZ ");
            selectQuery.Append(" ON MNY.MFHNNO  = SEZ.製造不変番号 AND ");
            selectQuery.Append(" MNY.MHINME  = SEZ.製鋼種 AND ");
            selectQuery.Append(" MNY.MSYAKI  = SEZ.製社内規格 AND ");
            selectQuery.Append(" MNY.MHINCD  = SEZ.製コード AND ");
            selectQuery.Append(" MNY.MKATA1  = SEZ.製形 AND ");
            selectQuery.Append(" MNY.MKYCD1  = SEZ.得意先コード AND ");
            selectQuery.Append(" MNY.MNAGAS  = SEZ.旧製長さ AND ");
            selectQuery.Append(" MNY.MJYTAI  = SEZ.製納入状態  ");
            selectQuery.Append(" )  ");
            var result = ExecuteQueryMultiResult<BCH_PROD_STOCK>(selectQuery.ToString());

            var result1 = result.AsEnumerable().Where(r => r.PROD_SPEC_1 != null || r.PROD_SHAPE == "S" || r.PROD_SHAPE == "P" || r.PROD_SHAPE == "F").Select(r => r).ToList(); //PROD SPEC 1 AND 2 NOT NULL AND SHAPE S, P, AND F DATA TAKEN IN RESULT1


            //result1.AsEnumerable().Where(r => r.PROD_SHAPE == "S" && r.PROD_SHAPE == "P" && r.PROD_SHAPE == "F").ToList().ForEach(i => { i.PRODUCT_CD = string.Empty; i.PROD_SPEC_1 = string.Empty; i.PROD_SPEC_2 = string.Empty; });  //FOR SHAPE S, P, F PRODUCT_CD, SPEC 1 AND SPEC 2 SET EMPTY
            var result2 = result.AsEnumerable().Where(r => r.PROD_SPEC_1 == null && !r.PROD_SHAPE.Equals("S") && !r.PROD_SHAPE.Equals("P") && !r.PROD_SHAPE.Equals("F")).Select(r => r).ToList(); //PROD SPEC 1 AND 2 IS NULL AND  SHAPE S, P, AND F NOT EXIST DATA TAKEN IN RESULT2

            foreach (var item in result2)
            {
                List<PRODUCT_MASTER_HOST> listData = new List<PRODUCT_MASTER_HOST>();
                string prodCD = item.PRODUCT_CD;
                string prodName = item.PRODUCT_NAME; //MHINME
                string comStandard = item.COMPANY_STANDARD; //MSYAKI
                decimal? dimD = item.DIM_D; //MSNPO1
                string shape = item.PROD_SHAPE; // MKATA1
                string dimTol = item.DIM_TOL; //MSNPKS
                string condition = item.CONDITION; //MJYTAI
                string allocCD = item.ALLOC_CD; //MKYCD1
                string prodLength = item.PRODUCT_LENGTH; //MNAGAS
                int? flagCode = null;


                listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                && r.MKATA1 == shape && r.MSNPKS == dimTol && r.MJYTAI == condition && r.MKYCD1 == allocCD && r.MNAGAS == prodLength).ToList(); //FLAG CODE 2 CHECKED
                if (listData.Count > 0)
                {
                    flagCode = 2;
                }

                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MJYTAI == condition && r.MKYCD1 == allocCD && r.MNAGAS == prodLength).ToList(); //FLAG CODE 3 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 3;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MKYCD1 == allocCD && r.MNAGAS == prodLength).ToList(); //FLAG CODE 4 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 4;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MNAGAS == prodLength).ToList(); //FLAG CODE 5 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 5;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MNAGAS == prodLength).ToList(); //FLAG CODE 6 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 6;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MSYAKI == comStandard && r.MKATA1 == shape && r.MNAGAS == prodLength).ToList(); //FLAG CODE 7 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 7;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MSYAKI == comStandard && r.MKATA1 == shape).ToList(); //FLAG CODE 8 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 8;
                    }
                    //var resultData = dt.AsEnumerable().Select(r => r).Where(r => r.Field<string>("MSYAKI") == comStandard && r.Field<string>("MKATA1") == shape).ToList(); //FLAG CODE 8 CHECKED

                    // if (resultData.Any())
                    // {
                    //     listData = resultData.CopyToDataTable();

                    // }

                }

                if (prodCD != string.Empty && listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MFHNNO == prodCD).ToList(); //DATA CHECKED WITH PRODUCT CD
                }
                if (listData.Count > 0)
                {
                    if (listData[0].PROD_SPEC_1 == "B" || listData[0].PROD_SPEC_1 == "C" || listData[0].PROD_SPEC_1 == "ｺｸｲﾝ" || listData[0].PROD_SPEC_1 == "ｻｲｶｺ" || listData[0].PROD_SPEC_1 == "ｿｻﾞｲ")
                    {
                        item.FLAG_CODE = 98;
                        item.DUPLICATE_RECORDS = null;
                        item.PROD_SPEC_1 = listData[0].PROD_SPEC_1;
                        item.PROD_SPEC_2 = listData[0].PROD_SPEC_2;
                        item.PRODUCT_CD = listData[0].MFHNNO;

                    }
                    else
                    {
                        item.FLAG_CODE = flagCode;
                        item.DUPLICATE_RECORDS = listData.Count;
                        item.PROD_SPEC_1 = listData[0].PROD_SPEC_1;
                        item.PROD_SPEC_2 = listData[0].PROD_SPEC_2;
                        item.PRODUCT_CD = listData[0].MFHNNO;
                    }
                }
            }
            prodDetails.AddRange(result1);

            result2.AsEnumerable().Where(r => r.FLAG_CODE == null && r.PROD_SPEC_1 == null && r.PROD_SPEC_2 == null).ToList().ForEach(i => i.FLAG_CODE = 99);  //FLAG CODE 99 SET FOR ALL NOT MATCHING DATA
            prodDetails.AddRange(result2);

            return prodDetails;


        }

        public List<BCH_KHAKORAI> GetKhakoraiBatchData()
        {
            List<BCH_KHAKORAI> khakoraiDetails = new List<BCH_KHAKORAI>();

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append(" KAKO.PCHNO1 AS CHUUBAN, ");
            selectQuery.Append(" KAKO.PCHNO2 AS GYOUBAN, ");
            selectQuery.Append(" KAKO.PSPEC1 AS PROD_SPEC_NO1, ");
            selectQuery.Append(" KAKO.PSPEC2 AS PROD_SPEC_NO2, ");
            selectQuery.Append(" KAKO.PKAKJR AS KAKOUIARI_QTY, ");
            selectQuery.Append(" KAKO.PHKIJR AS ACT_ALLOC_QTY, ");
            selectQuery.Append(" KAKO.PHKZJR AS ACT_ALLOC_WEIGHT, ");
            selectQuery.Append(" KAKO.PHINUJ AS ACT_SHUKKA_WEIGHT, ");
            selectQuery.Append(" KAKO.PSEHIJ AS PROD_WEIGHT, ");
            selectQuery.Append(" KAKO.PFHNNO AS PRODUCT_CD ");
            selectQuery.Append(" FROM ");

            selectQuery.Append(" SKAKOTRF KAKO ");
            selectQuery.Append(" INNER JOIN SEIZOCSV SEZ ON KAKO.PFHNNO = SEZ.製造不変番号 ");
            khakoraiDetails = ExecuteQueryMultiResult<BCH_KHAKORAI>(selectQuery.ToString());
            return khakoraiDetails;
        }
        public List<BCH_PROD_SHUKKA> GetProdShukkaData()
        {
            List<BCH_PROD_SHUKKA> shukkaDetails = new List<BCH_PROD_SHUKKA>();

            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  * FROM ( ");
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" SH.PSYORI AS PROCESS_KUBUN, ");
            selectQuery.Append(" SH.PDENBN AS SLIP_NUMBER, ");
            selectQuery.Append(" SH.PDENGY AS VOUCHER_NO, ");
            selectQuery.Append(" SH.PKUBUN AS SHUKKO_KUBUN, ");
            selectQuery.Append(" SH.PDENBI AS SHUKKA_DATE, ");
            selectQuery.Append(" SH.PHINME AS PRODUCT_NAME, ");
            selectQuery.Append(" SH.PSYAKI AS COMPANY_STANDARD, ");
            selectQuery.Append(" SH.PHINCD AS PRODUCT_CODE, ");
            selectQuery.Append(" SH.PSNPO1 AS DIM_D, ");
            selectQuery.Append(" SH.PKATA1 AS SHAPE, ");
            selectQuery.Append(" SH.PKYCD1 AS ALLOC_CD, ");
            selectQuery.Append(" SH.PSKCD1 AS SHUKKA_CD, ");
            selectQuery.Append(" SH.PTDCD1 AS RECEIVER_CD, ");
            selectQuery.Append(" SH.PSNPKS AS DIM_TOL, ");
            selectQuery.Append(" SH.PIKEI1 AS DIM_OTHER, ");
            selectQuery.Append(" SH.PNAGAS AS PROD_LENGTH, ");
            selectQuery.Append(" SH.PJYTAI AS CONDITION, ");
            selectQuery.Append(" SH.PCHAJI AS CHARGE, ");
            selectQuery.Append(" SH.PSYUKA AS SHUKKA_QTY, ");
            selectQuery.Append(" SH.PINZU1 AS NO_OF_UNITS, ");
            selectQuery.Append(" SH.PTANI1 AS UNIT, ");
            selectQuery.Append(" SH.PDENSY AS SHUKKA_YEAR, ");
            selectQuery.Append(" SH.PFHNNO AS PRODUCT_CD, ");
            selectQuery.Append(" SH.PYMDHM AS CREATED_DATE, ");
            selectQuery.Append(" SEZ.製仕様番号１ AS PROD_SPEC_1, ");
            selectQuery.Append(" SEZ.製仕様番号２ AS PROD_SPEC_2, ");

            selectQuery.Append(" CASE WHEN SH.PKATA1 = 'S' OR SH.PKATA1 = 'P' OR SH.PKATA1 = 'F' THEN 0 ");
            selectQuery.Append(" WHEN SEZ.製仕様番号１ IS NOT NULL THEN 1 ELSE NULL END AS FLAG_CODE, ");
            selectQuery.Append(" NULL AS DUPLICATE_RECORDS ");
            selectQuery.Append(" FROM MSYUKTRF SH ");
            selectQuery.Append(" LEFT JOIN SEIZOCSV SEZ ");
            selectQuery.Append(" ON SH.PFHNNO  = SEZ.製造不変番号 AND ");
            selectQuery.Append(" SH.PHINME  = SEZ.製鋼種 AND ");
            selectQuery.Append(" SH.PSYAKI  = SEZ.製社内規格 AND ");
            selectQuery.Append(" SH.PHINCD  = SEZ.製コード AND ");
            selectQuery.Append(" SH.PKATA1  = SEZ.製形 AND ");
            selectQuery.Append(" SH.PKYCD1  = SEZ.得意先コード AND ");
            selectQuery.Append(" SH.PNAGAS  = SEZ.旧製長さ AND ");
            selectQuery.Append(" SH.PJYTAI  = SEZ.製納入状態  ");
            selectQuery.Append(" )  ");
            var result = ExecuteQueryMultiResult<BCH_PROD_SHUKKA>(selectQuery.ToString());

            var result1 = result.AsEnumerable().Where(r => r.PROD_SPEC_1 != null || r.SHAPE == "S" || r.SHAPE == "P" || r.SHAPE == "F").Select(r => r).ToList(); //PROD SPEC 1 AND 2 NOT NULL AND SHAPE S, P, AND F DATA TAKEN IN RESULT1            
            var result2 = result.AsEnumerable().Where(r => r.PROD_SPEC_1 == null && !r.SHAPE.Equals("S") && !r.SHAPE.Equals("P") && !r.SHAPE.Equals("F")).Select(r => r).ToList(); //PROD SPEC 1 AND 2 IS NULL AND  SHAPE S, P, AND F NOT EXIST DATA TAKEN IN RESULT2


            foreach (var item in result2)
            {
                List<PRODUCT_MASTER_HOST> listData = new List<PRODUCT_MASTER_HOST>();
                string prodCD = item.PRODUCT_CD;
                string prodName = item.PRODUCT_NAME; //PHINME
                string comStandard = item.COMPANY_STANDARD; //PSYAKI
                decimal? dimD = item.DIM_D; //PSNPO1
                string shape = item.SHAPE; // PKATA1
                string dimTol = item.DIM_TOL; //PSNPKS
                string condition = item.CONDITION; //PJYTAI
                string allocCD = item.ALLOC_CD; //PKYCD1
                string prodLength = item.PROD_LENGTH; //PNAGAS
                int? flagCode = null;


                listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                && r.MKATA1 == shape && r.MSNPKS == dimTol && r.MJYTAI == condition && r.MKYCD1 == allocCD && r.MNAGAS == prodLength).ToList(); //FLAG CODE 2 CHECKED
                if (listData.Count > 0)
                {
                    flagCode = 2;
                }

                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MJYTAI == condition && r.MKYCD1 == allocCD && r.MNAGAS == prodLength).ToList(); //FLAG CODE 3 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 3;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MKYCD1 == allocCD && r.MNAGAS == prodLength).ToList(); //FLAG CODE 4 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 4;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MHINME == prodName && r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MNAGAS == prodLength).ToList(); //FLAG CODE 5 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 5;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MSYAKI == comStandard && r.MSNPO1 == dimD
                    && r.MKATA1 == shape && r.MNAGAS == prodLength).ToList(); //FLAG CODE 6 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 6;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MSYAKI == comStandard && r.MKATA1 == shape && r.MNAGAS == prodLength).ToList(); //FLAG CODE 7 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 7;
                    }
                }
                if (listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MSYAKI == comStandard && r.MKATA1 == shape).ToList(); //FLAG CODE 8 CHECKED
                    if (listData.Count > 0)
                    {
                        flagCode = 8;
                    }

                }

                if (prodCD != string.Empty && listData.Count == 0)
                {
                    listData = prodMaster.Where(r => r.MFHNNO == prodCD).ToList(); //DATA CHECKED WITH PRODUCT CD
                }
                if (listData.Count > 0)
                {
                    if (listData[0].PROD_SPEC_1 == "B" || listData[0].PROD_SPEC_1 == "C" || listData[0].PROD_SPEC_1 == "ｺｸｲﾝ" || listData[0].PROD_SPEC_1 == "ｻｲｶｺ" || listData[0].PROD_SPEC_1 == "ｿｻﾞｲ")
                    {
                        item.FLAG_CODE = 98;
                        item.DUPLICATE_RECORDS = null;
                        item.PROD_SPEC_1 = listData[0].PROD_SPEC_1;
                        item.PROD_SPEC_2 = listData[0].PROD_SPEC_2;
                        item.PRODUCT_CD = listData[0].MFHNNO;

                    }
                    else
                    {
                        item.FLAG_CODE = flagCode;
                        item.DUPLICATE_RECORDS = listData.Count;
                        item.PROD_SPEC_1 = listData[0].PROD_SPEC_1;
                        item.PROD_SPEC_2 = listData[0].PROD_SPEC_2;
                        item.PRODUCT_CD = listData[0].MFHNNO;
                    }
                }
            }
            shukkaDetails.AddRange(result1);

            result2.AsEnumerable().Where(r => r.FLAG_CODE == null && r.PROD_SPEC_1 == null && r.PROD_SPEC_2 == null).ToList().ForEach(i => i.FLAG_CODE = 99);  //FLAG CODE 99 SET FOR ALL NOT MATCHING DATA
            shukkaDetails.AddRange(result2);

            return shukkaDetails;
        }
        private void ImportDataProdStock(List<BCH_PROD_STOCK> details)
        {
            StringBuilder insertQuery = new StringBuilder();

            insertQuery.Append(" INSERT INTO BCH_PROD_STOCK ");
            insertQuery.Append(" ( ");
            insertQuery.Append(" SLIP_NUMBER, ");
            insertQuery.Append(" NYUKA_DATE, ");
            insertQuery.Append(" PRODUCT_NAME, ");
            insertQuery.Append(" COMPANY_STANDARD, ");
            insertQuery.Append(" STOCK_CD, ");
            insertQuery.Append(" DIM_D, ");
            insertQuery.Append(" PROD_SHAPE, ");
            insertQuery.Append(" ALLOC_CD, ");
            insertQuery.Append(" CURRENT_CD, ");
            insertQuery.Append(" DIM_TOL, ");
            insertQuery.Append(" DIM_OTHER, ");
            insertQuery.Append(" PRODUCT_LENGTH, ");
            insertQuery.Append(" CONDITION, ");
            insertQuery.Append(" PROD_STOCK_QTY, ");
            insertQuery.Append(" NYUKA_DATE_YEAR, ");
            insertQuery.Append(" PRODUCT_CD, ");
            insertQuery.Append(" FLAG_CODE, ");
            insertQuery.Append(" DUPLICATE_RECORDS, ");
            insertQuery.Append(" PROD_SPEC_1, ");
            insertQuery.Append(" PROD_SPEC_2 ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES ");
            insertQuery.Append(" ( ");
            insertQuery.Append(" :SLIP_NUMBER, ");
            insertQuery.Append(" :NYUKA_DATE, ");
            insertQuery.Append(" :PRODUCT_NAME, ");
            insertQuery.Append(" :COMPANY_STANDARD, ");
            insertQuery.Append(" :STOCK_CD, ");
            insertQuery.Append(" :DIM_D, ");
            insertQuery.Append(" :PROD_SHAPE, ");
            insertQuery.Append(" :ALLOC_CD, ");
            insertQuery.Append(" :CURRENT_CD, ");
            insertQuery.Append(" :DIM_TOL, ");
            insertQuery.Append(" :DIM_OTHER, ");
            insertQuery.Append(" :PRODUCT_LENGTH, ");
            insertQuery.Append(" :CONDITION, ");
            insertQuery.Append(" :PROD_STOCK_QTY, ");
            insertQuery.Append(" :NYUKA_DATE_YEAR, ");
            insertQuery.Append(" :PRODUCT_CD, ");
            insertQuery.Append(" :FLAG_CODE, ");
            insertQuery.Append(" :DUPLICATE_RECORDS, ");
            insertQuery.Append(" :PROD_SPEC_1, ");
            insertQuery.Append(" :PROD_SPEC_2 ");
            insertQuery.Append(" ) ");

            ExecuteNonQueryList(insertQuery.ToString(), details);


        }
        private void ImportDataKhakorai(List<BCH_KHAKORAI> details)
        {
            StringBuilder insertQuery = new StringBuilder();

            insertQuery.Append(" INSERT INTO BCH_KHAKORAI ");
            insertQuery.Append(" ( ");
            insertQuery.Append(" CHUUBAN, ");
            insertQuery.Append(" GYOUBAN, ");
            insertQuery.Append(" PROD_SPEC_NO1, ");
            insertQuery.Append(" PROD_SPEC_NO2, ");
            insertQuery.Append(" KAKOUIARI_QTY, ");
            insertQuery.Append(" ACT_ALLOC_QTY, ");
            insertQuery.Append(" ACT_ALLOC_WEIGHT, ");
            insertQuery.Append(" ACT_SHUKKA_WEIGHT, ");
            insertQuery.Append(" PROD_WEIGHT, ");
            insertQuery.Append(" PRODUCT_CD ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES ");
            insertQuery.Append(" ( ");
            insertQuery.Append(" :CHUUBAN, ");
            insertQuery.Append(" :GYOUBAN, ");
            insertQuery.Append(" :PROD_SPEC_NO1, ");
            insertQuery.Append(" :PROD_SPEC_NO2, ");
            insertQuery.Append(" :KAKOUIARI_QTY, ");
            insertQuery.Append(" :ACT_ALLOC_QTY, ");
            insertQuery.Append(" :ACT_ALLOC_WEIGHT, ");
            insertQuery.Append(" :ACT_SHUKKA_WEIGHT, ");
            insertQuery.Append(" :PROD_WEIGHT, ");
            insertQuery.Append(" :PRODUCT_CD ");
            insertQuery.Append(" ) ");

            ExecuteNonQueryList(insertQuery.ToString(), details);


        }

        public int PlaceOrder(GridSearchFields details, short inputLocationCD)
        {
            int poSuccessFlag = 0;
            ArrayList poNoList = new ArrayList();
            poSuccessFlag = PlacePO(Convert.ToInt16(details.EMPLOYEE_CD), poNoList, Convert.ToInt16(details.USER_ID), inputLocationCD, details.ROLL_YEAR, details.ROLL_R);
            string result;
            StringBuilder updateYrQuery = new StringBuilder();

            return poSuccessFlag;
        }
        /// <summary>
        /// CREATED BY :Namrata Ubale on 2025/05/07
        /// Used Screen : RollPomainAnalysis
        /// </summary>
        /// <param name="employeeCD"></param>
        /// <returns>
        /// Get Forecast Roll Year From MST_CONFIGURATION table
        /// </returns>
        public RollPoMainAnalysisDetails GetForecastRoll()
        {
            RollPoMainAnalysisDetails result = new RollPoMainAnalysisDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_YEAR' THEN VALUE END) AS ROLL_YEAR, ");
            selectQuery.Append("MAX(CASE WHEN KEY = 'FORECAST_BATCH_ROLL' THEN VALUE END) AS ROLL_R ");
            selectQuery.Append("FROM MST_CONFIGURATION ");
            selectQuery.Append("WHERE KEY IN ('FORECAST_BATCH_YEAR', 'FORECAST_BATCH_ROLL')");
            var query = selectQuery.ToString();
            selectQuery.Append(query);
            result = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(query);
            return result;
        }
        private int PlacePO(int empCode, ArrayList poNoList, int userId, short inputLocationCD, int ROll_YEAR, int Roll_R)
        {
            DataTable supplierMakerMatlTypeR = null;            
            long poNo;
            DataTable poDetailsFromTmpPoFc = null;           
            long poDtlNo;
            int srlNo;
            int poSuccessFlag = 0;


            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    supplierMakerMatlTypeR = GetSupplierMakerMatlTypeR();

                    if (supplierMakerMatlTypeR.Rows.Count == 0)
                    {
                        poSuccessFlag = 1;
                    }
                    else
                    {
                        try
                        {
                            foreach (DataRow row in supplierMakerMatlTypeR.Rows)
                            {
                                poNo = GeneratePONumber();

                                InsertPOHeader(row, poNo, empCode, userId, inputLocationCD, ROll_YEAR, Roll_R);

                                poDetailsFromTmpPoFc = GetPODetailsFromTempRollPODetails(row);
                                srlNo = 0;

                                if (poDetailsFromTmpPoFc != null && poDetailsFromTmpPoFc.Rows.Count > 0)
                                {
                                    foreach (DataRow detailRow in poDetailsFromTmpPoFc.Rows)
                                    {
                                        srlNo++;
                                        int poKubun = 1; //In windows poKubun declared as constant value = 1 as new po
                                        poDtlNo = GeneratePODetailCode(poNo, srlNo);
                                        InsertPODetails(detailRow, poNo, poDtlNo, poKubun, srlNo, userId, inputLocationCD);
                                        StringBuilder selectQuery = new StringBuilder();

                                        selectQuery.Append("SELECT PO_DTL_CD ");
                                        selectQuery.Append("FROM TRN_PO_DTL ");
                                        selectQuery.Append("WHERE PO_NO = " + poNo + " ");
                                        selectQuery.Append("AND PO_DTL_NO = " + poDtlNo);

                                        RollPoMainAnalysisDetails result = ExecuteQuerySingleResult<RollPoMainAnalysisDetails>(selectQuery.ToString());

                                        long? poDtlCd = result.PO_DTL_CD;
                                        long rollPoDtlCd = Convert.ToInt64(detailRow["ROLL_PO_DETAIL_CD"]);
                                        UpdateTempRollPoDetails(poNo, poDtlCd, rollPoDtlCd);


                                        
                                    }


                                }

                                poNoList.Add(poNo);
                            }

                        }
                        catch (Exception)
                        {

                            throw;

                        }

                        poSuccessFlag = 2; // PO Success

                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                return poSuccessFlag;

            }
            catch (Exception ex)
            {
                throw ex;
                poSuccessFlag = 0;
            }

        }


        public void UpdateTempRollPoDetails(long poNo, long? poDtlCd, long rollPoDtlCd)
        {         
            StringBuilder updateSql = new StringBuilder();
            updateSql.Append("UPDATE TEMP_ROLL_PO_DETAILS SET ");
            updateSql.Append("PO_NO = " + poNo + ", ");
            updateSql.Append("PO_DTL_CD = " + poDtlCd + " ");
            updateSql.Append("WHERE ROLL_PO_DETAIL_CD = " + rollPoDtlCd);
            ExecuteNonQuery(updateSql.ToString());
        }

        public DataTable GetSupplierMakerMatlTypeR()
        {

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" RPD.SUPPLIER_CD, ");
            selectQuery.Append(" MAKER_CD, ");
            selectQuery.Append(" MATL_KUBUN, ");
            selectQuery.Append(" ROLL_R,");
            selectQuery.Append(" NULL AS REPORT_PAGE_NO ");
            selectQuery.Append(" FROM TEMP_ROLL_PO_DETAILS RPD ");
            selectQuery.Append(" JOIN MST_MATERIAL MM ON RPD.MATERIAL_CD = MM.MATERIAL_CD ");
            selectQuery.Append(" JOIN MST_MATL_NAME_MAKER MK ON MM.MATL_NAME_MAKER_CD = MK.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" GROUP BY RPD.SUPPLIER_CD, MAKER_CD, MATL_KUBUN, ROLL_R ");
            selectQuery.Append(" ORDER BY SUPPLIER_CD, MAKER_CD, MATL_KUBUN, ROLL_R ");

            DataTable Result = ExecuteQueryDataTable(selectQuery.ToString());
            return Result;
        }

        private int GeneratePONumber()
        {
            DateTime Date = DateTime.Now;
            StringBuilder selectQuery = new StringBuilder();
            string poDate = Date.ToString(DateFormats.YYYYMMDD);
            selectQuery.Append("SELECT GET_NEW_PO_NUMBER(TO_DATE('" + poDate + "','YYYY/MM/DD')) FROM DUAL");
            var PO_NO = ExecuteScalar(selectQuery.ToString());
            return Convert.ToInt32(PO_NO);

        }

        public void InsertPOHeader(DataRow supplierMakerMatlTypeR, long poNo, int empCode, int userId, short inputLocationCD, int ROll_YEAR, int Roll_R)
        {
            string YearR = ROll_YEAR.ToString() + Roll_R.ToString("D2");
            StringBuilder insertSql = new StringBuilder();

            insertSql.AppendLine(" INSERT INTO TRN_PO_HDR ");
            insertSql.AppendLine(" (PO_NO, ");
            insertSql.AppendLine(" PO_DATE, ");
            insertSql.AppendLine(" EMPLOYEE_CD, ");
            insertSql.AppendLine(" SUPPLIER_CD, ");
            insertSql.AppendLine(" MAKER_CD, ");
            insertSql.AppendLine(" MATL_KUBUN, ");
            insertSql.AppendLine(" CREATED_DATE, ");
            insertSql.AppendLine(" UPDATED_DATE, ");
            insertSql.AppendLine(" REPORT_PAGE_NO, ");
            insertSql.AppendLine(" REPORT_YEAR, ");
            insertSql.Append(" USER_ID,");
            insertSql.AppendLine(" INPUT_LOCATION_CD, ");
            insertSql.AppendLine(" REPORT_PRINT_DATE, ");
            insertSql.AppendLine(" IS_MONTHLY_PO, ");
            insertSql.AppendLine(" MONTHLY_PO_YEAR_R) ");
            insertSql.AppendLine(" VALUES (");
            insertSql.AppendLine("'" + poNo + "',");
            insertSql.AppendLine(" SYSDATE, ");
            insertSql.AppendLine("'" + empCode + "',");
            insertSql.AppendLine("'" + supplierMakerMatlTypeR["SUPPLIER_CD"] + "', ");
            insertSql.AppendLine("'" + supplierMakerMatlTypeR["MAKER_CD"] + "', ");
            insertSql.AppendLine("'" + supplierMakerMatlTypeR["MATL_KUBUN"] + "',");
            insertSql.AppendLine(" SYSDATE, ");
            insertSql.AppendLine(" SYSDATE, ");
            insertSql.AppendLine("'" + supplierMakerMatlTypeR["REPORT_PAGE_NO"] + "', ");
            insertSql.AppendLine(" (SELECT TO_CHAR(SYSDATE, 'yyyy') FROM DUAL), ");
            insertSql.AppendLine("'" + userId + "',");
            insertSql.AppendLine("'" + inputLocationCD + "',");
            insertSql.AppendLine(" SYSDATE,");
            insertSql.AppendLine(" 1, ");
            insertSql.AppendLine("'" + YearR + "'");
            insertSql.AppendLine(" ) ");

            //commandText.AppendLine("UPDATE MST_SUPPLIER ");
            //commandText.AppendLine("SET REPORT_PAGE_NO = " + OracleDatabase.GetSQLValue(vendorMakerMatlTypeRow["REPORT_PAGE_NO"], DatatypesEnum.NumericType) + ", ");
            //commandText.AppendLine("REPORT_YEAR = (SELECT TO_CHAR(SYSDATE, 'yyyy') FROM DUAL) ");
            //commandText.AppendLine("WHERE SUPP_CD = " + OracleDatabase.GetSQLValue(vendorMakerMatlTypeRow["SUPP_CD"], DatatypesEnum.NumericType) + ";");
            //commandText.AppendLine("END;");

            ExecuteNonQuery(insertSql.ToString());
        }

        public DataTable GetPODetailsFromTempRollPODetails(DataRow supplierMakerMatlTypeR)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" RPD.ROLL_PO_DETAIL_CD,");
            selectQuery.Append(" RPD.MATERIAL_CD, ");
            selectQuery.Append(" RPD.MUKESAKI_CD, ");
            selectQuery.Append(" RPD.MOCHIKOMISAKI_CD, ");
            selectQuery.Append(" RPD.HTSAKI_CD, ");
            selectQuery.Append(" RPD.FUTOU_CD, ");
            selectQuery.Append(" ROLL_YEAR, ");
            selectQuery.Append(" ROLL_R, ");
            selectQuery.Append(" ROLL_CHANCE, ");
            selectQuery.Append(" QTY, ");
            selectQuery.Append(" RATE, ");
            selectQuery.Append(" RPD.REMARKS, ");
            selectQuery.Append(" NULL AS PO_INT_MEMO ");
            selectQuery.Append(" FROM TEMP_ROLL_PO_DETAILS RPD ");
            selectQuery.Append(" JOIN MST_MATERIAL HT ON RPD.MATERIAL_CD = HT.MATERIAL_CD  ");
            selectQuery.Append(" JOIN MST_MATL_NAME_MAKER MK ON HT.MATL_NAME_MAKER_CD = MK.MATL_NAME_MAKER_CD  ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" RPD.SUPPLIER_CD = '" + supplierMakerMatlTypeR["SUPPLIER_CD"] + "'");
            selectQuery.Append(" AND MAKER_CD = '" + supplierMakerMatlTypeR["MAKER_CD"] + "'");
            selectQuery.Append(" AND MATL_KUBUN = '" + supplierMakerMatlTypeR["MATL_KUBUN"] + "'");
            selectQuery.Append(" AND ROLL_R = '" + supplierMakerMatlTypeR["ROLL_R"] + "'");
            selectQuery.Append(" ORDER BY MUKESAKI_CD, RPD.MOCHIKOMISAKI_CD, MK.MATL_MAKER_NAME, HT.DIM_D, HT.SHAPE, HT.MATERIAL_LENGTH, ROLL_PO_DETAIL_CD");

            DataTable Result = ExecuteQueryDataTable(selectQuery.ToString());
            return Result;
        }

        public static long GeneratePODetailCode(long poNo, int srlNo)
        {
            string poDtlNoString;
            long poDtlNo = 0;

            // Concatenate PO number with the serial number, left-padded with zeroes to 3 digits
            poDtlNoString = poNo.ToString() + srlNo.ToString().PadLeft(3, '0');

            if (!string.IsNullOrEmpty(poDtlNoString))
            {
                poDtlNo = Convert.ToInt64(poDtlNoString);
            }


            return poDtlNo;
        }

        public void InsertPODetails(DataRow poDetails, long poNo, long poDtlNo, int poKubun, int srlNo, int userId, short inputLocationCD)
        {
            StringBuilder insertSql = new StringBuilder();
            int inactiveFlag = 0;
            int poStatus = 10;
            insertSql.Append(" INSERT INTO TRN_PO_DTL ");
            insertSql.Append(" (PO_DTL_CD, ");
            insertSql.Append(" PO_NO, ");
            insertSql.Append(" PO_DTL_SRLNO, ");
            insertSql.Append(" PO_KUBUN, ");
            insertSql.Append(" MATERIAL_CD, ");
            insertSql.Append(" HTSAKI_CD, ");
            insertSql.Append(" MOCHIKOMISAKI_CD, ");
            insertSql.Append(" FUTOU_CD, ");
            insertSql.Append(" MUKESAKI_CD, ");
            insertSql.Append(" QTY, ");
            insertSql.Append(" RATE, ");
            insertSql.Append(" YYYY, ");
            insertSql.Append(" ROLL_R, ");
            insertSql.Append(" CHANCE, ");
            insertSql.Append(" REMARKS, ");
            insertSql.Append(" PO_INT_MEMO, ");
            insertSql.Append(" CREATED_DATE, ");
            insertSql.Append(" PO_DTL_NO, ");
            insertSql.Append(" UPDATED_DATE,");
            insertSql.Append(" USER_ID,");
            insertSql.Append(" INPUT_LOCATION_CD,");
            insertSql.Append(" INACTIVE_FLAG,");
            insertSql.Append(" PO_STATUS ");
            insertSql.Append(" ) ");
            insertSql.Append(" VALUES (");
            insertSql.Append(" PO_DTL_CD_SEQ.NEXTVAL, ");
            insertSql.Append("'" + poNo + "',");
            insertSql.Append("'" + srlNo + "', ");
            insertSql.Append("'" + poKubun + "', ");
            insertSql.Append("'" + poDetails["MATERIAL_CD"] + "', ");
            insertSql.Append("'" + poDetails["HTSAKI_CD"] + "', ");
            insertSql.Append("'" + poDetails["MOCHIKOMISAKI_CD"] + "', ");
            insertSql.Append("'" + poDetails["FUTOU_CD"] + "', ");
            insertSql.Append("'" + poDetails["MUKESAKI_CD"] + "', ");
            insertSql.Append("'" + poDetails["QTY"] + "', ");
            insertSql.Append("'" + poDetails["RATE"] + "', ");
            insertSql.Append("'" + poDetails["ROLL_YEAR"] + "', ");
            insertSql.Append("'" + poDetails["ROLL_R"] + "', ");
            insertSql.Append("'" + poDetails["ROLL_CHANCE"] + "', ");
            insertSql.Append("'" + poDetails["REMARKS"] + "', ");
            insertSql.Append("'" + poDetails["PO_INT_MEMO"] + "', ");
            insertSql.Append(" SYSDATE, ");
            insertSql.Append("'" + poDtlNo + "', ");
            insertSql.Append(" SYSDATE, ");
            insertSql.Append("'" + userId + "', ");
            insertSql.Append("'" + inputLocationCD + "',");
            insertSql.Append("'" + inactiveFlag + "',");
            insertSql.Append("'" + poStatus + "'");
            insertSql.Append(")");

            ExecuteNonQuery(insertSql.ToString());
        }

        public string SetAnalysisOverForFcSummaryWithNoFC()
        {
            string result;
            StringBuilder updateSql = new StringBuilder();
            updateSql.Append(" UPDATE TEMP_INT_MATL_FC_SUMMARY SET ");
            updateSql.Append(" PO_REVIEWED_DATE = TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','yyyy/MM/dd HH24:MI:SS')  ");
            updateSql.Append(" WHERE ");
            updateSql.Append(" TEMP_INT_MATL_FC_SUMMARY. PO_REVIEWED_DATE IS NULL AND ");
            updateSql.Append(" (NVL(SALES_FC_QTY_THIS_MONTH, 0) = 0 ");
            updateSql.Append(" AND NVL(SALES_FC_QTY_1MONTH_AHEAD, 0) = 0 ");
            updateSql.Append(" AND NVL(SALES_FC_QTY_2MONTH_AHEAD, 0) = 0 ");
            updateSql.Append(" AND NVL(SALES_FC_QTY_3MONTH_AHEAD, 0) = 0) ");

            ExecuteNonQuery(updateSql.ToString());

            result = "Data Updated";

            return result;

        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/06/03      
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Check PO is already place or not
        /// </returns>
        /// 
        public int IsPODone(DateTimeDetails details)
        {
            int count;
            string YearR = details.CURRENT_YYYY.ToString() + details.CURRENT_R.ToString("D2");
            StringBuilder selectSql = new StringBuilder();
            selectSql.Append(" SELECT COUNT(*)  ");
            selectSql.Append(" FROM TRN_PO_HDR ");
            selectSql.Append(" WHERE IS_MONTHLY_PO = 1 ");
            selectSql.Append(" AND MONTHLY_PO_YEAR_R = '" + YearR + "' ");
            count = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/06/16     
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Check if record is found in TEMP_INT_MATL_FC_SUMMARY for that R And Year
        /// </returns>
        /// 
        public int CheckMatchingYearRRecords(RollPoMainAnalysisDetails details)
        {
            int count;
            StringBuilder selectSql = new StringBuilder();
            selectSql.Append(" SELECT COUNT(*)  ");
            selectSql.Append(" FROM TEMP_INT_MATL_FC_SUMMARY ");
            selectSql.Append(" WHERE ROLL_FC_YEAR = '" + details.ROLL_YEAR + "' ");
            selectSql.Append(" AND ROLL_FC_R = '" + details.ROLL_R + "' ");
            count = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/06/16     
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Check if ROLL_FC_LOCK_DATE is found for record in TRN_SALES_FORECAST for that R And Year
        /// </returns>
        /// 
        public int CheckForecastLocked(RollPoMainAnalysisDetails details)
        {
            int count;
            StringBuilder selectSql = new StringBuilder();
            selectSql.Append(" SELECT COUNT(ROLL_FC_LOCK_DATE)  ");
            selectSql.Append(" FROM TRN_SALES_FORECAST ");
            selectSql.Append(" WHERE ROLL_FC_YEAR = '" + details.ROLL_YEAR + "' ");
            selectSql.Append(" AND ROLL_FC_R = '" + details.ROLL_R + "' ");
            count = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/06/12      
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Check All PO_REVIEWED_DATE is not null
        /// </returns>
        /// 
        public int IsAllAnalysisIsOver()
        {
            int count;
            StringBuilder selectSql = new StringBuilder();
            selectSql.Append(" SELECT COUNT(*)  ");
            selectSql.Append(" FROM TEMP_INT_MATL_FC_SUMMARY ");
            selectSql.Append(" WHERE PO_REVIEWED_DATE IS NULL  ");
            count = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/06/12      
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// Get Employee cd to dropdown(Employee who executes the batch ) when data starts display on gird
        /// </returns>
        /// 
        public int GetEmployeeCd()
        {
            int employeeCd;
            StringBuilder selectSql = new StringBuilder();
            selectSql.Append(" SELECT EMPLOYEE_CD ");
            selectSql.Append(" FROM TEMP_INT_MATL_FC_SUMMARY ");
            selectSql.Append(" WHERE ROWNUM = 1  ");
            employeeCd = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
            return employeeCd;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2025/10/01       
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// This method inserts a productCD in TMP_MATL_NOT_FOUND table, when a record 
        /// matching with matl specs of a product whose forecast is registered in TRN_SALES_FORECAST 
        /// is not found in TEMP_INT_MATL_FC_SUMMARY. That means, that material is not yet registered in 
        /// material master.
        /// This table is maintained for gathering such records in order to print error report.
        /// </returns>
        /// 
        public void InsertInTmpMatlNotFound(int productCD)
        {
            StringBuilder insertSql = new StringBuilder();
            StringBuilder selectSql = new StringBuilder();
            int productCodeExists;

            selectSql.Append(" SELECT PRODUCT_CD FROM TMP_MATL_NOT_FOUND WHERE PRODUCT_CD ='" + productCD + "' ");
            productCodeExists = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));

            if (productCodeExists == 0)
            {
                insertSql.Append(" INSERT INTO TMP_MATL_NOT_FOUND( ");
                insertSql.Append(" PRODUCT_CD,");
                insertSql.Append(" CREATED_DATE");
                insertSql.Append(" )");
                insertSql.Append(" VALUES (");
                insertSql.Append("'" + productCD + "',");
                insertSql.Append(" SYSDATE) ");
                ExecuteNonQuery(insertSql.ToString());
            }
        }
    }

    public class RollYearList
    {
        public string Roll_RBeforeSixMonth { get; set; }
        public string Roll_MBeforeSixMonth { get; set; }
       // public string Roll_YearBeforeSixMonth { get; set; }

        public string Roll_RBeforeFiveMonth { get; set; }
        public string Roll_MBeforeFiveMonth { get; set; }
       // public string Roll_YearBeforeFiveMonth { get; set; }

        public string Roll_RBeforeFourMonth { get; set; }
        public string Roll_MBeforeFourMonth { get; set; }
        //public string Roll_YearBeforeFourMonth { get; set; }

        public string Roll_RBeforeThreeMonth { get; set; }
        public string Roll_YearBeforeThreeMonth { get; set; }

        public string Roll_RBeforeTwoMonth { get; set; }
        public string Roll_YearBeforeTwoMonth { get; set; }

        public string Roll_RBeforeOneMonth { get; set; }
        public string Roll_YearBeforeOneMonth { get; set; }

        public string Roll_RCurrentMonth { get; set; }
        public string Roll_YearCurrentMonth { get; set; }

        public string Roll_RAfterOneMonth { get; set; }
        public string Roll_YearAfterOneMonth { get; set; }

        public string Roll_RYearBeforeSixMonth { get; set; }
        public string Roll_MYearBeforeSixMonth { get; set; }
        public string Roll_RYearBeforeFiveMonth { get; set; }
        public string Roll_MYearBeforeFiveMonth { get; set; }
        public string Roll_RYearBeforeFourMonth { get; set; }
        public string Roll_MYearBeforeFourMonth { get; set; }


        public RollYearList(int _rollYear, int _rollR)
        {
            DateTime currentDate = new DateTime(_rollYear, _rollR, 1);
            DateTime currentR = currentDate.AddMonths(0);

            //DateTime dateBeforeSixMonth = currentR.AddMonths(-6);
            //DateTime monthBeforeSixMonth = dateBeforeSixMonth.AddMonths(2);

            //Roll_RBeforeSixMonth = dateBeforeSixMonth.ToString("MM");
            //Roll_MBeforeSixMonth = monthBeforeSixMonth.ToString("MM");
            //Roll_YearBeforeSixMonth = dateBeforeSixMonth.ToString("yyyy");

            //DateTime dateBeforeFiveMonth = currentR.AddMonths(-5);
            //DateTime monthBeforeFiveMonth = dateBeforeFiveMonth.AddMonths(2);

            //Roll_RBeforeFiveMonth = dateBeforeFiveMonth.ToString("MM");
            //Roll_MBeforeFiveMonth = monthBeforeFiveMonth.ToString("MM");
            //Roll_YearBeforeFiveMonth = dateBeforeFiveMonth.ToString("yyyy");

            //DateTime dateBeforeFourMonth = currentR.AddMonths(-4);
            //DateTime monthBeforeFourMonth = dateBeforeFourMonth.AddMonths(2);

            //Roll_RBeforeFourMonth = dateBeforeFourMonth.ToString("MM");
            //Roll_MBeforeFourMonth = monthBeforeFourMonth.ToString("MM");
            //Roll_YearBeforeFourMonth = dateBeforeFourMonth.ToString("yyyy");



            // SIX MONTH
            DateTime dateBeforeSixMonth = currentR.AddMonths(-6);
            DateTime monthBeforeSixMonth = dateBeforeSixMonth.AddMonths(2);

            Roll_RBeforeSixMonth = dateBeforeSixMonth.ToString("MM");
            Roll_RYearBeforeSixMonth = dateBeforeSixMonth.ToString("yyyy");

            Roll_MBeforeSixMonth = monthBeforeSixMonth.ToString("MM");
            Roll_MYearBeforeSixMonth = monthBeforeSixMonth.ToString("yyyy");


            // FIVE MONTH
            DateTime dateBeforeFiveMonth = currentR.AddMonths(-5);
            DateTime monthBeforeFiveMonth = dateBeforeFiveMonth.AddMonths(2);

            Roll_RBeforeFiveMonth = dateBeforeFiveMonth.ToString("MM");
            Roll_RYearBeforeFiveMonth = dateBeforeFiveMonth.ToString("yyyy");

            Roll_MBeforeFiveMonth = monthBeforeFiveMonth.ToString("MM");
            Roll_MYearBeforeFiveMonth = monthBeforeFiveMonth.ToString("yyyy");


            // FOUR MONTH
            DateTime dateBeforeFourMonth = currentR.AddMonths(-4);
            DateTime monthBeforeFourMonth = dateBeforeFourMonth.AddMonths(2);

            Roll_RBeforeFourMonth = dateBeforeFourMonth.ToString("MM");
            Roll_RYearBeforeFourMonth = dateBeforeFourMonth.ToString("yyyy");

            Roll_MBeforeFourMonth = monthBeforeFourMonth.ToString("MM");
            Roll_MYearBeforeFourMonth = monthBeforeFourMonth.ToString("yyyy");


            DateTime dateBeforeThreeMonth = currentR.AddMonths(-3);

            Roll_RBeforeThreeMonth = dateBeforeThreeMonth.ToString("MM");
            Roll_YearBeforeThreeMonth = dateBeforeThreeMonth.ToString("yyyy");

            DateTime dateBeforeTwoMonth = currentR.AddMonths(-2);

            Roll_RBeforeTwoMonth = dateBeforeTwoMonth.ToString("MM");
            Roll_YearBeforeTwoMonth = dateBeforeTwoMonth.ToString("yyyy");

            DateTime dateBeforeOneMonth = currentR.AddMonths(-1);

            Roll_RBeforeOneMonth = dateBeforeOneMonth.ToString("MM");
            Roll_YearBeforeOneMonth = dateBeforeOneMonth.ToString("yyyy");

            DateTime dateCurrentMonth = currentR;

            Roll_RCurrentMonth = dateCurrentMonth.ToString("MM");
            Roll_YearCurrentMonth = dateCurrentMonth.ToString("yyyy");

            DateTime dateAfterOneMonth = currentR.AddMonths(1);

            Roll_RAfterOneMonth = dateAfterOneMonth.ToString("MM");
            Roll_YearAfterOneMonth = dateAfterOneMonth.ToString("yyyy");


        }
    }
}
