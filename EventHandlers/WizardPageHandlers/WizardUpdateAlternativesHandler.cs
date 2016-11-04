

using Rationally.Visio.Forms;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal static class WizardUpdateAlternativesHandler
    {
        public static void Execute(ProjectSetupWizard wizard)
        {
            wizard.TableLayoutMainContentAlternatives.AlternativeRows.ForEach(a => a.UpdateModel());
        }
    }
}
