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
            RShape = shape;
        }

        public PlanningItemTextComponent(Page page, int index, string labelText) : base(page, labelText)
        {
            RationallyType = "planningItemTextComponent";

            Name = "PlanningItemTextComponent";

            AddAction("addPlanningItem", "QUEUEMARKEREVENT(\"add\")", "\"Add item\"", false);
            AddAction("deletePlanningItem", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this item\"", false);

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

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions();
                //if (Globals.RationallyAddIn.Model.Alternatives.Count > Index)
                //{
                    //TODO read content from model
                //}
            }
            base.Repaint();
        }
    }
}
