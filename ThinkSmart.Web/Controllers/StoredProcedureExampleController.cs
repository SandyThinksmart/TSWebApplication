using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web.Controllers
{
    public class StoredProcedureExampleController : Controller
    {
        private readonly IStoredProcedureDemoServices _storedProcedureServices;
        private readonly IViewRenderService _viewRenderService;
        private readonly LocalizationServices _localization;
        private readonly IHostEnvironment _hostEnvironment;
        public StoredProcedureExampleController(LocalizationServices localization, IStoredProcedureDemoServices storedProcedureServices, IViewRenderService viewRenderService, IHostEnvironment hostEnvironment)
        {

            _localization = localization;
            _storedProcedureServices = storedProcedureServices;
            _viewRenderService = viewRenderService;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ExecuteWithQueryOnly()
        {
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string tempDirectory = "";

            var result = _storedProcedureServices.TrialDemo1();

            
            tempDirectory = Path.Combine(contentRootPath, "wwwroot","CsvFile\\Sample.csv");

            if (System.IO.File.Exists(tempDirectory))
            {
                // If file found, delete it
                System.IO.File.Delete(tempDirectory);
                
            }
           

            using (var streamWriter = new StreamWriter(tempDirectory, true, System.Text.Encoding.UTF8))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    foreach (DataColumn dColumn in result.Columns)
                    {
                        csvWriter.WriteField(dColumn.ColumnName);
                    }
                    csvWriter.NextRecord();

                    foreach (DataRow dRow in result.Rows)
                    {
                        foreach (DataColumn dc in result.Columns)
                        {
                            csvWriter.WriteField(dRow[dc]);
                        }
                        csvWriter.NextRecord();
                    }

                    streamWriter.ToString();
                }
            }
                return Ok(new { IsSuccess = true, message = "/CsvFile/Sample.csv" });
        }
        public IActionResult ExecuteWithoutResult1()
        {
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string tempDirectory = "";

            var result = _storedProcedureServices.TrialDemo2();


            tempDirectory = Path.Combine(contentRootPath, "wwwroot", "CsvFile\\Sample.csv");

            if (System.IO.File.Exists(tempDirectory))
            {
                // If file found, delete it
                System.IO.File.Delete(tempDirectory);

            }


            using (var streamWriter = new StreamWriter(tempDirectory, true, System.Text.Encoding.UTF8))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    foreach (DataColumn dColumn in result.Columns)
                    {
                        csvWriter.WriteField(dColumn.ColumnName);
                    }
                    csvWriter.NextRecord();

                    foreach (DataRow dRow in result.Rows)
                    {
                        foreach (DataColumn dc in result.Columns)
                        {
                            csvWriter.WriteField(dRow[dc]);
                        }
                        csvWriter.NextRecord();
                    }

                    streamWriter.ToString();
                }
            }
            return Ok(new { IsSuccess = true, message = "/CsvFile/Sample.csv" });
        }
        public IActionResult ExecuteWithResult()
        {
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string tempDirectory = "";

            var result = _storedProcedureServices.TrialDemo3();


            tempDirectory = Path.Combine(contentRootPath, "wwwroot", "CsvFile\\Sample.csv");

            if (System.IO.File.Exists(tempDirectory))
            {
                // If file found, delete it
                System.IO.File.Delete(tempDirectory);

            }


            using (var streamWriter = new StreamWriter(tempDirectory, true, System.Text.Encoding.UTF8))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    foreach (DataColumn dColumn in result.Columns)
                    {
                        csvWriter.WriteField(dColumn.ColumnName);
                    }
                    csvWriter.NextRecord();

                    foreach (DataRow dRow in result.Rows)
                    {
                        foreach (DataColumn dc in result.Columns)
                        {
                            csvWriter.WriteField(dRow[dc]);
                        }
                        csvWriter.NextRecord();
                    }

                    streamWriter.ToString();
                }
            }
            return Ok(new { IsSuccess = true, message = "/CsvFile/Sample.csv" });
        }
    }
}
