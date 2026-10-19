using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;
using static System.Formats.Asn1.AsnWriter;

namespace TSWebApplication.Application.Services
{
    public class MakerMillSheetServices : IMakerMillSheetServices
    {
        private readonly IMakerMillSheetRepository _makermillsheetRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IAllCommonService _allCommonService;
        private string add = "A";
        private string update = "U";
        private string delete = "D";
        private string skip = "S";
        private string valerr = "VALERR";
        private string warning = "W";
        private short status_add = 0;
        private short status_err = 1;
        public MakerMillSheetServices(IMakerMillSheetRepository makermillsheetRepository, LocalizationServices localization, IAllCommonRepository allCommonRepository, IAllCommonService allCommonService)
        {
            _makermillsheetRepository = makermillsheetRepository;
            _localization = localization;
            _allCommonRepository = allCommonRepository;
            _allCommonService = allCommonService;

        }

        public int InsertMakerMillSheetDetails(List<MakerMillSheetDetails> details)
        {
            bool isFileUploaded = false;
            int responseCode = 0; // 0 for successfully save and 1 for FTP not connected
            int serialNumber = 0;

            if (details[0].CULTURE_NAME == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                DateTime sysDate = _allCommonService.GetSystemDateTime();
                foreach (var request in details)
                {
                    serialNumber++;
                    string formattedNumber = serialNumber.ToString("D4");

                    request.STATUS = status_add;
                    request.STATUS_MARK = add;
                    //if (request.FORM_FILE != null)
                    //{
                    //request.FILENAME = request.MAKER_NAME + "_" + request.CHARGE + "_" + sysDate.ToString("yyyyMMddHHmm");
                    //request.PDF_FILE_PATH = sysDate.Year + "/" + sysDate.Month.ToString("00") + "/" + request.FILENAME + ".pdf";
                    request.FILENAME = sysDate.ToString("yyyyMMddHHmmss") + "-"+ formattedNumber;
                    request.PDF_FILE_PATH = sysDate.Year + "/" + sysDate.Month.ToString("00") + "/" + request.FILENAME + ".pdf";
                    //  }

                    int logCD = _makermillsheetRepository.InsertMakerMillSheetLogDetails(request);

                    //if (request.FORM_FILE == null)
                    //{ 
                    //     request.STATUS = status_add;
                    //     request.STATUS_MARK = add;
                    //     int logCD = _makermillsheetRepository.InsertMakerMillSheetLogDetails(request);


                    //     bool isDuplicateMillsheet = _makermillsheetRepository.IsDuplicateDataExist(logCD);

                    //    if (isDuplicateMillsheet)
                    //    {

                    //        string msg = _localization.GetLocalizedHtmlString("valMsg_MillsheetDataAlreadyExist");
                    //        _makermillsheetRepository.UpdateMakerMillsheetLogStatus(logCD, status_err, valerr, msg);
                    //    }
                    //    else
                    //    {
                    //        bool isImportedByEDI = _makermillsheetRepository.IsDuplicateEDIExist(request.CHARGE, Convert.ToDecimal(request.DIM_D));

                    //        if (isImportedByEDI)
                    //        {
                    //            string msg = _localization.GetLocalizedHtmlString("valMsg_MillsheetImportdByEDI");
                    //            _makermillsheetRepository.UpdateMakerMillsheetLogStatus(logCD, status_err, valerr, msg);
                    //        }
                    //        else
                    //        {

                    //            if (Convert.ToInt32(request.USER_ID) > 0)
                    //            {
                    //                request.INPUT_LOCATION_CD = Convert.ToInt16(_allCommonRepository.GetUserLocationById(request.USER_ID.Value));
                    //            }
                    //            _makermillsheetRepository.InsertMakerMillSheetDetails(request);

                    //        }
                    //    }

                    //}
                    //else 
                    //{
                        if (Convert.ToInt32(request.USER_ID) > 0)
                        {
                            request.INPUT_LOCATION_CD = Convert.ToInt16(_allCommonRepository.GetUserLocationById(request.USER_ID.Value));
                        }
                        _makermillsheetRepository.InsertMakerMillSheetDetails(request);
                       
                         isFileUploaded = IsFileUploadedOnFTP(request.FORM_FILE, sysDate, request.FILENAME).Result;
                        
                   // }
                }

                    transaction.Complete();
                   
                
            }
            return responseCode;
        }
        public async Task<bool> IsFileUploadedOnFTP(IFormFile file, DateTime sysDate, string fileName)
        {
            bool isFileUploaded = false;
            List<MstConfiguration> configuration = new List<MstConfiguration>();
            string ftpPath = string.Empty;
            string ftpUsername = string.Empty;
            string ftpPassword = string.Empty;

            try
            {
                configuration = _allCommonService.GetFTPDetail();

                foreach (var item in configuration)
                {
                    if (item.KEY.ToUpper() == "MILLSHEET_FTPPATH")
                    {
                        ftpPath = item.VALUE;
                    }
                    else if (item.KEY.ToUpper() == "MILLSHEET_FTPPASSWORD")
                    {
                        ftpPassword = item.VALUE;
                    }
                    else if (item.KEY.ToUpper() == "MILLSHEET_FTPUSERNAME")
                    {
                        ftpUsername = item.VALUE;
                    }
                }
                string cYear = Convert.ToString(sysDate.Year);
                ftpPath = ftpPath + "/" + cYear;
                bool YrDirectory = FtpDirectoryExists(ftpPath, ftpUsername, ftpPassword);

                if (YrDirectory == false)
                {
                    CreateFtpDirectory(ftpPath, ftpUsername, ftpPassword);
                }
                string cMonth = Convert.ToString(sysDate.Month.ToString("00"));
                ftpPath = ftpPath + "/" + cMonth;

                bool MnthDirectory = FtpDirectoryExists(ftpPath, ftpUsername, ftpPassword);

                if (MnthDirectory == false)
                {
                    CreateFtpDirectory(ftpPath, ftpUsername, ftpPassword);
                }
                ftpPath = ftpPath + "/" + fileName + ".pdf";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    using (Stream fileStream = file.OpenReadStream())
                    {
                        await fileStream.CopyToAsync(requestStream);
                    }
                }
                isFileUploaded = true;
            }
            catch (Exception)
            {
                isFileUploaded = false;
            }

            return isFileUploaded;
        }
        public List<NameValue> GetUnitTypeList()
        {
            var _UnitTypeList = UIUtils.GetUnitTypetKubun();
            return _UnitTypeList;
        }
        public List<NameValue> GetChemicalSpecifcationList()
        {
            var _UnitTypeList = UIUtils.GetChemicalSpecifcationKubun();
            return _UnitTypeList;
        }

        public List<MakerMillSheetDetails> GetMakerMillSheetList(GridSearchFields searchRequest)
        {
            int Location_CD = _allCommonRepository.GetUserLocationById(Convert.ToInt32(searchRequest.USER_ID));
            List<SearchField> searchFields = ComposeSearchCriteria(searchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            return _makermillsheetRepository.GetMakerMillSheetList(searchFields, selectedSortCriteria, Location_CD);

        }

        public MakerMillSheetDetails GetMakerMillSheetDetailsByCD(long millsheetCD)
        {
            return _makermillsheetRepository.GetMillSheetDetailsByCD(millsheetCD);

        }
        private List<SearchField> ComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {

            var searchFields = new List<SearchField>();
            if (Convert.ToInt32(gridSearchRequest.MAKER_CD) > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.MAKER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MAKER_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.MATL_NAME_MAK))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.MATL_NAME_MAKER",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.MATL_NAME_MAK.ToLower()
                };
                searchFields.Add(searchField);
            }


            if (gridSearchRequest.MATL_DIM_D > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.DIM_D",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MATL_DIM_D).ToLower().Trim(),
                    SearchText2 = Convert.ToString(gridSearchRequest.MATL_DIM_W).ToLower().Trim()

                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.CHARGE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.CHARGE",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.CHARGE.ToLower()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.KOUBAN))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.KOUBAN",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.KOUBAN.ToLower()
                };
                searchFields.Add(searchField);
            }
            if (!string.IsNullOrEmpty(gridSearchRequest.COMPLETE_START_DATE) && !string.IsNullOrEmpty(gridSearchRequest.COMPLETE_END_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.CREATED_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.COMPLETE_START_DATE,
                    SearchText2 = gridSearchRequest.COMPLETE_END_DATE,

                };
                searchFields.Add(searchField);
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.COMPLETE_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.CREATED_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.COMPLETE_START_DATE,

                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.UPDATE_START_DATE) && !string.IsNullOrEmpty(gridSearchRequest.UPDATE_END_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.UPDATED_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.UPDATE_START_DATE,
                    SearchText2 = gridSearchRequest.UPDATE_END_DATE,

                };
                searchFields.Add(searchField);
            }
            else if (!string.IsNullOrEmpty(gridSearchRequest.UPDATE_START_DATE))
            {
                var searchField = new SearchField
                {
                    FieldName = "MMS.UPDATED_DATE",
                    SearchType = SearchType.Date,
                    SearchText1 = gridSearchRequest.UPDATE_START_DATE,

                };
                searchFields.Add(searchField);
            }
            return searchFields;


        }


        public int MakerMillSheetDetailsUpdate(MakerMillSheetDetails details)
        {
            bool isFileUploaded = false;
            int responseCode = 0; // 0 for successfully update, 1 for ftp not connected

            using (TransactionScope transaction = new TransactionScope())
            {

                //DateTime sysDate = _allCommonService.GetSystemDateTime();
                //if (details.FORM_FILE != null)
                //{

                //    details.FILENAME = details.MAKER_NAME + "_" + details.CHARGE + "_" + sysDate.ToString("yyyyMMddHHmm");
                //    details.PDF_FILE_PATH = sysDate.Year + "/" + sysDate.Month.ToString("00") + "/" + details.FILENAME + ".pdf";
                //}

                _makermillsheetRepository.UpdateMakerMillSheetDetails(details);

                //if (details.FORM_FILE != null)
                //{
                //    isFileUploaded = IsFileUploadedOnFTP(details.FORM_FILE, sysDate, details.FILENAME).Result;
                //}
                transaction.Complete();

                //if (details.file != null && isFileUploaded == true)
                //{
                //    transaction.Complete();
                //}
                //else if (details.file == null)
                //{
                //    transaction.Complete();
                //}
                //else
                //{
                //    responseCode = 1;
                //    transaction.Dispose();
                //}
            }
            return responseCode;
        }

        public int DeleteMakerMillSheetDetails(long MakerMillSheetCD)
        {
            bool isfileDeleted = false;
            int dataDeleted = 0;
            if (MakerMillSheetCD > 0)
                using (TransactionScope transaction = new TransactionScope())
                {
                    string pdfPath = _makermillsheetRepository.GetMillSheetPDFPath(MakerMillSheetCD);

                    _makermillsheetRepository.DeleteMakerMillSheetDetails(MakerMillSheetCD);
                    if (!string.IsNullOrEmpty(pdfPath))
                    {
                        isfileDeleted = DeleteFileFromFTP(pdfPath);
                    }
                    if(isfileDeleted == true)
                    {
                        transaction.Complete();
                    }else
                    {
                        dataDeleted = 1;
                        transaction.Dispose();
                    }

                }
            return dataDeleted; // 0 for success, 1 for ftp not connected
        }
        private bool DeleteFileFromFTP(string pdfPath)
        {
            bool fileDeleted = false;
            List<MstConfiguration> configuration = new List<MstConfiguration>();
            string ftpPath = string.Empty;
            string ftpUsername = string.Empty;
            string ftpPassword = string.Empty;
            try
            {
                configuration = _allCommonService.GetFTPDetail();

                foreach (var item in configuration)
                {
                    if (item.KEY.ToUpper() == "MILLSHEET_FTPPATH")
                    {
                        ftpPath = item.VALUE;
                    }
                    else if (item.KEY.ToUpper() == "MILLSHEET_FTPPASSWORD")
                    {
                        ftpPassword = item.VALUE;
                    }
                    else if (item.KEY.ToUpper() == "MILLSHEET_FTPUSERNAME")
                    {
                        ftpUsername = item.VALUE;
                    }
                }
                ftpPath = ftpPath + "/" + pdfPath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.UseBinary = true;
                request.KeepAlive = false;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine("Delete status: " + response.StatusDescription);
                }
                fileDeleted = true;
            }
            catch (Exception)
            {
                fileDeleted = false;
            }

            return fileDeleted;
        }
        public bool GetKubunName(string KubunName)
        {
            return _makermillsheetRepository.GetKubunName(KubunName);
        }

        public MakerMillSheetDetails GetNextAndPriviousPDFFile(long MakerCD, string PDFPath, int PDFCount)
        {
            var result = _makermillsheetRepository.GetNextAndPriviousPDFFile(MakerCD, PDFPath, PDFCount);
            return result;

        }
        public void SavePDFSettings(List<MakerMillSheetConfiguration> details)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var request in details)
                {

                    if (Convert.ToInt32(request.TOP_LEFT_X) > 0 && Convert.ToInt32(request.TOP_RIGHT_X) > 0 && Convert.ToInt32(request.BOTTOM_LEFT_X) > 0 && Convert.ToInt32(request.BOTTOM_RIGHT_X) > 0 && Convert.ToInt32(request.MAKER_CD) > 0)
                    {
                        _makermillsheetRepository.SaveSettings(request);
                    }
                }
                scope.Complete();
                scope.Dispose();
            }
        }
        public void UpdatePDFSettings(List<MakerMillSheetConfiguration> details)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var request in details)
                {
                    if (request.ACTION.ToUpper() == "A")
                    {
                        _makermillsheetRepository.SaveSettings(request);
                    }
                    else if (request.ACTION.ToUpper() == "U")
                    {
                        _makermillsheetRepository.UpdateSettings(request);
                    }
                    else if (request.ACTION.ToUpper() == "D")
                    {
                        _makermillsheetRepository.DeleteMillsheetConfigByFieldCD(request);
                    }

                }
                transaction.Complete();
                transaction.Dispose();
            }

        }
        public List<MakerMillSheetConfiguration> GetMillSheetPDFDetails(int makerCD,string templateName)
        {
            return _makermillsheetRepository.GetPDFDetails(makerCD, templateName);
        }
        public List<NameValue> GetMakerList()
        {
            return _makermillsheetRepository.GetMakerList();
        }
        public int GetDefaultLayoutData(string templateName)
        {
            return _makermillsheetRepository.GetLayoutData(templateName);
        }

        public List<MakerMillSheetDetails> GetMakerMillsheetDetails(string Charge, string DimD)
        {
            return _makermillsheetRepository.GetMakerMillsheetDetails(Charge, DimD);


        }

        public MakerMillSheetDetails GetMakerHardnessDetailJIS(string FileName)
        {
            return _makermillsheetRepository.GetMakerHardnessDetailJIS(FileName);


        }
        public MakerMillSheetDetails GetMakerHardnessDetailJISCal(string FileName)
        {
            return _makermillsheetRepository.GetMakerHardnessDetailJISCal(FileName);


        }
        public List<MakerMillSheetConfiguration> GetPDFConfigList(GridSearchFields searchRequest)
        {
            List<SearchField> searchFields = PDFConfigComposeSearchCriteria(searchRequest);
            string selectedSortCriteria = Helper.ParseSortCriteria(searchRequest.SORT_FIELDS);
            return _makermillsheetRepository.GetPDFConfigList(searchFields, selectedSortCriteria);

        }
        private List<SearchField> PDFConfigComposeSearchCriteria(GridSearchFields gridSearchRequest)
        {

            var searchFields = new List<SearchField>();
            if (Convert.ToInt32(gridSearchRequest.MAKER_CD) > 0)
            {
                var searchField = new SearchField
                {
                    FieldName = "CONF.MAKER_CD",
                    SearchType = SearchType.Number,
                    SearchText1 = Convert.ToString(gridSearchRequest.MAKER_CD)
                };
                searchFields.Add(searchField);
            }

            if (!string.IsNullOrEmpty(gridSearchRequest.UNIQUE_TEMPLATE_NAME))
            {
                var searchField = new SearchField
                {
                    FieldName = "CONF.UNIQUE_TEMPLATE_NAME",
                    SearchType = SearchType.Text,
                    SearchText1 = gridSearchRequest.UNIQUE_TEMPLATE_NAME.ToLower()
                };
                searchFields.Add(searchField);
            }

            return searchFields;
        }
        public List<MakerMillSheetConfiguration> GetPDFDetailsByTemplateName(string templateName)
        {
            return _makermillsheetRepository.GetPDFDetailsByTemplateName(templateName);

        }
        public void DeleteConfigDataByFieldCD(MakerMillSheetConfiguration request)
        {
            _makermillsheetRepository.DeleteMillsheetConfigByFieldCD(request);
       
        }
        public void DeleteConfigDataByTemplateName(string templateName)
        {
            _makermillsheetRepository.DeleteConfigByTemplateName(templateName);
        }
        public bool IsTemplateNameAlreadyExist(string templateName)
        {
            return _makermillsheetRepository.CheckTemplateName(templateName);
        }
        public List<NameValue> GetUniqueTemplateList()
        {
            return _makermillsheetRepository.GetTemplateList();

        }
        public int GetMakerNameByConfigCD(int configCD)
        {
            return _makermillsheetRepository.GetMakerByConfigCD(configCD);
        }
        public bool FtpDirectoryExists(string ftpFolderPath, string username, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFolderPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return true; // Folder exists
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;

                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false; // Folder does NOT exist
                }

                throw; // Other error
            }
        }
        public void CreateFtpDirectory(string ftpFolderPath, string username, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFolderPath);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine("Directory created successfully.");
                }
            }
            catch (WebException ex)
            {
                throw;
            }
        }
        public int GetMaxLogRunCD()
        {
            return _makermillsheetRepository.GetMaxRunCD();
        }
        public bool IsFieldElementSaved(int fieldCD, string templateName)
        {
            return _makermillsheetRepository.IsFieldAlreadyExist(fieldCD, templateName);
        }
        public bool IsKoubanAlreadySaved(string kouban, int millsheetCD)
        {
            return _makermillsheetRepository.IsKoubanAlreadySaved(kouban, millsheetCD);
        }
        public int UpdateEDIData(List<MakerMillSheetDetails> request)
        {
            bool isFileUploaded = false;
            bool errFound = false;
            string errMsg = string.Empty;
            int returnVal = 1; //0 for success, 1 for exception
            int serialNumber = 0;
            using (TransactionScope transaction = new TransactionScope())
            {

                DateTime sysDate = _allCommonService.GetSystemDateTime();
                foreach (var item in request)
                {
                    serialNumber++;
                    string formattedNumber = serialNumber.ToString("D4");

                    item.STATUS = status_add;
                    item.STATUS_MARK = add;
                    //item.FILENAME = item.MAKER_NAME + "_" + item.CHARGE + "_" + sysDate.ToString("yyyyMMddHHmm");
                    //item.PDF_FILE_PATH = sysDate.Year + "/" + sysDate.Month.ToString("00") + "/" + item.FILENAME + ".pdf";

                    item.FILENAME = sysDate.ToString("yyyyMMddHHmmss") + "-" + formattedNumber;
                    item.PDF_FILE_PATH = sysDate.Year + "/" + sysDate.Month.ToString("00") + "/" + item.FILENAME + ".pdf";

                    int logCD = _makermillsheetRepository.InsertMakerMillSheetLogDetails(item);

                        errFound = _makermillsheetRepository.IsMillSheetDataNotExist(item);
                        errMsg = _localization.GetLocalizedHtmlString("valMsg_MillsheetDataNotFound");

                    if (errFound == false)
                    {
                        errFound = _makermillsheetRepository.IsMillSheetMultipleRecordsExist(item);
                        errMsg = _localization.GetLocalizedHtmlString("valMsg_MillsheetMultipleRecords");
                    }
                    bool isFileAlreadyExist = _makermillsheetRepository.IsMillSheetFileNameAlreadyExist(item);

                    if (isFileAlreadyExist == true && errFound == false)
                    {
                        errFound = true;
                        errMsg = _localization.GetLocalizedHtmlString("valMsg_MillsheetFileNameAlreadyExist");
                        
                    }
                        if (errFound == true)
                    {
                        _makermillsheetRepository.UpdateMakerMillsheetLogStatus(logCD, status_err, valerr, errMsg);
                    }
                    if (errFound == false)
                    {
                        _makermillsheetRepository.UpdateEDIMillsheetData(item);
                        isFileUploaded = IsFileUploadedOnFTP(item.FORM_FILE, sysDate, item.FILENAME).Result;
                    }
                    //if (isFileUploaded == false)
                    //{
                    //    break;
                    //}
                }
                transaction.Complete();
                returnVal = 0;
            }

            return returnVal; 
        }
        public (bool,string) IsFTPServerActive()
        {
            string ipAddress = string.Empty;
            List<MstConfiguration> configuration = new List<MstConfiguration>();
            string ftpPath = string.Empty;
            string ftpUsername = string.Empty;
            string ftpPassword = string.Empty;

            try
            {
                configuration = _allCommonService.GetFTPDetail();

                foreach (var item in configuration)
                {
                    if (item.KEY.ToUpper() == "MILLSHEET_FTPPATH")
                    {
                        ftpPath = item.VALUE;
                        Uri uri = new Uri(ftpPath);
                        ipAddress = uri.Host;
                    }
                    else if (item.KEY.ToUpper() == "MILLSHEET_FTPPASSWORD")
                    {
                        ftpPassword = item.VALUE;
                    }
                    else if (item.KEY.ToUpper() == "MILLSHEET_FTPUSERNAME")
                    {
                        ftpUsername = item.VALUE;
                    }
                }

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;
                request.UsePassive = true;
                request.KeepAlive = false;

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                return (true,string.Empty);

            }
            catch (Exception)
            {
                return (false,ipAddress);
            }

        }
        public bool IsDuplicateDataExist(MakerMillSheetDetails request)
        {
            bool dataExist = false;
            int getRowsCount = 0;
            getRowsCount = _makermillsheetRepository.IsDuplicateDataExist(request);

            //if (getRowsCount == 0)
            //{
            //    _makermillsheetRepository.IsDuplicateEDIExist(request.CHARGE,Convert.ToDecimal(request.DIM_D));
            //}
            if (getRowsCount > 0)
            {
                dataExist = true;
            }
                return dataExist;
        }
        public bool IsEDIAllDataValid(int runCD)
        {
           return _makermillsheetRepository.IsEDIAllDataValid(runCD);
        }
    }
}
