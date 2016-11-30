using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class ForceDescriptionTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForceDescriptionComponent forceDescription = (ForceDescriptionComponent)view.GetComponentByShape(changedShape);
            if (forceDescription != null)
            {
                Globals.RationallyAddIn.Model.Forces[forceDescription.ForceIndex].Description = forceDescription.Text;
            }
        }
    }
}
