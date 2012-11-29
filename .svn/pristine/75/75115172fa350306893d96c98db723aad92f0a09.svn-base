using System;
using System.Collections;
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
using FastDB.Class;
using SQLBuilder.Clauses;
using Microsoft.Windows.Controls;
using System.Collections.ObjectModel;
using SQLBuilder;
using System.Xml.Serialization;
using System.IO;
using SQLBuilder.Common;
namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for SelectTabControl.xaml
    /// </summary>
    public partial class SelectTabControl : UserControl
    {
        public bool isValidated;
        private Brush TextBoxOriginalBorderBrush;
        private Brush RowOriginalBorderBrush;
        private string queryString;
        private Style ComboboxOriginalStyle;

        public ObservableCollection<Column> _FromSelectedColToCollection = new ObservableCollection<Column>();
        public ObservableCollection<Column> _SelectedColCollection = new ObservableCollection<Column>();
        public ObservableCollection<Column> computedColList = new ObservableCollection<Column>();
        private readonly ObservableCollection<string> m_items = new ObservableCollection<string>();
        private bool isExpanded = false;
        public static int totalCaseCondition = 2;
        public static int totalRow;
        public SelectTabControl()
        {

            InitializeComponent();
            DataContext = m_items;
            ComboboxOriginalStyle = this.ComputedColComboBox1.Style;
            ComboboxOriginalStyle = this.ComputedColFunctionComboBox.Style;
            this.ComputedColFunctionComboBox.ItemsSource = Functions.getFunctionNames();
            isValidated = false;
            ColsToSelectAcc.SelectionMode = AccordionSelectionMode.ZeroOrOne;
            RowOriginalBorderBrush = Brushes.White;
            //seting up list ToselectColumFrom
            lstToSelecteColFrom.ItemsSource = _FromSelectedColToCollection;
            //seting up selected list box itemsource
            lstSelectedCol.ItemsSource = _SelectedColCollection;
            //setting computed col listbox
            Column computedCol = new Column();
            computedCol.Name = "";
            computedCol.AliasName = "";
            computedColList.Add(computedCol);
            lstSelectedCol.MouseMove += new MouseEventHandler(lstSelectedCol_MouseMove);
            List<string> ComputedColFormatComboBoxItemSrc = Common.GetColumsFormatList();
            ComputedColFormatComboBoxItemSrc.Insert(0, "Select One");
            ComputedColFormatComboBox.ItemsSource = ComputedColFormatComboBoxItemSrc;
            ComputedColFormatComboBox.SelectedIndex = 0;

        }
        
        void lstSelectedCol_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            var col = ((FrameworkElement)e.OriginalSource).DataContext as Column;
            if (col != null)
            {
                toolTip.Content = col.Name;
                toolTip.IsOpen = true;
                lstSelectedCol.ToolTip = toolTip;
                ToolTipService.SetShowDuration(lstSelectedCol, 5000);
                toolTip.IsOpen = false;
            }
            else
            {
                toolTip.IsOpen = false;
            }
        }

        private List<string> GetColumnsFromAllFromTabTable()
        {
            List<string> list = new List<string>();
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.ComputedColFormatComboBox);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.ComputedColFormatComboBox);  //get resultview control containing cmbWherTabCondition

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                //get the From table columns 
                if (mainWindow.listOfTable != null & mainWindow.listOfTable.Count > 0)
                {
                    List<string> fc = GetFromTableColums(mainWindow.listOfTable[rvc.FromTabCntrl.cmbFromTable.SelectedIndex].columns, rvc.FromTabCntrl.txtFromAlias.Text);

                    if (fc != null)
                    {
                        list.AddRange(fc);
                    }
                }

            }
            return list;
        }
        
        private List<string> GetFromTableColums(List<MySQLData.Column> listOfColumns, string alias)
        {
            List<string> list = new List<string>();
            if (alias != string.Empty)
            {
                foreach (MySQLData.Column column in listOfColumns)
                {
                    list.Add(alias + "." + column.name);
                }
            }
            return list;
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
        
        private void lstToSelecteColFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var col = ((FrameworkElement)e.OriginalSource).DataContext as Column;
            if (col != null)
            {
                IEnumerable<Column> columns = _SelectedColCollection.Where(x => x.Name == col.Name && x.AliasName == col.AliasName);
                if (columns.Count() == 0)
                {
                    IEnumerable<Column> columnsWithSameAlias = _SelectedColCollection.Where(x => x.AliasName == col.AliasName);
                    if (columnsWithSameAlias.Count() == 0)
                    {
                        _SelectedColCollection.Add(col);
                    }
                    else
                    {
                        MessageBox.Show("Duplicate Alias");
                    }
                }
                else
                {
                    MessageBox.Show("Duplicate Column");
                }
                this.lstToSelecteColFrom.SelectedIndex = -1;
            }

            if (_SelectedColCollection.Count > 11 && isExpanded == false)
            {
                SelectedColsStackPanel.Width += 16;
                isExpanded = true;
            }

            /***********Validate Control***********/
            Validate();
            /**************************************/

            /*****Update Cross Tabulation**********/
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.UpdateCrossTabulationTabCntrl(mainWindow);
                rvc.UpdateTabulationTabCntrl(mainWindow);
            }
            
        }

        private void UpdateXmlQuery()
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (mainWindow.ValidateAllTabCntrls(rvc))
                {
                    SelectQueryBuilder queryBuilder = mainWindow.LoadSelectQueryBuilderNew(rvc);
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
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.lstToSelecteColFrom.SelectedItems.Count; i++)
            {
                Column col = (Column)this.lstToSelecteColFrom.SelectedItems[i];
                //check if alias already exist
                IEnumerable<Column> columnsWithSameAlias = _SelectedColCollection.Where(x => x.AliasName == col.AliasName);
                if (columnsWithSameAlias.Count() == 0)
                {
                    //check if column alreadyexist
                    IEnumerable<Column> columns = _SelectedColCollection.Where(x => x.Name == col.Name & x.AliasName == col.AliasName);
                    if (columns.Count() == 0)
                    {
                        _SelectedColCollection.Add(col);
                    }
                    else
                    {
                        System.Media.SystemSounds.Beep.Play();
                    }
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                }
            }

            if (this.lstToSelecteColFrom.SelectedIndex == -1)
            {
                Column computedCol = new Column();
                computedCol.Name = ComputedColExpTxtBox.Text;
                computedCol.AliasName = "";
                if (ComputedColFormatComboBox.SelectedValue.ToString() != "Select One")
                    computedCol.Format = ComputedColFormatComboBox.SelectedValue.ToString();
                else
                    computedCol.Format = "NO";

                if (computedCol != null)
                {
                    if (computedCol.Name != System.String.Empty & computedCol.AliasName != System.String.Empty)
                    {
                        IEnumerable<Column> columnsWithSameAlias = _SelectedColCollection.Where(x => x.AliasName == computedCol.AliasName);
                        if (columnsWithSameAlias.Count() == 0)
                        {
                            IEnumerable<Column> columns = _SelectedColCollection.Where(x => x.Name == computedCol.Name & x.AliasName == computedCol.AliasName);
                            if (columns.Count() == 0)
                            {
                                _SelectedColCollection.Add(computedCol);
                            }
                            else
                            {
                                System.Media.SystemSounds.Beep.Play();
                            }
                        }
                        else
                        {
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                }

            }

            if (_SelectedColCollection.Count > 11 && isExpanded == false)
            {
                SelectedColsStackPanel.Width += 16;
                isExpanded = true;
            }

            this.lstToSelecteColFrom.SelectedIndex = -1;
            this.lstSelectedCol.SelectedIndex = -1;
            if (this._SelectedColCollection.Count > 0)
            {
                this.lblErrorMessage.Content = "";
            }
            /***********Validate Control***********/
            Validate();
            /**************************************/

            /************Update query**************/
            //UpdateXmlQuery();
            /**************************************/

            /*****Update Cross Tabulation**********/
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.UpdateCrossTabulationTabCntrl(mainWindow);
                rvc.UpdateTabulationTabCntrl(mainWindow);
            }
            /**************************************/

            /*****************To Enable Save***********/
            //FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            //DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            //if (parent != null)
            //{
            //    ResultViewControl rvc = (ResultViewControl)parent;
            //    string directoryPath = rvc.result.directoryPath;

            //    //MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            //    //SelectQueryBuilder queryBuilder = mainWindow.LoadSelectQueryBuilderNew(rvc);

            //    //MainGirdViewControl mainGridView1 = new MainGirdViewControl(queryBuilder, mainWindow.CurrentDatabaseName);
            //    //rvc.result = mainGridView1.result;
            //    rvc.result.isModified = true;
            //    rvc.result.isNew = false;
            //    rvc.result.directoryPath = directoryPath;
            //}
            /********************************************/
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this._SelectedColCollection.Count == 0)
            {
                this.lblErrorMessage.Content = "Please select column(s)";
            }

            int count = this.lstSelectedCol.SelectedItems.Count;

            for (int i = 0; i < count; i++)
            {
                Column col = (Column)this.lstSelectedCol.SelectedItems[0];
                _SelectedColCollection.Remove(col);
            }
            this.lstToSelecteColFrom.SelectedIndex = -1;
            this.lstSelectedCol.SelectedIndex = -1;

            if (_SelectedColCollection.Count <= 11 && isExpanded == true)
            {
                SelectedColsStackPanel.Width -= 16;
                isExpanded = false;
            }
            /***********Validate Control***********/
            Validate();
            /**************************************/

            /*****Update Cross Tabulation**********/
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.UpdateCrossTabulationTabCntrl(mainWindow);
                rvc.UpdateTabulationTabCntrl(mainWindow);
            }
            /**************************************/
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            int upRow = 0;
            int currentRow = 0;
            btnDown.IsEnabled = true;
            currentRow = this.lstSelectedCol.SelectedIndex;
            if (currentRow == 0)
            {
                
            }
            else
            {
                upRow = currentRow - 1;
                if (upRow >= 0)
                {
                    _SelectedColCollection.Move(currentRow, upRow);
                    currentRow = upRow;
                }
                else
                {
                    
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            int downRow = 0;
            int currentRow = 0;
            btnUp.IsEnabled = true;
            currentRow = this.lstSelectedCol.SelectedIndex;

            if (currentRow != -1)
            {
                int totalRows = this.lstSelectedCol.Items.Count;
                if (currentRow == (totalRows - 1))
                {
                    
                }
                else
                {
                    downRow = currentRow + 1;
                    if (downRow >= 0)
                    {
                        _SelectedColCollection.Move(currentRow, downRow);
                        currentRow = downRow;
                    }
                    else
                    {
                        
                    }
                }
            }
        }
        
        public Boolean Validate()
        {
            bool validated = true;
            this.lblErrorMessage.Content = "";
            return validated;
        }

        private void RemoveCase_Click(object sender, RoutedEventArgs e)
        {
            Label lblWhen = new Label();
            TextBox txtWhen = new TextBox();
            Label lblThen = new Label();
            TextBox txtThen = new TextBox();
            Label lblElse = new Label();
            TextBox txtElse = new TextBox();

            if (totalCaseCondition > 2)
            {
                var lblw = (UIElement)this.FindName("lblWhen" + Convert.ToString(totalCaseCondition - 1));
                var txtw = (UIElement)this.FindName("txtWhen" + Convert.ToString(totalCaseCondition - 1));

                var lblt = (UIElement)this.FindName("lblThen" + Convert.ToString(totalCaseCondition - 1));
                var txtt = (UIElement)this.FindName("txtThen" + Convert.ToString(totalCaseCondition - 1));

                var lble = (UIElement)this.FindName("lblElse");
                var txte = (UIElement)this.FindName("txtElse");

                totalRow = totalRow - 2;

                this.computedGrid.Children.Remove(lblw);
                this.computedGrid.Children.Remove(txtw);

                totalRow = totalRow - 2;

                this.computedGrid.Children.Remove(lblt);
                this.computedGrid.Children.Remove(txtt);

                this.computedGrid.Children.Remove(lble);
                this.computedGrid.Children.Remove(txte);

                this.UnregisterName("lblElse");
                this.UnregisterName("txtElse");

                lblElse.Content = "Else";
                lblElse.Name = "lblElse";
                lblElse.Visibility = System.Windows.Visibility.Visible;
                lblElse.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                lblElse.Margin = new Thickness(7);
                lblElse.SetValue(Grid.RowProperty, totalRow);
                lblElse.SetValue(Grid.ColumnProperty, 0);
                this.RegisterName("lblElse", lblElse);

                txtElse.Text = "";
                txtElse.Name = "txtElse";
                txtElse.Height = 20;
                txtElse.Width = 120;
                txtElse.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
                txtElse.Visibility = System.Windows.Visibility.Visible;
                txtElse.Margin = new Thickness(0, 0, 5, 5);
                txtElse.SetValue(Grid.RowProperty, totalRow);
                txtElse.SetValue(Grid.ColumnProperty, 1);
                this.RegisterName("txtElse", txtElse);

                this.computedGrid.Children.Add(lblElse);
                this.computedGrid.Children.Add(txtElse);

                AddCompCol.SetValue(Grid.RowProperty, totalRow + 2);

                totalCaseCondition = totalCaseCondition - 1;
            }

        }

        private void AddCase_Click(object sender, RoutedEventArgs e)
        {
            int numberOfStackPanel = computedGrid.Children.Count;
            int i = 2;

            this.InitializeComponent();

            Label lblWhen = new Label();
            TextBox txtWhen = new TextBox();
            Label lblThen = new Label();
            TextBox txtThen = new TextBox();
            Label lblElse = new Label();
            TextBox txtElse = new TextBox();

            var lblW = (UIElement)this.FindName("lblWhen" + Convert.ToString(totalCaseCondition));
            var txtW = (UIElement)this.FindName("txtWhen" + Convert.ToString(totalCaseCondition));

            var lblT = (UIElement)this.FindName("lblThen" + Convert.ToString(totalCaseCondition));
            var txtT = (UIElement)this.FindName("txtThen" + Convert.ToString(totalCaseCondition));

            var lblE = (UIElement)this.FindName("lblElse");
            var txtE = (UIElement)this.FindName("txtElse");

            this.computedGrid.Children.Remove(lblE);
            this.computedGrid.Children.Remove(txtE);

            if (lblW != null)
            {
                this.UnregisterName("lblWhen" + Convert.ToString(totalCaseCondition));
                this.UnregisterName("txtWhen" + Convert.ToString(totalCaseCondition));

                this.UnregisterName("lblThen" + Convert.ToString(totalCaseCondition));
                this.UnregisterName("txtThen" + Convert.ToString(totalCaseCondition));

            }

            this.UnregisterName("lblElse");
            this.UnregisterName("txtElse");

            lblWhen.Name = "lblWhen" + Convert.ToString(totalCaseCondition);
            lblWhen.Content = "When";
            lblWhen.Visibility = System.Windows.Visibility.Visible;
            lblWhen.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            lblWhen.Margin = new Thickness(7);
            lblWhen.SetValue(Grid.RowProperty, totalRow);
            lblWhen.SetValue(Grid.ColumnProperty, 0);
            this.RegisterName("lblWhen" + Convert.ToString(totalCaseCondition), lblWhen);

            txtWhen.Text = "";
            txtWhen.Name = "txtWhen" + Convert.ToString(totalCaseCondition);
            txtWhen.Height = 20;
            txtWhen.Width = 120;
            txtWhen.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
            txtWhen.Visibility = System.Windows.Visibility.Visible;
            txtWhen.Margin = new Thickness(0, 0, 5, 5);
            txtWhen.SetValue(Grid.RowProperty, totalRow);
            txtWhen.SetValue(Grid.ColumnProperty, 1);
            this.RegisterName("txtWhen" + Convert.ToString(totalCaseCondition), txtWhen);

            totalRow = totalRow + 2;
            lblThen.Name = "lblThen" + Convert.ToString(i);
            lblThen.Content = "Then";
            lblThen.Visibility = System.Windows.Visibility.Visible;
            lblThen.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            lblThen.Margin = new Thickness(7);
            lblThen.SetValue(Grid.RowProperty, totalRow);
            lblThen.SetValue(Grid.ColumnProperty, 0);
            this.RegisterName("lblThen" + Convert.ToString(totalCaseCondition), lblThen);

            txtThen.Text = "";
            txtThen.Name = "txtThen" + Convert.ToString(totalCaseCondition);
            txtThen.Height = 20;
            txtThen.Width = 120;
            txtThen.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
            txtThen.Visibility = System.Windows.Visibility.Visible;
            txtThen.Margin = new Thickness(0, 0, 5, 5);
            txtThen.SetValue(Grid.RowProperty, totalRow);
            txtThen.SetValue(Grid.ColumnProperty, 1);
            this.RegisterName("txtThen" + Convert.ToString(totalCaseCondition), txtThen);

            this.computedGrid.Children.Add(lblWhen);
            this.computedGrid.Children.Add(txtWhen);
            this.computedGrid.Children.Add(lblThen);
            this.computedGrid.Children.Add(txtThen);

            totalRow = totalRow + 2;

            lblElse.Content = "Else";
            lblElse.Name = "lblElse";
            lblElse.Visibility = System.Windows.Visibility.Visible;
            lblElse.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            lblElse.Margin = new Thickness(7);
            lblElse.SetValue(Grid.RowProperty, totalRow);
            lblElse.SetValue(Grid.ColumnProperty, 0);
            this.RegisterName("lblElse", lblElse);

            txtElse.Text = "";
            txtElse.Name = "txtElse";
            txtElse.Height = 20;
            txtElse.Width = 120;
            txtElse.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
            txtElse.Visibility = System.Windows.Visibility.Visible;
            txtElse.Margin = new Thickness(0, 0, 5, 5);
            txtElse.SetValue(Grid.RowProperty, totalRow);
            txtElse.SetValue(Grid.ColumnProperty, 1);
            this.RegisterName("txtElse", txtElse);

            this.computedGrid.Children.Add(lblElse);
            this.computedGrid.Children.Add(txtElse);

            AddCompCol.SetValue(Grid.RowProperty, totalRow + 2);

            totalCaseCondition = totalCaseCondition + 1;

        }
        
        private void AddCompCol_Click(object sender, RoutedEventArgs e)
        {
            ComputedColumn computedCol = new ComputedColumn();
            if (ComputedColExpTxtBox.Text != "")
            {
                computedCol.Name = ComputedColExpTxtBox.Text;
            }
            else
            {
                string functionName = ComputedColFunctionComboBox.SelectedItem.ToString();
                Function function = Functions.getFunction(functionName);
                List<Parameter> paramList = new List<Parameter>();

                int paramIndex = 0;
                foreach (Parameter param in function.Parameters)
                {
                    paramIndex++;
                    string value = getParamSelectedValue(paramIndex);
                    Parameter parameter = new Parameter(param.Name, value, param.Type);
                    paramList.Add(parameter);
                }
                computedCol.Parameters = paramList;

                computedCol.Name = functionName;
                computedCol.Type = ComputedColumn.FUNCTION;

                if (functionName == "CASE")
                {
                    for (int i = 1; i < Convert.ToInt32(totalCaseCondition); i++)
                    {
                        Label lblWhen = (Label)this.FindName("lblWhen" + Convert.ToString(i));
                        TextBox txtWhen = (TextBox)this.FindName("txtWhen" + Convert.ToString(i));

                        Label lblThen = (Label)this.FindName("lblThen" + Convert.ToString(i));
                        TextBox txtThen = (TextBox)this.FindName("txtThen" + Convert.ToString(i));

                        computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + lblWhen.Content.ToString();
                        computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + txtWhen.Text.ToString();

                        computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + lblThen.Content.ToString();
                        computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + txtThen.Text.ToString();
                    }
                    Label lblElse = (Label)this.FindName("lblElse");
                    TextBox txtElse = (TextBox)this.FindName("txtElse");

                    computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + lblElse.Content.ToString();
                    computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + txtElse.Text.ToString();

                    computedCol.Parameters[0].Value = computedCol.Parameters[0].Value + " " + "END)";
                }
                
                if (functionName == "IFNULL")
                {
                    computedCol.Parameters[1].Value = ComputedColifnullTxtBox.Text;
                }
                else
                {
                    computedCol.Parameters = paramList;
                }
            }
            computedCol.AliasName = ComputedColNameTxtBox.Text;
            if (ComputedColFormatComboBox.SelectedValue.ToString() != "Select One")
                computedCol.Format = ComputedColFormatComboBox.SelectedValue.ToString();

            if (computedCol != null)
            {
                if (computedCol.Name != System.String.Empty & computedCol.AliasName != System.String.Empty)
                {
                    IEnumerable<Column> columnsWithSameAlias = _SelectedColCollection.Where(x => x.AliasName == computedCol.AliasName);
                    if (columnsWithSameAlias.Count() == 0)
                    {
                        IEnumerable<Column> columns = _SelectedColCollection.Where(x => x.Name == computedCol.Name & x.AliasName == computedCol.AliasName);
                        if (columns.Count() == 0)
                        {
                            _SelectedColCollection.Add(computedCol);
                            ComputedColNameTxtBox.Text = "";
                            ComputedColExpTxtBox.Text = "";
                            ComputedColFormatComboBox.Text = "Select One";
                        }
                        else
                        {
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                    else
                    {
                        System.Media.SystemSounds.Beep.Play();
                    }
                }
            }

            if (_SelectedColCollection.Count > 11 && isExpanded == false)
            {
                SelectedColsStackPanel.Width += 16;
                isExpanded = true;
            }
            ComputedColName1.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName2.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName3.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName4.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox1.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox2.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox3.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox4.Visibility = System.Windows.Visibility.Hidden;
            ComputedColifnullTxtBox.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox1.Text = "";
            ComputedColComboBox2.Text = "";
            ComputedColComboBox3.Text = "";
            ComputedColComboBox4.Text = "";
            this.ComputedColFunctionComboBox.ItemsSource = Functions.getFunctionNames();
            this.ComputedColFunctionComboBox.SelectedIndex = -1;
            AddCaseCondition.Visibility = System.Windows.Visibility.Hidden;
            RemoveCaseCondition.Visibility = System.Windows.Visibility.Hidden;

        }

        private void lstSelectedCol_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var col = ((FrameworkElement)e.OriginalSource).DataContext as Column;

            if (col != null)
            {
                _SelectedColCollection.Remove(col);
            }

            if (_SelectedColCollection.Count <= 11 && isExpanded == true)
            {
                SelectedColsStackPanel.Width -= 16;
                isExpanded = false;
            }
            /***********Validate Control***********/
            Validate();
            /**************************************/

            /*****Update Cross Tabulation**********/
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.UpdateCrossTabulationTabCntrl(mainWindow);
                rvc.UpdateTabulationTabCntrl(mainWindow);
            }
            /**************************************/
        }

        private void AddAllBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstToSelecteColFrom);
            _SelectedColCollection.Clear();
           
            foreach (Column col in this.lstToSelecteColFrom.Items)
            {
                IEnumerable<Column> columns = _SelectedColCollection.Where(x => x.Name == col.Name & x.AliasName == col.AliasName);
                if (columns.Count() == 0)
                {
                    _SelectedColCollection.Add(col);
                }
            }

            if (_SelectedColCollection.Count > 11 && isExpanded == false)
            {
                SelectedColsStackPanel.Width += 16;
                isExpanded = true;
            }
            /***********Validate Control***********/
            Validate();
            /**************************************/

            /*****Update Cross Tabulation**********/
            
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.UpdateCrossTabulationTabCntrl(mainWindow);
                
            }
            /**************************************/
        }

        private void RemoveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            _SelectedColCollection.Clear();
            if (_SelectedColCollection.Count <= 11 && isExpanded == true)
            {
                SelectedColsStackPanel.Width -= 16;
                isExpanded = false;
            }
            /***********Validate Control***********/
            Validate();
            /**************************************/

            /*****Update Cross Tabulation and Tabulation**********/
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.lstSelectedCol);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.lstSelectedCol);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.UpdateCrossTabulationTabCntrl(mainWindow);
                rvc.UpdateTabulationTabCntrl(mainWindow);
            }
            /**************************************/
        }

        private void ColsToSelectAcc_SelectedItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }
        
        private void ComputedColFunctionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ComputedColFunctionComboBox.Style = ComboboxOriginalStyle;

            ComputedColComboBox1.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox2.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox3.Visibility = System.Windows.Visibility.Hidden;
            ComputedColComboBox4.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName1.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName2.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName3.Visibility = System.Windows.Visibility.Hidden;
            ComputedColName4.Visibility = System.Windows.Visibility.Hidden;
            ComputedColifnullTxtBox.Visibility = System.Windows.Visibility.Hidden;
            AddCaseCondition.Visibility = System.Windows.Visibility.Hidden;
            RemoveCaseCondition.Visibility = System.Windows.Visibility.Hidden;

            Label lblWhen = new Label();
            TextBox txtWhen = new TextBox();
            Label lblThen = new Label();
            TextBox txtThen = new TextBox();
            Label lblElse = new Label();
            TextBox txtElse = new TextBox();

            for (int i = 1; i < totalCaseCondition; i++)
            {
                var lblW = (UIElement)this.FindName("lblWhen" + Convert.ToString(i));
                var txtW = (UIElement)this.FindName("txtWhen" + Convert.ToString(i));

                var lblT = (UIElement)this.FindName("lblThen" + Convert.ToString(i));
                var txtT = (UIElement)this.FindName("txtThen" + Convert.ToString(i));

                var lblE = (UIElement)this.FindName("lblElse");
                var txtE = (UIElement)this.FindName("txtElse");

                this.computedGrid.Children.Remove(lblW);
                this.computedGrid.Children.Remove(txtW);

                this.computedGrid.Children.Remove(lblT);
                this.computedGrid.Children.Remove(txtT);

                this.computedGrid.Children.Remove(lblE);
                this.computedGrid.Children.Remove(txtE);

                if (lblW != null)
                {
                    this.UnregisterName("lblWhen" + Convert.ToString(i));
                    this.UnregisterName("txtWhen" + Convert.ToString(i));

                    this.UnregisterName("lblThen" + Convert.ToString(i));
                    this.UnregisterName("txtThen" + Convert.ToString(i));

                    if (i == 1)
                    {
                        this.UnregisterName("lblElse");
                        this.UnregisterName("txtElse");
                    }
                }


            }
            this.ComputedColFunctionComboBox.ItemsSource = Functions.getFunctionNames();
            if (ComputedColFunctionComboBox.SelectedItem != null)
            {
                Function function = Functions.getFunction(ComputedColFunctionComboBox.SelectedItem.ToString());
                if (function != null)
                {
                    List<string> columnNames = GetColumnsFromAllFromTabTable();
                    List<string> formatType = Common.GetDateFormatList();
                    switch (function.NumParameters)
                    {
                        case 1:
                            ComputedColComboBox1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName1.Visibility = System.Windows.Visibility.Visible;
                            this.ComputedColComboBox1.ItemsSource = columnNames;
                            this.ComputedColName1.Content = "Column Name";
                            break;
                        case 2:
                            ComputedColComboBox1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColComboBox2.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName2.Visibility = System.Windows.Visibility.Visible;
                            this.ComputedColComboBox1.ItemsSource = columnNames;
                            this.ComputedColName1.Content = "Column Name";
                            if (function.Name == "DATE_FORMAT")
                            {
                                this.ComputedColName2.Content = "Format Type";
                                this.ComputedColComboBox2.ItemsSource = formatType;
                                break;
                            }
                            else if (function.Name == "IFNULL")
                            {
                                this.ComputedColName2.Content = "Value";
                                this.ComputedColifnullTxtBox.Visibility = System.Windows.Visibility.Visible;
                                this.ComputedColComboBox2.Visibility = System.Windows.Visibility.Hidden;
                                this.ComputedColifnullTxtBox.Text = "0";
                                break;
                            }
                            else
                            {
                                this.ComputedColName2.Content = "Value";
                                this.ComputedColComboBox2.ItemsSource = columnNames;
                                break;
                            }

                        case 3:
                            ComputedColComboBox1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColComboBox2.Visibility = System.Windows.Visibility.Visible;
                            ComputedColComboBox3.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName2.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName3.Visibility = System.Windows.Visibility.Visible;
                            this.ComputedColComboBox1.ItemsSource = columnNames;
                            this.ComputedColComboBox2.ItemsSource = columnNames;
                            this.ComputedColComboBox3.ItemsSource = columnNames;
                            this.ComputedColName1.Content = "Group Column";
                            this.ComputedColName2.Content = "Order Column";
                            this.ComputedColName3.Content = "Ref Column";
                            break;
                        case 4:
                            ComputedColComboBox1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColComboBox2.Visibility = System.Windows.Visibility.Visible;
                            ComputedColComboBox3.Visibility = System.Windows.Visibility.Visible;
                            ComputedColComboBox4.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName1.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName2.Visibility = System.Windows.Visibility.Visible;
                            ComputedColName3.Visibility = System.Windows.Visibility.Visible;
                            this.ComputedColComboBox1.ItemsSource = columnNames;
                            this.ComputedColComboBox2.ItemsSource = columnNames;
                            this.ComputedColComboBox3.ItemsSource = columnNames;
                            this.ComputedColComboBox4.ItemsSource = columnNames;
                            this.ComputedColName1.Content = "Group Column";
                            this.ComputedColName2.Content = "Order Column";
                            this.ComputedColName3.Content = "Ref Column";
                            break;
                        case 5:
                            AddCaseCondition.Visibility = System.Windows.Visibility.Visible;
                            RemoveCaseCondition.Visibility = System.Windows.Visibility.Visible;

                            totalRow = 13;

                            lblWhen.Name = "lblWhen1";
                            lblWhen.Content = "When";
                            lblWhen.Visibility = System.Windows.Visibility.Visible;
                            lblWhen.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            lblWhen.Margin = new Thickness(7);
                            lblWhen.SetValue(Grid.RowProperty, totalRow);
                            lblWhen.SetValue(Grid.ColumnProperty, 0);
                            this.RegisterName("lblWhen1", lblWhen);

                            txtWhen.Text = "";
                            txtWhen.Name = "txtWhen1";
                            txtWhen.Height = 20;
                            txtWhen.Width = 120;
                            txtWhen.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
                            txtWhen.Visibility = System.Windows.Visibility.Visible;
                            txtWhen.Margin = new Thickness(0, 0, 5, 5);
                            txtWhen.SetValue(Grid.RowProperty, totalRow);
                            txtWhen.SetValue(Grid.ColumnProperty, 1);
                            this.RegisterName("txtWhen1", txtWhen);

                            totalRow = totalRow + 2;
                            lblThen.Name = "lblThen1";
                            lblThen.Content = "Then";
                            lblThen.Visibility = System.Windows.Visibility.Visible;
                            lblThen.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            lblThen.Margin = new Thickness(7);
                            lblThen.SetValue(Grid.RowProperty, totalRow);
                            lblThen.SetValue(Grid.ColumnProperty, 0);
                            this.RegisterName("lblThen1", lblThen);

                            txtThen.Text = "";
                            txtThen.Name = "txtThen1";
                            txtThen.Height = 20;
                            txtThen.Width = 120;
                            txtThen.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
                            txtThen.Visibility = System.Windows.Visibility.Visible;
                            txtThen.Margin = new Thickness(0, 0, 5, 5);
                            txtThen.SetValue(Grid.RowProperty, totalRow);
                            txtThen.SetValue(Grid.ColumnProperty, 1);
                            this.RegisterName("txtThen1", txtThen);

                            this.computedGrid.Children.Add(lblWhen);
                            this.computedGrid.Children.Add(txtWhen);
                            this.computedGrid.Children.Add(lblThen);
                            this.computedGrid.Children.Add(txtThen);

                            totalRow = totalRow + 2;

                            lblElse.Content = "Else";
                            lblElse.Name = "lblElse";
                            lblElse.Visibility = System.Windows.Visibility.Visible;
                            lblElse.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            lblElse.Margin = new Thickness(7);
                            lblElse.SetValue(Grid.RowProperty, totalRow);
                            lblElse.SetValue(Grid.ColumnProperty, 0);
                            this.RegisterName("lblElse", lblElse);

                            txtElse.Text = "";
                            txtElse.Name = "txtElse";
                            txtElse.Height = 20;
                            txtElse.Width = 120;
                            txtElse.BorderBrush = System.Windows.Media.Brushes.SteelBlue;
                            txtElse.Visibility = System.Windows.Visibility.Visible;
                            txtElse.Margin = new Thickness(0, 0, 5, 5);
                            txtElse.SetValue(Grid.RowProperty, totalRow);
                            txtElse.SetValue(Grid.ColumnProperty, 1);
                            this.RegisterName("txtElse", txtElse);

                            this.computedGrid.Children.Add(lblElse);
                            this.computedGrid.Children.Add(txtElse);

                            AddCompCol.SetValue(Grid.RowProperty, totalRow + 2);

                            break;
                    }
                }
            }


        }

        public string getParamSelectedValue(int index)
        {
            string value = "";

            switch (index)
            {
                case 1:
                    value = ComputedColComboBox1.Text;
                    break;
                case 2:
                    value = ComputedColComboBox2.Text;
                    break;
                case 3:
                    value = ComputedColComboBox3.Text;
                    break;
                case 4:
                    value = ComputedColComboBox4.Text;
                    break;
                case 5:
                    break;
            }

            return value;
        }

    }

}
