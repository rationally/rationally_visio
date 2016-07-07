using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal sealed class ForceConcernComponent : RComponent
    {
        private static readonly Regex ForceConcernRegex = new Regex(@"ForceConcern(\.\d+)?$");
        public const string DefaultConcern = "<<concern>>";
        

        public ForceConcernComponent(Page page, int forceIndex) : base(page)
        {
            
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("rationallyType");
            RationallyType = "forceConcern";
            Name = "ForceConcern";

            AddUserRow("forceIndex");
            ForceIndex = forceIndex;


            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "\"Add force\"", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this force\"", false);

            
            InitStyle();
        }

        public ForceConcernComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        private void InitStyle()
        {
            Width = 1;
            Height = 0.33;
            Text = DefaultConcern;
            LineColor = "RGB(89,131,168)";
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
        }

        public static bool IsForceConcern(string name)
        {
            return ForceConcernRegex.IsMatch(name);
        }

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);
            if (ForceIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (ForceIndex == Globals.ThisAddIn.Model.Forces.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
