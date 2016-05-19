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

        public RelatedDocumentContainer(Page page, Shape containerShape) : base(page, false)
        {
            RShape = containerShape;
            Array ident = containerShape.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            Shape titleShape = shapes.FirstOrDefault(shape => RelatedDocumentTitleComponent.IsRelatedDocumentTitleContainer(shape.Name));
            this.Children.Add(new RelatedDocumentTitleComponent(page, titleShape));

            Shape fileShape = shapes.FirstOrDefault(shape => RelatedFileComponent.IsRelatedFileComponent(shape.Name));
            if (fileShape != null)
            {
                this.Children.Add(new RelatedFileComponent(page, fileShape));
            }
            else
            {
                Shape urlShape = shapes.FirstOrDefault(shape => RelatedUrlComponent.IsRelatedUrlComponent(shape.Name));
                this.Children.Add(new RelatedUrlComponent(page, urlShape));
            }
            //LayoutManager = new VerticalStretchLayout(this);
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
