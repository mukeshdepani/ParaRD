﻿#pragma checksum "..\..\..\ScriptGenerator.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A88240665C069648F41B2791C4B990A4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace FastDB {
    
    
    /// <summary>
    /// ScriptGenerator
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class ScriptGenerator : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFileName;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowse;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgScriptData;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colSrNo;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colColumnName;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colDataType;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colSize;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\ScriptGenerator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddRow;
        
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
            System.Uri resourceLocater = new System.Uri("/FastDB;component/scriptgenerator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ScriptGenerator.xaml"
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
            this.txtFileName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.btnBrowse = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\ScriptGenerator.xaml"
            this.btnBrowse.Click += new System.Windows.RoutedEventHandler(this.btnBrowse_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\ScriptGenerator.xaml"
            this.btnSubmit.Click += new System.Windows.RoutedEventHandler(this.btnSubmit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dgScriptData = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 5:
            this.colSrNo = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 6:
            this.colColumnName = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 7:
            this.colDataType = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 8:
            this.colSize = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 9:
            this.btnAddRow = ((System.Windows.Controls.Button)(target));
            
            #line 134 "..\..\..\ScriptGenerator.xaml"
            this.btnAddRow.Click += new System.Windows.RoutedEventHandler(this.btnAddRow_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
