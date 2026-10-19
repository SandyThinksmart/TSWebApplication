using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class WIPTagSplitRepository : BaseRepository,IWIPTagSplitRepository
    {
        public WIPTagSplitRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {
            
        }
        public WIPTagSplitDetails GetTagDetails(int divisionCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" DV.PROCESS_PLAN_CD, ");
            selectQuery.Append(" EMP.SHORT_NAME AS EMPLOYEE_NAME, ");
            selectQuery.Append(" GET_HYPHENATED_CHUUBAN_GYOUBAN(PP.CHUUBAN_YEAR ||PP.CHUUBAN,PP.GYOUBAN) AS PP_CHUUBAN_GYOUBAN, ");
            selectQuery.Append(" (CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL THEN ");
            selectQuery.Append(" PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ");
            selectQuery.Append(" ELSE PP.DIM_TOL_MIN_CHR END ) AS DIM_TOLERANCE, ");
            selectQuery.Append(" PP.PROD_DIM_D, ");
            selectQuery.Append(" PP.PROD_SHAPE, ");
            selectQuery.Append(" PP.PROD_LENGTH AS PROD_LENGTH, ");
            selectQuery.Append(" PP.CHARGE, ");
            selectQuery.Append(" PP.CHAMFER, ");
            selectQuery.Append(" PP.PROD_NAME_CUST AS PROD_NAME_CUST, ");
            selectQuery.Append(" CUS.SHORT_NAME AS CUSTOMER_NAME, ");
            selectQuery.Append(" DV.LOCATION_CD, ");
            selectQuery.Append(" DV.DIVISION_DATE, ");
            selectQuery.Append(" DV.DIVISION_OPERATOR_CD, ");
            selectQuery.Append(" DV.ORIGIN_TABA_NO, ");
            selectQuery.Append(" DV.DIVISION_TABA_NO_1, ");
            selectQuery.Append(" DV.DIVISION_NO_OF_UNITS_1, ");
            selectQuery.Append(" DV.DIVISION_UNIT_1, ");
            selectQuery.Append(" DV.DIVISION_QTY_1, ");
            selectQuery.Append(" DV.DIVISION_NEXT_PROCESS_1, ");
            selectQuery.Append(" DV.DIVISION_TABA_NO_2, ");
            selectQuery.Append(" DV.DIVISION_NO_OF_UNITS_2, ");
            selectQuery.Append(" DV.DIVISION_UNIT_2, ");
            selectQuery.Append(" DV.DIVISION_QTY_2, ");
            selectQuery.Append(" DV.DIVISION_NEXT_PROCESS_2, ");
            selectQuery.Append(" DV.DIVISION_TABA_NO_3, ");
            selectQuery.Append(" DV.DIVISION_NO_OF_UNITS_3, ");
            selectQuery.Append(" DV.DIVISION_UNIT_3, ");
            selectQuery.Append(" DV.DIVISION_QTY_3, ");
            selectQuery.Append(" DV.DIVISION_NEXT_PROCESS_3, ");
            selectQuery.Append(" DV.DIVISION_TABA_NO_4, ");
            selectQuery.Append(" DV.DIVISION_NO_OF_UNITS_4, ");
            selectQuery.Append(" DV.DIVISION_UNIT_4, ");
            selectQuery.Append(" DV.DIVISION_QTY_4, ");
            selectQuery.Append(" DV.DIVISION_NEXT_PROCESS_4 ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_TABA_DIVISION DV ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON DV.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON DV.DIVISION_OPERATOR_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" WHERE DV.TABA_DIVISION_CD =" + divisionCD);
            var result = ExecuteQuerySingleResult<WIPTagSplitDetails>(selectQuery.ToString());
            return result;
        }
        public void Delete(int divisionCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM TRN_TABA_DIVISION ");
            deleteQuery.Append(" WHERE TABA_DIVISION_CD = " + divisionCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }
    }
}
