using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.Enums;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.EventHandlers.WizardPageHandlers;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.Forms
{
    public partial class ProjectSetupWizard : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ProjectSetupWizard instance;
        private WizardFieldTypes selectedFieldType;
        public static bool DocumentCreation;
        private readonly PleaseWait pleaseWait = new PleaseWait();

        public static ProjectSetupWizard Instance
        {
            get
            {
                if (instance?.IsDisposed ?? true)
                {
                    instance = new ProjectSetupWizard();
                }
                return instance;
            }
        }

        public void ShowDialog(bool onDocumentCreation, WizardFieldTypes type)
        {
            Log.Debug("Entered showDialog.");
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
            Log.Debug("Read all general information from the model and wrote it to the wizard.");
            TableLayoutMainContentAlternatives.AlternativeRows.ForEach(a => a.UpdateData());
            TableLayoutMainContentForces.InitData();
            Log.Debug("Initialized alternatives wizard page.");
            TableLayoutMainContentDocuments.UpdateByModel();//create rows according to model
            TableLayoutMainContentDocuments.Documents.ForEach(d => d.UpdateData());
            Log.Debug("Initialized documents wizard page.");
            TableLayoutMainContentStakeholders.UpdateData();
            TableLayoutMainContentStakeholders.Stakeholders.ForEach(d => d.UpdateData());
            Log.Debug("Initialized stakeholders wizard page.");
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
            Log.Debug("Setting AcceptButton as CreateButton with text:" + CreateButton.Text);
            AcceptButton = CreateButton;
        }


        private void submit_Click(object sender, EventArgs e)
        {
            
            
            if (ValidateGeneralIfNotDebugging() && ValidateAlternatives() && TableLayoutMainContentForces.IsValid() && TableLayoutMainContentDocuments.IsValid() && TableLayoutMainContentStakeholders.IsValid())
            {
                Log.Debug("Everyting is valid.");
                pleaseWait.Show();
                pleaseWait.Refresh();
                //pleaseWait.Show();
                //wrap all changes that will be triggered by wizard changes in one undo scope
                int wizardScopeId = Globals.RationallyAddIn.Application.BeginUndoScope("Wizard actions");

                
                Log.Debug("Setting view page and rebuilding tree.");
                Globals.RationallyAddIn.View.Page = Globals.RationallyAddIn.Application.ActivePage;
                Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
                //handle changes in the "General Information" page
                WizardUpdateGeneralInformationHandler.Execute(this);
                Log.Debug("General information updated.");
                //handle changes in the "Forces" page
                WizardUpdateForcesHandler.Execute(this);
                Log.Debug("Forces updated.");
                //handle changes in the "Alternatives" page
                WizardUpdateAlternativesHandler.Execute(this);
                Log.Debug("Alternatives updated.");
                //handle changes in the "Related Documents" page
                WizardUpdateDocumentsHandler.Execute(this);
                Log.Debug("Documents updated.");
                //handle changes in the "Stakeholders" page
                WizardUpdateStakeholdersHandler.Execute(this);
                Log.Debug("Stakeholders updated.");

                //all changes have been made, close the scope and the wizard
                Globals.RationallyAddIn.Application.EndUndoScope(wizardScopeId, true);
                Close();
                Log.Debug("Closed wizard");
                pleaseWait.Hide();
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
            tableLayoutRightColumn.Controls.Add(FlowLayoutBottomButtons);
            //define bottom buttons
            FlowLayoutBottomButtons.Controls.Clear();
            FlowLayoutBottomButtons.Controls.Add(CreateButton);
            FlowLayoutBottomButtons.Refresh();
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
