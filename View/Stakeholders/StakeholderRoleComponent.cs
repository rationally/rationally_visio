using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Stakeholders
{
    internal class StakeholderRoleComponent : TextLabel
    {
        private static readonly Regex RoleRegex = new Regex(@"StakeholderRole(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public StakeholderRoleComponent(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public StakeholderRoleComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            RationallyType = "stakeholderRole";
            Index = index;

            Name = "StakeholderRole";
            //NameU = "StakeholderRole";
            AddAction("addStakeholder", "QUEUEMARKEREVENT(\"add\")", "Add stakeholder", false);
            AddAction("deleteStakeholder", "QUEUEMARKEREVENT(\"delete\")", "Delete this stakeholder", false);
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
        

        public override void Repaint()
        {

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Stakeholders.Count - 1);
                if (Globals.RationallyAddIn.Model.Stakeholders.Count > Index)
                {
                    Stakeholder stakeholder = Globals.RationallyAddIn.Model.Stakeholders[Index];
                    Text = stakeholder.Role;
                }
            }
            base.Repaint();
        }

        public static bool IsStakeholderRole(string name) => RoleRegex.IsMatch(name);
    }
}
