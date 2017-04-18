using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.ContextMenu;
using Color = System.Drawing.Color;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeStateShape : VisioShape
    {
        private const string StateMenuEventId = "STATE_MENU";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex StateRegex = new Regex(@"AlternativeState(\.\d+)?$");

        private readonly IDictionary<AlternativeState, ContextMenuItem> menu =
            new Dictionary<AlternativeState, ContextMenuItem>();

        private ContextMenuItem changeStateMenu;

        private AlternativeState state = default(AlternativeState);


        private AlternativeStateShape(Page page, Shape alternativeStateShape) : base(page)
        {
            Shape = alternativeStateShape;
        }

        public AlternativeState State
        {
            get { return state; }
            set
            {
                if (menu.ContainsKey(state))
                {
                    menu[state].IsEnabled = true;
                }
                state = value;
                SetBackgroundColor(state.GetColor());
                Text = state.GetName();
                if (menu.ContainsKey(state))
                {
                    menu[state].IsEnabled = false;
                }
            }
        }

        private void GenerateMenu()
        {
            changeStateMenu = ContextMenuItem.CreateAndRegister(this, StateMenuEventId, Messages.Menu_SetState);
            Enum.GetValues(typeof(AlternativeState)).Cast<AlternativeState>().ToList().ForEach(state =>
            {
                ContextMenuItem menuItem = ContextMenuItem.CreateAndRegister(this, state.GetName().ToUpper(),
                    state.GetName(), true);
                menuItem.Action = () => State = state;
                menu[state] = menuItem;
            });
        }

        public static AlternativeStateShape CreateFromShape(Page page, Shape alternativeStateShape)
        {
            AlternativeStateShape stateShape = new AlternativeStateShape(page, alternativeStateShape);
            stateShape.InitStyle();
            stateShape.GenerateMenu();
            return stateShape;
        }


        public static AlternativeStateShape CreateWithNewShape(Page page, int alternativeIndex,
            AlternativeState state)
        {
            string pathToStencil = Constants.MyShapesFolder + VisioFormulas.HiddenStencil;
            Shape shape = CreateShapeFromStencilMaster(page, pathToStencil, VisioFormulas.AlternativeState_ShapeMaster);

            AlternativeStateShape stateShape = new AlternativeStateShape(page, shape);
            stateShape.GenerateMenu();
            stateShape.AddUserRow("rationallyType");
            stateShape.RationallyType = "alternativeState";
            stateShape.AddUserRow("index");
            stateShape.Index = alternativeIndex;
            stateShape.Name = "AlternativeState";

            stateShape.State = state;
            stateShape.AddActionNew("addAlternative", "QUEUEMARKEREVENT(\"add\")", "Add alternative", false);
            stateShape.AddActionNew("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "Delete this alternative",
                false);
            stateShape.InitStyle();
            return stateShape;
        }


        private void InitStyle()
        {
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
        }


        public static bool IsAlternativeState(string name) => StateRegex.IsMatch(name);

        //TODO should be moved to parent (AlternativeContainer)
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                //undo's should not edit the shape again, visio handles that for us
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Alternatives.Count - 1);
                if (Globals.RationallyAddIn.Model.Alternatives.Count > Index)
                {
                    Alternative alternative = Globals.RationallyAddIn.Model.Alternatives[Index];
                    AlternativeState newAlternativeState;

                    if (Enum.TryParse(alternative.Status, out newAlternativeState))
                    {
                        State = newAlternativeState;
                    }
                }
            }
            base.Repaint();
        }
    }

    internal enum AlternativeState
    {
        Proposed,
        Accepted,
        Rejected,
        Challenged,
        Discarded,
    }

    internal static class AlternativeStateExtensions
    {
        public static Color GetColor(this AlternativeState state)
        {
            switch (state)
            {
                case AlternativeState.Accepted:
                    return Color.FromArgb(0, 175, 0);
                case AlternativeState.Rejected:
                    return Color.FromArgb(153, 12, 0);
                case AlternativeState.Challenged:
                    return Color.FromArgb(255, 173, 21);
                case AlternativeState.Discarded:
                    return Color.FromArgb(155, 155, 155);
                default:
                case AlternativeState.Proposed:
                    return Color.FromArgb(96, 182, 215);
            }
        }

        public static string GetName(this AlternativeState state)
        {
            switch (state)
            {
                case AlternativeState.Accepted:
                    return Messages.AlternativeState_Accepted;
                case AlternativeState.Rejected:
                    return Messages.AlternativeState_Rejected;
                case AlternativeState.Challenged:
                    return Messages.AlternativeState_Challenged;
                case AlternativeState.Discarded:
                    return Messages.AlternativeState_Discarded;
                default:
                case AlternativeState.Proposed:
                    return Messages.AlternativeState_Proposed;
            }
        }
    }
}