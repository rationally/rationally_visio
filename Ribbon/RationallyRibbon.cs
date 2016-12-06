using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Rationally.Visio.Enums;
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
                ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Title);
            }
            else
            {
                MessageBox.Show(Messages.Warning_WizardOnlyOnRationallyTemplates_Description,
                            Messages.Warning_WizardOnlyOnRationallyTemplates_Label,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign,
                            false);
            }
        }

        private void wizardButton_Click_1(object sender, RibbonControlEventArgs e)
        {

        }

        private void settingsButton_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
