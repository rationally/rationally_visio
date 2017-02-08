using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeStubContainer : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override int AlternativeIndex
        {
            get; protected set;
        }

        public AlternativeStubContainer(Page page,int alternativeIndex) : base(page)
        {
            AlternativeIndex = alternativeIndex;
        }

        public override bool ExistsInTree(Shape s) => false;
    }
}
