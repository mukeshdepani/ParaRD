using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace FastDB
{
    /// <summary>
    /// Class that simulates a DataAccess module.
    /// </summary>
    public static class DataAccess
    {
        
        private static DataTable Results()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("FirstName"));
            dt.Columns.Add(new DataColumn("LastName"));
            DataRow dr1 = dt.NewRow();
            dr1["FirstName"] = "Gajendra";
            dr1["LastName"] = "Medatia";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2["FirstName"] = "Bhakta";
            dr2["LastName"] = "yadlapalli";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["FirstName"] = "Amit";
            dr3["LastName"] = "Mandvia";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4["FirstName"] = "Rakesh";
            dr4["LastName"] = "Gajera";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5["FirstName"] = "James";
            dr5["LastName"] = "Bond";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6["FirstName"] = "John";
            dr6["LastName"] = "Doe";
            dt.Rows.Add(dr6);
            DataRow dr7 = dt.NewRow();
            dr7["FirstName"] = "Jill";
            dr7["LastName"] = "Medatia";
            dt.Rows.Add(dr7);
            DataRow dr8 = dt.NewRow();
            dr8["FirstName"] = "Chavo";
            dr8["LastName"] = "Garrero";
            dt.Rows.Add(dr8);
            DataRow dr9 = dt.NewRow();
            dr9["FirstName"] = "Bill";
            dr9["LastName"] = "Gate";
            dt.Rows.Add(dr9);
            DataRow dr10 = dt.NewRow();
            dr10["FirstName"] = "MotiChand";
            dr10["LastName"] = "Patel";
            dt.Rows.Add(dr10);
            return dt;
        }

        private static DataTable MySqLTestResult(string connectionString, string query)
        {
            DataTable table = new DataTable();
            DataSet myData = new DataSet();
            MySql.Data.MySqlClient.MySqlCommand cmd;
            MySql.Data.MySqlClient.MySqlDataAdapter myAdapter;
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                cmd = new MySql.Data.MySqlClient.MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                myAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();
                myAdapter.SelectCommand = cmd;
                myAdapter.Fill(table);

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        break;
                    case 1045:
                        break;
                }
            }

            return table;
        }

        public static DataView GetMySQLTestData(string connectionString, string query, int startRow, int rowPageSize, int startColumn, int columnPageSize, string sortColumn, bool ascending, out int totalRows, out int totalColumns)
        {
            string Query = query;
            if (sortColumn != null)
            {
                if (ascending)
                {
                    Query = Query + " ORDER BY " + sortColumn + " ASC";
                }
                else
                {
                    Query = Query + " ORDER BY " + sortColumn + " DESC";
                }
            }

            DataTable OriginalTable = MySqLTestResult(connectionString, Query);
            totalRows = OriginalTable.Rows.Count;
            totalColumns = OriginalTable.Columns.Count;

            DataTable filteredResult = new DataTable();
            
            for (int i = startColumn; i < startColumn + columnPageSize && i < totalColumns; i++)
            {
                DataColumn dc = new DataColumn(OriginalTable.Columns[i].ColumnName);
                filteredResult.Columns.Add(dc);

            }
            for (int i = startRow; i < startRow + rowPageSize && i < totalRows; i++)
            {
                DataRow dr = filteredResult.NewRow();
                filteredResult.ImportRow(OriginalTable.Rows[i]);

            }

            return filteredResult.AsDataView();
        }
    
    }
}
