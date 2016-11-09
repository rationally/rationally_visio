using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;

namespace Rationally.Visio.EventHandlers.TextChangedEventHandlers
{
    internal class AlternativeTitleTextChangedEventHandler : ITextChangedEventHandler
    {
        public void Execute(RationallyView view, Shape changedShape)
        {
            RationallyComponent alternativeTitleComponent = new RationallyComponent(view.Page) {RShape = changedShape};

            if (Globals.RationallyAddIn.Model.Alternatives.Count <= alternativeTitleComponent.AlternativeIndex) { return;}

            Alternative alternativeToUpdate = Globals.RationallyAddIn.Model.Alternatives[alternativeTitleComponent.AlternativeIndex];
            alternativeToUpdate.Title = alternativeTitleComponent.Text;
        }
    }
}
