using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class AlternativesContainer : RContainer
    {
        public AlternativesContainer(Page page, List<Alternative> alternatives) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.model.RationallyDocument.Masters["Alternatives"];
            RShape = Page.DropContainer(containerMaster, null);
            CenterX = 3;
            CenterY = 5;

            Name = "Alternatives";

            MsvSdContainerLocked = false;
            for (int i = 0; i < alternatives.Count; i++)
            {
                AlternativeContainer a = new AlternativeContainer(page, i, alternatives[i]);
                Children.Add(a);
            }

            LayoutManager = new VerticalStretchLayout(this);
            MsvSdContainerLocked = true;
            InitStyle();
        }

        public AlternativesContainer(Page page, Shape alternativesContainer) : base(page)
        {
            RShape = alternativesContainer;
            Array ident = alternativesContainer.ContainerProperties.GetMemberShapes(16);
            Regex alternativeRegex = new Regex(@"Alternative(\.\d+)?$");
            List<Shape> shapes = (new List<int>((int[]) ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => alternativeRegex.IsMatch(shape.Name)))
            {
                Children.Add(new AlternativeContainer(page, shape));
            }

            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded;
        }

    }
}
