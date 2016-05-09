using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeDescriptionComponent : HeaderlessContainer, IAlternativeComponent
    {

        public AlternativeDescriptionComponent(Page page, Shape alternativeComponent) : base(page, false)
        {
            RShape = alternativeComponent;
            InitStyle();
        }

        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            Application application = Globals.ThisAddIn.Application;
            InitStyle();

            AddUserRow("rationallyType");
            RationallyType = "alternativeDescription";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeDescription";

            Text = description;

            //Locks
            /*LockDelete = true;
            LockRotate = true;
            LockMoveX = true;
            LockMoveY = true; */
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            this.AlternativeIndex = alternativeIndex;
        }

        public void InitStyle()
        {
            Width = 4;
            Height = 2.5;
            SetMargin(0.2);
        }
    }
}
