using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public static class DynamicSearchProvider
    {
        public static string ApplySearchCriteria(SearchField searchField, string query)
        {
            switch (searchField.SearchType)
            {
                case SearchType.Text:
                    query = ApplyTextSearch(searchField, query);
                    break;
                case SearchType.Boolean:
                    break;
                case SearchType.Number:
                    query = ApplyNumberSearch(searchField, query);
                    break;
                case SearchType.NullableNumber:
                    query = ApplyNullableNumberSearch(searchField, query);
                    break;
                case SearchType.Date:
                    query = ApplyDateSearch(searchField, query);
                    break;
                case SearchType.DateTime:
                    query = ApplyDateTimeSearch(searchField, query);
                    break;
                case SearchType.NullableDate:
                    query = ApplyNullableDateSearch(searchField, query);
                    break;
                case SearchType.Null:
                    query = string.Empty;
                    break;
                case SearchType.IsNull:
                    query = ApplyIsNullSearch(searchField, query);
                    break;
                case SearchType.IsNotNull:
                    query = ApplyIsNotNullSearch(searchField, query);
                    break;
                case SearchType.PrintDate:
                    query = ApplyPrintDateSearch(searchField, query);
                    break;
                case SearchType.CompleteDate:
                    query = ApplyPrintDateSearch(searchField, query);
                    break;
                default:
                    break;
            }

            return query;
        }

        private static string ApplyNullableDateSearch(SearchField searchField, string query)
        {
            //  return "to_date(to_char(" + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd')  Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
            if (string.IsNullOrEmpty(searchField.SearchText2))
            {
                if (searchField.ReferenceTable != null)
                {
                    return "to_date(to_char(" + searchField.ReferenceTable + "." + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') = to_date('" + searchField.SearchText1 + "', 'yyyy/MM/dd')";
                    //  return searchField.ReferenceTable + "." + searchField.FieldName + "= to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
                }
                else
                    return "to_date(to_char(" + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') = to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
                // return  searchField.FieldName + " = to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
            }
            else
            {
                if (searchField.ReferenceTable != null)
                {

                    return "to_date(to_char(" + searchField.ReferenceTable + "." + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                }
                else
                {
                    return "to_date(to_char(" + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                    // return searchField.FieldName + " Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                }
            }

        }
        private static string ApplyIsNullSearch(SearchField searchField, string query)
        {
            if (searchField.ReferenceTable != null)
            {
                return searchField.ReferenceTable + "." + searchField.FieldName + " IS NULL ";
            }
            else
            {
                return searchField.FieldName + " IS NULL ";
            }
        }


        private static string ApplyIsNotNullSearch(SearchField searchField, string query)
        {
            if (searchField.ReferenceTable != null)
            {
                return searchField.ReferenceTable + "." + searchField.FieldName + " IS NOT NULL ";
            }
            else
            {
                return searchField.FieldName + " IS NOT NULL ";
            }
        }
        private static string ApplyPrintDateSearch(SearchField searchField, string query)
        {
            if (searchField.SearchText1 == "0")
            {
                if (searchField.ReferenceTable != null)
                {
                    return searchField.ReferenceTable + "." + searchField.FieldName + " IS NULL ";
                }
                else
                {
                    return searchField.FieldName + " IS NULL ";
                }
            }
            else if (searchField.SearchText1 == "1")
            {
                if (searchField.ReferenceTable != null)
                {
                    return searchField.ReferenceTable + "." + searchField.FieldName + " IS NOT NULL  ";
                }
                else
                {
                    return searchField.FieldName + " IS NOT NULL ";
                }
            }
            else
            {
                return string.Empty;
            }

        }
        private static string ApplyDateSearch(SearchField searchField, string query)
        {
            string cDate = "to_date(to_char(" + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd')";

            if (string.IsNullOrEmpty(searchField.SearchText2))
            {
                if (searchField.ReferenceTable != null)
                {
                    return "to_date(to_char(" + searchField.ReferenceTable + "." + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') = to_date('" + searchField.SearchText1 + "', 'yyyy/MM/dd')";
                    //  return searchField.ReferenceTable + "." + searchField.FieldName + "= to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
                }
                else
                    return "to_date(to_char(" + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') = to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
                // return  searchField.FieldName + " = to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
            }
            else
            {
                if (searchField.ReferenceTable != null)
                {

                    return "to_date(to_char(" + searchField.ReferenceTable + "." + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                }
                else
                {
                    return "to_date(to_char(" + searchField.FieldName + ", 'yyyy/MM/dd'), 'yyyy/MM/dd') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                    // return searchField.FieldName + " Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                }
            }
        }

        private static string ApplyDateTimeSearch(SearchField searchField, string query)
        {


            if (string.IsNullOrEmpty(searchField.SearchText2))
            {
                if (searchField.ReferenceTable != null)
                {
                    return "to_date(" + "to_char(" + searchField.ReferenceTable + "." + searchField.FieldName + ", 'yyyy/MM/dd hh24:mi'),'yyyy/MM/dd hh24:mi') = to_date('" + searchField.SearchText1 + "', 'yyyy/MM/dd hh24:mi')";
                    //  return searchField.ReferenceTable + "." + searchField.FieldName + "= to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
                }
                else
                    return "to_date(" + "to_char(" + searchField.FieldName + ", 'yyyy/MM/dd hh24:mi'),'yyyy/MM/dd hh24:mi') = to_date('" + searchField.SearchText1 + "','yyyy/MM/dd hh24:mi')";
                // return  searchField.FieldName + " = to_date('" + searchField.SearchText1 + "','yyyy/MM/dd')";
            }
            else
            {
                if (searchField.ReferenceTable != null)
                {

                    return "to_date(" + "to_char(" + searchField.ReferenceTable + "." + searchField.FieldName + ", 'yyyy/MM/dd hh24:mi'),'yyyy/MM/dd hh24:mi') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd hh24:mi') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd hh24:mi')";
                }
                else
                {
                   // return "to_date(" + searchField.FieldName + ",'yyyy/MM/dd hh24:mi') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd hh24:mi') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd hh24:mi')";
                    // return searchField.FieldName + " Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd')";
                    return "to_date("+"to_char(" + searchField.FieldName + ",'yyyy/MM/dd hh24:mi'),'yyyy/MM/dd hh24:mi') Between to_date('" + searchField.SearchText1 + "','yyyy/MM/dd hh24:mi') AND to_date('" + searchField.SearchText2 + "','yyyy/MM/dd hh24:mi')";

                    //to_date(to_char(fi.insp_date, 'yyyy/MM/dd hh24:mi'), 'yyyy/MM/dd hh24:mi') BETWEEN to_date('2026/01/09 04:11', 'yyyy/MM/dd hh24:mi') AND to_date
                    //  ('2026/01/09 11:21', 'yyyy/MM/dd hh24:mi')
                }
            }
        }

        private static string ApplyNullableNumberSearch(SearchField searchField, string query)
        {
            if (string.IsNullOrEmpty(searchField.SearchText2))
            {
                if (searchField.ReferenceTable != null)
                {
                    return searchField.ReferenceTable + "." + searchField.FieldName + " = " + searchField.SearchText1;
                }
                else
                    return searchField.FieldName + " = " + searchField.SearchText1;
            }
            else
            {
                if (searchField.ReferenceTable != null)
                {

                    return searchField.ReferenceTable + "." + searchField.FieldName + " Between " + searchField.SearchText1 + " AND " + searchField.SearchText2;
                }
                else
                    return searchField.FieldName + " Between " + searchField.SearchText1 + " AND " + searchField.SearchText2;

            }
        }

        private static string ApplyNumberSearch(SearchField searchField, string query)
        {

            if (string.IsNullOrEmpty(searchField.SearchText2))
            {
                if (searchField.ReferenceTable != null)
                {
                    if (searchField.SearchCondition != null)
                    {
                        switch (searchField.SearchCondition.ToString())
                        {
                            case "GreaterThan":
                                return searchField.ReferenceTable + "." + searchField.FieldName + " > " + searchField.SearchText1;

                            case "LessThan":
                                return searchField.ReferenceTable + "." + searchField.FieldName + " < " + searchField.SearchText1;
                            case "GreaterThanEqual":
                                return searchField.ReferenceTable + "." + searchField.FieldName + " >= " + searchField.SearchText1;
                            case "LessThanEqual":
                                return searchField.ReferenceTable + "." + searchField.FieldName + " <= " + searchField.SearchText1;
                            case "In":

                                return searchField.ReferenceTable + "." + searchField.FieldName + " IN (" + searchField.SearchText1 + ")";

                            default:
                                return searchField.ReferenceTable + "." + searchField.FieldName + " = " + searchField.SearchText1;
                        }
                    }
                    else
                    {
                        return searchField.ReferenceTable + "." + searchField.FieldName + " = " + searchField.SearchText1;

                    }
                }
                else
                {

                    if (searchField.SearchCondition != null)
                    {
                        switch (searchField.SearchCondition.ToString())
                        {
                            case "GreaterThan":
                                return searchField.FieldName + " > " + searchField.SearchText1;
                            case "LessThan":
                                return searchField.FieldName + " < " + searchField.SearchText1;
                            case "GreaterThanEqual":
                                return searchField.FieldName + " >= " + searchField.SearchText1;
                            case "LessThanEqual":
                                return searchField.FieldName + " <= " + searchField.SearchText1;
                            case "In":
                                return searchField.FieldName + " IN (" + searchField.SearchText1 + ")";
                            default:
                                return searchField.FieldName + " = " + searchField.SearchText1;
                        }
                    }
                    else
                    {
                        return searchField.FieldName + " = " + searchField.SearchText1;
                    }
                }
            }
            else
            {
                if (searchField.ReferenceTable != null)
                {

                    return searchField.ReferenceTable + "." + searchField.FieldName + " Between " + searchField.SearchText1 + " AND " + searchField.SearchText2;
                }
                else
                    return searchField.FieldName + " Between " + searchField.SearchText1 + " AND " + searchField.SearchText2;

            }
        }

        private static string ApplyNumberRangeSearc(SearchField searchField, string query)
        {
            if (searchField.ReferenceTable != null)
            {
                return searchField.ReferenceTable + "." + searchField.FieldName + " Between " + searchField.SearchText1 + " AND " + searchField.SearchText2;
            }
            else
                return searchField.FieldName + " Between " + searchField.SearchText1 + " AND " + searchField.SearchText2;
        }

        private static string ApplyTextSearch(SearchField searchField, string query)
        {
            if (searchField.ReferenceTable != null)
            {

                if (searchField.SearchCondition == SearchCondition.Equal.ToString())
                {
                    return "LOWER(" + searchField.ReferenceTable + "." + searchField.FieldName + ")" + " = LOWER( '" + searchField.SearchText1 + "')";
                }
                else
                {

                    // string _searchText = searchField.SearchText1.Replace("*", "%");
                    return "LOWER(" + searchField.ReferenceTable + "." + searchField.FieldName + ") " + " Like LOWER('%" + searchField.SearchText1 + "%')";

                }
            }
            else
            {
                //if (searchField.SearchText1.Contains("*"))
                //{
                //    string _searchText = searchField.SearchText1.Replace("*", "%");
                //    return "LOWER(" + searchField.FieldName + ") " + " Like LOWER('" + _searchText + "')";
                //}
                //else
                //{
                //    if (searchField.FieldName != null)
                //        return "LOWER(" + searchField.FieldName + ") " + " = LOWER( '" + searchField.SearchText1 + "')";
                //    else
                //        return string.Empty;
                //}
                if (searchField.SearchCondition == SearchCondition.Equal.ToString())
                {
                    return "LOWER(" + searchField.FieldName + ")" + " = LOWER( '" + searchField.SearchText1 + "')";
                }
                else
                {
                    if (searchField.FieldName != null)
                        return "LOWER(" + searchField.FieldName + ") " + " Like LOWER('%" + searchField.SearchText1 + "%' )";
                    else
                        return string.Empty;
                }
            }

        }


    }
}
