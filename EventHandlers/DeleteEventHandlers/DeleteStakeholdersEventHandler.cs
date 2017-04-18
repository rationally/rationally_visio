using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteStakeholdersEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.Shape.Equals(changedShape));
            Log.Debug("Handler of delete stakeholders entered.");
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is StakeholdersContainer))
            {
                model.Stakeholders.Clear();
                Log.Debug("model stakeholders list emptied.");
                RepaintHandler.Repaint();
            }
        }
    }
}
