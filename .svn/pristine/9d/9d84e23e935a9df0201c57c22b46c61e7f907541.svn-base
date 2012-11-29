using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data;
using System.Configuration;
using System;
using SQLBuilder.Clauses;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
namespace FastDB
{
    public class ResultViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        private Microsoft.Windows.Controls.DataGridColumn currentSortColumn;
        private string connectionString;
        private ListSortDirection currentSortDirection;
        private tableViewModel _TableViewModel; 
        private bool _getResultByQuery=false;
        public bool _getResultByTreeView = false;
        private SQLBuilder.SelectQueryBuilder _queryBuilder;
        private string _currentDatabaseName;
        private string _tableName;
        public bool isNew = false;
        public bool isModified = false;
        public string directoryPath;
        public bool _ShowVertcalScrollBar = true;
        public bool _ShowHorizontalScrollBar = true;
        
        //row variable
        #region rowvariable
        private Int64 startRow = 0;

        public int rowPageSize = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["rowPageSize"]); // rowPageSize is how many row we want to display on grid

        private string sortColumn;// = "Id";

        private bool ascending = true;

        private Int64 totalRows;
        #endregion
        //column variable
        #region columnvariable
        private int startColumn = 0;

        public int columnPageSize = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["columnPageSize"]); // columnPageSize is how many column we wan to display on grid

        private int totalColumns;// = "Id";

        #endregion

        //row comand variable
        #region rowcomandvariable

        private ICommand _firstRowCommand;

        private ICommand _previousRowCommand;

        private ICommand _nextRowCommand;

        private ICommand _lastRowCommand;

        #endregion

        // column command variable
        #region columncommandvariable
       
        private ICommand _firstColumnCommand;

        private ICommand _previousColumnCommand;

        private ICommand _nextColumnCommand;

        private ICommand _lastColumnCommand;

        #endregion

        private DataView _Results;
        private DataView _Results1;

        private ICommand simpleCommand;

        /// <summary>
        /// Gets the simple command.
        /// </summary>
        public ICommand SimpleCommand
        {
            get { return simpleCommand; }
        }

        public ResultViewModel(tableViewModel TVM, bool getResultByTreeView)
        {
            _getResultByTreeView = getResultByTreeView;
            if (getResultByTreeView)
            {
                isNew = true;
                isModified = true;
            }
            _getResultByQuery = true;
            _TableViewModel = TVM;

            simpleCommand = new RelayCommand(new Action<object>(DoSimpleCommand));

            RefreshResult();
        }
        
        private void DoSimpleCommand(Object obj)
        {
            var columnPopup = new GridHeaderColumn();
            MainWindow mainWindow = new MainWindow();
            if (mainWindow.queryBuilder == null)
            {
                mainWindow.queryBuilder = MainWindow.LatestQueryBuilder;
            }
            string dbColumn = obj.ToString();
            dbColumn = dbColumn.Trim();
            dbColumn = dbColumn.Replace(" ", "_");
            connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
            DataTable dataTable = new DataTable();
            DataSet columnds = new DataSet();
            SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
            column.Name = dbColumn;
            column.Format = "";
            column.AliasName = "";
            dataTable = MySQLData.DataAccess.ADODataBridge.getColumnStats(connectionString, mainWindow.queryBuilder, column);
            columnPopup.ColumnDataGrid.DataContext = dataTable;
            columnPopup.Show();
        }

        public ResultViewModel(SQLBuilder.SelectQueryBuilder QueryBuilder, string CurrentDatabaseName)
        {
            _getResultByQuery = true;
            _getResultByTreeView = false;
            _queryBuilder = QueryBuilder;
            _currentDatabaseName = CurrentDatabaseName;
            simpleCommand = new RelayCommand(new Action<object>(DoSimpleCommand));
            RefreshResult();
        }
        
        public ResultViewModel(DataView dataView)
        {
            simpleCommand = new RelayCommand(new Action<object>(DoSimpleCommand));
            Results = dataView;
        }
        
        public void CallRefeshResult()
        {
            RefreshResult1();
        }
        
        #region ColumnCommandProperty

        /// <summary>
        /// Gets the command for moving to the first column page of Result.
        /// </summary>
        public ICommand FirstColumnCommand
        {
            get
            {
                if (_firstColumnCommand == null)
                {
                    _firstColumnCommand = new RelayCommand
                    (
                        param =>
                        {
                            startColumn = 0;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor = null;
                        },
                        param =>
                        {
                            return startColumn - columnPageSize >= 0 ? true : false;
                        }
                    );
                }

                return _firstColumnCommand;
            }
        }
        /// <summary>
        /// Gets the command for moving to the previous column page of Result.
        /// </summary>
        public ICommand PreviousColumnCommand
        {
            get
            {
                if (_previousColumnCommand == null)
                {
                    _previousColumnCommand = new RelayCommand
                    (
                        param =>
                        {
                            startColumn -= columnPageSize;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor = null;
                        },
                        param =>
                        {
                            return startColumn - columnPageSize >= 0 ? true : false;
                        }
                    );
                }

                return _previousColumnCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the next column page of Result.
        /// </summary>
        public ICommand NextColumnCommand
        {
            get
            {
                if (_nextColumnCommand == null)
                {
                    _nextColumnCommand = new RelayCommand
                    (
                        param =>
                        {
                            startColumn += columnPageSize;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor = null;
                        },
                        param =>
                        {
                            return startColumn + columnPageSize < totalColumns ? true : false;
                        }
                    );
                }

                return _nextColumnCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the last column page of Result.
        /// </summary>
        public ICommand LastColumnCommand
        {
            get
            {
                if (_lastColumnCommand == null)
                {
                    _lastColumnCommand = new RelayCommand
                    (
                        param =>
                        {
                            startColumn = (totalColumns / columnPageSize - 1) * columnPageSize;
                            startColumn += totalColumns % columnPageSize == 0 ? 0 : columnPageSize;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor =null;
                        },
                        param =>
                        {
                            return startColumn + columnPageSize < totalColumns ? true : false;
                        }
                    );
                }

                return _lastColumnCommand;
            }
        }
        
        #endregion

        /// <summary>
        /// Sorts the list of products.
        /// </summary>
        /// <param name="sortColumn">The column or member that is the basis for sorting.</param>
        /// <param name="ascending">Set to true if the sort</param>
        
        public void Sort(string sortColumn, bool ascending)
        {
            this.sortColumn = sortColumn;
            this.ascending = ascending;
            startRow = 0;
            RefreshResult();
        }

        public void GotoPage(Int64 StartRow)
        {
            startRow = StartRow -1;
            RefreshResult();
        }
        
        private void RefreshResult1()
        {
            string connectionString = null;
            if (!_getResultByTreeView)
            {
                connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
            }
            else
            {
                connectionString = "";
                if (_TableViewModel._ParentSchemaName == ConfigurationManager.AppSettings["DerivedTablesPath"])
                {
                    connectionString = ConfigurationManager.AppSettings["FastDBConn"];
                }
                else
                {
                    connectionString = ConfigurationManager.AppSettings["FastDBConn"];
                }

                _queryBuilder = new SQLBuilder.SelectQueryBuilder();
                //loading query builder for first time call we have to manulay populate query builder
                SQLBuilder.Clauses.Table fromTable = null;
                if (_TableViewModel._table is MySQLData.DerivedTable)
                {
                    fromTable = new SQLBuilder.Clauses.DerivedTable((MySQLData.DerivedTable)_TableViewModel._table, "X");
                }
                else if (_TableViewModel._table is MySQLData.Table)
                {
                    fromTable = new SQLBuilder.Clauses.Table(_TableViewModel._table, "X");
                }

                _queryBuilder.SelectFromTable(fromTable);
            }

            bool isAddedColumnsManually = false;
            if (_queryBuilder.CrossTabClause.Col == null && (_queryBuilder.GroupByColumns == null || _queryBuilder.GroupByColumns.Count <= 0)
                    && (_queryBuilder.SelectedColumns == null || _queryBuilder.SelectedColumns.Count <= 0))
            {
                isAddedColumnsManually = true;
                if (_TableViewModel != null && _TableViewModel._table != null)
                {
                    foreach (MySQLData.Column col in _TableViewModel._table.columns)
                    {
                        //load query builder select columns
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        column.Name = "X." + col.name;
                        column.AliasName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(col.name);
                        _queryBuilder.SelectColumn(column);
                    }
                }
                else
                {
                    foreach (Table Tab in _queryBuilder.SelectedTables)
                    {
                        string tableName = Tab.Name;
                        List<MySQLData.Column> columns = MySQLData.DataAccess.ADODataBridge.getTableColumns(connectionString, ConfigurationManager.AppSettings["DefaultDatabase"], tableName);
                        foreach (MySQLData.Column col in columns)
                        {
                            //load query builder select columns
                            SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                            column.Name = "X." + col.name;
                            column.AliasName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(col.name);
                            _queryBuilder.SelectColumn(column);
                        }
                        break;
                    }
                }
            }

            if (_queryBuilder.CrossTabClause.Col == null)
            {
                if (_getResultByQuery)
                {
                    Results1 = MySQLData.DataAccess.ADODataBridge.getData(connectionString, _queryBuilder, 0, Convert.ToInt32(totalRows), 0, totalColumns, sortColumn, ascending, out totalRows, out totalColumns).DefaultView;
                    _getResultByQuery = false;
                }
                else
                {
                    Results1 = MySQLData.DataAccess.ADODataBridge.getData(connectionString, _queryBuilder, 0, Convert.ToInt32(totalRows), 0, totalColumns, sortColumn, ascending, out totalColumns).DefaultView;
                }
            }
            else
            {
                Results1 = MySQLData.DataAccess.ADODataBridge.getCrossTabulationData(connectionString, _queryBuilder, out totalRows, out totalColumns).Results.DefaultView;
            }

            if (isAddedColumnsManually)
            {
                _queryBuilder.SelectedColumns.Clear();
            }
            // to determine to show scroll bar
            if (Results1.Count == totalRows)
            {
                _ShowVertcalScrollBar = false;

            }
            if (Results1.Table.Columns.Count == totalColumns)
            {
                _ShowHorizontalScrollBar = false;
            }

            NotifyPropertyChanged("Results");
            NotifyPropertyChanged("Results1");
            NotifyPropertyChanged("StartRow");
            NotifyPropertyChanged("EndRow");
            NotifyPropertyChanged("TotalIRows");

            NotifyPropertyChanged("StartColumn");
            NotifyPropertyChanged("EndColumn");
            NotifyPropertyChanged("TotalColumns");
            MainWindow.LatestQueryBuilder = _queryBuilder;
        }
        
        private void RefreshResult()
        {
            string connectionString = null;
            if (!_getResultByTreeView)
            {
                connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
            }
            else
            {
                connectionString = "";
                if (_TableViewModel._ParentSchemaName == ConfigurationManager.AppSettings["DerivedTablesPath"])
                {
                    connectionString = ConfigurationManager.AppSettings["FastDBConn"];
                }
                else
                {
                    connectionString = ConfigurationManager.AppSettings["FastDBConn"];
                }

                _queryBuilder = new SQLBuilder.SelectQueryBuilder();
                //loading query builder for first time call we have to manulay populate query builder
                SQLBuilder.Clauses.Table fromTable = null;
                 if (_TableViewModel._table is MySQLData.DerivedTable)
                 {
                     fromTable = new SQLBuilder.Clauses.DerivedTable((MySQLData.DerivedTable)_TableViewModel._table, "X");
                 }
                 else if (_TableViewModel._table is MySQLData.Table)
                 {
                     fromTable = new SQLBuilder.Clauses.Table(_TableViewModel._table, "X");
                 }
                 _queryBuilder.SelectFromTable(fromTable);
            }
            bool isAddedColumnsManually = false;
            if (_queryBuilder.CrossTabClause.Col == null && (_queryBuilder.GroupByColumns== null || _queryBuilder.GroupByColumns.Count <= 0) 
                    && (_queryBuilder.SelectedColumns == null || _queryBuilder.SelectedColumns.Count <= 0))
            {
                isAddedColumnsManually = true;
                if (_TableViewModel != null && _TableViewModel._table != null)
                {
                    foreach (MySQLData.Column col in _TableViewModel._table.columns)
                    {
                        //load query builder select columns
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        column.Name = "X." + col.name;
                        _queryBuilder.SelectColumn(column);
                    }
                }
                else
                {
                    foreach (Table Tab in _queryBuilder.SelectedTables)
                    {
                        string tableName = Tab.Name;
                        List<MySQLData.Column> columns = MySQLData.DataAccess.ADODataBridge.getTableColumns(connectionString, ConfigurationManager.AppSettings["DefaultDatabase"], tableName);
                        foreach (MySQLData.Column col in columns)
                        {
                            //load query builder select columns
                            SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                            column.Name = "X." + col.name;
                            _queryBuilder.SelectColumn(column);
                        }
                        break;
                    }
                }
            }

            if (_queryBuilder.CrossTabClause.Col == null)
            {
                if (_getResultByQuery)
                {
                    Results = MySQLData.DataAccess.ADODataBridge.getData(connectionString, _queryBuilder, startRow, rowPageSize, startColumn, columnPageSize, sortColumn, ascending, out totalRows, out totalColumns).DefaultView;
                    _getResultByQuery = false;
                }
                else
                {
                    Results = MySQLData.DataAccess.ADODataBridge.getData(connectionString, _queryBuilder, startRow, rowPageSize, startColumn, columnPageSize, sortColumn, ascending, out totalColumns).DefaultView;
                }
            }
            else
            {
                Results = MySQLData.DataAccess.ADODataBridge.getCrossTabulationData(connectionString, _queryBuilder, out totalRows, out totalColumns).Results.DefaultView;
            }

            if (isAddedColumnsManually)
            {
                _queryBuilder.SelectedColumns.Clear();
            }
            // to determine to show scroll bar
            if (Results.Count == totalRows)
            {
                _ShowVertcalScrollBar = false;

            }
            if (Results.Table.Columns.Count == totalColumns)
            {
                _ShowHorizontalScrollBar = false;
            }
            
            NotifyPropertyChanged("Results");
            NotifyPropertyChanged("StartRow");
            NotifyPropertyChanged("EndRow");
            NotifyPropertyChanged("TotalIRows");

            NotifyPropertyChanged("StartColumn");
            NotifyPropertyChanged("EndColumn");
            NotifyPropertyChanged("TotalColumns");
            MainWindow.LatestQueryBuilder = _queryBuilder;
        }

        public SQLBuilder.SelectQueryBuilder QueryBulder
        {
            get
            {
                return _queryBuilder;
            }
            private set
            {
                if (object.ReferenceEquals(_queryBuilder, value) != true)
                {

                    _queryBuilder = value;

                }
            }
        }
        /// <summary>
        /// The list of results in the current page.
        /// </summary>
        public DataView Results
        {
            get
            {
                return _Results;
            }
            private set
            {
                if (object.ReferenceEquals(_Results, value) != true)
                {
                   
                    _Results = value;
                    
                }
            }
        }
        public DataView Results1
        {
            get
            {
                return _Results1;
            }
            private set
            {
                if (object.ReferenceEquals(_Results1, value) != true)
                {

                    _Results1 = value;

                }
            }
        }
        /// <summary>
        /// Gets the index of the first item in the products list.
        /// </summary>
        public Int64 StartRow { get { return startRow + 1; } }

        /// <summary>
        /// Gets the index of the last item in the products list.
        /// </summary>
        public Int64 EndRow { get { return startRow + rowPageSize < totalRows ? startRow + rowPageSize : totalRows; } }

        /// <summary>
        /// The number of total items in the data store.
        /// </summary>
        public Int64 TotalIRows { get { return totalRows; } }

        //******************
        /// <summary>
        /// Gets the index of the first  column in the Result Columns.
        /// </summary>
        public int StartColumn { get { return startColumn + 1; } }

        /// <summary>
        /// Gets the index of the last column in the Result Columns.
        /// </summary>
        public int EndColumn { get { return startColumn + columnPageSize < totalColumns ? startColumn + columnPageSize : totalColumns; } }

        /// <summary>
        /// The number of total column in the Result
        /// </summary>
        
        public int TotalColumns { get { return totalColumns; } }
       
        //************************

        // Row Command Property

        #region RowCommandProperty
        /// <summary>
        /// Gets the command for moving to the first page of products.
        /// </summary>
        public ICommand FirstRowCommand
        {
            get
            {
                if (_firstRowCommand == null)
                {
                    _firstRowCommand = new RelayCommand
                    (
                        param =>
                        {
                            startRow = 0;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor = null;
                        },
                        param =>
                        {
                            return startRow - rowPageSize >= 0 ? true : false;
                        }
                    );
                }

                return _firstRowCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the previous page of products.
        /// </summary>
        public ICommand PreviousRowCommand
        {
            get
            {
                if (_previousRowCommand == null)
                {
                    _previousRowCommand = new RelayCommand
                    (
                        param =>
                        {
                            startRow -= rowPageSize;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor =null;
                        },
                        param =>
                        {
                            return startRow - rowPageSize >= 0 ? true : false;
                        }
                    );
                }

                return _previousRowCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the next page of products.
        /// </summary>
        public ICommand NextRowCommand
        {
            get
            {
                if (_nextRowCommand == null)
                {
                    _nextRowCommand = new RelayCommand
                    (
                        param =>
                        {
                            startRow += rowPageSize;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor = null;
                        },
                        param =>
                        {
                            return startRow + rowPageSize < totalRows ? true : false;
                        }
                    );
                }

                return _nextRowCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the last page of products.
        /// </summary>
        public ICommand LastRowCommand
        {
            get
            {
                if (_lastRowCommand == null)
                {
                    _lastRowCommand = new RelayCommand
                    (
                        param =>
                        {
                            startRow = (totalRows / rowPageSize - 1) * rowPageSize;
                            startRow += totalRows % rowPageSize == 0 ? 0 : rowPageSize;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            RefreshResult();
                            Mouse.OverrideCursor = null;
                        },
                        param =>
                        {
                            return startRow + rowPageSize < totalRows ? true : false;
                        }
                    );
                }

                return _lastRowCommand;
            }
        }
        #endregion
        
        /// <summary>
        /// Notifies subscribers of changed properties.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }      

        
        /*public ICommand ColumnHeaderCommand

        public void HandleShowMessage()
        {
            MessageBox.Show("Hello Welcome to EventTrigger for MVVM.");
        }*/


    }



}
