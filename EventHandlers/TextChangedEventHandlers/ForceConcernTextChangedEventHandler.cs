using Rationally.Visio.View;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class ForceConcernTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            ForceConcernComponent forceConcern = (ForceConcernComponent)view.GetComponentByShape(changedShape);
            if (forceConcern != null)
            {
                if (forceConcern.Text == string.Empty)
                {
                    forceConcern.Text = ForceConcernComponent.DefaultConcern;
                }
                Globals.RationallyAddIn.Model.Forces[forceConcern.ForceIndex].Concern = forceConcern.Text;
            }
        }
    }
}
