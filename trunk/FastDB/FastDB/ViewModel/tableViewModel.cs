using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MySQLData;
namespace FastDB
{
    public class tableViewModel : TreeViewItemViewModel, INotifyPropertyChanged

    {
        
        private string _mySelectedItem;
        public readonly string _ParentSchemaName;

        public readonly Table _table;

        public tableViewModel(Table Table, schemaViewModel parentSchema)
            : base(parentSchema, true)
        {
            _table = Table;
            _ParentSchemaName = parentSchema.schemaName;
           
        }

        public string tableName
        {
            get { return _table.name; }
        }


        protected override void LoadChildren()
        {
            foreach (Column c in _table.columns)
                base.Children.Add(new columnViewModel(c, this));
        }
        
    }
}
