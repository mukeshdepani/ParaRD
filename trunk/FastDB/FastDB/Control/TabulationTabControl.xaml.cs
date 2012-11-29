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
    /// Interaction logic for TabulationTabControl.xaml
    /// </summary>
    public partial class TabulationTabControl : UserControl
    {
        public bool isValidated;
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        //isTabulation indicates that user wants to Tabulation
        public bool isTabulation;
        
        public TabulationTabControl()
        {
            InitializeComponent();
            isValidated = true;
            isTabulation = false;
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

            //set following  after row is created
            ComboboxOriginalStyle = ((TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[0]).cmbTabulationTabGroupByColumnsName.Style;
            TextBoxOriginalBorderBrush = ((TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[0]).txtTabulationTabGroupByAlias.BorderBrush;
        }
        
        public void AddGroupByRow()
        {
            TabulationTabStackPanelGroupByControl tg = new TabulationTabStackPanelGroupByControl();
            tg.Name = "tg1";
            tg.btnDelete.Visibility = System.Windows.Visibility.Hidden;
            this.StackPanelTabuLationTabGroupBy.Children.Add(tg);
        }
        
        public void AddSummaryRow()
        {
            TabulationTabStackPanelSummaryControl ts = new TabulationTabStackPanelSummaryControl();
            ts.Name = "ts1";
            ts.btnDelete.Visibility = System.Windows.Visibility.Hidden;
            this.StackPanelTabuLationTabSummary.Children.Add(ts);
        }
        
        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            Validate();
        }
        
        public Boolean Validate()
        {
            bool validated = isValidated;
            bool isGroupByValidated = false;
            bool isSummaryValidated = false;
            int groupByCount = 0;
            int summaryCount = 0;
            // checking groupby columns
            for (int i = 0; i < this.StackPanelTabuLationTabGroupBy.Children.Count; i++)
            {
                TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[i];
                if (tg.cmbTabulationTabGroupByColumnsName.Text != System.String.Empty)
                {
                    groupByCount = groupByCount + 1;
                    isGroupByValidated = true;
                }

            }
            if (groupByCount == 0)
            {
                isGroupByValidated = true;
            }
            //check we have atleast one summary row
            for (int i = 0; i < this.StackPanelTabuLationTabSummary.Children.Count; i++)
            {
                TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.StackPanelTabuLationTabSummary.Children[i];
                if (ts.cmbTabulationTabSummaryColumnsName.Text != System.String.Empty & ts.cmbTabulationTypeOfSummary.Text != System.String.Empty & ts.txtTabulationTabSummaryAlias.Text != System.String.Empty)
                {
                    summaryCount = summaryCount + 1;
                    break;
                }
            }
            //if we have one or more groupby then we must have atleast 1 complete summary row
            if (isGroupByValidated & groupByCount != 0)
            {
                if (summaryCount != 0)
                {
                    isSummaryValidated = true;
                }

            }
            // in case of if we don't have group by then we don't have to have summary
            if (isGroupByValidated & groupByCount == 0)
            {
                isSummaryValidated = true;
                // but check what if user provide summary but there is no group by
                if (summaryCount != 0)
                {
                    isGroupByValidated = false;
                }

            }
            // checking summary rows if user provides value for rows

            for (int i = 0; i < this.StackPanelTabuLationTabSummary.Children.Count; i++)
            {
                TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.StackPanelTabuLationTabSummary.Children[i];

                if (ts.cmbTabulationTabSummaryColumnsName.Text != System.String.Empty)
                {
                    if (ts.cmbTabulationTypeOfSummary.Text == System.String.Empty)
                    {
                        ts.cmbTabulationTypeOfSummary.Style = null;
                        ts.cmbTabulationTypeOfSummary.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                    if (ts.txtTabulationTabSummaryAlias.Text == System.String.Empty)
                    {
                        ts.txtTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                }
                if (ts.cmbTabulationTypeOfSummary.Text != System.String.Empty)
                {
                    if (ts.cmbTabulationTabSummaryColumnsName.Text == System.String.Empty)
                    {
                        ts.cmbTabulationTabSummaryColumnsName.Style = null;
                        ts.cmbTabulationTabSummaryColumnsName.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                    if (ts.txtTabulationTabSummaryAlias.Text == System.String.Empty)
                    {
                        ts.txtTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                }
                if (ts.txtTabulationTabSummaryAlias.Text != System.String.Empty)
                {
                    if (ts.cmbTabulationTabSummaryColumnsName.Text == System.String.Empty)
                    {
                        ts.cmbTabulationTabSummaryColumnsName.Style = null;
                        ts.cmbTabulationTabSummaryColumnsName.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                    if (ts.cmbTabulationTypeOfSummary.Text == System.String.Empty)
                    {
                        ts.cmbTabulationTypeOfSummary.Style = null;
                        ts.cmbTabulationTypeOfSummary.BorderBrush = Brushes.Red;
                        isSummaryValidated = false;
                    }
                }

                // sometimes if textbox has value but border does not turn back to normal
                if ((ts.cmbTabulationTabSummaryColumnsName.Text != System.String.Empty) & (ts.cmbTabulationTypeOfSummary.Text != System.String.Empty) & (ts.txtTabulationTabSummaryAlias.Text != System.String.Empty))
                {
                    ts.txtTabulationTabSummaryAlias.BorderBrush = TextBoxOriginalBorderBrush;
                }
            }

            //checking for group by duplicate alias
            for (int d = 0; d < this.StackPanelTabuLationTabGroupBy.Children.Count; d++)
            {
                TabulationTabStackPanelGroupByControl tg1 = (TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[d];
                for (int i = 0; i < this.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelGroupByControl tg2 = (TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[i];
                    if (tg1.txtTabulationTabGroupByAlias != tg2.txtTabulationTabGroupByAlias)
                    {
                        if (tg1.txtTabulationTabGroupByAlias.Text != System.String.Empty)
                        {
                            if (tg1.txtTabulationTabGroupByAlias.Text == tg2.txtTabulationTabGroupByAlias.Text)
                            {
                                tg1.txtTabulationTabGroupByAlias.BorderBrush = Brushes.Red;
                                isGroupByValidated = false;
                                break;
                            }
                        }

                    }
                    else
                    {
                        tg1.txtTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                        tg2.txtTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                    }
                }

            }
            // checking for summary duplicate alias
            for (int d = 0; d < this.StackPanelTabuLationTabSummary.Children.Count; d++)
            {
                TabulationTabStackPanelSummaryControl ts1 = (TabulationTabStackPanelSummaryControl)this.StackPanelTabuLationTabSummary.Children[d];
                for (int i = 0; i < this.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelSummaryControl ts2 = (TabulationTabStackPanelSummaryControl)this.StackPanelTabuLationTabSummary.Children[i];
                    if (ts1.txtTabulationTabSummaryAlias != ts2.txtTabulationTabSummaryAlias)
                    {
                        if (ts1.txtTabulationTabSummaryAlias.Text != System.String.Empty)
                        {
                            if (ts1.txtTabulationTabSummaryAlias.Text == ts2.txtTabulationTabSummaryAlias.Text)
                            {
                                ts1.txtTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                                isSummaryValidated = false;
                                break;
                            }
                        }
                    }
                }

            }
            //checking each other for duplicate
            for (int d = 0; d < this.StackPanelTabuLationTabSummary.Children.Count; d++)
            {
                TabulationTabStackPanelSummaryControl ts1 = (TabulationTabStackPanelSummaryControl)this.StackPanelTabuLationTabSummary.Children[d];
                for (int i = 0; i < this.StackPanelTabuLationTabGroupBy.Children.Count; i++)
                {
                    TabulationTabStackPanelGroupByControl tg2 = (TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[i];
                    if (ts1.txtTabulationTabSummaryAlias != tg2.txtTabulationTabGroupByAlias)
                    {
                        if (ts1.txtTabulationTabSummaryAlias.Text != System.String.Empty)
                        {
                            if (ts1.txtTabulationTabSummaryAlias.Text == tg2.txtTabulationTabGroupByAlias.Text)
                            {
                                ts1.txtTabulationTabSummaryAlias.BorderBrush = Brushes.Red;
                                isSummaryValidated = false;
                                break;
                            }
                        }

                    }
                    else
                    {
                        tg2.txtTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                    }
                }

            }

            if (isSummaryValidated == false)
            {
                if (summaryCount == 0)
                {
                    if (groupByCount == 0)
                    {
                        this.lblErrorMessage.Content = "****Please provide group by****";
                    }
                    else
                    {
                        this.lblErrorMessage.Content = "****Please provide atleast one summary****";
                    }
                }
                else
                {
                    this.lblErrorMessage.Content = "****Please provide the value for control in red****";
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
                    isTabulation = true;
                }
                else
                {
                    isTabulation = false;
                }
                this.lblErrorMessage.Content = "";
            }
            else
            {
                validated = false;
            }
            return validated;
        }

        private void btnRest_Click(object sender, RoutedEventArgs e)
        {
            this.lblErrorMessage.Content = "";
            for (int i = 0; i < this.StackPanelTabuLationTabGroupBy.Children.Count; i++)
            {
                TabulationTabStackPanelGroupByControl tg = (TabulationTabStackPanelGroupByControl)this.StackPanelTabuLationTabGroupBy.Children[i];
                tg.cmbTabulationTabGroupByColumnsName.Style = ComboboxOriginalStyle;
                tg.cmbTabulationSort.Style = ComboboxOriginalStyle;
                tg.txtTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
                tg.cmbTabulationTabGroupByColumnsName.SelectedIndex = -1;
                tg.cmbTabulationSort.SelectedIndex = -1;
                tg.txtTabulationTabGroupByAlias.Text = "";
            }
            for (int i = 0; i < this.StackPanelTabuLationTabSummary.Children.Count; i++)
            {
                TabulationTabStackPanelSummaryControl ts = (TabulationTabStackPanelSummaryControl)this.StackPanelTabuLationTabSummary.Children[i];
                ts.cmbTabulationTabSummaryColumnsName.Style = ComboboxOriginalStyle;
                ts.cmbTabulationTypeOfSummary.Style = ComboboxOriginalStyle;
                ts.txtTabulationTabSummaryAlias.BorderBrush = TextBoxOriginalBorderBrush;
                ts.cmbTabulationTabSummaryColumnsName.SelectedIndex = -1;
                ts.cmbTabulationTypeOfSummary.SelectedIndex = -1;
                ts.txtTabulationTabSummaryAlias.Text = "";
            }
        }
    }
}
