using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastDB.Class
{
    public class SelectTabColumn : IComparable, IEquatable<SelectTabColumn>
    {
        private string _name;
        private bool _isChecked;
        private string _alias;
        private string m_format;
        
        public SelectTabColumn(bool isChecked,string name, string alias)
        {
            this._isChecked = isChecked;
            this._name = name;
            this._alias = alias;
        }
        
        public string name 
        {
        get { return _name; }
        set { _name = value; }
        }
        
        public string alias 
        {
        get { return _alias; }
        set { _alias = value; }
        }
        
        public string format
        {
            get { return m_format; }
            set { m_format = value; }
        }
        
        #region IComparable Members
    public int CompareTo(object obj)
    {
        if (obj is SelectTabColumn)
        {
            SelectTabColumn sc2 = (SelectTabColumn)obj;
            return _name.CompareTo(sc2.name);
        }
        else
            throw new ArgumentException("Object is not a SelectTabColumn");
    }
    public bool Equals(SelectTabColumn other)
    {

        return _name.Equals(other.name);
        
    }

    #endregion
    }
}
