using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal class AlternativeStateComponent : TextLabel, IAlternativeComponent
    {
        private static readonly Regex StateRegex = new Regex(@"AlternativeState(\.\d+)?$");
        public AlternativeStateComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            RShape= alternativeComponent;
            InitStyle();
        }

        public AlternativeStateComponent(Page page, int alternativeIndex, string state ) : base(page, state)
        {
            AddUserRow("rationallyType");
            RationallyType = "alternativeState";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeState";
            //Events
            SetStateMenu(state);

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
            SetMargin(0.1);
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
        }

        public void SetAlternativeState(string newState)
        {
            Text = newState;
            Globals.ThisAddIn.Model.Alternatives[AlternativeIndex].Status = newState;
            SetStateMenu(newState);
        }

        private void SetStateMenu(string currentState)
        {
            AddAction("changeState", "", "\"Change state\"", false);

            RModel model = Globals.ThisAddIn.Model;
            for (int i = 0; i < model.AlternativeStates.Count; i++)
            {
                string stateName = "State_" + i;
                if (model.AlternativeStates[i] == currentState)
                { //todo: extract to container class
                    if (RShape.CellExistsU["Actions." + stateName + ".Action", 0] != 0)
                    {
                        RShape.DeleteRow((short)VisSectionIndices.visSectionAction, RShape.CellsRowIndex["Actions." + stateName + ".Action"]);
                    }
                    RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, stateName, (short)VisRowTags.visTagDefault);
                    RShape.CellsU["Actions." + stateName + ".Action"].Formula = "QUEUEMARKEREVENT(\"change." + model.AlternativeStates[i] + "\")";
                    RShape.CellsU["Actions." + stateName + ".Menu"].Formula = "\"" + currentState + "\"";
                    RShape.CellsU["Actions." + stateName + ".Disabled"].Formula = true.ToString().ToUpper();
                    RShape.CellsU["Actions." + stateName + ".FlyoutChild"].Formula = true.ToString().ToUpper();
                }
                else
                {
                    AddAction(stateName, "QUEUEMARKEREVENT(\"change." + model.AlternativeStates[i] + "\")", "\"" + model.AlternativeStates[i] + "\"", true);
                }
            }
        }
        public static bool IsAlternativeState(string name)
        {
            return StateRegex.IsMatch(name);
        }
    }
}
