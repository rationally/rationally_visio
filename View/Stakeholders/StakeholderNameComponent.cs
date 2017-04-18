
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Stakeholders
{
    public class StakeholderNameComponent : TextLabel
    {
        private static readonly Regex NameRegex = new Regex(@"StakeholderName(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public StakeholderNameComponent(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public StakeholderNameComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            Log.Debug("Starting shapesheet initing of stakeholdernamecontainer.");
            RationallyType = "stakeholderName";
            Log.Debug("Added rationally type");
            AddUserRow("index");
            Index = index;
            Log.Debug("index set");

            Name = "StakeholderName";

            AddAction("addStakeholder", "QUEUEMARKEREVENT(\"add\")", "\"Add stakeholder\"", false);
            AddAction("deleteStakeholder", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this stakeholder\"", false);
            Width = 1.9;
            Height = 0.2;
            InitStyle();
        }

        private void InitStyle()
        {
            HAlign = 0;//Enum is wrong, align left
            MarginLeft = 1;
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
                    Text = stakeholder.Name;
                }
            }
            base.Repaint();
        }

        public static bool IsStakeholderName(string name) => NameRegex.IsMatch(name);
    }
}
