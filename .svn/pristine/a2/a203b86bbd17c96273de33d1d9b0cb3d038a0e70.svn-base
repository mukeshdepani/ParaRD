using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace FastDB
{
    public class CloseableTabItem : TabItem, INotifyPropertyChanged
    {
        private string _Part_Label_Content;
        public Label labelStar;
        public event PropertyChangedEventHandler PropertyChanged;        

        static CloseableTabItem()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabItem),
                new FrameworkPropertyMetadata(typeof(CloseableTabItem)));
        }

        public static readonly RoutedEvent CloseTabEvent =
            EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CloseableTabItem));

        public string Part_Label_Content
        {
            get { return _Part_Label_Content; }
            set { NotifyPropertyChanged("Part_Label_Content"); _Part_Label_Content = value; }
        }

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button closeButton = base.GetTemplateChild("PART_Close") as Button;            
            if (closeButton != null)
                closeButton.Click += new System.Windows.RoutedEventHandler(closeButton_Click);
            
            labelStar = base.GetTemplateChild("PART_Label") as Label;
            if (labelStar != null)  
                labelStar.Content = Part_Label_Content;
        }

        void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
       
        /// <summary>
        /// Notifies subscribers of changed properties.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
