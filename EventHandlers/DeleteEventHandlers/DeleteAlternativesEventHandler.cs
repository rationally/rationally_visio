using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Logger;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteAlternativesEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
            TempFileLogger.Log("Handler of delete alternatives entered.");
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is AlternativesContainer))
            {
                model.Alternatives.Clear();
                TempFileLogger.Log("model alternatives list emptied.");
                RepaintHandler.Repaint();
            }
        }
    }
}
