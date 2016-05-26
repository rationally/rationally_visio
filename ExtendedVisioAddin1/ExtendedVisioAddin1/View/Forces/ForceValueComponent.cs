using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceValueComponent : RComponent
    {
        private static readonly Regex ForceValueRegex = new Regex(@"ForceValue(\.\d+)?$");

        public string AlternativeIdentifier
        {
            get { return RShape.CellsU["alternativeIdentifier"].ResultStr["Value"]; }
            set { RShape.Cells["User.alternativeIdentifier.Value"].Formula = "\"" + value + "\""; }
        }

        public ForceValueComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            this.AddUserRow("alternativeIdentifier");
            this.AlternativeIdentifier = "";

            this.AddUserRow("rationallyType");
            this.RationallyType = "forceValue";
            Name = "ForceValue";

            this.Width = (1.0/2.54);
            this.Height = 0.33;
            this.Text = "0";
            this.ToggleBoldFont(true);
        }

        public ForceValueComponent(Page page, string alternativeIdentifier) : this(page)
        {
            this.AlternativeIdentifier = alternativeIdentifier;
        }

        public ForceValueComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        public static bool IsForceValue(string name)
        {
            return ForceValueRegex.IsMatch(name);
        }
    }
}
