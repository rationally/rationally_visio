using System.Linq;
using log4net;
using Rationally.Visio.Model;
using Rationally.Visio.View.Documents;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Logger;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteRelatedDocumentsEventHandler : IDeleteEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(RationallyModel model, Shape changedShape)
        {
            TempFileLogger.Log("Entered DeleteRelatedDocumentsEventHandler.");
            Globals.RationallyAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
            if (!Globals.RationallyAddIn.View.Children.Any(x => x is RelatedDocumentsContainer))
            {
                TempFileLogger.Log("Deleting documents in document list in model.");
                model.Documents.Clear();
                RepaintHandler.Repaint();
            }
        }
    }
}
