﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.Office.Interop.Visio;
using Microsoft.Office.Tools;
using Microsoft.VisualStudio.Tools.Applications.Runtime;

#pragma warning disable 414
namespace ExtendedVisioAddin1
{


    /// 
    [StartupObject(0)]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public sealed partial class ThisAddIn : AddInBase
    {

        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        private Object missing = Type.Missing;

        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        internal Application Application;

        /// 
        [DebuggerNonUserCode()]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ThisAddIn(Factory factory, IServiceProvider serviceProvider) :
            base(factory, serviceProvider, "AddIn", "ThisAddIn")
        {
            Globals.Factory = factory;
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Initialize()
        {
            base.Initialize();
            this.Application = this.GetHostItem<Application>(typeof(Application), "Application");
            Globals.ThisAddIn = this;
            System.Windows.Forms.Application.EnableVisualStyles();
            this.InitializeCachedData();
            this.InitializeControls();
            this.InitializeComponents();
            this.InitializeData();
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void FinishInitialization()
        {
            this.InternalStartup();
            this.OnStartup();
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitializeDataBindings()
        {
            this.BeginInitialization();
            this.BindToData();
            this.EndInitialization();
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void InitializeCachedData()
        {
            if ((this.DataHost == null))
            {
                return;
            }
            if (this.DataHost.IsCacheInitialized)
            {
                this.DataHost.FillCachedData(this);
            }
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void InitializeData()
        {
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void BindToData()
        {
        }

        /// 
        [DebuggerNonUserCode()]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        private void StartCaching(string MemberName)
        {
            this.DataHost.StartCaching(this, MemberName);
        }

        /// 
        [DebuggerNonUserCode()]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        private void StopCaching(string MemberName)
        {
            this.DataHost.StopCaching(this, MemberName);
        }

        /// 
        [DebuggerNonUserCode()]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        private bool IsCached(string MemberName)
        {
            return this.DataHost.IsCached(this, MemberName);
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void BeginInitialization()
        {
            this.BeginInit();
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void EndInitialization()
        {
            this.EndInit();
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void InitializeControls()
        {
        }

        /// 
        [DebuggerNonUserCode()]
        [GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void InitializeComponents()
        {
        }

        /// 
        [DebuggerNonUserCode()]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        private bool NeedsFill(string MemberName)
        {
            return this.DataHost.NeedsFill(this, MemberName);
        }
    }

    /// 
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
    internal sealed partial class Globals
    {

        /// 
        private Globals()
        {
        }

        private static ThisAddIn _ThisAddIn;

        private static global::Microsoft.Office.Tools.Factory _factory;

        private static ThisRibbonCollection _ThisRibbonCollection;

        internal static ThisAddIn ThisAddIn
        {
            get
            {
                return _ThisAddIn;
            }
            set
            {
                if ((_ThisAddIn == null))
                {
                    _ThisAddIn = value;
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
        }

        internal static global::Microsoft.Office.Tools.Factory Factory
        {
            get
            {
                return _factory;
            }
            set
            {
                if ((_factory == null))
                {
                    _factory = value;
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
        }

        internal static ThisRibbonCollection Ribbons
        {
            get
            {
                if ((_ThisRibbonCollection == null))
                {
                    _ThisRibbonCollection = new ThisRibbonCollection(_factory.GetRibbonFactory());
                }
                return _ThisRibbonCollection;
            }
        }
    }

    /// 
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "12.0.0.0")]
    internal sealed partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonCollectionBase
    {

        /// 
        internal ThisRibbonCollection(global::Microsoft.Office.Tools.Ribbon.RibbonFactory factory) :
            base(factory)
        {
        }
    }
}
