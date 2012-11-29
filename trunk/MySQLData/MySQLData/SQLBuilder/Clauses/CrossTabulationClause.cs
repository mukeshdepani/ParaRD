using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    public class CrossTabulationClause
    {
        public Column _col;
        public bool _sortSet = false;
        public Sorting _sortOrder;
        public List<Column> _selectedColumns = new List<Column>();

        public CrossTabulationClause()
        {
        }

        public CrossTabulationClause(Column col)
        {
            _col = col;
            _sortSet = false;
        }

        public CrossTabulationClause(Column col, Sorting sortOrder)
        {
            _col = col;
            _sortSet = true;
            _sortOrder = sortOrder;
        }

        public Column Col
        {
            get { return _col; }
            set { _col = value; }
        }

        public Sorting SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        public bool SortSet
        {
            get { return _sortSet; }
            set { _sortSet = value; }
        }

        public List<Column> SelectedColumns
        {
            get { return _selectedColumns; }
            set { _selectedColumns = value; }
        }
    }
}
