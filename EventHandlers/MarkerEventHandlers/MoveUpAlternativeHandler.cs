﻿using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Constants;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveUpAlternativeHandler : IMarkerEventHandler
    {
        public void Execute(RationallyModel model, Shape changedShape, string identifier)
        {
            //locate the alternative(component) to move
            RationallyComponent toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.AlternativeIndex;
            //locate the alternative to swap with
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
            AlternativeContainer toChange = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.AlternativeIndex].ResultIU == currentIndex);
            AlternativeContainer other = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.AlternativeIndex].ResultIU == currentIndex - 1);

            string lowerIndex = (char) (65 + currentIndex - 1) + ":";
            string oldIndex = (char)(65 + currentIndex) + ":";
            //swap the item to move with the one below
            //swap the identifiers first
            model.Alternatives[currentIndex].IdentifierString = lowerIndex;
            model.Alternatives[currentIndex-1].IdentifierString = oldIndex;
            Alternative one = model.Alternatives[currentIndex];
            model.Alternatives[currentIndex] = model.Alternatives[currentIndex - 1];
            model.Alternatives[currentIndex - 1] = one;

            //update the index of the component and his children
            toChange.SetAlternativeIdentifier(currentIndex - 1);
            //same, for the other component
            other.SetAlternativeIdentifier(currentIndex);

            //update the related force column value identifiers
            ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is ForcesContainer);

            if (forcesContainer != null)
            {
                //set all force value cells with id "lowerIndex" to "temp"
                //set all force value cells with id "oldIndex" to "lowerIndex"
                //set all force value cells with id "temp" to "oldIndex"
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent) fcc).AlternativeIdentifier == lowerIndex).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = "temp"));
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent) fcc).AlternativeIdentifier == oldIndex).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = lowerIndex));
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && ((ForceValueComponent) fcc).AlternativeIdentifier == "temp").Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifier = oldIndex));
            }
            //swap the elements
            RationallyComponent temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex - 1];
            alternativesContainer.Children[currentIndex - 1] = temp;

            
            RepaintHandler.Repaint();
        }
    }
}