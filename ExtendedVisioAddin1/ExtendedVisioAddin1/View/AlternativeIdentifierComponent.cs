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
        public AlternativeIdentifierComponent(Page page, int alternativeIndex, string text) : base(page, text)
        {
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            this.RationallyType = "alternativeIdentifier";
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            this.AlternativeIndex = alternativeIndex;

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
