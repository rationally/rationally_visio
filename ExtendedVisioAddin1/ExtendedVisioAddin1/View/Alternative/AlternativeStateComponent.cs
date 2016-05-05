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
        public AlternativeStateComponent(Page page, IVShape alternativeComponent) : base(page, alternativeComponent)
        {
            this.RShape= alternativeComponent;
        }

        public AlternativeStateComponent(Page page, int alternativeIndex, string state ) : base(page, state)
        {
            this.AddUserRow("rationallyType");
            this.RationallyType = "alternativeState";
            this.AddUserRow("alternativeIndex");
            this.AlternativeIndex = alternativeIndex;

            this.Name = "AlternativeState";
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
