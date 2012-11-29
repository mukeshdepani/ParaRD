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
    /// Interaction logic for WhereTabNullNotNullConditionControl.xaml
    /// </summary>
    public partial class WhereTabNullNotNullConditionControl : UserControl
    {
        private Style ComboboxOriginalStyle;
        public WhereTabNullNotNullConditionControl()
        {
            InitializeComponent();
            ComboboxOriginalStyle = this.cmbWhereTabNullNotNullColumns.Style;
            List<string> ListOfLogicalOpreator = new List<string> { "And", "Or" };
            this.cmbWhereTabQueryAndOr.ItemsSource = ListOfLogicalOpreator;
        }

        private void cmbWhereTabNullNotNullColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbWhereTabNullNotNullColumns.Style = ComboboxOriginalStyle;
        }

        private void cmbWhereTabQueryLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbWhereTabQueryLevel.Style = ComboboxOriginalStyle;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbWhereTabNullNotNullColumns);

            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWhereTabNullNotNullColumns);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (rvc.WhereTabCntrl.StackPanelWhereTab.Children.Count > 0)
                {
                    DockPanel dkp = (DockPanel)btnDelete.Parent;
                    WhereTabNullNotNullConditionControl wsp = (WhereTabNullNotNullConditionControl)((Grid)((StackPanel)dkp.Parent).Parent).Parent;
                    rvc.WhereTabCntrl.StackPanelWhereTab.Children.Remove(wsp);
                    
                    if (rvc.WhereTabCntrl.StackPanelWhereTab.Children.Count != 0)
                    {
                        string controlType = rvc.WhereTabCntrl.StackPanelWhereTab.Children[0].GetType().ToString();
                        switch (controlType)
                        {
                            case "FastDB.Control.WhereTabRegularConditionControl":
                                WhereTabRegularConditionControl wr = (WhereTabRegularConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[0]);
                                wr.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wr.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                                break;
                            case "FastDB.Control.WhereTabBetweenConditionControl":
                                WhereTabBetweenConditionControl wb = (WhereTabBetweenConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[0]);
                                wb.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wb.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                                break;
                            case "FastDB.Control.WhereTabInNotInConditionControl":
                                WhereTabInNotInConditionControl wInNotin = (WhereTabInNotInConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[0]);
                                wInNotin.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wInNotin.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                                break;
                            case "FastDB.Control.WhereTabNullNotNullConditionControl":
                                WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)(rvc.WhereTabCntrl.StackPanelWhereTab.Children[0]);
                                wNullNotNull.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                                wNullNotNull.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
                                break;
                        }
                    }
                }
            }
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
    }
}
