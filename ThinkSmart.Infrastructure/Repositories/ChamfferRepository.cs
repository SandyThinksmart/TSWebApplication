using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Enums;
using TSWebApplication.Domain.Resources;
using Microsoft.Extensions.Primitives;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ChamfferRepository : BaseRepository, IChamfferRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly int days = 0;
        public ChamfferRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            days = Convert.ToInt32(_configuration.GetSection("PastDaysForSearchChuuban").Value);
        }
        public List<ChamferDetails> GetChamferingList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD)
        {
            List<ChamferDetails> result = new List<ChamferDetails>();
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN"); 
            selectQuery.Append(", CH.CHAMFERING_CD");
            selectQuery.Append(", CH.CH_TABA_NO_1");
            selectQuery.Append(",CONCAT(CH.CH_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE  WHEN CH.CH_TABA_NO_2 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', CH.CH_TABA_NO_2) ");
            selectQuery.Append(" ELSE   NULL ");
            selectQuery.Append(" END, CASE ");
            selectQuery.Append(" WHEN CH.CH_TABA_NO_3 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', CH.CH_TABA_NO_3) ");
            selectQuery.Append(" ELSE NULL  END   ),");
            selectQuery.Append(" CASE  WHEN CH.CH_TABA_NO_4 IS NOT NULL THEN ");
            selectQuery.Append(" CONCAT(', ', CH.CH_TABA_NO_4) ");
            selectQuery.Append(" ELSE   NULL  END ");
            selectQuery.Append(" )) AS TABA_NUMBERS ");
            selectQuery.Append(", TO_CHAR(CH.CH_START_DATE, 'yyyy/mm/dd HH24:MI') AS CH_START_DATE_STRING");
            selectQuery.Append(", EMP.SHORT_NAME AS CH_START_OPERATOR_NAME");
            selectQuery.Append(", CST.SHORT_NAME  AS PP_CUSTOMER_NAME");
            selectQuery.Append(", PP.PROD_NAME_CUST  AS PP_PROD_NAME_CUST");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D, PP.PROD_DIM_W, NULL, ");
            selectQuery.Append(" CASE WHEN PP.PROD_DIM_OTHER IS NOT NULL THEN ");
            selectQuery.Append(" PP.PROD_DIM_OTHER  ");
            selectQuery.Append(" END  || ");
            selectQuery.Append(" CASE  ");
            selectQuery.Append(" WHEN PP.CHAMFER IS NOT NULL ");
            selectQuery.Append(" AND PP.PROD_DIM_OTHER IS NOT NULL THEN ");
            selectQuery.Append(" ',C-' || PP.CHAMFER ");
            selectQuery.Append(" END  || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.CHAMFER IS NOT NULL ");
            selectQuery.Append(" AND PP.PROD_DIM_OTHER IS NULL THEN ");
            selectQuery.Append(" 'C-' || PP.CHAMFER ");
            selectQuery.Append(" END || ");
            selectQuery.Append(" CASE ");
            selectQuery.Append(" WHEN PP.CORNER_RADIUS IS NOT NULL ");
            selectQuery.Append(" AND(PP.CHAMFER IS NOT NULL ");
            selectQuery.Append(" OR PP.PROD_DIM_OTHER IS NOT NULL) THEN ");
            selectQuery.Append(" ',R-' || PP.CORNER_RADIUS ");
            selectQuery.Append(" END || CASE ");
            selectQuery.Append(" WHEN PP.CORNER_RADIUS IS NOT NULL ");
            selectQuery.Append(" AND PP.CHAMFER IS NULL ");
            selectQuery.Append(" AND PP.PROD_DIM_OTHER IS NULL THEN ");
            selectQuery.Append(" 'R-' || PP.CORNER_RADIUS ");
            selectQuery.Append(" END ");
            selectQuery.Append(" ) AS PP_PROD_DIM_D_STRING");
            selectQuery.Append(", PP.PROD_SHAPE AS PP_PROD_SHAPE");
            selectQuery.Append(", PP.PROD_LENGTH  AS PP_PROD_LENGTH");
            selectQuery.Append(", PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK");
            selectQuery.Append(", GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D, PP.MATL_DIM_W, MATL_SHAPE, PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_STRING");
            selectQuery.Append(", PP.MATL_SHAPE AS PP_MATL_SHAPE");
            selectQuery.Append(", PP.CHARGE  AS PP_CHARGE");
            selectQuery.Append(", CH.CH_METALLIC_PLATE_CODE   AS CH_METALLIC_PLATE_CODE ");
            selectQuery.Append(", CH.CH_FINISHED_BARS");
            selectQuery.Append(", CH.UNIT_TYPE");
            selectQuery.Append(", CH.CH_FINISHED_QTY");
            selectQuery.Append(", CH.CH_DECISION");

            selectQuery.Append(" ,(SELECT COUNT(PP.PROCESS_PLAN_CD)");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN P INNER JOIN TRN_CHAMFERING CHA ");
            selectQuery.Append(" ON P.PROCESS_PLAN_CD = CHA.PROCESS_PLAN_CD");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EM ON CHA.CH_START_OPERATOR_CD = EM.EMPLOYEE_CD");
            selectQuery.Append(" WHERE CHA.CH_START_OPERATOR_CD IS NOT NULL ) ");
            selectQuery.Append(" AS ROW_COUNT ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_CHAMFERING   CH  ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD=CH.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_EMPLOYEE EMP ");
            selectQuery.Append(" ON CH.CH_START_OPERATOR_CD=EMP.EMPLOYEE_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD= CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD =PP.INTERNAL_NAME_CD");


            selectQuery.Append(" WHERE CH.INPUT_LOCATION_CD =" + locationCD + " AND CH.CH_END_DATE IS NOT NULL ");
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" AND ROWNUM <= ");
            selectQuery.Append(" (CASE");
            selectQuery.Append(" WHEN (ROWNUM >" + _maxRowLimit + ")  THEN " + _maxRowLimit + "");
            selectQuery.Append(" ELSE ROWNUM");
            selectQuery.Append(" END)");

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
                query += " ORDER BY PP_CHUUBAN_GYOUBAN, CH.CH_TABA_NO_1";
            }
            result = ExecuteQueryMultiResult<ChamferDetails>(query);
            return result;
        }

        public ChamferDetails GetChamferingDetailsByCD(long chamferingCD)
        {
            ChamferDetails result = new ChamferDetails();
            StringBuilder selectQuery = new StringBuilder();
            //  selectQuery.Append("  SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN "); 
            selectQuery.Append(" SELECT GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(",  PP.CHUUBAN AS PP_CHUUBAN  ");
            selectQuery.Append(",  PP.GYOUBAN	AS	PP_GYOUBAN  ");
            selectQuery.Append(",  PP.PROD_SPEC_NO	AS	PP_PROD_SPEC_NO  ");
            selectQuery.Append(",  PP.DELIVERY_DATE	AS	PP_DELIVERY_DATE  ");
            selectQuery.Append(",  CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(",  TO_CHAR(PP.DELIVERY_DATE,'yyyy/mm/dd HH24:MI' )	AS	PP_DELIVERY_DATE_STRING  ");
            selectQuery.Append(", CASE   WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL");
            selectQuery.Append(" THEN EMP.SHORT_NAME   || ' '|| '(' || LOC.SHORT_NAME || ' ' || DEP.DEPARTMENT_NAME  || ')' END AS PP_EMPLOYEE_NAME ");
            selectQuery.Append(",  REC.SHORT_NAME	AS	PP_RECEIVER_NAME  ");
            selectQuery.Append(",  CST.SHORT_NAME	AS	PP_CUSTOMER_NAME  ");
            selectQuery.Append(",  PP.CHAMFER	AS	PP_CHAMFER  ");
            selectQuery.Append(",  PP.NYUKA_CD AS PP_NYUKA_CD  ");
            selectQuery.Append(",  pp.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(",  PP.COLOR_ID	AS	PP_COLOR  ");
            selectQuery.Append(",  PP.MATL_NAME_MAK	AS	PP_MATL_NAME_MAK  ");
            selectQuery.Append(",  PP.MATL_SHAPE	AS	PP_MATL_SHAPE  ");
            selectQuery.Append(",  PP.MATL_DIM_D	AS	PP_MATL_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W ,null,null) AS PP_MATL_DIM_D_STRING ");
            selectQuery.Append(",  PP.CHARGE	AS	PP_CHARGE  ");
            selectQuery.Append(",  PP.MAGARI_KUBUN	AS	PP_MAGARI_KUBUN  ");
            selectQuery.Append(",  PP.MAGARI_BEND_TOL	AS	PP_BEND_TOL  ");
            selectQuery.Append(",  PP.MAGARI_SPAN	AS	PP_MAGARI_SPAN  ");
            selectQuery.Append(",  PP.PROD_SHAPE	AS	PP_PROD_SHAPE  ");
            selectQuery.Append(",  PP.PROD_DIM_D	AS	PP_PROD_DIM_D  ");
            selectQuery.Append(",  GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,NULL,PP.PROD_DIM_OTHER) AS PP_PROD_DIM_D_STRING ");
            selectQuery.Append(",  PP.PROD_NAME_CUST	AS	PP_PROD_NAME_CUST  ");
            selectQuery.Append(",  PP.PROD_DIM_OTHER	AS	PP_PROD_DIM_OTHER  ");
            selectQuery.Append(",  PP.EXT_TEXTURE	AS	PP_EXT_TEXTURE  ");
            selectQuery.Append(",  PP.DIM_TOL_MIN_CHR	AS	PP_DIM_TOL_MIN_CHR  ");
            selectQuery.Append(",  PP.DIM_TOL_MAX_CHR	AS	PP_DIM_TOL_MAX_CHR  ");
            selectQuery.Append(", (CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(",  PP.DISPLAY	AS	PP_DISPLAY  ");
            selectQuery.Append(",  PP.STAMP	AS	PP_STAMP  ");
            selectQuery.Append(",  PP.PROD_LENGTH	AS	PP_PROD_LENGTH  ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(",  PP.LENGTH_TOL_MIN_CHR	AS	PP_LENGTH_TOL_MIN_CHR  ");
            selectQuery.Append(",  PP.LENGTH_TOL_MAX_CHR	AS	PP_LENGTH_TOL_MAX_CHR  ");
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(",  PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(",  PCK.PACKING_NAME  AS PP_PACKING_DECODE  ");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");

            selectQuery.Append(",  PP.NO_OF_UNITS	AS	PP_NO_OF_UNITS  ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT ");
            selectQuery.Append(",  PP.ACT_ALLOC_QTY	AS	PP_ACT_ALLOC_QTY  ");
            selectQuery.Append(",  PP.COIL_DIA_INTERNAL AS	PP_COIL_DIA_INTERNAL  ");
            selectQuery.Append(",  PP.COIL_DIA_EXTERNAL	AS	PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING ");
            //selectQuery.Append(",  K.REMARKS	AS	PP_KAKOUIRAI_REMARKS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_REMARK	AS	PP_PROCESS_PLAN_REMARK  ");
            selectQuery.Append(",  PP.STD_DEL_PERIOD	AS	PP_STD_DEL_PERIOD  ");
            selectQuery.Append(",  PP.CREATED_DATE	AS	PP_CREATED_DATE  ");
            selectQuery.Append(",  PP.UPDATED_DATE	AS	PP_UPDATED_DATE  ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");
            selectQuery.Append(",  PP.PROCESS_PLAN_CD	AS	PP_PROCESS_PLAN_CD  ");

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");
            selectQuery.Append(", M.MSG_CONTENTS AS CH_REMARKS  ");
            selectQuery.Append(", M.MSG_CD   ");
            selectQuery.Append(", CH.MACHINE_CD  ");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_MACHINE WHERE MACHINE_CD = CH.MACHINE_CD) AS CH_MACHINE_NAME ");
            selectQuery.Append(", CH.CHAMFERING_CD");
            selectQuery.Append(", TO_CHAR( CH.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_CREATED_DATE_STRING");
            //selectQuery.Append(", ST.UPDATED_DATE	AS	STR_UPDATED_DATE  ");
            selectQuery.Append(", TO_CHAR( CH.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_UPDATED_DATE_STRING");
            selectQuery.Append(", CH.CH_START_DATE AS CH_START_DATE ");
            selectQuery.Append(", TO_CHAR( CH.CH_START_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_START_DATE_STRING");
            selectQuery.Append(", TO_CHAR( CH.CH_END_DATE ,'yyyy/mm/dd HH24:MI' ) AS CH_END_DATE_STRING");
            selectQuery.Append(", CH.CH_END_DATE  AS CH_END_DATE  ");
            selectQuery.Append(", CH.CH_START_OPERATOR_CD ");
            selectQuery.Append(", CH.CH_END_OPERATOR_CD");

            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = CH.CH_START_OPERATOR_CD) AS CH_START_OPERATOR_NAME");
            selectQuery.Append(", (SELECT SHORT_NAME FROM MST_EMPLOYEE WHERE EMPLOYEE_CD = CH.CH_END_OPERATOR_CD) AS CH_END_OPERATOR_NAME");

            selectQuery.Append(", CH.CH_METALLIC_PLATE_CODE");
            //selectQuery.Append(",NY.PRODUCT_NYUKO_CD ");
            selectQuery.Append(", CH.CH_SHAPE");
            selectQuery.Append(", CH.FURNACE_NO");
            selectQuery.Append(", CH.CH_ACT_LENGTH");
            selectQuery.Append(", CH.RECHAMFERING");
            selectQuery.Append(", CH.CH_DECISION  ");
            selectQuery.Append(", CH.CH_FINISHED_BARS");
            selectQuery.Append(", CH.UNIT_TYPE");
            selectQuery.Append(", CH.CH_FINISHED_QTY");
            selectQuery.Append(", CH.CH_NEXT_PROCESS ");
            selectQuery.Append(", CH_TABA_NO_1");
            selectQuery.Append(", CH_TABA_NO_2");
            selectQuery.Append(", CH_TABA_NO_3");
            selectQuery.Append(", CH_TABA_NO_4");
            selectQuery.Append(", TO_CHAR( CH.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_CREATED_DATE_STRING");
            selectQuery.Append(", TO_CHAR( CH.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_UPDATED_DATE_STRING");
            selectQuery.Append("  FROM TRN_CHAMFERING CH  ");
            selectQuery.Append("  LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD =CH.PROCESS_PLAN_CD ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");
            selectQuery.Append("  LEFT JOIN TRN_MESSAGE M  ");
            selectQuery.Append("  ON M.MSG_CD = CH.MSG_CD  ");
            //selectQuery.Append(" LEFT JOIN TRN_KAKOUIRAI K ");
            //selectQuery.Append(" ON PP.KAKOUIRAI_CD = K.KAKOUIRAI_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CST on PP.CUSTOMER_CD = CST.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON PP.RECEIVER_CD = REC.RECEIVER_CD ");
            selectQuery.Append(" LEFT JOIN MST_CONDITION CON ON CON.CONDITION_CD = PP.PROD_CONDITION_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DG ON PP.DEPARTMENT_GROUP_CD = DG.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP  ON DG.EMPLOYEE_CD = EMP.EMPLOYEE_CD ");
            selectQuery.Append("LEFT JOIN MST_DEPARTMENT DEP ");
            selectQuery.Append("ON DEP.DEPARTMENT_CD = dg.DEPARTMENT_CD ");
            selectQuery.Append("LEFT JOIN MST_LOCATION LOC ");
            selectQuery.Append("ON LOC.LOCATION_CD = dg.LOCATION_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            //selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");



            selectQuery.Append("  WHERE ");
            selectQuery.Append("  CH.CHAMFERING_CD =" + chamferingCD);
            result = ExecuteQuerySingleResult<ChamferDetails>(selectQuery.ToString());
            return result;
        }

        public List<ThabaDetails> GetThabaDetailsWithOKDecision(string chuuban, string gyouban, long chamferingCD)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, CHA.CH_DECISION AS DECISION, CHA.CREATED_DATE ,");
            selectQuery.Append(" TO_CHAR(CHA.CH_TABA_NO_1) AS TABA_NO_1,");
            selectQuery.Append(" TO_CHAR(CHA.CH_TABA_NO_2) AS TABA_NO_2,");
            selectQuery.Append(" TO_CHAR(CHA.CH_TABA_NO_3) AS TABA_NO_3,");
            selectQuery.Append(" TO_CHAR(CHA.CH_TABA_NO_4) AS TABA_NO_4");
            selectQuery.Append(" FROM TRN_CHAMFERING CHA INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = CHA.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            selectQuery.Append(" WHERE CHA.CH_DECISION = " + decisionVal);
            selectQuery.Append(" AND PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            if (chamferingCD != 0)
            {
                selectQuery.Append(" AND CHA.CHAMFERING_CD !='" + chamferingCD + "' ");
            }
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }

        public void Delete(long chamferingCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_CHAMFERING ");
            deleteQuery.Append(" WHERE CHAMFERING_CD =" + chamferingCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public long ChamferDetailsUpdate(ChamferDetails chamferDetails)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_CHAMFERING SET ");

            // Start data is updated

            updateQuery.Append(" LOCATION_CD ='" + chamferDetails.LOCATION_CD + "',");
            updateQuery.Append(" MSG_CD ='" + chamferDetails.CH_MSG_CD + "',");
            updateQuery.Append(" CH_START_DATE = ");
            //updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(chamferDetails.CH_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')" + ",");
            updateQuery.Append(" TO_DATE('" + (Convert.ToDateTime(chamferDetails.CH_START_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");

            updateQuery.Append(" CH_START_OPERATOR_CD = '" + chamferDetails.CH_START_OPERATOR_CD + "',");
            updateQuery.Append(" CH_METALLIC_PLATE_CODE ='" + chamferDetails.CH_METALLIC_PLATE_CODE + "',");
            updateQuery.Append(" CH_SHAPE='" + chamferDetails.CH_SHAPE + "',");
            updateQuery.Append(" FURNACE_NO ='" + chamferDetails.FURNACE_NO + "',");
            updateQuery.Append(" RECHAMFERING ='" + chamferDetails.RECHAMFERING + "',");
            updateQuery.Append(" CH_ACT_LENGTH='" + chamferDetails.CH_ACT_LENGTH + "',");
            updateQuery.Append(" MACHINE_CD ='" + chamferDetails.MACHINE_CD + "',");

            //End data is updated 
            updateQuery.Append(" CH_END_OPERATOR_CD ='" + chamferDetails.CH_END_OPERATOR_CD + "',");
            updateQuery.Append(" CH_END_DATE= ");
            //updateQuery.Append(" TO_DATE('" + Convert.ToDateTime(chamferDetails.CH_END_DATE).ToString(Constants.DateFormats.YYYYMMDD_HHMMSS) + "','yyyy/mm/dd HH:MI:SS')" + ",");
            updateQuery.Append(" TO_DATE('" + (Convert.ToDateTime(chamferDetails.CH_END_DATE, new CultureInfo(Constants.Culture.jaJP, true))).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')" + ",");

            updateQuery.Append(" CH_FINISHED_BARS = '" + chamferDetails.CH_FINISHED_BARS + "',");
            updateQuery.Append(" UNIT_TYPE  = '" + chamferDetails.UNIT_TYPE + "',");
            updateQuery.Append(" CH_FINISHED_QTY= '" + chamferDetails.CH_FINISHED_QTY + "',");
            updateQuery.Append(" CH_TABA_NO_1 = '" + chamferDetails.CH_TABA_NO_1 + "',");
            updateQuery.Append(" CH_TABA_NO_2  = '" + chamferDetails.CH_TABA_NO_2 + "',");
            updateQuery.Append(" CH_TABA_NO_3  = '" + chamferDetails.CH_TABA_NO_3 + "',");
            updateQuery.Append(" CH_TABA_NO_4 = '" + chamferDetails.CH_TABA_NO_4 + "',");
            updateQuery.Append(" CH_DECISION='" + chamferDetails.CH_DECISION + "',");
            updateQuery.Append(" CH_NEXT_PROCESS= '" + chamferDetails.CH_NEXT_PROCESS + "',");
            updateQuery.Append(" USER_ID ='" + chamferDetails.USER_ID + "',");
            updateQuery.Append(" INPUT_LOCATION_CD ='" + (chamferDetails.INPUT_LOCATION_CD != null ? chamferDetails.INPUT_LOCATION_CD.ToString() : string.Empty) + "',");
            updateQuery.Append(" UPDATED_DATE = SYSDATE");

            updateQuery.Append(" WHERE ");
            updateQuery.Append(" CHAMFERING_CD= '" + chamferDetails.CHAMFERING_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());

            object cdVal = ExecuteScalar("SELECT MSG_CD FROM TRN_CHAMFERING WHERE CHAMFERING_CD= '" + chamferDetails.CHAMFERING_CD + "'");

            long msgCD = 0;

            if (!string.IsNullOrEmpty(Convert.ToString(cdVal)))
            {
                msgCD = Convert.ToInt64(cdVal);
            }
            return msgCD;
        }

        public List<ChamferDetails> GetChamfering24MachineList(string machineCd)
        {
            List<ChamferDetails> result = new List<ChamferDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT  ");
            selectQuery.Append(" CHA.MACHINE_CD,  ");
            selectQuery.Append(" CHA.CH_DECISION ,	");
            selectQuery.Append(" CUS.SHORT_NAME AS  PP_CUSTOMER_NAME,	");
            // selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, "); 
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" CHA.CH_METALLIC_PLATE_CODE,	");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST,	");
            selectQuery.Append("  PP.CHARGE AS PP_CHARGE,	");
            // selectQuery.Append("  PP.PROD_DIM_D|| ''||PP.PROD_SHAPE  AS PP_PROD_SHAPE,	");
            selectQuery.Append(" GET_DIM_D_W_SHAPE_OTHER(PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_SHAPE,PP.PROD_DIM_OTHER) AS PP_PROD_SHAPE,");
            selectQuery.Append("  PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO,	");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" CHA.CH_SHAPE,	");
            selectQuery.Append(" CONCAT (CH_TABA_NO_1, CASE WHEN CH_TABA_NO_2 IS NOT NULL THEN CONCAT ('-', CH_TABA_NO_2) ELSE NULL END) AS CHA_TABA_NO_1_2,");
            selectQuery.Append(" CONCAT(CASE WHEN CH_TABA_NO_3 IS NOT NULL THEN CH_TABA_NO_3 ELSE NULL END,");
            selectQuery.Append(" CASE WHEN CH_TABA_NO_4 IS NOT NULL THEN CONCAT ('-', CH_TABA_NO_4) ELSE NULL END ) AS CHA_TABA_NO_3_4 , ");
            selectQuery.Append("  CHA.CH_FINISHED_BARS,	");
            selectQuery.Append("  CHA.UNIT_TYPE , ");
            selectQuery.Append("  CHA.CH_FINISHED_QTY,	");
            selectQuery.Append("  MSG.MSG_CONTENTS AS CH_REMARKS, "); 
            selectQuery.Append("  NVL(CHA.CH_END_DATE,CHA.CH_START_DATE) AS ORDER_CLUASE");
            selectQuery.Append(" FROM TRN_CHAMFERING CHA ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP");
            selectQuery.Append(" ON CHA.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON CHA.MSG_CD=MSG.MSG_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");

            selectQuery.Append(" WHERE  CHA.MACHINE_CD  = '" + machineCd + "' AND NVL (CHA.CH_END_DATE, CHA.CH_START_DATE) > SYSDATE - 1");
            // selectQuery.Append("  AND ROWNUM <= 10 ");
            selectQuery.Append(" ORDER BY ORDER_CLUASE DESC ");
            result = ExecuteQueryMultiResult<ChamferDetails>(selectQuery.ToString());
            return result;
        }


        public ChamferDetails GetChamferDetailsByChuubanGyoubanMachine(string chuuban, short gyouban, string machineCD, string nifudaNo)
        {
            int count;
            StringBuilder selectQueryCount = new StringBuilder();
            string whereCondition1 = " PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL";
            string whereCondition2 = "  AND CH.MACHINE_CD = '" + machineCD + "' AND CH.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + "";
            string finalWhere = whereCondition1;
            finalWhere = whereCondition1 + whereCondition2;
            //SQL
            selectQueryCount.Append(" SELECT COUNT(*) ");
            selectQueryCount.Append(" FROM ");
            selectQueryCount.Append(" TRN_PROCESS_PLAN PP ");
            selectQueryCount.Append("  LEFT JOIN TRN_CHAMFERING CH ON CH.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
            selectQueryCount.Append(" WHERE " + finalWhere);
            count = Convert.ToInt32(ExecuteScalar(selectQueryCount.ToString()));

            if (count > 1)
            {
                finalWhere = whereCondition1 + whereCondition2;
            }
            else
            {
                finalWhere = whereCondition1;
            }


            ChamferDetails chamferDetails = new ChamferDetails();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
         ////   selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(", GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
            selectQuery.Append(" ,PP.CHAMFER AS PP_CHAMFER ");
            selectQuery.Append(" ,STD_DEL_PERIOD AS PP_STD_DEL_PERIOD");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,RE.SHORT_NAME AS PP_RECEIVER_NAME ");
            selectQuery.Append(" ,CASE WHEN PP.DEPARTMENT_GROUP_CD IS NOT NULL   THEN EMP.SHORT_NAME ||' ' ||'(' || LOC.SHORT_NAME || ' '|| DEP.DEPARTMENT_NAME || ')' END AS  PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,CON.CONDITION_DESCRIPTION AS PP_PRODUCT_CONDITION");
            selectQuery.Append(" ,PP.PROD_SHAPE AS PP_PROD_SHAPE ");
            selectQuery.Append(" ,GET_PROCESS_LIST_BY_PROD_CD(PP.PROCESS_PLAN_CD ) AS PP_PRODUCTION_PROCESS  ");

            selectQuery.Append(" ,(CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS PP_DIM_TOLERANCE ");
            selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D");
            selectQuery.Append(" ,PP.DIM_TOL_MAX_CHR  AS PP_DIM_TOL_MAX_CHR");
            selectQuery.Append(" ,PP.DIM_TOL_MIN_CHR  AS PP_DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.PROD_LENGTH AS PP_PROD_LENGTH ");
            selectQuery.Append(" , NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MAX_CHR AS PP_LENGTH_TOL_MAX_CHR ");
            selectQuery.Append(" ,PP.LENGTH_TOL_MIN_CHR  AS PP_LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY as PP_ACT_ALLOC_QTY  ");    //COIL_QTY

            selectQuery.Append(", PP.MAGARI_KUBUN");
            selectQuery.Append(", PP.MAGARI_BEND_TOL");
            selectQuery.Append(", PP.MAGARI_SPAN");

            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(", PP.NYUKA_CD AS PP_NYUKA_CD  ");
            selectQuery.Append(",  pp.KAKOUIRAI_REMARK AS PP_KAKOUIRAI_REMARKS");
            selectQuery.Append(" ,PP.UNIT_WT  AS PP_UNIT_WT ");
            selectQuery.Append(" ,PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN ");
            selectQuery.Append(" ,PP.UNIT_WT_MIN AS PP_UNIT_WT_MIN ");
            selectQuery.Append(" ,PP.UNIT_WT_MAX AS PP_UNIT_WT_MAX");
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
            selectQuery.Append(" ,(CASE WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append("PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.LENGTH_TOL_MIN_CHR END ) AS PP_LEN_TOL ");
            selectQuery.Append(" ,PCK.PACKING_NAME	AS	PP_PACKING  ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(", PP.UNIT AS PP_UNIT");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");
            selectQuery.Append(" ,PP.COIL_DIA_INTERNAL  AS PP_COIL_DIA_INTERNAL ");
            selectQuery.Append(" ,PP.COIL_DIA_EXTERNAL AS PP_COIL_DIA_EXTERNAL ");
            selectQuery.Append(", PP.COIL_DIA_WINDING");
           // selectQuery.Append(" ,K.REMARKS AS PP_KAKOUIRAI_REMARKS ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_REMARK  AS PP_PROCESS_PLAN_REMARK ");
            selectQuery.Append(" ,TO_CHAR(PP.PROCESS_PLAN_CMPL_DATE,'yyyy/mm/dd') AS PP_CMPL_DATE_STRING");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE");
            selectQuery.Append(", CH.CHAMFERING_CD");
            selectQuery.Append(" ,TO_CHAR( CH.CH_START_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_START_DATE_STRING ");
            selectQuery.Append(" ,TO_CHAR( CH.CH_END_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_END_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( CH.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_CREATED_DATE_STRING ");
            selectQuery.Append(",TO_CHAR( CH.UPDATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CH_UPDATED_DATE_STRING ");
            selectQuery.Append(" ,CH.UPDATED_DATE  UPDATED_DATE ");
            selectQuery.Append(", CH.CREATED_DATE ");
            selectQuery.Append(" , CH.CH_START_OPERATOR_CD");
            selectQuery.Append(" ,(SELECT EM.SHORT_NAME FROM MST_EMPLOYEE EM  WHERE EM.EMPLOYEE_CD = CH_START_OPERATOR_CD) AS CH_START_OPERATOR_NAME");
            selectQuery.Append(",(SELECT MACHINE_NAME  FROM MST_MACHINE  WHERE MACHINE_CD =CH.MACHINE_CD ) AS CH_MACHINE_NAME");
            selectQuery.Append(" , CH.CH_METALLIC_PLATE_CODE ");
            selectQuery.Append(" ,CH_SHAPE ");
            selectQuery.Append(" ,CH.FURNACE_NO ");
            selectQuery.Append(" ,CH.CH_ACT_LENGTH ");
            selectQuery.Append(" ,P1.PROCESS_NAME AS PP_PROCESS_1, ");
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

            selectQuery.Append(" P1.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_1, ");
            selectQuery.Append(" P2.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_2, ");
            selectQuery.Append(" P3.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_3, ");
            selectQuery.Append(" P4.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_4, ");
            selectQuery.Append(" P5.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_5, ");
            selectQuery.Append(" P6.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_6, ");
            selectQuery.Append(" P7.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_7, ");
            selectQuery.Append(" P8.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_8, ");
            selectQuery.Append(" P9.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_9, ");
            selectQuery.Append(" P10.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_10, ");
            selectQuery.Append(" P11.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_11, ");
            selectQuery.Append(" P12.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_12, ");
            selectQuery.Append(" P13.PROCESS_PROD_POINT_SUB AS PP_PROD_POINT_SUB_13, ");

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
            selectQuery.Append("PP.IMPACT_TEST_PATTERN3_ACTUAL");
            selectQuery.Append(",CH.RECHAMFERING ");
            selectQuery.Append(",CH.CH_TABA_NO_1");
            selectQuery.Append(",CH.CH_TABA_NO_2");
            selectQuery.Append(",CH.CH_TABA_NO_3");
            selectQuery.Append(",CH.CH_TABA_NO_4");
            selectQuery.Append(",CH.CH_DECISION ");
            selectQuery.Append(", (SELECT CH.UNIT_TYPE FROM TRN_CHAMFERING CH INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = CH.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE  PP.PROCESS_PLAN_CMPL_DATE IS NULL  AND PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "' AND CH.MACHINE_CD = '" + machineCD + "' ");
            selectQuery.Append(" AND CH.UNIT_TYPE IS NOT NULL AND CH.CREATED_DATE >= TO_DATE(SYSDATE) - " + days + " ORDER BY  CH.CHAMFERING_CD DESC FETCH FIRST ROW ONLY)  AS UNIT_TYPE ");
            selectQuery.Append(",GET_DIM_D_W_SHAPE_OTHER(PP.MATL_DIM_D,PP.MATL_DIM_W,PP.MATL_SHAPE,PP.MATL_DIM_OTHER) AS PP_MATL_DIM_D_SHAPE ");
            //selectQuery.Append(",NY.PRODUCT_NYUKO_CD ");
            selectQuery.Append(",CASE ");
            selectQuery.Append(" WHEN CH.CREATED_DATE >= TO_DATE(SYSDATE) -" + days + " THEN ");
            selectQuery.Append(" 1");
            selectQuery.Append(" ELSE");
            selectQuery.Append(" 0");
            selectQuery.Append(" END AS SHOW_DEFAULT");
            if (count == 0)
            {
                selectQuery.Append(" ,'NO' AS PAST_DATA_EXIST ");
            }
            else
            {
                selectQuery.Append(" ,'YES' AS PAST_DATA_EXIST ");
            }

            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append("  LEFT JOIN TRN_CHAMFERING CH ON CH.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
           // selectQuery.Append(" AND CH.MACHINE_CD = '" + machineCD + "' ");
            //selectQuery.Append("  LEFT JOIN TRN_KAKOUIRAI K  ");
            //selectQuery.Append("  ON PP.KAKOUIRAI_CD   = K.KAKOUIRAI_CD  ");
            selectQuery.Append("  LEFT JOIN MST_PACKING PCK  ");
            selectQuery.Append("  ON PCK.PACKING_CD = PP.PACKING_CD  ");

            selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INTE ON INTE.INTERNAL_NAME_CD= PP.INTERNAL_NAME_CD");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER RE ON RE.RECEIVER_CD =PP.RECEIVER_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT_GROUP DGPGR    ON DGPGR.DEPARTMENT_GROUP_CD =pp.DEPARTMENT_GROUP_CD ");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DGPGR.EMPLOYEE_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" LEFT JOIN MST_DEPARTMENT DEP  ON DEP.DEPARTMENT_CD =DGPGR.DEPARTMENT_CD");
            selectQuery.Append(" LEFT JOIN MST_LOCATION LOC ON LOC.LOCATION_CD =DGPGR.LOCATION_CD");
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
            selectQuery.Append(" LEFT JOIN TRN_NIFUDA NI ON NI.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
           // selectQuery.Append(" LEFT JOIN TRN_PROD_NYUKO NY ON NY.NIFUDA_CD = NI.NIFUDA_CD");

            selectQuery.Append(" WHERE " + finalWhere);
            //selectQuery.Append(" AND PP.GYOUBAN= '" + gyouban + "'");

            if (!string.IsNullOrEmpty(nifudaNo))
            {
                selectQuery.Append(" AND NI.NIFUDA_NO= '" + nifudaNo + "'");
            }
         //   selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            selectQuery.Append(" ORDER BY CH.CHAMFERING_CD DESC");
            selectQuery.Append(" FETCH FIRST 1 ROW ONLY ");
            chamferDetails = ExecuteQuerySingleResult<ChamferDetails>(selectQuery.ToString());
            return chamferDetails;
        }

        public List<ThabaDetails> GetThabaDetails(string chuuban, string gyouban)
        {
            List<ThabaDetails> result = new List<ThabaDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT * FROM ( ");
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PP.CHUUBAN  ");
            selectQuery.Append(" ,PP.GYOUBAN ");
            selectQuery.Append(" ,CHA.CH_DECISION AS DECISION");
            selectQuery.Append(" ,CHA.CHAMFERING_CD");
            selectQuery.Append(" ,CHA.CREATED_DATE ");
            selectQuery.Append(" , CONCAT (CHA.CH_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            selectQuery.Append(" ,DECODE(CHA.CH_DECISION,0,1,2,3) AS PROCESS_STATUS, ");
            selectQuery.Append(" ROW_NUMBER() OVER ( ");
            selectQuery.Append(" PARTITION BY CONCAT(CHA.CH_TABA_NO_1, CONCAT(CONCAT( ");
            selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_2 IS NOT NULL THEN CONCAT(', ', CHA.CH_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_3 IS NOT NULL THEN CONCAT(', ', CHA.CH_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE  WHEN CHA.CH_TABA_NO_4 IS NOT NULL THEN CONCAT(', ', CHA.CH_TABA_NO_4) ELSE NULL END )) ");
            selectQuery.Append(" ORDER BY CHA.UPDATED_DATE DESC ) RN ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_CHAMFERING CHA INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = CHA.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" ) WHERE RN = 1  AND DECISION IS NOT NULL");
            //selectQuery.Append(" SELECT PP.CHUUBAN, PP.GYOUBAN, CHA.CH_DECISION AS DECISION, CHA.CREATED_DATE ,");
            //selectQuery.Append(" CONCAT (CHA.CH_TABA_NO_1, CONCAT (CONCAT ( ");
            //selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_2) ELSE NULL END, ");
            //selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_3) ELSE NULL END ), ");
            //selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_4) ELSE NULL END )) AS TABA_NO, ");
            //selectQuery.Append(" DECODE(CHA.CH_DECISION,0,1,2,3) AS PROCESS_STATUS ");
            //selectQuery.Append(" FROM TRN_CHAMFERING CHA INNER JOIN TRN_PROCESS_PLAN PP ");
            //selectQuery.Append(" ON PP.PROCESS_PLAN_CD = CHA.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            //selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            //selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");

            //selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE  IS NULL ");
            //selectQuery.Append(" AND CHA.UPDATED_DATE IN ( ");
            //selectQuery.Append(" SELECT  MAX(UPDATED_DATE) FROM ( ");
            //selectQuery.Append(" SELECT  CHA.UPDATED_DATE , CONCAT (CHA.CH_TABA_NO_1, CONCAT (CONCAT ( ");
            //selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_2) ELSE NULL END, ");
            //selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_3) ELSE NULL END ), ");
            //selectQuery.Append(" CASE WHEN CHA.CH_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', CHA.CH_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            //selectQuery.Append(" FROM TRN_CHAMFERING CHA INNER JOIN TRN_PROCESS_PLAN PP  ");
            //selectQuery.Append(" ON PP.PROCESS_PLAN_CD = CHA.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");

            //selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            //selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");

            //selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            //selectQuery.Append(" ORDER BY TABA_NO ) group by TABA_NO  ) ");
            //selectQuery.Append(" ORDER BY CHA.CHAMFERING_CD ");
            result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }

        public void InsertChamferDetails(ChamferDetails chamferDetails)
        {
            ChamferDetails result = new ChamferDetails();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_CHAMFERING(");
            insertQuery.Append(" CHAMFERING_CD");
            insertQuery.Append(" ,PROCESS_PLAN_CD ");
            insertQuery.Append(" ,MACHINE_CD");
            insertQuery.Append(" ,CH_START_DATE");
            insertQuery.Append(" ,CH_START_OPERATOR_CD");
            insertQuery.Append(" ,CH_METALLIC_PLATE_CODE");
            insertQuery.Append(" ,CH_SHAPE");
            insertQuery.Append(" ,FURNACE_NO");
            insertQuery.Append(" ,CH_ACT_LENGTH");
            insertQuery.Append(", RECHAMFERING ");
            insertQuery.Append(", CH_TABA_NO_1");
            insertQuery.Append(", CH_TABA_NO_2");
            insertQuery.Append(", CH_TABA_NO_3");
            insertQuery.Append(", CH_TABA_NO_4");
            insertQuery.Append(" ,LOCATION_CD");
            insertQuery.Append(" ,INPUT_LOCATION_CD ");
            insertQuery.Append(" ,USER_ID ");
            insertQuery.Append(",PROCESS_KUBUN_CD");
            insertQuery.Append(" ,CREATED_DATE");
            insertQuery.Append(" ,UPDATED_DATE ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append(" TRN_CHAMFERING_SEQ.NEXTVAL ");
            insertQuery.Append(",'" + chamferDetails.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + chamferDetails.MACHINE_CD + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(chamferDetails.CH_START_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + chamferDetails.CH_START_OPERATOR_CD + "'");
            insertQuery.Append(",'" + chamferDetails.CH_METALLIC_PLATE_CODE + "'");
            insertQuery.Append(",'" + chamferDetails.CH_SHAPE + "'");
            insertQuery.Append(",'" + chamferDetails.FURNACE_NO + "'");
            insertQuery.Append(",'" + chamferDetails.CH_ACT_LENGTH + "'");
            insertQuery.Append(",'" + chamferDetails.RECHAMFERING + "'");
            insertQuery.Append(",'" + chamferDetails.CH_TABA_NO_1 + "'");
            insertQuery.Append(",'" + chamferDetails.CH_TABA_NO_2 + "'");
            insertQuery.Append(",'" + chamferDetails.CH_TABA_NO_3 + "'");
            insertQuery.Append(",'" + chamferDetails.CH_TABA_NO_4 + "'");
            insertQuery.Append(",'" + chamferDetails.LOCATION_CD + "'");
            insertQuery.Append(",'" + chamferDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + chamferDetails.USER_ID + "'");
            insertQuery.Append(",'" + chamferDetails.PROCESS_KUBUN_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            //result = GetChamferDetailsByChuubanGyoubanMachine(chamferDetails.PP_CHUUBAN,Convert.ToInt16( chamferDetails.PP_GYOUBAN), chamferDetails.MACHINE_CD.ToString(), "");

            //return result;
            //result = GetStamppingDetailsByChuuban(stamppingDetails.PP_CHUUBAN, stamppingDetails.PP_GYOUBAN.ToString(), stamppingDetails.MACHINE_CD.ToString());
            //return result;
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
        public List<NameValue> GetEmployeeNameList(string searchText, bool addEmpty)
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
                // selectQuery.Append(" AND ROWNUM < 50 ");          //Show All records 2019-08-26
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
                // query.Append(" AND ROWNUM < 50 ");      //Show All records 2019-08-26
                selectQuery.Append(" ORDER BY LOWER (EMP.YOBIGANA),LOWER (EMP.YOBINA),LOWER (EMP.SHORT_NAME)");
            }
            result = ExecuteQueryMultiResult<NameValue>(selectQuery.ToString());
            return result;
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

        public List<ChamferDetails> GetThabaDetailsByThabaNumber(string chuuban, string gyouban, string Thaba_No_String)
        {
            int decisionVal = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            var result = new List<ChamferDetails>();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT CH.CHAMFERING_CD  ");
            selectQuery.Append(" ,CONCAT (CH.CH_TABA_NO_1, CONCAT (CONCAT ( ");
            selectQuery.Append(" CASE WHEN CH.CH_TABA_NO_2 IS NOT NULL THEN CONCAT (', ', CH.CH_TABA_NO_2) ELSE NULL END, ");
            selectQuery.Append(" CASE WHEN CH.CH_TABA_NO_3 IS NOT NULL THEN CONCAT (', ', CH.CH_TABA_NO_3) ELSE NULL END ), ");
            selectQuery.Append(" CASE WHEN CH.CH_TABA_NO_4 IS NOT NULL THEN CONCAT (', ', CH.CH_TABA_NO_4) ELSE NULL END )) AS TABA_NO ");
            selectQuery.Append(" ,CH.CH_DECISION ");
            selectQuery.Append(" ,CH.RECHAMFERING AS RECHAMFERING");
            selectQuery.Append(" ,CH.CH_TABA_NO_1");
            selectQuery.Append(" ,CH.CH_TABA_NO_2");
            selectQuery.Append(" ,CH.CH_TABA_NO_3");
            selectQuery.Append(" ,CH.CH_TABA_NO_4");

            selectQuery.Append(" FROM TRN_CHAMFERING CH INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = CH.PROCESS_PLAN_CD AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");

            selectQuery.Append(" WHERE PP.CHUUBAN ='" + chuuban + "' ");
            selectQuery.Append(" AND CH.CH_DECISION =" + decisionVal + " ");
            selectQuery.Append(" AND PP.GYOUBAN ='" + gyouban + "' ");
            selectQuery.Append(" AND (CH.CH_TABA_NO_1 IN ('" + Thaba_No_String + "') OR CH.CH_TABA_NO_2 IN ('" + Thaba_No_String + "') OR CH.CH_TABA_NO_3 IN ('" + Thaba_No_String + "') OR CH.CH_TABA_NO_4 IN ('" + Thaba_No_String + "')) ");


            result = ExecuteQueryMultiResult<ChamferDetails>(selectQuery.ToString());
            return result;
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Chamffer process data according to search criteria        
        /// </returns>
        /// 
        public List<ChamferDetails> GetChamferDailyReportOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCiteria,int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<ChamferDetails> result = new List<ChamferDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" LOC.SHORT_NAME AS PROCESS_LOCATION, ");
            selectQuery.Append(" PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD,");
            selectQuery.Append(" MAC.MACHINE_NAME AS CH_MACHINE_NAME, ");
            selectQuery.Append(" TO_CHAR(CHA.CH_START_DATE, 'yyyy/mm/dd HH24:MI') AS CH_START_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(CHA.CH_END_DATE, 'yyyy/mm/dd HH24:MI') AS CH_END_DATE_STRING, ");
            selectQuery.Append(" EMP.SHORT_NAME AS CH_END_OPERATOR_NAME, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" CHA.CH_METALLIC_PLATE_CODE  AS CH_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            //selectQuery.Append(" PP.PROD_LENGTH AS PP_PROD_LENGTH, ");
            selectQuery.Append(" NVL(PP.LENGTH_OTHER, PP.PROD_LENGTH) AS PP_PROD_LENGTH_STRING, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE,  ");
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
            selectQuery.Append(" CHA.CH_FINISHED_BARS AS CH_FINISHED_BARS, ");
            selectQuery.Append(" CHA.UNIT_TYPE , ");
            selectQuery.Append(" CHA.CH_FINISHED_QTY AS CH_FINISHED_QTY, ");
            selectQuery.Append(" MSG.MSG_CONTENTS AS CH_REMARKS, ");
            selectQuery.Append(" CHA.CH_DECISION AS CH_DECISION, ");
            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" CONCAT('C-',CHA.CH_SHAPE) AS CH_SHAPE_STRING,");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME AS NEXT_PROCESS_JP, ");
            selectQuery.Append(" PROCKNEXTPROC.KUBUN_NAME_EN AS NEXT_PROCESS_EN ");
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
            //    query += " ORDER BY CH_END_DATE_STRING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN";
            //}

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result= ExecuteQueryMultiResult<ChamferDetails>(query);            
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Chamffer process data according to search criteria        
        /// </returns>
        /// 
        public List<ChamferDetails> GetChamferDetailsOnDailyProcessAnalysis(List<SearchField> searchCriteria, string sortCiteria, GridSearchFields gridsearchresult)
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
            //selectQuery.Append(" INTERN.INTERNAL_NAME AS PP_INTERNAL_NAME, ");
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
            selectQuery.Append(" PP.COIL_DIA_WINDING AS COIL_DIA_WINDING, ");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PP_PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER, ");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN AS MAGARI_KUBUN, ");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS PP_NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS PP_NEJIRE_SPAN, ");
            selectQuery.Append(" PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN, ");
            selectQuery.Append(" PP.UNIT_WT AS PP_UNIT_WT, ");
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
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN1_MIN_MAX AS TENSILE_STRN_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MIN AS TENSILE_STRN_PATTERN2_MIN, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN2_MAX AS TENSILE_STRN_PATTERN2_MAX, ");
            selectQuery.Append(" PP.TENSILE_STRN_PATTERN3_ACTUAL AS TENSILE_STRN_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN1 AS ELONGATION_PATTERN1, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN1_MIN_MAX AS ELONGATION_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MIN AS ELONGATION_PATTERN2_MIN, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN2_MAX AS ELONGATION_PATTERN2_MAX, ");
            selectQuery.Append(" PP.ELONGATION_PATTERN3_ACTUAL AS ELONGATION_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_CODE AS HARDNESS_PATTERN1_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1 AS HARDNESS_PATTERN1, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN1_MIN_MAX AS HARDNESS_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_CODE AS HARDNESS_PATTERN2_CODE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN3_ACTUAL AS HARDNESS_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.YEILD_PATTERN1 AS YEILD_PATTERN1, ");
            selectQuery.Append(" PP.YEILD_PATTERN1_MIN_MAX AS YEILD_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MIN AS YEILD_PATTERN2_MIN, ");
            selectQuery.Append(" PP.YEILD_PATTERN2_MAX AS YEILD_PATTERN2_MAX, ");
            selectQuery.Append(" PP.YEILD_PATTERN3_ACTUAL AS YEILD_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.PRESS_PATTERN1 AS PRESS_PATTERN1, ");
            selectQuery.Append(" PP.PRESS_PATTERN1_MIN_MAX AS PRESS_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MIN AS PRESS_PATTERN2_MIN, ");
            selectQuery.Append(" PP.PRESS_PATTERN2_MAX AS PRESS_PATTERN2_MAX, ");
            selectQuery.Append(" PP.PRESS_PATTERN3_ACTUAL AS PRESS_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.DECARB_1 AS DECARB_1, ");
            selectQuery.Append(" PP.DECARB_VAL_1 AS DECARB_VAL_1, ");
            selectQuery.Append(" PP.DECARB_2 AS DECARB_2, ");
            selectQuery.Append(" PP.DECARB_VAL_2 AS DECARB_VAL_2, ");
            selectQuery.Append(" PP.DECARB_3 AS DECARB_3, ");
            selectQuery.Append(" PP.DECARB_VAL_3 AS DECARB_VAL_3, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS PP_IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS PP_IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS PP_IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS PP_IMPACT_TEST_PATTERN3_ACTUAL, ");
            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK, ");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");
            //selectQuery.Append(" MATL.MATL_MAKER_NAME AS PP_MATL_MAKER_NAME, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS PP_MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PP_PROCESS_PLAN_KUBUN, ");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.KAKOUIARI_QTY AS PP_KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE, 'yyyy/mm/dd') AS PP_ACT_ALLOC_DATE_STRING, ");
            selectQuery.Append(" TO_CHAR(PP.PROCESS_PLAN_PRINT_DATE, 'yyyy/mm/dd') AS PP_PROCESS_PLAN_PRINT_DATE_STR, ");           
            selectQuery.Append(" MAC.MACHINE_NAME AS CH_MACHINE_NAME, ");
            selectQuery.Append(" LOC.SHORT_NAME AS PROCESS_LOCATION, ");
            selectQuery.Append(" TO_CHAR(CH.CH_START_DATE, 'yyyy/mm/dd HH24:MI') AS CH_START_DATE_STRING, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS CH_START_OPERATOR_NAME, ");
            selectQuery.Append(" CH.CH_METALLIC_PLATE_CODE AS CH_METALLIC_PLATE_CODE, ");
            selectQuery.Append(" CH.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" CH.RECHAMFERING AS RECHAMFERING,");
            selectQuery.Append(" CH.CH_SHAPE AS CH_SHAPE, ");
            selectQuery.Append(" TO_CHAR(CH.CH_END_DATE, 'yyyy/mm/dd HH24:MI') AS CH_END_DATE_STRING, ");
            selectQuery.Append(" EMP2.SHORT_NAME AS CH_END_OPERATOR_NAME, ");
            selectQuery.Append(" CH.CH_ACT_LENGTH AS CH_ACT_LENGTH, ");
            selectQuery.Append(" CH.CH_FINISHED_BARS AS CH_FINISHED_BARS, ");
            selectQuery.Append(" CH.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" CH.CH_FINISHED_QTY AS CH_FINISHED_QTY, ");
            selectQuery.Append(" CH.CH_TABA_NO_1 AS CH_TABA_NO_1, ");
            selectQuery.Append(" CH.CH_TABA_NO_2 AS CH_TABA_NO_2, ");
            selectQuery.Append(" CH.CH_TABA_NO_3 AS CH_TABA_NO_3, ");
            selectQuery.Append(" CH.CH_TABA_NO_4 AS CH_TABA_NO_4, ");
            selectQuery.Append(" CH.CH_DECISION AS CH_DECISION ");
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
            //if (!string.IsNullOrEmpty(sortCriteria))
            //{
            //    query = Helper.ApplySortCriteria(sortCriteria, query);
            //}
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
            return ExecuteQueryMultiResult<ChamferDetails>(query);
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
        public int GetRowCountForChamfferFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT COUNT(*) ");            
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
        public int GetRowCountForChamfferDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");   
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

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }
    }
}
