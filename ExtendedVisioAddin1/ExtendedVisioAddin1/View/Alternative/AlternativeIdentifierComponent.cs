using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeIdentifierComponent : TextLabel
    {
        public AlternativeIdentifierComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            InitStyle();
        }

        public AlternativeIdentifierComponent(Page page, int alternativeIndex, string text) : base(page, text)
        {
            this.AddUserRow("rationallyType");
            this.RationallyType = "alternativeIdentifier";
            this.AddUserRow("alternativeIndex");
            this.AlternativeIndex = alternativeIndex;

            this.Name = "AlternativeIdent";
            //Locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true;
            this.LockHeight = true;
            this.LockTextEdit = true;
            this.LockWidth = true;*/
            InitStyle();
        }

        private void InitStyle()
        {
            this.SetMargin(0.1);
        }
    }
}
