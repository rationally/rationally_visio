using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeDescriptionComponent : HeaderlessContainer
    {

        public AlternativeDescriptionComponent(Page page, Shape alternativeComponent) : base(page)
        {
            this.RShape = alternativeComponent;
        }

        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            Application application = Globals.ThisAddIn.Application;
            this.Width = 4;
            this.Height = 2.5;
            this.SetMargin(0.2);

            this.AddUserRow("rationallyType");
            this.RationallyType = "alternativeDescription";
            this.AddUserRow("alternativeIndex");
            this.AlternativeIndex = alternativeIndex;

            this.Name = "AlternativeDescription";

            this.Text = description;

            //Locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true; */
        }
    }
}
