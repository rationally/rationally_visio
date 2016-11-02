using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.ClickEventHandlers
{
    internal static class UpdateAlternativeHandler
    {
        public static void Execute(int alternativeIndex, string newTitle, string newState)
        {
            if (Globals.RationallyAddIn.Model.Alternatives.Count > 0)
            {
                Alternative alternative = Globals.RationallyAddIn.Model.Alternatives[alternativeIndex];
                alternative.Title = newTitle;
                alternative.Status = newState;
                RepaintHandler.Repaint(Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer));
            }
        }
    }
}
