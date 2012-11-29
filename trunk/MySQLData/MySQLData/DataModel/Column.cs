using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MySQLData
{
    [Serializable()]
    public class Column 
    {
        public Column(string name, string type)
        {
            this.name = name;
            this.type = type;
            this.format = null;
        }

        public Column(string name, string type, string format)
        {
            this.name = name;
            this.type = type;
            this.format = format;
        }

        public string name { get; private set; }
        public string type { get; private set; }
        public string format { get; private set; }
    }
}
