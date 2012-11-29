using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    public class OrderByClause
    {
        public string FieldName;
        public Sorting SortOrder;

        public OrderByClause()
        {
        }

        public OrderByClause(string field)
        {
            FieldName = field;
            SortOrder = Sorting.Ascending;
        }
        public OrderByClause(string field, Sorting order)
        {
            FieldName = field;
            SortOrder = order;
        }
    }
}
