using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MySQLData
{
    [Serializable()]
    public class Schema
    {
        public Schema(string name)
        {
            this.name = name;
        }

        public string name { get; private set; }

        public List<Table> tables
        {
            get;
            set;
        }
    }
}
