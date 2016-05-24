using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //swap the item to move with the one below
            model.Alternatives.Move(currentIndex, currentIndex - 1);
            //update the index of the component and his children
            toChange.SetAlternativeIdentifier(currentIndex - 1);
            //locate the alternative with the just assigned index and update his index to (index-1)
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);

            AlternativeContainer other = (AlternativeContainer)alternativesContainer.Children.First(c => ((int)c.RShape.CellsU["User.alternativeIndex"].ResultIU) == (currentIndex - 1));
            other.SetAlternativeIdentifier(currentIndex);

            //swap the elements
            RComponent temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex - 1];
            alternativesContainer.Children[currentIndex - 1] = temp;

            
            new RepaintHandler();
        }
    }
}
