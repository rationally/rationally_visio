using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    internal class DeleteInformationBoxEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            Globals.ThisAddIn.View.Children.RemoveAll(obj => obj.RShape.Equals(changedShape));
        }
    }
}
