using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.RationallyConstants;
using Color = System.Drawing.Color;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeStateComponent : RationallyComponent, IAlternativeComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
            Color color = Globals.RationallyAddIn.Model.AlternativeStateColors[state];
            RShape.CellsU["FillForegnd"].Formula = $"RGB({color.R},{color.G},{color.B})";
        }

        private AlternativeStateComponent(Page page) : base(page)
        {
            string docPath = Constants.MyShapesFolder + "\\RationallyHidden.vssx";
            Document rationallyDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden);
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

        public void SetAlternativeIdentifier(int alternativeIndex) => AlternativeIndex = alternativeIndex;

        public void SetAlternativeState(string newState)
        {
            Text = newState;
            SetStateMenu(newState);
        }

        private void SetStateMenu(string currentState)
        {
            AddAction("changeState", "", "\"Change state\"", false);

            RationallyModel model = Globals.RationallyAddIn.Model;
            List<string> alternativeStates = model.AlternativeStateColors.Keys.ToList();
            for (int i = 0; i < model.AlternativeStateColors.Keys.Count; i++)
            {
                string stateName = "State_" + i;
                if (alternativeStates[i] == currentState)
                { 
                    if (RShape.CellExistsU["Actions." + stateName + ".Action", (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                    {
                        RShape.DeleteRow((short)VisSectionIndices.visSectionAction, RShape.CellsRowIndex["Actions." + stateName + ".Action"]);
                    }
                    RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, stateName, (short)VisRowTags.visTagDefault);
                    RShape.CellsU["Actions." + stateName + ".Action"].Formula = "QUEUEMARKEREVENT(\"change." + alternativeStates[i] + "\")";
                    RShape.CellsU["Actions." + stateName + ".Menu"].Formula = "\"" + currentState + "\"";
                    RShape.CellsU["Actions." + stateName + ".Disabled"].Formula = true.ToString().ToUpper(); //Current state can't be selected again
                    RShape.CellsU["Actions." + stateName + ".FlyoutChild"].Formula = true.ToString().ToUpper();
                }
                else
                {
                    AddAction(stateName, "QUEUEMARKEREVENT(\"change." + alternativeStates[i] + "\")", "\"" + alternativeStates[i] + "\"", true);
                }
            }
        }
        public static bool IsAlternativeState(string name) => StateRegex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (AlternativeIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (AlternativeIndex == Globals.RationallyAddIn.Model.Alternatives.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //undo's should not edit the shape again, visio handles that for us
            {
                UpdateReorderFunctions();
                if (Globals.RationallyAddIn.Model.Alternatives.Count > AlternativeIndex)
                {
                    Alternative alternative = Globals.RationallyAddIn.Model.Alternatives[AlternativeIndex];
                    SetAlternativeState(alternative.Status);
                    UpdateBackgroundByState(alternative.Status);
                }
            }
            base.Repaint();
        }
    }
}
