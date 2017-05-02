using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers.MarkerEventHandlers;
using Rationally.Visio.View.ContextMenu;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeIdentifierShape : TextLabel
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex IdentRegex = new Regex($@"{ShapeNames.AlternativeIdentifier}(\.\d+)?$");

        /// <summary>
        /// Used to retrieve and modify the value of this component.
        /// </summary>
        public override int Index
        {
            get { return base.Index; }
            set
            {
                base.Index = value;
                Text = (char)(65 + value) + ":"; //map c-indexed value to capital alphabetical character (0 => "A:")
            }
        }

        public AlternativeIdentifierShape(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            InitStyle();
        }

        public AlternativeIdentifierShape(Page page, int index, string text) : base(page, text)
        {
            RationallyType = ShapeNames.TypeAlternativeIdentifier;
            Index = index;

            Name = ShapeNames.AlternativeIdentifier;

            ContextMenuItem addAlternativeMenuItem = ContextMenuItem.CreateAndRegister(this, VisioFormulas.EventId_AddAlternative, Messages.Menu_AddAlternative);
            addAlternativeMenuItem.Action = () => (new AddAlternativeEventHandler()).Execute(Shape, "add");
            ContextMenuItem removeAlternativeMenuItem = ContextMenuItem.CreateAndRegister(this, VisioFormulas.EventId_DeleteAlternative, Messages.Menu_DeleteAlternative);
            removeAlternativeMenuItem.Action = () => (new MarkerDeleteAlternativeEventHandler()).Execute(Shape, "delete");
            Height = 0.2;
            Width = 0.3;
            InitStyle();
        }

        private void InitStyle()
        {
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded;
        }

        public static bool IsAlternativeIdentifier(string name) => IdentRegex.IsMatch(name);
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //No need to do this during an update, Visio handles this
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Alternatives.Count - 1);
            }
            base.Repaint();
        }
    }
}
