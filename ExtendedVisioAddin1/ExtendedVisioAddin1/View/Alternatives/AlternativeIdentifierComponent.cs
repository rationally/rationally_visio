using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeIdentifierComponent : TextLabel, IAlternativeComponent
    {
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
            SetMargin(0.1);
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            this.AlternativeIndex = alternativeIndex;
            this.Text = (char)(65 + alternativeIndex) + ":";
        }
    }
}
