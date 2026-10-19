using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TSWebApplication.Domain.Utility
{
    public static class CommonConvertMethods
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                    {
                        if (dr[column.ColumnName] == DBNull.Value)
                        {
                            if (pro.PropertyType == typeof(string))
                            {
                                pro.SetValue(obj, null, null);
                            }
                            else if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(double) || pro.PropertyType == typeof(long))
                            {
                                pro.SetValue(obj, 0, null);
                            }
                            else if (pro.PropertyType == typeof(bool))
                            {
                                pro.SetValue(obj, false, null);
                            }
                            else if (pro.PropertyType == typeof(DateTime))
                            {
                                pro.SetValue(obj, DateTime.MinValue, null);
                            }
                            else if (pro.PropertyType == typeof(char))
                            {
                                pro.SetValue(obj, '\0', null);
                            }
                            else
                            {
                                pro.SetValue(obj, null, null);
                            }
                        }
                        else
                        {
                          
                            pro.SetValue(obj, dr[column.ColumnName], null);
                            
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }

        public static T ConvertDataTableToClass<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data.FirstOrDefault();
        }
    }
}
