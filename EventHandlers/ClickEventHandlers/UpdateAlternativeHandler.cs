using System.Linq;
using System.Reflection;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.ClickEventHandlers
{
    internal static class UpdateAlternativeHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
