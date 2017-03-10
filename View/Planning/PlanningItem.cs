using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.View.Planning
{
    class PlanningItem : HeaderlessContainer
    {
        private static readonly Regex regex = new Regex(@"PlanningItem(\.\d+)?$");

        public PlanningItem(Page page, Shape planningItem) : base(page,false)
        {
            RShape = planningItem;

            foreach (int shapeIdentifier in planningItem.ContainerProperties.GetMemberShapes((int) VisContainerFlags.visContainerFlagsExcludeNested))
            {
                Shape planningItemComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (CheckBoxComponent.IsCheckBoxComponent(planningItemComponent.Name))
                {
                    CheckBoxComponent cbComponent = new CheckBoxComponent(page, planningItemComponent);
                    Children.Add(cbComponent);
                }

                if (PlanningItemTextComponent.IsPlanningItemTextComponent(planningItemComponent.Name))
                {
                    PlanningItemTextComponent itemContent = new PlanningItemTextComponent(page, planningItemComponent);
                    Children.Add(itemContent);
                }
            }



            InitStyle();
        }

        public PlanningItem(Page page) : base(page)
        {
            CheckBoxComponent checkBoxComponent = new CheckBoxComponent(page);
            Children.Add(checkBoxComponent);

            PlanningItemTextComponent itemContent = new PlanningItemTextComponent(page,"<<Fill in something that needs to done>>");
            Children.Add(itemContent);

            AddUserRow("rationallyType");
            AddUserRow("Index");
            AddUserRow("uniqueId");

            Name = "PlanningItem";
            RationallyType = "planningItem";
            Index = 0;//TODO implement
            Id = -1;//TODO implement

            Width = 4;
            Height = 0.4;

            //locks
            MsvSdContainerLocked = true;

            //Events
            AddAction("addPlanningItem", "QUEUEMARKEREVENT(\"add\")", "\"Add item\"", false);

            InitStyle();
        }

        private void InitStyle()
        {
            MarginTop = 0.1;
            MarginBottom = 0.1;
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded;
        }



        public override void AddToTree(Shape s, bool allowAddInChildren)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) { RShape = s };

            if (CheckBoxComponent.IsCheckBoxComponent(s.Name))
            {
                if (Children.All(c => c.Index != shapeComponent.Index)) //there is no stub with this index
                {
                    Children.Add(new CheckBoxComponent(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    CheckBoxStubComponent stub = (CheckBoxStubComponent)Children.First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    CheckBoxComponent con = new CheckBoxComponent(Page, s);
                    if (Children.Count < con.Index) //TODO implement index
                    {
                        Children.Add(con);
                    }
                    else
                    {
                        Children.Insert(con.Index, con);
                    }

                }
            }
            else if (PlanningItemTextComponent.IsPlanningItemTextComponent(s.Name))
            {
                PlanningItemTextComponent com = new PlanningItemTextComponent(Page, s);
                if (com.Index == Index) //TODO implement index
                {
                    Children.Add(com);
                }
            }
            else
            {

                if (CheckBoxStateComponent.IsCheckBoxStateComponent(s.Name) && Children.All(c => c.Index != shapeComponent.Index)) //if parent not exists
                {
                    CheckBoxStubComponent stub = new CheckBoxStubComponent(Page, shapeComponent.Index);
                    Children.Insert(stub.Index, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddInChildren));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddInChildren));
                }
            }
            
        }

        public static bool IsPlanningItem(string name) => regex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            /*AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (Index == 0)
            {
                DeleteAction("moveUp");
            }

            if (Index == Globals.RationallyAddIn.Model.Alternatives.Count - 1)
            {
                DeleteAction("moveDown");
            }*/
        }

        public override void Repaint()
        {
            /*if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio takes care of this
            {
                UpdateReorderFunctions();
            }
            if (Children.Count == 4)
            {
                if (!(Children[0] is AlternativeIdentifierComponent))
                {
                    RationallyComponent c = Children.Find(x => x is AlternativeIdentifierComponent);
                    Children.Remove(c);
                    Children.Insert(0, c);
                }
                if (!(Children[1] is AlternativeTitleComponent))
                {
                    RationallyComponent c = Children.Find(x => x is AlternativeTitleComponent);
                    Children.Remove(c);
                    Children.Insert(1, c);
                }
                if (!(Children[2] is AlternativeStateComponent))
                {
                    RationallyComponent c = Children.Find(x => x is AlternativeStateComponent);
                    Children.Remove(c);
                    Children.Insert(2, c);
                }
            }*/
            base.Repaint();
        }
    }
}
