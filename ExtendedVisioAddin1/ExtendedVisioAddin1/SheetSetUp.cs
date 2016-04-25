using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rationally_visio
{
    public partial class SheetSetUp : Form
    {
        public SheetSetUp()
        {
            InitializeComponent();
            date.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(decisionName.Text))
            {
                MessageBox.Show("Enter a decision name.");
                this.DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(author.Text))
            {
                MessageBox.Show("Enter an author name.");
                this.DialogResult = DialogResult.None;
                return;
            }
            if (string.IsNullOrWhiteSpace(version.Text))
            {
                MessageBox.Show("Enter a version.");
                this.DialogResult = DialogResult.None;
                return;
            }
            DateTime temp;
            if (DateTime.TryParse(date.Text, out temp))
                this.DialogResult = DialogResult.OK;
            else
            {
                MessageBox.Show("Enter a valid date.");
                this.DialogResult = DialogResult.None;
            }
            return;
        }
    }
}
