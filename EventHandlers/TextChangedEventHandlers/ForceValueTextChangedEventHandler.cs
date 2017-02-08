using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class ForceValueTextChangedEventHandler : ITextChangedEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForcesContainer forcesContainer = (ForcesContainer)view.Children.First(c => c is ForcesContainer);

            ForceValueComponent forceValue = (ForceValueComponent)view.GetComponentByShape(changedShape);
            Globals.RationallyAddIn.Model.Forces[forceValue.ForceIndex].ForceValueDictionary[forceValue.AlternativeUniqueIdentifier] = forceValue.Text;
            RepaintHandler.Repaint(forceValue); //repaint the force value, for coloring
            ForceTotalsRow forceTotalsRow = forcesContainer.Children.First(c => c is ForceTotalsRow) as ForceTotalsRow;
            if (forceTotalsRow != null)
            {
                RepaintHandler.Repaint(forceTotalsRow.Children.Where(c => c is ForceTotalComponent).FirstOrDefault(c => c.AlternativeUniqueIdentifier == forceValue.AlternativeUniqueIdentifier));
            }
        }
    }
}
