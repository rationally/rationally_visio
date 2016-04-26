using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using Microsoft.Office.Interop.Visio;
using Shape = Microsoft.Office.Core.Shape;

namespace ExtendedVisioAddin1.Model
{
    internal class Alternative
    {

        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public Alternative(string title, string status, string description)
        {
            this.Title = title;
            this.Status = status;
            this.Description = description;
        }

        public void AddTo(IVShape alternatives, int alternativeIdentifier) 
        {
            
            Application application = Globals.ThisAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short) Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Document basicDocument = application.Documents.OpenEx("Basic Shapes.vss", (short) Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);

            Master alternativeMaster = containerDocument.Masters["Plain"];//wrapper for one whole alternative

            //--- define sub parts of the alternative. Only a Selection can be used to fill a container, so empty the current selection of the window and fill it with sub parts
            application.ActiveWindow.DeselectAll();

            //identifier
            string identifier = (char) (65 + alternativeIdentifier) + "";
            Master identifierRectangleMaster = basicDocument.Masters["Rectangle"];
            IVShape identifierRectangle = application.ActivePage.Drop(identifierRectangleMaster,0,0); //TODO check if can drop on a shape directly
            RationallyComponent identifierComponent = new RationallyComponent(identifierRectangle);
            //identifierComponent

            RationallyComponent altComponent = new RationallyComponent(alternatives);
            IVShape droppedAlternative = application.ActivePage.Drop(alternativeMaster, altComponent.CenterX, altComponent.CenterY);
            droppedAlternative.AddToContainers();

            containerDocument.Close();
        }
    }
}
