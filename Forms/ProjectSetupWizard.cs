using System.Diagnostics;
using System.Windows.Forms;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.Forms
{
    public partial class ProjectSetupWizard : Form
    {
        private static ProjectSetupWizard _instance;
        private static bool _documentCreation;

        public static ProjectSetupWizard Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new ProjectSetupWizard();
                }
                return _instance;
            }
        }

        public void ShowDialog(bool onDocumentCreation)
        {

            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            BringToFront();
            _documentCreation = onDocumentCreation;
            tableLayoutMainContentGeneral.TextAuthor.Text = Globals.RationallyAddIn.Model.Author;
            tableLayoutMainContentGeneral.TextDecisionTopic.Text = Globals.RationallyAddIn.Model.DecisionName;
            tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text = Globals.RationallyAddIn.Model.DateString;
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative1.UpdateData();
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative2.UpdateData();
            TableLayoutMainContentAlternatives.FlowLayoutPanelAlternative3.UpdateData();
            if (_documentCreation)
            {
                CreateButton.Text = Messages.Wizard_CreateButton_CreateView;
                Text = Messages.Wizard_Label_CreateView;
            }
            else
            {
                CreateButton.Text = Messages.Wizard_CreateButton_UpdateView;
                Text = Messages.Wizard_Label_UpdateView;
            }
           
            ShowDialog();
        }

        private ProjectSetupWizard()
        {
            InitializeComponent();
            tableLayoutMainContentGeneral = new TableLayoutMainContentGeneral();
            TableLayoutMainContentAlternatives = new TableLayoutMainContentAlternatives();
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
                                                tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text, _documentCreation);

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
