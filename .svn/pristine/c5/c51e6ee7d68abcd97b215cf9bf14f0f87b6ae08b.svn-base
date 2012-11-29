using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    public class JoinClause
    {
        public JoinType JoinType;
        public Table FromTable;
        public string FromColumn;
        public Comparison ComparisonOperator;
        public Table ToTable;
        public string ToColumn;

        public WhereStatement JoinCondition = new WhereStatement();

        public JoinClause()
        {
        }

        //public JoinClause(JoinType join, Table toTable, string toColumnName, Comparison @operator, Table fromTable, string fromColumnName)
        //{
        //    JoinType = join;
        //    FromTable = fromTable;
        //    FromColumn = fromColumnName;
        //    ComparisonOperator = @operator;
        //    ToTable = toTable;
        //    ToColumn = toColumnName;
        //}
        public JoinClause(JoinType join, Table fromTable, string fromColumnName, Comparison @operator, Table toTable, string toColumnName)
        {
            JoinType = join;
            FromTable = fromTable;
            FromColumn = fromColumnName;
            ComparisonOperator = @operator;
            ToTable = toTable;
            ToColumn = toColumnName;

            JoinCondition.Add(LogicOperator.None, fromTable.AliasName + "." + fromColumnName, @operator, toTable.AliasName + "." + toColumnName, 0, 0);
        }

        public void addJoinCondition(LogicOperator logicOperator, Table fromTable, string fromColumnName, Comparison @operator, Table toTable, string toColumnName)
        {
            JoinCondition.Add(logicOperator, fromTable.AliasName + "." + fromColumnName, @operator, toTable.AliasName + "." + toColumnName, 0);
        }

    }
}

