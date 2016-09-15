using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeStubContainer : RComponent
    {
        public override int AlternativeIndex
        {
            get;
            set;
        }

        public AlternativeStubContainer(Page page,int alternativeIndex) : base(page)
        {
            AlternativeIndex = alternativeIndex;
        }

        public override bool ExistsInTree(Shape s)
        {
            return false;
        }
    }
}
