﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentDocuments : TableLayoutPanel, IWizardPanel
    {
        public readonly List<FlowLayoutDocument> Documents;
        private readonly AntiAliasedButton addDocumentButton;

        public TableLayoutMainContentDocuments()
        {
            Documents = new List<FlowLayoutDocument>();

            addDocumentButton = new AntiAliasedButton();
            Init();
        }

        private void Init()
        {
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            //Controls.Add(Documents[0],0,0);
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
            Documents.Add(new FlowLayoutDocument(Documents.Count));
            UpdateRows();
        }

        public bool IsValid()
        {
            //check if all named rows have at least something in the path field
            if (!Documents.All(doc => !IsNullOrEmpty(doc.FilePath.Text) || IsNullOrEmpty(doc.FileName.Text)))
            {
                MessageBox.Show("For some named source(s), no file or link was choosen/entered");
                return false;
            }

            //select the documents to validate
            List<FlowLayoutDocument> toValidate = Documents.Where(doc => !IsNullOrEmpty(doc.FilePath.Text)).ToList();

            //because names are always valid and files not be malformatted, we only validate links
            List<FlowLayoutDocument> links = toValidate.Where(doc => !doc.FilePath.ReadOnly).ToList();

            if (!links.All(link => IsValidUrl(link.FilePath.Text)))
            {
                MessageBox.Show("One or more of the entered links are not valid hyperlinks.");
                return false;
            }
            return true;
        }

        private static bool IsValidUrl(string url) => Uri.IsWellFormedUriString(url, UriKind.Absolute);

        public void UpdateByModel()
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            Documents.Clear();
            for (int i = 0; i < model.Documents.Count; i++)
            {
                Documents.Add(new FlowLayoutDocument(i) {Document = model.Documents[i]});
            }
            UpdateRows();
        }

        public void UpdateModel()
        {
            throw new NotImplementedException();
        }
    }
}