﻿#pragma checksum "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "37299CF569E050A0A3B69E6D81001923"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SQLBuilder.Clauses;
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
    /// CrossTabulationTabStackPanelSummaryControl
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class CrossTabulationTabStackPanelSummaryControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCrossTabulationTabSummaryColumnsName;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCrossTabulationTypeOfSummary;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCrossTabulationTabUserSelectSummaryColFormat;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCrossTabulationTabSummaryAlias;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCrossTabSummaryDelete;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCrossTabulationTabSummaryColFormat;
        
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
            System.Uri resourceLocater = new System.Uri("/FastDB;component/control/crosstabulationtabstackpanelsummarycontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
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
            this.cmbCrossTabulationTabSummaryColumnsName = ((System.Windows.Controls.ComboBox)(target));
            
            #line 14 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
            this.cmbCrossTabulationTabSummaryColumnsName.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbCrossTabulationTabSummaryColumnsName_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cmbCrossTabulationTypeOfSummary = ((System.Windows.Controls.ComboBox)(target));
            
            #line 21 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
            this.cmbCrossTabulationTypeOfSummary.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbCrossTabulationTypeOfSummary_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmbCrossTabulationTabUserSelectSummaryColFormat = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
            this.cmbCrossTabulationTabUserSelectSummaryColFormat.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbCrossTabulationTabUserSelectSummaryColFormat_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtCrossTabulationTabSummaryAlias = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\..\..\Control\CrossTabulationTabStackPanelSummaryControl.xaml"
            this.txtCrossTabulationTabSummaryAlias.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtCrossTabulationTabSummaryAlias_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnCrossTabSummaryDelete = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.txtCrossTabulationTabSummaryColFormat = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

