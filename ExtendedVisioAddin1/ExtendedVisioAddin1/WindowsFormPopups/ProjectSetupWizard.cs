using System.Diagnostics;
using System.Windows.Forms;
using Rationally.Visio.EventHandlers.ClickEventHandlers;

namespace Rationally.Visio.WindowsFormPopups
{
    public partial class ProjectSetupWizard : Form
    {
        private static ProjectSetupWizard instance = null;
        private static bool documentCreation = false;

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
            ShowDialog();
        }

        private ProjectSetupWizard()
        {
            InitializeComponent();
            if (!Globals.RationallyAddIn.NewVersionAvailable)
            {
                UpdateLink.Hide();
            }
            this.tableLayoutMainContentGeneral.TextAuthor.Text = Globals.RationallyAddIn.Model.Author;
            this.tableLayoutMainContentGeneral.TextDecisionTopic.Text = Globals.RationallyAddIn.Model.DecisionName;
            this.tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text = Globals.RationallyAddIn.Model.Date;

            StartPosition = FormStartPosition.CenterScreen;
        }
        

        private void submit_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.tableLayoutMainContentGeneral.TextAuthor.Text))
            {
                MessageBox.Show("Enter the author's name.", "Author's name missing");
                DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(this.tableLayoutMainContentGeneral.TextDecisionTopic.Text))
            {
                MessageBox.Show("Enter a decision topic.", "Decision topic missing");
                DialogResult = DialogResult.None;
                return;
            }
            CreateDecisionClickHandler.Execute(tableLayoutMainContentGeneral.TextAuthor.Text, 
                                                tableLayoutMainContentGeneral.TextDecisionTopic.Text, 
                                                tableLayoutMainContentGeneral.DateTimePickerCreationDate.Text, documentCreation);
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
