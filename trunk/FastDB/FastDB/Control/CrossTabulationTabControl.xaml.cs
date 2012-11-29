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
using System.Collections;
using System.Collections.ObjectModel;
namespace FastDB.Control
{
    public partial class CrossTabulationTabControl : UserControl
    {
        public bool isValidated;
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        //isTabulation indicates that user wants to Tabulation
        public bool isCrossTabulation;
        private ObservableCollection<SQLBuilder.Clauses.Column> _CrossTabulationTabSummaryFirstRowColumns;
        public CrossTabulationTabControl()
        {
            InitializeComponent();
            this.cmbCrossTabulationTabSummaryFirstRowColumnsName.ItemsSource = _CrossTabulationTabSummaryFirstRowColumns;
            isValidated = true;
            isCrossTabulation = false;

            //add 3 groupby row
            AddGroupByRow();
            AddGroupByRow();
            AddGroupByRow();

            //add 6 summary rows
            AddSummaryRow();
            AddSummaryRow();
            AddSummaryRow();
            AddSummaryRow();
            AddSummaryRow();
            AddSummaryRow();
            //seting summary first row
            this.cmbCrossTabulationTabSummaryFristRowSort.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.Sorting)).ToList();
            this.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex = 0;
            //set following  after row is created
            ComboboxOriginalStyle = ((CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[0]).cmbCrossTabulationTabGroupByColumnsName.Style;
            TextBoxOriginalBorderBrush = ((CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[0]).txtCrossTabulationTabGroupByAlias.BorderBrush;
        }
       
        private void btnRest_Click(object sender, RoutedEventArgs e)
        {
            this.lblErrorMessage.Content = "";
            for (int i = 0; i < this.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
            {
                CrossTabulationTabStackPanelGroupByControl ctg = (CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[i];
                ctg.cmbCrossTabulationTabGroupByColumnsName.Style = ComboboxOriginalStyle;
                ctg.cmbCrossTabulationSort.Style = ComboboxOriginalStyle;
                ctg.txtCrossTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                ctg.cmbCrossTabulationTabGroupByColumnsName.SelectedIndex = -1;
                ctg.cmbCrossTabulationSort.SelectedIndex = -1;
                ctg.txtCrossTabulationTabGroupByAlias.Text = "";
            }
            this.cmbCrossTabulationTabSummaryFirstRowColumnsName.Style = ComboboxOriginalStyle;
            this.cmbCrossTabulationTabSummaryFristRowSort.Style = ComboboxOriginalStyle;
            this.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectedIndex = -1;
            this.cmbCrossTabulationTabSummaryFristRowSort.SelectedIndex = -1;

            for (int i = 0; i < this.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
            {
                CrossTabulationTabStackPanelSummaryControl cts = (CrossTabulationTabStackPanelSummaryControl)this.StackPanelCrossTabuLationTabSummary.Children[i];
                cts.cmbCrossTabulationTabSummaryColumnsName.Style = ComboboxOriginalStyle;
                cts.cmbCrossTabulationTypeOfSummary.Style = ComboboxOriginalStyle;
                cts.txtCrossTabulationTabSummaryAlias.BorderBrush = TextBoxOriginalBorderBrush;
                cts.cmbCrossTabulationTabSummaryColumnsName.SelectedIndex = -1;
                cts.cmbCrossTabulationTypeOfSummary.SelectedIndex = -1;
                cts.txtCrossTabulationTabSummaryAlias.Text = "";
            }
            

        }

        public void AddGroupByRow()
        {
            CrossTabulationTabStackPanelGroupByControl ctg = new CrossTabulationTabStackPanelGroupByControl();
            ctg.Name = "ctg1";
            ctg.btnCrossTabGroupByDelete.Visibility = System.Windows.Visibility.Hidden;
            this.StackPanelCrossTabuLationTabGroupBy.Children.Add(ctg);
        }

        public void AddSummaryRow()
        {
            CrossTabulationTabStackPanelSummaryControl cts = new CrossTabulationTabStackPanelSummaryControl();
            cts.Name = "cts1";
            cts.btnCrossTabSummaryDelete.Visibility = System.Windows.Visibility.Hidden;
            this.StackPanelCrossTabuLationTabSummary.Children.Add(cts);
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            Validate();
        }

        public Boolean Validate()
        {
            bool validated = isValidated;
            bool isGroupByValidated = false;
            bool IsSummaryFristRowValidate = false;
            bool isSummaryValidated = false;
            int groupByCount = 0;
            int summaryCount = 0;
            // checking groupby columns
            for (int i = 0; i < this.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
            {
                CrossTabulationTabStackPanelGroupByControl tg = (CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[i];
                if (tg.cmbCrossTabulationTabGroupByColumnsName.Text != System.String.Empty)
                {
                    groupByCount = groupByCount + 1;
                    isGroupByValidated = true;
                }

            }
            if (groupByCount == 0)
            {
                isGroupByValidated = true;
            }
            //check how many summary we have
            //check we have atleast one summary row
            for (int i = 0; i < this.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
            {
                CrossTabulationTabStackPanelSummaryControl ts = (CrossTabulationTabStackPanelSummaryControl)this.StackPanelCrossTabuLationTabSummary.Children[i];
                if (ts.cmbCrossTabulationTabSummaryColumnsName.Text != System.String.Empty & ts.cmbCrossTabulationTypeOfSummary.Text != System.String.Empty & ts.txtCrossTabulationTabSummaryAlias.Text != System.String.Empty)
                {
                    summaryCount = summaryCount + 1;
                    break;
                }
            }
            //check summary first row is valid
            if (this.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectedIndex != -1)
            {
                
                if (summaryCount!=0)
                {
                    IsSummaryFristRowValidate = true;
                }
            }
            
            //if we have one or more groupby then we must have atleast 1 complete summary row
            if (isGroupByValidated & groupByCount != 0)
            {
                if (summaryCount != 0 & IsSummaryFristRowValidate)
                {
                    isSummaryValidated = true;
                }
                if (this.cmbCrossTabulationTabSummaryFirstRowColumnsName.SelectedIndex == -1)
                {
                    this.cmbCrossTabulationTabSummaryFirstRowColumnsName.Style = null;
                    this.cmbCrossTabulationTabSummaryFirstRowColumnsName.BorderBrush = Brushes.Red;
                }
            }
            // in case of if we don't have group by then we don't have to have summary
            if (isGroupByValidated & groupByCount == 0)
            {
                if (summaryCount == 0)
                {
                    isSummaryValidated = true;
                }
                // but check what if user provide summary but there is no group by
                if (summaryCount != 0 & IsSummaryFristRowValidate)
                {
                    isGroupByValidated = false;
                }

            }
            // checking summary rows if user provides value for rows

            for (int i = 0; i < this.StackPanelCrossTabuLationTabSummary.Children.Count; i++)
            {
                CrossTabulationTabStackPanelSummaryControl ts = (CrossTabulationTabStackPanelSummaryControl)this.StackPanelCrossTabuLationTabSummary.Children[i];

                if (ts.cmbCrossTabulationTabSummaryColumnsName.Text != System.String.Empty)
                {
                    if (ts.cmbCrossTabulationTypeOfSummary.Text == System.String.Empty)
                    {
                        ts.cmbCrossTabulationTypeOfSummary.Style = null;
                        ts.cmbCrossTabulationTypeOfSummary.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                    if (ts.txtCrossTabulationTabSummaryAlias.Text == System.String.Empty)
                    {
                        ts.txtCrossTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                }
                if (ts.cmbCrossTabulationTypeOfSummary.Text != System.String.Empty)
                {
                    if (ts.cmbCrossTabulationTabSummaryColumnsName.Text == System.String.Empty)
                    {
                        ts.cmbCrossTabulationTabSummaryColumnsName.Style = null;
                        ts.cmbCrossTabulationTabSummaryColumnsName.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                    if (ts.txtCrossTabulationTabSummaryAlias.Text == System.String.Empty)
                    {
                        ts.txtCrossTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                }
                if (ts.txtCrossTabulationTabSummaryAlias.Text != System.String.Empty)
                {
                    if (ts.cmbCrossTabulationTabSummaryColumnsName.Text == System.String.Empty)
                    {
                        ts.cmbCrossTabulationTabSummaryColumnsName.Style = null;
                        ts.cmbCrossTabulationTabSummaryColumnsName.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                    if (ts.cmbCrossTabulationTypeOfSummary.Text == System.String.Empty)
                    {
                        ts.cmbCrossTabulationTypeOfSummary.Style = null;
                        ts.cmbCrossTabulationTypeOfSummary.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                }

                // sometimes if textbox has value but border does not turn back to normal
                if ((ts.cmbCrossTabulationTabSummaryColumnsName.Text != System.String.Empty) & (ts.cmbCrossTabulationTypeOfSummary.Text != System.String.Empty) & (ts.txtCrossTabulationTabSummaryAlias.Text != System.String.Empty))
                {
                    ts.txtCrossTabulationTabSummaryAlias.BorderBrush = TextBoxOriginalBorderBrush;
                }
            }

            //checking for group by duplicate alias
            for (int d = 0; d < this.StackPanelCrossTabuLationTabGroupBy.Children.Count; d++)
            {
                CrossTabulationTabStackPanelGroupByControl tg1 = (CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[d];
                for (int i = 0; i < this.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelGroupByControl tg2 = (CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[i];
                    if (tg1.txtCrossTabulationTabGroupByAlias != tg2.txtCrossTabulationTabGroupByAlias)
                    {
                        if (tg1.txtCrossTabulationTabGroupByAlias.Text != System.String.Empty)
                        {
                            if (tg1.txtCrossTabulationTabGroupByAlias.Text == tg2.txtCrossTabulationTabGroupByAlias.Text)
                            {
                                tg1.txtCrossTabulationTabGroupByAlias.BorderBrush = Brushes.Red;
                                isGroupByValidated = false;
                                break;
                            }
                        }

                    }
                    else
                    {
                        tg1.txtCrossTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                        tg2.txtCrossTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                    }
                }

            }
            // checking for summary duplicate alias
            for (int d = 0; d < this.StackPanelCrossTabuLationTabSummary.Children.Count; d++)
            {
                CrossTabulationTabStackPanelSummaryControl ts1 = (CrossTabulationTabStackPanelSummaryControl)this.StackPanelCrossTabuLationTabSummary.Children[d];
                for (int i = 0; i < this.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelSummaryControl ts2 = (CrossTabulationTabStackPanelSummaryControl)this.StackPanelCrossTabuLationTabSummary.Children[i];
                    if (ts1.txtCrossTabulationTabSummaryAlias != ts2.txtCrossTabulationTabSummaryAlias)
                    {
                        if (ts1.txtCrossTabulationTabSummaryAlias.Text != System.String.Empty)
                        {
                            if (ts1.txtCrossTabulationTabSummaryAlias.Text == ts2.txtCrossTabulationTabSummaryAlias.Text)
                            {
                                ts1.txtCrossTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                                isSummaryValidated = false;
                                break;
                            }
                        }
                    }
                }

            }
            //checking each other for duplicate
            for (int d = 0; d < this.StackPanelCrossTabuLationTabSummary.Children.Count; d++)
            {
                CrossTabulationTabStackPanelSummaryControl ts1 = (CrossTabulationTabStackPanelSummaryControl)this.StackPanelCrossTabuLationTabSummary.Children[d];
                for (int i = 0; i < this.StackPanelCrossTabuLationTabGroupBy.Children.Count; i++)
                {
                    CrossTabulationTabStackPanelGroupByControl tg2 = (CrossTabulationTabStackPanelGroupByControl)this.StackPanelCrossTabuLationTabGroupBy.Children[i];
                    if (ts1.txtCrossTabulationTabSummaryAlias != tg2.txtCrossTabulationTabGroupByAlias)
                    {
                        if (ts1.txtCrossTabulationTabSummaryAlias.Text != System.String.Empty)
                        {
                            if (ts1.txtCrossTabulationTabSummaryAlias.Text == tg2.txtCrossTabulationTabGroupByAlias.Text)
                            {
                                ts1.txtCrossTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                                isSummaryValidated = false;
                                break;
                            }
                        }

                    }
                    else
                    {
                        tg2.txtCrossTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                    }
                }

            }

            if (isSummaryValidated == false)
            {
                if (summaryCount != 0)
                {
                    if (groupByCount == 0)
                    {
                        this.lblErrorMessage.Content = "****Please provide group by****";
                    }
                    else
                    {
                        this.lblErrorMessage.Content = "****Please provide the value for control in red****";
                    }

                }
                else
                {
                    if (groupByCount != 0)
                    {
                        this.lblErrorMessage.Content = "****Please provide atleast one summary****";
                    }
                    else
                    {
                        this.lblErrorMessage.Content = "****Please provide the value for control in red****";
                    }
                }
            }
            else
            {
                if (isGroupByValidated == false & groupByCount == 0)
                {
                    this.lblErrorMessage.Content = "****Please provide group by****";
                }
            }
            // recheck summary for complete row
            if ((isGroupByValidated) & (isSummaryValidated))
            {
                validated = true;
                if (groupByCount != 0)
                {
                    isCrossTabulation = true;
                }
                else
                {
                    isCrossTabulation = false;
                }
                this.lblErrorMessage.Content = "";
            }
            else
            {
                validated = false;
            }
            return validated;
        }
        
        private void cmbCrossTabulationTabSummaryFirstRowColumnsName_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            /*******Validate necessary for saving*******/
            //this.Validate();
            /**********************/
        }

        private void cmbCrossTabulationTabSummaryFristRowSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*******Validate necessary for saving*******/
            //this.Validate();
            /**********************/
        }
    }
}
