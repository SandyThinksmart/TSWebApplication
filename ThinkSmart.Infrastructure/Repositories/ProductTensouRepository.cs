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


        public int SaveProductTensouDetails(WIPTensouDetails wipTensou)
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
            //insertQuery.Append("'" + wipTensou.TABA_NO_1 + "', ");
            //insertQuery.Append("'" + wipTensou.TABA_NO_2 + "', ");
            //insertQuery.Append("'" + wipTensou.TABA_NO_3 + "', ");
            //insertQuery.Append("'" + wipTensou.TABA_NO_4 + "', ");
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

        public List<WIPTensouDetails> GetProductTensouList(List<SearchField> searchCriteria, string sortCiteria, long locationCD)
        {
            List<WIPTensouDetails> result = new List<WIPTensouDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * FROM (");
            selectQuery.Append("SELECT ");
            selectQuery.Append(" DISTINCT PROD.TRANSFER_GROUP_NO");
            selectQuery.Append(", (SELECT MAX( TO_CHAR(PROD.TRANSFER_DATE, 'YYYY/MM/DD')) FROM TRN_WIP_AND_PROD_TRANSFER PROD1 WHERE PROD1.TRANSFER_GROUP_NO =PROD.TRANSFER_GROUP_NO)  AS TRANSFER_DATE_STRING ");
            selectQuery.Append(" , ( ");
            selectQuery.Append(" SELECT MAX(TO_CHAR(PROD1.TRANSIT_PLANNED_DATE, 'YYYY/MM/DD'))");
            selectQuery.Append(" FROM TRN_WIP_AND_PROD_TRANSFER PROD1");
            selectQuery.Append(" WHERE PROD1.TRANSFER_GROUP_NO = PROD.TRANSFER_GROUP_NO");
            selectQuery.Append(" ) AS ACCEPT_PLANNED_DATE");
            selectQuery.Append(", LOCTO.SHORT_NAME AS LOCATION_TO");
            //selectQuery.Append(", GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN)  AS PP_CHUUBAN_GYOUBAN");
            //selectQuery.Append(", CUS.SHORT_NAME AS PP_CUSTOMER_NAME");
            //selectQuery.Append(", PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            //selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'',PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING");
            //selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            //selectQuery.Append(", NVL(PP.LENGTH_OTHER,PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            //selectQuery.Append(", PP.CHARGE AS PP_CHARGE ");
            //selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'YYYY/MM/DD') AS ACT_ALLOC_DATE_STRING ");
            //selectQuery.Append(", PP.ACT_ALLOC_QTY");
            //selectQuery.Append(", (PP.PROD_SPEC_NO1 || '-' || PP.PROD_SPEC_NO2) AS PP_PROD_SPEC_NO ");
            selectQuery.Append(", VHE.SHORT_NAME AS VEHICLE_NAME");
            selectQuery.Append(", LOCFROM.SHORT_NAME AS LOCATION_FROM  ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_WIP_AND_PROD_TRANSFER PROD ");

            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCTO ON LOCTO.LOCATION_CD = PROD.LOCATION_CD_TO ");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOCFROM ON LOCFROM.LOCATION_CD = PROD.LOCATION_CD_FROM ");
            //selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD ");
            //selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME MIN ON MIN.INTERNAL_NAME_CD=PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_VEHICLE VHE ON VHE.VEHICLE_CD = PROD.START_VEHICLEL_CD ");
            selectQuery.Append(" WHERE PROD.INPUT_LOCATION_CD = " + locationCD);
            //selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " AND ";
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    if (searchField.FieldName != null)
                    {
                        whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                        andOperator = " AND ";
                    }
                }

                query = query + whereCondition;
            }

            query = query + ")";

            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                //query += " ORDER BY PP_CHUUBAN_GYOUBAN ASC ";
            }

            result = ExecuteQueryMultiResult<WIPTensouDetails>(query);
            return result;
        }

        public int GetMaxGroupNo()
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT MAX(TRANSFER_GROUP_NO) FROM TRN_WIP_AND_PROD_TRANSFER");
            int max = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            //int max = ExecuteQueryMultiResult<long?>(selectQuery);
            return max;
        }

        public List<WIPTensouDetails> GetTransferCD(long TransferGroupNo)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT TRANSFER_CD AS WIP_TRANSFER_CD,TRANSFER_GROUP_NO AS WIP_TRANSFER_GROUP_NO  from TRN_WIP_AND_PROD_TRANSFER WHERE TRANSFER_GROUP_NO= '" + TransferGroupNo + "'");
            var result = ExecuteQueryMultiResult<WIPTensouDetails>(selectQuery.ToString()).ToList();
            return result;

        }

        public Dictionary<string, List<string>> GetValidShukkaCD(string productCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            string[] ShukkaCD;
            ShukkaCD = productCD.Split(',');
            List<string> validShukkaCD = new List<string>();
            List<string> notvalidShukkaCD = new List<string>();
            Dictionary<string, List<string>> _dictProdnumber = new Dictionary<string, List<string>>();

            foreach (string ProdNum in ShukkaCD)
            {
                selectQuery.Append(" SELECT COUNT(SHUKKA_INSTRUCTION_CD) FROM TRN_SHUKKA_INSTRUCTION");
                selectQuery.Append(" WHERE SHUKKA_INSTRUCTION_CD  = (" + ProdNum + ")");
                int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                if (count > 0)
                {
                    validShukkaCD.Add(ProdNum);
                }
                else
                {
                    notvalidShukkaCD.Add(ProdNum);
                }
                selectQuery.Clear();

            }
            _dictProdnumber.Add("found", validShukkaCD);
            _dictProdnumber.Add("notFound", notvalidShukkaCD);
            return _dictProdnumber;
        }
        public List<ProductTensouDetails> GetShukkaProductDetails(string shukkaCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<ProductTensouDetails> result = new List<ProductTensouDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" SHU.SHUKKA_INSTRUCTION_CD AS SHUKKA_CD,");
            selectQuery.Append(" SHU.PRODUCT_CD,");
            selectQuery.Append(" SHU.CUSTOMER_CD,");
            //selectQuery.Append(" SHU.PROD_SPEC_NO1,");
            //selectQuery.Append(" SHU.PROD_SPEC_NO2,");
            selectQuery.Append(" SHU.PROD_DIM_D AS PP_PROD_DIM_D,");
            selectQuery.Append(" SHU.PROD_DIM_W AS PROD_DIM_W_OTHER,");
            selectQuery.Append(" SHU.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER,");
            selectQuery.Append(" SHU.PROD_SHAPE AS PP_PROD_SHAPE,");
            selectQuery.Append(" SHU.PRODUCT_LENGTH,");
            selectQuery.Append(" SHU.UNIT,");
            selectQuery.Append(" SHU.CHARGE,");
            selectQuery.Append(" SHU.NO_OF_UNITS,");
            selectQuery.Append(" SHU.QUANTITY AS ACT_ALLOC_QTY,");
            selectQuery.Append(" SHU.RATE,");
            selectQuery.Append(" CUS.CUSTOMER_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PROD.PRODUCT_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" FROM TRN_SHUKKA_INSTRUCTION SHU ");
            selectQuery.Append(" LEFT JOIN MST_PRODUCT PROD ON PROD.PRODUCT_CD = SHU.PRODUCT_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = SHU.CUSTOMER_CD ");
            selectQuery.Append(" WHERE SHU.SHUKKA_INSTRUCTION_CD = " + shukkaCD);
            result = ExecuteQueryMultiResult<ProductTensouDetails>(selectQuery.ToString());
            return result;
        }

    }
}
