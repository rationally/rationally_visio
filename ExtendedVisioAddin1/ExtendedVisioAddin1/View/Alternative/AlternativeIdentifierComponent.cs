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
        private IVShape alternativeComponent;

        public AlternativeIdentifierComponent(Page page, IVShape alternativeComponent) : base(page, alternativeComponent.Text, alternativeComponent.Characters.CharPropsRow[])
        {
            this.RShape = alternativeComponent;
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
        }
    }
}
