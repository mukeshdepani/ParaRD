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

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for ScirptGenControlMore.xaml
    /// </summary>
    public partial class ScirptGenControlMore : UserControl
    {
        public ScirptGenControlMore()
        {
            InitializeComponent();
            List<string> ListOfLogicalOpreator = new List<string> { "CHAR()", "VARCHAR()", "TINYTEXT", "TEXT", "BLOB", "MEDIUMTEXT", "MEDIUMBLOB", "LONGTEXT", "LONGBLOB", "TINYINT()", "SMALLINT()", "MEDIUMINT()", "INT()", "BIGINT()", "FLOAT", "DOUBLE(,)", "DECIMAL(,)", "DATE", "DATETIME", "TIMESTAMP", "TIME", "ENUM()", "SET" };
            this.cmbDataType.ItemsSource = ListOfLogicalOpreator;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btnDelete = (Button)sender;
            ScriptGen sg = (ScriptGen)GetTopLevelControl(this.cmbDataType);
            ScriptGeneratorControl scriptgenerator = (ScriptGeneratorControl)GetRVC(this.cmbDataType);
            DependencyObject parent = GetRVC(this.cmbDataType);

            if (parent != null)
            {
                //ScriptGen  = (ScriptGen)parent;
                //ScriptGeneratorControl sg = (ScriptGeneratorControl)parent;
                if (scriptgenerator.StackPanelScriptGen.Children.Count > 0)
                {
                    DockPanel dkp = (DockPanel)btnDelete.Parent;
                    ScirptGenControlMore scriptGenMore = (ScirptGenControlMore)((Grid)((StackPanel)dkp.Parent).Parent).Parent;
                    scriptgenerator.StackPanelScriptGen.Children.Remove(scriptGenMore);
                }
                if (scriptgenerator.StackPanelScriptGen.Children.Count == 0)
                {
                    scriptgenerator.btnGenerateScript.Visibility = System.Windows.Visibility.Hidden;
                    scriptgenerator.borderJoinDock.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }
        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    Button btnAdd = (Button)sender;
        //    ScriptGen sg = (ScriptGen)GetTopLevelControl(this.cmbDataType);
        //    ScriptGeneratorControl scriptgenerator = (ScriptGeneratorControl)GetRVC(this.cmbDataType);
        //    DependencyObject parent = GetRVC(this.cmbDataType);

        //    if (parent != null)
        //    {
        //        DockPanel dkp = (DockPanel)btnDelete.Parent;
        //        ScirptGenControlMore scriptGenMore = (ScirptGenControlMore)((Grid)((StackPanel)dkp.Parent).Parent).Parent;
        //        //scriptgenerator.StackPanelScriptGen.Children.Remove(scriptGenMore);


        //        int numberOfStackPanel = sg.ScriptGen1.StackPanelScriptGen.Children.Count;
        //        ScirptGenControlMore fs = new ScirptGenControlMore();

        //        scriptgenerator.StackPanelScriptGen.Children.Add(fs);
        //    }
        //}
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

        private void cmbDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox currentCmb = (ComboBox)sender;

            ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
            ScriptGeneratorControl StackPanelScriptGen = new ScriptGeneratorControl();
            int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
            for (int i = 0; i < numberOfStackPanel; i++)
            {
                if (scriptgenerator.ScriptGen1.StackPanelScriptGen.Children[i] != currentCmb)
                {
                    if (new string[] { "TINYTEXT", "TEXT", "BLOB", "MEDIUMTEXT", "MEDIUMBLOB", "LONGTEXT", "LONGBLOB", "FLOAT", "DATE", "DATETIME", "TIMESTAMP", "TIME", "SET" }.Contains(currentCmb.SelectedItem.ToString()))
                    {
                        txtSize.IsEnabled = false;
                    }
                    else
                    {
                        txtSize.IsEnabled = true;
                    }
                }

            }
        }

        //private void chkAutoIncrement_Checked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chkAutoIncrement = (CheckBox)sender;
        //    ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
        //    ScriptGeneratorControl StackPanelScriptGen = new ScriptGeneratorControl();
        //    int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
        //    for (int i = 0; i < numberOfStackPanel; i++)
        //    {
        //        if (scriptgenerator.ScriptGen1.StackPanelScriptGen.Children[i] != chkAutoIncrement)
        //        {
        //            if (chkAutoIncrement.IsChecked == true)
        //            {
        //                chkNULL.IsChecked = false;
        //                chkNULL.IsEnabled = false;
        //            }
        //        }
        //    }
        //}
        //private void chkAutoIncrement_UnChecked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chkAutoIncrement = (CheckBox)sender;
        //    ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
        //    ScriptGeneratorControl StackPanelScriptGen = new ScriptGeneratorControl();
        //    int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
        //    for (int i = 0; i < numberOfStackPanel; i++)
        //    {
        //        if (scriptgenerator.ScriptGen1.StackPanelScriptGen.Children[i] != chkAutoIncrement)
        //        {
        //            chkNULL.IsChecked = false;
        //            chkNULL.Visibility = System.Windows.Visibility.Visible;
        //            chkNULL.IsEnabled = true;

        //        }
        //    }
        //}
    }

}
