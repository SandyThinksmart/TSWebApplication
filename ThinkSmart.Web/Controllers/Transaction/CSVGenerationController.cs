using MailKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Net.Http.Headers;
using NPOI.SS.UserModel;
using OpenCvSharp;
using PdfSharp.Charting;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Web.MiddleWares;
using TSWebApplication.Web.Models;


namespace TSWebApplication.Web.Controllers.Transaction
{
    public class CSVGenerationController : Controller
    {
        private readonly LocalizationServices _localization;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICSVGenerationService _csvService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IFontResolver _fontResolver;
        public CSVGenerationController(IViewRenderService viewRenderService, IWebHostEnvironment hostEnvironment, LocalizationServices localization, ICSVGenerationService csvService, IFontResolver fontResolver)
        {
            _viewRenderService = viewRenderService;
            _localization = localization;
            _csvService = csvService;
            _hostEnvironment = hostEnvironment;
            _fontResolver = fontResolver;
        }
        public IActionResult CSVTitleList()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        public IActionResult CSVGroupTitleList()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

        public IActionResult CSVExtractCondition()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            int tensouProcessCD = Convert.ToInt32(UIUtils.GetProcessKubun(_localization.GetLocalizedHtmlString("lbl_ProcessWIP_Forwarding")));
            ViewBag.TensouProcessCD = tensouProcessCD;
            return View();
        }

        public IActionResult GridSearchTitles([FromBody] List<OutputCSVDetails> details)
        {
            string view = _viewRenderService.RenderToString("CSVGeneration/CSVTitleListGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }
        public IActionResult GridSearchGroupTitleList([FromBody] List<OutputCSVDetails> details)
        {
            string view = _viewRenderService.RenderToString("CSVGeneration/CSVGroupTitleGrid", details);
            return Json(new { IsSuccess = true, message = view });
        }

        public IActionResult GridSearchConditions([FromBody] OutputCSVDetails details)
        {
            details.WHERE_CONDITION_LIST = new List<WhereConditions>();

            WhereConditions items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN1))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN1;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN1;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE1;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE1;

            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN2))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN2;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN2;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE2;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE2;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN3))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN3;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN3;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE3;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE3;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN4))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN4;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN4;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE4;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE4;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN5))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN5;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN5;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE5;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE5;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN6))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN6;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN6;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE6;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE6;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN7))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN7;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN7;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE7;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE7;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN8))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN8;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN8;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE8;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE8;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN9))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN9;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN9;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE9;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE9;
            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            items = new WhereConditions();
            if (!string.IsNullOrEmpty(details.WHERE_CONDITION_COLUMN10))
            {
                items.SQL_PLSQL_TYPE = details.SQL_PLSQL_TYPE;
                items.DISPLAY_CONDITION_COLUMN = details.DISPLAY_CONDITION_COLUMN10;
                items.WHERE_CONDITION_COLUMN = details.WHERE_CONDITION_COLUMN10;
                items.WHERE_CONDITION_TYPE = details.WHERE_CONDITION_TYPE10;
                items.ON_OFF_FLAG_TYPE = details.ON_OFF_FLAG_TYPE10;

            }
            if (items.WHERE_CONDITION_COLUMN != null)
            {
                details.WHERE_CONDITION_LIST.Add(items);
            }

            string view = _viewRenderService.RenderToString("CSVGeneration/CSVConditionGrid", details);

            var listData = UIUtils.GetCSVConditions();

            var result = ComposeWhereCondition(listData); // where conditions binded as per Oracle syntax


            return Json(new { wherecondition = result, message = view });
        }
        public IActionResult CreateCSV([FromBody] OutputCSVDetails csvDetails)
        {
            DataTable dt = new DataTable();
            string fileName = "";
            string downloadFile = "";
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "OutputCSV-PDF");
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime < DateTime.Now.AddDays(-2)) // file will deleted which are more that 2 days ago
                {
                    fi.Delete();
                }
            }
            dt = GetExportData(csvDetails);

            if (dt.Rows.Count > 0)
            {
                string colName = dt.Columns[0].ColumnName;
                bool isJP = colName.Any(c =>
                        (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
                        (c >= 0x30A0 && c <= 0x30FF) ||  // Katakana
                        (c >= 0x4E00 && c <= 0x9FFF));   // Kanji

                for (int r = 0; r < dt.Columns.Count; r++)
                {
                    colName = dt.Columns[r].ColumnName;

                    bool currentColumn = colName.Any(c =>
                   (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
                   (c >= 0x30A0 && c <= 0x30FF) ||  // Katakana
                   (c >= 0x4E00 && c <= 0x9FFF));   // Kanji


                    if (isJP == true && currentColumn == false)
                    {
                        dt.Columns.RemoveAt(r);
                    }
                }

                fileName = csvDetails.CSV_FILE_NAME;
                if (csvDetails.ADD_DATE_FLAG == 1)
                {
                    fileName = fileName + DateTime.Now.ToString(" yyyymmdd");
                }
                if (csvDetails.ADD_DATE_FLAG == 1 && csvDetails.ADD_TIME_FLAG == 1)
                {
                    fileName = fileName + DateTime.Now.ToString("HHmmss");
                }
                else if (csvDetails.ADD_DATE_FLAG == 0 && csvDetails.ADD_TIME_FLAG == 1)
                {
                    fileName = fileName + DateTime.Now.ToString(" HHmmss");
                }

                path = path + "\\" + fileName + ".csv";
                using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    // Write headers
                    writer.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(dc => EscapeCsv(dc.ColumnName))));

                    // Write rows
                    foreach (DataRow row in dt.Rows)
                    {
                        var fields = row.ItemArray.Select(field => EscapeCsv(field?.ToString()));
                        writer.WriteLine(string.Join(",", fields));
                    }
                    downloadFile = "/OutputCSV-PDF/" + fileName + ".csv";
                }
            }

            return Json(new { downloadpath = downloadFile, filename = fileName + ".csv" });
        }

        public IActionResult CreatePDF([FromBody] OutputCSVDetails csvDetails)
        {
            DataTable dt = new DataTable();
            string fileName = "";
            string downloadFile = "";
            int countColumns = 0;
            List<int> headerWidthList = new List<int>();
            List<string> dataTypeList = new List<string>();
            int startWithLeft = 50;
            int x = startWithLeft;
            int y = 45;
            int rowHeight = 16;
            int rowsCount = 0;
            double pageWidth = 0;
            int headerHeight = 20;
            List<(string h_name,int h_width)> columnsList = new List<(string, int)>();
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "OutputCSV-PDF");
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime < DateTime.Now.AddDays(-2)) // file will deleted which are more that 2 days ago
                {
                    fi.Delete();
                }
            }

       
            dt = GetExportData(csvDetails);
            if (dt.Rows.Count > 0)
            {
                string colName = dt.Columns[0].ColumnName;
                bool isJP = colName.Any(c =>
                        (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
                        (c >= 0x30A0 && c <= 0x30FF) ||  // Katakana
                        (c >= 0x4E00 && c <= 0x9FFF));   // Kanji

                for (int r = 0; r < dt.Columns.Count; r++)
                {
                    colName = dt.Columns[r].ColumnName;

                    bool currentColumn = colName.Any(c =>
                   (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
                   (c >= 0x30A0 && c <= 0x30FF) ||  // Katakana
                   (c >= 0x4E00 && c <= 0x9FFF));   // Kanji


                    if (isJP == true && currentColumn == false)
                    {
                        dt.Columns.RemoveAt(r);
                    }
                }

                //string colName = dt.Columns[0].ColumnName;

                //bool jpColumn1 = colName.Any(c =>
                //    (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
                //    (c >= 0x30A0 && c <= 0x30FF) ||  // Katakana
                //    (c >= 0x4E00 && c <= 0x9FFF));   // Kanji

                //if (jpColumn1) // FOR JP HEADER REMOVE ENGLISH HEADER
                //{
                //if (dt.Columns.Contains("INPUT_LOCATION_CD"))
                //{
                //    dt.Columns.Remove("INPUT_LOCATION_CD");
                //}
                //if (dt.Columns.Contains("NEXT_PROCESS"))
                //{
                //    dt.Columns.Remove("NEXT_PROCESS"); 
                //}
                // }


                fileName = csvDetails.CSV_FILE_NAME;
                if (csvDetails.ADD_DATE_FLAG == 1)
                {
                    fileName = fileName + DateTime.Now.ToString(" yyyymmdd");
                }
                if (csvDetails.ADD_DATE_FLAG == 1 && csvDetails.ADD_TIME_FLAG == 1)
                {
                    fileName = fileName + DateTime.Now.ToString("HHmmss");
                }
                else if (csvDetails.ADD_DATE_FLAG == 0 && csvDetails.ADD_TIME_FLAG == 1)
                {
                    fileName = fileName + DateTime.Now.ToString(" HHmmss");
                }

                path = path + "\\" + fileName + ".pdf";

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();

                page.Size = SetPageSize(csvDetails.PAGE_SIZE);
                if (csvDetails.PAGE_ORIENTATION.ToLower().StartsWith("port"))
                {
                    page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                    pageWidth = page.Width - 18;
                }
                else
                {
                    page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                    pageWidth = page.Width - 25;
                }
                GlobalFontSettings.FontResolver = _fontResolver;

                XFont titleFont;
                XFont headerFont;
                XFont detailsFont;
                if (Convert.ToInt32(csvDetails.REPORT_TITLE_FONT) > 0)
                {
                    titleFont = new XFont("MS Gothic", Convert.ToDouble(csvDetails.REPORT_TITLE_FONT), XFontStyle.Bold);
                }
                else
                {
                    titleFont = new XFont("MS Gothic", 13, XFontStyle.Bold);
                }
                if (Convert.ToInt32(csvDetails.REPORT_HEADER_FONT) > 0)
                {
                    headerFont = new XFont("MS Gothic", Convert.ToDouble(csvDetails.REPORT_HEADER_FONT), XFontStyle.Regular);
                }
                else
                {
                    headerFont = new XFont("MS Gothic", 11, XFontStyle.Regular);
                }
                if (Convert.ToInt32(csvDetails.REPORT_DETAILS_FONT) > 0)
                {
                    detailsFont = new XFont("MS Gothic", Convert.ToDouble(csvDetails.REPORT_DETAILS_FONT), XFontStyle.Regular);
                }
                else
                {
                    detailsFont = new XFont("MS Gothic", 9, XFontStyle.Regular);
                }
                XPen thinPen_03 = new XPen(XColors.Black, 0.1);
                string title = csvDetails.TITLE;
                double marginTop = 5;

                double titleHeight = 40;

                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {


                    //=========================Draw Title Name====================

                    XBrush titleBrush = XBrushes.Black;
                    gfx.DrawString(title, titleFont, titleBrush,
                        new XRect(0, marginTop, pageWidth, titleHeight),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });
                    //=========================Draw Column Name====================



                    int columnWidth = 0;
                    x = startWithLeft;
                    y = 46;

                    double maxY = page.Height - 30 - marginTop; // 30 margin bottom removed

                    int usedWidth = 0;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string headerName = dt.Columns[i].ColumnName.Replace("\t", "");
                        Type dataType = dt.Columns[i].DataType;
                        XSize size = gfx.MeasureString(headerName, headerFont); //measure header width

                        columnWidth = Convert.ToInt32(size.Width) + 6; //6 extra left and right side padding added
                        if (usedWidth + columnWidth > pageWidth)
                        {
                            break;

                        }
                        string maxValue = string.Empty;
                        if (dataType == typeof(DateTime))
                        {
                            maxValue = "YYYY/MM/DD HH:MI:SS_";
                        }
                        else
                        { //MAX LENGTH VALUE TAKEN
                            maxValue = dt.AsEnumerable().Where(row => row[i] != DBNull.Value).Select(row => row[i].ToString()).OrderByDescending(x => x.Length).FirstOrDefault();
                        }
                        int widthPerValue = 0;
                        if (DateTime.TryParseExact(maxValue, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) // if date in string format then checked
                        {
                            maxValue = "YYYY/MM/DD HH:MI:SS_";
                        }
                        else if (DateTime.TryParseExact(maxValue, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        {
                            maxValue = "YYYY/MM/DD HH:MI_";
                        }

                        if (!string.IsNullOrEmpty(maxValue))
                        {
                            XSize size1 = gfx.MeasureString(maxValue, detailsFont);
                            widthPerValue = Convert.ToInt32(size1.Width) + 6;
                        }

                        if (widthPerValue > columnWidth)
                        {
                            columnWidth = widthPerValue; // If DataTable value width is greater than Header width then Value width used
                        }
                        countColumns = countColumns + 1;
                        headerWidthList.Add(columnWidth);

                        gfx.DrawRectangle(thinPen_03, XBrushes.LightGray, x, y, columnWidth, headerHeight);

                        gfx.DrawString(
                         headerName,
                         headerFont,
                         XBrushes.Black,
                         new XRect(x, y, columnWidth, 20),
                         new XStringFormat
                         {
                             Alignment = XStringAlignment.Center,
                             LineAlignment = XLineAlignment.Center
                         });
                        columnsList.Add((headerName, columnWidth));

                        usedWidth = usedWidth + columnWidth;
                        x = x + columnWidth;

                    }
                    //=============Add DataTable Rows value for Page 1  ==============================================
                    x = startWithLeft;
                    y = 67;
                    double leftPadding01 = 3;
                    double rightPadding01 = 6;
                    double topPadding = 2.5;
                    foreach (DataRow row in dt.Rows)
                    {

                        if (y + rowHeight > maxY)
                        {
                            break;
                        }

                        for (int i = 0; i < countColumns; i++)
                        {
                            string dataType = dt.Columns[i].DataType.Name;
                            string cellText = row[i]?.ToString() ?? "";
                            cellText = cellText.Replace("\t", "");
                            int dataWidth = headerWidthList[i];
                            XRect rect = new XRect(x + leftPadding01, y + topPadding, dataWidth - rightPadding01, rowHeight);

                            if (dataType.StartsWith("Int") || dataType == "Double" || dataType == "Single" || dataType == "Decimal" || dataType == "Float")
                            {
                                var format = new XStringFormat
                                {
                                    Alignment = XStringAlignment.Far,       // Right-align

                                };
                                gfx.DrawRectangle(thinPen_03, x, y - 1, dataWidth, rowHeight);
                                gfx.DrawString(cellText, detailsFont, XBrushes.Black, rect, format);
                            }
                            else if (dataType == "DateTime")
                            {
                                var format = new XStringFormat
                                {
                                    Alignment = XStringAlignment.Center,       //Center-align

                                };
                                gfx.DrawRectangle(thinPen_03, x, y - 1, dataWidth, rowHeight);
                                if (!string.IsNullOrEmpty(cellText))
                                {
                                    gfx.DrawString(Convert.ToDateTime(cellText).ToString("yyyy/MM/dd HH:mm:ss"), detailsFont, XBrushes.Black, rect, format);
                                }
                                else
                                {
                                    gfx.DrawString(cellText, detailsFont, XBrushes.Black, rect, format);
                                }

                            }
                            else
                            {
                                var format = new XStringFormat
                                {
                                    Alignment = XStringAlignment.Near,       //Left-align

                                };
                                gfx.DrawRectangle(thinPen_03, x, y - 1, dataWidth, rowHeight);
                                gfx.DrawString(cellText, detailsFont, XBrushes.Black, rect, format);
                            }

                            x = x + dataWidth;

                        }
                        rowsCount++;
                        x = startWithLeft;
                        y = y + rowHeight;

                    }
                }
                //=============Add DataTable Rows value from Page 2  ==============================================

                double maxY1 = page.Height - 20 - 10; // 30 margin bottom and top removed
                int checkRows = 0;
                double leftPaddingOther = 3;
                double rightPaddingOther = 6;
                double topPadding01 = 2.5;
                foreach (DataRow row in dt.Rows)
                {
                    if (checkRows > rowsCount)
                    {
                        if (y + rowHeight > maxY1)
                        {
                            x = startWithLeft; // after row complete start position set
                            y = 30 + rowHeight; // after row complete Y position increased

                            page = document.AddPage();
                            page.Size = SetPageSize(csvDetails.PAGE_SIZE);
                            if (csvDetails.PAGE_ORIENTATION.ToLower().StartsWith("port"))
                            {
                                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                            }
                            else
                            {
                                page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                            }
                            using (XGraphics gfx = XGraphics.FromPdfPage(page))
                            {
                                XBrush titleBrush = XBrushes.Black;
                                gfx.DrawString(title, titleFont, titleBrush,
                                    new XRect(0, marginTop, pageWidth, titleHeight),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                                foreach (var item in columnsList)
                                {

                                    gfx.DrawRectangle(thinPen_03, XBrushes.LightGray, x, y, item.h_width, headerHeight);

                                    gfx.DrawString(
                                     item.h_name,
                                     headerFont,
                                     XBrushes.Black,
                                     new XRect(x, y, item.h_width, 20),
                                     new XStringFormat
                                     {
                                         Alignment = XStringAlignment.Center,
                                         LineAlignment = XLineAlignment.Center
                                     });
                                    x = x + item.h_width;
                                }
                                x = startWithLeft;
                                y = y + 21;
                            }
                        }


                        using (XGraphics gfx = XGraphics.FromPdfPage(page))
                        {

                            for (int i = 0; i < countColumns; i++)
                            {
                                string cellText = row[i]?.ToString() ?? "";
                                cellText = cellText.Replace("\t", "");
                                int dataWidth = headerWidthList[i];
                                string dataType = dt.Columns[i].DataType.Name;
                                XRect rect = new XRect(x + leftPaddingOther, y + topPadding01, dataWidth - rightPaddingOther, rowHeight);
                                if (dataType.StartsWith("Int") || dataType == "Double" || dataType == "Single" || dataType == "Decimal" || dataType == "Float")
                                {
                                    var format = new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Far,       // Right-align

                                    };
                                    gfx.DrawRectangle(thinPen_03, x, y - 1, dataWidth, rowHeight);
                                    gfx.DrawString(cellText, detailsFont, XBrushes.Black, rect, format);
                                }
                                else if (dataType == "DateTime")
                                {
                                    var format = new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,       // Center-align

                                    };

                                    gfx.DrawRectangle(thinPen_03, x, y - 1, dataWidth, rowHeight);
                                    gfx.DrawString(Convert.ToDateTime(cellText).ToString("yyyy/MM/dd HH:mm:ss"), detailsFont, XBrushes.Black, rect, format);
                                }
                                else
                                {
                                    var format = new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Near,       // Left-align

                                    };
                                    gfx.DrawRectangle(thinPen_03, x, y - 1, dataWidth, rowHeight);
                                    gfx.DrawString(cellText, detailsFont, XBrushes.Black, rect, format);
                                }

                                x = x + dataWidth;

                            }


                        }
                        x = startWithLeft;
                        y = y + rowHeight;
                    }
                    checkRows++;
                }

                //=============Draw Footer ============================


                XFont footerFont = new XFont("MS Gothic", 10, XFontStyle.Regular);
                int pageNo = 1;
                for (int i = 0; i < document.PageCount; i++)
                {
                    var page2 = document.Pages[i];

                    double leftPadding = startWithLeft;
                    string printDateTime = _localization.GetLocalizedHtmlString("lbl_PrintDateTimeRpt") + DateTime.Now.ToString(Constants.DateFormats.YYYYMMDD_HH24MMSS);
                    using (XGraphics gfx3 = XGraphics.FromPdfPage(page2, XGraphicsPdfPageOptions.Append))
                    {
                        gfx3.DrawString(
                            printDateTime,
                            footerFont,
                            XBrushes.Black,
                            new XRect(leftPadding, page2.Height - 30, page2.Width - 40, 20),
                            new XStringFormat
                            {
                                Alignment = XStringAlignment.Near,
                                LineAlignment = XLineAlignment.Center
                            });
                    }

                    string pageNumberText = _localization.GetLocalizedHtmlString("lbl_Page") + Convert.ToInt32(pageNo + i) + "/" + document.PageCount;
                    using (XGraphics gfx2 = XGraphics.FromPdfPage(page2, XGraphicsPdfPageOptions.Append))
                    {
                        gfx2.DrawString(
                            pageNumberText,
                            footerFont,
                            XBrushes.Black,
                            new XRect(0, page2.Height - 30, page2.Width - 40, 20),
                            new XStringFormat
                            {
                                Alignment = XStringAlignment.Far,
                                LineAlignment = XLineAlignment.Center
                            });
                    }

                }
                // Save the file
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    document.Options.CompressContentStreams = true;
                    document.Options.NoCompression = false;
                    document.Save(stream);
                }

                downloadFile = "/OutputCSV-PDF/" + fileName + ".pdf";

            }


            return Json(new { downloadpath = downloadFile, filename = fileName + ".pdf" });

            #region Working Code
            //DataTable dt = new DataTable();
            //string fileName = "";
            //string downloadFile = "";
            //int startCol = 0;
            //List<int> headerWidthList = new List<int>();
            //List<string> dataTypeList = new List<string>();

            //string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "OutputCSV-PDF");
            //if (!Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}
            //string[] files = Directory.GetFiles(path);

            //foreach (string file in files)
            //{
            //    FileInfo fi = new FileInfo(file);
            //    if (fi.CreationTime < DateTime.Now.AddDays(-2)) // file will deleted which are more that 2 days ago
            //    {
            //        fi.Delete();
            //    }
            //}



            //dt = GetExportData(csvDetails);

            //if (dt.Rows.Count > 0)
            //{
            //    fileName = csvDetails.CSV_FILE_NAME;
            //    if (csvDetails.ADD_DATE_FLAG == 1)
            //    {
            //        fileName = fileName + DateTime.Now.ToString(" yyyymmdd");
            //    }
            //    if (csvDetails.ADD_DATE_FLAG == 1 && csvDetails.ADD_TIME_FLAG == 1)
            //    {
            //        fileName = fileName + DateTime.Now.ToString("HHmmss");
            //    }
            //    else if (csvDetails.ADD_DATE_FLAG == 0 && csvDetails.ADD_TIME_FLAG == 1)
            //    {
            //        fileName = fileName + DateTime.Now.ToString(" HHmmss");
            //    }

            //    path = path + "\\" + fileName + ".pdf";

            //    PdfDocument document = new PdfDocument();
            //    PdfPage page = document.AddPage();
            //    page.Size = PdfSharpCore.PageSize.A4;
            //    page.Orientation = PdfSharpCore.PageOrientation.Landscape;

            //    XGraphics gfx = XGraphics.FromPdfPage(page);

            //    XFont font = new XFont("Arial Unicode MS", 10, XFontStyle.Regular);
            //    XTextFormatter tf = new XTextFormatter(gfx);
            //    tf.Alignment = XParagraphAlignment.Center;

            //    //==============Page Number added for Page 1==========================
            //    XFont footerFont = new XFont("Arial Unicode MS", 10, XFontStyle.Regular);
            //    string pageNumberText = _localization.GetLocalizedHtmlString("lbl_Page") + 1 + "/" + 10;
            //    gfx.DrawString(
            //               pageNumberText,
            //               footerFont,
            //               XBrushes.Black,
            //               new XRect(0, page.Height.Point - 30, page.Width - 40, 20), // Adjust bottom margin
            //               new XStringFormat
            //               {
            //                   Alignment = XStringAlignment.Far,         // Right-aligned
            //                   LineAlignment = XLineAlignment.Center      // Vertically centered within footer area
            //               }
            //           );

            //    string title = csvDetails.TITLE;
            //    double marginTop = 10;
            //    double pageWidth = page.Width;
            //    double titleHeight = 40;

            //    // Draw title centered horizontally
            //    XFont titleFont = new XFont("Arial Unicode MS", 13, XFontStyle.Bold);
            //    XBrush titleBrush = XBrushes.Black;

            //    gfx.DrawString(title, titleFont, titleBrush,
            //        new XRect(0, marginTop, pageWidth, titleHeight),
            //        new XStringFormat
            //        {
            //            Alignment = XStringAlignment.Center,
            //            LineAlignment = XLineAlignment.Center
            //        });


            //    double leftPadding = 2;
            //    double rightPadding = 2;
            //    double marginBottom = 20;
            //    double margin = 40;
            //    double colWidth = 100;
            //    double rowHeight = 20;
            //    double totalWidthUsed = 0;
            //    double pageHeight = page.Height.Point;
            //    double maxY = pageHeight - marginBottom;

            //    double x = margin;
            //    double y = margin;

            //    double maxHeaderHeight = rowHeight; // rowHeight * 3
            //    double usablePageWidth = page.Width.Point - x - y;// page.Width.Point - 2 * margin;

            //    int columnsPerPage = (int)((page.Width - 2 * margin) / colWidth);

            //    int endCol = Math.Min(columnsPerPage, dt.Columns.Count);



            //    // === Draw Menu Header ===

            //    for (int i = startCol; i < dt.Columns.Count - 1; i++) // header height fixed using header char length from this loop 
            //    {
            //        string headerText = dt.Columns[i].ColumnName;
            //        int maxLen = dt.AsEnumerable().Select(row => row[i]?.ToString()?.Length ?? 0).Max();

            //        string dataType = dt.Columns[i].DataType.Name;
            //        int maxLength = dt.Columns[i].MaxLength;

            //        if (dataType == "Int16")
            //        {
            //            colWidth = 50;
            //        }
            //        else if (dataType == "Int32")
            //        {
            //            if (maxLen > 0 && maxLen < 8)
            //            {
            //                colWidth = 60;
            //            }
            //            else if (maxLen > 7 && maxLen < 13)
            //            {
            //                colWidth = 80;
            //            }
            //        }
            //        else if (dataType == "Int64" || dataType == "DateTime")
            //        {
            //            if (maxLen > 18)
            //            {
            //                colWidth = 95;
            //            }
            //            else
            //            {
            //                colWidth = 60;
            //            }

            //        }
            //        else if (dataType == "Object")
            //        {
            //            colWidth = 70;
            //        }
            //        else if (dataType == "String")
            //        {
            //            if (maxLen > 15 && maxLen < 25)
            //            {
            //                colWidth = 125;
            //            }
            //            else if (maxLen > 25 && maxLen < 45)
            //            {
            //                colWidth = 150;
            //            }
            //            else if (maxLen > 7 && maxLen < 15)
            //            {
            //                colWidth = 70;
            //            }
            //            else
            //            {
            //                colWidth = 60;
            //            }
            //        }

            //        if (totalWidthUsed + 50 > usablePageWidth)
            //        {
            //            break;

            //        }
            //        List<string> lines = WrapText(headerText, font, gfx, colWidth, rowHeight);
            //        double maxHt = rowHeight;
            //        maxHt += lines.Count * 5;

            //        if (maxHt > maxHeaderHeight)
            //        {
            //            maxHeaderHeight = maxHt;
            //        }

            //    }
            //    for (int i = startCol; i < dt.Columns.Count - 1; i++)
            //    {
            //        string headerText = dt.Columns[i].ColumnName;
            //        int maxLen = dt.AsEnumerable().Select(row => row[i]?.ToString()?.Length ?? 0).Max();
            //        string dataType = dt.Columns[i].DataType.Name;

            //        if (dataType == "Int16")
            //        {
            //            colWidth = 50;
            //        }
            //        else if (dataType == "Int32")
            //        {
            //            if (maxLen > 0 && maxLen < 8)
            //            {
            //                colWidth = 60;
            //            }
            //            else if (maxLen > 7 && maxLen < 13)
            //            {
            //                colWidth = 80;
            //            }

            //        }
            //        else if (dataType == "Int64" || dataType == "DateTime")
            //        {
            //            if (maxLen > 18)
            //            {
            //                colWidth = 95;
            //            }
            //            else
            //            {
            //                colWidth = 60;
            //            }

            //        }
            //        else if (dataType == "Object")
            //        {
            //            colWidth = 70;
            //        }
            //        else if (dataType == "String")
            //        {
            //            if (maxLen > 15 && maxLen < 25)
            //            {
            //                colWidth = 125;
            //            }
            //            else if (maxLen > 25 && maxLen < 45)
            //            {
            //                colWidth = 150;
            //            }
            //            else if (maxLen > 7 && maxLen < 15)
            //            {
            //                colWidth = 70;
            //            }
            //            else
            //            {
            //                colWidth = 60;
            //            }
            //        }

            //        if (totalWidthUsed + 50 > usablePageWidth)
            //        {
            //            break;

            //        }
            //        headerWidthList.Add(Convert.ToInt32(colWidth));
            //        dataTypeList.Add(dataType);
            //        totalWidthUsed += colWidth;
            //        startCol++;

            //        List<string> lines = WrapText(headerText, font, gfx, colWidth, rowHeight);

            //        int check = 0;

            //        for (int l = 0; l < lines.Count; l++)
            //        {
            //            if (check == 1)
            //            {
            //                rowHeight = 10;
            //            }

            //            gfx.DrawRectangle(XPens.Black, x, y, colWidth, maxHeaderHeight);
            //            tf.DrawString(lines[l], font, XBrushes.Black,
            //                new XRect(x + 2, y + (l * rowHeight), colWidth - (leftPadding + rightPadding), maxHeaderHeight));
            //            check = 1;
            //        }
            //        rowHeight = 20;
            //        x += colWidth;
            //    }

            //    y = maxHeaderHeight + 20; // details data starting position Y is added 
            //    x = margin;

            //    startCol = 0;
            //    // === Draw Rows ===
            //    int totalPages = 10;
            //    int pageNo = 1;
            //    bool addPage1Footer = true;
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (addPage1Footer == true)
            //        {
            //            addPage1Footer = false;
            //            maxY = maxY - 20;
            //        }
            //        if (y + rowHeight > maxY)
            //        {
            //            maxY = maxY + 20;
            //            pageNo = pageNo + 1;

            //            // Create new page
            //            page = document.AddPage();
            //            page.Size = PdfSharpCore.PageSize.A4;
            //            page.Orientation = PdfSharpCore.PageOrientation.Landscape;
            //            gfx = XGraphics.FromPdfPage(page);

            //            tf = new XTextFormatter(gfx);
            //            y = margin;

            //            //==========Page Number added========================
            //            XFont footerFont2 = new XFont("Arial Unicode MS", 10, XFontStyle.Regular);
            //            string pageNumberText2 = _localization.GetLocalizedHtmlString("lbl_Page") + pageNo + "/" + totalPages;   //$"Page {(i + 1).ToString("D2")}/{totalPages.ToString("D2")}";

            //            gfx.DrawString(
            //                pageNumberText2,
            //                footerFont2,
            //                XBrushes.Black,
            //                new XRect(0, page.Height - 30, page.Width - 40, 20), // Adjust bottom margin
            //                new XStringFormat
            //                {
            //                    Alignment = XStringAlignment.Far,         // Right-aligned
            //                    LineAlignment = XLineAlignment.Center      // Vertically centered within footer area
            //                }
            //            );
            //        }


            //        int hRow = 0;
            //        x = margin;

            //        for (int i = startCol; i < headerWidthList.Count; i++)
            //        {
            //            colWidth = headerWidthList[hRow]; // width will change as per Header
            //            string dType = dataTypeList[hRow];

            //            string cellText = row[i]?.ToString() ?? "";
            //            gfx.DrawRectangle(XPens.Black, x, y, colWidth, rowHeight);
            //            XRect rect = new XRect(x + leftPadding, y + (rowHeight - font.Height) / 2, colWidth - (leftPadding + rightPadding), rowHeight);

            //            if (dType.StartsWith("Int") || dType == "Double" || dType == "Single" || dType == "Decimal")
            //            {
            //                var format = new XStringFormat
            //                {
            //                    Alignment = XStringAlignment.Far // Right-align
            //                };
            //                gfx.DrawString(cellText, font, XBrushes.Black, rect, format);
            //            }
            //            else if (dType == "DateTime")
            //            {
            //                var format = new XStringFormat
            //                {
            //                    Alignment = XStringAlignment.Center,       // Center-align

            //                };
            //                gfx.DrawString(cellText, font, XBrushes.Black, rect, format);
            //            }
            //            else
            //            {
            //                var format = new XStringFormat
            //                {
            //                    Alignment = XStringAlignment.Near,       // Left-align

            //                };
            //                gfx.DrawString(cellText, font, XBrushes.Black, rect, format);

            //            }


            //            x += colWidth;
            //            hRow++;
            //        }
            //        y += rowHeight;

            //    }


            //    // Save the file
            //    using (FileStream stream = new FileStream(path, FileMode.Create))
            //    {
            //        document.Save(stream);
            //    }

            //    downloadFile = "/OutputCSV-PDF/" + fileName + ".pdf";

            //}
            //return Json(new { downloadpath = downloadFile, filename = fileName + ".pdf" });
            #endregion
        }
        public PdfSharpCore.PageSize SetPageSize(string pageSize)
        {
            PdfSharpCore.PageSize size = PdfSharpCore.PageSize.A4;

            if (pageSize.ToLower() == "letter")
            {
                size = PdfSharpCore.PageSize.Letter;
            }
            else if (pageSize.ToLower() == "tabloid")
            {
                size = PdfSharpCore.PageSize.Tabloid;
            }
            else if (pageSize.ToLower() == "legal")
            {
                size = PdfSharpCore.PageSize.Legal;
            }
            else if (pageSize.ToLower() == "statement")
            {
                size = PdfSharpCore.PageSize.Statement;
            }
            else if (pageSize.ToLower() == "executive")
            {
                size = PdfSharpCore.PageSize.Executive;
            }
            else if (pageSize.ToLower() == "a3")
            {
                size = PdfSharpCore.PageSize.A3;
            }
            else if (pageSize.ToLower() == "a4")
            {
                size = PdfSharpCore.PageSize.A4;
            }
            else if (pageSize.ToLower() == "a5")
            {
                size = PdfSharpCore.PageSize.A5;
            }
            else if (pageSize.ToLower() == "b4")
            {
                size = PdfSharpCore.PageSize.B4;
            }
            else if (pageSize.ToLower() == "b5")
            {
                size = PdfSharpCore.PageSize.B5;
            }
            else if (pageSize.ToLower() == "folio")
            {
                size = PdfSharpCore.PageSize.Folio;
            }
           
            return size;
        }
     
        public List<string> WrapText(string text, XFont font, XGraphics gfx, double maxWidth, double lineHeight)
        {
            List<string> lines = new List<string>();
            string currentLine = "";
            string[] words = text.Split(' ');

            
            string headevalue = string.Empty;
            string oldValue = string.Empty;
            string currentChar = string.Empty;
            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                oldValue = headevalue;
                currentChar = Convert.ToString(chars[i]);
                headevalue += currentChar;
                var size = gfx.MeasureString(headevalue, font);
                if (size.Width > maxWidth)
                {
                    currentLine = oldValue;
                    lines.Add(currentLine);
                    oldValue = string.Empty;
                    headevalue = currentChar;
                }
            }
            lines.Add(headevalue);
           
            return lines;
        }
    
        public string EscapeCsv(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            // Force text formatting if value is all digits with leading 0
            if (field.All(char.IsDigit) && field.StartsWith("0"))
            {
                return $"=\"{field}\"";
            }

            // Escape double quotes
            field = field.Replace("\"", "\"\"");

            // Wrap in quotes if it contains special characters
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return $"\"{field}\"";
            }

            return field;
        }
        public DataTable GetExportData(OutputCSVDetails csvDetails)
        {
            DataTable dt = new DataTable();
            if (csvDetails.SQL_PLSQL_TYPE == 0)
            {
                if (csvDetails.SQL_PLSQL_NAME.ToUpper().Contains("ORDER BY"))
                {

                    int orderByPos = csvDetails.SQL_PLSQL_NAME.ToUpper().LastIndexOf("ORDER BY");
                    int wherePos = csvDetails.SQL_PLSQL_NAME.ToUpper().LastIndexOf(" WHERE ");
                    int lastBracketPos = csvDetails.SQL_PLSQL_NAME.LastIndexOf(")"); // AFTER ORDER BY CHECKED ")"

                    bool isAfterLastBracket = orderByPos > lastBracketPos;
                    bool isBracketAfterWherePos = wherePos > lastBracketPos;
                    if (orderByPos >= 0 && wherePos > orderByPos && (isAfterLastBracket == true && isBracketAfterWherePos == true) || (isAfterLastBracket == false && isBracketAfterWherePos == false))
                    {
                        string sql = csvDetails.SQL_PLSQL_NAME.Substring(0, orderByPos) + csvDetails.SQL_PLSQL_NAME.Substring(wherePos);
                        string orderByClause = csvDetails.SQL_PLSQL_NAME.Substring(orderByPos, wherePos- orderByPos);
                        csvDetails.SQL_PLSQL_NAME = sql + " " + orderByClause;
                    }
                }
                dt = _csvService.GetCSVData(csvDetails.SQL_PLSQL_NAME);
            }
            else
            {
                StoredProcedureDetails procedureDetails = new StoredProcedureDetails();
                List<InputParameters> parametersList = new List<InputParameters>();
                InputParameters inputParameters = new InputParameters();

                foreach (var item in csvDetails.WHERE_CONDITION_LIST)
                {
                    inputParameters.PARAMETER_NAME = item.WHERE_CONDITION_COLUMN;
                    inputParameters.PARAMETER_VALUE = item.WHERE_CONDITION_VALUE;
                    string dataType = string.Empty;
                    if (item.WHERE_CONDITION_TYPE == 0)
                    {
                        dataType = "string";
                    }
                    else if (item.WHERE_CONDITION_TYPE == 1)
                    {
                        dataType = "number";
                    }
                    else if (item.WHERE_CONDITION_TYPE == 2 || item.WHERE_CONDITION_TYPE == 3 || item.WHERE_CONDITION_TYPE == 4 || item.WHERE_CONDITION_TYPE == 5)
                    {
                        dataType = "date";
                    }
                    else if (item.WHERE_CONDITION_TYPE == 6)
                    {
                        if (item.ON_OFF_FLAG_TYPE == 0)
                        {
                            dataType = "number";
                        }
                        else
                        {
                            dataType = "string";
                        }
                    }

                    inputParameters.DATA_TYPE = dataType;
                    parametersList.Add(inputParameters);
                }
                procedureDetails.PROCEDURE_NAME = csvDetails.SQL_PLSQL_NAME;
                procedureDetails.PARAMETERS = parametersList;
                dt = _csvService.GetCSVProcedureData(procedureDetails);
            }
            return dt;
        }
        public List<NameValue> ComposeWhereCondition(List<NameValue> data)
        {
            List<NameValue> resultData = new List<NameValue>();
            NameValue list = new NameValue();

            foreach (var result in data)
            {
                list = new NameValue();
                if (result.Name == _localization.GetLocalizedHtmlString("lbl_CharEqualTo"))
                {
                    list.CD = result.CD;
                    list.VALUE = " = ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_CharContains"))
                {
                    list.CD = result.CD;
                    list.VALUE = " LIKE '%NNN%' ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_CharStartWith"))
                {
                    list.CD = result.CD;
                    list.VALUE = " LIKE 'NNN%' ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_SignEqualTo"))
                {
                    list.CD = result.CD;
                    list.VALUE = " = ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_SignNotEqualTo"))
                {
                    list.CD = result.CD;
                    list.VALUE = " != ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_SignLessThan"))
                {
                    list.CD = result.CD;
                    list.VALUE = " < ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_SignLessThanEqualTo"))
                {
                    list.CD = result.CD;
                    list.VALUE = " <= ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_SignGreaterThan"))
                {
                    list.CD = result.CD;
                    list.VALUE = " > ";
                    list.Name = result.Name;

                }
                else if (result.Name == _localization.GetLocalizedHtmlString("lbl_SignGreaterThanEqualTo"))
                {
                    list.CD = result.CD;
                    list.VALUE = " >= ";
                    list.Name = result.Name;

                }
                resultData.Add(list);

            }
            return resultData;
        }
        private int GetActualHeaderWidth(int length)
        {
            int width = 0;
            switch (length)
            {
                case 1: width = 4; break;
                case 2: width = 8; break;
                case 3: width = 12; break;
                case 4: width = 16; break;
                case 5: width = 20; break;
                case 6: width = 24; break;
                case 7: width = 28; break;
                case 8: width = 32; break;
                case 9: width = 36; break;
                case 10: width = 40; break;
                case 11: width = 44; break;
                case 12: width = 48; break;
                case 13: width = 52; break;
                case 14: width = 56; break;
                case 15: width = 60; break;
                case 16: width = 64; break;
                case 17: width = 68; break;
                case 18: width = 72; break;
                case 19: width = 76; break;
                case 20: width = 80; break;
                case 21: width = 84; break;
                case 22: width = 88; break;
                case 23: width = 92; break;
                case 24: width = 96; break;
                case 25: width = 100; break;
                case 26: width = 104; break;
                case 27: width = 108; break;
                case 28: width = 112; break;
                case 29: width = 116; break;
                case 30: width = 120; break;
                case 31: width = 124; break;
                case 32: width = 128; break;
                case 33: width = 132; break;
                case 34: width = 136; break;
                case 35: width = 140; break;
                case 36: width = 144; break;
                case 37: width = 148; break;
                case 38: width = 152; break;
                case 39: width = 156; break;
                case 40: width = 160; break;
                case 41: width = 164; break;
                case 42: width = 168; break;
                case 43: width = 172; break;
                case 44: width = 176; break;
                case 45: width = 180; break;
                case 46: width = 184; break;
                case 47: width = 188; break;
                case 48: width = 192; break;
                case 49: width = 196; break;
                case 50: width = 200; break;

            }
            return width;
            
        }
    }
}
