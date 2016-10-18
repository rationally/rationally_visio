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
        }

        public VersionLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "informationVersion";

            Name = "InformationVersion";
        }

        public override void Repaint()
        {
            Text = Globals.RationallyAddIn.Model.Version;
            base.Repaint();
        }

        public static bool IsVersionLabel(string name)
        {
            return VersionRegex.IsMatch(name);
        }
    }
}
