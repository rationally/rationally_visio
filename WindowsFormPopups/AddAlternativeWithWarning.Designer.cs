namespace Rationally.Visio.WindowsFormPopups
{
    partial class AddAlternativeWithWarning
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
            this.WarningText = new System.Windows.Forms.Label();
            this.createAlternative = new System.Windows.Forms.Button();
            this.alternativeStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.alternativeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WarningText
            // 
            this.WarningText.Location = new System.Drawing.Point(9, 9);
            this.WarningText.Name = "WarningText";
            this.WarningText.Size = new System.Drawing.Size(231, 47);
            this.WarningText.TabIndex = 11;
            this.WarningText.Text = "Warning: Adding more than three alternative solutions may have a negative effect " +
    "on the layout.";
            // 
            // createAlternative
            // 
            this.createAlternative.Location = new System.Drawing.Point(9, 143);
            this.createAlternative.Name = "createAlternative";
            this.createAlternative.Size = new System.Drawing.Size(231, 23);
            this.createAlternative.TabIndex = 10;
            this.createAlternative.Text = "Create";
            this.createAlternative.UseVisualStyleBackColor = true;
            this.createAlternative.Click += new System.EventHandler(this.createAlternative_Click);
            // 
            // alternativeStatus
            // 
            this.alternativeStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alternativeStatus.FormattingEnabled = true;
            this.alternativeStatus.Location = new System.Drawing.Point(9, 116);
            this.alternativeStatus.Name = "alternativeStatus";
            this.alternativeStatus.Size = new System.Drawing.Size(231, 21);
            this.alternativeStatus.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Alternative status";
            // 
            // alternativeName
            // 
            this.alternativeName.Location = new System.Drawing.Point(9, 77);
            this.alternativeName.Name = "alternativeName";
            this.alternativeName.Size = new System.Drawing.Size(231, 20);
            this.alternativeName.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Alternative name";
            // 
            // AddAlternativeWithWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 178);
            this.Controls.Add(this.WarningText);
            this.Controls.Add(this.createAlternative);
            this.Controls.Add(this.alternativeStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.alternativeName);
            this.Controls.Add(this.label1);
            this.Name = "AddAlternativeWithWarning";
            this.Text = "Add alternative";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label WarningText;
        private System.Windows.Forms.Button createAlternative;
        internal System.Windows.Forms.ComboBox alternativeStatus;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox alternativeName;
        private System.Windows.Forms.Label label1;
    }
}