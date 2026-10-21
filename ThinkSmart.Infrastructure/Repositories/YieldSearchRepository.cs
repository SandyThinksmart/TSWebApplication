using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    internal class YieldSearchRepository : BaseRepository, IYieldSearchRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        private readonly string specialChuuban;

        public YieldSearchRepository(LocalizationServices localizationServices, IConfiguration _configuration) : base(_configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            _localization = localizationServices;
            specialChuuban = Convert.ToString(_configuration.GetSection("SpecialChuuban").Value);
        }

        public List<ProcessPlanForYieldSearchDetails> GetYieldSearchList(List<SearchField> searchCriteria, string sortCiteria, short? locationCD)
        {
            List<ProcessPlanForYieldSearchDetails> result = new List<ProcessPlanForYieldSearchDetails>();
            int decisionValue = UIUtils.GetResultKubunKey(_localization.GetLocalizedHtmlString("lbl_OK"));
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT * FROM ( ");
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
            //sb.Append("     BEND_STD, ");
            sb.Append("     PP_MAGARI_KUBUN, ");
            sb.Append("     PP_MAGARI_SPAN, ");
            sb.Append("     PP_BEND_TOL, ");
            sb.Append("     MAKER_NAME, ");
            sb.Append("     PP_ACT_ALLOC_DATE_STRING, ");
            sb.Append("     PP_ACT_ALLOC_QTY, ");
            sb.Append("     PROCESS_PLAN_CMPL_DATE_STRING, ");
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
            sb.Append("          THEN ROUND(FI_QTY / PP_ACT_ALLOC_QTY * 100, 2) END AS FI_YIELD ");

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

            sb.Append("CASE WHEN PP.DIM_TOL_MAX_CHR IS NOT NULL AND PP.DIM_TOL_MIN_CHR IS NOT NULL THEN PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR ELSE NVL(PP.DIM_TOL_MAX_CHR , PP.DIM_TOL_MIN_CHR) END AS PP_DIM_TOLERANCE ,");
            //sb.Append("       PP.DIM_TOL_MAX_CHR || ',' || PP.DIM_TOL_MIN_CHR AS PP_DIM_TOLERANCE, ");

            sb.Append("       CASE ");
            sb.Append("           WHEN PP.LENGTH_TOL_MAX_CHR IS NOT NULL ");
            sb.Append("            AND PP.LENGTH_TOL_MIN_CHR IS NOT NULL ");
            sb.Append("           THEN PP.LENGTH_TOL_MAX_CHR || ',' || PP.LENGTH_TOL_MIN_CHR ");
            sb.Append("           ELSE NVL(PP.LENGTH_TOL_MAX_CHR, PP.LENGTH_TOL_MIN_CHR) ");
            sb.Append("       END AS PP_LENGTH_TOL, ");

            sb.Append("  PP.MAGARI_KUBUN AS PP_MAGARI_KUBUN ,");
            sb.Append("  PP.MAGARI_SPAN AS PP_MAGARI_SPAN ,");
            sb.Append("  PP.MAGARI_BEND_TOL AS PP_BEND_TOL ,");
            //sb.Append(" REPLACE( ");
            //sb.Append("     MAGARI_KUBUN || ' - ' || MAGARI_BEND_TOL || ' - ' || MAGARI_SPAN, ");
            //sb.Append("     '  -   -  ' ");
            //sb.Append(" ) AS BEND_STD, ");


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
            sb.Append("               AND dw_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND REDRAWING = 0 ");

            sb.Append("             UNION ALL ");

            sb.Append("             SELECT dw_finished_qty ");
            sb.Append("             FROM trn_drawing_d ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND dw_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND REDRAWING = 0 ");
            sb.Append("               AND IM_DRAWING = 0 ");

            sb.Append("             UNION ALL ");

            sb.Append("             SELECT dw_finished_qty ");
            sb.Append("             FROM trn_drawing_w ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND dw_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND REDRAWING = 0 ");
            sb.Append("               AND IM_DRAWING = 0 ");

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
            sb.Append("               AND str_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND RESTRAIGHTING = 0 ");

            sb.Append("             UNION ALL ");

            sb.Append("             SELECT str_qty ");
            sb.Append("             FROM trn_straightening2 ");
            sb.Append("             WHERE process_plan_cd = pp.process_plan_cd ");
            sb.Append("               AND str_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND RESTRAIGHTING = 0 ");

            sb.Append("         ) ");
            sb.Append(" ) AS straightening_qty, ");

            sb.Append(" ( ");
            sb.Append("     SELECT SUM(ch_finished_qty) ");
            sb.Append("     FROM trn_chamfering tc ");
            sb.Append("     WHERE tc.process_plan_cd = pp.process_plan_cd ");
            sb.Append("       AND tc.ch_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND RECHAMFERING = 0 ");
            sb.Append(" ) AS chamfering_qty, ");

            sb.Append(" ( ");
            sb.Append("     SELECT SUM(ec_ok_qty) ");
            sb.Append("     FROM trn_ec ec ");
            sb.Append("     WHERE ec.process_plan_cd = pp.process_plan_cd ");
            sb.Append("       AND ec.ec_decision IN (10) ");// IN (10, 40) remove process add decsion by Client as user want only OK Percetage
            sb.Append("               AND REEC = 0 ");
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

            sb.Append(" WHERE PP_CHUUBAN_GYOUBAN NOT IN (" + specialChuuban + ") ");


            if (!string.IsNullOrEmpty(sortCiteria))
                sb = new StringBuilder(Helper.ApplySortCriteria(sortCiteria, sb.ToString()));
            else
                sb.Append(" ORDER BY PP_PROD_NAME ,PP_PROD_SHAPE , PP_PROD_DIM_D ,PP_PROD_DIM_W ,PP_PROD_DIM_OTHER , PP_PROD_LENGTH ,PP_PROD_LENGTH_OTHER ");


            return ExecuteQueryMultiResult<ProcessPlanForYieldSearchDetails>(sb.ToString());
            //return result;
        }


        public DataTable LoadECValForECPercentData(long processPlanCd)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("    SUM(EC_OK_BARS) AS EC_OK_QTY, ");
            sql.AppendLine("    SUM(EC_NG_BAR_TOTAL) AS EC_NG_QTY, ");
            sql.AppendLine("    TEI.PROCESS_PLAN_CD ");
            sql.AppendLine("FROM TRN_EC TEI ");
            sql.AppendLine("WHERE TEI.UNIT_TYPE = 1 ");
            sql.AppendLine($"  AND TEI.PROCESS_PLAN_CD = {processPlanCd} ");
            sql.AppendLine("  AND EC_TABA_NO NOT IN ( ");
            sql.AppendLine("        SELECT DW_TABA_NO FROM ( ");
            sql.AppendLine($"            SELECT DW_TABA_NO_1 AS DW_TABA_NO FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
            sql.AppendLine("            UNION ");
            sql.AppendLine($"            SELECT DW_TABA_NO_2 FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
            sql.AppendLine("            UNION ");
            sql.AppendLine($"            SELECT DW_TABA_NO_3 FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
            sql.AppendLine("            UNION ");
            sql.AppendLine($"            SELECT DW_TABA_NO_4 FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
            sql.AppendLine("        ) WHERE DW_TABA_NO IS NOT NULL ");
            sql.AppendLine("  ) ");
            sql.AppendLine("GROUP BY TEI.PROCESS_PLAN_CD");

            return ExecuteQueryDataTable(sql.ToString());
        }

        //public DataTable LoadECValForECPercentData(long processPlanCd)
        //{
        //    var sql = new StringBuilder();

        //    sql.AppendLine("SELECT ");
        //    sql.AppendLine("    SUM(TEI.EC_OK_BARS) AS EC_OK_QTY, ");
        //    sql.AppendLine("    SUM(TEI.EC_NG_BAR_TOTAL) AS EC_NG_QTY, ");
        //    sql.AppendLine("    TEI.PROCESS_PLAN_CD ");
        //    sql.AppendLine("FROM TRN_EC TEI ");
        //    sql.AppendLine("WHERE TEI.UNIT_TYPE = 1 ");
        //    sql.AppendLine($"  AND TEI.PROCESS_PLAN_CD = {processPlanCd} ");

        //    sql.AppendLine("  AND NOT EXISTS ( ");
        //    sql.AppendLine("      SELECT 1 ");
        //    sql.AppendLine("      FROM ( ");

        //    // -------- TRN_DRAWING ----------
        //    sql.AppendLine($"          SELECT DW_TABA_NO_1 AS DW_TABA_NO FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_2 FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_3 FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_4 FROM TRN_DRAWING WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");

        //    // -------- TRN_DRAWING_D ----------
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_1 FROM TRN_DRAWING_D WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_2 FROM TRN_DRAWING_D WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_3 FROM TRN_DRAWING_D WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_4 FROM TRN_DRAWING_D WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");

        //    // -------- TRN_DRAWING_W ----------
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_1 FROM TRN_DRAWING_W WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_2 FROM TRN_DRAWING_W WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_3 FROM TRN_DRAWING_W WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");
        //    sql.AppendLine("          UNION ");
        //    sql.AppendLine($"          SELECT DW_TABA_NO_4 FROM TRN_DRAWING_W WHERE EC_DONE_CHK = '1' AND UNIT_TYPE = 1 AND PROCESS_PLAN_CD = {processPlanCd} ");

        //    sql.AppendLine("      ) D ");
        //    sql.AppendLine("      WHERE D.DW_TABA_NO = TEI.EC_TABA_NO ");
        //    sql.AppendLine("  ) ");

        //    sql.AppendLine("GROUP BY TEI.PROCESS_PLAN_CD");

        //    return ExecuteQueryDataTable(sql.ToString());
        //}


        public DataTable GetDWValForECPercent(long processPlanCd)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("    SUM(NVL(DW_FINISHED_BARS, 0)) AS DW_FINISHED_BARS, ");
            sql.AppendLine("    SUM(NVL(EC_NG_BAR_TOTAL, 0)) AS EC_NG_BAR_TOTAL, ");
            sql.AppendLine("    ( ");
            sql.AppendLine("        SUM(NVL(DW_FINISHED_BARS, 0)) ");
            sql.AppendLine("        - ( ");
            sql.AppendLine("            SUM(NVL(DW_NG_BARS1, 0)) ");
            sql.AppendLine("          + SUM(NVL(DW_NG_BARS2, 0)) ");
            sql.AppendLine("          + SUM(NVL(DW_NG_BARS3, 0)) ");
            sql.AppendLine("          + SUM(NVL(EC_NG_BAR_TOTAL, 0)) ");
            sql.AppendLine("        ) ");
            sql.AppendLine("    ) AS TD_EC_OK_QTY ");
            sql.AppendLine("FROM TRN_DRAWING TD ");
            sql.AppendLine("WHERE TD.EC_DONE_CHK = '1' ");
            sql.AppendLine("  AND TD.UNIT_TYPE = 1 ");
            sql.AppendLine($"  AND TD.PROCESS_PLAN_CD = {processPlanCd} ");
            sql.AppendLine("GROUP BY TD.PROCESS_PLAN_CD");

            return ExecuteQueryDataTable(sql.ToString());
        }


        //public DataTable GetDWValForECPercent(long processPlanCd)
        //{
        //    var sql = new StringBuilder();

        //    sql.AppendLine("SELECT ");
        //    sql.AppendLine("    SUM(NVL(DW_FINISHED_BARS, 0)) AS DW_FINISHED_BARS, ");
        //    sql.AppendLine("    SUM(NVL(EC_NG_BAR_TOTAL, 0)) AS EC_NG_BAR_TOTAL, ");
        //    sql.AppendLine("    ( ");
        //    sql.AppendLine("        SUM(NVL(DW_FINISHED_BARS, 0)) ");
        //    sql.AppendLine("        - ( ");
        //    sql.AppendLine("            SUM(NVL(DW_NG_BARS1, 0)) ");
        //    sql.AppendLine("          + SUM(NVL(DW_NG_BARS2, 0)) ");
        //    sql.AppendLine("          + SUM(NVL(DW_NG_BARS3, 0)) ");
        //    sql.AppendLine("          + SUM(NVL(EC_NG_BAR_TOTAL, 0)) ");
        //    sql.AppendLine("        ) ");
        //    sql.AppendLine("    ) AS TD_EC_OK_QTY ");
        //    sql.AppendLine("FROM ( ");

        //    sql.AppendLine("    SELECT DW_FINISHED_BARS, DW_NG_BARS1, DW_NG_BARS2, DW_NG_BARS3, EC_NG_BAR_TOTAL ");
        //    sql.AppendLine("    FROM TRN_DRAWING ");
        //    sql.AppendLine("    WHERE EC_DONE_CHK = '1' ");
        //    sql.AppendLine("      AND UNIT_TYPE = 1 ");
        //    sql.AppendLine($"      AND PROCESS_PLAN_CD = {processPlanCd} ");

        //    sql.AppendLine("    UNION ALL ");

        //    sql.AppendLine("    SELECT DW_FINISHED_BARS, DW_NG_BARS1, DW_NG_BARS2, DW_NG_BARS3, EC_NG_BAR_TOTAL ");
        //    sql.AppendLine("    FROM TRN_DRAWING_D ");
        //    sql.AppendLine("    WHERE EC_DONE_CHK = '1' ");
        //    sql.AppendLine("      AND UNIT_TYPE = 1 ");
        //    sql.AppendLine($"      AND PROCESS_PLAN_CD = {processPlanCd} ");

        //    sql.AppendLine("    UNION ALL ");

        //    sql.AppendLine("    SELECT DW_FINISHED_BARS, DW_NG_BARS1, DW_NG_BARS2, DW_NG_BARS3, EC_NG_BAR_TOTAL ");
        //    sql.AppendLine("    FROM TRN_DRAWING_W ");
        //    sql.AppendLine("    WHERE EC_DONE_CHK = '1' ");
        //    sql.AppendLine("      AND UNIT_TYPE = 1 ");
        //    sql.AppendLine($"      AND PROCESS_PLAN_CD = {processPlanCd} ");

        //    sql.AppendLine(") ");

        //    return ExecuteQueryDataTable(sql.ToString());
        //}

    }
}
