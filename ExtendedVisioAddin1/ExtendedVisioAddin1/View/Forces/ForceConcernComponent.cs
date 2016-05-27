using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceConcernComponent : RComponent
    {
        private static readonly Regex ForceConcernRegex = new Regex(@"ForceConcern(\.\d+)?$");

        public ForceConcernComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("rationallyType");
            RationallyType = "forceConcern";
            Name = "ForceConcern";

            Width = 1;
            Height = 0.33;
            Text = "<<concern>>";
            InitStyle();
        }

        public ForceConcernComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        private void InitStyle()
        {
            LinePattern = 0;
        }

        public static bool IsForceConcern(string name)
        {
            return ForceConcernRegex.IsMatch(name);
        }
    }
}
