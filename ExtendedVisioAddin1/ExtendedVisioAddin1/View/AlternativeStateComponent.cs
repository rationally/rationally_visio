using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    internal class AlternativeStateComponent : TextLabel
    {
        public AlternativeStateComponent(Page page, int alternativeIndex, string state ) : base(page, state)
        {
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            this.RationallyType = "alternativeState";
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            this.AlternativeIndex = alternativeIndex;

            //Events
            this.AddAction("changeState", "", "\"Change state\"", false);

            RModel model = Globals.ThisAddIn.model;
            for (int i = 0; i < model.AlternativeStates.Count; i++)
            {
                string stateName = "State_" + i;
                this.AddAction(stateName, "QUEUEMARKEREVENT(\"stateChange." + model.AlternativeStates[i] + "\")", "\"" + model.AlternativeStates[i] + "\"", true);
            }

            //locks
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
