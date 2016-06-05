using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    internal class EditAlternativeStateEventHandler : MarkerEventHandler
    {
        public override void Execute(RModel model, Shape s, string newState)
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
