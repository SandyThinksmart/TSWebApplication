using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using static TSWebApplication.Domain.Utility.Enums;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class HardnessTemperingRepository : BaseRepository, IHardnessTemperingRepository
    {
        private readonly LocalizationServices _localization;
        private readonly IAllCommonRepository _commonRepository;
        public HardnessTemperingRepository(IConfiguration configuration,LocalizationServices localization, IAllCommonRepository commonRepository) : base(configuration)
        {
            _localization = localization;
            _commonRepository = commonRepository;
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
        public int GetFurnaceNumber(string furnaceNo)
        {
            

            int getCount = 0;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT COUNT (HT.FURNACE_NO) ");
            query.Append(" FROM TRN_HARDENING_TEMPERING HT");
            query.Append(" WHERE LOWER(HT.FURNACE_NO) =  '" + furnaceNo.ToLower() + "' ");
          
            getCount = Convert.ToInt32(ExecuteScalar(query.ToString()));
            return getCount;
        }
        public List<HardeningTemperingDetails> GetHardnessTemperingTempDetailsByUser(int userID)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");

            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MIN ");
            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MAX ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE");


            selectQuery.Append(", HT.TEMP_HARDENING_TEMPERING_CD ");
            selectQuery.Append(", HT.LOCATION_CD ");
            selectQuery.Append(", HT.PROCESS_PLAN_CD ");
            selectQuery.Append(", HT.FURNACE_NO ");
            selectQuery.Append(", HT.TABA_NO ");
            selectQuery.Append(", HT.HARDENING_MACHINE_CD ");
            selectQuery.Append(", HT.TEMPERING_MACHINE_CD ");
            selectQuery.Append(", HT.METALLIC_PLATE_CODE ");
            selectQuery.Append(", HT.NO_OF_UNITS ");
            selectQuery.Append(", HT.UNIT_TYPE ");
            selectQuery.Append(", HT.WEIGHT ");
            selectQuery.Append(", HT.WATER_OIL_TYPE ");
            selectQuery.Append(", HT.TEMPERATURE_FURNACE ");
            selectQuery.Append(", HT.CONCENTRATION_FURNACE ");
            selectQuery.Append(", HT.FREQUENCY_FURNACE ");
            selectQuery.Append(", HT.C_FURNACE ");
            selectQuery.Append(", HT.MN_FURNACE ");
            selectQuery.Append(", HT.MO_FURNACE ");
            selectQuery.Append(", HT.CR_FURNACE ");
            selectQuery.Append(", HT.HARDNESS_MIN ");
            selectQuery.Append(", HT.HARDNESS_MAX ");
            selectQuery.Append(", HT.OPERATOR_CD_HARDENING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_HARDENING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_HARDENING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_HARDENING ");
            selectQuery.Append(", HT.TIME_HARDENING ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.OPERATOR_CD_TEMPERING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_TEMPERING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_TEMPERING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_TEMPERING ");
            selectQuery.Append(", HT.TIME_TEMPERING ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.HT_NEXT_PROCESS ");
            selectQuery.Append(", HT.UPPER_DETAILS ");
            selectQuery.Append(", HT.LOWER_DETAILS ");
            selectQuery.Append(", T_MSG_CONTENTS AS REMARK");
            selectQuery.Append(", MCH1.MACHINE_NAME AS HARDENING_MACHINE_NAME ");
            selectQuery.Append(", MCH2.MACHINE_NAME AS TEMPERING_MACHINE_NAME ");
            selectQuery.Append(", EMP1.SHORT_NAME AS HARDENING_OPERATOR_NAME ");
            selectQuery.Append(", EMP2.SHORT_NAME AS TEMPERING_OPERATOR_NAME ");

            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER  JOIN TRN_HARDENING_TEMPERING_TEMP HT ON");
            selectQuery.Append(" HT.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MCH1 ON MCH1.MACHINE_CD=HT.HARDENING_MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MCH2 ON MCH2.MACHINE_CD=HT.TEMPERING_MACHINE_CD");

            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD=HT.OPERATOR_CD_HARDENING");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=HT.OPERATOR_CD_TEMPERING");

            selectQuery.Append(" WHERE HT.USER_ID = " + userID + "");

            selectQuery.Append(" AND HT.CREATED_DATE = (SELECT MAX(CREATED_DATE) FROM  TRN_HARDENING_TEMPERING_TEMP WHERE USER_ID = " + userID + " )");

            // selectQuery.Append(" ORDER BY HT.TEMP_HARDENING_TEMPERING_CD DESC ");

            return ExecuteQueryMultiResult<HardeningTemperingDetails>(selectQuery.ToString());
        }

        
        public HardeningTemperingDetails GetHTQualityDetailsByCD(long HTQualityCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");

            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MIN ");
            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MAX ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE");


            selectQuery.Append(", HT.HARDENING_TEMPERING_CD ");
            selectQuery.Append(", HT.LOCATION_CD ");
            selectQuery.Append(", HT.MSG_CD ");
            selectQuery.Append(", HT.PROCESS_PLAN_CD ");
            selectQuery.Append(", HT.FURNACE_NO ");
            selectQuery.Append(", HT.HARDENING_MACHINE_CD ");
            selectQuery.Append(", HT.TEMPERING_MACHINE_CD ");
            selectQuery.Append(", HT.METALLIC_PLATE_CODE ");
            selectQuery.Append(", HT.NO_OF_UNITS ");
            selectQuery.Append(", HT.UNIT_TYPE ");
            selectQuery.Append(", HT.WEIGHT ");
            selectQuery.Append(", HT.WATER_OIL_TYPE ");
            selectQuery.Append(", HT.TEMPERATURE_FURNACE ");
            selectQuery.Append(", HT.CONCENTRATION_FURNACE ");
            selectQuery.Append(", HT.FREQUENCY_FURNACE ");
            selectQuery.Append(", HT.C_FURNACE ");
            selectQuery.Append(", HT.MN_FURNACE ");
            selectQuery.Append(", HT.MO_FURNACE ");
            selectQuery.Append(", HT.CR_FURNACE ");
            selectQuery.Append(", HT.HARDNESS_MIN ");
            selectQuery.Append(", HT.HARDNESS_MAX ");
            selectQuery.Append(", HT.OPERATOR_CD_HARDENING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_HARDENING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_HARDENING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_HARDENING ");
            selectQuery.Append(", HT.TIME_HARDENING ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.OPERATOR_CD_TEMPERING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_TEMPERING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_TEMPERING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_TEMPERING ");
            selectQuery.Append(", HT.TIME_TEMPERING ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_CEN ");

            selectQuery.Append(", HTQ. HT_QUALITY_CD"); 
            selectQuery.Append(", HTQ. MSG_CD AS HT_MSG_CD");
            selectQuery.Append(", HTQ. INSPECTION_DATE");
            selectQuery.Append(", HTQ. INSPECTED_BY_CD");
            selectQuery.Append(", EMP. SHORT_NAME AS INSPECTED_OPERATOR_NAME");
            selectQuery.Append(", EMP. IN_ACTIVE_FLAG");
            selectQuery.Append(", HTQ. DM_F_MIN");
            selectQuery.Append(", HTQ. DM_F_MAX");
            selectQuery.Append(", HTQ. DM_T_MIN");
            selectQuery.Append(", HTQ. DM_T_MAX");
            selectQuery.Append(", HTQ. STRUCTURE_OUT");
            selectQuery.Append(", HTQ. STRUCTURE_IN");
            selectQuery.Append(", HTQ. DECISION");
            selectQuery.Append(", HTQ. HTQ_NEXT_PROCESS");

            selectQuery.Append(", MSG.MSG_CONTENTS AS REMARK");
            selectQuery.Append(", MSG1.MSG_CONTENTS AS HT_REMARK");
            selectQuery.Append(", MCH1.MACHINE_NAME AS HARDENING_MACHINE_NAME ");
            selectQuery.Append(", MCH2.MACHINE_NAME AS TEMPERING_MACHINE_NAME ");
            selectQuery.Append(", EMP1.SHORT_NAME AS HARDENING_OPERATOR_NAME ");
            selectQuery.Append(", EMP2.SHORT_NAME AS TEMPERING_OPERATOR_NAME ");

            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT  JOIN TRN_HARDENING_TEMPERING HT ON");
            selectQuery.Append(" HT.PROCESS_PLAN_CD=HTQ.PROCESS_PLAN_CD AND HT.FURNACE_NO = HTQ.FURNACE_NO ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON HT.MSG_CD =MSG.MSG_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG1 ON HTQ.MSG_CD =MSG1.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MCH1 ON MCH1.MACHINE_CD=HT.HARDENING_MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MCH2 ON MCH2.MACHINE_CD=HT.TEMPERING_MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON EMP.EMPLOYEE_CD=HTQ.INSPECTED_BY_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD=HT.OPERATOR_CD_HARDENING");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=HT.OPERATOR_CD_TEMPERING");
            selectQuery.Append("  WHERE HTQ.HT_QUALITY_CD= " + HTQualityCD); 

            return ExecuteQuerySingleResult<HardeningTemperingDetails>(selectQuery.ToString());
        }


        public HardeningTemperingDetails GetHardnessTemperingDetailsByChuubanGyoban(string chuuban, short? gyouban)
            {
          
            //checkfurnace = Convert.ToInt32(ExecuteScalar("SELECT COUNT(*) FROM TRN_MI_HARDNESS WHERE LOWER(HD_FURNACE_NO) =  '" + furnaceNo.ToLower() + "'"));

            //checkchuubangyouban = Convert.ToInt32(ExecuteScalar("SELECT COUNT(HRD.PROCESS_PLAN_CD) FROM TRN_PROCESS_PLAN PP INNER JOIN TRN_MI_HARDNESS HRD ON PP.PROCESS_PLAN_CD = HRD.PROCESS_PLAN_CD  WHERE PP.CHUUBAN = '" + chuuban + "' AND PP.GYOUBAN = '" + gyouban + "'"));
            //checkchuubangyouban = Convert.ToInt32(ExecuteScalar("SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_PROCESS_PLAN WHERE CHUUBAN = '" + chuuban + "' AND GYOUBAN = '" + gyouban + "'"));
            
                

                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append(" SELECT ");
                selectQuery.Append("  PP.PRODUCT_CD ");
                selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
                selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN ");
                selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
                selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
                selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
                selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
                selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
                selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
                selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
                selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");

                selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MIN ");
                selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MAX ");
                selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE");
                selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE");


                selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
              
                selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");

                selectQuery.Append(" WHERE PP.CHUUBAN = '" + chuuban + "'");
                selectQuery.Append(" AND PP.GYOUBAN = '" + gyouban + "'");
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL");
            return ExecuteQuerySingleResult<HardeningTemperingDetails>(selectQuery.ToString());
            
               
            
        }

        public HardeningTemperingDetails GetHardnessTemperingDetails(string furnaceNo, string chuuban, short? gyouban, int inputLocationCD)
        {
            int getRowsCount = 0;
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT (PP.PROCESS_PLAN_CD) ");
            selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER  JOIN TRN_HARDENING_TEMPERING HT ");
            selectQuery.Append(" ON HT.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD  ");
            selectQuery.Append(" WHERE PP.CHUUBAN = '" + chuuban + "' ");
            selectQuery.Append(" AND PP.GYOUBAN = '" + gyouban + "' ");
            if (!string.IsNullOrEmpty(furnaceNo))
            {
                selectQuery.Append(" AND LOWER(HT.FURNACE_NO) = '" + furnaceNo.ToLower() + "'");
            }
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            getRowsCount =Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));



            selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN");
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            //selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            //selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");

            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MIN ");
            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MAX ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE");
            selectQuery.Append(" ,PP.PROCESS_PLAN_START_DATE");
            
            selectQuery.Append(", HT.HARDENING_TEMPERING_CD ");
            selectQuery.Append(", HT.LOCATION_CD ");
            selectQuery.Append(", HT.MSG_CD ");
            selectQuery.Append(", HT.PROCESS_PLAN_CD ");
            selectQuery.Append(", HT.FURNACE_NO ");
            selectQuery.Append(", HT.TABA_NO ");
            selectQuery.Append(", HT.HARDENING_MACHINE_CD ");
            selectQuery.Append(", HT.TEMPERING_MACHINE_CD ");
            selectQuery.Append(", HT.METALLIC_PLATE_CODE ");
            selectQuery.Append(", HT.NO_OF_UNITS ");
            selectQuery.Append(", HT.UNIT_TYPE ");
            selectQuery.Append(", HT.WEIGHT ");
            selectQuery.Append(", HT.WATER_OIL_TYPE ");
            selectQuery.Append(", HT.TEMPERATURE_FURNACE ");
            selectQuery.Append(", HT.CONCENTRATION_FURNACE ");
            selectQuery.Append(", HT.FREQUENCY_FURNACE ");
            selectQuery.Append(", HT.C_FURNACE ");
            selectQuery.Append(", HT.MN_FURNACE ");
            selectQuery.Append(", HT.MO_FURNACE ");
            selectQuery.Append(", HT.CR_FURNACE ");
            selectQuery.Append(", HT.HARDNESS_MIN ");
            selectQuery.Append(", HT.HARDNESS_MAX ");
            selectQuery.Append(", HT.OPERATOR_CD_HARDENING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_HARDENING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_HARDENING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_HARDENING ");
            selectQuery.Append(", HT.TIME_HARDENING ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.OPERATOR_CD_TEMPERING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_TEMPERING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_TEMPERING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_TEMPERING ");
            selectQuery.Append(", HT.TIME_TEMPERING ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.HT_NEXT_PROCESS ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS REMARK");
            selectQuery.Append(", MCH1.MACHINE_NAME AS HARDENING_MACHINE_NAME ");
            selectQuery.Append(", MCH2.MACHINE_NAME AS TEMPERING_MACHINE_NAME ");
            selectQuery.Append(", EMP1.SHORT_NAME AS HARDENING_OPERATOR_NAME ");
            selectQuery.Append(", EMP2.SHORT_NAME AS TEMPERING_OPERATOR_NAME ");

            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER  JOIN TRN_HARDENING_TEMPERING HT ON");
            selectQuery.Append(" HT.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON HT.MSG_CD =MSG.MSG_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MCH1 ON MCH1.MACHINE_CD=HT.HARDENING_MACHINE_CD");
            selectQuery.Append(" LEFT JOIN MST_MACHINE MCH2 ON MCH2.MACHINE_CD=HT.TEMPERING_MACHINE_CD");

            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP1 ON EMP1.EMPLOYEE_CD=HT.OPERATOR_CD_HARDENING");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP2 ON EMP2.EMPLOYEE_CD=HT.OPERATOR_CD_TEMPERING");

            selectQuery.Append(" WHERE PP.CHUUBAN = '" + chuuban + "'");
            selectQuery.Append(" AND PP.GYOUBAN = '" + gyouban + "'");
            if (getRowsCount > 0)
            {
                selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            }
            if (!string.IsNullOrEmpty(furnaceNo))
            {
                selectQuery.Append(" AND LOWER(HT.FURNACE_NO) = '" + furnaceNo.ToLower() + "'");
            }
            if (inputLocationCD > 0)
            {
                selectQuery.Append(" AND HT.INPUT_LOCATION_CD = '" + inputLocationCD + "'");
            }
            selectQuery.Append(" ORDER BY HARDENING_TEMPERING_CD DESC ");

            return ExecuteQuerySingleResult<HardeningTemperingDetails>(selectQuery.ToString());
        }

        public void InsertHardnessTemperingDetails(HardeningTemperingDetails details)
        {
          
            
            StringBuilder deleteQuery = new StringBuilder();

            //deleteQuery.Append("DELETE FROM TRN_HARDENING_TEMPERING_TEMP WHERE LOWER(FURNACE_NO)  = '" + details.FURNACE_NO.ToLower() + "' AND PROCESS_PLAN_CD=" + details.PROCESS_PLAN_CD);
            //ExecuteNonQuery(deleteQuery.ToString());

            deleteQuery.Append(" DELETE ");
            deleteQuery.Append(" FROM ");
            deleteQuery.Append(" TRN_HARDENING_TEMPERING_TEMP ");
            deleteQuery.Append(" WHERE ");
            if (Convert.ToInt16(details.UPPER_DETAILS) > 0)
            {
                deleteQuery.Append(" UPPER_DETAILS = " + details.UPPER_DETAILS);
            }else
            {
                deleteQuery.Append(" LOWER_DETAILS = " + details.LOWER_DETAILS);
            }
            deleteQuery.Append(" AND ");
            deleteQuery.Append(" USER_ID = " + details.USER_ID);
            ExecuteNonQuery(deleteQuery.ToString());

            StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append(" INSERT INTO TRN_HARDENING_TEMPERING(");
                insertQuery.Append(" HARDENING_TEMPERING_CD");
                insertQuery.Append(" ,LOCATION_CD");
                insertQuery.Append(" ,MSG_CD");
                insertQuery.Append(" ,PROCESS_PLAN_CD");
                insertQuery.Append(" ,FURNACE_NO");
                insertQuery.Append(" ,TABA_NO");
                insertQuery.Append(" ,HARDENING_MACHINE_CD");
                insertQuery.Append(" ,TEMPERING_MACHINE_CD");
                insertQuery.Append(" ,METALLIC_PLATE_CODE");
                insertQuery.Append(" ,NO_OF_UNITS");
                insertQuery.Append(" ,UNIT_TYPE");
                insertQuery.Append(" ,WEIGHT");
                insertQuery.Append(" ,WATER_OIL_TYPE");
                insertQuery.Append(" ,TEMPERATURE_FURNACE");
                insertQuery.Append(" ,CONCENTRATION_FURNACE");
                insertQuery.Append(" ,FREQUENCY_FURNACE");
                insertQuery.Append(" ,C_FURNACE");
                insertQuery.Append(" ,MN_FURNACE");
                insertQuery.Append(" ,MO_FURNACE");
                insertQuery.Append(" ,CR_FURNACE");
                insertQuery.Append(" ,HARDNESS_MIN");
                insertQuery.Append(" ,HARDNESS_MAX");
                insertQuery.Append(" ,OPERATOR_CD_HARDENING");
                insertQuery.Append(" ,START_DATE_HARDENING");

                insertQuery.Append(" ,END_DATE_HARDENING");
                insertQuery.Append(" ,TEMPERATURE_HARDENING");
                insertQuery.Append(" ,TIME_HARDENING");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_1");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_2");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_3");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_CEN");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_1");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_2");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_3");
                insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_CEN");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_1");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_2");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_3");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_CEN");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_1");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_2");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_3");
                insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_CEN");
                insertQuery.Append(" ,OPERATOR_CD_TEMPERING");
                insertQuery.Append(" ,START_DATE_TEMPERING");
                insertQuery.Append(" ,END_DATE_TEMPERING");
                insertQuery.Append(" ,TEMPERATURE_TEMPERING");
                insertQuery.Append(" ,TIME_TEMPERING");
                insertQuery.Append(" ,T_HARDNESS_NORTH_UP_1");
                insertQuery.Append(" ,T_HARDNESS_NORTH_UP_2");
                insertQuery.Append(" ,T_HARDNESS_NORTH_UP_3");
                insertQuery.Append(" ,T_HARDNESS_NORTH_UP_CENT");
                insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_1");
                insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_2");
                insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_3");
                insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_CEN");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_1");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_2");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_3");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_CENT");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_1");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_2");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_3");
                insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_CEN");
                insertQuery.Append(" ,HT_NEXT_PROCESS");
                insertQuery.Append(" ,PROCESS_KUBUN_CD");
                insertQuery.Append(" ,USER_ID");
                insertQuery.Append(" ,INPUT_LOCATION_CD");
                insertQuery.Append(" ,CREATED_DATE");
                insertQuery.Append(" ,UPDATED_DATE");
                insertQuery.Append(" ) ");
                insertQuery.Append(" VALUES(");
                insertQuery.Append(" TRN_HARDENING_TEMPERING_SEQ.NEXTVAL ");
                insertQuery.Append(",'" + details.LOCATION_CD + "'");
                if (details.MSG_CD > 0)
                {
                    insertQuery.Append(",'" + details.MSG_CD + "'");
                }
                else
                {
                    insertQuery.Append("," + "NULL");
                }
                insertQuery.Append(",'" + details.PROCESS_PLAN_CD + "'");
                insertQuery.Append(",'" + details.FURNACE_NO.ToUpper() + "'");
                insertQuery.Append(",'" + details.TABA_NO + "'");
                insertQuery.Append(",'" + details.HARDENING_MACHINE_CD + "'");
                insertQuery.Append(",'" + details.TEMPERING_MACHINE_CD + "'");
                insertQuery.Append(",'" + details.METALLIC_PLATE_CODE + "'");
                insertQuery.Append(",'" + details.NO_OF_UNITS + "'");
                insertQuery.Append(",'" + details.UNIT_TYPE + "'");
                insertQuery.Append(",'" + details.WEIGHT + "'");
                insertQuery.Append(",'" + details.WATER_OIL_TYPE + "'");
                insertQuery.Append(",'" + details.TEMPERATURE_FURNACE + "'");
                insertQuery.Append(",'" + details.CONCENTRATION_FURNACE + "'");
                insertQuery.Append(",'" + details.FREQUENCY_FURNACE + "'");
                insertQuery.Append(",'" + details.C_FURNACE + "'");
                insertQuery.Append(",'" + details.MN_FURNACE + "'");
                insertQuery.Append(",'" + details.MO_FURNACE + "'");
                insertQuery.Append(",'" + details.CR_FURNACE + "'");

                insertQuery.Append(",'" + details.HARDNESS_MIN + "'");
                insertQuery.Append(",'" + details.HARDNESS_MAX + "'");
                insertQuery.Append(",'" + details.OPERATOR_CD_HARDENING + "'");
                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.START_DATE_HARDENING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");

                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.END_DATE_HARDENING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                insertQuery.Append(",'" + details.TEMPERATURE_HARDENING + "'");
                insertQuery.Append(",'" + details.TIME_HARDENING + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_1 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_2 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_3 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_CEN + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_1 + "'");     //
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_2 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_3 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_CEN + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_1 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_2 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_3 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_CEN + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_1 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_2 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_3 + "'");
                insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_CEN + "'");
                insertQuery.Append(",'" + details.OPERATOR_CD_TEMPERING + "'");
                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.START_DATE_TEMPERING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");

                insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.END_DATE_TEMPERING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                insertQuery.Append(",'" + details.TEMPERATURE_TEMPERING + "'");
                insertQuery.Append(",'" + details.TIME_TEMPERING + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_1 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_2 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_3 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_CENT + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_1 + "'");     //
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_2 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_3 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_CEN + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_1 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_2 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_3 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_CENT + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_1 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_2 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_3 + "'");
                insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_CEN + "'");
                insertQuery.Append(",'" + details.HT_NEXT_PROCESS + "'");
                insertQuery.Append(",'" + details.PROCESS_KUBUN_ID + "'");
                insertQuery.Append(",'" + details.USER_ID + "'");
                insertQuery.Append(",'" + details.INPUT_LOCATION_CD + "'");
                insertQuery.Append(",SYSDATE");
                insertQuery.Append(",SYSDATE ");
                insertQuery.Append(") ");
                ExecuteNonQuery(insertQuery.ToString());

        }

        public void InsertHTQualityDetails(HardeningTemperingDetails details)
        {

            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_HT_QUALITY(");
            insertQuery.Append(" HT_QUALITY_CD ");
            insertQuery.Append(" ,MSG_CD ");
            insertQuery.Append(" ,PROCESS_PLAN_CD ");
            insertQuery.Append(" ,FURNACE_NO ");
            insertQuery.Append(" ,INSPECTION_DATE ");
            insertQuery.Append(" ,INSPECTED_BY_CD ");
            insertQuery.Append(" ,LOCATION_CD "); 
            insertQuery.Append(" ,DM_F_MIN ");
            insertQuery.Append(" ,DM_F_MAX ");
            insertQuery.Append(" ,DM_T_MIN ");
            insertQuery.Append(" ,DM_T_MAX ");
            insertQuery.Append(" ,STRUCTURE_OUT ");
            insertQuery.Append(" ,STRUCTURE_IN ");
            insertQuery.Append(" ,DECISION ");
            insertQuery.Append(" ,HTQ_NEXT_PROCESS ");
            insertQuery.Append(" ,USER_ID ");
            insertQuery.Append(" ,INPUT_LOCATION_CD ");
            insertQuery.Append(" ,PROCESS_KUBUN_CD "); 
            insertQuery.Append(" ,CREATED_DATE ");
            insertQuery.Append(" ,UPDATED_DATE ");

            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append(" TRN_HT_QUALITY_SEQ.NEXTVAL ");
            insertQuery.Append(",'" + details.MSG_CD + "'");
            insertQuery.Append(",'" + details.PROCESS_PLAN_CD + "'");
            insertQuery.Append(",'" + details.FURNACE_NO.ToUpper() + "'");
            insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.INSPECTION_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            insertQuery.Append(",'" + details.INSPECTED_BY_CD + "'");
            insertQuery.Append(",'" + details.LOCATION_CD + "'");
            insertQuery.Append(",'" + details.DM_F_MIN + "'");
            insertQuery.Append(",'" + details.DM_F_MAX + "'");
            insertQuery.Append(",'" + details.DM_T_MIN + "'");
            insertQuery.Append(",'" + details.DM_T_MAX + "'");
            insertQuery.Append(",'" + details.STRUCTURE_OUT + "'");
            insertQuery.Append(",'" + details.STRUCTURE_IN + "'");
            insertQuery.Append(",'" + details.DECISION + "'");
            insertQuery.Append(",'" + details.HTQ_NEXT_PROCESS + "'");
            insertQuery.Append(",'" + details.USER_ID + "'");
            insertQuery.Append(",'" + details.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",'" + details.PROCESS_KUBUN_ID + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());


        }

        public void InsertHardnessTemperingTempDetails(HardeningTemperingDetails details)
        {
            if (details.TEMP_HARDENING_TEMPERING_CD > 0)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(" DELETE FROM TRN_HARDENING_TEMPERING_TEMP WHERE TEMP_HARDENING_TEMPERING_CD = " + details.TEMP_HARDENING_TEMPERING_CD);
                ExecuteNonQuery(deleteQuery.ToString());
            }


            StringBuilder insertQuery = new StringBuilder();
                    insertQuery.Append(" INSERT INTO TRN_HARDENING_TEMPERING_TEMP(");
                    insertQuery.Append(" TEMP_HARDENING_TEMPERING_CD");
                    insertQuery.Append(" ,LOCATION_CD");
                    insertQuery.Append(" ,PROCESS_PLAN_CD");
                    insertQuery.Append(" ,FURNACE_NO");
                    insertQuery.Append(" ,TABA_NO");
                    insertQuery.Append(" ,HARDENING_MACHINE_CD");
                    insertQuery.Append(" ,TEMPERING_MACHINE_CD");
                    insertQuery.Append(" ,METALLIC_PLATE_CODE");
                    insertQuery.Append(" ,NO_OF_UNITS");
                    insertQuery.Append(" ,UNIT_TYPE");
                    insertQuery.Append(" ,WEIGHT");
                    insertQuery.Append(" ,WATER_OIL_TYPE");
                    insertQuery.Append(" ,TEMPERATURE_FURNACE");
                    insertQuery.Append(" ,CONCENTRATION_FURNACE");
                    insertQuery.Append(" ,FREQUENCY_FURNACE");
                    insertQuery.Append(" ,C_FURNACE");
                    insertQuery.Append(" ,MN_FURNACE");
                    insertQuery.Append(" ,MO_FURNACE");
                    insertQuery.Append(" ,CR_FURNACE");
                    insertQuery.Append(" ,HARDNESS_MIN");
                    insertQuery.Append(" ,HARDNESS_MAX");
                    insertQuery.Append(" ,OPERATOR_CD_HARDENING");
                    insertQuery.Append(" ,START_DATE_HARDENING");

                    insertQuery.Append(" ,END_DATE_HARDENING");
                    insertQuery.Append(" ,TEMPERATURE_HARDENING");
                    insertQuery.Append(" ,TIME_HARDENING");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_1");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_2");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_3");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_UP_CEN");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_1");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_2");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_3");
                    insertQuery.Append(" ,Q_HARDNESS_NORTH_DOWN_CEN");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_1");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_2");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_3");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_UP_CEN");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_1");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_2");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_3");
                    insertQuery.Append(" ,Q_HARDNESS_SOUTH_DOWN_CEN");
                    insertQuery.Append(" ,OPERATOR_CD_TEMPERING");
                    insertQuery.Append(" ,START_DATE_TEMPERING");
                    insertQuery.Append(" ,END_DATE_TEMPERING");
                    insertQuery.Append(" ,TEMPERATURE_TEMPERING");
                    insertQuery.Append(" ,TIME_TEMPERING");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_UP_1");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_UP_2");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_UP_3");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_UP_CENT");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_1");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_2");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_3");
                    insertQuery.Append(" ,T_HARDNESS_NORTH_DOWN_CEN");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_1");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_2");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_3");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_UP_CENT");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_1");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_2");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_3");
                    insertQuery.Append(" ,T_HARDNESS_SOUTH_DOWN_CEN");
                    insertQuery.Append(" ,HT_NEXT_PROCESS");
                    insertQuery.Append(" ,PROCESS_KUBUN_CD");
                    insertQuery.Append(" ,UPPER_DETAILS");
                    insertQuery.Append(" ,LOWER_DETAILS");
                    insertQuery.Append(" ,T_MSG_CONTENTS");
                    insertQuery.Append(" ,USER_ID");
                    insertQuery.Append(" ,INPUT_LOCATION_CD");
                    insertQuery.Append(" ,CREATED_DATE");
                    insertQuery.Append(" ,UPDATED_DATE");
                    insertQuery.Append(" ) ");
                    insertQuery.Append(" VALUES(");
                    insertQuery.Append(" TRN_HRD_TEMPERING_TEMP_SEQ.NEXTVAL ");
                    insertQuery.Append(",'" + details.LOCATION_CD + "'");
                    insertQuery.Append(",'" + details.PROCESS_PLAN_CD + "'");
                    if (!string.IsNullOrEmpty(details.FURNACE_NO))
                    {
                        insertQuery.Append(",'" + details.FURNACE_NO.ToUpper() + "'");
                    }
                    else
                    {
                        insertQuery.Append(",'" + details.FURNACE_NO + "'");
                    }
                
                    insertQuery.Append(",'" + details.TABA_NO + "'");
                    insertQuery.Append(",'" + details.HARDENING_MACHINE_CD + "'");
                    insertQuery.Append(",'" + details.TEMPERING_MACHINE_CD + "'");
                    insertQuery.Append(",'" + details.METALLIC_PLATE_CODE + "'");
                    insertQuery.Append(",'" + details.NO_OF_UNITS + "'");
                    insertQuery.Append(",'" + details.UNIT_TYPE + "'");
                    insertQuery.Append(",'" + details.WEIGHT + "'");
                    insertQuery.Append(",'" + details.WATER_OIL_TYPE + "'");
                    insertQuery.Append(",'" + details.TEMPERATURE_FURNACE + "'");
                    insertQuery.Append(",'" + details.CONCENTRATION_FURNACE + "'");
                    insertQuery.Append(",'" + details.FREQUENCY_FURNACE + "'");
                    insertQuery.Append(",'" + details.C_FURNACE + "'");
                    insertQuery.Append(",'" + details.MN_FURNACE + "'");
                    insertQuery.Append(",'" + details.MO_FURNACE + "'");
                    insertQuery.Append(",'" + details.CR_FURNACE + "'");

                    insertQuery.Append(",'" + details.HARDNESS_MIN + "'");
                    insertQuery.Append(",'" + details.HARDNESS_MAX + "'");
                    insertQuery.Append(",'" + details.OPERATOR_CD_HARDENING + "'");
                    insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.START_DATE_HARDENING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");

                    insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.END_DATE_HARDENING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                    insertQuery.Append(",'" + details.TEMPERATURE_HARDENING + "'");
                    insertQuery.Append(",'" + details.TIME_HARDENING + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_1 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_2 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_3 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_UP_CEN + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_1 + "'");     //
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_2 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_3 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_NORTH_DOWN_CEN + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_1 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_2 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_3 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_UP_CEN + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_1 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_2 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_3 + "'");
                    insertQuery.Append(",'" + details.Q_HARDNESS_SOUTH_DOWN_CEN + "'");
                    insertQuery.Append(",'" + details.OPERATOR_CD_TEMPERING + "'");
                    insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.START_DATE_TEMPERING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");

                    insertQuery.Append(", TO_DATE('" + Convert.ToDateTime(details.END_DATE_TEMPERING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
                    insertQuery.Append(",'" + details.TEMPERATURE_TEMPERING + "'");
                    insertQuery.Append(",'" + details.TIME_TEMPERING + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_1 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_2 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_3 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_UP_CENT + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_1 + "'");     //
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_2 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_3 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_NORTH_DOWN_CEN + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_1 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_2 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_3 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_UP_CENT + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_1 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_2 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_3 + "'");
                    insertQuery.Append(",'" + details.T_HARDNESS_SOUTH_DOWN_CEN + "'");
                    insertQuery.Append(",'" + details.HT_NEXT_PROCESS + "'");
                    insertQuery.Append(",'" + details.PROCESS_KUBUN_ID + "'");
                    insertQuery.Append(",'" + details.UPPER_DETAILS + "'");
                    insertQuery.Append(",'" + details.LOWER_DETAILS + "'");
                    insertQuery.Append(",'" + details.REMARK + "'");
                    insertQuery.Append(",'" + details.USER_ID + "'");
                    insertQuery.Append(",'" + details.INPUT_LOCATION_CD + "'");
                    insertQuery.Append(", SYSDATE ");
                    insertQuery.Append(", SYSDATE ");
                    insertQuery.Append(") ");
                    ExecuteNonQuery(insertQuery.ToString());
                  
        }

        public long UpdateHardnessTemperingDetails(HardeningTemperingDetails details)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("  UPDATE TRN_HARDENING_TEMPERING SET ");
            updateQuery.Append("  LOCATION_CD =" + "'" + details.LOCATION_CD+ "'");
            updateQuery.Append("  ,MSG_CD =" + "'" + details.MSG_CD + "'");
            updateQuery.Append("  ,PROCESS_PLAN_CD =" + "'" + details.PROCESS_PLAN_CD + "'" );
            updateQuery.Append("  ,FURNACE_NO =" + "'" + details.FURNACE_NO.ToUpper() + "'");
            updateQuery.Append("  ,TABA_NO =" + "'" + details.TABA_NO + "'");
            updateQuery.Append("  ,HARDENING_MACHINE_CD =" + "'" + details.HARDENING_MACHINE_CD + "'");
            updateQuery.Append("  ,TEMPERING_MACHINE_CD =" + "'" + details.TEMPERING_MACHINE_CD + "'");
            updateQuery.Append("  ,METALLIC_PLATE_CODE =" +"'" + details.METALLIC_PLATE_CODE + "'");
            updateQuery.Append("  ,NO_OF_UNITS =" + "'" + details.NO_OF_UNITS + "'");
            updateQuery.Append("  ,UNIT_TYPE =" + "'" + details.UNIT_TYPE + "'");
            updateQuery.Append("  ,WEIGHT =" + "'" + details.WEIGHT + "'");
            updateQuery.Append("  ,WATER_OIL_TYPE =" +  "'" + details.WATER_OIL_TYPE + "'");
            updateQuery.Append("  ,TEMPERATURE_FURNACE =" + "'" + details.TEMPERATURE_FURNACE + "'");
            updateQuery.Append("  ,CONCENTRATION_FURNACE =" +"'" + details.CONCENTRATION_FURNACE + "'");
            updateQuery.Append("  ,FREQUENCY_FURNACE =" + "'" + details.FREQUENCY_FURNACE + "'");
            updateQuery.Append("  ,C_FURNACE =" + "'" + details.C_FURNACE + "'");
            updateQuery.Append("  ,MN_FURNACE =" + "'" + details.MN_FURNACE + "'");
            updateQuery.Append("  ,MO_FURNACE =" + "'" + details.MO_FURNACE + "'");
            updateQuery.Append("  ,CR_FURNACE =" + "'" + details.CR_FURNACE + "'");
            updateQuery.Append("  ,HARDNESS_MIN =" + "'" + details.HARDNESS_MIN + "'");
            updateQuery.Append("  ,HARDNESS_MAX =" + "'" + details.HARDNESS_MAX + "'");
            updateQuery.Append("  ,OPERATOR_CD_HARDENING =" + "'" + details.OPERATOR_CD_HARDENING.ToString().Trim() + "'");
            updateQuery.Append("  ,START_DATE_HARDENING =" + "TO_DATE('" + Convert.ToDateTime(details.START_DATE_HARDENING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append("  ,END_DATE_HARDENING =" + "TO_DATE('" + Convert.ToDateTime(details.END_DATE_HARDENING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append("  ,TEMPERATURE_HARDENING =" + "'" + details.TEMPERATURE_HARDENING + "'");
            updateQuery.Append("  ,TIME_HARDENING =" + "'" + details.TIME_HARDENING.ToString().Trim() + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_UP_1 =" + "'" + details.Q_HARDNESS_NORTH_UP_1 + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_UP_2 =" + "'" + details.Q_HARDNESS_NORTH_UP_2 + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_UP_3 =" + "'" + details.Q_HARDNESS_NORTH_UP_3 + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_UP_CEN =" + "'" + details.Q_HARDNESS_NORTH_UP_CEN + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_DOWN_1 =" + "'" + details.Q_HARDNESS_NORTH_DOWN_1 + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_DOWN_2 =" + "'" + details.Q_HARDNESS_NORTH_DOWN_2 + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_DOWN_3 =" + "'" + details.Q_HARDNESS_NORTH_DOWN_3 + "'");
            updateQuery.Append("  ,Q_HARDNESS_NORTH_DOWN_CEN =" + "'" + details.Q_HARDNESS_NORTH_DOWN_CEN + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_UP_1 =" + "'" + details.Q_HARDNESS_SOUTH_UP_1 + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_UP_2 =" + "'" + details.Q_HARDNESS_SOUTH_UP_2 + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_UP_3 =" + "'" + details.Q_HARDNESS_SOUTH_UP_3 + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_UP_CEN =" + "'" + details.Q_HARDNESS_SOUTH_UP_CEN + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_DOWN_1 =" + "'" + details.Q_HARDNESS_SOUTH_DOWN_1 + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_DOWN_2 =" + "'" + details.Q_HARDNESS_SOUTH_DOWN_2 + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_DOWN_3 =" + "'" + details.Q_HARDNESS_SOUTH_DOWN_3 + "'");
            updateQuery.Append("  ,Q_HARDNESS_SOUTH_DOWN_CEN =" + "'" + details.Q_HARDNESS_SOUTH_DOWN_CEN + "'");
            updateQuery.Append("  ,OPERATOR_CD_TEMPERING =" + "'" + details.OPERATOR_CD_TEMPERING + "'");
            updateQuery.Append("  ,START_DATE_TEMPERING =" + "TO_DATE('" + Convert.ToDateTime(details.START_DATE_TEMPERING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append("  ,END_DATE_TEMPERING =" + "TO_DATE('" + Convert.ToDateTime(details.END_DATE_TEMPERING).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");
            updateQuery.Append("  ,TEMPERATURE_TEMPERING =" + "'" + details.TEMPERATURE_TEMPERING + "'");
            updateQuery.Append("  ,TIME_TEMPERING =" + "'" + details.TIME_TEMPERING.ToString().Trim() + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_UP_1 =" + "'" + details.T_HARDNESS_NORTH_UP_1 + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_UP_2 =" + "'" + details.T_HARDNESS_NORTH_UP_2 + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_UP_3 =" + "'" + details.T_HARDNESS_NORTH_UP_3 + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_UP_CENT =" + "'" + details.T_HARDNESS_NORTH_UP_CENT + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_DOWN_1 =" + "'" + details.T_HARDNESS_NORTH_DOWN_1 + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_DOWN_2 =" + "'" + details.T_HARDNESS_NORTH_DOWN_2 + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_DOWN_3 =" + "'" + details.T_HARDNESS_NORTH_DOWN_3 + "'");
            updateQuery.Append("  ,T_HARDNESS_NORTH_DOWN_CEN =" + "'" + details.T_HARDNESS_NORTH_DOWN_CEN + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_UP_1 =" + "'" + details.T_HARDNESS_SOUTH_UP_1 + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_UP_2 =" + "'" + details.T_HARDNESS_SOUTH_UP_2 + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_UP_3 =" + "'" + details.T_HARDNESS_SOUTH_UP_3 + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_UP_CENT =" + "'" + details.T_HARDNESS_SOUTH_UP_CENT + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_DOWN_1 =" +"'" + details.T_HARDNESS_SOUTH_DOWN_1 + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_DOWN_2 =" +"'" + details.T_HARDNESS_SOUTH_DOWN_2 + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_DOWN_3 =" + "'" + details.T_HARDNESS_SOUTH_DOWN_3 + "'");
            updateQuery.Append("  ,T_HARDNESS_SOUTH_DOWN_CEN =" +"'" + details.T_HARDNESS_SOUTH_DOWN_CEN + "'");
            updateQuery.Append("  ,HT_NEXT_PROCESS =" + "'" + details.HT_NEXT_PROCESS + "'");
            updateQuery.Append("  ,USER_ID =" + "'" + details.USER_ID + "'");
            updateQuery.Append("  ,INPUT_LOCATION_CD =" + ("'" + details.INPUT_LOCATION_CD + "'"));
            updateQuery.Append("  ,UPDATED_DATE = SYSDATE");


            updateQuery.Append(" WHERE HARDENING_TEMPERING_CD = " + details.HARDENING_TEMPERING_CD);
            ExecuteNonQuery(updateQuery.ToString());
            return Convert.ToInt64(ExecuteScalar("SELECT NVL(MSG_CD,0) FROM TRN_HARDENING_TEMPERING WHERE HARDENING_TEMPERING_CD= " + details.HARDENING_TEMPERING_CD));
        }

        public long UpdateHTQualityDetails(HardeningTemperingDetails details)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("  UPDATE TRN_HT_QUALITY SET ");
          
            updateQuery.Append("  MSG_CD =" + "'" + details.MSG_CD + "'");
            updateQuery.Append("  ,PROCESS_PLAN_CD =" + "'" + details.PROCESS_PLAN_CD + "'");
        
            updateQuery.Append("  ,INSPECTION_DATE =" + "TO_DATE('" + Convert.ToDateTime(details.INSPECTION_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "','yyyy/mm/dd HH24:MI:SS')");

            updateQuery.Append("  ,INSPECTED_BY_CD =" + "'" + details.INSPECTED_BY_CD + "'");
            updateQuery.Append("  ,DM_F_MIN =" + "'" + details.DM_F_MIN + "'");
            updateQuery.Append("  ,DM_F_MAX =" + "'" + details.DM_F_MAX + "'");
            updateQuery.Append("  ,DM_T_MIN =" + "'" + details.DM_T_MIN + "'");
            updateQuery.Append("  ,DM_T_MAX =" + "'" + details.DM_T_MAX + "'");
            updateQuery.Append("  ,STRUCTURE_OUT =" + "'" + details.STRUCTURE_OUT + "'");
            updateQuery.Append("  ,STRUCTURE_IN =" + "'" + details.STRUCTURE_IN + "'");
            updateQuery.Append("  ,DECISION =" + "'" + details.DECISION + "'");
            updateQuery.Append("  ,HTQ_NEXT_PROCESS =" + "'" + details.HTQ_NEXT_PROCESS + "'");
            updateQuery.Append("  ,USER_ID =" + "'" + details.USER_ID + "'" );
            updateQuery.Append("  ,INPUT_LOCATION_CD =" + "'" + details.INPUT_LOCATION_CD + "'");
            updateQuery.Append("  ,UPDATED_DATE = SYSDATE");


            updateQuery.Append(" WHERE HT_QUALITY_CD = " + details.HT_QUALITY_CD);
            ExecuteNonQuery(updateQuery.ToString());
            long msgCD = Convert.ToInt64(ExecuteScalar("SELECT NVL(MSG_CD,0) FROM TRN_HT_QUALITY WHERE HT_QUALITY_CD= '" + details.HT_QUALITY_CD + "'"));
            return msgCD;
        }

        public void DeleteHTQualityDetails(long htQualityCD)
        {

            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  TRN_HT_QUALITY ");
            deleteQuery.Append(" WHERE HT_QUALITY_CD =" + htQualityCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }

        public void DeleteHardnessTemperingDetails(long hardening_tempering_cd)
        {
            
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(" DELETE FROM  TRN_HARDENING_TEMPERING ");
                deleteQuery.Append(" WHERE HARDENING_TEMPERING_CD =" + hardening_tempering_cd);
                ExecuteNonQuery(deleteQuery.ToString());        
        }

        public List<HardeningTemperingDetails> GetHardnessTemperingList(List<SearchField> searchFields, string sortCiteria, int LocationCD)
        {
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.CHUUBAN "); 
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
          
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
          
            selectQuery.Append(" ,EMP.SHORT_NAME AS  PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
          
            selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
            selectQuery.Append(" ,HT.HARDENING_TEMPERING_CD"); 
            selectQuery.Append(" ,HT.FURNACE_NO");
            selectQuery.Append(" ,HT.NO_OF_UNITS");
            selectQuery.Append(" ,HT.WEIGHT");
           
            
            selectQuery.Append(" ,TO_CHAR( HT.START_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_HARDENING_STRING");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_HARDENING_TEMPERING HT ON");
            selectQuery.Append(" HT.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD");
           
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
          
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON HT.OPERATOR_CD_HARDENING = EMP.EMPLOYEE_CD");

            selectQuery.Append(" WHERE ");
           // selectQuery.Append(" HT.END_DATE_HARDENING IS NOT NULL ");
            selectQuery.Append(" HT.INPUT_LOCATION_CD = " + LocationCD);
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            //selectQuery.Append(" AND ROWNUM <= ");
            //selectQuery.Append(" (CASE");
            //selectQuery.Append(" WHEN (ROWNUM >" + _maxRowLimit + ")  THEN " + _maxRowLimit + "");
            //selectQuery.Append(" ELSE ROWNUM");
            //selectQuery.Append(" END)");

            var query = selectQuery.ToString();
            if (searchFields != null && searchFields.Count > 0)
            {
                string whereCondition = " AND ";
                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
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
                query += "ORDER BY PP_CHUUBAN_GYOUBAN ASC, HT.FURNACE_NO";
            }
            return ExecuteQueryMultiResult<HardeningTemperingDetails>(query);
        }

        public List<HardeningTemperingDetails> GetHTQualityList(List<SearchField> searchFields, string sortCiteria, int LocationCD)
        {
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.CHUUBAN ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,EMP.SHORT_NAME AS  PP_EMPLOYEE_NAME ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
            selectQuery.Append(" ,HTQ.HT_QUALITY_CD AS HARDENING_TEMPERING_CD");
            selectQuery.Append(" ,HT.FURNACE_NO");
            selectQuery.Append(" ,HT.NO_OF_UNITS");
            selectQuery.Append(" ,HT.UNIT_TYPE");
            selectQuery.Append(" ,HT.WEIGHT");
            selectQuery.Append(" ,TO_CHAR( HTQ.INSPECTION_DATE,'yyyy/mm/dd HH24:MI' ) AS START_DATE_HARDENING_STRING");
            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" INNER JOIN TRN_HT_QUALITY HTQ ON");
            selectQuery.Append(" PP.PROCESS_PLAN_CD = HTQ.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT  JOIN TRN_HARDENING_TEMPERING HT ON");
            selectQuery.Append(" HT.PROCESS_PLAN_CD = HTQ.PROCESS_PLAN_CD AND  UPPER(HT.FURNACE_NO) = UPPER(HTQ.FURNACE_NO) ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD = PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN  MST_EMPLOYEE EMP ON HT.OPERATOR_CD_HARDENING = EMP.EMPLOYEE_CD");
            selectQuery.Append(" WHERE HT.END_DATE_HARDENING IS NOT NULL ");
            selectQuery.Append(" AND HTQ.INPUT_LOCATION_CD = " + LocationCD);
            selectQuery.Append(" AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
            selectQuery.Append(" AND ROWNUM <= ");
            selectQuery.Append(" (CASE");
            selectQuery.Append(" WHEN (ROWNUM >" + _maxRowLimit + ")  THEN " + _maxRowLimit + "");
            selectQuery.Append(" ELSE ROWNUM");
            selectQuery.Append(" END)");

            var query = selectQuery.ToString();
            if (searchFields != null && searchFields.Count > 0)
            {
                string whereCondition = " AND ";
                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
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
                query += "ORDER BY PP_CHUUBAN_GYOUBAN ASC, HTQ.INSPECTION_DATE DESC";
            }
            return ExecuteQueryMultiResult<HardeningTemperingDetails>(query);
        }

        public HardeningTemperingDetails GetHardnessTemperingDetailsByHardnessTemperingCD(long hardnessTemperingCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  PP.PRODUCT_CD ");
            selectQuery.Append(" ,PP.CHUUBAN AS PP_CHUUBAN"); 
            selectQuery.Append(" ,PP.GYOUBAN AS PP_GYOUBAN"); 
            selectQuery.Append(" ,PP.PROCESS_PLAN_CD AS PP_PROCESS_PLAN_CD ");
            selectQuery.Append(" ,PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST ");
            selectQuery.Append(" ,CUS.SHORT_NAME AS PP_CUSTOMER_NAME ");
            selectQuery.Append(" ,PP.CHARGE AS PP_CHARGE ");
            selectQuery.Append(" ,PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK ");
            //selectQuery.Append(" ,PP.MATL_DIM_D AS PP_MATL_DIM_D ");
            //selectQuery.Append(" ,PP.PROD_DIM_D AS PP_PROD_DIM_D ");
            selectQuery.Append(" ,PP.MATL_DIM_D || ' ' || PP.MATL_SHAPE AS PP_MATL_DIM_D_SHAPE ");
            selectQuery.Append(" ,PP.PROD_DIM_D || ' ' || PP.PROD_SHAPE AS PP_PROD_DIM_D_SHAPE_STRING ");
            selectQuery.Append(", PP.NO_OF_UNITS AS PP_NO_OF_UNITS ");
            selectQuery.Append(" ,PP.ACT_ALLOC_QTY AS PP_QUANTITY ");

            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MIN ");
            selectQuery.Append(" ,PP.HARDNESS_PATTERN2_MAX ");
            selectQuery.Append(" ,PP.PROCESS_PLAN_CMPL_DATE");

            selectQuery.Append(", HT.HARDENING_TEMPERING_CD ");
            selectQuery.Append(", HT.LOCATION_CD ");
            selectQuery.Append(", HT.MSG_CD ");
            selectQuery.Append(", HT.PROCESS_PLAN_CD ");
            selectQuery.Append(", HT.FURNACE_NO ");
            selectQuery.Append(", HT.TABA_NO ");
            selectQuery.Append(", HT.HARDENING_MACHINE_CD ");
            selectQuery.Append(", HT.TEMPERING_MACHINE_CD ");
            selectQuery.Append(", HT.METALLIC_PLATE_CODE ");
            selectQuery.Append(", HT.NO_OF_UNITS ");
            selectQuery.Append(", HT.UNIT_TYPE ");
            selectQuery.Append(", HT.WEIGHT ");
            selectQuery.Append(", HT.WATER_OIL_TYPE ");
            selectQuery.Append(", HT.TEMPERATURE_FURNACE ");
            selectQuery.Append(", HT.CONCENTRATION_FURNACE ");
            selectQuery.Append(", HT.FREQUENCY_FURNACE ");
            selectQuery.Append(", HT.C_FURNACE ");
            selectQuery.Append(", HT.MN_FURNACE ");
            selectQuery.Append(", HT.MO_FURNACE ");
            selectQuery.Append(", HT.CR_FURNACE ");
            selectQuery.Append(", HT.HARDNESS_MIN ");
            selectQuery.Append(", HT.HARDNESS_MAX ");
            selectQuery.Append(", HT.OPERATOR_CD_HARDENING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_HARDENING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_HARDENING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_HARDENING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_HARDENING ");
            selectQuery.Append(", HT.TIME_HARDENING ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_UP_CEN ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.Q_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.OPERATOR_CD_TEMPERING ");
            selectQuery.Append(", TO_CHAR( HT.START_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS START_DATE_TEMPERING_STRING");
            selectQuery.Append(", TO_CHAR( HT.END_DATE_TEMPERING,'yyyy/mm/dd HH24:MI' ) AS END_DATE_TEMPERING_STRING");
            selectQuery.Append(", HT.TEMPERATURE_TEMPERING ");
            selectQuery.Append(", HT.TIME_TEMPERING ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_NORTH_DOWN_CEN ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_UP_CENT ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_1 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_2 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_3 ");
            selectQuery.Append(", HT.T_HARDNESS_SOUTH_DOWN_CEN ");
            selectQuery.Append(", HT.HT_NEXT_PROCESS ");
            selectQuery.Append(", MSG.MSG_CONTENTS AS REMARK");


            selectQuery.Append("  FROM TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" LEFT  JOIN TRN_HARDENING_TEMPERING HT ON");
            selectQuery.Append(" HT.PROCESS_PLAN_CD=PP.PROCESS_PLAN_CD");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON HT.MSG_CD =MSG.MSG_CD");

            selectQuery.Append(" WHERE HT.HARDENING_TEMPERING_CD=" + hardnessTemperingCD);
            return ExecuteQuerySingleResult<HardeningTemperingDetails>(selectQuery.ToString());
        }

        /// <summary>
        /// CREATED BY :    
        /// used screens: Factory Daily Work Status Search
        /// </summary>
        /// <returns>
        /// List of Hardness Tempering process data according to search criteria        
        /// </returns>
        /// 
        public List<HardeningTemperingDetails> GetHardnessTempringOnFactoryDailyWorkStatus(List<SearchField> searchCriteria, string sortCiteria, int rowLimit)
        {
            StringBuilder selectQuery = new StringBuilder();
            List<HardeningTemperingDetails> result = new List<HardeningTemperingDetails>();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" HT.FURNACE_NO AS FURNACE_NO, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR || PP.CHUUBAN, PP.GYOUBAN) AS PP_CHUUBAN, ");
            selectQuery.Append(" CUST.SHORT_NAME AS PP_CUSTOMER_NAME, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PP_PROD_NAME_CUST, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.PROD_DIM_D AS PP_PROD_DIM_D, ");
            selectQuery.Append(" HT.METALLIC_PLATE_CODE AS METALLIC_PLATE_CODE, ");
            selectQuery.Append(" PP.NO_OF_UNITS AS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT,");
            selectQuery.Append(" PP.ACT_ALLOC_QTY AS PP_ACT_ALLOC_QTY, ");
            selectQuery.Append(" HT.NO_OF_UNITS AS NO_OF_UNITS, ");
            selectQuery.Append(" HT.UNIT_TYPE AS UNIT,");
            selectQuery.Append(" HT.WEIGHT AS WEIGHT, ");
            selectQuery.Append(" HT.WATER_OIL_TYPE,");
            selectQuery.Append(" HT.TEMPERATURE_FURNACE AS TEMPERATURE_FURNACE, ");
            selectQuery.Append(" HT.CONCENTRATION_FURNACE AS CONCENTRATION_FURNACE, ");
            selectQuery.Append(" HT.FREQUENCY_FURNACE AS FREQUENCY_FURNACE, ");
            selectQuery.Append(" HT.C_FURNACE AS C_FURNACE, ");
            selectQuery.Append(" HT.MN_FURNACE AS MN_FURNACE, ");
            selectQuery.Append(" HT.MO_FURNACE AS MO_FURNACE, ");
            selectQuery.Append(" HT.CR_FURNACE AS CR_FURNACE, ");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MIN AS HARDNESS_PATTERN2_MIN,");
            selectQuery.Append(" PP.HARDNESS_PATTERN2_MAX AS HARDNESS_PATTERN2_MAX,");
            selectQuery.Append(" HT.HARDNESS_MIN AS HARDNESS_MIN, ");
            selectQuery.Append(" HT.HARDNESS_MAX AS HARDNESS_MAX, ");
            selectQuery.Append(" M.MSG_CONTENTS AS REMARK, ");
            selectQuery.Append(" MAC.MACHINE_NAME AS HARDENING_MACHINE_NAME, ");
            selectQuery.Append(" EMP.SHORT_NAME AS HARDENING_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(HT.START_DATE_HARDENING, 'YYYY/MM/DD HH24:MI') AS START_DATE_HARDENING_STRING, ");
            selectQuery.Append(" TO_CHAR(HT.END_DATE_HARDENING, 'YYYY/MM/DD HH24:MI') AS END_DATE_HARDENING_STRING, ");
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
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_1 AS Q_HARDNESS_SOUTH_UP_1, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_2 AS Q_HARDNESS_SOUTH_UP_2, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_3 AS Q_HARDNESS_SOUTH_UP_3, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_UP_CEN AS Q_HARDNESS_SOUTH_UP_CEN, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_1 AS Q_HARDNESS_SOUTH_DOWN_1, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_2 AS Q_HARDNESS_SOUTH_DOWN_2, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_3 AS Q_HARDNESS_SOUTH_DOWN_3, ");
            selectQuery.Append(" HT.Q_HARDNESS_SOUTH_DOWN_CEN AS Q_HARDNESS_SOUTH_DOWN_CEN, ");
            selectQuery.Append(" MAC1.MACHINE_NAME AS TEMPERING_MACHINE_NAME, ");
            selectQuery.Append(" EMP1.SHORT_NAME AS TEMPERING_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(HT.START_DATE_TEMPERING, 'YYYY/MM/DD HH24:MI') AS START_DATE_TEMPERING_STRING, ");
            selectQuery.Append(" TO_CHAR(HT.END_DATE_TEMPERING, 'YYYY/MM/DD HH24:MI') AS END_DATE_TEMPERING_STRING, ");
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

            selectQuery.Append(" EMPL.SHORT_NAME AS INSPECTED_OPERATOR_NAME, ");
            selectQuery.Append(" TO_CHAR(HTQ.INSPECTION_DATE,'YYYY/MM/DD') AS INSPECTION_DATE_STRING ,");            
            selectQuery.Append(" LOC.SHORT_NAME AS LOCATION_NAME, ");            
            selectQuery.Append(" HTQ.DECISION AS DECISION, ");
            selectQuery.Append(" PP.PROD_SPEC_NO AS PP_PROD_SPEC_NO, ");
            selectQuery.Append(" HTQ.DM_F_MIN AS DM_F_MIN,");
            selectQuery.Append(" HTQ.DM_F_MAX AS DM_F_MAX,");
            selectQuery.Append(" HTQ.DM_T_MIN AS DM_T_MIN,");
            selectQuery.Append(" HTQ.DM_T_MAX AS DM_T_MAX,");
            selectQuery.Append(" HTQ.STRUCTURE_IN AS STRUCTURE_IN,");
            selectQuery.Append(" HTQ.STRUCTURE_OUT AS STRUCTURE_OUT,");
            selectQuery.Append(" M1.MSG_CONTENTS AS HT_REMARK,");


            selectQuery.Append(" INTERN.INTERNAL_NAME, ");
            selectQuery.Append(" PP.PROD_SHAPE AS PP_PROD_SHAPE, ");
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE, ");
            selectQuery.Append(" PP.PROD_SPEC_NO1, ");
            selectQuery.Append(" PP.PROD_SPEC_NO2, ");
            selectQuery.Append(" PP.PROD_LENGTH ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HARDENING_TEMPERING HT ");
            selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD AND HTQ.FURNACE_NO=HT.FURNACE_NO ");
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
            string whereCondition = "";
            var query = selectQuery.ToString();
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
                    //whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    //andOperator = " AND ";
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
            //    query += " ORDER BY INSPECTION_DATE_STRING DESC,HT.END_DATE_TEMPERING DESC,CUST.YOBINA,CUST.YOBIGANA,CUST.SHORT_NAME,PP_PROD_NAME_CUST,PP.PROD_SHAPE,PP.PROD_DIM_D,PP.PROD_DIM_W,PP.PROD_DIM_OTHER,PP.PROD_LENGTH,PP.LENGTH_OTHER,PP_CHUUBAN,FURNACE_NO";
            //}
            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            result = ExecuteQueryMultiResult<HardeningTemperingDetails>(query);            
            return result;
        }

        /// <summary>
        /// CREATED BY :  
        /// used screens: Daily Process Analysis With CSV
        /// </summary>
        /// <returns>
        /// List of Hardness Tempering process data according to search criteria        
        /// </returns>
        /// 
        public List<HardeningTemperingDetails> GetHardnessTemperingDetailsOnDailyProcessAnalysis(List<SearchField> searchCriteria,string sortCiteria, GridSearchFields gridsearchresult)
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
            // selectQuery.Append(" INTERN.INTERNAL_NAME AS INTERNAL_NAME, ");
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
            selectQuery.Append(" PP.COIL_DIA_WINDING AS PP_COIL_DIA_WINDING, ");
            selectQuery.Append(" CONDITION.CONDITION_DESCRIPTION AS PROD_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHAMFER AS PP_CHAMFER, ");
            selectQuery.Append(" PP.CORNER_RADIUS AS PP_CORNER_RADIUS, ");
            selectQuery.Append(" PP.MAGARI_KUBUN AS MAGARI_KUBUN, ");
            selectQuery.Append(" PP.MAGARI_BEND_TOL AS MAGARI_BEND_TOL, ");
            selectQuery.Append(" PP.MAGARI_SPAN AS MAGARI_SPAN, ");
            selectQuery.Append(" PP.NEJIRE_ANGLE AS NEJIRE_ANGLE, ");
            selectQuery.Append(" PP.NEJIRE_SPAN AS NEJIRE_SPAN, ");
            selectQuery.Append(" PP.UNIT_WT_KUBUN AS PP_UNIT_WT_KUBUN, ");
            selectQuery.Append(" PP.UNIT_WT AS UNIT_WT, ");
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
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1 AS IMPACT_TEST_PATTERN1, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN1_MIN_MAX AS IMPACT_TEST_PATTERN1_MIN_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MIN AS IMPACT_TEST_PATTERN2_MIN, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN2_MAX AS IMPACT_TEST_PATTERN2_MAX, ");
            selectQuery.Append(" PP.IMPACT_TEST_PATTERN3_ACTUAL AS IMPACT_TEST_PATTERN3_ACTUAL, ");
            //77 not in excel,
            selectQuery.Append(" PP.RANK_STD AS RANK_STD, ");
            selectQuery.Append(" PP.HT_CONDITION AS PP_HT_CONDITION, ");
            // --79,80 not in excel
            selectQuery.Append(" PP.MATL_TYPE_CBK AS PP_MATL_TYPE_CBK, ");
            selectQuery.Append(" PP.MATL_NAME_MAK AS PP_MATL_NAME_MAK, ");           
            selectQuery.Append(" PP.MATL_SHAPE AS PP_MATL_SHAPE , ");
            selectQuery.Append(" PP.MATL_DIM_D AS PP_MATL_DIM_D, ");
            selectQuery.Append(" PP.MATL_DIM_W AS PP_MATL_DIM_W, ");
            selectQuery.Append(" PP.MATL_DIM_OTHER AS PP_MATL_DIM_OTHER, ");
            selectQuery.Append(" PP.MATL_LENGTH AS PP_MATL_LENGTH, ");
            selectQuery.Append(" CONDITION1.CONDITION_DESCRIPTION AS MATL_CONDITION_DESCRIPTION, ");
            selectQuery.Append(" PP.CHARGE AS PP_CHARGE, ");
            selectQuery.Append(" PP.PROCESS_PLAN_KUBUN AS PROCESS_PLAN_KUBUN, ");
            selectQuery.Append(" PP.PROD_MNGMNT_KUBUN AS PP_PROD_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.SPCL_MNGMNT_KUBUN AS PP_SPCL_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.STORAGE_MNGMNT_KUBUN AS PP_STORAGE_MNGMNT_KUBUN, ");
            selectQuery.Append(" PP.KAKOUIARI_QTY AS KAKOUIARI_QTY, ");
            selectQuery.Append(" TO_CHAR(PP.DELIVERY_DATE, 'yyyy/mm/dd') AS PP_DELIVERY_DATE_STRING, ");
            selectQuery.Append(" PP.NO_OF_UNITS PP_NO_OF_UNITS, ");
            selectQuery.Append(" PP.UNIT AS PP_UNIT, ");
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
            selectQuery.Append(" HT.UNIT_TYPE AS UNIT_TYPE, ");
            selectQuery.Append(" HT.WEIGHT AS WEIGHT, ");
            selectQuery.Append(" HT.WATER_OIL_TYPE AS WATER_OIL_TYPE, ");
            selectQuery.Append(" HT.TEMPERATURE_FURNACE AS TEMPERATURE_FURNACE, ");
            selectQuery.Append(" HT.CONCENTRATION_FURNACE AS CONCENTRATION_FURNACE, ");
            selectQuery.Append(" HT.FREQUENCY_FURNACE AS FREQUENCY_FURNACE, ");
            selectQuery.Append(" HT.C_FURNACE AS C_FURNACE, ");
            selectQuery.Append(" HT.MN_FURNACE AS MN_FURNACE, ");
            selectQuery.Append(" HT.MO_FURNACE AS MO_FURNACE, ");
            selectQuery.Append(" HT.CR_FURNACE AS CR_FURNACE, ");
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
            selectQuery.Append(" HTQ.DECISION AS DECISION,");
            selectQuery.Append(" NVL(M1.MSG_CONTENTS, M.MSG_CONTENTS) AS REMARK");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HARDENING_TEMPERING HT ");
            selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD AND HTQ.FURNACE_NO=HT.FURNACE_NO");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" INNER JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
            selectQuery.Append(" LEFT JOIN MST_RECEIVER REC ON REC.RECEIVER_CD = PP.RECEIVER_CD ");
            //selectQuery.Append(" INNER JOIN MST_DEPARTMENT_GROUP DEP ON DEP.DEPARTMENT_GROUP_CD = PP.DEPARTMENT_GROUP_CD ");
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
           
            
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMPL ON EMPL.EMPLOYEE_CD = HTQ.INSPECTED_BY_CD");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M ON M.MSG_CD = HT.MSG_CD ");
            selectQuery.Append(" LEFT JOIN TRN_MESSAGE M1 ON M1.MSG_CD = HTQ.MSG_CD ");


            string whereCondition = "";
            var query = selectQuery.ToString();
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
            else if (gridsearchresult.ROW_LIMIT > 0  && searchCriteria.Count <= 0)
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
            return ExecuteQueryMultiResult<HardeningTemperingDetails>(query);
        }
        public HardeningTemperingDetails FurnaceIsAlreadyEntered(string furnaceNo)
        {


            
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" PP.PROCESS_PLAN_CD ");
            query.Append(" ,HT.HARDENING_TEMPERING_CD ");
            query.Append(" FROM TRN_HARDENING_TEMPERING HT");
            query.Append(" LEFT JOIN TRN_PROCESS_PLAN PP ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD");
            query.Append(" WHERE LOWER(HT.FURNACE_NO) =  '" + furnaceNo.ToLower() + "' ");
          
            return ExecuteQuerySingleResult<HardeningTemperingDetails>(query.ToString());
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
        public int GetRowCountForHardnessTemperingtFactory(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");            
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HARDENING_TEMPERING HT ");
            selectQuery.Append(" LEFT JOIN TRN_HT_QUALITY HTQ ON HTQ.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD AND HTQ.FURNACE_NO=HT.FURNACE_NO ");
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
            string whereCondition = "";
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                whereCondition = " WHERE ";

                string andOperator = string.Empty;

                foreach (var searchField in searchCriteria)
                {
                    //whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    //andOperator = " AND ";
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
        public int GetRowCountForHardnessTemperingDailyAnalysis(List<SearchField> searchCriteria)
        {
            int count;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT COUNT(*) ");
            
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

            selectQuery = new StringBuilder();
            selectQuery.Append(query);
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return count;
        }
        public List<ThabaDetails> GetThabaDetails(int processPlanCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" HARDENING_TEMPERING_CD ");
            selectQuery.Append(" ,TABA_NO ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HARDENING_TEMPERING ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" PROCESS_PLAN_CD =  ");
            selectQuery.Append(" '"  + processPlanCD + "' ");
            var result = ExecuteQueryMultiResult<ThabaDetails>(selectQuery.ToString());
            return result;
        }
        public bool IsProcessPlanSaved(GridSearchFields request)
        {
            bool isSaved = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(HT.HT_QUALITY_CD) ");
           
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_HT_QUALITY HT ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ");
            selectQuery.Append(" ON PP.PROCESS_PLAN_CD = HT.PROCESS_PLAN_CD ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" PROCESS_PLAN_CMPL_DATE IS NULL  ");
            selectQuery.Append(" AND PP.CHUUBAN =  ");
            selectQuery.Append(" '" + request.CHUUBAN + "' ");
            selectQuery.Append(" AND PP.GYOUBAN =  ");
            selectQuery.Append(" '" + request.GYOUBAN + "' ");
            selectQuery.Append(" AND LOWER(HT.FURNACE_NO) =  ");
            selectQuery.Append(" '" + request.FURNACE_NO.ToLower() + "' ");
            int counts = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (counts > 0) 
            {
                isSaved = true; 
            }
            return isSaved;
        }

    }
}
