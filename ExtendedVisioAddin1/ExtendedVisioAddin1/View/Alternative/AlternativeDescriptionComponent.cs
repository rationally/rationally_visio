using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeDescriptionComponent : RComponent
    {

        public AlternativeDescriptionComponent(Page page, IVShape alternativeComponent) : base(page)
        {
            this.RShape = alternativeComponent;
        }

        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            Application application = Globals.ThisAddIn.Application;
            Document basicDocument = application.Documents.OpenEx("Basic Shapes.vss", (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Master descRectangleMaster = basicDocument.Masters["Rectangle"];

            this.RShape = page.Drop(descRectangleMaster, 0, 0);

            this.AddUserRow("rationallyType");
            this.RationallyType = "alternativeDescription";
            this.AddUserRow("alternativeIndex");
            this.AlternativeIndex = alternativeIndex;

            this.Name = "AlternativeDescription";

            this.Text = description;
            basicDocument.Close();

            //Locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true; */
        }
    }
}
