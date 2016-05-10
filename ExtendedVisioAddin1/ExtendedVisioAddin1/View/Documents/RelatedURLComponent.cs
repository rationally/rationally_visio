
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedUrlComponent : RComponent
    {
        public RelatedUrlComponent(Page page, string url) : base(page)
        {
            Document basicShapes = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicShapes.Masters["Rectangle"]; 
            RShape = page.Drop(rectMaster, 0, 0);
            basicShapes.Close();

            Width = 0.6;
            Height = 0.6;
             //todo: create shappie properly

            Hyperlink link = RShape.AddHyperlink();
            link.Address = url;
            EventDblClick = "HYPERLINK(\"" + url + "\")";

            Name = "RelatedUrl";
            AddUserRow("rationallyType");
            RationallyType = "relatedUrl";
        }
    }
}
