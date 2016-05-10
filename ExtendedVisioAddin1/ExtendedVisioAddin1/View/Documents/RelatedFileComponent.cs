using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedFileComponent : RComponent
    {
        public RelatedFileComponent(Page page, string filePath) : base(page)
        {
            /*Document basicShapes = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss",(short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicShapes.Masters["Rectangle"];*/
            //RShape = page.Drop(rectMaster, 0, 0);
            RShape = page.InsertFromFile(filePath, (short)VisInsertObjArgs.visInsertLink | (short)VisInsertObjArgs.visInsertIcon);
            RShape.Name = "relatedFile";
            AddUserRow("rationallyType");
            RationallyType = "relatedFile";
            //basicShapes.Close();
            SetMargin(0.2);
        }
    }
}
