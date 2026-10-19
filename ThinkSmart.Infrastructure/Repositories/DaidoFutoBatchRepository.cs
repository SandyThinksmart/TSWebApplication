using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class DaidoFutoBatchRepository : BaseRepository, IDaidoFutoBatchRepository
    {

        public DaidoFutoBatchRepository(IConfiguration configuration ) : base(configuration)
        {
            
        }
        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/05       
        /// used screens: Daido Futou Batch Screen
        /// </summary>
        /// <returns>
        /// To bind machine name of HeatTreatment process
        /// </returns>
        /// 
        public List<NameValue> GetFutouList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT ");
            sql.Append(" FUTOU_CD AS CD, ");
            sql.Append(" FUTOU_NAME AS NAME ");
            sql.Append(" FROM ");
            sql.Append(" MST_FUTOU ");
            sql.Append(" WHERE ");
            sql.Append(" IN_ACTIVE_FLAG = 0 ");
            sql.Append(" ORDER BY ");
            sql.Append(" CASE ");
            sql.Append(" WHEN FUTOU_NAME = '川一産業（株）' THEN ");
            sql.Append(" 1 ELSE 2 ");
            sql.Append(" END, ");
            sql.Append(" YOBINA, ");
            sql.Append(" YOBIGANA, ");
            sql.Append(" FUTOU_NAME ");
            result = ExecuteQueryMultiResult<NameValue>(sql.ToString());
            return result;
        }

        public void DaidoNyukoEDIBatchDataInsert(DaidoFutouNyukoDetails details, string cultureName, int userID, int inputLocationCd)
        {
            StringBuilder insertQuery = new StringBuilder();
            List<DaidoFutouNyukoDetails> result = new List<DaidoFutouNyukoDetails>();
            insertQuery.Append("INSERT INTO TRN_DAIDO_FUTO_NYUKO_PROC_LOG ( ");
            insertQuery.Append(" DAIDO_FUTO_NYUKO_PROC_LOG_CD, ");
            insertQuery.Append(" PROCESS_DATE_AND_TIME, ");
            insertQuery.Append(" PROCESS_SERIAL_NUMBER, ");
            insertQuery.Append(" EXECUTE_RESULT, ");
            insertQuery.Append(" ERROR, ");
            insertQuery.Append(" FUTOU_CD, ");
            insertQuery.Append(" TYPE_OF_DATA, ");
            insertQuery.Append(" FUTO_NYUKO_DATE, ");
            insertQuery.Append(" CONTRACT_NUMBER, ");
            insertQuery.Append(" CONTRACT_GYOUBAN, ");
            insertQuery.Append(" FUTO_NYUKO_LOCATION, ");
            insertQuery.Append(" RECEIVER, ");
            insertQuery.Append(" MUKESAKI, ");
            insertQuery.Append(" MAKER_CODE, ");
            insertQuery.Append(" CONTRACTOR, ");
            insertQuery.Append(" PROCESSING_PERSON, ");
            insertQuery.Append(" ORDER_KOUBAN, ");
            insertQuery.Append(" STEEL_GRADE, ");
            insertQuery.Append(" DIM_IMAGE, ");
            insertQuery.Append(" DELIVERY_CONDITION, ");
            insertQuery.Append(" WINDING_DIRECTION, ");
            insertQuery.Append(" PROD_SPEC_KOUBAN, ");
            insertQuery.Append(" CHARGE, ");
            insertQuery.Append(" THABA_NO, ");
            insertQuery.Append(" NO_OF_UNITS, ");
            insertQuery.Append(" QUANTITY_IN_KG, ");
            insertQuery.Append(" YARD_KBN, ");
            insertQuery.Append(" NYUKA_KBN, ");
            insertQuery.Append(" SHUKKA_KBN, ");
            insertQuery.Append(" USER_MANAGEMENT_KEY, ");
            insertQuery.Append(" DIMD_DECOMPOSITION_SIZE, ");
            insertQuery.Append(" SHAPE_INFO, ");
            insertQuery.Append(" ORDER_PERSON_CODE, ");
            insertQuery.Append(" RESERVE, ");
            insertQuery.Append(" DESTINATION_CODE, ");
            insertQuery.Append(" FACTORY_SHUKKA_NUMBER, ");
            insertQuery.Append(" DELIVERY_NOTE_NUMBER, ");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");
            insertQuery.Append(" CREATED_DATE, ");
            insertQuery.Append(" CREATED_TIME, ");
            insertQuery.Append(" DATA_CREATED_DATE, ");
            insertQuery.Append(" UPDATED_DATE ");
            insertQuery.Append(") VALUES (");
            insertQuery.Append(" TRN_DAIDO_FUTO_NYUKO_PROC_LOG_SEQ.NEXTVAL ");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.PROCESS_DATE_AND_TIME).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");           
            insertQuery.Append(",'" + details.PROCESS_SERIAL_NUMBER + "'");
            insertQuery.Append(",'" + details.EXECUTE_RESULT + "'");
            insertQuery.Append(",'" + details.ERROR + "'");
            insertQuery.Append(",'" + details.FUTOU_CD + "'");
            insertQuery.Append(",'" + details.TYPE_OF_DATA + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.FUTO_NYUKO_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + details.CONTRACT_NUMBER + "'");
            insertQuery.Append(",'" + details.CONTRACT_GYOUBAN + "'");   
            insertQuery.Append(",'" + details.FUTO_NYUKO_LOCATION + "'");
            insertQuery.Append(",'" + details.RECEIVER + "'");
            insertQuery.Append(",'" + details.MUKESAKI + "'");
            insertQuery.Append(",'" + details.MAKER_CODE + "'");
            insertQuery.Append(",'" + details.CONTRACTOR + "'");
            insertQuery.Append(",'" + details.PROCESSING_PERSON + "'");
            insertQuery.Append(",'" + details.ORDER_KOUBAN + "'");
            insertQuery.Append(",'" + details.STEEL_GRADE + "'");
            insertQuery.Append(",'" + details.DIM_IMAGE + "'");
            insertQuery.Append(",'" + details.DELIVERY_CONDITION + "'");
            insertQuery.Append(",'" + details.WINDING_DIRECTION + "'");
            insertQuery.Append(",'" + details.PROD_SPEC_KOUBAN + "'");
            insertQuery.Append(",'" + details.CHARGE + "'");
            insertQuery.Append(",'" + details.THABA_NO + "'");
            insertQuery.Append(",'" + details.NO_OF_UNITS + "'");
            insertQuery.Append(",'" + details.QUANTITY_IN_KG + "'");
            insertQuery.Append(",'" + details.YARD_KBN + "'");
            insertQuery.Append(",'" + details.NYUKA_KBN + "'");
            insertQuery.Append(",'" + details.SHUKKA_KBN + "'");
            insertQuery.Append(",'" + details.USER_MANAGEMENT_KEY + "'");
            insertQuery.Append(",'" + details.DIMD_DECOMPOSITION_SIZE + "'");
            insertQuery.Append(",'" + details.SHAPE_INFO + "'");
            insertQuery.Append(",'" + details.ORDER_PERSON_CODE + "'");
            insertQuery.Append(",'" + details.RESERVE + "'");
            insertQuery.Append(",'" + details.DESTINATION_CODE + "'");
            insertQuery.Append(",'" + details.FACTORY_SHUKKA_NUMBER + "'");
            insertQuery.Append(",'" + details.DELIVERY_NOTE_NUMBER + "'");
            insertQuery.Append(",'" + userID + "'");
            insertQuery.Append(",'" + inputLocationCd + "'");
            insertQuery.Append(",'" + details.CREATED_DATE + "'");
            insertQuery.Append(",'" + details.CREATED_TIME + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());

           
        }

        public bool ValidateSameDataExist(DaidoFutouNyukoDetails details)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM TRN_DAIDO_FUTO_NYUKO_PROC_LOG   ");
            selectQuery.Append(" WHERE ");

            selectQuery.Append("  EXECUTE_RESULT = 0 ");
            if (!string.IsNullOrWhiteSpace(details.TYPE_OF_DATA))
            {
                selectQuery.Append(" AND TYPE_OF_DATA = '" + details.TYPE_OF_DATA + "'");
            }
            else
            {
                selectQuery.Append(" AND TYPE_OF_DATA IS NULL");
            }
            if(details.FUTO_NYUKO_DATE.HasValue)
            {
                selectQuery.Append(" AND FUTO_NYUKO_DATE = TO_DATE('" + (Convert.ToDateTime(details.FUTO_NYUKO_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");

            }
            else
            {
                selectQuery.Append(" AND FUTO_NYUKO_DATE IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.CONTRACT_NUMBER))
            {
                selectQuery.Append(" AND CONTRACT_NUMBER = '" + details.CONTRACT_NUMBER + "'");
            }
            else
            {
                selectQuery.Append(" AND CONTRACT_NUMBER IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.CONTRACT_GYOUBAN))
            {
                selectQuery.Append(" AND CONTRACT_GYOUBAN = '" + details.CONTRACT_GYOUBAN + "'");
            }
            else
            {
                selectQuery.Append(" AND CONTRACT_GYOUBAN IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.FUTO_NYUKO_LOCATION))
            {
                selectQuery.Append(" AND FUTO_NYUKO_LOCATION = '" + details.FUTO_NYUKO_LOCATION + "'");
            }
            else
            {
                selectQuery.Append(" AND FUTO_NYUKO_LOCATION IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.RECEIVER))
            {
                selectQuery.Append(" AND RECEIVER = '" + details.RECEIVER + "'");
            }
            else
            {
                selectQuery.Append(" AND RECEIVER IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.MUKESAKI))
            {
                selectQuery.Append(" AND MUKESAKI = '" + details.MUKESAKI + "'");
            }
            else
            {
                selectQuery.Append(" AND MUKESAKI IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.MAKER_CODE))
            {
                selectQuery.Append(" AND MAKER_CODE = '" + details.MAKER_CODE + "'");
            }
            else
            {
                selectQuery.Append(" AND MAKER_CODE IS NULL");
            }

            if (!string.IsNullOrWhiteSpace(details.CONTRACTOR))
            {
                selectQuery.Append(" AND CONTRACTOR = '" + details.CONTRACTOR + "'");
            }
            else
            {
                selectQuery.Append(" AND CONTRACTOR IS NULL");
            }
             if (!string.IsNullOrWhiteSpace(details.PROCESSING_PERSON))
            {
                selectQuery.Append(" AND PROCESSING_PERSON = '" + details.PROCESSING_PERSON + "'");
            }
            else
            {
                selectQuery.Append(" AND PROCESSING_PERSON IS NULL");
            }

             if(!string.IsNullOrWhiteSpace(details.ORDER_KOUBAN))
            {
                selectQuery.Append(" AND ORDER_KOUBAN = '" + details.ORDER_KOUBAN + "'");
            }
            else
            {
                selectQuery.Append(" AND ORDER_KOUBAN IS NULL");
            }
             if(!string.IsNullOrWhiteSpace(details.STEEL_GRADE))
            {
                selectQuery.Append(" AND STEEL_GRADE = '" + details.STEEL_GRADE + "'");
            }
             else
            {
                selectQuery.Append(" AND STEEL_GRADE IS NULL");
            }

            if (!string.IsNullOrWhiteSpace(details.DIM_IMAGE))
            {
                selectQuery.Append(" AND DIM_IMAGE = '" + details.DIM_IMAGE + "'");
            }
            else
            {
                selectQuery.Append(" AND DIM_IMAGE IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.DELIVERY_CONDITION))
            {
                selectQuery.Append(" AND DELIVERY_CONDITION = '" + details.DELIVERY_CONDITION + "'");
            }
            else
            {
                selectQuery.Append(" AND DELIVERY_CONDITION IS NULL");
            }
            if (!string.IsNullOrWhiteSpace(details.WINDING_DIRECTION))
            {
                selectQuery.Append(" AND WINDING_DIRECTION = '" + details.WINDING_DIRECTION + "'");
            }
            else
            {
                selectQuery.Append(" AND WINDING_DIRECTION IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.PROD_SPEC_KOUBAN))
            {
                selectQuery.Append(" AND PROD_SPEC_KOUBAN = '" + details.PROD_SPEC_KOUBAN + "'");
            }
            else 
            {   selectQuery.Append(" AND PROD_SPEC_KOUBAN IS NULL");

            }
            if(!string.IsNullOrWhiteSpace(details.CHARGE))
            {
                selectQuery.Append(" AND CHARGE = '" + details.CHARGE + "'");
            }
            else
            {
                selectQuery.Append(" AND CHARGE IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.THABA_NO))
            {
                selectQuery.Append(" AND THABA_NO = '" + details.THABA_NO + "'");
            }
            else
            {
                selectQuery.Append(" AND THABA_NO IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.NO_OF_UNITS))
            {
                selectQuery.Append(" AND NO_OF_UNITS = '" + details.NO_OF_UNITS + "'");
            }
            else
            {
                selectQuery.Append(" AND NO_OF_UNITS IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.QUANTITY_IN_KG))
            {
                selectQuery.Append(" AND QUANTITY_IN_KG = '" + details.QUANTITY_IN_KG + "'");
            }
            else
            {
                selectQuery.Append(" AND QUANTITY_IN_KG IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.YARD_KBN))
            {
                selectQuery.Append(" AND YARD_KBN = '" + details.YARD_KBN + "'");
            }
            else
            {
                selectQuery.Append(" AND YARD_KBN IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.NYUKA_KBN))
            {
                selectQuery.Append(" AND NYUKA_KBN = '" + details.NYUKA_KBN + "'");
            }
            else
            {
                selectQuery.Append(" AND NYUKA_KBN IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.SHUKKA_KBN))
            {
                selectQuery.Append(" AND SHUKKA_KBN = '" + details.SHUKKA_KBN + "'");
            }
            else
            {
                selectQuery.Append(" AND SHUKKA_KBN IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.USER_MANAGEMENT_KEY))
            {
                selectQuery.Append(" AND USER_MANAGEMENT_KEY = '" + details.USER_MANAGEMENT_KEY + "'");
            }
            else
            {
                selectQuery.Append(" AND USER_MANAGEMENT_KEY IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.DIMD_DECOMPOSITION_SIZE))
            {
                selectQuery.Append(" AND DIMD_DECOMPOSITION_SIZE = '" + details.DIMD_DECOMPOSITION_SIZE + "'");
            }
            else
            {
                selectQuery.Append(" AND DIMD_DECOMPOSITION_SIZE IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.SHAPE_INFO))
            {
                selectQuery.Append(" AND SHAPE_INFO = '" + details.SHAPE_INFO + "'");
            }
            else
            {
                selectQuery.Append(" AND SHAPE_INFO IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.ORDER_PERSON_CODE))
            {
                selectQuery.Append(" AND ORDER_PERSON_CODE = '" + details.ORDER_PERSON_CODE + "'");
            }
            else
            {
                selectQuery.Append(" AND ORDER_PERSON_CODE IS NULL");
            }

            if(!string.IsNullOrWhiteSpace(details.RESERVE))
            {
                selectQuery.Append(" AND RESERVE = '" + details.RESERVE + "'");
            }
            else
            {
                selectQuery.Append(" AND RESERVE IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.DESTINATION_CODE))
            {
                selectQuery.Append(" AND DESTINATION_CODE = '" + details.DESTINATION_CODE + "'");
            }
            else
            {
                selectQuery.Append(" AND DESTINATION_CODE IS NULL");
            }

           if(!string.IsNullOrWhiteSpace(details.FACTORY_SHUKKA_NUMBER))
            {
                selectQuery.Append(" AND FACTORY_SHUKKA_NUMBER = '" + details.FACTORY_SHUKKA_NUMBER + "'");
            }
            else
            {
                selectQuery.Append(" AND FACTORY_SHUKKA_NUMBER IS NULL");
            }
           if(!string.IsNullOrWhiteSpace(details.DELIVERY_NOTE_NUMBER))
            {
                selectQuery.Append(" AND DELIVERY_NOTE_NUMBER = '" + details.DELIVERY_NOTE_NUMBER + "'");
            }
            else
            {
                selectQuery.Append(" AND DELIVERY_NOTE_NUMBER IS NULL");
            }
            if(!string.IsNullOrWhiteSpace(details.CREATED_DATE))
            {
                selectQuery.Append(" AND CREATED_DATE = '" + details.CREATED_DATE + "'");
            }
            else
            {
                selectQuery.Append(" AND CREATED_DATE IS NULL");
            }
            if (!string.IsNullOrWhiteSpace(details.CREATED_TIME))
            {
                selectQuery.Append(" AND CREATED_TIME = '" + details.CREATED_TIME + "'");
            }
            else
            {
                selectQuery.Append(" AND CREATED_TIME IS NULL");
            }


            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                returnVal = true;
            }
            else
            {
                returnVal = false;
            }
            return returnVal;

        }

        public int GetKoubanCd(string kouban, string matlNameMaker, string dimD)
        {
            int returnVal; 
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" TK.KOUBAN_CD ");
            selectQuery.Append(" FROM V_KOUBAN_DETAILS TK  ");
            selectQuery.Append(" WHERE KOUBAN_COMPLETE_DATE IS NULL ");
            selectQuery.Append(" AND TK.KOUBAN = '" + kouban + "' ");
            selectQuery.Append(" AND TK.MATL_MAKER_NAME = '" + matlNameMaker + "'");
            selectQuery.Append(" AND TK.DIM_D = '" + dimD + "'");
            returnVal = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            
            return returnVal;
        }

        public void NyukoDataInsert(int koubanCd,DaidoFutouNyukoDetails details)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append("INSERT INTO TRN_FUTOU_NYUKO ( ");
            insertQuery.Append(" NYUKO_CD, ");
            insertQuery.Append(" KOUBAN_CD, ");
            insertQuery.Append(" NYUKO_DATE, ");
            insertQuery.Append(" QUANTITY, ");
            insertQuery.Append(" REMARKS, ");
            insertQuery.Append(" CHARGE, ");
            insertQuery.Append(" FUTOU_CD, ");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");
            insertQuery.Append(" CREATED_DATE, ");
            insertQuery.Append(" UPDATED_DATE ");
            insertQuery.Append(") VALUES (");

            insertQuery.Append("TRN_FUTOU_NYUKO_SEQ.NEXTVAL ");            
            insertQuery.Append(",'" + koubanCd + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.FUTO_NYUKO_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + details.QUANTITY_IN_KG + "'");
            insertQuery.Append(",'" + details.CONTRACT_NUMBER + "-" + details.CONTRACT_GYOUBAN + "'");
            insertQuery.Append(",'" + details.CHARGE + "'");
            insertQuery.Append(",'" + details.FUTOU_CD + "'");
            insertQuery.Append(",'" + 2 + "'");
            insertQuery.Append(",'" + 10 + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());


        }

        public int GetNyukoNo(int koubanCd, DaidoFutouNyukoDetails details)
        {
            int nyukoNo;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" NYUKO_CD ");
            selectQuery.Append(" FROM TRN_FUTOU_NYUKO ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" KOUBAN_CD = '" + koubanCd + "' ");            
            selectQuery.Append(" AND NYUKO_DATE = TO_DATE('" + (Convert.ToDateTime(details.FUTO_NYUKO_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD) + "','yyyy/mm/dd')");

            
            selectQuery.Append(" AND QUANTITY = '" + details.QUANTITY_IN_KG + "'");
            selectQuery.Append(" AND REMARKS = '" + details.CONTRACT_NUMBER + "-" + details.CONTRACT_GYOUBAN + "'");
            selectQuery.Append(" AND CHARGE = '" + details.CHARGE + "'");
            selectQuery.Append(" AND FUTOU_CD = '" + details.FUTOU_CD + "'");

            nyukoNo = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return nyukoNo;
        }

        public void UpdateNyukoNo(int nyukoNo, DaidoFutouNyukoDetails details)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_DAIDO_FUTO_NYUKO_PROC_LOG SET ");
            updateQuery.Append(" NYUKO_NO ='" + nyukoNo + "'");
            updateQuery.Append(" WHERE PROCESS_DATE_AND_TIME = TO_DATE('" + (Convert.ToDateTime(details.PROCESS_DATE_AND_TIME, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(" AND PROCESS_SERIAL_NUMBER = '" + details.PROCESS_SERIAL_NUMBER + "'");
            ExecuteNonQuery(updateQuery.ToString());          

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="kouban"></param>
        /// <returns></returns>
        public bool IsKoubanExists(string kouban)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" NVl(COUNT(*),0)  ");
            selectQuery.Append(" FROM V_KOUBAN_DETAILS   ");
            selectQuery.Append(" WHERE KOUBAN_COMPLETE_DATE IS NULL ");
            selectQuery.Append(" AND KOUBAN = '" + kouban + "' ");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                returnVal = true;
            }
            else
            {
                returnVal = false;
            }
            return returnVal;
        }
        public bool IsKoubanDeleted(string kouban)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM V_KOUBAN_DETAILS TK  ");
            selectQuery.Append(" WHERE KOUBAN_COMPLETE_DATE IS NOT NULL ");
            selectQuery.Append(" AND TK.KOUBAN = '" + kouban + "' ");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                returnVal = true;
            }
            else
            {
                returnVal = false;
            }
            return returnVal;
        }
        public bool IsMaterialNameMakerExists(string kouban, string matlNameMaker)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*)  ");
            selectQuery.Append(" FROM V_KOUBAN_DETAILS TK  ");
            selectQuery.Append(" WHERE KOUBAN_COMPLETE_DATE IS NULL ");
            selectQuery.Append(" AND TK.KOUBAN = '" + kouban + "' ");
            selectQuery.Append(" AND TK.MATL_MAKER_NAME = '" + matlNameMaker + "'");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                returnVal = true;
            }
            else
            {
                returnVal = false;
            }
            return returnVal;
        }

        public bool IsKoubanMaterialNameMakDimDExists(string kouban, string matlNameMaker, string dimD)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM V_KOUBAN_DETAILS TK  ");
            selectQuery.Append(" WHERE KOUBAN_COMPLETE_DATE IS NULL ");
            selectQuery.Append(" AND TK.KOUBAN = '" + kouban + "' ");
            selectQuery.Append(" AND TK.MATL_MAKER_NAME = '" + matlNameMaker + "'");
            selectQuery.Append(" AND TK.DIM_D = '" + dimD + "'");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                returnVal = true;
            }
            else
            {
                returnVal = false;
            }
            return returnVal;
        }

       
        public DaidoFutouNyukoDetails GetDataCount(DateTime ImportDate)
        {
            
            DaidoFutouNyukoDetails countDetails = new DaidoFutouNyukoDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM TRN_DAIDO_FUTO_NYUKO_PROC_LOG  ");            
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + ImportDate.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");
            countDetails.TOTAL_COUNT = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            StringBuilder selectErrorQuery = new StringBuilder();
            selectErrorQuery.Append(" SELECT ");
            selectErrorQuery.Append(" COUNT(*) ");
            selectErrorQuery.Append(" FROM TRN_DAIDO_FUTO_NYUKO_PROC_LOG  ");
            selectErrorQuery.Append(" WHERE EXECUTE_RESULT = 2 AND ");
            selectErrorQuery.Append(" TRUNC(PROCESS_DATE_AND_TIME) = TO_DATE('" + ImportDate.ToString("yyyy/MM/dd") + "', 'YYYY/MM/DD') ");
            countDetails.ERROR_COUNT = Convert.ToInt32(ExecuteScalar(selectErrorQuery.ToString()));

            return countDetails;
        }
    }

}
