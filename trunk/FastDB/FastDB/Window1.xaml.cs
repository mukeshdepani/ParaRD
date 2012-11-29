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

namespace FastDB
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string CurrentDatabaseName;         //made public zahed
        private string connectionString;
        public Control.ResultViewControl resultControlInit;
        public List<MySQLData.Table> listOfTable;
        private bool isErrorLoggingOn = Convert.ToBoolean(ConfigurationManager.AppSettings["isErrorLoggingOn"].ToString());
        public Window1(Control.ResultViewControl resultControl)
        {
            resultControlInit = resultControl;
            InitializeComponent();
            CenterWindowOnScreen();
            connectionString = ConfigurationManager.AppSettings["DefaultDBConn"]; //System.Configuration.ConfigurationSettings.AppSettings["FastDBConn"];
            CurrentDatabaseName = ConfigurationManager.AppSettings["DefaultDatabase"];

            connectionString = connectionString + "Database=" + CurrentDatabaseName + ";";
            MainWindow m = new MainWindow();
            try
            {
                List<Schema> schemas = MySQLData.DataAccess.ADODataBridge.getSchemaTree(connectionString, CurrentDatabaseName, ConfigurationManager.AppSettings["DerivedTablesPath"]);//DataAccess.GetDatabases();
                MainViewModel viewModel = new MainViewModel(schemas);
                
                m.MainTreeView.DataContext = viewModel;
                listOfTable = new List<MySQLData.Table>();
                foreach (Schema schema in schemas)
                {
                    listOfTable.AddRange(schema.tables);
                }
                if (listOfTable != null)
                {

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
            var collView = CollectionViewSource.GetDefaultView(m.tabControlCustomQuery.Items);
            collView.CurrentChanging +=  this.OnTabItemSelecting;

        }
        private void OnTabItemSelecting(Object sender, CurrentChangingEventArgs e)
        {

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
        private void RVCTAbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            resultControlInit.SaveXMLFile(txtFileName.Text);
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        private void DisplayErrorMessage()
        {
            MessageBox.Show(ConfigurationManager.AppSettings["ErrorMessage"]);
        }
    }
    

}
