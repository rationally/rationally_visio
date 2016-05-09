using System.Linq;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RView : RContainer, IObserver<RModel>
    {
        public RView(Page page) : base(page)
        {

        }

        public void Notify(RModel model)
        {
            UpdateAlternatives(model);
        }

        private void UpdateAlternatives(RModel model)
        {
            //trace the alternatives container
            
        }

        public void AddAlternative(Alternative alternative)
        {
            //todo: first is wrong, since we don;t know how many
            ((RContainer)Globals.ThisAddIn.View.Children.First(ch => ch is AlternativesContainer)).Children.Add(new AlternativeContainer(Globals.ThisAddIn.Application.ActivePage, Globals.ThisAddIn.Model.Alternatives.Count - 1, alternative));
            new RepaintHandler();
        }

        /// <summary>
        /// Deletes an alternative container from the view.
        /// </summary>
        /// <param name="index">identifier of the alternative.</param>
        public void DeleteAlternative(int index)
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.First(c => c is AlternativesContainer); //todo: we still don't know how many

            AlternativeContainer alternative = (AlternativeContainer) alternativesContainer.Children.First(x => x.AlternativeIndex == index && x is AlternativeContainer);
            alternativesContainer.Children.Remove(alternative);
            alternative.RShape.DeleteEx(0); //deletes the alternative, and it's child components
            int i = 0;
            alternativesContainer.Children.Where(c => c is AlternativeContainer).ToList().ForEach(c => ((AlternativeContainer)c).SetAlternativeIdentifier(i++));

            new RepaintHandler();
        }
    }
}
