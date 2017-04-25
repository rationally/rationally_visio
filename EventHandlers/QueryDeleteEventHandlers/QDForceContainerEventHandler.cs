using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForceContainerEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            VisioShape comp = view.Children.Find(x => x is ForcesContainer);
            if (comp is ForcesContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
