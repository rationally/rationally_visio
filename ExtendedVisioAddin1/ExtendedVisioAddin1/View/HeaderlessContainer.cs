using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class HeaderlessContainer : RContainer
    {
        public HeaderlessContainer(Page page)
        {
            Application application = Globals.ThisAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Master containerMaster = containerDocument.Masters["Plain"];
            this.Page = page;
            RShape = Page.DropContainer(containerMaster, null);

            containerDocument.Close();
        }
    }
}
