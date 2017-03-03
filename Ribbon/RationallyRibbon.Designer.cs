using System.ComponentModel;
using Microsoft.Office.Tools.Ribbon;

namespace Rationally.Visio
{
    partial class RationallyRibbon : RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        public RationallyRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RationallyRibbon));
            this.rationally_tab = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.wizardButton = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.alternativeStatesOptionsButton = this.Factory.CreateRibbonButton();
            this.settingsButton = this.Factory.CreateRibbonButton();
            this.Server = this.Factory.CreateRibbonGroup();
            this.saveToServerButton = this.Factory.CreateRibbonButton();
            this.rationally_tab.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.Server.SuspendLayout();
            this.SuspendLayout();
            // 
            // rationally_tab
            // 
            this.rationally_tab.Groups.Add(this.group1);
            this.rationally_tab.Groups.Add(this.group2);
            this.rationally_tab.Groups.Add(this.Server);
            this.rationally_tab.Label = "rationally";
            this.rationally_tab.Name = "rationally_tab";
            // 
            // group1
            // 
            this.group1.Items.Add(this.wizardButton);
            this.group1.Label = "Actions";
            this.group1.Name = "group1";
            // 
            // wizardButton
            // 
            this.wizardButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.wizardButton.Image = ((System.Drawing.Image)(resources.GetObject("wizardButton.Image")));
            this.wizardButton.Label = "Configure View";
            this.wizardButton.Name = "wizardButton";
            this.wizardButton.ShowImage = true;
            // 
            // group2
            // 
            this.group2.Items.Add(this.alternativeStatesOptionsButton);
            this.group2.Label = "Options";
            this.group2.Name = "group2";
            // 
            // alternativeStatesOptionsButton
            // 
            this.alternativeStatesOptionsButton.Label = "Alternative States";
            this.alternativeStatesOptionsButton.Name = "alternativeStatesOptionsButton";
            this.alternativeStatesOptionsButton.OfficeImageId = "ViewBackToColorView";
            this.alternativeStatesOptionsButton.ShowImage = true;
            this.alternativeStatesOptionsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.alternativeStatesOptionsButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Enabled = false;
            this.settingsButton.Image = global::Rationally.Visio.Properties.Resources.wizard;
            this.settingsButton.Label = "rationally Settings";
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.ShowImage = true;
            this.settingsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.settingsButton_Click);
            // 
            // Server
            // 
            this.Server.Items.Add(this.saveToServerButton);
            this.Server.Label = "Server";
            this.Server.Name = "Server";
            // 
            // saveToServerButton
            // 
            this.saveToServerButton.Label = "Save";
            this.saveToServerButton.Name = "saveToServerButton";
            this.saveToServerButton.OfficeImageId = "DatabaseMoveToSharePoint";
            this.saveToServerButton.ShowImage = true;
            this.saveToServerButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SaveToServerButton_click);
            // 
            // RationallyRibbon
            // 
            this.Name = "RationallyRibbon";
            // 
            // RationallyRibbon.OfficeMenu
            // 
            this.OfficeMenu.Items.Add(this.settingsButton);
            this.RibbonType = "Microsoft.Visio.Drawing";
            this.Tabs.Add(this.rationally_tab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RationallyRibbon_Load);
            this.rationally_tab.ResumeLayout(false);
            this.rationally_tab.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.Server.ResumeLayout(false);
            this.Server.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal RibbonTab rationally_tab;
        internal RibbonGroup group1;
        internal RibbonButton wizardButton;
        internal RibbonButton settingsButton;
        internal RibbonGroup group2;
        internal RibbonButton alternativeStatesOptionsButton;
        internal RibbonGroup Server;
        internal RibbonButton saveToServerButton;
    }

    partial class ThisRibbonCollection
    {
        internal RationallyRibbon RationallyRibbon
        {
            get { return this.GetRibbon<RationallyRibbon>(); }
        }
    }
}
