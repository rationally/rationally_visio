using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    class ForcesContainer : RContainer
    {
        private static readonly Regex ForcesRegex = new Regex(@"Forces(\.\d+)?$");

        public ForcesContainer(Page page, Shape forcesContainer) : base(page)
        {
            RShape = forcesContainer;
            Array ident = forcesContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();

            foreach (Shape shape in shapes)
            {
                if (ForceHeaderRow.IsForceHeaderRow(shape.Name))
                {
                    Children.Add(new ForceHeaderRow(page, shape));
                }
                else
                if (ForceContainer.IsForceContainer(shape.Name))
                {
                    Children.Add(new ForceContainer(page, shape));
                }
                else
                if (ForceTotalsRow.IsForceTotalsRow(shape.Name))
                {
                    Children.Add(new ForceTotalsRow(page, shape));
                }
            }
            //insert header, if it is absent
            if (Children.Count == 0 || !Children.Any(c => c is ForceHeaderRow))
            {
                Children.Insert(0, new ForceHeaderRow(Page));
                

            }
            //insert footer, if it is absent
            if (Children.Count == 0 || !Children.Any(c => c is ForceTotalsRow))
            {
                Children.Add(new ForceTotalsRow(Page));
            }
            else if (Children.Any(c => c is ForceTotalsRow))
            {
                RComponent toMove = Children.First(c => c is ForceTotalsRow);
                int toMoveIndex = Children.IndexOf(toMove);
                RComponent toSwapWith = Children.Last();
                Children[Children.Count - 1] = toMove;
                Children[toMoveIndex] = toSwapWith;
            }

            //fix the order of the force containers, using ForceIndex
            Children = Children.OrderBy(c => (c is ForceHeaderRow ? -1 : (c is ForceTotalsRow ? Children.Count : c.ForceIndex))).ToList();

            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;

            LayoutManager = new VerticalStretchLayout(this);
        }

        public static bool IsForcesContainer(string name)
        {
            return ForcesRegex.IsMatch(name);
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RComponent shapeComponent = new RComponent(Page);
            shapeComponent.RShape = s;

            if (ForceContainer.IsForceContainer(s.Name))
            {
                if (Children.Where(c => c is ForceContainer).All(c => c.ForceIndex != shapeComponent.ForceIndex)) //there is no forcecontainer stub with this index
                {
                    ForceContainer con = new ForceContainer(Page, s);
                    Children.Insert(con.ForceIndex + 1, con); //after header
                }
                else
                {
                    //remove stub, insert s as the shape of the stub wrapper
                    ForceContainer stub = (ForceContainer)Children.Where(c => c is ForceContainer).First(c => c.ForceIndex == shapeComponent.ForceIndex);
                    Children.Remove(stub);
                    stub.RShape.Delete();//NOT deleteEx
                    Children.Add(new ForceContainer(Page, s));

                }
            }
            else
            {
                bool isForceContainerChild = ForceConcernComponent.IsForceConcern(s.Name) || ForceDescriptionComponent.IsForceDescription(s.Name) || ForceValueComponent.IsForceValue(s.Name);

                if (isForceContainerChild && Children.Where(c => c is ForceContainer).All(c => c.ForceIndex != shapeComponent.ForceIndex)) //if parent not exists
                {
                    ForceContainer stub = ForceContainer.GetStub(Page, shapeComponent.ForceIndex);
                    Children.Insert(shapeComponent.ForceIndex + 1, stub); //after header
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                
            }
        }

    }
}
