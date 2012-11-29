using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MySQLData
{
    [Serializable()]
    public class Table 
    {
        public Table(string name, string schemaName)
        {
            this.name = name;
            this.schemaName = schemaName;
        }

        public string name { get; private set; }

        public string schemaName { get; private set; }

        public List<Column> columns
        {
            get;
            set;
        }
    }
}
