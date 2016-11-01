using System.Diagnostics;
using System.Windows.Forms;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.EventHandlers.ClickEventHandlers;

namespace Rationally.Visio.Forms
{
    public partial class ProjectSetupWizard : Form
    {
        private static ProjectSetupWizard instance;
        private static bool documentCreation;

        public static ProjectSetupWizard Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new ProjectSetupWizard();
                }
                return instance;
            }
        }

        public void ShowDialog(bool onDocumentCreation)
        {

            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            BringToFront();
            documentCreation = onDocumentCreation;
            tableLayoutMainContentGeneral.TextAuthor.Text = Globals.RationallyAddIn.Model.Author;
            tableLayoutMainContentGeneral.TextDecisionTopic.Text = Globals.RationallyAddIn.Model.DecisionName;
            tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text = Globals.RationallyAddIn.Model.DateString;
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative1.UpdateData();
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative2.UpdateData();
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative3.UpdateData();
            CreateButton.Text = documentCreation ? "Create Decision" : "Update Decision";
            ShowDialog();
        }

        private ProjectSetupWizard()
        {
            InitializeComponent();
            if (!Globals.RationallyAddIn.NewVersionAvailable)
            {
                UpdateLink.Hide();
            }

            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = CreateButton;
        }


        private void submit_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tableLayoutMainContentGeneral.TextDecisionTopic.Text))
            {
#if DEBUG
                tableLayoutMainContentGeneral.TextDecisionTopic.Text = "Title";
#else
                MessageBox.Show("Enter a decision topic.", "Decision topic missing");
                DialogResult = DialogResult.None;
                return;
#endif
            }
            if (string.IsNullOrWhiteSpace(tableLayoutMainContentGeneral.TextAuthor.Text))
            {
#if DEBUG
                tableLayoutMainContentGeneral.TextAuthor.Text = "Author";
#else
                MessageBox.Show("Enter the author's name.", "Author's name missing");
                DialogResult = DialogResult.None;
                return;
#endif
            }
            UpdateGeneralInformationHandler.Execute(tableLayoutMainContentGeneral.TextAuthor.Text,
                                                tableLayoutMainContentGeneral.TextDecisionTopic.Text,
                                                tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text, documentCreation);

            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative1.UpdateModel();
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative2.UpdateModel();
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative3.UpdateModel();
            Close();
        }

        private void UpdateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(Constants.RationallySite);
            Process.Start(sInfo);
        }

        private void buttonShowAlternatives_Click(object sender, System.EventArgs e)
        {
            tableLayoutRightColumn.Controls.Clear();
            tableLayoutRightColumn.Controls.Add(TableLayoutMainContentAlternatives);
            tableLayoutRightColumn.Controls.Add(flowLayoutBottomButtons);
            flowLayoutBottomButtons.Refresh();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            tableLayoutRightColumn.Controls.Clear();
            tableLayoutRightColumn.Controls.Add(tableLayoutMainContentGeneral);
            tableLayoutRightColumn.Controls.Add(flowLayoutBottomButtons);
            flowLayoutBottomButtons.Refresh();
        }
    }
}
