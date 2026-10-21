using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class MakerMillSheetJfeEDIController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public MakerMillSheetJfeEDIController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult MakerMillSheetJfeEDIBatch()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

        [HttpPost]
        public ActionResult ReadCSVFile(IFormFile file1, IFormFile file2)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var list = new List<MakerMillSheetEDIDetails>();

            string getFileNames = $"{file1.FileName}, {file2.FileName}";


            // Dictionary for CSV1 data
            var csv1Dictionary = new Dictionary<string, MakerMillSheetEDIDetails>();

            // =========================
            // READ CSV1
            // =========================
            using (var reader = new StreamReader(
                file1.OpenReadStream(),
                Encoding.GetEncoding(932)))
            {
                var headers = reader.ReadLine()?.Split(',');

                if (headers == null)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "CSV1 header missing."
                    });
                }

                var columnIndexes = headers
                    .Select((name, index) => new
                    {
                        Name = name.Trim(),
                        Index = index
                    })
                    .ToDictionary(x => x.Name, x => x.Index);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var cols = line.Split(',');

                    string GetValue(string columnName)
                    {
                        if (!columnIndexes.ContainsKey(columnName))
                            return string.Empty;

                        int index = columnIndexes[columnName];

                        if (index >= cols.Length)
                            return string.Empty;

                        return cols[index]
                            ?.Replace("\"", "")
                            .Trim();
                    }

                    string orderNumber = GetValue("注文番号");
                    string lineNumber = GetValue("行番");

                    string key = $"{orderNumber}_{lineNumber}";

                    csv1Dictionary[key] = new MakerMillSheetEDIDetails
                    {
                        //FILENAME = getFileNames,
                        // Charge No -> CHARGE
                        CHARGE = RemoveLeadingZeros(GetValue("チャージＮｏ")),

                        // Chemical Data
                        CCNAME1 = "C",
                        CCACTUAL1 = RemoveLeadingZeros(GetValue("化学成分Ｃ")),

                        CCNAME2 = "SI",
                        CCACTUAL2 = RemoveLeadingZeros(GetValue("化学成分ＳＩ")),

                        CCNAME3 = "MN",
                        CCACTUAL3 = RemoveLeadingZeros(GetValue("化学成分ＭＮ")),

                        CCNAME4 = "P",
                        CCACTUAL4 = RemoveLeadingZeros(GetValue("化学成分Ｐ")),

                        CCNAME5 = "S",
                        CCACTUAL5 = RemoveLeadingZeros(GetValue("化学成分Ｓ")),

                        CCNAME6 = "CU",
                        CCACTUAL6 = RemoveLeadingZeros(GetValue("化学成分ＣＵ")),

                        CCNAME7 = "NI",
                        CCACTUAL7 = RemoveLeadingZeros(GetValue("化学成分ＮＩ")),

                        CCNAME8 = "CR",
                        CCACTUAL8 = RemoveLeadingZeros(GetValue("化学成分ＣＲ")),

                        CCNAME9 = "MO",
                        CCACTUAL9 = RemoveLeadingZeros(GetValue("化学成分ＭＯ")),

                        CCNAME10 = "NI+CR",
                        CCACTUAL10 = RemoveLeadingZeros(GetValue("化学成分ＮＩ＋ＣＲ")),

                        CCNAME11 = "AL",
                        CCACTUAL11 = RemoveLeadingZeros(GetValue("化学成分ＡＩ")),

                        CCNAME12 = "TI",
                        CCACTUAL12 = RemoveLeadingZeros(GetValue("化学成分ＴＩ")),

                        CCNAME13 = "B",
                        CCACTUAL13 = RemoveLeadingZeros(GetValue("化学成分Ｂ")),

                        CCNAME14 = "V",
                        CCACTUAL14 = RemoveLeadingZeros(GetValue("化学成分Ｖ")),

                        CCNAME15 = "CA",
                        CCACTUAL15 = RemoveLeadingZeros(GetValue("化学成分ＣＡ")),

                        CCNAME16 = "SN",
                        CCACTUAL16 = RemoveLeadingZeros(GetValue("化学成分ＳＮ")),

                        CCNAME17 = "NB",
                        CCACTUAL17 = RemoveLeadingZeros(GetValue("化学成分ＮＢ")),

                        CCNAME18 = "PB",
                        CCACTUAL18 = RemoveLeadingZeros(GetValue("化学成分ＰＢ")),

                        CCNAME19 = "AS",
                        CCACTUAL19 = RemoveLeadingZeros(GetValue("化学成分ＡＳ")),

                        CCNAME20 = "SB",
                        CCACTUAL20 = RemoveLeadingZeros(GetValue("化学成分ＳＢ")),

                        CCNAME21 = "T-O",
                        CCACTUAL21 = RemoveLeadingZeros(GetValue("化学成分Ｔ－Ｏ")),

                        CCNAME22 = "T-N",
                        CCACTUAL22 = RemoveLeadingZeros(GetValue("化学成分Ｔ－Ｎ"))
                    };
                }
            }

            // =========================
            // READ CSV2
            // =========================
            using (var reader = new StreamReader(
                file2.OpenReadStream(),
                Encoding.GetEncoding(932)))
            {
                var headers = reader.ReadLine()?.Split(',');

                if (headers == null)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "CSV2 header missing."
                    });
                }

                var columnIndexes = headers
                    .Select((name, index) => new
                    {
                        Name = name.Trim(),
                        Index = index
                    })
                    .ToDictionary(x => x.Name, x => x.Index);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var cols = line.Split(',');

                    string GetValue(string columnName)
                    {
                        if (!columnIndexes.ContainsKey(columnName))
                            return string.Empty;

                        int index = columnIndexes[columnName];

                        if (index >= cols.Length)
                            return string.Empty;

                        return cols[index]
                            ?.Replace("\"", "")
                            .Trim();
                    }

                    string orderNumber = GetValue("注文番号"); 

                    string lineNumber = GetValue("行番"); 


                    string key = $"{orderNumber}_{lineNumber}";

                    if (!csv1Dictionary.ContainsKey(key))
                        continue;

                    var item = csv1Dictionary[key];

                    // Product Code -> MATL_NAME_MAKER
                    item.MATL_NAME_MAKER = GetValue("品名製品符号"); 


                    // Dimension Size 1 -> DIM_D
                    decimal dimD = 0;

                    var dimValue = GetValue("寸法サイズ１");

                    if (!string.IsNullOrWhiteSpace(dimValue))
                    {
                        dimValue = dimValue
                            .Replace("\"", "")   // Remove quotes
                            .Replace(",", "")
                            .Trim();

                        decimal.TryParse(
                            dimValue,
                            out dimD);
                    }

                    item.DIM_D = dimD > 0
                        ? dimD / 100
                        : 0;

                    list.Add(item);
                }
            }

            return Ok(new
            {
                isSuccess = true,
                data = list
            });
        }

        private string RemoveLeadingZeros(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value
                .Replace("\"", "")
                .Trim();

            decimal number;

            if (decimal.TryParse(value, out number))
            {
                return number.ToString();
            }

            return value;
        }

        private T GetValue<T>(string line, int start, int end)
        {
            string errorMessage = string.Empty;
            try
            {
                int length = (end - start) + 1;

                if (line.Length < start - 1 + length)
                {
                    errorMessage = "Not Found";
                    return default;
                }

                string value = line.Substring(start - 1, length).Trim();

                value = value.TrimStart('0').TrimStart('.').TrimStart('0');

                if (string.IsNullOrEmpty(value))
                {
                    errorMessage = "Not Found";
                    return default;
                }

                if (typeof(T) == typeof(string))
                    return (T)(object)value;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                {
                    errorMessage = "Not Found";
                    return default;
                }
            }
        }
    }
}
