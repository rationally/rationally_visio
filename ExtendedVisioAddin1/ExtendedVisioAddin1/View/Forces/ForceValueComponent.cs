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

        public ForceValueComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifier = "";

            AddUserRow("rationallyType");
            RationallyType = "forceValue";
            Name = "ForceValue";

            Width = 1.0 / 2.54;
            Height = 0.33;
            Text = "0";
            ToggleBoldFont(true);
        }

        public ForceValueComponent(Page page, string alternativeIdentifier) : this(page)
        {
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

        public override void Repaint()
        {
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
        }

    }
}

