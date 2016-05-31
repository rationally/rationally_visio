using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class MoveUpAlternativeHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape changedShape, string identifier)
        {
            AlternativeContainer toChange = new AlternativeContainer(Globals.ThisAddIn.Application.ActivePage, changedShape);
            int currentIndex = toChange.AlternativeIndex;
            //locate the alternative to swap with
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
            AlternativeContainer other = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU["User.alternativeIndex"].ResultIU == currentIndex - 1);

            //swap the item to move with the one below
            model.Alternatives.Move(currentIndex, currentIndex - 1);
            //update the index of the component and his children
            toChange.SetAlternativeIdentifier(currentIndex - 1);
            //same, for the other component
            other.SetAlternativeIdentifier(currentIndex);

            //swap the elements
            RComponent temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex - 1];
            alternativesContainer.Children[currentIndex - 1] = temp;

            
            new RepaintHandler();
        }
    }
}
