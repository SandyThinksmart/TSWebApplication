using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class SessionHandlerRepository :BaseRepository, ISessionHandlerRepository
    {

        public SessionHandlerRepository(IConfiguration _configuration) : base(_configuration)
        {
        }
        public LoginStatus getActivityTime(string workstationName, string browserName, string randomvalue)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * ");
            selectQuery.Append("FROM TRN_LOGIN_STATUS ");
            selectQuery.Append("WHERE BROWSER_ID='" + browserName + "'AND COOKIES_UNIQUE_ID='" + randomvalue + "'");

            selectQuery.Append(" AND WORKSTATION_NAME='" + workstationName + "'");


            var result = ExecuteQuerySingleResult<LoginStatus>(selectQuery.ToString());

            return result;

        }
        public void deleteLoginInfo(string uniqueid)
        {
            StringBuilder deleteQuery = new StringBuilder();

            deleteQuery.Append(" DELETE FROM TRN_LOGIN_STATUS ");
            deleteQuery.Append(" WHERE COOKIES_UNIQUE_ID = ");
            deleteQuery.Append(" '" + uniqueid + "' ");
            ExecuteNonQuery(deleteQuery.ToString());

        }
        public void updateLastUsedTime(string uniqueid)
        {
            StringBuilder updateQuery = new StringBuilder();

            updateQuery.Append(" UPDATE TRN_LOGIN_STATUS SET");
            updateQuery.Append(" LAST_USED_TIME = TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','yyyy/MM/dd HH24:MI:SS')");
            updateQuery.Append(" WHERE COOKIES_UNIQUE_ID = ");
            updateQuery.Append(" '" + uniqueid + "' ");
            ExecuteNonQuery(updateQuery.ToString());

        }
    }

}
