using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal sealed class AlternativeDescriptionComponent : HeaderlessContainer, IAlternativeComponent
    {
        private static readonly Regex DescriptionRegex = new Regex(@"AlternativeDescription(\.\d+)?$");
        public AlternativeDescriptionComponent(Page page, Shape alternativeComponent) : base(page, false)
        {
            RShape = alternativeComponent;
            MarginLeft = 0.1;
            MarginRight = 0.1;
            MarginBottom = 0.1;
            MarginTop = 0.05;
        }

        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            Width = 5.15;
            Height = 2.5;
            InitStyle();

            AddUserRow("rationallyType");
            RationallyType = "alternativeDescription";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeDescription";

            Text = description;

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
        }

        public static bool IsAlternativeDescription(string name)
        {
            return DescriptionRegex.IsMatch(name);
        }

        public void InitStyle()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
            }
            MarginLeft = 0.1;
            MarginRight = 0.1;
            MarginBottom = 0.1;
            MarginTop = 0.05;
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
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing) //Visio already handles this for us and does not allow us to do it during an undo
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
