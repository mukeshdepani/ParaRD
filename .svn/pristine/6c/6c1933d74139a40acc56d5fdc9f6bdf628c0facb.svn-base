using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SQLBuilder.Clauses
{

    [Serializable()]
    [XmlInclude(typeof(DerivedTable))]
    public class Table
    {
        private string m_SchemaName;
        private string m_Name;
        private string m_AliasName;

        public Table()
        {
        }

        public Table(MySQLData.Table table, string aliasName)
        {
            SchemaName = table.schemaName;
            Name = table.name;
            AliasName = aliasName;
        }

        public string SchemaName
        {
            get { return m_SchemaName; }
            set { m_SchemaName = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string AliasName
        {
            get { return m_AliasName; }
            set { m_AliasName = value; }
        }
    }
}
