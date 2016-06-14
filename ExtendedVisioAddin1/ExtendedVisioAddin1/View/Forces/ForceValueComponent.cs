using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceValueComponent : RComponent
    {
        private static readonly Regex ForceValueRegex = new Regex(@"ForceValue(\.\d+)?$");
        
        public ForceValueComponent(Page page, string alternativeIdentifier, int forceIndex) : base(page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifier = "";

            AddUserRow("forceIndex");
            ForceIndex = forceIndex;

            AddUserRow("rationallyType");
            RationallyType = "forceValue";
            Name = "ForceValue";

            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "\"Add force\"", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this force\"", false);

            Width = 1.0 / 2.54;
            Height = 0.33;
            Text = "0";
            ToggleBoldFont(true);

            AlternativeIdentifier = alternativeIdentifier;
        }

        public ForceValueComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        public static bool IsForceValue(string name)
        {
            return ForceValueRegex.IsMatch(name);
        }

        private void UpdateReorderFunctions()
        {
            if (ForceIndex > 0)
            {
                AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            }

            if (ForceIndex < Globals.ThisAddIn.Model.Forces.Count - 1)
            {
                AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);
            }
        }

        public override void Repaint()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions();
            }
            string toParse = Text.StartsWith("+") ? Text.Substring(1) : Text;
            int value;
            int.TryParse(toParse, out value);

            if (value < 0)
            {
                BackgroundColor = "RGB(153,12,0)";
                FontColor = "RGB(255,255,255)";
            }
            else if (value > 0)
            {
                BackgroundColor = "RGB(0,175,0)";
                FontColor = "RGB(255,255,255)";
            }
            else
            {
                BackgroundColor = "RGB(255,255,255)";
                FontColor = "RGB(89,131,168)";
            }
            base.Repaint();
        }



    }
}

