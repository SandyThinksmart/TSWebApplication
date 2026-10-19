using System;
using System.Collections.Generic;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public class Helper
    {
        public static string ParseSortCriteria(List<TSSortField> sortFields)
        {
            if (sortFields != null && sortFields.Count > 0)
            {
                var sortCriteria = new List<string>();
                foreach (var sortField in sortFields)
                {
                    var sortDirection = (sortField.SortOrder == TSSortDirection.Descending) ?
                                        Constants.SortOrder.DESC : Constants.SortOrder.ASC;
                    if (sortField.FieldName.Contains(',') && !sortField.FieldName.ToLower().StartsWith("nvl"))
                    {
                        string[] _sortFields = sortField.FieldName.Split(',');
                        foreach (string _field in _sortFields)
                        {
                            var sortExpression = _field + " " + sortDirection;
                            sortCriteria.Add(sortExpression);
                        }
                    }
                    else
                    {
                        var sortExpression = sortField.FieldName + " " + sortDirection;
                        sortCriteria.Add(sortExpression);
                    }
                }

                return string.Join<string>(", ", sortCriteria);
            }

            return null;
        }

        public static string ApplySortCriteria(string sortCriteria, string query)
        {
            query = query + " ORDER BY " + sortCriteria;
            return query;
        }

        public static string GetProductPatternFormat(float patternValue, int patternMinMaxKubun, decimal? patternaMinValue, decimal? patternMaxValue, int actualPattern)
        {
            string pattern = string.Empty;
            if (patternValue > 0)
            {
                pattern = Convert.ToString(patternValue);
                if (patternMinMaxKubun > 0)
                {
                    pattern += Constants.StringValues.SPACE + GetMinMaxValuebyKubun(patternMinMaxKubun);
                }
            }
            else if (patternaMinValue> 0)
            {
                pattern = patternaMinValue + Constants.StringValues.TILDE_SIGN + patternMaxValue;
            }
            else if (actualPattern > 0)
            {
                pattern = UIConstants.GetGlobalResourceString("lbl_ActualPattern");
                
            }


            return pattern;
        }

        public static string GetHardnessProductPatternFormat(int patternCode1, int? patternHardness, int patternMinMaxKubun, int? patternCode2, decimal? patternaMinValue, decimal? patternMaxValue, int actualPattern)
        {
            string pattern = string.Empty;
            if (patternCode1 > 0)
            {
                pattern = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(patternCode1)).ToString();
                if (patternHardness > 0)
                {
                    pattern += Constants.StringValues.SPACE + patternHardness;
                }
                if (patternMinMaxKubun > 0)
                {
                    pattern += Constants.StringValues.SPACE + GetMinMaxValuebyKubun(Convert.ToInt16(patternMinMaxKubun));
                }
            }
            else if (patternCode2 > 0)
            {
                pattern = UIUtils.GetStandardHardnessKubunValue(Convert.ToString(patternCode2)).ToString();
                if (patternaMinValue > 0)
                {
                    pattern += Constants.StringValues.SPACE + patternaMinValue.ToString();
                }
                if (patternMaxValue > 0)
                {
                    pattern += Constants.StringValues.TILDE_SIGN + patternMaxValue.ToString();
                }
            }
            else if (actualPattern > 0)
            {
                pattern = UIConstants.GetGlobalResourceString("lbl_ActualPattern");
            }


            return pattern;
        }

        public static string GetDecarbProductPatternFormat(int decarb1, float decarbVal1, int decarb2, float decarbVal2, int decarb3, float decarbVal3)
        {
            string pattern = string.Empty;
            string _decarbString = string.Empty;

            if (decarbVal1 > 0)
            {
                if (decarb1 == 1)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMF");
                }
                else if (decarb1 == 2)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMS");
                }
                else if (decarb1 == 3)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMT");
                }
                _decarbString += Constants.StringValues.SPACE + Convert.ToDecimal(decarbVal1).ToString("0.###") + Constants.StringValues.SPACE + UIConstants.GetGlobalResourceString("lbl_LessThan");

            }

            if (decarbVal2 > 0)
            {
                if (decarbVal1 > 0)
                {
                    _decarbString += ", ";
                }
                if (decarb2 == 1)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMF");
                }
                else if (decarb2 == 2)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMS");
                }
                else if (decarb2 == 3)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMT");
                }
                _decarbString += Constants.StringValues.SPACE + Convert.ToDecimal(decarbVal2).ToString("0.###") + Constants.StringValues.SPACE + UIConstants.GetGlobalResourceString("lbl_LessThan");

            }

            if (decarbVal3 > 0)
            {
                if (decarbVal1 > 0 || decarbVal2 > 0)
                {
                    _decarbString += ", ";
                }

                if (decarb3 == 1)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMF");
                }
                else if (decarb3 == 2)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMS");
                }
                else if (decarb3 == 3)
                {
                    _decarbString += UIConstants.GetGlobalResourceString("lbl_DecarbDMT");
                }
                _decarbString += Constants.StringValues.SPACE + Convert.ToDecimal(decarbVal3).ToString("0.###") + Constants.StringValues.SPACE + UIConstants.GetGlobalResourceString("lbl_LessThan");

            }

            pattern = _decarbString;

            return pattern;
        }
        public static string GetMinMaxValuebyKubun(int kubun)
        {
            Helper helper = new Helper();
            string minMaxValue = string.Empty;

            if (kubun == 1)
            {
                minMaxValue = UIConstants.GetGlobalResourceString("lbl_LessThan");   //lbl_LessThan lbl_Min_M
            }
            else if (kubun == 2)
            {
                minMaxValue = UIConstants.GetGlobalResourceString("lbl_Min_M");
            }

            return minMaxValue;
        }
    }
}
