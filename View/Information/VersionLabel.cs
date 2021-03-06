﻿using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    internal class VersionLabel : TextLabel
    {
        private static readonly Regex VersionRegex = new Regex(@"InformationVersion(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public VersionLabel(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public VersionLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "informationVersion";

            Name = "InformationVersion";
            InitStyle();
        }

        public override void Repaint()
        {
            if (Text != Globals.RationallyAddIn.Model.Version && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Text = Globals.RationallyAddIn.Model.Version;
            }
            base.Repaint();
        }

        private void InitStyle()
        {
            SetMargin(0.01);
            SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
        }
        public static bool IsVersionLabel(string name) => VersionRegex.IsMatch(name);
    }
}
