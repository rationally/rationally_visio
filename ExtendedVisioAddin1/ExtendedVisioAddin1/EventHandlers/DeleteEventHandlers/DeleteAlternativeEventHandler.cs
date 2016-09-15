using System.Linq;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.DeleteEventHandlers
{
    internal class DeleteAlternativeEventHandler : IDeleteEventHandler
    {
        public void Execute(string eventKey, RModel model, Shape changedShape)
        {
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (string.IsNullOrEmpty(Globals.ThisAddIn.LastDelete) && Globals.ThisAddIn.StartedUndoState == 0 && !Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                Globals.ThisAddIn.LastDelete = changedShape.Name;
                Globals.ThisAddIn.StartedUndoState = Globals.ThisAddIn.Application.BeginUndoScope("Delete alternative");  //TODO: Magic Number 
            }

            //trace alternative container in view tree
            RComponent alternativeComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            AlternativeContainer delete = alternativeComponent as AlternativeContainer;
            if (delete != null)
            {
                AlternativeContainer containerToDelete = delete;
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c =>
                    {
                        c.Deleted = true;
                        c.RShape.DeleteEx(0);  //TODO: Magic Number
                    }); //schedule the missing delete events (children not selected during the manual delete)
                }
                AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
                //update model
                model.Alternatives.RemoveAll(a => a.TimelessId == containerToDelete.TimelessId);

                //update view tree
                alternativesContainer.Children.Remove(containerToDelete);

                model.RegenerateAlternativeIdentifiers();
                if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
                {
                    alternativesContainer.MsvSdContainerLocked = true;
                }
                
                new RepaintHandler();//requires forces to repaint as well!  //TODO: Code smell. Should be RepaintHandler.requestRepaint() or repaint().   
            }
        }
    }
}
