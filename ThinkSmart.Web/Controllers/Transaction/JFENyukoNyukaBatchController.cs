using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.Formula.Functions;
using System.Data;



namespace TSWebApplication.Web.Controllers.Transaction
{
    public class JFENyukoNyukaBatchController : Controller
    {
        public IActionResult JFENyukoNyukaBatch()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }
        [HttpPost]
        public ActionResult ReadCSVFile(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var list = new List<JFENyukoNyukaBatchDetails>();

            using (var reader = new StreamReader(
                file.OpenReadStream(),
                Encoding.GetEncoding(932)))
            {
                var headers = reader.ReadLine()?.Split(',');

                if (headers == null)
                {
                    return Ok(new
                    {
                        isSuccess = false,
                        message = "CSV header missing."
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

                    var item = new JFENyukoNyukaBatchDetails
                    {
                        TYPE_OF_DATA = RemoveLeadingZeros(GetValue("データ種別")),
                        
                        CHARGE = RemoveLeadingZeros(GetValue("チャージＮｏ")),

                        INDIVIDUAL_NUMBER = RemoveLeadingZeros(GetValue("個体番号")),

                        CHUUBAN = RemoveLeadingZeros(GetValue("注文番号")),

                        GYOUBAN = RemoveLeadingZeros(GetValue("行番")),

                        QUNTITY = RemoveLeadingZeros(GetValue("質量")),

                        ROLLING_NUMBER = RemoveLeadingZeros(GetValue("圧延番号")),

                        ASSORTMENT_RECORD_DATE = RemoveLeadingZeros(GetValue("荷揃実績年月日")),

                        ROLLING_DATE = RemoveLeadingZeros(GetValue("圧延日")),

                        DIRECT_STOCK_FLAG = RemoveLeadingZeros(GetValue("直送在庫サイン")),

                        DIRECT_SHUKKA_DATE = RemoveLeadingZeros(GetValue("直送出荷年月日")),

                        SENDAI_STOCK_FLAG_VIA_BASE = RemoveLeadingZeros(GetValue("基地経由仙台在庫サイン")),

                        SALES_SENDAI_SHUKKA_DATE = RemoveLeadingZeros(GetValue("売上仙台出荷年月日")),

                        SALES_SHIP_SIGN = RemoveLeadingZeros(GetValue("売上船中サイン")),

                        SALES_BASE_FUTO_DATE = RemoveLeadingZeros(GetValue("売上基地入庫年月日")),

                        SALES_BASE_STOCK_FLAG = RemoveLeadingZeros(GetValue("売上基地在庫サイン")),

                    };
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
