using System;
using System.Windows.Forms;
using ExtendedVisioAddin1.Model;

namespace ExtendedVisioAddin1
{
    internal partial class AddAlternative : Form
    {
        public AddAlternative(RModel model)
        {
            InitializeComponent();
            alternativeStatus.Items.AddRange(model.AlternativeStates.ToArray());
        }

        private void createAlternative_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(alternativeName.Text))
            {
                MessageBox.Show("Enter a name for the alternative.");
                this.DialogResult = DialogResult.None;
                return;
            }
            if (alternativeStatus.SelectedIndex > -1)
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
