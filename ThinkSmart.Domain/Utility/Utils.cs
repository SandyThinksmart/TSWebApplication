using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public static class Utils
    {
        public static string GetMagariBendTolSpan(string magariKubun, decimal? magariBendTol, int? magariSpan)
        {
            string magariBendtolSpan = string.Empty;
            if (magariKubun != null)
            {
                if (magariKubun.Length > 0)
                {
                    magariBendtolSpan = magariKubun;
                    
                    


                        if (magariBendTol != null)
                       {
                        magariBendtolSpan += "-" + Convert.ToDecimal(magariBendTol);//.ToString("0.000");
                       }
                    
                    
                   
                    if (magariSpan != null)
                    {
                        magariBendtolSpan += "-" + magariSpan;
                    }
                }
            }
            return magariBendtolSpan;
        }
        public static string GetBendTolerance(string magariKubun, string magariBendTol, int? magariSpan)
        {
            string magariBendtolSpan = string.Empty;
            if (magariKubun != null)
            {
                if (magariKubun.Length > 0)
                {
                    magariBendtolSpan = magariKubun;




                    if (magariBendTol != null)
                    {
                        magariBendtolSpan += "- " + magariBendTol;//.ToString("0.000");
                    }



                    if (magariSpan != null)
                    {
                        magariBendtolSpan += "-" + magariSpan;
                    }
                }
            }
            return magariBendtolSpan;
        }


        public static string GetUnitWeigth(int UNIT_WT_KUBUN,int UNIT_WT, int UNIT_WT_MIN, int? UNIT_WT_MAX)
        {
            string UNIT_WT_KUBUN_STRING = string.Empty;
            if (UNIT_WT_KUBUN != null)
            {
                if (UNIT_WT_KUBUN == 1)
                {
                    UNIT_WT_KUBUN_STRING = Convert.ToString(UNIT_WT);
                }
                if(UNIT_WT_KUBUN == 2)
                {
                    UNIT_WT_KUBUN_STRING = Convert.ToString(UNIT_WT_MIN) + " ～ " + Convert.ToString(UNIT_WT_MAX);
                }
                if (UNIT_WT_KUBUN == 3)
                {
                    UNIT_WT_KUBUN_STRING = Convert.ToString(UNIT_WT) + " 本 ";
                }



            }
            return UNIT_WT_KUBUN_STRING;
        }


        public static string GetColorName(short value)
        {

            string colorName = string.Empty;

            //if (culture.Contains(Constants.Culture.JP) || culture.Contains(Constants.Culture.JPN))
            //{

            //    switch (value)
            //    {

            //        case 1:
            //            colorName = ReadDescription(Enums.ProductColorId.ｱｵ);
            //            break;
            //        case 2:
            //            colorName = ReadDescription(Enums.ProductColorId.ｱｶ);
            //            break;
            //        case 3:
            //            colorName = ReadDescription(Enums.ProductColorId.ｵﾚﾝｼﾞ);
            //            break;
            //        case 4:
            //            colorName = ReadDescription(Enums.ProductColorId.ｷｲﾛ);
            //            break;
            //        case 5:
            //            colorName = ReadDescription(Enums.ProductColorId.ｷﾐﾄﾞﾘ);
            //            break;
            //        case 6:
            //            colorName = ReadDescription(Enums.ProductColorId.ｸﾘｰﾑ);
            //            break;
            //        case 7:
            //            colorName = ReadDescription(Enums.ProductColorId.ｸﾞﾘｰﾝ);
            //            break;
            //        case 8:
            //            colorName = ReadDescription(Enums.ProductColorId.ｸﾞﾚｰ);
            //            break;
            //        case 9:
            //            colorName = ReadDescription(Enums.ProductColorId.ｸﾛ);
            //            break;
            //        case 10:
            //            colorName = ReadDescription(Enums.ProductColorId.ｺﾞｰﾙﾄﾞ);
            //            break;
            //        case 11:
            //            colorName = ReadDescription(Enums.ProductColorId.ｺﾝ);
            //            break;
            //        case 12:
            //            colorName = ReadDescription(Enums.ProductColorId.ｼﾙﾊﾞｰ);
            //            break;
            //        case 13:
            //            colorName = ReadDescription(Enums.ProductColorId.ｼﾛ);
            //            break;
            //        case 14:
            //            colorName = ReadDescription(Enums.ProductColorId.ｽｶｲﾌﾞﾙｰ);
            //            break;
            //        case 15:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾁｬｲﾛ);
            //            break;
            //        case 16:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾈｽﾞﾐ);
            //            break;
            //        case 17:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾊｲｲﾛ);
            //            break;
            //        case 18:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾊﾀﾞｲﾛ);
            //            break;
            //        case 19:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾋﾟﾝｸ);
            //            break;
            //        case 20:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾐｽﾞｲﾛ);
            //            break;
            //        case 21:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾗｲﾄｸﾞﾘｰﾝ);
            //            break;
            //        case 22:
            //            colorName = ReadDescription(Enums.ProductColorId.ﾜｶｻｲﾛ);
            //            break;
            //        default:
            //            colorName = string.Empty;
            //            break;
            //    }
            //}

            //else
            //{
                switch (value)
                {

                    case 1:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Blue);
                        break;
                    case 2:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Red);
                        break;
                    case 3:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Orange);
                        break;
                    case 4:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Yellow);
                        break;
                    case 5:
                        colorName = ReadDescription(Enums.ProductColorIdEng.YellowGreen);
                        break;
                    case 6:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Cream);
                        break;
                    case 7:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Green);
                        break;
                    case 8:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Gray);
                        break;
                    case 9:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Black);
                        break;
                    case 10:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Gold);
                        break;
                    case 11:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Con);
                        break;
                    case 12:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Silver);
                        break;
                    case 13:
                        colorName = ReadDescription(Enums.ProductColorIdEng.White);
                        break;
                    case 14:
                        colorName = ReadDescription(Enums.ProductColorIdEng.SkyBlue);
                        break;
                    case 15:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Brown);
                        break;
                    case 16:
                        colorName = ReadDescription(Enums.ProductColorIdEng.DarkGrey);
                        break;
                    case 17:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Gray);
                        break;
                    case 18:
                        colorName = ReadDescription(Enums.ProductColorIdEng.SkinColor);
                        break;
                    case 19:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Pink);
                        break;
                    case 20:
                        colorName = ReadDescription(Enums.ProductColorIdEng.LightBlue);
                        break;
                    case 21:
                        colorName = ReadDescription(Enums.ProductColorIdEng.LightGreen);
                        break;
                    case 22:
                        colorName = ReadDescription(Enums.ProductColorIdEng.Metallic);
                        break;
                    default:
                        colorName = string.Empty;
                        break;
                }
            //}
            return colorName;

        }

        public static string ReadDescription<T>(T enumMember)
        {
            FieldInfo fi = enumMember.GetType().GetField(enumMember.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : enumMember.ToString();
        }
        public static string GetColorNameForSalesProduction(int value)
        {
            string colorName = string.Empty;
           
            if (value > 0)
            {
                if (value ==1)
                {
                    colorName = ReadDescription(Enums.SalesProductionColorIdEng.Blue);

                }else if (value == 2)
                {
                    colorName = ReadDescription(Enums.SalesProductionColorIdEng.Red);

                }else if (value == 3)
                {
                    colorName = ReadDescription(Enums.SalesProductionColorIdEng.Orange);

                }else if(value == 4)
                {
                    colorName = ReadDescription(Enums.SalesProductionColorIdEng.Darkred);
                }
                else
                {
                    colorName = ReadDescription(Enums.ProductColorIdEng.Green);
                }
            }

            return colorName;
        }
    }
}
