using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers.MarkerEventHandlers;
using Rationally.Visio.View.ContextMenu;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeDescriptionShape : HeaderlessContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex DescriptionRegex = new Regex(@"AlternativeDescription(\.\d+)?$");
        public AlternativeDescriptionShape(Page page, Shape alternativeComponent) : base(page, false)
        {
            Shape = alternativeComponent;
            MarginLeft = 0.1;
            MarginRight = 0.1;
            MarginBottom = 0.1;
            MarginTop = 0.05;
        }

        public AlternativeDescriptionShape(Page page, int index) : base(page)
        {
            Width = 5.15;
            Height = 2.5;
            InitStyle();
            RationallyType = "alternativeDescription";
            Index = index;

            Name = "AlternativeDescription";

            ContextMenuItem addAlternativeMenuItem = ContextMenuItem.CreateAndRegister(this, VisioFormulas.EventId_AddAlternative, Messages.Menu_AddAlternative);
            addAlternativeMenuItem.Action = () => (new AddAlternativeEventHandler()).Execute(Shape, "add");
            ContextMenuItem removeAlternativeMenuItem = ContextMenuItem.CreateAndRegister(this, VisioFormulas.EventId_DeleteAlternative, Messages.Menu_DeleteAlternative);
            removeAlternativeMenuItem.Action = () => (new MarkerDeleteAlternativeEventHandler()).Execute(Shape, "delete");
        }

        public static bool IsAlternativeDescription(string name) => DescriptionRegex.IsMatch(name);

        private void InitStyle()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Shape.ContainerProperties.ResizeAsNeeded = 0;
            }
            MarginLeft = 0.1;
            MarginRight = 0.1;
            MarginBottom = 0.1;
            MarginTop = 0.05;
        }
        
        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio already handles this for us and does not allow us to do it during an undo
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Alternatives.Count - 1);
            }
            base.Repaint();
        }
    }
}
