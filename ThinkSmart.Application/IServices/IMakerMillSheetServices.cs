using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IMakerMillSheetServices
    {
        List<NameValue> GetUniqueTemplateList();
        int InsertMakerMillSheetDetails(List<MakerMillSheetDetails> details);
        List<NameValue> GetUnitTypeList();
        List<NameValue> GetChemicalSpecifcationList();
        List<MakerMillSheetDetails> GetMakerMillSheetList(GridSearchFields request);
        MakerMillSheetDetails GetMakerMillSheetDetailsByCD(long MAKERMILLSHEETCD);
        int MakerMillSheetDetailsUpdate(MakerMillSheetDetails request);
        int DeleteMakerMillSheetDetails(long MakerMillSheetCD);
        bool GetKubunName(string KubunName);
        MakerMillSheetDetails GetNextAndPriviousPDFFile(long MakerCD, string PDFPath, int PDFCount);
        void SavePDFSettings(List<MakerMillSheetConfiguration> request);
        List<MakerMillSheetConfiguration> GetMillSheetPDFDetails(int makerCD, string templateName);
        List<NameValue> GetMakerList();
        int GetDefaultLayoutData(string templateName);
        MakerMillSheetDetails GetMakerHardnessDetailJIS(string FileName);
        MakerMillSheetDetails GetMakerHardnessDetailJISCal(string FileName);
        List<MakerMillSheetDetails> GetMakerMillsheetDetails(string Charge,string DimD);
        List<MakerMillSheetConfiguration> GetPDFConfigList(GridSearchFields searchRequest);
        List<MakerMillSheetConfiguration> GetPDFDetailsByTemplateName(string templateName);
        void UpdatePDFSettings(List<MakerMillSheetConfiguration> request);
        void DeleteConfigDataByFieldCD(MakerMillSheetConfiguration request);
        void DeleteConfigDataByTemplateName(string templateName);
        bool IsTemplateNameAlreadyExist(string templateName);
        int GetMakerNameByConfigCD(int configCD);
        int GetMaxLogRunCD();
        bool IsFieldElementSaved(int fieldCD, string templateName);
        bool IsKoubanAlreadySaved(string kouban, int millsheetCD);
        int UpdateEDIData(List<MakerMillSheetDetails> request);
        (bool, string) IsFTPServerActive();
        bool IsDuplicateDataExist(MakerMillSheetDetails request);
        bool IsEDIAllDataValid(int runCD);

    }
}
