using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForcesEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape forcesContainer)
        {
            Log.Debug("Entered DeleteForcesEventHandler.");    
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.Shape.Equals(forcesContainer));
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is ForcesContainer))
            {
                model.Forces.Clear();
                Log.Debug("Cleared model forces list.");
                RepaintHandler.Repaint();
            }
        }
    }
}
