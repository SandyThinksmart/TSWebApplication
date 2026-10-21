using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenCvSharp;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.Models;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class MakerMillSheetController : Controller
    {
        private readonly IMakerMillSheetServices _makermillsheetService;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        private readonly IViewRenderService _viewRenderService;
        private IHostingEnvironment Environment;
        static string apiKey;


        public MakerMillSheetController(IMakerMillSheetServices makermillsheetService, IAllCommonService allCommonService, LocalizationServices localization, IViewRenderService viewRenderService, IHostingEnvironment _environment, IConfiguration _configuration)
        {


            _localization = localization;
            _makermillsheetService = makermillsheetService;
            _allCommonService = allCommonService;
            _viewRenderService = viewRenderService;
            Environment = _environment;
            apiKey = Convert.ToString(_configuration.GetSection("GoogleVisionAPI")["Key"]);

        }
        public IActionResult MakerMillsheetSaveEdit()
        {
            string filePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet");

            if (Directory.Exists(filePath))
            {
                DeleteOldFiles(filePath, 1); // delete one day ago files
            }
            filePath = Path.Combine(filePath, "TempFile");
            if (Directory.Exists(filePath))
            {
                DeleteOldFiles(filePath, 1); // delete one day ago files
            }
            return View();
        }

        public IActionResult MakerMillSheetListPage()
        {
            return View();
        }
        public IActionResult MakerMillSheetPDFConfiguration()
        {
            string filePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet");

            if (Directory.Exists(filePath))
            {
                DeleteOldFiles(filePath, 1); // delete one day ago files
            }
            return View();
        }
        public IActionResult MakerMillSheetConfigListPage()
        {
            return View();
        }
        public IActionResult GetMillsheetPageLables()
        {
            var result = UIUtils.GetMillsheetlabels();
            string view = _viewRenderService.RenderToString("MakerMillSheet/MakerMillSheetLabelGrid", result);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GetChemicalSpecifcation()
        {
            var result = _makermillsheetService.GetChemicalSpecifcationList();
            return Ok(new { IsSuccess = true, message = result });
        }
        public IActionResult GetUnitType()
        {
            var result = _makermillsheetService.GetUnitTypeList();
            return Ok(new { IsSuccess = true, message = result });
        }

        public IActionResult GridSearch([FromBody] List<MakerMillSheetDetails> details)
        {
            string view = _viewRenderService.RenderToString("MakerMillSheet/MakerMillSheetListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GridSearchPDFConfig([FromBody] List<MakerMillSheetConfiguration> details)
        {
            string view = _viewRenderService.RenderToString("MakerMillSheet/MakerMillSheetConfigListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult InitializeSortPopup(bool isEntryEditDeleteScreen = false)
        {


            if (isEntryEditDeleteScreen != true)
            {


                var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "CREATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_SaveDateTime"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "UPDATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_UpdateDateTime"),
                                             IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MATL_NAME_MAKER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TypeOfSteel"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerMaster"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                          new TSSortField {
                                            FieldName = "CHARGE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Charge"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                        new TSSortField {
                                            FieldName = "KOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Kouban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },




                                    };
                return Json(new { IsSuccess = true, message = sortFields });
            }
            else
            {
                var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "MMS.CREATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_SaveDateTime"),
                                            //IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                         new TSSortField {
                                            FieldName = "MMS.UPDATED_DATE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_UpdateDateTime"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MM.MAKER_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MakerMaster"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MMS.MATL_NAME_MAKER",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_TypeOfSteel"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MMS.DIM_D",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_DimD"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },
                                        new TSSortField {
                                            FieldName = "MMS.LENGTH",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Length"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                          new TSSortField {
                                            FieldName = "MMS.CHARGE",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Charge"),
                                             IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },

                                        new TSSortField {
                                            FieldName = "MMS.KOUBAN",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_Kouban"),
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Ascending
                                        },




                                    };
                return Json(new { IsSuccess = true, message = sortFields });
            }
        }


        [HttpPost]
        public JsonResult PreviewUpload(IFormFile postedFile2, string MakerName, int Pagenumber, string CHARGE, string FTPURL, string FTPUsername, string FTPPassword, bool Status)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            int pagenumber = Pagenumber;

            string date = System.DateTime.Now.ToString("yyyyMMddHHmm");
            string FolderYear = System.DateTime.Now.ToString("yyyy");
            string FolderMonth = System.DateTime.Now.ToString("MM");


            string PDFFilePath = MakerName + "_" + CHARGE + "_" + date;
            string Filename = MakerName + "_" + CHARGE + "_" + date + ".pdf";


            string docName = Path.GetFileName(postedFile2.FileName);

            string pathDocument = Path.Combine(this.Environment.WebRootPath, "Mill sheet folder");
            string inputPDFFilePath = Path.Combine(pathDocument, docName);

            if (!Directory.Exists(pathDocument))
            {
                Directory.CreateDirectory(pathDocument);
            }
            if (Status == true)
            {
                using (FileStream stream = new FileStream(Path.Combine(pathDocument, docName), FileMode.Create))
                {
                    postedFile2.CopyTo(stream);
                }
            }
            PdfSharpCore.Pdf.PdfDocument inputPDFFile = PdfSharpCore.Pdf.IO.PdfReader.Open(inputPDFFilePath, PdfSharpCore.Pdf.IO.PdfDocumentOpenMode.Import);

            //Get the total pages in the PDF
            var totalPagesInInputPDFFile = inputPDFFile.PageCount;

            //while (totalPagesInInputPDFFile !=0)
            //{
            //Create an instance of the PDF document in memory
            PdfSharpCore.Pdf.PdfDocument outputPDFDocument = new PdfSharpCore.Pdf.PdfDocument();

            // Add a specific page to the PdfDocument instance
            outputPDFDocument.AddPage(inputPDFFile.Pages[Pagenumber - 1]);

            //save the PDF document
            SaveOutputPDF(outputPDFDocument, Filename, pathDocument);
            UploadFileToFtp(pathDocument, Filename, FolderYear, FolderMonth, FTPURL, FTPUsername, FTPPassword);
            //docName = Path.GetFileName(postedFile2.FileName);
            var filePath1 = Path.Combine(pathDocument);

            System.IO.DirectoryInfo di = new DirectoryInfo(pathDocument);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            var url = FTPURL + "/" + FolderYear + "/" + FolderMonth + "," + Filename;
            string message = url;
            return Json(new { Status = "success", Message = message });
        }


        private static void SaveOutputPDF(PdfSharpCore.Pdf.PdfDocument outputPDFDocument, string Filename, string pathDocument)
        {
            // Output file path
            string outputPDFFilePath = Path.Combine(pathDocument, Filename);

            //Save the document
            outputPDFDocument.Save(outputPDFFilePath);
        }
        public static void UploadFileToFtp(string filePath, string PDFFilePath, string folderYear, string foldermonth, string FTPURL, string FTPUsername, string FTPPassword)
        {
            var filenamepath = filePath + "\\" + PDFFilePath;
            var url = FTPURL + "/" + folderYear + "/" + foldermonth + "/" + PDFFilePath;

            FtpWebRequest reqFTP = null;
            // Stream ftpStream = null;

            string currentDir = FTPURL + "/" + folderYear + "/" + foldermonth;


            try
            {
                //create the directory
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(currentDir));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                //return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    //return true;
                }
                else
                {
                    response.Close();
                    // return false;
                }
            }



            FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(url);
            requestFTPUploader.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
            requestFTPUploader.Method = WebRequestMethods.Ftp.MakeDirectory;
            requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
            FileInfo fileInfo = new FileInfo(filenamepath);
            FileStream fileStream = fileInfo.OpenRead();
            int bufferLength = 2048;
            byte[] buffer = new byte[bufferLength];
            Stream uploadStream = requestFTPUploader.GetRequestStream();
            int contentLength = fileStream.Read(buffer, 0, bufferLength);
            while (contentLength != 0)
            {
                uploadStream.Write(buffer, 0, contentLength);
                contentLength = fileStream.Read(buffer, 0, bufferLength);
            }
            uploadStream.Close();
            fileStream.Close();
            requestFTPUploader = null;


        }

        public JsonResult PreviewShowPDF(string PDFpath, string filename, int Pagenumber, string FTPURL, string FTPUsername, string FTPPassword)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            int pagenumber = Pagenumber;

            var url = PDFpath + "/" + filename;


            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
            request.KeepAlive = false;
            request.UsePassive = false;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            FileStream writeStream = new FileStream(wwwPath + "/" + "Mill Sheet Folder" + "/" + filename, FileMode.Create);
            int Length = 2048;
            Byte[] buffer = new Byte[Length];
            int bytesRead = responseStream.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = responseStream.Read(buffer, 0, Length);
            }
            writeStream.Close();
            response.Close();



            string message = "PDFFilePath";
            return Json(new { Status = "success", Message = message });
        }

        public JsonResult UploadedFileDeleted(string PDFpath, string filename, int Pagenumber, string FTPURL, string FTPUsername, string FTPPassword)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            int pagenumber = Pagenumber;

            var url = PDFpath + "/" + filename + "." + "pdf";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.UseBinary = true;
            //request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
            request.KeepAlive = false;
            request.UsePassive = false;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            // FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();

            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            //request.UseBinary = true;
            //request.Method = WebRequestMethods.Ftp.DownloadFile;
            //request.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
            //request.KeepAlive = false;
            //request.UsePassive = false;
            //FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //Stream responseStream = response.GetResponseStream();
            //FileStream writeStream = new FileStream(wwwPath + "/" +"Mill Sheet Folder"+"/" + filename, FileMode.Create);
            //int Length = 2048;
            //Byte[] buffer = new Byte[Length];
            //int bytesRead = responseStream.Read(buffer, 0, Length);
            //while (bytesRead > 0)
            //{
            //    writeStream.Write(buffer, 0, bytesRead);
            //    bytesRead = responseStream.Read(buffer, 0, Length);
            //}
            //writeStream.Close();
            //response.Close();



            string message = "PDFFilePath";
            return Json(new { Status = "success", Message = message });
        }

        public Bitmap ResizeImage(Bitmap original, int width)
        {
            int height = (int)(original.Height * (width / (float)original.Width));
            Bitmap resized = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, 0, 0, width, height);
            }
            return resized;
        }

        public async Task<IActionResult> GetPDFBoxLayout([FromForm] IFormFile file, int makerCD, string templateName)
        {
            string filePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet");
            string templatePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MillsheetTemplate");
            string outputPath = string.Empty;
            string outputPath1 = Path.Combine(filePath, "output.png");
            string fileName = string.Empty;

            MakerMillSheetConfiguration _sheetconfiguration = new MakerMillSheetConfiguration();

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!Directory.Exists(templatePath))
            {
                Directory.CreateDirectory(templatePath);
            }

            fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                file.CopyTo(stream);

            }
            using (FileStream streamTemplate = new FileStream(Path.Combine(templatePath, templateName + ".pdf"), FileMode.Create))
            {
                file.CopyTo(streamTemplate);

            }

            using (var document = PdfiumViewer.PdfDocument.Load(@filePath + "\\" + @fileName))
            {
                var pageSize = document.PageSizes[0];
                int width = (int)(pageSize.Width / 72.0 * 400);
                int height = (int)(pageSize.Height / 72.0 * 400);
                outputPath = Path.Combine(filePath, "MillSheet.png");
                var image = document.Render(0, width, height, 400, 400, PdfRenderFlags.Annotations);
                image.Save(outputPath, ImageFormat.Png);
            }

            Mat src = Cv2.ImRead(outputPath, ImreadModes.Grayscale);

            // Step 1: Invert and binarize the image
            Mat binary = new Mat();
            Cv2.AdaptiveThreshold(~src, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 15, -2);

            // Step 2: Extract horizontal lines
            Mat horizontal = binary.Clone();
            int horizontalSize = horizontal.Cols / 30;  // Tune this value for your layout
            Mat horizontalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(horizontalSize, 1));
            Cv2.Erode(horizontal, horizontal, horizontalStructure);
            Cv2.Dilate(horizontal, horizontal, horizontalStructure);

            // Step 3: Extract vertical lines
            Mat vertical = binary.Clone();
            int verticalSize = vertical.Rows / 30;  // Tune this too
            Mat verticalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, verticalSize));
            Cv2.Erode(vertical, vertical, verticalStructure);
            Cv2.Dilate(vertical, vertical, verticalStructure);

            // Step 4: Combine lines
            Mat mask = new Mat();
            Cv2.Add(horizontal, vertical, mask);

            // Step 5: Find contours (table boxes)
            Cv2.FindContours(mask, out Point[][] contours, out HierarchyIndex[] hierarchy,
                RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // Step 6: Draw rectangles for visualization
            Mat output = Cv2.ImRead(outputPath); // Read original in color
            foreach (var cnt in contours)
            {
                var rect = Cv2.BoundingRect(cnt);

                if (rect.Width > 500 && rect.Height > 500) // 1000 & 800
                {


                    var topLeft = (rect.X, rect.Y);
                    var topRight = (rect.X + rect.Width, rect.Y);
                    var bottomLeft = (rect.X, rect.Y + rect.Height);
                    var bottomRight = (rect.X + rect.Width, rect.Y + rect.Height);

                    if (topLeft.X > 0)
                    {
                        _sheetconfiguration = new MakerMillSheetConfiguration();
                        _sheetconfiguration.TOP_LEFT_X = topLeft.X;
                        _sheetconfiguration.TOP_LEFT_Y = topLeft.Y;

                        _sheetconfiguration.TOP_RIGHT_X = topRight.Item1;
                        _sheetconfiguration.TOP_RIGHT_Y = topRight.Y;

                        //_sheetconfiguration.END_X1 = bottomLeft.X;
                        //_sheetconfiguration.END_Y1 = bottomLeft.Item2;

                        //_sheetconfiguration.END_X2 = bottomRight.Item1;
                        //_sheetconfiguration.END_Y2 = bottomRight.Item2;

                        _sheetconfiguration.BOTTOM_LEFT_X = bottomLeft.X;
                        _sheetconfiguration.BOTTOM_LEFT_Y = bottomLeft.Item2;

                        _sheetconfiguration.BOTTOM_RIGHT_X = bottomRight.Item1;
                        _sheetconfiguration.BOTTOM_RIGHT_Y = bottomRight.Item2;

                         Cv2.Rectangle(output, rect, Scalar.Red, 2);
                    }

                }
            }
            
            if (_sheetconfiguration != null)
            {
                int boxHeight = Convert.ToInt32(_sheetconfiguration.BOTTOM_LEFT_Y - _sheetconfiguration.TOP_LEFT_Y);
                if (boxHeight < 1000)
                {
                    var result = CaptureLargeScaleBox(outputPath);
                    if (result != null)
                    {
                        _sheetconfiguration = new MakerMillSheetConfiguration();
                        _sheetconfiguration.TOP_LEFT_X = result.TOP_LEFT_X;
                        _sheetconfiguration.TOP_LEFT_Y = result.TOP_LEFT_Y;

                        _sheetconfiguration.TOP_RIGHT_X = result.TOP_RIGHT_X;
                        _sheetconfiguration.TOP_RIGHT_Y = result.TOP_RIGHT_Y;

                        _sheetconfiguration.BOTTOM_LEFT_X = result.BOTTOM_LEFT_X;
                        _sheetconfiguration.BOTTOM_LEFT_Y = result.BOTTOM_LEFT_Y;

                        _sheetconfiguration.BOTTOM_RIGHT_X = result.BOTTOM_RIGHT_X;
                        _sheetconfiguration.BOTTOM_RIGHT_Y = result.BOTTOM_RIGHT_Y;
                    }
                }
            }
            
            return Ok(new { message = _sheetconfiguration });
        }
        public async Task<IActionResult> GetPDFData([FromForm] IFormFile file, [FromForm] string details, int pageNo) //IFormFile file, 
        {
            int totalPages = 0;
            string filePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet");
            List<MakerMillSheetOcrDetails> ocrResult = new List<MakerMillSheetOcrDetails>();
            string fileName = string.Empty;
            string returnFile = string.Empty;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            foreach (var deleteFile in Directory.GetFiles(filePath))
            {
                System.IO.File.Delete(deleteFile);
            }

            var pdfFieldItems = JsonConvert.DeserializeObject<List<MakerMillSheetConfiguration>>(details);

            fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                file.CopyTo(stream);

            }


            using (var document = PdfiumViewer.PdfDocument.Load(@filePath + "\\" + @fileName))
            {

                int pageCount = document.PageCount; // Render the first page (index starts from 0)

                totalPages = document.PageCount;

                MakerMillSheetDetails result = new MakerMillSheetDetails();

                int dpi = 400;
                var pageSize = document.PageSizes[pageNo - 1];
                int width = (int)(pageSize.Width / 72.0 * dpi);
                int height = (int)(pageSize.Height / 72.0 * dpi);
                string outputPath = Path.Combine(filePath, "MillSheet.png");
                var image = document.Render(pageNo - 1, width, height, dpi, dpi, PdfRenderFlags.Annotations);
                image.Save(outputPath, ImageFormat.Png);

                //using (var highDpiImage = (Bitmap)document.Render(page, 300, 300, true))
                //{
                //    using (var resized = ResizeImage(highDpiImage, 2000))
                //    {
                //        string outputPath = Path.Combine(filePath, "MillSheet.png");
                //        resized.Save(outputPath, ImageFormat.Png);
                //    }
                //}

                string responseData = await GetOCRData(filePath + "\\MillSheet.png");  // get PDF CUSTOMER and ORDER X and Y position to get actual value for each field

                if (!string.IsNullOrEmpty(responseData))
                {
                    result = SetVisionData(pdfFieldItems, responseData, filePath);
                }

                ocrResult.Add(new MakerMillSheetOcrDetails { PAGE_NUMBER = pageNo, MAKER_DATA = result });

            }
            return Ok(new { PageCount = totalPages, Message = ocrResult });


        }

        public static async Task<string> GetOCRData(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64Image = Convert.ToBase64String(imageBytes);//DOCUMENT_TEXT_DETECTION

            string jsonRequest = $@"
        {{
            ""requests"": [
                {{
                    ""image"": {{
                        ""content"": ""{base64Image}""
                    }},
                    ""features"": [
                        {{
                            ""type"": ""DOCUMENT_TEXT_DETECTION"", 
                            ""maxResults"": 5
                        }}
                    ],
""imageContext"": {{
        ""languageHints"": [""en""],
        ""textDetectionParams"": {{
          ""enableTextDetectionConfidenceScore"": true
        }}
      }}
                }}
            ]
        }}";

            using var client = new HttpClient();
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            string url = $"https://vision.googleapis.com/v1/images:annotate?key={apiKey}";

            HttpResponseMessage response = await client.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();



            return responseBody;

        }
        public static bool IsAllDigits(string text) // . allowed
        {
           // return Regex.IsMatch(s, @"^-?$|^-?\d+(\.\d+)?$");
            return Regex.IsMatch(text, @"^-?$|^\.$|^-?\d+(\.\d+)?$"); //^-?$ only "-" OR empty minus, ^-?\d+(\.\d+)?$ normal number (int or decimal with optional minus)
        }
        public static bool IsAllNumeric(string text) // . not allowed
        {
            
            return Regex.IsMatch(text, @"^-?$|^-?\d+(\.\d+)?$"); //^-?$ only "-" OR empty minus, ^-?\d+(\.\d+)?$ normal number (int or decimal with optional minus)
        }
       public static string ModifyValue(string textValue)
        {
            
            textValue = textValue.Trim();

            // MIN / MAX with any extra characters after them
            if (textValue.ToUpper().StartsWith("MI", StringComparison.OrdinalIgnoreCase))
                return "MIN";

            if (textValue.ToUpper().StartsWith("MA", StringComparison.OrdinalIgnoreCase))
                return "MAX";

            // Keep only digits and .
            string value = Regex.Replace(textValue, @"[^0-9.]", "");

            // Remove dots from beginning/end
            value = value.Trim('.');

            // Keep only one decimal point
            int firstDot = value.IndexOf('.');

            if (firstDot >= 0)
            {
                string beforeDot = value.Substring(0, firstDot);
                string afterDot = value.Substring(firstDot + 1).Replace(".", "");

                value = beforeDot + "." + afterDot;
            }

            return value;
        }
        public static MakerMillSheetDetails SetVisionData(List<MakerMillSheetConfiguration> request, string response, string strPath)
        {
            string CCNameKey = "7,12,17,22,27,32,37,42,47,52,57,62,67,72,77,82,87,92,97,102,107,112,117,122,127";
            string CCRangeKey = "8,13,18,23,28,33,38,43,48,53,58,63,68,73,78,83,88,93,98,103,108,113,118,123,128";
            string CCMinKey = "9,14,19,24,29,34,39,44,49,54,59,64,69,74,79,84,89,94,99,104,109,114,119,124,129";
            string CCMaxKey = "10,15,20,25,30,35,40,45,50,55,60,65,70,75,80,85,90,95,100,105,110,115,120,125,130";
            string hardRangeKey = "132,136,140,144,148,152,156,160,164,168,172,176,180,184,188,192,196,200,204,208,212,216,220,224,228";
            string hardMinKey = "133,137,141,145,149,153,157,161,165,169,173,177,181,185,189,193,197,201,205,209,213,217,221,225,229";
            string hardMaxKey = "134,138,142,146,150,154,158,162,166,170,174,178,182,186,190,194,198,202,206,210,214,218,222,226,230";
            decimal currentHeight = 0;
            decimal currentWidth = 0;
            decimal topX1_Current = 0;
            decimal topY1_Current = 0;
            decimal topX2_Current = 0;
            decimal topY2_Current = 0;
            decimal bottomX1_Current = 0;
            decimal bottomY1_Current = 0;
            decimal bottomX2_Current = 0;
            decimal bottomY2_Current = 0;

            decimal originalHeight = 0;
            decimal originalWidth = 0;
            decimal topX1_Original = 0;
            decimal topY1_Original = 0;
            decimal topX2_Original = 0;
            decimal topY2_Original = 0;
            decimal bottomX1_Original = 0;
            decimal bottomY1_Original = 0;
            decimal bottomX2_Original = 0;
            decimal bottomY2_Original = 0;

            decimal topLeft_X = 0;
            decimal topLeft_Y = 0;
            decimal topRight_X = 0;
            decimal topRight_Y = 0;
            decimal bottomLeft_X = 0;
            decimal bottomLeft_Y = 0;
            decimal bottomRight_X = 0;
            decimal bottomRight_Y = 0;

            var result = JsonConvert.DeserializeObject<GoogleTextDetection>(response);

            MakerMillSheetDetails details = new MakerMillSheetDetails();

            var textWords = result.Responses[0].TextAnnotations;
            var fullTextWords = result.Responses[0].FullTextAnnotation.Pages.SelectMany(p => p.Blocks).SelectMany(b => b.Paragraphs).SelectMany(p => p.Words).SelectMany(w => w.Symbols).ToList();
            

            string imgPath = Path.Combine(strPath, "MillSheet.png");

            Mat src = Cv2.ImRead(imgPath, ImreadModes.Grayscale);

            // Step 1: Invert and binarize the image
            Mat binary = new Mat();
            Cv2.AdaptiveThreshold(~src, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 15, -2);

            // Step 2: Extract horizontal lines
            Mat horizontal = binary.Clone();
            int horizontalSize = horizontal.Cols / 30;  // Tune this value for your layout
            Mat horizontalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(horizontalSize, 1));
            Cv2.Erode(horizontal, horizontal, horizontalStructure);
            Cv2.Dilate(horizontal, horizontal, horizontalStructure);

            // Step 3: Extract vertical lines
            Mat vertical = binary.Clone();
            int verticalSize = vertical.Rows / 30;  // Tune this too
            Mat verticalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, verticalSize));
            Cv2.Erode(vertical, vertical, verticalStructure);
            Cv2.Dilate(vertical, vertical, verticalStructure);

            // Step 4: Combine lines
            Mat mask = new Mat();
            Cv2.Add(horizontal, vertical, mask);

            // Step 5: Find contours (table boxes)
            Cv2.FindContours(mask, out Point[][] contours, out HierarchyIndex[] hierarchy,
                RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // Step 6: Draw rectangles for visualization
            Mat output = Cv2.ImRead(imgPath); // Read original in color
            foreach (var cnt in contours)
            {
                var rect = Cv2.BoundingRect(cnt);


                if (rect.Width > 500 && rect.Height > 500) // 1000 & 800
                {
                    var topLeft = (rect.X, rect.Y);
                    var topRight = (rect.X + rect.Width, rect.Y);
                    var bottomLeft = (rect.X, rect.Y + rect.Height);
                    var bottomRight = (rect.X + rect.Width, rect.Y + rect.Height);

                    if (topLeft.X > 0)
                    {
                        topX1_Current = topLeft.X;
                        topY1_Current = topLeft.Y;

                        topX2_Current = topRight.Item1;
                        topY2_Current = topRight.Y;

                        bottomX1_Current = bottomLeft.X;
                        bottomY1_Current = bottomLeft.Item2;

                        bottomX2_Current = bottomRight.Item1;
                        bottomY2_Current = bottomRight.Item2;

                        currentWidth = bottomX2_Current - topX1_Current;
                        currentHeight = bottomY2_Current - topY1_Current;
                       // Cv2.Rectangle(output, rect, Scalar.Red, 2);
                    }

                }
            }
            // Cv2.ImWrite(strPath+"\\debug_tables.png", output);
            if (topY1_Current > 0 && bottomY1_Current > 0)
            {
                int checkHeight = Convert.ToInt32(bottomY1_Current - topY1_Current);
                if (checkHeight < 1000)
                {
                    var resultData = CaptureLargeScaleBox(imgPath);
                    if (resultData != null)
                    {
                        topX1_Current = resultData.TOP_LEFT_X;
                        topY1_Current = resultData.TOP_LEFT_Y;

                        topX2_Current = resultData.TOP_RIGHT_X;
                        topY2_Current = resultData.TOP_RIGHT_Y;

                        bottomX1_Current = resultData.BOTTOM_LEFT_X;
                        bottomY1_Current = resultData.BOTTOM_LEFT_Y;

                        bottomX2_Current = resultData.BOTTOM_RIGHT_X;
                        bottomY2_Current = resultData.BOTTOM_RIGHT_Y;

                        currentWidth = bottomX2_Current - topX1_Current;
                        currentHeight = bottomY2_Current - topY1_Current;
                    }
                }
            }
            

            string textValue = string.Empty;
            var originalData = request.AsEnumerable().Where(w => w.FIELD_CD == 999).Select(w => w).FirstOrDefault();
            if (originalData != null)
            {
                topX1_Original = originalData.TOP_LEFT_X;
                topY1_Original = originalData.TOP_LEFT_Y;
                topX2_Original = originalData.TOP_RIGHT_X;
                topY2_Original = originalData.TOP_RIGHT_Y;

                //bottomX1_Original = originalData.END_X1;
                //bottomY1_Original = originalData.END_Y1;

                //bottomX2_Original = originalData.END_X2;
                //bottomY2_Original = originalData.END_Y2;

                //originalWidth = originalData.END_X2 - originalData.TOP_LEFT_X;
                //originalHeight = originalData.END_Y2 - originalData.TOP_LEFT_Y;
                bottomX1_Original = originalData.BOTTOM_LEFT_X;
                bottomY1_Original = originalData.BOTTOM_LEFT_Y;

                bottomX2_Original = originalData.BOTTOM_RIGHT_X;
                bottomY2_Original = originalData.BOTTOM_RIGHT_Y;

                originalWidth = originalData.BOTTOM_RIGHT_X - originalData.TOP_LEFT_X;
                originalHeight = originalData.BOTTOM_RIGHT_Y - originalData.TOP_LEFT_Y;

            }

            foreach (var item in request)
            {
                textValue = string.Empty;
                if (item.FIELD_CD != 999)
                {

                    decimal relX = (item.TOP_LEFT_X - topX1_Original) / originalWidth;
                    decimal relY = (item.TOP_LEFT_Y - topY1_Original) / originalHeight;

                    int topX1 = Convert.ToInt32(topX1_Current + relX * currentWidth);
                    int topY1 = Convert.ToInt32(topY1_Current + relY * currentHeight);

                    relX = (item.TOP_RIGHT_X - topX1_Original) / originalWidth;
                    relY = (item.TOP_RIGHT_Y - topY1_Original) / originalHeight;

                    int topX2 = Convert.ToInt32(topX1_Current + relX * currentWidth);
                    int topY2 = Convert.ToInt32(topY1_Current + relY * currentHeight);

                    //relY = (item.END_Y1 - topY1_Original) / originalHeight;
                    relY = (item.BOTTOM_LEFT_Y - topY1_Original) / originalHeight;
                    int bottomX1 = topX2;
                    int bottomY1 = Convert.ToInt32(topY1_Current + relY * currentHeight);


                    //relY = (item.END_Y2 - topY2_Original) / originalHeight;
                    relY = (item.BOTTOM_RIGHT_Y - topY2_Original) / originalHeight;
                    int bottomX2 = topX1;
                    int bottomY2 = Convert.ToInt32(topY2_Current + relY * currentHeight);

                    topLeft_X = topX1;
                    topLeft_Y = topY1;
                    topRight_X = topX2;
                    topRight_Y = topY2;

                    bottomLeft_X = bottomX2;
                    bottomLeft_Y = bottomY1;
                    bottomRight_X = bottomX1;
                    bottomRight_Y = bottomY2;

                    bool isCCNameHeader = CCNameKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));
                    bool isCCRange = CCRangeKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));
                    bool isCCMin = CCMinKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));
                    bool isCCMax = CCMaxKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));

                    bool isHardRange = hardRangeKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));
                    bool isHardMin = hardMinKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));
                    bool isHardMax = hardMaxKey.Split(',').Select(int.Parse).Contains(Convert.ToInt32(item.FIELD_CD));

                    if (item.FIELD_CD == 1) //  STEEL TYPE
                    {
                        var listDetails = textWords.AsEnumerable().Where(w =>
                       w.BoundingPoly.Vertices[0].X >= topLeft_X && w.BoundingPoly.Vertices[0].X <= topRight_X && w.BoundingPoly.Vertices[0].Y >= topLeft_Y && w.BoundingPoly.Vertices[0].Y <= bottomRight_Y
                       && w.BoundingPoly.Vertices[1].X >= topLeft_X && w.BoundingPoly.Vertices[1].X <= topRight_X && w.BoundingPoly.Vertices[1].Y >= topLeft_Y && w.BoundingPoly.Vertices[1].Y <= bottomRight_Y
                       && w.BoundingPoly.Vertices[2].X >= bottomLeft_X && w.BoundingPoly.Vertices[2].X <= bottomRight_X && w.BoundingPoly.Vertices[2].Y >= topLeft_Y && w.BoundingPoly.Vertices[2].Y <= bottomRight_Y
                       && w.BoundingPoly.Vertices[3].X >= bottomLeft_X && w.BoundingPoly.Vertices[3].X <= bottomRight_X && w.BoundingPoly.Vertices[3].Y >= topLeft_Y && w.BoundingPoly.Vertices[3].Y <= bottomRight_Y
                        ).Select(w => w.Description).ToList();

                        if (listDetails.Any())
                        {
                            textValue = string.Join(" ", listDetails).Replace("$", "S");
                        }


                    }
                    else if (item.FIELD_CD == 2) //  CHARGE
                    {
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                           w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                           && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                           && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                           && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                           ).Select(w => w.Text).ToList());

                        if (!string.IsNullOrEmpty(textValue))
                        {
                            textValue = textValue.Replace("$", "S");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }

                    }
                    else if (isCCNameHeader == true) // CC NAME HEADER
                    {
                        // var listDetails = words.AsEnumerable().Where(w =>
                        //w.BoundingPoly.Vertices[0].X >= topLeft_X && w.BoundingPoly.Vertices[0].X <= topRight_X && w.BoundingPoly.Vertices[0].Y >= topLeft_Y && w.BoundingPoly.Vertices[0].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[1].X >= topLeft_X && w.BoundingPoly.Vertices[1].X <= topRight_X && w.BoundingPoly.Vertices[1].Y >= topLeft_Y && w.BoundingPoly.Vertices[1].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[2].X >= bottomLeft_X && w.BoundingPoly.Vertices[2].X <= bottomRight_X && w.BoundingPoly.Vertices[2].Y >= topLeft_Y && w.BoundingPoly.Vertices[2].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[3].X >= bottomLeft_X && w.BoundingPoly.Vertices[3].X <= bottomRight_X && w.BoundingPoly.Vertices[3].Y >= topLeft_Y && w.BoundingPoly.Vertices[3].Y <= bottomRight_Y
                        // ).Select(w => w.Description).ToList();

                        // if (listDetails.Any())
                        // {
                        //     textValue = string.Join(" ", listDetails).Replace(" ", "").Replace("5", "S").Replace("Ni +", "Ni+Cr").Replace("NI+C", "Ni+Cr").Replace("NI+", "Ni+Cr").Replace("+Cr", "Ni+Cr").Replace("+ Cr", "Ni+Cr"); 
                        // }
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                          w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                          && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                          && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                          && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                          ).Select(w => w.Text).ToList());

                        if (!string.IsNullOrEmpty(textValue))
                        {
                            textValue = textValue.Replace(" ", "").Replace("5", "S").Replace("Ni +", "Ni+Cr").Replace("NI+C", "Ni+Cr").Replace("NI+", "Ni+Cr").Replace("+Cr", "Ni+Cr").Replace("+ Cr", "Ni+Cr").Replace("|", "");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }
                    }
                    else if (isCCRange == true) // CC RANGE
                    {
                        //  var listDetails = words.AsEnumerable().Where(w =>
                        //w.BoundingPoly.Vertices[0].X >= topLeft_X && w.BoundingPoly.Vertices[0].X <= topRight_X && w.BoundingPoly.Vertices[0].Y >= topLeft_Y && w.BoundingPoly.Vertices[0].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[1].X >= topLeft_X && w.BoundingPoly.Vertices[1].X <= topRight_X && w.BoundingPoly.Vertices[1].Y >= topLeft_Y && w.BoundingPoly.Vertices[1].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[2].X >= bottomLeft_X && w.BoundingPoly.Vertices[2].X <= bottomRight_X && w.BoundingPoly.Vertices[2].Y >= topLeft_Y && w.BoundingPoly.Vertices[2].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[3].X >= bottomLeft_X && w.BoundingPoly.Vertices[3].X <= bottomRight_X && w.BoundingPoly.Vertices[3].Y >= topLeft_Y && w.BoundingPoly.Vertices[3].Y <= bottomRight_Y
                        // ).Select(w => w.Description).ToList();
                        //  if (listDetails.Any())
                        //  {
                        //      textValue = string.Join(" ", listDetails).Replace(" ", "").Replace("%", "").Replace("x", "").Replace("X", "");
                        //      if (textValue.StartsWith("."))
                        //      {
                        //          textValue = textValue.Substring(1);
                        //      }
                        //  }
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                         w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                         && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                         && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                         && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                         ).Select(w => w.Text).ToList());

                        textValue = ModifyValue(textValue);
                        if (!string.IsNullOrEmpty(textValue))
                        {
                            textValue = textValue.Replace(" ", "").Replace("%", "").Replace("x", "").Replace("X", "");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }
                    }
                    else if (isHardRange == true) // HARDNESS RANGE
                    {
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                           w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                           && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                           && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                           && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                           ).Select(w => w.Text).ToList());

                        textValue = ModifyValue(textValue);
                        if (!string.IsNullOrEmpty(textValue) && IsAllDigits(textValue) == true)
                        {
                            textValue = textValue.Replace("-", "");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }
                    }
                    else if (isCCMin == true) // CC MIN
                    {
                        //     var detailsList = words.AsEnumerable().Where(w =>
                        //w.BoundingPoly.Vertices[0].X >= topLeft_X && w.BoundingPoly.Vertices[0].X <= topRight_X && w.BoundingPoly.Vertices[0].Y >= topLeft_Y && w.BoundingPoly.Vertices[0].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[1].X >= topLeft_X && w.BoundingPoly.Vertices[1].X <= topRight_X && w.BoundingPoly.Vertices[1].Y >= topLeft_Y && w.BoundingPoly.Vertices[1].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[2].X >= bottomLeft_X && w.BoundingPoly.Vertices[2].X <= bottomRight_X && w.BoundingPoly.Vertices[2].Y >= topLeft_Y && w.BoundingPoly.Vertices[2].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[3].X >= bottomLeft_X && w.BoundingPoly.Vertices[3].X <= bottomRight_X && w.BoundingPoly.Vertices[3].Y >= topLeft_Y && w.BoundingPoly.Vertices[3].Y <= bottomRight_Y
                        //&& (IsAllDigits(w.Description) == true || (w.Description.StartsWith("MA") || w.Description.StartsWith("MI") || w.Description.StartsWith("NA")))
                        //).Select(w => w.Description).ToList();
                        //     if (detailsList.Any())
                        //     {
                        //         textValue = string.Join("", detailsList).Replace("-","");
                        //         if (textValue.StartsWith("."))
                        //         {
                        //             textValue = textValue.Substring(1);
                        //         }
                        //     }
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                            w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                            && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                            && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                            && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                            ).Select(w => w.Text).ToList());
                        textValue = ModifyValue(textValue);
                        textValue = textValue.Replace(".", "").Replace(",", "");
                        if (!string.IsNullOrEmpty(textValue) && (IsAllNumeric(textValue) == true || textValue.StartsWith("MAX") || textValue.StartsWith("MIN")))
                        {
                            textValue = textValue.Replace("-", "");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }
                    }
                    else if (isHardMin == true) // HARDNESS MIN
                    {
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                            w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                            && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                            && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                            && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                            ).Select(w => w.Text).ToList());

                        textValue = ModifyValue(textValue);
                        if (!string.IsNullOrEmpty(textValue) && (IsAllDigits(textValue) == true || textValue.StartsWith("MAX") || textValue.StartsWith("MIN")))
                        {
                            textValue = textValue.Replace("-", "");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }
                    }
                    else if (isCCMax == true) // CC MAX
                    {
                        //   var detailsList = words.AsEnumerable().Where(w =>
                        //w.BoundingPoly.Vertices[0].X >= topLeft_X && w.BoundingPoly.Vertices[0].X <= topRight_X && w.BoundingPoly.Vertices[0].Y >= topLeft_Y && w.BoundingPoly.Vertices[0].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[1].X >= topLeft_X && w.BoundingPoly.Vertices[1].X <= topRight_X && w.BoundingPoly.Vertices[1].Y >= topLeft_Y && w.BoundingPoly.Vertices[1].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[2].X >= bottomLeft_X && w.BoundingPoly.Vertices[2].X <= bottomRight_X && w.BoundingPoly.Vertices[2].Y >= topLeft_Y && w.BoundingPoly.Vertices[2].Y <= bottomRight_Y
                        //&& w.BoundingPoly.Vertices[3].X >= bottomLeft_X && w.BoundingPoly.Vertices[3].X <= bottomRight_X && w.BoundingPoly.Vertices[3].Y >= topLeft_Y && w.BoundingPoly.Vertices[3].Y <= bottomRight_Y
                        //&& (IsAllDigits(w.Description) == true || (w.Description.StartsWith("MA") || w.Description.StartsWith("MI") || w.Description.StartsWith("NA")))
                        //).Select(w => w.Description).ToList();
                        //   if (detailsList.Any())
                        //   {
                        //       textValue = string.Join("", detailsList).Replace("-", "");
                        //       if (textValue.StartsWith("."))
                        //       {
                        //           textValue = textValue.Substring(1);
                        //       }
                        //   }

                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                        w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                        ).Select(w => w.Text).ToList());
                        textValue = ModifyValue(textValue);
                        textValue = textValue.Replace(".", "").Replace(",", "");
                        if (!string.IsNullOrEmpty(textValue) && (IsAllNumeric(textValue) == true || textValue.StartsWith("MAX") || textValue.StartsWith("MIN")))
                        {
                            textValue = textValue.Replace("-", "");
                            textValue = textValue.Trim('.');
                        }
                        else
                        {
                            textValue = string.Empty;
                        }

                    }
                    else if (isHardMax == true) // HARDNESS MAX
                    {
                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                        w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                        ).Select(w => w.Text).ToList());
                        textValue = ModifyValue(textValue);
                        if (!string.IsNullOrEmpty(textValue) && (IsAllDigits(textValue) == true || textValue.StartsWith("MAX") || textValue.StartsWith("MIN")))
                        {
                            textValue = textValue.Replace("-", "");
                            textValue = textValue.Trim('.');

                        }
                        else
                        {
                            textValue = string.Empty;
                        }

                    }
                    else
                    {
                        //textValue = words.AsEnumerable().Where(w =>
                        // w.BoundingPoly.Vertices[0].X >= topLeft_X && w.BoundingPoly.Vertices[0].X <= topRight_X && w.BoundingPoly.Vertices[0].Y >= topLeft_Y && w.BoundingPoly.Vertices[0].Y <= bottomRight_Y
                        // && w.BoundingPoly.Vertices[1].X >= topLeft_X && w.BoundingPoly.Vertices[1].X <= topRight_X && w.BoundingPoly.Vertices[1].Y >= topLeft_Y && w.BoundingPoly.Vertices[1].Y <= bottomRight_Y
                        // && w.BoundingPoly.Vertices[2].X >= bottomLeft_X && w.BoundingPoly.Vertices[2].X <= bottomRight_X && w.BoundingPoly.Vertices[2].Y >= topLeft_Y && w.BoundingPoly.Vertices[2].Y <= bottomRight_Y
                        // && w.BoundingPoly.Vertices[3].X >= bottomLeft_X && w.BoundingPoly.Vertices[3].X <= bottomRight_X && w.BoundingPoly.Vertices[3].Y >= topLeft_Y && w.BoundingPoly.Vertices[3].Y <= bottomRight_Y
                        // && IsAllDigits(w.Description) == true
                        // ).Select(w => w.Description).FirstOrDefault();

                        textValue = string.Join("", fullTextWords.AsEnumerable().Where(w =>
                        w.BoundingBox.Vertices[0].X >= topLeft_X && w.BoundingBox.Vertices[0].X <= topRight_X && w.BoundingBox.Vertices[0].Y >= topLeft_Y && w.BoundingBox.Vertices[0].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[1].X >= topLeft_X && w.BoundingBox.Vertices[1].X <= topRight_X && w.BoundingBox.Vertices[1].Y >= topLeft_Y && w.BoundingBox.Vertices[1].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[2].X >= bottomLeft_X && w.BoundingBox.Vertices[2].X <= bottomRight_X && w.BoundingBox.Vertices[2].Y >= topLeft_Y && w.BoundingBox.Vertices[2].Y <= bottomRight_Y
                        && w.BoundingBox.Vertices[3].X >= bottomLeft_X && w.BoundingBox.Vertices[3].X <= bottomRight_X && w.BoundingBox.Vertices[3].Y >= topLeft_Y && w.BoundingBox.Vertices[3].Y <= bottomRight_Y
                        && IsAllNumeric(w.Text) == true).Select(w => w.Text).ToList());
                        textValue = ModifyValue(textValue);
                        if (!string.IsNullOrEmpty(textValue))
                        {
                            textValue = textValue.Replace("-", "");
                            textValue = textValue.Trim('.'); // remove leading and trailing dots
                        }

                    }
                   
                    switch (item.FIELD_CD)
                    {
                        case 1: // Steel Type
                            details.MATL_NAME_MAKER = textValue != string.Empty ? textValue.Replace(": ", "").Replace(":", "") : string.Empty;
                            break;
                        case 2: // Charge
                            details.CHARGE = textValue;
                            break;
                        case 3: // Dimension
                            details.DIM_D = textValue != string.Empty && Convert.ToDecimal(textValue) > 0 ? Convert.ToDecimal(textValue) : null;
                            break;
                        case 4: // Shape
                            details.SHAPE = textValue;
                            break;
                        case 5: // No of Units
                            details.NO_OF_UNITS = textValue != string.Empty && Convert.ToDecimal(textValue) > 0 ? Convert.ToDecimal(textValue) : null;
                            break;
                        case 6: // Weight
                            details.QUANTITY = textValue != string.Empty && Convert.ToDecimal(textValue) > 0 ? Convert.ToDecimal(textValue) : null;
                            break;
                        case 7: // Chemical Header1
                            details.CCNAME1 = textValue;
                            break;
                        case 12: // Chemical Header2
                            details.CCNAME2 = textValue;
                            break;
                        case 17: // Chemical Header3
                            details.CCNAME3 = textValue;
                            break;
                        case 22: // Chemical Header4
                            details.CCNAME4 = textValue;
                            break;
                        case 27: // Chemical Header5
                            details.CCNAME5 = textValue;
                            break;
                        case 32: // Chemical Header6
                            details.CCNAME6 = textValue;
                            break;
                        case 37: // Chemical Header7
                            details.CCNAME7 = textValue;
                            break;
                        case 42: // Chemical Header8
                            details.CCNAME8 = textValue;
                            break;
                        case 47: // Chemical Header9
                            details.CCNAME9 = textValue;
                            break;
                        case 52: // Chemical Header10
                            details.CCNAME10 = textValue;
                            break;
                        case 57: // Chemical Header11
                            details.CCNAME11 = textValue;
                            break;
                        case 62: // Chemical Header12
                            details.CCNAME12 = textValue;
                            break;
                        case 67: // Chemical Header13
                            details.CCNAME13 = textValue;
                            break;
                        case 72: // Chemical Header14
                            details.CCNAME14 = textValue;
                            break;
                        case 77: // Chemical Header15
                            details.CCNAME15 = textValue;
                            break;
                        case 82: // Chemical Header16
                            details.CCNAME16 = textValue;
                            break;
                        case 87: // Chemical Header17
                            details.CCNAME17 = textValue;
                            break;
                        case 92: // Chemical Header18
                            details.CCNAME18 = textValue;
                            break;
                        case 97: // Chemical Header19
                            details.CCNAME19 = textValue;
                            break;
                        case 102: // Chemical Header20
                            details.CCNAME20 = textValue;
                            break;
                        case 107: // Chemical Header21
                            details.CCNAME21 = textValue;
                            break;
                        case 112: // Chemical Header22
                            details.CCNAME22 = textValue;
                            break;
                        case 117: // Chemical Header23
                            details.CCNAME23 = textValue;
                            break;
                        case 122: // Chemical Header24
                            details.CCNAME24 = textValue;
                            break;
                        case 127: // Chemical Header25
                            details.CCNAME25 = textValue;
                            break;
                        case 8: // Chemical Element1
                            details.CC_CHEMICAL1_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 13: // Chemical Element2
                            details.CC_CHEMICAL2_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 18: // Chemical Element3
                            details.CC_CHEMICAL3_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 23: // Chemical Element4
                            details.CC_CHEMICAL4_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 28: // Chemical Element5
                            details.CC_CHEMICAL5_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 33: // Chemical Element6
                            details.CC_CHEMICAL6_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 38: // Chemical Element7
                            details.CC_CHEMICAL7_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 43: // Chemical Element8
                            details.CC_CHEMICAL8_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 48: // Chemical Element9
                            details.CC_CHEMICAL9_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;  
                        case 53: // Chemical Element10
                            details.CC_CHEMICAL10_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 58: // Chemical Element11
                            details.CC_CHEMICAL11_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 63: // Chemical Element12
                            details.CC_CHEMICAL12_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 68: // Chemical Element13
                            details.CC_CHEMICAL13_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 73: // Chemical Element14
                            details.CC_CHEMICAL14_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 78: // Chemical Element15
                            details.CC_CHEMICAL15_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 83: // Chemical Element16
                            details.CC_CHEMICAL16_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 88: // Chemical Element17
                            details.CC_CHEMICAL17_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 93: // Chemical Element18
                            details.CC_CHEMICAL18_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 98: // Chemical Element19
                            details.CC_CHEMICAL19_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 103: // Chemical Element20
                            details.CC_CHEMICAL20_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 108: // Chemical Element21
                            details.CC_CHEMICAL21_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 113: // Chemical Element22
                            details.CC_CHEMICAL22_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 118: // Chemical Element23
                            details.CC_CHEMICAL23_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 123: // Chemical Element24
                            details.CC_CHEMICAL24_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;
                        case 128: // Chemical Element25
                            details.CC_CHEMICAL25_STRING = Regex.Replace(textValue, @"[^0-9.]", "");
                            break;

                        case 9: // Chemical Min1
                            details.CCMIN1_STRING = textValue;
                            break;
                        case 14: // Chemical Min2
                            details.CCMIN2_STRING = textValue;
                            break;
                        case 19: // Chemical Min3
                            details.CCMIN3_STRING = textValue;
                            break;
                        case 24: // Chemical Min4
                            details.CCMIN4_STRING = textValue;
                            break;
                        case 29: // Chemical Min5
                            details.CCMIN5_STRING = textValue;
                            break;
                        case 34: // Chemical Min6
                            details.CCMIN6_STRING = textValue;
                            break;
                        case 39: // Chemical Min7
                            details.CCMIN7_STRING = textValue;
                            break;
                        case 44: // Chemical Min8
                            details.CCMIN8_STRING = textValue;
                            break;
                        case 49: // Chemical Min9
                            details.CCMIN9_STRING = textValue;
                            break;
                        case 54: // Chemical Min10
                            details.CCMIN10_STRING = textValue;
                            break;
                        case 59: // Chemical Min11
                            details.CCMIN11_STRING = textValue;
                            break;
                        case 64: // Chemical Min12
                            details.CCMIN12_STRING = textValue;
                            break;
                        case 69: // Chemical Min13
                            details.CCMIN13_STRING = textValue;
                            break;
                        case 74: // Chemical Min14
                            details.CCMIN14_STRING = textValue;
                            break;
                        case 79: // Chemical Min15
                            details.CCMIN15_STRING = textValue;
                            break;
                        case 84: // Chemical Min16
                            details.CCMIN16_STRING = textValue;
                            break;
                        case 89: // Chemical Min17
                            details.CCMIN17_STRING = textValue;
                            break;
                        case 94: // Chemical Min18
                            details.CCMIN18_STRING = textValue;
                            break;
                        case 99: // Chemical Min19
                            details.CCMIN19_STRING = textValue;
                            break;
                        case 104: // Chemical Min20
                            details.CCMIN20_STRING = textValue;
                            break;
                        case 109: // Chemical Min21
                            details.CCMIN21_STRING = textValue;
                            break;
                        case 114: // Chemical Min22
                            details.CCMIN22_STRING = textValue;
                            break;
                        case 119: // Chemical Min23
                            details.CCMIN23_STRING = textValue;
                            break;
                        case 124: // Chemical Min24
                            details.CCMIN24_STRING = textValue;
                            break;
                        case 129: // Chemical Min25
                            details.CCMIN25_STRING = textValue;
                            break;

                        case 10: // Chemical Max1
                            details.CCMAX1_STRING = textValue;
                            break;
                        case 15: // Chemical Max2
                            details.CCMAX2_STRING = textValue;
                            break;
                        case 20: // Chemical Max3
                            details.CCMAX3_STRING = textValue;
                            break;
                        case 25: // Chemical Max4
                            details.CCMAX4_STRING = textValue;
                            break;
                        case 30: // Chemical Max5
                            details.CCMAX5_STRING = textValue;
                            break;
                        case 35: // Chemical Max6
                            details.CCMAX6_STRING = textValue;
                            break;
                        case 40: // Chemical Max7
                            details.CCMAX7_STRING = textValue;
                            break;
                        case 45: // Chemical Max8
                            details.CCMAX8_STRING = textValue;
                            break;
                        case 50: // Chemical Max9
                            details.CCMAX9_STRING = textValue;
                            break;
                        case 55: // Chemical Max10
                            details.CCMAX10_STRING = textValue;
                            break;
                        case 60: // Chemical Max11
                            details.CCMAX11_STRING = textValue;
                            break;
                        case 65: // Chemical Max12
                            details.CCMAX12_STRING = textValue;
                            break;
                        case 70: // Chemical Max13
                            details.CCMAX13_STRING = textValue;
                            break;
                        case 75: // Chemical Max14
                            details.CCMAX14_STRING = textValue;
                            break;
                        case 80: // Chemical Max15
                            details.CCMAX15_STRING = textValue;
                            break;
                        case 85: // Chemical Max16
                            details.CCMAX16_STRING = textValue;
                            break;
                        case 90: // Chemical Max17
                            details.CCMAX17_STRING = textValue;
                            break;
                        case 95: // Chemical Max18
                            details.CCMAX18_STRING = textValue;
                            break;
                        case 100: // Chemical Max19
                            details.CCMAX19_STRING = textValue;
                            break;
                        case 105: // Chemical Max20
                            details.CCMAX20_STRING = textValue;
                            break;
                        case 110: // Chemical Max21
                            details.CCMAX21_STRING = textValue;
                            break;
                        case 115: // Chemical Max22
                            details.CCMAX22_STRING = textValue;
                            break;
                        case 120: // Chemical Max23
                            details.CCMAX23_STRING = textValue;
                            break;
                        case 125: // Chemical Max24
                            details.CCMAX24_STRING = textValue;
                            break;
                        case 130: // Chemical Max25
                            details.CCMAX25_STRING = textValue;
                            break;
                        case 11: // Chemical Result1
                            details.CCACTUAL1 = textValue;
                            break;
                        case 16: // Chemical Result2
                            details.CCACTUAL2 = textValue;
                            break;
                        case 21: // Chemical Result3
                            details.CCACTUAL3 = textValue;
                            break;
                        case 26: // Chemical Result4
                            details.CCACTUAL4 = textValue;
                            break;
                        case 31: // Chemical Result5
                            details.CCACTUAL5 = textValue;
                            break;
                        case 36: // Chemical Result6
                            details.CCACTUAL6 = textValue;
                            break;
                        case 41: // Chemical Result7
                            details.CCACTUAL7 = textValue;
                            break;
                        case 46: // Chemical Result8
                            details.CCACTUAL8 = textValue;
                            break;
                        case 51: // Chemical Result9
                            details.CCACTUAL9 = textValue;
                            break;
                        case 56: // Chemical Result10
                            details.CCACTUAL10 = textValue;
                            break;
                        case 61: // Chemical Result11
                            details.CCACTUAL11 = textValue;
                            break;
                        case 66: // Chemical Result12
                            details.CCACTUAL12 = textValue;
                            break;
                        case 71: // Chemical Result13
                            details.CCACTUAL13 = textValue;
                            break;
                        case 76: // Chemical Result14
                            details.CCACTUAL14 = textValue;
                            break;
                        case 81: // Chemical Result15
                            details.CCACTUAL15 = textValue;
                            break;
                        case 86: // Chemical Result16
                            details.CCACTUAL16 = textValue;
                            break;
                        case 91: // Chemical Result17
                            details.CCACTUAL17 = textValue;
                            break;
                        case 96: // Chemical Result18
                            details.CCACTUAL18 = textValue;
                            break;
                        case 101: // Chemical Result19
                            details.CCACTUAL19 = textValue;
                            break;
                        case 106: // Chemical Result20
                            details.CCACTUAL20 = textValue;
                            break;
                        case 111: // Chemical Result21
                            details.CCACTUAL21 = textValue;
                            break;
                        case 116: // Chemical Result22
                            details.CCACTUAL22 = textValue;
                            break;
                        case 121: // Chemical Result23
                            details.CCACTUAL23 = textValue;
                            break;
                        case 126: // Chemical Result24
                            details.CCACTUAL24 = textValue;
                            break;
                        case 131: // Chemical Result25
                            details.CCACTUAL25 = textValue;
                            break;

                        case 132: // Quench Element1
                            details.HARD_POSITION1_STRING = textValue;
                            break;
                        case 136: // Quench Element2
                            details.HARD_POSITION2_STRING = textValue;
                            break;
                        case 140: // Quench Element3
                            details.HARD_POSITION3_STRING = textValue;
                            break;
                        case 144: // Quench Element4
                            details.HARD_POSITION4_STRING = textValue;
                            break;
                        case 148: // Quench Element5
                            details.HARD_POSITION5_STRING = textValue;
                            break;
                        case 152: // Quench Element6
                            details.HARD_POSITION6_STRING = textValue;
                            break;
                        case 156: // Quench Element7
                            details.HARD_POSITION7_STRING = textValue;
                            break;
                        case 160: // Quench Element8
                            details.HARD_POSITION8_STRING = textValue;
                            break;
                        case 164: // Quench Element9
                            details.HARD_POSITION9_STRING = textValue;
                            break;
                        case 168: // Quench Element10
                            details.HARD_POSITION10_STRING = textValue;
                            break;
                        case 172: // Quench Element11
                            details.HARD_POSITION11_STRING = textValue;
                            break;
                        case 176: // Quench Element12
                            details.HARD_POSITION12_STRING = textValue;
                            break;
                        case 180: // Quench Element13
                            details.HARD_POSITION13_STRING = textValue;
                            break;
                        case 184: // Quench Element14
                            details.HARD_POSITION14_STRING = textValue;
                            break;
                        case 188: // Quench Element15
                            details.HARD_POSITION15_STRING = textValue;
                            break;
                        case 192: // Quench Element16
                            details.HARD_POSITION16_STRING = textValue;
                            break;
                        case 196: // Quench Element17
                            details.HARD_POSITION17_STRING = textValue;
                            break;
                        case 200: // Quench Element18
                            details.HARD_POSITION18_STRING = textValue;
                            break;
                        case 204: // Quench Element19
                            details.HARD_POSITION19_STRING = textValue;
                            break;
                        case 208: // Quench Element20
                            details.HARD_POSITION20_STRING = textValue;
                            break;
                        case 212: // Quench Element21
                            details.HARD_POSITION21_STRING = textValue;
                            break;
                        case 216: // Quench Element22
                            details.HARD_POSITION22_STRING = textValue;
                            break;
                        case 220: // Quench Element23
                            details.HARD_POSITION23_STRING = textValue;
                            break;
                        case 224: // Quench Element24
                            details.HARD_POSITION24_STRING = textValue;
                            break;
                        case 228: // Quench Element25
                            details.HARD_POSITION25_STRING = textValue;
                            break;

                        case 133: // Quench Min1
                            details.HARD_MIN1_STRING = textValue;
                            break;
                        case 137: // Quench Min2
                            details.HARD_MIN2_STRING = textValue;
                            break;
                        case 141: // Quench Min3
                            details.HARD_MIN3_STRING = textValue;
                            break;
                        case 145: // Quench Min4
                            details.HARD_MIN4_STRING = textValue;
                            break;
                        case 149: // Quench Min5
                            details.HARD_MIN5_STRING = textValue;
                            break;
                        case 153: // Quench Min6
                            details.HARD_MIN6_STRING = textValue;
                            break;
                        case 157: // Quench Min7
                            details.HARD_MIN7_STRING = textValue;
                            break;
                        case 161: // Quench Min8
                            details.HARD_MIN8_STRING = textValue;
                            break;
                        case 165: // Quench Min9
                            details.HARD_MIN9_STRING = textValue;
                            break;
                        case 169: // Quench Min10
                            details.HARD_MIN10_STRING = textValue;
                            break;
                        case 173: // Quench Min11
                            details.HARD_MIN11_STRING = textValue;
                            break;
                        case 177: // Quench Min12
                            details.HARD_MIN12_STRING = textValue;
                            break;
                        case 181: // Quench Min13
                            details.HARD_MIN13_STRING = textValue;
                            break;
                        case 185: // Quench Min14
                            details.HARD_MIN14_STRING = textValue;
                            break;
                        case 189: // Quench Min15
                            details.HARD_MIN15_STRING = textValue;
                            break;
                        case 193: // Quench Min16
                            details.HARD_MIN16_STRING = textValue;
                            break;
                        case 197: // Quench Min17
                            details.HARD_MIN17_STRING = textValue;
                            break;
                        case 201: // Quench Min18
                            details.HARD_MIN18_STRING = textValue;
                            break;
                        case 205: // Quench Min19
                            details.HARD_MIN19_STRING = textValue;
                            break;
                        case 209: // Quench Min20
                            details.HARD_MIN20_STRING = textValue;
                            break;
                        case 213: // Quench Min21
                            details.HARD_MIN21_STRING = textValue;
                            break;
                        case 217: // Quench Min22
                            details.HARD_MIN22_STRING = textValue;
                            break;
                        case 221: // Quench Min23
                            details.HARD_MIN23_STRING = textValue;
                            break;
                        case 225: // Quench Min24
                            details.HARD_MIN24_STRING = textValue;
                            break;
                        case 229: // Quench Min25
                            details.HARD_MIN25_STRING = textValue;
                            break;


                        case 134: // Quench Max1
                            details.HARD_MAX1_STRING = textValue;
                            break;
                        case 138: // Quench Max2
                            details.HARD_MAX2_STRING = textValue;
                            break;
                        case 142: // Quench Max3
                            details.HARD_MAX3_STRING = textValue;
                            break;
                        case 146: // Quench Max4
                            details.HARD_MAX4_STRING = textValue;
                            break;
                        case 150: // Quench Max5
                            details.HARD_MAX5_STRING = textValue;
                            break;
                        case 154: // Quench Max6
                            details.HARD_MAX6_STRING = textValue;
                            break;
                        case 158: // Quench Max7
                            details.HARD_MAX7_STRING = textValue;
                            break;
                        case 162: // Quench Max8
                            details.HARD_MAX8_STRING = textValue;
                            break;
                        case 166: // Quench Max9
                            details.HARD_MAX9_STRING = textValue;
                            break;
                        case 170: // Quench Max10
                            details.HARD_MAX10_STRING = textValue;
                            break;
                        case 174: // Quench Max11
                            details.HARD_MAX11_STRING = textValue;
                            break;
                        case 178: // Quench Max12
                            details.HARD_MAX12_STRING = textValue;
                            break;
                        case 182: // Quench Max13
                            details.HARD_MAX13_STRING = textValue;
                            break;
                        case 186: // Quench Max14
                            details.HARD_MAX14_STRING = textValue;
                            break;
                        case 190: // Quench Max15
                            details.HARD_MAX15_STRING = textValue;
                            break;
                        case 194: // Quench Max16
                            details.HARD_MAX16_STRING = textValue;
                            break;
                        case 198: // Quench Max17
                            details.HARD_MAX17_STRING = textValue;
                            break;
                        case 202: // Quench Max18
                            details.HARD_MAX18_STRING = textValue;
                            break;
                        case 206: // Quench Max19
                            details.HARD_MAX19_STRING = textValue;
                            break;
                        case 210: // Quench Max20
                            details.HARD_MAX20_STRING = textValue;
                            break;
                        case 214: // Quench Max21
                            details.HARD_MAX21_STRING = textValue;
                            break;
                        case 218: // Quench Max22
                            details.HARD_MAX22_STRING = textValue;
                            break;
                        case 222: // Quench Max23
                            details.HARD_MAX23_STRING = textValue;
                            break;
                        case 226: // Quench Max24
                            details.HARD_MAX24_STRING = textValue;
                            break;
                        case 230: // Quench Max25
                            details.HARD_MAX25_STRING = textValue;
                            break;

                        case 135: // Quench Result1
                            details.HARD_ACTUAL1_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 139: // Quench Result2
                            details.HARD_ACTUAL2_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 143: // Quench Result3
                            details.HARD_ACTUAL3_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 147: // Quench Result4
                            details.HARD_ACTUAL4_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 151: // Quench Result5
                            details.HARD_ACTUAL5_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 155: // Quench Result6
                            details.HARD_ACTUAL6_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 159: // Quench Result7
                            details.HARD_ACTUAL7_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 163: // Quench Result8
                            details.HARD_ACTUAL8_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 167: // Quench Result9
                            details.HARD_ACTUAL9_STRING =  !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 171: // Quench Result10
                            details.HARD_ACTUAL10_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 175: // Quench Result11
                            details.HARD_ACTUAL11_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 179: // Quench Result12
                            details.HARD_ACTUAL12_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 183: // Quench Result13
                            details.HARD_ACTUAL13_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 187: // Quench Result14
                            details.HARD_ACTUAL14_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 191: // Quench Result15
                            details.HARD_ACTUAL15_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 195: // Quench Result16
                            details.HARD_ACTUAL16_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 199: // Quench Result17
                            details.HARD_ACTUAL17_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 203: // Quench Result18
                            details.HARD_ACTUAL18_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 207: // Quench Result19
                            details.HARD_ACTUAL19_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 211: // Quench Result20
                            details.HARD_ACTUAL20_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 215: // Quench Result21
                            details.HARD_ACTUAL21_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 219: // Quench Result22
                            details.HARD_ACTUAL22_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 223: // Quench Result23
                            details.HARD_ACTUAL23_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 227: // Quench Result24
                            details.HARD_ACTUAL24_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;
                        case 231: // Quench Result25
                            details.HARD_ACTUAL25_STRING = !string.IsNullOrWhiteSpace(textValue) && Convert.ToInt32(textValue) > 0 ? textValue : null;
                            break;

                    }

                }
            }
            return details;
        }
        public IActionResult InitializeSort()
        {
            var sortFields = new List<TSSortField> {

                                        new TSSortField {
                                            FieldName = "MK.SHORT_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_MaterialMaker"),
                                            IsDefaultSort=false,
                                            SortOrder=TSSortDirection.Descending
                                        },
                                         new TSSortField {
                                            FieldName = "ML.UNIQUE_TEMPLATE_NAME",
                                            DisplayName = _localization.GetLocalizedHtmlString("lbl_M_TemplateName"), //7
                                            IsDefaultSort=true,
                                            SortOrder=TSSortDirection.Descending
                                        }


                                    };
            return Json(new { IsSuccess = true, message = sortFields });
        }

        public IActionResult SavePDFTemp([FromForm] IFormFile file) //IFormFile file, 
        {

            string filePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet");
            string fileName = string.Empty;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(filePath, "TempFile");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                file.CopyTo(stream);

            }
            return Json(new { IsSuccess = true });
        }
        public void DeleteOldFiles(string folderPath, int days)
        {
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath);

                foreach (var file in files)
                {
                    var creationTime = System.IO.File.GetCreationTime(file);

                    if (creationTime < DateTime.Now.AddDays(-days)) // Expire after 30 min
                    {
                        System.IO.File.Delete(file);
                    }
                }
            }
        }

        public IActionResult DowloadAndSavePDF(string filePath)
        {
            List<MstConfiguration> configuration = new List<MstConfiguration>();
            string ftpPath = string.Empty;
            string ftpUsername = string.Empty;
            string ftpPassword = string.Empty;
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            string rootPath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet\\");


            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

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
            ftpPath = ftpPath + "/" + filePath;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = new FileStream(rootPath + fileName, FileMode.Create))
            {
                responseStream.CopyTo(fileStream);
            }

            return Json(new { IsSuccess = true, message = "/MakerMillsheet/" + fileName });
        }
        public static MakerMillSheetConfiguration CaptureLargeScaleBox(string imgPath)
        {
            MakerMillSheetConfiguration result = new MakerMillSheetConfiguration();
            Mat src = Cv2.ImRead(imgPath, ImreadModes.Grayscale);

            // Step 1: Invert and binarize the image
            Mat binary = new Mat();
            Cv2.AdaptiveThreshold(~src, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 15, -2);

            // Step 2: Extract horizontal lines
            Mat horizontal = binary.Clone();
            int horizontalSize = horizontal.Cols / 30;
            Mat horizontalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(horizontalSize, 1));
            Cv2.Erode(horizontal, horizontal, horizontalStructure);
            Cv2.Dilate(horizontal, horizontal, horizontalStructure);

            // Step 3: Extract vertical lines
            Mat vertical = binary.Clone();
            int verticalSize = vertical.Rows / 30;
            Mat verticalStructure = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, verticalSize));
            Cv2.Erode(vertical, vertical, verticalStructure);
            Cv2.Dilate(vertical, vertical, verticalStructure);

            // Step 4: Combine lines
            Mat mask = new Mat();
            Cv2.Add(horizontal, vertical, mask);

            // Step 5: Find contours
            Cv2.FindContours(mask, out Point[][] contours, out HierarchyIndex[] hierarchy,
                RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 👉 NEW: Collect valid table rectangles
            List<Rect> tableRects = new List<Rect>();

            foreach (var cnt in contours)
            {
                var rect = Cv2.BoundingRect(cnt);

                // 🔥 Adjust this filter (important)
                if (rect.Width > 300 && rect.Height > 80)
                {
                    tableRects.Add(rect);
                }
            }

            // Step 6: Load original image (color for output)
            Mat output = Cv2.ImRead(imgPath);

            // 👉 Debug: draw detected tables
            foreach (var r in tableRects)
            {
                Cv2.Rectangle(output, r, Scalar.Red, 2);
            }

           // Cv2.ImWrite("debug_tables.png", output);

            // Step 7: Merge all rectangles into ONE BIG TABLE
            if (tableRects.Count > 0)
            {
                int minX = tableRects.Min(r => r.X);
                int minY = tableRects.Min(r => r.Y);
                int maxX = tableRects.Max(r => r.X + r.Width);
                int maxY = tableRects.Max(r => r.Y + r.Height);

                Rect finalRect = new Rect(minX, minY, maxX - minX, maxY - minY);

                // Step 8: Crop final table
                Mat originalColor = Cv2.ImRead(imgPath);
                Mat finalTable = new Mat(originalColor, finalRect);

                Point topLeft = new Point(finalRect.X, finalRect.Y);
                Point topRight = new Point(finalRect.X + finalRect.Width, finalRect.Y);
                Point bottomLeft = new Point(finalRect.X, finalRect.Y + finalRect.Height);
                Point bottomRight = new Point(finalRect.X + finalRect.Width, finalRect.Y + finalRect.Height);


                result.TOP_LEFT_X = topLeft.X;
                result.TOP_LEFT_Y = topLeft.Y;
                result.TOP_RIGHT_X = topRight.X;
                result.TOP_RIGHT_Y = topRight.Y;
                result.BOTTOM_LEFT_X = bottomLeft.X;
                result.BOTTOM_LEFT_Y = bottomLeft.Y;
                result.BOTTOM_RIGHT_X = bottomRight.X;
                result.BOTTOM_RIGHT_Y = bottomRight.Y;
               
            }
            
            return result;
        }
        public IActionResult SetBase64StringForUpload([FromForm] IFormFile file, int pageNo) //IFormFile file, 
        {
            string filePath = Path.Combine(Environment.ContentRootPath, "wwwroot\\MakerMillsheet");
            string fileName = string.Empty;
            string returnFile = string.Empty;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            foreach (var deleteFile in Directory.GetFiles(filePath))
            {
                System.IO.File.Delete(deleteFile);
            }


            fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                file.CopyTo(stream);

            }

            PdfSharpCore.Pdf.PdfDocument inputPDFFile = PdfSharpCore.Pdf.IO.PdfReader.Open(Path.Combine(filePath, fileName), PdfSharpCore.Pdf.IO.PdfDocumentOpenMode.Import);

            PdfSharpCore.Pdf.PdfDocument outputPDFDocument = new PdfSharpCore.Pdf.PdfDocument();


            outputPDFDocument.AddPage(inputPDFFile.Pages[pageNo - 1]);

            //save the PDF document
            SaveOutputPDF(outputPDFDocument, "MillSheetTemp.pdf", filePath);
            returnFile = filePath + "\\MillSheetTemp.pdf";
            byte[] bytes = System.IO.File.ReadAllBytes(returnFile);

            return Ok(new { Message = Convert.ToBase64String(bytes) });


        }

    }
}
