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
    internal class MoveDownAlternativeHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape changedShape, string identifier)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);

            VisioShape toChangeComponent = Globals.RationallyAddIn.View.GetComponentByShape(changedShape);
            int currentIndex = toChangeComponent.Index;
            
            AlternativeShape toChange = (AlternativeShape)alternativesContainer.Children.First(c => (int)c.Shape.CellsU[VisioFormulas.Cell_Index].ResultIU == currentIndex);
            //locate the alternative that we are going to swap with
            AlternativeShape other = (AlternativeShape)alternativesContainer.Children.First(c => (int)c.Shape.CellsU[VisioFormulas.Cell_Index].ResultIU == currentIndex + 1);
            
            //swap the items in the model
            model.Alternatives[currentIndex].GenerateIdentifier(currentIndex + 1);
            model.Alternatives[currentIndex + 1].GenerateIdentifier(currentIndex);

            string higherIndex = model.Alternatives[currentIndex].IdentifierString;
            string oldIndex = model.Alternatives[currentIndex + 1].IdentifierString;

            Alternative one = model.Alternatives[currentIndex];
            model.Alternatives[currentIndex] = model.Alternatives[currentIndex + 1];
            model.Alternatives[currentIndex + 1] = one;

            //update the index of the component and his children
            toChange.Index = currentIndex + 1;
            
            //same, for the other component
            other.Index = currentIndex;

            //update the related force column value identifiers
            ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is ForcesContainer);

            //set all force value cells with id "higherIndex" to "temp"
            //set all force value cells with id "oldIndex" to "higherIndex"
            //set all force value cells with id "temp" to "oldIndex"
            forcesContainer?.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && (((ForceValueComponent) fcc).AlternativeIdentifierString == higherIndex)).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifierString = "temp"));
            forcesContainer?.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && (((ForceValueComponent) fcc).AlternativeIdentifierString == oldIndex)).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifierString = higherIndex));
            forcesContainer?.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(fc => fc.Children.Where(fcc => fcc is ForceValueComponent && (((ForceValueComponent) fcc).AlternativeIdentifierString == "temp")).Cast<ForceValueComponent>().ToList().ForEach(fvc => fvc.AlternativeIdentifierString = oldIndex));
            //swap the elements in the view tree
            VisioShape temp = alternativesContainer.Children[currentIndex];
            alternativesContainer.Children[currentIndex] = alternativesContainer.Children[currentIndex + 1];
            alternativesContainer.Children[currentIndex + 1] = temp;

            
            RepaintHandler.Repaint();
        }
    }
}
