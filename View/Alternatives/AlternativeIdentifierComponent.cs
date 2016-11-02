using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeIdentifierComponent : TextLabel, IAlternativeComponent
    {
        private static readonly Regex IdentRegex = new Regex(@"AlternativeIdent(\.\d+)?$");
        public AlternativeIdentifierComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            InitStyle();
        }

        public AlternativeIdentifierComponent(Page page, int alternativeIndex, string text) : base(page, text)
        {
            RationallyType = "alternativeIdentifier";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeIdent";

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);
            InitStyle();
        }

        private void InitStyle()
        {
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded;
            Height = 0.2;
            Width = 0.3;
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
            if (Text != (char) (65 + alternativeIndex) + ":")
            {
                Text = (char) (65 + alternativeIndex) + ":";
            }
        }
        public static bool IsAlternativeIdentifier(string name)
        {
            return IdentRegex.IsMatch(name);
        }

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
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //No need to do this during an update, Visio handles this
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
