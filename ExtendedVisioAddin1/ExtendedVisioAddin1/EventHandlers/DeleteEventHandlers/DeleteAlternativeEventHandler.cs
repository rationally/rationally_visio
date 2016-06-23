
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
            //NOTE: this eventhandler is ment to be called while the changedShape is not completely deleted. Preferrable from ShapeDeleted eventhandler.

            //trace alternative container in view tree
            RComponent alternativeComponent = Globals.ThisAddIn.View.GetComponentByShape(changedShape);

            if (alternativeComponent is AlternativeContainer)
            {
                AlternativeContainer containerToDelete = (AlternativeContainer)alternativeComponent;
                AlternativeDescriptionComponent desc = containerToDelete.Children.Find(x => x is AlternativeDescriptionComponent) as AlternativeDescriptionComponent;
                List<Shape> shapes = new List<Shape>();
                foreach (int shapeIdentifier in desc.RShape.ContainerProperties.GetMemberShapes(0))
                {
                    Shape compShape = desc.Page.Shapes.ItemFromID[shapeIdentifier];
                    shapes.Add(compShape);
                    //compShape.Delete();
                }
                //desc.Deleted = true;
                //desc.RShape.DeleteEx(0);
                containerToDelete.Children.Where(c => !c.Deleted).ToList().ForEach(c => { c.Deleted = true; c.RShape.Delete(); });//schedule the missing delete events (children not selected during the manual delete)
                foreach (Shape s in shapes)
                {
                //    s.Delete();
                }

                AlternativesContainer alternativesContainer = (AlternativesContainer)Globals.ThisAddIn.View.Children.First(c => c is AlternativesContainer);
                //update model
                int index = containerToDelete.AlternativeIndex;
                model.Alternatives.RemoveAt(index);
                
                //update view tree
                alternativesContainer.Children.Remove(containerToDelete);

                model.RegenerateAlternativeIdentifiers();
                new RepaintHandler();//requires forces to repaint as well!
            }
        }
    }
}
