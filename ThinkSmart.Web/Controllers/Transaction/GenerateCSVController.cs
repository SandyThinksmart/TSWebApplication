using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using System.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Linq;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class GenerateCSVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GenerateCSV([FromBody] IEnumerable<Object> obj)
        {
            DataTable dt = ConvertToDataTable(obj);
            //List<DataObject> dataList = JsonConvert.DeserializeObject<List<DataObject>>(jsonData);
            return Json(new { IsSuccess = true });
        }
        //public DataTable ToDataTable<Object>(IEnumerable<Object> objects)
        //{
        //    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(objects);
        //    DataTable table = new DataTable();
        //    for (int i = 0; i < props.Count; i++)
        //    {
        //        PropertyDescriptor prop = props[i];
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    }
        //    object[] values = new object[props.Count];
        //    foreach (Object item in objects)
        //    {
        //        for (int i = 0; i < values.Length; i++)
        //            values[i] = props[i].GetValue(item) ?? DBNull.Value;
        //        table.Rows.Add(values);
        //    }
        //    return table;
        //}


        //private DataTable ConvertToDataTable(IEnumerable<object> data)
        //{
        //    DataTable dataTable = new DataTable();


        //    if (data != null && data.Any())
        //    {

        //        var properties = data.First().GetType().GetProperties();


        //        foreach (var property in properties)
        //        {
        //            dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
        //        }


        //        foreach (var item in data)
        //        {
        //            DataRow row = dataTable.NewRow();
        //            foreach (var property in properties)
        //            {
        //                row[property.Name] = property.GetValue(item) ?? DBNull.Value;
        //            }
        //            dataTable.Rows.Add(row);
        //        }
        //    }

        //    return dataTable;
        //}

        private DataTable ConvertToDataTable(IEnumerable<object> data)
        {
            DataTable dataTable = new DataTable();
            
            if (data != null && data.Any())
            {              
                var properties = data.First().GetType().GetProperties();

                
                foreach (var property in properties)
                {
                    string columnName = property.Name;
                    int count = 1;

                    
                    while (dataTable.Columns.Contains(columnName))
                    {
                        columnName = $"{property.Name}_{count}";
                        count++;
                    }

                    dataTable.Columns.Add(columnName, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                }

                // Add rows to DataTable
                foreach (var item in data)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var property in properties)
                    {
                        row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }


    }
}
