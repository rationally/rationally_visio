using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.EventHandlers.MarkerEventHandlers
{
    internal class MarkerDeleteAlternativeEventHandler : IMarkerEventHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Execute(Shape s, string context)
        {
            RationallyModel model = Globals.RationallyAddIn.Model;
            VisioShape component = new VisioShape(Globals.RationallyAddIn.Application.ActivePage) { Shape = s };

            int index = component.Index;
            Alternative alternative = model.Alternatives[index];
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete " + alternative.Title + "?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Shape shapeToPass;

                if (AlternativeShape.IsAlternativeContainer(s.Name))
                {
                    shapeToPass = s;
                }
                else //subpart of alternative container
                {
                    //trace alternatives container
                    AlternativesContainer alternativesContainer = (AlternativesContainer) Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
                    //trace the correct alternative container
                    AlternativeShape alternativeShape = (AlternativeShape) alternativesContainer.Children.First(c => c is AlternativeShape && (component.Index == c.Index));
                    
                    shapeToPass = alternativeShape.Shape;
                }
                //initiate a delete handler with the container's shape
                shapeToPass.Delete();
            }
        }
    }
}
