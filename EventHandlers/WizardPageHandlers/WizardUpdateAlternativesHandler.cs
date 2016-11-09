using Rationally.Visio.Forms;

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
