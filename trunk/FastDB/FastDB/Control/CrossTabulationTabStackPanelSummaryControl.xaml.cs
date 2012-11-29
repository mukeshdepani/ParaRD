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
    /// Interaction logic for CrossTabulationTabStackPanelSummaryControl.xaml
    /// </summary>
    public partial class CrossTabulationTabStackPanelSummaryControl : UserControl
    {
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        private ObservableCollection<SQLBuilder.Clauses.Column> _CrossTabulationTabStackPanelSummaryColumns;
        public CrossTabulationTabStackPanelSummaryControl()
        {
            InitializeComponent();
            this.cmbCrossTabulationTabSummaryColumnsName.ItemsSource = _CrossTabulationTabStackPanelSummaryColumns;
            //set following  after row is created
            ComboboxOriginalStyle = this.cmbCrossTabulationTabSummaryColumnsName.Style;
            TextBoxOriginalBorderBrush = this.txtCrossTabulationTabSummaryAlias.BorderBrush;
            this.cmbCrossTabulationTypeOfSummary.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.GroupFunction)).ToList();
            this.cmbCrossTabulationTabUserSelectSummaryColFormat.ItemsSource = Common.GetColumsFormatList();
        }

        private void cmbCrossTabulationTabSummaryColumnsName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cmbCrossTabulationTabSummaryColumnsName.Style = ComboboxOriginalStyle;
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbCrossTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                if (this.cmbCrossTabulationTabSummaryColumnsName.SelectedItem != null)
                {
                    this.txtCrossTabulationTabSummaryAlias.Text = this.cmbCrossTabulationTabSummaryColumnsName.SelectedItem.ToString();
                    this.txtCrossTabulationTabSummaryAlias.Text = this.txtCrossTabulationTabSummaryAlias.Text.Substring(txtCrossTabulationTabSummaryAlias.Text.IndexOf('.') + 1); 
                    this.txtCrossTabulationTabSummaryAlias.Text = this.txtCrossTabulationTabSummaryAlias.Text.Replace("_", " ");
                    this.txtCrossTabulationTabSummaryAlias.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.txtCrossTabulationTabSummaryAlias.Text);
                }
                ResultViewControl rvc = (ResultViewControl)parent;
            }
        }

        private void cmbCrossTabulationTypeOfSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbCrossTabulationTypeOfSummary.Style = ComboboxOriginalStyle;
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbCrossTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
        }

        private void txtCrossTabulationTabSummaryAlias_TextChanged(object sender, TextChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.txtCrossTabulationTabSummaryAlias.BorderBrush = TextBoxOriginalBorderBrush;
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbCrossTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
        }

        private void cmbCrossTabulationTabUserSelectSummaryColFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedIndex != -1)
            {
                this.txtCrossTabulationTabSummaryColFormat.Text = this.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectedValue.ToString();
            }
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbCrossTabulationTabSummaryColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
        }
    }
}
