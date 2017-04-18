using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Planning
{
    class PlanningItemTextComponent : PaddedTextLabel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex regex = new Regex(@"PlanningItemTextComponent(\.\d+)?$");

        public PlanningItemTextComponent(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public PlanningItemTextComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            RationallyType = "planningItemTextComponent";

            Name = "PlanningItemTextComponent";

            AddAction("addPlanningItem", "QUEUEMARKEREVENT(\"add\")", "\"Add item\"", false);
            AddAction("deletePlanningItem", "QUEUEMARKEREVENT(\"delete\")", "\"Delete item\"", false);

            AddUserRow("Index");
            Index = index; 
            Width = 4.3;
            Height = 0.2;
            InitStyle();
        }

        public new void InitStyle()
        {
            MarginTop = 0.1;
            MarginBottom = 0.1;
            MarginLeft = 0.2;
            UsedSizingPolicy = SizingPolicy.FixedSize;
        }

        public static bool IsPlanningItemTextComponent(string name) => regex.IsMatch(name);
        

        public override void Repaint()
        {

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.PlanningItems.Count - 1);
                if (Globals.RationallyAddIn.Model.PlanningItems.Count > Index)
                {
                    PlanningItem item = Globals.RationallyAddIn.Model.PlanningItems[Index];
                    Text = item.ItemText;
                    StrikeThrough = item.Finished;
                }
            }
            base.Repaint();
        }
    }
}
