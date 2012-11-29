using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    [System.Xml.Serialization.XmlInclude(typeof(GeneralWhereClause))]
    [System.Xml.Serialization.XmlInclude(typeof(BetweenWhereClause))]
    public abstract class WhereClause
    {
        private LogicOperator m_LogicalOperator;
        private int m_Level;

        //internal class SubClause
        //{
        //    public LogicOperator LogicOperator;
        //    public Comparison ComparisonOperator;
        //    public object Value;

        //    public SubClause()
        //    {
        //    }

        //    public SubClause(LogicOperator logic, Comparison compareOperator, object compareValue)
        //    {
        //        LogicOperator = logic;
        //        ComparisonOperator = compareOperator;
        //        Value = compareValue;
        //    }
        //}
        //internal List<SubClause> SubClauses;	// Array of SubClause

        /// <summary>
        /// Gets/sets the LogicOperator method
        /// </summary>
        public LogicOperator LogicalOperator
        {
            get { return m_LogicalOperator; }
            set { m_LogicalOperator = value; }
        }

        /// <summary>
        /// Gets/sets the Level of the sub condition
        /// </summary>
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

    }
}
