using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteInformationComponentEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Deleting information component.");
            InformationContainer cont = Globals.RationallyAddIn.View.Children.FirstOrDefault(obj => obj.RationallyType == "information") as InformationContainer;
            cont?.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
        }
    }
}
