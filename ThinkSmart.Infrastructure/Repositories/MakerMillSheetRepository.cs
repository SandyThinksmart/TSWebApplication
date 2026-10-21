using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using static TSWebApplication.Domain.Utility.Enums;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class MakerMillSheetRepository : BaseRepository, IMakerMillSheetRepository
    {
        private readonly string _connectionString;
        private readonly LocalizationServices _localization;
        string result;
        private string makerMillSheet_Seq;
        private string makerMillSheetLog_Seq;
        private string makerMillSheetRunID_Seq;
        private readonly IConfiguration _configuration;
        public MakerMillSheetRepository(IConfiguration configuration, LocalizationServices localization) : base(configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDBConnection");
            _localization = localization;
            _configuration = configuration;
        }
        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/07
        /// used for Registration Data MAKER_MILL_SHEET Table 
        /// </summary>
        public void InsertMakerMillSheetDetails(MakerMillSheetDetails makermillsheetDetails)
        {
            makerMillSheet_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Maker_Mill_Sheet").Value);
           
            MakerMillSheetDetails result = new MakerMillSheetDetails();
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
            insertQuery.Append(" ,IS_EDI_DATA ");
            insertQuery.Append(" ,RUN_CD ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            if (string.IsNullOrEmpty(makerMillSheet_Seq))
            {
                insertQuery.Append(" MAKER_MILL_SHEET_SEQ.NEXTVAL ");
            }
            else
            {
                insertQuery.Append(makerMillSheet_Seq);
            }
            
            insertQuery.Append(",'" + makermillsheetDetails.MAKER_CD + "'");
            insertQuery.Append(",'" + makermillsheetDetails.MAKER_NAME + "'");
            insertQuery.Append(",'" + makermillsheetDetails.PDF_FILE_PATH + "'");
            insertQuery.Append(",'" + makermillsheetDetails.FILENAME + "'");
            // insertQuery.Append(",'" + makermillsheetDetails.PDF_PAGE_NUMBER + "'");
            insertQuery.Append(",'" + makermillsheetDetails.KOUBAN + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CHARGE + "'");
            insertQuery.Append(",'" + makermillsheetDetails.MATL_NAME_MAKER + "'");
            insertQuery.Append(",'" + makermillsheetDetails.DIM_D + "'");
            insertQuery.Append(",'" + makermillsheetDetails.LENGTH + "'");
            insertQuery.Append(",'" + makermillsheetDetails.NO_OF_UNITS + "'");
            insertQuery.Append(",'" + makermillsheetDetails.UNIT + "'");
            insertQuery.Append(",'" + makermillsheetDetails.QUANTITY + "'");
            insertQuery.Append(",'" + makermillsheetDetails.GRAIN_SIZE_STD + "'");
            insertQuery.Append(",'" + makermillsheetDetails.GRAIN_SIZE_ACTUAL + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARDNESS_STD + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARDNESS_MIN + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARDNESS_MAX + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARDNESS_ACTUAL + "'");
            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCNAME25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCRANGE25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMIN25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCMAX25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.CCACTUAL25 + "'");
            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_STD25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MIN25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_MAX25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL1 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL2 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL3 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL4 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL5 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL6 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL7 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL8 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL9 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL10 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL11 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL12 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL13 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL14 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL15 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL16 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL17 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL18 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL19 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL20 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL21 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL22 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL23 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL24 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.HARD_ACTUAL25 + "'");
            insertQuery.Append(",'" + makermillsheetDetails.INPUT_LOCATION_CD + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(",SYSDATE ");
            insertQuery.Append(",'" + makermillsheetDetails.STATUS + "'");
            insertQuery.Append(",'" + makermillsheetDetails.STATUS_MARK + "'");
            insertQuery.Append(",'0'"); //IS_EDI_DATA 0 as per window code
            insertQuery.Append(",'" + makermillsheetDetails.RUN_CD + "'");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());

        }
        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/07
        /// used for Update Data HSKMILLSHEET Table 
        /// </summary>
        public List<MakerMillSheetDetails> GetMakerMillSheetList(List<SearchField> searchFields, string sortCiteria, int LocationCD)
        {
            string _maxRowLimit = Constants.DefaultValues.MAX_ROW_LIMIT;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append("  MMS.MAKER_MILL_SHEET_CD ");
            selectQuery.Append(" ,MM.MAKER_NAME AS MAKER_NAME ");
            selectQuery.Append(" ,MMS.MATL_NAME_MAKER ");
            selectQuery.Append(" ,MMS.DIM_D ");
            selectQuery.Append(" ,MMS.LENGTH ");

            selectQuery.Append(" ,MMS.CHARGE ");

            selectQuery.Append(" ,MMS.KOUBAN ");
            selectQuery.Append(" ,MMS.NO_OF_UNITS ");
            selectQuery.Append(" ,MMS.QUANTITY ");
            selectQuery.Append(" ,MMS.CREATED_DATE ");
            selectQuery.Append(" ,MMS.UPDATED_DATE ");
            selectQuery.Append(" ,TO_CHAR(MMS.CREATED_DATE,'yyyy/mm/dd HH24:MI' ) AS CREATED_DATE_STRING");
            selectQuery.Append(" ,TO_CHAR(MMS.UPDATED_DATE,'yyyy/mm/dd HH24:MI') AS UPDATED_DATE_STRING");
            selectQuery.Append(" ,MMS.FILENAME");

            selectQuery.Append("  FROM MAKER_MILL_SHEET MMS ");
            selectQuery.Append(" INNER JOIN MST_MAKER MM ON");
            selectQuery.Append(" MMS.MAKER_CD = MM.MAKER_CD");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MMS.INPUT_LOCATION_CD = '" + LocationCD + "'");
            var query = selectQuery.ToString();
            if (searchFields != null && searchFields.Count > 0)
            {
                string whereCondition = "";
                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
                {
                    andOperator = " AND ";
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);

                }
                query = query + whereCondition;
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += "ORDER BY MMS.CREATED_DATE DESC, MMS.MATL_NAME_MAKER ASC, MMS.DIM_D ASC, MMS.MAKER_NAME ASC, MMS.LENGTH ASC, MMS.CHARGE ASC, MMS.KOUBAN ASC";
            }
            return ExecuteQueryMultiResult<MakerMillSheetDetails>(query);
        }

        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/07
        /// used for Update and Deleted  Data MAKER_MILL_SHEET Table with selected MAKER_MILL_SHEET_CD
        /// </summary>
        public MakerMillSheetDetails GetMillSheetDetailsByCD(long millsheetCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            // selectQuery.Append("  MMS.MAKER_MILL_SHEET_CD ");
            //  selectQuery.Append(" ,MMS.MAKER_CD ");
            selectQuery.Append(" MM.SHORT_NAME AS MAKER_NAME ");
            selectQuery.Append(" ,MMS.* ");
            //selectQuery.Append(" ,MMS.DIM_D ");
            //selectQuery.Append(" ,MMS.LENGTH ");

            //selectQuery.Append(" ,MMS.CHARGE ");

            //selectQuery.Append(" ,MMS.KOUBAN ");
            //selectQuery.Append(" ,MMS.NO_OF_UNITS ");
            //selectQuery.Append(" ,MMS.QUANTITY ");

            //selectQuery.Append(" ,TO_CHAR(MMS.CREATED_DATE,'yyyy/mm/dd HH24:MI') AS CREATED_DATE");
            //selectQuery.Append(" ,TO_CHAR(MMS.UPDATED_DATE,'yyyy/mm/dd HH24:MI') AS UPDATED_DATE");
            //selectQuery.Append(" ,MMS.FILENAME");

            selectQuery.Append("  FROM MAKER_MILL_SHEET MMS ");
            selectQuery.Append(" INNER JOIN MST_MAKER MM ON");
            selectQuery.Append(" MMS.MAKER_CD = MM.MAKER_CD");


            selectQuery.Append("  WHERE MMS.MAKER_MILL_SHEET_CD= " + millsheetCD);

            return ExecuteQuerySingleResult<MakerMillSheetDetails>(selectQuery.ToString());
        }


        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/15
        /// used for Update Data MAKER_MILL_SHEET Table Selected Row 
        /// </summary>
        public void UpdateMakerMillSheetDetails(MakerMillSheetDetails details)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("  UPDATE MAKER_MILL_SHEET SET ");
            updateQuery.Append(" MAKER_CD =" + "'" + details.MAKER_CD + "'");
            updateQuery.Append(" ,MAKER_NAME =" + "'" + details.MAKER_NAME + "'");
          //updateQuery.Append(" ,PDF_FILE_PATH =" + "'" + details.PDF_FILE_PATH + "'");
          //updateQuery.Append(" ,FILENAME =" + "'" + details.FILENAME + "'");
            updateQuery.Append(" ,KOUBAN =" + " '" + details.KOUBAN + "'");
            updateQuery.Append(" ,CHARGE =" + "'" + details.CHARGE + "'");
            updateQuery.Append(" ,MATL_NAME_MAKER =" + "'" + details.MATL_NAME_MAKER + "'");
            updateQuery.Append(" ,DIM_D =" + "'" + details.DIM_D + "'");
            updateQuery.Append(" ,LENGTH =" + "'" + details.LENGTH + "'");
            updateQuery.Append(" ,NO_OF_UNITS =" + "'" + details.NO_OF_UNITS + "'");
            updateQuery.Append(" ,UNIT =" + "'" + details.UNIT + "'");
            updateQuery.Append(" ,QUANTITY =" + "'" + details.QUANTITY + "'");
            updateQuery.Append(" ,GRAIN_SIZE_STD =" + "'" + details.GRAIN_SIZE_STD + "'");
            updateQuery.Append(" ,GRAIN_SIZE_ACTUAL =" + "'" + details.GRAIN_SIZE_ACTUAL + "'");
            updateQuery.Append(" ,HARDNESS_STD =" + "'" + details.HARDNESS_STD + "'");
            updateQuery.Append(" ,HARDNESS_MIN =" + "'" + details.HARDNESS_MIN + "'");
            updateQuery.Append(" ,HARDNESS_MAX =" + "'" + details.HARDNESS_MAX + "'");
            updateQuery.Append(" ,HARDNESS_ACTUAL =" + "'" + details.HARDNESS_ACTUAL + "'");

            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
            updateQuery.Append(",CCNAME1 =" + "'" + details.CCNAME1 + "'");
            updateQuery.Append(",CCNAME2 =" + "'" + details.CCNAME2 + "'");
            updateQuery.Append(",CCNAME3 =" + "'" + details.CCNAME3 + "'");
            updateQuery.Append(",CCNAME4 =" + "'" + details.CCNAME4 + "'");
            updateQuery.Append(",CCNAME5 =" + "'" + details.CCNAME5 + "'");
            updateQuery.Append(",CCNAME6 =" + "'" + details.CCNAME6 + "'");
            updateQuery.Append(",CCNAME7 =" + "'" + details.CCNAME7 + "'");
            updateQuery.Append(",CCNAME8 =" + "'" + details.CCNAME8 + "'");
            updateQuery.Append(",CCNAME9 =" + "'" + details.CCNAME9 + "'");
            updateQuery.Append(",CCNAME10 =" + "'" + details.CCNAME10 + "'");
            updateQuery.Append(",CCNAME11 =" + "'" + details.CCNAME11 + "'");
            updateQuery.Append(",CCNAME12 =" + "'" + details.CCNAME12 + "'");
            updateQuery.Append(",CCNAME13 =" + "'" + details.CCNAME13 + "'");
            updateQuery.Append(",CCNAME14 =" + "'" + details.CCNAME14 + "'");
            updateQuery.Append(",CCNAME15 =" + "'" + details.CCNAME15 + "'");
            updateQuery.Append(",CCNAME16 =" + "'" + details.CCNAME16 + "'");
            updateQuery.Append(",CCNAME17 =" + "'" + details.CCNAME17 + "'");
            updateQuery.Append(",CCNAME18 =" + "'" + details.CCNAME18 + "'");
            updateQuery.Append(",CCNAME19 =" + "'" + details.CCNAME19 + "'");
            updateQuery.Append(",CCNAME20 =" + "'" + details.CCNAME20 + "'");
            updateQuery.Append(",CCNAME21 =" + "'" + details.CCNAME21 + "'");
            updateQuery.Append(",CCNAME22 =" + "'" + details.CCNAME22 + "'");
            updateQuery.Append(",CCNAME23 =" + "'" + details.CCNAME23 + "'");
            updateQuery.Append(",CCNAME24 =" + "'" + details.CCNAME24 + "'");
            updateQuery.Append(",CCNAME25 =" + "'" + details.CCNAME25 + "'");
            updateQuery.Append(",CCRANGE1 =" + "'" + details.CCRANGE1 + "'");
            updateQuery.Append(",CCRANGE2 =" + "'" + details.CCRANGE2 + "'");
            updateQuery.Append(",CCRANGE3 =" + "'" + details.CCRANGE3 + "'");
            updateQuery.Append(",CCRANGE4 =" + "'" + details.CCRANGE4 + "'");
            updateQuery.Append(",CCRANGE5 =" + "'" + details.CCRANGE5 + "'");
            updateQuery.Append(",CCRANGE6 =" + "'" + details.CCRANGE6 + "'");
            updateQuery.Append(",CCRANGE7 =" + "'" + details.CCRANGE7 + "'");
            updateQuery.Append(",CCRANGE8 =" + "'" + details.CCRANGE8 + "'");
            updateQuery.Append(",CCRANGE9 =" + "'" + details.CCRANGE9 + "'");
            updateQuery.Append(",CCRANGE10 =" + "'" + details.CCRANGE10 + "'");
            updateQuery.Append(",CCRANGE11 =" + "'" + details.CCRANGE11 + "'");
            updateQuery.Append(",CCRANGE12 =" + "'" + details.CCRANGE12 + "'");
            updateQuery.Append(",CCRANGE13 =" + "'" + details.CCRANGE13 + "'");
            updateQuery.Append(",CCRANGE14 =" + "'" + details.CCRANGE14 + "'");
            updateQuery.Append(",CCRANGE15 =" + "'" + details.CCRANGE15 + "'");
            updateQuery.Append(",CCRANGE16 =" + "'" + details.CCRANGE16 + "'");
            updateQuery.Append(",CCRANGE17 =" + "'" + details.CCRANGE17 + "'");
            updateQuery.Append(",CCRANGE18 =" + "'" + details.CCRANGE18 + "'");
            updateQuery.Append(",CCRANGE19 =" + "'" + details.CCRANGE19 + "'");
            updateQuery.Append(",CCRANGE20 =" + "'" + details.CCRANGE20 + "'");
            updateQuery.Append(",CCRANGE21 =" + "'" + details.CCRANGE21 + "'");
            updateQuery.Append(",CCRANGE22 =" + "'" + details.CCRANGE22 + "'");
            updateQuery.Append(",CCRANGE23 =" + "'" + details.CCRANGE23 + "'");
            updateQuery.Append(",CCRANGE24 =" + "'" + details.CCRANGE24 + "'");
            updateQuery.Append(",CCRANGE25 =" + "'" + details.CCRANGE25 + "'");
            updateQuery.Append(",CCMIN1 =" + "'" + details.CCMIN1 + "'");
            updateQuery.Append(",CCMIN2 =" + "'" + details.CCMIN2 + "'");
            updateQuery.Append(",CCMIN3 =" + "'" + details.CCMIN3 + "'");
            updateQuery.Append(",CCMIN4 =" + "'" + details.CCMIN4 + "'");
            updateQuery.Append(",CCMIN5 =" + "'" + details.CCMIN5 + "'");
            updateQuery.Append(",CCMIN6 =" + "'" + details.CCMIN6 + "'");
            updateQuery.Append(",CCMIN7 =" + "'" + details.CCMIN7 + "'");
            updateQuery.Append(",CCMIN8 =" + "'" + details.CCMIN8 + "'");
            updateQuery.Append(",CCMIN9 =" + "'" + details.CCMIN9 + "'");
            updateQuery.Append(",CCMIN10 =" + "'" + details.CCMIN10 + "'");
            updateQuery.Append(",CCMIN11 =" + "'" + details.CCMIN11 + "'");
            updateQuery.Append(",CCMIN12 =" + "'" + details.CCMIN12 + "'");
            updateQuery.Append(",CCMIN13 =" + "'" + details.CCMIN13 + "'");
            updateQuery.Append(",CCMIN14 =" + "'" + details.CCMIN14 + "'");
            updateQuery.Append(",CCMIN15 =" + "'" + details.CCMIN15 + "'");
            updateQuery.Append(",CCMIN16 =" + "'" + details.CCMIN16 + "'");
            updateQuery.Append(",CCMIN17 =" + "'" + details.CCMIN17 + "'");
            updateQuery.Append(",CCMIN18 =" + "'" + details.CCMIN18 + "'");
            updateQuery.Append(",CCMIN19 =" + "'" + details.CCMIN19 + "'");
            updateQuery.Append(",CCMIN20 =" + "'" + details.CCMIN20 + "'");
            updateQuery.Append(",CCMIN21 =" + "'" + details.CCMIN21 + "'");
            updateQuery.Append(",CCMIN22 =" + "'" + details.CCMIN22 + "'");
            updateQuery.Append(",CCMIN23 =" + "'" + details.CCMIN23 + "'");
            updateQuery.Append(",CCMIN24 =" + "'" + details.CCMIN24 + "'");
            updateQuery.Append(",CCMIN25 =" + "'" + details.CCMIN25 + "'");
            updateQuery.Append(",CCMAX1 =" + "'" + details.CCMAX1 + "'");
            updateQuery.Append(",CCMAX2 =" + "'" + details.CCMAX2 + "'");
            updateQuery.Append(",CCMAX3 =" + "'" + details.CCMAX3 + "'");
            updateQuery.Append(",CCMAX4 =" + "'" + details.CCMAX4 + "'");
            updateQuery.Append(",CCMAX5 =" + "'" + details.CCMAX5 + "'");
            updateQuery.Append(",CCMAX6 =" + "'" + details.CCMAX6 + "'");
            updateQuery.Append(",CCMAX7 =" + "'" + details.CCMAX7 + "'");
            updateQuery.Append(",CCMAX8 =" + "'" + details.CCMAX8 + "'");
            updateQuery.Append(",CCMAX9 =" + "'" + details.CCMAX9 + "'");
            updateQuery.Append(",CCMAX10 =" + "'" + details.CCMAX10 + "'");
            updateQuery.Append(",CCMAX11 =" + "'" + details.CCMAX11 + "'");
            updateQuery.Append(",CCMAX12 =" + "'" + details.CCMAX12 + "'");
            updateQuery.Append(",CCMAX13 =" + "'" + details.CCMAX13 + "'");
            updateQuery.Append(",CCMAX14 =" + "'" + details.CCMAX14 + "'");
            updateQuery.Append(",CCMAX15 =" + "'" + details.CCMAX15 + "'");
            updateQuery.Append(",CCMAX16 =" + "'" + details.CCMAX16 + "'");
            updateQuery.Append(",CCMAX17 =" + "'" + details.CCMAX17 + "'");
            updateQuery.Append(",CCMAX18 =" + "'" + details.CCMAX18 + "'");
            updateQuery.Append(",CCMAX19 =" + "'" + details.CCMAX19 + "'");
            updateQuery.Append(",CCMAX20 =" + "'" + details.CCMAX20 + "'");
            updateQuery.Append(",CCMAX21 =" + "'" + details.CCMAX21 + "'");
            updateQuery.Append(",CCMAX22 =" + "'" + details.CCMAX22 + "'");
            updateQuery.Append(",CCMAX23 =" + "'" + details.CCMAX23 + "'");
            updateQuery.Append(",CCMAX24 =" + "'" + details.CCMAX24 + "'");
            updateQuery.Append(",CCMAX25 =" + "'" + details.CCMAX25 + "'");
            updateQuery.Append(",CCACTUAL1 =" + "'" + details.CCACTUAL1 + "'");
            updateQuery.Append(",CCACTUAL2 =" + "'" + details.CCACTUAL2 + "'");
            updateQuery.Append(",CCACTUAL3 =" + "'" + details.CCACTUAL3 + "'");
            updateQuery.Append(",CCACTUAL4 =" + "'" + details.CCACTUAL4 + "'");
            updateQuery.Append(",CCACTUAL5 =" + "'" + details.CCACTUAL5 + "'");
            updateQuery.Append(",CCACTUAL6 =" + "'" + details.CCACTUAL6 + "'");
            updateQuery.Append(",CCACTUAL7 =" + "'" + details.CCACTUAL7 + "'");
            updateQuery.Append(",CCACTUAL8 =" + "'" + details.CCACTUAL8 + "'");
            updateQuery.Append(",CCACTUAL9 =" + "'" + details.CCACTUAL9 + "'");
            updateQuery.Append(",CCACTUAL10 =" + "'" + details.CCACTUAL10 + "'");
            updateQuery.Append(",CCACTUAL11 =" + "'" + details.CCACTUAL11 + "'");
            updateQuery.Append(",CCACTUAL12 =" + "'" + details.CCACTUAL12 + "'");
            updateQuery.Append(",CCACTUAL13 =" + "'" + details.CCACTUAL13 + "'");
            updateQuery.Append(",CCACTUAL14 =" + "'" + details.CCACTUAL14 + "'");
            updateQuery.Append(",CCACTUAL15 =" + "'" + details.CCACTUAL15 + "'");
            updateQuery.Append(",CCACTUAL16 =" + "'" + details.CCACTUAL16 + "'");
            updateQuery.Append(",CCACTUAL17 =" + "'" + details.CCACTUAL17 + "'");
            updateQuery.Append(",CCACTUAL18 =" + "'" + details.CCACTUAL18 + "'");
            updateQuery.Append(",CCACTUAL19 =" + "'" + details.CCACTUAL19 + "'");
            updateQuery.Append(",CCACTUAL20 =" + "'" + details.CCACTUAL20 + "'");
            updateQuery.Append(",CCACTUAL21 =" + "'" + details.CCACTUAL21 + "'");
            updateQuery.Append(",CCACTUAL22 =" + "'" + details.CCACTUAL22 + "'");
            updateQuery.Append(",CCACTUAL23 =" + "'" + details.CCACTUAL23 + "'");
            updateQuery.Append(",CCACTUAL24 =" + "'" + details.CCACTUAL24 + "'");
            updateQuery.Append(",CCACTUAL25 =" + "'" + details.CCACTUAL25 + "'");

            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------
            updateQuery.Append(",HARD_STD1 =" + "'" + details.HARD_STD1 + "'");
            updateQuery.Append(",HARD_STD2 =" + "'" + details.HARD_STD2 + "'");
            updateQuery.Append(",HARD_STD3 =" + "'" + details.HARD_STD3 + "'");
            updateQuery.Append(",HARD_STD4 =" + "'" + details.HARD_STD4 + "'");
            updateQuery.Append(",HARD_STD5 =" + "'" + details.HARD_STD5 + "'");
            updateQuery.Append(",HARD_STD6 =" + "'" + details.HARD_STD6 + "'");
            updateQuery.Append(",HARD_STD7 =" + "'" + details.HARD_STD7 + "'");
            updateQuery.Append(",HARD_STD8 =" + "'" + details.HARD_STD8 + "'");
            updateQuery.Append(",HARD_STD9 =" + "'" + details.HARD_STD9 + "'");
            updateQuery.Append(",HARD_STD10 =" + "'" + details.HARD_STD10 + "'");
            updateQuery.Append(",HARD_STD11 =" + "'" + details.HARD_STD11 + "'");
            updateQuery.Append(",HARD_STD12 =" + "'" + details.HARD_STD12 + "'");
            updateQuery.Append(",HARD_STD13 =" + "'" + details.HARD_STD13 + "'");
            updateQuery.Append(",HARD_STD14 =" + "'" + details.HARD_STD14 + "'");
            updateQuery.Append(",HARD_STD15 =" + "'" + details.HARD_STD15 + "'");
            updateQuery.Append(",HARD_STD16 =" + "'" + details.HARD_STD16 + "'");
            updateQuery.Append(",HARD_STD17 =" + "'" + details.HARD_STD17 + "'");
            updateQuery.Append(",HARD_STD18 =" + "'" + details.HARD_STD18 + "'");
            updateQuery.Append(",HARD_STD19 =" + "'" + details.HARD_STD19 + "'");
            updateQuery.Append(",HARD_STD20 =" + "'" + details.HARD_STD20 + "'");
            updateQuery.Append(",HARD_STD21 =" + "'" + details.HARD_STD21 + "'");
            updateQuery.Append(",HARD_STD22 =" + "'" + details.HARD_STD22 + "'");
            updateQuery.Append(",HARD_STD23 =" + "'" + details.HARD_STD23 + "'");
            updateQuery.Append(",HARD_STD24 =" + "'" + details.HARD_STD24 + "'");
            updateQuery.Append(",HARD_STD25 =" + "'" + details.HARD_STD25 + "'");
            updateQuery.Append(",HARD_MIN1 =" + "'" + details.HARD_MIN1 + "'");
            updateQuery.Append(",HARD_MIN2 =" + "'" + details.HARD_MIN2 + "'");
            updateQuery.Append(",HARD_MIN3 =" + "'" + details.HARD_MIN3 + "'");
            updateQuery.Append(",HARD_MIN4 =" + "'" + details.HARD_MIN4 + "'");
            updateQuery.Append(",HARD_MIN5 =" + "'" + details.HARD_MIN5 + "'");
            updateQuery.Append(",HARD_MIN6 =" + "'" + details.HARD_MIN6 + "'");
            updateQuery.Append(",HARD_MIN7 =" + "'" + details.HARD_MIN7 + "'");
            updateQuery.Append(",HARD_MIN8 =" + "'" + details.HARD_MIN8 + "'");
            updateQuery.Append(",HARD_MIN9 =" + "'" + details.HARD_MIN9 + "'");
            updateQuery.Append(",HARD_MIN10 =" + "'" + details.HARD_MIN10 + "'");
            updateQuery.Append(",HARD_MIN11 =" + "'" + details.HARD_MIN11 + "'");
            updateQuery.Append(",HARD_MIN12 =" + "'" + details.HARD_MIN12 + "'");
            updateQuery.Append(",HARD_MIN13 =" + "'" + details.HARD_MIN13 + "'");
            updateQuery.Append(",HARD_MIN14 =" + "'" + details.HARD_MIN14 + "'");
            updateQuery.Append(",HARD_MIN15 =" + "'" + details.HARD_MIN15 + "'");
            updateQuery.Append(",HARD_MIN16 =" + "'" + details.HARD_MIN16 + "'");
            updateQuery.Append(",HARD_MIN17 =" + "'" + details.HARD_MIN17 + "'");
            updateQuery.Append(",HARD_MIN18 =" + "'" + details.HARD_MIN18 + "'");
            updateQuery.Append(",HARD_MIN19 =" + "'" + details.HARD_MIN19 + "'");
            updateQuery.Append(",HARD_MIN20 =" + "'" + details.HARD_MIN20 + "'");
            updateQuery.Append(",HARD_MIN21 =" + "'" + details.HARD_MIN21 + "'");
            updateQuery.Append(",HARD_MIN22 =" + "'" + details.HARD_MIN22 + "'");
            updateQuery.Append(",HARD_MIN23 =" + "'" + details.HARD_MIN23 + "'");
            updateQuery.Append(",HARD_MIN24 =" + "'" + details.HARD_MIN24 + "'");
            updateQuery.Append(",HARD_MIN25 =" + "'" + details.HARD_MIN25 + "'");
            updateQuery.Append(",HARD_MAX1 =" + "'" + details.HARD_MAX1 + "'");
            updateQuery.Append(",HARD_MAX2 =" + "'" + details.HARD_MAX2 + "'");
            updateQuery.Append(",HARD_MAX3 =" + "'" + details.HARD_MAX3 + "'");
            updateQuery.Append(",HARD_MAX4 =" + "'" + details.HARD_MAX4 + "'");
            updateQuery.Append(",HARD_MAX5 =" + "'" + details.HARD_MAX5 + "'");
            updateQuery.Append(",HARD_MAX6 =" + "'" + details.HARD_MAX6 + "'");
            updateQuery.Append(",HARD_MAX7 =" + "'" + details.HARD_MAX7 + "'");
            updateQuery.Append(",HARD_MAX8 =" + "'" + details.HARD_MAX8 + "'");
            updateQuery.Append(",HARD_MAX9 =" + "'" + details.HARD_MAX9 + "'");
            updateQuery.Append(",HARD_MAX10 =" + "'" + details.HARD_MAX10 + "'");
            updateQuery.Append(",HARD_MAX11 =" + "'" + details.HARD_MAX11 + "'");
            updateQuery.Append(",HARD_MAX12 =" + "'" + details.HARD_MAX12 + "'");
            updateQuery.Append(",HARD_MAX13 =" + "'" + details.HARD_MAX13 + "'");
            updateQuery.Append(",HARD_MAX14 =" + "'" + details.HARD_MAX14 + "'");
            updateQuery.Append(",HARD_MAX15 =" + "'" + details.HARD_MAX15 + "'");
            updateQuery.Append(",HARD_MAX16 =" + "'" + details.HARD_MAX16 + "'");
            updateQuery.Append(",HARD_MAX17 =" + "'" + details.HARD_MAX17 + "'");
            updateQuery.Append(",HARD_MAX18 =" + "'" + details.HARD_MAX18 + "'");
            updateQuery.Append(",HARD_MAX19 =" + "'" + details.HARD_MAX19 + "'");
            updateQuery.Append(",HARD_MAX20 =" + "'" + details.HARD_MAX20 + "'");
            updateQuery.Append(",HARD_MAX21 =" + "'" + details.HARD_MAX21 + "'");
            updateQuery.Append(",HARD_MAX22 =" + "'" + details.HARD_MAX22 + "'");
            updateQuery.Append(",HARD_MAX23 =" + "'" + details.HARD_MAX23 + "'");
            updateQuery.Append(",HARD_MAX24 =" + "'" + details.HARD_MAX24 + "'");
            updateQuery.Append(",HARD_MAX25 =" + "'" + details.HARD_MAX25 + "'");
            updateQuery.Append(",HARD_ACTUAL1 =" + "'" + details.HARD_ACTUAL1 + "'");
            updateQuery.Append(",HARD_ACTUAL2 =" + "'" + details.HARD_ACTUAL2 + "'");
            updateQuery.Append(",HARD_ACTUAL3 =" + "'" + details.HARD_ACTUAL3 + "'");
            updateQuery.Append(",HARD_ACTUAL4 =" + "'" + details.HARD_ACTUAL4 + "'");
            updateQuery.Append(",HARD_ACTUAL5 =" + "'" + details.HARD_ACTUAL5 + "'");
            updateQuery.Append(",HARD_ACTUAL6 =" + "'" + details.HARD_ACTUAL6 + "'");
            updateQuery.Append(",HARD_ACTUAL7 =" + "'" + details.HARD_ACTUAL7 + "'");
            updateQuery.Append(",HARD_ACTUAL8 =" + "'" + details.HARD_ACTUAL8 + "'");
            updateQuery.Append(",HARD_ACTUAL9 =" + "'" + details.HARD_ACTUAL9 + "'");
            updateQuery.Append(",HARD_ACTUAL10 =" + "'" + details.HARD_ACTUAL10 + "'");
            updateQuery.Append(",HARD_ACTUAL11 =" + "'" + details.HARD_ACTUAL11 + "'");
            updateQuery.Append(",HARD_ACTUAL12 =" + "'" + details.HARD_ACTUAL12 + "'");
            updateQuery.Append(",HARD_ACTUAL13 =" + "'" + details.HARD_ACTUAL13 + "'");
            updateQuery.Append(",HARD_ACTUAL14 =" + "'" + details.HARD_ACTUAL14 + "'");
            updateQuery.Append(",HARD_ACTUAL15 =" + "'" + details.HARD_ACTUAL15 + "'");
            updateQuery.Append(",HARD_ACTUAL16 =" + "'" + details.HARD_ACTUAL16 + "'");
            updateQuery.Append(",HARD_ACTUAL17 =" + "'" + details.HARD_ACTUAL17 + "'");
            updateQuery.Append(",HARD_ACTUAL18 =" + "'" + details.HARD_ACTUAL18 + "'");
            updateQuery.Append(",HARD_ACTUAL19 =" + "'" + details.HARD_ACTUAL19 + "'");
            updateQuery.Append(",HARD_ACTUAL20 =" + "'" + details.HARD_ACTUAL20 + "'");
            updateQuery.Append(",HARD_ACTUAL21 =" + "'" + details.HARD_ACTUAL21 + "'");
            updateQuery.Append(",HARD_ACTUAL22 =" + "'" + details.HARD_ACTUAL22 + "'");
            updateQuery.Append(",HARD_ACTUAL23 =" + "'" + details.HARD_ACTUAL23 + "'");
            updateQuery.Append(",HARD_ACTUAL24 =" + "'" + details.HARD_ACTUAL24 + "'");
            updateQuery.Append(",HARD_ACTUAL25 =" + "'" + details.HARD_ACTUAL25 + "'");

            updateQuery.Append(",UPDATED_DATE = SYSDATE");

            updateQuery.Append(" WHERE MAKER_MILL_SHEET_CD = " + details.MAKER_MILL_SHEET_CD);
            ExecuteNonQuery(updateQuery.ToString());

        }

        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/20
        /// used for Deleted row  MAKER_MILL_SHEET Table  
        /// </summary>
        public void DeleteMakerMillSheetDetails(long MakerMillSheetCD)
        {

            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE FROM  MAKER_MILL_SHEET ");
            deleteQuery.Append(" WHERE MAKER_MILL_SHEET_CD =" + MakerMillSheetCD);
            ExecuteNonQuery(deleteQuery.ToString());
        }


        public bool GetKubunName(string KubunName)
        {
            bool result = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append("SELECT COUNT(*)  FROM MAKER_MILL_SHEET");

            selectQuery.Append(" WHERE LOWER(KOUBAN) = '" + KubunName.ToLower() + "' ");
            int getCount = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (getCount > 0)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/01/28
        /// used for NEXT And Privious PDF Show in   MAKER_MILL_SHEET Table  
        /// </summary>
        public MakerMillSheetDetails GetNextAndPriviousPDFFile(long MakerCD, string PDFPath, int PDFCount)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MM.MAKER_NAME ");
            selectQuery.Append(" ,MMS.* ");


            selectQuery.Append("  FROM MAKER_MILL_SHEET MMS ");
            selectQuery.Append(" INNER JOIN MST_MAKER MM ON");
            selectQuery.Append(" MMS.MAKER_CD = MM.MAKER_CD");


            selectQuery.Append("  WHERE MMS.MAKER_CD= " + MakerCD);
            selectQuery.Append("  AND MMS.PDF_FILE_PATH=  '" + PDFPath + "'");
            selectQuery.Append("  AND MMS.PDF_PAGE_NUMBER= " + PDFCount);

            return ExecuteQuerySingleResult<MakerMillSheetDetails>(selectQuery.ToString());
        }


        /// <summary>
        /// CREATED BY : Sandeep Hajare on 2025/11/18
        /// used for HSKMILLSHEET Screen Showing On  Maker Details  in   MAKER_MILL_SHEET Table  
        /// parameter  Charge and Dim_D 
        /// </summary>
        public List<MakerMillSheetDetails> GetMakerMillsheetDetails(string Charge, string DimD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            //selectQuery.Append(" MM.MAKER_NAME ");
            selectQuery.Append(" MMS.* ");


            selectQuery.Append("  FROM MAKER_MILL_SHEET MMS ");
            selectQuery.Append("  WHERE MMS.CHARGE= '" + Charge + "'");
            selectQuery.Append("  AND MMS.DIM_D=  '" + DimD + "'");


            return ExecuteQueryMultiResult<MakerMillSheetDetails>(selectQuery.ToString());
        }


        public MakerMillSheetDetails GetMakerHardnessDetailJIS(string FileName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            //selectQuery.Append(" MM.MAKER_NAME ");
            selectQuery.Append(" MMS.* ");


            selectQuery.Append("  FROM MAKER_MILL_SHEET MMS ");
            selectQuery.Append("  WHERE MMS.FileName= '" + FileName + "'");



            return ExecuteQuerySingleResult<MakerMillSheetDetails>(selectQuery.ToString());
        }
        public MakerMillSheetDetails GetMakerHardnessDetailJISCal(string FileName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAKER_MILL_SHEET_CD,");
            selectQuery.Append(" ( CASE WHEN	HARD_STD1	<>0 AND	HARD_STD1	IS NOT NULL THEN (TO_CHAR(	HARD_STD1	/10))ELSE NULL  END) AS	HARD_STD1	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN1	<>0 AND	HARD_MIN1	IS NOT NULL THEN ROUND((	HARD_MIN1	),1)ELSE NULL  END) AS	HARD_MIN1	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX1	<>0 AND	HARD_MAX1	IS NOT NULL THEN ROUND((	HARD_MAX1	),1)ELSE NULL  END) AS	HARD_MAX1	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL1	<>0 AND	HARD_ACTUAL1	IS NOT NULL THEN ROUND((	HARD_ACTUAL1	),1)ELSE NULL  END) AS	HARD_ACTUAL1	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD2	<>0 AND	HARD_STD2	IS NOT NULL THEN ROUND((	HARD_STD2	/10),1)ELSE NULL  END) AS	HARD_STD2	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN2	<>0 AND	HARD_MIN2	IS NOT NULL THEN ROUND((	HARD_MIN2	),1)ELSE NULL  END) AS	HARD_MIN2	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX2	<>0 AND	HARD_MAX2	IS NOT NULL THEN ROUND((	HARD_MAX2	),1)ELSE NULL  END) AS	HARD_MAX2	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL2	<>0 AND	HARD_ACTUAL2	IS NOT NULL THEN ROUND((	HARD_ACTUAL2	),1)ELSE NULL  END) AS	HARD_ACTUAL2	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD3	<>0 AND	HARD_STD3	IS NOT NULL THEN ROUND((	HARD_STD3	/10),1)ELSE NULL  END) AS	HARD_STD3	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN3	<>0 AND	HARD_MIN3	IS NOT NULL THEN ROUND((	HARD_MIN3	),1)ELSE NULL  END) AS	HARD_MIN3	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX3	<>0 AND	HARD_MAX3	IS NOT NULL THEN ROUND((	HARD_MAX3	),1)ELSE NULL  END) AS	HARD_MAX3	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL3	<>0 AND	HARD_ACTUAL3	IS NOT NULL THEN ROUND((	HARD_ACTUAL3	),1)ELSE NULL  END) AS	HARD_ACTUAL3	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD4	<>0 AND	HARD_STD4	IS NOT NULL THEN ROUND((	HARD_STD4	/10),1)ELSE NULL  END) AS	HARD_STD4	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN4	<>0 AND	HARD_MIN4	IS NOT NULL THEN ROUND((	HARD_MIN4	),1)ELSE NULL  END) AS	HARD_MIN4	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX4	<>0 AND	HARD_MAX4	IS NOT NULL THEN ROUND((	HARD_MAX4	),1)ELSE NULL  END) AS	HARD_MAX4	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL4	<>0 AND	HARD_ACTUAL4	IS NOT NULL THEN ROUND((	HARD_ACTUAL4	),1)ELSE NULL  END) AS	HARD_ACTUAL4	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD5	<>0 AND	HARD_STD5	IS NOT NULL THEN ROUND((	HARD_STD5	/10),1)ELSE NULL  END) AS	HARD_STD5	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN5	<>0 AND	HARD_MIN5	IS NOT NULL THEN ROUND((	HARD_MIN5	),1)ELSE NULL  END) AS	HARD_MIN5	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX5	<>0 AND	HARD_MAX5	IS NOT NULL THEN ROUND((	HARD_MAX5	),1)ELSE NULL  END) AS	HARD_MAX5	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL5	<>0 AND	HARD_ACTUAL5	IS NOT NULL THEN ROUND((	HARD_ACTUAL5	),1)ELSE NULL  END) AS	HARD_ACTUAL5	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD6	<>0 AND	HARD_STD6	IS NOT NULL THEN ROUND((	HARD_STD6	/10),1)ELSE NULL  END) AS	HARD_STD6	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN6	<>0 AND	HARD_MIN6	IS NOT NULL THEN ROUND((	HARD_MIN6	),1)ELSE NULL  END) AS	HARD_MIN6	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX6	<>0 AND	HARD_MAX6	IS NOT NULL THEN ROUND((	HARD_MAX6	),1)ELSE NULL  END) AS	HARD_MAX6	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL6	<>0 AND	HARD_ACTUAL6	IS NOT NULL THEN ROUND((	HARD_ACTUAL6	),1)ELSE NULL  END) AS	HARD_ACTUAL6	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD7	<>0 AND	HARD_STD7	IS NOT NULL THEN ROUND((	HARD_STD7	/10),1)ELSE NULL  END) AS	HARD_STD7	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN7	<>0 AND	HARD_MIN7	IS NOT NULL THEN ROUND((	HARD_MIN7	),1)ELSE NULL  END) AS	HARD_MIN7	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX7	<>0 AND	HARD_MAX7	IS NOT NULL THEN ROUND((	HARD_MAX7	),1)ELSE NULL  END) AS	HARD_MAX7	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL7	<>0 AND	HARD_ACTUAL7	IS NOT NULL THEN ROUND((	HARD_ACTUAL7	),1)ELSE NULL  END) AS	HARD_ACTUAL7	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD8	<>0 AND	HARD_STD8	IS NOT NULL THEN ROUND((	HARD_STD8	/10),1)ELSE NULL  END) AS	HARD_STD8	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN8	<>0 AND	HARD_MIN8	IS NOT NULL THEN ROUND((	HARD_MIN8	),1)ELSE NULL  END) AS	HARD_MIN8	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX8	<>0 AND	HARD_MAX8	IS NOT NULL THEN ROUND((	HARD_MAX8	),1)ELSE NULL  END) AS	HARD_MAX8	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL8	<>0 AND	HARD_ACTUAL8	IS NOT NULL THEN ROUND((	HARD_ACTUAL8	),1)ELSE NULL  END) AS	HARD_ACTUAL8	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD9	<>0 AND	HARD_STD9	IS NOT NULL THEN ROUND((	HARD_STD9	/10),1)ELSE NULL  END) AS	HARD_STD9	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN9	<>0 AND	HARD_MIN9	IS NOT NULL THEN ROUND((	HARD_MIN9	),1)ELSE NULL  END) AS	HARD_MIN9	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX9	<>0 AND	HARD_MAX9	IS NOT NULL THEN ROUND((	HARD_MAX9	),1)ELSE NULL  END) AS	HARD_MAX9	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL9	<>0 AND	HARD_ACTUAL9	IS NOT NULL THEN ROUND((	HARD_ACTUAL9	),1)ELSE NULL  END) AS	HARD_ACTUAL9	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD10	<>0 AND	HARD_STD10	IS NOT NULL THEN ROUND((	HARD_STD10	/10),1)ELSE NULL  END) AS	HARD_STD10	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN10	<>0 AND	HARD_MIN10	IS NOT NULL THEN ROUND((	HARD_MIN10	),1)ELSE NULL  END) AS	HARD_MIN10	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX10	<>0 AND	HARD_MAX10	IS NOT NULL THEN ROUND((	HARD_MAX10	),1)ELSE NULL  END) AS	HARD_MAX10	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL10	<>0 AND	HARD_ACTUAL10	IS NOT NULL THEN ROUND((	HARD_ACTUAL10	),1)ELSE NULL  END) AS	HARD_ACTUAL10	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD11	<>0 AND	HARD_STD11	IS NOT NULL THEN ROUND((	HARD_STD11	/10),1)ELSE NULL  END) AS	HARD_STD11	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN11	<>0 AND	HARD_MIN11	IS NOT NULL THEN ROUND((	HARD_MIN11	),1)ELSE NULL  END) AS	HARD_MIN11	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX11	<>0 AND	HARD_MAX11	IS NOT NULL THEN ROUND((	HARD_MAX11	),1)ELSE NULL  END) AS	HARD_MAX11	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL11	<>0 AND	HARD_ACTUAL11	IS NOT NULL THEN ROUND((	HARD_ACTUAL11	),1)ELSE NULL  END) AS	HARD_ACTUAL11	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD12	<>0 AND	HARD_STD12	IS NOT NULL THEN ROUND((	HARD_STD12	/10),1)ELSE NULL  END) AS	HARD_STD12	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN12	<>0 AND	HARD_MIN12	IS NOT NULL THEN ROUND((	HARD_MIN12	),1)ELSE NULL  END) AS	HARD_MIN12	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX12	<>0 AND	HARD_MAX12	IS NOT NULL THEN ROUND((	HARD_MAX12	),1) ELSE NULL  END) AS	HARD_MAX12	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL12	<>0 AND	HARD_ACTUAL12	IS NOT NULL THEN ROUND((	HARD_ACTUAL12	),1)ELSE NULL  END) AS	HARD_ACTUAL12	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD13	<>0 AND	HARD_STD13	IS NOT NULL THEN ROUND((	HARD_STD13	/10),1)ELSE NULL  END) AS	HARD_STD13	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN13	<>0 AND	HARD_MIN13	IS NOT NULL THEN ROUND((	HARD_MIN13	),1)ELSE NULL  END) AS	HARD_MIN13	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX13	<>0 AND	HARD_MAX13	IS NOT NULL THEN ROUND((	HARD_MAX13	),1)ELSE NULL  END) AS	HARD_MAX13	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL13	<>0 AND	HARD_ACTUAL13	IS NOT NULL THEN ROUND((	HARD_ACTUAL13	),1)ELSE NULL  END) AS	HARD_ACTUAL13	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD14	<>0 AND	HARD_STD14	IS NOT NULL THEN ROUND((	HARD_STD14	/10),1)ELSE NULL  END) AS	HARD_STD14	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN14	<>0 AND	HARD_MIN14	IS NOT NULL THEN ROUND((	HARD_MIN14	),1)ELSE NULL  END) AS	HARD_MIN14	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX14	<>0 AND	HARD_MAX14	IS NOT NULL THEN ROUND((	HARD_MAX14	),1)ELSE NULL  END) AS	HARD_MAX14	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL14	<>0 AND	HARD_ACTUAL14	IS NOT NULL THEN ROUND((	HARD_ACTUAL14	),1)ELSE NULL  END) AS	HARD_ACTUAL14	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD15	<>0 AND	HARD_STD15	IS NOT NULL THEN ROUND((	HARD_STD15	/10),1)ELSE NULL  END) AS	HARD_STD15	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN15	<>0 AND	HARD_MIN15	IS NOT NULL THEN ROUND((	HARD_MIN15	),1)ELSE NULL  END) AS	HARD_MIN15	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX15	<>0 AND	HARD_MAX15	IS NOT NULL THEN ROUND((	HARD_MAX15	),1)ELSE NULL  END) AS	HARD_MAX15	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL15	<>0 AND	HARD_ACTUAL15	IS NOT NULL THEN ROUND((	HARD_ACTUAL15	),1)ELSE NULL  END) AS	HARD_ACTUAL15	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD16	<>0 AND	HARD_STD16	IS NOT NULL THEN ROUND((	HARD_STD16	/10),1)ELSE NULL  END) AS	HARD_STD16	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN16	<>0 AND	HARD_MIN16	IS NOT NULL THEN ROUND((	HARD_MIN16	),1)ELSE NULL  END) AS	HARD_MIN16	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX16	<>0 AND	HARD_MAX16	IS NOT NULL THEN ROUND((	HARD_MAX16	),1)ELSE NULL  END) AS	HARD_MAX16	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL16	<>0 AND	HARD_ACTUAL16	IS NOT NULL THEN ROUND((	HARD_ACTUAL16	),1)ELSE NULL  END) AS	HARD_ACTUAL16	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD17	<>0 AND	HARD_STD17	IS NOT NULL THEN ROUND((	HARD_STD17	/10),1)ELSE NULL  END) AS	HARD_STD17	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN17	<>0 AND	HARD_MIN17	IS NOT NULL THEN ROUND((	HARD_MIN17	),1)ELSE NULL  END) AS	HARD_MIN17	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX17	<>0 AND	HARD_MAX17	IS NOT NULL THEN ROUND((	HARD_MAX17	),1)ELSE NULL  END) AS	HARD_MAX17	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL17	<>0 AND	HARD_ACTUAL17	IS NOT NULL THEN ROUND((	HARD_ACTUAL17	),1)ELSE NULL  END) AS	HARD_ACTUAL17	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD18	<>0 AND	HARD_STD18	IS NOT NULL THEN ROUND((	HARD_STD18	/10),1)ELSE NULL  END) AS	HARD_STD18	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN18	<>0 AND	HARD_MIN18	IS NOT NULL THEN ROUND((	HARD_MIN18	),1)ELSE NULL  END) AS	HARD_MIN18	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX18	<>0 AND	HARD_MAX18	IS NOT NULL THEN ROUND((	HARD_MAX18	),1)ELSE NULL  END) AS	HARD_MAX18	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL18	<>0 AND	HARD_ACTUAL18	IS NOT NULL THEN ROUND((	HARD_ACTUAL18	),1)ELSE NULL  END) AS	HARD_ACTUAL18	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD19	<>0 AND	HARD_STD19	IS NOT NULL THEN ROUND((	HARD_STD19	/10),1)ELSE NULL  END) AS	HARD_STD19	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN19	<>0 AND	HARD_MIN19	IS NOT NULL THEN ROUND((	HARD_MIN19	),1)ELSE NULL  END) AS	HARD_MIN19	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX19	<>0 AND	HARD_MAX19	IS NOT NULL THEN ROUND((	HARD_MAX19	),1)ELSE NULL  END) AS	HARD_MAX19	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL19	<>0 AND	HARD_ACTUAL19	IS NOT NULL THEN ROUND((	HARD_ACTUAL19	),1)ELSE NULL  END) AS	HARD_ACTUAL19	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD20	<>0 AND	HARD_STD20	IS NOT NULL THEN ROUND((	HARD_STD20	/10),1)ELSE NULL  END) AS	HARD_STD20	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN20	<>0 AND	HARD_MIN20	IS NOT NULL THEN ROUND((	HARD_MIN20	),1)ELSE NULL  END) AS	HARD_MIN20	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX20	<>0 AND	HARD_MAX20	IS NOT NULL THEN ROUND((	HARD_MAX20	),1)ELSE NULL  END) AS	HARD_MAX20	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL20	<>0 AND	HARD_ACTUAL20	IS NOT NULL THEN ROUND((	HARD_ACTUAL20	),1)ELSE NULL  END) AS	HARD_ACTUAL20	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD21	<>0 AND	HARD_STD21	IS NOT NULL THEN ROUND((	HARD_STD21	/10),1)ELSE NULL  END) AS	HARD_STD21	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN21	<>0 AND	HARD_MIN21	IS NOT NULL THEN ROUND((	HARD_MIN21	),1)ELSE NULL  END) AS	HARD_MIN21	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX21	<>0 AND	HARD_MAX21	IS NOT NULL THEN ROUND((	HARD_MAX21	),1)ELSE NULL  END) AS	HARD_MAX21	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL21	<>0 AND	HARD_ACTUAL21	IS NOT NULL THEN ROUND((	HARD_ACTUAL21	),1)ELSE NULL  END) AS	HARD_ACTUAL21	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD22	<>0 AND	HARD_STD22	IS NOT NULL THEN ROUND((	HARD_STD22	/10),1)ELSE NULL  END) AS	HARD_STD22	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN22	<>0 AND	HARD_MIN22	IS NOT NULL THEN ROUND((	HARD_MIN22	),1)ELSE NULL  END) AS	HARD_MIN22	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX22	<>0 AND	HARD_MAX22	IS NOT NULL THEN ROUND((	HARD_MAX22	),1)ELSE NULL  END) AS	HARD_MAX22	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL22	<>0 AND	HARD_ACTUAL22	IS NOT NULL THEN ROUND((	HARD_ACTUAL22	),1)ELSE NULL  END) AS	HARD_ACTUAL22	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD23	<>0 AND	HARD_STD23	IS NOT NULL THEN ROUND((	HARD_STD23	/10),1)ELSE NULL  END) AS	HARD_STD23	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN23	<>0 AND	HARD_MIN23	IS NOT NULL THEN ROUND((	HARD_MIN23	),1)ELSE NULL  END) AS	HARD_MIN23	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX23	<>0 AND	HARD_MAX23	IS NOT NULL THEN ROUND((	HARD_MAX23	),1)ELSE NULL  END) AS	HARD_MAX23	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL23	<>0 AND	HARD_ACTUAL23	IS NOT NULL THEN ROUND((	HARD_ACTUAL23	),1)ELSE NULL  END) AS	HARD_ACTUAL23	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD24	<>0 AND	HARD_STD24	IS NOT NULL THEN ROUND((	HARD_STD24	/10),1)ELSE NULL  END) AS	HARD_STD24	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN24	<>0 AND	HARD_MIN24	IS NOT NULL THEN ROUND((	HARD_MIN24	),1)ELSE NULL  END) AS	HARD_MIN24	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX24	<>0 AND	HARD_MAX24	IS NOT NULL THEN ROUND((	HARD_MAX24	),1)ELSE NULL  END) AS	HARD_MAX24	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL24	<>0 AND	HARD_ACTUAL24	IS NOT NULL THEN ROUND((	HARD_ACTUAL24	),1)ELSE NULL  END) AS	HARD_ACTUAL24	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_STD25	<>0 AND	HARD_STD25	IS NOT NULL THEN ROUND((	HARD_STD25	/10),1)ELSE NULL  END) AS	HARD_STD25	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MIN25	<>0 AND	HARD_MIN25	IS NOT NULL THEN ROUND((	HARD_MIN25	),1)ELSE NULL  END) AS	HARD_MIN25	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_MAX25	<>0 AND	HARD_MAX25	IS NOT NULL THEN ROUND((	HARD_MAX25	),1)ELSE NULL  END) AS	HARD_MAX25	,	");
            selectQuery.Append(" ( CASE WHEN	HARD_ACTUAL25	<>0 AND	HARD_ACTUAL25	IS NOT NULL THEN ROUND((	HARD_ACTUAL25	),1)ELSE NULL  END) AS	HARD_ACTUAL25 ");


            selectQuery.Append("  FROM MAKER_MILL_SHEET MMS ");
            selectQuery.Append("  WHERE MMS.FileName= '" + FileName + "'");



            return ExecuteQuerySingleResult<MakerMillSheetDetails>(selectQuery.ToString());
        }


        public bool IsDefaultBoxPositionSaved(string templateName)
        {
            bool returnValue = false;

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append("  FROM MAKER_MILL_SHEET_CONFIG ");

            selectQuery.Append("  WHERE LOWER(UNIQUE_TEMPLATE_NAME) = '" + templateName.Trim().ToLower() + "'");
            selectQuery.Append("  AND FIELD_CD = '999' ");
            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            if (count > 0)
            {
                returnValue = true;
            }
            return returnValue;
        }
        public void SaveSettings(MakerMillSheetConfiguration request)
        {

            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO ");
            insertQuery.Append(" MAKER_MILL_SHEET_CONFIG ");
            insertQuery.Append(" ( ");
            insertQuery.Append(" MAKER_CONFIG_CD ");
            insertQuery.Append(" ,MAKER_CD ");
            insertQuery.Append(" ,UNIQUE_TEMPLATE_NAME ");
            insertQuery.Append(" ,FIELD_CD ");
            insertQuery.Append(" , TOP_LEFT_X ");
            insertQuery.Append(" , TOP_LEFT_Y ");
            insertQuery.Append(" , TOP_RIGHT_X ");
            insertQuery.Append(" , TOP_RIGHT_Y ");
            insertQuery.Append(" , BOTTOM_LEFT_X ");
            insertQuery.Append(" , BOTTOM_LEFT_Y  ");
            insertQuery.Append(" , BOTTOM_RIGHT_X ");
            insertQuery.Append(" , BOTTOM_RIGHT_Y ");

            insertQuery.Append(" ,USER_ID ");
            insertQuery.Append(" ,CREATED_DATE ");
            insertQuery.Append(" ,UPDATED_DATE ");
            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES ");
            insertQuery.Append(" ( ");
            insertQuery.Append(" MILLSHEET_CONFIGURATION_SEQ.NEXTVAL ");
            insertQuery.Append(" , " + request.MAKER_CD);
            insertQuery.Append(" , '" + request.UNIQUE_TEMPLATE_NAME + "'");
            insertQuery.Append(" , '" + request.FIELD_CD + "'");
            insertQuery.Append(" , " + request.TOP_LEFT_X);
            insertQuery.Append(" , " + request.TOP_LEFT_Y);
            insertQuery.Append(" , " + request.TOP_RIGHT_X);
            insertQuery.Append(" , " + request.TOP_RIGHT_Y);
            insertQuery.Append(" , " + request.BOTTOM_LEFT_X);
            insertQuery.Append(" , " + request.BOTTOM_LEFT_Y);
            insertQuery.Append(" , " + request.BOTTOM_RIGHT_X);
            insertQuery.Append(" , " + request.BOTTOM_RIGHT_Y);
            insertQuery.Append(" , " + request.USER_ID);
            insertQuery.Append(", SYSDATE");
            insertQuery.Append(", SYSDATE");
            insertQuery.Append(")");
            ExecuteNonQuery(insertQuery.ToString());

        }
        public void UpdateSettings(MakerMillSheetConfiguration request)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE ");
            updateQuery.Append(" MAKER_MILL_SHEET_CONFIG ");
            updateQuery.Append(" SET ");
            updateQuery.Append(" TOP_LEFT_X = '" + request.TOP_LEFT_X + "'");
            updateQuery.Append(" ,TOP_LEFT_Y = '" + request.TOP_LEFT_Y + "'");
            updateQuery.Append(" ,TOP_RIGHT_X = '" + request.TOP_RIGHT_X + "'");
            updateQuery.Append(" ,TOP_RIGHT_Y = '" + request.TOP_RIGHT_Y + "'");
            updateQuery.Append(" ,BOTTOM_LEFT_X = '" + request.BOTTOM_LEFT_X + "'");
            updateQuery.Append(" ,BOTTOM_LEFT_Y = '" + request.BOTTOM_LEFT_Y + "'");
            updateQuery.Append(" ,BOTTOM_RIGHT_X = '" + request.BOTTOM_RIGHT_X + "'");
            updateQuery.Append(" ,BOTTOM_RIGHT_Y = '" + request.BOTTOM_RIGHT_Y + "'");
            updateQuery.Append(" ,UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" UNIQUE_TEMPLATE_NAME = '" + request.UNIQUE_TEMPLATE_NAME + "'");
            updateQuery.Append(" AND ");
            updateQuery.Append(" MAKER_CD = '" + request.MAKER_CD + "'");
            updateQuery.Append(" AND ");
            updateQuery.Append(" FIELD_CD = '" + request.FIELD_CD + "'");
            ExecuteNonQuery(updateQuery.ToString());

        }
        public List<NameValue> GetMakerList()
        {
            StringBuilder selectQuery = new StringBuilder();


            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MK.MAKER_CD AS CD ");
            selectQuery.Append(" ,MK.MAKER_NAME AS NAME ");
            selectQuery.Append(" ,MK.IN_ACTIVE_FLAG");
            //selectQuery.Append(" ,NVL (");
            //selectQuery.Append(" ( SELECT CASE WHEN MAKER_CD IS NOT NULL THEN 0 END ");
            //selectQuery.Append(" FROM MAKER_MILL_SHEET_CONFIG WHERE MAKER_CD=MK.MAKER_CD AND FIELD_CD = '999' ");
            //selectQuery.Append(" ), 1) AS VALUE ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" MST_MAKER MK");

            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MK.IN_ACTIVE_FLAG = 0 ");
            selectQuery.Append(" ORDER BY MK.DISPLAY_PRIORITY ASC ");

            return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
        }
        public List<MakerMillSheetConfiguration> GetPDFDetails(int makerCD, string templateName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAKER_CD ");
            selectQuery.Append(" ,FIELD_CD ");
            selectQuery.Append(" , TOP_LEFT_X ");
            selectQuery.Append(" , TOP_LEFT_Y ");
            selectQuery.Append(" , TOP_RIGHT_X ");
            selectQuery.Append(" , TOP_RIGHT_Y ");
            selectQuery.Append(" , BOTTOM_LEFT_X ");
            selectQuery.Append(" , BOTTOM_LEFT_Y ");
            selectQuery.Append(" , BOTTOM_RIGHT_X ");
            selectQuery.Append(" , BOTTOM_RIGHT_Y ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET_CONFIG ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MAKER_CD = " + makerCD);
            selectQuery.Append(" AND LOWER(UNIQUE_TEMPLATE_NAME) = '" + templateName.ToLower() + "'");
            selectQuery.Append(" ORDER BY FIELD_CD ASC");
            var result = ExecuteQueryMultiResult<MakerMillSheetConfiguration>(selectQuery.ToString());
            return result;
        }
        public int GetLayoutData(string templateName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(MAKER_CD) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET_CONFIG");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FIELD_CD = '999' ");
            selectQuery.Append(" AND ");
            selectQuery.Append(" LOWER(UNIQUE_TEMPLATE_NAME) =  '" + templateName.Trim().ToLower() + "'");
            int rowsCount = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));

            return rowsCount;
        }
        public List<MakerMillSheetConfiguration> GetPDFConfigList(List<SearchField> searchFields, string sortCiteria)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MKR.SHORT_NAME AS MAKER_NAME");
            selectQuery.Append(" , CONF.UNIQUE_TEMPLATE_NAME");


            selectQuery.Append("  FROM MAKER_MILL_SHEET_CONFIG CONF");
            selectQuery.Append("  INNER JOIN MST_MAKER MKR ON MKR.MAKER_CD = CONF.MAKER_CD");
            selectQuery.Append("  WHERE ");

            var query = selectQuery.ToString();
            if (searchFields != null && searchFields.Count > 0)
            {
                string whereCondition = "";
                string andOperator = string.Empty;

                foreach (var searchField in searchFields)
                {
                    whereCondition = whereCondition + andOperator + DynamicSearchProvider.ApplySearchCriteria(searchField, query);
                    andOperator = " AND ";
                }
                query = query + whereCondition;
                query += " GROUP BY CONF.MAKER_CD, MKR.SHORT_NAME,CONF.UNIQUE_TEMPLATE_NAME ";
            }
            if (!string.IsNullOrEmpty(sortCiteria))
            {
                query = Helper.ApplySortCriteria(sortCiteria, query);
            }
            else
            {
                query += " ORDER BY CONF.MAKER_CD DESC, CONF.UNIQUE_TEMPLATE_NAME DESC";
            }
            return ExecuteQueryMultiResult<MakerMillSheetConfiguration>(query).ToList();
        }
        public List<MakerMillSheetConfiguration> GetPDFDetailsByTemplateName(string templateName)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" * ");

            selectQuery.Append("  FROM MAKER_MILL_SHEET_CONFIG ");
            selectQuery.Append("  WHERE LOWER(UNIQUE_TEMPLATE_NAME) = '" + templateName.ToLower() + "'");

            return ExecuteQueryMultiResult<MakerMillSheetConfiguration>(selectQuery.ToString());
        }
        public void DeleteMillsheetConfigByFieldCD(MakerMillSheetConfiguration request)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE ");
            deleteQuery.Append(" FROM ");

            deleteQuery.Append("  MAKER_MILL_SHEET_CONFIG ");
            deleteQuery.Append("  WHERE LOWER(UNIQUE_TEMPLATE_NAME) = '" + request.UNIQUE_TEMPLATE_NAME.ToLower() + "'");
            deleteQuery.Append("  AND FIELD_CD = '" + request.FIELD_CD + "'");

            ExecuteNonQuery(deleteQuery.ToString());
        }
        public void DeleteConfigByTemplateName(string templateName)
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append(" DELETE ");
            deleteQuery.Append(" FROM ");

            deleteQuery.Append("  MAKER_MILL_SHEET_CONFIG ");
            deleteQuery.Append("  WHERE LOWER(UNIQUE_TEMPLATE_NAME) = '" + templateName.ToLower() + "'");

            ExecuteNonQuery(deleteQuery.ToString());
        }
        public bool CheckTemplateName(string templateName)
        {
            bool returnVal = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(MAKER_CONFIG_CD) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append("  MAKER_MILL_SHEET_CONFIG ");
            selectQuery.Append("  WHERE LOWER(UNIQUE_TEMPLATE_NAME) = '" + templateName.ToLower() + "'");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                returnVal = true;
            }
            return returnVal;
        }
        public List<NameValue> GetTemplateList()
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" MAX(MAKER_CONFIG_CD) AS MAKER_CONFIG_CD ");
            selectQuery.Append(" ,MAX(MAKER_CD) AS CD ");
            selectQuery.Append(" ,UNIQUE_TEMPLATE_NAME AS NAME ");

            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET_CONFIG");
            selectQuery.Append(" GROUP BY ");
            selectQuery.Append(" UNIQUE_TEMPLATE_NAME ");

            selectQuery.Append(" ORDER BY MAKER_CONFIG_CD DESC ");

            return ExecuteQueryMultiResult<NameValue>(selectQuery.ToString()).ToList();
        }
        public int GetMakerByConfigCD(int configCD)
        {
            StringBuilder selectQuery = new StringBuilder();

            selectQuery.Append(" SELECT ");
            selectQuery.Append(" CF.MAKER_CD ");

            selectQuery.Append(" FROM ");
            // selectQuery.Append(" MST_MAKER MK ");
            selectQuery.Append("MAKER_MILL_SHEET_CONFIG CF");
            // selectQuery.Append(" ON MK.MAKER_CD = CF.MAKER_CD ");

            // selectQuery.Append(" WHERE MK.IN_ACTIVE_FLAG = 0 ");
            selectQuery.Append(" WHERE  MK.MAKER_CD = " + configCD);

            return Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
        }
        public int GetMaxRunCD()
        {
            makerMillSheetRunID_Seq = Convert.ToString(_configuration.GetSection("Millsheet_Seq").GetSection("Maker_Mill_Sheet_RunID").Value);
            int runCD = 0;
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
            runCD = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            return runCD;
        }
        public int InsertMakerMillSheetLogDetails(MakerMillSheetDetails request)
        {
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
            logCD = Convert.ToInt32(ExecuteScalar("SELECT " + seqName  +" FROM DUAL"));
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append(" INSERT INTO MAKER_MILL_SHEET_LOG(");
            insertQuery.Append(" MAKER_MILL_SHEET_LOG_CD");
            insertQuery.Append(" ,RUN_CD");
            insertQuery.Append(" ,MAKER_NAME ");
            insertQuery.Append(" ,PDF_FILE_PATH ");
            insertQuery.Append(" ,FILENAME ");
            insertQuery.Append(" ,KOUBAN");
            insertQuery.Append(" ,CHARGE");
            insertQuery.Append(" ,MATL_NAME_MAKER");
            insertQuery.Append(" ,DIM_D ");
            insertQuery.Append(" ,LENGTH");
            insertQuery.Append(" ,NO_OF_UNITS");
            insertQuery.Append(" ,UNIT");
            insertQuery.Append(" ,QUANTITY");
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
            insertQuery.Append(" ,STATUS ");
            insertQuery.Append(" ,STATUS_MARK ");
            insertQuery.Append(" ,MESSAGE ");

            insertQuery.Append(" ,CREATED_DATE");


            insertQuery.Append(" ) ");
            insertQuery.Append(" VALUES(");
            insertQuery.Append("'" + logCD + "'");
            insertQuery.Append(",'" + request.RUN_CD + "'");
            insertQuery.Append(",'" + request.MAKER_NAME + "'");
            insertQuery.Append(",'" + request.PDF_FILE_PATH + "'");
            insertQuery.Append(",'" + request.FILENAME + "'");
            insertQuery.Append(",'" + request.KOUBAN + "'");
            insertQuery.Append(",'" + request.CHARGE + "'");
            insertQuery.Append(",'" + request.MATL_NAME_MAKER + "'");
            insertQuery.Append(",'" + request.DIM_D + "'");
            insertQuery.Append(",'" + request.LENGTH + "'");
            insertQuery.Append(",'" + request.NO_OF_UNITS + "'");
            insertQuery.Append(",'" + request.UNIT + "'");
            insertQuery.Append(",'" + request.QUANTITY + "'");
            insertQuery.Append(",'" + request.GRAIN_SIZE_STD + "'");
            insertQuery.Append(",'" + request.GRAIN_SIZE_ACTUAL + "'");
            insertQuery.Append(",'" + request.HARDNESS_STD + "'");
            insertQuery.Append(",'" + request.HARDNESS_MIN + "'");
            insertQuery.Append(",'" + request.HARDNESS_MAX + "'");
            insertQuery.Append(",'" + request.HARDNESS_ACTUAL + "'");
            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
            insertQuery.Append(",'" + request.CCNAME1 + "'");
            insertQuery.Append(",'" + request.CCNAME2 + "'");
            insertQuery.Append(",'" + request.CCNAME3 + "'");
            insertQuery.Append(",'" + request.CCNAME4 + "'");
            insertQuery.Append(",'" + request.CCNAME5 + "'");
            insertQuery.Append(",'" + request.CCNAME6 + "'");
            insertQuery.Append(",'" + request.CCNAME7 + "'");
            insertQuery.Append(",'" + request.CCNAME8 + "'");
            insertQuery.Append(",'" + request.CCNAME9 + "'");
            insertQuery.Append(",'" + request.CCNAME10 + "'");
            insertQuery.Append(",'" + request.CCNAME11 + "'");
            insertQuery.Append(",'" + request.CCNAME12 + "'");
            insertQuery.Append(",'" + request.CCNAME13 + "'");
            insertQuery.Append(",'" + request.CCNAME14 + "'");
            insertQuery.Append(",'" + request.CCNAME15 + "'");
            insertQuery.Append(",'" + request.CCNAME16 + "'");
            insertQuery.Append(",'" + request.CCNAME17 + "'");
            insertQuery.Append(",'" + request.CCNAME18 + "'");
            insertQuery.Append(",'" + request.CCNAME19 + "'");
            insertQuery.Append(",'" + request.CCNAME20 + "'");
            insertQuery.Append(",'" + request.CCNAME21 + "'");
            insertQuery.Append(",'" + request.CCNAME22 + "'");
            insertQuery.Append(",'" + request.CCNAME23 + "'");
            insertQuery.Append(",'" + request.CCNAME24 + "'");
            insertQuery.Append(",'" + request.CCNAME25 + "'");
            insertQuery.Append(",'" + request.CCRANGE1 + "'");
            insertQuery.Append(",'" + request.CCRANGE2 + "'");
            insertQuery.Append(",'" + request.CCRANGE3 + "'");
            insertQuery.Append(",'" + request.CCRANGE4 + "'");
            insertQuery.Append(",'" + request.CCRANGE5 + "'");
            insertQuery.Append(",'" + request.CCRANGE6 + "'");
            insertQuery.Append(",'" + request.CCRANGE7 + "'");
            insertQuery.Append(",'" + request.CCRANGE8 + "'");
            insertQuery.Append(",'" + request.CCRANGE9 + "'");
            insertQuery.Append(",'" + request.CCRANGE10 + "'");
            insertQuery.Append(",'" + request.CCRANGE11 + "'");
            insertQuery.Append(",'" + request.CCRANGE12 + "'");
            insertQuery.Append(",'" + request.CCRANGE13 + "'");
            insertQuery.Append(",'" + request.CCRANGE14 + "'");
            insertQuery.Append(",'" + request.CCRANGE15 + "'");
            insertQuery.Append(",'" + request.CCRANGE16 + "'");
            insertQuery.Append(",'" + request.CCRANGE17 + "'");
            insertQuery.Append(",'" + request.CCRANGE18 + "'");
            insertQuery.Append(",'" + request.CCRANGE19 + "'");
            insertQuery.Append(",'" + request.CCRANGE20 + "'");
            insertQuery.Append(",'" + request.CCRANGE21 + "'");
            insertQuery.Append(",'" + request.CCRANGE22 + "'");
            insertQuery.Append(",'" + request.CCRANGE23 + "'");
            insertQuery.Append(",'" + request.CCRANGE24 + "'");
            insertQuery.Append(",'" + request.CCRANGE25 + "'");
            insertQuery.Append(",'" + request.CCMIN1 + "'");
            insertQuery.Append(",'" + request.CCMIN2 + "'");
            insertQuery.Append(",'" + request.CCMIN3 + "'");
            insertQuery.Append(",'" + request.CCMIN4 + "'");
            insertQuery.Append(",'" + request.CCMIN5 + "'");
            insertQuery.Append(",'" + request.CCMIN6 + "'");
            insertQuery.Append(",'" + request.CCMIN7 + "'");
            insertQuery.Append(",'" + request.CCMIN8 + "'");
            insertQuery.Append(",'" + request.CCMIN9 + "'");
            insertQuery.Append(",'" + request.CCMIN10 + "'");
            insertQuery.Append(",'" + request.CCMIN11 + "'");
            insertQuery.Append(",'" + request.CCMIN12 + "'");
            insertQuery.Append(",'" + request.CCMIN13 + "'");
            insertQuery.Append(",'" + request.CCMIN14 + "'");
            insertQuery.Append(",'" + request.CCMIN15 + "'");
            insertQuery.Append(",'" + request.CCMIN16 + "'");
            insertQuery.Append(",'" + request.CCMIN17 + "'");
            insertQuery.Append(",'" + request.CCMIN18 + "'");
            insertQuery.Append(",'" + request.CCMIN19 + "'");
            insertQuery.Append(",'" + request.CCMIN20 + "'");
            insertQuery.Append(",'" + request.CCMIN21 + "'");
            insertQuery.Append(",'" + request.CCMIN22 + "'");
            insertQuery.Append(",'" + request.CCMIN23 + "'");
            insertQuery.Append(",'" + request.CCMIN24 + "'");
            insertQuery.Append(",'" + request.CCMIN25 + "'");
            insertQuery.Append(",'" + request.CCMAX1 + "'");
            insertQuery.Append(",'" + request.CCMAX2 + "'");
            insertQuery.Append(",'" + request.CCMAX3 + "'");
            insertQuery.Append(",'" + request.CCMAX4 + "'");
            insertQuery.Append(",'" + request.CCMAX5 + "'");
            insertQuery.Append(",'" + request.CCMAX6 + "'");
            insertQuery.Append(",'" + request.CCMAX7 + "'");
            insertQuery.Append(",'" + request.CCMAX8 + "'");
            insertQuery.Append(",'" + request.CCMAX9 + "'");
            insertQuery.Append(",'" + request.CCMAX10 + "'");
            insertQuery.Append(",'" + request.CCMAX11 + "'");
            insertQuery.Append(",'" + request.CCMAX12 + "'");
            insertQuery.Append(",'" + request.CCMAX13 + "'");
            insertQuery.Append(",'" + request.CCMAX14 + "'");
            insertQuery.Append(",'" + request.CCMAX15 + "'");
            insertQuery.Append(",'" + request.CCMAX16 + "'");
            insertQuery.Append(",'" + request.CCMAX17 + "'");
            insertQuery.Append(",'" + request.CCMAX18 + "'");
            insertQuery.Append(",'" + request.CCMAX19 + "'");
            insertQuery.Append(",'" + request.CCMAX20 + "'");
            insertQuery.Append(",'" + request.CCMAX21 + "'");
            insertQuery.Append(",'" + request.CCMAX22 + "'");
            insertQuery.Append(",'" + request.CCMAX23 + "'");
            insertQuery.Append(",'" + request.CCMAX24 + "'");
            insertQuery.Append(",'" + request.CCMAX25 + "'");
            insertQuery.Append(",'" + request.CCACTUAL1 + "'");
            insertQuery.Append(",'" + request.CCACTUAL2 + "'");
            insertQuery.Append(",'" + request.CCACTUAL3 + "'");
            insertQuery.Append(",'" + request.CCACTUAL4 + "'");
            insertQuery.Append(",'" + request.CCACTUAL5 + "'");
            insertQuery.Append(",'" + request.CCACTUAL6 + "'");
            insertQuery.Append(",'" + request.CCACTUAL7 + "'");
            insertQuery.Append(",'" + request.CCACTUAL8 + "'");
            insertQuery.Append(",'" + request.CCACTUAL9 + "'");
            insertQuery.Append(",'" + request.CCACTUAL10 + "'");
            insertQuery.Append(",'" + request.CCACTUAL11 + "'");
            insertQuery.Append(",'" + request.CCACTUAL12 + "'");
            insertQuery.Append(",'" + request.CCACTUAL13 + "'");
            insertQuery.Append(",'" + request.CCACTUAL14 + "'");
            insertQuery.Append(",'" + request.CCACTUAL15 + "'");
            insertQuery.Append(",'" + request.CCACTUAL16 + "'");
            insertQuery.Append(",'" + request.CCACTUAL17 + "'");
            insertQuery.Append(",'" + request.CCACTUAL18 + "'");
            insertQuery.Append(",'" + request.CCACTUAL19 + "'");
            insertQuery.Append(",'" + request.CCACTUAL20 + "'");
            insertQuery.Append(",'" + request.CCACTUAL21 + "'");
            insertQuery.Append(",'" + request.CCACTUAL22 + "'");
            insertQuery.Append(",'" + request.CCACTUAL23 + "'");
            insertQuery.Append(",'" + request.CCACTUAL24 + "'");
            insertQuery.Append(",'" + request.CCACTUAL25 + "'");
            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------
            insertQuery.Append(",'" + request.HARD_STD1 + "'");
            insertQuery.Append(",'" + request.HARD_STD2 + "'");
            insertQuery.Append(",'" + request.HARD_STD3 + "'");
            insertQuery.Append(",'" + request.HARD_STD4 + "'");
            insertQuery.Append(",'" + request.HARD_STD5 + "'");
            insertQuery.Append(",'" + request.HARD_STD6 + "'");
            insertQuery.Append(",'" + request.HARD_STD7 + "'");
            insertQuery.Append(",'" + request.HARD_STD8 + "'");
            insertQuery.Append(",'" + request.HARD_STD9 + "'");
            insertQuery.Append(",'" + request.HARD_STD10 + "'");
            insertQuery.Append(",'" + request.HARD_STD11 + "'");
            insertQuery.Append(",'" + request.HARD_STD12 + "'");
            insertQuery.Append(",'" + request.HARD_STD13 + "'");
            insertQuery.Append(",'" + request.HARD_STD14 + "'");
            insertQuery.Append(",'" + request.HARD_STD15 + "'");
            insertQuery.Append(",'" + request.HARD_STD16 + "'");
            insertQuery.Append(",'" + request.HARD_STD17 + "'");
            insertQuery.Append(",'" + request.HARD_STD18 + "'");
            insertQuery.Append(",'" + request.HARD_STD19 + "'");
            insertQuery.Append(",'" + request.HARD_STD20 + "'");
            insertQuery.Append(",'" + request.HARD_STD21 + "'");
            insertQuery.Append(",'" + request.HARD_STD22 + "'");
            insertQuery.Append(",'" + request.HARD_STD23 + "'");
            insertQuery.Append(",'" + request.HARD_STD24 + "'");
            insertQuery.Append(",'" + request.HARD_STD25 + "'");
            insertQuery.Append(",'" + request.HARD_MIN1 + "'");
            insertQuery.Append(",'" + request.HARD_MIN2 + "'");
            insertQuery.Append(",'" + request.HARD_MIN3 + "'");
            insertQuery.Append(",'" + request.HARD_MIN4 + "'");
            insertQuery.Append(",'" + request.HARD_MIN5 + "'");
            insertQuery.Append(",'" + request.HARD_MIN6 + "'");
            insertQuery.Append(",'" + request.HARD_MIN7 + "'");
            insertQuery.Append(",'" + request.HARD_MIN8 + "'");
            insertQuery.Append(",'" + request.HARD_MIN9 + "'");
            insertQuery.Append(",'" + request.HARD_MIN10 + "'");
            insertQuery.Append(",'" + request.HARD_MIN11 + "'");
            insertQuery.Append(",'" + request.HARD_MIN12 + "'");
            insertQuery.Append(",'" + request.HARD_MIN13 + "'");
            insertQuery.Append(",'" + request.HARD_MIN14 + "'");
            insertQuery.Append(",'" + request.HARD_MIN15 + "'");
            insertQuery.Append(",'" + request.HARD_MIN16 + "'");
            insertQuery.Append(",'" + request.HARD_MIN17 + "'");
            insertQuery.Append(",'" + request.HARD_MIN18 + "'");
            insertQuery.Append(",'" + request.HARD_MIN19 + "'");
            insertQuery.Append(",'" + request.HARD_MIN20 + "'");
            insertQuery.Append(",'" + request.HARD_MIN21 + "'");
            insertQuery.Append(",'" + request.HARD_MIN22 + "'");
            insertQuery.Append(",'" + request.HARD_MIN23 + "'");
            insertQuery.Append(",'" + request.HARD_MIN24 + "'");
            insertQuery.Append(",'" + request.HARD_MIN25 + "'");
            insertQuery.Append(",'" + request.HARD_MAX1 + "'");
            insertQuery.Append(",'" + request.HARD_MAX2 + "'");
            insertQuery.Append(",'" + request.HARD_MAX3 + "'");
            insertQuery.Append(",'" + request.HARD_MAX4 + "'");
            insertQuery.Append(",'" + request.HARD_MAX5 + "'");
            insertQuery.Append(",'" + request.HARD_MAX6 + "'");
            insertQuery.Append(",'" + request.HARD_MAX7 + "'");
            insertQuery.Append(",'" + request.HARD_MAX8 + "'");
            insertQuery.Append(",'" + request.HARD_MAX9 + "'");
            insertQuery.Append(",'" + request.HARD_MAX10 + "'");
            insertQuery.Append(",'" + request.HARD_MAX11 + "'");
            insertQuery.Append(",'" + request.HARD_MAX12 + "'");
            insertQuery.Append(",'" + request.HARD_MAX13 + "'");
            insertQuery.Append(",'" + request.HARD_MAX14 + "'");
            insertQuery.Append(",'" + request.HARD_MAX15 + "'");
            insertQuery.Append(",'" + request.HARD_MAX16 + "'");
            insertQuery.Append(",'" + request.HARD_MAX17 + "'");
            insertQuery.Append(",'" + request.HARD_MAX18 + "'");
            insertQuery.Append(",'" + request.HARD_MAX19 + "'");
            insertQuery.Append(",'" + request.HARD_MAX20 + "'");
            insertQuery.Append(",'" + request.HARD_MAX21 + "'");
            insertQuery.Append(",'" + request.HARD_MAX22 + "'");
            insertQuery.Append(",'" + request.HARD_MAX23 + "'");
            insertQuery.Append(",'" + request.HARD_MAX24 + "'");
            insertQuery.Append(",'" + request.HARD_MAX25 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL1 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL2 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL3 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL4 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL5 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL6 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL7 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL8 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL9 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL10 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL11 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL12 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL13 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL14 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL15 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL16 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL17 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL18 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL19 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL20 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL21 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL22 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL23 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL24 + "'");
            insertQuery.Append(",'" + request.HARD_ACTUAL25 + "'");
            insertQuery.Append(",'" + request.STATUS + "'");
            insertQuery.Append(",'" + request.STATUS_MARK + "'");
            insertQuery.Append(",'" + request.MESSAGE + "'");
            insertQuery.Append(",SYSDATE");
            insertQuery.Append(") ");
            ExecuteNonQuery(insertQuery.ToString());
            return logCD;
        }
        public int IsDuplicateDataExist(MakerMillSheetDetails request)
        {
            int rowcount = 0;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");

            selectQuery.Append(" GET_DUPLICATE_DATA_COUNT('");
            selectQuery.Append(request.MAKER_NAME);
            selectQuery.Append(request.MATL_NAME_MAKER);
            selectQuery.Append(request.CHARGE);
            // selectQuery.Append( request.KOUBAN );
            selectQuery.Append(request.DIM_D);
            selectQuery.Append(request.LENGTH);
            selectQuery.Append(request.NO_OF_UNITS);
            selectQuery.Append(request.UNIT);
            selectQuery.Append(request.QUANTITY);
            selectQuery.Append(request.HARDNESS_STD);
            selectQuery.Append(request.HARDNESS_MIN);
            selectQuery.Append(request.HARDNESS_MAX);
            selectQuery.Append(request.HARDNESS_ACTUAL);
            selectQuery.Append(request.CCNAME1);
            selectQuery.Append(request.CCRANGE1);
            selectQuery.Append(request.CCMIN1);
            selectQuery.Append(request.CCMAX1);
            selectQuery.Append(request.CCACTUAL1);
            selectQuery.Append(request.CCNAME2);
            selectQuery.Append(request.CCRANGE2);
            selectQuery.Append(request.CCMIN2);
            selectQuery.Append(request.CCMAX2);
            selectQuery.Append(request.CCACTUAL2);
            selectQuery.Append(request.CCNAME3);
            selectQuery.Append(request.CCRANGE3);
            selectQuery.Append(request.CCMIN3);
            selectQuery.Append(request.CCMAX3);
            selectQuery.Append(request.CCACTUAL3);
            selectQuery.Append(request.CCNAME4);
            selectQuery.Append(request.CCRANGE4);
            selectQuery.Append(request.CCMIN4);
            selectQuery.Append(request.CCMAX4);
            selectQuery.Append(request.CCACTUAL4);
            selectQuery.Append(request.CCNAME5);
            selectQuery.Append(request.CCRANGE5);
            selectQuery.Append(request.CCMIN5);
            selectQuery.Append(request.CCMAX5);
            selectQuery.Append(request.CCACTUAL5);
            selectQuery.Append(request.CCNAME6);
            selectQuery.Append(request.CCRANGE6);
            selectQuery.Append(request.CCMIN6);
            selectQuery.Append(request.CCMAX6);
            selectQuery.Append(request.CCACTUAL6);
            selectQuery.Append(request.CCNAME7);
            selectQuery.Append(request.CCRANGE7);
            selectQuery.Append(request.CCMIN7);
            selectQuery.Append(request.CCMAX7);
            selectQuery.Append(request.CCACTUAL7);
            selectQuery.Append(request.CCNAME8);
            selectQuery.Append(request.CCRANGE8);
            selectQuery.Append(request.CCMIN8);
            selectQuery.Append(request.CCMAX8);
            selectQuery.Append(request.CCACTUAL8);
            selectQuery.Append(request.CCNAME9);
            selectQuery.Append(request.CCRANGE9);
            selectQuery.Append(request.CCMIN9);
            selectQuery.Append(request.CCMAX9);
            selectQuery.Append(request.CCACTUAL9);
            selectQuery.Append(request.CCNAME10);
            selectQuery.Append(request.CCRANGE10);
            selectQuery.Append(request.CCMIN10);
            selectQuery.Append(request.CCMAX10);
            selectQuery.Append(request.CCACTUAL10);
            selectQuery.Append(request.CCNAME11);
            selectQuery.Append(request.CCRANGE11);
            selectQuery.Append(request.CCMIN11);
            selectQuery.Append(request.CCMAX11);
            selectQuery.Append(request.CCACTUAL11);
            selectQuery.Append(request.CCNAME12);
            selectQuery.Append(request.CCRANGE12);
            selectQuery.Append(request.CCMIN12);
            selectQuery.Append(request.CCMAX12);
            selectQuery.Append(request.CCACTUAL12);
            selectQuery.Append(request.CCNAME13);
            selectQuery.Append(request.CCRANGE13);
            selectQuery.Append(request.CCMIN13);
            selectQuery.Append(request.CCMAX13);
            selectQuery.Append(request.CCACTUAL13);
            selectQuery.Append(request.CCNAME14);
            selectQuery.Append(request.CCRANGE14);
            selectQuery.Append(request.CCMIN14);
            selectQuery.Append(request.CCMAX14);
            selectQuery.Append(request.CCACTUAL14);
            selectQuery.Append(request.CCNAME15);
            selectQuery.Append(request.CCRANGE15);
            selectQuery.Append(request.CCMIN15);
            selectQuery.Append(request.CCMAX15);
            selectQuery.Append(request.CCACTUAL15);
            selectQuery.Append(request.CCNAME16);
            selectQuery.Append(request.CCRANGE16);
            selectQuery.Append(request.CCMIN16);
            selectQuery.Append(request.CCMAX16);
            selectQuery.Append(request.CCACTUAL16);
            selectQuery.Append(request.CCNAME17);
            selectQuery.Append(request.CCRANGE17);
            selectQuery.Append(request.CCMIN17);
            selectQuery.Append(request.CCMAX17);
            selectQuery.Append(request.CCACTUAL17);
            selectQuery.Append(request.CCNAME18);
            selectQuery.Append(request.CCRANGE18);
            selectQuery.Append(request.CCMIN18);
            selectQuery.Append(request.CCMAX18);
            selectQuery.Append(request.CCACTUAL18);
            selectQuery.Append(request.CCNAME19);
            selectQuery.Append(request.CCRANGE19);
            selectQuery.Append(request.CCMIN19);
            selectQuery.Append(request.CCMAX19);
            selectQuery.Append(request.CCACTUAL19);
            selectQuery.Append(request.CCNAME20);
            selectQuery.Append(request.CCRANGE20);
            selectQuery.Append(request.CCMIN20);
            selectQuery.Append(request.CCMAX20);
            selectQuery.Append(request.CCACTUAL20);
            selectQuery.Append(request.CCNAME21);
            selectQuery.Append(request.CCRANGE21);
            selectQuery.Append(request.CCMIN21);
            selectQuery.Append(request.CCMAX21);
            selectQuery.Append(request.CCACTUAL21);
            selectQuery.Append(request.CCNAME22);
            selectQuery.Append(request.CCRANGE22);
            selectQuery.Append(request.CCMIN22);
            selectQuery.Append(request.CCMAX22);
            selectQuery.Append(request.CCACTUAL22);
            selectQuery.Append(request.CCNAME23);
            selectQuery.Append(request.CCRANGE23);
            selectQuery.Append(request.CCMIN23);
            selectQuery.Append(request.CCMAX23);
            selectQuery.Append(request.CCACTUAL23);
            selectQuery.Append(request.CCNAME24);
            selectQuery.Append(request.CCRANGE24);
            selectQuery.Append(request.CCMIN24);
            selectQuery.Append(request.CCMAX24);
            selectQuery.Append(request.CCACTUAL24);
            selectQuery.Append(request.CCNAME25);
            selectQuery.Append(request.CCRANGE25);
            selectQuery.Append(request.CCMIN25);
            selectQuery.Append(request.CCMAX25);
            selectQuery.Append(request.CCACTUAL25);
            selectQuery.Append(request.HARD_STD1);
            selectQuery.Append(request.HARD_MIN1);
            selectQuery.Append(request.HARD_MAX1);
            selectQuery.Append(request.HARD_ACTUAL1);
            selectQuery.Append(request.HARD_STD2);
            selectQuery.Append(request.HARD_MIN2);
            selectQuery.Append(request.HARD_MAX2);
            selectQuery.Append(request.HARD_ACTUAL2);
            selectQuery.Append(request.HARD_STD3);
            selectQuery.Append(request.HARD_MIN3);
            selectQuery.Append(request.HARD_MAX3);
            selectQuery.Append(request.HARD_ACTUAL3);
            selectQuery.Append(request.HARD_STD4);
            selectQuery.Append(request.HARD_MIN4);
            selectQuery.Append(request.HARD_MAX4);
            selectQuery.Append(request.HARD_ACTUAL4);
            selectQuery.Append(request.HARD_STD5);
            selectQuery.Append(request.HARD_MIN5);
            selectQuery.Append(request.HARD_MAX5);
            selectQuery.Append(request.HARD_ACTUAL5);
            selectQuery.Append(request.HARD_STD6);
            selectQuery.Append(request.HARD_MIN6);
            selectQuery.Append(request.HARD_MAX6);
            selectQuery.Append(request.HARD_ACTUAL6);
            selectQuery.Append(request.HARD_STD7);
            selectQuery.Append(request.HARD_MIN7);
            selectQuery.Append(request.HARD_MAX7);
            selectQuery.Append(request.HARD_ACTUAL7);
            selectQuery.Append(request.HARD_STD8);
            selectQuery.Append(request.HARD_MIN8);
            selectQuery.Append(request.HARD_MAX8);
            selectQuery.Append(request.HARD_ACTUAL8);
            selectQuery.Append(request.HARD_STD9);
            selectQuery.Append(request.HARD_MIN9);
            selectQuery.Append(request.HARD_MAX9);
            selectQuery.Append(request.HARD_ACTUAL9);
            selectQuery.Append(request.HARD_STD10);
            selectQuery.Append(request.HARD_MIN10);
            selectQuery.Append(request.HARD_MAX10);
            selectQuery.Append(request.HARD_ACTUAL10);
            selectQuery.Append(request.HARD_STD11);
            selectQuery.Append(request.HARD_MIN11);
            selectQuery.Append(request.HARD_MAX11);
            selectQuery.Append(request.HARD_ACTUAL11);
            selectQuery.Append(request.HARD_STD12);
            selectQuery.Append(request.HARD_MIN12);
            selectQuery.Append(request.HARD_MAX12);
            selectQuery.Append(request.HARD_ACTUAL12);
            selectQuery.Append(request.HARD_STD13);
            selectQuery.Append(request.HARD_MIN13);
            selectQuery.Append(request.HARD_MAX13);
            selectQuery.Append(request.HARD_ACTUAL13);
            selectQuery.Append(request.HARD_STD14);
            selectQuery.Append(request.HARD_MIN14);
            selectQuery.Append(request.HARD_MAX14);
            selectQuery.Append(request.HARD_ACTUAL14);
            selectQuery.Append(request.HARD_STD15);
            selectQuery.Append(request.HARD_MIN15);
            selectQuery.Append(request.HARD_MAX15);
            selectQuery.Append(request.HARD_ACTUAL15);
            selectQuery.Append(request.HARD_STD16);
            selectQuery.Append(request.HARD_MIN16);
            selectQuery.Append(request.HARD_MAX16);
            selectQuery.Append(request.HARD_ACTUAL16);
            selectQuery.Append(request.HARD_STD17);
            selectQuery.Append(request.HARD_MIN17);
            selectQuery.Append(request.HARD_MAX17);
            selectQuery.Append(request.HARD_ACTUAL17);
            selectQuery.Append(request.HARD_STD18);
            selectQuery.Append(request.HARD_MIN18);
            selectQuery.Append(request.HARD_MAX18);
            selectQuery.Append(request.HARD_ACTUAL18);
            selectQuery.Append(request.HARD_STD19);
            selectQuery.Append(request.HARD_MIN19);
            selectQuery.Append(request.HARD_MAX19);
            selectQuery.Append(request.HARD_ACTUAL19);
            selectQuery.Append(request.HARD_STD20);
            selectQuery.Append(request.HARD_MIN20);
            selectQuery.Append(request.HARD_MAX20);
            selectQuery.Append(request.HARD_ACTUAL20);
            selectQuery.Append(request.HARD_STD21);
            selectQuery.Append(request.HARD_MIN21);
            selectQuery.Append(request.HARD_MAX21);
            selectQuery.Append(request.HARD_ACTUAL21);
            selectQuery.Append(request.HARD_STD22);
            selectQuery.Append(request.HARD_MIN22);
            selectQuery.Append(request.HARD_MAX22);
            selectQuery.Append(request.HARD_ACTUAL22);
            selectQuery.Append(request.HARD_STD23);
            selectQuery.Append(request.HARD_MIN23);
            selectQuery.Append(request.HARD_MAX23);
            selectQuery.Append(request.HARD_ACTUAL23);
            selectQuery.Append(request.HARD_STD24);
            selectQuery.Append(request.HARD_MIN24);
            selectQuery.Append(request.HARD_MAX24);
            selectQuery.Append(request.HARD_ACTUAL24);
            selectQuery.Append(request.HARD_STD25);
            selectQuery.Append(request.HARD_MIN25);
            selectQuery.Append(request.HARD_MAX25);
            selectQuery.Append(request.HARD_ACTUAL25);
            selectQuery.Append(request.GRAIN_SIZE_STD);
            selectQuery.Append(request.GRAIN_SIZE_ACTUAL);
            selectQuery.Append("' ) ");
            selectQuery.Append("  FROM DUAL ");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            rowcount = count;
            return rowcount;
        }
        public int IsDuplicateEDIExist(string charge, decimal dim_d)
        {
            int rowsCount = 0;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" CHARGE = '" + charge + "'");
            selectQuery.Append(" AND DIM_D = '" + dim_d + "'");
            selectQuery.Append(" AND IS_EDI_DATA = 1 ");
            selectQuery.Append(" AND FILENAME IS NOT NULL ");
            selectQuery.Append(" AND CREATED_DATE > ADD_MONTHS(SYSDATE,-8) ");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            rowsCount = count;
            return rowsCount;
        }
        public void UpdateMakerMillsheetLogStatus(int logCD, short status, string status_mark, string error)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE  MAKER_MILL_SHEET_LOG ");
            updateQuery.Append(" SET ");
            updateQuery.Append(" STATUS ='" + status + "'");
            updateQuery.Append(" , STATUS_MARK ='" + status_mark + "'");
            updateQuery.Append(" , MESSAGE ='" + error + "'");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" MAKER_MILL_SHEET_LOG_CD ='" + logCD + "'");
            ExecuteNonQuery(updateQuery.ToString());
        }
        public bool IsFieldAlreadyExist(int fieldCD, string templateName)
        {
            bool fieldExist = false;

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM MAKER_MILL_SHEET_CONFIG ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" FIELD_CD = '" + fieldCD + "'");
            selectQuery.Append(" AND UNIQUE_TEMPLATE_NAME = '" + templateName + "'");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                fieldExist = true;
            }
            return fieldExist;
        }
        public string GetMillSheetPDFPath(long millsheetCD)
        {
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" PDF_FILE_PATH ");
            selectQuery.Append(" FROM MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" MAKER_MILL_SHEET_CD = '" + millsheetCD + "'");

            string pdfPath = Convert.ToString(ExecuteScalar(selectQuery.ToString()));

            return pdfPath;
        }

        public bool IsKoubanAlreadySaved(string kouban, int millsheetCD)
        {
            bool fieldExist = false;

            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" LOWER(KOUBAN) = '" + kouban.ToLower() + "'");
            selectQuery.Append(" AND MAKER_MILL_SHEET_CD != '" + millsheetCD + "'");

            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                fieldExist = true;
            }
            return fieldExist;
        }

        public void UpdateEDIMillsheetData(MakerMillSheetDetails request)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE  MAKER_MILL_SHEET ");
            updateQuery.Append(" SET ");
            updateQuery.Append(" FILENAME ='" + request.FILENAME + "'");
            updateQuery.Append(" , PDF_FILE_PATH ='" + request.PDF_FILE_PATH + "'");
            updateQuery.Append(" , IS_EDI_DATA = 2 ");

            if (request.MAKER_CD > 0) { updateQuery.Append(" ,MAKER_CD  = CASE WHEN MAKER_CD IS NULL THEN " + request.MAKER_CD + " ELSE MAKER_CD END "); }
            if (request.MAKER_NAME != null) { updateQuery.Append(" ,MAKER_NAME = CASE WHEN MAKER_NAME IS NULL THEN '" + request.MAKER_NAME + "' ELSE MAKER_NAME END "); }
            if (request.KOUBAN != null) { updateQuery.Append(" ,KOUBAN = CASE WHEN KOUBAN IS NULL THEN '" + request.KOUBAN + "' ELSE KOUBAN END "); }
            if (request.CHARGE != null) { updateQuery.Append(" ,CHARGE = CASE WHEN CHARGE IS NULL THEN '" + request.CHARGE + "' ELSE CHARGE END "); }
            if (request.DIM_D != null) { updateQuery.Append(" ,DIM_D = CASE WHEN DIM_D IS NULL THEN " + request.DIM_D + " ELSE DIM_D END "); }
            if (request.LENGTH != null) { updateQuery.Append(" ,LENGTH = CASE WHEN LENGTH IS NULL THEN " + request.LENGTH + " ELSE LENGTH END "); }
            if (request.NO_OF_UNITS != null) { updateQuery.Append(" ,NO_OF_UNITS = CASE WHEN NO_OF_UNITS IS NULL THEN " + request.NO_OF_UNITS + " ELSE NO_OF_UNITS END "); }
            if (request.UNIT != null) { updateQuery.Append(" ,UNIT = CASE WHEN UNIT IS NULL THEN " + request.UNIT + " ELSE UNIT END "); }
            if (request.QUANTITY != null) { updateQuery.Append(" ,QUANTITY = CASE WHEN QUANTITY IS NULL THEN " + request.QUANTITY + " ELSE QUANTITY END "); }
            if (request.GRAIN_SIZE_STD != null) { updateQuery.Append(" ,GRAIN_SIZE_STD = CASE WHEN GRAIN_SIZE_STD IS NULL THEN " + request.GRAIN_SIZE_STD + " ELSE GRAIN_SIZE_STD END "); }
            if (request.GRAIN_SIZE_ACTUAL != null) { updateQuery.Append(" ,GRAIN_SIZE_ACTUAL = CASE WHEN GRAIN_SIZE_ACTUAL IS NULL THEN " + request.GRAIN_SIZE_ACTUAL + " ELSE GRAIN_SIZE_ACTUAL END "); }
            if (request.HARDNESS_STD !=null) { updateQuery.Append(" ,HARDNESS_STD = CASE WHEN HARDNESS_STD IS NULL THEN " + request.HARDNESS_STD + " ELSE HARDNESS_STD END "); }
            if (request.HARDNESS_MIN !=null) { updateQuery.Append(" ,HARDNESS_MIN = CASE WHEN HARDNESS_MIN IS NULL THEN " + request.HARDNESS_MIN + " ELSE HARDNESS_MIN END "); }
            if (request.HARDNESS_MAX != null) { updateQuery.Append(" ,HARDNESS_MAX = CASE WHEN HARDNESS_MAX IS NULL THEN " + request.HARDNESS_MAX + " ELSE HARDNESS_MAX END "); }
            if (request.HARDNESS_ACTUAL != null) { updateQuery.Append(" ,HARDNESS_ACTUAL = CASE WHEN HARDNESS_ACTUAL IS NULL THEN " + request.HARDNESS_ACTUAL + " ELSE HARDNESS_ACTUAL END "); }
            if (request.CCNAME1 !=null) { updateQuery.Append(" ,CCNAME1 = CASE WHEN CCNAME1 IS NULL THEN '" + request.CCNAME1 + "' ELSE CCNAME1 END "); }
            if (request.CCNAME2 !=null) { updateQuery.Append(" ,CCNAME2 = CASE WHEN CCNAME2 IS NULL THEN '" + request.CCNAME2 + "' ELSE CCNAME2 END "); }
            if (request.CCNAME3 !=null) { updateQuery.Append(" ,CCNAME3 = CASE WHEN CCNAME3 IS NULL THEN '" + request.CCNAME3 + "' ELSE CCNAME3 END "); }
            if (request.CCNAME4 !=null) { updateQuery.Append(" ,CCNAME4 = CASE WHEN CCNAME4 IS NULL THEN '" + request.CCNAME4 + "' ELSE CCNAME4 END "); }
            if (request.CCNAME5 !=null) { updateQuery.Append(" ,CCNAME5 = CASE WHEN CCNAME5 IS NULL THEN '" + request.CCNAME5 + "' ELSE CCNAME5 END "); }
            if (request.CCNAME6 !=null) { updateQuery.Append(" ,CCNAME6 = CASE WHEN CCNAME6 IS NULL THEN '" + request.CCNAME6 + "' ELSE CCNAME6 END "); }
            if (request.CCNAME7 !=null) { updateQuery.Append(" ,CCNAME7 = CASE WHEN CCNAME7 IS NULL THEN '" + request.CCNAME7 + "' ELSE CCNAME7 END "); }
            if (request.CCNAME8 !=null) { updateQuery.Append(" ,CCNAME8 = CASE WHEN CCNAME8 IS NULL THEN '" + request.CCNAME8 + "' ELSE CCNAME8 END "); }
            if (request.CCNAME9 !=null) { updateQuery.Append(" ,CCNAME9 = CASE WHEN CCNAME9 IS NULL THEN '" + request.CCNAME9 + "' ELSE CCNAME9 END "); }
            if (request.CCNAME10 !=null) { updateQuery.Append(" ,CCNAME10 = CASE WHEN CCNAME10 IS NULL THEN '" + request.CCNAME10 + "' ELSE CCNAME10 END "); }
            if (request.CCNAME11 !=null) { updateQuery.Append(" ,CCNAME11 = CASE WHEN CCNAME11 IS NULL THEN '" + request.CCNAME11 + "' ELSE CCNAME11 END "); }
            if (request.CCNAME12 !=null) { updateQuery.Append(" ,CCNAME12 = CASE WHEN CCNAME12 IS NULL THEN '" + request.CCNAME12 + "' ELSE CCNAME12 END "); }
            if (request.CCNAME13 !=null) { updateQuery.Append(" ,CCNAME13 = CASE WHEN CCNAME13 IS NULL THEN '" + request.CCNAME13 + "' ELSE CCNAME13 END "); }
            if (request.CCNAME14 !=null) { updateQuery.Append(" ,CCNAME14 = CASE WHEN CCNAME14 IS NULL THEN '" + request.CCNAME14 + "' ELSE CCNAME14 END "); }
            if (request.CCNAME15 !=null) { updateQuery.Append(" ,CCNAME15 = CASE WHEN CCNAME15 IS NULL THEN '" + request.CCNAME15 + "' ELSE CCNAME15 END "); }
            if (request.CCNAME16 !=null) { updateQuery.Append(" ,CCNAME16 = CASE WHEN CCNAME16 IS NULL THEN '" + request.CCNAME16 + "' ELSE CCNAME16 END "); }
            if (request.CCNAME17 !=null) { updateQuery.Append(" ,CCNAME17 = CASE WHEN CCNAME17 IS NULL THEN '" + request.CCNAME17 + "' ELSE CCNAME17 END "); }
            if (request.CCNAME18 !=null) { updateQuery.Append(" ,CCNAME18 = CASE WHEN CCNAME18 IS NULL THEN '" + request.CCNAME18 + "' ELSE CCNAME18 END "); }
            if (request.CCNAME19 !=null) { updateQuery.Append(" ,CCNAME19 = CASE WHEN CCNAME19 IS NULL THEN '" + request.CCNAME19 + "' ELSE CCNAME19 END "); }
            if (request.CCNAME20 !=null) { updateQuery.Append(" ,CCNAME20 = CASE WHEN CCNAME20 IS NULL THEN '" + request.CCNAME20 + "' ELSE CCNAME20 END "); }
            if (request.CCNAME21 !=null) { updateQuery.Append(" ,CCNAME21 = CASE WHEN CCNAME21 IS NULL THEN '" + request.CCNAME21 + "' ELSE CCNAME21 END "); }
            if (request.CCNAME22 !=null) { updateQuery.Append(" ,CCNAME22 = CASE WHEN CCNAME22 IS NULL THEN '" + request.CCNAME22 + "' ELSE CCNAME22 END "); }
            if (request.CCNAME23 !=null) { updateQuery.Append(" ,CCNAME23 = CASE WHEN CCNAME23 IS NULL THEN '" + request.CCNAME23 + "' ELSE CCNAME23 END "); }
            if (request.CCNAME24 !=null) { updateQuery.Append(" ,CCNAME24 = CASE WHEN CCNAME24 IS NULL THEN '" + request.CCNAME24 + "' ELSE CCNAME24 END "); }
            if (request.CCNAME25 !=null) { updateQuery.Append(" ,CCNAME25 = CASE WHEN CCNAME25 IS NULL THEN '" + request.CCNAME25 + "' ELSE CCNAME25 END "); }
            if (request.CCRANGE1 !=null) { updateQuery.Append(" ,CCRANGE1 = CASE WHEN CCRANGE1 IS NULL THEN " + request.CCRANGE1 + " ELSE CCRANGE1 END "); }
            if (request.CCRANGE2 !=null) { updateQuery.Append(" ,CCRANGE2 = CASE WHEN CCRANGE2 IS NULL THEN " + request.CCRANGE2 + " ELSE CCRANGE2 END "); }
            if (request.CCRANGE3 !=null) { updateQuery.Append(" ,CCRANGE3 = CASE WHEN CCRANGE3 IS NULL THEN " + request.CCRANGE3 + " ELSE CCRANGE3 END "); }
            if (request.CCRANGE4 !=null) { updateQuery.Append(" ,CCRANGE4 = CASE WHEN CCRANGE4 IS NULL THEN " + request.CCRANGE4 + " ELSE CCRANGE4 END "); }
            if (request.CCRANGE5 !=null) { updateQuery.Append(" ,CCRANGE5 = CASE WHEN CCRANGE5 IS NULL THEN " + request.CCRANGE5 + " ELSE CCRANGE5 END "); }
            if (request.CCRANGE6 !=null) { updateQuery.Append(" ,CCRANGE6 = CASE WHEN CCRANGE6 IS NULL THEN " + request.CCRANGE6 + " ELSE CCRANGE6 END "); }
            if (request.CCRANGE7 !=null) { updateQuery.Append(" ,CCRANGE7 = CASE WHEN CCRANGE7 IS NULL THEN " + request.CCRANGE7 + " ELSE CCRANGE7 END "); }
            if (request.CCRANGE8 !=null) { updateQuery.Append(" ,CCRANGE8 = CASE WHEN CCRANGE8 IS NULL THEN " + request.CCRANGE8 + " ELSE CCRANGE8 END "); }
            if (request.CCRANGE9 != null) { updateQuery.Append(" ,CCRANGE9 = CASE WHEN CCRANGE9 IS NULL THEN " + request.CCRANGE9 + " ELSE CCRANGE9 END "); }
            if (request.CCRANGE10 !=null) { updateQuery.Append(" ,CCRANGE10 = CASE WHEN CCRANGE10 IS NULL THEN " + request.CCRANGE10 + " ELSE CCRANGE10 END "); }
            if (request.CCRANGE11 !=null) { updateQuery.Append(" ,CCRANGE11 = CASE WHEN CCRANGE11 IS NULL THEN " + request.CCRANGE11 + " ELSE CCRANGE11 END "); }
            if (request.CCRANGE12 !=null) { updateQuery.Append(" ,CCRANGE12 = CASE WHEN CCRANGE12 IS NULL THEN " + request.CCRANGE12 + " ELSE CCRANGE12 END "); }
            if (request.CCRANGE13 !=null) { updateQuery.Append(" ,CCRANGE13 = CASE WHEN CCRANGE13 IS NULL THEN " + request.CCRANGE13 + " ELSE CCRANGE13 END "); }
            if (request.CCRANGE14 !=null) { updateQuery.Append(" ,CCRANGE14 = CASE WHEN CCRANGE14 IS NULL THEN " + request.CCRANGE14 + " ELSE CCRANGE14 END "); }
            if (request.CCRANGE15 !=null) { updateQuery.Append(" ,CCRANGE15 = CASE WHEN CCRANGE15 IS NULL THEN " + request.CCRANGE15 + " ELSE CCRANGE15 END "); }
            if (request.CCRANGE16 !=null) { updateQuery.Append(" ,CCRANGE16 = CASE WHEN CCRANGE16 IS NULL THEN " + request.CCRANGE16 + " ELSE CCRANGE16 END "); }
            if (request.CCRANGE17 !=null) { updateQuery.Append(" ,CCRANGE17 = CASE WHEN CCRANGE17 IS NULL THEN " + request.CCRANGE17 + " ELSE CCRANGE17 END "); }
            if (request.CCRANGE18 !=null) { updateQuery.Append(" ,CCRANGE18 = CASE WHEN CCRANGE18 IS NULL THEN " + request.CCRANGE18 + " ELSE CCRANGE18 END "); }
            if (request.CCRANGE19 !=null) { updateQuery.Append(" ,CCRANGE19 = CASE WHEN CCRANGE19 IS NULL THEN " + request.CCRANGE19 + " ELSE CCRANGE19 END "); }
            if (request.CCRANGE20 !=null) { updateQuery.Append(" ,CCRANGE20 = CASE WHEN CCRANGE20 IS NULL THEN " + request.CCRANGE20 + " ELSE CCRANGE20 END "); }
            if (request.CCRANGE21 !=null) { updateQuery.Append(" ,CCRANGE21 = CASE WHEN CCRANGE21 IS NULL THEN " + request.CCRANGE21 + " ELSE CCRANGE21 END "); }
            if (request.CCRANGE22 !=null) { updateQuery.Append(" ,CCRANGE22 = CASE WHEN CCRANGE22 IS NULL THEN " + request.CCRANGE22 + " ELSE CCRANGE22 END "); }
            if (request.CCRANGE23 !=null) { updateQuery.Append(" ,CCRANGE23 = CASE WHEN CCRANGE23 IS NULL THEN " + request.CCRANGE23 + " ELSE CCRANGE23 END "); }
            if (request.CCRANGE24 !=null) { updateQuery.Append(" ,CCRANGE24 = CASE WHEN CCRANGE24 IS NULL THEN " + request.CCRANGE24 + " ELSE CCRANGE24 END "); }
            if (request.CCRANGE25 !=null) { updateQuery.Append(" ,CCRANGE25 = CASE WHEN CCRANGE25 IS NULL THEN " + request.CCRANGE25 + " ELSE CCRANGE25 END "); }
            if (request.CCMIN1 != null) { updateQuery.Append(" ,CCMIN1 = CASE WHEN CCMIN1 IS NULL THEN " + request.CCMIN1 + " ELSE CCMIN1 END "); }
            if (request.CCMIN2 !=null) { updateQuery.Append(" ,CCMIN2 = CASE WHEN CCMIN2 IS NULL THEN " + request.CCMIN2 + " ELSE CCMIN2 END "); }
            if (request.CCMIN3 !=null) { updateQuery.Append(" ,CCMIN3 = CASE WHEN CCMIN3 IS NULL THEN " + request.CCMIN3 + " ELSE CCMIN3 END "); }
            if (request.CCMIN4 !=null) { updateQuery.Append(" ,CCMIN4 = CASE WHEN CCMIN4 IS NULL THEN " + request.CCMIN4 + " ELSE CCMIN4 END "); }
            if (request.CCMIN5 !=null) { updateQuery.Append(" ,CCMIN5 = CASE WHEN CCMIN5 IS NULL THEN " + request.CCMIN5 + " ELSE CCMIN5 END "); }
            if (request.CCMIN6 !=null) { updateQuery.Append(" ,CCMIN6 = CASE WHEN CCMIN6 IS NULL THEN " + request.CCMIN6 + " ELSE CCMIN6 END "); }
            if (request.CCMIN7 !=null) { updateQuery.Append(" ,CCMIN7 = CASE WHEN CCMIN7 IS NULL THEN " + request.CCMIN7 + " ELSE CCMIN7 END "); }
            if (request.CCMIN8 !=null) { updateQuery.Append(" ,CCMIN8 = CASE WHEN CCMIN8 IS NULL THEN " + request.CCMIN8 + " ELSE CCMIN8 END "); }
            if (request.CCMIN9 != null) { updateQuery.Append(" ,CCMIN9 = CASE WHEN CCMIN9 IS NULL THEN " + request.CCMIN9 + " ELSE CCMIN9 END "); }
            if (request.CCMIN10 !=null) { updateQuery.Append(" ,CCMIN10 = CASE WHEN CCMIN10 IS NULL THEN " + request.CCMIN10 + " ELSE CCMIN10 END "); }
            if (request.CCMIN11 !=null) { updateQuery.Append(" ,CCMIN11 = CASE WHEN CCMIN11 IS NULL THEN " + request.CCMIN11 + " ELSE CCMIN11 END "); }
            if (request.CCMIN12 !=null) { updateQuery.Append(" ,CCMIN12 = CASE WHEN CCMIN12 IS NULL THEN " + request.CCMIN12 + " ELSE CCMIN12 END "); }
            if (request.CCMIN13 !=null) { updateQuery.Append(" ,CCMIN13 = CASE WHEN CCMIN13 IS NULL THEN " + request.CCMIN13 + " ELSE CCMIN13 END "); }
            if (request.CCMIN14 !=null) { updateQuery.Append(" ,CCMIN14 = CASE WHEN CCMIN14 IS NULL THEN " + request.CCMIN14 + " ELSE CCMIN14 END "); }
            if (request.CCMIN15 !=null) { updateQuery.Append(" ,CCMIN15 = CASE WHEN CCMIN15 IS NULL THEN " + request.CCMIN15 + " ELSE CCMIN15 END "); }
            if (request.CCMIN16 !=null) { updateQuery.Append(" ,CCMIN16 = CASE WHEN CCMIN16 IS NULL THEN " + request.CCMIN16 + " ELSE CCMIN16 END "); }
            if (request.CCMIN17 !=null) { updateQuery.Append(" ,CCMIN17 = CASE WHEN CCMIN17 IS NULL THEN " + request.CCMIN17 + " ELSE CCMIN17 END "); }
            if (request.CCMIN18 !=null) { updateQuery.Append(" ,CCMIN18 = CASE WHEN CCMIN18 IS NULL THEN " + request.CCMIN18 + " ELSE CCMIN18 END "); }
            if (request.CCMIN19 !=null) { updateQuery.Append(" ,CCMIN19 = CASE WHEN CCMIN19 IS NULL THEN " + request.CCMIN19 + " ELSE CCMIN19 END "); }
            if (request.CCMIN20 !=null) { updateQuery.Append(" ,CCMIN20 = CASE WHEN CCMIN20 IS NULL THEN " + request.CCMIN20 + " ELSE CCMIN20 END "); }
            if (request.CCMIN21 !=null) { updateQuery.Append(" ,CCMIN21 = CASE WHEN CCMIN21 IS NULL THEN " + request.CCMIN21 + " ELSE CCMIN21 END "); }
            if (request.CCMIN22 !=null) { updateQuery.Append(" ,CCMIN22 = CASE WHEN CCMIN22 IS NULL THEN " + request.CCMIN22 + " ELSE CCMIN22 END "); }
            if (request.CCMIN23 !=null) { updateQuery.Append(" ,CCMIN23 = CASE WHEN CCMIN23 IS NULL THEN " + request.CCMIN23 + " ELSE CCMIN23 END "); }
            if (request.CCMIN24 !=null) { updateQuery.Append(" ,CCMIN24 = CASE WHEN CCMIN24 IS NULL THEN " + request.CCMIN24 + " ELSE CCMIN24 END "); }
            if (request.CCMIN25 != null) { updateQuery.Append(" ,CCMIN25 = CASE WHEN CCMIN25 IS NULL THEN " + request.CCMIN25 + " ELSE CCMIN25 END "); }
            if (request.CCMAX1 !=null) { updateQuery.Append(" ,CCMAX1 = CASE WHEN CCMAX1 IS NULL THEN " + request.CCMAX1 + " ELSE CCMAX1 END "); }
            if (request.CCMAX2 !=null) { updateQuery.Append(" ,CCMAX2 = CASE WHEN CCMAX2 IS NULL THEN " + request.CCMAX2 + " ELSE CCMAX2 END "); }
            if (request.CCMAX3 !=null) { updateQuery.Append(" ,CCMAX3 = CASE WHEN CCMAX3 IS NULL THEN " + request.CCMAX3 + " ELSE CCMAX3 END "); }
            if (request.CCMAX4 !=null) { updateQuery.Append(" ,CCMAX4 = CASE WHEN CCMAX4 IS NULL THEN " + request.CCMAX4 + " ELSE CCMAX4 END "); }
            if (request.CCMAX5 !=null) { updateQuery.Append(" ,CCMAX5 = CASE WHEN CCMAX5 IS NULL THEN " + request.CCMAX5 + " ELSE CCMAX5 END "); }
            if (request.CCMAX6 !=null) { updateQuery.Append(" ,CCMAX6 = CASE WHEN CCMAX6 IS NULL THEN " + request.CCMAX6 + " ELSE CCMAX6 END "); }
            if (request.CCMAX7 !=null) { updateQuery.Append(" ,CCMAX7 = CASE WHEN CCMAX7 IS NULL THEN " + request.CCMAX7 + " ELSE CCMAX7 END "); }
            if (request.CCMAX8 !=null) { updateQuery.Append(" ,CCMAX8 = CASE WHEN CCMAX8 IS NULL THEN " + request.CCMAX8 + " ELSE CCMAX8 END "); }
            if (request.CCMAX9 != null) { updateQuery.Append(" ,CCMAX9 = CASE WHEN CCMAX9 IS NULL THEN " + request.CCMAX9 + " ELSE CCMAX9 END "); }
            if (request.CCMAX10 !=null) { updateQuery.Append(" ,CCMAX10 = CASE WHEN CCMAX10 IS NULL THEN " + request.CCMAX10 + " ELSE CCMAX10 END "); }
            if (request.CCMAX11 !=null) { updateQuery.Append(" ,CCMAX11 = CASE WHEN CCMAX11 IS NULL THEN " + request.CCMAX11 + " ELSE CCMAX11 END "); }
            if (request.CCMAX12 !=null) { updateQuery.Append(" ,CCMAX12 = CASE WHEN CCMAX12 IS NULL THEN " + request.CCMAX12 + " ELSE CCMAX12 END "); }
            if (request.CCMAX13 !=null) { updateQuery.Append(" ,CCMAX13 = CASE WHEN CCMAX13 IS NULL THEN " + request.CCMAX13 + " ELSE CCMAX13 END "); }
            if (request.CCMAX14 !=null) { updateQuery.Append(" ,CCMAX14 = CASE WHEN CCMAX14 IS NULL THEN " + request.CCMAX14 + " ELSE CCMAX14 END "); }
            if (request.CCMAX15 !=null) { updateQuery.Append(" ,CCMAX15 = CASE WHEN CCMAX15 IS NULL THEN " + request.CCMAX15 + " ELSE CCMAX15 END "); }
            if (request.CCMAX16 !=null) { updateQuery.Append(" ,CCMAX16 = CASE WHEN CCMAX16 IS NULL THEN " + request.CCMAX16 + " ELSE CCMAX16 END "); }
            if (request.CCMAX17 !=null) { updateQuery.Append(" ,CCMAX17 = CASE WHEN CCMAX17 IS NULL THEN " + request.CCMAX17 + " ELSE CCMAX17 END "); }
            if (request.CCMAX18 !=null) { updateQuery.Append(" ,CCMAX18 = CASE WHEN CCMAX18 IS NULL THEN " + request.CCMAX18 + " ELSE CCMAX18 END "); }
            if (request.CCMAX19 !=null) { updateQuery.Append(" ,CCMAX19 = CASE WHEN CCMAX19 IS NULL THEN " + request.CCMAX19 + " ELSE CCMAX19 END "); }
            if (request.CCMAX20 !=null) { updateQuery.Append(" ,CCMAX20 = CASE WHEN CCMAX20 IS NULL THEN " + request.CCMAX20 + " ELSE CCMAX20 END "); }
            if (request.CCMAX21 !=null) { updateQuery.Append(" ,CCMAX21 = CASE WHEN CCMAX21 IS NULL THEN " + request.CCMAX21 + " ELSE CCMAX21 END "); }
            if (request.CCMAX22 !=null) { updateQuery.Append(" ,CCMAX22 = CASE WHEN CCMAX22 IS NULL THEN " + request.CCMAX22 + " ELSE CCMAX22 END "); }
            if (request.CCMAX23 !=null) { updateQuery.Append(" ,CCMAX23 = CASE WHEN CCMAX23 IS NULL THEN " + request.CCMAX23 + " ELSE CCMAX23 END "); }
            if (request.CCMAX24 !=null) { updateQuery.Append(" ,CCMAX24 = CASE WHEN CCMAX24 IS NULL THEN " + request.CCMAX24 + " ELSE CCMAX24 END "); }
            if (request.CCMAX25 != null) { updateQuery.Append(" ,CCMAX25 = CASE WHEN CCMAX25 IS NULL THEN " + request.CCMAX25 + " ELSE CCMAX25 END "); }
            if (request.CCACTUAL1 !=null) { updateQuery.Append(" ,CCACTUAL1 = CASE WHEN CCACTUAL1 IS NULL THEN '" + request.CCACTUAL1 + "' ELSE CCACTUAL1 END "); }
            if (request.CCACTUAL2 !=null) { updateQuery.Append(" ,CCACTUAL2 = CASE WHEN CCACTUAL2 IS NULL THEN '" + request.CCACTUAL2 + "' ELSE CCACTUAL2 END "); }
            if (request.CCACTUAL3 !=null) { updateQuery.Append(" ,CCACTUAL3 = CASE WHEN CCACTUAL3 IS NULL THEN '" + request.CCACTUAL3 + "' ELSE CCACTUAL3 END "); }
            if (request.CCACTUAL4 !=null) { updateQuery.Append(" ,CCACTUAL4 = CASE WHEN CCACTUAL4 IS NULL THEN '" + request.CCACTUAL4 + "' ELSE CCACTUAL4 END "); }
            if (request.CCACTUAL5 !=null) { updateQuery.Append(" ,CCACTUAL5 = CASE WHEN CCACTUAL5 IS NULL THEN '" + request.CCACTUAL5 + "' ELSE CCACTUAL5 END "); }
            if (request.CCACTUAL6 !=null) { updateQuery.Append(" ,CCACTUAL6 = CASE WHEN CCACTUAL6 IS NULL THEN '" + request.CCACTUAL6 + "' ELSE CCACTUAL6 END "); }
            if (request.CCACTUAL7 !=null) { updateQuery.Append(" ,CCACTUAL7 = CASE WHEN CCACTUAL7 IS NULL THEN '" + request.CCACTUAL7 + "' ELSE CCACTUAL7 END "); }
            if (request.CCACTUAL8 !=null) { updateQuery.Append(" ,CCACTUAL8 = CASE WHEN CCACTUAL8 IS NULL THEN '" + request.CCACTUAL8 + "' ELSE CCACTUAL8 END "); }
            if (request.CCACTUAL9 != null) { updateQuery.Append(" ,CCACTUAL9 = CASE WHEN CCACTUAL9 IS NULL THEN '" + request.CCACTUAL9 + "' ELSE CCACTUAL9 END "); }
            if (request.CCACTUAL10 !=null) { updateQuery.Append(" ,CCACTUAL10 = CASE WHEN CCACTUAL10 IS NULL THEN '" + request.CCACTUAL10 + "' ELSE CCACTUAL10 END "); }
            if (request.CCACTUAL11 !=null) { updateQuery.Append(" ,CCACTUAL11 = CASE WHEN CCACTUAL11 IS NULL THEN '" + request.CCACTUAL11 + "' ELSE CCACTUAL11 END "); }
            if (request.CCACTUAL12 !=null) { updateQuery.Append(" ,CCACTUAL12 = CASE WHEN CCACTUAL12 IS NULL THEN '" + request.CCACTUAL12 + "' ELSE CCACTUAL12 END "); }
            if (request.CCACTUAL13 !=null) { updateQuery.Append(" ,CCACTUAL13 = CASE WHEN CCACTUAL13 IS NULL THEN '" + request.CCACTUAL13 + "' ELSE CCACTUAL13 END "); }
            if (request.CCACTUAL14 !=null) { updateQuery.Append(" ,CCACTUAL14 = CASE WHEN CCACTUAL14 IS NULL THEN '" + request.CCACTUAL14 + "' ELSE CCACTUAL14 END "); }
            if (request.CCACTUAL15 !=null) { updateQuery.Append(" ,CCACTUAL15 = CASE WHEN CCACTUAL15 IS NULL THEN '" + request.CCACTUAL15 + "' ELSE CCACTUAL15 END "); }
            if (request.CCACTUAL16 !=null) { updateQuery.Append(" ,CCACTUAL16 = CASE WHEN CCACTUAL16 IS NULL THEN '" + request.CCACTUAL16 + "' ELSE CCACTUAL16 END "); }
            if (request.CCACTUAL17 !=null) { updateQuery.Append(" ,CCACTUAL17 = CASE WHEN CCACTUAL17 IS NULL THEN '" + request.CCACTUAL17 + "' ELSE CCACTUAL17 END "); }
            if (request.CCACTUAL18 !=null) { updateQuery.Append(" ,CCACTUAL18 = CASE WHEN CCACTUAL18 IS NULL THEN '" + request.CCACTUAL18 + "' ELSE CCACTUAL18 END "); }
            if (request.CCACTUAL19 !=null) { updateQuery.Append(" ,CCACTUAL19 = CASE WHEN CCACTUAL19 IS NULL THEN '" + request.CCACTUAL19 + "' ELSE CCACTUAL19 END "); }
            if (request.CCACTUAL20 !=null) { updateQuery.Append(" ,CCACTUAL20 = CASE WHEN CCACTUAL20 IS NULL THEN '" + request.CCACTUAL20 + "' ELSE CCACTUAL20 END "); }
            if (request.CCACTUAL21 !=null) { updateQuery.Append(" ,CCACTUAL21 = CASE WHEN CCACTUAL21 IS NULL THEN '" + request.CCACTUAL21 + "' ELSE CCACTUAL21 END "); }
            if (request.CCACTUAL22 !=null) { updateQuery.Append(" ,CCACTUAL22 = CASE WHEN CCACTUAL22 IS NULL THEN '" + request.CCACTUAL22 + "' ELSE CCACTUAL22 END "); }
            if (request.CCACTUAL23 !=null) { updateQuery.Append(" ,CCACTUAL23 = CASE WHEN CCACTUAL23 IS NULL THEN '" + request.CCACTUAL23 + "' ELSE CCACTUAL23 END "); }
            if (request.CCACTUAL24 !=null) { updateQuery.Append(" ,CCACTUAL24 = CASE WHEN CCACTUAL24 IS NULL THEN '" + request.CCACTUAL24 + "' ELSE CCACTUAL24 END "); }
            if (request.CCACTUAL25 != null) { updateQuery.Append(" ,CCACTUAL25 = CASE WHEN CCACTUAL25 IS NULL THEN '" + request.CCACTUAL25 + "' ELSE CCACTUAL25 END "); }
            if (request.HARD_STD1 != null) { updateQuery.Append(" ,HARD_STD1 = CASE WHEN HARD_STD1 IS NULL THEN " + request.HARD_STD1 + " ELSE HARD_STD1 END "); }
            if (request.HARD_STD2 != null) { updateQuery.Append(" ,HARD_STD2 = CASE WHEN HARD_STD2 IS NULL THEN " + request.HARD_STD2 + " ELSE HARD_STD2 END "); }
            if (request.HARD_STD3 != null) { updateQuery.Append(" ,HARD_STD3 = CASE WHEN HARD_STD3 IS NULL THEN " + request.HARD_STD3 + " ELSE HARD_STD3 END "); }
            if (request.HARD_STD4 != null) { updateQuery.Append(" ,HARD_STD4 = CASE WHEN HARD_STD4 IS NULL THEN " + request.HARD_STD4 + " ELSE HARD_STD4 END "); }
            if (request.HARD_STD5 != null) { updateQuery.Append(" ,HARD_STD5 = CASE WHEN HARD_STD5 IS NULL THEN " + request.HARD_STD5 + " ELSE HARD_STD5 END "); }
            if (request.HARD_STD6 != null) { updateQuery.Append(" ,HARD_STD6 = CASE WHEN HARD_STD6 IS NULL THEN " + request.HARD_STD6 + " ELSE HARD_STD6 END "); }
            if (request.HARD_STD7 != null) { updateQuery.Append(" ,HARD_STD7 = CASE WHEN HARD_STD7 IS NULL THEN " + request.HARD_STD7 + " ELSE HARD_STD7 END "); }
            if (request.HARD_STD8 != null) { updateQuery.Append(" ,HARD_STD8 = CASE WHEN HARD_STD8 IS NULL THEN " + request.HARD_STD8 + " ELSE HARD_STD8 END "); }
            if (request.HARD_STD9 != null) { updateQuery.Append(" ,HARD_STD9 = CASE WHEN HARD_STD9 IS NULL THEN " + request.HARD_STD9 + " ELSE HARD_STD9 END "); }
            if (request.HARD_STD10 !=null) { updateQuery.Append(" ,HARD_STD10 = CASE WHEN HARD_STD10 IS NULL THEN " + request.HARD_STD10 + " ELSE HARD_STD10 END "); }
            if (request.HARD_STD11 !=null) { updateQuery.Append(" ,HARD_STD11 = CASE WHEN HARD_STD11 IS NULL THEN " + request.HARD_STD11 + " ELSE HARD_STD11 END "); }
            if (request.HARD_STD12 !=null) { updateQuery.Append(" ,HARD_STD12 = CASE WHEN HARD_STD12 IS NULL THEN " + request.HARD_STD12 + " ELSE HARD_STD12 END "); }
            if (request.HARD_STD13 !=null) { updateQuery.Append(" ,HARD_STD13 = CASE WHEN HARD_STD13 IS NULL THEN " + request.HARD_STD13 + " ELSE HARD_STD13 END "); }
            if (request.HARD_STD14 !=null) { updateQuery.Append(" ,HARD_STD14 = CASE WHEN HARD_STD14 IS NULL THEN " + request.HARD_STD14 + " ELSE HARD_STD14 END "); }
            if (request.HARD_STD15 !=null) { updateQuery.Append(" ,HARD_STD15 = CASE WHEN HARD_STD15 IS NULL THEN " + request.HARD_STD15 + " ELSE HARD_STD15 END "); }
            if (request.HARD_STD16 !=null) { updateQuery.Append(" ,HARD_STD16 = CASE WHEN HARD_STD16 IS NULL THEN " + request.HARD_STD16 + " ELSE HARD_STD16 END "); }
            if (request.HARD_STD17 !=null) { updateQuery.Append(" ,HARD_STD17 = CASE WHEN HARD_STD17 IS NULL THEN " + request.HARD_STD17 + " ELSE HARD_STD17 END "); }
            if (request.HARD_STD18 !=null) { updateQuery.Append(" ,HARD_STD18 = CASE WHEN HARD_STD18 IS NULL THEN " + request.HARD_STD18 + " ELSE HARD_STD18 END "); }
            if (request.HARD_STD19 !=null) { updateQuery.Append(" ,HARD_STD19 = CASE WHEN HARD_STD19 IS NULL THEN " + request.HARD_STD19 + " ELSE HARD_STD19 END "); }
            if (request.HARD_STD20 !=null) { updateQuery.Append(" ,HARD_STD20 = CASE WHEN HARD_STD20 IS NULL THEN " + request.HARD_STD20 + " ELSE HARD_STD20 END "); }
            if (request.HARD_STD21 !=null) { updateQuery.Append(" ,HARD_STD21 = CASE WHEN HARD_STD21 IS NULL THEN " + request.HARD_STD21 + " ELSE HARD_STD21 END "); }
            if (request.HARD_STD22 !=null) { updateQuery.Append(" ,HARD_STD22 = CASE WHEN HARD_STD22 IS NULL THEN " + request.HARD_STD22 + " ELSE HARD_STD22 END "); }
            if (request.HARD_STD23 !=null) { updateQuery.Append(" ,HARD_STD23 = CASE WHEN HARD_STD23 IS NULL THEN " + request.HARD_STD23 + " ELSE HARD_STD23 END "); }
            if (request.HARD_STD24 !=null) { updateQuery.Append(" ,HARD_STD24 = CASE WHEN HARD_STD24 IS NULL THEN " + request.HARD_STD24 + " ELSE HARD_STD24 END "); }
            if (request.HARD_STD25 != null) { updateQuery.Append(" ,HARD_STD25 = CASE WHEN HARD_STD25 IS NULL THEN " + request.HARD_STD25 + " ELSE HARD_STD25 END "); }
            if (request.HARD_MIN1 !=null) { updateQuery.Append(" ,HARD_MIN1 = CASE WHEN HARD_MIN1 IS NULL THEN " + request.HARD_MIN1 + " ELSE HARD_MIN1 END "); }
            if (request.HARD_MIN2 !=null) { updateQuery.Append(" ,HARD_MIN2 = CASE WHEN HARD_MIN2 IS NULL THEN " + request.HARD_MIN2 + " ELSE HARD_MIN2 END "); }
            if (request.HARD_MIN3 !=null) { updateQuery.Append(" ,HARD_MIN3 = CASE WHEN HARD_MIN3 IS NULL THEN " + request.HARD_MIN3 + " ELSE HARD_MIN3 END "); }
            if (request.HARD_MIN4 !=null) { updateQuery.Append(" ,HARD_MIN4 = CASE WHEN HARD_MIN4 IS NULL THEN " + request.HARD_MIN4 + " ELSE HARD_MIN4 END "); }
            if (request.HARD_MIN5 !=null) { updateQuery.Append(" ,HARD_MIN5 = CASE WHEN HARD_MIN5 IS NULL THEN " + request.HARD_MIN5 + " ELSE HARD_MIN5 END "); }
            if (request.HARD_MIN6 !=null) { updateQuery.Append(" ,HARD_MIN6 = CASE WHEN HARD_MIN6 IS NULL THEN " + request.HARD_MIN6 + " ELSE HARD_MIN6 END "); }
            if (request.HARD_MIN7 !=null) { updateQuery.Append(" ,HARD_MIN7 = CASE WHEN HARD_MIN7 IS NULL THEN " + request.HARD_MIN7 + " ELSE HARD_MIN7 END "); }
            if (request.HARD_MIN8 !=null) { updateQuery.Append(" ,HARD_MIN8 = CASE WHEN HARD_MIN8 IS NULL THEN " + request.HARD_MIN8 + " ELSE HARD_MIN8 END "); }
            if (request.HARD_MIN9 != null) { updateQuery.Append(" ,HARD_MIN9 = CASE WHEN HARD_MIN9 IS NULL THEN " + request.HARD_MIN9 + " ELSE HARD_MIN9 END "); }
            if (request.HARD_MIN10 !=null) { updateQuery.Append(" ,HARD_MIN10 = CASE WHEN HARD_MIN10 IS NULL THEN " + request.HARD_MIN10 + " ELSE HARD_MIN10 END "); }
            if (request.HARD_MIN11 !=null) { updateQuery.Append(" ,HARD_MIN11 = CASE WHEN HARD_MIN11 IS NULL THEN " + request.HARD_MIN11 + " ELSE HARD_MIN11 END "); }
            if (request.HARD_MIN12 !=null) { updateQuery.Append(" ,HARD_MIN12 = CASE WHEN HARD_MIN12 IS NULL THEN " + request.HARD_MIN12 + " ELSE HARD_MIN12 END "); }
            if (request.HARD_MIN13 !=null) { updateQuery.Append(" ,HARD_MIN13 = CASE WHEN HARD_MIN13 IS NULL THEN " + request.HARD_MIN13 + " ELSE HARD_MIN13 END "); }
            if (request.HARD_MIN14 !=null) { updateQuery.Append(" ,HARD_MIN14 = CASE WHEN HARD_MIN14 IS NULL THEN " + request.HARD_MIN14 + " ELSE HARD_MIN14 END "); }
            if (request.HARD_MIN15 !=null) { updateQuery.Append(" ,HARD_MIN15 = CASE WHEN HARD_MIN15 IS NULL THEN " + request.HARD_MIN15 + " ELSE HARD_MIN15 END "); }
            if (request.HARD_MIN16 !=null) { updateQuery.Append(" ,HARD_MIN16 = CASE WHEN HARD_MIN16 IS NULL THEN " + request.HARD_MIN16 + " ELSE HARD_MIN16 END "); }
            if (request.HARD_MIN17 !=null) { updateQuery.Append(" ,HARD_MIN17 = CASE WHEN HARD_MIN17 IS NULL THEN " + request.HARD_MIN17 + " ELSE HARD_MIN17 END "); }
            if (request.HARD_MIN18 !=null) { updateQuery.Append(" ,HARD_MIN18 = CASE WHEN HARD_MIN18 IS NULL THEN " + request.HARD_MIN18 + " ELSE HARD_MIN18 END "); }
            if (request.HARD_MIN19 !=null) { updateQuery.Append(" ,HARD_MIN19 = CASE WHEN HARD_MIN19 IS NULL THEN " + request.HARD_MIN19 + " ELSE HARD_MIN19 END "); }
            if (request.HARD_MIN20 !=null) { updateQuery.Append(" ,HARD_MIN20 = CASE WHEN HARD_MIN20 IS NULL THEN " + request.HARD_MIN20 + " ELSE HARD_MIN20 END "); }
            if (request.HARD_MIN21 !=null) { updateQuery.Append(" ,HARD_MIN21 = CASE WHEN HARD_MIN21 IS NULL THEN " + request.HARD_MIN21 + " ELSE HARD_MIN21 END "); }
            if (request.HARD_MIN22 !=null) { updateQuery.Append(" ,HARD_MIN22 = CASE WHEN HARD_MIN22 IS NULL THEN " + request.HARD_MIN22 + " ELSE HARD_MIN22 END "); }
            if (request.HARD_MIN23 !=null) { updateQuery.Append(" ,HARD_MIN23 = CASE WHEN HARD_MIN23 IS NULL THEN " + request.HARD_MIN23 + " ELSE HARD_MIN23 END "); }
            if (request.HARD_MIN24 !=null) { updateQuery.Append(" ,HARD_MIN24 = CASE WHEN HARD_MIN24 IS NULL THEN " + request.HARD_MIN24 + " ELSE HARD_MIN24 END "); }
            if (request.HARD_MIN25 != null) { updateQuery.Append(" ,HARD_MIN25 = CASE WHEN HARD_MIN25 IS NULL THEN " + request.HARD_MIN25 + " ELSE HARD_MIN25 END "); }
            if (request.HARD_MAX1 !=null) { updateQuery.Append(" ,HARD_MAX1 = CASE WHEN HARD_MAX1 IS NULL THEN " + request.HARD_MAX1 + " ELSE HARD_MAX1 END "); }
            if (request.HARD_MAX2 !=null) { updateQuery.Append(" ,HARD_MAX2 = CASE WHEN HARD_MAX2 IS NULL THEN " + request.HARD_MAX2 + " ELSE HARD_MAX2 END "); }
            if (request.HARD_MAX3 !=null) { updateQuery.Append(" ,HARD_MAX3 = CASE WHEN HARD_MAX3 IS NULL THEN " + request.HARD_MAX3 + " ELSE HARD_MAX3 END "); }
            if (request.HARD_MAX4 !=null) { updateQuery.Append(" ,HARD_MAX4 = CASE WHEN HARD_MAX4 IS NULL THEN " + request.HARD_MAX4 + " ELSE HARD_MAX4 END "); }
            if (request.HARD_MAX5 !=null) { updateQuery.Append(" ,HARD_MAX5 = CASE WHEN HARD_MAX5 IS NULL THEN " + request.HARD_MAX5 + " ELSE HARD_MAX5 END "); }
            if (request.HARD_MAX6 !=null) { updateQuery.Append(" ,HARD_MAX6 = CASE WHEN HARD_MAX6 IS NULL THEN " + request.HARD_MAX6 + " ELSE HARD_MAX6 END "); }
            if (request.HARD_MAX7 !=null) { updateQuery.Append(" ,HARD_MAX7 = CASE WHEN HARD_MAX7 IS NULL THEN " + request.HARD_MAX7 + " ELSE HARD_MAX7 END "); }
            if (request.HARD_MAX8 !=null) { updateQuery.Append(" ,HARD_MAX8 = CASE WHEN HARD_MAX8 IS NULL THEN " + request.HARD_MAX8 + " ELSE HARD_MAX8 END "); }
            if (request.HARD_MAX9 != null) { updateQuery.Append(" ,HARD_MAX9 = CASE WHEN HARD_MAX9 IS NULL THEN " + request.HARD_MAX9 + " ELSE HARD_MAX9 END "); }
            if (request.HARD_MAX10 !=null) { updateQuery.Append(" ,HARD_MAX10 = CASE WHEN HARD_MAX10 IS NULL THEN " + request.HARD_MAX10 + " ELSE HARD_MAX10 END "); }
            if (request.HARD_MAX11 !=null) { updateQuery.Append(" ,HARD_MAX11 = CASE WHEN HARD_MAX11 IS NULL THEN " + request.HARD_MAX11 + " ELSE HARD_MAX11 END "); }
            if (request.HARD_MAX12 !=null) { updateQuery.Append(" ,HARD_MAX12 = CASE WHEN HARD_MAX12 IS NULL THEN " + request.HARD_MAX12 + " ELSE HARD_MAX12 END "); }
            if (request.HARD_MAX13 !=null) { updateQuery.Append(" ,HARD_MAX13 = CASE WHEN HARD_MAX13 IS NULL THEN " + request.HARD_MAX13 + " ELSE HARD_MAX13 END "); }
            if (request.HARD_MAX14 !=null) { updateQuery.Append(" ,HARD_MAX14 = CASE WHEN HARD_MAX14 IS NULL THEN " + request.HARD_MAX14 + " ELSE HARD_MAX14 END "); }
            if (request.HARD_MAX15 !=null) { updateQuery.Append(" ,HARD_MAX15 = CASE WHEN HARD_MAX15 IS NULL THEN " + request.HARD_MAX15 + " ELSE HARD_MAX15 END "); }
            if (request.HARD_MAX16 !=null) { updateQuery.Append(" ,HARD_MAX16 = CASE WHEN HARD_MAX16 IS NULL THEN " + request.HARD_MAX16 + " ELSE HARD_MAX16 END "); }
            if (request.HARD_MAX17 !=null) { updateQuery.Append(" ,HARD_MAX17 = CASE WHEN HARD_MAX17 IS NULL THEN " + request.HARD_MAX17 + " ELSE HARD_MAX17 END "); }
            if (request.HARD_MAX18 !=null) { updateQuery.Append(" ,HARD_MAX18 = CASE WHEN HARD_MAX18 IS NULL THEN " + request.HARD_MAX18 + " ELSE HARD_MAX18 END "); }
            if (request.HARD_MAX19 !=null) { updateQuery.Append(" ,HARD_MAX19 = CASE WHEN HARD_MAX19 IS NULL THEN " + request.HARD_MAX19 + " ELSE HARD_MAX19 END "); }
            if (request.HARD_MAX20 !=null) { updateQuery.Append(" ,HARD_MAX20 = CASE WHEN HARD_MAX20 IS NULL THEN " + request.HARD_MAX20 + " ELSE HARD_MAX20 END "); }
            if (request.HARD_MAX21 !=null) { updateQuery.Append(" ,HARD_MAX21 = CASE WHEN HARD_MAX21 IS NULL THEN " + request.HARD_MAX21 + " ELSE HARD_MAX21 END "); }
            if (request.HARD_MAX22 !=null) { updateQuery.Append(" ,HARD_MAX22 = CASE WHEN HARD_MAX22 IS NULL THEN " + request.HARD_MAX22 + " ELSE HARD_MAX22 END "); }
            if (request.HARD_MAX23 !=null) { updateQuery.Append(" ,HARD_MAX23 = CASE WHEN HARD_MAX23 IS NULL THEN " + request.HARD_MAX23 + " ELSE HARD_MAX23 END "); }
            if (request.HARD_MAX24 !=null) { updateQuery.Append(" ,HARD_MAX24 = CASE WHEN HARD_MAX24 IS NULL THEN " + request.HARD_MAX24 + " ELSE HARD_MAX24 END "); }
            if (request.HARD_MAX25 !=null) { updateQuery.Append(" ,HARD_MAX25 = CASE WHEN HARD_MAX25 IS NULL THEN " + request.HARD_MAX25 + " ELSE HARD_MAX25 END "); }
            if (request.HARD_ACTUAL1 !=null) { updateQuery.Append(" ,HARD_ACTUAL1 = CASE WHEN HARD_ACTUAL1 IS NULL THEN " + request.HARD_ACTUAL1 + " ELSE HARD_ACTUAL1 END "); }
            if (request.HARD_ACTUAL2 !=null) { updateQuery.Append(" ,HARD_ACTUAL2 = CASE WHEN HARD_ACTUAL2 IS NULL THEN " + request.HARD_ACTUAL2 + " ELSE HARD_ACTUAL2 END "); }
            if (request.HARD_ACTUAL3 !=null) { updateQuery.Append(" ,HARD_ACTUAL3 = CASE WHEN HARD_ACTUAL3 IS NULL THEN " + request.HARD_ACTUAL3 + " ELSE HARD_ACTUAL3 END "); }
            if (request.HARD_ACTUAL4 !=null) { updateQuery.Append(" ,HARD_ACTUAL4 = CASE WHEN HARD_ACTUAL4 IS NULL THEN " + request.HARD_ACTUAL4 + " ELSE HARD_ACTUAL4 END "); }
            if (request.HARD_ACTUAL5 !=null) { updateQuery.Append(" ,HARD_ACTUAL5 = CASE WHEN HARD_ACTUAL5 IS NULL THEN " + request.HARD_ACTUAL5 + " ELSE HARD_ACTUAL5 END "); }
            if (request.HARD_ACTUAL6 !=null) { updateQuery.Append(" ,HARD_ACTUAL6 = CASE WHEN HARD_ACTUAL6 IS NULL THEN " + request.HARD_ACTUAL6 + " ELSE HARD_ACTUAL6 END "); }
            if (request.HARD_ACTUAL7 !=null) { updateQuery.Append(" ,HARD_ACTUAL7 = CASE WHEN HARD_ACTUAL7 IS NULL THEN " + request.HARD_ACTUAL7 + " ELSE HARD_ACTUAL7 END "); }
            if (request.HARD_ACTUAL8 !=null) { updateQuery.Append(" ,HARD_ACTUAL8 = CASE WHEN HARD_ACTUAL8 IS NULL THEN " + request.HARD_ACTUAL8 + " ELSE HARD_ACTUAL8 END "); }
            if (request.HARD_ACTUAL9 !=null) { updateQuery.Append(" ,HARD_ACTUAL9 = CASE WHEN HARD_ACTUAL9 IS NULL THEN " + request.HARD_ACTUAL9 + " ELSE HARD_ACTUAL9 END "); }
            if (request.HARD_ACTUAL10 !=null) { updateQuery.Append(" ,HARD_ACTUAL10 = CASE WHEN HARD_ACTUAL10 IS NULL THEN " + request.HARD_ACTUAL10 + "ELSE HARD_ACTUAL10 END "); }
            if (request.HARD_ACTUAL11 !=null) { updateQuery.Append(" ,HARD_ACTUAL11 = CASE WHEN HARD_ACTUAL11 IS NULL THEN " + request.HARD_ACTUAL11 + "ELSE HARD_ACTUAL11 END "); }
            if (request.HARD_ACTUAL12 !=null) { updateQuery.Append(" ,HARD_ACTUAL12 = CASE WHEN HARD_ACTUAL12 IS NULL THEN " + request.HARD_ACTUAL12 + "ELSE HARD_ACTUAL12 END "); }
            if (request.HARD_ACTUAL13 !=null) { updateQuery.Append(" ,HARD_ACTUAL13 = CASE WHEN HARD_ACTUAL13 IS NULL THEN " + request.HARD_ACTUAL13 + "ELSE HARD_ACTUAL13 END "); }
            if (request.HARD_ACTUAL14 !=null) { updateQuery.Append(" ,HARD_ACTUAL14 = CASE WHEN HARD_ACTUAL14 IS NULL THEN " + request.HARD_ACTUAL14 + "ELSE HARD_ACTUAL14 END "); }
            if (request.HARD_ACTUAL15 !=null) { updateQuery.Append(" ,HARD_ACTUAL15 = CASE WHEN HARD_ACTUAL15 IS NULL THEN " + request.HARD_ACTUAL15 + "ELSE HARD_ACTUAL15 END "); }
            if (request.HARD_ACTUAL16 !=null) { updateQuery.Append(" ,HARD_ACTUAL16 = CASE WHEN HARD_ACTUAL16 IS NULL THEN " + request.HARD_ACTUAL16 + "ELSE HARD_ACTUAL16 END "); }
            if (request.HARD_ACTUAL17 !=null) { updateQuery.Append(" ,HARD_ACTUAL17 = CASE WHEN HARD_ACTUAL17 IS NULL THEN " + request.HARD_ACTUAL17 + "ELSE HARD_ACTUAL17 END "); }
            if (request.HARD_ACTUAL18 !=null) { updateQuery.Append(" ,HARD_ACTUAL18 = CASE WHEN HARD_ACTUAL18 IS NULL THEN " + request.HARD_ACTUAL18 + "ELSE HARD_ACTUAL18 END "); }
            if (request.HARD_ACTUAL19 !=null) { updateQuery.Append(" ,HARD_ACTUAL19 = CASE WHEN HARD_ACTUAL19 IS NULL THEN " + request.HARD_ACTUAL19 + "ELSE HARD_ACTUAL19 END "); }
            if (request.HARD_ACTUAL20 !=null) { updateQuery.Append(" ,HARD_ACTUAL20 = CASE WHEN HARD_ACTUAL20 IS NULL THEN " + request.HARD_ACTUAL20 + "ELSE HARD_ACTUAL20 END "); }
            if (request.HARD_ACTUAL21 !=null) { updateQuery.Append(" ,HARD_ACTUAL21 = CASE WHEN HARD_ACTUAL21 IS NULL THEN " + request.HARD_ACTUAL21 + "ELSE HARD_ACTUAL21 END "); }
            if (request.HARD_ACTUAL22 !=null) { updateQuery.Append(" ,HARD_ACTUAL22 = CASE WHEN HARD_ACTUAL22 IS NULL THEN " + request.HARD_ACTUAL22 + "ELSE HARD_ACTUAL22 END "); }
            if (request.HARD_ACTUAL23 !=null) { updateQuery.Append(" ,HARD_ACTUAL23 = CASE WHEN HARD_ACTUAL23 IS NULL THEN " + request.HARD_ACTUAL23 + "ELSE HARD_ACTUAL23 END "); }
            if (request.HARD_ACTUAL24 !=null) { updateQuery.Append(" ,HARD_ACTUAL24 = CASE WHEN HARD_ACTUAL24 IS NULL THEN " + request.HARD_ACTUAL24 + "ELSE HARD_ACTUAL24 END "); }
            if (request.HARD_ACTUAL25 !=null) { updateQuery.Append(" ,HARD_ACTUAL25 = CASE WHEN HARD_ACTUAL25 IS NULL THEN " + request.HARD_ACTUAL25 + "ELSE HARD_ACTUAL25 END "); }
            updateQuery.Append(" ,UPDATED_DATE = SYSDATE ");
            updateQuery.Append(" WHERE ");
            updateQuery.Append(" CHARGE ='" + request.CHARGE + "'");
            updateQuery.Append(" AND DIM_D =" + request.DIM_D);
            updateQuery.Append(" AND MATL_NAME_MAKER ='" + request.MATL_NAME_MAKER + "'");
            updateQuery.Append(" AND CREATED_DATE > ADD_MONTHS(SYSDATE, -8) ");
            ExecuteNonQuery(updateQuery.ToString());
        }
        public bool IsMillSheetDataNotExist(MakerMillSheetDetails request)
        {
            bool dataNotExist = true;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" CHARGE ='" + request.CHARGE + "'");
            selectQuery.Append(" AND DIM_D ='" + request.DIM_D + "'");
            selectQuery.Append(" AND MATL_NAME_MAKER ='" + request.MATL_NAME_MAKER + "'");
            selectQuery.Append(" AND CREATED_DATE > ADD_MONTHS(SYSDATE, -8) ");
            //selectQuery.Append(" AND FILENAME IS NULL ");
            //selectQuery.Append(" AND IS_EDI_DATA = 1 ");
            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                dataNotExist = false;
            }
            return dataNotExist;
        }
        public bool IsMillSheetFileNameAlreadyExist(MakerMillSheetDetails request)
        {
            bool fileAlreadyxist = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" CHARGE ='" + request.CHARGE + "'");
            selectQuery.Append(" AND DIM_D ='" + request.DIM_D + "'");
            selectQuery.Append(" AND MATL_NAME_MAKER ='" + request.MATL_NAME_MAKER + "'");
            selectQuery.Append(" AND CREATED_DATE > ADD_MONTHS(SYSDATE, -8) ");
            selectQuery.Append(" AND FILENAME IS NOT NULL ");
            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 0)
            {
                fileAlreadyxist = true;
            }
            return fileAlreadyxist;
        }
        public bool IsMillSheetMultipleRecordsExist(MakerMillSheetDetails request)
        {
            bool multipleDataExist = false;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" CHARGE ='" + request.CHARGE + "'");
            selectQuery.Append(" AND DIM_D ='" + request.DIM_D + "'");
            selectQuery.Append(" AND MATL_NAME_MAKER ='" + request.MATL_NAME_MAKER + "'");
            selectQuery.Append(" AND CREATED_DATE > ADD_MONTHS(SYSDATE, -8) ");
            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count > 1)
            {
                multipleDataExist = true;
            }
            return multipleDataExist;
        }
        public bool IsEDIAllDataValid(int runCD)
        {
            bool isValid = true;
            StringBuilder selectQuery = new StringBuilder();
            selectQuery.Append(" SELECT ");
            selectQuery.Append(" COUNT(*) ");
            selectQuery.Append(" FROM ");
            selectQuery.Append(" MAKER_MILL_SHEET_LOG ");
            selectQuery.Append(" WHERE ");
            selectQuery.Append(" RUN_CD ='" + runCD + "'");
            selectQuery.Append(" AND STATUS ='1' ");
            int count = Convert.ToInt32(ExecuteScalar(selectQuery.ToString()));
            if (count ==0)
            {
                isValid = false;
            }


            return isValid;
        }
    }
}
