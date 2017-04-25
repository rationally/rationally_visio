using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    internal class AuthorLabel : TextLabel
    {
        private static readonly Regex AuthorRegex = new Regex(@"InformationAuthor(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public AuthorLabel(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public AuthorLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "informationAuthor";

            Name = "InformationAuthor";
            InitStyle();
        }

        private void InitStyle()
        {
            SetMargin(0.01);
            SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
        }

        public override void Repaint()
        {
            if (Text != Globals.RationallyAddIn.Model.Author && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Text = Globals.RationallyAddIn.Model.Author;
            }
            base.Repaint();
        }

        public static bool IsAuthorLabel(string name) => AuthorRegex.IsMatch(name);
    }
}
