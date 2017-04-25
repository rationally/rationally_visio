using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    internal class HeaderlessContainer : RationallyContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected HeaderlessContainer(Page page) : base(page)
        {

            Application application = Globals.RationallyAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short)VisOpenSaveArgs.visOpenHidden);
            Master containerMaster = containerDocument.Masters["Plain"];

            Shape = Page.DropContainer(containerMaster, null);

            Shape.CellsU["User.msvSDHeadingStyle"].ResultIU = 0; //Remove visible header
            containerDocument.Close();
        }

        protected HeaderlessContainer(Page page, bool makeShape) : base(page)
        {
            //Can't overload unless using different parameters.
        }
    }
}
