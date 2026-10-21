using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class OutputCSVDetails
    {
        public int OUTPUT_SQL_CD { get; set; }
        public short SQL_PLSQL_TYPE { get; set; }
        public string SQL_PLSQL_NAME { get; set; }
        public string GROUP_TITLE_NAME { get; set; }
        public string TITLE { get; set; }
        public string TITLE_OTHER { get; set; }
        public short DISPLAY_PRIORITY { get; set; }
        public string CSV_FILE_NAME { get; set; }
        public List<WhereConditions> WHERE_CONDITION_LIST { get; set; }
        public string DISPLAY_CONDITION_COLUMN1 { get; set; }
        public string WHERE_CONDITION_COLUMN1 { get; set; }
        public short? WHERE_CONDITION_TYPE1 { get; set; }
        public short? ON_OFF_FLAG_TYPE1 { get; set; }
        public string DISPLAY_CONDITION_COLUMN2 { get; set; }
        public string WHERE_CONDITION_COLUMN2 { get; set; }
        public short? WHERE_CONDITION_TYPE2 { get; set; }
        public short? ON_OFF_FLAG_TYPE2 { get; set; }
        public string DISPLAY_CONDITION_COLUMN3 { get; set; }
        public string WHERE_CONDITION_COLUMN3 { get; set; }
        public short? WHERE_CONDITION_TYPE3 { get; set; }
        public short? ON_OFF_FLAG_TYPE3 { get; set; }
        public string DISPLAY_CONDITION_COLUMN4 { get; set; }
        public string WHERE_CONDITION_COLUMN4 { get; set; }
        public short? WHERE_CONDITION_TYPE4 { get; set; }
        public short? ON_OFF_FLAG_TYPE4 { get; set; }
        public string DISPLAY_CONDITION_COLUMN5 { get; set; }
        public string WHERE_CONDITION_COLUMN5 { get; set; }
        public short? WHERE_CONDITION_TYPE5 { get; set; }
        public short? ON_OFF_FLAG_TYPE5 { get; set; }
        public string DISPLAY_CONDITION_COLUMN6 { get; set; }
        public string WHERE_CONDITION_COLUMN6 { get; set; }
        public short? WHERE_CONDITION_TYPE6 { get; set; }
        public short? ON_OFF_FLAG_TYPE6 { get; set; }
        public string DISPLAY_CONDITION_COLUMN7 { get; set; }
        public string WHERE_CONDITION_COLUMN7 { get; set; }
        public short? WHERE_CONDITION_TYPE7 { get; set; }
        public short? ON_OFF_FLAG_TYPE7 { get; set; }
        public string DISPLAY_CONDITION_COLUMN8 { get; set; }
        public string WHERE_CONDITION_COLUMN8 { get; set; }
        public short? WHERE_CONDITION_TYPE8 { get; set; }
        public short? ON_OFF_FLAG_TYPE8 { get; set; }
        public string DISPLAY_CONDITION_COLUMN9 { get; set; }
        public string WHERE_CONDITION_COLUMN9 { get; set; }
        public short? WHERE_CONDITION_TYPE9 { get; set; }
        public short? ON_OFF_FLAG_TYPE9 { get; set; }
        public string DISPLAY_CONDITION_COLUMN10 { get; set; }
        public string WHERE_CONDITION_COLUMN10 { get; set; }
        public short? WHERE_CONDITION_TYPE10 { get; set; }
        public short? ON_OFF_FLAG_TYPE10 { get; set; }
        public short ADD_DATE_FLAG { get; set; }
        public short ADD_TIME_FLAG { get; set; }
        public string PAGE_SIZE { get; set; }
        public string PAGE_ORIENTATION { get; set; }
        public short? REPORT_TITLE_FONT { get; set; }
        public short? REPORT_HEADER_FONT { get; set; }
        public short? REPORT_DETAILS_FONT { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public int? USER_ID { get; set; }

    }
    public class WhereConditions
    {
        public short SQL_PLSQL_TYPE { get; set; }
        public string DISPLAY_CONDITION_COLUMN { get; set; }
        public string WHERE_CONDITION_COLUMN { get; set; }
        public string WHERE_CONDITION_VALUE { get; set; }
        public short? WHERE_CONDITION_TYPE { get; set; }
        public short? ON_OFF_FLAG_TYPE { get; set; }
        

    }
}
