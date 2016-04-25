﻿namespace rationally_visio
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
            this.decisionName = new System.Windows.Forms.TextBox();
            this.decisionNameHead = new System.Windows.Forms.Label();
            this.authorNameHEad = new System.Windows.Forms.Label();
            this.author = new System.Windows.Forms.TextBox();
            this.versionDecision = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.dateHeader = new System.Windows.Forms.Label();
            this.date = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // decisionName
            // 
            this.decisionName.Location = new System.Drawing.Point(12, 32);
            this.decisionName.Name = "decisionName";
            this.decisionName.Size = new System.Drawing.Size(188, 20);
            this.decisionName.TabIndex = 0;
            // 
            // decisionNameHead
            // 
            this.decisionNameHead.AutoSize = true;
            this.decisionNameHead.Location = new System.Drawing.Point(12, 16);
            this.decisionNameHead.Name = "decisionNameHead";
            this.decisionNameHead.Size = new System.Drawing.Size(77, 13);
            this.decisionNameHead.TabIndex = 1;
            this.decisionNameHead.Text = "Decision name";
            // 
            // authorNameHEad
            // 
            this.authorNameHEad.AutoSize = true;
            this.authorNameHEad.Location = new System.Drawing.Point(12, 59);
            this.authorNameHEad.Name = "authorNameHEad";
            this.authorNameHEad.Size = new System.Drawing.Size(81, 13);
            this.authorNameHEad.TabIndex = 2;
            this.authorNameHEad.Text = "Name of Author";
            // 
            // author
            // 
            this.author.Location = new System.Drawing.Point(12, 76);
            this.author.Name = "author";
            this.author.Size = new System.Drawing.Size(188, 20);
            this.author.TabIndex = 3;
            // 
            // versionDecision
            // 
            this.versionDecision.AutoSize = true;
            this.versionDecision.Location = new System.Drawing.Point(12, 103);
            this.versionDecision.Name = "versionDecision";
            this.versionDecision.Size = new System.Drawing.Size(42, 13);
            this.versionDecision.TabIndex = 4;
            this.versionDecision.Text = "Version";
            // 
            // version
            // 
            this.version.Location = new System.Drawing.Point(12, 120);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(188, 20);
            this.version.TabIndex = 5;
            this.version.Text = "0.0.0";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(12, 193);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(188, 23);
            this.submitButton.TabIndex = 6;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // dateHeader
            // 
            this.dateHeader.AutoSize = true;
            this.dateHeader.Location = new System.Drawing.Point(12, 147);
            this.dateHeader.Name = "dateHeader";
            this.dateHeader.Size = new System.Drawing.Size(30, 13);
            this.dateHeader.TabIndex = 7;
            this.dateHeader.Text = "Date";
            // 
            // date
            // 
            this.date.Location = new System.Drawing.Point(12, 164);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(188, 20);
            this.date.TabIndex = 8;
            // 
            // SheetSetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 228);
            this.Controls.Add(this.date);
            this.Controls.Add(this.dateHeader);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.version);
            this.Controls.Add(this.versionDecision);
            this.Controls.Add(this.author);
            this.Controls.Add(this.authorNameHEad);
            this.Controls.Add(this.decisionNameHead);
            this.Controls.Add(this.decisionName);
            this.Name = "SheetSetUp";
            this.Text = "Decision creator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label decisionNameHead;
        private System.Windows.Forms.Label authorNameHEad;
        private System.Windows.Forms.Label versionDecision;
        public System.Windows.Forms.TextBox decisionName;
        public System.Windows.Forms.TextBox author;
        public System.Windows.Forms.TextBox version;
        public System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label dateHeader;
        public System.Windows.Forms.TextBox date;
    }
}