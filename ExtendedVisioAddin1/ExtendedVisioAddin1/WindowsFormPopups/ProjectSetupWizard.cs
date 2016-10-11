using System.Diagnostics;
using System.Windows.Forms;
using Rationally.Visio.EventHandlers.ClickEventHandlers;

namespace Rationally.Visio.WindowsFormPopups
{
    public partial class ProjectSetupWizard : Form
    {
        public ProjectSetupWizard()
        {
            InitializeComponent();
            if (!Globals.RationallyAddIn.NewVersionAvailable)
            {
                UpdateLink.Hide();
            }
        }
        

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textAuthor.Text))
            {
                MessageBox.Show("Enter the author's name.", "Author's name missing");
                DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(textDecisionTopic.Text))
            {
                MessageBox.Show("Enter a decision topic.", "Decision topic missing");
                DialogResult = DialogResult.None;
                return;
            }
            CreateDecisionClickHandler.Execute(textAuthor.Text, textDecisionTopic.Text, dateTimePickerCreationDate.Text);
            Close();
        }

        private void UpdateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(Constants.RationallySite);
            Process.Start(sInfo);
        }
    }
}
