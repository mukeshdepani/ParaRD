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
    /// <summary>
    /// Interaction logic for ResultViewControl.xaml
    /// </summary>

    public partial class ResultViewControl : UserControl
    {
        public string CurrentDatabaseName;
        private string connectionString;
        public List<MySQLData.Table> listOfTable;
        public ResultViewModel result;
        private bool isErrorLoggingOn = Convert.ToBoolean(ConfigurationManager.AppSettings["isErrorLoggingOn"].ToString());
        private bool isAllTabValidated = false;
        private string queryString;
        public string flName = "";
        private string _tableName;
        public tableViewModel tvm1;
        
        private void OnTabItemSelecting(Object sender, CurrentChangingEventArgs e)
        {

        }
        
        private void DisplayErrorMessage()
        {
            MessageBox.Show(ConfigurationManager.AppSettings["ErrorMessage"]);
        }
        
        private void DisplayConfirmationMessage()
        {
            MessageBox.Show(ConfigurationManager.AppSettings["ConfirmationMesssage"]);
        }
        
        public ResultViewControl(tableViewModel tvm, bool getResultByTreeView)
        {

            InitializeComponent();
            if (tvm != null)
            {
                MainGirdViewControl mainGridView = new MainGirdViewControl(tvm, getResultByTreeView);
                result = mainGridView.result;
                _tableName = tvm._table.name;
                tvm1 = tvm;

                if (this.StackPanelResultViewControl.Children.Count == 0)
                {
                    this.StackPanelResultViewControl.Children.Add(mainGridView);
                    this.ResultTab.IsSelected = true;             
                    this.CustomQueryAccordion.SelectionMode = AccordionSelectionMode.ZeroOrMore;
                    int mainGridWidth = (((FastDB.ResultViewModel)(mainGridView.MainDataGrid.DataContext)).EndColumn * 160);
                    {
                        mainGridView.MainDataGrid.Width = mainGridWidth;
                    }
                    mainGridView.MainDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                }
                this.DataContext = result;
            }
        }
        
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;

            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;

            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;

            }
        }
        
        public ResultViewControl(SQLBuilder.SelectQueryBuilder QuerryBuilder, string CurrentDatabaseName)
        {
            InitializeComponent();
            if (QuerryBuilder.CrossTabClause != null)
            {
                if (QuerryBuilder.CrossTabClause.Col == null)
                {
                    if (this.StackPanelResultViewControl.Children.Count == 0)
                    {
                        MainGirdViewControl mainGridView = new MainGirdViewControl(QuerryBuilder, CurrentDatabaseName);
                        result = mainGridView.result;
                        this.StackPanelResultViewControl.Children.Add(mainGridView);
                        this.ResultTab.IsSelected = true;
                        this.CustomQueryAccordion.SelectionMode = AccordionSelectionMode.ZeroOrMore;
                        int mainGridWidth = (((FastDB.ResultViewModel)(mainGridView.MainDataGrid.DataContext)).EndColumn * 160);
                        {
                            mainGridView.MainDataGrid.Width = mainGridWidth;
                        }
                        mainGridView.MainDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;    //zahed
                    }
                }
                else
                {
                    if (this.StackPanelResultViewControl.Children.Count == 1)
                    {
                        if ((MainGirdViewControl)this.StackPanelResultViewControl.Children[0] != null)
                        {
                            this.StackPanelResultViewControl.Children.RemoveAt(0);
                        }
                    }
                    CrossTabulationViewControl crossTabViewControl = new CrossTabulationViewControl(QuerryBuilder, CurrentDatabaseName);
                    result = crossTabViewControl.result;
                    this.ResultTab.IsSelected = true;
                    this.CustomQueryAccordion.SelectionMode = AccordionSelectionMode.ZeroOrMore;
                    this.StackPanelResultViewControl.Children.Add(crossTabViewControl);
                }
            }
            this.DataContext = result;
        }

        private void RunQueryBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.ShortCutsToolBar);
            isAllTabValidated = mainWindow.ValidateAllTabCntrls(this);

            SelectQueryBuilder queryBuilder = mainWindow.LoadSelectQueryBuilderNew(this);
            string queryString;
            if (queryBuilder != null)
            {
                queryString = queryBuilder.BuildQuery();
            }
            XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
            StringWriter writer = new StringWriter();
            SerializerObj.Serialize(writer, queryBuilder);
            DateTime startTime = DateTime.Now;
            TabItem rvctabItem = RVCTAbControl.SelectedItem as TabItem;
            if (XmlSQLTabTxt.Text != null && rvctabItem.Name == "XmlSQLTab")
            {
                try
                {
                    connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
                    CurrentDatabaseName = ConfigurationManager.AppSettings["DefaultDatabase"];

                    connectionString = connectionString + "Database=" + CurrentDatabaseName + ";";
                    DataTable dataTable = new DataTable();
                    string sqldata = "";
                    dataTable = (MySQLData.DataAccess.ADODataBridge.getData(connectionString, XmlSQLTabTxt.Text));
                    DataView dataView = new DataView(dataTable);
                    ResultViewModel rvm = new ResultViewModel(dataView);
                    MainGirdViewControl mainGridView1 = new MainGirdViewControl(queryBuilder, mainWindow.CurrentDatabaseName, sqldata);

                    CloseableTabItem tabItem = (CloseableTabItem)this.ResultTab;

                    rvm.isModified = true;
                    rvm.isNew = true;

                    mainGridView1.DataContext = rvm;
                    tabItem.Content = mainGridView1;
                    mainGridView1.MainDataGrid.Width = dataTable.Columns.Count * 160;
                    mainGridView1.MainDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    this.DataContext = rvm;
                    this.ResultTab.IsSelected = true;
                    lblXmlSQLTabErrorMessage.Visibility = System.Windows.Visibility.Hidden;

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    if (isErrorLoggingOn)
                    {
                        lblXmlSQLTabErrorMessage.Visibility = System.Windows.Visibility.Visible;
                        lblXmlSQLTabErrorMessage.Height = 20;
                        lblXmlSQLTabErrorMessage.Content = ex.Message;
                        LogError.Log_Err("btnRunQuery_Click", ex);
                    }
                }
                catch (Exception ex)
                {
                    if (isErrorLoggingOn)
                    {
                        LogError.Log_Err("btnRunQuery_Click", ex);
                    }
                }
                Mouse.OverrideCursor = null;
            }
            else if (isAllTabValidated)
            {
                if (mainWindow.queryString != String.Empty)
                {
                    if (queryBuilder != null)
                    {
                        CloseableTabItem tabItem = (CloseableTabItem)this.ResultTab;

                        if (tabItem.Content == null)
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            try
                            {
                                if (queryBuilder.CrossTabClause != null)
                                {
                                    if (queryBuilder.CrossTabClause.Col == null)
                                    {
                                        MainGirdViewControl mainGridView1 = new MainGirdViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
                                        result = mainGridView1.result;

                                        result.isModified = true;
                                        result.isNew = true;

                                        mainGridView1.DataContext = result;
                                        tabItem.Content = mainGridView1;

                                        mainGridView1.MainDataGrid.Width = result.EndColumn * 160;
                                        mainGridView1.MainDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                                        this.DataContext = result;
                                        this.ResultTab.IsSelected = true;
                                    }
                                    else
                                    {
                                        if (this.StackPanelResultViewControl.Children.Count == 1)
                                        {
                                            if ((MainGirdViewControl)this.StackPanelResultViewControl.Children[0] != null)
                                            {
                                                this.StackPanelResultViewControl.Children.RemoveAt(0);
                                            }
                                        }
                                        CrossTabulationViewControl crossTabViewControl = new CrossTabulationViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
                                        result = crossTabViewControl.result;
                                        this.StackPanelResultViewControl.Children.Add(crossTabViewControl);
                                    }
                                }
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                if (isErrorLoggingOn)
                                {
                                    LogError.Log_Err("btnRunQuery_Click", ex);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (isErrorLoggingOn)
                                {
                                    LogError.Log_Err("btnRunQuery_Click", ex);
                                }
                            }
                            Mouse.OverrideCursor = null;
                        }
                        else
                        {

                            ResultViewModel rv = (ResultViewModel)this.result;
                            if (rv.isNew)
                            {
                                // we still let user run the query, query still new and modified= true
                                Mouse.OverrideCursor = Cursors.Wait;
                                try
                                {
                                    if (queryBuilder.CrossTabClause != null)
                                    {
                                        if (queryBuilder.CrossTabClause.Col == null)
                                        {
                                            if (queryBuilder.GroupByColumns.Count == 0 && queryBuilder.SelectedColumns.Count == 0)
                                            {
                                                foreach (SQLBuilder.Clauses.Column col in this.SelectTabCntrl.lstToSelecteColFrom.Items)
                                                {
                                                    queryBuilder.SelectColumn(col);
                                                }
                                            }
                                            MainGirdViewControl mainGridView1 = new MainGirdViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
                                            result = mainGridView1.result;

                                            result.isModified = true;
                                            result.isNew = true;

                                            mainGridView1.DataContext = result;
                                            tabItem.Content = mainGridView1;

                                            mainGridView1.MainDataGrid.Width = result.EndColumn * 160;
                                            mainGridView1.MainDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                            CloseableTabItem ti = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;
                                            ti.labelStar.Content = "*";
                                            this.DataContext = result;
                                            this.ResultTab.IsSelected = true;
                                        }
                                        else
                                        {
                                            if (this.StackPanelResultViewControl.Children.Count == 1)
                                            {
                                                if (this.StackPanelResultViewControl.Children[0] != null)
                                                {
                                                    this.StackPanelResultViewControl.Children.RemoveAt(0);
                                                }
                                            }
                                            CrossTabulationViewControl crossTabViewControl = new CrossTabulationViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
                                            result = crossTabViewControl.result;
                                            this.StackPanelResultViewControl.Children.Add(crossTabViewControl);
                                            this.ResultTab.IsSelected = true;
                                        }
                                    }
                                }
                                catch (MySql.Data.MySqlClient.MySqlException ex)
                                {
                                    if (isErrorLoggingOn)
                                    {
                                        LogError.Log_Err("btnRunQuery_Click", ex);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (isErrorLoggingOn)
                                    {
                                        LogError.Log_Err("btnRunQuery_Click", ex);
                                    }
                                }
                                Mouse.OverrideCursor = null;
                            }
                            else
                            {
                                // it is old
                                //check to see if old querybuilder and new querybuilder is same, if different CompairQueryBuilder retuns true
                                if (mainWindow.CompairQueryBuilder(rv.QueryBulder, queryBuilder))
                                {
                                    Mouse.OverrideCursor = Cursors.Wait;
                                    try
                                    {
                                        string directoryPath = String.Empty;
                                        if (result != null)
                                        {
                                            if (result.directoryPath != null)
                                            {
                                                directoryPath = result.directoryPath;
                                            }
                                        }

                                        if (queryBuilder.CrossTabClause != null && queryBuilder.CrossTabClause.Col != null)
                                        {
                                            if (this.StackPanelResultViewControl.Children.Count == 1)
                                            {
                                                if (this.StackPanelResultViewControl.Children[0] != null)
                                                {
                                                    this.StackPanelResultViewControl.Children.RemoveAt(0);
                                                }
                                            }
                                            CrossTabulationViewControl crossTabViewControl = new CrossTabulationViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
                                            result = crossTabViewControl.result;
                                            this.StackPanelResultViewControl.Children.Add(crossTabViewControl);
                                            this.ResultTab.IsSelected = true;
                                            CloseableTabItem ti = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;
                                            ti.labelStar.Content = "*";
                                        }
                                        else
                                        {
                                            MainGirdViewControl mainGridView1 = new MainGirdViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
                                            result = mainGridView1.result;
                                            mainGridView1.DataContext = result;
                                            tabItem.Content = mainGridView1;

                                            mainGridView1.MainDataGrid.Width = result.EndColumn * 160;
                                            mainGridView1.MainDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                            CloseableTabItem ti = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;
                                            ti.labelStar.Content = "*";
                                        }

                                        if (directoryPath != String.Empty)
                                        {
                                            result.directoryPath = directoryPath;
                                        }

                                        result.isModified = true;
                                        result.isNew = false;

                                        this.DataContext = result;
                                        this.ResultTab.IsSelected = true;
                                    }
                                    catch (MySql.Data.MySqlClient.MySqlException ex)
                                    {
                                        if (isErrorLoggingOn)
                                        {
                                            LogError.Log_Err("btnRunQuery_Click", ex);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (isErrorLoggingOn)
                                        {
                                            LogError.Log_Err("btnRunQuery_Click", ex);
                                        }
                                    }
                                    Mouse.OverrideCursor = null;
                                }
                                this.ResultTab.IsSelected = true;
                            }
                        }
                    }
                }
                XmlSQLTabTxt.Text = queryBuilder.BuildQuery();
            }
            
            Console.WriteLine("Cross Tabulation View Control execution time: " + (DateTime.Now - startTime));
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

        private void OnChecked_chkSSQL(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(ConfigurationManager.AppSettings["ConfirmationMesssage"], "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                XmlSQLTabTxt.IsEnabled = true;
                QueryTab.IsEnabled = false;
                chkSQL.IsChecked = true;
            }
            else if (result == MessageBoxResult.No)
            {
                XmlSQLTabTxt.IsEnabled = false;
                QueryTab.IsEnabled = true;
                chkSQL.IsChecked = false;
            }
            
        }

        private void OnUnChecked_chkSSQL(object sender, RoutedEventArgs e)
        {
            chkSQL.IsChecked = false;
            XmlSQLTabTxt.IsEnabled = false;
            QueryTab.IsEnabled = true;
        }

        private void StopExecutionBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveQueryBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this.GetTopLevelControl(this.RVCTAbControl);
            if (parent != null)
            {
                MainWindow mainWindow = (MainWindow)parent;
                CloseableTabItem closeableTabItem = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;

                if (closeableTabItem.Name != "tabItem1")
                {
                    CheckQueryNeededToBeSaved(closeableTabItem, "Save");
                }
            }
        }

        private void SaveQueryAsBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this.GetTopLevelControl(this.RVCTAbControl);
            if (parent != null)
            {
                MainWindow mainWindow = (MainWindow)parent;
                CloseableTabItem closeableTabItem = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;

                if (closeableTabItem.Name != "tabItem1")
                {
                    CheckQueryNeededToBeSaved(closeableTabItem, "SaveAs");
                }
            }
        }
        
        private void SaveXML_Click(object sender, RoutedEventArgs e)
        {
            var newW = new Window1(this);

            newW.Show();
        }
        
        public void SaveXMLFile(string fileName)
        {
            flName = fileName;
            DependencyObject parent = this.GetTopLevelControl(this.RVCTAbControl);
            if (parent != null)
            {
                MainWindow mainWindow = (MainWindow)parent;
                CloseableTabItem closeableTabItem = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;

                if (closeableTabItem.Name != "tabItem1")
                {
                    CheckQueryNeededToBeSaved(closeableTabItem, "SaveXML");
                }
            }
        }

        public void CheckQueryNeededToBeSaved(CloseableTabItem tabItem, String caller)
        {
            /*****************To Save Modified Query***********/

            string directoryPath = this.result.directoryPath;

            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.ShortCutsToolBar);
            mainWindow.ValidateAllTabCntrls(this);
            SelectQueryBuilder queryBuilder = mainWindow.LoadSelectQueryBuilderNew(this);

            MainGirdViewControl mainGridView1 = new MainGirdViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
            this.result = mainGridView1.result;
            if (directoryPath != null)
            {
                this.result.isModified = true;
                this.result.isNew = false;
                this.result.directoryPath = directoryPath;
            }
            else
            {
                this.result.isModified = true;
                this.result.isNew = true;
            }

            /********************************************/

            ResultViewModel rv = (ResultViewModel)this.result;   

            if (this.Content != null)
            {
                if (rv.isModified)
                {
                    if (rv.isNew == false)
                    {
                        switch (caller)
                        {
                            case "Save":

                                string FileName = rv.directoryPath + tabItem.Header.ToString() + ".xml";
                                if (FileName != System.String.Empty)
                                {
                                    XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                                    StreamWriter swriter = new StreamWriter(FileName);
                                    SerializerObj.Serialize(swriter, rv.QueryBulder);

                                    swriter.Flush();
                                    swriter.Close();
                                    tabItem.Header = tabItem.Header.ToString();
                                    //we saved the query change isModified to false
                                    rv.isModified = false;
                                    tabItem.labelStar.Content = "";
                                    MessageBox.Show("query saved successfully");
                                }
                                break;

                            case "SaveAs":

                                MainWindow mw = new MainWindow();
                                string FileName1 = mw.OpenSaveDialog(tabItem.Name);

                                if (FileName1 != System.String.Empty)
                                {
                                    XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                                    StreamWriter swriter = new StreamWriter(FileName1);
                                    SerializerObj.Serialize(swriter, rv.QueryBulder);

                                    swriter.Flush();
                                    swriter.Close();
                                    string[] splitedArray = FileName1.Split('\\');
                                    string tabItemHeader = splitedArray[splitedArray.Length - 1].Remove((splitedArray[splitedArray.Length - 1]).Length - 4, 4);
                                    tabItem.Header = tabItemHeader;
                                    //we saved the query change isModified to false
                                    rv.isModified = false;
                                    tabItem.labelStar.Content = "";
                                    MessageBox.Show("query saved successfully");
                                }
                                break;

                            case "SaveXML":

                                FileName = ConfigurationManager.AppSettings["DerivedTablesPath"].ToString() + flName + ".xml";
                                if (File.Exists(FileName))
                                {
                                    MessageBox.Show("File already exists....");
                                }
                                else
                                {
                                    if (FileName != System.String.Empty)
                                    {
                                        XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                                        StreamWriter swriter = new StreamWriter(FileName);
                                        SerializerObj.Serialize(swriter, rv.QueryBulder);

                                        swriter.Flush();
                                        swriter.Close();
                                        tabItem.Header = tabItem.Header.ToString();

                                        rv.isModified = false;
                                        tabItem.labelStar.Content = "";
                                        MessageBox.Show("query saved successfully");

                                        connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
                                        CurrentDatabaseName = ConfigurationManager.AppSettings["DefaultDatabase"];

                                        connectionString = connectionString + "Database=" + CurrentDatabaseName + ";";
                                        DependencyObject parent = this.GetTopLevelControl(this);

                                        try
                                        {
                                            List<MySQLData.Schema> schemas = MySQLData.DataAccess.ADODataBridge.getSchemaTree(connectionString, CurrentDatabaseName, ConfigurationManager.AppSettings["DerivedTablesPath"]);//DataAccess.GetDatabases();
                                            MainViewModel viewModel = new MainViewModel(schemas);

                                            if (parent != null)
                                            {
                                                MainWindow m = (MainWindow)parent;
                                                m.MainTreeView.DataContext = viewModel;
                                                listOfTable = new List<MySQLData.Table>();
                                                foreach (MySQLData.Schema schema in schemas)
                                                {
                                                    listOfTable.AddRange(schema.tables);
                                                }
                                                if (listOfTable != null)
                                                {

                                                }
                                            }

                                        }
                                        catch (MySql.Data.MySqlClient.MySqlException ex)
                                        {
                                            if (isErrorLoggingOn)
                                            {
                                                LogError.Log_Err("MainWindow Constructor", ex);
                                                DisplayErrorMessage();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (isErrorLoggingOn)
                                            {
                                                LogError.Log_Err("MainWindow Constructor", ex);
                                                DisplayErrorMessage();
                                            }
                                        }
                                        if (parent != null)
                                        {
                                            MainWindow m = (MainWindow)parent;
                                            var collView = CollectionViewSource.GetDefaultView(m.tabControlCustomQuery.Items);
                                            collView.CurrentChanging += this.OnTabItemSelecting;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (caller)
                        {
                            case "Save":
                                if (rv.TotalIRows != 0)
                                {
                                    SaveNewQuery(tabItem, rv.QueryBulder);
                                }
                                break;

                            case "SaveAs":

                                if (rv.TotalIRows != 0)
                                {
                                    //save save as the query
                                    MainWindow mw = new MainWindow();
                                    string FileName = mw.OpenSaveDialog(tabItem.Name);

                                    if (FileName != System.String.Empty)
                                    {
                                        XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                                        StreamWriter swriter = new StreamWriter(FileName);
                                        SerializerObj.Serialize(swriter, rv.QueryBulder);

                                        swriter.Flush();
                                        swriter.Close();
                                        string[] splitedArray = FileName.Split('\\');
                                        string tabItemHeader = splitedArray[splitedArray.Length - 1].Remove((splitedArray[splitedArray.Length - 1]).Length - 4, 4);
                                        tabItem.Header = tabItemHeader;
                                        //we saved the query change isModified to false
                                        rv.isModified = false;
                                        tabItem.labelStar.Content = "";
                                        MessageBox.Show("query saved successfully");
                                    }
                                }
                                break;

                            case "SaveXML":
                                string FileNameXML = ConfigurationManager.AppSettings["DerivedTablesPath"].ToString() + flName + ".xml";
                                if (File.Exists(FileNameXML))
                                {
                                    MessageBox.Show("File already exists....");
                                }
                                else
                                {
                                    if (FileNameXML != System.String.Empty)
                                    {
                                        XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                                        StreamWriter swriter = new StreamWriter(FileNameXML);
                                        SerializerObj.Serialize(swriter, rv.QueryBulder);

                                        swriter.Flush();
                                        swriter.Close();
                                        tabItem.Header = tabItem.Header.ToString();

                                        rv.isModified = false;
                                        tabItem.labelStar.Content = "";
                                        MessageBox.Show("query saved successfully");


                                        connectionString = ConfigurationManager.AppSettings["DefaultDBConn"];
                                        CurrentDatabaseName = ConfigurationManager.AppSettings["DefaultDatabase"];

                                        connectionString = connectionString + "Database=" + CurrentDatabaseName + ";";
                                        DependencyObject parent = this.GetTopLevelControl(this);

                                        try
                                        {
                                            List<MySQLData.Schema> schemas = MySQLData.DataAccess.ADODataBridge.getSchemaTree(connectionString, CurrentDatabaseName, ConfigurationManager.AppSettings["DerivedTablesPath"]);
                                            MainViewModel viewModel = new MainViewModel(schemas);

                                            if (parent != null)
                                            {
                                                MainWindow m = (MainWindow)parent;
                                                m.MainTreeView.DataContext = viewModel;
                                                listOfTable = new List<MySQLData.Table>();
                                                foreach (MySQLData.Schema schema in schemas)
                                                {
                                                    listOfTable.AddRange(schema.tables);
                                                }
                                                if (listOfTable != null)
                                                {
                                                    
                                                }
                                            }

                                        }
                                        catch (MySql.Data.MySqlClient.MySqlException ex)
                                        {
                                            if (isErrorLoggingOn)
                                            {
                                                LogError.Log_Err("MainWindow Constructor", ex);
                                                DisplayErrorMessage();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (isErrorLoggingOn)
                                            {
                                                LogError.Log_Err("MainWindow Constructor", ex);
                                                DisplayErrorMessage();
                                            }
                                        }
                                        if (parent != null)
                                        {
                                            MainWindow m = (MainWindow)parent;
                                            var collView = CollectionViewSource.GetDefaultView(m.tabControlCustomQuery.Items);
                                            collView.CurrentChanging += this.OnTabItemSelecting;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (caller)
                    {
                        case "Save":
                            break;
                        case "SaveAs":
                            break;
                        case "SaveXML":
                            break;
                        case "CloseTab":
                            TabControl tabControl = tabItem.Parent as TabControl;
                            if (tabControl != null)
                                tabControl.Items.Remove(tabItem);
                            break;
                    }
                }
            }
            else
            {
                if (caller == "CloseTab")
                {
                    TabControl tabControl = tabItem.Parent as TabControl;
                    if (tabControl != null)
                        tabControl.Items.Remove(tabItem);
                }
            }
        }

        public void SaveNewQuery(CloseableTabItem tabItem, SelectQueryBuilder QueryBulder)
        {
            string messageBoxText = "Do you want to save current query?";
            string caption = "Current Query";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // User pressed Yes button
                    //save save query
                    ResultViewModel rv = (ResultViewModel)this.result;
                    string FileName1 = System.String.Empty;
                    if (rv.isNew == false)
                    {
                        FileName1 = rv.directoryPath + tabItem.Header.ToString() + ".xml";
                    }
                    else
                    {
                        MainWindow mw = new MainWindow();
                        FileName1 = mw.OpenSaveDialog(tabItem.Name);
                    }
                    if (FileName1 != System.String.Empty)
                    {
                        XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                        StreamWriter swriter = new StreamWriter(FileName1);
                        SerializerObj.Serialize(swriter, QueryBulder);

                        swriter.Flush();
                        swriter.Close();
                        string[] splitedArray = FileName1.Split('\\');
                        string tabItemHeader = splitedArray[splitedArray.Length - 1].Remove((splitedArray[splitedArray.Length - 1]).Length - 4, 4);
                        tabItem.Header = tabItemHeader;
                        //we saved the query change isModified to false
                        rv.isModified = false;
                        tabItem.labelStar.Content = "";
                        MessageBox.Show("query saved successfully");
                        /**************************/
                        this.result.directoryPath = FileName1.Replace(splitedArray[splitedArray.Length - 1], "");
                        /**************************/
                    }

                    break;
                case MessageBoxResult.No:
                    // User pressed No button
                    TabControl tabControl = tabItem.Parent as TabControl;
                    if (tabControl != null)
                        tabControl.Items.Remove(tabItem);
                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    break;
            }
        }

        private void CustomQueryAccordion_SelectedItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AccordionItem item = (AccordionItem)CustomQueryAccordion.SelectedItem;
            MainWindow mainWindow = new MainWindow();
            isAllTabValidated = mainWindow.ValidateAllTabCntrls(this);

            if (item != null)
            {
                switch (item.Name)
                {
                    case "FromTabItem":
                        break;

                    case "WhereTabItem":
                        break;

                    case "SelectAccordionItem":
                        if (this.FromTabCntrl.isValidated && (this.TabulationTabCntrl.isValidated && this.TabulationTabCntrl.isTabulation == false) && (this.CrossTabulationTabCntrl.isValidated && this.CrossTabulationTabCntrl.isCrossTabulation == false))
                        {
                            this.SelectTabCntrl.lstToSelecteColFrom.IsEnabled = true;
                            this.SelectTabCntrl.lstToSelecteColFrom.ItemsSource = mainWindow.GenerateListOfSelectTabCntrlColumns(this);
                        }
                        break;

                    case "TabulationAccordionItem":

                        if (this.FromTabCntrl.isValidated)
                        {
                            this.TabulationTabCntrl.Visibility = System.Windows.Visibility.Visible;
                            this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.IsEnabled = true;
                            this.TabulationTabCntrl.StackPanelTabuLationTabSummary.IsEnabled = true;

                            List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = mainWindow.GenerateListOfTabulationTabCntrlColumns(this);

                            //if first drop down is null on Tabulation means all dropdown item source is null
                            if (((TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.Items.Count == 0)
                            {
                                // loading groupby columns
                                for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                                {
                                    TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                                    tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                                // loading summary columns
                                for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                                {
                                    TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                                    ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                            }
                            else
                            {
                                List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.ItemsSource;

                                IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                                if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                                {
                                    // Reloading groupby columns
                                    for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                                    {
                                        TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                                        tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                                    }
                                    // Reloading summary columns
                                    for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                                    {
                                        TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                                        ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                                    }
                                }
                            }
                        }
                        break;
                    case "CrossTabulationAccordionItem":
                        if (this.FromTabCntrl.isValidated)
                        {
                            this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.IsEnabled = true;
                            this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.IsEnabled = true;
                            this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.IsEnabled = true;
                            this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.IsEnabled = true;
                            List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = mainWindow.GenerateListOfTabulationTabCntrlColumns(this);

                            //if first drop down is null on Cross Tabulation means all dropdown item source is null
                            if (((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.Items.Count == 0)
                            {
                                // loading groupby columns
                                for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                                {
                                    CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                                    ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                                //loading summary first row means (column Name and sort) row
                                this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;
                                // loading summary columns
                                for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                                {
                                    CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                                    cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                            }
                            else
                            {
                                List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.ItemsSource;

                                IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                                if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                                {
                                    // Reloading groupby columns
                                    for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                                    {
                                        CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                                        ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                                    }
                                    // Reloading summary first row means (column Name and sort) row
                                    this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;

                                    // Reloading summary columns
                                    for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                                    {
                                        CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                                        cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                                    }
                                }
                            }
                        }
                        break;

                    case "ActionTabAccordionItem":
                        if (isAllTabValidated)
                        {
                            SelectQueryBuilder queryBuilder = mainWindow.LoadSelectQueryBuilderNew(this);
                            if (queryBuilder != null)
                            {
                                queryString = queryBuilder.BuildQuery();
                            }
                            XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                            StringWriter writer = new StringWriter();
                            SerializerObj.Serialize(writer, queryBuilder);
                        }
                        else
                        {

                        }
                        break;
                }
            }
        }

        public void UpdateTabulationTabCntrl(MainWindow mainWindow)
        {
            if (this.FromTabCntrl.isValidated)
            {
                this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.IsEnabled = true;
                this.TabulationTabCntrl.StackPanelTabuLationTabSummary.IsEnabled = true;

                List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = mainWindow.GenerateListOfTabulationTabCntrlColumns(this);

                //if first drop down is null on Tabulation means all dropdown item source is null
                if (((TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.Items.Count == 0)
                {
                    // loading groupby columns
                    for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                    {
                        TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                        tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                    // loading summary columns
                    for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                    {
                        TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                        ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                }
                else
                {
                    List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.ItemsSource;

                    IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                    if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                    {
                        // Reloading groupby columns
                        for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                        {
                            TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                            tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                        }
                        // Reloading summary columns
                        for (int i = 0; i < this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                        {
                            TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                            ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                        }
                    }
                }
            }
        }

        public void UpdateCrossTabulationTabCntrl(MainWindow mainWindow)
        {
            if (this.FromTabCntrl.isValidated)
            {
                this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.IsEnabled = true;
                this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.IsEnabled = true;
                this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.IsEnabled = true;
                this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.IsEnabled = true;
                List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = mainWindow.GenerateListOfTabulationTabCntrlColumns(this);

                //if first drop down is null on Cross Tabulation means all dropdown item source is null
                if (((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.Items.Count == 0)
                {
                    // loading groupby columns
                    for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                    {
                        CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                        ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                    //loading summary first row means (column Name and sort) row
                    this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;
                    // loading summary columns
                    for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                    {
                        CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                        cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                }
                else
                {
                    List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.ItemsSource;

                    IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                    if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                    {
                        // Reloading groupby columns
                        for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                        {
                            CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                            ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                        }
                        // Reloading summary first row means (column Name and sort) row
                        this.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;

                        // Reloading summary columns
                        for (int i = 0; i < this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                        {
                            CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                            cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                        }
                    }
                }
            }
        }

        private void WhereGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        public void ExportToExcelFile()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.ShortCutsToolBar);
                CloseableTabItem closeAbleTabItem = (CloseableTabItem)mainWindow.tabControl1.SelectedItem;
                if (closeAbleTabItem != null)
                {
                    if (closeAbleTabItem.Name != "tabItem1")
                    {
                        ResultViewControl rc = (ResultViewControl)closeAbleTabItem.Content;
                        if (rc != null)
                        {
                            ResultViewModel rv = (ResultViewModel)rc.DataContext;
                            if (rv.Results != null)
                            {
                                try
                                {
                                    string connectionString = ConfigurationManager.AppSettings["DefaultDBConn"] + "database=" + mainWindow.CurrentDatabaseName + ";";
                                    

                                    ResultViewModel rvm = (ResultViewModel)rc.result;
                                    string cl = "";
                                    uint row = 2;
                                    System.Data.DataView allRecordsView = rvm.Results;
                                    string filePath = string.Empty;
                                    Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                                    sfd.DefaultExt = ".xlsx";
                                    sfd.Filter = "Default (.xlsx)|*.xlsx";
                                    if (sfd.ShowDialog() == true)
                                    {
                                        filePath = sfd.FileName;
                                        if (rvm.QueryBulder.CrossTabClause.Col != null && rvm.QueryBulder.CrossTabClause.Col.Name != "")
                                        {
                                            Common.CreateCrossTabulation(filePath, allRecordsView.Table, rv.QueryBulder.FinalSelectedColumns, rvm);
                                        }
                                        else
                                        {
                                            rv.CallRefeshResult();
                                            DataView dv = rv.Results1;
                                            Common.CreateSpreadsheetWorkbook(filePath, dv.Table, rv.QueryBulder.FinalSelectedColumns);
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    if (isErrorLoggingOn)
                                    {
                                        LogError.Log_Err("menuItemExportToExcel_Click", ex);
                                        MessageBox.Show(ConfigurationManager.AppSettings["ErrorMessage"]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcelFile();
        }
        
        private void RVCTAbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CloseableTabItem tabItem = (CloseableTabItem)sender;
            TabItem ti = RVCTAbControl.SelectedItem as TabItem;
            if (ti.Name == "XmlQueryTab")
            {
                MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.ShortCutsToolBar);

                if (mainWindow.ValidateAllTabCntrls(this))
                {
                    SelectQueryBuilder queryBuilder = mainWindow.LoadSelectQueryBuilderNew(this);
                    if (queryBuilder != null)
                    {
                        queryString = queryBuilder.BuildQuery();
                    }
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                    StringWriter writer = new StringWriter();
                    SerializerObj.Serialize(writer, queryBuilder);
                    this.XmlQueryTabTxt.Text = writer.ToString();
                    this.lblXmlQueryTabErrorMessage.Content = "";
                }
                else
                {
                    this.XmlQueryTabTxt.Text = "";
                    this.lblXmlQueryTabErrorMessage.Content = "There is an error on one or more tab, please fix an error";
                }
            }
        }
    }
}
