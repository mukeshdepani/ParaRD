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
using SQLBuilder;
using FastDB.Class;
using System.Xml.Serialization;
using System.IO;
namespace FastDB.Control
{
    public partial class FromTabStackPanelControl : UserControl
    {
        public bool isValidated;
        private Style ComboboxOriginalStyle;
        private string queryString;
        private Brush TextBoxOriginalBorderBrush;
        private List<SQLBuilder.Clauses.Column> lstToSelectColsFrom;

        public FromTabStackPanelControl()
        {
            InitializeComponent();
            ComboboxOriginalStyle = this.cmbFromTabJoinType.Style;
            TextBoxOriginalBorderBrush = this.txtJoinTableAlias.BorderBrush;

            this.cmbFromTabJoinType.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.JoinType)).ToList();
            this.cmbFromTabQueryOpretor.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.Comparison)).ToList();
            this.cmbFromTabQueryOpretor.SelectedIndex = 0;
        }

        public void UpdateAllControls(ResultViewControl rvc)
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(rvc);

            rvc.SelectTabCntrl.lstToSelecteColFrom.IsEnabled = true;
            mainWindow.ColListForSelectTab = mainWindow.GenerateListOfSelectTabCntrlColumns(rvc);
            rvc.SelectTabCntrl.lstToSelecteColFrom.ItemsSource = mainWindow.ColListForSelectTab;

            rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.IsEnabled = true;
            rvc.TabulationTabCntrl.StackPanelTabuLationTabSummary.IsEnabled = true;

            List<SQLBuilder.Clauses.Column> listOfTabulationTabColumns = mainWindow.GenerateListOfTabulationTabCntrlColumns(rvc);

            if (((TabulationTabStackPanelGroupByControl)rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.Items.Count == 0)
            {
                // loading groupby columns
                for (int i = 0; i < rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                    tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                }
                // loading summary columns
                for (int i = 0; i < rvc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                {
                    TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)rvc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                    ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                }
            }
            else
            {
                List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((TabulationTabStackPanelGroupByControl)rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.ItemsSource;

                IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                {
                    // Reloading groupby columns
                    for (int i = 0; i < rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                    {
                        TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)rvc.TabulationTabCntrl.StackPanelTabuLationTabGroupBy.Children[i];
                        tg.cmbTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                    // Reloading summary columns
                    for (int i = 0; i < rvc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children.Count; i++)
                    {
                        TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)rvc.TabulationTabCntrl.StackPanelTabuLationTabSummary.Children[i];
                        ts.cmbTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                }
            }

            rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.IsEnabled = true;
            rvc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.IsEnabled = true;
            rvc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFristRowSort.IsEnabled = true;
            rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.IsEnabled = true;

            //if first drop down is null on Cross Tabulation means all dropdown item source is null
            if (((CrossTabulationTabStackPanelGroupByControl)rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.Items.Count == 0)
            {
                // loading groupby columns
                for (int i = 0; i < rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                    ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                }
                //loading summary first row means (column Name and sort) row
                rvc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;
                // loading summary columns
                for (int i = 0; i < rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                    cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                }
            }
            else
            {
                List<SQLBuilder.Clauses.Column> list1 = (List<SQLBuilder.Clauses.Column>)((CrossTabulationTabStackPanelGroupByControl)rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.ItemsSource;

                IEnumerable<SQLBuilder.Clauses.Column> difference = list1.Except(listOfTabulationTabColumns);

                if (list1.SequenceEqual(listOfTabulationTabColumns) == false)
                {
                    // Reloading groupby columns
                    for (int i = 0; i < rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                    {
                        CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabGroupBy.Children[i];
                        ctg.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                    // Reloading summary first row means (column Name and sort) row
                    rvc.CrossTabulationTabCntrl.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = listOfTabulationTabColumns;

                    // Reloading summary columns
                    for (int i = 0; i < rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
                    {
                        CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)rvc.CrossTabulationTabCntrl.StackPanelCrossTabuLationTabSummary.Children[i];
                        cts.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = listOfTabulationTabColumns;
                    }
                }
            }

            List<string> Collist = (from x in (mainWindow.GenerateListOfTabulationTabCntrlColumns(rvc))
                                    select x.Name).ToList<string>();

            if (rvc.WhereTabCntrl.StackPanelWhereTab.Children.Count != 0)
            {
                for (int i = 0; i < rvc.WhereTabCntrl.StackPanelWhereTab.Children.Count; i++)
                {
                    if (rvc.WhereTabCntrl.StackPanelWhereTab.Children[i].GetType().Name == "WhereTabRegularConditionControl")
                    {
                        WhereTabRegularConditionControl regCondn = (WhereTabRegularConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[i]);
                        regCondn.cmbWhereTabLeftSideColumns.ItemsSource = Collist;
                        regCondn.cmbWhereTabRightSideColumns.ItemsSource = Collist;
                    }
                    else if (rvc.WhereTabCntrl.StackPanelWhereTab.Children[i].GetType().Name == "WhereTabBetweenConditionControl")
                    {
                        WhereTabBetweenConditionControl regCondn = (WhereTabBetweenConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[i]);
                        regCondn.cmbWhereTabBetweenColumns.ItemsSource = Collist;
                    }
                    else if (rvc.WhereTabCntrl.StackPanelWhereTab.Children[i].GetType().Name == "WhereTabInNotInConditionControl")
                    {
                        WhereTabInNotInConditionControl regCondn = (WhereTabInNotInConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[i]);
                        regCondn.cmbWhereTabInNotInColumns.ItemsSource = Collist;
                    }
                    else if (rvc.WhereTabCntrl.StackPanelWhereTab.Children[i].GetType().Name == "WhereTabNullNotNullConditionControl")
                    {
                        WhereTabNullNotNullConditionControl regCondn = (WhereTabNullNotNullConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[i]);
                        regCondn.cmbWhereTabNullNotNullColumns.ItemsSource = Collist;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btnDelete = (Button)sender;
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTabJoinTable);

            DependencyObject parent = GetRVC(this.cmbFromTabJoinTable);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (rvc.FromTabCntrl.StackPanelFromTab.Children.Count > 0)
                {
                    DockPanel dkp = (DockPanel)btnDelete.Parent;
                    FromTabStackPanelControl fsp = (FromTabStackPanelControl)((Grid)((StackPanel)dkp.Parent).Parent).Parent;
                    rvc.FromTabCntrl.StackPanelFromTab.Children.Remove(fsp);

                    if (rvc.FromTabCntrl.isValidated & (rvc.TabulationTabCntrl.isValidated & rvc.TabulationTabCntrl.isTabulation == false) & (rvc.CrossTabulationTabCntrl.isValidated & rvc.CrossTabulationTabCntrl.isCrossTabulation == false))
                    {
                        rvc.SelectTabCntrl.lstToSelecteColFrom.IsEnabled = true;
                        rvc.SelectTabCntrl.lstToSelecteColFrom.ItemsSource = mainWindow.GenerateListOfSelectTabCntrlColumns(rvc);
                    }
                    rvc.CustomQueryAccordion.LayoutChildren(true);

                }
                if (rvc.FromTabCntrl.StackPanelFromTab.Children.Count == 0)
                {
                    rvc.FromTabCntrl.DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Hidden;
                    UpdateAllControls(rvc);
                    rvc.CustomQueryAccordion.LayoutChildren(true);

                    /************Update query**************/
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
                    /**************************************/
                }
            }
        }

        public DependencyObject GetRVCFromTab(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                if (tmp.DependencyObjectType.Name == "FromTabControl")
                {
                    parent = tmp;
                    break;
                }
            }
            return parent;
        }

        public DependencyObject GetRVC(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                if (tmp.DependencyObjectType.Name == "ResultViewControl")
                {
                    parent = tmp;
                    break;
                }
            }
            return parent;
        }
        
        public DependencyObject GetRVCFromStack(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                if (tmp.DependencyObjectType.Name == "FromTabStackPanelControl")
                {
                    parent = tmp;
                    break;
                }
            }
            return parent;
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

        private void cmbFromTabJoinTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbFromTabJoinTable.Style = ComboboxOriginalStyle;
            ComboBox currentCmb = (ComboBox)sender;
            int stkpanelNumberToCmbJoinColumClear = -1;
            MainWindow mainWindow = new MainWindow { WindowStartupLocation = WindowStartupLocation.CenterOwner, Owner = Window.GetWindow(this) };

            DependencyObject parent = GetRVC(this.cmbFromTabJoinTable);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (rvc.FromTabCntrl.StackPanelFromTab.Children.Count > 1)
                {

                    if (currentCmb.SelectedIndex > -1)
                    {
                        bool isSelectedTableThere = false;
                        for (int i = 0; i < rvc.FromTabCntrl.StackPanelFromTab.Children.Count; i++)
                        {
                            FromTabStackPanelControl fs = (FromTabStackPanelControl)rvc.FromTabCntrl.StackPanelFromTab.Children[i];
                            if (fs.cmbFromTabJoinTable != currentCmb)
                            {
                                if (fs.cmbFromTabJoinTable.SelectedIndex != currentCmb.SelectedIndex)
                                {
                                    isSelectedTableThere = false;
                                }
                                else
                                {
                                    isSelectedTableThere = true;
                                    stkpanelNumberToCmbJoinColumClear = i;
                                    currentCmb.SelectedIndex = -1;
                                    break;
                                }
                            }
                        }
                        if (isSelectedTableThere == false)
                        {
                            LoadJoinTableColumnComboBox();
                        }
                    }
                }
                else
                {
                    UpdateAllControls(rvc);
                    LoadJoinTableColumnComboBox();
                }
            }
        }

        private void LoadJoinTableColumnComboBox()
        {
            MainWindow mainWindow = new MainWindow { WindowStartupLocation = WindowStartupLocation.CenterOwner, Owner = Window.GetWindow(this) };
            if (this.cmbFromTabJoinTable.SelectedIndex != -1)
            {
                if (mainWindow.listOfTable != null & mainWindow.listOfTable.Count > 0)
                {
                    this.cmbFromTabJoinColumns.ItemsSource = GetFromTabColums(mainWindow.listOfTable[this.cmbFromTabJoinTable.SelectedIndex].columns);
                }
            }
        }

        private List<string> GetFromTabColums(List<MySQLData.Column> listOfColumns)
        {
            List<string> list = new List<string>();
            foreach (MySQLData.Column column in listOfColumns)
            {
                list.Add(column.name);
            }
            return list;
        }
        
        private void cmbFromTabFromColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbFromTabFromColumns.Style = ComboboxOriginalStyle;
        }

        private void cmbFromTabQueryOpretor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbFromTabQueryOpretor.Style = ComboboxOriginalStyle;
        }

        private void cmbFromTabJoinColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbFromTabJoinColumns.Style = ComboboxOriginalStyle;

            /********Validate********/
            DependencyObject parent = GetRVC(this.cmbFromTabJoinTable);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /************************/
        }

        private void UpdateXmlQuery()
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbFromTabJoinColumns);

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

        private void txtJoinTableAlias_TextChanged(object sender, TextChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.txtJoinTableAlias.BorderBrush = TextBoxOriginalBorderBrush;// TextBoxOriginalStyle;
        }

        private void txtJoinTableAlias_LostFocus(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow { WindowStartupLocation = WindowStartupLocation.CenterOwner, Owner = Window.GetWindow(this) };
            DependencyObject parent = GetRVC(this.cmbFromTabJoinTable);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                UpdateAllControls(rvc);
            }
        }
        
        private void btnAddMore_Click(object sender, RoutedEventArgs e)
        {
            AddFromTabStackPanel();
           
            FromTabStackPanelControlMore fromTabStackPanelCntrlMore = new FromTabStackPanelControlMore();
            DependencyObject parent = fromTabStackPanelCntrlMore.GetRVC(this.cmbFromTabJoinType);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.CustomQueryAccordion.LayoutChildren(true);
            }
            
            
        }
        
        private void AddFromTabStackPanel()
        {
            if (this.cmbFromTabJoinType.SelectedIndex != -1)
            {
                MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTabJoinType);
                FromTabStackPanelControl FromTabStackPanelControl = (FromTabStackPanelControl)GetRVCFromStack(this.cmbFromTabJoinType);
                int numberOfStackPanel = FromTabStackPanelControl.StackPanelFromTabMore.Children.Count;
                FromTabStackPanelControlMore fs = new FromTabStackPanelControlMore();
                Validate();
                
                fs.Name = "Fs2";
                fs.Margin = new Thickness(0, 5, 0, 5);
                fs.btndeletemore.Visibility = System.Windows.Visibility.Visible;
                fs.btndeletemore.Uid = (numberOfStackPanel + 1).ToString();
                fs.btndeletemore.Width = 25.0;

                LoadFromTableColumnComboBox(fs);
                this.StackPanelFromTabMore.Children.Add(fs);
            }
        }
        
        public Boolean Validate()
        {
            bool validated = true;
            if (this.cmbFromTabJoinType.SelectedIndex == -1)
            {
                this.cmbFromTabJoinType.Style = null;
                this.cmbFromTabJoinType.BorderBrush = Brushes.Red;
                validated = false;
            }
            for (int i = 0; i < this.StackPanelFromTabMore.Children.Count; i++)
            {
                FromTabStackPanelControlMore fsm = (FromTabStackPanelControlMore)this.StackPanelFromTabMore.Children[i];
                if (fsm.cmbFromTabFromANDOR.SelectedIndex == -1)
                {
                    fsm.cmbFromTabFromANDOR.Style = null;
                    fsm.cmbFromTabFromANDOR.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fsm.cmbFromTabFromANDOR.SelectedIndex == -1)
                {
                    fsm.cmbFromTabFromANDOR.Style = null;
                    fsm.cmbFromTabFromANDOR.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fsm.cmbFromTabFromColumns.SelectedIndex == -1 & fsm.cmbFromTabFromColumns.Text == "")
                {
                    fsm.cmbFromTabFromColumns.Style = null;
                    fsm.cmbFromTabFromColumns.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fsm.cmbFromTabQueryOpretor.SelectedIndex == -1)
                {
                    fsm.cmbFromTabQueryOpretor.Style = null;
                    fsm.cmbFromTabQueryOpretor.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fsm.cmbFromTabJoinColumns.SelectedIndex == -1 & fsm.cmbFromTabJoinColumns.Text == "")
                {
                    fsm.cmbFromTabJoinColumns.Style = null;
                    fsm.cmbFromTabJoinColumns.BorderBrush = Brushes.Red;
                    validated = false;
                }
            }
            if (validated)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
            }
            validated = true;
            return validated;
        }
       
        private void cmbFromTabJoinType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cmbFromTabJoinType.Style = ComboboxOriginalStyle;
            if (this.cmbFromTabJoinType.SelectedIndex > -1)
            {
                FromTabStackPanelControl fromTabSPCntrl = new FromTabStackPanelControl();
                DependencyObject parent = fromTabSPCntrl.GetRVC(this.cmbFromTabJoinType);

                if (parent != null)
                {
                    ResultViewControl rvc = (ResultViewControl)parent;
                    fromTabSPCntrl.UpdateAllControls(rvc);
                    rvc.SelectTabCntrl._SelectedColCollection.Clear();
                }
                this.cmbFromTabJoinType.Style = ComboboxOriginalStyle;
                for (int i = 0; i < this.StackPanelFromTabMore.Children.Count; i++)
                {
                    FromTabStackPanelControlMore fs = (FromTabStackPanelControlMore)this.StackPanelFromTabMore.Children[i];
                    LoadFromTableColumnComboBox(fs);
                }
            }
        }
        
        private void LoadFromTableColumnComboBox(FromTabStackPanelControlMore fs)
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTabJoinType);

            FromTabControl fromTabCntrl = new FromTabControl();
            DependencyObject parent1 = this.GetRVCFromTab(this.cmbFromTabFromColumns);

            if (parent1 != null)
            {
                fromTabCntrl = (FromTabControl)parent1;
            }

            if (this.cmbFromTabJoinType.SelectedIndex != -1)
            {
                if (mainWindow.listOfTable != null & mainWindow.listOfTable.Count > 0)
                {
                    fs.cmbFromTabFromColumns.Text = "";
                    fs.cmbFromTabFromColumns.ItemsSource = Common.ConvertColumsToStringList(mainWindow.listOfTable[fromTabCntrl.cmbFromTable.SelectedIndex].columns);
                    if (this.cmbFromTabJoinTable.SelectedIndex != -1)
                    {
                        fs.cmbFromTabJoinColumns.ItemsSource = Common.ConvertColumsToStringList(mainWindow.listOfTable[this.cmbFromTabJoinTable.SelectedIndex].columns);
                    }
                }
            }
        }
    }
}
