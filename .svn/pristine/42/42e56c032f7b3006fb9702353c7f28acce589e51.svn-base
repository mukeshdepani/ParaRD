using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Common;

namespace SQLBuilder.Clauses
{
    [Serializable()]
    public class ComputedColumn : Column
    {
        public static int EXPRESSION = 1;
        public static int FUNCTION = 2;

        private int m_Type;
        private List<Parameter> m_parameters = new List<Parameter>();

        public ComputedColumn()
        {
            base.DataType = "varchar";
        }

        public ComputedColumn(string name, string alias_name, string format, string type)
            : base(name, alias_name, format, "varchar")
        {
        }

        public ComputedColumn(string name, string alias_name, string format, string type, string dataType)
            : base(name, alias_name, format, dataType)
        {
        }

        public override string Name
        {
            get 
            {
                if (m_Type == FUNCTION && base.Name.IndexOf('(') < 0)
                {
                    int paramIndex = 0;
                    string functionName = base.Name;
                    Function function = Functions.getFunction(functionName);
                    string computedColExpr;
                    if (functionName == "CASE")
                    {
                        computedColExpr = "(" + functionName + " ";
                        foreach (Parameter param in m_parameters)
                        {
                            paramIndex++;
                            string value = param.Value;
                            computedColExpr += value;
                            if (paramIndex < function.NumParameters)
                            {
                                computedColExpr += " ";
                            }
                        }
                        computedColExpr += " ";
                    }
                    else
                    {
                        computedColExpr = functionName + "(";
                        foreach (Parameter param in m_parameters)
                        {
                            paramIndex++;
                            string value = param.Value;
                            computedColExpr += value;
                            if (paramIndex < function.NumParameters)
                            {
                                computedColExpr += ", ";
                            }
                        }
                        computedColExpr += ")";
                    }
                    
                    

                    return computedColExpr; 
                }
                else
                {
                    return base.Name;
                }
            }
            set { base.Name = value; }
        }

        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public List<Parameter> Parameters
        {
            get { return m_parameters; }
            set { m_parameters = value; }
        }

        public void addParameter(Parameter param)
        {
            m_parameters.Add(param);
        }

        public Parameter getParameter(int index)
        {
            return m_parameters[index];
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is ComputedColumn)
            {
                ComputedColumn sc2 = (ComputedColumn)obj;
                return Name.CompareTo(sc2.Name);
            }
            else
                throw new ArgumentException("Object is not a ComputedColumn");
        }
        public bool Equals(ComputedColumn other)
        {

            return Name.Equals(other.Name);

        }

        #endregion
    }
}
