using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceDescriptionComponent : RComponent
    {
        private static readonly Regex ForceDescriptionRegex = new Regex(@"ForceDescription(\.\d+)?$");

        public ForceDescriptionComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("rationallyType");
            RationallyType = "forceDescription";
            Name = "ForceDescription";

            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "\"Add force\"", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this force\"", false);

            Width = 2;
            Height = 0.33;
            Text = "<<description>>";
            InitStyle();
        }

        public ForceDescriptionComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        private void InitStyle()
        {

        }

        public static bool IsForceDescription(string name)
        {
            return ForceDescriptionRegex.IsMatch(name);
        }
    }
}
