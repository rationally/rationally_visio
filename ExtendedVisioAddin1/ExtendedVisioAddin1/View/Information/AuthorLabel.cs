using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    internal class AuthorLabel : TextLabel
    {
        private static readonly Regex AuthorRegex = new Regex(@"InformationAuthor(\.\d+)?$");

        public AuthorLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
        }

        public AuthorLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "informationAuthor";

            Name = "InformationAuthor";
        }

        public override void Repaint()
        {
            if (Text != Globals.RationallyAddIn.Model.Author)
            {
                Text = Globals.RationallyAddIn.Model.Author;
            }
            base.Repaint();
        }

        public static bool IsAuthorLabel(string name)
        {
            return AuthorRegex.IsMatch(name);
        }
    }
}
