
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
            //NOTE: this eventhandler is ment to be called while the changedShape is not completely deleted. Preferrable from ShapeDeleted eventhandler.

            //trace alternative container in view tree
            RComponent alternativeComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            if (alternativeComponent is AlternativeContainer)
            {
                AlternativeContainer containerToDelete = (AlternativeContainer)alternativeComponent;
                containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c => { c.Deleted = true; c.RShape.Delete(); });//schedule the missing delete events (children not selected during the manual delete)

                AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
                //update model
                int index = containerToDelete.AlternativeIndex;
                model.Alternatives.RemoveAt(index);
                //update view tree
                alternativesContainer.Children.Remove(containerToDelete);
                new RepaintHandler();//requires forces to repaint as well!
            }
        }
    }
}
