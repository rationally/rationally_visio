using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteForcesEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(string eventKey, RModel model, Shape forcesContainer)
        {
            Log.Debug("Entered DeleteForcesEventHandler.");    
            Globals.ThisAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(forcesContainer));
            if (!Globals.ThisAddIn.View.Children.Any(x => x is ForcesContainer))
            {
                model.Forces.Clear();
                Log.Debug("Cleared model forces list.");
                new RepaintHandler();
            }
        }
    }
}
