using Rationally.Visio.View;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDRelatedDocumentContainerEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            RComponent comp = view.Children.Find(x => x is RelatedDocumentsContainer);
            if (comp is RelatedDocumentsContainer)
            {
                comp.MsvSdContainerLocked = false; //Child shapes can now be removed.
            }
        }
    }
}
