using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    internal class PaddedTextLabel : TextLabel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public PaddedTextLabel(Page page, Shape shape) : base(page, shape)
        {
            InitStyle();
        }

        public PaddedTextLabel(Page page, string labelText) : base(page, labelText)
        {
            InitStyle();
        }

        protected void InitStyle()
        {
            SetMargin(0.01);
            MarginLeft = 0.02;
            SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
        }
    }
}
