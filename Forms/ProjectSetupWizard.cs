using System;
using System.Diagnostics;
using System.Drawing.Text;
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
                if (instance == null || instance.IsDisposed)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedFieldType), selectedFieldType, "You actually managed to set a wrong enum value. Well done.");
            }
        }

        public void SetGeneralPanel()
        {
            tableLayoutRightColumn.Controls.Clear();
            tableLayoutRightColumn.Controls.Add(tableLayoutMainContentGeneral);
            tableLayoutRightColumn.Controls.Add(flowLayoutBottomButtons);
            flowLayoutBottomButtons.Refresh();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            base.OnPaint(pevent);
        }
    }
}
