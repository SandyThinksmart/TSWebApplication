using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IMakerMillSheetJfeEDIRepository
    {
        void MillSheetJfeEDILogDataInsert(MakerMillSheetEDIDetails row, int runID, string maker_name, int status, string statusMark, string errormessage, string pdfFileStorePath, DateTime created_date);

        List<MakerMillSheetEDIDetails> GetMakerLogTablesData(int runID);

        public void UpdateMakerMillSheetLog(long logCd, int status, string statusMark, string errorMessage);

        public bool IsDuplicateDataExist(long logCd);

        public bool IsDuplicateOCRExist(string charge, decimal? dim_d);

        public void MillSheetJfeEDIDataInsert(MakerMillSheetEDIDetails row, int maker_cd, string maker_name, int batchID, int status, string statusMark, string errorMessage, string pdfFileStorePath,int Location_CD);  // long makerCd,

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetProcessRptData(int runID, DateTime printReportDate);

        public List<MakerMillSheetEDIDetails> GetMakerMillSheetErrorRptData(int runID, DateTime printReportDate);
        public int GetMakerMillSheetBatchRunID();

        public Dictionary<string, ColumnMeta> GetColumnMetadata();
        public int GetmakerCD(string maker_Name);
    }
}
