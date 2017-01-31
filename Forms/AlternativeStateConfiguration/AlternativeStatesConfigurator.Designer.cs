using System.Drawing;

namespace Rationally.Visio.Forms
{
    partial class AlternativeStatesConfigurator
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
            this.tableLayoutAllContent = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutStateList = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutMainContent = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutStateActionButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.addStateButton = new System.Windows.Forms.Button();
            this.tableLayoutStateContent = new Rationally.Visio.Forms.AlternativeStateConfiguration.TableLayoutAlternativeStates();
            this.tableLayoutActionButtons = new System.Windows.Forms.TableLayoutPanel();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutAllContent.SuspendLayout();
            this.tableLayoutStateList.SuspendLayout();
            this.tableLayoutMainContent.SuspendLayout();
            this.flowLayoutStateActionButtons.SuspendLayout();
            this.tableLayoutActionButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutAllContent
            // 
            this.tableLayoutAllContent.ColumnCount = 1;
            this.tableLayoutAllContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutAllContent.Controls.Add(this.tableLayoutStateList, 0, 0);
            this.tableLayoutAllContent.Controls.Add(this.tableLayoutActionButtons, 0, 1);
            this.tableLayoutAllContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutAllContent.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutAllContent.Name = "tableLayoutAllContent";
            this.tableLayoutAllContent.RowCount = 2;
            this.tableLayoutAllContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutAllContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutAllContent.Size = new System.Drawing.Size(501, 384);
            this.tableLayoutAllContent.TabIndex = 0;
            // 
            // tableLayoutStateList
            // 
            this.tableLayoutStateList.ColumnCount = 1;
            this.tableLayoutStateList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutStateList.Controls.Add(this.tableLayoutMainContent, 0, 0);
            this.tableLayoutStateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutStateList.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutStateList.Name = "tableLayoutStateList";
            this.tableLayoutStateList.RowCount = 1;
            this.tableLayoutStateList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutStateList.Size = new System.Drawing.Size(495, 338);
            this.tableLayoutStateList.TabIndex = 0;
            // 
            // tableLayoutMainContent
            // 
            this.tableLayoutMainContent.ColumnCount = 1;
            this.tableLayoutMainContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMainContent.Controls.Add(this.flowLayoutStateActionButtons, 0, 1);
            this.tableLayoutMainContent.Controls.Add(this.tableLayoutStateContent, 0, 0);
            this.tableLayoutMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMainContent.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMainContent.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutMainContent.Name = "tableLayoutMainContent";
            this.tableLayoutMainContent.RowCount = 3;
            this.tableLayoutMainContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMainContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutMainContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutMainContent.Size = new System.Drawing.Size(495, 338);
            this.tableLayoutMainContent.TabIndex = 0;
            // 
            // flowLayoutStateActionButtons
            // 
            this.flowLayoutStateActionButtons.Controls.Add(this.addStateButton);
            this.flowLayoutStateActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutStateActionButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutStateActionButtons.Location = new System.Drawing.Point(3, 281);
            this.flowLayoutStateActionButtons.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.flowLayoutStateActionButtons.Name = "flowLayoutStateActionButtons";
            this.flowLayoutStateActionButtons.Size = new System.Drawing.Size(492, 34);
            this.flowLayoutStateActionButtons.TabIndex = 0;
            // 
            // addStateButton
            // 
            this.addStateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addStateButton.Location = new System.Drawing.Point(366, 3);
            this.addStateButton.Name = "addStateButton";
            this.addStateButton.Size = new System.Drawing.Size(123, 28);
            this.addStateButton.TabIndex = 5;
            this.addStateButton.Text = "Add State";
            this.addStateButton.UseVisualStyleBackColor = true;
            this.addStateButton.Click += addStateButton_Click;

            // 
            // tableLayoutStateContent
            // 
            this.tableLayoutStateContent.ColumnCount = 1;
            this.tableLayoutStateContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutStateContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutStateContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutStateContent.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutStateContent.Name = "tableLayoutStateContent";
            this.tableLayoutStateContent.RowCount = 1;
            this.tableLayoutStateContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 272F));
            this.tableLayoutStateContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 272F));
            this.tableLayoutStateContent.Size = new System.Drawing.Size(489, 272);
            this.tableLayoutStateContent.TabIndex = 1;
            // 
            // tableLayoutActionButtons
            // 
            this.tableLayoutActionButtons.ColumnCount = 2;
            this.tableLayoutActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutActionButtons.Controls.Add(this.saveButton, 0, 0);
            this.tableLayoutActionButtons.Controls.Add(this.cancelButton, 0, 0);
            this.tableLayoutActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutActionButtons.Location = new System.Drawing.Point(3, 347);
            this.tableLayoutActionButtons.Name = "tableLayoutActionButtons";
            this.tableLayoutActionButtons.RowCount = 1;
            this.tableLayoutActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutActionButtons.Size = new System.Drawing.Size(495, 34);
            this.tableLayoutActionButtons.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(369, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(123, 28);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(3, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(123, 28);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // AlternativeStatesConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 384);
            this.Controls.Add(this.tableLayoutAllContent);
            this.Name = "AlternativeStatesConfigurator";
            this.Text = "Configure Alternative States";
            this.tableLayoutAllContent.ResumeLayout(false);
            this.tableLayoutStateList.ResumeLayout(false);
            this.tableLayoutMainContent.ResumeLayout(false);
            this.flowLayoutStateActionButtons.ResumeLayout(false);
            this.tableLayoutActionButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutAllContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutStateList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutActionButtons;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMainContent;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutStateActionButtons;
        private System.Windows.Forms.Button addStateButton;
        private Rationally.Visio.Forms.AlternativeStateConfiguration.TableLayoutAlternativeStates tableLayoutStateContent;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        
    }
}