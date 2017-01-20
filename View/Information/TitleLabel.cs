using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    public class TitleLabel : TextLabel
    {
        private static readonly Regex NameRegex = new Regex(@"Topic(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TitleLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
        }

        public TitleLabel(Page page, string labelText) : base(page, labelText)
        {
            RationallyType = "decisionName";

            Name = "Topic";
            EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")";
            InitStyle();
        }

        private void InitStyle()
        {
            SetUsedSizingPolicy(SizingPolicy.FixedSize);
            HAlign = 0; //left, since the enum is wrong
            Width = 7.7;
            Height = 0.3056;
            SetFontSize(22);
            CenterX = 4.15;
            CenterY = 22.483;
        }

        public override void Repaint()
        {
            if (Text != Globals.RationallyAddIn.Model.DecisionName)
            {
                Text = Globals.RationallyAddIn.Model.DecisionName;
            }
            base.Repaint();
        }

        public static bool IsTitleLabel(string name) => NameRegex.IsMatch(name);
    }
}