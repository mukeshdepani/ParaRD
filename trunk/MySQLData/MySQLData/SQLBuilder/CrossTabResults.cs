using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Clauses;
using System.Data;

namespace SQLBuilder
{
    public class CrossTabResults
    {
        protected List<Column> _groupByColumns; 
        protected Column _crossTabColumn;
        protected List<Column> _summarizeColumns; 
        protected List<string> _crossTabColumnVaues;
        protected DataTable _results;
        protected Dictionary<string, Object> _dataMap;
        protected List<List<string>> _groupByColumnValueList;
        protected List<string> _keyPrefixes;

        public CrossTabResults()
        {
        }

        public CrossTabResults(List<Column> groupByColumns, Column crossTabColumn, List<Column> summarizeColumns)
        {
            _groupByColumns = groupByColumns;
            _crossTabColumn = crossTabColumn;
            _summarizeColumns = summarizeColumns;
        }

        public CrossTabResults(List<Column> groupByColumns, Column crossTabColumn, List<Column> summarizeColumns, List<string> crossTabColumnVaues)
        {
            _groupByColumns = groupByColumns;
            _crossTabColumn = crossTabColumn;
            _summarizeColumns = summarizeColumns;
            _crossTabColumnVaues = crossTabColumnVaues;
        }

        public List<Column> GroupByColumns
        {
            get { return _groupByColumns; }
            set { _groupByColumns = value; }
        }

        public Column CrossTabColumn
        {
            get { return _crossTabColumn; }
            set { _crossTabColumn = value; }
        }

        public List<Column> SummarizeColumns
        {
            get { return _summarizeColumns; }
            set { _summarizeColumns = value; }
        }

        public List<string> CrossTabColumnVaues
        {
            get { return _crossTabColumnVaues; }
            set { _crossTabColumnVaues = value; }
        }

        public DataTable Results
        {
            get { return _results; }
            set { _results = value; }
        }

        public Dictionary<string, Object> DataMap
        {
            get { return _dataMap; }
            set { _dataMap = value; }
        }

        public List<List<string>> GroupByColumnValueList
        {
            get { return _groupByColumnValueList; }
            set { _groupByColumnValueList = value; }
        }

        public List<string> KeyPrefixes
        {
            get { return _keyPrefixes; }
            set { _keyPrefixes = value; }
        }
    }
}
