using System;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.WindowsFormPopups.WizardComponents;

namespace Rationally.Visio.WindowsFormPopups
{
    partial class ProjectSetupWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.tableLayoutLeftMenu = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutRightColumn = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonShowAlternatives = new System.Windows.Forms.Button();
            this.tableLayoutMainContentGeneral = new Rationally.Visio.WindowsFormPopups.WizardComponents.TableLayoutMainContentGeneral();
            this.TableLayoutMainContentAlternatives = new Rationally.Visio.WindowsFormPopups.WizardComponents.TableLayoutMainContentAlternatives();
            this.tableLayoutForEverything.SuspendLayout();
            this.tableLayoutLeftColumn.SuspendLayout();
            this.tableLayoutLeftLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutLeftMenu.SuspendLayout();
            this.tableLayoutRightColumn.SuspendLayout();
            this.flowLayoutBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutForEverything
            // 
            this.tableLayoutForEverything.ColumnCount = 2;
            this.tableLayoutForEverything.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.5283F));
            this.tableLayoutForEverything.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.4717F));
            this.tableLayoutForEverything.Controls.Add(this.tableLayoutLeftColumn, 0, 0);
            this.tableLayoutForEverything.Controls.Add(this.tableLayoutRightColumn, 1, 0);
            this.tableLayoutForEverything.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutForEverything.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutForEverything.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutForEverything.Name = "tableLayoutForEverything";
            this.tableLayoutForEverything.RowCount = 1;
            this.tableLayoutForEverything.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutForEverything.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 548F));
            this.tableLayoutForEverything.Size = new System.Drawing.Size(1028, 548);
            this.tableLayoutForEverything.TabIndex = 0;
            // 
            // tableLayoutLeftColumn
            // 
            this.tableLayoutLeftColumn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tableLayoutLeftColumn.ColumnCount = 1;
            this.tableLayoutLeftColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutLeftColumn.Controls.Add(this.tableLayoutLeftLogo, 0, 1);
            this.tableLayoutLeftColumn.Controls.Add(this.tableLayoutLeftMenu, 0, 0);
            this.tableLayoutLeftColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLeftColumn.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutLeftColumn.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutLeftColumn.Name = "tableLayoutLeftColumn";
            this.tableLayoutLeftColumn.RowCount = 2;
            this.tableLayoutLeftColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutLeftColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tableLayoutLeftColumn.Size = new System.Drawing.Size(244, 540);
            this.tableLayoutLeftColumn.TabIndex = 0;
            // 
            // tableLayoutLeftLogo
            // 
            this.tableLayoutLeftLogo.ColumnCount = 1;
            this.tableLayoutLeftLogo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLeftLogo.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutLeftLogo.Controls.Add(this.labelBottomLeftText, 0, 1);
            this.tableLayoutLeftLogo.Controls.Add(this.UpdateLink, 0, 2);
            this.tableLayoutLeftLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLeftLogo.Location = new System.Drawing.Point(3, 288);
            this.tableLayoutLeftLogo.Name = "tableLayoutLeftLogo";
            this.tableLayoutLeftLogo.RowCount = 3;
            this.tableLayoutLeftLogo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.61603F));
            this.tableLayoutLeftLogo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.38397F));
            this.tableLayoutLeftLogo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutLeftLogo.Size = new System.Drawing.Size(238, 249);
            this.tableLayoutLeftLogo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(232, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelBottomLeftText
            // 
            this.labelBottomLeftText.AutoSize = true;
            this.labelBottomLeftText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBottomLeftText.Location = new System.Drawing.Point(6, 56);
            this.labelBottomLeftText.Margin = new System.Windows.Forms.Padding(6, 3, 3, 0);
            this.labelBottomLeftText.Name = "labelBottomLeftText";
            this.labelBottomLeftText.Size = new System.Drawing.Size(229, 170);
            this.labelBottomLeftText.TabIndex = 1;
            this.labelBottomLeftText.Text = "We will help you to organize and document your decisions. Just a few quick steps " +
    "to prepare the document for you and help you save some time.";
            // 
            // UpdateLink
            // 
            this.UpdateLink.AutoSize = true;
            this.UpdateLink.Location = new System.Drawing.Point(3, 226);
            this.UpdateLink.Name = "UpdateLink";
            this.UpdateLink.Size = new System.Drawing.Size(119, 19);
            this.UpdateLink.TabIndex = 2;
            this.UpdateLink.TabStop = true;
            this.UpdateLink.Text = "Update available";
            this.UpdateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateLink_LinkClicked);
            // 
            // tableLayoutLeftMenu
            // 
            this.tableLayoutLeftMenu.ColumnCount = 1;
            this.tableLayoutLeftMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutLeftMenu.Controls.Add(this.buttonShowAlternatives, 0, 1);
            this.tableLayoutLeftMenu.Controls.Add(this.button1, 0, 0);
            this.tableLayoutLeftMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLeftMenu.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutLeftMenu.Name = "tableLayoutLeftMenu";
            this.tableLayoutLeftMenu.RowCount = 3;
            this.tableLayoutLeftMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutLeftMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutLeftMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutLeftMenu.Size = new System.Drawing.Size(238, 279);
            this.tableLayoutLeftMenu.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(232, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "General Information";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutRightColumn
            // 
            this.tableLayoutRightColumn.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutRightColumn.ColumnCount = 1;
            this.tableLayoutRightColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRightColumn.Controls.Add(this.tableLayoutMainContentGeneral, 0, 0);
            this.tableLayoutRightColumn.Controls.Add(this.flowLayoutBottomButtons, 0, 1);
            this.tableLayoutRightColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutRightColumn.Location = new System.Drawing.Point(256, 4);
            this.tableLayoutRightColumn.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutRightColumn.Name = "tableLayoutRightColumn";
            this.tableLayoutRightColumn.RowCount = 2;
            this.tableLayoutRightColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRightColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutRightColumn.Size = new System.Drawing.Size(768, 540);
            this.tableLayoutRightColumn.TabIndex = 1;
            // 
            // flowLayoutBottomButtons
            // 
            this.flowLayoutBottomButtons.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutBottomButtons.Controls.Add(this.button2);
            this.flowLayoutBottomButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutBottomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutBottomButtons.Location = new System.Drawing.Point(4, 494);
            this.flowLayoutBottomButtons.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutBottomButtons.Name = "flowLayoutBottomButtons";
            this.flowLayoutBottomButtons.Size = new System.Drawing.Size(760, 42);
            this.flowLayoutBottomButtons.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(557, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 34);
            this.button2.TabIndex = 0;
            this.button2.Text = "Create Decision";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.submit_Click);
            // 
            // buttonShowAlternatives
            // 
            this.buttonShowAlternatives.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonShowAlternatives.Location = new System.Drawing.Point(3, 43);
            this.buttonShowAlternatives.Name = "buttonShowAlternatives";
            this.buttonShowAlternatives.Size = new System.Drawing.Size(232, 34);
            this.buttonShowAlternatives.TabIndex = 1;
            this.buttonShowAlternatives.Text = "Alternatives";
            this.buttonShowAlternatives.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonShowAlternatives.UseVisualStyleBackColor = true;
            this.buttonShowAlternatives.Click += new System.EventHandler(this.buttonShowAlternatives_Click);
            // 
            // tableLayoutMainContentGeneral
            // 
            this.tableLayoutMainContentGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutMainContentGeneral.ColumnCount = 1;
            this.tableLayoutMainContentGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMainContentGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMainContentGeneral.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutMainContentGeneral.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutMainContentGeneral.Name = "tableLayoutMainContentGeneral";
            this.tableLayoutMainContentGeneral.RowCount = 4;
            this.tableLayoutMainContentGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutMainContentGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutMainContentGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutMainContentGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutMainContentGeneral.Size = new System.Drawing.Size(760, 482);
            this.tableLayoutMainContentGeneral.TabIndex = 0;
            // 
            // TableLayoutMainContentAlternatives
            // 
            this.TableLayoutMainContentAlternatives.BackColor = System.Drawing.SystemColors.Control;
            this.TableLayoutMainContentAlternatives.ColumnCount = 1;
            this.TableLayoutMainContentAlternatives.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutMainContentAlternatives.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutMainContentAlternatives.Location = new System.Drawing.Point(4, 4);
            this.TableLayoutMainContentAlternatives.Margin = new System.Windows.Forms.Padding(4);
            this.TableLayoutMainContentAlternatives.Name = "TableLayoutMainContentAlternatives";
            this.TableLayoutMainContentAlternatives.RowCount = 4;
            this.TableLayoutMainContentAlternatives.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TableLayoutMainContentAlternatives.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TableLayoutMainContentAlternatives.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TableLayoutMainContentAlternatives.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.TableLayoutMainContentAlternatives.Size = new System.Drawing.Size(760, 482);
            this.TableLayoutMainContentAlternatives.TabIndex = 0;
            // 
            // ProjectSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 548);
            this.Controls.Add(this.tableLayoutForEverything);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProjectSetupWizard";
            this.Text = "Rationally - Prepare Decision View";
            this.tableLayoutForEverything.ResumeLayout(false);
            this.tableLayoutLeftColumn.ResumeLayout(false);
            this.tableLayoutLeftLogo.ResumeLayout(false);
            this.tableLayoutLeftLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutLeftMenu.ResumeLayout(false);
            this.tableLayoutRightColumn.ResumeLayout(false);
            this.flowLayoutBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion


        private System.Windows.Forms.TableLayoutPanel tableLayoutForEverything;
        private System.Windows.Forms.TableLayoutPanel tableLayoutLeftColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutRightColumn;
        public TableLayoutMainContentGeneral tableLayoutMainContentGeneral;//here
        public TableLayoutMainContentAlternatives TableLayoutMainContentAlternatives;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutBottomButtons;
        
        
       
        
        private System.Windows.Forms.TableLayoutPanel tableLayoutLeftLogo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelBottomLeftText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutLeftMenu;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.LinkLabel UpdateLink;
        private System.Windows.Forms.Button buttonShowAlternatives;
    }
}