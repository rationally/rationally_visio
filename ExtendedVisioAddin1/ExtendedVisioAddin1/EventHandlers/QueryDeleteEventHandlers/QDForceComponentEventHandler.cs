using System.Linq;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDForceComponentEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            ForcesContainer forcesContainer = (ForcesContainer)view.Children.First(c => c is ForcesContainer);
            foreach (ForceContainer forceContainer in forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList()) //find all candidate containers
            {
                if (forceContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0 && !forceContainer.Deleted)//find the right container of changedShape and the container was not part of the selection at the querycancel shapedelete event
                {
                    forceContainer.Deleted = true;
                    forceContainer.RShape.Delete();
                }
            }
        }
    }
}
