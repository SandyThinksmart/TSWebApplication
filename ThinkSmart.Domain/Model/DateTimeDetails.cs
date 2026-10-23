using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Model
{
    public class DateTimeDetails
    {
        public DateTime? START_DATETIME { get; set; }
        public DateTime? END_DATETIME { get; set; }
        public string START_DATETIME_STRING { get; set; }
        public string END_DATETIME_STRING { get; set; }
        public int DATERANGE { get; set; }
        public DateTime RANGE_DATETIME { get; set; }

        public DateTime FORECAST_R { get; set; }
        public int CURRENT_R { get; set; }
        public int CURRENT_YYYY { get; set; }
        public int LAST_R { get; set; }
        public int LAST_YYYY { get; set; }
        public DateTime CURRENT_FORECAST_R { get; set; }
        public int m_dispatchMonth1 { get; set; }
        public int m_dispatchMonth2 { get; set; }
        public int m_dispatchMonth3 { get; set; }
        public int m_dispatchYear1 { get; set; }

        public int m_dispatchYear2 { get; set; }
        public int m_dispatchYear3 { get; set; }
        public int resultMonth { get; set; }
        public int resultYear { get; set; }
        public int count { get; set; }
    }
}
