using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    internal class DateLabel : TextLabel
    {
        private static readonly Regex DateRegex = new Regex(@"InformationDate(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public DateLabel(Page page, Shape shape) : base(page, shape)
        {
            Shape = shape;
            InitStyle();
        }

        public DateLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "informationDate";

            Name = "InformationDate";
            InitStyle();
        }

        public override void Repaint()
        {
            if (Text != Globals.RationallyAddIn.Model.DateString && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Text = Globals.RationallyAddIn.Model.DateString;
            }
            base.Repaint();
        }

        private void InitStyle()
        {
            SetMargin(0.01);
            SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
        }


        public static bool IsDateLabel(string name) => DateRegex.IsMatch(name);
    }
}
