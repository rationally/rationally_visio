using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            Children = Children.OrderBy(c => c.AlternativeIndex).ToList();
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
            //Get a list of all alternativeIndices
            List<int> indexList = Children.Select(alternative => alternative.AlternativeIndex).ToList();
            indexList.Sort();
            indexList.Reverse(); //Reverse so indices don't change
            foreach (int index in indexList)
            {
                Globals.ThisAddIn.Model.Alternatives.RemoveAt(index);
            }
        }

    }
}
