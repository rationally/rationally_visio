using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    internal class DateLabel : TextLabel
    {
        private static readonly Regex DateRegex = new Regex(@"InformationDate(\.\d+)?$");

        public DateLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
        }

        public DateLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "informationDate";

            Name = "InformationDate";
        }

        public override void Repaint()
        {
            Text = Globals.RationallyAddIn.Model.Date;
            base.Repaint();
        }

        public static bool IsDateLabel(string name)
        {
            return DateRegex.IsMatch(name);
        }
    }
}
