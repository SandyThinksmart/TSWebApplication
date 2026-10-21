using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class VehicleMasterRepository: BaseRepository, IVehicleMasterRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        public VehicleMasterRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
        }


        public long registerVehicle(VehicleDetails request)
        {
            VehicleDetails result = new VehicleDetails();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO MST_VEHICLE(");
            insertQuery.Append(" VEHICLE_CD");
            insertQuery.Append(" ,VEHICLE_NAME ");
            insertQuery.Append(" ,DRIVER_NAME");
            insertQuery.Append(" ,SHORT_NAME");
            insertQuery.Append(" ,YOBIGANA");
            insertQuery.Append(" ,YOBINA");
            insertQuery.Append(" ,VEHICLE_NUMBER");
            insertQuery.Append(" ,DISPLACEMENT");
            insertQuery.Append(" ,MAX_LOAD");
            insertQuery.Append(", TELEPHONE_NO ");
            insertQuery.Append(", TRANSPORT_COMPANY_NAME");
            insertQuery.Append(", REMARKS");
            insertQuery.Append(", USER_ID");
            insertQuery.Append(" ,CREATED_DATE");
            insertQuery.Append(" ,UPDATED_DATE ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append(" MST_VEHICLE_SEQ.NEXTVAL ");
            insertQuery.Append(",'" + request.VEHICLE_NAME + "'");
            insertQuery.Append(",'" + request.DRIVER_NAME + "'");
            insertQuery.Append(",'" + request.SHORT_NAME + "'");
            insertQuery.Append(",UTL_I18N.TRANSLITERATE('" + request.YOBIGANA + "', 'kana_hwkatakana')");
            insertQuery.Append(",UTL_I18N.TRANSLITERATE('" + request.YOBINA + "', 'kana_hwkatakana')"); 
            //insertQuery.Append(",'" + request.YOBIGANA + "'");
            //insertQuery.Append(",'" + request.YOBINA + "'");
            insertQuery.Append(",'" + request.VEHICLE_NUMBER + "'");
            insertQuery.Append(",'" + request.DISPLACEMENT + "'");
            insertQuery.Append(",'" + request.MAX_LOAD + "'");
            insertQuery.Append(",'" + request.TELEPHONE_NO + "'");
            insertQuery.Append(",'" + request.TRANSPORT_COMPANY_NAME + "'");
            insertQuery.Append(",'" + request.REMARKS + "'");
            insertQuery.Append(",'" + request.USER_ID + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MAX(VEHICLE_CD) FROM MST_VEHICLE");
            int VEHICLE_CD = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                       
            return VEHICLE_CD;
        }


        public VehicleDetails GetVehicleById(long vehicleId)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MV.VEHICLE_CD, MV.VEHICLE_NAME, MV.DRIVER_NAME, MV.SHORT_NAME , MV.YOBIGANA , MV.YOBINA , MV.VEHICLE_NUMBER , MV.DISPLACEMENT , MV.MAX_LOAD , MV.TELEPHONE_NO , MV.TRANSPORT_COMPANY_NAME , MV.REMARKS , MV.IN_ACTIVE_FLAG AS UN_USE_FLAG , MV.CREATED_DATE , MV.UPDATED_DATE");
            selectQuery.Append(" FROM MST_VEHICLE MV ");
            
            selectQuery.Append(" WHERE MV.VEHICLE_CD =" + vehicleId + " ");
                      
             return ExecuteQuerySingleResult<VehicleDetails>(selectQuery.ToString());
        }

        public bool DeleteVehicleDetailsById(long vehicleId)
        {
            StringBuilder deleteQuery = new StringBuilder();

            deleteQuery.Append("DELETE FROM MST_VEHICLE ");
            deleteQuery.Append("WHERE VEHICLE_CD = " + vehicleId);
            try
            {
                ExecuteNonQuery(deleteQuery.ToString());
                return true;
            }
            catch (OracleException ex)
            {
                throw;
            }

        }

        //public bool IsVehicleUsed(long vehicleId)
        //{
        //    StringBuilder query = new StringBuilder();

        //    query.Append("SELECT COUNT(*) ");
        //    query.Append("FROM TRN_WIP_AND_PROD_TRANSFER ");
        //    query.Append("WHERE VEHICLE_CD = ");
        //    query.Append(vehicleId);

        //    try
        //    {
        //        int count = ExecuteQuerySingleResult<int>(query.ToString());

        //        return count > 0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool ChangeVehicleStatus(long vehicleId)
        {
            StringBuilder updateQuery = new StringBuilder();

            updateQuery.Append("UPDATE MST_VEHICLE ");
            updateQuery.Append("SET IN_ACTIVE_FLAG = CASE ");
            updateQuery.Append("WHEN IN_ACTIVE_FLAG = 0 THEN 1 ");
            updateQuery.Append("ELSE 0 ");
            updateQuery.Append("END ");
            updateQuery.Append("WHERE VEHICLE_CD = " + vehicleId);

            try
            {
                ExecuteNonQuery(updateQuery.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public List<VehicleDetails> FetchVehicleDetails()
        //{
        //    StringBuilder selectQuery = new StringBuilder();
           
        //    selectQuery.Append("SELECT  ");
        //    selectQuery.Append("VEHICLE_CD,  ");
        //    selectQuery.Append("VEHICLE_NAME,  ");
        //    selectQuery.Append("DRIVER_NAME,  ");
        //    selectQuery.Append("SHORT_NAME,  ");
        //    selectQuery.Append("YOBIGANA,  ");
        //    selectQuery.Append("YOBINA,  ");
        //    selectQuery.Append("VEHICLE_NUMBER,  ");
        //    selectQuery.Append("DISPLACEMENT,  ");
        //    selectQuery.Append("MAX_LOAD,  ");
        //    selectQuery.Append("TELEPHONE_NO, ");
        //    selectQuery.Append("TRANSPORT_COMPANY_NAME,  ");
        //    selectQuery.Append("REMARKS, ");
        //    selectQuery.Append("IN_ACTIVE_FLAG AS UN_USE_FLAG, "); 
        //    selectQuery.Append("TO_CHAR(CREATED_DATE, 'YYYY/MM/DD') AS CREATED_DATE_STRING,");
        //    selectQuery.Append("TO_CHAR(UPDATED_DATE, 'YYYY/MM/DD') AS UPDATED_DATE_STRING ");

        //    selectQuery.Append(" FROM MST_VEHICLE ");

        //    return ExecuteQueryMultiResult<VehicleDetails>(selectQuery.ToString());

        //}

        public List<VehicleDetails> FetchVehicleDetails(List<SearchField> searchCriteria, string sortCiteria)
        {
            List<VehicleDetails> result = new List<VehicleDetails>();
            int decisionValue = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ");
            sb.Append("VEHICLE_CD, ");
            sb.Append("VEHICLE_NAME, ");
            sb.Append("DRIVER_NAME, ");
            sb.Append("SHORT_NAME, ");
            sb.Append("YOBIGANA, ");
            sb.Append("YOBINA, ");
            sb.Append("VEHICLE_NUMBER, ");
            sb.Append("DISPLACEMENT, ");
            sb.Append("MAX_LOAD, ");
            sb.Append("TELEPHONE_NO, ");
            sb.Append("TRANSPORT_COMPANY_NAME, ");
            sb.Append("REMARKS, ");
            sb.Append("IN_ACTIVE_FLAG AS UN_USE_FLAG, ");
            sb.Append("TO_CHAR(CREATED_DATE, 'YYYY/MM/DD') AS CREATED_DATE_STRING, ");
            sb.Append("TO_CHAR(UPDATED_DATE, 'YYYY/MM/DD') AS UPDATED_DATE_STRING ");

            sb.Append("FROM MST_VEHICLE ");
            sb.Append("WHERE 1 = 1 ");

            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                foreach (var field in searchCriteria)
                {
                    sb.Append(" AND ");
                    sb.Append(DynamicSearchProvider.ApplySearchCriteria(field, sb.ToString()));
                }
            }

            //  Sorting
            if (!string.IsNullOrEmpty(sortCiteria))
                sb = new StringBuilder(Helper.ApplySortCriteria(sortCiteria, sb.ToString()));
            else
                sb.Append(" ORDER BY YOBIGANA ");


            return ExecuteQueryMultiResult<VehicleDetails>(sb.ToString());
            //return result;
        }

        public long UpdateVehicleDetails(VehicleDetails request)
        {
            StringBuilder updateQuery = new StringBuilder();

            updateQuery.Append(" UPDATE MST_VEHICLE SET ");
            updateQuery.Append(" VEHICLE_NAME = '" + request.VEHICLE_NAME + "'");
            updateQuery.Append(" ,DRIVER_NAME = '" + request.DRIVER_NAME + "'");
            updateQuery.Append(" ,SHORT_NAME = '" + request.SHORT_NAME + "'");
            updateQuery.Append(" ,YOBIGANA = UTL_I18N.TRANSLITERATE('" + request.YOBIGANA + "', 'kana_hwkatakana')");
            updateQuery.Append(" ,YOBINA = UTL_I18N.TRANSLITERATE('" + request.YOBINA + "', 'kana_hwkatakana')");
            //updateQuery.Append(" ,YOBIGANA = '" + request.YOBIGANA + "'");
            //updateQuery.Append(" ,YOBINA = '" + request.YOBINA + "'");
            updateQuery.Append(" ,VEHICLE_NUMBER = '" + request.VEHICLE_NUMBER + "'");
            updateQuery.Append(" ,DISPLACEMENT = '" + request.DISPLACEMENT + "'");
            updateQuery.Append(" ,MAX_LOAD = '" + request.MAX_LOAD + "'");
            updateQuery.Append(" ,TELEPHONE_NO = '" + request.TELEPHONE_NO + "'");
            updateQuery.Append(" ,TRANSPORT_COMPANY_NAME = '" + request.TRANSPORT_COMPANY_NAME + "'");
            updateQuery.Append(" ,REMARKS = '" + request.REMARKS + "'");
            updateQuery.Append(" ,USER_ID = '" + request.USER_ID + "'");
            updateQuery.Append(" ,UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE VEHICLE_CD = " + request.VEHICLE_CD);

            ExecuteNonQuery(updateQuery.ToString());

            return request.VEHICLE_CD;
        }
    }
}
