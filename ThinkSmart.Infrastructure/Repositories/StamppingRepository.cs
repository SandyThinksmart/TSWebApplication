using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class StamppingRepository : BaseRepository, IStamppingRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public StamppingRepository(IConfiguration _configuration,LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }
        public List<StamppingDetails> GetStamppingList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD)
        {
            List<StamppingDetails> result = new List<StamppingDetails>();

            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN");            
            selectQuery.Append(",PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD");
            selectQuery.Append(",PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO ");
            selectQuery.Append(",CST.SHORT_NAME AS PP_CUSTOMER_NAME");
            selectQuery.Append(",PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST");
            selectQuery.Append(",PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,'',PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ");
            selectQuery.Append(",PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(",NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING");
            selectQuery.Append(",PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(",PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(",GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING ");
            selectQuery.Append(",PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(",ST.ST_TABA_NO");
            //selectQuery.Append(",ST.ST_MACHINE_NAME");
            selectQuery.Append(",(SELECT MACHINE_NAME FROM MST_MACHINE WHERE MACHINE_CD = ST.MACHINE_CD) AS ST_MACHINE_NAME ");
            selectQuery.Append(",TO_CHAR( ST.ST_END_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_END_DATE_STRING");
            selectQuery.Append(",ST.ST_START_OPERATOR_CD AS ST_START_OPERATOR_CD");
            selectQuery.Append(",(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = ST_START_OPERATOR_CD) AS ST_START_OPERATOR_NAME");
            selectQuery.Append(",ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE");
            selectQuery.Append(",ST.ST_OK_BARS");
            selectQuery.Append(",ST.UNIT_TYPE AS UNIT_TYPE");
            selectQuery.Append(",ST.ST_OK_QTY");
            selectQuery.Append(",ST.ST_NG_BARS1");
            selectQuery.Append(",ST.ST_NG_BARS2");
            selectQuery.Append(",ST.ST_NG_BARS3");
            selectQuery.Append(",ST.ST_NG_DETAILS1");
            selectQuery.Append(",ST.ST_NG_DETAILS2");
            selectQuery.Append(",ST.ST_NG_DETAILS3");
            
            selectQuery.Append(",(SELECT EM1.SHORT_NAME FROM MST_EMPLOYEE EM1  WHERE EM1.EMPLOYEE_CD = ST_END_OPERATOR_CD) AS ST_END_OPERATOR_NAME");            
            selectQuery.Append(",ST.ST_DECISION AS ST_DECISION");
            selectQuery.Append(",ST.STAMPING_CD AS STAMPING_CD");
            selectQuery.Append(",INTE.INTERNAL_NAME");
            //sub query for count row no..
            selectQuery.Append(" ,(SELECT COUNT(PP.PROCESS_PLAN_CD)");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP INNER JOIN TRN_STAMPING ST ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EM ON ST.ST_START_OPERATOR_CD = EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE ST.ST_END_OPERATOR_CD IS NOT NULL ) ");
            selectQuery.Append(" AS ROW_COUNT ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_STAMPING ST  ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ");
            selectQuery.Append(" ON ST.ST_START_OPERATOR_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");

            selectQuery.Append(" WHERE ST.INPUT_LOCATION_CD =" + locationCD + "");
            selectQuery.Append(" AND ST_END_DATE IS NOT NULL ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");                      
            
            var query = selectQuery.ToString();
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                string whereCondition = " AND ";
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
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += " ORDER BY PP_CHUUBAN_GYOUBAN ASC,ST.ST_TABA_NO ASC";
            }
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<StamppingDetails>(query);
            return result;
        }

        public List<NameValue> GetFilteredEmployeeNameList(string searchText, bool addEmpty)
        {
            var result = new List<NameValue>();
            StringBuilder selectQuery = new StringBuilder();
            if (string.IsNullOrEmpty(searchText))
            {
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA ");
                selectQuery.Append(" ,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.SHORT_NAME ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD");
                selectQuery.Append(" WHERE EMP.IN_ACTIVE_FLAG =0 ");
               
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME) ");
            }
            else
            {
                selectQuery.Append(" SELECT DISTINCT(EMP.EMPLOYEE_CD) AS CD ");
                selectQuery.Append(" ,EMP.SHORT_NAME AS NAME ");
                selectQuery.Append(" ,EMP.YOBIGANA,EMP.YOBINA ");
                selectQuery.Append(" ,EMP.IN_ACTIVE_FLAG ");
                selectQuery.Append(" FROM MST_EMPLOYEE EMP ");
                selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG on DG.EMPLOYEE_CD=EMP.EMPLOYEE_CD");
                selectQuery.Append(" WHERE LOWER (EMP.YOBINA) LIKE lower('" + searchText + "%') ");
                selectQuery.Append(" AND EMP.IN_ACTIVE_FLAG = 0");
                
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME)");
            }
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
        }

        public StamppingDetails GetStamppingDetailsByStamppingCD(long stamppingCD)
        {
            StamppingDetails stamppingDetails = new StamppingDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ST.STAMPING_CD AS STAMPING_CD  ");
            selectQuery.Append(",ST.PROCESS_PLAN_CD	AS	PROCESS_PLAN_CD  ");
            selectQuery.Append(",ST.MACHINE_CD	AS	MACHINE_CD  ");
            selectQuery.Append(",(SELECT SHORT_NAME FROM MST_MACHINE WHERE MACHINE_CD = ST.MACHINE_CD) AS ST_MACHINE_NAME ");
            selectQuery.Append(",ST.ST_START_DATE AS ST_START_DATE  ");
            selectQuery.Append(",TO_CHAR( ST.ST_START_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_START_DATE_STRING");
            selectQuery.Append(",ST.ST_START_OPERATOR_CD AS	ST_START_OPERATOR_CD  ");
            selectQuery.Append(", ST.STAMP_DETAILS  ");
            selectQuery.Append(", ST.ST_TRIAL_RESULT");
            selectQuery.Append(", ST.ST_NG_QTY");
            selectQuery.Append(",ST.ST_OK_QTY AS ST_OK_QTY  ");
            selectQuery.Append(",ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE  ");
            selectQuery.Append(",ST.ST_END_DATE	AS	ST_END_DATE  ");
            selectQuery.Append(",TO_CHAR( ST.ST_END_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_END_DATE_STRING");
            selectQuery.Append(",ST.ST_END_OPERATOR_CD AS ST_END_OPERATOR_CD  ");
            selectQuery.Append(",ST.ST_DECISION	AS ST_DECISION  ");
            selectQuery.Append(", ST.ST_NEXT_PROCESS");
            selectQuery.Append(",ST.ST_OK_BARS AS ST_OK_BARS  ");
            selectQuery.Append(",ST.ST_NG_BARS1	AS	ST_NG_BARS1  ");
            selectQuery.Append(",ST.ST_NG_DETAILS1	AS	ST_NG_DETAILS1  ");
            selectQuery.Append(",ST.ST_NG_BARS2	AS	ST_NG_BARS2  ");
            selectQuery.Append(",ST.ST_NG_DETAILS2	AS	ST_NG_DETAILS2  ");
            selectQuery.Append(",ST.ST_NG_BARS3	AS	ST_NG_BARS3  ");
            selectQuery.Append(",ST.ST_NG_DETAILS3	AS	ST_NG_DETAILS3  ");
            selectQuery.Append(",ST.MSG_CD	AS	MSG_CD  ");
            selectQuery.Append(",ST.CREATED_DATE AS	CREATED_DATE  ");
            selectQuery.Append(",TO_CHAR( ST.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_CREATED_DATE_STRING");
            selectQuery.Append(",ST.UPDATED_DATE AS	UPDATED_DATE  ");
            selectQuery.Append(",TO_CHAR( ST.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_UPDATED_DATE_STRING");
            selectQuery.Append(",ST.ST_TABA_NO	AS	ST_TABA_NO  ");
            selectQuery.Append(",ST.UNIT_TYPE AS UNIT_TYPE  ");
            selectQuery.Append(",M.MSG_CD AS MSG_CD  ");
            selectQuery.Append(",ST.USER_ID	AS	USER_ID  ");

            selectQuery.Append(" , GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(",  PP.CHUUBAN	AS	PP_CHUUBAN  ");
            selectQuery.Append(",  PP.GYOUBAN	AS	PP_GYOUBAN  ");
            selectQuery.Append(",  PP.PROD_SPEC_NO	AS	PP_PROD_SPEC_NO  ");
            selectQuery.Append(",  PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(",  TO_CHAR(PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' )	AS	PP_DELIVERY_DATE_STRING  ");
            selectQuery.Append(",  EMP.SHORT_NAME	AS	PP_EMPLOYEE_NAME  ");
           
            selectQuery.Append(",  REC.SHORT_NAME	AS	PP_RECEIVER_NAME  ");
            selectQuery.Append(",  CST.short_name	AS	PP_CUSTOMER_NAME  ");
            selectQuery.Append(",  PP.CHAMFER	AS	PP_CHAMFER  ");
            selectQuery.Append(",  PP.COLOR_ID	AS	PP_COLOR  ");
            selectQuery.Append(",  PP.MATL_NAME_MAK	AS	PP_MATL_NAME_MAK  ");
            selectQuery.Append(",  PP.MATL_SHAPE	AS	PP_MATL_SHAPE  ");
            selectQuery.Append(",  PP.MATL_DIM_D	AS	PP_MATL_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(pp.matl_dim_d,PP.MATL_DIM_W ,null,null) AS PP_MATL_DIM_D_STRING ");
            selectQuery.Append(",  PP.CHARGE	AS	PP_CHARGE  ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            //selectQuery.Append(", PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");
            selectQuery.Append(", PP.NYUKA_CD ");
            selectQuery.Append(",  PP.MAGARI_SPAN	AS	PP_MAGARI_SPAN  ");
            selectQuery.Append(",  PP.PROD_SHAPE	AS	PP_PROD_SHAPE  ");
            selectQuery.Append(",  PP.PROD_DIM_D	AS	PP_PROD_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,NULL,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ");
            selectQuery.Append(",  PP.PROD_NAME_CUST	AS	PP_PROD_NAME_CUST  ");
            selectQuery.Append(",  PP.PROD_DIM_OTHER	AS	PP_PROD_DIM_OTHER  ");
            selectQuery.Append(",  PP.EXT_TEXTURE	AS	PP_EXT_TEXTURE  ");
            selectQuery.Append(",  PP.DIM_TOL_MIN_CHR	AS	PP_DIM_TOL_MIN_CHR  ");
            selectQuery.Append(",  PP.DIM_TOL_MAX_CHR	AS	PP_DIM_TOL_MAX_CHR  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(",  PP.DISPLAY	AS	PP_DISPLAY  ");
            selectQuery.Append(",  PP.STAMP	AS	PP_STAMP  ");
            selectQuery.Append(",  PP.PROD_LENGTH	AS	PP_PROD_LENGTH  ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(",  PP.LENGTH_TOL_MIN_CHR	AS	PP_LENGTH_TOL_MIN_CHR  ");
            selectQuery.Append(",  PP.LENGTH_TOL_MAX_CHR	AS	PP_LENGTH_TOL_MAX_CHR  ");

            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");

            selectQuery.Append(",  PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(",  PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(",  PP.NO_OF_UNITS	AS	PP_NO_OF_UNITS  ");
            selectQuery.Append(",  PP.UNIT	AS	PP_UNIT  ");
            selectQuery.Append(",  PP.ACT_ALLOC_QTY	AS	PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(",  PP.COIL_DIA_INTERNAL	AS	PP_COIL_DIA_INTERNAL  ");
            selectQuery.Append(",  PP.COIL_DIA_EXTERNAL	AS	PP_COIL_DIA_EXTERNAL  ");
            selectQuery.Append(", TO_CHAR( PP.COIL_DIA_WINDING)	AS	PP_COIL_DIA_WINDING  ");
            selectQuery.Append(", MC.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION ");
            selectQuery.Append(",  PP.PROCESS_PLAN_REMARK	AS	PP_PROCESS_PLAN_REMARK  ");
            selectQuery.Append(",  M.MSG_CONTENTS AS ST_REMARKS ");
            selectQuery.Append(",  PP.STD_DEL_PERIOD	AS	PP_STD_DEL_PERIOD  ");
            selectQuery.Append(",  PP.CREATED_DATE	AS	PP_CREATED_DATE  ");
            selectQuery.Append(",  PP.UPDATED_DATE	AS	PP_UPDATED_DATE  ");
            
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_CD	AS	PP_PROCESS_PLAN_CD  ");
           // selectQuery.Append(", K.REMARKS 	AS	PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(",  PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");

            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = ST_START_OPERATOR_CD) AS ST_START_OPERATOR_NAME");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = ST_END_OPERATOR_CD) AS ST_END_OPERATOR_NAME");

            selectQuery.Append("  FROM TRN_STAMPING ST  ");
            selectQuery.Append("  INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD ");
            
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK ON PCK.PACKING_CD = PP.PACKING_CD  ");
           
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD ");
            
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST ON PP.CUSTOMER_CD = CST.CUSTOMER_CD"); 
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DG ON PP.DEPARTMENT_GROUP_CD = DG.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON PP.RECEIVER_CD = REC.RECEIVER_CD ");            
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON PP.LOCATION_CD = LOC.LOCATION_CD");
            selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION MC ON PP.PROD_CONDITION_CD = MC.CONDITION_CD");

            selectQuery.Append("  WHERE ST.STAMPING_CD=" + stamppingCD);
            stamppingDetails = ExecuteQuerySingleResult<StamppingDetails>(selectQuery.ToString());
            return stamppingDetails;
        }

        public List<NameValue> GetMachineNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MACHINE_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_MACHINE WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        public List<NameValue> GetLocationNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_LOCATION WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }
        public List<NameValue> GetCustomerNameList()
        {
            List<NameValue> result = new List<NameValue>();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT CUSTOMER_CD AS CD ");
            query.Append(" ,SHORT_NAME AS NAME ");
            query.Append(" ,IN_ACTIVE_FLAG ");
            query.Append(" FROM MST_CUSTOMER WHERE IN_ACTIVE_FLAG = 0");
            query.Append(" ORDER BY LOWER (SHORT_NAME) ");
            result = ExecuteQueryMultiResult<NameValue>(query.ToString());
            return result;
        }

        public void DeleteStamppingDetails(long stamppingCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_STAMPING ");
            deleteQuery.Append(" WHERE STAMPING_CD =" + stamppingCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public MessageDetails MessageDetailsInsert(MessageDetails messageDetails)
        {
            MessageDetails result = new MessageDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT  M.MSG_CD, M.PROCESS_PLAN_CD, M.MACHINE_CD , M.PROCESS_KUBUN_ID, M.OPERATOR_CD  ,M.MSG_DATE , M.MSG_CONTENTS, M.USER_ID, M.CREATED_DATE, M.UPDATED_DATE FROM TRN_MESSAGE M ");
            selectQuery.Append("WHERE MSG_CD = (SELECT MAX(MSG_CD) FROM TRN_MESSAGE)");

            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_MESSAGE ( ");
            insertQuery.Append("  PROCESS_PLAN_CD ");
            insertQuery.Append(", MACHINE_CD ");
            insertQuery.Append(", PROCESS_KUBUN_ID ");
            insertQuery.Append(", OPERATOR_CD ");
            insertQuery.Append(", MSG_DATE ");
            insertQuery.Append(", MSG_CONTENTS ");
            insertQuery.Append(", INPUT_LOCATION_CD ");
            insertQuery.Append(", USER_ID ");
            insertQuery.Append(", CREATED_DATE ");
            insertQuery.Append(", UPDATED_DATE ");

            insertQuery.Append(") VALUES (");

            insertQuery.Append("'" + messageDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + messageDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + messageDetails.PROCESS_KUBUN_ID + "'");
            insertQuery.Append(",'" + messageDetails.OPERATOR_CD + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(",'" + messageDetails.MSG_CONTENTS + "'");
            insertQuery.Append(",'" + messageDetails.INPUT_LOCATION_CD + "'");
            //insertQuery.Append(",'" + 5 + "'");
            insertQuery.Append(",'" + messageDetails.USER_ID + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());
            result = ExecuteQuerySingleResult<MessageDetails>(selectQuery.ToString());
            return result;
        }

        public MessageDetails MessageDetailsUpdate(long messageCD, string msgContents, int userID)
        {
            MessageDetails result = new MessageDetails();

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT  M.MSG_CD, M.PROCESS_PLAN_CD, M.MACHINE_CD , M.PROCESS_KUBUN_ID, M.OPERATOR_CD  ,M.MSG_DATE , M.MSG_CONTENTS, M.USER_ID, M.CREATED_DATE, M.UPDATED_DATE FROM TRN_MESSAGE M ");
            selectQuery.Append("WHERE MSG_CD = " + messageCD);

            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE TRN_MESSAGE SET ");
            updateQuery.Append(" MSG_DATE = SYSDATE");
            updateQuery.Append(", MSG_CONTENTS = '");
            updateQuery.Append(msgContents + "'");
            updateQuery.Append(", USER_ID = '");
            updateQuery.Append(userID + "'");
            updateQuery.Append(", UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE MSG_CD = ");
            updateQuery.Append(messageCD);
            ExecuteNonQuery(updateQuery.ToString());
            result = ExecuteQuerySingleResult<MessageDetails>(selectQuery.ToString());
            return result;
        }

        public void MessageDetailsDelete(long messageCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM TRN_MESSAGE ");
            deleteQuery.Append(" WHERE MSG_CD = ");
            deleteQuery.Append(messageCD);
            ExecuteNonQuery(deleteQuery.ToString());            
        }

        public long UpdateStamppingDetails(StamppingDetails stamppingDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_STAMPING SET ");
            updateQuery.Append("  STAMPING_CD =" + (stamppingDetails.STAMPING_CD != null ? stamppingDetails.STAMPING_CD.ToString().Trim() : "''"));
            updateQuery.Append(",  PROCESS_PLAN_CD =" + (stamppingDetails.PROCESS_PLAN_CD != null ? stamppingDetails.PROCESS_PLAN_CD.ToString().Trim() : "''"));
            updateQuery.Append(", MACHINE_CD =" + (stamppingDetails.MACHINE_CD != null ? stamppingDetails.MACHINE_CD.ToString().Trim(): "''"));
            updateQuery.Append(", MSG_CD ='" + stamppingDetails.MSG_CD + "'");
            updateQuery.Append(", ST_START_DATE = TO_DATE('" + (Convert.ToDateTime(stamppingDetails.ST_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(", ST_START_OPERATOR_CD =" + (stamppingDetails.ST_START_OPERATOR_CD != null ? stamppingDetails.ST_START_OPERATOR_CD.ToString().Trim(): "''"));
            updateQuery.Append(", ST_METALLIC_PLATE_CODE = '" + (stamppingDetails.ST_METALLIC_PLATE_CODE + "'" != null ? stamppingDetails.ST_METALLIC_PLATE_CODE.ToString().Trim() : string.Empty) + "'");

            updateQuery.Append(", STAMP_DETAILS ='" + stamppingDetails.STAMP_DETAILS + "'");
            updateQuery.Append(", ST_TRIAL_RESULT ='" + (stamppingDetails.ST_TRIAL_RESULT + "'" != null ? stamppingDetails.ST_TRIAL_RESULT.ToString().Trim(): string.Empty)+"'"); 
            updateQuery.Append(", ST_END_DATE = TO_DATE('" + (Convert.ToDateTime(stamppingDetails.ST_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append(", ST_END_OPERATOR_CD =" + (stamppingDetails.ST_END_OPERATOR_CD != null ? stamppingDetails.ST_END_OPERATOR_CD.ToString().Trim(): "''"));
            updateQuery.Append(", ST_DECISION =" + (stamppingDetails.ST_DECISION !=null ? stamppingDetails.ST_DECISION.ToString().Trim() : "''"));
            updateQuery.Append(", ST_NG_QTY ='" + (stamppingDetails.ST_NG_QTY != null ? stamppingDetails.ST_NG_QTY.ToString():string.Empty)+ "'");
            updateQuery.Append(", ST_OK_QTY ='" + (stamppingDetails.ST_OK_QTY != null ? stamppingDetails.ST_OK_QTY.ToString():string.Empty)+ "'");
            updateQuery.Append(", ST_NG_BARS1 ='" + (stamppingDetails.ST_NG_BARS1 +"'" !=null ? stamppingDetails.ST_NG_BARS1.ToString():string.Empty)+ "'");
            updateQuery.Append(", ST_NG_BARS2 ='" + (stamppingDetails.ST_NG_BARS2 + "'" !=null ? stamppingDetails.ST_NG_BARS2.ToString():string.Empty)+"'");
            updateQuery.Append(", ST_NG_BARS3 ='" + (stamppingDetails.ST_NG_BARS3 + "'" != null ? stamppingDetails.ST_NG_BARS3.ToString():string.Empty)+"'");
            updateQuery.Append(", ST_NG_DETAILS1 ='" + (stamppingDetails.ST_NG_DETAILS1 + "'" != null ? stamppingDetails.ST_NG_DETAILS1.ToString():string.Empty)+ "'");
            updateQuery.Append(", ST_NG_DETAILS2 ='" + (stamppingDetails.ST_NG_DETAILS2 + "'" != null ? stamppingDetails.ST_NG_DETAILS2.ToString():string.Empty)+ "'");
            updateQuery.Append(", ST_NG_DETAILS3 ='" + (stamppingDetails.ST_NG_DETAILS3 + "'" !=null ? stamppingDetails.ST_NG_DETAILS3.ToString():string.Empty)+ "'");
            updateQuery.Append(", ST_TABA_NO ='" + (stamppingDetails.ST_TABA_NO + "'" != null ? stamppingDetails.ST_TABA_NO.ToString():string.Empty)+"'");
            updateQuery.Append(", ST_OK_BARS='" + (stamppingDetails.ST_OK_BARS + "'" != null ? stamppingDetails.ST_OK_BARS.ToString():string.Empty)+"'");
            updateQuery.Append(", UNIT_TYPE ='" + stamppingDetails.UNIT_TYPE + "'");
            updateQuery.Append(", ST_NEXT_PROCESS='" + (stamppingDetails.ST_NEXT_PROCESS + "'"!= null ? stamppingDetails.ST_NEXT_PROCESS.ToString():string.Empty)+"'");            
            updateQuery.Append(", UPDATED_DATE = SYSDATE ");
            updateQuery.Append(", PROCESS_KUBUN_CD ='" + stamppingDetails.PROCESS_KUBUN_CD + "'");
            updateQuery.Append(", USER_ID ='" + (stamppingDetails.USER_ID + "'" != null ? stamppingDetails.USER_ID.ToString():string.Empty)+"'");
            updateQuery.Append(", INPUT_LOCATION_CD ='" + (stamppingDetails.INPUT_LOCATION_CD != null ? stamppingDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "'");           
            updateQuery.Append(", LOCATION_CD ='" + stamppingDetails.LOCATION_CD + "'");
            updateQuery.Append(" WHERE STAMPING_CD = ");
            updateQuery.Append(stamppingDetails.STAMPING_CD);
            ExecuteNonQuery(updateQuery.ToString());

            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_STAMPING WHERE STAMPING_CD= '" + stamppingDetails.STAMPING_CD + "'");

            long msgCD = 0;

            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCD = Convert.ToInt64(cdVal);
            }
            return msgCD;
        }

        public StamppingDetails GetStamppingDetailsByChuuban(string CHUUBAN, short? GYOUBAN, int MACHINE_CD, string NIFUDA_NO)
        {
            StringBuilder selectCountQuery = new StringBuilder();
            StamppingDetails stamppingDetails = new StamppingDetails();

            int count = 0;

            string whereCondition1 = " PP.CHUUBAN = '" + CHUUBAN + "' AND PP.GYOUBAN = '" + GYOUBAN + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL";
            string whereCondition2 = "  AND ST.MACHINE_CD = '" + MACHINE_CD + "' AND ST.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + "";
            string finalWhere = "";
            finalWhere = whereCondition1 + whereCondition2;
            

            selectCountQuery.Append(" SELECT COUNT(*) ");
            selectCountQuery.Append(" FROM ");
            selectCountQuery.Append(" TRN_PROCESS_PLAN  PP ");
            selectCountQuery.Append(" INNER JOIN TRN_STAMPING ST ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectCountQuery.Append(" WHERE " + finalWhere);
            count = Convert.ToInt32(ExecuteScalar(selectCountQuery.ToString()));

            if (count > 0)
            {
                finalWhere = whereCondition1 + whereCondition2;
            }
            else
            {
                finalWhere = whereCondition1;
            }


            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.PRODUCT_CD  AS PP_PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
           
            selectQuery.Append(" ,EMP.SHORT_NAME AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,INTE.INTERNAL_NAME AS PP_PROD_NAME_INT");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR "); 
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");

            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR "); 
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(" ,PP.NYUKA_CD ");
            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            //selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
            selectQuery.Append(", PP.UNIT_WT_MIN AS UNIT_WT_MIN ");
            selectQuery.Append(", PP.UNIT_WT_MAX AS UNIT_WT_MAX");

            selectQuery.Append(" ,PP.COLOR_ID  AS PP_COLOR ");
            selectQuery.Append(" ,PP.TEST_SPECIMEN AS PP_JIS_TEST_SPEC ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY ");
            selectQuery.Append(" ,PP.PROD_SPEC_NO  AS PP_PROD_SPEC_NO ");
            selectQuery.Append(" ,PP.CHUUBAN || '-'|| PP.GYOUBAN AS  PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.DELIVERY_DATE  AS PP_DELIVERY_DATE ");
            selectQuery.Append(" ,TO_CHAR( PP.DELIVERY_DATE,'yyyy/mm/dd' ) AS PP_DELIVERY_DATE_STRING ");
            selectQuery.Append(" ,PP.MATL_SHAPE AS PP_MATL_SHAPE ");
            selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.PROD_DIM_OTHER AS PP_PROD_DIM_OTHER ");
            selectQuery.Append(" ,PP.EXT_TEXTURE AS PP_EXT_TEXTURE  ");
            selectQuery.Append(" ,PP.DISPLAY AS PP_DISPLAY ");
            selectQuery.Append(" ,PP.STAMP AS PP_STAMP ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" ,PCK.PACKING_NAME AS PP_PACKING ");
            selectQuery.Append(", PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(" ,To_CHAR (PP.COIL_DIA_WINDING) AS PP_COIL_DIA_WINDING ");
           
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(", PP.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(" ,ST.STAMPING_CD AS STAMPING_CD ");
            selectQuery.Append(" ,ST.CREATED_DATE  AS CREATED_DATE ");
            selectQuery.Append(" ,ST.UPDATED_DATE  AS UPDATED_DATE ");
            selectQuery.Append(" ,(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD = ST.MACHINE_CD) AS ST_MACHINE_NAME ");
            selectQuery.Append(" ,ST.PROCESS_PLAN_CD AS PROCESS_PLAN_CD ");
            selectQuery.Append(" ,ST.ST_START_DATE AS ST_START_DATE  ");
            selectQuery.Append(" ,TO_CHAR( ST.ST_START_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_START_DATE_STRING");
            selectQuery.Append(" ,ST.ST_START_OPERATOR_CD AS ST_START_OPERATOR_CD ");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = ST_START_OPERATOR_CD) AS ST_START_OPERATOR_NAME  ");
            selectQuery.Append(" ,ST.ST_TRIAL_RESULT AS ST_TRIAL_RESULT ");
            selectQuery.Append(" ,ST.ST_OK_QTY AS ST_OK_QTY ");
            selectQuery.Append(" ,ST.ST_NG_QTY AS ST_NG_QTY ");
            selectQuery.Append(" ,ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,ST.MSG_CD AS MSG_CD ");
            selectQuery.Append(" ,ST.STAMP_DETAILS AS STAMP_DETAILS ");
            selectQuery.Append(" ,ST.ST_END_DATE AS ST_END_DATE  ");
            selectQuery.Append(" ,TO_CHAR( ST.ST_END_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_END_DATE_STRING");
            selectQuery.Append(" ,ST.ST_END_OPERATOR_CD AS ST_END_OPERATOR_CD ");
            selectQuery.Append(" ,ST.ST_TABA_NO AS ST_TABA_NO ");
            selectQuery.Append(" ,ST.ST_OK_BARS AS ST_OK_BARS ");
            //selectQuery.Append(" ,ST.UNIT_TYPE AS UNIT_TYPE ");
            selectQuery.Append(", (SELECT ST.UNIT_TYPE FROM TRN_STAMPING ST INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE  PP.PROCESS_PLAN_CMPL_DATE IS NULL  AND PP.CHUUBAN = '" + CHUUBAN + "' AND PP.GYOUBAN = '" + GYOUBAN + "' AND ST.MACHINE_CD = '" + MACHINE_CD + "' ");
            selectQuery.Append(" AND ST.UNIT_TYPE IS NOT NULL AND ST.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " ORDER BY  ST.STAMPING_CD DESC FETCH FIRST ROW ONLY)  AS UNIT_TYPE ");
            selectQuery.Append(" ,ST.ST_NG_BARS1 AS ST_NG_BARS1 ");
            selectQuery.Append(" ,ST.ST_NG_BARS2 AS ST_NG_BARS2 ");
            selectQuery.Append(" ,ST.ST_NG_BARS3 AS ST_NG_BARS3 ");
            selectQuery.Append(" ,ST.ST_NG_DETAILS1 AS ST_NG_DETAILS1 ");
            selectQuery.Append(" ,ST.ST_NG_DETAILS3 AS ST_NG_DETAILS2 ");
            selectQuery.Append(" ,ST.ST_NG_DETAILS3 AS ST_NG_DETAILS3 ");
            selectQuery.Append(" ,ST.ST_DECISION AS ST_DECISION ");
            selectQuery.Append(" ,ST.ST_NEXT_PROCESS AS ST_NEXT_PROCESS ");
            selectQuery.Append(" ,ST.USER_ID AS USER_ID, ");

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

            selectQuery.Append(" P1.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_1, ");
            selectQuery.Append(" P2.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_2, ");
            selectQuery.Append(" P3.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_3, ");
            selectQuery.Append(" P4.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_4, ");
            selectQuery.Append(" P5.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_5, ");
            selectQuery.Append(" P6.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_6, ");
            selectQuery.Append(" P7.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_7, ");
            selectQuery.Append(" P8.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_8, ");
            selectQuery.Append(" P9.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_9, ");
            selectQuery.Append(" P10.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_10, ");
            selectQuery.Append(" P11.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_11, ");
            selectQuery.Append(" P12.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_12, ");
            selectQuery.Append(" P13.ANOTHER_NAME AS PP_PROCESS_ANOTHER_NAME_13, ");

            selectQuery.Append(" PP.PROD_POINT_1 AS PP_PROD_POINT_1, ");
            selectQuery.Append(" PP.PROD_POINT_2 AS PP_PROD_POINT_2, ");
            selectQuery.Append(" PP.PROD_POINT_3 AS PP_PROD_POINT_3, ");
            selectQuery.Append(" PP.PROD_POINT_4 AS PP_PROD_POINT_4, ");
            selectQuery.Append(" PP.PROD_POINT_5 AS PP_PROD_POINT_5, ");
            selectQuery.Append(" PP.PROD_POINT_6 AS PP_PROD_POINT_6, ");
            selectQuery.Append(" PP.PROD_POINT_7 AS PP_PROD_POINT_7, ");
            selectQuery.Append(" PP.PROD_POINT_8 AS PP_PROD_POINT_8, ");
            selectQuery.Append(" PP.PROD_POINT_9 AS PP_PROD_POINT_9, ");
            selectQuery.Append(" PP.PROD_POINT_10 AS PP_PROD_POINT_10, ");
            selectQuery.Append(" PP.PROD_POINT_11 AS PP_PROD_POINT_11, ");
            selectQuery.Append(" PP.PROD_POINT_12 AS PP_PROD_POINT_12, ");
            selectQuery.Append(" PP.PROD_POINT_13 AS PP_PROD_POINT_13, ");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MAX,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN2_MIN,");
            selectQuery.Append("PP.TENSILE_STRN_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.ELONGATION_PATTERN1,");
            selectQuery.Append("PP.ELONGATION_PATTERN1_MIN_MAX 	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MAX	,");
            selectQuery.Append("PP.ELONGATION_PATTERN2_MIN ,");
            selectQuery.Append("PP.ELONGATION_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.HARDNESS_PATTERN1 ,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN1_MIN_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_CODE  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MAX  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN2_MIN  	,");
            selectQuery.Append("PP.HARDNESS_PATTERN3_ACTUAL  	,");

            selectQuery.Append("PP.DECARB_1 ,");
            selectQuery.Append("PP.DECARB_2 ,");
            selectQuery.Append("PP.DECARB_3 ,");
            selectQuery.Append("PP.DECARB_VAL_1 ,");
            selectQuery.Append("PP.DECARB_VAL_2  ,");
            selectQuery.Append("PP.DECARB_VAL_3,");

            selectQuery.Append("PP.YEILD_PATTERN1,");
            selectQuery.Append("PP.YEILD_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MAX,");
            selectQuery.Append("PP.YEILD_PATTERN2_MIN,");
            selectQuery.Append("PP.YEILD_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.PRESS_PATTERN1,");
            selectQuery.Append("PP.PRESS_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MAX,");
            selectQuery.Append("PP.PRESS_PATTERN2_MIN,");
            selectQuery.Append("PP.PRESS_PATTERN3_ACTUAL,");

            selectQuery.Append("PP.IMPACT_TEST_PATTERN1,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN1_MIN_MAX,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN2_MAX,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN2_MIN,");
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL,");
            selectQuery.Append("PP.PROCESS_PLAN_CMPL_DATE AS PP_PROCESS_PLAN_CMPL_DATE,");
            selectQuery.Append("PP.PROCESS_PLAN_START_DATE,  ");
            selectQuery.Append(" MC.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION, ");
            selectQuery.Append("K.REMARKS 	AS	PP_KAKOUIRAI_REMARKS,  ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN ST.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " THEN ");
            selectQuery.Append(" 1 ");
            selectQuery.Append(" ELSE ");
            selectQuery.Append(" 0 ");
            selectQuery.Append(" END AS SHOW_DEFAULT, ");
            if (count == 0)
            {
                selectQuery.Append("'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append("'YES' AS PAST_DATA_EXIST ");
            }

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_STAMPING ST ON");
            selectQuery.Append(" ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            //selectQuery.Append(" AND ST.MACHINE_CD = '" + MACHINE_CD + "' ");
            
            selectQuery.Append(" LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append(" ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append(" ON M.MSG_CD = ST.MSG_CD  ");
           
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
          
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON PP.EMPLOYEE_CD = EMP.EMPLOYEE_CD");
          
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
            selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION MC ON PP.PROD_CONDITION_CD = MC.CONDITION_CD ");
            selectQuery.Append(" WHERE " + finalWhere);
            //selectQuery.Append(" WHERE PP.CHUUBAN= '" + CHUUBAN + "'");
            //selectQuery.Append(" AND PP.GYOUBAN= '" + GYOUBAN + "'");
            //selectQuery.Append(" AND ST.MACHINE_CD = '" + MACHINE_CD + "' ");
            //selectQuery.Append(" AND ST.CREATED_DATE >= TO_DATE(SYSDATE)-" + days + " ");
            //selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" ORDER BY ST.STAMPING_CD DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY");
            stamppingDetails = ExecuteQuerySingleResult<StamppingDetails>(selectQuery.ToString());
            return stamppingDetails;
        }

        public List<MessageDetails> GetMessageDetailsByProcessPlanCD(long processPlanCD, bool isSortDesc)
        {
            List<MessageDetails> result = new List<MessageDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append("   MSG_DATE ");
            selectQuery.Append(" , TO_CHAR( MSG_DATE,'yyyy/mm/dd HH24:MI' ) AS MSG_DATE_STRING");
            selectQuery.Append(" , MSG_CONTENTS");
            selectQuery.Append(" , MAC.SHORT_NAME  AS MACHINE_NAME  ");
            selectQuery.Append(" , EMP.SHORT_NAME AS  OPERATOR_NAME ");
            selectQuery.Append(" , PKU.KUBUN_NAME AS PROCESS_NAME_JP ");
            selectQuery.Append(" , PKU.KUBUN_NAME_EN AS PROCESS_NAME_EN");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_MESSAGE  MSG ");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MAC  ON MAC.MACHINE_CD =MSG.MACHINE_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=MSG.OPERATOR_CD");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PKU ON MSG.PROCESS_KUBUN_ID=PKU.KUBUN_ID");
            selectQuery.Append(" WHERE MSG.PROCESS_PLAN_CD=" + processPlanCD);
            if (isSortDesc == true)
            {
                selectQuery.Append(" ORDER BY MSG_DATE DESC");
            }
            else
            {
                selectQuery.Append(" ORDER BY MSG_DATE ASC");
            }
            result = ExecuteQueryMultiResult<MessageDetails>(selectQuery.ToString());
            return result;
        }

        public List<ThabaDetails> GetThabaDetails(string chuuban, short? gyuban)
        {
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN  ");
            selectQuery.Append(" ,PP.GYOUBAN ");
            selectQuery.Append(" ,ST.ST_DECISION AS DECISION");
            selectQuery.Append(" ,ST.CREATED_DATE ");
            selectQuery.Append(" ,TO_CHAR(ST.ST_TABA_NO) AS TABA_NO ");
            selectQuery.Append(" ,DECODE(ST.ST_DECISION,0,1,2,3) AS PROCESS_STATUS ");
            selectQuery.Append(" FROM TRN_STAMPING ST INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");

            selectQuery.Append(" AND ST.UPDATED_DATE IN ( ");
            selectQuery.Append(" SELECT  MAX(ST.UPDATED_DATE) FROM ( ");
            selectQuery.Append(" SELECT  ST.UPDATED_DATE , ST.ST_TABA_NO AS TABA_NO_INT ");

            selectQuery.Append(" FROM TRN_STAMPING ST INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyuban + "' ");

            selectQuery.Append(" ORDER BY ST_TABA_NO ) GROUP BY ST_TABA_NO  ) ");
            selectQuery.Append(" ORDER BY ST.STAMPING_CD ");
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }

        public List<StamppingDetails> GetThabaDetailsByThabaNumber(string chuuban, short? gyouban, short? Thaba_No)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new List<StamppingDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, ST.STAMPING_CD,ST.ST_TABA_NO AS TABA_NO,ST.ST_DECISION ");
            selectQuery.Append(" FROM TRN_STAMPING ST INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = ST.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND ST.ST_DECISION ='" + decisionVal + "' ");
            if (Thaba_No != 0)
            {
                selectQuery.Append(" AND ST.ST_TABA_NO ='" + Thaba_No + "' ");
            }


            result = ExecuteQueryMultiResult<StamppingDetails>(selectQuery.ToString());
            return result;
        }

        public MST_MACHINE GetMachineNameByID(long machineCd)
        {
            MST_MACHINE result = new MST_MACHINE();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT * From  MST_MACHINE");
            selectQuery.Append(" WHERE MACHINE_CD = '" + machineCd + "' ");
            //selectQuery.Append("AND IN_ACTIVE_FLAG = 0");     //only Active Machine is selected Pradeep M.2019-09-11
            result = ExecuteQuerySingleResult<MST_MACHINE>(selectQuery.ToString());
            return result;
        }

        public List<StamppingDetails> GetStampping24MachineList(string machineCd)
        {
            List<StamppingDetails> result = new List<StamppingDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" ST.MACHINE_CD AS MACHINE_CD,  ");
            selectQuery.Append(" ST.ST_DECISION AS ST_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");
            //selectQuery.Append(" CONCAT (CONCAT (PP.CHUUBAN, '-'), PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,	");

            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN,");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append(" ST.ST_METALLIC_PLATE_CODE  AS ST_METALLIC_PLATE_CODE,	");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D,	");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY,	");
            selectQuery.Append(" ST.ST_NG_QTY,	"); 
            //selectQuery.Append(" PP.PROD_DIM_D || '' || PP.PROD_SHAPE  AS  PP_PROD_DIM_D_SHAPE, ");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_SHAPE,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_SHAPE,");

            selectQuery.Append(" ST.ST_TABA_NO AS ST_TABA_NO,");

            selectQuery.Append(" ST.ST_OK_BARS AS ST_OK_BARS, ");
            selectQuery.Append(" ST.UNIT_TYPE AS ST_UNIT_TYPE, ");
            selectQuery.Append(" ST.ST_OK_QTY AS ST_OK_QTY, ");
            selectQuery.Append(" ST.ST_NG_QTY AS ST_NG_QTY, ");
            selectQuery.Append(" ST.STAMP_DETAILS ,");
            selectQuery.Append("  M.MSG_CONTENTS AS ST_REMARKS, ");
            selectQuery.Append("  ST.MSG_CD AS MSG_CD, ");
            selectQuery.Append("  PP.PROD_SPEC_NO AS PROD_SPEC_NO,	");
            selectQuery.Append("  PP.CHARGE AS PP_CHARGE,	");
            selectQuery.Append("  PP.PROD_LENGTH AS PP_PROD_LENGTH,");
            selectQuery.Append("  NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append("  NVL (ST.ST_END_DATE, ST.ST_START_DATE) AS ORDER_CLAUSE ");

            selectQuery.Append(" FROM TRN_STAMPING ST");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ");
            selectQuery.Append(" ON M.MSG_CD = ST.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" WHERE  ST.MACHINE_CD  = '" + machineCd + "' AND NVL (ST.ST_END_DATE, ST.ST_START_DATE) > SYSDATE - 1");
            //selectQuery.Append(" AND ROWNUM <= 10");
            selectQuery.Append(" ORDER BY ORDER_CLAUSE DESC");
            result = ExecuteQueryMultiResult<StamppingDetails>(selectQuery.ToString());
            return result;
        }

        public string GetEmployeeNameByID(long employeeCd)
        {
            string result = string.Empty;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT EM.SHORT_NAME  FROM MST_EMPLOYEE EM");
            selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DG ");
            selectQuery.Append(" ON DG.EMPLOYEE_CD =EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE EM.EMPLOYEE_CD = '" + employeeCd + "' ");
            result = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            return result;
        }

        public void InsertStamppingDetails(StamppingDetails stamppingDetails)
        {
            StamppingDetails result = new StamppingDetails();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_STAMPING(");
            insertQuery.Append(" STAMPING_CD,");
            insertQuery.Append(" PROCESS_PLAN_CD,");
            insertQuery.Append(" MACHINE_CD,");
            insertQuery.Append(" LOCATION_CD,");
            insertQuery.Append(" MSG_CD,");
            insertQuery.Append(" ST_START_DATE,");
            insertQuery.Append(" ST_START_OPERATOR_CD,");
            insertQuery.Append(" ST_METALLIC_PLATE_CODE,");
            insertQuery.Append(" STAMP_DETAILS,");
            insertQuery.Append(" ST_TRIAL_RESULT,");
            insertQuery.Append(" ST_TABA_NO,");
            insertQuery.Append(" USER_ID, ");
            insertQuery.Append(" INPUT_LOCATION_CD, ");            
            insertQuery.Append(" CREATED_DATE,");
            insertQuery.Append(" UPDATED_DATE,");
            insertQuery.Append(" PROCESS_KUBUN_CD ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("TRN_STAMPING_SEQ.NEXTVAL");
            insertQuery.Append(",'" + stamppingDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + stamppingDetails.MACHINE_CD + "'");
            insertQuery.Append(",'" + stamppingDetails.LOCATION_CD + "'");
            insertQuery.Append(",'" + stamppingDetails.MSG_CD + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(stamppingDetails.ST_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + stamppingDetails.ST_START_OPERATOR_CD + "'");
            insertQuery.Append(",'" + stamppingDetails.ST_METALLIC_PLATE_CODE + "'");
            insertQuery.Append(",'" + stamppingDetails.STAMP_DETAILS + "'");
            insertQuery.Append(",'" + stamppingDetails.ST_TRIAL_RESULT + "'");
            insertQuery.Append(",'" + stamppingDetails.ST_TABA_NO + "'");

            insertQuery.Append(",'" + stamppingDetails.USER_ID + "'");
            insertQuery.Append(",'" + stamppingDetails.INPUT_LOCATION_CD + "'");            
           
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + stamppingDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
           
        }

        public int GetUserLocationById(int userID)
        {
            int locationCD = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT LOCATION_CD");
            query.Append(" FROM MST_USER_LOGIN ");
            query.Append(" WHERE USER_ID=" + userID);
            locationCD = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return locationCD;
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Stampping process data according to search criteria        
        /// </returns>
        /// 
        public List<StamppingDetails> GetStamppingDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCiteria,int rowLimit,string cultureName)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<StamppingDetails> result = new List<StamppingDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS PP_LOCATION_NAME, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
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
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" ST.ST_TABA_NO AS ST_TABA_NO, ");
            selectQuery.Append(" ST.ST_OK_BARS AS ST_OK_BARS, ");
            selectQuery.Append(" ST.UNIT_TYPE, ");
            selectQuery.Append(" ST.ST_OK_QTY AS ST_OK_QTY, ");
            selectQuery.Append(" ST.STAMP_DETAILS AS STAMP_DETAILS, ");
            selectQuery.Append(" M.MSG_CONTENTS AS ST_REMARKS, ");
            selectQuery.Append(" ST.ST_DECISION AS ST_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" ST.ST_NG_BARS1, ");
            if(cultureName == "en")
            {
                selectQuery.Append(" DEFECT1.OTHER_NAME_SHORT AS ST_NG_DETAILS1_STRING,");
                selectQuery.Append(" DEFECT2.OTHER_NAME_SHORT AS ST_NG_DETAILS2_STRING,");
                selectQuery.Append(" DEFECT3.OTHER_NAME_SHORT AS ST_NG_DETAILS3_STRING,");
            }
            else
            {
                selectQuery.Append(" DEFECT1.PROC_DEFECT_SHORT AS ST_NG_DETAILS1_STRING,");
                selectQuery.Append(" DEFECT2.PROC_DEFECT_SHORT AS ST_NG_DETAILS2_STRING,");
                selectQuery.Append(" DEFECT3.PROC_DEFECT_SHORT AS ST_NG_DETAILS3_STRING,");
            }
            
            selectQuery.Append(" ST.ST_NG_BARS2, ");
            
            selectQuery.Append(" ST.ST_NG_BARS3, ");
            selectQuery.Append(" PROCK.KUBUN_NAME AS PROCESS_KUBUN_JP, ");
            selectQuery.Append(" PROCK.KUBUN_NAME_EN AS PROCESS_KUBUN_EN ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STAMPING ST ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = ST.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = ST.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = ST.ST_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON ST.ST_NG_DETAILS1 = DEFECT1.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON ST.ST_NG_DETAILS2 = DEFECT2.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON ST.ST_NG_DETAILS3 = DEFECT3.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = ST.ST_NEXT_PROCESS");

            var query = selectQuery.ToString();
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                if (rowLimit > 0)
                {
                    whereCondition = " WHERE ROWNUM <=" + rowLimit + " AND ";
                }
                else
                {
                    whereCondition = " WHERE ";
                }
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }
            else if (rowLimit > 0 && searchCriteria.Count <= 0)
            {
                whereCondition = " WHERE ROWNUM <=" + rowLimit;
                query = query + whereCondition;
            }
            
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            //else
            //{
            //    query += " ORDER BY ST_END_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result= ExecuteQueryMultiResult<StamppingDetails>(query);           
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Stampping process data according to search criteria        
        /// </returns>
        /// 
        public List<StamppingDetails> GetDailyProcessStamppingReport(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
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
            //selectQuery.Append(", INTE.INTERNAL_NAME AS PP_INTERNAL_NAME");
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
            selectQuery.Append(", PP.COIL_DIA_WINDING AS COIL_DIA_WINDING");

            selectQuery.Append(",  PP.NO_OF_UNITS AS PP_NO_OF_UNITS  ");
            selectQuery.Append(",  PP.UNIT_WT AS PP_UNIT_WT");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(",  PP.CHAMFER AS PP_CHAMFER");
            selectQuery.Append(",  PP.CORNER_RADIUS AS PP_CORNER_R");

            selectQuery.Append(", PP.MAGARI_KUBUN AS MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN AS MAGARI_SPAN");
            selectQuery.Append(", PP.NEJIRE_ANGLE AS NEJIRE_ANGLE");
            selectQuery.Append(", PP.NEJIRE_SPAN AS NEJIRE_SPAN");
            selectQuery.Append(", PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(", PP.UNIT_WT AS PP_UNIT_WT ");
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
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1 AS TENSILE_STRN_PATTERN1");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX");
            selectQuery.Append(", PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1");
            selectQuery.Append(", PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX ");

            selectQuery.Append(", PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN");
            selectQuery.Append(", PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX");
            selectQuery.Append(", PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1");
            selectQuery.Append(", PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE ");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN");
            selectQuery.Append(", PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX");
            selectQuery.Append(", PP.HARDNESS_PATTERN3_ACTUAL AS HARDNESS_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.YEILD_PATTERN1 AS YEILD_PATTERN1");
            selectQuery.Append(", PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN");
            selectQuery.Append(", PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX");
            selectQuery.Append(", PP.YEILD_PATTERN3_ACTUAL AS YEILD_PATTERN3_ACTUAL");
            selectQuery.Append(", PP.PRESS_PATTERN1 AS PRESS_PATTERN1");
            selectQuery.Append(", PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN");
            selectQuery.Append(", PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX");
            selectQuery.Append(", PP.PRESS_PATTERN3_ACTUAL AS PRESS_PATTERN3_ACTUAL");

            selectQuery.Append(", PP.DECARB_1 AS DECARB_1");
            selectQuery.Append(", PP.DECARB_VAL_1 AS DECARB_VAL_1");
            selectQuery.Append(", PP.DECARB_2 AS DECARB_2");
            selectQuery.Append(", PP.DECARB_VAL_2 AS DECARB_VAL_2");
            selectQuery.Append(", PP.DECARB_3 AS DECARB_3");
            selectQuery.Append(", PP.DECARB_VAL_3 AS DECARB_VAL_3");

            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX");
            selectQuery.Append(", PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL");

            selectQuery.Append(", PP.HT_CONDITION AS PP_HT_CONDITION");
            selectQuery.Append(", PP.RANK_STD AS RANK_STD");
            selectQuery.Append(", PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(", PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK ");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.MATL_DIM_D AS PP_MATL_DIM_D");
            selectQuery.Append(", PP.MATL_DIM_W AS PP_MATL_DIM_W");
            selectQuery.Append(", PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER");

            selectQuery.Append(", PP.MATL_LENGTH AS PP_MATL_LENGTH");
            selectQuery.Append(", CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION");
            selectQuery.Append(", PP.CHARGE AS PP_CHARGE");
            selectQuery.Append(", PP.PROCESS_PLAN_KUBUN AS PROCESS_PLAN_KUBUN");
            selectQuery.Append(",  PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN ");            
            selectQuery.Append(", PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN");
            selectQuery.Append(", PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN ");
            selectQuery.Append(", PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY");
            selectQuery.Append(", TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING");
            //
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(", PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY");
            selectQuery.Append(", TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING");
            selectQuery.Append(", TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR");
            //stampping                     
            selectQuery.Append(", MAC.MACHINE_NAME AS ST_MACHINE_NAME ");
            selectQuery.Append(", LOC.SHORT_NAME AS ST_LOCATION_NAME ");
            selectQuery.Append(", TO_CHAR( ST.ST_START_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_START_DATE_STRING");            
            selectQuery.Append(", EMP1.SHORT_NAME AS ST_START_OPERATOR_NAME");
            selectQuery.Append(", ST.ST_METALLIC_PLATE_CODE AS ST_METALLIC_PLATE_CODE");
            selectQuery.Append(", ST.ST_OK_BARS As ST_OK_BARS");
            selectQuery.Append(", ST.UNIT_TYPE AS ST_UNIT_TYPE");
            selectQuery.Append(", ST.ST_TRIAL_RESULT AS ST_TRIAL_RESULT");
            selectQuery.Append(", TO_CHAR( ST.ST_END_DATE,'yyyy/mm/dd HH24:MI' ) AS ST_END_DATE_STRING");                      
            selectQuery.Append(", EMP2.SHORT_NAME AS ST_END_OPERATOR_NAME");
            selectQuery.Append(", ST.ST_TABA_NO AS  ST_TABA_NO");
            selectQuery.Append(", ST.ST_OK_BARS AS ST_OK_BARS");
            selectQuery.Append(", ST.UNIT_TYPE AS UNIT_TYPE");
            selectQuery.Append(", ST.ST_OK_QTY AS ST_OK_QTY");
            selectQuery.Append(", ST.ST_NG_QTY AS ST_NG_QTY");
            selectQuery.Append(", ST.ST_NG_BARS1 AS ST_NG_BARS1");
            selectQuery.Append(", ST.ST_NG_BARS2 AS ST_NG_BARS2");
            selectQuery.Append(", ST.ST_NG_BARS3 AS ST_NG_BARS3");
            selectQuery.Append(", DEFECT1.PROC_DEFECT_SHORT AS ST_NG_DETAILS1_JP");
            selectQuery.Append(", DEFECT1.OTHER_NAME_SHORT AS ST_NG_DETAILS1_EN");
            selectQuery.Append(", DEFECT2.PROC_DEFECT_SHORT AS ST_NG_DETAILS2_JP");
            selectQuery.Append(", DEFECT2.OTHER_NAME_SHORT AS ST_NG_DETAILS2_EN");
            selectQuery.Append(", DEFECT3.PROC_DEFECT_SHORT AS ST_NG_DETAILS3_JP");
            selectQuery.Append(", DEFECT3.OTHER_NAME_SHORT AS ST_NG_DETAILS3_EN");
            selectQuery.Append(", ST.STAMP_DETAILS AS STAMP_DETAILS");
            selectQuery.Append(", ST.ST_DECISION AS ST_DECISION");
            selectQuery.Append(", M.MSG_CONTENTS AS ST_REMARKS ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER  JOIN TRN_STAMPING ST ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");           
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD");           
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD = PP.RECEIVER_CD");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD");
           // selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD");
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
                if (gridsearchresult.ROW_LIMIT > 0)
                {
                    whereCondition = " WHERE ROWNUM <=" + gridsearchresult.ROW_LIMIT + " AND ";
                }
                else
                {
                    whereCondition = " WHERE ";
                }
                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }

                query = query + whereCondition;
            }
            else if (gridsearchresult.ROW_LIMIT > 0 && searchCriteria.Count <= 0)
            {
                whereCondition = " WHERE ROWNUM <=" + gridsearchresult.ROW_LIMIT;
                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = query + " ORDER BY " + sortCiteria;
            }
            selectQuery = new StringBuilder();
             selectQuery.Append(query);
            return ExecuteQueryMultiResult<StamppingDetails>(query);
         }

       
        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/04       
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForStamppingFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_STAMPING ST ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = ST.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" INNER JOIN MST_MACHINE MAC ON MAC.MACHINE_CD = ST.MACHINE_CD ");
            selectQuery.Append(" INNER JOIN MST_LOCATION LOC ON LOC.LOCATION_CD = ST.LOCATION_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD = ST.ST_END_OPERATOR_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTERN ON INTERN.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT1 ON ST.ST_NG_DETAILS1 = DEFECT1.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT2 ON ST.ST_NG_DETAILS2 = DEFECT2.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_DEFECT DEFECT3 ON ST.ST_NG_DETAILS3 = DEFECT3.PROCESS_DEFECT_CD ");
            selectQuery.Append(" LEFT JOIN MST_PROCESS_KUBUN PROCK ON PROCK.KUBUN_ID = ST.ST_NEXT_PROCESS");

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

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }

        /// <summary>
        /// CREATED BY : Pallavi Gawade on 2024/07/05       
        /// used screens: Daily Process Analysis (CSV Export)
        /// </summary>
        /// <returns>
        /// Get row Count of selected search criteria 
        /// This method is use to check data row count if row count is greater than max limit (enter limit in text box on screen) then show warning message
        /// </returns>
        /// 
        public int GetRowCountForStamppingDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");  
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP");
            selectQuery.Append(" INNER  JOIN TRN_STAMPING ST ON ST.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
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

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }
              

    }
}
