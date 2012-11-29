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
using System.Globalization;

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for CrossTabulationTabStackPanelGroupByControl.xaml
    /// </summary>
    public partial class CrossTabulationTabStackPanelGroupByControl : UserControl
    {
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        private ObservableCollection<SQLBuilder.Clauses.Column> _CrossTabulationTabStackPanelGroupByColumns;
        public CrossTabulationTabStackPanelGroupByControl()
        {
            InitializeComponent();
            this.cmbCrossTabulationTabGroupByColumnsName.ItemsSource = _CrossTabulationTabStackPanelGroupByColumns;
            this.cmbCrossTabulationSort.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.Sorting)).ToList();
            ComboboxOriginalStyle = this.cmbCrossTabulationTabGroupByColumnsName.Style;
            TextBoxOriginalBorderBrush = this.txtCrossTabulationTabGroupByAlias.BorderBrush;
        }

        private void txtCrossTabulationTabGroupByAlias_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txtCrossTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbCrossTabulationTabGroupByColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
        }

        private void cmbCrossTabulationTabGroupByColumnsName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((SQLBuilder.Clauses.Column)this.cmbCrossTabulationTabGroupByColumnsName.SelectedItem != null)
            {
                this.txtCrossTabulationTabGroupByColFormat.Text = ((SQLBuilder.Clauses.Column)this.cmbCrossTabulationTabGroupByColumnsName.SelectedItem).Format;
                this.txtCrossTabulationTabGroupByAlias.Text = this.cmbCrossTabulationTabGroupByColumnsName.SelectedItem.ToString();
                this.txtCrossTabulationTabGroupByAlias.Text = this.txtCrossTabulationTabGroupByAlias.Text.Substring(txtCrossTabulationTabGroupByAlias.Text.IndexOf('.') + 1);
                this.txtCrossTabulationTabGroupByAlias.Text = this.txtCrossTabulationTabGroupByAlias.Text.Replace("_", " ");

                this.txtCrossTabulationTabGroupByAlias.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.txtCrossTabulationTabGroupByAlias.Text);
                this.cmbCrossTabulationSort.SelectedItem = "Ascending";
            }

            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbCrossTabulationTabGroupByColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }

        }
    }
}
