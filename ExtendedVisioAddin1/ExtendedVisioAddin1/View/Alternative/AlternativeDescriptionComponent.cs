using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeDescriptionComponent : HeaderlessContainer
    {

        public AlternativeDescriptionComponent(Page page, Shape alternativeComponent) : base(page, false)
        {
            RShape = alternativeComponent;
        }

        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            Application application = Globals.ThisAddIn.Application;
            Width = 4;
            Height = 2.5;
            SetMargin(0.2);

            AddUserRow("rationallyType");
            RationallyType = "alternativeDescription";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeDescription";

            Text = description;

            //Locks
            /*LockDelete = true;
            LockRotate = true;
            LockMoveX = true;
            LockMoveY = true; */
        }
    }
}
