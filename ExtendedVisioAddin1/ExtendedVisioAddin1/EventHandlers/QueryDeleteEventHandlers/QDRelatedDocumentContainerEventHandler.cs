using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDRelatedDocumentContainerEventHandler : QueryDeleteEventHandler
    {
        public override void Execute(string eventKey, RView view, Shape changedShape)
        {
            RComponent comp = view.Children.Find(x => x is RelatedDocumentsContainer);
            if (comp is RelatedDocumentsContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
