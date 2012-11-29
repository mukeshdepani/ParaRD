using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    public class WhereStatement : List<WhereClause>
    {
        // The list in this container will contain lists of clauses, and 
        // forms a where statement alltogether!

        public WhereStatement()
        {
        }

        public int ClauseLevels
        {
            get { return this.Count; }
        }

        public WhereClause Add(LogicOperator logicalOperator, string field, Comparison @operator, object compareValue, int level, int index)
        {
            GeneralWhereClause NewWhereClause = new GeneralWhereClause(logicalOperator, field, @operator, compareValue, level);
            this.Insert(index, NewWhereClause);
            //this.AddWhereClauseToLevel(NewWhereClause, level);
            return NewWhereClause;
        }

        public WhereClause Add(LogicOperator logicalOperator, string field, Comparison @operator, object compareValue, int level)
        {
            GeneralWhereClause NewWhereClause = new GeneralWhereClause(logicalOperator, field, @operator, compareValue, level);
            this.Add(NewWhereClause);
            return NewWhereClause;
        }

        public WhereClause Add(LogicOperator logicalOperator, string field, string fromValue, string toValue, int level, int index)
        {
            BetweenWhereClause NewWhereClause = new BetweenWhereClause(logicalOperator, field, fromValue, toValue, level);
            this.Insert(index, NewWhereClause);
            //this.AddWhereClauseToLevel(NewWhereClause, level);
            return NewWhereClause;
        }

        private void AddWhereClause(WhereClause clause, int index)
        {
            this.Insert(index, clause);
        }

        private void AddWhereClause(WhereClause clause)
        {
            this.Add(clause);
        }

        public string BuildWhereStatement()
        {
            string Result = "";

            int PreviousLevel = 0;
            foreach (WhereClause Clause in this) // Loop through all conditions, AND them together
            {
                string WhereClause = "";

                if (Clause is GeneralWhereClause)
                {
                    GeneralWhereClause gClause = (GeneralWhereClause)Clause;
                    WhereClause = CreateComparisonClause(gClause.LogicalOperator, SelectQueryBuilder.getColumnPartQuery(gClause.FieldName), gClause.ComparisonOperator, gClause.Value, gClause.Level, PreviousLevel);
                }
                else if (Clause is BetweenWhereClause)
                {
                    BetweenWhereClause bClause = (BetweenWhereClause)Clause;
                    WhereClause = CreateBetweenClause(bClause.LogicalOperator, SelectQueryBuilder.getColumnPartQuery(bClause.FieldName), bClause.FromValue, bClause.ToValue, bClause.Level, PreviousLevel);
                }

                Result += WhereClause + " ";
                PreviousLevel = Clause.Level;
            }

            String RemainingParenthesis = "";
            while (PreviousLevel > 0)
            {
                RemainingParenthesis += ")";
                PreviousLevel--;
            }
            Result += RemainingParenthesis;
            return Result;
        }

        public static String GetLogicalOperatorString(LogicOperator logicalOperator)
        {
            String LogicalOperatrString = "";
            switch (logicalOperator)
            {
                case LogicOperator.And:
                    LogicalOperatrString = " AND "; break;
                case LogicOperator.Or:
                    LogicalOperatrString = " OR "; break;
                case LogicOperator.None:
                    break;
            }

            return LogicalOperatrString;
        }

        internal static string CreateComparisonClause(string fieldName, Comparison comparisonOperator, object value)
        {
            return CreateComparisonClause(LogicOperator.None, fieldName, comparisonOperator, value, 0, 0);
        }
        
        internal static string CreateComparisonClause(LogicOperator logicalOperator, string fieldName, Comparison comparisonOperator, object value, int CurrentLevel, int PreviousLevel)
        {
            string Output = "";

            if (PreviousLevel > CurrentLevel)
            {
                for (int i = 0; i < (PreviousLevel - CurrentLevel); i++)
                {
                    Output += ")";
                }

                Output += GetLogicalOperatorString(logicalOperator);
            }
            else
            {
                if (PreviousLevel < CurrentLevel)
                {
                    Output += GetLogicalOperatorString(logicalOperator);
                    for (int i = 0; i < (CurrentLevel - PreviousLevel); i++)
                    {
                        Output += "(";
                    }
                }
                else
                {
                    Output += GetLogicalOperatorString(logicalOperator);
                }
            }

            if (value != null && value != System.DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case Comparison.Equals:
                        Output += fieldName + " = " + FormatSQLValue(value); break;
                    case Comparison.NotEquals:
                        Output += fieldName + " <> " + FormatSQLValue(value); break;
                    case Comparison.GreaterThan:
                        Output += fieldName + " > " + FormatSQLValue(value); break;
                    case Comparison.GreaterOrEquals:
                        Output += fieldName + " >= " + FormatSQLValue(value); break;
                    case Comparison.LessThan:
                        Output += fieldName + " < " + FormatSQLValue(value); break;
                    case Comparison.LessOrEquals:
                        Output += fieldName + " <= " + FormatSQLValue(value); break;
                    case Comparison.Like:
                        Output += fieldName + " LIKE " + FormatSQLValue(value); break;
                    case Comparison.NotLike:
                        Output += "NOT " + fieldName + " LIKE " + FormatSQLValue(value); break;
                    case Comparison.In:
                        Output += fieldName + " IN (" + value + ")"; break;
                    case Comparison.NotIn:
                        Output += fieldName + " NOT IN (" + value + ")"; break;
                }
            }
            else // value==null	|| value==DBNull.Value
            {
                if ((comparisonOperator != Comparison.Equals) && (comparisonOperator != Comparison.NotEquals))
                {
                    throw new Exception("Cannot use comparison operator " + comparisonOperator.ToString() + " for NULL values.");
                }
                else
                {
                    switch (comparisonOperator)
                    {
                        case Comparison.Equals:
                            Output += fieldName + " IS NULL"; break;
                        case Comparison.NotEquals:
                            Output += "NOT " + fieldName + " IS NULL"; break;
                    }
                }
            }
            return Output;
        }

        internal static string CreateBetweenClause(LogicOperator logicalOperator, string fieldName, string fromValue, string toValue, int CurrentLevel, int PreviousLevel)
        {
            string Output = "";

            if (PreviousLevel > CurrentLevel)
            {
                for (int i = 0; i < (PreviousLevel - CurrentLevel); i++)
                {
                    Output += ")";
                }

                Output += GetLogicalOperatorString(logicalOperator);
            }
            else
            {
                if (PreviousLevel < CurrentLevel)
                {
                    Output += GetLogicalOperatorString(logicalOperator);
                    for (int i = 0; i < (CurrentLevel - PreviousLevel); i++)
                    {
                        Output += "(";
                    }
                }
                else
                {
                    Output += GetLogicalOperatorString(logicalOperator);
                }
            }

            Output += fieldName + " between \"" + fromValue + "\" and \"" + toValue + "\"";
            return Output;
        }

        internal static string FormatSQLValue(object someValue)
        {
            string FormattedValue = "";
            //				string StringType = Type.GetType("string").Name;
            //				string DateTimeType = Type.GetType("DateTime").Name;

            if (someValue == null)
            {
                FormattedValue = "NULL";
            }
            else
            {
                FormattedValue = someValue.ToString();
                //switch (someValue.GetType().Name)
                //{
                //    case "String": FormattedValue = "'" + ((string)someValue).Replace("'", "''") + "'"; break;
                //    case "DateTime": FormattedValue = "'" + ((DateTime)someValue).ToString("yyyy/MM/dd hh:mm:ss") + "'"; break;
                //    case "DBNull": FormattedValue = "NULL"; break;
                //    case "Boolean": FormattedValue = (bool)someValue ? "1" : "0"; break;
                //    case "SqlLiteral": FormattedValue = ((SqlLiteral)someValue).Value; break;
                //    case "SelectQueryBuilder": FormattedValue = ((SelectQueryBuilder)someValue).BuildQuery(); break;
                //    default: FormattedValue = someValue.ToString(); break;
                //}
            }
            return FormattedValue;
        }

        /// <summary>
        /// This static method combines 2 where statements with eachother to form a new statement
        /// </summary>
        /// <param name="statement1"></param>
        /// <param name="statement2"></param>
        /// <returns></returns>
        //public static WhereStatement CombineStatements(WhereStatement statement1, WhereStatement statement2)
        //{
        //    // statement1: {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%') OR {Level2}(Age BETWEEN 15 AND 20))
        //    // Statement2: {Level1}((Name = 'Peter'))
        //    // Return statement: {Level1}((Age<15 or Age>=20) AND (strEmail like 'e%') AND (Name = 'Peter'))

        //    // Make a copy of statement1
        //    WhereStatement result = WhereStatement.Copy(statement1);

        //    // Add all clauses of statement2 to result
        //    for (int i = 0; i < statement2.ClauseLevels; i++) // for each clause level in statement2
        //    {
        //        List<WhereClause> level = statement2[i];
        //        foreach (WhereClause clause in level) // for each clause in level i
        //        {
        //            for (int j = 0; j < result.ClauseLevels; j++)  // for each level in result, add the clause
        //            {
        //                result.AddWhereClauseToLevel(clause, j);
        //            }
        //        }
        //    }

        //    return result;
        //}

        public static WhereStatement Copy(WhereStatement statement)
        {
            WhereStatement result = new WhereStatement();
            foreach (WhereClause clause in statement)
            {
                WhereClause clauseCopy = null; 
                if (clause is GeneralWhereClause)
                {
                    GeneralWhereClause gClause = (GeneralWhereClause)clause;
                    clauseCopy = new GeneralWhereClause(gClause.LogicalOperator, gClause.FieldName, gClause.ComparisonOperator, gClause.Value, gClause.Level);
                }
                else if (clause is BetweenWhereClause)
                {
                    BetweenWhereClause bClause = (BetweenWhereClause)clause;
                    clauseCopy = new BetweenWhereClause(bClause.LogicalOperator, bClause.FieldName, bClause.FromValue, bClause.ToValue, bClause.Level);
                }
                //foreach (WhereClause.SubClause subClause in clause.SubClauses)
                //{
                //    WhereClause.SubClause subClauseCopy = new WhereClause.SubClause(subClause.LogicOperator, subClause.ComparisonOperator, subClause.Value);
                //    clauseCopy.SubClauses.Add(subClauseCopy);
                //}
                result.Add(clauseCopy);
            }
            return result;
        }

    }
}
