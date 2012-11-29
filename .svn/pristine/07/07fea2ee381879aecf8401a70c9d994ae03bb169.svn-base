using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using FastDB.Control.CrossTabulationViewControls;
using FastDB.Excel;
using SQLBuilder;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Documents;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace FastDB.Class
{
    public class Common
    {
        public static string getColumnNameOrAlias(SQLBuilder.Clauses.Column Column)
        {
            string colNameprAlias = "";
            if (Column.AliasName == System.String.Empty)
            {
                colNameprAlias = Column.Name;
            }
            else
            {
                colNameprAlias = Column.AliasName;
            }
            return colNameprAlias;
        }
        
        public static int getIndex(List<string> list, string name)
        {
            int index = list.FindIndex(r => r == name);
            return index;
        }
        
        public static List<string> ConvertColumsToStringList(List<MySQLData.Column> listOfColumns)
        {
            List<string> list = new List<string>();
            foreach (MySQLData.Column column in listOfColumns)
            {
                list.Add(column.name);
            }
            return list;
        }
        
        public static List<string> ConvertTablesToStringList(List<MySQLData.Table> listOfColumns)
        {
            List<string> list = new List<string>();
            foreach (MySQLData.Table table in listOfColumns)
            {
                list.Add(table.name);
            }
            return list;
        }
        
        public static List<string> GetWhereClauseDropDownColumns(List<SelectTabColumn> listOfColumns)
        {
            List<string> list = new List<string>();
            foreach (SelectTabColumn col in listOfColumns)
            {
                list.Add(col.name);
            }
            return list;
        }
        
        public static string GetStringValueForEnum(string eNum, object enumvalue)
        {

            string value = "";

            switch (eNum)
            {
                case "LogicOperator":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.LogicOperator), (SQLBuilder.Enums.LogicOperator)enumvalue);
                    break;
                case "Comparison":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.Comparison), (SQLBuilder.Enums.Comparison)enumvalue);
                    break;
                case "JoinType":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.JoinType), (SQLBuilder.Enums.JoinType)enumvalue);
                    break;
                case "Condition":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.Condition), (SQLBuilder.Enums.Condition)enumvalue);
                    break;
                case "Sorting":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.Sorting), (SQLBuilder.Enums.Sorting)enumvalue);
                    break;
                case "TopUnit":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.TopUnit), (SQLBuilder.Enums.TopUnit)enumvalue);
                    break;
                case "GroupFunction":
                    value = Enum.GetName(typeof(SQLBuilder.Enums.GroupFunction), (SQLBuilder.Enums.GroupFunction)enumvalue);
                    break;
            }
            return value;
        }

        public static void ExportToCSV(System.Data.DataView dv, string strFilePath)
        {
            var sw = new StreamWriter(strFilePath, false);

            // Write the headers.
            int iColCount = dv.Table.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dv.Table.Columns[i]);
                if (i < iColCount - 1) sw.Write(",");
            }

            sw.Write(sw.NewLine);

            // Write rows.
            foreach (System.Data.DataRow dr in dv.Table.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        if (dr[i].ToString().StartsWith("0"))
                        {
                            sw.Write(@"=""" + dr[i] + @"""");
                        }
                        else
                        {
                            // if value contains, replace with ""
                            sw.Write(dr[i].ToString().Replace(",", ""));
                        }
                    }

                    if (i < iColCount - 1) sw.Write(",");
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        public static List<string> GetColumsFormatList()
        {
            List<string> list = new List<string>();
            list.Add("Number");
            list.Add("Percentage");
            list.Add("Phone");
            list.Add("Currency");
            return list;

        }

        public static List<string> GetDateFormatList()
        {
            List<string> list = new List<string>();
            list.Add("'%d/%m/%Y'");
            list.Add("'%m-%d-%Y'");
            list.Add("'%d %b %y'");
            list.Add("'%m/%d/%Y'");
            list.Add("'%e %M %Y'");
            list.Add("'%d %b %Y %T:%f'");
            return list;
        }

        public static void CreateExcelDocument(DataTable dt, SelectQueryBuilder queryBuilder, string xlsxFilePath)
        {
            using (SpreadsheetDocument myWorkbook = SpreadsheetDocument.Create(xlsxFilePath, SpreadsheetDocumentType.Workbook))
            {
                //Access the main Workbook part, which contains all references                
                WorkbookPart workbookPart = myWorkbook.WorkbookPart;

                InsertValuesInSheets(dt.TableName, queryBuilder, workbookPart, dt);
                workbookPart.Workbook.Save();
            }
        }

        public static void InsertValuesInSheets(string sheetName, SelectQueryBuilder queryBuilder, WorkbookPart workbookPart, DataTable table)
        {
            WorksheetPart worksheetPart = null;

            if (!string.IsNullOrEmpty(sheetName))
            {
                Sheet ss = workbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).SingleOrDefault<Sheet>();
                worksheetPart = (WorksheetPart)workbookPart.GetPartById(ss.Id);
            }
            else
            {
                worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
            }

            if (worksheetPart != null)
            {
                SheetData data = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                //add column names to the first row  
                Row header = new Row();
                header.RowIndex = (UInt32)1;

                foreach (DataColumn column in table.Columns)
                {
                    Cell headerCell = createTextCell(
                        table.Columns.IndexOf(column) + 1,
                        1,
                        column.ColumnName);

                    header.AppendChild(headerCell);
                }
                data.AppendChild(header);

                //loop through each data row  
                DataRow contentRow;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    contentRow = table.Rows[i];
                    data.AppendChild(createContentRow(contentRow, i + 2));
                }

                worksheetPart.Worksheet.Save();
            }
        }

        public static Cell createTextCell(
            int columnIndex,
            int rowIndex,
            object cellValue)
        {
            Cell cell = new Cell();

            cell.DataType = CellValues.InlineString;
            cell.CellReference = getColumnName(columnIndex) + rowIndex;

            InlineString inlineString = new InlineString();
            DocumentFormat.OpenXml.Spreadsheet.Text t = new DocumentFormat.OpenXml.Spreadsheet.Text();

            t.Text = cellValue.ToString();
            inlineString.AppendChild(t);
            cell.AppendChild(inlineString);

            return cell;
        }

        public static Row createContentRow(
            DataRow dataRow,
            int rowIndex)
        {
            Row row = new Row
            {
                RowIndex = (UInt32)rowIndex
            };

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                Cell dataCell = createTextCell(i + 1, rowIndex, dataRow[i]);
                row.AppendChild(dataCell);
            }
            return row;
        }

        public static string getColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = String.Empty;
            int modifier;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName =
                    Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (int)((dividend - modifier) / 26);
            }

            return columnName;
        }

        public static void CreateCrossTabulation(string filePath, DataTable dt, List<SQLBuilder.Clauses.Column> SelectedColumns, ResultViewModel rvm)
        {
            FileInfo f = new FileInfo(filePath);
            if (f.Exists)
                f.Delete();

            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            var stylesPart = spreadsheetDocument.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet styles = new CustomStylesheet();
            styles.Save(stylesPart);
            workbookpart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = (spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets()));

            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "mySheet" };
            sheets.Append(sheet);
            string cl = "A";
            uint row = 1;

            int index;
            Cell cell = new Cell();
            System.Windows.Controls.Label colRow = new System.Windows.Controls.Label();
            int lineNumber = 0;

            colRow.Content = "Cross Tabulation by " + rvm.QueryBulder.CrossTabulationResults.CrossTabColumn.AliasName;
            cell = InsertCellInWorksheet(cl, row, worksheetPart);
            cell.CellValue = new CellValue(Convert.ToString(colRow.Content));
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.StyleIndex = 7;

            row++;

            List<List<string>> groupByColumnValues = rvm.QueryBulder.CrossTabulationResults.GroupByColumnValueList;

            for (int groupByColIndex = 0; groupByColIndex < rvm.QueryBulder.CrossTabulationResults.GroupByColumns.Count; groupByColIndex++)
            {
                SQLBuilder.Clauses.Column groupByCol = rvm.QueryBulder.CrossTabulationResults.GroupByColumns.ElementAt<SQLBuilder.Clauses.Column>(groupByColIndex);

                string colFormat = SQLBuilder.Common.ColumnFormat.Instance.getColumnFormat(groupByCol.Format);
                CrossTabulationViewGroupByControl ctvgCntrl = new CrossTabulationViewGroupByControl();
                colRow.Content = Common.getColumnNameOrAlias(groupByCol);

                for (int i = 0; i <= groupByColumnValues.Count + 1; i++)
                {
                    if (groupByColIndex >= 26)
                    {
                        cl = Convert.ToString(Convert.ToChar(65 + ((groupByColIndex / 26) - 1))) + Convert.ToString(Convert.ToChar(65 + groupByColIndex % 26));
                    }
                    else
                    {
                        cl = Convert.ToString(Convert.ToChar(65 + groupByColIndex));
                    }
                    SharedStringTablePart shareStringPart;
                    if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
                    }
                    if (row == 2)
                    {
                        index = InsertSharedStringItem(dt.Columns[groupByColIndex].ColumnName, shareStringPart);
                        cell = InsertCellInWorksheet(cl, row, worksheetPart);
                        cell.CellValue = new CellValue(Convert.ToString(dt.Columns[groupByColIndex]));
                        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                        cell.StyleIndex = 7;
                    }
                    else if (row > 3)
                    {
                        cell = InsertCellInWorksheet(cl, row, worksheetPart);
                        cell.CellValue = new CellValue(Convert.ToString(groupByColumnValues.ElementAt<List<string>>(i - 2).ElementAt<string>(groupByColIndex)));
                    }
                    lineNumber = lineNumber + 1;
                    row++;
                }
                row = 2;
            }

            int groupByColCount = rvm.QueryBulder.CrossTabulationResults.GroupByColumns.Count;
            int summarrizeValueIndex = groupByColCount + 1;
            Dictionary<string, Object> dataMap = rvm.QueryBulder.CrossTabulationResults.DataMap;
            row = 1;
            foreach (string summaryMainValue in rvm.QueryBulder.CrossTabulationResults.CrossTabColumnVaues)
            {

                CrossTabulationViewSummaryMainControl summaryMain = new CrossTabulationViewSummaryMainControl();
                summaryMain.lblSummaryHeader.Content = summaryMainValue;
                int totalcolumn = rvm.QueryBulder.CrossTabulationResults.SummarizeColumns.Count + groupByColCount;
                for (int summaryColIndex = 0; summaryColIndex < rvm.QueryBulder.CrossTabulationResults.SummarizeColumns.Count; summaryColIndex++)
                {
                    SQLBuilder.Clauses.Column summaryCol = rvm.QueryBulder.CrossTabulationResults.SummarizeColumns.ElementAt<SQLBuilder.Clauses.Column>(summaryColIndex);
                    string summaryColName = summaryCol.AliasName;
                    string summarycolFormat = SQLBuilder.Common.ColumnFormat.Instance.getColumnFormat(summaryCol.Format);
                    CrossTabulationViewSummaryControl ctvsCtrl = new CrossTabulationViewSummaryControl();

                    for (int keyIndex = 0; keyIndex <= rvm.QueryBulder.CrossTabulationResults.KeyPrefixes.Count + 2; keyIndex++)
                    {
                        if (summaryColIndex >= 26)
                        {
                            cl = Convert.ToString(Convert.ToChar(65 + ((summarrizeValueIndex / 26) - 1))) + Convert.ToString(Convert.ToChar(65 + summarrizeValueIndex % 26));
                        }
                        else
                        {
                            cl = Convert.ToString(Convert.ToChar(65 + summarrizeValueIndex));
                        }
                        SharedStringTablePart shareStringPart;
                        if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                        {
                            shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        }
                        else
                        {
                            shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
                        }
                        if (row == 1)
                        {
                            index = InsertSharedStringItem(summaryMainValue, shareStringPart);
                            cell = InsertCellInWorksheet(cl, row, worksheetPart);
                            cell.CellValue = new CellValue(Convert.ToString(summaryMainValue));
                            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                            cell.StyleIndex = 7;
                        }
                        else if (row == 2)
                        {
                            index = InsertSharedStringItem(summaryColName, shareStringPart);
                            cell = InsertCellInWorksheet(cl, row, worksheetPart);
                            cell.CellValue = new CellValue(Convert.ToString(summaryColName));
                            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                            cell.StyleIndex = 7;
                        }
                        else if (row > 3)
                        {
                            string key = rvm.QueryBulder.CrossTabulationResults.KeyPrefixes.ElementAt<string>(keyIndex - 3);

                            string keyValue = key + summaryMainValue + summaryColIndex;

                            cell = InsertCellInWorksheet(cl, row, worksheetPart);
                            if (dataMap.ContainsKey(keyValue))
                            {
                                if (summarycolFormat != null)
                                {
                                    cell.CellValue = new CellValue(String.Format(summarycolFormat, dataMap[keyValue]));
                                }
                                else
                                {
                                    cell.CellValue = new CellValue(Convert.ToString(dataMap[keyValue]));
                                }
                            }
                            else
                            {
                                cell.CellValue = new CellValue(Convert.ToString("00"));
                            }

                        }
                        lineNumber = lineNumber + 1;

                        if (row > rvm.QueryBulder.CrossTabulationResults.KeyPrefixes.Count + 2)
                        {
                            row = 1;
                            summarrizeValueIndex = summarrizeValueIndex + 1;
                        }
                        else
                        {
                            row++;
                        }
                    }
                    lineNumber = 0;
                }
                row = 1;
                groupByColCount = groupByColCount + 1;
            }
            worksheetPart.Worksheet.Save();
            workbookpart.Workbook.Save();
            spreadsheetDocument.Close();
        }
        
        public static void CreateSpreadsheetWorkbook(string filepath, DataTable dt, List<SQLBuilder.Clauses.Column> SelectedColumns)
        {
            FileInfo f = new FileInfo(filepath);
            if (f.Exists)
                f.Delete();

            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            var stylesPart =
                spreadsheetDocument.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet styles = new CustomStylesheet();
            styles.Save(stylesPart);
            workbookpart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "mySheet" };
            sheets.Append(sheet);
            string cl = "";
            uint row = 2;
            int index;
            Cell cell;
            foreach (DataRow dr in dt.Rows)
            {
                for (int idx = 0; idx < dt.Columns.Count; idx++)
                {
                    if (idx >= 26)
                    {
                        cl = Convert.ToString(Convert.ToChar(65 + ((idx / 26) - 1))) + Convert.ToString(Convert.ToChar(65 + idx % 26));
                    }
                    else
                    {
                        cl = Convert.ToString(Convert.ToChar(65 + idx));
                    }

                    SharedStringTablePart shareStringPart;
                    if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
                    }
                    if (row == 2)
                    {
                        index = InsertSharedStringItem(dt.Columns[idx].ColumnName, shareStringPart);
                        cell = InsertCellInWorksheet(cl, row - 1, worksheetPart);
                        cell.CellValue = new CellValue(index.ToString());
                        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                        cell.StyleIndex = 7;
                    }

                    cell = InsertCellInWorksheet(cl, row, worksheetPart);
                    cell.CellValue = new CellValue(Convert.ToString(dr[idx]));

                    if (dr[idx] != null)
                    {
                        if (SelectedColumns.Count != 0)
                        {
                            string colFormat = SelectedColumns[idx].Format;
                            if (colFormat != null)
                            {
                                cell.StyleIndex = (UInt32)SQLBuilder.Common.ColumnFormat.Instance.getExcelColumnFormat(colFormat);
                                if (dr[idx].GetType() == typeof(string))
                                {
                                    cell.DataType = CellValues.String;
                                }
                                else if (dr[idx].GetType() == typeof(bool))
                                {
                                    cell.DataType = CellValues.Boolean;
                                }
                                else if (dr[idx].GetType() == typeof(DateTime))
                                {
                                    cell.DataType = CellValues.Date;
                                }
                                else if (dr[idx].GetType() == typeof(decimal) ||
                                         dr[idx].GetType() == typeof(double))
                                {
                                    cell.DataType = CellValues.Number;
                                }
                                else if (dr[idx].GetType() == typeof(Int16) || dr[idx].GetType() == typeof(Int32) || dr[idx].GetType() == typeof(Int64))
                                {
                                    cell.DataType = CellValues.Number;
                                }
                                else
                                {
                                    cell.DataType = CellValues.String;
                                }
                            }
                            else
                            {
                                cell.StyleIndex = 8;
                                cell.DataType = CellValues.String;
                            }
                        }
                        
                    }

                }
                row++;
            }

            worksheetPart.Worksheet.Save();
            workbookpart.Workbook.Save();
            // Close the document.
            spreadsheetDocument.Close();
        }

        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {

            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }
            int i = 0;
            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }
                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();
            return i;
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one. 
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {

                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                Cell newCell = new Cell() { CellReference = cellReference };

                row.InsertBefore(newCell, refCell);
                return newCell;
            }

        }

    }

}
