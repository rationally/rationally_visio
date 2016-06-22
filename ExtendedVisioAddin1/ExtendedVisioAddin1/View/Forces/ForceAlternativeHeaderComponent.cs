﻿using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Forces
{
    internal class ForceAlternativeHeaderComponent : RComponent
    {
        private static readonly Regex ForceAlternativeHeaderComponentRegex = new Regex(@"ForceAlternativeHeaderComponent(\.\d+)?$");
        
        private ForceAlternativeHeaderComponent(Page page) : base(page) 
        {
            Document basicDocument = Globals.ThisAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeTimelessId");
            AlternativeTimelessId = -2;//for debugging, to distinguish from default highest of -1

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifier = "";

            AddUserRow("rationallyType");
            RationallyType = "forceAlternativeHeaderComponent";
            Name = "ForceAlternativeHeaderComponent";

            Width = 1.0 / 2.54;
            Height = 0.33;
            Text = "0";
            ToggleBoldFont(true);
            InitStyle();
        }

        private void InitStyle()
        {
            LineColor = "RGB(89,131,168)";
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
        }

        public ForceAlternativeHeaderComponent(Page page, string altId, int id) : this(page)
        {
            AlternativeTimelessId = id;
            AlternativeIdentifier = altId;
            Text = altId;
        }

        public ForceAlternativeHeaderComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        public static bool IsForceAlternativeHeaderComponent(string name)
        {
            return ForceAlternativeHeaderComponentRegex.IsMatch(name);
        }
    }
}
