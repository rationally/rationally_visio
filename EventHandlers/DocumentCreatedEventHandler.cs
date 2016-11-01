﻿using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Constants;
using Rationally.Visio.Model;
using Rationally.Visio.WindowsFormPopups;

namespace Rationally.Visio.EventHandlers
{
    internal class DocumentCreatedEventHandler
    {
        public DocumentCreatedEventHandler(IVDocument document)
        {
            if (document.Template.Contains(Constant.TemplateName))
            {
                Globals.RationallyAddIn.Model = new RationallyModel();
                ShowSetupWizard();
            }
        }

        private static void ShowSetupWizard()
        {
            ProjectSetupWizard.Instance.ShowDialog(true);
        }
    }
}