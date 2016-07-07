using System.Linq;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeComponentEventHandler : IQueryDeleteEventHandler
    {
        public void Execute(string eventKey, RView view, Shape changedShape)
        {
            AlternativesContainer cont = (AlternativesContainer)view.Children.First(x => x is AlternativesContainer);

            foreach (AlternativeContainer alternativeContainer in cont.Children.Where(c => c is AlternativeContainer).Cast<AlternativeContainer>().ToList())
            {
                if (alternativeContainer.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0 && !alternativeContainer.Deleted) //check if this alternative contains the to be deleted component and is not already deleted
                {
                    alternativeContainer.Deleted = true;
                    alternativeContainer.RShape.Delete(); //delete the parent wrapper of s
                }
            }
        }
    }
}
