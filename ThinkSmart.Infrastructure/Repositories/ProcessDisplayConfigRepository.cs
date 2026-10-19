using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProcessDisplayConfigRepository : BaseRepository, IProcessDisplayConfigRepository
    {
        private readonly string _connectionString;
        public ProcessDisplayConfigRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }
        public List<ProcessDisplayConfigDetails> GetProcessKubunDetails()
        {
            List<ProcessDisplayConfigDetails> result = new List<ProcessDisplayConfigDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM MST_PROCESS_KUBUN");
            selectQuery.Append(" ORDER BY KUBUN_ID ASC ");
            result = ExecuteQueryMultiResult<ProcessDisplayConfigDetails>(selectQuery.ToString());
            return result;
        }

        public void SaveProcessKubun(ProcessDisplayConfigDetails processDisplayConfigDetails)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO MST_NEXT_PROCESS_CONFIGURATION(");
            insertQuery.Append(" PROCESS_TYPE_CD");
            insertQuery.Append(",NEXT_PROCESS_TYPE_CD");
            insertQuery.Append(",SHOW_PRIORITY");
            insertQuery.Append(", USER_ID ");
            insertQuery.Append(", UPDATED_DATE");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
           
            insertQuery.Append(processDisplayConfigDetails.PROCESS_TYPE_CD );
            insertQuery.Append("," + processDisplayConfigDetails.NEXT_PROCESS_TYPE_CD);
            insertQuery.Append("," + processDisplayConfigDetails.SHOW_PRIORITY);
            insertQuery.Append("," + processDisplayConfigDetails.USER_ID);
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
        }       
       
        public void DeleteExistedKubun(int process)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  MST_NEXT_PROCESS_CONFIGURATION ");
            deleteQuery.Append(" WHERE PROCESS_TYPE_CD =" + process );
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public List<ProcessDisplayConfigDetails> GetActiveNextProcessKubunDetails(long Process_Type_CD)
        {
            List<ProcessDisplayConfigDetails> result = new List<ProcessDisplayConfigDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PROCESS_TYPE_CD, ");
            selectQuery.Append(" NEXT_PROCESS_TYPE_CD,");
            selectQuery.Append(" SHOW_PRIORITY");
            selectQuery.Append(" FROM MST_NEXT_PROCESS_CONFIGURATION  WHERE PROCESS_TYPE_CD =" + Process_Type_CD);
            result = ExecuteQueryMultiResult<ProcessDisplayConfigDetails>(selectQuery.ToString());
            return result;
        }
    }
}
