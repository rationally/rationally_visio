using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;


namespace Rationally.Visio.View.Planning
{
    class PlanningItemComponent : HeaderlessContainer
    {
        private static readonly Regex regex = new Regex(@"PlanningItem(\.\d+)?$");

        public PlanningItemComponent(Page page, Shape planningItem) : base(page,false)
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

        public PlanningItemComponent(Page page, int index, PlanningItem item) : base(page)
        {
            CheckBoxComponent checkBoxComponent = new CheckBoxComponent(page, index, item.Finished);
            Children.Add(checkBoxComponent);

            PlanningItemTextComponent itemContent = new PlanningItemTextComponent(page,index,item.ItemText);
            Children.Add(itemContent);

            AddUserRow("rationallyType");
            AddUserRow("Index");
            AddUserRow("uniqueId");

            Name = "PlanningItem";
            RationallyType = "planningItem";
            Index = index;
            Id = item.Id;

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
            MarginTop = Index == 0 ? 0.3 : 0.0;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
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

        public void SetPlanningItemIndex(int index)
        {
            Index = index;
            Children.ForEach(c => c.Index = index);
            InitStyle();
        }


        public static bool IsPlanningItem(string name) => regex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (Index == 0) //Top shape can't move up
            {
                DeleteAction("moveUp");
            }

            if (Index == Globals.RationallyAddIn.Model.PlanningItems.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio takes care of this
            {
                UpdateReorderFunctions();
            }
            base.Repaint();
        }
    }
}
