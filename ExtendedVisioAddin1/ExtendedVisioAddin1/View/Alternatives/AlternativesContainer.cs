using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    public class AlternativesContainer : RContainer
    {
        private static readonly Regex AlternativesRegex = new Regex(@"Alternatives(\.\d+)?$");
        
        public AlternativesContainer(Page page, Shape alternativesContainer) : base(page)
        {
            RShape = alternativesContainer;
            Array ident = alternativesContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => AlternativeContainer.IsAlternativeContainer(shape.Name)))
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

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (AlternativeContainer.IsAlternativeContainer(s.Name))
            {
                Children.Add(new AlternativeContainer(Page, s));
            }
        }

        public static bool IsAlternativesContainer(string name)
        {
            return AlternativesRegex.IsMatch(name);
        }

        public void RemoveAlternativesFromModel()
        {
            List<int> indexList = new List<int>();
            foreach (RComponent alternative in Children)
            {
                indexList.Add(alternative.AlternativeIndex);
            }
            indexList.Sort();
            indexList.Reverse();
            foreach (int index in indexList)
            {
                Globals.ThisAddIn.Model.Alternatives.RemoveAt(index);
            }
        }

    }
}
