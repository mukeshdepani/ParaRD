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
    public partial class FromTabStackPanelControlMore : UserControl
    {
        private Style ComboboxOriginalStyle;
        private string queryString;
        private Brush TextBoxOriginalBorderBrush;
        private List<SQLBuilder.Clauses.Column> lstToSelectColsFrom;

        public FromTabStackPanelControlMore()
        {
            InitializeComponent();
            ComboboxOriginalStyle = this.cmbFromTabFromANDOR.Style;
            List<string> ListOfLogicalOpreator = new List<string> { "And", "Or" };
            this.cmbFromTabFromANDOR.ItemsSource = ListOfLogicalOpreator;
            TextBoxOriginalBorderBrush = this.cmbFromTabFromANDOR.BorderBrush;

            this.cmbFromTabQueryOpretor.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.Comparison)).ToList();
            this.cmbFromTabQueryOpretor.SelectedIndex = 0;
            

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
            this.cmbFromTabFromColumns.Style = ComboboxOriginalStyle;
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
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btnDelete = (Button)sender;
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTabFromANDOR);
            FromTabStackPanelControl FromTabStackPanelControl = (FromTabStackPanelControl)GetRVCFromStack(this.cmbFromTabFromANDOR);
            DependencyObject parent = GetRVC(this.cmbFromTabFromANDOR);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (FromTabStackPanelControl.StackPanelFromTabMore.Children.Count > 0)
                {
                    DockPanel dkp = (DockPanel)btnDelete.Parent;
                    FromTabStackPanelControlMore fsp = (FromTabStackPanelControlMore)((Grid)((StackPanel)dkp.Parent).Parent).Parent;
                    FromTabStackPanelControl.StackPanelFromTabMore.Children.Remove(fsp);
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
    }
}
