using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForcesContainer : RContainer
    {
        private static readonly Regex ForcesRegex = new Regex(@"Forces(\.\d+)?$");

        public ForcesContainer(Page page) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.Model.RationallyDocument.Masters["Forces"];
            RShape = Page.DropContainer(containerMaster, null);
            CenterX = 12.875;
            CenterY = 8.375;

            Name = "Forces";
            
            InitStyle();
        }

        public ForcesContainer(Page page, Shape forcesContainer) : base(page)
        {
            RShape = forcesContainer;
            Array ident = forcesContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => ForceContainer.IsForceContainer(shape.Name)))
            {
                Children.Add(new ForceContainer(page, shape));
            }
            InitStyle();
        }

        private void InitStyle()
        {
            LayoutManager = new VerticalStretchLayout(this);
        }

        public static bool IsForcesContainer(string name)
        {
            return ForcesRegex.IsMatch(name);
        }
    }
}
