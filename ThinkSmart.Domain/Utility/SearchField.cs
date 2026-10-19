using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public enum SearchType
    {
        Text,
        Boolean,
        Number,
        NullableNumber,
        Date,
        NullableDate,
        Null,
        PrintDate,
        CompleteDate,
        DateTime,
        IsNull,
        IsNotNull,
        Custom
    }

    public enum SearchCondition
    {
        Equal,
        Like,
        GreaterThan,
        LessThan,
        GreaterThanEqual,
        LessThanEqual,
        In

    }
    public class SearchField
    {
        public string FieldName { get; set; }
        public string ReferenceTable { get; set; }
        public Type FieldType { get; set; }
        public SearchType SearchType { get; set; }
        public string SearchText1 { get; set; }
        public string SearchText2 { get; set; }
        public string OtherProperty { get; set; }
        public string SearchCondition { get; set; }
        public bool IsOnlyInitialSearch { get; set; }


    }
}
