using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentsContainer : RContainer
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Documents(\.\d+)?$");
        
        public RelatedDocumentsContainer(Page page, Shape relatedDocumentsContainer) : base(page)
        {
            RShape = relatedDocumentsContainer;
            Array ident = relatedDocumentsContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => RelatedDocumentContainer.IsRelatedDocumentContainer(shape.Name)))
            {
                Children.Add(new RelatedDocumentContainer(page, shape));
            }
            Children = Children.OrderBy(c => c.DocumentIndex).ToList();
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (RelatedDocumentContainer.IsRelatedDocumentContainer(s.Name))
            {
                RelatedDocumentContainer con = new RelatedDocumentContainer(Page, s);
                Children.Insert(con.DocumentIndex, con);
            }
            else
            {
                Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
            }
        }


        public void InitStyle()
        {
            //MakeListItem();
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public static bool IsRelatedDocumentsContainer(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
