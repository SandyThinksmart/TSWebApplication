using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
   public interface IMakerMillSheetRepository
    {
        void InsertMakerMillSheetDetails(MakerMillSheetDetails request);
        List<MakerMillSheetDetails> GetMakerMillSheetList(List<SearchField> searchFields, string sortCiteria, int locationCD);
        MakerMillSheetDetails GetMillSheetDetailsByCD(long MakerMillSheetCD);
        void UpdateMakerMillSheetDetails(MakerMillSheetDetails details);
        void DeleteMakerMillSheetDetails(long MakerMillSheetCD);
        bool GetKubunName(string KubunName);
        MakerMillSheetDetails GetNextAndPriviousPDFFile(long MakerCD, string PDFPath, int PDFCount);
        void SaveSettings(MakerMillSheetConfiguration request);
        List<MakerMillSheetConfiguration> GetPDFDetails(int makerCD, string templateName);
        List<NameValue> GetMakerList();
        int GetLayoutData(string templateName);
        List<MakerMillSheetDetails> GetMakerMillsheetDetails(string Charge, string DimD);
        MakerMillSheetDetails GetMakerHardnessDetailJIS(string FileName);
        MakerMillSheetDetails GetMakerHardnessDetailJISCal(string FileName);
        List<MakerMillSheetConfiguration> GetPDFConfigList(List<SearchField> searchFields, string sortCiteria);
        List<MakerMillSheetConfiguration> GetPDFDetailsByTemplateName(string templateName);
        void UpdateSettings(MakerMillSheetConfiguration details);
        void DeleteMillsheetConfigByFieldCD(MakerMillSheetConfiguration request);
        void DeleteConfigByTemplateName(string templateName);
        bool CheckTemplateName(string templateName);
        List<NameValue> GetTemplateList();
        int GetMakerByConfigCD(int configCD);
        int GetMaxRunCD();
        int InsertMakerMillSheetLogDetails(MakerMillSheetDetails request);
        int IsDuplicateDataExist(MakerMillSheetDetails request);
        int IsDuplicateEDIExist(string charge, decimal dim_d);
        void UpdateMakerMillsheetLogStatus(int logCD, short status, string status_mark, string error);
        bool IsFieldAlreadyExist(int fieldCD, string templateName);
        string GetMillSheetPDFPath(long millsheetCD);
        bool IsKoubanAlreadySaved(string kouban, int millsheetCD);
        //int ProcessMillSheetPdf(List<MakerMillSheetOcrDetails> request);
        public void UpdateEDIMillsheetData(MakerMillSheetDetails request);
        bool IsMillSheetDataNotExist(MakerMillSheetDetails request);
        bool IsMillSheetFileNameAlreadyExist(MakerMillSheetDetails request);
        bool IsMillSheetMultipleRecordsExist(MakerMillSheetDetails request);
        bool IsEDIAllDataValid(int runCD);
       

    }
}
