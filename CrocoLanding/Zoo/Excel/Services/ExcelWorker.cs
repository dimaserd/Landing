using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using Zoo.Excel.Models;

namespace Zoo.Excel.Services
{
    public static class ExcelWorker
    {
        /// <summary>
        /// Перекладывает первый лист из таблицы Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string excelFilePath) where T : class, new()
        {
            List<Dictionary<string, object>> listOfDicts;

            using (var dt = ExcelToDataTable(excelFilePath))
            {
                listOfDicts = ToDictionaryList(dt);
            }

            var descriptions = GetMyPropertyDescriptions(typeof(T));

            foreach (var dict in listOfDicts)
            {
                foreach (var description in descriptions)
                {
                    if (!dict.ContainsKey(description.DisplayName))
                    {
                        continue;
                    }

                    dict[description.PropertyName] = dict[description.DisplayName];
                    dict.Remove(description.DisplayName);
                }
            }

            return listOfDicts.Select(x => x.ToObject<T>()).ToList();
        }

        public static List<Dictionary<string, object>> ToDictionaryList(DataTable dataTable)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (var column in dataTable.Columns)
                {
                    var columnName = column.ToString();

                    var value = row[columnName];

                    dict[columnName] = value;
                }

                list.Add(dict);
            }

            return list;
        }

        public static List<MyPropertyDescription> GetMyPropertyDescriptions(Type type)
        {
            var result = new List<MyPropertyDescription>();

            var props = TypeDescriptor.GetProperties(type);

            foreach (PropertyDescriptor prop in props)
            {
                result.Add(new MyPropertyDescription
                {
                    DisplayName = GetDisplayNameForProperty(prop),
                    PropertyName = prop.Name,
                    Type = prop.PropertyType
                });
            }

            return result;
        }


        public static DataTable ToDataTable<T>(this IList<T> data, string tableName = null, bool displayFromAttr = false)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));

            var table = new DataTable
            {
                TableName = tableName ?? typeof(T).Name
            };

            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];

                var propName = displayFromAttr ? GetDisplayNameForProperty(prop) : prop.Name;

                table.Columns.Add(propName, prop.PropertyType);
            }

            var values = new object[props.Count];

            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        private static string GetDisplayNameForProperty(PropertyDescriptor prop)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            var display = prop.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

            if (display == null)
            {
                throw new NullReferenceException($"Атрибут Display не указан на свойстве {prop.Name}");
            }

            return display.Name;
        }

        public static void SaveListToExcel<T>(IList<T> list, string filePath, string tableName = null, bool fromDisplayAttr = false)
        {
            var dt = list.ToDataTable(tableName, fromDisplayAttr);

            SaveDataTableToExcel(dt, filePath);
        }

        public static void SaveDataTableToExcel(DataTable dt, string filePath)
        {
            using(var ds = new DataSet())
            {
                ds.Tables.Add(dt);

                SaveDataSetToExcel(ds, filePath);
            }
        }

        public static void SaveDataSetToExcel(DataSet ds, string filePath)
        {
            var ext = Path.GetExtension(filePath);

            if (ext != ".xlsx")
            {
                throw new Exception("Недопустимое разрешение файла");
            }

            using (var wb = new XLWorkbook())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    wb.Worksheets.Add(dt, dt.TableName);
                }

                wb.SaveAs(filePath);
            }
        }

        public static DataTable ExcelToDataTable(string filePath)
        {
            //Open the Excel file using ClosedXML.
            using (var workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                var workSheet = workBook.Worksheet(1);

                //Create a new DataTable.
                var dt = new DataTable();

                //Loop through the Worksheet rows.
                var firstRow = true;
                foreach (var row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        var i = 0;
                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                return dt;
            }
        }
    }
}
