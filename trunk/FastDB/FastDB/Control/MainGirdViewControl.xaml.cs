using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Windows.Controls;
using FastDB.Control.CrossTabulationViewControls;
using System.Configuration;
using System.Xml.Serialization;
using SQLBuilder;
using System.IO;
using FastDB.Class;
using Microsoft.CSharp;
using System.Runtime.InteropServices;
using FastDB.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Office.Core;

namespace FastDB.Control
{
    public partial class MainGirdViewControl : UserControl
    {
        double VBarThumbTopMargin;
        double HBarThumbTopMargin;

        private int totalRows;
        private int totalColumns;

        private int VBarMoveWidth;
        private int HBarMoveWidth;

        private Int64 rowToatlPage;
        public Int64 RowCurrenPage;
        private Int64 startRow;
        private Int64 EndCurrentPageRow;

        private Int64 columnToatlPage;
        private Int64 ColCurrenPage;
        private Int64 startColumn;
        private Int64 EndCurrentPageColumn;
        private Int32 ColNum = 0;

        private string _tableName;
        public SQLBuilder.SelectQueryBuilder dbColumnQuerryBuilder;

        private string currentSortColumn;
        private string currentSortDirection;
        private string currentSortColumnIndex;
        public ResultViewModel result;
        private string connectionString;

        public MainGirdViewControl(tableViewModel tvm, bool getResultByTreeView)
        {
            InitializeComponent();

            _tableName = tvm._table.name;
            result = new ResultViewModel(tvm, getResultByTreeView);

            //this.DataContext = result;
            //int rowCount = this.MainDataGrid.Items.Count;

            //ResultViewModel result = (ResultViewModel)this.DataContext;
            if (result != null)
            {

                this.DataContext = result;

                // manualy adding grid column this is just a test
                //Microsoft.Windows.Controls.DataGridTextColumn dataGridTextColumn   = new Microsoft.Windows.Controls.DataGridTextColumn();
                //dataGridTextColumn.Header = "Gajendra";
                //this.MainDataGrid.Columns.Add(dataGridTextColumn);

                ShowControl();

                //CurrenPage = CurrenPage + 1;
                //this.lblPageNumber.Content = "Page " + CurrenPage.ToString() + " Of " + rowToatlPage.ToString();


                rowToatlPage = getnumberOfPages(result.TotalIRows, result.rowPageSize);
                columnToatlPage = getnumberOfPages(result.TotalColumns, result.columnPageSize);

                if (rowToatlPage > 0)
                {
                    RowCurrenPage = 1;
                    getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
                    this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
                    this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
                }
                if (columnToatlPage > 0 && result.TotalIRows != 0)
                {
                    ColCurrenPage = 1;
                    getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
                    this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();
                }

            }
        }

        public MainGirdViewControl(SQLBuilder.SelectQueryBuilder QuerryBuilder, string CurrentDatabaseName)
        {
            this.dbColumnQuerryBuilder = QuerryBuilder;
            InitializeComponent();
            result = new ResultViewModel(QuerryBuilder, CurrentDatabaseName);
            if (result != null)
            {

                this.DataContext = result;
                if (result.QueryBulder != null)
                {
                    if (result.QueryBulder.SelectedTables.Count != 0)
                    {
                        _tableName = result.QueryBulder.SelectedTables[0].Name;
                    }
                }
                ShowControl();

                //CurrenPage = CurrenPage + 1;
                //this.lblPageNumber.Content = "Page " + CurrenPage.ToString() + " Of " + rowToatlPage.ToString();


                rowToatlPage = getnumberOfPages(result.TotalIRows, result.rowPageSize);
                columnToatlPage = getnumberOfPages(result.TotalColumns, result.columnPageSize);

                if (rowToatlPage > 0)
                {
                    RowCurrenPage = 1;
                    getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
                    this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
                    this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
                }
                if (columnToatlPage > 0 && result.TotalIRows != 0)
                {
                    ColCurrenPage = 1;
                    getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
                    this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();
                }

            }
        }

        public MainGirdViewControl(SQLBuilder.SelectQueryBuilder QuerryBuilder, string CurrentDatabaseName, string sqldata)
        {
            this.dbColumnQuerryBuilder = QuerryBuilder;
            InitializeComponent();
            result = new ResultViewModel(QuerryBuilder, CurrentDatabaseName);
            if (result != null)
            {

                this.DataContext = result;
                if (result.QueryBulder != null)
                {
                    if (result.QueryBulder.SelectedTables.Count != 0)
                    {
                        _tableName = result.QueryBulder.SelectedTables[0].Name;
                    }
                }
                //ShowControl();

                //CurrenPage = CurrenPage + 1;
                //this.lblPageNumber.Content = "Page " + CurrenPage.ToString() + " Of " + rowToatlPage.ToString();


                //rowToatlPage = getnumberOfPages(result.TotalIRows, result.rowPageSize);
                //columnToatlPage = getnumberOfPages(result.TotalColumns, result.columnPageSize);

                //if (rowToatlPage > 0)
                //{
                //    RowCurrenPage = 1;
                //    getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
                //    this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
                //    this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
                //}
                //if (columnToatlPage > 0 && result.TotalIRows != 0)
                //{
                //    ColCurrenPage = 1;
                //    getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
                //    this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();
                //}

            }
        }

        private void MainDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Windows.Controls.DataGrid dataGrid = (Microsoft.Windows.Controls.DataGrid)sender;

            //this.Style = dataGrid.Resources["DataGridColumnHeaderStyle"] as Style;

            //string colProperty = "Help";
            //Microsoft.Windows.Controls.DataGridTextColumn col = new Microsoft.Windows.Controls.DataGridTextColumn();
            //col.Binding = new Binding(colProperty);
            //var spHeader = new StackPanel() { Orientation = Orientation.Horizontal };
            //spHeader.Children.Add(new TextBlock(new System.Windows.Documents.Run(colProperty)));
            //var button = new Button();
            //button.Click += Button_Filter_Click;
            //button.Content = "filter";
            //spHeader.Children.Add(button);
            //col.Header = spHeader;

            //dataGrid.Columns.Add(col);
        }
        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        { 
        
        }
        private void MainDataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            Microsoft.Windows.Controls.DataGrid dataGrid = (Microsoft.Windows.Controls.DataGrid)sender;
            if (currentSortColumnIndex != null)
            {
                if (currentSortDirection == "Ascending")
                {
                    dataGrid.Columns[Int32.Parse(currentSortColumnIndex)].SortDirection = ListSortDirection.Ascending;
                }
                else
                {
                    dataGrid.Columns[Int32.Parse(currentSortColumnIndex)].SortDirection = ListSortDirection.Descending;
                }
            }

        }

        private void MainDataGrid_Sorting(object sender, Microsoft.Windows.Controls.DataGridSortingEventArgs e)
        {

            //var columnPopup = new GridHeaderColumn();
            //int columnHeaderIndex = e.Column.DisplayIndex;
            //string dbColumn = String.Empty;
            //MainWindow mw = (MainWindow)GetTopLevelControl(this.MainDataGrid);
            //DependencyObject parent = GetRVC(this.MainDataGrid);
            //if (parent != null)
            //{
            //    ResultViewControl rvc = (ResultViewControl)parent;
            //    int selectedColumnCount = rvc.SelectTabCntrl._SelectedColCollection.Count;
            //    int totalCount = rvc.SelectTabCntrl.lstToSelecteColFrom.Items.Count;

            //    if (selectedColumnCount == 0 || selectedColumnCount == null)
            //    {
            //        for (int i = 0; i < totalCount; i++)
            //        {
            //            if (i == columnHeaderIndex)
            //            {
            //                dbColumn = rvc.SelectTabCntrl.lstToSelecteColFrom.Items[i].ToString();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < selectedColumnCount; i++)
            //        {
            //            if (i == columnHeaderIndex)
            //            {
            //                dbColumn = rvc.SelectTabCntrl._SelectedColCollection[i].ToString();
            //            }
            //        }
            //    }

            //}

            //string columnHeader = e.Column.SortMemberPath;
            //MainWindow mainWindow = (MainWindow)GetTopLevelControl(this);
            //if (mainWindow.queryBuilder == null)
            //{
            //    mainWindow.queryBuilder = MainWindow.LatestQueryBuilder;
            //}
            //connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
            //DataTable dataTable = new DataTable();
            //DataSet columnds = new DataSet();
            //SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
            //column.Name = dbColumn;
            //column.Format = "";
            //column.AliasName = "";
            //dataTable = MySQLData.DataAccess.ADODataBridge.getColumnStats(connectionString, mainWindow.queryBuilder, column);
            //columnPopup.ColumnDataGrid.DataContext = dataTable;
            //columnPopup.Show();

            //e.Handled = true;
            //bool sortAscending = true;
            //if (e.Column.SortMemberPath != currentSortColumn)
            //{
            //    currentSortColumn = e.Column.SortMemberPath;
            //    currentSortDirection = "Ascending";
            //    sortAscending = true;
            //}
            //else
            //{
            //    switch (currentSortDirection)
            //    {
            //        case "Ascending":
            //            currentSortDirection = "Descending";
            //            sortAscending = false;
            //            break;
            //        case "Descending":
            //            currentSortDirection = "Ascending";
            //            sortAscending = true;
            //            break;
            //        case null:
            //            currentSortDirection = "Ascending";
            //            sortAscending = true;
            //            break;
            //    }

            //}
            //currentSortColumnIndex = e.Column.DisplayIndex.ToString();
            //Mouse.OverrideCursor = Cursors.Wait;
            //result.Sort(e.Column.SortMemberPath, sortAscending);
            //Mouse.OverrideCursor = null;
            //if (RowCurrenPage != 0 && RowCurrenPage > 0)
            //{
            //    RowCurrenPage = 1;
            //    getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
            //    this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
            //    this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
            //}

        }

        void MainDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = "single click to sort column and double click to get column statistics";
            toolTip.IsOpen = true;
            MainDataGrid.ToolTip = toolTip;
            ToolTipService.SetShowDuration(MainDataGrid, 5000);
            toolTip.IsOpen = false;
        }
        DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                parent = tmp;
            }
            return parent;
        }

        private void VBarBtnNext_Click(object sender, RoutedEventArgs e)
        {
            RowCurrenPage = RowCurrenPage + 1;
            getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
            this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
            this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
        }

        private void VBarPrevious_Click(object sender, RoutedEventArgs e)
        {
            RowCurrenPage = RowCurrenPage - 1;
            getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
            this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
            this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
        }

        private void VBarBtnLast_Click(object sender, RoutedEventArgs e)
        {
            RowCurrenPage = rowToatlPage;
            getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
            this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
            this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();

        }

        private void VBarBtnFirst_Click(object sender, RoutedEventArgs e)
        {
            RowCurrenPage = 1;
            getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
            this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
            this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
        }

        private void HBarBtnNext_Click(object sender, RoutedEventArgs e)
        {

            ColCurrenPage = ColCurrenPage + 1;
            getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
            this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();
        }

        private void HBarBtnPrevious_Click(object sender, RoutedEventArgs e)
        {

            ColCurrenPage = ColCurrenPage - 1;
            getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
            this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();
        }

        private void HBarBtnLast_Click(object sender, RoutedEventArgs e)
        {

            ColCurrenPage = columnToatlPage;
            getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
            this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();

        }

        private void HBarBtnFirst_Click(object sender, RoutedEventArgs e)
        {
            ColCurrenPage = 1;
            getPageInfo(ColCurrenPage, result.columnPageSize, result.TotalColumns, out startColumn, out EndCurrentPageColumn);
            this.lblColumnNumber.Content = "Col " + startColumn.ToString() + " - " + EndCurrentPageColumn.ToString() + " Of " + result.TotalColumns.ToString();
        }

        private void btnGotopage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RowCurrenPage = Int32.Parse(this.txtGotoPage.Text);

                if (RowCurrenPage < rowToatlPage)
                {
                    if (RowCurrenPage != 0 && RowCurrenPage > 0)
                    {
                        getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
                        Mouse.OverrideCursor = Cursors.Wait;
                        result.GotoPage(startRow);
                        Mouse.OverrideCursor = null;
                        this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
                        this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
                        //clear the textbox value
                    }
                }
                else
                {
                    RowCurrenPage = rowToatlPage;
                    getPageInfo(RowCurrenPage, result.rowPageSize, result.TotalIRows, out startRow, out EndCurrentPageRow);
                    Mouse.OverrideCursor = Cursors.Wait;
                    result.GotoPage(startRow);
                    Mouse.OverrideCursor = null;
                    this.lblRowNumber.Content = "Row " + startRow.ToString() + " - " + EndCurrentPageRow.ToString() + " Of " + result.TotalIRows.ToString();
                    this.lblPageNumber.Content = "Page " + RowCurrenPage.ToString() + " Of " + rowToatlPage.ToString();
                }
            }
            catch (System.FormatException ex)
            {
                string s = ex.ToString();
            }
            this.txtGotoPage.Text = "";
        }

        private DependencyObject GetRVC(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                if (tmp.DependencyObjectType.Name == "ResultViewControl")
                {
                    parent = tmp;
                    break;
                }
            }
            return parent;
        }

        private void ShowControl()
        {
            if (result.TotalIRows == 0)
            {
                this.lblTableName.Visibility = Visibility.Hidden;
                this.MainDataGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                this.lblTableName.Content = _tableName;
                if (result._ShowVertcalScrollBar)
                {
                    this.VBarMainStakPanel.Visibility = Visibility.Visible;
                    this.DocPnlGotoPage.Visibility = Visibility.Visible;
                }
                if (result._ShowHorizontalScrollBar)
                {
                    this.HBarMainStakPanel.Visibility = Visibility.Visible;
                }
            }
        }

        public static void getPageInfo(Int64 pageNumber, int pageSize, Int64 total, out Int64 start, out Int64 end)
        {
            start = (pageNumber - 1) * pageSize + 1;
            end = (start + pageSize) - 1;
            if (start > total)
            {
                start = total;
            }
            if (end > total)
            {
                end = total;
            }
        }

        public static Int64 getnumberOfPages(Int64 total, int pageSize)
        {
            Int64 numPages = 0;
            if (total % pageSize > 0)
                numPages = (total / pageSize) + 1;
            else
                numPages = (total / pageSize);
            return numPages;
        }

        private void MainDataGrid_AutoGeneratingColumn(object sender, Microsoft.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            int endColIndx = ((FastDB.ResultViewModel)(this.DataContext)).EndColumn;
            Microsoft.Windows.Controls.DataGridTextColumn dataGridTextColumn = e.Column as Microsoft.Windows.Controls.DataGridTextColumn;
            TextBlock tb = new TextBlock();

            if (ColNum >= 0 && ColNum < endColIndx - 1)
            {
                e.Column.Width = new Microsoft.Windows.Controls.DataGridLength(160, Microsoft.Windows.Controls.DataGridLengthUnitType.Pixel);
                ColNum = ColNum + 1;
            }
            else
            {
                e.Column.Width = new Microsoft.Windows.Controls.DataGridLength(1, Microsoft.Windows.Controls.DataGridLengthUnitType.Star);
                ColNum = 0;
            }

            if (e.PropertyType == typeof(Int16) || e.PropertyType == typeof(Decimal) || e.PropertyType == typeof(Double) || e.PropertyType == typeof(Int32) || e.PropertyType == typeof(Int64))
            {
                if (dataGridTextColumn != null)
                {

                    dataGridTextColumn.ElementStyle = (Style)FindResource("RightAlignStyle");
                }
            }
            else
            {
                dataGridTextColumn.ElementStyle = (Style)FindResource("DataGridCellStyle");

            }
            foreach (SQLBuilder.Clauses.Column col in result.QueryBulder.FinalSelectedColumns)
            {
                if (col.AliasName == dataGridTextColumn.Header.ToString() && col.Format != null)
                {
                    dataGridTextColumn.Binding.StringFormat = SQLBuilder.Common.ColumnFormat.Instance.getColumnFormat(col.Format);
                    break;
                }
            }
        }

        private void MainDataGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            Microsoft.Windows.Controls.DataGrid dataGrid = (Microsoft.Windows.Controls.DataGrid)sender;
            
            //string colProperty = "Help";
            //Microsoft.Windows.Controls.DataGridTextColumn col = new Microsoft.Windows.Controls.DataGridTextColumn();
            //col.Binding = new Binding(colProperty);
            //var spHeader = new StackPanel() { Orientation = Orientation.Horizontal };
            //spHeader.Children.Add(new TextBlock(new System.Windows.Documents.Run(colProperty)));
            //var button = new Button();
            //button.Click += Button_Filter_Click;
            //button.Content = "Help";
            //spHeader.Children.Add(button);
            //col.Header = spHeader;

            //dataGrid.Columns.Add(col);
        }
    }
}
