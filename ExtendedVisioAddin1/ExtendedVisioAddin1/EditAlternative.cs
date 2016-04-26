using System;
using System.Windows.Forms;
using ExtendedVisioAddin1.Model;

namespace ExtendedVisioAddin1
{
    internal partial class EditAlternative : Form
    {
        public EditAlternative(RModel model, string alternative, string currentStatus)
        {
            InitializeComponent();
            name.Text = "Change state of " + alternative;
            editStatusBox.Items.AddRange(model.AlternativeStates.ToArray());
            editStatusBox.SelectedItem = currentStatus;
        }

        private void editAlternativeButton_Click(object sender, EventArgs e)
        {
            if (editStatusBox.SelectedIndex > -1)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Select a status.");
                this.DialogResult = DialogResult.None;
            }
            return;
        }
    }
}
