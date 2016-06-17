
using System.Linq;
using ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    //simulates a manual delete on the force container
    internal class StartDeleteForceEventHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string identifier)
        {

            //get the corresponding view tree component
            RComponent forceComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);
            //get his parent, or himself, if changedShape is the container already
            ForcesContainer forcesContainer = ((ForcesContainer) Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer));
            //loop over forcecontainers. Return the one that a child matching changedShape OR forceComponent, for changedShape is the container itself
            ForceContainer forceContainer = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().FirstOrDefault(c => c.Children.Any(x => x.RShape.Equals(changedShape))) ?? (ForceContainer)forceComponent;

            forceContainer.Deleted = true;
            forceContainer.RShape.Delete();

        }
    }
}
