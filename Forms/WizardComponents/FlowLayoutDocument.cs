using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class FlowLayoutDocument : FlowLayoutPanel
    {
        private readonly int documentIndex;

        internal readonly AntiAliasedLabel FileNameLabel;
        internal readonly TextBox FileName;
        private readonly AntiAliasedLabel filePathLabel;
        internal readonly TextBox FilePath;
        private readonly AntiAliasedButton chooseFileButton;
        private readonly AntiAliasedButton deleteDocumentButton;

        public RelatedDocument Document;

        public FlowLayoutDocument(int documentIndex)
        {
            this.documentIndex = documentIndex;

            Dock = DockStyle.Fill;
            //this.Anchor = AnchorStyles.Left;
            Location = new System.Drawing.Point(3, 3);
            Name = "flowLayoutPanelDocument" + this.documentIndex;
            Size = new System.Drawing.Size(714, 84);
            TabIndex = 0;

            FileNameLabel = new AntiAliasedLabel();
            FileName = new TextBox();
            filePathLabel = new AntiAliasedLabel();
            FilePath = new TextBox();
            chooseFileButton = new AntiAliasedButton();
            deleteDocumentButton = new AntiAliasedButton();
            SuspendLayout();
            Init();
        }

        private void Init()
        {
            Controls.Add(FileNameLabel);
            Controls.Add(FileName);
            Controls.Add(filePathLabel);
            Controls.Add(FilePath);
            Controls.Add(chooseFileButton);
            Controls.Add(deleteDocumentButton);
            //
            // fileNameLabel
            //
            FileNameLabel.AutoSize = true;
            FileNameLabel.Location = new System.Drawing.Point(3, 9);
            FileNameLabel.Margin = new Padding(3, 10, 3, 0);
            FileNameLabel.Name = "fileNameLabel";
            FileNameLabel.Size = new System.Drawing.Size(100, 19);
            FileNameLabel.TabIndex = 0;
            FileNameLabel.Text = "Name:";
            //
            // fileName
            //
            FileName.Location = new System.Drawing.Point(110, 7);
            FileName.Margin = new Padding(3, 6, 400, 3);
            FilePath.Name = "fileName";
            FileName.Size = new System.Drawing.Size(300, 27);
            FileName.TabIndex = 1;
            //
            // filepathlabel
            //
            filePathLabel.AutoSize = true;
            filePathLabel.Location = new System.Drawing.Point(3, 59);
            filePathLabel.Margin = new Padding(3, 10, 3, 0);
            filePathLabel.Name = "filePathLabel";
            filePathLabel.Size = new System.Drawing.Size(100, 19);
            filePathLabel.TabIndex = 2;
            filePathLabel.Text = "Path:";
            //
            // filepath
            //
            FilePath.Location = new System.Drawing.Point(110, 57);
            FilePath.Margin = new Padding(3, 6, 3, 3);
            FilePath.Name = "filepath";
            FilePath.Size = new System.Drawing.Size(300, 27);
            FilePath.TabIndex = 3;
            //
            // choosefilebutton
            //
            chooseFileButton.Name = "ChooseFileButton";
            chooseFileButton.UseVisualStyleBackColor = true;
            chooseFileButton.Click += new System.EventHandler(this.ChooseFileButton_Click);
            chooseFileButton.TabIndex = 4;
            chooseFileButton.Location = new Point(480, 56);
            chooseFileButton.Size = new Size(100, 30);
            chooseFileButton.Margin = new Padding(3, 0, 3, 3);
            chooseFileButton.Text = "Choose file";
            //
            // deleteDocumentButton
            //
            deleteDocumentButton.Name = "DeleteDocumentButton";
            deleteDocumentButton.UseVisualStyleBackColor = true;
            deleteDocumentButton.Click += new System.EventHandler(this.RemoveFile);
            deleteDocumentButton.TabIndex = 5;
            deleteDocumentButton.Location = new Point(580, 56);
            deleteDocumentButton.Size = new Size(150, 30);
            deleteDocumentButton.Margin = new Padding(3, 0, 3, 3);
            deleteDocumentButton.Text = "Remove this file";
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            //let the user pick a file and save the path of that file, so store in the textbox of this document row
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath.Text = openFileDialog.FileName;
                FilePath.ReadOnly = true;
            }
        }


        private void RemoveFile(object sender, EventArgs e)
        {
            ProjectSetupWizard.Instance.TableLayoutMainContentDocuments.Documents.RemoveAt(documentIndex);
            ProjectSetupWizard.Instance.TableLayoutMainContentDocuments.UpdateRows();
        }

    }
}
