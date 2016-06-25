using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeContainerEventHander : QueryDeleteEventHandler
    {
        public override void Execute(string eventKey, RView view, Shape changedShape)
        {
            RComponent comp = view.Children.Find(x => x is AlternativesContainer);
            if (comp is AlternativesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
