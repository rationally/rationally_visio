using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedURLURLComponent : TextLabel
    {
        private static readonly Regex UrlUrlRegex = new Regex(@"RelatedUrlUrl(\.\d+)?$");

        public RelatedURLURLComponent(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public RelatedURLURLComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedUrlUrl";
            Name = "RelatedUrlUrl";
            AddUserRow("documentIndex");
            DocumentIndex = index;
            InitStyle();
        }

        private void InitStyle()
        {
            Width = 4.2;
            UsedSizingPolicy = SizingPolicy.All;
            SetUsedSizingPolicy(UsedSizingPolicy &= ~SizingPolicy.ExpandXIfNeeded);//we want to remove this one from the policy: AND with everything else on true
        }

        public static bool IsRelatedUrlUrlComponent(string name)
        {
            return UrlUrlRegex.IsMatch(name);
        }
    }
}
