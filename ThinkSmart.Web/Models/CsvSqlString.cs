using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using OpenCvSharp.Flann;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Text;
using System.Text.RegularExpressions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static NPOI.HSSF.Util.HSSFColor;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Web.Models
{
    public class CsvSqlString
    {
        private readonly LocalizationServices _localization;
        private readonly string _specialChuubanForSQL;
        public CsvSqlString(LocalizationServices localization, IConfiguration _configuration)
        {
            _localization = localization;
            _specialChuubanForSQL = _configuration.GetSection("SpecialChuubanForSQL").Value;
        }
        /// <summary>
        /// CREATED BY :       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// SQL of Heat Treatment process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHeatTreatmentCSVSQLForFactory(List<SearchField> searchCriteria, string sortCiteria ,string cultureNames)
        {
            

            StringBuilder selectQuery = new StringBuilder();
            List<HeatTreatMentDetails> result = new List<HeatTreatMentDetails>();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" LOCA.SHORT_NAME AS PROC_LOCATION_NAME,");
            selectQuery.Append(" MAC.MACHINE_NAME AS MACHINE_NAME,");
            selectQuery.Append(" TO_CHAR(HT.HT_END_DATE, 'YYYY/MM/DD HH24:MI') AS HT_END_DATE_STRING,");
            selectQuery.Append(" EMP.SHORT_NAME AS OFFLOAD_CONF_BY_EMPLOYEE_NAME,");
            selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST,");
            selectQuery.Append(" PP.CHARGE,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,'','' ) AS MATERIAL_DIA,");
            selectQuery.Append(" PP.MATL_SHAPE AS MATERIAL_SHAPE,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'','' ) AS PRODUCT_DIA,");
            selectQuery.Append(" PP.PROD_SHAPE AS PRODUCT_SHAPE,");
            selectQuery.Append(" HT.LOT_NO,");
            //selectQuery.Append(" HT.LOT_NO IS NOT NULL THEN ''''||HT.LOT_NO||'''' END AS LOT_NO,");
            selectQuery.Append(" HT.HT_NO_OF_UNITS, " );
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");

            selectQuery.Append(" HT.HT_WEIGHT,");
            selectQuery.Append(" HT.ROLL_MONTH,");
            selectQuery.Append(" HT.COIL_NO,");
            selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO,");
            selectQuery.Append(" HT.HT_CONDITION,");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE1 ||' , '|| HT.LOCATION_IN_FURNACE2 AS LOCATION_IN_FURNACE1,");
          
            selectQuery.Append(" HT.PATTERN_NO,");
            selectQuery.Append(" MESS.MSG_CONTENTS AS PP_PROCESS_PLAN_REMARK, ");
            if (cultureNames == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN,");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            }
            selectQuery.Append(" HT.HT_REMARK AS HT_REMARKS");

            
            selectQuery.Append(" FROM TRN_HEAT_TREATMENT HT INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.MACHINE_CD ");
           
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = HT.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE CONFSTARTEMP ON CONFSTARTEMP.EMPLOYEE_CD = HT.LOADING_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE ENDEMP ON ENDEMP.EMPLOYEE_CD = HT.OFFLOADED_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE CONFENDEMP ON CONFENDEMP.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = HT.HT_NEXT_PROCESS");
            string whereCondition = "";
            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {


                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/05/10       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// SQL of Drawing Bar To Bar process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHikinukiBarToBarSQLForFactory(List<SearchField> searchFields, string sortCriteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append("SELECT  ");
            selectQuery.Append("LOCA.SHORT_NAME AS DW_LOCATION,");
            selectQuery.Append("MACH.MACHINE_NAME AS DW_MACHINE_NAME,");
            selectQuery.Append(" TO_CHAR(DRAWINGD.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DRAWINGD.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append("EMP.SHORT_NAME AS DW_END_OPERATOR_NAME,");
            selectQuery.Append("CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append("GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append("PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            selectQuery.Append("PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append("PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append("PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ', '|| PP.DIM_TOL_MIN_CHR AS DIM_TOL_CHR,");
            selectQuery.Append("PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append("DRAWINGD.HT_CONDITION AS HT_CONDITION,  ");
            selectQuery.Append("PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append("DRAWINGD. DW_METALLIC_PLATE_CODE,");
            selectQuery.Append(" PP.NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_NO_OF_UNITS, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

            selectQuery.Append("DRAWINGD.DIE_DIM ||' '|| DRAWINGD.DIE_SHAPE,");
            selectQuery.Append("DRAWINGD.DIE_NO,");
            //selectQuery.Append("DRAWINGD.DW_ACT_DIM1|| ' '|| DRAWINGD.DW_ACT_DIM2|| ' '|| DRAWINGD.DW_ACT_DIM3 AS DW_ACT_DIM,");
            selectQuery.Append("CONCAT(TO_CHAR(DRAWINGD.DW_ACT_DIM1, 'FM990.000'), CONCAT( ");
            selectQuery.Append("CASE ");
            selectQuery.Append("WHEN DRAWINGD.DW_ACT_DIM2 IS NOT NULL THEN ");
            selectQuery.Append("CONCAT('|　', TO_CHAR(DRAWINGD.DW_ACT_DIM2, 'FM990.000')) ");
            selectQuery.Append("ELSE NULL END, ");
            selectQuery.Append("CASE ");
            selectQuery.Append("WHEN DRAWINGD.DW_ACT_DIM3 IS NOT NULL THEN ");
            selectQuery.Append("CONCAT('|　', TO_CHAR(DRAWINGD.DW_ACT_DIM3, 'FM990.000')) ");
            selectQuery.Append("ELSE NULL END ");
            selectQuery.Append(")) AS ALL_ACT_DIM, ");
            selectQuery.Append("DRAWINGD.DW_FINISHED_BARS AS FINISHED_BARS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DRAWINGD.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN DRAWINGD.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN DRAWINGD.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN DRAWINGD.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");

            //selectQuery.Append("DRAWINGD.UNIT_TYPE,");
            selectQuery.Append("DRAWINGD.DW_FINISHED_QTY AS DW_FINISHED_QTY,");
            //selectQuery.Append("DRAWINGD.DW_EXT_TEXTURE_STATUS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_EXT_TEXTURE_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_EXT_TEXTURE_STATUS,");

            //selectQuery.Append("DRAWINGD.DW_BEND_STATUS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_BEND_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_BEND_STATUS, ");
            selectQuery.Append(" DRAWINGD.DW_TABA_NO_ AS DW_TABA_NO_,");
            if(cultureName == "en")
            {
                selectQuery.Append("CASE WHEN MESS.MSG_CONTENTS IS NOT NULL THEN  ");

                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( MESS.MSG_CONTENTS || ' ' || CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL OR DRAWINGD.DW_NG_BARS2 IS NOT NULL OR DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || DRAWINGD.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN DRAWINGD.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   DRAWINGD.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   DRAWINGD.DW_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL OR DRAWINGD.DW_NG_BARS2 IS NOT NULL OR DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || DRAWINGD.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN DRAWINGD.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   DRAWINGD.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   DRAWINGD.DW_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append("  END AS DW_REMARKS, ");
            }
            else
            {
                selectQuery.Append("CASE WHEN MESS.MSG_CONTENTS IS NOT NULL THEN  ");

                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( MESS.MSG_CONTENTS || ' ' || CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL OR DRAWINGD.DW_NG_BARS2 IS NOT NULL OR DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || DRAWINGD.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN DRAWINGD.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   DRAWINGD.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   DRAWINGD.DW_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL OR DRAWINGD.DW_NG_BARS2 IS NOT NULL OR DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DRAWINGD.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || DRAWINGD.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN DRAWINGD.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   DRAWINGD.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DRAWINGD.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   DRAWINGD.DW_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append("  END AS DW_REMARKS, ");
            }
                 

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN DRAWINGD.DW_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP ");
            }

            selectQuery.Append(" FROM TRN_DRAWING_D DRAWINGD");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DRAWINGD.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = DRAWINGD.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = DRAWINGD.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD =DRAWINGD.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DRAWINGD.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DRAWINGD.DW_NG_DETAILS1=DEFECT1.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DRAWINGD.DW_NG_DETAILS2=DEFECT2.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DRAWINGD.DW_NG_DETAILS3=DEFECT3.PROCESS_DEFECT_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DRAWINGD.DW_NEXT_PROCESS");

            var query = selectQuery.ToString();
            if (searchFields != null && searchFields.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }
                query = query + whereCondition;
                //    query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/05/10       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// SQL of Hikinuki Coil to Bar (Combined Daily Report) process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetCombinedDailyReportSQLForFactory(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<HikinukiDetails> result = new List<HikinukiDetails>();

            selectQuery.Append(" SELECT ");

            selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(DW.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.MATL_DIM_D PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");
            //selectQuery.Append(" PP.UNIT AS UNIT_TYPE,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" DW.DIE_DIM AS DW_DIE_DIM, ");

            selectQuery.Append(" RTRIM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_1 IS NOT NULL THEN DW_TABA_NO_1 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_2 IS NOT NULL THEN DW_TABA_NO_2 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END || ");
            selectQuery.Append("  CASE ");
            selectQuery.Append("  WHEN DW_TABA_NO_3 IS NOT NULL THEN DW_TABA_NO_3 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append("  END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append("  WHEN DW_TABA_NO_4 IS NOT NULL THEN DW_TABA_NO_4 ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append("  END, ',' ");
            selectQuery.Append("  ) AS dw_taba_no, ");


            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM1, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM2, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM3, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM4, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM5, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM6, 'FM990.000'), ");

            selectQuery.Append(" DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3, ");

            //selectQuery.Append(" M.MSG_CONTENTS AS DW_REMARKS, ");
            if(cultureName == "en")
            {
                selectQuery.Append(" CASE WHEN M.MSG_CONTENTS IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(M.MSG_CONTENTS || ' ' || CASE WHEN DW.DW_NG_BARS1 IS NOT NULL OR DW.DW_NG_BARS2 IS NOT NULL OR DW.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DW.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || DW.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || DW.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || DW.DW_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END) ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(CASE WHEN DW.DW_NG_BARS1 IS NOT NULL OR DW.DW_NG_BARS2 IS NOT NULL OR DW.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DW.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || DW.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || DW.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || DW.DW_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END) ");
                selectQuery.Append(" END AS DW_REMARKS, ");
            }
            else
            {
                selectQuery.Append(" CASE WHEN M.MSG_CONTENTS IS NOT NULL THEN ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(M.MSG_CONTENTS || ' ' || CASE WHEN DW.DW_NG_BARS1 IS NOT NULL OR DW.DW_NG_BARS2 IS NOT NULL OR DW.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DW.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || DW.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || DW.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || DW.DW_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END) ");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(CASE WHEN DW.DW_NG_BARS1 IS NOT NULL OR DW.DW_NG_BARS2 IS NOT NULL OR DW.DW_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN DW.DW_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || DW.DW_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || DW.DW_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN DW.DW_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || DW.DW_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END) ");
                selectQuery.Append(" END AS DW_REMARKS, ");
            }

                //selectQuery.Append(" DW.DW_FINISHED_BARS AS DW_FINISHED_BARS, ");
                // selectQuery.Append(" DW.UNIT_TYPE AS DW_UNIT_TYPE, ");
                selectQuery.Append(" DW.DW_FINISHED_BARS AS DW_FINISHED_BARS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN  DW.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN  DW.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN  DW.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN  DW.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_UNIT_TYPE, ");
            selectQuery.Append(" DW.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");
            selectQuery.Append(" DW.MAGNETISM AS DW_MAGNETISM, ");

            //selectQuery.Append(" DW.DW_BEND_STATUS, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_BEND_STATUS, ");


            // selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_EXT_TEXTURE_STATUS,");

            selectQuery.Append(" DW.SHIKII_VALUE AS DW_SHIKII_VALUE, ");
            selectQuery.Append(" DW.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL, ");

            //selectQuery.Append(" DW.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP ");
            }


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DW.DW_NG_DETAILS1 = DEFECT1.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DW.DW_NG_DETAILS2 = DEFECT2.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DW.DW_NG_DETAILS3 = DEFECT3.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DW.DW_NEXT_PROCESS");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/05/12       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// SQL of Cold Drawing Wire Drawing(Coil To Coil) process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetColdDrawingWireDrawingSQLForFactory(List<SearchField> searchCriteria, string sortCriteria, string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<ColdDrawingWireDrawingDetails> result = new List<ColdDrawingWireDrawingDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(DW.DW_START_DATE, 'YYYY/MM/DD HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_END_DATE, 'YYYY/MM/DD HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PROD_SPEC_NO, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" PP.PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_SHAPE,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ', ' || PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN_CHR,");

            selectQuery.Append(" PP.MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_SHAPE, ");

            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" DW.ROLLING_NUMBER AS ROLLING_NUMBER, ");


            //selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            //selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_NO_OF_UNITS, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

            selectQuery.Append(" DW.DIE_DIM AS DW_DIE_DIM, ");
            selectQuery.Append(" DW.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" DW.DIE_NO AS DW_DIE_NO,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_RightCoil") + "'");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_LeftCoil") + "'");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_RightCarrier") + "'");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_LeftCarrier") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_WINDING_SHAPE, ");


            //selectQuery.Append(" DW.DW_ACT_DIM_START || ' ' || DW.DW_ACT_DIM_END,");
            selectQuery.Append(" CONCAT(TO_CHAR(DW.DW_ACT_DIM_START, 'FM990.000'), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_ACT_DIM_END IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(DW.DW_ACT_DIM_END, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append("  ");
            selectQuery.Append(" ) AS ALL_ACT_DIM, ");
            // selectQuery.Append(" DW.DW_EXT_TEXTURE_STATUS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_EXT_TEXTURE_STATUS,");

            selectQuery.Append(" DW.DW_FINISHED_QTY AS DW_FINISHED_QTY,");

            selectQuery.Append(" DW.DW_MAGNETIC_INSPECTIONS AS DW_MAGNETIC_INSPECTIONS, ");

            selectQuery.Append(" RTRIM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_1 IS NOT NULL THEN DW_TABA_NO_1 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW_TABA_NO_2 IS NOT NULL THEN DW_TABA_NO_2 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END || ");
            selectQuery.Append("  CASE ");
            selectQuery.Append("  WHEN DW_TABA_NO_3 IS NOT NULL THEN DW_TABA_NO_3 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append("  END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append("  WHEN DW_TABA_NO_4 IS NOT NULL THEN DW_TABA_NO_4 ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append("  END, ',' ");
            selectQuery.Append("  ) AS dw_taba_no, ");

            selectQuery.Append(" M.MSG_CONTENTS AS DW_REMARKS, ");
            //selectQuery.Append(" DW.DW_DECISION AS DW_DECISION, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP ");
            }

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING_W DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = DW.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = DW.DW_NEXT_PROCESS");
            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

		/// <summary>
		/// CREATED BY : Pallavi Gawade on 2024/05/16       
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Straighteing process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetStraighteningSQLForFactory(List<SearchField> searchFields, string sortCriteria,string cultureName)
		{
			StringBuilder selectQuery = new StringBuilder();

			selectQuery.Append(" SELECT  ");
			selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
			selectQuery.Append(" MAC.MACHINE_NAME AS STR_MACHINE_NAME ,");
            selectQuery.Append(" TO_CHAR(STRAI.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(STRAI.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING,");
			selectQuery.Append(" EMP.SHORT_NAME AS STR_END_OPERATOR_NAME,");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
			selectQuery.Append(" STRAI.STR_METALLIC_PLATE_CODE,");
			selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
			selectQuery.Append(" PP.PROD_SHAPE AS PROD_SHAPE,");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");			
			selectQuery.Append(" CONCAT(DIM_TOL_MAX_CHR, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN DIM_TOL_MIN_CHR IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', DIM_TOL_MIN_CHR) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(") AS DIM_TOL_MIN_MAX_CHR, ");
			selectQuery.Append(" CONCAT(LENGTH_TOL_MAX_CHR, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN LENGTH_TOL_MIN_CHR IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', LENGTH_TOL_MIN_CHR) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(") AS LENGTH_TOL_MIN_MAX_CHR, ");
            
            selectQuery.Append(" PP.NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

            selectQuery.Append(" RTRIM( ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN STR_TABA_NO_1 IS NOT NULL THEN STR_TABA_NO_1 || ',' ");
			selectQuery.Append(" ELSE NULL ");
			selectQuery.Append(" END || ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN STR_TABA_NO_2 IS NOT NULL THEN STR_TABA_NO_2 || ',' ");
			selectQuery.Append(" ELSE NULL ");
			selectQuery.Append(" END || ");
			selectQuery.Append("  CASE ");
			selectQuery.Append("  WHEN STR_TABA_NO_3 IS NOT NULL THEN STR_TABA_NO_3 || ',' ");
			selectQuery.Append(" ELSE NULL ");
			selectQuery.Append("  END || ");
			selectQuery.Append(" CASE ");
			selectQuery.Append("  WHEN STR_TABA_NO_4 IS NOT NULL THEN STR_TABA_NO_4 ");
			selectQuery.Append(" ELSE NULL ");
			selectQuery.Append("  END, ',' ");
			selectQuery.Append("  ) AS ALL_TABA_NO, ");

            selectQuery.Append(" CONCAT(TO_CHAR(STR_DIM_BEFORE_1, 'FM990.000'), CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_BEFORE_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_BEFORE_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_BEFORE, ");
           
            selectQuery.Append(" CONCAT(TO_CHAR(STR_DIM_AFTER_1, 'FM990.000'), CONCAT(CONCAT(CONCAT(CONCAT( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_2, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_3, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_4, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_5 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_5, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ), ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_DIM_AFTER_6 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT('|　', TO_CHAR(STR_DIM_AFTER_6, 'FM990.000')) ");
            selectQuery.Append(" ELSE NULL END ");
            selectQuery.Append(" )) AS ALL_DIM_AFTER, ");

            selectQuery.Append(" STRAI.STR_LENGTH_AFTER_1,");
            selectQuery.Append(" STRAI.STR_LENGTH_AFTER_2,");
            selectQuery.Append(" STRAI.STR_BARS AS STR_BARS,");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN STRAI.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN STRAI.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN STRAI.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN STRAI.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS STR_UNIT_TYPE, ");

			selectQuery.Append(" STRAI.STR_QTY AS TOTAL_STR_QTY, ");
            if(cultureName == "en")            
            {
                selectQuery.Append("CASE WHEN MESS.MSG_CONTENTS IS NOT NULL THEN  ");

                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( MESS.MSG_CONTENTS || ' ' || CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL OR STRAI.STR_NG_BARS2 IS NOT NULL OR STRAI.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STRAI.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STRAI.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STRAI.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STRAI.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STRAI.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL OR STRAI.STR_NG_BARS2 IS NOT NULL OR STRAI.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STRAI.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STRAI.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STRAI.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STRAI.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STRAI.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append("  END AS STR_REMARKS, ");
            }
            else
            {
                selectQuery.Append("CASE WHEN MESS.MSG_CONTENTS IS NOT NULL THEN  ");

                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( MESS.MSG_CONTENTS || ' ' || CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL OR STRAI.STR_NG_BARS2 IS NOT NULL OR STRAI.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STRAI.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STRAI.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STRAI.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STRAI.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STRAI.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL OR STRAI.STR_NG_BARS2 IS NOT NULL OR STRAI.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STRAI.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STRAI.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STRAI.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STRAI.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STRAI.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STRAI.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append("  END AS STR_REMARKS, ");
            }

                selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STRAI.STR_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN STRAI.STR_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN STRAI.STR_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN STRAI.STR_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN STRAI.STR_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS STR_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            }
            else
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP ");
            }



            selectQuery.Append(" FROM TRN_STRAIGHTENING STRAI ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STRAI.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STRAI.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = STRAI.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = STRAI.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=STRAI.STR_END_OPERATOR_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = STRAI.STR_NEXT_PROCESS ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = STRAI.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = STRAI.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = STRAI.STR_NG_DETAILS3 ");
            var query = selectQuery.ToString();
			if (searchFields != null && searchFields.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchFields)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}
				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCriteria))
			{
				query = query + " ORDER BY " + sortCriteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade   
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Straighteing2 process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetStraighteing2SQLForFactory(List<SearchField> searchFields, string sortCriteria,string cultureName)
		{
			StringBuilder selectQuery = new StringBuilder();

			selectQuery.Append(" SELECT  ");
			selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
			selectQuery.Append(" MACH.MACHINE_NAME AS STR_MACHINE_NAME,");
            selectQuery.Append(" TO_CHAR(STR2.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING, ");
			selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
			selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
			selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
			selectQuery.Append(" CONCAT(PP.DIM_TOL_MAX_CHR, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN PP.DIM_TOL_MIN_CHR IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', PP.DIM_TOL_MIN_CHR) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(") AS DIM_TOL_MIN_MAX_CHR, ");

			selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");

			selectQuery.Append(" STR2.STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" PP.NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_NO_OF_UNITS, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");


            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_BEFORE_1, 'FM990.000') AS STR_DIM_BEFORE_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_BEFORE_2, 'FM990.000') AS STR_DIM_BEFORE_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_X_1, 'FM990.000') AS STR_DIM_AFTER_X_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_X_2, 'FM990.000') AS STR_DIM_AFTER_X_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_X_3, 'FM990.000') AS STR_DIM_AFTER_X_3_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_Y_1, 'FM990.000') AS STR_DIM_AFTER_Y_1_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_Y_2, 'FM990.000') AS STR_DIM_AFTER_Y_2_STRING, ");
            selectQuery.Append(" TO_CHAR(STR2.STR_DIM_AFTER_Y_3, 'FM990.000') AS STR_DIM_AFTER_Y_3_STRING, ");
            selectQuery.Append(" STR2.STR_BARS AS STR_BARS,");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN STR2.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN STR2.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN STR2.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN STR2.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS UNIT_TYPE, ");
			selectQuery.Append(" STR2.STR_QTY,");

			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 15 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 25 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 35 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 45 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 50 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 55 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
			selectQuery.Append(" WHEN  STR2.STR_EXT_TEXTURE = 99 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS STR_EXT_TEXTURE,");

			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 15 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 25 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 35 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
			selectQuery.Append(" WHEN  STR2.STR_BEND_STATUS = 99 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS STR_BEND_STATUS, ");

            selectQuery.Append(" RTRIM( ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_1 IS NOT NULL THEN STR_TABA_NO_1 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR_TABA_NO_2 IS NOT NULL THEN STR_TABA_NO_2 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append(" END || ");
            selectQuery.Append("  CASE ");
            selectQuery.Append("  WHEN STR_TABA_NO_3 IS NOT NULL THEN STR_TABA_NO_3 || ',' ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append("  END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append("  WHEN STR_TABA_NO_4 IS NOT NULL THEN STR_TABA_NO_4 ");
            selectQuery.Append(" ELSE NULL ");
            selectQuery.Append("  END, ',' ");
            selectQuery.Append("  ) AS ALL_TABA_NO, ");

            if(cultureName == "en")
            {
                selectQuery.Append("CASE WHEN MESS.MSG_CONTENTS IS NOT NULL THEN  ");

                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( MESS.MSG_CONTENTS || ' ' || CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL OR STR2.STR_NG_BARS2 IS NOT NULL OR STR2.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STR2.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STR2.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STR2.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STR2.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STR2.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL OR STR2.STR_NG_BARS2 IS NOT NULL OR STR2.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STR2.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STR2.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STR2.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STR2.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STR2.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append("  END AS STR_REMARKS, ");
            }
            else
            {
                selectQuery.Append("CASE WHEN MESS.MSG_CONTENTS IS NOT NULL THEN  ");

                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( MESS.MSG_CONTENTS || ' ' || CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL OR STR2.STR_NG_BARS2 IS NOT NULL OR STR2.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STR2.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STR2.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STR2.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STR2.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STR2.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append(" ELSE ");
                selectQuery.Append(" CONCAT(  CONCAT( CONCAT( CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL OR STR2.STR_NG_BARS2 IS NOT NULL OR STR2.STR_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN STR2.STR_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "'  || STR2.STR_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END),  CASE WHEN STR2.STR_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' ||   STR2.STR_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN STR2.STR_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' ||   STR2.STR_NG_BARS3  || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)");

                selectQuery.Append("  END AS STR_REMARKS, ");
            }


                selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR2.STR_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN STR2.STR_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN STR2.STR_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN STR2.STR_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN STR2.STR_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS STR_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP ");
            }

            selectQuery.Append(" FROM TRN_STRAIGHTENING2 STR2");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR2.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR2.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = STR2.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD =STR2.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = STR2.STR_END_OPERATOR_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = STR2.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = STR2.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = STR2.STR_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = STR2.STR_NEXT_PROCESS");
            var query = selectQuery.ToString();
			if (searchFields != null && searchFields.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchFields)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}
				query = query + whereCondition;

			}
			if (!string.IsNullOrEmpty(sortCriteria))
			{
				query = query + " ORDER BY " + sortCriteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();

		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Chamffer process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetChamfferSQLForFactory(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<ChamferDetails> result = new List<ChamferDetails>();
			selectQuery.Append(" SELECT ");
			selectQuery.Append(" LOC.SHORT_NAME AS PROCESS_LOCATION, ");
			selectQuery.Append(" MAC.MACHINE_NAME AS CH_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(CHA.CH_START_DATE, 'yyyy/mm/dd HH24:MI') AS CH_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(CHA.CH_END_DATE, 'yyyy/mm/dd HH24:MI') AS CH_END_DATE_STRING, ");
			selectQuery.Append(" EMP.SHORT_NAME AS CH_END_OPERATOR_NAME, ");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
			selectQuery.Append(" CHA.CH_METALLIC_PLATE_CODE  AS CH_METALLIC_PLATE_CODE, ");
			selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
			selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" CONCAT('C-',CHA.CH_SHAPE) AS CH_SHAPE_STRING,");

            selectQuery.Append(" PP.NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

            selectQuery.Append(" CONCAT(CH_TABA_NO_1, CONCAT(CONCAT( ");
			selectQuery.Append(" CASE  WHEN CH_TABA_NO_2 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', CH_TABA_NO_2) ");
			selectQuery.Append(" ELSE   NULL ");
			selectQuery.Append(" END, CASE ");
			selectQuery.Append(" WHEN CH_TABA_NO_3 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', CH_TABA_NO_3) ");
			selectQuery.Append(" ELSE NULL  END   ),");
			selectQuery.Append(" CASE  WHEN CH_TABA_NO_4 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', CH_TABA_NO_4) ");
			selectQuery.Append(" ELSE   NULL  END ");
			selectQuery.Append(" )) AS ALL_TABA_NO, ");
			selectQuery.Append(" CHA.CH_FINISHED_BARS AS CH_FINISHED_BARS,");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN CHA.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN CHA.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN CHA.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN CHA.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS UNIT_TYPE, ");
			selectQuery.Append(" CHA.CH_FINISHED_QTY AS CH_FINISHED_QTY, ");
			selectQuery.Append(" MSG.MSG_CONTENTS AS CH_REMARKS, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN CHA.CH_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN CHA.CH_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN CHA.CH_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN CHA.CH_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN CHA.CH_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS CH_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            }
            else
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP ");
            }

            selectQuery.Append(" FROM ");
			selectQuery.Append(" TRN_CHAMFERING CHA ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON CHA.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = CHA.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = CHA.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD =CHA.CH_END_OPERATOR_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON CHA.MSG_CD = MSG.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = CHA.CH_NEXT_PROCESS ");

            var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCiteria))
			{
				query = query + " ORDER BY " + sortCiteria;
			}

			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of EC process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetECSQLForFactory(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
		{
			StringBuilder selectQuery = new StringBuilder();
			selectQuery.Append(" SELECT  ");
			selectQuery.Append(" LOCA.SHORT_NAME AS PP_PROCESS_LOCATION,");
			selectQuery.Append(" MAC.MACHINE_NAME AS EC_MACHINE_NAME ,");
            selectQuery.Append(" TO_CHAR(EC.EC_START_DATE, 'yyyy/mm/dd HH24:MI') AS EC_START_DATE_STRING ,");
            selectQuery.Append(" TO_CHAR(EC.EC_END_DATE, 'yyyy/mm/dd HH24:MI') AS EC_END_DATE_STRING ,");
			selectQuery.Append(" EMP.SHORT_NAME AS EC_END_OPERATOR_NAME,");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
			selectQuery.Append(" EC.EC_METALLIC_PLATE_CODE ,");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
			selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
			selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
			selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN PP.UNIT = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN PP.UNIT = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN PP.UNIT = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN PP.UNIT = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS UNIT_TYPE, ");
			selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

			selectQuery.Append(" EC.EC_TABA_NO AS EC_TABA_NO, ");
			selectQuery.Append(" EC.EC_OK_BARS AS EC_OK_BARS, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN EC.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN EC.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN EC.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN EC.UNIT_TYPE= 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS EC_OK_UNIT_TYPE, ");

			selectQuery.Append(" EC.EC_OK_QTY,  ");
            selectQuery.Append(" EC.EC_NG_BARS1, ");

            if(cultureName == "en")
            {
                selectQuery.Append(" DEF1.OTHER_NAME_SHORT AS EC_NG_DETAILS1_EN, ");
            }
            else
            {
                selectQuery.Append(" DEF1.PROC_DEFECT_SHORT AS EC_NG_DETAILS1_JP,");
            }
            selectQuery.Append(" EC.EC_NG_BARS2, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEF2.OTHER_NAME_SHORT AS EC_NG_DETAILS2_EN, ");
            }
            else
            {
                selectQuery.Append(" DEF2.PROC_DEFECT_SHORT AS EC_NG_DETAILS2_JP,");
            }
            selectQuery.Append(" EC.EC_NG_BARS3, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEF3.OTHER_NAME_SHORT AS EC_NG_DETAILS3_EN, ");
            }
            else
            {
                selectQuery.Append(" DEF3.PROC_DEFECT_SHORT AS EC_NG_DETAILS3_JP,");
            }

            //selectQuery.Append(" EC.EC_NG_BARS1 || ' ' || EC.EC_NG_BARS2 || ' ' || EC.EC_NG_BARS3 ,");
            //         //selectQuery.Append(" DEF1.PROC_DEFECT_SHORT || ' ' || DEF2.PROC_DEFECT_SHORT || ' ' ||  DEF3.PROC_DEFECT_SHORT, ");
            //         if (cultureName == "en")
            //         {
            //             selectQuery.Append(" DEF1.OTHER_NAME_SHORT || ' ' || DEF2.OTHER_NAME_SHORT || ' ' || DEF3.OTHER_NAME_SHORT AS EC_NG_DETAILS_EN, ");
            //         }
            //         else
            //         {
            //             selectQuery.Append(" DEF1.PROC_DEFECT_SHORT || ' ' || DEF2.PROC_DEFECT_SHORT || ' ' || DEF3.PROC_DEFECT_SHORT AS EC_NG_DETAILS_JP, ");
            //         }


            selectQuery.Append(" EC.IS_OK,");
			selectQuery.Append(" EC.LINE_SPEED AS LINE_SPEED, ");
			selectQuery.Append(" TO_NUMBER(EC.MAGNETISM)  AS EC_MAGNETISM,  ");

			selectQuery.Append(" MESS.MSG_CONTENTS AS EC_REMARKS,");

			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN EC.EC_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN EC.EC_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS EC_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP ");
            }


            selectQuery.Append(" FROM TRN_EC EC");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = EC.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = EC.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = EC.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = EC.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = EC.EC_END_OPERATOR_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
			selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF1 ON DEF1.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1");
			selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF2 ON DEF2.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2");
			selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEF3 ON DEF3.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = EC.EC_NEXT_PROCESS");


            var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCiteria))
			{
				query = query + " ORDER BY " + sortCiteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Stammping process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetStamppingSQLForFactory(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
		{
			StringBuilder selectQuery = new StringBuilder();

			selectQuery.Append(" SELECT ");
			selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
			selectQuery.Append(" MAC.MACHINE_NAME AS ST_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(ST.ST_START_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(ST.ST_END_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_END_DATE_STRING, ");
			selectQuery.Append(" EMP.SHORT_NAME AS ST_END_OPERATOR_NAME, ");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
			selectQuery.Append(" ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE, ");
			selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
			selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" ST.ST_TABA_NO AS ST_TABA_NO, ");
			selectQuery.Append(" ST.ST_OK_BARS AS ST_OK_BARS, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN  ST.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN  ST.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN  ST.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN  ST.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS ST_OK_UNIT_TYPE, ");

			selectQuery.Append(" ST.ST_OK_QTY AS ST_OK_QTY, ");
			selectQuery.Append(" ST.STAMP_DETAILS AS STAMP_DETAILS, ");


            //selectQuery.Append(" M.MSG_CONTENTS AS ST_REMARKS, ");
            if(cultureName == "en")
            {
                selectQuery.Append(" CASE WHEN M.MSG_CONTENTS IS NOT NULL THEN  ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(M.MSG_CONTENTS || ' ' || CASE WHEN ST.ST_NG_BARS1 IS NOT NULL OR ST.ST_NG_BARS2 IS NOT NULL OR ST.ST_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END,  ");
                selectQuery.Append(" CASE WHEN ST.ST_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || ST.ST_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || ST.ST_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || ST.ST_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)  ");
                selectQuery.Append(" ELSE  ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(CASE WHEN ST.ST_NG_BARS1 IS NOT NULL OR ST.ST_NG_BARS2 IS NOT NULL OR ST.ST_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN ST.ST_NG_BARS1 IS NOT NULL THEN  DEFECT1.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || ST.ST_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS2 IS NOT NULL THEN DEFECT2.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || ST.ST_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS3 IS NOT NULL THEN  DEFECT3.OTHER_NAME_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || ST.ST_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)  ");
                selectQuery.Append(" END AS ST_REMARKS, ");
            }
            else
            {
                selectQuery.Append(" CASE WHEN M.MSG_CONTENTS IS NOT NULL THEN  ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(M.MSG_CONTENTS || ' ' || CASE WHEN ST.ST_NG_BARS1 IS NOT NULL OR ST.ST_NG_BARS2 IS NOT NULL OR ST.ST_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END,  ");
                selectQuery.Append(" CASE WHEN ST.ST_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || ST.ST_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || ST.ST_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || ST.ST_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)  ");
                selectQuery.Append(" ELSE  ");
                selectQuery.Append(" CONCAT(CONCAT(CONCAT(CASE WHEN ST.ST_NG_BARS1 IS NOT NULL OR ST.ST_NG_BARS2 IS NOT NULL OR ST.ST_NG_BARS3 IS NOT NULL THEN '" + _localization.GetLocalizedHtmlString("lbl_DefectDetails") + "'END, ");
                selectQuery.Append(" CASE WHEN ST.ST_NG_BARS1 IS NOT NULL THEN  DEFECT1.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo1") + "' || ST.ST_NG_BARS1 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS2 IS NOT NULL THEN DEFECT2.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo2") + "' || ST.ST_NG_BARS2 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END), CASE WHEN ST.ST_NG_BARS3 IS NOT NULL THEN  DEFECT3.PROC_DEFECT_SHORT || '" + _localization.GetLocalizedHtmlString("lbl_BundleNo3") + "' || ST.ST_NG_BARS3 || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' END)  ");
                selectQuery.Append(" END AS ST_REMARKS, ");
            }
            

            selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN ST.ST_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN ST.ST_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS ST_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP ");
            }


            selectQuery.Append(" FROM ");
			selectQuery.Append(" TRN_STAMPING ST ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = ST.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = ST.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = ST.ST_END_OPERATOR_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON ST.ST_NG_DETAILS1 = DEFECT1.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON ST.ST_NG_DETAILS2 = DEFECT2.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON ST.ST_NG_DETAILS3 = DEFECT3.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = ST.ST_NEXT_PROCESS");

            var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCiteria))
			{
				query = query + " ORDER BY " + sortCiteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Hardness Tempering process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetHardnessTemperingSQLForFactory(List<SearchField> searchCriteria, string sortCiteria)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<HardeningTemperingDetails> result = new List<HardeningTemperingDetails>();
			selectQuery.Append(" SELECT ");
			selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO, ");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
			selectQuery.Append(" PP.MATL_DIM_D  AS MATL_DIM_D, ");
			selectQuery.Append(" PP.MATL_SHAPE AS MATL_SHAPE, ");
			selectQuery.Append(" PP.PROD_DIM_D AS PROD_DIM_D , ");
			selectQuery.Append(" PP.PROD_SHAPE  AS PROD_SHAPE, ");

			selectQuery.Append(" HT.METALLIC_PLATE_CODE AS METALLIC_PLATE_CODE, ");
			selectQuery.Append(" PP.NO_OF_UNITS || ' ' || ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN  PP.UNIT = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN  PP.UNIT = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN  PP.UNIT = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN  PP.UNIT = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END  || '.' || PP.ACT_ALLOC_QTY AS PP_UNIT_WT, ");


			selectQuery.Append(" HT.NO_OF_UNITS || ' ' || ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN  HT.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN  HT.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN  HT.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN  HT.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END  || '.' || HT.WEIGHT AS HT_UNIT_WT, ");

            //selectQuery.Append(" HT.WATER_OIL_TYPE,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN  HT.WATER_OIL_TYPE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_Water") + "'");
            selectQuery.Append(" WHEN  HT.WATER_OIL_TYPE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_LowUQE") + "'");
            selectQuery.Append(" WHEN  HT.WATER_OIL_TYPE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_MiddleUQE") + "'");
            selectQuery.Append(" WHEN  HT.WATER_OIL_TYPE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_HighUQE") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS WATER_OIL_TYPE, ");

            selectQuery.Append("CASE ");
			selectQuery.Append("WHEN HT.TEMPERATURE_FURNACE IS NOT NULL THEN HT.TEMPERATURE_FURNACE || ' ℃' ");
			selectQuery.Append("ELSE NULL ");
			selectQuery.Append("END AS TEMPERATURE_FURNACE, ");

			selectQuery.Append("CASE ");
			selectQuery.Append("WHEN HT.CONCENTRATION_FURNACE IS NOT NULL THEN HT.CONCENTRATION_FURNACE || ' %' ");
			selectQuery.Append("ELSE NULL ");
			selectQuery.Append("END AS CONCENTRATION_FURNACE, ");

			selectQuery.Append("CASE ");
			selectQuery.Append("WHEN HT.FREQUENCY_FURNACE IS NOT NULL THEN HT.FREQUENCY_FURNACE || ' Hz' ");
			selectQuery.Append("ELSE NULL ");
			selectQuery.Append("END AS FREQUENCY_FURNACE, ");

			selectQuery.Append("CASE ");
			selectQuery.Append("WHEN HT.C_FURNACE IS NOT NULL THEN HT.C_FURNACE || ' %' ");
			selectQuery.Append("ELSE NULL ");
			selectQuery.Append("END AS C_FURNACE, ");

			selectQuery.Append("CASE ");
			selectQuery.Append("WHEN HT.MN_FURNACE IS NOT NULL THEN HT.MN_FURNACE || ' %' ");
			selectQuery.Append("ELSE NULL ");
			selectQuery.Append("END AS MN_FURNACE, ");

			

			selectQuery.Append("CASE ");
			selectQuery.Append("WHEN HT.CR_FURNACE IS NOT NULL THEN HT.CR_FURNACE || ' %' ");
			selectQuery.Append("ELSE NULL ");
			selectQuery.Append("END AS CR_FURNACE, ");

            selectQuery.Append("CASE ");
            selectQuery.Append("WHEN HT.MO_FURNACE IS NOT NULL THEN HT.MO_FURNACE || ' %' ");
            selectQuery.Append("ELSE NULL ");
            selectQuery.Append("END AS MO_FURNACE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN || '～' || PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MIN_MAX,");

			selectQuery.Append(" HT.HARDNESS_MIN || '～' || HT.HARDNESS_MAX AS HARDNESS_MIN, ");

			selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");
			selectQuery.Append(" MAC.MACHINE_NAME || '.' || EMP.SHORT_NAME AS HARDENING_MACHINE_EMP_NAME, ");

			selectQuery.Append(" TO_CHAR(HT.START_DATE_HARDENING, 'YYYY/MM/DD HH24:MI') || '.' || TO_CHAR(HT.END_DATE_HARDENING, 'YYYY/MM/DD HH24:MI') AS DATE_HARDENING_STRING, ");

			selectQuery.Append(" HT.TEMPERATURE_HARDENING || '℃ ×' || HT.TIME_HARDENING || ' 分 '  AS TEMPERATURE_HARDENING, ");

			selectQuery.Append(" HT.Q_HARDNESS_NORTH_UP_1 || ' ' || HT.Q_HARDNESS_NORTH_UP_2 || ' ' || HT.Q_HARDNESS_NORTH_UP_3 || ' ' || HT.Q_HARDNESS_NORTH_UP_CEN AS Q_HARDNESS_NORTH_UP, ");

			selectQuery.Append(" HT.Q_HARDNESS_NORTH_DOWN_1 || ' ' || HT.Q_HARDNESS_NORTH_DOWN_2 || ' ' || HT.Q_HARDNESS_NORTH_DOWN_3 || ' ' || HT.Q_HARDNESS_NORTH_DOWN_CEN AS Q_HARDNESS_NORTH_DOWN, ");


			selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_1 || ' ' || HT.Q_HARDNESS_SOUTH_UP_2 || ' ' || HT.Q_HARDNESS_SOUTH_UP_3 || ' ' || HT.Q_HARDNESS_SOUTH_UP_CEN AS Q_HARDNESS_SOUTH_UP, ");


			selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_1 || ' ' || HT.Q_HARDNESS_SOUTH_DOWN_2 || ' ' || HT.Q_HARDNESS_SOUTH_DOWN_3 || ' ' || HT.Q_HARDNESS_SOUTH_DOWN_CEN AS Q_HARDNESS_SOUTH_DOWN_1, ");


			selectQuery.Append(" MAC1.MACHINE_NAME || '.' || EMP1.SHORT_NAME AS TEMPERING_MACHINE_EMP_NAME, ");

			selectQuery.Append(" TO_CHAR(HT.START_DATE_TEMPERING, 'YYYY/MM/DD HH24:MI') || ' ' || TO_CHAR(HT.END_DATE_TEMPERING, 'YYYY/MM/DD HH24:MI') AS DATE_TEMPERING_STRING, ");

			selectQuery.Append(" HT.TEMPERATURE_TEMPERING || '℃ ×' || HT.TIME_TEMPERING || ' 分 '  AS TEMPERATURE_TEMPERING, ");

			selectQuery.Append(" HT.T_HARDNESS_NORTH_UP_1 || ' ' || HT.T_HARDNESS_NORTH_UP_2 || ' ' || HT.T_HARDNESS_NORTH_UP_3 || ' ' || HT.T_HARDNESS_NORTH_UP_CENT AS T_HARDNESS_NORTH_UP, ");


			selectQuery.Append(" HT.T_HARDNESS_NORTH_DOWN_1 || ' ' || HT.T_HARDNESS_NORTH_DOWN_2 || ' ' || HT.T_HARDNESS_NORTH_DOWN_3 || ' ' || HT.T_HARDNESS_NORTH_DOWN_CEN AS T_HARDNESS_NORTH_DOWN, ");


			selectQuery.Append(" HT.T_HARDNESS_SOUTH_UP_1 || ' ' || HT.T_HARDNESS_SOUTH_UP_2 || ' ' || HT.T_HARDNESS_SOUTH_UP_3 || ' ' || HT.T_HARDNESS_SOUTH_UP_CENT AS T_HARDNESS_SOUTH_UP, ");

			selectQuery.Append(" HT.T_HARDNESS_SOUTH_DOWN_1 || ' ' || HT.T_HARDNESS_SOUTH_DOWN_2 || ' ' || HT.T_HARDNESS_SOUTH_DOWN_3 || ' ' || HT.T_HARDNESS_SOUTH_DOWN_CEN AS T_HARDNESS_SOUTH_DOWN, ");


			selectQuery.Append(" EMPL.SHORT_NAME AS INSPECTED_OPERATOR_NAME, ");
			selectQuery.Append(" TO_CHAR(HTQ.INSPECTION_DATE,'YYYY/MM/DD') AS INSPECTION_DATE_STRING ,");
            //selectQuery.Append(" TO_CHAR(HT.START_DATE_HARDENING, 'YYYY/MM/DD HH24:MI') AS START_DATE_HARDENING_STRING,");
            //selectQuery.Append(" TO_CHAR(HT.END_DATE_HARDENING, 'YYYY/MM/DD HH24:MI') AS END_DATE_HARDENING_STRING,");
            //selectQuery.Append(" TO_CHAR(HT.START_DATE_TEMPERING, 'YYYY/MM/DD HH24:MI') AS START_DATE_TEMPERING_STRING,");
            //selectQuery.Append(" TO_CHAR(HT.END_DATE_TEMPERING, 'YYYY/MM/DD HH24:MI') AS END_DATE_TEMPERING_STRING,");
            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");

			selectQuery.Append(" HTQ.DM_F_MIN || '～' || HTQ.DM_F_MAX AS DM_F_MIN,");

			selectQuery.Append(" HTQ.DM_T_MIN || '～' || HTQ.DM_T_MAX AS DM_T_MIN,");

			selectQuery.Append(" HTQ.STRUCTURE_OUT AS STRUCTURE_OUT,");
			selectQuery.Append(" HTQ.STRUCTURE_IN AS STRUCTURE_IN,");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");

			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN HTQ.DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN HTQ.DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS DECISION, ");
			selectQuery.Append(" M1.MSG_CONTENTS AS HT_REMARK");


			selectQuery.Append(" FROM ");
			selectQuery.Append(" TRN_HARDENING_TEMPERING HT ");
            selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD AND HTQ.FURNACE_NO=HT.FURNACE_NO");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON HT.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = HT.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.HARDENING_MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = HT.OPERATOR_CD_HARDENING ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC1 ON MAC1.MACHINE_CD = HT.TEMPERING_MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = HT.OPERATOR_CD_TEMPERING ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
			
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = HTQ.INSPECTED_BY_CD");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = HTQ.INPUT_LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE M1 ON M1.MSG_CD = HTQ.MSG_CD ");
			var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
                    //if (searchField.FieldName == "HT.HARDENING_MACHINE_CD")
                    //{
                    //	whereCondition = whereCondition + " AND " + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    //}
                    //else if (searchField.FieldName == "HT.TEMPERING_MACHINE_CD")
                    //{
                    //	whereCondition = whereCondition + " OR " + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    //}
                    //else
                    //{
                    //	whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    //	andOperator = " AND ";
                    //}
                    if (searchField.SearchType == SearchType.Custom)
                    {
                        whereCondition += andOperator + "(" + searchField.FieldName + ")";
                    }
                    else
                    {
                        whereCondition += andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    }
                    andOperator = " AND ";
                }

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCiteria))
			{
				query = query + " ORDER BY " + sortCiteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of QA Test Hardness process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetQATestHardnessSQLForFactory(List<SearchField> searchCriteria, string sortCiteria)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<PostHTDetails> result = new List<PostHTDetails>();
			selectQuery.Append(" SELECT ");
			selectQuery.Append(" LOC.SHORT_NAME AS INSPECTION_LOCATION_NAME, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS IQ_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(IQ.INT_QLTY_TEST_DATE, 'yyyy/mm/dd') AS INT_QLTY_TEST_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS IQ_START_OPERATOR_NAME , ");
            selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME, ");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS CHUUBAN, ");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME, ");
			selectQuery.Append(" PP.CHARGE AS CHARGE, ");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,'','' ) AS PP_MATL_DIM_D_STRING,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('','',PP.MATL_SHAPE,'' ) AS PP_MATL_SHAPE, ");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'','' ) AS PP_PROD_DIM_D_STRING,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('','',PP.PROD_SHAPE,'' ) AS PP_PROD_SHAPE,");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
			selectQuery.Append(" IQ.FURNACE_NO AS FURNACE_NO, ");
			selectQuery.Append(" CONCAT(IQ.LOCATION_IN_FURNACE1, ");
			selectQuery.Append(" CASE  WHEN IQ.LOCATION_IN_FURNACE2 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(', ', IQ.LOCATION_IN_FURNACE2) ");
			selectQuery.Append(" ELSE   NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(" ) AS LOCATION_IN_FURNACE1_2, ");
			selectQuery.Append(" IQ.HT_CONDITION, ");
            //selectQuery.Append(" IQ.STACK_POSITION, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 0 ");
            selectQuery.Append(" THEN '' ");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Above") + "'");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Medium") + "'");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Below") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STACK_POSITION, ");
            selectQuery.Append(" IQ.PATTERN_NO AS PATTERN_NO, ");
			selectQuery.Append(" IQ.HRD_CENTER AS HRD_CENTER, ");
			selectQuery.Append(" IQ.ACT_DM_F AS ACT_DM_F, ");
			selectQuery.Append(" IQ.HRD_D4_1 AS HRD_D4_1, ");
			selectQuery.Append(" CONCAT(IQ.ACT_DM_T_MIN, ");
			selectQuery.Append(" CASE  WHEN IQ.ACT_DM_T_MAX IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(', ', IQ.ACT_DM_T_MAX) ");
			selectQuery.Append(" ELSE   NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(" ) AS ALL_ACT_DM_T, ");
			selectQuery.Append(" IQ.HRD_D4_2 AS HRD_D4_2, ");
			selectQuery.Append(" IQ.HRD_D4_3 AS HRD_D4_3, ");
			selectQuery.Append(" IQ.ACT_DM_T_AVG AS ACT_DM_T_AVG_STRING,");
			selectQuery.Append(" IQ.HRD_D4_4 AS HRD_D4_4, ");
			selectQuery.Append(" IQ.ACT_STRUCTURE, ");
			selectQuery.Append(" IQ.HRD_D4_AVG AS HRD_D4_AVG, ");
			selectQuery.Append(" M.MSG_CONTENTS AS INT_QLTY_REMARKS, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS INTERNAL_QUALITY_DECISION ");
			selectQuery.Append(" FROM ");
			selectQuery.Append(" TRN_INTERNAL_QUALITY IQ ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON IQ.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = IQ.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = IQ.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = IQ.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
			selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD =IQ.INT_QLTY_TEST_OPERATOR_CD ");


			var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCiteria))
			{
				query = query + " ORDER BY " + sortCiteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Mech Hardness process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetMechHardnessSQLForFactory(List<SearchField> searchFields, string sortCriteria)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<MIHardnessDetails> result = new List<MIHardnessDetails>();
			selectQuery.Append(" SELECT  ");
			selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
			selectQuery.Append(" TO_CHAR(MI.HD_TEST_DATE, 'yyyy/mm/dd HH24:MI') AS HD_TEST_DATE_STRING,");
            selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
			selectQuery.Append(" MI.DETAIL_NO, ");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,'','' ) AS PP_MATL_DIM_D_STRING,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('','',PP.MATL_SHAPE,'' ) AS PP_MATL_SHAPE,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'','' ) AS PP_PROD_DIM_D_STRING,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('','',PP.PROD_SHAPE,'' ) AS PP_PROD_SHAPE,");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
			selectQuery.Append(" MI.HD_FURNACE_NO ,");
			selectQuery.Append(" MI.HD_ACT_DIM,");
			selectQuery.Append(" MI.HD_SUTEOSHI,");
			selectQuery.Append(" MI.HD_D4_1 ,");
			selectQuery.Append(" MI.HD_D4_2 ,");
			selectQuery.Append(" MI.HD_D4_3 , ");
			selectQuery.Append(" MI.HD_D4_4 , ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 50 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
			selectQuery.Append(" WHEN MI.HD_PATTERN_ACT = 99 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS HD_PATTERN_ACT, ");

			selectQuery.Append(" MI.HD_D4_AVG, ");
			selectQuery.Append(" MESS.MSG_CONTENTS AS REMARKS, ");

			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN MI.HD_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN MI.HD_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN MI.HD_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN MI.HD_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN MI.HD_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN MI.HD_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS HD_DECISION ");

			selectQuery.Append(" FROM  TRN_MI_HARDNESS MI ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = MI.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = MI.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = MI.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME   INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
			selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = MI.HD_TEST_OPERATOR_CD ");

			var query = selectQuery.ToString();
			if (searchFields != null && searchFields.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchFields)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}
				query = query + whereCondition;

			}

			if (!string.IsNullOrEmpty(sortCriteria))
			{
				query = query + " ORDER BY " + sortCriteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Mech Tensile process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetMechTensileSQLForFactory(List<SearchField> searchCriteria, string sortCriteria)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<TensileDetails> result = new List<TensileDetails>();
			selectQuery.Append(" SELECT ");
			selectQuery.Append(" LOC.SHORT_NAME AS INSPECTION_LOCATION_NAME, ");
			selectQuery.Append(" TO_CHAR(TENSILE.TS_TEST_DATE, 'YYYY/MM/DD HH24:MI') AS TS_TEST_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME , ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
			selectQuery.Append(" TENSILE.DETAIL_NO AS DETAIL_NO,");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
			selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,'','' ) AS PP_MATL_DIM_D_STRING,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('','',PP.MATL_SHAPE,'' ) AS PP_MATL_SHAPE,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'','' ) AS PP_PROD_DIM_D_STRING,");
			selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('','',PP.PROD_SHAPE,'' ) AS PP_PROD_SHAPE,");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
			selectQuery.Append(" TENSILE.TS_FURNACE_NO AS TS_FURNACE_NO, ");
			selectQuery.Append(" TENSILE.TS_MAX_LOAD_N AS TS_MAX_LOAD_N, ");
			selectQuery.Append(" TENSILE.TS_YIELD_LOAD_N AS TS_YIELD_LOAD_N, ");
			selectQuery.Append(" TENSILE.TS_ACT_DIM AS TS_ACT_DIM, ");			
			selectQuery.Append(" TENSILE.TS_CUT_DIM, ");
            selectQuery.Append(" TENSILE.TS_TEST_SPECIMEN_DIM, ");
            selectQuery.Append(" TENSILE.TS_GAUGE_LENGTH AS TS_GAUGE_LENGTH, ");
			selectQuery.Append(" TENSILE.TS_EXTENDED_LENGTH ,");
			selectQuery.Append(" CONCAT(TS_TEST_IMPACT_1, CONCAT( CASE  WHEN TS_TEST_IMPACT_2 IS NOT NULL THEN  CONCAT(',', TS_TEST_IMPACT_2) ELSE NULL  END, CASE WHEN TS_TEST_IMPACT_3 IS NOT NULL THEN CONCAT (',', TS_TEST_IMPACT_3) ELSE NULL END) )AS TS_TEST_IMPACT_1_2_3,");
			selectQuery.Append(" MSG.MSG_CONTENTS AS REMARKS, ");
			selectQuery.Append(" TENSILE.TS_TENSILE AS TS_TENSILE, ");
			selectQuery.Append(" TENSILE.TS_YIELD_STRENGTH AS TS_YIELD_STRENGTH, ");
			selectQuery.Append(" TENSILE.TS_ELONGATION AS TS_ELONGATION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN TENSILE.TS_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN TENSILE.TS_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN TENSILE.TS_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN TENSILE.TS_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN TENSILE.TS_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS TS_DECISION, ");
			selectQuery.Append(" TENSILE.TS_REDUCTION AS TS_REDUCTION ");

			selectQuery.Append(" FROM ");
			selectQuery.Append(" TRN_MI_TENSILE_STRENGTH TENSILE ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON TENSILE.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = TENSILE.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = TENSILE.MSG_CD ");
			selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = TENSILE.TS_TEST_OPERATOR_CD  ");

			var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = "AND ";
				}

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCriteria))
			{
				query = query + " ORDER BY " + sortCriteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}

		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of Final Inspection process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetFinalInspectionSQLForFactory(List<SearchField> searchFields, string sortCriteria)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<FinalInspectionDetails> result = new List<FinalInspectionDetails>();
			selectQuery.Append(" SELECT  ");
			selectQuery.Append(" LOCA.SHORT_NAME AS PROC_LOCATION_NAME,");
			selectQuery.Append(" TO_CHAR(FI.INSP_DATE, 'yyyy/mm/dd HH24:MI') AS INSPECTED_DATE_STRING,");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
			selectQuery.Append(" NVL(NIFUDA.NIFUDA_NO,FI.FI_NIFUDA_NO) AS FI_NIFUDA_NO,");
			selectQuery.Append(" FI.FI_TABA_NO AS FI_TABA_NO,");
			selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME,");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST,");
			selectQuery.Append(" PP.CHARGE AS CHARGE,");
			selectQuery.Append(" PP.PROD_DIM_D AS PROD_DIM_D,");
			selectQuery.Append(" PP.PROD_SHAPE AS PROD_SHAPE,");
			selectQuery.Append(" CONCAT(PP.DIM_TOL_MAX_CHR, CASE WHEN PP.DIM_TOL_MIN_CHR IS NOT NULL THEN  CONCAT(',', PP.DIM_TOL_MIN_CHR) ELSE NULL  END) AS ALL_DIM_TOL_MIN_MAX_CHR,");
            //selectQuery.Append(" TO_NUMBER(FI.FI_DIM1) AS FI_DIM1,");
            //selectQuery.Append(" TO_NUMBER(FI.FI_DIM2) AS FI_DIM2,");
            //selectQuery.Append(" FI.FI_DIM3,");
            //selectQuery.Append(" FI.FI_DIM4,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM1, 'FM990.000') AS FI_DIM1_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM2, 'FM990.000') AS FI_DIM2_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM3, 'FM990.000') AS FI_DIM3_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM4, 'FM990.000') AS FI_DIM4_STRING,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PROD_LENGTH_STRING, ");
			selectQuery.Append(" CONCAT(PP.LENGTH_TOL_MAX_CHR, CASE WHEN PP.LENGTH_TOL_MIN_CHR IS NOT NULL THEN  CONCAT(',', PP.LENGTH_TOL_MIN_CHR) ELSE NULL  END) AS ALL_LENGTH_TOL_MIN_CHR,");
			selectQuery.Append(" FI.FI_LENGTH AS FI_LENGTH,");
			selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
			selectQuery.Append(" FI.NO_OF_BARS AS NO_OF_BARS_VALUE,");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN FI.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN FI.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN FI.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS UNIT_TYPE, ");
			selectQuery.Append(" FI.FI_QTY AS PP_FINISHED_QTY, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.COMBINED_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.COMBINED_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN FI.COMBINED_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS COMBINED_DECISION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.FI_DIM_DECISION = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.FI_DIM_DECISION = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS FI_DIM_DECISION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.FI_LENGTH_DECISION = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.FI_LENGTH_DECISION = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS FI_LENGTH_DECISION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.FI_COLOR_DECISION = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.FI_COLOR_DECISION = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS FI_COLOR_DECISION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.FI_CHAMFERING_DECISION = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.FI_CHAMFERING_DECISION = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS FI_CHAMFERING_DECISION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.FI_SPARK_DECISION = 0 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
			selectQuery.Append(" WHEN FI.FI_SPARK_DECISION = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.FI_SPARK_DECISION = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS FI_SPARK_DECISION, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN FI.FI_EXT_TEXTURE_DECISION = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN FI.FI_EXT_TEXTURE_DECISION = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS FI_EXT_TEXTURE_DECISION, ");
			selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO1,");
			selectQuery.Append(" EMP.SHORT_NAME AS EMPLOYEE_NAME,");
			selectQuery.Append(" MESS.MSG_CONTENTS AS FI_REMARK ");

			selectQuery.Append(" FROM TRN_FINAL_INSPECTION FI");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD =FI.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = PP.PRODUCT_CD ");
			selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = FI.MSG_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = FI.INSP_BY ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
			selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIFUDA ON NIFUDA.NIFUDA_CD=FI.NIFUDA_CD ");

			var query = selectQuery.ToString();
			if (searchFields != null && searchFields.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchFields)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}

				query = query + whereCondition;

			}
			if (!string.IsNullOrEmpty(sortCriteria))
			{
				query = query + " ORDER BY " + sortCriteria;
			}
			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}


		/// <summary>
		/// CREATED BY : Pallavi Gawade        
		/// used screens: Factory Daily Work Status Search
		/// </summary>
		/// <returns>
		/// SQL of General Process data according to search criteria to download CSV      
		/// </returns>
		/// 
		public string GetGeneralDailyProcessSQLForFactory(List<SearchField> searchCriteria, string sortCiteria, string cultureName)
		{
			StringBuilder selectQuery = new StringBuilder();
			List<GeneralDailyProcessDetails> result = new List<GeneralDailyProcessDetails>();
			selectQuery.Append(" SELECT ");
			if (cultureName == "en")
			{
				selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN,");
			}
			else
			{
				selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
			}
			selectQuery.Append(" LOC.SHORT_NAME AS GP_LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(GP.GP_START_DATE, 'YYYY/MM/DD HH24:MI') AS GP_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(GP.GP_END_DATE, 'YYYY/MM/DD HH24:MI') AS GP_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS GP_END_OPERATOR_NAME, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS GP_MACHINE_NAME, ");
			selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
			selectQuery.Append(" LPAD(PP.GYOUBAN, 2, '0') AS PP_GYOUBAN_STRING, ");
			selectQuery.Append(" CONCAT(GP_TABA_NO_1, CONCAT(CONCAT( ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP_TABA_NO_2 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', GP_TABA_NO_2) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP_TABA_NO_3 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', GP_TABA_NO_3) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(" ), ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP_TABA_NO_4 IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', GP_TABA_NO_4) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(" )) AS ALL_TABA_NO, ");
			selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
			selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
			selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
			selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
			selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
			selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
			selectQuery.Append(" CONCAT(DIM_TOL_MAX_CHR, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN DIM_TOL_MIN_CHR IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', DIM_TOL_MIN_CHR) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(") AS DIM_TOL_MIN_MAX_CHR, ");
			selectQuery.Append(" CONCAT(LENGTH_TOL_MAX_CHR, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN LENGTH_TOL_MIN_CHR IS NOT NULL THEN ");
			selectQuery.Append(" CONCAT(',', LENGTH_TOL_MIN_CHR) ");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" NULL ");
			selectQuery.Append(" END ");
			selectQuery.Append(") AS LENGTH_TOL_MIN_MAX_CHR, ");
			selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
			selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
			selectQuery.Append(" GP.GP_OK_UNITS ,");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP.UNIT_TYPE = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN GP.UNIT_TYPE = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN GP.UNIT_TYPE = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN GP.UNIT_TYPE = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS UNIT_TYPE, ");
			selectQuery.Append(" GP.GP_QTY AS GP_QTY_DOUBLE, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT1_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT1_JP, ");
            }
            //selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_STRING1, ");

            selectQuery.Append(" GP.GP_NG_UNITS1 ||' '|| ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS GP_NG_UNIT_TYPE1, ");
			selectQuery.Append(" GP.GP_NG_QTY1, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT2_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT2_JP, ");
            }
            //selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_STRING2, ");
            selectQuery.Append(" GP.GP_NG_UNITS2 ||' ' || ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS GP_NG_UNIT_TYPE2, ");
			
			selectQuery.Append(" GP.GP_NG_QTY2, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT3_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT3_JP, ");
            }
            //selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_STRING3, ");

            selectQuery.Append(" GP.GP_NG_UNITS3 ||' '||");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 1 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 2 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 3 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
			selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 4 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS GP_NG_UNIT_TYPE3, ");
			
			selectQuery.Append(" GP.GP_NG_QTY3, ");
			
			selectQuery.Append(" GP.DAILY_PROCESS_NOTE, ");
			selectQuery.Append(" CASE ");
			selectQuery.Append(" WHEN GP.GP_DECISION = 10 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 20 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 30 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 40 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 60 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 65 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 70 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 73 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 76 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 80 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
			selectQuery.Append(" WHEN GP.GP_DECISION = 90 ");
			selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
			selectQuery.Append(" ELSE ");
			selectQuery.Append(" '' ");
			selectQuery.Append(" END AS GP_DECISION, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            }
            else
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP ");
            }

            selectQuery.Append(" FROM ");
			selectQuery.Append(" TRN_GENERAL_DAILY_PROCESS GP ");
			selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON GP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
			selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = GP.MACHINE_CD ");
			selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
			selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
			selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS1 ");
			selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS2 ");
			selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS3 ");
			selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = GP.PROCESS_KUBUN_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = GP.GP_NEXT_PROCESS ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD= GP.LOCATION_CD ");
			selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD= GP.GP_END_OPERATOR_CD");

			var query = selectQuery.ToString();
			if (searchCriteria != null && searchCriteria.Count > 0)
			{
				string whereCondition = " WHERE ";
				string andOperator = string.Empty;

				foreach (var searchField in searchCriteria)
				{
					whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
					andOperator = " AND ";
				}

				query = query + whereCondition;
			}
			if (!string.IsNullOrEmpty(sortCiteria))
			{
				query = query + " ORDER BY " + sortCiteria;
			}

			selectQuery = new StringBuilder();
			selectQuery.Append(query);
			return selectQuery.ToString();
		}


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/04/17       
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Heat Treatment process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHeatTreatmentSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {

            StringBuilder selectQuery = new StringBuilder();
            List<HeatTreatMentDetails> result = new List<HeatTreatMentDetails>();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, (''))  AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_PROD_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION, ");

            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" 'R-' || PP.CORNER_RADIUS ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS NEJIRE_SPAN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");           
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE , ");

            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");


            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" PROCESS1.PROCESS_NAME  AS PP_PROCESS_1, ");
            selectQuery.Append(" PROCESS2.PROCESS_NAME  AS PP_PROCESS_2, ");
            selectQuery.Append(" PROCESS3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" PROCESS4.PROCESS_NAME  AS PP_PROCESS_4, ");
            selectQuery.Append(" PROCESS5.PROCESS_NAME  AS PP_PROCESS_5, ");
            selectQuery.Append(" PROCESS6.PROCESS_NAME  AS PP_PROCESS_6, ");
            selectQuery.Append(" PROCESS7.PROCESS_NAME  AS PP_PROCESS_7, ");
            selectQuery.Append(" PROCESS8.PROCESS_NAME  AS PP_PROCESS_8, ");
            selectQuery.Append(" PROCESS9.PROCESS_NAME  AS PP_PROCESS_9, ");
            selectQuery.Append(" PROCESS10.PROCESS_NAME  AS PP_PROCESS_10, ");
            selectQuery.Append(" PROCESS11.PROCESS_NAME  AS PP_PROCESS_11, ");
            selectQuery.Append(" PROCESS12.PROCESS_NAME  AS PP_PROCESS_12, ");
            selectQuery.Append(" PROCESS13.PROCESS_NAME  AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS TEST_SPECIMEN , ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");


            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");


            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");
            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");

            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");

            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE , ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PROCESS_PLAN_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");


            selectQuery.Append(" PP.KAKOUIARI_QTY AS KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PROCESS_PLAN_PRINT_DATE_STRING, ");
            //HeatTreatment
            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS MACHINE_NAME, ");
            selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO,  ");

            selectQuery.Append(" HT.HT_CONDITION AS HT_CONDITION, ");

            selectQuery.Append(" HT.PATTERN_NO AS PATTERN_NO, ");
            selectQuery.Append(" TO_CHAR(HT.HT_START_DATE, 'yyyy/mm/dd HH24:MI') AS HT_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS SETUP_CONF_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS LOADING_CONF_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" TO_CHAR(HT.PLANNED_UNLOAD_DATE, 'yyyy/mm/dd HH24:MI') AS PLANNED_UNLOAD_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(HT.HT_END_DATE, 'yyyy/mm/dd HH24:MI') AS HT_END_DATE_STRING, ");
            selectQuery.Append(" EMP3.SHORT_NAME AS OFFLOADED_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" EMP4.SHORT_NAME AS OFFLOAD_CONF_BY_EMPLOYEE_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN HT.COIL_BAR_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN HT.COIL_BAR_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN HT.COIL_BAR_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS COIL_BAR_TYPE, ");

            selectQuery.Append(" HT.LOT_NO AS LOT_NO, ");
            selectQuery.Append(" HT.HT_NO_OF_UNITS AS HT_NO_OF_UNITS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");

            selectQuery.Append(" HT.HT_WEIGHT AS HT_WEIGHT, ");

            selectQuery.Append(" HT.ROLL_MONTH AS ROLL_MONTH, ");
            selectQuery.Append(" HT.COIL_NO AS COIL_NO, ");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE1 AS LOCATION_IN_FURNACE1, ");
            selectQuery.Append(" HT.LOCATION_IN_FURNACE2 AS LOCATION_IN_FURNACE2, ");
            selectQuery.Append(" HT.HT_TABA_NO_1 AS HT_TABA_NO_1, ");
            selectQuery.Append(" HT.HT_TABA_NO_2 AS HT_TABA_NO_2, ");
            selectQuery.Append(" HT.HT_TABA_NO_3 AS HT_TABA_NO_3, ");
            selectQuery.Append(" HT.HT_TABA_NO_4 AS HT_TABA_NO_4, ");
            selectQuery.Append(" MESS.MSG_CONTENTS AS PP_PROCESS_PLAN_REMARK,");
            selectQuery.Append(" HT.HT_REMARK AS HT_REMARKS");


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HEAT_TREATMENT HT ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = HT.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = HT.SETUP_CONF_BY ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = HT.LOADING_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP3 ON EMP3.EMPLOYEE_CD = HT.OFFLOADED_BY ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP4 ON EMP4.EMPLOYEE_CD = HT.OFFLOAD_CONF_BY ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS1 ON PP.PROCESS_1_CD = PROCESS1.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS2 ON PP.PROCESS_2_CD = PROCESS2.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS3 ON PP.PROCESS_3_CD = PROCESS3.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS4 ON PP.PROCESS_4_CD = PROCESS4.PROCESS_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS5 ON PP.PROCESS_5_CD = PROCESS5.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS6 ON PP.PROCESS_6_CD = PROCESS6.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS7 ON PP.PROCESS_7_CD = PROCESS7.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS8 ON PP.PROCESS_8_CD = PROCESS8.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS9 ON PP.PROCESS_9_CD = PROCESS9.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS10 ON PP.PROCESS_10_CD = PROCESS10.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS11 ON PP.PROCESS_11_CD = PROCESS11.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS12 ON PP.PROCESS_12_CD = PROCESS12.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS13 ON PP.PROCESS_13_CD = PROCESS13.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = HT.MSG_CD ");

            string whereCondition = "";
            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {


                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/04/19       
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Drawing Bar To Bar process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHikinukiBarToBarSQLForDailyProcess(List<SearchField> searchCriteria, string sortCriteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.SHORT_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", EMPL.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(", NVL(INTE.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(", PP.PROD_DIM_W AS PP_PROD_DIM_W");
            selectQuery.Append(", PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(", PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN");
            selectQuery.Append(", PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX");
            selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(", PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER");
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS P_COIL_DIA_WINDING");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");


            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_R, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN");

            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN");

            //selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT");

            selectQuery.Append(", PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");

            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR_NAME");
            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");
            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13");
            selectQuery.Append(", PP.TEST_SPECIMEN AS TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE ");

            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1,");


            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE ");

            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX,");
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1");

            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2");

            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3");


            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.RANK_STD AS PP_RANK_STD");

            selectQuery.Append(", PP.HT_CONDITION AS PP_HT_CONDITION,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK ");

            selectQuery.Append(", PP.MATL_NAME_MAK AS MATL_MAKER_NAME ");

            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH ");
            selectQuery.Append(" ,CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION ");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE,");


            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN ");

            selectQuery.Append(", PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT ");

            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");

            // Drawing D 
            selectQuery.Append(", MAC.MACHINE_NAME AS DW_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS DW_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( DD.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(", EMP1.SHORT_NAME AS DW_START_OPERATOR_NAME");
            selectQuery.Append(", DD.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE");
            selectQuery.Append(", DD.DIE_DIM AS DW_DIE_DIM");
            selectQuery.Append(", DD.DIE_SHAPE AS DIE_SHAPE");
           
            selectQuery.Append(", DD.DIE_NO AS DW_DIE_NO");
            selectQuery.Append(", DD.HT_CONDITION AS HT_CONDITION");

            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN DD.IM_DRAWING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN DD.IM_DRAWING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_InBetween") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IM_DRAWING, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DD.REDRAWING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN DD.REDRAWING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS REDRAWING ");
            selectQuery.Append(", TO_CHAR( DD.DW_END_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(", EMP2.SHORT_NAME AS DW_END_OPERATOR_NAME");            

            selectQuery.Append(", TO_CHAR(DD.DW_ACT_DIM1, 'FM990.000') AS DW_ACT_DIM1_STRING ");
            selectQuery.Append(", TO_CHAR(DD.DW_ACT_DIM2, 'FM990.000') AS DW_ACT_DIM2_STRING ");
            selectQuery.Append(", TO_CHAR(DD.DW_ACT_DIM3, 'FM990.000') AS DW_ACT_DIM3_STRING ");

            selectQuery.Append(", DD.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1");
            selectQuery.Append(", DD.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2");
            selectQuery.Append(", DD.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3");
            selectQuery.Append(", DD.DW_FINISHED_BARS AS DW_FINISHED_BARS,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DD.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN DD.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN DD.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN DD.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE ");


            selectQuery.Append(", DD.DW_FINISHED_QTY AS DW_FINISHED_QTY");
            selectQuery.Append(", DD.DW_TABA_NO_ AS DW_TABA_NO_,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
            selectQuery.Append(" WHEN DD.DW_BEND_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_BEND_STATUS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN DD.DW_EXT_TEXTURE_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_EXT_TEXTURE_STATUS");

            selectQuery.Append(", DD.DW_NG_BARS1 AS DW_NG_BARS1");
            if (cultureName == "en")
            {
                selectQuery.Append(" ,DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN ");
            }
            else
            {
                selectQuery.Append(" ,DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP ");
            }
            //selectQuery.Append(" ,DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1");
            selectQuery.Append(", DD.DW_NG_BARS2 AS DW_NG_BARS2");
            if (cultureName == "en")
            {
                selectQuery.Append(" ,DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN ");
            }
            else
            {
                selectQuery.Append(" ,DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP ");
            }
            //selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2");
            selectQuery.Append(", DD.DW_NG_BARS3 AS DW_NG_BARS3");
            if (cultureName == "en")
            {
                selectQuery.Append(" , DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN ");
            }
            else
            {
                selectQuery.Append(" , DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP ");
            }
            //selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3");
            selectQuery.Append(", DD.DW_CORRECTED_BARS AS DW_CORRECTED_BARS");
            selectQuery.Append(", DD.DW_DISPOSAL_NO AS DW_DISPOSAL_NO,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DD.DW_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN DD.DW_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DECISION ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS DW_REMARK ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER JOIN TRN_DRAWING_D DD ON DD.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DD.DW_START_OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DD.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DD.MACHINE_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = DD.DW_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = DD.DW_END_OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DD.MSG_CD");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/04/17       
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Hikinuki Coil to Bar process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHikinukiCoilToBarSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria, string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_PROD_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME ,PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");


            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE, ");
            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");


            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10, ");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11, ");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12, ");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS TEST_SPECIMEN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");


            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS PP_IMPACT_TEST_PATTERN1, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS PP_IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS PP_IMPACT_TEST_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");

            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");


            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");

            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");


            selectQuery.Append(" MAC.MACHINE_NAME AS DW_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(DW.DW_START_DATE, 'yyyy/mm/dd HH24:MI') AS DW_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS DW_START_OPERATOR_NAME, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.TP_TEST = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun1of3") + "'");
            selectQuery.Append(" WHEN DW.TP_TEST = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun2of3") + "'");
            selectQuery.Append(" WHEN DW.TP_TEST = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_TP_TEST, ");

            selectQuery.Append(" DW.FURNACE_NO AS DW_FURNACE_NO, ");
            selectQuery.Append(" DW.DW_METALLIC_PLATE_CODE AS DW_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" DW.CARBON_PERCENTAGE AS DW_CARBON_PERCENTAGE, ");
            selectQuery.Append(" DW.MAGNIZ_PERCENTAGE AS DW_MAGNIZ_PERCENTAGE, ");
            selectQuery.Append(" DW.DIE_DIM AS DW_DIE_DIM, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_R") + "'");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun2Of4") + "'");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun3Of4") + "'");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun4Of4") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DIE_SHAPE, ");

            selectQuery.Append(" DW.DIE_NO AS DW_DIE_NO, ");
            selectQuery.Append(" DW.DW_DIM1 AS DW_DIM1, ");
            selectQuery.Append(" DW.DW_DIM2 AS DW_DIM2, ");
            selectQuery.Append(" DW.UPPER_LOWER_ROLLER_ANGLE1 AS DW_UPPER_LOWER_ROLLER_ANGLE1, ");
            selectQuery.Append(" DW.UPPER_LOWER_ROLLER_ANGLE2 AS DW_UPPER_LOWER_ROLLER_ANGLE2, ");
            selectQuery.Append(" DW.UPPER_ROLLER_ELEVATION AS DW_UPPER_ROLLER_ELEVATION, ");
            selectQuery.Append(" DW.DW_TONNAGE AS DW_TONNAGE, ");
            selectQuery.Append(" DW.HEX_R AS DW_HEX_R, ");
            selectQuery.Append(" DW.SHIKII_VALUE AS DW_SHIKII_VALUE, ");
            selectQuery.Append(" DW.EC_COIL_SIZE AS DW_EC_COIL_SIZE, ");
            selectQuery.Append(" DW.EC_COIL_SHAPE AS DW_EC_COIL_SHAPE, ");
            selectQuery.Append(" DW.EC_IS_OK AS DW_EC_IS_OK, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.REDRAWING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN DW.REDRAWING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_REDRAWING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_END_DATE, 'yyyy/mm/dd HH24:MI') AS DW_END_DATE_STRING, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS DW_END_OPERATOR_NAME, ");
           
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM1, 'FM990.000') AS DW_ACT_DIM1_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM2, 'FM990.000') AS DW_ACT_DIM2_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM3, 'FM990.000') AS DW_ACT_DIM3_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM4, 'FM990.000') AS DW_ACT_DIM4_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM5, 'FM990.000') AS DW_ACT_DIM5_STRING, ");
            selectQuery.Append(" TO_CHAR(DW.DW_ACT_DIM6, 'FM990.000') AS DW_ACT_DIM6_STRING, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH1 AS DW_ACT_LENGTH1, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH2 AS DW_ACT_LENGTH2, ");
            selectQuery.Append(" DW.DW_ACT_LENGTH3 AS DW_ACT_LENGTH3, ");
            selectQuery.Append(" DW.DW_FINISHED_BARS AS DW_FINISHED_BARS, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");


            selectQuery.Append(" DW.DW_FINISHED_QTY AS DW_FINISHED_QTY, ");
            selectQuery.Append(" DW.A_SIDE_FURE1 AS DW_A_SIDE_FURE1, ");
            selectQuery.Append(" DW.A_SIDE_FURE2 AS DW_A_SIDE_FURE2, ");
            selectQuery.Append(" DW.A_SIDE_FURE3 AS DW_A_SIDE_FURE3, ");
            selectQuery.Append(" DW.B_SIDE_FURE1 AS DW_B_SIDE_FURE1, ");
            selectQuery.Append(" DW.B_SIDE_FURE2 AS DW_B_SIDE_FURE2, ");
            selectQuery.Append(" DW.B_SIDE_FURE3 AS DW_B_SIDE_FURE3, ");
            selectQuery.Append(" DW.DW_TABA_NO_1 AS DW_TABA_NO_1, ");
            selectQuery.Append(" DW.DW_TABA_NO_2 AS DW_TABA_NO_2, ");
            selectQuery.Append(" DW.DW_TABA_NO_3 AS DW_TABA_NO_3, ");
            selectQuery.Append(" DW.DW_TABA_NO_4 AS DW_TABA_NO_4, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
            selectQuery.Append(" WHEN DW.DW_BEND_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_BEND_STATUS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_EXT_TEXTURE_STATUS,");

            selectQuery.Append(" DW.DW_NG_BARS1 AS DW_NG_BARS1, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP, ");
            }
            //selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1,");
            selectQuery.Append(" DW.DW_NG_BARS2 AS DW_NG_BARS2, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP, ");
            }
            //selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2,");
            selectQuery.Append(" DW.DW_NG_BARS3 AS DW_NG_BARS3, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP, ");
            }
            //selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3,");
            selectQuery.Append(" DW.DW_CORRECTED_BARS AS DW_CORRECTED_BARS, ");
            selectQuery.Append(" DW.DW_DISPOSAL_NO AS DW_DISPOSAL_NO, ");
            selectQuery.Append(" DW.EC_NG_BAR_TOTAL AS DW_EC_NG_BAR_TOTAL, ");
            selectQuery.Append(" DW.MAGNETISM AS DW_MAGNETISM, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DECISION, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS DW_REMARKS ");


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_DRAWING DW ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = DW.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = DW.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DW.DW_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = DW.DW_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD =  DW.DW_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD =  DW.DW_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DW.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/04/26       
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Cold Drawing Wire Drawing(Coil To Coil) process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHikinukiCoilToCoilSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN ");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING  ");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.RECEIVER_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(", NVL(INTE.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(", PP.PROD_DIM_W AS PP_PROD_DIM_W");
            selectQuery.Append(", PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(", PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN");
            selectQuery.Append(", PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX");
            selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(", PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER");
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS COIL_DIA_EXTERNAL,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING");
            
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");

            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_R, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN");

            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS NEJIRE_SPAN,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN");

            //selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT");

            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");

            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR");


            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");

            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_NAME_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_NAME_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_NAME_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_NAME_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_NAME_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_NAME_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_NAME_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_NAME_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_NAME_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_NAME_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_NAME_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_NAME_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_NAME_13");

            selectQuery.Append(", PP.TEST_SPECIMEN AS TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX");


            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE ");

            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE ");


            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1");

            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2");

            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3");

            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN ");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");

            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.RANK_STD AS RANK_STD ");

            selectQuery.Append(", PP.HT_CONDITION AS PP_HT_CONDITION,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK ");

            selectQuery.Append(", PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(", CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PROCESS_PLAN_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN ");

            selectQuery.Append(", PP.KAKOUIARI_QTY AS KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT ");

            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");

            //Cold_Drawing_Wire          
            selectQuery.Append(", MAC.MACHINE_NAME AS DW_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS DW_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( DW.DW_START_DATE,'yyyy/mm/dd HH24:MI' ) AS DW_START_DATE_STRING");
            selectQuery.Append(", EMP1.SHORT_NAME AS DW_START_OPERATOR_NAME");
            selectQuery.Append(", DW.ROLLING_NUMBER AS DW_ROLLING_NUMBER");
            selectQuery.Append(", DW.FURNACE_NO AS DW_FURNACE_NO");
            selectQuery.Append(", DW.DIE_DIM AS DW_DIE_DIM");
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_R") + "'");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun2Of4") + "'");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun3Of4") + "'");
            selectQuery.Append(" WHEN DW.DIE_SHAPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun4Of4") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DIE_SHAPE ");
            selectQuery.Append(", DW.DIE_NO AS DW_DIE_NO,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DIE_CHK_START  = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NewMeasure") + "'");
            selectQuery.Append(" WHEN DW.DIE_CHK_START  = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Good") + "'");
            selectQuery.Append(" WHEN DW.DIE_CHK_START  = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Wrap") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DIE_CHK_START, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_RightCoil") + "'");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_LeftCoil") + "'");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_RightCarrier") + "'");
            selectQuery.Append(" WHEN DW.WINDING_SHAPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_LeftCarrier") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_WINDING_SHAPE, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.IM_DRAWING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN DW.IM_DRAWING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_InBetween") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_IM_DRAWING, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.REDRAWING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN DW.REDRAWING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_REDRAWING ");

            selectQuery.Append(", TO_CHAR( DW.DW_END_DATE,'YYYY/MM/DD HH24:MI' ) AS DW_END_DATE_STRING");
            selectQuery.Append(", EMP2.SHORT_NAME AS DW_END_OPERATOR_NAME ");


            //selectQuery.Append(", DW.DW_ACT_DIM_START AS DW_ACT_DIM_START ");
            //selectQuery.Append(", DW.DW_ACT_DIM_END AS DW_ACT_DIM_END ");

            selectQuery.Append(", TO_CHAR(DW.DW_ACT_DIM_START, 'FM990.000') AS DW_ACT_DIM_START_STRING ");
            selectQuery.Append(", TO_CHAR(DW.DW_ACT_DIM_END, 'FM990.000') AS DW_ACT_DIM_END_STRING ");

            selectQuery.Append(", DW.DW_FINISHED_COILS AS DW_FINISHED_COILS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN DW.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_UNIT_TYPE ");

            selectQuery.Append(", DW.DW_FINISHED_QTY AS DW_FINISHED_QTY");
            selectQuery.Append(", DW.DW_TABA_NO_1 AS DW_TABA_NO_1");
            selectQuery.Append(", DW.DW_TABA_NO_2 AS DW_TABA_NO_2");
            selectQuery.Append(", DW.DW_TABA_NO_3 AS DW_TABA_NO_3");
            selectQuery.Append(", DW.DW_TABA_NO_4 AS DW_TABA_NO_4,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN DW.DW_EXT_TEXTURE_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_EXT_TEXTURE_STATUS,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_MAGNETIC_INSPECTIONS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK1") + "'");
            selectQuery.Append(" WHEN DW.DW_MAGNETIC_INSPECTIONS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK2") + "'");
            selectQuery.Append(" WHEN DW.DW_MAGNETIC_INSPECTIONS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NotGood") + "'");
            selectQuery.Append(" WHEN DW.DW_MAGNETIC_INSPECTIONS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_MAGNETIC_INSPECTIONS,");


            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DIE_CHK_END = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Good") + "'");
            selectQuery.Append(" WHEN DW.DIE_CHK_END = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Wrap") + "'");
            selectQuery.Append(" WHEN DW.DIE_CHK_END = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProdShukkoKubun7_Scrap") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DIE_CHK_END,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN DW.DW_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN DW.DW_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DW_DECISION, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS DW_REMARKS ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT  JOIN TRN_DRAWING_W DW ON DW.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");

            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = DW.DW_START_OPERATOR_CD");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = DW.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON DW.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=DW.DW_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = DW.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/04/26       
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Straighteing1 process data according to search criteria to download CSV      
        /// </returns>
        /// 

        public string GetStraighteningSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VER,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME,PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN_STRING,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX_STRING,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN_SINGLE,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX_SINGLE,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");

            
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

           
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS NEJIRE_SPAN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE , ");

            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");

            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10,");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11,");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12,");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX,");

            
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");

            
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX,");

            
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");

            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");


            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE , 'yyyy/mm/dd') AS PROCESS_PLAN_PRINT_DATE_STRING,");

            //Straightening
            selectQuery.Append(" MACH.MACHINE_NAME AS STR_MACHINE_NAME,");
            selectQuery.Append(" LOCA.SHORT_NAME AS LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(STR.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_TIME,");
            selectQuery.Append(" EMPL.SHORT_NAME AS STR_START_OPERATOR_NAME,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.TP_TEST = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun1of3") + "'");
            selectQuery.Append(" WHEN STR.TP_TEST = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun2of3") + "'");
            selectQuery.Append(" WHEN STR.TP_TEST = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_TP_TEST, ");

            selectQuery.Append(" STR.STR_METALLIC_PLATE_CODE AS STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" STR.FURNACE_NO AS STR_FURNACE_NO,");            

            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_1, 'FM990.000') AS STR_DIM_BEFORE_1_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_2, 'FM990.000') AS STR_DIM_BEFORE_2_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_3, 'FM990.000') AS STR_DIM_BEFORE_3_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_4, 'FM990.000')AS STR_DIM_BEFORE_4_STRING,");

            selectQuery.Append(" STR.STR_LENGTH_BEFORE_1 AS STR_LENGTH_BEFORE_1,");
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_2 AS STR_LENGTH_BEFORE_2,");
            selectQuery.Append(" STR.UPPER_LOWER_ROLLER_ANGLE1 AS STR_UPPER_LOWER_ROLLER_ANGLE1,");
            selectQuery.Append(" STR.UPPER_LOWER_ROLLER_ANGLE2 AS STR_UPPER_LOWER_ROLLER_ANGLE2,");
            selectQuery.Append(" STR.UPPER_ROLLER_ELEVATION AS STR_UPPER_ROLLER_ELEVATION,");
            selectQuery.Append(" STR.RESERVATION_NUMBER AS STR_RESERVATION_NUMBER,");
            selectQuery.Append(" STR.STR_TONNAGE AS STR_TONNAGE,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.RESTRAIGHTING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN STR.RESTRAIGHTING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_RESTRAIGHTING, ");


            selectQuery.Append(" TO_CHAR(STR.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING,");
            selectQuery.Append(" EMPL1.SHORT_NAME AS STR_END_OPERATOR_NAME,");            

            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_1, 'FM990.000') AS STR_DIM_AFTER_1_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_2, 'FM990.000') AS STR_DIM_AFTER_2_STRING ,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_3, 'FM990.000') AS STR_DIM_AFTER_3_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_4, 'FM990.000') AS STR_DIM_AFTER_4_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_5, 'FM990.000') AS STR_DIM_AFTER_5_STRING,");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_6, 'FM990.000') AS STR_DIM_AFTER_6_STRING,");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_1 AS STR_LENGTH_AFTER_1,");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_2 AS STR_LENGTH_AFTER_2,");
            selectQuery.Append(" STR.STR_BARS AS STR_BARS,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");


            selectQuery.Append(" STR.STR_QTY AS TOTAL_STR_QTY,");
            selectQuery.Append(" STR.A_SIDE_FURE1 AS STR_A_SIDE_FURE1,");
            selectQuery.Append(" STR.A_SIDE_FURE2 AS STR_A_SIDE_FURE2,");
            selectQuery.Append(" STR.A_SIDE_FURE3 AS STR_A_SIDE_FURE3,");
            selectQuery.Append(" STR.B_SIDE_FURE1 AS STR_B_SIDE_FURE1,");
            selectQuery.Append(" STR.B_SIDE_FURE2 AS STR_B_SIDE_FURE2,");
            selectQuery.Append(" STR.B_SIDE_FURE3 AS STR_B_SIDE_FURE3,");
            selectQuery.Append(" STR.STR_TABA_NO_1 AS STR_TABA_NO_1,");
            selectQuery.Append(" STR.STR_TABA_NO_2 AS STR_TABA_NO_2,");
            selectQuery.Append(" STR.STR_TABA_NO_3 AS STR_TABA_NO_3,");
            selectQuery.Append(" STR.STR_TABA_NO_4 AS STR_TABA_NO_4,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_EXT_TEXTURE,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_BEND_STATUS, ");

            selectQuery.Append(" STR.STR_NG_BARS1 AS STR_NG_BARS1,");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP, ");
            }
            //selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1,");
            selectQuery.Append(" STR.STR_NG_BARS2 AS STR_NG_BARS2,");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP, ");
            }
            //selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2,");
            selectQuery.Append(" STR.STR_NG_BARS3 AS STR_NG_BARS3,");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP, ");
            }
            //selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_DECISION ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS STR_REMARKS ");

            selectQuery.Append("  FROM ");

            selectQuery.Append(" TRN_STRAIGHTENING STR ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD = STR.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = STR.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL1 ON EMPL1.EMPLOYEE_CD = STR.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = STR.STR_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = STR.MSG_CD ");


            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade       
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Straighteing2 process data according to search criteria to download CSV      
        /// </returns>
        /// 

        public string GetStraighteing2SQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VERR,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER_STRING, ");

            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS CORNER_RADIUS_STRING, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_MAGARI_KUBUN_STRING,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PR_MAGARI_BEND_TOL,");
            selectQuery.Append(" PP.MAGARI_SPAN,");
            selectQuery.Append(" PP.NEJIRE_ANGLE,");
            selectQuery.Append(" PP.NEJIRE_SPAN,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN_STRING,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN,");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX,");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE,");

            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR,");

            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY,");
            selectQuery.Append(" PP.STAMP AS PP_STAMP");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PAT1_MINMAX_STRNG,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");

            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PAT3_ACTUAL_STRNG, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PAT1_MINMAX_STRNG,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN_SINGLE,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX_SINGLE,");
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PAT3_ACTUAL_STRNG, ");


            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PAT1_MINMAX_STRNG,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX,");
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PAT3_ACTUAL_STRNG, ");

            selectQuery.Append(" PP.YEILD_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PAT1_MINMAX_STRNG,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PAT3_ACTUAL_STRNG, ");

            selectQuery.Append(" PP.PRESS_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PAT1_MINMAX_STRNG,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX,");
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PAT3_ACTUAL_STRNG, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_DECARB_STD_1,");

            selectQuery.Append(" PP.DECARB_VAL_1,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_DECARB_STD_2,");

            selectQuery.Append(" PP.DECARB_VAL_2,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_DECARB_STD_3,");

            selectQuery.Append(" PP.DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PAT1_MINMAX_STRNG,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PAT3_ACTUAL_STRNG, ");

            selectQuery.Append(" PP.RANK_STD,");
           
            selectQuery.Append(" PP.HT_CONDITION,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK_STRING, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");

            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_CONDITION_DESCRIPTION ,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN_STRING, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN_STRING, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN_STRING, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN_STRING, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT_TYPE_STRING, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR,");

            selectQuery.Append(" MACH.MACHINE_NAME AS STR_MACHINE_NAME,");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(STR.STR_START_DATE, 'yyyy/mm/dd HH24:MI') AS STR_START_DATE_STRING,");
            selectQuery.Append(" EMPL.SHORT_NAME AS STR_START_OPERATOR_NAME,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.TP_TEST = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun1of3") + "'");
            selectQuery.Append(" WHEN STR.TP_TEST = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun2of3") + "'");
            selectQuery.Append(" WHEN STR.TP_TEST = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_TP_TEST_STRING, ");

            selectQuery.Append(" STR.STR_METALLIC_PLATE_CODE,");
            selectQuery.Append(" STR.FURNACE_NO,");
            
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_1, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_BEFORE_2, 'FM990.000'), ");
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_1,");
            selectQuery.Append(" STR.STR_LENGTH_BEFORE_2,");
            selectQuery.Append(" STR.RESERVATION_NUMBER,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.RESTRAIGHTING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN STR.RESTRAIGHTING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS RESTRAIGHTING, ");
            selectQuery.Append(" TO_CHAR(STR.STR_END_DATE, 'yyyy/mm/dd HH24:MI') AS STR_END_DATE_STRING,");
            selectQuery.Append(" EMPL1.SHORT_NAME AS STR_END_OPERATOR_NAME,");
           
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_X_1, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_X_2, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_X_3, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_Y_1, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_Y_2, 'FM990.000'), ");
            selectQuery.Append(" TO_CHAR(STR.STR_DIM_AFTER_Y_3, 'FM990.000'), ");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_1,");
            selectQuery.Append(" STR.STR_LENGTH_AFTER_2,");
            selectQuery.Append(" STR.STR_BARS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN STR.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_UNIT_TYPE_STRING, ");

            selectQuery.Append(" STR.STR_QTY,");
            selectQuery.Append(" STR.STR_TABA_NO_1,");
            selectQuery.Append(" STR.STR_TABA_NO_2,");
            selectQuery.Append(" STR.STR_TABA_NO_3,");
            selectQuery.Append(" STR.STR_TABA_NO_4,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun2of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun3of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun11of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun4of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun6of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 45 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun7of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun1of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 55 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun9of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ExternalSurfaceConditionKubun10of11") + "'");
            selectQuery.Append(" WHEN STR.STR_EXT_TEXTURE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_EXT_TEXTURE_STATUS_DECODE,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OKStatus") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 15 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun2of7") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun3of7") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 25 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Straightening") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_BendStatusKubun6of7") + "'");
            selectQuery.Append(" WHEN STR.STR_BEND_STATUS = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_BEND_STATUS_DECODE, ");

            selectQuery.Append(" STR.STR_NG_BARS1,");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT1_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1_JP, ");
            }
            //selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT1,");
            selectQuery.Append(" STR.STR_NG_BARS2,");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT2_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2_JP, ");
            }
            //selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT2,");
            selectQuery.Append(" STR.STR_NG_BARS3,");

            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT_SHORT3_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3_JP, ");
            }
            //selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_SHORT3,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN STR.STR_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN STR.STR_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STR_DECISION ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS STR_REMARKS ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STRAIGHTENING2 STR INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = STR.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=STR.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN  MST_MACHINE MACH ON MACH.MACHINE_CD=STR.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD=STR.STR_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL1 ON EMPL1.EMPLOYEE_CD=STR.STR_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD=STR.STR_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = STR.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Chamffer process data according to search criteria to download CSV      
        /// </returns>
        /// 

        public string GetChamfferSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");
           
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE, ");

            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");

            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1,");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2,");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3,");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4,");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5,");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6,");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7,");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8,");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9,");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10,");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11,");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12,");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13,");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");


            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");

            
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");

           
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");
            
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");
            
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS PP_IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS PP_IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS PP_IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");


            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");

            selectQuery.Append(" MAC.MACHINE_NAME AS CH_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS PROCESS_LOCATION, ");
            selectQuery.Append(" TO_CHAR(CH.CH_START_DATE, 'yyyy/mm/dd HH24:MI') AS CH_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS CH_START_OPERATOR_NAME, ");
            selectQuery.Append(" CH.CH_METALLIC_PLATE_CODE AS CH_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" CASE WHEN CH.CH_SHAPE IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || CH.CH_SHAPE ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS CH_SHAPE_STRING, ");
            selectQuery.Append(" CH.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN CH.RECHAMFERING = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN CH.RECHAMFERING  = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS RECHAMFERING, ");

            selectQuery.Append(" TO_CHAR(CH.CH_END_DATE, 'yyyy/mm/dd HH24:MI') AS CH_END_DATE_STRING, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS CH_END_OPERATOR_NAME, ");
            selectQuery.Append(" CH.CH_ACT_LENGTH AS CH_ACT_LENGTH, ");
            selectQuery.Append(" CH.CH_FINISHED_BARS AS CH_FINISHED_BARS, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN CH.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN CH.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN CH.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN CH.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");

            selectQuery.Append(" CH.CH_FINISHED_QTY AS CH_FINISHED_QTY, ");
            selectQuery.Append(" CH.CH_TABA_NO_1 AS CH_TABA_NO_1, ");
            selectQuery.Append(" CH.CH_TABA_NO_2 AS CH_TABA_NO_2, ");
            selectQuery.Append(" CH.CH_TABA_NO_3 AS CH_TABA_NO_3, ");
            selectQuery.Append(" CH.CH_TABA_NO_4 AS CH_TABA_NO_4, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN CH.CH_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN CH.CH_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS CH_DECISION ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS CH_REMARKS ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_CHAMFERING CH ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = CH.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");

            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = CH.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = CH.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = CH.CH_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = CH.CH_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS  P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = CH.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of EC process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetECSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN ");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.SHORT_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            
            selectQuery.Append(",NVL(INTE.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(", PP.PROD_DIM_W AS PP_PROD_DIM_W");
            selectQuery.Append(", PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(", PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN");
            selectQuery.Append(", PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX");
            selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(", PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER");
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL");
           
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING");

            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
           
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

           
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS ||'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_R, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN");

            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS NEJIRE_SPAN,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN");

            //selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT");

            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");

            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR");

            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");

            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13");

            selectQuery.Append(", PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX");


            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1,");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");

           
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX,");

           
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1");

            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2");

            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3");

            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN ");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");
            
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.RANK_STD AS RANK_STD ");
            selectQuery.Append(",PP.HT_CONDITION AS PP_HT_CONDITION, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK ");

            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
           
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(", CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION ");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PROCESS_PLAN_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN ");



            selectQuery.Append(", PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS,  ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT ");

            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");

            //EC
            selectQuery.Append(", MAC.MACHINE_NAME AS EC_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS EC_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( EC.EC_START_DATE,'yyyy/mm/dd HH24:MI' ) AS EC_START_DATE_STRING");
            selectQuery.Append(", EMP1.SHORT_NAME AS EC_START_OPERATOR_NAME");
            selectQuery.Append(", EC.EC_METALLIC_PLATE_CODE AS EC_METALLIC_PLATE_CODE");
            selectQuery.Append(", EC.FURNACE_NO AS FURNACE_NO,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN EC.TP_TEST = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun1of3") + "'");
            selectQuery.Append(" WHEN EC.TP_TEST = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun2of3") + "'");
            selectQuery.Append(" WHEN EC.TP_TEST = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_TPTestKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TP_TEST ");

            selectQuery.Append(", EC.IS_OK AS IS_OK");
            selectQuery.Append(", EC.EC_COIL_SIZE");
            selectQuery.Append(", EC.EC_COIL_SHAPE");

            selectQuery.Append(", EC.SHIKII_VALUE AS SHIKII_VALUE");
            selectQuery.Append(", EC.DEAD_LENGTH_A AS DEAD_LENGTH_A");
            selectQuery.Append(", EC.DEAD_LENGTH_B AS DEAD_LENGTH_B");
            selectQuery.Append(", EC.LINE_SPEED AS LINE_SPEED,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN EC.REEC = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Blank") + "'");
            selectQuery.Append(" WHEN EC.REEC  = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS REEC, ");
            selectQuery.Append(" TO_CHAR(EC.EC_END_DATE,'yyyy/mm/dd HH24:MI') AS EC_END_DATE_STRING");
            selectQuery.Append(", EMP2.SHORT_NAME  AS EC_END_OPERATOR_NAME");

            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM1, 'FM990.000') AS EC_ACT_DIM1_STRING");
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM2, 'FM990.000') AS EC_ACT_DIM2_STRING");
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM3, 'FM990.000') AS EC_ACT_DIM3_STRING");
            selectQuery.Append(", TO_CHAR(EC.EC_ACT_DIM4, 'FM990.000') AS EC_ACT_DIM4_STRING");
            selectQuery.Append(", EC.EC_ACT_LENGTH AS EC_ACT_LENGTH");
            selectQuery.Append(", EC.MAGNETISM  AS MAGNETISM ");
            selectQuery.Append(", EC.EC_TABA_NO AS EC_TABA_NO");
            selectQuery.Append(", EC.EC_OK_BARS AS EC_OK_BARS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN EC.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN EC.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN EC.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN EC.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE ");

            selectQuery.Append(", EC.EC_OK_QTY  AS EC_OK_QTY ");
            selectQuery.Append(", EC.EC_NG_QTY  AS EC_NG_QTY ");
            selectQuery.Append(", EC.EC_NG_BARS1 AS EC_NG_BARS1");
            if (cultureName == "en")
            {
                selectQuery.Append(", DEFECT1.OTHER_NAME_SHORT AS EC_NG_DETAILS1_EN");
            }
            else
            {
                selectQuery.Append(", DEFECT1.PROC_DEFECT_SHORT AS EC_NG_DETAILS1_JP ");
            }
            ////selectQuery.Append(", DEFECT1.PROC_DEFECT_SHORT AS EC_NG_DETAILS1_STRING");
            selectQuery.Append(", EC.EC_NG_BARS2 AS EC_NG_BARS2");
            if (cultureName == "en")
            {
                selectQuery.Append(", DEFECT2.OTHER_NAME_SHORT AS EC_NG_DETAILS2_EN");
            }
            else
            {
                selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS EC_NG_DETAILS2_JP ");
            }
            ////selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS EC_NG_DETAILS2_STRING");
            selectQuery.Append(", EC.EC_NG_BARS3 AS EC_NG_BARS3");
            if (cultureName == "en")
            {
                selectQuery.Append(", DEFECT3.OTHER_NAME_SHORT AS EC_NG_DETAILS3_EN");
            }
            else
            {
                selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS EC_NG_DETAILS3_JP ");
            }
            ////selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS EC_NG_DETAILS3_STRING");
            selectQuery.Append(", CASE WHEN EC.EC_NG_BAR_TOTAL > 0 THEN TO_CHAR(ROUND(EC.EC_NG_BAR_TOTAL / (EC.EC_OK_BARS + EC.EC_NG_BAR_TOTAL) * 100,2),'FM9999990.00') ELSE NULL END AS EC_NG_PERCENTAGE ");
            selectQuery.Append(",M.MSG_CONTENTS AS MESSAGE_CONTENTS");
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN EC.EC_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN EC.EC_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS EC_DECISION ");


            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER JOIN TRN_EC EC ON EC.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = EC.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = EC.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON EC.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = EC.EC_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = EC.EC_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = EC.EC_NG_DETAILS3 ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Stampping process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetStamppingSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append("  GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN  ");
            selectQuery.Append(", LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING  ");
            selectQuery.Append(", PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1");
            selectQuery.Append(", PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2");
            selectQuery.Append(", PP.VER AS PP_PROD_VER");
            selectQuery.Append(", CUST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(", RE.SHORT_NAME AS PP_RECEIVER_NAME");
            selectQuery.Append(", EMP.SHORT_NAME AS PP_EMPLOYEE_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            
            selectQuery.Append(",NVL(INTE.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(", PP.PROD_DIM_W AS PP_PROD_DIM_W");
            selectQuery.Append(", PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER");
            selectQuery.Append(", PP.DIM_TOL_MIN AS DIM_TOL_MIN");
            selectQuery.Append(", PP.DIM_TOL_MAX AS DIM_TOL_MAX");
            selectQuery.Append(", PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR");
            selectQuery.Append(", PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(", PP.PROD_LENGTH AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER");
            selectQuery.Append(", PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN");
            selectQuery.Append(", PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX");
            selectQuery.Append(", PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR");
            selectQuery.Append(", PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR");
            selectQuery.Append(", PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL");
            selectQuery.Append(", PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL");
           
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING");

            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");

            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");
            
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_R, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN");

            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS NEJIRE_SPAN,");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN");

            //selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT");

            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");
            selectQuery.Append(", PP.EXT_TEXTURE AS PP_EXT_TEXTURE");

            selectQuery.Append(", PP.COLOR_ID AS PP_COLOR");

            selectQuery.Append(", PP.DISPLAY AS PP_DISPLAY");

            selectQuery.Append(", PP.STAMP AS PP_STAMP ");
            selectQuery.Append(", P1.PROCESS_NAME AS PP_PROCESS_1 ");
            selectQuery.Append(", P2.PROCESS_NAME AS PP_PROCESS_2 ");
            selectQuery.Append(", P3.PROCESS_NAME AS PP_PROCESS_3 ");
            selectQuery.Append(", P4.PROCESS_NAME AS PP_PROCESS_4 ");
            selectQuery.Append(", P5.PROCESS_NAME AS PP_PROCESS_5 ");
            selectQuery.Append(", P6.PROCESS_NAME AS PP_PROCESS_6 ");
            selectQuery.Append(", P7.PROCESS_NAME AS PP_PROCESS_7 ");
            selectQuery.Append(", P8.PROCESS_NAME AS PP_PROCESS_8 ");
            selectQuery.Append(", P9.PROCESS_NAME AS PP_PROCESS_9 ");
            selectQuery.Append(", P10.PROCESS_NAME AS PP_PROCESS_10");
            selectQuery.Append(", P11.PROCESS_NAME AS PP_PROCESS_11");
            selectQuery.Append(", P12.PROCESS_NAME AS PP_PROCESS_12");
            selectQuery.Append(", P13.PROCESS_NAME AS PP_PROCESS_13");

            selectQuery.Append(", PP.TEST_SPECIMEN AS TEST_SPECIMEN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1,");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE ");

            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1");

            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2");

            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3");

            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL ");

            selectQuery.Append(", PP.RANK_STD AS RANK_STD");
            selectQuery.Append(", PP.HT_CONDITION AS PP_HT_CONDITION,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK ");

            selectQuery.Append(", PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(", CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE,");


            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PROCESS_PLAN_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN ");

            selectQuery.Append(", PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT ");

            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");
            //stampping                     
            selectQuery.Append(", MAC.MACHINE_NAME AS ST_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS ST_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( ST.ST_START_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_START_DATE_STRING");
            selectQuery.Append(", EMP1.SHORT_NAME AS ST_START_OPERATOR_NAME");
            selectQuery.Append(", ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE");
            selectQuery.Append(", ST.STAMP_DETAILS AS STAMP_DETAILS,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ST.ST_TRIAL_RESULT = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN ST.ST_TRIAL_RESULT = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ST_TRIAL_RESULT ");

            selectQuery.Append(", TO_CHAR( ST.ST_END_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_END_DATE_STRING");
            selectQuery.Append(", EMP2.SHORT_NAME AS ST_END_OPERATOR_NAME");
            selectQuery.Append(", ST.ST_TABA_NO AS  ST_TABA_NO");
            selectQuery.Append(", ST.ST_OK_BARS As ST_OK_BARS,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ST.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN ST.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN ST.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN ST.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ST_UNIT_TYPE ");


            selectQuery.Append(", ST.ST_OK_QTY AS ST_OK_QTY");
            selectQuery.Append(", ST.ST_NG_QTY AS ST_NG_QTY");
            selectQuery.Append(", ST.ST_NG_BARS1 AS ST_NG_BARS1");
            if (cultureName == "en")
            {
                selectQuery.Append(", DEFECT1.OTHER_NAME_SHORT AS ST_NG_DETAILS1_EN");
            }
            else
            {
                selectQuery.Append(", DEFECT1.PROC_DEFECT_SHORT AS ST_NG_DETAILS1_JP ");
            }
            //selectQuery.Append(", DEFECT1.PROC_DEFECT_SHORT AS ST_NG_DETAILS1_STRING");
            selectQuery.Append(", ST.ST_NG_BARS2 AS ST_NG_BARS2");
            if (cultureName == "en")
            {
                selectQuery.Append(", DEFECT2.OTHER_NAME_SHORT AS ST_NG_DETAILS2_EN");
            }
            else
            {
                selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS ST_NG_DETAILS2_JP ");
            }
            //selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS ST_NG_DETAILS2_STRING");
            selectQuery.Append(", ST.ST_NG_BARS3 AS ST_NG_BARS3");
            if (cultureName == "en")
            {
                selectQuery.Append(", DEFECT3.OTHER_NAME_SHORT AS ST_NG_DETAILS3_EN");
            }
            else
            {
                selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS ST_NG_DETAILS3_JP ");
            }
            //selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS ST_NG_DETAILS3_STRING");

            selectQuery.Append(", CASE ");
            selectQuery.Append(" WHEN ST.ST_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN ST.ST_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ST_DECISION, ");
            selectQuery.Append(" M.MSG_CONTENTS AS ST_REMARKS ");



            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" LEFT  JOIN TRN_STAMPING ST ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
           
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = ST.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON ST.MACHINE_CD = MAC.MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_CONDITION  CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = ST.ST_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = ST.ST_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = ST.ST_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = ST.ST_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = ST.ST_NG_DETAILS3 ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD ");
            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Hardness Tempering process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetHardnessTemperingSQLForDailyProcess(List<SearchField> searchCriteria, string sortCriteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_COIL_DIA_WINDING,");

            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION, ");

            
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS NEJIRE_SPAN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE , ");

            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");


            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_10, ");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_11, ");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_12, ");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_ANOTHER_NAME_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS TEST_SPECIMEN , ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");
            
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");
            
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");
          
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE , ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PROCESS_PLAN_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS PP_NO_OF_UNITS, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PROCESS_PLAN_PRINT_DATE_STRING, ");
            //HardnessTempering

            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");
            selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" MACH.MACHINE_NAME AS HARDENING_MACHINE_NAME, ");
            selectQuery.Append(" MACT.MACHINE_NAME AS TEMPERING_MACHINE_NAME, ");
            selectQuery.Append(" HT.METALLIC_PLATE_CODE AS METALLIC_PLATE_CODE, ");
            selectQuery.Append(" HT.NO_OF_UNITS AS  NO_OF_UNITS, ");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN HT.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");

            selectQuery.Append(" HT.WEIGHT AS WEIGHT, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN HT.WATER_OIL_TYPE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_Water") + "'");
            selectQuery.Append(" WHEN HT.WATER_OIL_TYPE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_LowUQE") + "'");
            selectQuery.Append(" WHEN HT.WATER_OIL_TYPE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_MiddleUQE") + "'");
            selectQuery.Append(" WHEN HT.WATER_OIL_TYPE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Kubun_HighUQE") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS WATER_OIL_TYPE, ");

            selectQuery.Append(" HT.TEMPERATURE_FURNACE AS TEMPERATURE_FURNACE, ");
            selectQuery.Append(" HT.CONCENTRATION_FURNACE AS CONCENTRATION_FURNACE, ");
            selectQuery.Append(" HT.FREQUENCY_FURNACE AS FREQUENCY_FURNACE, ");
            selectQuery.Append(" HT.C_FURNACE AS C_FURNACE, ");
            selectQuery.Append(" HT.MN_FURNACE AS MN_FURNACE, ");
            selectQuery.Append(" HT.CR_FURNACE AS CR_FURNACE, ");
            selectQuery.Append(" HT.MO_FURNACE AS MO_FURNACE, ");
            
            selectQuery.Append(" HT.HARDNESS_MIN AS HARDNESS_MIN, ");
            selectQuery.Append(" HT.HARDNESS_MAX AS HARDNESS_MAX, ");
            selectQuery.Append(" EMPH.SHORT_NAME AS HARDENING_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(HT.START_DATE_HARDENING, 'yyyy/mm/dd HH24:MI') AS START_DATE_HARDENING_STRING, ");
            selectQuery.Append(" TO_CHAR(HT.END_DATE_HARDENING, 'yyyy/mm/dd HH24:MI') AS  END_DATE_HARDENING_STRING, ");
            selectQuery.Append(" HT.TEMPERATURE_HARDENING AS TEMPERATURE_HARDENING, ");
            selectQuery.Append(" HT.TIME_HARDENING AS TIME_HARDENING, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_UP_1 AS Q_HARDNESS_NORTH_UP_1, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_UP_2 AS Q_HARDNESS_NORTH_UP_2, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_UP_3 AS Q_HARDNESS_NORTH_UP_3, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_UP_CEN AS Q_HARDNESS_NORTH_UP_CEN, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_DOWN_1 AS Q_HARDNESS_NORTH_DOWN_1, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_DOWN_2 AS Q_HARDNESS_NORTH_DOWN_2, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_DOWN_3 AS Q_HARDNESS_NORTH_DOWN_3, ");
            selectQuery.Append(" HT.Q_HARDNESS_NORTH_DOWN_CEN AS Q_HARDNESS_NORTH_DOWN_CEN, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_1 AS  Q_HARDNESS_SOUTH_UP_1, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_2 AS Q_HARDNESS_SOUTH_UP_2, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_3 AS Q_HARDNESS_SOUTH_UP_3, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_CEN AS Q_HARDNESS_SOUTH_UP_CEN, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_1 AS Q_HARDNESS_SOUTH_DOWN_1, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_2 AS Q_HARDNESS_SOUTH_DOWN_2, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_3 AS Q_HARDNESS_SOUTH_DOWN_3, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_CEN AS Q_HARDNESS_SOUTH_DOWN_CEN, ");
            selectQuery.Append(" EMPT.SHORT_NAME AS TEMPERING_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(HT.START_DATE_TEMPERING, 'yyyy/mm/dd HH24:MI') AS START_DATE_TEMPERING_STRING, ");
            selectQuery.Append(" TO_CHAR(HT.END_DATE_TEMPERING, 'yyyy/mm/dd HH24:MI') AS END_DATE_TEMPERING_STRING, ");
            selectQuery.Append(" HT.TEMPERATURE_TEMPERING AS TEMPERATURE_TEMPERING, ");
            selectQuery.Append(" HT.TIME_TEMPERING AS TIME_TEMPERING, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_UP_1 AS T_HARDNESS_NORTH_UP_1, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_UP_2 AS T_HARDNESS_NORTH_UP_2, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_UP_3 AS T_HARDNESS_NORTH_UP_3, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_UP_CENT AS T_HARDNESS_NORTH_UP_CENT, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_DOWN_1 AS T_HARDNESS_NORTH_DOWN_1, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_DOWN_2 AS T_HARDNESS_NORTH_DOWN_2, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_DOWN_3 AS T_HARDNESS_NORTH_DOWN_3, ");
            selectQuery.Append(" HT.T_HARDNESS_NORTH_DOWN_CEN AS T_HARDNESS_NORTH_DOWN_CEN, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_UP_1 AS T_HARDNESS_SOUTH_UP_1, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_UP_2 AS T_HARDNESS_SOUTH_UP_2, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_UP_3 AS T_HARDNESS_SOUTH_UP_3, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_UP_CENT AS T_HARDNESS_SOUTH_UP_CENT, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_DOWN_1 AS T_HARDNESS_SOUTH_DOWN_1, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_DOWN_2 AS T_HARDNESS_SOUTH_DOWN_2, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_DOWN_3 AS T_HARDNESS_SOUTH_DOWN_3, ");
            selectQuery.Append(" HT.T_HARDNESS_SOUTH_DOWN_CEN AS T_HARDNESS_SOUTH_DOWN_CEN, ");
            //HT_Quality

            selectQuery.Append(" TO_CHAR(HTQ.INSPECTION_DATE,'yyyy/mm/dd') AS  INSPECTION_DATE_STRING,");
            selectQuery.Append(" EMPL.EMPLOYEE_NAME  AS INSPECTED_OPERATOR_NAME,");
            selectQuery.Append(" HTQ.DM_F_MIN  AS DM_F_MIN,");
            selectQuery.Append(" HTQ.DM_F_MAX AS DM_F_MAX,");
            selectQuery.Append(" HTQ.DM_T_MIN  AS DM_T_MIN,");
            selectQuery.Append(" HTQ.DM_T_MAX AS DM_T_MAX,");
            selectQuery.Append(" HTQ.STRUCTURE_OUT AS STRUCTURE_OUT,");
            selectQuery.Append(" HTQ.STRUCTURE_IN AS STRUCTURE_IN,");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN HTQ.DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN HTQ.DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS DECISION, ");
            selectQuery.Append(" NVL(M1.MSG_CONTENTS, M.MSG_CONTENTS) AS REMARK");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HARDENING_TEMPERING HT ");
            selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD AND HTQ.FURNACE_NO=HT.FURNACE_NO");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
           
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.process_1_cd ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD  ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = HT.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MACH ON MACH.MACHINE_CD = HT.HARDENING_MACHINE_CD ");
            selectQuery.Append("  LEFT JOIN MST_MACHINE MACT ON MACT.MACHINE_CD = HT.TEMPERING_MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMPH ON EMPH.EMPLOYEE_CD = HT.OPERATOR_CD_HARDENING ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPT ON EMPT.EMPLOYEE_CD = HT.OPERATOR_CD_TEMPERING ");

            //selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = HTQ.INSPECTED_BY_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = HT.MSG_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M1 ON M1.MSG_CD = HTQ.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = query + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of QA Test Hardness process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetQATestHardnessSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_COIL_DIA_WINDING,");

            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");

            
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

           
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PP_MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS PP_MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE, ");

            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");

            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" PROCESS1.PROCESS_NAME  AS PP_PROCESS_1, ");
            selectQuery.Append(" PROCESS2.PROCESS_NAME  AS PP_PROCESS_2, ");
            selectQuery.Append(" PROCESS3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" PROCESS4.PROCESS_NAME  AS PP_PROCESS_4, ");
            selectQuery.Append(" PROCESS5.PROCESS_NAME  AS PP_PROCESS_5, ");
            selectQuery.Append(" PROCESS6.PROCESS_NAME  AS PP_PROCESS_6, ");
            selectQuery.Append(" PROCESS7.PROCESS_NAME  AS PP_PROCESS_7, ");
            selectQuery.Append(" PROCESS8.PROCESS_NAME  AS PP_PROCESS_8, ");
            selectQuery.Append(" PROCESS9.PROCESS_NAME  AS PP_PROCESS_9, ");
            selectQuery.Append(" PROCESS10.PROCESS_NAME  AS PP_PROCESS_10, ");
            selectQuery.Append(" PROCESS11.PROCESS_NAME  AS PP_PROCESS_11, ");
            selectQuery.Append(" PROCESS12.PROCESS_NAME  AS PP_PROCESS_12, ");
            selectQuery.Append(" PROCESS13.PROCESS_NAME  AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");

            
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");

            
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");

           
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");

           
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");

          
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");

            
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");

            selectQuery.Append(" MAC.MACHINE_NAME AS IQ_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS INSPECTION_LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(IQ.INT_QLTY_TEST_DATE, 'yyyy/mm/dd') AS INT_QLTY_TEST_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS IQ_START_OPERATOR_NAME, ");
            selectQuery.Append(" IQ.HT_CONDITION AS HT_CONDITION, ");
            selectQuery.Append(" IQ.PATTERN_NO AS PATTERN_NO, ");
            selectQuery.Append(" IQ.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" IQ.LOCATION_IN_FURNACE1 AS LOCATION_IN_FURNACE1, ");
            selectQuery.Append(" IQ.LOCATION_IN_FURNACE2 AS LOCATION_IN_FURNACE2, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 0 ");
            selectQuery.Append(" THEN '' ");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Above") + "'");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Medium") + "'");
            selectQuery.Append(" WHEN IQ.STACK_POSITION = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Below") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS STACK_POSITION, ");

            selectQuery.Append(" IQ.ACT_DM_F AS ACT_DM_F, ");
            selectQuery.Append(" IQ.ACT_DM_T_MIN AS ACT_DM_T_MIN, ");
            selectQuery.Append(" IQ.ACT_DM_T_MAX AS ACT_DM_T_MAX, ");
            selectQuery.Append(" IQ.ACT_DM_T_AVG AS ACT_DM_T_AVG, ");
            selectQuery.Append(" IQ.ACT_STRUCTURE AS ACT_STRUCTURE, ");
            selectQuery.Append(" IQ.HRD_CENTER AS HRD_CENTER, ");
            selectQuery.Append(" IQ.HRD_D4_1 AS HRD_D4_1, ");
            selectQuery.Append(" IQ.HRD_D4_2 AS HRD_D4_2, ");
            selectQuery.Append(" IQ.HRD_D4_3 AS HRD_D4_3, ");
            selectQuery.Append(" IQ.HRD_D4_4 AS HRD_D4_4, ");
            selectQuery.Append(" IQ.HRD_D4_AVG AS HRD_D4_AVG, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN IQ.INTERNAL_QUALITY_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS INTERNAL_QUALITY_DECISION, ");
            selectQuery.Append(" M.MSG_CONTENTS AS INT_QLTY_REMARKS ");


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_INTERNAL_QUALITY IQ ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = IQ.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
           
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");

            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = IQ.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = IQ.INT_QLTY_TEST_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS1 ON PP.PROCESS_1_CD = PROCESS1.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS2 ON PP.PROCESS_2_CD = PROCESS2.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS3 ON PP.PROCESS_3_CD = PROCESS3.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS4 ON PP.PROCESS_4_CD = PROCESS4.PROCESS_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS5 ON PP.PROCESS_5_CD = PROCESS5.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS6 ON PP.PROCESS_6_CD = PROCESS6.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS7 ON PP.PROCESS_7_CD = PROCESS7.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS8 ON PP.PROCESS_8_CD = PROCESS8.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS9 ON PP.PROCESS_9_CD = PROCESS9.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS10 ON PP.PROCESS_10_CD = PROCESS10.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS11 ON PP.PROCESS_11_CD = PROCESS11.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS12 ON PP.PROCESS_12_CD = PROCESS12.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS13 ON PP.PROCESS_13_CD = PROCESS13.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = IQ.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = IQ.MACHINE_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Mech Hardness process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetMechHardnessSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VER,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
           
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
         
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");

            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
           
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

         
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

         
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PR_MAGARI_BEND_TOL,");
            selectQuery.Append(" PP.MAGARI_SPAN,");
            selectQuery.Append(" PP.NEJIRE_ANGLE,");
            selectQuery.Append(" PP.NEJIRE_SPAN,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN,");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX,");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE,");
            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR,");

            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY,");
            selectQuery.Append(" PP.STAMP AS PP_STAMP,");
            selectQuery.Append(" PROCESS1.PROCESS_NAME  AS PP_PROCESS_1, ");
            selectQuery.Append(" PROCESS2.PROCESS_NAME  AS PP_PROCESS_2, ");
            selectQuery.Append(" PROCESS3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" PROCESS4.PROCESS_NAME  AS PP_PROCESS_4, ");
            selectQuery.Append(" PROCESS5.PROCESS_NAME  AS PP_PROCESS_5, ");
            selectQuery.Append(" PROCESS6.PROCESS_NAME  AS PP_PROCESS_6, ");
            selectQuery.Append(" PROCESS7.PROCESS_NAME  AS PP_PROCESS_7, ");
            selectQuery.Append(" PROCESS8.PROCESS_NAME  AS PP_PROCESS_8, ");
            selectQuery.Append(" PROCESS9.PROCESS_NAME  AS PP_PROCESS_9, ");
            selectQuery.Append(" PROCESS10.PROCESS_NAME  AS PP_PROCESS_10, ");
            selectQuery.Append(" PROCESS11.PROCESS_NAME  AS PP_PROCESS_11, ");
            selectQuery.Append(" PROCESS12.PROCESS_NAME  AS PP_PROCESS_12, ");
            selectQuery.Append(" PROCESS13.PROCESS_NAME  AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX,");
          
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX,");

          
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX,");
          
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1,");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX,");
          
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX,");
          
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");


            selectQuery.Append(" PP.RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR,");

            selectQuery.Append(" MIHARD.DETAIL_NO,");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(MIHARD.HD_TEST_DATE, 'yyyy/mm/dd HH24:MI')  AS HD_TEST_DATE_STRING,");
            selectQuery.Append(" EMP1.SHORT_NAME AS OPERATOR_NAME,");
            selectQuery.Append(" MIHARD.HD_FURNACE_NO,");
            selectQuery.Append(" MIHARD.HD_ACT_DIM,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_PATTERN_ACT = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HD_PATTERN_ACT, ");

            selectQuery.Append(" MIHARD.HD_SUTEOSHI,");
            selectQuery.Append(" MIHARD.HD_D4_1,");
            selectQuery.Append(" MIHARD.HD_D4_2,");
            selectQuery.Append(" MIHARD.HD_D4_3,");
            selectQuery.Append(" MIHARD.HD_D4_4,");
            selectQuery.Append(" MIHARD.HD_D4_AVG,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MIHARD.IS_INCLUDE_CENTER_AVG = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NotIncluded") + "'");
            selectQuery.Append(" WHEN MIHARD.IS_INCLUDE_CENTER_AVG = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Included") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS IS_INCLUDE_CENTER_AVG, ");


           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN MIHARD.HD_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HD_DECISION, ");
            selectQuery.Append(" MESS.MSG_CONTENTS AS REMARKS ");


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MI_HARDNESS MIHARD INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = MIHARD.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=MIHARD.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = MIHARD.HD_TEST_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS1 ON PP.PROCESS_1_CD = PROCESS1.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS2 ON PP.PROCESS_2_CD = PROCESS2.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS3 ON PP.PROCESS_3_CD = PROCESS3.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS4 ON PP.PROCESS_4_CD = PROCESS4.PROCESS_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS5 ON PP.PROCESS_5_CD = PROCESS5.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS6 ON PP.PROCESS_6_CD = PROCESS6.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS7 ON PP.PROCESS_7_CD = PROCESS7.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS8 ON PP.PROCESS_8_CD = PROCESS8.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS9 ON PP.PROCESS_9_CD = PROCESS9.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS10 ON PP.PROCESS_10_CD = PROCESS10.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS11 ON PP.PROCESS_11_CD = PROCESS11.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS12 ON PP.PROCESS_12_CD = PROCESS12.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS13 ON PP.PROCESS_13_CD = PROCESS13.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = MIHARD.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Mech Tensile process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetMechTensileSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VER,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");

            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
           
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

           
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PR_MAGARI_BEND_TOL,");
            selectQuery.Append(" PP.MAGARI_SPAN,");
            selectQuery.Append(" PP.NEJIRE_ANGLE,");
            selectQuery.Append(" PP.NEJIRE_SPAN,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN,");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX,");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE,");
            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR,");


            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY,");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" PROCESS1.PROCESS_NAME  AS PP_PROCESS_1, ");
            selectQuery.Append(" PROCESS2.PROCESS_NAME  AS PP_PROCESS_2, ");
            selectQuery.Append(" PROCESS3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" PROCESS4.PROCESS_NAME  AS PP_PROCESS_4, ");
            selectQuery.Append(" PROCESS5.PROCESS_NAME  AS PP_PROCESS_5, ");
            selectQuery.Append(" PROCESS6.PROCESS_NAME  AS PP_PROCESS_6, ");
            selectQuery.Append(" PROCESS7.PROCESS_NAME  AS PP_PROCESS_7, ");
            selectQuery.Append(" PROCESS8.PROCESS_NAME  AS PP_PROCESS_8, ");
            selectQuery.Append(" PROCESS9.PROCESS_NAME  AS PP_PROCESS_9, ");
            selectQuery.Append(" PROCESS10.PROCESS_NAME  AS PP_PROCESS_10, ");
            selectQuery.Append(" PROCESS11.PROCESS_NAME  AS PP_PROCESS_11, ");
            selectQuery.Append(" PROCESS12.PROCESS_NAME  AS PP_PROCESS_12, ");
            selectQuery.Append(" PROCESS13.PROCESS_NAME  AS PP_PROCESS_13, ");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");

            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR,");


            selectQuery.Append(" MITENS.DETAIL_NO,");
            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(MITENS.TS_TEST_DATE, 'yyyy/mm/dd HH24:MI') AS TS_TEST_DATE_STRING,");
            selectQuery.Append(" EMP1.SHORT_NAME AS OPERATOR_NAME,");
            selectQuery.Append(" MITENS.TS_FURNACE_NO,");
          
            selectQuery.Append(" MJS.JIS_TEST_SPEC_SHORT, ");
            selectQuery.Append(" MITENS.TS_ACT_DIM,");
            //selectQuery.Append(" MITENS.TS_ACT_DIM_SHAPE,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MITENS.TS_ACT_DIM_SHAPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_R") + "'");
            selectQuery.Append(" WHEN MITENS.TS_ACT_DIM_SHAPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun2Of4") + "'");
            selectQuery.Append(" WHEN MITENS.TS_ACT_DIM_SHAPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun3Of4") + "'");
            selectQuery.Append(" WHEN MITENS.TS_ACT_DIM_SHAPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun4Of4") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TS_ACT_DIM_SHAPE, ");

            selectQuery.Append(" MITENS.TS_TEST_SPECIMEN_DIM,");
            selectQuery.Append(" MITENS.TS_GAUGE_LENGTH,");
            selectQuery.Append(" MITENS.TS_MAX_LOAD_N,");
            selectQuery.Append(" MITENS.TS_YIELD_LOAD_N,");
            selectQuery.Append(" MITENS.TS_EXTENDED_LENGTH,");
            selectQuery.Append(" MITENS.TS_CUT_DIM,");
            selectQuery.Append(" MITENS.TS_TEST_IMPACT_1,");
            selectQuery.Append(" MITENS.TS_TEST_IMPACT_2,");
            selectQuery.Append(" MITENS.TS_TEST_IMPACT_3,");
            selectQuery.Append(" MITENS.TS_TENSILE,");
            selectQuery.Append(" MITENS.TS_YIELD_STRENGTH AS TS_YIELD_STRENGTH_SHORT,");
            selectQuery.Append(" MITENS.TS_ELONGATION,");
            selectQuery.Append(" MITENS.TS_REDUCTION,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN MITENS.TS_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TS_DECISION, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS REMARKS ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MI_TENSILE_STRENGTH   MITENS INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = MITENS.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");

            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=MITENS.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = MITENS.TS_TEST_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS1 ON PP.PROCESS_1_CD = PROCESS1.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS2 ON PP.PROCESS_2_CD = PROCESS2.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS3 ON PP.PROCESS_3_CD = PROCESS3.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS4 ON PP.PROCESS_4_CD = PROCESS4.PROCESS_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS5 ON PP.PROCESS_5_CD = PROCESS5.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS6 ON PP.PROCESS_6_CD = PROCESS6.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS7 ON PP.PROCESS_7_CD = PROCESS7.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS8 ON PP.PROCESS_8_CD = PROCESS8.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS9 ON PP.PROCESS_9_CD = PROCESS9.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS10 ON PP.PROCESS_10_CD = PROCESS10.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS11 ON PP.PROCESS_11_CD = PROCESS11.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS12 ON PP.PROCESS_12_CD = PROCESS12.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS PROCESS13 ON PP.PROCESS_13_CD = PROCESS13.PROCESS_CD ");
            selectQuery.Append(" LEFT JOIN MST_JIS_TEST_SPEC MJS ON MJS.JIS_TEST_SPEC_CD = MITENS.TEST_SPECIMEN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.MSG_CD = MITENS.MSG_CD ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }


        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of Final Inspection process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetFinalInspectionSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN,");
            selectQuery.Append(" LPAD(PP.GYOUBAN , 2, '0') AS PP_GYOUBAN_STRING,");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1,");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2,");
            selectQuery.Append(" PP.VER AS PP_PROD_VER,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME,");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST,");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PROD_DIM_SHAPE,");
            selectQuery.Append(" PP.PROD_DIM_D AS DIM_DD,");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W,");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN,");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX,");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN,");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX,");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
           
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL,");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_COIL_DIA_WINDING,");

            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION,");
            
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

            
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS CORNER_RADIUS, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS PR_MAGARI_BEND_TOL,");
            selectQuery.Append(" PP.MAGARI_SPAN,");
            selectQuery.Append(" PP.NEJIRE_ANGLE,");
            selectQuery.Append(" PP.NEJIRE_SPAN,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            //selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN,");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX,");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE,");
            selectQuery.Append(" PP.COLOR_ID AS COLOR,");

            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY,");

            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10, ");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11, ");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12, ");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13, ");

            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN,");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX,");
            
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD,");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK,");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE,");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W,");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER,");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH,");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");


            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY,");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING,");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR,");


            selectQuery.Append(" LOCA.SHORT_NAME AS PP_LOCATION_NAME,");
            selectQuery.Append(" TO_CHAR(FI.INSP_DATE, 'yyyy/mm/dd HH24:MI') AS INSPECTED_DATE_STRING,");
            selectQuery.Append(" EMP2.SHORT_NAME AS OPERATOR_NAME, ");
            selectQuery.Append(" NVL(NIFUDA.NIFUDA_NO,FI.FI_NIFUDA_NO) AS FI_NIFUDA_NO,");
            selectQuery.Append(" FI.FI_TABA_NO,");
            //selectQuery.Append(" FI.FI_DIM1 AS FI_DIM_1,");
            //selectQuery.Append(" FI.FI_DIM2 AS FI_DIM_2,");
            //selectQuery.Append(" FI.FI_DIM3,");
            //selectQuery.Append(" FI.FI_DIM4,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM1, 'FM990.000') AS FI_DIM1_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM2, 'FM990.000') AS FI_DIM2_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM3, 'FM990.000') AS FI_DIM3_STRING,");
            selectQuery.Append(" TO_CHAR(FI.FI_DIM4, 'FM990.000') AS FI_DIM4_STRING,");

            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_DIM_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_DIM_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_DIM_DECISION, ");

            selectQuery.Append(" FI.FI_LENGTH,");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_LENGTH_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_LENGTH_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_LENGTH_DECISION, ");

            selectQuery.Append(" FI.FI_ACT_RADIUS_1,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_2,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_3,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_4,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_5,");
            selectQuery.Append(" FI.FI_ACT_RADIUS_6,");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_CORNER_RADIUS_STD_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_CORNER_RADIUS_STD_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_CORNER_RADIUS_STD_DECISION, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_COLOR_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_COLOR_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_COLOR_DECISION, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_CHAMFERING_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_CHAMFERING_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_CHAMFERING_DECISION, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_EXT_TEXTURE_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_EXT_TEXTURE_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_EXT_TEXTURE_DECISION, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.FI_SPARK_DECISION = 0 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN FI.FI_SPARK_DECISION = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.FI_SPARK_DECISION = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS FI_SPARK_DECISION, ");


            selectQuery.Append(" EMP3.SHORT_NAME AS SPARK_INSP_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(FI.SPARK_INSP_DATE,'yyyy/mm/dd HH24:MI') AS SPARK_INSP_DATE_STRING,");
            selectQuery.Append(" FI.NO_OF_BARS AS FI_NO_OF_BARS,");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN FI.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN FI.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN FI.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE_STRING, ");
            selectQuery.Append(" FI.FI_QTY AS FI_QTY_1,");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN FI.COMBINED_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN FI.COMBINED_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN FI.COMBINED_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS COMBINED_DECISION, ");
            selectQuery.Append(" MESS.MSG_CONTENTS AS FI_REMARK ");


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_FINAL_INSPECTION FI INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOCA ON LOCA.LOCATION_CD=FI.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9  ON P9.PROCESS_CD = PP.PROCESS_9_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=FI.INSP_BY ");
           
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP3 ON EMP3.EMPLOYEE_CD=FI.SPARK_INSP_BY ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MESS ON MESS.MSG_CD = FI.MSG_CD ");
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NIFUDA ON NIFUDA.NIFUDA_CD=FI.NIFUDA_CD ");


            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade        
        /// used screens:Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// SQL of General Process data according to search criteria to download CSV      
        /// </returns>
        /// 
        public string GetGeneralDailyProcessSQLForDailyProcess(List<SearchField> searchCriteria, string sortCiteria, string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();

            
            selectQuery.Append(" SELECT ");

            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, ('')) AS PP_CHUUBAN, ");
            selectQuery.Append(" LPAD(PP.GYOUBAN, 2, '0') AS PP_GYOUBAN_STRING, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1 AS PP_PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2 AS PP_PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.VER AS PP_VER, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" REC.SHORT_NAME AS PP_RECEIVER_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS PP_EMPLOYEE_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
           
            selectQuery.Append(" NVL(INTERN.INTERNAL_NAME, PP.MATL_NAME_MAK) AS PP_INTERNAL_NAME,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            selectQuery.Append(" PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            selectQuery.Append(" PP.DIM_TOL_MIN AS PP_DIM_TOL_MIN, ");
            selectQuery.Append(" PP.DIM_TOL_MAX AS PP_DIM_TOL_MAX, ");
            selectQuery.Append(" PP.DIM_TOL_MIN_CHR AS PP_DIM_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR AS PP_DIM_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN AS PP_LENGTH_TOL_MIN, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX AS PP_LENGTH_TOL_MAX, ");
            selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR AS PP_LENGTH_TOL_MIN_CHR, ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR, ");
            selectQuery.Append(" PP.COIL_DIA_INTERNAL AS PP_COIL_DIA_INTERNAL, ");
            selectQuery.Append(" PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections1of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_CoilWindDirections2of3") + "'");
            selectQuery.Append(" WHEN PP.COIL_DIA_WINDING = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Other") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS COIL_DIA_WINDING,");

            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION,");
          
            selectQuery.Append(" CASE WHEN PP.CHAMFER IS NOT NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CHAMFER, ");

           
            selectQuery.Append(" CASE WHEN PP.CORNER_RADIUS IS NOT NULL THEN ");
            selectQuery.Append(" PP.CORNER_RADIUS || 'R' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_CORNER_RADIUS, ");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "'");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS MAGARI_KUBUN,");

            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnitWt") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("rdb_UnitWtRange_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bunch") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PP_UNIT_WT_KUBUN,");

            // selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT_WT_KUBUN = 3 and  PP.UNIT_WT > 0 ");
            selectQuery.Append(" THEN TO_CHAR(PP.UNIT_WT) || '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" ELSE TO_CHAR(PP.UNIT_WT) ");
            selectQuery.Append(" END AS PP_UNIT_WT,");

            selectQuery.Append(" PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN, ");
            selectQuery.Append(" PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX, ");
            selectQuery.Append(" PP.EXT_TEXTURE AS PP_EXT_TEXTURE, ");
            selectQuery.Append(" PP.COLOR_ID AS PP_COLOR, ");


            selectQuery.Append(" PP.DISPLAY AS PP_DISPLAY, ");
            selectQuery.Append(" PP.STAMP AS PP_STAMP, ");
            selectQuery.Append(" P1.PROCESS_NAME AS PP_PROCESS_1, ");
            selectQuery.Append(" P2.PROCESS_NAME AS PP_PROCESS_2, ");
            selectQuery.Append(" P3.PROCESS_NAME AS PP_PROCESS_3, ");
            selectQuery.Append(" P4.PROCESS_NAME AS PP_PROCESS_4, ");
            selectQuery.Append(" P5.PROCESS_NAME AS PP_PROCESS_5, ");
            selectQuery.Append(" P6.PROCESS_NAME AS PP_PROCESS_6, ");
            selectQuery.Append(" P7.PROCESS_NAME AS PP_PROCESS_7, ");
            selectQuery.Append(" P8.PROCESS_NAME AS PP_PROCESS_8, ");
            selectQuery.Append(" P9.PROCESS_NAME AS PP_PROCESS_9, ");
            selectQuery.Append(" P10.PROCESS_NAME AS PP_PROCESS_10,");
            selectQuery.Append(" P11.PROCESS_NAME AS PP_PROCESS_11,");
            selectQuery.Append(" P12.PROCESS_NAME AS PP_PROCESS_12,");
            selectQuery.Append(" P13.PROCESS_NAME AS PP_PROCESS_13,");
            selectQuery.Append(" PP.TEST_SPECIMEN AS PP_TEST_SPECIMEN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.TENSILE_STRN_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.TENSILE_STRN_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS TENSILE_STRN_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");
          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.ELONGATION_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS ELONGATION_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.ELONGATION_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS ELONGATION_PATTERN3_ACTUAL, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS HARDNESS_PATTERN1_MIN_MAX,");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun1of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun2of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun3of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun4of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun5of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun6of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StandardHardnessKubun7of7") + "'");
            selectQuery.Append(" WHEN PP.HARDNESS_PATTERN2_CODE = 99 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN2_CODE, ");

            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");
           
            selectQuery.Append(" CASE WHEN PP.HARDNESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS HARDNESS_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.YEILD_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS YEILD_PATTERN1_MIN_MAX,");


            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");
          
            selectQuery.Append(" CASE WHEN PP.YEILD_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS YEILD_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PRESS_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS PRESS_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");
          
            selectQuery.Append(" CASE WHEN PP.PRESS_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PRESS_PATTERN3_ACTUAL, ");

          
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_1,");

            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_2,");

            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.DECARB_3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMF") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMS") + "'");
            selectQuery.Append(" WHEN PP.DECARB_3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecarbDMT") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS DECARB_3,");

            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS PP_IMPACT_TEST_PATTERN1, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.IMPACT_TEST_PATTERN1_MIN_MAX = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_HrdMaxMinKubun2of2") + "'");
            selectQuery.Append(" ELSE");
            selectQuery.Append("''");
            selectQuery.Append(" END AS IMPACT_TEST_PATTERN1_MIN_MAX,");

            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS PP_IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS PP_IMPACT_TEST_PATTERN2_MAX, ");
           
            selectQuery.Append(" CASE WHEN PP.IMPACT_TEST_PATTERN3_ACTUAL IS NOT NULL THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_ActualPattern") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");

            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Bar") + "'");
            selectQuery.Append(" WHEN PP.MATL_TYPE_CBK = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Kurokawa") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_MATL_TYPE_CBK, ");

            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_General") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Reprocess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lblProcessKubunAddedProcess") + "'");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Cutting") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROCESS_PLAN_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun1of2") + "'");
            selectQuery.Append(" WHEN PP.PROD_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProductionManagementKubun2of2") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_PROD_MNGMNT_KUBUN, ");

           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun1of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun2of3") + "'");
            selectQuery.Append(" WHEN PP.SPCL_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_SpecialManagementKubun3of3") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_SPCL_MNGMNT_KUBUN, ");

            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun1of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun2of5") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_QuickDispatch") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_PlannedStock") + "'");
            selectQuery.Append(" WHEN PP.STORAGE_MNGMNT_KUBUN = 50 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_StorageManagementKubun5of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_STORAGE_MNGMNT_KUBUN, ");


            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.UNIT = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN PP.UNIT = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS PP_UNIT, ");

            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'YYYY/MM/DD') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");


            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");
            }
            else
            {
                selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            }

            selectQuery.Append(" MAC.MACHINE_NAME AS GP_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS GP_LOCATION_NAME, ");
            selectQuery.Append(" TO_CHAR(GP.GP_START_DATE, 'YYYY/MM/DD HH24:MI') AS GP_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS GP_START_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(GP.GP_END_DATE, 'YYYY/MM/DD HH24:MI') AS GP_END_DATE_STRING, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS GP_END_OPERATOR_NAME, ");
            selectQuery.Append(" GP.GP_OK_UNITS AS GP_OK_UNITS, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN GP.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN GP.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN GP.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");

            selectQuery.Append(" GP.GP_QTY AS GP_QTY, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS PROC_DEFECT1_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT1_JP, ");
            }
            //selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS PROC_DEFECT_STRING1, ");
            selectQuery.Append(" GP.GP_NG_UNITS1, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE1 = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS GP_NG_UNIT_TYPE1, ");

            selectQuery.Append(" GP.GP_NG_QTY1, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS PROC_DEFECT2_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT2_JP, ");
            }
            //selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS PROC_DEFECT_STRING2, ");
            selectQuery.Append(" GP.GP_NG_UNITS2, ");
            
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE2 = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS GP_NG_UNIT_TYPE2, ");

            selectQuery.Append(" GP.GP_NG_QTY2, ");
            if (cultureName == "en")
            {
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS PROC_DEFECT3_EN,");
            }
            else
            {
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT3_JP, ");
            }
            //selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS PROC_DEFECT_STRING3, ");
            selectQuery.Append(" GP.GP_NG_UNITS3, ");
           
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
            selectQuery.Append(" WHEN GP.GP_NG_UNIT_TYPE3 = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS GP_NG_UNIT_TYPE3, ");

            selectQuery.Append(" GP.GP_NG_QTY3, ");
            selectQuery.Append(" GP.GP_TABA_NO_1,");
            selectQuery.Append(" GP.GP_TABA_NO_2,");
            selectQuery.Append(" GP.GP_TABA_NO_3,");
            selectQuery.Append(" GP.GP_TABA_NO_4,");
            selectQuery.Append(" GP.DAILY_PROCESS_NOTE, ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN GP.GP_DECISION = 10 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 20 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 30 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 35 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 40 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 60 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 65 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 70 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 73 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 76 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 79 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 80 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
            selectQuery.Append(" WHEN GP.GP_DECISION = 90 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END AS GP_DECISION,");
           

            if (cultureName == "en")
            {
                selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN");
            }
            else
            {
                selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP ");
            }


            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_GENERAL_DAILY_PROCESS GP ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = GP.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
           
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION ON CONDITION.CONDITION_CD = PP.PROD_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.MATL_CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = GP.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = GP.LOCATION_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD = GP.GP_START_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD = GP.GP_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P1 ON P1.PROCESS_CD = PP.PROCESS_1_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P2 ON P2.PROCESS_CD = PP.PROCESS_2_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P3 ON P3.PROCESS_CD = PP.PROCESS_3_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P4 ON P4.PROCESS_CD = PP.PROCESS_4_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P5 ON P5.PROCESS_CD = PP.PROCESS_5_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P6 ON P6.PROCESS_CD = PP.PROCESS_6_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P7 ON P7.PROCESS_CD = PP.PROCESS_7_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P8 ON P8.PROCESS_CD = PP.PROCESS_8_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P9 ON P9.PROCESS_CD = PP.PROCESS_9_CD  ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P10 ON P10.PROCESS_CD = PP.PROCESS_10_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P11 ON P11.PROCESS_CD = PP.PROCESS_11_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P12 ON P12.PROCESS_CD = PP.PROCESS_12_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS P13 ON P13.PROCESS_CD = PP.PROCESS_13_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON DEFECT1.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS1 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON DEFECT2.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS2 ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON DEFECT3.PROCESS_DEFECT_CD = GP.GP_NG_DETAILS3 ");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = GP.PROCESS_KUBUN_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = GP.GP_NEXT_PROCESS");


            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/06/18       
        /// Used Screen: Sales Production Status Search
        /// </summary>
        /// <returns>
        /// Get SQL To Download Sales Production Status Search CSV
        /// </returns>
        /// 
        public string GetSalesProductionStatusSQL(List<SearchField> searchCriteria, string sortCiteria)
        {
            List<SalesProductionStatusSearchDetails> result = new List<SalesProductionStatusSearchDetails>();

            StringBuilder selectQuery = new StringBuilder();


            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");
            //selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            selectQuery.Append(",CST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(",EMP.SHORT_NAME AS PP_EMPLOYEE_NAME");
            selectQuery.Append(",TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME");
            selectQuery.Append(",PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(",NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH");
            selectQuery.Append(",CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL AND PP.DIM_TOL_MIN_CHR IS NOT NULL THEN PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ELSE NVL(PP.DIM_TOL_MAX_CHR,PP.DIM_TOL_MIN_CHR) END AS PP_DIM_TOLERANCE");
            selectQuery.Append(",CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL AND PP.LENGTH_TOL_MIN_CHR IS NOT NULL THEN  PP.LENGTH_TOL_MAX_CHR ||','||PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append("ELSE  NVL(PP.LENGTH_TOL_MAX_CHR,PP.LENGTH_TOL_MIN_CHR) END AS PP_LENGTH_TOL,");

            selectQuery.Append("CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 1 THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "' || '-' ");
            selectQuery.Append(" WHEN PP.MAGARI_KUBUN = 2 THEN ");
            selectQuery.Append(" '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "' || '-' ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" '' ");
            selectQuery.Append(" END ");
            selectQuery.Append(" || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.MAGARI_SPAN IS NOT NULL THEN ");
            selectQuery.Append(" TO_CHAR(PP.MAGARI_BEND_TOL, '0.099') || '-' ");
            selectQuery.Append(" END ");
            selectQuery.Append(" || PP.MAGARI_SPAN AS PP_MAGARI_BEND_TOL ");

            selectQuery.Append(",PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(",TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(",PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(",FI.FINISHED_QTY AS FINISHED_WT, ");
            //selectQuery.Append(", (SELECT SUM(WIP_QTY) FROM TRN_WIP_STATUS WHERE PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ) AS FINISHED_WT, ");
            selectQuery.Append(" CASE WHEN FI.FINISHED_QTY > 0 AND PP.ACT_ALLOC_QTY > 0 THEN TO_CHAR((FI.FINISHED_QTY/ PP.ACT_ALLOC_QTY) * 100, 'FM999999990.00') || '%' END AS SUCCESS_PERCENTAGE, ");
            //selectQuery.Append(",CONDITION1.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun1of2") + "'");

            selectQuery.Append(" ELSE");
            selectQuery.Append("'" + _localization.GetLocalizedHtmlString("lbl_ProcessPlanStatusKubun2of2") + "'");
            selectQuery.Append(" END AS PROD_CONDITION_DESCRIPTION ");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(",PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(",PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(",PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO");

            selectQuery.Append(",TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE, 'yyyy/mm/dd') AS PROCESS_PLAN_CMPL_DATE_STRING");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");

            selectQuery.Append(" LEFT JOIN( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD, ");
            selectQuery.Append(" NVL(SUM(FI.FI_QTY), 0) AS FINISHED_QTY ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_FINAL_INSPECTION FI ");
            selectQuery.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON FI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" AND FI.COMBINED_DECISION = 10 ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" ) FI ON PP.PROCESS_PLAN_CD = FI.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD  ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.PROD_CONDITION_CD ");

            selectQuery.Append(" WHERE ");


            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " AND ";
                string bindQuery = string.Empty;
                int i = 0;
                foreach (var searchField in searchCriteria)
                {

                    i = i + 1;
                    if (i == 1)
                    {
                        bindQuery = DynamicSearchProvider.ApplySearchCriteria(searchField, query);

                    }
                    else
                    {
                        bindQuery += whereCondition + " " + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    }
                }
                query = query + bindQuery;
                query = query + " AND CONCAT(PP.CHUUBAN,PP.GYOUBAN) NOT IN (" + _specialChuubanForSQL + ")"; // EXCEPT SPECIAL TYPE PROCESS PLANCE CD SKIPPED.
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += " ORDER BY PP_CHUUBAN_GYOUBAN, PP_PROD_NAME";
            }

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();
        }

        public string GetNextProcessWaitingStatusSearchSQL(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridsearchresult)
        {
            var queryResult = "";
            StringBuilder selectQuery = new StringBuilder();
            if (gridsearchresult.WIP_STATUS == "1")
            {
                queryResult = "";
                selectQuery.Append(" SELECT * FROM (");
                selectQuery.Append(" SELECT ");
                //selectQuery.Append(" 'WAIT' AS WIP_STATUS_STRING,");
                selectQuery.Append("'" + _localization.GetLocalizedHtmlString("lbl_WAIT") + "'  AS WIP_STATUS_STRING,");

                if (gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");

                }
                else
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");

                }
                                    
                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
                selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");                
                selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");               
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS || ' ' || ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.UNIT = 1 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 2 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 3 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 4 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS WIP_NO_OF_UNITS, ");

                selectQuery.Append(" WIP.WIP_QTY AS WIP_QTY, ");
                if (gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");

                }
                else
                {
                    selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
                }
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 10 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 20 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 30 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 35 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 40 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 60 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 65 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 70 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 73 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 76 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 79 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 80 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 90 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS DW_DECISION, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" TO_DATE('') AS PROCESS_START_DATE_STRING,");
                selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");
               
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE");

                //selectQuery.Append(" '1' as WIP_STATUS");

                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_WIP_STATUS WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = WIP.MSG_CD ");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NOT NULL OR WIP.START_EMPLOYEE_CD IS NULL) AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuubanForSQL + ")  ");

                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append(whereCondition);
                }

                selectQuery.Append(" )");
                queryResult = selectQuery.ToString();

            }
            else if (gridsearchresult.WIP_STATUS == "2")
            {
                queryResult = "";
                selectQuery.Append(" SELECT * FROM (");
                selectQuery.Append(" SELECT ");

                selectQuery.Append("'" + _localization.GetLocalizedHtmlString("lbl_StatusWIP") + "'  AS WIP_STATUS_STRING,");
                if(gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");

                }
                else
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");

                }

                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
                selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");
               
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS || ' ' || ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.UNIT = 1 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 2 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 3 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 4 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS WIP_NO_OF_UNITS, ");

                selectQuery.Append(" WIP.WIP_QTY AS WIP_QTY, ");   
                if(gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" NULL AS PROCESS_KUBUN_EN, ");

                }
                else
                {
                    selectQuery.Append(" NULL AS PROCESS_KUBUN_JP, ");

                }
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 10 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 20 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 30 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 35 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 40 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 60 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 65 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 70 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 73 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 76 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 79 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 80 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 90 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS DW_DECISION, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" TO_CHAR(WIP.PROCESS_START_DATE,'YYYY/MM/DD HH24:MI') AS PROCESS_START_DATE_STRING, ");
                selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE");

                //selectQuery.Append(" '2' as WIP_STATUS");


                selectQuery.Append(" FROM ");
                selectQuery.Append(" TRN_WIP_STATUS WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = WIP.MSG_CD ");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NULL AND WIP.START_EMPLOYEE_CD IS NOT NULL) AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuubanForSQL + ")  ");

                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append(whereCondition);
                }
                selectQuery.Append(" )");
                queryResult = selectQuery.ToString();
            }


            else
            {
                selectQuery.Append(" SELECT* FROM (");
                selectQuery.Append(" (SELECT ");
                selectQuery.Append("'" + _localization.GetLocalizedHtmlString("lbl_WAIT") + "'  AS WIP_STATUS_STRING,");
                if (gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");

                }
                else
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");

                }
                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");

                selectQuery.Append(" CUST.SHORT_NAME        AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST      AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME   AS INTERNAL_NAME, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_SHAPE          AS PP_PROD_SHAPE, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE              AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");               
                selectQuery.Append(" WIP.WIP_NO_OF_UNITS || ' ' || ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.UNIT = 1 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 2 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 3 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 4 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS WIP_NO_OF_UNITS, ");

                selectQuery.Append(" WIP.WIP_QTY            AS WIP_QTY, ");
                if (gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN, ");

                }
                else
                {
                    selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
                }
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 10 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_OK") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 20 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_NG") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 30 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hold") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 35 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_UnTested") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 40 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DecisionKubun4of4") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 60 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReHeatTreatment") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 65 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Temp_Again") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 70 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReDrawing") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 73 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Restraightening") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 76 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Re_Chamfering") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 79 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_ReEc") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 80 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lb_SparkUntested") + "'");
                selectQuery.Append(" WHEN WIP.PRE_PROCESS_DECISION = 90 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_DieDimKubun5Of5") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS DW_DECISION, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY       AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME         AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" NULL AS PROCESS_START_DATE_STRING, ");
                selectQuery.Append(" M.MSG_CONTENTS         AS REMARK, ");
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE");

                // selectQuery.Append(" '1' as WIP_STATUS");


                selectQuery.Append(" FROM ");

                selectQuery.Append(" TRN_WIP_STATUS  WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN       PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER           CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME      INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE           EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE            M ON M.MSG_CD = WIP.MSG_CD ");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.NEXT_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NOT NULL OR WIP.START_EMPLOYEE_CD IS NULL) ");
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuubanForSQL + ")  ");

                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append(whereCondition);
                }
                selectQuery.Append("  ) ");


                selectQuery.Append("  UNION ALL ");

                selectQuery.Append(" (SELECT ");

                selectQuery.Append("'" + _localization.GetLocalizedHtmlString("lbl_StatusWIP") + "'  AS WIP_STATUS_STRING,");
                if (gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME_EN AS NEXT_PROCESS_EN, ");

                }
                else
                {
                    selectQuery.Append(" PROCK1.KUBUN_NAME AS NEXT_PROCESS_JP, ");

                }

                selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
                selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
                selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
                selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
                selectQuery.Append(" PP.PROD_DIM_D, ");
                selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
                selectQuery.Append(" PP.PROD_DIM_W, ");
                selectQuery.Append(" PP.PROD_DIM_OTHER, ");
                selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING,");
                selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
                selectQuery.Append(" CASE WHEN(WIP.TABA_NO != 0) ");
                selectQuery.Append(" THEN WIP.TABA_NO ELSE ");
                selectQuery.Append(" NULL ");
                selectQuery.Append(" END AS TABA_NO, ");

                selectQuery.Append(" WIP.WIP_NO_OF_UNITS || ' ' || ");
                selectQuery.Append(" CASE ");
                selectQuery.Append(" WHEN WIP.UNIT = 1 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 2 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 3 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "'");
                selectQuery.Append(" WHEN WIP.UNIT = 4 ");
                selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "'");
                selectQuery.Append(" ELSE ");
                selectQuery.Append(" '' ");
                selectQuery.Append(" END AS WIP_NO_OF_UNITS, ");

                selectQuery.Append(" WIP.WIP_QTY AS WIP_QTY, ");
                if (gridsearchresult.CULTURE_NAME == "en")
                {
                    selectQuery.Append(" NULL AS PROCESS_KUBUN_EN, ");

                }
                else
                {
                    selectQuery.Append(" NULL AS PROCESS_KUBUN_JP, ");

                }
                selectQuery.Append(" TO_CHAR(WIP.PRE_PROCESS_DATE, 'YYYY/MM/DD HH24:MI') AS PRE_PROSESS_DATE_STRING, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - WIP.PRE_PROCESS_DATE) AS DELAY_DAYS_A, ");
                selectQuery.Append(" FLOOR(CURRENT_DATE - PP.ACT_ALLOC_DATE) AS DELAY_DAYS_B, ");
                selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
                selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
                selectQuery.Append(" EMP.SHORT_NAME AS OPERATOR_NAME, ");
                selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'YYYY/MM/DD') AS PP_DELIVERY_DATE_STRING, ");
                selectQuery.Append(" TO_CHAR(WIP.PROCESS_START_DATE,'YYYY/MM/DD HH24:MI') AS PROCESS_START_DATE_STRING, ");
                selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");
                selectQuery.Append(" PP.PROD_SPEC_NO, ");
                selectQuery.Append(" WIP.NEXT_PROCESS AS NEXT_PROCESS_INVISIBLE");

                // selectQuery.Append(" '2' as WIP_STATUS");


                selectQuery.Append(" FROM ");

                selectQuery.Append(" TRN_WIP_STATUS         WIP ");
                selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN       PP ON PP.PROCESS_PLAN_CD = WIP.PROCESS_PLAN_CD ");
                selectQuery.Append(" INNER JOIN MST_CUSTOMER           CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME      INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                selectQuery.Append(" LEFT JOIN MST_EMPLOYEE           EMP ON EMP.EMPLOYEE_CD = PP.EMPLOYEE_CD ");
                selectQuery.Append(" LEFT JOIN TRN_MESSAGE            M ON M.MSG_CD = WIP.MSG_CD ");
                selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK1 ON PROCK1.KUBUN_ID = WIP.PRE_PROCESS");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" (WIP.PRE_PROCESS_DATE IS NULL AND WIP.START_EMPLOYEE_CD IS NOT NULL) AND PP.PROCESS_PLAN_CMPL_DATE IS NULL AND CONCAT(PP.CHUUBAN, PP.GYOUBAN)  NOT IN  (" + _specialChuubanForSQL + ")  ");

                if (searchFields != null && searchFields.Count > 0)
                {
                    string whereCondition = " AND ";
                    string andOperator = string.Empty;

                    foreach (var searchField in searchFields)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                        andOperator = " AND ";
                    }
                    selectQuery.Append(whereCondition);
                }
                selectQuery.Append(" ) ");
                selectQuery.Append(" ) ");

                queryResult = selectQuery.ToString();

            }


            if (!string.IsNullOrEmpty(sortCriteria))
            {
                queryResult = queryResult + " ORDER BY " + sortCriteria;
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(queryResult);



            return selectQuery.ToString();
        }

        public string GetProductForecastDetailsSQL(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridsearchresult)
        {
            var queryResult = "";
            StringBuilder selectQuery = new StringBuilder();


            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PRD.PRODUCT_KUBUN,");
            selectQuery.Append(" PRD.PRODUCT_NAME_CUST,");
            //selectQuery.Append(" CUST.SHORT_NAME  AS CUSTOMER_NAME,");
            selectQuery.Append(" PRD.DIM_D AS PROD_DIM_D,");
            selectQuery.Append(" PRD.SHAPE,");
            selectQuery.Append(" PRD.DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" PRD.DIM_OTHER AS PROD_DIM_OTHER,");
            selectQuery.Append(" PRD.LENGTH_OTHER,");//Len
            selectQuery.Append("PRD.magari_kubun,");
            selectQuery.Append(" PRD.CONDITION_DESCRIPTION AS CONDITION_DESCRIPTION,");
            selectQuery.Append(" SF.LAST1_MNTH_DISPACH_QTY AS DISPATCH_QTY1, ");
            selectQuery.Append(" SF.LAST2_MNTH_DISPACH_QTY AS DISPATCH_QTY2, ");
            selectQuery.Append(" SF.LAST3_MNTH_DISPACH_QTY AS DISPATCH_QTY3,");
            selectQuery.Append(@"
                               CAST((
                                     NVL(SF.LAST1_MNTH_DISPACH_QTY, 0) + 
                                     NVL(SF.LAST2_MNTH_DISPACH_QTY, 0) + 
                                     NVL(SF.LAST3_MNTH_DISPACH_QTY, 0)
                                     )* 1.0 / 3 AS NUMBER(18, 2)) AS AVG_DISPATCH_QTY,
                             ");
            selectQuery.Append(" SF.FC_QTY_R1,");
            selectQuery.Append(" SF.FC_QTY_R2,");
            selectQuery.Append(" SF.FC_QTY_R3,");
            selectQuery.Append(" SF.FC_QTY_R4,");
            selectQuery.Append(" PRD.PROD_SPEC_NO   AS PROD_SPEC_NO,");
            selectQuery.Append(" PRD.MATL_NAME_MAKER AS MATL_MAKER_NAME,");
            selectQuery.Append(" PRD.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_D, NULL, NULL, NULL) AS DIM_D_STRING,"); //DimTol
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_W, NULL, NULL, NULL) AS DIM_W_STRING,"); //LenTol
            selectQuery.Append(" SF.PROD_STOCK,");
            selectQuery.Append(" SF.WIP_STOCK,");
            selectQuery.Append(" PRD.REMARKS,");
            selectQuery.Append(" SF.SALES_FC_DONE_DATE,");
            selectQuery.Append(" SF.REPLY_FROM_MATL_DEPT,");
            selectQuery.Append(" PRD.MAKER_NAME AS MAKER_NAME");

            selectQuery.Append(" FROM");
            selectQuery.Append(" TRN_SALES_FORECAST SF");
            selectQuery.Append(" LEFT JOIN V_PRODUCT_DETAILS   PRD ON SF.PRODUCT_CD = PRD.PRODUCT_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER        CUST ON SF.CUST_CD = CUST.CUSTOMER_CD");


            if (searchFields != null && searchFields.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, null);
                    andOperator = " AND ";
                }
                selectQuery.Append(whereCondition);
            }



            if (!string.IsNullOrEmpty(sortCriteria))
            {
                queryResult = queryResult + " ORDER BY " + sortCriteria;
            }
            //selectQuery = new StringBuilder();
            selectQuery.Append(queryResult);


            return selectQuery.ToString();
        }

        public string GetMaterialForecastDetailsSQL(List<SearchField> searchCriteria, string sortCriteria, GridSearchFields gridsearchresult)
        {
            var queryResult = "";
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            //selectQuery.Append(" SF.ROLL_FC_KUBUN,");
            selectQuery.Append(" MD.MATL_MAKER_NAME,");
            selectQuery.Append(" MD.MATL_KUBUN,");
            selectQuery.Append(" MD.DIM_D AS MATL_DIM_D,");
            selectQuery.Append(" MD.SHAPE AS MATL_SHAPE,");
            selectQuery.Append(" MD.DIM_W AS MATL_DIM_W,");
            //selectQuery.Append(" MD.DIM_OTHER AS MATL_DIM_OTHER,");//Aux
            selectQuery.Append(" MD.MATERIAL_LENGTH,");         
                  
                             
            selectQuery.Append(" SF.FC_QTY_R1,");
            selectQuery.Append(" SF.FC_QTY_R2,");
            selectQuery.Append(" SF.FC_QTY_R3,");
            selectQuery.Append(" SF.FC_QTY_R4,");
            selectQuery.Append(" SF.SALES_FC_DONE_DATE,");
            selectQuery.Append(" MD.MAKER_NAME   AS MAKER_NAME,");
            selectQuery.Append(" MD.INTERNAL_NAME ,");
            selectQuery.Append(" DECODE(MD.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION ,");
            selectQuery.Append(" MKSAKI.SHORT_NAME AS MOCHIKOMISAKI_NAME,");
            selectQuery.Append(" SF.MSG_TO_MATL_DEPT as REMARKS,");
            selectQuery.Append(" SF.REPLY_FROM_MATL_DEPT");
            

            selectQuery.Append(" FROM");
            selectQuery.Append(" TRN_SALES_FORECAST SF ");
            selectQuery.Append(" LEFT JOIN V_HT_MATL_DETAILS MD ON SF.MATERIAL_CD = MD.MATERIAL_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION  CND ON MD.CONDITION_CD = CND.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI   MKSAKI ON MD.MOCHIKOMISAKI_CD = MKSAKI.MOCHIKOMISAKI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER   CS ON SF.CUST_CD = CS.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DG ON CS.DEPARTMENT_GROUP_CD= DG.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE  ME ON DG.EMPLOYEE_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append(" WHERE");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " ROLL_FC_KUBUN =" + Constants.Numbers.MATERIAL_KUBUN + " AND ";
                string andOperator = string.Empty;
                foreach (var searchField in searchCriteria)
                {

                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";

                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }


            return query.ToString();
        }
        public string GetTransferReportDailySQL(List<SearchField> searchCriteria, string sortCiteria, string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();

            //selectQuery.Append(" SELECT ");
            //selectQuery.Append(" WIP.TRANSFER_DATE , ");
            //selectQuery.Append(" ACC .ACCEPTANCE_DATE , ");
            //selectQuery.Append(" LOC_FROM.SHORT_NAME AS TRANSFER_START_LOCATION , ");
            //selectQuery.Append(" LOC_TO.SHORT_NAME AS TRANSFER_PLANNED_LOCATION , ");
            //selectQuery.Append(" LOC_ACC.SHORT_NAME AS TRANSFER_ACCEPTED_LOCATION , ");
            //selectQuery.Append("GET_HYPHENATED_CHUUBAN_GYOUBAN(pp.chuuban_year||PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN , ");
            //selectQuery.Append("CUST.SHORT_NAME AS PP_CUSTOMER_NAME , ");
            //selectQuery.Append("PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST , ");
            //selectQuery.Append("PP.PROD_DIM_D AS PP_PROD_DIM_D , ");
            //selectQuery.Append("PP.PROD_SHAPE AS PP_PROD_SHAPE , ");            
            //selectQuery.Append("PP.PROD_DIM_OTHER AS PROD_DIM_W_OTHER , ");
            //selectQuery.Append("NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING , ");
            //selectQuery.Append("PP.CHARGE AS PP_CHARGE , ");
            ////all taba no combine
            //selectQuery.Append("RTRIM(");
            //selectQuery.Append("NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '') || ");
            //selectQuery.Append("NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '') || ");
            //selectQuery.Append("NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '') || ");
            //selectQuery.Append("NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', '')");
            //selectQuery.Append(", ',') AS TABA_STRING , ");

            //selectQuery.Append("NVL(ACC.NO_OF_UNITS,WIP.NO_OF_UNITS) AS NO_OF_UNITS , ");


            //selectQuery.Append(" CASE NVL(ACC.UNIT_TYPE, WIP.UNIT_TYPE) ");
            //selectQuery.Append(" WHEN 1 ");
            //selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            //selectQuery.Append(" WHEN 2 ");
            //selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "' ");
            //selectQuery.Append(" WHEN 3 ");
            //selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "' ");
            //selectQuery.Append(" WHEN 4 ");
            //selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "' ");
            //selectQuery.Append(" ELSE '' ");
            //selectQuery.Append(" END AS UNIT_TYPE, ");

            //selectQuery.Append("NVL(ACC.QTY,WIP.QTY) AS QTY , ");
            //selectQuery.Append("PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            //selectQuery.Append("WIP.WIP_TRANSFER_CD , ");
            //selectQuery.Append("MSG_WIP.MSG_CONTENTS AS TRANSFER_REMARK, ");
            //selectQuery.Append("MSG_ACC.MSG_CONTENTS AS ACCEPTANCE_REMARK, ");
            //if (cultureName == "en")
            //{
            //    selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            //}
            //else
            //{
            //    selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP ");
            //}
            //selectQuery.Append(" FROM ");
            //selectQuery.Append(" TRN_WIP_TRANSFER WIP");
            //selectQuery.Append(" INNER JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM ");
            //selectQuery.Append(" INNER JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO ");
            //selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = WIP.PROCESS_KUBUN_CD ");
            //selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            //selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            //selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_WIP_TRANSFER_ACCEPTANCE ACC ON ACC.WIP_TRANSFER_CD = WIP.WIP_TRANSFER_CD ");
            //selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = NVL(ACC.NEXT_PROSESS,WIP.PLANNED_NEXT_PROSESS) ");
            //selectQuery.Append(" LEFT JOIN MST_LOCATION LOC_ACC ON LOC_ACC.LOCATION_CD = ACC.INPUT_LOCATION_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_WIP ON WIP.MSG_CD = MSG_WIP.MSG_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_ACC ON ACC.MSG_CD = MSG_ACC.MSG_CD");

            selectQuery.Append(" SELECT");
            selectQuery.Append(" TO_CHAR(WIP.ACCEPT_PLANNED_DATE, 'YYYY/MM/DD') AS ACCEPT_PLANNED_DATE,");
            selectQuery.Append(" LOC_TO.SHORT_NAME AS TRANSFER_PLANNED_LOCATION,");
            selectQuery.Append(" TO_CHAR(WIP.TRANSFER_DATE, 'YYYY/MM/DD') AS TRANSFER_DATE_STRING,");
            selectQuery.Append(" LOC_FROM.SHORT_NAME AS TRANSFER_START_LOCATION,");
            selectQuery.Append(" MV.VEHICLE_NAME AS VEHICLE_NAME,");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN,");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,");
           
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER('', PP.PROD_DIM_W, '', NVL(TRIM(PP.PROD_DIM_OTHER), '')) AS PP_PROD_DIM_W_OTHER,");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, TO_CHAR(PP.PROD_LENGTH)) AS PP_PROD_LENGTH_STRING,");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE,");
            selectQuery.Append(" RTRIM(NVL2(WIP.TABA_NO_1, WIP.TABA_NO_1 || ',', '')");
            selectQuery.Append(" || NVL2(WIP.TABA_NO_2, WIP.TABA_NO_2 || ',', '')");
            selectQuery.Append(" || NVL2(WIP.TABA_NO_3, WIP.TABA_NO_3 || ',', '')");
            selectQuery.Append(" || NVL2(WIP.TABA_NO_4, WIP.TABA_NO_4 || ',', ''), ',') AS TABA_STRING,");
            selectQuery.Append(" WIP.NO_OF_UNITS AS NO_OF_UNITS,");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN WIP.UNIT_TYPE = 1 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Hone") + "' ");
            selectQuery.Append(" WHEN WIP.UNIT_TYPE = 2 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Thaba_L") + "' ");
            selectQuery.Append(" WHEN WIP.UNIT_TYPE = 3 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Pieces_L") + "' ");
            selectQuery.Append(" WHEN WIP.UNIT_TYPE = 4 ");
            selectQuery.Append(" THEN '" + _localization.GetLocalizedHtmlString("lbl_Coil_L") + "' ");
            selectQuery.Append(" ELSE '' ");
            selectQuery.Append(" END AS UNIT_TYPE, ");
            selectQuery.Append(" WIP.QTY AS QTY,");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,");
            selectQuery.Append(" WIP.TRANSFER_CD,");
            selectQuery.Append(" MSG_WIP.MSG_CONTENTS AS TRANSFER_REMARK,");
            selectQuery.Append(" MSG_ACC.MSG_CONTENTS AS ACCEPTANCE_REMARK,");
            
            if (cultureName == "en")
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
            }
            else
            {
                selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP ");
            }
            selectQuery.Append(" FROM");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER WIP");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC_FROM ON LOC_FROM.LOCATION_CD = WIP.LOCATION_CD_FROM");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC_TO ON LOC_TO.LOCATION_CD = WIP.LOCATION_CD_TO");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON WIP.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" INNER JOIN MST_PROCESS_KUBUN PROCKNEXTPROC ON PROCKNEXTPROC.KUBUN_ID = NVL(WIP.ACCEPT_NEXT_PROSESS, WIP.PLANNED_NEXT_PROSESS)");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_WIP ON WIP.MSG_CD = MSG_WIP.MSG_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG_ACC ON WIP.ACCEPT_MSG_CD = MSG_ACC.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE MV ON MV.VEHICLE_CD = WIP.START_VEHICLEL_CD");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " WHERE ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    //whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    if (searchField.SearchType == SearchType.Custom)
                    {
                        whereCondition += andOperator + "(" + searchField.FieldName + ")";
                    }
                    else
                    {
                        whereCondition += andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    }
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }            

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            return selectQuery.ToString();

        }

        public string GetYieldSearchStatusSQL(List<SearchField> searchCriteria, string sortCiteria)
        {
            List<SalesProductionStatusSearchDetails> result = new List<SalesProductionStatusSearchDetails>();

            StringBuilder sb = new StringBuilder();


            sb.Append("   SELECT ");
            sb.Append("     PP_CHUUBAN_GYOUBAN, ");
            
            sb.Append("     PP_PROD_SPEC_NO, ");
            sb.Append("     PP_CUSTOMER_NAME, ");
            sb.Append("     PP_PROD_NAME, ");
            sb.Append("     PP_PROD_SHAPE, ");
            sb.Append("     PP_PROD_DIM_D, ");
            
            sb.Append("     PP_PROD_LENGTH, ");
           
            sb.Append("     PROD_CONDITION_DESCRIPTION, ");
            sb.Append("     PP_DIM_TOLERANCE, ");
            sb.Append("     PP_LENGTH_TOL, ");
            sb.Append("     BEND_STD, ");
            
            sb.Append("     PP_ACT_ALLOC_DATE_STRING, ");
            sb.Append("     PP_ACT_ALLOC_QTY, ");

            sb.Append("     HIKINUKI_QTY, "); 
            sb.Append("     HIKINUKI_YIELD, ");
            sb.Append("     STRAIGHTENING_QTY, ");
            sb.Append("     STRAIGHTENING_YIELD, ");
            sb.Append("     CHAMFERING_QTY, ");
            sb.Append("     CHAMFERING_YIELD, ");
            sb.Append("     EC_QTY, ");
            sb.Append("     EC_YIELD, ");
            sb.Append("     FI_QTY, ");
            sb.Append("     FI_YIELD, ");
            sb.Append("     PROCESS_PLAN_CMPL_DATE_STRING ");
            sb.Append("   FROM ( ");
            sb.Append("   SELECT ");
            sb.Append("     PP_CHUUBAN_GYOUBAN, ");
            sb.Append("     PP_PROCESS_PLAN_CD, ");
            sb.Append("     PP_PROD_SPEC_NO, ");
            sb.Append("     PP_CUSTOMER_NAME, ");
            sb.Append("     PP_PROD_NAME, ");
            sb.Append("     PP_PROD_SHAPE, ");
            sb.Append("     PP_PROD_DIM_D, ");
            sb.Append("     PP_PROD_DIM_W, ");
            sb.Append("     PP_PROD_DIM_OTHER, ");
            sb.Append("     PP_PROD_LENGTH, ");
            sb.Append("     PP_PROD_LENGTH_OTHER, ");
            sb.Append("     PROD_CONDITION_DESCRIPTION, ");
            sb.Append("     PP_DIM_TOLERANCE, ");
            sb.Append("     PP_LENGTH_TOL, ");
            sb.Append("     BEND_STD, ");
            sb.Append("     MAKER_NAME, ");
            sb.Append("     PP_ACT_ALLOC_DATE_STRING, ");
            sb.Append("     PP_ACT_ALLOC_QTY, ");
            
            sb.Append("     HIKINUKI_QTY, ");
            sb.Append("     STRAIGHTENING_QTY, ");
            sb.Append("     CHAMFERING_QTY, ");
            sb.Append("     EC_QTY, ");
            sb.Append("     FI_QTY, ");

            sb.Append("     CASE WHEN PP_ACT_ALLOC_QTY <> 0 ");
            sb.Append("          THEN ROUND(HIKINUKI_QTY / PP_ACT_ALLOC_QTY * 100, 2) END AS HIKINUKI_YIELD, ");

            sb.Append("     CASE WHEN HIKINUKI_QTY <> 0 ");
            sb.Append("          THEN ROUND(STRAIGHTENING_QTY / HIKINUKI_QTY * 100, 2) END AS STRAIGHTENING_YIELD, ");

            sb.Append("     CASE WHEN HIKINUKI_QTY <> 0 ");
            sb.Append("          THEN ROUND(CHAMFERING_QTY / HIKINUKI_QTY * 100, 2) END AS CHAMFERING_YIELD, ");

            sb.Append("     CASE WHEN HIKINUKI_QTY <> 0 ");
            sb.Append("          THEN ROUND(EC_QTY / HIKINUKI_QTY * 100, 2) END AS EC_YIELD, ");

            sb.Append("     CASE WHEN PP_ACT_ALLOC_QTY <> 0 ");
            sb.Append("          THEN ROUND(FI_QTY / PP_ACT_ALLOC_QTY * 100, 2) END AS FI_YIELD, ");
            sb.Append("     PROCESS_PLAN_CMPL_DATE_STRING ");

            sb.Append("   FROM ( ");


            sb.Append("     SELECT ");
            sb.Append("       PP.ACT_ALLOC_DATE AS PP_ACT_ALLOC_DATE, ");

            sb.Append("       GET_HYPHENATED_CHUUBAN_GYOUBAN( ");
            sb.Append("           PP.CHUUBAN_YEAR || PP.CHUUBAN, ");
            sb.Append("           TRIM(LEADING ' ' FROM TO_CHAR(PP.GYOUBAN,'00')) ");
            sb.Append("       ) AS PP_CHUUBAN_GYOUBAN, ");

            sb.Append("       PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD, ");
            sb.Append("       PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
            sb.Append("       CST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            sb.Append("       PP.PROD_NAME_CUST AS PP_PROD_NAME, ");
            sb.Append("       PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            sb.Append("       PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            sb.Append("       PP.PROD_DIM_W AS PP_PROD_DIM_W, ");
            sb.Append("       PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER, ");
            sb.Append("       NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH, ");
            sb.Append("       PP.LENGTH_OTHER AS PP_PROD_LENGTH_OTHER, ");
            sb.Append("       CONDITION1.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION, ");

            //sb.Append("       PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR AS PP_DIM_TOLERANCE, ");
            sb.Append("CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL AND PP.DIM_TOL_MIN_CHR IS NOT NULL THEN PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ELSE NVL(PP.DIM_TOL_MAX_CHR , PP.DIM_TOL_MIN_CHR) END AS PP_DIM_TOLERANCE ,");

            sb.Append("       CASE ");
            sb.Append("           WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL ");
            sb.Append("            AND PP.LENGTH_TOL_MIN_CHR IS NOT NULL ");
            sb.Append("           THEN PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            sb.Append("           ELSE NVL(PP.LENGTH_TOL_MAX_CHR, PP.LENGTH_TOL_MIN_CHR) ");
            sb.Append("       END AS PP_LENGTH_TOL, ");

            //sb.Append("       TRIM( ");
            //sb.Append("           NVL(PP.MAGARI_KUBUN, '') || ' - ' || ");
            //sb.Append("           NVL(PP.MAGARI_BEND_TOL, '') || ' - ' || ");
            //sb.Append("           NVL(PP.MAGARI_SPAN, '') ");
            //sb.Append("       ) AS BEND_STD, ");
            sb.Append("CASE ");
            sb.Append(" WHEN PP.MAGARI_KUBUN = 1 THEN ");
            sb.Append(" '" + _localization.GetLocalizedHtmlString("lbl_Bend") + "' || '-' ");
            sb.Append(" WHEN PP.MAGARI_KUBUN = 2 THEN ");
            sb.Append(" '" + _localization.GetLocalizedHtmlString("lbl_Bend_Round") + "' || '-' ");
            sb.Append(" ELSE ");
            sb.Append(" '' ");
            sb.Append(" END ");
            sb.Append(" || ");
            sb.Append(" CASE ");
            sb.Append(" WHEN PP.MAGARI_SPAN IS NOT NULL THEN ");
            sb.Append(" TO_CHAR(PP.MAGARI_BEND_TOL, '0.099') || '-' ");
            sb.Append(" END ");
            sb.Append(" || PP.MAGARI_SPAN AS BEND_STD ,");

            sb.Append("       MK.MAKER_NAME, ");
            sb.Append("       TO_CHAR(PP.ACT_ALLOC_DATE,'YYYY/MM/DD') AS PP_ACT_ALLOC_DATE_STRING, ");
            sb.Append("       PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");

            //sb.Append(" ( ");
            //sb.Append("     SELECT SUM(dw_finished_qty) ");
            //sb.Append("     FROM trn_drawing td ");
            //sb.Append("     WHERE td.process_plan_cd = pp.process_plan_cd ");
            //sb.Append("       AND td.dw_decision IN (10, 40) ");
            //sb.Append(" ) AS hikinuki_qty, ");
            sb.Append("     ( ");
            sb.Append("         SELECT SUM(dw_finished_qty) ");
            sb.Append("         FROM ( ");

            sb.Append("             SELECT dw_finished_qty ");
            sb.Append("             FROM trn_drawing ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND dw_decision IN (10, 40) ");

            sb.Append("             UNION ALL ");

            sb.Append("             SELECT dw_finished_qty ");
            sb.Append("             FROM trn_drawing_d ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND dw_decision IN (10, 40) ");

            sb.Append("             UNION ALL ");

            sb.Append("             SELECT dw_finished_qty ");
            sb.Append("             FROM trn_drawing_w ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND dw_decision IN (10, 40) ");

            sb.Append("         ) ");
            sb.Append("     ) AS hikinuki_qty, ");

            //sb.Append(" ( ");
            //sb.Append("     SELECT SUM(str_qty) ");
            //sb.Append("     FROM trn_straightening ts ");
            //sb.Append("     WHERE ts.process_plan_cd = pp.process_plan_cd ");
            //sb.Append("       AND ts.str_decision IN (10, 40) ");
            //sb.Append(" ) AS straightening_qty, ");

            sb.Append(" ( ");
            sb.Append("     SELECT SUM(str_qty) ");
            sb.Append("         FROM ( ");

            sb.Append("             SELECT str_qty ");
            sb.Append("             FROM trn_straightening ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND str_decision IN (10, 40) ");

            sb.Append("             UNION ALL ");

            sb.Append("             SELECT str_qty ");
            sb.Append("             FROM trn_straightening2 ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND str_decision IN (10, 40) ");

            sb.Append("         ) ");
            sb.Append(" ) AS straightening_qty, ");

            sb.Append(" ( ");
            sb.Append("     SELECT SUM(ch_finished_qty) ");
            sb.Append("     FROM trn_chamfering tc ");
            sb.Append("     WHERE tc.process_plan_cd = pp.process_plan_cd ");
            sb.Append("       AND tc.ch_decision IN (10, 40) ");
            sb.Append(" ) AS chamfering_qty, ");

            sb.Append(" ( ");
            sb.Append("     SELECT SUM(ec_ok_qty) ");
            sb.Append("     FROM trn_ec ec ");
            sb.Append("     WHERE ec.process_plan_cd = pp.process_plan_cd ");
            sb.Append("       AND ec.ec_decision IN (10, 30) ");
            sb.Append(" ) AS ec_qty, ");

            sb.Append(" ( ");
            sb.Append("     SELECT SUM(fi_qty) ");
            sb.Append("     FROM trn_final_inspection fi ");
            sb.Append("     WHERE fi.process_plan_cd = pp.process_plan_cd ");
            sb.Append("       AND fi.combined_decision = 10 ");
            sb.Append(" ) AS fi_qty, ");


            sb.Append("       TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE,'YYYY/MM/DD') AS PROCESS_PLAN_CMPL_DATE_STRING ");

            sb.Append("     FROM TRN_PROCESS_PLAN PP ");
            sb.Append("     LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            sb.Append("     LEFT JOIN MST_CONDITION CONDITION1 ON CONDITION1.CONDITION_CD = PP.PROD_CONDITION_CD ");
            sb.Append("     LEFT JOIN MST_MAKER MK ON PP.CUSTOMER_CD = MK.MAKER_CD ");


            sb.Append("     WHERE 1 = 1 ");


            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                foreach (var field in searchCriteria)
                {
                    sb.Append(" AND ");
                    sb.Append(DynamicSearchProvider.ApplySearchCriteria(field, "PP"));
                }
            }

            sb.Append("   ) ");
            sb.Append(" ) ");

            sb.Append(" WHERE PP_CHUUBAN_GYOUBAN NOT IN (" + _specialChuubanForSQL + ") ");// EXCEPT SPECIAL TYPE PROCESS PLANCE CD SKIPPED.


            if (!string.IsNullOrEmpty(sortCiteria))
                sb = new StringBuilder(Helper.ApplySortCriteria(sortCiteria, sb.ToString()));
            else
                sb.Append(" ORDER BY PP_PROD_NAME ,PP_PROD_SHAPE , PP_PROD_DIM_D ,PP_PROD_DIM_W ,PP_PROD_DIM_OTHER , PP_PROD_LENGTH ,PP_PROD_LENGTH_OTHER ");


            return sb.ToString();


        }
    }
}
