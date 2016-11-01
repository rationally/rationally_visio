using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerDeleteAlternativeEventHandler : IMarkerEventHandler
    {
        public void Execute(RationallyModel model, Shape s, string context)
        {

            RationallyComponent component = new RationallyComponent(Globals.RationallyAddIn.Application.ActivePage) { RShape = s };

            int index = component.AlternativeIndex;
            Alternative alternative = model.Alternatives[index];
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete " + alternative.Title + "?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Shape shapeToPass;

                if (AlternativeContainer.IsAlternativeContainer(s.Name))
                {
                    shapeToPass = s;
                }
                else //subpart of alternative container
                {
                    //trace alternatives container
                    AlternativesContainer alternativesContainer = (AlternativesContainer) Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
                    //trace the correct alternative container
                    AlternativeContainer alternativeContainer = (AlternativeContainer) alternativesContainer.Children.First(c => c is AlternativeContainer && component.AlternativeIndex == c.AlternativeIndex);
                    
                    shapeToPass = alternativeContainer.RShape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
