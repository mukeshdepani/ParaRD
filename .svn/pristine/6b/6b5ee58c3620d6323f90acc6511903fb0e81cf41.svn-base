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
using MySql.Data.MySqlClient;
using System.Data;


namespace FastDB.Control.CrossTabulationViewControls
{
    /// <summary>
    /// Interaction logic for CrossTabulationViewControl.xaml
    /// </summary>
    public partial class CrossTabulationViewControl : UserControl
    {


        private string _tableName;
        public ResultViewModel result;

        public CrossTabulationViewControl(tableViewModel tvm, bool getResultByTreeView)
        {
            InitializeComponent();
            _tableName = tvm._table.name;
            result = new ResultViewModel(tvm, getResultByTreeView);
            if (result != null)
            {

                this.DataContext = result;
            }

        }
        public CrossTabulationViewControl(SQLBuilder.SelectQueryBuilder QuerryBuilder, string CurrentDatabaseName)
        {
            InitializeComponent();
            result = new ResultViewModel(QuerryBuilder, CurrentDatabaseName);
            if (result != null)
            {

                this.DataContext = result;
                if (result.QueryBulder != null)
                {
                    if (result.QueryBulder.SelectedTables.Count != 0)
                    {
                        _tableName = result.QueryBulder.SelectedTables[0].Name;
                    }
                }
            }
            PopulateCrossTabulationViewControl(QuerryBuilder);
        }
        private void PopulateCrossTabulationViewControl(SQLBuilder.SelectQueryBuilder QuerryBuilder)
        {
            //display  CrossTabulation header
            this.lblSummaryMainColumnName.Content = "Cross Tabulation by " + QuerryBuilder.CrossTabulationResults.CrossTabColumn.AliasName;

            // adding gropby column control
            int lineNumber = 0;
            List<List<string>> groupByColumnValues = QuerryBuilder.CrossTabulationResults.GroupByColumnValueList;
            int goupbyColumValueIndex = 0;
            for (int groupByColIndex = 0; groupByColIndex < QuerryBuilder.CrossTabulationResults.GroupByColumns.Count; groupByColIndex++)
            {
                SQLBuilder.Clauses.Column groupByCol = QuerryBuilder.CrossTabulationResults.GroupByColumns.ElementAt<SQLBuilder.Clauses.Column>(groupByColIndex);
                // find colum format
                string colFormat = SQLBuilder.Common.ColumnFormat.Instance.getColumnFormat(groupByCol.Format);
                CrossTabulationViewGroupByControl ctvgCntrl = new CrossTabulationViewGroupByControl();
                ctvgCntrl.lblGroupByColumnHeader.Content = Common.getColumnNameOrAlias(groupByCol);
                for (int i = 0; i < groupByColumnValues.Count; i++)
                {
                    Label colRow = new Label();

                    if ((lineNumber % 2) == 0)
                    {
                        colRow.Style = (Style)FindResource("CrossTabulationEvenGroupByColumRowStyle");
                    }
                    else
                    {
                        colRow.Style = (Style)FindResource("CrossTabulationOddGroupByColumRowStyle");
                    }

                    if (colFormat != null)
                    {
                        //formating group by col value
                        colRow.Content = String.Format(colFormat, groupByColumnValues.ElementAt<List<string>>(i).ElementAt<string>(groupByColIndex));
                    }
                    else
                    {
                        colRow.Content = groupByColumnValues.ElementAt<List<string>>(i).ElementAt<string>(groupByColIndex);
                    }
                    ctvgCntrl.StackPaenlGroupbyColumnRows.Children.Add(colRow);

                    lineNumber = lineNumber + 1;
                }
                lineNumber = 0;
                this.StackPanelCrossTabulationViewGroupByControls.Children.Add(ctvgCntrl);
                goupbyColumValueIndex = goupbyColumValueIndex + 1;
            }
            lineNumber = 0;
            int groupByColCount = QuerryBuilder.CrossTabulationResults.GroupByColumns.Count;
            int summarrizeValueIndex = groupByColCount;
            // adding summary main control and summary column control
            // add Grand Total to CrossTabColumnVaues as las item
            Dictionary<string, Object> dataMap = QuerryBuilder.CrossTabulationResults.DataMap;
            QuerryBuilder.CrossTabulationResults.CrossTabColumnVaues.Add("Grand Total");

            foreach (string summaryMainValue in QuerryBuilder.CrossTabulationResults.CrossTabColumnVaues)
            {
                CrossTabulationViewSummaryMainControl summaryMain = new CrossTabulationViewSummaryMainControl();
                summaryMain.lblSummaryHeader.Content = summaryMainValue;

                for (int summaryColIndex = 0; summaryColIndex < QuerryBuilder.CrossTabulationResults.SummarizeColumns.Count; summaryColIndex++)
                {

                    SQLBuilder.Clauses.Column summaryCol = QuerryBuilder.CrossTabulationResults.SummarizeColumns.ElementAt<SQLBuilder.Clauses.Column>(summaryColIndex);
                    // find column format
                    string summarycolFormat = SQLBuilder.Common.ColumnFormat.Instance.getColumnFormat(summaryCol.Format);
                    CrossTabulationViewSummaryControl ctvsCtrl = new CrossTabulationViewSummaryControl();
                    ctvsCtrl.lblSummaryColumnHeader.Content = " " + Common.getColumnNameOrAlias(summaryCol) + " ";
                    //chnageing background  color for last grad total section
                    if (summaryMainValue == "Grand Total")
                    {
                        summaryMain.lblSummaryHeader.Style = (Style)FindResource("CrossTabulationGrandTotalHeaderColumnStyle");
                        ctvsCtrl.lblSummaryColumnHeader.Style = (Style)FindResource("CrossTabulationGrandTotalHeaderColumnStyle");
                    }

                    for (int keyIndex = 0; keyIndex < QuerryBuilder.CrossTabulationResults.KeyPrefixes.Count; keyIndex++)
                    {
                        string key = QuerryBuilder.CrossTabulationResults.KeyPrefixes.ElementAt<string>(keyIndex);
                        Label colRow = new Label();

                        if ((lineNumber % 2) == 0)
                        {
                            if (summaryMainValue == "Grand Total")
                            {
                                colRow.Style = (Style)FindResource("CrossTabulationGrandTotalSummaryColumRowStyle");
                            }
                            else
                            {
                                colRow.Style = (Style)FindResource("CrossTabulationEvenSummaryColumRowStyle");
                            }
                        }
                        else
                        {
                            colRow.Style = (Style)FindResource("CrossTabulationOddSummaryColumRowStyle");
                        }
                        string keyValue = key + summaryMainValue + summaryColIndex;
                        if (dataMap.ContainsKey(keyValue))
                        {
                            if (summarycolFormat != null)
                            {
                                colRow.Content = String.Format(summarycolFormat, dataMap[keyValue]);
                            }
                            else
                            {
                                colRow.Content = dataMap[keyValue].ToString();
                            }

                        }

                        else
                        {

                            colRow.Content =
                            "";
                        }
                        DockPanel dp = new DockPanel();
                        dp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        dp.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        dp.Children.Add(colRow);
                        ctvsCtrl.StackPaenlSummaryColumnRows.Children.Add(dp);

                        lineNumber = lineNumber + 1;
                    }

                    lineNumber = 0;
                    summaryMain.StackPaenlSummaryMainColumns.Children.Add(ctvsCtrl);
                    summarrizeValueIndex = summarrizeValueIndex + 1;
                }

                this.StackPanelCrossTabulationViewSummaryControls.Children.Add(summaryMain);
            }

        }

    }
}
