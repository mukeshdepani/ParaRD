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

namespace FastDB
{
    /// <summary>
    /// Interaction logic for GridHeaderColumn.xaml
    /// </summary>
    public partial class GridHeaderColumn : Window
    {
        public string dbColumnName = String.Empty;
        private string connectionString;
        
        public GridHeaderColumn()
        {
            InitializeComponent();
        }
        
        public GridHeaderColumn(string ColumnName,SelectQueryBuilder queryBuilder)
        {
            dbColumnName = ColumnName;
            
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
    }
    
}
