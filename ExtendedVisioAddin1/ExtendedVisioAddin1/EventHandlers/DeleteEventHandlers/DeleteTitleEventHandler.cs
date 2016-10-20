using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using log4net;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteTitleEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            Log.Debug("Deleting title label.");
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
        }
    }
}
