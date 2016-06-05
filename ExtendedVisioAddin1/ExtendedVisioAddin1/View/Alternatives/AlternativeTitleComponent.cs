using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal class AlternativeTitleComponent : TextLabel, IAlternativeComponent
    {
        private static readonly Regex TitleRegex = new Regex(@"AlternativeTitle(\.\d+)?$");
        public AlternativeTitleComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            RShape = alternativeComponent;
            InitStyle();
        }

        public AlternativeTitleComponent(Page page, int alternativeIndex, string text) : base(page, text)
        {
            AddUserRow("rationallyType");
            RationallyType = "alternativeTitle";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeTitle";

            //Locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true;*/
            InitStyle();
        }

        private void InitStyle()
        {
            SetMargin(0.1);
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded;
            Height = 0.3667;
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
        }
        public static bool IsAlternativeTitle(string name)
        {
            return TitleRegex.IsMatch(name);
        }
    }
}
