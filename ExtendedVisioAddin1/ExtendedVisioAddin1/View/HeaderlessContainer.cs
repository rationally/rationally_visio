using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    internal class HeaderlessContainer : RContainer
    {
        public HeaderlessContainer(Page page) : base(page)
        {

            Application application = Globals.RationallyAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short)VisOpenSaveArgs.visOpenHidden);
            Master containerMaster = containerDocument.Masters["Plain"];

            RShape = Page.DropContainer(containerMaster, null);

            RShape.CellsU["User.msvSDHeadingStyle"].ResultIU = 0; //Remove visible header
            containerDocument.Close();
        }

        public HeaderlessContainer(Page page, bool makeShape) : base(page)
        {
            //Can't overload unless using different parameters.
        }
    }
}
