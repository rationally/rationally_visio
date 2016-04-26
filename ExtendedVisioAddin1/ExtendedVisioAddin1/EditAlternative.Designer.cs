namespace ExtendedVisioAddin1
{
    partial class EditAlternative
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
            this.editStatusBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.editAlternativeButton = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // editStatusBox
            // 
            this.editStatusBox.FormattingEnabled = true;
            this.editStatusBox.Location = new System.Drawing.Point(16, 44);
            this.editStatusBox.Name = "editStatusBox";
            this.editStatusBox.Size = new System.Drawing.Size(168, 21);
            this.editStatusBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Status";
            // 
            // editAlternativeButton
            // 
            this.editAlternativeButton.Location = new System.Drawing.Point(16, 71);
            this.editAlternativeButton.Name = "editAlternativeButton";
            this.editAlternativeButton.Size = new System.Drawing.Size(168, 23);
            this.editAlternativeButton.TabIndex = 2;
            this.editAlternativeButton.Text = "Accept";
            this.editAlternativeButton.UseVisualStyleBackColor = true;
            this.editAlternativeButton.Click += new System.EventHandler(this.editAlternativeButton_Click);
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(13, 9);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(35, 13);
            this.name.TabIndex = 3;
            this.name.Text = "label2";
            // 
            // EditAlternative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 106);
            this.Controls.Add(this.name);
            this.Controls.Add(this.editAlternativeButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editStatusBox);
            this.Name = "EditAlternative";
            this.Text = "EditAlternative";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editAlternativeButton;
        private System.Windows.Forms.Label name;
        internal System.Windows.Forms.ComboBox editStatusBox;
    }
}