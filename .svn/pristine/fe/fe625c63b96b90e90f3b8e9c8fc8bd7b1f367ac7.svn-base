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
    /// Interaction logic for ScriptGen.xaml
    /// </summary>
    public partial class ScriptGen : Window
    {
        public ScriptGen()
        {
            InitializeComponent();
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
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
            int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;

            FromTabStackPanelControl fs = new FromTabStackPanelControl();
            fs.Name = "Fs2";
            fs.Margin = new Thickness(20, 250, 20, 255);
            fs.btnDelete.Visibility = System.Windows.Visibility.Visible;
            fs.btnDelete.Uid = (numberOfStackPanel + 1).ToString();
            fs.btnDelete.Width = 25.0;
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
        public DependencyObject GetRVC(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                if (tmp.DependencyObjectType.Name == "ScriptGeneratorControl")
                {
                    parent = tmp;
                    break;
                }
            }
            return parent;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (TabItem1.IsSelected)
                {
                    ScriptGen1.Margin = new Thickness(0, 75, 6, 40);
                }
                if (TabItem2.IsSelected)
                {
                    ScriptGen1.Margin = new Thickness(0, 115, 6, 40);
                }
            }

        }
    }
}
