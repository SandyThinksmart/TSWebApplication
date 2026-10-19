using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using TSWebApplication.Domain.Resources;

namespace TSWebApplication.Domain.Utility
{
    public class UIHelper
    {
        private static LocalizationServices _localizationServices;
        private static IHttpContextAccessor _httpContext;

        public static void UIHeplerConfigure(LocalizationServices localizationServices, IHttpContextAccessor httpContext)
        {
            _localizationServices = localizationServices;
            _httpContext = httpContext;
        }
        public static bool IsValidDate(string _dateText)
        {
            try
            {
                DateTime _date = Convert.ToDateTime(_dateText);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static StringBuilder GetErrorMessageForInactiveMaster(string dataValue, string masterName, string inactiveLabel, string activeLabel)
        {
           
            StringBuilder inactiveFieldsMsg = new StringBuilder();
            inactiveFieldsMsg.Append(_localizationServices.GetLocalizedHtmlString("valMsg_COM_CanNotUseInactiveMaster"));
            inactiveFieldsMsg.Replace("DATA_VALUE", dataValue);
            inactiveFieldsMsg.Replace("MASTER_NAME", masterName);
            inactiveFieldsMsg.Replace("INACTIVE", inactiveLabel);
            inactiveFieldsMsg.Replace("ACTIVE", activeLabel);
            inactiveFieldsMsg.Append(Environment.NewLine);
            return inactiveFieldsMsg;
        }

        public static void ToCSV(DataTable dtDataTable, string strFilePath)
        {

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames
                    = dtDataTable.Columns.Cast<DataColumn>().
                        Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dtDataTable.Rows)
            {
                IEnumerable<string> fields
                    = row.ItemArray.Select(
                    field => (Convert.ToString(field)));
                // field => (field.ToString()).Replace(",", " "));
                sb.AppendLine(string.Join(",", fields));
            }

           
            using (var streamWriter = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    foreach (DataColumn dColumn in dtDataTable.Columns)
                    {
                        csvWriter.WriteField(dColumn.ColumnName);
                    }
                    csvWriter.NextRecord();

                    foreach (DataRow dRow in dtDataTable.Rows)
                    {
                        foreach (DataColumn dc in dtDataTable.Columns)
                        {
                            csvWriter.WriteField(Convert.ToString(dRow[dc]).Replace(Environment.NewLine," "));
                        }
                        csvWriter.NextRecord();
                    }

                    streamWriter.ToString();
                }
            }
        }
        public static string GetCultureName()
        {
            return CultureInfo.CurrentCulture.Name;
        }
      public static double Clamp(double value, double min = 0.0, double max = 1.0)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static double Interpolate(double start1, double end1, double value1, double start2, double end2)
        {
            if (end1 == start1)
                return start2; // Prevent divide-by-zero
            double percent = (value1 - start1) / (end1 - start1);
            percent = Clamp(percent);
            return start2 + percent * (end2 - start2);
        }
        public static (double x, double y, double a, double b) InterpolateChart1ToChart2(
        (double x, double y, double a, double b) chart1Start,
        (double x, double y, double a, double b) chart1End,
        (double x, double y, double a, double b) chart1Value,
        (double x, double y, double a, double b) chart2Start,
        (double x, double y, double a, double b) chart2End)
        {
            double x = Interpolate(chart1Start.x, chart1End.x, chart1Value.x, chart2Start.x, chart2End.x);
            double y = Interpolate(chart1Start.y, chart1End.y, chart1Value.y, chart2Start.y, chart2End.y);
            double a = Interpolate(chart1Start.a, chart1End.a, chart1Value.a, chart2Start.a, chart2End.a);
            double b = Interpolate(chart1Start.b, chart1End.b, chart1Value.b, chart2Start.b, chart2End.b);

            return (Math.Round(x, 2), Math.Round(y, 2), Math.Round(a, 2), Math.Round(b, 2));
        }
    }
    }
