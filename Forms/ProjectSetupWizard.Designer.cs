using System.ComponentModel;
using System.Windows.Forms;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.Forms
{
    partial class ProjectSetupWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSetupWizard));
            this.tableLayoutForEverything = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutLeftColumn = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutLeftLogo = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelBottomLeftText = new System.Windows.Forms.Label();
            this.UpdateLink = new System.Windows.Forms.LinkLabel();
            this.tableLayoutRightColumn = new System.Windows.Forms.TableLayoutPanel();
            this.FlowLayoutBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutLeftMenu = new Rationally.Visio.Forms.WizardComponents.MenuPanel();
            this.tableLayoutMainContentGeneral = new Rationally.Visio.Forms.WizardComponents.TableLayoutMainContentGeneral();
            this.CreateButton = new Rationally.Visio.Forms.WizardComponents.AntiAliasedButton();
            this.tableLayoutForEverything.SuspendLayout();
            this.tableLayoutLeftColumn.SuspendLayout();
            this.tableLayoutLeftLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutRightColumn.SuspendLayout();
            this.FlowLayoutBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutForEverything
            // 
            resources.ApplyResources(this.tableLayoutForEverything, "tableLayoutForEverything");
            this.tableLayoutForEverything.Controls.Add(this.tableLayoutLeftColumn, 0, 0);
            this.tableLayoutForEverything.Controls.Add(this.tableLayoutRightColumn, 1, 0);
            this.tableLayoutForEverything.Name = "tableLayoutForEverything";
            // 
            // tableLayoutLeftColumn
            // 
            this.tableLayoutLeftColumn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            resources.ApplyResources(this.tableLayoutLeftColumn, "tableLayoutLeftColumn");
            this.tableLayoutLeftColumn.Controls.Add(this.tableLayoutLeftLogo, 0, 1);
            this.tableLayoutLeftColumn.Controls.Add(this.tableLayoutLeftMenu, 0, 0);
            this.tableLayoutLeftColumn.Name = "tableLayoutLeftColumn";
            // 
            // tableLayoutLeftLogo
            // 
            resources.ApplyResources(this.tableLayoutLeftLogo, "tableLayoutLeftLogo");
            this.tableLayoutLeftLogo.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutLeftLogo.Controls.Add(this.labelBottomLeftText, 0, 1);
            this.tableLayoutLeftLogo.Controls.Add(this.UpdateLink, 0, 2);
            this.tableLayoutLeftLogo.Name = "tableLayoutLeftLogo";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // labelBottomLeftText
            // 
            resources.ApplyResources(this.labelBottomLeftText, "labelBottomLeftText");
            this.labelBottomLeftText.Name = "labelBottomLeftText";
            // 
            // UpdateLink
            // 
            resources.ApplyResources(this.UpdateLink, "UpdateLink");
            this.UpdateLink.Name = "UpdateLink";
            this.UpdateLink.TabStop = true;
            this.UpdateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateLink_LinkClicked);
            // 
            // tableLayoutRightColumn
            // 
            this.tableLayoutRightColumn.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tableLayoutRightColumn, "tableLayoutRightColumn");
            this.tableLayoutRightColumn.Controls.Add(this.tableLayoutMainContentGeneral, 0, 0);
            this.tableLayoutRightColumn.Controls.Add(this.FlowLayoutBottomButtons, 0, 1);
            this.tableLayoutRightColumn.Name = "tableLayoutRightColumn";
            // 
            // FlowLayoutBottomButtons
            // 
            this.FlowLayoutBottomButtons.BackColor = System.Drawing.Color.WhiteSmoke;
            this.FlowLayoutBottomButtons.Controls.Add(this.CreateButton);
            resources.ApplyResources(this.FlowLayoutBottomButtons, "FlowLayoutBottomButtons");
            this.FlowLayoutBottomButtons.Name = "FlowLayoutBottomButtons";
            // 
            // tableLayoutLeftMenu
            // 
            resources.ApplyResources(this.tableLayoutLeftMenu, "tableLayoutLeftMenu");
            this.tableLayoutLeftMenu.Name = "tableLayoutLeftMenu";
            // 
            // tableLayoutMainContentGeneral
            // 
            this.tableLayoutMainContentGeneral.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tableLayoutMainContentGeneral, "tableLayoutMainContentGeneral");
            this.tableLayoutMainContentGeneral.Name = "tableLayoutMainContentGeneral";
            // 
            // CreateButton
            // 
            resources.ApplyResources(this.CreateButton, "CreateButton");
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.submit_Click);
            // 
            // ProjectSetupWizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutForEverything);
            this.Name = "ProjectSetupWizard";
            this.Load += new System.EventHandler(this.ProjectSetupWizard_Activated);
            this.tableLayoutForEverything.ResumeLayout(false);
            this.tableLayoutLeftColumn.ResumeLayout(false);
            this.tableLayoutLeftLogo.ResumeLayout(false);
            this.tableLayoutLeftLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutRightColumn.ResumeLayout(false);
            this.FlowLayoutBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion


        private TableLayoutPanel tableLayoutForEverything;
        private TableLayoutPanel tableLayoutLeftColumn;
        public TableLayoutPanel tableLayoutRightColumn;
        public TableLayoutMainContentGeneral tableLayoutMainContentGeneral;//here
        public TableLayoutMainContentAlternatives TableLayoutMainContentAlternatives = new TableLayoutMainContentAlternatives();
        public TableLayoutMainContentForces TableLayoutMainContentForces = new TableLayoutMainContentForces();
        public TableLayoutMainContentDocuments TableLayoutMainContentDocuments = new TableLayoutMainContentDocuments();
        public TableLayoutMainContentStakeholders TableLayoutMainContentStakeholders = new TableLayoutMainContentStakeholders();
        public TableLayoutMainContentPlanningItems TableLayoutMainContentPlanningItems = new TableLayoutMainContentPlanningItems();
        public FlowLayoutPanel FlowLayoutBottomButtons;
        
        
       
        
        private TableLayoutPanel tableLayoutLeftLogo;
        private PictureBox pictureBox1;
        private Label labelBottomLeftText;
        private MenuPanel tableLayoutLeftMenu;
        public AntiAliasedButton CreateButton;
        private LinkLabel UpdateLink;    
        
    }
}