using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastDB.Class
{
    public class FromTabClass
    {
        public int MyProperty { get; set; }
        private List<string> _FromTableColumns = new List<string>();

        public List<string> FromTableColumns
        {
            get { return _FromTableColumns; }
            set { _FromTableColumns = value; }
        }
        
        private List<string> GetFromTabColums()
        {
            List<string> list = new List<string>();
            list.Add("ABC");
            list.Add("xyz");
            return list;
        }
    }
}
