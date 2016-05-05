using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers
{
    class EditAlternativeStateEventHandler : EventHandler
    {
        public EditAlternativeStateEventHandler(RModel model, string newState)
        {
            Selection selectedComponents = Globals.ThisAddIn.Application.ActiveWindow.Selection;
            foreach (Shape s in selectedComponents)
            {
                RComponent c = new RComponent(Globals.ThisAddIn.Application.ActivePage) {RShape = s};
                if (c.Type == "alternativeState")
                {
                    int index = c.AlternativeIndex;
                    Alternative alternative = model.Alternatives[index];
                    alternative.Status = newState;
                    AlternativeContainer container = (AlternativeContainer)((AlternativesContainer)Globals.ThisAddIn.View.Children.Find(y => y.Name == "Alternatives")).Children.Find(x => x.AlternativeIndex == index && x is AlternativeContainer);
                    AlternativeStateComponent component = (AlternativeStateComponent)container.Children.Find(x => x is AlternativeStateComponent); //TODO: DIT KAN MOOIER
                    component.Text = newState;
                    new RepaintHandler();
                }
            }
        }
    }
}
