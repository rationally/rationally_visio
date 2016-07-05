using System.ComponentModel;
using System.Windows.Forms;

namespace ExtendedVisioAddin1
{
    partial class AddAlternative
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
            this.label1 = new System.Windows.Forms.Label();
            this.alternativeName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.alternativeStatus = new System.Windows.Forms.ComboBox();
            this.createAlternative = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Alternative name";
            // 
            // alternativeName
            // 
            this.alternativeName.Location = new System.Drawing.Point(15, 25);
            this.alternativeName.Name = "alternativeName";
            this.alternativeName.Size = new System.Drawing.Size(231, 20);
            this.alternativeName.TabIndex = 1;
            this.alternativeName.Text = "Alternativo Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Alternative status";
            // 
            // alternativeStatus
            // 
            this.alternativeStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alternativeStatus.FormattingEnabled = true;
            this.alternativeStatus.Location = new System.Drawing.Point(15, 64);
            this.alternativeStatus.Name = "alternativeStatus";
            this.alternativeStatus.Size = new System.Drawing.Size(231, 21);
            this.alternativeStatus.TabIndex = 3;
            // 
            // createAlternative
            // 
            this.createAlternative.Location = new System.Drawing.Point(13, 91);
            this.createAlternative.Name = "createAlternative";
            this.createAlternative.Size = new System.Drawing.Size(231, 23);
            this.createAlternative.TabIndex = 4;
            this.createAlternative.Text = "Create";
            this.createAlternative.UseVisualStyleBackColor = true;
            this.createAlternative.Click += new System.EventHandler(this.createAlternative_Click);
            // 
            // AddAlternative
            // 
            this.AcceptButton = this.createAlternative;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 116);
            this.Controls.Add(this.createAlternative);
            this.Controls.Add(this.alternativeStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.alternativeName);
            this.Controls.Add(this.label1);
            this.Name = "AddAlternative";
            this.Text = "Add alternative";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        internal TextBox alternativeName;
        private Label label2;
        internal ComboBox alternativeStatus;
        private Button createAlternative;
    }
}