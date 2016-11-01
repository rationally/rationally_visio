using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeContainerEventHander : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RationallyView view, Shape changedShape)
        {
            RationallyComponent comp = view.Children.Find(x => x is AlternativesContainer);
            if (comp is AlternativesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
