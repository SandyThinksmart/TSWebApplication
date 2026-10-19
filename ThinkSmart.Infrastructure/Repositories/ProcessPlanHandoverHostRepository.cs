using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Constants;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class ProcessPlanHandoverHostRepository : BaseRepository, IProcessPlanHandoverHostRepository
    {
        private readonly string _connectionString;
        private readonly string _connectionStringForRplication;
        private readonly string _locationCd_DaiIchi;
        private readonly string _locationCd_DaiNi;
        private readonly string _locationCd_Nogi;
        private readonly string _tensile_yeild_Value;
        private readonly string _elongation1_press1_Value;
        private readonly string _hardness2_Value;
        string result;
        decimal? codeValue;
        string minMaxValue;
        string minValue;
        string maxValue;
        string actualValue;
        string str;
        int index = 0;
        bool isStrPatternStartWithNum = false;
        string[] data;
        private readonly string _Prod_Spec_No_1_Value;
        public ProcessPlanHandoverHostRepository(IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _connectionStringForRplication = _configuration.GetConnectionString("OracleDBConnectionForReplication");
            _locationCd_DaiIchi = _configuration.GetSection("location_CD").GetSection("DaiIchi").Value;
            _locationCd_DaiNi = _configuration.GetSection("location_CD").GetSection("DaiNi").Value;
            _locationCd_Nogi = _configuration.GetSection("location_CD").GetSection("Nogi").Value;
            _tensile_yeild_Value = _configuration.GetSection("host_Japanese_Value").GetSection("tensile_yeild_Value").Value;
            _elongation1_press1_Value = _configuration.GetSection("host_Japanese_Value").GetSection("elongation1_press1_Value").Value;
            _hardness2_Value = _configuration.GetSection("host_Japanese_Value").GetSection("hardness2_Value").Value;
            _Prod_Spec_No_1_Value = _configuration.GetSection("Prod_Spec_No_1_Value").Value;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/01/31       
        /// used screens: ProcessPlanHandoverHost
        /// </summary>
        /// <returns>
        /// Fetch data by using OrNo using table TRN_PROCESS_PLAN and HTMP_PRODUCT_ORDER
        /// </returns>
        /// 
        public List<GridSearchFields> ValidateChuuban(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();

            int count = 0;
            int countForHost = 0;
            int completeDateCnt = 0;
            int nifudaCnt = 0;
            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                if (processPlanDetails[i].DISPLAYPROCESSPLANDATA == "beforeProcessStart")//
                {
                    StringBuilder selectQueryProcessCntForHost = new StringBuilder();
                    selectQueryProcessCntForHost.Append("SELECT COUNT(*) FROM hsk_replication.HTMP_PRODUCT_ORDER ");
                    selectQueryProcessCntForHost.Append(" WHERE ");
                    selectQueryProcessCntForHost.Append(" CHUUBAN || Lpad(GYOUBAN,2,0)  = '" + processPlanDetails[i].CHUUBAN + "'  ");
                    countForHost = Convert.ToInt32(ExecuteScalar(selectQueryProcessCntForHost.ToString()));
                    if (countForHost > 0)
                    {
                        StringBuilder selectQueryNifudaFor = new StringBuilder();
                        selectQueryNifudaFor.Append("SELECT COUNT(*) FROM hsk.TAG ");
                        selectQueryNifudaFor.Append(" WHERE ");
                        selectQueryNifudaFor.Append(" ORDERNO || Lpad(ORDERNOROWNO,2,0)  = '" + processPlanDetails[i].CHUUBAN + "'  ");
                        nifudaCnt = Convert.ToInt32(ExecuteScalar(selectQueryNifudaFor.ToString()));
                        if (nifudaCnt == 0)
                        {
                            GridSearchFields message = new GridSearchFields()
                            { CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN, RESPONCE = "Nifuda Not Found For This OrNo" };
                            result.Add(message);
                        }
                    }
                    else
                    {
                        GridSearchFields message = new GridSearchFields()
                        { CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN, RESPONCE = "Or No Not Found" };
                        result.Add(message);
                    }
                }

                else
                {

                    StringBuilder selectQueryProcessCntForAfter = new StringBuilder();
                    selectQueryProcessCntForAfter.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
                    selectQueryProcessCntForAfter.Append(" WHERE ");
                    selectQueryProcessCntForAfter.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "'  AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
                    count = Convert.ToInt32(ExecuteScalar(selectQueryProcessCntForAfter.ToString()));
                    if (count == 0)
                    {
                        GridSearchFields message = new GridSearchFields()
                        { CHUUBAN_GYOUBAN = processPlanDetails[i].CHUUBAN, RESPONCE = "Process Is Not Started" };
                        result.Add(message);
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/01/04       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Insert data into table TRN_PROCESS_PLAN 
        /// Insert data into table TRN_NIFUDA 
        /// Insert data into table TRN_WIP_STATUS
        /// </returns>
        /// 
        public string SaveProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {


            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                GridSearchFields HPOResult = new GridSearchFields();
                int checkCount = CheckProcessPlanDataExist(processPlanDetails[i]);
                if (checkCount == 0)
                {

                    HPOResult = CheckProcessCount(processPlanDetails[i]);
                    InsertHandoverData(processPlanDetails[i], HPOResult);
                }
                else
                {
                    var resultCompleted = CompleteExistingProcessPlanData(processPlanDetails[i]);
                    if (resultCompleted == "Data Updated Successfully")
                    {
                        HPOResult = CheckProcessCount(processPlanDetails[i]);
                        InsertHandoverData(processPlanDetails[i], HPOResult);
                    }

                }

            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2026/09/16       
        /// used screens: ProcessPlanHandoverEntry
        /// checking for old data into table TRN_PROCESS_PLAN 
        /// </summary>
        /// <returns>
        /// result 1=Old data found for this chuuban
        /// result 0=Old data not found for this chuuban
        /// </returns>
        /// 
        public int CheckProcessPlanDataExist(GridSearchFields processPlanDetails)
        {
            int result;
            int count;

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT SUBSTR(CHUUBAN_YEAR, 2, 1) FROM TRN_PROCESS_PLAN ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" CHUUBAN = '" + processPlanDetails.CHUUBAN + "' AND GYOUBAN ='" + processPlanDetails.GYOUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
            string chuubanYear = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
            if (chuubanYear != processPlanDetails.CHHUBAN_YEAR && !string.IsNullOrWhiteSpace(chuubanYear))
            {
                result = 1;  //Old data found for this chuuban
            }
            else
            {
                result = 0;  //Old data not found for this chuuban
            }
            return result;
        }

        /// <summary>
        ///  CREATED BY : Namrata Ubale on 2026/09/16
        /// </summary>
        ///  used screens: ProcessPlanHandoverEntry
        /// checking for old data into table TRN_PROCESS_PLAN  and Update the PROCESS_PLAN_CMPL_DATE for this old data
        /// <param name="processPlanDetails"></param>
        /// 
        /// <returns>
        /// result "Data Updated Successfully"=Old data found for this chuuban and updated successfully
        /// </returns>

        public string CompleteExistingProcessPlanData(GridSearchFields processPlanDetails)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" CHUUBAN = '" + processPlanDetails.CHUUBAN + "' AND GYOUBAN ='" + processPlanDetails.GYOUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
            var process_Plan_Cd = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("UPDATE TRN_PROCESS_PLAN SET ");
            updateQuery.Append("PROCESS_PLAN_CMPL_DATE = TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','yyyy/MM/dd HH24:MI:SS') ");
            updateQuery.Append("WHERE ");
            updateQuery.Append("PROCESS_PLAN_CD = '" + process_Plan_Cd + "'");
            ExecuteNonQuery(updateQuery.ToString());
            result = "Data Updated Successfully";
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/07/24       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Insert data into table TRN_PROCESS_PLAN 
        /// Insert data into table TRN_NIFUDA 
        /// Insert data into table TRN_WIP_STATUS
        /// </returns>
        /// 

        public string InsertHandoverData(GridSearchFields processPlanDetails, GridSearchFields HPOResult)
        {
            var PrintDate = "";
            var Del_Date = "";
            var Delivery_Date = "";
            var Del_Date_Month = "";
            var Del_Date_Date = "";
            var Print_Date_Month_date = "";
            var Print_Date_Month = "";
            var Print_Date_Date = "";
            var Print_date = "";
            var Date_Year = "";
            var unitWtData1 = "";
            var unitWtData2 = "";
            var unitWtData3 = "";
            string[] unitWtData;



            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append("INSERT INTO TRN_PROCESS_PLAN(");
            insertQuery.Append("PROCESS_PLAN_CD,");//in seq we added code for seq process plan cd
            insertQuery.Append("PRODUCT_CD,");
            insertQuery.Append("ROOT_VER_PRODUCT_KEY,");
            insertQuery.Append("KAKOUIRAI_CD,");
            insertQuery.Append("ACT_ALLOC_CD,");
            insertQuery.Append("CUSTOMER_CD,");
            insertQuery.Append("RECEIVER_CD,");
            insertQuery.Append("INTERNAL_NAME_CD,");
            insertQuery.Append("DEPARTMENT_GROUP_CD,");
            insertQuery.Append("MATL_NAME_MAK_CD,");
            insertQuery.Append("PROD_CONDITION_CD,");
            insertQuery.Append("MATL_CONDITION_CD,");
            insertQuery.Append("NYUKA_CD,");
            insertQuery.Append("STORAGE_LOCATION_CD,");
            insertQuery.Append("LOCATION_CD,");
            insertQuery.Append("PACKING_CD,");
            insertQuery.Append("CHUUBAN,");
            insertQuery.Append("GYOUBAN,");
            insertQuery.Append("ORIGINAL_CHUBBAN,");
            insertQuery.Append("PROD_SPEC_NO,");
            insertQuery.Append("PROD_SPEC_NO1,");
            insertQuery.Append("PROD_SPEC_NO2,");
            insertQuery.Append("VER,");
            insertQuery.Append("STD_DEL_PERIOD,");
            insertQuery.Append("PROD_NAME_CUST,");//25
            insertQuery.Append("PROD_SHAPE,");
            insertQuery.Append("PROD_DIM_D,");
            insertQuery.Append("PROD_DIM_W,");
            insertQuery.Append("PROD_DIM_OTHER,");
            insertQuery.Append("DIM_TOL_MIN,");
            insertQuery.Append("DIM_TOL_MAX,");
            insertQuery.Append("DIM_TOL_MIN_CHR,");
            insertQuery.Append("DIM_TOL_MAX_CHR,");
            insertQuery.Append("PROD_LENGTH,");
            insertQuery.Append("LENGTH_OTHER,");
            insertQuery.Append("LENGTH_TOL_MIN,");
            insertQuery.Append("LENGTH_TOL_MAX,");
            insertQuery.Append("LENGTH_TOL_MIN_CHR,");
            insertQuery.Append("LENGTH_TOL_MAX_CHR,");
            insertQuery.Append("COIL_DIA_INTERNAL,");
            insertQuery.Append("COIL_DIA_EXTERNAL,");
            insertQuery.Append("COIL_DIA_WINDING,");
            insertQuery.Append("CHAMFER,");
            insertQuery.Append("MAGARI_KUBUN,");
            insertQuery.Append("MAGARI_BEND_TOL,");
            insertQuery.Append("MAGARI_SPAN,");
            insertQuery.Append("NEJIRE_ANGLE,");
            insertQuery.Append("NEJIRE_SPAN,");
            insertQuery.Append("UNIT_WT_KUBUN,");
            insertQuery.Append("UNIT_WT,");//50
            insertQuery.Append("UNIT_WT_MIN,");
            insertQuery.Append("UNIT_WT_MAX,");
            insertQuery.Append("EXT_TEXTURE,");
            insertQuery.Append("COLOR_ID,");
            insertQuery.Append("DISPLAY,");
            insertQuery.Append("STAMP,");
            insertQuery.Append("TEST_SPECIMEN,");
            insertQuery.Append("TENSILE_STRN_PATTERN1,");
            insertQuery.Append("TENSILE_STRN_PATTERN1_MIN_MAX,");
            insertQuery.Append("TENSILE_STRN_PATTERN2_MIN,");
            insertQuery.Append("TENSILE_STRN_PATTERN2_MAX,");
            insertQuery.Append("TENSILE_STRN_PATTERN3_ACTUAL,");
            insertQuery.Append("ELONGATION_PATTERN1,");
            insertQuery.Append("ELONGATION_PATTERN1_MIN_MAX,");
            insertQuery.Append("ELONGATION_PATTERN2_MIN,");
            insertQuery.Append("ELONGATION_PATTERN2_MAX,");
            insertQuery.Append("ELONGATION_PATTERN3_ACTUAL,");
            insertQuery.Append("HARDNESS_PATTERN1_CODE,");
            insertQuery.Append("HARDNESS_PATTERN1,");
            insertQuery.Append("HARDNESS_PATTERN1_MIN_MAX,");
            insertQuery.Append("HARDNESS_PATTERN2_CODE,");
            insertQuery.Append("HARDNESS_PATTERN2_MIN,");
            insertQuery.Append("HARDNESS_PATTERN2_MAX,");
            insertQuery.Append("HARDNESS_PATTERN3_ACTUAL,");
            insertQuery.Append("YEILD_PATTERN1,");//75
            insertQuery.Append("YEILD_PATTERN1_MIN_MAX,");
            insertQuery.Append("YEILD_PATTERN2_MIN,");
            insertQuery.Append("YEILD_PATTERN2_MAX,");
            insertQuery.Append("YEILD_PATTERN3_ACTUAL,");
            insertQuery.Append("PRESS_PATTERN1,");
            insertQuery.Append("PRESS_PATTERN1_MIN_MAX,");
            insertQuery.Append("PRESS_PATTERN2_MIN,");
            insertQuery.Append("PRESS_PATTERN2_MAX,");
            insertQuery.Append("PRESS_PATTERN3_ACTUAL,");
            insertQuery.Append("DECARB_1,");
            insertQuery.Append("DECARB_VAL_1,");
            insertQuery.Append("DECARB_2,");
            insertQuery.Append("DECARB_VAL_2,");
            insertQuery.Append("DECARB_3,");
            insertQuery.Append("DECARB_VAL_3,");
            insertQuery.Append("IMPACT_TEST_PATTERN1,");
            insertQuery.Append("IMPACT_TEST_PATTERN1_MIN_MAX,");
            insertQuery.Append("IMPACT_TEST_PATTERN2_MIN,");
            insertQuery.Append("IMPACT_TEST_PATTERN2_MAX,");
            insertQuery.Append("IMPACT_TEST_PATTERN3_ACTUAL,");
            insertQuery.Append("TEST_SPEC_REMARKS,");
            insertQuery.Append("RANK_STD,");
            insertQuery.Append("CORNER_RADIUS,");
            insertQuery.Append("MBEU,");
            insertQuery.Append("HT_CONDITION,");//100
            insertQuery.Append("PROCESS_1_CD,");
            insertQuery.Append("PROCESS_2_CD,");
            insertQuery.Append("PROCESS_3_CD,");
            insertQuery.Append("PROCESS_4_CD,");
            insertQuery.Append("PROCESS_5_CD,");
            insertQuery.Append("PROCESS_6_CD,");
            insertQuery.Append("PROCESS_7_CD,");
            insertQuery.Append("PROCESS_8_CD,");
            insertQuery.Append("PROCESS_9_CD,");
            insertQuery.Append("PROCESS_10_CD,");
            insertQuery.Append("PROCESS_11_CD,");
            insertQuery.Append("PROCESS_12_CD,");
            insertQuery.Append("PROCESS_13_CD,");
            insertQuery.Append("MACHINE_CD_1,");
            insertQuery.Append("MACHINE_CD_2,");
            insertQuery.Append("MACHINE_CD_3,");
            insertQuery.Append("MACHINE_CD_4,");
            insertQuery.Append("MACHINE_CD_5,");
            insertQuery.Append("MACHINE_CD_6,");
            insertQuery.Append("MACHINE_CD_7,");
            insertQuery.Append("MACHINE_CD_8,");
            insertQuery.Append("MACHINE_CD_9,");
            insertQuery.Append("MACHINE_CD_10,");
            insertQuery.Append("MACHINE_CD_11,");
            insertQuery.Append("MACHINE_CD_12,");//125
            insertQuery.Append("MACHINE_CD_13,");
            insertQuery.Append("PROD_POINT_1,");
            insertQuery.Append("PROD_POINT_2,");
            insertQuery.Append("PROD_POINT_3,");
            insertQuery.Append("PROD_POINT_4,");//130
            insertQuery.Append("PROD_POINT_5,");
            insertQuery.Append("PROD_POINT_6,");
            insertQuery.Append("PROD_POINT_7,");
            insertQuery.Append("PROD_POINT_8,");
            insertQuery.Append("PROD_POINT_9,");
            insertQuery.Append("PROD_POINT_10,");
            insertQuery.Append("PROD_POINT_11,");
            insertQuery.Append("PROD_POINT_12,");
            insertQuery.Append("PROD_POINT_13,");
            insertQuery.Append("PROD_POINT_SUB_1,");
            insertQuery.Append("PROD_POINT_SUB_2,");
            insertQuery.Append("PROD_POINT_SUB_3,");
            insertQuery.Append("PROD_POINT_SUB_4,");
            insertQuery.Append("PROD_POINT_SUB_5,");
            insertQuery.Append("PROD_POINT_SUB_6,");
            insertQuery.Append("PROD_POINT_SUB_7,");
            insertQuery.Append("PROD_POINT_SUB_8,");
            insertQuery.Append("PROD_POINT_SUB_9,");
            insertQuery.Append("PROD_POINT_SUB_10,");
            insertQuery.Append("PROD_POINT_SUB_11,");//150
            insertQuery.Append("PROD_POINT_SUB_12,");
            insertQuery.Append("PROD_POINT_SUB_13,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_1,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_2,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_3,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_4,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_5,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_6,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_7,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_8,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_9,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_10,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_11,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_12,");
            insertQuery.Append("PROCESS_COLOR_KUBUN_13,");
            insertQuery.Append("MATL_TYPE_CBK,");
            insertQuery.Append("MATL_NAME_MAK,");
            insertQuery.Append("MATL_SHAPE,");
            insertQuery.Append("MATL_DIM_D,");
            insertQuery.Append("MATL_DIM_W,");
            insertQuery.Append("MATL_DIM_OTHER,");
            insertQuery.Append("MATL_LENGTH,");
            insertQuery.Append("KAKOUIARI_QTY,");
            insertQuery.Append("DELIVERY_DATE,");
            insertQuery.Append("NO_OF_UNITS,");//175
            insertQuery.Append("UNIT,");
            insertQuery.Append("ACT_ALLOC_QTY,");
            insertQuery.Append("ACT_ALLOC_DATE,");
            insertQuery.Append("CHARGE,");
            insertQuery.Append("PROCESS_PLAN_REMARK,");
            insertQuery.Append("PROCESS_PLAN_KUBUN,");
            insertQuery.Append("PROD_MNGMNT_KUBUN,");
            insertQuery.Append("SPCL_MNGMNT_KUBUN,");
            insertQuery.Append("STORAGE_MNGMNT_KUBUN,");
            insertQuery.Append("PROCESS_PLAN_PRINT_DATE,");
            insertQuery.Append("PROCESS_PLAN_CMPL_DATE,");
            insertQuery.Append("USER_ID,");
            insertQuery.Append("INPUT_LOCATION_CD,");
            insertQuery.Append("CREATED_DATE,");
            insertQuery.Append("UPDATED_DATE, ");//190
            insertQuery.Append("PROCESS_PLAN_START_DATE,");
            insertQuery.Append("FIRST_PROCESS_CD, ");
            insertQuery.Append("EMPLOYEE_CD, ");
            insertQuery.Append("CHUUBAN_YEAR, ");
            insertQuery.Append("MAKER_CD, ");
            insertQuery.Append("KAKOUIRAI_REMARK ");
            insertQuery.Append(")");
            insertQuery.Append("VALUES( ");
            insertQuery.Append(" PROCESS_PLAN_SEQ.NEXTVAL,");
            insertQuery.Append("'" + "" + "',");//9999999998 product_cd
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");//5
            insertQuery.Append("'" + processPlanDetails.CUSTOMER_CD + "',");
            insertQuery.Append("'" + processPlanDetails.RECEIVER_CD + "',");
            insertQuery.Append("'" + processPlanDetails.INTERNAL_NAME_CD + "',");
            insertQuery.Append("'" + processPlanDetails.DEPARTMENT_GROUP_CD + "',");
            insertQuery.Append("'" + "" + "',");//10
            insertQuery.Append("(SELECT CONDITION_CD FROM MST_CONDITION WHERE CONDITION_DESCRIPTION = '" + processPlanDetails.PROD_CONDITION_STRING + "'),");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + processPlanDetails.HOST_REFERENCE_NO + "',"); //NYUKA_CD
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("(SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + processPlanDetails.USER_ID + "),");
            insertQuery.Append("'" + processPlanDetails.PACKING_CD + "',");
            insertQuery.Append("'" + processPlanDetails.CHUUBAN + "',");
            insertQuery.Append("'" + processPlanDetails.GYOUBAN + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + processPlanDetails.PROD_SPEC_NO1 + "-" + processPlanDetails.PROD_SPEC_NO2 + "',");
            insertQuery.Append("'" + processPlanDetails.PROD_SPEC_NO1 + "',");
            insertQuery.Append("'" + processPlanDetails.PROD_SPEC_NO2 + "',");
            insertQuery.Append("'" + processPlanDetails.VER + "',");
            insertQuery.Append("'" + processPlanDetails.STD_DEL_PERIOD + "',");
            insertQuery.Append("'" + processPlanDetails.PROD_NAME_CUST + "',");//25
            insertQuery.Append("'" + processPlanDetails.SHAPE + "',");
            insertQuery.Append("'" + processPlanDetails.PROD_DIM_D + "',");
            insertQuery.Append("'" + "" + "',");
            if (processPlanDetails.PROD_DIM_OTHER_STRING.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_DIM_OTHER_STRING + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + processPlanDetails.DIM_TOL_MIN + "',");
            insertQuery.Append("'" + processPlanDetails.DIM_TOL_MAX + "',");
            insertQuery.Append("'" + processPlanDetails.DIM_TOL_MIN_CHR + "',");
            insertQuery.Append("'" + processPlanDetails.DIM_TOL_MAX_CHR + "',");
            //if (processPlanDetails.DIM_TOL_MIN_CHR.Trim() != string.Empty)
            //{
            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MIN_CHR + "" + processPlanDetails.DIM_TOL_MIN + "',");
            //}
            //else
            //{

            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MIN + "',");
            //}
            //if (processPlanDetails.DIM_TOL_MAX_CHR.Trim() != string.Empty)
            //{

            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MAX_CHR + "" + processPlanDetails.DIM_TOL_MAX + "',");
            //}
            //else
            //{
            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MAX + "',");
            //}

            //if (processPlanDetails.DIM_TOL_MIN_CHR.Trim() != string.Empty)
            //{
            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MIN_CHR + "" + processPlanDetails.DIM_TOL_MIN + "',");

            //}
            //else
            //{
            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MIN + "',");
            //}
            //if (processPlanDetails.DIM_TOL_MAX_CHR.Trim() != string.Empty)
            //{
            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MAX_CHR + "" + processPlanDetails.DIM_TOL_MAX + "',");
            //}
            //else
            //{
            //    insertQuery.Append("'" + processPlanDetails.DIM_TOL_MAX + "',");
            //}

            if (processPlanDetails.PROD_LENGTH != null)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_LENGTH + "',");//34
            }
            else
            {
                insertQuery.Append("'" + "" + "',");//34
            }
            if (processPlanDetails.LENGTH_OTHER != null)
            {
                insertQuery.Append("'" + processPlanDetails.LENGTH_OTHER + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_LENGTH != null)
            {
                insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MIN + "',");
                insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MAX + "',");
                insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MIN_CHR + "',");
                insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MAX_CHR + "',");
                //if (processPlanDetails.LENGTH_TOL_MIN_CHR.Trim() != string.Empty)
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MIN_CHR + "" + processPlanDetails.LENGTH_TOL_MIN + "',");
                //}
                //else
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MIN + "',");
                //}
                //if (processPlanDetails.LENGTH_TOL_MAX_CHR.Trim() != string.Empty)
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MAX_CHR + "" + processPlanDetails.LENGTH_TOL_MAX + "',");
                //}
                //else
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MAX + "',");
                //}

                //if (processPlanDetails.LENGTH_TOL_MIN_CHR.Trim() != string.Empty)
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MIN_CHR + "" + processPlanDetails.LENGTH_TOL_MIN + "',");
                //}
                //else
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MIN + "',");
                //}
                //if (processPlanDetails.LENGTH_TOL_MAX_CHR.Trim() != string.Empty)
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MAX_CHR + "" + processPlanDetails.LENGTH_TOL_MAX + "',");
                //}
                //else
                //{
                //    insertQuery.Append("'" + processPlanDetails.LENGTH_TOL_MAX + "',");
                //}



            }
            else
            {
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.COIL_DIA_INT.Trim() != string.Empty)
            {
                insertQuery.Append("'" + Convert.ToInt32(processPlanDetails.COIL_DIA_INT) + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.COIL_DIA_EXT.Trim() != string.Empty)
            {
                insertQuery.Append("'" + Convert.ToInt32(processPlanDetails.COIL_DIA_EXT) + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.COIL_WND_DIR != "0")
            {
                insertQuery.Append("'" + Convert.ToInt32(processPlanDetails.COIL_WND_DIR) + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + processPlanDetails.CHAMFER + "',");//chamfering
            if (processPlanDetails.MAGARI_KUBUN != 0)
            {
                insertQuery.Append("'" + processPlanDetails.MAGARI_KUBUN + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.MAGARI_BEND_TOL_STRING != null)
            {
                insertQuery.Append("'" + Convert.ToSingle(processPlanDetails.MAGARI_BEND_TOL_STRING) + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.MAGARI_SPAN_STRING != null)
            {
                insertQuery.Append("'" + Convert.ToInt32(processPlanDetails.MAGARI_SPAN_STRING) + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");

            if (processPlanDetails.UNIT_WT.All(char.IsDigit))
            {
                insertQuery.Append("'" + 1 + "',");
                insertQuery.Append("'" + processPlanDetails.UNIT_WT + "',");//50
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }
            else if (processPlanDetails.UNIT_WT.Contains('-'))
            {
                string[] parts = processPlanDetails.UNIT_WT.Split('-');
                if (int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _))
                {
                    insertQuery.Append("'" + 2 + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + parts[0] + "',");
                    insertQuery.Append("'" + parts[1] + "',");
                }
                else if (processPlanDetails.UNIT_WT.Contains("ﾎﾝ"))
                {
                    //str = processPlanDetails.UNIT_WT;
                    //unitWtData3 = new string(str.Where(char.IsDigit).ToArray());
                    str = processPlanDetails.UNIT_WT;
                    unitWtData3 = str.Replace("ﾎﾝ", "").Trim();
                    insertQuery.Append("'" + 3 + "',");
                    insertQuery.Append("'" + unitWtData3 + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                }
                else
                {
                    insertQuery.Append("'" + 1 + "',");
                    insertQuery.Append("'" + processPlanDetails.UNIT_WT + "',");//50
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                }
            }
            else if (processPlanDetails.UNIT_WT.Contains("ﾎﾝ"))
            {
                //str = processPlanDetails.UNIT_WT;
                //unitWtData3 = new string(str.Where(char.IsDigit).ToArray());
                str = processPlanDetails.UNIT_WT;
                unitWtData3 = str.Replace("ﾎﾝ", "").Trim();
                insertQuery.Append("'" + 3 + "',");
                insertQuery.Append("'" + unitWtData3 + "',");//50
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }


            if (processPlanDetails.EXT_TEXTURE.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.EXT_TEXTURE + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.COLOR_ID != 0)
            {
                insertQuery.Append("'" + processPlanDetails.COLOR_NAME + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.DISPLAY.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.DISPLAY + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.STAMP.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.STAMP + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.TEST_SPECIMEN.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.TEST_SPECIMEN + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            commonFunctionToGetTensileYeildValue(processPlanDetails.TENSILE_STRN_PATTERN1_STRING);
            insertQuery.Append("'" + codeValue + "',");
            insertQuery.Append("'" + minMaxValue + "',");
            insertQuery.Append("'" + minValue + "',");
            insertQuery.Append("'" + maxValue + "',");
            insertQuery.Append("'" + actualValue + "',");

            getStrPatternElongationPressValue(processPlanDetails.ELONGATION_1, processPlanDetails.ELONGATION_2);
            insertQuery.Append("'" + codeValue + "',");
            insertQuery.Append("'" + minMaxValue + "',");
            insertQuery.Append("'" + minValue + "',");
            insertQuery.Append("'" + maxValue + "',");
            insertQuery.Append("'" + actualValue + "',");

            if (processPlanDetails.HARDNESS_1.Trim() != string.Empty)
            {
                if (processPlanDetails.HARDNESS_2.StartsWith("MIN"))
                {
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_1 + "',");
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_3 + "',");
                    insertQuery.Append("'" + 2 + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                }
                else if (processPlanDetails.HARDNESS_2.StartsWith("MAX"))
                {
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_1 + "',");
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_3 + "',");
                    insertQuery.Append("'" + 1 + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                }
                else if (char.IsDigit(processPlanDetails.HARDNESS_2[0]) == true)
                {
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_1 + "',");
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_2 + "',");
                    insertQuery.Append("'" + processPlanDetails.HARDNESS_3 + "',");
                    insertQuery.Append("'" + "" + "',");
                }
                else if (processPlanDetails.HARDNESS_2.StartsWith(_hardness2_Value))
                {
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + 1 + "',");
                }
                else
                {
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                    insertQuery.Append("'" + "" + "',");
                }

            }
            commonFunctionToGetTensileYeildValue(processPlanDetails.YEILD);
            insertQuery.Append("'" + codeValue + "',");
            insertQuery.Append("'" + minMaxValue + "',");
            insertQuery.Append("'" + minValue + "',");
            insertQuery.Append("'" + maxValue + "',");
            insertQuery.Append("'" + actualValue + "',");

            getStrPatternElongationPressValue(processPlanDetails.PRESS_1, processPlanDetails.PRESS_2);
            insertQuery.Append("'" + codeValue + "',");
            insertQuery.Append("'" + minMaxValue + "',");
            insertQuery.Append("'" + minValue + "',");
            insertQuery.Append("'" + maxValue + "',");
            insertQuery.Append("'" + actualValue + "',");

            if (processPlanDetails.DECARB_1 == "DMF")
            {
                insertQuery.Append("'" + 1 + "',");
            }
            else if (processPlanDetails.DECARB_1 == "DMS")
            {
                insertQuery.Append("'" + 2 + "',");
            }
            else if (processPlanDetails.DECARB_1 == "DMT")
            {
                insertQuery.Append("'" + 3 + "',");
            }
            else if (processPlanDetails.DECARB_1.Trim() == "")
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.DECARB_2.Trim() != string.Empty)
            {
                str = processPlanDetails.DECARB_2;
                index = Convert.ToInt32(CommonFunctionForRemoveChar(str));
                decimal decarb2 = Convert.ToDecimal(str.Substring(index));
                insertQuery.Append("'" + decarb2 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.DECARB_3 == "DMF")
            {
                insertQuery.Append("'" + 1 + "',");
            }
            else if (processPlanDetails.DECARB_3 == "DMS")
            {
                insertQuery.Append("'" + 2 + "',");
            }
            else if (processPlanDetails.DECARB_3 == "DMT")
            {
                insertQuery.Append("'" + 3 + "',");
            }
            else if (processPlanDetails.DECARB_3.Trim() == "")
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.DECARB_4.Trim() != string.Empty)
            {
                {
                    str = processPlanDetails.DECARB_4;
                    index = Convert.ToInt32(CommonFunctionForRemoveChar(str));
                    decimal decarb4 = Convert.ToDecimal(str.Substring(index));
                    insertQuery.Append("'" + decarb4 + "',");
                }
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");//decarb3

            if (processPlanDetails.IMPACT_TEST_PATTERN == 1)
            {
                insertQuery.Append("'" + processPlanDetails.IMPACT_TEST_MAX + "',");
                insertQuery.Append("'" + 1 + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }
            else if (processPlanDetails.IMPACT_TEST_PATTERN == 2)
            {
                insertQuery.Append("'" + processPlanDetails.IMPACT_TEST_MIN + "',");
                insertQuery.Append("'" + 2 + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }
            else if (processPlanDetails.IMPACT_TEST_PATTERN == 3)
            {
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + processPlanDetails.IMPACT_TEST_MIN + "',");
                insertQuery.Append("'" + processPlanDetails.IMPACT_TEST_MAX + "',");
                insertQuery.Append("'" + "" + "',");
            }
            else if (processPlanDetails.IMPACT_TEST_PATTERN == 4)
            {
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + 1 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
                insertQuery.Append("'" + "" + "',");
            }

            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");//100

            CheckMissMatchData(processPlanDetails, HPOResult);
            //PROCESS_1_CD
            if (HPOResult.PROCESS_1_CD != null)
            {

                if (HPOResult.PROCESS_1_CD != processPlanDetails.PROCESS_1_CD)
                {
                    processPlanDetails.PROCESS_1_CD = HPOResult.PROCESS_1_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_1_CD = processPlanDetails.PROCESS_1_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_1_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_1_CD + "',");

            //PROCESS_2_CD
            if (HPOResult.PROCESS_2_CD != null)
            {

                if (HPOResult.PROCESS_2_CD != processPlanDetails.PROCESS_2_CD)
                {
                    processPlanDetails.PROCESS_2_CD = HPOResult.PROCESS_2_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_2_CD = processPlanDetails.PROCESS_2_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_2_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_2_CD + "',");

            //PROCESS_3_CD
            if (HPOResult.PROCESS_3_CD != null)
            {

                if (HPOResult.PROCESS_3_CD != processPlanDetails.PROCESS_3_CD)
                {
                    processPlanDetails.PROCESS_3_CD = HPOResult.PROCESS_3_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_3_CD = processPlanDetails.PROCESS_3_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_3_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_3_CD + "',");

            //PROCESS_4_CD
            if (HPOResult.PROCESS_4_CD != null)
            {
                if (HPOResult.PROCESS_4_CD != processPlanDetails.PROCESS_4_CD)
                {
                    processPlanDetails.PROCESS_4_CD = HPOResult.PROCESS_4_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_4_CD = processPlanDetails.PROCESS_4_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_4_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_4_CD + "',");

            //PROCESS_5_CD
            if (HPOResult.PROCESS_5_CD != null)
            {
                if (HPOResult.PROCESS_5_CD != processPlanDetails.PROCESS_5_CD)
                {
                    processPlanDetails.PROCESS_5_CD = HPOResult.PROCESS_5_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_5_CD = processPlanDetails.PROCESS_5_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_5_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_5_CD + "',");

            //PROCESS_6_CD
            if (HPOResult.PROCESS_6_CD != null)
            {
                if (HPOResult.PROCESS_6_CD != processPlanDetails.PROCESS_6_CD)
                {
                    processPlanDetails.PROCESS_6_CD = HPOResult.PROCESS_6_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_6_CD = processPlanDetails.PROCESS_6_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_6_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_6_CD + "',");

            //PROCESS_7_CD
            if (HPOResult.PROCESS_7_CD != null)
            {
                if (HPOResult.PROCESS_7_CD != processPlanDetails.PROCESS_7_CD)
                {
                    processPlanDetails.PROCESS_7_CD = HPOResult.PROCESS_7_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_7_CD = processPlanDetails.PROCESS_7_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_7_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_7_CD + "',");

            //PROCESS_8_CD
            if (HPOResult.PROCESS_8_CD != null)
            {
                if (HPOResult.PROCESS_8_CD != processPlanDetails.PROCESS_8_CD)
                {
                    processPlanDetails.PROCESS_8_CD = HPOResult.PROCESS_8_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_8_CD = processPlanDetails.PROCESS_8_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_8_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_8_CD + "',");

            //PROCESS_9_CD
            if (HPOResult.PROCESS_9_CD != null)
            {
                if (HPOResult.PROCESS_9_CD != processPlanDetails.PROCESS_9_CD)
                {
                    processPlanDetails.PROCESS_9_CD = HPOResult.PROCESS_9_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_9_CD = processPlanDetails.PROCESS_9_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_9_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_9_CD + "',");

            //PROCESS_10_CD
            if (HPOResult.PROCESS_10_CD != null)
            {
                if (HPOResult.PROCESS_10_CD != processPlanDetails.PROCESS_10_CD)
                {
                    processPlanDetails.PROCESS_10_CD = HPOResult.PROCESS_10_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_10_CD = processPlanDetails.PROCESS_10_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_10_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_10_CD + "',");

            //PROCESS_11_CD
            if (HPOResult.PROCESS_11_CD != null)
            {
                if (HPOResult.PROCESS_11_CD != processPlanDetails.PROCESS_11_CD)
                {
                    processPlanDetails.PROCESS_11_CD = HPOResult.PROCESS_11_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_11_CD = processPlanDetails.PROCESS_11_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_11_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_11_CD + "',");

            //PROCESS_12_CD
            if (HPOResult.PROCESS_12_CD != null)
            {
                if (HPOResult.PROCESS_12_CD != processPlanDetails.PROCESS_12_CD)
                {
                    processPlanDetails.PROCESS_12_CD = HPOResult.PROCESS_12_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_12_CD = processPlanDetails.PROCESS_12_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_12_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_12_CD + "',");

            //PROCESS_13_CD
            if (HPOResult.PROCESS_13_CD != null)
            {
                if (HPOResult.PROCESS_13_CD != processPlanDetails.PROCESS_13_CD)
                {
                    processPlanDetails.PROCESS_13_CD = HPOResult.PROCESS_13_CD;
                }
                else
                {
                    processPlanDetails.PROCESS_13_CD = processPlanDetails.PROCESS_13_CD;
                }
            }
            else
            {
                processPlanDetails.PROCESS_13_CD = null;
            }
            insertQuery.Append("'" + processPlanDetails.PROCESS_13_CD + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");// machinecd
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");//125
            insertQuery.Append("'" + "" + "',");
            if (processPlanDetails.PROD_POINT_1.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_1 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_2.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_2 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_3.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_3 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_4.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_4 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_5.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_5 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_6.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_6 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_7.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_7 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_8.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_8 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_9.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_9 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_10.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_10 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_11.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_11 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_12.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_12 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_13.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_13 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_1.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_1 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_2.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_2 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_3.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_3 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_4.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_4 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_5.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_5 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_6.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_6 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_7.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_7 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_8.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_8 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_9.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_9 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_10.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_10 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_11.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_11 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_12.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_12 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            if (processPlanDetails.PROD_POINT_13.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.PROD_POINT_13 + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + processPlanDetails.MATL_NAME_MAK + "',");
            if (processPlanDetails.MATL_SHAPE.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.MATL_SHAPE + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + processPlanDetails.HOST_MATL_DIM_D + "',");
            insertQuery.Append("'" + "" + "',");
            if (processPlanDetails.MATL_DIM_OTHER.Trim() != string.Empty)
            {
                insertQuery.Append("'" + processPlanDetails.MATL_DIM_OTHER + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + processPlanDetails.KAKOUIARI_QTY + "',");//173 

            PrintDate = Convert.ToString(processPlanDetails.PRINT_DATE).Substring(0, 2);
            DateTime createdDate = Convert.ToDateTime(processPlanDetails.CREATED_DATE);
            var createdDateyear = createdDate.Year;
            var Created_Date_Year = Convert.ToString(createdDateyear).Substring(0, 2);

            if (PrintDate.Length >= 2)
            {
                Date_Year = Created_Date_Year + PrintDate;
            }
            var DeliveryDate = Convert.ToString(processPlanDetails.HOST_DELIVERY_DATE);
            if (DeliveryDate.Length < 4)
            {
                Del_Date = "0" + Convert.ToString(processPlanDetails.HOST_DELIVERY_DATE);
                Del_Date_Month = Del_Date.Substring(0, 2);
                Del_Date_Date = Del_Date.Substring(2, 2);
                Delivery_Date = Date_Year + "/" + Del_Date_Month + "/" + Del_Date_Date + " " + "00.00";
            }
            else
            {
                Del_Date = Convert.ToString(processPlanDetails.HOST_DELIVERY_DATE);
                Del_Date_Month = Del_Date.Substring(0, 2);
                Del_Date_Date = Del_Date.Substring(2, 2);
                Delivery_Date = Date_Year + "/" + Del_Date_Month + "/" + Del_Date_Date + " " + "00.00";
            }
            Print_Date_Month_date = Convert.ToString(processPlanDetails.PRINT_DATE).Substring(2, 4);
            Print_Date_Month = Print_Date_Month_date.Substring(0, 2);
            Print_Date_Date = Print_Date_Month_date.Substring(2, 2);
            Print_date = Date_Year + "/" + Print_Date_Month + "/" + Print_Date_Date + " " + "00.00";

            insertQuery.Append("TO_DATE('" + Delivery_Date + "', 'yyyy/mm/dd HH24:MI:SS'),");
            insertQuery.Append("'" + processPlanDetails.HPO_INZU + "',");//175
            if (processPlanDetails.UNIT != 0)
            {
                insertQuery.Append("'" + processPlanDetails.UNIT + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + processPlanDetails.ACT_ALLOC_QTY + "',");
            insertQuery.Append("TO_DATE('" + Convert.ToDateTime(processPlanDetails.ACT_ALLOC_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "', 'yyyy/mm/dd HH24:MI:SS'),");
            insertQuery.Append("'" + processPlanDetails.CHARGE + "',");
            insertQuery.Append("'" + "" + "',");
            if (processPlanDetails.PROCESS_PLAN_KUBUN != 0)
            {
                insertQuery.Append("'" + processPlanDetails.PROCESS_PLAN_KUBUN + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("'" + "" + "',");
            insertQuery.Append("TO_DATE('" + Print_date + "', 'yyyy/mm/dd HH24:MI:SS'),");
            insertQuery.Append("'" + "" + "',");
            if (processPlanDetails.USER_ID != 0)
            {
                insertQuery.Append("'" + processPlanDetails.USER_ID + "',");
            }
            else
            {
                insertQuery.Append("'" + "" + "',");
            }
            insertQuery.Append("(SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + processPlanDetails.USER_ID + ") ,");
            //insertQuery.Append(" TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd') ,");
            //insertQuery.Append("TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd'),");//190 
            //insertQuery.Append("TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd'), ");
            insertQuery.Append("SYSDATE,");
            insertQuery.Append("SYSDATE, ");
            insertQuery.Append("SYSDATE,");
            insertQuery.Append("'" + processPlanDetails.PROCESS_CD + "',");
            insertQuery.Append("'" + processPlanDetails.EMPLOYEE_CD + "',");
            if (Convert.ToString(processPlanDetails.CREATED_DATE) != null)
            {
                var chubbanYear = "";

                var created_Year = Convert.ToString(createdDateyear).Substring(3, 1);//for 2019
                var chubban_Year = processPlanDetails.CHHUBAN_YEAR;
                var Year = Convert.ToString(createdDateyear).Substring(2, 1);

                if (created_Year == "9" && chubban_Year == "0")
                {
                    int chubbanYearNew = Convert.ToInt32(Year) + 1; //year=2019+1,
                    chubbanYear = chubbanYearNew + chubban_Year;
                    insertQuery.Append("'" + chubbanYear + "',");
                }
                else
                {
                    chubbanYear = Year + chubban_Year;
                    insertQuery.Append("'" + chubbanYear + "',");
                }

            }
            insertQuery.Append("'" + processPlanDetails.MAKER_CD + "', ");
            insertQuery.Append("'" + processPlanDetails.KAKOUIRAI_REMARK + "' ");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());
            result = "success";
            if (result == "success")
            {
                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append(" SELECT  ");
                selectQuery.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD_HOST,");
                selectQuery.Append(" PP.CUSTOMER_CD,");
                selectQuery.Append(" PP.RECEIVER_CD,");
                selectQuery.Append(" PP.INTERNAL_NAME_CD,");
                selectQuery.Append(" PP.PROD_CONDITION_CD,");
                selectQuery.Append(" PP.DEPARTMENT_GROUP_CD,");
                selectQuery.Append(" PP.CHUUBAN,");
                selectQuery.Append(" LPAD(PP.GYOUBAN,2,0) AS GYOUBAN_HOST,");
                selectQuery.Append(" PP.CHARGE,");
                selectQuery.Append(" PP.PROD_NAME_CUST,");
                selectQuery.Append(" PP.PROD_SHAPE,");
                selectQuery.Append(" PP.PROD_DIM_D,");
                selectQuery.Append(" PP.PROD_DIM_W,");
                selectQuery.Append(" PP.PROD_DIM_OTHER AS PROD_DIM_OTHER_STRING,");
                selectQuery.Append(" PP.DIM_TOL_MIN,");
                selectQuery.Append(" PP.DIM_TOL_MAX,");
                selectQuery.Append(" PP.DIM_TOL_MIN_CHR,");
                selectQuery.Append(" PP.DIM_TOL_MAX_CHR,");
                selectQuery.Append(" PP.PROD_LENGTH,");
                selectQuery.Append(" PP.LENGTH_OTHER,");
                selectQuery.Append(" PP.LENGTH_TOL_MIN,");
                selectQuery.Append(" PP.LENGTH_TOL_MAX,");
                selectQuery.Append(" PP.LENGTH_TOL_MIN_CHR,");
                selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR,");
                selectQuery.Append(" PP.CHAMFER,");
                selectQuery.Append(" PP.CORNER_RADIUS,");
                selectQuery.Append(" PP.USER_ID,");
                selectQuery.Append(" PP.INPUT_LOCATION_CD,");
                selectQuery.Append(" PP.ACT_ALLOC_QTY,");
                selectQuery.Append(" PP.NO_OF_UNITS,");
                selectQuery.Append(" PP.UNIT,");
                selectQuery.Append(" PP.LOCATION_CD, ");
                selectQuery.Append(" PP.FIRST_PROCESS_CD AS NEXT_PROCESS_CD ");
                selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
                selectQuery.Append(" WHERE CHUUBAN = '" + processPlanDetails.CHUUBAN + "' AND GYOUBAN = '" + processPlanDetails.GYOUBAN + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
                var ProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                if (ProcessPlanData != null)
                {
                    List<GridSearchFields> nifudaDataList = new List<GridSearchFields>();
                    StringBuilder selectQueryForNifuda = new StringBuilder();
                    selectQueryForNifuda.Append(" SELECT TAG.NBNNO,NBNGYO FROM ");
                    selectQueryForNifuda.Append(" HSK_REPLICATION.HTMP_PRODUCT_ORDER HPO ");
                    selectQueryForNifuda.Append(" INNER JOIN HSK.TAG ON HSK.TAG.ORDERNO=HPO.CHUUBAN AND HSK.TAG.ORDERNOROWNO=HPO.GYOUBAN ");
                    selectQueryForNifuda.Append(" WHERE ");
                    selectQueryForNifuda.Append(" ORDERNO   = '" + ProcessPlanData.CHUUBAN + "' AND ORDERNOROWNO = '" + ProcessPlanData.GYOUBAN_HOST + "'  ");
                    nifudaDataList = ExecuteQueryMultiResult<GridSearchFields>(selectQueryForNifuda.ToString());
                    if (nifudaDataList.Count > 0)
                    {
                        for (int p = 0; p < nifudaDataList.Count; p++)
                        {
                            StringBuilder insertSql = new StringBuilder();
                            insertSql.Append("INSERT INTO TRN_NIFUDA(");
                            insertSql.Append("NIFUDA_CD,");
                            insertSql.Append("PROCESS_PLAN_CD,");
                            insertSql.Append("SHUKKA_INSTRUCTION_CD,");
                            insertSql.Append("SPOT_NYUKA_CD,");
                            insertSql.Append("NYUKA_CD,");
                            insertSql.Append("CUSTOMER_CD,");
                            insertSql.Append("RECEIVER_CD,");
                            insertSql.Append("INTERNAL_NAME_CD,");
                            insertSql.Append("PROD_CONDITION_CD,");
                            insertSql.Append("DEPARTMENT_GROUP_CD,");
                            insertSql.Append("NIFUDA_LOCATION_CD,");
                            insertSql.Append("NIFUDA_TYPE,");
                            insertSql.Append("NIFUDA_CLASSIFICATION,");
                            insertSql.Append("NIFUDA_NO,");
                            insertSql.Append("LAST_PROCESS_KUBUN,");
                            insertSql.Append("CHUUBAN,");
                            insertSql.Append("GYOUBAN,");
                            insertSql.Append("FI_TABA_NO,");
                            insertSql.Append("CUSTOMER_NAME_OR_SYOKUCHI,");
                            insertSql.Append("CHARGE,");
                            insertSql.Append("PROD_NAME_CUST,");
                            insertSql.Append("PROD_SHAPE,");
                            insertSql.Append("PROD_DIM_D,");
                            insertSql.Append("PROD_DIM_W,");
                            insertSql.Append("PROD_DIM_OTHER,");
                            insertSql.Append("DIM_TOL_MIN,");
                            insertSql.Append("DIM_TOL_MAX,");
                            insertSql.Append("DIM_TOL_MIN_CHR,");
                            insertSql.Append("DIM_TOL_MAX_CHR,");
                            insertSql.Append("PROD_LENGTH,");
                            insertSql.Append("LENGTH_OTHER,");
                            insertSql.Append("LENGTH_TOL_MIN,");
                            insertSql.Append("LENGTH_TOL_MAX,");
                            insertSql.Append("LENGTH_TOL_MIN_CHR,");
                            insertSql.Append("LENGTH_TOL_MAX_CHR,");
                            insertSql.Append("CHAMFER,");
                            insertSql.Append("CORNER_RADIUS,");
                            insertSql.Append("QUANTITY,");
                            insertSql.Append("NO_OF_UNITS,");
                            insertSql.Append("UNIT,");
                            insertSql.Append("STOCK_VERIFY_INSTRUCT_DATE,");
                            insertSql.Append("STOCK_VERI_QTY,");
                            insertSql.Append("STOCK_VERI_UNIT,");
                            insertSql.Append("STOCK_VERI_NO_OF_UNITS,");
                            insertSql.Append("STOCK_VERIFICATION_DATE,");
                            insertSql.Append("PROD_NIFUDA_GEN_DATE,");
                            insertSql.Append("SHUKKA_INST_NIFUDA_GEN_DATE,");
                            insertSql.Append("NIFUDA_REPRINT_DATE,");
                            insertSql.Append("INVOICE_GENERATED_FLAG,");
                            insertSql.Append("SPOT_KRW_NIFUDA_STICK_DATE,");
                            insertSql.Append("NIFUDA_COMPLETE_DATE,");
                            insertSql.Append("REMARKS,");
                            insertSql.Append("USER_ID,");
                            insertSql.Append("INPUT_LOCATION_CD,");
                            insertSql.Append("CREATED_DATE,");
                            insertSql.Append("UPDATED_DATE,");
                            insertSql.Append("ORIGIN_NIFUDA_CD,");
                            insertSql.Append("SHUKKA_INST_NIFUDA_SOURCE");
                            insertSql.Append(")");
                            insertSql.Append("VALUES( ");
                            insertSql.Append("TRN_NIFUDA_SEQ.NEXTVAL,");
                            insertSql.Append("'" + ProcessPlanData.PROCESS_PLAN_CD_HOST + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + ProcessPlanData.CUSTOMER_CD + "',");
                            insertSql.Append("'" + ProcessPlanData.RECEIVER_CD + "',");
                            insertSql.Append("'" + ProcessPlanData.INTERNAL_NAME_CD + "',");
                            //insertSql.Append("'" + "1" + "',");//internal name cd
                            insertSql.Append("'" + ProcessPlanData.PROD_CONDITION_CD + "',");
                            insertSql.Append("'" + ProcessPlanData.DEPARTMENT_GROUP_CD + "',");
                            insertSql.Append("'" + ProcessPlanData.INPUT_LOCATION_CD + "',");
                            insertSql.Append("'" + "1" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + nifudaDataList[p].NBNNO + "" + nifudaDataList[p].NBNGYO + "',");//14
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + ProcessPlanData.CHUUBAN + "',");
                            insertSql.Append("'" + ProcessPlanData.GYOUBAN_HOST + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + ProcessPlanData.CHARGE + "',");
                            insertSql.Append("'" + ProcessPlanData.PROD_NAME_CUST + "',");
                            insertSql.Append("'" + ProcessPlanData.PROD_SHAPE + "',");
                            insertSql.Append("'" + ProcessPlanData.PROD_DIM_D + "',");
                            insertSql.Append("'" + ProcessPlanData.PROD_DIM_W + "',");//24
                            insertSql.Append("'" + ProcessPlanData.PROD_DIM_OTHER_STRING + "',");
                            insertSql.Append("'" + ProcessPlanData.DIM_TOL_MIN + "',");
                            insertSql.Append("'" + ProcessPlanData.DIM_TOL_MAX + "',");
                            insertSql.Append("'" + ProcessPlanData.DIM_TOL_MIN_CHR + "',");
                            insertSql.Append("'" + ProcessPlanData.DIM_TOL_MAX_CHR + "',");
                            insertSql.Append("'" + ProcessPlanData.PROD_LENGTH + "',");
                            insertSql.Append("'" + ProcessPlanData.LENGTH_OTHER + "',");
                            insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MIN + "',");
                            insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MAX + "',");
                            insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MIN_CHR + "',");
                            insertSql.Append("'" + ProcessPlanData.LENGTH_TOL_MAX_CHR + "',");//35
                            insertSql.Append("'" + ProcessPlanData.CHAMFER + "',");
                            insertSql.Append("'" + ProcessPlanData.CORNER_RADIUS + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");//no of units
                            insertSql.Append("'" + "" + "',");//unit
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "0" + "',");//invoice generated flag
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "',");//52
                            insertSql.Append("'" + ProcessPlanData.USER_ID + "',");
                            insertSql.Append("'" + ProcessPlanData.INPUT_LOCATION_CD + "',");
                            //insertSql.Append(" TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd') ,");
                            //insertSql.Append("TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'yyyy/MM/dd'),");
                            insertSql.Append("SYSDATE,");
                            insertSql.Append("SYSDATE, ");
                            insertSql.Append("'" + "" + "',");
                            insertSql.Append("'" + "" + "'");
                            insertSql.Append(")");
                            ExecuteNonQuery(insertSql.ToString());
                        }
                    }


                    result = InsertIntoWipProcessPlan(Convert.ToInt32(ProcessPlanData.PROCESS_PLAN_CD_HOST), Convert.ToInt32(ProcessPlanData.LOCATION_CD), Convert.ToDecimal(ProcessPlanData.NO_OF_UNITS), Convert.ToInt32(ProcessPlanData.UNIT), Convert.ToInt32(ProcessPlanData.ACT_ALLOC_QTY), Convert.ToInt32(ProcessPlanData.NEXT_PROCESS_CD));

                    if (!string.IsNullOrEmpty(processPlanDetails.REMARK))
                    {
                        if (result == "Data Inserted Successfully")
                        {
                            StringBuilder insertMessage = new StringBuilder();
                            insertMessage.Append("  INSERT INTO TRN_MESSAGE ( ");
                            insertMessage.Append("  PROCESS_PLAN_CD ");
                            insertMessage.Append(", OPERATOR_CD ");
                            insertMessage.Append(", MSG_DATE ");
                            insertMessage.Append(", MSG_CONTENTS ");
                            insertMessage.Append(", INPUT_LOCATION_CD ");
                            insertMessage.Append(", USER_ID ");
                            insertMessage.Append(", CREATED_DATE ");
                            insertMessage.Append(", UPDATED_DATE ");

                            insertMessage.Append(") VALUES (");
                            //insertMessage.Append(" MESSAGE_SEQ.NEXTVAL,");
                            insertMessage.Append("'" + ProcessPlanData.PROCESS_PLAN_CD_HOST + "'");
                            insertMessage.Append(",'" + processPlanDetails.EMPLOYEE_CD + "'");
                            insertMessage.Append(", SYSDATE ");
                            insertMessage.Append(",'" + processPlanDetails.REMARK + "'");
                            insertMessage.Append(",'" + ProcessPlanData.INPUT_LOCATION_CD + "'");
                            insertMessage.Append(",'" + ProcessPlanData.USER_ID + "'");
                            insertMessage.Append(", SYSDATE ");
                            insertMessage.Append(", SYSDATE ");
                            insertMessage.Append(")");
                            ExecuteNonQuery(insertMessage.ToString());
                            result = "Data Inserted Successfully";
                        }
                    }


                }


            }
            return result;
        }


        public void CheckMissMatchData(GridSearchFields processPlanDetails, GridSearchFields HPOResult)
        {
            Boolean status = false;

            if (HPOResult.PROCESS_1_CD != processPlanDetails.PROCESS_1_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_2_CD != processPlanDetails.PROCESS_2_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_3_CD != processPlanDetails.PROCESS_3_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_4_CD != processPlanDetails.PROCESS_4_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_5_CD != processPlanDetails.PROCESS_5_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_6_CD != processPlanDetails.PROCESS_6_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_7_CD != processPlanDetails.PROCESS_7_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_8_CD != processPlanDetails.PROCESS_8_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_9_CD != processPlanDetails.PROCESS_9_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_10_CD != processPlanDetails.PROCESS_10_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_11_CD != processPlanDetails.PROCESS_11_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_12_CD != processPlanDetails.PROCESS_12_CD)
            {
                status = true;
            }
            else if (HPOResult.PROCESS_13_CD != processPlanDetails.PROCESS_13_CD)
            {
                status = true;
            }
            if (status == true)
            {
                InsertMismatchData(processPlanDetails, HPOResult);
            }
        }
        public void InsertMismatchData(GridSearchFields processPlanDetails, GridSearchFields HPOResult)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO TRN_PROCESS_PLAN_TEMP ( ");
            insertQuery.Append("  PROCESS_PLAN_CD_TEMP ");
            insertQuery.Append(", CHUUBAN ");
            insertQuery.Append(", GYOUBAN ");

            insertQuery.Append(", TRN_PROCESS_PLAN_1_CD ");
            insertQuery.Append(", TRN_PROCESS_PLAN_2_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_3_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_4_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_5_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_6_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_7_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_8_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_9_CD");
            insertQuery.Append(", TRN_PROCESS_PLAN_10_CD ");
            insertQuery.Append(", TRN_PROCESS_PLAN_11_CD ");
            insertQuery.Append(", TRN_PROCESS_PLAN_12_CD ");
            insertQuery.Append(", TRN_PROCESS_PLAN_13_CD ");

            insertQuery.Append(", HPO_PROCESS_1_CD ");
            insertQuery.Append(", HPO_PROCESS_2_CD ");
            insertQuery.Append(", HPO_PROCESS_3_CD ");
            insertQuery.Append(", HPO_PROCESS_4_CD ");
            insertQuery.Append(", HPO_PROCESS_5_CD ");
            insertQuery.Append(", HPO_PROCESS_6_CD ");
            insertQuery.Append(", HPO_PROCESS_7_CD ");
            insertQuery.Append(", HPO_PROCESS_8_CD ");
            insertQuery.Append(", HPO_PROCESS_9_CD ");
            insertQuery.Append(", HPO_PROCESS_10_CD ");
            insertQuery.Append(", HPO_PROCESS_11_CD ");
            insertQuery.Append(", HPO_PROCESS_12_CD ");
            insertQuery.Append(", HPO_PROCESS_13_CD ");

            insertQuery.Append(", CHUUBAN_YEAR ");
            insertQuery.Append(", CREATED_DATE ");

            insertQuery.Append(") VALUES (");
            insertQuery.Append("  TRN_PROCESS_PLAN_TEMP_SEQ.NEXTVAL ");
            insertQuery.Append(",'" + processPlanDetails.CHUUBAN + "'");
            insertQuery.Append(",'" + processPlanDetails.GYOUBAN + "'");

            insertQuery.Append(",'" + processPlanDetails.PROCESS_1_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_2_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_3_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_4_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_5_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_6_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_7_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_8_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_9_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_10_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_11_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_12_CD + "'");
            insertQuery.Append(",'" + processPlanDetails.PROCESS_13_CD + "'");

            insertQuery.Append(",'" + HPOResult.PROCESS_1_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_2_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_3_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_4_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_5_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_6_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_7_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_8_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_9_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_10_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_11_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_12_CD + "'");
            insertQuery.Append(",'" + HPOResult.PROCESS_13_CD + "'");

            insertQuery.Append(",'" + processPlanDetails.CHHUBAN_YEAR + "'");
            insertQuery.Append(", SYSDATE ");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/01/04       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Save data into table TRN_WIP_STATUS
        /// </returns>
        /// 

        public string InsertIntoWipProcessPlan(int processPlanCD, int locationCD, decimal noOfUnits, int unit, int actAllocQty, int processCD)
        {
            StringBuilder insertSql = new StringBuilder();
            insertSql.Append("INSERT INTO TRN_WIP_STATUS(");
            insertSql.Append("WIP_STATUS_CD,");
            insertSql.Append("PROCESS_PLAN_CD,");
            insertSql.Append("LOCATION_CD,");
            insertSql.Append("TABA_NO,");
            insertSql.Append("WIP_NO_OF_UNITS,");
            insertSql.Append("UNIT,");
            insertSql.Append("WIP_QTY,");
            insertSql.Append("NEXT_PROCESS");
            insertSql.Append(")");
            insertSql.Append("VALUES( ");
            insertSql.Append("TRN_WIP_STATUS_SEQ.NEXTVAL,");
            insertSql.Append("'" + processPlanCD + "',");
            insertSql.Append("'" + locationCD + "',");
            insertSql.Append("'" + 1 + "',");
            insertSql.Append("'" + noOfUnits + "',");
            insertSql.Append("'" + unit + "',");
            insertSql.Append("'" + actAllocQty + "',");
            insertSql.Append("'" + processCD + "'");
            insertSql.Append(")");
            ExecuteNonQuery(insertSql.ToString());
            result = "Data Inserted Successfully";
            return result;

        }

        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/11/01       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Delete data from table TRN_PROCESS_PLAN 
        /// Delete data from table TRN_WIP_STATUS
        /// Delete data from table TRN_NIFUDA
        /// </returns>
        /// 
        public List<GridSearchFields> DeleteProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)

        {
            int count = processPlanDetails.Count;
            List<GridSearchFields> result = new List<GridSearchFields>();
            using (TransactionScope transaction = new TransactionScope())
            {
                // try{
                for (int i = 0; i < count; i++)
                {
                    string chuban_Gyouban_String = processPlanDetails[i].CHUUBAN_GYOUBAN_STRING;
                    int processPlanCd = Convert.ToInt32(processPlanDetails[i].PROCESS_PLAN_CD);
                    bool started = CheckProcessStartStatus(processPlanCd);
                    if (started == true)
                    {
                        GridSearchFields message = new GridSearchFields()
                        { CHUUBAN_GYOUBAN_STRING = chuban_Gyouban_String, RESPONCE = "Process Started" };
                        result.Add(message);
                    }
                    else
                    {

                        var responce = DeleteProcessPlanDetails(processPlanCd);
                        if (responce.Contains("ORA-02292"))
                        {
                            GridSearchFields message = new GridSearchFields()
                            { CHUUBAN_GYOUBAN_STRING = chuban_Gyouban_String, RESPONCE = "Process Started" };
                            result.Add(message);
                        }
                        else
                        {
                            GridSearchFields message = new GridSearchFields()
                            { CHUUBAN_GYOUBAN_STRING = chuban_Gyouban_String, RESPONCE = responce };
                            result.Add(message);
                        }


                    }

                }
                transaction.Complete();
            }

            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/10/30       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Check ProcessCd count into base table
        /// </returns>
        /// 
        private bool CheckProcessStartStatus(int processPlanCd)
        {
            int count = 0;
            StringBuilder selectQuery = new StringBuilder();
            //selectQuery.Append("SELECT COUNT(*) FROM TRN_WIP_STATUS ");
            //selectQuery.Append(" WHERE ");
            //selectQuery.Append(" PROCESS_PLAN_CD = '" + processPlanCd + "' AND PROCESS_START_DATE IS NOT NULL ");
            //count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            //if (count > 0)
            //    return true;
            //else
            //    return false;
            selectQuery.Append(" SELECT SUM(PROCESS_PLAN_COUNT)");
            selectQuery.Append(" FROM(");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) AS PROCESS_PLAN_COUNT FROM TRN_DRAWING WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_DRAWING_D WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_DRAWING_W WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_STAMPING WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_CHAMFERING WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_EC WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_STRAIGHTENING WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_STRAIGHTENING2 WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_HEAT_TREATMENT WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_INTERNAL_QUALITY WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_MI_TENSILE_STRENGTH WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_MI_HARDNESS WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_HARDENING_TEMPERING WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_FINAL_INSPECTION WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_GENERAL_DAILY_PROCESS WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_TABA_DIVISION WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_TABA_COMBINE WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_WIP_TRANSFER WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_WIP_TRANSFER_ACCEPTANCE WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" UNION ALL");
            selectQuery.Append(" SELECT COUNT(PROCESS_PLAN_CD) FROM TRN_HT_QUALITY WHERE PROCESS_PLAN_CD = '" + processPlanCd + "'");
            selectQuery.Append(" )");
            count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
                return true;
            else
                return false;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2023/11/02       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Delete data from table TRN_PROCESS_PLAN 
        /// Delete data from table TRN_WIP_STATUS
        /// Delete data from table TRN_NIFUDA
        /// </returns>
        /// 
        public string DeleteProcessPlanDetails(int processPlanCd)
        {
            StringBuilder deleteSqlForMsg = new StringBuilder();
            deleteSqlForMsg.Append("DELETE  FROM TRN_MESSAGE ");
            deleteSqlForMsg.Append("WHERE ");
            deleteSqlForMsg.Append("PROCESS_PLAN_CD = '" + processPlanCd + "' ");
            deleteSqlForMsg.Append("AND MACHINE_CD IS NULL ");
            deleteSqlForMsg.Append("AND PROCESS_KUBUN_ID IS NULL ");
            //deleteSqlForMsg.Append("AND OPERATOR_CD IS NULL "); 
            ExecuteNonQuery(deleteSqlForMsg.ToString());


            StringBuilder deleteSqlForWip = new StringBuilder();
            deleteSqlForWip.Append("DELETE  FROM TRN_WIP_STATUS ");
            deleteSqlForWip.Append("WHERE ");
            deleteSqlForWip.Append("PROCESS_PLAN_CD = '" + processPlanCd + "'");
            deleteSqlForWip.Append("  AND PROCESS_START_DATE IS NULL");
            ExecuteNonQuery(deleteSqlForWip.ToString());

            StringBuilder deleteSqlForNifuda = new StringBuilder();
            deleteSqlForNifuda.Append("DELETE  FROM  TRN_NIFUDA  ");
            deleteSqlForNifuda.Append("WHERE ");
            deleteSqlForNifuda.Append("PROCESS_PLAN_CD ='" + processPlanCd + "' ");

            ExecuteNonQuery(deleteSqlForNifuda.ToString());



            try
            {
                StringBuilder deleteSqlForProcessPlan = new StringBuilder();
                deleteSqlForProcessPlan.Append("DELETE  FROM  TRN_PROCESS_PLAN  ");
                deleteSqlForProcessPlan.Append("WHERE ");
                deleteSqlForProcessPlan.Append("PROCESS_PLAN_CD ='" + processPlanCd + "' AND PROCESS_PLAN_CMPL_DATE IS NULL ");
                ExecuteNonQuery(deleteSqlForProcessPlan.ToString());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            result = "Data Deleted Successfully";
            return result;
        }

        public int CommonFunctionForRemoveChar(string str)
        {
            int value = str.IndexOfAny(new char[] { '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            return value;
        }
        public void commonFunctionToGetTensileYeildValue(string strPattern)
        {
            isStrPatternStartWithNum = false;
            isStrPatternStartWithNum = char.IsDigit(strPattern[0]);
            string[] data;
            codeValue = null;
            minMaxValue = "";
            minValue = "";
            maxValue = "";
            actualValue = "";
            if (strPattern.Trim() != string.Empty)
            {
                if (strPattern.StartsWith("MIN"))
                {
                    str = strPattern;
                    index = Convert.ToInt32(CommonFunctionForRemoveChar(str));
                    codeValue = Convert.ToDecimal(str.Substring(index));
                    minMaxValue = "2";
                    minValue = "";
                    maxValue = "";
                    actualValue = "";
                }
                else if (strPattern.StartsWith("MAX"))
                {
                    str = strPattern;
                    index = Convert.ToInt32(CommonFunctionForRemoveChar(str));
                    codeValue = Convert.ToDecimal(str.Substring(index));
                    minMaxValue = "1";
                    minValue = "";
                    maxValue = "";
                    actualValue = "";
                }
                else if (isStrPatternStartWithNum == true)
                {

                    data = strPattern.Split("-");
                    codeValue = null;
                    minMaxValue = "";
                    minValue = data[0];
                    maxValue = data[1];
                    actualValue = "";
                }
                else if (strPattern == _tensile_yeild_Value)
                {
                    codeValue = null;
                    minMaxValue = "";
                    minValue = "";
                    maxValue = "";
                    actualValue = "1";
                }
            }
            else
            {
                codeValue = null;
                minMaxValue = "";
                minValue = "";
                maxValue = "";
                actualValue = "";
            }
        }
        public void getStrPatternElongationPressValue(string strPattern1, string strPattern2)
        {
            codeValue = null;
            minMaxValue = "";
            minValue = "";
            maxValue = "";
            actualValue = "";
            isStrPatternStartWithNum = false;
            isStrPatternStartWithNum = char.IsDigit(strPattern1[0]);
            if (strPattern1.Trim() != string.Empty)
            {
                if (strPattern1.StartsWith("MIN"))
                {
                    codeValue = Convert.ToDecimal(strPattern2);
                    minMaxValue = "2";
                    minValue = "";
                    maxValue = "";
                    actualValue = "";

                }
                else if (strPattern1.StartsWith("MAX"))
                {
                    codeValue = Convert.ToDecimal(strPattern2);
                    minMaxValue = "1";
                    minValue = "";
                    maxValue = "";
                    actualValue = "";
                }
                else if (isStrPatternStartWithNum == true)
                {
                    if (strPattern2.StartsWith("UP"))
                    {
                        codeValue = Convert.ToDecimal(strPattern1);
                        minMaxValue = "2";//save min value from 01/12/2025 as per windows 
                        minValue = "";
                        maxValue = "";
                        actualValue = "";
                    }
                    else
                    {
                        codeValue = null;
                        minMaxValue = "";
                        minValue = strPattern1;
                        maxValue = strPattern2;
                        actualValue = "";
                    }

                }
                else if (strPattern1 == _elongation1_press1_Value)
                {
                    codeValue = null;
                    minMaxValue = "";
                    minValue = "";
                    maxValue = "";
                    actualValue = "1";
                }
            }
            else
            {
                codeValue = null;
                minMaxValue = "";
                minValue = "";
                maxValue = "";
                actualValue = "";
            }
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/06/18       
        /// used screens: ProcessPlanHandoverHost
        /// </summary>
        /// <returns>
        /// check master data into master table
        /// returns Internal_Name_Cd,Customer_Cd,Employee_Cd,Receiver_Cd,Process_Cd,Packing_Cd,Department_Group_Cd
        /// </returns>
        /// 
        public List<GridSearchFields> ValidateDataExist(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();
            List<GridSearchFields> chubbanData = new List<GridSearchFields>();
            // result = processPlanDetails;
            List<string> prodSpecNo1List = _Prod_Spec_No_1_Value.Split(",").ToList();
            string prodSpecNo1_Val1 = prodSpecNo1List[0];
            string prodSpecNo1_Val2 = prodSpecNo1List[1];
            int cust_CD;
            int customer_CD;
            int emp_CD;
            int employee_CD;
            int maker_CD;
            int matl_Int_CD;
            int internal_Name_CD;
            int department_Group_CD;
            short packing_CD;
            int mst_Receiver_CD;
            int receiver_CD;
            int prod_Spec_CD;
            //int prod_Spec_CD;
            //int prod_Spec_CD;
            int prod_Receiver_CD;
            string process_Name = "";
            int proc_CD1, proc_CD2, proc_CD3, proc_CD4, proc_CD5, proc_CD6, proc_CD7, proc_CD8, proc_CD9, proc_CD10, proc_CD11, proc_CD12, proc_CD13;
            int process_CD1, process_CD2, process_CD3, process_CD4, process_CD5, process_CD6, process_CD7, process_CD8, process_CD9, process_CD10, process_CD11, process_CD12, process_CD13;
            GridSearchFields dept_Details = new GridSearchFields();
            GridSearchFields prod_Ver_Details = new GridSearchFields();
            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                result.Add(processPlanDetails[i]);
                StringBuilder selectQuery1 = new StringBuilder();
                selectQuery1.Append("SELECT CUST_CD FROM HSKUSER.MST_CUSTOMER ");
                selectQuery1.Append("WHERE ");
                selectQuery1.Append("CUST_CD_HOST = '" + result[i].CUSTOMER_CODE.Trim() + "' ");
                cust_CD = Convert.ToInt32(ExecuteScalar(selectQuery1.ToString()));
                if (cust_CD > 0)
                {
                    StringBuilder selectQuery2 = new StringBuilder();
                    selectQuery2.Append("SELECT CUSTOMER_CD FROM MST_CUSTOMER ");
                    selectQuery2.Append("WHERE ");
                    selectQuery2.Append("CUSTOMER_CD = '" + cust_CD + "' ");
                    customer_CD = Convert.ToInt32(ExecuteScalar(selectQuery2.ToString()));
                    if (customer_CD > 0)
                    {
                        result[i].CUSTOMER_CD = customer_CD;
                    }
                    else
                    {
                        result[i].CUSTOMER_CD = null;
                        break;
                    }

                }
                else
                {
                    result[i].CUSTOMER_CD = null;
                    break;
                }


                StringBuilder selectQuery3 = new StringBuilder();
                selectQuery3.Append("SELECT EMP_CD FROM HSKUSER.MST_EMPLOYEE ");
                selectQuery3.Append("WHERE ");
                selectQuery3.Append("EMP_CD_HOST = '" + result[i].INCHARGE_CODE.Trim() + "' ");
                emp_CD = Convert.ToInt32(ExecuteScalar(selectQuery3.ToString()));
                if (emp_CD > 0)
                {
                    StringBuilder selectQuery4 = new StringBuilder();
                    selectQuery4.Append("SELECT EMPLOYEE_CD FROM MST_EMPLOYEE ");
                    selectQuery4.Append("WHERE ");
                    selectQuery4.Append("EMPLOYEE_CD = '" + emp_CD + "' ");
                    employee_CD = Convert.ToInt32(ExecuteScalar(selectQuery4.ToString()));
                    if (employee_CD > 0)
                    {
                        result[i].EMPLOYEE_CD = employee_CD;
                    }
                    else
                    {
                        result[i].EMPLOYEE_CD = null;
                        break;
                    }

                }
                else
                {
                    result[i].EMPLOYEE_CD = null;
                    break;
                }

                StringBuilder selectInternalCd = new StringBuilder();
                selectInternalCd.Append("SELECT INTERNAL_NAME_CD FROM MST_INTERNAL_NAME ");// code changed on 2025/12/23 as per client requirement
                selectInternalCd.Append("WHERE ");
                selectInternalCd.Append("MATL_NAME_INT = '" + result[i].MATL_NAME_INT.Trim() + "'  ");
                internal_Name_CD = Convert.ToInt32(ExecuteScalar(selectInternalCd.ToString()));
                if (internal_Name_CD > 0)
                {
                    result[i].INTERNAL_NAME_CD = internal_Name_CD;
                }
                else
                {
                    result[i].INTERNAL_NAME_CD = null;
                    result[i].MAKER_CODE = result[i].MAKER_CODE;
                    result[i].MAKER_NAME = result[i].MAKER_NAME;
                    result[i].MATL_NAME_INT = result[i].MATL_NAME_INT;
                    result[i].MATL_NAME_MAK = result[i].MATL_NAME_MAK;
                    break;
                }

                //StringBuilder selectInternalCdCnt = new StringBuilder();
                //selectInternalCdCnt.Append("SELECT COUNT(*) AS TOTALCOUNT FROM MST_INTERNAL_NAME ");// code changed on 2025/12/11 as per client requirement
                //selectInternalCdCnt.Append("WHERE ");
                //selectInternalCdCnt.Append("MATL_NAME_INT = '" + result[i].MATL_NAME_INT.Trim() + "'  ");
                //int Count = Convert.ToInt32(ExecuteScalar(selectInternalCdCnt.ToString()));
                //if (Count == 1)
                //{
                //    StringBuilder selectInternalCd = new StringBuilder();
                //    selectInternalCd.Append("SELECT INTERNAL_NAME_CD FROM MST_INTERNAL_NAME ");// code changed on 2025/12/11 as per client requirement
                //    selectInternalCd.Append("WHERE ");
                //    selectInternalCd.Append("MATL_NAME_INT = '" + result[i].MATL_NAME_INT.Trim() + "'  ");
                //    internal_Name_CD = Convert.ToInt32(ExecuteScalar(selectInternalCd.ToString()));
                //    result[i].INTERNAL_NAME_CD = internal_Name_CD;
                //}
                //else
                //{
                //    StringBuilder selectQuery44 = new StringBuilder();
                //    selectQuery44.Append("SELECT MAKER_CD FROM HSKUSER.MST_MAKER ");
                //    selectQuery44.Append("WHERE ");
                //    selectQuery44.Append("MAKER_CD_HOST = '" + result[i].MAKER_CODE.Trim() + "' ");
                //    maker_CD = Convert.ToInt32(ExecuteScalar(selectQuery44.ToString()));
                //    if (maker_CD > 0)
                //    {
                //        StringBuilder selectQuery45 = new StringBuilder();
                //        selectQuery45.Append(" SELECT  MATLINT.INTERNAL_NAME_CD FROM HSKUSER.MST_MATL_NAME_MAK MATLMAK ");
                //        selectQuery45.Append(" INNER JOIN HSKWEBUSER.MST_INTERNAL_NAME MATLINT ON MATLMAK.MATL_CD_INT = MATLINT.INTERNAL_NAME_CD ");
                //        selectQuery45.Append(" WHERE ");
                //        selectQuery45.Append("MATLMAK.MAKER_CD = '" + maker_CD + "' AND  MATLMAK.MATL_NAME_MAK = '" + result[i].MATL_NAME_MAK.Trim() + "' AND  MATLINT.MATL_NAME_INT = '" + result[i].MATL_NAME_INT.Trim() + "'  ");
                //        internal_Name_CD = Convert.ToInt32(ExecuteScalar(selectQuery45.ToString()));
                //        if (internal_Name_CD > 0)
                //        {
                //            result[i].INTERNAL_NAME_CD = internal_Name_CD;
                //        }
                //        else
                //        {
                //            result[i].INTERNAL_NAME_CD = null;
                //            result[i].MAKER_CODE = result[i].MAKER_CODE;
                //            result[i].MAKER_NAME = result[i].MAKER_NAME;
                //            result[i].MATL_NAME_INT = result[i].MATL_NAME_INT;
                //            result[i].MATL_NAME_MAK = result[i].MATL_NAME_MAK;
                //            break;
                //        }

                //    }
                //    else
                //    {
                //        result[i].INTERNAL_NAME_CD = null;
                //        result[i].MAKER_CODE = result[i].MAKER_CODE;
                //        result[i].MAKER_NAME = result[i].MAKER_NAME;
                //        result[i].MATL_NAME_INT = result[i].MATL_NAME_INT;
                //        result[i].MATL_NAME_MAK = result[i].MATL_NAME_MAK;
                //        break;
                //    }
                //}

                StringBuilder selectQuery8 = new StringBuilder();
                selectQuery8.Append("SELECT EMP_CD FROM HSKUSER.MST_EMPLOYEE ");
                selectQuery8.Append("WHERE ");
                selectQuery8.Append("EMP_CD_HOST = '" + result[i].INCHARGE_CODE.Trim() + "' ");
                emp_CD = Convert.ToInt32(ExecuteScalar(selectQuery8.ToString()));
                if (emp_CD > 0)
                {
                    StringBuilder selectQuery9 = new StringBuilder();
                    selectQuery9.Append("SELECT LOC_CD AS LOCATION_CD,DEPT_CD AS DEPARTMENT_CD FROM HSKUSER.MST_DEPT ");
                    selectQuery9.Append("WHERE ");
                    selectQuery9.Append("DEPT_CD_HOST = '" + result[i].DEPT_CODE.Trim() + "' ");
                    dept_Details = ExecuteQuerySingleResult<GridSearchFields>(selectQuery9.ToString());
                    if (dept_Details != null)
                    {
                        StringBuilder selectQuery10 = new StringBuilder();
                        selectQuery10.Append("SELECT DEPARTMENT_GROUP_CD FROM MST_DEPARTMENT_GROUP ");
                        selectQuery10.Append("WHERE ");
                        selectQuery10.Append("EMPLOYEE_CD = '" + emp_CD + "' AND LOCATION_CD ='" + dept_Details.LOCATION_CD + "' AND DEPARTMENT_CD='" + dept_Details.DEPARTMENT_CD + "'");
                        department_Group_CD = Convert.ToInt32(ExecuteScalar(selectQuery10.ToString()));
                        if (department_Group_CD > 0)
                        {
                            result[i].DEPARTMENT_GROUP_CD = department_Group_CD;
                        }
                        else
                        {
                            result[i].DEPARTMENT_GROUP_CD = null;
                            break;
                        }
                    }
                    else
                    {
                        result[i].DEPARTMENT_GROUP_CD = null;
                        break;
                    }

                }
                else
                {
                    result[i].DEPARTMENT_GROUP_CD = null;
                    break;
                }







                if (result[i].PACKING_NAME.Trim() != string.Empty)
                {
                    StringBuilder selectQuery11 = new StringBuilder();
                    selectQuery11.Append("SELECT PACKING_CD FROM MST_PACKING ");
                    selectQuery11.Append("WHERE ");
                    selectQuery11.Append("PACKING_NAME = '" + result[i].PACKING_NAME.Trim() + "' ");
                    packing_CD = Convert.ToInt16(ExecuteScalar(selectQuery11.ToString()));
                    if (packing_CD > 0)
                    {
                        result[i].PACKING_CD = packing_CD;
                    }
                    else
                    {
                        result[i].PACKING_CD = null;
                    }
                }


                if (result[i].RECEIVER_NAME.Trim() != string.Empty)
                {
                    StringBuilder selectQuery12 = new StringBuilder();
                    selectQuery12.Append("SELECT RECEIVER_CD FROM HSKUSER.MST_RECEIVER ");
                    selectQuery12.Append("WHERE ");
                    selectQuery12.Append("SHORT_NAME = '" + result[i].RECEIVER_NAME.Trim() + "' ");
                    mst_Receiver_CD = Convert.ToInt32(ExecuteScalar(selectQuery12.ToString()));
                    if (mst_Receiver_CD == 0)
                    {
                        StringBuilder selectQuery13 = new StringBuilder();
                        selectQuery13.Append("SELECT PROD_SPEC_CD FROM HSKUSER.MST_PROD_SPEC ");
                        selectQuery13.Append("WHERE ");
                        selectQuery13.Append("PROD_SPEC_NO_1 = '" + result[i].PROD_SPEC_NO1.Trim() + "' AND PROD_SPEC_NO_2 = '" + result[i].PROD_SPEC_NO2.Trim() + "' ");
                        prod_Spec_CD = Convert.ToInt32(ExecuteScalar(selectQuery13.ToString()));
                        if (prod_Spec_CD > 0)
                        {
                            StringBuilder selectQuery14 = new StringBuilder();
                            selectQuery14.Append("SELECT RECEIVER_CD FROM HSKUSER.MST_PROD_VER ");
                            selectQuery14.Append("WHERE ");
                            selectQuery14.Append("PROD_SPEC_CD = '" + prod_Spec_CD + "' ");
                            selectQuery14.Append("ORDER BY VER DESC ");//changed created date to ver on 10/12/2025 as per client requirment(Nilesh sir)
                            selectQuery14.Append("FETCH FIRST 1 ROWS ONLY ");
                            prod_Receiver_CD = Convert.ToInt32(ExecuteScalar(selectQuery14.ToString()));
                            if (prod_Receiver_CD > 0)
                            {
                                StringBuilder selectQuery15 = new StringBuilder();
                                selectQuery15.Append("SELECT RECEIVER_CD FROM MST_RECEIVER ");
                                selectQuery15.Append("WHERE ");
                                selectQuery15.Append("RECEIVER_CD = '" + prod_Receiver_CD + "' ");
                                receiver_CD = Convert.ToInt16(ExecuteScalar(selectQuery15.ToString()));
                                if (receiver_CD > 0)
                                {
                                    result[i].RECEIVER_CD = receiver_CD;
                                }
                                else
                                {
                                    result[i].RECEIVER_CD = null;
                                    break;
                                }
                            }
                            else
                            {
                                result[i].RECEIVER_CD = null;
                                break;
                            }
                        }
                        else
                        {
                            result[i].RECEIVER_CD = null;
                            break;
                        }

                    }
                    else
                    {
                        StringBuilder selectQuery16 = new StringBuilder();
                        selectQuery16.Append("SELECT RECEIVER_CD FROM MST_RECEIVER ");
                        selectQuery16.Append("WHERE ");
                        selectQuery16.Append("RECEIVER_CD = '" + mst_Receiver_CD + "' ");
                        receiver_CD = Convert.ToInt16(ExecuteScalar(selectQuery16.ToString()));
                        if (receiver_CD > 0)
                        {
                            result[i].RECEIVER_CD = receiver_CD;
                        }
                        else
                        {
                            result[i].RECEIVER_CD = null;
                            break;
                        }
                    }
                }
                else
                {
                    result[i].RECEIVER_CD = 0;
                }


                if (result[i].PROCESS_NAME1.Trim() != string.Empty)
                {

                    if (result[i].PROCESS_NAME1.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME1.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME1;
                    }
                    StringBuilder selectQuery17 = new StringBuilder();
                    selectQuery17.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery17.Append("WHERE ");
                    selectQuery17.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD1 = Convert.ToInt16(ExecuteScalar(selectQuery17.ToString()));
                    if (proc_CD1 > 0)
                    {
                        StringBuilder selectQuery18 = new StringBuilder();
                        selectQuery18.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery18.Append("WHERE ");
                        selectQuery18.Append("PROCESS_CD = '" + proc_CD1 + "'");
                        process_CD1 = Convert.ToInt16(ExecuteScalar(selectQuery18.ToString()));
                        if (process_CD1 > 0)
                        {
                            result[i].PROCESS_1_CD = process_CD1;
                        }
                        else
                        {
                            result[i].PROCESS_1_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME1;
                            break;
                            //add process name if not found
                        }
                    }
                    else
                    {
                        result[i].PROCESS_1_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME1;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_1_CD = 0;
                }
                if (result[i].PROCESS_NAME2.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME2.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME2.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME2;
                    }
                    StringBuilder selectQuery19 = new StringBuilder();
                    selectQuery19.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery19.Append("WHERE ");
                    selectQuery19.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD2 = Convert.ToInt16(ExecuteScalar(selectQuery19.ToString()));
                    if (proc_CD2 > 0)
                    {
                        StringBuilder selectQuery20 = new StringBuilder();
                        selectQuery20.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery20.Append("WHERE ");
                        selectQuery20.Append("PROCESS_CD = '" + proc_CD2 + "'");
                        process_CD2 = Convert.ToInt16(ExecuteScalar(selectQuery20.ToString()));
                        if (process_CD2 > 0)
                        {
                            result[i].PROCESS_2_CD = process_CD2;
                        }
                        else
                        {
                            result[i].PROCESS_2_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME2;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_2_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME2;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_2_CD = 0;

                }
                if (result[i].PROCESS_NAME3.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME3.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME3.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME3;
                    }
                    StringBuilder selectQuery21 = new StringBuilder();
                    selectQuery21.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery21.Append("WHERE ");
                    selectQuery21.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD3 = Convert.ToInt16(ExecuteScalar(selectQuery21.ToString()));
                    if (proc_CD3 > 0)
                    {
                        StringBuilder selectQuery22 = new StringBuilder();
                        selectQuery22.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery22.Append("WHERE ");
                        selectQuery22.Append("PROCESS_CD = '" + proc_CD3 + "'");
                        process_CD3 = Convert.ToInt16(ExecuteScalar(selectQuery22.ToString()));
                        if (process_CD3 > 0)
                        {
                            result[i].PROCESS_3_CD = process_CD3;
                        }
                        else
                        {
                            result[i].PROCESS_3_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME3;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_3_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME3;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_3_CD = 0;
                }
                if (result[i].PROCESS_NAME4.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME4.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME4.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME4;
                    }
                    StringBuilder selectQuery23 = new StringBuilder();
                    selectQuery23.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery23.Append("WHERE ");
                    selectQuery23.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD4 = Convert.ToInt16(ExecuteScalar(selectQuery23.ToString()));
                    if (proc_CD4 > 0)
                    {
                        StringBuilder selectQuery24 = new StringBuilder();
                        selectQuery24.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery24.Append("WHERE ");
                        selectQuery24.Append("PROCESS_CD = '" + proc_CD4 + "'");
                        process_CD4 = Convert.ToInt16(ExecuteScalar(selectQuery24.ToString()));
                        if (process_CD4 > 0)
                        {
                            result[i].PROCESS_4_CD = process_CD4;
                        }
                        else
                        {
                            result[i].PROCESS_4_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME4;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_4_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME4;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_4_CD = 0;
                }
                if (result[i].PROCESS_NAME5.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME5.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME5.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME5;
                    }
                    StringBuilder selectQuery25 = new StringBuilder();
                    selectQuery25.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery25.Append("WHERE ");
                    selectQuery25.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD5 = Convert.ToInt16(ExecuteScalar(selectQuery25.ToString()));
                    if (proc_CD5 > 0)
                    {
                        StringBuilder selectQuery26 = new StringBuilder();
                        selectQuery26.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery26.Append("WHERE ");
                        selectQuery26.Append("PROCESS_CD = '" + proc_CD5 + "'");
                        process_CD5 = Convert.ToInt16(ExecuteScalar(selectQuery26.ToString()));
                        if (process_CD5 > 0)
                        {
                            result[i].PROCESS_5_CD = process_CD5;
                        }
                        else
                        {
                            result[i].PROCESS_5_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME5;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_5_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME5;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_5_CD = 0;
                }
                if (result[i].PROCESS_NAME6.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME6.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME6.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME6;
                    }
                    StringBuilder selectQuery27 = new StringBuilder();
                    selectQuery27.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery27.Append("WHERE ");
                    selectQuery27.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD6 = Convert.ToInt16(ExecuteScalar(selectQuery27.ToString()));
                    if (proc_CD6 > 0)
                    {
                        StringBuilder selectQuery28 = new StringBuilder();
                        selectQuery28.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery28.Append("WHERE ");
                        selectQuery28.Append("PROCESS_CD = '" + proc_CD6 + "'");
                        process_CD6 = Convert.ToInt16(ExecuteScalar(selectQuery28.ToString()));
                        if (process_CD6 > 0)
                        {
                            result[i].PROCESS_6_CD = process_CD6;
                        }
                        else
                        {
                            result[i].PROCESS_6_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME6;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_6_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME6;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_6_CD = 0;
                }
                if (result[i].PROCESS_NAME7.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME7.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME7.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME7;
                    }
                    StringBuilder selectQuery29 = new StringBuilder();
                    selectQuery29.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery29.Append("WHERE ");
                    selectQuery29.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD7 = Convert.ToInt16(ExecuteScalar(selectQuery29.ToString()));
                    if (proc_CD7 > 0)
                    {
                        StringBuilder selectQuery30 = new StringBuilder();
                        selectQuery30.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery30.Append("WHERE ");
                        selectQuery30.Append("PROCESS_CD = '" + proc_CD7 + "'");
                        process_CD7 = Convert.ToInt16(ExecuteScalar(selectQuery30.ToString()));
                        if (process_CD7 > 0)
                        {
                            result[i].PROCESS_7_CD = process_CD7;
                        }
                        else
                        {
                            result[i].PROCESS_7_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME7;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_7_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME7;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_7_CD = 0;
                }
                if (result[i].PROCESS_NAME8.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME8.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME8.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME8;
                    }
                    StringBuilder selectQuery31 = new StringBuilder();
                    selectQuery31.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery31.Append("WHERE ");
                    selectQuery31.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD8 = Convert.ToInt16(ExecuteScalar(selectQuery31.ToString()));
                    if (proc_CD8 > 0)
                    {
                        StringBuilder selectQuery32 = new StringBuilder();
                        selectQuery32.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery32.Append("WHERE ");
                        selectQuery32.Append("PROCESS_CD = '" + proc_CD8 + "'");
                        process_CD8 = Convert.ToInt16(ExecuteScalar(selectQuery32.ToString()));
                        if (process_CD8 > 0)
                        {
                            result[i].PROCESS_8_CD = process_CD8;
                        }
                        else
                        {
                            result[i].PROCESS_8_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME8;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_8_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME8;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_8_CD = 0;
                }
                if (result[i].PROCESS_NAME9.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME9.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME9.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME9;
                    }
                    StringBuilder selectQuery33 = new StringBuilder();
                    selectQuery33.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery33.Append("WHERE ");
                    selectQuery33.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD9 = Convert.ToInt16(ExecuteScalar(selectQuery33.ToString()));
                    if (proc_CD9 > 0)
                    {
                        StringBuilder selectQuery34 = new StringBuilder();
                        selectQuery34.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery34.Append("WHERE ");
                        selectQuery34.Append("PROCESS_CD = '" + proc_CD9 + "'");
                        process_CD9 = Convert.ToInt16(ExecuteScalar(selectQuery34.ToString()));
                        if (process_CD9 > 0)
                        {
                            result[i].PROCESS_9_CD = process_CD9;
                        }
                        else
                        {
                            result[i].PROCESS_9_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME9;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_9_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME9;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_9_CD = 0;
                }
                if (result[i].PROCESS_NAME10.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME10.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME10.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME10;
                    }
                    StringBuilder selectQuery35 = new StringBuilder();
                    selectQuery35.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery35.Append("WHERE ");
                    selectQuery35.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD10 = Convert.ToInt16(ExecuteScalar(selectQuery35.ToString()));
                    if (proc_CD10 > 0)
                    {
                        StringBuilder selectQuery36 = new StringBuilder();
                        selectQuery36.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery36.Append("WHERE ");
                        selectQuery36.Append("PROCESS_CD = '" + proc_CD10 + "'");
                        process_CD10 = Convert.ToInt16(ExecuteScalar(selectQuery36.ToString()));
                        if (process_CD10 > 0)
                        {
                            result[i].PROCESS_10_CD = process_CD10;
                        }
                        else
                        {
                            result[i].PROCESS_10_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME10;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_10_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME10;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_10_CD = 0;
                }
                if (result[i].PROCESS_NAME11.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME11.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME11.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME11;
                    }
                    StringBuilder selectQuery37 = new StringBuilder();
                    selectQuery37.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery37.Append("WHERE ");
                    selectQuery37.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD11 = Convert.ToInt16(ExecuteScalar(selectQuery37.ToString()));
                    if (proc_CD11 > 0)
                    {
                        StringBuilder selectQuery38 = new StringBuilder();
                        selectQuery38.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery38.Append("WHERE ");
                        selectQuery38.Append("PROCESS_CD = '" + proc_CD11 + "'");
                        process_CD11 = Convert.ToInt16(ExecuteScalar(selectQuery38.ToString()));
                        if (process_CD11 > 0)
                        {
                            result[i].PROCESS_11_CD = process_CD11;
                        }
                        else
                        {
                            result[i].PROCESS_11_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME11;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_11_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME11;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_11_CD = 0;
                }
                if (result[i].PROCESS_NAME12.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME12.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME12.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME12;
                    }
                    StringBuilder selectQuery39 = new StringBuilder();
                    selectQuery39.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery39.Append("WHERE ");
                    selectQuery39.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD12 = Convert.ToInt16(ExecuteScalar(selectQuery39.ToString()));
                    if (proc_CD12 > 0)
                    {
                        StringBuilder selectQuery40 = new StringBuilder();
                        selectQuery40.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery40.Append("WHERE ");
                        selectQuery40.Append("PROCESS_CD = '" + proc_CD12 + "'");
                        process_CD12 = Convert.ToInt16(ExecuteScalar(selectQuery40.ToString()));
                        if (process_CD12 > 0)
                        {
                            result[i].PROCESS_12_CD = process_CD12;
                        }
                        else
                        {
                            result[i].PROCESS_12_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME12;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_12_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME12;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_12_CD = 0;
                }
                if (result[i].PROCESS_NAME13.Trim() != string.Empty)
                {
                    if (result[i].PROCESS_NAME13.Contains("＿"))
                    {
                        process_Name = result[i].PROCESS_NAME13.Replace("＿", "_");
                    }
                    else
                    {
                        process_Name = result[i].PROCESS_NAME13;
                    }
                    StringBuilder selectQuery41 = new StringBuilder();
                    selectQuery41.Append("SELECT PROC_CD FROM HSKUSER.MST_PROCESS ");
                    selectQuery41.Append("WHERE ");
                    selectQuery41.Append("PROC_NAME LIKE  '" + process_Name + "'");
                    proc_CD13 = Convert.ToInt16(ExecuteScalar(selectQuery41.ToString()));
                    if (proc_CD13 > 0)
                    {
                        StringBuilder selectQuery42 = new StringBuilder();
                        selectQuery42.Append("SELECT PROCESS_CD FROM MST_PROCESS ");
                        selectQuery42.Append("WHERE ");
                        selectQuery42.Append("PROCESS_CD = '" + proc_CD13 + "'");
                        process_CD13 = Convert.ToInt16(ExecuteScalar(selectQuery42.ToString()));
                        if (process_CD13 > 0)
                        {
                            result[i].PROCESS_13_CD = process_CD13;
                        }
                        else
                        {
                            result[i].PROCESS_13_CD = null;
                            result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME13;
                            break;
                        }
                    }
                    else
                    {
                        result[i].PROCESS_13_CD = null;
                        result[i].PROCESS_NAME_STRING = result[i].PROCESS_NAME13;
                        break;
                    }
                }
                else
                {
                    result[i].PROCESS_13_CD = 0;
                }
                StringBuilder selectQuery47 = new StringBuilder();
                selectQuery47.Append("SELECT MAKER_CD FROM HSKUSER.MST_MAKER ");
                selectQuery47.Append("WHERE ");
                selectQuery47.Append("MAKER_CD_HOST = '" + result[i].MAKER_CODE.Trim() + "' ");
                int win_Maker_CD = Convert.ToInt32(ExecuteScalar(selectQuery47.ToString()));
                if (win_Maker_CD > 0)
                {
                    StringBuilder selectQuery48 = new StringBuilder();
                    selectQuery48.Append("SELECT MAKER_CD FROM MST_MAKER ");
                    selectQuery48.Append("WHERE ");
                    selectQuery48.Append("MAKER_CD = '" + win_Maker_CD + "' ");
                    int web_Maker_CD = Convert.ToInt32(ExecuteScalar(selectQuery48.ToString()));
                    if (web_Maker_CD > 0)
                    {
                        result[i].MAKER_CD = web_Maker_CD;
                    }
                    else
                    {
                        result[i].MAKER_CD = null;
                        break;
                    }
                }
                else
                {
                    result[i].MAKER_CD = null;
                    break;
                }
                StringBuilder selectQuery49 = new StringBuilder();
                selectQuery49.Append("SELECT PROD_SPEC_CD FROM HSKUSER.MST_PROD_SPEC ");
                selectQuery49.Append("WHERE ");
                selectQuery49.Append("PROD_SPEC_NO_1 = '" + result[i].PROD_SPEC_NO1.Trim() + "' AND PROD_SPEC_NO_2 = '" + result[i].PROD_SPEC_NO2.Trim() + "' ");
                prod_Spec_CD = Convert.ToInt32(ExecuteScalar(selectQuery49.ToString()));
                StringBuilder selectQuery50 = new StringBuilder();
                selectQuery50.Append("SELECT BEND_TOL AS MAGARI_BEND_TOL_STRING ,MAGARI_SPAN AS MAGARI_SPAN_STRING FROM HSKUSER.MST_PROD_VER ");
                selectQuery50.Append("WHERE ");
                selectQuery50.Append("PROD_SPEC_CD = '" + prod_Spec_CD + "' ");
                selectQuery50.Append("ORDER BY VER DESC ");//changed created date to ver on 10/12/2025 as per client requirment(Nilesh sir)
                selectQuery50.Append("FETCH FIRST 1 ROWS ONLY ");
                prod_Ver_Details = ExecuteQuerySingleResult<GridSearchFields>(selectQuery50.ToString());
                if (prod_Ver_Details.MAGARI_BEND_TOL_STRING != null && prod_Ver_Details.MAGARI_SPAN_STRING != null && result[i].MAGARI_BEND_TOL_STRING != null && result[i].MAGARI_SPAN_STRING != null)
                {

                    if (prod_Ver_Details.MAGARI_BEND_TOL_STRING == result[i].MAGARI_BEND_TOL_STRING && prod_Ver_Details.MAGARI_SPAN_STRING == result[i].MAGARI_SPAN_STRING)
                    {
                        result[i].MAGARI_BEND_TOL_STRING = result[i].MAGARI_BEND_TOL_STRING;
                        result[i].MAGARI_SPAN_STRING = result[i].MAGARI_SPAN_STRING;
                    }
                    else
                    {
                        result[i].MAGARI_BEND_TOL_STRING = "";
                        result[i].MAGARI_SPAN_STRING = "";
                        break;
                    }
                }
                else
                {
                    result[i].MAGARI_BEND_TOL_STRING = result[i].MAGARI_BEND_TOL_STRING;//if prod_Ver_Details.MAGARI_BEND_TOL_STRING = null || prod_Ver_Details.MAGARI_SPAN_STRING =null || result[i].MAGARI_BEND_TOL_STRING = null || result[i].MAGARI_SPAN_STRING = null save value from HTMP_PROD_ORDER as per client requirement (Nilesh sir)
                    result[i].MAGARI_SPAN_STRING = result[i].MAGARI_SPAN_STRING;
                }

                if (result[i].PROD_SPEC_NO1.Trim().Contains("ｾﾂﾀﾝ") || result[i].PROD_SPEC_NO1.Trim().Contains("ｻｲｶｺ") || result[i].PROD_SPEC_NO1.Trim().Contains("ｺｸｲﾝ")) //code added on 26/12/2025
                {
                    if (result[i].DIM_TOL_MAX_SPLIT.Trim() != string.Empty && result[i].DIM_TOL_MIN_SPLIT.Trim() != string.Empty)
                    {
                        if (result[i].DIM_TOL_MAX_SPLIT.Any(char.IsLetter) && result[i].DIM_TOL_MIN_SPLIT.Any(char.IsLetter))
                        {
                            result[i].DIM_TOL_MAX = null;
                            result[i].DIM_TOL_MIN = null;
                            result[i].DIM_TOL_MAX_CHR = result[i].DIM_TOL_MAX_SPLIT;
                            result[i].DIM_TOL_MIN_CHR = result[i].DIM_TOL_MIN_SPLIT;
                        }
                        else
                        {
                            result[i].DIM_TOL_MAX = Convert.ToSingle(result[i].DIM_TOL_MAX_SPLIT);
                            result[i].DIM_TOL_MIN = Convert.ToSingle(result[i].DIM_TOL_MIN_SPLIT);
                            result[i].DIM_TOL_MAX_CHR = result[i].DIM_TOL_MAX_SPLIT;
                            result[i].DIM_TOL_MIN_CHR = result[i].DIM_TOL_MIN_SPLIT;
                        }

                    }
                    else
                    {
                        result[i].DIM_TOL_MAX = null;
                        result[i].DIM_TOL_MIN = null;
                        result[i].DIM_TOL_MAX_CHR = null;
                        result[i].DIM_TOL_MIN_CHR = null;

                    }

                    if (result[i].LEN_TOL_MAX_SPLIT.Trim() != string.Empty && result[i].LEN_TOL_MIN_SPLIT.Trim() != string.Empty)
                    {
                        if (result[i].LEN_TOL_MAX_SPLIT.Any(char.IsLetter) && result[i].LEN_TOL_MIN_SPLIT.Any(char.IsLetter))
                        {
                            result[i].HOST_LENGTH_TOL_MAX = null;
                            result[i].HOST_LENGTH_TOL_MIN = null;
                            result[i].LENGTH_TOL_MAX_CHR = result[i].LEN_TOL_MAX_SPLIT;
                            result[i].LENGTH_TOL_MIN_CHR = result[i].LEN_TOL_MIN_SPLIT;
                        }
                        else
                        {
                            result[i].HOST_LENGTH_TOL_MAX = Convert.ToSingle(result[i].LEN_TOL_MAX_SPLIT);
                            result[i].HOST_LENGTH_TOL_MIN = Convert.ToSingle((result[i].LEN_TOL_MIN_SPLIT));
                            result[i].LENGTH_TOL_MAX_CHR = result[i].LEN_TOL_MAX_SPLIT;
                            result[i].LENGTH_TOL_MIN_CHR = result[i].LEN_TOL_MIN_SPLIT;
                        }


                    }
                    else
                    {
                        result[i].HOST_LENGTH_TOL_MAX = null;
                        result[i].HOST_LENGTH_TOL_MIN = null;
                        result[i].LENGTH_TOL_MAX_CHR = null;
                        result[i].LENGTH_TOL_MIN_CHR = null;
                    }
                }
                else
                {
                    StringBuilder selectSpecCd = new StringBuilder();
                    selectSpecCd.Append("SELECT PROD_SPEC_CD FROM HSKUSER.MST_PROD_SPEC ");
                    selectSpecCd.Append("WHERE ");
                    selectSpecCd.Append("PROD_SPEC_NO_1 = '" + result[i].PROD_SPEC_NO1.Trim() + "' AND PROD_SPEC_NO_2 = '" + result[i].PROD_SPEC_NO2.Trim() + "' ");
                    prod_Spec_CD = Convert.ToInt32(ExecuteScalar(selectSpecCd.ToString()));

                    StringBuilder SelectDimLenTolVal = new StringBuilder();
                    SelectDimLenTolVal.Append("SELECT DIM_TOL_MIN,DIM_TOL_MAX,LEN_TOL_MIN AS LENGTH_TOL_MIN ,LEN_TOL_MAX AS LENGTH_TOL_MAX FROM HSKUSER.MST_PROD_VER ");
                    SelectDimLenTolVal.Append("WHERE ");
                    SelectDimLenTolVal.Append("PROD_SPEC_CD = '" + prod_Spec_CD + "' ");
                    SelectDimLenTolVal.Append("ORDER BY VER DESC ");//changed created date to ver on 11/12/2025 as per client requirment(Nilesh sir)
                    SelectDimLenTolVal.Append("FETCH FIRST 1 ROWS ONLY ");
                    prod_Ver_Details = ExecuteQuerySingleResult<GridSearchFields>(SelectDimLenTolVal.ToString());
                    if (result[i].DIM_TOL_MAX_SPLIT.Trim() != string.Empty && result[i].DIM_TOL_MIN_SPLIT.Trim() != string.Empty)
                    {
                        if (result[i].DIM_TOL_MAX_SPLIT.Any(char.IsLetter) && result[i].DIM_TOL_MIN_SPLIT.Any(char.IsLetter))
                        {
                            if (prod_Ver_Details.DIM_TOL_MAX == result[i].DIM_TOL_MAX && prod_Ver_Details.DIM_TOL_MIN == result[i].DIM_TOL_MIN)
                            {
                                result[i].DIM_TOL_MAX = null;
                                result[i].DIM_TOL_MIN = null;
                                result[i].DIM_TOL_MAX_CHR = result[i].DIM_TOL_MAX_SPLIT;
                                result[i].DIM_TOL_MIN_CHR = result[i].DIM_TOL_MIN_SPLIT;
                            }
                            else
                            {
                                result[i].DIM_TOL_MAX = 0;
                                result[i].DIM_TOL_MIN = 0;
                                result[i].DIM_TOL_MAX_CHR = "";
                                result[i].DIM_TOL_MIN_CHR = "";
                                break;
                            }
                        }
                        else
                        {
                            if (prod_Ver_Details.DIM_TOL_MAX == Convert.ToSingle(result[i].DIM_TOL_MAX_SPLIT) && prod_Ver_Details.DIM_TOL_MIN == Convert.ToSingle(result[i].DIM_TOL_MIN_SPLIT))
                            {
                                result[i].DIM_TOL_MAX = Convert.ToSingle(result[i].DIM_TOL_MAX_SPLIT);
                                result[i].DIM_TOL_MIN = Convert.ToSingle(result[i].DIM_TOL_MIN_SPLIT);
                                result[i].DIM_TOL_MAX_CHR = result[i].DIM_TOL_MAX_SPLIT;
                                result[i].DIM_TOL_MIN_CHR = result[i].DIM_TOL_MIN_SPLIT;
                            }
                            else
                            {
                                result[i].DIM_TOL_MAX = 0;
                                result[i].DIM_TOL_MIN = 0;
                                result[i].DIM_TOL_MAX_CHR = "";
                                result[i].DIM_TOL_MIN_CHR = "";
                                break;
                            }
                        }
                    }
                    else
                    {
                        result[i].DIM_TOL_MAX = null;
                        result[i].DIM_TOL_MIN = null;
                        result[i].DIM_TOL_MAX_CHR = null;
                        result[i].DIM_TOL_MIN_CHR = null;
                    }
                    if (result[i].LEN_TOL_MAX_SPLIT.Trim() != string.Empty && result[i].LEN_TOL_MIN_SPLIT.Trim() != string.Empty)
                    {
                        if (result[i].LEN_TOL_MAX_SPLIT.Any(char.IsLetter) && result[i].LEN_TOL_MIN_SPLIT.Any(char.IsLetter))
                        {
                            if (prod_Ver_Details.LENGTH_TOL_MAX == result[i].HOST_LENGTH_TOL_MAX && prod_Ver_Details.LENGTH_TOL_MIN == result[i].HOST_LENGTH_TOL_MIN)
                            {
                                result[i].HOST_LENGTH_TOL_MAX = null;
                                result[i].HOST_LENGTH_TOL_MIN = null;
                                result[i].LENGTH_TOL_MAX_CHR = result[i].LEN_TOL_MAX_SPLIT;
                                result[i].LENGTH_TOL_MIN_CHR = result[i].LEN_TOL_MIN_SPLIT;
                            }
                            else
                            {
                                result[i].HOST_LENGTH_TOL_MAX = 0;
                                result[i].HOST_LENGTH_TOL_MIN = 0;
                                result[i].LENGTH_TOL_MAX_CHR = "";
                                result[i].LENGTH_TOL_MIN_CHR = "";
                                break;
                            }
                        }
                        else
                        {
                            if (prod_Ver_Details.LENGTH_TOL_MAX == Convert.ToSingle(result[i].LEN_TOL_MAX_SPLIT) && prod_Ver_Details.LENGTH_TOL_MIN == Convert.ToSingle(result[i].LEN_TOL_MIN_SPLIT))
                            {
                                result[i].HOST_LENGTH_TOL_MAX = Convert.ToSingle(result[i].LEN_TOL_MAX_SPLIT);
                                result[i].HOST_LENGTH_TOL_MIN = Convert.ToSingle((result[i].LEN_TOL_MIN_SPLIT));
                                result[i].LENGTH_TOL_MAX_CHR = result[i].LEN_TOL_MAX_SPLIT;
                                result[i].LENGTH_TOL_MIN_CHR = result[i].LEN_TOL_MIN_SPLIT;
                            }
                            else
                            {
                                result[i].HOST_LENGTH_TOL_MAX = 0;
                                result[i].HOST_LENGTH_TOL_MIN = 0;
                                result[i].LENGTH_TOL_MAX_CHR = "";
                                result[i].LENGTH_TOL_MIN_CHR = "";
                                break;
                            }
                        }

                    }
                    else
                    {
                        result[i].HOST_LENGTH_TOL_MAX = null;
                        result[i].HOST_LENGTH_TOL_MIN = null;
                        result[i].LENGTH_TOL_MAX_CHR = null;
                        result[i].LENGTH_TOL_MIN_CHR = null;
                    }
                }


                StringBuilder SelectProdVerImpactData = new StringBuilder();
                SelectProdVerImpactData.Append(" SELECT HSKUSER.GET_TS_IMPACT_STD_INFO('" + result[i].PROD_SPEC_NO1 + "', '" + result[i].PROD_SPEC_NO2 + "', '" + result[i].FUHEN_NO + "') AS IMPACT_TEST_INFO, ");
                SelectProdVerImpactData.Append(" HSKUSER.GET_TS_IMPACT_STD_MIN('" + result[i].PROD_SPEC_NO1 + "', '" + result[i].PROD_SPEC_NO2 + "', '" + result[i].FUHEN_NO + "') AS IMPACT_TEST_MIN, ");
                SelectProdVerImpactData.Append(" HSKUSER.GET_TS_IMPACT_STD_MAX('" + result[i].PROD_SPEC_NO1 + "', '" + result[i].PROD_SPEC_NO2 + "', '" + result[i].FUHEN_NO + "') AS IMPACT_TEST_MAX, ");
                SelectProdVerImpactData.Append(" HSKUSER.GET_TS_IMPACT_STD_PATTERN('" + result[i].PROD_SPEC_NO1 + "', '" + result[i].PROD_SPEC_NO2 + "', '" + result[i].FUHEN_NO + "') AS IMPACT_TEST_PATTERN FROM DUAL ");
                prod_Ver_Details = ExecuteQuerySingleResult<GridSearchFields>(SelectProdVerImpactData.ToString());
                result[i].IMPACT_TEST_INFO = prod_Ver_Details.IMPACT_TEST_INFO;
                result[i].IMPACT_TEST_MIN = prod_Ver_Details.IMPACT_TEST_MIN;
                result[i].IMPACT_TEST_MAX = prod_Ver_Details.IMPACT_TEST_MAX;
                result[i].IMPACT_TEST_PATTERN = prod_Ver_Details.IMPACT_TEST_PATTERN;

            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/07/18       
        /// used screens: ProcessPlanHandoverHost
        /// </summary>
        /// <returns>
        /// check data is exist in TRN_PROCESS_PLAN table for Update Btn
        /// </returns>
        /// 
        public List<GridSearchFields> ValidateDataForUpdate(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();
            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                //int count;
                //StringBuilder selectQuery = new StringBuilder();
                //selectQuery.Append("SELECT COUNT(*) FROM TRN_PROCESS_PLAN ");
                //selectQuery.Append(" WHERE ");
                //selectQuery.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN_GYOUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
                //count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
                //result.Add(processPlanDetails[i]);
                //if (count == 0)
                //{
                //    result[i].CHUUBAN_GYOUBAN = result[i].CHUUBAN_GYOUBAN;
                //    result[i].RESPONCE = "Data does not exist";
                //    break;
                //}
                StringBuilder sqlQuery = new StringBuilder();
                sqlQuery.Append("SELECT SUBSTR(CHUUBAN_YEAR,2,1) FROM TRN_PROCESS_PLAN ");
                sqlQuery.Append(" WHERE ");
                sqlQuery.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN_GYOUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
                string chuubanYear = Convert.ToString(ExecuteScalar(sqlQuery.ToString()));
                result.Add(processPlanDetails[i]);
                if (chuubanYear != processPlanDetails[i].CHHUBAN_YEAR)//check if chuuban year is differnt and complete date is not null
                {
                    result[i].CHUUBAN_GYOUBAN = result[i].CHUUBAN_GYOUBAN;
                    result[i].RESPONCE = "Can not update existing data";//If chuuban year is not same as existing data then user can not update existing data
                    break;
                }

            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/06/18       
        /// used screens: ProcessPlanHandoverHost
        /// used screens: ProcessPlanHandoverHost
        /// </summary>
        /// <returns>
        /// check data is exist in TRN_PROCESS_PLAN table for AddNew btn
        /// </returns>
        /// 
        public List<GridSearchFields> ValidateDataForAddNewBtn(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();
            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                int count;
                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append("SELECT SUBSTR(CHUUBAN_YEAR,2,1) FROM TRN_PROCESS_PLAN ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN_GYOUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
                string chuubanyear = Convert.ToString(ExecuteScalar(selectQuery.ToString()));
                result.Add(processPlanDetails[i]);
                if (chuubanyear == processPlanDetails[i].CHHUBAN_YEAR)
                {
                    result[i].CHUUBAN_GYOUBAN = result[i].CHUUBAN_GYOUBAN;
                    result[i].RESPONCE = "Data Already exist";
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2024/01/04       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Insert data into table TRN_PROCESS_PLAN 
        /// Insert data into table TRN_NIFUDA 
        /// Insert data into table TRN_WIP_STATUS
        /// </returns>
        /// 
        public string UpdateProcessPlanHandoverData(List<GridSearchFields> processPlanDetails)
        {

            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                GridSearchFields HPOResult = new GridSearchFields();
                HPOResult = CheckProcessCount(processPlanDetails[i]);
                int process_Plan_CD;
                var PrintDate = "";
                var Del_Date = "";
                var Delivery_Date = "";
                var Del_Date_Month = "";
                var Del_Date_Date = "";
                var Print_Date_Month_date = "";
                var Print_Date_Month = "";
                var Print_Date_Date = "";
                var Print_date = "";
                var Date_Year = "";
                var unitWtData1 = "";
                var unitWtData2 = "";
                var unitWtData3 = "";
                string[] unitWtData;

                StringBuilder selectQuery = new StringBuilder();
                selectQuery.Append("SELECT PROCESS_PLAN_CD FROM TRN_PROCESS_PLAN ");
                selectQuery.Append(" WHERE ");
                selectQuery.Append(" CHUUBAN = '" + processPlanDetails[i].CHUUBAN + "' AND GYOUBAN ='" + processPlanDetails[i].GYOUBAN + "' AND  PROCESS_PLAN_CMPL_DATE IS NULL ");
                process_Plan_CD = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append("UPDATE TRN_PROCESS_PLAN SET ");
                updateQuery.Append("PRODUCT_CD = '" + "" + "', "); // Placeholder
                updateQuery.Append("ROOT_VER_PRODUCT_KEY = '" + "" + "', ");
                updateQuery.Append("KAKOUIRAI_CD = '" + "" + "', ");
                updateQuery.Append("ACT_ALLOC_CD = '" + "" + "', ");
                updateQuery.Append("CUSTOMER_CD = '" + processPlanDetails[i].CUSTOMER_CD + "', ");
                updateQuery.Append("RECEIVER_CD = '" + processPlanDetails[i].RECEIVER_CD + "', ");
                updateQuery.Append("INTERNAL_NAME_CD = '" + processPlanDetails[i].INTERNAL_NAME_CD + "', ");
                updateQuery.Append("DEPARTMENT_GROUP_CD = '" + processPlanDetails[i].DEPARTMENT_GROUP_CD + "', ");
                updateQuery.Append("MATL_NAME_MAK_CD = '" + "" + "', ");
                updateQuery.Append("PROD_CONDITION_CD = (SELECT CONDITION_CD FROM MST_CONDITION WHERE CONDITION_DESCRIPTION = '" + processPlanDetails[i].PROD_CONDITION_STRING + "'), ");
                updateQuery.Append("MATL_CONDITION_CD = '" + "" + "', ");
                updateQuery.Append("NYUKA_CD = '" + processPlanDetails[i].HOST_REFERENCE_NO + "', ");
                updateQuery.Append("STORAGE_LOCATION_CD = '" + "" + "', ");
                updateQuery.Append("LOCATION_CD = (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = '" + processPlanDetails[i].USER_ID + "'), ");
                updateQuery.Append("PACKING_CD = '" + processPlanDetails[i].PACKING_CD + "', ");
                updateQuery.Append("CHUUBAN = '" + processPlanDetails[i].CHUUBAN + "', ");
                updateQuery.Append("GYOUBAN = '" + processPlanDetails[i].GYOUBAN + "', ");
                updateQuery.Append("ORIGINAL_CHUBBAN = '" + "" + "', ");
                updateQuery.Append("PROD_SPEC_NO = '" + processPlanDetails[i].PROD_SPEC_NO1 + "-" + processPlanDetails[i].PROD_SPEC_NO2 + "', ");
                updateQuery.Append("PROD_SPEC_NO1 = '" + processPlanDetails[i].PROD_SPEC_NO1 + "', ");
                updateQuery.Append("PROD_SPEC_NO2 = '" + processPlanDetails[i].PROD_SPEC_NO2 + "', ");
                updateQuery.Append("VER = '" + processPlanDetails[i].VER + "', ");
                updateQuery.Append("STD_DEL_PERIOD = '" + processPlanDetails[i].STD_DEL_PERIOD + "',");
                updateQuery.Append("PROD_NAME_CUST = '" + processPlanDetails[i].PROD_NAME_CUST + "',");//25
                updateQuery.Append("PROD_SHAPE = '" + processPlanDetails[i].SHAPE + "',");
                updateQuery.Append("PROD_DIM_D = '" + processPlanDetails[i].PROD_DIM_D + "',");
                updateQuery.Append("PROD_DIM_W = '" + "" + "',");
                if (processPlanDetails[i].PROD_DIM_OTHER_STRING.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_DIM_OTHER = '" + processPlanDetails[i].PROD_DIM_OTHER_STRING + "',");
                }
                else
                {
                    updateQuery.Append("PROD_DIM_OTHER = '" + "" + "',");
                }
                updateQuery.Append(" DIM_TOL_MIN = '" + processPlanDetails[i].DIM_TOL_MIN + "',");
                updateQuery.Append("DIM_TOL_MAX = '" + processPlanDetails[i].DIM_TOL_MAX + "',");
                updateQuery.Append("DIM_TOL_MIN_CHR = '" + processPlanDetails[i].DIM_TOL_MIN_CHR + "',");
                updateQuery.Append("DIM_TOL_MAX_CHR = '" + processPlanDetails[i].DIM_TOL_MAX_CHR + "',");
                //if (processPlanDetails[i].DIM_TOL_MIN_CHR.Trim() != string.Empty)
                //{
                //    updateQuery.Append("DIM_TOL_MIN = '" + processPlanDetails[i].DIM_TOL_MIN_CHR + "" + processPlanDetails[i].DIM_TOL_MIN + "',");
                //}
                //else
                //{

                //    updateQuery.Append(" DIM_TOL_MIN = '" + processPlanDetails[i].DIM_TOL_MIN + "',");
                //}
                //if (processPlanDetails[i].DIM_TOL_MAX_CHR.Trim() != string.Empty)
                //{

                //    updateQuery.Append("DIM_TOL_MAX = '" + processPlanDetails[i].DIM_TOL_MAX_CHR + "" + processPlanDetails[i].DIM_TOL_MAX + "',");
                //}
                //else
                //{
                //    updateQuery.Append("DIM_TOL_MAX = '" + processPlanDetails[i].DIM_TOL_MAX + "',");
                //}
                //if (processPlanDetails[i].DIM_TOL_MIN_CHR.Trim() != string.Empty)
                //{
                //    updateQuery.Append("DIM_TOL_MIN_CHR = '" + processPlanDetails[i].DIM_TOL_MIN_CHR + "" + processPlanDetails[i].DIM_TOL_MIN + "',");

                //}
                //else
                //{
                //    updateQuery.Append("DIM_TOL_MIN_CHR = '" + processPlanDetails[i].DIM_TOL_MIN + "',");
                //}
                //if (processPlanDetails[i].DIM_TOL_MAX_CHR.Trim() != string.Empty)
                //{
                //    updateQuery.Append("DIM_TOL_MAX_CHR = '" + processPlanDetails[i].DIM_TOL_MAX_CHR + "" + processPlanDetails[i].DIM_TOL_MAX + "',");
                //}
                //else
                //{
                //    updateQuery.Append("DIM_TOL_MAX_CHR = '" + processPlanDetails[i].DIM_TOL_MAX + "',");
                //}
                if (processPlanDetails[i].PROD_LENGTH != null)
                {
                    updateQuery.Append("PROD_LENGTH = '" + processPlanDetails[i].PROD_LENGTH + "',");//34
                }
                else
                {
                    updateQuery.Append("PROD_LENGTH = '" + "" + "',");//34
                }
                if (processPlanDetails[i].LENGTH_OTHER != null)
                {
                    updateQuery.Append("LENGTH_OTHER = '" + processPlanDetails[i].LENGTH_OTHER + "',");
                }
                else
                {
                    updateQuery.Append("LENGTH_OTHER = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_LENGTH != null)
                {
                    updateQuery.Append("LENGTH_TOL_MIN = '" + processPlanDetails[i].LENGTH_TOL_MIN + "',");
                    updateQuery.Append("LENGTH_TOL_MAX = '" + processPlanDetails[i].LENGTH_TOL_MAX + "',");
                    updateQuery.Append("LENGTH_TOL_MIN_CHR = '" + processPlanDetails[i].LENGTH_TOL_MIN_CHR + "',");
                    updateQuery.Append("LENGTH_TOL_MAX_CHR = '" + processPlanDetails[i].LENGTH_TOL_MAX_CHR + "',");
                    //if (processPlanDetails[i].LENGTH_TOL_MIN_CHR.Trim() != string.Empty)
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MIN = '" + processPlanDetails[i].LENGTH_TOL_MIN_CHR + "" + processPlanDetails[i].LENGTH_TOL_MIN + "',");
                    //}
                    //else
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MIN = '" + processPlanDetails[i].LENGTH_TOL_MIN + "',");
                    //}
                    //if (processPlanDetails[i].LENGTH_TOL_MAX_CHR.Trim() != string.Empty)
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MAX = '" + processPlanDetails[i].LENGTH_TOL_MAX_CHR + "" + processPlanDetails[i].LENGTH_TOL_MAX + "',");
                    //}
                    //else
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MAX = '" + processPlanDetails[i].LENGTH_TOL_MAX + "',");
                    //}

                    //if (processPlanDetails[i].LENGTH_TOL_MIN_CHR.Trim() != string.Empty)
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MIN_CHR = '" + processPlanDetails[i].LENGTH_TOL_MIN_CHR + "" + processPlanDetails[i].LENGTH_TOL_MIN + "',");
                    //}
                    //else
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MIN_CHR = '" + processPlanDetails[i].LENGTH_TOL_MIN + "',");
                    //}
                    //if (processPlanDetails[i].LENGTH_TOL_MAX_CHR.Trim() != string.Empty)
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MAX_CHR = '" + processPlanDetails[i].LENGTH_TOL_MAX_CHR + "" + processPlanDetails[i].LENGTH_TOL_MAX + "',");
                    //}
                    //else
                    //{
                    //    updateQuery.Append("LENGTH_TOL_MAX_CHR = '" + processPlanDetails[i].LENGTH_TOL_MAX + "',");
                    //}



                }
                else
                {
                    updateQuery.Append("LENGTH_TOL_MIN = '" + "" + "',");
                    updateQuery.Append("LENGTH_TOL_MAX = '" + "" + "',");
                    updateQuery.Append("LENGTH_TOL_MIN_CHR = '" + "" + "',");
                    updateQuery.Append("LENGTH_TOL_MAX_CHR = '" + "" + "',");
                }
                if (processPlanDetails[i].COIL_DIA_INT.Trim() != string.Empty)
                {
                    updateQuery.Append("COIL_DIA_INTERNAL = '" + Convert.ToInt32(processPlanDetails[i].COIL_DIA_INT) + "',");
                }
                else
                {
                    updateQuery.Append("COIL_DIA_INTERNAL = '" + "" + "',");
                }
                if (processPlanDetails[i].COIL_DIA_EXT.Trim() != string.Empty)
                {
                    updateQuery.Append("COIL_DIA_EXTERNAL = '" + Convert.ToInt32(processPlanDetails[i].COIL_DIA_EXT) + "',");
                }
                else
                {
                    updateQuery.Append("COIL_DIA_EXTERNAL = '" + "" + "',");
                }
                if (processPlanDetails[i].COIL_WND_DIR != "0")
                {
                    updateQuery.Append("COIL_DIA_WINDING = '" + Convert.ToInt32(processPlanDetails[i].COIL_WND_DIR) + "',");
                }
                else
                {
                    updateQuery.Append("COIL_DIA_WINDING = '" + "" + "',");
                }
                updateQuery.Append("CHAMFER = '" + processPlanDetails[i].CHAMFER + "',");
                if (processPlanDetails[i].MAGARI_KUBUN != 0)
                {
                    updateQuery.Append("MAGARI_KUBUN = '" + processPlanDetails[i].MAGARI_KUBUN + "',");
                }
                else
                {
                    updateQuery.Append("MAGARI_KUBUN = '" + "" + "',");
                }
                if (processPlanDetails[i].MAGARI_BEND_TOL_STRING != null)
                {
                    updateQuery.Append("MAGARI_BEND_TOL = '" + Convert.ToSingle(processPlanDetails[i].MAGARI_BEND_TOL_STRING) + "',");
                }
                else
                {
                    updateQuery.Append("MAGARI_BEND_TOL = '" + "" + "',");
                }
                if (processPlanDetails[i].MAGARI_SPAN_STRING != null)
                {
                    updateQuery.Append("MAGARI_SPAN = '" + Convert.ToInt32(processPlanDetails[i].MAGARI_SPAN_STRING) + "',");
                }
                else
                {
                    updateQuery.Append("MAGARI_SPAN = '" + "" + "',");
                }
                updateQuery.Append("NEJIRE_ANGLE = '" + "" + "',");
                updateQuery.Append("NEJIRE_SPAN = '" + "" + "',");

                if (processPlanDetails[i].UNIT_WT.All(char.IsDigit))
                {
                    updateQuery.Append("UNIT_WT_KUBUN = '" + 1 + "',");
                    updateQuery.Append("UNIT_WT = '" + processPlanDetails[i].UNIT_WT + "',");//50
                    updateQuery.Append("UNIT_WT_MIN = '" + "" + "',");
                    updateQuery.Append("UNIT_WT_MAX = '" + "" + "',");
                }
                else if (processPlanDetails[i].UNIT_WT.Contains('-'))
                {
                    string[] parts = processPlanDetails[i].UNIT_WT.Split('-');
                    if (int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _))
                    {
                        updateQuery.Append("UNIT_WT_KUBUN = '" + 2 + "',");
                        updateQuery.Append("UNIT_WT = '" + "" + "',");
                        updateQuery.Append("UNIT_WT_MIN = '" + parts[0] + "',");
                        updateQuery.Append("UNIT_WT_MAX = '" + parts[1] + "',");
                    }
                    else if (processPlanDetails[i].UNIT_WT.Contains("ﾎﾝ"))
                    {
                        //str = processPlanDetails[i].UNIT_WT;
                        //unitWtData3 = new string(str.Where(char.IsDigit).ToArray());
                        str = processPlanDetails[i].UNIT_WT;
                        unitWtData3 = str.Replace("ﾎﾝ", "").Trim();
                        updateQuery.Append("UNIT_WT_KUBUN = '" + 3 + "',");
                        updateQuery.Append("UNIT_WT = '" + unitWtData3 + "',");
                        updateQuery.Append("UNIT_WT_MIN = '" + "" + "',");
                        updateQuery.Append("UNIT_WT_MAX = '" + "" + "',");
                    }
                    else
                    {
                        updateQuery.Append("UNIT_WT_KUBUN = '" + 1 + "',");
                        updateQuery.Append("UNIT_WT = '" + processPlanDetails[i].UNIT_WT + "',");//50
                        updateQuery.Append("UNIT_WT_MIN = '" + "" + "',");
                        updateQuery.Append("UNIT_WT_MAX = '" + "" + "',");
                    }
                }
                else if (processPlanDetails[i].UNIT_WT.Contains("ﾎﾝ"))
                {
                    //str = processPlanDetails[i].UNIT_WT;
                    //unitWtData3 = new string(str.Where(char.IsDigit).ToArray());
                    str = processPlanDetails[i].UNIT_WT;
                    unitWtData3 = str.Replace("ﾎﾝ", "").Trim();
                    updateQuery.Append("UNIT_WT_KUBUN = '" + 3 + "',");
                    updateQuery.Append("UNIT_WT = '" + unitWtData3 + "',");
                    updateQuery.Append("UNIT_WT_MIN = '" + "" + "',");
                    updateQuery.Append("UNIT_WT_MAX = '" + "" + "',");
                }
                else
                {
                    updateQuery.Append("UNIT_WT_KUBUN = '" + "" + "',");
                    updateQuery.Append("UNIT_WT = '" + "" + "',");
                    updateQuery.Append("UNIT_WT_MIN = '" + "" + "',");
                    updateQuery.Append("UNIT_WT_MAX = '" + "" + "',");
                }




                if (processPlanDetails[i].EXT_TEXTURE.Trim() != string.Empty)
                {
                    updateQuery.Append("EXT_TEXTURE = '" + processPlanDetails[i].EXT_TEXTURE + "',");
                }
                else
                {
                    updateQuery.Append("EXT_TEXTURE = '" + "" + "',");
                }
                if (processPlanDetails[i].COLOR_ID != 0)
                {
                    updateQuery.Append("COLOR_ID = '" + processPlanDetails[i].COLOR_NAME + "',");
                }
                else
                {
                    updateQuery.Append("COLOR_ID = '" + "" + "',");
                }
                if (processPlanDetails[i].DISPLAY.Trim() != string.Empty)
                {
                    updateQuery.Append("DISPLAY = '" + processPlanDetails[i].DISPLAY + "',");
                }
                else
                {
                    updateQuery.Append("DISPLAY = '" + "" + "',");
                }
                if (processPlanDetails[i].STAMP.Trim() != string.Empty)
                {
                    updateQuery.Append("STAMP = '" + processPlanDetails[i].STAMP + "',");
                }
                else
                {
                    updateQuery.Append("STAMP = '" + "" + "',");
                }
                if (processPlanDetails[i].TEST_SPECIMEN.Trim() != string.Empty)
                {
                    updateQuery.Append("TEST_SPECIMEN = '" + processPlanDetails[i].TEST_SPECIMEN + "',");
                }
                else
                {
                    updateQuery.Append("TEST_SPECIMEN = '" + "" + "',");
                }
                commonFunctionToGetTensileYeildValue(processPlanDetails[i].TENSILE_STRN_PATTERN1_STRING);
                updateQuery.Append("TENSILE_STRN_PATTERN1 = '" + codeValue + "',");
                updateQuery.Append("TENSILE_STRN_PATTERN1_MIN_MAX = '" + minMaxValue + "',");
                updateQuery.Append("TENSILE_STRN_PATTERN2_MIN = '" + minValue + "',");
                updateQuery.Append("TENSILE_STRN_PATTERN2_MAX = '" + maxValue + "',");
                updateQuery.Append("TENSILE_STRN_PATTERN3_ACTUAL = '" + actualValue + "',");

                getStrPatternElongationPressValue(processPlanDetails[i].ELONGATION_1, processPlanDetails[i].ELONGATION_2);
                updateQuery.Append("ELONGATION_PATTERN1 = '" + codeValue + "',");
                updateQuery.Append("ELONGATION_PATTERN1_MIN_MAX = '" + minMaxValue + "',");
                updateQuery.Append("ELONGATION_PATTERN2_MIN = '" + minValue + "',");
                updateQuery.Append("ELONGATION_PATTERN2_MAX = '" + maxValue + "',");
                updateQuery.Append("ELONGATION_PATTERN3_ACTUAL = '" + actualValue + "',");

                if (processPlanDetails[i].HARDNESS_1.Trim() != string.Empty)
                {
                    if (processPlanDetails[i].HARDNESS_2.StartsWith("MIN"))
                    {
                        updateQuery.Append("HARDNESS_PATTERN1_CODE = '" + processPlanDetails[i].HARDNESS_1 + "',");
                        updateQuery.Append("HARDNESS_PATTERN1 = '" + processPlanDetails[i].HARDNESS_3 + "',");
                        updateQuery.Append("HARDNESS_PATTERN1_MIN_MAX = '" + 2 + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_CODE = '" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MIN = '" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MAX = '" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN3_ACTUAL = '" + "" + "',");
                    }
                    else if (processPlanDetails[i].HARDNESS_2.StartsWith("MAX"))
                    {
                        updateQuery.Append("HARDNESS_PATTERN1_CODE ='" + processPlanDetails[i].HARDNESS_1 + "',");
                        updateQuery.Append("HARDNESS_PATTERN1 = '" + processPlanDetails[i].HARDNESS_3 + "',");
                        updateQuery.Append("HARDNESS_PATTERN1_MIN_MAX ='" + 1 + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_CODE ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MIN ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MAX ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN3_ACTUAL ='" + "" + "',");
                    }
                    else if (char.IsDigit(processPlanDetails[i].HARDNESS_2[0]) == true)
                    {
                        updateQuery.Append("HARDNESS_PATTERN1_CODE ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN1 = '" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN1_MIN_MAX ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_CODE ='" + processPlanDetails[i].HARDNESS_1 + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MIN ='" + processPlanDetails[i].HARDNESS_2 + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MAX ='" + processPlanDetails[i].HARDNESS_3 + "',");
                        updateQuery.Append("HARDNESS_PATTERN3_ACTUAL ='" + "" + "',");
                    }
                    else if (processPlanDetails[i].HARDNESS_2.StartsWith(_hardness2_Value))
                    {
                        updateQuery.Append("HARDNESS_PATTERN1_CODE ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN1 ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN1_MIN_MAX ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_CODE ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MIN ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MAX ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN3_ACTUAL ='" + 1 + "',");
                    }
                    else
                    {
                        updateQuery.Append("HARDNESS_PATTERN1_CODE ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN1 ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN1_MIN_MAX ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_CODE ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MIN ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN2_MAX ='" + "" + "',");
                        updateQuery.Append("HARDNESS_PATTERN3_ACTUAL ='" + "" + "',");
                    }

                }
                commonFunctionToGetTensileYeildValue(processPlanDetails[i].YEILD);
                updateQuery.Append("YEILD_PATTERN1 = '" + codeValue + "',");
                updateQuery.Append("YEILD_PATTERN1_MIN_MAX = '" + minMaxValue + "',");
                updateQuery.Append("YEILD_PATTERN2_MIN = '" + minValue + "',");
                updateQuery.Append("YEILD_PATTERN2_MAX = '" + maxValue + "',");
                updateQuery.Append("YEILD_PATTERN3_ACTUAL = '" + actualValue + "',");

                getStrPatternElongationPressValue(processPlanDetails[i].PRESS_1, processPlanDetails[i].PRESS_2);
                updateQuery.Append("PRESS_PATTERN1 = '" + codeValue + "',");
                updateQuery.Append("PRESS_PATTERN1_MIN_MAX = '" + minMaxValue + "',");
                updateQuery.Append("PRESS_PATTERN2_MIN = '" + minValue + "',");
                updateQuery.Append("PRESS_PATTERN2_MAX = '" + maxValue + "',");
                updateQuery.Append("PRESS_PATTERN3_ACTUAL = '" + actualValue + "',");
                if (processPlanDetails[i].DECARB_1 == "DMF")
                {
                    updateQuery.Append("DECARB_1 = '" + 1 + "',");
                }
                else if (processPlanDetails[i].DECARB_1 == "DMS")
                {
                    updateQuery.Append("DECARB_1 = '" + 2 + "',");
                }
                else if (processPlanDetails[i].DECARB_1 == "DMT")
                {
                    updateQuery.Append("DECARB_1 = '" + 3 + "',");
                }
                else if (processPlanDetails[i].DECARB_1.Trim() == "")
                {
                    updateQuery.Append("DECARB_1 = '" + "" + "',");
                }
                if (processPlanDetails[i].DECARB_2.Trim() != string.Empty)
                {
                    str = processPlanDetails[i].DECARB_2;
                    index = Convert.ToInt32(CommonFunctionForRemoveChar(str));
                    decimal decarb2 = Convert.ToDecimal(str.Substring(index));
                    updateQuery.Append("DECARB_VAL_1 = '" + decarb2 + "',");
                }
                else
                {
                    updateQuery.Append("DECARB_VAL_1 = '" + "" + "',");
                }
                if (processPlanDetails[i].DECARB_3 == "DMF")
                {
                    updateQuery.Append("DECARB_2 = '" + 1 + "',");
                }
                else if (processPlanDetails[i].DECARB_3 == "DMS")
                {
                    updateQuery.Append("DECARB_2 = '" + 2 + "',");
                }
                else if (processPlanDetails[i].DECARB_3 == "DMT")
                {
                    updateQuery.Append("DECARB_2 = '" + 3 + "',");
                }
                else if (processPlanDetails[i].DECARB_3.Trim() == "")
                {
                    updateQuery.Append("DECARB_2 = '" + "" + "',");
                }
                if (processPlanDetails[i].DECARB_4.Trim() != string.Empty)
                {
                    {
                        str = processPlanDetails[i].DECARB_4;
                        index = Convert.ToInt32(CommonFunctionForRemoveChar(str));
                        decimal decarb4 = Convert.ToDecimal(str.Substring(index));
                        updateQuery.Append("DECARB_VAL_2 = '" + decarb4 + "',");
                    }
                }
                else
                {
                    updateQuery.Append("DECARB_VAL_2 = '" + "" + "',");
                }
                updateQuery.Append("DECARB_3 = '" + "" + "',");
                updateQuery.Append("DECARB_VAL_3 = '" + "" + "',");
                //updateQuery.Append("IMPACT_TEST_PATTERN1 = '" + "" + "',");
                //updateQuery.Append("IMPACT_TEST_PATTERN1_MIN_MAX = '" + "" + "',");
                //updateQuery.Append("IMPACT_TEST_PATTERN2_MIN = '" + "" + "',");
                //updateQuery.Append("IMPACT_TEST_PATTERN2_MAX = '" + "" + "',");
                //updateQuery.Append("IMPACT_TEST_PATTERN3_ACTUAL = '" + "" + "',");

                if (processPlanDetails[i].IMPACT_TEST_PATTERN == 1)
                {
                    updateQuery.Append(" IMPACT_TEST_PATTERN1 = '" + processPlanDetails[i].IMPACT_TEST_MAX + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN1_MIN_MAX =  '" + 1 + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MIN =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MAX = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN3_ACTUAL = '" + "" + "',");
                }
                else if (processPlanDetails[i].IMPACT_TEST_PATTERN == 2)
                {
                    updateQuery.Append(" IMPACT_TEST_PATTERN1 = '" + processPlanDetails[i].IMPACT_TEST_MIN + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN1_MIN_MAX =  '" + 2 + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MIN =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MAX = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN3_ACTUAL = '" + "" + "',");
                }
                else if (processPlanDetails[i].IMPACT_TEST_PATTERN == 3)
                {
                    updateQuery.Append(" IMPACT_TEST_PATTERN1 = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN1_MIN_MAX =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MIN =  '" + processPlanDetails[i].IMPACT_TEST_MIN + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MAX = '" + processPlanDetails[i].IMPACT_TEST_MAX + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN3_ACTUAL = '" + "" + "',");
                }
                else if (processPlanDetails[i].IMPACT_TEST_PATTERN == 4)
                {
                    updateQuery.Append(" IMPACT_TEST_PATTERN1 = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN1_MIN_MAX =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MIN =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MAX = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN3_ACTUAL = '" + 1 + "',");
                }
                else
                {
                    updateQuery.Append(" IMPACT_TEST_PATTERN1 = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN1_MIN_MAX =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MIN =  '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN2_MAX = '" + "" + "',");
                    updateQuery.Append(" IMPACT_TEST_PATTERN3_ACTUAL = '" + "" + "',");
                }
                updateQuery.Append("TEST_SPEC_REMARKS = '" + "" + "',");
                updateQuery.Append("RANK_STD = '" + "" + "',");
                updateQuery.Append("CORNER_RADIUS = '" + "" + "',");
                updateQuery.Append("MBEU = '" + "" + "',");
                updateQuery.Append("HT_CONDITION = '" + "" + "',");//100
                CheckMissMatchData(processPlanDetails[i], HPOResult);
                //PROCESS_1_CD
                if (HPOResult.PROCESS_1_CD != null)
                {

                    if (HPOResult.PROCESS_1_CD != processPlanDetails[i].PROCESS_1_CD)
                    {
                        processPlanDetails[i].PROCESS_1_CD = HPOResult.PROCESS_1_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_1_CD = processPlanDetails[i].PROCESS_1_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_1_CD = null;
                }
                updateQuery.Append("PROCESS_1_CD = '" + processPlanDetails[i].PROCESS_1_CD + "',");

                //PROCESS_2_CD
                if (HPOResult.PROCESS_2_CD != null)
                {
                    if (HPOResult.PROCESS_2_CD != processPlanDetails[i].PROCESS_2_CD)
                    {
                        processPlanDetails[i].PROCESS_2_CD = HPOResult.PROCESS_2_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_2_CD = processPlanDetails[i].PROCESS_2_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_2_CD = null;
                }
                updateQuery.Append("PROCESS_2_CD = '" + processPlanDetails[i].PROCESS_2_CD + "',");

                //PROCESS_3_CD
                if (HPOResult.PROCESS_3_CD != null)
                {
                    if (HPOResult.PROCESS_3_CD != processPlanDetails[i].PROCESS_3_CD)
                    {
                        processPlanDetails[i].PROCESS_3_CD = HPOResult.PROCESS_3_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_3_CD = processPlanDetails[i].PROCESS_3_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_3_CD = null;
                }
                updateQuery.Append("PROCESS_3_CD = '" + processPlanDetails[i].PROCESS_3_CD + "',");

                //PROCESS_4_CD
                if (HPOResult.PROCESS_4_CD != null)
                {
                    if (HPOResult.PROCESS_4_CD != processPlanDetails[i].PROCESS_4_CD)
                    {
                        processPlanDetails[i].PROCESS_4_CD = HPOResult.PROCESS_4_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_4_CD = processPlanDetails[i].PROCESS_4_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_4_CD = null;
                }
                updateQuery.Append("PROCESS_4_CD = '" + processPlanDetails[i].PROCESS_4_CD + "',");

                //PROCESS_5_CD
                if (HPOResult.PROCESS_5_CD != null)
                {
                    if (HPOResult.PROCESS_5_CD != processPlanDetails[i].PROCESS_5_CD)
                    {
                        processPlanDetails[i].PROCESS_5_CD = HPOResult.PROCESS_5_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_5_CD = processPlanDetails[i].PROCESS_5_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_5_CD = null;
                }
                updateQuery.Append("PROCESS_5_CD = '" + processPlanDetails[i].PROCESS_5_CD + "',");

                //PROCESS_6_CD
                if (HPOResult.PROCESS_6_CD != null)
                {
                    if (HPOResult.PROCESS_6_CD != processPlanDetails[i].PROCESS_6_CD)
                    {
                        processPlanDetails[i].PROCESS_6_CD = HPOResult.PROCESS_6_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_6_CD = processPlanDetails[i].PROCESS_6_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_6_CD = null;
                }
                updateQuery.Append("PROCESS_6_CD = '" + processPlanDetails[i].PROCESS_6_CD + "',");

                //PROCESS_7_CD
                if (HPOResult.PROCESS_7_CD != null)
                {
                    if (HPOResult.PROCESS_7_CD != processPlanDetails[i].PROCESS_7_CD)
                    {
                        processPlanDetails[i].PROCESS_7_CD = HPOResult.PROCESS_7_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_7_CD = processPlanDetails[i].PROCESS_7_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_7_CD = null;
                }
                updateQuery.Append("PROCESS_7_CD = '" + processPlanDetails[i].PROCESS_7_CD + "',");

                //PROCESS_8_CD
                if (HPOResult.PROCESS_8_CD != null)
                {
                    if (HPOResult.PROCESS_8_CD != processPlanDetails[i].PROCESS_8_CD)
                    {
                        processPlanDetails[i].PROCESS_8_CD = HPOResult.PROCESS_8_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_8_CD = processPlanDetails[i].PROCESS_8_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_8_CD = null;
                }
                updateQuery.Append("PROCESS_8_CD = '" + processPlanDetails[i].PROCESS_8_CD + "',");

                //PROCESS_9_CD
                if (HPOResult.PROCESS_9_CD != null)
                {
                    if (HPOResult.PROCESS_9_CD != processPlanDetails[i].PROCESS_9_CD)
                    {
                        processPlanDetails[i].PROCESS_9_CD = HPOResult.PROCESS_9_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_9_CD = processPlanDetails[i].PROCESS_9_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_9_CD = null;
                }
                updateQuery.Append("PROCESS_9_CD = '" + processPlanDetails[i].PROCESS_9_CD + "',");

                //PROCESS_10_CD
                if (HPOResult.PROCESS_10_CD != null)
                {
                    if (HPOResult.PROCESS_10_CD != processPlanDetails[i].PROCESS_10_CD)
                    {
                        processPlanDetails[i].PROCESS_10_CD = HPOResult.PROCESS_10_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_10_CD = processPlanDetails[i].PROCESS_10_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_10_CD = null;
                }
                updateQuery.Append("PROCESS_10_CD = '" + processPlanDetails[i].PROCESS_10_CD + "',");

                //PROCESS_11_CD
                if (HPOResult.PROCESS_11_CD != null)
                {
                    if (HPOResult.PROCESS_11_CD != processPlanDetails[i].PROCESS_11_CD)
                    {
                        processPlanDetails[i].PROCESS_11_CD = HPOResult.PROCESS_11_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_11_CD = processPlanDetails[i].PROCESS_11_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_11_CD = null;
                }
                updateQuery.Append("PROCESS_11_CD = '" + processPlanDetails[i].PROCESS_11_CD + "',");

                //PROCESS_12_CD
                if (HPOResult.PROCESS_12_CD != null)
                {
                    if (HPOResult.PROCESS_12_CD != processPlanDetails[i].PROCESS_12_CD)
                    {
                        processPlanDetails[i].PROCESS_12_CD = HPOResult.PROCESS_12_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_12_CD = processPlanDetails[i].PROCESS_12_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_12_CD = null;
                }
                updateQuery.Append("PROCESS_12_CD = '" + processPlanDetails[i].PROCESS_12_CD + "',");

                //PROCESS_13_CD
                if (HPOResult.PROCESS_13_CD != null)
                {
                    if (HPOResult.PROCESS_13_CD != processPlanDetails[i].PROCESS_13_CD)
                    {
                        processPlanDetails[i].PROCESS_13_CD = HPOResult.PROCESS_13_CD;
                    }
                    else
                    {
                        processPlanDetails[i].PROCESS_13_CD = processPlanDetails[i].PROCESS_13_CD;
                    }
                }
                else
                {
                    processPlanDetails[i].PROCESS_13_CD = null;
                }
                updateQuery.Append("PROCESS_13_CD = '" + processPlanDetails[i].PROCESS_13_CD + "',");
                updateQuery.Append("MACHINE_CD_1 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_2 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_3 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_4 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_5 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_6 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_7 = '" + "" + "',");// machinecd
                updateQuery.Append("MACHINE_CD_8 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_9 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_10 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_11 = '" + "" + "',");
                updateQuery.Append("MACHINE_CD_12 = '" + "" + "',");//125
                updateQuery.Append("MACHINE_CD_13 = '" + "" + "',");
                if (processPlanDetails[i].PROD_POINT_1.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_1 = '" + processPlanDetails[i].PROD_POINT_1 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_1 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_2.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_2 = '" + processPlanDetails[i].PROD_POINT_2 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_2 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_3.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_3 = '" + processPlanDetails[i].PROD_POINT_3 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_3 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_4.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_4 = '" + processPlanDetails[i].PROD_POINT_4 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_4 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_5.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_5 = '" + processPlanDetails[i].PROD_POINT_5 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_5 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_6.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_6 = '" + processPlanDetails[i].PROD_POINT_6 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_6 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_7.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_7 = '" + processPlanDetails[i].PROD_POINT_7 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_7 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_8.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_8 = '" + processPlanDetails[i].PROD_POINT_8 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_8 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_9.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_9 = '" + processPlanDetails[i].PROD_POINT_9 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_9 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_10.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_10 = '" + processPlanDetails[i].PROD_POINT_10 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_10 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_11.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_11 = '" + processPlanDetails[i].PROD_POINT_11 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_11 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_12.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_12 = '" + processPlanDetails[i].PROD_POINT_12 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_12 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_13.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_13 = '" + processPlanDetails[i].PROD_POINT_13 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_13 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_1.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_1 = '" + processPlanDetails[i].PROD_POINT_1 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_1 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_2.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_2 = '" + processPlanDetails[i].PROD_POINT_2 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_2 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_3.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_3 = '" + processPlanDetails[i].PROD_POINT_3 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_3 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_4.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_4 = '" + processPlanDetails[i].PROD_POINT_4 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_4 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_5.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_5 = '" + processPlanDetails[i].PROD_POINT_5 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_5 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_6.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_6 = '" + processPlanDetails[i].PROD_POINT_6 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_6 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_7.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_7 = '" + processPlanDetails[i].PROD_POINT_7 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_7 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_8.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_8 = '" + processPlanDetails[i].PROD_POINT_8 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_8 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_9.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_9 = '" + processPlanDetails[i].PROD_POINT_9 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_9 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_10.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_10 = '" + processPlanDetails[i].PROD_POINT_10 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_10 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_11.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_11 = '" + processPlanDetails[i].PROD_POINT_11 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_11 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_12.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_12 = '" + processPlanDetails[i].PROD_POINT_12 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_12 = '" + "" + "',");
                }
                if (processPlanDetails[i].PROD_POINT_13.Trim() != string.Empty)
                {
                    updateQuery.Append("PROD_POINT_SUB_13 = '" + processPlanDetails[i].PROD_POINT_13 + "',");
                }
                else
                {
                    updateQuery.Append("PROD_POINT_SUB_13 = '" + "" + "',");
                }
                updateQuery.Append("PROCESS_COLOR_KUBUN_1 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_2 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_3 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_4 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_5 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_6 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_7 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_8 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_9 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_10 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_11 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_12 = '" + "" + "',");
                updateQuery.Append("PROCESS_COLOR_KUBUN_13 = '" + "" + "',");
                updateQuery.Append("MATL_TYPE_CBK = '" + "" + "',");
                updateQuery.Append("MATL_NAME_MAK = '" + processPlanDetails[i].MATL_NAME_MAK + "',");
                if (processPlanDetails[i].MATL_SHAPE.Trim() != string.Empty)
                {
                    updateQuery.Append("MATL_SHAPE = '" + processPlanDetails[i].MATL_SHAPE + "',");
                }
                else
                {
                    updateQuery.Append("MATL_SHAPE = '" + "" + "',");
                }
                updateQuery.Append("MATL_DIM_D = '" + processPlanDetails[i].HOST_MATL_DIM_D + "',");
                updateQuery.Append("MATL_DIM_W = '" + "" + "',");
                if (processPlanDetails[i].MATL_DIM_OTHER.Trim() != string.Empty)
                {
                    updateQuery.Append("MATL_DIM_OTHER = '" + processPlanDetails[i].MATL_DIM_OTHER + "',");
                }
                else
                {
                    updateQuery.Append("MATL_DIM_OTHER = '" + "" + "',");
                }
                updateQuery.Append("MATL_LENGTH = '" + "" + "',");
                updateQuery.Append("KAKOUIARI_QTY = '" + processPlanDetails[i].KAKOUIARI_QTY + "',");
                PrintDate = Convert.ToString(processPlanDetails[i].PRINT_DATE).Substring(0, 2);
                DateTime createdDate = Convert.ToDateTime(processPlanDetails[i].CREATED_DATE);
                var createdDateyear = createdDate.Year;
                var Created_Date_Year = Convert.ToString(createdDateyear).Substring(0, 2);

                if (PrintDate.Length >= 2)
                {
                    Date_Year = Created_Date_Year + PrintDate;
                }
                var DeliveryDate = Convert.ToString(processPlanDetails[i].HOST_DELIVERY_DATE);
                if (DeliveryDate.Length < 4)
                {
                    Del_Date = "0" + Convert.ToString(processPlanDetails[i].HOST_DELIVERY_DATE);
                    Del_Date_Month = Del_Date.Substring(0, 2);
                    Del_Date_Date = Del_Date.Substring(2, 2);
                    Delivery_Date = Date_Year + "/" + Del_Date_Month + "/" + Del_Date_Date + " " + "00.00";
                }
                else
                {
                    Del_Date = Convert.ToString(processPlanDetails[i].HOST_DELIVERY_DATE);
                    Del_Date_Month = Del_Date.Substring(0, 2);
                    Del_Date_Date = Del_Date.Substring(2, 2);
                    Delivery_Date = Date_Year + "/" + Del_Date_Month + "/" + Del_Date_Date + " " + "00.00";
                }
                Print_Date_Month_date = Convert.ToString(processPlanDetails[i].PRINT_DATE).Substring(2, 4);
                Print_Date_Month = Print_Date_Month_date.Substring(0, 2);
                Print_Date_Date = Print_Date_Month_date.Substring(2, 2);
                Print_date = Date_Year + "/" + Print_Date_Month + "/" + Print_Date_Date + " " + "00.00";

                updateQuery.Append("DELIVERY_DATE = TO_DATE('" + Delivery_Date + "', 'yyyy/mm/dd HH24:MI:SS'),");
                updateQuery.Append("NO_OF_UNITS = '" + processPlanDetails[i].HPO_INZU + "',");
                if (processPlanDetails[i].UNIT != 0)
                {
                    updateQuery.Append("UNIT = '" + processPlanDetails[i].UNIT + "',");
                }
                else
                {
                    updateQuery.Append("UNIT = '" + "" + "',");
                }
                updateQuery.Append("ACT_ALLOC_QTY = '" + processPlanDetails[i].ACT_ALLOC_QTY + "',");
                updateQuery.Append("ACT_ALLOC_DATE = TO_DATE('" + Convert.ToDateTime(processPlanDetails[i].ACT_ALLOC_DATE).ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS) + "', 'yyyy/mm/dd HH24:MI:SS'),");
                updateQuery.Append("CHARGE = '" + processPlanDetails[i].CHARGE + "',");
                updateQuery.Append("PROCESS_PLAN_REMARK = '" + "" + "',");
                if (processPlanDetails[i].PROCESS_PLAN_KUBUN != 0)
                {
                    updateQuery.Append("PROCESS_PLAN_KUBUN = '" + processPlanDetails[i].PROCESS_PLAN_KUBUN + "',");
                }
                else
                {
                    updateQuery.Append("PROCESS_PLAN_KUBUN = '" + "" + "',");
                }
                updateQuery.Append("PROD_MNGMNT_KUBUN = '" + "" + "',");
                updateQuery.Append("SPCL_MNGMNT_KUBUN = '" + "" + "',");
                updateQuery.Append("STORAGE_MNGMNT_KUBUN = '" + "" + "',");
                updateQuery.Append("PROCESS_PLAN_PRINT_DATE = TO_DATE('" + Print_date + "', 'yyyy/mm/dd HH24:MI:SS'),");
                updateQuery.Append("PROCESS_PLAN_CMPL_DATE = '" + "" + "',");
                if (processPlanDetails[i].USER_ID != 0)
                {
                    updateQuery.Append("USER_ID = '" + processPlanDetails[i].USER_ID + "',");
                }
                else
                {
                    updateQuery.Append("USER_ID = '" + "" + "',");
                }
                updateQuery.Append("INPUT_LOCATION_CD = (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + processPlanDetails[i].USER_ID + ") ,");
                //updateQuery.Append(" CREATED_DATE = SYSDATE,");
                updateQuery.Append(" UPDATED_DATE = SYSDATE,");
                updateQuery.Append(" PROCESS_PLAN_START_DATE = SYSDATE,");
                //updateQuery.Append("FIRST_PROCESS_CD = '" + processPlanDetails[i].PROCESS_CD + "',"); //code commented on 02/12/2025 as per client requirement
                updateQuery.Append("EMPLOYEE_CD = '" + processPlanDetails[i].EMPLOYEE_CD + "',");
                if (Convert.ToString(processPlanDetails[i].CREATED_DATE) != null)
                {
                    var chubbanYear = "";

                    var created_Year = Convert.ToString(createdDateyear).Substring(3, 1);//for 2019
                    var chubban_Year = processPlanDetails[i].CHHUBAN_YEAR;
                    var Year = Convert.ToString(createdDateyear).Substring(2, 1);

                    if (created_Year == "9" && chubban_Year == "0")
                    {
                        int chubbanYearNew = Convert.ToInt32(Year) + 1; //year=2019+1,
                        chubbanYear = chubbanYearNew + chubban_Year;
                        updateQuery.Append("CHUUBAN_YEAR = '" + chubbanYear + "',");
                    }
                    else
                    {
                        chubbanYear = Year + chubban_Year;
                        updateQuery.Append("CHUUBAN_YEAR = '" + chubbanYear + "',");
                    }
                }
                updateQuery.Append("MAKER_CD = '" + processPlanDetails[i].MAKER_CD + "', ");
                updateQuery.Append("KAKOUIRAI_REMARK = '" + processPlanDetails[i].KAKOUIRAI_REMARK + "' ");
                updateQuery.Append("WHERE PROCESS_PLAN_CD = '" + process_Plan_CD + "'");
                ExecuteNonQuery(updateQuery.ToString());
                result = "Data Updated Successfully";

                if (result == "Data Updated Successfully")
                {
                    //if (!string.IsNullOrEmpty(processPlanDetails[i].REMARK))
                    //{
                    StringBuilder selectMsgCntQuery = new StringBuilder();
                    selectMsgCntQuery.Append(" SELECT  COUNT(*) FROM TRN_MESSAGE");
                    selectMsgCntQuery.Append(" WHERE PROCESS_PLAN_CD = " + process_Plan_CD);
                    selectMsgCntQuery.Append(" AND MACHINE_CD IS NULL ");
                    selectMsgCntQuery.Append(" AND PROCESS_KUBUN_ID IS NULL ");
                    //selectMsgCntQuery.Append(" AND OPERATOR_CD IS NULL ");
                    int msgCount = Convert.ToInt16(ExecuteScalar(selectMsgCntQuery.ToString()));
                    if (msgCount > 0 && !string.IsNullOrEmpty(processPlanDetails[i].REMARK))
                    {
                        StringBuilder updateMsgQuery = new StringBuilder();
                        updateMsgQuery.Append(" UPDATE TRN_MESSAGE SET ");
                        updateMsgQuery.Append(" OPERATOR_CD = '" + processPlanDetails[i].EMPLOYEE_CD + "',");
                        updateMsgQuery.Append(" MSG_CONTENTS ='" + processPlanDetails[i].REMARK + "',");
                        updateMsgQuery.Append(" USER_ID = '" + processPlanDetails[i].USER_ID + "',");
                        updateMsgQuery.Append(" INPUT_LOCATION_CD = (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID=" + processPlanDetails[i].USER_ID + ") , ");
                        updateMsgQuery.Append(" UPDATED_DATE = SYSDATE ");
                        updateMsgQuery.Append(" WHERE PROCESS_PLAN_CD = " + process_Plan_CD);
                        updateMsgQuery.Append(" AND MACHINE_CD IS NULL ");
                        updateMsgQuery.Append(" AND PROCESS_KUBUN_ID IS NULL ");
                        // updateMsgQuery.Append(" AND OPERATOR_CD IS NULL ");
                        ExecuteNonQuery(updateMsgQuery.ToString());
                        result = "Data Updated Successfully";
                    }
                    else if (msgCount == 0 && !string.IsNullOrEmpty(processPlanDetails[i].REMARK))
                    {
                        StringBuilder insertMessage = new StringBuilder();
                        insertMessage.Append(" INSERT INTO TRN_MESSAGE ( ");
                        insertMessage.Append(" PROCESS_PLAN_CD ");
                        insertMessage.Append(", OPERATOR_CD ");
                        insertMessage.Append(", MSG_DATE ");
                        insertMessage.Append(", MSG_CONTENTS ");
                        insertMessage.Append(", INPUT_LOCATION_CD ");
                        insertMessage.Append(", USER_ID ");
                        insertMessage.Append(", CREATED_DATE ");
                        insertMessage.Append(", UPDATED_DATE ");

                        insertMessage.Append(") VALUES (");
                        insertMessage.Append("'" + process_Plan_CD + "',");
                        insertMessage.Append(" '" + processPlanDetails[i].EMPLOYEE_CD + "',");
                        insertMessage.Append("  SYSDATE, ");
                        insertMessage.Append(" '" + processPlanDetails[i].REMARK + "',");
                        insertMessage.Append(" (SELECT LOCATION_CD FROM MST_USER_LOGIN WHERE USER_ID = " + processPlanDetails[i].USER_ID + "), ");
                        insertMessage.Append(" '" + processPlanDetails[i].USER_ID + "',");
                        insertMessage.Append(" SYSDATE,");
                        insertMessage.Append(" SYSDATE ");
                        insertMessage.Append(")");
                        ExecuteNonQuery(insertMessage.ToString());
                        result = "Data Updated Successfully";
                    }
                    else if (msgCount > 0 && string.IsNullOrEmpty(processPlanDetails[i].REMARK))
                    {
                        StringBuilder deleteSqlForMsg = new StringBuilder();
                        deleteSqlForMsg.Append("DELETE  FROM TRN_MESSAGE ");
                        deleteSqlForMsg.Append("WHERE ");
                        deleteSqlForMsg.Append("PROCESS_PLAN_CD = '" + process_Plan_CD + "' ");
                        deleteSqlForMsg.Append("AND MACHINE_CD IS NULL ");
                        deleteSqlForMsg.Append("AND PROCESS_KUBUN_ID IS NULL ");
                        //deleteSqlForMsg.Append("AND OPERATOR_CD IS NULL ");
                        ExecuteNonQuery(deleteSqlForMsg.ToString());
                        result = "Data Updated Successfully";
                    }

                    //}



                }


                //result = "success";
                //if (result == "success") //code commented on 02/12/2025 as per client requirement
                //{
                //    StringBuilder selectProessPlanData = new StringBuilder();
                //    selectProessPlanData.Append(" SELECT  ");
                //    selectProessPlanData.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD_HOST,");
                //    selectProessPlanData.Append(" PP.ACT_ALLOC_QTY,");
                //    selectProessPlanData.Append(" PP.NO_OF_UNITS,");
                //    selectProessPlanData.Append(" PP.UNIT,");
                //    selectProessPlanData.Append(" PP.LOCATION_CD, ");
                //    selectProessPlanData.Append(" PP.FIRST_PROCESS_CD AS NEXT_PROCESS_CD ");
                //    selectProessPlanData.Append(" FROM TRN_PROCESS_PLAN PP ");
                //    selectProessPlanData.Append(" WHERE CHUUBAN = '" + processPlanDetails[i].CHUUBAN + "' AND GYOUBAN = '" + processPlanDetails[i].GYOUBAN + "' AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
                //    var ProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectProessPlanData.ToString());
                //    result = UpdateIntoWipProcessPlan(Convert.ToInt32(ProcessPlanData.PROCESS_PLAN_CD_HOST), Convert.ToInt32(ProcessPlanData.LOCATION_CD), Convert.ToDecimal(ProcessPlanData.NO_OF_UNITS), Convert.ToInt32(ProcessPlanData.UNIT), Convert.ToInt32(ProcessPlanData.ACT_ALLOC_QTY), Convert.ToInt32(ProcessPlanData.NEXT_PROCESS_CD));

                //}

            }

            return result;
        }
        /// CREATED BY : Namrata Ubale on 2025/10/15       
        /// used screens: ProcessPlanHandoverEntry
        /// </summary>
        /// <returns>
        /// Update data into table TRN_WIP_STATUS
        /// </returns>
        /// 

        public string UpdateIntoWipProcessPlan(int processPlanCD, int locationCD, decimal noOfUnits, int unit, int actAllocQty, int processCD)
        {
            StringBuilder updateSql = new StringBuilder();
            updateSql.Append(" UPDATE TRN_WIP_STATUS SET");
            updateSql.Append(" LOCATION_CD = '" + locationCD + "',");
            updateSql.Append(" TABA_NO = '" + 1 + "',");
            updateSql.Append(" WIP_NO_OF_UNITS = '" + noOfUnits + "',");
            updateSql.Append(" UNIT = '" + unit + "',");
            updateSql.Append(" WIP_QTY = '" + actAllocQty + "',");
            updateSql.Append(" NEXT_PROCESS = '" + processCD + "'");
            updateSql.Append(" WHERE");
            updateSql.Append(" PROCESS_PLAN_CD =  '" + processPlanCD + "' ");
            ExecuteNonQuery(updateSql.ToString());
            result = "Data Updated Successfully";
            return result;

        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2025/12/08       
        /// used screens: ProcessPlanHandoverHost
        /// </summary>
        /// <returns>
        /// Fetch data by using OrNo using table TRN_PROCESS_PLAN and HTMP_PRODUCT_ORDER
        /// </returns>
        /// 
        public List<GridSearchFields> GetProcessPlanDetails(List<GridSearchFields> processPlanDetails)
        {
            List<GridSearchFields> result = new List<GridSearchFields>();

            int count = 0;
            int countForHost = 0;
            int completeDateCnt = 0;
            int nifudaCnt = 0;
            for (int i = 0; i < processPlanDetails.Count; i++)
            {
                if (processPlanDetails[i].DISPLAYPROCESSPLANDATA == "beforeProcessStart")//
                {
                    StringBuilder selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT ");
                    selectQuery.Append(" HPO.CUST_SHORT_NAME AS CUSTOMER_NAME,");
                    selectQuery.Append(" HPO.PROD_NAME_CUST,");
                    selectQuery.Append(" HPO.MATL_NAME_INT, ");
                    selectQuery.Append(" HPO.SHAPE,");
                    selectQuery.Append(" HPO.DIM_D AS PROD_DIM_D,");
                    selectQuery.Append(" HPO.DIM_OTHER AS PROD_DIM_OTHER_STRING,");
                    selectQuery.Append(" HPO.DIM_TOL,");
                    selectQuery.Append(" SUBSTR(HPO.DIM_TOL, 1, INSTR(HPO.DIM_TOL, ',') - 1) AS DIM_TOL_MAX_SPLIT,");
                    selectQuery.Append(" SUBSTR(HPO.DIM_TOL, INSTR(HPO.DIM_TOL, ',') + 1) AS DIM_TOL_MIN_SPLIT,");
                    selectQuery.Append(" HPO.DIM_TOL_1 AS DIM_TOL_MAX,");
                    selectQuery.Append(" HPO.DIM_TOL_2 AS DIM_TOL_MIN,");
                    selectQuery.Append(" HPO.DIM_TOL_SIGN_1 AS DIM_TOL_MAX_CHR,");
                    selectQuery.Append(" HPO.DIM_TOL_SIGN_2 AS DIM_TOL_MIN_CHR,");
                    selectQuery.Append(" HPO.LEN_TOL,");
                    selectQuery.Append(" SUBSTR(HPO.LEN_TOL, 1, INSTR(HPO.LEN_TOL, ',') - 1) AS LEN_TOL_MAX_SPLIT,");
                    selectQuery.Append(" SUBSTR(HPO.LEN_TOL, INSTR(HPO.LEN_TOL, ',') + 1)     AS LEN_TOL_MIN_SPLIT,");
                    selectQuery.Append(" HPO.LEN_TOL_1 AS HOST_LENGTH_TOL_MAX,");
                    selectQuery.Append(" HPO.LEN_TOL_2 AS HOST_LENGTH_TOL_MIN,");
                    selectQuery.Append(" HPO.LEN_TOL_SIGN_1 AS LENGTH_TOL_MAX_CHR,");
                    selectQuery.Append(" HPO.LEN_TOL_SIGN_2 AS LENGTH_TOL_MIN_CHR,");
                    selectQuery.Append(" CASE WHEN REGEXP_LIKE(HPO.LENGTH,'[^0-9]+') THEN NULL ELSE HPO.LENGTH END AS PROD_LENGTH,");
                    selectQuery.Append(" CASE WHEN REGEXP_LIKE(HPO.LENGTH,'[^0-9]+') THEN TRIM(HPO.LENGTH) ELSE NULL END AS LENGTH_OTHER,");
                    selectQuery.Append(" HPO.CHARGE,");
                    selectQuery.Append(" HPO.INZU AS HPO_INZU,");
                    selectQuery.Append(" HPO.INZU_UNIT AS HOST_UNIT_TYPE_STRING,");
                    selectQuery.Append(" HPO.ACT_ALLOC_QTY,");
                    selectQuery.Append(" TO_DATE(HPO.ACT_ALLOC_DATE,'YYYY/MM/DD') AS HOST_ACT_ALLOC_DATE,");
                    selectQuery.Append(" HPO.CUSTOMER_CODE,");
                    selectQuery.Append(" HPO.RECEIVER_SHORT_NAME AS RECEIVER_NAME,");
                    selectQuery.Append(" HPO.MAKER_CODE,");
                    selectQuery.Append(" HPO.MAKER_NAME,");
                    selectQuery.Append(" HPO.MATL_NAME_MAK, ");
                    selectQuery.Append(" HPO.DEPT_CODE, ");
                    selectQuery.Append(" HPO.PACKING AS PACKING_NAME,");
                    selectQuery.Append(" HPO.CHUUBAN, ");
                    selectQuery.Append(" HPO.GYOUBAN AS GYOUBAN_HOST,");
                    selectQuery.Append(" HPO.PROD_SPEC_NO_1 AS PROD_SPEC_NO1, ");
                    selectQuery.Append(" HPO.PROD_SPEC_NO_2 AS PROD_SPEC_NO2, ");
                    selectQuery.Append(" HPO.CHANGE_VER_NUMBER AS VER, ");
                    selectQuery.Append(" HPO.STD_DELIVERAY_DATE AS STD_DEL_PERIOD,");

                    selectQuery.Append(" HPO.COIL_DIA_INT,");
                    selectQuery.Append(" HPO.COIL_DIA_EXT,");
                    selectQuery.Append(" HPO.COIL_WND_DIR,");
                    //selectQuery.Append(" HPO.BEND_TOL AS MAGARI_BEND_TOL_STRING,");
                    selectQuery.Append(" REGEXP_REPLACE(HPO.BEND_TOL, '[^0-9.]', '') AS MAGARI_BEND_TOL_STRING,");
                    selectQuery.Append(" HPO.MAGARI_KUBUN AS MAGARI_KUBUN_STRING,");
                    //selectQuery.Append(" REGEXP_REPLACE(HPO.MAGARI_SPAN, '[^0-9]', '')AS MAGARI_SPAN_STRING,");
                    selectQuery.Append(" CASE WHEN REGEXP_REPLACE(HPO.MAGARI_SPAN, '[^0-9M]', '') = '1M' THEN '1000'");
                    selectQuery.Append(" ELSE REGEXP_REPLACE(HPO.MAGARI_SPAN, '[^0-9M]', '') END AS MAGARI_SPAN_STRING,");
                    selectQuery.Append(" HPO.UNIT_WT, ");
                    selectQuery.Append(" HPO.EXT_TEXTURE, ");
                    selectQuery.Append(" HPO.COLOR_ID AS COLOR_NAME, ");
                    selectQuery.Append(" HPO.DISPLAY, ");
                    selectQuery.Append(" HPO.STAMP,");
                    selectQuery.Append(" HPO.TEST_SPECIMEN, ");
                    selectQuery.Append(" HPO.TENSILE_STRENGTH AS TENSILE_STRN_PATTERN1_STRING,");
                    selectQuery.Append(" HPO.ELONGATION_1,");
                    selectQuery.Append(" HPO.ELONGATION_2,");
                    selectQuery.Append(" HPO.HARDNESS_1,");
                    selectQuery.Append(" HPO.HARDNESS_2,");
                    selectQuery.Append(" HPO.HARDNESS_3, ");
                    selectQuery.Append(" HPO.YEILD,");
                    selectQuery.Append(" HPO.PRESS_1,HPO.PRESS_2,");
                    selectQuery.Append(" HPO.DECARB_1,");
                    selectQuery.Append(" HPO.DECARB_2,");
                    selectQuery.Append(" HPO.DECARB_3,");
                    selectQuery.Append(" HPO.DECARB_4,");
                    selectQuery.Append(" HPO.PROCESS_1 AS PROCESS_NAME1,");
                    selectQuery.Append(" HPO.PROCESS_2 AS PROCESS_NAME2,");
                    selectQuery.Append(" HPO.PROCESS_3 AS PROCESS_NAME3,");
                    selectQuery.Append(" HPO.PROCESS_4 AS PROCESS_NAME4,");
                    selectQuery.Append(" HPO.PROCESS_5 AS PROCESS_NAME5,");
                    selectQuery.Append(" HPO.PROCESS_6 AS PROCESS_NAME6,");
                    selectQuery.Append(" HPO.PROCESS_7 AS PROCESS_NAME7,");
                    selectQuery.Append(" HPO.PROCESS_8 AS PROCESS_NAME8,");
                    selectQuery.Append(" HPO.PROCESS_9 AS PROCESS_NAME9,");
                    selectQuery.Append(" HPO.PROCESS_10 AS PROCESS_NAME10,");
                    selectQuery.Append(" HPO.PROCESS_11 AS PROCESS_NAME11,");
                    selectQuery.Append(" HPO.PROCESS_12 AS PROCESS_NAME12,");
                    selectQuery.Append(" HPO.PROCESS_13 AS PROCESS_NAME13,");
                    selectQuery.Append(" HPO.PROD_POINT_1,");
                    selectQuery.Append(" HPO.PROD_POINT_2,");
                    selectQuery.Append(" HPO.PROD_POINT_3,");
                    selectQuery.Append(" HPO.PROD_POINT_4,");
                    selectQuery.Append(" HPO.PROD_POINT_5,");
                    selectQuery.Append(" HPO.PROD_POINT_6,");
                    selectQuery.Append(" HPO.PROD_POINT_7,");
                    selectQuery.Append(" HPO.PROD_POINT_8,");
                    selectQuery.Append(" HPO.PROD_POINT_9,");
                    selectQuery.Append(" HPO.PROD_POINT_10,");
                    selectQuery.Append(" HPO.PROD_POINT_11,");
                    selectQuery.Append(" HPO.PROD_POINT_12,");
                    selectQuery.Append(" HPO.PROD_POINT_13,");
                    selectQuery.Append(" HPO.MATL_SHAPE AS MATL_SHAPE, ");
                    selectQuery.Append(" HPO.MATL_DIM_D AS HOST_MATL_DIM_D, ");
                    selectQuery.Append(" HPO.MATL_DIM_OTHER AS MATL_DIM_OTHER, ");
                    selectQuery.Append(" HPO.ORG_QTY AS KAKOUIARI_QTY, ");//add into save method 
                    selectQuery.Append(" HPO.DELIVERY_DATE AS HOST_DELIVERY_DATE,");
                    selectQuery.Append(" HPO.PRINT_DATE, ");
                    selectQuery.Append(" TO_DATE(HPO.CREATED_DATETIME,'YYYY/MM/DD HH24:MI:SS') AS CREATED_DATE, ");
                    selectQuery.Append(" HPO.CHHUBAN_YEAR, ");
                    selectQuery.Append(" HPO.INCHARGE_CODE, ");
                    selectQuery.Append(" CONCAT(HPO.CHUUBAN,HPO.GYOUBAN) AS CHUUBAN_GYOUBAN, ");
                    selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(HPO.CHUUBAN,HPO.GYOUBAN) AS CHUUBAN_GYOUBAN_STRING, ");
                    selectQuery.Append(" HPO.FUHEN_NO, ");
                    selectQuery.Append(" HPO.HOST_REFERENCE_NO, ");
                    selectQuery.Append(" CASE WHEN SUBSTR(SPL_PROCESS, 1, 1) > 0 THEN  SUBSTR(SPL_PROCESS, 1, 1) ELSE NULL END AS CHAMFER, ");
                    selectQuery.Append(" CASE WHEN NULLIF(TRIM(HPO.KAKOUIRAI_REMARK_1), '') IS NOT NULL AND NULLIF(TRIM(HPO.KAKOUIRAI_REMARK_2), '') IS NOT NULL ");
                    selectQuery.Append(" THEN TRIM(HPO.KAKOUIRAI_REMARK_1) || ' ' || TRIM(HPO.KAKOUIRAI_REMARK_2) ");
                    selectQuery.Append(" WHEN NULLIF(TRIM(HPO.KAKOUIRAI_REMARK_1), '') IS NOT NULL THEN TRIM(HPO.KAKOUIRAI_REMARK_1)");
                    selectQuery.Append(" WHEN NULLIF(TRIM(HPO.KAKOUIRAI_REMARK_2), '') IS NOT NULL THEN TRIM(HPO.KAKOUIRAI_REMARK_2) ");
                    selectQuery.Append(" ELSE NULL ");
                    selectQuery.Append(" END AS KAKOUIRAI_REMARK, ");
                    selectQuery.Append(" CASE SUBSTR(HPO.PROD_SPEC_NO_2, 0, 1) ");
                    selectQuery.Append(" WHEN '1' THEN 'DB' ");
                    selectQuery.Append(" WHEN '2' THEN 'HTDB' ");
                    selectQuery.Append(" WHEN '3' THEN 'DC' ");
                    selectQuery.Append(" WHEN '5' THEN 'DB' ");
                    selectQuery.Append(" WHEN '6' THEN 'HTDB' ");
                    selectQuery.Append(" ELSE NULL ");
                    selectQuery.Append(" END AS PROD_CONDITION_STRING ");
                    selectQuery.Append(" FROM hsk_replication.HTMP_PRODUCT_ORDER HPO ");
                    selectQuery.Append(" WHERE CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "' ");
                    var ProcessPlanDataForHost = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                    if (ProcessPlanDataForHost != null)
                    {

                        result.Add(ProcessPlanDataForHost);

                    }

                }
                else
                {
                    StringBuilder selectQuery = new StringBuilder();
                    selectQuery.Append(" SELECT CUST.SHORT_NAME AS CUSTOMER_NAME, ");
                    selectQuery.Append(" PP.PROD_NAME_CUST,");
                    selectQuery.Append(" PP.PROD_SHAPE,");
                    selectQuery.Append(" PP.PROD_DIM_D,");
                    selectQuery.Append(" PP.PROD_DIM_W,");
                    selectQuery.Append(" TO_CHAR(PP.PROD_DIM_OTHER) AS PROD_DIM_OTHER_STRING,");
                    selectQuery.Append(" PP.PROD_LENGTH,");
                    selectQuery.Append(" PP.LENGTH_OTHER,");
                    selectQuery.Append(" PP.HT_CONDITION,");
                    selectQuery.Append(" PP.CHARGE,");
                    selectQuery.Append(" PP.ACT_ALLOC_CD,");
                    selectQuery.Append(" PP.ACT_ALLOC_QTY,");
                    selectQuery.Append(" TO_CHAR(PP.ACT_ALLOC_DATE,'YYYY/MM/DD') AS ACT_ALLOC_DATE_STRING,");
                    selectQuery.Append(" PP.NO_OF_UNITS,");
                    selectQuery.Append(" PP.UNIT,");
                    selectQuery.Append(" PP.PROD_SPEC_NO1,");
                    selectQuery.Append(" PP.PROD_SPEC_NO2,");
                    selectQuery.Append(" PP.PROCESS_PLAN_CD AS PROCESS_PLAN_CD_HOST,");
                    selectQuery.Append(" PP.LOCATION_CD, ");
                    selectQuery.Append(" PP.FIRST_PROCESS_CD AS NEXT_PROCESS_CD, ");
                    selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || DIM_TOL_MIN_CHR AS DIM_TOL, ");
                    selectQuery.Append(" PP.LENGTH_TOL_MAX_CHR || ',' || LENGTH_TOL_MIN_CHR AS LEN_TOL, ");
                    selectQuery.Append(" INT.INTERNAL_NAME AS INTERNAL_NAME_STRING, ");
                    selectQuery.Append(" CONCAT(CHUUBAN,CASE WHEN LENGTH(TRIM(GYOUBAN))=1 THEN '0'||GYOUBAN END) AS CHUUBAN_GYOUBAN, ");
                    selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(CHUUBAN,CASE WHEN LENGTH(TRIM(GYOUBAN))=1 THEN '0'||GYOUBAN END) AS CHUUBAN_GYOUBAN_STRING,");
                    selectQuery.Append(" MSG.MSG_CONTENTS AS REMARK,");
                    selectQuery.Append(" COND.CONDITION_DESCRIPTION AS PROD_CONDITION_STRING ");
                    selectQuery.Append(" FROM TRN_PROCESS_PLAN PP ");
                    selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUST ON CUST.CUSTOMER_CD = PP.CUSTOMER_CD ");
                    selectQuery.Append(" LEFT JOIN MST_INTERNAL_NAME INT ON INT.INTERNAL_NAME_CD = PP.INTERNAL_NAME_CD ");
                    selectQuery.Append(" LEFT JOIN TRN_MESSAGE MSG ON MSG.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD AND MSG.MACHINE_CD IS NULL AND  MSG.PROCESS_KUBUN_ID IS NULL  ");
                    selectQuery.Append(" LEFT JOIN MST_CONDITION COND ON COND.CONDITION_CD = PP.PROD_CONDITION_CD ");
                    selectQuery.Append(" WHERE CHUUBAN || Lpad(GYOUBAN,2,0) = '" + processPlanDetails[i].CHUUBAN + "'  AND PP.PROCESS_PLAN_CMPL_DATE IS NULL ");
                    var afterProcessPlanData = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
                    if (afterProcessPlanData != null)
                    {
                        result.Add(afterProcessPlanData);
                    }

                }

            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Namrata Ubale on 2026/09/21       
        /// used screens: ProcessPlanHandoverHost
        /// </summary>
        /// <param name="processPlanDetails"></param>
        /// <returns>
        /// returns process code for each process name from HTMP_PRODUCT_ORDER table and MST_PROCESS table
        /// </returns>
        public GridSearchFields CheckProcessCount(GridSearchFields processPlanDetails)
        {
            GridSearchFields result = new GridSearchFields();
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT ");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_1 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_1,'＿','_')) ELSE NULL END AS PROCESS_1_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_2 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_2,'＿','_')) ELSE NULL END AS PROCESS_2_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_3 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_3,'＿','_')) ELSE NULL END AS PROCESS_3_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_4 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_4,'＿','_')) ELSE NULL END AS PROCESS_4_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_5 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_5,'＿','_')) ELSE NULL END AS PROCESS_5_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_6 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_6,'＿','_')) ELSE NULL END AS PROCESS_6_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_7 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_7,'＿','_')) ELSE NULL END AS PROCESS_7_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_8 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_8,'＿','_')) ELSE NULL END AS PROCESS_8_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_9 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_9,'＿','_')) ELSE NULL END AS PROCESS_9_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_10 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_10,'＿','_')) ELSE NULL END AS PROCESS_10_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_11 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_11,'＿','_')) ELSE NULL END AS PROCESS_11_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_12 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_12,'＿','_')) ELSE NULL END AS PROCESS_12_CD,");
            selectQuery.Append(" CASE WHEN HPO.PROCESS_13 IS NOT NULL THEN (SELECT MP2.PROCESS_CD FROM HSKUSER.MST_PROCESS MP1 INNER JOIN HSKWEBUSER.MST_PROCESS MP2 ON MP2.PROCESS_CD = MP1.PROC_CD WHERE PROC_NAME LIKE REPLACE(HPO.PROCESS_13,'＿','_')) ELSE NULL END AS PROCESS_13_CD ");
            selectQuery.Append(" FROM hsk_replication.HTMP_PRODUCT_ORDER HPO ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append("  HPO.CHUUBAN = '" + processPlanDetails.CHUUBAN + "' AND TO_NUMBER(TRIM(HPO.GYOUBAN))='" + processPlanDetails.GYOUBAN + "' ");
            result = ExecuteQuerySingleResult<GridSearchFields>(selectQuery.ToString());
            return result;
        }

    }
}
