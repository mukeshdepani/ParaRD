using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLData;
namespace FastDB
{
    public class columnViewModel : TreeViewItemViewModel
    {
        readonly Column _column;

        public columnViewModel(Column Column, tableViewModel parentTable)
            : base(parentTable, false)
        {
            _column = Column;
        }

        public string columnName
        {
            get { return _column.name; }
        }
    }
}
