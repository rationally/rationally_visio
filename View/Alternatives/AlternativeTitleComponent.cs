﻿using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeTitleComponent : TextLabel, IAlternativeComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex TitleRegex = new Regex(@"AlternativeTitle(\.\d+)?$");
        public AlternativeTitleComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            Shape = alternativeComponent;
            InitStyle();
        }


        public AlternativeTitleComponent(Page page, int index, string text) : base(page, text)
        {
            RationallyType = "alternativeTitle";
            AddUserRow("index");
            Index = index;

            Name = "AlternativeTitle";

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);
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

        public void SetAlternativeIdentifier(int alternativeIndex) => Index = alternativeIndex;

        public static bool IsAlternativeTitle(string name) => TitleRegex.IsMatch(name);
        

        public override void Repaint()
        {

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Alternatives.Count - 1);
                if (Globals.RationallyAddIn.Model.Alternatives.Count > Index)
                {
                    Alternative alternative = Globals.RationallyAddIn.Model.Alternatives[Index];
                    Text = alternative.Title;
                }
            }
            base.Repaint();
        }
    }
}
