using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    public class BetweenWhereClause: WhereClause  
    {
        private string m_FieldName;
        private string m_FromValue;
        private string m_ToValue;

        public BetweenWhereClause()
        {
        }

        /// <summary>
        /// Gets/sets the name of the database column this WHERE clause should operate on
        /// </summary>
        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }

        /// <summary>
        /// Gets/sets the FromValue method
        /// </summary>
        public string FromValue
        {
            get { return m_FromValue; }
            set { m_FromValue = value; }
        }

        /// <summary>
        /// Gets/sets the FromValue method
        /// </summary>
        public string ToValue
        {
            get { return m_ToValue; }
            set { m_ToValue = value; }
        }

        public BetweenWhereClause(LogicOperator logicalOperator, string field, string fromValue, string toValue, int level)
        {
            base.LogicalOperator = logicalOperator;
            m_FieldName = field;
            m_FromValue = fromValue;
            m_ToValue = toValue;
            base.Level = level;
        }
    }
}
