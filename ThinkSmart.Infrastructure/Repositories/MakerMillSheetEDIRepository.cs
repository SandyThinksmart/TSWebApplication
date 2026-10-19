using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Markup;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class MakerMillSheetEDIRepository : BaseRepository, IMakerMillSheetEDIRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        string result;
        private string makerMillSheet_Seq;
        private string makerMillSheetLog_Seq;
        private string makerMillSheetRunID_Seq;
        private readonly IConfiguration _configuration;
        public MakerMillSheetEDIRepository(IConfiguration configuration, LocalizationServices localization) : base(configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            _configuration = configuration;
        }
        //public MakerMillSheetEDIRepository(IConfiguration _configuration, LocalizationServices localization) : base(_configuration)
        //{
        //    _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        //    _localization = localization;
        //}


        public void MillSheetEDILogDataInsert(MakerMillSheetEDIDetails row, int runID, string maker_name, int status, string statusMark, string errormessage, string pdfFileStorePath, DateTime created_date)
        {

            //Values taken hard coded from  windows table MST_MAKER_MILLSHEET_EDI_FORMAT
            var chem_comp_name_fixedValues = new Dictionary<string, string>()
            {
                { "chem_comp_name1_fixed", "" },
                { "chem_comp_name2_fixed", "" },
                { "chem_comp_name3_fixed", "" },
                { "chem_comp_name4_fixed", "" },
                { "chem_comp_name5_fixed", "" },
                { "chem_comp_name6_fixed", "" },
                { "chem_comp_name7_fixed", "" },
                { "chem_comp_name8_fixed", "" },
                { "chem_comp_name9_fixed", "" },
                { "chem_comp_name10_fixed", "" },
                { "chem_comp_name11_fixed", "" },
                { "chem_comp_name12_fixed", "" },
                { "chem_comp_name13_fixed", "" },
                { "chem_comp_name14_fixed", "" },
                { "chem_comp_name15_fixed", "" },
                { "chem_comp_name16_fixed", "" },
                { "chem_comp_name17_fixed", "" },
                { "chem_comp_name18_fixed", "" },
                { "chem_comp_name19_fixed", "" },
                { "chem_comp_name20_fixed", "" },
                { "chem_comp_name21_fixed", "" },
                { "chem_comp_name22_fixed", "" },
                { "chem_comp_name23_fixed", "" },
                { "chem_comp_name24_fixed", "" },
                { "chem_comp_name25_fixed", "" },

                { "hard_std1_fixed", "1.5" },
                { "hard_std2_fixed", "3" },
                { "hard_std3_fixed", "5" },
                { "hard_std4_fixed", "7" },
                { "hard_std5_fixed", "9" },
                { "hard_std6_fixed", "11" },
                { "hard_std7_fixed", "13" },
                { "hard_std8_fixed", "15" },
                { "hard_std9_fixed", "20" },
                { "hard_std10_fixed", "25" },
                { "hard_std11_fixed", "30" },
                { "hard_std12_fixed", "35" },
                { "hard_std13_fixed", "40" },
                { "hard_std14_fixed", "45" },
                { "hard_std15_fixed", "50" },
                { "hard_std16_fixed", "" },
                { "hard_std17_fixed", "" },
                { "hard_std18_fixed", "" },
                { "hard_std19_fixed", "" },
                { "hard_std20_fixed", "" },
                { "hard_std21_fixed", "" },
                { "hard_std22_fixed", "" },
                { "hard_std23_fixed", "" },
                { "hard_std24_fixed", "" },
                { "hard_std25_fixed", "" }
            };

            string is_chemical_comp_mul_code = "1";

            string chem_comp_name1_fixed = chem_comp_name_fixedValues["chem_comp_name1_fixed"];
            string chem_comp_name2_fixed = chem_comp_name_fixedValues["chem_comp_name2_fixed"];
            string chem_comp_name3_fixed = chem_comp_name_fixedValues["chem_comp_name3_fixed"];
            string chem_comp_name4_fixed = chem_comp_name_fixedValues["chem_comp_name4_fixed"];
            string chem_comp_name5_fixed = chem_comp_name_fixedValues["chem_comp_name5_fixed"];
            string chem_comp_name6_fixed = chem_comp_name_fixedValues["chem_comp_name6_fixed"];
            string chem_comp_name7_fixed = chem_comp_name_fixedValues["chem_comp_name7_fixed"];
            string chem_comp_name8_fixed = chem_comp_name_fixedValues["chem_comp_name8_fixed"];
            string chem_comp_name9_fixed = chem_comp_name_fixedValues["chem_comp_name9_fixed"];
            string chem_comp_name10_fixed = chem_comp_name_fixedValues["chem_comp_name10_fixed"];
            string chem_comp_name11_fixed = chem_comp_name_fixedValues["chem_comp_name11_fixed"];
            string chem_comp_name12_fixed = chem_comp_name_fixedValues["chem_comp_name12_fixed"];
            string chem_comp_name13_fixed = chem_comp_name_fixedValues["chem_comp_name13_fixed"];
            string chem_comp_name14_fixed = chem_comp_name_fixedValues["chem_comp_name14_fixed"];
            string chem_comp_name15_fixed = chem_comp_name_fixedValues["chem_comp_name15_fixed"];
            string chem_comp_name16_fixed = chem_comp_name_fixedValues["chem_comp_name16_fixed"];
            string chem_comp_name17_fixed = chem_comp_name_fixedValues["chem_comp_name17_fixed"];
            string chem_comp_name18_fixed = chem_comp_name_fixedValues["chem_comp_name18_fixed"];
            string chem_comp_name19_fixed = chem_comp_name_fixedValues["chem_comp_name19_fixed"];
            string chem_comp_name20_fixed = chem_comp_name_fixedValues["chem_comp_name20_fixed"];
            string chem_comp_name21_fixed = chem_comp_name_fixedValues["chem_comp_name21_fixed"];
            string chem_comp_name22_fixed = chem_comp_name_fixedValues["chem_comp_name22_fixed"];
            string chem_comp_name23_fixed = chem_comp_name_fixedValues["chem_comp_name23_fixed"];
            string chem_comp_name24_fixed = chem_comp_name_fixedValues["chem_comp_name24_fixed"];
            string chem_comp_name25_fixed = chem_comp_name_fixedValues["chem_comp_name25_fixed"];

            string hard_std1_fixed = chem_comp_name_fixedValues["hard_std1_fixed"];
            string hard_std2_fixed = chem_comp_name_fixedValues["hard_std2_fixed"];
            string hard_std3_fixed = chem_comp_name_fixedValues["hard_std3_fixed"];
            string hard_std4_fixed = chem_comp_name_fixedValues["hard_std4_fixed"];
            string hard_std5_fixed = chem_comp_name_fixedValues["hard_std5_fixed"];
            string hard_std6_fixed = chem_comp_name_fixedValues["hard_std6_fixed"];
            string hard_std7_fixed = chem_comp_name_fixedValues["hard_std7_fixed"];
            string hard_std8_fixed = chem_comp_name_fixedValues["hard_std8_fixed"];
            string hard_std9_fixed = chem_comp_name_fixedValues["hard_std9_fixed"];
            string hard_std10_fixed = chem_comp_name_fixedValues["hard_std10_fixed"];
            string hard_std11_fixed = chem_comp_name_fixedValues["hard_std11_fixed"];
            string hard_std12_fixed = chem_comp_name_fixedValues["hard_std12_fixed"];
            string hard_std13_fixed = chem_comp_name_fixedValues["hard_std13_fixed"];
            string hard_std14_fixed = chem_comp_name_fixedValues["hard_std14_fixed"];
            string hard_std15_fixed = chem_comp_name_fixedValues["hard_std15_fixed"];
            string hard_std16_fixed = chem_comp_name_fixedValues["hard_std16_fixed"];
            string hard_std17_fixed = chem_comp_name_fixedValues["hard_std17_fixed"];
            string hard_std18_fixed = chem_comp_name_fixedValues["hard_std18_fixed"];
            string hard_std19_fixed = chem_comp_name_fixedValues["hard_std19_fixed"];
            string hard_std20_fixed = chem_comp_name_fixedValues["hard_std20_fixed"];
            string hard_std21_fixed = chem_comp_name_fixedValues["hard_std21_fixed"];
            string hard_std22_fixed = chem_comp_name_fixedValues["hard_std22_fixed"];
            string hard_std23_fixed = chem_comp_name_fixedValues["hard_std23_fixed"];
            string hard_std24_fixed = chem_comp_name_fixedValues["hard_std24_fixed"];
            string hard_std25_fixed = chem_comp_name_fixedValues["hard_std25_fixed"];

            //Added to make web and win data in sync 
            makerMillSheetLog_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Maker_Mill_Sheet_log").Value);
            int logCD = 0;
            string seqName = string.Empty;

            if (string.IsNullOrEmpty(makerMillSheetLog_Seq))
            {
                seqName = "MAKER_MILL_SHEET_LOG_CD_SEQ.NEXTVAL";
            }
            else
            {
                seqName = makerMillSheetLog_Seq;
            }
            logCD = Convert.ToInt32(ExecuteScalar("SELECT " + seqName + " FROM DUAL"));

            MakerMillSheetEDIDetails result = new MakerMillSheetEDIDetails();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO MAKER_MILL_SHEET_LOG(");
            insertQuery.Append(" MAKER_MILL_SHEET_LOG_CD");
            //insertQuery.Append(" ,MAKER_CD ");
            insertQuery.Append(" ,MAKER_NAME ");
            insertQuery.Append(" ,PDF_FILE_PATH ");
            insertQuery.Append(" ,FILENAME ");
            //insertQuery.Append(" ,PDF_PAGE_NUMBER ");
            insertQuery.Append(" ,KOUBAN");
            insertQuery.Append(" ,CHARGE");
            insertQuery.Append(" ,MATL_NAME_MAKER");
            insertQuery.Append(" ,DIM_D ");
            insertQuery.Append(" ,LENGTH");
            insertQuery.Append(" ,NO_OF_UNITS");
            //insertQuery.Append(" ,UNIT");
            insertQuery.Append(" ,QUANTITY");
            insertQuery.Append(" ,RUN_CD");
            insertQuery.Append(" ,STATUS_MARK");
            insertQuery.Append(" ,STATUS");
            insertQuery.Append(" ,MESSAGE");
            insertQuery.Append(" ,GRAIN_SIZE_STD");
            insertQuery.Append(" ,GRAIN_SIZE_ACTUAL");
            insertQuery.Append(" ,HARDNESS_STD");
            insertQuery.Append(" ,HARDNESS_MIN");
            insertQuery.Append(" ,HARDNESS_MAX");
            insertQuery.Append(" ,HARDNESS_ACTUAL");
            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
            insertQuery.Append(" ,CCNAME1");
            insertQuery.Append(" ,CCNAME2");
            insertQuery.Append(" ,CCNAME3");
            insertQuery.Append(" ,CCNAME4");
            insertQuery.Append(" ,CCNAME5");
            insertQuery.Append(" ,CCNAME6");
            insertQuery.Append(" ,CCNAME7");
            insertQuery.Append(" ,CCNAME8");
            insertQuery.Append(" ,CCNAME9");
            insertQuery.Append(" ,CCNAME10");
            insertQuery.Append(" ,CCNAME11");
            insertQuery.Append(" ,CCNAME12");
            insertQuery.Append(" ,CCNAME13");
            insertQuery.Append(" ,CCNAME14");
            insertQuery.Append(" ,CCNAME15");
            insertQuery.Append(" ,CCNAME16");
            insertQuery.Append(" ,CCNAME17");
            insertQuery.Append(" ,CCNAME18");
            insertQuery.Append(" ,CCNAME19");
            insertQuery.Append(" ,CCNAME20");
            insertQuery.Append(" ,CCNAME21");
            insertQuery.Append(" ,CCNAME22");
            insertQuery.Append(" ,CCNAME23");
            insertQuery.Append(" ,CCNAME24");
            insertQuery.Append(" ,CCNAME25");
            insertQuery.Append(" ,CCRANGE1");
            insertQuery.Append(" ,CCRANGE2");
            insertQuery.Append(" ,CCRANGE3");
            insertQuery.Append(" ,CCRANGE4");
            insertQuery.Append(" ,CCRANGE5");
            insertQuery.Append(" ,CCRANGE6");
            insertQuery.Append(" ,CCRANGE7");
            insertQuery.Append(" ,CCRANGE8");
            insertQuery.Append(" ,CCRANGE9");
            insertQuery.Append(" ,CCRANGE10");
            insertQuery.Append(" ,CCRANGE11");
            insertQuery.Append(" ,CCRANGE12");
            insertQuery.Append(" ,CCRANGE13");
            insertQuery.Append(" ,CCRANGE14");
            insertQuery.Append(" ,CCRANGE15");
            insertQuery.Append(" ,CCRANGE16");
            insertQuery.Append(" ,CCRANGE17");
            insertQuery.Append(" ,CCRANGE18");
            insertQuery.Append(" ,CCRANGE19");
            insertQuery.Append(" ,CCRANGE20");
            insertQuery.Append(" ,CCRANGE21");
            insertQuery.Append(" ,CCRANGE22");
            insertQuery.Append(" ,CCRANGE23");
            insertQuery.Append(" ,CCRANGE24");
            insertQuery.Append(" ,CCRANGE25");
            insertQuery.Append(" ,CCMIN1");
            insertQuery.Append(" ,CCMIN2");
            insertQuery.Append(" ,CCMIN3");
            insertQuery.Append(" ,CCMIN4");
            insertQuery.Append(" ,CCMIN5");
            insertQuery.Append(" ,CCMIN6");
            insertQuery.Append(" ,CCMIN7");
            insertQuery.Append(" ,CCMIN8");
            insertQuery.Append(" ,CCMIN9");
            insertQuery.Append(" ,CCMIN10");
            insertQuery.Append(" ,CCMIN11");
            insertQuery.Append(" ,CCMIN12");
            insertQuery.Append(" ,CCMIN13");
            insertQuery.Append(" ,CCMIN14");
            insertQuery.Append(" ,CCMIN15");
            insertQuery.Append(" ,CCMIN16");
            insertQuery.Append(" ,CCMIN17");
            insertQuery.Append(" ,CCMIN18");
            insertQuery.Append(" ,CCMIN19");
            insertQuery.Append(" ,CCMIN20");
            insertQuery.Append(" ,CCMIN21");
            insertQuery.Append(" ,CCMIN22");
            insertQuery.Append(" ,CCMIN23");
            insertQuery.Append(" ,CCMIN24");
            insertQuery.Append(" ,CCMIN25");
            insertQuery.Append(" ,CCMAX1");
            insertQuery.Append(" ,CCMAX2");
            insertQuery.Append(" ,CCMAX3");
            insertQuery.Append(" ,CCMAX4");
            insertQuery.Append(" ,CCMAX5");
            insertQuery.Append(" ,CCMAX6");
            insertQuery.Append(" ,CCMAX7");
            insertQuery.Append(" ,CCMAX8");
            insertQuery.Append(" ,CCMAX9");
            insertQuery.Append(" ,CCMAX10");
            insertQuery.Append(" ,CCMAX11");
            insertQuery.Append(" ,CCMAX12");
            insertQuery.Append(" ,CCMAX13");
            insertQuery.Append(" ,CCMAX14");
            insertQuery.Append(" ,CCMAX15");
            insertQuery.Append(" ,CCMAX16");
            insertQuery.Append(" ,CCMAX17");
            insertQuery.Append(" ,CCMAX18");
            insertQuery.Append(" ,CCMAX19");
            insertQuery.Append(" ,CCMAX20");
            insertQuery.Append(" ,CCMAX21");
            insertQuery.Append(" ,CCMAX22");
            insertQuery.Append(" ,CCMAX23");
            insertQuery.Append(" ,CCMAX24");
            insertQuery.Append(" ,CCMAX25");
            insertQuery.Append(" ,CCACTUAL1");
            insertQuery.Append(" ,CCACTUAL2");
            insertQuery.Append(" ,CCACTUAL3");
            insertQuery.Append(" ,CCACTUAL4");
            insertQuery.Append(" ,CCACTUAL5");
            insertQuery.Append(" ,CCACTUAL6");
            insertQuery.Append(" ,CCACTUAL7");
            insertQuery.Append(" ,CCACTUAL8");
            insertQuery.Append(" ,CCACTUAL9");
            insertQuery.Append(" ,CCACTUAL10");
            insertQuery.Append(" ,CCACTUAL11");
            insertQuery.Append(" ,CCACTUAL12");
            insertQuery.Append(" ,CCACTUAL13");
            insertQuery.Append(" ,CCACTUAL14");
            insertQuery.Append(" ,CCACTUAL15");
            insertQuery.Append(" ,CCACTUAL16");
            insertQuery.Append(" ,CCACTUAL17");
            insertQuery.Append(" ,CCACTUAL18");
            insertQuery.Append(" ,CCACTUAL19");
            insertQuery.Append(" ,CCACTUAL20");
            insertQuery.Append(" ,CCACTUAL21");
            insertQuery.Append(" ,CCACTUAL22");
            insertQuery.Append(" ,CCACTUAL23");
            insertQuery.Append(" ,CCACTUAL24");
            insertQuery.Append(" ,CCACTUAL25");
            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------

            insertQuery.Append(" ,HARD_STD1");
            insertQuery.Append(" ,HARD_STD2");
            insertQuery.Append(" ,HARD_STD3");
            insertQuery.Append(" ,HARD_STD4");
            insertQuery.Append(" ,HARD_STD5");
            insertQuery.Append(" ,HARD_STD6");
            insertQuery.Append(" ,HARD_STD7");
            insertQuery.Append(" ,HARD_STD8");
            insertQuery.Append(" ,HARD_STD9");
            insertQuery.Append(" ,HARD_STD10");
            insertQuery.Append(" ,HARD_STD11");
            insertQuery.Append(" ,HARD_STD12");
            insertQuery.Append(" ,HARD_STD13");
            insertQuery.Append(" ,HARD_STD14");
            insertQuery.Append(" ,HARD_STD15");
            insertQuery.Append(" ,HARD_STD16");
            insertQuery.Append(" ,HARD_STD17");
            insertQuery.Append(" ,HARD_STD18");
            insertQuery.Append(" ,HARD_STD19");
            insertQuery.Append(" ,HARD_STD20");
            insertQuery.Append(" ,HARD_STD21");
            insertQuery.Append(" ,HARD_STD22");
            insertQuery.Append(" ,HARD_STD23");
            insertQuery.Append(" ,HARD_STD24");
            insertQuery.Append(" ,HARD_STD25");
            insertQuery.Append(" ,HARD_MIN1");
            insertQuery.Append(" ,HARD_MIN2");
            insertQuery.Append(" ,HARD_MIN3");
            insertQuery.Append(" ,HARD_MIN4");
            insertQuery.Append(" ,HARD_MIN5");
            insertQuery.Append(" ,HARD_MIN6");
            insertQuery.Append(" ,HARD_MIN7");
            insertQuery.Append(" ,HARD_MIN8");
            insertQuery.Append(" ,HARD_MIN9");
            insertQuery.Append(" ,HARD_MIN10");
            insertQuery.Append(" ,HARD_MIN11");
            insertQuery.Append(" ,HARD_MIN12");
            insertQuery.Append(" ,HARD_MIN13");
            insertQuery.Append(" ,HARD_MIN14");
            insertQuery.Append(" ,HARD_MIN15");
            insertQuery.Append(" ,HARD_MIN16");
            insertQuery.Append(" ,HARD_MIN17");
            insertQuery.Append(" ,HARD_MIN18");
            insertQuery.Append(" ,HARD_MIN19");
            insertQuery.Append(" ,HARD_MIN20");
            insertQuery.Append(" ,HARD_MIN21");
            insertQuery.Append(" ,HARD_MIN22");
            insertQuery.Append(" ,HARD_MIN23");
            insertQuery.Append(" ,HARD_MIN24");
            insertQuery.Append(" ,HARD_MIN25");
            insertQuery.Append(" ,HARD_MAX1");
            insertQuery.Append(" ,HARD_MAX2");
            insertQuery.Append(" ,HARD_MAX3");
            insertQuery.Append(" ,HARD_MAX4");
            insertQuery.Append(" ,HARD_MAX5");
            insertQuery.Append(" ,HARD_MAX6");
            insertQuery.Append(" ,HARD_MAX7");
            insertQuery.Append(" ,HARD_MAX8");
            insertQuery.Append(" ,HARD_MAX9");
            insertQuery.Append(" ,HARD_MAX10");
            insertQuery.Append(" ,HARD_MAX11");
            insertQuery.Append(" ,HARD_MAX12");
            insertQuery.Append(" ,HARD_MAX13");
            insertQuery.Append(" ,HARD_MAX14");
            insertQuery.Append(" ,HARD_MAX15");
            insertQuery.Append(" ,HARD_MAX16");
            insertQuery.Append(" ,HARD_MAX17");
            insertQuery.Append(" ,HARD_MAX18");
            insertQuery.Append(" ,HARD_MAX19");
            insertQuery.Append(" ,HARD_MAX20");
            insertQuery.Append(" ,HARD_MAX21");
            insertQuery.Append(" ,HARD_MAX22");
            insertQuery.Append(" ,HARD_MAX23");
            insertQuery.Append(" ,HARD_MAX24");
            insertQuery.Append(" ,HARD_MAX25");
            insertQuery.Append(" ,HARD_ACTUAL1");
            insertQuery.Append(" ,HARD_ACTUAL2");
            insertQuery.Append(" ,HARD_ACTUAL3");
            insertQuery.Append(" ,HARD_ACTUAL4");
            insertQuery.Append(" ,HARD_ACTUAL5");
            insertQuery.Append(" ,HARD_ACTUAL6");
            insertQuery.Append(" ,HARD_ACTUAL7");
            insertQuery.Append(" ,HARD_ACTUAL8");
            insertQuery.Append(" ,HARD_ACTUAL9");
            insertQuery.Append(" ,HARD_ACTUAL10");
            insertQuery.Append(" ,HARD_ACTUAL11");
            insertQuery.Append(" ,HARD_ACTUAL12");
            insertQuery.Append(" ,HARD_ACTUAL13");
            insertQuery.Append(" ,HARD_ACTUAL14");
            insertQuery.Append(" ,HARD_ACTUAL15");
            insertQuery.Append(" ,HARD_ACTUAL16");
            insertQuery.Append(" ,HARD_ACTUAL17");
            insertQuery.Append(" ,HARD_ACTUAL18");
            insertQuery.Append(" ,HARD_ACTUAL19");
            insertQuery.Append(" ,HARD_ACTUAL20");
            insertQuery.Append(" ,HARD_ACTUAL21");
            insertQuery.Append(" ,HARD_ACTUAL22");
            insertQuery.Append(" ,HARD_ACTUAL23");
            insertQuery.Append(" ,HARD_ACTUAL24");
            insertQuery.Append(" ,HARD_ACTUAL25");


            insertQuery.Append(" ,CREATED_DATE");
            //insertQuery.Append(" ,UPDATED_DATE ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            //insertQuery.Append(" MAKER_MILL_SHEET_LOG_CD_SEQ.NEXTVAL ");  //Commented to make web and win data in sync 
            insertQuery.Append("'" + logCD + "'");               //Added to make web and win data in sync 
            //insertQuery.Append(",'" + row.MAKER_CD + "'");   
            insertQuery.Append(",'" + maker_name + "'");
            insertQuery.Append(",'" + row.PDF_FILE_PATH + "'");
            insertQuery.Append(",'" + row.FILENAME + "'");
            //insertQuery.Append(",'" + row.PDF_PAGE_NUMBER + "'");
            insertQuery.Append(",'" + row.KOUBAN + "'");
            insertQuery.Append(",'" + row.CHARGE + "'");
            insertQuery.Append(",'" + row.MATL_NAME_MAKER + "'");
            insertQuery.Append(",'" + row.DIM_D + "'");
            insertQuery.Append(",'" + row.LENGTH + "'");
            insertQuery.Append(",'" + row.NO_OF_UNITS + "'");
            //insertQuery.Append(",'" + row.UNIT + "'");
            insertQuery.Append(",'" + row.QUANTITY + "'");
            insertQuery.Append(",'" + runID + "'");
            insertQuery.Append(",'" + statusMark + "'");
            insertQuery.Append(",'" + status + "'");
            insertQuery.Append(",'" + errormessage + "'");
            //insertQuery.Append(",'" + row.GRAIN_SIZE_STD + "'");
            //insertQuery.Append(",'" + row.GRAIN_SIZE_ACTUAL + "'");
            //insertQuery.Append(",'" + row.HARDNESS_STD + "'");
            //insertQuery.Append(",'" + row.HARDNESS_MIN + "'");
            //insertQuery.Append(",'" + row.HARDNESS_MAX + "'");
            //insertQuery.Append(",'" + row.HARDNESS_ACTUAL + "'");
            insertQuery.Append(row.GRAIN_SIZE_STD == null || row.GRAIN_SIZE_STD == 0 ? ",NULL" : ",'" + row.GRAIN_SIZE_STD + "'");
            insertQuery.Append(row.GRAIN_SIZE_ACTUAL == null || row.GRAIN_SIZE_ACTUAL == 0 ? ",NULL" : ",'" + row.GRAIN_SIZE_ACTUAL + "'");
            insertQuery.Append(string.IsNullOrEmpty(row.HARDNESS_STD) || row.HARDNESS_STD == "0" ? ",NULL" : ",'" + row.HARDNESS_STD + "'");
            insertQuery.Append(string.IsNullOrEmpty(row.HARDNESS_MIN) || row.HARDNESS_MIN == "0" ? ",NULL" : ",'" + row.HARDNESS_MIN + "'");
            insertQuery.Append(string.IsNullOrEmpty(row.HARDNESS_MAX) || row.HARDNESS_MAX == "0" ? ",NULL" : ",'" + row.HARDNESS_MAX + "'");
            insertQuery.Append(string.IsNullOrEmpty(row.HARDNESS_ACTUAL) || row.HARDNESS_ACTUAL == "0" ? ",NULL" : ",'" + row.HARDNESS_ACTUAL + "'");

            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
            //insertQuery.Append(",'" + row.CCNAME1 + "'");


            if (string.IsNullOrEmpty(chem_comp_name1_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME1 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name1_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name2_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME2 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name2_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name3_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME3 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name3_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name4_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME4 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name4_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name5_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME5 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name5_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name6_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME6 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name6_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name7_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME7 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name7_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name8_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME8 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name8_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name9_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME9 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name9_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name10_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME10 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name10_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name11_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME11 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name11_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name12_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME12 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name12_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name13_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME13 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name13_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name14_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME14 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name14_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name15_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME15 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name15_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name16_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME16 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name16_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name17_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME17 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name17_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name18_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME18 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name18_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name19_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME19 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name19_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name20_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME20 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name20_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name21_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME21 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name21_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name22_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME22 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name22_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name23_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME23 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name23_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name24_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME24 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name24_fixed}'");
            }
            if (string.IsNullOrEmpty(chem_comp_name25_fixed))
            {
                insertQuery.Append(",'" + row.CCNAME25 + "'");
            }
            else
            {
                insertQuery.Append($",'{chem_comp_name25_fixed}'");
            }



            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE1?.ToString()) && row.CCRANGE1.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE1);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE1) ? ",NULL" : ",'" + row.CCRANGE1 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE1) ? ",NULL" : ",'" + row.CCRANGE1 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE2?.ToString()) && row.CCRANGE2.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE2);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE2) ? ",NULL" : ",'" + row.CCRANGE2 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE2) ? ",NULL" : ",'" + row.CCRANGE2 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE3?.ToString()) && row.CCRANGE3.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE3);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE3) ? ",NULL" : ",'" + row.CCRANGE3 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE3) ? ",NULL" : ",'" + row.CCRANGE3 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE4?.ToString()) && row.CCRANGE4.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE4);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE4) ? ",NULL" : ",'" + row.CCRANGE4 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE4) ? ",NULL" : ",'" + row.CCRANGE4 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE5?.ToString()) && row.CCRANGE5.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE5);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE5) ? ",NULL" : ",'" + row.CCRANGE5 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE5) ? ",NULL" : ",'" + row.CCRANGE5 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE6?.ToString()) && row.CCRANGE6.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE6);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE6) ? ",NULL" : ",'" + row.CCRANGE6 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE6) ? ",NULL" : ",'" + row.CCRANGE6 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE7?.ToString()) && row.CCRANGE7.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE7);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE7) ? ",NULL" : ",'" + row.CCRANGE7 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE7) ? ",NULL" : ",'" + row.CCRANGE7 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE8?.ToString()) && row.CCRANGE8.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE8);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE8) ? ",NULL" : ",'" + row.CCRANGE8 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE8) ? ",NULL" : ",'" + row.CCRANGE8 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE9?.ToString()) && row.CCRANGE9.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE9);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE9) ? ",NULL" : ",'" + row.CCRANGE9 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE9) ? ",NULL" : ",'" + row.CCRANGE9 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE10?.ToString()) && row.CCRANGE10.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE10);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE10) ? ",NULL" : ",'" + row.CCRANGE10 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE10) ? ",NULL" : ",'" + row.CCRANGE10 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE11?.ToString()) && row.CCRANGE11.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE11);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE11) ? ",NULL" : ",'" + row.CCRANGE11 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE11) ? ",NULL" : ",'" + row.CCRANGE11 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE12?.ToString()) && row.CCRANGE12.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE12);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE12) ? ",NULL" : ",'" + row.CCRANGE12 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE12) ? ",NULL" : ",'" + row.CCRANGE12 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE13?.ToString()) && row.CCRANGE13.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE13);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE13) ? ",NULL" : ",'" + row.CCRANGE13 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE13) ? ",NULL" : ",'" + row.CCRANGE13 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE14?.ToString()) && row.CCRANGE14.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE14);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE14) ? ",NULL" : ",'" + row.CCRANGE14 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE14) ? ",NULL" : ",'" + row.CCRANGE14 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE15?.ToString()) && row.CCRANGE15.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE15);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE15) ? ",NULL" : ",'" + row.CCRANGE15 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE15) ? ",NULL" : ",'" + row.CCRANGE15 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE16?.ToString()) && row.CCRANGE16.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE16);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE16) ? ",NULL" : ",'" + row.CCRANGE16 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE16) ? ",NULL" : ",'" + row.CCRANGE16 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE17?.ToString()) && row.CCRANGE17.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE17);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE17) ? ",NULL" : ",'" + row.CCRANGE17 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE17) ? ",NULL" : ",'" + row.CCRANGE17 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE18?.ToString()) && row.CCRANGE18.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE18);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE18) ? ",NULL" : ",'" + row.CCRANGE18 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE18) ? ",NULL" : ",'" + row.CCRANGE18 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE19?.ToString()) && row.CCRANGE19.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE19);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE19) ? ",NULL" : ",'" + row.CCRANGE19 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE19) ? ",NULL" : ",'" + row.CCRANGE19 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE20?.ToString()) && row.CCRANGE20.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE20);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE20) ? ",NULL" : ",'" + row.CCRANGE20 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE20) ? ",NULL" : ",'" + row.CCRANGE20 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE21?.ToString()) && row.CCRANGE21.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE21);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE21) ? ",NULL" : ",'" + row.CCRANGE21 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE21) ? ",NULL" : ",'" + row.CCRANGE21 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE22?.ToString()) && row.CCRANGE22.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE22);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE22) ? ",NULL" : ",'" + row.CCRANGE22 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE22) ? ",NULL" : ",'" + row.CCRANGE22 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE23?.ToString()) && row.CCRANGE23.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE23);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE23) ? ",NULL" : ",'" + row.CCRANGE23 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE23) ? ",NULL" : ",'" + row.CCRANGE23 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE24?.ToString()) && row.CCRANGE24.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE24);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE24) ? ",NULL" : ",'" + row.CCRANGE24 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE24) ? ",NULL" : ",'" + row.CCRANGE24 + "'");
            }

            if (is_chemical_comp_mul_code == "1")
            {
                if (!string.IsNullOrEmpty(row.CCRANGE25?.ToString()) && row.CCRANGE25.ToString() != "0")
                {
                    int mulAddZero = Convert.ToInt32(row.CCRANGE25);
                    string multiplier = AddZeroForMul(mulAddZero);

                    insertQuery.Append($",'{multiplier}'");
                }
                else
                {
                    insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE25) ? ",NULL" : ",'" + row.CCRANGE25 + "'");
                }

            }
            else
            {
                insertQuery.Append(string.IsNullOrEmpty(row.CCRANGE25) ? ",NULL" : ",'" + row.CCRANGE25 + "'");
            }
            //insertQuery.Append(",'" + row.CCNAME2 + "'");
            //insertQuery.Append(",'" + row.CCNAME3 + "'");
            //insertQuery.Append(",'" + row.CCNAME4 + "'");
            //insertQuery.Append(",'" + row.CCNAME5 + "'");
            //insertQuery.Append(",'" + row.CCNAME6 + "'");
            //insertQuery.Append(",'" + row.CCNAME7 + "'");
            //insertQuery.Append(",'" + row.CCNAME8 + "'");
            //insertQuery.Append(",'" + row.CCNAME9 + "'");
            //insertQuery.Append(",'" + row.CCNAME10 + "'");
            //insertQuery.Append(",'" + row.CCNAME11 + "'");
            //insertQuery.Append(",'" + row.CCNAME12 + "'");
            //insertQuery.Append(",'" + row.CCNAME13 + "'");
            //insertQuery.Append(",'" + row.CCNAME14 + "'");
            //insertQuery.Append(",'" + row.CCNAME15 + "'");
            //insertQuery.Append(",'" + row.CCNAME16 + "'");
            //insertQuery.Append(",'" + row.CCNAME17 + "'");
            //insertQuery.Append(",'" + row.CCNAME18 + "'");
            //insertQuery.Append(",'" + row.CCNAME19 + "'");
            //insertQuery.Append(",'" + row.CCNAME20 + "'");
            //insertQuery.Append(",'" + row.CCNAME21 + "'");
            //insertQuery.Append(",'" + row.CCNAME22 + "'");
            //insertQuery.Append(",'" + row.CCNAME23 + "'");
            //insertQuery.Append(",'" + row.CCNAME24 + "'");
            //insertQuery.Append(",'" + row.CCNAME25 + "'");

            //insertQuery.Append(",'" + row.CCRANGE1 + "'");
            //insertQuery.Append(",'" + row.CCRANGE2 + "'");
            //insertQuery.Append(",'" + row.CCRANGE3 + "'");
            //insertQuery.Append(",'" + row.CCRANGE4 + "'");
            //insertQuery.Append(",'" + row.CCRANGE5 + "'");
            //insertQuery.Append(",'" + row.CCRANGE6 + "'");
            //insertQuery.Append(",'" + row.CCRANGE7 + "'");
            //insertQuery.Append(",'" + row.CCRANGE8 + "'");
            //insertQuery.Append(",'" + row.CCRANGE9 + "'");
            //insertQuery.Append(",'" + row.CCRANGE10 + "'");
            //insertQuery.Append(",'" + row.CCRANGE11 + "'");
            //insertQuery.Append(",'" + row.CCRANGE12 + "'");
            //insertQuery.Append(",'" + row.CCRANGE13 + "'");
            //insertQuery.Append(",'" + row.CCRANGE14 + "'");
            //insertQuery.Append(",'" + row.CCRANGE15 + "'");
            //insertQuery.Append(",'" + row.CCRANGE16 + "'");
            //insertQuery.Append(",'" + row.CCRANGE17 + "'");
            //insertQuery.Append(",'" + row.CCRANGE18 + "'");
            //insertQuery.Append(",'" + row.CCRANGE19 + "'");
            //insertQuery.Append(",'" + row.CCRANGE20 + "'");
            //insertQuery.Append(",'" + row.CCRANGE21 + "'");
            //insertQuery.Append(",'" + row.CCRANGE22 + "'");
            //insertQuery.Append(",'" + row.CCRANGE23 + "'");
            //insertQuery.Append(",'" + row.CCRANGE24 + "'");
            //insertQuery.Append(",'" + row.CCRANGE25 + "'");

            insertQuery.Append(",'" + row.CCMIN1 + "'");
            insertQuery.Append(",'" + row.CCMIN2 + "'");
            insertQuery.Append(",'" + row.CCMIN3 + "'");
            insertQuery.Append(",'" + row.CCMIN4 + "'");
            insertQuery.Append(",'" + row.CCMIN5 + "'");
            insertQuery.Append(",'" + row.CCMIN6 + "'");
            insertQuery.Append(",'" + row.CCMIN7 + "'");
            insertQuery.Append(",'" + row.CCMIN8 + "'");
            insertQuery.Append(",'" + row.CCMIN9 + "'");
            insertQuery.Append(",'" + row.CCMIN10 + "'");
            insertQuery.Append(",'" + row.CCMIN11 + "'");
            insertQuery.Append(",'" + row.CCMIN12 + "'");
            insertQuery.Append(",'" + row.CCMIN13 + "'");
            insertQuery.Append(",'" + row.CCMIN14 + "'");
            insertQuery.Append(",'" + row.CCMIN15 + "'");
            insertQuery.Append(",'" + row.CCMIN16 + "'");
            insertQuery.Append(",'" + row.CCMIN17 + "'");
            insertQuery.Append(",'" + row.CCMIN18 + "'");
            insertQuery.Append(",'" + row.CCMIN19 + "'");
            insertQuery.Append(",'" + row.CCMIN20 + "'");
            insertQuery.Append(",'" + row.CCMIN21 + "'");
            insertQuery.Append(",'" + row.CCMIN22 + "'");
            insertQuery.Append(",'" + row.CCMIN23 + "'");
            insertQuery.Append(",'" + row.CCMIN24 + "'");
            insertQuery.Append(",'" + row.CCMIN25 + "'");
            insertQuery.Append(",'" + row.CCMAX1 + "'");
            insertQuery.Append(",'" + row.CCMAX2 + "'");
            insertQuery.Append(",'" + row.CCMAX3 + "'");
            insertQuery.Append(",'" + row.CCMAX4 + "'");
            insertQuery.Append(",'" + row.CCMAX5 + "'");
            insertQuery.Append(",'" + row.CCMAX6 + "'");
            insertQuery.Append(",'" + row.CCMAX7 + "'");
            insertQuery.Append(",'" + row.CCMAX8 + "'");
            insertQuery.Append(",'" + row.CCMAX9 + "'");
            insertQuery.Append(",'" + row.CCMAX10 + "'");
            insertQuery.Append(",'" + row.CCMAX11 + "'");
            insertQuery.Append(",'" + row.CCMAX12 + "'");
            insertQuery.Append(",'" + row.CCMAX13 + "'");
            insertQuery.Append(",'" + row.CCMAX14 + "'");
            insertQuery.Append(",'" + row.CCMAX15 + "'");
            insertQuery.Append(",'" + row.CCMAX16 + "'");
            insertQuery.Append(",'" + row.CCMAX17 + "'");
            insertQuery.Append(",'" + row.CCMAX18 + "'");
            insertQuery.Append(",'" + row.CCMAX19 + "'");
            insertQuery.Append(",'" + row.CCMAX20 + "'");
            insertQuery.Append(",'" + row.CCMAX21 + "'");
            insertQuery.Append(",'" + row.CCMAX22 + "'");
            insertQuery.Append(",'" + row.CCMAX23 + "'");
            insertQuery.Append(",'" + row.CCMAX24 + "'");
            insertQuery.Append(",'" + row.CCMAX25 + "'");
            insertQuery.Append(",'" + row.CCACTUAL1 + "'");
            insertQuery.Append(",'" + row.CCACTUAL2 + "'");
            insertQuery.Append(",'" + row.CCACTUAL3 + "'");
            insertQuery.Append(",'" + row.CCACTUAL4 + "'");
            insertQuery.Append(",'" + row.CCACTUAL5 + "'");
            insertQuery.Append(",'" + row.CCACTUAL6 + "'");
            insertQuery.Append(",'" + row.CCACTUAL7 + "'");
            insertQuery.Append(",'" + row.CCACTUAL8 + "'");
            insertQuery.Append(",'" + row.CCACTUAL9 + "'");
            insertQuery.Append(",'" + row.CCACTUAL10 + "'");
            insertQuery.Append(",'" + row.CCACTUAL11 + "'");
            insertQuery.Append(",'" + row.CCACTUAL12 + "'");
            insertQuery.Append(",'" + row.CCACTUAL13 + "'");
            insertQuery.Append(",'" + row.CCACTUAL14 + "'");
            insertQuery.Append(",'" + row.CCACTUAL15 + "'");
            insertQuery.Append(",'" + row.CCACTUAL16 + "'");
            insertQuery.Append(",'" + row.CCACTUAL17 + "'");
            insertQuery.Append(",'" + row.CCACTUAL18 + "'");
            insertQuery.Append(",'" + row.CCACTUAL19 + "'");
            insertQuery.Append(",'" + row.CCACTUAL20 + "'");
            insertQuery.Append(",'" + row.CCACTUAL21 + "'");
            insertQuery.Append(",'" + row.CCACTUAL22 + "'");
            insertQuery.Append(",'" + row.CCACTUAL23 + "'");
            insertQuery.Append(",'" + row.CCACTUAL24 + "'");
            insertQuery.Append(",'" + row.CCACTUAL25 + "'");
            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------

            if (string.IsNullOrEmpty(hard_std1_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD1 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std1_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std2_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD2 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std2_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std3_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD3 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std3_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std4_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD4 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std4_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std5_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD5 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std5_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std6_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD6 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std6_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std7_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD7 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std7_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std8_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD8 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std8_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std9_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD9 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std9_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std10_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD10 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std10_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std11_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD11 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std11_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std12_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD12 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std12_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std13_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD13 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std13_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std13_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD14 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std14_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std15_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD15 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std15_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std16_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD16 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std16_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std17_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD17 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std17_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std18_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD18 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std18_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std19_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD19 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std19_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std20_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD20 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std20_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std21_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD21 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std21_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std22_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD22 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std22_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std23_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD23 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std23_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std24_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD24 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std24_fixed + "'");
            }

            if (string.IsNullOrEmpty(hard_std25_fixed))
            {
                insertQuery.Append(",'" + row.HARD_STD25 + "'");
            }
            else
            {
                insertQuery.Append(",'" + hard_std25_fixed + "'");
            }
            //insertQuery.Append(",'" + row.HARD_STD1 + "'");
            //insertQuery.Append(",'" + row.HARD_STD2 + "'");
            //insertQuery.Append(",'" + row.HARD_STD3 + "'");
            //insertQuery.Append(",'" + row.HARD_STD4 + "'");
            //insertQuery.Append(",'" + row.HARD_STD5 + "'");
            //insertQuery.Append(",'" + row.HARD_STD6 + "'");
            //insertQuery.Append(",'" + row.HARD_STD7 + "'");
            //insertQuery.Append(",'" + row.HARD_STD8 + "'");
            //insertQuery.Append(",'" + row.HARD_STD9 + "'");
            //insertQuery.Append(",'" + row.HARD_STD10 + "'");
            //insertQuery.Append(",'" + row.HARD_STD11 + "'");
            //insertQuery.Append(",'" + row.HARD_STD12 + "'");
            //insertQuery.Append(",'" + row.HARD_STD13 + "'");
            //insertQuery.Append(",'" + row.HARD_STD14 + "'");
            //insertQuery.Append(",'" + row.HARD_STD15 + "'");
            //insertQuery.Append(",'" + row.HARD_STD16 + "'");
            //insertQuery.Append(",'" + row.HARD_STD17 + "'");
            //insertQuery.Append(",'" + row.HARD_STD18 + "'");
            //insertQuery.Append(",'" + row.HARD_STD19 + "'");
            //insertQuery.Append(",'" + row.HARD_STD20 + "'");
            //insertQuery.Append(",'" + row.HARD_STD21 + "'");
            //insertQuery.Append(",'" + row.HARD_STD22 + "'");
            //insertQuery.Append(",'" + row.HARD_STD23 + "'");
            //insertQuery.Append(",'" + row.HARD_STD24 + "'");
            //insertQuery.Append(",'" + row.HARD_STD25 + "'");
            insertQuery.Append(",'" + row.HARD_MIN1 + "'");
            insertQuery.Append(",'" + row.HARD_MIN2 + "'");
            insertQuery.Append(",'" + row.HARD_MIN3 + "'");
            insertQuery.Append(",'" + row.HARD_MIN4 + "'");
            insertQuery.Append(",'" + row.HARD_MIN5 + "'");
            insertQuery.Append(",'" + row.HARD_MIN6 + "'");
            insertQuery.Append(",'" + row.HARD_MIN7 + "'");
            insertQuery.Append(",'" + row.HARD_MIN8 + "'");
            insertQuery.Append(",'" + row.HARD_MIN9 + "'");
            insertQuery.Append(",'" + row.HARD_MIN10 + "'");
            insertQuery.Append(",'" + row.HARD_MIN11 + "'");
            insertQuery.Append(",'" + row.HARD_MIN12 + "'");
            insertQuery.Append(",'" + row.HARD_MIN13 + "'");
            insertQuery.Append(",'" + row.HARD_MIN14 + "'");
            insertQuery.Append(",'" + row.HARD_MIN15 + "'");
            insertQuery.Append(",'" + row.HARD_MIN16 + "'");
            insertQuery.Append(",'" + row.HARD_MIN17 + "'");
            insertQuery.Append(",'" + row.HARD_MIN18 + "'");
            insertQuery.Append(",'" + row.HARD_MIN19 + "'");
            insertQuery.Append(",'" + row.HARD_MIN20 + "'");
            insertQuery.Append(",'" + row.HARD_MIN21 + "'");
            insertQuery.Append(",'" + row.HARD_MIN22 + "'");
            insertQuery.Append(",'" + row.HARD_MIN23 + "'");
            insertQuery.Append(",'" + row.HARD_MIN24 + "'");
            insertQuery.Append(",'" + row.HARD_MIN25 + "'");
            insertQuery.Append(",'" + row.HARD_MAX1 + "'");
            insertQuery.Append(",'" + row.HARD_MAX2 + "'");
            insertQuery.Append(",'" + row.HARD_MAX3 + "'");
            insertQuery.Append(",'" + row.HARD_MAX4 + "'");
            insertQuery.Append(",'" + row.HARD_MAX5 + "'");
            insertQuery.Append(",'" + row.HARD_MAX6 + "'");
            insertQuery.Append(",'" + row.HARD_MAX7 + "'");
            insertQuery.Append(",'" + row.HARD_MAX8 + "'");
            insertQuery.Append(",'" + row.HARD_MAX9 + "'");
            insertQuery.Append(",'" + row.HARD_MAX10 + "'");
            insertQuery.Append(",'" + row.HARD_MAX11 + "'");
            insertQuery.Append(",'" + row.HARD_MAX12 + "'");
            insertQuery.Append(",'" + row.HARD_MAX13 + "'");
            insertQuery.Append(",'" + row.HARD_MAX14 + "'");
            insertQuery.Append(",'" + row.HARD_MAX15 + "'");
            insertQuery.Append(",'" + row.HARD_MAX16 + "'");
            insertQuery.Append(",'" + row.HARD_MAX17 + "'");
            insertQuery.Append(",'" + row.HARD_MAX18 + "'");
            insertQuery.Append(",'" + row.HARD_MAX19 + "'");
            insertQuery.Append(",'" + row.HARD_MAX20 + "'");
            insertQuery.Append(",'" + row.HARD_MAX21 + "'");
            insertQuery.Append(",'" + row.HARD_MAX22 + "'");
            insertQuery.Append(",'" + row.HARD_MAX23 + "'");
            insertQuery.Append(",'" + row.HARD_MAX24 + "'");
            insertQuery.Append(",'" + row.HARD_MAX25 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL1 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL2 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL3 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL4 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL5 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL6 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL7 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL8 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL9 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL10 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL11 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL12 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL13 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL14 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL15 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL16 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL17 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL18 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL19 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL20 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL21 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL22 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL23 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL24 + "'");
            //insertQuery.Append(",'" + row.HARD_ACTUAL25 + "'");
            insertQuery.Append(row.HARD_ACTUAL1 == null || row.HARD_ACTUAL1 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL1 + "'");
            insertQuery.Append(row.HARD_ACTUAL2 == null || row.HARD_ACTUAL2 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL2 + "'");
            insertQuery.Append(row.HARD_ACTUAL3 == null || row.HARD_ACTUAL3 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL3 + "'");
            insertQuery.Append(row.HARD_ACTUAL4 == null || row.HARD_ACTUAL4 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL4 + "'");
            insertQuery.Append(row.HARD_ACTUAL5 == null || row.HARD_ACTUAL5 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL5 + "'");
            insertQuery.Append(row.HARD_ACTUAL6 == null || row.HARD_ACTUAL6 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL6 + "'");
            insertQuery.Append(row.HARD_ACTUAL7 == null || row.HARD_ACTUAL7 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL7 + "'");
            insertQuery.Append(row.HARD_ACTUAL8 == null || row.HARD_ACTUAL8 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL8 + "'");
            insertQuery.Append(row.HARD_ACTUAL9 == null || row.HARD_ACTUAL9 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL9 + "'");
            insertQuery.Append(row.HARD_ACTUAL10== null || row.HARD_ACTUAL10== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL10+ "'");
            insertQuery.Append(row.HARD_ACTUAL11== null || row.HARD_ACTUAL11== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL11+ "'");
            insertQuery.Append(row.HARD_ACTUAL12== null || row.HARD_ACTUAL12== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL12+ "'");
            insertQuery.Append(row.HARD_ACTUAL13== null || row.HARD_ACTUAL13== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL13+ "'");
            insertQuery.Append(row.HARD_ACTUAL14== null || row.HARD_ACTUAL14== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL14+ "'");
            insertQuery.Append(row.HARD_ACTUAL15== null || row.HARD_ACTUAL15== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL15+ "'");
            insertQuery.Append(row.HARD_ACTUAL16 == null || row.HARD_ACTUAL16 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL16 + "'");
            insertQuery.Append(row.HARD_ACTUAL17 == null || row.HARD_ACTUAL17 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL17 + "'");
            insertQuery.Append(row.HARD_ACTUAL18 == null || row.HARD_ACTUAL18 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL18 + "'");
            insertQuery.Append(row.HARD_ACTUAL19 == null || row.HARD_ACTUAL19 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL19 + "'");
            insertQuery.Append(row.HARD_ACTUAL20== null || row.HARD_ACTUAL20== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL20 + "'");
            insertQuery.Append(row.HARD_ACTUAL21 == null || row.HARD_ACTUAL21 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL21 + "'");
            insertQuery.Append(row.HARD_ACTUAL22 == null || row.HARD_ACTUAL22 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL22 + "'");
            insertQuery.Append(row.HARD_ACTUAL23 == null || row.HARD_ACTUAL23 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL23 + "'");
            insertQuery.Append(row.HARD_ACTUAL24 == null || row.HARD_ACTUAL24 == 0 ? ",NULL" : ",'" + row.HARD_ACTUAL24 + "'");
            insertQuery.Append(row.HARD_ACTUAL25== null || row.HARD_ACTUAL25== 0 ? ",NULL" : ",'" + row.HARD_ACTUAL25 + "'");


            insertQuery.Append(",SYSDATE");
            //insertQuery.Append(",SYSDATE ");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            // result = GetChamferDetailsByChuubanGyoubanMachine(chamferDetails.PP_CHUUBAN,Convert.ToInt16( chamferDetails.PP_GYOUBAN), chamferDetails.MACHINE_CD.ToString(), "");

            // result = "success";
            //result = GetStamppingDetailsByChuuban(stamppingDetails.PP_CHUUBAN, stamppingDetails.PP_GYOUBAN.ToString(), stamppingDetails.MACHINE_CD.ToString());
            //  return result;
        }

        public string AddZeroForMul(int noOfZeros)
        {
            return "1" + new string('0', noOfZeros);
        }


        public List<MakerMillSheetEDIDetails> GetMakerLogTablesData(int runID)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * ");
            sb.Append("FROM MAKER_MILL_SHEET_LOG ");
            sb.Append("WHERE RUN_CD = ");
            sb.Append(runID);

            return ExecuteEdiQueryMultiResult<MakerMillSheetEDIDetails>(sb.ToString());
        }

        public void UpdateMakerMillSheetLog(long logCd, int status, string statusMark, string errorMessage)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE MAKER_MILL_SHEET_LOG ");
            updateQuery.Append(" SET STATUS = " + status);
            updateQuery.Append(" , STATUS_MARK = '" + statusMark + "'");
            updateQuery.Append(" , MESSAGE = '" + errorMessage + "'");
            updateQuery.Append(" WHERE MAKER_MILL_SHEET_LOG_CD = " + logCd);
            ExecuteNonQuery(updateQuery.ToString());
        }

        public bool IsDuplicateDataExist(long logCd)
        {

            StringBuilder selectSql = new StringBuilder();

            selectSql.Append("SELECT GET_DUPLICATE_EDIDATA_COUNT(GET_DUPLICATE_DATA(");
            selectSql.Append(logCd);
            selectSql.Append("), ");
            selectSql.Append(logCd);
            selectSql.Append(") AS DUPCOUNT FROM DUAL");

            try
            {
                int rowCount = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
                return rowCount > 0;
            }
            catch
            {
                throw;
            }

        }

        public bool IsDuplicateOCRExist(string charge, decimal? dim_d)
        {

            StringBuilder selectSql = new StringBuilder();

            selectSql.Append(" SELECT COUNT(*) FROM MAKER_MILL_SHEET MM ");
            selectSql.Append(" WHERE MM.CHARGE = '" + charge + "'");
            selectSql.Append(" AND MM.DIM_D = '" + dim_d + "'");
            selectSql.Append(" AND MM.IS_EDI_DATA = 0 ");
            selectSql.Append(" AND MM.CREATED_DATE > ADD_MONTHS(SYSDATE,-8) ");

            try
            {
                int rowCount = Convert.ToInt32(ExecuteScalar(selectSql.ToString()));
                return rowCount > 0;
            }
            catch
            {
                throw;
            }
        }

        public void MillSheetEDIDataInsert(MakerMillSheetEDIDetails row, int maker_cd, string maker_name, int batchID, int status, string statusMark, string errorMessage, string pdfFileStorePath,int Location_CD)   //long makerCd,
        {
            makerMillSheet_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Maker_Mill_Sheet").Value);

            MakerMillSheetEDIDetails result = new MakerMillSheetEDIDetails();
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO MAKER_MILL_SHEET(");
            insertQuery.Append(" MAKER_MILL_SHEET_CD");
            insertQuery.Append(" ,MAKER_CD ");
            insertQuery.Append(" ,MAKER_NAME ");
            insertQuery.Append(" ,PDF_FILE_PATH ");
            insertQuery.Append(" ,FILENAME ");
            // insertQuery.Append(" ,PDF_PAGE_NUMBER ");
            insertQuery.Append(" ,KOUBAN");
            insertQuery.Append(" ,CHARGE");
            insertQuery.Append(" ,MATL_NAME_MAKER");
            insertQuery.Append(" ,DIM_D ");
            insertQuery.Append(" ,LENGTH");
            insertQuery.Append(" ,NO_OF_UNITS");
            insertQuery.Append(" ,UNIT");
            insertQuery.Append(" ,QUANTITY");
            insertQuery.Append(" ,RUN_CD");
            insertQuery.Append(" ,GRAIN_SIZE_STD");
            insertQuery.Append(" ,GRAIN_SIZE_ACTUAL");
            insertQuery.Append(" ,HARDNESS_STD");
            insertQuery.Append(" ,HARDNESS_MIN");
            insertQuery.Append(" ,HARDNESS_MAX");
            insertQuery.Append(" ,HARDNESS_ACTUAL");
            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
            insertQuery.Append(" ,CCNAME1");
            insertQuery.Append(" ,CCNAME2");
            insertQuery.Append(" ,CCNAME3");
            insertQuery.Append(" ,CCNAME4");
            insertQuery.Append(" ,CCNAME5");
            insertQuery.Append(" ,CCNAME6");
            insertQuery.Append(" ,CCNAME7");
            insertQuery.Append(" ,CCNAME8");
            insertQuery.Append(" ,CCNAME9");
            insertQuery.Append(" ,CCNAME10");
            insertQuery.Append(" ,CCNAME11");
            insertQuery.Append(" ,CCNAME12");
            insertQuery.Append(" ,CCNAME13");
            insertQuery.Append(" ,CCNAME14");
            insertQuery.Append(" ,CCNAME15");
            insertQuery.Append(" ,CCNAME16");
            insertQuery.Append(" ,CCNAME17");
            insertQuery.Append(" ,CCNAME18");
            insertQuery.Append(" ,CCNAME19");
            insertQuery.Append(" ,CCNAME20");
            insertQuery.Append(" ,CCNAME21");
            insertQuery.Append(" ,CCNAME22");
            insertQuery.Append(" ,CCNAME23");
            insertQuery.Append(" ,CCNAME24");
            insertQuery.Append(" ,CCNAME25");
            insertQuery.Append(" ,CCRANGE1");
            insertQuery.Append(" ,CCRANGE2");
            insertQuery.Append(" ,CCRANGE3");
            insertQuery.Append(" ,CCRANGE4");
            insertQuery.Append(" ,CCRANGE5");
            insertQuery.Append(" ,CCRANGE6");
            insertQuery.Append(" ,CCRANGE7");
            insertQuery.Append(" ,CCRANGE8");
            insertQuery.Append(" ,CCRANGE9");
            insertQuery.Append(" ,CCRANGE10");
            insertQuery.Append(" ,CCRANGE11");
            insertQuery.Append(" ,CCRANGE12");
            insertQuery.Append(" ,CCRANGE13");
            insertQuery.Append(" ,CCRANGE14");
            insertQuery.Append(" ,CCRANGE15");
            insertQuery.Append(" ,CCRANGE16");
            insertQuery.Append(" ,CCRANGE17");
            insertQuery.Append(" ,CCRANGE18");
            insertQuery.Append(" ,CCRANGE19");
            insertQuery.Append(" ,CCRANGE20");
            insertQuery.Append(" ,CCRANGE21");
            insertQuery.Append(" ,CCRANGE22");
            insertQuery.Append(" ,CCRANGE23");
            insertQuery.Append(" ,CCRANGE24");
            insertQuery.Append(" ,CCRANGE25");
            insertQuery.Append(" ,CCMIN1");
            insertQuery.Append(" ,CCMIN2");
            insertQuery.Append(" ,CCMIN3");
            insertQuery.Append(" ,CCMIN4");
            insertQuery.Append(" ,CCMIN5");
            insertQuery.Append(" ,CCMIN6");
            insertQuery.Append(" ,CCMIN7");
            insertQuery.Append(" ,CCMIN8");
            insertQuery.Append(" ,CCMIN9");
            insertQuery.Append(" ,CCMIN10");
            insertQuery.Append(" ,CCMIN11");
            insertQuery.Append(" ,CCMIN12");
            insertQuery.Append(" ,CCMIN13");
            insertQuery.Append(" ,CCMIN14");
            insertQuery.Append(" ,CCMIN15");
            insertQuery.Append(" ,CCMIN16");
            insertQuery.Append(" ,CCMIN17");
            insertQuery.Append(" ,CCMIN18");
            insertQuery.Append(" ,CCMIN19");
            insertQuery.Append(" ,CCMIN20");
            insertQuery.Append(" ,CCMIN21");
            insertQuery.Append(" ,CCMIN22");
            insertQuery.Append(" ,CCMIN23");
            insertQuery.Append(" ,CCMIN24");
            insertQuery.Append(" ,CCMIN25");
            insertQuery.Append(" ,CCMAX1");
            insertQuery.Append(" ,CCMAX2");
            insertQuery.Append(" ,CCMAX3");
            insertQuery.Append(" ,CCMAX4");
            insertQuery.Append(" ,CCMAX5");
            insertQuery.Append(" ,CCMAX6");
            insertQuery.Append(" ,CCMAX7");
            insertQuery.Append(" ,CCMAX8");
            insertQuery.Append(" ,CCMAX9");
            insertQuery.Append(" ,CCMAX10");
            insertQuery.Append(" ,CCMAX11");
            insertQuery.Append(" ,CCMAX12");
            insertQuery.Append(" ,CCMAX13");
            insertQuery.Append(" ,CCMAX14");
            insertQuery.Append(" ,CCMAX15");
            insertQuery.Append(" ,CCMAX16");
            insertQuery.Append(" ,CCMAX17");
            insertQuery.Append(" ,CCMAX18");
            insertQuery.Append(" ,CCMAX19");
            insertQuery.Append(" ,CCMAX20");
            insertQuery.Append(" ,CCMAX21");
            insertQuery.Append(" ,CCMAX22");
            insertQuery.Append(" ,CCMAX23");
            insertQuery.Append(" ,CCMAX24");
            insertQuery.Append(" ,CCMAX25");
            insertQuery.Append(" ,CCACTUAL1");
            insertQuery.Append(" ,CCACTUAL2");
            insertQuery.Append(" ,CCACTUAL3");
            insertQuery.Append(" ,CCACTUAL4");
            insertQuery.Append(" ,CCACTUAL5");
            insertQuery.Append(" ,CCACTUAL6");
            insertQuery.Append(" ,CCACTUAL7");
            insertQuery.Append(" ,CCACTUAL8");
            insertQuery.Append(" ,CCACTUAL9");
            insertQuery.Append(" ,CCACTUAL10");
            insertQuery.Append(" ,CCACTUAL11");
            insertQuery.Append(" ,CCACTUAL12");
            insertQuery.Append(" ,CCACTUAL13");
            insertQuery.Append(" ,CCACTUAL14");
            insertQuery.Append(" ,CCACTUAL15");
            insertQuery.Append(" ,CCACTUAL16");
            insertQuery.Append(" ,CCACTUAL17");
            insertQuery.Append(" ,CCACTUAL18");
            insertQuery.Append(" ,CCACTUAL19");
            insertQuery.Append(" ,CCACTUAL20");
            insertQuery.Append(" ,CCACTUAL21");
            insertQuery.Append(" ,CCACTUAL22");
            insertQuery.Append(" ,CCACTUAL23");
            insertQuery.Append(" ,CCACTUAL24");
            insertQuery.Append(" ,CCACTUAL25");
            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------

            insertQuery.Append(" ,HARD_STD1");
            insertQuery.Append(" ,HARD_STD2");
            insertQuery.Append(" ,HARD_STD3");
            insertQuery.Append(" ,HARD_STD4");
            insertQuery.Append(" ,HARD_STD5");
            insertQuery.Append(" ,HARD_STD6");
            insertQuery.Append(" ,HARD_STD7");
            insertQuery.Append(" ,HARD_STD8");
            insertQuery.Append(" ,HARD_STD9");
            insertQuery.Append(" ,HARD_STD10");
            insertQuery.Append(" ,HARD_STD11");
            insertQuery.Append(" ,HARD_STD12");
            insertQuery.Append(" ,HARD_STD13");
            insertQuery.Append(" ,HARD_STD14");
            insertQuery.Append(" ,HARD_STD15");
            insertQuery.Append(" ,HARD_STD16");
            insertQuery.Append(" ,HARD_STD17");
            insertQuery.Append(" ,HARD_STD18");
            insertQuery.Append(" ,HARD_STD19");
            insertQuery.Append(" ,HARD_STD20");
            insertQuery.Append(" ,HARD_STD21");
            insertQuery.Append(" ,HARD_STD22");
            insertQuery.Append(" ,HARD_STD23");
            insertQuery.Append(" ,HARD_STD24");
            insertQuery.Append(" ,HARD_STD25");
            insertQuery.Append(" ,HARD_MIN1");
            insertQuery.Append(" ,HARD_MIN2");
            insertQuery.Append(" ,HARD_MIN3");
            insertQuery.Append(" ,HARD_MIN4");
            insertQuery.Append(" ,HARD_MIN5");
            insertQuery.Append(" ,HARD_MIN6");
            insertQuery.Append(" ,HARD_MIN7");
            insertQuery.Append(" ,HARD_MIN8");
            insertQuery.Append(" ,HARD_MIN9");
            insertQuery.Append(" ,HARD_MIN10");
            insertQuery.Append(" ,HARD_MIN11");
            insertQuery.Append(" ,HARD_MIN12");
            insertQuery.Append(" ,HARD_MIN13");
            insertQuery.Append(" ,HARD_MIN14");
            insertQuery.Append(" ,HARD_MIN15");
            insertQuery.Append(" ,HARD_MIN16");
            insertQuery.Append(" ,HARD_MIN17");
            insertQuery.Append(" ,HARD_MIN18");
            insertQuery.Append(" ,HARD_MIN19");
            insertQuery.Append(" ,HARD_MIN20");
            insertQuery.Append(" ,HARD_MIN21");
            insertQuery.Append(" ,HARD_MIN22");
            insertQuery.Append(" ,HARD_MIN23");
            insertQuery.Append(" ,HARD_MIN24");
            insertQuery.Append(" ,HARD_MIN25");
            insertQuery.Append(" ,HARD_MAX1");
            insertQuery.Append(" ,HARD_MAX2");
            insertQuery.Append(" ,HARD_MAX3");
            insertQuery.Append(" ,HARD_MAX4");
            insertQuery.Append(" ,HARD_MAX5");
            insertQuery.Append(" ,HARD_MAX6");
            insertQuery.Append(" ,HARD_MAX7");
            insertQuery.Append(" ,HARD_MAX8");
            insertQuery.Append(" ,HARD_MAX9");
            insertQuery.Append(" ,HARD_MAX10");
            insertQuery.Append(" ,HARD_MAX11");
            insertQuery.Append(" ,HARD_MAX12");
            insertQuery.Append(" ,HARD_MAX13");
            insertQuery.Append(" ,HARD_MAX14");
            insertQuery.Append(" ,HARD_MAX15");
            insertQuery.Append(" ,HARD_MAX16");
            insertQuery.Append(" ,HARD_MAX17");
            insertQuery.Append(" ,HARD_MAX18");
            insertQuery.Append(" ,HARD_MAX19");
            insertQuery.Append(" ,HARD_MAX20");
            insertQuery.Append(" ,HARD_MAX21");
            insertQuery.Append(" ,HARD_MAX22");
            insertQuery.Append(" ,HARD_MAX23");
            insertQuery.Append(" ,HARD_MAX24");
            insertQuery.Append(" ,HARD_MAX25");
            insertQuery.Append(" ,HARD_ACTUAL1");
            insertQuery.Append(" ,HARD_ACTUAL2");
            insertQuery.Append(" ,HARD_ACTUAL3");
            insertQuery.Append(" ,HARD_ACTUAL4");
            insertQuery.Append(" ,HARD_ACTUAL5");
            insertQuery.Append(" ,HARD_ACTUAL6");
            insertQuery.Append(" ,HARD_ACTUAL7");
            insertQuery.Append(" ,HARD_ACTUAL8");
            insertQuery.Append(" ,HARD_ACTUAL9");
            insertQuery.Append(" ,HARD_ACTUAL10");
            insertQuery.Append(" ,HARD_ACTUAL11");
            insertQuery.Append(" ,HARD_ACTUAL12");
            insertQuery.Append(" ,HARD_ACTUAL13");
            insertQuery.Append(" ,HARD_ACTUAL14");
            insertQuery.Append(" ,HARD_ACTUAL15");
            insertQuery.Append(" ,HARD_ACTUAL16");
            insertQuery.Append(" ,HARD_ACTUAL17");
            insertQuery.Append(" ,HARD_ACTUAL18");
            insertQuery.Append(" ,HARD_ACTUAL19");
            insertQuery.Append(" ,HARD_ACTUAL20");
            insertQuery.Append(" ,HARD_ACTUAL21");
            insertQuery.Append(" ,HARD_ACTUAL22");
            insertQuery.Append(" ,HARD_ACTUAL23");
            insertQuery.Append(" ,HARD_ACTUAL24");
            insertQuery.Append(" ,HARD_ACTUAL25");
            insertQuery.Append(" ,INPUT_LOCATION_CD");
            insertQuery.Append(" ,CREATED_DATE");
            insertQuery.Append(" ,UPDATED_DATE ");
            insertQuery.Append(" ,STATUS");
            insertQuery.Append(" ,STATUS_MARK ");
            insertQuery.Append(" ,MESSAGE");
            insertQuery.Append(" ,IS_EDI_DATA");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            //insertQuery.Append(" MAKER_MILL_SHEET_SEQ.NEXTVAL ");   //Commented to make web and win data in sync 
            if (string.IsNullOrEmpty(makerMillSheet_Seq))
            {
                insertQuery.Append(" MAKER_MILL_SHEET_SEQ.NEXTVAL ");
            }
            else
            {
                insertQuery.Append(makerMillSheet_Seq);
            }
            insertQuery.Append(",'" + maker_cd + "'");
            insertQuery.Append(",'" + maker_name + "'");
            insertQuery.Append(",'" + row.PDF_FILE_PATH + "'");
            insertQuery.Append(",'" + row.FILENAME + "'");
            // insertQuery.Append(",'" + makermillsheetDetails.PDF_PAGE_NUMBER + "'");
            insertQuery.Append(",'" + row.KOUBAN + "'");
            insertQuery.Append(",'" + row.CHARGE + "'");
            insertQuery.Append(",'" + row.MATL_NAME_MAKER + "'");
            insertQuery.Append(",'" + row.DIM_D + "'");
            insertQuery.Append(",'" + row.LENGTH + "'");
            insertQuery.Append(",'" + row.NO_OF_UNITS + "'");
            insertQuery.Append(",'" + row.UNIT + "'");
            insertQuery.Append(",'" + row.QUANTITY + "'");
            insertQuery.Append(",'" + batchID + "'");
            insertQuery.Append(",'" + row.GRAIN_SIZE_STD + "'");
            insertQuery.Append(",'" + row.GRAIN_SIZE_ACTUAL + "'");
            insertQuery.Append(",'" + row.HARDNESS_STD + "'");
            insertQuery.Append(",'" + row.HARDNESS_MIN + "'");
            insertQuery.Append(",'" + row.HARDNESS_MAX + "'");
            insertQuery.Append(",'" + row.HARDNESS_ACTUAL + "'");
            // -----------------------row-------------------- Chemical Composition Detail ------------------------------------------------
            insertQuery.Append(",'" + row.CCNAME1 + "'");
            insertQuery.Append(",'" + row.CCNAME2 + "'");
            insertQuery.Append(",'" + row.CCNAME3 + "'");
            insertQuery.Append(",'" + row.CCNAME4 + "'");
            insertQuery.Append(",'" + row.CCNAME5 + "'");
            insertQuery.Append(",'" + row.CCNAME6 + "'");
            insertQuery.Append(",'" + row.CCNAME7 + "'");
            insertQuery.Append(",'" + row.CCNAME8 + "'");
            insertQuery.Append(",'" + row.CCNAME9 + "'");
            insertQuery.Append(",'" + row.CCNAME10 + "'");
            insertQuery.Append(",'" + row.CCNAME11 + "'");
            insertQuery.Append(",'" + row.CCNAME12 + "'");
            insertQuery.Append(",'" + row.CCNAME13 + "'");
            insertQuery.Append(",'" + row.CCNAME14 + "'");
            insertQuery.Append(",'" + row.CCNAME15 + "'");
            insertQuery.Append(",'" + row.CCNAME16 + "'");
            insertQuery.Append(",'" + row.CCNAME17 + "'");
            insertQuery.Append(",'" + row.CCNAME18 + "'");
            insertQuery.Append(",'" + row.CCNAME19 + "'");
            insertQuery.Append(",'" + row.CCNAME20 + "'");
            insertQuery.Append(",'" + row.CCNAME21 + "'");
            insertQuery.Append(",'" + row.CCNAME22 + "'");
            insertQuery.Append(",'" + row.CCNAME23 + "'");
            insertQuery.Append(",'" + row.CCNAME24 + "'");
            insertQuery.Append(",'" + row.CCNAME25 + "'");
            insertQuery.Append(",'" + row.CCRANGE1 + "'");
            insertQuery.Append(",'" + row.CCRANGE2 + "'");
            insertQuery.Append(",'" + row.CCRANGE3 + "'");
            insertQuery.Append(",'" + row.CCRANGE4 + "'");
            insertQuery.Append(",'" + row.CCRANGE5 + "'");
            insertQuery.Append(",'" + row.CCRANGE6 + "'");
            insertQuery.Append(",'" + row.CCRANGE7 + "'");
            insertQuery.Append(",'" + row.CCRANGE8 + "'");
            insertQuery.Append(",'" + row.CCRANGE9 + "'");
            insertQuery.Append(",'" + row.CCRANGE10 + "'");
            insertQuery.Append(",'" + row.CCRANGE11 + "'");
            insertQuery.Append(",'" + row.CCRANGE12 + "'");
            insertQuery.Append(",'" + row.CCRANGE13 + "'");
            insertQuery.Append(",'" + row.CCRANGE14 + "'");
            insertQuery.Append(",'" + row.CCRANGE15 + "'");
            insertQuery.Append(",'" + row.CCRANGE16 + "'");
            insertQuery.Append(",'" + row.CCRANGE17 + "'");
            insertQuery.Append(",'" + row.CCRANGE18 + "'");
            insertQuery.Append(",'" + row.CCRANGE19 + "'");
            insertQuery.Append(",'" + row.CCRANGE20 + "'");
            insertQuery.Append(",'" + row.CCRANGE21 + "'");
            insertQuery.Append(",'" + row.CCRANGE22 + "'");
            insertQuery.Append(",'" + row.CCRANGE23 + "'");
            insertQuery.Append(",'" + row.CCRANGE24 + "'");
            insertQuery.Append(",'" + row.CCRANGE25 + "'");
            insertQuery.Append(",'" + row.CCMIN1 + "'");
            insertQuery.Append(",'" + row.CCMIN2 + "'");
            insertQuery.Append(",'" + row.CCMIN3 + "'");
            insertQuery.Append(",'" + row.CCMIN4 + "'");
            insertQuery.Append(",'" + row.CCMIN5 + "'");
            insertQuery.Append(",'" + row.CCMIN6 + "'");
            insertQuery.Append(",'" + row.CCMIN7 + "'");
            insertQuery.Append(",'" + row.CCMIN8 + "'");
            insertQuery.Append(",'" + row.CCMIN9 + "'");
            insertQuery.Append(",'" + row.CCMIN10 + "'");
            insertQuery.Append(",'" + row.CCMIN11 + "'");
            insertQuery.Append(",'" + row.CCMIN12 + "'");
            insertQuery.Append(",'" + row.CCMIN13 + "'");
            insertQuery.Append(",'" + row.CCMIN14 + "'");
            insertQuery.Append(",'" + row.CCMIN15 + "'");
            insertQuery.Append(",'" + row.CCMIN16 + "'");
            insertQuery.Append(",'" + row.CCMIN17 + "'");
            insertQuery.Append(",'" + row.CCMIN18 + "'");
            insertQuery.Append(",'" + row.CCMIN19 + "'");
            insertQuery.Append(",'" + row.CCMIN20 + "'");
            insertQuery.Append(",'" + row.CCMIN21 + "'");
            insertQuery.Append(",'" + row.CCMIN22 + "'");
            insertQuery.Append(",'" + row.CCMIN23 + "'");
            insertQuery.Append(",'" + row.CCMIN24 + "'");
            insertQuery.Append(",'" + row.CCMIN25 + "'");
            insertQuery.Append(",'" + row.CCMAX1 + "'");
            insertQuery.Append(",'" + row.CCMAX2 + "'");
            insertQuery.Append(",'" + row.CCMAX3 + "'");
            insertQuery.Append(",'" + row.CCMAX4 + "'");
            insertQuery.Append(",'" + row.CCMAX5 + "'");
            insertQuery.Append(",'" + row.CCMAX6 + "'");
            insertQuery.Append(",'" + row.CCMAX7 + "'");
            insertQuery.Append(",'" + row.CCMAX8 + "'");
            insertQuery.Append(",'" + row.CCMAX9 + "'");
            insertQuery.Append(",'" + row.CCMAX10 + "'");
            insertQuery.Append(",'" + row.CCMAX11 + "'");
            insertQuery.Append(",'" + row.CCMAX12 + "'");
            insertQuery.Append(",'" + row.CCMAX13 + "'");
            insertQuery.Append(",'" + row.CCMAX14 + "'");
            insertQuery.Append(",'" + row.CCMAX15 + "'");
            insertQuery.Append(",'" + row.CCMAX16 + "'");
            insertQuery.Append(",'" + row.CCMAX17 + "'");
            insertQuery.Append(",'" + row.CCMAX18 + "'");
            insertQuery.Append(",'" + row.CCMAX19 + "'");
            insertQuery.Append(",'" + row.CCMAX20 + "'");
            insertQuery.Append(",'" + row.CCMAX21 + "'");
            insertQuery.Append(",'" + row.CCMAX22 + "'");
            insertQuery.Append(",'" + row.CCMAX23 + "'");
            insertQuery.Append(",'" + row.CCMAX24 + "'");
            insertQuery.Append(",'" + row.CCMAX25 + "'");
            insertQuery.Append(",'" + row.CCACTUAL1 + "'");
            insertQuery.Append(",'" + row.CCACTUAL2 + "'");
            insertQuery.Append(",'" + row.CCACTUAL3 + "'");
            insertQuery.Append(",'" + row.CCACTUAL4 + "'");
            insertQuery.Append(",'" + row.CCACTUAL5 + "'");
            insertQuery.Append(",'" + row.CCACTUAL6 + "'");
            insertQuery.Append(",'" + row.CCACTUAL7 + "'");
            insertQuery.Append(",'" + row.CCACTUAL8 + "'");
            insertQuery.Append(",'" + row.CCACTUAL9 + "'");
            insertQuery.Append(",'" + row.CCACTUAL10 + "'");
            insertQuery.Append(",'" + row.CCACTUAL11 + "'");
            insertQuery.Append(",'" + row.CCACTUAL12 + "'");
            insertQuery.Append(",'" + row.CCACTUAL13 + "'");
            insertQuery.Append(",'" + row.CCACTUAL14 + "'");
            insertQuery.Append(",'" + row.CCACTUAL15 + "'");
            insertQuery.Append(",'" + row.CCACTUAL16 + "'");
            insertQuery.Append(",'" + row.CCACTUAL17 + "'");
            insertQuery.Append(",'" + row.CCACTUAL18 + "'");
            insertQuery.Append(",'" + row.CCACTUAL19 + "'");
            insertQuery.Append(",'" + row.CCACTUAL20 + "'");
            insertQuery.Append(",'" + row.CCACTUAL21 + "'");
            insertQuery.Append(",'" + row.CCACTUAL22 + "'");
            insertQuery.Append(",'" + row.CCACTUAL23 + "'");
            insertQuery.Append(",'" + row.CCACTUAL24 + "'");
            insertQuery.Append(",'" + row.CCACTUAL25 + "'");
            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------
            insertQuery.Append(",'" + row.HARD_STD1 + "'");
            insertQuery.Append(",'" + row.HARD_STD2 + "'");
            insertQuery.Append(",'" + row.HARD_STD3 + "'");
            insertQuery.Append(",'" + row.HARD_STD4 + "'");
            insertQuery.Append(",'" + row.HARD_STD5 + "'");
            insertQuery.Append(",'" + row.HARD_STD6 + "'");
            insertQuery.Append(",'" + row.HARD_STD7 + "'");
            insertQuery.Append(",'" + row.HARD_STD8 + "'");
            insertQuery.Append(",'" + row.HARD_STD9 + "'");
            insertQuery.Append(",'" + row.HARD_STD10 + "'");
            insertQuery.Append(",'" + row.HARD_STD11 + "'");
            insertQuery.Append(",'" + row.HARD_STD12 + "'");
            insertQuery.Append(",'" + row.HARD_STD13 + "'");
            insertQuery.Append(",'" + row.HARD_STD14 + "'");
            insertQuery.Append(",'" + row.HARD_STD15 + "'");
            insertQuery.Append(",'" + row.HARD_STD16 + "'");
            insertQuery.Append(",'" + row.HARD_STD17 + "'");
            insertQuery.Append(",'" + row.HARD_STD18 + "'");
            insertQuery.Append(",'" + row.HARD_STD19 + "'");
            insertQuery.Append(",'" + row.HARD_STD20 + "'");
            insertQuery.Append(",'" + row.HARD_STD21 + "'");
            insertQuery.Append(",'" + row.HARD_STD22 + "'");
            insertQuery.Append(",'" + row.HARD_STD23 + "'");
            insertQuery.Append(",'" + row.HARD_STD24 + "'");
            insertQuery.Append(",'" + row.HARD_STD25 + "'");
            insertQuery.Append(",'" + row.HARD_MIN1 + "'");
            insertQuery.Append(",'" + row.HARD_MIN2 + "'");
            insertQuery.Append(",'" + row.HARD_MIN3 + "'");
            insertQuery.Append(",'" + row.HARD_MIN4 + "'");
            insertQuery.Append(",'" + row.HARD_MIN5 + "'");
            insertQuery.Append(",'" + row.HARD_MIN6 + "'");
            insertQuery.Append(",'" + row.HARD_MIN7 + "'");
            insertQuery.Append(",'" + row.HARD_MIN8 + "'");
            insertQuery.Append(",'" + row.HARD_MIN9 + "'");
            insertQuery.Append(",'" + row.HARD_MIN10 + "'");
            insertQuery.Append(",'" + row.HARD_MIN11 + "'");
            insertQuery.Append(",'" + row.HARD_MIN12 + "'");
            insertQuery.Append(",'" + row.HARD_MIN13 + "'");
            insertQuery.Append(",'" + row.HARD_MIN14 + "'");
            insertQuery.Append(",'" + row.HARD_MIN15 + "'");
            insertQuery.Append(",'" + row.HARD_MIN16 + "'");
            insertQuery.Append(",'" + row.HARD_MIN17 + "'");
            insertQuery.Append(",'" + row.HARD_MIN18 + "'");
            insertQuery.Append(",'" + row.HARD_MIN19 + "'");
            insertQuery.Append(",'" + row.HARD_MIN20 + "'");
            insertQuery.Append(",'" + row.HARD_MIN21 + "'");
            insertQuery.Append(",'" + row.HARD_MIN22 + "'");
            insertQuery.Append(",'" + row.HARD_MIN23 + "'");
            insertQuery.Append(",'" + row.HARD_MIN24 + "'");
            insertQuery.Append(",'" + row.HARD_MIN25 + "'");
            insertQuery.Append(",'" + row.HARD_MAX1 + "'");
            insertQuery.Append(",'" + row.HARD_MAX2 + "'");
            insertQuery.Append(",'" + row.HARD_MAX3 + "'");
            insertQuery.Append(",'" + row.HARD_MAX4 + "'");
            insertQuery.Append(",'" + row.HARD_MAX5 + "'");
            insertQuery.Append(",'" + row.HARD_MAX6 + "'");
            insertQuery.Append(",'" + row.HARD_MAX7 + "'");
            insertQuery.Append(",'" + row.HARD_MAX8 + "'");
            insertQuery.Append(",'" + row.HARD_MAX9 + "'");
            insertQuery.Append(",'" + row.HARD_MAX10 + "'");
            insertQuery.Append(",'" + row.HARD_MAX11 + "'");
            insertQuery.Append(",'" + row.HARD_MAX12 + "'");
            insertQuery.Append(",'" + row.HARD_MAX13 + "'");
            insertQuery.Append(",'" + row.HARD_MAX14 + "'");
            insertQuery.Append(",'" + row.HARD_MAX15 + "'");
            insertQuery.Append(",'" + row.HARD_MAX16 + "'");
            insertQuery.Append(",'" + row.HARD_MAX17 + "'");
            insertQuery.Append(",'" + row.HARD_MAX18 + "'");
            insertQuery.Append(",'" + row.HARD_MAX19 + "'");
            insertQuery.Append(",'" + row.HARD_MAX20 + "'");
            insertQuery.Append(",'" + row.HARD_MAX21 + "'");
            insertQuery.Append(",'" + row.HARD_MAX22 + "'");
            insertQuery.Append(",'" + row.HARD_MAX23 + "'");
            insertQuery.Append(",'" + row.HARD_MAX24 + "'");
            insertQuery.Append(",'" + row.HARD_MAX25 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL1 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL2 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL3 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL4 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL5 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL6 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL7 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL8 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL9 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL10 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL11 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL12 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL13 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL14 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL15 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL16 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL17 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL18 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL19 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL20 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL21 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL22 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL23 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL24 + "'");
            insertQuery.Append(",'" + row.HARD_ACTUAL25 + "'");
            insertQuery.Append(",'" + Location_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + status + "'");
            insertQuery.Append(",'" + statusMark + "'");
            insertQuery.Append(",'" + errorMessage + "'");
            insertQuery.Append(",1");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());

        }
      
        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID, DateTime printReportDate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" MAKER_MILL_SHEET_LOG_CD, FILENAME, MAKER_NAME, MATL_NAME_MAKER, CHARGE, ");
            sb.Append(" DIM_D AS DIM_D ,");
            sb.Append(" LENGTH, NO_OF_UNITS, QUANTITY,  HARDNESS_STD, HARDNESS_MIN , ");
            sb.Append("(CASE   WHEN GRAIN_SIZE_STD IS NULL   THEN NULL WHEN  HSKWEBUSER.IS_NUMBER(GRAIN_SIZE_STD) = 1 THEN   TO_CHAR (GRAIN_SIZE_STD, 90.0)  ELSE   GRAIN_SIZE_STD   END  ) AS GRAIN_SIZE_STD , ");
            sb.Append(" (CASE   WHEN GRAIN_SIZE_ACTUAL IS NULL   THEN NULL WHEN  HSKWEBUSER.IS_NUMBER(GRAIN_SIZE_ACTUAL) = 1 THEN   TO_CHAR (GRAIN_SIZE_ACTUAL, 90.0)  ELSE   GRAIN_SIZE_ACTUAL   END  ) AS GRAIN_SIZE_ACTUAL, ");
            sb.Append(" HARDNESS_MAX, HARDNESS_ACTUAL, CCNAME1, 	");
            sb.Append(" (case when CCRANGE1 is null then null else '×'||CCRANGE1 end ) as CCRANGE1 , CCMIN1, CCMAX1,	");
            sb.Append("	      CCACTUAL1 	");
            sb.Append(" , CCNAME2, 	");
            sb.Append("  (case when CCRANGE2 is null then null else '×'||CCRANGE2 end ) as CCRANGE2, 	");
            sb.Append(" CCMIN2, CCMAX2, CCACTUAL2, CCNAME3,	");
            sb.Append("  (case when CCRANGE3 is null then null else '×'||CCRANGE3 end ) as CCRANGE3,	");
            sb.Append("  CCMIN3, CCMAX3, CCACTUAL3, CCNAME4, 	")                                        ;
            sb.Append("  (case when CCRANGE4 is null then null else '×'||CCRANGE4 end ) as CCRANGE4,	");
            sb.Append("   CCMIN4, CCMAX4,	")                                                            ;
            sb.Append(" CCACTUAL4, CCNAME5, 	")                                                        ;
            sb.Append(" (case when CCRANGE5 is null then null else '×'||CCRANGE5 end ) as CCRANGE5 ,	");
            sb.Append("  CCMIN5, CCMAX5, CCACTUAL5, CCNAME6,	")                                        ;
            sb.Append(" (case when CCRANGE6 is null then null else '×'||CCRANGE6 end ) as CCRANGE6 , 	");
            sb.Append(" CCMIN6, CCMAX6, CCACTUAL6, CCNAME7, 	")                                        ;
            sb.Append("  (case when CCRANGE7 is null then null else '×'||CCRANGE7 end ) as CCRANGE7,	");
            sb.Append("  CCMIN7, CCMAX7,       CCACTUAL7, CCNAME8,	");
            sb.Append(" (case when CCRANGE8 is null then null else '×'||CCRANGE8 end ) as CCRANGE8,       	");
            sb.Append("  CCMIN8, CCMAX8, CCACTUAL8, CCNAME9,	");
            sb.Append(" (case when CCRANGE9 is null then null else '×'||CCRANGE9 end ) as CCRANGE9,	");
            sb.Append(" CCMIN9, CCMAX9, CCACTUAL9, CCNAME10, CCMIN10,	");
            sb.Append(" CCMAX10, CCACTUAL10, CCNAME11, 	");
            sb.Append(" (case when CCRANGE10 is null then null else '×'||CCRANGE10 end ) as CCRANGE10,	");
            sb.Append("	      (case when CCRANGE11 is null then null else '×'||CCRANGE11 end ) as CCRANGE11,	");
            sb.Append(" CCMIN11, CCMAX11, CCACTUAL11,       CCNAME12, 	");
            sb.Append(" (case when CCRANGE12 is null then null else '×'||CCRANGE12 end ) as CCRANGE12, 	");
            sb.Append(" CCMIN12, CCMAX12, CCACTUAL12, CCNAME13, 	")                                    ;
            sb.Append(" (case when CCRANGE13 is null then null else '×'||CCRANGE13 end ) as CCRANGE13,	");
            sb.Append(" CCMIN13, CCMAX13, CCACTUAL13, CCNAME14, 	")                                    ;
            sb.Append(" (case when CCRANGE14 is null then null else '×'||CCRANGE14 end ) as CCRANGE14,	");
            sb.Append(" CCMIN14, CCMAX14,	")  ;
            sb.Append(" CCACTUAL14, CCNAME15, 	") ;
            sb.Append(" (case when CCRANGE15 is null then null else '×'||CCRANGE15 end ) as CCRANGE15,	");
            sb.Append("  CCMIN15, CCMAX15, CCACTUAL15,	");
            sb.Append(" CCNAME16, 	");                                                                   
            sb.Append(" (case when CCRANGE16 is null then null else '×'||CCRANGE16 end ) as CCRANGE16,	");
            sb.Append("  CCMIN16, CCMAX16, CCACTUAL16, CCNAME17, 	");
            sb.Append("	      (case when CCRANGE17 is null then null else '×'||CCRANGE17 end ) as CCRANGE17,	");
            sb.Append(" CCMIN17, CCMAX17, CCACTUAL17, CCNAME18, 	");
            sb.Append("	      (case when CCRANGE18 is null then null else '×'||CCRANGE18 end ) as CCRANGE18,	");
            sb.Append(" CCMIN18, CCMAX18,	");
            sb.Append(" CCACTUAL18, CCNAME19, 	");
            sb.Append(" (case when CCRANGE19 is null then null else '×'||CCRANGE19 end ) as CCRANGE19,	");
            sb.Append(" CCMIN19, CCMAX19, CCACTUAL19,	");
            sb.Append(" CCNAME20,	");
            sb.Append("  (case when CCRANGE20 is null then null else '×'||CCRANGE20 end ) as CCRANGE20,	");
            sb.Append("   CCMIN20, CCMAX20, CCACTUAL20, CCNAME21, 	");
            sb.Append("   (case when CCRANGE21 is null then null else '×'||CCRANGE21 end ) as CCRANGE21,	");
            sb.Append(" CCMIN21, CCMAX21, CCACTUAL21, CCNAME22, CCRANGE22, CCMIN22, CCMAX22,	");
            sb.Append(" CCACTUAL22, CCNAME23, 	");
            sb.Append("	      (case when CCRANGE23 is null then null else '×'||CCRANGE23 end ) as CCRANGE23,	");
            sb.Append(" CCMIN23, CCMAX23, CCACTUAL23,	");
            sb.Append(" CCNAME24, 	");
            sb.Append(" (case when CCRANGE24 is null then null else '×'||CCRANGE24 end ) as CCRANGE24,	");
            sb.Append(" CCMIN24, CCMAX24, CCACTUAL24, CCNAME25, 	");
            sb.Append(" (case when CCRANGE25 is null then null else '×'||CCRANGE25 end ) as CCRANGE25,	");
            sb.Append(" CCMIN25, CCMAX25, CCACTUAL25, HARD_STD1, HARD_MIN1, HARD_MAX1,	");
            sb.Append(" HARD_ACTUAL1, HARD_STD2, HARD_MIN2, HARD_MAX2, HARD_ACTUAL2, HARD_STD3,	");
            sb.Append(" HARD_MIN3, HARD_MAX3, HARD_ACTUAL3, HARD_STD4, HARD_MIN4, HARD_MAX4,	");
            sb.Append(" HARD_ACTUAL4, HARD_STD5, HARD_MIN5, HARD_MAX5, HARD_ACTUAL5, HARD_STD6,	");
            sb.Append(" HARD_MIN6, HARD_MAX6, HARD_ACTUAL6, HARD_STD7, HARD_MIN7, HARD_MAX7,	");
            sb.Append(" HARD_ACTUAL7, HARD_STD8, HARD_MIN8, HARD_MAX8, HARD_ACTUAL8, HARD_STD9,	");
            sb.Append(" HARD_MIN9, HARD_MAX9, HARD_ACTUAL9, HARD_STD10, HARD_MIN10, HARD_MAX10,	");
            sb.Append(" HARD_ACTUAL10, HARD_STD11, HARD_MIN11, HARD_MAX11, HARD_ACTUAL11,	");
            sb.Append(" HARD_STD12, HARD_MIN12, HARD_MAX12, HARD_ACTUAL12, HARD_STD13,	");
            sb.Append(" HARD_MIN13, HARD_MAX13, HARD_ACTUAL13, HARD_STD14, HARD_MIN14,	");
            sb.Append(" HARD_MAX14, HARD_ACTUAL14, HARD_STD15, HARD_MIN15, HARD_MAX15,	");
            sb.Append(" HARD_ACTUAL15, HARD_STD16, HARD_MIN16, HARD_MAX16, HARD_ACTUAL16,	");
            sb.Append(" HARD_STD17, HARD_MIN17, HARD_MAX17, HARD_ACTUAL17, HARD_STD18,	");
            sb.Append(" HARD_MIN18, HARD_MAX18, HARD_ACTUAL18, HARD_STD19, HARD_MIN19,	");
            sb.Append(" HARD_MAX19, HARD_ACTUAL19, HARD_STD20, HARD_MIN20, HARD_MAX20,	");
            sb.Append(" HARD_ACTUAL20, HARD_STD21, HARD_MIN21, HARD_MAX21, HARD_ACTUAL21,	");
            sb.Append(" HARD_STD22, HARD_MIN22, HARD_MAX22, HARD_ACTUAL22, HARD_STD23,	");
            sb.Append(" HARD_MIN23, HARD_MAX23, HARD_ACTUAL23, HARD_STD24, HARD_MIN24,	");
            sb.Append(" HARD_MAX24, HARD_ACTUAL24, HARD_STD25, HARD_MIN25, HARD_MAX25,	");
            sb.Append(" HARD_ACTUAL25, DUMMY, RUN_CD, STATUS, STATUS_MARK, MESSAGE,	") ;
            sb.Append(" TO_CHAR(CREATED_DATE,'yyyy/mm/dd hh24:mi:ss') AS CREATED_DATE ");
            sb.Append(" FROM  ");
            sb.Append(" MAKER_MILL_SHEET_LOG ");
            sb.Append(" WHERE ");
            sb.Append(" RUN_CD = " + runID);
            sb.Append(" AND STATUS_MARK <> 'S'");
            sb.Append(" ORDER BY MAKER_MILL_SHEET_LOG_CD ");

            return ExecuteQueryMultiResult<MakerMillSheetEDIDetails>(sb.ToString());

        }

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetErrorRptData(int runID, DateTime printReportDate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" MAKER_MILL_SHEET_LOG_CD, FILENAME, MAKER_NAME, MATL_NAME_MAKER, CHARGE, ");
            sb.Append("  DIM_D AS DIM_D ,");
            sb.Append(" LENGTH, NO_OF_UNITS, QUANTITY, HARDNESS_STD, HARDNESS_MIN,	");
            sb.Append("(CASE   WHEN GRAIN_SIZE_STD IS NULL   THEN NULL WHEN  HSKUSER.IS_NUMBER(GRAIN_SIZE_STD) = 1 THEN   TO_CHAR (GRAIN_SIZE_STD, 90.0)  ELSE   GRAIN_SIZE_STD   END  ) AS GRAIN_SIZE_STD , ");
            sb.Append(" (CASE   WHEN GRAIN_SIZE_ACTUAL IS NULL   THEN NULL WHEN  HSKUSER.IS_NUMBER(GRAIN_SIZE_ACTUAL) = 1 THEN   TO_CHAR (GRAIN_SIZE_ACTUAL, 90.0)  ELSE   GRAIN_SIZE_ACTUAL   END  ) AS GRAIN_SIZE_ACTUAL, ");
            sb.Append(" HARDNESS_MAX, HARDNESS_ACTUAL, CCNAME1, 	");
            sb.Append(" (case when CCRANGE1 is null then null else '×'||CCRANGE1 end ) as CCRANGE1 , CCMIN1, CCMAX1,	");
            sb.Append("	      CCACTUAL1 	");
            sb.Append(" , CCNAME2, 	");
            sb.Append("  (case when CCRANGE2 is null then null else '×'||CCRANGE2 end ) as CCRANGE2, 	");
            sb.Append(" CCMIN2, CCMAX2, CCACTUAL2, CCNAME3,	");
            sb.Append("  (case when CCRANGE4 is null then null else '×'||CCRANGE4 end ) as CCRANGE4,	");
            sb.Append("   CCMIN4, CCMAX4,	");
            sb.Append(" CCACTUAL4, CCNAME5, 	");
            sb.Append(" (case when CCRANGE5 is null then null else '×'||CCRANGE5 end ) as CCRANGE5 ,	");
            sb.Append("  CCMIN5, CCMAX5, CCACTUAL5, CCNAME6,	");
            sb.Append(" (case when CCRANGE6 is null then null else '×'||CCRANGE6 end ) as CCRANGE6 , 	");
            sb.Append(" CCMIN6, CCMAX6, CCACTUAL6, CCNAME7, 	");
            sb.Append("  (case when CCRANGE7 is null then null else '×'||CCRANGE7 end ) as CCRANGE7,	");
            sb.Append("  CCMIN7, CCMAX7,       CCACTUAL7, CCNAME8 ,	");
            sb.Append(" (case when CCRANGE8 is null then null else '×'||CCRANGE8 end ) as CCRANGE8,       	");
            sb.Append("  CCMIN8, CCMAX8, CCACTUAL8, CCNAME9,	");
            sb.Append(" (case when CCRANGE9 is null then null else '×'||CCRANGE9 end ) as CCRANGE9,	");
            sb.Append(" CCMIN9, CCMAX9, CCACTUAL9, CCNAME10,  CCMIN10,	");
            sb.Append(" (case when CCRANGE10 is null then null else '×'||CCRANGE10 end ) as CCRANGE10,	");
            sb.Append(" CCMAX10, CCACTUAL10, CCNAME11, 	");
            sb.Append("	      (case when CCRANGE11 is null then null else '×'||CCRANGE11 end ) as CCRANGE11,	");
            sb.Append(" CCMIN11, CCMAX11, CCACTUAL11,       CCNAME12, 	");
            sb.Append(" (case when CCRANGE12 is null then null else '×'||CCRANGE12 end ) as CCRANGE12, 	");
            sb.Append(" CCMIN12, CCMAX12, CCACTUAL12, CCNAME13, 	");
            sb.Append(" (case when CCRANGE13 is null then null else '×'||CCRANGE13 end ) as CCRANGE13,	");
            sb.Append(" CCMIN13, CCMAX13, CCACTUAL13, CCNAME14, 	");
            sb.Append(" (case when CCRANGE14 is null then null else '×'||CCRANGE14 end ) as CCRANGE14,	");
            sb.Append(" CCMIN14, CCMAX14,	");
            sb.Append(" CCACTUAL14, CCNAME15, 	");
            sb.Append(" (case when CCRANGE15 is null then null else '×'||CCRANGE15 end ) as CCRANGE15,	");
            sb .Append("  CCMIN15, CCMAX15, CCACTUAL15,	");
            sb.Append(" CCNAME16, 	");
            sb.Append(" (case when CCRANGE16 is null then null else '×'||CCRANGE16 end ) as CCRANGE16,	");
            sb.Append("  CCMIN16, CCMAX16, CCACTUAL16, CCNAME17, 	");
            sb.Append("	      (case when CCRANGE17 is null then null else '×'||CCRANGE17 end ) as CCRANGE17,	");
            sb.Append(" CCMIN17, CCMAX17, CCACTUAL17, CCNAME18, 	");
            sb.Append("	      (case when CCRANGE18 is null then null else '×'||CCRANGE18 end ) as CCRANGE18,	");
            sb.Append(" CCMIN18, CCMAX18,	");
            sb.Append(" CCACTUAL18, CCNAME19, 	");
            sb.Append(" (case when CCRANGE19 is null then null else '×'||CCRANGE19 end ) as CCRANGE19,	");
            sb.Append(" CCMIN19, CCMAX19, CCACTUAL19,	");
            sb.Append(" CCNAME20,	");
            sb.Append("  (case when CCRANGE20 is null then null else '×'||CCRANGE20 end ) as CCRANGE20,	");
            sb.Append("   CCMIN20, CCMAX20, CCACTUAL20, CCNAME21, 	");
            sb.Append("   (case when CCRANGE21 is null then null else '×'||CCRANGE21 end ) as CCRANGE21,	");
            sb.Append("   (case when CCRANGE22 is null then null else '×'||CCRANGE22 end ) as CCRANGE22,	");
            sb.Append(" CCMIN21, CCMAX21, CCACTUAL21, CCNAME22, CCMIN22, CCMAX22,	");
            sb.Append(" CCACTUAL22, CCNAME23, 	");
            sb.Append("	      (case when CCRANGE23 is null then null else '×'||CCRANGE23 end ) as CCRANGE23,	");
            sb.Append(" CCMIN23, CCMAX23, CCACTUAL23,	");
            sb.Append(" CCNAME24, 	");
            sb.Append(" (case when CCRANGE24 is null then null else '×'||CCRANGE24 end ) as CCRANGE24,	");
            sb.Append(" CCMIN24, CCMAX24, CCACTUAL24, CCNAME25, 	");
            sb.Append(" (case when CCRANGE25 is null then null else '×'||CCRANGE25 end ) as CCRANGE25,	");
            sb.Append(" CCMIN25, CCMAX25, CCACTUAL25, HARD_STD1, HARD_MIN1, HARD_MAX1,	");
            sb.Append(" HARD_ACTUAL1, HARD_STD2, HARD_MIN2, HARD_MAX2, HARD_ACTUAL2, HARD_STD3,	");
            sb.Append(" HARD_MIN3, HARD_MAX3, HARD_ACTUAL3, HARD_STD4, HARD_MIN4, HARD_MAX4,	");
            sb.Append(" HARD_ACTUAL4, HARD_STD5, HARD_MIN5, HARD_MAX5, HARD_ACTUAL5, HARD_STD6,	");
            sb.Append(" HARD_MIN6, HARD_MAX6, HARD_ACTUAL6, HARD_STD7, HARD_MIN7, HARD_MAX7,	");
            sb.Append(" HARD_ACTUAL7, HARD_STD8, HARD_MIN8, HARD_MAX8, HARD_ACTUAL8, HARD_STD9,	");
            sb.Append(" HARD_MIN9, HARD_MAX9, HARD_ACTUAL9, HARD_STD10, HARD_MIN10, HARD_MAX10,	");
            sb.Append(" HARD_ACTUAL10, HARD_STD11, HARD_MIN11, HARD_MAX11, HARD_ACTUAL11,	");
            sb.Append(" HARD_STD12, HARD_MIN12, HARD_MAX12, HARD_ACTUAL12, HARD_STD13,	");
            sb.Append(" HARD_MIN13, HARD_MAX13, HARD_ACTUAL13, HARD_STD14, HARD_MIN14,	");
            sb.Append(" HARD_MAX14, HARD_ACTUAL14, HARD_STD15, HARD_MIN15, HARD_MAX15,	");
            sb.Append(" HARD_ACTUAL15, HARD_STD16, HARD_MIN16, HARD_MAX16, HARD_ACTUAL16,	");
            sb.Append(" HARD_STD17, HARD_MIN17, HARD_MAX17, HARD_ACTUAL17, HARD_STD18,	");
            sb.Append(" HARD_MIN18, HARD_MAX18, HARD_ACTUAL18, HARD_STD19, HARD_MIN19,	");
            sb.Append(" HARD_MAX19, HARD_ACTUAL19, HARD_STD20, HARD_MIN20, HARD_MAX20,	");
            sb.Append(" HARD_ACTUAL20, HARD_STD21, HARD_MIN21, HARD_MAX21, HARD_ACTUAL21,	");
            sb.Append(" HARD_STD22, HARD_MIN22, HARD_MAX22, HARD_ACTUAL22, HARD_STD23,	");
            sb.Append(" HARD_MIN23, HARD_MAX23, HARD_ACTUAL23, HARD_STD24, HARD_MIN24,	");
            sb.Append(" HARD_MAX24, HARD_ACTUAL24, HARD_STD25, HARD_MIN25, HARD_MAX25,	");
            sb.Append(" HARD_ACTUAL25, DUMMY, RUN_CD, STATUS, 'エラー' AS STATUS_MARK, MESSAGE,	");
            sb.Append(" TO_CHAR(CREATED_DATE,'yyyy/mm/dd hh24:mi:ss') AS CREATED_DATE ");
            sb.Append(" FROM  ");
            sb.Append(" MAKER_MILL_SHEET_LOG ");
            sb.Append(" WHERE ");
            sb.Append(" RUN_CD = " + runID);
            sb.Append(" AND STATUS = 1 ");
            sb.Append(" AND STATUS_MARK <> 'S'");
            sb.Append(" ORDER BY MAKER_MILL_SHEET_LOG_CD ");

            return ExecuteQueryMultiResult<MakerMillSheetEDIDetails>(sb.ToString());

        }

        public int GetMakerMillSheetBatchRunID()
        {
            //string sql = "SELECT MAKER_MILL_SHEET_RUNID_SEQ.NEXTVAL FROM DUAL";

            //int batchID = Convert.ToInt32(ExecuteScalar(sql));

            makerMillSheetRunID_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Maker_Mill_Sheet_RunID").Value);
            
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            if (string.IsNullOrEmpty(makerMillSheetRunID_Seq))
            {
                selectQuery.Append(" MAKER_MILL_SHEET_RUNID_SEQ.NEXTVAL ");
            }
            else
            {
                selectQuery.Append(makerMillSheetRunID_Seq);
            }


            selectQuery.Append(" FROM ");
            selectQuery.Append(" DUAL ");
            int batchID = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return batchID;
        }


        public Dictionary<string, ColumnMeta> GetColumnMetadata()
        {
            string sql = @"SELECT 
                   COLUMN_NAME,
                   DATA_TYPE,
                   DATA_LENGTH,
                   DATA_PRECISION,
                   DATA_SCALE
                   FROM USER_TAB_COLUMNS
                   WHERE TABLE_NAME = 'MAKER_MILL_SHEET'
                   ORDER BY COLUMN_ID";

            var columnMeta = new Dictionary<string, ColumnMeta>();

            using (var conn = Connection())
            {
                conn.Open();

                using (var cmd = new OracleCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var meta = new ColumnMeta
                        {
                            ColumnName = reader["COLUMN_NAME"].ToString(),
                            DataType = reader["DATA_TYPE"].ToString(),
                            //Length = reader["DATA_LENGTH"] as int?,
                            //Precision = reader["DATA_PRECISION"] as int?,
                            //Scale = reader["DATA_SCALE"] as int ?

                            Length = reader["DATA_LENGTH"] != DBNull.Value
                            ? Convert.ToInt32(reader["DATA_LENGTH"])
                            : (int?)null,

                            Precision = reader["DATA_PRECISION"] != DBNull.Value
                            ? Convert.ToInt32(reader["DATA_PRECISION"])
                            : (int?)null,

                            Scale = reader["DATA_SCALE"] != DBNull.Value
                            ? Convert.ToInt32(reader["DATA_SCALE"])
                            : (int?)null,
                        };

                        columnMeta[meta.ColumnName] = meta;
                    }
                }
            }

            return columnMeta;
        }

        public int GetmakerCD(string maker_Name)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT MAKER_CD ");
            sb.Append("FROM MST_MAKER ");
            sb.Append("WHERE MAKER_NAME = '");
            sb.Append(maker_Name.Replace("'", "''"));
            sb.Append("'");

            var result = ExecuteScalar(sb.ToString());

            if (result == null || result == DBNull.Value)
                return 0; // default value

            return Convert.ToInt32(result);
        }
    }
}
