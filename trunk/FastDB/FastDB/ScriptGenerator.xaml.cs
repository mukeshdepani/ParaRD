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
    /// Interaction logic for ScriptGenerator.xaml
    /// </summary>
    public partial class ScriptGenerator : Window
    {
        //public ObservableCollection<string> Datatype { get; set; }
        List<Model> list = new List<Model>();
        public static int TotalCount;
        public ScriptGenerator()
        {
            InitializeComponent();
            PopulateBlankGrid();
        }
        void dgScriptData_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            //if (e.Column == colStateCandiate)
            //{
            //    DataGridCell cell = e.Column.GetCellContent(e.Row).Parent as DataGridCell;
            //    cell.IsEnabled = (e.Row.Item as Model).StateCandidates != null;
            //}
        }
        public class Model : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private int _srno;
            private string _datatype;
            private string _size;
            private string _columnName;
            private List<string> _datatypes = new List<string>() { "CHAR()", "VARCHAR()", "TINYTEXT", "TEXT", "BLOB", "MEDIUMTEXT", "MEDIUMBLOB", "LONGTEXT", "LONGBLOB", "TINYINT()", "SMALLINT()", "MEDIUMINT()", "INT()", "BIGINT()", "FLOAT", "DOUBLE(,)", "DECIMAL(,)", "DATE", "DATETIME", "TIMESTAMP", "TIME", "ENUM()", "SET" };

            public int SrNo
            {
                get { return _srno; }
                set
                {
                    if (_srno != value)
                    {
                        _srno = value;
                        TotalCount = _srno;
                        OnPropertyChanged("SrNo");
                    }
                }
            }

            public string DataType
            {
                get { return _datatype; }
                set
                {
                    if (_datatype != value)
                    {
                        _datatype = value;
                        OnPropertyChanged("DataType");
                    }
                }
            }
            public string Size
            {
                get { return _size; }
                set
                {
                    if (_size != value)
                    {
                        _size = value;
                        OnPropertyChanged("Size");
                    }
                }
            }
            public string ColumnName
            {
                get { return _columnName; }
                set
                {
                    if (_columnName != value)
                    {
                        _columnName = value;
                        OnPropertyChanged("ColumnName");
                    }
                }
            }

            public List<string> DataTypes
            {
                get { return _datatypes; }
            }
            //public List<string> SrNos
            //{
            //    get { return _srnos; }
            //}
            public void OnPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                txtFileName.Text = filename;
            }
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }
        public void PopulateBlankGrid()
        {
            
            list.Add(new Model() { SrNo = 1, DataType = "" });
            list.Add(new Model() { SrNo = 2, DataType = "" });
            list.Add(new Model() { SrNo = 3, DataType = "" });
            list.Add(new Model() { SrNo = 4, DataType = "" });
            list.Add(new Model() { SrNo = 5, DataType = "" });
            list.Add(new Model() { SrNo = 6, DataType = "" });
            list.Add(new Model() { SrNo = 7, DataType = "" });
            list.Add(new Model() { SrNo = 8, DataType = "" });
            list.Add(new Model() { SrNo = 9, DataType = "" });
            list.Add(new Model() { SrNo = 10, DataType = "" });

            dgScriptData.ItemsSource = list;
            dgScriptData.PreparingCellForEdit += new EventHandler<DataGridPreparingCellForEditEventArgs>(dgScriptData_PreparingCellForEdit);
        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {
            TotalCount = TotalCount + 1;
            list.Add(new Model() { SrNo = TotalCount, DataType = "" });
            dgScriptData.ItemsSource = list;
            //OnPropertyChanged("SrNo");
        }
    }
}
