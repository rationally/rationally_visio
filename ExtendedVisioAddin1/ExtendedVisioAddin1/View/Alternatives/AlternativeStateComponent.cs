using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    internal class AlternativeStateComponent : TextLabel, IAlternativeComponent
    {
        public AlternativeStateComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            this.RShape= alternativeComponent;
            InitStyle();
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
                if (model.AlternativeStates[i] == state)
                { //todo: extract to container class
                    RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, stateName, (short)VisRowTags.visTagDefault);
                    RShape.CellsU["Actions." + stateName + ".Action"].Formula = "QUEUEMARKEREVENT(\"stateChange." + model.AlternativeStates[i] + "\")";
                    RShape.CellsU["Actions." + stateName + ".Menu"].Formula = "\"" +state+ "\"";
                    RShape.CellsU["Actions." + stateName + ".Disabled"].Formula = true.ToString().ToUpper();
                    RShape.CellsU["Actions." + stateName + ".FlyoutChild"].Formula = true.ToString().ToUpper();
                }
                else
                {
                    this.AddAction(stateName, "QUEUEMARKEREVENT(\"stateChange." + model.AlternativeStates[i] + "\")", "\"" + model.AlternativeStates[i] + "\"", true);
                }
            }

            //locks
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

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            this.AlternativeIndex = alternativeIndex;
        }
    }
}
