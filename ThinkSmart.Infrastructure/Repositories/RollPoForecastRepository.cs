using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class RollPoForecastRepository : BaseRepository, IRollPoForecastRepository
    {
        private readonly LocalizationServices _localization;
        private StringBuilder insertQuery;

        public RollPoForecastRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _localization = localization;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/12/24       
        /// used screens: RollSalesForecast
        /// </summary>
        /// <returns>
        /// Fetch Data From MST_PRODUCT Master Table
        /// </returns>
        /// 
        public List<GridSearchFields> GetProductMasterDataList(List<SearchField> searchFields, string sortCriteria)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<GridSearchFields> result = new List<GridSearchFields>();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT DISTINCT  ");
            selectQuery.Append(" (PRD.PRODUCT_CD ) AS PRODUCT_CD,");
            selectQuery.Append(" PRD.CUSTOMER_CD,");
            selectQuery.Append(" PRD.CUSTOMER_NAME,");
            selectQuery.Append(" PRD.ROOT_VER_PRODUCT_KEY,");
            selectQuery.Append(" PRD.PROD_SPEC_NO1 AS PROD_SPEC_NO1,");
            selectQuery.Append(" PRD.PROD_SPEC_NO2 AS PROD_SPEC_NO2,");
            selectQuery.Append(" PRD.PROD_SPEC_NO AS PROD_SPEC_NO,");
            selectQuery.Append(" VERSION AS VERSION,");
            selectQuery.Append(" PRD.CONDITION_DESCRIPTION   AS MATERIALCONDITION,");
            selectQuery.Append(" PRD.PRODUCT_NAME_CUST,");
            selectQuery.Append(" PRD.RECEIVER_NAME,");
            selectQuery.Append(" PRD.MAKER_NAME,");
            selectQuery.Append(" PRD.CHAMFER,");
            selectQuery.Append(" PRD.UNIT_WT,");
            selectQuery.Append(" PRD.MATL_NAME_MAKER AS MATL_MAKER_NAME,");
            selectQuery.Append(" PRD.MATL_SHAPE,");
            selectQuery.Append(" PRD.MATL_DIM_D,");
            selectQuery.Append(" PRD.MATL_TYPE_CBK,");
            selectQuery.Append(" PRD.MATL_LENGTH,");
            selectQuery.Append(" PRD.STEEL_TYPE_NAME,");
            selectQuery.Append(" PRD.MAGARI_KUBUN,");
            selectQuery.Append(" PRD.MAGARI_BEND_TOL,");
            selectQuery.Append(" PRD.MAGARI_SPAN,");
            selectQuery.Append(" PRD.SHAPE,");
            selectQuery.Append(" DIM_D_X_DIM_W,");
            selectQuery.Append(" PRD.DIM_D AS PROD_DIM_D,");
            selectQuery.Append(" PRD.DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_D, NULL, NULL, NULL) AS DIM_D_STRING,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_W, NULL, NULL, NULL) AS DIM_W_STRING, ");
            selectQuery.Append(" PRD.DIM_OTHER AS PROD_DIM_OTHER_STRING, ");
            selectQuery.Append(" PRD.EXT_TEXTURE,");
            selectQuery.Append(" PRD.DIM_TOL_MIN AS DIM_TOL_MIN_VALUE, ");
            selectQuery.Append(" PRD.DIM_TOL_MAX AS DIM_TOL_MAX_VALUE,");
            selectQuery.Append(" PRD.DISPLAY,");
            selectQuery.Append(" PRD.STAMP, ");
            selectQuery.Append(" PRD.PRODUCT_LENGTH AS PROD_LENGTH,");
            selectQuery.Append(" NVL(PRD.LENGTH_OTHER, PRD.PRODUCT_LENGTH) AS PRODUCT_LENGTH_STRING,");
            selectQuery.Append(" COALESCE(TO_CHAR(PRD.PRODUCT_LENGTH), TO_CHAR(PRD.LENGTH_OTHER)) AS PROD_LEN_OTHER, ");
            selectQuery.Append(" PRD.PACKING_CD, ");
            selectQuery.Append(" PRD.COIL_DIA_INTERNAL,");
            selectQuery.Append(" PRD.COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" PRD.COIL_DIA_WINDING,");
            selectQuery.Append(" PRD.REMARKS, ");
            selectQuery.Append(" PRD.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" PRD.LENGTH_OTHER, ");
            selectQuery.Append(" COALESCE(TO_CHAR(PRD.PRODUCT_LENGTH), TO_CHAR(PRD.LENGTH_OTHER)) AS PROD_LEN_OTHER,");
            selectQuery.Append(" PRD.TEST_STANDARD, ");
            selectQuery.Append(" PRD.CREATED_DATE,");
            selectQuery.Append(" PRD.IN_ACTIVE_FLAG, ");
            selectQuery.Append(" PRD.UPDATED_DATE,");
            selectQuery.Append(" DIM_TOL,");
            selectQuery.Append(" PRD.LEN_TOL, ");
            selectQuery.Append(" MATL_KUBUN, ");
            selectQuery.Append(" PRD.HT_CONDITION,");
            selectQuery.Append(" PRD.EMPLOYEE_SHORT_NAME AS EMPLOYEE_NAME,");
            selectQuery.Append(" PRD.LAST_USED_DATE,");
            selectQuery.Append(" CUS.YOBINA AS CUST_YOBINA,");
            selectQuery.Append(" CUS.YOBIGANA AS CUST_YOBIGANA, ");
            selectQuery.Append(" PRD.PRODUCT_KUBUN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" V_PRODUCT_DETAILS PRD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PRD.CUSTOMER_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" VERSION = (");
            selectQuery.Append(" SELECT MAX(VER) MAX_VERSION FROM ");
            selectQuery.Append(" MST_PRODUCT PROD ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" HAVING ");
            selectQuery.Append(" PROD.ROOT_VER_PRODUCT_KEY = PRD.ROOT_VER_PRODUCT_KEY ) ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchFields != null && searchFields.Count > 0)
            {
                whereCondition = " AND ";
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
            result = ExecuteQueryMultiResult<GridSearchFields>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Komal Kadam on 2024/12/20   
        /// Used Screens: RollSalesForecast
        /// </summary>
        /// <param name="next_R">
        /// Pass Next Roll
        /// e.g next_R = 2 
        /// 
        /// </param>
        /// <param name="next_YYYY">
        /// Pass Next Roll Year
        /// e.g next_YYYY = 2025  
        /// </param>
        /// <returns>This function check next roll and year batch run status </returns>
        public int IsBatchRunSuccess(int next_R, int next_YYYY)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(MONTH) AS COUNT FROM BCH_BATCH_RUN_STATUS");
            selectQuery.Append("  WHERE ");
            selectQuery.Append("  MONTH =" + next_R);
            selectQuery.Append("  And YEAR =" + next_YYYY);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;

        }
        /// <summary>
        /// CREATED BY : Komal Kadam on 2024/12/20   
        /// Used Screens: RollSalesForecast
        /// </summary>
        /// <param name="last_R">
        /// Pass Next Roll
        /// e.g last_R = 1
        /// </param>
        /// <param name="last_YYYY">
        /// Pass Next Roll Year
        /// e.g last_YYYY = 2025  
        /// </param>
        /// <returns>This function check last roll and year  Forecast data exist in DB</returns>
        public int IsForecastDataPresent(int last_R, int last_YYYY)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(ROLL_FC_CD) FROM TRN_SALES_FORECAST");
            selectQuery.Append("  WHERE ");
            selectQuery.Append("   ROLL_FC_R =" + last_R);
            selectQuery.Append("  And ROLL_FC_YEAR =" + last_YYYY);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;

        }
        /// <summary>
        /// CREATED BY : Komal Kadam on 2024/12/20   
        /// Used Screens: RollSalesForecast
        /// </summary>
        /// <param name="last_R">
        /// Pass Next Roll
        /// e.g last_R = 1
        /// </param>
        /// <param name="last_YYYY">
        /// Pass Next Roll Year
        /// e.g last_YYYY = 2025  
        /// </param>
        /// <returns>This function check Forecast is complete or not for last roll and year  </returns>
        public int IsPreviousRForecastLocked(int last_R, int last_YYYY)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(ROLL_FC_DATE) FROM TRN_SALES_FORECAST");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" ROLL_FC_R =" + last_R);
            selectQuery.Append(" And ROLL_FC_YEAR =" + last_YYYY);
            selectQuery.Append(" AND ROLL_FC_LOCK_DATE  IS NOT NULL");
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;

        }

        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/12/26      
        /// used screens: RollSalesForecast
        /// </summary>
        /// <returns>
        /// Fetch Data From MST_PRODUCT Master Table Using PRODUCT_CD
        /// </returns>
        /// 
        // public GridSearchFields GetProductDetailsByProductCD(long PRODUCT_CD)
        public RollSalesForecastDetails GetProductDetailsByProductCD(long PRODUCT_CD)
        {
            StringBuilder selectQuery = new StringBuilder();
            RollSalesForecastDetails result = new RollSalesForecastDetails();
            selectQuery.Append(" SELECT DISTINCT  ");
            selectQuery.Append(" (PRD.PRODUCT_CD ) AS PRODUCT_CD,");
            selectQuery.Append(" PRD.CUSTOMER_CD,");
            selectQuery.Append(" PRD.CUSTOMER_NAME,");
            selectQuery.Append(" PRD.ROOT_VER_PRODUCT_KEY,");
            selectQuery.Append(" PRD.PROD_SPEC_NO1 AS PROD_SPEC_NO1,");
            selectQuery.Append(" PRD.PROD_SPEC_NO2 AS PROD_SPEC_NO2,");
            selectQuery.Append(" PRD.PROD_SPEC_NO AS PROD_SPEC_NO,");
            selectQuery.Append(" VERSION AS VERSION,");
            selectQuery.Append(" PRD.CONDITION_DESCRIPTION   AS MATERIALCONDITION,");
            selectQuery.Append(" PRD.PRODUCT_NAME_CUST,");
            selectQuery.Append(" PRD.RECEIVER_NAME,");
            selectQuery.Append(" PRD.MAKER_NAME AS MATL_MAKER_NAME,");
            selectQuery.Append(" PRD.CHAMFER,");
            selectQuery.Append(" PRD.UNIT_WT,");
            selectQuery.Append(" PRD.MATL_NAME_MAKER AS MAKER_NAME,");
            selectQuery.Append(" PRD.MATL_SHAPE,");
            selectQuery.Append(" PRD.MATL_DIM_D,");
            selectQuery.Append(" PRD.MATL_TYPE_CBK,");
            selectQuery.Append(" PRD.MATL_LENGTH,");
            selectQuery.Append(" PRD.STEEL_TYPE_NAME,");
            selectQuery.Append(" PRD.MAGARI_KUBUN,");
            selectQuery.Append(" PRD.MAGARI_BEND_TOL,");
            selectQuery.Append(" PRD.MAGARI_SPAN,");
            selectQuery.Append(" PRD.SHAPE,");
            selectQuery.Append(" DIM_D_X_DIM_W,");
            selectQuery.Append(" PRD.DIM_D AS PROD_DIM_D,");
            selectQuery.Append(" PRD.DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_D, NULL, NULL, NULL) AS DIM_D_STRING,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_W, NULL, NULL, NULL) AS DIM_W_STRING, ");
            selectQuery.Append(" PRD.DIM_OTHER AS PROD_DIM_OTHER, ");
            selectQuery.Append(" PRD.EXT_TEXTURE,");
            selectQuery.Append(" PRD.DIM_TOL_MIN AS DIM_TOL_MIN_VALUE, ");
            selectQuery.Append(" PRD.DIM_TOL_MAX AS DIM_TOL_MAX_VALUE,");
            selectQuery.Append(" PRD.DISPLAY,");
            selectQuery.Append(" PRD.STAMP, ");
            selectQuery.Append(" PRD.PRODUCT_LENGTH AS PROD_LENGTH,");
            selectQuery.Append(" NVL(PRD.LENGTH_OTHER, PRD.PRODUCT_LENGTH) AS PRODUCT_LENGTH_STRING,");
            selectQuery.Append(" COALESCE(TO_CHAR(PRD.PRODUCT_LENGTH), TO_CHAR(PRD.LENGTH_OTHER)) AS PROD_LEN_OTHER, ");
            selectQuery.Append(" PRD.PACKING_CD, ");
            selectQuery.Append(" PRD.COIL_DIA_INTERNAL,");
            selectQuery.Append(" PRD.COIL_DIA_EXTERNAL, ");
            selectQuery.Append(" PRD.COIL_DIA_WINDING,");
            selectQuery.Append(" PRD.REMARKS, ");
            selectQuery.Append(" PRD.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
            selectQuery.Append(" PRD.LENGTH_OTHER, ");
            selectQuery.Append(" COALESCE(TO_CHAR(PRD.PRODUCT_LENGTH), TO_CHAR(PRD.LENGTH_OTHER)) AS PROD_LEN_OTHER,");
            selectQuery.Append(" PRD.TEST_STANDARD, ");
            selectQuery.Append(" PRD.CREATED_DATE,");
            selectQuery.Append(" PRD.IN_ACTIVE_FLAG, ");
            selectQuery.Append(" PRD.UPDATED_DATE,");
            selectQuery.Append(" DIM_TOL,");
            selectQuery.Append(" PRD.LEN_TOL, ");
            selectQuery.Append(" MATL_KUBUN, ");
            selectQuery.Append(" PRD.HT_CONDITION,");
            selectQuery.Append(" PRD.EMPLOYEE_SHORT_NAME AS EMPLOYEE_NAME,");
            selectQuery.Append(" PRD.LAST_USED_DATE,");
            selectQuery.Append(" CUS.YOBINA AS CUST_YOBINA,");
            selectQuery.Append(" CUS.YOBIGANA AS CUST_YOBIGANA, ");
            selectQuery.Append(" PRD.PRODUCT_KUBUN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" V_PRODUCT_DETAILS PRD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PRD.CUSTOMER_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" VERSION = (");
            selectQuery.Append(" SELECT MAX(VER) MAX_VERSION FROM ");
            selectQuery.Append(" MST_PRODUCT PROD ");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" ROOT_VER_PRODUCT_KEY ");
            selectQuery.Append(" HAVING ");
            selectQuery.Append(" PROD.ROOT_VER_PRODUCT_KEY = PRD.ROOT_VER_PRODUCT_KEY ) ");

            var query = selectQuery.ToString();
            string whereCondition = "";
            whereCondition = " AND PRD.PRODUCT_CD = " + PRODUCT_CD + " fetch first row only";
            query = query + whereCondition;
            selectQuery.Append(query);
            result = ExecuteQuerySingleResult<RollSalesForecastDetails>(query.ToString());
            return result;
        }
        public GridSearchFields GetProductDetailsByProductCD1(long PRODUCT_CD)
        {
            StringBuilder selectQuery = new StringBuilder();
            GridSearchFields gridSearch = new GridSearchFields();
            selectQuery.Append(" SELECT DISTINCT  ");

            selectQuery.Append(" PRD.CUSTOMER_NAME");


            selectQuery.Append(" FROM V_PRODUCT_DETAILS PRD ");


            var result = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());


            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/12/27      
        /// used screens: RollSalesForecast
        /// </summary>
        /// <returns>
        /// Fetch Data From MST_MATERIAL Master Table 
        /// </returns>
        /// 
        public List<GridSearchFields> GetMaterialMasterDataList(List<SearchField> searchFields, string sortCriteria)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<GridSearchFields> result = new List<GridSearchFields>();
            //selectQuery.Append(" SELECT  * FROM (");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MATL.*,");
            selectQuery.Append(" MTMAK.MATL_MAKER_NAME,");
            selectQuery.Append(" MTINT.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" MAK.SHORT_NAME AS MAKER_NAME,");
            selectQuery.Append(" CON.CONDITION_DESCRIPTION,");
            selectQuery.Append(" SUP.SHORT_NAME AS SUPPLIER_NAME,");
            selectQuery.Append(" FU.FUTOU_NAME AS FUTOU_NAME,");
            selectQuery.Append(" MOCH.MKSAKI_NAME,");
            selectQuery.Append(" MOCH.SHORT_NAME AS MOCHIKOMISAKI_NAME,");
            selectQuery.Append(" ST.STEEL_TYPE_NAME,");
            selectQuery.Append(" HIKI.SHORT_NAME AS HTSAKI_NAME,");
            selectQuery.Append(" MUKE.SHORT_NAME AS MUKESAKI_NAME, ");
            selectQuery.Append(" MATL.DIM_D AS MATL_DIM_D, ");
            selectQuery.Append(" MATL.SHAPE AS MATL_SHAPE, ");
            selectQuery.Append(" MATL.DIM_W AS MATL_DIM_W, ");
            selectQuery.Append(" MATL.DIM_OTHER AS MATL_DIM_OTHER, ");
            selectQuery.Append(" MATL.MATL_KUBUN AS MATL_TYPE_CBK ");
            //selectQuery.Append(" (GET_FUTOU_NYUKO_QTY_BY_MATL(MATERIAL_CD) - GET_MATL_NYUKA_QTY(MATERIAL_CD) - GET_FUTOU_SHUKKO_QTY_BY_MATL(MATERIAL_CD) ) AS ACTUAL_STOCK ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_MATERIAL MATL ");
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER   MTMAK ON MATL.MATL_NAME_MAKER_CD = MTMAK.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MAKER MAK ON MAK.MAKER_CD = MTMAK.MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME MTINT ON MTMAK.INTERNAL_NAME_CD = MTINT.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_STEEL_TYPE ST ON ST.STEEL_TYPE_CD = MTINT.STEEL_TYPE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = MATL.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_SUPPLIER SUP ON SUP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            selectQuery.Append(" INNER JOIN MST_FUTOU FU ON FU.FUTOU_CD = MATL.FUTOU_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");
            selectQuery.Append(" LEFT JOIN MST_HIKITORISAKI HIKI ON HIKI.HTSAKI_CD = MATL.HTSAKI_CD  ");
            selectQuery.Append(" LEFT JOIN MST_MUKESAKI MUKE ON MUKE.MUKESAKI_CD = MATL.MUKESAKI_CD ");
            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchFields != null && searchFields.Count > 0)
            {
                whereCondition = " WHERE ";
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
            result = ExecuteQueryMultiResult<GridSearchFields>(query);
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/12/30      
        /// used screens: RollSalesForecast
        /// </summary>
        /// <param name="MATERIAL_CD"
        /// <returns>
        /// Fetch Data From MST_MATERIAL Master Table Using MATERIAL_CD
        /// </returns>
        /// 
        public RollSalesForecastDetails GetMaterialDetailsByMaterialCD(long MATERIAL_CD)
        {
            StringBuilder selectQuery = new StringBuilder();
            RollSalesForecastDetails result = new RollSalesForecastDetails();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MATL.*,");
            selectQuery.Append(" MTMAK.MATL_MAKER_NAME,");
            selectQuery.Append(" MTINT.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" MAK.SHORT_NAME AS MAKER_NAME,");
            selectQuery.Append(" CON.CONDITION_DESCRIPTION,");
            selectQuery.Append(" SUP.SHORT_NAME AS SUPPLIER_NAME,");
            selectQuery.Append(" FU.FUTOU_NAME AS FUTOU_NAME,");
            selectQuery.Append(" MOCH.MKSAKI_NAME,");
            selectQuery.Append(" MOCH.SHORT_NAME AS MOCHIKOMISAKI_NAME,");
            selectQuery.Append(" ST.STEEL_TYPE_NAME,");
            selectQuery.Append(" HIKI.SHORT_NAME AS HTSAKI_NAME,");
            selectQuery.Append(" MUKE.SHORT_NAME AS MUKESAKI_NAME, ");
            selectQuery.Append(" MATL.DIM_D AS MATL_DIM_D, ");
            selectQuery.Append(" MATL.SHAPE AS MATL_SHAPE, ");
            selectQuery.Append(" MATL.DIM_W AS MATL_DIM_W, ");
            selectQuery.Append(" MATL.DIM_OTHER AS MATL_DIM_OTHER, ");
            //selectQuery.Append(" MATL.MATL_KUBUN AS MATL_TYPE_CBK, ");
            //selectQuery.Append(" MATL.MATL_SPECIAL_KUBUN AS MATL_SPECIAL_KUBUN_INT, "); 
            selectQuery.Append(" MATL.MUKESAKI_CD, ");
            selectQuery.Append(" MATL.SUPPLIER_CD, ");
            selectQuery.Append(" MATL.MOCHIKOMISAKI_CD, ");
            selectQuery.Append(" MATL.HTSAKI_CD, ");
            selectQuery.Append(" MATL.FUTOU_CD, ");
            //selectQuery.Append(" MATLRATE.RATE, ");
            selectQuery.Append(" DECODE(MATL.MATL_KUBUN, 1, 'COIL', 2, 'BAR', 3, '黒皮') AS MATERIAL_TYPE_NAME_CBK, ");
            selectQuery.Append(" DECODE(MATL.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_MATERIAL MATL ");
            selectQuery.Append(" INNER JOIN MST_MATL_NAME_MAKER   MTMAK ON MATL.MATL_NAME_MAKER_CD = MTMAK.MATL_NAME_MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_MAKER MAK ON MAK.MAKER_CD = MTMAK.MAKER_CD ");
            selectQuery.Append(" INNER JOIN MST_INTERNAL_NAME MTINT ON MTMAK.INTERNAL_NAME_CD = MTINT.INTERNAL_NAME_CD ");
            selectQuery.Append(" INNER JOIN MST_STEEL_TYPE ST ON ST.STEEL_TYPE_CD = MTINT.STEEL_TYPE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = MATL.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_SUPPLIER SUP ON SUP.SUPPLIER_CD = MATL.SUPPLIER_CD ");
            selectQuery.Append(" INNER JOIN MST_FUTOU FU ON FU.FUTOU_CD = MATL.FUTOU_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI MOCH ON MOCH.MOCHIKOMISAKI_CD = MATL.MOCHIKOMISAKI_CD ");
            selectQuery.Append(" LEFT JOIN MST_HIKITORISAKI HIKI ON HIKI.HTSAKI_CD = MATL.HTSAKI_CD  ");
            selectQuery.Append(" LEFT JOIN MST_MUKESAKI MUKE ON MUKE.MUKESAKI_CD = MATL.MUKESAKI_CD ");
            //selectQuery.Append(" LEFT JOIN MST_MATERIAL_RATE MATLRATE ON MATLRATE.MATL_NAME_MAKER_CD = MTMAK.MATL_NAME_MAKER_CD ");
            var query = selectQuery.ToString();
            string whereCondition = "";
            whereCondition = " WHERE MATL.MATERIAL_CD = " + MATERIAL_CD + " fetch first row only";
            query = query + whereCondition;
            selectQuery.Append(query);
            result = ExecuteQuerySingleResult<RollSalesForecastDetails>(query.ToString());
            return result;
        }

        public List<RollSalesForecastDetails> GetSpecInput1Details(int cust_Cd, int incharge_Cd, int? current_R, int? current_YYYY, decimal? history_Month1, decimal? history_Month2, decimal? history_Month3)
        {

            List<RollSalesForecastDetails> result = new List<RollSalesForecastDetails>();
            var selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FC.PRODUCT_CD,");
            selectQuery.Append(" PD.PROD_SPEC_NO,");
            selectQuery.Append(" PD.PRODUCT_CD,");
            selectQuery.Append(" PD.PRODUCT_KUBUN,");
            selectQuery.Append(" PD.CUSTOMER_NAME,");
            selectQuery.Append(" PD.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" PD.INTERNAL_NAME_CD,");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST,");
            selectQuery.Append(" PD.MATL_NAME_MAKER AS MATL_MAKER_NAME,");
            selectQuery.Append(" PD.DIM_D AS PROD_DIM_W,");
            selectQuery.Append(" PD.DIM_W AS PROD_DIM_D,");
            selectQuery.Append(" PD.DIM_OTHER AS PROD_DIM_OTHER,");
            selectQuery.Append(" PD.SHAPE,");
            selectQuery.Append(" PD.PRODUCT_LENGTH AS PROD_LENGTH ,");
            selectQuery.Append(" PD.LENGTH_OTHER AS LENGTH_OTHER ,");
            selectQuery.Append(" DIM_TOL,");
            selectQuery.Append(" LEN_TOL ,");
            selectQuery.Append(" PD.MAGARI_KUBUN,");
            selectQuery.Append(" PD.MAGARI_BEND_TOL,");
            selectQuery.Append(" PD.MAGARI_SPAN,");
            selectQuery.Append(" PD.CONDITION_CD AS CONDITION_DESCRIPTION,");
            selectQuery.Append(" PD.MAKER_NAME,");
            selectQuery.Append(" FC.ROLL_FC_KUBUN,");
            selectQuery.Append(" FC.ROLL_FC_R,");
            selectQuery.Append(" FC.ROLL_FC_YEAR,");
            selectQuery.Append(" FC.ROLL_FC_DATE,");
            selectQuery.Append(" FC.EMP_CD,");
            selectQuery.Append(" FC.CUST_CD,");
            selectQuery.Append(" FC.FC_QTY_R1,");
            selectQuery.Append(" FC.FC_QTY_R2,");
            selectQuery.Append(" FC.FC_QTY_R3,");
            selectQuery.Append(" FC.FC_QTY_R4,");
            //selectQuery.Append(" FC.MSG_TO_SHIZAI,");
            selectQuery.Append(" ME.SHORT_NAME     INCHAGE_SHORT_NAME,");
            selectQuery.Append(" ROLL_FC_LOCK_DATE,");
            selectQuery.Append(" PROD_SPEC_NO,");
            selectQuery.Append(" FC.USER_ID,");
            selectQuery.Append(" SUM(DECODE(B.DISPATCH_MONTH,'" + history_Month1 + "', B.DISPATCH_QTY, NULL)) AS DISPATCH_QTY1,");
            selectQuery.Append(" SUM(DECODE(B.DISPATCH_MONTH, '" + history_Month2 + "', B.DISPATCH_QTY, NULL)) AS DISPATCH_QTY2,");
            selectQuery.Append(" SUM(DECODE(B.DISPATCH_MONTH, '" + history_Month3 + "', B.DISPATCH_QTY, NULL)) AS DISPATCH_QTY3,");
            selectQuery.Append(" to_char(");
            selectQuery.Append(" SUM(DECODE(B.DISPATCH_MONTH,'" + history_Month1 + "', B.DISPATCH_QTY, NULL)) + ");
            selectQuery.Append(" SUM(DECODE(B.DISPATCH_MONTH,'" + history_Month2 + "', B.DISPATCH_QTY, NULL)) + ");
            selectQuery.Append(" SUM(DECODE(B.DISPATCH_MONTH,'" + history_Month2 + "', B.DISPATCH_QTY, NULL))");
            selectQuery.Append(" / 3,'FM9990.00') AS AVG_DISPATCH_QTY ");
            // selectQuery.Append(" SUM(bps.PROD_STOCK_QTY)/1000 AS PROD_STOCK_QTY,");
            //selectQuery.Append(" SUM(CASE");
            // selectQuery.Append(" WHEN ROUND(SYSDATE - bps.NYUKA_DATE) > 90 ");
            // selectQuery.Append(" THEN bps.PROD_STOCK_QTY/1000 ");
            // selectQuery.Append("  ELSE 0 ");
            // selectQuery.Append("  END) AS PENDING_STOCK");
            selectQuery.Append(" FROM");
            selectQuery.Append(" V_PRODUCT_DETAILS   PD");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST        FC ON FC.PRODUCT_CD = PD.PRODUCT_CD ");
            selectQuery.Append("  INNER JOIN MST_CUSTOMER   MCI ON FC.CUST_CD = MCI.CUSTOMER_CD ");
            selectQuery.Append("  INNER JOIN MST_EMPLOYEE  ME ON FC.EMP_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append("  LEFT JOIN (");
            selectQuery.Append("  SELECT ");
            selectQuery.Append("  MONTH AS DISPATCH_MONTH, ");
            selectQuery.Append("  ROOT_VER_PRODUCT_KEY, ");
            selectQuery.Append("  NVL(DISPATCH_QTY, 0) / 1000 AS DISPATCH_QTY");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" BCH_MONTH_END_SHIPMENT");
            selectQuery.Append("  WHERE ");
            selectQuery.Append(" YEAR = '" + current_YYYY + "'");
            selectQuery.Append(" AND MONTH IN( " + history_Month1 + "," + history_Month2 + ","  + history_Month3 + ")");
            selectQuery.Append("   ) B ON PD.ROOT_VER_PRODUCT_KEY = B.ROOT_VER_PRODUCT_KEY");
            //selectQuery.Append("  LEFT JOIN BCH_PROD_STOCK BPS ON BPS.PROD_SPEC_CD = PD.PROD_SPEC_NO");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" PD.VERSION = (");
            selectQuery.Append(" SELECT");
            selectQuery.Append(" MAX(V2.VERSION)");
            selectQuery.Append(" FROM");
            selectQuery.Append(" V_PRODUCT_DETAILS V2");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" V2.PRODUCT_CD = PD.PRODUCT_CD");
            selectQuery.Append(" )");
            selectQuery.Append(" AND FC.CUST_CD =" + cust_Cd + "");
            selectQuery.Append(" AND FC.EMP_CD =" + incharge_Cd + "");
            selectQuery.Append(" AND FC.ROLL_FC_KUBUN = " + Constants.Numbers.PRODUCT_KUBUN + "");
            selectQuery.Append(" AND FC.ROLL_FC_YEAR = " + current_YYYY + "");
            selectQuery.Append(" AND FC.ROLL_FC_R = " + current_R + "");
            selectQuery.Append(" GROUP BY");
            selectQuery.Append(" FC.PRODUCT_CD, ");
            selectQuery.Append(" PD.PROD_SPEC_NO,");
            selectQuery.Append(" PD.PRODUCT_CD,");
            selectQuery.Append(" PD.PRODUCT_KUBUN,");
            selectQuery.Append(" PD.CUSTOMER_NAME,");
            selectQuery.Append(" PD.INTERNAL_NAME,");
            selectQuery.Append(" PD.INTERNAL_NAME_CD,");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST,");
            selectQuery.Append(" PD.MATL_NAME_MAKER,");
            selectQuery.Append(" PD.DIM_D,");
            selectQuery.Append(" PD.DIM_W,");
            selectQuery.Append(" PD.DIM_OTHER,");
            selectQuery.Append(" PD.SHAPE,");
            selectQuery.Append(" PD.PRODUCT_LENGTH,");
            selectQuery.Append(" PD.LENGTH_OTHER,");
            selectQuery.Append(" PD.DIM_TOL,");
            selectQuery.Append(" PD.LEN_TOL,");
            selectQuery.Append(" PD.MAGARI_KUBUN,");
            selectQuery.Append(" PD.MAGARI_BEND_TOL,");
            selectQuery.Append(" PD.MAGARI_SPAN,");
            selectQuery.Append(" PD.CONDITION_CD,");
            selectQuery.Append(" PD.MAKER_NAME ,");
            selectQuery.Append(" ROLL_FC_CD,");
            selectQuery.Append(" FC.ROLL_FC_NO,");
            selectQuery.Append(" FC.ROLL_FC_SRLNO,");
            selectQuery.Append(" FC.ROLL_FC_KUBUN,");
            selectQuery.Append(" FC.ROLL_FC_R,");
            selectQuery.Append(" FC.ROLL_FC_YEAR,");
            selectQuery.Append(" FC.ROLL_FC_DATE,");
            selectQuery.Append(" FC.EMP_CD,");
            selectQuery.Append(" FC.CUST_CD,");
            selectQuery.Append(" FC.FC_QTY_R1,");
            selectQuery.Append(" FC.FC_QTY_R2,");
            selectQuery.Append(" FC.FC_QTY_R3,");
            selectQuery.Append(" FC.FC_QTY_R4,");
            selectQuery.Append(" ME.SHORT_NAME,");
            selectQuery.Append(" FC.ROLL_FC_LOCK_DATE,");
            selectQuery.Append(" PD.PROD_SPEC_NO,");
            selectQuery.Append(" FC.USER_ID,");
            selectQuery.Append(" PD.ROOT_VER_PRODUCT_KEY ");
            result = ExecuteQueryMultiResult<RollSalesForecastDetails>(selectQuery.ToString());
            return result;
        }
        public List<RollSalesForecastDetails> GetSpecInput2Details(int cust_Cd, int incharge_Cd, int? current_R, int? current_YYYY)
        {

            List<RollSalesForecastDetails> result = new List<RollSalesForecastDetails>();
            var selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FC.PRODUCT_CD,");
            selectQuery.Append(" PD.PROD_SPEC_NO,");
            selectQuery.Append(" PD.PRODUCT_CD,");
            selectQuery.Append(" PD.PRODUCT_KUBUN,");
            selectQuery.Append(" PD.CUSTOMER_NAME,");
            selectQuery.Append(" PD.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" PD.INTERNAL_NAME_CD,");
            selectQuery.Append(" PD.PRODUCT_NAME_CUST,");
            selectQuery.Append(" PD.MATL_NAME_MAKER AS MATL_MAKER_NAME,");
            selectQuery.Append(" PD.DIM_D AS PROD_DIM_W,");
            selectQuery.Append(" PD.DIM_W AS PROD_DIM_D,");
            selectQuery.Append(" PD.DIM_OTHER AS PROD_DIM_OTHER,");
            selectQuery.Append(" PD.SHAPE,");
            selectQuery.Append(" PD.PRODUCT_LENGTH AS PROD_LENGTH ,");
            selectQuery.Append(" PD.LENGTH_OTHER AS LENGTH_OTHER ,");
            selectQuery.Append(" DIM_TOL,");
            selectQuery.Append(" LEN_TOL ,");
            selectQuery.Append(" PD.MAGARI_KUBUN,");
            selectQuery.Append(" PD.MAGARI_BEND_TOL,");
            selectQuery.Append(" PD.MAGARI_SPAN,");
            selectQuery.Append(" PD.CONDITION_CD AS CONDITION_DESCRIPTION,");
            selectQuery.Append(" PD.MAKER_NAME,");
            selectQuery.Append(" FC.ROLL_FC_CD,");
            selectQuery.Append(" FC.ROLL_FC_NO ,");
            selectQuery.Append(" FC.ROLL_FC_SRLNO,");
            selectQuery.Append(" FC.ROLL_FC_KUBUN,");
            selectQuery.Append(" FC.ROLL_FC_R,");
            selectQuery.Append(" FC.ROLL_FC_YEAR,");
            selectQuery.Append(" FC.ROLL_FC_DATE,");
            selectQuery.Append(" FC.EMPLOYEE_CD,");
            selectQuery.Append(" FC.CUSTOMER_CD,");
            selectQuery.Append(" FC.FC_QTY_R1,");
            selectQuery.Append(" FC.FC_QTY_R2,");
            selectQuery.Append(" FC.FC_QTY_R3,");
            selectQuery.Append(" FC.FC_QTY_R4,");
            // selectQuery.Append(" FC.MSG_TO_SHIZAI,");
            selectQuery.Append(" ME.SHORT_NAME     INCHAGE_SHORT_NAME,");
            selectQuery.Append(" ROLL_FC_LOCK_DATE,");
            selectQuery.Append(" PROD_SPEC_NO,");
            selectQuery.Append(" FC.USER_ID");
            // selectQuery.Append(" BKI.KAKOIRAI_REM_QTY / 1000 AS KAKOIRAI_REM_QTY,");
            selectQuery.Append(" FROM");
            selectQuery.Append(" V_PRODUCT_DETAILS   PD");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST        FC ON FC.PRODUCT_CD = PD.PRODUCT_CD");
            //selectQuery.Append(" LEFT OUTER JOIN BCH_KAKOIRAI_INFO   BKI ON PD.PROD_SPEC_CD = BKI.PROD_SPEC_CD");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER   MCI ON FC.CUSTOMER_CD = MCI.CUSTOMER_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE        ME ON FC.EMPLOYEE_CD = ME.EMPLOYEE_CD");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" PD.VERSION = (");
            selectQuery.Append(" SELECT");
            selectQuery.Append(" MAX(VERSION)");
            selectQuery.Append(" FROM");
            selectQuery.Append(" V_PRODUCT_DETAILS");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" PD.PRODUCT_CD = PRODUCT_CD");
            selectQuery.Append(" )");
            selectQuery.Append(" AND FC.CUSTOMER_CD =" + cust_Cd + "");
            selectQuery.Append(" AND FC.EMPLOYEE_CD =" + incharge_Cd + "");
            selectQuery.Append(" AND FC.ROLL_FC_KUBUN = " + Constants.Numbers.PRODUCT_KUBUN);
            selectQuery.Append(" AND PD.IN_ACTIVE_FLAG = " + 0 + "");
            selectQuery.Append(" AND FC.ROLL_FC_YEAR = " + current_YYYY + "");
            selectQuery.Append(" AND FC.ROLL_FC_R = " + current_R + "");
            result = ExecuteQueryMultiResult<RollSalesForecastDetails>(selectQuery.ToString());
            return result;
        }
        public int IsForecastDataPresentCustomerWise(int cust_CD, int last_R, int last_YYYY)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(ROLL_FC_CD) FROM TRN_SALES_FORECAST");
            selectQuery.Append("  WHERE ");
            selectQuery.Append("   CUST_CD =" + cust_CD);
            selectQuery.Append("  AND ROLL_FC_R =" + last_R);
            selectQuery.Append("  And ROLL_FC_YEAR =" + last_YYYY);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;

        }

        public List<RollSalesForecastDetails> GetSpecInput1MaterailaDetails(int cust_Cd, int incharge_Cd, int? current_R, int? current_YYYY)
        {
            List<RollSalesForecastDetails> result = new List<RollSalesForecastDetails>();
            var selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" FC.ROLL_FC_KUBUN,");
            selectQuery.Append(" MD.MATL_MAKER_NAME,");
            selectQuery.Append(" MD.MATL_KUBUN,");
            selectQuery.Append(" MD.DIM_D AS MATL_DIM_D,");
            selectQuery.Append(" MD.SHAPE AS MATL_SHAPE,");
            selectQuery.Append(" MD.DIM_W AS MATL_DIM_W,");
            selectQuery.Append(" MD.DIM_OTHER AS MATL_DIM_OTHER,");
            selectQuery.Append(" MD.MATERIAL_LENGTH,");
            selectQuery.Append(" MD.MATERIAL_CD,");
            selectQuery.Append(" MD.MAKER_NAME   AS MAKER_NAME,");
            selectQuery.Append(" MD.INTERNAL_NAME ,");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION ,");
            selectQuery.Append(" MD.MOCHIKOMISAKI_CD,");
            selectQuery.Append(" MKSAKI.SHORT_NAME ,");
            selectQuery.Append(" 0 AS ORDER_REM_QTY,");
            selectQuery.Append(" FC.ROLL_FC_NO,");
            selectQuery.Append(" FC.ROLL_FC_SRLNO,");
            selectQuery.Append(" FC.ROLL_FC_CD,");
            selectQuery.Append(" FC.ROLL_FC_DATE,");
            selectQuery.Append(" FC.ROLL_FC_R,");
            selectQuery.Append(" FC.ROLL_FC_YEAR,");
            selectQuery.Append(" FC.EMP_CD,");
            selectQuery.Append(" FC.CUST_CD,");
            selectQuery.Append(" FC.FC_QTY_R1,");
            selectQuery.Append(" FC.FC_QTY_R2,");
            selectQuery.Append(" FC.FC_QTY_R3,");
            selectQuery.Append(" FC.FC_QTY_R4,");
            // selectQuery.Append(" FC.MSG_TO_SHIZAI,");
            selectQuery.Append(" ME.SHORT_NAME     INCHAGE_SHORT_NAME,");
            selectQuery.Append(" MD.INTERNAL_NAME_CD,");
            selectQuery.Append(" MD.CONDITION_CD,");
            selectQuery.Append(" ROLL_FC_LOCK_DATE,");
            selectQuery.Append(" FC.USER_ID");
            selectQuery.Append(" FROM");
            selectQuery.Append(" V_HT_MATL_DETAILS   MD");
            selectQuery.Append(" INNER JOIN TRN_SALES_FORECAST        FC ON FC.MATERIAL_CD = MD.MATERIAL_CD ");
            selectQuery.Append(" LEFT OUTER JOIN MST_CONDITION       CND ON MD.CONDITION_CD = CND.CONDITION_CD ");
            selectQuery.Append(" LEFT OUTER JOIN MST_MOCHIKOMISAKI   MKSAKI ON MD.MOCHIKOMISAKI_CD = MKSAKI.MOCHIKOMISAKI_CD ");
            selectQuery.Append(" JOIN MST_CUSTOMER   CS ON FC.CUST_CD = CS.CUSTOMER_CD");
            selectQuery.Append(" JOIN MST_DEPARTMENT_GROUP DG ON CS.DEPARTMENT_GROUP_CD= DG.EMPLOYEE_CD ");
            selectQuery.Append(" JOIN MST_EMPLOYEE        ME ON DG.EMPLOYEE_CD = ME.EMPLOYEE_CD ");
            selectQuery.Append(" WHERE");
            selectQuery.Append(" FC.ROLL_FC_KUBUN=" + Constants.Numbers.MATERIAL_KUBUN);
            selectQuery.Append(" AND md.in_active_flag = 0");
            selectQuery.Append(" AND FC.CUST_CD = " + cust_Cd + "");
            selectQuery.Append(" AND FC.EMP_CD = " + incharge_Cd + "");
            selectQuery.Append(" AND FC.ROLL_FC_YEAR = " + current_YYYY + "");
            selectQuery.Append(" AND FC.ROLL_FC_R = " + current_R + "");
            result = ExecuteQueryMultiResult<RollSalesForecastDetails>(selectQuery.ToString());
            return result;
        }

        #region Save And Update Material,Product Data
        /// <summary>
        /// CREATED BY : Komal Kadam on 2025/04/16 
        /// Screen Name: RollSalesForecast
        /// </summary>
        /// <param name="rollSalesForecastDetails"></param>
        /// <returns>Save Product and Material Data</returns>
        public RollSalesForecastDetails SaveProductMaterialForecastDetails(RollSalesForecastDetails rollSalesForecastDetails)
        {

            RollSalesForecastDetails result = new RollSalesForecastDetails();
            try
            {
                // Save Product Data
                long commonRollFcNo = 0;
                if (rollSalesForecastDetails.PRODUCT_LIST != null && rollSalesForecastDetails.PRODUCT_LIST.Count > 0)
                {
                    var selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT SALES_FC_NO_SEQ.NEXTVAL FROM DUAL ");
                    commonRollFcNo = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                   
                    foreach (var productData in rollSalesForecastDetails.PRODUCT_LIST)
                      {

                        if (productData.ROLL_FC_CD == null)
                        {
                            if (productData.ROLL_FC_NO == null)
                            {
                                productData.ROLL_FC_NO = commonRollFcNo;
                                InsertProductData(productData, rollSalesForecastDetails.HEADER);
                            }
                            else
                            {
                               
                                InsertProductData(productData, rollSalesForecastDetails.HEADER);
                            }
                            
                        }
                        else
                        {
                         UpdateProductData(productData, rollSalesForecastDetails.HEADER);
                            
                        }
                        
                        
                    }
                     
                }

                //Save Material Data
                
                if (rollSalesForecastDetails.MATERIAL_LIST != null || rollSalesForecastDetails.MATERIAL_LIST.Count > 0  )
                {
                   
                    foreach (var materialData in rollSalesForecastDetails.MATERIAL_LIST)
                    {

                        if (materialData.ROLL_FC_CD == null)
                        {
                            
                                InsertMaterialData(materialData, rollSalesForecastDetails.HEADER, commonRollFcNo);
                          

                        }
                        else
                        {
                            UpdateMaterialData(materialData, rollSalesForecastDetails.HEADER);

                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return rollSalesForecastDetails;
        }

        private RollSalesForecastDetails InsertProductData(RollSalesForecastDetails productData, RollSalesForecastDetails headerData)
        {
            if (productData.PRODUCT_CD != null)
            {
                StringBuilder insertQuery = new StringBuilder();


                insertQuery.Append("INSERT INTO TRN_SALES_FORECAST (");
                insertQuery.Append("ROLL_FC_CD, ");
                insertQuery.Append("ROLL_FC_NO, ");
                insertQuery.Append("ROLL_FC_SRLNO, ");
                insertQuery.Append("EMP_CD, ");
                insertQuery.Append("CUST_CD, ");
                insertQuery.Append("ROLL_FC_YEAR, ");
                insertQuery.Append("ROLL_FC_R, ");
                insertQuery.Append("ROLL_FC_KUBUN, ");
                insertQuery.Append("PRODUCT_CD, ");
                insertQuery.Append("WIP_STOCK, ");
                insertQuery.Append("PROD_STOCK, ");
                insertQuery.Append("ROLL_FC_DATE, ");
                insertQuery.Append("LAST1_MNTH_DISPACH_QTY, ");
                insertQuery.Append("LAST2_MNTH_DISPACH_QTY, ");
                insertQuery.Append("LAST3_MNTH_DISPACH_QTY, ");
                insertQuery.Append("FC_QTY_R1, ");
                insertQuery.Append("FC_QTY_R2, ");
                insertQuery.Append("FC_QTY_R3, ");
                insertQuery.Append("FC_QTY_R4, ");
                insertQuery.Append("MSG_TO_MATL_DEPT, ");
                insertQuery.Append("SALES_FC_DONE_DATE, ");
                insertQuery.Append("ROLL_FC_LOCK_DATE, ");
                insertQuery.Append("USER_ID, ");
                insertQuery.Append("CREATED_DATE, ");
                insertQuery.Append("UPDATED_DATE ");
                insertQuery.Append(") VALUES (");
                insertQuery.Append("SALES_FORECAST_SEQ.NEXTVAL, ");
                insertQuery.Append("'" + productData.ROLL_FC_NO + "', ");
                insertQuery.Append("'" + productData.FC_SRLNO + "', ");
                insertQuery.Append("'" + headerData.EMPLOYEE_CD + "', ");
                insertQuery.Append("'" + headerData.CUSTOMER_CD + "', ");
                insertQuery.Append("'" + headerData.ROLL_FC_YEAR + "', ");
                insertQuery.Append("'" + headerData.ROLL_FC_R + "', ");
                insertQuery.Append("'" + Constants.Numbers.PRODUCT_KUBUN + "', ");
                insertQuery.Append("'" + productData.PRODUCT_CD + "', ");
               
                
                insertQuery.Append("'" + productData.WIP_STOCK + "', ");
                insertQuery.Append("'" + productData.PROD_STOCK + "', ");

                // Safe Date Handling
                if (!string.IsNullOrEmpty(headerData.ROLL_FC_DATE_STRING))
                {
                   
                    insertQuery.Append("TO_DATE('" + Convert.ToDateTime(headerData.ROLL_FC_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");
                }
                else
                {
                    insertQuery.Append("NULL, ");
                }

                insertQuery.Append("'" + productData.DISPATCH_QTY1 + "', ");
                insertQuery.Append("'" + productData.DISPATCH_QTY2 + "', ");
                insertQuery.Append("'" + productData.DISPATCH_QTY3 + "', ");
                insertQuery.Append("'" + productData.FC_QTY_R1 + "', ");
                insertQuery.Append("'" + productData.FC_QTY_R2 + "', ");
                insertQuery.Append("'" + productData.FC_QTY_R3 + "', ");
                insertQuery.Append("'" + productData.FC_QTY_R4 + "', ");
               
                insertQuery.Append("'" + productData.REMARKS + "', ");

                // SALES_FC_DONE_DATE
                if (!string.IsNullOrEmpty(headerData.SALES_FC_DONE_DATE_STRING))
                {
                    insertQuery.Append(" TO_DATE('" + Convert.ToDateTime(headerData.SALES_FC_DONE_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");
                }
                else
                {
                    insertQuery.Append("NULL, ");
                }
                if (!string.IsNullOrEmpty(headerData.ROLL_FC_LOCK_DATE_STRING))
                {
                    insertQuery.Append(" TO_DATE('" + Convert.ToDateTime(headerData.ROLL_FC_LOCK_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS'),");
                }
                else
                {
                    insertQuery.Append("NULL, ");
                }
                insertQuery.Append("'" + headerData.USER_ID + "',");
                insertQuery.Append(" SYSDATE ,");
                insertQuery.Append(" SYSDATE  ");
                insertQuery.Append(") ");
                insertQuery.Append(" RETURNING ROLL_FC_CD INTO : OUT_CD");
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand(insertQuery.ToString(), con))
                    {
                        OracleParameter outCd = new OracleParameter("OUT_CD", OracleDbType.Int64);
                        outCd.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outCd);
                        

                        con.Open();
                        cmd.ExecuteNonQuery();

                        // ✅ ASSIGN BACK TO OBJECT
                        productData.ROLL_FC_CD = Convert.ToInt64(outCd.Value.ToString());
                       
                    }
                    // ExecuteNonQuery(insertQuery.ToString());
                }




            }
            return productData;
        }
        private RollSalesForecastDetails UpdateProductData(RollSalesForecastDetails productData,RollSalesForecastDetails headerData)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_SALES_FORECAST SET ");
            updateQuery.Append("CUST_CD = '" + headerData.CUSTOMER_CD + "', ");
            updateQuery.Append("EMP_CD = '" + headerData.EMPLOYEE_CD + "', ");
            if (!string.IsNullOrEmpty(headerData.SALES_FC_DONE_DATE_STRING))
            {
                updateQuery.Append("SALES_FC_DONE_DATE = TO_DATE('" +Convert.ToDateTime(headerData.SALES_FC_DONE_DATE_STRING).ToString("yyyy/MM/dd HH:mm:ss") +  "','YYYY/MM/DD HH24:MI:SS')," );
            }
            else
            {
                updateQuery.Append("SALES_FC_DONE_DATE = NULL,");
            }
            updateQuery.Append("WIP_STOCK = '" + productData.WIP_STOCK + "', ");
            updateQuery.Append("PROD_STOCK = '" + productData.PROD_STOCK + "', ");
            updateQuery.Append("LAST1_MNTH_DISPACH_QTY = '" + productData.DISPATCH_QTY1 + "', ");
            updateQuery.Append("LAST2_MNTH_DISPACH_QTY = '" + productData.DISPATCH_QTY2 + "', ");
            updateQuery.Append("LAST3_MNTH_DISPACH_QTY = '" + productData.DISPATCH_QTY3 + "', ");
            updateQuery.Append("FC_QTY_R1 = '" + productData.FC_QTY_R1 + "', ");
            updateQuery.Append("FC_QTY_R2 = '" + productData.FC_QTY_R2 + "', ");
            updateQuery.Append("FC_QTY_R3 = '" + productData.FC_QTY_R3 + "', ");
            updateQuery.Append("FC_QTY_R4 = '" + productData.FC_QTY_R4 + "', ");
            updateQuery.Append("MSG_TO_MATL_DEPT = '" + productData.REMARKS + "', ");
            updateQuery.Append("UPDATED_DATE = SYSDATE ");
            updateQuery.Append("WHERE ");
            updateQuery.Append("ROLL_FC_CD = '" + productData.ROLL_FC_CD + "' ");
            ExecuteNonQuery(updateQuery.ToString());
            return productData;
        }

        private RollSalesForecastDetails InsertMaterialData(RollSalesForecastDetails materialData, RollSalesForecastDetails headerData, long commonMaterialRollFcNo)
        {
            if (materialData.MATERIAL_CD != null)
            {

                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append(" INSERT INTO TRN_SALES_FORECAST(");
                insertQuery.Append(" ROLL_FC_CD,");
                insertQuery.Append(" ROLL_FC_NO,");
                insertQuery.Append(" ROLL_FC_SRLNO,");
                insertQuery.Append(" EMP_CD,");
                insertQuery.Append(" CUST_CD,");
                insertQuery.Append(" ROLL_FC_YEAR,");
                insertQuery.Append(" ROLL_FC_R,");
                insertQuery.Append(" ROLL_FC_KUBUN,");
                insertQuery.Append(" MATERIAL_CD,");
                insertQuery.Append(" ROLL_FC_DATE, ");
                insertQuery.Append(" FC_QTY_R1, ");
                insertQuery.Append(" FC_QTY_R2, ");
                insertQuery.Append(" FC_QTY_R3, ");
                insertQuery.Append(" FC_QTY_R4, ");
                insertQuery.Append(" MSG_TO_MATL_DEPT ,");
                insertQuery.Append(" SALES_FC_DONE_DATE, ");
                insertQuery.Append(" ROLL_FC_LOCK_DATE, ");
                insertQuery.Append(" USER_ID, ");
                insertQuery.Append(" CREATED_DATE, ");
                insertQuery.Append(" UPDATED_DATE ");
                insertQuery.Append(" ) ");
                insertQuery.Append(" VALUES(");
                insertQuery.Append(" SALES_FORECAST_SEQ.NEXTVAL");
                insertQuery.Append(" ,'" + commonMaterialRollFcNo +"'");
                insertQuery.Append(",'" + materialData.FC_SRLNO + "'");
                insertQuery.Append(",'" + headerData.EMPLOYEE_CD + "'");
                insertQuery.Append(",'" + headerData.CUSTOMER_CD + "'");
                insertQuery.Append(",'" + headerData.ROLL_FC_YEAR + "'");
                insertQuery.Append(",'" + headerData.ROLL_FC_R + "'");
                insertQuery.Append(",'" + Constants.Numbers.MATERIAL_KUBUN + "'");
                insertQuery.Append(",'" + materialData.MATERIAL_CD + "'");
                if (!string.IsNullOrEmpty(headerData.ROLL_FC_DATE_STRING))
                {

                    insertQuery.Append(",TO_DATE('" + Convert.ToDateTime(headerData.ROLL_FC_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                }
                else
                {
                    insertQuery.Append(",NULL");
                }
                insertQuery.Append(",'" + materialData.FC_QTY_R1 + "'");
                insertQuery.Append(",'" + materialData.FC_QTY_R2 + "'");
                insertQuery.Append(",'" + materialData.FC_QTY_R3 + "'");
                insertQuery.Append(",'" + materialData.FC_QTY_R4 + "'");
                insertQuery.Append(",'" + materialData.REMARKS + "'");
                if (!string.IsNullOrEmpty(headerData.SALES_FC_DONE_DATE_STRING))
                {
                    insertQuery.Append("TO_DATE('" + Convert.ToDateTime(headerData.SALES_FC_DONE_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                }
                else
                {
                    insertQuery.Append(",NULL");
                }
                if (!string.IsNullOrEmpty(headerData.ROLL_FC_LOCK_DATE_STRING))
                {
                    insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(headerData.ROLL_FC_LOCK_DATE_STRING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                }
                else
                {
                    insertQuery.Append(",NULL ");
                }
                insertQuery.Append(",'" + headerData.USER_ID + "'");
                insertQuery.Append(",SYSDATE ");
                insertQuery.Append(",SYSDATE ");
                insertQuery.Append(") ");
                // ExecuteNonQuery(insertQuery.ToString());
                insertQuery.Append(" RETURNING ROLL_FC_CD INTO : OUT_CD");
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    using (OracleCommand cmd = new OracleCommand(insertQuery.ToString(), con))
                    {
                        OracleParameter outCd = new OracleParameter("OUT_CD", OracleDbType.Int64);
                        outCd.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outCd);


                        con.Open();
                        cmd.ExecuteNonQuery();

                        // ✅ ASSIGN BACK TO OBJECT
                        materialData.ROLL_FC_CD = Convert.ToInt64(outCd.Value.ToString());

                    }
                    // ExecuteNonQuery(insertQuery.ToString());
                }


            }
            return materialData;
        }

        private RollSalesForecastDetails UpdateMaterialData(RollSalesForecastDetails materialData, RollSalesForecastDetails headerData)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_SALES_FORECAST SET ");
            updateQuery.Append("CUST_CD = '" + headerData.CUSTOMER_CD + "', ");
            updateQuery.Append("EMP_CD = '" + headerData.EMPLOYEE_CD + "', ");
            if (!string.IsNullOrEmpty(headerData.SALES_FC_DONE_DATE_STRING))
            {
                updateQuery.Append("SALES_FC_DONE_DATE = TO_DATE('" + Convert.ToDateTime(headerData.SALES_FC_DONE_DATE_STRING).ToString("yyyy/MM/dd HH:mm:ss") + "','YYYY/MM/DD HH24:MI:SS'),");
            }
            else
            {
                updateQuery.Append("SALES_FC_DONE_DATE = NULL,");
            }
            updateQuery.Append("FC_QTY_R1 = '" + materialData.FC_QTY_R1 + "', ");
            updateQuery.Append("FC_QTY_R2 = '" + materialData.FC_QTY_R2 + "', ");
            updateQuery.Append("FC_QTY_R3 = '" + materialData.FC_QTY_R3 + "', ");
            updateQuery.Append("FC_QTY_R4 = '" + materialData.FC_QTY_R4 + "', ");
            updateQuery.Append("MSG_TO_MATL_DEPT = '" + materialData.REMARKS + "', ");
            updateQuery.Append("UPDATED_DATE = SYSDATE ");
            updateQuery.Append("WHERE ");
            updateQuery.Append("ROLL_FC_CD = '" + materialData.ROLL_FC_CD + "' ");
            ExecuteNonQuery(updateQuery.ToString());
            return materialData;
        }
        #endregion

        /// <summary>
        /// CREATED BY : Komal Kadam on 2025/07/09 
        /// Screen Name: RollPoForecast(List Page)
        /// Bind  Data for Product And Material  
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="sortCriteria"></param>
        /// <returns></returns>
        public List<RollSalesForecastDetails> GetProductForecastDetails(List<SearchField> searchCriteria, string sortCriteria)
        {
            List<RollSalesForecastDetails> rollSalesForecastDetails = new List<RollSalesForecastDetails>();
            var selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" SF.ROLL_FC_CD,");
            selectQuery.Append(" SF.ROLL_FC_NO,");
            selectQuery.Append(" SF.ROLL_FC_SRLNO,");
            selectQuery.Append(" SF.EMP_CD,");
            selectQuery.Append(" SF.CUST_CD,");
            selectQuery.Append(" SF.ROLL_FC_YEAR,");
            selectQuery.Append(" SF.ROLL_FC_R,");
            selectQuery.Append(" SF.ROLL_FC_KUBUN,");
            selectQuery.Append(" SF.MATERIAL_CD,");
            selectQuery.Append(" SF.INT_MATL_FC_SUMMARY_CD,");
            selectQuery.Append(" SF.WIP_STOCK,");
            selectQuery.Append(" SF.PROD_STOCK,");
            selectQuery.Append(" SF.ROLL_FC_DATE,");
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
            selectQuery.Append(" SF.MSG_TO_MATL_DEPT AS REMARKS,");
            selectQuery.Append(" SF.REPLY_FROM_MATL_DEPT,");
            selectQuery.Append(" SF.SALES_FC_DONE_DATE,");
            selectQuery.Append(" SF.ROLL_FC_LOCK_DATE,");
            selectQuery.Append(" PRD.PRODUCT_KUBUN,");
            selectQuery.Append(" PRD.PROD_SPEC_NO   AS PROD_SPEC_NO,");
            selectQuery.Append(" CUST.SHORT_NAME  AS CUSTOMER_NAME,");
            selectQuery.Append(" PRD.CONDITION_DESCRIPTION AS CONDITION_DESCRIPTION,");
            selectQuery.Append(" PRD.PRODUCT_NAME_CUST,");
            selectQuery.Append(" PRD.MAKER_NAME AS MAKER_NAME,");
            selectQuery.Append(" PRD.MATL_NAME_MAKER AS MATL_MAKER_NAME,");
            selectQuery.Append(" PRD.SHAPE,");
            selectQuery.Append(" PRD.DIM_D AS PROD_DIM_D,");
            selectQuery.Append(" PRD.DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" PRD.DIM_W AS PROD_DIM_W,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_D, NULL, NULL, NULL) AS DIM_D_STRING,");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PRD.DIM_W, NULL, NULL, NULL) AS DIM_W_STRING,");
            selectQuery.Append(" PRD.DIM_OTHER AS PROD_DIM_OTHER,");
            selectQuery.Append(" PRD.REMARKS,");
            selectQuery.Append(" PRD.INTERNAL_NAME AS INTERNAL_NAME_STRING,");
            selectQuery.Append(" PRD.LENGTH_OTHER,");
            selectQuery.Append(" PRD.MAGARI_KUBUN,");
            selectQuery.Append(" SF.REPLY_FROM_MATL_DEPT");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_SALES_FORECAST SF ");
            selectQuery.Append(" LEFT JOIN V_PRODUCT_DETAILS   PRD ON SF.PRODUCT_CD = PRD.PRODUCT_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER        CUST ON SF.CUST_CD = CUST.CUSTOMER_CD");
            selectQuery.Append(" WHERE ");
            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = "ROLL_FC_KUBUN=" + Constants.Numbers.PRODUCT_KUBUN + " AND ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {

                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = "  AND  ";

                }

                query = query + whereCondition;
            }

            if (!string.IsNullOrEmpty(sortCriteria))
            {
                query = Helper.ApplySortCriteria(sortCriteria, query);
            }
            rollSalesForecastDetails = ExecuteQueryMultiResult<RollSalesForecastDetails>(query.ToString());
            return rollSalesForecastDetails;


        }

        public List<RollSalesForecastDetails> GetMaterialForecastDetails(List<SearchField> searchCriteria, string sortCriteria)
        {
            List<RollSalesForecastDetails> rollSalesForecastDetails = new List<RollSalesForecastDetails>();
            var selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" SF.MATERIAL_CD,");
            selectQuery.Append(" SF.ROLL_FC_KUBUN,");
            selectQuery.Append(" MD.MATL_MAKER_NAME,");
            selectQuery.Append(" MD.MATL_KUBUN,");
            selectQuery.Append(" MD.DIM_D AS MATL_DIM_D,");
            selectQuery.Append(" MD.SHAPE AS MATL_SHAPE,");
            selectQuery.Append(" MD.DIM_W AS MATL_DIM_W,");
            selectQuery.Append(" MD.DIM_OTHER AS MATL_DIM_OTHER,");
            selectQuery.Append(" MD.MATERIAL_LENGTH,");
            selectQuery.Append(" MD.MAKER_NAME   AS MAKER_NAME,");
            selectQuery.Append(" MD.INTERNAL_NAME as INTERNAL_NAME_STRING,");
            selectQuery.Append(" CND.CONDITION_DESCRIPTION ,");
            selectQuery.Append(" MD.MOCHIKOMISAKI_CD,");
            selectQuery.Append(" MKSAKI.SHORT_NAME AS MOCHIKOMISAKI_NAME,");
            selectQuery.Append(" SF.WIP_STOCK,");
            selectQuery.Append(" SF.PROD_STOCK,");
            selectQuery.Append(" SF.ROLL_FC_DATE,");
            selectQuery.Append(" SF.ROLL_FC_R,");
            selectQuery.Append(" SF.ROLL_FC_YEAR,");
            selectQuery.Append(" SF.EMP_CD,");
            selectQuery.Append(" SF.CUST_CD,");
            selectQuery.Append(" SF.FC_QTY_R1,");
            selectQuery.Append(" SF.FC_QTY_R2,");
            selectQuery.Append(" SF.FC_QTY_R3,");
            selectQuery.Append(" SF.FC_QTY_R4,");
            selectQuery.Append(" SF.MSG_TO_MATL_DEPT as REMARKS,");
            selectQuery.Append(" SF.REPLY_FROM_MATL_DEPT,");
            selectQuery.Append(" SF.SALES_FC_DONE_DATE,");
            selectQuery.Append(" ME.SHORT_NAME     INCHAGE_SHORT_NAME,");
            selectQuery.Append(" MD.INTERNAL_NAME_CD,");
            selectQuery.Append(" DECODE(MD.MATL_SPECIAL_KUBUN, 1, '汎用', 2, '特別') AS MATL_SPECIAL_KUBUN, ");
            selectQuery.Append(" MD.CONDITION_CD,");
            selectQuery.Append(" ROLL_FC_LOCK_DATE,");
            selectQuery.Append(" CUST.SHORT_NAME AS CUSTOMER_NAME");
            selectQuery.Append(" FROM");
            selectQuery.Append(" TRN_SALES_FORECAST SF ");
            selectQuery.Append(" LEFT JOIN V_HT_MATL_DETAILS MD ON SF.MATERIAL_CD = MD.MATERIAL_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION  CND ON MD.CONDITION_CD = CND.CONDITION_CD ");
            selectQuery.Append(" LEFT JOIN MST_MOCHIKOMISAKI   MKSAKI ON MD.MOCHIKOMISAKI_CD = MKSAKI.MOCHIKOMISAKI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER   CUST ON SF.CUST_CD = CUST.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DG ON CUST.DEPARTMENT_GROUP_CD= DG.EMPLOYEE_CD ");
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
            rollSalesForecastDetails = ExecuteQueryMultiResult<RollSalesForecastDetails>(query.ToString());
            return rollSalesForecastDetails;


        }
    }
}
