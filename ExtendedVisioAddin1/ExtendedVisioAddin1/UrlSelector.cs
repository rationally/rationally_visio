using System;
using System.Windows.Forms;

namespace ExtendedVisioAddin1
{
    public partial class UrlSelecter : Form
    {
        public UrlSelecter()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                MessageBox.Show("Enter an url.");
                DialogResult = DialogResult.None;
                return;
            }else if (!IsValidUrl(urlTextBox.Text))
            {
                MessageBox.Show("Enter a proper url, including the protocol to use.");
                DialogResult = DialogResult.None;
                return;
            }else if (string.IsNullOrWhiteSpace(nameTextbox.Text))
            {
                MessageBox.Show("Enter a name.");
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
