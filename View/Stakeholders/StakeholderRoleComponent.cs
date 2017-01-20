using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Stakeholders
{
    class StakeholderRoleComponent : TextLabel
    {
        private static readonly Regex RoleRegex = new Regex(@"StakeholderRole(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public StakeholderRoleComponent(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
            InitStyle();
        }

        public StakeholderRoleComponent(Page page, int stakeholderIndex, string labelText) : base(page, labelText)
        {
            RationallyType = "stakeholderRole";
            AddUserRow("stakeholderIndex");
            StakeholderIndex = stakeholderIndex;

            Name = "StakeholderRole";
            //NameU = "StakeholderRole";
            AddAction("addStakeholder", "QUEUEMARKEREVENT(\"add\")", "\"Add stakeholder\"", false);
            AddAction("deleteStakeholder", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this stakeholder\"", false);
            Width = 1.9;
            Height = 0.2;
            InitStyle();
        }

        private void InitStyle()
        {
            HAlign = 0;//Enum is wrong, align left
            MarginLeft = 0.1;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.01;
            UsedSizingPolicy = SizingPolicy.FixedSize;
        }

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

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions();
                if (Globals.RationallyAddIn.Model.Stakeholders.Count > StakeholderIndex)
                {
                    Stakeholder stakeholder = Globals.RationallyAddIn.Model.Stakeholders[StakeholderIndex];
                    Text = stakeholder.Role;
                }
            }
            base.Repaint();
        }

        public static bool IsStakeholderRole(string name) => RoleRegex.IsMatch(name);
    }
}
