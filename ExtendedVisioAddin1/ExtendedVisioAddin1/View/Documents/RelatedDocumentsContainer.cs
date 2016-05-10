using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class RelatedDocumentsContainer : RContainer
    {
        public RelatedDocumentsContainer(Page page) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.Model.RationallyDocument.Masters["Related Documents"];
            RShape = Page.DropContainer(containerMaster, null);
            CenterX = 12;
            CenterY = 8;
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public RelatedDocumentsContainer(Page page, Shape relatedDocumentsContainer) : base(page)
        {
            RShape = relatedDocumentsContainer;
            Array ident = relatedDocumentsContainer.ContainerProperties.GetMemberShapes(16);
            /*//TODO extract, change
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => alternativeRegex.IsMatch(shape.Name)))
            {
                Children.Add(new AlternativeContainer(page, shape));
            }*/

            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public void InitStyle()
        {
            MakeListItem();
        }
    }
}
