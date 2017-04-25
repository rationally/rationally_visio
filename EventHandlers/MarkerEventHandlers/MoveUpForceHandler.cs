using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveUpForceHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);

            VisioShape currentComponent = new VisioShape(changedShape.ContainingPage) {Shape = changedShape};
            int currentForceIndex = currentComponent.Index;
            int currentChildIndex = currentForceIndex + 1;

            //swap the forces in the model
            Force currentForce = model.Forces[currentForceIndex];
            model.Forces[currentForceIndex] = model.Forces[currentForceIndex - 1];
            model.Forces[currentForceIndex - 1] = currentForce;

            ForceContainer toMove = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().First(c => c.Index == currentForceIndex);
            ForceContainer toSwapWith = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().First(c => c.Index == currentForceIndex - 1);

            //update the index of the component and his children
            toMove.Children.ForEach(c => c.Index = currentForceIndex - 1);
            toMove.Index = currentForceIndex - 1;

            //same, for the other component
            toSwapWith.Children.ForEach(c => c.Index = currentForceIndex);
            toSwapWith.Index = currentForceIndex;

            VisioShape temp = forcesContainer.Children[currentChildIndex];
            forcesContainer.Children[currentChildIndex] = forcesContainer.Children[currentChildIndex - 1];
            forcesContainer.Children[currentChildIndex - 1] = temp;

            RepaintHandler.Repaint();
        }
    }
}
