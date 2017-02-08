using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteAlternativesEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
            Log.Debug("Handler of delete alternatives entered.");
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is AlternativesContainer))
            {
                model.Alternatives.Clear();
                Log.Debug("model alternatives list emptied.");
                RepaintHandler.Repaint();
            }
        }
    }
}
