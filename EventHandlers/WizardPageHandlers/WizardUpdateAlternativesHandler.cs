using System.Reflection;
using log4net;
using Rationally.Visio.Forms;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal static class WizardUpdateAlternativesHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Execute(ProjectSetupWizard wizard) => wizard.TableLayoutMainContentAlternatives.AlternativeRows.ForEach(a => a.UpdateModel());
    }
}
