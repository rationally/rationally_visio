using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.ClickEventHandlers
{
    internal static class UpdateAlternativeHandler
    {
        public static void Execute(Alternative alternativeToUpdate, string newTitle, string newState)
        {
            alternativeToUpdate.Title = newTitle;
            alternativeToUpdate.Status = newState;
            RepaintHandler.Repaint(Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer));
        }
    }
}
