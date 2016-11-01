namespace rationally_visio
{
    partial class SheetSetUp
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.decisionName = new System.Windows.Forms.Label();
            this.authorName = new System.Windows.Forms.Label();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.headerTextDecision = new System.Windows.Forms.Label();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 32);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 0;
            // 
            // decisionName
            // 
            this.decisionName.AutoSize = true;
            this.decisionName.Location = new System.Drawing.Point(9, 16);
            this.decisionName.Name = "decisionName";
            this.decisionName.Size = new System.Drawing.Size(77, 13);
            this.decisionName.TabIndex = 1;
            this.decisionName.Text = "Decision name";
            // 
            // authorName
            // 
            this.authorName.AutoSize = true;
            this.authorName.Location = new System.Drawing.Point(12, 59);
            this.authorName.Name = "authorName";
            this.authorName.Size = new System.Drawing.Size(81, 13);
            this.authorName.TabIndex = 2;
            this.authorName.Text = "Name of Author";
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Location = new System.Drawing.Point(12, 76);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(100, 20);
            this.textBoxAuthor.TabIndex = 3;
            // 
            // headerTextDecision
            // 
            this.headerTextDecision.AutoSize = true;
            this.headerTextDecision.Location = new System.Drawing.Point(12, 103);
            this.headerTextDecision.Name = "headerTextDecision";
            this.headerTextDecision.Size = new System.Drawing.Size(101, 13);
            this.headerTextDecision.TabIndex = 4;
            this.headerTextDecision.Text = "Decision headertext";
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Location = new System.Drawing.Point(12, 120);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeader.TabIndex = 5;
            // 
            // submitButton
            // 
            this.submitButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.submitButton.Location = new System.Drawing.Point(12, 147);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 6;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            // 
            // SheetSetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.textBoxHeader);
            this.Controls.Add(this.headerTextDecision);
            this.Controls.Add(this.textBoxAuthor);
            this.Controls.Add(this.authorName);
            this.Controls.Add(this.decisionName);
            this.Controls.Add(this.textBoxName);
            this.Name = "SheetSetUp";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label decisionName;
        private System.Windows.Forms.Label authorName;
        private System.Windows.Forms.Label headerTextDecision;
        public System.Windows.Forms.TextBox textBoxName;
        public System.Windows.Forms.TextBox textBoxAuthor;
        public System.Windows.Forms.TextBox textBoxHeader;
        public System.Windows.Forms.Button submitButton;
    }
}