using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data;
using System.Configuration;
using System;
using System.Collections.Generic;
namespace FastDB
{
    public class WhereTabColumns : INotifyPropertyChanged
    {
       
        private List<string>  _ListOfColumns = new List<string>();

        public List<String> ListOfColumns
        {
            get { return _ListOfColumns;  }


            set { _ListOfColumns = value; NotifyPropertyChanged("ListOfColumns"); }

        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Notifies subscribers of changed properties.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        /// 
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
