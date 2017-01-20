using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Forces
{
    internal sealed class ForceStubContainer : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override int ForceIndex
        {
            get;
            set;
        }

        public ForceStubContainer(Page page, int forceIndex) : base(page)
        {
            ForceIndex = forceIndex;
        }

        public override bool ExistsInTree(Shape s)
        {
            return false;
        }
    }
}
