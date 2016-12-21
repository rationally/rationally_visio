
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Stakeholders
{
    public class StakeholderNameComponent : TextLabel
    {
        private static readonly Regex NameRegex = new Regex(@"StakeholderName(\.\d+)?$");

        public StakeholderNameComponent(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
            InitStyle();
        }

        public StakeholderNameComponent(Page page, int stakeholderIndex, string labelText) : base(page, labelText)
        {
            RationallyType = "stakeholderName";
            AddUserRow("stakeholderIndex");
            StakeholderIndex = stakeholderIndex;

            Name = "StakeholderName";

            AddAction("addStakeholder", "QUEUEMARKEREVENT(\"add\")", "\"Add stakeholder\"", false);
            AddAction("deleteStakeholder", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this stakeholder\"", false);
            Width = 3.7;
            Height = 0.2;
            InitStyle();
        }

        private void InitStyle()
        {
            HAlign = 0;//Enum is wrong, align left
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
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
                    Text = stakeholder.Name;
                }
            }
            base.Repaint();
        }

        public static bool IsStakeholderName(string name) => NameRegex.IsMatch(name);
    }
}
