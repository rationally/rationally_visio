using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.Forms.WizardComponents
{
    public sealed class FlowLayoutDocument : GroupBox
    {
        public int DocumentIndex { get; private set; }

        private readonly AntiAliasedLabel fileNameLabel;
        internal readonly TextBox FileName;
        private readonly AntiAliasedLabel filePathLabel;
        internal readonly TextBox FilePath;
        private readonly AntiAliasedButton chooseFileButton;
        private readonly AntiAliasedButton deleteDocumentButton;

        public RelatedDocument Document => ProjectSetupWizard.Instance.ModelCopy.Documents.Count > DocumentIndex ? ProjectSetupWizard.Instance.ModelCopy.Documents[DocumentIndex] : null;

        public FlowLayoutDocument(int documentIndex)
        {
            DocumentIndex = documentIndex;

            Dock = DockStyle.Top;
            //this.Anchor = AnchorStyles.Left;
            Location = new Point(3, 3);
            Name = "flowLayoutPanelDocument" + DocumentIndex;
            Size = new Size(714, 84);
            TabIndex = 0;

            fileNameLabel = new AntiAliasedLabel();
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
            Controls.Add(fileNameLabel);
            Controls.Add(FileName);
            Controls.Add(filePathLabel);
            Controls.Add(FilePath);
            Controls.Add(chooseFileButton);
            Controls.Add(deleteDocumentButton);
            //
            // fileNameLabel
            //
            fileNameLabel.AutoSize = true;
            fileNameLabel.Location = new Point(8, 17);
            fileNameLabel.Margin = new Padding(3, 10, 3, 0);
            fileNameLabel.Name = "fileNameLabel";
            fileNameLabel.Size = new Size(100, 19);
            fileNameLabel.TabIndex = 0;
            fileNameLabel.Text = "Name:";
            //
            // fileName
            //
            FileName.Location = new Point(110, 15);
            FileName.Margin = new Padding(3, 6, 400, 3);
            FilePath.Name = "fileName";
            FileName.Size = new Size(350, 27);
            FileName.TabIndex = 1;
            //
            // filepathlabel
            //
            filePathLabel.AutoSize = true;
            filePathLabel.Location = new Point(8,52);
            filePathLabel.Margin = new Padding(3, 10, 3, 0);
            filePathLabel.Name = "filePathLabel";
            filePathLabel.Size = new Size(100, 19);
            filePathLabel.TabIndex = 2;
            filePathLabel.Text = "Path:";
            //
            // filepath
            //
            FilePath.Location = new Point(110, 50);
            FilePath.Margin = new Padding(3, 6, 3, 3);
            FilePath.Name = "filepath";
            FilePath.Size = new Size(350, 27);
            FilePath.TabIndex = 3;
            //
            // choosefilebutton
            //
            chooseFileButton.Name = "ChooseFileButton";
            chooseFileButton.UseVisualStyleBackColor = true;
            chooseFileButton.Click += ChooseFileButton_Click;
            chooseFileButton.TabIndex = 4;
            chooseFileButton.Location = new Point(500, 50);
            chooseFileButton.Size = new Size(100, 27);
            chooseFileButton.Margin = new Padding(3, 0, 3, 3);
            chooseFileButton.Text = "Choose file";
            //
            // deleteDocumentButton
            //
            deleteDocumentButton.Name = "DeleteDocumentButton";
            deleteDocumentButton.UseVisualStyleBackColor = true;
            deleteDocumentButton.Click += RemoveFile;
            deleteDocumentButton.TabIndex = 5;
            deleteDocumentButton.Location = new Point(600, 50);
            deleteDocumentButton.Size = new Size(140, 27);
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
            ProjectSetupWizard.Instance.TableLayoutMainContentDocuments.Documents.Remove(this);
            ProjectSetupWizard.Instance.ModelCopy.Documents.Remove(Document);
            ProjectSetupWizard.Instance.TableLayoutMainContentDocuments.UpdateRows();
        }

        public void UpdateModel()
        {
            if (Document != null)
            {
                if (FilePath.ReadOnly == Document.IsFile) //either both are files, or both are links
                {
                    Document.Name = FileName.Text;
                    Document.Path = FilePath.Text;
                    return;
                }
                //else
                Globals.RationallyAddIn.Model.Documents.RemoveAt(DocumentIndex); //remove old (which is wrong type), create a new

            }

            //create a new document (optionally at the correct index)
            RelatedDocument newDocument = new RelatedDocument(FilePath.Text,FileName.Text,FilePath.ReadOnly);
            DocumentIndex = Math.Min(DocumentIndex, ProjectSetupWizard.Instance.ModelCopy.Documents.Count);
            ProjectSetupWizard.Instance.ModelCopy.Documents.Insert(DocumentIndex, newDocument);
        }

        public void UpdateData()
        {
            if (Document != null)
            {
                FileName.Text = Document.Name;
                FilePath.Text = Document.Path;//what if empty //TODO should reference file link object instead of path shape
                FilePath.ReadOnly = Document.IsFile;
            }
        }
    }
}
