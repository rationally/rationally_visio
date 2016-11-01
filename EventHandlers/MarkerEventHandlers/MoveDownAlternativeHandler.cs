using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Constants;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveDownAlternativeHandler : IMarkerEventHandler
    {
        public void Execute(RationallyModel model, Shape changedShape, string identifier)
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);

            RationallyComponent toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.AlternativeIndex;
            
            AlternativeContainer toChange = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.AlternativeIndex].ResultIU == currentIndex);
            //locate the alternative that we are going to swap with
            AlternativeContainer other = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.AlternativeIndex].ResultIU == currentIndex + 1);

            string higherIndex = (char) (65 + currentIndex + 1) + ":";
            string oldIndex = (char) (65 + currentIndex) + ":";
            //swap the items in the model
            model.Alternatives[currentIndex].IdentifierString = higherIndex;
            model.Alternatives[currentIndex + 1].IdentifierString = oldIndex;
            Alternative one = model.Alternatives[currentIndex];
            model.Alternatives[currentIndex] = model.Alternatives[currentIndex + 1];
            model.Alternatives[currentIndex + 1] = one;

            //update the index of the component and his children
            toChange.SetAlternativeIdentifier(currentIndex + 1);
            
            //same, for the other component
            other.SetAlternativeIdentifier(currentIndex);

            //update the related force column value identifiers
            ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is ForcesContainer);
            
            if (forcesContainer != null)
            {
                //set all force value cells with id "higherIndex" to "temp"
                //set all force value cells with id "oldIndex" to "higherIndex"
                //set all force value cells with id "temp" to "oldIndex"
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent) fcc).AlternativeIdentifier == higherIndex).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = "temp"));
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent) fcc).AlternativeIdentifier == oldIndex).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = higherIndex));
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent) fcc).AlternativeIdentifier == "temp").Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = oldIndex));
            }
            //swap the elements in the view tree
            RationallyComponent temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex + 1];
            alternativesContainer.Children[currentIndex + 1] = temp;

            
            RepaintHandler.Repaint();
        }
    }
}
