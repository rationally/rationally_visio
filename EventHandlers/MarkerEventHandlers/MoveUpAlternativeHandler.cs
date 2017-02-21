using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MoveUpAlternativeHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            //locate the alternative(component) to move
            RationallyComponent toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.AlternativeIndex;
            //locate the alternative to swap with
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
            AlternativeContainer toChange = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.AlternativeIndex].ResultIU == currentIndex);
            AlternativeContainer other = (AlternativeContainer)alternativesContainer.Children.First(c => (int)c.RShape.CellsU[CellConstants.AlternativeIndex].ResultIU == currentIndex - 1);
            
            //swap the item to move with the one below
            //swap the identifiers first
            model.Alternatives[currentIndex].GenerateIdentifier(currentIndex-1);
            model.Alternatives[currentIndex-1].GenerateIdentifier(currentIndex);

            string lowerIndex = model.Alternatives[currentIndex].IdentifierString;
            string oldIndex = model.Alternatives[currentIndex - 1].IdentifierString;

            Alternative one = model.Alternatives[currentIndex];
            model.Alternatives[currentIndex] = model.Alternatives[currentIndex - 1];
            model.Alternatives[currentIndex - 1] = one;

            //update the index of the component and his children
            toChange.SetAlternativeIdentifier(currentIndex - 1);
            //same, for the other component
            other.SetAlternativeIdentifier(currentIndex);

            //update the related force column value identifiers
            ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is ForcesContainer);

            //set all force value cells with id "lowerIndex" to "temp"
            //set all force value cells with id "oldIndex" to "lowerIndex"
            //set all force value cells with id "temp" to "oldIndex"
            forcesContainer?.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && (((ForceValueComponent) fcc).AlternativeIdentifierString == lowerIndex)).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifierString = "temp"));
            forcesContainer?.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && (((ForceValueComponent) fcc).AlternativeIdentifierString == oldIndex)).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifierString = lowerIndex));
            forcesContainer?.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && (((ForceValueComponent) fcc).AlternativeIdentifierString == "temp")).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifierString = oldIndex));
            //swap the elements
            RationallyComponent temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex - 1];
            alternativesContainer.Children[currentIndex - 1] = temp;

            
            RepaintHandler.Repaint();
        }
    }
}
