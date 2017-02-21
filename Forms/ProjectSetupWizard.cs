using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json;
using Rationally.Visio.Enums;
using Rationally.Visio.Forms.WizardComponents;
using Rationally.Visio.RationallyConstants;
using RestSharp;


namespace Rationally.Visio.Forms
{
    public partial class ProjectSetupWizard : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ProjectSetupWizard instance;
        private WizardFieldTypes selectedFieldType;
        public static bool DocumentCreation;
        private readonly PleaseWait pleaseWait = new PleaseWait();
        private readonly List<IWizardPanel> panelList;
        public IWizardPanel CurrentPanel;

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
            panelList = new List<IWizardPanel>
            {
                tableLayoutMainContentGeneral,
                TableLayoutMainContentAlternatives,
                TableLayoutMainContentForces,
                TableLayoutMainContentDocuments,
                TableLayoutMainContentStakeholders
            };
            StartPosition = FormStartPosition.CenterScreen;
            Log.Debug("Setting AcceptButton as CreateButton with text:" + CreateButton.Text);
            AcceptButton = CreateButton;
        }


        private void submit_Click(object sender, EventArgs e)
        {
            if(panelList.TrueForAll(panel => panel.IsValid())) {
                Log.Debug("Everyting is valid.");
                pleaseWait.Show();
                pleaseWait.Refresh();
                //wrap all changes that will be triggered by wizard changes in one undo scope
                int wizardScopeId = Globals.RationallyAddIn.Application.BeginUndoScope("Wizard actions");

                CurrentPanel.UpdateModel();
                
                //all changes have been made, close the scope and the wizard
                Globals.RationallyAddIn.Application.EndUndoScope(wizardScopeId, true);
                Close();
                Log.Debug("Closed wizard");
                pleaseWait.Hide();
                //TestServerCreateDecision();
            }
        }

        public void TestServerCreateDecision()
        {
            RestClient client = new RestClient("http://localhost:4567/");
            RestRequest request = new RestRequest("/decision/", Method.POST);
            string jsonToSend = JsonConvert.SerializeObject(Globals.RationallyAddIn.Model);
            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
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
            CurrentPanel = tableLayoutMainContentGeneral;
            tableLayoutLeftMenu.HighLightedButton = tableLayoutLeftMenu.ButtonShowGeneral;
            tableLayoutRightColumn.Controls.Clear();
            tableLayoutRightColumn.Controls.Add(tableLayoutMainContentGeneral);
            tableLayoutRightColumn.Controls.Add(FlowLayoutBottomButtons);
            //define bottom buttons
            FlowLayoutBottomButtons.Controls.Clear();
            FlowLayoutBottomButtons.Controls.Add(CreateButton);
            FlowLayoutBottomButtons.Refresh();

            tableLayoutMainContentGeneral.InitData();
        }

    }
}
