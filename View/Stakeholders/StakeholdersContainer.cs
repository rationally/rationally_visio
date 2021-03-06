﻿using System;
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
    internal class StakeholdersContainer : RationallyContainer
    {
        private static readonly Regex StakeholdersRegex = new Regex(@"Stakeholders(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public StakeholdersContainer(Page page, Shape stakeholderContainer) : base(page)
        {
            Shape = stakeholderContainer;
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
            VisioShape shapeComponent = new VisioShape(Page) { Shape = s };

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
            Children.Add(new StakeholderContainer(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Stakeholders.Count - 1, stakeholder, stakeholder.Id));//assumes stakeholder is already in the model
            RepaintHandler.Repaint(this);
            pleaseWait.Hide();
        }

        public override void Repaint()
        {
            List<VisioShape> toDelete = Children.Where(stake => !Globals.RationallyAddIn.Model.Stakeholders.Select(sth => sth.Id).Contains(stake.Id)).ToList();
            if (Globals.RationallyAddIn.Model.Stakeholders.Count > Children.Count)
            {
                Globals.RationallyAddIn.Model.Stakeholders
                    .Where(sth => Children.Count == 0 || Globals.RationallyAddIn.Model.Stakeholders.IndexOf(sth) > Children.Last().Index).ToList()
                    .ForEach(sth =>
                        Children.Add(new StakeholderContainer(Globals.RationallyAddIn.Application.ActivePage, Children.Count, sth, sth.Id))
                    );
            }
            toDelete.ForEach(doc => doc.Shape.Delete());
            base.Repaint();
        }
    }
}
