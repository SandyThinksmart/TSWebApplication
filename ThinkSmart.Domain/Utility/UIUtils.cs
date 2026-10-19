using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TSWebApplication.Domain.Utility
{

    public sealed class UIUtils
    {
        public static int K_DEAFULT = 0;
        public static string K_DEAFULT_STRING = String.Empty;
        public static int K_DEAFULT_MINUS_ONE = -1;
        private static readonly Lazy<UIUtils> lazy = new Lazy<UIUtils>(() => new UIUtils());
        public static string GetProductUnitKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ProductUnitKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static string GetUnitIDByName(string id)
        {
            if (IsAllDigits(id) || id == string.Empty)
            {
                return id;
            }
            else
            {
                string kubunValue = string.Empty;
                var kubunName = UIConstants.ProductUnitKubunGet().FirstOrDefault(x => x.Value == id).Key;

                kubunValue = Convert.ToString(kubunName);

                return kubunValue;
            }
        }
        public static string GetProductKubunNameByID(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ProductkubunValue = string.Empty;
                var ProductkubunName = UIConstants.ProductTypeKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;

                ProductkubunValue = Convert.ToString(ProductkubunName);

                return ProductkubunValue;
            }
        }
        public static string GetMagariKubunValueByName(string id)
        {
            if (IsAllDigits(id) || id == string.Empty)
            {
                return id;
            }
            else
            {
                string kubunValue = string.Empty;
               
                var kubunName = UIConstants.MagariKubunGet().FirstOrDefault(x => x.Value == id).Key;
                var aa = UIConstants.MagariKubunGet();


                    kubunValue = Convert.ToString(kubunName);
                return kubunValue;
            }
        }
        public static string GetCoilDiaWindingKubunValueByName(string id)
        {
            if (IsAllDigits(id) || id == string.Empty)
            {
                return id;
            }
            else
            {
                string kubunValue = string.Empty;
                
                var value = UIConstants.CoilDiaWindingKubunGet().FirstOrDefault(x => x.Value == id).Key;
                
                kubunValue = Convert.ToString(value);
                return kubunValue;
            }
        }
        public static string GetStandardHardnessKubunValueByName(string id)
        {
            if (IsAllDigits(id) || id == string.Empty)
            {
                return id;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.StandardHardnessKubunGet().FirstOrDefault(x => x.Value == id).Key;
               
                kubunValue = Convert.ToString(value);
                return kubunValue;
            }
        }
        public string GetECDoneCheckKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ECDoneCheckKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }

        public static UIUtils Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public string GetMagariKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.MagariKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }

        public static string GetColorKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ColorKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetColorNameByID(string id)
        {
            if (IsAllDigits(id) || id == string.Empty)
            {
                return id;
            }
            else
            {
                string kubunValue = string.Empty;

                var colorName =UIConstants.ColorKubunGet().FirstOrDefault(x => x.Value == id).Key;
              
                    kubunValue = Convert.ToString(colorName);
                
                return kubunValue;
            }
        }

        public static string GetCoilDiaWindingKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.CoilDiaWindingKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static bool IsAllDigits(string digitString)
        {
            digitString = digitString == null ? string.Empty : digitString;
            return digitString.All(char.IsDigit);
        }
        public static List<NameValue> GetTPTest(bool isDefaultIncluded = false)
        {
            var returntpTestList = new List<NameValue>();

            Dictionary<int, string> _testList = UIConstants.TPTestKubunGet();

            if (isDefaultIncluded)
                returntpTestList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in _testList)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returntpTestList.Add(nmVal);
            }
            return returntpTestList;
        }

        public static List<NameValue> GetNextProcesKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.NextProcessKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetTPTestKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.TPTestKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static List<NameValue> GetTempTypeKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_tempTypeKubun = UIConstants.HardnessOilTypeKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_tempTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static List<NameValue> GetTextureKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_tempTypeKubun = UIConstants.GetHardnessTextureTypeKubun();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_tempTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }


        public static List<NameValue> GetExteriorConditionKubunGetKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.ExteriorConditionKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static List<NameValue> GetMagneticDust(bool isDefaultIncluded = false)
        {
            var returnMDustList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetColdDrawingMDust();

            if (isDefaultIncluded)
                returnMDustList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnMDustList.Add(nmVal);
            }
            return returnMDustList;
        }
        public static List<NameValue> GetDiceInspStart(bool isDefaultIncluded = false)
        {
            var returnDiceStartList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetDiceInspStartData();

            if (isDefaultIncluded)
                returnDiceStartList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnDiceStartList.Add(nmVal);
            }
            return returnDiceStartList;
        }

        public static List<NameValue> GetDiceInspEnd(bool isDefaultIncluded = false)
        {
            var returnDiceEndList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetDiceInspEndData();

            if (isDefaultIncluded)
                returnDiceEndList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnDiceEndList.Add(nmVal);
            }
            return returnDiceEndList;
        }
        public static List<NameValue> GetwindingShape(bool isDefaultIncluded = false)
        {
            var returnswindinghapeList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetColdDrawingWindingShape();

            if (isDefaultIncluded)
                returnswindinghapeList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnswindinghapeList.Add(nmVal);
            }
            return returnswindinghapeList;
        }
        public static List<NameValue> GetDieShape(bool isDefaultIncluded = false)
        {
            var returnswindinghapeList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetDieShape();

            if (isDefaultIncluded)
                returnswindinghapeList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnswindinghapeList.Add(nmVal);
            }
            return returnswindinghapeList;
        }

        public static string GetDieShapeList(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.GetDieShape().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }
        public static string GetMagneticDustList(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.GetColdDrawingMDust().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }
        public static string GetColdDrawingWindingShape(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string w_shapeValue = string.Empty;

                var value = UIConstants.GetColdDrawingWindingShape().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    w_shapeValue = Convert.ToString(value);
                }
                return w_shapeValue;
            }
        }
        public static string GetExteriorCondition(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.ExteriorConditionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }

        public static List<NameValue> GetStampKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.StampKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static string GetStampbykubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.StampKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }
        public static string GetHardnessOilTypeKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.HardnessOilTypeKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static string GetStandardHardnessKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.StandardHardnessKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
      
        public static string GetardnessMaxMinKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.HardnessMaxMinKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static string GetTrialResultKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.TrialResultKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }
        public static int GetResultKubunKey(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return 0;
            }
            else
            {
                int kubunValue = 0;

                var value = UIConstants.ResultKubunGet().FirstOrDefault(x => x.Value == name).Key;
                if (value > 0)
                {
                    kubunValue = value;
                }
               return kubunValue;
            }
        }

        public static string GetResultKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ResultKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }

        public static string GetNGReasonKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.NGReasonKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetBendStateKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.BendStateKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }

        public static string GetExteriorConditionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ExteriorConditionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }

        public static string GetMaterialUnitKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.MaterialUnitKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetTPTestKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.TPTestKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }

        public static string GetDimDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgDimDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetLengthDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgLengthDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetChamferingDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgChamferingDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetColorDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgColorDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetExtTextureDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgExtTextureDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetCombinedDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgCombinedDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static List<NameValue> GetBendStatus(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.BendStateKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetStandardHardnessKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.StandardHardnessKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static List<NameValue> GetResultKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.ResultKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetTrialResultKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.TrialResultKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetFIResultKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.FIKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static string GetFIResultKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.FIKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }
        public static List<NameValue> GetHrdResultKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> hrdTypeKubun = UIConstants.HrdResultKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in hrdTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetMaterialTypeList(bool isDefaultIncluded = false)
        {
            var returnMaterialTypeList = new List<NameValue>();

            Dictionary<int, string> MaterialTypeKubun = UIConstants.MaterialTypeListGet();

            if (isDefaultIncluded)
                returnMaterialTypeList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in MaterialTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnMaterialTypeList.Add(nmVal);
            }
            return returnMaterialTypeList;
        }
        public static List<NameValue> GetMatlSpecNameList(bool isDefaultIncluded = false)
        {
            var returnMaterialGenSplTypeList = new List<NameValue>();

            Dictionary<int, string> GetMatlSpecNameKubun = UIConstants.GeneralSpecialKubunGet();

            if (isDefaultIncluded)
                returnMaterialGenSplTypeList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in GetMatlSpecNameKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnMaterialGenSplTypeList.Add(nmVal);
            }
            return returnMaterialGenSplTypeList;
        }
        public static List<NameValue> GetUnitTypetKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.UnitTypeKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetChemicalSpecifcationKubun(bool isDefaultIncluded = false)
        {
            var returnchemicalspeciKubunList = new List<NameValue>();

            Dictionary<int, string> d_chemicalspecificationKubun = UIConstants.UnitChemicalSpeciKubunGet();

            if (isDefaultIncluded)
                returnchemicalspeciKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_chemicalspecificationKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnchemicalspeciKubunList.Add(nmVal);
            }
            return returnchemicalspeciKubunList;
        }
        public static List<NameValue> GetUnitTypetKubunForGeneralProcess(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.UnitTypeKubunGetForGeneralProcess();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static string GetUnitTypeName(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.UnitTypeKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }
        public static string GetMaterialTypeName(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.MaterialTypeListGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }
        public static string GetGeneSpecNameForMaterialKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string ExteriorConditionValue = string.Empty;

                var value = UIConstants.GeneralSpecialKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    ExteriorConditionValue = Convert.ToString(value);
                }
                return ExteriorConditionValue;
            }
        }
        public static List<NameValue> GetExternalSurfaceConditionKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.ExteriorConditionKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetGetBendStatusKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.BendStateKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static List<NameValue> GetProcConditionKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> d_ProcConditionKubun = UIConstants.ProcConditionKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in d_ProcConditionKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static List<NameValue> GetMaterialKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> d_materialKubun = UIConstants.MaterialKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in d_materialKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }

            return returnTypeKubunList;
        }

        public static List<NameValue> GetMaterialUnitKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> d_matlUnitKubun = UIConstants.MaterialUnitKubunGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in d_matlUnitKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }

            return returnTypeKubunList;
        }

        public static string GetProcConditionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ProcConditionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public List<NameValue> GetProductUnitKubun(bool isDefaultIncluded = false)
        {

            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> d_prodUnitKubun = UIConstants.ProductUnitKubunGet(); 

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in d_prodUnitKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }

            return returnTypeKubunList;
        }

        public List<NameValue> GetMatlTypeKubun(bool isDefaultIncluded = false)
        {

            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> d_prodUnitKubun = UIConstants.MetlTypeKubunFullNameGet();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in d_prodUnitKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }

            return returnTypeKubunList;
        }
        public static string GetSparkDecisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.OkNgSparkDecisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetDecarbKubunStd(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.DecarbKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static string GetNextProcessName(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.NextProcessKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static int GetProcessKubun(string name)
        {
            
                int kubunValue = 0;
            if (name !=null)
            { 
                var value = UIConstants.NextProcessKubunGet().FirstOrDefault(x => x.Value == name).Key;
                if (value != null)
                {
                    kubunValue = Convert.ToInt32(value);
                }
               
            }
            return kubunValue;
        }
        public static string GetStorageManagementKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.StorageManagementKubun().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static string GetSpecialManagementKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.SpecialManagementKubun().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static string GetProductManagementKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ProductManagementKubun().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
       
        public static List<NameValue> GetStackPositionKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.StackPositionKubun();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static string GetMatlTypeCBK(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.MatlTypeCBK().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public List<NameValue> GetInOut(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> InOutKubun = UIConstants.InOutKubun();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in InOutKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public List<NameValue> GetOrientationKubun(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();
            Dictionary<int, string> orientationKubun = UIConstants.OrientationKubun();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT, Name = K_DEAFULT_STRING });

            foreach (var kb in orientationKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }

        public static string GetProcessPlanKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.ProcessPlanKubun().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetHardnessDecision(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.HardnessDecisionGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetUnitWtKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.UnitWtKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static string GetCornerRadiuscisionKubunValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.CornerRadiuscisionKubunGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }

        public static List<NameValue> GetMaterialReplyForcast(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetMaterialReplyForcast();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static List<NameValue> GetCSVConditions()
        {
            var conditionsList = new List<NameValue>();

           var result = UIConstants.GetCSVSearchConditions();
            Dictionary<int, string> getList = new Dictionary<int, string>();
            foreach (var itemsList in result)
            {
                foreach (var item in itemsList.Value)
                {
                    NameValue nmVal = new NameValue();
                    nmVal.CD = itemsList.Key;
                    nmVal.Name = item;
                    conditionsList.Add(nmVal);
                }

            }
            return conditionsList;
        }
        public static List<NameValue> GetTPTestKubunList(bool isDefaultIncluded = false)
        {
            var returnTypeKubunList = new List<NameValue>();

            Dictionary<int, string> d_nifudaTypeKubun = UIConstants.GetTPTestKubun();

            if (isDefaultIncluded)
                returnTypeKubunList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in d_nifudaTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnTypeKubunList.Add(nmVal);
            }
            return returnTypeKubunList;
        }
        public static string GetTPTestListValue(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.GetTPTestKubun().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }

                return kubunValue;
            }
        }
        public static List<NameValue> GetMillsheetlabels(bool isDefaultIncluded = false)
        {
            var returnLabelsList = new List<NameValue>();

            Dictionary<int, string> label = UIConstants.GetMillsheetPageLables();

            if (isDefaultIncluded)
                returnLabelsList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in label)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnLabelsList.Add(nmVal);
            }
            return returnLabelsList;
        }
        public static List<NameValue> GetMatlList(bool isDefaultIncluded = false)
        {
            var returnMaterialTypeList = new List<NameValue>();

            Dictionary<int, string> MaterialTypeKubun = UIConstants.MatlListGet();

            if (isDefaultIncluded)
                returnMaterialTypeList.Add(new NameValue { CD = K_DEAFULT_MINUS_ONE, Name = K_DEAFULT_STRING });

            foreach (var kb in MaterialTypeKubun)
            {
                NameValue nmVal = new NameValue();
                nmVal.CD = kb.Key;
                nmVal.Name = kb.Value;
                returnMaterialTypeList.Add(nmVal);
            }
            return returnMaterialTypeList;
        }

        public static string GetMatlTypeName(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string typeValue = string.Empty;

                var value = UIConstants.MatlListGet().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    typeValue = Convert.ToString(value);
                }
                return typeValue;
            }
        }
        public static string GetHardnessMaxMinKubun(string key)
        {
            if (!IsAllDigits(key) || key == string.Empty)
            {
                return key;
            }
            else
            {
                string kubunValue = string.Empty;

                var value = UIConstants.HardnessMaxMinKubun().FirstOrDefault(x => x.Key == Convert.ToInt32(key)).Value;
                if (value != null)
                {
                    kubunValue = Convert.ToString(value);
                }
                return kubunValue;
            }
        }
        public static int GetResultDecisionFI(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return 0;
            }
            else
            {
                int kubunValue = 0;

                var value = UIConstants.ResultDecisionFIGet().FirstOrDefault(x => x.Value == name).Key;
                if (value > 0)
                {
                    kubunValue = value;
                }
                return kubunValue;
            }
        }
        public static int GetMillsheetLabelCD(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return 0;
            }
            else
            {
                int fieldValue = 0;

                var value = UIConstants.GetMillsheetPageLables().FirstOrDefault(x => x.Value == fieldName).Key;
                if (value > 0)
                {
                    fieldValue = value;
                }
                return fieldValue;
            }
        }
    }
}
