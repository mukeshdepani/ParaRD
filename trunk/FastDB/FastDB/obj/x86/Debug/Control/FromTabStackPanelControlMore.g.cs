﻿#pragma checksum "..\..\..\..\Control\FromTabStackPanelControlMore.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CCF068245F74B92A3F3D5145501AFC74"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SQLBuilder.Enums;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FastDB.Control {
    
    
    /// <summary>
    /// FromTabStackPanelControlMore
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class FromTabStackPanelControlMore : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbFromTabFromANDOR;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbFromTabFromColumns;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbFromTabQueryOpretor;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbFromTabJoinColumns;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btndeletemore;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FastDB;component/control/fromtabstackpanelcontrolmore.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbFromTabFromANDOR = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.cmbFromTabFromColumns = ((System.Windows.Controls.ComboBox)(target));
            
            #line 16 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
            this.cmbFromTabFromColumns.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbFromTabFromColumns_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmbFromTabQueryOpretor = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.cmbFromTabJoinColumns = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
            this.cmbFromTabJoinColumns.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbFromTabFromColumns_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btndeletemore = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\..\Control\FromTabStackPanelControlMore.xaml"
            this.btndeletemore.Click += new System.Windows.RoutedEventHandler(this.btnDelete_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

