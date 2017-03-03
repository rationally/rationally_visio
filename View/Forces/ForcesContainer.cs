using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View.Forces
{
    internal class ForcesContainer : RationallyContainer
    {
        private static readonly Regex ForcesRegex = new Regex(@"Evaluation(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ForcesContainer(Page page, Shape forcesContainer) : base(page)
        {
            RShape = forcesContainer;
            Array ident = forcesContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
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
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                //insert header, if it is absent
                if ((Children.Count == 0) || !Children.Any(c => c is ForceHeaderRow))
                {
                    Children.Insert(0, new ForceHeaderRow(Page));
                }
                //insert footer, if it is absent
                if ((Children.Count == 0) || !Children.Any(c => c is ForceTotalsRow))
                {
                    Children.Add(new ForceTotalsRow(Page));
                }
                else if (Children.Any(c => c is ForceTotalsRow))
                {
                    RationallyComponent toMove = Children.First(c => c is ForceTotalsRow);
                    int toMoveIndex = Children.IndexOf(toMove);
                    RationallyComponent toSwapWith = Children.Last();
                    Children[Children.Count - 1] = toMove;
                    Children[toMoveIndex] = toSwapWith;
                }
            }

            //fix the order of the force containers, using ForceIndex
            Children = Children.OrderBy(c => (c is ForceHeaderRow ? -1 : (c is ForceTotalsRow ? Children.Count : c.Index))).ToList();

            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
            LayoutManager = new VerticalStretchLayout(this);
        }

        public static bool IsForcesContainer(string name) => ForcesRegex.IsMatch(name);

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) { RShape = s };

            if (ForceContainer.IsForceContainer(s.Name))
            {
                if (Children.Where(c => c is ForceContainer || c is ForceStubContainer).All(c => c.Index != shapeComponent.Index)) //there is no forcecontainer stub with this index
                {
                    ForceContainer con = new ForceContainer(Page, s);
                    Children.Insert(con.Index + 1, con); //after header
                }
                else
                {
                    //remove stub, insert s as the shape of the stub wrapper
                    ForceStubContainer stub = (ForceStubContainer)Children.Where(c => c is ForceStubContainer).First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    ForceContainer con = new ForceContainer(Page, s);
                    Children.Insert(con.Index + 1, con); //after header

                }
            }
            else
            {
                bool isForceChild = ForceConcernComponent.IsForceConcern(s.Name) || ForceDescriptionComponent.IsForceDescription(s.Name) || ForceValueComponent.IsForceValue(s.Name);

                if (isForceChild && Children.Where(c => c is ForceContainer || c is ForceStubContainer).All(c => c.Index != shapeComponent.Index)) //if parent not exists
                {
                    ForceStubContainer stub = new ForceStubContainer(Page, shapeComponent.Index);
                    Children.Insert(shapeComponent.Index + 1, stub); //after header
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    //default case
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }
        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                List<RationallyComponent> toDelete = new List<RationallyComponent>();
                int i = Children.Count - 2;
                Children.Where(force => force is ForceContainer && !Globals.RationallyAddIn.Model.Forces.Select(frc => frc.Id).Contains(force.Id)).ToList().ForEach(force =>
                {
                    toDelete.Add(force);
                    i--;
                });

                Globals.RationallyAddIn.Model.Forces.Where(modelForce => Children.All(force => !(force is ForceContainer) || modelForce.Id != force.Id)).ToList().ForEach(modelForce =>
                        Children.Add(new ForceContainer(Page, i++, modelForce.Id))
                        );
                toDelete.ForEach(force => force.RShape.Delete());

                if (Children.Any(c => c is ForceTotalsRow))
                {
                    RationallyComponent toMove = Children.First(c => c is ForceTotalsRow);
                    while (toMove != Children.Last())
                    {
                        int toMoveIndex = Children.IndexOf(toMove);
                        RationallyComponent toSwapWith = Children[toMoveIndex + 1];
                        Children[toMoveIndex + 1] = toMove;
                        Children[toMoveIndex] = toSwapWith;
                    }
                }

            }
            base.Repaint();
        }
    }
}
