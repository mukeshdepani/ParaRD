using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;

namespace SQLBuilder.Clauses
{
    [Serializable()]
    [XmlInclude(typeof(ComputedColumn))]
    public class Column : IComparable, IEquatable<Column>
    {
        private string m_Name;
        private string m_AliasName;
        private string m_Format;
        private string m_DataType;

        public Column()
        {
        }

        public Column(string name, string alias_name, string format, string dataType)
        {
            Name = name;
            AliasName = alias_name;
            Format = format;
            DataType = dataType;
        }

        public virtual string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string AliasName
        {
            get 
            {
                if (m_AliasName == null || "".Equals(m_AliasName.Trim()))
                {
                    int dotIndex = m_Name.IndexOf('.');
                    int parenIndex = m_Name.IndexOf('(');
                    if (parenIndex < 0 && dotIndex >= 0)
                    {
                        m_AliasName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m_Name.Substring(dotIndex + 1).Replace('_', ' '));
                    }
                    else
                    {
                        m_AliasName = m_Name;
                    }
                }
                
                return m_AliasName;
            }
            set { m_AliasName = value; }
        }

        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        public string DataType
        {
            get { return m_DataType; }
            set { m_DataType = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is Column)
            {
                Column sc2 = (Column)obj;
                return Name.CompareTo(sc2.Name);
            }
            else
                throw new ArgumentException("Object is not a SelectTabColumn");
        }
        public bool Equals(Column other)
        {

            return Name.Equals(other.Name);

        }

        #endregion
    }
}
