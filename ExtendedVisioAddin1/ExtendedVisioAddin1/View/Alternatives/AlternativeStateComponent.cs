using System.Text.RegularExpressions;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeStateComponent : RComponent, IAlternativeComponent
    {
        private static readonly Regex StateRegex = new Regex(@"AlternativeState(\.\d+)?$");
        public AlternativeStateComponent(Page page, Shape alternativeComponent) : base(page)
        {
            RShape= alternativeComponent;
            InitStyle();
        }

        public AlternativeStateComponent(Page page, int alternativeIndex, string state ) : this(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "alternativeState";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeState";
            
            //Update text, and the background accordingly
            RShape.Text = state;
            UpdateBackgroundByState(state);

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);
            SetStateMenu(state);
            InitStyle();
        }

        public void UpdateBackgroundByState(string state)
        {
            switch (state.ToLower())  //Currently hardcoded, could be made user setting in the future
            {
                case "accepted":
                    RShape.CellsU["FillForegnd"].Formula = "RGB(0,175,0)";
                    break;
                case "rejected":
                    RShape.CellsU["FillForegnd"].Formula = "RGB(153,12,0)";
                    break;
                case "proposed":
                    RShape.CellsU["FillForegnd"].Formula = "RGB(96,182,215)";
                    break;
                case "challenged":
                    RShape.CellsU["FillForegnd"].Formula = "RGB(255,173,21)";
                    break;
                case "discarded":
                    RShape.CellsU["FillForegnd"].Formula = "RGB(155,155,155)";
                    break;
                default:
                    RShape.CellsU["FillForegnd"].Formula = "RGB(255,255,255)";
                    break;
            }
        }

        public AlternativeStateComponent(Page page) : base(page)
        {
            string docPath = Globals.ThisAddIn.FolderPath + "RationallyHidden.vssx";
            Document rationallyDocument = Globals.ThisAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden);
            Master rectMaster = rationallyDocument.Masters["Alternative State"];
            RShape = page.Drop(rectMaster, 0, 0);
            rationallyDocument.Close();
        }

        private void InitStyle()
        {
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
        }

        public void SetAlternativeState(string newState)
        {
            Text = newState;
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
                { 
                    if (RShape.CellExistsU["Actions." + stateName + ".Action", 0] != 0)
                    {
                        RShape.DeleteRow((short)VisSectionIndices.visSectionAction, RShape.CellsRowIndex["Actions." + stateName + ".Action"]);
                    }
                    RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, stateName, (short)VisRowTags.visTagDefault);
                    RShape.CellsU["Actions." + stateName + ".Action"].Formula = "QUEUEMARKEREVENT(\"change." + model.AlternativeStates[i] + "\")";
                    RShape.CellsU["Actions." + stateName + ".Menu"].Formula = "\"" + currentState + "\"";
                    RShape.CellsU["Actions." + stateName + ".Disabled"].Formula = true.ToString().ToUpper(); //Current state can't be selected again
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

        public void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (AlternativeIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (AlternativeIndex == Globals.ThisAddIn.Model.Alternatives.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing) //undo's should not edit the shape again, visio handles that for us
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
