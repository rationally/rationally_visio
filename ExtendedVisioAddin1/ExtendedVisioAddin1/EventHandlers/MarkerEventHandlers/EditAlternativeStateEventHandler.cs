using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class EditAlternativeStateEventHandler : IMarkerEventHandler
    {
        public void Execute(RModel model, Shape s, string newState)
        {
            RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage) { RShape = s };

            int index = c.AlternativeIndex;
            Alternative alternative = model.Alternatives[index];
            alternative.Status = newState;
            AlternativeContainer container = (AlternativeContainer)((AlternativesContainer)Globals.ThisAddIn.View.Children.Find(y => y.Name == "Alternatives")).Children.Find(x => x.AlternativeIndex == index && x is AlternativeContainer);
            AlternativeStateComponent component = (AlternativeStateComponent)container.Children.Find(x => x is AlternativeStateComponent);
            component.SetAlternativeState(newState);
            component.UpdateBackgroundByState(newState);
            new RepaintHandler(container);

        }
    }
}
