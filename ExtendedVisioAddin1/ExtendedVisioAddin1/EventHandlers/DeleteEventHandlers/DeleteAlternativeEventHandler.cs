
using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    internal class DeleteAlternativeEventHandler : DeleteEventHandler
    {
        public override void Execute(string eventKey, RModel model, Shape changedShape)
        {
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (String.IsNullOrEmpty(Globals.ThisAddIn.lastDelete) && Globals.ThisAddIn.StartedUndoState == 0)
            {
                Globals.ThisAddIn.lastDelete = changedShape.Name;
                Globals.ThisAddIn.StartedUndoState = Globals.ThisAddIn.Application.BeginUndoScope("scope");
            }
            //NOTE: this eventhandler is meant to be called while the changedShape is not completely deleted. Preferrable from ShapeDeleted eventhandler.

            //trace alternative container in view tree
            RComponent alternativeComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            if (alternativeComponent is AlternativeContainer)
            {
                AlternativeContainer containerToDelete = (AlternativeContainer)alternativeComponent;
                containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c => { c.Deleted = true; c.RShape.DeleteEx(0); });//schedule the missing delete events (children not selected during the manual delete)
                AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
                //update model
                int index = containerToDelete.AlternativeIndex;
                model.Alternatives.RemoveAt(index);
                
                //update view tree
                alternativesContainer.Children.Remove(containerToDelete);

                model.RegenerateAlternativeIdentifiers();

                alternativesContainer.MsvSdContainerLocked = true;
                
                new RepaintHandler();//requires forces to repaint as well!
            }
        }
    }
}
