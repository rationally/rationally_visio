using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentsContainer : RContainer
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Documents(\.\d+)?$");
        public RelatedDocumentsContainer(Page page) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.Model.RationallyDocument.Masters["Related Documents"];
            RShape = Page.DropContainer(containerMaster, null);
            CenterX = 12;
            CenterY = 8;
            Name = "Related Documents";
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public RelatedDocumentsContainer(Page page, Shape relatedDocumentsContainer) : base(page)
        {
            RShape = relatedDocumentsContainer;
            Array ident = relatedDocumentsContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => RelatedDocumentContainer.IsRelatedDocumentContainer(shape.Name)))
            {
                Children.Add(new RelatedDocumentContainer(page, shape));
            }

            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public void InitStyle()
        {
            //MakeListItem();
            this.UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public static bool IsRelatedDocumentsContainer(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
