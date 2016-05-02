using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeDescriptionComponent : RComponent
    {
        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            //TODO SCHAPH MACHEN
            Application application = Globals.ThisAddIn.Application;
            Document basicDocument = application.Documents.OpenEx("Basic Shapes.vss", (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Master descRectangleMaster = basicDocument.Masters["Rectangle"];

            this.RShape = page.Drop(descRectangleMaster, 0, 0);

            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            this.RationallyType = "alternativeDescription";
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            this.AlternativeIndex = alternativeIndex;

            this.Text = description;
            basicDocument.Close();
        }
    }
}
