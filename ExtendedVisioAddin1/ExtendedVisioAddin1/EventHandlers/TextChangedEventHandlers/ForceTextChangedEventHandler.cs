using System.Linq;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.TextChangedEventHandlers
{
    internal class ForceTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            ForcesContainer forcesContainer = (ForcesContainer)view.Children.First(c => c is ForcesContainer);

            ForceValueComponent forceValue = (ForceValueComponent)view.GetComponentByShape(changedShape);
            new RepaintHandler(forceValue); //repaint the force value, for coloring
            ForceTotalsRow forceTotalsRow = forcesContainer.Children.First(c => c is ForceTotalsRow) as ForceTotalsRow;
            if (forceTotalsRow != null) new RepaintHandler(forceTotalsRow.Children.Where(c => c is ForceTotalComponent).FirstOrDefault(c => c.AlternativeTimelessId == forceValue.AlternativeTimelessId));
        }
    }
}
