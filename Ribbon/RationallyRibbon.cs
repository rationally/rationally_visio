using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.Forms;

namespace Rationally.Visio
{
    public partial class RationallyRibbon
    {
        private void RationallyRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            wizardButton.Click += wizardButton_Click;
        }

        private static void wizardButton_Click(object sender, RibbonControlEventArgs e)
        {
            if (Globals.RationallyAddIn.Application.ActiveDocument.Template.Contains(Constants.TemplateName))
            {
                ProjectSetupWizard.Instance.ShowDialog(false);
            }
            else
            {
                MessageBox.Show("You can only open this wizard while working in rationally document.",
                            "Action Not Available",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign,
                            false);
            }
        }
    }
}
