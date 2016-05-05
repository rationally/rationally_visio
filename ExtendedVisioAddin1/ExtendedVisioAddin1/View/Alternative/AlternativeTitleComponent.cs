using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeTitleComponent : TextLabel
    {

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
        }
    }
}
