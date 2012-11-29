using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLData
{
    public class DerivedTable : Table
    {
        private string query;

        public DerivedTable(string name, string schemaName, string query) : base(name, schemaName)
        {
            this.query = query;
        }

        public string Query
        {
            get { return query; }
            set { query = value; }
        }
    }
}
