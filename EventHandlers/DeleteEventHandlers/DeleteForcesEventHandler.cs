using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Logger;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForcesEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape forcesContainer)
        {
            TempFileLogger.Log("Entered DeleteForcesEventHandler.");    
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(forcesContainer));
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is ForcesContainer))
            {
                model.Forces.Clear();
                TempFileLogger.Log("Cleared model forces list.");
                RepaintHandler.Repaint();
            }
        }
    }
}
