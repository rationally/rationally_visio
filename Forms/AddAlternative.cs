using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.Model;

namespace Rationally.Visio.Forms
{
    internal partial class AddAlternative : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public AddAlternative(RationallyModel model)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            alternativeStatus.Items.AddRange(model.AlternativeStateColors.Keys.ToArray());
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
