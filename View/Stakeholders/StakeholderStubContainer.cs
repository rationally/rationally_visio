using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Stakeholders
{
    internal class StakeholderStubContainer : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override int Index
        {
            get; set;
        }

        public StakeholderStubContainer(Page page, int index) : base(page)
        {
            Index = index;
        }

        public override bool ExistsInTree(Shape s) => false;
    }
}
