using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using System.Reflection.PortableExecutable;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class WIPTensouAcceptanceRepository : BaseRepository, IWIPTensouAcceptanceRepository
    {
        private readonly string _connectionString;
        private readonly object _sortField;

        public WIPTensouAcceptanceRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }

        /// <summary>
        /// Get Acceptance Details By Transfer CD. 
        /// </summary>
        /// <param name="tensouCD"></param>
        /// <returns> WIPTensouDetails </returns>
        public List<WIPTensouDetails> GetWIPTransferAcceptanceDetailsByTrasferCD(string tensouCD)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            //string[] WTensouCD;
            //WTensouCD = tensouCD.Split(',');
            // var temp = lstGridData.Where(x => x.WIP_TRANSFER_CD > 0).Select(x => x.WIP_TRANSFER_CD).ToList();
            //var TensouNo = string.Join(",",temp);
            // WIPTensouDetails result = new WIPTensouDetails();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append("SELECT ");
            selectQuery.Append("WIP.TRANSFER_CD AS WIP_TRANSFER_CD");
            //selectQuery.Append(", WIPA.TRN_WIP_TRANSFER_ACCEPTANCE_CD");
            selectQuery.Append(", WIP.PROCESS_PLAN_CD");
            selectQuery.Append(", WIP.LOCATION_CD_FROM");
            selectQuery.Append(", WIP.LOCATION_CD_TO");
            selectQuery.Append(", WIP.MSG_CD");
            selectQuery.Append(",GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN)AS PP_CHUUBAN");
            selectQuery.Append(",CT.SHORT_NAME AS  PP_CUSTOMER_NAME");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(",PP.PROD_SHAPE  AS PP_MATL_SHAPE");
            selectQuery.Append(",PP.PROD_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(",PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(",NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(",GET_DIM_D_W_SHAPE_OTHER('',PP.PROD_DIM_W, '', PP.PROD_DIM_OTHER) AS PP_PROD_DIM_W_OTHER");
            selectQuery.Append(",PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(",PP.PROCESS_PLAN_CMPL_DATE AS PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(",PP.PROCESS_PLAN_START_DATE AS PROCESS_PLAN_START_DATE");
            //all taba no combine
            /*selectQuery.Append(", CASE WHEN WIP.TABA_NO_2 IS NOT NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1");
            selectQuery.Append(" || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 ");
            selectQuery.Append(" ELSE");
            selectQuery.Append(" CASE");
            selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 || ',' || WIP.TABA_NO_4 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 || ',' || WIP.TABA_NO_4 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NOT NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_2 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NOT NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_3 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN WIP.TABA_NO_2 IS NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_3 IS NULL ");
            selectQuery.Append(" AND WIP.TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" WIP.TABA_NO_1 || ',' || WIP.TABA_NO_4 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" TO_CHAR(WIP.TABA_NO_1)");
            selectQuery.Append(" END ");
            selectQuery.Append(" END ");
            selectQuery.Append(" END  ");
            selectQuery.Append(" END ");
            selectQuery.Append(" END ");
            selectQuery.Append(" END ");
            selectQuery.Append(" END ");
            selectQuery.Append(" AS TABA_STRING");*/
            selectQuery.Append(" ,RTRIM(");
            selectQuery.Append(" NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') ||");
            selectQuery.Append(" NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') ||");
            selectQuery.Append(" NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') ||");
            selectQuery.Append(" NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', ''),");
            selectQuery.Append(" ','");
            selectQuery.Append(" ) AS TABA_STRING ");

            selectQuery.Append(",WIP.NO_OF_UNITS , WIP.UNIT_TYPE ");
            selectQuery.Append(",WIP.QTY AS QTY ");
            selectQuery.Append(",WIP.PLANNED_NEXT_PROSESS AS NEXT_PROSESS");
            selectQuery.Append(",WIP.TABA_NO_1");
            selectQuery.Append(",WIP.TABA_NO_2");
            selectQuery.Append(",WIP.TABA_NO_3");
            selectQuery.Append(",WIP.TABA_NO_4");
            selectQuery.Append(",MSG.MSG_CONTENTS AS MSG_CONTENTS ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER WIP INNER JOIN ");
            selectQuery.Append(" TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CT ON CT.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON NVL(WIP.MSG_CD, NULL) = MSG.MSG_CD ");
            selectQuery.Append(" WHERE WIP.TRANSFER_CD IN (" + tensouCD + ")");
            selectQuery.Append(" ORDER BY INSTR('" + tensouCD + "', transfer_cd) ");
            var query = selectQuery.ToString();
            result = ExecuteQueryMultiResult<WIPTensouDetails>(query);
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WIPTensouCD"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetValidTensouNo(string WIPTensouCD)
        {
            StringBuilder selectQuery = new StringBuilder();

            string[] WTensouCD;
            WTensouCD = WIPTensouCD.Split(',');
            List<string> validTensouCD = new List<string>();
            List<string> notvalidTensouCD = new List<string>();
            Dictionary<string, List<string>> _dictTransfernumber = new Dictionary<string, List<string>>();
            foreach (string TrasNum in WTensouCD)
            {
                selectQuery.Append(" SELECT COUNT(TRANSFER_CD) FROM TRN_WIP_AND_PROD_TRANSFER");
                selectQuery.Append(" WHERE TRANSFER_CD  = (" + TrasNum + ")");
                int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                if (count > 0)
                {
                    validTensouCD.Add(TrasNum);
                }
                else
                {
                    notvalidTensouCD.Add(TrasNum);
                }
                selectQuery.Clear();

            }
            _dictTransfernumber.Add("found", validTensouCD);
            _dictTransfernumber.Add("notFound", notvalidTensouCD);
            return _dictTransfernumber;

        }

        public Dictionary<string, List<string>> GetCompleteChuuban(string completeTransferNo)
        {
            StringBuilder selectQuery = new StringBuilder();

            string[] WTensouCD;
            WTensouCD = completeTransferNo.Split(',');
            List<string> completeTensouCD = new List<string>();
            List<string> notcompleteTensouCD = new List<string>();
            Dictionary<string, List<string>> _dictTransfernumber = new Dictionary<string, List<string>>();
            foreach (string TrasNum in WTensouCD)
            {
                selectQuery.Append(" SELECT COUNT(PP.PROCESS_PLAN_CMPL_DATE)FROM TRN_WIP_AND_PROD_TRANSFER  WIP");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN ");
                selectQuery.Append(" PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD  ");
                selectQuery.Append(" WHERE WIP.TRANSFER_CD  = (" + TrasNum + ")");

                int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                if (count > 0)
                {
                    completeTensouCD.Add(TrasNum);
                }
                else
                {
                    notcompleteTensouCD.Add(TrasNum);
                }
                selectQuery.Clear();

            }
            _dictTransfernumber.Add("complete", completeTensouCD);
            _dictTransfernumber.Add("notComplete", notcompleteTensouCD);
            return _dictTransfernumber;

        }

        public Dictionary<string, List<string>> TransferNoAccepted(string transferNoAccepted)
        {
            StringBuilder selectQuery = new StringBuilder();

            string[] WTensouCD;
            WTensouCD = transferNoAccepted.Split(',');
            List<string> acceptedTensouCD = new List<string>();
            List<string> notAcceptedTensouCD = new List<string>();
            Dictionary<string, List<string>> _dictTransfernumber = new Dictionary<string, List<string>>();
            foreach (string TrasNum in WTensouCD)
            {
                selectQuery.Append(" SELECT COUNT(TRANSFER_CD) FROM TRN_WIP_AND_PROD_TRANSFER");
                selectQuery.Append(" WHERE TRANSFER_CD  = (" + TrasNum + ")");
                selectQuery.Append(" AND ACCEPTANCE_DATE IS NOT NULL ");
                int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                if (count > 0)
                {
                    acceptedTensouCD.Add(TrasNum);
                }
                else
                {
                    notAcceptedTensouCD.Add(TrasNum);
                }
                selectQuery.Clear();

            }
            _dictTransfernumber.Add("accepted", acceptedTensouCD);
            _dictTransfernumber.Add("notAccepted", notAcceptedTensouCD);
            return _dictTransfernumber;


        }

        public int GetMaxGroupNo()
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT MAX(TRANSFER_GROUP_NO) FROM TRN_WIP_AND_PROD_TRANSFER");
            int max = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            //int max = ExecuteQueryMultiResult<long?>(selectQuery);
            return max;
        }

        public void SaveWIPTensouAcceptanceDetails(WIPTensouDetails wipTransfer)
        {
            StringBuilder updateQuery = new StringBuilder();
            var now = DateTime.Now;
            string time = now.ToString("HH:mm:ss");
            string dateTime = wipTransfer.TRANSFER_DATE_STRING + " " + time;

            //insertQuery.Append("INSERT INTO TRN_WIP_TRANSFER_ACCEPTANCE");
            //insertQuery.Append("(TRN_WIP_TRANSFER_ACCEPTANCE_CD	");
            //insertQuery.Append(",	PROCESS_KUBUN_CD ");
            //insertQuery.Append(",	PROCESS_PLAN_CD	");
            //insertQuery.Append(",	WIP_TRANSFER_CD	");
            //insertQuery.Append(",	LOCATION_CD_FROM	");
            //insertQuery.Append(",	LOCATION_CD_TO");
            //insertQuery.Append(",	ACCEPTANCE_DATE");
            //insertQuery.Append(",	TABA_NO_1");
            //insertQuery.Append(",	TABA_NO_2");
            //insertQuery.Append(",	TABA_NO_3");
            //insertQuery.Append(",	TABA_NO_4");
            //insertQuery.Append(",	NO_OF_UNITS");
            //insertQuery.Append(",	UNIT_TYPE");
            //insertQuery.Append(",	QTY	");
            //insertQuery.Append(",	MSG_CD	");
            //insertQuery.Append(",	NEXT_PROSESS");
            //insertQuery.Append(",	USER_ID	");
            //insertQuery.Append(",	INPUT_LOCATION_CD");
            //insertQuery.Append(",	CREATED_DATE");
            //insertQuery.Append(",	UPDATED_DATE");
            //insertQuery.Append(" ) ");
            //insertQuery.Append("VALUES(");
            //insertQuery.Append("TRN_WIP_TRANS_ACCEPT_SEQ.NEXTVAL ,");
            //insertQuery.Append(wipTransfer.PROCESS_KUBUN_ID + ",");
            //insertQuery.Append(wipTransfer.PROCESS_PLAN_CD + ",");
            //insertQuery.Append(wipTransfer.WIP_TRANSFER_CD + ",");
            //insertQuery.Append(wipTransfer.LOCATION_CD_FROM + ",");
            //insertQuery.Append(wipTransfer.LOCATION_CD_TO + ",");
            //insertQuery.Append(" TO_DATE('" + dateTime + "','"+ Constants.DateFormats.YYYYMMDD_HH24MISS + "')");
            //insertQuery.Append(",'" + wipTransfer.TABA_NO_1 + "'");
            //insertQuery.Append(",'" + wipTransfer.TABA_NO_2 + "'");
            //insertQuery.Append(",'" + wipTransfer.TABA_NO_3 + "'");
            //insertQuery.Append(",'" + wipTransfer.TABA_NO_4 + "'");
            //insertQuery.Append(",'" + wipTransfer.NO_OF_UNITS + "'");
            //insertQuery.Append(",'" + wipTransfer.UNIT_TYPE + "', ");
            //insertQuery.Append("'" + wipTransfer.QTY + "',");
            //insertQuery.Append("'" + wipTransfer.MSG_CD + "',");
            //insertQuery.Append("'" + wipTransfer.NEXT_PROSESS + "',");
            //insertQuery.Append("'" + wipTransfer.USER_ID + "',");
            //insertQuery.Append("'" + wipTransfer.INPUT_LOCATION_CD + "',");
            //insertQuery.Append("SYSDATE,");
            //insertQuery.Append("SYSDATE)");

            updateQuery.Append(" UPDATE TRN_WIP_AND_PROD_TRANSFER SET "); 
            updateQuery.Append(" PROCESS_KUBUN_CD ='" + wipTransfer.PROCESS_KUBUN_ID + "'");
            updateQuery.Append(" ,LOCATION_CD_TO ='" + wipTransfer.LOCATION_CD_TO + "'");
            updateQuery.Append(" ,ACCEPTANCE_DATE = TO_DATE('" + dateTime + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "')");
            if (wipTransfer.MSG_CD == 0)
            {
                updateQuery.Append(", ACCEPT_MSG_CD = null ");
            }
            else
            {
                updateQuery.Append(",ACCEPT_MSG_CD ='" + wipTransfer.MSG_CD + "'");
            }
            updateQuery.Append(" ,ACCEPT_NEXT_PROSESS ='" + wipTransfer.NEXT_PROSESS + "'");
            updateQuery.Append(" ,ACCEPT_USER_ID ='" + wipTransfer.USER_ID + "'");
            updateQuery.Append(" ,ACCEPT_INPUT_LOCATION_CD ='" + wipTransfer.INPUT_LOCATION_CD + "'");
            updateQuery.Append(" ,UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" TRANSFER_CD = ");
            updateQuery.Append(wipTransfer.WIP_TRANSFER_CD);
            ExecuteNonQuery(updateQuery.ToString());
        }

        public void WIPTensouAcceptanceDetailsDelete(string WIPTransferCD)
        {
               var kubunCD = UIConstants.K_PROCESS_WIP_TRANSFER;
            
                StringBuilder deleteQuery = new StringBuilder();

                deleteQuery.Append("UPDATE TRN_WIP_AND_PROD_TRANSFER SET ");
                deleteQuery.Append("PROCESS_KUBUN_CD = " + kubunCD);
                deleteQuery.Append(", ACCEPTANCE_DATE = NULL");
                deleteQuery.Append(", ACCEPT_NEXT_PROSESS = NULL");
                deleteQuery.Append(", ACCEPT_MSG_CD = NULL");
                deleteQuery.Append(", ACCEPT_USER_ID = NULL");
                deleteQuery.Append(", ACCEPT_INPUT_LOCATION_CD = NULL");
                deleteQuery.Append(", UPDATED_DATE = SYSDATE");
                deleteQuery.Append(" WHERE TRANSFER_CD = " + WIPTransferCD);

                ExecuteNonQuery(deleteQuery.ToString());
         
        }

      
        public int GetMsgCdByMsgContent(string MSG_CONTENTS)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MSG_CD FROM TRN_MESSAGE WHERE MSG_CONTENTS='" + MSG_CONTENTS + "'");
            String MSG = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            int result = string.IsNullOrEmpty(MSG) ? 0 : Convert.ToInt32(MSG);
            return result;

        }

        public int GetMsgCdFromTensouRequest(int msgCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MSG_CD FROM TRN_WIP_AND_PROD_TRANSFER WHERE MSG_CD='" + msgCD + "'");
            String MSG = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            int result = string.IsNullOrEmpty(MSG) ? 0 : Convert.ToInt32(MSG);
            return result;

        }

    }
}
