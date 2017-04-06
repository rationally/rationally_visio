using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.QueryDeleteEventHandlers
{
    internal class QDAlternativeComponentEventHandler : IQueryDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(RationallyView view, Shape changedShape)
        {
            AlternativesContainer cont = (AlternativesContainer)view.Children.First(x => x is AlternativesContainer);

            foreach (AlternativeContainer alternativeContainer in cont.Children.Where(c => c is AlternativeContainer).Cast<AlternativeContainer>().ToList())
            {
                if ((alternativeContainer.Children.Where(c => c.Shape.Equals(changedShape)).ToList().Count > 0) && !alternativeContainer.Deleted) //check if this alternative contains the to be deleted component and is not already deleted
                {
                    alternativeContainer.Deleted = true;
                    alternativeContainer.Shape.Delete(); //delete the parent wrapper of s
                }
            }
        }
    }
}
