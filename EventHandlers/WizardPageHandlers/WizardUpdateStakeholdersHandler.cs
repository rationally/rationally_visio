﻿using System.Collections.Generic;
using System.Reflection;
using log4net;
using Rationally.Visio.Forms;
using Rationally.Visio.Forms.WizardComponents;

namespace Rationally.Visio.EventHandlers.WizardPageHandlers
{
    internal class WizardUpdateStakeholdersHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Execute(ProjectSetupWizard wizard)
        {

            Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
            //validation is done here, so just pick the filled in rows
            List<FlowLayoutStakeholder> filledInPanels = wizard.TableLayoutMainContentStakeholders.Stakeholders;
            filledInPanels.ForEach(filledInPanel => filledInPanel.UpdateModel());
            Log.Debug("filled in panels:" + filledInPanels.Count);
        }
    }
}
