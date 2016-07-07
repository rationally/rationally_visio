using System.Linq;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeContainerEventHander : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            RComponent comp = view.Children.Find(x => x is AlternativesContainer);
            if (comp is AlternativesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
