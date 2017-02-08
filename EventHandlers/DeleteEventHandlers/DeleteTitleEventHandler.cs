using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteTitleEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Deleting title label.");
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
        }
    }
}
