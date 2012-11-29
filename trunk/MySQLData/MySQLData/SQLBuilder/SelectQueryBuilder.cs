using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Clauses;
using SQLBuilder.Enums;
using System.Data.Common;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Serialization;
using SQLBuilder.Common;



namespace SQLBuilder
{
    [Serializable()]
    public class SelectQueryBuilder : IQueryBuilder
    {
        //protected bool _distinct = false;
        //protected TopClause _topClause = new TopClause(100, TopUnit.Percent);
        protected List<Column> _selectedColumns = new List<Column>();	// array of string
        protected List<Table> _selectedTables = new List<Table>();	// array of string
        protected List<JoinClause> _joins = new List<JoinClause>();	// array of JoinClause
        protected WhereStatement _whereStatement = new WhereStatement();
        protected List<OrderByClause> _orderByStatement = new List<OrderByClause>();	// array of OrderByClause
        protected List<Column> _groupByColumns = new List<Column>();		// array of string
        protected List<Column> _summarizeColumns = new List<Column>();		// array of string
        protected WhereStatement _havingStatement = new WhereStatement();
        protected CrossTabulationClause _crossTabClause = new CrossTabulationClause();
        [XmlIgnoreAttribute]
        protected CrossTabResults _crossTabResults;
        [XmlIgnoreAttribute]
        protected List<Column> _finalSelectedColumns = new List<Column>();


        internal WhereStatement WhereStatement
        {
            get { return _whereStatement; }
            set { _whereStatement = value; }
        }

        public SelectQueryBuilder() { }
        //public SelectQueryBuilder(DbProviderFactory factory)
        //{
        //    _dbProviderFactory = factory;
        //}

        //private DbProviderFactory _dbProviderFactory;
        //public void SetDbProviderFactory(DbProviderFactory factory)
        //{
        //    _dbProviderFactory = factory;
        //}

        //public bool Distinct
        //{
        //    get { return _distinct; }
        //    set { _distinct = value; }
        //}

        //public int TopRecords
        //{
        //    get { return _topClause.Quantity; }
        //    set
        //    {
        //        _topClause.Quantity = value;
        //        _topClause.Unit = TopUnit.Records;
        //    }
        //}
        //public TopClause TopClause
        //{
        //    get { return _topClause; }
        //    set { _topClause = value; }
        //}

        public List<Column> SelectedColumns
        {
            get { return _selectedColumns; }
            set { _selectedColumns = value; }
        }

        [XmlIgnoreAttribute]
        public List<Column> FinalSelectedColumns
        {
            get { return _finalSelectedColumns; }
            set { _finalSelectedColumns = value; }
        }

        public List<Table> SelectedTables
        {
            get { return _selectedTables; }
            set { _selectedTables = value; }
        }

        public List<JoinClause> Joins
        {
            get { return _joins; }
            set { _joins = value; }
        }

        public List<OrderByClause> OrderByStatement
        {
            get { return _orderByStatement; }
            set { _orderByStatement = value; }
        }

        public List<Column> GroupByColumns
        {
            get { return _groupByColumns; }
            set { _groupByColumns = value; }
        }

        public List<Column> SummarizeColumns
        {
            get { return _summarizeColumns; }
            set { _summarizeColumns = value; }
        }

        [XmlIgnoreAttribute]
        public CrossTabResults CrossTabulationResults
        {
            get { return _crossTabResults; }
            set { _crossTabResults = value; }
        }

        public void SelectAllColumns()
        {
            _selectedColumns.Clear();
        }
        public void SelectCount()
        {
            Column column = new Column();
            column.Name = "count(1)";
            column.AliasName = "CountColumn";
            SelectColumn(column);
        }
        public void SelectColumn(Column column)
        {
            //_selectedColumns.Clear();
            _selectedColumns.Add(column);
        }
        public void SelectColumns(params Column[] columns)
        {
            _selectedColumns.Clear();
            foreach (Column column in columns)
            {
                _selectedColumns.Add(column);
            }
        }
        
        public void SelectFromTable(Table table)
        {
            _selectedTables.Clear();
            _selectedTables.Add(table);
        }
        public void SelectFromTables(params Table[] tables)
        {
            _selectedTables.Clear();
            foreach (Table Table in tables)
            {
                _selectedTables.Add(Table);
            }
        }
        public JoinClause AddJoin(JoinClause newJoin)
        {
            _joins.Add(newJoin);
            return newJoin;
        }

        public JoinClause AddJoin(JoinType join, Table toTable, string toColumnName, Comparison @operator, Table fromTable, string fromColumnName)
        {
            JoinClause NewJoin = new JoinClause(join, toTable, toColumnName, @operator, fromTable, fromColumnName);
            _joins.Add(NewJoin);

            return NewJoin;
        }

        public WhereStatement Where
        {
            get { return _whereStatement; }
            set { _whereStatement = value; }
        }

        //public void AddWhere(WhereClause clause) { AddWhere(clause, 1); }
        //public void AddWhere(WhereClause clause, int level)
        //{
        //    _whereStatement.Add(clause);
        //}
        public WhereClause AddWhere(LogicOperator logicalOperator, string field, Comparison @operator, object compareValue) { return AddWhere(logicalOperator, field, @operator, compareValue, 1); }
        //public WhereClause AddWhere(Enum field, Comparison @operator, object compareValue) { return AddWhere(logicalOperator, field.ToString(), @operator, compareValue, 1); }
        public WhereClause AddWhere(LogicOperator logicalOperator, string field, Comparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new GeneralWhereClause(logicalOperator, field, @operator, compareValue, level);
            _whereStatement.Add(NewWhereClause);
            return NewWhereClause;
        }

        public WhereClause AddWhere(LogicOperator logicalOperator, string field, string fromValue, string toValue, int level)
        {
            WhereClause NewWhereClause = new BetweenWhereClause(logicalOperator, field, fromValue, toValue, level);
            _whereStatement.Add(NewWhereClause);
            return NewWhereClause;
        }

        public void AddOrderBy(OrderByClause clause)
        {
            _orderByStatement.Add(clause);
        }
        public void AddOrderBy(Enum field, Sorting order) { this.AddOrderBy(field.ToString(), order); }
        public void AddOrderBy(string field, Sorting order)
        {
            OrderByClause NewOrderByClause = new OrderByClause(field, order);
            _orderByStatement.Add(NewOrderByClause);
        }

        public void AddGroupBy(Column column)
        {
            _groupByColumns.Add(column);
        }

        public void GroupBy(Column[] columns)
        {
            foreach (Column Column in columns)
            {
                _groupByColumns.Add(Column);
            }
        }

        public void GroupBy(List<Column> columns)
        {
            _groupByColumns = columns;
        }

        public void AddSummarize(Column column)
        {
            _summarizeColumns.Add(column);
        }

        public void Summarize(Column[] columns)
        {
            foreach (Column Column in columns)
            {
                _summarizeColumns.Add(Column);
            }
        }

        public void Summarize(List<Column> columns)
        {
            _summarizeColumns = columns;
        }

        public WhereStatement Having
        {
            get { return _havingStatement; }
            set { _havingStatement = value; }
        }

        public void AddHaving(WhereClause clause) { AddHaving(clause, 1); }
        public void AddHaving(WhereClause clause, int level)
        {
            _havingStatement.Add(clause);
        }
        //public WhereClause AddHaving(string field, Comparison @operator, object compareValue) { return AddHaving(field, @operator, compareValue); }
        //public WhereClause AddHaving(Enum field, Comparison @operator, object compareValue) { return AddHaving(field.ToString(), @operator, compareValue, 1); }
        public WhereClause AddHaving(string field, Comparison @operator, object compareValue)
        {
            WhereClause NewWhereClause = new GeneralWhereClause(LogicOperator.None, field, @operator, compareValue, 0);
            _havingStatement.Add(NewWhereClause);
            return NewWhereClause;
        }

        public CrossTabulationClause CrossTabClause
        {
            get { return _crossTabClause; }
            set { _crossTabClause = value; }
        }


        public string getSelectPartQuery(int startCol, int numCols, out int totalCols)
        {
            return getSelectPartQuery(startCol, numCols, out totalCols, false, false);
        }

        public string getSelectPartQuery(int startCol, int numCols, out int totalCols, bool ignoreGroupBy, bool nestedGroupBy)
        {
            string Query = "SELECT ";

            //// Output Distinct
            //if (_distinct)
            //{
            //    Query += "DISTINCT ";
            //}

            //// Output Top clause
            //if (!(_topClause.Quantity == 100 & _topClause.Unit == TopUnit.Percent))
            //{
            //    Query += "TOP " + _topClause.Quantity;
            //    if (_topClause.Unit == TopUnit.Percent)
            //    {
            //        Query += " PERCENT";
            //    }
            //    Query += " ";
            //}

            List<Column> columns;
            if (ignoreGroupBy)
            {
               columns = _selectedColumns;
               if (_selectedColumns == null || _selectedColumns.Count <= 0)
               {

               }
            }
            else
            {
                if (_crossTabClause.Col != null)
                {
                    //columns = _crossTabClause.SelectedColumns;
                    columns = new List<Column>(_groupByColumns);
                    columns.Add(CrossTabClause.Col);
                    columns.AddRange(_summarizeColumns);
                }
                else
                {
                    if (_groupByColumns.Count > 0)
                    {
                        columns = new List<Column>(_groupByColumns);
                        columns.AddRange(_summarizeColumns);
                    }
                    else
                    {
                        columns = _selectedColumns;
                    }
                }
                _finalSelectedColumns = columns;
            }
            totalCols = columns.Count();
            if (totalCols == 0)
            {
                if(_selectedTables.Count > 0)
                {
                    Query += "*";
                }
                else
                {
                for (int i = 0; i < _selectedTables.Count; i++)
                {
                    Query += _selectedTables[0].AliasName + ".*,";
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                if (_selectedTables.Count == 1)
                    Query += _selectedTables[0] + "."; // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.
                }
            }
            else
            {
                int endCol;
                if (numCols < 0)
                {
                    endCol = totalCols;
                }
                else
                {
                    endCol = startCol + numCols;
                }
                if (endCol > totalCols)
                {
                    endCol = totalCols;
                }
                for (int i = startCol; i < endCol; i++)
                {
                    Column Col = (Column)columns.ElementAt(i);
                    if (nestedGroupBy)
                    {
                        int stParenIndex = Col.Name.IndexOf('(');
                        int endParenIndex = Col.Name.IndexOf(')');
                        string colName;
                        if (stParenIndex >= 0 && endParenIndex >=0)
                        {
                            colName = Col.Name.Substring(0, stParenIndex + 1) + "grp." + Col.Name.Substring((stParenIndex + 1), (endParenIndex - stParenIndex)-1) + Col.Name.Substring(endParenIndex);
                        }
                        else
                        {
                            colName = "grp." + Col.Name;
                        }
                        Query += getColumnPartQuery(colName);
                        if (Col.AliasName == null || "".Equals(Col.AliasName.Trim()))
                        {
                            Query += " AS \"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Col.Name.Substring(Col.Name.IndexOf('.') + 1).Replace('_', ' ')) + "\",";
                        }
                        else
                        {
                            Query += " AS \"" + Col.AliasName + "\",";
                        }
                    }
                    else
                    {
                        Query += getColumnPartQuery(Col);
                        if (Col.AliasName == null || "".Equals(Col.AliasName.Trim()))
                        {
                            Query += " AS \"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Col.Name.Substring(Col.Name.IndexOf('.') + 1).Replace('_', ' ')) + "\",";
                        }
                        else
                        {
                            Query += " AS \"" + Col.AliasName + "\",";
                        }
                    }
                }

                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
            }
            Query += ' ';

            return Query;
        }

        public string getCrossTabSelectPartQuery()
        {
            string Query = "SELECT ";

            // Output Distinct
            //if (_distinct)
            //{
            //    Query += "DISTINCT ";
            //}

            List<Column> columns = new List<Column>();

            for (int i = 0; i < _groupByColumns.Count; i++)
            {
                columns.Add((Column)_groupByColumns.ElementAt(i));
            }
            columns.Add(_crossTabClause._col);
            List<Column> summarizeCols = _crossTabResults.SummarizeColumns;
            for (int i = 0; i < summarizeCols.Count; i++)
            {
                columns.Add((Column)summarizeCols.ElementAt(i));
            }

            int totalCols = _groupByColumns.Count + 1 + summarizeCols.Count;

            for (int i = 0; i < totalCols; i++)
            {
                Column Col = (Column)columns.ElementAt(i);
                Query += SelectQueryBuilder.getColumnPartQuery(Col.Name);
                if (Col.AliasName == null || "".Equals(Col.AliasName.Trim()))
                {
                    Query += " AS \"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Col.Name.Substring(Col.Name.IndexOf('.') + 1).Replace('_', ' ')) + "\",";
                }
                else
                {
                    Query += " AS \"" + Col.AliasName + "\",";
                }
            }

            Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop

            Query += ' ';

            return Query;
        }

        //public void setSelectedColumns_CrossTabulation(string connectionString)
        //{
        //    Column crossTabColumn = CrossTabClause.Col;
        //    string crossTabColQuery = "SELECT DISTINCT " + crossTabColumn.Name + getCrossTabQueryPartWithoutSelect();
        //            //_selectedTables.ElementAt<Table>(0).Name + " " +_selectedTables.ElementAt<Table>(0).AliasName;
        //    if (CrossTabClause.SortSet)
        //    {
        //        if (CrossTabClause.SortOrder == Sorting.Ascending)
        //        {
        //            crossTabColQuery += " ORDER BY " + crossTabColumn.Name + " ASC";
        //        }
        //        else if (CrossTabClause.SortOrder == Sorting.Descending)
        //        {
        //            crossTabColQuery += " ORDER BY " + crossTabColumn.Name + " DESC";
        //        }

        //    }
        //    crossTabColQuery += ";";

        //    MySqlConnection connection = new MySqlConnection(connectionString);
        //    connection.Open();
        //    try
        //    {
        //        List<Column> columns = new List<Column>();
        //        //ADD groupby columns to select columns
        //        for (int i = 0; i < _groupByColumns.Count; i++)
        //        {
        //            Column c = _groupByColumns.ElementAt<Column>(i);

        //            columns.Add(c);
        //        }

        //        DataSet dataSet = new DataSet();
        //        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(crossTabColQuery, connection);
        //        dataAdapter.Fill(dataSet);
        //        DataRowCollection rows = dataSet.Tables[0].Rows;

        //        List<string> crossTabValues = new List<string>();
        //        List<Column> summarizeColumns = new List<Column>();
        //        for (int i = 0; i <= rows.Count; i++)
        //        {
        //            String name = null;
        //            if (i < rows.Count)
        //            {
        //                DataRow row = rows[i];
        //                if (row.ItemArray[0] != System.DBNull.Value)
        //                {
        //                    //name = (string)row.ItemArray[0];
        //                    name = "" + row.ItemArray[0];
        //                }
        //                crossTabValues.Add(name);
        //            }

        //            for (int colIndex = 0; colIndex < _selectedColumns.Count; colIndex++)
        //            {
        //                Column currentCol = _selectedColumns.ElementAt<Column>(colIndex);

        //                Column col = new Column();
        //                int ColNameIndex = currentCol.Name.IndexOf('(')+1;
                        
        //                if (ColNameIndex > 0)
        //                {
        //                    string summuryFunction = currentCol.Name.Substring(0, ColNameIndex);
        //                    string colName = currentCol.Name.Substring(ColNameIndex, (currentCol.Name.IndexOf(')') - ColNameIndex));

        //                    //SUM(IF(grade_mic='A-',close_bal,0)) 'A- Closing Balance',
        //                    if (i < rows.Count)
        //                    {
        //                        if (name != null)
        //                        {
        //                            name = name.Replace("'", "''");
        //                            if (summuryFunction.ToUpper().StartsWith("COUNT"))
        //                            {
        //                                col.Name = "SUM(" + "IF(" + crossTabColumn.Name + "='" + name + "'," + "1,0))";
        //                            }
        //                            else
        //                            {
        //                                col.Name = summuryFunction + "IF(" + crossTabColumn.Name + "='" + name + "'," + colName + ",0))";
        //                            }
        //                            //col.AliasName = name + " " + currentCol.AliasName;
        //                            col.AliasName = currentCol.AliasName;
        //                        }
        //                        else
        //                        {
        //                            if (summuryFunction.ToUpper().StartsWith("COUNT"))
        //                            {
        //                                col.Name = "SUM(" + "IF(ISNULL(" + crossTabColumn.Name + ")" + "," + "1,0))";
        //                            }
        //                            else
        //                            {
        //                                col.Name = summuryFunction + "IF(ISNULL(" + crossTabColumn.Name + ")" + "," + colName + ",0))";
        //                            }
        //                            col.AliasName = "NULL " + currentCol.AliasName;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        col.Name = currentCol.Name;
        //                        col.AliasName = currentCol.AliasName;
        //                    }
        //                    columns.Add(col);
        //                }

        //            }
                        
        //        }

        //        //add summarize columns to the list
        //        for (int i = _groupByColumns.Count; i < _selectedColumns.Count; i++)
        //        {
        //            summarizeColumns.Add(_selectedColumns.ElementAt<Column>(i));
        //        }
        //        _crossTabResults = new CrossTabResults(_groupByColumns, crossTabColumn, summarizeColumns, crossTabValues);
        //        CrossTabClause.SelectedColumns = columns;
        //        //_selectedColumns.Clear();
        //        //_selectedColumns = columns;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //}

        public void setSelectedColumns_CrossTabulation_old(string connectionString)
        {
            Column crossTabColumn = CrossTabClause.Col;
            string crossTabColQuery = "SELECT DISTINCT " + crossTabColumn.Name + getCrossTabQueryPartWithoutSelect();
            if (CrossTabClause.SortSet)
            {
                if (CrossTabClause.SortOrder == Sorting.Ascending)
                {
                    crossTabColQuery += " ORDER BY " + crossTabColumn.Name + " ASC";
                }
                else if (CrossTabClause.SortOrder == Sorting.Descending)
                {
                    crossTabColQuery += " ORDER BY " + crossTabColumn.Name + " DESC";
                }

            }
            crossTabColQuery += ";";

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                List<Column> columns = new List<Column>();
                //ADD groupby columns to select columns
                //for (int i = 0; i < _groupByColumns.Count; i++)
                //{
                //    Column c = _groupByColumns.ElementAt<Column>(i);

                //    columns.Add(c);
                //}

                //columns.Add(crossTabColumn);

                DataSet dataSet = new DataSet();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(crossTabColQuery, connection);
                dataAdapter.Fill(dataSet);
                DataRowCollection rows = dataSet.Tables[0].Rows;

                List<string> crossTabValues = new List<string>();
                List<Column> summarizeColumns = new List<Column>();
                for (int i = 0; i < rows.Count; i++)
                {
                    String name = null;
                    if (i < rows.Count)
                    {
                        DataRow row = rows[i];
                        if (row.ItemArray[0] != System.DBNull.Value)
                        {
                            //name = (string)row.ItemArray[0];
                            name = "" + row.ItemArray[0];
                        }
                        crossTabValues.Add(name);
                    }

                }

                //add summarize columns to the list
                for (int i = _groupByColumns.Count; i < _selectedColumns.Count; i++)
                {
                    summarizeColumns.Add(_selectedColumns.ElementAt<Column>(i));
                }
                _crossTabResults = new CrossTabResults(_groupByColumns, crossTabColumn, summarizeColumns, crossTabValues);
                CrossTabClause.SelectedColumns = columns;
            }
            finally
            {
                connection.Close();
            }

        }

        public void 
            setSelectedColumns_CrossTabulation(string connectionString)
        {
            Column crossTabColumn = CrossTabClause.Col;
            List<Column> columns = new List<Column>();
            List<Column> summarizeColumns = new List<Column>();
            //add summarize columns to the list
            //for (int i = _groupByColumns.Count; i < _selectedColumns.Count; i++)
            //{
            //    summarizeColumns.Add(_selectedColumns.ElementAt<Column>(i));
            //}
            _crossTabResults = new CrossTabResults(_groupByColumns, crossTabColumn, _summarizeColumns);
            CrossTabClause.SelectedColumns = columns;

        }

        public string getQueryPartWithoutSelect()
        {
            return getQueryPartWithoutSelect(null, false);
        }

        public string getQueryPartWithoutSelect(string sortColumn, bool ascending)
        {
            string Query = "";

            // Output table names
            if (_selectedTables.Count > 0)
            {
                Query += " FROM ";
                foreach (Table Tab in _selectedTables)
                {
                    if (Tab is DerivedTable)
                    {
                        Query += "(" + ((DerivedTable)Tab).Query + ")  " + Tab.AliasName + ",";
                    }
                    else if (Tab is Table)
                    {
                        if (Tab.SchemaName == null || Tab.SchemaName.Trim().Equals(""))
                        {
                            Query += Tab.Name + " " + Tab.AliasName + ",";
                        }
                        else
                        {
                            Query += Tab.SchemaName + "." +Tab.Name + " " + Tab.AliasName + ",";
                        }
                    }
                 }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }

            // Output joins
            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case JoinType.InnerJoin: JoinString = "INNER JOIN"; break;
                        //case JoinType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    string toTableStr = "";
                    if (Clause.ToTable is DerivedTable)
                    {
                        toTableStr += "(" + ((DerivedTable)Clause.ToTable).Query + ")";
                    }
                    else if (Clause.ToTable is Table)
                    {
                        if (Clause.ToTable.SchemaName == null || Clause.ToTable.SchemaName.Trim().Equals(""))
                        {
                            toTableStr += Clause.ToTable.Name;
                        }
                        else
                        {
                            toTableStr += Clause.ToTable.SchemaName + "." + Clause.ToTable.Name;
                        }                        //toTableStr += Clause.ToTable.Name;
                    }
                    JoinString += " " + toTableStr + " " + Clause.ToTable.AliasName + " ON ";
                    //JoinString += WhereStatement.CreateComparisonClause(Clause.FromTable.AliasName + '.' + "`"+Clause.FromColumn+"`", Clause.ComparisonOperator, Clause.ToTable.AliasName + '.' + "`"+Clause.ToColumn+"`");
                    JoinString += Clause.JoinCondition.BuildWhereStatement();
                    Query += JoinString + ' ';
                }
            }

            // Check the computed column and add the joins according ly for GROUP_FIRST, GROUP_LAST etc
            Query += getComputeColumnsFromClauseQuery();

            // Output where statement
            if (_whereStatement.ClauseLevels > 0)
            {
                Query += " WHERE " + _whereStatement.BuildWhereStatement();
            }

            // Output GroupBy statement
            if (_groupByColumns.Count > 0)
            {
                Query += " GROUP BY ";
                foreach (Column Col in _groupByColumns)
                {
                    Query += SelectQueryBuilder.getColumnPartQuery(Col.Name) + ',';
                }
                // Add cross Tabulation column as group by in query
                if (CrossTabClause.Col != null)
                {
                    Query += SelectQueryBuilder.getColumnPartQuery(CrossTabClause.Col.Name) + ',';
                }
                Query = Query.TrimEnd(',');
                Query += ' ';
            }

            // Output having statement
            if (_havingStatement.ClauseLevels > 0)
            {
                // Check if a Group By Clause was set
                if (_groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }

                Query += " HAVING " + _havingStatement.BuildWhereStatement();
            }

            // Output OrderBy statement
            if (_orderByStatement.Count > 0 || sortColumn != null)
            {
                Query += " ORDER BY ";
                string OrderByClause = "";
                if (sortColumn != null)
                {
                    if (ascending)
                    {
                        OrderByClause = SelectQueryBuilder.getColumnPartQuery(sortColumn) + " ASC,";
                    }
                    else
                    {
                        OrderByClause = SelectQueryBuilder.getColumnPartQuery(sortColumn) + " DESC,";
                    }
                }
                foreach (OrderByClause Clause in _orderByStatement)
                {
                    switch (Clause.SortOrder)
                    {
                        case Sorting.Ascending:
                            OrderByClause += SelectQueryBuilder.getColumnPartQuery(Clause.FieldName) + " ASC,"; break;
                        case Sorting.Descending:
                            OrderByClause += SelectQueryBuilder.getColumnPartQuery(Clause.FieldName) + " DESC,"; break;
                    }
                }
                Query += OrderByClause;
                Query = Query.TrimEnd(','); // Trim de last AND inserted by foreach loop
                Query += ' ';
            }

            // Return the built query
            return Query;
        }


        public string getQueryPartWithoutSelectForGroupBy()
        {
            string Query = "";
            int totalCols;

            string selectPart = getSelectPartQuery(0, -1, out totalCols, true, false);
            Query = " FROM (" + selectPart;

            // Output table names
            if (_selectedTables.Count > 0)
            {
                Query += " FROM ";
                foreach (Table Tab in _selectedTables)
                {
                    Query += Tab.Name + " " + Tab.AliasName + ",";
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }

            // Output joins
            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case JoinType.InnerJoin: JoinString = "INNER JOIN"; break;
                        //case JoinType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    JoinString += " " + Clause.ToTable.Name + " " + Clause.ToTable.AliasName + " ON ";
                    //JoinString += WhereStatement.CreateComparisonClause(Clause.FromTable.AliasName + '.' + "`" + Clause.FromColumn+"`", Clause.ComparisonOperator, Clause.ToTable.AliasName + '.' + "`"+Clause.ToColumn+"`");
                    JoinString += Clause.JoinCondition.BuildWhereStatement();
                    Query += JoinString + ' ';
                }
            }

            // Output where statement
            if (_whereStatement.ClauseLevels > 0)
            {
                Query += " WHERE " + _whereStatement.BuildWhereStatement();
            }

            Query += ") grp ";
            // Output GroupBy statement
            if (_groupByColumns.Count > 0)
            {
                Query += " GROUP BY ";
                foreach (Column Col in _groupByColumns)
                {
                    Query += "`" + Col.Name + "`" + ',';
                }
                // Add cross Tabulation column as group by in query
                if (CrossTabClause.Col != null)
                {
                    Query += "`" + CrossTabClause.Col.Name + "`" + ',';
                }
                Query = Query.TrimEnd(',');
                Query += ' ';
            }

            // Output having statement
            if (_havingStatement.ClauseLevels > 0)
            {
                // Check if a Group By Clause was set
                if (_groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }

                Query += " HAVING " + _havingStatement.BuildWhereStatement();
            }

            // Output OrderBy statement
            if (_orderByStatement.Count > 0)
            {
                Query += " ORDER BY ";
                string OrderByClause = "";
                    
                foreach (OrderByClause Clause in _orderByStatement)
                {
                    string orderbyFiledStr;
                    if (_selectedColumns.Count > 0 && _groupByColumns.Count > 0)
                    {
                        orderbyFiledStr = "`" + Clause.FieldName + "`";
                    }
                    else
                    {
                        orderbyFiledStr = SelectQueryBuilder.getColumnPartQuery(Clause.FieldName);
                    }
                    switch (Clause.SortOrder)
                    {
                        case Sorting.Ascending:
                            OrderByClause += orderbyFiledStr + " ASC,"; break;
                        case Sorting.Descending:
                            OrderByClause += orderbyFiledStr + " DESC,"; break;
                    }
                }
                Query += OrderByClause;
                Query = Query.TrimEnd(','); // Trim de last AND inserted by foreach loop
                Query += ' ';
            }

            // Return the built query
            return Query;
        }

        public string getQueryforGroupBy(out int totalCols)
        {
            string Query = "";

            if (_selectedColumns.Count > 0 && _groupByColumns.Count > 0)
            {
                Query += getSelectPartQuery(0, -1, out totalCols, false, true); ;
                Query += getQueryPartWithoutSelectForGroupBy();
            }
            else
            {
                Query += getSelectPartQuery(0, -1, out totalCols);
                Query += getQueryPartWithoutSelect(null, false);
            }
            return Query;
        }

        public string getCrossTabQueryPartWithoutSelect()
        {
            string Query = "";

            // Output table names
            if (_selectedTables.Count > 0)
            {
                Query += " FROM ";
                foreach (Table Tab in _selectedTables)
                {
                    Query += Tab.Name + " " + Tab.AliasName + ",";
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }

            // Output joins
            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case JoinType.InnerJoin: JoinString = "INNER JOIN"; break;
                        //case JoinType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    JoinString += " " + Clause.ToTable.Name + " " + Clause.ToTable.AliasName + " ON ";
                    //JoinString += WhereStatement.CreateComparisonClause(Clause.FromTable.AliasName + '.' + "`"+Clause.FromColumn+"`", Clause.ComparisonOperator, new SqlLiteral(Clause.ToTable.AliasName + '.' + "`"+Clause.ToColumn+"`"));
                    JoinString += Clause.JoinCondition.BuildWhereStatement();
                    Query += JoinString + ' ';
                }
            }

            // Output where statement
            if (_whereStatement.ClauseLevels > 0)
            {
                Query += " WHERE " + _whereStatement.BuildWhereStatement();
            }

            // Return the built query
            return Query;
        }

        public string getLimitRowsPartQuery(Int64 start, int itemCount)
        {
            string Query = "";
            if (_groupByColumns.Count() == 0)
            {
                Query += " LIMIT " + start + "," + itemCount; ;
            }

            return Query;
        }

        /// <summary>
        /// Builds the select query
        /// </summary>
        /// <returns>Returns a string containing the query, or a DbCommand containing a command with parameters</returns>
        /// 
        public string BuildQuery()
        {
            string Query = "SELECT ";

            //// Output Distinct
            //if (_distinct)
            //{
            //    Query += "DISTINCT ";
            //}

            //// Output Top clause
            //if (!(_topClause.Quantity == 100 & _topClause.Unit == TopUnit.Percent))
            //{
            //    Query += "TOP " + _topClause.Quantity;
            //    if (_topClause.Unit == TopUnit.Percent)
            //    {
            //        Query += " PERCENT";
            //    }
            //    Query += " ";
            //}

            // Output column names
            if (_selectedColumns.Count == 0)
            {
                if (_selectedTables.Count == 1)
                    Query += _selectedTables[0] + "."; // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.

                Query += "*";
            }
            else
            {
                foreach (Column Col in _selectedColumns)
                {
                    if (Col.AliasName == null || "".Equals(Col.AliasName.Trim()))
                    {
                        Query += Col.Name + " AS \"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Col.Name.Substring(Col.Name.IndexOf('.') + 1).Replace('_', ' ')) + "\",";
                    }
                    else
                    {
                        Query += Col.Name + " AS \"" + Col.AliasName + "\",";
                    }
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }
            // Output table names
            if (_selectedTables.Count > 0)
            {
                Query += " FROM ";
                foreach (Table Tab in _selectedTables)
                {
                    Query += Tab.Name + " " + Tab.AliasName + ",";
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }

            // Output joins
            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case JoinType.InnerJoin: JoinString = "INNER JOIN"; break;
                        //case JoinType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    JoinString += " " + Clause.ToTable.Name + " " + Clause.ToTable.AliasName + " ON ";
                    //JoinString += WhereStatement.CreateComparisonClause(Clause.FromTable.AliasName + '.' + Clause.FromColumn, Clause.ComparisonOperator, new SqlLiteral(Clause.ToTable.AliasName + '.' + Clause.ToColumn));
                    JoinString += Clause.JoinCondition.BuildWhereStatement();
                    Query += JoinString + ' ';
                }
            }

            // Output where statement
            if (_whereStatement.ClauseLevels > 0)
            {
                Query += " WHERE " + _whereStatement.BuildWhereStatement();
            }

            // Output GroupBy statement
            if (_groupByColumns.Count > 0)
            {
                Query += " GROUP BY ";
                foreach (Column Col in _groupByColumns)
                {
                    Query += Col.Name + ',';
                }
                Query = Query.TrimEnd(',');
                Query += ' ';
            }

            // Output having statement
            if (_havingStatement.ClauseLevels > 0)
            {
                // Check if a Group By Clause was set
                if (_groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }
                
                Query += " HAVING " + _havingStatement.BuildWhereStatement();
            }

            // Output OrderBy statement
            if (_orderByStatement.Count > 0)
            {
                Query += " ORDER BY ";
                foreach (OrderByClause Clause in _orderByStatement)
                {
                    string OrderByClause = "";
                    switch (Clause.SortOrder)
                    {
                        case Sorting.Ascending:
                            OrderByClause = Clause.FieldName + " ASC"; break;
                        case Sorting.Descending:
                            OrderByClause = Clause.FieldName + " DESC"; break;
                    }
                    Query += OrderByClause + ',';
                }
                Query = Query.TrimEnd(','); // Trim de last AND inserted by foreach loop
                Query += ' ';
            }

            // Return the built query
            return Query;
        }

        public static string getColumnPartQuery(Column column)
        {
            if (column is ComputedColumn)
            {
                ComputedColumn compColumn = (ComputedColumn)column;

                if (compColumn.Type == ComputedColumn.FUNCTION)
                {
                    if (compColumn.Name.StartsWith(Functions.FUNC_GROUP_FIRST) || compColumn.Name.StartsWith(Functions.FUNC_GROUP_LAST))
                    {
                        List<Parameter> parameters = compColumn.Parameters;
                        string refCol = getColumnName(parameters[2].Value);
                        return compColumn.AliasName+"."+refCol;
                    }
                }
            }
            
            return getColumnPartQuery(column.Name);
        }

        public static string getColumnName(string str)
        {
            StringBuilder columnStr = new StringBuilder("");

            int dotIndex = str.IndexOf('.');
            if(dotIndex >= 0)
            {
                while(true)
                {
                    str = str.Substring(dotIndex + 1);
                    int i = 0;
                    for (; i < str.Length; i++)
                    {
                        char c = str[i];
                        // Add the code ignore the string in single quotes and double quotes etc
                        if (!(char.IsLetter(c)) && (!(char.IsNumber(c))) && (!(char.IsWhiteSpace(c))) && c != '_')
                            break;
                    }
                    columnStr.Append("`"+str.Substring(0, i).Trim()+"`");
                    if (i < str.Length)
                    {
                        str = str.Substring(i);
                        dotIndex = str.IndexOf('.');
                        if (dotIndex < 0)
                        {
                            columnStr.Append(str);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                columnStr.Append(str);
            }

            return columnStr.ToString();
        }

        public static string getColumnPartQuery(string str)
        {
            StringBuilder columnStr = new StringBuilder("");

            str = str.Replace("COUNT_DISTINCT(", "COUNT(DISTINCT ");
            int dotIndex = str.IndexOf('.');
            if(dotIndex >= 0)
            {
                while(true)
                {
                    columnStr.Append(str.Substring(0, dotIndex + 1));
                    str = str.Substring(dotIndex + 1);
                    int i = 0;
                    for (; i < str.Length; i++)
                    {
                        char c = str[i];
                        // Add the code ignore the string in single quotes and double quotes etc
                        if (!(char.IsLetter(c)) && (!(char.IsNumber(c))) && (!(char.IsWhiteSpace(c))) && c != '_')
                            break;
                    }
                    columnStr.Append("`"+str.Substring(0, i).Trim()+"`");
                    if (i < str.Length)
                    {
                        str = str.Substring(i);
                        dotIndex = str.IndexOf('.');
                        if (dotIndex < 0)
                        {
                            columnStr.Append(str);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                columnStr.Append(str);
            }

            return columnStr.ToString();
        }

        public string getComputeColumnsFromClauseQuery()
        {
            string query = "";

            foreach (Column col in _selectedColumns)
            {
                if (col is ComputedColumn)
                {
                    ComputedColumn compColumn = (ComputedColumn)col;
                    string compColumnAlias = compColumn.AliasName;
                    if (compColumn.Name.StartsWith(Functions.FUNC_GROUP_FIRST) || compColumn.Name.StartsWith(Functions.FUNC_GROUP_LAST))
                    {
                        List<Parameter> parameters = compColumn.Parameters;
                        string groupCol = getColumnName(parameters[0].Value);
                        string orderCol = getColumnName(parameters[1].Value); 
                        string refCol = getColumnName(parameters[2].Value);

                        string masterTableName = _selectedTables[0].Name;
                        string masterTableAlias = _selectedTables[0].AliasName;
                        string alias1 = compColumnAlias + "1";
                        string alias2 = compColumnAlias + "2";
                        if (compColumn.Name.StartsWith(Functions.FUNC_GROUP_FIRST))
                        {
                            query += " left outer join ( select " + alias1 + "." + groupCol + "," + alias1 + "." + refCol +
                                        " from " + masterTableName + " " + alias1 + " inner join ( " +
                                        "select " + groupCol + ", " + "min(" + orderCol + ") as " + orderCol + " from " + masterTableName + " group by " + groupCol + " ) " +
                                        alias2 + " on " + alias1 + "." + groupCol + " = " + alias2 + "." + groupCol + " and " + alias1 + "." + orderCol + " = " + alias2 + "." + orderCol +
                                        " ) " + compColumnAlias + " on " + masterTableAlias + "." + groupCol + " = " + compColumnAlias + "." + groupCol + " ";
                        }
                        else
                        {
                            query += " left outer join ( select " + alias1 + "." + groupCol + "," + alias1 + "." + refCol +
                                        " from " + masterTableName + " " + alias1 + " inner join ( " +
                                        "select " + groupCol + ", " + "max(" + orderCol + ") as " + orderCol + " from " + masterTableName + " group by " + groupCol + " ) " +
                                        alias2 + " on " + alias1 + "." + groupCol + " = " + alias2 + "." + groupCol + " and " + alias1 + "." + orderCol + " = " + alias2 + "." + orderCol +
                                        " ) " + compColumnAlias + " on " + masterTableAlias + "." + groupCol + " = " + compColumnAlias + "." + groupCol + " ";
                        }

                    }
                }
            }

            return query;
        }
    }

}
