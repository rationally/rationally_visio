using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Model;
using static System.String;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentDocuments : TableLayoutPanel, IWizardPanel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public readonly List<FlowLayoutDocument> Documents;
        public readonly AntiAliasedButton AddDocumentButton;

        public TableLayoutMainContentDocuments()
        {
            Documents = new List<FlowLayoutDocument>();

            AddDocumentButton = new AntiAliasedButton();
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
            //
            // addDocumentButton
            //
            AddDocumentButton.Name = "AddDocumentButton";
            AddDocumentButton.UseVisualStyleBackColor = true;
            AddDocumentButton.Click += AddDocumentButton_Click;
            AddDocumentButton.Text = "Add File";
            AddDocumentButton.Size = new Size(200,34);
            AddDocumentButton.Margin = new Padding(0,0,360,0);
            AddDocumentButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            

            UpdateRows();
        }

        private void InitScrollBar()
        {
            //the following lines are a weird hack to enable vertical scrolling without enabling horizontal scrolling:
            HorizontalScroll.Maximum = 0;
            AutoScroll = false;
            //VerticalScroll.Visible = false;
            AutoScroll = true;
        }

        public void UpdateRows()
        {
            Controls.Clear();
            RowStyles.Clear();

            RowCount = Documents.Count;
            InitScrollBar();

            for (int i = 0; i < Documents.Count; i++)
            {
                Controls.Add(Documents[i],0,i);//add control to view
                RowStyles.Add(new RowStyle(SizeType.Absolute, 95));//style the just added row
            }
        }

        private void AddDocumentButton_Click(object sender, EventArgs e) => AddFile();

        private void AddFile()
        {
            Documents.Add(new FlowLayoutDocument(Documents.Count > 0 ? Documents.Last().DocumentIndex + 1 : 0));
            UpdateRows();
        }

        public void InitData()
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            Documents.Clear();
            for (int i = 0; i < model.Documents.Count; i++)
            {
                Documents.Add(new FlowLayoutDocument(i));
            }
            UpdateRows();
            Documents.ForEach(d => d.UpdateData());
            Log.Debug("Initialized documents wizard page.");
        }

        public bool IsValid()
        {
            //check if all named rows have at least something in the path field
            if (!Documents.All(doc => !IsNullOrEmpty(doc.FilePath.Text) || IsNullOrEmpty(doc.FileName.Text)))
            {
                MessageBox.Show("For some named documents, no file or link was choosen/entered");
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
        
        public void UpdateModel()
        {
            //handle changes in the "Related Documents" page
            WizardUpdateDocumentsHandler.Execute(ProjectSetupWizard.Instance);
            Log.Debug("Documents updated.");
        }
    }
}
