using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal class AlternativeIdentifierComponent : TextLabel, IAlternativeComponent
    {
        private static readonly Regex IdentRegex = new Regex(@"AlternativeIdent(\.\d+)?$");
        public AlternativeIdentifierComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            InitStyle();
        }

        public AlternativeIdentifierComponent(Page page, int alternativeIndex, string text) : base(page, text)
        {
            AddUserRow("rationallyType");
            RationallyType = "alternativeIdentifier";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeIdent";

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);
            //Locks
            /*LockDelete = true;
            LockRotate = true;
            LockMoveX = true;
            LockMoveY = true;
            LockHeight = true;
            LockTextEdit = true;
            LockWidth = true;*/
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
            Text = (char)(65 + alternativeIndex) + ":";
        }
        public static bool IsAlternativeIdentifier(string name)
        {
            return IdentRegex.IsMatch(name);
        }
    }
}
