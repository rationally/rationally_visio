using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Stakeholders
{
    class StakeholdersContainer : RationallyContainer
    {
        private static readonly Regex StakeholdersRegex = new Regex(@"Stakeholders(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public StakeholdersContainer(Page page, Shape stakeholderContainer) : base(page)
        {
            RShape = stakeholderContainer;
            Array ident = stakeholderContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => StakeholderContainer.IsStakeholderContainer(shape.Name)))
            {
                Children.Add(new StakeholderContainer(page, shape));
            }
            Children = Children.OrderBy(c => c.Index).ToList();
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        private void InitStyle() => UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded;

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) { RShape = s };

            if (StakeholderContainer.IsStakeholderContainer(s.Name))
            {
                if (Children.All(c => c.Index != shapeComponent.Index)) //there is no stakeholder stub with this index
                {
                    Children.Add(new StakeholderContainer(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    StakeholderStubContainer stub = (StakeholderStubContainer)Children.First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    StakeholderContainer con = new StakeholderContainer(Page, s);
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
                bool isStakeholderChild = StakeholderNameComponent.IsStakeholderName(s.Name) || StakeholderRoleComponent.IsStakeholderRole(s.Name);

                if (isStakeholderChild && Children.All(c => c.Index != shapeComponent.Index)) //if parent not exists
                {
                    StakeholderStubContainer stub = new StakeholderStubContainer(Page, shapeComponent.Index);
                    Children.Insert(stub.Index, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }

        public static bool IsStakeholdersContainer(string name) => StakeholdersRegex.IsMatch(name);

        public void RemoveStakeholdersFromModel()
        {
            //Get a list of all stakeholderIndices
            List<int> indexList = Children.Select(stakeholder => stakeholder.Index).ToList();
            indexList.Sort();
            indexList.Reverse(); //Reverse so indices don't change
            foreach (int index in indexList)
            {
                Globals.RationallyAddIn.Model.Stakeholders.RemoveAt(index);
            }
        }

        public void AddStakeholder(string name,string role)
        {
            PleaseWait pleaseWait = new PleaseWait();
            pleaseWait.Show();
            pleaseWait.Refresh();
            Stakeholder stakeholder = new Stakeholder(name,role);
            Globals.RationallyAddIn.Model.Stakeholders.Add(stakeholder);
            Children.Add(new StakeholderContainer(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Stakeholders.Count - 1, stakeholder));//assumes stakeholder is already in the model
            RepaintHandler.Repaint(this);
            pleaseWait.Hide();
        }
    }
}
