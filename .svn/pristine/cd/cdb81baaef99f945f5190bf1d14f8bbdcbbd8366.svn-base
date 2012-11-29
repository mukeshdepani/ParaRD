using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FastDB.Control
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        
        DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = VisualTreeHelper.GetParent(tmp)) != null)
            {
                parent = tmp;
            }
            return parent;
        }
        
        private void RVCTAbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

}
