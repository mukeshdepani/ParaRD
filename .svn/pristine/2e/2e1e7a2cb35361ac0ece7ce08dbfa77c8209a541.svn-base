using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLData;
namespace FastDB
{
    public class schemaViewModel: TreeViewItemViewModel
    {
        readonly Schema _schema;

        public schemaViewModel(Schema Schema)
            : base(null, true)
        {
            _schema = Schema;
        }

        public string schemaName
        {
            get { return _schema.name; }
        }

        protected override void LoadChildren()
        {
            foreach (Table tb in _schema.tables)
                base.Children.Add(new tableViewModel(tb, this));

        }
    }
}
