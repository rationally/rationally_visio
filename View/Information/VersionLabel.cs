using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    internal class VersionLabel : TextLabel
    {
        private static readonly Regex VersionRegex = new Regex(@"InformationVersion(\.\d+)?$");

        public VersionLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
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
            if (Text != Globals.RationallyAddIn.Model.Version)
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
