using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Collections;
using System.Xml.Linq;
using static TSWebApplication.Domain.Utility.Constants;
using System.Configuration;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class AllCommonRepository : BaseRepository, IAllCommonRepository
    {
        private readonly string _connectionString;
        private readonly IHostEnvironment _hostEnvironment;
        
        public AllCommonRepository(IConfiguration _configuration, IHostEnvironment hostEnvironment) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _hostEnvironment = hostEnvironment;
        }
        public List<NameValue> GetProcessDefectList(int kubunCD)
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PD.PROCESS_DEFECT_CD AS CD ");
            //selectQuery.Append(" ,PD.ENGLISH_NAME AS NAME ");
            selectQuery.Append(" ,PD.PROC_DEFECT_LONG AS DEFECT_NAME_JP ");
            selectQuery.Append(" ,PD.OTHER_NAME AS DEFECT_NAME_EN ");
            selectQuery.Append(" FROM MST_PROCESS_DEFECT PD");
            selectQuery.Append(" WHERE PD.IN_ACTIVE_FLAG =0 ");
            selectQuery.Append(" AND PD.PROCESS_TYPE = " + kubunCD);
            selectQuery.Append(" ORDER BY LOWER (PD.PROC_DEFECT_LONG)");
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public MST_MACHINE GetMachineNameByID(long machineCd)
        {
            MST_MACHINE result = new MST_MACHINE();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * From  MST_MACHINE");
            selectQuery.Append(" WHERE MACHINE_CD = '" + machineCd + "' ");
            // selectQuery.Append("AND IN_ACTIVE_FLAG = 0");     //only Active Machine is selected Pradeep M.2019-09-11
            result = ExecuteQuerySingleResult<MST_MACHINE>(selectQuery.ToString());
            return result;
        }
        public NameValue GetEmployeeNameByID(long employeeCd)

        {
            NameValue result = new NameValue();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT EM.SHORT_NAME AS YOBINA, EM.EMPLOYEE_CD AS CD,  EM.IN_ACTIVE_FLAG  FROM MST_EMPLOYEE EM");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG ");
            //selectQuery.Append(" ON DG.EMPLOYEE_CD =EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE EM.EMPLOYEE_CD = '" + employeeCd + "' ");
            result = ExecuteQuerySingleResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public int GetEmployeeNameActive(long employeeCd)

        {
            int Activeflag;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT count(EM.EMPLOYEE_CD)  FROM MST_EMPLOYEE EM");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG ");
            //selectQuery.Append(" ON DG.EMPLOYEE_CD =EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE EM.EMPLOYEE_CD = '" + employeeCd + "' AND EM.IN_ACTIVE_FLAG ='" + 1 + "' ");
            Activeflag = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return Activeflag;

            //public int checkvalidFurnaceNo(string furnaceNo)
            //{
            //    StringBuilder selectQuery = new StringBuilder();
            //    selectQuery.Append(" SELECT count(FURNACE_NO)  FROM TRN_HEAT_TREATMENT");

            //    selectQuery.Append(" WHERE FURNACE_NO ='" + furnaceNo + "'  ");
            //    var query = selectQuery.ToString();

            //    var FurnaceNo = Convert.ToInt32(ExecuteScalar(query.ToString()));
            //    return FurnaceNo;


            //}





        }
        public MessageDetails MessageDetailsInsert(MessageDetails messageDetails)
        {
            MessageDetails result = new MessageDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT  M.MSG_CD, M.PROCESS_PLAN_CD, M.MACHINE_CD , M.PROCESS_KUBUN_ID, M.OPERATOR_CD  ,M.MSG_DATE , M.MSG_CONTENTS, M.USER_ID, M.CREATED_DATE, M.UPDATED_DATE FROM TRN_MESSAGE M ");
            selectQuery.Append("WHERE MSG_CD = (SELECT MAX(MSG_CD) FROM TRN_MESSAGE)");

            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_MESSAGE ( ");
            //insertQuery.Append("  MSG_CD ");
            insertQuery.Append(" PROCESS_PLAN_CD ");
            insertQuery.Append(", MACHINE_CD ");
            insertQuery.Append(", PROCESS_KUBUN_ID ");
            insertQuery.Append(", OPERATOR_CD ");
            insertQuery.Append(", MSG_DATE ");
            insertQuery.Append(", MSG_CONTENTS ");
            insertQuery.Append(", INPUT_LOCATION_CD ");
            insertQuery.Append(", USER_ID ");
            insertQuery.Append(", CREATED_DATE ");
            insertQuery.Append(", UPDATED_DATE ");

            insertQuery.Append(") VALUES (");
            //insertQuery.Append(" MESSAGE_SEQ.NEXTVAL,");
            insertQuery.Append("'" + messageDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + messageDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + messageDetails.PROCESS_KUBUN_ID + "'");
            insertQuery.Append(",'" + messageDetails.OPERATOR_CD + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(",'" + messageDetails.MSG_CONTENTS + "'");
            insertQuery.Append(",'" + messageDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + messageDetails.USER_ID + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());
            result = ExecuteQuerySingleResult<MessageDetails>(selectQuery.ToString());
            return result;
        }
        public List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc)
        {
            List<MessageDetails> result = new List<MessageDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("   MSG_DATE ");
            selectQuery.Append(" , TO_CHAR( MSG_DATE,'yyyy/mm/dd HH24:MI' ) AS MSG_DATE_STRING");
            selectQuery.Append(" , MSG_CONTENTS");
           // selectQuery.Append(" , MAC.SHORT_NAME  AS MACHINE_NAME  ");
            selectQuery.Append(" , MAC.MACHINE_NAME  AS MACHINE_NAME  "); 
            selectQuery.Append(" , EMP.SHORT_NAME AS  OPERATOR_NAME ");
            selectQuery.Append(" , PKU.KUBUN_NAME AS PROCESS_NAME_JP ");
            selectQuery.Append(" , PKU.KUBUN_NAME_EN AS PROCESS_NAME_EN");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MESSAGE  MSG ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC  ON MAC.MACHINE_CD =MSG.MACHINE_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=MSG.OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PKU ON MSG.PROCESS_KUBUN_ID=PKU.KUBUN_ID");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD=" + processPlanCD);
            if (isSortDesc == true)
            {
                selectQuery.Append(" ORDER BY MSG_DATE DESC");
            }
            else
            {
                selectQuery.Append(" ORDER BY MSG_DATE ASC");
            }
            result = ExecuteQueryMultiResult<MessageDetails>(selectQuery.ToString());
            return result;
        }
        public void MessageDetailsDelete(long messageCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM TRN_MESSAGE ");
            deleteQuery.Append(" WHERE MSG_CD = ");
            deleteQuery.Append(messageCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }
        public MessageDetails MessageDetailsUpdate(MessageDetails messageDetails)
        {
            MessageDetails result = new MessageDetails();

            //StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append("SELECT  M.MSG_CD, M.PROCESS_PLAN_CD, M.MACHINE_CD , M.PROCESS_KUBUN_ID, M.OPERATOR_CD  ,M.MSG_DATE , M.MSG_CONTENTS, M.USER_ID, M.CREATED_DATE, M.UPDATED_DATE FROM TRN_MESSAGE M ");
            //selectQuery.Append("WHERE MSG_CD = " + messageDetails.MSG_CD);

            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE TRN_MESSAGE SET ");
            //updateQuery.Append(" MSG_DATE = SYSDATE");            
            updateQuery.Append(" MSG_CONTENTS ='" + messageDetails.MSG_CONTENTS + "'");
            updateQuery.Append(", PROCESS_PLAN_CD = '" + messageDetails.PROCESS_PLAN_CD + "'");
            updateQuery.Append(", USER_ID = '" + messageDetails.USER_ID + "'");
            updateQuery.Append(", PROCESS_KUBUN_ID = '" + messageDetails.PROCESS_KUBUN_ID + "'");
            updateQuery.Append(",  MACHINE_CD ='" + messageDetails.MACHINE_CD + "'");
            updateQuery.Append(", OPERATOR_CD ='" + messageDetails.OPERATOR_CD + "'");
            updateQuery.Append(", INPUT_LOCATION_CD ='" + messageDetails.INPUT_LOCATION_CD + "'");
            updateQuery.Append(", UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE MSG_CD = " + messageDetails.MSG_CD);
            ExecuteNonQuery(updateQuery.ToString());
            result = ExecuteQuerySingleResult<MessageDetails>(updateQuery.ToString());
            return result;
        }

        public ECDetails GetOperatorDetailsById(long employeeCd)  // Added By Komal K 2022 Nov 15
        {
            ECDetails result = new ECDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM MST_EMPLOYEE EM");
            selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG ");
            selectQuery.Append(" ON DG.EMPLOYEE_CD =EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE EM.EMPLOYEE_CD = '" + employeeCd + "' ");
            result = ExecuteQuerySingleResult<ECDetails>(selectQuery.ToString());
            return result;
        }
        public List<NameValue> GetOperatorNameList(string name, int operatorCD)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append("  CD ");
                selectQuery.Append(" ,NAME ");
                selectQuery.Append(" ,YOBIGANA ");
                selectQuery.Append(" ,YOBINA ");
                selectQuery.Append(" ,SHORT_NAME ");
                selectQuery.Append(" ,IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ( ");
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                selectQuery.Append(" WHERE EMP.EMPLOYEE_CD = " + operatorCD);
                
                selectQuery.Append(" UNION ");
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                //  selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD");
                selectQuery.Append(" WHERE EMP.IN_ACTIVE_FLAG = 0");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
                selectQuery.Append(" ) " );
                selectQuery.Append(" ORDER BY LOWER (YOBIGANA),LOWER (YOBINA),LOWER (SHORT_NAME) ");
            }
            else
            {
               

                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,EMP.SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP ");
                // selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD"); commented as per confirmation by Nilesh Sir
                selectQuery.Append(" WHERE LOWER (EMP.YOBINA) LIKE lower('%" + name + "%') ");
                selectQuery.Append(" AND EMP.IN_ACTIVE_FLAG = 0");
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME)");
            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public List<NameValue> GetOperatorList()
        {
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
           
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME) ");
           
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public List<NameValue> GetMachineNameList(string? machineType)
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();

            if (!string.IsNullOrEmpty(machineType))
            {
                query.Append(" SELECT MACHINE_CD AS CD ");
                query.Append(" ,MACHINE_NAME AS NAME ");
                query.Append(" ,IN_ACTIVE_FLAG ");
                query.Append(" FROM MST_MACHINE ");
                query.Append(" WHERE ");
                query.Append(" MACHINE_TYPE IN ( " + machineType + " ) ");
                query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
                result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            }
            else
            {
                query.Append(" SELECT MACHINE_CD AS CD ");
                query.Append(" ,MACHINE_NAME AS NAME ");
                query.Append(" ,IN_ACTIVE_FLAG ");
                query.Append(" FROM MST_MACHINE ");
                //query.Append(" WHERE IN_ACTIVE_FLAG = 0");
                query.Append(" ORDER BY LOWER (MACHINE_NAME) ");
                result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            }
            return result;
        }

        public List<NameValue> GetConditionNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT CONDITION_CD AS CD,");
            query.Append(" CONDITION_DESCRIPTION AS NAME,");
            query.Append(" IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_CONDITION ");
            query.Append(" ORDER BY (CONDITION_DESCRIPTION) ASC ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        public List<string> GetChuubanListForInvaidCheck(string chuubanList)
        {
            string[] chuuban;
            chuuban = chuubanList.Split(',');
            List<string> result = new List<string>();
            StringBuilder selectQuery = new StringBuilder();
            foreach (string ChuubanNum in chuuban)
            {
                if (ChuubanNum.Length > 2)
                {
                    selectQuery.Append("SELECT DISTINCT (CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00'))) AS CHUUBAN FROM TRN_PROCESS_PLAN");
                    selectQuery.Append(" WHERE LOWER(CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00')))  IN (" + ChuubanNum + ")");
                    // selectQuery.Append("SELECT COUNT(CHUUBAN) FROM TRN_PROCESS_PLAN");
                    // selectQuery.Append(" WHERE CHUUBAN IN (" + ChuubanNum + ")");
                    var dt = ExecuteQueryDataTable(selectQuery.ToString());

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        result.Add(Convert.ToString(dt.Rows[i][0]));
                    }
                }
            }

            return result;
        }
        public List<string> GetCompletedChuubanList(string chuubanList)
        {
            List<string> result = new List<string>();
            StringBuilder selectCompleteChuubanQuery = new StringBuilder();
            selectCompleteChuubanQuery.Append("SELECT DISTINCT (CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00'))) AS CHUUBAN FROM TRN_PROCESS_PLAN");
            selectCompleteChuubanQuery.Append(" WHERE LOWER(CHUUBAN||trim(LEADING ' ' FROM To_Char(GYOUBAN,'00')))  IN (" + chuubanList + ")");
            selectCompleteChuubanQuery.Append(" AND PROCESS_PLAN_CMPL_DATE IS NOT Null");
            var dt = ExecuteQueryDataTable(selectCompleteChuubanQuery.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(Convert.ToString(dt.Rows[i][0]));
            }
            return result;
        }
        public List<NameValue> GetFutouNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT FUTOU_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(", IN_ACTIVE_FLAG");
            query.Append(" FROM MST_FUTOU ORDER BY LOWER (YOBINA) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        public List<NameValue> GetFutouNameList(string searchText)
        {
            StringBuilder query = new StringBuilder();
            try
            {
                query.Append(" SELECT FUTOU_CD AS CD ");
                query.Append(" ,SHORT_NAME AS NAME ");
                query.Append(" FROM MST_FUTOU ");
                query.Append(" WHERE MST_FUTOU.IN_ACTIVE_FLAG = 0");
                query.Append(" AND (SHORT_NAME) LIKE lower('" + searchText + "%') ");
                return ExecuteQueryMultiResult<NameValue>(query.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", query.ToString());
                throw;
            }
        }
        public List<NameValue> GetAllMochikomisakiActiveNameList(string searchText)
        {
            StringBuilder selectQuery = new StringBuilder();
            try
            {

                selectQuery.Append(" SELECT MCK.MOCHIKOMISAKI_CD AS CD ");
                selectQuery.Append(" ,MCK.SHORT_NAME AS NAME ");
                selectQuery.Append(", IN_ACTIVE_FLAG");
                selectQuery.Append(" FROM MST_MOCHIKOMISAKI MCK ");
                selectQuery.Append(" WHERE MCK.IN_ACTIVE_FLAG =0 AND lower(MCK.YOBINA) like lower('" + searchText.Trim() + "%') ");
                selectQuery.Append(" ORDER BY LOWER (YOBIGANA),LOWER (YOBINA),LOWER (SHORT_NAME) ");

                return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }

        }
        public List<NameValue> GetAllMochikomisakiActiveNameList()
        {

            StringBuilder selectQuery = new StringBuilder();
            try
            {

                selectQuery.Append(" SELECT MCK.MOCHIKOMISAKI_CD AS CD ");
                selectQuery.Append(" ,MCK.SHORT_NAME AS NAME ");
                selectQuery.Append(", IN_ACTIVE_FLAG");
                selectQuery.Append(" FROM MST_MOCHIKOMISAKI MCK ");
                selectQuery.Append(" WHERE MCK.IN_ACTIVE_FLAG =0 ");

                return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }

        }
        public List<NameValue> GetCustomerNameList(string name)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT DISTINCT ");
                selectQuery.Append(" (CUST.CUSTOMER_CD ) AS CD,");
                selectQuery.Append(" CUST.SHORT_NAME AS NAME,");
                selectQuery.Append(" CUST.YOBIGANA,");
                selectQuery.Append(" CUST.YOBINA,");
                selectQuery.Append(" CUST.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" MST_CUSTOMER CUST");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" CUST.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(CUST.YOBIGANA), ");
                selectQuery.Append(" LOWER(CUST.YOBINA), ");
                selectQuery.Append(" LOWER(CUST.SHORT_NAME) ");
            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT ");
                selectQuery.Append(" (CUST.CUSTOMER_CD ) AS CD,");
                selectQuery.Append(" CUST.SHORT_NAME AS NAME,");
                selectQuery.Append(" CUST.YOBIGANA,");
                selectQuery.Append(" CUST.YOBINA,");
                selectQuery.Append(" CUST.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" MST_CUSTOMER CUST");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" LOWER(CUST.YOBINA) LIKE LOWER('" + name + "%') ");
                selectQuery.Append(" AND CUST.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(CUST.YOBIGANA), ");
                selectQuery.Append(" LOWER(CUST.YOBINA), ");
                selectQuery.Append(" LOWER(CUST.SHORT_NAME) ");
            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/03    
        /// Used to Maker bind in dropdown list in MST_Master Table

        /// </summary>        
        /// <returns>
        /// MAKER_CD ,SHORT_NAME ,IN_ACTIVE_FLAG
        /// </returns>  
        public List<NameValue> GetMakerNameAndCDList(string name)
        {
            StringBuilder selectQuery = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    selectQuery.Append(" SELECT MAKER_CD AS CD,MAKER_NAME AS NAME,IN_ACTIVE_FLAG ");
                    selectQuery.Append(" FROM MST_MAKER WHERE IN_ACTIVE_FLAG = 0");
                    selectQuery.Append(" ORDER BY DISPLAY_PRIORITY ASC ");
                }
                else
                {
                    selectQuery.Append(" SELECT MAKER_CD AS CD,MAKER_NAME AS NAME,IN_ACTIVE_FLAG ");
                    selectQuery.Append(" FROM MST_MAKER WHERE IN_ACTIVE_FLAG = 0");
                    selectQuery.Append(" AND LOWER(MAKER_NAME) LIKE LOWER('" + name + "%') ");
                    selectQuery.Append(" ORDER BY DISPLAY_PRIORITY ASC ");
                }
                return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }


        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/03    
        /// Used to Maker bind in dropdown list in MST_Master Table

        /// </summary>        
        /// <returns>
        /// MAKER_CD ,SHORT_NAME ,IN_ACTIVE_FLAG
        /// </returns>  
        public List<NameValue> GetMakerNameAndCDList()
        {
            StringBuilder selectQuery = new StringBuilder();
            try
            {
                
                    selectQuery.Append(" SELECT MAKER_CD AS CD,MAKER_NAME AS MAKER_NAME,IN_ACTIVE_FLAG ");
                    selectQuery.Append(" FROM MST_MAKER WHERE IN_ACTIVE_FLAG = 0");
                    selectQuery.Append(" ORDER BY DISPLAY_PRIORITY ASC ");
                
                return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }


        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/03    
        /// Used to Maker bind in dropdown list in MST_Master Table

        /// </summary>        
        /// <returns>
        /// MAKER_CD ,SHORT_NAME ,IN_ACTIVE_FLAG
        /// </returns>  
        public List<MstConfiguration> GetFTPDetail()
        {
            StringBuilder selectQuery = new StringBuilder();
            try
            {

                selectQuery.Append(" SELECT KEY, VALUE ");
                selectQuery.Append(" FROM MST_CONFIGURATION ");
                selectQuery.Append(" WHERE KEY IN('MILLSHEET_FTPPATH','MILLSHEET_FTPPASSWORD','MILLSHEET_FTPUSERNAME') ");

                return ExecuteQueryMultiResult<MstConfiguration>(selectQuery.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }

        public List<NameValue> GetMaterialMakerNameList()
        {
            StringBuilder selectQuery = new StringBuilder();
            try
            {

                selectQuery.Append(" SELECT MNM.MATL_NAME_MAKER_CD AS CD ");
                selectQuery.Append(" , MNM.MATL_MAKER_NAME ||'  ' || MKR.SHORT_NAME  AS NAME ");
                selectQuery.Append(" , MNM.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_MATL_NAME_MAKER MNM");
                selectQuery.Append(" INNER JOIN MST_MAKER MKR on MNM.MAKER_CD = MKR.MAKER_CD");
                selectQuery.Append(" ORDER BY LOWER (MATL_MAKER_NAME) ");

                return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ex.Data.Add("sql", selectQuery.ToString());
                throw;
            }
        }
        public int GetUserLocationById(int userID)
        {
            int locationCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD");
            query.Append(" FROM MST_USER_LOGIN ");
            query.Append(" WHERE USER_ID=" + userID);
            locationCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return locationCD;
        }

        public List<NameValue> GetNextProcesKubun(int processTypeCD)
        {

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" NXT.PROCESS_TYPE_CD,");
            query.Append(" NXT.NEXT_PROCESS_TYPE_CD AS PROCESS_KUBUN_CD,");
            query.Append(" KUB.KUBUN_NAME AS KUBUN_NAME_JP,");
            query.Append(" KUB.KUBUN_NAME_EN AS KUBUN_NAME_EN");
            query.Append(" FROM MST_NEXT_PROCESS_CONFIGURATION NXT ");
            query.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = NXT.NEXT_PROCESS_TYPE_CD");
            query.Append(" WHERE PROCESS_TYPE_CD=" + processTypeCD);
            query.Append(" ORDER BY NXT.SHOW_PRIORITY ASC");

            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            //return result;

            if (result.Count == 0)
            {
                query = new StringBuilder();
                query.Append(" SELECT ");
                query.Append(" KUB.KUBUN_ID AS PROCESS_KUBUN_CD,");
                query.Append(" KUB.KUBUN_NAME AS KUBUN_NAME_JP,");
                query.Append(" KUB.KUBUN_NAME_EN AS KUBUN_NAME_EN,");
                query.Append(" CASE WHEN KUB.KUBUN_NAME IS NOT NULL THEN 1 END AS NEXT_PROCESS_FLAG ");
                query.Append(" FROM MST_PROCESS_KUBUN KUB ");
                query.Append(" WHERE KUB.KUBUN_ID NOT IN(810, 820, 830, 835) "); //this 4 kubun_id remove from next process list for without taba condition on 13/10/2025 
                query.Append(" ORDER BY KUB.KUBUN_ID ASC");
                result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            }
            return result;
        }
        public void DeleteWIPStatusData(WipStatusDetails wipDetails)
        {
            //StringBuilder deleteQuery = new StringBuilder();
            //deleteQuery.Append(" DELETE FROM  TRN_WIP_STATUS ");
            //deleteQuery.Append(" WHERE PROCESS_PLAN_CD =" + processPlanCD + " AND TABA_NO = " + thabaNo);
            ExecuteStoredProcedure(wipDetails);

        }

        public void DeleteWIPStatusDataPreProcess(long processPlanCD, short thabaNo)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_WIP_STATUS ");
            deleteQuery.Append(" WHERE PROCESS_PLAN_CD =" + processPlanCD + " AND TABA_NO = " + thabaNo);
            ExecuteNonQuery(deleteQuery.ToString());
           
        }
        
        public void InsertWIPStatusData(WipStatusDetails request)
        {
            //foreach (WipStatusDetails result in request)
            //{
            

            if (request.TABA_NO > 0)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(" DELETE FROM  TRN_WIP_STATUS ");
                deleteQuery.Append(" WHERE PROCESS_PLAN_CD =" + request.PROCESS_PLAN_CD + " AND TABA_NO IN('" + request.TABA_NO + "','0') ");
                ExecuteNonQuery(deleteQuery.ToString());
            }
            if (request.MSG_CD == 0)
            {
                request.MSG_CD = null;
            }
                if (request.FINISHED_QTY == null)
            {
                request.FINISHED_BARS = null;
                request.UNIT = null;
            }

            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_WIP_STATUS(");
            insertQuery.Append(" WIP_STATUS_CD,");
            insertQuery.Append(" PROCESS_PLAN_CD,");
            insertQuery.Append(" LOCATION_CD,");
            insertQuery.Append(" TABA_GROUP_NO,");
            insertQuery.Append(" TABA_NO,");
            insertQuery.Append(" WIP_NO_OF_UNITS,");
            insertQuery.Append(" UNIT,");
            insertQuery.Append(" WIP_QTY,");
            insertQuery.Append(" PRE_PROCESS_DATE,");
            insertQuery.Append(" PRE_PROCESS,");
            insertQuery.Append(" PRE_PROCESS_DECISION,");
            insertQuery.Append(" NEXT_PROCESS,");
            insertQuery.Append(" MSG_CD, ");
            insertQuery.Append(" PROCESS_START_DATE, ");
            insertQuery.Append(" NIFUDA_CD, ");
            insertQuery.Append(" START_EMPLOYEE_CD, ");
            insertQuery.Append(" END_EMPLOYEE_CD");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("TRN_WIP_STATUS_SEQ.NEXTVAL");
            insertQuery.Append(",'" + request.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + request.LOCATION_CD + "'");
            insertQuery.Append(",'" + request.TABA_GROUP_NO + "'");
            insertQuery.Append(",'" + request.TABA_NO + "'");
            insertQuery.Append(",'" + request.FINISHED_BARS + "'");
            insertQuery.Append(",'" + request.UNIT + "'");
            insertQuery.Append(",'" + request.FINISHED_QTY + "'");
            insertQuery.Append(request.PROCESS_END_DATE != null ? ", TO_DATE('" + (Convert.ToDateTime(request.PROCESS_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" : ", NULL");
            if (request.PROCESS_END_DATE != null)
            {
                insertQuery.Append(",'" + request.PROCESS_KUBUN_CD + "'");
            }
            else
            {
                insertQuery.Append(",'" + null + "'");
            }
            insertQuery.Append(",'" + request.PRE_PROSESS_DECISION + "'");
            if (request.PROCESS_END_DATE == null)
            {
                insertQuery.Append(",'" + request.PROCESS_KUBUN_CD + "'");
            }
            else
            {
                insertQuery.Append(",'" + request.NEXT_PROCESS + "'");
            }
            insertQuery.Append(",'" + request.MSG_CD + "'");
            insertQuery.Append(", TO_DATE('" + (Convert.ToDateTime(request.PROCESS_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(request.NIFUDA_CD > 0 ? ",'" + request.NIFUDA_CD + "'" : ", NULL");
            insertQuery.Append(",'" + request.START_OPERATOR_CD + "'");
            insertQuery.Append(",'" + request.END_OPERATOR_CD + "'");
            insertQuery.Append(") ");

            ExecuteNonQuery(insertQuery.ToString());
        }
        //}
        //public string ExportCSV(DataTable dt, string fileName)
        //{
        //    string contentRootPath = _hostEnvironment.ContentRootPath;
        //    string tempDirectory = "";

        //    DateTime date = DateTime.Now;
        //    var hour = date.Hour;
        //    var minute = date.Minute;
        //    var second = date.Second;

        //    var hrs = hour.ToString();
        //    var minutes = minute.ToString();
        //    var seconds = second.ToString();


        //    if (hrs.Length < 2)
        //    {
        //        hrs = "0" + hrs;
        //    }
        //    else
        //    {
        //        hrs = hrs;
        //    }
        //    if (minutes.Length < 2)
        //    {
        //        minutes = "0" + minutes;
        //    }
        //    else
        //    {
        //        minutes = minutes;
        //    }
        //    if (seconds.Length < 2)
        //    {
        //        seconds = "0" + seconds;
        //    }
        //    else
        //    {
        //        seconds = seconds;
        //    }
        //    var time = hrs + "" + minutes + "" + seconds;


        //    string formattedDate = date.ToString("yyyyMMdd");
        //    string fileNameFormat = fileName + "_" + formattedDate + time;
        //    tempDirectory = Path.Combine(contentRootPath, "wwwroot", "CsvFile\\" + fileNameFormat + ".csv");
        //    DataTable dtCSV = dt;
        //    UIHelper.ToCSV(dtCSV, tempDirectory);
        //    var message = "/CsvFile/" + fileNameFormat + ".csv";

        //    return message;
        //}
        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/04/17    
        /// Used to export CSV file from all status screen
        /// We have used parameter such as 
        /// SQL 
        /// FileName
        /// DataHeader
        /// Which are taken from respective screens used to generate CSV File and return file path to download it        
        /// </summary>        
        /// <returns>
        /// file path
        /// </returns>
        public string ExportCSV(string sql, string PLSqlName, string outPutTable, string fileName, DataTable dataHeader)
        {
            var message = "";

            DataTable result = ExecuteQueryDataTable(sql);

            if (result.Rows.Count > 0)
            {

                string targetColumn = "SALES_FC_DONE_DATE";

                if (result.Columns.Contains(targetColumn) && result.Columns[targetColumn].DataType == typeof(DateTime))
                {
                    foreach (DataRow row in result.Rows)
                    {
                        var value = row[targetColumn];

                        if (value != DBNull.Value && (DateTime)value == DateTime.MinValue)
                        {
                            row[targetColumn] = DBNull.Value; ; // Replace with blank
                        }
                    }
                }

                for (int i = 0; i < dataHeader.Columns.Count; i++)
                {
                    result.Columns[i].ColumnName = dataHeader.Columns[i].ColumnName;
                }

                //foreach (DataRow data in result.Rows)
                //{
                //    dataHeader.Rows.Add(data);
                //}

                string contentRootPath = _hostEnvironment.ContentRootPath;
                string tempDirectory = "";

                //var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                //string folderName = "CsvFile"; 
                //string folderPath = Path.Combine(wwwRootPath, folderName);

                // Check if the folder exists; if not, create it
                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}

                //tempDirectory = Path.Combine(folderPath + "\\" + fileName + ".csv");

                tempDirectory = Path.Combine(contentRootPath, "wwwroot", "CsvFile\\" + fileName + ".csv");
                DataTable dtCSV = result;
                if(dtCSV.Columns.Contains("NEXT_PROCESS_INVISIBLE"))
                {
                    dtCSV.Columns.Remove("NEXT_PROCESS_INVISIBLE");// This is used to hide column in CSV file but used in sort functionality for Next Process Waiting Status Screen

                }
                UIHelper.ToCSV(dtCSV, tempDirectory);
                 message = "/CsvFile/" + fileName + ".csv";
            }
            else
            {
                message = "Data not available";
            }

            return message;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/06/03       
        /// used by all Screen to check validation for chubban
        /// check chubban data  with PROCESS_PLAN_START_DATE into table TRN_PROCESS_PLAN 
        /// check chubban data count  into table TRN_PROCESS_PLAN 
        /// check chubban data count with PROCESS_PLAN_CMPL_DATE into table TRN_PROCESS_PLAN
        /// </summary>
        /// <returns>
        /// return 1 for Success
        /// return 2 for OrNo not found
        /// return 3  for OrNo not started from Handover
        /// return 4 for complete date is null record not found for OrNo
        /// </returns>
        /// 
        public CommonFields ValidateChubban(string chuuban, short gyuban)
        {
            int chubbanCount;
            int chubbanCountNull;
            int dateCnt;
            int result;
            string processStartDate;
            CommonFields commonFields = new CommonFields();
            StringBuilder selectQueryForChubbanCnt = new StringBuilder();
            selectQueryForChubbanCnt.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
            selectQueryForChubbanCnt.Append(" WHERE ");
            selectQueryForChubbanCnt.Append(" CHUUBAN = '" + chuuban + "' AND  GYOUBAN ='" + gyuban + "'  ");
            chubbanCount = Convert.ToInt32(ExecuteScalar(selectQueryForChubbanCnt.ToString()));
            if (chubbanCount > 0)
            {
                StringBuilder selectQueryForChubbanCountNull = new StringBuilder();
                selectQueryForChubbanCountNull.Append("SELECT  COUNT(*) FROM TRN_PROCESS_PLAN ");
                selectQueryForChubbanCountNull.Append(" WHERE ");
                selectQueryForChubbanCountNull.Append(" CHUUBAN = '" + chuuban + "' AND  GYOUBAN ='" + gyuban + "' AND PROCESS_PLAN_CMPL_DATE IS NULL  ");
                chubbanCountNull = Convert.ToInt32(ExecuteScalar(selectQueryForChubbanCountNull.ToString()));

                if(chubbanCountNull > 1)
                {
                    commonFields.RESPONCE = 5;
                }
                else
                {
                    StringBuilder selectQueryForProcessStartDate = new StringBuilder();
                    selectQueryForProcessStartDate.Append("SELECT TO_CHAR(PROCESS_PLAN_START_DATE,'YYYY/MM/DD') AS PROCESS_PLAN_START_DATE,GET_HYPHENATED_CHUUBAN_GYOUBAN(CHUUBAN_YEAR || CHUUBAN,GYOUBAN) AS PP_CHUUBAN_GYOUBAN FROM TRN_PROCESS_PLAN ");
                    selectQueryForProcessStartDate.Append(" WHERE ");
                    selectQueryForProcessStartDate.Append(" CHUUBAN = '" + chuuban + "' AND  GYOUBAN ='" + gyuban + "' AND PROCESS_PLAN_CMPL_DATE IS NULL  ");
                    commonFields = ExecuteQuerySingleResult<CommonFields>(selectQueryForProcessStartDate.ToString());

                    if (chubbanCount > 0 && commonFields == null)
                    {
                        commonFields = new CommonFields();
                        commonFields.RESPONCE = 4;
                    }
                    else if (commonFields.PROCESS_PLAN_START_DATE == null)
                    {
                        commonFields.RESPONCE = 3;
                    }
                    else
                    {
                        commonFields.RESPONCE = 1;
                    }
                }
                    
            }
            else
            {
                commonFields.RESPONCE = 2;
            }
            return commonFields;
        }
        /// <summary>
        /// CREATED BY : Rahul Sutar on 2024/06/10       
        /// used for all Processing screen to separate chuuban and gyouban
        /// chuuban and gyouban seperated by using MST_Configuration tbale as per below KEYS
        /// START_CHECK_DIGIT_POSITION_IN_CHUUBAN , END_CHECK_DIGIT_POSITION_IN_CHUUBAN , START_GYOUBAN_POSITION_IN_CHUUBAN, END_GYOUBAN_POSITION_IN_CHUUBAN
        /// START_ORDER_NO_POSITION_IN_CHUUBAN, END_ORDER_NO_POSITION_IN_CHUUBAN, START_YEAR_POSITION_IN_CHUUBAN, END_YEAR_POSITION_IN_CHUUBAN
        /// </summary>
        /// <returns>
        /// return CHUUBAN and GYOUBAN
        /// </returns>
        /// 
        public CommonFields GetMappedChuubanAndGyouban(string orderNo)
        {
            int startCheckDigit;
            int endCheckDigit;
            int startGyoubanPosition;
            int endGyoubanPosition;
            int startChuubanPosition;
            int endChuubanPosition;
            int startYearPosition;
            int endYearPosition;
            int chubanLenght;

            string digitValue=null;
            string gyoubanValue= null;
            string chuubanValue= null;
            string yearValue= null;

            CommonFields commonFilds = new CommonFields();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" * ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_CONFIGURATION ");

            var result =  ExecuteQueryMultiResult<MstConfiguration>(selectQuery.ToString());

            startCheckDigit = Convert.ToInt32(result.Where(r => r.KEY == "START_CHECK_DIGIT_POSITION_IN_CHUUBAN").Select(c => c.VALUE).SingleOrDefault());
            endCheckDigit = Convert.ToInt32(result.Where(r => r.KEY == "END_CHECK_DIGIT_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());

            startGyoubanPosition= Convert.ToInt32(result.Where(r => r.KEY == "START_GYOUBAN_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());
            endGyoubanPosition = Convert.ToInt32(result.Where(r => r.KEY == "END_GYOUBAN_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());

            startChuubanPosition = Convert.ToInt32(result.Where(r => r.KEY == "START_ORDER_NO_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());
            endChuubanPosition = Convert.ToInt32(result.Where(r => r.KEY == "END_ORDER_NO_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());

            startYearPosition = Convert.ToInt32(result.Where(r => r.KEY == "START_YEAR_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());
            endYearPosition = Convert.ToInt32(result.Where(r => r.KEY == "END_YEAR_POSITION_IN_CHUUBAN").Select(r => r.VALUE).SingleOrDefault());

            chubanLenght = Convert.ToInt32(result.Where(r => r.KEY == "CHUUBAN_LENGTH").Select(r => r.VALUE).SingleOrDefault());

            var getOrderNo=orderNo.Reverse().ToArray();
            try
            {
                
                if (getOrderNo.Length > chubanLenght) 
                {
                    if (startCheckDigit > 0 && endCheckDigit > 0)
                    {
                        for (int i = startCheckDigit; i <= endCheckDigit; i++)
                        {

                            if (string.IsNullOrEmpty(digitValue))
                            {
                                digitValue = Convert.ToString(getOrderNo[i - 1]);
                            }
                            else
                            {
                                digitValue = digitValue + Convert.ToString(getOrderNo[i - 1]);
                            }
                        }

                        digitValue = ReverseChar(digitValue);
                    }

                    if (startGyoubanPosition > 0 && endGyoubanPosition > 0)
                    {

                        for (int i = startGyoubanPosition; i <= endGyoubanPosition; i++)
                        {

                            if (string.IsNullOrEmpty(gyoubanValue))
                            {
                                gyoubanValue = Convert.ToString(getOrderNo[i - 1]);
                            }
                            else
                            {
                                gyoubanValue = gyoubanValue + Convert.ToString(getOrderNo[i - 1]);
                            }
                        }

                        gyoubanValue = ReverseChar(gyoubanValue);
                    }
                    if (startChuubanPosition > 0 && endChuubanPosition > 0)
                    {
                        for (int i = startChuubanPosition; i <= endChuubanPosition; i++)
                        {

                            if (string.IsNullOrEmpty(chuubanValue))
                            {
                                chuubanValue = Convert.ToString(getOrderNo[i - 1]);
                            }
                            else
                            {
                                chuubanValue = chuubanValue + Convert.ToString(getOrderNo[i - 1]);
                            }
                        }
                        chuubanValue = ReverseChar(chuubanValue);
                    }

                    if (startYearPosition > 0 && endYearPosition > 0)
                    {
                        for (int i = startYearPosition; i <= endYearPosition; i++)
                        {


                            if (string.IsNullOrEmpty(yearValue))
                            {
                                yearValue = Convert.ToString(getOrderNo[i - 1]);
                            }
                            else
                            {
                                yearValue = yearValue + Convert.ToString(getOrderNo[i - 1]);
                            }
                        }
                        yearValue = ReverseChar(yearValue);
                    }
                    commonFilds.DIGIT_IN_CHUUBAN = digitValue;
                    commonFilds.GYOUBAN_IN_CHUUBAN = gyoubanValue;
                    commonFilds.ORDER_NO_IN_CHUUBAN = chuubanValue;
                    commonFilds.YEAR_IN_CHUUBAN = yearValue;
                }

                else
                {

                    //gyoubanValue = getOrderNo[1].ToString() + getOrderNo[0].ToString();
                   // chuubanValue = string.Concat(getOrderNo[2..9].Reverse());
                    chuubanValue = orderNo.Substring(0, (chubanLenght - 2));
                    gyoubanValue = orderNo.Substring(chubanLenght - 2, 2);
                   // gyoubanValue = orderNo.Substring((chubanLenght - 2), chubanLenght);
                    //getOrderNo[8].ToString() + getOrderNo[7].ToString() + getOrderNo[6].ToString() + getOrderNo[5].ToString() + getOrderNo[4].ToString() + getOrderNo[3].ToString() + getOrderNo[2].ToString();
                    commonFilds.DIGIT_IN_CHUUBAN = orderNo;
                    commonFilds.GYOUBAN_IN_CHUUBAN = gyoubanValue;
                    commonFilds.ORDER_NO_IN_CHUUBAN = chuubanValue;
                }

              }
            catch (Exception ex)
            {
                if (ex is IndexOutOfRangeException)
                {
                    commonFilds = new CommonFields();
                    return commonFilds;
                }
            }
            return commonFilds;
        }
        public CommonFields GetMappedNifuda(string nifudaNo)
        {
            int startCheckDigit;
            int endCheckDigit;
            int startSerialNoPosition;
            int endSerialNoPosition;
            int nifudaLenght;
            string digitValue = null;
            string nifudaValue = null;

            CommonFields commonFilds = new CommonFields();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" * ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_CONFIGURATION ");

            var result = ExecuteQueryMultiResult<MstConfiguration>(selectQuery.ToString());

            startCheckDigit = Convert.ToInt32(result.Where(r => r.KEY.ToUpper() == "START_CHECK_DIGIT_POSITION_IN_NIFUDA").Select(c => c.VALUE).SingleOrDefault());//1 configuration table value-2
            endCheckDigit = Convert.ToInt32(result.Where(r => r.KEY.ToUpper() == "END_CHECK_DIGIT_POSITION_IN_NIFUDA").Select(r => r.VALUE).SingleOrDefault());//1
            startSerialNoPosition = Convert.ToInt32(result.Where(r => r.KEY.ToUpper() == "START_SERIAL_NO_POSITION_IN_NIFUDA").Select(c => c.VALUE).SingleOrDefault());//2
            endSerialNoPosition = Convert.ToInt32(result.Where(r => r.KEY.ToUpper() == "END_SERIAL_NO_POSITION_IN_NIFUDA").Select(r => r.VALUE).SingleOrDefault());//1
            nifudaLenght = Convert.ToInt32(result.Where(r => r.KEY.ToUpper() == "NIFUDA_LENGTH").Select(r => r.VALUE).SingleOrDefault());
            var getNifudaNo = nifudaNo.Reverse().ToArray();
            
            try
            {
                if (getNifudaNo.Length > nifudaLenght)
                { 
                if (startCheckDigit > 0 && endCheckDigit > 0)
                {
                    for (int i = startCheckDigit; i <= endCheckDigit; i++)
                    {

                        if (string.IsNullOrEmpty(digitValue))
                        {
                            digitValue = Convert.ToString(getNifudaNo[i - 1]);
                        }
                        else
                        {
                            digitValue = digitValue + Convert.ToString(getNifudaNo[i - 1]);
                        }
                    }

                    digitValue = ReverseChar(digitValue);
                }
                if (startSerialNoPosition > 0 && endSerialNoPosition > 0)
                {
                    for (int i = startSerialNoPosition; i <= endSerialNoPosition; i++)
                    {

                        if (string.IsNullOrEmpty(nifudaValue))
                        {
                            nifudaValue = Convert.ToString(getNifudaNo[i - 1]);
                        }
                        else
                        {
                            nifudaValue = nifudaValue + Convert.ToString(getNifudaNo[i - 1]);
                        }
                    }

                    nifudaValue = ReverseChar(nifudaValue);
                }

                commonFilds.DIGIT_IN_NIFUDA = digitValue;
                
                commonFilds.NIFUDA_NO = nifudaValue;

                }
                else
                {
                    commonFilds.NIFUDA_NO = nifudaNo;
                }
            }
            catch (Exception ex)
            {
                if (ex is IndexOutOfRangeException)
                {
                    commonFilds = new CommonFields();
                    return commonFilds;
                }
            }
            return commonFilds;
        }

        public string ReverseChar(string chars)
        {
            var result = chars.ToCharArray();
            Array.Reverse(result);
            return new string(result);
        }
        public string GetLocationName(int userId)
        {
            string locationName = string.Empty;
                                                                           
            StringBuilder selectQuery = new StringBuilder();                                                      
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.LOCATION_NAME ");

            selectQuery.Append(" FROM MST_LOCATION LOC");
            selectQuery.Append(" INNER JOIN MST_USER_LOGIN UL ON UL.LOCATION_CD = LOC.LOCATION_CD");
            selectQuery.Append(" WHERE UL.USER_ID=" + userId);
            locationName = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            return locationName;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/01/22      
        /// used screens: RollPoMainAnalysis
        /// </summary>
        /// <returns>
        /// get Batch roll from MST_CONFIGURATION table
        /// </returns>
        /// 
        public int GetBatchRollFromConfig()
        {
            int batch_Roll;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT VALUE FROM MST_CONFIGURATION WHERE KEY IN('FORECAST_BATCH_ROLL') ");
            batch_Roll = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return batch_Roll;

        }

        public int GetMsgCDByNifudaCD(int nifudaCD)
        {
            int msgCD;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MSG_CD FROM TRN_FINAL_INSPECTION ");
            selectQuery.Append(" WHERE NIFUDA_CD=" + nifudaCD);
            msgCD = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return msgCD;

        }
        public DateTime GetSystemDateTime()
        {
            
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT SYSDATE FROM DUAL ");
           return Convert.ToDateTime(ExecuteScalar(selectQuery.ToString()));
            

        }
        public List<NameValue> GetCustomerList(string name, int customerCD)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append("  CD ");
                selectQuery.Append(" ,NAME ");
                selectQuery.Append(" ,YOBIGANA ");
                selectQuery.Append(" ,YOBINA ");
                selectQuery.Append(" ,SHORT_NAME ");
                selectQuery.Append(" ,IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ( ");
                selectQuery.Append(" SELECT DISTINCT(CUST.CUSTOMER_CD) AS CD ");
                selectQuery.Append(" ,CUST.SHORT_NAME AS NAME ");
                selectQuery.Append(" ,CUST.YOBIGANA ");
                selectQuery.Append(" ,CUST.YOBINA ");
                selectQuery.Append(" ,CUST.SHORT_NAME");
                selectQuery.Append(" ,CUST.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_CUSTOMER CUST");
                selectQuery.Append(" WHERE CUST.CUSTOMER_CD = " + customerCD);

                selectQuery.Append(" UNION ");
                selectQuery.Append(" SELECT DISTINCT(CUST.CUSTOMER_CD) AS CD ");
                selectQuery.Append(" ,CUST.SHORT_NAME AS NAME ");
                selectQuery.Append(" ,CUST.YOBIGANA ");
                selectQuery.Append(" ,CUST.YOBINA ");
                selectQuery.Append(" ,CUST.SHORT_NAME ");
                selectQuery.Append(" ,CUST.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_CUSTOMER CUST");

                selectQuery.Append(" WHERE CUST.IN_ACTIVE_FLAG = 0");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
                selectQuery.Append(" ) ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(YOBIGANA), ");
                selectQuery.Append(" LOWER(YOBINA), ");
                selectQuery.Append(" LOWER(SHORT_NAME) ");
            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT ");
                selectQuery.Append(" (CUST.CUSTOMER_CD ) AS CD,");
                selectQuery.Append(" CUST.SHORT_NAME AS NAME,");
                selectQuery.Append(" CUST.YOBIGANA,");
                selectQuery.Append(" CUST.YOBINA,");
                selectQuery.Append(" CUST.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" MST_CUSTOMER CUST");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" LOWER(CUST.YOBINA) LIKE LOWER('%" + name + "%') ");
                selectQuery.Append(" AND CUST.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(CUST.YOBIGANA), ");
                selectQuery.Append(" LOWER(CUST.YOBINA), ");
                selectQuery.Append(" LOWER(CUST.SHORT_NAME) ");
            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }

        public List<NameValue> GetMukesakiNameList(string name, int mukesakiCD)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" CD ");
                selectQuery.Append(" , NAME ");
                selectQuery.Append(" , YOBIGANA ");
                selectQuery.Append(" , YOBINA ");
                selectQuery.Append(" , SHORT_NAME ");
                selectQuery.Append(" , IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM( ");
                selectQuery.Append(" SELECT DISTINCT(MUKE.MUKESAKI_CD) AS CD ");
                selectQuery.Append(" , MUKE.SHORT_NAME AS NAME ");
                selectQuery.Append(" , MUKE.YOBIGANA ");
                selectQuery.Append(" , MUKE.YOBINA ");
                selectQuery.Append(" , MUKE.SHORT_NAME ");
                selectQuery.Append(" , MUKE.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_MUKESAKI MUKE ");
                selectQuery.Append(" WHERE MUKE.MUKESAKI_CD = " + mukesakiCD );

                selectQuery.Append(" UNION ");
                selectQuery.Append(" SELECT DISTINCT(MUKE.MUKESAKI_CD) AS CD ");
                selectQuery.Append(" , MUKE.SHORT_NAME AS NAME ");
                selectQuery.Append(" , MUKE.YOBIGANA ");
                selectQuery.Append(" , MUKE.YOBINA ");
                selectQuery.Append(" , MUKE.SHORT_NAME ");
                selectQuery.Append(" , MUKE.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_MUKESAKI MUKE ");

                selectQuery.Append(" WHERE MUKE.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit );
                selectQuery.Append(" ) ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(YOBIGANA), ");
                selectQuery.Append(" LOWER(YOBINA), ");
                selectQuery.Append(" LOWER(SHORT_NAME)");

            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT ");
                selectQuery.Append(" (MUKE.MUKESAKI_CD ) AS CD, ");
                selectQuery.Append(" MUKE.SHORT_NAME AS NAME, ");
                selectQuery.Append(" MUKE.YOBIGANA, ");
                selectQuery.Append(" MUKE.YOBINA,");
                selectQuery.Append(" MUKE.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ");
                selectQuery.Append(" MST_MUKESAKI MUKE ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" LOWER(MUKE.YOBINA) LIKE LOWER('" + name + "%') ");
                selectQuery.Append(" AND MUKE.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(MUKE.YOBIGANA), ");
                selectQuery.Append(" LOWER(MUKE.YOBINA), ");
                selectQuery.Append(" LOWER(MUKE.SHORT_NAME) ");

            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public List<NameValue> GetSupplierNameList(string name, int supplierCD)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" CD ");
                selectQuery.Append(" , NAME ");
                selectQuery.Append(" , YOBIGANA ");
                selectQuery.Append(" , YOBINA ");
                selectQuery.Append(" , SHORT_NAME ");
                selectQuery.Append(" , IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM( ");
                selectQuery.Append(" SELECT DISTINCT(SUPP.SUPPLIER_CD) AS CD ");
                selectQuery.Append(" , SUPP.SHORT_NAME AS NAME ");
                selectQuery.Append(" , SUPP.YOBIGANA ");
                selectQuery.Append(" , SUPP.YOBINA ");
                selectQuery.Append(" , SUPP.SHORT_NAME ");
                selectQuery.Append(" , SUPP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_SUPPLIER SUPP ");
                selectQuery.Append(" WHERE SUPP.SUPPLIER_CD = " + supplierCD);

                selectQuery.Append(" UNION ");
                selectQuery.Append(" SELECT DISTINCT(SUPP.SUPPLIER_CD) AS CD ");
                selectQuery.Append(" , SUPP.SHORT_NAME AS NAME ");
                selectQuery.Append(" , SUPP.YOBIGANA ");
                selectQuery.Append(" , SUPP.YOBINA ");
                selectQuery.Append(" , SUPP.SHORT_NAME ");
                selectQuery.Append(" , SUPP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_SUPPLIER SUPP ");

                selectQuery.Append(" WHERE SUPP.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
                selectQuery.Append(" ) ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(YOBIGANA), ");
                selectQuery.Append(" LOWER(YOBINA), ");
                selectQuery.Append(" LOWER(SHORT_NAME)");

            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT(SUPP.SUPPLIER_CD) AS CD ");
                selectQuery.Append(" , SUPP.SHORT_NAME AS NAME ");
                selectQuery.Append(" , SUPP.YOBIGANA ");
                selectQuery.Append(" , SUPP.YOBINA ");
                selectQuery.Append(" , SUPP.SHORT_NAME ");
                selectQuery.Append(" , SUPP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_SUPPLIER SUPP ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" LOWER(SUPP.YOBINA) LIKE LOWER('" + name + "%') ");
                selectQuery.Append(" AND SUPP.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(SUPP.YOBIGANA), ");
                selectQuery.Append(" LOWER(SUPP.YOBINA), ");
                selectQuery.Append(" LOWER(SUPP.SHORT_NAME) ");

            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
        public List<NameValue> GetMochikomisakiNameList(string name, int mochikomisakiCD)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append(" CD ");
                selectQuery.Append(" , NAME ");
                selectQuery.Append(" , YOBIGANA ");
                selectQuery.Append(" , YOBINA ");
                selectQuery.Append(" , SHORT_NAME ");
                selectQuery.Append(" , IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM( ");
                selectQuery.Append(" SELECT DISTINCT(MOCH.MOCHIKOMISAKI_CD) AS CD ");
                selectQuery.Append(" , MOCH.SHORT_NAME AS NAME ");
                selectQuery.Append(" , MOCH.YOBIGANA ");
                selectQuery.Append(" , MOCH.YOBINA ");
                selectQuery.Append(" , MOCH.SHORT_NAME ");
                selectQuery.Append(" , MOCH.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_MOCHIKOMISAKI MOCH ");
                selectQuery.Append(" WHERE MOCH.MOCHIKOMISAKI_CD = " + mochikomisakiCD);

                selectQuery.Append(" UNION ");
                selectQuery.Append(" SELECT DISTINCT(MOCH.MOCHIKOMISAKI_CD) AS CD ");
                selectQuery.Append(" , MOCH.SHORT_NAME AS NAME ");
                selectQuery.Append(" , MOCH.YOBIGANA ");
                selectQuery.Append(" , MOCH.YOBINA ");
                selectQuery.Append(" , MOCH.SHORT_NAME ");
                selectQuery.Append(" , MOCH.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_MOCHIKOMISAKI MOCH ");

                selectQuery.Append(" WHERE MOCH.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
                selectQuery.Append(" ) ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(YOBIGANA), ");
                selectQuery.Append(" LOWER(YOBINA), ");
                selectQuery.Append(" LOWER(SHORT_NAME)");

            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT(MOCH.MOCHIKOMISAKI_CD) AS CD ");
                selectQuery.Append(" , MOCH.SHORT_NAME AS NAME ");
                selectQuery.Append(" , MOCH.YOBIGANA ");
                selectQuery.Append(" , MOCH.YOBINA ");
                selectQuery.Append(" , MOCH.SHORT_NAME ");
                selectQuery.Append(" , MOCH.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_MOCHIKOMISAKI MOCH ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" LOWER(MOCH.YOBINA) LIKE LOWER('" + name + "%') ");
                selectQuery.Append(" AND MOCH.IN_ACTIVE_FLAG = 0 ");
                selectQuery.Append(" ORDER BY ");
                selectQuery.Append(" LOWER(MOCH.YOBIGANA), ");
                selectQuery.Append(" LOWER(MOCH.YOBINA), ");
                selectQuery.Append(" LOWER(MOCH.SHORT_NAME) ");

            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }

        public CommonFields GetChuubanGyuobanByNifuda(string nifudaNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            CommonFields details = new CommonFields();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" TN.CHUUBAN AS CHUUBAN, ");
            selectQuery.Append(" TN.GYOUBAN AS GYOUBAN,");
            selectQuery.Append(" NIFUDA_COMPLETE_DATE AS NIFUDA_COMPLETE_DATE ");
            selectQuery.Append(" FROM TRN_NIFUDA TN ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON TN.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE NIFUDA_NO = '" + nifudaNo + "' ");
            selectQuery.Append(" AND NIFUDA_TYPE = 1 ");
            selectQuery.Append(" AND PROCESS_PLAN_CMPL_DATE IS NULL ");
            details = ExecuteQuerySingleResult<CommonFields>(selectQuery.ToString());
            return details;
        }
        public List<NameValue> GetOperatorDeptNameList(string name, int operatorCD)
        {
            string _maxRowLimit = Constants.DefaultValues.SEARCH_COMBO_MAX_LIMIT;
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(name))
            {
                selectQuery.Append(" SELECT ");
                selectQuery.Append("  CD ");
                selectQuery.Append(" ,NAME ");
                selectQuery.Append(" ,YOBIGANA ");
                selectQuery.Append(" ,YOBINA ");
                selectQuery.Append(" ,SHORT_NAME ");
                selectQuery.Append(" ,IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM ( ");
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,null DEPARTMENT_NAME");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                selectQuery.Append(" WHERE EMP.EMPLOYEE_CD = " + operatorCD);

                selectQuery.Append(" UNION ");
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME || ' - ' || NVL(DEPARTMENT_NAME, '') AS NAME");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,DEPT.DEPARTMENT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT  DEPT ON DEPT.DEPARTMENT_CD = DG.DEPARTMENT_CD");
                selectQuery.Append(" WHERE EMP.IN_ACTIVE_FLAG = 0");
                selectQuery.Append(" AND ROWNUM <= " + _maxRowLimit);
                selectQuery.Append(" ) ");
                selectQuery.Append(" ORDER BY LOWER (YOBIGANA),LOWER (YOBINA),LOWER (SHORT_NAME) ");
            }
            else
            {


                selectQuery.Append(" SELECT DISTINCT");
                selectQuery.Append(" EMP.EMPLOYEE_CD AS CD ");
                selectQuery.Append(" ,SHORT_NAME || ' - ' || NVL(DEPARTMENT_NAME, '') AS NAME");
                selectQuery.Append(" ,EMP.YOBIGANA,EMP.YOBINA ");
                selectQuery.Append(" ,DEPT.DEPARTMENT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP ");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD"); //commented as per confirmation by Nilesh Sir
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT DEPT ON DEPT.DEPARTMENT_CD = DG.DEPARTMENT_CD");
                selectQuery.Append(" WHERE LOWER (EMP.YOBINA) LIKE lower('" + name + "%') ");
                selectQuery.Append(" AND EMP.IN_ACTIVE_FLAG = 0");
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (NAME)");
            }

            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }
    }
}
