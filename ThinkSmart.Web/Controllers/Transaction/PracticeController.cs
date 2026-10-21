using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using Image = Google.Cloud.Vision.V1.Image;
//using OpenCvSharp;
//using Point = OpenCvSharp.Point;
using System.Linq;

namespace TSWebApplication.Web.Controllers.Transaction
{

    public class PracticeController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;
        string tessdata = string.Empty;
        public PracticeController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;

        }
            public IActionResult GraphSample()
        {
            return View();
        }
        public IActionResult LineGraph1()
        {
            return View();
        }
        public IActionResult LineGraph2()
        {
            return View();
        }

        public IActionResult PDFSample()
        {
            return View();
        }
        public IActionResult PdfToText()
        {
            //string pdfPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\MakerMillsheet\\");
            

            //using (var document = PdfiumViewer.PdfDocument.Load(pdfPath+ "20241007145648-0006.pdf"))
            //{
            //    // Set the page you want to render (e.g., the first page)
            //    int pageCount = document.PageCount; // Render the first page (index starts from 0)
            //    var image = document.Render(0, 300, 300, true);
            //    Rectangle cropRect = new Rectangle(108, 152, 137-108, 166-152); // x, y width, height

            //    using (Bitmap croppedImage = new Bitmap(cropRect.Width, cropRect.Height))
            //    {
            //        using (Graphics g = Graphics.FromImage(croppedImage))
            //        {
            //            g.DrawImage(image, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel);
            //        }
            //        pdfPath = pdfPath + "Sample.png";
            //        croppedImage.Save(pdfPath , System.Drawing.Imaging.ImageFormat.Png);
            //    }
            //}

           
            return View();
        }
        public IActionResult PdfToText1()
        {
          
            // string imgPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\MakerMillsheet\\");
            //string imagePath = Path.Combine(imgPath, "MillSheet.png");
            // string outputPath = Path.Combine(imgPath, "boxed_output.png");

            // // Load grayscale image
            // Mat gray = Cv2.ImRead(imagePath, ImreadModes.Grayscale);

            // // Convert to binary
            // Mat binary = new();
            // Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

            // // Detect horizontal lines
            // int hSize = binary.Cols / 29;
            // Mat hKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(hSize, 1));
            // Mat horizontal = binary.Clone();
            // Cv2.Erode(horizontal, horizontal, hKernel);
            // Cv2.Dilate(horizontal, horizontal, hKernel);

            // // Detect vertical lines
            // int vSize = binary.Rows / 29;
            // Mat vKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(1, vSize));
            // Mat vertical = binary.Clone();
            // Cv2.Erode(vertical, vertical, vKernel);
            // Cv2.Dilate(vertical, vertical, vKernel);

            // // Combine both masks
            // Mat mask = new();
            // Cv2.Add(horizontal, vertical, mask);

            // // Find contours
            // Cv2.FindContours(mask, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // // Draw boxes on color image
            // Mat output = Cv2.ImRead(imagePath, ImreadModes.Color);
            // Console.WriteLine("Detected Boxes:");
            // foreach (var cnt in contours)
            // {
            //     var rect = Cv2.BoundingRect(cnt);
            //     if (rect.Width > 30 && rect.Height > 10)
            //     {
            //         Console.WriteLine($"X={rect.X}, Y={rect.Y}, W={rect.Width}, H={rect.Height}");
            //         var topLeft = (rect.X, rect.Y);
            //         var topRight = (rect.X + rect.Width, rect.Y);
            //         var bottomLeft = (rect.X, rect.Y + rect.Height);
            //         var bottomRight = (rect.X + rect.Width, rect.Y + rect.Height);

            //         Cv2.Rectangle(output, rect, new Scalar(0, 0, 255), 2);
            //     }
            // }

            // // Save output image
            // Cv2.ImWrite(outputPath, output);
            // Console.WriteLine("Saved to: " + outputPath);

            //string outputDir = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\MakerMillsheet\\"); ;
            //Directory.CreateDirectory(outputDir);

            //using (var document = PdfiumViewer.PdfDocument.Load(pdfPath))
            //{
            //    for (int page = 0; page < document.PageCount; page++)
            //    {
            //        using (var image = document.Render(page, 300, 300, true))
            //        {
            //            string outputPath = Path.Combine(outputDir, $"page-{page}.png");
            //            image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
            //            Console.WriteLine($"Saved: {outputPath}");
            //        }
            //    }
            //}


            // Call the OCR function
            // AIzaSyAXnoEjPkuutw8qS0b2kXPsHwY-X6OUlcI
            //  MainCall();

            return View();
        }

        
        public IActionResult GetPDFData(IFormFile file)
        {
            string fileName = string.Empty;
            string imgPath = "";
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\MakerMillsheet");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);

            }
            string pdfPath = Path.Combine(path + "\\" + file.FileName);


            using (var document = PdfiumViewer.PdfDocument.Load(pdfPath))
            {
                for (int page = 0; page < document.PageCount; page++)
                {
                    using (var image = document.Render(page, 300, 300, true))
                    {
                        string outputPath = Path.Combine(path, "MillSheet.png");
                        image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }

         

            return Ok(new { message = "File uploaded successfully" });

        }
        static async void MainCall()
        {
            
        }
        public IActionResult Upload(IFormFile file)
        {
            string fileName = string.Empty;
            string imgPath = "";
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\MakerMillsheet");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            fileName = Path.GetFileName(file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                file.CopyTo(stream);

                }
            string pdfPath = Path.Combine(path+ "\\"+file.FileName);
            

            using (var document = PdfiumViewer.PdfDocument.Load(pdfPath))
            {
                for (int page = 0; page < document.PageCount; page++)
                {
                    using (var image = document.Render(page, 300, 300, true))
                    {
                        string outputPath = Path.Combine(path, "MillSheet.png");
                        image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
          
           CropPdfForCompositionData(pdfPath, 190);

            return Ok(new { message = "File uploaded successfully" });
            
        }
        public void CropPdfForCompositionData(string path, int startRange)
        {
            List<int> startPosition = new List<int>();
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\MakerMillsheet\\");

            SaveImage(path, 30, 150, 150, 20, filePath + "\\specification.png"); // SPECIFICATON FIELD
            SaveImage(path, 30, 205, 60, 60, filePath + "\\heatno.png"); // HEAT NO FIELD
            SaveImage(path, 110, 205, 60, 60, filePath + "\\dimention.png"); // DIMENTION FIELD
            SaveImage(path, 210, 205, 60, 60, filePath + "\\qty.png"); // QTY FIELD
            SaveImage(path, 240, 205, 60, 60, filePath + "\\wight.png"); // WIEGHT FIELD

            SaveImage(path, 385, 210, 25, 55, filePath + "\\composition1.png"); // composition1 FIELD
            SaveImage(path, 408, 210, 25, 55, filePath + "\\composition2.png"); // composition2 FIELD
            SaveImage(path, 430, 210, 25, 55, filePath + "\\composition3.png"); // composition3 FIELD
            SaveImage(path, 452, 210, 25, 55, filePath + "\\composition4.png"); // composition4 FIELD
            SaveImage(path, 474, 210, 25, 55, filePath + "\\composition5.png"); // composition5 FIELD
            SaveImage(path, 476, 210, 25, 55, filePath + "\\composition6.png"); // composition6 FIELD
            SaveImage(path, 498, 210, 25, 55, filePath + "\\composition7.png"); // composition7 FIELD
            SaveImage(path, 520, 210, 25, 55, filePath + "\\composition8.png"); // composition8 FIELD
                                   
            SaveImage(path, 545, 213, 25, 55, filePath + "\\composition9.png"); // composition9 FIELD
            SaveImage(path, 568, 213, 25, 55, filePath + "\\composition10.png"); // composition10 FIELD
            SaveImage(path, 590, 213, 25, 55, filePath + "\\composition11.png"); // composition11 FIELD
            SaveImage(path, 614, 213, 25, 55, filePath + "\\composition12.png"); // composition12 FIELD
            SaveImage(path, 638, 213, 25, 55, filePath + "\\composition13.png"); // composition13 FIELD
            SaveImage(path, 661, 213, 25, 55, filePath + "\\composition14.png"); // composition14 FIELD
            SaveImage(path, 684, 213, 25, 55, filePath + "\\composition15.png"); // composition15 FIELD
            SaveImage(path, 708, 213, 25, 55, filePath + "\\composition16.png"); // composition16 FIELD
            SaveImage(path, 732, 213, 25, 55, filePath + "\\composition17.png"); // composition17 FIELD


        }
        public void SaveImage(string path, int xRange,  int yRange, int width, int height, string imgPath)
        {
            using (var document = PdfiumViewer.PdfDocument.Load(@path))
            {
                // Set the page you want to render (e.g., the first page)
                int pageCount = document.PageCount; // Render the first page (index starts from 0)
                var image = document.Render(0, 300, 300, true);
                Rectangle cropRect = new Rectangle(xRange, yRange, width, height);

                    using (Bitmap croppedImage = new Bitmap(cropRect.Width, cropRect.Height))
                    {
                        using (Graphics g = Graphics.FromImage(croppedImage))
                        {
                            g.DrawImage(image, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel);
                        }

                        croppedImage.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
               
            }
        }
        [HttpPost]
        public IActionResult PdfToText(List<IFormFile> postedFiles)
        {
            string fileName = string.Empty;
            string imgPath = "";
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Uploads");
          
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (IFormFile postedFile in postedFiles)
            {
                 fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                 
                }
            }

 

            

           
            return View();
        }
       
       
        [HttpPost]
        public IActionResult ConvertPdfToText([FromForm] List<DataList> file)
        {
            return Ok();
        }

        }

    public class DataList
    {
        public IFormFile file { get; set; }
    }
    public class Zone
    {
        public string Name { get; set; }  // Optional: for labeling
        public float MinX { get; set; }
        public float MaxX { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
    }
    public class WordModel
    {
        public string Text { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
