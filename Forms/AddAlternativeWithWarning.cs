using System;
using System.Windows.Forms;
using Rationally.Visio.Model;

namespace Rationally.Visio.WindowsFormPopups
{
    public partial class AddAlternativeWithWarning : Form
    {
        public AddAlternativeWithWarning(RationallyModel model)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            alternativeStatus.Items.AddRange(model.AlternativeStates.ToArray());
            alternativeStatus.SelectedIndex = 0;
        }

        private void createAlternative_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(alternativeName.Text))
            {
                MessageBox.Show("Enter a name for the alternative.", "Name missing");
                DialogResult = DialogResult.None;
                return;
            }
            if (alternativeStatus.SelectedIndex > -1)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Select a status.", "Status missing");
                DialogResult = DialogResult.None;
            }
        }
    }
}
