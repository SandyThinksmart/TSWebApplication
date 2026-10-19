using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Linq;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class CSVGenerationRepository : BaseRepository, ICSVGenerationRepository
    {
        public CSVGenerationRepository(IConfiguration _configuration, IHostEnvironment hostEnvironment) : base(_configuration)
        {
        }
        public List<OutputCSVDetails> GetGroupTitleList()
        {
            List<OutputCSVDetails> result = new List<OutputCSVDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GROUP_TITLE AS GROUP_TITLE_NAME ");
            selectQuery.Append(" , TITLE ");
            selectQuery.Append(" , TITLE_OTHER ");
            selectQuery.Append(" FROM OUTPUT_CSV_SQL");
          //  selectQuery.Append(" GROUP BY GROUP_TITLE ");
            result = ExecuteQueryMultiResult<OutputCSVDetails>(selectQuery.ToString());
            return result;
        }
        public List<OutputCSVDetails> GetCSVTitleList(string groupTitleName)
        {
            List<OutputCSVDetails> result = new List<OutputCSVDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" OUTPUT_SQL_CD, ");
            selectQuery.Append(" SQL_PLSQL_TYPE, ");
            selectQuery.Append(" SQL_PLSQL_NAME, ");
            selectQuery.Append(" TITLE, ");
            selectQuery.Append(" TITLE_OTHER ");
            selectQuery.Append(" FROM OUTPUT_CSV_SQL");
            if (!string.IsNullOrEmpty(groupTitleName))
            {
                selectQuery.Append(" WHERE GROUP_TITLE = '" + groupTitleName + "'");
            }
            selectQuery.Append(" ORDER BY DISPLAY_PRIORITY ");

            result = ExecuteQueryMultiResult<OutputCSVDetails>(selectQuery.ToString());
            return result;
        }

        public List<OutputCSVDetails> GetConditionsList(int outputCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" OUTPUT_SQL_CD, ");
            selectQuery.Append(" SQL_PLSQL_TYPE, ");
            selectQuery.Append(" SQL_PLSQL_NAME, ");
            selectQuery.Append(" TITLE, ");
            selectQuery.Append(" TITLE_OTHER, ");
            selectQuery.Append(" DISPLAY_PRIORITY, ");
            selectQuery.Append(" CSV_FILE_NAME, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN1, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN1, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE1, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE1, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN2, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN2, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE2, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE2, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN3, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN3, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE3, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE3, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN4, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN4, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE4, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE4, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN5, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN5, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE5, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE5, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN6, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN6, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE6, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE6, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN7, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN7, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE7, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE7, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN8, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN8, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE8, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE8, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN9, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN9, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE9, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE9, ");
            selectQuery.Append(" DISPLAY_CONDITION_COLUMN10, ");
            selectQuery.Append(" WHERE_CONDITION_COLUMN10, ");
            selectQuery.Append(" WHERE_CONDITION_TYPE10, ");
            selectQuery.Append(" ON_OFF_FLAG_TYPE10, ");
            selectQuery.Append(" ADD_DATE_FLAG, ");
            selectQuery.Append(" ADD_TIME_FLAG, ");
            selectQuery.Append(" PAGE_SIZE, ");
            selectQuery.Append(" PAGE_ORIENTATION, ");
            selectQuery.Append(" REPORT_TITLE_FONT, ");
            selectQuery.Append(" REPORT_HEADER_FONT, ");
            selectQuery.Append(" REPORT_DETAILS_FONT ");
        
            selectQuery.Append(" FROM OUTPUT_CSV_SQL");
            if (outputCD > 0)
            {
                selectQuery.Append(" WHERE OUTPUT_SQL_CD = " + outputCD);
            }
            selectQuery.Append(" ORDER BY DISPLAY_PRIORITY ");

            var result = ExecuteQueryMultiResult<OutputCSVDetails>(selectQuery.ToString());
            return result;

        }
        public DataTable GetData(string query)
        {

            DataTable dt = ExecuteQueryDataTable(query.ToString());
            return dt;
        }
        public DataTable GetCSVProcedureData(StoredProcedureDetails pDetails)
        {
            DataTable dt = ExecuteStoredProcedureWithResult(pDetails);
            return dt;
        }
        public List<NameValue> GetLocationNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD AS CD,  ");
            query.Append(" RANK() OVER(ORDER BY CASE WHEN LOCATION_CD = 10 THEN 1 ELSE 2 END) AS RNK  ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG  ");
            query.Append(" FROM MST_LOCATION WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY RNK, DISPLAY_PRIORITY, ");
            query.Append(" LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        public List<NameValue> GetNextProcessList(int nextProcessCD)
        {

            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" MAX(NXT.PROCESS_TYPE_CD),");
            query.Append(" NXT.NEXT_PROCESS_TYPE_CD AS PROCESS_KUBUN_CD,");
            query.Append(" KUB.KUBUN_NAME AS KUBUN_NAME_JP,");
            query.Append(" KUB.KUBUN_NAME_EN AS KUBUN_NAME_EN");
            query.Append(" FROM MST_NEXT_PROCESS_CONFIGURATION NXT ");
            query.Append(" INNER JOIN MST_PROCESS_KUBUN KUB ON KUB.KUBUN_ID = NXT.NEXT_PROCESS_TYPE_CD");
            query.Append(" WHERE NEXT_PROCESS_TYPE_CD = " + nextProcessCD);
            query.Append(" GROUP BY NXT.NEXT_PROCESS_TYPE_CD, KUB.KUBUN_NAME, KUB.KUBUN_NAME_EN");

            var result = ExecuteQueryMultiResult<NameValue>(query.ToString());

            return result;
        }
    }
}
