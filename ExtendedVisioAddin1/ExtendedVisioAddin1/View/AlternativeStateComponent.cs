using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeStateComponent : TextLabel
    {
        public AlternativeStateComponent(Page page, int alternativeIndex, string state ) : base(page, state)
        {
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            this.RationallyType = "alternativeState";
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            this.AlternativeIndex = alternativeIndex;

            //Events
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, "Action_1", (short)VisRowTags.visTagDefault);
            this.RShape.CellsU["Actions.Action_1.Action"].Formula = "";
            this.RShape.CellsU["Actions.Action_1.Menu"].Formula = "\"Change state\"";
            this.RShape.CellsU["Actions.Action_1.FlyoutChild"].Formula = "FALSE";
            RModel model = Globals.ThisAddIn.model;
            for (int i = 0; i < model.AlternativeStates.Count; i++)
            {
                string stateName = "State_" + i;
                this.RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, stateName, (short)VisRowTags.visTagDefault);
                this.RShape.CellsU["Actions." + stateName + ".Action"].Formula = "QUEUEMARKEREVENT(\"stateChange." + model.AlternativeStates[i] + "\")";
                this.RShape.CellsU["Actions." + stateName + ".Menu"].Formula = "\"" + model.AlternativeStates[i] + "\"";
                this.RShape.CellsU["Actions." + stateName + ".FlyoutChild"].Formula = "TRUE";
            }
        }
    }
}
