using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public class TSSortField
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefaultSort { get; set; }

        public int DisplayPosition { get; set; }
        public TSSortDirection SortOrder { get; set; }

        public static string ApplySortCriteria(string sortCriteria, string query)
        {
            throw new NotImplementedException();
        }
    }

    public enum TSSortDirection
    {
        Ascending,
        Descending
    }


}
