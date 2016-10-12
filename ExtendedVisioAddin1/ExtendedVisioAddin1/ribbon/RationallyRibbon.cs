using Microsoft.Office.Tools.Ribbon;
using Rationally.Visio.WindowsFormPopups;

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
            ProjectSetupWizard.Instance.Show();
        }
    }
}
