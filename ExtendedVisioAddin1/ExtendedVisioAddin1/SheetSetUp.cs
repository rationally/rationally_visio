using System;
using System.Windows.Forms;

namespace ExtendedVisioAddin1
{
    public partial class SheetSetUp : Form
    {
        public SheetSetUp()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            date.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(decisionName.Text))
            {
                MessageBox.Show("Enter a decision name.");
                DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(author.Text))
            {
                MessageBox.Show("Enter an author name.");
                DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(version.Text))
            {
                MessageBox.Show("Enter a version.");
                DialogResult = DialogResult.None;
                return;
            }
            DateTime temp;
            if (DateTime.TryParse(date.Text, out temp))
                DialogResult = DialogResult.OK;
            else
            {
                MessageBox.Show("Enter a valid date.");
                DialogResult = DialogResult.None;
            }
        }
    }
}
