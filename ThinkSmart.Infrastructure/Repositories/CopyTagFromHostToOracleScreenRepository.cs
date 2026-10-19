using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;
namespace TSWebApplication.Infrastructure.Repositories
{
    public class CopyTagFromHostToOracleScreenRepository : BaseRepository, ICopyTagFromHostToOracleScreenRepository
    {
        private readonly string _connectionString;
        private readonly string _connectionStringForRplication;
        private readonly string _locationCd_DaiIchi;
        private readonly string _locationCd_DaiNi;
        private readonly string _locationCd_Nogi;
        private readonly string _tensile_yeild_Value;
        private readonly string _elongation1_press1_Value;
        private readonly string _hardness2_Value;

        string resultData;
        List<GridSearchFields> result = new List<GridSearchFields>();
        decimal? codeValue;
        string minMaxValue;
        string minValue;
        string maxValue;
        string actualValue;
        string str;
        int index = 0;
        bool isStrPatternStartWithNum = false;
        string[] data;
        public CopyTagFromHostToOracleScreenRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _connectionStringForRplication = _configuration.GetConnectionString("OracleDBConnectionForReplication");
            _locationCd_DaiIchi = _configuration.GetSection("location_CD").GetSection("DaiIchi").Value;
            _locationCd_DaiNi = _configuration.GetSection("location_CD").GetSection("DaiNi").Value;
            _locationCd_Nogi = _configuration.GetSection("location_CD").GetSection("Nogi").Value;
            _tensile_yeild_Value = _configuration.GetSection("host_Japanese_Value").GetSection("tensile_yeild_Value").Value;
            _elongation1_press1_Value = _configuration.GetSection("host_Japanese_Value").GetSection("elongation1_press1_Value").Value;
            _hardness2_Value = _configuration.GetSection("host_Japanese_Value").GetSection("hardness2_Value").Value;
        }
        /// <summary>
        /// CREATED BY : Komal Kadam       
        /// used screens: CopyTagFromHostToOracle
        /// </summary>
        /// <returns>
        /// Fetch data by using NifudaNo using table Tag and HTMP_PRODUCT_ORDER
        /// </returns>
        /// 


        public List<GridSearchFields> GetDetailsByNifuda(List<GridSearchFields> lstNifudaNo)
        {
            int countForHost = 0, count = 0;
            int isExistnifudaDataCount = 0;
            List<GridSearchFields> result = new List<GridSearchFields>();
            for (int i = 0; i < lstNifudaNo.Count; i++)
            {

                if (lstNifudaNo[i].DISPLAYPROCESSPLANDATA == "beforeProcessStart")//
                {
                    StringBuilder selectNifudaDataQuery = new StringBuilder();
                    selectNifudaDataQuery.Append(" SELECT COUNT(*) ");
                    selectNifudaDataQuery.Append(" FROM TRN_NIFUDA  ");
                    selectNifudaDataQuery.Append(" INNER JOIN TRN_PROCESS_PLAN ON TRN_PROCESS_PLAN.PROCESS_PLAN_CD= TRN_NIFUDA.PROCESS_PLAN_CD AND TRN_PROCESS_PLAN.PROCESS_PLAN_CMPL_DATE IS NULL ");
                    selectNifudaDataQuery.Append(" WHERE NIFUDA_NO = '" + lstNifudaNo[i].NBNNO_NBNGYO + "'  ");
                    isExistnifudaDataCount = Convert.ToInt32(ExecuteScalar(selectNifudaDataQuery.ToString()));

                    if (isExistnifudaDataCount > 0)
                    {
                        GridSearchFields message = new GridSearchFields()
                        { NBNNO_NBNGYO = lstNifudaNo[i].NBNNO_NBNGYO, RESPONCE = "Tag No.Are already copied" };
                        result.Add(message);
                    }
                    else
                    {
                        StringBuilder selectQueryProcessCntForHost = new StringBuilder();
                        selectQueryProcessCntForHost.Append("SELECT COUNT(*) FROM HSK.TAG TAG  ");
                        selectQueryProcessCntForHost.Append(" WHERE TAG.NBNNO||TAG.NBNGYO  ='" + lstNifudaNo[i].NBNNO_NBNGYO + "'");
                        countForHost = Convert.ToInt32(ExecuteScalar(selectQueryProcessCntForHost.ToString()));
                        if (countForHost > 0)
                        {
                            StringBuilder selectQueryOrderNo = new StringBuilder();
                            selectQueryOrderNo.Append("SELECT TAG.ORDERNO AS CHUUBAN,");
                            selectQueryOrderNo.Append(" TAG.ORDERNOROWNO AS GYOUBAN");
                            selectQueryOrderNo.Append(" FROM HSK.TAG TAG ");
                            selectQueryOrderNo.Append(" WHERE TAG.NBNNO||TAG.NBNGYO  ='" + lstNifudaNo[i].NBNNO_NBNGYO + "'");
                            var dataHost = ExecuteQuerySingleResult<GridSearchFields>(selectQueryOrderNo.ToString());
                            StringBuilder selectQueryProcessData = new StringBuilder();
                            selectQueryProcessData.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");

                            selectQueryProcessData.Append("  WHERE CHUUBAN = '" + dataHost.CHUUBAN + "' AND GYOUBAN = '" + dataHost.GYOUBAN + "' AND PROCESS_PLAN_CMPL_DATE IS NULL  ");
                            count = Convert.ToInt32(ExecuteScalar(selectQueryProcessData.ToString()));
                            if (count > 0)
                            {
                                StringBuilder selectQuery = new StringBuilder();
                                selectQuery.Append(" SELECT HPO.CHUUBAN, ");
                                selectQuery.Append(" CONCAT(HPO.CHUUBAN,HPO.GYOUBAN) AS CHUUBAN_GYOUBAN, ");
                                selectQuery.Append(" HPO.GYOUBAN AS GYOUBAN_HOST,");
                                selectQuery.Append(" HPO.CUST_SHORT_NAME AS CUSTOMER_NAME,");
                                selectQuery.Append(" HPO.PROD_NAME_CUST,");
                                selectQuery.Append(" HPO.SHAPE,");
                                selectQuery.Append(" HPO.DIM_D AS PROD_DIM_D,");
                                selectQuery.Append(" HPO.DIM_OTHER AS PROD_DIM_OTHER_STRING,");
                                selectQuery.Append(" CASE WHEN REGEXP_LIKE(HPO.LENGTH,'[^0-9]+') THEN NULL ELSE HPO.LENGTH END AS PROD_LENGTH_STRING,");
                                selectQuery.Append(" CASE WHEN REGEXP_LIKE(HPO.LENGTH,'[^0-9]+') THEN TRIM(HPO.LENGTH) ELSE NULL END AS LENGTH_OTHER,");
                                selectQuery.Append(" HPO.CHARGE,");
                                selectQuery.Append(" HPO.ACT_ALLOC_QTY,");
                                selectQuery.Append(" HPO.DELIVERY_DATE AS HOST_DELIVERY_DATE,");
                                selectQuery.Append(" TO_DATE(HPO.ACT_ALLOC_DATE,'YYYY/MM/DD') AS HOST_ACT_ALLOC_DATE,");
                                selectQuery.Append(" HPO.INZU AS HPO_INZU,");
                                selectQuery.Append(" HPO.INZU_UNIT AS HOST_UNIT_TYPE_STRING,");
                                selectQuery.Append(" HPO.PROD_SPEC_NO_1 AS PROD_SPEC_NO1, ");
                                selectQuery.Append(" HPO.PROD_SPEC_NO_2 AS PROD_SPEC_NO2, ");
                                selectQuery.Append(" CONCAT(TAG.NBNNO, TAG.NBNGYO) AS NBNNO_NBNGYO");
                                selectQuery.Append(" FROM HSK.TAG TAG ");
                                selectQuery.Append(" INNER JOIN HSK_REPLICATION.HTMP_PRODUCT_ORDER   HPO ON CONCAT(HPO.CHUUBAN, HPO.GYOUBAN) = CONCAT(TAG.ORDERNO, TAG.ORDERNOROWNO)");
                                selectQuery.Append(" WHERE TAG.NBNNO||TAG.NBNGYO  ='" + lstNifudaNo[i].NBNNO_NBNGYO + "' ");
                                var ProcessPlanDataForHost = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                                if (ProcessPlanDataForHost != null)
                                {
                                    result.Add(ProcessPlanDataForHost);
                                }
                            }
                            else
                            {
                                GridSearchFields message = new GridSearchFields()
                                { NBNNO_NBNGYO = lstNifudaNo[i].NBNNO_NBNGYO, RESPONCE = "Tag No Can Not Data  Insert No live data found in Process plan  table" };
                                result.Add(message);
                            }

                        }
                        else
                        {
                            GridSearchFields message = new GridSearchFields()
                            { NBNNO_NBNGYO = lstNifudaNo[i].NBNNO_NBNGYO, RESPONCE = "Tag No Not Found From Host" };
                            result.Add(message);
                        }
                    }
                }
                else
                {
                    StringBuilder selectQueryProcessCntForAfter = new StringBuilder();
                    selectQueryProcessCntForAfter.Append(" SELECT COUNT(*) ");
                    selectQueryProcessCntForAfter.Append(" FROM TRN_NIFUDA  ");
                    selectQueryProcessCntForAfter.Append(" INNER JOIN TRN_PROCESS_PLAN ON TRN_PROCESS_PLAN.PROCESS_PLAN_CD= TRN_NIFUDA.PROCESS_PLAN_CD AND TRN_PROCESS_PLAN.PROCESS_PLAN_CMPL_DATE IS NULL ");
                    selectQueryProcessCntForAfter.Append(" WHERE NIFUDA_NO = '" + lstNifudaNo[i].NBNNO_NBNGYO + "'  ");
                    count = Convert.ToInt32(ExecuteScalar(selectQueryProcessCntForAfter.ToString()));
                    if (count > 0)
                    {
                        StringBuilder selectQuery = new StringBuilder();
                        selectQuery.Append(" SELECT CUST.SHORT_NAME AS CUSTOMER_NAME, ");
                        selectQuery.Append(" PP.PROD_NAME_CUST,");
                        selectQuery.Append(" PP.PROD_SHAPE,");
                        selectQuery.Append(" PP.PROD_DIM_D,");
                        selectQuery.Append(" PP.PROD_DIM_W,");
                        selectQuery.Append(" TO_CHAR(PP.PROD_DIM_OTHER) AS PROD_DIM_OTHER_STRING,");
                        selectQuery.Append(" PP.PROD_LENGTH,");
                        selectQuery.Append(" PP.LENGTH_OTHER,");
                        selectQuery.Append(" PP.PROD_CONDITION_CD,");
                        selectQuery.Append(" PP.CHARGE,");
                        selectQuery.Append(" PP.ACT_ALLOC_CD,");
                        selectQuery.Append(" PP.ACT_ALLOC_QTY,");
                        selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE,'YYYY/MM/DD') AS ACT_ALLOC_DATE_STRING,");
                        selectQuery.Append(" PP.NO_OF_UNITS,");
                        selectQuery.Append(" PP.UNIT,");
                        selectQuery.Append(" PP.PROD_SPEC_NO1,");
                        selectQuery.Append(" PP.PROD_SPEC_NO2,");
                        selectQuery.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD_HOST,");
                        selectQuery.Append(" PP.LOCATION_CD, ");
                        selectQuery.Append(" CONCAT(NI.CHUUBAN,CASE WHEN LENGTH(TRIM(NI.GYOUBAN))=1 THEN '0'||NI.GYOUBAN END) AS CHUUBAN_GYOUBAN , ");
                        selectQuery.Append(" NI.NIFUDA_NO AS NBNNO_NBNGYO");
                        selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
                        selectQuery.Append(" INNER JOIN TRN_NIFUDA NI ON PP.PROCESS_PLAN_CD = NI.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
                        selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                        selectQuery.Append(" WHERE NIFUDA_NO = '" + lstNifudaNo[i].NBNNO_NBNGYO + "'");
                        var afterProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                        if (afterProcessPlanData != null)
                        {
                            result.Add(afterProcessPlanData);
                        }
                    }
                    else
                    {
                        GridSearchFields message = new GridSearchFields()
                        { NBNNO_NBNGYO = lstNifudaNo[i].NBNNO_NBNGYO, RESPONCE = "Tag Not Found" };
                        result.Add(message);
                    }


                }



            }
            return result;
        }

        /// <summary>
        /// CREATED BY : Komal Kadam        
        /// used screens: CopyTagFromHostToOracle
        /// </summary>
        /// <returns>
        /// Fetch data by using NifudaNo using table Tag and HTMP_PRODUCT_ORDER
        /// </returns>
        /// 

        public string SaveNifudaData(List<GridSearchFields> nifudaDetails)
        {
            string responce = "";



            int location_CD = 0;
            var PrintDate = "";
            var Delivery_Print_Date_year = "";
            var Del_Date = "";
            var Complete_del_date = "";
            var Del_Date_Month = "";
            var Del_Date_Date = "";
            var Print_Date_Month_date = "";
            var Print_Date_Month = "";
            var Print_Date_Date = "";
            var Complete_Print_date = "";
            decimal tensilePatternValue;
            var tensilePatternValue1 = "";
            var tensilePatternValue2 = "";
            decimal dimTolMinValue;
            decimal yeildPatternValue;
            var yeildPatternValue1 = "";
            var yeildPatternValue2 = "";
            bool startWithNumForTensile = false;
            bool startWithNumForElongation = false;
            bool startWithNumForPress = false;
            bool startWithNumForYeild = false;
            var val = "";
            var unitWtData1 = "";
            var unitWtData2 = "";
            var unitWtData3 = "";
            string[] tensileData;
            string[] unitWtData;
            string[] yeildData;
            StringBuilder insertSql = new StringBuilder();
            for (int i = 0; i < nifudaDetails.Count; i++)
            {
                try
                {
                    // StringBuilder selectQuery = new StringBuilder();
                    // selectQuery.Append(" SELECT  MAX(PROCESS_PLAN_CD) AS PROCESS_PLAN_CD ");
                    //selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
                    // selectQuery.Append(" WHERE CHUUBAN = '" + nifudaDetails[i].CHUUBAN + "' AND GYOUBAN = '" + nifudaDetails[i].GYOUBAN + "'  ");
                    // var ProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());

                    StringBuilder selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT  ");
                    selectQuery.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD_HOST,");
                    selectQuery.Append(" PP.CUSTOMER_CD,");
                    selectQuery.Append(" PP.RECEIVER_CD,");
                    selectQuery.Append(" PP.INTERNAL_NAME_CD,");
                    selectQuery.Append(" PP.PROD_CONDITION_CD,");
                    selectQuery.Append(" PP.DEPARTMENT_GROUP_CD,");
                    selectQuery.Append(" PP.CHUUBAN,");
                    selectQuery.Append(" LPAD(PP.GYOUBAN,2,0) AS GYOUBAN_HOST,");
                    selectQuery.Append(" PP.CHARGE,");
                    selectQuery.Append(" PP.PROD_NAME_CUST,");
                    selectQuery.Append(" PP.PROD_SHAPE,");
                    selectQuery.Append(" PP.PROD_DIM_D,");
                    selectQuery.Append(" PP.PROD_DIM_W,");
                    selectQuery.Append(" PP.PROD_DIM_OTHER AS PROD_DIM_OTHER_STRING,");
                    selectQuery.Append(" PP.DIM_TOL_MIN,");
                    selectQuery.Append(" PP.DIM_TOL_MAX,");
                    selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
                    selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
                    selectQuery.Append(" PP.PROD_LENGTH,");
                    selectQuery.Append(" PP.LENGTH_OTHER,");
                    selectQuery.Append(" PP.LENGTH_TOL_MIN,");
                    selectQuery.Append(" PP.LENGTH_TOL_MAX,");
                    selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
                    selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
                    selectQuery.Append(" PP.CHAMFER,");
                    selectQuery.Append(" PP.CORNER_RADIUS,");
                    selectQuery.Append(" PP.USER_ID,");
                    selectQuery.Append(" PP.INPUT_LOCATION_CD,");
                    selectQuery.Append(" PP.ACT_ALLOC_QTY,");
                    selectQuery.Append(" PP.NO_OF_UNITS,");
                    selectQuery.Append(" PP.UNIT,");
                    selectQuery.Append(" PP.LOCATION_CD, ");
                    selectQuery.Append(" PP.FIRST_PROCESS_CD AS NEXT_PROCESS_CD ");
                    selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
                    selectQuery.Append(" WHERE CHUUBAN = '" + nifudaDetails[i].CHUUBAN + "' AND GYOUBAN = '" + nifudaDetails[i].GYOUBAN + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
                    var ProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());


                    //StringBuilder selectNifudaDataQuery = new StringBuilder();
                    //selectNifudaDataQuery.Append(" SELECT NIFUDA_NO ");
                    //selectNifudaDataQuery.Append(" FROM TRN_NIFUDA  ");
                    //selectNifudaDataQuery.Append(" WHERE NIFUDA_NO = '" + nifudaDetails[i].NBNNO_NBNGYO + "'  ");
                    //var nifudaDataCount = ExecuteQuerySingleResult<GridSearchFields>(selectNifudaDataQuery.ToString());
                    //if (nifudaDataCount == null)
                    //{
                    insertSql = new StringBuilder();
                    insertSql.Append("INSERT INTO TRN_NIFUDA(");
                    insertSql.Append("NIFUDA_CD,");
                    insertSql.Append("PROCESS_PLAN_CD,");
                    insertSql.Append("SHUKKA_INSTRUCTION_CD,");
                    insertSql.Append("SPOT_NYUKA_CD,");
                    insertSql.Append("NYUKA_CD,");
                    insertSql.Append("CUSTOMER_CD,");
                    insertSql.Append("RECEIVER_CD,");
                    insertSql.Append("INTERNAL_NAME_CD,");
                    insertSql.Append("PROD_CONDITION_CD,");
                    insertSql.Append("DEPARTMENT_GROUP_CD,");
                    insertSql.Append("NIFUDA_LOCATION_CD,");
                    insertSql.Append("NIFUDA_TYPE,");
                    insertSql.Append("NIFUDA_CLASSIFICATION,");
                    insertSql.Append("NIFUDA_NO,");
                    insertSql.Append("LAST_PROCESS_KUBUN,");
                    insertSql.Append("CHUUBAN,");
                    insertSql.Append("GYOUBAN,");
                    insertSql.Append("FI_TABA_NO,");
                    insertSql.Append("CUSTOMER_NAME_OR_SYOKUCHI,");
                    insertSql.Append("CHARGE,");
                    insertSql.Append("PROD_NAME_CUST,");
                    insertSql.Append("PROD_SHAPE,");
                    insertSql.Append("PROD_DIM_D,");
                    insertSql.Append("PROD_DIM_W,");
                    insertSql.Append("PROD_DIM_OTHER,");
                    insertSql.Append("DIM_TOL_MIN,");
                    insertSql.Append("DIM_TOL_MAX,");
                    insertSql.Append("DIM_TOL_MIN_CHR,");
                    insertSql.Append("DIM_TOL_MAX_CHR,");
                    insertSql.Append("PROD_LENGTH,");
                    insertSql.Append("LENGTH_OTHER,");
                    insertSql.Append("LENGTH_TOL_MIN,");
                    insertSql.Append("LENGTH_TOL_MAX,");
                    insertSql.Append("LENGTH_TOL_MIN_CHR,");
                    insertSql.Append("LENGTH_TOL_MAX_CHR,");
                    insertSql.Append("CHAMFER,");
                    insertSql.Append("CORNER_RADIUS,");
                    insertSql.Append("QUANTITY,");
                    insertSql.Append("NO_OF_UNITS,");
                    insertSql.Append("UNIT,");
                    insertSql.Append("STOCK_VERIFY_INSTRUCT_DATE,");
                    insertSql.Append("STOCK_VERI_QTY,");
                    insertSql.Append("STOCK_VERI_UNIT,");
                    insertSql.Append("STOCK_VERI_NO_OF_UNITS,");
                    insertSql.Append("STOCK_VERIFICATION_DATE,");
                    insertSql.Append("PROD_NIFUDA_GEN_DATE,");
                    insertSql.Append("SHUKKA_INST_NIFUDA_GEN_DATE,");
                    insertSql.Append("NIFUDA_REPRINT_DATE,");
                    insertSql.Append("INVOICE_GENERATED_FLAG,");
                    insertSql.Append("SPOT_KRW_NIFUDA_STICK_DATE,");
                    insertSql.Append("NIFUDA_COMPLETE_DATE,");
                    insertSql.Append("REMARKS,");
                    insertSql.Append("USER_ID,");
                    insertSql.Append("INPUT_LOCATION_CD,");
                    insertSql.Append("CREATED_DATE,");
                    insertSql.Append("UPDATED_DATE,");
                    insertSql.Append("ORIGIN_NIFUDA_CD,");
                    insertSql.Append("SHUKKA_INST_NIFUDA_SOURCE");
                    insertSql.Append(")");
                    insertSql.Append("VALUES( ");
                    insertSql.Append("TRN_NIFUDA_SEQ.NEXTVAL,");
                    insertSql.Append("'" + ProcessPlanData.PROCESS_PLAN_CD_HOST + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + ProcessPlanData.CUSTOMER_CD + "',");
                    insertSql.Append("'" + ProcessPlanData.RECEIVER_CD + "',");
                    insertSql.Append("'" + ProcessPlanData.INTERNAL_NAME_CD + "',");
                    insertSql.Append("'" + ProcessPlanData.PROD_CONDITION_CD + "',");
                    insertSql.Append("'" + ProcessPlanData.DEPARTMENT_GROUP_CD + "',");
                    insertSql.Append("'" + ProcessPlanData.INPUT_LOCATION_CD + "',");
                    insertSql.Append("'" + "1" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + nifudaDetails[i].NBNNO_NBNGYO + "',");//14
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + ProcessPlanData.CHUUBAN + "',");
                    insertSql.Append("'" + ProcessPlanData.GYOUBAN_HOST + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + ProcessPlanData.CHARGE + "',");
                    insertSql.Append("'" + ProcessPlanData.PROD_NAME_CUST + "',");
                    insertSql.Append("'" + ProcessPlanData.PROD_SHAPE + "',");
                    insertSql.Append("'" + ProcessPlanData.PROD_DIM_D + "',");
                    insertSql.Append("'" + ProcessPlanData.PROD_DIM_W + "',");//24
                    insertSql.Append("'" + ProcessPlanData.PROD_DIM_OTHER_STRING + "',");
                    insertSql.Append("'" + ProcessPlanData.DIM_TOL_MIN + "',");
                    insertSql.Append("'" + ProcessPlanData.DIM_TOL_MAX + "',");
                    insertSql.Append("'" + ProcessPlanData.DIM_TOL_MIN_CHR + "',");
                    insertSql.Append("'" + ProcessPlanData.DIM_TOL_MAX_CHR + "',");
                    //insertSql.Append("'" + nifudaDetails[i].DIM_TOL_MIN_CHR + nifudaDetails[i].DIM_TOL_MIN + "', ");
                    //insertSql.Append("'" + nifudaDetails[i].DIM_TOL_MAX_CHR + nifudaDetails[i].DIM_TOL_MAX + "' ,");
                    insertSql.Append("'" + ProcessPlanData.PROD_LENGTH + "',");
                    insertSql.Append("'" + ProcessPlanData.LENGTH_OTHER + "',");
                    insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MIN + "',");
                    insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MAX + "',");
                    insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MIN_CHR + "',");
                    insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MAX_CHR + "',");//35
                    insertSql.Append("'" + ProcessPlanData.CHAMFERING + "',");
                    insertSql.Append("'" + ProcessPlanData.CORNER_RADIUS + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "0" + "',");//invoice generated flag
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "',");//52
                    if (nifudaDetails[i].USER_ID != 0)
                    {
                        insertSql.Append("'" + nifudaDetails[i].USER_ID + "',");
                    }
                    else
                    {
                        insertSql.Append("'" + "" + "',");
                    }
                    insertSql.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + nifudaDetails[i].USER_ID + ") ,");
                    //insertSql.Append(" TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd'),");
                    //insertSql.Append(" TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd'),");
                    insertSql.Append("SYSDATE,");
                    insertSql.Append("SYSDATE, ");
                    insertSql.Append("'" + "" + "',");
                    insertSql.Append("'" + "" + "'");
                    insertSql.Append(")");
                    ExecuteNonQuery(insertSql.ToString());
                    responce = "Data Inserted Successfully";
                    //return result;

                    //}
                    //else
                    //{
                    //    GridSearchFields message = new GridSearchFields()
                    //    { CHUUBAN_GYOUBAN = nifudaDetails[i].NBNNO_NBNGYO, RESPONCE = "Tag No.Are already copied  " };
                    //    result.Add(message);
                    //}
                }
                catch (Exception ex)
                {
                    ex.Data.Add("sql", insertSql.ToString());
                    throw;
                }
            }

            // }


            return responce;
        }

        public List<GridSearchFields> DeleteCopyTagData(List<GridSearchFields> processPlanDetails)
        {
            int count = processPlanDetails.Count;
            List<GridSearchFields> result = new List<GridSearchFields>();
            using (TransactionScope transaction = new TransactionScope())
            {
                // try{
                for (int i = 0; i < count; i++)
                {
                    string nifudaNo = processPlanDetails[i].NBNNO_NBNGYO;
                    int processPlanCd = Convert.ToInt32(processPlanDetails[i].PROCESS_PLAN_CD);

                    var responce = DeleteProcessPlanDetails(nifudaNo);
                    GridSearchFields message = new GridSearchFields()
                    { NBNNO_NBNGYO = nifudaNo, RESPONCE = responce };
                    result.Add(message);
                }
                transaction.Complete();
            }

            return result;
        }



        public string DeleteProcessPlanDetails(string nifudaNO)
        {
            StringBuilder deleteSqlForNifuda = new StringBuilder();
            try
            {

                int nifuda_Cd = 0;
                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append(" SELECT NIFUDA_CD ");
                selectQuery.Append(" FROM TRN_NIFUDA  ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN ON TRN_PROCESS_PLAN.PROCESS_PLAN_CD = TRN_NIFUDA.PROCESS_PLAN_CD AND TRN_PROCESS_PLAN.PROCESS_PLAN_CMPL_DATE IS NULL ");
                selectQuery.Append(" WHERE NIFUDA_NO = '" + nifudaNO + "'  ");
                nifuda_Cd = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

                deleteSqlForNifuda = new StringBuilder();
                deleteSqlForNifuda.Append("DELETE  FROM  TRN_NIFUDA  ");
                deleteSqlForNifuda.Append("WHERE ");
                deleteSqlForNifuda.Append("NIFUDA_CD ='" + nifuda_Cd + "' ");
                ExecuteNonQuery(deleteSqlForNifuda.ToString());
                resultData = "Data Deleted Successfully";
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.Message.Contains("ORA-02292"))
                {
                    resultData = "Final Inspection done";
                }
               
            }
            return resultData;
        }
    }
}

