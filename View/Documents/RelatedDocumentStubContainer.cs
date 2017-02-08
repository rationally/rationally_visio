using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedDocumentStubContainer : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override int DocumentIndex
        {
            get;
            set;
        }

        public RelatedDocumentStubContainer(Page page, int documentIndex) : base(page)
        {
            DocumentIndex = documentIndex;
        }

        public override bool ExistsInTree(Shape s) => false;
    }
}
