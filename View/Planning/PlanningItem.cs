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
            AddUserRow("order");
            AddUserRow("uniqueId");

            RationallyType = "planningItem";
            Order = -1;//TODO implement
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
            if (CheckBoxComponent.IsCheckBoxComponent(s.Name))
            {
                CheckBoxComponent com = new CheckBoxComponent(Page, s);
                if (com.Index == Index)//TODO implement index
                {
                    Children.Add(com);
                }
            }
            else if (PlanningItemTextComponent.IsPlanningItemTextComponent(s.Name))
            {
                PlanningItemTextComponent com = new PlanningItemTextComponent(Page, s);
                if (com.Index == Index)//TODO implement index
                {
                    Children.Add(com);
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
