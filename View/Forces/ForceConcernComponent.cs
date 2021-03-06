﻿using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Forces
{
    internal sealed class ForceConcernComponent : VisioShape
    {
        private static readonly Regex ForceConcernRegex = new Regex(@"ForceConcern(\.\d+)?$");
        public const string DefaultConcern = "<<concern>>";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ForceConcernComponent(Page page, int index) : base(page)
        {
            
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            Shape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();
            
            RationallyType = "forceConcern";
            Name = "ForceConcern";
            
            Index = index;

            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "Add force", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "Delete this force", false);
            
            InitStyle();
        }

        public ForceConcernComponent(Page page, Shape shape) : base(page)
        {
            Shape = shape;
        }

        private void InitStyle()
        {
            Width = 1;
            Height = 0.33;
            Text = DefaultConcern;
            LineColor = "RGB(89,131,168)";
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
        }

        public static bool IsForceConcern(string name) => ForceConcernRegex.IsMatch(name);
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Forces.Count - 1);
                if (Text != Globals.RationallyAddIn.Model.Forces[Index].Concern)
                {
                    Text = Globals.RationallyAddIn.Model.Forces[Index].Concern;
                }
            }
            base.Repaint();
        }
    }
}
