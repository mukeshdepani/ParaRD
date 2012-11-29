using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using SQLBuilder;
using System.Globalization;
using SQLBuilder.Enums;
using System.IO;
using System.Xml.Serialization;


namespace MySQLData.DataAccess
{
    public class ADODataBridge
    {
        public ADODataBridge()
        {
        }

        public static DataTable getData(string connectionString, string query)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                return dataSet.Tables[0];
            }
            finally
            {
                connection.Close();
            }
        }

        public static int getSelectedRowsCount(string connectionString, SelectQueryBuilder queryBuilder)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            int totalItems;

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                string queryWithoutSelect = queryBuilder.getQueryPartWithoutSelect();
                string countQuery = "select count(*) " + queryWithoutSelect;

                MySqlCommand cmd = new MySqlCommand(countQuery, connection);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    totalItems = Convert.ToInt32(result);
                }
                else
                {
                    totalItems = 0;
                }

                return totalItems;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void removeExtraRows(DataSet dataSet, Int64 start, int itemCount)
        {
            for (int i = 0; i < start; i++)
            {
                dataSet.Tables[0].Rows.RemoveAt(0);
            }

            int extraRowsAtEnd = (dataSet.Tables[0].Rows.Count - itemCount);
            if (extraRowsAtEnd > 0)
            {
                for (int i = 0; i < extraRowsAtEnd; i++)
                {
                    dataSet.Tables[0].Rows.RemoveAt(itemCount);
                }
            }
        }

        public static DataTable getData(string connectionString, string query, Int64 start, int itemCount, int startCol, int numCols, string sortColumn, bool ascending, out Int64 totalItems, out int totalCols)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                query.Trim();
                int fromStartIndex = query.ToUpper().IndexOf("FROM ");
                string selectColumns = query.Substring(7, (fromStartIndex - 7));
                string queryWithoutSelect = query.Substring(fromStartIndex);
                string countQuery = "select count(*) " + queryWithoutSelect;

                totalItems = 0;

                char[] seps = { ',', '\n', '\r' };
                String[] columns = selectColumns.Split(seps);
                totalCols = columns.Length;
                if (startCol < totalCols)
                {
                    String selectQueryStr = "select ";
                    for (int i = startCol; i < totalCols && i < (startCol + numCols); i++)
                    {
                        selectQueryStr += columns[i];
                        selectQueryStr += " AS \"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(columns[i].Substring(columns[i].IndexOf('.') + 1).Replace('_', ' ')) + "\",";
                    }
                    if (selectQueryStr.EndsWith(","))
                    {
                        selectQueryStr = selectQueryStr.Substring(0, selectQueryStr.Length - 1);
                    }
                    string finalQuery = selectQueryStr + " " + queryWithoutSelect + " limit " + start + "," + itemCount + "; " + countQuery + ";";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);

                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);

                    DataTable countQueryResults = dataSet.Tables[1];
                    foreach (DataRow row in countQueryResults.Rows)
                    {
                        totalItems = Convert.ToInt64(row[countQueryResults.Columns[0]]);
                    }

                    return dataSet.Tables[0];
                }
                else
                {
                    return null; //throw exception
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable getData(string connectionString, SelectQueryBuilder queryBuilder, Int64 start, int itemCount, int startCol, int numCols, string sortColumn, bool ascending, out Int64 totalItems, out int totalCols)
        {
            DateTime startTime = DateTime.Now;
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                if (queryBuilder.CrossTabClause.Col != null)
                {
                    queryBuilder.setSelectedColumns_CrossTabulation(connectionString);
                }

                DataSet dataSet = new DataSet();

                totalItems = 0;

                if (queryBuilder.GroupByColumns.Count() == 0)
                {
                    string selectPartQuery = queryBuilder.getSelectPartQuery(startCol, numCols, out totalCols);
                    string queryWithoutSelect = queryBuilder.getQueryPartWithoutSelect();

                    string countQuery = "select count(*) " + queryWithoutSelect;

                    string finalQuery = selectPartQuery + " " + queryWithoutSelect + queryBuilder.getLimitRowsPartQuery(start, itemCount)
                                        + "; " + countQuery + ";";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);
                    dataAdapter.Fill(dataSet);
                    DataTable countQueryResults = dataSet.Tables[1];
                    foreach (DataRow row in countQueryResults.Rows)
                    {
                        totalItems = Convert.ToInt64(row[countQueryResults.Columns[0]]);
                    }
                }
                else
                {
                    string finalQuery = queryBuilder.getQueryforGroupBy(out totalCols) + ";";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);
                    dataAdapter.Fill(dataSet);
                    totalItems = dataSet.Tables[0].Rows.Count;
                    removeExtraRows(dataSet, start, itemCount);
                }
                Console.WriteLine("Total time in query execution: " + (DateTime.Now - startTime));
                return dataSet.Tables[0];
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable getData(string connectionString, SelectQueryBuilder queryBuilder, Int64 start, int itemCount, int startCol, int numCols, string sortColumn, bool ascending, out int totalCols)
        {
            DateTime startTime = DateTime.Now;
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            DataSet dataSet = new DataSet();
            connection.Open();
            try
            {
                string queryWithoutSelect = queryBuilder.getQueryPartWithoutSelect();
                string selectPartQuery = queryBuilder.getSelectPartQuery(startCol, numCols, out totalCols);
                if (queryBuilder.GroupByColumns.Count() == 0)
                {
                    string finalQuery = selectPartQuery + " " + queryWithoutSelect + queryBuilder.getLimitRowsPartQuery(start, itemCount);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);
                    dataAdapter.Fill(dataSet);
                }
                else
                {
                    string finalQuery = queryBuilder.getQueryforGroupBy(out totalCols) + ";";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);
                    dataAdapter.Fill(dataSet);
                    removeExtraRows(dataSet, start, itemCount);
                }

                Console.WriteLine("Total time in query execution: " + (DateTime.Now - startTime));
                return dataSet.Tables[0];
            }
            finally
            {
                connection.Close();
            }
        }

        public static string getQuery(SelectQueryBuilder queryBuilder)
        {
            string finalQuery;
            string queryWithoutSelect = queryBuilder.getQueryPartWithoutSelect();
            int totalCols = 0;
            string selectPartQuery = queryBuilder.getSelectPartQuery(0, -1, out totalCols);
            if (queryBuilder.GroupByColumns.Count() == 0)
            {
                finalQuery = selectPartQuery + " " + queryWithoutSelect;
            }
            else
            {
                finalQuery = queryBuilder.getQueryforGroupBy(out totalCols) + ";";
            }

            return finalQuery;
        }

        public static CrossTabResults getCrossTabulationData_old(string connectionString, SelectQueryBuilder queryBuilder, out int totalItems, out int totalCols)
        {
            DateTime startTime = DateTime.Now;
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                string queryWithoutSelect = queryBuilder.getQueryforGroupBy(out totalCols);
                queryBuilder.setSelectedColumns_CrossTabulation(connectionString);

                string selectPartQuery = queryBuilder.getCrossTabSelectPartQuery();
                DataSet dataSet = new DataSet();

                totalItems = 0;
                CrossTabResults crossTabResults = queryBuilder.CrossTabulationResults;
                totalCols = crossTabResults.GroupByColumns.Count + (crossTabResults.SummarizeColumns.Count * (crossTabResults.CrossTabColumnVaues.Count + 1));

                if (queryBuilder.GroupByColumns.Count() == 0)
                {
                    throw new Exception("Calling wrong method!!!");
                }
                else
                {
                    string finalQuery = selectPartQuery + " " + queryWithoutSelect + ";";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);

                    dataAdapter.Fill(dataSet);
                    totalItems = dataSet.Tables[0].Rows.Count;
                }

                crossTabResults.Results = dataSet.Tables[0];
                List<string> columnTypes = new List<string>();
                for (int colIndex = 0; colIndex < dataSet.Tables[0].Columns.Count; colIndex++)
                {
                    columnTypes.Add(dataSet.Tables[0].Columns[colIndex].DataType.FullName);
                }


                DataView dataView = crossTabResults.Results.DefaultView;
                Dictionary<string, Object> dataMap = new Dictionary<string, Object>();
                List<List<string>> groupByColumnValueList = new List<List<string>>();
                List<string> keyPrefixes = new List<string>();
                List<string> groupByColumnValue = null;
                List<string> prevGroupByColumnValue = null;
                string key = "";
                for (int rowIndex = 0; rowIndex < dataView.Count; rowIndex++)
                {
                    groupByColumnValue = new List<string>();
                    int colIndex = 0;
                    for (; colIndex < crossTabResults.GroupByColumns.Count; colIndex++)
                    {
                        string value = dataView[rowIndex].Row[colIndex].ToString();
                        groupByColumnValue.Add(value);
                    }

                    if (prevGroupByColumnValue == null || !groupByColumnValue.SequenceEqual(prevGroupByColumnValue))
                    {
                        prevGroupByColumnValue = groupByColumnValue;
                        groupByColumnValueList.Add(groupByColumnValue);
                        key = "";
                        for (int i = 0; i < groupByColumnValue.Count; i++)
                        {
                            key += groupByColumnValue.ElementAt<string>(i);
                        }
                        key += "~";
                        keyPrefixes.Add(key);
                    }

                    string crossTabColValue = dataView[rowIndex].Row[colIndex].ToString();
                    colIndex++;

                    for (int i = 0; i < crossTabResults.SummarizeColumns.Count; i++)
                    {
                        Object value = dataView[rowIndex].Row[i + colIndex];
                        dataMap.Add(key + crossTabColValue + i, value); // key = append all group by column + cross Tab Column + summarize column Index
                    }
                }

                if (prevGroupByColumnValue == null || !groupByColumnValue.SequenceEqual(prevGroupByColumnValue))
                {
                    groupByColumnValueList.Add(groupByColumnValue);
                }

                // Populate missing values and Totals
                int summarizColStartIndex = crossTabResults.GroupByColumns.Count + 1; // +1 for cross tabulation column (ehich is also part of groupby column in final query
                for (int keyIndex = 0; keyIndex < keyPrefixes.Count; keyIndex++)
                {
                    string keyPrefix = keyPrefixes.ElementAt<string>(keyIndex);
                    decimal[] totals = new decimal[crossTabResults.SummarizeColumns.Count];
                    for (int crosTabColValueIndex = 0; crosTabColValueIndex < crossTabResults.CrossTabColumnVaues.Count; crosTabColValueIndex++)
                    {
                        string crossTabColValue = crossTabResults.CrossTabColumnVaues.ElementAt<string>(crosTabColValueIndex);
                        for (int i = 0; i < crossTabResults.SummarizeColumns.Count; i++)
                        {
                            string keyValue = keyPrefix + crossTabColValue + i;
                            string valueType = columnTypes.ElementAt<string>(summarizColStartIndex + i);
                            SQLBuilder.Clauses.Column summarizeColumn = crossTabResults.SummarizeColumns.ElementAt<SQLBuilder.Clauses.Column>(i);

                            if (dataMap.ContainsKey(keyValue))
                            {
                                //Add column values
                                Object value = dataMap[keyValue];
                                if (!(value == null || "".Equals(value.ToString().Trim())))
                                {
                                    totals[i] = summarize(summarizeColumn.Name.Substring(0, summarizeColumn.Name.IndexOf('(')), totals[i], decimal.Parse(value.ToString()));
                                }
                            }
                            else
                            {
                                // set the missing column value to 0
                                Object zeroValueObject = getZeroValueObject(valueType);
                                dataMap.Add(keyValue, zeroValueObject);

                            }
                        }
                    }

                    for (int i = 0; i < crossTabResults.SummarizeColumns.Count; i++)
                    {
                        dataMap.Add(keyPrefix + "Grand Total" + i, getValueObject(totals[i], columnTypes[summarizColStartIndex + i]));
                    }
                }

                crossTabResults.DataMap = dataMap;
                crossTabResults.GroupByColumnValueList = groupByColumnValueList;
                crossTabResults.KeyPrefixes = keyPrefixes;
                Console.WriteLine("Total time in Cross Tabulation execution: " + (DateTime.Now - startTime));
                return crossTabResults;
            }
            finally
            {
                connection.Close();
            }
        }

        public static CrossTabResults getCrossTabulationData(string connectionString, SelectQueryBuilder queryBuilder, out Int64 totalItems, out int totalCols)
        {
            DateTime startTime = DateTime.Now;
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                queryBuilder.setSelectedColumns_CrossTabulation(connectionString);
                DataSet dataSet = new DataSet();

                totalItems = 0;
                CrossTabResults crossTabResults = queryBuilder.CrossTabulationResults;

                if (queryBuilder.GroupByColumns.Count() == 0)
                {
                    throw new Exception("Calling wrong method!!!");
                }
                else
                {
                    string finalQuery = queryBuilder.getQueryforGroupBy(out totalCols);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);

                    dataAdapter.Fill(dataSet);
                    totalItems = dataSet.Tables[0].Rows.Count;
                }

                crossTabResults.Results = dataSet.Tables[0];
                List<string> columnTypes = new List<string>();
                for (int colIndex = 0; colIndex < dataSet.Tables[0].Columns.Count; colIndex++)
                {
                    columnTypes.Add(dataSet.Tables[0].Columns[colIndex].DataType.FullName);
                }

                DataView dataView = crossTabResults.Results.DefaultView;
                Dictionary<string, Object> dataMap = new Dictionary<string, Object>();
                List<List<string>> groupByColumnValueList = new List<List<string>>();
                List<string> keyPrefixes = new List<string>();
                List<string> groupByColumnValue = null;
                List<string> prevGroupByColumnValue = null;
                List<string> crossTabColumnValues = new List<string>();
                string key = "";
                for (int rowIndex = 0; rowIndex < dataView.Count; rowIndex++)
                {
                    groupByColumnValue = new List<string>();
                    int colIndex = 0;
                    for (; colIndex < crossTabResults.GroupByColumns.Count; colIndex++)
                    {
                        string value = dataView[rowIndex].Row[colIndex].ToString();
                        groupByColumnValue.Add(value);
                    }

                    if (prevGroupByColumnValue == null || !groupByColumnValue.SequenceEqual(prevGroupByColumnValue))
                    {
                        prevGroupByColumnValue = groupByColumnValue;
                        groupByColumnValueList.Add(groupByColumnValue);
                        key = "";
                        for (int i = 0; i < groupByColumnValue.Count; i++)
                        {
                            key += groupByColumnValue.ElementAt<string>(i);
                        }
                        key += "~";
                        keyPrefixes.Add(key);
                    }

                    string crossTabColValue = dataView[rowIndex].Row[colIndex].ToString();
                    if (!crossTabColumnValues.Contains(crossTabColValue))
                    {
                        crossTabColumnValues.Add(crossTabColValue);
                    }
                    colIndex++; //skip the cross tabulation column

                    for (int i = 0; i < crossTabResults.SummarizeColumns.Count; i++)
                    {
                        Object value = dataView[rowIndex].Row[i + colIndex];
                        dataMap.Add(key + crossTabColValue + i, value); // key = append all group by column + cross Tab Column + summarize column Index
                    }
                }

                if (prevGroupByColumnValue == null || !groupByColumnValue.SequenceEqual(prevGroupByColumnValue))
                {
                    groupByColumnValueList.Add(groupByColumnValue);
                }

                SQLBuilder.Clauses.CrossTabulationClause crossTabClause = queryBuilder.CrossTabClause;
                if (crossTabClause.SortSet)
                {
                    crossTabColumnValues.Sort();
                    if (crossTabClause.SortOrder == Sorting.Descending)
                    {
                        crossTabColumnValues.Reverse();
                    }

                }
                crossTabResults.CrossTabColumnVaues = crossTabColumnValues;
                totalCols = crossTabResults.GroupByColumns.Count + (crossTabResults.SummarizeColumns.Count * (crossTabResults.CrossTabColumnVaues.Count + 1));
                // Populate missing values and Totals
                int summarizColStartIndex = crossTabResults.GroupByColumns.Count + 1; // +1 for cross tabulation column (ehich is also part of groupby column in final query
                for (int keyIndex = 0; keyIndex < keyPrefixes.Count; keyIndex++)
                {
                    string keyPrefix = keyPrefixes.ElementAt<string>(keyIndex);
                    decimal[] totals = new decimal[crossTabResults.SummarizeColumns.Count];
                    for (int crosTabColValueIndex = 0; crosTabColValueIndex < crossTabResults.CrossTabColumnVaues.Count; crosTabColValueIndex++)
                    {
                        string crossTabColValue = crossTabResults.CrossTabColumnVaues.ElementAt<string>(crosTabColValueIndex);
                        for (int i = 0; i < crossTabResults.SummarizeColumns.Count; i++)
                        {
                            string keyValue = keyPrefix + crossTabColValue + i;
                            string valueType = columnTypes.ElementAt<string>(summarizColStartIndex + i);
                            SQLBuilder.Clauses.Column summarizeColumn = crossTabResults.SummarizeColumns.ElementAt<SQLBuilder.Clauses.Column>(i);

                            if (dataMap.ContainsKey(keyValue))
                            {
                                //Add column values
                                Object value = dataMap[keyValue];
                                if (!(value == null || "".Equals(value.ToString().Trim())))
                                {
                                    totals[i] = summarize(summarizeColumn.Name.Substring(0, summarizeColumn.Name.IndexOf('(')), totals[i], decimal.Parse(value.ToString()));
                                }

                            }
                            else
                            {
                                Object zeroValueObject = getZeroValueObject(valueType);
                                dataMap.Add(keyValue, zeroValueObject);

                            }
                        }
                    }

                    for (int i = 0; i < crossTabResults.SummarizeColumns.Count; i++)
                    {
                        dataMap.Add(keyPrefix + "Grand Total" + i, getValueObject(totals[i], columnTypes[summarizColStartIndex + i]));
                    }
                }

                crossTabResults.DataMap = dataMap;
                crossTabResults.GroupByColumnValueList = groupByColumnValueList;
                crossTabResults.KeyPrefixes = keyPrefixes;
                Console.WriteLine("Total time in Cross Tabulation execution: " + (DateTime.Now - startTime));
                return crossTabResults;
            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal summarize(string summarizeFunction, decimal total, decimal value)
        {
            decimal returnValue;
            switch (summarizeFunction.ToUpper())
            {
                case "SUM":
                case "COUNT":
                    returnValue = total + value;
                    break;
                case "AVG":
                    returnValue = (total + value) / 2;
                    break;
                default:
                    returnValue = total;
                    break;

            }

            return returnValue;
        }

        public static Object getZeroValueObject(string valueType)
        {
            Object returnValue = null;
            switch (valueType)
            {
                case "System.Decimal":
                    returnValue = Decimal.Zero;
                    break;

                case "System.Int64":
                    returnValue = new System.Int64();
                    returnValue = 0;
                    break;

                case "System.Int32":
                    returnValue = new System.Int32();
                    returnValue = 0;
                    break;

                case "System.Int16":
                    returnValue = new System.Int16();
                    returnValue = 0;
                    break;

                default:
                    returnValue = "0";
                    break;
            }

            return returnValue;
        }

        public static Object getValueObject(Decimal value, string valueType)
        {
            Object returnValue = null;
            switch (valueType)
            {
                case "System.Decimal":
                    returnValue = value;
                    break;

                case "System.Int64":
                    returnValue = Convert.ToInt64(value);
                    break;

                case "System.Int32":
                    returnValue = Convert.ToInt32(value);
                    break;

                case "System.Int16":
                    returnValue = Convert.ToInt16(value);
                    break;

                default:
                    returnValue = value.ToString();
                    break;
            }

            return returnValue;
        }

        public static List<Schema> getSchemaTree(string connectionString, string schemaStr, string derivedTableFilePath)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                string schemaQuery = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '" + schemaStr + "'";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(schemaQuery, connection);

                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable schemaList = dataSet.Tables[0];
                List<Schema> schemas = new List<Schema>();
                foreach (DataRow row in schemaList.Rows)
                {
                    string schemaName = (string)row[schemaList.Columns[0]];
                    Schema schema = new Schema(schemaName);
                    schema.tables = getTableStructure(connection, schemaName);
                    schemas.Add(schema);
                }

                //process derived tables
                Schema derivedSchema = addDerivedTables(derivedTableFilePath);
                if (derivedSchema != null)
                {
                    schemas.Add(derivedSchema);
                }
                return schemas;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Schema addDerivedTables(string derivedTableFilePath)
        {
            Schema schema = null;
            if (derivedTableFilePath != null)
            {
                schema = new Schema("DerivedTables");
                List<Table> tables = new List<Table>();
                foreach (string file in Directory.EnumerateFiles(derivedTableFilePath, "*.xml"))
                {
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                    SelectQueryBuilder loadedQuery = (SelectQueryBuilder)SerializerObj.Deserialize(new StreamReader(file));

                    string fileName = file.Substring(file.LastIndexOf('/') + 1).Replace(' ', '_');
                    DerivedTable table = new DerivedTable(fileName.Substring(0, fileName.Length - 4), "", getQuery(loadedQuery));
                    List<Column> columns = new List<Column>();
                    foreach (SQLBuilder.Clauses.Column col in loadedQuery.SelectedColumns)
                    {
                        string columnName = col.AliasName;
                        string columnType = col.DataType;
                        Object formatType = col.Format;
                        Column column;
                        if (formatType != null)
                        {
                            column = new Column(columnName, columnType, (string)formatType);
                        }
                        else
                        {
                            column = new Column(columnName, columnType);
                        }
                        columns.Add(column);
                    }
                    table.columns = columns;
                    tables.Add(table);
                }
                schema.tables = tables;
            }

            return schema;
        }

        public static List<Table> getTableStructure(string connectionString, string schemaName)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                List<Table> tables = getTableStructure(connection, schemaName);
                return tables;
            }
            finally
            {
                connection.Close();
            }
        }

        public static List<Table> getTableStructure(MySqlConnection connection, string schemaName)
        {
            List<Table> tables = new List<Table>();
            string tableQuery = "SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_schema = '" + schemaName + "'";

            MySqlDataAdapter tableDataAdapter = new MySqlDataAdapter(tableQuery, connection);

            DataSet tableDataSet = new DataSet();
            tableDataAdapter.Fill(tableDataSet);


            DataTable tableList = tableDataSet.Tables[0];
            foreach (DataRow tableRow in tableList.Rows)
            {
                string tableName = (String)tableRow[tableList.Columns[0]];
                Table table = new Table(tableName, schemaName);

                table.columns = getTableColumns(connection, schemaName, tableName);
                tables.Add(table);
            }

            return tables;
        }

        public static List<Column> getTableColumns(string connectionString, string schemaName, string tableName)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                List<Column> columns = getTableColumns(connection, schemaName, tableName);
                return columns;
            }
            finally
            {
                connection.Close();
            }
        }

        public static List<Column> getTableColumns(MySqlConnection connection, string schemaName, string tableName)
        {
            List<Column> columns = new List<Column>();
            string columnQuery = "SELECT C.COLUMN_NAME, C.DATA_TYPE, F.FORMAT, C.IS_NULLABLE, C.COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS C LEFT JOIN COLUMN_FORMATS F ON UPPER(concat(C.TABLE_NAME,'.',C.COLUMN_NAME)) = UPPER(F.COLUMN_NAME) WHERE table_name = '"
                    + tableName + "' AND table_schema = '" + schemaName + "'";

            MySqlDataAdapter columnDataAdapter = new MySqlDataAdapter(columnQuery, connection);
            DataSet columnDataSet = new DataSet();
            columnDataAdapter.Fill(columnDataSet);

            DataTable columnList = columnDataSet.Tables[0];
            foreach (DataRow columnRow in columnList.Rows)
            {
                string columnName = (String)columnRow[columnList.Columns[0]];
                string columnType = (String)columnRow[columnList.Columns[1]];
                Object formatType = columnRow[columnList.Columns[2]];
                Column column;
                if (formatType != System.DBNull.Value)
                {
                    column = new Column(columnName, columnType, (string)formatType);
                }
                else
                {
                    column = new Column(columnName, columnType);
                }
                columns.Add(column);
            }
            return columns;
        }

        public static DataTable getColumnStats(string connectionString, SelectQueryBuilder queryBuilder, SQLBuilder.Clauses.Column column)
        {
            DateTime startTime = DateTime.Now;
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);

            int totalCols = 0;
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                if (queryBuilder.CrossTabClause.Col != null)
                {
                    queryBuilder.setSelectedColumns_CrossTabulation(connectionString);
                }

                DataSet dataSet = new DataSet();
                DataSet dataSetColumn = new DataSet();

                if (queryBuilder.GroupByColumns.Count() == 0)
                {
                    string queryWithoutSelect = queryBuilder.getQueryPartWithoutSelect();
                    string countQuery = "select count(distinct " + SelectQueryBuilder.getColumnPartQuery(column) + ") " + queryWithoutSelect + ";";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(countQuery, connection);
                    dataAdapter.Fill(dataSet);
                    DataTable countQueryResults = dataSet.Tables[0];
                    foreach (DataRow row in countQueryResults.Rows)
                    {
                        Int64 totalRowCount = Convert.ToInt64(row[countQueryResults.Columns[0]]);
                        if (totalRowCount > 100)
                        {
                            throw new Exception("Too many vlaues...");
                        }
                    }
                    string selectPartQuery = queryBuilder.getSelectPartQuery(0, -1, out totalCols);
                    //string finalQuery = "select distinct " + SelectQueryBuilder.getColumnPartQuery(column) + " " + queryWithoutSelect + ";";
                    string finalQuery = "select " + SelectQueryBuilder.getColumnPartQuery(column) + ", count(*) as Count " + queryWithoutSelect
                                            + " group by " + SelectQueryBuilder.getColumnPartQuery(column)
                                            + " order by " + SelectQueryBuilder.getColumnPartQuery(column) + ";";

                    dataAdapter = new MySqlDataAdapter(finalQuery, connection);
                    dataAdapter.Fill(dataSetColumn);
                }
                else
                {
                    string finalQuery = "select distinct " + SelectQueryBuilder.getColumnPartQuery(column) + " from (" + queryBuilder.getQueryforGroupBy(out totalCols) + ");";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(finalQuery, connection);
                    dataAdapter.Fill(dataSet);
                }
                Console.WriteLine("Total time in query execution: " + (DateTime.Now - startTime));
                return dataSetColumn.Tables[0];
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
