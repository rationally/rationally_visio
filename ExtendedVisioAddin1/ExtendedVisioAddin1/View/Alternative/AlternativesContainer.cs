using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativesContainer : RContainer
    {
        public AlternativesContainer(Page page, List<Alternative> alternatives) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.model.RationallyDocument.Masters["Alternatives"];
            RShape = Page.DropContainer(containerMaster, null);
            this.CenterX = 10;
            this.CenterY = 10;

            this.Name = "Alternatives";

            this.MsvSdContainerLocked = false;
            for (int i = 0; i < alternatives.Count; i++)
            {
                AlternativeContainer a = new AlternativeContainer(page, i, alternatives[i]);
                this.Children.Add(a);
            }
            this.UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded & SizingPolicy.ExpandYIfNeeded;
            this.LayoutManager = new InlineLayout(this);
            this.MsvSdContainerLocked = true;
        }

        public AlternativesContainer(Page page, IVShape alternativesContainer) : base(page)
        {
            RShape = alternativesContainer;
            Array ident = alternativesContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[]) ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => shape.Name == "Alternative"))
            {
                this.Children.Add(new AlternativeContainer(page, shape));
            }
            this.UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded & SizingPolicy.ExpandYIfNeeded;
        }
    }
}
