using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.View.Planning
{
    internal class PlanningContainer : RationallyContainer
    {
        private static readonly Regex PlanningRegex = new Regex(@"Planning(\.\d+)?$");

        public PlanningContainer(Page page, Shape planningContainer) : base(page)
        {
            Shape = planningContainer;
            Array ident = planningContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => PlanningItemComponent.IsPlanningItem(shape.Name)))
            {
                Children.Add(new PlanningItemComponent(page, shape));
            }
            Children = Children.OrderBy(c => c.Index).ToList();
            LayoutManager = new VerticalStretchLayout(this);
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            VisioShape shapeComponent = new VisioShape(Page) { Shape = s };

            if (PlanningItemComponent.IsPlanningItem(s.Name))
            {
                if (Children.All(c => c.Index != shapeComponent.Index)) //there is no forcecontainer stub with this index
                {
                    Children.Add(new PlanningItemComponent(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    PlanningStubItem stub = (PlanningStubItem)Children.First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    PlanningItemComponent con = new PlanningItemComponent(Page, s);
                    if (Children.Count < con.Index)
                    {
                        Children.Add(con);
                    }
                    else
                    {
                        Children.Insert(con.Index, con);
                    }

                }
            }
            else
            {
                bool isPlanningChild = CheckBoxComponent.IsCheckBoxComponent(s.Name) || PlanningItemTextComponent.IsPlanningItemTextComponent(s.Name);

                if (isPlanningChild && Children.All(c => c.Index != shapeComponent.Index)) //if parent not exists
                {
                    PlanningStubItem stub = new PlanningStubItem(Page, shapeComponent.Index);
                    Children.Insert(stub.Index, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }

        public void AddPlanningItem()
        {
            PleaseWait pleaseWait = new PleaseWait();
            pleaseWait.Show();
            pleaseWait.Refresh();
            PlanningItem newItem = new PlanningItem(Constants.DefaultPlanningItemText, false);
            Globals.RationallyAddIn.Model.PlanningItems.Add(newItem);
            Children.Add(new PlanningItemComponent(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.PlanningItems.Count-1, newItem));
            RepaintHandler.Repaint();
            pleaseWait.Hide();
        }

        public override void Repaint()
        {
            //remove alternatives that are no longer in the model, but still in the view
            List<VisioShape> toDelete = Children.Where(planning => !Globals.RationallyAddIn.Model.PlanningItems.Select(pln => pln.Id).Contains(planning.Id)).ToList();
            
            if (Globals.RationallyAddIn.Model.PlanningItems.Count > Children.Count)
            {
                Globals.RationallyAddIn.Model.PlanningItems
                    .Where(pln => Children.Count == 0 || Children.All(c => c.Id != pln.Id)).ToList()
                    .ForEach(pln => Children.Add(new PlanningItemComponent(Globals.RationallyAddIn.Application.ActivePage, Children.Count, pln)));
            }
            
            toDelete.ForEach(alt => alt.Shape.Delete());
            base.Repaint();
        }

        public static bool IsPlanningContainer(string name) => PlanningRegex.IsMatch(name);
    }
}
