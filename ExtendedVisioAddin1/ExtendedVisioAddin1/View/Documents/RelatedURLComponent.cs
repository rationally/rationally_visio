
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedUrlComponent : RComponent
    {
        public RelatedUrlComponent(Page page, string url, string name) : base(page)
        {
            Document basicShapes = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicShapes.Masters["Rectangle"]; 
            RShape = page.Drop(rectMaster, 0, 0);
             //todo: create shappie properly
            Hyperlink link = RShape.AddHyperlink();
            link.Address = url;
            RShape.CellsU["EventDblClick"].Formula = "HYPERLINK(\"" + url + "\")"; //Hyperlink simply opens the url
            basicShapes.Close();

            Name = "RelatedUrl";
            RationallyType = "relatedUrl";
        }
    }
}
