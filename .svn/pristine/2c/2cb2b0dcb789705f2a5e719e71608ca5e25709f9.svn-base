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
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MySQLData;
using System.Configuration;
using FastDB.Control;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using SQLBuilder;
using FastDB.Class;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace FastDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CurrentDatabaseName;         //made public zahed
        private string connectionString;
        public bool isAllTabValidated = false;       //made public zahed
        public string queryString;                 //made public zahed
        public SelectQueryBuilder queryBuilder;    ////made public zahed
        public List<MySQLData.Table> listOfTable;
        public string ElementNameValue;
        public ObservableCollection<MySQLData.Column> listOfSelectTabColumns;
        public ObservableCollection<string> listOfTabulationTabColumns;
        private tableViewModel currentTableViewModel;
        private string nameOfTabitemAssoWithCustomQuery;
        private string directoryPath;
        private bool isErrorLoggingOn = Convert.ToBoolean(ConfigurationManager.AppSettings["isErrorLoggingOn"].ToString());
        private int count = 0;
        public List<SQLBuilder.Clauses.Column> ColListForSelectTab;
        private bool isExpanded = false;
        public static SelectQueryBuilder LatestQueryBuilder { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            Label labelStar = base.GetTemplateChild("PART_Label") as Label;

            connectionString = ConfigurationManager.AppSettings["DefaultDBConn"]; //System.Configuration.ConfigurationSettings.AppSettings["FastDBConn"];
            CurrentDatabaseName = ConfigurationManager.AppSettings["DefaultDatabase"];

            connectionString = connectionString + "Database=" + CurrentDatabaseName + ";";
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            //add handler for CloseTabItem closeTabEvent
            this.AddHandler(CloseableTabItem.CloseTabEvent, new RoutedEventHandler(this.CloseTab));
            //binding Left side panel The tree view for all table

            try
            {
                List<Schema> schemas = MySQLData.DataAccess.ADODataBridge.getSchemaTree(connectionString, CurrentDatabaseName, ConfigurationManager.AppSettings["DerivedTablesPath"]);//DataAccess.GetDatabases();
                MainViewModel viewModel = new MainViewModel(schemas);
                this.MainTreeView.DataContext = viewModel;
                //binding customequery From tab
                //listOfTable = MySQLData.DataAccess.ADODataBridge.getTableStructure(connectionString, CurrentDatabaseName);
                listOfTable = new List<MySQLData.Table>();
                foreach (Schema schema in schemas)
                {
                    listOfTable.AddRange(schema.tables);
                }
                if (listOfTable != null)
                {
                    //this.FromTabUC.cmbFromTable.ItemsSource = Common.ConvertTablesToStringList(listOfTable);//svm.Children;   //zahed

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
            var collView = CollectionViewSource.GetDefaultView(this.tabControlCustomQuery.Items);

            collView.CurrentChanging += this.OnTabItemSelecting;
        }
        private void MenuCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            String command, targetobj;
            command = ((RoutedCommand)e.Command).Name;
            targetobj = ((FrameworkElement)target).Name;
            switch (command)
            {
                case "New":
                    New_Click();
                    break;
                case "Open":
                    try
                    {
                        Open_Click();
                    }
                    catch (Exception ex)
                    {
                        if (isErrorLoggingOn)
                        {
                            LogError.Log_Err("Menu Item Open_Click", ex);
                            DisplayErrorMessage();
                        }
                    }
                    break;
                case "Save":
                    try
                    {
                        Save_Click();
                    }
                    catch (Exception ex)
                    {
                        if (isErrorLoggingOn)
                        {
                            LogError.Log_Err("Menu Item Save_Click", ex);
                            DisplayErrorMessage();
                        }
                    }
                    break;
                case "Close":
                    //Application.Current.ShutdownMode= ShutdownMode.OnExplicitShutdown;
                    Application.Current.Shutdown();
                    //e.Handled = t;
                    break;
            }


        }
        private void MenuCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnTabItemSelecting(Object sender, CurrentChangingEventArgs e)        //uncomment
        {
            ItemCollection tbc = (ItemCollection)sender;
            //validate all tabs before we go to next tab
            ValidateAllTabs();
            TabItem ti = tabControlCustomQuery.SelectedItem as TabItem;
            switch (ti.Name)
            {
                case "FromTabItem":
                    //this.CustomQueryTxTBlk.Text = "From";
                    break;
                case "WhereTabItem":
                    //this.CustomQueryTxTBlk.Text = "Where";
                    break;
                case "SelectTabItem":
                    //this.CustomQueryTxTBlk.Text = "Select";
                    // check to see From tab validated
                    if (this.FromTabUC.isValidated & (this.TabulationTabUC.isValidated & this.TabulationTabUC.isTabulation == false) & (this.CrossTabulationTabUC.isValidated & this.CrossTabulationTabUC.isCrossTabulation == false))
                    {
                        this.SelectTabUC.lstToSelecteColFrom.IsEnabled = true;
                        this.SelectTabUC.lstToSelecteColFrom.ItemsSource = GenerateListOfSelectTabColumns();
                    }
                    break;
                case "TabulationTabItem":

                    if (this.FromTabUC.isValidated)
                    {
                        this.TabulationTabUC.StackPanelTabuLationTabGroupBy.IsEnabled = true;
                        this.TabulationTabUC.StackPanelTabuLationTabSummary.IsEnabled = true;

                        List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = GenerateListOfTabulationTabColumns();

                        //if first drop down is null on Tabulation means all dropdown item source is null
                        if (((TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.Items.Count == 0)
                        {
                            // loading groupby columns
                            for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                            {
                                TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[i];
                                tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                            }
                            // loading summary columns
                            for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count; i++)
                            {
                                TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabUC.StackPanelTabuLationTabSummary.Children[i];
                                ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                            }
                        }
                        else
                        {
                            List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.ItemsSource;

                            IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                            if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                            {
                                // Reloading groupby columns
                                for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                                {
                                    TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[i];
                                    tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                                // Reloading summary columns
                                for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count; i++)
                                {
                                    TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabUC.StackPanelTabuLationTabSummary.Children[i];
                                    ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                            }
                        }
                    }
                    break;
                case "CrossTabulationTabItem":
                    if (this.FromTabUC.isValidated)
                    {
                        this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.IsEnabled = true;
                        this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.IsEnabled = true;
                        this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFristRowSort.IsEnabled = true;
                        this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.IsEnabled = true;
                        List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = GenerateListOfTabulationTabColumns();

                        //if first drop down is null on Cross Tabulation means all dropdown item source is null
                        if (((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.Items.Count == 0)
                        {
                            // loading groupby columns
                            for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                            {
                                CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[i];
                                ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                            }
                            //loading summary first row means (column Name and sort) row
                            this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;
                            // loading summary columns
                            for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                            {
                                CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children[i];
                                cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                            }
                        }
                        else
                        {
                            List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.ItemsSource;

                            IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                            if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                            {
                                // Reloading groupby columns
                                for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                                {
                                    CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[i];
                                    ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                                // Reloading summary first row means (column Name and sort) row
                                this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;

                                // Reloading summary columns
                                for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                                {
                                    CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children[i];
                                    cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                                }
                            }
                        }
                    }
                    break;
                case "ActionsTabItem":
                    if (isAllTabValidated)
                    {
                        queryBuilder = LoadSelectQueryBuilder();
                        if (queryBuilder != null)
                        {
                            queryString = queryBuilder.BuildQuery();
                        }
                        XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                        StringWriter writer = new StringWriter();
                        SerializerObj.Serialize(writer, queryBuilder);
                        this.txtQuery.Text = writer.ToString();
                        this.lblActionTabErrorMessage.Content = "";
                    }
                    else
                    {
                        this.txtQuery.Text = "";
                        this.lblActionTabErrorMessage.Content = "There is an error on one or more tab, please fix an error";
                    }
                    break;
            }

        }

        //zahed
        public bool ValidateAllTabCntrls(ResultViewControl rc)
        {
            int numberOfInvalidTab = 0;
            isAllTabValidated = rc.FromTabCntrl.Validate();
            //this jus temp code untill i figure out how to handle this
            if (isAllTabValidated == false)
            {
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            isAllTabValidated = rc.WhereTabCntrl.Validate();
            //this jus tem code untill i figure out how to handle this
            if (isAllTabValidated == false)
            {
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            //we have to validate both Tabulation Tab & Cross Tabulation
            bool isTabaulationTabValid = rc.TabulationTabCntrl.Validate();
            bool isCrossTabulationTabValid = rc.CrossTabulationTabCntrl.Validate();
            if (isTabaulationTabValid)
            {
                isAllTabValidated = true;
            }
            else
            {
                isAllTabValidated = false;
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            if (isCrossTabulationTabValid)
            {
                isAllTabValidated = true;
            }
            else
            {
                isAllTabValidated = false;
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            //this jus temp code untill i figure out how to handle this
            if (isAllTabValidated)
            {

                //Tabulation Tab is valid
                //check do we have Tabulation on both
                //bool isBothHasTabulation = false;
                if (rc.TabulationTabCntrl.isTabulation & rc.CrossTabulationTabCntrl.isCrossTabulation)
                {
                    rc.TabulationTabCntrl.lblErrorMessage.Content = "You can do only one Tabulation, either do Tabulation Tab or Cross Tabulation Tab, please Clear one of Tabaulation ";
                    rc.CrossTabulationTabCntrl.lblErrorMessage.Content = "You can do only one Tabulation, either do Tabulation Tab or Cross Tabulation Tab, please Clear one of Tabaulation ";
                    // we have tabulation then we don't need select tab
                    rc.SelectTabCntrl.lblErrorMessage.Content = "";
                    //rc.SelectTabCntrl._SelectedColCollection.Clear();
                    isAllTabValidated = false;
                    numberOfInvalidTab = numberOfInvalidTab + 1;
                }
                else
                {
                    if (rc.TabulationTabCntrl.isTabulation)
                    {
                        // we have tabulation then we don't need select tab
                        rc.SelectTabCntrl.lblErrorMessage.Content = "";
                        //rc.SelectTabCntrl._SelectedColCollection.Clear();
                    }
                    else if (rc.CrossTabulationTabCntrl.isCrossTabulation)
                    {
                        // we have tabulation then we don't need select tab
                        rc.SelectTabCntrl.lblErrorMessage.Content = "";
                        //rc.SelectTabCntrl._SelectedColCollection.Clear();
                    }
                    else
                    {
                        isAllTabValidated = rc.SelectTabCntrl.Validate();
                        //if all other tab validated  select  tab datagrid is null
                        // that is user did not visit the select tab and datagrid on select tab is null
                        if (rc.SelectTabCntrl.lstToSelecteColFrom.ItemsSource == null)
                        {
                            isAllTabValidated = false;
                        }
                        if (isAllTabValidated == false)
                        {
                            numberOfInvalidTab = numberOfInvalidTab + 1;
                        }
                    }
                }

            }

            if (numberOfInvalidTab > 0)
            {
                isAllTabValidated = false;
                return false;
            }
            return true;
        }
        //zahed

        public void ValidateAllTabs()
        {
            int numberOfInvalidTab = 0;
            isAllTabValidated = this.FromTabUC.Validate();
            //this jus temp code untill i figure out how to handle this
            if (isAllTabValidated == false)
            {
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            isAllTabValidated = this.WhereTabUC.Validate();
            //this jus tem code untill i figure out how to handle this
            if (isAllTabValidated == false)
            {
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            //we have to validate both Tabulation Tab & Cross Tabulation
            bool isTabaulationTabValid = this.TabulationTabUC.Validate();
            bool isCrossTabulationTabValid = this.CrossTabulationTabUC.Validate();
            if (isTabaulationTabValid)
            {
                isAllTabValidated = true;
            }
            else
            {
                isAllTabValidated = false;
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            if (isCrossTabulationTabValid)
            {
                isAllTabValidated = true;
            }
            else
            {
                isAllTabValidated = false;
                numberOfInvalidTab = numberOfInvalidTab + 1;
            }
            //this jus temp code untill i figure out how to handle this
            if (isAllTabValidated)
            {

                //Tabulation Tab is valid
                //check do we have Tabulation on both
                //bool isBothHasTabulation = false;
                if (this.TabulationTabUC.isTabulation & this.CrossTabulationTabUC.isCrossTabulation)
                {
                    this.TabulationTabUC.lblErrorMessage.Content = "You can do only one Tabulation, either do Tabulation Tab or Cross Tabulation Tab, please Clear one of Tabaulation ";
                    this.CrossTabulationTabUC.lblErrorMessage.Content = "You can do only one Tabulation, either do Tabulation Tab or Cross Tabulation Tab, please Clear one of Tabaulation ";
                    // we have tabulation then we don't need select tab
                    this.SelectTabUC.lblErrorMessage.Content = "";
                    this.SelectTabUC._SelectedColCollection.Clear();
                    isAllTabValidated = false;
                    numberOfInvalidTab = numberOfInvalidTab + 1;
                }
                else
                {
                    if (this.TabulationTabUC.isTabulation)
                    {
                        // we have tabulation then we don't need select tab
                        this.SelectTabUC.lblErrorMessage.Content = "";
                        this.SelectTabUC._SelectedColCollection.Clear();
                    }
                    else if (this.CrossTabulationTabUC.isCrossTabulation)
                    {
                        // we have tabulation then we don't need select tab
                        this.SelectTabUC.lblErrorMessage.Content = "";
                        this.SelectTabUC._SelectedColCollection.Clear();
                    }
                    else
                    {
                        isAllTabValidated = this.SelectTabUC.Validate();
                        //if all other tab validated  select  tab datagrid is null
                        // that is user did not visit the select tab and datagrid on select tab is null
                        if (this.SelectTabUC.lstToSelecteColFrom.ItemsSource == null)
                        {
                            isAllTabValidated = false;
                        }
                        if (isAllTabValidated == false)
                        {
                            numberOfInvalidTab = numberOfInvalidTab + 1;
                        }
                    }
                }

            }

            if (numberOfInvalidTab > 0)
            {
                isAllTabValidated = false;
            }

        }
        private void CloseTab(object source, RoutedEventArgs args)
        {
            CloseableTabItem tabItem = args.Source as CloseableTabItem;
            if (tabItem != null)
            {
                if (tabItem.Name != "tabItem1")
                {

                    CheckQueryNeededToBeSaved(tabItem, "CloseTab");
                }
            }

        }
        private void SaveNewQuery(CloseableTabItem tabItem, SelectQueryBuilder QueryBulder)
        {
            //prompt the user
            // Configure the message box to be displayed
            string messageBoxText = "Do you want to save current query?";
            string caption = "Current Query";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            // Process message box results
            switch (result)
            {

                case MessageBoxResult.Yes:
                    // User pressed Yes button
                    //save save query                     
                    ResultViewControl rc = (ResultViewControl)tabItem.Content;
                    //ResultViewModel rv = (ResultViewModel)rc.DataContext;
                    /******************/
                    ResultViewModel rv = rc.result;
                    /******************/
                    string FileName1 = System.String.Empty;
                    if (rv.isNew == false)
                    {
                        FileName1 = rv.directoryPath + tabItem.Header.ToString() + ".xml";
                    }
                    else
                    {
                        FileName1 = OpenSaveDialog(tabItem.Name);
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
                        this.CustomQueryTxTBlk.Text = "Custom query for " + tabItemHeader;
                        //we saved the query change isModified to false
                        rv.isModified = false;
                        tabItem.labelStar.Content = "";
                        MessageBox.Show("query saved successfully");

                        /**************************/
                        rc.result.directoryPath = FileName1.Replace(splitedArray[splitedArray.Length - 1], "");
                        /**************************/

                        /******************************/
                        TabControl tabControl = tabItem.Parent as TabControl;
                        if (tabControl.Items.Count == 0)
                        {
                            this.Logo.Visibility = Visibility.Visible;
                            this.LogoTxt.Visibility = Visibility.Visible;
                            this.tabControl1.Visibility = Visibility.Hidden;
                        }
                        /******************************/
                    }

                    break;
                case MessageBoxResult.No:
                    // User pressed No button
                    TabControl tabControl1 = tabItem.Parent as TabControl; //zahed
                    if (tabControl1 != null)
                        tabControl1.Items.Remove(tabItem);
                    /******************************/
                    if (tabControl1.Items.Count == 0)
                    {
                        this.Logo.Visibility = Visibility.Visible;
                        this.LogoTxt.Visibility = Visibility.Visible;
                        this.tabControl1.Visibility = Visibility.Hidden;
                    }
                    /******************************/

                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    // ...
                    break;
            }
        }
        public void CheckQueryNeededToBeSaved(CloseableTabItem tabItem, string caller) //zahed
        {
            ResultViewControl rc = (ResultViewControl)tabItem.Content;

            //ResultViewModel rv = (ResultViewModel)rc.DataContext;               //moved this from here from inside if
            ResultViewModel rv = (ResultViewModel)rc.result;//mainGV.DataContext;
            if (rc != null && currentTableViewModel != null && rv != null)     //zahed
            {
                //ResultViewModel rv = (ResultViewModel)rc.DataContext;         
                if (rv.isModified)
                {
                    if (rv.isNew == false)
                    {
                        //over write
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
                                    this.CustomQueryTxTBlk.Text = "Custom query for " + tabItem.Header;
                                    //we saved the query change isModified to false
                                    rv.isModified = false;
                                    tabItem.labelStar.Content = "";
                                    MessageBox.Show("query saved successfully");
                                }
                                break;
                            case "SaveAs":
                                //save save as the query
                                string FileName1 = OpenSaveDialog(tabItem.Name);

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
                                    this.CustomQueryTxTBlk.Text = "Custom query for " + tabItemHeader;
                                    //we saved the query change isModified to false
                                    rv.isModified = false;
                                    tabItem.labelStar.Content = "";
                                    MessageBox.Show("query saved successfully");
                                }
                                break;
                            case "CloseTab":
                                SaveNewQuery(tabItem, rv.QueryBulder);
                                break;

                        }

                    }
                    else
                    {

                        //it is new ****

                        switch (caller)
                        {

                            case "SaveAs":
                                if (rv.TotalIRows != 0)
                                {
                                    //save save as the query
                                    string FileName = OpenSaveDialog(tabItem.Name);

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
                                        this.CustomQueryTxTBlk.Text = "Custom query for " + tabItemHeader;
                                        //we saved the query change isModified to false
                                        rv.isModified = false;
                                        tabItem.labelStar.Content = "";
                                        MessageBox.Show("query saved successfully");
                                    }
                                }
                                break;

                            case "Save":
                                if (rv.TotalIRows != 0)
                                {
                                    SaveNewQuery(tabItem, rv.QueryBulder);
                                }
                                break;
                            case "CloseTab":
                                if (rv.TotalIRows != 0)
                                {
                                    SaveNewQuery(tabItem, rv.QueryBulder);
                                }
                                else
                                {
                                    TabControl tabControl = tabItem.Parent as TabControl;
                                    if (tabControl != null)
                                        tabControl.Items.Remove(tabItem);
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

                            /******************************/
                            if (tabControl.Items.Count == 0)
                            {
                                this.Logo.Visibility = Visibility.Visible;
                                this.LogoTxt.Visibility = Visibility.Visible;
                                this.tabControl1.Visibility = Visibility.Hidden;
                            }
                            /******************************/
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

                    /******************************/
                    if (tabControl.Items.Count == 0)
                    {
                        this.Logo.Visibility = Visibility.Visible;
                        this.LogoTxt.Visibility = Visibility.Visible;
                        this.tabControl1.Visibility = Visibility.Hidden;
                    }
                    /******************************/
                }
            }
        }

        private string GetTabHeaderName(string tableName, int tabNumber)
        {
            // Build Tab Header Name by splitting table name on underscores
            string headerName = "";
            string[] words = tableName.Split('_');

            foreach(string word in words)
            {
                headerName = headerName + word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length-1).ToLower() + " ";
            }
            headerName = headerName + Convert.ToString(tabNumber);
            return headerName;
        }

        private void MainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tv = (TreeView)sender;
            count = 0;                      //zahed          

            if (tv.SelectedItem.GetType().Name == "tableViewModel")
            {

                tableViewModel tvm = (tableViewModel)tv.SelectedItem;

                currentTableViewModel = tvm;
                CurrentDatabaseName = tvm._ParentSchemaName;
                // display path at the top of the screen
                //txtBlockDisplayPath.Text = "All Databases/" + ((schemaViewModel)tvm.Parent).schemaName + "/" + tvm.tableName;
                int tabNumber = this.tabControl1.Items.Count + 1;
                CloseableTabItem tabItem = new CloseableTabItem();
                tabItem.Name = tvm.tableName + tabNumber;
                nameOfTabitemAssoWithCustomQuery = tabItem.Name;                
                tabItem.Header = GetTabHeaderName(tvm.tableName, tabNumber);                                

                //add ResultViewerControl
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Control.ResultViewControl resultControl = new Control.ResultViewControl(tvm, true);

                    ResultViewModel rv = (ResultViewModel)resultControl.DataContext;
                    if (rv.Results.Count != 0)
                    {
                        rv.isNew = true;
                        rv.isModified = true;
                        tabItem.Part_Label_Content = "*";
                    }

                    tabItem.Content = resultControl;
                    /******************************/
                    this.Logo.Visibility = Visibility.Hidden;
                    this.LogoTxt.Visibility = Visibility.Hidden;
                    this.tabControl1.Visibility = Visibility.Visible;
                    /******************************/
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    if (isErrorLoggingOn)
                    {
                        LogError.Log_Err("MainTreeView_SelectedItemChanged", ex);
                        DisplayErrorMessage();
                    }
                }
                catch (Exception ex)
                {
                    if (isErrorLoggingOn)
                    {
                        LogError.Log_Err("MainTreeView_SelectedItemChanged", ex);
                        DisplayErrorMessage();
                    }
                }
                Mouse.OverrideCursor = null;
                //add tab to tabcontrol
                this.tabControl1.Items.Add(tabItem);
                //Get focus on new tab
                ((TabItem)(this.tabControl1.Items[tabNumber - 1])).Focus();


                this.adornedControl.IsAdornerVisible = false;
                //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;



            }

        }
        public bool FindElement(MySQLData.Table tb)
        {
            return (tb.name == ElementNameValue);
        }


        private void BtnPin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.TreeViewDocPanel.Width = 0;
            TreeViewHideStackPanel.Width = 20;
        }

        private void BtnHideTreView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.TreeViewDocPanel.Width = 199;
            TreeViewHideStackPanel.Width = 0;
            this.adornedControl.IsAdornerVisible = false;
            //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
        }

        private void RowMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (this.tabControl1.Items.Count > 1)//&& this.tabItem1.IsFocused == false
            {
                //this.adornedControl.IsAdornerVisible = true;
                this.TreeViewDocPanel.Width = 0;
                TreeViewHideStackPanel.Width = 20;

                //this.MainTreeView.IsEnabled = false;
                MenuItem rowMI = (MenuItem)sender;
                rowMI.IsEnabled =
                false;
                TabItem ti = tabControl1.SelectedItem as TabItem;
                foreach (TabItem tbi in tabControl1.Items)
                {
                    if (tbi != ti)
                    {
                        tbi.IsEnabled =
                        false;
                    }
                }
            }

        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem rowMI = (MenuItem)this.MainMenu.Items[0];
            rowMI.IsEnabled =
            true;

            this.adornedControl.IsAdornerVisible = false;
            //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
            this.TreeViewDocPanel.Width = 199;
            TreeViewHideStackPanel.Width = 0;
            this.MainTreeView.IsEnabled = true;
            foreach (TabItem tbi in tabControl1.Items)
            {
                tbi.IsEnabled =
                true;
            }

        }
        private void reSizeButton_Click(object sender, RoutedEventArgs e)
        {
            MiniMizeCustomeQyeryWindow();
        }
        private void MiniMizeCustomeQyeryWindow()
        {
            if (DocPanelCustomQuery.HorizontalAlignment == HorizontalAlignment.Right)
            {
                DocPanelCustomQuery.HorizontalAlignment =
                HorizontalAlignment.Stretch;
                DocPanelCustomQuery.Margin =
                new Thickness(0, 60, 0, 0);
                DocPanelCustomQuery.Width =
                Double.NaN;
                //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                DocPanelCustomQuery.Margin =
                new Thickness(0, 0, 0, 0);
                DocPanelCustomQuery.Width = 500;
                DocPanelCustomQuery.HorizontalAlignment =
                HorizontalAlignment.Right;
                //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void tabControlCustomQuery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //zahed
        public SelectQueryBuilder LoadSelectQueryBuilderNew(ResultViewControl rc)     //made public zahed 
        {
            SelectQueryBuilder query = new SelectQueryBuilder();

            bool isSelecteTabColumnsOrGroupbyColumns = false;
            if (rc.SelectTabCntrl.lstSelectedCol.ItemsSource != null)
            {
                //****Geting select columns*****
                ObservableCollection<SQLBuilder.Clauses.Column> ListOfSelectColumn = ((ObservableCollection<SQLBuilder.Clauses.Column>)rc.SelectTabCntrl.lstSelectedCol.ItemsSource);
                query.SelectedColumns = ListOfSelectColumn.ToList();
                //foreach (SQLBuilder.Clauses.Column Col in ListOfSelectColumn)
                //{
                //    SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                //    column.Name = Col.Name;
                //    column.AliasName = Col.AliasName;
                //    column.Format = Col.Format;
                //    query.SelectColumn(column);
                //}
                isSelecteTabColumnsOrGroupbyColumns = true;
            }
            if (rc.TabulationTabCntrl.isTabulation)
            {
                //group by column
                for (int i = 0; i < rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                    if (tg.cmbTabulationTabGroupByColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();

                        column.Name = tg.cmbTabulationTabGroupByColumnsName.Text;
                        column.Format = tg.txtTabulationTabGroupByColFormat.Text;
                        column.AliasName = tg.txtTabulationTabGroupByAlias.Text;

                        query.AddGroupBy(column);
                        if (tg.cmbTabulationSort.SelectedIndex != -1)
                        {
                            query.AddOrderBy(tg.cmbTabulationTabGroupByColumnsName.Text, GetSortingEnum(tg.cmbTabulationSort.Text));
                        }
                    }
                }
                //summarize column
                for (int i = 0; i < rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                {
                    TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                    if (ts.cmbTabulationTabSummaryColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        column.Name = ts.cmbTabulationTypeOfSummary.Text + "(" + ts.cmbTabulationTabSummaryColumnsName.Text + ")";
                        column.Format = ts.txtTabulationTabSummaryColFormat.Text;
                        column.AliasName = ts.txtTabulationTabSummaryAlias.Text;
                        query.AddSummarize(column);
                    }
                }

                isSelecteTabColumnsOrGroupbyColumns = true;
            }
            else if (rc.CrossTabulationTabCntrl.isCrossTabulation)
            {
                //group by column
                for (int i = 0; i < rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                    if (ctg.cmbCrossTabulationTabGroupByColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        //modified on  11/18/11
                        //column.Name = ctg.cmbCrossTabulationTabGroupByColumnsName.Text;
                        column.Name = ctg.cmbCrossTabulationTabGroupByColumnsName.Text;//((SQLBuilder.Clauses.Column)ctg.cmbCrossTabulationTabGroupByColumnsName.SelectionBoxItem).Name;
                        column.Format = ctg.txtCrossTabulationTabGroupByColFormat.Text; //((SQLBuilder.Clauses.Column)ctg.cmbCrossTabulationTabGroupByColumnsName.SelectionBoxItem).Format;
                        column.AliasName = ctg.txtCrossTabulationTabGroupByAlias.Text;
                        //query.SelectColumn(column);
                        query.AddGroupBy(column);
                        if (ctg.cmbCrossTabulationSort.SelectedIndex != -1)
                        {
                            // SQLBuilder.Clauses.OrderByClause orderBy = new  SQLBuilder.Clauses.OrderByClause(tg.cmbTabulationTabGroupByColumnsName.Text,GetSortingEnum(tg.cmbTabulationSort.Text));
                            // modified on 11/18/11
                            query.AddOrderBy(ctg.cmbCrossTabulationTabGroupByColumnsName.Text, GetSortingEnum(ctg.cmbCrossTabulationSort.Text));
                            //query.AddOrderBy(((SQLBuilder.Clauses.Column)ctg.cmbCrossTabulationTabGroupByColumnsName.SelectionBoxItem).Name, GetSortingEnum(ctg.cmbCrossTabulationSort.Text));
                        }
                    }
                }
                //summarize column
                for (int i = 0; i < rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelSummaryControl ts = (CrossTabulationTabStackPanelSummaryControl)rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                    if (ts.cmbCrossTabulationTabSummaryColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        //modified on 11/18/11
                        column.Name = ts.cmbCrossTabulationTypeOfSummary.Text + "(" + ts.cmbCrossTabulationTabSummaryColumnsName.Text + ")";
                        column.Format = ts.txtCrossTabulationTabSummaryColFormat.Text;
                        //column.Name = ts.cmbCrossTabulationTypeOfSummary.Text + "(" + ((SQLBuilder.Clauses.Column)ts.cmbCrossTabulationTabSummaryColumnsName.SelectionBoxItem).Name + ")";
                        //column.Format = ((SQLBuilder.Clauses.Column)ts.cmbCrossTabulationTabSummaryColumnsName.SelectionBoxItem).Format;
                        column.AliasName = ts.txtCrossTabulationTabSummaryAlias.Text;
                        //query.SelectColumn(column);
                        query.AddSummarize(column);
                    }
                }
                //summary first row
                SQLBuilder.Clauses.Column column1 = new SQLBuilder.Clauses.Column();
                //modified on 11/18/11
                column1.Name = rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.Text;
                //column1.Name =  ((SQLBuilder.Clauses.Column)this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectionBoxItem).Name;
                if (rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex == -1)
                {
                    query.CrossTabClause = new SQLBuilder.Clauses.CrossTabulationClause(column1);
                    isSelecteTabColumnsOrGroupbyColumns = true;
                }
                else
                {
                    query.CrossTabClause = new SQLBuilder.Clauses.CrossTabulationClause(column1, GetSortingEnum(rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.Text));
                    isSelecteTabColumnsOrGroupbyColumns = true;
                }
            }

            if (isSelecteTabColumnsOrGroupbyColumns)
            {
                //Geting from table
                SQLBuilder.Clauses.Table fromTable = null;
                MySQLData.Table table = this.listOfTable[rc.FromTabCntrl.cmbFromTable.SelectedIndex];
                if (table is MySQLData.DerivedTable)
                {
                    fromTable = new SQLBuilder.Clauses.DerivedTable((MySQLData.DerivedTable)table, rc.FromTabCntrl.txtFromAlias.Text);
                }
                else if (table is MySQLData.Table)
                {
                    fromTable = new SQLBuilder.Clauses.Table(table, rc.FromTabCntrl.txtFromAlias.Text);
                }

                //fromTable.Name = rc.FromTabCntrl.cmbFromTable.Text;
                //fromTable.AliasName = rc.FromTabCntrl.txtFromAlias.Text;
                query.SelectFromTable(fromTable);

                //Geting join table and join colums
                for (int i = 0; i < rc.FromTabCntrl.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs = (FromTabStackPanelControl)rc.FromTabCntrl.StackPanelFromTab.Children[i];
                    SQLBuilder.Clauses.Table joinTable = null;

                    MySQLData.Table jTable = null;
                    if (fs.cmbFromTabJoinTable.SelectedIndex != -1)
                    {
                        jTable = this.listOfTable[fs.cmbFromTabJoinTable.SelectedIndex];

                        if (jTable is MySQLData.DerivedTable)
                        {
                            joinTable = new SQLBuilder.Clauses.DerivedTable((MySQLData.DerivedTable)jTable, fs.txtJoinTableAlias.Text);
                        }
                        else if (jTable is MySQLData.Table)
                        {
                            joinTable = new SQLBuilder.Clauses.Table(jTable, fs.txtJoinTableAlias.Text);
                        }

                        //joinTable.Name = fs.cmbFromTabJoinTable.Text;
                        //joinTable.AliasName = fs.txtJoinTableAlias.Text;
                        SQLBuilder.Clauses.JoinClause joinClause = query.AddJoin(GetJoinType(fs.cmbFromTabJoinType.Text), fromTable, fs.cmbFromTabFromColumns.Text, GetComparisonOpreator(fs.cmbFromTabQueryOpretor.Text), joinTable, fs.cmbFromTabJoinColumns.Text);
                        for (int condIndex = 0; condIndex < fs.StackPanelFromTabMore.Children.Count; condIndex++)
                        {
                            FromTabStackPanelControlMore fsCondition = (FromTabStackPanelControlMore)fs.StackPanelFromTabMore.Children[condIndex];

                            joinClause.addJoinCondition(GetLogicalOpreator(fsCondition.cmbFromTabFromANDOR.Text), fromTable, fsCondition.cmbFromTabFromColumns.Text, GetComparisonOpreator(fsCondition.cmbFromTabQueryOpretor.Text), joinTable, fsCondition.cmbFromTabJoinColumns.Text);
                        }
                    }
                }
                //******Geting Wheree Clause********
                for (int i = 0; i < rc.WhereTabCntrl.StackPanelWhereTab.Children.Count; i++)
                {
                    string controlType = rc.WhereTabCntrl.StackPanelWhereTab.Children[i].GetType().ToString();
                    switch (controlType)
                    {
                        case "FastDB.Control.WhereTabRegularConditionControl":
                            WhereTabRegularConditionControl ws = (WhereTabRegularConditionControl)rc.WhereTabCntrl.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, ws.cmbWhereTabLeftSideColumns.Text, GetComparisonOpreator(ws.cmbWhereTabQueryOpretor.Text), ws.cmbWhereTabRightSideColumns.Text, Convert.ToInt32(ws.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(ws.cmbWhereTabQueryAndOr.Text), ws.cmbWhereTabLeftSideColumns.Text, GetComparisonOpreator(ws.cmbWhereTabQueryOpretor.Text), ws.cmbWhereTabRightSideColumns.Text, Convert.ToInt32(ws.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                        case "FastDB.Control.WhereTabBetweenConditionControl":
                            WhereTabBetweenConditionControl wsb = (WhereTabBetweenConditionControl)rc.WhereTabCntrl.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, wsb.cmbWhereTabBetweenColumns.Text, wsb.txtBetweenLeftValue.Text, wsb.txtBetweenRightValue.Text, Convert.ToInt32(wsb.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(wsb.cmbWhereTabQueryAndOr.Text), wsb.cmbWhereTabBetweenColumns.Text, wsb.txtBetweenLeftValue.Text, wsb.txtBetweenRightValue.Text, Convert.ToInt32(wsb.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                        case "FastDB.Control.WhereTabInNotInConditionControl":
                            WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)rc.WhereTabCntrl.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, wInNotIn.cmbWhereTabInNotInColumns.Text, GetComparisonOpreator(wInNotIn.lblInNotIn.Content.ToString()), wInNotIn.txtInNotInValue.Text, Convert.ToInt32(wInNotIn.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(wInNotIn.cmbWhereTabQueryAndOr.Text), wInNotIn.cmbWhereTabInNotInColumns.Text, GetComparisonOpreator(wInNotIn.lblInNotIn.Content.ToString()), wInNotIn.txtInNotInValue.Text, Convert.ToInt32(wInNotIn.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                        case "FastDB.Control.WhereTabNullNotNullConditionControl":
                            WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)rc.WhereTabCntrl.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, wNullNotNull.cmbWhereTabNullNotNullColumns.Text, GetComparisonOpreator(wNullNotNull.lblNullNotNull.Content.ToString()), null, Convert.ToInt32(wNullNotNull.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(wNullNotNull.cmbWhereTabQueryAndOr.Text), wNullNotNull.cmbWhereTabNullNotNullColumns.Text, GetComparisonOpreator(wNullNotNull.lblNullNotNull.Content.ToString()), null, Convert.ToInt32(wNullNotNull.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                    }
                }
            }
            return query;
        }

        public SelectQueryBuilder LoadSelectQueryBuilder()     //made public zahed 
        {
            SelectQueryBuilder query = new SelectQueryBuilder();

            bool isSelecteTabColumnsOrGroupbyColumns = false;
            if (this.SelectTabUC.lstSelectedCol.ItemsSource != null)
            {
                //****Geting select columns*****
                //List<SQLBuilder.Clauses.Column> ListOfSelectColumn = ((List<SQLBuilder.Clauses.Column>)this.SelectTabUC.lstSelectedCol.ItemsSource);
                ObservableCollection<SQLBuilder.Clauses.Column> ListOfSelectColumn = ((ObservableCollection<SQLBuilder.Clauses.Column>)this.SelectTabUC.lstSelectedCol.ItemsSource);
                foreach (SQLBuilder.Clauses.Column Col in ListOfSelectColumn)
                {
                    SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                    column.Name = Col.Name;
                    column.AliasName = Col.AliasName;
                    column.Format = Col.Format;
                    query.SelectColumn(column);
                }
                isSelecteTabColumnsOrGroupbyColumns = true;
            }
            if (this.TabulationTabUC.isTabulation)
            {
                //group by column
                for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[i];
                    if (tg.cmbTabulationTabGroupByColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        //modified on 11/18/11
                        column.Name = tg.cmbTabulationTabGroupByColumnsName.Text;
                        column.Format = tg.txtTabulationTabGroupByColFormat.Text;
                        column.AliasName = tg.txtTabulationTabGroupByAlias.Text;
                        //query.SelectColumn(column);
                        query.AddGroupBy(column);
                        if (tg.cmbTabulationSort.SelectedIndex != -1)
                        {
                            query.AddOrderBy(tg.cmbTabulationTabGroupByColumnsName.Text, GetSortingEnum(tg.cmbTabulationSort.Text));
                        }
                    }
                }
                //summarize column
                for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count; i++)
                {
                    TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabUC.StackPanelTabuLationTabSummary.Children[i];
                    if (ts.cmbTabulationTabSummaryColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        column.Name = ts.cmbTabulationTypeOfSummary.Text + "(" + ts.cmbTabulationTabSummaryColumnsName.Text + ")";
                        column.Format = ts.txtTabulationTabSummaryColFormat.Text;
                        column.AliasName = ts.txtTabulationTabSummaryAlias.Text;
                        query.AddSummarize(column);
                    }
                }

                isSelecteTabColumnsOrGroupbyColumns = true;
            }
            else if (this.CrossTabulationTabUC.isCrossTabulation)
            {
                //group by column
                for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[i];
                    if (ctg.cmbCrossTabulationTabGroupByColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        column.Name = ctg.cmbCrossTabulationTabGroupByColumnsName.Text;
                        column.Format = ctg.txtCrossTabulationTabGroupByColFormat.Text;
                        column.AliasName = ctg.txtCrossTabulationTabGroupByAlias.Text;
                        query.AddGroupBy(column);
                        if (ctg.cmbCrossTabulationSort.SelectedIndex != -1)
                        {
                            query.AddOrderBy(ctg.cmbCrossTabulationTabGroupByColumnsName.Text, GetSortingEnum(ctg.cmbCrossTabulationSort.Text));
                        }
                    }
                }
                //summarize column
                for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelSummaryControl ts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children[i];
                    if (ts.cmbCrossTabulationTabSummaryColumnsName.SelectedIndex != -1)
                    {
                        SQLBuilder.Clauses.Column column = new SQLBuilder.Clauses.Column();
                        column.Name = ts.cmbCrossTabulationTypeOfSummary.Text + "(" + ts.cmbCrossTabulationTabSummaryColumnsName.Text + ")";
                        column.Format = ts.txtCrossTabulationTabSummaryColFormat.Text;
                        column.AliasName = ts.txtCrossTabulationTabSummaryAlias.Text;
                        query.AddSummarize(column);
                    }
                }
                //summary first row
                SQLBuilder.Clauses.Column column1 = new SQLBuilder.Clauses.Column();
                column1.Name = this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.Text;
                if (this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex == -1)
                {
                    query.CrossTabClause = new SQLBuilder.Clauses.CrossTabulationClause(column1);
                    isSelecteTabColumnsOrGroupbyColumns = true;
                }
                else
                {
                    query.CrossTabClause = new SQLBuilder.Clauses.CrossTabulationClause(column1, GetSortingEnum(this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFristRowSort.Text));
                    isSelecteTabColumnsOrGroupbyColumns = true;
                }
            }

            if (isSelecteTabColumnsOrGroupbyColumns)
            {
                //Geting from table
                SQLBuilder.Clauses.Table fromTable = new SQLBuilder.Clauses.Table();
                fromTable.Name = this.FromTabUC.cmbFromTable.Text;
                fromTable.AliasName = this.FromTabUC.txtFromAlias.Text;
                query.SelectFromTable(fromTable);

                for (int i = 0; i < this.FromTabUC.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs = (FromTabStackPanelControl)this.FromTabUC.StackPanelFromTab.Children[i];
                    SQLBuilder.Clauses.Table joinTable = new SQLBuilder.Clauses.Table();
                    joinTable.Name = fs.cmbFromTabJoinTable.Text;
                    joinTable.AliasName = fs.txtJoinTableAlias.Text;
                    query.AddJoin(GetJoinType(fs.cmbFromTabJoinType.Text), joinTable, fs.cmbFromTabJoinColumns.Text, GetComparisonOpreator(fs.cmbFromTabQueryOpretor.Text), fromTable, fs.cmbFromTabFromColumns.Text);
                }
                //******Geting Wheree Clause********
                for (int i = 0; i < this.WhereTabUC.StackPanelWhereTab.Children.Count; i++)
                {
                    string controlType = this.WhereTabUC.StackPanelWhereTab.Children[i].GetType().ToString();
                    switch (controlType)
                    {
                        case "FastDB.Control.WhereTabRegularConditionControl":
                            WhereTabRegularConditionControl ws = (WhereTabRegularConditionControl)this.WhereTabUC.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, ws.cmbWhereTabLeftSideColumns.Text, GetComparisonOpreator(ws.cmbWhereTabQueryOpretor.Text), ws.cmbWhereTabRightSideColumns.Text, Convert.ToInt32(ws.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(ws.cmbWhereTabQueryAndOr.Text), ws.cmbWhereTabLeftSideColumns.Text, GetComparisonOpreator(ws.cmbWhereTabQueryOpretor.Text), ws.cmbWhereTabRightSideColumns.Text, Convert.ToInt32(ws.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                        case "FastDB.Control.WhereTabBetweenConditionControl":
                            WhereTabBetweenConditionControl wsb = (WhereTabBetweenConditionControl)this.WhereTabUC.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, wsb.cmbWhereTabBetweenColumns.Text, wsb.txtBetweenLeftValue.Text, wsb.txtBetweenRightValue.Text, Convert.ToInt32(wsb.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(wsb.cmbWhereTabQueryAndOr.Text), wsb.cmbWhereTabBetweenColumns.Text, wsb.txtBetweenLeftValue.Text, wsb.txtBetweenRightValue.Text, Convert.ToInt32(wsb.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                        case "FastDB.Control.WhereTabInNotInConditionControl":
                            WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)this.WhereTabUC.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, wInNotIn.cmbWhereTabInNotInColumns.Text, GetComparisonOpreator(wInNotIn.lblInNotIn.Content.ToString()), wInNotIn.txtInNotInValue.Text, Convert.ToInt32(wInNotIn.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(wInNotIn.cmbWhereTabQueryAndOr.Text), wInNotIn.cmbWhereTabInNotInColumns.Text, GetComparisonOpreator(wInNotIn.lblInNotIn.Content.ToString()), wInNotIn.txtInNotInValue.Text, Convert.ToInt32(wInNotIn.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                        case "FastDB.Control.WhereTabNullNotNullConditionControl":
                            WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)this.WhereTabUC.StackPanelWhereTab.Children[i];
                            if (i == 0)
                            {
                                query.AddWhere(SQLBuilder.Enums.LogicOperator.None, wNullNotNull.cmbWhereTabNullNotNullColumns.Text, GetComparisonOpreator(wNullNotNull.lblNullNotNull.Content.ToString()), null, Convert.ToInt32(wNullNotNull.cmbWhereTabQueryLevel.Text));
                            }
                            else
                            {
                                query.AddWhere(GetLogicalOpreator(wNullNotNull.cmbWhereTabQueryAndOr.Text), wNullNotNull.cmbWhereTabNullNotNullColumns.Text, GetComparisonOpreator(wNullNotNull.lblNullNotNull.Content.ToString()), null, Convert.ToInt32(wNullNotNull.cmbWhereTabQueryLevel.Text));
                            }
                            break;
                    }
                }
            }
            return query;
        }

        private SQLBuilder.Enums.JoinType GetJoinType(string value)
        {
            SQLBuilder.Enums.JoinType joinType = SQLBuilder.Enums.JoinType.InnerJoin;
            switch (value)
            {
                case "Equals":
                    joinType = SQLBuilder.Enums.JoinType.InnerJoin;
                    break;
                case "LeftJoin":
                    joinType = SQLBuilder.Enums.JoinType.LeftJoin;
                    break;
                case "RightJoin":
                    joinType = SQLBuilder.Enums.JoinType.RightJoin;
                    break;
                //case "OuterJoin":
                //    joinType = SQLBuilder.Enums.JoinType.OuterJoin;
                //    break;
            }
            return joinType;
        }

        private SQLBuilder.Enums.LogicOperator GetLogicalOpreator(string value)
        {
            SQLBuilder.Enums.LogicOperator logicalOpreator;
            if (value == "And")
            {
                logicalOpreator = SQLBuilder.Enums.LogicOperator.And;
            }
            else
            {
                logicalOpreator = SQLBuilder.Enums.LogicOperator.Or;
            }
            return logicalOpreator;
        }

        private SQLBuilder.Enums.Comparison GetComparisonOpreator(string value)
        {
            SQLBuilder.Enums.Comparison comparisonOpreator = SQLBuilder.Enums.Comparison.Equals;
            switch (value)
            {
                case "Equals":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.Equals;
                    break;
                case "NotEquals":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.NotEquals;
                    break;
                case "Like":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.Like;
                    break;
                case "NotLike":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.NotLike;
                    break;
                case "GreaterThan":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.GreaterThan;
                    break;
                case "GreaterOrEquals":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.GreaterOrEquals;
                    break;
                case "LessThan":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.LessThan;
                    break;
                case "LessOrEquals":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.LessOrEquals;
                    break;
                case "    in":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.In;
                    break;
                case "not in":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.NotIn;
                    break;
                case "null":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.Equals;
                    break;
                case "not null":
                    comparisonOpreator = SQLBuilder.Enums.Comparison.NotEquals;
                    break;
            }
            return comparisonOpreator;
        }
        private SQLBuilder.Enums.Sorting GetSortingEnum(string value)
        {
            SQLBuilder.Enums.Sorting sorting;//= SQLBuilder.Enums.Sorting.Ascending;
            if (value == "Ascending")
            {
                sorting = SQLBuilder.Enums.Sorting.Ascending;
            }
            else
            {
                sorting = SQLBuilder.Enums.Sorting.Descending;
            }
            return sorting;
        }

        public bool CompairQueryBuilder(SelectQueryBuilder queryBuilder1, SelectQueryBuilder queryBuilder2) //made public
        {

            bool isDifferenct = true;
            //seralizeing old querybuider
            XmlSerializer SerializerObj1 = new XmlSerializer(typeof(SelectQueryBuilder));
            StringWriter writer1 = new StringWriter();
            SerializerObj1.Serialize(writer1, queryBuilder1);

            //seralizeing New querybuider
            XmlSerializer SerializerObj2 = new XmlSerializer(typeof(SelectQueryBuilder));
            StringWriter writer2 = new StringWriter();
            SerializerObj2.Serialize(writer2, queryBuilder2);

            if (writer1.ToString() == writer2.ToString())
            {
                isDifferenct = false;
            }
            return isDifferenct;
        }

        private void btnRunQuery_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = DateTime.Now;
            if (isAllTabValidated)
            {

                if (queryString != System.String.Empty)
                {
                    if (queryBuilder != null)
                    {
                        CloseableTabItem tabItem = (CloseableTabItem)this.tabControl1.SelectedItem;
                        if (tabItem.Content == null)
                        {
                            //it is new
                            Mouse.OverrideCursor = Cursors.Wait;
                            try
                            {
                                Control.ResultViewControl resultControl = new Control.ResultViewControl(queryBuilder, CurrentDatabaseName);
                                ResultViewModel rv = (ResultViewModel)resultControl.DataContext;
                                //set isNew and isModified 
                                rv.isNew = true;
                                rv.isModified = true;
                                tabItem.labelStar.Content = "*";
                                tabItem.Content = resultControl;
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                if (isErrorLoggingOn)
                                {
                                    LogError.Log_Err("btnRunQuery_Click", ex);
                                    DisplayErrorMessage();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (isErrorLoggingOn)
                                {
                                    LogError.Log_Err("btnRunQuery_Click", ex);
                                    DisplayErrorMessage();
                                }
                            }
                            Mouse.OverrideCursor = null;
                        }
                        else
                        {
                            //it canbe old or it can be new
                            Control.ResultViewControl oldResultControl = (Control.ResultViewControl)tabItem.Content;
                            ResultViewModel rv = (ResultViewModel)oldResultControl.DataContext;
                            if (rv.isNew)
                            {
                                // we still let user run the query, query still new and modified= true
                                Mouse.OverrideCursor = Cursors.Wait;
                                try
                                {
                                    Control.ResultViewControl resultControl = new Control.ResultViewControl(queryBuilder, CurrentDatabaseName);
                                    ResultViewModel rv2 = (ResultViewModel)resultControl.DataContext;
                                    rv2.isNew = true;
                                    rv2.isModified = true;
                                    tabItem.Content = resultControl;
                                    tabItem.labelStar.Content = "*";
                                }
                                catch (MySql.Data.MySqlClient.MySqlException ex)
                                {
                                    if (isErrorLoggingOn)
                                    {
                                        LogError.Log_Err("btnRunQuery_Click", ex);
                                        DisplayErrorMessage();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (isErrorLoggingOn)
                                    {
                                        LogError.Log_Err("btnRunQuery_Click", ex);
                                        DisplayErrorMessage();
                                    }
                                }
                                Mouse.OverrideCursor = null;
                            }
                            else
                            {
                                // it is old
                                //check to see if old querybuilder and new querybuilder is same, if different CompairQueryBuilder retuns true
                                if (CompairQueryBuilder(rv.QueryBulder, queryBuilder))
                                {
                                    Mouse.OverrideCursor = Cursors.Wait;
                                    try
                                    {
                                        Control.ResultViewControl resultControl = new Control.ResultViewControl(queryBuilder, CurrentDatabaseName);
                                        ResultViewModel rv1 = (ResultViewModel)resultControl.DataContext;
                                        //set isModified 
                                        rv1.isModified = true;
                                        //assign orignial directory path
                                        rv1.directoryPath = rv.directoryPath;
                                        tabItem.Content = resultControl;
                                        tabItem.labelStar.Content = "*";
                                    }
                                    catch (MySql.Data.MySqlClient.MySqlException ex)
                                    {
                                        if (isErrorLoggingOn)
                                        {
                                            LogError.Log_Err("btnRunQuery_Click", ex);
                                            DisplayErrorMessage();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (isErrorLoggingOn)
                                        {
                                            LogError.Log_Err("btnRunQuery_Click", ex);
                                            DisplayErrorMessage();
                                        }
                                    }
                                    Mouse.OverrideCursor = null;
                                }
                                //else it is same we don't need do anything
                            }

                        }

                        if (DocPanelCustomQuery.HorizontalAlignment != HorizontalAlignment.Right)
                        {
                            MiniMizeCustomeQyeryWindow();
                        }
                    }
                }
            }
            Console.WriteLine("Cross Tabulation View Control execution time: " + (DateTime.Now - startTime));
        }

        private void BtnPinCustomQueryWindow_Click(object sender, RoutedEventArgs e)
        {

            this.adornedControl.IsAdornerVisible = false;
            //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
        }

        private void BtnHideCustomQueryWindow_Click(object sender, RoutedEventArgs e)
        {
            this.TreeViewDocPanel.Width = 0;
            TreeViewHideStackPanel.Width = 20;

            this.adornedControl.IsAdornerVisible = true;

            DocPanelCustomQuery.HorizontalAlignment =
            HorizontalAlignment.Right;
            if (DocPanelCustomQuery.HorizontalAlignment == HorizontalAlignment.Right)
            {
                DocPanelCustomQuery.HorizontalAlignment = HorizontalAlignment.Stretch;
                DocPanelCustomQuery.Margin = new Thickness(0, 60, 0, 0);
                DocPanelCustomQuery.Width =
                Double.NaN;
                //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                DocPanelCustomQuery.Margin = new Thickness(0, 0, 0, 0);
                DocPanelCustomQuery.Width = 500;
                DocPanelCustomQuery.HorizontalAlignment = HorizontalAlignment.Right;
                //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
            }

        }

        public List<SQLBuilder.Clauses.Column> GenerateListOfSelectTabCntrlColumns(ResultViewControl rc)
        {
            List<SQLBuilder.Clauses.Column> list = new List<SQLBuilder.Clauses.Column>();

            //get the From table columns 
            if (this.listOfTable != null & this.listOfTable.Count > 0)
            {
                if (rc.FromTabCntrl.cmbFromTable.SelectedIndex != -1)
                {
                    foreach (Column col in this.listOfTable[rc.FromTabCntrl.cmbFromTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();//false, this.FromTabUC.txtFromAlias.Text + "." + col.name, "");
                        sc.Name = rc.FromTabCntrl.txtFromAlias.Text + "." + col.name;
                        sc.AliasName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(col.name);
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }
                }
            }
            // get join table columns
            if (rc.FromTabCntrl.StackPanelFromTab.Children.Count > 0)
            {
                for (int i = 0; i < rc.FromTabCntrl.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs = (FromTabStackPanelControl)rc.FromTabCntrl.StackPanelFromTab.Children[i];
                    if (fs.cmbFromTabJoinTable.SelectedIndex > 0)
                    {
                        foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTable.SelectedIndex].columns)
                        {
                            SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                            sc.Name = fs.txtJoinTableAlias.Text + "." + col.name;
                            sc.AliasName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fs.txtJoinTableAlias.Text + " " + col.name);
                            sc.Format = col.format;
                            sc.DataType = col.type;
                            list.Add(sc);
                        }
                    }
                    //if (fs.cmbFromTabJoinTableMore.SelectedIndex > 0)
                    //{
                    //    foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTableMore.SelectedIndex].columns)
                    //    {
                    //        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                    //        sc.Name = fs.txtJoinTableAlias.Text + "." + col.name;
                    //        sc.Format = col.format;
                    //        list.Add(sc);
                    //    }
                    //}

                }
            }
            return list;
        }

        private List<SQLBuilder.Clauses.Column> GenerateListOfSelectTabColumns()
        {
            List<SQLBuilder.Clauses.Column> list = new List<SQLBuilder.Clauses.Column>();

            //get the From table columns 
            if (this.listOfTable != null & this.listOfTable.Count > 0)
            {
                if (this.FromTabUC.cmbFromTable.SelectedIndex != -1)
                {
                    foreach (Column col in this.listOfTable[this.FromTabUC.cmbFromTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();//false, this.FromTabUC.txtFromAlias.Text + "." + col.name, "");
                        sc.Name = this.FromTabUC.txtFromAlias.Text + "." + col.name;
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }
                }
            }
            // get join table columns
            if (this.FromTabUC.StackPanelFromTab.Children.Count > 0)
            {
                for (int i = 0; i < this.FromTabUC.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs = (FromTabStackPanelControl)this.FromTabUC.StackPanelFromTab.Children[i];
                    foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                        sc.Name = fs.txtJoinTableAlias.Text + "." + col.name;
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }

                    //foreach (string name in ((List<string>)fs.cmbFromTabJoinColumns.ItemsSource))
                    //{
                    //    SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                    //    sc.Name = fs.txtJoinTableAlias.Text + "." + name;
                    //    sc.Format = 
                    //    list.Add(sc);
                    //}
                }

                //for (int i = 0; i < this.FromTabUC.StackPanelFromTab.Children.Count; i++)
                //{
                //    FromTabStackPanelControl fs = (FromTabStackPanelControl)this.FromTabUC.StackPanelFromTab.Children[i];
                //    foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTableMore.SelectedIndex].columns)
                //    {
                //        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                //        sc.Name = fs.txtJoinTableAlias.Text + "." + col.name;
                //        sc.Format = col.format;
                //        list.Add(sc);
                //    }

                //}
            }
            return list;
        }
        //private List<MySQLData.Column> GenerateListOfSelectTabColumns()
        //{
        //    List<MySQLData.Column> list = new List<MySQLData.Column>();

        //    //get the From table columns 
        //    if (this.listOfTable != null & this.listOfTable.Count > 0)
        //    {
        //        if (this.FromTabUC.cmbFromTable.SelectedIndex != -1)
        //        {
        //            foreach (Column col in this.listOfTable[this.FromTabUC.cmbFromTable.SelectedIndex].columns)
        //            {
        //                MySQLData.Column sc = new MySQLData.Column(this.FromTabUC.txtFromAlias.Text + "." + col.name, col.type);
        //                list.Add(sc);
        //            }
        //        }
        //    }
        //    // get join table columns
        //    if (this.FromTabUC.StackPanelFromTab.Children.Count > 0)
        //    {
        //        for (int i = 0; i < this.FromTabUC.StackPanelFromTab.Children.Count; i++)
        //        {
        //            FromTabStackPanelControl fs = (FromTabStackPanelControl)this.FromTabUC.StackPanelFromTab.Children[i];
        //            foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTable.SelectedIndex].columns)
        //            {
        //                MySQLData.Column sc = new MySQLData.Column(fs.txtJoinTableAlias.Text + "." + col.name, col.type);
        //                list.Add(sc);
        //            }
        //            //foreach (string name in ((List<string>)fs.cmbFromTabJoinColumns.ItemsSource))
        //            //{
        //            //    MySQLData.Column sc = new MySQLData.Column(fs.txtJoinTableAlias.Text + "." + name, );
        //            //      list.Add(sc);
        //            //}
        //        }
        //    }
        //    return list;
        //}            

        //zahed code start

        public List<SQLBuilder.Clauses.Column> GenerateListOfTabulationTabCntrlColumns_old(ResultViewControl rc)
        {
            List<SQLBuilder.Clauses.Column> list = new List<SQLBuilder.Clauses.Column>();

            //get the From table columns 
            if (this.listOfTable != null & this.listOfTable.Count > 0)
            {
                if (rc.FromTabCntrl.cmbFromTable.SelectedIndex != -1)
                {
                    foreach (Column col in this.listOfTable[rc.FromTabCntrl.cmbFromTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();//false, this.FromTabUC.txtFromAlias.Text + "." + col.name, "");
                        sc.Name = rc.FromTabCntrl.txtFromAlias.Text + "." + col.name;
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }
                }
            }
            // get join table columns
            if (rc.FromTabCntrl.StackPanelFromTab.Children.Count > 0)
            {
                for (int i = 0; i < rc.FromTabCntrl.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs = (FromTabStackPanelControl)rc.FromTabCntrl.StackPanelFromTab.Children[i];
                    foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                        sc.Name = fs.txtJoinTableAlias.Text + "." + col.name;
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }
                }
            }
            return list;

        }

        public List<SQLBuilder.Clauses.Column> GenerateListOfTabulationTabCntrlColumns(ResultViewControl rc)
        {
            if (rc.SelectTabCntrl._SelectedColCollection.Count > 0)
            {
                List<SQLBuilder.Clauses.Column> list = new List<SQLBuilder.Clauses.Column>();
                foreach (SQLBuilder.Clauses.Column column in rc.SelectTabCntrl._SelectedColCollection)
                {
                    SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                    sc.Name = column.AliasName;//column.Name;
                    sc.Format = column.Format;
                    sc.DataType = column.DataType;
                    list.Add(sc);
                }
                return list;
            }
            else
            {
                return GenerateListOfSelectTabCntrlColumns(rc);
            }
        }

        private List<SQLBuilder.Clauses.Column> GenerateListOfTabulationTabColumns()
        {
            if (this.SelectTabUC._SelectedColCollection.Count > 0)
            {
                List<SQLBuilder.Clauses.Column> list = new List<SQLBuilder.Clauses.Column>();
                foreach (SQLBuilder.Clauses.Column column in this.SelectTabUC._SelectedColCollection)
                {
                    SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                    sc.Name = column.AliasName;
                    sc.Format = column.Format;
                    sc.DataType = column.DataType;
                    list.Add(sc);
                }
                return list;
            }
            else
            {
                return GenerateListOfSelectTabColumns();
            }
        }

        private List<SQLBuilder.Clauses.Column> GenerateListOfTabulationTabColumns_old()
        {
            List<SQLBuilder.Clauses.Column> list = new List<SQLBuilder.Clauses.Column>();

            //get the From table columns 
            if (this.listOfTable != null & this.listOfTable.Count > 0)
            {
                if (this.FromTabUC.cmbFromTable.SelectedIndex != -1)
                {
                    foreach (Column col in this.listOfTable[this.FromTabUC.cmbFromTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();//false, this.FromTabUC.txtFromAlias.Text + "." + col.name, "");
                        sc.Name = this.FromTabUC.txtFromAlias.Text + "." + col.name;
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }
                }
            }
            // get join table columns
            if (this.FromTabUC.StackPanelFromTab.Children.Count > 0)
            {
                for (int i = 0; i < this.FromTabUC.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs = (FromTabStackPanelControl)this.FromTabUC.StackPanelFromTab.Children[i];
                    foreach (Column col in this.listOfTable[fs.cmbFromTabJoinTable.SelectedIndex].columns)
                    {
                        SQLBuilder.Clauses.Column sc = new SQLBuilder.Clauses.Column();
                        sc.Name = fs.txtJoinTableAlias.Text + "." + col.name;
                        sc.Format = col.format;
                        sc.DataType = col.type;
                        list.Add(sc);
                    }
                }
            }
            return list;

        }

        //zahed code end

        //private ObservableCollection<SelectTabColumn> GenerateSelectTabColumns()
        //{
        //    ObservableCollection<SelectTabColumn> list = new ObservableCollection<SelectTabColumn>();


        //    //get the From table columns 
        //    if (this.listOfTable != null & this.listOfTable.Count > 0)
        //    {
        //        foreach (Column col in this.listOfTable[this.FromTabUC.cmbFromTable.SelectedIndex].columns)
        //        {
        //            SelectTabColumn sc = new SelectTabColumn(false, this.FromTabUC.txtFromAlias.Text + "." + col.name, "");
        //            list.Add(sc);
        //        }

        //    }
        //    // get join table columns
        //    if (this.FromTabUC.StackPanelFromTab.Children.Count > 0)
        //    {
        //        for (int i = 0; i < this.FromTabUC.StackPanelFromTab.Children.Count; i++)
        //        {
        //            FromTabStackPanelControl fs = (FromTabStackPanelControl)this.FromTabUC.StackPanelFromTab.Children[i];
        //            foreach (string name in ((List<string>)fs.cmbFromTabJoinColumns.ItemsSource))
        //            {
        //                SelectTabColumn sc = new SelectTabColumn(false, fs.txtJoinTableAlias.Text + "." + name, "");
        //                list.Add(sc);
        //            }

        //            //List<string> jc = GetJoinTableColums((List<string>)fs.cmbFromTabJoinColumns.ItemsSource, fs.txtJoinTableAlias.Text);
        //            //if (jc != null)
        //            //{
        //            //    list.AddRange(jc);
        //            //}
        //        }
        //    }
        //    return list;
        //}

        private void btnFromTabSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private String OpenFileDialog()
        {
            string filename = System.String.Empty;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Title = "Open Query";
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "xml documents (.xml)|*.xml"; // Filter files by extension
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
            }
            return filename;
        }
        public String OpenSaveDialog(string defaultFileName)    //made this public
        {
            string filename = System.String.Empty;
            // Configure open file dialog box
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Title = "Save Query AS";
            dlg.FileName = defaultFileName;//"Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "xml documents (.xml)|*.xml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
            }

            return filename;
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            //Open_Click();
        }

        private void New_Click()
        {
            /******************************/
            this.Logo.Visibility = Visibility.Hidden;
            this.LogoTxt.Visibility = Visibility.Hidden;
            this.tabControl1.Visibility = Visibility.Visible;
            /******************************/

            count = 0;
            //addd new tab
            CloseableTabItem tabItem = new CloseableTabItem();
            tabItem.Name = "CustomQuery" + (this.tabControl1.Items.Count + 1).ToString();
            tabItem.Header = "SQLQuery" + (this.tabControl1.Items.Count + 1).ToString();
            tabItem.Part_Label_Content = "";
            //tableViewModel tvm = null;
            currentTableViewModel = null;
            Control.ResultViewControl rvCntrl = new ResultViewControl(currentTableViewModel, false);
            tabItem.Content = rvCntrl;

            //add tab to tabcontrol
            this.tabControl1.Items.Add(tabItem);
            //Clear Custom QueryTabs

            //ClearAllCustomeQueryTabs();

            //Get focus on new tab
            ((TabItem)(this.tabControl1.Items[this.tabControl1.Items.Count - 1])).Focus();

            //this.adornedControl.IsAdornerVisible = true;

            //Display Custom query window
            //DocPanelCustomQuery.HorizontalAlignment = HorizontalAlignment.Right;
            //DocPanelCustomQuery.HorizontalAlignment = HorizontalAlignment.Stretch;
            //DocPanelCustomQuery.Margin = new Thickness(0, 60, 0, 0);
            //DocPanelCustomQuery.Width = Double.NaN;
            //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Hidden;
            //Hide if tree view window open
            //if (this.TreeViewDocPanel.Width != 0)
            //{
            //    this.TreeViewDocPanel.Width = 0;
            //    TreeViewHideStackPanel.Width = 20;
            //}
        }

        private void Open_Click()
        {
            string FileName = OpenFileDialog();
            if (FileName != System.String.Empty)
            {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
                SelectQueryBuilder loadedQuery = (SelectQueryBuilder)SerializerObj.Deserialize(new StreamReader(FileName));
                string[] splitedArray = FileName.Split('\\');
                string tabItemName = splitedArray[splitedArray.Length - 1].Remove((splitedArray[splitedArray.Length - 1]).Length - 4, 4);
                if (loadedQuery != null)
                {
                    //addd new tab
                    CloseableTabItem tabItem = new CloseableTabItem();
                    tabItem.Name = "CustomQuery" + (this.tabControl1.Items.Count + 1).ToString();
                    tabItem.Header = tabItemName;

                    /******************************/
                    this.Logo.Visibility = Visibility.Hidden;
                    this.LogoTxt.Visibility = Visibility.Hidden;
                    this.tabControl1.Visibility = Visibility.Visible;
                    /******************************/

                    //add tab to tabcontrol
                    this.tabControl1.Items.Add(tabItem);
                    //Clear Custom QueryTabs

                    //Modified 11/10/11 clearing tab with common method ClearAllCustomeQueryTabs
                    //ClearAllCustomeQueryTabs();
                    ////Clear from tab
                    //this.FromTabUC.cmbFromTable.SelectedIndex = -1;
                    //this.FromTabUC.txtFromAlias.Text = System.String.Empty;
                    //this.FromTabUC.StackPanelFromTab.Children.Clear();

                    //////Clear where tab
                    //this.WhereTabUC.StackPanelWhereTab.Children.Clear();

                    ////clear Select Tab
                    //this.SelectTabUC.dgSelectTab.ItemsSource = null;

                    //Get focus on new tab
                    ((TabItem)(this.tabControl1.Items[this.tabControl1.Items.Count - 1])).Focus();
                    string queryString = loadedQuery.BuildQuery();
                    if (queryString != System.String.Empty)
                    {
                        CloseableTabItem tbItem = (CloseableTabItem)this.tabControl1.SelectedItem;
                        Mouse.OverrideCursor = Cursors.Wait;
                        Control.ResultViewControl resultControl = new Control.ResultViewControl(loadedQuery, CurrentDatabaseName);

                        //*****
                        directoryPath = FileName.Replace(splitedArray[splitedArray.Length - 1], "");//Directory.GetCurrentDirectory();
                        ResultViewModel rv = (ResultViewModel)resultControl.result;//DataContext;
                        rv.directoryPath = directoryPath;
                        //*********

                        tabItem.Content = resultControl;
                        //LoadAllCustomQueryTabs(loadedQuery, rv._getResultByTreeView);
                        LoadAllCustomQueryCntrls(rv.QueryBulder, rv._getResultByTreeView, resultControl);
                        Mouse.OverrideCursor = null;
                        //if (DocPanelCustomQuery.HorizontalAlignment != HorizontalAlignment.Right)
                        //{
                        //    MiniMizeCustomeQyeryWindow();
                        //}
                    }
                }
            }
        }

        private void Save_Click()
        {
            CloseableTabItem closeAbleTabItem = (CloseableTabItem)this.tabControl1.SelectedItem;
            if (closeAbleTabItem.Name != "tabItem1")
            {
                CheckQueryNeededToBeSaved(closeAbleTabItem, "Save");
            }
        }
        private void menuItemSave_Click(object sender, RoutedEventArgs e)
        {
            //Directory.GetCurrentDirectory();
            //CloseableTabItem closeAbleTabItem = (CloseableTabItem)this.tabControl1.SelectedItem;
            //if (closeAbleTabItem.Name != "tabItem1")
            //{
            //    CheckQueryNeededToBeSaved(closeAbleTabItem,"Save");
            //    //ResultViewControl rc = (ResultViewControl)closeAbleTabItem.Content;
            //    //if (rc != null)
            //    //{
            //    //    ResultViewModel rv = (ResultViewModel)rc.DataContext;
            //    //    string FileName =  Directory.GetCurrentDirectory() + "\\" + closeAbleTabItem.Name +".xml";
            //    //    if (FileName != System.String.Empty)
            //    //    {
            //    //        XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
            //    //        StreamWriter swriter = new StreamWriter(FileName);
            //    //        SerializerObj.Serialize(swriter, rv.QueryBulder);

            //    //        swriter.Flush();
            //    //        swriter.Close();
            //    //    }

            //    //}
            //}


        }
        private void menuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            CloseableTabItem tabItem = (CloseableTabItem)this.tabControl1.SelectedItem;

            if (tabItem.Name != "tabItem1")
            {

                CheckQueryNeededToBeSaved(tabItem, "SaveAs");

            }
            //if (closeAbleTabItem.Name != "tabItem1")
            //{
            //    ResultViewControl rc = (ResultViewControl)closeAbleTabItem.Content;
            //    if (rc != null)
            //    {
            //        ResultViewModel rv = (ResultViewModel)rc.DataContext;
            //        string FileName = OpenSaveDialog(closeAbleTabItem.Name);
            //        if (FileName != System.String.Empty)
            //        {
            //            XmlSerializer SerializerObj = new XmlSerializer(typeof(SelectQueryBuilder));
            //            StreamWriter swriter = new StreamWriter(FileName);
            //            SerializerObj.Serialize(swriter, rv.QueryBulder);

            //            swriter.Flush();
            //            swriter.Close();
            //        }

            //    }
            //}
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl TC = (TabControl)sender;
            CloseableTabItem closeAbleTabItem = (CloseableTabItem)TC.SelectedItem;//(CloseableTabItem)this.tabControl1.SelectedItem;

            if (closeAbleTabItem != null)
            {
                if (closeAbleTabItem.Name != "tabItem1")
                {
                    ResultViewControl rc = (ResultViewControl)closeAbleTabItem.Content;
                    if (rc != null && currentTableViewModel != null)         //zahed
                    {
                        ResultViewModel rv = (ResultViewModel)rc.DataContext;
                        if (rv != null)                                     //zahed
                        {
                            //LoadAllCustomQueryTabs(rv.QueryBulder, rv._getResultByTreeView);
                            //zahed
                            if (count == 0) //it is causing the error in Select All action in select because of variable ColListForSelectTab in MainWindow which is not refresheing according to tab
                            {
                                LoadAllCustomQueryCntrls(rv.QueryBulder, rv._getResultByTreeView, rc);
                            }
                            //zahed

                        }
                    }
                    else
                    {
                        //Modified on 11/28/11****************
                        ClearAllCustomeQueryTabs();
                        //ClearAllCustomQueryTabCntrls(rc);
                        //**********************
                        ////Clear from tab
                        //this.FromTabUC.cmbFromTable.SelectedIndex = -1;
                        //this.FromTabUC.txtFromAlias.Text = System.String.Empty;
                        //this.FromTabUC.StackPanelFromTab.Children.Clear();
                        //this.FromTabUC.DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Hidden;
                        //////Clear where tab
                        //this.WhereTabUC.StackPanelWhereTab.Children.Clear();

                        ////clear Select Tab
                        //this.SelectTabUC.lstToSelecteColFrom.ItemsSource = null;
                        //this.SelectTabUC.lstSelectedCol.ItemsSource = null;

                        ////clear action tab txtquery
                        //this.txtQuery.Text = "";
                    }
                    this.CustomQueryTxTBlk.Text = "Custom query for " + closeAbleTabItem.Header;
                    //Get focus on new tab
                    //((TabItem)(this.tabControlCustomQuery.Items[0])).Focus();                 //zahed
                    //modified following line on 10/27/11
                    //((TabItem)(this.tabControlCustomQuery.Items[0])).IsSelected =true;        //zahed
                }

                if (closeAbleTabItem.Name == "tabItem1")
                {
                    //***** modified on 11/28//11************
                    //ClearAllCustomeQueryTabs();                                               //zahed
                    //***************************************
                    //((TabItem)(this.tabControlCustomQuery.Items[0])).IsSelected = true;       //zahed
                }
                ////hide custom query window or pin custome query window
                //this.adornedControl.IsAdornerVisible = false;
                //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ClearAllCustomeQueryTabs()
        {
            //Clear from tab
            this.FromTabUC.cmbFromTable.SelectedIndex = -1;
            this.FromTabUC.txtFromAlias.Text = System.String.Empty;
            this.FromTabUC.StackPanelFromTab.Children.Clear();
            this.FromTabUC.DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Hidden;

            ////Clear where tab
            this.WhereTabUC.StackPanelWhereTab.Children.Clear();

            //clear Select Tab
            this.SelectTabUC.lstToSelecteColFrom.ItemsSource = null;
            this.SelectTabUC._SelectedColCollection.Clear();

            this.SelectTabUC.lstSelectedCol.ItemsSource = this.SelectTabUC._SelectedColCollection;
            //clear old regular tabulation
            //clear group by rows
            for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count; i++)
            {
                TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[i];
                //tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                tg.cmbTabulationTabGroupByColumnsName.SelectedIndex = -1;
                tg.cmbTabulationSort.SelectedIndex = -1;
                tg.txtTabulationTabGroupByAlias.Text = System.String.Empty;
            }
            //clear summary rows
            for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count; i++)
            {
                TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabUC.StackPanelTabuLationTabSummary.Children[i];
                ts.cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                //ts.cmbTabulationTabSummaryColumnsName.SelectedIndex = -1;
                ts.cmbTabulationTypeOfSummary.SelectedIndex = -1;
                ts.cmbTabulationTabUserSelectSummaryColFormat.SelectedIndex = -1;
                ts.txtTabulationTabSummaryAlias.Text = System.String.Empty;
                ts.txtTabulationTabSummaryColFormat.Text = System.String.Empty;

            }
            //clear old Cross tabulation
            //clear group by rows
            for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
            {
                CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[i];
                //ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                ctg.cmbCrossTabulationTabGroupByColumnsName.SelectedIndex = -1;
                ctg.cmbCrossTabulationSort.SelectedIndex = -1;
                ctg.txtCrossTabulationTabGroupByAlias.Text = System.String.Empty;
            }
            //clear summary rows
            for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
            {
                CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children[i];
                //cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                cts.cmbCrossTabulationTabSummaryColumnsName.SelectedIndex = -1;
                cts.cmbCrossTabulationTypeOfSummary.SelectedIndex = -1;
                cts.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedIndex = -1;
                cts.txtCrossTabulationTabSummaryColFormat.Text = System.String.Empty;
                cts.txtCrossTabulationTabSummaryAlias.Text = System.String.Empty;

            }
            // clear summary frist row
            //this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
            this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectedIndex = -1;
            this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex = -1;
            //clear action tab txtquery
            this.txtQuery.Text = "";
            this.CustomQueryTxTBlk.Text = "";
        }

        /************************************************zahed************************/

        private void ClearAllCustomQueryTabCntrls(ResultViewControl rc)
        {
            //Clear from tab
            rc.FromTabCntrl.cmbFromTable.SelectedIndex = -1;
            rc.FromTabCntrl.txtFromAlias.Text = System.String.Empty;
            rc.FromTabCntrl.StackPanelFromTab.Children.Clear();
            rc.FromTabCntrl.DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Hidden;

            ////Clear where tab
            rc.WhereTabCntrl.StackPanelWhereTab.Children.Clear();

            //clear Select Tab
            rc.SelectTabCntrl.lstToSelecteColFrom.ItemsSource = null;
            rc.SelectTabCntrl._SelectedColCollection.Clear();

            rc.SelectTabCntrl.lstSelectedCol.ItemsSource = rc.SelectTabCntrl._SelectedColCollection;
            //clear old regular tabulation
            //clear group by rows
            for (int i = 0; i < rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
            {
                TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                //tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                tg.cmbTabulationTabGroupByColumnsName.SelectedIndex = -1;
                tg.cmbTabulationSort.SelectedIndex = -1;
                tg.txtTabulationTabGroupByAlias.Text = System.String.Empty;
            }
            //clear summary rows
            for (int i = 0; i < rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
            {
                TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                //ts.cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                ts.cmbTabulationTabSummaryColumnsName.SelectedIndex = -1;
                ts.cmbTabulationTypeOfSummary.SelectedIndex = -1;
                ts.cmbTabulationTabUserSelectSummaryColFormat.SelectedIndex = -1;
                ts.txtTabulationTabSummaryAlias.Text = System.String.Empty;
                ts.txtTabulationTabSummaryColFormat.Text = System.String.Empty;

            }
            //clear old Cross tabulation
            //clear group by rows
            for (int i = 0; i < rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
            {
                CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                //ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                ctg.cmbCrossTabulationTabGroupByColumnsName.SelectedIndex = -1;
                ctg.cmbCrossTabulationSort.SelectedIndex = -1;
                ctg.txtCrossTabulationTabGroupByAlias.Text = System.String.Empty;
            }
            //clear summary rows
            for (int i = 0; i < rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
            {
                CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                //cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                cts.cmbCrossTabulationTabSummaryColumnsName.SelectedIndex = -1;
                cts.cmbCrossTabulationTypeOfSummary.SelectedIndex = -1;
                cts.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedIndex = -1;
                cts.txtCrossTabulationTabSummaryColFormat.Text = System.String.Empty;
                cts.txtCrossTabulationTabSummaryAlias.Text = System.String.Empty;

            }
            // clear summary frist row
            rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
            rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectedIndex = -1;
            rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex = -1;
            //clear action tab txtquery
            this.txtQuery.Text = "";
            this.CustomQueryTxTBlk.Text = "";
        }

        private void LoadAllCustomQueryCntrls(SQLBuilder.SelectQueryBuilder queryBuilder, bool isGetResultByTreeView, ResultViewControl rc)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string db = ConfigurationManager.AppSettings["DefaultDatabase"].ToString();
            List<Schema> schemas1 = MySQLData.DataAccess.ADODataBridge.getSchemaTree(connectionString, db, ConfigurationManager.AppSettings["DerivedTablesPath"]);//DataAccess.GetDatabases();

            listOfTable = new List<MySQLData.Table>();
            foreach (Schema schema in schemas1)
            {
                listOfTable.AddRange(schema.tables);
            }

            if (listOfTable != null)
            {
                rc.FromTabCntrl.cmbFromTable.ItemsSource = Common.ConvertTablesToStringList(listOfTable);
            }
            rc.FromTabCntrl.cmbFromTable.SelectedIndex = Common.getIndex((List<string>)rc.FromTabCntrl.cmbFromTable.ItemsSource, queryBuilder.SelectedTables[0].Name);
            rc.FromTabCntrl.txtFromAlias.Text = queryBuilder.SelectedTables[0].AliasName;

            int loopCount = 0;
            foreach (SQLBuilder.Clauses.JoinClause JoinClasue in queryBuilder.Joins)
            {
                // create join row control
                int numberOfStackPanel = loopCount++;
                FromTabStackPanelControl fc = new FromTabStackPanelControl();
                fc.Name = "Fs2";
                fc.Margin = new Thickness(0, 5, 0, 5);
                fc.btnDelete.Visibility = System.Windows.Visibility.Visible;
                fc.btnDelete.Uid = (numberOfStackPanel + 1).ToString();
                fc.btnDelete.Width = 25.0;

                if (this.listOfTable != null & this.listOfTable.Count > 0)
                {
                    fc.cmbFromTabJoinTable.ItemsSource = Common.ConvertTablesToStringList(this.listOfTable);
                    fc.cmbFromTabFromColumns.ItemsSource = Common.ConvertColumsToStringList(this.listOfTable[rc.FromTabCntrl.cmbFromTable.SelectedIndex].columns);
                }
                fc.cmbFromTabJoinType.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabJoinType.ItemsSource, JoinClasue.JoinType.ToString());
                fc.cmbFromTabJoinTable.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabJoinTable.ItemsSource, JoinClasue.ToTable.Name);
                fc.txtJoinTableAlias.Text = JoinClasue.ToTable.AliasName;
                fc.cmbFromTabFromColumns.Text = JoinClasue.FromColumn.ToString();
                fc.cmbFromTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabQueryOpretor.ItemsSource, JoinClasue.ComparisonOperator.ToString());
                fc.cmbFromTabJoinColumns.ItemsSource = Common.ConvertColumsToStringList(this.listOfTable[fc.cmbFromTabJoinTable.SelectedIndex].columns);
                fc.cmbFromTabJoinColumns.Text = JoinClasue.ToColumn.ToString();
                rc.FromTabCntrl.StackPanelFromTab.Children.Add(fc);
                rc.FromTabCntrl.DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Visible;
                rc.FromTabCntrl.borderJoinDock.Visibility = System.Windows.Visibility.Visible;


                SQLBuilder.Clauses.WhereClause[] joinConditions = queryBuilder.Joins[loopCount - 1].JoinCondition.ToArray<SQLBuilder.Clauses.WhereClause>();
                for (int index = 1; index < joinConditions.Length; index++)
                {
                    FromTabStackPanelControlMore fcm = new FromTabStackPanelControlMore();
                    fcm.Name = "Fs2";
                    fcm.Margin = new Thickness(0, 5, 0, 5);
                    fcm.btndeletemore.Visibility = System.Windows.Visibility.Visible;
                    fcm.btndeletemore.Uid = (numberOfStackPanel + 1).ToString();
                    fcm.btndeletemore.Width = 25.0;

                    List<string> Collist = (from x in (GenerateListOfSelectTabCntrlColumns(rc))
                                            select x.Name).ToList<string>();

                    //List<string> Collist1 = (from x in (GenerateListOfSelectTabColumns(rc))
                    //                        select x.Name).ToList<string>();

                    SQLBuilder.Clauses.GeneralWhereClause generalClause = (SQLBuilder.Clauses.GeneralWhereClause)joinConditions[index];
                    fcm.cmbFromTabFromColumns.ItemsSource = Collist;
                    fcm.cmbFromTabJoinColumns.ItemsSource = Collist;
                    //if (this.listOfTable != null & this.listOfTable.Count > 0)
                    //{
                    //    fcm.cmbFromTabFromColumns.ItemsSource = Common.ConvertColumsToStringList(this.listOfTable[fc.cmbFromTabFromColumns.SelectedIndex].columns);
                    //    fcm.cmbFromTabJoinColumns.ItemsSource = Common.ConvertColumsToStringList(this.listOfTable[fc.cmbFromTabJoinColumns.SelectedIndex].columns);
                    //}
                    fcm.cmbFromTabFromANDOR.Visibility = System.Windows.Visibility.Visible;
                    List<string> ListOfLogicalOpreator = new List<string> { "And", "Or" };
                    fcm.cmbFromTabFromANDOR.ItemsSource = ListOfLogicalOpreator;
                    fcm.cmbFromTabFromANDOR.SelectedIndex = Common.getIndex((List<string>)fcm.cmbFromTabFromANDOR.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));


                    fcm.cmbFromTabFromColumns.Text = generalClause.FieldName;
                    fcm.cmbFromTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)fcm.cmbFromTabQueryOpretor.ItemsSource, Common.GetStringValueForEnum("Comparison", generalClause.ComparisonOperator));

                    fcm.cmbFromTabJoinColumns.Text = generalClause.Value.ToString();
                    fc.StackPanelFromTabMore.Children.Add(fcm);




                }

                //int RowNumber1 = 0;
                //foreach (SQLBuilder.Clauses.JoinClause whereClause in queryBuilder.Joins)
                //{
                //    int i = 1;
                //    FromTabStackPanelControlMore fcm = new FromTabStackPanelControlMore();
                //    int innerJoinCount = queryBuilder.Joins[loopCount - 1].JoinCondition.Count;
                //    for (i = 1; i < innerJoinCount;i++)
                //    {
                //        fcm.Name = "Fs2";
                //        fcm.Margin = new Thickness(0, 5, 0, 5);
                //        fcm.btndeletemore.Visibility = System.Windows.Visibility.Visible;
                //        fcm.btndeletemore.Uid = (numberOfStackPanel + 1).ToString();
                //        fcm.btndeletemore.Width = 25.0;
                //    }
                //    string controlType = whereClause.GetType().FullName;
                //    List<string> Collist = (from x in (GenerateListOfSelectTabCntrlColumns(rc))
                //                            select x.Name).ToList<string>();
                //    RowNumber1 = RowNumber1 + 1;
                //    SQLBuilder.Clauses.JoinClause generalClause = (SQLBuilder.Clauses.JoinClause)whereClause;

                //    //FromTabStackPanelControlMore fcm = new FromTabStackPanelControlMore();
                //    //if (RowNumber1 != 1)
                //    //{
                //    //    if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                //    //    {
                //    //        generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                //    //    }
                //    //}
                //    //fcm.cmbFromTabFromANDOR.SelectedIndex = Common.getIndex((List<string>)fcm.cmbFromTabFromANDOR.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                //    if (RowNumber1 == 1)
                //    {
                //        fcm.cmbFromTabFromANDOR.Visibility = System.Windows.Visibility.Hidden;
                //    }
                //    fcm.cmbFromTabFromColumns.ItemsSource = Collist;
                //    fcm.cmbFromTabFromColumns.Text = generalClause.FromColumn.ToString();
                //    fcm.cmbFromTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)fcm.cmbFromTabQueryOpretor.ItemsSource, Common.GetStringValueForEnum("Comparison", generalClause.ComparisonOperator));
                //    fcm.cmbFromTabJoinColumns.ItemsSource = Collist;
                //    fcm.cmbFromTabJoinColumns.Text = generalClause.ToColumn.ToString();
                //    fc.StackPanelFromTabMore.Children.Add(fcm);
                //    i++;
                //}



                //int RowNumber1 = 0;
                //foreach (SQLBuilder.Clauses.WhereClause whereClause in queryBuilder.Where)
                //{
                //    string controlType = whereClause.GetType().FullName;
                //    List<string> Collist = (from x in (GenerateListOfSelectTabCntrlColumns(rc))
                //                            select x.Name).ToList<string>();
                //    RowNumber1 = RowNumber1 + 1;
                //    SQLBuilder.Clauses.GeneralWhereClause generalClause = (SQLBuilder.Clauses.GeneralWhereClause)whereClause;
                //    FromTabStackPanelControlMore fcm = new FromTabStackPanelControlMore();
                //    if (RowNumber1 != 1)
                //    {
                //        if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                //        {
                //            generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                //        }
                //    }
                //    fcm.cmbFromTabFromANDOR.SelectedIndex = Common.getIndex((List<string>)fcm.cmbFromTabFromANDOR.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                //    if (RowNumber1 == 1)
                //    {
                //        fcm.cmbFromTabFromANDOR.Visibility = System.Windows.Visibility.Hidden;
                //    }
                //    fcm.cmbFromTabFromColumns.ItemsSource = Collist;
                //    fcm.cmbFromTabFromColumns.Text = generalClause.FieldName;
                //    fcm.cmbFromTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)fcm.cmbFromTabQueryOpretor.ItemsSource, Common.GetStringValueForEnum("Comparison", generalClause.ComparisonOperator));
                //    fcm.cmbFromTabJoinColumns.ItemsSource = Collist;
                //    fcm.cmbFromTabJoinColumns.Text = generalClause.Value.ToString();
                //    fc.StackPanelFromTabMore.Children.Add(fcm);
                //}
            }

            //load select tab
            ColListForSelectTab = GenerateListOfSelectTabCntrlColumns(rc);
            // seting up select Tab's observabla collection _FromSelectedColToCollection
            //ColListForSelectTab.ForEach(x => this.SelectTabUC._FromSelectedColToCollection.Add(x));
            rc.SelectTabCntrl.lstToSelecteColFrom.ItemsSource = ColListForSelectTab;
            //clear old selected columns
            rc.SelectTabCntrl._SelectedColCollection.Clear();
            //load selected Column List and set  _SelectedColCollection observable collection from queryBuilder.SelectedColumns
            //Regex regex = new Regex("(?<X>[.]+).(?<Z>)");// this to check if queryBuilder is not generated from custome query, so if user selects table from Tree view we don't need to show selected columns
            if (queryBuilder.SelectedColumns.Count != 0)
            {
                if (!isGetResultByTreeView)
                {
                    //first we copy  queryBuilder.SelectedColumns from generic list to observable collection
                    queryBuilder.SelectedColumns.ForEach(x => rc.SelectTabCntrl._SelectedColCollection.Add(x));
                    //this.SelectTabUC.lstSelectedCol.ItemsSource = this.SelectTabUC._SelectedColCollection;//queryBuilder.SelectedColumns;
                }
            }

            if (queryBuilder.GroupByColumns.Count > 0)
            {

                //we have Tabluation but we need to find out what type of tabulation we have
                //***
                if (queryBuilder.CrossTabClause._col == null)
                {
                    //we have regular tabulation
                    //clear tabulation
                    rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Clear();
                    rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Clear();
                    //loading groupby
                    List<string> GroupByColumnNameList = new List<string>();
                    foreach (SQLBuilder.Clauses.Column groupByColumn in queryBuilder.GroupByColumns)
                    {
                        if (queryBuilder.OrderByStatement.Count == 0)
                        {
                            //we don't have oreder by
                            TabulationTabStackPanelGroupByControl tg = new TabulationTabStackPanelGroupByControl();
                            tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                            //modified on 11-18-11

                            tg.cmbTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                            tg.txtTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                            //tg.cmbTabulationTabGroupByColumnsName.SelectedIndex = 1;// groupByColumn;
                            // tg.cmbTabulationTabGroupByColumnsName.SelectedItem= groupByColumn;
                            tg.txtTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                            //add Grupby row
                            rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Add(tg);
                            GroupByColumnNameList.Add(groupByColumn.Name);
                        }
                        else
                        {
                            //we have orderby
                            foreach (SQLBuilder.Clauses.OrderByClause orderByClause in ((List<SQLBuilder.Clauses.OrderByClause>)queryBuilder.OrderByStatement))
                            {
                                if (groupByColumn.Name == orderByClause.FieldName)
                                {
                                    TabulationTabStackPanelGroupByControl tg = new TabulationTabStackPanelGroupByControl();
                                    tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                                    tg.cmbTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                                    tg.txtTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                                    tg.cmbTabulationSort.Text = Common.GetStringValueForEnum("Sorting", orderByClause.SortOrder);
                                    tg.txtTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                                    //add Grupby row
                                    rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Add(tg);
                                    GroupByColumnNameList.Add(groupByColumn.Name);
                                }
                            }
                        }
                    }
                    //we display total 3 rows in Group by secction,  we are going to add missing row
                    int numberOfMissingGroupByRow = 3 - rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count;
                    if (rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingGroupByRow; i++)
                        {
                            rc.TabulationTabCntrl.AddGroupByRow();
                            //loading group by drop down columns to just added group by row
                            ((TabulationTabStackPanelGroupByControl)rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count - 1]).cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc); ;
                        }

                    }
                    //load summary
                    foreach (SQLBuilder.Clauses.Column summaryColumn in queryBuilder.SummarizeColumns)
                    {
                        //foreach (string colName in GroupByColumnNameList)
                        //{
                        //    if (summaryColumn.Name != colName)
                        //    {
                        if (summaryColumn.Name.Contains("(") & summaryColumn.Name.Contains(")"))
                        {
                            TabulationTabStackPanelSummaryControl ts = new TabulationTabStackPanelSummaryControl();
                            ts.cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                            // first remove ')' from column name then spilt the string by'('
                            string[] summaryArr = summaryColumn.Name.Remove(summaryColumn.Name.Length - 1, 1).Split('(');
                            ts.cmbTabulationTabSummaryColumnsName.Text = summaryArr[1];
                            ts.cmbTabulationTypeOfSummary.SelectedIndex = Common.getIndex((List<string>)ts.cmbTabulationTypeOfSummary.ItemsSource, summaryArr[0]);
                            ts.cmbTabulationTabUserSelectSummaryColFormat.SelectedIndex = Common.getIndex((List<string>)ts.cmbTabulationTabUserSelectSummaryColFormat.ItemsSource, summaryColumn.Format);
                            ts.txtTabulationTabSummaryColFormat.Text = summaryColumn.Format;
                            ts.txtTabulationTabSummaryAlias.Text = summaryColumn.AliasName;
                            rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Add(ts);
                        }
                        //    }
                        //}
                    }
                    //we display total 6 rows in summary secction, we are going to add missing row
                    int numberOfMissingSummaryRow = 6 - rc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count;
                    if (rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingSummaryRow; i++)
                        {
                            rc.TabulationTabCntrl.AddSummaryRow();
                            //loading group by drop down columns to just added group by row
                            ((TabulationTabStackPanelSummaryControl)rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[rc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count - 1]).cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc); ;
                        }

                    }
                }
                else
                {
                    //we have cross tabulation
                    //clear cross tabulation tab
                    rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Clear();    //here
                    rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Clear();
                    //loading groupby
                    List<string> GroupByColumnNameList = new List<string>();
                    foreach (SQLBuilder.Clauses.Column groupByColumn in queryBuilder.GroupByColumns)
                    {
                        if (queryBuilder.OrderByStatement.Count == 0)
                        {
                            //we don't have oreder by
                            CrossTabulationTabStackPanelGroupByControl ctg = new CrossTabulationTabStackPanelGroupByControl();
                            ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                            ctg.cmbCrossTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                            ctg.txtCrossTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                            ctg.txtCrossTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                            //add Grupby row
                            rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Add(ctg);
                            GroupByColumnNameList.Add(groupByColumn.Name);
                        }
                        else
                        {
                            //we have orderby
                            foreach (SQLBuilder.Clauses.OrderByClause orderByClause in ((List<SQLBuilder.Clauses.OrderByClause>)queryBuilder.OrderByStatement))
                            {
                                if (groupByColumn.Name == orderByClause.FieldName)
                                {
                                    CrossTabulationTabStackPanelGroupByControl ctg = new CrossTabulationTabStackPanelGroupByControl();
                                    ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                                    ctg.cmbCrossTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                                    ctg.txtCrossTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                                    ctg.cmbCrossTabulationSort.Text = Common.GetStringValueForEnum("Sorting", orderByClause.SortOrder);
                                    ctg.txtCrossTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                                    //add Grupby row
                                    rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Add(ctg);
                                    GroupByColumnNameList.Add(groupByColumn.Name);
                                }
                            }
                        }
                    }
                    //we display total 3 rows in Group by secction,  we are going to add missing row
                    int numberOfMissingGroupByRow = 3 - rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count;
                    if (rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingGroupByRow; i++)
                        {
                            rc.CrossTabulationTabCntrl.AddGroupByRow();
                            //loading group by drop down columns to just added group by row
                            ((CrossTabulationTabStackPanelGroupByControl)rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count - 1]).cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc); ;
                        }

                    }
                    //load summary
                    foreach (SQLBuilder.Clauses.Column summaryColumn in queryBuilder.SummarizeColumns)
                    {
                        //foreach (string colName in GroupByColumnNameList)
                        //{
                        //    if (summaryColumn.Name != colName)
                        //    {
                        if (summaryColumn.Name.Contains("(") & summaryColumn.Name.Contains(")"))
                        {
                            CrossTabulationTabStackPanelSummaryControl cts = new CrossTabulationTabStackPanelSummaryControl();
                            cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                            // first remove ')' from column name then spilt the string by'('
                            string[] summaryArr = summaryColumn.Name.Remove(summaryColumn.Name.Length - 1, 1).Split('(');
                            cts.cmbCrossTabulationTabSummaryColumnsName.Text = summaryArr[1];
                            cts.txtCrossTabulationTabSummaryColFormat.Text = summaryColumn.Format;
                            cts.cmbCrossTabulationTypeOfSummary.SelectedIndex = Common.getIndex((List<string>)cts.cmbCrossTabulationTypeOfSummary.ItemsSource, summaryArr[0]);
                            cts.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedIndex = Common.getIndex((List<string>)cts.cmbCrossTabulationTabUserSelectSummaryColFormat.ItemsSource, summaryColumn.Format);
                            cts.txtCrossTabulationTabSummaryColFormat.Text = summaryColumn.Format;
                            cts.txtCrossTabulationTabSummaryAlias.Text = summaryColumn.AliasName;
                            rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Add(cts);
                        }
                        //    }
                        //}
                    }
                    //we display total 6 rows in summary secction, we are going to add missing row
                    int numberOfMissingSummaryRow = 6 - rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count;
                    if (this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingSummaryRow; i++)
                        {
                            rc.CrossTabulationTabCntrl.AddSummaryRow();
                            //loading group by drop down columns to just added group by row
                            ((CrossTabulationTabStackPanelSummaryControl)rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[rc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count - 1]).cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc); ;
                        }

                    }

                    //load summary frist row
                    rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = GenerateListOfTabulationTabCntrlColumns(rc);
                    rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.Text = queryBuilder.CrossTabClause.Col.Name;
                    if (queryBuilder.CrossTabClause.SortSet)
                    {
                        rc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.Text = Common.GetStringValueForEnum("Sorting", queryBuilder.CrossTabClause.SortOrder);
                    }

                }
                //********
            }


            int RowNumber = 0;
            //rc.WhereTabCntrl.StackPanelWhereTab.Children.Clear();
            rc.WhereTabCntrl.IsAllDisabled = false;
            rc.WhereTabCntrl.whereborder.Visibility = System.Windows.Visibility.Visible;
            foreach (SQLBuilder.Clauses.WhereClause whereClause in queryBuilder.Where)
            {
                RowNumber = RowNumber + 1;
                // create where Tab row control

                string controlType = whereClause.GetType().FullName;
                //List<string> Collist = (from x in (GenerateListOfTabulationTabCntrlColumns(rc))
                //                        select x.Name).ToList<string>();
                List<string> Collist = (from x in (GenerateListOfSelectTabCntrlColumns(rc))
                                        select x.Name).ToList<string>();
                if (controlType == "SQLBuilder.Clauses.BetweenWhereClause")
                {
                    //between where clause
                    SQLBuilder.Clauses.BetweenWhereClause Betweenclause = (SQLBuilder.Clauses.BetweenWhereClause)whereClause;
                    WhereTabBetweenConditionControl wsb = new WhereTabBetweenConditionControl();
                    if (RowNumber != 1)
                    {
                        if (Common.GetStringValueForEnum("LogicOperator", Betweenclause.LogicalOperator) == "None")
                        {
                            Betweenclause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                        }
                    }
                    wsb.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wsb.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", Betweenclause.LogicalOperator));
                    if (RowNumber == 1)
                    {
                        wsb.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                        wsb.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                    }
                    wsb.cmbWhereTabBetweenColumns.ItemsSource = Collist;// GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns( (List<SelectTabColumn>) this.SelectTabUC.dgSelectTab.ItemsSource);
                    wsb.cmbWhereTabBetweenColumns.Text = Betweenclause.FieldName;
                    //wsb.cmbWhereTabBetweenColumns.SelectedIndex = Common.getIndex((List<string>)wsb.cmbWhereTabBetweenColumns.ItemsSource, Betweenclause.FieldName);
                    wsb.txtBetweenLeftValue.Text = Betweenclause.FromValue;
                    wsb.txtBetweenRightValue.Text = Betweenclause.ToValue;
                    wsb.cmbWhereTabQueryLevel.SelectedIndex = (Betweenclause.Level - 1);
                    rc.WhereTabCntrl.StackPanelWhereTab.Children.Add(wsb);
                }

                else
                {
                    SQLBuilder.Clauses.GeneralWhereClause generalClause = (SQLBuilder.Clauses.GeneralWhereClause)whereClause;

                    if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.In)
                    {
                        //in where clause
                        WhereTabInNotInConditionControl wInNotIn = new WhereTabInNotInConditionControl();
                        if (RowNumber != 1)
                        {
                            if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                            {
                                generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                            }
                        }
                        wInNotIn.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wInNotIn.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                        if (RowNumber == 1)
                        {
                            wInNotIn.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                            wInNotIn.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                        }
                        wInNotIn.cmbWhereTabInNotInColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns( (List<SelectTabColumn>) this.SelectTabUC.dgSelectTab.ItemsSource);
                        wInNotIn.cmbWhereTabInNotInColumns.Text = generalClause.FieldName;
                        wInNotIn.lblInNotIn.Content = "    in"; ;
                        wInNotIn.txtInNotInValue.Text = generalClause.Value.ToString();
                        wInNotIn.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                        rc.WhereTabCntrl.StackPanelWhereTab.Children.Add(wInNotIn);
                    }

                    else if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.NotIn)
                    {
                        //not in where clause
                        WhereTabInNotInConditionControl wInNotIn2 = new WhereTabInNotInConditionControl();
                        if (RowNumber != 1)
                        {
                            if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                            {
                                generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                            }
                        }
                        wInNotIn2.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wInNotIn2.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                        if (RowNumber == 1)
                        {
                            wInNotIn2.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                            wInNotIn2.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                        }
                        wInNotIn2.cmbWhereTabInNotInColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                        wInNotIn2.cmbWhereTabInNotInColumns.Text = generalClause.FieldName;
                        wInNotIn2.lblInNotIn.Content = "not in";
                        wInNotIn2.txtInNotInValue.Text = generalClause.Value.ToString();
                        wInNotIn2.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                        rc.WhereTabCntrl.StackPanelWhereTab.Children.Add(wInNotIn2);
                    }

                    else if (generalClause.Value == null)
                    {

                        if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.Equals)
                        {
                            // is null where clause
                            WhereTabNullNotNullConditionControl wNullNotNull = new WhereTabNullNotNullConditionControl();
                            if (RowNumber != 1)
                            {
                                if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                                {
                                    generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                                }
                            }
                            wNullNotNull.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wNullNotNull.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                            if (RowNumber == 1)
                            {
                                wNullNotNull.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wNullNotNull.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                            }
                            wNullNotNull.cmbWhereTabNullNotNullColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                            wNullNotNull.cmbWhereTabNullNotNullColumns.Text = generalClause.FieldName;
                            wNullNotNull.lblNullNotNull.Content = "null";
                            wNullNotNull.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                            rc.WhereTabCntrl.StackPanelWhereTab.Children.Add(wNullNotNull);
                        }

                        else if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.NotEquals)
                        {
                            // is not null where clause
                            WhereTabNullNotNullConditionControl wNullNotNull2 = new WhereTabNullNotNullConditionControl();
                            if (RowNumber != 1)
                            {
                                if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                                {
                                    generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                                }
                            }
                            wNullNotNull2.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wNullNotNull2.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                            if (RowNumber == 1)
                            {
                                wNullNotNull2.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wNullNotNull2.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                            }

                            //IEnumerable<Column> columnsWithSameAlias = _SelectedColCollection.Where(x => x.AliasName == compuatedCol.AliasName)

                            wNullNotNull2.cmbWhereTabNullNotNullColumns.ItemsSource = Collist; //GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                            wNullNotNull2.cmbWhereTabNullNotNullColumns.Text = generalClause.FieldName;
                            wNullNotNull2.lblNullNotNull.Content = "not null";
                            wNullNotNull2.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                            rc.WhereTabCntrl.StackPanelWhereTab.Children.Add(wNullNotNull2);
                        }
                    }
                    else
                    {
                        //Regular where clause
                        WhereTabRegularConditionControl ws = new WhereTabRegularConditionControl();
                        if (RowNumber != 1)
                        {
                            if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                            {
                                generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                            }
                        }
                        ws.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                        if (RowNumber == 1)
                        {
                            ws.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                            ws.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                        }
                        ws.cmbWhereTabLeftSideColumns.ItemsSource = Collist;// GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                        ws.cmbWhereTabLeftSideColumns.Text = generalClause.FieldName;
                        ///ws.cmbWhereTabLeftSideColumns.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabLeftSideColumns.ItemsSource, generalClause.FieldName);
                        ws.cmbWhereTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabQueryOpretor.ItemsSource, Common.GetStringValueForEnum("Comparison", generalClause.ComparisonOperator));
                        ws.cmbWhereTabRightSideColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                        ws.cmbWhereTabRightSideColumns.Text = generalClause.Value.ToString();
                        //ws.cmbWhereTabRightSideColumns.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabRightSideColumns.ItemsSource, generalClause.Value.ToString());
                        ws.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                        rc.WhereTabCntrl.StackPanelWhereTab.Children.Add(ws);
                    }
                }
            }
            rc.WhereTabCntrl.cmbWherTabCondition.SelectedIndex = 0;
            //make all tab validated
            rc.FromTabCntrl.isValidated = true;
            rc.FromTabCntrl.lblErrorMessage.Content = "";
            rc.WhereTabCntrl.isValidated = true;
            rc.WhereTabCntrl.lblErrorMessage.Content = "";
            rc.SelectTabCntrl.isValidated = true;
            rc.SelectTabCntrl.lblErrorMessage.Content = "";
            rc.SelectTabCntrl.lstToSelecteColFrom.IsEnabled = true;
            rc.TabulationTabCntrl.lblErrorMessage.Content = "";
            rc.TabulationTabCntrl.isValidated = true;
            rc.CrossTabulationTabCntrl.lblErrorMessage.Content = "";
            rc.CrossTabulationTabCntrl.isValidated = true;
            Mouse.OverrideCursor = null;
            //Clear Action Tab
            this.txtQuery.Text = System.String.Empty;
            count = count + 1;
        }
        /************************************************zahed*****************************************/

        //loads right hand side custom query tabs
        private void LoadAllCustomQueryTabs(SQLBuilder.SelectQueryBuilder queryBuilder, bool isGetResultByTreeView)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            //Clear all tabs
            ClearAllCustomeQueryTabs();
            //make all tab validated

            if (listOfTable != null)
            {
                this.FromTabUC.cmbFromTable.ItemsSource = Common.ConvertTablesToStringList(listOfTable);
            }
            this.FromTabUC.cmbFromTable.SelectedIndex = Common.getIndex((List<string>)this.FromTabUC.cmbFromTable.ItemsSource, queryBuilder.SelectedTables[0].Name);
            this.FromTabUC.txtFromAlias.Text = queryBuilder.SelectedTables[0].AliasName;
            foreach (SQLBuilder.Clauses.JoinClause JoinClasue in queryBuilder.Joins)
            {
                // create join row control
                FromTabStackPanelControl fc = new FromTabStackPanelControl();
                if (this.listOfTable != null & this.listOfTable.Count > 0)
                {
                    fc.cmbFromTabJoinTable.ItemsSource = Common.ConvertTablesToStringList(this.listOfTable);
                    fc.cmbFromTabFromColumns.ItemsSource = Common.ConvertColumsToStringList(this.listOfTable[this.FromTabUC.cmbFromTable.SelectedIndex].columns);
                }
                fc.cmbFromTabJoinType.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabJoinType.ItemsSource, JoinClasue.JoinType.ToString());
                fc.cmbFromTabJoinTable.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabJoinTable.ItemsSource, JoinClasue.ToTable.Name);
                //fc.cmbFromTabJoinTable.Text = JoinClasue.ToTable.Name;
                fc.txtJoinTableAlias.Text = JoinClasue.ToTable.AliasName;
                fc.cmbFromTabFromColumns.Text = JoinClasue.FromColumn.ToString();
                //fc.cmbFromTabFromColumns.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabFromColumns.ItemsSource, JoinClasue.FromColumn.ToString());
                fc.cmbFromTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabQueryOpretor.ItemsSource, JoinClasue.ComparisonOperator.ToString());
                //fc.cmbFromTabJoinColumns.ItemsSource = Common.ConvertColumsToStringList(this.listOfTable[fc.cmbFromTabJoinTable.SelectedIndex].columns);
                fc.cmbFromTabJoinColumns.Text = JoinClasue.ToColumn.ToString();
                //fc.cmbFromTabJoinColumns.SelectedIndex = Common.getIndex((List<string>)fc.cmbFromTabJoinColumns.ItemsSource, JoinClasue.ToColumn.ToString());
                this.FromTabUC.StackPanelFromTab.Children.Add(fc);
            }


            //load select tab
            List<SQLBuilder.Clauses.Column> ColListForSelectTab = GenerateListOfSelectTabColumns();
            // seting up select Tab's observabla collection _FromSelectedColToCollection
            //ColListForSelectTab.ForEach(x => this.SelectTabUC._FromSelectedColToCollection.Add(x));
            this.SelectTabUC.lstToSelecteColFrom.ItemsSource = ColListForSelectTab;

            //clear old selected columns
            this.SelectTabUC._SelectedColCollection.Clear();
            //load selected Column List and set  _SelectedColCollection observable collection from queryBuilder.SelectedColumns
            //Regex regex = new Regex("(?<X>[.]+).(?<Z>)");// this to check if queryBuilder is not generated from custome query, so if user selects table from Tree view we don't need to show selected columns
            if (queryBuilder.SelectedColumns.Count != 0)
            {
                if (!isGetResultByTreeView)
                {
                    //first we copy  queryBuilder.SelectedColumns from generic list to observable collection
                    queryBuilder.SelectedColumns.ForEach(x => this.SelectTabUC._SelectedColCollection.Add(x));
                    //this.SelectTabUC.lstSelectedCol.ItemsSource = this.SelectTabUC._SelectedColCollection;//queryBuilder.SelectedColumns;
                }
            }

            //load select tab or tabulation tab 
            if (queryBuilder.GroupByColumns.Count > 0)
            {

                //we have Tabluation but we need to find out what type of tabulation we have
                //***
                if (queryBuilder.CrossTabClause._col == null)
                {
                    //we have regular tabulation
                    //clear tabulation
                    this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Clear();
                    this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Clear();
                    //loading groupby
                    List<string> GroupByColumnNameList = new List<string>();
                    foreach (SQLBuilder.Clauses.Column groupByColumn in queryBuilder.GroupByColumns)
                    {
                        if (queryBuilder.OrderByStatement.Count == 0)
                        {
                            //we don't have oreder by
                            TabulationTabStackPanelGroupByControl tg = new TabulationTabStackPanelGroupByControl();
                            tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                            //modified on 11-18-11

                            tg.cmbTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                            tg.txtTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                            //tg.cmbTabulationTabGroupByColumnsName.SelectedIndex = 1;// groupByColumn;
                            // tg.cmbTabulationTabGroupByColumnsName.SelectedItem= groupByColumn;
                            tg.txtTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                            //add Grupby row
                            this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Add(tg);
                            GroupByColumnNameList.Add(groupByColumn.Name);
                        }
                        else
                        {
                            //we have orderby
                            foreach (SQLBuilder.Clauses.OrderByClause orderByClause in ((List<SQLBuilder.Clauses.OrderByClause>)queryBuilder.OrderByStatement))
                            {
                                if (groupByColumn.Name == orderByClause.FieldName)
                                {
                                    TabulationTabStackPanelGroupByControl tg = new TabulationTabStackPanelGroupByControl();
                                    tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                                    tg.cmbTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                                    tg.txtTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                                    tg.cmbTabulationSort.Text = Common.GetStringValueForEnum("Sorting", orderByClause.SortOrder);
                                    tg.txtTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                                    //add Grupby row
                                    this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Add(tg);
                                    GroupByColumnNameList.Add(groupByColumn.Name);
                                }
                            }
                        }
                    }
                    //we display total 3 rows in Group by secction,  we are going to add missing row
                    int numberOfMissingGroupByRow = 3 - this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count;
                    if (this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingGroupByRow; i++)
                        {
                            this.TabulationTabUC.AddGroupByRow();
                            //loading group by drop down columns to just added group by row
                            ((TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count - 1]).cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns(); ;
                        }

                    }
                    //load summary
                    foreach (SQLBuilder.Clauses.Column summaryColumn in queryBuilder.SelectedColumns)
                    {
                        //foreach (string colName in GroupByColumnNameList)
                        //{
                        //    if (summaryColumn.Name != colName)
                        //    {
                        if (summaryColumn.Name.Contains("(") & summaryColumn.Name.Contains(")"))
                        {
                            TabulationTabStackPanelSummaryControl ts = new TabulationTabStackPanelSummaryControl();
                            ts.cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                            // first remove ')' from column name then spilt the string by'('
                            string[] summaryArr = summaryColumn.Name.Remove(summaryColumn.Name.Length - 1, 1).Split('(');
                            ts.cmbTabulationTabSummaryColumnsName.Text = summaryArr[1];
                            ts.cmbTabulationTypeOfSummary.SelectedIndex = Common.getIndex((List<string>)ts.cmbTabulationTypeOfSummary.ItemsSource, summaryArr[0]);
                            ts.cmbTabulationTabUserSelectSummaryColFormat.SelectedIndex = Common.getIndex((List<string>)ts.cmbTabulationTabUserSelectSummaryColFormat.ItemsSource, summaryColumn.Format);
                            ts.txtTabulationTabSummaryColFormat.Text = summaryColumn.Format;
                            ts.txtTabulationTabSummaryAlias.Text = summaryColumn.AliasName;
                            this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Add(ts);
                        }
                        //    }
                        //}
                    }
                    //we display total 6 rows in summary secction, we are going to add missing row
                    int numberOfMissingSummaryRow = 6 - this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count;
                    if (this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingSummaryRow; i++)
                        {
                            this.TabulationTabUC.AddSummaryRow();
                            //loading group by drop down columns to just added group by row
                            ((TabulationTabStackPanelSummaryControl)this.TabulationTabUC.StackPanelTabuLationTabSummary.Children[this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count - 1]).cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns(); ;
                        }

                    }
                }
                else
                {
                    //we have cross tabulation
                    //clear cross tabulation tab
                    this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Clear();
                    this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Clear();
                    //loading groupby
                    List<string> GroupByColumnNameList = new List<string>();
                    foreach (SQLBuilder.Clauses.Column groupByColumn in queryBuilder.GroupByColumns)
                    {
                        if (queryBuilder.OrderByStatement.Count == 0)
                        {
                            //we don't have oreder by
                            CrossTabulationTabStackPanelGroupByControl ctg = new CrossTabulationTabStackPanelGroupByControl();
                            ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                            ctg.cmbCrossTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                            ctg.txtCrossTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                            ctg.txtCrossTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                            //add Grupby row
                            this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Add(ctg);
                            GroupByColumnNameList.Add(groupByColumn.Name);
                        }
                        else
                        {
                            //we have orderby
                            foreach (SQLBuilder.Clauses.OrderByClause orderByClause in ((List<SQLBuilder.Clauses.OrderByClause>)queryBuilder.OrderByStatement))
                            {
                                if (groupByColumn.Name == orderByClause.FieldName)
                                {
                                    CrossTabulationTabStackPanelGroupByControl ctg = new CrossTabulationTabStackPanelGroupByControl();
                                    ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                                    ctg.cmbCrossTabulationTabGroupByColumnsName.Text = groupByColumn.Name;
                                    ctg.txtCrossTabulationTabGroupByColFormat.Text = groupByColumn.Format;
                                    ctg.cmbCrossTabulationSort.Text = Common.GetStringValueForEnum("Sorting", orderByClause.SortOrder);
                                    ctg.txtCrossTabulationTabGroupByAlias.Text = groupByColumn.AliasName;
                                    //add Grupby row
                                    this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Add(ctg);
                                    GroupByColumnNameList.Add(groupByColumn.Name);
                                }
                            }
                        }
                    }
                    //we display total 3 rows in Group by secction,  we are going to add missing row
                    int numberOfMissingGroupByRow = 3 - this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count;
                    if (this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingGroupByRow; i++)
                        {
                            this.CrossTabulationTabUC.AddGroupByRow();
                            //loading group by drop down columns to just added group by row
                            ((CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count - 1]).cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns(); ;
                        }

                    }
                    //load summary
                    foreach (SQLBuilder.Clauses.Column summaryColumn in queryBuilder.SelectedColumns)
                    {
                        //foreach (string colName in GroupByColumnNameList)
                        //{
                        //    if (summaryColumn.Name != colName)
                        //    {
                        if (summaryColumn.Name.Contains("(") & summaryColumn.Name.Contains(")"))
                        {
                            CrossTabulationTabStackPanelSummaryControl cts = new CrossTabulationTabStackPanelSummaryControl();
                            cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                            // first remove ')' from column name then spilt the string by'('
                            string[] summaryArr = summaryColumn.Name.Remove(summaryColumn.Name.Length - 1, 1).Split('(');
                            cts.cmbCrossTabulationTabSummaryColumnsName.Text = summaryArr[1];
                            cts.txtCrossTabulationTabSummaryColFormat.Text = summaryColumn.Format;
                            cts.cmbCrossTabulationTypeOfSummary.SelectedIndex = Common.getIndex((List<string>)cts.cmbCrossTabulationTypeOfSummary.ItemsSource, summaryArr[0]);
                            cts.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedIndex = Common.getIndex((List<string>)cts.cmbCrossTabulationTabUserSelectSummaryColFormat.ItemsSource, summaryColumn.Format);
                            cts.txtCrossTabulationTabSummaryColFormat.Text = summaryColumn.Format;
                            cts.txtCrossTabulationTabSummaryAlias.Text = summaryColumn.AliasName;
                            this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Add(cts);
                        }
                        //    }
                        //}
                    }
                    //we display total 6 rows in summary secction, we are going to add missing row
                    int numberOfMissingSummaryRow = 6 - this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count;
                    if (this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count != 0)
                    {
                        for (int i = 0; i < numberOfMissingSummaryRow; i++)
                        {
                            this.CrossTabulationTabUC.AddSummaryRow();
                            //loading group by drop down columns to just added group by row
                            ((CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children[this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count - 1]).cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns(); ;
                        }

                    }

                    //load summary frist row
                    this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                    this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.Text = queryBuilder.CrossTabClause.Col.Name;
                    if (queryBuilder.CrossTabClause.SortSet)
                    {
                        this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFristRowSort.Text = Common.GetStringValueForEnum("Sorting", queryBuilder.CrossTabClause.SortOrder);
                    }

                }
                //********
            }
            else
            {
                //we have select
                //clear old regular tabulation
                //clear group by rows
                for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.TabulationTabUC.StackPanelTabuLationTabGroupBy.Children[i];
                    tg.cmbTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                    tg.cmbTabulationTabGroupByColumnsName.SelectedIndex = -1;
                    tg.cmbTabulationSort.SelectedIndex = -1;
                    tg.txtTabulationTabGroupByAlias.Text = System.String.Empty;
                }
                //clear summary rows
                for (int i = 0; i < this.TabulationTabUC.StackPanelTabuLationTabSummary.Children.Count; i++)
                {
                    TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.TabulationTabUC.StackPanelTabuLationTabSummary.Children[i];
                    ts.cmbTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                    ts.cmbTabulationTabSummaryColumnsName.SelectedIndex = -1;
                    ts.cmbTabulationTypeOfSummary.SelectedIndex = -1;
                    ts.cmbTabulationTabUserSelectSummaryColFormat.SelectedIndex = -1;
                    ts.txtTabulationTabSummaryColFormat.Text = System.String.Empty;
                    ts.txtTabulationTabSummaryAlias.Text = System.String.Empty;

                }
                //clear old Cross tabulation
                //clear group by rows
                for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabGroupBy.Children[i];
                    ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                    ctg.cmbCrossTabulationTabGroupByColumnsName.SelectedIndex = -1;
                    ctg.cmbCrossTabulationSort.SelectedIndex = -1;
                    ctg.txtCrossTabulationTabGroupByAlias.Text = System.String.Empty;
                }
                //clear summary rows
                for (int i = 0; i < this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.CrossTabulationTabUC.StackPanelCrossTabuLationTabSummary.Children[i];
                    cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                    cts.cmbCrossTabulationTabSummaryColumnsName.SelectedIndex = -1;
                    cts.cmbCrossTabulationTypeOfSummary.SelectedIndex = -1;
                    cts.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedIndex = -1;
                    cts.txtCrossTabulationTabSummaryColFormat.Text = System.String.Empty;
                    cts.txtCrossTabulationTabSummaryAlias.Text = System.String.Empty;

                }
                // clear summary frist row
                this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = GenerateListOfTabulationTabColumns();
                this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectedIndex = -1;
                this.CrossTabulationTabUC.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex = -1;

                //load select tab
                List<SQLBuilder.Clauses.Column> colListForSelectTab = GenerateListOfSelectTabColumns();
                // seting up select Tab's observabla collection _FromSelectedColToCollection
                //ColListForSelectTab.ForEach(x => this.SelectTabUC._FromSelectedColToCollection.Add(x));
                this.SelectTabUC.lstToSelecteColFrom.ItemsSource = colListForSelectTab;


                //clear old selected columns
                this.SelectTabUC._SelectedColCollection.Clear();
                //load selected Column List and set  _SelectedColCollection observable collection from queryBuilder.SelectedColumns
                //Regex regex = new Regex("(?<X>[.]+).(?<Z>)");// this to check if queryBuilder is not generated from custome query, so if user selects table from Tree view we don't need to show selected columns
                if (queryBuilder.SelectedColumns.Count != 0)
                {
                    if (!isGetResultByTreeView)
                    {
                        //first we copy  queryBuilder.SelectedColumns from generic list to observable collection
                        queryBuilder.SelectedColumns.ForEach(x => this.SelectTabUC._SelectedColCollection.Add(x));
                        //this.SelectTabUC.lstSelectedCol.ItemsSource = this.SelectTabUC._SelectedColCollection;//queryBuilder.SelectedColumns;
                    }
                }
            }

            //load where tab
            int RowNumber = 0;
            foreach (SQLBuilder.Clauses.WhereClause whereClause in queryBuilder.Where)
            {
                RowNumber = RowNumber + 1;
                // create where Tab row control

                string controlType = whereClause.GetType().FullName;
                //List<string> Collist = (from x in (GenerateListOfTabulationTabColumns())
                //                        select x.Name).ToList<string>();
                List<string> Collist = (from x in (GenerateListOfSelectTabColumns())
                                        select x.Name).ToList<string>();
                if (controlType == "SQLBuilder.Clauses.BetweenWhereClause")
                {
                    //between where clause
                    SQLBuilder.Clauses.BetweenWhereClause Betweenclause = (SQLBuilder.Clauses.BetweenWhereClause)whereClause;
                    WhereTabBetweenConditionControl wsb = new WhereTabBetweenConditionControl();
                    if (RowNumber != 1)
                    {
                        if (Common.GetStringValueForEnum("LogicOperator", Betweenclause.LogicalOperator) == "None")
                        {
                            Betweenclause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                        }
                    }
                    wsb.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wsb.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", Betweenclause.LogicalOperator));
                    if (RowNumber == 1)
                    {
                        wsb.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                        wsb.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                    }
                    wsb.cmbWhereTabBetweenColumns.ItemsSource = Collist;// GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns( (List<SelectTabColumn>) this.SelectTabUC.dgSelectTab.ItemsSource);
                    wsb.cmbWhereTabBetweenColumns.Text = Betweenclause.FieldName;
                    //wsb.cmbWhereTabBetweenColumns.SelectedIndex = Common.getIndex((List<string>)wsb.cmbWhereTabBetweenColumns.ItemsSource, Betweenclause.FieldName);
                    wsb.txtBetweenLeftValue.Text = Betweenclause.FromValue;
                    wsb.txtBetweenRightValue.Text = Betweenclause.ToValue;
                    wsb.cmbWhereTabQueryLevel.SelectedIndex = (Betweenclause.Level - 1);
                    this.WhereTabUC.StackPanelWhereTab.Children.Add(wsb);
                }

                else
                {
                    SQLBuilder.Clauses.GeneralWhereClause generalClause = (SQLBuilder.Clauses.GeneralWhereClause)whereClause;

                    if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.In)
                    {
                        //in where clause
                        WhereTabInNotInConditionControl wInNotIn = new WhereTabInNotInConditionControl();
                        if (RowNumber != 1)
                        {
                            if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                            {
                                generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                            }
                        }
                        wInNotIn.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wInNotIn.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                        if (RowNumber == 1)
                        {
                            wInNotIn.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                            wInNotIn.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                        }
                        wInNotIn.cmbWhereTabInNotInColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns( (List<SelectTabColumn>) this.SelectTabUC.dgSelectTab.ItemsSource);
                        wInNotIn.cmbWhereTabInNotInColumns.Text = generalClause.FieldName;
                        wInNotIn.lblInNotIn.Content = "    in"; ;
                        wInNotIn.txtInNotInValue.Text = generalClause.Value.ToString();
                        wInNotIn.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                        this.WhereTabUC.StackPanelWhereTab.Children.Add(wInNotIn);
                    }

                    else if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.NotIn)
                    {
                        //not in where clause
                        WhereTabInNotInConditionControl wInNotIn2 = new WhereTabInNotInConditionControl();
                        if (RowNumber != 1)
                        {
                            if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                            {
                                generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                            }
                        }
                        wInNotIn2.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wInNotIn2.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                        if (RowNumber == 1)
                        {
                            wInNotIn2.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                            wInNotIn2.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                        }
                        wInNotIn2.cmbWhereTabInNotInColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                        wInNotIn2.cmbWhereTabInNotInColumns.Text = generalClause.FieldName;
                        wInNotIn2.lblInNotIn.Content = "not in";
                        wInNotIn2.txtInNotInValue.Text = generalClause.Value.ToString();
                        wInNotIn2.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                        this.WhereTabUC.StackPanelWhereTab.Children.Add(wInNotIn2);
                    }

                    else if (generalClause.Value == null)
                    {

                        if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.Equals)
                        {
                            // is null where clause
                            WhereTabNullNotNullConditionControl wNullNotNull = new WhereTabNullNotNullConditionControl();
                            if (RowNumber != 1)
                            {
                                if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                                {
                                    generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                                }
                            }
                            wNullNotNull.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wNullNotNull.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                            if (RowNumber == 1)
                            {
                                wNullNotNull.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wNullNotNull.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                            }
                            wNullNotNull.cmbWhereTabNullNotNullColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                            wNullNotNull.cmbWhereTabNullNotNullColumns.Text = generalClause.FieldName;
                            wNullNotNull.lblNullNotNull.Content = "null";
                            wNullNotNull.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                            this.WhereTabUC.StackPanelWhereTab.Children.Add(wNullNotNull);
                        }

                        else if (generalClause.ComparisonOperator == SQLBuilder.Enums.Comparison.NotEquals)
                        {
                            // is not null where clause
                            WhereTabNullNotNullConditionControl wNullNotNull2 = new WhereTabNullNotNullConditionControl();
                            if (RowNumber != 1)
                            {
                                if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                                {
                                    generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                                }
                            }
                            wNullNotNull2.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)wNullNotNull2.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                            if (RowNumber == 1)
                            {
                                wNullNotNull2.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wNullNotNull2.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                            }

                            //IEnumerable<Column> columnsWithSameAlias = _SelectedColCollection.Where(x => x.AliasName == compuatedCol.AliasName)

                            wNullNotNull2.cmbWhereTabNullNotNullColumns.ItemsSource = Collist; //GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                            wNullNotNull2.cmbWhereTabNullNotNullColumns.Text = generalClause.FieldName;
                            wNullNotNull2.lblNullNotNull.Content = "not null";
                            wNullNotNull2.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                            this.WhereTabUC.StackPanelWhereTab.Children.Add(wNullNotNull2);
                        }
                    }
                    else
                    {
                        //Regular where clause
                        WhereTabRegularConditionControl ws = new WhereTabRegularConditionControl();
                        if (RowNumber != 1)
                        {
                            if (Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator) == "None")
                            {
                                generalClause.LogicalOperator = SQLBuilder.Enums.LogicOperator.And;
                            }
                        }
                        ws.cmbWhereTabQueryAndOr.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabQueryAndOr.ItemsSource, Common.GetStringValueForEnum("LogicOperator", generalClause.LogicalOperator));
                        if (RowNumber == 1)
                        {
                            ws.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                            ws.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                        }
                        ws.cmbWhereTabLeftSideColumns.ItemsSource = Collist;// GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                        ws.cmbWhereTabLeftSideColumns.Text = generalClause.FieldName;
                        ///ws.cmbWhereTabLeftSideColumns.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabLeftSideColumns.ItemsSource, generalClause.FieldName);
                        ws.cmbWhereTabQueryOpretor.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabQueryOpretor.ItemsSource, Common.GetStringValueForEnum("Comparison", generalClause.ComparisonOperator));
                        ws.cmbWhereTabRightSideColumns.ItemsSource = Collist;//GenerateListOfTabulationTabColumns();//Common.GetWhereClauseDropDownColumns((List<SelectTabColumn>)this.SelectTabUC.dgSelectTab.ItemsSource);
                        ws.cmbWhereTabRightSideColumns.Text = generalClause.Value.ToString();
                        //ws.cmbWhereTabRightSideColumns.SelectedIndex = Common.getIndex((List<string>)ws.cmbWhereTabRightSideColumns.ItemsSource, generalClause.Value.ToString());
                        ws.cmbWhereTabQueryLevel.SelectedIndex = (generalClause.Level - 1);
                        this.WhereTabUC.StackPanelWhereTab.Children.Add(ws);
                    }
                }
            }
            this.WhereTabUC.cmbWherTabCondition.SelectedIndex = 0;
            //make all tab validated
            this.FromTabUC.isValidated = true;
            this.FromTabUC.lblErrorMessage.Content = "";
            this.WhereTabUC.isValidated = true;
            this.WhereTabUC.lblErrorMessage.Content = "";
            this.SelectTabUC.isValidated = true;
            this.SelectTabUC.lblErrorMessage.Content = "";
            this.SelectTabUC.lstToSelecteColFrom.IsEnabled = true;
            this.TabulationTabUC.lblErrorMessage.Content = "";
            this.TabulationTabUC.isValidated = true;
            this.CrossTabulationTabUC.lblErrorMessage.Content = "";
            this.CrossTabulationTabUC.isValidated = true;
            Mouse.OverrideCursor = null;
            //Clear Action Tab
            this.txtQuery.Text = System.String.Empty;
        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
        }

        private void menuItemViewTreeView_Click(object sender, RoutedEventArgs e)
        {
            this.TreeViewDocPanel.Width = 199;
            TreeViewHideStackPanel.Width = 0;

            this.adornedControl.IsAdornerVisible = false;
            //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Visible;
        }

        private void menuItemViewCustomQuery_Click(object sender, RoutedEventArgs e)
        {
            this.adornedControl.IsAdornerVisible = true;
            DocPanelCustomQuery.HorizontalAlignment = HorizontalAlignment.Stretch;
            DocPanelCustomQuery.Margin = new Thickness(0, 60, 0, 0);
            DocPanelCustomQuery.Width =
            Double.NaN;
            //this.BtnHideCustomQueryWindow.Visibility = System.Windows.Visibility.Hidden;

        }

        private void menuItemExportToPDF_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            CloseableTabItem closeAbleTabItem = (CloseableTabItem)this.tabControl1.SelectedItem;
            ResultViewControl rc = (ResultViewControl)closeAbleTabItem.Content;
            rc.ExportToExcelFile();
            //CloseableTabItem closeAbleTabItem = (CloseableTabItem)this.tabControl1.SelectedItem;
            //if (closeAbleTabItem != null)
            //{
            //    if (closeAbleTabItem.Name != "tabItem1")
            //    {
            //         ResultViewControl rc = (ResultViewControl)closeAbleTabItem.Content;
            //         if (rc != null)
            //         {
            //             ResultViewModel rv = (ResultViewModel)rc.DataContext;
            //             if (rv.Results != null)
            //             {
            //                 try
            //                 {
            //                     string connectionString = ConfigurationManager.AppSettings["DefaultDBConn"] + "database=" + CurrentDatabaseName + ";";
            //                     System.Data.DataView allRecordsView = MySQLData.DataAccess.ADODataBridge.getData(connectionString, rv.QueryBulder.BuildQuery()).DefaultView;
            //                     string filePath = string.Empty;
            //                     Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            //                     sfd.DefaultExt = ".xlsx";
            //                     sfd.Filter = "Default (.xlsx)|*.xlsx";
            //                     if (sfd.ShowDialog() == true)
            //                     {
            //                         filePath = sfd.FileName;
            //                         //this is to Export records from Datagrid result
            //                         //Common.ExportToCSV(rv.Results, filePath);
            //                         //this is to Export all records from Database by runing th query bulilder
            //                         //Common.ExportToCSV(allRecordsView, filePath);
            //                         //Common.CreateExcelDocument(allRecordsView.Table, rv.QueryBulder, filePath);
            //                         Common.CreateSpreadsheetWorkbook(filePath, allRecordsView.Table,rv.QueryBulder.FinalSelectedColumns);
            //                     }
            //                 }
            //                 catch (Exception ex)
            //                 {
            //                     if (isErrorLoggingOn)
            //                     {
            //                         LogError.Log_Err("menuItemExportToExcel_Click", ex);
            //                         DisplayErrorMessage();
            //                     }
            //                 }
            //             }
            //         }
            //    }
            //}
        }

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {

            AboutBox1 about = new AboutBox1();

            about.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            bool isThereUnsavedTab = false;
            foreach (TabItem tabItem in tabControl1.Items)
            {
                CloseableTabItem tbItem = (CloseableTabItem)tabItem;
                if (tbItem.Part_Label_Content == "*")
                {
                    isThereUnsavedTab = true;
                    break;
                }
            }
            if (isThereUnsavedTab)
            {
                if (MessageBox.Show("Are sure you want to close this window? You have one or more unsaved query", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    // Do not close the window
                    e.Cancel = true;
                }


            }
        }
        private void DisplayErrorMessage()
        {
            MessageBox.Show(ConfigurationManager.AppSettings["ErrorMessage"]);
        }
        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void menuItemImportScript_Click(object sender, RoutedEventArgs e)
        {
            //var newW = new ScriptGenerator();
            var newW = new ScriptGen();
            newW.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            newW.Show();
        }

        //private void BtnHideTreView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    this.TreeViewDocPanel.Width = 199;
        //    TreeViewHideStackPanel.Width = 0;
        //    this.adornedControl.IsAdornerVisible = false;
        //}
    }
}
