using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Text;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class MakerMillSheetJfeEDIService: IMakerMillSheetJfeEDIService
    {
        private readonly IMakerMillSheetJfeEDIRepository _makermillsheetJfeEdiRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IAllCommonService _allCommonService;
        private readonly IConfiguration _configuration;
        private readonly string _makerName;
        private int _runId;
        public MakerMillSheetJfeEDIService(IMakerMillSheetJfeEDIRepository makermillsheetJfeediRepository, LocalizationServices localization, IAllCommonRepository allCommonRepository, IAllCommonService allCommonService, IConfiguration configuration)
        {
            _makermillsheetJfeEdiRepository = makermillsheetJfeediRepository;
            _localization = localization;
            _allCommonRepository = allCommonRepository;
            _allCommonService = allCommonService;
            _configuration = configuration;
            _makerName = _configuration.GetSection("MakerNameJfe").Value;

        }


        public void MillSheetJfeEDILogDataInsert(List<MakerMillSheetEDIDetails> request, string cultureName, int userID)
        {

            string pdfFileStorePath;
            DateTime created_date;
            DateTime printReportDate;
                        
            int runID = _makermillsheetJfeEdiRepository.GetMakerMillSheetBatchRunID();
            _runId = runID;
            int Location_CD = _allCommonRepository.GetUserLocationById(userID);

            string maker_Name = _makerName;
            int maker_cd = _makermillsheetJfeEdiRepository.GetmakerCD(maker_Name);


            //created_date = DateTime.Now;
            created_date = _allCommonRepository.GetSystemDateTime();
            printReportDate = DateTime.Now;

            DateTime dateTime = _allCommonRepository.GetSystemDateTime();
            string sysDate = Convert.ToString(dateTime.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS));

            if (cultureName == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }


            // Insert data into MAKER_MILL_SHEET_LOG TABLE 

            if (request == null || request.Count == 0)
            {
                //Utils.ShowMessage(
                //    Utils.GetMMSResource("msg_NoNyukaRecords"),
                //    MessageTypesEnum.Information
                //);
                return;
            }

            using (TransactionScope transaction = new TransactionScope())
            {

                try
                {
                    foreach (var row in request)
                    {
                        int status = 0;
                        const string statusMark = "A";
                        string errorMessage = string.Empty;
                        pdfFileStorePath = string.Empty;
                        //runID = 0;

                        _makermillsheetJfeEdiRepository.MillSheetJfeEDILogDataInsert(row, runID, maker_Name, status, statusMark, errorMessage, pdfFileStorePath, created_date);

                    }


                    // Insert data into MAKER_MILL_SHEET TABLE 

                    List<MakerMillSheetEDIDetails> makerLogTablesData = _makermillsheetJfeEdiRepository.GetMakerLogTablesData(runID);

                    if (makerLogTablesData == null || makerLogTablesData.Count == 0)
                    {
                        transaction.Complete();
                        return; 
                    }

                    var columnMeta = _makermillsheetJfeEdiRepository.GetColumnMetadata();

                    foreach (var row in makerLogTablesData)
                    {
                        string inValidData = string.Empty;

                        //long makerCd = row.MAKER_CD;
                        long logCd = row.MAKER_MILL_SHEET_LOG_CD;
                        string charge = row.CHARGE;
                        decimal? dim_d = row.DIM_D;

                        int status = 0;
                        string statusMark = "A";
                        string errorMessage = string.Empty;
                        pdfFileStorePath = string.Empty;

                        
                        inValidData = validateDataRow(row, columnMeta);

                        if (!string.IsNullOrWhiteSpace(inValidData))
                        {
                            status = 1;
                            statusMark = "VALERR";
                            errorMessage = inValidData;

                            _makermillsheetJfeEdiRepository.UpdateMakerMillSheetLog(logCd, status, statusMark, errorMessage);

                            continue;
                        }

                        if (_makermillsheetJfeEdiRepository.IsDuplicateDataExist(logCd))
                        {
                            status = 1;
                            statusMark = "VALERR";
                            errorMessage = _localization.GetLocalizedHtmlString("valMsg_MillsheetDataAlreadyExist");
                            //errorMessage = "同一のミルシート内容は既に取込済みの為、取込処理は行いませんでした";

                            _makermillsheetJfeEdiRepository.UpdateMakerMillSheetLog(logCd, status, statusMark, errorMessage);

                            continue;
                        }

                        if (_makermillsheetJfeEdiRepository.IsDuplicateOCRExist(charge, dim_d))
                        {
                            status = 1;
                            statusMark = "VALERR";
                            errorMessage = _localization.GetLocalizedHtmlString("valMsg_DuplicateOCRExist");
                            //errorMessage = "既に、OCR取込したデータが存在します。取り込む必要は有りません。";

                            _makermillsheetJfeEdiRepository.UpdateMakerMillSheetLog(logCd, status, statusMark, errorMessage);

                            continue;
                        }

                        _makermillsheetJfeEdiRepository.MillSheetJfeEDIDataInsert(row, maker_cd, maker_Name, runID, status, statusMark, errorMessage, pdfFileStorePath, Location_CD);
                    }
                    transaction.Complete();
                }
                catch 
                {
                    throw;
                }
            }
        }

        public string validateDataRow(MakerMillSheetEDIDetails row, Dictionary<string, ColumnMeta> columnMeta)
        {
            var properties = typeof(MakerMillSheetEDIDetails).GetProperties();

            foreach (var prop in properties)
            {
                string columnName = prop.Name.ToUpper();

                if (!columnMeta.ContainsKey(columnName))
                    continue;

                var meta = columnMeta[columnName];

                var value = prop.GetValue(row);

                if (value == null)
                    continue;

                string dataVal = value.ToString();

                if (string.IsNullOrEmpty(dataVal) || dataVal == "NULL")
                    continue;

                // VARCHAR validation
                if (meta.DataType == "VARCHAR2")
                {
                    if (meta.Length.HasValue && dataVal.Length > meta.Length.Value)
                    {
                        return $"「{columnName}:{dataVal}」 {_localization.GetLocalizedHtmlString("valMsg_DataSizeTooLarge")}";
                    }
                }

                // NUMBER validation
                else if (meta.DataType == "NUMBER")
                {
                    if (!decimal.TryParse(dataVal, out decimal number))
                    {
                        return $"「{columnName}:{dataVal}」 {_localization.GetLocalizedHtmlString("valMsg_NotNumericData")}";
                    }

                    int numericLength = meta.Precision ?? 0;
                    int decimalLength = meta.Scale ?? 0;

                    numericLength -= decimalLength;

                    var parts = dataVal.Split('.');

                    string numericPart = parts[0];
                    string decimalPart = parts.Length > 1 ? parts[1] : "";

                    if (numericPart.Length > numericLength)
                    {
                        return $"「{columnName}:{dataVal}」 {_localization.GetLocalizedHtmlString("valMsg_SizeExeedsAllowedLimit")}";
                    }

                    if (!string.IsNullOrEmpty(decimalPart) &&
                        decimalPart.Length > decimalLength)
                    {
                        return $"「{columnName}:{dataVal}」 {_localization.GetLocalizedHtmlString("valMsg_SizeExeedsAllowedLimit")}";
                    }
                }
            }

            return "";
        }

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID, DateTime printReportDate)
        {
            runID = _runId;

            List<MakerMillSheetEDIDetails> allData = _makermillsheetJfeEdiRepository.GetMakerMillSheetProcessRptData(runID, printReportDate);

            return allData;

        }

        public (int RunID, List<MakerMillSheetEDIDetails> ErrorData) GetMakerMillSheetErrorRptData(int runID, DateTime printReportDate)
        {
            runID = _runId;
            List<MakerMillSheetEDIDetails> errorData = _makermillsheetJfeEdiRepository.GetMakerMillSheetErrorRptData(runID, printReportDate);

            return (runID, errorData);
        }
    }
}
