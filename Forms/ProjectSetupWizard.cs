using System;
using System.Diagnostics;
using System.Windows.Forms;
using Rationally.Visio.Enums;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.Forms
{
    public partial class ProjectSetupWizard : Form
    {
        private static ProjectSetupWizard instance;
        private WizardFieldTypes selectedFieldType;
        public static bool DocumentCreation;

        public static ProjectSetupWizard Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)//if (instance?.IsDisposed != true)
                {
                    instance = new ProjectSetupWizard();
                }
                return instance;
            }
        }

        public void ShowDialog(bool onDocumentCreation, WizardFieldTypes type)
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
            tableLayoutMainContentGeneral.TextVersion.Text = Globals.RationallyAddIn.Model.Version;
            TableLayoutMainContentAlternatives.AlternativeRows.ForEach(a => a.UpdateData());
            TableLayoutMainContentForces.InitData();

            TableLayoutMainContentDocuments.UpdateByModel();//create rows according to model
            TableLayoutMainContentDocuments.Documents.ForEach(d => d.UpdateData());
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
            selectedFieldType = type;
            ShowDialog();
        }

        private ProjectSetupWizard()
        {
            InitializeComponent();
            tableLayoutMainContentGeneral = new TableLayoutMainContentGeneral();
            TableLayoutMainContentAlternatives = new TableLayoutMainContentAlternatives();
            if (!Globals.RationallyAddIn.NewVersionAvailable)
            {
                UpdateLink.Text = "Current version: " + Globals.RationallyAddIn.AddInLocalVersion;
            }

            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = CreateButton;
        }


        private void submit_Click(object sender, EventArgs e)
        {
            if (ValidateGeneralIfNotDebugging() && ValidateAlternatives() && TableLayoutMainContentForces.IsValid() && TableLayoutMainContentDocuments.IsValid())
            {
                //wrap all changes that will be triggered by wizard changes in one undo scope
                int wizardScopeId = Globals.RationallyAddIn.Application.BeginUndoScope("Wizard actions");

                Globals.RationallyAddIn.View.Page = Globals.RationallyAddIn.Application.ActivePage;
                Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
                //handle changes in the "General Information" page
                WizardUpdateGeneralInformationHandler.Execute(this);
                //handle changes in the "Forces" page
                WizardUpdateForcesHandler.Execute(this);
                //handle changes in the "Alternatives" page
                WizardUpdateAlternativesHandler.Execute(this);
                //handle changes in the "Related Documents" page
                WizardUpdateDocumentsHandler.Execute(this);


                //all changes have been made, close the scope and the wizard
                Globals.RationallyAddIn.Application.EndUndoScope(wizardScopeId, true);
                Close();
            }
        }

        private void UpdateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(Constants.RationallySite);
            Process.Start(sInfo);
        }



        private void ProjectSetupWizard_Activated(object sender, EventArgs e)
        {
            SetGeneralPanel();
            switch (selectedFieldType)
            {
                case WizardFieldTypes.Title:
                    tableLayoutMainContentGeneral.TextDecisionTopic.Select();
                    break;
                case WizardFieldTypes.Author:
                    tableLayoutMainContentGeneral.TextAuthor.Select();
                    break;
                case WizardFieldTypes.Date:
                    tableLayoutMainContentGeneral.DateTimePickerCreationDate.Select();
                    break;
                case WizardFieldTypes.Version:
                    tableLayoutMainContentGeneral.TextVersion.Select();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedFieldType), selectedFieldType, "You actually managed to set a wrong enum value. Well done.");
            }
        }

        public void SetGeneralPanel()
        {
            tableLayoutLeftMenu.HighLightedButton = tableLayoutLeftMenu.ButtonShowGeneral;
            tableLayoutRightColumn.Controls.Clear();
            tableLayoutRightColumn.Controls.Add(tableLayoutMainContentGeneral);
            tableLayoutRightColumn.Controls.Add(flowLayoutBottomButtons);
            flowLayoutBottomButtons.Refresh();
        }

        private bool ValidateGeneralIfNotDebugging()
        {
            if (string.IsNullOrWhiteSpace(tableLayoutMainContentGeneral.TextDecisionTopic.Text))
            {
#if DEBUG
                tableLayoutMainContentGeneral.TextDecisionTopic.Text = "Title";
#else
                MessageBox.Show("Enter a decision topic.", "Decision topic missing");
                return false;
#endif
            }
            if (string.IsNullOrWhiteSpace(tableLayoutMainContentGeneral.TextAuthor.Text))
            {
#if DEBUG
                tableLayoutMainContentGeneral.TextAuthor.Text = "Author";
#else
                MessageBox.Show("Enter the author's name.", "Author's name missing");
                return false;
#endif
            }
            if (string.IsNullOrWhiteSpace(tableLayoutMainContentGeneral.TextVersion.Text))
            {
#if DEBUG
                tableLayoutMainContentGeneral.TextVersion.Text = "1.0.0";
#else
                MessageBox.Show("Enter the version number.", "Version number missing");
                return false;
#endif
            }
            return true;
        }

        private bool ValidateAlternatives()
        {
            bool validFields = TableLayoutMainContentAlternatives.AlternativeRows.TrueForAll(row => (row.Alternative == null) || !string.IsNullOrWhiteSpace(row.TextBoxAlternativeTitle.Text));
            if (!validFields)
            {
                MessageBox.Show("Enter a name for every existing alternative.", "Alternative name missing");
            }
            return validFields;
        }

    }
}
