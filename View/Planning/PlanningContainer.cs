using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.View.Planning
{
    class PlanningContainer : RationallyContainer
    {
        private static readonly Regex PlanningRegex = new Regex(@"Planning(\.\d+)?$");

        public PlanningContainer(Page page, Shape planningContainer) : base(page)
        {
            RShape = planningContainer;
            Array ident = planningContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => PlanningItem.IsPlanningItem(shape.Name)))
            {
                Children.Add(new PlanningItem(page, shape));
            }
            Children = Children.OrderBy(c => c.Index).ToList();
            LayoutManager = new VerticalStretchLayout(this);
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) { RShape = s };

            if (PlanningItem.IsPlanningItem(s.Name))
            {
                if (Children.All(c => c.Index != shapeComponent.Index)) //there is no forcecontainer stub with this index
                {
                    Children.Add(new PlanningItem(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    PlanningStubItem stub = (PlanningStubItem)Children.First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    PlanningItem con = new PlanningItem(Page, s);
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
            pleaseWait.Refresh();//TODO model stuff
            //Alternative newAlternative = new Alternative(title, state);
            //newAlternative.GenerateIdentifier(Globals.RationallyAddIn.Model.Alternatives.Count);
            //Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);
            Children.Add(new PlanningItem(Globals.RationallyAddIn.Application.ActivePage));
            RepaintHandler.Repaint();
            pleaseWait.Hide();
        }

        public override void Repaint()
        {
            //remove alternatives that are no longer in the model, but still in the view
            /*List<RationallyComponent> toDelete = Children.Where(alternative => !Globals.RationallyAddIn.Model.Alternatives.Select(alt => alt.Id).Contains(alternative.Id)).ToList();


            if (Globals.RationallyAddIn.Model.Alternatives.Count > Children.Count)
            {
                Globals.RationallyAddIn.Model.Alternatives
                    .Where(alt => Children.Count == 0 || Children.All(c => c.Id != alt.Id)).ToList()
                    .ForEach(alt => {
                        alt.GenerateIdentifier(Children.Count);
                        Children.Add(new AlternativeContainer(Globals.RationallyAddIn.Application.ActivePage, Children.Count, alt));
                    });
            }


            toDelete.ForEach(alt => alt.RShape.Delete());*/
            base.Repaint();
        }

        public static bool IsPlanningContainer(string name) => PlanningRegex.IsMatch(name);
    }
}
