﻿using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FinalYearProject.Models
{

    public class ExcelData
    {
        string _path;
        public ExcelData(string path)
        {
            _path = path;
        }

        public IExcelDataReader getExcelReader()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);
            //
            // We return the interface, so that
            IExcelDataReader reader = null;
            try
            {
                if (_path.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                if (_path.EndsWith(".ods"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".csv"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                return reader;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<string> getWorksheetNames()
        {
            var reader = this.getExcelReader();
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
            return sheets;
        }
        public IEnumerable<DataRow> getData(string sheet, bool firstRowIsColumnNames = true)
        {
            var reader = this.getExcelReader();
            //reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
            //string SheetName = getWorksheetNames().FirstOrDefault();
            var workSheet = reader.AsDataSet().Tables[sheet];
            var filteredRows = workSheet.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull)));
            // TTliteUtil.Util.WriteToEventLog("Sheet read: "+workSheet.ToString());
            var rows = from DataRow a in filteredRows select a;
            return rows;
        }
    }















    //    public static class ExcelToIList
    //    {
    //        public static DataTable ConvertCSVtoDataTable(string strFilePath)
    //        {
    //            DataTable dt = new DataTable();
    //            using (StreamReader sr = new StreamReader(strFilePath))
    //            {
    //                string[] headers = sr.ReadLine().Split(',');
    //                foreach (string header in headers)
    //                {
    //                    dt.Columns.Add(header);
    //                }
    //                while (!sr.EndOfStream)
    //                {
    //                    string[] rows = sr.ReadLine().Split(',');
    //                    if (rows.Length > 1)
    //                    {
    //                        DataRow dr = dt.NewRow();
    //                        for (int i = 0; i < headers.Length; i++)
    //                        {
    //                            dr[i] = rows[i].Trim();
    //                        }
    //                        dt.Rows.Add(dr);
    //                    }
    //                }
    //            }
    //            var studentDetails = ConvertDataTable<dynamic>(dt);
    //            return dt;
    //        }

    //        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
    //        {
    //            OleDbConnection oledbConn = new OleDbConnection(connString);
    //            DataTable dt = new DataTable();
    //            try
    //            {
    //                oledbConn.Open();
    //                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
    //                {
    //                    OleDbDataAdapter oleda = new OleDbDataAdapter();
    //                    oleda.SelectCommand = cmd;
    //                    DataSet ds = new DataSet();
    //                    oleda.Fill(ds);
    //                    dt = ds.Tables[0];
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //            }
    //            finally
    //            {
    //                oledbConn.Close();
    //            }
    //            return dt;
    //        }

    //        public static List<T> ConvertDataTable<T>(DataTable dt)
    //        {
    //            List<T> data = new List<T>();
    //            foreach (DataRow row in dt.Rows)
    //            {
    //                T item = GetItem<T>(row);
    //                if (item != null)
    //                {
    //                    data.Add(item);
    //                }

    //            }
    //            return data;
    //        }
    //        private static T GetItem<T>(DataRow dr)
    //        {
    //            T NullObj = Activator.CreateInstance<T>();
    //            try
    //            {
    //                Type temp = typeof(T);
    //                T obj = Activator.CreateInstance<T>();

    //                foreach (DataColumn column in dr.Table.Columns)
    //                {
    //                    int count = 1;
    //                    foreach (PropertyInfo pro in temp.GetProperties())
    //                    {
    //                        if (pro.Name == column.ColumnName)
    //                        {
    //                            //   pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
    //                            pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType), null);
    //                            // Convert.ChangeType(value, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType)

    //                        }
    //                        else
    //                            continue;
    //                        count++;
    //                    }
    //                }
    //                return obj;

    //            }
    //            catch (Exception ex)
    //            {
    //                return NullObj;
    //            }
    //        }

    //    }
}
