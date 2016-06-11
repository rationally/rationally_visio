using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceValueComponent : RComponent
    {
        private static readonly Regex ForceValueRegex = new Regex(@"ForceValue(\.\d+)?$");

        public string AlternativeIdentifier
        {
            get { return RShape.CellsU["User.alternativeIdentifier"].ResultStr["Value"]; }
            set { RShape.Cells["User.alternativeIdentifier.Value"].Formula = "\"" + value + "\""; }
        }

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
            UpdateReorderFunctions();
            string toParse = Text.StartsWith("+") ? Text.Substring(1) : Text;
            int v;
            int.TryParse(toParse, out v);

            if (v < 0)
            {
                RShape.CellsU["Char.Color"].Formula = "THEMEGUARD(RGB(255,128,0))";
            }
            else if (v > 0)
            {
                RShape.CellsU["Char.Color"].Formula = "THEMEGUARD(RGB(0,255,0))";
            }
            else
            {
                RShape.CellsU["Char.Color"].Formula = "THEMEGUARD(RGB(0,0,0))";
            }
            base.Repaint();
        }



    }
}

