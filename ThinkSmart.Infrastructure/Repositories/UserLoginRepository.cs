using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;


namespace TSWebApplication.Infrastructure.Repositories
{
    public class UserLoginRepository : BaseRepository, IUserLoginRepository
    {
        private readonly string _connectionString;
        public UserLoginRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
       
        public UserDetails GetUserByUserIdPassword(string userName, string password)
        {
            UserDetails UserDetails1 = new UserDetails();
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append("SELECT * FROM MST_USER_LOGIN UL ");
            selectQuery.Append(" WHERE LOWER(UL.SYSTEM_NAME) = '" + userName + "' ");

            if (password.Trim() == string.Empty)
                selectQuery.Append(" AND UL.PASSWORD IS NULL ");
            else
                selectQuery.Append(" AND LOWER(UL.PASSWORD) ='" + password + "' ");


            //  UserDetails1 = ExecuteQuerySingleResult<UserDetails>(selectQuery.ToString());
            UserDetails1=   ExecuteQuerySingleResult<UserDetails>(selectQuery.ToString());
           return UserDetails1;
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

        public List<NameValue> GetUserLocationNameById(int userID)
        {
            List<NameValue> result = new List<NameValue>();
            var locCD = GetUserLocationById(userID);
            
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT SHORT_NAME AS Name"); 
            query.Append(" FROM MST_LOCATION ");
            query.Append(" WHERE LOCATION_CD=" + locCD);

            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
            
        }

        public List<NameValue> GetForwordingLocation()
        {
            int kubun = UIConstants.K_LOC_FACTORY;
            List<NameValue> result = new List<NameValue>();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT LOCATION_CD AS CD ");
            query.Append(", SHORT_NAME AS NAME ");
            query.Append(", IN_ACTIVE_FLAG AS FLAG ");
            query.Append(" FROM MST_LOCATION ");
            query.Append(" WHERE LOCATION_KUBUN = " + kubun + " ");
            query.Append(" AND IN_ACTIVE_FLAG = 0 ");
            query.Append(" ORDER BY DISPLAY_PRIORITY ");

            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        public List<NameValue> GetVehicleName()
        {
            List<NameValue> result = new List<NameValue>();

            StringBuilder query = new StringBuilder();
            query.Append("SELECT VEHICLE_CD AS CD ");
            query.Append(", SHORT_NAME AS NAME ");
            query.Append(", IN_ACTIVE_FLAG AS FLAG ");
            query.Append(" FROM MST_VEHICLE ");
            query.Append(" WHERE IN_ACTIVE_FLAG = 0 ");
            query.Append(" ORDER BY YOBIGANA,YOBINA,VEHICLE_NAME");

           result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        
    }
}
