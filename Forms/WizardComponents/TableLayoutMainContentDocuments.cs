using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentDocuments : TableLayoutPanel
    {
        public readonly List<FlowLayoutDocument> Documents;
        private readonly AntiAliasedButton addDocumentButton;

        public TableLayoutMainContentDocuments()
        {
            Documents = new List<FlowLayoutDocument>() { new FlowLayoutDocument(0) };
            addDocumentButton = new AntiAliasedButton();
            Init();
        }

        private void Init()
        {
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(Documents[0],0,0);
            Dock = DockStyle.Fill;
            Location = new Point(4, 4);
            Size = new Size(760, 482);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentDocuments";
            //the following lines are a weird hack to enable vertical scrolling without enabling horizontal scrolling:
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            VerticalScroll.Visible = false;
            AutoScroll = true;
            //
            // addDocumentButton
            //
            addDocumentButton.Name = "AddDocumentButton";
            addDocumentButton.UseVisualStyleBackColor = true;
            addDocumentButton.Click += AddDocumentButton_Click;
            addDocumentButton.Text = "Add File";
            addDocumentButton.Size = new Size(200,30);
            addDocumentButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            

            UpdateRows();
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = Documents.Count + 1;//+ row with "add file" button

            for (int i = 0; i < Documents.Count; i++)
            {
                Controls.Add(Documents[i],0,i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 100));//style the just added row
            }
            Controls.Add(addDocumentButton, 0, RowCount-1);//c-indexed
            RowStyles.Add(new RowStyle(SizeType.AutoSize));//add a style for the add file button
        }

        private void AddDocumentButton_Click(object sender, EventArgs e) => AddFile();

        private void AddFile()
        {
            Documents.Add(new FlowLayoutDocument(Globals.RationallyAddIn.Model.Documents.Count));
            UpdateRows();
        }

        public bool IsValid()
        {
            bool isValid = Documents.Select(document => IsNullOrEmpty(document.FileName.Text) || !IsNullOrEmpty(document.FilePath.Text)).Aggregate(true,(doc1,doc2) => doc1 && doc2);
            if (!isValid)
            {
                MessageBox.Show("For some named source(s), no file or link was choosen/entered");
            }
            return isValid;
        }
    }
}
