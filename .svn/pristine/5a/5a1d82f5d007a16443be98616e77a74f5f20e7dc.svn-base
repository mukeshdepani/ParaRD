using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MySQLData;
namespace FastDB
{
    public class MainViewModel
    {
        readonly ReadOnlyCollection<schemaViewModel> _schemas;

        public MainViewModel(List<Schema> schemas)
        {
            _schemas = new ReadOnlyCollection<schemaViewModel>(
                (from Schema in schemas
                 select new schemaViewModel(Schema))
                .ToList());
        }


        public ReadOnlyCollection<schemaViewModel> schemas
        {
            get { return _schemas; }
        }

    }
}
