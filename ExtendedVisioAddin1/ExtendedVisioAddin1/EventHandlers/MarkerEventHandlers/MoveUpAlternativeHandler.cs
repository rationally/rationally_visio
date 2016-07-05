using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.MarkerEventHandlers
{
    internal class MoveUpAlternativeHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape changedShape, string identifier)
        {
            //locate the alternative(component) to move
            RComponent toChangeComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.AlternativeIndex;
            //locate the alternative to swap with
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
            AlternativeContainer toChange = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU["User.alternativeIndex"].ResultIU == currentIndex);
            AlternativeContainer other = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU["User.alternativeIndex"].ResultIU == currentIndex - 1);

            string lowerIndex = (char) (65 + currentIndex - 1) + ":";
            string oldIndex = (char)(65 + currentIndex) + ":";
            //swap the item to move with the one below
            //swap the identifiers first
            model.Alternatives[currentIndex].Identifier = lowerIndex;
            model.Alternatives[currentIndex-1].Identifier = oldIndex;
            Alternative one = model.Alternatives[currentIndex];
            model.Alternatives[currentIndex] = model.Alternatives[currentIndex - 1];
            model.Alternatives[currentIndex - 1] = one;

            //update the index of the component and his children
            toChange.SetAlternativeIdentifier(currentIndex - 1);
            //same, for the other component
            other.SetAlternativeIdentifier(currentIndex);

            //update the related force column value identifiers
            ForcesContainer forcesContainer = (ForcesContainer)Globals.ThisAddIn.View.Children.First(c => c is ForcesContainer);
            //set all force value cells with id "lowerIndex" to "temp"
            //set all force value cells with id "oldIndex" to "lowerIndex"
            //set all force value cells with id "temp" to "oldIndex"
            forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent)fcc).AlternativeIdentifier == lowerIndex).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = "temp"));
            forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent)fcc).AlternativeIdentifier == oldIndex).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = lowerIndex));
            forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent)fcc).AlternativeIdentifier == "temp").Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = oldIndex));

            //swap the elements
            RComponent temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex - 1];
            alternativesContainer.Children[currentIndex - 1] = temp;

            
            new RepaintHandler();
        }
    }
}
