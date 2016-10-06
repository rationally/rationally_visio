using System;
using System.Windows.Forms;

namespace Rationally.Visio
{
    public partial class SheetSetUpFormPopUp : Form
    {
        public SheetSetUpFormPopUp()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            date.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(decisionName.Text))
            {
                MessageBox.Show("Enter a decision name.", "Name missing");
                DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(author.Text))
            {
                MessageBox.Show("Enter an author name.", "Author missing");
                DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(version.Text))
            {
                MessageBox.Show("Enter a version.", "Version missing");
                DialogResult = DialogResult.None;
                return;
            }
            DateTime temp;
            if (DateTime.TryParse(date.Text, out temp))
                DialogResult = DialogResult.OK;
            else
            {
                MessageBox.Show("Enter a valid date.", "Date invalid");
                DialogResult = DialogResult.None;
            }
        }
    }
}
