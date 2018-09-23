using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;


namespace Test_MSDynamics.DataHelper
{
    public static class ExcelDataReader
    {
        private static DataTable GetDataTable(string sql, string connectionString)
        {
            DataTable dt = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {
                        dt.Load(rdr);
                        return dt;
                    }
                }
            }
        }
        public static DataTable getDataFromSheet(string sheetName, string path)
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", path);            
            OleDbConnection connection = new OleDbConnection(connString);
            DataTable dataTable = GetDataTable("SELECT * FROM [" + sheetName + "$]", connString);
            return dataTable;
        }
    }
}
