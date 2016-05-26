using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceDescriptionComponent : RComponent
    {
        private static readonly Regex forceDescriptionRegex = new Regex(@"ForceDescription(\.\d+)?$");

        public ForceDescriptionComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            this.AddUserRow("rationallyType");
            this.RationallyType = "forceDescription";
            Name = "ForceDescription";

            this.Width = 2;
            this.Height = 0.33;
            this.Text = "USE THE FORCE!";
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
            return forceDescriptionRegex.IsMatch(name);
        }
    }
}
