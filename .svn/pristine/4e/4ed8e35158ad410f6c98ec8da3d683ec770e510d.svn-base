using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SQLBuilder.Clauses
{
    [Serializable()]
    public class DerivedTable : Table
    {
        private string query;

        public DerivedTable()
        {
        }

        public DerivedTable(MySQLData.DerivedTable table, string aliasName)
        {
            base.SchemaName = table.schemaName;
            base.Name = table.name;
            base.AliasName = aliasName;
            Query = table.Query;
        }

        public string Query
        {
            get { return query; }
            set { query = value; }
        }

    }
}
