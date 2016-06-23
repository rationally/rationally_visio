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

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);

            //Locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true;*/
            Width = 3.7;
            Height = 0.2;
            InitStyle();
        }

        private void InitStyle()
        {
            
            HAlign = 0;//Enum is wrong, align left
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
            UsedSizingPolicy = SizingPolicy.FixedSize;
            
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
