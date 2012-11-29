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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Windows.Controls;
namespace FastDB.Control
{
    public partial class FromTabControl : UserControl
    {
        private List<FromTabClass> list = new List<FromTabClass>();
        
        public bool isValidated;
        private Style ComboboxOriginalStyle;
        private Brush TextBoxOriginalBorderBrush;
        
        public FromTabControl()
        {
            InitializeComponent();
            ComboboxOriginalStyle = this.cmbFromTable.Style;
            TextBoxOriginalBorderBrush = this.txtFromAlias.BorderBrush;
            
            isValidated = false;
            this.txtFromAlias.Text = "X";

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            AddFromTabStackPanel();
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbFromTable);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.CustomQueryAccordion.LayoutChildren(true);
            }
        }

        private void AddFromTabStackPanel()
        {
            if (this.cmbFromTable.SelectedIndex != -1)
            {
                MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTable);
                int numberOfStackPanel = mainWindow.FromTabUC.StackPanelFromTab.Children.Count;

                //validate before adding stack panel row
                Validate();
                FromTabStackPanelControl fs = new FromTabStackPanelControl();

                fs.Name = "Fs2";
                fs.Margin = new Thickness(0, 5, 0, 5);
                fs.btnDelete.Visibility = System.Windows.Visibility.Visible;
                fs.btnDelete.Uid = (numberOfStackPanel + 1).ToString();
                fs.btnDelete.Width = 25.0;

                LoadFromTableColumnComboBox(fs);
                this.StackPanelFromTab.Children.Add(fs);
                DockPanelFromTabRowHeader.Visibility = System.Windows.Visibility.Visible;
                borderJoinDock.Visibility = System.Windows.Visibility.Visible;
            }
            
        }
        
        private void LoadFromTableColumnComboBox(FromTabStackPanelControl fs)
        {
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTable);
            if (this.cmbFromTable.SelectedIndex != -1)
            {
                if (mainWindow.listOfTable != null & mainWindow.listOfTable.Count > 0)
                {
                    fs.cmbFromTabJoinTable.ItemsSource = Common.ConvertTablesToStringList(mainWindow.listOfTable);
                    fs.cmbFromTabFromColumns.Text = "";
                    fs.cmbFromTabFromColumns.ItemsSource = Common.ConvertColumsToStringList(mainWindow.listOfTable[this.cmbFromTable.SelectedIndex].columns);
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

        private void cmbFromTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbFromTable.SelectedIndex > -1)
            {
                FromTabStackPanelControl fromTabSPCntrl = new FromTabStackPanelControl();
                DependencyObject parent = fromTabSPCntrl.GetRVC(this.cmbFromTable);

                if (parent != null)
                {
                    ResultViewControl rvc = (ResultViewControl)parent;
                    fromTabSPCntrl.UpdateAllControls(rvc);
                    rvc.SelectTabCntrl._SelectedColCollection.Clear();
                }
                this.cmbFromTable.Style = ComboboxOriginalStyle;
                for (int i = 0; i < this.StackPanelFromTab.Children.Count; i++)
                {
                    FromTabStackPanelControl fs1 = (FromTabStackPanelControl)this.StackPanelFromTab.Children[i];
                    LoadFromTableColumnComboBox(fs1);
                }
            }
        }

        private void txtFromAlias_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txtFromAlias.BorderBrush = TextBoxOriginalBorderBrush;
        }

        private void btnFromTabSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                this.isValidated = true;
                this.lblErrorMessage.Visibility = System.Windows.Visibility.Hidden;
                MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbFromTable);
                foreach (TabItem tbi in mainWindow.tabControlCustomQuery.Items)
                {
                    if (tbi.Name == "WhereTabItem")
                    {
                        tbi.Focus();
                        break;
                    }
                }
            }
            else
            {
                this.isValidated = false;
                this.lblErrorMessage.Visibility = System.Windows.Visibility.Visible;
            }
        }
        
        public Boolean Validate()
        {
            bool validated = true;
            if (this.cmbFromTable.SelectedIndex == -1)
            {
                this.cmbFromTable.Style = null;
                this.cmbFromTable.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (this.txtFromAlias.Text == string.Empty)
            {
                this.txtFromAlias.BorderBrush = Brushes.Red;
                validated = false;
            }
            for (int i = 0; i < this.StackPanelFromTab.Children.Count; i++)
            {
                FromTabStackPanelControl fs = (FromTabStackPanelControl)this.StackPanelFromTab.Children[i];
                if (fs.cmbFromTabJoinType.SelectedIndex == -1)
                {
                    fs.cmbFromTabJoinType.Style = null;
                    fs.cmbFromTabJoinType.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fs.cmbFromTabJoinTable.SelectedIndex == -1)
                {
                    fs.cmbFromTabJoinTable.Style = null;
                    fs.cmbFromTabJoinTable.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fs.cmbFromTabFromColumns.SelectedIndex == -1 & fs.cmbFromTabFromColumns.Text == "")
                {
                    fs.cmbFromTabFromColumns.Style = null;
                    fs.cmbFromTabFromColumns.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fs.cmbFromTabQueryOpretor.SelectedIndex == -1)
                {
                    fs.cmbFromTabQueryOpretor.Style = null;
                    fs.cmbFromTabQueryOpretor.BorderBrush = Brushes.Red;
                    validated = false;
                }
                if (fs.cmbFromTabJoinColumns.SelectedIndex == -1 & fs.cmbFromTabJoinColumns.Text == "")
                {
                    fs.cmbFromTabJoinColumns.Style = null;
                    fs.cmbFromTabJoinColumns.BorderBrush = Brushes.Red;
                    validated = false;
                }

                //checking for duplicate alias
                for (int d = 0; d < this.StackPanelFromTab.Children.Count; d++)
                {
                    FromTabStackPanelControl fs2 = (FromTabStackPanelControl)this.StackPanelFromTab.Children[d];
                    if (this.txtFromAlias.Text == fs2.txtJoinTableAlias.Text)
                    {
                        fs.txtJoinTableAlias.BorderBrush = Brushes.Red;
                        this.txtFromAlias.BorderBrush = Brushes.Red;
                        validated = false;
                        break;
                    }
                    else
                    {
                        this.txtFromAlias.BorderBrush = TextBoxOriginalBorderBrush;
                        fs.txtJoinTableAlias.BorderBrush = TextBoxOriginalBorderBrush;
                    }
                    if (fs.txtJoinTableAlias != fs2.txtJoinTableAlias)
                    {
                        if (fs.txtJoinTableAlias.Text == fs2.txtJoinTableAlias.Text)
                        {
                            fs.txtJoinTableAlias.BorderBrush = Brushes.Red;
                            validated = false;
                            break;
                        }

                    }
                    else
                    {
                        fs.txtJoinTableAlias.BorderBrush = TextBoxOriginalBorderBrush;
                    }

                }
              
                if (fs.txtJoinTableAlias.Text == string.Empty)
                {
                    fs.txtJoinTableAlias.BorderBrush = Brushes.Red;
                    validated = false;
                }
            }
            if (validated)
            {
                isValidated = true;
                this.lblErrorMessage.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                isValidated = false;
                this.lblErrorMessage.Visibility = System.Windows.Visibility.Visible;
            }
            return validated;
        }

        private void DockPanel_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txtFromAlias_LostFocus(object sender, RoutedEventArgs e)
        {
            FromTabStackPanelControl fromTabSPCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabSPCntrl.GetRVC(this.cmbFromTable);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                fromTabSPCntrl.UpdateAllControls(rvc);
                rvc.SelectTabCntrl.ColsToSelectAcc.LayoutChildren(true);
            }
        }
    }
}
