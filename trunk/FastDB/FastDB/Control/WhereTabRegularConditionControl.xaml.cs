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
using System.Xml.Serialization;
using System.IO;

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for WhereTabRegularConditionControl.xaml
    /// </summary>
    public partial class WhereTabRegularConditionControl : UserControl
    {
        private Style ComboboxOriginalStyle;
        private string queryString;

        public WhereTabRegularConditionControl()
        {
            InitializeComponent();
            ComboboxOriginalStyle = this.cmbWhereTabQueryAndOr.Style;
            List<string> ListOfLogicalOpreator = new List<string> { "And", "Or" };
            this.cmbWhereTabQueryAndOr.ItemsSource = ListOfLogicalOpreator;
            this.cmbWhereTabQueryOpretor.ItemsSource = Enum.GetNames(typeof(SQLBuilder.Enums.Comparison)).ToList();
            this.cmbWhereTabQueryOpretor.SelectedIndex = 0;
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btnDelete = (Button)sender;
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbWhereTabLeftSideColumns);

            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWhereTabLeftSideColumns);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (rvc.WhereTabCntrl.StackPanelWhereTab.Children.Count > 0)
                {
                    DockPanel dkp = (DockPanel)btnDelete.Parent;
                    WhereTabRegularConditionControl fsp = (WhereTabRegularConditionControl)((Grid)((StackPanel)dkp.Parent).Parent).Parent;
                    rvc.WhereTabCntrl.StackPanelWhereTab.Children.Remove(fsp);
                    
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
                /**********Update Xml Query*********/
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
                /************************************/
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

        private void cmbWhereTabLeftSideColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbWhereTabLeftSideColumns.Style = ComboboxOriginalStyle;
        }

        private void UpdateXmlQuery()
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbWhereTabLeftSideColumns);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWhereTabLeftSideColumns);

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
        
        private void cmbWhereTabRightSideColumns_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbWhereTabRightSideColumns.Text == "")
            {
                cmbWhereTabRightSideColumns.Style = null;
                cmbWhereTabRightSideColumns.BorderBrush = Brushes.Red;
            }
            else
            {
                this.cmbWhereTabRightSideColumns.Style = ComboboxOriginalStyle;
                WhereTabControl wtc = new WhereTabControl();
                wtc.isValidated = true;
                wtc.Validate();
            }
        }
        
        private void cmbWhereTabQueryOpretor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbWhereTabQueryOpretor.Style = ComboboxOriginalStyle;
        }

        private void cmbWhereTabRightSideColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // seting the style if style is changed due to Error it changes border of the combobox
            this.cmbWhereTabRightSideColumns.Style = ComboboxOriginalStyle;
        }

        private void cmbWhereTabQueryAndOr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void cmbWhereTabQueryLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
