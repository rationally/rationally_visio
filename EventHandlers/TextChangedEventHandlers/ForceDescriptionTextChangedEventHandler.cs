using System.Linq;
using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class ForceDescriptionTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForceConcernComponent forceDescription = (ForceConcernComponent)view.GetComponentByShape(changedShape);
            Globals.RationallyAddIn.Model.Forces[forceDescription.ForceIndex].Description = forceDescription.Text;
        }
    }
}
