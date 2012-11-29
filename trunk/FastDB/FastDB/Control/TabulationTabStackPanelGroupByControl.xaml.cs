using System;
using System.Collections;
using System.Collections.ObjectModel;
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
using SQLBuilder.Clauses;
using System.Globalization;

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for TabulationTabStackPanelGroupByControl.xaml
    /// </summary>
    public partial class TabulationTabStackPanelGroupByControl : UserControl
    {
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        private ObservableCollection<SQLBuilder.Clauses.Column> _TabulationTabStackPanelGroupByColumns;
        public TabulationTabStackPanelGroupByControl()
        {
            InitializeComponent();
           
            this.cmbTabulationTabGroupByColumnsName.ItemsSource = _TabulationTabStackPanelGroupByColumns;
            this.cmbTabulationSort.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.Sorting)).ToList();
            ComboboxOriginalStyle = this.cmbTabulationTabGroupByColumnsName.Style;
            TextBoxOriginalBorderBrush = this.txtTabulationTabGroupByAlias.BorderBrush;
        }

        private void txtTabulationTabGroupByAlias_TextChanged(object sender, TextChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.txtTabulationTabGroupByAlias.BorderBrush = TextBoxOriginalBorderBrush;
            /****************************************************/
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbTabulationTabGroupByColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /****************************************************/
        }

        private void cmbTabulationTabGroupByColumnsName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if ((SQLBuilder.Clauses.Column)this.cmbTabulationTabGroupByColumnsName.SelectedItem != null)
            {
                this.txtTabulationTabGroupByColFormat.Text = ((SQLBuilder.Clauses.Column)this.cmbTabulationTabGroupByColumnsName.SelectedItem).Format;
                this.txtTabulationTabGroupByAlias.Text = this.cmbTabulationTabGroupByColumnsName.SelectedItem.ToString();
                this.txtTabulationTabGroupByAlias.Text = this.txtTabulationTabGroupByAlias.Text.Substring(txtTabulationTabGroupByAlias.Text.IndexOf('.') + 1); 
                this.txtTabulationTabGroupByAlias.Text = this.txtTabulationTabGroupByAlias.Text.Replace("_", " ");
                this.txtTabulationTabGroupByAlias.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.txtTabulationTabGroupByAlias.Text);
                this.cmbTabulationSort.SelectedItem = "Ascending";
            }
            
            this.cmbTabulationTabGroupByColumnsName.Style = ComboboxOriginalStyle;

            /****************************************************/
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbTabulationTabGroupByColumnsName);
            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
            }
            /****************************************************/
        }
       
        private List<string> GetTabulationTabStackPanelGroupByColumns(List<SQLBuilder.Clauses.Column> listOfColumns)
        {
            List<string> list = new List<string>();
            foreach (SQLBuilder.Clauses.Column column in listOfColumns)
            {
                list.Add(column.Name);
            }
            return list;
        }
        
        public List<Column> GetTabCols()
        {
            List<Column> list = new List<Column>();
            Column c1 = new Column();
            c1.Name = "xyx";
            Column c2 = new Column();
            c2.Name = "abc";
            Column c3 = new Column();
            c3.Name = "klm";

            list.Add(c1);
            list.Add(c2);
            list.Add(c3);
            return list;

        }
    }
}
