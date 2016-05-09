using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentComponent : RComponent
    {
        public RelatedDocumentComponent(Page page, string filePath) : base(page)
        {
            Document basicShapes = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss",(short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicShapes.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
        }
    }
}
