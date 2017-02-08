using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDStakeholderContainerEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            RationallyComponent comp = view.Children.Find(x => x is StakeholdersContainer);
            if (comp is StakeholdersContainer)
            {
                comp.MsvSdContainerLocked = false;
            }
        }
    }
}
