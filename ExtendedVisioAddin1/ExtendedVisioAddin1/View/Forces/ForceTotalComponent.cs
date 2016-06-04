using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForceTotalComponent : RComponent
    {
        private static readonly Regex ForceTotalComponentRegex = new Regex(@"ForceTotalComponent(\.\d+)?$");
        private Shape shape;

        public string AlternativeIdentifier
        {
            get { return RShape.CellsU["User.alternativeIdentifier"].ResultStr["Value"]; }
            set { RShape.Cells["User.alternativeIdentifier.Value"].Formula = "\"" + value + "\""; }
        }

        public ForceTotalComponent(Page page) : base(page) //TODO make private?
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifier = "";

            AddUserRow("rationallyType");
            RationallyType = "forceTotalComponent";
            Name = "ForceTotalComponent";

            Width = 1.0 / 2.54;
            Height = 0.33;
            Text = "0";
            ToggleBoldFont(true);
        }

        public ForceTotalComponent(Page page, string id) : this(page)
        {
            AlternativeIdentifier = id;
        }

        public ForceTotalComponent(Page page, Shape shape) : this(page)
        {
            RShape = shape;
        }

        public static bool IsForceTotalComponent(string name)
        {
            return ForceTotalComponentRegex.IsMatch(name);
        }

        public override void Repaint()
        {
            int total = 0;
            List<ForceValueComponent> totalCandidates = new List<ForceValueComponent>();

            ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
            //for each forcecontainer, look up the forcevalue related to this' total and store it in totalCandidates
            forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(c => c.Children.Where(d => d is ForceValueComponent).ToList().Cast<ForceValueComponent>().Where(fv => fv.AlternativeIdentifier == AlternativeIdentifier).ToList().ForEach(childForTotal => totalCandidates.Add(childForTotal)));

            foreach (ForceValueComponent fv in totalCandidates)
            {
                string t = fv.Text;
                int v = 0;
                string toParse = fv.Text;
 
                if (t.StartsWith("+"))
                {
                    toParse = t.Substring(1);
                }

                if (int.TryParse(toParse, out v))
                {
                    total += v;
                }

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

            this.Text = total+"";

        }
    }
}
