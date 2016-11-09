using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    //simulates a manual delete on the force container
    internal class StartDeleteForceEventHandler : IMarkerEventHandler
    {
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            //get the corresponding view tree component
            RationallyComponent forceComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            //get his parent, or himself, if changedShape is the container already
            ForcesContainer forcesContainer = (ForcesContainer) Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
            //loop over forcecontainers. Return the one that a child matching changedShape OR forceComponent, for changedShape is the container itself
            ForceContainer forceContainer = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().FirstOrDefault(c => c.Children.Any(x => x.RShape.Equals(changedShape))) ?? (ForceContainer)forceComponent;

            forceContainer.Deleted = true;
            forceContainer.RShape.Delete();
        }
    }
}
