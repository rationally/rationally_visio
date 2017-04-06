using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForceComponentEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForcesContainer forcesContainer = (ForcesContainer)view.Children.First(c => c is ForcesContainer);
            foreach (ForceContainer forceContainer in forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList()) //find all candidate containers
            {
                if ((forceContainer.Children.Where(c => c.Shape.Equals(changedShape)).ToList().Count > 0) && !forceContainer.Deleted)//find the right container of changedShape and the container was not part of the selection at the querycancel shapedelete event
                {
                    forceContainer.Deleted = true;
                    forceContainer.Shape.Delete();
                }
            }
        }
    }
}
