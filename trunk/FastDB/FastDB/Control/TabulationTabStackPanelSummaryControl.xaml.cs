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
using FastDB.Class;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for TabulationTabStackPanelSummaryControl.xaml
    /// </summary>
    
    public partial class TabulationTabStackPanelSummaryControl : UserControl
    {
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        private ObservableCollection<SQLBuilder.Clauses.Column> _TabulationTabStackPanelSummaryColumns;
        
        public TabulationTabStackPanelSummaryControl()
        {
            InitializeComponent();
            this.cmbTabulationTabSummaryColumnsName.ItemsSource = _TabulationTabStackPanelSummaryColumns;
            //set following  after row is created
            ComboboxOriginalStyle = this.cmbTabulationTabSummaryColumnsName.Style;
            TextBoxOriginalBorderBrush = this.txtTabulationTabSummaryAlias.BorderBrush;
            this.cmbTabulationTypeOfSummary.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.GroupFunction)).ToList();
            this.cmbTabulationTabUserSelectSummaryColFormat.ItemsSource = Common.GetColumsFormatList();
        }
       
        private List<string> GetTypeOfSummaryAggregateFunction()
        {
            List<string> list = new List<string>();
            list.Add("Sum");
            list.Add("Count");
            list.Add("Average");
            return list;
        }

        private void cmbTabulationTabSummaryColumnsName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbTabulationTabSummaryColumnsName.Style = ComboboxOriginalStyle;
            
            /****************************************************/
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                if (this.cmbTabulationTabSummaryColumnsName.SelectedItem != null)
                {
                    this.txtTabulationTabSummaryAlias.Text = this.cmbTabulationTabSummaryColumnsName.SelectedItem.ToString();
                    this.txtTabulationTabSummaryAlias.Text = this.txtTabulationTabSummaryAlias.Text.Substring(txtTabulationTabSummaryAlias.Text.IndexOf('.') + 1); 
                    this.txtTabulationTabSummaryAlias.Text = this.txtTabulationTabSummaryAlias.Text.Replace("_", " ");
                    this.txtTabulationTabSummaryAlias.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.txtTabulationTabSummaryAlias.Text);
                }
                
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /****************************************************/
        }

        private void cmbTabulationTypeOfSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbTabulationTypeOfSummary.Style = ComboboxOriginalStyle;

            /****************************************************/
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /****************************************************/
        }

        private void txtTabulationTabSummaryAlias_TextChanged(object sender, TextChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.txtTabulationTabSummaryAlias.BorderBrush = TextBoxOriginalBorderBrush;
           
            /****************************************************/
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /****************************************************/
        }

        private void cmbTabulationTabUserSelectSummaryColFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbTabulationTabUserSelectSummaryColFormat.SelectedIndex != -1)
            {
                this.txtTabulationTabSummaryColFormat.Text = this.cmbTabulationTabUserSelectSummaryColFormat.SelectedValue.ToString();
            }

            /****************************************************/
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /****************************************************/
        }
    }
}
