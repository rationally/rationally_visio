using System.Diagnostics;
using System.Windows.Forms;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.Forms
{
    public partial class ProjectSetupWizard : Form
    {
        private static ProjectSetupWizard _instance;
        public static bool DocumentCreation;

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
            DocumentCreation = onDocumentCreation;
            tableLayoutMainContentGeneral.TextAuthor.Text = Globals.RationallyAddIn.Model.Author;
            tableLayoutMainContentGeneral.TextDecisionTopic.Text = Globals.RationallyAddIn.Model.DecisionName;
            tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text = Globals.RationallyAddIn.Model.DateString;
            TableLayoutMainContentAlternatives.AlternativeRows.ForEach(a => a.UpdateData());
            if (DocumentCreation)
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
            //wrap all changes that will be triggered by wizard changes in one undo scope
            int wizardScopeId = Globals.RationallyAddIn.Application.BeginUndoScope("Wizard actions");


            //handle changes in the "General Information" page
            WizardUpdateGeneralInformationHandler.Execute(this);
            //handle changes in the "Alternatives" page
            WizardUpdateAlternativesHandler.Execute(this);


            //all changes have been made, close the scope and the wizard
            Globals.RationallyAddIn.Application.EndUndoScope(wizardScopeId, true);
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
