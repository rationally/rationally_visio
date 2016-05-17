using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentContainer : HeaderlessContainer
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Document(\.\d+)?$");
        public RelatedDocumentContainer(Page page) : base(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentContainer";
            Name = "Related Document";
            InitStyle();
        }

        public RelatedDocumentContainer(Page page, Shape shape) : base(page, false)
        {
            RShape = shape;
            Array ident = shape.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            /*foreach (Shape subShape in shapes.Where(subShape => RelatedDocumentContainer.IsRelatedDocumentContainer(shape.Name)))
            {
                Children.Add(new RelatedDocumentContainer(page, shape));
            }*/
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public void InitStyle()
        {

            LinePattern = 0;//borderless
            SetMargin(0.2);
            MarginTop = 0.3;
            MarginBottom = 0;
            Height = 1;
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public static bool IsRelatedDocumentContainer(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
