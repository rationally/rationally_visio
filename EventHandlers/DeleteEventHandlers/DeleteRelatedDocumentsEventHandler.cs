using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteRelatedDocumentsEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            Log.Debug("Entered DeleteRelatedDocumentsEventHandler.");
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.Shape.Equals(changedShape));
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is RelatedDocumentsContainer))
            {
                Log.Debug("Deleting documents in document list in model.");
                model.Documents.Clear();
                RepaintHandler.Repaint();
            }
        }
    }
}
