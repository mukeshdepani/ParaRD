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
    /// Interaction logic for WhereTabControl.xaml
    /// </summary>

    public partial class WhereTabControl : UserControl
    {
        public bool isValidated;
        private bool isAllDisabled;
        public WhereTabControl()
        {
            InitializeComponent();
            isValidated = false;
            isAllDisabled = true;
            this.cmbWherTabCondition.ItemsSource = LoadWhereConditionCombo();
            this.cmbWherTabCondition.SelectedIndex = 0;
            this.cmbWherTabCondition.IsEnabled = true;
        }

        public bool IsAllDisabled
        {
            get { return isAllDisabled; }
            set { isAllDisabled = value; }
        }

        private List<string> GetColumnsFromAllFromTabTable()
        {
            List<string> list = new List<string>();
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbWherTabCondition);
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWherTabCondition);  //get resultview control containing cmbWherTabCondition

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                //get the From table columns 
                if (mainWindow.listOfTable != null & mainWindow.listOfTable.Count > 0)
                {
                    List<string> fc = GetFromTableColums(mainWindow.listOfTable[rvc.FromTabCntrl.cmbFromTable.SelectedIndex].columns, rvc.FromTabCntrl.txtFromAlias.Text);

                    if (fc != null)
                    {
                        list.AddRange(fc);
                    }
                }

                if (rvc.FromTabCntrl.StackPanelFromTab.Children.Count > 0)
                {
                    for (int i = 0; i < rvc.FromTabCntrl.StackPanelFromTab.Children.Count; i++)
                    {
                        FromTabStackPanelControl fs = (FromTabStackPanelControl)rvc.FromTabCntrl.StackPanelFromTab.Children[i];

                        List<string> jc = GetJoinTableColums((List<string>)fs.cmbFromTabJoinColumns.ItemsSource, fs.txtJoinTableAlias.Text);
                        if (jc != null)
                        {
                            list.AddRange(jc);
                        }
                    }
                }
            }
            return list;
        }

        private List<string> GetJoinTableColums(List<string> listOfColumns, string alias)
        {
            List<string> list = new List<string>();
            if (alias != string.Empty)
            {
                foreach (string column in listOfColumns)
                {
                    list.Add(alias + "." + column);
                }
            }
            return list;
        }

        private List<string> GetFromTableColums(List<MySQLData.Column> listOfColumns, string alias)
        {
            List<string> list = new List<string>();
            if (alias != string.Empty)
            {
                foreach (MySQLData.Column column in listOfColumns)
                {
                    list.Add(alias + "." + column.name);
                }
            }
            return list;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            whereborder.Visibility = System.Windows.Visibility.Visible;
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbWherTabCondition);

            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWherTabCondition);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (rvc.FromTabCntrl.isValidated)
                {
                    this.lblErrorMessage.Content = "";

                    //validate before adding stack panel row
                    Validate();
                    //** Check for AllDisabled
                    SetIsAllDisabled();
                    //***Add condition control
                    if (this.cmbWherTabCondition.SelectedIndex != -1)
                    {
                        switch (this.cmbWherTabCondition.Text)
                        {
                            case "Regular":
                                AddRegularConditionRow();
                                break;
                            case "Between":
                                AddBetweenConditionRow();
                                break;
                            case "In":
                                AddInNotInConditionRow();
                                break;
                            case "Not In":
                                AddInNotInConditionRow();
                                break;
                            case "Null":
                                AddNullNotNullConditionRow();
                                break;
                            case "Not Null":
                                AddNullNotNullConditionRow();
                                break;
                        }
                    }
                    Size size = new Size(1200, 500);
                }
                else
                {
                    this.lblErrorMessage.Content = "********There is an error on From Tab**********";
                    rvc.lblXmlQueryTabErrorMessage.Content = "There is an error in one of Tabs";
                }
            }
        }

        private void AddNullNotNullConditionRow()
        {
            WhereTabNullNotNullConditionControl wNullNotNull = new WhereTabNullNotNullConditionControl();
            wNullNotNull.Name = "wNullNotNull1";
            wNullNotNull.cmbWhereTabQueryLevel.SelectedIndex = 0;
            wNullNotNull.cmbWhereTabQueryAndOr.SelectedIndex = 0;
            if (this.cmbWherTabCondition.Text == "Null")
            {
                wNullNotNull.lblNullNotNull.Content = "null";
            }
            if (this.cmbWherTabCondition.Text == "Not Null")
            {
                wNullNotNull.lblNullNotNull.Content = "not null";
            }
            if (isAllDisabled)
            {
                this.lblErrorMessage.Content = "";
                wNullNotNull.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                wNullNotNull.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
            }
            wNullNotNull.btnDelete.Visibility = System.Windows.Visibility.Visible;
            LoadColumnComboBox(wNullNotNull);
            this.StackPanelWhereTab.Children.Add(wNullNotNull);

        }
        
        private void AddInNotInConditionRow()
        {
            WhereTabInNotInConditionControl wInNotIn = new WhereTabInNotInConditionControl();
            wInNotIn.Name = "wInNotIn1";
            wInNotIn.cmbWhereTabQueryLevel.SelectedIndex = 0;
            wInNotIn.cmbWhereTabQueryAndOr.SelectedIndex = 0;
            if (this.cmbWherTabCondition.Text == "In")
            {
                wInNotIn.lblInNotIn.Content = "    in";
            }
            if (this.cmbWherTabCondition.Text == "Not In")
            {
                wInNotIn.lblInNotIn.Content = "not in";
            }
            if (isAllDisabled)
            {
                this.lblErrorMessage.Content = "";
                wInNotIn.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                wInNotIn.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
            }
            wInNotIn.btnDelete.Visibility = System.Windows.Visibility.Visible;
            LoadColumnComboBox(wInNotIn);
            this.StackPanelWhereTab.Children.Add(wInNotIn);
        }
        
        private void AddBetweenConditionRow()
        {
            WhereTabBetweenConditionControl wb = new WhereTabBetweenConditionControl();
            wb.Name = "wb1";
            wb.cmbWhereTabQueryLevel.SelectedIndex = 0;
            wb.cmbWhereTabQueryAndOr.SelectedIndex = 0;
            if (isAllDisabled)
            {
                this.lblErrorMessage.Content = "";
                wb.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                wb.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
            }
            wb.btnDelete.Visibility = System.Windows.Visibility.Visible;
            LoadColumnComboBox(wb);
            this.StackPanelWhereTab.Children.Add(wb);
        }
        
        private void AddRegularConditionRow()
        {
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWherTabCondition);
            ResultViewControl rvc = null;

            if (parent != null)
            {
                rvc = (ResultViewControl)parent;
            }

            WhereTabRegularConditionControl ws = new WhereTabRegularConditionControl();
            ws.Name = "Fs1";
            ws.cmbWhereTabQueryLevel.SelectedIndex = 0;
            ws.cmbWhereTabQueryAndOr.SelectedIndex = 0;
            ////** Check for AllDisabled
            if (isAllDisabled)
            {
                this.lblErrorMessage.Content = "";
                ws.lblLogicalOpreator.Visibility = System.Windows.Visibility.Hidden;
                ws.cmbWhereTabQueryAndOr.Visibility = System.Windows.Visibility.Hidden;
            }
            ws.btnDelete.Visibility = System.Windows.Visibility.Visible;
            LoadColumnComboBox(ws);
            this.StackPanelWhereTab.Children.Add(ws);
        }
        
        private void SetIsAllDisabled()
        {
            if (this.StackPanelWhereTab.Children.Count > 0)
            {
                for (int i = 0; i < this.StackPanelWhereTab.Children.Count; i++)
                {
                    string controlType = this.StackPanelWhereTab.Children[i].GetType().ToString();
                    switch (controlType)
                    {
                        case "FastDB.Control.WhereTabRegularConditionControl":
                            WhereTabRegularConditionControl wsToCheck = (WhereTabRegularConditionControl)this.StackPanelWhereTab.Children[i];
                            if (wsToCheck.cmbWhereTabLeftSideColumns.IsEnabled == false)
                            {
                                isAllDisabled = true;
                            }
                            else
                            {
                                isAllDisabled = false;
                            }
                            break;
                        case "FastDB.Control.WhereTabBetweenConditionControl":
                            WhereTabBetweenConditionControl wsbToCheck = (WhereTabBetweenConditionControl)this.StackPanelWhereTab.Children[i];
                            if (wsbToCheck.cmbWhereTabBetweenColumns.IsEnabled == false)
                            {
                                isAllDisabled = true;
                            }
                            else
                            {
                                isAllDisabled = false;
                            }
                            break;

                        case "FastDB.Control.WhereTabInNotInConditionControl":
                            WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)this.StackPanelWhereTab.Children[i];
                            if (wInNotIn.cmbWhereTabInNotInColumns.IsEnabled == false)
                            {
                                isAllDisabled = true;
                            }
                            else
                            {
                                isAllDisabled = false;
                            }
                            break;
                        case "FastDB.Control.WhereTabNullNotNullConditionControl":
                            WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)this.StackPanelWhereTab.Children[i];
                            if (wNullNotNull.cmbWhereTabNullNotNullColumns.IsEnabled == false)
                            {
                                isAllDisabled = true;
                            }
                            else
                            {
                                isAllDisabled = false;
                            }
                            break;

                    }
                }
            }
            else
            {
                //the row count should be Zero, even though there is no stack panel row we set it that all row are disabled
                isAllDisabled = true;
            }
        }

        private void LoadColumnComboBox(UserControl uc)
        {

            string controlType = this.cmbWherTabCondition.Text;
            switch (controlType)
            {
                case "Regular":
                    WhereTabRegularConditionControl wsToCheck = (WhereTabRegularConditionControl)uc;
                    wsToCheck.cmbWhereTabLeftSideColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    wsToCheck.cmbWhereTabRightSideColumns.ItemsSource = wsToCheck.cmbWhereTabLeftSideColumns.ItemsSource;
                    break;
                case "Between":
                    WhereTabBetweenConditionControl wsbToCheck = (WhereTabBetweenConditionControl)uc;
                    wsbToCheck.cmbWhereTabBetweenColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
                case "In":
                    WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)uc;
                    wInNotIn.cmbWhereTabInNotInColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
                case "Not In":
                    WhereTabInNotInConditionControl wInNotIn2 = (WhereTabInNotInConditionControl)uc;
                    wInNotIn2.cmbWhereTabInNotInColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
                case "Null":
                    WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)uc;
                    wNullNotNull.cmbWhereTabNullNotNullColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
                case "Not Null":
                    WhereTabNullNotNullConditionControl wNullNotNull2 = (WhereTabNullNotNullConditionControl)uc;
                    wNullNotNull2.cmbWhereTabNullNotNullColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
            }

        }
        
        private List<string> LoadWhereConditionCombo()
        {
            List<string> list = new List<string>();
            list.Add("Regular");
            list.Add("Between");
            list.Add("In");
            list.Add("Not In");
            list.Add("Null");
            list.Add("Not Null");
            return list;
        }
        
        private void ReLoadColumnComboBox(UserControl uc)
        {
            string controlType = uc.GetType().ToString();

            switch (controlType)
            {
                case "FastDB.Control.WhereTabRegularConditionControl":
                    WhereTabRegularConditionControl wsToCheck = (WhereTabRegularConditionControl)uc;
                    wsToCheck.cmbWhereTabLeftSideColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    wsToCheck.cmbWhereTabRightSideColumns.ItemsSource = wsToCheck.cmbWhereTabLeftSideColumns.ItemsSource;
                    break;
                case "FastDB.Control.WhereTabBetweenConditionControl":
                    WhereTabBetweenConditionControl wsbToCheck = (WhereTabBetweenConditionControl)uc;
                    wsbToCheck.cmbWhereTabBetweenColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
                case "FastDB.Control.WhereTabInNotInConditionControl":
                    WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)uc;
                    wInNotIn.cmbWhereTabInNotInColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
                case "FastDB.Control.WhereTabNullNotNullConditionControl":
                    WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)uc;
                    wNullNotNull.cmbWhereTabNullNotNullColumns.ItemsSource = GetColumnsFromAllFromTabTable();
                    break;
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

        private void cmbWherTabCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        public Boolean Validate()
        {
            bool validated = true;
            if (this.StackPanelWhereTab.Children.Count > 0)
            {
                for (int i = 0; i < this.StackPanelWhereTab.Children.Count; i++)
                {
                    string controlType = this.StackPanelWhereTab.Children[i].GetType().ToString();
                    UserControl uc = (UserControl)this.StackPanelWhereTab.Children[i];
                    switch (controlType)
                    {
                        case "FastDB.Control.WhereTabRegularConditionControl":
                            validated = ValidateRegularCondition(uc);
                            break;
                        case "FastDB.Control.WhereTabBetweenConditionControl":
                            validated = ValidateBetweenCondition(uc);
                            break;
                        case "FastDB.Control.WhereTabInNotInConditionControl":
                            validated = ValidateInNotInCondition(uc);
                            break;
                        case "FastDB.Control.WhereTabNullNotNullConditionControl":
                            validated = ValidateNullNotNullCondition(uc);
                            break;
                    }
                }
            }

            if (validated)
            {
                isValidated = true;
                this.lblErrorMessage.Content = "";
            }
            else
            {
                isValidated = false;
                this.lblErrorMessage.Content = "****Please provide the value for control in red******";
            }

            return validated;

        }
        
        public Boolean ValidateNullNotNullCondition(UserControl uc)
        {
            bool validated = true;
            WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)uc;
            if (wNullNotNull.cmbWhereTabNullNotNullColumns.SelectedIndex == -1 & wNullNotNull.cmbWhereTabNullNotNullColumns.Text == "")
            {
                wNullNotNull.cmbWhereTabNullNotNullColumns.Style = null;
                wNullNotNull.cmbWhereTabNullNotNullColumns.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (wNullNotNull.cmbWhereTabQueryLevel.SelectedIndex == -1)
            {
                wNullNotNull.cmbWhereTabQueryLevel.Style = null;
                wNullNotNull.cmbWhereTabQueryLevel.BorderBrush = Brushes.Red;
                validated = false;
            }
            return validated;
        }
        
        public Boolean ValidateInNotInCondition(UserControl uc)
        {
            bool validated = true;
            WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)uc;
            if (wInNotIn.cmbWhereTabInNotInColumns.SelectedIndex == -1 & wInNotIn.cmbWhereTabInNotInColumns.Text == "")
            {
                wInNotIn.cmbWhereTabInNotInColumns.Style = null;
                wInNotIn.cmbWhereTabInNotInColumns.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (wInNotIn.txtInNotInValue.Text == System.String.Empty)
            {
                wInNotIn.txtInNotInValue.BorderBrush = Brushes.Red;
            }
            if (wInNotIn.cmbWhereTabQueryLevel.SelectedIndex == -1)
            {
                wInNotIn.cmbWhereTabQueryLevel.Style = null;
                wInNotIn.cmbWhereTabQueryLevel.BorderBrush = Brushes.Red;
                validated = false;
            }
            return validated;
        }
        
        public Boolean ValidateBetweenCondition(UserControl uc)
        {
            bool validated = true;
            WhereTabBetweenConditionControl ws = (WhereTabBetweenConditionControl)uc;
            if (ws.cmbWhereTabBetweenColumns.SelectedIndex == -1 & ws.cmbWhereTabBetweenColumns.Text == "")
            {
                ws.cmbWhereTabBetweenColumns.Style = null;
                ws.cmbWhereTabBetweenColumns.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (ws.txtBetweenLeftValue.Text == System.String.Empty)
            {
                ws.txtBetweenLeftValue.BorderBrush = Brushes.Red;
            }
            if (ws.txtBetweenRightValue.Text == System.String.Empty)
            {
                ws.txtBetweenRightValue.BorderBrush = Brushes.Red;
            }
            if (ws.cmbWhereTabQueryLevel.SelectedIndex == -1)
            {
                ws.cmbWhereTabQueryLevel.Style = null;
                ws.cmbWhereTabQueryLevel.BorderBrush = Brushes.Red;
                validated = false;
            }
            return validated;
        }
        
        public Boolean ValidateRegularCondition(UserControl uc)
        {
            bool validated = true;
            WhereTabRegularConditionControl ws = (WhereTabRegularConditionControl)uc;
            if (ws.cmbWhereTabLeftSideColumns.SelectedIndex == -1 & ws.cmbWhereTabLeftSideColumns.Text == "")
            {
                ws.cmbWhereTabLeftSideColumns.Style = null;
                ws.cmbWhereTabLeftSideColumns.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (ws.cmbWhereTabQueryOpretor.SelectedIndex == -1)
            {
                ws.cmbWhereTabQueryOpretor.Style = null;
                ws.cmbWhereTabQueryOpretor.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (ws.cmbWhereTabRightSideColumns.SelectedIndex == -1 & ws.cmbWhereTabRightSideColumns.Text == "")
            {
                ws.cmbWhereTabRightSideColumns.Style = null;
                ws.cmbWhereTabRightSideColumns.BorderBrush = Brushes.Red;
                validated = false;
            }
            if (ws.cmbWhereTabQueryLevel.SelectedIndex == -1)
            {
                ws.cmbWhereTabQueryLevel.Style = null;
                ws.cmbWhereTabQueryLevel.BorderBrush = Brushes.Red;
                validated = false;
            }
            return validated;
        }

        private void DockPanel_GotFocus(object sender, RoutedEventArgs e)
        {
            bool isEqual = true;
            MainWindow mainWindow = (MainWindow)GetTopLevelControl(this.cmbWherTabCondition);

            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWherTabCondition);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;

                if (rvc.FromTabCntrl.isValidated)
                {
                    for (int i = 0; i < this.StackPanelWhereTab.Children.Count; i++)
                    {
                        string controlType = this.StackPanelWhereTab.Children[i].GetType().ToString();
                        switch (controlType)
                        {
                            case "FastDB.Control.WhereTabRegularConditionControl":
                                WhereTabRegularConditionControl ws = (WhereTabRegularConditionControl)this.StackPanelWhereTab.Children[i];
                                List<string> listofRegCol = GetColumnsFromAllFromTabTable();
                                isEqual = (listofRegCol.SequenceEqual((List<string>)ws.cmbWhereTabLeftSideColumns.ItemsSource));
                                if (isEqual == false)
                                {
                                    ReLoadColumnComboBox(ws);
                                }
                                break;
                            case "FastDB.Control.WhereTabBetweenConditionControl":
                                WhereTabBetweenConditionControl wsb = (WhereTabBetweenConditionControl)this.StackPanelWhereTab.Children[i];
                                isEqual = (GetColumnsFromAllFromTabTable().SequenceEqual((List<string>)wsb.cmbWhereTabBetweenColumns.ItemsSource));
                                if (isEqual == false)
                                {
                                    ReLoadColumnComboBox(wsb);
                                }
                                break;
                            case "FastDB.Control.WhereTabInNotInConditionControl":
                                WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)this.StackPanelWhereTab.Children[i];
                                isEqual = (GetColumnsFromAllFromTabTable().SequenceEqual((List<string>)wInNotIn.cmbWhereTabInNotInColumns.ItemsSource));
                                if (isEqual == false)
                                {
                                    ReLoadColumnComboBox(wInNotIn);
                                }
                                break;
                            case "FastDB.Control.WhereTabNullNotNullConditionControl":
                                WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)this.StackPanelWhereTab.Children[i];
                                isEqual = (GetColumnsFromAllFromTabTable().SequenceEqual((List<string>)wNullNotNull.cmbWhereTabNullNotNullColumns.ItemsSource));
                                if (isEqual == false)
                                {
                                    ReLoadColumnComboBox(wNullNotNull);
                                }
                                break;
                        }


                    }
                }
                else
                {
                    this.lblErrorMessage.Content = "********There is an error on From Tab**********";
                    SetWhereTabRowControlDisabled();
                }
            }
        }

        private void SetWhereTabRowControlDisabled()
        {
            for (int i = 0; i < this.StackPanelWhereTab.Children.Count; i++)
            {
                string controlType = this.StackPanelWhereTab.Children[i].GetType().ToString();
                switch (controlType)
                {
                    case "FastDB.Control.WhereTabRegularConditionControl":
                        WhereTabRegularConditionControl ws = (WhereTabRegularConditionControl)this.StackPanelWhereTab.Children[i];
                        ws.cmbWhereTabQueryAndOr.IsEnabled = false;
                        ws.cmbWhereTabLeftSideColumns.IsEnabled = false;
                        ws.cmbWhereTabQueryOpretor.IsEnabled = false;
                        ws.cmbWhereTabRightSideColumns.IsEnabled = false;
                        ws.cmbWhereTabQueryLevel.IsEnabled = false;
                        ws.cmbWhereTabQueryAndOr.IsEnabled = false;
                        break;
                    case "FastDB.Control.WhereTabBetweenConditionControl":
                        WhereTabBetweenConditionControl wsb = (WhereTabBetweenConditionControl)this.StackPanelWhereTab.Children[i];
                        wsb.cmbWhereTabQueryAndOr.IsEnabled = false;
                        wsb.cmbWhereTabBetweenColumns.IsEnabled = false;
                        wsb.txtBetweenLeftValue.IsEnabled = false;
                        wsb.txtBetweenRightValue.IsEnabled = false;
                        wsb.cmbWhereTabQueryLevel.IsEnabled = false;
                        break;
                    case "FastDB.Control.WhereTabInNotInConditionControl":
                        WhereTabInNotInConditionControl wInNotIn = (WhereTabInNotInConditionControl)this.StackPanelWhereTab.Children[i];
                        wInNotIn.cmbWhereTabQueryAndOr.IsEnabled = false;
                        wInNotIn.cmbWhereTabInNotInColumns.IsEnabled = false;
                        wInNotIn.txtInNotInValue.IsEnabled = false;
                        wInNotIn.cmbWhereTabQueryLevel.IsEnabled = false;
                        break;
                    case "FastDB.Control.WhereTabNullNotNullConditionControl":
                        WhereTabNullNotNullConditionControl wNullNotNull = (WhereTabNullNotNullConditionControl)this.StackPanelWhereTab.Children[i];
                        wNullNotNull.cmbWhereTabQueryAndOr.IsEnabled = false;
                        wNullNotNull.cmbWhereTabNullNotNullColumns.IsEnabled = false;
                        wNullNotNull.cmbWhereTabQueryLevel.IsEnabled = false;
                        break;
                }
            }
        }

        private void StackPanelWhereTab_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FromTabStackPanelControl fromTabStackPanelCntrl = new FromTabStackPanelControl();
            DependencyObject parent = fromTabStackPanelCntrl.GetRVC(this.cmbWherTabCondition);

            if (parent != null)
            {
                ResultViewControl rvc = (ResultViewControl)parent;
                rvc.CustomQueryAccordion.LayoutChildren(true);
            }
        }
    }
}
