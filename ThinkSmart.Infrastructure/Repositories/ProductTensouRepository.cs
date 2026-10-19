using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProductTensouRepository : BaseRepository, IProductTensouRepository
    {
        private readonly string _connectionString;
        private object _sortField;
        private readonly LocalizationServices _localization;
        private readonly IConfiguration _configuration;
        private string postHT_Seq;
        public ProductTensouRepository(IConfiguration configuration, LocalizationServices localization) : base(configuration)
        {
            _localization = localization;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }

        /// <summary>
        /// CREATED BY : Shobhana on 2026/09/11       
        /// used screens: Product tensou
        /// </summary>
        /// <returns>
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


        public List<GridSearchFields> GetProductDetailsByProductCD(string productCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<GridSearchFields> result = new List<GridSearchFields>();
            selectQuery.Append(" SELECT ");

            selectQuery.Append(" CST.CUSTOMER_NAME ");
            selectQuery.Append(" ,SHU.PRODUCT_NAME_CUST ");
            selectQuery.Append(" ,SHU.PROD_DIM_D ");
            selectQuery.Append(" ,SHU.PRODUCT_LENGTH AS PROD_LENGTH ");
            selectQuery.Append(" ,SHU.CHARGE ");
            selectQuery.Append(" ,SHU.NO_OF_UNITS ");
            selectQuery.Append(" ,SHU.UNIT ");
            selectQuery.Append(" ,SHU.QUANTITY AS ACT_ALLOC_QTY ");
            selectQuery.Append(" ,SHU.RATE ");
            selectQuery.Append(" ,SHU.PROD_SHAPE ");
            selectQuery.Append(" ,CON.CONDITION_DESCRIPTION ");
            selectQuery.Append(" ,SHU.USER_ID ");
            selectQuery.Append(",GET_DIM_D_W_SHAPE_OTHER('',SHU.PROD_DIM_W, '', SHU.PROD_DIM_OTHER) AS PROD_DIM_OTHER_STRING");
           
            selectQuery.Append(" FROM TRN_SHUKKA_INSTRUCTION SHU");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PD on PD.PRODUCT_CD = SHU.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST on SHU.CUSTOMER_CD = CST.CUSTOMER_CD ");

            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON SHU.CONDITION_CD = CON.CONDITION_CD ");
            //selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP  ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");


            selectQuery.Append(" WHERE SHU.PRODUCT_CD = " + productCD);

            result = ExecuteQueryMultiResult<GridSearchFields>(selectQuery.ToString());
            return result;
        }


        public int SaveWIPTensouDetails(WIPTensouDetails wipTensou)
        {
            WIPTensouDetails wip = new WIPTensouDetails();
            StringBuilder insertQuery = new StringBuilder();
            var now = DateTime.Now;
            string time = "00:00:00";
            string dateTime = wipTensou.TRANSFER_DATE_STRING + " " + time;
            DateTime transferDate = Convert.ToDateTime(wipTensou.TRANSFER_DATE_STRING + " " + time);
           
            DateTime acceptancePlanDate = Convert.ToDateTime(wipTensou.ACCEPT_PLANNED_DATE + " " + time);
            if (transferDate == acceptancePlanDate)
            {
                acceptancePlanDate = acceptancePlanDate.AddMinutes(1);
            }
            string AcceptancePlanDate = acceptancePlanDate.ToString("yyyy/MM/dd HH:mm:ss");

            insertQuery.Append("INSERT INTO TRN_WIP_AND_PROD_TRANSFER (");
            insertQuery.Append("TRANSFER_CD, ");
            insertQuery.Append("TRANSFER_GROUP_NO, ");
            insertQuery.Append("PROCESS_PLAN_CD, ");
            insertQuery.Append("PROCESS_KUBUN_CD, ");
            insertQuery.Append("NIFUDA_CD, ");
            insertQuery.Append("LOCATION_CD_FROM, ");
            insertQuery.Append("TRANSIT_LOCATION_CD, ");
            insertQuery.Append("LOCATION_CD_TO, ");
            insertQuery.Append("START_VEHICLEL_CD, ");
            insertQuery.Append("RELAY_VEHICLEL_CD, ");
            insertQuery.Append("TRANSFER_DATE, ");
            insertQuery.Append("TRANSIT_PLANNED_DATE, ");
            insertQuery.Append("ACCEPTANCE_DATE, ");
            insertQuery.Append("ACCEPT_PLANNED_DATE,");
            insertQuery.Append("TRANSIT_TYPE, ");
            insertQuery.Append("TABA_NO_1, ");
            insertQuery.Append("TABA_NO_2, ");
            insertQuery.Append("TABA_NO_3, ");
            insertQuery.Append("TABA_NO_4, ");
            insertQuery.Append("NO_OF_UNITS, ");
            insertQuery.Append("UNIT_TYPE, ");
            insertQuery.Append("QTY, ");
            insertQuery.Append("MSG_CD, ");
            insertQuery.Append("PLANNED_NEXT_PROSESS, ");
            insertQuery.Append("USER_ID, ");
            insertQuery.Append("INPUT_LOCATION_CD, ");
            insertQuery.Append("CREATED_DATE, ");
            insertQuery.Append("UPDATED_DATE ");
            insertQuery.Append(") VALUES (");
            insertQuery.Append("TRN_WIP_AND_PROD_TRANSFER_SEQ.NEXTVAL, ");
            insertQuery.Append("'" + wipTensou.WIP_TRANSFER_GROUP_NO + "', ");
            insertQuery.Append("NULL, "); //insertQuery.Append("'" + wipTensou.PROCESS_PLAN_CD + "', ");
            insertQuery.Append("'" + wipTensou.PROCESS_KUBUN_ID + "', ");
            insertQuery.Append("'" + wipTensou.NIFUDA_CD + "', ");
            insertQuery.Append("'" + wipTensou.LOCATION_CD + "', ");
            insertQuery.Append("NULL, "); 
            insertQuery.Append("'" + wipTensou.LOCATION_CD_TO + "', ");
            insertQuery.Append("'" + wipTensou.START_VEHICLEL_CD + "', ");
            insertQuery.Append("NULL, ");
            insertQuery.Append("TO_DATE('" + dateTime + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "'), ");
            insertQuery.Append("NULL, "); 
            insertQuery.Append("NULL, ");
            insertQuery.Append("TO_DATE('" + AcceptancePlanDate + "','" + Constants.DateFormats.YYYYMMDD_HH24MISS + "'), ");
            insertQuery.Append("0, ");
            insertQuery.Append("'" + wipTensou.TABA_NO_1 + "', ");
            insertQuery.Append("'" + wipTensou.TABA_NO_2 + "', ");
            insertQuery.Append("'" + wipTensou.TABA_NO_3 + "', ");
            insertQuery.Append("'" + wipTensou.TABA_NO_4 + "', ");
            insertQuery.Append("'" + wipTensou.NO_OF_UNITS + "', ");
            insertQuery.Append("'" + wipTensou.UNIT_TYPE + "', ");
            insertQuery.Append("'" + wipTensou.QTY + "', ");
            
            if (wipTensou.MSG_CD == 0)
            {
                insertQuery.Append(" null ,");
            }
            else
            {
                insertQuery.Append("'" + wipTensou.MSG_CD + "', ");
            }
            insertQuery.Append("'" + wipTensou.NEXT_PROSESS + "', ");
            insertQuery.Append("'" + wipTensou.USER_ID + "', ");
            insertQuery.Append("'" + wipTensou.INPUT_LOCATION_CD + "', ");
            insertQuery.Append("SYSDATE, ");
            insertQuery.Append("SYSDATE)");
            ExecuteNonQuery(insertQuery.ToString());
            
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT MAX(TRANSFER_GROUP_NO) FROM TRN_WIP_AND_PROD_TRANSFER");
            int WIP_TRANSFER_GROUP_NO = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return WIP_TRANSFER_GROUP_NO;

        }
    }
}
