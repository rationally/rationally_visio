
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Stakeholders
{
    internal sealed class StakeholderContainer : HeaderlessContainer
    {
        private static readonly Regex StakeholderRegex = new Regex(@"Stakeholder(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public StakeholderContainer(Page page, Shape stakeholder) : base(page, false)
        {
            RShape = stakeholder;
            string name = null;
            string role = null;
            foreach (int shapeIdentifier in stakeholder.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested))
            {
                Shape stakeholderComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (StakeholderNameComponent.IsStakeholderName(stakeholderComponent.Name))
                {
                    StakeholderNameComponent comp = new StakeholderNameComponent(page, stakeholderComponent);
                    Children.Add(comp);
                    name = comp.Text;
                } else if (StakeholderRoleComponent.IsStakeholderRole(stakeholderComponent.Name))
                {
                    StakeholderRoleComponent comp = new StakeholderRoleComponent(page, stakeholderComponent);
                    Children.Add(comp);
                    role = comp.Text;
                }
            }

            if ((name != null) && (role != null))
            {
                Stakeholder newStakeholder = new Stakeholder(name,role);

                if (StakeholderIndex <= Globals.RationallyAddIn.Model.Stakeholders.Count)
                {
                    Globals.RationallyAddIn.Model.Stakeholders.Insert(StakeholderIndex, newStakeholder);
                }
                else
                {
                    Globals.RationallyAddIn.Model.Stakeholders.Add(newStakeholder);
                }
            }

            InitStyle();
        }

        public StakeholderContainer(Page page, int stakeholderIndex, Stakeholder stakeholder) : base(page)
        {

            StakeholderNameComponent nameComponent = new StakeholderNameComponent(page, stakeholderIndex, stakeholder.Name);
            StakeholderRoleComponent roleComponent = new StakeholderRoleComponent(page, stakeholderIndex, stakeholder.Role);

            Children.Add(nameComponent);
            Children.Add(roleComponent);

            Name = "Stakeholder";
            AddUserRow("rationallyType");
            RationallyType = "stakeholder";
            AddUserRow("stakeholderIndex");
            StakeholderIndex = stakeholderIndex;

            //locks
            MsvSdContainerLocked = true;

            //Events
            AddAction("addStakeholder", "QUEUEMARKEREVENT(\"add\")", "\"Add stakeholder\"", false);
            AddAction("deleteStakeholder", "QUEUEMARKEREVENT(\"delete\")", "\"Delete stakeholder\"", false);

            Width = 5.26;

            LinePattern = 0;
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            MarginTop = StakeholderIndex == 0 ? 0.3 : 0.0;
            MarginLeft = 0.01;
            MarginRight = 0.01;
            MarginBottom = 0.1;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
        }

        public override void AddToTree(Shape s, bool allowAddInChildren)
        {
            if (StakeholderNameComponent.IsStakeholderName(s.Name))
            {
                StakeholderNameComponent com = new StakeholderNameComponent(Page, s);
                if (com.StakeholderIndex == StakeholderIndex)
                {
                    Children.Add(com);
                }
            }
            else if (StakeholderRoleComponent.IsStakeholderRole(s.Name))
            {
                StakeholderRoleComponent com = new StakeholderRoleComponent(Page, s);
                if (com.StakeholderIndex == StakeholderIndex)
                {
                    Children.Add(com);
                }
            }
        }

        public void SetStakeholderIndex(int index)
        {
            StakeholderIndex = index;
            Children.ForEach(c => c.StakeholderIndex = index);
            InitStyle();
        }

        public static bool IsStakeholderContainer(string name) => StakeholderRegex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (StakeholderIndex == 0) //Top shape can't move up
            {
                DeleteAction("moveUp");
            }

            if (StakeholderIndex == Globals.RationallyAddIn.Model.Stakeholders.Count - 1)
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
