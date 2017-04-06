﻿using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Forces
{
    internal sealed class ForceDescriptionComponent : VisioShape
    {
        private static readonly Regex ForceDescriptionRegex = new Regex(@"ForceDescription(\.\d+)?$");
        public const string DefaultDescription = "<<Force>>";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ForceDescriptionComponent(Page page, int index) : base(page)
        {
            
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            Shape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("rationallyType");
            RationallyType = "forceDescription";
            Name = "ForceDescription";

            AddUserRow("index");
            Index = index;
            
            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "\"Add force\"", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this force\"", false);
            InitStyle();
        }

        public ForceDescriptionComponent(Page page, Shape shape) : base(page)
        {
            Shape = shape;
        }

        private void InitStyle()
        {
            Width = 2;
            Height = 0.33;
            Text = DefaultDescription;
            LineColor = "RGB(89,131,168)";
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
        }

        public static bool IsForceDescription(string name) => ForceDescriptionRegex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);
            if (Index == 0)
            {
                DeleteAction("moveUp");
            }

            if (Index == Globals.RationallyAddIn.Model.Forces.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions();
                if (Text != Globals.RationallyAddIn.Model.Forces[Index].Description)
                {
                    Text = Globals.RationallyAddIn.Model.Forces[Index].Description;
                }
            }
            base.Repaint();
        }
    }
}
