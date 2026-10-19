using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProcessTypeRemarkUpdationRepository : BaseRepository, IProcessTypeRemarkUpdationRepository
    {
        private readonly string _connectionString;
        public ProcessTypeRemarkUpdationRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
        public List<ProcessTypeRemarkUpdationDetails> GetProcessKubunDetails()
        {
            List<ProcessTypeRemarkUpdationDetails> result = new List<ProcessTypeRemarkUpdationDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM MST_PROCESS_KUBUN");
            selectQuery.Append(" ORDER BY KUBUN_ID ASC ");
            result = ExecuteQueryMultiResult<ProcessTypeRemarkUpdationDetails>(selectQuery.ToString());
            return result;
        }
        public void ProcessKubunUpdateDetails(ProcessTypeRemarkUpdationDetails processTypeRemarkUpdationDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE MST_PROCESS_KUBUN SET ");
            updateQuery.Append(" REMARK_TEMPLATE ='" + processTypeRemarkUpdationDetails.REMARK_TEMPLATE + "'");
            updateQuery.Append(", USER_ID ='" + processTypeRemarkUpdationDetails.USER_ID + "'");
            updateQuery.Append(", UPDATED_DATE = SYSDATE ");

            updateQuery.Append(" WHERE KUBUN_ID = " + processTypeRemarkUpdationDetails.KUBUN_ID);
            ExecuteNonQuery(updateQuery.ToString());
        }
    }
}
