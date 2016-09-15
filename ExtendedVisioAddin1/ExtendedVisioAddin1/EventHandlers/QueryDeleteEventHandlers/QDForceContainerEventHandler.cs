using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForceContainerEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string rationallyType, RView view, Shape changedShape)
        {
            RComponent comp = view.Children.Find(x => x is ForcesContainer);
            if (comp is ForcesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
