using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class MoveUpForceHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
            ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);

            RComponent currentComponent = new RComponent(changedShape.ContainingPage);
            currentComponent.RShape = changedShape;
            int currentIndex = currentComponent.ForceIndex;

            //swap the forces in the model
            Force currentForce = model.Forces[currentIndex];
            model.Forces[currentIndex] = model.Forces[currentIndex - 1];
            model.Forces[currentIndex - 1] = currentForce;

            ForceContainer toMove = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().First(c => c.ForceIndex == currentIndex);
            ForceContainer toSwapWith = forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().First(c => c.ForceIndex == currentIndex - 1);

            //update the index of the component and his children
            toMove.Children.ForEach(c => c.ForceIndex = currentIndex - 1);

            //same, for the other component
            toSwapWith.Children.ForEach(c => c.ForceIndex = currentIndex);

            RComponent temp = forcesContainer.Children[currentIndex];
            forcesContainer.Children[currentIndex] = forcesContainer.Children[currentIndex - 1];
            forcesContainer.Children[currentIndex - 1] = temp;

            new RepaintHandler();
        }
    }
}
