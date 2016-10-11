using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteAlternativesEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
            Log.Debug("Handler of delete alternatives entered.");
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is AlternativesContainer))
            {
                model.Alternatives.Clear();
                Log.Debug("model alternatives list emptied.");
                new RepaintHandler();
            }
        }
    }
}
