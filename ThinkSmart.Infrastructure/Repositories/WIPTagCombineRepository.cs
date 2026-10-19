using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class WIPTagCombineRepository :BaseRepository, IWIPTagCombineRepository
    {
        public WIPTagCombineRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        {

        }
        public WIPTagCombineDetails GetTagDetailsByCombineCD(int combineCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COMB.PROCESS_PLAN_CD, ");
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
            selectQuery.Append(" COMBINE_DATE, ");
            selectQuery.Append(" COMBINE_OPERATOR_CD, ");
            selectQuery.Append(" COMBINE_TABA_NO, ");
            selectQuery.Append(" COMBINE_NEXT_PROCESS, ");
            selectQuery.Append(" COMBINE_SOURCES_TABA_NO_1, ");
            selectQuery.Append(" COMBINE_SOURCES_NO_OF_UNITS_1, ");
            selectQuery.Append(" COMBINE_SOURCES_UNIT_1, ");
            selectQuery.Append(" COMBINE_SOURCES_QTY_1, ");
            selectQuery.Append(" COMBINE_SOURCES_TABA_NO_2, ");
            selectQuery.Append(" COMBINE_SOURCES_NO_OF_UNITS_2, ");
            selectQuery.Append(" COMBINE_SOURCES_UNIT_2, ");
            selectQuery.Append(" COMBINE_SOURCES_QTY_2, ");
            selectQuery.Append(" COMBINE_SOURCES_TABA_NO_3, ");
            selectQuery.Append(" COMBINE_SOURCES_NO_OF_UNITS_3, ");
            selectQuery.Append(" COMBINE_SOURCES_UNIT_3, ");
            selectQuery.Append(" COMBINE_SOURCES_QTY_3, ");
            selectQuery.Append(" COMBINE_SOURCES_TABA_NO_4, ");
            selectQuery.Append(" COMBINE_SOURCES_NO_OF_UNITS_4, ");
            selectQuery.Append(" COMBINE_SOURCES_UNIT_4, ");
            selectQuery.Append(" COMBINE_SOURCES_QTY_4, ");
            selectQuery.Append(" COMBINE_QTY, ");
            selectQuery.Append(" COMBINE_UNITS, ");
            selectQuery.Append(" COMBINE_NO_OF_UNITS ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" TRN_TABA_COMBINE COMB ");
            selectQuery.Append(" INNER JOIN TRN_PROCESS_PLAN PP ON COMB.PROCESS_PLAN_CD = PP.PROCESS_PLAN_CD ");
            selectQuery.Append(" LEFT JOIN MST_CUSTOMER CUS ON CUS.CUSTOMER_CD =PP.CUSTOMER_CD");
            selectQuery.Append(" LEFT JOIN MST_EMPLOYEE EMP ON COMB.COMBINE_OPERATOR_CD =EMP.EMPLOYEE_CD");
            selectQuery.Append(" WHERE COMB.TABA_COMBINE_CD =" + combineCD);
            var result = ExecuteQuerySingleResult<WIPTagCombineDetails>(selectQuery.ToString());
            return result;
        }
        public void Delete(int combineCD)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM TRN_TABA_COMBINE ");
            deleteQuery.Append(" WHERE TABA_COMBINE_CD = " + combineCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }
    }
}
