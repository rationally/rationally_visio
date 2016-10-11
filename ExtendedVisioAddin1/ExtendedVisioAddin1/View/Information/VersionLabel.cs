using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{ 
    class VersionLabel : TextLabel
    {
        private static readonly Regex VersionRegex = new Regex(@"VersionLabel(\.\d+)?$");

        public VersionLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
        }

        public VersionLabel(Page page, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "informationVersion";

            Name = "InformationVersion";
        }

        public override void Repaint()
        {
            this.Text = Globals.ThisAddIn.Model.Version;
            base.Repaint();
        }

        public static bool IsVersionLabel(string name)
        {
            return VersionRegex.IsMatch(name);
        }
    }
}
