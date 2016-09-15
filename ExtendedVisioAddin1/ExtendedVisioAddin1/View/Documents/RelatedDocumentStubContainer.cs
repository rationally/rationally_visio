using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedDocumentStubContainer : RComponent
    {

        public override int DocumentIndex
        {
            get;
            set;
        }

        public RelatedDocumentStubContainer(Page page, int documentIndex) : base(page)
        {
            DocumentIndex = documentIndex;
        }

        public override bool ExistsInTree(Shape s)
        {
            return false;
        }
    }
}
