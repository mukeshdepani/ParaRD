using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLBuilder.Clauses
{
    [Serializable()]
    public class Parameter
    {
        public static int STRING = 1;
        public static int INT = 2;
        public static int BOOLEAN = 3;
        public static int DOUBLE = 4;
        public static int DATE = 5;

        private string m_Name;
        private string m_Value;
        private int m_Type;

        public Parameter() 
        {
        }

        public Parameter(string name, int type) 
        {
            m_Name = name;
            m_Type = type;
        }

        public Parameter(string value)
        {
            m_Value = value;
        }

        public Parameter(string name, string value, int type)
        {
            m_Name = name;
            m_Value = value;
            m_Type = type;
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }


    }
}
