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
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for ScriptGeneratorControl.xaml
    /// </summary>

    public partial class ScriptGeneratorControl : UserControl
    {
        public ScriptGeneratorControl()
        {
            InitializeComponent();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
            this.btnGenerateScript.Visibility = System.Windows.Visibility.Visible;
            int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
            ScirptGenControlMore fs = new ScirptGenControlMore();

            this.StackPanelScriptGen.Children.Add(fs);
            DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Visible;
            borderJoinDock.Visibility = System.Windows.Visibility.Visible;
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

        private void btnGenerateScript_Click(object sender, RoutedEventArgs e)
        {
            ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
            if (scriptgenerator.txtTableName.Text == "")
            {
                MessageBox.Show("Enter Table Name");
                scriptgenerator.txtTableName.Focus();
            }
            else
            {
                int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
                ScirptGenControlMore fs = new ScirptGenControlMore();
                StringBuilder query = new StringBuilder();
                query.Append("CREATE TABLE ");
                query.Append(scriptgenerator.txtTableName.Text);
                query.Append(" ( ");
                for (int i = 0; i < numberOfStackPanel; i++)
                {
                    ScirptGenControlMore ts = (ScirptGenControlMore)this.StackPanelScriptGen.Children[i];
                    if (ts.txtColumnName.Text != "")
                    {
                        string columnName = ts.txtColumnName.Text;
                        string datatype = ts.cmbDataType.SelectedItem.ToString();
                        datatype = datatype.Replace("(", " ");
                        datatype = datatype.Replace(")", " ");
                        string size = ts.txtSize.Text;
                        query.Append(columnName);
                        query.Append(" ");
                        query.Append(datatype);
                        if (ts.txtSize.IsEnabled == true)
                        {
                            query.Append("(" + size + ")");
                        }
                        query.Append(" ");
                        if (ts.chkNULL.IsChecked == true)
                        {
                            query.Append("NULL");
                            query.Append(" ");
                            query.Append("DEFAULT");
                            query.Append(" ");
                            query.Append("NULL");
                        }
                        else
                        {
                            query.Append("NOT NULL");
                            query.Append(" ");
                        }
                        if (ts.chkLookUp.IsChecked == true)
                        {
                            query.Append("LOOKUP");
                        }
                        query.Append(", ");
                    }
                }
                if (query.Length > 1)
                {
                    query.Length -= 2;
                }
                query.Append(")");
                query.Append("COLLATE='latin1_bin'");
                query.Append("ENGINE=BRIGHTHOUSE");
                query.Append("ROW_FORMAT=DEFAULT");
                //query = query.
                //string connectionString = ConfigurationManager.AppSettings["FastDBConn"];
                //connection = new MySqlConnection(connectionString);

                //connection.Open();
                //MySqlCommand cmd = new MySqlCommand();
                //cmd.CommandText = query.ToString();
                //cmd.Connection = connection;
                //cmd.ExecuteNonQuery();
                //connection.Close();

                string fileText = query.ToString();
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
                {
                    Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
                };
                if (dialog.ShowDialog() == true)
                {
                    File.WriteAllText(dialog.FileName, fileText);
                }
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            ScriptGen scriptgenerator = (ScriptGen)GetTopLevelControl(this);
            if (scriptgenerator.txtHeader.Text.Trim() != "")
            {
                string header = scriptgenerator.txtHeader.Text.Trim();
                header = header.Replace("\"", "");
                header = header.Trim();
                header = header.Replace(" ", "_");
                string[] headerArray = header.Split(',');
                foreach (string strheader in headerArray)
                {
                    this.btnGenerateScript.Visibility = System.Windows.Visibility.Visible;
                    int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
                    ScirptGenControlMore fs = new ScirptGenControlMore();
                    //Regex.Replace(strheader, "[^a-zA-Z0-9]", "");
                    fs.txtColumnName.Text = strheader.ToString();
                    this.StackPanelScriptGen.Children.Add(fs);
                    DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Visible;
                    borderJoinDock.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else if (scriptgenerator.txtFileName.Text.Trim() != "")
            {
                string filename = scriptgenerator.txtFileName.Text.Trim();
                string ext = System.IO.Path.GetExtension(filename);
                if (ext == ".txt")
                {
                    string header = File.ReadAllText(filename);
                    header = header.Replace("\"", "");
                    header = header.Trim();
                    header = header.Replace(" ", "_");
                    string[] headerArray = header.Split(',');
                    foreach (string strheader in headerArray)
                    {
                        this.btnGenerateScript.Visibility = System.Windows.Visibility.Visible;
                        int numberOfStackPanel = scriptgenerator.ScriptGen1.StackPanelScriptGen.Children.Count;
                        ScirptGenControlMore fs = new ScirptGenControlMore();
                        //Regex.Replace(strheader, "[^a-zA-Z0-9]", "");
                        fs.txtColumnName.Text = strheader.ToString();
                        this.StackPanelScriptGen.Children.Add(fs);
                        DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Visible;
                        borderJoinDock.Visibility = System.Windows.Visibility.Visible;
                    }
                }

            }
        }
    }
}
