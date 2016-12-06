namespace Rationally.Visio
{
    partial class RationallyRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.settingsButton = this.Factory.CreateRibbonButton();
            this.rationally_tab.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rationally_tab
            // 
            this.rationally_tab.Groups.Add(this.group1);
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
            this.wizardButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.wizardButton_Click_1);
            // 
            // settingsButton
            // 
            this.settingsButton.Image = global::Rationally.Visio.Properties.Resources.wizard;
            this.settingsButton.Label = "rationally Settings";
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.ShowImage = true;
            this.settingsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.settingsButton_Click);
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
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab rationally_tab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton wizardButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton settingsButton;
    }

    partial class ThisRibbonCollection
    {
        internal RationallyRibbon RationallyRibbon
        {
            get { return this.GetRibbon<RationallyRibbon>(); }
        }
    }
}
