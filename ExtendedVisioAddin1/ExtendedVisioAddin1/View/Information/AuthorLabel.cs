using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{ 
    class AuthorLabel : TextLabel
    {
        private static readonly Regex AuthorRegex = new Regex(@"AuthorLabel(\.\d+)?$");

        public AuthorLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
        }

        public AuthorLabel(Page page, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "informationVersion";

            Name = "InformationVersion";
        }

        public override void Repaint()
        {
            this.Text = Globals.RationallyAddIn.Model.Author;
            base.Repaint();
        }

        public static bool IsAuthorLabel(string name)
        {
            return AuthorRegex.IsMatch(name);
        }
    }
}
